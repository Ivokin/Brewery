$(function () {
    $("#add").on('click', function (e) {
        e.preventDefault();

        var table = $('.table');
        var rows = table.find('tr.data-row');
        var newItemIndex = rows.length;
        var rowsLenght = rows.length;
        var resources = [rowsLenght+1];
        var url = "/BrewsManagement/GetOptions/";
        var unit = '';

        for (var i = 0; i < rowsLenght; i++) {
            resources[i] = $(".select-resource#" + i + " option:selected").text();
        }

        var baseOptions;
        $.ajax({
            method: 'POST',
            url: url,
            data: {
                resources: resources
            },
        }).done(function (response) {
            if (response.success) {
                baseOptions = response.data;

                var options = [];
                for (var i = 0; i < baseOptions.length; i++) {
                    var option = baseOptions[i].Name;
                    if (!resources.includes(option)) {
                        if (i == 0) {
                            unit = baseOptions[i].unit
                            resources[rowsLenght] = baseOptions[i].name
                        }
                        options += '<option id="' + baseOptions[i].unit + '" value="' + baseOptions[i].name + '">' + baseOptions[i].name + '</option>';
                    }
                }

                for (var i = 0; i < rowsLenght; i++) {
                    for (var a = 0; a < resources.length; a++) {
                        if ($("#" + i + " option:selected").text() != resources[a]) {
                            $("#" + i + " option[value='" + resources[a] + "']").remove();                            
                        }
                    }
                }

                if (options.length > 0) {
                    table.append(
                        '<tr class="data-row" id="' + newItemIndex + '">\
                        <td class="noborder" id="responseName-' + newItemIndex + '">\
                            <select id="' + newItemIndex + '" class="form-control select-resource" name="Recipes[' + newItemIndex + '].ResourceName">\
                            '+ options + '\
                            </select>\
                        </td >\
                        <td class="noborder" id="responseDescription">\
                            <input class="form-control" type="text" id="Recipes_' + newItemIndex + '__Description" name="Recipes[' + newItemIndex + '].Description" value="">\
                        </td>\
                        <td class="noborder" id="responseValue">\
                            <input value="1" name="Recipes[' + newItemIndex + '].Amount" class="form-control input-group-text" type="text" data-val="true" data-val-number="The field Amount must be a number." data-val-required="The Amount field is required." id="Recipes_' + newItemIndex + '__Amount">\
                        </td>\
                        <td class= "noborder" id = "responseValue" >\
                            <div id="unit-'+ newItemIndex + '">' + unit + '</div>\
                            <input type="hidden" value="true" class="form-control" data-val="true" id="Recipes_'+ newItemIndex + '__IsNew" name="Recipes[' + newItemIndex + '].IsNew">\
                        </td>\
                        <td class="noborder">\
                            <button id="removeResource" type="button" class="btn btn-link removeresource" data-id="' + newItemIndex + '">Remove Resource</button>\
                        </td>\
                        </tr> '
                    );
                }
            }
        });
    });
})
