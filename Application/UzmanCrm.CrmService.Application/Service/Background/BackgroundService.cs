using AutoMapper;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using UzmanCrm.CrmService.Application.Abstractions.Service.BackgroundService;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardClassSegmentService;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardTypeService;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService;
using UzmanCrm.CrmService.Application.Abstractions.Service.EmailService;
using UzmanCrm.CrmService.Application.Abstractions.Service.EmailService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.EndorsementService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltySpecificationService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Abstractions.Service.WagescaleService;
using UzmanCrm.CrmService.Application.Abstractions.Service.WageScaleService.Model;
using UzmanCrm.CrmService.Common;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.Common.Helpers;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;
using Vakko.CrmService.Application.Abstractions.Service.BatchApprovalList;
using Vakko.CrmService.Application.Abstractions.Service.BatchApprovalList.Model;

namespace UzmanCrm.CrmService.Application.Service.Background
{
    public class BackgroundService : IBackgroundService
    {
        private readonly IMapper mapper;
        private readonly IDapperService dapperService;
        private readonly IWageScaleService wageScaleService;
        private readonly ILoyaltyCardService loyaltyCardService;
        private readonly ICardTypeService cardTypeService;
        private readonly ICardExceptionDiscountService cardExceptionDiscountService;
        private readonly IContactService contactService;
        private readonly IEndorsementService endorsementService;
        private readonly ILogService logService;
        private readonly ILoyaltySpecificationService loyaltySpecService;
        private readonly IEmailService emailService;
        private readonly ICardClassSegmentService cardClassSegmentService;
        private readonly IBatchApprovalListService batchApprovalListService;

        public BackgroundService(
            IMapper mapper,
            IDapperService dapperService,
            IWageScaleService wageScaleService,
            ILoyaltyCardService loyaltyCardService,
            ICardTypeService cardTypeService,
            ICardExceptionDiscountService cardExceptionDiscountService,
            IContactService contactService,
            IEndorsementService endorsementService,
            ILogService logService,
            ILoyaltySpecificationService loyaltySpecService,
            IEmailService emailService,
            ICardClassSegmentService cardClassSegmentService,
            IBatchApprovalListService batchApprovalListService)
        {
            this.mapper = mapper;
            this.dapperService = dapperService;
            this.wageScaleService = wageScaleService;
            this.loyaltyCardService = loyaltyCardService;
            this.cardTypeService = cardTypeService;
            this.cardExceptionDiscountService = cardExceptionDiscountService;
            this.contactService = contactService;
            this.endorsementService = endorsementService;
            this.logService = logService;
            this.loyaltySpecService = loyaltySpecService;
            this.emailService = emailService;
            this.cardClassSegmentService = cardClassSegmentService;
            this.batchApprovalListService = batchApprovalListService;
        }

        public async Task<Response<object>> Customer_Endorsement_Data_Processing()
        {
            var result = new Response<object>();

            // 1- [uzm_integrationstatus] = 0 olan datalar çekilecek
            var dataToProcessedRes = await endorsementService.GetUnintegratedEndorsementList();
            if (dataToProcessedRes.Success)
            {
                var discountFixingFlag = false; // Sadakat Kart Tanımı kaydı üzerindeki Barem İndirim Artışı Sabitleme flagi

                #region BillType Liste Tanımlamaları

                // ZP01 = 1, // P.Satış - Normal Fatura
                // ZP02 = 2, // P.Satış - KDVsiz Fatura
                // ZP03 = 3, // P.Satış - Taxfree Fatura
                // ZP04 = 4, // P.Satış - Web Fatura
                // ZP05 = 5, // P.Satış - E-fatura
                // ZP08 = 6, // P.Satış - Gift Kart
                // ZP09 = 7, // P.Satış - Toplu Gift Kart
                // ZP12 = 8, // P.Satış - RB Değişim

                // ZR01 = 11, // P.İade -  Normal Fatura
                // ZR02 = 12, // P.İade -  KDVsiz Fatura
                // ZR03 = 13, // P.İade -  Taxfree Fatura
                // ZR04 = 14, // P.İade -  Web Fatura
                // ZR05 = 15, // P.İade -  E-fatura
                // ZR08 = 16, // P.İade -  Gift Kart
                // ZR09 = 17, // P.İade -  Toplu Gift Kart
                // ZR12 = 18, // P.İade -  RB Değişim


                // Geçerli fatura tipi listesi
                var validBillTypeList = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 11, 12, 13, 14, 15, 16, 17, 18 });

