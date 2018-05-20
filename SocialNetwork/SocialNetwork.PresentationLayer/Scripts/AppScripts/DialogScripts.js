function LoadDialog(url) {
    $('#dialog-main').load(url);
}

function IsTextEmpty() {    
    if ($.trim($('#dialog-main-tabfield-text').val()) == '') {
        $('#dialog-main-tabfield-send').prop("disabled", true);
    }
    else {
        $('#dialog-main-tabfield-send').prop("disabled", false);        
    }
}

function IsFileChoosen() {
    console.log('1');
    var fileName = $('#fileInput1').val();
    if (fileName != '') {
        $('#dialog-main-tabfield-send').prop("disabled", false);
    }
    else {
        $('#dialog-main-tabfield-send').prop("disabled", true);
    }
}

window.onload = function () {
    var lastDialogID = $("#hiddenLastPage").val();
    console.log(lastDialogID);
    if (lastDialogID != "none" && isNumber(lastDialogID)) {
        var url = "/User/Dialog?dialogID=" + Number(lastDialogID);
        $('#dialog-main').load(url);
    }
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
                    }
                }
            };
            xhr.send(new FormData(form));
        }
    }
}, true);

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
