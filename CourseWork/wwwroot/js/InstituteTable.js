function AddButton() {
    window.location = `ChangeInstitute?id=${-1}`;
}

let deleteModal = new bootstrap.Modal(document.getElementById('deleteBackDrop'), { backdrop: true, keyboard: true, focus: true });
let Id = 0;

document.getElementById('confirmDeleteBtn').addEventListener("click", function (e) {
    DeleteInstitute();
});



//document.getElementById('deleteBackDrop').addEventListener('hidden.bs.modal', function (event) {
//    alert("Закриття");
//})

async function DeleteInstitute() {
    deleteModal.toggle();
    const query = await fetch(`https://localhost:7113/Main/DeleteInstitute?id=${Id}`);
}
function EditButton(id) {
    window.location = `ChangeInstitute?id=${id}`;
}
function DeleteButton(id) {
    Id = id;
    deleteModal.toggle();
}
