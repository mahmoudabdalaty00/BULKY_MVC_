var dataTable;

$(document).ready(function () {
    loadDataTable();

    // ✅ Delegated click event for lock/unlock buttons
    $('#tblData tbody').on('click', '.btn-lockunlock', function () {
        var button = $(this);
        var userId = button.data('id');
        if (!userId) return;

        $.ajax({
            type: "POST",
            url: '/Admin/User/LockUnlock',
            data: JSON.stringify(userId),
            contentType: "application/json",
            success: function (data) {
                if (data.success) {
                    // Toggle button instantly
                    if (button.hasClass('btn-danger')) {
                        button.removeClass('btn-danger').addClass('btn-success');
                        button.html('<i class="bi bi-unlock-fill"></i> Unlock');
                    } else {
                        button.removeClass('btn-success').addClass('btn-danger');
                        button.html('<i class="bi bi-lock-fill"></i> Lock');
                    }
                } else {
                    alert(data.message);
                }
            },
            error: function (err) {
                console.log(err);
                alert("Something went wrong.");
            }
        });
    });
});

// Initialize DataTable
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/user/getall' },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "company.name", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data": null, // full row
                "render": function (row) {
                    return renderLockButton(row);
                },
                "width": "25%"
            }
        ]
    });
}

// Helper function to render lock/unlock button
function renderLockButton(row) {
    var today = new Date().getTime();
    var lockout = row.lockoutEnd ? new Date(row.lockoutEnd).getTime() : 0;

    let isLocked = lockout > today;
    let buttonClass = isLocked ? "btn-danger" : "btn-success";
    let icon = isLocked ? "bi-lock-fill" : "bi-unlock-fill";
    let text = isLocked ? "Lock" : "Unlock";

    return `
        <div class="text-center">
            <button type="button" data-id="${row.id}" class="btn ${buttonClass} text-white btn-lockunlock" style="width:100px;">
                <i class="bi ${icon}"></i> ${text}
            </button>
            <a href="/admin/user/RoleManagement?userId=${row.id}" 
               class="btn btn-warning text-white" style="width:150px;">
               <i class="bi bi-pencil-square"></i> Permission
            </a>
        </div>`;
}
