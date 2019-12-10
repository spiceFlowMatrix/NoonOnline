using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.DefaultValues;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class DefaultValuesBusiness
    {
        private readonly EFDefaultValues EFDefaultValues;

        public DefaultValuesBusiness(
            EFDefaultValues EFDefaultValues
        )
        {
            this.EFDefaultValues = EFDefaultValues;
        }

        public DefaultValues Create(DefaultValuesModel DefaultValuesModel, int id)
        {
            DefaultValues DefaultValues = new DefaultValues
            {
                istimeouton = DefaultValuesModel.istimeouton,
                reminder = DefaultValuesModel.reminder,
                timeout = DefaultValuesModel.timeout,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };

            EFDefaultValues.Insert(DefaultValues);

            return DefaultValues;
        }

        public DefaultValues Update(DefaultValuesModel DefaultValuesModel, int id)
        {
            DefaultValues DefaultValues = EFDefaultValues.GetById(b => b.Id == DefaultValuesModel.id);

            DefaultValues.istimeouton = DefaultValuesModel.istimeouton;
            DefaultValues.reminder = DefaultValuesModel.reminder;
            DefaultValues.timeout = DefaultValuesModel.timeout;
            DefaultValues.intervals = DefaultValuesModel.intervals;
            DefaultValues.LastModificationTime = DateTime.Now.ToString();
            DefaultValues.LastModifierUserId = id;

            EFDefaultValues.Update(DefaultValues);

            return DefaultValues;
        }

        public List<DefaultValues> Get()
        {

            return EFDefaultValues.GetAll();
        }



        public DefaultValues GetById(long id)
        {
            return EFDefaultValues.GetById(b=>b.Id==id);
        }



    }
}
