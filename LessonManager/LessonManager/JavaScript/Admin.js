$(document).ready(() => {
    // Φόρτωση των δεδομένων στο DataTable
    // Για την φόρτωση των δεδομένων χρησιμοποιείται το endpoint /lessons/getAllLessons
    const table = $("#users_table").DataTable({
        ajax: "/user/getUsers?role=simpleUser",
        columns: [
            { data: "Id" },
            { data: "FullName" },
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return "<select class='roles'>" +
                        "<option value='teacher'>Διδάσκων</option>" +
                        "<option value='student'>Φοιτητής</option>" +
                        "</select>"
                        ;
                }
            },
            {
                "defaultContent": `<button type='button' style='float: right;' id='add-lesson' class=' d-flex btn btn-labeled btn-success f-right mb-4'>
                    <span class= 'btn-label mr-2'>
                        <i class='fa fa-user mr-2'></i>
                    </span> Αποδοχή Χρήστη
                </button>`
            },
        ],
    }
    );

    $('#users_table tbody').on('click', 'button', function () {
        var data = table.row($(this).parents('tr')).data();
        console.log($(this).parents('tr').val());
    });
})