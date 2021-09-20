$(document).ready(() => {
    $("#success-message").hide();
    $("#semester").select2();
    window.localStorage.clear();
    // Φόρτωση των δεδομένων στο DataTable
    // Για την φόρτωση των δεδομένων χρησιμοποιείται το endpoint /lessons/getAllLessons
<<<<<<< HEAD
    const classTable = $("#classes_table").DataTable({

        ajax: "/lessons/getClasses",
        dom: 'Bfrtip',
        buttons: [
          'excel'
        ],
=======
    const classTable =$("#classes_table").DataTable({
        
        ajax: "/lessons/getClasses",
>>>>>>> 5db4543b2e99eda124ff499176e33e0eb6f2c5f2
        columns: [
            { data: "Id" },
            { data: "LessonName" },
            { data: "TeacherName" },
            { data: "Semester" },
            { data: "ExamMark" },
            { data: "LabMark" },
            { data: "FinalMark"},
            { data: "Status" },
            {
                data:"Status",
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<input type="checkbox" class="editor-active">';
                    }
                    return data;
                },
                className: "dt-body-center"
            }
        ],
        select: {
            style: 'os',
            selector: 'td:not(:last-child)' // no row selection on last column
        },
        rowCallback: function (row, data) {
            // Set the checked state of the checkbox in the table
            $('input.editor-active', row).prop('checked', data.active == 1);
        }
    }
    );
    const dilosistable = $("#dilosis-table").DataTable({
        
        ajax: "/lessons/getClasses",
<<<<<<< HEAD
        dom: 'Bfrtip',
        buttons: [
            'excel'
        ],
=======
>>>>>>> 5db4543b2e99eda124ff499176e33e0eb6f2c5f2
        columns: [
            { data: "Id" },
            { data: "LessonName" },
            { data: "TeacherName" },
            { data: "Semester" },
            {
                data: "ExamMark",
                 render: function (data, type, row) {
                    if (type === 'display') {
                        return `<input name="exam" value="${data}" class="editor-active">`;
                    }
                    return data;
                },
                className: "dt-body-center"
            },
            {
                data: "LabMark",
                render: function (data, type, row) {
                    if (type === 'display') {
                        return `<input name="lab" value="${data}" class="editor-active">`;
                    }
                    return data;
                },
                className: "dt-body-center"
            },
            {
                data: "FinalMark",
               
            },
            {
                data: "Status",
                render: function (data, type, row) {
                    if (type === 'display') {
                        return '<Button type="button" class="oristikopoihsh btn btn-primary editor-active">Οριστιτικοποίηση βαθμολογίας</button';
                    }
                    return data;
                },
                className: "dt-body-center"
            }
        ],
        select: {
            style: 'os',
            selector: 'td:not(:last-child)' // no row selection on last column
        },
        rowCallback: function (row, data) {
            // Set the checked state of the checkbox in the table
            $('input.editor-active', row).prop('checked', data.active == 1);
        }
    }
    );

    $('#classes_table').on('change', 'input.editor-active', function () {
        var trData = classTable.row($(this).parents('tr')).data();
        var data = {
            id : trData.Id,
            status : ($(this).is(':checked')) ? 1 : 0,
        }
        $.post("/lessons/manageDilosi", data).then((response) => {
            if (response.status == "success") {
                window.close();
            } else {
                console.log("as");
            }
        });
        console.log(data);
    });

    $('#dilosis-table').on('change', 'input.editor-active', function () {
        var trData = dilosistable.row($(this).parents('tr')).data();
        window.localStorage.setItem($(this).attr('name'), $(this).val())
        if (
            window.localStorage.getItem('exam') != null &&
            window.localStorage.getItem('lab') != null) {
            let data = {
                labmark : window.localStorage.getItem('lab'),
                exammmark: window.localStorage.getItem('exam'),
                TeachClassId: trData.Id
            }
            $.post("/lessons/CalculataFinalMark", data).then((response) => {
                window.location.reload();
            });
        }
      
    });

    $('.oristikopoihsh').on('click', () => {
        console.log('lakii');
    })

    $("#teacher").select2({
        ajax: {
            url: "/user/getUsers?role=teacher&type=select2",
            dataType: "json"
        }
    });

    // Φόρτωση των δεδομένων στο DataTable
    // Για την φόρτωση των δεδομένων χρησιμοποιείται το endpoint /lessons/getAllLessons
    const table = $("#lessons_table").DataTable({
<<<<<<< HEAD
        dom: 'Bfrtip',
        buttons: [
            'excel'
        ],
=======
>>>>>>> 5db4543b2e99eda124ff499176e33e0eb6f2c5f2
            ajax: "/lessons/getAllLessons",
            columns: [
                { data: "Id" },
                { data: "Name" },
                { data: "Description" },
            ],
        }
    );    // Φόρτωση των δεδομένων στο DataTable
    // Για την φόρτωση των δεδομένων χρησιμοποιείται το endpoint /lessons/getAllLessons
    const table2 = $("#lessons_table2").DataTable({
<<<<<<< HEAD
        ajax: "/lessons/getAllLessons",
        dom: 'Bfrtip',
        buttons: [
            'csv', 'excel', 'pdf'
        ],
=======
            ajax: "/lessons/getAllLessons",
>>>>>>> 5db4543b2e99eda124ff499176e33e0eb6f2c5f2
            columns: [
                { data: "Id" },
                { data: "Name" },
                { data: "Description" },
            ],
        }
    );



    // Κατα την επιλογή ενός νέου μαθήματος άνοιγμα  των επιλογών του 
    // σε νέο παράθυρο για επεξεργασία ή και διαγραφή  
    $("#lessons_table tbody").on("click",
        "tr",
        function() {
            const data = table.row(this).data();
            window.open(`/lessons/overview?id=${data.Id}`);
        });

    $("#add-lesson").click(() => {
        $("#lesson-modal").show(500);
    });

    $("#delete-lesson").click(() => {
        const data = {
            id: $("#id").val()
        };
        try {
            $.post("/lessons/deleteLesson", data).then((response) => {
                if (response.status == "success") {
                    window.close();
                } else {
                    console.log("as");
                }
            });
        } catch (e) {
            console.log(e);
        }
    });

    $("#edit-class").click(() => {
        console.log($("#examMandatory").is(':selected'));
        const data = {
            id: $("#id").val(),
            examWeight: $("#examWeight").val(),
            labWeight: $("#labWeight").val(),
            labMandatory: ($("#labMandatory").is(':selected'))? true:false,
            examMandatory: $("#examMandatory").is(':selected') ? true : false,
        };
        try {
            $.post("/lessons/editClass", data).then((response) => {
                if (response.status == "success") {
                    $("#lesson-modal").hide();
                    $("#success-message").show(500);
                } else {
                    console.log("as");
                }
            });
        } catch (e) {
            console.log(e);
        }

    });

    $("#set-class").on("click",
        () => {
            const data = {
                teacher: $("#teacher").val(),
                semester: $("#semester").val(),
                lesson_id: $("#id").val(),
            };
            try {
                $.post("/lessons/makeClass", data).then((response) => {
                    if (response.status == "success") {
                        $("#lesson-modal").hide();
                        $("#success-message").show(500);
                    } else {
                        console.log("as");
                    }
                });
            } catch (e) {
                console.log(e);
            }
        });

    $("#close-modal").click(() => {
        $("#lessonlesson-modal").hide();
    });

    $("#edit-lesson , #submit-new-lesson").click(() => {
        const data = {
            id: ((typeof $("#id").val() != "undefined") ? $("#id").val() : 0),
            name: $("#name").val(),
            description: $("#description").val()
        };
        try {
            $.post("/lessons/addLesson", data).then((response) => {
                if (response.status == "success") {
                    $("#lesson-modal").hide();
                    $("#success-message").show(500);
                } else {
                    console.log("as");
                }
            });
        } catch (e) {
            console.log(e);
        }

    });
});