using System;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.AddressService.Model
{
    public class AddressSaveRequestDto
    {
        public string ErpId { get; set; } = null;
        public string AddressId { get; set; }
        public CountryCodeEnum CountryCode { get; set; } = CountryCodeEnum.Bilinmiyor;
        public string CityCode { get; set; } = null;
        public string DistrictCode { get; set; } = null;
        public string NeighborhoodCode { get; set; } = null;
        public string AddressLine { get; set; } = null;
        public string PostCode { get; set; } = null;
        public string Location { get; set; } = null;
        public AddressTypeEnum AddressType { get; set; } = AddressTypeEnum.Fatura;
        public ChannelEnum ChannelId { get; set; } = ChannelEnum.Bilinmiyor;
        public bool? IsDefaultAddress { get; set; } = null;
        public StatusType StatusEnum { get; set; } = StatusType.Aktif;

        public Guid? CustomerCrmId { get; set; }
        public Guid? CountryId { get; set; } = null;
        public Guid? CityId { get; set; } = null;
        public Guid? DistrictId { get; set; } = null;
        public Guid? NeighborhoodId { get; set; } = null;
        public Guid? LocationId { get; set; } = null;
        public Guid? PersonId { get; set; } = null;


    }
}




