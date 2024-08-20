using Microsoft.Xrm.Sdk;
using System;
using UzmanCrm.CrmService.Plugins.Enums;

namespace UzmanCrm.CrmService.Plugins
{
    public class KartTipi : IPlugin
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
                // var preImage = (Entity)context.PreEntityImages["Image"];
                // var postImage = (Entity)context.PostEntityImages["Image"];
                if (context.InputParameters.Contains("Target") && (context.InputParameters["Target"] is Entity || context.InputParameters["Target"] is EntityReference))
                {
                    if (context.Depth > 1) return;
                    if (context.PrimaryEntityName == "uzm_cardtypedefinition")
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

            Entity ent = new Entity("uzm_cardtypedefinitionlog");
            ent["uzm_operation"] = new OptionSetValue((int)operationType); // İşlem tipi create/update/delete
            ent["uzm_operationuserid"] = new EntityReference("systemuser", context.InitiatingUserId);

            switch (contextMessage)
            {
                case "create":
                    ent["uzm_cardtypedefinitionid"] = new EntityReference("uzm_cardtypedefinition", entityid); // ilgili kart tipi kaydı
                    var createPostImage = (Entity)context.PostEntityImages["postImage"];

                    if (createPostImage.Attributes.Contains("uzm_name")) // Kart Tipi Adı
                        ent["uzm_cardtypenamenew"] = createPostImage["uzm_name"];
                    if (createPostImage.Attributes.Contains("uzm_cardtypedescription")) // Açıklama
                        ent["uzm_cardtypedescriptionnew"] = createPostImage["uzm_cardtypedescription"];
                    if (createPostImage.Attributes.Contains("uzm_code")) // Kart Tipi Kodu
                        ent["uzm_codenew"] = createPostImage["uzm_code"];
                    break;
                case "delete":
                    var deletePreImage = (Entity)context.PreEntityImages["preImage"];

                    if (deletePreImage.Attributes.Contains("uzm_name")) // Kart Tipi Adı
                        ent["uzm_cardtypenameold"] = deletePreImage["uzm_name"];
                    if (deletePreImage.Attributes.Contains("uzm_cardtypedescription")) // Açıklama
                        ent["uzm_cardtypedescriptionold"] = deletePreImage["uzm_cardtypedescription"];
                    if (deletePreImage.Attributes.Contains("uzm_code")) // Kart Tipi Kodu
                        ent["uzm_codeold"] = deletePreImage["uzm_code"];
                    break;
                case "update":
                    var updatePreImage = (Entity)context.PreEntityImages["preImage"];
                    var updatePostImage = (Entity)context.PostEntityImages["postImage"];

                    ent["uzm_cardtypedefinitionid"] = new EntityReference("uzm_cardtypedefinition", entityid); // ilgili kart tipi kaydı

                    #region  Kart Tipi Adı güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_name") && updatePostImage.Attributes.Contains("uzm_name"))
                    {
                        if (updatePostImage["uzm_name"].ToString() != updatePreImage["uzm_name"].ToString())
                        {
                            ent["uzm_cardtypenameold"] = updatePreImage["uzm_name"];
                            ent["uzm_cardtypenamenew"] = updatePostImage["uzm_name"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_name") && updatePostImage.Attributes.Contains("uzm_name"))
                        ent["uzm_cardtypenamenew"] = updatePostImage["uzm_name"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_name") && !updatePostImage.Attributes.Contains("uzm_name"))
                        ent["uzm_cardtypenameold"] = updatePreImage["uzm_name"];
                    #endregion

                    #region Açıklama güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_cardtypedescription") && updatePostImage.Attributes.Contains("uzm_cardtypedescription"))
                    {
                        if (updatePostImage["uzm_cardtypedescription"].ToString() != updatePreImage["uzm_cardtypedescription"].ToString())
                        {
                            ent["uzm_cardtypedescriptionold"] = updatePreImage["uzm_cardtypedescription"];
                            ent["uzm_cardtypedescriptionnew"] = updatePostImage["uzm_cardtypedescription"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_cardtypedescription") && updatePostImage.Attributes.Contains("uzm_cardtypedescription"))
                        ent["uzm_cardtypedescriptionnew"] = updatePostImage["uzm_cardtypedescription"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_cardtypedescription") && !updatePostImage.Attributes.Contains("uzm_cardtypedescription"))
                        ent["uzm_cardtypedescriptionold"] = updatePreImage["uzm_cardtypedescription"];
                    #endregion

                    #region  Kart Tipi Kodu güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_code") && updatePostImage.Attributes.Contains("uzm_code"))
                    {
                        if (updatePostImage["uzm_code"].ToString() != updatePreImage["uzm_code"].ToString())
                        {
                            ent["uzm_codeold"] = updatePreImage["uzm_code"];
                            ent["uzm_codenew"] = updatePostImage["uzm_code"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_code") && updatePostImage.Attributes.Contains("uzm_code"))
                        ent["uzm_codenew"] = updatePostImage["uzm_code"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_code") && !updatePostImage.Attributes.Contains("uzm_code"))
                        ent["uzm_codeold"] = updatePreImage["uzm_code"];
                    #endregion

                    break;
                default:
                    break;
            }

            service.Create(ent);
        }
    }
}

