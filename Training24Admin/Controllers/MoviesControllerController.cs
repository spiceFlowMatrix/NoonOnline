using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Training24Admin.Controllers
{
    [Authorize(Policy = "Member")]
    [Route("movies")]
    public class MoviesController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            //return Content("List of Movies");

            var dict = new Dictionary<string, string>();
            HttpContext.User.Claims.ToList().ForEach(item => dict.Add(item.Type, item.Value));

            return Ok(dict);
        }
    }
}