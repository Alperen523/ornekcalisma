using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace UzmanCrm.CrmService.Plugins
{
    public class CheckNumberValue : IPlugin
    {
        #region Secure/Unsecure Configuration Setup
        private string _secureConfig = null;
        private string _unsecureConfig = null;

        public CheckNumberValue(string unsecureConfig, string secureConfig)
        {
            _secureConfig = secureConfig;
            _unsecureConfig = unsecureConfig;
        }
        #endregion
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracer = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = factory.CreateOrganizationService(context.UserId);

            try
            {
                if (context.InputParameters.Contains("Target") && (context.InputParameters["Target"] is Entity || context.InputParameters["Target"] is EntityReference))
                {
                    if (context.Depth > 1) return;
                    
                    #region Create
                    if (context.MessageName.ToLower().Equals("create"))
                    {
                        #region Post-Create
                        if (context.Stage == 40)/*Post Create*/
                        {
                            var createPostImage = (Entity)context.PostEntityImages["postImage"];
                            if (createPostImage.Attributes.Contains("uzm_discountrate")) // İndirim Yüzdesi
                                checkNumber((double)createPostImage["uzm_discountrate"]);
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
                            //var updatePostImage = (Entity)context.PostEntityImages["postImage"];
                            //if (updatePostImage.Attributes.Contains("uzm_discountrate")) // İndirim Yüzdesi
                            //    checkNumber((double)updatePostImage["uzm_discountrate"]);
                        }
                        #endregion
                        #region Post-Update 
                        if (context.Stage == 40)/*Post Update*/
                        {
                            var updatePostImage = (Entity)context.PostEntityImages["postImage"];
                            if (updatePostImage.Attributes.Contains("uzm_discountrate")) // İndirim Yüzdesi
                                checkNumber((double)updatePostImage["uzm_discountrate"]);
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                throw new InvalidPluginExecutionException(e.Message);
            }
        }

        public void checkNumber(double number)
        {
            if (number > 0 && number % 5 == 0) { }
            else { throw new InvalidPluginExecutionException("\nİstisna İndirim Oranı 5'in katları olmak zorundadır.\n"); }
        }
    }
}
