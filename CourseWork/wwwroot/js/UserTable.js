function AddButton() {

}
function EditButton(id) {
    window.location = `ChangeUser?id=${id}`;
}
function DeleteButton(id) {
    alert(id);
}
function RestoreButton(id) {
    alert(id);
}

let data = [];
let table = document.getElementById("data");
for (let i = 0, row; row = table.rows[i]; i++) {
    data.push({
        id: row.cells[0].innerHTML,
        login: row.cells[1].innerHTML,
        pib: row.cells[2].innerHTML,
        email: row.cells[3].innerHTML,
        phone: row.cells[4].innerHTML,
        rank: row.cells[5].innerHTML,
        degree: row.cells[6].innerHTML,
        chair: row.cells[7].innerHTML,
        access: row.cells[8].innerHTML,
        delete: row.cells[9].innerHTML,
        redact: row.cells[10].innerHTML,
        button2: row.cells[11].innerHTML
    });


}

$(document).ready(function () {
    $('#table_id').DataTable();
    $('#table_id').DataTable({
        paging: true
    });
});

$(document).ready(function () {
    $('#table_id').DataTable({
        "pagingType": "simple" // "simple" option for 'Previous' and 'Next' buttons only
    });
    $('.dataTables_length').addClass('bs-select');
});

console.log(data);