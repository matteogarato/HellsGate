using HellsGate.Lib.Interfaces;
using HellsGate.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace HellsGate.Lib
{
    public class LoginManager<TUser> : SignInManager<TUser> where TUser : PeopleAnagraphicModel
    {
        private readonly UserManager<TUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ISecurLib SecurLib;

        public LoginManager(
            UserManager<TUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<TUser>> logger,
            IAuthenticationSchemeProvider schemeProvider,
            IUserConfirmation<TUser> confirmation
            )
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemeProvider, confirmation)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public async Task<string> GetUserByInputAsync(string UserInput)
        {
            try
            {
                var user = new PeopleAnagraphicModel();
                if (string.IsNullOrEmpty(UserInput) || string.IsNullOrEmpty(UserInput.Trim()))
                { return string.Empty; }
                string toFind = UserInput.Trim();
                using (var c = new HellsGateContext())
                {
                    if (IsValidEmail(toFind) && await c.Peoples.AnyAsync(p => String.Equals(p.Email, UserInput, StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false))
                    {
                        user = await c.Peoples.FirstOrDefaultAsync(p => String.Equals(p.Email, UserInput, StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false);
                    }
                    else if (!IsValidEmail(toFind) && await c.Peoples.AnyAsync(p => String.Equals(p.UserName, UserInput, StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false))
                    {
                        user = await c.Peoples.FirstOrDefaultAsync(p => String.Equals(p.UserName, UserInput, StringComparison.OrdinalIgnoreCase)).ConfigureAwait(false);
                    }
                    return user.Id;
                }
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during Login", MethodBase.GetCurrentMethod(), ex);
                throw new System.Exception();
            }
        }

        public override async Task<SignInResult> PasswordSignInAsync(string username, string password, bool rememberMe, bool shouldLockout)
        {
            try
            {
                using (var c = new HellsGateContext())
                {
                    var userId = await GetUserByInputAsync(username).ConfigureAwait(false);
                    if (string.IsNullOrEmpty(userId)) { return SignInResult.Failed; }
                    if (await c.Peoples.AnyAsync(p => p.Id == userId).ConfigureAwait(false))
                    {
                        PeopleAnagraphicModel userSelected = await c.Peoples.FirstOrDefaultAsync(p => p.UserName == userId).ConfigureAwait(false);
                        if (await SecurLib.CompareHashAsync(
                                    Convert.FromBase64String(userSelected.Password)
                                    , await SecurLib.EncriptLineAsync(password).ConfigureAwait(false)).ConfigureAwait(false))
                        {
                            return SignInResult.Success;
                        }
                        else
                        {
                            return SignInResult.Failed;
                        }
                    }
                    return SignInResult.Failed;
                }
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during Login", MethodBase.GetCurrentMethod(), ex);
                return SignInResult.Failed;
            }
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "not a valid email", MethodBase.GetCurrentMethod(), ex);
                return false;
            }
        }
    }
}