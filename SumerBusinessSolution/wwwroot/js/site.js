// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

//$(function () {
//    $('button[data-toggle="ajax-modal"]').click(function (event) {
//        alert('button clicked');
//    });
//});

$(function () {
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        // url to Razor Pages handler which returns modal HTML
        var url = '/sales/billings/create?handler=CreateCustomerModalPartial';
        $.get(url).done(function (data) {
            // append HTML to document, find modal and show it
            $(document).append(data).find('.modal').modal('show');
        });
    });
});

$(function () {

    $('[data-toggle="tooltip"]').tooltip();

});

function enableNotifications() {

    var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

    connection.on("NewTransferRequest", function (from, to, id) {
        console.log("received notification");
        $('<tr><td>' + from + '</td><td>' + to + '</td><td><a data-notificationId="' + id + '" class="notification-link" href="/inventory/transferrequests/requestdetails?ReqId=' + id + '" target="_blank">Link</a></td></tr>').prependTo('#table-body');

        $('#notification').popover('show');
    });

    connection.start().then(function () {
        return console.log("Connected");
    }).catch(function (err) {
        return console.error(err.toString());
    });


    $('#notification').popover({
        content: function () {
            return $($("#notification-content").html());
        }
    });

    $.getJSON('/inventory/transferrequests/index?handler=pendingrequests', function (data) {
        if (data && data.length) {
            data.forEach((tx) => {
                $('<tr><td>' + tx.fromWarehouse.whName + '</td><td>' + tx.toWarehouse.whName + '</td><td><a data-notificationId="' + tx.id + '" class="notification-link" href="/inventory/transferrequests/requestdetails?ReqId=' + tx.id + '" target="_blank">Link</a></td></tr>').prependTo('#table-body');
            });
            //$('#notification').popover('show');
        }
    });
}