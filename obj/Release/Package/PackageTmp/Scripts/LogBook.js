$(document).ready(function () {
    var locationId = $("#LocationSearchTextBox").val();
    if (locationId !== "") {
        GetLogEntryPartialView();
    }
});

Number.isInteger = Number.isInteger || function (value) {
    return typeof value === "number" &&
        isFinite(value) &&
        Math.floor(value) === value;
};

function LocationIdPress(e) {
    if (e.which === 13) {
        e.preventDefault();
        GetLogEntryPartialView();
    }
}

function CommentSearchPress(e) {
    if (e.which === 13) {
        e.preventDefault();
        GetCommentsResultsPartialView();
    }
}

function ClearAll() {
    $('#LogEntry').html('');
    $('#Comments').html('');
    $('#MaxViewLogs').html('');
    $('#AIMSWorkOrders').html('');
    $('#CyberLockLogs').html('');
}

function GetLogEntryPartialView() {

    var locationId = $("#LocationSearchTextBox").val();
    if (CheckId(locationId)) {
        $.ajax({
            url: urlGetLogEntry,
            type: "GET",
            cache: false,
            async: true,
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            data: { "id": locationId },
            success: function(result) {
                ClearAll();
                $('#LogEntry').html(result);
                GetCommentsPartialView();
                GetMaxViewLogsPartialView();
                GetCyberLockLogsPartialView();
                GetAIMSWorkOrdersPartialView();
            },
            beforeSend: function() {
                $("#LocationSearch")
                    .append(
                        "<span id='LocationSpinner' class='spinner-border spinner-border-sm' role='status' aria-hidden='true'></span>");

                //StartReportSpinner();
            },
            complete: function() {
                $("#Timestamp").datetimepicker({ format: 'm/d/yy H:i', step: 5 });
                var element = document.getElementById("LocationSpinner");
                element.parentNode.removeChild(element);
            },
            error: function(xhr, status, error) {
                $('#LogEntry').html(xhr.responseText);
                var element = document.getElementById("LocationSpinner");
                element.parentNode.removeChild(element);
            }
        });
    }
}

function GetCommentsPartialView() {
    var tosend = {};
    tosend.id = $("#LocationSearchTextBox").val();
    $.ajax({
        url: urlGetComments,
        type: "GET",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data:tosend,
        success: function (data) {
            $('#Comments').html(data);
        },
        error: function (xhr, status, error) {
            $('#Comments').html(xhr.responseText);
        }
    });
}

function CheckId(id){
    if (id == null || id == "") {
        $('#LogEntry')
            .html("<ul><li class='list-group-item list-group-item-danger'>Location ID Required</li></ul>");
        return false;
    } else {
        return true;
    }
}

function GetCommentsResultsPartialView(pageNumber) {
    var tosend = {};
    tosend.id = $("#locationIdLabel")[0].innerText;
    tosend.pageNumber = pageNumber;
    tosend.searchText = $("#CommentSearchTextBox").val();
    $.ajax({
        url: urlGetCommentsResult,
        type: "GET",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: tosend,
        success: function (data) {
            $('#CommentsCardBody').html(data);
        },
        error: function (xhr, status, error) {
            $('#CommentsCardBody').html(xhr.responseText);
        },
        complete: function () {
            var commentsTotal = $('#CommentsTotalNumber').val();
            $('#CommentsBadge')[0].innerText = commentsTotal;
        }
    });
}

function GetMaxViewLogsPartialView() {
    var tosend = {};
    tosend.id = $("#LocationSearchTextBox").val();
    if ($.isNumeric(tosend.id)) {
        $.ajax({
            url: urlGetMaxViewLogs,
            type: "GET",
            cache: false,
            async: true,
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            data: tosend,
            success: function(data) {
                $('#MaxViewLogs').html(data);
            },
            error: function(xhr, status, error) {
                $('#MaxViewLogs').html(xhr.responseText);
            }
        });
    }
}

