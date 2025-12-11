var DashController = function () {

    var init = function (count) {

        if (count > 0) {
            $("#branchProfiles").modal("show");
            $('.draggable').draggable({
                handle: ".modal-header"
            });
        }     

        getEvents();


        $("#tBranchProfiles").on("dblclick", "td",
            function () {
                debugger;
                var branchId = $(this).closest("tr").find("td:eq(0)").text();
                var BranchName = $(this).closest("tr").find("td:eq(2)").text();

                var form = $('<form>', { method: 'POST' });
                var targetURL = '/Home/AssignBranch';
                form.attr('action', targetURL);

                form.append($('<input>', {
                    type: 'branchId',
                    name: 'branchId',
                    value: branchId
                }));
                form.append($('<input>', {
                    type: 'BranchName',
                    name: 'BranchName',
                    value: BranchName
                }));

                form.hide();

                $(".container-fluid").append(form);

                form.submit();
                form.remove();

            });





        const percentageCells = document.querySelectorAll("tbody tr td:nth-child(6)");
        percentageCells.forEach(cell => {
            const percentage = parseInt(cell.textContent);
            const circle = document.createElement("div");
            circle.className = "percentage-circle";

            const percentageText = document.createElement("span");
            percentageText.className = "percentage-text";
            percentageText.textContent = `${percentage}%`;

            circle.style.background = `conic-gradient(#20c997 ${percentage}%, transparent ${percentage}% 100%)`;


            cell.textContent = "";
            circle.appendChild(percentageText);
            cell.appendChild(circle);
        });

        
        const percentageCells2 = document.querySelectorAll("#uba tbody tr td:nth-child(6)");
        percentageCells2.forEach(cell => {
            const percentage = parseInt(cell.textContent);
            const circle = document.createElement("div");
            circle.className = "percentage-circle";

            const percentageText = document.createElement("span");
            percentageText.className = "percentage-text";
            percentageText.textContent = `${percentage}%`;

            circle.style.background = `conic-gradient(#2D9596 ${percentage}%, transparent ${percentage}% 100%)`;


            cell.textContent = "";
            circle.appendChild(percentageText);
            cell.appendChild(circle);
        });



        debugger;
        var Completed = $("#Completed").val();
        var Ongoing = $("#Ongoing").val();
        var Remaining = $("#Remaining").val();

        var UnPlanRemaining = $("#UnPlanRemaining").val();
        var UnPlanCompleted = $("#UnPlanCompleted").val();
        var UnPlanOngoing = $("#UnPlanOngoing").val();


        const data = {
            labels: ['Completed', 'Ongoing', 'Remaining'],
            datasets: [
                {
                    data: [Completed , Ongoing  , Remaining ],
                   
                    backgroundColor: ['#00aba9', '#2b5797', '#b91d47'],
                    labels: ['Plan Completed', 'Plan Ongoing', 'Plan Remaining'], 
                },
                {
                    data: [UnPlanCompleted, UnPlanOngoing, UnPlanRemaining],
                    
                    backgroundColor: ['#00aba9', '#2b5797', '#b91d47'],
                    labels: ['UnPlan Completed', 'UnPlan Ongoing', 'UnPlan Remaining'], 
                }
            ],
        };

        const options = {
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        const datasetIndex = tooltipItem.datasetIndex;
                        const labelIndex = tooltipItem.index;
                        const label = data.datasets[datasetIndex].labels[labelIndex];
                        const value = data.datasets[datasetIndex].data[labelIndex];
                        
                        return label + ': ' + value + '%';
                    }
                }
            },
            legend: {
                display: true,
            }
        };

        new Chart(document.getElementById('combined-chart'), {
            type: 'doughnut',
            data: data,
            options: options
        });


        //SecondPart
        

        $("#planDropdown").on("change", function () {
            var selectedValue = $(this).val();
            
            console.log("Selected Plan: " + selectedValue);

            


            $.ajax({
                url: '/Home/UpdateAuditComponents', // Replace with your actual controller and action
                type: 'POST', // or 'GET' depending on your server-side implementation
                data: { selectedValue: selectedValue },
                success: function (data) {
                    
                    console.log(data);
                },
                error: function (error) {
                    console.error('Error updating AuditComponentList:', error);
                }
            });




        });



        //BarChart - 1

        var totalAudit = $("#TotalAudit").val();
        debugger;
        var auditOpen = $("#OngoinAudit").val() ;
        var teamAssign = 90;
        var issuesCreateOnAudit = $("#IssueCreatedOnAudit").val(); 
        var totalIssues = $("#TotalIssue").val(); 
        var totalRisks = $("#TotalRisk").val();

        

        const barData = {
         
            labels: ['Total Audit', 'Audit Open', /*'Team Assign',*/ 'Issues Created on Audit', 'Total Issues', 'Total Risks'],

            datasets: [
                {
                    label: 'Audit Data',
                    data: [totalAudit, auditOpen, /*teamAssign,*/ issuesCreateOnAudit, totalIssues, totalRisks],
                    backgroundColor: [
                        '#FF5733',  
                        '#33FF57',  
                        //'#3357FF',  
                        '#FF33A1', 
                        '#33A1FF',  
                        '#A133FF'   
                    ],
                    borderColor: [
                        '#FF5733',  
                        '#33FF57', 
                        //'#3357FF',  
                        '#FF33A1',  
                        '#33A1FF',  
                        '#A133FF'   
                    ],
                    borderWidth: 1
                }
            ],
            
        };

        const barOptions = {
            scales: {
                y: {
                    beginAtZero: true
                },
                x: {
                    barPercentage: 0.2,       
                    categoryPercentage: 0.5  
                }
            },
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                }
            }
        };

   
        new Chart(document.getElementById('myBarChart'), {
            type: 'bar',
            data: barData,
            options: barOptions
        });

        //pie - 2
        
        var IssueApprove  = $("#IssueApprove").val();;
        var totalRejected = 43;
        var pendingApproval = 124;

        const issuesData = {
            labels: ['Approved Issues', 'Rejected Issues', 'Pending Approval'],
            datasets: [
                {
                    data: [IssueApprove, totalRejected, pendingApproval],
                    backgroundColor: ['#28a745', '#9BCF53', '#ffc107'], // Green, Red, Yellow
                    borderColor: ['#28a745', '#9BCF53', '#ffc107'],
                    borderWidth: 1
                }
            ]
        };

        const issuesOptions = {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom'
                },
                tooltip: {
                    callbacks: {
                        label: function (tooltipItem) {
                            let value = issuesData.datasets[0].data[tooltipItem.dataIndex];
                            return ` ${issuesData.labels[tooltipItem.dataIndex]}: ${value}`;
                        }
                    }
                }
            }
        };

        new Chart(document.getElementById('issuesPieChart'), {
            type: 'pie',
            data: issuesData,
            options: issuesOptions
        });


        //Pie - 3
        
        var High = $("#IssueProrityHigh").val();
        var Medium = $("#IssueProrityMedium").val() ; 
        var Low = $("#IssueProrityLow").val();

        const priorityData = {
            labels: ['High Priority', 'Medium Priority', 'Low Priority'],
            datasets: [
                {
                    data: [High, Medium, Low],
                    backgroundColor: ['#9BCF53', '#FFA500', '#32CD32'], // Red, Orange, Green
                    borderColor: ['#9BCF53', '#FFA500', '#32CD32'],
                    borderWidth: 1
                }
            ]
        };

        const priorityOptions = {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom'
                },
                tooltip: {
                    callbacks: {
                        label: function (tooltipItem) {
                            let value = priorityData.datasets[0].data[tooltipItem.dataIndex];
                            return ` ${priorityData.labels[tooltipItem.dataIndex]}: ${value}`;
                        }
                    }
                }
            }
        };

        new Chart(document.getElementById('priorityPieChart'), {
            type: 'pie',
            data: priorityData,
            options: priorityOptions
        });
        
        
        //Financial - 4

        var prepaymentReviewed = $("#PrepaymentReviewed").val();  
        var uncertaintyCommunicated = $("#UncertaintyCommunicated").val();
        var financialImpact = $("#FinancialImpact").val();
        

        const financeData = {
            labels: ['Prepayment Reviewed', 'Uncertainty Communicated', 'Financial Impact'],
            datasets: [
                {
                    data: [prepaymentReviewed, uncertaintyCommunicated, financialImpact],
                    backgroundColor: ['#007bff', '#ffc107', '#2E8A99'],
                    borderColor: ['#007bff', '#ffc107', '#2E8A99'],
                    borderWidth: 1
                }
            ]
        };

        const financeOptions = {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom'
                },
                tooltip: {
                    callbacks: {
                        label: function (tooltipItem) {
                            let value = financeData.datasets[0].data[tooltipItem.dataIndex];
                            return ` ${financeData.labels[tooltipItem.dataIndex]}: ${value}`;
                        }
                    }
                }
            }
        };

        new Chart(document.getElementById('financePieChart'), {
            type: 'pie',
            data: financeData,
            options: financeOptions
        });


        //issueBarChart - 5

        //var totalDeadLine = 50;
        var issueBeforeDeadLine = $("#BeforeDeadLineIssue").val();
        var issueDeadlineLapsed = $("#MissDeadLineIssues").val();

        const issuebarData = {

            labels: [/*'Total DeadLine',*/ 'Issue BeforeDeadLine', 'Issue DeadlineLapsed'],

            datasets: [
                {
                    label: 'Issue Data',
                    data: [/*totalDeadLine,*/ issueBeforeDeadLine, issueDeadlineLapsed],
                    backgroundColor: [
                        //'#FF5733',  
                        '#33FF57',  
                        '#3357FF',  

                    ],
                    borderColor: [
                        //'#FF5733',  
                        '#33FF57',  
                        '#3357FF',  

                    ],
                    borderWidth: 1
                }
            ],

        };

        const issueBarOptions = {
            scales: {
                y: {
                    beginAtZero: true
                },
                x: {
                    barPercentage: 0.2,
                    categoryPercentage: 0.5
                }
            },
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                }
            }
        };


        new Chart(document.getElementById('issueBarChart'), {
            type: 'bar',
            data: issuebarData,
            options: issueBarOptions
        });




        //AuditIssue Reporting Category - 6


        //var categorylist = [];
        //var totalCountArrayData = [];
        //$.ajax({
        //    url: '/Deshboard/GetIssueCategoryData',
        //})
        //    .done((result) => {
        //        debugger;
        //        categorylist = result;

        //        result.forEach(item => {
        //            totalCountArray.push(item.totalCount);
        //        });
        //    })
        //    .fail(() => {
        //        console.error("AJAX request failed.");
        //    });

        var Investigation = 45;
        var StrategicMeeting = 56;
        var ManagementReviewMeeting = 20;
        var OtherMeeting = 50; 
        var Training = 50; 

        const meetingData = {
            labels: ['Investigation', 'Strategic Meeting', 'Management Review Meeting', 'Other Meeting',"Training"],
            datasets: [
                {
                    data: [Investigation, StrategicMeeting, ManagementReviewMeeting, OtherMeeting, Training],
                    backgroundColor: ['#9BCF53', '#FFA500', '#32CD32', '#FF6384','#33FF57'], 
                    borderColor: ['#9BCF53', '#FFA500', '#32CD32', '#FF6384','33FF57'],
                    borderWidth: 1
                }
            ]
        };

        const meetingOptions = {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom'
                },
                tooltip: {
                    callbacks: {
                        label: function (tooltipItem) {
                            let value = meetingData.datasets[0].data[tooltipItem.dataIndex];
                            return ` ${meetingData.labels[tooltipItem.dataIndex]}: ${value}`;
                        }
                    }
                }
            }
        };
        new Chart(document.getElementById('meetingPieChart'), {
            type: 'pie',
            data: meetingData,
            options: meetingOptions
        });


        //IssueProcess - 6

        var categorylist = [];
        var totalCountArray = [];
        $.ajax({
            url: '/Deshboard/GetIssueCategoryData',
        })
            .done((result) => {
                debugger;
                categorylist = result;

                result.forEach(item => {
                    totalCountArray.push(item.totalCount);
                });
            })
            .fail(() => {
                console.error("AJAX request failed.");
            });



        var Compliance = 45;
        var Financial = 56;
        var Operational = 20;

        var arr = [];
        arr[0] = 34;
        arr[1] = 56;
        arr[2] = 20;

        
        var interval = setInterval(function () {
            if (totalCountArray.length > 0) {
                clearInterval(interval);  

                const issueProcessData = {
                    labels: ['Compliance', 'Financial', 'Operational'],
                    datasets: [
                        {
                            data: [totalCountArray[5], totalCountArray[6], totalCountArray[7]],  
                            backgroundColor: ['#9BCF53', '#FFA500', '#32CD32'],
                            borderColor: ['#9BCF53', '#FFA500', '#32CD32'],
                            borderWidth: 1
                        }
                    ]
                };

                const issueProcessOptions = {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        legend: {
                            position: 'bottom'
                        },
                        tooltip: {
                            callbacks: {
                                label: function (tooltipItem) {
                                    let value = issueProcessData.datasets[0].data[tooltipItem.dataIndex];
                                    return ` ${issueProcessData.labels[tooltipItem.dataIndex]}: ${value}`;
                                }
                            }
                        }
                    }
                };

      
                new Chart(document.getElementById('issueProcessPieChart'), {
                    type: 'pie',
                    data: issueProcessData,
                    options: issueProcessOptions
                });

            }
        }, 100); 



        //var categorylist = [];
        //var totalCountArray = [];
        //$.ajax({
        //    url: '/Deshboard/GetIssueCategoryData',
        //})
        //    .done((result) => {
        //        debugger;
        //        categorylist = result;

        //        result.forEach(item => {
        //            totalCountArray.push(item.totalCount);
        //        });
        //    })
        //    .fail();


        //debugger


        //var Compliance = 45;
        //var Financial = 56;
        //var Operational = 20;

        //var arr = [];
        //arr[0] = 34;
        //arr[1] = 56;
        //arr[2] = 20;


        //const issueProcessData = {
        //    labels: ['Compliance', 'Financial', 'Operational'],
        //    datasets: [
        //        {
        //            data: [arr[0], arr[1], totalCountArray[7]],
        //            backgroundColor: ['#9BCF53', '#FFA500', '#32CD32'],
        //            borderColor: ['#9BCF53', '#FFA500', '#32CD32'],
        //            borderWidth: 1
        //        }
        //    ]
        //};

        //const issueProcessOptions = {
        //    responsive: true,
        //    maintainAspectRatio: false,
        //    plugins: {
        //        legend: {
        //            position: 'bottom'
        //        },
        //        tooltip: {
        //            callbacks: {
        //                label: function (tooltipItem) {
        //                    let value = meetingData.datasets[0].data[tooltipItem.dataIndex];
        //                    return ` ${meetingData.labels[tooltipItem.dataIndex]}: ${value}`;
        //                }
        //            }
        //        }
        //    }
        //};

        //new Chart(document.getElementById('issueProcessPieChart'), {
        //    type: 'pie',
        //    data: issueProcessData,
        //    options: issueProcessOptions
        //});




        //last
        debugger;
        var branchList = {};
        var labels = [];
        var auditData = [];
        var issueData = [];

        $.ajax({
            url: '/Deshboard/GetTotalBranchWithAuditCount',
            method: 'GET',
            dataType: 'json'
        })
            .done((result) => {
                debugger;
                branchList = result;
                labels = result.map(branch => branch.branchName);  // Fill labels dynamically
                auditData = result.map(branch => branch.totalAudits);  // Fill audit data
                issueData = result.map(branch => branch.totalIssues);  // Fill issue data

                // Now that we have the data, initialize the chart
                const ctx = document.getElementById('auditChart').getContext('2d');

                new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: labels,  // Use the full array of labels
                        datasets: [
                            {
                                label: 'Total Audit',
                                data: auditData,  // Use the full array of audit data
                                backgroundColor: 'blue'
                            },
                            {
                                label: 'Total Issues',
                                data: issueData,  // Use the full array of issue data
                                backgroundColor: 'red'
                            }
                        ]
                    },
                    options: {
                        responsive: true,
                        plugins: {
                            legend: {
                                display: true
                            }
                        },
                        scales: {
                            x: { stacked: true },
                            y: { stacked: true }
                        }
                    }
                });

                console.log("Chart updated with:", labels, auditData, issueData);
            })
            .fail((jqXHR, textStatus, errorThrown) => {
                console.error("AJAX Error:", textStatus, errorThrown);
            });






        //var branchList = {};
        //var labels = [];
        //var auditData = [];
        //var issueData = [];

        //$.ajax({
        //    url: '/Deshboard/GetTotalBranchWithAuditCount', 
        //    method: 'GET',
        //    dataType: 'json'
        //})
        //    .done((result) => {
        //        debugger;
        //        branchList = result;
        //        var labelsdata = [`${result[0].branchName}`];
        //        labels = result.map(branch => branch.branchName);  
        //        auditData = result.map(branch => branch.totalAudits); 
        //        issueData = result.map(branch => branch.totalIssues); 
        //    })
        //    .fail((jqXHR, textStatus, errorThrown) => {
        //        console.error("AJAX Error:", textStatus, errorThrown);
        //    });



        //const ctx = document.getElementById('auditChart').getContext('2d');

        //new Chart(ctx, {
        //    type: 'bar',

        //    data: {
        //        labels:
        //            [labels[0]
        //        ],
        //        datasets: [
        //            {
        //                label: 'Total Audit',
        //                data: [auditData[0]],
        //                backgroundColor: 'blue'
        //            },
        //            {
        //                label: 'Total Issues',
        //                data: [issueData[0]],
        //                backgroundColor: 'red'
        //            }
        //        ]
        //    },


        //    options: {
        //        responsive: true,
        //        plugins: {
        //            legend: {
        //                display: true
        //            }
        //        },
        //        scales: {
        //            x: { stacked: true }, 
        //            y: { stacked: true } 
        //        }
        //    }
        //});




    }

    
    //End Init


    debugger;
    let value = $("#preValue").text();       
    let number = parseFloat(value);
    var val = Number(parseFloat(number).toFixed(2)).toLocaleString('en', { minimumFractionDigits: 2 });
    $("#preValue").text(val);
    //
    let corrAmountValue = $("#corrAmount").text();
    let corrAmountNumber = parseFloat(corrAmountValue);
    var corrAmountVal = Number(parseFloat(corrAmountNumber).toFixed(2)).toLocaleString('en', { minimumFractionDigits: 2 });
    $("#corrAmount").text(corrAmountVal);
    //
    let addPaymentValue = $("#addPayment").text();
    let addPaymentNumber = parseFloat(addPaymentValue);
    var addPaymentVal = Number(parseFloat(addPaymentNumber).toFixed(2)).toLocaleString('en', { minimumFractionDigits: 2 });
    $("#addPayment").text(addPaymentVal);
    
    debugger;

    function drawOragnogram() {
        google.load("visualization", "1", { packages: ["orgchart"] });
        google.setOnLoadCallback(drawChart);
    }


    function drawChart() {
        $.ajax({
            type: "GET",
            url: "/Home/GetEmployees",
        })
            .done(function (result) {


                var data = new google.visualization.DataTable();
                data.addColumn('string', 'Entity');
                data.addColumn('string', 'ParentEntity');
                data.addColumn('string', 'ToolTip');
                for (var i = 0; i < result.length; i++) {
                    var employeeId = result[i][0].toString();
                    var employeeName = result[i][1];
                    var designation = result[i][2];
                    var reportingManager = result[i][3] != 0 ? result[i][3].toString() : '';

                    var row =
                        [
                            [

                                {
                                    v: employeeId,
                                    f: employeeName + '<div>(<span>' + designation + '</span>)</div><img src = "/Images/' + employeeId + '.jpg" />'
                                }, reportingManager, designation
                            ]

                        ]




                    data.addRows(row);
                }

                var chart = new google.visualization.OrgChart($("#chart")[0]);
                chart.draw(data, { allowHtml: true });
            })
            .fail(function () {
                alert('failed');
            });

    }

    function getEvents() {

        var events = [];
        $.ajax({
            type: "GET",
            url: "/calender/GetEvents",

        })
            .done(function (data) {

                GenerateCalender(data);


            })
            .fail(function (fail) {
                alert('failed');
            });

    }


    function GenerateCalender(events) {

        $('#calender').fullCalendar('destroy');
        $('#calender').fullCalendar({
            contentHeight: 400,
            defaultDate: new Date(),
            timeFormat: 'h(:mm)a',
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,basicWeek,basicDay,agenda'
            },
            eventLimit: true,
            eventColor: '#378006',
            events: events,
            eventClick: function (calEvent, jsEvent, view) {
               
            }
        })

    }


    $(document).ready(function () {
        $("#datepicker").datepicker({
            format: "yyyy",
            viewMode: "years",
            minViewMode: "years",
            autoclose: true
        });
    })

    $('#datepicker').off('change').on('change', function (e) {   
        debugger;
        let year =  $(this).val();
        $.ajax({
            url: '/Home/Index', 
            type: 'POST', 
            data: { IsShow : '', year: year },
            success: function (data) { 
                debugger;
                console.log(data);
                window.location.reload();
            },
            error: function (error) {
                console.error('Error updating AuditComponentList:', error);
            }
        });
    });



    return {
        init: init
    }

}();


