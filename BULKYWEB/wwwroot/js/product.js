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
                         <a  href="/admin/product/deletes?id=${data}"  class="btn btn-danger mx-2">
                                        <i class="bi bi-trash3"></i> Delete
                         </a>
                  </div>`
              }
          },
    ]
});
 
};

 
