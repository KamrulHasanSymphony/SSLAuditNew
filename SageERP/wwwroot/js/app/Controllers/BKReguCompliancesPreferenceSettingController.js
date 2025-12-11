var BKReguCompliancesPreferenceSettingController = function (CommonService, BKReguCompliancesPreferenceSettingService) {



    var init = function () {


        
        //if ($("#BKAuditOfficeTypeId").length) {
        if ($("#BKReguCompliancesPreferenceSetting_BKAuditOfficeTypeId").length) {
            //LoadCombo("BKAuditOfficeTypeId", '/Common/BKAuditOfficeTypes');
            LoadCombo("BKReguCompliancesPreferenceSetting_BKAuditOfficeTypeId", '/Common/BKAuditOfficeTypes');
        }   
        if ($("#BKAuditOfficeId").length) {
            LoadCombo("BKAuditOfficeId", '/Common/BKAuditOffice');
        } 


        $(".chkAll").click(function () {
            $('.dSelected:input:checkbox').not(this).prop('checked', this.checked);
        });

        var indexTable = BKReguCompliancesPreferenceSettingTable();


        //$(".btnsave").click(function () {
        $(".btnsaveBKReguCompliancesPreferenceSetting").click(function () {
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


        var validator = $("#frm_BKReguCompliancesPreferenceSetting").validate();
        var BKReguCompliancesPreferenceSetting = serializeInputs("frm_BKReguCompliancesPreferenceSetting");
        var result = validator.form();

        var isChecked = $("#BKReguComplianceStatus").is(":checked");
        BKReguCompliancesPreferenceSetting["BKReguCompliancesPreferenceSetting.Status"] = isChecked.toString();

        var InternationTxnFlag = $("#InternationTxnFlag").is(":checked");
        BKReguCompliancesPreferenceSetting["BKReguCompliancesPreferenceSetting.InternationTxnFlag"] = InternationTxnFlag.toString();

        var ForexFlag = $("#ForexFlag").is(":checked");
        BKReguCompliancesPreferenceSetting["BKReguCompliancesPreferenceSetting.ForexFlag"] = ForexFlag.toString();

        var HighProfileClintsFlag = $("#HighProfileClintsFlag").is(":checked");
        BKReguCompliancesPreferenceSetting["BKReguCompliancesPreferenceSetting.HighProfileClintsFlag"] = HighProfileClintsFlag.toString();

        var CorporateChientsFlag = $("#CorporateChientsFlag").is(":checked");
        BKReguCompliancesPreferenceSetting["BKReguCompliancesPreferenceSetting.CorporateChientsFlag"] = CorporateChientsFlag.toString();

        var AmlFlag = $("#AmlFlag").is(":checked");
        BKReguCompliancesPreferenceSetting["BKReguCompliancesPreferenceSetting.AmlFlag"] = AmlFlag.toString();


        if (!result) {
            validator.focusInvalid();
            return;
        }

        BKReguCompliancesPreferenceSettingService.save(BKReguCompliancesPreferenceSetting, saveDone, saveFail);

    }


    function saveDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                //$(".btnsave").html('Update');
                $(".btnsaveBKReguCompliancesPreferenceSetting").html('Update');
                //$("#Id").val(result.data.id);
                $("#BKReguCompliancesPreferenceSetting_Id").val(result.data.id);
                $("#Code").val(result.data.code);            
                result.data.operation = "update";
                //$("#Operation").val(result.data.operation);
                $("#BKReguCompliancesPreferenceSetting_Operation").val(result.data.operation);


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


    var BKReguCompliancesPreferenceSettingTable = function () {

        var dataTable = $("#BKReguCompliancesPreferenceSettingList").DataTable({
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
                url: '/BKReguCompliancesPreferenceSetting/_index',
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
                        return "<a href='/BranchProfile/Edit/" + bid + "?commonId=" + data + "&status=ReguCompliances#tabs-6' class='edit btn btn-primary btn-sm'>" +
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
                    data: "internationTxnFlag",
                    name: "InternationTxnFlag"
                },
                {
                    data: "forexFlag",
                    name: "ForexFlag"
                },
                {
                    data: "highProfileClintsFlag",
                    name: "HighProfileClintsFlag"
                },
                {
                    data: "corporateChientsFlag",
                    name: "CorporateChientsFlag"
                },
                {
                    data: "amlFlag",
                    name: "AmlFlag"
                },
                {
                    data: "status",
                    name: "Status"
                }
            ]
        });

        return dataTable;
    }



    //var BKReguCompliancesPreferenceSettingTable = function () {
    //    $('#BKReguCompliancesPreferenceSettingList thead tr')
    //        .clone(true)
    //        .addClass('filters')
    //        .appendTo('#BKReguCompliancesPreferenceSettingList thead');


    //    var dataTable = $("#BKReguCompliancesPreferenceSettingList").DataTable({
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
    //            url: '/BKReguCompliancesPreferenceSetting/_index',
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

    //                    return "<a href='/BranchProfile/Edit/" + bid + "?commonId=" + data + "&status=ReguCompliances' class='edit btn btn-primary btn-sm' >" +
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
    //                data: "internationTxnFlag",
    //                name: "InternationTxnFlag"
    //            }
    //            ,
    //            {
    //                data: "forexFlag",
    //                name: "ForexFlag"
    //            }
    //            ,
    //            {
    //                data: "highProfileClintsFlag",
    //                name: "HighProfileClintsFlag"
    //            }
    //            ,
    //            {
    //                data: "corporateChientsFlag",
    //                name: "CorporateChientsFlag"
    //            }
    //            ,
    //            {
    //                data: "amlFlag",
    //                name: "AmlFlag"
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




    //    $("#BKReguCompliancesPreferenceSettingList").on("change",
    //        ".acc-filters",
    //        function () {

    //            dataTable.draw();

    //        });
    //    $("#BKReguCompliancesPreferenceSettingList").on("keyup",
    //        ".acc-filters",
    //        function () {

    //            dataTable.draw();

    //        });

    //    return dataTable;

    //}

    return {
        init: init
    }

}(CommonService, BKReguCompliancesPreferenceSettingService);