// Write your JavaScript code.
$("#e-mail").click(function () {

    var $input = $("<input>");
    $("body").append($input);
    $input.val($(this).text()).select();
    document.execCommand("copy");
    $input.remove();
    

    alert("Copied to clipboard");
});
