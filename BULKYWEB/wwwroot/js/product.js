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
            url: '/Admin/Product/getall',
            dataSrc: 'data'
        },
        columns: [
            { data: "name" },
            { data: "price" },
            { data: "author" },
            { data: "isbn" },
            {
                data: "category",
                render: function (data) {
                    return data ? data.name : "";
                }
            },
            { data: "displayOrder" },
            {
                data: "id",
                render: function (data) {
                    return `
                        <div class="w-75 btn-group" role="group"> 
                            <a href="/admin/product/upsert?id=${data}" 
                               class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-fill"></i> Edit
                            </a>
                            <a onClick=Delete('/admin/product/delete?id=${data}') 
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
        pageLength: 10,                      // ✅ default rows per page
        lengthMenu: [10, 25, 50, 100],   // ✅ user can choose
        responsive: true,
        autoWidth: true                    // ✅ prevent sWidth issue
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
