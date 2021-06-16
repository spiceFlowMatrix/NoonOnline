using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class DiscountBusiness
    {

        private readonly EFDiscountBusinessRepository EFDiscountBusinessRepository;


        public DiscountBusiness(
            EFDiscountBusinessRepository EFDiscountBusinessRepository
        )
        {
            this.EFDiscountBusinessRepository = EFDiscountBusinessRepository;
        }

        public Discount Create(Discount Discount, int id)
        {
            Discount discount = EFDiscountBusinessRepository.ListQuery(b => b.PackageName == Discount.PackageName).SingleOrDefault();
            if (discount != null)
            {
                return null;
            }

            Discount.CreationTime = DateTime.Now.ToString();
            Discount.CreatorUserId = id;
            Discount.IsDeleted = false;
            EFDiscountBusinessRepository.Update(Discount);
            return EFDiscountBusinessRepository.GetById(b => b.Id == Discount.Id);
        }

        public Discount Update(Discount Discount, int id)
        {
            Discount discount = EFDiscountBusinessRepository.GetById(b => b.Id == Discount.Id);
            discount.FreeSubscriptions = Discount.FreeSubscriptions;
            discount.OffSubscriptions = Discount.OffSubscriptions;
            discount.OffTotalPrice = Discount.OffTotalPrice;
            discount.PackageName = Discount.PackageName;
            discount.LastModificationTime = DateTime.Now.ToString();
            discount.LastModifierUserId = id;
            EFDiscountBusinessRepository.Update(discount);
            return EFDiscountBusinessRepository.GetById(b => b.Id == Discount.Id);
        }

        public List<Discount> DiscountList(PaginationModel paginationModel, out int total)
        {
            List<Discount> DiscountList = new List<Discount>();
            DiscountList = EFDiscountBusinessRepository.GetAll().OrderByDescending(b => b.Id).ToList(); ;

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = DiscountList.Count();

                DiscountList = DiscountList.OrderByDescending(b => b.Id).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();

                if (!string.IsNullOrEmpty(paginationModel.search))
                    DiscountList = DiscountList.OrderByDescending(b => b.Id).Where(
                                                       b => b.Id.ToString().Any(k => b.Id.ToString().Contains(paginationModel.search))
                                                    || b.OffTotalPrice.ToString().ToLower().Any(k => b.OffTotalPrice.ToString().Contains(paginationModel.search))
                                                    || b.OffSubscriptions.ToString().ToLower().Any(k => b.OffSubscriptions.ToString().Contains(paginationModel.search))
                                                    || b.FreeSubscriptions.ToString().ToLower().Any(k => b.FreeSubscriptions.ToString().Contains(paginationModel.search))
                                                    ).ToList();

                return DiscountList;
            }

            total = DiscountList.Count();
            return DiscountList;
        }

        public Discount getDiscountById(long id)
        {
            return EFDiscountBusinessRepository.GetById(b => b.Id == id && b.IsDeleted != true);
        }

        public Discount Delete(int Id, int DeleterId)
        {
            Discount Discount = new Discount();
            Discount.Id = Id;
            Discount.DeleterUserId = DeleterId;
            EFDiscountBusinessRepository.Delete(Discount);
            Discount deletedDiscount = EFDiscountBusinessRepository.GetById(b => b.Id == Id);
            return deletedDiscount;
        }

    }
}
