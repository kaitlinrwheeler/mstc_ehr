// Adds event listener to delete button to delete ins;urance record without having to refresh the page.
document.addEventListener('DOMContentLoaded', function () {
    const deleteButtons = document.querySelectorAll('.delete-button');
    const modal = new bootstrap.Modal(document.getElementById('deleteConfirmModal'));
    const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');
    let medAdminId;

    deleteButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault(); // Prevent the default form submission
            medAdminId = this.getAttribute('data-admin-id');
            const medName = this.getAttribute('data-med-name');
            const adminDate = this.getAttribute('data-admin-date');
            const adminTime = this.getAttribute('data-admin-time');
            message = medName + ' on ' + adminDate + ' at ' + adminTime;
            document.getElementById('messagePlaceholder').textContent = message; // Update the name placeholder
            modal.show();
        });
    });

    confirmDeleteBtn.addEventListener('click', function () {
        fetch(`/Medication/DeleteMedAdminHistory?administrationId=${medAdminId}`, {
            method: 'POST',
        })
            .then(response => {
                if (response.ok) {
                    // Row to remove
                    const row = document.getElementById(`row-${medAdminId}`);
                    row.remove();
                }
                else {
                    alert('Failed to delete. Please try again.');
                }
            })
            .finally(() => {
                modal.hide();
            });
    });
});