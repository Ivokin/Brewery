$(function () {
    $("#return").on('click', function (e) {
        e.preventDefault();
        var url = "RedirectIndex";
        $.get(url);
    });
});