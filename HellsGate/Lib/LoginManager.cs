using System;
using System.Reflection;
using System.Threading.Tasks;
using HellsGate.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HellsGate.Lib
{
    public class LoginManager<TUser> : SignInManager<PeopleAnagraphicModel> where TUser : class
    {

        private readonly UserManager<PeopleAnagraphicModel> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public LoginManager(
            UserManager<PeopleAnagraphicModel> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<PeopleAnagraphicModel> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<PeopleAnagraphicModel>> logger,
            IAuthenticationSchemeProvider schemeProvider,
            IUserConfirmation<PeopleAnagraphicModel> confirmation
            )
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemeProvider, confirmation)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
        }

        public async Task<int> GetUserByInputAsync(string UserInput)
        {
            try
            {
                var user = new PeopleAnagraphicModel();
                if (string.IsNullOrEmpty(UserInput) || string.IsNullOrEmpty(UserInput.Trim()))
                { return user.Id; }
                string toFind = UserInput.Trim();
                using (var c = new Context())
                {
                    if (IsValidEmail(toFind) && await c.Peoples.AnyAsync(p => p.Email == UserInput).ConfigureAwait(false))
                    {
                        user = await c.Peoples.FirstOrDefaultAsync(p => p.Email == UserInput).ConfigureAwait(false);

                    }
                    else if (!IsValidEmail(toFind) && await c.Peoples.AnyAsync(p => p.UserName == UserInput).ConfigureAwait(false))
                    {
                        user = await c.Peoples.FirstOrDefaultAsync(p => p.UserName == UserInput).ConfigureAwait(false);

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

        public override async Task<SignInResult> PasswordSignInAsync(string userName, string password, bool rememberMe, bool shouldLockout)
        {
            try
            {
                using (var c = new Context())
                {
                    if (await c.Peoples.AnyAsync(p => p.UserName == userName).ConfigureAwait(false))
                    {
                        PeopleAnagraphicModel userSelected = await c.Peoples.FirstOrDefaultAsync(p => p.UserName == userName).ConfigureAwait(false);
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


    }
}
