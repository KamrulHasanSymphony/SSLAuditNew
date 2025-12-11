var BKAuditTemlateMasterController = function (CommonService, BKAuditTemlateMasterService) {



    var init = function () {


        if ($("#BKAuditCategoryId").length) {
            LoadCombo("BKAuditCategoryId", '/Common/BKAuidtCategorys');
        }
        if ($("#BKAuditOfficeTypeId").length) {
            LoadCombo("BKAuditOfficeTypeId", '/Common/BKAuditOfficeTypes');
        }


        $(".chkAll").click(function () {
            $('.dSelected:input:checkbox').not(this).prop('checked', this.checked);
        });

        var indexTable = BKAuditTemlateMasterTable();



        $(".btnsave").click(function () {
            save();
        });
       

    };

    /*init end*/

    //$('#BKAuditOfficeTypeId').on('change', function () {
    //    debugger;
    //    var selectedValue = $(this).val();
    //    GetAllMappingData(selectedValue);
    //});

    //$('#BKAuditOfficeTypeId').on('click', function () {
    //    debugger;
    //    $(this).trigger('change');
    //});

    //$('#BKAuditOfficeTypeId').on('change', function () {
    //    debugger;
    //    var selectedValue = $(this).val();
    //    GetAllMappingData(selectedValue);
    //});


   

    $('select[name="BKAuditOfficeTypeId"]').on('change', function () {
        debugger;
        var id = $("#Id").val();
        var selectedValue = $(this).val();
        //GetAllMappingData(selectedValue);
        GetAllMappingData(selectedValue,id);
    });

    function GetAllMappingData(BKAuditOfficeTypeId,id) {
        if (BKAuditOfficeTypeId !== '' && BKAuditOfficeTypeId !== '0') {

            $.ajax({
                type: "POST",
                //url: "/BKAuditTemlateMaster/GetAllMappingData?bKAuditOfficeTypeId=" + BKAuditOfficeTypeId,
                url: "/BKAuditTemlateMaster/GetAllMappingData?bKAuditOfficeTypeId=" + BKAuditOfficeTypeId + "&id=" + id,
                dataType: 'json',
                success: function (result) {
                    debugger;
                    let htmlData = '';
                    if (!result.length) {
                        htmlData = '<tr><td colspan="2" class="text-center">No data found!</td></tr>';
                    } else {
                        debugger;

                        for (let i = 0; i < result.data.length; i++) {

                            debugger;
                            const item = result.data[i];


                            htmlData += `
                    <div class="col-sm-6 d-flex align-items-center mt-4">
                        <label for="Compliance${i}" class="mr-2 mb-0" style="width: 150px;">Compliance:</label>
                        <input type="text" class="form-control form-control-sm" id="Compliance${i}" name="Compliance" value="${result.data[i]['bkAuditComplianceDes']}" style="width: 70%;">
                         <input type="hidden" id="ComplainceId${i}" name="CheckListItemId" value="${result.data[i]['bkAuditComplianceId']}" />
                    </div>

                    <div class="col-sm-6 d-flex align-items-center mt-4">
                        <label for="CheckListType${i}" class="mr-2 mb-0" style="width: 150px;">CheckList Type:</label>
                        <input type="text" class="form-control form-control-sm" id="CheckListType${i}" name="CheckListType" value="${result.data[i]['bkCheckListTypeDes']}" style="width: 70%;">
                         <input type="hidden" id="checkListTypeId${i}" name="CheckListTypeId" value="${result.data[i]['bkCheckListTypeId']}" />

                    </div>

                    <div class="col-sm-6 d-flex align-items-center mt-4">
                        <label for="CheckListSubType${i}" class="mr-2 mb-0" style="width: 150px;">CheckList Sub Type:</label>
                        <input type="text" class="form-control form-control-sm" id="CheckListSubType${i}" name="CheckListSubType" value="${result.data[i]['bkCheckListSubTypeDes']}" style="width: 70%;">
                         <input type="hidden" id="CheckListSubTypeId${i}" name="CheckListSubTypeId" value="${result.data[i]['bkCheckListSubTypeId']}" />

                    </div>

                   
                `;

                        


                            debugger;
                            let val = result.data.checkListItemList;
                            let itemData = result.data[i];

                            let checkListItemList = itemData.checkListItemList;
                            if (checkListItemList && checkListItemList.length > 0) {

                                htmlData += `

                                 <!-- CheckList Items -->
                    <div class="col-sm-6 d-flex align-items-center mt-4">
                        <label for="CheckListItems${i}" class="mr-2 mb-0" style="width: 150px;">CheckList Items:</label>
                    </div>
                    
                `;

                                htmlData += `
                    <div class="col-sm-6 d-flex align-items-center mt-4 mb-4">
                        <input type="checkbox" id="myCheckbox" checked name="myCheckbox" class="chkAll" style="width:30px;height:30px;margin-right:30px;" value="true" />
                        <label for="CheckListSubType" class="mr-2 mb-0">[All Select And De-Select Option]</label>

                        

                    </div>
                `;


                                for (let j = 0; j < checkListItemList.length; j++) {
                                    const checklistItem = checkListItemList[j];

                                   

                                    htmlData += `
                    <div class="col-sm-6 d-flex align-items-center mt-4 mb-4">
                        <input type="checkbox" id="CheckListItem${i}_${j}" name="CheckListItem" style="width:30px;height:30px;" ${checklistItem.status ? 'checked' : ''} />
                                               
                        <textarea class="form-control form-control-sm"id="CheckListItemText${i}_${j}" name="CheckListItemText"style="width: 60%; margin-left:20px; margin-right:100px;">${checklistItem.description}</textarea>
                        <input type="hidden" id="CheckListItemId${i}_${j}" name="CheckListItemId" value="${checklistItem.id}" />
                        <input type="hidden" id="BKCheckListSubTypesIdItems${i}_${j}" name="BKCheckListSubTypesIdItems" value="${checklistItem.bkCheckListSubTypesId}" />


                         <!-- Dropdown for Mandatory/Optional -->
                <select id="CheckListItem${i}_${j}_MandatoryOptional" name="CheckListItemMandatoryOptional" style="width: 100px; margin-left:10px;">
                    <option value="Mandatory" ${checklistItem.isFieldType ? 'selected' : ''}>Mandatory</option>
                    <option value="Optional" ${!checklistItem.isFieldType ? 'selected' : ''}>Optional</option>
                </select>


                    </div>
                `;
                                }

                            }

                            htmlData += `<hr />`;

                        }
                    }


                    $(".card-bodyData").html(htmlData);

                },
                error: function (error) {
                    console.error('Error fetching data:', error);
                }
            });
        }
    }


 
    //$(document).on('change', '#myCheckbox', function () {
    //    let isChecked = $(this).is(':checked');
    //    $('input[name="CheckListItem"]').prop('checked', isChecked);
    //});
    //$(document).on('change', 'input[name="CheckListItem"]', function () {
    //    if (!$(this).is(':checked')) {
    //        $('#myCheckbox').prop('checked', false);
    //    } else {
    //        let allChecked = $('input[name="CheckListItem"]').length === $('input[name="CheckListItem"]:checked').length;
    //        $('#myCheckbox').prop('checked', allChecked);
    //    }
    //});



    function save() {

        debugger;

        var validator = $("#frm_BKAuditTemlateMaster").validate();
        var result = validator.form();

        if (!result) {
            validator.focusInvalid();
            return;
        }

        // Collect form data
        let masterObj = {
            Id: $('#Id').val(),
            Code: $('#Code').val(),
            BKAuditOfficeTypeId: $('#BKAuditOfficeTypeId').val(),
            BKAuditCategoryId: $('#BKAuditCategoryId').val(),
            Description: $('#Description').val(),
            Status: $('#Status').is(':checked'),
            Operation: $('#Operation').val(),
            Edit: "Edit",  // Placeholder value
            BKAuditTemplateDetailsList: []  // Initialize the list
        };

        // Collect Mapping Data and CheckList Items
        $('input[name="Compliance"]').each(function (index) {
            let complianceId = $('#ComplainceId' + index).val();
            let complianceDescription = $('#Compliance' + index).val();
            let checkListTypeId = $('#checkListTypeId' + index).val();
            let checkListTypeDescription = $('#CheckListType' + index).val();
            let checkListSubTypeId = $('#CheckListSubTypeId' + index).val();
            let checkListSubTypeDescription = $('#CheckListSubType' + index).val();

            // Collect checklist items related to this compliance
            let checkListItems = [];
            $('input[name="CheckListItem"]').each(function () {
                if ($(this).siblings('input[name="BKCheckListSubTypesIdItems"]').val() === checkListSubTypeId) {
                    checkListItems.push({
                        Id: $(this).siblings('input[name="CheckListItemId"]').val(),
                        Description: $(this).siblings('textarea[name="CheckListItemText"]').val(),
                        Status: $(this).is(':checked'),
                        IsFieldType: $(this).siblings('select[name="CheckListItemMandatoryOptional"]').val() === 'Mandatory',
                        CheckListSubTypeId: checkListSubTypeId
                    });
                }
            });


            // Push the details to the master object
            masterObj.BKAuditTemplateDetailsList.push({

                BKAuditComplianceId: complianceId,
                BKCheckListTypeId: checkListTypeId,
                BKAuditTemlateMasterId: masterObj.Id,  
                //BKCheckListItemId: complianceId,
                BKCheckListSubTypeId: checkListSubTypeId,
                IsTemplateDetails: true,  
                Status: true,
                Description: complianceDescription,
                MappingData: [{
                    ComplianceId: complianceId,
                    ComplianceDescription: complianceDescription,
                    CheckListTypeId: checkListTypeId,
                    CheckListTypeDescription: checkListTypeDescription,
                    CheckListSubTypeId: checkListSubTypeId,
                    CheckListSubTypeDescription: checkListSubTypeDescription
                }],
                CheckListItemList: checkListItems
            });
        });

        BKAuditTemlateMasterService.save(masterObj, saveDone, saveFail);


    }


    function saveDone(result) {
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

    function saveFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }


    var BKAuditTemlateMasterTable = function () {

        $('#BKAuditTemlateMasterList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#BKAuditTemlateMasterList thead');


        var dataTable = $("#BKAuditTemlateMasterList").DataTable({
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
                url: '/BKAuditTemlateMaster/_index',
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

                        return "<a href=/BKAuditTemlateMaster/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='' data-original-title='Edit'></i></a> " //+

                            //"<input onclick='CheckAll(this)' class='dSelected' type='checkbox' data-Id=" + data + " >"                         

                            ;


                    },
                    "width": "9%",
                    "orderable": false
                },
                {
                    data: "code",
                    name: "Code"

                }
                ,
                {
                    data: "description",
                    name: "Description"

                }
                ,
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




        $("#BKAuditTemlateMasterList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#BKAuditTemlateMasterList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }

    return {
        init: init
    }

}(CommonService, BKAuditTemlateMasterService);