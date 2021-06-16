using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Training24Admin.Controllers
{
    [Route("")]
    public class ValuesController : Controller
    {
        [HttpGet]
        public async Task<string> Get ()
        {
            return "value";
        }
    }
}
