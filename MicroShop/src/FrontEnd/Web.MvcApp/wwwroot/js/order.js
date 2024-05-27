var dataTable;

$(document).ready(function () {
    const url = window.location.search;
    if (url.includes("approved")) {
        loadDataTable("approved");
    }
    else {
        if (url.includes("processing")) {
            loadDataTable("processing");
        }
        else {
            if (url.includes("cancelled")) {
                loadDataTable("cancelled");
            }
            else {
                loadDataTable("all");
            }
        }
    }
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        order: [[0, 'desc']],
        "ajax": { url: "/order/history?status=" + status },
        "columns": [
            { data: 'id', "width": "5%"},
            { data: 'status', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/order/detail?orderId=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i></a>
                    </div>`
                },
                "width": "10%"
            }
        ],
    })
}
