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
          { data: "category.displayOrder" }
    ]
});
 
};

 
