$(function () {
    $(document).ajaxComplete(function myErrorHandler(event, xhr, ajaxOptions, thrownError) {
        if (xhr.status == 500) {
            console.log("Ajax request completed with response code " + xhr.status);
        }       
    });
});