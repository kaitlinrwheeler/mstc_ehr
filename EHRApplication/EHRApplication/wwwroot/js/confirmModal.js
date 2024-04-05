document.addEventListener('DOMContentLoaded', function () {
    const confirmBtn = document.getElementById('confirmBtn');

    confirmBtn.addEventListener('click', function () {
        document.getElementById('confirmModal').classList.remove('show');
        document.getElementById('confirmModal').style.display = 'none';
        document.querySelector('.modal-backdrop').remove();
    });
});