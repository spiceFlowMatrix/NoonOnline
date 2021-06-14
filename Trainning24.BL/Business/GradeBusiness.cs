using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Grade;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class GradeBusiness
    {
        private readonly EFGradeRepository EFGradeRepository;

        public GradeBusiness(
            EFGradeRepository EFGradeRepository
        )
        {
            this.EFGradeRepository = EFGradeRepository;
        }

        public Grade Create(AddGradeModel AddGradeModel, int id)
        {
            Grade Grade = new Grade
            {
                Name = AddGradeModel.name,
                Description = AddGradeModel.description,
                SchoolId = AddGradeModel.schoolid,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };

            EFGradeRepository.Insert(Grade);

            return Grade;
        }

        public Grade Update(UpdateGradeModel UpdateGradeModel, int id)
        {
            Grade grade = EFGradeRepository.GetById(b => b.Id == UpdateGradeModel.id);
            grade.Name = UpdateGradeModel.name;
            grade.Description = UpdateGradeModel.description;
            grade.SchoolId= UpdateGradeModel.schoolid;
            grade.LastModificationTime = DateTime.Now.ToString();
            grade.LastModifierUserId = id;            

            if (EFGradeRepository.Update(grade) == 1)
            {
                return EFGradeRepository.GetById(b => b.Id == grade.Id);
            }
            else {
                return null;
            }                        
        }

        public List<Grade> GradeList(PaginationModel paginationModel, out int total)
        {
            List<Grade> GradeList = new List<Grade>();
            GradeList = EFGradeRepository.GetAll();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = GradeList.Count();

                if (!string.IsNullOrEmpty(paginationModel.search))
                {
                    GradeList = GradeList.Where(b => b.Name.Any(k => b.Name.ToLower().Contains(paginationModel.search.ToLower())) ||
                                 b.Id.ToString().Any(k => b.Id.ToString().ToLower().Contains(paginationModel.search.ToLower()))).OrderByDescending(b => b.Id).ToList();

                    total = GradeList.Count();

                    GradeList = GradeList.OrderByDescending(b => b.Id).
                                Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                                Take(paginationModel.perpagerecord).
                                ToList();


                    return GradeList;
                }

                GradeList = GradeList.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();

                return GradeList;
            }

            total = GradeList.Count();
            return EFGradeRepository.GetAll();
        }

        public Grade getGradeById(long id)
        {
            return EFGradeRepository.GetById(b=>b.Id==id && b.IsDeleted != true);
        }

        public Grade Delete(int Id, int DeleterId)
        {
            Grade Grade = new Grade();
            Grade.Id = Id;
            Grade.DeleterUserId = DeleterId;
            int i = EFGradeRepository.Delete(Grade);
            Grade deletedGrade = EFGradeRepository.GetById(b => b.Id == Id);
            return deletedGrade;
        }

        public Grade getSchoolByGrade(long id)
        {
            return EFGradeRepository.GetById(b => b.Id == id && b.IsDeleted != true);
        }
    }
}
