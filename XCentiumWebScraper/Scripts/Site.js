$(function () {
    var ctx = document.getElementById("wordChart").getContext('2d');

    var wc_word_1 = document.getElementById("wc_word_1").value,
        wc_word_2 = document.getElementById("wc_word_2").value,
        wc_word_3 = document.getElementById("wc_word_3").value,
        wc_word_4 = document.getElementById("wc_word_4").value,
        wc_word_5 = document.getElementById("wc_word_5").value,
        wc_word_6 = document.getElementById("wc_word_6").value,
        wc_word_7 = document.getElementById("wc_word_7").value,
        wc_word_8 = document.getElementById("wc_word_8").value,
        wc_word_9 = document.getElementById("wc_word_9").value,
        wc_word_10 = document.getElementById("wc_word_10").value;
    var wc_count_1 = document.getElementById("wc_count_1").value,
        wc_count_2 = document.getElementById("wc_count_2").value,
        wc_count_3 = document.getElementById("wc_count_3").value,
        wc_count_4 = document.getElementById("wc_count_4").value,
        wc_count_5 = document.getElementById("wc_count_5").value,
        wc_count_6 = document.getElementById("wc_count_6").value,
        wc_count_7 = document.getElementById("wc_count_7").value,
        wc_count_8 = document.getElementById("wc_count_8").value,
        wc_count_9 = document.getElementById("wc_count_9").value,
        wc_count_10 = document.getElementById("wc_count_10").value;
    

    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [wc_word_1, wc_word_2, wc_word_3, wc_word_4, wc_word_5, wc_word_6, wc_word_7, wc_word_8, wc_word_9, wc_word_10],
            datasets: [{
                data: [wc_count_1, wc_count_2, wc_count_3, wc_count_4, wc_count_5, wc_count_6, wc_count_7, wc_count_8, wc_count_9, wc_count_10],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: true,
            legend: {
                display: false,
            },
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
});