
var origin = window.location.origin;   // Returns base URL (https://localhost:44323)

if (origin.includes("vip")) {
    //Publish URL
    var getCustomerGroupList = "/Loyalty/Card/GetCustomerGroupList";
    var getCardDiscount = "/Loyalty/Card/GetCardDiscount";
    var updateCardExceptionDiscountApprovalStatusAndExplanation = "/Loyalty/Card/UpdateCardExceptionDiscountApprovalStatusAndExplanation";
    var updateLoyaltyCardValidDiscountRate = "/Loyalty/Card/UpdateLoyaltyCardValidDiscountRate";
    var updateEndDateAndDiscountRate = "/Loyalty/Card/UpdateCardExceptionEndDateAndDiscountRate";
    var updateApprovalExplanation = "/Loyalty/Card/UpdateCardExceptionApprovalExplanation";
    var sendEmailToApproval = "/Loyalty/Card/SendEmailToApproval";
}

else {
    //URL
    var getCustomerGroupList = "/Card/GetCustomerGroupList";
    var getCardDiscount = "/Card/GetCardDiscount";
    var updateCardExceptionDiscountApprovalStatusAndExplanation = "/Card/UpdateCardExceptionDiscountApprovalStatusAndExplanation";
    var updateLoyaltyCardValidDiscountRate = "/Card/UpdateLoyaltyCardValidDiscountRate";
    var updateEndDateAndDiscountRate = "/Card/UpdateCardExceptionEndDateAndDiscountRate";
    var updateApprovalExplanation = "/Card/UpdateCardExceptionApprovalExplanation";
    var sendEmailToApproval = "/Card/SendEmailToApproval";
}


var today = moment();
var future = moment(today).add(1, 'Y');
var t = today.format('YYYY-MM-DD');
var f = future.format('YYYY-MM-DD');

var endDateFlag = 0;
console.log(endDateFlag);

var table = $('#CustomerTable').DataTable({
    rowReorder: {
        selector: 'td:nth-child(2)'
    },
    responsive: true
});

$(document).ready(function () {
    EndDate.max = new Date(f).toISOString().split("T")[0];
    EndDate.min = new Date(t).toISOString().split("T")[0];

    $('#TodaysDate').val(today.format('DD.MM.YYYY'));

    $('#EndDate, #DiscountRate').on('input', function () {
        endDateFlag = 1;
        console.log(endDateFlag);
    });

    $('form#RegisterForm').find('input').each(function () {
        $(".required-indicator").css({ "color": "red", "font-weight": "bold" });
        if ($(this).prop('required')) {
            $(this).append(
                $("<span>", { "class": "required-indicator" }).text("*")
            );
        }
    });

    $.ajax({
        url: getCustomerGroupList,
        type: "GET",
        dataType: "json",
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (res) {
            for (const resp of res) {
                $('#CustomerGroupId').append(`<option value="${resp.Id}">${resp.CustomerGroupName}</option>`);
                var id = $('#OptionSetCustomerGroupId').val();
            };

            $('#CustomerGroupId').val(id);

            for (const ccs of res[res.length - 1].CardClassSegment) {
                $('#CardClassSegmentId').append(`<option value="${ccs.SegmentName}">${ccs.Id}</option>`);
            }
        },
        error: function (res) {
            console.log(res.responseText);
            //$('#CustomerGroupId').append(`<option value="${resp.Id}">${resp.CustomerGroupName}</option>`);
        }
    });

    var loyaltyCardId = $("#LoyaltyCardId").val();
    $.ajax({
        url: getCardDiscount,
        type: "POST",
        dataType: "json",
        data: {
            LoyaltyCardId: loyaltyCardId
        },
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (res) {
            var date = res.Data.uzm_enddate;
            var parseDate = new Date(parseInt(date.substr(6)));
            parseDate.setHours(parseDate.getHours() + 3);
            var newDate = parseDate.toISOString().substr(0, 10);
            $('#CustomerName').val(res.Data.uzm_contactidname);
            $('#DemandedUserName').val(res.Data.uzm_demandedusername);
            $('#DemandedUserId').val(res.Data.uzm_demandeduser);
            $('#Description').val(res.Data.uzm_description);
            $('#DiscountRate').val(res.Data.uzm_discountrate);
            $('#EndDate').val(newDate);
            $('#OptionSetCustomerGroupId').val(res.Data.uzm_customergroupid);
            $('#CustomerCardNumber').val(res.Data.uzm_cardnumber);
            $('#CustomerErpId').val(res.Data.uzm_erpid);
            $('#CustomerPhone').val(res.Data.uzm_mobilephone);
            $('#ValidDiscountRate').val(res.Data.uzm_validdiscountrate);
            $('#TurnOverEndorsement').val(addCommas(res.Data.uzm_turnoverendorsement));
            $('#CardDiscountId').val(res.Data.uzm_carddiscountid);
            $('#ValidEndorsement').val(addCommas(res.Data.uzm_periodendorsement));
        },
        error: function (res) {
            console.log("ERROR")
        }
    });
});

