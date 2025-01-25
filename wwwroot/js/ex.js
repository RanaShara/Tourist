function showmessage(id) {
    get_id = id;

    $('#del').modal('show');
}


function confirm_delete() {

    window.location.href = "DeletePackage?id=" + get_id

}
function confirm_deleteP() {

    window.location.href = "DeleteCity?id=" + get_id

}
document.addEventListener('DOMContentLoaded', function () {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    const tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    })
})