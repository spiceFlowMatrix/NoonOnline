using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trainning24.BL.ViewModels;

namespace Training24Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = nameof(SwaggerGrouping.Device))]
    public class DeviceController : ControllerBase
    {
        [HttpGet()]
        public IActionResult Get()
        {
            return StatusCode(406, ModelState);
        }

        [HttpGet("{userId}")]
        public IActionResult Get(int userId)
        {
            return StatusCode(406, ModelState);
        }

        [HttpPost("{userId}")]
        public IActionResult Post(int userId)
        {
            return StatusCode(406, ModelState);
        }

        [HttpPut("{userId}")]
        public IActionResult Put(int userId, int deviceId)
        {
            return StatusCode(406, ModelState);
        }

        [HttpDelete("{userId}")]
        public IActionResult Delte(int userId)
        {
            return StatusCode(406, ModelState);
        }
    }
}