function addCommas(nStr) {
    nStr += '';
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

$('#Accept').click(function () {
    var discountRate = $('#DiscountRate').val();
    var validDiscountRate = $('#ValidDiscountRate').val();
    var customerName = $("#CustomerName").val();

    if (parseInt(discountRate) <= parseInt(validDiscountRate)) {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-right',
            iconColor: 'white',
            customClass: {
                popup: 'colored-toast'
            },
            showConfirmButton: false,
            timer: 2500,
            timerProgressBar: true
        })
        Toast.fire({
            icon: 'error',
            title: 'Lütfen mevcut orandan daha büyük bir değer giriniz!'
        })

        return false;
    }

    if (parseInt(discountRate) % 5 != 0) {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-right',
            iconColor: 'white',
            customClass: {
                popup: 'colored-toast'
            },
            showConfirmButton: false,
            timer: 2500,
            timerProgressBar: true
        })
        Toast.fire({
            icon: 'error',
            title: 'Talep edilen yeni oran 5 ve 5`in katı olmalıdır!'
        })

        return false;
    }

    Swal.fire({
        title: 'Talebi onaylamak istediğinize emin misiniz?',
        input: 'textarea',
        inputLabel: 'Lütfen onay nedeninizi belirtiniz',
        inputPlaceholder: 'Onay nedenini giriniz...',
        inputAttributes: {
            'aria-label': 'Onay nedenini giriniz'
        },
        inputValue: 'Uygundur.',
        inputAttributes: {
            autocapitalize: 'off'
        },
        icon: 'info',
        showLoaderOnConfirm: true,
        preConfirm: (value) => {
            if (!value) {
                Swal.showValidationMessage(
                    '<i class="fa fa-info-circle"></i> Açıklama alanı boş olamaz!'
                )
            }
        },
        showDenyButton: true,
        confirmButtonText: 'Evet',
        denyButtonText: `Hayır`
    }).then((result) => {
        if (result.isConfirmed) {
            var cardDiscountId = $("#CardDiscountId").val();
            var approvalStatus = 'Approved';
            var statusCode = 'InUse';
            var discountRate = $('#DiscountRate').val();
            var validDiscountRate = $('#ValidDiscountRate').val();
            var approvalExplanation = result.value;
            //console.log(result.value);

            $.ajax({
                url: updateCardExceptionDiscountApprovalStatusAndExplanation,
                type: "POST",
                dataType: "json",
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                data: {
                    CardDiscountId: cardDiscountId,
                    ApprovalStatus: approvalStatus,
                    StatusCode: statusCode,
                    ApprovalExplanation: approvalExplanation
                },
                success: function (res) {
                    var loyaltyCardId = $("#LoyaltyCardId").val();
                    var endDate = $('#EndDate').val();
                    var discountRate = $('#DiscountRate').val();
                    //$.ajax({
                    //    url: updateLoyaltyCardValidDiscountRate,
                    //    type: "POST",
                    //    dataType: "json",
                    //    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    //    data: {
                    //        LoyaltyCardId: loyaltyCardId,
                    //        ValidDiscountRate: discountRate
                    //    },
                    //    success: function (res) {
                    //        console.log('Eureka! Discountrate is updated')
                    //    },
                    //    error: function (res) {
                    //        console.log('Error!!! Discountrate update unsuccessfull');
                    //    }
                    //});

                    if (endDateFlag == 1) {

                        $.ajax({
                            url: updateEndDateAndDiscountRate,
                            type: "POST",
                            dataType: "json",
                            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                            data: {
                                CardDiscountId: cardDiscountId,
                                EndDate: endDate,
                                DiscountRate: discountRate
                            },
                            success: function (res) {
                                console.log('Eureka! Enddate is updated')
                            },
                            error: function (res) {
                                console.log('Error!!! Enddate update unsuccessfull');
                            }
                        });
                    }
                    debugger;
                    var portalUserId = $('#DemandedUserId').val();
                    var messageType = "SendRequester";
                    var reqResult = "kabul edildi.";
                    var discountRate = $('#DiscountRate').val();
                    //Customer infos inside mail
                    var endDate = $("#EndDate").val();
                    var demandStore = $("#Store").val();                  
                    $.ajax({
                        url: sendEmailToApproval,
                        type: "POST",
                        dataType: "json",
                        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                        data: {
                            PortalUserId: portalUserId,
                            Message: messageType,
                            RequestResult: reqResult,
                            approvalExplanation,
                            DiscountRate: discountRate,
                            EndDate: endDate,
                            DemandStore: demandStore,
                            CustomerName: customerName
                        },
                        success: function (res) {
                            if (res.Success) {
                                console.log("Email sent");
                            }
                            else {
                                alert("Email not sent!");
                            }
                        },
                        error: function (res) {
                            alert('Email not sent!!!');
                        }
                    });

                    Swal.fire({
                        title: 'Talep kabul edildi!',
                        icon: 'success',
                        confirmButtonColor: '#3085d6',
                        confirmButtonText: 'Tamam',
                        allowOutsideClick: false
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.reload();
                        }
                    })
                },
                error: function (res) {
                    console.log('ERROR');
                    Swal.fire({
                        icon: 'error',
                        title: 'İşlem Başarısız!',
                        text: 'Bir hata oluştu, lütfen tekrar deneyin.',
                    })
                }
            });
        }
    })
});

