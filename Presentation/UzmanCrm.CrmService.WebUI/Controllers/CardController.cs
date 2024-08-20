using Microsoft.Extensions.Caching.Memory;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using UzmanCrm.CrmService.Application.Abstractions.Service.BackgroundService;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService;
using UzmanCrm.CrmService.Application.Abstractions.Service.CardExceptionDiscountService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.EmailService;
using UzmanCrm.CrmService.Application.Abstractions.Service.EmailService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LoyaltyCardService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.PortalService;
using UzmanCrm.CrmService.Application.Abstractions.Service.PortalService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.Common.Helpers;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Rest;
using UzmanCrm.CrmService.WebUI.Models;

namespace UzmanCrm.CrmService.WebUI.Controllers
{
    public class CardController : Controller
    {
        #region CTOR
        private readonly IRestService _restService;
        private readonly IPortalService _portalService;
        private readonly ILogService _logService;
        private readonly IContactService _contactService;
        private readonly IEmailService _emailService;
        private readonly ILoyaltyCardService _loyaltyCardService;
        private readonly ICardExceptionDiscountService _cardExceptionDiscountService;
        private readonly IMemoryCache _memoryCache;
        private readonly IBackgroundService _backgroundService;
        public CardController(IRestService restService,
            IPortalService portalService,
            IContactService contactService,
            IEmailService emailService,
            ILogService logService,
            ILoyaltyCardService loyaltyCardService,
            ICardExceptionDiscountService cardExceptionDiscountService,
            IMemoryCache memoryCache,
            IBackgroundService backgroundService)
        {
            _restService = restService;
            _portalService = portalService;
            _contactService = contactService;
            _emailService = emailService;
            _logService = logService;
            _loyaltyCardService = loyaltyCardService;
            _cardExceptionDiscountService = cardExceptionDiscountService;
            _memoryCache = memoryCache;
            _backgroundService = backgroundService;
        }

        #endregion

