using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace WebAPIApplication.Controllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private static string accessToken;

        [HttpGet]
        [Route("public")]
        public async Task<IActionResult> PublicAsync()
        {
            ClientCredentialsFlow();
            await GetUsers();            
            var client = new RestClient("https://satyamdev.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"client_id\":\"KTpcQ3FHmQMlNPGVa0pP1davk8XTp9zx\",\"client_secret\":\"o83mxra0fPHtNvLcpielBpMDB8girlhjttbnQcyCY687QvKLEsS92ukkRrFGEgsW\",\"audience\":\"https://satyamdev.auth0.com/api/v2/users\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return Json(new
            {
                Message = "Hello from a public endpoint! You don't need to be authenticated to see this."
            });
        }

        protected static async Task GetUsers()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync("https://satyamdev.auth0.com/api/v2/users");
                var responseBody = await response.Content.ReadAsStringAsync();
            }
        }

        protected static void ClientCredentialsFlow()
        {
            var body = new Model
            {
                grant_type = "client_credentials",
                client_id = "3aM51DshJGurH3sp41VMtBC5EH9gOEkD",
                client_secret = "tYdUEQLusO83YWRkWCKY1qWtLl7nwFAw-1VOM8Z39UOEBHYOZ11EvI1XZDa-84zy",
                audience = "https://satyamdev.auth0.com/oauth/token"
            };

            var client = new RestClient("https://satyamdev.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"client_id\":\"3aM51DshJGurH3sp41VMtBC5EH9gOEkD\",\"client_secret\":\"tYdUEQLusO83YWRkWCKY1qWtLl7nwFAw-1VOM8Z39UOEBHYOZ11EvI1XZDa-84zy\",\"audience\":\"https://satyamdev.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var deserilizeBody = JsonConvert.DeserializeObject<AuthResponseModel>(response.Content.ToString());
            accessToken = deserilizeBody.access_token;            
        }


        internal class Model
        {
            public string grant_type { get; set; }
            public string client_id { get; set; }
            public string client_secret { get; set; }
            public string audience { get; set; }
        }

        internal class AuthResponseModel
        {
            public string access_token { get; set; }
            public string scopes { get; set; }
            public string expires_in { get; set; }
            public string token_type { get; set; }
        }

        internal class User1
        {
            public string email { get; set; }
            public bool email_verified { get; set; }
            public string connection { get; set; }
            public string username { get; set; }
            public string password { get; set; }

        }

        [HttpGet]
        [Route("private")]
        [Authorize]
        public IActionResult Private()
        {
            return Json(new
            {
                Message = "Hello from a private endpoint! You need to be authenticated to see this."
            });
        }

        [HttpGet]
        [Route("private-scoped")]
        [Authorize("read:messages")]
        public IActionResult Scoped()
        {
            return Json(new
            {
                Message = "Hello from a private endpoint! You need to be authenticated and have a scope of read:messages to see this."
            });
        }


        /// <summary>
        /// This is a helper action. It allows you to easily view all the claims of the token
        /// </summary>
        /// <returns></returns>
        [HttpGet("claims")]
        public IActionResult Claims()
        {
            return Json(User.Claims.Select(c =>
                new
                {
                    c.Type,
                    c.Value
                }));
        }
    }
}
