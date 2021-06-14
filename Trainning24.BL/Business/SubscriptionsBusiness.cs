using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class SubscriptionsBusiness
    {
        private readonly EFSubscriptions EFSubscriptions;
        private readonly SalesAgentBusiness _salesAgentBusiness;

        public SubscriptionsBusiness(
            EFSubscriptions EFSubscriptions,
            SalesAgentBusiness salesAgentBusiness
        )
        {
            this.EFSubscriptions = EFSubscriptions;
            this._salesAgentBusiness = salesAgentBusiness;
        }

        public Subscriptions Create(Subscriptions Subscriptions, int id)
        {
            //Subscriptions subscriptions = new Subscriptions();
            //subscriptions = EFSubscriptions.GetById(b => b.SubscriptionMetadataId == Subscriptions.SubscriptionMetadataId);

            //if (subscriptions == null) {
            Subscriptions.CreationTime = DateTime.Now.ToString();
            Subscriptions.CreatorUserId = id;
            Subscriptions.IsDeleted = false;
            EFSubscriptions.Insert(Subscriptions);
            return EFSubscriptions.GetById(b => b.Id == Subscriptions.Id);
            //}
            //else
            //{
            //    subscriptions.UserId = Subscriptions.UserId;
            //    subscriptions.SubscriptionMetadataId = Subscriptions.SubscriptionMetadataId;
            //    subscriptions.LastModificationTime = DateTime.Now.ToString();
            //    subscriptions.LastModifierUserId = id;
            //    EFSubscriptions.Update(subscriptions);
            //    return EFSubscriptions.GetById(b => b.SubscriptionMetadataId == Subscriptions.SubscriptionMetadataId);
            //}
        }

        public Subscriptions Update(Subscriptions Subscriptions, int id)
        {
            Subscriptions subscriptions = new Subscriptions();
            subscriptions = EFSubscriptions.GetById(b => b.SubscriptionMetadataId == Subscriptions.SubscriptionMetadataId);
            if (subscriptions != null)
            {
                subscriptions.UserId = Subscriptions.UserId;
                subscriptions.SubscriptionMetadataId = Subscriptions.SubscriptionMetadataId;
                subscriptions.LastModificationTime = DateTime.Now.ToString();
                subscriptions.LastModifierUserId = id;
                EFSubscriptions.Update(subscriptions);
            }
            return EFSubscriptions.GetById(b => b.SubscriptionMetadataId == Subscriptions.SubscriptionMetadataId);
        }

        public Subscriptions GetSubscriptionsByMetadataId(long id)
        {
            return EFSubscriptions.GetSubscriptionsByMetadataId(id);
        }


    }
}
