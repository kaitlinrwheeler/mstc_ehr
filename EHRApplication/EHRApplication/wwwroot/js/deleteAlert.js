// Adds event listener to delete button to delete patient alert record
document.addEventListener('DOMContentLoaded', function () {
    const deleteButtons = document.querySelectorAll('.delete-button');
    const modal = new bootstrap.Modal(document.getElementById('deleteConfirmModal'));
    const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');
    let alertId;

    deleteButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault(); // Prevent the default form submission
            alertId = this.getAttribute('data-alertId');
            alertName = this.getAttribute('data-name'); // Get the name
            document.getElementById('alertPlaceholder').textContent = alertName; // Update the name placeholder
            modal.show();
        });
    });

    confirmDeleteBtn.addEventListener('click', function () {
        const formId = `deleteForm-${alertId}`;

        fetch(`/Patient/DeleteAlert?alertId=${alertId}`, {
            method: 'POST',
        })
            .then(response => {
                if (response.ok) {
                    // Row to remove
                    const row = document.getElementById(`row-${alertId}`);
                    row.remove();

                }
                else {
                    alert('Failed to delete. Please try again.');
                }
            })
            .finally(() => {
                modal.hide();
                window.location.reload();
            });
    });
});