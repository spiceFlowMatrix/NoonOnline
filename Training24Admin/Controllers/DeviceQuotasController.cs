using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trainning24.BL.ViewModels;

namespace Training24Admin.Controllers
{
    /// <summary>
    /// Everything about your device quotas
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = nameof(SwaggerGrouping.DeviceQuotas))]
    public class DeviceQuotasController : ControllerBase
    {
        /// <summary>
        /// Fetch a list of quotas for all students who have activated (or registered) the student app on it
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public IActionResult Get()
        {
            return StatusCode(406, ModelState);
        }

        /// <summary>
        /// Request a device quota exension
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="objData">Quota extension request details</param>
        /// <returns></returns>
        [HttpPost("{userId}")]
        public IActionResult Post(int userId, DeviceQuotas objData)
        {
            return StatusCode(406, ModelState);
        }

        /// <summary>
        /// Approve existing quota extension requests
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="extensionRequestId">Requested device extention id</param>
        /// <returns></returns>
        [HttpPut("{userId}")]
        public IActionResult Put(int userId, int extensionRequestId)
        {
            return StatusCode(406, ModelState);
        }

        /// <summary>
        /// Reject existing quota extension requests
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="extensionRequestId">Requested device extention id</param>
        /// <returns></returns>
        [HttpDelete("{userId}")]
        public IActionResult Delete(int userId, int extensionRequestId)
        {
            return StatusCode(406, ModelState);
        }
    }
}