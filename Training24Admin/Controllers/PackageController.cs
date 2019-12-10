using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Package;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly PackageBusiness _packageBusiness;
        private readonly LessonBusiness _lessonBusiness;
        public PackageController(
           PackageBusiness packageBusiness,
           PackageCourseBusiness packageCourseBusiness,
           LessonBusiness lessonBusiness,
           CourseBusiness courseBusiness
           )
        {
            _packageBusiness = packageBusiness;
            _lessonBusiness = lessonBusiness;
        }

        #region Package
        [Authorize]
        [HttpGet("GetPackage/{Id}")]
        public IActionResult Getpackage(long Id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            PackageDTO obj = new PackageDTO();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    var data = _packageBusiness.GetById(Id);
                    if (data != null)
                    {
                        obj.id = data.Id;
                        obj.name = data.Name;
                        obj.price = data.Price;
                        successResponse.data = obj;
                        successResponse.response_code = 0;
                        successResponse.message = "Package get";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "No package found";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
                    }
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "You are not authorized.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(401, unsuccessResponse);
                }

            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [Authorize]
        [HttpGet("GetPackages")]
        public IActionResult GetPackages(int pagenumber, int perpagerecord)
        {
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            List<PackageDTO> objlst = new List<PackageDTO>();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            PaginationModel paginationModel = new PaginationModel
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord
            };
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")) || tc.RoleName.Contains(General.getRoleType("13")) || tc.RoleName.Contains(General.getRoleType("17")))
                {
                    var data = _packageBusiness.GetPackageList(paginationModel, out int total);
                    if (data != null)
                    {
                        foreach (var service in data)
                        {
                            PackageDTO obj = new PackageDTO();
                            obj.id = service.Id;
                            obj.name = service.Name;
                            obj.price = service.Price;
                            objlst.Add(obj);
                        }
                        successResponse.data = objlst;
                        successResponse.totalcount = total;
                        successResponse.response_code = 0;
                        successResponse.message = "package list get";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "No package found";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
                    }
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "You are not authorized.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(401, unsuccessResponse);
                }

            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [Authorize]
        [HttpPost("AddPackage")]
        public IActionResult AddPackage(PackageDTO obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            if (string.IsNullOrEmpty(obj.name))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "name is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            if (string.IsNullOrEmpty(obj.price))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "price is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    Package package = new Package();
                    package.Name = obj.name;
                    package.Price = obj.price;
                    package.CreatorUserId = int.Parse(tc.Id);
                    package.CreationTime = DateTime.Now.ToString();
                    var data = _packageBusiness.AddPackage(package);
                    obj.id = data.Id;
                    successResponse.data = obj;
                    successResponse.response_code = 0;
                    successResponse.message = "Package added";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "You are not authorized.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(401, unsuccessResponse);
                }

            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [Authorize]
        [HttpPut("UpdatePackage")]
        public IActionResult UpdatePackage(PackageDTO obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            if (string.IsNullOrEmpty(obj.name))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "name is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            if (string.IsNullOrEmpty(obj.price))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "price is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    Package package = new Package();
                    package.Id = obj.id;
                    package.Name = obj.name;
                    package.Price = obj.price;
                    package.LastModifierUserId = int.Parse(tc.Id);
                    package.LastModificationTime = DateTime.Now.ToString();
                    var data = _packageBusiness.UpdatePackage(package);
                    obj.id = data.Id;
                    successResponse.data = obj;
                    successResponse.response_code = 0;
                    successResponse.message = "package updated";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "You are not authorized.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(401, unsuccessResponse);
                }

            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [Authorize]
        [HttpDelete("DeletePackage/{Id}")]
        public IActionResult DeletePackage(long Id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    Package package = new Package();
                    package.Id = Id;
                    package.DeleterUserId = int.Parse(tc.Id);
                    package.DeletionTime = DateTime.Now.ToString();
                    var data = _packageBusiness.DeletePackage(package);
                    if (data == 1)
                    {
                        successResponse.response_code = 0;
                        successResponse.message = "Package deleted";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "No package found";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
                    }
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "You are not authorized.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(401, unsuccessResponse);
                }

            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        #endregion
    }
}