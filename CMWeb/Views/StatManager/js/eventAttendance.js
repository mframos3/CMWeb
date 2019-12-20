
const dataEv = [{name: "ev1", data: [{conference: "conf1", attendance: 1}, {conference: "conf2", attendance: 2}]},
    {name: "ev2", data: [{conference: "conf1", attendance: 2}, {conference: "conf2", attendance: 3}]},
    {name: "ev3", data: [{conference: "conf1", attendance: 1}, {conference: "conf2", attendance: 4}]}]

var eventsAttendance = [];

var myChart2 = document.getElementById('myChart1').getContext('2d');

var massPopChart2 = new Chart(myChart2, {
    type: "bar",
    data: {
        labels: confsName,
        datasets: datasetsEv
    },
    options: {
        scales: {
            yAxes: [{
                ticks: {
                    beginAtZero: true,
                    precision: 0
                }
            }]
        },
        title: {
            display: true,
            text: "Attendance",
            fontSize: 25
        },
        legend: {
            position: "right",
            labels : {
                fontColor: "#000"
            }
        }
    }
});