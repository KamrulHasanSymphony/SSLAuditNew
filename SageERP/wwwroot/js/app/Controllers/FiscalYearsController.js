var FiscalYearsController = function (CommonService, FiscalYearsService) {


    var init = function () {

       
        


        var getId = $("#Id").val() || 0;
        var getOperation = $("#Operation").val() || '';
        var getYearValue = $('#YearPeriod').val();

        generateYearList(getYearValue);

        //GetGridDataList();

        var $table = $('#fiscalYearDetails');

        $('.btnsave').click('click', function () {
            
            var getId = $('#Id').val();
            var status = "Save";
            if (parseInt(getId) > 0) {
                status = "Update";
            }
            Confirmation("Are you sure? Do You Want to " + status + " Data?",
                function (result) {
                    if (result) {
                        save($table);
                    }
                });
        });

        $('.NewButton ').on('click', function () {
            $("#dtHeader").show();
        })

        $('.btnDelete').on('click', function () {

            Confirmation("Are you sure? Do You Want to Delete Data?",
                function (result) {
                    
                    if (result) {
                        SelectData();
                    }
                });
        });

        $("#YearLock").on('click', function () {
            if ($(this).is(':checked')) {
                $(".MonthLock").attr('checked', true);
            }
            else {
                $(".MonthLock").attr('checked', false);
            }
        });

        $(".MonthLock").on('click', function () {
            if ($('.MonthLock:checkbox:not(:checked)').length > 0) {
                $(".YearLock").attr('checked', false);
            }
            else {
                $(".YearLock").attr('checked', true);
            }
        });        

        $("#Year").on('change', function () {
            
            var year = $('#Year').val();
            var yearStartDate = $('#YearStart').val();
            var updatedYearStartDate = yearStartDate.replace(/^(\d{4})/, year.toString());
            $('#YearStart').val(updatedYearStartDate);
            var startDate = new Date(updatedYearStartDate);

            var endDate = new Date(startDate);
            endDate.setFullYear(startDate.getFullYear() + 1);

            endDate.setMonth(endDate.getMonth());
            endDate.setDate(0);

            
            var updatedYearEndDate = endDate.toISOString().split('T')[0]; 

            
            $('#YearEnd').val(updatedYearEndDate);
        });
        
        $("#btnFDt").on('click', function () {
            debugger;
            $('#fiscalYearDetails').show();               

            var yearStart = $('#YearStart').val();
            var yearEnd = $('#YearEnd').val();

            let url = '/FiscalYear/FiscalYearSet?YearStart=' + yearStart + '&YearEnd=' + yearEnd;
            $('#fiscalYearDetails').html('');
            $.get(url, function (data) {
                
                $('#fiscalYearDetails').append(data);

            }).fail(function (xhr, status, error) {
                $('#fiscalYearDetails').html('<div class="error-message">Failed to load data. Please try again later.</div>');
            });
        });
        $('#YearLock').change(function () {
            var isChecked = $(this).prop('checked');


            $('.PeriodLock').each(function () {
                $(this).prop('checked', isChecked);
                $(this).prop('disabled', isChecked); 
            });
        });


        //var indexTable = FiscalYearTable();

        var indexTable = FiscalYearTable();


    }


    //End Init

    //$(".PeriodLock").each(function () {
    //    debugger;
    //    let hiddenValue = $(this).prev("input[type=hidden]").val();
    //    $(this).prop("checked", hiddenValue === "True" || hiddenValue === "true");
    //});

    var FiscalYearTable = function () {

        var dataTable = $("#FiscalYearList").DataTable({
            orderCellsTop: true,
            fixedHeader: true,
            serverSide: true,
            "processing": true,
            "bProcessing": true,
            dom: 'lBfrtip',
            bRetrieve: true,
            searching: false, // Disables search functionality

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
                url: '/FiscalYear/_index',
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

                        return "<a href=/FiscalYear/Edit/" + data + " class='edit btn btn-primary btn-sm' ><i class='fas fa-pencil-alt  ' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>  ";
                    },
                    "width": "9%",
                    "orderable": false
                },
                {
                    data: "year",
                    name: "Year"
                },
                {
                    data: "yearStart",
                    name: "YearStart"
                },
                {
                    data: "yearEnd",
                    name: "YearEnd"
                }

                
                
            ]
        });

        return dataTable;
    }




    function SelectData() {
        
        var IDs = [];

        var selectedRows = $("#FiscalYearsGrid").data("kendoGrid").select();

        if (selectedRows.length === 0) {
            ShowNotification(3, "You are requested to Select checkbox!");
            return;
        }

        selectedRows.each(function () {
            var dataItem = $("#FiscalYearsGrid").data("kendoGrid").dataItem(this);
            IDs.push(dataItem.Id);
        });

        var model = {
            IDs: IDs
        };

        var url = "/DMS/FiscalYear/Delete";

        CommonAjaxService.deleteData(url, model, deleteDone, saveFail);
    };

    function generateYearList(getYearValue) {
        
        var yearList = [];
        var currentYear = new Date().getFullYear();
        yearList.push({ key: (currentYear - 1).toString(), value: (currentYear - 1).toString() });
        yearList.push({ key: currentYear.toString(), value: currentYear.toString() });
        for (var i = 1; i <= 3; i++) {
            var year = currentYear + i;
            yearList.push({ key: year.toString(), value: year.toString() });
        }

        var yearDropdown = $('#Year');
        yearList.forEach(function (year) {
            yearDropdown.append($('<option>', {
                value: year.key,
                text: year.value
            }));
        });
        
        if (parseInt(getYearValue) > 0) {
            $('#Year').val(getYearValue);
        }
    };


    $('body').on('click', '.btnDelete-FiscalYear', function (e) {
        
        var data = $(this).attr('id');
        var id = data.split('~')[0];
        var url = "/FiscalYear/DeleteItem?id=" + id + "";

        Confirmation("Are you sure? Do You Want to Delete Data?",
            function (result) {
                
                if (result) {
                    
                    $.ajax({
                        type: 'POST',
                        url: url,
                        success: function (response) {
                            
                            if (response.status == "200") {
                                ShowNotification(1, response.message);
                            }
                            else {
                                ShowNotification(3, response.message);
                            }
                            if (response.status == "200") {
                                setTimeout(function () {
                                    window.location.reload();
                                }, 1000);
                            }
                        },
                        error: function (error) {
                            
                            ShowNotification(3, response.message);
                        }
                    });
                }
            });
    });

    function save() {

        debugger;
        var yearLock = $('#YearLock').is(':checked');
        var remarks = $("#Remarks").val();
        var validator = $("#frmEntry").validate();
        var fiscal = serializeInputs("frmEntry");
        fiscal.YearLock = yearLock;
       
        fiscal.Remarks = remarks;
        // Validate the form
        var result = validator.form();
        if (!result) {
            validator.focusInvalid();
            return;
        }


        // Initialize an empty array for fiscalYearDetails
        var fiscalYearDetails = [];
        var operation = $("#Operation").val();
        
        if (operation == 'add') {
            $('#fiscalYearDetails .card-body').each(function () {
                var row = $(this);

                // Create an object for each row's data
                var detail = {
                    Id: row.find('input[name$=".Id"]').val(),
                    MonthName: row.find('input[name$=".MonthName"]').val(),
                    MonthStart: row.find('input[name$=".MonthStart"]').val(),
                    MonthEnd: row.find('input[name$=".MonthEnd"]').val(),
                    MonthId: row.find('input[name$=".MonthId"]').val(),
                    MonthLock: row.find('input[name$=".MonthLock"]').prop('checked'),
                    Remarks: row.find('input[name$=".Remarks"]').val()
                };

                // Add the object to the fiscalYearDetails array
                fiscalYearDetails.push(detail);
            });
        }
        else {
            debugger;
            $("#dtHeader").hide();
            $('.fiscalYearRow').each(function () {
                var row = $(this);

                // Create an object for each row's data
                var detail = {
                    Id: row.find('input[name$=".Id"]').val(),
                    MonthName: row.find('input[name$=".MonthName"]').val(),  
                    MonthStart: row.find('input[name$=".MonthStart"]').val(), 
                    MonthEnd: row.find('input[name$=".MonthEnd"]').val(),  
                    MonthId: row.find('input[name$=".MonthId"]').val(),  
                    MonthLock: row.find('input[name$=".MonthLock"]').prop('checked'),
                    Remarks: row.find('input[name$=".Remarks"]').val()
                };

                // Add the object to the array
                fiscalYearDetails.push(detail);
            });



            

           
        }
        

        // Assign the array to fiscal.fiscalYearDetails
        fiscal.fiscalYearDetails = fiscalYearDetails;

        var url = "/FiscalYear/CreateEdit"

        // Call the save service
        FiscalYearsService.finalImageSave(fiscal, saveDone, saveFail);
    };


    function saveDone(result) {
        debugger;
        
        if (result.status == "200") {
            if (result.data.operation == "add") {
                ShowNotification(1, result.message);
                $(".btnsave").html('Update');
                $(".btnsave").addClass('sslUpdate');
                $("#Id").val(result.data.Id);
                $("#Operation").val("update");
                

            } else {
                ShowNotification(1, result.message);
  
            }
        }
        else if (result.status == "400") {
            ShowNotification(3, result.message); 
        }
        else if (result.status == "199") {
            ShowNotification(2, result.message); 
        }
    }

    function saveFail(result) {
             
        ShowNotification(3, "Query Exception!");
    }
    function deleteDone(result) {
        
        var grid = $('#FiscalYearsGrid').data('kendoGrid');
        if (grid) {
            grid.dataSource.read();
        }
        if (result.Status == 200) {
            ShowNotification(1, result.Message);
        }
        else if (result.Status == 400) {
            ShowNotification(3, result.Message);
        }
        else {
            ShowNotification(2, result.Message);
        }
    };

    function fail(err) {
        
        
        ShowNotification(3, "Something gone wrong");
    };

    return {
        init: init
    }


}(CommonService, FiscalYearsService);