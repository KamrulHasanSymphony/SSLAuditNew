var BKRiskAssessRegulationPreferenceCBSController = function (CommonService, BKRiskAssessRegulationPreferenceCBSService) {

    var init = function () {

      
        if ($("#BranchID").length) {
            LoadCombo("BranchID", '/Common/BKAuditOffice');
        } 

        $(".chkAll").click(function () {
            $('.dSelected:input:checkbox').not(this).prop('checked', this.checked);
        });

        var indexTable = BKRiskAssessRegulationPreferenceCBSTable();


        $(".btnsave").click(function () {
            save();
        });     


        $(".btnRiskAssessRegulationIntegration").click(function () {
            debugger;

            if ($.fn.DataTable.isDataTable("#IntegrationRiskAssessRegulationPreferenceCBSList")) {
                $("#IntegrationRiskAssessRegulationPreferenceCBSList").DataTable().destroy();
            }
            itegrationTable = IntegrationRiskAssessRegulationTable();

        });


    }


    /*init end*/

    $('#SaveRisk').on('click', function () {

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

        var dataTable = $('#IntegrationRiskAssessRegulationPreferenceCBSList').DataTable();


        BKRiskAssessRegulationPreferenceCBSService.saveRiskAssessRegulationPreferenceIntegration(model, saveRiskAssessRegulationDone, saveRiskAssessRegulationFail);

    }

    function saveRiskAssessRegulationDone(result) {
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

    function saveRiskAssessRegulationFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }



    var IntegrationRiskAssessRegulationTable = function () {

        // Remove the filter row to prevent search fields from appearing
        $('#IntegrationRiskAssessRegulationPreferenceCBSList thead .filters').remove();

        // Destroy existing DataTable if it exists
        if ($.fn.DataTable.isDataTable("#IntegrationRiskAssessRegulationPreferenceCBSList")) {
            $("#IntegrationRiskAssessRegulationPreferenceCBSList").DataTable().destroy();
        }

        var dataTable = $("#IntegrationRiskAssessRegulationPreferenceCBSList").DataTable({
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
                url: '/BKRiskAssessRegulationPreferenceCBS/RiskAssessRegulationIntegration',
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
                { data: "riskTxnAmount", name: "RiskTxnAmount" },
                { data: "status", name: "Status" }
            ]
        });

        return dataTable;
    };


    function save() {


        var validator = $("#frm_BKRiskAssessRegulationPreferenceCBS").validate();
        var BKRiskAssessRegulationPreferenceCBS = serializeInputs("frm_BKRiskAssessRegulationPreferenceCBS");
        var result = validator.form();


        var CompFinProductsFlg = $("#CompFinProductsFlg").is(":checked");
        BKRiskAssessRegulationPreferenceCBS.CompFinProductsFlg = CompFinProductsFlg;

        var GeoLocRiskFlg = $("#GeoLocRiskFlg").is(":checked");
        BKRiskAssessRegulationPreferenceCBS.GeoLocRiskFlg = GeoLocRiskFlg;

        var ForexFlg = $("#ForexFlg").is(":checked");
        BKRiskAssessRegulationPreferenceCBS.ForexFlg = ForexFlg;

        var HighProfileClientsFlg = $("#HighProfileClientsFlg").is(":checked");
        BKRiskAssessRegulationPreferenceCBS.HighProfileClientsFlg = HighProfileClientsFlg;

        var CorporateClientsFlg = $("#CorporateClientsFlg").is(":checked");
        BKRiskAssessRegulationPreferenceCBS.CorporateClientsFlg = CorporateClientsFlg;

        var InternationTxnFlg = $("#InternationTxnFlg").is(":checked");
        BKRiskAssessRegulationPreferenceCBS.InternationTxnFlg = InternationTxnFlg;

        var Status = $("#Status").is(":checked");
        BKRiskAssessRegulationPreferenceCBS.Status = Status;


        if (!result) {
            validator.focusInvalid();
            return;
        }

        BKRiskAssessRegulationPreferenceCBSService.save(BKRiskAssessRegulationPreferenceCBS, saveDone, saveFail);

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


    var BKRiskAssessRegulationPreferenceCBSTable = function () {

        $('#BKRiskAssessRegulationPreferenceCBSList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#BKRiskAssessRegulationPreferenceCBSList thead');

        var dataTable = $("#BKRiskAssessRegulationPreferenceCBSList").DataTable({
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
                url: '/BKRiskAssessRegulationPreferenceCBS/_index',
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

                        return "<a href=/BKRiskAssessRegulationPreferenceCBS/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a> "                                                 
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
                    data: "riskTxnAmount",
                    name: "RiskTxnAmount"
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

        $("#BKRiskAssessRegulationPreferenceCBSList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#BKRiskAssessRegulationPreferenceCBSList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }

    return {
        init: init
    }

}(CommonService, BKRiskAssessRegulationPreferenceCBSService);