$(function () {
    $(document).ajaxComplete(function myErrorHandler(event, xhr, ajaxOptions, thrownError) {
        if (xhr.status == 500) {
            document.location = "/Error/500";
        }       
    });
});