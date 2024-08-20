using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Activities;

namespace UzmanCrm.CrmService.CustomWorkflows
{
    public class AssigningExceptionLookupValueOnLoyaltyCard : CodeActivity
    {
        protected override void Execute(CodeActivityContext executionContext)
        {
            ContextInitializer contextInitilazer = new ContextInitializer(executionContext);
            
            Guid primaryId = contextInitilazer.context.PrimaryEntityId;
            contextInitilazer.tracingService.Trace("Card Discount Id = " + primaryId);

            // Get the target entity from the context
            Entity targetEntity = contextInitilazer.context.InputParameters["Target"] as Entity;

            if (targetEntity != null)
            {
                // Retrieve the field value
                if (targetEntity.Attributes.Contains("vkk_erpid"))
                {
                    var erpid = targetEntity.GetAttributeValue<string>("vkk_erpid");
                    contextInitilazer.tracingService.Trace("ErpId = " + erpid);

                    var loyaltyCardQuery = new QueryExpression("uzm_loyaltycard");
                    loyaltyCardQuery.Criteria.AddCondition("uzm_erpid", ConditionOperator.Equal, erpid);
                    EntityCollection loyaltyCardList = contextInitilazer.service.RetrieveMultiple(loyaltyCardQuery);

                    foreach (var loyaltyCardEntity in loyaltyCardList.Entities)
                    {
                        contextInitilazer.tracingService.Trace("Loyalty Card Id = " + loyaltyCardEntity.Id);
                        loyaltyCardEntity["uzm_carddiscountid"] = new EntityReference("uzm_carddiscount", primaryId);
                        contextInitilazer.service.Update(loyaltyCardEntity);
                    }
                }
            }
        }
    }
}
