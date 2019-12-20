const data = [{name: "conf1", data: [{event: "ev1", attendance: 1}, {event: "ev2", attendance: 2}, {event: "ev3", attendance: 1}]},
    {name: "conf2", data: [{event: "ev1", attendance: 2}, {event: "ev2", attendance: 3}, {event: "ev3", attendance: 4}]}];



var myChart = document.getElementById('myChart').getContext('2d');
var eventsName = [];
var confsName = [];


let datasets = []
let datasetsEv = []

for (let d of data)
{
    let attendance = []
    for (let e of d.data)
    {
        if (! eventsName.includes(e.event))
        {
            eventsName.push(e.event)
        }
        attendance.push(e.attendance)

    }
    let data = {label: d.name, data: attendance, backgroundColor: getRandomColor()}
    datasets.push(data)


}

for (let d of dataEv)
{
    let attendance = []
    for (let e of d.data)
    {
        if (! confsName.includes(e.conference))
        {
            confsName.push(e.conference)
        }
        attendance.push(e.attendance)

    }

    let data = {label: d.name, data: attendance, backgroundColor: getRandomColor()}
    datasetsEv.push(data)


}


var massPopChart = new Chart(myChart, {
    type: "bar",
    data: {
        labels: eventsName,
        datasets: datasets
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