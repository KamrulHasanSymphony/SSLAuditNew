var BKFraudIrregularitiesInternalControlPreferenceCBSController = function (CommonService, BKFraudIrregularitiesInternalControlPreferenceCBSService) {

    var init = function () {

      
        if ($("#BranchID").length) {
            LoadCombo("BranchID", '/Common/BKAuditOffice');
        } 

        $(".chkAll").click(function () {
            $('.dSelected:input:checkbox').not(this).prop('checked', this.checked);
        });

        var indexTable = BKFraudIrregularitiesInternalControlPreferenceCBSTable();


        $(".btnsave").click(function () {
            save();
        });     


        $(".btnFraudIrregularitiesInternalControlIntegration").click(function () {
            debugger;

            if ($.fn.DataTable.isDataTable("#IntegrationFraudIrregularitiesInternalControlCBSList")) {
                $("#IntegrationFraudIrregularitiesInternalControlCBSList").DataTable().destroy();
            }
            itegrationTable = IntegrationFraudIrregularitiesInternalControlCBSTable();

        });


    }


    /*init end*/

    $('#SaveFraud').on('click', function () {

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

        var dataTable = $('#IntegrationFraudIrregularitiesInternalControlCBSList').DataTable();


        BKFraudIrregularitiesInternalControlPreferenceCBSService.saveFraudIrregularitiesInternalControlreferenceIntegration(model, saveFraudIrregularitiesInternalControlDone, saveFraudIrregularitiesInternalControlFail);

    }

    function saveFraudIrregularitiesInternalControlDone(result) {
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

    function saveFraudIrregularitiesInternalControlFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }



    var IntegrationFraudIrregularitiesInternalControlCBSTable = function () {

        // Remove the filter row to prevent search fields from appearing
        $('#IntegrationFraudIrregularitiesInternalControlCBSList thead .filters').remove();

        // Destroy existing DataTable if it exists
        if ($.fn.DataTable.isDataTable("#IntegrationFraudIrregularitiesInternalControlCBSList")) {
            $("#IntegrationFraudIrregularitiesInternalControlCBSList").DataTable().destroy();
        }

        var dataTable = $("#IntegrationFraudIrregularitiesInternalControlCBSList").DataTable({
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
                url: '/BKFraudIrregularitiesInternalControlPreferenceCBS/FraudIrregularitiesInternalControlPreferenceIntegration',
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
                { data: "previouslyFraudIncidentFlg", name: "PreviouslyFraudIncidentFlg" },
                { data: "status", name: "Status" }
            ]
        });

        return dataTable;
    };




    function save() {


        var validator = $("#frm_BKFraudIrregularitiesInternalControlPreferenceCBS").validate();
        var BKFraudIrregularitiesInternalControlPreferenceCBS = serializeInputs("frm_BKFraudIrregularitiesInternalControlPreferenceCBS");
        var result = validator.form();


        var PreviouslyFraudIncidentFlg = $("#PreviouslyFraudIncidentFlg").is(":checked");
        BKFraudIrregularitiesInternalControlPreferenceCBS.PreviouslyFraudIncidentFlg = PreviouslyFraudIncidentFlg;

        var EmpMisConductFlg = $("#EmpMisConductFlg").is(":checked");
        BKFraudIrregularitiesInternalControlPreferenceCBS.EmpMisConductFlg = EmpMisConductFlg;

        var IrregularitiesFlg = $("#IrregularitiesFlg").is(":checked");
        BKFraudIrregularitiesInternalControlPreferenceCBS.IrregularitiesFlg = IrregularitiesFlg;

        var InternalControlFlg = $("#InternalControlFlg").is(":checked");
        BKFraudIrregularitiesInternalControlPreferenceCBS.InternalControlFlg = InternalControlFlg;

        var ProperDocumentationFlg = $("#ProperDocumentationFlg").is(":checked");
        BKFraudIrregularitiesInternalControlPreferenceCBS.ProperDocumentationFlg = ProperDocumentationFlg;

        var ProperReportingFlg = $("#ProperReportingFlg").is(":checked");
        BKFraudIrregularitiesInternalControlPreferenceCBS.ProperReportingFlg = ProperReportingFlg;

        var Status = $("#Status").is(":checked");
        BKFraudIrregularitiesInternalControlPreferenceCBS.Status = Status;




        if (!result) {
            validator.focusInvalid();
            return;
        }

        BKFraudIrregularitiesInternalControlPreferenceCBSService.save(BKFraudIrregularitiesInternalControlPreferenceCBS, saveDone, saveFail);

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


    var BKFraudIrregularitiesInternalControlPreferenceCBSTable = function () {

        $('#BKFraudIrregularitiesInternalControlPreferenceCBSList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#BKFraudIrregularitiesInternalControlPreferenceCBSList thead');

        var dataTable = $("#BKFraudIrregularitiesInternalControlPreferenceCBSList").DataTable({
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
                url: '/BKFraudIrregularitiesInternalControlPreferenceCBS/_index',
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

                        return "<a href=/BKFraudIrregularitiesInternalControlPreferenceCBS/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a> "                                                 
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
                    data: "previouslyFraudIncidentFlg",
                    name: "PreviouslyFraudIncidentFlg"
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

        $("#BKFraudIrregularitiesInternalControlPreferenceCBSList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#BKFraudIrregularitiesInternalControlPreferenceCBSList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }

    return {
        init: init
    }

}(CommonService, BKFraudIrregularitiesInternalControlPreferenceCBSService);