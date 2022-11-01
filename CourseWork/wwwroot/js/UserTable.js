function AddButton() {

}

let deleteModal = new bootstrap.Modal(document.getElementById('deleteBackDrop'), { backdrop: true, keyboard: true, focus: true });
let restoreModal = new bootstrap.Modal(document.getElementById('restoreModal'), { backdrop: true, keyboard: true, focus: true });
let Id = 0;

document.getElementById('confirmDeleteBtn').addEventListener("click", function (e) {
    deleteModal.toggle();
    alert(Id);
});

document.getElementById('restoreConfirm').addEventListener("click", function (e) {
    restoreModal.toggle();
    alert(Id);
    alert(document.querySelector("#username").value);
    alert(document.querySelector("#password").value);
});


//document.getElementById('deleteBackDrop').addEventListener('hidden.bs.modal', function (event) {
//    alert("Закриття");
//})


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
