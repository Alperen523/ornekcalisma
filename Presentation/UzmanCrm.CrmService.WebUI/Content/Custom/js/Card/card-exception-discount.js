var pathname = window.location.pathname; // Returns path only (/Card/CardExceptionDiscount)
var url = window.location.href;     // Returns full URL (https://localhost:44323/Card/CardExceptionDiscount?PortalUserId=4B7198C3-D1FD-419D-B44F-BA83FFEB895F)
var origin = window.location.origin;   // Returns base URL (https://localhost:44323)

if (origin.includes("vip")) {
    //Publish URL
    var getCustomerGroupList = "/Loyalty/Card/GetCustomerGroupList";
    var getPortalUser = "/Loyalty/Card/GetPortalUser";
    var saveCardExDiscount = "/Loyalty/Card/SaveCardExDiscount";
    var getCardDiscount = "/Loyalty/Card/GetCardDiscount";
    var updateCardExceptionDiscountApprovalStatusAndExplanation = "/Loyalty/Card/UpdateCardExceptionDiscountApprovalStatusAndExplanation";
    var sendEmailToApproval = "/Loyalty/Card/SendEmailToApproval";
    var getContactsForCard = "/Loyalty/Card/GetContactsForCard";
    var getLoyaltyCard = "/Loyalty/Card/GetLoyaltyCard";
    var getCustomerEndorsements = "/Loyalty/Card/GetCustomerEndorsements";
}

else {
    //URL
    var getCustomerGroupList = "/Card/GetCustomerGroupList";
    var getPortalUser = "/Card/GetPortalUser";
    var saveCardExDiscount = "/Card/SaveCardExDiscount";
    var getCardDiscount = "/Card/GetCardDiscount";
    var updateCardExceptionDiscountApprovalStatusAndExplanation = "/Card/UpdateCardExceptionDiscountApprovalStatusAndExplanation";
    var sendEmailToApproval = "/Card/SendEmailToApproval";
    var getContactsForCard = "/Card/GetContactsForCard";
    var getLoyaltyCard = "/Card/GetLoyaltyCard";
    var getCustomerEndorsements = "/Card/GetCustomerEndorsements";
}

