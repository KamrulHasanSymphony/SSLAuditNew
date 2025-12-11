var BKAuditOfficePreferenceCBSController = function (CommonService, BKAuditOfficePreferenceCBSService) {

    var init = function () {

      
        if ($("#BranchID").length) {
            LoadCombo("BranchID", '/Common/BKAuditOffice');
        } 

        $(".chkAll").click(function () {
            $('.dSelected:input:checkbox').not(this).prop('checked', this.checked);
        });

        var indexTable = BKAuditOfficePreferenceCBSTable();

        var itegrationTable;


        $(".btnsave").click(function () {
            save();
        });     

        //$(".btnAuditOfficePreferenceIntegration").click(function () {
        //    debugger;
        //    //btnAuditOfficePreferenceIntegration();
        //    itegrationTable = IntegrationBKAuditOfficePreferenceCBSTable();
        //});


        $(".chkAll").click(function () {
            $('.dSelected:input:checkbox').not(this).prop('checked', this.checked);
        });

        $(".btnAuditOfficePreferenceIntegration").click(function () {
            debugger;

            // Destroy the existing DataTable instance if it exists
            if ($.fn.DataTable.isDataTable("#IntegrationBKAuditOfficePreferenceCBSList")) {
                $("#IntegrationBKAuditOfficePreferenceCBSList").DataTable().destroy();
            }

            // Reinitialize the DataTable
            itegrationTable = IntegrationBKAuditOfficePreferenceCBSTable();
        });



    }


    /*init end*/


    var IntegrationBKAuditOfficePreferenceCBSTable = function () {

        // Remove the filter row to prevent search fields from appearing
        $('#IntegrationBKAuditOfficePreferenceCBSList thead .filters').remove();

        // Destroy existing DataTable if it exists
        if ($.fn.DataTable.isDataTable("#IntegrationBKAuditOfficePreferenceCBSList")) {
            $("#IntegrationBKAuditOfficePreferenceCBSList").DataTable().destroy();
        }

        var dataTable = $("#IntegrationBKAuditOfficePreferenceCBSList").DataTable({
            orderCellsTop: true,
            fixedHeader: true,
            serverSide: true,
            processing: true,
            dom: 'lBfrtip',
            bRetrieve: true,
            searching: false,  // Disable built-in search

            buttons: [
                {
                    extend: 'pdfHtml5',
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
                url: '/BKAuditOfficePreferenceCBS/AuditOfficePreferenceIntegration',
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
                    data: "importId",
                    render: function (data) {
                        return "<input onclick='CheckAll(this)' class='dSelected' type='checkbox' data-Id=" + data + " >";
                    },
                    width: "9%",
                    orderable: false
                },
                { data: "auditYear", name: "AuditYear" },
                { data: "historicalPerformFlg", name: "HistoricalPerformFlg" },
                { data: "status", name: "Status" }
            ]
        });

        return dataTable;
    };


    $('#SaveAOPI').on('click', function () {

        Confirmation("Are you sure? Do You Want to Save Data?", function (result) {
            console.log(result);
            if (result) {

                SelectData(true);
            }
        });

    });

    function SelectData(IsPost) {

        debugger;

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

        var dataTable = $('#IntegrationBKAuditOfficePreferenceCBSList').DataTable();

        //var rowData = dataTable.rows().data().toArray();
        //var filteredData = [];
        //var filteredData1 = [];
        //if (IsPost) {
        //    filteredData = rowData.filter(x => x.isPost === "Y" && IDs.includes(x.id.toString()));
        //}
        //else {
        //    filteredData = rowData.filter(x => x.isPush === "Y" && IDs.includes(x.id.toString()));
        //    filteredData1 = rowData.filter(x => x.isPost === "N" && IDs.includes(x.id.toString()));
        //}

       
        BKAuditOfficePreferenceCBSService.saveAuditOfficePreferenceIntegration(model, saveAuditOfficeDone, saveAuditOfficeFail);
            
    }

    function saveAuditOfficeDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnsave").html('Update');
                $("#Id").val(result.data.id);
                $("#Code").val(result.data.code);
                result.data.operation = "update";
                $("#Operation").val(result.data.operation);

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

    function saveAuditOfficeFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }


    
    function btnAuditOfficePreferenceIntegration() {

        debugger;

        var validator = $("#frm_AuditOfficePreferenceIntegration").validate();
        var integration = serializeInputs("frm_AuditOfficePreferenceIntegration");

        var result = validator.form();

        if (!result) {
            validator.focusInvalid();
            return;
        }

        BKAuditOfficePreferenceCBSService.AuditOfficePreferenceIntegration(integration, saveAuditOfficePreferenceDone, saveAuditOfficePreferenceFail);

    }

    function saveAuditOfficePreferenceDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnsave").html('Update');
                $("#Id").val(result.data.id);
                $("#Code").val(result.data.code);
                result.data.operation = "update";
                $("#Operation").val(result.data.operation);

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

    function saveAuditOfficePreferenceFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }



    function save() {
        debugger;

        var validator = $("#frm_BKAuditOfficePreferenceCBS").validate();
        var BKAuditOfficePreferenceCBS = serializeInputs("frm_BKAuditOfficePreferenceCBS");
        var result = validator.form();


        var HistoricalPerformFlg = $('#HistoricalPerformFlg').prop('checked');
        BKAuditOfficePreferenceCBS.HistoricalPerformFlg = HistoricalPerformFlg;

        var LastYearAuditFindingsFlg = $('#LastYearAuditFindingsFlg').prop('checked');
        BKAuditOfficePreferenceCBS.LastYearAuditFindingsFlg = LastYearAuditFindingsFlg;

        var TechCyberRiskFlg = $('#TechCyberRiskFlg').prop('checked');
        BKAuditOfficePreferenceCBS.TechCyberRiskFlg = TechCyberRiskFlg;

        var OfficeSizeFlg = $('#OfficeSizeFlg').prop('checked');
        BKAuditOfficePreferenceCBS.OfficeSizeFlg = OfficeSizeFlg;

        var OfficeSignificanceFlg = $('#OfficeSignificanceFlg').prop('checked');
        BKAuditOfficePreferenceCBS.OfficeSignificanceFlg = OfficeSignificanceFlg;

        var StaffTurnoverFlg = $('#StaffTurnoverFlg').prop('checked');
        BKAuditOfficePreferenceCBS.StaffTurnoverFlg = StaffTurnoverFlg;

        var StaffTrainingGapsFlg = $('#StaffTrainingGapsFlg').prop('checked');
        BKAuditOfficePreferenceCBS.StaffTrainingGapsFlg = StaffTrainingGapsFlg;

        var StrategicInitiativeFlg = $('#StrategicInitiativeFlg').prop('checked');
        BKAuditOfficePreferenceCBS.StrategicInitiativeFlg = StrategicInitiativeFlg;

        var Status = $('#Status').prop('checked');
        BKAuditOfficePreferenceCBS.Status = Status;


        if (!result) {
            validator.focusInvalid();
            return;
        }

        BKAuditOfficePreferenceCBSService.save(BKAuditOfficePreferenceCBS, saveDone, saveFail);

    }

    function saveDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnsave").html('Update');
                //$("#Id").val(result.data.id);
                $("#BKRiskAssessPerferenceSetting_Id").val(result.data.id);
                $("#Code").val(result.data.code);            
                result.data.operation = "update";
                $("#Operation").val(result.data.operation);

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


    var BKAuditOfficePreferenceCBSTable = function () {

        $('#BKAuditOfficePreferenceCBSList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#BKAuditOfficePreferenceCBSList thead');

        var dataTable = $("#BKAuditOfficePreferenceCBSList").DataTable({
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
                url: '/BKAuditOfficePreferenceCBS/_index',
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {

                            "indexsearch": $("#Branchs").val(),
                            "branchid": $("#CurrentBranchId").val(),
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

                        return "<a href=/BKAuditOfficePreferenceCBS/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a> "                                                 
                            ;                
                    },
                    "width": "9%",
                    "orderable": false
                },                           
                {
                    data: "auditYear",
                    name: "AuditYear"
                }
                ,
                {
                    data: "historicalPerformFlg",
                    name: "HistoricalPerformFlg"
                },
                {
                    data: "status",
                    name: "Status"
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

        $("#BKAuditOfficePreferenceCBSList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#BKAuditOfficePreferenceCBSList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }

    return {
        init: init
    }

}(CommonService, BKAuditOfficePreferenceCBSService);