using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class SalesAgentBusiness
    {
        private readonly EFSalesAgentBusinessRepository EFSalesAgentBusinessRepository;

        public SalesAgentBusiness(
            EFSalesAgentBusinessRepository EFSalesAgentBusinessRepository
        )
        {
            this.EFSalesAgentBusinessRepository = EFSalesAgentBusinessRepository;
        }

        public List<Currency> AddCurrency(List<Currency> currencies)
        {
            foreach (var Currency in currencies)
            {
                EFSalesAgentBusinessRepository.InsertCurrency(Currency);
            }
            return currencies;
        }

        public List<Currency> getAllCurrency()
        {
            return EFSalesAgentBusinessRepository.getAllCurrency();
        }

        public SalesAgent Create(SalesAgent SalesAgent, int id)
        {
            SalesAgent.CreationTime = DateTime.Now.ToString();
            SalesAgent.CreatorUserId = id;
            SalesAgent.IsDeleted = false;
            EFSalesAgentBusinessRepository.Insert(SalesAgent);
            return SalesAgent;
        }

        public SalesAgent Update(SalesAgent SalesAgent, int id)
        {
            SalesAgent salesAgent = EFSalesAgentBusinessRepository.GetById(b => b.Email == SalesAgent.Email && b.IsDeleted != true);
            if (salesAgent != null)
            {
                salesAgent.AgentName = SalesAgent.AgentName;
                salesAgent.AgentCategoryId = SalesAgent.AgentCategoryId;
                salesAgent.Email = SalesAgent.Email;
                salesAgent.Phone = SalesAgent.Phone;
                salesAgent.PartnerBackgroud = SalesAgent.PartnerBackgroud;
                salesAgent.FocalPoint = SalesAgent.FocalPoint;
                salesAgent.Position = SalesAgent.Position;
                salesAgent.PhysicalAddress = SalesAgent.PhysicalAddress;
                salesAgent.CurrencyCode = SalesAgent.CurrencyCode;
                salesAgent.AgreedMonthlyDeposit = SalesAgent.AgreedMonthlyDeposit;
                salesAgent.IsActive = SalesAgent.IsActive;
                salesAgent.UserId = SalesAgent.UserId;
                salesAgent.LastModificationTime = DateTime.Now.ToString();
                salesAgent.LastModifierUserId = id;

                EFSalesAgentBusinessRepository.Update(salesAgent);
                return salesAgent;
                //return EFSalesAgentBusinessRepository.GetById(b => b.Id == SalesAgent.Id);
            }
            else
            {
                return null;
            }
        }

        public List<SalesAgent> SalesAgentList(PaginationModel paginationModel, out int total)
        {
            List<SalesAgent> SalesAgentList = new List<SalesAgent>();
            SalesAgentList = EFSalesAgentBusinessRepository.GetAll().OrderByDescending(b => b.Id).ToList();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = SalesAgentList.Count();

                SalesAgentList = SalesAgentList.OrderByDescending(b => b.Id).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();

                if (!string.IsNullOrEmpty(paginationModel.search))
                    SalesAgentList = SalesAgentList.OrderByDescending(b => b.Id).Where(
                                                       b => b.Id.ToString().Any(k => b.Id.ToString().Contains(paginationModel.search))
                                                    || b.AgentName.ToString().ToLower().Any(k => b.AgentName.ToString().Contains(paginationModel.search))
                                                    || b.Email.ToString().ToLower().Any(k => b.Email.ToString().Contains(paginationModel.search))
                                                    ).ToList();

                return SalesAgentList;
            }

            total = SalesAgentList.Count();
            return SalesAgentList;
        }


        public List<SalesAgent> AllSalesAgentList()
        {
            return EFSalesAgentBusinessRepository.GetAll();
        }

        public SalesAgent getSalesAgentById(long id)
        {
            return EFSalesAgentBusinessRepository.GetById(b => b.Id == id);
        }

        public SalesAgent getSalesAgentByUserId(long id)
        {
            return EFSalesAgentBusinessRepository.GetById(b => b.UserId == id && b.IsDeleted == false);
        }

        public SalesAgent Delete(int Id, int DeleterId)
        {
            SalesAgent SalesAgent = new SalesAgent();
            SalesAgent.Id = Id;
            SalesAgent.DeleterUserId = DeleterId;
            EFSalesAgentBusinessRepository.Delete(SalesAgent);
            //SalesAgent deletedSalesAgent = EFSalesAgentBusinessRepository.GetById(b => b.Id == Id);
            return SalesAgent;
        }

        public SalesAgent DeleteByAdmin(long Id, int DeleterId)
        {
            SalesAgent SalesAgent = new SalesAgent();
            SalesAgent.Id = Id;
            SalesAgent.DeleterUserId = DeleterId;
            EFSalesAgentBusinessRepository.Delete(SalesAgent);
            //SalesAgent deletedSalesAgent = EFSalesAgentBusinessRepository.GetById(b => b.Id == Id);
            return SalesAgent;
        }

        public SalesAgent GetAgentByEmail(string email)
        {
            return EFSalesAgentBusinessRepository.GetById(b => b.Email == email && b.IsDeleted != true);
        }

        public SalesAgent GetAgentById(long Id)
        {
            return EFSalesAgentBusinessRepository.GetById(b => b.Id == Id && b.IsDeleted != true);
        }

        public SalesAgent GetAgentByUserId(int Id)
        {
            return EFSalesAgentBusinessRepository.GetById(b => b.UserId == Id && b.IsDeleted != true);
        }
    }
}
