using HellsGate.Extension;
using HellsGate.Models;
using HellsGate.Models.Context;
using HellsGate.Models.DatabaseModel;
using HellsGate.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HellsGate.Services
{
    public class AccessManagerService : IAccessManagerService
    {
        private readonly IAutorizationManagerService _autorizationManagerService;
        private readonly IConfiguration _configuration;
        private readonly HellsGateContext _context;
        private readonly ISecurLibService _securLib;

        public AccessManagerService(ISecurLibService securLib,
            IConfiguration configuration, IAutorizationManagerService autorizationManagerService, HellsGateContext context)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _securLib = securLib ?? throw new ArgumentNullException(nameof(securLib));
            _autorizationManagerService = autorizationManagerService ?? throw new ArgumentNullException(nameof(autorizationManagerService));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> Access(AccessModel newAccess, WellknownAuthorizationLevel AccessType)
        {
            newAccess.GrantedAccess = false;
            if (newAccess.PeopleEntered.Equals(Guid.Empty) || !string.IsNullOrEmpty(newAccess.Plate) || !string.IsNullOrEmpty(newAccess.CardNumber))
            {
                try
                {
                    PeopleAnagraphicModel owner = new PeopleAnagraphicModel();
                    if (newAccess.PeopleEntered.Equals(Guid.Empty) && !string.IsNullOrEmpty(newAccess.Plate))
                    {
                        if (await _context.Cars.AnyAsync(c => c.LicencePlate == newAccess.Plate))
                        {
                            var entered = await _context.Cars.FirstAsync(c => c.LicencePlate == newAccess.Plate).ConfigureAwait(false);
                            owner = entered.Owner;
                        }
                    }
                    else if (await _context.Peoples.AnyAsync(c => c.CardNumber.CardNumber == newAccess.CardNumber).ConfigureAwait(false))
                    {
                        owner = await _context.Peoples.FirstAsync(a => a.CardNumber.CardNumber == newAccess.CardNumber).ConfigureAwait(false);
                    }
                    if (await _autorizationManagerService.IsAutorized(owner.Id, AccessType).ConfigureAwait(false))
                    {
                        newAccess.PeopleEntered = owner.Id;
                        newAccess.GrantedAccess = true;
                    }
                    await _context.Access.AddAsync(newAccess).ConfigureAwait(false);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    //StaticEventHandler.SendMail(new MailEventArgs(ResourceString.AccessCarMailSubject, ResourceString.AccessCarMailBody, DateTime.UtcNow));
                }
                catch (Exception ex)
                {
                    StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during Card verification", MethodBase.GetCurrentMethod(), ex);
                    return false;
                }
            }
            return newAccess.GrantedAccess;
        }

        public async Task<Guid> GetUserByInputAsync(string UserInput)
        {
            try
            {
                var user = new PeopleAnagraphicModel();
                if (string.IsNullOrEmpty(UserInput) || string.IsNullOrEmpty(UserInput.Trim()))
                { return Guid.Empty; }
                string toFind = UserInput.Trim();

                if (toFind.IsValidEmail() && await _context.Peoples.AnyAsync(p => String.Equals(p.Email, UserInput, StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false))
                {
                    user = await _context.Peoples.FirstOrDefaultAsync(p => String.Equals(p.Email, UserInput, StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false);
                }
                else if (!toFind.IsValidEmail() && await _context.Peoples.AnyAsync(p => String.Equals(p.UserName, UserInput, StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false))
                {
                    user = await _context.Peoples.FirstOrDefaultAsync(p => String.Equals(p.UserName, UserInput, StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false);
                }
                return user.Id;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during Login", MethodBase.GetCurrentMethod(), ex);
                throw new System.Exception();
            }
        }

        public async Task<SignInResult> ValidateLoginAsync(string username, string password, bool rememberMe, bool shouldLockout)
        {
            try
            {
                var userId = await GetUserByInputAsync(username).ConfigureAwait(false);
                if (userId.Equals(Guid.Empty)) { return SignInResult.Failed; }
                if (await _context.Peoples.AnyAsync(p => p.Id == userId).ConfigureAwait(false))
                {
                    PeopleAnagraphicModel userSelected = await _context.Peoples.Include(Auth => Auth.AutorizationLevel).Include(Card => Card.CardNumber).FirstOrDefaultAsync(p => p.Id == userId).ConfigureAwait(false);
                    var encrypted = await _securLib.EncriptLine(password);
                    if (await _securLib.CompareHash(userSelected.Password, encrypted))
                    {
                        // authentication successful so generate jwt token
                        var tokenHandler = new JwtSecurityTokenHandler();

                        var key = Encoding.ASCII.GetBytes(_configuration["PreSharedKey"]);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                                    new Claim(ClaimTypes.Name, userSelected.Id.ToString())
                            }),
                            Expires = DateTime.UtcNow.AddDays(7),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var secureToken = tokenHandler.WriteToken(token);
                        return SignInResult.Success;
                    }
                    else
                    {
                        return SignInResult.Failed;
                    }
                }
                return SignInResult.Failed;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during Login", MethodBase.GetCurrentMethod(), ex);
                return SignInResult.Failed;
            }
        }
    }
}