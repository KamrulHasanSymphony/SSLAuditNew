var AuditPointsController = function (CommonService, AuditPointsService) {

    var indexTable;

    var pointTable;


    var init = function () {

        debugger;

        var pId = $("#PIdData").val();
        var id = $("#Id").val();

        if ($("#PId").length) {
            LoadCombo("PId", '/Common/AuditPointType?pId=' + id, true);
        }


        $(".chkAll").click(function () {
            $('.dSelected:input:checkbox').not(this).prop('checked', this.checked);
        });

        indexTable = AuditPointTable();

        $(".btnsave").click(function () {
            debugger;
            save();
        });



        //Add

        //if ($("#AuditPointList").length) {
        //    debugger;           
        //    var tablePointConfigs = getPointTableConfig()
        //    pointTable = $("#AuditPointList").DataTable(tablePointConfigs);
        //}

        //$("#addAuditPointsdata").on("click", function (e) {
        //    AuditPointsService.auditPointModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAddPoint(result, pointTable) }, null, (res) => console.log(res), () => { pointTable.draw() });
        //})

        //$("#AuditPointList").on("click", ".js-edit", function () {
        //    rowEdit($(this).data('id'), $("#Edit").val());
        //});

        //End

    }

    /*init end*/


    //Add Point


    //function rowEdit(id, Edit) {
    //    AuditPointsService.auditAreaModal({ Id: id, Edit }, (result) => { onAddPoint(result, detailTable) }, null, null, () => { detailTable.draw() });
    //}

    //function onAddPoint(result, detailTable) {
    //    debugger;
    //    var auditPoint = serializeInputs("AuditAreas");
    //    AuditPointsService.save(auditPoint, saveDone, saveFail);
    //}


    //End


    $("#indexSearch").click(function () {
        debugger;

        //var data = $("#Branchs").val();

        var PId = $("#PId").val();
        var P_Level = $("#P_Level").val();

        //if (data == "xx") {
        //    ShowNotification(3, "Please Select Branch Type First");
        //    return;
        //}
        //if (!fromDate || !toDate) {
        //    ShowNotification(3, "Please Select both From Date and To Date");
        //    return;
        //}

        indexTable.draw();
       

    });



    $("#PId").on("change", function (e) {
        if (e.originalEvent) { // Ensures event was triggered by user, not by JavaScript
            debugger;
            var empDropdown = document.getElementById("PId");
            var Id = document.getElementById("Id");
            var selectedEmpId = empDropdown.value;
            var selectedText = $("#PId option:selected").text();

            var commonVM = {
                PId: document.getElementById("PId").value,
                Id: document.getElementById("Id").value,
                Operation: document.getElementById("Operation").value,
                AuditType: selectedText
            };

            $.ajax({
                type: "POST",
                url: '/AuditPoints/GetAuditPointLevel',
                contentType: 'application/json',
                data: JSON.stringify(commonVM),
                success: function (result) {
                    debugger;
                    if (result && result.length > 0) {

                        var data = result[0].p_Level;
                        $("#P_Level").val(data);
                        $("#AuditTypeId").val(result[0].auditTypeId);

                    }
                },
                error: function () {
                    console.error('Error occurred while fetching data.');
                }
            });
        }
    });



    

    
    //Add

    function getPointIndexURL() {
        return '/AuditPoints/_index/?id=' + $("#Id").val()
    }

    var getPointTableConfig = function () {

        $('#AuditPointList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#AuditPointList thead');

        var id = $("#Id").val();
        var counter = 1;
        var databaseRowCount = 100;
        var edit = $("#Edit").val();

        var dataTable = $("#AuditPointList").DataTable({
            orderCellsTop: true,
            fixedHeader: true,
            serverSide: true,
            "processing": true,
            "bProcessing": true,
            dom: 'lBfrtip',
            bRetrieve: true,
            searching: false,


            buttons: [
                {
                    extend: 'pdfHtml5',
                    customize: function (doc) {
                        doc.content.splice(0, 0, {
                            text: ""
                        });
                    },
                    exportOptions: {
                        columns: [1, 2, 3, 4]

                    }
                },
                {
                    extend: 'copyHtml5',
                    exportOptions: {
                        columns: [1, 2, 3, 4]

                    }
                },
                {
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                'csvHtml5'
            ],


            ajax: {
                url: getPointIndexURL(),
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {

                            "issuename": $("#md-IssueName").val(),
                            "issuepriority": $("#md-IssuePriority").val(),
                            "dateofsubmission": $("#md-DateOfSubmission").val(),
                            "enddate": $("#md-EndDate").val(),
                            "ispost": $("#md-Post").val(),

                            "PId": $("#PId").val(),
                            "P_Level": $("#P_Level").val()

                        });
                }
            },

            columns: [

                {
                    data: "id",
                    render: function (data) {

                        return "<a href=/AuditPoints/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt  ' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>  "
                            ;

                    },
                    "width": "9%",
                    "orderable": false
                },
                {
                    data: "auditType",
                    name: "AuditType"

                }
                ,
                {
                    data: "weightPersent",
                    name: "WeightPersent"

                }
                ,
                {
                    data: "p_Level",
                    name: "P_Level"

                }

            ],

            order: [1, "desc"],

        });

        if (dataTable.columns().eq(0)) {
            dataTable.columns().eq(0).each(function (colIdx) {

                var cell = $('.filters th').eq($(dataTable.column(colIdx).header()).index());

                var title = $(cell).text();


                if ($(cell).hasClass('action')) {
                    $(cell).html('');

                }
                else if ($(cell).hasClass('bool')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Y</option><option>N</option></select>');

                }
                else if ($(cell).hasClass('private')) {

                    $(cell).html('');

                }

                else {
                    $(cell).html('<input type="text" class="acc-filters filter-input"  placeholder="' +
                        title +
                        '"  id="md-' +
                        title.replace(/ /g, "") +
                        '"/>');
                }
            });
        }


        $("#AuditPointList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();
            });

        $("#AuditPointList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;
    }


    //End


    var AuditPointTable = function () {

        debugger;

        $('#AuditPointList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#AuditPointList thead');


        var dataTable = $("#AuditPointList").DataTable({
            orderCellsTop: true,
            fixedHeader: true,
            serverSide: true,
            "processing": true,
            "bProcessing": true,
            dom: 'lBfrtip',
            bRetrieve: true,
            searching: false,


            buttons: [
                {
                    extend: 'pdfHtml5',
                    customize: function (doc) {
                        doc.content.splice(0, 0, {
                            text: ""
                        });
                    },
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'copyHtml5',
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                'csvHtml5'
            ],


            ajax: {
                url: '/AuditPoints/_index/',
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {

                            "indexsearch": $("#Branchs").val(),
                            "branchid": $("#CurrentBranchId").val(),

                            "auditType": $("#md-AuditType").val(),
                            "weightPersent": $("#md-WeightPersent").val(),
                            "level": $("#md-Level").val(),

                            "ispost": $("#md-Post").val(),
                            "ponumber": $("#md-PONumber").val(),
                            "ispost": $("#md-Post").val(),
                            "ispush": $("#md-Push").val(),

                            "PId": $("#PId").val(),
                            "P_Level": $("#P_Level").val()

                        });
                }
            },
            columns: [



                {
                    data: "id",
                    render: function (data) {

                        return "<a href=/AuditPoints/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt  ' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>  "
                            ;

                    },
                    "width": "9%",
                    "orderable": false
                },
                {
                    data: "auditType",
                    name: "AuditType"

                }
                ,
                {
                    data: "weightPersent",
                    name: "WeightPersent"

                }
                ,
                {
                    data: "p_Level",
                    name: "P_Level"

                }
            ]

        });


        if (dataTable.columns().eq(0)) {
            dataTable.columns().eq(0).each(function (colIdx) {

                var cell = $('.filters th').eq($(dataTable.column(colIdx).header()).index());

                var title = $(cell).text();


                if ($(cell).hasClass('action')) {
                    $(cell).html('');

                } else if ($(cell).hasClass('bool')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Y</option><option>N</option></select>');

                } else {
                    $(cell).html('<input type="text" class="acc-filters filter-input"  placeholder="' +
                        title +
                        '"  id="md-' +
                        title.replace(/ /g, "") +
                        '"/>');
                }
            });
        }




        $("#AuditPointList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#AuditPointList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }


    $('.Submit').click('click', function () {


        ReasonOfUnPost = $("#UnPostReason").val();

        var tours = serializeInputs("frm_Tours");

        tours["ReasonOfUnPost"] = ReasonOfUnPost;

        Confirmation("Are you sure? Do You Want to UnPost Data?", function (result) {
            if (ReasonOfUnPost === "" || ReasonOfUnPost === null) {
                ShowNotification(3, "Please Write down Reason Of UnPost");
                $("#ReasonOfUnPost").focus();
                return;
            }

            if (result) {


                tours.IDs = tours.Id;
                ToursService.ToursMultipleUnPost(tours, ToursMultipleUnPost, ToursMultipleUnPostFail);


            }
        });
    });

    function ToursMultipleUnPost(result) {
        console.log(result.message);

        if (result.status == "200") {
            ShowNotification(1, result.message);
            $("#IsPost").val('N');
            //Visibility(false);
            $("#divReasonOfUnPost").hide();
            $(".btnUnPost").hide();
            $(".btnReject").hide();
            $(".btnApproved").hide();

            var dataTable = $('#ToursList').DataTable();

            dataTable.draw();


            $('#modal-default').modal('hide');


        }
        else if (result.status == "400") {
            ShowNotification(3, result.message); // <-- display the error message here
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message); // <-- display the error message here
        }
    }

    function ToursMultipleUnPostFail(result) {
        ShowNotification(3, "Something gone wrong");
        var dataTable = $('#ToursList').DataTable();

        dataTable.draw();
    }

    $('.btnPost').click('click', function () {

        Confirmation("Are you sure? Do You Want to Post Data?", function (result) {
            console.log(result);
            if (result) {


                var tours = serializeInputs("frm_Tours");
                if (tours.IsPost == "Y") {
                    ShowNotification(3, "Data has already been Posted.");
                }
                else {
                    tours.IDs = tours.Id;
                    ToursService.ToursMultiplePost(tours, ToursMultiplePosts, ToursMultiplePostFail);
                }
            }
        });

    });





    $('#PostTR').on('click', function () {

        Confirmation("Are you sure? Do You Want to Post Data?", function (result) {
            console.log(result);
            if (result) {

                SelectData(true);
            }
        });

    });

    function SelectData(IsPost) {

        var IDs = [];
        var $Items = $(".dSelected:input:checkbox:checked");

        if ($Items == null || $Items.length == 0) {
            ShowNotification(3, "You are requested to Select checkbox!");
            return;
        }

        $Items.each(function () {
            var ID = $(this).attr("data-Id");
            IDs.push(ID);
        });

        var model = {
            IDs: IDs,

        }


        var dataTable = $('#ToursList').DataTable();

        var rowData = dataTable.rows().data().toArray();
        var filteredData = [];
        var filteredData1 = [];
        if (IsPost) {
            filteredData = rowData.filter(x => x.isPost === "Y" && IDs.includes(x.id.toString()));

        }
        else {
            filteredData = rowData.filter(x => x.isPush === "Y" && IDs.includes(x.id.toString()));
            filteredData1 = rowData.filter(x => x.isPost === "N" && IDs.includes(x.id.toString()));
        }

        if (IsPost) {
            if (filteredData.length > 0) {
                ShowNotification(3, "Data has already been Posted.");
                return;
            }

        }

        //else {
        //    if (filteredData.length > 0) {


        //        ShowNotification(3, "Data has already been Pushed.");
        //        return;

        //    }
        //    if (filteredData1.length > 0) {
        //        ShowNotification(3, "Please Data Post First!");

        //        return;
        //    }
        //}


        if (IsPost) {
            ToursService.ToursMultiplePost(model, ToursMultiplePosts, ToursMultiplePostFail);
        }

        //else {
        //    ICReceiptsService.ICRMultiplePush(model, ICRMultiplePush, ICRMultiplePushFail);

        //}

    }



    function ToursMultiplePosts(result) {
        console.log(result.message);

        if (result.status == "200") {

            ShowNotification(1, result.message);

            $("#IsPost").val('Y');

            $(".btnUnPost").show();
            $(".btnReject").show();
            $(".btnApproved").show();


            $(".btnPush").show();

            //Visibility(true);



            var dataTable = $('#ToursList').DataTable();
            dataTable.draw();




        }
        else if (result.status == "400") {
            ShowNotification(3, result.message);
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message);
        }
    }

    function ToursMultiplePostFail(result) {
        ShowNotification(3, "Something gone wrong");
        var dataTable = $('#ToursList').DataTable();
        dataTable.draw();

    }






    function save() {

        debugger;

        var validator = $("#frm_AuditPoint").validate();
        var auditPoint = serializeInputs("frm_AuditPoint");

        var PId = $("#PId").val();
        //var value = $("#PId option:contains('----- Select -----')").val();

        if (auditPoint.WeightPersent > 100) {
            ShowNotification(3, "Weight Persent is over 100");
            return;
        }

        if (PId == "xx") {
            ShowNotification(3, "Please Select Correct Option");
            return;
        }

        var result = validator.form();

        if (!result) {
            validator.focusInvalid();
            return;
        }

        AuditPointsService.save(auditPoint, saveDone, saveFail);

    }


    function saveDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {


                ShowNotification(1, result.message);
                $(".btnsave").html('Update');

                $(".btnSave").addClass('sslUpdate');

                $("#Id").val(result.data.id);
                $("#Code").val(result.data.code);

                $("#divUpdate").show();
                //change
                //$("#btnPost").show();
                //end
                $("#divSave").hide();
                $("#SavePost").show();


                result.data.operation = "update";
                $("#Operation").val(result.data.operation);


                //$('#modal-default').modal('hide');


            } else {
                ShowNotification(1, result.message);

                $("#divSave").hide();

                $("#divUpdate").show();


            }
        }
        else if (result.status == "400") {
            ShowNotification(3, result.message || result.error);
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message || result.error);
        }
    }


    function saveFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }


    function btnSaleSave() {

        debugger;

        var validator = $("#frm_Integration").validate();
        var integration = serializeInputs("frm_Integration");

        var result = validator.form();

        if (!result) {
            validator.focusInvalid();
            return;
        }

        ToursService.SaleSave(integration, saveSaleDone, saveSaleFail);

    }


    function saveSaleDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);

            } else {
                ShowNotification(1, result.message);

                $("#divSave").hide();

                $("#divUpdate").show();

            }
        }
        else if (result.status == "400") {
            ShowNotification(3, result.message || result.error);
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message || result.error);
        }
    }

    function saveSaleFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }





    function btnPurchaseSave() {

        debugger;

        var validator = $("#frm_Integration").validate();
        var integration = serializeInputs("frm_Integration");

        var result = validator.form();

        if (!result) {
            validator.focusInvalid();
            return;
        }

        ToursService.PurchaseSave(integration, savePurchaseDone, savePurchaseFail);

    }


    function savePurchaseDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);

            } else {
                ShowNotification(1, result.message);

                $("#divSave").hide();

                $("#divUpdate").show();

            }
        }
        else if (result.status == "400") {
            ShowNotification(3, result.message || result.error);
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message || result.error);
        }
    }

    function savePurchaseFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }



    return {
        init: init
    }


}(CommonService, AuditPointsService);