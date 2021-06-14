using System;
using System.Collections.Generic;
using System.Linq;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFRolesRepository
    {
        private readonly EFGenericRepository<Role> _context;
        //private static Training24Context _updateContext;

        public EFRolesRepository
        (
            EFGenericRepository<Role> context
            //Training24Context training24Context
        )
        {
            _context = context;
            //_updateContext = training24Context;
        }

        public int Insert(Role obj)
        {
            return _context.Insert(obj);
        }

        public int Update(Role obj)
        {
            Role role = _context.ListQuery(b => b.Id == obj.Id).SingleOrDefault();

            role.Name = obj.Name;
            role.LastModificationTime = DateTime.Now.ToString();
            role.LastModifierUserId = 1;

            return _context.Update(role);
        }

        public int Delete(Role obj, string Id)
        {
            return _context.Delete(obj, Id);
        }

        public Role RoleExist(Role role)
        {
            Role result = null;

            try
            {
                result = _context.ListQuery(b => b.Name == role.Name).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public Role RoleExistById(Role role)
        {
            Role result = null;

            try
            {
                result = _context.ListQuery(b => b.Id == role.Id).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public List<Role> GetAll()
        {
            return _context.GetAll().Where(b => b.IsDeleted == false).ToList();
        }

        public List<Role> GetAllPageWise(int pagenumber, int perpagerecord)
        {
            if (pagenumber != 0 && perpagerecord != 0)
            {
                return _context.ListQuery(b => b.IsDeleted != true).
                        OrderByDescending(b => b.CreationTime).Skip(perpagerecord * (pagenumber - 1)).
                        Take(perpagerecord).ToList();
            }
            return _context.GetAll().OrderBy(b => b.CreationTime).ToList();
        }
    }
}
