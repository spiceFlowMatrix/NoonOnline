using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Grade;
using Trainning24.BL.ViewModels.Library;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly LibraryBusiness _libraryBusiness;
        private readonly LessonBusiness _lessonBusiness;
        private readonly GradeBusiness _gradeBusiness;
        private readonly FilesBusiness _filesBusiness;
        private IHostingEnvironment _hostingEnvironment;
        public LibraryController(
        LibraryBusiness libraryBusiness,
        LessonBusiness lessonBusiness,
        GradeBusiness gradeBusiness,
        FilesBusiness filesBusiness,
        IHostingEnvironment hostingEnvironment
        )
        {
            _hostingEnvironment = hostingEnvironment;
            _libraryBusiness = libraryBusiness;
            _lessonBusiness = lessonBusiness;
            _gradeBusiness = gradeBusiness;
            _filesBusiness = filesBusiness;
        }

        [Authorize]
        [HttpGet("GetBookById/{Id}")]
        public IActionResult GetBookById(long Id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            BooksViewModelDTO obj = new BooksViewModelDTO();
            List<ResponseGradeModel> responseGradelst = new List<ResponseGradeModel>();
            string Certificate = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    var data = _libraryBusiness.GetById(Id);
                    if (data != null)
                    {
                        obj.id = data.Id;
                        obj.title = data.Title;
                        obj.author = data.Author;
                        obj.subject = data.Subject;
                        obj.bookpublisher = data.BookPublisher;
                        obj.gradeid = data.GradeId;
                        obj.description = data.Description;
                        obj.fileid = data.FileId;
                        obj.IsPublished = data.IsPublished;
                        string[] gradeids = new string[] { };
                        if (data.GradeId != null)
                        {
                            gradeids = data.GradeId.Split(",");
                        }
                        for (int i = 0; i < gradeids.Length; i++)
                        {
                            ResponseGradeModel responseGradeModel = new ResponseGradeModel();
                            Grade grade = _gradeBusiness.getGradeById(int.Parse(gradeids[i]));
                            if (grade != null)
                            {
                                responseGradeModel.id = grade.Id;
                                responseGradeModel.name = grade.Name;
                                responseGradeModel.description = grade.Description;
                                responseGradelst.Add(responseGradeModel);
                            }
                        }
                        obj.grades = responseGradelst;
                        if (data.FileId != 0)
                        {
                            Files newFiles = _filesBusiness.getFilesById(data.FileId);
                            if (newFiles != null)
                            {
                                ResponseFilesModel fileModel = new ResponseFilesModel();
                                var filetyped = _filesBusiness.FileType(newFiles);
                                fileModel.Id = newFiles.Id;
                                fileModel.name = newFiles.Name;
                                fileModel.filename = newFiles.FileName;
                                fileModel.description = newFiles.Description;
                                fileModel.filetypename = filetyped.Filetype;
                                if (!string.IsNullOrEmpty(newFiles.Url))
                                    fileModel.url = _lessonBusiness.geturl(newFiles.Url, Certificate);
                                fileModel.filesize = newFiles.FileSize;
                                fileModel.filetypeid = newFiles.FileTypeId;
                                fileModel.totalpages = newFiles.TotalPages;
                                fileModel.duration = newFiles.Duration;
                                obj.file = fileModel;
                            }
                        }
                        if (data.coverimage != 0)
                        {
                            Files newFiles = _filesBusiness.getFilesById(data.coverimage);
                            if (newFiles != null)
                            {
                                var filetyped = _filesBusiness.FileType(newFiles);
                                obj.coverimage = newFiles.Id;
                                if (!string.IsNullOrEmpty(newFiles.Url))
                                    obj.coverurl = _lessonBusiness.geturl(newFiles.Url, Certificate);
                            }
                        }
                        successResponse.data = obj;
                        successResponse.response_code = 0;
                        successResponse.message = "Book get";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "No book found";
                        unsuccessResponse.message = "No book found";
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
        [HttpGet("GetBooks")]
        public IActionResult GetBooks(int pagenumber, int perpagerecord, string search, string bygrade)
        {
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            List<BooksViewModelDTO> objlst = new List<BooksViewModelDTO>();
            string Certificate = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
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
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    var booklist = _libraryBusiness.GetBooksList(paginationModel, search, bygrade, out int total);
                    if (booklist != null)
                    {
                        foreach (var data in booklist)
                        {
                            BooksViewModelDTO obj = new BooksViewModelDTO();
                            List<ResponseGradeModel> responseGradelst = new List<ResponseGradeModel>();
                            obj.id = data.Id;
                            obj.title = data.Title;
                            obj.author = data.Author;
                            obj.subject = data.Subject;
                            obj.bookpublisher = data.BookPublisher;
                            obj.gradeid = data.GradeId;
                            obj.description = data.Description;
                            obj.fileid = data.FileId;
                            obj.IsPublished = data.IsPublished;
                            if (data.coverimage != 0)
                            {
                                Files newFiles = _filesBusiness.getFilesById(data.coverimage);
                                if (newFiles != null)
                                {
                                    var filetyped = _filesBusiness.FileType(newFiles);
                                    obj.coverimage = newFiles.Id;
                                    if (!string.IsNullOrEmpty(newFiles.Url))
                                        obj.coverurl = _lessonBusiness.geturl(newFiles.Url, Certificate);
                                }
                            }
                            objlst.Add(obj);
                        }
                        successResponse.data = objlst;
                        successResponse.totalcount = total;
                        successResponse.response_code = 0;
                        successResponse.message = "books list get";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "No books found";
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
        [HttpPost("AddBook")]
        public IActionResult AddBook(BooksDTO obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            if (string.IsNullOrEmpty(obj.title))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "Book title is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            if (string.IsNullOrEmpty(obj.author))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "Author name is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            if (obj.grades == null)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "Grades is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            if (string.IsNullOrEmpty(obj.subject))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "Subject is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            if (string.IsNullOrEmpty(obj.bookpublisher))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "Publisher name is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            if (obj.fileid == 0)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "File is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    int checkDuplicateBook = _libraryBusiness.checkDuplicateBook(obj);
                    if (checkDuplicateBook == 1)
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "Book with same title available";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
                    }
                    if (checkDuplicateBook == 2)
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "Same file was used for another book";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
                    }
                    Books books = new Books();
                    books.Title = obj.title;
                    books.Author = obj.author;
                    books.Subject = obj.subject;
                    books.BookPublisher = obj.bookpublisher;
                    if (obj.grades != null)
                    {
                        foreach (int gid in obj.grades)
                        {
                            if (string.IsNullOrEmpty(books.GradeId))
                                books.GradeId += gid;
                            else
                                books.GradeId += "," + gid;
                        }
                    }
                    books.Description = obj.description;
                    books.FileId = obj.fileid;
                    books.coverimage = obj.coverimage;
                    books.IsPublished = false;
                    books.CreatorUserId = int.Parse(tc.Id);
                    books.CreationTime = DateTime.Now.ToString();
                    var data = _libraryBusiness.AddBook(books);
                    obj.id = data.Id;
                    successResponse.data = obj;
                    successResponse.response_code = 0;
                    successResponse.message = "Book added";
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
        [HttpPut("UpdateBook")]
        public IActionResult UpdateBook(BooksDTO obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            if (string.IsNullOrEmpty(obj.title))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "Book title is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            if (string.IsNullOrEmpty(obj.author))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "Author name is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            if (obj.grades == null)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "Grades is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            if (string.IsNullOrEmpty(obj.subject))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "Subject is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            if (string.IsNullOrEmpty(obj.bookpublisher))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "Publisher name is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            if (obj.fileid == 0)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "File is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    int checkDuplicateBookUpdate = _libraryBusiness.checkDuplicateBookUpdate(obj);
                    if (checkDuplicateBookUpdate == 1)
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "Book with same title available";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
                    }
                    if (checkDuplicateBookUpdate == 2)
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "Same file was used for another book";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
                    }
                    Books books = new Books();
                    books.Id = obj.id;
                    books.Title = obj.title;
                    books.Author = obj.author;
                    books.Subject = obj.subject;
                    books.BookPublisher = obj.bookpublisher;
                    if (obj.grades != null)
                    {
                        foreach (int gid in obj.grades)
                        {
                            if (string.IsNullOrEmpty(books.GradeId))
                                books.GradeId += gid;
                            else
                                books.GradeId += "," + gid;
                        }
                    }
                    books.Description = obj.description;
                    books.FileId = obj.fileid;
                    books.coverimage = obj.coverimage;
                    books.IsPublished = false;
                    books.LastModifierUserId = int.Parse(tc.Id);
                    books.LastModificationTime = DateTime.Now.ToString();
                    var data = _libraryBusiness.UpdateBook(books);
                    obj.id = data.Id;
                    successResponse.data = obj;
                    successResponse.response_code = 0;
                    successResponse.message = "Book updated";
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
        [HttpPut("PublishBook/{Id}")]
        public IActionResult PublishBook(long Id)
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
                    Books books = new Books();
                    books.Id = Id;
                    books.IsPublished = true;
                    books.PublisherUserId = int.Parse(tc.Id);
                    books.PublishedTime = DateTime.Now.ToString();
                    var data = _libraryBusiness.PublishBook(books);
                    if (data == 1)
                    {
                        successResponse.response_code = 0;
                        successResponse.message = "Book published";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Something went wrong please try again.";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(401, unsuccessResponse);
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
        [HttpDelete("DeleteBook/{Id}")]
        public IActionResult DeleteBook(long Id)
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
                    Books books = new Books();
                    books.Id = Id;
                    books.DeleterUserId = int.Parse(tc.Id);
                    books.DeletionTime = DateTime.Now.ToString();
                    var data = _libraryBusiness.DeleteBook(books);
                    if (data == 1)
                    {
                        successResponse.response_code = 0;
                        successResponse.message = "Book Deleted";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Book are published it can not be deleted/archived";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(401, unsuccessResponse);
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
        [HttpGet("GetBooksApp")]
        public IActionResult GetBooksApp(int pagenumber, int perpagerecord, string search)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            List<BooksAppDTO> objlst = new List<BooksAppDTO>();
            string Certificate = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
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
                if (tc.RoleName.Contains(General.getRoleType("4")))
                {
                    var getGradeList = _libraryBusiness.GetGradeListByStudentId(int.Parse(tc.Id));
                    var booklist = _libraryBusiness.GetBooksListApp(getGradeList, paginationModel, search);
                    if (booklist != null)
                    {
                        foreach (var data in booklist)
                        {
                            BooksAppDTO obj = new BooksAppDTO();
                            List<ResponseGradeModel> responseGradelst = new List<ResponseGradeModel>();
                            obj.id = data.Id;
                            obj.title = data.Title;
                            obj.author = data.Author;
                            obj.subject = data.Subject;
                            obj.bookpublisher = data.BookPublisher;
                            obj.description = data.Description;
                            if (data.FileId != 0)
                            {
                                Files newFiles = _filesBusiness.getFilesById(data.FileId);
                                if (newFiles != null)
                                {
                                    ResponseFilesModel fileModel = new ResponseFilesModel();
                                    var filetyped = _filesBusiness.FileType(newFiles);
                                    fileModel.Id = newFiles.Id;
                                    fileModel.name = newFiles.Name;
                                    fileModel.filename = newFiles.FileName;
                                    fileModel.description = newFiles.Description;
                                    fileModel.filetypename = filetyped.Filetype;
                                    if (!string.IsNullOrEmpty(newFiles.Url))
                                    {
                                        fileModel.url = _lessonBusiness.geturl(newFiles.Url, Certificate);
                                        //obj.bookcoverimage = fileModel.url;
                                    }
                                    fileModel.filesize = newFiles.FileSize;
                                    fileModel.filetypeid = newFiles.FileTypeId;
                                    fileModel.totalpages = newFiles.TotalPages;
                                    fileModel.duration = newFiles.Duration;
                                    obj.file = fileModel;
                                }
                            }
                            if (data.coverimage != 0)
                            {
                                Files newFiles = _filesBusiness.getFilesById(data.coverimage);
                                if (newFiles != null)
                                {
                                    if (!string.IsNullOrEmpty(newFiles.Url))
                                        obj.bookcoverimage = _lessonBusiness.geturl(newFiles.Url, Certificate);
                                }
                            }
                            objlst.Add(obj);
                        }
                        successResponse.data = objlst;
                        successResponse.response_code = 0;
                        successResponse.message = "books list get";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "No books found";
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
        [HttpGet("GetBooksGradeWiseApp")]
        public IActionResult GetBooksGradeWiseApp(int pagenumber, int perpagerecord, string search)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            List<BooksGradeWiseDTO> objlst = new List<BooksGradeWiseDTO>();
            string Certificate = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
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
                if (tc.RoleName.Contains(General.getRoleType("4")))
                {
                    var getGradeList = _libraryBusiness.GetGradeListByStudentId(int.Parse(tc.Id));
                    if (getGradeList != null)
                    {
                        foreach (var gid in getGradeList)
                        {
                            BooksGradeWiseDTO booksGradeWiseDTO = new BooksGradeWiseDTO();
                            var getGradeById = _libraryBusiness.getGradeById(gid);
                            var getbooklist = _libraryBusiness.GetBooksGradeWiseApp(gid, paginationModel, search);
                            if (getbooklist != null && getbooklist.Count > 0)
                            {
                                booksGradeWiseDTO.id = getGradeById.Id;
                                booksGradeWiseDTO.gradename = getGradeById.Name;
                                List<BooksAppDTO> booklist = new List<BooksAppDTO>();
                                foreach (var data in getbooklist)
                                {
                                    BooksAppDTO obj = new BooksAppDTO();
                                    obj.id = data.Id;
                                    obj.title = data.Title;
                                    obj.author = data.Author;
                                    obj.subject = data.Subject;
                                    obj.bookpublisher = data.BookPublisher;
                                    obj.description = data.Description;
                                    if (data.FileId != 0)
                                    {
                                        Files newFiles = _filesBusiness.getFilesById(data.FileId);
                                        if (newFiles != null)
                                        {
                                            ResponseFilesModel fileModel = new ResponseFilesModel();
                                            var filetyped = _filesBusiness.FileType(newFiles);
                                            fileModel.Id = newFiles.Id;
                                            fileModel.name = newFiles.Name;
                                            fileModel.filename = newFiles.FileName;
                                            fileModel.description = newFiles.Description;
                                            fileModel.filetypename = filetyped.Filetype;
                                            if (!string.IsNullOrEmpty(newFiles.Url))
                                            {
                                                fileModel.url = _lessonBusiness.geturl(newFiles.Url, Certificate);
                                                //obj.bookcoverimage = fileModel.url;
                                            }
                                            fileModel.filesize = newFiles.FileSize;
                                            fileModel.filetypeid = newFiles.FileTypeId;
                                            fileModel.totalpages = newFiles.TotalPages;
                                            fileModel.duration = newFiles.Duration;
                                            obj.file = fileModel;
                                        }
                                    }
                                    if (data.coverimage != 0)
                                    {
                                        Files newFiles = _filesBusiness.getFilesById(data.coverimage);
                                        if (newFiles != null)
                                        {
                                            if (!string.IsNullOrEmpty(newFiles.Url))
                                                obj.bookcoverimage = _lessonBusiness.geturl(newFiles.Url, Certificate);
                                        }
                                    }
                                    booklist.Add(obj);
                                }
                                booksGradeWiseDTO.books = booklist;
                                objlst.Add(booksGradeWiseDTO);
                            }
                        }
                        successResponse.data = objlst;
                        successResponse.response_code = 0;
                        successResponse.message = "books list get";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "No books found";
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


        [HttpPost("AddBookCover")]
        public async Task<IActionResult> AddBookCover()
        {
            string jsonPath = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);

            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = _lessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    string fileName = "";
                    IFormFile file = null;
                    if (Request.Form.Files.Count != 0)
                        file = Request.Form.Files[0];

                    var imageAcl = PredefinedObjectAcl.PublicRead;

                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                    var ext = fileName.Substring(fileName.LastIndexOf("."));
                    var extension = ext.ToLower();
                    if (AllowedFileExtensions.Contains(extension))
                    {
                        Guid imageGuid = Guid.NewGuid();
                        fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;
                        if (Request.Form["fileTypeId"] == "4")
                        {
                            var imageObject = await storage.UploadObjectAsync(
                                bucket: "t24-primary-image-storage",
                                objectName: fileName,
                                contentType: file.ContentType,
                                source: file.OpenReadStream(),
                                options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                            );
                            mediaLink = imageObject.MediaLink;
                        }
                        AddFilesModel FilesModel = new AddFilesModel();
                        if (!string.IsNullOrEmpty(Request.Form["description"].ToString()))
                            FilesModel.Description = Request.Form["description"];
                        FilesModel.Url = mediaLink;
                        FilesModel.Name = fileName;
                        FilesModel.FileName = fileName;
                        FilesModel.FileTypeId = long.Parse(Request.Form["fileTypeId"]);
                        FilesModel.FileSize = file.Length;
                        if (!string.IsNullOrEmpty(Request.Form["duration"].ToString()))
                            FilesModel.Duration = Request.Form["duration"].ToString();
                        if (!string.IsNullOrEmpty(Request.Form["totalpages"]))
                            FilesModel.TotalPages = int.Parse(Request.Form["totalpages"]);
                        Files newFiles = _filesBusiness.Create(FilesModel, int.Parse(tc.Id));
                        var filetype = _filesBusiness.FileType(newFiles);
                        ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                        responseFilesModel.Id = newFiles.Id;
                        responseFilesModel.name = newFiles.Name;
                        responseFilesModel.url = newFiles.Url;
                        responseFilesModel.filename = newFiles.FileName;
                        responseFilesModel.description = newFiles.Description;
                        responseFilesModel.filetypeid = newFiles.FileTypeId;
                        responseFilesModel.filesize = newFiles.FileSize;
                        responseFilesModel.filetypename = filetype.Filetype;
                        responseFilesModel.duration = newFiles.Duration;
                        responseFilesModel.totalpages = newFiles.TotalPages;

                        successResponse.data = responseFilesModel;
                        successResponse.response_code = 0;
                        successResponse.message = "Files Created";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "Need image file";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
                    }
                }
                else
                {
                    return StatusCode(406, ModelState);
                }
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

    }
}