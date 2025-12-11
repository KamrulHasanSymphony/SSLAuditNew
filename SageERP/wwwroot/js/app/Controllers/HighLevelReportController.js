var HighLevelReportController = function (CommonService, HighLevelReportService) {

    var indexTable;

    var init = function () {

        debugger;
        if ($("#HighLevelReportIndexList").length) {
            var Condigs = highLevelReportIndexListTableData()
            indexTable = $("#HighLevelReportIndexList").DataTable();
        }

    }

    /*init end*/

    var highLevelReportIndexListTableData = function () {

        $('#HighLevelReportIndexList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#HighLevelReportIndexList thead');

        var Status = $("#Edit").val();
        var databaseRowCount = 1000;


        var dataTable = $("#HighLevelReportIndexList").DataTable({
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
                url: '/HithLevelReport/_index?edit=' + Status,
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

                    data: "auditId",
                    render: function (data) {

                        return "<a href='/Audit/Edit/" + data + "?edit=issue'  class='edit js-edit' data-id='" + data + "'  ><i class='material-icons' data-toggle='tooltip' title='Edit Area' data-id='" + data + "' data-original-title='Edit'></i></a>  "

                    },
                    //"width": "4%",
                    "orderable": false
                },

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
                    data: "code",
                    name: "Code"
                }
                ,
                {
                    data: "name",
                    name: "Name"
                },
                {
                    data: "issueName",
                    name: "IssueName"
                },
                {
                    data: "issueDetails",
                    name: "IssueDetails",

                },
                {
                    data: "risk",
                    name: "Risk",

                },
                {
                    data: "feedbackDetails",
                    name: "FeedbackDetails",

                }
                ,
                {
                    data: "branchFeedBackDetails",
                    name: "BranchFeedBackDetails",

                },
                {
                    data: "auditStatus",
                    name: "AuditStatus"
                }
                ,
                {
                    data: "issuePriority",
                    name: "IssuePriority",

                }
                ,
                {
                    data: "issueStatus",
                    name: "IssueStatus",
                },
                {
                    data: "dateOfSubmission",
                    name: "DateOfSubmission"
                },
                {

                    data: "investigationOrForensis",
                    name: "InvestigationOrForensis",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,
                {

                    data: "stratigicMeeting",
                    name: "StratigicMeeting",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,
                {

                    data: "managementReviewMeeting",
                    name: "ManagementReviewMeeting",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,
                {

                    data: "otherMeeting",
                    name: "OtherMeeting",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,
                {

                    data: "training",
                    name: "Training",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,
                {

                    data: "operational",
                    name: "Operational",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }
                }
                ,
                {

                    data: "financial",
                    name: "Financial",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                }
                ,
                {

                    data: "compliance",
                    name: "Compliance",
                    "width": "5%",
                    render: function (data) {
                        if (data) {
                            return '✔';

                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

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


        $("#HighLevelReportIndexList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        $("#HighLevelReportIndexList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        return dataTable;

    }



    return {
        init: init
    }


}(CommonService, HighLevelReportService);