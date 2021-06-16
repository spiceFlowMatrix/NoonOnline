using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class FilesBusiness
    {
        private readonly EFFilesRepository EFFilesRepository;
        private readonly LogObjectBusiness _logObjectBusiness;

        public FilesBusiness(
            EFFilesRepository EFFilesRepository,
            LogObjectBusiness logObjectBusiness
        )
        {
            this.EFFilesRepository = EFFilesRepository;
            _logObjectBusiness = logObjectBusiness;
        }


        public FileTypes FileType(Files file)
        {
            return EFFilesRepository.FileType(file);
        }

        public Files Create(AddFilesModel AddFilesModel, int id)
        {
            Files files1st = EFFilesRepository.ListQuery(b => b.FileName == AddFilesModel.FileName || b.Name == AddFilesModel.Name).SingleOrDefault();

            Files Files = new Files
            {
                Name = AddFilesModel.Name,
                FileName = AddFilesModel.FileName,
                FileSize = AddFilesModel.FileSize,
                FileTypeId = AddFilesModel.FileTypeId,
                Url = AddFilesModel.Url,
                Duration = AddFilesModel.Duration,
                TotalPages = AddFilesModel.TotalPages,
                Description = AddFilesModel.Description,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id,
                IsDeleted = false
            };

            if (files1st == null)
            {
                EFFilesRepository.Insert(Files);
                _logObjectBusiness.AddLogsObject(19, Files.Id, id);
            }

            return Files;
        }

        public Files Update(AddFilesModel AddFilesModel, int id)
        {
            Files Files = new Files
            {
                Id = AddFilesModel.Id,
                Name = AddFilesModel.Name,
                FileName = AddFilesModel.FileName,
                FileSize = AddFilesModel.FileSize,
                FileTypeId = AddFilesModel.FileTypeId,
                Url = AddFilesModel.Url,
                Duration = AddFilesModel.Duration,
                TotalPages = AddFilesModel.TotalPages,
                Description = AddFilesModel.Description,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id,
                IsDeleted = false
            };

            EFFilesRepository.UpdateTest(Files);
            _logObjectBusiness.AddLogsObject(20, Files.Id, id);
            return Files;
        }

        public List<Files> GetALL()
        {
            return EFFilesRepository.GetAll();
        }

        public Files Update(UpdateFilesModel UpdateFilesModel, int id)
        {
            Files Files = new Files
            {
                Id = UpdateFilesModel.Id,
                Name = UpdateFilesModel.Name,
                FileName = UpdateFilesModel.FileName,
                FileSize = UpdateFilesModel.FileSize,
                FileTypeId = UpdateFilesModel.FileTypeId,
                Duration = UpdateFilesModel.Duration,
                TotalPages = UpdateFilesModel.TotalPages,
                Url = UpdateFilesModel.Url,
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = id
            };

            EFFilesRepository.Update(Files);
            _logObjectBusiness.AddLogsObject(20, Files.Id, id);
            return Files;
        }

        public List<Files> FilesList(PaginationModel paginationModel, out int total)
        {
            List<Files> FilesList = EFFilesRepository.GetAll().Where(b => b.IsDeleted != true).OrderByDescending(b => b.Id).ToList();
            if (paginationModel.pagenumber == 0 && paginationModel.perpagerecord == 0 && paginationModel.roleid != 0)
            {

                FilesList = FilesList.Where(b => b.FileTypeId == paginationModel.roleid && b.IsDeleted != true).ToList();
                total = FilesList.Count();
                if (!string.IsNullOrEmpty(paginationModel.search))
                {
                    //b.Url.Contains(paginationModel.search) &&
                    FilesList = FilesList.Where(
                            b => b.Name.ToLower().Any(k => b.Name.Contains(paginationModel.search.ToLower()))
                            || b.FileTypeId == paginationModel.roleid).OrderByDescending(b => b.Id).Take(400).ToList();
                    total = FilesList.Count();
                    return FilesList;
                }
                return FilesList;
            }
            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0 && paginationModel.roleid != 0)
            {
                if (!string.IsNullOrEmpty(paginationModel.search))
                {
                    FilesList = FilesList.Where(b => b.FileTypeId == paginationModel.roleid && b.IsDeleted != true).ToList();


                    FilesList = FilesList.Where(b => b.Name.Any(k => b.Name.ToLower().Contains(paginationModel.search.ToLower())) ||
                                                     b.FileName.Any(k => b.FileName.ToLower().Contains(paginationModel.search.ToLower()))
                                                     //|| b.Url.Any(k => b.Url.ToLower().Contains(paginationModel.search.ToLower()))
                                                     ).OrderByDescending(b => b.Id).ToList();
                    total = FilesList.Count();
                    FilesList = FilesList.OrderByDescending(b => b.Id).
                                Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                                Take(paginationModel.perpagerecord).
                                ToList();

                    return FilesList;
                }
                FilesList = FilesList.Where(b => b.FileTypeId == paginationModel.roleid && b.IsDeleted != true).ToList();
                total = FilesList.Count();
                FilesList = FilesList.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();

                return FilesList;
            }
            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = FilesList.Count();
                if (!string.IsNullOrEmpty(paginationModel.search))
                {
                    FilesList = FilesList.Where(b => b.Name.Any(k => b.Name.ToLower().Contains(paginationModel.search.ToLower())) ||
                                                     b.FileName.Any(k => b.FileName.ToLower().Contains(paginationModel.search.ToLower()))
                                                     //|| b.Url.Any(k => b.Url.ToLower().Contains(paginationModel.search.ToLower()))
                                                     ).OrderByDescending(b => b.Id).ToList();

                    total = FilesList.Count();

                    FilesList = FilesList.OrderByDescending(b => b.Id).
                                Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                                Take(paginationModel.perpagerecord).
                                ToList();


                    return FilesList;
                }
                FilesList = FilesList.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                return FilesList;
            }
            total = FilesList.Count();
            if (!string.IsNullOrEmpty(paginationModel.search))
            {
                FilesList = FilesList.Where(b => b.Name.Any(k => b.Name.ToLower().Contains(paginationModel.search.ToLower())) ||
                                                 b.FileName.Any(k => b.FileName.ToLower().Contains(paginationModel.search.ToLower()))
                                                 //||b.Url.Any(k => b.Url.ToLower().Contains(paginationModel.search.ToLower()))
                                                 ).OrderByDescending(b => b.Id).ToList();
                total = FilesList.Count();
                return FilesList;
            }
            return FilesList;
        }

        public Files getFilesById(long id)
        {
            return EFFilesRepository.GetById(id);
        }

        public Files GetByUrl(string url)
        {
            return EFFilesRepository.GetByUrl(url);
        }

        public Files Delete(int Id, int DeleterId)
        {
            Files deletedFiles = EFFilesRepository.GetById(Id);
            Files Files = new Files();
            Files.Id = Id;
            Files.DeleterUserId = DeleterId;
            Files.DeletionTime = DateTime.Now.ToString();
            int i = EFFilesRepository.Delete(Files);
            _logObjectBusiness.AddLogsObject(21, Files.Id, DeleterId);
            return deletedFiles;
        }

        public object ImportVideo()
        {
            string jsonPath = "training24-28e994f9833c.json";
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);
            foreach (var obj in storage.ListObjects("t24-primary-video-storage", ""))
            {
                Files file = GetByUrl(obj.MediaLink);
                if (file == null)
                {
                    AddFilesModel FilesModel = new AddFilesModel();
                    FilesModel.Url = obj.MediaLink;
                    FilesModel.Name = obj.Name;
                    FilesModel.FileName = obj.Name;
                    FilesModel.FileTypeId = 2;
                    FilesModel.FileSize = long.Parse(obj.Size.ToString());
                    Files newFiles = Create(FilesModel, int.Parse("1"));
                }
            }

            return null;
        }

        public object ImportImage()
        {
            string jsonPath = "training24-28e994f9833c.json";
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);

            foreach (var obj in storage.ListObjects("t24-primary-image-storage", ""))
            {
                Files file = GetByUrl(obj.MediaLink);
                if (file == null)
                {
                    AddFilesModel FilesModel = new AddFilesModel();
                    FilesModel.Url = obj.MediaLink;
                    FilesModel.Name = obj.Name;
                    FilesModel.FileName = obj.Name;
                    FilesModel.FileTypeId = 3;
                    FilesModel.FileSize = long.Parse(obj.Size.ToString());
                    Files newFiles = Create(FilesModel, int.Parse("1"));
                }
            }
            return null;
        }

        public object ImportPdf()
        {
            string jsonPath = "training24-28e994f9833c.json";
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);
            foreach (var obj in storage.ListObjects("t24-primary-pdf-storage", ""))
            {
                Files file = GetByUrl(obj.MediaLink);
                if (file == null)
                {
                    AddFilesModel FilesModel = new AddFilesModel();
                    FilesModel.Url = obj.MediaLink;
                    FilesModel.Name = obj.Name;
                    FilesModel.FileName = obj.Name;
                    FilesModel.FileTypeId = 1;
                    FilesModel.FileSize = long.Parse(obj.Size.ToString());
                    Files newFiles = Create(FilesModel, int.Parse("1"));
                }
            }
            return null;
        }

        public List<Files> FilesListNew(PaginationModel paginationModel, out int total)
        {
            List<Files> FilesList = EFFilesRepository.ListQuery(b => b.Name.ToLower().Contains(paginationModel.search.ToLower()) && b.FileTypeId == paginationModel.roleid && b.IsDeleted != true).OrderByDescending(b => b.Id).ToList();
            total = FilesList.Count();
            if (!string.IsNullOrEmpty(paginationModel.search))
            {
                FilesList = FilesList.Where(
                        b => b.Name.ToLower().Contains(paginationModel.search.ToLower())
                        || b.FileTypeId == paginationModel.roleid).OrderByDescending(b => b.Id).Take(400).ToList();
                total = FilesList.Count();
            }
            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                FilesList = FilesList.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                total = FilesList.Count();
            }
            return FilesList;
        }

        public int GetFileTypeByExt(string extension)
        {
            var fileType = 0;
            if (!string.IsNullOrEmpty(extension))
            {
                if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp" || extension == ".gif")
                {
                    fileType = 3;
                }
                if (extension == ".pdf")
                {
                    fileType = 1;
                }
                if (extension == ".avi" || extension == ".mp4" || extension == ".divx" || extension == ".wmv" || extension ==".webm")
                {
                    fileType = 2;
                }
            }
            return fileType;
        }

    }
}
