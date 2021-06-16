using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.SubscriptionMetadata;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class SubscriptionMetadataBusiness
    {
        private readonly EFSubscriptionMetadata EFSubscriptionMetadata;
        private readonly SubscriptionsBusiness SubscriptionsBusiness;
        private readonly CourseDefinationBusiness CourseDefinationBusiness;
        private readonly DiscountBusiness DiscountBusiness;
        private readonly PackageBusiness _packageBusiness;
        private readonly AddtionalServicesBusiness _addtionalServicesBusiness;
        public SubscriptionMetadataBusiness(
            EFSubscriptionMetadata EFSubscriptionMetadata,
            SubscriptionsBusiness SubscriptionsBusiness,
            DiscountBusiness DiscountBusiness,
            CourseDefinationBusiness CourseDefinationBusiness,
            PackageBusiness packageBusiness,
            AddtionalServicesBusiness addtionalServicesBusiness
        )
        {
            this.EFSubscriptionMetadata = EFSubscriptionMetadata;
            this.DiscountBusiness = DiscountBusiness;
            this.SubscriptionsBusiness = SubscriptionsBusiness;
            this.CourseDefinationBusiness = CourseDefinationBusiness;
            _packageBusiness = packageBusiness;
            _addtionalServicesBusiness = addtionalServicesBusiness;
        }

        public SubscriptionMetadata Create(SubscriptionMetadata SubscriptionMetadata, int id)
        {
            SubscriptionMetadata.CreationTime = DateTime.Now.ToString();
            SubscriptionMetadata.CreatorUserId = id;
            SubscriptionMetadata.IsDeleted = false;
            EFSubscriptionMetadata.Insert(SubscriptionMetadata);
            return EFSubscriptionMetadata.GetById(b => b.Id == SubscriptionMetadata.Id);
        }

        public SubscriptionMetadata Update(SubscriptionMetadata SubscriptionMetadata, int id)
        {
            SubscriptionMetadata subscriptionMetadata = EFSubscriptionMetadata.GetById(b => b.Id == SubscriptionMetadata.Id);

            subscriptionMetadata.CourseId = SubscriptionMetadata.CourseId;
            subscriptionMetadata.EnrollmentFromDate = SubscriptionMetadata.EnrollmentFromDate;
            subscriptionMetadata.EnrollmentToDate = SubscriptionMetadata.EnrollmentToDate;
            subscriptionMetadata.SalesAgentId = SubscriptionMetadata.SalesAgentId;
            subscriptionMetadata.Status = SubscriptionMetadata.Status;
            subscriptionMetadata.SubscriptionTypeId = SubscriptionMetadata.SubscriptionTypeId;
            subscriptionMetadata.NoOfMonths = SubscriptionMetadata.NoOfMonths;
            subscriptionMetadata.PackageId = SubscriptionMetadata.PackageId;
            subscriptionMetadata.ServiceId = SubscriptionMetadata.ServiceId;
            subscriptionMetadata.DiscountPackageId = SubscriptionMetadata.DiscountPackageId;
            subscriptionMetadata.Tax = SubscriptionMetadata.Tax;
            subscriptionMetadata.LastModificationTime = DateTime.Now.ToString();
            subscriptionMetadata.LastModifierUserId = id;

            EFSubscriptionMetadata.Update(subscriptionMetadata);
            //return EFSubscriptionMetadata.GetById(b => b.Id == SubscriptionMetadata.Id);
            return subscriptionMetadata;
        }

        public List<SubscriptionMetadata> GetPurchaseList(string search)
        {
            List<SubscriptionMetadata> subscriptionMetadatas = EFSubscriptionMetadata.GetAll();
            if (!string.IsNullOrEmpty(search))
            {
                subscriptionMetadatas = EFSubscriptionMetadata.GetPurchaseList(search);
            }
            return subscriptionMetadatas;
        }


        public async Task<List<SubscriptionMetadata>> GetPurchaseListTest(PurchaseListFilterModel purchaseListFilterModel)
        {
            var subscriptionMetadatas = await EFSubscriptionMetadata.GetAllAsync();

            if (purchaseListFilterModel.isfilterneeded == true)
            {
                if (!string.IsNullOrEmpty(purchaseListFilterModel.courseid))
                {
                    subscriptionMetadatas = subscriptionMetadatas.FindAll(b => b.CourseId == purchaseListFilterModel.courseid);
                }
                if (purchaseListFilterModel.discountpackageid != 0)
                {
                    subscriptionMetadatas = subscriptionMetadatas.FindAll(b => b.DiscountPackageId == purchaseListFilterModel.discountpackageid);
                }
                if (purchaseListFilterModel.salesagentid != 0)
                {
                    subscriptionMetadatas = subscriptionMetadatas.FindAll(b => b.SalesAgentId == purchaseListFilterModel.salesagentid);
                }
                if (!string.IsNullOrEmpty(purchaseListFilterModel.status))
                {
                    subscriptionMetadatas = subscriptionMetadatas.FindAll(b => b.Status == purchaseListFilterModel.status);
                }
                //if (!string.IsNullOrEmpty(purchaseListFilterModel.enrollmentfromdate) && !string.IsNullOrEmpty(purchaseListFilterModel.enrollmenttodate))
                //{
                //    subscriptionMetadatas = subscriptionMetadatas.FindAll(b => b.Status == purchaseListFilterModel.status);
                //}
                if (!string.IsNullOrEmpty(purchaseListFilterModel.enrollmentfromdate) && !string.IsNullOrEmpty(purchaseListFilterModel.enrollmenttodate))
                {
                    subscriptionMetadatas = subscriptionMetadatas.FindAll(
                        b => DateTime.ParseExact(b.EnrollmentFromDate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture) >= DateTime.ParseExact(purchaseListFilterModel.enrollmentfromdate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture)
                        && DateTime.ParseExact(b.EnrollmentFromDate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture) <= DateTime.ParseExact(purchaseListFilterModel.enrollmenttodate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture));
                }
                return subscriptionMetadatas;
            }
            else
            {
                return subscriptionMetadatas;
            }
        }

        public async Task<List<SubscriptionMetadata>> GetPurchaseListSalesAgent(PurchaseListFilterModel purchaseListFilterModel, long agentId)
        {
            var subscriptionMetadatas = await EFSubscriptionMetadata.GetByAgentIdAsync(b => b.SalesAgentId == agentId && b.IsDeleted != null);

            if (purchaseListFilterModel.isfilterneeded == true)
            {
                if (!string.IsNullOrEmpty(purchaseListFilterModel.courseid))
                {
                    subscriptionMetadatas = subscriptionMetadatas.FindAll(b => b.CourseId == purchaseListFilterModel.courseid);
                }
                if (purchaseListFilterModel.discountpackageid != 0)
                {
                    subscriptionMetadatas = subscriptionMetadatas.FindAll(b => b.DiscountPackageId == purchaseListFilterModel.discountpackageid);
                }
                if (purchaseListFilterModel.salesagentid != 0)
                {
                    subscriptionMetadatas = subscriptionMetadatas.FindAll(b => b.SalesAgentId == purchaseListFilterModel.salesagentid);
                }
                if (!string.IsNullOrEmpty(purchaseListFilterModel.status))
                {
                    subscriptionMetadatas = subscriptionMetadatas.FindAll(b => b.Status == purchaseListFilterModel.status);
                }
                //if (!string.IsNullOrEmpty(purchaseListFilterModel.enrollmentfromdate) && !string.IsNullOrEmpty(purchaseListFilterModel.enrollmenttodate))
                //{
                //    subscriptionMetadatas = subscriptionMetadatas.FindAll(b => b.Status == purchaseListFilterModel.status);
                //}
                if (!string.IsNullOrEmpty(purchaseListFilterModel.enrollmentfromdate) && !string.IsNullOrEmpty(purchaseListFilterModel.enrollmenttodate))
                {
                    subscriptionMetadatas = subscriptionMetadatas.FindAll(
                        b => DateTime.ParseExact(b.EnrollmentFromDate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture) >= DateTime.ParseExact(purchaseListFilterModel.enrollmentfromdate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture)
                        && DateTime.ParseExact(b.EnrollmentFromDate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture) <= DateTime.ParseExact(purchaseListFilterModel.enrollmenttodate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture));
                }
                return subscriptionMetadatas;
            }
            else
            {
                return subscriptionMetadatas;
            }
        }


        public SubscriptionMetadata GetMetadataByid(long id)
        {
            return EFSubscriptionMetadata.GetById(b => b.Id == id);
        }


        public List<SubscriptionMetadata> SubscriptionMetaDataList(long id)
        {
            return EFSubscriptionMetadata.SubscriptionMetaDataList(b => b.SalesAgentId == id);
        }

        public async Task<AboutAllPurchase> PurchaseSummarySalesAgent(PurchaseListFilterModel purchaseListFilterModel, long agentId)
        {
            var allMetadatas = await EFSubscriptionMetadata.SubscriptionMetaDataListAsync(b => b.SalesAgentId == agentId && b.IsDeleted != true);

            if (purchaseListFilterModel.isfilterneeded == true)
            {
                if (!string.IsNullOrEmpty(purchaseListFilterModel.courseid))
                {
                    allMetadatas = allMetadatas.FindAll(b => b.CourseId == purchaseListFilterModel.courseid);
                }
                if (purchaseListFilterModel.discountpackageid != 0)
                {
                    allMetadatas = allMetadatas.FindAll(b => b.DiscountPackageId == purchaseListFilterModel.discountpackageid);
                }
                if (purchaseListFilterModel.salesagentid != 0)
                {
                    allMetadatas = allMetadatas.FindAll(b => b.SalesAgentId == purchaseListFilterModel.salesagentid);
                }
                if (!string.IsNullOrEmpty(purchaseListFilterModel.status))
                {
                    allMetadatas = allMetadatas.FindAll(b => b.Status == purchaseListFilterModel.status);
                }
                if (!string.IsNullOrEmpty(purchaseListFilterModel.enrollmentfromdate) && !string.IsNullOrEmpty(purchaseListFilterModel.enrollmenttodate))
                {
                    allMetadatas = allMetadatas.FindAll(
                        b => DateTime.ParseExact(b.EnrollmentFromDate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture) >= DateTime.ParseExact(purchaseListFilterModel.enrollmentfromdate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture)
                        && DateTime.ParseExact(b.EnrollmentFromDate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture) <= DateTime.ParseExact(purchaseListFilterModel.enrollmenttodate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture));
                }
            }

            AboutAllPurchase aboutAllPurchase = new AboutAllPurchase();

            int noofsubscription = 0;
            decimal totalamountcompletedpending = 0;
            foreach (SubscriptionMetadata subscriptionMetadata in allMetadatas)
            {
                Subscriptions Subscriptions = SubscriptionsBusiness.GetSubscriptionsByMetadataId(subscriptionMetadata.Id);
                if (Subscriptions != null)
                {

                    string[] uids = new string[] { };
                    if (!string.IsNullOrEmpty(Subscriptions.UserId))
                    {
                        uids = Subscriptions.UserId.Split(",");
                        totalamountcompletedpending += totalfinalprice(subscriptionMetadata, uids.Length);
                        noofsubscription += uids.Length;
                    }
                }
            }

            List<SubscriptionMetadata> pendingsubscriptionMetadatas = allMetadatas.FindAll(b => b.IsDeleted != true && b.Status == "Pending" || b.Status == "Waiting");
            decimal totalamountpending = 0;
            foreach (SubscriptionMetadata subscriptionMetadata in pendingsubscriptionMetadatas)
            {
                Subscriptions Subscriptions = SubscriptionsBusiness.GetSubscriptionsByMetadataId(subscriptionMetadata.Id);
                if (Subscriptions != null)
                {
                    string[] uids = new string[] { };
                    if (!string.IsNullOrEmpty(Subscriptions.UserId))
                    {
                        uids = Subscriptions.UserId.Split(",");
                        totalamountpending += totalfinalprice(subscriptionMetadata, uids.Length);
                    }
                }
            }

            List<SubscriptionMetadata> completedsubscriptionMetadatas = allMetadatas.FindAll(b => b.IsDeleted != true && b.Status == "Completed");
            decimal totalamountcompleted = 0;
            foreach (SubscriptionMetadata subscriptionMetadata in completedsubscriptionMetadatas)
            {
                Subscriptions Subscriptions = SubscriptionsBusiness.GetSubscriptionsByMetadataId(subscriptionMetadata.Id);

                if (Subscriptions != null)
                {
                    string[] uids = new string[] { };
                    if (!string.IsNullOrEmpty(Subscriptions.UserId))
                    {
                        uids = Subscriptions.UserId.Split(",");
                        totalamountcompleted += totalfinalprice(subscriptionMetadata, uids.Length);
                    }
                }
            }

            aboutAllPurchase.noofpurchase = allMetadatas.Count;
            aboutAllPurchase.noofsubscription = noofsubscription;
            aboutAllPurchase.totalamountcompletedpending = totalamountcompletedpending;
            aboutAllPurchase.totalamountpending = totalamountpending;
            aboutAllPurchase.totalamountcompleted = totalamountcompleted;

            return aboutAllPurchase;
        }

        public async Task<AboutAllPurchase> PurchaseSummary(PurchaseListFilterModel purchaseListFilterModel)
        {
            var allMetadatas = await EFSubscriptionMetadata.SubscriptionMetaDataListAsync(b => b.IsDeleted != true);

            if (purchaseListFilterModel.isfilterneeded == true)
            {
                if (!string.IsNullOrEmpty(purchaseListFilterModel.courseid))
                {
                    allMetadatas = allMetadatas.FindAll(b => b.CourseId == purchaseListFilterModel.courseid);
                }
                if (purchaseListFilterModel.discountpackageid != 0)
                {
                    allMetadatas = allMetadatas.FindAll(b => b.DiscountPackageId == purchaseListFilterModel.discountpackageid);
                }
                if (purchaseListFilterModel.salesagentid != 0)
                {
                    allMetadatas = allMetadatas.FindAll(b => b.SalesAgentId == purchaseListFilterModel.salesagentid);
                }
                if (!string.IsNullOrEmpty(purchaseListFilterModel.status))
                {
                    allMetadatas = allMetadatas.FindAll(b => b.Status == purchaseListFilterModel.status);
                }
                if (!string.IsNullOrEmpty(purchaseListFilterModel.enrollmentfromdate) && !string.IsNullOrEmpty(purchaseListFilterModel.enrollmenttodate))
                {
                    allMetadatas = allMetadatas.FindAll(
                        b => DateTime.ParseExact(b.EnrollmentFromDate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture) >= DateTime.ParseExact(purchaseListFilterModel.enrollmentfromdate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture)
                        && DateTime.ParseExact(b.EnrollmentFromDate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture) <= DateTime.ParseExact(purchaseListFilterModel.enrollmenttodate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture));
                }
            }

            AboutAllPurchase aboutAllPurchase = new AboutAllPurchase();

            int noofsubscription = 0;
            decimal totalamountcompletedpending = 0;
            foreach (SubscriptionMetadata subscriptionMetadata in allMetadatas)
            {
                Subscriptions Subscriptions = SubscriptionsBusiness.GetSubscriptionsByMetadataId(subscriptionMetadata.Id);
                if (Subscriptions != null)
                {

                    string[] uids = new string[] { };
                    if (!string.IsNullOrEmpty(Subscriptions.UserId))
                    {
                        uids = Subscriptions.UserId.Split(",");
                        totalamountcompletedpending += totalfinalprice(subscriptionMetadata, uids.Length);
                        noofsubscription += uids.Length;
                    }
                }
            }

            List<SubscriptionMetadata> pendingsubscriptionMetadatas = allMetadatas.FindAll(b => b.IsDeleted != true && b.Status == "Pending" || b.Status == "Waiting");
            decimal totalamountpending = 0;
            foreach (SubscriptionMetadata subscriptionMetadata in pendingsubscriptionMetadatas)
            {
                Subscriptions Subscriptions = SubscriptionsBusiness.GetSubscriptionsByMetadataId(subscriptionMetadata.Id);
                if (Subscriptions != null)
                {
                    string[] uids = new string[] { };
                    if (!string.IsNullOrEmpty(Subscriptions.UserId))
                    {
                        uids = Subscriptions.UserId.Split(",");
                        totalamountpending += totalfinalprice(subscriptionMetadata, uids.Length);
                    }
                }
            }

            List<SubscriptionMetadata> completedsubscriptionMetadatas = allMetadatas.FindAll(b => b.IsDeleted != true && b.Status == "Completed");
            decimal totalamountcompleted = 0;
            foreach (SubscriptionMetadata subscriptionMetadata in completedsubscriptionMetadatas)
            {
                Subscriptions Subscriptions = SubscriptionsBusiness.GetSubscriptionsByMetadataId(subscriptionMetadata.Id);

                if (Subscriptions != null)
                {
                    string[] uids = new string[] { };
                    if (!string.IsNullOrEmpty(Subscriptions.UserId))
                    {
                        uids = Subscriptions.UserId.Split(",");
                        totalamountcompleted += totalfinalprice(subscriptionMetadata, uids.Length);
                    }
                }
            }

            aboutAllPurchase.noofpurchase = allMetadatas.Count;
            aboutAllPurchase.noofsubscription = noofsubscription;
            aboutAllPurchase.totalamountcompletedpending = totalamountcompletedpending;
            aboutAllPurchase.totalamountpending = totalamountpending;
            aboutAllPurchase.totalamountcompleted = totalamountcompleted;

            return aboutAllPurchase;
        }

        public decimal totalfinalprice(SubscriptionMetadata subscriptionMetadata, int noofsub)
        {
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

            decimal totalbaseprice = 0;

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

            if (subscriptionMetadata.NoOfMonths > 0)
            {
                totalbaseprice = totalbaseprice * noofsub * subscriptionMetadata.NoOfMonths;
                totalbaseprice = totalbaseprice / noofsub;
            }
            else
            {
                totalbaseprice = totalbaseprice * noofsub;
                totalbaseprice = totalbaseprice / noofsub;
            }

            Discount discount = new Discount();
            decimal finalprice = 0;

            if (subscriptionMetadata.DiscountPackageId != 0)
            {
                discount = DiscountBusiness.getDiscountById(subscriptionMetadata.DiscountPackageId);
                int ftotalsubscription = 0;
                if (noofsub < discount.FreeSubscriptions)
                {
                    ftotalsubscription = 0;
                }
                else
                {
                    ftotalsubscription = (noofsub - discount.FreeSubscriptions);
                }

                if (ftotalsubscription != 0)
                {
                    totalbaseprice = totalbaseprice * ftotalsubscription;
                    finalprice = totalbaseprice - totalbaseprice * (ftotalsubscription * discount.OffSubscriptions) / 100;
                    finalprice = finalprice - (finalprice * discount.OffTotalPrice) / 100;
                }
                else
                {
                    finalprice = totalbaseprice * (ftotalsubscription * discount.OffTotalPrice) / 100;
                }
            }
            else
            {
                finalprice = totalbaseprice;
            }

            return finalprice;
        }
    }
}
