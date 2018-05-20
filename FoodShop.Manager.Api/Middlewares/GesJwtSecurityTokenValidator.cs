using FoodShop.Manager.Common;
using FoodShop.Manager.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace FoodShop.Manager.Api.Middlewares
{
    /// <summary>
    /// Validate token
    /// </summary>
    public class GesJwtSecurityTokenValidator : ISecurityTokenValidator
    {
        private readonly ILogger _logger;
        private int _maxTokenSizeInBytes = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly IUserRepositories _userService;
        private string _appendRole;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userService">service to get user info (include region)</param>
        /// <param name="defaultRole">default role to ensure (active in DEBUG mode only)</param>
        public GesJwtSecurityTokenValidator(ILogger<GesJwtSecurityTokenValidator> logger, IUserRepositories userService, string defaultRole = null)
        {
            _userService = userService;
            _appendRole = defaultRole;
            _tokenHandler = new JwtSecurityTokenHandler();
            _logger = logger;
        }

        /// <summary>
        ///  Returns true if a token can be validated.
        /// </summary>
        public bool CanValidateToken
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets and sets the maximum size in bytes, that a will be processed.
        /// </summary>
        public int MaximumTokenSizeInBytes
        {
            get
            {
                return _maxTokenSizeInBytes;
            }

            set
            {
                _maxTokenSizeInBytes = value;
            }
        }

        /// <summary>
        /// Returns true if the token can be read, false otherwise.
        /// </summary>
        /// <param name="securityToken"></param>
        /// <returns></returns>
        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        /// <summary>
        /// Validates a token passed as a string using Microsoft.IdentityModel.Tokens.TokenValidationParameters
        /// </summary>
        /// <param name="securityToken">Bearer token</param>
        /// <param name="validationParameters"></param>
        /// <param name="validatedToken"></param>
        /// <returns></returns>
        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            try
            {
                ClaimsPrincipal principal = _tokenHandler.ValidateToken(securityToken, validationParameters, out validatedToken);
                if (principal.Identity is ClaimsIdentity)
                {
                    ClaimsIdentity identity = principal.Identity as ClaimsIdentity;
                    if (identity.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Name, identity.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value));
                    }

                    var emailClaim = identity.FindFirst(ClaimTypes.Email);
                    var user = _userService.GetUser(int.Parse(emailClaim?.Value));
                    if (user == null)
                    {
                        throw new Exception("User not found.");
                    }

                    identity.AddClaim(new Claim(WsConstants.UserIdClaim, user.Id.ToString()));
                    GetUserPermissions(identity, user.UserName);
                }

                return principal;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Exception occured in parse token");
                throw;
            }
        }

        private List<string> GetOriginalRoles(ClaimsIdentity identity)
        {
            var roles = new List<string>();
            if (identity.HasClaim(c => c.Type == "permissions"))
            {
                Claim claim = identity.Claims.FirstOrDefault(c => c.Type == "permissions");
                if (claim != null)
                {
                    roles.AddRange(claim.Value.Split(','));
                }
            }

            //Mock roles for test convenience
            if (!string.IsNullOrWhiteSpace(_appendRole))
            {
                roles.AddRange(_appendRole.Split(','));
            }

            return roles;
        }

        private void GetUserPermissions(ClaimsIdentity identity, string userRegion)
        {
            var roles = GetOriginalRoles(identity);

            if (roles.Any(role => role.Equals(WsConstants.GlobalAdmin, StringComparison.OrdinalIgnoreCase)))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            }
            else if (roles.Any(role => role.Equals(WsConstants.Employee, StringComparison.OrdinalIgnoreCase)))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "Employee"));
            }
            else
            {
                //no permission
                //throw new Exception("Forbidden: Please request appropriate permissions from MyAccess before you continue.");
            }
        }
    }
}
