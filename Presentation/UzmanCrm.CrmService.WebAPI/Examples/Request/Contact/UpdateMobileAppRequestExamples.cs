using Swashbuckle.Examples;
using System;
using UzmanCrm.CrmService.WebAPI.Models.Customer;

namespace UzmanCrm.CrmService.WebAPI.Examples.Request.Contact
{



    public class UpdateMobileAppRequestExamples : IExamplesProvider
    {
        public object GetExamples()
        {
            return new UpdateMobileAppRequest
            {
                ChannelId = Common.Enums.ChannelEnum.MobilApp,
                CrmId = Guid.Empty,
                MobileId = Guid.NewGuid().ToString(),
                MobileAppDownloadedDate = DateTime.Now,

            };

        }
    }
}