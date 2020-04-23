using HellsGate.Models.DatabaseModel;
using HellsGate.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace HellsGate.Services
{
    public class LoginManagerService<TUser> : SignInManager<TUser> where TUser : PeopleAnagraphicModel
    {
        private readonly IAccessManagerService _accessManagerService;

        public LoginManagerService(
            UserManager<TUser> userManager,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<TUser>> logger,
            IAuthenticationSchemeProvider schemeProvider,
            IUserConfirmation<TUser> confirmation,
            IAccessManagerService accessManagerService
            )
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemeProvider, confirmation)
        {
            _accessManagerService = accessManagerService ?? throw new ArgumentNullException(nameof(accessManagerService));
        }

        public async Task<string> GetUserByInputAsync(string UserInput)
        {
            try
            {
                return await _accessManagerService.GetUserByInputAsync(UserInput);
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during Login", MethodBase.GetCurrentMethod(), ex);
                throw new System.Exception();
            }
        }

        public override async Task<SignInResult> PasswordSignInAsync(string username, string password, bool isPersistent, bool lockoutOnFailure)
        {
            try
            {
                return await _accessManagerService.ValidateLoginAsync(username, password, isPersistent, lockoutOnFailure);
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during Login", MethodBase.GetCurrentMethod(), ex);
                return SignInResult.Failed;
            }
        }

        public override async Task<SignInResult> PasswordSignInAsync(TUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            try
            {
                return await _accessManagerService.ValidateLoginAsync(user.UserName, password, isPersistent, lockoutOnFailure);
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during Login", MethodBase.GetCurrentMethod(), ex);
                return SignInResult.Failed;
            }
        }
    }
}