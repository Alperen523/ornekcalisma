using Microsoft.Xrm.Sdk;
using System;
using UzmanCrm.CrmService.Plugins.Enums;

namespace UzmanCrm.CrmService.Plugins
{
    public class LoyaltyTanimlama : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // throw new InvalidPluginExecutionException("Ahirzamanda beklenen hata!\n");
            ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            try
            {
                if (context.InputParameters.Contains("Target") && (context.InputParameters["Target"] is Entity || context.InputParameters["Target"] is EntityReference))
                {
                    if (context.Depth > 1) return;
                    if (context.PrimaryEntityName == "uzm_loyaltyspecification")
                    {
                        #region Parameters
                        Guid _InputEntity = Guid.Empty;
                        string exceptionMessage = string.Empty;
                        EntityReference inputEntityR = null;
                        Entity inputEntity = null;
                        string urunKimligi = string.Empty;
                        #endregion

                        #region GetTargetEntityId
                        if (context.MessageName.ToLower().Equals("create") || context.MessageName.ToLower().Equals("update"))
                        {
                            inputEntity = (Entity)context.InputParameters["Target"];
                            if (inputEntity != null && inputEntity.Id != Guid.Empty)
                                _InputEntity = inputEntity.Id;
                        }
                        else if (context.MessageName.ToLower().Equals("delete"))
                        {
                            inputEntityR = (EntityReference)context.InputParameters["Target"];
                            if (inputEntityR != null && inputEntityR.Id != Guid.Empty)
                                _InputEntity = inputEntityR.Id;
                        }
                        #endregion

                        #region MainCode
                        if (_InputEntity != Guid.Empty)
                        {
                            #region Create
                            if (context.ParentContext.MessageName.ToLower().Equals("revise"))
                            {
                                return;
                            }
                            if (context.MessageName.ToLower().Equals("create"))
                            {
                                #region Pre-Create
                                if (context.Stage == 20)/*Pre Create*/
                                {

                                }
                                #endregion
                                #region Post-Create
                                else if (context.Stage == 40)/*Post Create*/
                                {
                                    createLog(_InputEntity, context, service, "create");
                                }
                                #endregion
                            }
                            #endregion
                            #region Update
                            else if (context.MessageName.ToLower().Equals("update"))
                            {
                                #region Pre-Update
                                if (context.Stage == 20)/*Pre Update*/
                                {

                                }
                                #endregion
                                #region Post-Update
                                else if (context.Stage == 40)/*Post Update*/
                                {
                                    createLog(_InputEntity, context, service, "update");
                                }
                                #endregion
                            }
                            #endregion
                            #region Delete
                            else if (context.MessageName.ToLower().Equals("delete"))
                            {
                                #region Pre-Delete
                                if (context.Stage == 20)/*Pre Delete*/
                                {
                                    createLog(_InputEntity, context, service, "delete");
                                }
                                #endregion
                                #region Post-Delete
                                else if (context.Stage == 40)/*Post Delete*/
                                {

                                }
                                #endregion
                            }
                            #endregion
                            #region assign
                            else if (context.MessageName.ToLower().Equals("assign"))
                            {
                                #region Pre-Assign
                                if (context.Stage == 20)/*Pre Assign*/
                                {

                                }
                                #endregion
                                #region Post-Assign
                                else if (context.Stage == 40)/*Post Assign*/
                                {

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

        public static void createLog(Guid entityid, IPluginExecutionContext context, IOrganizationService service, string contextMessage)
        {
            var operationType = (OperationType)Enum.Parse(typeof(OperationType), contextMessage);

            Entity ent = new Entity("uzm_loyaltyspecificationlog");
            ent["uzm_operation"] = new OptionSetValue((int)operationType); // İşlem tipi create/update/delete
            ent["uzm_operationuserid"] = new EntityReference("systemuser", context.InitiatingUserId);

            switch (contextMessage)
            {
                case "create":
                    ent["uzm_loyaltyspecificationid"] = new EntityReference("uzm_loyaltyspecification", entityid); // ilgili kart tipi kaydı
                    var createPostImage = (Entity)context.PostEntityImages["postImage"];

                    if (createPostImage.Attributes.Contains("uzm_name")) // Dönem Adı
                        ent["uzm_namenew"] = createPostImage["uzm_name"];
                    if (createPostImage.Attributes.Contains("uzm_wagescalestartcalculationdate")) // Barem Hesaplama Başlangıç Tarihi
                        ent["uzm_wagescalestartcalculationdatenew"] = createPostImage["uzm_wagescalestartcalculationdate"];
                    if (createPostImage.Attributes.Contains("uzm_discountfixingflag")) // Barem İndirim Artış Sabitleme
                        ent["uzm_discountfixingflagnew"] = createPostImage["uzm_discountfixingflag"];
                    if (createPostImage.Attributes.Contains("uzm_periodtransitiondate")) // Dönem Geçiş Tarihi
                        ent["uzm_periodtransitiondatenew"] = createPostImage["uzm_periodtransitiondate"];
                    if (createPostImage.Attributes.Contains("uzm_processtype")) // Dönem / Barem Değişikliği
                        ent["uzm_processtypenew"] = createPostImage.GetAttributeValue<OptionSetValue>("uzm_processtype");
                    break;
                case "delete":
                    var deletePreImage = (Entity)context.PreEntityImages["preImage"];

                    if (deletePreImage.Attributes.Contains("uzm_name")) // Dönem Adı
                        ent["uzm_nameold"] = deletePreImage["uzm_name"];
                    if (deletePreImage.Attributes.Contains("uzm_wagescalestartcalculationdate")) // Barem Hesaplama Başlangıç Tarihi
                        ent["uzm_wagescalestartcalculationdateold"] = deletePreImage["uzm_wagescalestartcalculationdate"];
                    if (deletePreImage.Attributes.Contains("uzm_discountfixingflag")) // Barem İndirim Artış Sabitleme
                        ent["uzm_discountfixingflagold"] = deletePreImage["uzm_discountfixingflag"];
                    if (deletePreImage.Attributes.Contains("uzm_periodtransitiondate")) // Dönem Geçiş Tarihi
                        ent["uzm_periodtransitiondateold"] = deletePreImage["uzm_periodtransitiondate"];
                    if (deletePreImage.Attributes.Contains("uzm_processtype")) // Dönem / Barem Değişikliği
                        ent["uzm_processtypeold"] = deletePreImage.GetAttributeValue<OptionSetValue>("uzm_processtype");
                    break;
                case "update":
                    var updatePreImage = (Entity)context.PreEntityImages["preImage"];
                    var updatePostImage = (Entity)context.PostEntityImages["postImage"];

                    ent["uzm_loyaltyspecificationid"] = new EntityReference("uzm_loyaltyspecification", entityid); // ilgili kart tipi kaydı

                    #region Dönem Adı güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_name") && updatePostImage.Attributes.Contains("uzm_name"))
                    {
                        if (updatePostImage["uzm_name"].ToString() != updatePreImage["uzm_name"].ToString())
                        {
                            ent["uzm_nameold"] = updatePreImage["uzm_name"];
                            ent["uzm_namenew"] = updatePostImage["uzm_name"];
                        }
                    }
                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_name") && updatePostImage.Attributes.Contains("uzm_name"))
                        ent["uzm_namenew"] = updatePostImage["uzm_name"];
                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_name") && !updatePostImage.Attributes.Contains("uzm_name"))
                        ent["uzm_nameold"] = updatePreImage["uzm_name"];
                    #endregion

                    #region Barem Hesaplama Başlangıç Tarihi güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_wagescalestartcalculationdate") && updatePostImage.Attributes.Contains("uzm_wagescalestartcalculationdate"))
                    {
                        if (updatePostImage["uzm_wagescalestartcalculationdate"].ToString() != updatePreImage["uzm_wagescalestartcalculationdate"].ToString())
                        {
                            ent["uzm_wagescalestartcalculationdateold"] = updatePreImage["uzm_wagescalestartcalculationdate"];
                            ent["uzm_wagescalestartcalculationdatenew"] = updatePostImage["uzm_wagescalestartcalculationdate"];
                        }
                    }
                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_wagescalestartcalculationdate") && updatePostImage.Attributes.Contains("uzm_wagescalestartcalculationdate"))
                        ent["uzm_wagescalestartcalculationdatenew"] = updatePostImage["uzm_wagescalestartcalculationdate"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_wagescalestartcalculationdate") && !updatePostImage.Attributes.Contains("uzm_wagescalestartcalculationdate"))
                        ent["uzm_wagescalestartcalculationdateold"] = updatePreImage["uzm_wagescalestartcalculationdate"];
                    #endregion

                    #region Barem İndirim Artış Sabitleme güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_discountfixingflag") && updatePostImage.Attributes.Contains("uzm_discountfixingflag"))
                    {
                        if (updatePostImage.GetAttributeValue<bool>("uzm_discountfixingflag") != updatePreImage.GetAttributeValue<bool>("uzm_discountfixingflag"))
                        {
                            ent["uzm_discountfixingflagold"] = updatePreImage["uzm_discountfixingflag"];
                            ent["uzm_discountfixingflagnew"] = updatePostImage["uzm_discountfixingflag"];
                        }
                    }
                    
                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_discountfixingflag") && updatePostImage.Attributes.Contains("uzm_discountfixingflag"))
                        ent["uzm_discountfixingflagnew"] = updatePostImage["uzm_discountfixingflag"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_discountfixingflag") && !updatePostImage.Attributes.Contains("uzm_discountfixingflag"))
                        ent["uzm_discountfixingflagold"] = updatePreImage["uzm_discountfixingflag"];
                    #endregion

                    #region Dönem Geçiş Tarihi güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_periodtransitiondate") && updatePostImage.Attributes.Contains("uzm_periodtransitiondate"))
                    {
                        if (updatePostImage["uzm_periodtransitiondate"].ToString() != updatePreImage["uzm_periodtransitiondate"].ToString())
                        {
                            ent["uzm_periodtransitiondateold"] = updatePreImage["uzm_periodtransitiondate"];
                            ent["uzm_periodtransitiondatenew"] = updatePostImage["uzm_periodtransitiondate"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_periodtransitiondate") && updatePostImage.Attributes.Contains("uzm_periodtransitiondate"))
                        ent["uzm_periodtransitiondatenew"] = updatePostImage["uzm_periodtransitiondate"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_periodtransitiondate") && !updatePostImage.Attributes.Contains("uzm_periodtransitiondate"))
                        ent["uzm_periodtransitiondateold"] = updatePreImage["uzm_periodtransitiondate"];
                    #endregion

                    #region Dönem / Barem Değişikliği güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_processtype") && updatePostImage.Attributes.Contains("uzm_processtype"))
                    {
                        if (updatePreImage.GetAttributeValue<OptionSetValue>("uzm_processtype") != updatePostImage.GetAttributeValue<OptionSetValue>("uzm_processtype"))
                        {
                            ent["uzm_processtypeold"] = updatePreImage.GetAttributeValue<OptionSetValue>("uzm_processtype");
                            ent["uzm_processtypenew"] = updatePostImage.GetAttributeValue<OptionSetValue>("uzm_processtype");
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_processtype") && updatePostImage.Attributes.Contains("uzm_processtype"))
                        ent["uzm_processtypenew"] = updatePostImage.GetAttributeValue<OptionSetValue>("uzm_processtype");

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_processtype") && !updatePostImage.Attributes.Contains("uzm_processtype"))
                        ent["uzm_processtypeold"] = updatePreImage.GetAttributeValue<OptionSetValue>("uzm_processtype");
                    #endregion

                    break;
                default:
                    break;
            }

            service.Create(ent);
        }
    }
}
