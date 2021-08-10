$(document).ready(function () {
    $("#btnLoadReport").click(function () {
        ReportManager.LoadReport();
    })
});

var ReportManager ={

    LoadReport: function () {
        var JsonParam = "",
        var serviceUrl = "../User/GenerateUserReport";
        function onFaailed(error) {
            alert("not Found");
        }
    },
    GetReport: function (serviceUrl, JsonParam, errorCallback) {
        jQuery.ajax({

            url: serviceUrl,
            async: false,
            type: "POST",
            data: "{" + JsonParam + "}",
            contentType: "application/json; charset=utf-8",
            success: function () {
                window.open('../Report/ReportViewer.aspx', '_newtab')
            },
            error: errorCallback
        });
    }
}