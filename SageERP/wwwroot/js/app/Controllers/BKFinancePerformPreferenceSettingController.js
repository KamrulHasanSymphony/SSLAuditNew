var BKFinancePerformPreferenceSettingController = function (CommonService, BKFinancePerformPreferenceSettingService) {



    var init = function () {



        //if ($("#BKAuditOfficeTypeId").length) {
        if ($("#BKFinancePerformPreferenceSetting_BKAuditOfficeTypeId").length) {
            //LoadCombo("BKAuditOfficeTypeId", '/Common/BKAuditOfficeTypes');
            LoadCombo("BKFinancePerformPreferenceSetting_BKAuditOfficeTypeId", '/Common/BKAuditOfficeTypes');
        }
        if ($("#BKAuditOfficeId").length) {
            LoadCombo("BKAuditOfficeId", '/Common/BKAuditOffice');
        }


        $(".chkAll").click(function () {
            $('.dSelected:input:checkbox').not(this).prop('checked', this.checked);
        });

        var indexTable = BKFinancePerformPreferenceSettingTable();


        //$(".btnsave").click(function () {
        $(".btnsaveBKFinancePerformPreferenceSetting").click(function () {
            save();
        });


    }




    /*init end*/
    $('.Submit').click('click', function () {


        ReasonOfUnPost = $("#UnPostReason").val();

        var advances = serializeInputs("frm_Advances");

        advances["ReasonOfUnPost"] = ReasonOfUnPost;

        Confirmation("Are you sure? Do You Want to UnPost Data?", function (result) {
            if (ReasonOfUnPost === "" || ReasonOfUnPost === null) {
                ShowNotification(3, "Please Write down Reason Of UnPost");
                $("#ReasonOfUnPost").focus();
                return;
            }

            if (result) {

                advances.IDs = advances.Id;
                AdvancesService.AdvancesMultipleUnPost(advances, AdvancesMultipleUnPost, AdvancesMultipleUnPostFail);


            }
        });
    });



    function AdvancesMultipleUnPost(result) {
        console.log(result.message);

        if (result.status == "200") {
            ShowNotification(1, result.message);
            $("#IsPost").val('N');
            $("#divReasonOfUnPost").hide();
            $(".btnUnPost").hide();
            var dataTable = $('#ReceiptLists').DataTable();
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



    function AdvancesMultipleUnPostFail(result) {
        ShowNotification(3, "Something gone wrong");

        indexTable.draw();
    }



    $('.btnPost').click('click', function () {

        Confirmation("Are you sure? Do You Want to Post Data?", function (result) {
            console.log(result);
            if (result) {


                var advance = serializeInputs("frm_Advances");
                if (advance.IsPost == "Y") {
                    ShowNotification(3, "Data has already been Posted.");
                }
                else {
                    advance.IDs = advance.Id;
                    AdvancesService.AdvancesMultiplePost(advance, AdvancesMultiplePosts, AdvancesMultiplePostFail);
                }
            }
        });

    });



    $('#PostAD').on('click', function () {

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


        var dataTable = $('#AdvancesList').DataTable();

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

        if (IsPost) {
            AdvancesService.AdvancesMultiplePost(model, AdvancesMultiplePosts, AdvancesMultiplePostFail);
        }

    }

    function AdvancesMultiplePosts(result) {
        console.log(result.message);

        if (result.status == "200") {

            ShowNotification(1, result.message);

            $("#IsPost").val('Y');
            $(".btnUnPost").show();
            $(".btnPush").show();
            var dataTable = $('#AdvancesList').DataTable();
            dataTable.draw();

        }
        else if (result.status == "400") {
            ShowNotification(3, result.error);
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message);
        }
    }

    function AdvancesMultiplePostFail(result) {
        ShowNotification(3, "Something gone wrong");
        var dataTable = $('#AdvancesList').DataTable();
        dataTable.draw();

    }


    function save() {


        var validator = $("#frm_BKFinancePerformPreferenceSetting").validate();
        var BKFinancePerformPreferenceSetting = serializeInputs("frm_BKFinancePerformPreferenceSetting");
        var result = validator.form();


        var isChecked = $("#BKFinanceStatus").is(":checked");
        BKFinancePerformPreferenceSetting["BKFinancePerformPreferenceSetting.Status"] = isChecked.toString();


        isChecked = $("#FundAvailableFlag").is(":checked");
        BKFinancePerformPreferenceSetting["BKFinancePerformPreferenceSetting.FundAvailableFlag"] = isChecked.toString();

        var MisManagementClinentsFlag = $("#MisManagementClinentsFlag").is(":checked");
        BKFinancePerformPreferenceSetting["BKFinancePerformPreferenceSetting.MisManagementClinentsFlag"] = MisManagementClinentsFlag.toString();

        //var EfficienctyFlag = $("#EfficienctyFlag").is(":checked");
        //BKFinancePerformPreferenceSetting.EfficienctyFlag = EfficienctyFlag;

        var EfficienctyFlag = $("#EfficienctyFlag").is(":checked");
        BKFinancePerformPreferenceSetting["BKFinancePerformPreferenceSetting.EfficienctyFlag"] = EfficienctyFlag.toString();

        //var NPLSLargeFlag = $("#NPLSLargeFlag").is(":checked");
        //BKFinancePerformPreferenceSetting.NPLSLargeFlag = NPLSLargeFlag;

        var NPLSLargeFlag = $("#NPLSLargeFlag").is(":checked");
        BKFinancePerformPreferenceSetting["BKFinancePerformPreferenceSetting.NPLSLargeFlag"] = NPLSLargeFlag.toString();

        //var LargeTxnManageFlag = $("#LargeTxnManageFlag").is(":checked");
        //BKFinancePerformPreferenceSetting.NPLSLargeFlag = LargeTxnManageFlag;

        var LargeTxnManageFlag = $("#LargeTxnManageFlag").is(":checked");
        BKFinancePerformPreferenceSetting["BKFinancePerformPreferenceSetting.LargeTxnManageFlag"] = LargeTxnManageFlag.toString();

        //var HighValueAssetMangeFlag = $("#HighValueAssetMangeFlag").is(":checked");
        //BKFinancePerformPreferenceSetting.HighValueAssetMangeFlag = HighValueAssetMangeFlag;

        var HighValueAssetMangeFlag = $("#HighValueAssetMangeFlag").is(":checked");
        BKFinancePerformPreferenceSetting["BKFinancePerformPreferenceSetting.HighValueAssetMangeFlag"] = HighValueAssetMangeFlag.toString();

        //var BudgetMgtFlag = $("#BudgetMgtFlag").is(":checked");
        //BKFinancePerformPreferenceSetting.NPLSLargeFlag = BudgetMgtFlag;

        var BudgetMgtFlag = $("#BudgetMgtFlag").is(":checked");
        BKFinancePerformPreferenceSetting["BKFinancePerformPreferenceSetting.BudgetMgtFlag"] = BudgetMgtFlag.toString();


        if (!result) {
            validator.focusInvalid();
            return;
        }

        BKFinancePerformPreferenceSettingService.save(BKFinancePerformPreferenceSetting, saveDone, saveFail);

    }


    function saveDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnsaveBKFinancePerformPreferenceSetting").html('Update');
                //$("#Id").val(result.data.id);
                $("#BKFinancePerformPreferenceSetting_Id").val(result.data.id);
                $("#Code").val(result.data.code);
                result.data.operation = "update";
                $("#Operation").val(result.data.operation);
                $("#BKFinancePerformPreferenceSetting_Operation").val(result.data.operation);

            } else {
                ShowNotification(1, result.message);


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


    var BKFinancePerformPreferenceSettingTable = function () {

        var dataTable = $("#BKFinancePerformPreferenceSettingList").DataTable({
            orderCellsTop: true,
            fixedHeader: true,
            serverSide: true,
            "processing": true,
            "bProcessing": true,
            dom: 'lBfrtip',
            bRetrieve: true,
            searching: true,

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
                url: '/BKFinancePerformPreferenceSetting/_index',
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {
                            "indexsearch": $("#Branchs").val(),
                            "branchid": $("#BranchID").val(),
                            "code": $("#md-Code").val(),
                            "advanceAmount": $("#md-AdvanceAmount").val(),
                            "description": $("#md-Description").val(),
                            "ispost": $("#md-Post").val(),
                            "ponumber": $("#md-PONumber").val(),
                            "ispush": $("#md-Push").val(),
                            "fromDate": $("#FromDate").val(),
                            "toDate": $("#ToDate").val()
                        });
                }
            },
            columns: [
                {
                    data: "id",
                    render: function (data) {
                        var bid = $("#BranchID").val();
                        return "<a href='/BranchProfile/Edit/" + bid + "?commonId=" + data + "&status=FinancePerForm#tabs-3' class='edit btn btn-primary btn-sm' >" +
                            "<i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>";
                    },
                    "width": "9%",
                    "orderable": false
                },
                {
                    data: "code",
                    name: "Code"
                },
                {
                    data: "highValueAssetMangeFlag",
                    name: "HighValueAssetMangeFlag"
                },
                {
                    data: "budgetMgtFlag",
                    name: "BudgetMgtFlag"
                },
                {
                    data: "status",
                    name: "Status_3"
                }
            ]
        });

        return dataTable;
    }


    //var BKFinancePerformPreferenceSettingTable = function () {

    //    $('#BKFinancePerformPreferenceSettingList thead tr')
    //        .clone(true)
    //        .addClass('filters')
    //        .appendTo('#BKFinancePerformPreferenceSettingList thead');


    //    var dataTable = $("#BKFinancePerformPreferenceSettingList").DataTable({
    //        orderCellsTop: true,
    //        fixedHeader: true,
    //        serverSide: true,
    //        "processing": true,
    //        "bProcessing": true,
    //        dom: 'lBfrtip',
    //        bRetrieve: true,
    //        searching: false,


    //        buttons: [
    //            {
    //                extend: 'pdfHtml5',
    //                customize: function (doc) {
    //                    doc.content.splice(0, 0, {
    //                        text: ""
    //                    });
    //                },
    //                exportOptions: {
    //                    columns: [1, 2, 3, 4]
    //                }
    //            },
    //            {
    //                extend: 'copyHtml5',
    //                exportOptions: {
    //                    columns: [1, 2, 3, 4]
    //                }
    //            },
    //            {
    //                extend: 'excelHtml5',
    //                exportOptions: {
    //                    columns: [1, 2, 3, 4]
    //                }
    //            },
    //            'csvHtml5'
    //        ],


    //        ajax: {
    //            url: '/BKFinancePerformPreferenceSetting/_index',
    //            type: 'POST',
    //            data: function (payLoad) {
    //                return $.extend({},
    //                    payLoad,
    //                    {

    //                        "indexsearch": $("#Branchs").val(),
    //                        "branchid": $("#BranchID").val(),
    //                        "code": $("#md-Code").val(),
    //                        "advanceAmount": $("#md-AdvanceAmount").val(),
    //                        "description": $("#md-Description").val(),
    //                        "ispost": $("#md-Post").val(),
    //                        "ponumber": $("#md-PONumber").val(),
    //                        "ispush": $("#md-Push").val(),
    //                        "fromDate": $("#FromDate").val(),
    //                        "toDate": $("#ToDate").val()

    //                    });
    //            }
    //        },
    //        columns: [

    //            {
    //                data: "id",
    //                render: function (data) {


    //                    var bid = $("#BranchID").val();


    //                    return "<a href='/BranchProfile/Edit/" + bid + "?commonId=" + data + "&status=FinancePerForm' class='edit btn btn-primary btn-sm' >" +
    //                        "<i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>";


    //                },
    //                "width": "9%",
    //                "orderable": false
    //            },
    //            {
    //                data: "code",
    //                name: "Code"
    //            },
    //            {
    //                data: "highValueAssetMangeFlag",
    //                name: "HighValueAssetMangeFlag"
    //            }
    //            ,
    //            {
    //                data: "budgetMgtFlag",
    //                name: "BudgetMgtFlag"
    //            }
    //            ,
    //            {
    //                data: "status",
    //                name: "Status"
    //            }

    //        ]

    //    });


    //    if (dataTable.columns().eq(0)) {
    //        dataTable.columns().eq(0).each(function (colIdx) {

    //            var cell = $('.filters th').eq($(dataTable.column(colIdx).header()).index());

    //            var title = $(cell).text();


    //            if ($(cell).hasClass('action')) {
    //                $(cell).html('');

    //            } else if ($(cell).hasClass('bool')) {

    //                $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Y</option><option>N</option></select>');

    //            } else {
    //                $(cell).html('<input type="text" class="acc-filters filter-input"  placeholder="' +
    //                    title +
    //                    '"  id="md-' +
    //                    title.replace(/ /g, "") +
    //                    '"/>');
    //            }
    //        });
    //    }




    //    $("#BKFinancePerformPreferenceSettingList").on("change",
    //        ".acc-filters",
    //        function () {

    //            dataTable.draw();

    //        });
    //    $("#BKFinancePerformPreferenceSettingList").on("keyup",
    //        ".acc-filters",
    //        function () {

    //            dataTable.draw();

    //        });

    //    return dataTable;

    //}

    return {
        init: init
    }

}(CommonService, BKFinancePerformPreferenceSettingService);