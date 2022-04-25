using System;
using System.Collections;
using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
namespace ProductsInventory
{
     
    /// <summary>
    /// 
    /// </summary>
    public class IdentityServerConfig
    {
        /// <summary>
        /// API Name
        /// </summary>
        public const string ApiName = "productsInventory_api";
        /// <summary>
        /// API Friendly Name
        /// </summary>
        public const string ApiFriendlyName = "products Inventory api"; 
        /// <summary>
        /// Quick App ClientID
        /// </summary>
        public const string QuickAppClientID = "productsInventory";
        /// <summary>
        /// Swagger ClientID
        /// </summary>
        public const string SwaggerClientID = "swaggerui";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Phone(),
                new IdentityResources.Email(),
                new IdentityResource("roles", new List<string> { JwtClaimTypes.Role })
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // Api resources.
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource() 
                {
                    Name = ApiName,
                    Description = ApiFriendlyName,
                    Scopes = new List<string>(){ApiName },
                    UserClaims = {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.PhoneNumber,
                        JwtClaimTypes.Role,
                        ClaimConstants.Permission,
                        ClaimConstants.Subject
                    }
                }
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // Clients want to access resources.
        public static IEnumerable<Client> GetClients()
        {

            // Clients credentials.
            return new List<Client>
            {
                // http://docs.identityserver.io/en/release/reference/client.html.
                new Client
                {
                    ClientId = IdentityServerConfig.QuickAppClientID,
                    AllowedGrantTypes =   new string[] { "password"}, // Resource Owner Password Credential grant.
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false, // This client does not need a secret to request tokens from the token endpoint.
                    
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId, // For UserInfo endpoint.
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Email,
                        ScopeConstants.Roles,
                        ApiName
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                         "https://DukkanTek.com",
                        "http://DukkanTek.com",
                         "http://localhost:5000",
                        "https://localhost:5001",
                    },

                    AccessTokenLifetime = 5184000 ,
                    AllowOfflineAccess = true, // For refresh token.
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly
                     // Lifetime of access token in seconds.
                     
                    //AbsoluteRefreshTokenLifetime = 7200,
                    //SlidingRefreshTokenLifetime = 900,
                },

                new Client
                {
                    ClientId = SwaggerClientID,
                    ClientName = "Swagger UI",
                    AllowedGrantTypes = new string[] { "password"},//GrantTypes.ResourceOwnerPassword,
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false,
                    AllowedCorsOrigins = new List<string>
                    {
                        "https://DukkanTek.com",
                        "http://DukkanTek.com",
                        "http://localhost:5000",
                        "https://localhost:5001",
                    },
                    AllowedScopes = {
                        ApiName
                    }
                }
            };
        }

        
        /// <summary>
        /// Api Scopes.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope(ApiName, ApiFriendlyName) {
                    UserClaims = {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.PhoneNumber,
                        JwtClaimTypes.Role,
                        ClaimConstants.Permission
                    }
                }
            };
        }
    }
    /// <summary>
    /// Claim constants
    /// </summary>
    public static class ClaimConstants
    {
        ///<summary>A claim that specifies the subject of an entity</summary>
        public const string Subject = "sub";
        
        ///<summary>A claim that specifies the permission of an entity</summary>
        public const string Permission = "permission";
    }
    /// <summary>
    /// Scope constants
    /// </summary>
    public static class ScopeConstants
    {
        ///<summary>A scope that specifies the roles of an entity</summary>
        public const string Roles = "roles";
    }
}
