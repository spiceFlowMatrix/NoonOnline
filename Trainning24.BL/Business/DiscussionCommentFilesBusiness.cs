using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.DiscussionTopic;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class DiscussionCommentFilesBusiness
    {
        private readonly EFDiscussionCommentFilesRepository EFDiscussionCommentFilesRepository;
        private readonly LogObjectBusiness _logObjectBusiness;

        public DiscussionCommentFilesBusiness(
            EFDiscussionCommentFilesRepository EFDiscussionCommentFilesRepository,
            LogObjectBusiness logObjectBusiness
        )
        {
            this.EFDiscussionCommentFilesRepository = EFDiscussionCommentFilesRepository;
            _logObjectBusiness = logObjectBusiness;
        }


        public FileTypes FileType(DiscussionCommentFiles file)
        {
            return EFDiscussionCommentFilesRepository.FileType(file);
        }

        public DiscussionCommentFiles Create(DiscussionCommentFiles AddFilesModel, int id)
        {
            DiscussionCommentFiles files1st = EFDiscussionCommentFilesRepository.ListQuery(b => b.FileName == AddFilesModel.FileName || b.Name == AddFilesModel.Name).SingleOrDefault();

            DiscussionCommentFiles Files = new DiscussionCommentFiles
            {
                Name = AddFilesModel.Name,
                FileName = AddFilesModel.FileName,
                FileSize = AddFilesModel.FileSize,
                FileTypeId = AddFilesModel.FileTypeId,
                Url = AddFilesModel.Url,
                Duration = AddFilesModel.Duration,
                TotalPages = AddFilesModel.TotalPages,
                CreationTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                CreatorUserId = id,
                IsDeleted = false
            };

            if (files1st == null)
            {
                EFDiscussionCommentFilesRepository.Insert(Files);
                _logObjectBusiness.AddLogsObject(34, Files.Id, id);
            }

            return Files;
        }

        public DiscussionCommentFiles getFilesById(long id)
        {
            return EFDiscussionCommentFilesRepository.GetById(id);
        }

        public DiscussionCommentFiles GetByUrl(string url)
        {
            return EFDiscussionCommentFilesRepository.GetByUrl(url);
        }

        public DiscussionCommentFiles Delete(int Id, int DeleterId)
        {
            DiscussionCommentFiles deletedFiles = EFDiscussionCommentFilesRepository.GetById(Id);
            DiscussionCommentFiles Files = new DiscussionCommentFiles();
            Files.Id = Id;
            Files.DeleterUserId = DeleterId;
            Files.DeletionTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            int i = EFDiscussionCommentFilesRepository.Delete(Files);
            _logObjectBusiness.AddLogsObject(36, Files.Id, DeleterId);
            return deletedFiles;
        }

        public int UpdateCommentId(int FileId, int commentid)
        {
            DiscussionCommentFiles getbyId = getFilesById(FileId);
            getbyId.CommentId = commentid;
            return EFDiscussionCommentFilesRepository.Update(getbyId);
        }

        public List<DiscussionCommentFiles> GetFilesByCommentId(int commentid)
        {
            return EFDiscussionCommentFilesRepository.ListQuery(b => b.CommentId == commentid && b.IsDeleted != true).OrderByDescending(b => b.Id).ToList();
        }

    }
}
