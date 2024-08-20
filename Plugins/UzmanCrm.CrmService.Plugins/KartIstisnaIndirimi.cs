using Microsoft.Xrm.Sdk;
using System;
using UzmanCrm.CrmService.Plugins.DbConnection;
using UzmanCrm.CrmService.Plugins.Enums;

namespace UzmanCrm.CrmService.Plugins
{
    public class Kartİstisnaİndirimi : IPlugin
    {

        public void Execute(IServiceProvider serviceProvider)
        {
            // throw new InvalidPluginExecutionException("Ahirzamanda beklenen hata!\n");
            ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);
            CrmConnectionDal crmConDal = new CrmConnectionDal();

            try
            {
                if (context.InputParameters.Contains("Target") && (context.InputParameters["Target"] is Entity || context.InputParameters["Target"] is EntityReference))
                {
                    if (context.Depth > 1) return;
                    if (context.PrimaryEntityName == "uzm_carddiscount")
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
                                    var createPostImage = (Entity)context.PostEntityImages["postImage"];

                                    if (createPostImage.Attributes.Contains("vkk_erpid"))
                                    {
                                        string erpId = createPostImage.GetAttributeValue<string>("vkk_erpid"); // Erp Id
                                        var cardDiscountSql = $@"select count(uzm_carddiscountId) as recordcount from uzm_carddiscount with(nolock) where vkk_erpid='{erpId}' and statecode=0 and uzm_statuscode!=4";
                                        var dtCardDiscount = crmConDal.ToDataTable(cardDiscountSql);
                                        if (Convert.ToInt32(dtCardDiscount.Rows[0]["recordcount"]) > 1)
                                        {
                                            throw new InvalidPluginExecutionException("\nİlgili müşteri kartı için kullanımda olan istisna indirimi tanımlaması bulunmaktadır. Yeni istisna indirim tanımı oluşturulamaz.\n");// Sql: " + cardDiscountSql + "\n recordcount: " + Convert.ToInt32(dtCardDiscount.Rows[0]["recordcount"]));
                                        }
                                    }
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

            Entity ent = new Entity("uzm_carddiscountlog");
            ent["uzm_operation"] = new OptionSetValue((int)operationType); // İşlem tipi create/update/delete
            ent["uzm_operationuserid"] = new EntityReference("systemuser", context.InitiatingUserId);

            switch (contextMessage)
            {
                case "create":
                    var createPostImage = (Entity)context.PostEntityImages["postImage"];

                    ent["uzm_carddiscountid"] = new EntityReference("uzm_carddiscount", entityid); // ilgili kart istisna tanımı kaydı

                    if (createPostImage.Attributes.Contains("uzm_loyaltycardid"))
                        ent["uzm_loyaltycardidnew"] = createPostImage["uzm_loyaltycardid"]; // Müşteri Kartı
                    if (createPostImage.Attributes.Contains("uzm_cardclasssegmentid"))
                        ent["uzm_cardclasssegmentidnew"] = createPostImage["uzm_cardclasssegmentid"]; // Kart Sınıfı Segmenti
                    if (createPostImage.Attributes.Contains("uzm_customergroupid"))
                        ent["uzm_customergroupidnew"] = createPostImage["uzm_customergroupid"]; // Müşteri Grubu
                    if (createPostImage.Attributes.Contains("uzm_demandeduser"))
                        ent["uzm_demandedusernew"] = createPostImage["uzm_demandeduser"]; // Talep Eden
                    if (createPostImage.Attributes.Contains("uzm_discountrate"))
                        ent["uzm_discountratenew"] = createPostImage["uzm_discountrate"]; // İndirim Yüzdesi
                    if (createPostImage.Attributes.Contains("uzm_approvalstatus"))
                        ent["uzm_approvalstatusnew"] = createPostImage.GetAttributeValue<OptionSetValue>("uzm_approvalstatus"); // İndirim Onay Statüsü
                    if (createPostImage.Attributes.Contains("uzm_startdate"))
                        ent["uzm_startdatenew"] = createPostImage["uzm_startdate"]; // Başlangıç Tarihi
                    if (createPostImage.Attributes.Contains("uzm_enddate"))
                        ent["uzm_enddatenew"] = createPostImage["uzm_enddate"]; // Bitiş Tarihi
                    if (createPostImage.Attributes.Contains("uzm_demanddate"))
                        ent["uzm_demanddatenew"] = createPostImage["uzm_demanddate"]; // Talep Tarihi
                    if (createPostImage.Attributes.Contains("uzm_description"))
                        ent["uzm_descriptionnew"] = createPostImage["uzm_description"]; // Açıklama
                    if (createPostImage.Attributes.Contains("uzm_approvedby"))
                        ent["uzm_approvedbynew"] = createPostImage["uzm_approvedby"]; // Onaylayan
                    if (createPostImage.Attributes.Contains("uzm_statuscode"))
                        ent["uzm_statuscodenew"] = createPostImage.GetAttributeValue<OptionSetValue>("uzm_statuscode"); // Statü
                    if (createPostImage.Attributes.Contains("vkk_erpid")) // ErpId
                        ent["vkk_erpidnew"] = createPostImage["vkk_erpid"];
                    break;
                case "delete":
                    var deletePreImage = (Entity)context.PreEntityImages["preImage"];

                    if (deletePreImage.Attributes.Contains("uzm_loyaltycardid"))
                        ent["uzm_loyaltycardidold"] = deletePreImage["uzm_loyaltycardid"]; // Müşteri Kartı
                    if (deletePreImage.Attributes.Contains("uzm_cardclasssegmentid"))
                        ent["uzm_cardclasssegmentidold"] = deletePreImage["uzm_cardclasssegmentid"]; // Kart Sınıfı Segmenti
                    if (deletePreImage.Attributes.Contains("uzm_customergroupid"))
                        ent["uzm_customergroupidold"] = deletePreImage["uzm_customergroupid"]; // Müşteri Grubu
                    if (deletePreImage.Attributes.Contains("uzm_demandeduser"))
                        ent["uzm_demandeduserold"] = deletePreImage["uzm_demandeduser"]; // Talep Eden
                    if (deletePreImage.Attributes.Contains("uzm_discountrate"))
                        ent["uzm_discountrateold"] = deletePreImage["uzm_discountrate"]; // İndirim Yüzdesi
                    if (deletePreImage.Attributes.Contains("uzm_approvalstatus"))
                        ent["uzm_approvalstatusold"] = deletePreImage.GetAttributeValue<OptionSetValue>("uzm_approvalstatus"); // İndirim Onay Statüsü
                    if (deletePreImage.Attributes.Contains("uzm_startdate"))
                        ent["uzm_startdateold"] = deletePreImage["uzm_startdate"]; // Başlangıç Tarihi
                    if (deletePreImage.Attributes.Contains("uzm_enddate"))
                        ent["uzm_enddateold"] = deletePreImage["uzm_enddate"]; // Bitiş Tarihi
                    if (deletePreImage.Attributes.Contains("uzm_demanddate"))
                        ent["uzm_demanddateold"] = deletePreImage["uzm_demanddate"]; // Talep Tarihi
                    if (deletePreImage.Attributes.Contains("uzm_description"))
                        ent["uzm_descriptionold"] = deletePreImage["uzm_description"]; // Açıklama
                    if (deletePreImage.Attributes.Contains("uzm_approvedby"))
                        ent["uzm_approvedbyold"] = deletePreImage["uzm_approvedby"]; // Onaylayan
                    if (deletePreImage.Attributes.Contains("uzm_statuscode"))
                        ent["uzm_statuscodeold"] = deletePreImage.GetAttributeValue<OptionSetValue>("uzm_statuscode"); // Statü
                    if (deletePreImage.Attributes.Contains("vkk_erpid")) // ErpId
                        ent["vkk_erpidold"] = deletePreImage["vkk_erpid"];
                    break;
                case "update":
                    var updatePreImage = (Entity)context.PreEntityImages["preImage"];
                    var updatePostImage = (Entity)context.PostEntityImages["postImage"];

                    ent["uzm_carddiscountid"] = new EntityReference("uzm_carddiscount", entityid); // ilgili kart segment kaydı

                    #region Müşteri Kartı güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_loyaltycardid") && updatePostImage.Attributes.Contains("uzm_loyaltycardid"))
                    {
                        if (updatePostImage.GetAttributeValue<EntityReference>("uzm_loyaltycardid").Id != updatePreImage.GetAttributeValue<EntityReference>("uzm_loyaltycardid").Id)
                        {
                            ent["uzm_loyaltycardidold"] = updatePreImage["uzm_loyaltycardid"];
                            ent["uzm_loyaltycardidnew"] = updatePostImage["uzm_loyaltycardid"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_loyaltycardid") && updatePostImage.Attributes.Contains("uzm_loyaltycardid"))
                        ent["uzm_loyaltycardidnew"] = updatePostImage["uzm_loyaltycardid"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_loyaltycardid") && !updatePostImage.Attributes.Contains("uzm_loyaltycardid"))
                        ent["uzm_loyaltycardidold"] = updatePreImage["uzm_loyaltycardid"];
                    #endregion

                    #region Kart Sınıfı Segmenti güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_cardclasssegmentid") && updatePostImage.Attributes.Contains("uzm_cardclasssegmentid"))
                    {
                        if (updatePostImage.GetAttributeValue<EntityReference>("uzm_cardclasssegmentid").Id != updatePreImage.GetAttributeValue<EntityReference>("uzm_cardclasssegmentid").Id)
                        {
                            ent["uzm_cardclasssegmentidold"] = updatePreImage["uzm_cardclasssegmentid"];
                            ent["uzm_cardclasssegmentidnew"] = updatePostImage["uzm_cardclasssegmentid"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_cardclasssegmentid") && updatePostImage.Attributes.Contains("uzm_cardclasssegmentid"))
                        ent["uzm_cardclasssegmentidnew"] = updatePostImage["uzm_cardclasssegmentid"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_cardclasssegmentid") && !updatePostImage.Attributes.Contains("uzm_cardclasssegmentid"))
                        ent["uzm_cardclasssegmentidold"] = updatePreImage["uzm_cardclasssegmentid"];
                    #endregion

                    #region Müşteri Grubu güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_customergroupid") && updatePostImage.Attributes.Contains("uzm_customergroupid"))
                    {
                        if (updatePostImage.GetAttributeValue<EntityReference>("uzm_customergroupid").Id != updatePreImage.GetAttributeValue<EntityReference>("uzm_customergroupid").Id)
                        {
                            ent["uzm_customergroupidold"] = updatePreImage["uzm_customergroupid"];
                            ent["uzm_customergroupidnew"] = updatePostImage["uzm_customergroupid"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_customergroupid") && updatePostImage.Attributes.Contains("uzm_customergroupid"))
                        ent["uzm_customergroupidnew"] = updatePostImage["uzm_customergroupid"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_customergroupid") && !updatePostImage.Attributes.Contains("uzm_customergroupid"))
                        ent["uzm_customergroupidold"] = updatePreImage["uzm_customergroupid"];
                    #endregion

                    #region Talep Eden güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_demandeduser") && updatePostImage.Attributes.Contains("uzm_demandeduser"))
                    {
                        if (updatePostImage.GetAttributeValue<EntityReference>("uzm_demandeduser").Id != updatePreImage.GetAttributeValue<EntityReference>("uzm_demandeduser").Id)
                        {
                            ent["uzm_demandeduserold"] = updatePreImage["uzm_demandeduser"];
                            ent["uzm_demandedusernew"] = updatePostImage["uzm_demandeduser"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_demandeduser") && updatePostImage.Attributes.Contains("uzm_demandeduser"))
                        ent["uzm_demandedusernew"] = updatePostImage["uzm_demandeduser"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_demandeduser") && !updatePostImage.Attributes.Contains("uzm_demandeduser"))
                        ent["uzm_demandeduserold"] = updatePreImage["uzm_demandeduser"];
                    #endregion

                    #region İndirim Yüzdesi güncellendi mi
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

                    #region İndirim Onay Statüsü güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_approvalstatus") && updatePostImage.Attributes.Contains("uzm_approvalstatus"))
                    {
                        if (updatePreImage.GetAttributeValue<OptionSetValue>("uzm_approvalstatus") != updatePostImage.GetAttributeValue<OptionSetValue>("uzm_approvalstatus"))
                        {
                            ent["uzm_approvalstatusold"] = updatePreImage.GetAttributeValue<OptionSetValue>("uzm_approvalstatus");
                            ent["uzm_approvalstatusnew"] = updatePostImage.GetAttributeValue<OptionSetValue>("uzm_approvalstatus");
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_approvalstatus") && updatePostImage.Attributes.Contains("uzm_approvalstatus"))
                        ent["uzm_approvalstatusnew"] = updatePostImage.GetAttributeValue<OptionSetValue>("uzm_approvalstatus");

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_approvalstatus") && !updatePostImage.Attributes.Contains("uzm_approvalstatus"))
                        ent["uzm_approvalstatusold"] = updatePreImage.GetAttributeValue<OptionSetValue>("uzm_approvalstatus");
                    #endregion

                    #region Başlangıç Tarihi güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_startdate") && updatePostImage.Attributes.Contains("uzm_startdate"))
                    {
                        if (updatePostImage["uzm_startdate"].ToString() != updatePreImage["uzm_startdate"].ToString())
                        {
                            ent["uzm_startdateold"] = updatePreImage["uzm_startdate"];
                            ent["uzm_startdatenew"] = updatePostImage["uzm_startdate"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_startdate") && updatePostImage.Attributes.Contains("uzm_startdate"))
                        ent["uzm_startdatenew"] = updatePostImage["uzm_startdate"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_startdate") && !updatePostImage.Attributes.Contains("uzm_startdate"))
                        ent["uzm_startdateold"] = updatePreImage["uzm_startdate"];
                    #endregion

                    #region Bitiş Tarihi güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_enddate") && updatePostImage.Attributes.Contains("uzm_enddate"))
                    {
                        if (updatePostImage["uzm_enddate"].ToString() != updatePreImage["uzm_enddate"].ToString())
                        {
                            ent["uzm_enddateold"] = updatePreImage["uzm_enddate"];
                            ent["uzm_enddatenew"] = updatePostImage["uzm_enddate"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_enddate") && updatePostImage.Attributes.Contains("uzm_enddate"))
                        ent["uzm_enddatenew"] = updatePostImage["uzm_enddate"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_enddate") && !updatePostImage.Attributes.Contains("uzm_enddate"))
                        ent["uzm_enddateold"] = updatePreImage["uzm_enddate"];
                    #endregion

                    #region Talep Tarihi güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_demanddate") && updatePostImage.Attributes.Contains("uzm_demanddate"))
                    {
                        if (updatePostImage["uzm_demanddate"].ToString() != updatePreImage["uzm_demanddate"].ToString())
                        {
                            ent["uzm_demanddateold"] = updatePreImage["uzm_demanddate"];
                            ent["uzm_demanddatenew"] = updatePostImage["uzm_demanddate"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_demanddate") && updatePostImage.Attributes.Contains("uzm_demanddate"))
                        ent["uzm_demanddatenew"] = updatePostImage["uzm_demanddate"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_demanddate") && !updatePostImage.Attributes.Contains("uzm_demanddate"))
                        ent["uzm_demanddateold"] = updatePreImage["uzm_demanddate"];
                    #endregion

                    #region Açıklama güncellendi mi
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

                    #region Onaylayan güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_approvedby") && updatePostImage.Attributes.Contains("uzm_approvedby"))
                    {
                        if (updatePostImage.GetAttributeValue<EntityReference>("uzm_approvedby").Id != updatePreImage.GetAttributeValue<EntityReference>("uzm_approvedby").Id)
                        {
                            ent["uzm_approvedbyold"] = updatePreImage["uzm_approvedby"];
                            ent["uzm_approvedbynew"] = updatePostImage["uzm_approvedby"];
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_approvedby") && updatePostImage.Attributes.Contains("uzm_approvedby"))
                        ent["uzm_approvedbynew"] = updatePostImage["uzm_approvedby"];

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_approvedby") && !updatePostImage.Attributes.Contains("uzm_approvedby"))
                        ent["uzm_approvedbyold"] = updatePreImage["uzm_approvedby"];
                    #endregion

                    #region Statü güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("uzm_statuscode") && updatePostImage.Attributes.Contains("uzm_statuscode"))
                    {
                        if (updatePreImage.GetAttributeValue<OptionSetValue>("uzm_statuscode") != updatePostImage.GetAttributeValue<OptionSetValue>("uzm_statuscode"))
                        {
                            ent["uzm_statuscodeold"] = updatePreImage.GetAttributeValue<OptionSetValue>("uzm_statuscode");
                            ent["uzm_statuscodenew"] = updatePostImage.GetAttributeValue<OptionSetValue>("uzm_statuscode");
                        }
                    }

                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("uzm_statuscode") && updatePostImage.Attributes.Contains("uzm_statuscode"))
                        ent["uzm_statuscodenew"] = updatePostImage.GetAttributeValue<OptionSetValue>("uzm_statuscode");

                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("uzm_statuscode") && !updatePostImage.Attributes.Contains("uzm_statuscode"))
                        ent["uzm_statuscodeold"] = updatePreImage.GetAttributeValue<OptionSetValue>("uzm_statuscode");
                    #endregion

                    #region ErpId güncellendi mi
                    // iki alan da doluysa ve eşit değillerse
                    if (updatePreImage.Attributes.Contains("vkk_erpid") && updatePostImage.Attributes.Contains("vkk_erpid"))
                    {
                        if (updatePostImage["vkk_erpid"].ToString() != updatePreImage["vkk_erpid"].ToString())
                        {
                            ent["vkk_erpidold"] = updatePreImage["vkk_erpid"];
                            ent["vkk_erpidnew"] = updatePostImage["vkk_erpid"];
                        }
                    }
                    // eski değer boş, yeni değer doluysa
                    else if (!updatePreImage.Attributes.Contains("vkk_erpid") && updatePostImage.Attributes.Contains("vkk_erpid"))
                        ent["vkk_erpidnew"] = updatePostImage["vkk_erpid"];
                    // yeni değer boş, eski değer doluysa
                    else if (updatePreImage.Attributes.Contains("vkk_erpid") && !updatePostImage.Attributes.Contains("vkk_erpid"))
                        ent["vkk_erpidold"] = updatePreImage["vkk_erpid"];
                    #endregion

                    break;
                default:
                    break;
            }

            service.Create(ent);
        }
    }
}