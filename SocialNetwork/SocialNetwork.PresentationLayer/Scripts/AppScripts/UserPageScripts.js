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
            var json = result['Result'];
            if (json !== null) {
                if (json == 'Unfollowed') {
                    IsFollowed = false;
                    $("#buttonFollow").val("Follow");
                }                
                else {
                    $("#errorMsg").text(json);
                    $("#myModalBox").modal('show');
                }
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
            var json = result['Result'];
            if (json !== null) {
                if (json == 'Followed') {
                    IsFollowed = true;
                    $("#buttonFollow").val("Unfollow");
                }
                else {
                    $("#errorMsg").text(json);
                    $("#myModalBox").modal('show');
                }
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
            var json = result['Result'];
            if (json !== null) {
                if (json == 'Blocked') {
                    IsUserInBlacklist = true;
                    IsFollowed = false;

                    $("#buttonBlacklist").val("Unblock");
                    $("#buttonFollow").prop("disabled", true);
                    $("#buttonFollow").val("Follow");
                    $("#buttonStartDialog, #buttonAddToDialog").prop("disabled", true);
                }
                else {
                    $("#errorMsg").text(json);
                    $("#myModalBox").modal('show');
                }                
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
            var json = result['Result'];
            if (json !== null) {
                if (json == 'Unblocked') {;
                    IsUserInBlacklist = false;
                    IsFollowed = false;

                    $("#buttonBlacklist").val("Block");
                    $("#buttonFollow").prop("disabled", false);
                    $("#buttonFollow").val("Follow");

                    if (IsUserAllowsAddToDialog)
                        $("#buttonStartDialog, #buttonAddToDialog").prop("disabled", false);
                }
                else {
                    $("#errorMsg").text(json);
                    $("#myModalBox").modal('show');
                }
            }                
        }
    });
}

function StartDialog(id) {
    var url = "/Dialog/CreateDialog?companion=" + id;
    window.open(url, "_self");
}
function AddToDialog(id) {
    var url = "/Dialog/AddToDialog?userID=" + id;
    window.open(url, "_self");
}