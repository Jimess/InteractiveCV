// Write your JavaScript code.
$("#e-mail").click(function () {

    var $input = $("<input>");
    $("body").append($input);
    $input.val($(this).text()).select();
    document.execCommand("copy");
    $input.remove();

    alert("Copied to clipboard");
});

/* Hide elemenets in the AddLink View which are not needed for the form
   i.e when isExternal is selected ignore project number;
   when isExternal is not selected ignore externalLink*/

/* Not working as it only reads the change once.
$("#linkControl").change(function () {
    if ($(".extLink").prop("checked") == true) {
        //alert("checked");
        $(".extLink").show();
    } else {
        $(".extLink").hide();
        //alert("not checked");
    }
}); */

$(document).ready(checkForExternalControl);

$("#linkControl").click(function () {
    checkForExternalControl();
});

function checkForExternalControl() {
    if ($("#linkControl").prop("checked") == true) {
        //show external link fields
        $(".extLink").css("visibility", "visible");
        //hide normal(internal) link fields
        $(".intLink").css("visibility", "hidden");
    } else {
        //hide external link fields
        $(".extLink").css("visibility", "hidden");
        //display normal(internal) link fields
        $(".intLink").css("visibility", "visible");
    }
}
