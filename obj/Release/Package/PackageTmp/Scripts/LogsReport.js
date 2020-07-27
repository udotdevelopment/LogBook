$(function (ready) {
    $(".datepicker").attr('type', 'text');
    $(".datepicker").datepicker();
});

$('#CreateMetric').click(function () {
    //var form = $("#MainForm")[0];
    //$.validator.unobtrusive.parse(form);
    //if ($(form).valid()) {
    GetReportCharts();
    //}
});

function GetReportCharts() {
    var tosend = {};
    GetToSendValues(tosend);
    GetMetric(urlGetReportCharts, tosend);
}

$('#Download').click(function () {
    GetDownload();
});
function GetDownload() {
    var tosend = {};
    GetToSendValues(tosend);
    GetFileDownload(urlGetDownload, tosend);
}

function GetToSendValues(tosend) {
    tosend.StartDate = $("#StartDate").val();
    tosend.EndDate = $("#EndDate").val();
    tosend.LocationId = $("#LocationId").val();
    tosend.CommentSearch = $("#CommentSearch").val();
    tosend.UserSearch = $("#UserSearch").val();
    tosend.OnSite = $("#OnSite").is(":checked");
    tosend.Remote = $("#Remote").is(":checked");

    tosend.ReasonForResponses = [];
    if ($("#Complaint_Work_Order").is(':checked')) {
        tosend.ReasonForResponses.push({ "id": "1" });
    }
    if ($("#Preventative_Maintenance").is(':checked')) {
        tosend.ReasonForResponses.push({ "id": "2" });
    }
    if ($("#On-call_After_Hours").is(':checked')) {
        tosend.ReasonForResponses.push({ "id": "3" });
    }
    if ($("#Failed_Equipment").is(':checked')) {
        tosend.ReasonForResponses.push({ "id": "4" });
    }
    if ($("#Detection_Related").is(':checked')) {
        tosend.ReasonForResponses.push({ "id": "5" });
    }
    if ($("#Timing").is(':checked')) {
        tosend.ReasonForResponses.push({ "id": "6" });
    }
    if ($("#Other").is(':checked')) {
        tosend.ReasonForResponses.push({ "id": "7" });
    }
}

function GetMetric(urlPath, tosend) {
    var test = JSON.stringify(tosend);

    $.ajax({
        url: urlPath,
        type: "POST",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        success: function (data) {

            $('#ReportPlaceHolder').html(data);
            $('#ReportPlaceHolder').focus();
        },
        beforeSend: function () {
            StartReportSpinner();
        },
        complete: function () {
            StopReportSpinner();
        },
        error: function (xhr, status, error) {
            StopReportSpinner();
            $('#ReportPlaceHolder').html(xhr.responseText);
        }
    });
}

function GetFileDownload(urlPath, tosend) {
    var test = JSON.stringify(tosend);

    $.ajax({
        url: urlPath,
        type: "POST",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        success: function (data) {
            var blob = new Blob([data], { type: "text/plain; encoding=utf8" });
            saveData(blob, 'ELogBookReport.csv');   
        },
        beforeSend: function () {
            StartReportSpinner();
        },
        complete: function () {
            StopReportSpinner();
        },
        error: function (xhr, status, error) {
            StopReportSpinner();
            $('#ReportPlaceHolder').html(xhr.responseText);
        }
    });
}

function saveData(blob, fileName) // does the same as FileSaver.js
{
    var a = document.createElement("a");
    var isIE = /*@cc_on!@*/false || !!document.documentMode;
    if (isIE)
        a.src = "about:blank";
        //a.src = "javascript:'<script>window.onload=function(){document.write(\\'<script>document.domain=\\\"" + document.domain + "\\\";<\\\\/script>\\');document.close();};<\/script>'";
    document.body.appendChild(a);
    a.style = "display: none";


    var url = window.URL.createObjectURL(blob);
    a.href = url;
    a.download = fileName;
    a.click();
    window.URL.revokeObjectURL(url);
}


function StartReportSpinner() {
    $("#SpinnerPlaceHolder").append(
        "<span id='LocationSpinner' class='spinner-border spinner-border-lg' role='status' aria-hidden='true'></span>");
}

function StopReportSpinner() {
    var element = document.getElementById("LocationSpinner");
    element.parentNode.removeChild(element);
}

function StartDownloadSpinner() {
    $("#SpinnerPlaceHolder").append(
        "<span id='LocationSpinner' class='spinner-border spinner-border-lg' role='status' aria-hidden='true'></span>");
}
function StopDownloadSpinner() {
    var element = document.getElementById("LocationSpinner");
    element.parentNode.removeChild(element);
}

function GetResultsPartialView(ReportPageNumber) {
    var tosend = {};
    GetToSendValues(tosend);
    tosend.ReportPageNumber = ReportPageNumber;
    GetMetric(urlGetReportCharts, tosend);

}
//{
//    var tosend = {};
//    tosend.id = $("#locationIdLabel")[0].innerText;
//    tosend.pageNumber = pageNumber;
//    tosend.searchText = $("#CommentSearchTextBox").val();
//    $.ajax({
//        url: urlGetReportCharts,
//        type: "GET",
//        cache: false,
//        async: true,
//        datatype: "json",
//        contentType: "application/json; charset=utf-8",
//        data: tosend,
//        success: function (data) {
//            $('#CommentsCardBody').html(data);
//        },
//        error: function (xhr, status, error) {
//            $('#CommentsCardBody').html(xhr.responseText);
//        },
//        complete: function () {
//            var commentsTotal = $('#CommentsTotalNumber').val();
//            $('#CommentsBadge')[0].innerText = commentsTotal;
//        }
//    });
//}