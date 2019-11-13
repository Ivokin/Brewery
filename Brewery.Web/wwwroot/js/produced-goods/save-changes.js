$(function () {
    $(".save").on('click', function (e) {
        e.preventDefault();
        var i = this.id;
        var id = this.attributes.value.nodeValue;
        var amount = $("#amount-" + i).val();
        var url = "ProducedGoods/Create/";

        if (amount < 0) {

        }
        $.ajax({
            method: 'POST',
            url: url,
            data: {
                id: id,
                amount: amount
            }
        }).done(function (response) {
            if (response.success) {
                var producedGoods = response.producedGoods;
                for (var i = 0; i < producedGoods.length; i++) {
                    var valueId = producedGoods[i].brewId;
                    var valueAvaible = producedGoods[i].avaible;
                    $(".avaible#" + valueId + "").text(valueAvaible);
                    if (valueAvaible > 0)
                    {
                        $("button.save[value=" + id + "]").attr('class', 'save btn btn-success');
                    }
                    else
                    {
                        $("button.save[value=" + id + "]").attr('class', 'save btn btn-secondary');
                    }
                }
                $("input[type=number]").val("");
            }
            else {
                alert('faild')
            }
        });
    });
});