using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class DiscussionFilesBusiness
    {
        private readonly EFDiscussionFilesRepository EFDiscussionFilesRepository;
        private readonly LogObjectBusiness _logObjectBusiness;
        public DiscussionFilesBusiness(
            EFDiscussionFilesRepository EFDiscussionFilesRepository,
            LogObjectBusiness logObjectBusiness
        )
        {
            this.EFDiscussionFilesRepository = EFDiscussionFilesRepository;
            _logObjectBusiness = logObjectBusiness;
        }


        public FileTypes FileType(DiscussionFiles file)
        {
            return EFDiscussionFilesRepository.FileType(file);
        }

        public DiscussionFiles Create(DiscussionFilesModel AddFilesModel, int id)
        {
            DiscussionFiles files1st = EFDiscussionFilesRepository.ListQuery(b => b.FileName == AddFilesModel.FileName || b.Name == AddFilesModel.Name).SingleOrDefault();

            DiscussionFiles Files = new DiscussionFiles
            {
                Name = AddFilesModel.Name,
                FileName = AddFilesModel.FileName,
                FileSize = AddFilesModel.FileSize,
                FileTypeId = AddFilesModel.FileTypeId,
                Url = AddFilesModel.Url,
                Duration = AddFilesModel.Duration,
                TotalPages = AddFilesModel.TotalPages,
                Description = AddFilesModel.Description,
                CreationTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                CreatorUserId = id,
                IsDeleted = false
            };

            if (files1st == null)
            {
                EFDiscussionFilesRepository.Insert(Files);
                _logObjectBusiness.AddLogsObject(31, Files.Id, id);
            }

            return Files;
        }

        public DiscussionFiles Update(AddFilesModel AddFilesModel, int id)
        {
            DiscussionFiles Files = new DiscussionFiles
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
                CreationTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                CreatorUserId = id,
                IsDeleted = false
            };

            EFDiscussionFilesRepository.UpdateTest(Files);
            _logObjectBusiness.AddLogsObject(32, Files.Id, id);
            return Files;
        }

        public List<DiscussionFiles> GetALL()
        {
            return EFDiscussionFilesRepository.GetAll();
        }

        public DiscussionFiles Update(UpdateFilesModel UpdateFilesModel, int id)
        {
            DiscussionFiles Files = new DiscussionFiles
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

            EFDiscussionFilesRepository.Update(Files);
            return Files;
        }

        public List<DiscussionFiles> FilesList(PaginationModel paginationModel, out int total)
        {
            List<DiscussionFiles> FilesList = EFDiscussionFilesRepository.GetAll().Where(b => b.IsDeleted != true).OrderByDescending(b=>b.Id).ToList();
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

        public DiscussionFiles getFilesById(long id)
        {
            return EFDiscussionFilesRepository.GetById(id);
        }

        public DiscussionFiles GetByUrl(string url)
        {
            return EFDiscussionFilesRepository.GetByUrl(url);
        }

        public DiscussionFiles Delete(int Id, int DeleterId)
        {
            DiscussionFiles deletedFiles = EFDiscussionFilesRepository.GetById(Id);
            DiscussionFiles Files = new DiscussionFiles();
            Files.Id = Id;
            Files.DeleterUserId = DeleterId;
            Files.DeletionTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            int i = EFDiscussionFilesRepository.Delete(Files);
            _logObjectBusiness.AddLogsObject(33, Files.Id, DeleterId);
            return deletedFiles;
        }

        public int UpdateTopicId(int FileId, int TopicId)
        {
            DiscussionFiles getbyId = getFilesById(FileId);
            getbyId.TopicId = TopicId;
            return EFDiscussionFilesRepository.Update(getbyId);
        }

        public List<DiscussionFiles> GetFilesByTopicId(int TopicId)
        {
            return EFDiscussionFilesRepository.ListQuery(b => b.TopicId ==TopicId && b.IsDeleted != true).OrderByDescending(b => b.Id).ToList();
        }
    }
}
