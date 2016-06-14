using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Configuration;
using System.Globalization;

using OSIsoft.PICS.API;
using OSIsoft.PICS.Common;

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using OSIsoft.PICS.Security.Authorization;

using OSIsoft.Qi;
using OSIsoft.Qi.Http;
using System.Net.Http.Headers;

namespace FakeIngressAPI.Controllers
{
    /// <summary>
    /// Extends the HttpRequestMessage collection
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Returns a dictionary of QueryStrings that's easier to work with 
        /// than GetQueryNameValuePairs KevValuePairs collection.
        /// 
        /// If you need to pull a few single values use GetQueryString instead.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetQueryStrings(
            this HttpRequestMessage request)
        {
            return request.GetQueryNameValuePairs()
                          .ToDictionary(kv => kv.Key, kv => kv.Value,
                               StringComparer.OrdinalIgnoreCase);
        }
    }

    public class Device
    {
        public string DeviceName
        {
            get;set;
        }

        public string SASToken
        {
            get; set;
        }
    }
    

    [Authorize]
    public class fakeIngressServiceController : ApiController
    {
        [HttpGet]
        // GET: api/DeviceList
        [AuthorizePICS(GroupNames = GroupTypes.Administrators)]
        public async Task<IHttpActionResult> DeviceList()
        {
            // Access the System Service.
            // await AccessSystemService();
            // string owner = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

            Dictionary<string, string> queryParams = ControllerContext.Request.GetQueryStrings();

            //string TenantId = queryParams["tenant"];
            //string NamespaceId = queryParams["namespace"];

            List<Device> Devices = new List<Device>();

            Devices.Add(new Device
            {
                DeviceName = "Device1",
                SASToken = "SASDevice1"
            });

            Devices.Add(new Device
            {
                DeviceName = "Device2",
                SASToken = "SASDevice2"
            });

            Devices.Add(new Device
            {
                DeviceName = "Device3",
                SASToken = "SASDevice3"
            });


            return Json(Devices);
        }

              
        //private async Task AccessSystemService()
        //{
        //    try
        //    {
        //        SystemClient systemClient = new SystemClient(ConfigurationManager.AppSettings["SubscriptionTarget"],
        //            ConfigurationManager.AppSettings["ClientApplicationClientID"],
        //            ConfigurationManager.AppSettings["ClientApplicationClientKey"].ConvertToSecureString());

        //        bool hasGroup = await systemClient.Users.HasGroupClaimAsync(ClaimsPrincipal.Current.Identity as ClaimsIdentity, "PICS Administrators");

        //        bool tenantExists = await systemClient.Tenants.ExistsAsync(ConfigurationManager.AppSettings["ida:Tenant"]);

        //        // UserClient(string target, string clientId, SecureString clientKey, bool delegateCall, bool cacheEnabled = true)
        //        UserClient userClient = new UserClient(ConfigurationManager.AppSettings["SubscriptionTarget"],
        //            ConfigurationManager.AppSettings["ClientApplicationClientID"],
        //            ConfigurationManager.AppSettings["ClientApplicationClientKey"].ConvertToSecureString(), true, true);

        //        IEnumerable<OSIsoft.PICS.API.Models.Group> groups = await userClient.Groups.GetAllAsync();

        //        await TestDelegation();
        //    }
        //    catch (Exception e)
        //    {
        //    }
        //}

        private async Task TestDelegation()
        {
            var bootstrapContext = ClaimsPrincipal.Current.Identities.First().BootstrapContext as global::System.IdentityModel.Tokens.BootstrapContext;

            ClientCredential clientCredential = new ClientCredential(ConfigurationManager.AppSettings["ClientApplicationClientID"],
                    ConfigurationManager.AppSettings["ClientApplicationClientKey"].ConvertToSecureString());


            string userName = ClaimsPrincipal.Current.FindFirst(ClaimTypes.Upn) != null ? ClaimsPrincipal.Current.FindFirst(ClaimTypes.Upn).Value : ClaimsPrincipal.Current.FindFirst(ClaimTypes.Email).Value;

            string tenantId = ClaimsPrincipal.Current.Claims.SingleOrDefault(c => c.Type == ClaimTypesEx.TenantIdentifier).Value;

            UserAssertion userAssertion = new UserAssertion(bootstrapContext.Token, "urn:ietf:params:oauth:grant-type:jwt-bearer", userName);

            AuthenticationContext authenticationContext = new AuthenticationContext(string.Format(CultureInfo.InvariantCulture, "https://login.microsoftonline.com/{0}", tenantId));

            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenAsync("https://pihomedev.onmicrosoft.com/picssystemservice", clientCredential, userAssertion);
        }
    }
}
