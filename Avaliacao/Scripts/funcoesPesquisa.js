function tipoFiltro() {

    var valor = $("#pesquisaDropListId :selected").val();

    //-1 - Selecione uma opção
    if (valor != "-1") {

        $("#strCriterio").val("");
        var tipo = valor.substring(0, valor.indexOf("|"));
        $('#strCriterio').attr('type', tipo);

    } else {
        $('#strCriterio').attr('type', 'text');
        $("#strCriterio").val("");
    }
    $("#strCriterio").focus();

};


function Pesquisar() {


    var valor = $("#pesquisaDropListId :selected").val();

    //-1 - Selecione uma opção
    if (valor != "-1") {


        var criterio = valor + '|' + $("#strCriterio").val();

        $.ajax({

            //url: '@Url.Action("Lista")',
            url: 'Lista',
            data: { 'strCriterio': criterio },
            type: "post",
            cache: false,
            success: function (savingStatus) {

                var parcialView = $($.parseHTML(savingStatus)).find('#TabelaAjax')
                $("#TabelaAjax").html(parcialView);

                var paginacao = $($.parseHTML(savingStatus)).find('#paginacao')
                $("#paginacao").html(paginacao);

                var listapaginacao = $($.parseHTML(savingStatus)).find('#listapaginacao')
                $("#listapaginacao").html(listapaginacao);

            },
            error: function (xhr, err) {
                alert("readyState: " + xhr.readyState + "\nstatus: " + xhr.status);
                alert("responseText: " + xhr.responseText);
            }
        });


    } else {
        alert('Selecione um campo de pesquisa');
    }

};