var today = moment();
var future = moment(today).add(1, 'Y');
var t = today.format('YYYY-MM-DD');
var f = future.format('YYYY-MM-DD');

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

    $('form#RegisterForm').find('input').each(function () {
        $(".required-indicator").css({ "color": "red", "font-weight": "bold" });
        if ($(this).prop('required')) {
            $(this).append(
                $("<span>", { "class": "required-indicator" }).text("*")
            );
        }
    });

    var portalUserId = $('#PortalUserId').val();

    $.ajax({
        url: getPortalUser,
        type: "POST",
        dataType: "json",
        data: {
            PortalUserId: portalUserId
        },
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (res) {
            //console.log(res.Data.uzm_cardexceptiondiscountapprover);
            //console.log(res.Data.uzm_cardexceptiondiscountapproverName);
            $('#DemandedUserName').val(res.Data.uzm_fullname);
            $('#DemandedUserId').val(res.Data.uzm_portaluserid);
            $('#OrganizationId').val(res.Data.uzm_organizationid);
            //$('#ApprovedBy').val(res.Data.uzm_firstname + " " + res.Data.uzm_lastname);
            $('#ApprovedBy').val('ÜST YÖNETİM KOMİTESİ');
            $('#ApprovingSuperVisorId').val(res.Data.uzm_cardexceptiondiscountapprover);
            var approvingSupervisorId = $('#ApprovingSuperVisorId').val().toUpperCase(); //If Hülya Kaan label-> Üst Yönetim Komitesi
            if (approvingSupervisorId == '658CD83A-C4DA-45DF-AF25-4A298868A680') {
                $('#ApprovedBy').val('ÜST YÖNETİM KOMİTESİ')
            }
            $('#BusinessUnitId').val(res.Data.BusinessUnitId);

            var erp = $('#PortalErpId').val();
            var organization = $('#OrganizationId').val();
            if (erp != null && erp != "") {
                $.ajax({
                    url: getLoyaltyCard,
                    type: "GET",
                    dataType: "json",
                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                    data: {
                        ErpId: erp,
                        OrganizationId: organization
                    },
                    success: function (res) {
                        $('#ValidDiscountRate').val(res.ValidDiscountRate); //Mevcut Oran
                        $('#TurnOverEndorsement').val(addCommas(res.TurnoverEndorsement.toString())); //Müşteri Bir Önceki Yıl Toplam Alışveriş Tutarı
                        $('#PeriodEndorsement').val(addCommas(res.PeriodEndorsement.toString())); //Müşteri Mevcut Yıl Toplam Alışveriş Tutarı
                        $('#LoyaltyCardId').val(res.Id);
                        $('#CustomerName').val(res.CustomerName);
                        $('#CustomerCardNumber').val(res.CardNumber);
                        $('#CustomerErpId').val(res.ErpId);
                        var cardType = res.CardTypeDefinitionName;
                        //$.ajax({
                        //    url: getCustomerEndorsements,
                        //    type: "POST",
                        //    dataType: "json",
                        //    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                        //    data: {
                        //        ErpId: erp,
                        //        CardType: cardType,
                        //    },
                        //    success: function (res) {
                        //        debugger;
                        //        //$('#ValidDiscountRate').val(res.DiscountPercent); //Mevcut Oran
                        //    },
                        //    error: function (res) {
                        //        console.log('ERROR');
                        //        Swal.fire({
                        //            icon: 'error',
                        //            title: 'İşlem Başarısız!',
                        //            text: 'Bir hata oluştu, lütfen tekrar deneyin.',
                        //        })
                        //    }
                        //});

                        $.ajax({
                            url: getContactsForCard,
                            type: "POST",
                            dataType: "json",
                            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                            data: {
                                ErpId: res.ErpId,
                            },
                            success: function (res) {
                                var mobile = res.Data[0].mobilephone;
                                $('#CustomerPhone').val(mobile);
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
        },
        error: function (res) {
            console.log("ERROR")
        }
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

    $.ajax({
        url: getCustomerGroupList,
        type: "GET",
        dataType: "json",
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        success: function (res) {
            for (const resp of res) {
                $('#CustomerGroupId').append(`<option value="${resp.Id}">${resp.CustomerGroupName}</option>`); //Get CustomerGroup dynamically from server
            };
            //for (const ccs of res[res.length - 1].CardClassSegment) {
            //    $('#CardClassSegmentId').append(`<option value="${ccs.Id}">${ccs.SegmentName}</option>`);
            //}
            $("#CustomerGroupId").find('option:contains(null)').hide(); //Hide null options in select
            var select = $('#CustomerGroupId');
            select.find('option:contains("Diğer")').appendTo(select);   //Change 'Diğer' placement to end of the optionset
        },
        error: function (res) {
            $('#CustomerGroupId').append(`<option value="${res.Message}">${res.Message}</option>`);
        }
    });
});


//$("#CustomerGroupId").change(function () {
//    var customerGroup = $('#CustomerGroupId').find(":selected").text();

//    var customerGroupValuesF2 = ['Sanatçı', 'Bakan', 'Sporcu', 'Milletvekili', 'Kaymakam'];
//    var customerGroupValuesF3 = ['Belediye Başkanı', 'Basın', 'Cemiyet', 'Tedarikçi', 'İş Ortağı', 'Diğer'];

//    var custGroup = $('#CustomerGroupId').find(":selected").text();

//    if ($.inArray(custGroup, customerGroupValuesF2) != -1) {
//        $('#CardClassSegmentId').val("118396ec-4633-ed11-915c-00505685232b"); //F2
//    }
//    if ($.inArray(custGroup, customerGroupValuesF3) != -1) {
//        $('#CardClassSegmentId').val("437a9ff6-4633-ed11-915c-00505685232b"); //F3
//    }
//});

$('#RegisterForm').submit(function (e) {
    e.preventDefault();
    var discountRate = $('#DiscountRate').val();
    var endDate = $('#EndDate').val();
    var customerGroupId = $('#CustomerGroupId').val();
    var description = $('#Description').val();
    var loyaltyCardId = $('#LoyaltyCardId').val();
    var demandedUserId = $('#DemandedUserId').val();
    var startDate = moment().format('YYYY-MM-DD hh:mm:ss');
    var demandDate = moment().format('YYYY-MM-DD hh:mm:ss');
    var customerGroup = $('#CustomerGroupId').find(":selected").text();

    var customerGroupValuesF2 = ['Sanatçı', 'Bakan', 'Sporcu', 'Milletvekili', 'Kaymakam'];
    var customerGroupValuesF3 = ['Belediye Başkanı', 'Basın', 'Cemiyet', 'Tedarikçi', 'İş Ortağı', 'Diğer'];

    var custGroup = $('#CustomerGroupId').find(":selected").text();

    //if ($.inArray(custGroup, customerGroupValuesF2) != -1) {
    //    $('#CardClassSegmentId').val("118396ec-4633-ed11-915c-00505685232b"); //F2
    //}
    //if ($.inArray(custGroup, customerGroupValuesF3) != -1) {
    //    $('#CardClassSegmentId').val("437a9ff6-4633-ed11-915c-00505685232b"); //F3
    //}

    //var cardClassSegmentId = $('#CardClassSegmentId').find(":selected").val();

    var approvedBy = $('#ApprovedBy').val();
    var approvingSupervisorId = $('#ApprovingSuperVisorId').val();

    var validDiscountRate = $('#ValidDiscountRate').val();

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
            title: 'Talep edilen yeni oran mevcut indirim oranından küçük olamaz!'
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
        icon: 'info',
        title: 'Kart İndirim Talebi',
        html: "Kart indirim talebiniz " + approvedBy + "'ne gönderilecek." + "</br></br>" + "Bilgilerin doğru olduğundan emin misiniz?",
        showDenyButton: true,
        confirmButtonText: 'Tamam',
        denyButtonText: `İptal`,
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: saveCardExDiscount,
                type: "POST",
                dataType: "json",
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                data: {
                    DiscountRate: discountRate,
                    EndDate: endDate,
                    CustomerGroupId: customerGroupId,
                    Description: description,
                    LoyaltyCardId: loyaltyCardId,
                    DemandedUserId: demandedUserId,
                    startDate,
                    demandDate,
                    //cardClassSegmentId,
                    ApprovedByUserId: approvingSupervisorId
                },
                success: function (res) {
                    if (res.Success) {

                        var loyaltyCardId = $("#LoyaltyCardId").val();

                        $.ajax({
                            url: getCardDiscount,
                            type: "POST",
                            dataType: "json",
                            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                            data: {
                                LoyaltyCardId: loyaltyCardId
                            },
                            success: function (res) {
                                $('#CardDiscountId').val(res.Data.uzm_carddiscountid);
                                //console.log($('#CardDiscountId').val())
                                var cardDiscountId = $("#CardDiscountId").val();
                                var approvalStatus = 'WaitingForApproval';
                                var statusCode = 'Draft';
                                var arrivalChannel = 'Portal';
                                var businessUnitId = $("#BusinessUnitId").val();
                                //console.log(cardDiscountId);
                                $.ajax({
                                    url: updateCardExceptionDiscountApprovalStatusAndExplanation,
                                    type: "POST",
                                    dataType: "json",
                                    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                                    data: {
                                        CardDiscountId: cardDiscountId,
                                        ApprovalStatus: approvalStatus,
                                        StatusCode: statusCode,
                                        ArrivalChannel: arrivalChannel,
                                        BusinessUnitId: businessUnitId
                                    },
                                    success: function (res) {
                                        console.log('Card approval status is "DRAFT"')
                                    },
                                    error: function (res) {
                                        console.log('Card approval status is not change!!!');
                                    }
                                });
                                //!!!DEMAND EMAIL IS SENT VIA CRM WORKFLOW BUT CONFIRMATION MAIL SENT VIA VIP PORTAL!!!
                                //var approvingSupervisor = $("#ApprovingSuperVisorId").val();
                                //var loyaltyCardId = $("#LoyaltyCardId").val();
                                //var portalUserId = $("#PortalUserId").val();
                                ////Customer infos inside mail
                                //var discountRate = $("#DiscountRate").val();
                                //var customerName = $("#CustomerName").val();
                                //var validDiscountRate = $("#ValidDiscountRate").val();
                                //var demandedUserName = $("#DemandedUserName").val();
                                //var endDate = $("#EndDate").val();
                                //var description = $("#Description").val();

                                //var messageType = "SendApproval";
                                //$.ajax({
                                //    url: sendEmailToApproval,
                                //    type: "POST",
                                //    dataType: "json",
                                //    contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                                //    data: {
                                //        CardDiscountId: cardDiscountId,
                                //        ApprovingSuperVisorId: approvingSupervisor,
                                //        ApprovalStatus: approvalStatus,
                                //        LoyaltyCardId: loyaltyCardId,
                                //        PortalUserId: portalUserId,
                                //        Message: messageType,
                                //        DiscountRate: discountRate,
                                //        CustomerName: customerName,
                                //        ValidDiscountRate: validDiscountRate,
                                //        demandedUserName: demandedUserName,
                                //        EndDate: endDate,
                                //        Description: description
                                //    },
                                //    success: function (res) {
                                //        if (res.Success) {
                                //            console.log("Email sent");
                                //        }
                                //        else {
                                //            alert("Email gönderilirken bir sorun oluştu!");
                                //        }
                                //    },
                                //    error: function (res) {
                                //        alert('Email gönderilirken bir sorun oluştu!');
                                //    }
                                //});
                            },
                            error: function (res) {
                                console.log("ERROR")
                            }
                        });

                        Swal.fire({
                            icon: 'success',
                            title: 'İndirim talebiniz başarıyla kaydedildi',
                            /*text: res.Message,*/
                            showConfirmButton: true,
                        }).then((result) => {
                            if (result.isConfirmed) {
                                //parent.location.reload();
                                urlToGoto = "http://vip.vakko.com.tr/Home/Search";  //CANLI
                                //urlToGoto = "http://viptest.vakko.com.tr/Home/Search";    //TEST
                                parent.location.replace(urlToGoto);
                            }
                        });
                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: 'İndirim talebi oluşturulurken hata oluştu',
                            /*text: res.Message,*/
                            showConfirmButton: true,
                        })
                    }
                },
                error: function (res) {
                    Swal.fire({
                        icon: 'error',
                        title: 'İşlem Başarısız!',
                        text: 'Bir hata oluştu, lütfen tekrar deneyin.',
                    })
                }
            });

        } else if (result.isDenied) {
            Swal.fire({
                icon: 'error',
                title: 'İşlem Başarısız!',
                text: 'Talebiniz Kaydedilmedi!',
            })
        }
    })

    e.preventDefault();

});

