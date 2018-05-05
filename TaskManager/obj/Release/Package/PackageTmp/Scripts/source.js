function Init() {
    
    $('#myTab a').on('click',
        function(e) {
            e.preventDefault()
            $(this).tab('show')
        })

    $(document).ready(function() {
        var table = document.getElementById('upss');
        if (table != null) {
            for (i = 1; i < table.rows.length; i++) {
                for (j = 0; j < table.rows[i].cells.length-2; j++) {
                    table.rows[i].cells[j].onclick = onTDClick;
                }
            }
        }
    });

    /* START OF DEMO JS - NOT NEEDED */
    if (window.location == window.parent.location) {
        $('#fullscreen').html('<span class="glyphicon glyphicon-resize-small"></span>');
        $('#fullscreen').attr('href', 'http://bootsnipp.com/mouse0270/snippets/PbDb5');
        $('#fullscreen').attr('title', 'Back To Bootsnipp');
    }
    $('#fullscreen').on('click',
        function(event) {
            event.preventDefault();
            window.parent.location = $('#fullscreen').attr('href');
        });
    // $('#fullscreen').tooltip();
    /* END DEMO OF JS */

    $('.navbar-toggler').on('click',
        function(event) {
            event.preventDefault();
            $(this).closest('.navbar-minimal').toggleClass('open');
        })


    $(function() {
        $('agenda-date1').click(function(e) {
            //ловим элемент, по которому кликнули
            var t = e.target || e.srcElement;
            //получаем название тега
            var elm_name = t.tagName.toLowerCase();
            //если это инпут - ничего не делаем
            if (elm_name == 'input') {
                return false;
            }
            //document.getElementById('save_table').disabled = false;


            var val = $(this).find(".agenda-date1").html();
            if (val == undefined) {
                val = $(this).html();
            }
            val = $.trim(val);
            var style = 'style=" word-wrap: break-word; height:' +
                $(this)[0].getBoundingClientRect().height +
                'px; width:' +
                $(this)[0].getBoundingClientRect().width +
                'px;"';
            //style += 'style=" width:' + $(this)[0].getBoundingClientRect().width + 'px;"';
            var code = '<textarea type="textarea" id="edit" value="' + val + '" ' + style + '>' + val + '</textarea>';
            $(this).empty().append(code);
            $('#edit').focus();
            $('#edit').blur(function() {
                var val = $(this).val();
                var a = $(this).parent();
                $(this).parent().empty();
                a.html(val);
                a.css("width", "40%");
            });
        });

        $("#excel").click(function () {
            var cars = [];
            var a = $($(document.getElementById('month_table')).find("tbody")).find("tr");
            for (var i = 0; i < a.length; i++) {
                var plan = [];
                var result = [];
                var time = [];
                for (var j = 0; j < $($(a[i]).find("td")[1]).find("li").length; j++)
                    plan.push($.trim($($($(a[i]).find("td")[1]).find("li")[j]).text()));

                for (var j = 0; j < $($(a[i]).find("td")[1]).find("li").length; j++)
                    result.push($.trim($($($(a[i]).find("td")[2]).find("li")[j]).text()));

                for (var j = 0; j < $($(a[i]).find("td")[1]).find("li").length; j++)
                    time.push($.trim($($($(a[i]).find("td")[3]).find("li")[j]).text()));

                var w = {
                    PlanText: plan,
                    PlanResult:result,
                    Time: time,
                    Date: $.trim($($($(a[i]).find("td")[0]).find("div")[0]).text()) + " " + $.trim($($($(a[i]).find("td")[0]).find("div")[2]).text()),
                };
                cars.push(w);
            }
            $.ajax({
                type: "POST",
                url: "/Home/GetExcel",
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify(cars)
            });
       // }

           // $.post("/Home/GetExcel", JSON.stringify(cars), null, "json");
        });

        //if (($("#name_sort").find("span"))[0].classList.length == 0) {
        //    ($("#name_sort").find("span"))[0].classList.add('glyphicon');
        //    ($("#name_sort").find("span"))[0].classList.add('glyphicon-triangle-bottom');
        //}
        //$(".btn-xs").click(function() {
        //    var a = 1;
        //});
        $("#name_sort").click(function () {
            if (($(this).find("span")).hasClass("glyphicon glyphicon-triangle-bottom")) {
                document.location.href = "/Home/Sorting?attr=name&type=desc";
                ($(this).find("span"))[0].classList.remove('lyphicon-triangle-bottom');
                ($(this).find("span"))[0].classList.add('glyphicon-triangle-top');
            } else {
                document.location.href = "/Home/Sorting?attr=name&type=upp";
                ($(this).find("span"))[0].classList.remove('glyphicon-triangle-top');
               ($(this).find("span"))[0].classList.add('glyphicon-triangle-bottom');
            }
        })
        $('.12').click(function(e) {
            var a = 1;
        });
        $('.agenda-date').click(function(e) {
            $($(document.getElementsByClassName("modal-header")).find("h1")).text("");
                var rr = $($($($(this).parent()).find("td")[0]).find("div")[2]).text();
                var rrr = $($($($(this).parent()).find("td")[0]).find("div")[0]).text();

                $(document.getElementsByClassName("modal-header")).find("h1").append(rrr + " " + rr);
            
            //ловим элемент, по которому кликнули
            var t = e.target || e.srcElement;
            //получаем название тега
            var elm_name = t.tagName.toLowerCase();
            //если это инпут - ничего не делаем
            if (elm_name == 'input') {
                return false;
            }

            var dialog = document.getElementById('window');

            //var elements = $($(this).parent()[0])[0];

            //var trs = $($(this).parent()[0]).parent()[0];
            //var lis = $($($($($(this).parent()[0]).parent()[0])[0]).find("td")[1]).find("li");
            //var tbl = dialog.getElementsByTagName("tbody")[0];
            //var trs = tbl.rows;
            //var t = trs.length;
            //for (var i = 0; i < trs.length; i++) {
            //    tbl.deleteRow(i);

            //}
            $(dialog.getElementsByTagName("tbody")[0]).find('tr').remove();
            for (var i = 0; i < $($($(this).parent()[0]).find("td")[1]).find("li").length; i++) {
                var tbody = dialog.getElementsByTagName("tbody")[0];
                var row = document.createElement("TR");
                var td0 = document.createElement("TD");
                td0.appendChild(document.createTextNode(""));
                var td2 = document.createElement("TD");
                var td1 = document.createElement("TD");
                td0.appendChild(document.createTextNode(i + 1));
                td1.appendChild(document.createTextNode(
                    $.trim($($($($(this).parent()).find("td")[1]).find("li")[i]).text())));
                var td2 = document.createElement("TD");
                td2.appendChild(document.createTextNode(
                    $.trim($($($($(this).parent()).find("td")[2]).find("li")[i]).text())));
                var td3 = document.createElement("TD");
                td3.appendChild(document.createTextNode(
                    $.trim($($($($(this).parent()).find("td")[3]).find("li")[i]).text())));
                td1.classList.add("agenda-date1");
                td1.style.maxWidth = "400px";
                td2.classList.add("agenda-date1");
                td3.classList.add("eagenda-date");
                td2.style.maxWidth = "400px";
                td3.style.maxWidth = "400px";
                td2.style.width = "400px";
                td3.style.width = "400px";
                td0.style.width = "40px";
                td1.style.width = "400px";

                var td4 = document.createElement("TD");

                
                var a = document.createElement("a");
              
                td4.appendChild(document.createTextNode(
                    $.trim($($($($(this).parent()).find("td")[5]).find("li")[i]).text())));
                td4.style.width = "1px";
                td4.style.display = "none";
                row.appendChild(td0);
                row.appendChild(td1);
                row.appendChild(td2);
                row.appendChild(td3);
                row.appendChild(td4);
               
                tbody.appendChild(row);


            }
            var table = document.getElementById('tabke');
            if (table != null) {
                for (i = 1; i < table.rows.length; i++) {
                    for (j = 0; j < table.rows[i].cells.length-1; j++) {
                        table.rows[i].cells[j].onclick = onTDClick1;
                    }
                }
            }
            var a = $.trim($($($(this).parent()[0]).children()[1]).text());
            $(document.getElementById('PlanText')).val(a);
            var s = $.trim($($($(this).parent()[0]).children()[0]).text());
            $(document.getElementById('datedatepo')).text(s);
            $(dialog).css("display", "");
            dialog.show();


            //var val = $(this).find(".agenda-date").html();
            //if(val == undefined){
            //    val = $(this).html();
            //}
            //val = $.trim(val);
            //var style = 'style=" word-wrap: break-word; height:' + $(this)[0].getBoundingClientRect().height + 'px; width:' + $(this)[0].getBoundingClientRect().width + 'px;"';
            ////style += 'style=" width:' + $(this)[0].getBoundingClientRect().width + 'px;"';
            //var code = '<textarea type="textarea" id="edit" value="' + val + '" ' + style + '>'+val+'</textarea>';
            //$(this).empty().append(code);
            $('#edit').focus();
            $('#edit').blur(function() {
                var val = $(this).val();
                var a = $(this).parent();
                $(this).parent().empty();
                a.css("width", "40px");
            });
        });

    });


    var i = 1;
    $("#add_row").click(function() {
        var dialog = document.getElementById('window');
        var val = document.getElementById('tabke').rows.length;
        //if(val == undefined){
        var tbody = dialog.getElementsByTagName("tbody")[0];
        var row = document.createElement("TR");
        var td0 = document.createElement("TD");
        td0.appendChild(document.createTextNode(""));
        var td2 = document.createElement("TD");
        var td1 = document.createElement("TD");
        td0.appendChild(document.createTextNode(val));
        td1.appendChild(document.createTextNode(""));
        var td2 = document.createElement("TD");
        td2.appendChild(document.createTextNode(""));
        var td3 = document.createElement("TD");
        td3.appendChild(document.createTextNode(""));
        td1.classList.add("agenda-date1");
        td1.style.maxWidth = "200px";
        td1.style.fontSize = "17px";
        td2.classList.add("agenda-date1");
        td2.style.fontSize = "17px";
        td3.style.fontSize = "17px";
        td3.classList.add("eagenda-date");
        td2.style.maxWidth = "200px";
        td3.style.maxWidth = "200px";
        td2.style.width = "200px";
        td3.style.width = "200px";
        td0.style.width = "20px";
        td1.style.width = "200px";
        row.appendChild(td0);
        row.appendChild(td1);
        row.appendChild(td2);
        row.appendChild(td3);
       // row.appendChild(td5);
        tbody.appendChild(row);
        var table = document.getElementById('tabke');
        if (table != null) {
            for (i = 1; i < table.rows.length; i++) {
                for (j = 0; j < table.rows[i].cells.length-1; j++) {
                    table.rows[i].cells[j].onclick = onTDClick1;
                }
            }
        }
        //    val = $(this).html();
        //}
        //val = $.trim(val);
        //var style = 'style=" word-wrap: break-word; height:' + $(this)[0].getBoundingClientRect().height + 'px; width:' + $(this)[0].getBoundingClientRect().width + 'px;"';
        ////style += 'style=" width:' + $(this)[0].getBoundingClientRect().width + 'px;"';
        //var code = '<textarea type="textarea" id="edit" value="' + val + '" ' + style + '>'+val+'</textarea>';
        //$(this).empty().append(code);
        //$('#addr' + val).html('<td>' + (val + 1) + '</td><td class="agenda-date1"><td class="agenda-date1"><td/>');
        //$(document.getElementById('tabke')).append('<tr id="addr' + (val + 1) + '"></tr>');
        //i++;
    });
    $("#delete_row").click(function() {
        if (i > 1) {
            $("#addr" + (i - 1)).html('');
            i--;
        }
    });
    var s;
    $("#sup").click(function() {
        var a = $($(document.getElementById('upss')).find("tbody")).find("tr");
        for (var i = 0; i < a.length; i++) {
            //var e = document.getElementsByClassName("sups");
            //var strUser = e.options[e.selectedIndex].text;

            var elements = $($($($($(document.getElementById('upss')).find("tbody")[0]).children())[i])[0]).find("td");
            var w = {
                PlanText: $.trim($(elements[0]).text()),
                PlanResult: $.trim($(elements[1]).text()),
                Time: $.trim($(elements[2]).text()),
                Id: $.trim($(elements[4]).text()),
                Date: ($.trim($(document.getElementById("str")).text()).split(' ')[1]),
                Tag: $.trim($($(elements[3]).find("select option:selected")).val())
        };
            var person = { Name: "Sa" };
            $.post("/Home/People", w, null, "json").done(function(data) {

                //window.location.href = "/Home/WeekSchedule?date=" + Day + "-" + Month + "-" + Year;
                location.reload();
            });
        }

        document.getElementById('sup').disabled = true;
    });
    function successGetData(data) {
               var tbody = document.getElementById('upss').getElementsByTagName("TBODY")[0];
               var row = document.createElement("TR");
               row.classList.add("lavender");

               var td1 = document.createElement("TD");
               td1.appendChild(document.createTextNode(""));
               var td2 = document.createElement("TD");
               td2.appendChild(document.createTextNode(""));
               var td3 = document.createElement("TD");
               td3.appendChild(document.createTextNode(""));
               td1.classList.add("ert");
               td1.style.maxWidth = "200px";
               td2.classList.add("ert");
               td3.classList.add("ert");
               td2.style.maxWidth = "200px";
               td3.style.maxWidth = "200px";
               td2.style.width = "200px";

               td1.style.width = "200px";


        //       <div class="form-group">

        //       <select class="form-control" id="exampleFormControlSelect1">
        //    <option>1</option>
        //    <option>2</option>
        //    <option>3</option>
        //    <option>4</option>
        //    <option>5</option>
        //  </select>
        //</div>


               var a = document.createElement('div');
               a.class = "form-group form-control-lg";
               var select = document.createElement('select');

               a.appendChild(select);
               select.classList.add = "form-group form-control-lg";
               select.style.width = "100%";
        select.style.fontSize = "17px";
        select.style.height = "40px";
        var optionEmpty = document.createElement('option');
        optionEmpty.text = "";
        select.appendChild(optionEmpty);
        for (var i = 0; i < data.length; i++) {
            var option = document.createElement('option');
            option.text = data[i];
            select.appendChild(option);
        }
        var td4 = document.createElement("TD");
        var td5 = document.createElement("TD");
               td4.appendChild(a);
               td4.style.width = "50px";
               td4.style.maxWidth = "50px";
        td5.style.display = "none";
               row.appendChild(td1);
               row.appendChild(td2);
               row.appendChild(td3);
               row.appendChild(td4);
               row.appendChild(td5);
               tbody.appendChild(row);
               var table = document.getElementById('upss');
               for (i = 1; i < table.rows.length; i++) {
                   for (j = 0; j < table.rows[i].cells.length-2; j++) {
                       table.rows[i].cells[j].onclick = onTDClick;
                   }
               }

    }


    $("#ups").click(function () {
        jQuery.ajax({
            type: "POST",
            url: "/Home/GetTag",
            success: successGetData
        });
       
    });
 
    
    function onTDClick() {
        if ($(this).find("textarea").length == 0) {
            document.getElementById("sup").disabled = false;
            var val = $(this).find("ert").html();
            if (val == undefined) {
                val = $(this).text();
            }
            val = $.trim(val);
            var style = 'style=" white-space :pre-wrap;font-size:17px; height:' +
                $(this).height() +
                'px; width:' +
                '100%;"';
            //style += 'style=" width:' + $(this)[0].getBoundingClientRect().width + 'px;"';
            var code = '<textarea type="textarea" id="edit" value="' + val + '" ' + style + '>' + val + '</textarea>';

            $(this).empty().append(code);
            $('#edit').focus();
        }
        $('#edit').blur(function() {
            var val = $(this).val();
            var a = $(this).parent();
            $(this).parent().empty();
            a.html(val);
            //a.css("width", "40%");
        });
    }

    function onTDClick1() {
        if ($(this).find("textarea").length == 0) {
            document.getElementById("save_table").disabled = false;
            var val = $(this).find("ert").html();
            if (val == undefined) {
                val = $(this).text();
            }
            val = $.trim(val);
            var style = 'style=" word-wrap: break-word; height:' +
                $(this).height() +
                'px; width:' +
                '100%;"';
            //style += 'style=" width:' + $(this)[0].getBoundingClientRect().width + 'px;"';
            var code = '<textarea type="textarea" id="edit" value="' + val + '" ' + style + '>' + val + '</textarea>';

            $(this).empty().append(code);
            $('#edit').focus();
        }
        $('#edit').blur(function() {
            var val = $(this).val();
            var a = $(this).parent();
            $(this).parent().empty();
            a.html(val);
            //a.css("width", "40%");
        });
    }

    $("#close").click(function() {
        // $(document.getElementById('window')).hide();
        $(document.getElementById('window'))[0].removeAttribute("open");
        $(document.getElementById('window')).hide();

    });


    //$("#save_table").click(function () {
    //    $(document.getElementById('window')).hide();
    //    var elements = $($($($($(document.getElementById('tabke')).find("tbody")[0]).children())[0])[0]).find("td");
    //    var person = { Name: $.trim($(elements[1]).text()), S: 1 };
    //    var w = { PlanText: $.trim($(elements[1]).text()), PlanResult: $.trim($(elements[2]).text()), Time: $.trim($(elements[3]).text()) };
    //    // $.getJSON("/Home/People", "fd", "hj");

    //    //var person = { Name: name };
    //    $.getJSON("/Home/People", w, null, "json");
    //});
    $("#save_table").click(function() {
        var a = $($(document.getElementById('tabke')).find("tbody")).find("tr");
        var b = $($(document.getElementsByClassName("modal-header")).find("h1")).text().split(' ');
        document.getElementById('save_table').disabled = true;
        for (var i = 0; i < a.length; i++) {

            var elements = $($($($($(document.getElementById('tabke')).find("tbody")[0]).children())[i])[0]).find("td");
            var w = {
                PlanText: $(elements[1]).text().replace(/^\s*(.*)\s*$/, '$1'),
                PlanResult: $(elements[2]).text().replace(/^\s*(.*)\s*$/, '$1'),
                Time: $.trim($(elements[3]).text()),
                Id: $.trim($(elements[4]).text()),
                Day: b[0],
                Month: b[1],
                Year: b[2],
            };
            var person = { Name: "Sa" };
            $.post("/Home/People1", w, null, "json").done(function(data) {

                //window.location.href = "/Home/WeekSchedule?date=" + Day + "-" + Month + "-" + Year;
                location.reload();
            });
        }
        $(document.getElementById('window'))[0].removeAttribute("open");
        $(document.getElementById('window')).hide();
        //window.location.href = "/Home/WeekSchedule";
    });

    //rr$("#add_row").on("click", function () {
    //    // Dynamic Rows Code

    //    // Get max row id and set new id
    //    var newid = 0;
    //    $.each($("#tabke tr"), function () {
    //        if (parseInt($(this).data("id")) > newid) {
    //            newid = parseInt($(this).data("id"));
    //        }
    //    });
    //newid++;

    //var tr = $("<tr></tr>", {
    //});

    //// loop through each td and create new elements with name of newid
    //$.each($("#tab_logic tbody tr:nth(0) td"), function () {
    //    var cur_td = $(this);

    //    var children = cur_td.children();

    //    // add new td and element if it has a nane
    //    if ($(this).data("name") != undefined) {
    //        var td = $("<td></td>", {
    //            "data-name": $(cur_td).data("name")
    //        });

    //        var c = $(cur_td).find($(children[0]).prop('tagName')).clone().val("");
    //        c.attr("name", $(cur_td).data("name") + newid);
    //        c.appendTo($(td));
    //        td.appendTo($(tr));
    //    } else {
    //        var td = $("<td></td>", {
    //            'text': $('#tab_logic tr').length
    //        }).appendTo($(tr));
    //    }
    //});

    // add delete button and td
    /*
    $("<td></td>").append(
        $("<button class='btn btn-danger glyphicon glyphicon-remove row-remove'></button>")
            .click(function() {
                $(this).closest("tr").remove();
            })
    ).appendTo($(tr));
    */

    // add the new row
    //    $(tr).appendTo($('#tab_logic'));

    //    $(tr).find("td button.row-remove").on("click", function () {
    //        $(this).closest("tr").remove();
    //    });
    //});


    // Sortable Code
    var fixHelperModified = function(e, tr) {
        var $originals = tr.children();
        var $helper = tr.clone();

        $helper.children().each(function(index) {
            $(this).width($originals.eq(index).width());
        });

        return $helper;
    };

    $("#back").click(function() {
        var a = ($.trim($($($(this).parent()).find("h1")).text()).split(' ')[1]).split('.');
        var w = { Day: a[0], Month: a[1], Year: a[2] };
        $.post("/Home/TodaySchedule", w, null, "json");

    });

    $('#delete_search').click(function() {
        location.reload();
    });

}


// $("#add_row").trigger("click");

//    $("#button_search").click(function() {
//        var a = $("#text_search").val();
//        var w = { Text: a };
//        window.location.href = '/Home/SearchResult/' + a;
//       // $.post("/Home/SearchResult", w, null, "json");
//    });

//}

