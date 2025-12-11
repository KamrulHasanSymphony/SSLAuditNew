var AuditApproveStatusController = function (CommonService, AuditApproveStatusService) {

    var init = function () {
     
        $(".chkAll").click(function () {
            $('.dSelected:input:checkbox').not(this).prop('checked', this.checked);
        });


        var indexTable = AuditTable();
        var SelfAuditindexTable = SelfAuditTable();

        var auditStatusTable = AuditStatusTable();

    }

    $('#MultipleAA').on('click', function () {

        Confirmation("Are you sure? Do You Want to Approve Multiple Audit?", function (result) {
            console.log(result);
            if (result) {

                SelectData(true);
            }
        });

    });

    function SelectData(IsApprove) {

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
       
        if (IsApprove) {

            AuditApproveStatusService.MultipleAuditApproval(model, MultipleAuditApprovalDone, MultipleAuditApprovalFail);
        }

    }

    function MultipleAuditApprovalDone(result) {
        console.log(result.message);

        if (result.status == "200") {


            ShowNotification(1, result.message);
            var dataTable = $('#AuditStatusList').DataTable();
            dataTable.draw();

        }
        else if (result.status == "400") {
            ShowNotification(3, result.message);
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message);
        }
    }

    function MultipleAuditApprovalFail(result) {
        ShowNotification(3, "Something gone wrong");
        var dataTable = $('#AuditStatusList').DataTable();
        dataTable.draw();

    }


    var AuditStatusTable = function () {

        $('#AuditStatusList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#AuditStatusList thead');


        var dataTable = $("#AuditStatusList").DataTable({
            orderCellsTop: true,
            fixedHeader: true,
            serverSide: true,
            "processing": true,
            "bProcessing": true,
            dom: '<"pagination-left"lBfrtip>',
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
                url: '/Audit/_auditStatusIndex',
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {

                            "indexsearch": $("#Branchs").val(),
                            "branchid": $("#CurrentBranchId").val(),

                            "code": $("#md-Code").val(),
                            "name": $("#md-Name").val(),
                            "branchName": $("#md-BranchName").val(),
                            "approvalStatus": $("#md-ApprovalStatus").val()

                        });
                }
            },
            columns: [

                {
                    data: "id",
                    render: function (data) {

                        return "<a href='/Audit/Edit/" + data + "?edit=preview' class='edit btn btn-primary btn-sm' title='Preview'><i class='fas fa-chart-bar' data-toggle='tooltip' title='Preview'></i></a>"
                            ;


                    },
                    "width": "10%",
                    "orderable": false
                },
                {
                    data: "code",
                    name: "Code",
                    "width": "8%",

                }
                ,
                {
                    data: "name",
                    name: "Name",
                    "width": "8%",

                }
                ,
                {
                    data: "approvalStatus",
                    name: "ApprovalStatus",
                    "width": "8%",

                }

                ,
                {
                    data: "branchName",
                    name: "BranchName",
                    "width": "8%"

                }
                ,
                {
                    data: "auditStatus",
                    name: "AuditStatus",
                    "width": "8%"

                },
                {
                    data: "startDate",
                    name: "StartDate",
                    "width": "8%"

                }
                ,
                {
                    data: "endDate",
                    name: "EndDate",
                    "width": "10%"

                },
                {

                    data: "isPlaned",
                    name: "IsPlaned",
                    "width": "8%",

                    render: function (data) {
                        if (data) {
                            return '✔';
                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }                 

                },
                {
                    data: "isApprovedL4",
                    name: "IsApprovedL4",
                    "width": "8%",

                    render: function (data) {
                        if (data) {
                            return '✔';
                        } else {
                            return '<span class="red-icon" style="font-size: 16px;"><b>✘</b></span>';
                        }
                    }

                },               
                {
                    data: "isCompleteIssueTeamFeedback",
                    name: "IsCompleteIssueTeamFeedback",
                    "width": "8%",


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
                    data: "isCompleteIssueBranchFeedback",
                    name: "IsCompleteIssueBranchFeedback",
                    "width": "8%",

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
                    data: "isPost",
                    name: "IsPost",
                    "width": "10%"

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

                }

                else if ($(cell).hasClass('status')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Approved</option><option>Reject</option></select>');

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




        $("#AuditStatusList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#AuditStatusList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }



    $('.RejectSubmit').click('click', function () {


        RejectedComments = $("#RejectedComments").val();

        var auditreject = serializeInputs("frm_Audit");

        auditreject["RejectedComments"] = RejectedComments;

        Confirmation("Are you sure? Do You Want to Reject Data?", function (result) {
            if (RejectedComments === "" || RejectedComments === null) {
                ShowNotification(3, "Please Write down Reason Of Reject");
                $("#RejectedComments").focus();
                return;
            }

            if (result) {

                var edit = $("#Edit").val();
                if (edit == "issueApprove") {
                    var IssueIDs = [];
                    var $Items = $(".dSelected:input:checkbox:checked");
                    if ($Items == null || $Items.length == 0) {
                        ShowNotification(3, "You are requested to Select checkbox!");
                        return;
                    }
                    $Items.each(function () {
                        var ID = $(this).attr("data-Id");
                        IssueIDs.push(ID);
                    });
                    var model = {
                        IssueIDs: IssueIDs,
                    }
                    auditreject.IssueIDs = model.IssueIDs;

                }


                var id = $("#Id").val();
                auditreject.Id = id;

                auditreject.IDs = auditreject.Id;
                AuditApproveStatusService.AuditMultipleRejectData(auditreject, AuditMultipleReject, AuditMultipleUnRejectFail);


            }
        });
    });

    function AuditMultipleReject(result) {
        console.log(result.message);

        if (result.status == "200") {
            ShowNotification(1, "Data Reject Successfully");

            $("#IsPost").val('N');

            $("#divReasonOfUnPost").hide();
            $(".btnUnPost").hide();



            //change of button
            $(".btnReject").show();
            $(".btnApproved").show();
            //end

            var dataTable = $('#AuditList').DataTable();

            dataTable.draw();

            $('#modal-defaultReject').modal('hide');


            //ForHidingRejectAndApproved
            $(".btnReject, .ApprovedSubmit").hide();
            var container = $(".replaceData");
            container.html('<button type="button" class="button sslPush btnPushed">Already Rejected</button>');

        }
        else if (result.status == "400") {
            ShowNotification(3, result.message); // <-- display the error message here
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message); // <-- display the error message here
        }
    }

    function AuditMultipleUnRejectFail(result) {
        ShowNotification(3, "Something gone wrong");
        var dataTable = $('#AuditList').DataTable();

        dataTable.draw();
    }


    var isReject = $("#IsRejected").val();
    var issueIsRejected = $("#IssueIsRejected").val();
    var bFIsRejected = $("#BFIsRejected").val();
    var edit = $("#Edit").val();

    if (bFIsRejected == "True" && edit == "branchFeedbackApprove") {

        $(".btnReject, .ApprovedSubmit").hide();
        var container = $(".replaceData");
        container.html('<button type="button" class="button sslPush btnPushed">Already Rejected</button>');


    }
    if (issueIsRejected == "True" && edit == "issueApprove") {

        $(".btnReject, .ApprovedSubmit").hide();
        var container = $(".replaceData");
        container.html('<button type="button" class="button sslPush btnPushed">Already Rejected</button>');


    }
    if (isReject == "True" && edit == "auditStatus") {

        $(".btnReject, .ApprovedSubmit").hide();
        var container = $(".replaceData");
        container.html('<button type="button" class="button sslPush btnPushed">Already Rejected</button>');


    }

    $('.ApprovedSubmit').click('click', function () {



        CommentsL1 = $("#CommentsL1").val();

        var auditapprove = serializeInputs("frm_Audit");

        auditapprove["CommentsL1"] = CommentsL1;

        Confirmation("Are you sure? Do You Want to Approve Data?", function (result) {
            if (CommentsL1 === "" || CommentsL1 === null) {
                ShowNotification(3, "Please Write down Reason Of Approved");
                $("#CommentsL1").focus();
                return;
            }

            if (result) {

                var edit = $("#Edit").val();
                if (edit == "issueApprove") {

                    var IssueIDs = [];
                    var $Items = $(".dSelected:input:checkbox:checked");

                    if ($Items == null || $Items.length == 0) {
                        ShowNotification(3, "You are requested to Select checkbox!");
                        return;
                    }

                    $Items.each(function () {
                        var ID = $(this).attr("data-Id");
                        IssueIDs.push(ID);
                    });

                    var model = {
                        IssueIDs: IssueIDs,

                    }
                    auditapprove.IssueIDs = model.IssueIDs;
                }

                auditapprove.IDs = auditapprove.Id;
                AuditApproveStatusService.AuditMultipleApprovedData(auditapprove, AuditMultipleApproved, AuditMultipleApprovedFail);

            }
        });
    });


    function AuditMultipleApproved(result) {
        console.log(result.message);

        if (result.status == "200") {

            ShowNotification(1, "Audit Has Been Approved Successfully");
            $("#IsPost").val('N');
            $("#divReasonOfUnPost").hide();
            $(".btnUnPost").hide();
            $(".btnReject").show();
            $(".btnApproved").show();

            var dataTable = $('#AuditList').DataTable();

            dataTable.draw();

            $('#modal-defaultApproved').modal('hide');
            $(".btnReject, .ApprovedSubmit").hide();

            var container = $(".replaceData");
            container.html('<button type="button" class="button sslPush btnPushed">Already Approved</button>');




        }
        else if (result.status == "400") {
            ShowNotification(3, result.message); 
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message);
            
        }
    }

    function AuditMultipleApprovedFail(result) {

        var post = $("#IsPost").val();
        var emil = result.SingleValue;       
        ShowNotification(3, "You Have Already Approved It.Your Approved Premission Is Over");

        var dataTable = $('#AuditList').DataTable();

        dataTable.draw();
    }





    $('#modelClose').click('click', function () {

        $("#UnPostReason").val("");
        $('#modal-default').modal('hide');


    });


    var AuditTable = function () {

        $('#ApproveStatusList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#ApproveStatusList thead');


        var dataTable = $("#ApproveStatusList").DataTable({
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
                url: '/Audit/_approveStatusIndex',
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {
                            "indexsearch": $("#Branchs").val(),
                            "branchid": $("#CurrentBranchId").val(),

                            "code": $("#md-Code").val(),
                            "name": $("#md-Name").val(),
                            "description": $("#md-Description").val(),
                            "approveStatus": $("#md-ApproveStatus").val(),
                            "ispost": $("#md-Post").val(),


                            "ponumber": $("#md-PONumber").val(),
                            "ispost": $("#md-Post").val(),
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
                     
                        return "<a href=/Audit/Edit/" + data + "?edit=auditStatus class='edit btn btn-primary btn-sm' ><i class='fas fa-check tick-icon' data-toggle='tooltip' title='' data-original-title='Audit'></i></a>  "


                            + "<input onclick='CheckAll(this)' class='dSelected' type='checkbox' data-Id=" + data + " >"

                          
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
                    data: "name",
                    name: "Name"

                }


                ,
                {
                    data: "approveStatus",
                    name: "ApproveStatus"

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

                }

                else if ($(cell).hasClass('status')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>0</option><option>1</option><option>2</option><option>3</option><option>4</option><option>R</option></select>');

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




        $("#ApproveStatusList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#ApproveStatusList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }

    var SelfAuditTable = function () {

        $('#SelfApproveStatusList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#SelfApproveStatusList thead');

        var Status = 'self';

        var dataTable = $("#SelfApproveStatusList").DataTable({
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
                url: '/Audit/_selfapproveStatusIndex',              
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {
                            "indexsearch": $("#Branchs").val(),
                            "branchid": $("#CurrentBranchId").val(),

                            "code": $("#md-Code").val(),
                            "name": $("#md-Name").val(),
                            "description": $("#md-Description").val(),
                            "approveStatus": $("#md-ApproveStatus").val(),
                            "ispost": $("#md-Post").val(),


                            "ponumber": $("#md-PONumber").val(),
                            "ispost": $("#md-Post").val(),
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

                        return "<a href=/Audit/Edit/" + data + "?edit=auditSelfApprove class='edit btn btn-primary btn-sm' ><i class='fas fa-file-invoice' data-toggle='tooltip' title='' data-original-title='Audit'></i></a>  "


                          
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
                    data: "name",
                    name: "Name"

                }

                ,
                {
                    data: "approveStatus",
                    name: "ApproveStatus"

                }
                ,
                {
                    data: "approveStatus",
                    name: "ApproveStatus"

                }
                ,
                {
                    data: "approveStatus",
                    name: "ApproveStatus"

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

                }

                else if ($(cell).hasClass('status')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>0</option><option>1</option><option>2</option><option>3</option><option>4</option><option>R</option></select>');

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


        $("#SelfApproveStatusList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#SelfApproveStatusList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }




    return {
        init: init

    }


}(CommonService, AuditApproveStatusService);