$('#CustomerSearchBtn').click(function () {

    table.clear().draw();

    var erpId = $('#ErpId').val();
    var cardNo = $('#CardNo').val();
    var mobilePhone = $('#MobilePhone').val();
    var emailAddress1 = $('#EmailAddress1').val();

    $.ajax({
        url: getContactsForCard,
        type: "POST",
        dataType: "json",
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: {
            ErpId: erpId,
            CardNo: cardNo,
            MobilePhone: mobilePhone,
            EmailAddress1: emailAddress1
        },
        success: function (res) {
            for (const resp of res.Data) {
                table.row.add([
                    resp.uzm_erpid,
                    resp.fullname,
                    resp.mobilephone,
                    resp.emailaddress1,
                    resp.uzm_cardnumber
                ]).draw();
            };
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
})

$(document).on('keypress', function (e) {
    if ($("#customerSearchModal").hasClass('show') && (e.keyCode == 13 || e.which == 13)) {
        console.log("Enter clicked...")
        $('#CustomerSearchBtn').click();
        return false;
    }
});

$('#CustomerTable tbody').on('dblclick', 'tr', function () {
    var data = table.row(this).data();

    $('#CustomerErpId').val(data[0]);
    $('#CustomerName').val(data[1]);
    $('#CustomerPhone').val(data[2]);
    $('#CustomerCardNumber').val(data[4]);

    var erp = $('#CustomerErpId').val();
    var card = $('#CustomerCardNumber').val();

    $.ajax({
        url: getLoyaltyCard,
        type: "POST",
        dataType: "json",
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: {
            ErpId: erp,
            CardNo: card,
        },
        success: function (res) {
            //$('#ValidDiscountRate').val(res.ValidDiscountRate); //Mevcut Oran
            $('#TurnOverEndorsement').val(addCommas(res.TurnoverEndorsement)); //Müşteri Bir Önceki Yıl Toplam Alışveriş Tutarı
            $('#PeriodEndorsement').val(addCommas(res.PeriodEndorsement)); //Müşteri Mevcut Yıl Toplam Alışveriş Tutarı
            $('#LoyaltyCardId').val(res.Id);
            var cardType = res.CardTypeDefinitionName;
            $.ajax({
                url: getCustomerEndorsements,
                type: "GET",
                dataType: "json",
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                data: {
                    ErpId: erp,
                    CardType: cardType,
                },
                success: function (res) {
                    $('#ValidDiscountRate').val(res.DiscountPercent); //Mevcut Oran
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

    /*window.location = '@Url.Action("CardExceptionDiscount", "Card")?ErpId=' + erp + "&CardNo=" + card;*/

    $('#customerSearchModal').modal('toggle');
});