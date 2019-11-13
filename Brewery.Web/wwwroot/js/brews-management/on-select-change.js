$(document).on('change', '.select-resource', function (e) {

    e.preventDefault();
    var str = ""
    var resources = [];
    var url = "/BrewsManagement/GetOptions/";
    var num = 0;

    $("select option:selected").each(function () {
        str += $(this).attr("value") + " ";
        resources[num] = $(this).attr("value");
        num += 1;
    });
    num = 0;

    $("#amount-in-stock").empty();
    $("#amount-in-stock").text(str);

    var baseOptions;

    $.ajax({
        method: 'POST',
        url: url,
        data: {
            resources: resources
        },
    }).done(function (response) {
        baseOptions = response.data;
        if (response.success) {

            for (var a = 0; a < resources.length; a++) {

                var currentSelected = $("#" + a + " option:selected").text();
                var newOptions = '<option id="' + $("#" + a + " option:selected").attr("id") + '" value="' + currentSelected + '">' + currentSelected + '</option>';

                for (var b = 0; b < baseOptions.length; b++) {
                    if (baseOptions[b].name != currentSelected) {
                        newOptions += '<option id="' + baseOptions[b].unit + '" value="' + baseOptions[b].name + '">' + baseOptions[b].name + '</option>';
                    }
                }

                $("#responseName-" + a + " .select-resource").remove();
                var newSelect =
                    '<select id="' + a + '" class="form-control select-resource" name="Recipes[' + a + '].ResourceName">\
                            '+ newOptions + '\
                            </select>';
                $("#responseName-" + a).append(newSelect);
            }
        }
    });
});




