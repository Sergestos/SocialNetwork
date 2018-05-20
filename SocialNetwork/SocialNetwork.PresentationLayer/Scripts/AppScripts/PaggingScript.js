function InitilizePagging(currentPage, isLastPage) {
    if (currentPage == 0) {
        $("#pagging-firstPage, #pagging-previousPage").prop("disabled", true);
    }
    if (isLastPage) {
        $("#pagging-nextPage").prop("disabled", true);
    }        
}

function GoToPage(page) {
    var url = document.getElementById("form").getAttribute("action")
    url = url + "?page=" + page;
    console.log(url);
    window.open(url, "_self");
}