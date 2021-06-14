using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.School;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class SchoolBusiness
    {
        private readonly EFSchoolRepository EFSchoolRepository;

        public SchoolBusiness(
            EFSchoolRepository EFSchoolRepository
        )
        {
            this.EFSchoolRepository = EFSchoolRepository;
        }

        public School Create(AddSchoolModel AddSchoolModel, int id)
        {
            School School = new School
            {
                Name = AddSchoolModel.name,
                Code = AddSchoolModel.code,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };

            EFSchoolRepository.Insert(School);

            return School;
        }

        public School Update(UpdateSchoolModel UpdateSchoolModel, int id)
        {
            School School = EFSchoolRepository.GetById(b => b.Id == UpdateSchoolModel.id);
            School.Name = UpdateSchoolModel.name;
            School.Code = UpdateSchoolModel.code;
            School.LastModificationTime = DateTime.Now.ToString();
            School.LastModifierUserId = id;

            if (EFSchoolRepository.Update(School) == 1)
            {
                return EFSchoolRepository.GetById(b => b.Id == School.Id);
            }
            else
            {
                return null;
            }
        }

        public List<School> SchoolList(PaginationModel paginationModel, out int total)
        {
            List<School> SchoolList = new List<School>();
            SchoolList = EFSchoolRepository.GetAll();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = SchoolList.Count();

                if (!string.IsNullOrEmpty(paginationModel.search))
                {
                    SchoolList = SchoolList.Where(b => b.Name.Any(k => b.Name.ToLower().Contains(paginationModel.search.ToLower())) ||                                 
                                 b.Id.ToString().Any(k => b.Id.ToString().ToLower().Contains(paginationModel.search.ToLower()))).OrderByDescending(b => b.Id).ToList();


                    total = SchoolList.Count();

                    SchoolList = SchoolList.OrderByDescending(b => b.Id).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();


                    return SchoolList;


                }

                SchoolList = SchoolList.OrderByDescending(b => b.Id).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();

                return SchoolList;
            }

            total = SchoolList.Count();
            return EFSchoolRepository.GetAll();
        }

        public School getSchoolById(long id)
        {
            return EFSchoolRepository.GetById(b => b.Id == id && b.IsDeleted != true);
        }

        public School Delete(int Id, int DeleterId)
        {
            School School = new School();
            School.Id = Id;
            School.DeleterUserId = DeleterId;
            int i = EFSchoolRepository.Delete(School);
            School deletedSchool = EFSchoolRepository.GetById(b => b.Id == Id);
            return deletedSchool;
        }
    }
}