        Response<LoginResponse> token = null;
        string apiUser = ConfigurationManager.AppSettings["apiUser"];
        string apiPass = ConfigurationManager.AppSettings["apiPass"];
        public async Task<Response<LoginResponse>> Authenticate(LoginRequest req)
        {
            try
            {
                const string key = "auth";
                if (_memoryCache.TryGetValue(key, out token))
                    return token;
                Response<Response<LoginResponse>> res = await _restService.SendRequest<Response<LoginResponse>>(req, ConfigurationManager.AppSettings["Authenticate"]);
                if (res.Success)
                {
                    token = res.Data;
                    var cache = _memoryCache.Set(res.Data, token, DateTimeOffset.Now.AddSeconds(300));
                }
                else
                {
                    token.Success = res.Success;
                    token.Message = res.Message;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return token;
        }

        // GET: Card
        public async Task<ActionResult> CardExceptionDiscount(PortalUserRequestViewModel req)
        {
            Response<CardExceptionDiscountDto> cardException = new Response<CardExceptionDiscountDto>();
            cardException = await _cardExceptionDiscountService.CheckIfCardExceptionDiscountExistByErpId(req.PortalErpId);
            if (cardException.Data?.uzm_carddiscountId != null && cardException.Data?.uzm_carddiscountId != Guid.Empty)
            {
                cardException = await _cardExceptionDiscountService.GetCardExceptionDiscountStatusAndApprovalStatusById(cardException.Data.uzm_carddiscountId);
                if (cardException.Data.uzm_statuscode == 1 & (cardException.Data.uzm_approvalstatus == 3 || cardException.Data.uzm_approvalstatus == 4))
                {
                    req.IsExist = true;
                    return View(req);
                }
            }
            //req.PortalUserId = "4B7198C3-D1FD-419D-B44F-BA83FFEB895F";
            //req.PortalErpId = "15874582";
            return View(req);
        }

        /// <summary>
        ///   Get Loyalty Card
        /// </summary>
        /// <param name="req"></param>
        /// <remarks>
        ///     Portal e giriş yapan kullanıcının portaluserid si ile talep formundaki gerekli alanlardaki bilgileri çeker.
        ///     Servisteki GetLoyaltyCard metodunu kullanır.
        /// </remarks>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<JsonResult> GetLoyaltyCard(CardExceptionDiscounSearchViewModel req)
        {
            Response<LoyaltyCardGetResponse> resp = new Response<LoyaltyCardGetResponse>();
            try
            {
                if (token == null)
                    token = await Authenticate(new LoginRequest() { Username = apiUser, Password = apiPass });
                var organization = req.OrganizationId.ToString();
                //VAKKO: 3c410f1c-b1ec-e811-812b-005056991930
                //VAKKOCRM: 
                //VAKKOMAĞAZALAR: 
                //VAKKORAMA: 93b315e6-e5f3-e811-812b-005056991930
                //WCOLLECTION: 83c96523-e6f3-e811-812b-005056991930
                switch (organization)
                {
                    case "3c410f1c-b1ec-e811-812b-005056991930": req.ChannelType = 1; break;
                    case "0306b337-47cd-e811-8120-005056991930": req.ChannelType = 1; break;
                    case "93b315e6-e5f3-e811-812b-005056991930": req.ChannelType = 2; break;
                    case "83c96523-e6f3-e811-812b-005056991930": req.ChannelType = 3; break;
                    default: break;
                }

                Response<Response<LoyaltyCardGetResponse>> res = await _restService.SendRequest<Response<LoyaltyCardGetResponse>>(req, ConfigurationManager.AppSettings["GetLoyaltyCardAPI"], token.Data.Token);

                if (!res.Success)
                {
                    if (req.ChannelType == 1 || req.ChannelType == 2)
                        req.ChannelType = 3;
                    else
                        req.ChannelType = 1;

                    res = await _restService.SendRequest<Response<LoyaltyCardGetResponse>>(req, ConfigurationManager.AppSettings["GetLoyaltyCardAPI"], token.Data.Token);
                }

                res.Data.Data.TurnoverEndorsement = Convert.ToDouble(res.Data.Data.TurnoverEndorsement);
                res.Data.Data.PeriodEndorsement = Convert.ToDouble(res.Data.Data.PeriodEndorsement);

                if (res.Success)
                {
                    resp = res.Data;
                }
                else
                {
                    resp.Success = res.Success;
                    resp.Message = res.Message;
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }

            return Json(resp.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetCustomerEndorsements(CustomerEndorsementRequestViewModel req)
        {
            Response<EndorsementGetResponse> resp = new Response<EndorsementGetResponse>();
            try
            {
                if (token == null)
                    token = await Authenticate(new LoginRequest() { Username = apiUser, Password = apiPass });

                Response<Response<EndorsementGetResponse>> res = await _restService.SendRequest<Response<EndorsementGetResponse>>(null, ConfigurationManager.AppSettings["GetCustomerEndorsementAPI"] + $"?ErpId={req.ErpId}&CardType={req.CardType}", token.Data.Token, Method.Get);

                if (res.Success)
                {
                    resp = res.Data;
                }
                else
                {
                    resp.Success = res.Success;
                    resp.Message = res.Message;
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }

            return Json(resp.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetCardDiscount(CardExceptionApprovalRequestViewModel req)
        {
            Response<CardExceptionDiscountAndContactDto> resp = new Response<CardExceptionDiscountAndContactDto>();
            try
            {
                var loyaltyCardRes = await _loyaltyCardService.GetLoyaltyCardByIdAsync(Guid.Parse(req.LoyaltyCardId));
                if (loyaltyCardRes.Success)
                {
                    Response<CardExceptionDiscountAndContactDto> res = await _cardExceptionDiscountService.GetCardDiscount(loyaltyCardRes.Data.uzm_carddiscountid);
                    res.Data.uzm_turnoverendorsement = Convert.ToDouble(String.Format("{0:0.00}", loyaltyCardRes?.Data?.uzm_turnoverendorsement != null ? loyaltyCardRes?.Data?.uzm_turnoverendorsement : 0.0));
                    res.Data.uzm_periodendorsement = Convert.ToDouble(String.Format("{0:0.00}", loyaltyCardRes?.Data?.uzm_periodendorsement != null ? loyaltyCardRes?.Data?.uzm_periodendorsement : 0.0));
                    string organization = res.Data.uzm_organizationid.ToString();
                    switch (organization)
                    {
                        case "3c410f1c-b1ec-e811-812b-005056991930": res.Data.uzm_validdiscountrate = loyaltyCardRes.Data.uzm_validdiscountratevakko; break;
                        case "0306b337-47cd-e811-8120-005056991930": res.Data.uzm_validdiscountrate = loyaltyCardRes.Data.uzm_validdiscountratevakko; break;
                        case "93b315e6-e5f3-e811-812b-005056991930": res.Data.uzm_validdiscountrate = loyaltyCardRes.Data.uzm_validdiscountratevr; break;
                        case "83c96523-e6f3-e811-812b-005056991930": res.Data.uzm_validdiscountrate = loyaltyCardRes.Data.uzm_validdiscountratewcol; break;
                        default: break;
                    }

                    if (res.Success)
                    {
                        resp.Data = res.Data;
                    }
                    else
                    {
                        resp.Success = res.Success;
                        resp.Message = res.Message;
                    }
                }
                else
                {
                    resp.Success = loyaltyCardRes.Success;
                    resp.Message = loyaltyCardRes.Message;
                    resp.Error = loyaltyCardRes.Error;
                }

                return Json(resp, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<JsonResult> GetContactsForCard(CardContactRequestViewModel req)
        {
            CardExceptionContactRequestDto contactReq = new CardExceptionContactRequestDto();
            Response<List<ContactDto>> resp = new Response<List<ContactDto>>();
            try
            {
                contactReq.ErpId = req.ErpId;
                contactReq.CardNo = req.CardNo;
                contactReq.MobilePhone = req.MobilePhone;
                contactReq.EmailAddress1 = req.EmailAddress1;
                Response<List<ContactDto>> res = await _contactService.GetContactListForCardExceptionDiscount(contactReq);

                if (res.Success)
                {
                    resp = res;
                }
                else
                {
                    resp.Success = res.Success;
                    resp.Message = res.Message;
                }

                return Json(resp, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<JsonResult> GetPortalUser(PortalUserRequestViewModel req)
        {
            PortalUserRequestDto portalUserReq = new PortalUserRequestDto();
            Response<PortalUserResponseDto> resp = new Response<PortalUserResponseDto>();
            try
            {
                portalUserReq.uzm_portaluserid = Guid.Parse(req.PortalUserId);
                Response<PortalUserResponseDto> res = await _portalService.GetPortalUserAndApprovedBy(portalUserReq);

                if (res.Success)
                {
                    resp = res;
                }
                else
                {
                    resp.Success = res.Success;
                    resp.Message = res.Message;
                }

                return Json(resp, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        ///   Get Customer Group List
        /// </summary>
        /// <remarks>
        ///   Müşteri Grubu(CRM) - Üst Yönetim Komitesine Gönderilecek Talep Sebebi
        /// </remarks>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<JsonResult> GetCustomerGroupList()
        {
            Response<List<CustomerGroupGetResponse>> resp = new Response<List<CustomerGroupGetResponse>>();
            try
            {
                if (token == null)
                    token = await Authenticate(new LoginRequest() { Username = apiUser, Password = apiPass });
                Response<Response<List<CardClassSegmentGetResponse>>> cardClassSegment = await _restService.SendRequest<Response<List<CardClassSegmentGetResponse>>>(null, ConfigurationManager.AppSettings["CardClassSegmentListAPI"], token.Data.Token, Method.Get);
                Response<Response<List<CustomerGroupGetResponse>>> res = await _restService.SendRequest<Response<List<CustomerGroupGetResponse>>>(null, ConfigurationManager.AppSettings["CustomerGroupListAPI"], token.Data.Token, Method.Get);

                if (res.Success)
                {
                    resp = res.Data;
                    resp.Message = res.Message;
                    resp.Data.Add(new CustomerGroupGetResponse { CardClassSegment = cardClassSegment.Data.Data });
                }
                else
                {
                    resp.Success = res.Success;
                    resp.Message = res.Message;
                }

                return Json(resp.Data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        /// <summary>
        ///   Save Card Exception Discount
        /// </summary>
        /// <param name="req"></param>
        /// <remarks>
        ///   Bu metot vakko kart istisna kaydı için kullanılmaktadır.
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveCardExDiscount(CardExceptionDiscountCreateViewModel req)
        {
            Response<CardExceptionDiscountSaveResponse> resp = new Response<CardExceptionDiscountSaveResponse>();
            try
            {
                if (token == null)
                    token = await Authenticate(new LoginRequest() { Username = apiUser, Password = apiPass });
                Response<Response<CardExceptionDiscountSaveResponse>> res = await _restService.SendRequest<Response<CardExceptionDiscountSaveResponse>>(req, ConfigurationManager.AppSettings["SaveCardExceptionDiscountAPI"], token.Data.Token);

                if (res.Success)
                {
                    resp = res.Data;
                    await _logService.LogSave(LogEventEnum.DbInfo, "Response", System.Reflection.MethodBase.GetCurrentMethod().Name, CompanyEnum.KD, LogTypeEnum.Response, resp);
                }
                else
                {
                    resp.Success = res.Success;
                    resp.Message = res.Message;
                    await _logService.LogSave(LogEventEnum.DbInfo, "Response", System.Reflection.MethodBase.GetCurrentMethod().Name, CompanyEnum.KD, LogTypeEnum.Response, resp);
                }
            }
            catch (Exception ex)
            {
                resp.Success = false;
                resp.Message = ex.Message;
                await _logService.LogSave(LogEventEnum.DbWarning, "Response", System.Reflection.MethodBase.GetCurrentMethod().Name, CompanyEnum.KD, LogTypeEnum.Response, resp);
            }
            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="req"></param>
        /// <param name="approvalExplanation"></param>
        /// <remarks>
        ///    Kart istisna talebini onaycıya göndermek ve talep sonucunu talep eden portal kullanıcısını bilgilendiren email atmak için kullanılır.
        ///    Kart istisna talebinin onaycıya gönderilmesi CRM workflow ile yapıldığından çift mail gönderilmemesi için iptal edilmiştir.
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SendEmailToApproval(SendEmailRequestViewModel req, string approvalExplanation = null)
        {

            Response<EmailResponse> res = new Response<EmailResponse>();
            try
            {
                string[] user = new string[] { };
                var subject = "Vakko Kart İndirim Talebi - [Vakko VIP Portal Bilgilendirme]";
                var message = "";
                string formattedEndDate = req.EndDate.ToString("dd/MM/yyyy hh:MM:ss");
                if (req.Message == "SendApproval")
                {
                    user = new string[] { req.ApprovingSuperVisorId };  //Send approvingsupervisor
                    //user = new string[] { req.PortalUserId };  //Send demandeduser
                    var loyaltyCardId = Guid.Parse(req.LoyaltyCardId);
                    string approvingPage = ConfigurationManager.AppSettings["SendEmailApproval"] + loyaltyCardId;
                    message = $@"Merhaba,
                                <br/><br/>
                                Yeni vakko kart indirim talebi bulunmaktadır.
                                <br/><br/>
                                Müşteri: {req.CustomerName}
                                <br/>
                                Mevcut Oran: {req.ValidDiscountRate}
                                <br/>
                                Talep Edilen Yeni Oran: {req.DiscountRate}
                                <br/>
                                İndirim Oranı Bitiş Tarihi: {formattedEndDate}
                                <br/>
                                Ek Açıklama: {req.Description}
                                <br/><br/>
                                Vakko VIP Portal Talep Onay sayfasına gitmek için <a href='{approvingPage}'>Tıklayınız.</a>
                                <br/><br/>
                                <br/><br/>
                                İyi Çalışmalar,
                                <br/><br/>
                                Vakko Talep Sistemi";
                }
                else if (req.Message == "SendRequester")
                {
                    user = new string[] { req.PortalUserId };
                    var acceptOrReject = "";
                    var discountRate = "";
                    if (req.RequestResult == "reddedildi.")
                        acceptOrReject = "Ret";
                    else
                    {
                        acceptOrReject = "Onay";
                        discountRate = "Onaylanan indirim tutarı:" + req.DiscountRate + "%";
                    }
                    message = $@"Merhaba,
                                <br/><br/>
                                Vakko kart indirim talebiniz aşağıdaki detaylar ile {req.RequestResult}.
                                <br/><br/>
                                Talep Eden Lokasyon: {req.DemandStore}
                                <br/>
                                Müşteri: {req.CustomerName}
                                <br/>
                                İndirim Oranı Bitiş Tarihi: {formattedEndDate}
                                <br/>
                                Talep Edilen Yeni Oran: {req.DiscountRate}
                                <br/>
                                {acceptOrReject} nedeni: {approvalExplanation}.
                                <br/><br/>
                                <br/><br/>
                                İyi Çalışmalar,
                                <br/><br/>
                                Vakko Talep Sistemi";
                }
                if (req.Message != "SendApproval")
                {
                    var resId = _emailService.SendEmail(subject, message, "uzm_portaluser", user);
                    if (resId.Data.Id == Guid.Empty)
                    {
                        res.Success = false;
                        res.Message = "Mail gönderiminde hata oldu lütfen ilgili birime iletin.";
                        _logService.LogSaveSync(LogEventEnum.DbInfo, "Response", System.Reflection.MethodBase.GetCurrentMethod().Name, CompanyEnum.KD, LogTypeEnum.Response, res);
                    }
                    else
                    {
                        res.Success = true;
                        res.Message = "Mail gönderimi başarılı.";
                    }
                }
                else
                {
                    res.Success = true;
                    res.Message = "Mail gönderimi başarılı.";
                }

            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Success = false;
                _logService.LogSaveSync(LogEventEnum.DbWarning, "Response", System.Reflection.MethodBase.GetCurrentMethod().Name, CompanyEnum.KD, LogTypeEnum.Response, res);
            }
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        #region CARD EXCEPTION DISCOUNT APPROVAL
        public async Task<ActionResult> CardExceptionDiscountApproval(CardExceptionApprovalRequestViewModel req)
        {
            try
            {
                var getLoyaltyCardResponse = await _loyaltyCardService.GetLoyaltyCardByCardNoAsync(req.LoyaltyCardId);
                if (getLoyaltyCardResponse.Success)
                {
                    req.LoyaltyCardId = getLoyaltyCardResponse.Data.uzm_loyaltycardid.ToString();
                    var cardException = await _cardExceptionDiscountService.GetCardDiscount(getLoyaltyCardResponse.Data.uzm_carddiscountid);
                    req.ApprovalStatus = cardException.Data.uzm_approvalstatus;
                    req.StatusCode = cardException.Data.uzm_statuscode;
                    req.DemandStore = cardException.Data.uzm_storeidname;
                }
            }
            catch (Exception ex)
            {
                await _logService.LogSave(Common.Enums.LogEventEnum.DbError,
                   this.GetType().Name,
                   nameof(CardExceptionDiscountApproval),
                   CompanyEnum.KD,
                   LogTypeEnum.Response,
                   ex
                   );
            }
            return View(req);
        }

        /// <summary>
        /// Update Card Exception Discount Approval Status And Explanation And BusinessUnitId
        /// </summary>
        /// <param name="req"></param>
        /// <remarks>
        ///     İstisna statülerini, onaycı açıklamasını ve talep eden mağazayı günceller.
        ///     Talep eden mağaza email workflow un çalışması için tekrar güncellenmektedir.
        /// </remarks>
        /// <returns></returns>
        public async Task<JsonResult> UpdateCardExceptionDiscountApprovalStatusAndExplanation(CardApprovalStatusAndExplanationRequestViewModel req)
        {
            CardApprovalStatusAndExplanationRequestDto cardApprovalStatus = new CardApprovalStatusAndExplanationRequestDto();
            Response<CardExceptionDiscountSaveResponseDto> resp = new Response<CardExceptionDiscountSaveResponseDto>();

            try
            {
                cardApprovalStatus.ApprovalStatus = req.ApprovalStatus;
                cardApprovalStatus.StatusCode = req.StatusCode;
                cardApprovalStatus.CardDiscountId = Guid.Parse(req.CardDiscountId);
                cardApprovalStatus.ApprovalExplanation = req.ApprovalExplanation;
                cardApprovalStatus.ArrivalChannel = req.ArrivalChannel;
                if (req.BusinessUnitId.IsNotNullAndEmpty())
                    cardApprovalStatus.BusinessUnitId = Guid.Parse(req.BusinessUnitId);

                Response<CardExceptionDiscountSaveResponseDto> res = await _cardExceptionDiscountService.UpdateCardExceptionDiscountApprovalStatusAndExplanationAsync(cardApprovalStatus);

                if (res.Success)
                {
                    resp = res;
                }
                else
                {
                    resp.Success = res.Success;
                    resp.Message = res.Message;
                    await _logService.LogSave(LogEventEnum.DbInfo, "Response", System.Reflection.MethodBase.GetCurrentMethod().Name, CompanyEnum.KD, LogTypeEnum.Response, resp);
                }
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                resp.Success = false;
                await _logService.LogSave(LogEventEnum.DbWarning, "Response", System.Reflection.MethodBase.GetCurrentMethod().Name, CompanyEnum.KD, LogTypeEnum.Response, ex);
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateCardExceptionEndDateAndDiscountRate(CardApprovalEndDateRequestViewModel req)
        {
            CardEndDateRequestDto cardEndDate = new CardEndDateRequestDto();
            Response<CardExceptionDiscountSaveResponseDto> resp = new Response<CardExceptionDiscountSaveResponseDto>();

            try
            {
                cardEndDate.EndDate = req.EndDate;
                cardEndDate.CardDiscountId = Guid.Parse(req.CardDiscountId);
                cardEndDate.DiscountRate = req.DiscountRate;

                Response<CardExceptionDiscountSaveResponseDto> res = await _cardExceptionDiscountService.UpdateCardExceptionDiscountEndDateAndDiscountRateAsync(cardEndDate);

                if (res.Success)
                {
                    resp = res;
                }
                else
                {
                    resp.Success = res.Success;
                    resp.Message = res.Message;
                    await _logService.LogSave(LogEventEnum.DbInfo, "Response", System.Reflection.MethodBase.GetCurrentMethod().Name, CompanyEnum.KD, LogTypeEnum.Response, resp);
                }
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                resp.Success = false;
                await _logService.LogSave(LogEventEnum.DbWarning, "Response", System.Reflection.MethodBase.GetCurrentMethod().Name, CompanyEnum.KD, LogTypeEnum.Response, ex);
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateLoyaltyCardValidDiscountRate(LoyaltyCardRequestViewModel req)
        {
            LoyaltyCardUpdateDto loyaltyCardUpdateDto = new LoyaltyCardUpdateDto();
            Response<LoyaltyCardUpdateResponseDto> resp = new Response<LoyaltyCardUpdateResponseDto>();

            try
            {
                var getLoyaltyCardResponse = await _loyaltyCardService.GetLoyaltyCardByIdAsync(Guid.Parse(req.LoyaltyCardId));
                if (getLoyaltyCardResponse.Success)
                {
                    //req.LoyaltyCardId = Guid.Parse("B151A51E-D748-ED11-915D-00505685232B");
                    loyaltyCardUpdateDto.Id = Guid.Parse(req.LoyaltyCardId);
                    if (getLoyaltyCardResponse.Data.uzm_validdiscountratevakko < req.ValidDiscountRate)
                        loyaltyCardUpdateDto.ValidDiscountRateVakko = req.ValidDiscountRate;

                    if (getLoyaltyCardResponse.Data.uzm_validdiscountratevr < req.ValidDiscountRate)
                        loyaltyCardUpdateDto.ValidDiscountRateVr = req.ValidDiscountRate;

                    if (getLoyaltyCardResponse.Data.uzm_validdiscountratewcol < req.ValidDiscountRate)
                        loyaltyCardUpdateDto.ValidDiscountRateWcol = req.ValidDiscountRate;

                    Response<LoyaltyCardUpdateResponseDto> res = await _loyaltyCardService.LoyaltyCardUpdateAsync(loyaltyCardUpdateDto);

                    if (res.Success)
                    {
                        resp.Success = res.Success;
                        resp.Data = res.Data;
                    }
                    else
                    {
                        resp.Success = res.Success;
                        resp.Message = res.Message;
                    }
                }
                else
                {
                    resp.Success = getLoyaltyCardResponse.Success;
                    resp.Message = getLoyaltyCardResponse.Message;
                }
            }
            catch (Exception ex)
            {
                resp.Message = ex.Message;
                resp.Success = false;
                await _logService.LogSave(LogEventEnum.DbWarning, "Response", System.Reflection.MethodBase.GetCurrentMethod().Name, CompanyEnum.KD, LogTypeEnum.Response, ex);
            }

            await _logService.LogSave(LogEventEnum.DbInfo, "Response", System.Reflection.MethodBase.GetCurrentMethod().Name, CompanyEnum.KD, LogTypeEnum.Response, resp);

            return Json(resp, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}