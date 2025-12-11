var BKCommonSelectionSettingController = function (CommonService, BKCommonSelectionSettingService) {



    var init = function () {



        //if ($("#BKAuditOfficeTypeId").length) {
        if ($("#BKCommonSelectionSetting_BKAuditOfficeTypeId").length) {
            //LoadCombo("BKAuditOfficeTypeId", '/Common/BKAuditOfficeTypes');
            LoadCombo("BKCommonSelectionSetting_BKAuditOfficeTypeId", '/Common/BKAuditOfficeTypes');
        }
        if ($("#BKAuditOfficeId").length) {
            LoadCombo("BKAuditOfficeId", '/Common/BKAuditOffice');
        }


        $(".chkAll").click(function () {
            $('.dSelected:input:checkbox').not(this).prop('checked', this.checked);
        });

        var indexTable = BKCommonSelectionSettingTable();


        //$(".btnsave").click(function () {
        $(".btnsaveBKCommonSelectionSetting").click(function () {
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

        debugger;
        var validator = $("#frm_BKCommonSelectionSetting").validate();
        var bkCommonSelectionSetting = serializeInputs("frm_BKCommonSelectionSetting");
        var result = validator.form();

        var isChecked = $("#BKCommonStatus").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Status"] = isChecked.toString();

        var HitoricalPreformFlag = $("#HitoricalPreformFlag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.HitoricalPreformFlag"] = HitoricalPreformFlag.toString();

        var LastYearAuditFindingFlag = $("#LastYearAuditFindingFlag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.LastYearAuditFindingFlag"] = LastYearAuditFindingFlag.toString();

        var PreviousYearExceptLastYearAuditFindingFlag = $("#PreviousYearExceptLastYearAuditFindingFlag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.PreviousYearExceptLastYearAuditFindingFlag"] = PreviousYearExceptLastYearAuditFindingFlag.toString();

        var TechCyberRiskFlag = $("#TechCyberRiskFlag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.TechCyberRiskFlag"] = TechCyberRiskFlag.toString();

        var OfficeSizeFlag = $("#OfficeSizeFlag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.OfficeSizeFlag"] = OfficeSizeFlag.toString();

        var OfficeSignificanceFlag = $("#OfficeSignificanceFlag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.OfficeSignificanceFlag"] = OfficeSignificanceFlag.toString();

        var StaffTurnoverFlag = $("#StaffTurnoverFlag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.StaffTurnoverFlag"] = StaffTurnoverFlag.toString();

        var StaffTrainingGapsFlag = $("#StaffTrainingGapsFlag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.StaffTrainingGapsFlag"] = StaffTrainingGapsFlag.toString();

        var StrategicInitiativeFlagveFlag = $("#StrategicInitiativeFlagveFlag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.StrategicInitiativeFlagveFlag"] = StrategicInitiativeFlagveFlag.toString();

        var OperationalCompFlag = $("#OperationalCompFlag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.OperationalCompFlag"] = OperationalCompFlag.toString();


        var Fields1Flag = $("#Fields1Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields1Flag"] = Fields1Flag.toString();

        var Fields2Flag = $("#Fields2Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields2Flag"] = Fields2Flag.toString();


        var Fields3Flag = $("#Fields3Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields3Flag"] = Fields3Flag.toString();


        var Fields4Flag = $("#Fields4Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields4Flag"] = Fields4Flag.toString();

        var Fields5Flag = $("#Fields5Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields5Flag"] = Fields5Flag.toString();

        var Fields6Flag = $("#Fields6Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields6Flag"] = Fields6Flag.toString();

        var Fields7Flag = $("#Fields7Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields7Flag"] = Fields7Flag.toString();

        var Fields8Flag = $("#Fields8Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields8Flag"] = Fields8Flag.toString();

        var Fields9Flag = $("#Fields9Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields9Flag"] = Fields9Flag.toString();

        var Fields10Flag = $("#Fields10Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields10Flag"] = Fields10Flag.toString();


        var Fields11Flag = $("#Fields11Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields11Flag"] = Fields11Flag.toString();

        var Fields12Flag = $("#Fields12Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields12Flag"] = Fields12Flag.toString();

        var Fields13Flag = $("#Fields13Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields13Flag"] = Fields13Flag.toString();

        var Fields14Flag = $("#Fields14Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields14Flag"] = Fields14Flag.toString();

        var Fields15Flag = $("#Fields15Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields15Flag"] = Fields15Flag.toString();

        var Fields16Flag = $("#Fields16Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields16Flag"] = Fields16Flag.toString();

        var Fields17Flag = $("#Fields17Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields17Flag"] = Fields17Flag.toString();

        var Fields18Flag = $("#Fields18Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields18Flag"] = Fields18Flag.toString();

        var Fields19Flag = $("#Fields19Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields19Flag"] = Fields19Flag.toString();

        var Fields20Flag = $("#Fields20Flag").is(":checked");
        bkCommonSelectionSetting["BKCommonSelectionSetting.Fields20Flag"] = Fields20Flag.toString();




        if (!result) {
            validator.focusInvalid();
            return;
        }

        BKCommonSelectionSettingService.save(bkCommonSelectionSetting, saveDone, saveFail);

    }


    function saveDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                //$(".btnsave").html('Update');
                $(".btnsaveBKCommonSelectionSetting").html('Update');
                //$("#Id").val(result.data.id);
                $("#BKCommonSelectionSetting_Id").val(result.data.id);
                $("#Code").val(result.data.code);
                result.data.operation = "update";
                //$("#Operation").val(result.data.operation);
                $("#BKCommonSelectionSetting_Operation").val(result.data.operation);


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


    var BKCommonSelectionSettingTable = function () {

        var dataTable = $("#BKCommonSelectionSettingList").DataTable({
            orderCellsTop: true,
            fixedHeader: true,
            serverSide: true,
            "processing": true,
            "bProcessing": true,
            dom: 'lBfrtip',
            bRetrieve: true,
            searching: true, // Disables search functionality

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
                url: '/BKCommonSelectionSetting/_index',
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({}, payLoad, {
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
                        return "<a href='/BranchProfile/Edit/" + bid + "?commonId=" + data + "&status=CommonSelection#tabs-8' class='edit btn btn-primary btn-sm'>" +
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
                    data: "historicalPerformFlagDesc",
                    name: "HistoricalPerformFlagDesc"
                },
                {
                    data: "lastYearAuditFindingFlagDesc",
                    name: "LastYearAuditFindingFlagDesc"
                },
                {
                    data: "techCyberRiskFlagDesc",
                    name: "TechCyberRiskFlagDesc"
                },
                {
                    data: "status",
                    name: "Status"
                }
            ]
        });

        return dataTable;
    }




    //var BKCommonSelectionSettingTable = function () {
    //    $('#BKCommonSelectionSettingList thead tr')
    //        .clone(true)
    //        .addClass('filters')
    //        .appendTo('#BKCommonSelectionSettingList thead');

    //    var dataTable = $("#BKCommonSelectionSettingList").DataTable({
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
    //            url: '/BKCommonSelectionSetting/_index',
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

    //                    return "<a href='/BranchProfile/Edit/" + bid + "?commonId=" + data + "&status=CommonSelection' class='edit btn btn-primary btn-sm' >" +
    //                        "<i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>";

    //                },
    //                "width": "9%",
    //                "orderable": false
    //            },
    //            {
    //                data: "code",
    //                name: "Code"
    //            }
    //            ,
    //            {
    //                data: "historicalPerformFlagDesc",
    //                name: "HistoricalPerformFlagDesc"
    //            }
    //            ,
    //            {
    //                data: "lastYearAuditFindingFlagDesc",
    //                name: "LastYearAuditFindingFlagDesc"
    //            }
    //            ,
    //            {
    //                data: "techCyberRiskFlagDesc",
    //                name: "TechCyberRiskFlagDesc"
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




    //    $("#BKCommonSelectionSettingList").on("change",
    //        ".acc-filters",
    //        function () {

    //            dataTable.draw();

    //        });
    //    $("#BKCommonSelectionSettingList").on("keyup",
    //        ".acc-filters",
    //        function () {

    //            dataTable.draw();

    //        });

    //    return dataTable;

    //}

    return {
        init: init
    }

}(CommonService, BKCommonSelectionSettingService);