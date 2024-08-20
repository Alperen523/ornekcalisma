using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using UzmanCrm.CrmService.Plugins.DbConnection;

namespace UzmanCrm.CrmService.Plugins
{
    public class PostUpdate_CardExceptionDiscountStateChange : IPlugin
    {
        #region Secure/Unsecure Configuration Setup
        private string _secureConfig = null;
        private string _unsecureConfig = null;

        public PostUpdate_CardExceptionDiscountStateChange(string unsecureConfig, string secureConfig)
        {
            _secureConfig = secureConfig;
            _unsecureConfig = unsecureConfig;
        }
        #endregion

        public void Execute(IServiceProvider serviceProvider)
        {
            //throw new InvalidPluginExecutionException("Ahirzamanda beklenen hata!\n");
            ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            try
            {
                if (context.InputParameters.Contains("Target") && (context.InputParameters["Target"] is Entity || context.InputParameters["Target"] is EntityReference))
                {
                    if (context.Depth > 1) return;
                    if (context.PrimaryEntityName == "uzm_carddiscount")
                    {
                        #region Parameters
                        Guid _InputEntity = Guid.Empty;
                        Entity inputEntity = null;
                        #endregion

                        #region GetTargetEntityId
                        inputEntity = (Entity)context.InputParameters["Target"];
                        if (inputEntity != null && inputEntity.Id != Guid.Empty)
                            _InputEntity = inputEntity.Id;
                        #endregion

                        #region MainCode
                        if (_InputEntity != Guid.Empty)
                        {
                            #region Update
                            if (context.MessageName.ToLower().Equals("update"))
                            {
                                #region Post-Update
                                if (context.Stage == 40)/*Post Update*/
                                {
                                    var updatePreImage = (Entity)context.PreEntityImages["preImage"];
                                    var updatePostImage = (Entity)context.PostEntityImages["postImage"];

                                    if (updatePostImage.Attributes.Contains("vkk_erpid"))
                                    {
                                        var erpId = updatePostImage.GetAttributeValue<string>("vkk_erpid");
                                        if (!string.IsNullOrEmpty(erpId))
                                        {
                                            #region İndirim Onay Statüsü güncellendi mi
                                            // iki alan da doluysa ve eşit değillerse
                                            if (updatePreImage.Attributes.Contains("uzm_approvalstatus") && updatePostImage.Attributes.Contains("uzm_approvalstatus"))
                                            {
                                                if (updatePreImage.GetAttributeValue<OptionSetValue>("uzm_approvalstatus") != updatePostImage.GetAttributeValue<OptionSetValue>("uzm_approvalstatus"))
                                                {
                                                    CreateEndorsement(erpId);
                                                }
                                            }

                                            // eski değer boş, yeni değer doluysa
                                            else if (!updatePreImage.Attributes.Contains("uzm_approvalstatus") && updatePostImage.Attributes.Contains("uzm_approvalstatus"))
                                                CreateEndorsement(erpId);

                                            // yeni değer boş, eski değer doluysa
                                            else if (updatePreImage.Attributes.Contains("uzm_approvalstatus") && !updatePostImage.Attributes.Contains("uzm_approvalstatus"))
                                                CreateEndorsement(erpId);
                                            #endregion

                                            #region Statü güncellendi mi
                                            // iki alan da doluysa ve eşit değillerse
                                            if (updatePreImage.Attributes.Contains("uzm_statuscode") && updatePostImage.Attributes.Contains("uzm_statuscode"))
                                            {
                                                if (updatePreImage.GetAttributeValue<OptionSetValue>("uzm_statuscode") != updatePostImage.GetAttributeValue<OptionSetValue>("uzm_statuscode"))
                                                    CreateEndorsement(erpId);
                                            }

                                            // eski değer boş, yeni değer doluysa
                                            else if (!updatePreImage.Attributes.Contains("uzm_statuscode") && updatePostImage.Attributes.Contains("uzm_statuscode"))
                                                CreateEndorsement(erpId);

                                            // yeni değer boş, eski değer doluysa
                                            else if (updatePreImage.Attributes.Contains("uzm_statuscode") && !updatePostImage.Attributes.Contains("uzm_statuscode"))
                                                CreateEndorsement(erpId);
                                            #endregion

                                            #region Durum güncellendi mi
                                            // Statecode kesinlikle dolu olacağı için sadece eşit değiller mi diye kontrol edilir
                                            if (updatePreImage.Attributes.Contains("statecode") && updatePostImage.Attributes.Contains("statecode"))
                                            {
                                                if (updatePreImage.GetAttributeValue<OptionSetValue>("statecode").Value != updatePostImage.GetAttributeValue<OptionSetValue>("statecode").Value)
                                                    CreateEndorsement(erpId);
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }

        public static void CreateEndorsement(string erpId)
        {
            CrmConnectionDal crmConDal = new CrmConnectionDal();

            var loyaltyCardSql = $@"select uzm_contactid, uzm_loyaltycardId, uzm_cardnumber from KahveDunyasi_MSCRM.dbo.uzm_loyaltycard with(nolock) where uzm_erpId='{erpId}' and statecode=0";
            var dtLoyaltyCard = crmConDal.ToDataTable(loyaltyCardSql);

            for (int i = 0; i < dtLoyaltyCard.Rows.Count; i++)
            {
                DataRow drLoyaltyCard = dtLoyaltyCard.Rows[i];
                var loyaltyCardNumber = drLoyaltyCard["uzm_cardnumber"].ToString();
                var loyaltyCardId = drLoyaltyCard["uzm_loyaltycardId"].ToString();
                var contactId = (Guid)drLoyaltyCard["uzm_contactid"];
                var now = DateTime.Now.AddHours(-3).ToString("yyyy-MM-ddTHH:mm:ss");

                var queryCrm = $@"
                        INSERT INTO [uzm_customerendorsementBase]
                                   ([uzm_customerendorsementId]
		                           ,[CreatedOn]
                                   ,[CreatedBy]
                                   ,[ModifiedOn]
                                   ,[ModifiedBy]
                                   ,[CreatedOnBehalfBy]
                                   ,[ModifiedOnBehalfBy]
                                   ,[OrganizationId]
                                   ,[statecode]
                                   ,[statuscode]
                                   ,[ImportSequenceNumber]
                                   ,[OverriddenCreatedOn]
                                   ,[TimeZoneRuleVersionNumber]
                                   ,[UTCConversionTimeZoneCode]
                                   ,[uzm_name]
                                   ,[uzm_invoicenumber]
                                   ,[uzm_ordernumber]
                                   ,[uzm_orderdate]
                                   ,[uzm_totalamount]
                                   ,[uzm_storecode]
                                   ,[uzm_transactionid]
                                   ,[uzm_erpid]
                                   ,[uzm_integrationstatus]
                                   ,[uzm_giftcardamount]
                                   ,[uzm_billtype]
                                   ,[uzm_cardno]
                                   ,[uzm_invoicedate]
                                   ,[uzm_contactid]
                                   ,[uzm_willbepassive]
                                   ,[uzm_loyaltycardid])
	                         OUTPUT INSERTED.uzm_customerendorsementId
                             VALUES	 
                                  ( NEWID()
		                           ,DATEADD(HOUR, -3, GETDATE())
                                   ,'A627B337-47CD-E811-8120-005056991930' -- crmadmin
                                   ,DATEADD(HOUR, -3, GETDATE())
                                   ,'A627B337-47CD-E811-8120-005056991930' -- crmadmin
                                   ,NULL
                                   ,NULL
                                   ,'30F6172F-47CD-E811-8120-005056991930'--VakkoCRM
                                   ,0
                                   ,1
                                   ,NULL
                                   ,NULL
                                   ,26
                                   ,134
                                   ,'CardException-INV-{erpId}-{loyaltyCardNumber}-{now}'
                                   ,'CardException-INV-{erpId}-{loyaltyCardNumber}-{now}'
                                   ,'CardException-ORD-{erpId}-{loyaltyCardNumber}-{now}'
                                   ,DATEADD(HOUR, -3, GETDATE())
                                   ,0
                                   ,'1132'
                                   ,'CardException-TRNSCTN-{erpId}-{loyaltyCardNumber}-{now}'
                                   ,'{erpId}'
                                   ,0
                                   ,0
                                   ,1 --ZP01
                                   ,'{loyaltyCardNumber}'
                                   ,DATEADD(HOUR, -3, GETDATE())
                                   ,{(contactId != Guid.Empty ? "\'" + contactId + "\'" : "NULL")}
                                   ,1
                                   ,'{loyaltyCardId}'
                        )";
                var dtCardDiscount = crmConDal.ExecuteNonQuery<string>(queryCrm, null);
            }
        }
    }
}