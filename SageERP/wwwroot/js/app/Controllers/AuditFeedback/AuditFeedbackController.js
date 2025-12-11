var AuditFeedbackController = function (AuditFeedbackService) {

    var init = function (param) {

        var indexTable;
        if ($("#AuditBranchFeedbackList").length) {
            var Condigs = excelIndexListTableData()
            indexTable = $("#AuditBranchFeedbackList").DataTable();          
        }  

        if ($("#AuditFeedList").length) {
            var tableConfigs = GetIndexTable(param)
            detailTable = $("#AuditFeedList").DataTable(tableConfigs);
        }

        $(".btnSave").on("click", function () {
            $(this)
            Save();

        });


        if ($("#AuditIssueId").length) {
            LoadCombo("AuditIssueId", '/Common/GetIssues');
        }
    }


    var excelIndexListTableData = function () {

        $('#AuditBranchFeedbackList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#AuditBranchFeedbackList thead');

        debugger;
        var auditId = $("#Id").val();
        var databaseRowCount = 1000;


        var dataTable = $("#AuditBranchFeedbackList").DataTable({
            orderCellsTop: true,
            fixedHeader: true,
            serverSide: true,
            "processing": true,
            "bProcessing": true,           
            dom: '<"pagination-left"lBfrtip>',

            bRetrieve: true,
            searching: false,
            lengthMenu: [10, 25, 50, 100, 125, 150, 175, 200, 300, 400, 500],

            buttons: [
                {
                    extend: 'pdfHtml5',
                    title: 'GDIC Audit Pdf File',
                    exportOptions: {
                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18]
                    }
                },
                {
                    extend: 'copyHtml5',
                    title: 'GDIC Audit',
                    exportOptions: {
                        columns: [1, 2, 3, 4]
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: 'GDIC Audit Excel File',
                    exportOptions: {
                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19]
                    }
                },
                'csvHtml5'
            ],


            ajax: {
                url: '/AuditBranchFeedback/_branchPreviewIndex?id=' + auditId,
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {

                            "auditCode": $("#md-AuditCode").val(),
                            "auditName": $("#md-AuditName").val(),
                            "auditStatus": $("#md-AuditStatus").val(),
                            "issuename": $("#md-IssueName").val(),
                            "issueHeading": $("#md-IssueHeading").val(),
                            "issuepriority": $("#md-IssuePriority").val(),
                            "issueStatus": $("#md-IssueStatus").val(),
                            "dateofsubmission": $("#md-DateOfSubmission").val(),
                            "investigationOrforensis": $("#md-InvestigationOrForensis").val(),
                            "stratigicMeeting": $("#md-StratigicMeeting").val(),
                            "managementReviewMeeting": $("#md-ManagementReviewMeeting").val(),
                            "otherMeeting": $("#md-OtherMeeting").val(),
                            "training": $("#md-Training").val(),
                            "operational": $("#md-Operational").val(),
                            "financial": $("#md-Financial").val(),
                            "compliance": $("#md-Compliance").val(),
                            "enddate": $("#md-EndDate").val(),
                            "ispost": $("#md-Post").val()


                        });
                }
            },

            columns: [

                {

                    data: null,
                    name: "ID",
                    "width": "4%",
                    render: function (data, type, row, meta) {
                        if (type === 'display') {
                            var displayCount = meta.row + meta.settings._iDisplayStart + 1;
                            if (displayCount <= databaseRowCount) {
                                return displayCount;
                            } else {
                                return '';
                            }
                        }
                        return data;
                    }
                },

                {
                    data: "issueName",
                    name: "IssueName"
                }
                ,
                {
                    data: "issueDetails",
                    name: "IssueDetails"
                }
                ,
                {
                    data: "createdBy",
                    name: "CreatedBy"
                }
                ,
                {
                    data: "branchFeedbackDetails",
                    name: "BranchFeedbackDetails"
                }
            ],

            order: [1, "asc"],

        });


        if (dataTable.columns().eq(0)) {
            dataTable.columns().eq(0).each(function (colIdx) {

                var cell = $('.filters th').eq($(dataTable.column(colIdx).header()).index());

                var title = $(cell).text();


                if ($(cell).hasClass('action')) {
                    $(cell).html('');

                } else if ($(cell).hasClass('bool')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>True</option><option>False</option></select>');

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

        $("#AuditBranchFeedbackList").on("change",
            ".acc-filters",
            function () {
                dataTable.draw();
            });

        $("#AuditBranchFeedbackList").on("keyup",
            ".acc-filters",
            function () {
                dataTable.draw();
            });
        return dataTable;

    }


    function GetIndexTable(param) {
        return {

            "processing": true,
            serverSide: true,

            ajax: {
                url: '/AuditFeedback/_index?id=' + param.auditId,
                type: 'POST',
                data: function (payLoad) {

                    return $.extend({},
                        payLoad,
                        {
                            //"search2": $("#name").val()
                        });
                }
            },

            columns: [
                {
                    data: "id",
                    render: function (data) {

                        return "<a href=/AuditFeedback/Edit/" + data + " class='edit' ><i class='editIcon' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>  ";

                    },
                    "width": "7%",
                    "orderable": false
                },

                {
                    data: "auditName",
                    name: "AuditName"
                },
                {
                    data: "issueName",
                    name: "IssueName"
                },
                {
                    data: "heading",
                    name: "Heading"
                }
              
            ],
            order: [1, "asc"],

        }
    }



    function Save() {

        var validator = $("#frm_Audit").validate();
        var result = validator.form();

        if (!result) {
            ShowNotification(2, "Please complete the form");
            return;
        }

        var form = $("#frm_Audit")[0];
        var formData = new FormData(form);

        AuditFeedbackService.save(formData, saveDone, saveFail);

    }

    function addListItem(result) {
        var list = $(".fileGroup");

        result.data.attachmentsList.forEach(function (item) {

            var item = '<li class="list-group-item" id="file-' + item.id + '"> <span>' +
                item.displayName +
                '</span><a target="_blank" href="/AuditIssue/DownloadFile?filePath=' + item.fileName + '" class=" ml-2 btn btn-primary btn-sm float-right ml-2">Download</a>' +
                '<button onclick="AuditFeedbackController.deleteFile(\'' + item.id + '\', \'' + item.fileName + '\')" class=" ml-2 btn btn-danger btn-sm float-right" type="button">Delete</button>' +
                '</li>';

            list.append(item);
        });
    }


    function saveDone(result) {
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnSave").html('Update');
                $("#Id").val(result.data.id);

                result.data.operation = "update";

                $("#Operation").val(result.data.operation);

                addListItem(result);


            } else {

                addListItem(result);
                ShowNotification(1, result.message);
            }
        }
        else if (result.status == "400") {
            ShowNotification(3, "Something gone wrong");
        }
    }

    function saveFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }



    var deleteFile = function deleteFile(fileId, filePath) {

        AuditFeedbackService.deleteFile({ id: fileId, filePath: filePath }, (result) => {


            if (result.status == "200") {
                $("#file-" + fileId).remove();

                ShowNotification(1, result.message);
            }
            else if (result.status == "400") {
                ShowNotification(3, result.message);
            }

        }, saveFailDelete);

    };

    function saveFailDelete(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }

    return {
        init: init,
        deleteFile
    }

}(AuditFeedbackService);