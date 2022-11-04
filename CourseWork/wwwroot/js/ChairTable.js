function AddButton() {
    window.location = `ChangeChair?id=${-1}`;
}
let deleteModal = new bootstrap.Modal(document.getElementById('deleteBackDrop'), { backdrop: true, keyboard: true, focus: true });

let _Id = 0;

function DeleteButton(id) {
    _Id = id;
    deleteModal.toggle();
}

document.getElementById('confirmDeleteBtn').addEventListener("click", function (e) {
    DeleteChair();
});

async function DeleteChair() {
    deleteModal.toggle();
    const query = await fetch(`https://localhost:7113/Main/DeleteChair?id=${_Id}`);
    window.location = '';
}
function EditButton(id) {
    window.location = `ChangeChair?id=${id}`;
}
