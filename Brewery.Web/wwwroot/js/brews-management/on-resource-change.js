$(document).on('change', 'select', function (e) {

    e.preventDefault();
    var id = e.originalEvent.target.id;
    var unit = $("#" + id + " option:selected")[0].id;
    var index = id.charAt(id.length - 1);
    $("#unit-" + index).text(unit);
});