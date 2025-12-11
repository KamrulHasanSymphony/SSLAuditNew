var AuditService = function () {

    var ReportDataUpdate = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/ReportDataUpdate',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);
    };

    var save = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/CreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);
    };

    var DbUpdatesaveSave = function (masterObj, done, fail) {

        $.ajax({
            url: '/Settings/DbUpdate',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);
    };

    var sendEmailSave = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/SendEmailCreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);
    };

    var ExvelSave = function (auditMaster, done, fail) {

        console.log("Sending auditMasterList:", auditMaster);
        debugger;

        $.ajax({
            url: '/Audit/ExcelSaveCreateEdit',
            method: 'post',
            data: JSON.stringify(auditMaster),
            contentType: 'application/json',
        })
            .done(done)
            .fail(fail);
    };

    var ReportStatus = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/ReportCreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);
    };

    var AuditStatus = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/AuditStatusCreateEdit',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);
    };

    var PendingAuditApproval = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/PendingAuditApproval',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };

    var AuditIssueComplete = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/IssueComplete',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };


    var AuditFeedbackComplete = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/FeedbackComplete',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };


    var AuditBranchFeedbackComplete = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/BranchFeedbackComplete',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };

    var AuditMultiplePost = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/MultiplePost',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);


    };

    var MultipleIssueSave = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/MultipleIssueSave',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };

    var AuditMultipleUnPost = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/MultipleUnPost',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);

    };

    var saveSeeAllReport = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/ReportSeeAllCreateEdit',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };


    var saveReportHeading = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/ReportHeadingCreateEdit',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };

    var saveSecondReportHeading = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/SecondReportHeadingCreateEdit',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };

    var saveAnnexureReport = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/AnnexureReportCreateEdit',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };


    var saveArea = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/AreaCreateEdit',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };



    var saveEmail = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/AuditUserCreateEdit',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };
    var deleteEmail = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/Delete',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };


    var SendToHOD = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/SendToHOD',
            method: 'post',
            data: masterObj

        })
            .done(done)
            .fail(fail);
    };


    var deleteIssueEmail = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditIssue/Delete',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };


    var deleteReportEmail = function (masterObj, done, fail) {

        $.ajax({
            url: '/AuditIssue/ReportDelete',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };


    var IssuesaveEmail = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/AuditIssueUserCreateEdit',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };

    var ReportInserUserEmail = function (masterObj, done, fail) {

        $.ajax({
            url: '/Audit/AuditReportInserUser',
            method: 'post',
            data: masterObj,
            processData: false,
            contentType: false

        })
            .done(done)
            .fail(fail);

    };


    function saveDoneEmail(result) {
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnSaveAuditIssueUser").html('Update');
                $("#AuditUserId").val(result.data.id);
                result.data.operation = "update";
                $("#AuditUserOperation").val(result.data.operation);

            } else {

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


    var seeAllModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/SeeAllModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);



        var modalCloseEvent = function (callBack) {

            $('#auditSeeAllModal').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }
            modalCloseEvent();

            $('.auditSeeAllSummerNote').summernote({
                height: 550,

            });

            if ($("#AuditSeeAllDetails").length) {

                var encodedData = $("#AuditSeeAllDetails").val();
                $('.auditSeeAllSummerNote').summernote('code', encodedData);

            }

            setEvents();
        }


        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#auditSeeAllModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#auditSeeAllModal").modal({ backdrop: 'static', keyboard: false }, "show");

        }

        function setEvents() {

            $(".btnSaveSeeAllReport").on("click", function (e) {
                addCallBack(e);
            });
        }
    };


    var auditReportModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditReportModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);


        var modalCloseEvent = function (callBack) {

            $('#auditReportModal').on('hidden.bs.modal', function () {
                if (typeof closeCallback == "function") {
                    closeCallback();
                }
            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            debugger;

            modalCloseEvent();

            $('.auditReportSummerNote').summernote({
                height: 550,
                fontNames: ['Arial', 'Arial Black', 'Comic Sans MS', 'Courier New', 'Consolas', 'Source Sans Pro', 'Segoe UI', 'Times New Roman'],
                toolbar: [
                    ['style', ['style']],
                    ['font', ['bold', 'italic', 'underline', 'clear']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['table', ['table']],
                    ['view', ['fullscreen', 'codeview', 'help']]
                ]
            });

            if ($("#AuditReportDetails").length) {

                var encodedData = $("#AuditReportDetails").val();
                var decodedData = atob(encodedData);
                $('.auditReportSummerNote').summernote('code', decodedData);

            }

            setEvents();
        }

        function onFail(result) {
            fail(result);
        }

        function showModal(html) {
            $("#auditReportModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#auditReportModal").modal({ backdrop: 'static', keyboard: false }, "show");

        }


        function setEvents() {

            $(".btnSaveReport").on("click", function (e) {

                addCallBack(e);

            });
        }
    };


    var auditSecondReportModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditSecondReportModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);

        var modalCloseEvent = function (callBack) {
            $('#auditSecondReportModal').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }
            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);
            }

            modalCloseEvent();

            $('.auditSecondReportSummerNote').summernote({
                height: 550,
                fontNames: ['Arial', 'Arial Black', 'Comic Sans MS', 'Courier New', 'Consolas', 'Source Sans Pro', 'Segoe UI', 'Times New Roman'],
                toolbar: [
                    ['style', ['style']],
                    ['font', ['bold', 'italic', 'underline', 'clear']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['table', ['table']],
                    ['view', ['fullscreen', 'codeview', 'help']]
                ]

            });

            if ($("#AuditSecondReportDetails").length) {

                var encodedData = $("#AuditSecondReportDetails").val();
                var decodedData = decodeURIComponent(escape(window.atob(encodedData)));
                $('.auditSecondReportSummerNote').summernote('code', decodedData);

            }

            setEvents();
        }


        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#auditSecondReportModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#auditSecondReportModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }

        function setEvents() {

            $(".btnSaveSecondReport").on("click", function (e) {
                addCallBack(e);
            });
        }
    };

    var annexureReportModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AnnexureReportModal ',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);



        var modalCloseEvent = function (callBack) {
            $('#auditAnnexureReportModal').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }
            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            debugger;

            modalCloseEvent();

            var summernoteOpened = false;

            $('.auditAnnexureSummerNote').summernote({
                height: 550,

                fontNames: ['Arial', 'Arial Black', 'Comic Sans MS', 'Courier New', 'Consolas', 'Source Sans Pro', 'Segoe UI', 'Times New Roman'],
                toolbar: [
                    ['style', ['style']],
                    ['font', ['bold', 'italic', 'underline', 'clear']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['table', ['table']],
                    ['view', ['fullscreen', 'codeview', 'help']]
                ],

                callbacks: {
                    onInit: function () {
                        makeTablesResizable();
                    },
                    onChange: function (contents, $editable) {
                        makeTablesResizable();
                    },
                    onKeyup: function (e) {
                        makeTablesResizable();
                    },
                    onPaste: function (e) {
                        makeTablesResizable();
                    }
                }

            });



            if ($("#AuditAnnexureDetails").length) {

                var encodedData = $("#AuditAnnexureDetails").val();
                var decodedData = decodeURIComponent(escape(window.atob(encodedData)));
                $('.auditAnnexureSummerNote').summernote('code', decodedData);

            }


            setEvents();
        }


        //TableExpand
        function makeTablesResizable() {
            
            $('.note-editable table').each(function () {
                var $table = $(this);              
                $table.resizable({
                    handles: 'e', 
                    minWidth: 100,
                    stop: function (event, ui) {                        
                        $table.css('table-layout', 'fixed');
                    }
                });
            });

            $('.note-editable table').each(function () {
                var $table = $(this);
                $table.find('th, td').each(function () {
                    $(this).resizable({
                        handles: 'e',
                        minWidth: 30,
                        stop: function (event, ui) {
                            $table.css('table-layout', 'fixed');
                        }
                    });
                });
            });
        }


        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#auditAnnexureReportModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#auditAnnexureReportModal").modal({ backdrop: 'static', keyboard: false }, "show");

        }

        function setEvents() {

            $(".btnSaveAnnexureReport").on("click", function (e) {
                addCallBack(e);
            });
        }
    };

    var AuditReportUserModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditReportUserModal ',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);


        var modalCloseEvent = function (callBack) {

            $('#auditReportUserModal').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }
            });

        }
        function onSuccess(result) {
            showModal(result);
            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();
            var auditReportUserTable;
            if ($("#auditReportUser").length) {
                var tableConfigs = getAuditReportUserTableConfig()
                auditReportUserTable = $("#auditReportUser").DataTable(tableConfigs);
            }

            setEvents(auditReportUserTable);
            $("#addAuditReportUser").on("click", function (e) {
                auditReportInsertUserModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAuditReportInsertUserAdd(result, auditReportUserTable) }, null, null, () => { auditReportUserTable.draw() });
            })

            $('#auditReportUser').on('click', '.reportEmail', function () {
                debugger;
                var rowData = auditReportUserTable.row($(this).closest('tr')).data();
                var AuditReportUserList = [];
                if (rowData) {
                    var userId = rowData.id;
                    var userName = rowData.userName;
                    var emailAddress = rowData.emailAddress;
                    var auditId = rowData.auditId;

                    AuditReportUserList.push({
                        Id: rowData.id,
                        UserName: rowData.userName,
                        EmailAddress: rowData.emailAddress,
                        AuditId: rowData.auditId
                    });
                }
                var AuditId = $("#AuditId").val();
                var masterObj = {
                    AuditReportUserList: AuditReportUserList,
                    AuditId: AuditId
                };

                AuditReportSaveEmail(masterObj, SaveReportEmailDone, ReportEmailFail);

            });


            var AuditReportSaveEmail = function (masterObj, done, fail) {

                console.log("Sending auditReportList:", masterObj);
                debugger;
                $.ajax({
                    url: '/Audit/AuditReportEmailSend',
                    method: 'post',
                    data: masterObj

                })
                    .done(done)
                    .fail(fail);

            };


            function SaveReportEmailDone(result) {
                if (result.status == "200") {
                    //ShowNotification(1, "Email has been send successfully ");
                    ShowNotification(1, result.message);

                }
                else {
                    //ShowNotification(3, "Item Is Not Found");
                    ShowNotification(3, result.message);

                }
            }
            function ReportEmailFail(result) {
                console.log(result);
                ShowNotification(3, "Item Is Not Found");
            }

        }

        function getAuditReportUserTableConfig() {
            debugger;
            return {

                "processing": true,
                serverSide: true,
                "info": false,
                ajax: {

                    url: '/Audit/_indexAuditReportUser?id=' + $("#Id").val(),
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

                            return "<a   data-id='" + data + "' class='edit auditReportUserEdit' ><i data-id='" + data + "' class='material-icons auditReportUserEdit' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>  " +
                                "<a data-id='" + data + "' class='reportEmail' data-toggle='tooltip' title='' data-original-title='Display Info'>    <i data-id='" + data + "' class='material-icons '>email</i>    </a>"
                                ;

                        },
                        "width": "7%",
                        "orderable": false
                    }

                ],
                order: [1, "asc"],

            }
        }

        function onFail(result) {
            fail(result);
        }
        function showModal(html) {
            $("#auditReportUserModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#auditReportUserModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }

        function setEvents(auditReportUserTable) {

            $(".btnSaveAnnexureReport").on("click", function (e) {
                addCallBack(e);
            });


            $("#auditReportUser").on("click", ".auditReportUserEdit", function (e) {
                debugger;
                var id = $(this).data("id");
                auditReportInsertUserModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val(), Id: id }, (result) => { onAuditReportInsertUserAdd(result, auditReportUserTable) }, null, null, () => { auditReportUserTable.draw() });

            });
        }
    };


    var auditReportInsertUserModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditReportInsertUserModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();
            setEvents();

            if ($("#UserId").length) {
                LoadCombo("UserId", '/Common/GetAllUserName');
            }

            $('.btnDeleteAuditReportUser').click('click', function () {
                Confirmation("Are you sure? Do You Want to Delete Data?", function (result) {
                    console.log(result);

                    if (result) {

                        var form = $("#frm_Audit_Report_User")[0];
                        var formData = new FormData(form);
                        AuditService.deleteReportEmail(formData, deleteReportDoneEmail, deleteReportFail);

                    }
                });

            });

            function deleteReportDoneEmail(result) {
                if (result.status == "200") {
                    ShowNotification(1, result.message);
                    $('#auditReportAddModal').modal('hide');
                }
                else if (result.status == "400") {
                    ShowNotification(3, "Something gone wrong");
                }
            }
            function deleteReportFail(result) {
                console.log(result);
                ShowNotification(3, "Something gone wrong");
            }
        }

        var modalCloseEvent = function (callBack) {

            $('#auditReportAddModal').on('hidden.bs.modal', function () {
                if (typeof closeCallback == "function") {
                    closeCallback();
                }
            });

        }


        function onFail(result) {
            if (typeof fail == "function") {
                fail(result);
            }

        }

        function showModal(html) {

            $("#auditReportAddModal").html(html);
            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#auditReportAddModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }

        function setEvents() {

            $(".btnSaveAuditReportUser").on("click", function (e) {
                addCallBack();
            });

            $("#UserId").on("change", function (e) {

                $.ajax({
                    url: '/Audit/GetUserInfo?userId=' + $(this).val(),
                })
                    .done((result) => {
                        $("#EmailAddress").val(result.email)
                    })
                    .fail();
            });
        }

    };


    function onAuditReportInsertUserAdd(result) {
        var validator = $("#frm_Audit_Report_User").validate({
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
                }


            }
        });

        var result = validator.form();

        if (!result) {
            ShowNotification(2, "Please complete the form");
            return;
        }

        var form = $("#frm_Audit_Report_User")[0];
        var formData = new FormData(form);
        formData.set('AuditIssueId', $('#IssueId').val())
        debugger;
        ReportInserUserEmail(formData, saveDoneReportEmail, saveReportFail);

    }

    function saveDoneReportEmail(result) {
        if (result.status == "200") {
            if (result.data.operation == "add") {

                ShowNotification(1, result.message);
                $(".btnSaveAuditReportUser").html('Update');
                $("#AuditUserId").val(result.data.id);
                result.data.operation = "update";
                $("#AuditUserOperation").val(result.data.operation);

            } else {

                ShowNotification(1, result.message);
            }
        }
        else if (result.status == "400") {
            ShowNotification(3, "Something gone wrong");
        }
    }

    function saveReportFail(result) {
        console.log(result);
        ShowNotification(3, "Something gone wrong");
    }


    //function createDropdown(MaximumMark) {

    //    var markValue = 3;

    //    var dropdown = $('<select></select>')
    //        .attr('id', 'dynamicDropdown')
    //        .addClass('form-control');


    //    var num = parseInt(MaximumMark, 10);

    //    for (var i = 0; i <= MaximumMark; i++) {
    //        $('<option></option>')
    //            .val(i)
    //            .text(i)
    //            .appendTo(dropdown);
    //    }
    //    $('#dropdownContainer').html(dropdown);
    //}

    function createDropdown(MaximumMark, markValue) {
        debugger;
        var dropdown = $('<select></select>')
            .attr('id', 'dynamicDropdown')
            .addClass('form-control');

        var num = parseInt(MaximumMark, 10);
        var defaultValue = parseInt(markValue, 10) || 0; // Set default value to markValue, fallback to 0

        for (var i = 0; i <= MaximumMark; i++) {
            var option = $('<option></option>')
                .val(i)
                .text(i);

            if (i === defaultValue) {
                option.attr("selected", "selected"); // Set the default selected value
            }

            dropdown.append(option);
        }

        $('#dropdownContainer').html(dropdown);
    }


    //GDIC
    var auditGDICAreaModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/GDICAuditAreaModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);



        var modalCloseEvent = function (callBack) {


            $('#areaModal').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            debugger;

            $('.auditAreaSummerNote').summernote({
                fontNames: ['Arial', 'Arial Black', 'Comic Sans MS', 'Courier New', 'Consolas', 'Source Sans Pro', 'Segoe UI', 'Times New Roman'],
                height: 300,
                toolbar: [
                    ['style', ['style']],
                    ['font', ['bold', 'italic', 'underline', 'clear']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['table', ['table']],
                    ['view', ['fullscreen', 'codeview', 'help']]
                ]
            });


           


            if ($("#AreaDetails").length) {
                var encodedData = $("#AreaDetails").val();
                var decodedData = atob(encodedData);
                $('.auditAreaSummerNote').summernote('code', decodedData);
            }


            setEvents();



        }

        




        function onFail(result) {
            fail(result);
        }


        function showModal(html) {

            $("#areaModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#areaModal").modal({ backdrop: 'static', keyboard: false }, "show");

        }


        function setEvents() {

            $(".btnSaveArea").on("click", function (e) {

                addCallBack(e);

            });
        }
    };


    var auditAreaModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditAreaModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);



        var modalCloseEvent = function (callBack) {


            $('#areaModal').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            debugger;

            //if ($("#AuditTypeId").length) {
            //    LoadCombo("AuditTypeId", '/Common/GetAreaAuditTypes');
            //}

            if ($("#AreaAuditType").length) {
                LoadCombo("AreaAuditType", '/Common/GetAreaAuditTypes');
            }

            debugger;
            var MaximumMark = $("#MaximumMark").val();
            var P_Mark = $("#P_Mark").val();
            createDropdown(MaximumMark, P_Mark);
            

            $('.auditAreaSummerNote').summernote({
                fontNames: ['Arial', 'Arial Black', 'Comic Sans MS', 'Courier New', 'Consolas', 'Source Sans Pro', 'Segoe UI', 'Times New Roman'],
                height: 300,

                toolbar: [
                    ['style', ['style']],
                    ['font', ['bold', 'italic', 'underline', 'clear']],
                    ['fontname', ['fontname']],
                    ['fontsize', ['fontsize']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['table', ['table']],
                    ['view', ['fullscreen', 'codeview', 'help']]
                ]

                              
                                              
            });


       
            if ($("#AreaDetails").length) {
                var encodedData = $("#AreaDetails").val();
                var decodedData = atob(encodedData);
                $('.auditAreaSummerNote').summernote('code', decodedData);
            }


            setEvents();
        }

        //New Addition

        //function makeTablesResizable() {
        //    $('.note-editable table').each(function () {
        //        var $table = $(this);
        //        $table.find('th, td').each(function () {
        //            $(this).resizable({
        //                handles: 'e',
        //                minWidth: 30,
        //                stop: function (event, ui) {
        //                    $table.css('table-layout', 'fixed');
        //                }
        //            });
        //        });
        //    });
        //}




        function onFail(result) {
            fail(result);
        }


        function showModal(html) {

            $("#areaModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#areaModal").modal({ backdrop: 'static', keyboard: false }, "show");



        }


        function setEvents() {

            $(".btnSaveArea").on("click", function (e) {

                addCallBack(e);

            });
        }
    };



    var auditIssueModal = function (masterObj, addCallBack, done, fail, closeCallback) {
        debugger;


        $.ajax({
            url: '/Audit/AuditIssueModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);


        var modalCloseEvent = function (callBack) {

            $('#IssueModal').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });

        }

        function onSuccess(result) {
            var auditUserTable;
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }
            modalCloseEvent();

            InitDateRange();


            if ($("#IssuePriority").length) {
                LoadCombo("IssuePriority", '/Common/GetIssuePriority');
            }
            if ($("#IssueStatus").length) {
                LoadCombo("IssueStatus", '/Common/GetIssueStatus');
            }
            if ($("#AuditType").length) {
                LoadCombo("AuditType", '/Common/GetAuditTypes');
            }
            if ($("#RiskType").length) {
                LoadCombo("RiskType", '/Common/GetRiskType');
            }

            debugger;
            var  auditid = $("#Id").val();
            if ($("#CheckListItemId").length) {
                LoadCombo("CheckListItemId", '/Common/CheckListItem?auditId=' + auditid);
            }

            var AuditIssueLevel = 2;
            var auditidVal = $("#Id").val();
            if ($("#AuditAreaId").length) {
                LoadCombo("AuditAreaId", '/Common/AuditAreaType?auditIssueLevel=' + auditidVal);
            }


            debugger;



            $('.issueDetailsTextArea').summernote({

                fontNames: ['Arial', 'Arial Black', 'Comic Sans MS', 'Courier New', 'Consolas', 'Source Sans Pro', 'Segoe UI', 'Times New Roman'],

                toolbar: [
                    ['style', ['bold', 'italic', 'underline', 'clear']],
                    ['fontsize', ['fontsize']],
                    ['fontname', ['fontname']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['height', ['height']],
                    ['view', ['fullscreen', 'codeview']],
                    ['insert', ['link', 'picture', 'video']],
                ],

                height: 300,

                callbacks: {
                    onInit: function () {
                        makeTablesResizable();
                    },
                    onChange: function (contents, $editable) {
                        makeTablesResizable();
                    },
                    onKeyup: function (e) {
                        makeTablesResizable();
                    },
                    onPaste: function (e) {
                        makeTablesResizable();
                    }
                }

                //callbacks: {
                //    onInit: function () {
                //        $('.note-editable').css('text-align', 'left');
                //    },
                //    onKeyup: function (e) {
                //        $('.note-editable').css('text-align', 'left');
                //    },
                //    onPaste: function (e) {
                //        setTimeout(function () {
                //            $('.note-editable').css('text-align', 'left');
                //        }, 10);
                //    }
                //}

            });


            if ($("#IssueDetails").length) {
                debugger;
                var encodedData = $("#IssueDetails").val();
                var decodedData = decodeURIComponent(escape(window.atob(encodedData)));

                $('.issueDetailsTextArea').summernote('code', decodedData);

            }

            if ($("#issueUserAudit").length) {
                var tableConfigs = getAuditUserTableConfig()
                auditUserTable = $("#issueUserAudit").DataTable(tableConfigs);
            }

            setEvents(auditUserTable);

            ///-----          

            $("#addIssueAuditUser").on("click", function (e) {
                auditIssueUserModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAuditUserAdd(result, auditUserTable) }, null, null, () => { auditUserTable.draw() });

            })

            $('#issueUserAudit').on('click', '.displayInfo', function () {
                debugger;
                var rowData = auditUserTable.row($(this).closest('tr')).data();
                var AuditIssueUserList = [];


                if (rowData) {
                    var userId = rowData.id;
                    var userName = rowData.userName;
                    var emailAddress = rowData.emailAddress;

                    AuditIssueUserList.push({
                        Id: rowData.id,
                        UserName: rowData.userName,
                        EmailAddress: rowData.emailAddress
                    });
                }

                var AuditId = $("#AuditId").val();

                var masterObj = {
                    AuditIssueUserList: AuditIssueUserList,
                    AuditId: AuditId
                };

                AuditIssueSaveAllEmail(masterObj, SaveAllEmailDone, AllEmailFail);

            });

            $("#AllIssueAuditUserEmail").on("click", function (e) {
                debugger;

                var masterObj = {};
                var AuditIssueUserList = [];


                auditUserTable.rows().every(function (index, element) {
                    var data = this.data();
                    AuditIssueUserList.push({
                        Id: data.id,
                        UserName: data.userName,
                        EmailAddress: data.emailAddress
                    });
                    return true;
                });


                var AuditId = $("#AuditId").val();

                masterObj.AuditIssueUserList = AuditIssueUserList;
                masterObj.AuditId = AuditId;

                AuditIssueSaveAllEmail(masterObj, SaveAllEmailDone, AllEmailFail);
            })


            var AuditIssueSaveAllEmail = function (masterObj, done, fail) {

                console.log("Sending auditMasterList:", masterObj);
                debugger;

                $.ajax({
                    url: '/Audit/AuditIssueUserAllEmailCreateEdit',
                    method: 'post',
                    data: masterObj

                })
                    .done(done)
                    .fail(fail);

            };

            function SaveAllEmailDone(result) {

                debugger;
                if (result.status == "200") {

                    ShowNotification(1, result.message);
                    //ShowNotification(1, "Email has been sent successfully");

                }
                else if (result.status == "400") {
                    ShowNotification(3, "Feedback is not completed,Please complete feedback first");
                }
                else {
                    ShowNotification(3, "Feedback is not completed,Please complete feedback first");
                }

            }
            function AllEmailFail(result) {
                console.log(result);
                ShowNotification(3, "Feedback is not completed,Please complete feedback first");
            }

            $('.AuditIssuePost').click('click', function () {

                Confirmation("Are you sure? Do You Want to Post Data For Audit Issue?", function (result) {
                    console.log(result);
                    if (result) {


                        var issue = serializeInputs("frm_Audit_Issue");
                        if (issue.IsPost == "Y") {
                            ShowNotification(3, "Data has already been Posted.");
                        }
                        else {
                            issue.IDs = issue.Id;
                            AuditIssueService.AuditIssueMultiplePost(issue, AuditIssueMultiplePosts, AuditIssueMultiplePostFail);
                        }
                    }
                });

            });


            function AuditIssueMultiplePosts(result) {
                console.log(result.message);

                if (result.status == "200") {

                    ShowNotification(1, result.message);
                    $("#IsPost").val('Y');
                    $(".btnUnPost").show();

                    var dataTable = $('#AuditIssueDetails').DataTable();
                    dataTable.draw();

                }
                else if (result.status == "400") {
                    ShowNotification(3, result.message);
                }
                else if (result.status == "199") {
                    ShowNotification(3, result.message);
                }
            }

            function AuditIssueMultiplePostFail(result) {
                ShowNotification(3, "Something gone wrong");
                var dataTable = $('#AuditIssueDetails').DataTable();
                dataTable.draw();

            }


            //DeActive

            $('.btnDeActive').click('click', function () {
                Confirmation("Are you sure? Do You Want to De-Active Issue?", function (result) {
                    console.log(result);
                    if (result) {

                        debugger;
                        var form = $("#frm_Audit_Issue")[0];
                        var formData = new FormData(form);
                        var Id = formData.get("Id");
                        formData.append("Id", Id);
                        AuditIssueService.DeActive(formData, DeAcitveSave, DeAcitveFail);
                    }
                });
            });

            function DeAcitveSave(result) {
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
            }

            function DeAcitveFail(result) {
                ShowNotification(3, "Something gone wrong");
            }

            //EndDeActive

            //Active

            $('.btnActive').click('click', function () {
                Confirmation("Are you sure? Do You Want to Active Issue?", function (result) {
                    console.log(result);
                    if (result) {
                        debugger;
                        var form = $("#frm_Audit_Issue")[0];
                        var formData = new FormData(form);
                        var Id = formData.get("Id");
                        formData.append("Id", Id);
                        AuditIssueService.Active(formData, AcitveSave, AcitveFail);
                    }
                });
            });

            function AcitveSave(result) {
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
            }

            function AcitveFail(result) {
                ShowNotification(3, "Something gone wrong");
            }

            //EndActive


            $('.IssueSubmit').click('click', function () {

                UnPostReasonOfIssue = $("#UnPostReasonOfIssue").val();

                var issue = serializeInputs("frm_Audit_Issue");

                issue["UnPostReasonOfIssue"] = UnPostReasonOfIssue;

                Confirmation("Are you sure? Do You Want to UnPost Data?", function (result) {

                    if (UnPostReasonOfIssue === "" || UnPostReasonOfIssue === null) {
                        ShowNotification(3, "Please Write down Reason Of UnPost");
                        $("#UnPostReasonOfIssue").focus();
                        return;
                    }

                    if (result) {

                        issue.IDs = issue.Id;
                        AuditIssueService.AuditIssueMultipleUnPost(issue, AuditIssueMultipleUnPost, AuditIssueMultipleUnPostFail);

                    }

                });
            });

            function AuditIssueMultipleUnPost(result) {
                console.log(result.message);

                if (result.status == "200") {

                    ShowNotification(1, result.message);
                    $("#IsPost").val('N');
                    $("#divReasonOfUnPost").hide();
                    $(".btnUnPost").hide();
                    var dataTable = $('#AuditIssueDetails').DataTable();
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

            function AuditIssueMultipleUnPostFail(result) {
                ShowNotification(3, "Something gone wrong");
                var dataTable = $('#AuditIssueDetails').DataTable();

                dataTable.draw();
            }

        }



        //TableExpand
        //function makeTablesResizable() {
        //    $('.note-editable table').each(function () {
        //        var $table = $(this);
        //        $table.resizable({
        //            handles: 'e',
        //            minWidth: 100,
        //            stop: function (event, ui) {
        //                $table.css('table-layout', 'fixed');
        //            }
        //        });
        //    });
        //    $('.note-editable table').each(function () {
        //        var $table = $(this);
        //        $table.find('th, td').each(function () {
        //            $(this).resizable({
        //                handles: 'e',
        //                minWidth: 30,
        //                stop: function (event, ui) {
        //                    $table.css('table-layout', 'fixed');
        //                }
        //            });
        //        });
        //    });
        //}



        function makeTablesResizable() {
            // Make the entire table resizable
            $('.note-editable table').resizable({
                handles: 'e',
                minWidth: 100,
                start: function (event, ui) {
                    $(this).css('table-layout', 'auto');
                },
                stop: function (event, ui) {
                    $(this).css('table-layout', 'fixed');
                    saveEditorContent();
                }
            });

            // Make individual columns resizable
            $('.note-editable table th, .note-editable table td').each(function () {
                var $cell = $(this);
                $cell.resizable({
                    handles: 'e',
                    minWidth: 30,
                    start: function (event, ui) {
                        var $table = $cell.closest('table');
                        $table.css('table-layout', 'auto');
                        $cell.width($cell.width());
                    },
                    resize: function (event, ui) {
                        var colIndex = $cell.index();
                        var $table = $cell.closest('table');
                        $table.find('tr').each(function () {
                            $(this).children().eq(colIndex).width(ui.size.width);
                        });
                    },
                    stop: function (event, ui) {
                        var $table = $cell.closest('table');
                        $table.css('table-layout', 'fixed');
                        saveEditorContent();
                    }
                });
            });
        }

        function saveEditorContent() {
            var $editor = $('.issueDetailsTextArea');
            var content = $editor.summernote('code');
            $editor.summernote('code', content);
        }

        function onAuditUserAdd(result) {
            var validator = $("#frm_Audit_Issue_User").validate({
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
                    }

                }
            });
            var result = validator.form();

            if (!result) {
                ShowNotification(2, "Please complete the form");
                return;
            }

            var form = $("#frm_Audit_Issue_User")[0];
            var formData = new FormData(form);

            formData.set('AuditIssueId', $('#IssueId').val())
            IssuesaveEmail(formData, saveDoneEmail, saveFail);

        }

        function getAuditUserTableConfig() {

            return {

                "processing": true,
                serverSide: true,
                "info": false,
                ajax: {
                    url: '/Audit/_indexAuditIssueUser?id=' + $("#IssueId").val(),
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
                        data: "mailStatus",
                        name: "MailStatus"
                    },
                    {
                        data: "id",
                        render: function (data) {

                            return "<a   data-id='" + data + "' class='edit auditIssueUserEdit' ><i data-id='" + data + "' class='material-icons auditIssueUserEdit' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>  " +
                                "<a data-id='" + data + "' class='displayInfo' data-toggle='tooltip' title='' data-original-title='Display Info'>    <i data-id='" + data + "' class='material-icons '>email</i>    </a>"
                                ;
                        },
                        "width": "7%",
                        "orderable": false
                    }

                ],
                order: [1, "asc"],

            }
        }

        function onFail(result) {
            fail(result);
        }

        function showModal(html) {
            $("#IssueModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#IssueModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }

        function setEvents(auditUserTable) {

            $(".btnSaveIssue").on("click", function (e) {

                addCallBack(auditUserTable);

            });

            $("#newButton").on("click", function (e) {

            });

            $("#issueUserAudit").on("click", ".auditIssueUserEdit", function (e) {

                var id = $(this).data("id");
                auditIssueUserModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val(), Id: id }, (result) => { onAuditUserAdd(result, auditUserTable) }, null, null, () => { auditUserTable.draw() });

            });
        }

    };

    //GDIC
    var auditIssueModalGDIC = function (masterObj, addCallBack, done, fail, closeCallback) {
        debugger;


        $.ajax({
            url: '/Audit/AuditIssueModalGDIC',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);


        var modalCloseEvent = function (callBack) {

            $('#IssueModal').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });

        }

        function onSuccess(result) {
            var auditUserTable;
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }
            modalCloseEvent();

            InitDateRange();


            if ($("#IssuePriority").length) {
                LoadCombo("IssuePriority", '/Common/GetIssuePriority');
            }
            if ($("#IssueStatus").length) {
                LoadCombo("IssueStatus", '/Common/GetIssueStatus');
            }
            //if ($("#AuditType").length) {
                //LoadCombo("AuditType", '/Common/GetAuditTypes');
            //}


            debugger;



            $('.issueDetailsTextArea').summernote({

                fontNames: ['Arial', 'Arial Black', 'Comic Sans MS', 'Courier New', 'Consolas', 'Source Sans Pro', 'Segoe UI', 'Times New Roman'],

                toolbar: [
                    ['style', ['bold', 'italic', 'underline', 'clear']],
                    ['fontsize', ['fontsize']],
                    ['fontname', ['fontname']],
                    ['color', ['color']],
                    ['para', ['ul', 'ol', 'paragraph']],
                    ['height', ['height']],
                    ['view', ['fullscreen', 'codeview']],
                    ['insert', ['link', 'picture', 'video']],
                ],

                height: 300,

                callbacks: {
                    onInit: function () {
                        makeTablesResizable();
                    },
                    onChange: function (contents, $editable) {
                        makeTablesResizable();
                    },
                    onKeyup: function (e) {
                        makeTablesResizable();
                    },
                    onPaste: function (e) {
                        makeTablesResizable();
                    }
                }

                //callbacks: {
                //    onInit: function () {
                //        $('.note-editable').css('text-align', 'left');
                //    },
                //    onKeyup: function (e) {
                //        $('.note-editable').css('text-align', 'left');
                //    },
                //    onPaste: function (e) {
                //        setTimeout(function () {
                //            $('.note-editable').css('text-align', 'left');
                //        }, 10);
                //    }
                //}

            });


            if ($("#IssueDetails").length) {
                debugger;
                var encodedData = $("#IssueDetails").val();
                var decodedData = decodeURIComponent(escape(window.atob(encodedData)));

                $('.issueDetailsTextArea').summernote('code', decodedData);

            }

            if ($("#issueUserAudit").length) {
                var tableConfigs = getAuditUserTableConfig()
                auditUserTable = $("#issueUserAudit").DataTable(tableConfigs);
            }

            setEvents(auditUserTable);

            ///-----          

            $("#addIssueAuditUser").on("click", function (e) {
                auditIssueUserModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAuditUserAdd(result, auditUserTable) }, null, null, () => { auditUserTable.draw() });

            })

            $('#issueUserAudit').on('click', '.displayInfo', function () {
                debugger;
                var rowData = auditUserTable.row($(this).closest('tr')).data();
                var AuditIssueUserList = [];


                if (rowData) {
                    var userId = rowData.id;
                    var userName = rowData.userName;
                    var emailAddress = rowData.emailAddress;

                    AuditIssueUserList.push({
                        Id: rowData.id,
                        UserName: rowData.userName,
                        EmailAddress: rowData.emailAddress
                    });
                }

                var AuditId = $("#AuditId").val();

                var masterObj = {
                    AuditIssueUserList: AuditIssueUserList,
                    AuditId: AuditId
                };

                AuditIssueSaveAllEmail(masterObj, SaveAllEmailDone, AllEmailFail);

            });

            $("#AllIssueAuditUserEmail").on("click", function (e) {
                debugger;

                var masterObj = {};
                var AuditIssueUserList = [];


                auditUserTable.rows().every(function (index, element) {
                    var data = this.data();
                    AuditIssueUserList.push({
                        Id: data.id,
                        UserName: data.userName,
                        EmailAddress: data.emailAddress
                    });
                    return true;
                });


                var AuditId = $("#AuditId").val();

                masterObj.AuditIssueUserList = AuditIssueUserList;
                masterObj.AuditId = AuditId;

                AuditIssueSaveAllEmail(masterObj, SaveAllEmailDone, AllEmailFail);
            })


            var AuditIssueSaveAllEmail = function (masterObj, done, fail) {

                console.log("Sending auditMasterList:", masterObj);
                debugger;

                $.ajax({
                    url: '/Audit/AuditIssueUserAllEmailCreateEdit',
                    method: 'post',
                    data: masterObj

                })
                    .done(done)
                    .fail(fail);

            };

            function SaveAllEmailDone(result) {

                debugger;
                if (result.status == "200") {

                    ShowNotification(1, result.message);
                    //ShowNotification(1, "Email has been sent successfully");

                }
                else if (result.status == "400") {
                    ShowNotification(3, "Feedback is not completed,Please complete feedback first");
                }
                else {
                    ShowNotification(3, "Feedback is not completed,Please complete feedback first");
                }

            }
            function AllEmailFail(result) {
                console.log(result);
                ShowNotification(3, "Feedback is not completed,Please complete feedback first");
            }

            $('.AuditIssuePost').click('click', function () {

                Confirmation("Are you sure? Do You Want to Post Data For Audit Issue?", function (result) {
                    console.log(result);
                    if (result) {


                        var issue = serializeInputs("frm_Audit_Issue");
                        if (issue.IsPost == "Y") {
                            ShowNotification(3, "Data has already been Posted.");
                        }
                        else {
                            issue.IDs = issue.Id;
                            AuditIssueService.AuditIssueMultiplePost(issue, AuditIssueMultiplePosts, AuditIssueMultiplePostFail);
                        }
                    }
                });

            });


            function AuditIssueMultiplePosts(result) {
                console.log(result.message);

                if (result.status == "200") {

                    ShowNotification(1, result.message);
                    $("#IsPost").val('Y');
                    $(".btnUnPost").show();

                    var dataTable = $('#AuditIssueDetails').DataTable();
                    dataTable.draw();

                }
                else if (result.status == "400") {
                    ShowNotification(3, result.message);
                }
                else if (result.status == "199") {
                    ShowNotification(3, result.message);
                }
            }

            function AuditIssueMultiplePostFail(result) {
                ShowNotification(3, "Something gone wrong");
                var dataTable = $('#AuditIssueDetails').DataTable();
                dataTable.draw();

            }


            //DeActive

            $('.btnDeActive').click('click', function () {
                Confirmation("Are you sure? Do You Want to De-Active Issue?", function (result) {
                    console.log(result);
                    if (result) {

                        debugger;
                        var form = $("#frm_Audit_Issue")[0];
                        var formData = new FormData(form);
                        var Id = formData.get("Id");
                        formData.append("Id", Id);
                        AuditIssueService.DeActive(formData, DeAcitveSave, DeAcitveFail);
                    }
                });
            });

            function DeAcitveSave(result) {
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
            }

            function DeAcitveFail(result) {
                ShowNotification(3, "Something gone wrong");
            }

            //EndDeActive

            //Active

            $('.btnActive').click('click', function () {
                Confirmation("Are you sure? Do You Want to Active Issue?", function (result) {
                    console.log(result);
                    if (result) {
                        debugger;
                        var form = $("#frm_Audit_Issue")[0];
                        var formData = new FormData(form);
                        var Id = formData.get("Id");
                        formData.append("Id", Id);
                        AuditIssueService.Active(formData, AcitveSave, AcitveFail);
                    }
                });
            });

            function AcitveSave(result) {
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
            }

            function AcitveFail(result) {
                ShowNotification(3, "Something gone wrong");
            }

            //EndActive


            $('.IssueSubmit').click('click', function () {

                UnPostReasonOfIssue = $("#UnPostReasonOfIssue").val();

                var issue = serializeInputs("frm_Audit_Issue");

                issue["UnPostReasonOfIssue"] = UnPostReasonOfIssue;

                Confirmation("Are you sure? Do You Want to UnPost Data?", function (result) {

                    if (UnPostReasonOfIssue === "" || UnPostReasonOfIssue === null) {
                        ShowNotification(3, "Please Write down Reason Of UnPost");
                        $("#UnPostReasonOfIssue").focus();
                        return;
                    }

                    if (result) {

                        issue.IDs = issue.Id;
                        AuditIssueService.AuditIssueMultipleUnPost(issue, AuditIssueMultipleUnPost, AuditIssueMultipleUnPostFail);

                    }

                });
            });

            function AuditIssueMultipleUnPost(result) {
                console.log(result.message);

                if (result.status == "200") {

                    ShowNotification(1, result.message);
                    $("#IsPost").val('N');
                    $("#divReasonOfUnPost").hide();
                    $(".btnUnPost").hide();
                    var dataTable = $('#AuditIssueDetails').DataTable();
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

            function AuditIssueMultipleUnPostFail(result) {
                ShowNotification(3, "Something gone wrong");
                var dataTable = $('#AuditIssueDetails').DataTable();

                dataTable.draw();
            }

        }



        //TableExpand
        //function makeTablesResizable() {
        //    $('.note-editable table').each(function () {
        //        var $table = $(this);
        //        $table.resizable({
        //            handles: 'e',
        //            minWidth: 100,
        //            stop: function (event, ui) {
        //                $table.css('table-layout', 'fixed');
        //            }
        //        });
        //    });
        //    $('.note-editable table').each(function () {
        //        var $table = $(this);
        //        $table.find('th, td').each(function () {
        //            $(this).resizable({
        //                handles: 'e',
        //                minWidth: 30,
        //                stop: function (event, ui) {
        //                    $table.css('table-layout', 'fixed');
        //                }
        //            });
        //        });
        //    });
        //}



        function makeTablesResizable() {
            // Make the entire table resizable
            $('.note-editable table').resizable({
                handles: 'e',
                minWidth: 100,
                start: function (event, ui) {
                    $(this).css('table-layout', 'auto');
                },
                stop: function (event, ui) {
                    $(this).css('table-layout', 'fixed');
                    saveEditorContent();
                }
            });

            // Make individual columns resizable
            $('.note-editable table th, .note-editable table td').each(function () {
                var $cell = $(this);
                $cell.resizable({
                    handles: 'e',
                    minWidth: 30,
                    start: function (event, ui) {
                        var $table = $cell.closest('table');
                        $table.css('table-layout', 'auto');
                        $cell.width($cell.width());
                    },
                    resize: function (event, ui) {
                        var colIndex = $cell.index();
                        var $table = $cell.closest('table');
                        $table.find('tr').each(function () {
                            $(this).children().eq(colIndex).width(ui.size.width);
                        });
                    },
                    stop: function (event, ui) {
                        var $table = $cell.closest('table');
                        $table.css('table-layout', 'fixed');
                        saveEditorContent();
                    }
                });
            });
        }

        function saveEditorContent() {
            var $editor = $('.issueDetailsTextArea');
            var content = $editor.summernote('code');
            $editor.summernote('code', content);
        }

        function onAuditUserAdd(result) {
            var validator = $("#frm_Audit_Issue_User").validate({
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
                    }

                }
            });
            var result = validator.form();

            if (!result) {
                ShowNotification(2, "Please complete the form");
                return;
            }

            var form = $("#frm_Audit_Issue_User")[0];
            var formData = new FormData(form);

            formData.set('AuditIssueId', $('#IssueId').val())
            IssuesaveEmail(formData, saveDoneEmail, saveFail);

        }

        function getAuditUserTableConfig() {

            return {

                "processing": true,
                serverSide: true,
                "info": false,
                ajax: {
                    url: '/Audit/_indexAuditIssueUser?id=' + $("#IssueId").val(),
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
                        data: "mailStatus",
                        name: "MailStatus"
                    },
                    {
                        data: "id",
                        render: function (data) {

                            return "<a   data-id='" + data + "' class='edit auditIssueUserEdit' ><i data-id='" + data + "' class='material-icons auditIssueUserEdit' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>  " +
                                "<a data-id='" + data + "' class='displayInfo' data-toggle='tooltip' title='' data-original-title='Display Info'>    <i data-id='" + data + "' class='material-icons '>email</i>    </a>"
                                ;
                        },
                        "width": "7%",
                        "orderable": false
                    }

                ],
                order: [1, "asc"],

            }
        }

        function onFail(result) {
            fail(result);
        }

        function showModal(html) {
            $("#IssueModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#IssueModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }

        function setEvents(auditUserTable) {

            $(".btnSaveIssue").on("click", function (e) {

                addCallBack(auditUserTable);

            });

            $("#newButton").on("click", function (e) {

            });

            $("#issueUserAudit").on("click", ".auditIssueUserEdit", function (e) {

                var id = $(this).data("id");
                auditIssueUserModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val(), Id: id }, (result) => { onAuditUserAdd(result, auditUserTable) }, null, null, () => { auditUserTable.draw() });

            });
        }

    };

    var auditReportStatusEditModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditReportStatusModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);



        var modalCloseEvent = function (callBack) {

            $('#auditReportStatusModal').on('hidden.bs.modal', function () {
                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });

        }

        function onSuccess(result) {
            var auditUserTable;
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            setEvents();

            if ($("#ReportStatusModal").length) {
                LoadCombo("ReportStatusModal", '/Common/GetReportStatus');
            }
            if ($("#IssuePriorityUpdate").length) {
                LoadCombo("IssuePriorityUpdate", '/Common/GetIssuePriority');
            }

            InitDateRange();
        }

        function setEvents() {

            $(".btnSaveReport").on("click", function (e) {
                addCallBack();

            });
        }



        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#auditReportStatusModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#auditReportStatusModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }

    };

    var auditStatusModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditStatusModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);



        var modalCloseEvent = function (callBack) {

            $('#forauditstatusModal').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });

        }

        function onSuccess(result) {
            var auditUserTable;
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            setEvents();

            if ($("#AuditStatus").length) {
                LoadCombo("AuditStatus", '/Common/GetAuditStatus');
            }


            if ($("#BranchIDStatus").length) {
                LoadCombo("BranchIDStatus", '/Common/Branch');
            }
            InitDateRange();

        }

        function setEvents() {

            $(".btnSaveStatus").on("click", function (e) {
                addCallBack();

            });
        }

        function onFail(result) {
            fail(result);
        }

        function showModal(html) {
            $("#forauditstatusModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#forauditstatusModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }

    };

    var auditIssueUserModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditIssueUserModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            setEvents();

            if ($("#IssuePriority").length) {
                LoadCombo("IssuePriority", '/Common/GetIssuePriority');
            }
            if ($("#UserId").length) {
                LoadCombo("UserId", '/Common/GetAllUserName');
            }

            //ChangeForAuditIssueEmailForDelete

            $('.btnDeleteAuditIssueUser').click('click', function () {

                Confirmation("Are you sure? Do You Want to Delete Data?", function (result) {
                    console.log(result);

                    if (result) {
                        var form = $("#frm_Audit_Issue_User")[0];
                        var formData = new FormData(form);
                        AuditService.deleteIssueEmail(formData, deleteIssueDoneEmail, deleteIssueFail);
                    }
                });

            });

            function deleteIssueDoneEmail(result) {
                if (result.status == "200") {
                    ShowNotification(1, result.message);
                    $('#AuditUser').modal('hide');
                }
                else if (result.status == "400") {
                    ShowNotification(3, "Something gone wrong");
                }
            }

            function deleteIssueFail(result) {
                console.log(result);
                ShowNotification(3, "Something gone wrong");
            }
        }

        var modalCloseEvent = function (callBack) {

            $('#AuditIssueUser').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });
        }

        function onFail(result) {
            if (typeof fail == "function") {
                fail(result);
            }

        }

        function showModal(html) {
            $("#AuditIssueUser").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#AuditIssueUser").modal({ backdrop: 'static', keyboard: false }, "show");
        }


        function setEvents() {

            $(".btnSaveAuditIssueUser").on("click", function (e) {

                addCallBack();


            });

            $("#UserId").on("change", function (e) {
                $.ajax({
                    url: '/Audit/GetUserInfo?userId=' + $(this).val(),

                })
                    .done((result) => {

                        $("#EmailAddress").val(result.email)
                    })
                    .fail();
            });
        }

    };


    var auditFeedbackModal = function (masterObj, addCallBack, done, fail, closeCallback, { auditId }) {

        $.ajax({
            url: '/Audit/AuditFeedbackModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);

        var modalCloseEvent = function (callBack) {

            $('#FeedbackModal').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            setEvents();

            $("#seeAuditIssuePreviewForFeedback").on("click", function (e) {

                var IssueIdForFeedback = $('#AuditIssueId').val();

                if (IssueIdForFeedback == null) {

                    ShowNotification(3, "You Have to Select a Item");
                    return false;

                }
                auditIssuePreviewModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val(), Id: IssueIdForFeedback });
            })

            if ($("#AuditIssueId").length) {

                if (auditId) {
                    LoadCombo("AuditIssueId", '/Common/GetIssues?auditid=' + auditId);

                }
                else {
                    LoadCombo("AuditIssueId", '/Common/GetIssues?auditid=' + auditId);

                }
            }

            $('.issueFeedbackSummerNote').summernote({
                height: 300,

            });

            if ($("#FeedbackDetails").length) {

                debugger;
                var encodedData = $("#FeedbackDetails").val();

                var decodedData = decodeURIComponent(escape(window.atob(encodedData)));
                $('.issueFeedbackSummerNote').summernote('code', decodedData);

            }

            $('.AuditFeedback').click('click', function () {

                Confirmation("Are you sure? Do You Want to Post Data for AuditFeedback?", function (result) {
                    console.log(result);
                    if (result) {


                        var feedback = serializeInputs("frm_Audit_feedback");
                        if (feedback.IsPost == "Y") {
                            ShowNotification(3, "Data has already been Posted.");
                        }
                        else {
                            feedback.IDs = feedback.Id;
                            feedback.Id = feedback.Id;
                            AuditFeedbackService.AuditFeedbackPost(feedback, feedbackMultiplePost, feedbackMultiplePostFail);
                        }
                    }
                });

            });


            function feedbackMultiplePost(result) {
                console.log(result.message);

                if (result.status == "200") {

                    ShowNotification(1, result.message);
                    $("#IsPost").val('Y');
                    $(".btnUnPost").show();

                    var dataTable = $('#AuditFeedbackDetails').DataTable();
                    dataTable.draw();

                }
                else if (result.status == "400") {
                    ShowNotification(3, result.message);
                }
                else if (result.status == "199") {
                    ShowNotification(3, result.message);
                }
            }

            function feedbackMultiplePostFail(result) {
                ShowNotification(3, "Something gone wrong");
                var dataTable = $('#AuditFeedbackDetails').DataTable();
                dataTable.draw();

            }



            $('.FeedbackSubmit').click('click', function () {

                UnPostReasonOfFeedback = $("#UnPostReasonOfFeedback").val();

                var feedback = serializeInputs("frm_Audit_feedback");

                feedback["UnPostReasonOfFeedback"] = UnPostReasonOfFeedback;

                Confirmation("Are you sure? Do You Want to UnPost Data?", function (result) {
                    if (UnPostReasonOfFeedback === "" || UnPostReasonOfFeedback === null) {
                        ShowNotification(3, "Please Write down Reason Of UnPost");
                        $("#UnPostReasonOfFeedback").focus();
                        return;
                    }

                    if (result) {

                        feedback.IDs = feedback.Id;
                        AuditFeedbackService.AuditFeedbackUnPost(feedback, feedbackMultipleUnPost, feedbackMultipleUnPostFail);

                    }
                });
            });
            function feedbackMultipleUnPost(result) {
                console.log(result.message);

                if (result.status == "200") {
                    ShowNotification(1, result.message);
                    $("#IsPost").val('N');
                    $("#divReasonOfUnPost").hide();
                    $(".btnUnPost").hide();

                    var dataTable = $('#AuditFeedbackDetails').DataTable();

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

            function feedbackMultipleUnPostFail(result) {
                ShowNotification(3, result.message);
                var dataTable = $('#AuditFeedbackDetails').DataTable();

                dataTable.draw();
            }

            $('.btnFeedback').click('click', function () {

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
                formData.set("Operation", "add");

                AuditFeedbackService.save(formData, saveDoneFeedback, saveFail);

            });


            function saveDoneFeedback(result) {
                if (result.status == "200") {
                    if (result.data.operation == "add") {

                        ShowNotification(1, result.message);
                        $(".btnSaveFeedback").html('Update');
                        $("#feedbackId").val(result.data.id);
                        result.data.operation = "update";
                        $("#feedbackOperation").val(result.data.operation);
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

            function saveFail(result) {
                console.log(result);
                ShowNotification(3, "Something gone wrong");
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
        }

        function onFail(result) {

            if (typeof fail == "function") {
                fail(result);
            }
            console.log(result)
        }


        function showModal(html) {
            $("#FeedbackModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });
            $("#FeedbackModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }

        function setEvents() {
            $(".btnSaveFeedback").on("click", function (e) {
                addCallBack();

            });
        }
    };


    var auditBranchFeedbackModal = function (masterObj, addCallBack, done, fail, closeCallback, { auditId }) {

        $.ajax({
            url: '/Audit/AuditBranchFeedbackModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);


        var modalCloseEvent = function (callBack) {

            $('#BranchFeedbackModal').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }

            });
        }

        function onSuccess(result) {

            showModal(result);

            if (typeof done == "function") {
                done(result);

            }
            modalCloseEvent();
            setEvents();

            debugger;
            $("#showTestField").click(function () {
                $("#testFieldContainer").toggle();
            });

            $("#seeAuditIssuePreview").on("click", function (e) {

                var IssueId = $('#AuditBranchIssueId').val();

                if (IssueId == null) {

                    ShowNotification(3, "You Have to Select a Item");
                    return false;
                }
                auditIssuePreviewModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val(), Id: IssueId });
            })



            InitDateRange();

            debugger;
            if ($("#AuditBranchIssueId").length) {

                if (auditId) {
                    //LoadCombo("AuditBranchIssueId", '/Common/GetBranchFeedbackIssues?auditid=' + auditId);
                    let operaion = $("#feedbackBranchOperation").val();
                    LoadCombo("AuditBranchIssueId", '/Common/GetBranchFeedbackIssues?auditid=' + auditId + '&Operation=' + operaion);
                }
                else {

                    LoadCombo("AuditBranchIssueId", '/Common/GetBranchFeedbackIssues?auditid=' + auditId);
                }
            }
            LoadCombo("IssueStatus", '/Common/GetIssueStatus');




            debugger;
            $('.issueBranchFeedbackSummerNote').summernote({
                fontNames: ['Arial', 'Arial Black', 'Comic Sans MS', 'Courier New', 'Consolas', 'Source Sans Pro', 'Segoe UI', 'Times New Roman'],
                height: 300,
            });

            if ($("#IssueBranchDetailsFeedback").length) {

                debugger;
                var encodedData = $("#IssueBranchDetailsFeedback").val();
                var decodedData = decodeURIComponent(escape(window.atob(encodedData)));
                $('.issueBranchFeedbackSummerNote').summernote('code', decodedData);

            }

            $('.AuditBranchFeedback').click('click', function () {

                Confirmation("Are you sure? Do You Want to Post Data for BranchFeedback?", function (result) {
                    console.log(result);
                    if (result) {

                        var branchfeedback = serializeInputs("frm_Audit_Branch_feedback");
                        if (branchfeedback.IsPost == "Y") {
                            ShowNotification(3, "Data has already been Posted.");
                        }
                        else {
                            branchfeedback.IDs = branchfeedback.Id;
                            AuditFeedbackService.AuditBranchFeedbackPost(branchfeedback, BranchFeedbackMultiplePost, BranchFeedbackMultiplePostFail);
                        }
                    }
                });

            });

            function BranchFeedbackMultiplePost(result) {
                console.log(result.message);

                if (result.status == "200") {

                    ShowNotification(1, result.message);
                    $("#IsPost").val('Y');
                    $(".btnUnPost").show();

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

            function BranchFeedbackMultiplePostFail(result) {
                ShowNotification(3, "Something gone wrong");
                var dataTable = $('#AuditBranchFeedbackDetails').DataTable();
                dataTable.draw();

            }

            $('.BranchFeedbackSubmit').click('click', function () {

                UnPostReasonOfBranchFeedback = $("#UnPostReasonOfBranchFeedback").val();
                var branchfeedback = serializeInputs("frm_Audit_Branch_feedback");
                branchfeedback["UnPostReasonOfBranchFeedback"] = UnPostReasonOfBranchFeedback;

                Confirmation("Are you sure? Do You Want to UnPost Data?", function (result) {
                    if (UnPostReasonOfBranchFeedback === "" || UnPostReasonOfBranchFeedback === null) {
                        ShowNotification(3, "Please Write down Reason Of UnPost");
                        $("#UnPostReasonOfBranchFeedback").focus();
                        return;
                    }
                    if (result) {

                        branchfeedback.IDs = branchfeedback.Id;
                        AuditFeedbackService.AuditBranchBranchFeedbackUnPost(branchfeedback, BranchFeedbackMultipleUnPost, BranchFeedbackMultipleUnPostFail);

                    }
                });
            });
            function BranchFeedbackMultipleUnPost(result) {
                console.log(result.message);

                if (result.status == "200") {
                    ShowNotification(1, result.message);

                    $("#IsPost").val('N');
                    $("#divReasonOfUnPost").hide();
                    $(".btnUnPost").hide();

                    var dataTable = $('#AuditBranchFeedbackDetails').DataTable();
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

            function BranchFeedbackMultipleUnPostFail(result) {
                ShowNotification(3, result.message);
                var dataTable = $('#AuditBranchFeedbackDetails').DataTable();
                dataTable.draw();
            }

            $('.btnBranchFeedback').click('click', function () {

                var validator = $("#frm_Audit_Branch_feedback").validate({
                    rules: {

                        AuditBranchIssueId: {
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

                        AuditBranchIssueId: {
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

                var IssueStatus = $("#IssueStatus").val();

                var form = $("#frm_Audit_Branch_feedback")[0];
                var formData = new FormData(form);

                $('.issueBranchFeedbackSummerNote').summernote();
                var summernotes = $('.issueBranchFeedbackSummerNote').summernote('code');
                var encodedSummernotes = btoa(unescape(encodeURIComponent(summernotes)));

                formData.set("BranchFeedbackDetails", encodedSummernotes);
                formData.set("IssueStatus", IssueStatus);


                var ForBranchEmail = $('#AuditBranchIssueId').val();

                formData.set("Operation", "add");
                formData.set("TeamCheck", "Team");
                formData.set("BranchEmailIssueId", ForBranchEmail);
                AuditFeedbackService.FeedbackBranchSave(formData, saveDoneBranchFeedback, saveFail);

            });

            function saveDoneBranchFeedback(result) {
                debugger;
                if (result.status == "200") {
                    if (result.data.operation == "add") {

                        ShowNotification(1, result.message);
                        $(".btnBranchSaveFeedback").html('Update');
                        $("#BranchfeedbackId").val(result.data.id);
                        $("#Id").val(result.data.auditId);
                        $("#divFeedback").show();
                        result.data.operation = "update";

                        $("#feedbackBranchOperation").val(result.data.operation);

                        addListItemBranchFeedBack(result);


                        var dataTable = $('#AuditBranchFeedbackDetails').DataTable();
                        dataTable.draw();

                    } else {

                        addListItemBranchFeedBack(result);
                        ShowNotification(1, result.message);
                    }

                    $("#fileToUpload").val('');

                }
                else if (result.status == "400") {
                    ShowNotification(3, result.message);

                    var dataTable = $('#AuditBranchFeedbackDetails').DataTable();
                    dataTable.draw();
                }
            }

            function saveFail(result) {
                console.log(result);
                ShowNotification(3, "Something gone wrong");

                var dataTable = $('#AuditBranchFeedbackDetails').DataTable();
                dataTable.draw();
            }


            $('.btnSendEamil').click('click', function () {

                var IssueStatus = $("#IssueStatus").val();
                var form = $("#frm_Audit_Branch_feedback")[0];
                var formData = new FormData(form);
                $('.issueBranchFeedbackSummerNote').summernote();
                var summernotes = $('.issueBranchFeedbackSummerNote').summernote('code');
                var encodedSummernotes = btoa(unescape(encodeURIComponent(summernotes)));

                formData.set("BranchFeedbackDetails", encodedSummernotes);
                formData.set("IssueStatus", IssueStatus);

                var ForBranchEmail = $('#AuditBranchIssueId').val();

                formData.set("Operation", "add");
                formData.set("TeamCheck", "Team");
                formData.set("BranchEmailIssueId", ForBranchEmail);
                AuditFeedbackService.FeedbackBranchSendEmail(formData, saveDoneBranchFeedbackEamil, saveFailEamil);


            });
            function saveDoneBranchFeedbackEamil(result) {
                if (result.status == "200") {

                    ShowNotification(1, "Email Send Successfully");

                }
                else if (result.status == "400") {

                    ShowNotification(3, result.message);
                }
            }

            function saveFailEamil(result) {
                console.log(result);
                ShowNotification(3, "Something gone wrong");
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
        }

        function onFail(result) {

            if (typeof fail == "function") {
                fail(result);
            }
            console.log(result)

        }

        function showModal(html) {
            $("#BranchFeedbackModal").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });
            $("#BranchFeedbackModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }

        function setEvents() {

            $(".btnBranchSaveFeedback").on("click", function (e) {
                addCallBack();
            });

            $("#AuditBranchIssueId").on("change", function (e) {

                $.ajax({
                    url: '/Audit/GetIssueDeadLine?issueId=' + $(this).val(),

                })
                    .done((result) => {

                        $("#ImplementationDate").val(result.implementationDate)
                        $("#DeadLineDate").val(result.issueDeadLine)
                        $("#Status").val(result.status)

                    })
                    .fail();
            });

        }

    };


    var auditIssuePreviewModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditIssuePreviewModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);

        var modalCloseEvent = function (callBack) {
            $('#AuditIssuePreview').on('hidden.bs.modal', function () {

                if (typeof closeCallback == "function") {
                    closeCallback();
                }
            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            $('.auditIssuePreviewSummernote').summernote({
                fontNames: ['Arial', 'Arial Black', 'Comic Sans MS', 'Courier New', 'Consolas', 'Source Sans Pro', 'Segoe UI'],
                height: 300,
            });
            if ($("#FeedbackDetailsPreview").length) {

                debugger;
                var encodedData = $("#FeedbackDetailsPreview").val();
                var decodedData = decodeURIComponent(escape(window.atob(encodedData)));
                $('.auditIssuePreviewSummernote').summernote('code', decodedData);

            }

            setEvents();

            $("#addIssueAuditUser").on("click", function (e) {
                auditIssueUserModal({ AuditId: $("#Id").val(), Edit: $("#Edit").val() }, (result) => { onAuditUserAdd(result, auditUserTable) }, null, null, () => { auditUserTable.draw() });

            })

        }

        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#AuditIssuePreview").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#AuditIssuePreview").modal({ backdrop: 'static', keyboard: false }, "show");

        }

        function setEvents() {

            $(".btnSaveArea").on("click", function (e) {

                addCallBack(e);

            });
        }
    };

    //EndOfModalOfAuditIssuePreview

    var auditUserModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/AuditUserModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);

        var modalCloseEvent = function (callBack) {

            $('#AuditUser').on('hidden.bs.modal', function () {
                if (typeof closeCallback == "function") {
                    closeCallback();
                }
            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            setEvents();

            if ($("#IssuePriority").length) {
                LoadCombo("IssuePriority", '/Common/GetIssuePriority');
            }
            if ($("#UserId").length) {
                LoadCombo("UserId", '/Common/GetAllUserName');
            }

            //Changeforemailfordelete

            $('.btnDeleteAuditUser').click('click', function () {

                Confirmation("Are you sure? Do You Want to Delete Data?", function (result) {
                    console.log(result);

                    if (result) {

                        var teamValue = $("#TeamValue").val();
                        var form = $("#frm_Audit_User")[0];
                        var formData = new FormData(form);
                        formData.append("TeamValue", teamValue);
                        AuditService.deleteEmail(formData, deleteDoneEmail, deleteFail);

                    }
                });

            });

            function deleteDoneEmail(result) {
                if (result.status == "200") {

                    ShowNotification(1, result.message);
                    $('#AuditUser').modal('hide');

                }
                else if (result.status == "400") {
                    ShowNotification(3, result.message);
                }
            }
            function deleteFail(result) {
                console.log(result);
                ShowNotification(3, result.message);
            }

        }


        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#AuditUser").html(html);

            $('.draggable').draggable({
                handle: ".modal-header"
            });

            $("#AuditUser").modal({ backdrop: 'static', keyboard: false }, "show");
        }


        function setEvents() {

            $(".btnSaveAuditUser").on("click", function (e) {
                addCallBack();
            });

            $("#UserId").on("change", function (e) {
                $.ajax({
                    url: '/Audit/GetUserInfo?userId=' + $(this).val(),
                })
                    .done((result) => {

                        $("#EmailAddress").val(result.email)

                    })
                    .fail();

            });
        }

    };


    //Add Point

    var auditPointModal = function (masterObj, addCallBack, done, fail, closeCallback) {

        $.ajax({
            url: '/Audit/auditPointModal',
            method: 'post',
            data: masterObj

        })
            .done(onSuccess)
            .fail(onFail);

        var modalCloseEvent = function (callBack) {

            $('#auditPointModal').on('hidden.bs.modal', function () {
                if (typeof closeCallback == "function") {
                    closeCallback();
                }
            });

        }

        function onSuccess(result) {
            showModal(result);

            if (typeof done == "function") {
                done(result);

            }

            modalCloseEvent();

            setEvents();

            var id = $("#Id").val();

            if ($("#PIdData").length) {
                LoadCombo("PIdData", '/Common/AuditPointType?pId=' + id, true);
            }

            //Modal Add Point

            $("#PIdData").on("change", function (e) {
                debugger;

                if (e.originalEvent) { 

                    debugger;
                    var empDropdown = document.getElementById("PIdData");
                    var Id = document.getElementById("Id");
                    var selectedEmpId = empDropdown.value;
                    var selectedText = $("#PIdData option:selected").text();

                    var commonVM = {
                        PId: document.getElementById("PIdData").value,
                        Id: document.getElementById("Id").value,
                        Operation: document.getElementById("Operation").value,
                        AuditType: selectedText
                    };

                    $.ajax({
                        type: "POST",
                        url: '/AuditPoints/GetAuditPointLevel',
                        contentType: 'application/json',
                        data: JSON.stringify(commonVM),
                        success: function (result) {
                            debugger;
                            if (result && result.length > 0) {

                                var data = result[0].p_Level;
                                $("#P_LevelData").val(data);
                                $("#AuditTypeId").val(result[0].auditTypeId);

                            }
                        },
                        error: function () {
                            console.error('Error occurred while fetching data.');
                        }
                    });

                }


            });


            //if ($("#IssuePriority").length) {
            //    LoadCombo("IssuePriority", '/Common/GetIssuePriority');
            //}
            //if ($("#UserId").length) {
            //    LoadCombo("UserId", '/Common/GetAllUserName');
            //}
            //$('.btnDeleteAuditUser').click('click', function () {
            //    Confirmation("Are you sure? Do You Want to Delete Data?", function (result) {
            //        console.log(result);
            //        if (result) {
            //            var teamValue = $("#TeamValue").val();
            //            var form = $("#frm_Audit_User")[0];
            //            var formData = new FormData(form);
            //            formData.append("TeamValue", teamValue);
            //            AuditService.deleteEmail(formData, deleteDoneEmail, deleteFail);
            //        }
            //    });
            //});

            //function deleteDoneEmail(result) {
            //    if (result.status == "200") {
            //        ShowNotification(1, result.message);
            //        $('#auditPointModal').modal('hide');
            //    }
            //    else if (result.status == "400") {
            //        ShowNotification(3, result.message);
            //    }
            //}

            //function deleteFail(result) {
            //    console.log(result);
            //    ShowNotification(3, result.message);
            //}

        }

        function onFail(result) {
            fail(result);
        }


        function showModal(html) {
            $("#auditPointModal").html(html);
            $('.draggable').draggable({
                handle: ".modal-header"
            });
            $("#auditPointModal").modal({ backdrop: 'static', keyboard: false }, "show");
        }

        function setEvents() {

            $(".btnSaveAuditPoint").on("click", function (e) {
                addCallBack();
            });

            //$("#UserId").on("change", function (e) {
            //    $.ajax({
            //        url: '/Audit/GetUserInfo?userId=' + $(this).val(),
            //    })
            //        .done((result) => {
            //            $("#EmailAddress").val(result.email)
            //        })
            //        .fail();
            //});
        }

    };



    return {
        save, auditAreaModal, auditIssueModal, auditPointModal
        , auditFeedbackModal, saveArea, auditUserModal, saveEmail
        , AuditMultiplePost, AuditMultipleUnPost, auditBranchFeedbackModal
        , deleteEmail, auditStatusModal, auditReportStatusEditModal
        , ReportStatus, AuditStatus, deleteIssueEmail, ExvelSave, sendEmailSave, AuditIssueComplete, AuditBranchFeedbackComplete, AuditFeedbackComplete,
        auditIssuePreviewModal, SendToHOD, auditReportModal, saveReportHeading, saveSecondReportHeading, auditSecondReportModal, annexureReportModal, saveAnnexureReport, seeAllModal, saveSeeAllReport
        , ReportDataUpdate, PendingAuditApproval, AuditReportUserModal, auditReportInsertUserModal, deleteReportEmail, DbUpdatesaveSave, MultipleIssueSave, auditGDICAreaModal, auditIssueModalGDIC
    }

}();