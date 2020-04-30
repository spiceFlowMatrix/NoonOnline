using Auth0.ManagementApi;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Training24Admin.Model
{
    public class General
    {
        //getting claims after decoding id_token 
        public static TokenClaims GetClaims(string headers)
        {
            TokenClaims obj = new TokenClaims();
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(headers.Split(" ")[1]) as JwtSecurityToken;
                var roles = tokenS.Claims.Where(claim => claim.Type == "https://noon-online/roles").ToList();
                List<string> rolelist = new List<string>();
                foreach (var role in roles)
                {
                    rolelist.Add(role.Value);
                }

                //var uid = tokenS.Claims.First(claim => claim.Type == "https://noon-online/uid").Value;                
                //obj.Id = uid;
                obj.sub = tokenS.Subject;
                obj.RoleName = rolelist;
                return obj;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return obj;
            }
        }


        //Uncomment this for live
        public static ManagementApiClient getAuthManagementApiToken()
        {
            string managementurl = Environment.GetEnvironmentVariable("MANAGEMENTURL_ENVIRONMENT");
            string client_id = Environment.GetEnvironmentVariable("CLIENT_ID_ENVIRONMENT");
            string client_secret = Environment.GetEnvironmentVariable("CLIENT_SECRET_ENVIRONMENT");
            string audience = Environment.GetEnvironmentVariable("AUDIENCE_ENVIRONMENT");
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string token = "";
            //get auth0 management api token
            var clientA0 = new RestClient(managementurl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"grant_type\":\"client_credentials\",\"client_id\": \"" + client_id + "\",\"client_secret\": \"" + client_secret + "\",\"audience\": \"" + audience + "\"}", ParameterType.RequestBody);
            IRestResponse response = clientA0.Execute(request);
            string strResponse = response.Content;
            var definition = new { access_token = "", token_type = "" };
            var resp = JsonConvert.DeserializeAnonymousType(strResponse, definition);
            token = resp.access_token;
            return new ManagementApiClient(token, new Uri(audience));
        }

        //Uncomment this for live
        public static async Task<ManagementApiClient> getAuthManagementApiTokenAsync()
        {
            string managementurl = Environment.GetEnvironmentVariable("MANAGEMENTURL_ENVIRONMENT");
            string client_id = Environment.GetEnvironmentVariable("CLIENT_ID_ENVIRONMENT");
            string client_secret = Environment.GetEnvironmentVariable("CLIENT_SECRET_ENVIRONMENT");
            string audience = Environment.GetEnvironmentVariable("AUDIENCE_ENVIRONMENT");
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string token = "";
            //get auth0 management api token
            var clientA0 = new RestClient(managementurl);
            var request = new RestRequest(Method.POST);
            var cancellationTokenSource = new CancellationTokenSource();
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"grant_type\":\"client_credentials\",\"client_id\": \"" + client_id + "\",\"client_secret\": \"" + client_secret + "\",\"audience\": \"" + audience + "\"}", ParameterType.RequestBody);
            IRestResponse response = await clientA0.ExecuteTaskAsync(request, cancellationTokenSource.Token);
            string strResponse = response.Content;
            var definition = new { access_token = "", token_type = "" };
            var resp = JsonConvert.DeserializeAnonymousType(strResponse, definition);
            token = resp.access_token;
            return new ManagementApiClient(token, new Uri(audience));
        }


        ////getting ManagementAPIClient authority for Auth0 implementation
        //public static ManagementApiClient getAuthManagementApiToken()
        //{
        //    string token = "";
        //    //get auth0 management api token
        //    var clientA0 = new RestClient("https://edgsolutions.eu.auth0.com/oauth/token");
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("content-type", "application/json");
        //    request.AddParameter("application/json", "{\"grant_type\":\"client_credentials\",\"client_id\": \"53ufLSqF7qvyqRaPb8DLCexw33PQp2qz\",\"client_secret\": \"Qs9ciXa7ijSM-aCE-If4g_TAA14pa0xA3-L5YzsEcplIDjshfYTePZvkVh1_wD2f\",\"audience\": \"https://edgsolutions.eu.auth0.com/api/v2/\"}", ParameterType.RequestBody);
        //    IRestResponse response = clientA0.Execute(request);
        //    string strResponse = response.Content;
        //    var definition = new { access_token = "", token_type = "" };
        //    var resp = JsonConvert.DeserializeAnonymousType(strResponse, definition);
        //    token = resp.access_token;
        //    return new ManagementApiClient(token, new Uri("https://edgsolutions.eu.auth0.com/api/v2/"));
        //}

        ////getting ManagementAPIClient authority for Auth0 implementation
        //public static async Task<ManagementApiClient> getAuthManagementApiTokenAsync()
        //{
        //    string token = "";
        //    //get auth0 management api token
        //    var clientA0 = new RestClient("https://edgsolutions.eu.auth0.com/oauth/token");
        //    var request = new RestRequest(Method.POST);
        //    var cancellationTokenSource = new CancellationTokenSource();
        //    request.AddHeader("content-type", "application/json");
        //    request.AddParameter("application/json", "{\"grant_type\":\"client_credentials\",\"client_id\": \"53ufLSqF7qvyqRaPb8DLCexw33PQp2qz\",\"client_secret\": \"Qs9ciXa7ijSM-aCE-If4g_TAA14pa0xA3-L5YzsEcplIDjshfYTePZvkVh1_wD2f\",\"audience\": \"https://edgsolutions.eu.auth0.com/api/v2/\"}", ParameterType.RequestBody);
        //    IRestResponse response = await clientA0.ExecuteTaskAsync(request, cancellationTokenSource.Token);
        //    string strResponse = response.Content;
        //    var definition = new { access_token = "", token_type = "" };
        //    var resp = JsonConvert.DeserializeAnonymousType(strResponse, definition);
        //    token = resp.access_token;
        //    return new ManagementApiClient(token, new Uri("https://edgsolutions.eu.auth0.com/api/v2/"));
        //}



        //// this is for local
        //public static ManagementApiClient getAuthManagementApiToken()
        //{
        //    string token = "";
        //    //get auth0 management api token
        //    var clientA0 = new RestClient("https://satyamdev.auth0.com/oauth/token");
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("content-type", "application/json");
        //    request.AddParameter("application/json", "{\"grant_type\":\"client_credentials\",\"client_id\": \"3aM51DshJGurH3sp41VMtBC5EH9gOEkD\",\"client_secret\": \"tYdUEQLusO83YWRkWCKY1qWtLl7nwFAw-1VOM8Z39UOEBHYOZ11EvI1XZDa-84zy\",\"audience\": \"https://satyamdev.auth0.com/api/v2/\"}", ParameterType.RequestBody);
        //    IRestResponse response = clientA0.Execute(request);
        //    string strResponse = response.Content;
        //    var definition = new { access_token = "", token_type = "" };
        //    var resp = JsonConvert.DeserializeAnonymousType(strResponse, definition);
        //    token = resp.access_token;
        //    return new ManagementApiClient(token, new Uri("https://satyamdev.auth0.com/api/v2"));
        //}

        //// this is for local
        //public static async Task<ManagementApiClient> getAuthManagementApiTokenAsync()
        //{
        //    string token = "";
        //    //get auth0 management api token
        //    var clientA0 = new RestClient("https://satyamdev.auth0.com/oauth/token");
        //    var request = new RestRequest(Method.POST);
        //    var cancellationTokenSource = new CancellationTokenSource();
        //    request.AddHeader("content-type", "application/json");
        //    request.AddParameter("application/json", "{\"grant_type\":\"client_credentials\",\"client_id\": \"3aM51DshJGurH3sp41VMtBC5EH9gOEkD\",\"client_secret\": \"tYdUEQLusO83YWRkWCKY1qWtLl7nwFAw-1VOM8Z39UOEBHYOZ11EvI1XZDa-84zy\",\"audience\": \"https://satyamdev.auth0.com/api/v2/\"}", ParameterType.RequestBody);
        //    IRestResponse response = await clientA0.ExecuteTaskAsync(request, cancellationTokenSource.Token);
        //    string strResponse = response.Content;
        //    var definition = new { access_token = "", token_type = "" };
        //    var resp = JsonConvert.DeserializeAnonymousType(strResponse, definition);
        //    token = resp.access_token;
        //    return new ManagementApiClient(token, new Uri("https://satyamdev.auth0.com/api/v2"));
        //}

        public static string getRoleType(string roleid)
        {
            switch (roleid)
            {
                case "1":
                    return "admin";
                case "2":
                    return "analyst";
                case "3":
                    return "teacher";
                case "4":
                    return "student";
                case "5":
                    return "customer";
                case "6":
                    return "aafmanager";
                case "7":
                    return "coordinator";
                case "8":
                    return "edit_team_leader";
                case "9":
                    return "shooting_team_leader";
                case "10":
                    return "graphics_team_leader";
                case "11":
                    return "quality_assurance";
                case "12":
                    return "feedback_edge_team";
                case "13":
                    return "sales_admin";
                case "14":
                    return "filming_staff";
                case "15":
                    return "editing_staff";
                case "16":
                    return "graphics_staff";
                case "17":
                    return "sales_agent";
                case "18":
                    return "parent";
                default:
                    break;
            };
            return "";
        }

        public static string getBucketName(string fileTypeId)
        {
            string bucketName = "";
            if (fileTypeId == "1")
            {
                bucketName = "t24-primary-pdf-storage";
            }

            if (fileTypeId == "2")
            {
                bucketName = "t24-primary-video-storage";
            }

            if (fileTypeId == "3")
            {
                bucketName = "t24-primary-image-storage";
            }

            if (fileTypeId == "4")
            {
                bucketName = "t24-primary-image-storage";
            }

            if (fileTypeId == "6" || fileTypeId == "7" || fileTypeId == "8")
            {
                bucketName = "t24-primary-pdf-storage";
            }
            return bucketName;
        }

        public static string getBucketName(long fileTypeId)
        {
            string bucketName = "";

            switch (fileTypeId)
            {
                case 1:
                    bucketName = "t24-primary-pdf-storage";
                    break;
                case 2:
                    bucketName = "t24-primary-video-storage";
                    break;
                case 3:
                    bucketName = "t24-primary-image-storage";
                    break;
                case 6:
                    bucketName = "t24-primary-pdf-storage";
                    break;
                case 7:
                    bucketName = "t24-primary-pdf-storage";
                    break;
                case 8:
                    bucketName = "t24-primary-pdf-storage";
                    break;
            }
            return bucketName;
        }
    }

    public class MyData
    {
        //public Authorization authorization { get; set; }
        public List<string> roles { get; set; }
        //public string username { get; set; }
        //public string uid { get; set; }
    }

    public class Authorization
    {
        public List<string> roles { get; set; }
    }

    public static class ClaimType
    {
        public const string Id = "Id";
        public const string RoleId = "RoleId";
        public const string Email = "Email";
    }

    public static class DeviceType
    {
        public const string Android = "android";
        public const string IOS = "ios";
    }

    public class TokenClaims
    {
        public string sub { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public List<string> RoleName { get; set; }
        public List<string> RoleId { get; set; }
    }

    public enum RoleType
    {
        Admin = 1,
        Teacher = 2,
        Student = 3,
        Parent = 4
    }
}
