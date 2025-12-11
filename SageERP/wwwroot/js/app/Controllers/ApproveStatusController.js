var ApproveStatusController = function (CommonService, ApproveStatusService) {



    var init = function () {


        if ($("#TeamId").length) {
            LoadCombo("TeamId", '/Common/TeamName');
        }
        if ($("#AuditId").length) {
            LoadCombo("AuditId", '/Common/AuditName');
        }
            

        var indexTable = ToursTable();

    }

    /*init end*/


    $('.RejectSubmit').click('click', function () {


        RejectedComments = $("#RejectedComments").val();

        var tours = serializeInputs("frm_Tours");

        tours["RejectedComments"] = RejectedComments;

        Confirmation("Are you sure? Do You Want to Reject Data?", function (result) {
            if (RejectedComments === "" || RejectedComments === null) {
                ShowNotification(3, "Please Write down Reason Of Reject");
                $("#RejectedComments").focus();
                return;
            }

            if (result) {
                
                tours.IDs = tours.Id;
                ApproveStatusService.ToursMultipleRejectData(tours, ToursMultipleReject, ToursMultipleUnRejectFail);

            }
        });
    });

    function ToursMultipleReject(result) {
        console.log(result.message);

        if (result.status == "200") {
     
            ShowNotification(1, "Data Reject Successfully");

            $("#IsPost").val('N');

            $("#divReasonOfUnPost").hide();
            $(".btnUnPost").hide();
            $(".btnReject").show();
            $(".btnApproved").show();

            var dataTable = $('#ToursList').DataTable();

            dataTable.draw();

            $('#modal-defaultReject').modal('hide');

        }
        else if (result.status == "400") {
            ShowNotification(3, result.message); // <-- display the error message here
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message); // <-- display the error message here
        }
    }

    function ToursMultipleUnRejectFail(result) {
        ShowNotification(3, "Something gone wrong");
        var dataTable = $('#ToursList').DataTable();

        dataTable.draw();
    }


    $('.ApprovedSubmit').click('click', function () {


        CommentsL1 = $("#CommentsL1").val();

        var tours = serializeInputs("frm_Tours");

        tours["CommentsL1"] = CommentsL1;

        Confirmation("Are you sure? Do You Want to Approve Data?", function (result) {
            if (CommentsL1 === "" || CommentsL1 === null) {
                ShowNotification(3, "Please Write down Reason Of Approved");
                $("#CommentsL1").focus();
                return;
            }

            if (result) {

                tours.IDs = tours.Id;
                ApproveStatusService.ToursMultipleApprovedData(tours, ToursMultipleApproved, ToursMultipleApprovedFail);


            }
        });
    });

    function ToursMultipleApproved(result) {
        console.log(result.message);

        if (result.status == "200") {

            ShowNotification(1, "Data Approved Successfully");
            $("#IsPost").val('N');

            $("#divReasonOfUnPost").hide();
            $(".btnUnPost").hide();
            $(".btnReject").show();
            $(".btnApproved").show();
    
            var dataTable = $('#ToursList').DataTable();

            dataTable.draw();

            $('#modal-defaultApproved').modal('hide');

        }
        else if (result.status == "400") {
            ShowNotification(3, result.message); // <-- display the error message here
        }
        else if (result.status == "199") {
            ShowNotification(3, result.message); // <-- display the error message here
        }
    }

    function ToursMultipleApprovedFail(result) {
        ShowNotification(3, "Data has already been approved.");
        var dataTable = $('#ToursList').DataTable();

        dataTable.draw();
    }





    $('#modelClose').click('click', function () {

        $("#UnPostReason").val("");
        $('#modal-default').modal('hide');


    });


    var ToursTable = function () {

        $('#ApproveStatusist thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#ApproveStatusist thead');


        var dataTable = $("#ApproveStatusist").DataTable({
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
                
                url: '/Tours/_approveStatusIndex?',
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {
                            "indexsearch": $("#Branchs").val(),
                            "branchid": $("#CurrentBranchId").val(),

                            "code": $("#md-Code").val(),
                            "teamname": $("#md-TeamName").val(),
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

                        return "<a href=/Tours/Edit/" + data + "?edit=approve class='edit btn btn-primary btn-sm' ><i class='fas fa-check tick-icon' data-toggle='tooltip' title='' data-original-title='Tour'></i></a>" 
                      
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
                    data: "teamName",
                    name: "TeamName"

                }            
                ,
                {
                    data: "description",
                    name: "Description"

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




        $("#ApproveStatusist").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#ApproveStatusist").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }


    return {
        init: init
        
    }


}(CommonService, ApproveStatusService);