using System;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.ActiveDirectory;
using System.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens;
using OSIsoft.PICS.Security.Authorization;
using OSIsoft.PICS.Common;

namespace FakeIngressAPI
{
    public partial class Startup
    {
        private const string PublicPrivateKeyXMLFileName = "PublicPrivateKey.xml";

        private string PathToCurrentDirectory
        {
            get
            {
                return Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            }
        }

        /// <summary>
        /// Path to the signing public-private key file.
        /// This public/private key is used to secure the cookie that travels with web api calls between an ADALJS enabled 
        /// web client and the service provider using PICS Bearer Authentication    
        /// </summary>
        private string PathToPublicPrivateKeyXMLFile
        {
            get
            {
                return PathToCurrentDirectory.EndsWith(@"\") ? $"{PathToCurrentDirectory}Keys\\{PublicPrivateKeyXMLFileName}" : $"{PathToCurrentDirectory}\\Keys\\{PublicPrivateKeyXMLFileName}";
            }
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    Audience = ConfigurationManager.AppSettings["ida:Audience"],
                    Tenant = ConfigurationManager.AppSettings["ida:Tenant"],
                    TokenValidationParameters = new TokenValidationParameters() { ValidateIssuer = false, SaveSigninToken = true }
                });


            // Add the PICSBearerAuthentication middleware.
            PICSBearerAuthenticationOptions piCSBearerAuthenticationOptions = new PICSBearerAuthenticationOptions()
            {
                Target = ConfigurationManager.AppSettings["SubscriptionTarget"],
                ClientApplicationClientId = ConfigurationManager.AppSettings["ClientApplicationClientID"],
                ClientApplicationClientKey = ConfigurationManager.AppSettings["ClientApplicationClientKey"].ConvertToSecureString(),
                ForwardUnauthenticatedRequestsToApplication = true,
                // This public/private key is used to secure the cookie that travels with web api calls between an ADALJS enabled 
                // web client and the service provider using PICS Bearer Authentication    
                GroupClaimsPublicPrivateKey = File.ReadAllText(PathToPublicPrivateKeyXMLFile).ConvertToSecureString()
            };

            app.UsePICSBearerAuthentication(piCSBearerAuthenticationOptions);

        }

    }
}

