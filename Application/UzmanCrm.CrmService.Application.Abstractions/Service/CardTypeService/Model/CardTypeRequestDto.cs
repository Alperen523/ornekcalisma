using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.CardTypeService.Model
{
    /// <summary>
    /// Kart Tipi istek modeli
    /// </summary>
    public class CardTypeRequestDto
    {
        public CardTypeEnum CardType { get; set; } = CardTypeEnum.Unknown;

        public string CardTypeCode { get; set; } = null;
    }
}
