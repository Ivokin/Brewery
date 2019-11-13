$(function () {
    $(".tbody").on('click', ' .removeresource', function (e) {
        e.preventDefault();

        var table = $('.table');
        var id = $(this).attr('data-id');
        var unit = $(".select-resource#" + id + " option:selected")[0].id;
        var resourceName = $(".select-resource#" + id + " option:selected")[0].text;
        var rows = table.find('tr.data-row');
        var newItemIndex = rows.length;

        if (newItemIndex > 1) {
            var selects = $('.select-resource');
            $('tr#' + id).remove();
            for (var i = 0; i < selects.length; i++) {
                selects[i].append(new Option(resourceName, resourceName, false, false));
                var last = $('.select-resource')[i].length - 1;
                $('.select-resource')[i][last].id = unit;
            }
        }
    });
});