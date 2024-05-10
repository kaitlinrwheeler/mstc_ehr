
// Adds event listener to delete button to delete ins;urance record without having to refresh the page.
document.addEventListener('DOMContentLoaded', function () {
    const deleteButtons = document.querySelectorAll('.delete-button');
    const modal = new bootstrap.Modal(document.getElementById('deleteConfirmModal'));
    const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');
    let labId;

    deleteButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault(); // Prevent the default form submission
            labId = this.getAttribute('data-labId');
            testDate = this.getAttribute('data-test-date');
            testName = this.getAttribute('data-test-name');
            message = testName + ' on ' + testDate;
            document.getElementById('messagePlaceholder').textContent = message; // Update the name placeholder
            modal.show();
        });
    });

    confirmDeleteBtn.addEventListener('click', function () {
        fetch(`/Lab/DeleteLabResults?resultId=${labId}`, {
            method: 'POST',
        })
            .then(response => {
                if (response.ok) {
                    // Row to remove
                    const row = document.getElementById(`row-${labId}`);
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