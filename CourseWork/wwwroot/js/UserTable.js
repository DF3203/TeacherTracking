function AddButton() {
    window.location = `ChangeUser?id=${-1}`;
}
let deleteModal = new bootstrap.Modal(document.getElementById('deleteBackDrop'), { backdrop: true, keyboard: true, focus: true });
let restoreModal = new bootstrap.Modal(document.getElementById('restoreModal'), { backdrop: true, keyboard: true, focus: true });
let Id = 0;

document.getElementById('confirmDeleteBtn').addEventListener("click", function (e) {
    DeleteUser();
});

document.getElementById('restoreConfirm').addEventListener("click", function (e) {
    restoreModal.toggle();
    alert(document.querySelector("#username").value);
    alert(document.querySelector("#password").value);
});


//document.getElementById('deleteBackDrop').addEventListener('hidden.bs.modal', function (event) {
//    alert("Закриття");
//})

async function DeleteUser() {
    deleteModal.toggle();
    const query = await fetch(`https://localhost:7113/Main/DeleteUser?id=${Id}`);
}
function EditButton(id) {
    window.location = `ChangeUser?id=${id}`;
}
function DeleteButton(id) {
    Id = id;
    deleteModal.toggle();
}
function RestoreButton(id) {
    Id = id;
    restoreModal.toggle();
}
