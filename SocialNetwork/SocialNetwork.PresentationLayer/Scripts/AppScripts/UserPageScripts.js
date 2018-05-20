var IsFollowed = true;
var IsUserAllowsAddToDialog = true;
var IsUserInBlacklist = true;

function InitializeStates(isFollowed, isUserAllowsAddToDialog, isUserInBlacklist) {
    IsFollowed = isFollowed;
    IsUserAllowsAddToDialog = isUserAllowsAddToDialog;
    IsUserInBlacklist = isUserInBlacklist;

    if (!IsUserInBlacklist) {
        $("#buttonBlacklist").val("Block");

        if (IsFollowed === true) {
            $("#buttonFollow").val("Unfollow");
        }
        else {
            $("#buttonFollow").val("Follow");
        }

        if (IsUserAllowsAddToDialog === true) {
            $("#buttonStartDialog, #buttonAddToDialog").prop("disabled", false);
        }
        else if (IsUserAllowsAddToDialog === false) {
            $("#buttonStartDialog, #buttonAddToDialog").prop("disabled", true);
        }
    }
    else {
        $("#buttonBlacklist").val("Unblock");

        $("#buttonFollow").val("Follow");
        $("#buttonFollow").prop("disabled", true);        

        $("#buttonStartDialog, #buttonAddToDialog").prop("disabled", true);
    }   
}

function Following(id) {
    event.preventDefault();

    if (!IsUserInBlacklist) {
        if (IsFollowed === true) {
            Unfollow(id);
        }
        else {
            Follow(id);
        }
    }
}

function Unfollow(id) {             

    $.ajax({
        url: './Unfollow',
        type: "GET",
        data: { "id": id },
        contentType: 'application/json',
        success: function (result) {
            var obj = jQuery.parseJSON('"Result"');
            if (obj !== null) {
                IsFollowed = false;
                $("#buttonFollow").val("Follow");
            }
            else {
                console.log(9);
                // error message
            }
        }
    });
}

function Follow(id) {

    $.ajax({
        url: './Follow',
        type: "GET",
        data: { "id": id },
        contentType: 'application/json',
        success: function (result) {
            var obj = jQuery.parseJSON('"Result"');
            if (obj !== null) {
                IsFollowed = true;
                $("#buttonFollow").val("Unfollow");
            }
            else {
                // error message
            }
        }
    });
}

function Blocking(id) {
    event.preventDefault();

    if (IsUserInBlacklist === true) {
        Unblock(id);
    }
    else {
        Block(id);
    }   
}

function Block(id) {

    $.ajax({
        url: './Block',
        type: "GET",
        data: { "id": id },
        contentType: 'application/json',
        success: function (result) {
            var obj = jQuery.parseJSON('"Result"');
            if (obj !== null) {
                IsUserInBlacklist = true;     
                IsFollowed = false;

                $("#buttonBlacklist").val("Unblock");
                $("#buttonFollow").prop("disabled", true);    
                $("#buttonFollow").val("Follow");
                $("#buttonStartDialog, #buttonAddToDialog").prop("disabled", true);
            }
            else {
                // error message
            }
        }
    });
}

function Unblock(id) {    

    $.ajax({
        url: './Unblock',
        type: "GET",
        data: { "id": id },
        contentType: 'application/json',
        success: function (result) {
            var obj = jQuery.parseJSON('"Result"');
            if (obj !== null) {
                IsUserInBlacklist = false;
                IsFollowed = false;

                $("#buttonBlacklist").val("Block");
                $("#buttonFollow").prop("disabled", false);
                $("#buttonFollow").val("Follow");

                if (IsUserAllowsAddToDialog)
                    $("#buttonStartDialog, #buttonAddToDialog").prop("disabled", false);
            }
            else {
                // error message
            }
        }
    });
}

function StartDialog(id) {
    var url = "/User/CreateDialog?companion=" + id;
    window.open(url, "_self");
}