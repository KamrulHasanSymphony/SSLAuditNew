var AuditController = function (AuditService, AuditIssueService, AuditFeedbackService) {

    var indexTable;
    var detailTable;
    var detailTableGDIC;

    var detailTablePoint;

    var detailIssueTable;
    var detailFeedbackTable;
    var detailBranchFeedbackTable;
    var auditUserTable;
    var detailAuditResponseTable;
    var CompanyCode = "";

    var init = function () {
        debugger;
        showSections();
        var show = $("#Edit").val();

        CompanyCode = $("#CompanyCode").val();

        $(".collapse").removeClass("show");
        if (show == "issue") {
            var element = document.getElementById("collapseOne");
            element.classList.add("show");
        }
        if (show == "feedback") {
            var element2 = document.getElementById("collapseTwo");
            element2.classList.add("show");
        }
        if (show == "Branchfeedback") {
            var element = document.getElementById("collapseThree");
            element.classList.add("show");
        }

        //===================
        //AuditIndex
        //===================
        if ($("#AuditList").length) {
            var indexConfig = GetIndexTable();
            indexTable = $("#AuditList").DataTable();
            //indexTable = $("#AuditList").DataTable(indexConfig);
        }

        if ($("#AuditTypeId").length) {
            LoadCombo("AuditTypeId", '/Common/GetAuditType?isPlanned=' + $('#IsPlaned').prop('checked'));
        }

        if ($("#ReportStatus").length) {
            LoadCombo("ReportStatus", '/Common/GetReportStatus');
        }

        if ($("#AuditStatus").length) {
            LoadCombo("AuditStatus", '/Common/GetAuditStatus');
        }

        if ($("#BranchID").length) {
            LoadCombo("BranchID", '/Common/Branch');
        }

        if ($("#TeamId").length) {
            LoadCombo("TeamId", '/Common/GetTeams');
        }

        $("#IsPlaned").on("change", function () {
            LoadCombo("AuditTypeId", '/Common/GetAuditType?isPlanned=' + $('#IsPlaned').prop('checked'));
        })

        if ($("#AuditTemplateId").length) {
            LoadCombo("AuditTemplateId", '/Common/AuditTemplate');
        }

        debugger;
        //===========================
        //GDICAreaTable 
        //===========================
        debugger;
        if (CompanyCode.toUpperCase() === "GDIC") {
            if ($("#GDICAuditAreasDetails").length) {
                debugger;
                var tableConfigs = getGDICTableConfig()
                detailTable = $("#GDICAuditAreasDetails").DataTable(tableConfigs);
                
            }
        }
        

        //AreaTable
        if (CompanyCode.toUpperCase() === "BRAC") {
            if ($("#AuditAreasDetails").length) {
                debugger;
                var tableConfigs = getTableConfig()
                detailTable = $("#AuditAreasDetails").DataTable(tableConfigs);
            }
        }

        //===============================
        //GDICIssueIndex
        //===============================
        if (CompanyCode.toUpperCase() === "GDIC") {
            if ($("#AuditIssueDetails").length) {
                var tableConfigs = getIssueTableConfigGDIC()
                detailIssueTable = $("#AuditIssueDetails").DataTable();               
            }
        }

        //===============================
        //BRACIssueIndex
        //===============================
        if (CompanyCode.toUpperCase() === "BRAC") {
            if ($("#AuditIssueDetails").length) {
                var tableConfigs = getIssueTableConfig()
                detailIssueTable = $("#AuditIssueDetails").DataTable();
                //detailIssueTable = $("#AuditIssueDetails").DataTable(tableConfigs);
            }
        }

        //Feedback
        if ($("#AuditFeedbackDetails").length) {
            var tableConfigs = getFeedbackTableConfig()
            detailFeedbackTable = $("#AuditFeedbackDetails").DataTable(tableConfigs);
        }

        //Branchfeedback
        if ($("#AuditBranchFeedbackDetails").length) {
            var tableConfigs = getBranchFeedbackTableConfig()
            //detailIssueTable = $("#AuditBranchFeedbackDetails").DataTable();
            detailBranchFeedbackTable = $("#AuditBranchFeedbackDetails").DataTable(tableConfigs);
        }

        //AuditResponse

        var indexTabledata = AuditResponseTable();

        //AuditUser Email
        if ($("#AuditUserDetails").length) {
            var tableConfigs = getAuditUserTableConfig()
            auditUserTable = $("#AuditUserDetails").DataTable(tableConfigs);
        }


        //Add Point

        if ($("#AuditPointList").length) {
            debugger;
            var tableConfigs = getTableConfigPoint()
            detailTablePoint = $("#AuditPointList").DataTable(tableConfigs);
        }

        $("#AuditPointList").on("click", ".js-AuditPointList", function () {
            rowAuditPointList($(this).data('id'), $("#Edit").val());
        });

        function rowAuditPointList(id, Edit) {
            AuditService.auditPointModal({ Id: id, Edit }, (result) => { onAuditPointAdd(result, detailTable) }, null, null, () => { detailTable.draw() });
            //AuditService.auditPointModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAuditPointAdd(result, detailTablePoint) }, null, null, () => { detailTablePoint.draw() }, { auditId: $("#Id").val() });
        }
        //End


        $(".btnAddDetails").on("click", function () {
            rowAdd(detailTable)
        });

        $("#AuditAreasDetails").on("click", ".js-delete", function () {
            var button = $(this);
            rowDelete(button, detailTable);
        });

        $("#AuditUserDetails").on('click', '.Audituserdelete', function () {
            $(this).closest('tr').remove();
        });

        $("#AuditAreasDetails").on("click", ".js-edit", function () {
            rowEdit($(this).data('id'), $("#Edit").val());
        });


        //GDIC
        debugger;
        if (CompanyCode.toUpperCase() === "GDIC") {
            $("#GDICAuditAreasDetails").on("click", ".js-edit", function () {
                rowEdit_GDIC($(this).data('id'), $("#Edit").val());
            });
        }

        //SaveAudit

        $(".btnSave").on("click", function (e) {
            Save(detailTable);
        })

        //SendEmail
        $(".btnSendEmail").on("click", function (e) {
            SendEmailSave();
        })

        //Excel
        $(".btnExcelSave").on("click", function (e) {
            btnExcelSave();
        })

        //DbUpdate
        $(".DbUpdateSave").on("click", function (e) {
            debugger;
            DbUpdateSave();
        })

        //SeeAll
        $("#addSeeAll").on("click", function (e) {
            AuditService.seeAllModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onSeeAll(result, detailTable) }, null, (res) => console.log(res), () => { detailTable.draw() });
        })

        //ReportHeading
        $("#addReportHeading").on("click", function (e) {
            AuditService.auditReportModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAuditReportHeadingAdd(result, detailTable) }, null, (res) => console.log(res), () => { detailTable.draw() });
        })

        //SecondReportHeading
        $("#addSecondReportHeading").on("click", function (e) {
            AuditService.auditSecondReportModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAuditSecondReportHeadingAdd(result, detailTable) }, null, (res) => console.log(res), () => { detailTable.draw() });
        })

        //AddAnnexure
        $("#addAnnexure").on("click", function (e) {
            AuditService.annexureReportModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAnnexureAdd(result, detailTable) }, null, (res) => console.log(res), () => { detailTable.draw() });
        })

        //AddAuditReportUsersEmil
        $("#addReportUserEmail").on("click", function (e) {
            AuditService.AuditReportUserModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAuditReportUserAdd(result, detailTable) }, null, (res) => console.log(res), () => { detailTable.draw() });
        })

        //GDIC
        $("#addGDICArea").on("click", function (e) {
            AuditService.auditGDICAreaModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onGDICAdd(result, detailTable) }, null, (res) => console.log(res), () => { detailTable.draw() });          
        })

        //Area Add
        $("#addArea").on("click", function (e) {

            AuditService.auditAreaModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAdd(result, detailTable) }, null, (res) => console.log(res), () => { detailTable.draw() });
        })

        //Audit Issue
        $("#addIssue").on("click", function (e) {

            AuditService.auditIssueModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (auditUserTable) => { onIssueAdd(auditUserTable, detailIssueTable) }, null, null, () => { detailIssueTable.draw() });
        })

        //GDIC
        //Audit Issue
        $("#addIssueGDIC").on("click", function (e) {
            AuditService.auditIssueModalGDIC({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (auditUserTable) => { onIssueAddGDIC(auditUserTable, detailIssueTable) }, null, null, () => { detailIssueTable.draw() });
        })

        //==================================
        //FeedbackPouUp+Save
        //==================================
        $("#addFeedback").on("click", function (e) {
            AuditService.auditFeedbackModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onFeedbackAdd(result, detailFeedbackTable) }, null, null, () => { detailFeedbackTable.draw() }, { auditId: $("#Id").val() });
            detailFeedbackTable.draw();
        })

        //====================================
        //BranchFeedbackPopUp+Save
        //====================================
        $("#addBranchFeedback").on("click", function (e) {
            AuditService.auditBranchFeedbackModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onBranchFeedbackAdd(result, detailBranchFeedbackTable) }, null, null, () => { detailBranchFeedbackTable.draw() }, { auditId: $("#Id").val() });
        })

        //AuditUser Email
        $("#addAuditUser").on("click", function (e) {
            AuditService.auditUserModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAuditUserAdd(result, auditUserTable) }, null, null, () => { auditUserTable.draw() }, { auditId: $("#Id").val() });
        })

        //IssueEmail
        $("#addAuditIssueUser").on("click", function (e) {
            AuditService.auditUserModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAuditUserAdd(result, auditUserTable) }, null, null, () => { auditUserTable.draw() }, { auditId: $("#Id").val() });
        })

        //Add Point

        $("#addAuditPoint").on("click", function (e) {
            debugger;
            AuditService.auditPointModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAuditPointAdd(result, detailTablePoint) }, null, null, () => { detailTablePoint.draw() }, { auditId: $("#Id").val() });
        })

        //End

        $("#AuditList").on("click", ".auditStatusLineEdit", function () {
            auditStatusEdit($(this).data('id'), $("#Edit").val());
        });

        //IssueIconClick
        $("#AuditIssueDetails").on("click", ".issueLineEdit", function () {
            IssueEdit($(this).data('id'), $("#Edit").val());
        });

        //GDIC
        //IssueIconClick
        $("#AuditIssueDetails").on("click", ".issueLineEditGDIC", function () {
            IssueEditGDIC($(this).data('id'), $("#Edit").val());
        });

        $("#AuditIssueDetails").on("click", ".reportStatus", function () {
            ReportStatusEdit($(this).data('id'), $("#Edit").val());
        });

        //AuditStatus
        $("#AuditList").on("click", ".auditStatus", function () {
            auditStatusEdit($(this).data('id'), $("#Edit").val());
        });

        $("#AuditFeedbackDetails").on("click", ".feedbackLineEdit", function () {
            feedbackEdit($(this).data('id'), $("#Edit").val());
        });
        //BranchFeedback
        $("#AuditBranchFeedbackDetails").on("click", ".BranchfeedbackLineEdit", function () {
            BranchfeedbackEdit($(this).data('id'), $("#Edit").val());
        });

        $("#AuditUserDetails").on("click", ".auditEdit", function () {
            auditUserEdit($(this).data('id'), $("#Edit").val());
        });

        $("#AuditIssueUserDetails").on("click", ".auditEdit", function () {
            auditUserEdit($(this).data('id'), $("#Edit").val());
        });

        $(".chkAll").click(function () {
            $('.dSelected:input:checkbox').not(this).prop('checked', this.checked);
        });

        var IsPost = $('#IsPost').val();

        var edit = $('#Edit').val();

        if (edit === 'feedback') {
            Visibility(true);
        }

        progressBar()

        if ($("#downloadButton").length) {

            var downloadButton = document.getElementById('downloadButton');
            downloadButton.addEventListener('click', function () {
                DownLoadData();
            });

        }


        //function Visibility(action) {
        //    $('#frm_AREntry').find(':input').prop('readonly', action);
        //    $('#frm_AREntry').find('table, table *').prop('disabled', action);
        //    $('#frm_AREntry').find(':input[type="button"]').prop('disabled', action);
        //    $('#frm_AREntry').find(':input[type="checkbox"]').prop('disabled', action);
        //};

    }

    //================================
    //EndOfInit
    //================================

    //BranchManuShowOrNot
    debugger;
    let BranchUser = $("#IsBranchUser").val();
    if (BranchUser === "True") {

        $("#frm_Audit").hide();
        $("#auditcreationId").hide();
    }

    function DbUpdateSave() {
        debugger;

        var masterObj = {};

        AuditService.DbUpdatesaveSave(masterObj, DbUpdatesaveDone, DbUpdatesaveFail);

    }

    function DbUpdatesaveDone(result) {

        if (result.status == "200") {

            console.log(result)
            ShowNotification(1, result.message);

        }
        else if (result.status == "400") {
            ShowNotification(3, result.message || result.error);
        }
    }

    function DbUpdatesaveFail(result) {
        console.log(result);
        ShowNotification(3, result.message);
    }

    //PendingAuditApproval
    $('#AuditList').on('click', '.displayInfoPendingAuditApproval', function () {
        debugger;
        var rowData = indexTable.row($(this).closest('tr')).data();
        var id = rowData.id;

        var masterObj = {
            Id: id
        };

        AuditService.PendingAuditApproval(masterObj, PendingAuditApprovalDone, PendingAuditApprovalFail);

    });

    function PendingAuditApprovalDone(result) {

        if (result.status == "200") {

            ShowNotification(1, "Email Has Been Sent Successfully");
        }
        else {
            ShowNotification(3, "You Have To Add Email First");
        }

    }

    function PendingAuditApprovalFail(result) {
        console.log(result);
        ShowNotification(3, "Item Is Not Found");

    }

    //ReportCheckBox

    $('#btnReportCheck').on('click', function () {

        Confirmation("Are you sure? Do You Want to Save?", function (result) {
            console.log(result);
            if (result) {
                SelectReportData(true);
            }
        });

    });

    function SelectReportData(IsReport) {
        debugger;
        var IDs = [];
        var $Items = $(".dSelected:input:checkbox");

        if ($Items == null || $Items.length == 0) {
            ShowNotification(3, "You are requested to Select checkbox!");
            return;
        }

        $Items.each(function () {
            var ID = $(this).attr("data-Id");
            var isCheck = this.checked;
            IDs.push(ID + ":" + isCheck);
        });

        var model = {
            IDs: IDs,

        }
        debugger;
        if (IsReport) {

            AuditService.ReportDataUpdate(model, ReportDataSaveDone, ReportDataSaveFail);

        }

    }

    function ReportDataSaveDone(result) {
        console.log(result.message);
        if (result.status == "200") {
            ShowNotification(1, result.message);
            var dataTable = $('#AuditBranchFeedbackDetails').DataTable();
            dataTable.draw();
        }
        else if (result.status == "400") {
            ShowNotification(3, result.message);
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message);
        }
    }

    function ReportDataSaveFail(result) {
        ShowNotification(3, "Something gone wrong");
        var dataTable = $('#AuditBranchFeedbackDetails').DataTable();
        dataTable.draw();

    }

    //EndOfReportDataSave

    function DownLoadData() {
        debugger;

        var id = $("#Id").val();
        var url = '/Report/AuditeeResponse?id=' + id;
        var win = window.open(url, '_blank');
    };



    //ForClossingModalByPressingEsc

    $(document).keydown(function (event) {
        if (event.key === 'Escape') {
            console.log('Escape key pressed!');

            var value = $("#Edit").val();

            try {
                if ($('#FeedbackModal').is(':visible')) {
                    $('#FeedbackModal').modal('hide');
                }

                if ($('#IssueModal').is(':visible')) {

                    $('#IssueModal').modal('hide');
                }

                if ($('#BranchFeedbackModal').is(':visible')) {

                    $('#BranchFeedbackModal').modal('hide');
                }
                if ($('#AuditIssuePreview').is(':visible')) {

                    $('#AuditIssuePreview').modal('hide');
                }

            } catch (error) {
                console.error('Error closing modal:', error);
            }
        }
    });

    function progressBar() {

    }

    function Visibility(action) {

    };


    //IssueCompletedOperation

    var issueComplete = $("#IssueCompleted").val();
    if (issueComplete == "True") {
        //$("#addIssue").hide();
    }


    $('#IssueComplete').on('click', function () {

        Confirmation("Are you sure? Do You Want Issue Complete?", function (result) {
            console.log(result);
            if (result) {

                var IssueCom = $("#IssueCompleted").val();
                if (IssueCom == "True") {
                    ShowNotification(3, "Data Is Already Saved");
                    return false;
                }

                var IssueCompleted = $("#IssueCompletedCheck").is(":checked");

                if (IssueCompleted == false) {

                    ShowNotification(2, "You Have to Check First");

                    return false;
                }

                SelectIssueComplete(true);
            }
        });

    });


    function SelectIssueComplete(isIssue) {

        var id = $("#Id").val();
        var model = {};
        model.IssueCompleted = isIssue;
        model.Id = id;

        AuditService.AuditIssueComplete(model, AuditIssueCompleteDone, AuditIssueCompleteFail);

    }

    function AuditIssueCompleteDone(result) {
        console.log(result.message);

        if (result.status == "200") {

            ShowNotification(1, result.message);
            var dataTable = $('#AuditList').DataTable();
            dataTable.draw();
            $("#addIssue").hide();

            $("#addIssueGDIC").hide();

        }
        else if (result.status == "400") {
            ShowNotification(3, result.message);
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message);
        }
    }

    function AuditIssueCompleteFail(result) {
        ShowNotification(3, "Something gone wrong");
        var dataTable = $('#AuditList').DataTable();
        dataTable.draw();

    }

    //EndOfIssueComplete


    //FeedbackCompleteOperation
    var feedbackComplete = $("#FeedbackCompleted").val();
    if (feedbackComplete == "True") {
        $("#addFeedback").hide();

    }

    $('#CompleteIssueTeamFeedback').on('click', function () {

        Confirmation("Are you sure? Do You Want Feedback Complete?", function (result) {
            console.log(result);
            if (result) {

                var Issuefeedback = $("#FeedbackCompleted").val();
                if (Issuefeedback == "True") {
                    ShowNotification(3, "Data Is Already Saved");
                    return false;
                }

                var FeedbackCompleted = $("#ckIsCompleteIssueTeamFeedback").is(":checked");

                if (FeedbackCompleted == false) {

                    ShowNotification(2, "You Have to Check First");
                    return false;
                }
                SelectFeedbackCompletedCompleted(true);
            }
        });

    });

    function SelectFeedbackCompletedCompleted(isFeedback) {

        var id = $("#Id").val();
        var model = {};
        model.IsCompleteIssueTeamFeedback = isFeedback;
        model.Id = id;
        AuditService.AuditFeedbackComplete(model, AuditFeedbackCompletedDone, AuditFeedbackCompletedFail);

    }

    function AuditFeedbackCompletedDone(result) {
        console.log(result.message);

        if (result.status == "200") {

            ShowNotification(1, result.message);
            var dataTable = $('#AuditList').DataTable();
            dataTable.draw();

        }
        else if (result.status == "400") {
            ShowNotification(3, result.message);
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message);
        }
    }

    function AuditFeedbackCompletedFail(result) {
        ShowNotification(3, "Something gone wrong");
        var dataTable = $('#AuditList').DataTable();
        dataTable.draw();
    }


    //baranchFeedbackOperation
    debugger;
    var branchFeedbackComplete = $("#BranchFeedbackCompleted").val();
    if (branchFeedbackComplete === "True") {
        $("#addBranchFeedback").hide();
    }

    $('#CompleteBranchFeedback').on('click', function () {

        Confirmation("Are you sure? Do You Want BranchFeedback Complete?", function (result) {
            console.log(result);
            if (result) {

                var Issuebranchfeedback = $("#BranchFeedbackCompleted").val();
                if (Issuebranchfeedback == "True") {
                    ShowNotification(3, "Data Is Already Saved");
                    return false;
                }

                var BranchFeedbackCompleted = $("#ckBranchFeedbackCompleted").is(":checked");
                if (BranchFeedbackCompleted == false) {

                    ShowNotification(2, "You Have to Check First");
                    return false;

                }

                SelectBranchFeedbackCompleted(true);
            }
        });

    });

    function SelectBranchFeedbackCompleted(isBranchFeedback) {

        var id = $("#Id").val();
        var model = {};
        model.BranchFeedbackCompleted = isBranchFeedback;
        model.Id = id;
        AuditService.AuditBranchFeedbackComplete(model, AuditBranchFeedbackCompletedDone, AuditBranchFeedbackCompletedFail);

    }

    function AuditBranchFeedbackCompletedDone(result) {
        console.log(result.message);

        if (result.status == "200") {

            ShowNotification(1, result.message);
            var dataTable = $('#AuditList').DataTable();
            dataTable.draw();

        }
        else if (result.status == "400") {
            ShowNotification(3, result.message);
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message);
        }
    }

    function AuditBranchFeedbackCompletedFail(result) {
        ShowNotification(3, "Something gone wrong");
        var dataTable = $('#AuditList').DataTable();
        dataTable.draw();

    }


    //HOD

    $('#btnSendToHOD').click('click', function () {

        Confirmation("Are you sure? Do You Want to Send To HOD?", function (result) {
            console.log(result);
            if (result) {

                var id = $("#Id").val();
                var model = {};
                model.IsHOD = true;
                model.Id = id;

                AuditService.SendToHOD(model, HODDoneEmail, HODFail);

            }
        });

    });

    function HODDoneEmail(result) {
        if (result.status == "200") {

            ShowNotification(1, "Email has been sent successfully");

        }
        else if (result.status == "400") {
            ShowNotification(3, "Something gone wrong");
        }
    }
    function HODFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }


    $('#saveMultipleIssues').on('click', function () {

        Confirmation("Are you sure? Do You Want to Save Rest Of Issues?", function (result) {
            console.log(result);
            if (result) {

                SaveAllIssues();
            }
        });

    });

    function SaveAllIssues() {

        debugger;
        var auditId = $("#Id").val();
        var masterObj = {
            Id: auditId,
        }

        AuditService.MultipleIssueSave(masterObj, MultipleIssueSaveDone, MultipleIssueSaveFail);

    }

    function MultipleIssueSaveDone(result) {

        console.log(result.message);

        if (result.status == "200") {

            ShowNotification(1, result.message);

        }
        else if (result.status == "400") {
            ShowNotification(3, result.message);
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message);
        }
        else {
            ShowNotification(3, "All Items Are Saved Already");

        }
    }

    function MultipleIssueSaveFail(result) {
        ShowNotification(3, "Something gone wrong");
    }

    $('#PostAU').on('click', function () {

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

        var dataTable = $('#AuditList').DataTable();

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

            AuditService.AuditMultiplePost(model, AuditMultiplePosts, AuditMultiplePostFail);

        }
    }

    function showSections() {
        if ($("#allSections").is(':hidden') && $("#Id").val() != 0) {
            $("#allSections").show();
        }
    }

    function getIssueIndexURL() {
        return '/AuditIssue/_index?id=' + $("#Id").val()
    }

    function getAreaIndexURL() {
        return '/Audit/_indexArea?id=' + $("#Id").val()
    }

    function getAreaIndexURL_GDIC() {
        debugger;
        return '/Audit/_GDICIndexArea?id=' + $("#Id").val()
    }


    //Add Point
    function getAreaIndexURLPoint() {
        return '/AuditPoints/_index/?id=' + $("#Id").val()
    }
    //End

    function getIssueFeedIndexURL() {
        return '/AuditFeedback/_index?id=' + $("#Id").val()
    }

    function getIssueBranchFeedIndexURL() {
        return '/AuditBranchFeedback/_index?id=' + $("#Id").val()
    }

    function getUserIndexURL() {

        return '/Audit/_indexAuditUser?id=' + $("#Id").val()
    }

    function IssueEdit(id, Edit) {

        AuditService.auditIssueModal({ Id: id, Edit }, (result) => { onIssueAdd(result, detailIssueTable) }, () => { showUserAuditIssue() }, null, () => { detailIssueTable.draw() });
    }

    //GDIC
    function IssueEditGDIC(id, Edit) {
        AuditService.auditIssueModalGDIC({ Id: id, Edit }, (result) => { onIssueAddGDIC(result, detailIssueTable) }, () => { showUserAuditIssue() }, null, () => { detailIssueTable.draw() });
    }

    function ReportStatusEdit(id, Edit) {

        AuditService.auditReportStatusEditModal({ Id: id, Edit }, (result) => { onAuditReportAdd(result, null) }, null, null, null);

    }

    function auditStatusEdit(id, Edit) {

        AuditService.auditStatusModal({ Id: id, Edit }, (result) => { onAuditStatusAdd(result, indexTable) }, null, null, null);

    }

    function feedbackEdit(id, Edit) {
        AuditService.auditFeedbackModal({ Id: id, Edit }, (result) => { onFeedbackAdd(result, detailFeedbackTable) }, null, null, () => { detailFeedbackTable.draw() }, { auditId: $("#Id").val() });
    }

    function BranchfeedbackEdit(id, Edit) {
        AuditService.auditBranchFeedbackModal({ Id: id, Edit }, (result) => { onBranchFeedbackAdd(result, detailFeedbackTable) }, null, null, () => { detailFeedbackTable.draw() }, { auditId: $("#Id").val() });
    }

    function auditUserEdit(id, Edit) {
        AuditService.auditUserModal({ Id: id, Edit }, (result) => { onAuditUserAdd(result, auditUserTable) }, null, null, () => { auditUserTable.draw() });
    }
    function auditUserdelete(id, Edit) {
        AuditService.auditUserDelete({ Id: id, Edit }, (result) => { onAuditUserAdd(result, auditUserTable) }, null, null, () => { auditUserTable.draw() });
    }

    function auditIssueUserEdit(id, Edit) {
        AuditService.auditUserModal({ Id: id, Edit }, (result) => { onAuditUserAdd(result, auditUserTable) }, null, null, () => { auditUserTable.draw() }, { auditId: $("#Id").val() });
    }

    function onAuditStatusAdd(result) {

        var masterObj = $("#frm_Audit_Status").serialize();
        masterObj = queryStringToObj(masterObj);
        var validator = $("#frm_Audit_Status").validate();
        var result = validator.form();
        AuditService.AuditStatus(masterObj, saveDone, saveFail);

    }


    function onAuditReportAdd(auditUserTable, issueTable) {

        var validator = $("#frm_Audit_Report").validate({
            rules: {
                ReportStatusModal: {
                    required: true
                }
            },
            messages: {
                ReportStatusModal: {
                    required: "Please enter the Report Status."
                }
            }
        }
        );

        var result = validator.form();

        if (!result) {
            ShowNotification(2, "Please complete the form");
            return;
        }


        var masterObj = $("#frm_Audit_Report").serialize();
        masterObj = queryStringToObj(masterObj);
        debugger;

        masterObj.IsPlaned = $('#IsPlaned').is(":checked");
        masterObj.ReportStatus = masterObj.ReportStatusModal;

        var validator = $("#frm_Audit_Report").validate();
        var result = validator.form();

        AuditService.ReportStatus(masterObj, saveDone, saveFail);

    }

    function onIssueAdd(auditUserTable, issueTable) {
        var validator = $("#frm_Audit_Issue").validate({
            rules: {
                IssueName: {
                    required: true
                },
                DateOfSubmission: {
                    required: true
                },
                IssuePriority: {
                    required: true
                },

                AuditType: {
                    required: true
                },
                IssueStatus: {
                    required: true
                }
            },
            messages: {
                IssueName: {
                    required: "Please enter the issue name."
                },
                DateOfSubmission: {
                    required: "Please select the date of submission."
                },
                IssuePriority: {
                    required: "Please select the issue priority."
                },
                AuditType: {
                    required: "Please provide the Audit Type."
                },
                IssueStatus: {
                    required: "Please provide the Issue Status."
                }
            }
        });

        var result = validator.form();

        if (!result) {
            ShowNotification(2, "Please complete the form");
            return;
        }

        var InvestigationOrForensis = $("#InvestigationOrForensis").is(":checked");
        var StratigicMeeting = $("#StratigicMeeting").is(":checked");
        var ManagementReviewMeeting = $("#ManagementReviewMeeting").is(":checked");
        var OtherMeeting = $("#OtherMeeting").is(":checked");
        var Training = $("#Training").is(":checked");
        var Operational = $("#Operational").is(":checked");
        var Financial = $("#Financial").is(":checked");
        var Compliance = $("#Compliance").is(":checked");

        debugger;

        $('.issueDetailsTextArea').summernote();
        var summernotes = $('.issueDetailsTextArea').summernote('code');

        var encodedSummernotes = btoa(unescape(encodeURIComponent(summernotes)));

        var form = $("#frm_Audit_Issue")[0];
        var formData = new FormData(form);

        formData.set("IssueDetails", encodedSummernotes);

        AuditIssueService.save(formData, (result) => { saveDoneIssue(result, auditUserTable) }, saveFail);

    };

    //GDIC
    function onIssueAddGDIC(auditUserTable, issueTable) {
        var validator = $("#frm_Audit_Issue").validate({
            rules: {
                IssueName: {
                    required: true
                },
                DateOfSubmission: {
                    required: true
                },
                IssuePriority: {
                    required: true
                },

                AuditType: {
                    required: true
                },
                IssueStatus: {
                    required: true
                }
            },
            messages: {
                IssueName: {
                    required: "Please enter the issue name."
                },
                DateOfSubmission: {
                    required: "Please select the date of submission."
                },
                IssuePriority: {
                    required: "Please select the issue priority."
                },
                AuditType: {
                    required: "Please provide the Audit Type."
                },
                IssueStatus: {
                    required: "Please provide the Issue Status."
                }
            }
        });

        var result = validator.form();

        if (!result) {
            ShowNotification(2, "Please complete the form");
            return;
        }

        var InvestigationOrForensis = $("#InvestigationOrForensis").is(":checked");
        var StratigicMeeting = $("#StratigicMeeting").is(":checked");
        var ManagementReviewMeeting = $("#ManagementReviewMeeting").is(":checked");
        var OtherMeeting = $("#OtherMeeting").is(":checked");
        var Training = $("#Training").is(":checked");
        var Operational = $("#Operational").is(":checked");
        var Financial = $("#Financial").is(":checked");
        var Compliance = $("#Compliance").is(":checked");

        debugger;

        $('.issueDetailsTextArea').summernote();
        var summernotes = $('.issueDetailsTextArea').summernote('code');

        var encodedSummernotes = btoa(unescape(encodeURIComponent(summernotes)));

        var form = $("#frm_Audit_Issue")[0];
        var formData = new FormData(form);

        formData.set("IssueDetails", encodedSummernotes);

        AuditIssueService.save(formData, (result) => { saveDoneIssueGDIC(result, auditUserTable) }, saveFailGDIC);

    };
    function saveDoneIssueGDIC(result, auditIssueUserTable) {

        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnSaveIssue").html('Update');
                $("#IssueId").val(result.data.id);
                result.data.operation = "update";
                $("#IssueOperation").val(result.data.operation);
                $("#SavePost").show();
                addListItem(result);
                auditIssueUserTable.ajax.url('/Audit/_indexAuditIssueUser?id=' + result.data.id);
                showUserAuditIssue();
                console.log(result)
              


            } else {

                addListItem(result);
                ShowNotification(1, result.message);

                debugger;
                setTimeout(function () {
                    $('.sslClose').click();  
                }, 1000);
            }

            $("#fileToUpload").val('');

        }
        else if (result.status == "400") {
            ShowNotification(3, result.message);
        }
    }
    function saveFailGDIC(result) {
        console.log(result);
        ShowNotification(3, result.message);
    }


    function onFeedbackAdd(result) {
        var validator = $("#frm_Audit_feedback").validate({
            rules: {
                AuditIssueId: {
                    required: true
                },
                Heading: {
                    required: true
                },
                IssueDetails: {
                    required: true
                }
            },
            messages: {
                AuditIssueId: {
                    required: "Please select the audit issue."
                },
                Heading: {
                    required: "Please enter the feedback heading."
                },
                IssueDetails: {
                    required: "Please provide the feedback details."
                }
            }
        }
        );
        var result = validator.form();

        if (!result) {
            ShowNotification(2, "Please complete the form");
            return;
        }

        var form = $("#frm_Audit_feedback")[0];
        var formData = new FormData(form);

        $('.issueFeedbackSummerNote').summernote();
        var summernotes = $('.issueFeedbackSummerNote').summernote('code');

        var encodedSummernotes = btoa(unescape(encodeURIComponent(summernotes)));

        formData.set("FeedbackDetails", encodedSummernotes);

        AuditFeedbackService.save(formData, saveDoneFeedback, saveFail);

    }

    function onBranchFeedbackAdd(result) {

        var validator = $("#frm_Audit_Branch_feedback").validate({
            rules: {

                AuditBranchIssueId: {
                    required: true
                },
                IssueDetails: {
                    required: true
                },
                Status: {
                    required: true
                },

                IssueStatus: {
                    required: true
                }

            },
            messages: {

                AuditBranchIssueId: {
                    required: "Please select the audit issue."
                },

                IssueDetails: {
                    required: "Please provide the feedback details."
                },
                IssueStatus: {
                    required: "Please provide the Issue Status."
                }
            }
        }
        );

        var result = validator.form();

        if (!result) {
            ShowNotification(2, "Please complete the form");
            return;
        }

        debugger;
        var IssueStatus = $("#IssueStatus").val();

        var form = $("#frm_Audit_Branch_feedback")[0];
        var formData = new FormData(form);

        $('.issueBranchFeedbackSummerNote').summernote();
        var summernotes = $('.issueBranchFeedbackSummerNote').summernote('code');

        var encodedSummernotes = btoa(unescape(encodeURIComponent(summernotes)));

        formData.set("BranchFeedbackDetails", encodedSummernotes);
        formData.set("IssueStatus", IssueStatus);

        AuditFeedbackService.FeedbackBranchSave(formData, saveDoneBranchFeedback, saveFail);
    }

    function saveDoneBranchFeedback(result) {
        debugger;
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);

                $(".btnBranchSaveFeedback").html('Update');
                $("#BranchfeedbackId").val(result.data.id);
                $("#Id").val(result.data.auditId);
                $("#divFeedbackBranch").show();
                $(".btnPost").show();
                result.data.operation = "update";
                $("#feedbackBranchOperation").val(result.data.operation);

                addListItemBranchFeedBack(result);


            } else {

                addListItemBranchFeedBack(result);
                ShowNotification(1, result.message);
            }

            $("#fileToUpload").val('');

        }
        else if (result.status == "400") {
            ShowNotification(3, result.message);
        }
    }


    function saveFail(result) {
        console.log(result);
        ShowNotification(3, result.message);
    }

    function onAuditUserAdd(result) {
        var validator = $("#frm_Audit_User").validate({
            rules: {

                EmailAddress: {
                    required: true,
                    email: true
                },
                IssuePriority: {
                    required: true
                }


            },
            messages: {

                EmailAddress: {
                    required: "Email address is required",
                    email: "Please enter a valid email address"
                },
                IssuePriority: {
                    required: "Issue Priority is required"
                }

            }
        });

        var result = validator.form();

        if (!result) {
            ShowNotification(2, "Please complete the form");
            return;
        }

        var form = $("#frm_Audit_User")[0];
        var formData = new FormData(form);

        AuditService.saveEmail(formData, saveDoneEmail, saveFail);
    }

    //Add Point

    function onAuditPointAdd(result) {

        debugger;

        var validator = $("#frm_AuditPoint").validate();
        var auditPoint = serializeInputs("frm_AuditPoint");

        var PIdData = $("#PIdData").val();
        var P_LevelData = $("#P_LevelData").val();

        if (auditPoint.WeightPersent > 100) {
            ShowNotification(3, "Weight Persent is over 100");
            return;
        }

        if (PIdData == "xx") {
            ShowNotification(3, "Please Select Correct Option");
            return;
        }

        var result = validator.form();

        if (!result) {
            validator.focusInvalid();
            return;
        }

        auditPoint.PId = PIdData;
        auditPoint.P_Level = P_LevelData;

        AuditPointsService.save(auditPoint, pointSaveDone, pointSaveFail);

    }
    function pointSaveDone(result) {
        debugger;
        if (result.status == "200") {
            if (result.data.operation == "add") {
                console.log(result)

                debugger;

                ShowNotification(1, result.message);
                $(".btnSaveAuditPoint").html('Update');
                $("#Id").val(result.data.id);
                $("#Code").val(result.data.code);
                result.data.operation = "update";
                $("#Operation").val(result.data.operation);

                //$("#TeamValue").val(result.data.teamId);
                //$("#divUpdate").show();
                //$("#divSave").hide();
                //$("#SavePost").show();

                //detailIssueTable.ajax.url(getIssueIndexURL());
                //detailFeedbackTable.ajax.url(getIssueFeedIndexURL());
                //detailTable.ajax.url(getAreaIndexURL());
                //auditUserTable.ajax.url(getUserIndexURL());

                showSections();
                detailTablePoint.draw();


            } else {
                ShowNotification(1, result.message);
                //$("#divSave").hide();
                //$("#divUpdate").show();
            }
        }
        else if (result.status == "400") {
            ShowNotification(3, result.message || result.error);
        }
    }

    function pointSaveFail(result) {
        console.log(result);
        ShowNotification(3, result.message);
    }


    //End

    function addListItem(result) {
        var list = $(".fileGroup");

        result.data.attachmentsList.forEach(function (item) {

            var item = '<li class="list-group-item" id="file-' + item.id + '"> <span>' +
                item.displayName +
                '</span><a target="_blank" href="/AuditIssue/DownloadFile?filePath=' + item.fileName + '" class=" ml-2 btn btn-primary btn-sm float-right ml-2">Download</a>' +
                '<button onclick="AuditController.deleteIssueFile(\'' + item.id + '\', \'' + item.fileName + '\')" class=" ml-2 btn btn-danger btn-sm float-right" type="button">Delete</button>' +
                '</li>';

            list.append(item);
        });
    }


    function addListItemFeedBack(result) {
        var list = $(".fileGroup");

        result.data.attachmentsList.forEach(function (item) {

            var item = '<li class="list-group-item" id="file-' + item.id + '"> <span>' +
                item.displayName +
                '</span><a target="_blank" href="/AuditFeedback/DownloadFile?filePath=' + item.fileName + '" class=" ml-2 btn btn-primary btn-sm float-right ml-2">Download</a>' +
                '<button onclick="AuditController.deleteFeedbackFile(\'' + item.id + '\', \'' + item.fileName + '\')" class=" ml-2 btn btn-danger btn-sm float-right" type="button">Delete</button>' +
                '</li>';

            list.append(item);
        });
    }


    function addListItemBranchFeedBack(result) {
        var list = $(".fileGroup");

        result.data.attachmentsList.forEach(function (item) {

            var item = '<li class="list-group-item" id="file-' + item.id + '"> <span>' +
                item.displayName +
                '</span><a target="_blank" href="/AuditBranchFeedback/DownloadFile?filePath=' + item.fileName + '" class=" ml-2 btn btn-primary btn-sm float-right ml-2">Download</a>' +
                '<button onclick="AuditController.deleteBranchFeedbackFile(\'' + item.id + '\', \'' + item.fileName + '\')" class=" ml-2 btn btn-danger btn-sm float-right" type="button">Delete</button>' +
                '</li>';

            list.append(item);
        });
    }


    function saveDoneIssue(result, auditIssueUserTable) {

        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnSaveIssue").html('Update');
                $("#IssueId").val(result.data.id);
                result.data.operation = "update";
                $("#IssueOperation").val(result.data.operation);
                $("#SavePost").show();
                addListItem(result);
                auditIssueUserTable.ajax.url('/Audit/_indexAuditIssueUser?id=' + result.data.id);
                showUserAuditIssue();
                console.log(result)


            } else {

                addListItem(result);
                ShowNotification(1, result.message);
            }

            $("#fileToUpload").val('');

        }
        else if (result.status == "400") {
            ShowNotification(3, result.message);
        }
    }

    function showUserAuditIssue() {

        if ($("#allSectionsIssueUser").is(':hidden') && $("#Id").val() != 0) {
            $("#allSectionsIssueUser").show();
        }

    }

    function saveDoneArea(result) {
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnSaveArea").html('Update');
                $("#AreaId").val(result.data.id);
                result.data.operation = "update";
                $("#AreaOperation").val(result.data.operation);

            } else {

                ShowNotification(1, result.message);
            }
        }
        else if (result.status == "400") {
            ShowNotification(3, "Something gone wrong");
        }
    }

    function saveDoneFeedback(result) {

        if (result.status == "200") {

            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnSaveFeedback").html('Update');
                $("#feedbackId").val(result.data.id);
                result.data.operation = "update";
                $("#feedbackOperation").val(result.data.operation);
                $("#divFeedback").show();
                $("#SavePost").show();
                showUserAuditIssue();
                addListItemFeedBack(result);

            } else {

                addListItemFeedBack(result);
                ShowNotification(1, result.message);
            }

            $("#fileToUpload").val('');

        }
        else if (result.status == "400") {

            ShowNotification(3, result.message);

        }
    }

    function saveDoneEmail(result) {
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnSaveAuditUser").html('Update');
                $("#AuditUserId").val(result.data.id);
                result.data.operation = "update";
                $("#AuditUserOperation").val(result.data.operation);

            } else {

                ShowNotification(1, result.message);
            }
        }
        else if (result.status == "400") {
            ShowNotification(3, result.message);
        }
    }

    //-------------------------------

    //GDIC
    function onGDICAdd(result, detailTable) {
        debugger;
        var validator = $("#AuditAreas").validate({
            rules: {
                AuditArea: {
                    required: true
                }
            },
            messages: {
                AuditArea: {
                    required: "Audit Area is required."
                }
            }
        });
        var result = validator.form();

        if (!result) {
            return;
        }

        $('.auditAreaSummerNote').summernote();
        var summernotes = $('.auditAreaSummerNote').summernote('code');
        var encodedSummernotes = btoa(summernotes);
        var form = $("#AuditAreas")[0];
        var formData = new FormData(form);
        formData.set("AreaDetails", encodedSummernotes);

        AuditService.saveArea(formData, saveGDICDoneArea, saveGDICFail);

    }
    function saveGDICDoneArea(result) {
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnSaveArea").html('Update');
                $("#AreaId").val(result.data.id);
                result.data.operation = "update";
                $("#AreaOperation").val(result.data.operation);

            } else {

                ShowNotification(1, result.message);
            }
        }
        else if (result.status == "400") {
            ShowNotification(3, "Something gone wrong");
        }
    }
    function saveGDICFail(result) {
        console.log(result);
        ShowNotification(3, result.message);
    }

    function onAdd(result, detailTable) {

        debugger;

        var selectedValue = $("#dynamicDropdown").val();
        $("#P_Mark").val(selectedValue);



        var validator = $("#AuditAreas").validate({
            rules: {
                AuditArea: {
                    required: true
                }
            },
            messages: {
                AuditArea: {
                    required: "Audit Area is required."
                }
            }
        });
        var result = validator.form();

        if (!result) {
            return;
        }

        $('.auditAreaSummerNote').summernote();
        var summernotes = $('.auditAreaSummerNote').summernote('code');
        var encodedSummernotes = btoa(summernotes);
        var form = $("#AuditAreas")[0];
        var formData = new FormData(form);
        formData.set("AreaDetails", encodedSummernotes);

        formData.set("P_Mark", selectedValue);

        AuditService.saveArea(formData, saveDoneArea, saveFail);

    }

    function onSeeAll(result, detailTable) {

        $('.auditSeeAllSummerNote').summernote();
        var summernotes = $('.auditSeeAllSummerNote').summernote('code');
        var encodedSummernotes = btoa(summernotes);
        var form = $("#SeeAllReport")[0];
        var formData = new FormData(form);
        formData.set("AuditSeeAllDetails", encodedSummernotes);
        AuditService.saveSeeAllReport(formData, saveDoneSeeAll, seeAllSaveFail);

    }

    function saveDoneSeeAll(result) {

        if (result.status == "200") {

            ShowNotification(1, result.message);
            $(".btnSaveReport").html('Update');
            $("#AuditReportId").val(result.data.id);
            result.operation = "update";
            $("#AuditReportOperation").val(result.operation);
        }
        else if (result.status == "400") {
            ShowNotification(3, "Something gone wrong");
        }

    }

    function seeAllSaveFail(result) {
        console.log(result);
        ShowNotification(3, result.message);
    }

    function onAuditReportUserAdd(result, detailTable) {

    }

    function onAuditReportHeadingAdd(result, detailTable) {

        var validator = $("#AuditReportHeading").validate({
            rules: {
                AuditReportDetails: {
                    required: true
                },

            },
            messages: {
                AuditReportDetails: {
                    required: "Audit Report Details is required."
                },

            }
        });

        var result = validator.form();

        if (!result) {
            ShowNotification(3, "Audit Report Details is required.");
            return;
        }

        if (!result) {
            return;
        }


        $('.auditReportSummerNote').summernote();
        var summernotes = $('.auditReportSummerNote').summernote('code');
        var encodedSummernotes = btoa(summernotes);
        var form = $("#AuditReportHeading")[0];
        var formData = new FormData(form);
        formData.set("AuditReportDetails", encodedSummernotes);

        AuditService.saveReportHeading(formData, saveDoneAuditReport, auditReportSaveFail);

    }

    function saveDoneAuditReport(result) {

        if (result.status == "200") {

            ShowNotification(1, result.message);
            $(".btnSaveReport").html('Update');
            $("#AuditReportId").val(result.data.id);

            result.operation = "update";
            $("#AuditReportOperation").val(result.operation);

        }
        else if (result.status == "400") {
            ShowNotification(3, "Something gone wrong");
        }
    }

    function auditReportSaveFail(result) {
        console.log(result);
        ShowNotification(3, result.message);
    }


    //AuditSecondReportHeading

    function onAuditSecondReportHeadingAdd(result, detailTable) {

        var validator = $("#AuditSecondReportHeading").validate({
            rules: {
                AuditReportDetails: {
                    required: true
                },
            },
            messages: {
                AuditReportDetails: {
                    required: "Audit Report Details is required."
                },
            }
        });

        var result = validator.form();

        if (!result) {
            ShowNotification(3, "Audit Report Details is required.");
            return;
        }

        if (!result) {
            return;
        }

        $('.auditSecondReportSummerNote').summernote();
        var summernotes = $('.auditSecondReportSummerNote').summernote('code');
        var encodedSummernotes = btoa(unescape(encodeURIComponent(summernotes)));

        var form = $("#AuditSecondReportHeading")[0];
        var formData = new FormData(form);
        formData.set("AuditSecondReportDetails", encodedSummernotes);

        AuditService.saveSecondReportHeading(formData, saveDoneAuditSecondReport, auditSecondReportSaveFail);

    }

    function saveDoneAuditSecondReport(result) {

        if (result.status == "200") {

            ShowNotification(1, result.message);
            $(".btnSaveReport").html('Update');
            $("#AuditReportId").val(result.data.id);
            result.operation = "update";
            $("#AuditReportOperation").val(result.operation);

        }
        else if (result.status == "400") {
            ShowNotification(3, "Something gone wrong");
        }

    }

    function auditSecondReportSaveFail(result) {
        console.log(result);
        ShowNotification(3, result.message);
    }

    //EndAuditSecondReportHeading


    //Annexure

    function onAnnexureAdd(result, detailTable) {

        var validator = $("#AuditAnnexureReport").validate({
            rules: {
                AuditReportDetails: {
                    required: true
                },
            },
            messages: {
                AuditReportDetails: {
                    required: "Audit Report Details is required."
                },

            }
        });

        var result = validator.form();

        if (!result) {
            ShowNotification(3, "Audit Report Details is required.");
            return;
        }

        if (!result) {
            return;
        }

        $('.auditAnnexureSummerNote').summernote();
        var summernotes = $('.auditAnnexureSummerNote').summernote('code');
        var encodedSummernotes = btoa(unescape(encodeURIComponent(summernotes)));

        var form = $("#AuditAnnexureReport")[0];
        var formData = new FormData(form);
        formData.set("AuditAnnexureDetails", encodedSummernotes);

        AuditService.saveAnnexureReport(formData, saveAnnexure, AnnexureSaveFail);

    }

    function saveAnnexure(result) {

        if (result.status == "200") {

            ShowNotification(1, result.message);
            $(".btnSaveReport").html('Update');
            $("#AuditReportId").val(result.data.id);
            result.operation = "update";
            $("#AuditReportOperation").val(result.operation);

        }
        else if (result.status == "400") {
            ShowNotification(3, "Something gone wrong");
        }

    }

    function AnnexureSaveFail(result) {
        console.log(result);
        ShowNotification(3, result.message);
    }

    //EndAnnexure

    function rowEdit(id, Edit) {
        AuditService.auditAreaModal({ Id: id, Edit }, (result) => { onAdd(result, detailTable) }, null, null, () => { detailTable.draw() });

    }

    //GDIC
    function rowEdit_GDIC(id, Edit) {
        AuditService.auditGDICAreaModal({ Id: id, Edit }, (result) => { onGDICAdd(result, detailTable) }, null, null, () => { detailTable.draw() });
        
    }

    function onClose() {

    }


    function rowAdd(detailTable) {

        var result = parseFormModal("#AuditAreas");
        result.AreaDetails = encodeBase64(CKEDITOR.instances['AreaDetails'].getData());

        if (!result.AuditArea) {
            alert('Enter Audit Area Name');
            return;
        }

        if (!result.AreaDetails) {
            alert('Enter Audit Area Details');
            return;

        }

        console.log(result.AreaDetails)

        detailTable.rows.add([
            result
        ]).draw();


        $("#AuditArea").val('');
        CKEDITOR.instances['AreaDetails'].setData('')
    }



    function rowDelete(button, table) {
        table.row(button.parents('tr')).remove().draw();
    }

    //GDIC
    function getGDICTableConfig() {

        return {

            "processing": true,
            "serverSide": true,
            "info": true,
            "orderCellsTop": true,
            "fixedHeader": true,
            "bProcessing": true,
            "dom": 'lBfrtip',
            "bRetrieve": true,


            ajax: {
                url: getAreaIndexURL_GDIC(),
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
                    data: "auditArea",
                    name: "auditArea"

                },
                {
                    data: "areaDetails",
                    name: "areaDetails",
                    "visible": false
                },


                {
                    data: "id",
                    render: function (data) {

                        return "<a  class='edit js-edit' data-id='" + data + "'  ><i class='material-icons' data-toggle='tooltip' title='Edit Area' data-id='" + data + "' data-original-title='Edit'></i></a>  "

                    },
                    "width": "7%",
                    "orderable": false
                }
            ],
        }
    }


    function getTableConfig() {

        return {

            "processing": true,
            "serverSide": true,
            "info": true,
            "orderCellsTop": true,
            "fixedHeader": true,
            "bProcessing": true,
            "dom": 'lBfrtip',
            "bRetrieve": true,


            ajax: {
                url: getAreaIndexURL(),
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
                    data: "auditType",
                    name: "AuditType"

                },
                {
                    data: "weightPersent",
                    name: "WeightPersent"

                },

                {
                    data: "p_Mark",
                    name: "P_Mark"

                },

                {
                    data: "p_Level",
                    name: "P_Level"

                },

                //{
                //    data: "areaDetails",
                //    name: "areaDetails",
                //    "visible": false
                //},


                {
                    data: "id",
                    render: function (data) {

                        return "<a  class='edit js-edit' data-id='" + data + "'  ><i class='material-icons' data-toggle='tooltip' title='Edit Area' data-id='" + data + "' data-original-title='Edit'></i></a>  "

                    },
                    "width": "7%",
                    "orderable": false
                }
            ],
        }
    }


    // Add Point

    function getTableConfigPoint() {

        return {

            "processing": true,
            "serverSide": true,
            "info": true,
            "orderCellsTop": true,
            "fixedHeader": true,
            "bProcessing": true,
            "dom": 'lBfrtip',
            "bRetrieve": true,


            ajax: {
                url: getAreaIndexURLPoint(),
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

                        return "<a  class='edit js-AuditPointList' data-id='" + data + "'  ><i class='material-icons' data-toggle='tooltip' title='Edit Area' data-id='" + data + "' data-original-title='Edit'></i></a>  "

                    },
                    "width": "7%",
                    "orderable": false
                },

                {
                    data: "auditType",
                    name: "AuditType"

                }
                ,
                {
                    data: "weightPersent",
                    name: "WeightPersent"

                }
                ,
                {
                    data: "p_Level",
                    name: "P_Level"

                }

            ],
        }
    }



    //End


    //Issue

    var getIssueTableConfig = function () {

        $('#AuditIssueDetails thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#AuditIssueDetails thead');

        var id = $("#Id").val();
        var counter = 1;
        var databaseRowCount = 100;
        var edit = $("#Edit").val();

        var dataTable = $("#AuditIssueDetails").DataTable({
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
                url: getIssueIndexURL(),
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {

                            "issuename": $("#md-IssueName").val(),
                            "issuepriority": $("#md-IssuePriority").val(),
                            "dateofsubmission": $("#md-DateOfSubmission").val(),
                            "enddate": $("#md-EndDate").val(),
                            "ispost": $("#md-Post").val()

                        });
                }
            },

            columns: [

                {
                    data: null,
                    name: "ID",
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
                    data: "id",
                    render: function (data, type, row) {

                        var edit = $('#Edit').val();
                        var issueCheckData = row.issueCheck;
                        var team = $('#IsHOD').val();

                        var output = "<a data-id='" + data + "' class='edit issueLineEdit'><i data-id='" + data + "' class='material-icons' data-toggle='tooltip' title='Edit Issue' data-original-title='Edit'></i></a>"
                            + "&nbsp;"
                            + "&nbsp;"
                            + "<a data-id='" + data + "' class='edit reportStatus' style='background-color: #74E291;' ><i data-id='" + data + "' class='fas fa-chart-line' style='color: green;' data-toggle='tooltip' title='Audit Report Status' data-original-title='Edit'></i></a>";

                        if (edit === "issueApprove") {

                            output += "&nbsp;"
                            output += "<input onclick='CheckAll(this)' class='dSelected' type='checkbox' data-Id=" + data;

                            if (issueCheckData == true) {
                                output += " checked";
                            }

                            output += ">";
                        }

                        return output;
                    },
                    "width": "15%",
                    "orderable": false
                },
                {
                    data: "issueName",
                    name: "IssueName"
                },
                {
                    data: "issuePriority",
                    name: "IssuePriority",

                    render: function (data) {

                        var priorityClass = '';
                        if (data === "High") {
                            priorityClass = 'priority-high';
                        } else if (data === "Medium") {
                            priorityClass = 'priority-medium';
                        } else if (data === "Low") {
                            priorityClass = 'priority-low';
                        }

                        return '<span class="' + priorityClass + '">' + data + '</span>';
                    }
                }
                ,
                {
                    data: "dateOfSubmission",
                    name: "DateOfSubmission"
                },

                {
                    data: "createdBy",
                    name: "CreatedBy"
                },

                {
                    data: "createdOn",
                    name: "CreatedOn"
                },

                {
                    data: "isPost",
                    name: "IsPost"
                },
                {
                    data: "issueCheck",

                    render: function (data) {
                        var isChecked = (data == true);
                        var checkboxHTML = "<input onclick='CheckAll(this)' class='dSelected' type='checkbox' data-Id=" + data;
                        if (isChecked) {
                            checkboxHTML += " checked";
                        }
                        checkboxHTML += "> ";
                        return checkboxHTML;
                    },

                    "width": "9%",
                    "orderable": false,
                    "visible": false
                },

            ],

            order: [1, "desc"],

        });

        if (dataTable.columns().eq(0)) {
            dataTable.columns().eq(0).each(function (colIdx) {

                var cell = $('.filters th').eq($(dataTable.column(colIdx).header()).index());

                var title = $(cell).text();


                if ($(cell).hasClass('action')) {
                    $(cell).html('');

                }
                else if ($(cell).hasClass('bool')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Y</option><option>N</option></select>');

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


        $("#AuditIssueDetails").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();
            });

        $("#AuditIssueDetails").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;
    }


    //GDIC
    var getIssueTableConfigGDIC = function () {

        $('#AuditIssueDetails thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#AuditIssueDetails thead');

        var id = $("#Id").val();
        var counter = 1;
        var databaseRowCount = 100;
        var edit = $("#Edit").val();

        var dataTable = $("#AuditIssueDetails").DataTable({
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
                url: getIssueIndexURL(),
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {

                            "issuename": $("#md-IssueName").val(),
                            "issuepriority": $("#md-IssuePriority").val(),
                            "dateofsubmission": $("#md-DateOfSubmission").val(),
                            "enddate": $("#md-EndDate").val(),
                            "ispost": $("#md-Post").val()

                        });
                }
            },

            columns: [

                {
                    data: null,
                    name: "ID",
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
                    data: "id",
                    render: function (data, type, row) {

                        var edit = $('#Edit').val();
                        var issueCheckData = row.issueCheck;
                        var team = $('#IsHOD').val();

                        var output = "<a data-id='" + data + "' class='edit issueLineEditGDIC'><i data-id='" + data + "' class='material-icons' data-toggle='tooltip' title='Edit Issue' data-original-title='Edit'></i></a>"
                            + "&nbsp;"
                            + "&nbsp;"
                            + "<a data-id='" + data + "' class='edit reportStatus' style='background-color: #74E291;' ><i data-id='" + data + "' class='fas fa-chart-line' style='color: green;' data-toggle='tooltip' title='Audit Report Status' data-original-title='Edit'></i></a>";

                        if (edit === "issueApprove") {

                            output += "&nbsp;"
                            output += "<input onclick='CheckAll(this)' class='dSelected' type='checkbox' data-Id=" + data;

                            if (issueCheckData == true) {
                                output += " checked";
                            }

                            output += ">";
                        }

                        return output;
                    },
                    "width": "15%",
                    "orderable": false
                },
                {
                    data: "issueName",
                    name: "IssueName"
                },
                {
                    data: "issuePriority",
                    name: "IssuePriority",

                    render: function (data) {

                        var priorityClass = '';
                        if (data === "High") {
                            priorityClass = 'priority-high';
                        } else if (data === "Medium") {
                            priorityClass = 'priority-medium';
                        } else if (data === "Low") {
                            priorityClass = 'priority-low';
                        }

                        return '<span class="' + priorityClass + '">' + data + '</span>';
                    }
                }
                ,
                {
                    data: "dateOfSubmission",
                    name: "DateOfSubmission"
                },

                {
                    data: "createdBy",
                    name: "CreatedBy"
                },

                {
                    data: "createdOn",
                    name: "CreatedOn"
                },

                {
                    data: "isPost",
                    name: "IsPost"
                },
                {
                    data: "issueCheck",

                    render: function (data) {
                        var isChecked = (data == true);
                        var checkboxHTML = "<input onclick='CheckAll(this)' class='dSelected' type='checkbox' data-Id=" + data;
                        if (isChecked) {
                            checkboxHTML += " checked";
                        }
                        checkboxHTML += "> ";
                        return checkboxHTML;
                    },

                    "width": "9%",
                    "orderable": false,
                    "visible": false
                },

            ],

            order: [1, "desc"],

        });

        if (dataTable.columns().eq(0)) {
            dataTable.columns().eq(0).each(function (colIdx) {

                var cell = $('.filters th').eq($(dataTable.column(colIdx).header()).index());

                var title = $(cell).text();


                if ($(cell).hasClass('action')) {
                    $(cell).html('');

                }
                else if ($(cell).hasClass('bool')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:100%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Y</option><option>N</option></select>');

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


        $("#AuditIssueDetails").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();
            });

        $("#AuditIssueDetails").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;
    }

    function getFeedbackTableConfig() {

        var databaseRowCount = 100;

        return {
            "processing": true,
            "serverSide": true,
            "info": true,
            "orderCellsTop": true,
            "fixedHeader": true,
            "bProcessing": true,
            "dom": 'lBfrtip',
            "bRetrieve": true,


            ajax: {
                url: getIssueFeedIndexURL(),
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

                    data: null,
                    name: "ID",
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
                    },
                    "width": "3%"

                },
                {
                    data: "id",
                    render: function (data) {

                        var value = $("#IsReviewer").val();
                        var IsReviewer = false;
                        if (value == "True") {
                            IsReviewer = true;
                        }
                        var visibilityClass = IsReviewer ? 'visible-icon' : 'hidden-icon';

                        return "<a   data-id='" + data + "' class='edit feedbackLineEdit' ><i data-id='" + data + "' class='material-icons' data-toggle='tooltip' title='Edit Feedback' data-original-title='Edit'></i></a>  "
                            ;

                    },

                    "width": "3%",
                    "orderable": false
                },
                {
                    data: "issueName",
                    name: "IssueName",
                    "width": "20%"
                },
                {
                    data: "heading",
                    name: "Heading",
                    "width": "10%"
                },
                {
                    data: "createdBy",
                    name: "CreatedBy",
                    "width": "10%"
                },

                {
                    data: "createdOn",
                    name: "CreatedOn",
                    "width": "10%"
                },


                {
                    data: "isPost",
                    name: "IsPost",
                    "width": "10%"
                }

            ],
            order: [1, "desc"],

        }
    }

    //Edit
    var selectedCheckboxes = [];

    function getBranchFeedbackTableConfig() {

        var databaseRowCount = 100;

        return {
            "processing": true,
            "serverSide": true,
            "info": true,
            "orderCellsTop": true,
            "fixedHeader": true,
            "bProcessing": true,
            "dom": 'lBfrtip',
            "bRetrieve": true,

            ajax: {
                url: getIssueBranchFeedIndexURL(),
                type: 'POST',
                data: function (payLoad) {
                    debugger;
                    return $.extend({},
                        payLoad,
                        {
                            //"search2": $("#name").val()
                        });
                }
            },

            columns: [
                {
                    data: null,
                    name: "ID",
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
                    },
                    "width": "5%"
                },
                {
                    data: "id",
                    render: function (data, type, row) {

                        var value = $("#Edit").val();
                        var isTeam = $("#IsHOD").val();
                        var shouldShowIcon = false;
                        var isReportData = row.isReport;
                        var issueApproveComments = row.issueApproveComments;

                        if (value == "Branchfeedback") {
                            shouldShowIcon = true;
                        }

                        var visibilityClass = shouldShowIcon ? 'visible-icon' : 'hidden-icon';

                        if (issueApproveComments === "Rejected") {
                            var output = null;
                            return output;
                        }

                        var output = "<a data-id='" + data + "' class='edit BranchfeedbackLineEdit'><i data-id='" + data + "' class='material-icons' data-toggle='tooltip' title='Edit BranchFeedback' data-original-title='Edit'></i></a>"

                        return output;

                    },

                    "width": "4%",
                    "orderable": false
                },

                {
                    data: "commonFields.issueName",
                    data: "commonFields.issueName",
                    "width": "15%"

                },
                {
                    data: "heading",
                    name: "Heading"
                },
                {
                    data: "isPost",
                    name: "IsPost"
                },

                {
                    data: "commonFields.createdBy",
                    name: "commonFields.CreatedBy"

                },

                {
                    data: "commonFields.createdOn",
                    name: "commonFields.CreatedOn"

                },
                {
                    data: "implementationDate",
                    name: "ImplementationDate"
                },
                {
                    data: "implementationStatus",
                    name: "ImplementationStatus"
                },

                {
                    data: "issueRejectComments",
                    name: "IssueRejectComments"
                },
                {
                    data: "issueApproveComments",
                    name: "IssueApproveComments"
                },
                {
                    data: "isReport",
                    name: "IsReport",
                    "width": "9%",
                    "orderable": false,
                    "visible": false
                },
                {
                    data: "id",
                    render: function (data, type, row) {
                        debugger;
                        var value = $("#Edit").val();
                        var isTeam = $("#IsHOD").val();
                        var shouldShowIcon = false;
                        var isReportData = row.isReport;
                        var issueApproveComments = row.issueApproveComments;


                        if (value == "Branchfeedback") {
                            shouldShowIcon = true;
                        }

                        var visibilityClass = shouldShowIcon ? 'visible-icon' : 'hidden-icon';

                        if (issueApproveComments === "Rejected") {
                            var output = null;
                            return output;
                        }

                        var output = "";

                        if (isTeam === "True") {
                            debugger;
                            output += "<input onclick='CheckAll(this)' class='dSelected' type='checkbox' data-Id=" + data;
                            if (isReportData == true) {
                                output += " checked";
                            }
                            output += ">";
                        };

                        return output;
                    },

                    "width": "7%",
                    "orderable": false
                }

            ],
            order: [1, "desc"],
        }
    }

    //AuditResponse
    var AuditResponseTable = function () {

        var databaseRowCount = 1000;

        var dataTable = $("#AuditResponseDetails").DataTable({
            orderCellsTop: true,
            fixedHeader: true,
            serverSide: true,
            "processing": true,
            "bProcessing": true,
            dom: 'lBfrtip',
            bRetrieve: true,


            ajax: {

                url: '/Audit/_indexAuditResponse',
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
                    data: "auditId",
                    render: function (data, type, row) {

                        var edit = $('#Edit').val();
                        var output = "<a href='/Audit/Edit/" + data + "?edit=issue'  class='edit js-edit' data-id='" + data + "'  ><i class='material-icons' data-toggle='tooltip' title='Edit Area' data-id='" + data + "' data-original-title='Edit'></i></a>"
                        if (edit === "Issuedeadlinelapsed") {

                        }
                        else if (edit === "FollowUpAuditIssues") {
                        }
                        else if (edit === "TotalPendingIssueReview") {
                            output += "<a data-id='" + data + "' class='displayInfoTotalPendingIssue' data-toggle='tooltip' title='' data-original-title='Display Info'>    <i data-id='" + data + "' class='material-icons '>email</i>    </a>"
                        }
                        else if (edit === "PendingForReviewerFeedback") {
                            output = "<a href='/Audit/Edit/" + data + "?edit=feedback'  class='edit js-edit' data-id='" + data + "'  ><i class='material-icons' data-toggle='tooltip' title='Edit Area' data-id='" + data + "' data-original-title='Edit'></i></a>"
                        }
                        return output;
                    },

                    "width": "5%",
                    "orderable": false
                },
                {
                    data: null,
                    name: "ID",

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
                    data: "auditName",
                    name: "AuditName"
                }
                ,
                {
                    data: "issueName",
                    name: "IssueName"
                }
                ,
                {
                    data: "issuePriority",
                    name: "IssuePriority"
                }
                ,
                {
                    data: "dateOfSubmission",
                    name: "DateOfSubmission"
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

        $("#AuditResponseDetails").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#AuditResponseDetails").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }

    function getAuditUserTableConfig() {

        return {

            "processing": true,
            "serverSide": true,
            "info": true,
            "orderCellsTop": true,
            "fixedHeader": true,
            "bProcessing": true,
            "dom": 'lBfrtip',
            "bRetrieve": true,


            ajax: {
                url: getUserIndexURL(),
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
                    data: "userName",
                    name: "UserName"
                },
                {
                    data: "emailAddress",
                    name: "emailAddress"
                },
                {
                    data: "id",
                    render: function (data) {

                        return "<a   data-id='" + data + "' class='edit auditEdit' ><i data-id='" + data + "' class='material-icons' data-toggle='tooltip' title='Edit Email' data-original-title='Edit'></i></a>";

                    },
                    "width": "7%",
                    "orderable": false
                }

            ],
            order: [1, "asc"],

        }
    }



    //DownloadAuditIndexData

    $("#indexSearch").click(function () {
        debugger;
        var data = $("#Branchs").val();
        var fromDate = $("#FromDate").val();
        var toDate = $("#ToDate").val();
        if (data == "xx") {
            ShowNotification(3, "Please Select Branch Type First");
            return;
        }
        if (!fromDate || !toDate) {
            ShowNotification(3, "Please Select both From Date and To Date");
            return;
        }
        indexTable.draw();
    });


    $("#download").on("click", function () {
        var fromDate = $("#FromDate").val();
        var toDate = $("#ToDate").val();
        var branchId = $("#Branchs").val();
        if (branchId === "null") {
            branchId = null;

        }


        if (fromDate === "" || toDate === "") {
            alert("Please select both 'from date', 'to date'");
            return;
        }

        var url = '/Audit/AuditExcel?fromDate=' +
            fromDate +
            '&toDate=' +
            toDate +
            '&branchId=' +
            branchId;

        var Id = $("#Id").val();

        url += '&Id=' + (Id !== null ? Id : 'null');
        var win = window.open(url, '_blank');
    });

    //EndOfDownload

    var GetIndexTable = function () {

        $('#AuditList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#AuditList thead');


        var Status = $("#Edit").val();
        var id = $("#Id").val();
        var counter = 1;
        var databaseRowCount = 500;


        var dataTable = $("#AuditList").DataTable({
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
                    customize: function (doc) {
                        doc.content.splice(0, 0, {
                            text: ""
                        });
                    },
                    exportOptions: {
                        columns: [2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13]

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
                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14]
                    }
                },
                'csvHtml5'
            ],


            ajax: {
                url: '/Audit/_index?edit=' + Status,
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {

                            "code": $("#md-Code").val(),
                            "name": $("#md-Name").val(),
                            "auditStatus": $("#md-AuditStatus").val(),
                            "startdate": $("#md-StartDate").val(),
                            "enddate": $("#md-EndDate").val(),
                            "ispost": $("#md-Post").val(),
                            "auditPlan": $("#md-AuditPlan").val(),
                            "branchName": $("#md-BranchName").val(),
                            "auditType": $("#md-AuditType").val(),
                            "auditApprove": $("#md-AuditApprove").val(),
                            "issueComplete": $("#md-IssueComplete").val(),
                            "feedback": $("#md-Feedback").val(),
                            "branchFeedback": $("#md-BranchFeedback").val(),
                            "fromDate": $("#FromDate").val(),
                            "toDate": $("#ToDate").val()

                        });
                }
            },

            columns: [

                {
                    data: "id",
                    render: function (data) {

                        debugger;
                        var value = $("#Edit").val();
                        var shouldShowIcon = false;
                        var shouldShowIconIssue = false;
                        var shouldShowIconFeedback = false;
                        var shouldShowIconBranchFeedback = false;
                        var shouldShowIconEmail = false;

                        if (value == "Compliance" || value == "Department" || value == "Follow-Up" || value == "Petty" || value == "Subsidiary" || value == "Underwriting" || value == "Compliance" || value == "Branch" || value == "Audit" || value == "AuditApproved" || value == "AuditRejected" || value == "TotalFollowUpAudit" || value == "PendingForApproval" || value == "IndividualAudit" ||
                            value == "IssueRisk" || value == "FinalAuidtApproved" || value == "PendingAuditApproval" || value == "PendingAuditResponse" || value == "PendingForAuditFeedback") {
                            shouldShowIcon = true;
                        }
                        if (value == "Issue" || value == "IssueRejected" || value == "Issues" || value == "BeforeDeadLineIssue") {
                            shouldShowIconIssue = true;
                        }
                        if (value == "Feedback") {
                            shouldShowIconFeedback = true;
                        }
                        if (value == "BranchFeedback") {
                            shouldShowIconBranchFeedback = true;
                        }
                        if (value == "PendingAuditApproval") {
                            shouldShowIconEmail = true;
                        }

                        var visibilityClass = shouldShowIcon ? 'visible-icon' : 'hidden-icon';
                        var visibilityClassIssue = shouldShowIconIssue ? 'visible-icon' : 'hidden-icon';
                        var visibilityClassFeedback = shouldShowIconFeedback ? 'visible-icon' : 'hidden-icon';
                        var visibilityClassBranchFeedback = shouldShowIconBranchFeedback ? 'visible-icon' : 'hidden-icon';
                        var visibilityClassEmail = shouldShowIconEmail ? 'visible-icon' : 'hidden-icon';

                        return "<a href='/Audit/Edit/" + data + "?edit=audit' class='edit btn btn-primary btn-sm " + visibilityClass + "' ><i class='fas fa-pencil-alt' data-toggle='tooltip' title='Area' data-original-title='Edit'></i></a>"

                            + " <a href=/Audit/Edit/" + data + "?edit=issue class=' btn btn-primary btn-sm issue-icon " + visibilityClassIssue + " ' ><i class='fas fa-bug issue-icon' data-toggle='tooltip' title='Issue' data-original-title='Issue'></i></a>  "

                            + " <a href=/Audit/Edit/" + data + "?edit=feedback class=' btn btn-primary btn-sm feedback-icon " + visibilityClassFeedback + "   ' ><i class='fas fa-align-justify' data-toggle='tooltip' title='Feedback' data-original-title='Feedback'></i></a>  "

                            + " <a href=/Audit/Edit/" + data + "?edit=Branchfeedback class=' btn btn-primary btn-sm branchfeedback-icon " + visibilityClassBranchFeedback + " ' ><i class='fas fa-comment-alt' data-toggle='tooltip' title='Branch Feedback' data-original-title='BranchFeedback'></i></a>  "

                            + "<a   data-id='" + data + "' class='auditStatus btn btn-sm' style='background-color: #74E291;' ><i data-id='" + data + "' class='fas fa-caret-down' style='color: green;' data-toggle='tooltip' title='Audit Status' data-original-title='Edit'></i></a>  "

                            + "<input onclick='CheckAll(this)' class='dSelected " + visibilityClass + " ' type='checkbox' data-Id=" + data + " >"

                            + "<a data-id='" + data + "' class='displayInfoPendingAuditApproval  " + visibilityClassEmail + " ' data-toggle='tooltip' title='' data-original-title='Display Info'>    <i data-id='" + data + "' class='material-icons '>email</i>    </a>"

                            ;

                    },

                    "width": "10%",
                    "orderable": false
                },

                {
                    data: null,
                    name: "ID",
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
                    name: "Code",

                    "width": "8%"
                },
                {
                    data: "name",
                    name: "Name",
                    "width": "8%"

                },
                {
                    data: "branchName",
                    name: "BranchName",
                    "width": "8%"
                }
                ,
                {
                    data: "auditTypeId",
                    name: "AuditTypeId",
                    "width": "8%"
                }
                ,
                {
                    data: "auditStatus",
                    name: "AuditStatus",
                    "width": "8%"

                },
                {
                    data: "issueStatus",
                    name: "IssueStatus",
                    "width": "8%"
                },
                {
                    data: "totalIssues",
                    name: "TotalIssues",
                    "width": "8%"

                }
                ,
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
                },
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

            ],

            order: [1, "desc"],

        });


        if (dataTable.columns().eq(0)) {
            dataTable.columns().eq(0).each(function (colIdx) {

                var cell = $('.filters th').eq($(dataTable.column(colIdx).header()).index());

                var title = $(cell).text();


                if ($(cell).hasClass('action')) {
                    $(cell).html('');

                } else if ($(cell).hasClass('bool')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:125%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Y</option><option>N</option></select>');

                }
                else if ($(cell).hasClass('plan')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:125%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Plan</option><option>UnPlan</option></select>');

                }
                else if ($(cell).hasClass('auditapprove')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:125%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Approve</option><option>Pending</option></select>');

                }
                else if ($(cell).hasClass('issuecomplete')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:125%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Complete</option><option>Pending</option></select>');

                }
                else if ($(cell).hasClass('feedbackcomplete')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:125%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Complete</option><option>Pending</option></select>');

                }
                else if ($(cell).hasClass('branchfeedbackcomplete')) {

                    $(cell).html('<select class="acc-filters filter-input " style="width:125%"  id="md-' + title.replace(/ /g, "") + '"><option>Select</option><option>Complete</option><option>Pending</option></select>');

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

        $("#AuditList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });


        $("#AuditList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }

    //ValidationForIssue
    var getPreview = $("#Edit").val();

    if (getPreview != "preview") {

        var issue = $("#IsApprovedL4").val();
        if (issue == "False") {
            ShowNotification(3, "Audit Is Not Approved");
            return false;
        }

    }


    $("#AuditList").on("click", ".issue-icon", function () {

        indexTable = $("#AuditList").DataTable();

        var row = indexTable.row($(this).closest("tr"));
        var rowData = row.data();
        var code = rowData.code;
        var name = rowData.name;
        var startDate = rowData.startDate;
        var endDate = rowData.endDate;
        var isApprovedL4 = rowData.isApprovedL4;

        if (isApprovedL4 == false) {
            ShowNotification(3, "Audit Is Not Approved");
            return false;
        }

    });


    //ValidationForFeedback
    var getPreview = $("#Edit").val();
    if (getPreview != "preview") {
        var feedback = $("#IsCompleteIssue").val();
        if (feedback == "False") {

        }
    }

    $("#AuditList").on("click", ".feedback-icon", function () {

        indexTable = $("#AuditList").DataTable();
        var row = indexTable.row($(this).closest("tr"));
        var rowData = row.data();
        var code = rowData.code;
        var name = rowData.name;
        var startDate = rowData.startDate;
        var endDate = rowData.endDate;
        var isApprovedL4 = rowData.isApprovedL4;
        var IsCompleteIssue = rowData.isCompleteIssue;

        if (IsCompleteIssue == false) {

        }

    });



    //ValidationForBranchFeedback
    var getPreview = $("#Edit").val();

    if (getPreview != "preview") {

        var isCompleteIssueTeamFeedback = $("#IsCompleteIssueTeamFeedback").val();
        if (isCompleteIssueTeamFeedback == "False") {
            ShowNotification(3, "Feedback Is Not Completed");
            return false;
        }

    }


    $("#AuditList").on("click", ".branchfeedback-icon", function () {

        indexTable = $("#AuditList").DataTable();
        var row = indexTable.row($(this).closest("tr"));
        var rowData = row.data();
        var code = rowData.code;
        var name = rowData.name;
        var startDate = rowData.startDate;
        var endDate = rowData.endDate;
        var isApprovedL4 = rowData.isApprovedL4;
        var IsCompleteIssueTeamFeedback = rowData.isCompleteIssueTeamFeedback;

        if (IsCompleteIssueTeamFeedback == false) {
            ShowNotification(3, "Feedback Is Not Completed");
            return false;
        }

    });

    //End validation

    //Excel
    function btnExcelSave() {

        var table = document.getElementById('ExcelAuditList');
        debugger;
        var auditMaster = {};
        auditMaster.Id = 0;
        var AuditMasterList = [];

        for (var i = 1; i < table.rows.length; i++) {
            var row = table.rows[i];
            var rowData = {
                'Name': row.cells[0].innerText,
                'StartDate': row.cells[1].innerText,
                'EndDate': row.cells[2].innerText,
                'IsPlanned': row.cells[3].innerText,
                'AuditTypeName': row.cells[4].innerText,
                'AuditStatus': row.cells[5].innerText,
                'TeamName': row.cells[6].innerText,
                'BranchID': row.cells[7].innerText,
                'BusinessTarget': row.cells[8].innerText,
                'Remarks': row.cells[9].innerText
            };

            var CheckName = row.cells[0].innerText;

            var CheckStartDate = row.cells[1].innerText;
            var CheckEndDate = row.cells[2].innerText;

            CheckStartDate = formatDateToDesiredFormat(CheckStartDate);
            CheckEndDate = formatDateToDesiredFormat(CheckEndDate);


            const dateToCheck = '7/19/2023';
            const dateToCheck1 = '12:00:00 AM';

            const isValidForStartDate = isValidDateFormat(CheckStartDate);
            const isValidForEndDate = isValidDateFormat(CheckEndDate);
            if (!isValidForStartDate) {
                ShowNotification(2, "There is a Problem In Line " + i + ".The DateFormate Is Not Correct.Please Set A Correct StartDate");
                return false;
            }
            if (!isValidForEndDate) {
                ShowNotification(2, "There is a Problem In Line " + i + ".The DateFormate Is Not Correct.Please Set A Correct EndDate");
                return false;
            }

            var CheckIsPlanned = row.cells[3].innerText;
            var CheckAuditType = row.cells[4].innerText;
            var CheckAuditStatus = row.cells[5].innerText;
            var CheckTeamName = row.cells[6].innerText;
            var CheckBranchName = row.cells[7].innerText;
            var CheckBusinessTarget = row.cells[8].innerText;
            var CheckRemarks = row.cells[9].innerText;

            if (CheckName == null || CheckName == "") {
                ShowNotification(2, "There is a Problem In Line " + i + ".Please Fill the the Audit Name Field in The line");
                return false;
            }
            if (CheckStartDate == null || CheckStartDate == "") {
                ShowNotification(2, "There is a Problem In Line " + i + ".Please Fill the the StartDate Field in The line");
                return false;
            }
            if (CheckEndDate == null || CheckEndDate == "") {
                ShowNotification(2, "There is a Problem In Line " + i + ".Please Fill the the EndDate Field in The line");
                return false;
            }
            if (CheckIsPlanned == null || CheckIsPlanned == "") {
                ShowNotification(2, "There is a Problem In Line " + i + ".Please Fill the the Is Planned Field in The line");
                return false;
            }
            if (CheckAuditType == null || CheckAuditType == "") {
                ShowNotification(2, "There is a Problem In Line " + i + ".Please Fill the the Audit Type Field in The line");
                return false;
            }
            if (CheckAuditStatus == null || CheckAuditStatus == "") {
                ShowNotification(2, "There is a Problem In Line " + i + ".Please Fill the the Audit Status Field in The line");
                return false;
            }
            if (CheckTeamName == null || CheckTeamName == "") {
                ShowNotification(2, "There is a Problem In Line " + i + ".Please Fill the the Team Name Field in The line");
                return false;
            }
            if (CheckBranchName == null || CheckBranchName == "") {
                ShowNotification(2, "There is a Problem In Line " + i + ".Please Fill the the Branch Name Field in The line");
                return false;
            }
            if (CheckBusinessTarget == null || CheckBusinessTarget == "") {
                ShowNotification(2, "There is a Problem In Line " + i + ".Please Fill the the Business Target Field in The line");
                return false;
            }
            if (CheckRemarks == null || CheckRemarks == "") {
                ShowNotification(2, "There is a Problem In Line " + i + ".Please Fill the the Remarks Field in The line");
                return false;
            }

            AuditMasterList.push(rowData);
        }
        auditMaster.AuditMasterList = AuditMasterList;
        AuditService.ExvelSave(auditMaster, ExcelSaveDone, ExcelSaveFail);


    }
    function isValidDateFormat(dateString) {
        const datePattern = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
        return datePattern.test(dateString);
    }
    function formatDateToDesiredFormat(dateString) {
        const parsedDate = new Date(dateString);
        const month = parsedDate.getMonth() + 1;
        const day = parsedDate.getDate();
        const year = parsedDate.getFullYear();
        return `${month}/${day}/${year}`;
    }


    function ExcelSaveDone(result) {

        ShowNotification(1, "save Successfully");

    }

    function ExcelSaveFail(result) {
        console.log(result);
        ShowNotification(3, result.message);
    }

    //EndExcel


    //SendEmail
    function SendEmailSave(table) {

        var masterObj = $("#frm_Audit").serialize();
        masterObj = queryStringToObj(masterObj);
        masterObj.IsPlaned = $('#IsPlaned').is(":checked");

        var validator = $("#frm_Audit").validate();
        var result = validator.form();

        if (!result) {
            ShowNotification(2, "Please complete the form");
            return;
        }

        AuditService.sendEmailSave(masterObj, saveSendEmailDone, saveSendEmailFail);

    }
    function saveSendEmailDone(result) {

        console.log(result)
        ShowNotification(1, "Email Send Successfully");

        if (result.status == "400") {

            ShowNotification(3, result.message || result.error);
        }
    }

    function saveSendEmailFail(result) {
        console.log(result);
        ShowNotification(3, result.message);
    }

    //EndSendEmail

    function Save(table) {
        debugger;
        var AuditType = $('#AuditTypeId').val();
        var BusinessTarget = $('#BusinessTarget').val();

        if (AuditType == "1" && BusinessTarget == "" || BusinessTarget == null) {
            ShowNotification(2, "Please Fill The Business Target First");
            return false;
        }

        var masterObj = $("#frm_Audit").serialize();
        masterObj = queryStringToObj(masterObj);

        debugger;
        masterObj.IsPlaned = $('#IsPlaned').is(":checked");

        var validator = $("#frm_Audit").validate();
        var result = validator.form();

        if (!result) {
            ShowNotification(2, "Please complete the form");
            return;
        }
        debugger;
        AuditService.save(masterObj, saveDone, saveFail);

    }


    function saveDone(result) {
        debugger;
        if (result.status == "200") {
            if (result.data.operation == "add") {
                console.log(result)

                debugger;

                ShowNotification(1, result.message);
                $(".btnSave").html('Update');
                $("#Id").val(result.data.id);
                $("#Code").val(result.data.code);
                result.data.operation = "update";
                $("#Operation").val(result.data.operation);
                $("#TeamValue").val(result.data.teamId);
                $("#divUpdate").show();
                $("#divSave").hide();
                $("#SavePost").show();

                detailIssueTable.ajax.url(getIssueIndexURL());
                detailFeedbackTable.ajax.url(getIssueFeedIndexURL());

                debugger;
                if (CompanyCode.toUpperCase() === "BRAC") {
                    detailTable.ajax.url(getAreaIndexURL());
                }
                if (CompanyCode.toUpperCase() === "GDIC") {
                    detailTable.ajax.url(getAreaIndexURL_GDIC());
                }

                auditUserTable.ajax.url(getUserIndexURL());
                showSections();
                auditUserTable.draw();


            } else {
                ShowNotification(1, result.message);
                $("#divSave").hide();
                $("#divUpdate").show();
            }
        }
        else if (result.status == "400") {
            ShowNotification(3, result.message || result.error);
        }
    }

    function saveFail(result) {
        console.log(result);
        ShowNotification(3, result.message);
    }

    $('.btnPost').click('click', function () {

        Confirmation("Are you sure? Do You Want to Post Data for Audit?", function (result) {
            console.log(result);
            if (result) {


                var audit = serializeInputs("frm_Audit");
                if (audit.IsPost == "Y") {
                    ShowNotification(3, "Data has already been Posted.");
                }
                else {
                    audit.IDs = audit.Id;
                    AuditService.AuditMultiplePost(audit, AuditMultiplePosts, AuditMultiplePostFail);
                }
            }

        });

    });

    function AuditMultiplePosts(result) {
        console.log(result.message);

        if (result.status == "200") {

            ShowNotification(1, result.message);
            $("#IsPost").val('Y');
            $(".btnUnPost").show();
            $(".btnReject").show();
            $(".btnApproved").show();
            $(".btnPush").show();

            var dataTable = $('#AuditList').DataTable();
            dataTable.draw();
        }
        else if (result.status == "400") {
            ShowNotification(3, result.message);
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message);
        }
    }

    function AuditMultiplePostFail(result) {
        ShowNotification(3, "Something gone wrong");
        var dataTable = $('#AuditList').DataTable();
        dataTable.draw();

    }

    $('.Submit').click('click', function () {


        ReasonOfUnPost = $("#UnPostReason").val();
        var audit = serializeInputs("frm_Audit");

        audit["ReasonOfUnPost"] = ReasonOfUnPost;

        Confirmation("Are you sure? Do You Want to UnPost Data?", function (result) {
            if (ReasonOfUnPost === "" || ReasonOfUnPost === null) {
                ShowNotification(3, "Please Write down Reason Of UnPost");
                $("#ReasonOfUnPost").focus();
                return;
            }

            if (result) {


                audit.IDs = audit.Id;
                AuditService.AuditMultipleUnPost(audit, AuditMultipleUnPost, AuditMultipleUnPostFail);


            }
        });
    });

    function AuditMultipleUnPost(result) {
        console.log(result.message);

        if (result.status == "200") {
            ShowNotification(1, result.message);
            $("#IsPost").val('N');
            $("#divReasonOfUnPost").hide();
            $(".btnUnPost").hide();
            $(".btnReject").hide();
            $(".btnApproved").hide();

            var dataTable = $('#AuditList').DataTable();
            dataTable.draw();
            $('#modal-default').modal('hide');


        }
        else if (result.status == "400") {
            ShowNotification(3, result.message);
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message);
        }
    }

    function AuditMultipleUnPostFail(result) {
        ShowNotification(3, "Something gone wrong");
        var dataTable = $('#AuditList').DataTable();

        dataTable.draw();
    }


    var deleteIssueFile = function deleteFile(fileId, filePath) {

        AuditIssueService.deleteFile({ id: fileId, filePath: filePath }, (result) => {

            if (result.status == "200") {
                $("#file-" + fileId).remove();

                ShowNotification(1, result.message);
            }
            else if (result.status == "400") {
                ShowNotification(3, result.message);
            }

        }, saveFail);

    };

    var deleteFeedbackFile = function deleteFile(fileId, filePath) {

        AuditFeedbackService.deleteFile({ id: fileId, filePath: filePath }, (result) => {

            if (result.status == "200") {
                $("#file-" + fileId).remove();

                ShowNotification(1, result.message);
            }
            else if (result.status == "400") {
                ShowNotification(3, result.message);
            }

        }, saveFail);

    };

    var deleteBranchFeedbackFile = function deleteFileBranch(fileId, filePath) {

        AuditFeedbackService.deleteFileBranch({ id: fileId, filePath: filePath }, (result) => {

            if (result.status == "200") {
                $("#file-" + fileId).remove();

                ShowNotification(1, result.message);
            }
            else if (result.status == "400") {
                ShowNotification(3, result.message);
            }

        }, saveFail);

    };

    return {
        init: init, deleteIssueFile, deleteFeedbackFile, deleteBranchFeedbackFile
    }

}(AuditService, AuditIssueService, AuditFeedbackService);