$(function () {
    $(".btn-delete").on('click', function (e) {
        e.preventDefault();

        var id = $(this).attr('data-id');
        var url = "BrewsManagement/Delete";
        var token = $('input[name="__RequestVerificationToken"]').val();

        $.ajax({
            method: 'POST',
            url: url,
            data: {
                __RequestVerificationToken: token,
                id: id
            }
        }).done(function (response) {
            if (response.success) {
                $('#table_id tr#' + id).remove();
            }
            else {
                alert('faild')
            }
        });
    });
});