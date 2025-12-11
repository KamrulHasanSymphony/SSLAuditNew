var BKFinancePreformPreferenceCBSController = function (CommonService, BKFinancePreformPreferenceCBSService) {

    var init = function () {

      
        if ($("#BranchID").length) {
            LoadCombo("BranchID", '/Common/BKAuditOffice');
        } 

        $(".chkAll").click(function () {
            $('.dSelected:input:checkbox').not(this).prop('checked', this.checked);
        });

        var indexTable = BKFinancePreformPreferenceCBSTable();


        $(".btnsave").click(function () {
            save();
        });     


        $(".btnFinancePerformPreferenceIntegration").click(function () {
            debugger;

            if ($.fn.DataTable.isDataTable("#IntegrationFinancePerformPreferenceCBSList")) {
                $("#IntegrationFinancePerformPreferenceCBSList").DataTable().destroy();
            }
            itegrationTable = IntegrationFinancePerformPreferenceCBSTable();

        });


    }


    /*init end*/

    $('#SaveFinance').on('click', function () {

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

        var dataTable = $('#IntegrationFinancePerformPreferenceCBSList').DataTable();

      
        BKFinancePreformPreferenceCBSService.saveFinancePreformPreferenceIntegration(model, saveFinancePerformDone, saveFinancePerformFail);

    }

    function saveFinancePerformDone(result) {
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

    function saveFinancePerformFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }



    var IntegrationFinancePerformPreferenceCBSTable = function () {

        // Remove the filter row to prevent search fields from appearing
        $('#IntegrationFinancePerformPreferenceCBSList thead .filters').remove();

        // Destroy existing DataTable if it exists
        if ($.fn.DataTable.isDataTable("#IntegrationBKAuditOfficePreferenceCBSList")) {
            $("#IntegrationFinancePerformPreferenceCBSList").DataTable().destroy();
        }

        var dataTable = $("#IntegrationFinancePerformPreferenceCBSList").DataTable({
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
                url: '/BKFinancePreformPreferenceCBS/AuditOfficePreferenceIntegration',
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
                { data: "financialPerformFlg", name: "FinancialPerformFlg" },
                { data: "status", name: "Status" }
            ]
        });

        return dataTable;
    };

    function save() {


        var validator = $("#frm_BKFinancePreformPreferenceCBS").validate();
        var BKFinancePreformPreferenceCBS = serializeInputs("frm_BKFinancePreformPreferenceCBS");
        var result = validator.form();


        var FinancialPerformFlg = $("#FinancialPerformFlg").is(":checked");
        BKFinancePreformPreferenceCBS.FinancialPerformFlg = FinancialPerformFlg;

        var FundAvailableFlg = $("#FundAvailableFlg").is(":checked");
        BKFinancePreformPreferenceCBS.FundAvailableFlg = FundAvailableFlg;

        var MisManagementClientsFlg = $("#MisManagementClientsFlg").is(":checked");
        BKFinancePreformPreferenceCBS.MisManagementClientsFlg = MisManagementClientsFlg;

        var EfficiencyFlg = $("#EfficiencyFlg").is(":checked");
        BKFinancePreformPreferenceCBS.EfficiencyFlg = EfficiencyFlg;

        var NplsLargeFlg = $("#NplsLargeFlg").is(":checked");
        BKFinancePreformPreferenceCBS.NplsLargeFlg = NplsLargeFlg;

        var LargeTxnManageFlg = $("#LargeTxnManageFlg").is(":checked");
        BKFinancePreformPreferenceCBS.LargeTxnManageFlg = LargeTxnManageFlg;

        var HighValueAssetManageFlg = $("#HighValueAssetManageFlg").is(":checked");
        BKFinancePreformPreferenceCBS.HighValueAssetManageFlg = HighValueAssetManageFlg;

        var SecurityMeasuresStaffFlg = $("#SecurityMeasuresStaffFlg").is(":checked");
        BKFinancePreformPreferenceCBS.SecurityMeasuresStaffFlg = SecurityMeasuresStaffFlg;

        var BudgetMgtFlg = $("#BudgetMgtFlg").is(":checked");
        BKFinancePreformPreferenceCBS.BudgetMgtFlg = BudgetMgtFlg;

        var Status = $("#Status").is(":checked");
        BKFinancePreformPreferenceCBS.Status = Status;

        
        

        if (!result) {
            validator.focusInvalid();
            return;
        }

        BKFinancePreformPreferenceCBSService.save(BKFinancePreformPreferenceCBS, saveDone, saveFail);

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


    var BKFinancePreformPreferenceCBSTable = function () {

        $('#BKFinancePreformPreferenceCBSList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#BKFinancePreformPreferenceCBSList thead');

        var dataTable = $("#BKFinancePreformPreferenceCBSList").DataTable({
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
                url: '/BKFinancePreformPreferenceCBS/_index',
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

                        return "<a href=/BKFinancePreformPreferenceCBS/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a> "                                                 
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
                    data: "financialPerformFlg",
                    name: "FinancialPerformFlg"
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

        $("#BKFinancePreformPreferenceCBSList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#BKFinancePreformPreferenceCBSList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }

    return {
        init: init
    }

}(CommonService, BKFinancePreformPreferenceCBSService);