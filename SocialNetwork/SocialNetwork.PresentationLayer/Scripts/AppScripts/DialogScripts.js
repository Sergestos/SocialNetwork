function LoadDialog(url) {
    console.log(url);
    $('#dialog-main').load(url);
}

function IsTextEmpty() {
    if (isCanSend) {
        if ($.trim($('#dialog-main-tabfield-text').val()) == '') {
            $('#dialog-main-tabfield-submit').prop("disabled", true);
        }
        else {
            $('#dialog-main-tabfield-submit').prop("disabled", false);
        }
    }
}

function IsFileChoosen() {
    if (isCanSend) {
        var fileName = $('#fileInput1').val();
        if (fileName != '') {
            $('#dialog-main-tabfield-submit').prop("disabled", false);
        }
        else {
            $('#dialog-main-tabfield-submit').prop("disabled", true);
        }
    }    
}

window.onload = function () {
    var lastDialogID = $.cookie('lastDialogId');
    console.log("Last page is: " + lastDialogID);
    if (lastDialogID != "none" && isNumber(lastDialogID)) {
        var url = "/Dialog/Dialog?dialogID=" + Number(lastDialogID);
        $('#dialog-main').load(url);
        $('#dialog-main-tabfield').show();
    }  

    $('#hiddenDialogIdSender').val($.cookie('lastDialogId'));
}

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

function ResetInputs() {
    $('#dialog-main-tabfield-text').val('');
    $('#fileInput1').val('');

    $('#dialog-main-tabfield-submit').prop("disabled", true);
    $('#dialog-main-tabfield-submit').prop("disabled", true);
}


window.addEventListener("submit", function (e) {
    var form = e.target;
    if (form.getAttribute("enctype") === "multipart/form-data") {
        if (form.dataset.ajax) {
            e.preventDefault();
            e.stopImmediatePropagation();
            var xhr = new XMLHttpRequest();
            xhr.open(form.method, form.action);
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    if (form.dataset.ajaxUpdate) {
                        var updateTarget = document.querySelector(form.dataset.ajaxUpdate);
                        if (updateTarget) {
                            updateTarget.innerHTML = xhr.responseText;
                        }
                        ResetInputs();
                    }
                }
            };
            xhr.send(new FormData(form));
        }
    }
}, true);


