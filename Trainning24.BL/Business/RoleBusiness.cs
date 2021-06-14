using System;
using System.Collections.Generic;
using System.Linq;
using Trainning24.BL.ViewModels.Roles;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class RoleBusiness
    {
        private readonly EFRolesRepository _EFRolesRepository;

        public RoleBusiness
        (
            EFRolesRepository eFRolesRepository
        )
        {
            _EFRolesRepository = eFRolesRepository;
        }

        public Role Create(CreateRoleViewModel createRoleViewModel, string Id)
        {
            Role newRole = new Role
            {
                Name = createRoleViewModel.Name,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = int.Parse(Id),
                IsDeleted = false,
                DeleterUserId = 0,
                DeletionTime = DateTime.Now.ToString(),
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = 0
            };

            newRole.Id = _EFRolesRepository.Insert(newRole);

            return newRole;
        }

        public Role Update(CreateRoleViewModel createRoleViewModel, string Id)
        {
            Role newRole = new Role
            {
                Id = createRoleViewModel.Id,
                Name = createRoleViewModel.Name,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = 0,
                IsDeleted = false,
                DeleterUserId = int.Parse(Id),
                DeletionTime = DateTime.Now.ToString(),
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = 0
            };

            _EFRolesRepository.Update(newRole);

            return newRole;
        }

        public Role Delete(Role obj, string Id)
        {
            _EFRolesRepository.Delete(obj, Id);
            return obj;
        }

        public Role RoleExistance(CreateRoleViewModel createRoleViewModel)
        {
            Role _role = new Role
            {
                Name = createRoleViewModel.Name
            };

            return _EFRolesRepository.RoleExist(_role);
        }

        public Role RoleExistanceById(int Id)
        {
            Role _role = new Role
            {
                Id = Id
            };

            return _EFRolesRepository.RoleExistById(_role);
        }

        public List<Role> RoleList(PaginationModel paginationModel)
        {
            return _EFRolesRepository.GetAllPageWise(paginationModel.pagenumber, paginationModel.perpagerecord);
        }

        public int TotalRoleCount()
        {
            return _EFRolesRepository.GetAll().Count();
        }
    }
}
