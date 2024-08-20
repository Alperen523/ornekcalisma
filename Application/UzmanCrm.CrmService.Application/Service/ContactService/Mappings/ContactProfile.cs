using AutoMapper;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Contact;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Domain.Entity.CRM.Contact;

namespace UzmanCrm.CrmService.Application.Service.ContactService.Mappings
{
    public class ContactProfile : Profile
    {
        public ContactProfile()
        {
            this.CreateMap<ContactDto, GetCustomerResponseDto>()
           .ForMember(_ => _.CrmId, i => i.MapFrom(j => j.contactid))
           .ForMember(_ => _.EmailAddress, i => i.MapFrom(j => j.emailaddress1))
           .ForMember(_ => _.EmailPermit, i => i.MapFrom(j => j.uzm_emailpermission))
           .ForMember(_ => _.SmsPermit, i => i.MapFrom(j => j.uzm_smspermission))
           .ForMember(_ => _.CallPermit, i => i.MapFrom(j => j.uzm_callpermission))
           .ForMember(_ => _.KvkkPermit, i => i.MapFrom(j => j.uzm_kvkkpermission))
           //.ForMember(_ => _.CustomerTypeId, i => i.MapFrom(j => j.uzm_customertypeid))
           .ForMember(_ => _.FirstName, i => i.MapFrom(j => j.firstname))
           .ForMember(_ => _.LastName, i => i.MapFrom(j => j.lastname))
           .ForMember(_ => _.BirthDate, i => i.MapFrom(j => j.birthdate))
           .ForMember(_ => _.GenderId, i => i.MapFrom(j => j.gendercode))
           .ForMember(_ => _.LoyaltyActivationDate, i => i.MapFrom(j => j.uzm_loyaltyactivationdate))
           .ReverseMap();

            this.CreateMap<Response<ContactDto>, Response<GetCustomerResponseDto>>().ReverseMap();

            this.CreateMap<SaveCustomerRequestDto, Contact>()
              .ForMember(_ => _.firstname, i => i.MapFrom(j => j.FirstName))
              .ForMember(_ => _.contactid, i => i.MapFrom(j => j.CrmId))
              .ForMember(_ => _.lastname, i => i.MapFrom(j => j.LastName))
              .ForMember(_ => _.birthdate, i => i.MapFrom(j => j.BirthDate))
              .ForMember(_ => _.gendercode, i => i.MapFrom(j => j.GenderId))
              .ForMember(_ => _.emailaddress1, i => i.MapFrom(j => j.EmailAddress))
              .ForMember(_ => _.mobilephone, i => i.MapFrom(j => j.MobilePhone))
              .ForMember(_ => _.uzm_mobileid, i => i.MapFrom(j => j.MobileId))
              .ForMember(_ => _.uzm_mobileappdownloadeddate, i => i.MapFrom(j => j.MobileAppDownloadedDate))
              .ForMember(_ => _.uzm_kvkkpermission, i => i.MapFrom(j => j.KvkkPermit))
              .ForMember(_ => _.uzm_smspermission, i => i.MapFrom(j => j.SmsPermit))
              .ForMember(_ => _.uzm_smspermissionsource, i => i.MapFrom(j => j.SmsPermit != null ? GeneralHelper.GetChannelIdByChannelEnum(j.ChannelId) : null))
              .ForMember(_ => _.uzm_smspermissiondate, i => i.MapFrom(j => j.SmsPermitDate))
              .ForMember(_ => _.uzm_callpermission, i => i.MapFrom(j => j.CallPermit))
              .ForMember(_ => _.uzm_callpermissionsource, i => i.MapFrom(j => j.CallPermit != null ? GeneralHelper.GetChannelIdByChannelEnum(j.ChannelId) : null))
              .ForMember(_ => _.uzm_callpermissiondate, i => i.MapFrom(j => j.CallPermitDate))
              .ForMember(_ => _.uzm_emailpermission, i => i.MapFrom(j => j.EmailPermit))
              .ForMember(_ => _.uzm_emailpermissionsource, i => i.MapFrom(j => j.EmailPermit != null ? GeneralHelper.GetChannelIdByChannelEnum(j.ChannelId) : null))
              .ForMember(_ => _.uzm_emailpermissiondate, i => i.MapFrom(j => j.EmailPermitDate))
              .ForMember(_ => _.uzm_smspermissionissend, i => i.MapFrom(j => j.SmsPermit != null ? true : false))
              .ForMember(_ => _.uzm_callpermissionissend, i => i.MapFrom(j => j.CallPermit != null ? true : false))
              .ForMember(_ => _.uzm_emailpermissionissend, i => i.MapFrom(j => j.EmailPermit != null ? true : false))
              .ForMember(_ => _.uzm_createdbypersonid, i => i.MapFrom(j => j.PersonId))
              .ForMember(_ => _.uzm_modifiedbypersonid, i => i.MapFrom(j => j.PersonId))
              .ForMember(_ => _.uzm_createdbystoreid, i => i.MapFrom(j => j.StoreId))
              .ForMember(_ => _.uzm_modifiedbystoreid, i => i.MapFrom(j => j.StoreId))
              .ForAllOtherMembers(_ => _.Ignore());



            //old...

            //this.CreateMap<LoyaltyCardInfoOnContactRequest, Contact>()
            //    .ForMember(dest => dest.contactid, from => from.MapFrom(j => j.contactid))
            //    //.ForMember(dest => dest.uzm_loyaltycardid, from => from.MapFrom(j => j.uzm_loyaltycardid))
            //    .ReverseMap();

            //this.CreateMap<SaveCustomerRequestDto, ContactSaveRequestDto>()
            //   .ForMember(_ => _.OrganizationId, i => i.MapFrom(j => CompanyEnum.KD))

            //   .ForMember(_ => _.CustomerType, i => i.MapFrom(j => j.CustomerType))

            //   .ForMember(_ => _.IsKvkk, i => i.MapFrom(j => j.IsKvkk))
            //   .ForMember(_ => _.BirthDate, i => i.MapFrom(j => j.BirthDate))
            //    .AfterMap((src, dest) =>
            //    {
            //        dest.Phone.PhoneNumber = src.PhoneNumber;
            //        dest.Phone.SmsPermit = src.SmsPermit;
            //        dest.Phone.CallPermit = src.CallPermit;
            //    })
            //   .AfterMap((src, dest) =>
            //   {
            //       dest.Email.EmailAddress = src.EmailAddress;
            //       dest.Email.EmailPermit = src.EmailPermit;
            //   });

            //this.CreateMap<SaveCustomerRequestDto, ContactSaveRequestDto>()
            //.ForMember(_ => _.OrganizationId, i => i.MapFrom(j => CompanyEnum.KD))
            //.ForMember(_ => _.CardType, i => i.MapFrom(j => GeneralHelper.GetLocationIdByCardTypeEnum(j.PersonNo)))
            //.ForMember(_ => _.PersonNo, i => i.MapFrom(j => j.PersonNo))
            // .ForMember(_ => _.CustomerType, i => i.MapFrom(j => j.CustomerType))
            //.ForMember(_ => _.IsKvkk, i => i.MapFrom(j => j.IsKvkk))
            //.ForMember(_ => _.BirthDate, i => i.MapFrom(j => j.BirthDate))
            // .AfterMap((src, dest) =>
            // {
            //     dest.Phone.PhoneNumber = src.PhoneNumber;
            //     dest.Phone.SmsPermit = src.SmsPermit;
            //     dest.Phone.CallPermit = src.CallPermit;
            // })
            //.AfterMap((src, dest) =>
            //{
            //    dest.Email.EmailAddress = src.EmailAddress;
            //    dest.Email.EmailPermit = src.EmailPermit;
            //});

            //this.CreateMap<ContactSaveRequestDto, Contact>()
            //  //.ForMember(_ => _.uzm_erpid, i => i.MapFrom(j => j.ErpId))
            //  //.ForMember(_ => _.uzm_customerno, i => i.MapFrom(j => j.CustomerNo))
            //  //.ForMember(_ => _.uzm_customertype, i => i.MapFrom(j => j.CustomerType))
            //  //.ForMember(_ => _.uzm_identificationnumber, i => i.MapFrom(j => j.IdentificationNumber))
            //  .ForMember(_ => _.firstname, i => i.MapFrom(j => j.FirstName))
            //  .ForMember(_ => _.contactid, i => i.MapFrom(j => j.CrmId))
            //  .ForMember(_ => _.lastname, i => i.MapFrom(j => j.LastName))
            //  .ForMember(_ => _.birthdate, i => i.MapFrom(j => j.BirthDate))
            //  .ForMember(_ => _.gendercode, i => i.MapFrom(j => j.GenderId))
            //  //.ForMember(_ => _.uzm_personelnumber, i => i.MapFrom(j => j.PersonNo))
            //  .ForMember(_ => _.mobilephone, i => i.MapFrom(j => j.Phone.Type == PhoneTypeEnum.MobilePhone ? j.Phone.PhoneNumber : null))
            //  .ForMember(_ => _.telephone1, i => i.MapFrom(j => j.Phone.Type == PhoneTypeEnum.LandPhone ? j.Phone.PhoneNumber : null))
            //  .ForMember(_ => _.emailaddress1, i => i.MapFrom(j => j.Email.EmailAddress))
            //  //.ForMember(_ => _.uzm_isonline, i => i.MapFrom(j => ContactHelper.IsOnlineContact(j.ChannelId)))
            //  //.ForMember(_ => _.uzm_isoffline, i => i.MapFrom(j => ContactHelper.IsOfflineContact(j.ChannelId)))
            //  //.ForMember(_ => _.uzm_iskvkk, i => i.MapFrom(j => j.IsKvkk))
            //  .ForAllOtherMembers(_ => _.Ignore());

            //this.CreateMap<ContactSaveRequestDto, Customer>()
            //.ForMember(_ => _.erpid, i => i.MapFrom(j => j.ErpId))
            //.ForMember(_ => _.customerno, i => i.MapFrom(j => j.CustomerNo))
            //.ForMember(_ => _.customertype, i => i.MapFrom(j => j.CustomerType))
            //.ForMember(_ => _.ecomid, i => i.MapFrom(j => j.EcomId))
            //.ForMember(_ => _.tckno, i => i.MapFrom(j => j.IdentificationNumber))
            //.ForMember(_ => _.crmid, i => i.MapFrom(j => j.CrmId))
            //.ForMember(_ => _.firstname, i => i.MapFrom(j => j.FirstName))
            //.ForMember(_ => _.lastname, i => i.MapFrom(j => j.LastName))
            //.ForMember(_ => _.birthdate, i => i.MapFrom(j => j.BirthDate))
            //.ForMember(_ => _.genderid, i => i.MapFrom(j => j.GenderId))
            //.ForMember(_ => _.personelnumber, i => i.MapFrom(j => j.PersonNo))
            //.ForMember(_ => _.createduser, i => i.MapFrom(j => j.PersonNo))
            //.ForMember(_ => _.gsm1, i => i.MapFrom(j => j.Phone.Type == PhoneTypeEnum.MobilePhone ? j.Phone.PhoneNumber : null))
            //.ForMember(_ => _.contactablegsm1, i => i.MapFrom(j => (j.Phone.SmsPermit.HasValue ? j.Phone.SmsPermit.Value : false) && j.Phone.Type == PhoneTypeEnum.MobilePhone ? true : false))
            //.ForMember(_ => _.homephone1, i => i.MapFrom(j => j.Phone.Type == PhoneTypeEnum.LandPhone ? j.Phone.PhoneNumber : null))
            //.ForMember(_ => _.contactablehomephone1, i => i.MapFrom(j => (j.Phone.SmsPermit.HasValue ? j.Phone.SmsPermit.Value : false) && j.Phone.Type == PhoneTypeEnum.LandPhone ? true : false))
            //.ForMember(_ => _.email1, i => i.MapFrom(j => j.Email.EmailAddress))
            //.ForMember(_ => _.channelid, i => i.MapFrom(j => ContactHelper.GetContactChannelId(j.ChannelId)))
            //.ForMember(_ => _.createdlocation, i => i.MapFrom(j => j.Location))
            //.ForMember(_ => _.isonline, i => i.MapFrom(j => ContactHelper.IsOnlineContact(j.ChannelId)))
            //.ForMember(_ => _.isoffline, i => i.MapFrom(j => ContactHelper.IsOnlineContact(j.ChannelId)))
            // .ForMember(_ => _.iskvkk, i => i.MapFrom(j => j.IsKvkk))
            //.ForAllOtherMembers(_ => _.Ignore());


            //this.CreateMap<ContactSaveRequestDto, DataSourceDto>()
            //  .ForMember(_ => _.uzm_customerid, i => i.MapFrom(j => j.CrmId))
            //  .ForMember(_ => _.uzm_customerexternalid, i => i.MapFrom(j => j.EcomId))
            //  .ForMember(_ => _.uzm_phone, i => i.MapFrom(j => j.Phone.PhoneNumber))
            //  .ForMember(_ => _.uzm_email, i => i.MapFrom(j => j.Email.EmailAddress))
            //   .ForMember(_ => _.uzm_datasourceid, i => i.MapFrom(j => GeneralHelper.GetLocationIdByDataSourceId(j.Location)))
            //  .ForAllOtherMembers(_ => _.Ignore());

            //this.CreateMap<DataSourceDto, DataSource>().ReverseMap();


            //this.CreateMap<CrmCustomerFormRequestDto, CustomerPermission>()
            // .ForMember(_ => _.uzm_customerid, i => i.MapFrom(j => j.CrmId))
            // .ForMember(_ => _.uzm_documentno, i => i.MapFrom(j => j.FormNo))
            // .ForMember(_ => _.uzm_type, i => i.MapFrom(j => j.FormType))
            // .ForMember(_ => _.uzm_createdlocationid, i => i.MapFrom(j => j.CreatedLocation))
            // .ForMember(_ => _.uzm_createdpersonid, i => i.MapFrom(j => j.CreatedPersonId))
            // .ForMember(_ => _.uzm_approvaldate, i => i.MapFrom(j => j.ApprovalDate))
            // .ForMember(_ => _.uzm_approvalnumber, i => i.MapFrom(j => j.ApprovalNumber))
            // .ForMember(_ => _.uzm_statuscode, i => i.MapFrom(j => 1)) // 1 :İmzalı İzinli
            // .ForAllOtherMembers(_ => _.Ignore());

        }

    }
}
