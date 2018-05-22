$(function () {
    console.log('Initialize signalR');

    var chat = $.connection.dialogHub; 

    $.connection.hub.start().done(function () {
        $(".dialog-preview-item-open-search").click(function () {
            console.log('Connection...');            
            chat.server.connect();
        });

        $('#dialog-main-tabfield-submit').click(function () {            
            console.log('New message: ' + $.cookie('lastDialogId'));
            chat.server.send($.cookie('lastDialogId'));
        });
    });

    chat.client.addMessage = function (message) {        
        console.log('New message gotten, target dialogID: ' + message);
        var dialogID = $.cookie('lastDialogId');
        if (window.location.pathname == '/User/DialogPreviews' && message == dialogID) {
            console.log('Reloading...');
            $('#dialog-main').load('/User/Dialog?dialogID=' + dialogID);
        }
    };
});

