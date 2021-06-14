using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.Library;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class LibraryBusiness
    {
        private readonly EFLibraryRepository _EFLibraryRepository;
        private readonly LogObjectBusiness _logObjectBusiness;
        private readonly EFStudentCourseRepository _EFStudentCourseRepository;
        private readonly EFCourseGradeRepository _EFCourseGradeRepository;
        private readonly EFGradeRepository _EFGradeRepository;
        public LibraryBusiness(
            EFLibraryRepository EFLibraryRepository,
            LogObjectBusiness logObjectBusiness,
            EFStudentCourseRepository EFStudentCourseRepository,
             EFCourseGradeRepository EFCourseGradeRepository,
            EFGradeRepository EFGradeRepository
        )
        {
            _EFLibraryRepository = EFLibraryRepository;
            _logObjectBusiness = logObjectBusiness;
            _EFStudentCourseRepository = EFStudentCourseRepository;
            _EFCourseGradeRepository = EFCourseGradeRepository;
            _EFGradeRepository = EFGradeRepository;
        }

        public Books GetById(long Id)
        {
            return _EFLibraryRepository.GetById(b => b.Id == Id && b.DeletionTime == null);
        }

        public int checkDuplicateBookUpdate(BooksDTO obj)
        {
            var getAllBooks = _EFLibraryRepository.GetById(b => b.Title.ToLower() == obj.title.ToLower() && b.Id != obj.id && b.DeletionTime == null);
            var getSameFileBook = _EFLibraryRepository.GetById(b => b.FileId == obj.fileid && b.Id != obj.id && b.DeletionTime == null);
            if (getAllBooks != null)
            {
                return 1;
            }
            else if (getSameFileBook != null)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        public int checkDuplicateBook(BooksDTO obj)
        {
            var getAllBooks = _EFLibraryRepository.GetById(b => b.Title.ToLower() == obj.title.ToLower() && b.DeletionTime == null);
            var getSameFileBook = _EFLibraryRepository.GetById(b => b.FileId == obj.fileid && b.DeletionTime == null);
            if (getAllBooks != null)
            {
                return 1;
            }
            else if (getSameFileBook != null)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }


        public List<Books> GetBooksList(PaginationModel paginationModel, string search, string bygrade, out int total)
        {
            var data = _EFLibraryRepository.ListQuery(p => p.DeletionTime == null).ToList();
            total = data.Count();
            if (!string.IsNullOrEmpty(bygrade))
            {
                data = data.Where(x => bygrade.Any(fp => x.GradeId.Contains(fp))).Distinct().ToList();
            }
            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(b => b.Title != null && b.Title.ToLower().Contains(search.ToLower()) ||
                                  b.Author.ToLower().Contains(search.ToLower()) ||
                                  b.Subject.ToLower().Contains(search.ToLower()) ||
                                  b.BookPublisher.ToLower().Contains(search.ToLower())).ToList();
            }
            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                data = data.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                return data;
            }
            return data;
        }

        public Books AddBook(Books obj)
        {
            _EFLibraryRepository.Insert(obj);
            _logObjectBusiness.AddLogsObject(46, obj.Id, obj.CreatorUserId ?? 0);
            return obj;
        }

        public Books UpdateBook(Books obj)
        {
            var service = GetById(obj.Id);
            if (service != null)
            {
                service.Title = obj.Title;
                service.Author = obj.Author;
                service.Subject = obj.Subject;
                service.BookPublisher = obj.BookPublisher;
                service.GradeId = obj.GradeId;
                service.Description = obj.Description;
                service.FileId = obj.FileId;
                service.coverimage = obj.coverimage;
                service.IsPublished = obj.IsPublished;
                service.LastModificationTime = obj.LastModificationTime;
                service.LastModifierUserId = obj.LastModifierUserId;
                _EFLibraryRepository.Update(service);
                _logObjectBusiness.AddLogsObject(47, service.Id, service.LastModifierUserId ?? 0);
            }
            return service;
        }

        public int PublishBook(Books obj)
        {
            var data = GetById(obj.Id);
            if (data != null)
            {
                data.IsPublished = true;
                data.PublishedTime = obj.PublishedTime;
                data.PublisherUserId = obj.PublisherUserId;
                _EFLibraryRepository.Update(data);
                _logObjectBusiness.AddLogsObject(49, data.Id, data.DeleterUserId ?? 0);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int DeleteBook(Books obj)
        {
            var data = _EFLibraryRepository.GetById(b => b.Id == obj.Id && b.IsPublished != true);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeletionTime = obj.DeletionTime;
                data.DeleterUserId = obj.DeleterUserId;
                _EFLibraryRepository.Update(data);
                _logObjectBusiness.AddLogsObject(48, data.Id, data.DeleterUserId ?? 0);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public List<Books> GetBooksListApp(List<long> gradelist, PaginationModel paginationModel, string search)
        {
            string gradelists = "";
            if (gradelist != null)
            {
                foreach (int gid in gradelist)
                {
                    if (string.IsNullOrEmpty(gradelists))
                        gradelists += gid;
                    else
                        gradelists += "," + gid;
                }
            }

            var data = _EFLibraryRepository.ListQuery(p => p.DeletionTime == null && p.IsPublished == true).ToList();
            data = data.Where(x => gradelists.Any(fp => x.GradeId.Contains(fp))).Distinct().ToList();

            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(b => b.Title != null && b.Title.ToLower().Contains(search.ToLower()) ||
                                  b.Author.ToLower().Contains(search.ToLower()) ||
                                  b.Subject.ToLower().Contains(search.ToLower()) ||
                                  b.BookPublisher.ToLower().Contains(search.ToLower())).ToList();
            }
            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                data = data.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                return data;
            }
            return data;
        }

        public List<long> GetGradeListByStudentId(long id)
        {
            List<long> gradelist = new List<long>();
            List<long> courselist = _EFStudentCourseRepository.ListQuery(b => b.UserId == id && b.IsDeleted != true).Select(b => b.CourseId).ToList();
            if (courselist != null)
            {

                gradelist = _EFCourseGradeRepository.ListQuery(b => b.IsDeleted != true).Where(s => courselist.Any(u => u.Equals(s.CourseId))).Select(g => g.Gradeid).ToList();
                gradelist = gradelist.Distinct().ToList();
            }
            return gradelist;
        }

        public List<Books> GetBooksGradeWiseApp(long gradeId, PaginationModel paginationModel, string search)
        {
            var data = _EFLibraryRepository.ListQuery(p => p.DeletionTime == null && p.IsPublished == true).ToList();
            string grade = gradeId.ToString();
            data = data.Where(x => grade.Any(fp => x.GradeId.Contains(fp))).Distinct().ToList();

            if (!string.IsNullOrEmpty(search))
            {
                data = data.Where(b => b.Title != null && b.Title.ToLower().Contains(search.ToLower()) ||
                                  b.Author.ToLower().Contains(search.ToLower()) ||
                                  b.Subject.ToLower().Contains(search.ToLower()) ||
                                  b.BookPublisher.ToLower().Contains(search.ToLower())).ToList();
            }
            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                data = data.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                return data;
            }
            return data;
        }

        public Grade getGradeById(long id)
        {
            return _EFGradeRepository.GetById(b => b.Id == id && b.IsDeleted != true);
        }
    }
}
