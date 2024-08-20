using System.Collections.Generic;

namespace UzmanCrm.CrmService.Application.Helper
{
    public static class StaticConsts
    {
        public const string JwtSecurityKey = "c8fd5318192b0a75f201d81f5a65ed1";
        public const string JwtIssuer = "http://localhost:40000";
        public const string JwtAudience = "http://localhost:40000";

        public static List<string> DisregardedPointList = new List<string> { "CLNSX", "CLNSUAX", "CLNSRUX", "CLNSROX", "CLNSBY2019X" };
        public static string SearchSqlFieldText = @"select [ContactId],[FirstName],[LastName],[GenderCode],[MobilePhone],[emailaddress1]
      ,[uzm_customertypeid]
      ,[uzm_birthdaycelebration]
      ,[uzm_isebaregistration]
      ,[uzm_ebaregistrationdate]
      ,[uzm_loyaltyactivationdate]
      ,[uzm_smspermission]
      ,[uzm_emailpermission]
      ,[uzm_callpermission]
      ,[uzm_membershipagreementapproval]
      ,[uzm_kvkkpermission]
      ,[uzm_neighbourhood]
      ,[uzm_mobileappdownloaded]
      ,[uzm_mobileappdownloadeddate]
      ,[uzm_mobileid]
      ,[uzm_countryid]
      ,[uzm_countyid]
      ,[uzm_neighborstoreid]
      ,[uzm_arrivalstoreid]
      ,[uzm_neighborcampaign]
      ,[uzm_cityid]
      ,[uzm_createdbyuserid]
      ,[uzm_modifiedbyuserid]
      ,[uzm_createdbystoreid]
      ,[uzm_modifiedbystoreid]
      ,[uzm_createdbypersonid]
      ,[uzm_modifiedbypersonid]
      ,[uzm_mobilcustomerid]                                               
       FROM [KahveDunyasi_MSCRM].[dbo].[ContactBase] cst with(nolock) where ";

        public static string SearchLikeSqlFieldText = @"select * from
                                            ";

        public static string SearchSqlWhereEnd = @" and statecode = 0";


        public static string Sql_Update_Ovm_ContactPhone = @" DECLARE @Gsm1 nvarchar(50)= NULL;
                                            DECLARE @Gsm2 nvarchar(50)= NULL;
                                            DECLARE @MessagePermit1 nvarchar(50)= NULL;
                                            DECLARE @MessagePermit2 nvarchar(50)= NULL;
                                            ;with cte AS (
                                              SELECT 
                                                uzm_customerphonenumber, 
                                                uzm_iyscallpermit, 
                                                uzm_iysphonepermit, 
                                                CreatedOn, 
                                                ModifiedOn, 
                                                ROW_NUMBER() OVER (
                                                  ORDER BY 
                                                    ModifiedOn DESC
                                                ) row_num 
                                              FROM 
                                                {0}..uzm_customerphone with(nolock) 
                                              WHERE 
                                                uzm_customerId = @CustomerCrmId 
                                                AND statecode = 0
                                            ) 
                                            SELECT 
                                              @Gsm1 = uzm_customerphonenumber, 
                                              @MessagePermit1 = uzm_iysphonepermit 
                                            FROM 
                                              cte 
                                            WHERE 
                                              row_num = 1 ------Güncel Gsm2 bilgisini almak.
                                            ;with cte AS (
                                              SELECT 
                                                uzm_customerphonenumber, 
                                                uzm_iyscallpermit, 
                                                uzm_iysphonepermit, 
                                                CreatedOn, 
                                                ModifiedOn, 
                                                ROW_NUMBER() OVER (
                                                  ORDER BY 
                                                    ModifiedOn DESC
                                                ) row_num 
                                              FROM 
                                                {0}..uzm_customerphone with(nolock) 
                                              WHERE 
                                                uzm_customerId = @CustomerCrmId 
                                                AND statecode = 0
                                            ) 
                                            SELECT 
                                              @Gsm2 = uzm_customerphonenumber, 
                                              @MessagePermit2 = uzm_iysphonepermit 
                                            FROM 
                                              cte 
                                            WHERE 
                                              row_num = 2 
                                            UPDATE 
                                              Customer 
                                            SET 
                                              Gsm1 = @Gsm1, 
                                              Gsm2 = @Gsm2, 
                                              UpdatedDate = GETDATE(), 
                                              ContactableGsm1 = @MessagePermit1, 
                                              ContactableGsm2 = @MessagePermit2 
                                            WHERE 
                                              CrmId = @CustomerCrmId and Statu=0
                                            SELECT 
                                              @@ROWCOUNT ";


        public static string Sql_Update_Crm_ContactPhone = @" 
                                            DECLARE @Gsm1 nvarchar(50)= NULL;
                                            ;with cte AS (
                                              SELECT 
                                                uzm_customerphonenumber, 
                                                uzm_iyscallpermit, 
                                                uzm_iysphonepermit, 
                                                CreatedOn, 
                                                ModifiedOn, 
                                                ROW_NUMBER() OVER (
                                                  ORDER BY 
                                                    ModifiedOn DESC
                                                ) row_num 
                                              FROM 
                                                {0}..uzm_customerphone with(nolock) 
                                              WHERE 
                                                uzm_customerId = @CustomerCrmId 
                                                AND statecode = 0
                                            ) 
                                            SELECT 
                                              @Gsm1 = uzm_customerphonenumber
                                            FROM 
                                              cte 
                                            WHERE 
                                              row_num = 1 ------Güncel MobilePhone bilgisini almak.
                                           
                                            UPDATE 
                                              {0}..ContactBase 
                                            SET 
                                              MobilePhone = @Gsm1                                         
                                            WHERE 
                                              ContactId = @CustomerCrmId  
                                            SELECT 
                                              @@ROWCOUNT ";
    }
}
