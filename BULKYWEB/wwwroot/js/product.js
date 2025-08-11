var datatable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable(){
  $('#myTable').DataTable({
    ajax: {
        url: '/Admin/Product/getall',
        dataSrc: 'data'
    },
      columns: [
        { data: "name" },
          { data: "price" },
          { data: "author" },
          { data: "isbn" },
          { data: "category.name" },
          { data: "category.displayOrder" },
          {
              data: "id",
              "render": function (data) {
                  return `<div class="w-75 btn-group" role="group"> 
                          <a href="/admin/product/upsert?id=${data}" class="btn btn-primary mx-2">
                                        <i class="bi bi-pencil-fill"></i> Edit
                         </a>
                         <a  onClick=Delete('/admin/product/deletes?id=${data}')  class="btn btn-danger mx-2">
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
