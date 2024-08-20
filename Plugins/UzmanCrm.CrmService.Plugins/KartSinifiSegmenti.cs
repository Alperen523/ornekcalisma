using Microsoft.Xrm.Sdk;
using System;
using UzmanCrm.CrmService.Plugins.Enums;

namespace UzmanCrm.CrmService.Plugins
{
    public class KartSinifiSegmenti : IPlugin
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
                    if (context.PrimaryEntityName == "uzm_cardclasssegment")
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
                                #region Post-Create
                                if (context.Stage == 40)/*Post Create*/
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

            Entity ent = new Entity("uzm_cardclasssegmentlog");
            ent["uzm_operation"] = new OptionSetValue((int)operationType); // İşlem tipi create/update/delete
            ent["uzm_operationuserid"] = new EntityReference("systemuser", context.InitiatingUserId);

            switch (contextMessage)
            {
                case "create":
                    var createPostImage = (Entity)context.PostEntityImages["postImage"];

                    ent["uzm_cardclasssegmentid"] = new EntityReference("uzm_cardclasssegment", entityid); // ilgili kart segment kaydı
                    
                    if (createPostImage.Attributes.Contains("uzm_name")) // Segment Adı
                        ent["uzm_segmentnamenew"] = createPostImage["uzm_name"];
                    if (createPostImage.Attributes.Contains("uzm_description")) // Açıklama
                        ent["uzm_descriptionnew"] = createPostImage["uzm_description"];
                    if (createPostImage.Attributes.Contains("uzm_validityperiod")) // Geçerlilik Süresi(gün)
                        ent["uzm_validityperiodnew"] = createPostImage["uzm_validityperiod"];
                    if (createPostImage.Attributes.Contains("uzm_notificationperiod")) // Birinci Bildirim Süresi(gün)
                        ent["uzm_notificationperiodnew"] = createPostImage["uzm_notificationperiod"];
                    if (createPostImage.Attributes.Contains("uzm_secondnotificationperiod")) // İkinci Bildirim Süresi(gün)
                        ent["uzm_secondnotificationperiodnew"] = createPostImage["uzm_secondnotificationperiod"];
                    break;
                case "delete":
                    var deletePreImage = (Entity)context.PreEntityImages["preImage"];

                    if (deletePreImage.Attributes.Contains("uzm_name")) // Segment Adı
                        ent["uzm_segmentnameold"] = deletePreImage["uzm_name"];
                    if (deletePreImage.Attributes.Contains("uzm_description")) // Açıklama
                        ent["uzm_descriptionold"] = deletePreImage["uzm_description"];
                    if (deletePreImage.Attributes.Contains("uzm_validityperiod")) // Geçerlilik Süresi(gün)
                        ent["uzm_validityperiodold"] = deletePreImage["uzm_validityperiod"];
                    if (deletePreImage.Attributes.Contains("uzm_notificationperiod")) // Birinci Bildirim Süresi(gün)
                        ent["uzm_notificationperiodold"] = deletePreImage["uzm_notificationperiod"];
                    if (deletePreImage.Attributes.Contains("uzm_secondnotificationperiod")) // İkinci Bildirim Süresi(gün)
                        ent["uzm_secondnotificationperiodold"] = deletePreImage["uzm_secondnotificationperiod"];
                    break;
                case "update":
                    var updatePreImage = (Entity)context.PreEntityImages["preImage"];
                    var updatePostImage = (Entity)context.PostEntityImages["postImage"];

                    ent["uzm_cardclasssegmentid"] = new EntityReference("uzm_cardclasssegment", entityid); // ilgili kart segment kaydı

                    #region Segment Adı güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_name") && updatePostImage.Attributes.Contains("uzm_name"))
                    {
                        if (updatePostImage["uzm_name"].ToString() != updatePreImage["uzm_name"].ToString())
                        {
                            ent["uzm_segmentnameold"] = updatePreImage["uzm_name"];
                            ent["uzm_segmentnamenew"] = updatePostImage["uzm_name"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_name") && updatePostImage.Attributes.Contains("uzm_name"))
                        ent["uzm_segmentnamenew"] = updatePostImage["uzm_name"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_name") && !updatePostImage.Attributes.Contains("uzm_name"))
                        ent["uzm_segmentnameold"] = updatePreImage["uzm_name"];
                    #endregion

                    #region  Açıklama güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_description") && updatePostImage.Attributes.Contains("uzm_description"))
                    {
                        if (updatePostImage["uzm_description"].ToString() != updatePreImage["uzm_description"].ToString())
                        {
                            ent["uzm_descriptionold"] = updatePreImage["uzm_description"];
                            ent["uzm_descriptionnew"] = updatePostImage["uzm_description"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_description") && updatePostImage.Attributes.Contains("uzm_description"))
                        ent["uzm_descriptionnew"] = updatePostImage["uzm_description"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_description") && !updatePostImage.Attributes.Contains("uzm_description"))
                        ent["uzm_descriptionold"] = updatePreImage["uzm_description"];
                    #endregion

                    #region  Geçerlilik Süresi(gün) güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_validityperiod") && updatePostImage.Attributes.Contains("uzm_validityperiod"))
                    {
                        if (updatePostImage["uzm_validityperiod"].ToString() != updatePreImage["uzm_validityperiod"].ToString())
                        {
                            ent["uzm_validityperiodold"] = updatePreImage["uzm_validityperiod"];
                            ent["uzm_validityperiodnew"] = updatePostImage["uzm_validityperiod"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_validityperiod") && updatePostImage.Attributes.Contains("uzm_validityperiod"))
                        ent["uzm_validityperiodnew"] = updatePostImage["uzm_validityperiod"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_validityperiod") && !updatePostImage.Attributes.Contains("uzm_validityperiod"))
                        ent["uzm_validityperiodold"] = updatePreImage["uzm_validityperiod"];
                    #endregion

                    #region Birinci Bildirim Süresi(gün) güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_notificationperiod") && updatePostImage.Attributes.Contains("uzm_notificationperiod"))
                    {
                        if (updatePostImage["uzm_notificationperiod"].ToString() != updatePreImage["uzm_notificationperiod"].ToString())
                        {
                            ent["uzm_notificationperiodold"] = updatePreImage["uzm_notificationperiod"];
                            ent["uzm_notificationperiodnew"] = updatePostImage["uzm_notificationperiod"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_notificationperiod") && updatePostImage.Attributes.Contains("uzm_notificationperiod"))
                        ent["uzm_notificationperiodnew"] = updatePostImage["uzm_notificationperiod"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_notificationperiod") && !updatePostImage.Attributes.Contains("uzm_notificationperiod"))
                        ent["uzm_notificationperiodold"] = updatePreImage["uzm_notificationperiod"];
                    #endregion

                    #region İkinci Bildirim Süresi(gün) güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_secondnotificationperiod") && updatePostImage.Attributes.Contains("uzm_secondnotificationperiod"))
                    {
                        if (updatePostImage["uzm_secondnotificationperiod"].ToString() != updatePreImage["uzm_secondnotificationperiod"].ToString())
                        {
                            ent["uzm_secondnotificationperiodold"] = updatePreImage["uzm_secondnotificationperiod"];
                            ent["uzm_secondnotificationperiodnew"] = updatePostImage["uzm_secondnotificationperiod"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_secondnotificationperiod") && updatePostImage.Attributes.Contains("uzm_secondnotificationperiod"))
                        ent["uzm_secondnotificationperiodnew"] = updatePostImage["uzm_secondnotificationperiod"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_secondnotificationperiod") && !updatePostImage.Attributes.Contains("uzm_secondnotificationperiod"))
                        ent["uzm_secondnotificationperiodold"] = updatePreImage["uzm_secondnotificationperiod"];
                    #endregion

                    break;
                default:
                    break;
            }

            service.Create(ent);
        }
    }
}