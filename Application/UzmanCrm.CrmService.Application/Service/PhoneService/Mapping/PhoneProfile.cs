using AutoMapper;
using System.Collections.Generic;
using UzmanCrm.CrmService.Application.Abstractions.Service.PhoneService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Service.PhoneService.Mapping
{
    public class PhoneProfile : Profile
    {
        public PhoneProfile()
        {
            this.CreateMap<PhoneDto, Domain.Entity.CRM.Contact.Phone>()
                .ForMember(_ => _.uzm_iyssend, i => i.MapFrom(j => true))
                .ForMember(_ => _.uzm_calliyssend, i => i.MapFrom(j => true))
                .ReverseMap();

            this.CreateMap<Response<PhoneDto>, Response<Domain.Entity.CRM.Contact.Phone>>().ReverseMap();
            this.CreateMap<Response<List<PhoneDto>>, Response<List<Domain.Entity.CRM.Contact.Phone>>>().ReverseMap();

            this.CreateMap<PhoneDto, ContactPhoneModelDto>()
                .ForMember(_ => _.PhoneNumber, i => i.MapFrom(j => j.uzm_customerphonenumber))
                .ForMember(_ => _.SmsPermit, i => i.MapFrom(j => j.uzm_phonepermission))
                .ForMember(_ => _.CallPermit, i => i.MapFrom(j => j.uzm_iyscallpermit))
                .ForMember(_ => _.SmsPermit, i => i.MapFrom(j => j.uzm_iysphonepermit))
                .ForMember(_ => _.Type, i => i.MapFrom(j => j.uzm_phonetype))
                .ForMember(_ => _.CreatedDate, i => i.MapFrom(j => j.createdon))
                .ForMember(_ => _.UpdatedDate, i => i.MapFrom(j => j.modifiedon))
                .ForMember(_ => _.Channel, i => i.MapFrom(j => j.uzm_datasourceid))
                .ForMember(_ => _.CreatedPerson, i => i.MapFrom(j => j.createdpersonno))
                .ForMember(_ => _.UpdatedPerson, i => i.MapFrom(j => j.updatedpersonno))
                .ReverseMap();

            this.CreateMap<Response<PhoneDto>, Response<ContactPhoneModelDto>>().ReverseMap();
            this.CreateMap<Response<List<PhoneDto>>, Response<List<ContactPhoneModelDto>>>().ReverseMap();


            this.CreateMap<PhoneSaveRequestDto, PhoneDto>()
               .ForMember(_ => _.uzm_customerid, i => i.MapFrom(j => j.CustomerCrmId))
               .ForMember(_ => _.uzm_customerphonenumber, i => i.MapFrom(j => j.PhoneNumber))
               .ForMember(_ => _.uzm_phonepermission, i => i.MapFrom(j => j.SmsPermit))
               .ForMember(_ => _.uzm_iyscallpermit, i => i.MapFrom(j => j.CallPermit))
               .ForMember(_ => _.uzm_iysphonepermit, i => i.MapFrom(j => j.SmsPermit))
               .ForMember(_ => _.uzm_createdlocationid, i => i.MapFrom(j => j.StoreId))
               .ForMember(_ => _.uzm_modifiedbylocationid, i => i.MapFrom(j => j.StoreId))
               .ForMember(_ => _.uzm_createdpersonid, i => i.MapFrom(j => j.PersonId))
               .ForMember(_ => _.uzm_modifiedbypersonid, i => i.MapFrom(j => j.PersonId))
               .ForMember(_ => _.uzm_datasourceid, i => i.MapFrom(j => j.ChannelId))
               .ForMember(_ => _.uzm_releatedpermissionid, i => i.MapFrom(j => j.ReleatedPermissionId))
               .ReverseMap();
        }
    }
}
