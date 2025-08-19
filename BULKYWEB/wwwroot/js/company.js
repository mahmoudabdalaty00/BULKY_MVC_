var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#myTable').DataTable({
        ajax: {
            url: '/Admin/Company/GetAll',
            dataSrc: 'data',
            error: function (xhr, error, thrown) {
                console.error('DataTables AJAX Error:', error);
                console.error('Response:', xhr.responseText);
                Swal.fire({
                    title: 'Error!',
                    text: 'Failed to load company data. Check the console for details.',
                    icon: 'error'
                });
            }
        },
        columns: [
            { data: "name", defaultContent: "" },
            { data: "email", defaultContent: "" },
            { data: "phoneNumber", defaultContent: "" },
            { data: "city", defaultContent: "" },
            { data: "streetAddress", defaultContent: "" },
            { data: "postalCode", defaultContent: "" },
            {
                data: "id",
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group"> 
                          <a href="/Admin/Company/Upsert?id=${data}" class="btn btn-primary mx-2">
                                        <i class="bi bi-pencil-fill"></i> Edit
                         </a>
                         <a onclick="Delete('/Admin/Company/Delete?id=${data}')" class="btn btn-danger mx-2">
                                        <i class="bi bi-trash3"></i> Delete
                         </a>
                  </div>`;
                }
            }
        ],
        processing: true,
        language: {
            processing: "Loading companies..."
        }
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
                    if (data.success) {
                        Swal.fire({
                            title: 'Deleted!',
                            text: data.message || 'Company has been deleted.',
                            icon: 'success',
                            timer: 2000,
                            showConfirmButton: false
                        });
                    } else {
                        Swal.fire({
                            title: 'Error!',
                            text: data.message || 'Error deleting company.',
                            icon: 'error'
                        });
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Delete Error:', error);
                    Swal.fire({
                        title: 'Error!',
                        text: 'Error deleting company',
                        icon: 'error'
                    });
                }
            });
        }
    });
}