using AutoMapper;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Domain.Entity.CRM.LoyaltyCard;

namespace UzmanCrm.CrmService.Application.Service.LoyaltyCardService.Mappings
{
    public class LoyaltyCardProfile : Profile
    {
        public LoyaltyCardProfile()
        {
            this.CreateMap<LoyaltyCardDto, LoyaltyCard>().ReverseMap();
            this.CreateMap<Response<LoyaltyCardDto>, LoyaltyCard>()
                .ForMember(dest => dest.uzm_erpid, from => from.MapFrom(j => j.Data.uzm_erpid))
                .ForMember(dest => dest.uzm_contactid, from => from.MapFrom(j => j.Data.uzm_contactid))
                .ForMember(dest => dest.uzm_email, from => from.MapFrom(j => j.Data.uzm_email))
                .ForMember(dest => dest.uzm_mobilephone, from => from.MapFrom(j => j.Data.uzm_mobilephone))
                .ForMember(dest => dest.uzm_cardnumber, from => from.MapFrom(j => j.Data.uzm_cardnumber))
                .ForMember(dest => dest.uzm_statuscode, from => from.MapFrom(j => j.Data.uzm_statuscode))
                .ReverseMap();

            //this.CreateMap<Response<List<EmailDto>>, Response<List<Email>>>().ReverseMap();

            //this.CreateMap<EmailDto, ContactEmailDto>()
            //    .ForMember(_ => _.Email, i => i.MapFrom(j => j.uzm_emailaddress))
            //    .ForMember(_ => _.Permission, i => i.MapFrom(j => j.uzm_emailpermission))
            //    .ForMember(_ => _.CreatedDate, i => i.MapFrom(j => j.createdon))
            //    .ForMember(_ => _.UpdatedDate, i => i.MapFrom(j => j.modifiedon))
            //    .ForMember(_ => _.CreatedPerson, i => i.MapFrom(j => j.createdpersonno))
            //    .ForMember(_ => _.UpdatedPerson, i => i.MapFrom(j => j.updatedpersonno))
            //    .ReverseMap();

            //this.CreateMap<Response<EmailDto>, Response<ContactEmailDto>>().ReverseMap();
            //this.CreateMap<Response<List<EmailDto>>, Response<List<ContactEmailDto>>>().ReverseMap();

            this.CreateMap<LoyaltyCardRequestDto, LoyaltyCardDto>()
                .ForMember(dest => dest.uzm_erpid, from => from.MapFrom(j => j.ErpId))
                .ForMember(dest => dest.uzm_cardtypedefinitionid, from => from.MapFrom(j => GeneralHelper.GetCardIdByCardTypeEnum(j.CardType)))
                .ForMember(dest => dest.uzm_statuscode, from => from.MapFrom(j => j.CardStatusCodeType))
                .ForMember(dest => dest.uzm_contactid, from => from.MapFrom(j => j.CrmId))
                .ForMember(dest => dest.uzm_storecode, from => from.MapFrom(j => j.StoreCode))
                .ReverseMap();

            this.CreateMap<LoyaltyCardUpdateDto, LoyaltyCard>()
                .ForMember(dest => dest.uzm_cardnumber, from => from.MapFrom(j => j.CardNumber))
                .ForMember(dest => dest.uzm_statuscode, from => from.MapFrom(j => j.CardStatusCodeType))
                .ForMember(dest => dest.uzm_cardtypedefinitionid, from => from.MapFrom(j => j.CardTypeId))
                .ForMember(dest => dest.uzm_contactid, from => from.MapFrom(j => j.ContactId))
                .ForMember(dest => dest.uzm_differenceendorsement, from => from.MapFrom(j => j.DifferenceEndorsement))
                .ForMember(dest => dest.uzm_email, from => from.MapFrom(j => j.Email))
                .ForMember(dest => dest.uzm_erpid, from => from.MapFrom(j => j.ErpId))
                .ForMember(dest => dest.uzm_loyaltycardid, from => from.MapFrom(j => j.Id))
                .ForMember(dest => dest.uzm_mobilephone, from => from.MapFrom(j => j.MobilePhone))
                .ForMember(dest => dest.uzm_periodendorsement, from => from.MapFrom(j => j.PeriodEndorsement))
                .ForMember(dest => dest.uzm_turnoverendorsement, from => from.MapFrom(j => j.TurnoverEndorsement))
                .ForMember(dest => dest.uzm_uppersegmentdiscountpercentvakko, from => from.MapFrom(j => j.UpperSegmentDiscountPercentVakko))
                .ForMember(dest => dest.uzm_uppersegmentdiscountpercentvr, from => from.MapFrom(j => j.UpperSegmentDiscountPercentVr))
                .ForMember(dest => dest.uzm_uppersegmentdiscountpercentwcol, from => from.MapFrom(j => j.UpperSegmentDiscountPercentWcol))
                .ForMember(dest => dest.uzm_validdiscountratevakko, from => from.MapFrom(j => j.ValidDiscountRateVakko))
                .ForMember(dest => dest.uzm_validdiscountratevr, from => from.MapFrom(j => j.ValidDiscountRateVr))
                .ForMember(dest => dest.uzm_validdiscountratewcol, from => from.MapFrom(j => j.ValidDiscountRateWcol))
                .ForMember(dest => dest.uzm_amountforuppersegmentvakko, from => from.MapFrom(j => j.AmountForUpperSegmentVakko))
                .ForMember(dest => dest.uzm_amountforuppersegmentvr, from => from.MapFrom(j => j.AmountForUpperSegmentVr))
                .ForMember(dest => dest.uzm_amountforuppersegmentwcol, from => from.MapFrom(j => j.AmountForUpperSegmentWcol))
                .ForMember(dest => dest.uzm_validendorsement, from => from.MapFrom(j => j.ValidEndorsement))
                .ReverseMap();



            this.CreateMap<ContactByErpIdDto, LoyaltyCard>().ReverseMap();
            this.CreateMap<Response<ContactByErpIdDto>, LoyaltyCard>()
                .ForMember(dest => dest.uzm_contactid, from => from.MapFrom(j => j.Data.ContactId))
                .ForMember(dest => dest.uzm_email, from => from.MapFrom(j => j.Data.EMailAddress1))
                .ForMember(dest => dest.uzm_mobilephone, from => from.MapFrom(j => j.Data.MobilePhone))
                .ForMember(dest => dest.uzm_erpid, from => from.MapFrom(j => j.Data.uzm_ErpId))
                .ReverseMap();
        }
    }
}
