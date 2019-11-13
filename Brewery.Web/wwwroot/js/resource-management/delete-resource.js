$(function () {
    $(".deleteresource").on('click', function (e) {
        e.preventDefault();
        var id = this.attributes[3].nodeValue;
        var url = "ResourceManager/Delete/";

        $.post(url + id, function (data) {
            if (data.success == true) {
                $('#table_id tr#' + id).remove();
            }
        });
    });
});