                // Satış fatura tipi listesi
                var saleBillTypeList = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8 });

                // İade fatura tipi listesi
                var returnBillTypeList = new List<int>(new int[] { 11, 12, 13, 14, 15, 16, 17, 18 });


                #endregion

                // Şu an itibariyle şimdiden küçük en güncel Sadakat Kart Tanımı kaydını çekip Barem İndirim Artışı Sabitleme seçeneği
                // aktif mi kontolü yapılır. Aktifse ciro hesaplama ignore edilir

                var validLoyaltySpecification = await loyaltySpecService.ValidLoyaltySpecificationGetItem();
                if (validLoyaltySpecification.Success)
                    discountFixingFlag = (bool)validLoyaltySpecification.Data.uzm_discountfixingflag;

                //var endorsementTasks = dataToProcessedRes.Data.Select((endorsement) => Task.Run(async () =>
                //await ParallelAsync.ForEachAsync(dataToProcessedRes.Data, async (endorsement) => 

                for (int i = 0; i < dataToProcessedRes.Data.Count; i++)
                {
                    var endorsement = dataToProcessedRes.Data[i];
                    var endorsementId = endorsement.uzm_customerendorsementid.ToString();

                    #region Değişkenleri initialize etme
                    var validEndorsement = 0.0; // Geçerli Ciro
                    var periodEndorsement = 0.0; // Dönem Ciro
                    var turnoverEndorsement = 0.0; // Devir Ciro
                    var cardExceptionDiscountRate = 0.0; // Kart İstisna Tanımı İndirim Oranı
                    var differenceEndorsement = 0.0; // Fark Ciro

                    double? validDiscountRateVakko = 0.0; // Vakko Geçerli İndirim Oranı
                    double? validDiscountRateVr = 0.0; // Vakkorama Geçerli İndirim Oranı
                    double? validDiscountRateWcol = 0.0; // WColl Geçerli İndirim Oranı

                    double? upperSegmentDiscountPercentVakko = 0.0; // Vakko Bir Sonraki İndirim Oranı
                    double? upperSegmentDiscountPercentVr = 0.0; // Vakkorama Bir Sonraki İndirim Oranı
                    double? upperSegmentDiscountPercentWcol = 0.0; // WColl Bir Sonraki İndirim Oranı

                    double? amountForUpperSegmentVakko = 0.0; // Vakko Üst Limit İçin Gereken Harcama
                    double? amountForUpperSegmentVr = 0.0; // Vakkorama Üst Limit İçin Gereken Harcama
                    double? amountForUpperSegmentWcol = 0.0; // WColl Üst Limit İçin Gereken Harcama

                    var wagescaleidVakko = Guid.Empty; // Denk gelen Vakko Marka İndirim Barem Detayı
                    var wagescaleidVr = Guid.Empty; // Denk gelen Vakkorama Marka İndirim Barem Detayı
                    var wagescaleidWcol = Guid.Empty; // Denk gelen W Collection Marka İndirim Barem Detayı
                    #endregion

                    if (discountFixingFlag)
                    {
                        var endorsementIntegrationStateRes = await endorsementService.SetEndorsementIntegrationStateWithSqlAsync(
                            1,
                            $"Geçerli olan {validLoyaltySpecification.Data.uzm_name} isimli Sadakat Kart Tanımı kaydında Barem İndirim Artış Sabitleme seçeneği aktif olduğu için ciro hesaplama ignore edilmiştir.",
                            endorsementId,
                            discountFixingFlag);
                    }
                    else
                    {
                        if (validBillTypeList.Exists(x => x == endorsement.uzm_billtype))
                        {
                            var resService = await contactService.GetContactItemByErpIdAsync(endorsement.uzm_erpid);
                            if (resService.Success)
                            {
                                var erpId = endorsement.uzm_erpid;
                                if (resService?.Data?.uzm_customertype == "Z")
                                {
                                    var mainContactRes = await contactService.GetMainContactItemBySubContactIdAsync(resService?.Data?.ContactId.ToString());
                                    if (mainContactRes.Success)
                                        erpId = mainContactRes.Data.uzm_ErpId;
                                }

                                // 4- Kart Numarası kullanılarak kart üzerindeki bilgiler ve (Dönem Ciro) bilgisi getirilecek.
                                var loyaltyCardInfoRes = await loyaltyCardService.GetLoyaltyCardByCardNoAsync(endorsement.uzm_cardno);

                                if (loyaltyCardInfoRes.Success)
                                {
                                    turnoverEndorsement = loyaltyCardInfoRes?.Data?.uzm_turnoverendorsement == null ? turnoverEndorsement
                                        : (double)loyaltyCardInfoRes?.Data?.uzm_turnoverendorsement;
                                    periodEndorsement = loyaltyCardInfoRes?.Data?.uzm_periodendorsement == null ? periodEndorsement
                                        : (double)loyaltyCardInfoRes?.Data?.uzm_periodendorsement;

                                    validDiscountRateVakko = loyaltyCardInfoRes?.Data?.uzm_validdiscountratevakko == null ? validDiscountRateVakko
                                        : loyaltyCardInfoRes?.Data?.uzm_validdiscountratevakko;
                                    validDiscountRateVr = loyaltyCardInfoRes?.Data?.uzm_validdiscountratevr == null ? validDiscountRateVr
                                        : loyaltyCardInfoRes?.Data?.uzm_validdiscountratevr;
                                    validDiscountRateWcol = loyaltyCardInfoRes?.Data?.uzm_validdiscountratewcol == null ? validDiscountRateWcol
                                        : loyaltyCardInfoRes?.Data?.uzm_validdiscountratewcol;

                                    // 5- SAP den gelen işlenecek datalar üzerindeki [Toplam Tutar - Gift Tutar] + [Dönem Ciro (Loyalty Kart Üzerindeki Alan)]  formülü ile  Dönem Ciro bilgisi hesaplanacaktır. Fatura tipi iade ise cirodan gelen tutar çıkartılacak, satış ise eklenecektir.

                                    if (saleBillTypeList.Exists(x => x == endorsement.uzm_billtype))
                                        periodEndorsement += (double)endorsement.uzm_totalamount - (double)endorsement?.uzm_giftcardamount;
                                    else if (returnBillTypeList.Exists(x => x == endorsement.uzm_billtype))
                                        periodEndorsement -= (double)endorsement.uzm_totalamount - (double)endorsement?.uzm_giftcardamount;

                                    // 6- Loyalty Kart üzerindeki Devir Ciro ile hesaplanan Dönem Ciro karşılaştırılıp büyük olan Ciro geçerli olacak ve Barem aralık bilgisi buna göre hesaplanacaktır.
                                    validEndorsement = periodEndorsement > turnoverEndorsement ? periodEndorsement : turnoverEndorsement;
                                    differenceEndorsement = turnoverEndorsement - periodEndorsement;

                                    // 11- Loyalty Kart üzerindeki Dönem Ciro ve Geçerli indirim uzm_loyaltycard varlığında güncellenecektir.
                                    var updateReq = new LoyaltyCardUpdateDto();
                                    updateReq.Id = (Guid)loyaltyCardInfoRes.Data.uzm_loyaltycardid;
                                    updateReq.ValidEndorsement = validEndorsement; // Geçerli Ciro
                                    updateReq.PeriodEndorsement = periodEndorsement; // Dönem Ciro
                                    updateReq.DifferenceEndorsement = differenceEndorsement; // Fark Ciro

                                    updateReq.CustomerEndorsementId = (Guid)endorsement.uzm_customerendorsementid; // Müşteri Ciro Id

                                    var loyaltyCardUpdateRes = await loyaltyCardService.LoyaltyCardEndorsementFieldsUpdateWithSqlAsync(updateReq);

                                    // 2- Kart tipine göre ilgili Barem listesi çekilecek
                                    var wagescaleListRes = await wageScaleService.ValidWageScaleGetList();
                                    if (wagescaleListRes.Success)
                                    {
                                        // 8 Kart İstisna tablosu Erp Id bilgisi ile sorgulanacak, Kayıt aktif mi?,Kayıt güncel tarih başlangıç ve bitiş tarihi aralığı içinde mi?,İndirim Onay durumu :Onaylı kayıtlar mı?
                                        var cardExceptionDiscountRes = await cardExceptionDiscountService.GetCardExceptionDiscountDetailByErpIdAsync(erpId);
                                        if (cardExceptionDiscountRes.Success)
                                            cardExceptionDiscountRate = (double)cardExceptionDiscountRes.Data.uzm_discountrate;

                                        if (loyaltyCardInfoRes.Data.uzm_cardtypedefinitionidname.IsNotNullAndEmpty())
                                        {
                                            var loyaltyCardCardType = (CardTypeEnum)Enum.Parse(typeof(CardTypeEnum), loyaltyCardInfoRes.Data.uzm_cardtypedefinitionidname);
                                            var cardTypeEnumList = Enum.GetValues(typeof(CardTypeEnum)).Cast<CardTypeEnum>().ToList();
                                            foreach (var cardTypeEnum in cardTypeEnumList)
                                            {
                                                if (cardTypeEnum == CardTypeEnum.Unknown)
                                                    continue;

                                                // Kart tipi W ise V ve R tipi için gereksiz hesaplama yapma
                                                if (loyaltyCardCardType == CardTypeEnum.W && (cardTypeEnum == CardTypeEnum.V || cardTypeEnum == CardTypeEnum.R))
                                                    continue;

                                                // Kart tipi V veya R ise W tipi için gereksiz hesaplama yapma
                                                if ((loyaltyCardCardType == CardTypeEnum.V || loyaltyCardCardType == CardTypeEnum.R) && cardTypeEnum == CardTypeEnum.W)
                                                    continue;

                                                var selectedWagescaleValidDiscountRate = 0.0; // Hangi marka için barem sorgusu yapılıyorsa ona denk gelen Geçerli İndirim Oranı
                                                var selectedWagescaleUpperSegmentDiscountPercent = 0.0; // Hangi marka için barem sorgusu yapılıyorsa ona denk gelen Bir Sonraki İndirim Oranı
                                                var selectedWagescaleAmountForUpperSegment = 0.0; // Hangi marka için barem sorgusu yapılıyorsa ona denk gelen Üst Limit İçin Gereken Harcama
                                                var selectedWagescaleId = Guid.Empty; // Hangi marka için barem sorgusu yapılıyorsa ona denk gelen Marka İndirim Barem Detayı

                                                // 7- Değişken olarak Geçerli Ciro bilgisi ile Kart tipine göre eşleşen barem satırı bilgisi getirilecektir.
                                                //var selectedWagescaleRes = await wageScaleService.GetWagescaleByEndorsement(wagescaleListRes.Data, validEndorsement, (Guid)loyaltyCardInfoRes?.Data?.uzm_cardtypedefinitionid);
                                                var selectedWagescaleRes = await wageScaleService.GetValidWagescale(wagescaleListRes.Data, (double)validEndorsement, cardTypeEnum, (Guid)loyaltyCardInfoRes?.Data?.uzm_cardtypedefinitionid, loyaltyCardCardType);

                                                if (!selectedWagescaleRes.Success)
                                                {
                                                    // var minWageScale = await wageScaleService.GetMinWagescaleByCardType(wagescaleListRes.Data, (Guid)loyaltyCardInfoRes?.Data?.uzm_cardtypedefinitionid);
                                                    selectedWagescaleRes = new Response<ValidWageScaleListDto>
                                                    {
                                                        Data = new ValidWageScaleListDto
                                                        {
                                                            uzm_cardtypedefinitionid = loyaltyCardInfoRes?.Data?.uzm_cardtypedefinitionid,
                                                            uzm_discountrate = 0,
                                                            uzm_rangestart = 0,
                                                            uzm_rangeend = 0, // minWageScale?.Data?.uzm_rangestart != null ? minWageScale.Data.uzm_rangestart : 0,
                                                            uzm_wagescaleid = Guid.Empty,
                                                        },
                                                        Error = null,
                                                        Message = null,
                                                        Success = true
                                                    };
                                                }
                                                // Eşleşen barem kaydının id bilgisi
                                                selectedWagescaleId = (Guid)selectedWagescaleRes?.Data?.uzm_wagescaleid;

                                                // Eşleşen barem kaydından indirim oranını getirme
                                                selectedWagescaleValidDiscountRate = (double)selectedWagescaleRes.Data.uzm_discountrate;

                                                // 9- Kart İstisna tablosu için indirim bulundunduktan sonra , Bulunan Barem indirim oranı bilgisi ile karşılaştırılıp büyük olan indirim oranı bulunup Loyalty kart varlığındaki geçerli indirim bilgisi güncellenecektir 
                                                selectedWagescaleValidDiscountRate = selectedWagescaleValidDiscountRate > cardExceptionDiscountRate ? selectedWagescaleValidDiscountRate : cardExceptionDiscountRate;


                                                // 10- Loyalty Kart (uzm_loyaltycard) üzerindeki Üst Segment alanları ekstra hesaplanıp güncellenecek , Güncellenecek değerler Mevcut baremdeki bir üst barem bilgisi olmalı.
                                                // Üst baremi bulmak için: Kart tipine göre Barem Alt Sınırı geçerli cirodan büyük, en küçük İndirim Oranına sahip baremi bulma sorgusu atılır.

                                                var upperWagescaleRes = await wageScaleService.GetValidUpperWagescale(wagescaleListRes.Data, (double)validEndorsement, cardTypeEnum, (Guid)loyaltyCardInfoRes?.Data?.uzm_cardtypedefinitionid, loyaltyCardCardType);

                                                if (upperWagescaleRes.Success)
                                                {
                                                    selectedWagescaleAmountForUpperSegment = (double)upperWagescaleRes.Data.uzm_rangestart - periodEndorsement;
                                                    selectedWagescaleUpperSegmentDiscountPercent = (double)upperWagescaleRes.Data.uzm_discountrate;
                                                }

                                                switch (cardTypeEnum)
                                                {
                                                    case CardTypeEnum.V:
                                                        wagescaleidVakko = selectedWagescaleId;
                                                        validDiscountRateVakko = selectedWagescaleValidDiscountRate;
                                                        amountForUpperSegmentVakko = selectedWagescaleAmountForUpperSegment;
                                                        upperSegmentDiscountPercentVakko = selectedWagescaleUpperSegmentDiscountPercent;
                                                        break;
                                                    case CardTypeEnum.R:
                                                        wagescaleidVr = selectedWagescaleId;
                                                        validDiscountRateVr = selectedWagescaleValidDiscountRate;
                                                        amountForUpperSegmentVr = selectedWagescaleAmountForUpperSegment;
                                                        upperSegmentDiscountPercentVr = selectedWagescaleUpperSegmentDiscountPercent;
                                                        break;
                                                    case CardTypeEnum.W:
                                                        wagescaleidWcol = selectedWagescaleId;
                                                        validDiscountRateWcol = selectedWagescaleValidDiscountRate;
                                                        amountForUpperSegmentWcol = selectedWagescaleAmountForUpperSegment;
                                                        upperSegmentDiscountPercentWcol = selectedWagescaleUpperSegmentDiscountPercent;
                                                        break;
                                                    default:
                                                        break;
                                                }
                                            }

                                            // Kart tipi W ise V ve R tipi alanları boşalt
                                            if (loyaltyCardCardType == CardTypeEnum.W)
                                            {
                                                validDiscountRateVakko = null;
                                                validDiscountRateVr = null;

                                                upperSegmentDiscountPercentVakko = null;
                                                upperSegmentDiscountPercentVr = null;

                                                amountForUpperSegmentVakko = null;
                                                amountForUpperSegmentVr = null;
                                            }

                                            // Kart tipi V veya R ise W tipi için alanları boşalt
                                            if (loyaltyCardCardType == CardTypeEnum.V || loyaltyCardCardType == CardTypeEnum.R)
                                            {
                                                validDiscountRateWcol = null;
                                                upperSegmentDiscountPercentWcol = null;
                                                amountForUpperSegmentWcol = null;
                                            }
                                        }
                                    }

                                    // 11- Loyalty Kart üzerindeki Dönem Ciro ve Geçerli indirim uzm_loyaltycard varlığında güncellenecektir.
                                    var updateDiscountFieldsReq = new LoyaltyCardUpdateDto();
                                    updateDiscountFieldsReq.Id = (Guid)loyaltyCardInfoRes.Data.uzm_loyaltycardid;

                                    updateDiscountFieldsReq.ValidDiscountRateVakko = validDiscountRateVakko; // Vakko Geçerli İndirim Yüzdesi
                                    updateDiscountFieldsReq.ValidDiscountRateVr = validDiscountRateVr; // Vakkorama Geçerli İndirim Yüzdesi
                                    updateDiscountFieldsReq.ValidDiscountRateWcol = validDiscountRateWcol; // W Collection Geçerli İndirim Yüzdesi

                                    updateDiscountFieldsReq.AmountForUpperSegmentVakko = amountForUpperSegmentVakko; // Vakko Üst Segment İçin Kalan Ciro
                                    updateDiscountFieldsReq.AmountForUpperSegmentVr = amountForUpperSegmentVr; // Vakko Üst Segment İçin Kalan Ciro
                                    updateDiscountFieldsReq.AmountForUpperSegmentWcol = amountForUpperSegmentWcol; // Vakko Üst Segment İçin Kalan Ciro

                                    updateDiscountFieldsReq.UpperSegmentDiscountPercentVakko = upperSegmentDiscountPercentVakko; // Vakko Üst Segment İndirim Oranı
                                    updateDiscountFieldsReq.UpperSegmentDiscountPercentVr = upperSegmentDiscountPercentVr; // Vakkoramaa Üst Segment İndirim Oranı
                                    updateDiscountFieldsReq.UpperSegmentDiscountPercentWcol = upperSegmentDiscountPercentWcol; // W Collection Üst Segment İndirim Oranı

                                    var loyaltyCardDiscountFieldsUpdateRes = await loyaltyCardService.LoyaltyCardDiscountFieldsUpdateWithSqlAsync(updateDiscountFieldsReq);

                                    if (loyaltyCardUpdateRes.Success)
                                    {
                                        try
                                        {
                                            // 12- Bulunan BaremId bilgisi uzm_customerwagescale tablosuna ContactId ile historik olarak eklenecektir.(GÜncel dönem için Aktiflik Durumu Aktif tir.)
                                            var req = new CreateCustomerWageScaleRequestDto
                                            {
                                                ActivityStatus = true,
                                                CardDiscount_DiscountRate = cardExceptionDiscountRate,
                                                EndorsementId = (Guid)endorsement?.uzm_customerendorsementid,
                                                LoyaltyCardId = loyaltyCardInfoRes?.Data?.uzm_loyaltycardid,
                                                PeriodEndorsement = periodEndorsement,
                                                TurnoverEndorsement = turnoverEndorsement,
                                                ValidDiscountRateVakko = validDiscountRateVakko,
                                                ValidDiscountRateVr = validDiscountRateVr,
                                                ValidDiscountRateWcol = validDiscountRateWcol
                                            };

                                            if (wagescaleidVakko != null && wagescaleidVakko != Guid.Empty)
                                                req.WageScaleIdVakko = wagescaleidVakko;
                                            if (wagescaleidVr != null && wagescaleidVr != Guid.Empty)
                                                req.WageScaleIdVr = wagescaleidVr;
                                            if (wagescaleidWcol != null && wagescaleidWcol != Guid.Empty)
                                                req.WageScaleIdWcol = wagescaleidWcol;

                                            var createCustomerWagescaleRes = await wageScaleService.CreateCustomerWagescaleAsync(req);

                                            result.Message = CommonStaticConsts.Message.Success;
                                        }
                                        catch (Exception ex)
                                        {
                                            await logService.LogSave(Common.Enums.LogEventEnum.DbError,
                                                this.GetType().Name,
                                                nameof(Customer_Endorsement_Data_Processing),
                                                CompanyEnum.KD,
                                                LogTypeEnum.Response,
                                                ex
                                                );
                                        }
                                    }
                                }
                                else
                                {
                                    var endorsementIntegrationStateRes = await endorsementService.SetEndorsementIntegrationStateWithSqlAsync(
                                        1,
                                        $"{endorsement.uzm_erpid} Erp Id bilgisi ile eşleşip kullanımda olan aktif sadakat kartı kaydı bulunamadığı için ignore edilmiştir.",
                                        endorsementId,
                                        discountFixingFlag);
                                }
                            }
                            else
                            {
                                var endorsementIntegrationStateRes = await endorsementService.SetEndorsementIntegrationStateWithSqlAsync(
                                    1,
                                    $"{endorsement.uzm_erpid} Erp Id bilgisi ile eşleşen aktif müşteri kaydı bulunamadığı için ignore edilmiştir.",
                                    endorsementId,
                                    discountFixingFlag);
                            }
                        }
                        else
                        {
                            var endorsementIntegrationStateRes = await endorsementService.SetEndorsementIntegrationStateWithSqlAsync(
                                1,
                                $"{endorsement.uzm_billtype}: Ciro hesaplamada kullanılan bir fatura tipi olmadığı için ignore edilmiştir.",
                                endorsementId,
                                discountFixingFlag);
                        }
                    }
                };
                // Task.WaitAll(endorsementTasks.ToArray());
            }
            else
            {
                result.Message = CommonStaticConsts.Message.GetUnintegratedEndorsementListError;
            }

            return result;
        }

        public async Task<Response<object>> Card_Exception_Discount_Send_Email_By_ArrivalChannel()
        {
            var result = new Response<object>();

            string filePath = ConfigurationManager.AppSettings["CardExceptionReportFilePath"];
            string fileName = DateTime.Now.ToString("yyyy-MM-dd") + " Tarihli-Kart İstisna İndirimleri";
            filePath += fileName + ".xlsx";
            var lastMonthCards = await this.cardExceptionDiscountService.GetCardExceptionDiscountsCreatedFromStore();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Monthly Card Exception Report");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Kart İstisna İd";
                worksheet.Cell(currentRow, 2).Value = "Kart Sınıfı";
                worksheet.Cell(currentRow, 3).Value = "İndirim Oranı";
                worksheet.Cell(currentRow, 4).Value = "Talep Eden";
                worksheet.Cell(currentRow, 5).Value = "Talep Tarihi";
                worksheet.Cell(currentRow, 6).Value = "İndirim Onay Statüsü";
                worksheet.Cell(currentRow, 7).Value = "Statü";
                worksheet.Cell(currentRow, 8).Value = "Geliş Kanalı";
                worksheet.Cell(currentRow, 9).Value = "Müşteri Grubu";
                worksheet.Cell(currentRow, 10).Value = "İstisna Tanım Başlangıç Tarihi";
                worksheet.Cell(currentRow, 11).Value = "İstisna Tanım Bitiş Tarihi";
                worksheet.Cell(currentRow, 12).Value = "Ek Açıklama";
                worksheet.Cell(currentRow, 13).Value = "Onaylayan";
                worksheet.Cell(currentRow, 14).Value = "Onay Açıklaması";
                foreach (var user in lastMonthCards.Data)
                {
                    currentRow++;
                    string approvalStatus = "";
                    string statusCode = "";
                    switch (user.uzm_approvalstatus)
                    {
                        case 1: approvalStatus = "Taslak"; break;
                        case 2: approvalStatus = "Reddedildi"; break;
                        case 3: approvalStatus = "Onaylandı"; break;
                        case 4: approvalStatus = "Onay Beklemede"; break;
                        default: break;
                    }
                    switch (user.uzm_statuscode)
                    {
                        case 0: statusCode = "Taslak"; break;
                        case 1: statusCode = "Kullanımda"; break;
                        case 2: statusCode = "Bloke Edildi"; break;
                        case 3: statusCode = "Süresi Doldu"; break;
                        case 4: statusCode = "İptal Edildi"; break;
                        default: break;
                    }
                    //TO BE REVISED!!!(SUSPENDED)
                    //worksheet.Cell(currentRow, 1).Value = user.uzm_carddiscountId;
                    //worksheet.Cell(currentRow, 2).Value = user.SegmentName;  //uzm_cardclasssegmentId
                    //worksheet.Cell(currentRow, 3).Value = user.uzm_discountrate;
                    //worksheet.Cell(currentRow, 4).Value = user.uzm_firstname + " " + user.uzm_lastname; //uzm_demandeduser
                    //worksheet.Cell(currentRow, 5).Value = user.uzm_demanddate;
                    //worksheet.Cell(currentRow, 6).Value = approvalStatus;    //uzm_approvalstatus
                    //worksheet.Cell(currentRow, 7).Value = statusCode;    //statuscode
                    //worksheet.Cell(currentRow, 8).Value = "Portal";  //uzm_arrivalstatus
                    //worksheet.Cell(currentRow, 9).Value = user.CustomerGroupName;   //uzm_name    //uzm_customergroupId
                    //worksheet.Cell(currentRow, 10).Value = user.uzm_startdate;
                    //worksheet.Cell(currentRow, 11).Value = user.uzm_enddate;
                    //worksheet.Cell(currentRow, 12).Value = user.uzm_description;
                    //worksheet.Cell(currentRow, 13).Value = user.uzm_fullname; //uzm_approvedby
                    //worksheet.Cell(currentRow, 14).Value = user.uzm_approvalexplanation;
                }
                worksheet.Columns().AdjustToContents();
                int rowLength = lastMonthCards.Data.Count;
                worksheet.Columns().AdjustToContents();
                var columnCount = worksheet.Columns().Count();
                var range = worksheet.Range(1, 1, rowLength + 1, columnCount);
                var table = range.CreateTable();
                table.Theme = XLTableTheme.TableStyleMedium9;

                workbook.SaveAs(filePath);
            }

            //string subject = "Kart İstisna İndirim Talebi Aylık Raporu(TEST) - [Vakko VIP Portal Bilgilendirme]";
            //string subject = System.Configuration.ConfigurationManager.AppSettings["CardExceptionReportFilePath"];

            bool isTest = false;
            Boolean.TryParse(ConfigurationManager.AppSettings["IsTest"], out isTest);
            string subject = isTest ? ConfigurationManager.AppSettings["CardExceptionReportSubjectTest"] : ConfigurationManager.AppSettings["CardExceptionReportSubject"];

            string msg = $@"Merhaba,
                            <br/><br/>
                            Test maildir lütfen dikkate almayınız!
                            <br/><br/>
                            Aylık kart istisna indirim talep raporu ektedir.
                            <br/><br/>
                            İyi Çalışmalar,
                            <br/><br/>
                            Saygılarımızla
                            <br/><br/><br/>
                            Vakko Talep Sistemi";
            FileStream stream = File.OpenRead(filePath);
            byte[] byteData = new byte[stream.Length];
            stream.Read(byteData, 0, byteData.Length);
            stream.Close();

            var mailList = new List<ToMailSend>();
            mailList = null;
            List<string> mailTo = new List<string>();
            List<string> mailCc = new List<string>();

            foreach (var item in mailList)
            {
                if (item.uzm_mailtype == "1")
                {
                    mailTo.Add(item.uzm_email);
                }
                else if (item.uzm_mailtype == "2")
                {
                    mailCc.Add(item.uzm_email);
                }
            }

            //var strToList = ConfigurationManager.AppSettings["CardExceptionReportMailTo"].ToString();
            //string[] strAllToList = strToList.Split(',');
            //List<string> toList = strAllToList.ToList();

            //Guid portalUserId = Guid.Parse("4B7198C3-D1FD-419D-B44F-BA83FFEB895F");

            var resultMail = this.emailService.SendEmail(subject, msg, "uzm_portaluser", null, mailTo, mailCc, byteData, "application/ms-excel", fileName + ".xlsx");

            return result;
        }

        public async Task<Response<object>> Will_Be_Expired_Soon_Card_Exception_Discount_Send_Email()
        {
            var result = new Response<object>();

            var firstCardExceptionDiscountList = new List<GetCardExceptionDiscountsWillBeExpiredSoon_ResponseDto>();
            var lastCardExceptionDiscountList = new List<GetCardExceptionDiscountsWillBeExpiredSoon_ResponseDto>();

            var cardClassSegmentListRes = await cardClassSegmentService.GetCardClassSegmentListAsync(null);
            if (cardClassSegmentListRes.Success)
            {
                foreach (var cardClassSegment in cardClassSegmentListRes.Data)
                {
                    #region Bitimine, Kart Sınıfında tanımlı olan Birinci Bildirim Süresi(gün) kadar zaman kalan Kart İstisna İndirim kayıtlarını listeler
                    if (cardClassSegment.uzm_notificationperiod.IsNotNullAndEmpty())
                    {
                        var cedResult = await cardExceptionDiscountService.GetCardExceptionDiscountsWillBeExpiredSoon((Guid)cardClassSegment.uzm_cardclasssegmentid, (int)cardClassSegment.uzm_notificationperiod);
                        if (cedResult.Success)
                        {
                            firstCardExceptionDiscountList.AddRange(cedResult.Data);
                        }
                    }
                    #endregion

                    #region Bitimine, Kart Sınıfında tanımlı olan İkinci Bildirim Süresi(gün) kadar zaman kalan Kart İstisna İndirim kayıtlarını listeler
                    if (cardClassSegment.uzm_secondnotificationperiod.IsNotNullAndEmpty())
                    {
                        var lastCedResult = await cardExceptionDiscountService.GetCardExceptionDiscountsWillBeExpiredSoon((Guid)cardClassSegment.uzm_cardclasssegmentid, (int)cardClassSegment.uzm_secondnotificationperiod);
                        if (lastCedResult.Success)
                        {
                            lastCardExceptionDiscountList.AddRange(lastCedResult.Data);
                        }
                    }
                    #endregion
                }
            }

            if (firstCardExceptionDiscountList.Count > 0 || lastCardExceptionDiscountList.Count > 0)
            {
                string filePath = HostingEnvironment.MapPath("~/Storage/CardExceptionReports/");
                if (!Directory.Exists(filePath))
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                string fileName = DateTime.Now.ToString("yyyy-MM-dd") + " Tarihli - Süresi Dolacak Kart İstisna İndirimleri";
                filePath += fileName + ".xlsx";

                using (var workbook = new XLWorkbook())
                {
                    #region Birinci Bildirim Süresi
                    if (firstCardExceptionDiscountList.Count > 0)
                    {
                        var worksheet = workbook.Worksheets.Add("Birinci Bildirim Süresi");
                        var currentRow = 1;

                        worksheet.Cell(currentRow, 1).Value = "Müşteri";
                        worksheet.Cell(currentRow, 2).Value = "Erp Id";
                        worksheet.Cell(currentRow, 3).Value = "Kart Numarası";
                        worksheet.Cell(currentRow, 4).Value = "İstisna İndirim Oranı";
                        worksheet.Cell(currentRow, 5).Value = "İstisna Başlangıç Tarihi";
                        worksheet.Cell(currentRow, 6).Value = "İstisna Bitiş Tarihi";
                        worksheet.Cell(currentRow, 7).Value = "Müşteri Grubu";
                        worksheet.Cell(currentRow, 8).Value = "Kart Sınıfı";
                        worksheet.Cell(currentRow, 9).Value = "Talep Eden Personel";
                        worksheet.Cell(currentRow, 10).Value = "Talep Eden Lokasyon";
                        worksheet.Cell(currentRow, 11).Value = "Talep Kanalı";
                        worksheet.Cell(currentRow, 12).Value = "Talep Tarihi";
                        worksheet.Cell(currentRow, 13).Value = "Ek Açıklama";
                        worksheet.Cell(currentRow, 14).Value = "Onaylayan";
                        worksheet.Cell(currentRow, 15).Value = "Onay Açıklaması";
                        foreach (var cardExceptionDiscount in firstCardExceptionDiscountList)
                        {
                            currentRow++;

                            worksheet.Cell(currentRow, 1).Value = cardExceptionDiscount.uzm_contactidname;
                            worksheet.Cell(currentRow, 2).Value = cardExceptionDiscount.uzm_erpid;
                            worksheet.Cell(currentRow, 3).Value = cardExceptionDiscount.uzm_loyaltycardidname;
                            worksheet.Cell(currentRow, 4).Value = cardExceptionDiscount.uzm_discountrate;
                            worksheet.Cell(currentRow, 5).Value = cardExceptionDiscount.uzm_startdate;
                            worksheet.Cell(currentRow, 6).Value = cardExceptionDiscount.uzm_enddate;
                            worksheet.Cell(currentRow, 7).Value = cardExceptionDiscount.uzm_customergroupidname;
                            worksheet.Cell(currentRow, 8).Value = cardExceptionDiscount.uzm_cardclasssegmentidname;
                            worksheet.Cell(currentRow, 9).Value = cardExceptionDiscount.uzm_demandedusername;
                            worksheet.Cell(currentRow, 10).Value = cardExceptionDiscount.uzm_demandstorename;
                            worksheet.Cell(currentRow, 11).Value = cardExceptionDiscount.uzm_arrivalchannelname;
                            worksheet.Cell(currentRow, 12).Value = cardExceptionDiscount.uzm_demanddate;
                            worksheet.Cell(currentRow, 13).Value = cardExceptionDiscount.uzm_description;
                            worksheet.Cell(currentRow, 14).Value = cardExceptionDiscount.uzm_approvedbyname;
                            worksheet.Cell(currentRow, 15).Value = cardExceptionDiscount.uzm_approvalexplanation;
                        }
                        worksheet.Columns().AdjustToContents();
                        int rowLength = firstCardExceptionDiscountList.Count;
                        worksheet.Columns().AdjustToContents();
                        var columnCount = worksheet.Columns().Count();
                        var range = worksheet.Range(1, 1, rowLength + 1, columnCount);
                        var table = range.CreateTable();
                        table.Theme = XLTableTheme.TableStyleMedium9;
                    }
                    #endregion

                    #region İkinci Bildirim Süresi
                    if (lastCardExceptionDiscountList.Count > 0)
                    {
                        var worksheet2 = workbook.Worksheets.Add("İkinci Bildirim Süresi");
                        var currentRow2 = 1;
                        worksheet2.Cell(currentRow2, 1).Value = "Müşteri";
                        worksheet2.Cell(currentRow2, 2).Value = "Erp Id";
                        worksheet2.Cell(currentRow2, 3).Value = "Kart Numarası";
                        worksheet2.Cell(currentRow2, 4).Value = "İstisna İndirim Oranı";
                        worksheet2.Cell(currentRow2, 5).Value = "İstisna Başlangıç Tarihi";
                        worksheet2.Cell(currentRow2, 6).Value = "İstisna Bitiş Tarihi";
                        worksheet2.Cell(currentRow2, 7).Value = "Müşteri Grubu";
                        worksheet2.Cell(currentRow2, 8).Value = "Kart Sınıfı";
                        worksheet2.Cell(currentRow2, 9).Value = "Talep Eden Personel";
                        worksheet2.Cell(currentRow2, 10).Value = "Talep Eden Lokasyon";
                        worksheet2.Cell(currentRow2, 11).Value = "Talep Kanalı";
                        worksheet2.Cell(currentRow2, 12).Value = "Talep Tarihi";
                        worksheet2.Cell(currentRow2, 13).Value = "Ek Açıklama";
                        worksheet2.Cell(currentRow2, 14).Value = "Onaylayan";
                        worksheet2.Cell(currentRow2, 15).Value = "Onay Açıklaması";
                        foreach (var cardExceptionDiscount in lastCardExceptionDiscountList)
                        {
                            currentRow2++;

                            worksheet2.Cell(currentRow2, 1).Value = cardExceptionDiscount.uzm_contactidname;
                            worksheet2.Cell(currentRow2, 2).Value = cardExceptionDiscount.uzm_erpid;
                            worksheet2.Cell(currentRow2, 3).Value = cardExceptionDiscount.uzm_loyaltycardidname;
                            worksheet2.Cell(currentRow2, 4).Value = cardExceptionDiscount.uzm_discountrate;
                            worksheet2.Cell(currentRow2, 5).Value = cardExceptionDiscount.uzm_startdate;
                            worksheet2.Cell(currentRow2, 6).Value = cardExceptionDiscount.uzm_enddate;
                            worksheet2.Cell(currentRow2, 7).Value = cardExceptionDiscount.uzm_customergroupidname;
                            worksheet2.Cell(currentRow2, 8).Value = cardExceptionDiscount.uzm_cardclasssegmentidname;
                            worksheet2.Cell(currentRow2, 9).Value = cardExceptionDiscount.uzm_demandedusername;
                            worksheet2.Cell(currentRow2, 10).Value = cardExceptionDiscount.uzm_demandstorename;
                            worksheet2.Cell(currentRow2, 11).Value = cardExceptionDiscount.uzm_arrivalchannelname;
                            worksheet2.Cell(currentRow2, 12).Value = cardExceptionDiscount.uzm_demanddate;
                            worksheet2.Cell(currentRow2, 13).Value = cardExceptionDiscount.uzm_description;
                            worksheet2.Cell(currentRow2, 14).Value = cardExceptionDiscount.uzm_approvedbyname;
                            worksheet2.Cell(currentRow2, 15).Value = cardExceptionDiscount.uzm_approvalexplanation;
                        }
                        worksheet2.Columns().AdjustToContents();
                        int rowLength2 = lastCardExceptionDiscountList.Count;
                        worksheet2.Columns().AdjustToContents();
                        var columnCount2 = worksheet2.Columns().Count();
                        var range2 = worksheet2.Range(1, 1, rowLength2 + 1, columnCount2);
                        var table2 = range2.CreateTable();
                        table2.Theme = XLTableTheme.TableStyleMedium9;
                    }
                    #endregion

                    workbook.SaveAs(filePath);
                }

                string subject = "Expired Card Discount Exception";

                string msg = $@"Merhaba,
                                <br/><br/>
                                Bitiş tarihi yaklaşan sadakat kart istisna kayıtları raporu ektedir.
                                <br/><br/>
                                İyi Çalışmalar,
                                <br/><br/>
                                Saygılarımızla
                                <br/>";
                FileStream stream = File.OpenRead(filePath);
                byte[] byteData = new byte[stream.Length];
                stream.Read(byteData, 0, byteData.Length);
                stream.Close();

                var mailList = new List<ToMailSend>();
                mailList = null;
                List<string> mailTo = new List<string>();
                List<string> mailCc = new List<string>();

                foreach (var item in mailList)
                {
                    if (item.uzm_mailtype == "1")
                    {
                        mailTo.Add(item.uzm_email);
                    }
                    else if (item.uzm_mailtype == "2")
                    {
                        mailCc.Add(item.uzm_email);
                    }
                }

                var resultMail = emailService.SendEmail(subject, msg, "uzm_portaluser", null, mailTo, mailCc, byteData, "application/ms-excel", fileName + ".xlsx");
            }

            return result;
        }

        public async Task<Response<object>> Set_Status_Expired_Today_Card_Exception_Discount()
        {
            var result = new Response<object>();

            var cedResult = await cardExceptionDiscountService.GetExpiredTodayCardExceptionDiscounts();
            if (cedResult.Success)
            {
                var req = new CardApprovalStatusAndExplanationRequestDto
                {
                    StatusCode = CardDiscountStatusCodeType.Expired
                };

                foreach (var cardExceptionDiscount in cedResult.Data)
                {
                    req.CardDiscountId = cardExceptionDiscount.Id;
                    var resd = await cardExceptionDiscountService.UpdateCardExceptionDiscountApprovalStatusAndExplanationAsync(req);
                }
            }

            return result;
        }

        public async Task<Response<object>> Batch_Approval_List_Data_Processing()
        {
            var result = new Response<object>();

            // Tamamlanmamış olan toplu onay listesi kayıtları çekilir
            var balResult = await batchApprovalListService.GetWillBeProcessedBatchApprovalList();
            if (balResult.Success)
            {
                foreach (var bal in balResult.Data)
                {
                    // Toplu onay listesi ile ilişkili olup henüz onaylanmamış veya reddedilmemiş olan istisna kayıtları çekilir
                    var cedResult = await cardExceptionDiscountService.GetCardExceptionDiscountsByBatchApprovalListId(bal.vkk_batchapprovallistid);
                    if (cedResult.Success)
                    {
                        foreach (var ced in cedResult.Data)
                        {
                            var tryCount = 1;
                            tryagain:
                            try
                            {
                                var loyaltyCardId = Guid.Empty;
                                // Müşteri erpid'si ile eşleşen, kullanımda olan loyalty card'ı yoksa default olarak V tipinde yeni kart oluştur
                                var loyaltyCardInfoRes = await loyaltyCardService.GetLoyaltyCardByErpIdAsync(ced.vkk_erpid);
                                if (!loyaltyCardInfoRes.Success)
                                {
                                    var loyaltyCardResponse = await loyaltyCardService.LoyaltyCardSaveAsync(new LoyaltyCardRequestDto
                                    {
                                        CardType = CardTypeEnum.V,
                                        ErpId = ced.vkk_erpid,
                                        StoreCode = "1132"
                                    }, new Response<CardExceptionDiscountDto> { Data = ced, Success = true, Message = cedResult.Message });
                                    if (loyaltyCardResponse.Success)
                                        loyaltyCardId = loyaltyCardResponse.Data.Id;
                                }
                                else
                                {
                                    loyaltyCardId = (Guid)loyaltyCardInfoRes.Data.uzm_loyaltycardid;
                                }

                                if (loyaltyCardId != Guid.Empty)
                                {
                                    var req = new CardExceptionDiscountRequestDto
                                    {
                                        ApprovedByUserId = bal.vkk_approvedby,
                                        ApprovalExplanation = bal.vkk_approvalexplanation,
                                        ApprovalStatus = (ApprovalStatusType)bal.vkk_approvalstatus,
                                        ArrivalChannel = bal.vkk_arrivalchannel,
                                        CardDiscountId = ced.uzm_carddiscountId,
                                        DemandDate = bal.vkk_demanddate,
                                        DemandedUserId = bal.vkk_demandeduser,
                                        DemandStore = bal.vkk_demandstore,
                                        LoyaltyCardId = loyaltyCardId,
                                        StatusCode = (CardDiscountStatusCodeType)LoyaltyCardStatusCodeType.InUse,
                                    };

                                    var cardExceptionDiscountRes = await cardExceptionDiscountService.CardExceptionDiscountSaveAsync(req);
                                }

                            }
                            catch (Exception ex)
                            {
                                tryCount++;
                                if (tryCount < 4) // İstisna bazlı 3 kere tekrar deneme
                                    goto tryagain;
                            }
                        }
                    }

                    var UpdateBatchApprovalListProcessStatusResult = await batchApprovalListService.UpdateBatchApprovalListProcessStatusAsync(new BatchApprovalListProcessStatusRequestDto
                    {
                        BatchApprovalListId = bal.vkk_batchapprovallistid,
                        ProcessStatus = 1 // Tamamlandı
                    });
                }
            }

            return result;
        }

    }
}