$('#Decline').click(function () {
    Swal.fire({
        title: 'Talebi reddetmek istediğinize emin misiniz?',
        input: 'textarea',
        inputLabel: 'Lütfen ret nedeninizi belirtiniz',
        inputPlaceholder: 'Ret nedenini giriniz...',
        inputAttributes: {
            'aria-label': 'Ret nedenini giriniz'
        },
        inputAttributes: {
            autocapitalize: 'off'
        },
        icon: 'info',
        showLoaderOnConfirm: true,
        preConfirm: (value) => {
            if (!value) {
                Swal.showValidationMessage(
                    '<i class="fa fa-info-circle"></i> Açıklama alanı boş olamaz!'
                )
            }
        },
        showDenyButton: true,
        confirmButtonText: 'Evet',
        denyButtonText: `Hayır`,
    }).then((result) => {
        if (result.isConfirmed) {
            debugger;
            var customerName = $("#CustomerName").val();
            var cardDiscountId = $("#CardDiscountId").val();
            var approvalStatus = 'Denied';
            var statusCode = 'Canceled';
            var approvalExplanation = result.value;

            //console.log(result.value);
            $.ajax({
                url: updateCardExceptionDiscountApprovalStatusAndExplanation,
                type: "POST",
                dataType: "json",
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                data: {
                    CardDiscountId: cardDiscountId,
                    ApprovalStatus: approvalStatus,
                    StatusCode: statusCode,
                    ApprovalExplanation: approvalExplanation                  
                },
                success: function (res) {
                    if (res.Success) {

                        var portalUserId = $('#DemandedUserId').val();
                        var messageType = "SendRequester";
                        var reqResult = "reddedildi.";
                        var discountRate = $('#DiscountRate').val();
                        //Customer infos inside mail
                        var endDate = $("#EndDate").val();
                        var demandStore = $("#Store").val();

                        $.ajax({
                            url: sendEmailToApproval,
                            type: "POST",
                            dataType: "json",
                            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                            data: {
                                PortalUserId: portalUserId,
                                Message: messageType,
                                RequestResult: reqResult,
                                approvalExplanation,
                                DiscountRate: discountRate,
                                EndDate: endDate,
                                DemandStore: demandStore,
                                CustomerName: customerName
                            },
                            success: function (res) {
                                if (res.Success) {
                                    console.log("Email sent");
                                }
                                else {
                                    alert("Email not sent!");
                                }
                            },
                            error: function (res) {
                                alert('Email not sent!!!');
                            }
                        });

                        Swal.fire({
                            title: 'Talep reddedildi!',
                            icon: 'success',
                            confirmButtonColor: '#3085d6',
                            confirmButtonText: 'Tamam',
                            allowOutsideClick: false
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.reload();
                            }
                        })
                    }
                    else {
                        Swal.fire('Bir hata oluştu!', 'Lütfen tekrar deneyiniz.', 'error')
                    }
                },
                error: function (res) {
                    console.log('ERROR');
                    Swal.fire({
                        icon: 'error',
                        title: 'İşlem Başarısız!',
                        text: 'Bir hata oluştu, lütfen tekrar deneyin.',
                    })
                }
            });
        }
    })
});