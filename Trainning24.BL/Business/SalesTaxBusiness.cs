using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class SalesTaxBusiness
    {
        private readonly EFSalesTaxRepository _EFSalesTaxRepository;
        public SalesTaxBusiness(
            EFSalesTaxRepository EFSalesTaxRepository
        )
        {
            _EFSalesTaxRepository = EFSalesTaxRepository;
        }

        public SalesTax GetById(long Id)
        {
            return _EFSalesTaxRepository.GetById(b => b.Id == Id && b.DeletionTime == null);
        }

        public SalesTax Get()
        {
            return _EFSalesTaxRepository.Get();
        }

        public SalesTax UpdateTax(SalesTax obj)
        {
            var tax = GetById(obj.Id);
            if (tax != null)
            {
                tax.Tax = obj.Tax;
                tax.LastModificationTime = obj.LastModificationTime;
                tax.LastModifierUserId = obj.LastModifierUserId;
                _EFSalesTaxRepository.Update(tax);
            }
            return tax;
        }
    }
}
