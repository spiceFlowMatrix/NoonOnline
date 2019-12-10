using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.CourseDefination;
using Trainning24.BL.ViewModels.Deposit;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class DepositBusiness
    {
        private readonly EFDeposit EFDeposit;
        private readonly SalesAgentBusiness SalesAgentBusiness;
        private readonly AgentCategoryBusiness AgentCategoryBusiness;
        private readonly SubscriptionMetadataBusiness SubscriptionMetadataBusiness;
        private readonly SubscriptionsBusiness SubscriptionsBusiness;
        private readonly DiscountBusiness DiscountBusiness;
        private readonly CourseDefinationBusiness CourseDefinationBusiness;
        private readonly CourseBusiness CourseBusiness;
        private readonly GradeBusiness GradeBusiness;
        private readonly PackageBusiness _packageBusiness;
        private readonly AddtionalServicesBusiness _addtionalServicesBusiness;

        public DepositBusiness(
             EFDeposit EFDeposit,
             SubscriptionMetadataBusiness SubscriptionMetadataBusiness,
             AgentCategoryBusiness AgentCategoryBusiness,
             SubscriptionsBusiness SubscriptionsBusiness,
             GradeBusiness GradeBusiness,
             CourseBusiness CourseBusiness,
             DiscountBusiness DiscountBusiness,
             CourseDefinationBusiness CourseDefinationBusiness,
             SalesAgentBusiness SalesAgentBusiness,
             PackageBusiness packageBusiness,
             AddtionalServicesBusiness addtionalServicesBusiness
        )
        {
            this.EFDeposit = EFDeposit;
            this.SubscriptionMetadataBusiness = SubscriptionMetadataBusiness;
            this.AgentCategoryBusiness = AgentCategoryBusiness;
            this.CourseBusiness = CourseBusiness;
            this.SubscriptionsBusiness = SubscriptionsBusiness;
            this.GradeBusiness = GradeBusiness;
            this.DiscountBusiness = DiscountBusiness;
            this.CourseDefinationBusiness = CourseDefinationBusiness;
            this.SalesAgentBusiness = SalesAgentBusiness;
            _packageBusiness = packageBusiness;
            _addtionalServicesBusiness = addtionalServicesBusiness;
        }

        public Deposit Create(Deposit Deposit, int id)
        {
            Deposit.IsDeleted = false;
            Deposit.CreationTime = DateTime.Now.ToString();
            Deposit.CreatorUserId = id;

            EFDeposit.Insert(Deposit);

            return Deposit;
        }

        public Deposit Update(Deposit Dpt, int id)
        {
            Deposit Deposit = EFDeposit.GetById(b => b.Id == Dpt.Id);
            Deposit.DepositAmount = Dpt.DepositAmount;

            if (!string.IsNullOrEmpty(Dpt.DocumentIds))
            {
                Deposit.DocumentIds = Dpt.DocumentIds;
            }

            Deposit.DepositDate = Dpt.DepositDate;
            Deposit.SalesAgentId = Dpt.SalesAgentId;
            Deposit.IsRevoke = Dpt.IsRevoke;
            Deposit.IsConfirm = Dpt.IsConfirm;
            Deposit.LastModificationTime = DateTime.Now.ToString();
            Deposit.LastModifierUserId = id;

            EFDeposit.Update(Deposit);
            return Deposit;
        }

        public List<Deposit> DepositList(PaginationModel paginationModel, out int total)
        {
            List<Deposit> DepositList = new List<Deposit>();
            DepositList = EFDeposit.GetAll();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = DepositList.Count();

                DepositList = DepositList.OrderByDescending(b => b.Id).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();

                return DepositList;
            }

            total = DepositList.Count();
            return EFDeposit.GetAll();
        }

        public Deposit getGradeById(long id)
        {
            return EFDeposit.GetById(b => b.Id == id && b.IsDeleted != true);
        }

        public Deposit Delete(int Id, int DeleterId)
        {
            Deposit Deposit = new Deposit();
            Deposit.Id = Id;
            Deposit.DeleterUserId = DeleterId;
            int i = EFDeposit.Delete(Deposit);
            Deposit deletedDeposit = EFDeposit.GetById(b => b.Id == Id);
            return deletedDeposit;
        }


        public List<AgentAccountSummary> GetAccountSummary()
        {
            List<SalesAgent> allAgent = SalesAgentBusiness.AllSalesAgentList();

            var depositList = EFDeposit.GetAll().GroupBy(b => b.SalesAgentId).ToList();

            List<AgentAccountSummary> agentAccountSummaries = new List<AgentAccountSummary>();

            foreach (var agent in allAgent)
            {
                AgentAccountSummary agentAccountSummary = GetAgentBalanceSummary(agent.Id);

                //AgentCategory agentc = AgentCategoryBusiness.getAgentCategoryById(agent.AgentCategoryId);

                //int commission = agentc.Commission;

                //agentAccountSummary.totalexpenditure = agentAccountSummary.totalexpenditure - (agentAccountSummary.totalexpenditure * commission) / 100;

                agentAccountSummaries.Add(agentAccountSummary);
            }

            return agentAccountSummaries;
        }

        public AgentAccountSummary GetAgentBalanceSummary(long id)
        {
            var depositList = EFDeposit.GetAll().Where(b => b.SalesAgentId == id).ToList();
            AgentAccountSummary agentAccountSummary = new AgentAccountSummary();
            agentAccountSummary.agentid = id;
            agentAccountSummary.agentname = SalesAgentBusiness.getSalesAgentById(id).AgentName;

            depositList = depositList.Where(b => b.IsConfirm == true).ToList();

            if (depositList.Count != 0)
            {
                agentAccountSummary.totaldeposit = depositList.Where(b => b.IsConfirm == true && b.IsRevoke == false).Select(b => b.DepositAmount).Sum();
                agentAccountSummary.depositcount = depositList.Where(b => b.IsConfirm == true && b.IsRevoke == false).Count();
                agentAccountSummary.latestdeposit = depositList.OrderByDescending(b => b.Id).FirstOrDefault().DepositAmount;
            }

            //foreach (var deposit in depositList)
            //{
            List<SubscriptionMetadata> subscriptionMetadataList = SubscriptionMetadataBusiness.SubscriptionMetaDataList(id);

            if (subscriptionMetadataList.Count() != 0)
            {
                decimal finalpriced = 0;

                foreach (var subscriptionMetadata in subscriptionMetadataList)
                {
                    if (subscriptionMetadata.Status == "Completed")
                    {
                        decimal finalprice = 0;

                        decimal totalbaseprice = 0;

                        Subscriptions Subscriptions = SubscriptionsBusiness.GetSubscriptionsByMetadataId(subscriptionMetadata.Id);

                        if (Subscriptions != null)
                        {
                            string[] uids = new string[] { };
                            if (!string.IsNullOrEmpty(Subscriptions.UserId))
                            {
                                uids = Subscriptions.UserId.Split(",");
                            }
                            string totalsubscriptions = uids.Length.ToString();

                            agentAccountSummary.purchasecount = subscriptionMetadataList.Count();

                            Discount discount = DiscountBusiness.getDiscountById(subscriptionMetadata.DiscountPackageId);

                            string[] coursedefinationids = new string[] { };
                            string[] additionalserviceids = new string[] { };

                            if (!string.IsNullOrEmpty(subscriptionMetadata.CourseId))
                            {
                                coursedefinationids = subscriptionMetadata.CourseId.Split(",");
                            }

                            if (!string.IsNullOrEmpty(subscriptionMetadata.ServiceId))
                            {
                                additionalserviceids = subscriptionMetadata.ServiceId.Split(",");
                            }

                            List<ResponseCourseDefination> ResponseCourseDefinationList = new List<ResponseCourseDefination>();

                            for (int i = 0; i < coursedefinationids.Length; i++)
                            {
                                CourseDefination courseDefination = CourseDefinationBusiness.getCourseDefinationById(int.Parse(coursedefinationids[i]));
                                if (courseDefination != null)
                                {
                                    totalbaseprice += decimal.Parse(courseDefination.BasePrice);
                                }
                            }

                            if (subscriptionMetadata.PackageId != 0)
                            {
                                Package package = _packageBusiness.GetById(subscriptionMetadata.PackageId);
                                if (package != null)
                                {
                                    totalbaseprice += decimal.Parse(package.Price);
                                }
                            }

                            for (int i = 0; i < additionalserviceids.Length; i++)
                            {
                                AddtionalServices addtionalServices = _addtionalServicesBusiness.GetById(int.Parse(additionalserviceids[i]));
                                if (addtionalServices != null)
                                {
                                    totalbaseprice += decimal.Parse(addtionalServices.Price);
                                }
                            }


                            decimal ftotalbaseprice = totalbaseprice * decimal.Parse(totalsubscriptions);

                            if (subscriptionMetadata.NoOfMonths > 0)
                            {
                                ftotalbaseprice = ftotalbaseprice * subscriptionMetadata.NoOfMonths;
                                if (decimal.Parse(totalsubscriptions) > 0)
                                {
                                    totalbaseprice = ftotalbaseprice / decimal.Parse(totalsubscriptions);
                                }

                            }

                            int ftotalsubscription = 0;

                            if (discount != null)
                            {
                                if (int.Parse(totalsubscriptions) < discount.FreeSubscriptions)
                                {
                                    ftotalsubscription = 0;
                                }
                                else
                                {
                                    ftotalsubscription = int.Parse(totalsubscriptions) - discount.FreeSubscriptions;
                                }

                                if (ftotalsubscription != 0)
                                {
                                    ftotalbaseprice = totalbaseprice * ftotalsubscription;

                                    finalprice = ftotalbaseprice - ftotalbaseprice * (ftotalsubscription * discount.OffSubscriptions) / 100; // this is discount for discounted off subscriptions

                                    finalprice = finalprice - (finalprice * discount.OffTotalPrice) / 100; // this is discount for discounted off total price 
                                }
                                else
                                {
                                    finalprice = totalbaseprice * (ftotalsubscription * discount.OffTotalPrice) / 100;
                                }
                            }
                            else
                            {
                                ftotalbaseprice = totalbaseprice;

                                finalprice = ftotalbaseprice;
                            }
                            finalpriced += finalprice;
                        }
                    }

                    agentAccountSummary.totalexpenditure = finalpriced;

                    SalesAgent salesAgent = SalesAgentBusiness.getSalesAgentById(id);

                    AgentCategory agentc = AgentCategoryBusiness.getAgentCategoryById(salesAgent.AgentCategoryId);

                    int commission = agentc.Commission;

                    decimal agentCommission = agentAccountSummary.totalexpenditure * commission / 100;

                    decimal agentTaxDeduct = agentCommission * subscriptionMetadata.Tax / 100;

                    decimal finalCommissionAmount = agentCommission - agentTaxDeduct;

                    agentAccountSummary.totalexpenditure = agentAccountSummary.totalexpenditure - finalCommissionAmount;

                    agentAccountSummary.currentbalance = agentAccountSummary.totaldeposit - agentAccountSummary.totalexpenditure;
                }
            }
            else
            {
                agentAccountSummary.currentbalance = agentAccountSummary.totaldeposit - agentAccountSummary.totalexpenditure;
            }
            //}

            return agentAccountSummary;
        }

        public AgentAccountSummary GetAgentSummaryDetails(long id)
        {
            var depositList = EFDeposit.GetAll().Where(b => b.SalesAgentId == id).ToList();
            AgentAccountSummary agentAccountSummary = new AgentAccountSummary();
            agentAccountSummary.agentid = id;
            agentAccountSummary.agentname = SalesAgentBusiness.getSalesAgentById(id).AgentName;

            depositList = depositList.Where(b => b.IsConfirm == true).ToList();

            if (depositList.Count != 0)
            {
                agentAccountSummary.totaldeposit = depositList.Where(b => b.IsConfirm == true && b.IsRevoke == false).Select(b => b.DepositAmount).Sum();
                agentAccountSummary.depositcount = depositList.Where(b => b.IsConfirm == true && b.IsRevoke == false).Count();
                agentAccountSummary.latestdeposit = depositList.OrderByDescending(b => b.Id).FirstOrDefault().DepositAmount;
            }

            //foreach (var deposit in depositList)
            //{
            List<SubscriptionMetadata> subscriptionMetadataList = SubscriptionMetadataBusiness.SubscriptionMetaDataList(id);

            if (subscriptionMetadataList.Count() != 0)
            {
                decimal finalpriced = 0;

                foreach (var subscriptionMetadata in subscriptionMetadataList)
                {
                    if (subscriptionMetadata.Status == "Completed")
                    {
                        decimal finalprice = 0;

                        decimal totalbaseprice = 0;

                        Subscriptions Subscriptions = SubscriptionsBusiness.GetSubscriptionsByMetadataId(subscriptionMetadata.Id);

                        if (Subscriptions != null)
                        {
                            string[] uids = new string[] { };
                            if (!string.IsNullOrEmpty(Subscriptions.UserId))
                            {
                                uids = Subscriptions.UserId.Split(",");
                            }
                            string totalsubscriptions = uids.Length.ToString();

                            agentAccountSummary.purchasecount = subscriptionMetadataList.Count();

                            Discount discount = DiscountBusiness.getDiscountById(subscriptionMetadata.DiscountPackageId);

                            string[] coursedefinationids = new string[] { };
                            string[] additionalserviceids = new string[] { };

                            if (!string.IsNullOrEmpty(subscriptionMetadata.CourseId))
                            {
                                coursedefinationids = subscriptionMetadata.CourseId.Split(",");
                            }

                            if (!string.IsNullOrEmpty(subscriptionMetadata.ServiceId))
                            {
                                additionalserviceids = subscriptionMetadata.ServiceId.Split(",");
                            }

                            List<ResponseCourseDefination> ResponseCourseDefinationList = new List<ResponseCourseDefination>();

                            for (int i = 0; i < coursedefinationids.Length; i++)
                            {
                                CourseDefination courseDefination = CourseDefinationBusiness.getCourseDefinationById(int.Parse(coursedefinationids[i]));
                                if (courseDefination != null)
                                {
                                    totalbaseprice += decimal.Parse(courseDefination.BasePrice);
                                }
                            }

                            if (subscriptionMetadata.PackageId != 0)
                            {
                                Package package = _packageBusiness.GetById(subscriptionMetadata.PackageId);
                                if (package != null)
                                {
                                    totalbaseprice += decimal.Parse(package.Price);
                                }
                            }

                            for (int i = 0; i < additionalserviceids.Length; i++)
                            {
                                AddtionalServices addtionalServices = _addtionalServicesBusiness.GetById(int.Parse(additionalserviceids[i]));
                                if (addtionalServices != null)
                                {
                                    totalbaseprice += decimal.Parse(addtionalServices.Price);
                                }
                            }


                            decimal ftotalbaseprice = totalbaseprice * decimal.Parse(totalsubscriptions);

                            if (subscriptionMetadata.NoOfMonths > 0)
                            {
                                ftotalbaseprice = ftotalbaseprice * subscriptionMetadata.NoOfMonths;
                                if (decimal.Parse(totalsubscriptions) > 0)
                                {
                                    totalbaseprice = ftotalbaseprice / decimal.Parse(totalsubscriptions);
                                }

                            }

                            int ftotalsubscription = 0;

                            if (discount != null)
                            {
                                if (int.Parse(totalsubscriptions) < discount.FreeSubscriptions)
                                {
                                    ftotalsubscription = 0;
                                }
                                else
                                {
                                    ftotalsubscription = int.Parse(totalsubscriptions) - discount.FreeSubscriptions;
                                }

                                if (ftotalsubscription != 0)
                                {
                                    ftotalbaseprice = totalbaseprice * ftotalsubscription;

                                    finalprice = ftotalbaseprice - ftotalbaseprice * (ftotalsubscription * discount.OffSubscriptions) / 100; // this is discount for discounted off subscriptions

                                    finalprice = finalprice - (finalprice * discount.OffTotalPrice) / 100; // this is discount for discounted off total price 
                                }
                                else
                                {
                                    finalprice = totalbaseprice * (ftotalsubscription * discount.OffTotalPrice) / 100;
                                }
                            }
                            else
                            {
                                ftotalbaseprice = totalbaseprice;

                                finalprice = ftotalbaseprice;
                            }
                            finalpriced += finalprice;
                        }
                    }

                    agentAccountSummary.totalexpenditure = finalpriced;

                    SalesAgent salesAgent = SalesAgentBusiness.getSalesAgentById(id);

                    AgentCategory agentc = AgentCategoryBusiness.getAgentCategoryById(salesAgent.AgentCategoryId);

                    int commission = agentc.Commission;

                    agentAccountSummary.totalexpenditure = agentAccountSummary.totalexpenditure - (agentAccountSummary.totalexpenditure * commission) / 100;

                    agentAccountSummary.currentbalance = agentAccountSummary.totaldeposit - agentAccountSummary.totalexpenditure;
                }
            }
            else
            {
                agentAccountSummary.currentbalance = agentAccountSummary.totaldeposit - agentAccountSummary.totalexpenditure;
            }
            //}

            return agentAccountSummary;
        }
    }
}
