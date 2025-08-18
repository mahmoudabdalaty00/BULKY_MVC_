var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    // destroy old instance if exists
    if ($.fn.DataTable.isDataTable('#myTable')) {
        $('#myTable').DataTable().clear().destroy();
    }

    dataTable = $('#myTable').DataTable({
        ajax: {
            url: '/Admin/store/getall',
            dataSrc: 'data'
        },
        columns: [
            { data: "name" },
            { data: "email" },
            { data: "isActive" },
            { data: "city" },
            { data: "country" },           
            {
                data: "id",
                render: function (data) {
                    return `
                        <div class="w-75 btn-group" role="group"> 
                            <a href="/admin/Store/upsert?id=${data}" 
                               class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-fill"></i> Edit
                            </a>
                            <a onClick=Delete('/admin/store/delete?id=${data}') 
                               class="btn btn-danger mx-2">
                                <i class="bi bi-trash3"></i> Delete
                            </a>
                        </div>`;
                }
            }
        ],
        paging: true,
        searching: true,
        lengthChange: true,
        pageLength: 10,                       
        lengthMenu: [10, 25, 50, 100],    
        responsive: true,
        autoWidth: true                    
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#D9534F",
        cancelButtonColor: "#325D88",
        confirmButtonText: "Yes, delete it!",
        buttonsStyling: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                },
                error: function () {
                    toastr.error("Something went wrong while deleting.");
                }
            });
        }
    });
}
