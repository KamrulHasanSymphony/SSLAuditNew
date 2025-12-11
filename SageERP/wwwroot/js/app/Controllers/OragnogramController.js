var OragnogramController = function () {

    var init = function (count) {

        if ($("#ReportingManager").length) {
            LoadCombo("ReportingManager", '/Common/ReportingManagers');
        }

        var indexTable = EmployeesHierarchyTable();

        $(".btnsave").click(function () {
            save();
        });

        drawOragnogram();
        //chart.draw(data, { allowHtml: true, size: 'medium' });

    }

 

    //end of init

    function save() {


        var validator = $("#frm_EmployeesHierarchy").validate();
        
        var form = $("#frm_EmployeesHierarchy")[0];
        var circulars = new FormData(form);


        var result = validator.form();

        if (!result) {
            validator.focusInvalid();
            return;
        }


        OragnogramService.save(circulars, saveDone, saveFail);

    }
    function addListItem(result) {
        var list = $(".fileGroup");

        result.data.attachmentsList.forEach(function (item) {

            var item = '<li class="list-group-item" id="file-' + item.id + '"> <span>' +
                item.displayName +
                '</span><a target="_blank" href="/OragnogramEntry/DownloadFile?filePath=' + item.fileName + '" class=" ml-2 btn btn-primary btn-sm float-right ml-2">Download</a>' +
                '<button onclick="OragnogramController.deleteFile(\'' + item.id + '\', \'' + item.fileName + '\')" class=" ml-2 btn btn-danger btn-sm float-right" type="button">Delete</button>' +
                '</li>';

            list.append(item);
        });
    }

    var deleteFile = function deleteFile(fileId, filePath) {

        OragnogramService.deleteFile({ id: fileId, filePath: filePath }, (result) => {


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
    function saveDone(result) {
        debugger
        if (result.status == "200") {
            if (result.data.operation == "add") {


                ShowNotification(1, result.message);
                $(".btnsave").html('Update');

                $(".btnSave").addClass('sslUpdate');

                $("#EmployeeId").val(result.data.employeeId);
                $("#Code").val(result.data.code);

                $("#divUpdate").show();

                $("#divSave").hide();

                $("#SavePost").show();

                result.data.operation = "update";
                $("#Operation").val(result.data.operation);

                addListItem(result);



            } else {

                addListItem(result);

                ShowNotification(1, result.message);

                $("#divSave").hide();

                $("#divUpdate").show();


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

    var EmployeesHierarchyTable = function () {

        $('#EmployeesHierarchyList thead tr')
            .clone(true)
            .addClass('filters')
            .appendTo('#EmployeesHierarchyList thead');


        var dataTable = $("#EmployeesHierarchyList").DataTable({
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
                url: '/OragnogramEntry/_index',
                type: 'POST',
                data: function (payLoad) {
                    return $.extend({},
                        payLoad,
                        {
                            "indexsearch": $("#Branchs").val(),
                            "branchid": $("#CurrentBranchId").val(),

                            "code": $("#md-Code").val(),
                            "name": $("#md-Name").val(),
                            "designation": $("#md-Designation").val(),


                            "ispost": $("#md-Post").val(),
                            "ponumber": $("#md-PONumber").val(),
                            //"ispost": $("#md-Post").val(),
                            "ispush": $("#md-Push").val(),
                            "fromDate": $("#FromDate").val(),
                            "toDate": $("#ToDate").val()
                        });
                }
            },
            columns: [

                {
                    data: "employeeId",
                    render: function (data) {

                        return "<a href=/OragnogramEntry/Edit/" + data + " class='edit' ><i class='editIcon' data-toggle='tooltip' title='' data-original-title='Edit'></i></a>"                        
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
                    data: "designation",
                    name: "Designation"

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




        $("#EmployeesHierarchyList").on("change",
            ".acc-filters",
            function () {

                dataTable.draw();

            });
        $("#EmployeesHierarchyList").on("keyup",
            ".acc-filters",
            function () {

                dataTable.draw();

            });

        return dataTable;

    }


    function drawOragnogram() {
        google.charts.load('current', { packages: ['orgchart'] });
        google.charts.setOnLoadCallback(drawChart);
    }

    function drawChart() {
        $.ajax({
            type: "GET",
            url: "/Oragnogram/GetEmployees",
        })
            .done(function (result) {
                var data = new google.visualization.DataTable();
                data.addColumn('string', 'Entity');
                data.addColumn('string', 'ParentEntity');
                data.addColumn('string', 'ToolTip');

                let hierarchy = {};
                let rootNodes = [];

                debugger;
                // Step 1: Build Hierarchy Object
                result.forEach(emp => {
                    let employeeId = emp[0].toString();
                    let employeeName = emp[1];
                    let designation = emp[2];
                    let reportingManager = emp[3] !== 0 ? emp[3].toString() : '';

                    hierarchy[employeeId] = {
                        id: employeeId,
                        name: employeeName,
                        designation: designation,
                        manager: reportingManager,
                        children: [],
                        row: [
                            {
                                v: employeeId,
                                f: `<div>${employeeName}</div><div>(${designation})</div><img src="/Images/${employeeId}.jpg" />`
                            },
                            reportingManager,
                            designation
                        ]
                    };

                    // Track root nodes (top-level managers)
                    if (!reportingManager) {
                        rootNodes.push(employeeId);
                    }
                });

                // Step 2: Build Tree Structure
                Object.values(hierarchy).forEach(emp => {
                    if (emp.manager && hierarchy[emp.manager]) {
                        hierarchy[emp.manager].children.push(emp);
                    }
                });

                // Step 3: Recursive Function to Sort and Process Data
                function processHierarchy(nodeId) {
                    let node = hierarchy[nodeId];
                    if (!node) return;

                    // Sort children left-to-right (by name or ID)
                    node.children.sort((a, b) => a.name.localeCompare(b.name));

                    // Add current node to chart data
                    data.addRows([node.row]);

                    // Recursively process child nodes
                    node.children.forEach(child => processHierarchy(child.id));
                }

                // Step 4: Process Each Root Node
                rootNodes.sort((a, b) => hierarchy[a].name.localeCompare(hierarchy[b].name)); // Sort root managers
                rootNodes.forEach(rootId => processHierarchy(rootId));

                // Step 5: Draw Chart and Force Vertical Layout
                var chart = new google.visualization.OrgChart(document.getElementById("chart"));
                chart.draw(data, {
                    allowHtml: true,
                    size: 'small',
                    // Set the orientation to 'vertical' to force vertical layout
                    layout: 'vertical'
                });
            })

            .fail(function () {
                alert('Failed to load org chart data.');
            });

    }


    //function drawOragnogram() {
    //    google.load("visualization", "1", { packages: ["orgchart"] });
    //    google.setOnLoadCallback(drawChart);
    //}

    //function drawChart() {
    //    $.ajax({
    //        type: "GET",
    //        url: "/Oragnogram/GetEmployees",
    //    })
    //        .done(function (result) {
    //            debugger;
    //            var data = new google.visualization.DataTable();
    //            data.addColumn('string', 'Entity');
    //            data.addColumn('string', 'ParentEntity');
    //            data.addColumn('string', 'ToolTip');
             
    //            for (var i = 0; i < result.length; i++) {

    //                var employeeId = result[i][0].toString();
    //                var employeeName = result[i][1];
    //                var designation = result[i][2];
    //                var reportingManager = result[i][3] != 0 ? result[i][3].toString() : '';
                   
    //                var row =
    //                    [
    //                        [
    //                            {
    //                                v: employeeId,
    //                                f: employeeName + '<div>(<span>' + designation + '</span>)</div><img src = "/Images/' + employeeId + '.jpg" />'
    //                            }, reportingManager, designation
    //                        ]
    //                    ]
    //                data.addRows(row);
    //            }

    //            var chart = new google.visualization.OrgChart($("#chart")[0]);
    //            chart.draw(data, { allowHtml: true });
    //        })
    //        .fail(function () {
    //            alert('failed');
    //        });
    //}

    
    return {
        init: init,
        deleteFile
    }

}();