// 1- [uzm_integrationstatus] = 0 olan datalar çekilecek

// 2- Kart tipine göre ilgili Barem listesi çekilecek

// 3- Müşterini KartNo bilgisine göre müşteri KartTipi çekilecek

// 4- Erp Id kullanılarak kart üzerindeki bilgiler ve (Dönem Ciro) bilgisi getirilecek.

// 5- SAP den gelen işlenecek datalar üzerindeki [Toplam Tutar - Gift Tutar] + [Dönem Ciro (Loyalty Kart Üzerindeki Alan)]  formülü ile  Dönem Ciro bilgisi hesaplanacaktır.

// 6- Loyalty Kart üzerindeki Devir Ciro ile hesaplanan Dönem Ciro karşılaştırılıp büyük olan Ciro geçerli olacak ve Barem aralık bilgisi buna göre hesaplanacaktır.

// 7- Değişken olarak Geçerli Ciro bilgisi ile Kart tipine göre eşleşen barem satırı bilgisi getirilecektir.

// 8 Kart İstisna tablosu Kart Id bilgisi ile sorgulanacak, Kayıt aktif mi?,Kayıt güncel tarih başlangıç ve bitiş tarihi aralığı içinde mi?,İndirim Onay durumu :Onaylı kayıtlar mı?

// 9- Kart İstisna tablosu için indirim bulundunduktan sonra , Bulunan Barem indirim oranı bilgisi ile karşılaştırılıp büyük olan indirim oranı bulunup Loyalty kart varlığındaki geçerli indirim bilgisi güncellenecektir 

// 10- Loyalty Kart (uzm_loyaltycard) üzerindeki Üst Segment alanları ekstra hesaplanıp güncellenecek , Güncellenecek değerler Mevcut baremdeki bir üst barem bilgisi olmalı.
// Bulunan baremin Barem Üst Sınırı değerine 1 ekleyerek bir üst barem bulunur.
// Direk Barem Üst Sınırı değeri kullanılarak da bulunabilirdi, bir sonraki baremin alt sınırı olduğu için ama kafa karışıklılığı olmaması için bu şekilde tercih edildi.

// 11- Loyalty Kart üzerindeki Dönem Ciro ve Geçerli indirim uzm_loyaltycard varlığında güncellenecektir.

// 12- Bulunan BaremId bilgisi uzm_customerwagescale tablosuna ContactId ile historik olarak eklenecektir.(GÜncel dönem için Aktiflik Durumu Aktif tir.)
