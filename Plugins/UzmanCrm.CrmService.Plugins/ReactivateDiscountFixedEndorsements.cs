using Microsoft.Xrm.Sdk;
using System;
using UzmanCrm.CrmService.Plugins.DbConnection;

namespace UzmanCrm.CrmService.Plugins
{
    public class ReactivateDiscountFixedEndorsements : IPlugin
    {
        #region Secure/Unsecure Configuration Setup
        private string _secureConfig = null;
        private string _unsecureConfig = null;

        public ReactivateDiscountFixedEndorsements(string unsecureConfig, string secureConfig)
        {
            _secureConfig = secureConfig;
            _unsecureConfig = unsecureConfig;
        }
        #endregion

        /// <summary>
        /// Barem artış sabitleme dönemine denk gelen ciroların tekrar hesaplanabilmesini sağlar.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <exception cref="InvalidPluginExecutionException"></exception>
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
                    if (context.PrimaryEntityName == "uzm_loyaltyspecification")
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
                                    var updatePostImage = (Entity)context.PostEntityImages["postImage"];

                                    if (updatePostImage.Attributes.Contains("uzm_wagescalestartcalculationdate") & updatePostImage.Attributes.Contains("uzm_discountfixingflag"))
                                    {
                                        var uzm_wagescalestartcalculationdate = updatePostImage.GetAttributeValue<DateTime>("uzm_wagescalestartcalculationdate"); // Barem Hesaplama Başlangıç Tarihi
                                        var uzm_discountfixingflag = updatePostImage.GetAttributeValue<bool>("uzm_discountfixingflag"); // Barem İndirim Artış Sabitleme

                                        // Barem İndirim Artış Sabitleme pasife çekildiyse çalışır
                                        if (!uzm_discountfixingflag)
                                        {
                                            var reactivateEndorsementSql = $@"
BEGIN TRANSACTION
BEGIN TRY
	BEGIN
		declare @ceCount int; set @ceCount=0;
		declare @lsCount int; set @lsCount=0;
		declare @UpperWageScaleStartCalculationDate datetime;

		--Başlangıç tarihi işlem yapılan Sadakat Kart Tanımı'ndan büyük olup en küçük Barem Hesaplama Başlangıç Tarihi bulunur 
		SELECT TOP (1) @UpperWageScaleStartCalculationDate=[uzm_wagescalestartcalculationdate]
		FROM [KahveDunyasi_MSCRM].[dbo].[uzm_loyaltyspecificationBase]  with(nolock)
		WHERE statecode=0 AND uzm_wagescalestartcalculationdate > '{uzm_wagescalestartcalculationdate}'
		order by uzm_wagescalestartcalculationdate asc;
		select @lsCount=@@ROWCOUNT;
		
        -- Daha güncel Sadakat Kart Tanımı bulamazsa ciro sorgusu aralık bitiş tarihi olarak şimdiyi set eder
		if @lsCount = 0
			BEGIN
				set @UpperWageScaleStartCalculationDate = GETDATE();
			END

		--işlem yapılan ve bir sonraki Sadakat Kart Tanımı kayıtlarının Barem Hesaplama Başlangıç Tarihi değerleri aralığında olan,
		-- ciro hesaplaması yapılmamış ciroların entegrasyon durumu pasife çekilip Hangfire tarafından işlenecek duruma getirilmiş olur
		Update uzm_customerendorsement 
		set uzm_integrationstatus=0, uzm_integrationdescription=null, uzm_discountfixingflag=0
		WHERE CreatedOn BETWEEN '{uzm_wagescalestartcalculationdate}' AND @UpperWageScaleStartCalculationDate
		AND uzm_integrationstatus = 1 AND uzm_discountfixingflag = 1 AND statecode = 0;
		select @ceCount= @@ROWCOUNT;
		
		IF @ceCount = 0
			BEGIN 
				ROLLBACK TRANSACTION 
				select 0 as result
		END
	END

COMMIT TRANSACTION 
select 1 as result
END TRY 

BEGIN CATCH 
	ROLLBACK TRANSACTION
	select 0 as result
END CATCH
";
                                            var reactivateEndorsementRes = crmConDal.ExecuteNonQuery<int>(reactivateEndorsementSql, null);
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
    }
}
