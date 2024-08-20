using AutoMapper;
using System.Collections.Generic;
using UzmanCrm.CrmService.Application.Abstractions.Service.EmailService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Domain.Entity.CRM.Contact;

namespace UzmanCrm.CrmService.Application.Service.EmailService.Mapping
{
    public class EmailProfile : Profile
    {
        public EmailProfile()
        {

            this.CreateMap<Response<EmailDto>, Response<Email>>().ReverseMap();
            this.CreateMap<Response<List<EmailDto>>, Response<List<Email>>>().ReverseMap();
            this.CreateMap<EmailDto, Email>()
                .ForMember(_ => _.uzm_emailiyssendstatus, i => i.MapFrom(j => 1))
                .ReverseMap();


            this.CreateMap<EmailSaveRequestDto, EmailDto>()
                .ForMember(_ => _.uzm_contactid, i => i.MapFrom(j => j.CustomerCrmId))
                .ForMember(_ => _.uzm_emailoptindate, i => i.MapFrom(j => j.EmailOptinDate))
                .ForMember(_ => _.uzm_emailaddress, i => i.MapFrom(j => j.EmailAddress))
                .ForMember(_ => _.uzm_emailpermission, i => i.MapFrom(j => j.EmailPermission))
                .ForMember(_ => _.uzm_emailoptinchannelid, i => i.MapFrom(j => j.EmailPermission != null ? GeneralHelper.GetChannelIdByChannelEnum(j.ChannelId) : null))
                .ForMember(_ => _.uzm_emailtype, i => i.MapFrom(j => 1))
                .ForMember(_ => _.uzm_createdbypersonid, i => i.MapFrom(j => j.PersonId))
                .ForMember(_ => _.uzm_modifiedbypersonid, i => i.MapFrom(j => j.PersonId))
                .ForMember(_ => _.uzm_createdbystoreid, i => i.MapFrom(j => j.StoreId))
                .ForMember(_ => _.uzm_modifiedbystoreid, i => i.MapFrom(j => j.StoreId))
                .ForMember(_ => _.uzm_name, i => i.MapFrom(j => j.EmailAddress))
                .ReverseMap();


        }
    }
}
