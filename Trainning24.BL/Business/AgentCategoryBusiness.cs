using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class AgentCategoryBusiness
    {

        private readonly EFAgentCategoryBusinessRepository EFAgentCategoryBusinessRepository;


        public AgentCategoryBusiness(
            EFAgentCategoryBusinessRepository EFAgentCategoryBusinessRepository
        )
        {
            this.EFAgentCategoryBusinessRepository = EFAgentCategoryBusinessRepository;
        }

        public AgentCategory Create(AgentCategory AgentCategory, int id)
        {
            AgentCategory agentCategory = EFAgentCategoryBusinessRepository.ListQuery(b => b.CategoryName == AgentCategory.CategoryName).SingleOrDefault();
            if(agentCategory != null)
            {
                return null;
            }
            AgentCategory.CreationTime = DateTime.Now.ToString();
            AgentCategory.CreatorUserId = id;
            AgentCategory.IsDeleted = false;

            EFAgentCategoryBusinessRepository.Update(AgentCategory);
            return EFAgentCategoryBusinessRepository.GetById(b => b.Id == AgentCategory.Id);
        }

        public AgentCategory Update(AgentCategory AgentCategory, int id)
        {
            AgentCategory agentCategory = EFAgentCategoryBusinessRepository.GetById(b => b.Id == AgentCategory.Id);
            agentCategory.CategoryName = AgentCategory.CategoryName;
            agentCategory.Commission = AgentCategory.Commission;
            agentCategory.LastModificationTime = DateTime.Now.ToString();
            agentCategory.LastModifierUserId = id;
            EFAgentCategoryBusinessRepository.Update(agentCategory);
            return EFAgentCategoryBusinessRepository.GetById(b => b.Id == AgentCategory.Id);
        }

        public List<AgentCategory> AgentCategoryList(PaginationModel paginationModel, out int total)
        {
            List<AgentCategory> AgentCategoryList = new List<AgentCategory>();
            AgentCategoryList = EFAgentCategoryBusinessRepository.GetAll().OrderByDescending(b => b.Id).ToList();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = AgentCategoryList.Count();

                AgentCategoryList = AgentCategoryList.OrderByDescending(b => b.Id).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();

                if (!string.IsNullOrEmpty(paginationModel.search))
                    AgentCategoryList = AgentCategoryList.OrderByDescending(b => b.Id).Where(
                                                       b => b.Id.ToString().Any(k => b.Id.ToString().Contains(paginationModel.search))
                                                    || b.CategoryName.ToString().ToLower().Any(k => b.CategoryName.ToString().Contains(paginationModel.search))
                                                    || b.Commission.ToString().ToLower().Any(k => b.Commission.ToString().Contains(paginationModel.search))
                                                    ).ToList();

                return AgentCategoryList;
            }

            total = AgentCategoryList.Count();
            return AgentCategoryList;
        }

        public AgentCategory getAgentCategoryById(long id)
        {
            return EFAgentCategoryBusinessRepository.GetById(b => b.Id == id);
        }

        public AgentCategory Delete(int Id, int DeleterId)
        {
            AgentCategory AgentCategory = new AgentCategory();
            AgentCategory.Id = Id;
            AgentCategory.DeleterUserId = DeleterId;
            EFAgentCategoryBusinessRepository.Delete(AgentCategory);
            AgentCategory deletedAgentCategory = EFAgentCategoryBusinessRepository.GetById(b => b.Id == Id);
            return deletedAgentCategory;
        }
    }
}
