using Microsoft.Xrm.Sdk;
using System;
using UzmanCrm.CrmService.Plugins.Enums;

namespace UzmanCrm.CrmService.Plugins
{
    public class Barem : IPlugin
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
                    if (context.PrimaryEntityName == "uzm_wagescale")
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

            Entity ent = new Entity("uzm_wagescalelog");
            ent["uzm_operation"] = new OptionSetValue((int)operationType); // İşlem tipi create/update/delete
            ent["uzm_operationuserid"] = new EntityReference("systemuser", context.InitiatingUserId);

            switch (contextMessage)
            {
                case "create":
                    ent["uzm_wagescaleid"] = new EntityReference("uzm_wagescale", entityid); // ilgili kart tipi kaydı
                    var createPostImage = (Entity)context.PostEntityImages["postImage"];

                    if (createPostImage.Attributes.Contains("uzm_cardtypedefinitionid")) // Marka Kart Tipi
                        ent["uzm_cardtypedefinitionidnew"] = createPostImage["uzm_cardtypedefinitionid"];
                    if (createPostImage.Attributes.Contains("uzm_wagescaleyear")) // Barem Başlangıç Tarihi
                        ent["uzm_wagescaleyearnew"] = createPostImage["uzm_wagescaleyear"];
                    if (createPostImage.Attributes.Contains("uzm_rangestart")) // Aralık-1
                        ent["uzm_rangestartnew"] = createPostImage["uzm_rangestart"];
                    if (createPostImage.Attributes.Contains("uzm_rangeend")) // Aralık-2
                        ent["uzm_rangeendnew"] = createPostImage["uzm_rangeend"];
                    if (createPostImage.Attributes.Contains("uzm_discountrate")) // İndirim(%)
                        ent["uzm_discountratenew"] = createPostImage["uzm_discountrate"];
                    break;
                case "delete":
                    var deletePreImage = (Entity)context.PreEntityImages["preImage"];

                    if (deletePreImage.Attributes.Contains("uzm_cardtypedefinitionid")) // Marka Kart Tipi
                        ent["uzm_cardtypedefinitionidold"] = deletePreImage["uzm_cardtypedefinitionid"];
                    if (deletePreImage.Attributes.Contains("uzm_wagescaleyear")) // Barem Başlangıç Tarihi
                        ent["uzm_wagescaleyearold"] = deletePreImage["uzm_wagescaleyear"];
                    if (deletePreImage.Attributes.Contains("uzm_rangestart")) // Aralık-1
                        ent["uzm_rangestartold"] = deletePreImage["uzm_rangestart"];
                    if (deletePreImage.Attributes.Contains("uzm_rangeend")) // Aralık-2
                        ent["uzm_rangeendold"] = deletePreImage["uzm_rangeend"];
                    if (deletePreImage.Attributes.Contains("uzm_discountrate")) // İndirim(%)
                        ent["uzm_discountrateold"] = deletePreImage["uzm_discountrate"];
                    break;
                case "update":
                    var updatePreImage = (Entity)context.PreEntityImages["preImage"];
                    var updatePostImage = (Entity)context.PostEntityImages["postImage"];

                    ent["uzm_wagescaleid"] = new EntityReference("uzm_wagescale", entityid); // ilgili kart tipi kaydı

                    #region Marka Kart Tipi güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_cardtypedefinitionid") && updatePostImage.Attributes.Contains("uzm_cardtypedefinitionid"))
                    {
                        if (updatePostImage.GetAttributeValue<EntityReference>("uzm_cardtypedefinitionid").Id != updatePreImage.GetAttributeValue<EntityReference>("uzm_cardtypedefinitionid").Id)
                        {
                            ent["uzm_cardtypedefinitionidold"] = updatePreImage["uzm_cardtypedefinitionid"];
                            ent["uzm_cardtypedefinitionidnew"] = updatePostImage["uzm_cardtypedefinitionid"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_cardtypedefinitionid") && updatePostImage.Attributes.Contains("uzm_cardtypedefinitionid"))
                        ent["uzm_cardtypedefinitionidnew"] = updatePostImage["uzm_cardtypedefinitionid"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_cardtypedefinitionid") && !updatePostImage.Attributes.Contains("uzm_cardtypedefinitionid"))
                        ent["uzm_cardtypedefinitionidold"] = updatePreImage["uzm_cardtypedefinitionid"];
                    #endregion

                    #region Barem Başlangıç Tarihi güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_wagescaleyear") && updatePostImage.Attributes.Contains("uzm_wagescaleyear"))
                    {
                        if (updatePostImage["uzm_wagescaleyear"].ToString() != updatePreImage["uzm_wagescaleyear"].ToString())
                        {
                            ent["uzm_wagescaleyearold"] = updatePreImage["uzm_wagescaleyear"];
                            ent["uzm_wagescaleyearnew"] = updatePostImage["uzm_wagescaleyear"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_wagescaleyear") && updatePostImage.Attributes.Contains("uzm_wagescaleyear"))
                        ent["uzm_wagescaleyearnew"] = updatePostImage["uzm_wagescaleyear"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_wagescaleyear") && !updatePostImage.Attributes.Contains("uzm_wagescaleyear"))
                        ent["uzm_wagescaleyearold"] = updatePreImage["uzm_wagescaleyear"];
                    #endregion

                    #region Aralık-1 güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_rangestart") && updatePostImage.Attributes.Contains("uzm_rangestart"))
                    {
                        if (updatePostImage["uzm_rangestart"].ToString() != updatePreImage["uzm_rangestart"].ToString())
                        {
                            ent["uzm_rangestartold"] = updatePreImage["uzm_rangestart"];
                            ent["uzm_rangestartnew"] = updatePostImage["uzm_rangestart"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_rangestart") && updatePostImage.Attributes.Contains("uzm_rangestart"))
                        ent["uzm_rangestartnew"] = updatePostImage["uzm_rangestart"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_rangestart") && !updatePostImage.Attributes.Contains("uzm_rangestart"))
                        ent["uzm_rangestartold"] = updatePreImage["uzm_rangestart"];
                    #endregion

                    #region Aralık-2 güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_rangeend") && updatePostImage.Attributes.Contains("uzm_rangeend"))
                    {
                        if (updatePostImage["uzm_rangeend"].ToString() != updatePreImage["uzm_rangeend"].ToString())
                        {
                            ent["uzm_rangeendold"] = updatePreImage["uzm_rangeend"];
                            ent["uzm_rangeendnew"] = updatePostImage["uzm_rangeend"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_rangeend") && updatePostImage.Attributes.Contains("uzm_rangeend"))
                        ent["uzm_rangeendnew"] = updatePostImage["uzm_rangeend"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_rangeend") && !updatePostImage.Attributes.Contains("uzm_rangeend"))
                        ent["uzm_rangeendold"] = updatePreImage["uzm_rangeend"];
                    #endregion

                    #region İndirim(%) güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_discountrate") && updatePostImage.Attributes.Contains("uzm_discountrate"))
                    {
                        if (updatePostImage["uzm_discountrate"].ToString() != updatePreImage["uzm_discountrate"].ToString())
                        {
                            ent["uzm_discountrateold"] = updatePreImage["uzm_discountrate"];
                            ent["uzm_discountratenew"] = updatePostImage["uzm_discountrate"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_discountrate") && updatePostImage.Attributes.Contains("uzm_discountrate"))
                        ent["uzm_discountratenew"] = updatePostImage["uzm_discountrate"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_discountrate") && !updatePostImage.Attributes.Contains("uzm_discountrate"))
                        ent["uzm_discountrateold"] = updatePreImage["uzm_discountrate"];
                    #endregion

                    break;
                default:
                    break;
            }

            service.Create(ent);
        }
    }
}
