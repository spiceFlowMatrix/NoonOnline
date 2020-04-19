using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.Device;

namespace Training24Admin.Controllers
{
    /// <summary>
    /// Everything about your devices
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = nameof(SwaggerGrouping.Device))]
    public class DeviceController : ControllerBase
    {
        /// <summary>
        /// Fetch a list of all devices that have activated (or registered) the student app on it
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public IActionResult Get()
        {
            return StatusCode(406, ModelState);
        }

        /// <summary>
        /// Get my device profile
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns></returns>
        [HttpGet("{userId}")]
        public IActionResult Get(int userId)
        {
            return StatusCode(406, ModelState);
        }

        /// <summary>
        /// Register device quota for user and activate new device
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="objData">New device that I want to activate</param>
        /// <returns></returns>
        [HttpPost("{userId}")]
        public IActionResult Post(int userId, DeviceActivate objData)
        {
            return StatusCode(406, ModelState);
        }

        /// <summary>
        /// Reactivate an existing device that has been deactivated
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="deviceId">Device id</param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public IActionResult Put(int userId, int deviceId)
        {
            return StatusCode(406, ModelState);
        }

        /// <summary>
        /// Deactivate an existing device
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        public IActionResult Delete(int userId)
        {
            return StatusCode(406, ModelState);
        }
    }
}