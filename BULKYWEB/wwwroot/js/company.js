var datatable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $('#myTable').DataTable({
        ajax: {
            url: '/Admin/company/getall',
            dataSrc: 'data'
        },
        columns: [
            { data: "name" },
            { data: "email" },
            { data: "phoneNumber" },
            { data: "city" },
            { data: "streetAddress" },
            { data: "postalCode" },
            {
                data: "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group"> 
                          <a href="/admin/company/upsert?id=${data}" class="btn btn-primary mx-2">
                                        <i class="bi bi-pencil-fill"></i> Edit
                         </a>
                         <a  onClick=Delete('/admin/company/deletes?id=${data}')  class="btn btn-danger mx-2">
                                        <i class="bi bi-trash3"></i> Delete
                         </a>
                  </div>`
                }
            },
        ]
    });
};

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
                }
            })
        }
    });
} 