function GetMaxViewLogsResultPartialView(pageNumber) {
    var tosend = {};
    tosend.id = $("#locationIdLabel")[0].innerText;
    tosend.pageNumber = pageNumber;
    if ($.isNumeric(tosend.id)) {
        $.ajax({
            url: urlGetMaxViewLogsResult,
            type: "GET",
            cache: false,
            async: true,
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            data: tosend,
            success: function(data) {
                $('#MaxViewCardBody').html(data);
            },
            error: function(xhr, status, error) {
                $('#MaxViewCardBody').html(xhr.responseText);
            }
        });
    }
}

function GetCyberLockLogsResultPartialView(pageNumber) {
    var tosend = {};
    tosend.id = $("#locationIdLabel")[0].innerText;
    if ($.isNumeric(tosend.id)) {
        tosend.pageNumber = pageNumber;
        $.ajax({
            url: urlCyberLockLogsResult,
            type: "GET",
            cache: false,
            async: true,
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            data: tosend,
            success: function(data) {
                $('#CyberLockBody').html(data);
            },
            error: function(xhr, status, error) {
                $('#CyberLockBody').html(xhr.responseText);
            },
            complete: function() {
            }
        });
    }
}


function GetCyberLockLogsPartialView() {
    var tosend = {};
    tosend.id = $("#LocationSearchTextBox").val();
    if ($.isNumeric(tosend.id)) {
        $.ajax({
            url: urlCyberLockLogs,
            type: "GET",
            cache: false,
            async: true,
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            data: tosend,
            success: function(data) {
                $('#CyberLockLogs').html(data);
            },
            error: function(xhr, status, error) {
                $('#CyberLockLogs').html(xhr.responseText);
            }
        });
    }
}


function GetAIMSWorkOrdersPartialView() {
    var tosend = {};
    tosend.id = $("#LocationSearchTextBox").val();
    $.ajax({
        url: urlAIMSWorkOrders,
        type: "GET",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: tosend,
        success: function (data) {
            $('#AIMSWorkOrders').html(data);
        },
        error: function (xhr, status, error) {
            $('#LogEntry').html(xhr.responseText);
        }
    });
}

function GetAIMSResultPartialView(pageNumber) {
    if (pageNumber === undefined) {
        pageNumber = 1;
    }
    //if ($('#AIMSResults')[0].innerText == "") {
        var tosend = {};
    tosend.id = $("#locationIdLabel")[0].innerText;
        tosend.pageNumber = pageNumber;
        $.ajax({
            url: urlAIMSResult,
            type: "GET",
            cache: false,
            async: true,
            datatype: "json",
            contentType: "application/json; charset=utf-8",
            data: tosend,
            success: function (data) {
                $('#AIMSResults').html(data);
            },
            error: function (xhr, status, error) {
                $('#LogEntry').html(xhr.responseText);
            }
        });
    //}
}

function SaveLog() {
    var tosend = {};
    tosend.LocationId = $("#locationIdLabel")[0].innerText;
    tosend.LocationTypeDescription = $("#locationTypeLabel")[0].innerText;
    tosend.Timestamp = $("#Timestamp").val();
    tosend.Onsite = $("#Onsite").is(':checked');
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
    //tosend.WorkOrderNumber = $("#WorkOrderNumber").val();
    tosend.Comment = $("#Comment").val();
    $.ajax({
        url: urlSaveLogBookEntry,
        type: "Post",
        cache: false,
        async: true,
        datatype: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(tosend),
        success: function (data) {
            $('#SaveMessage').html(data);
        },
        beforeSend: function () {
            //StartReportSpinner();
        },
        complete: function () {
            //$('#Comment').val("");
            GetCommentsResultsPartialView(1);
            //StopReportSpinner();
        },
        error: function (xhr, status, error) {
            //StopReportSpinner();
            $('#LogEntry').html(xhr.responseText);
        }
    });
}