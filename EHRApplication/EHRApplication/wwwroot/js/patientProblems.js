﻿
// Adds event listener to delete button to delete ins;urance record without having to refresh the page.
document.addEventListener('DOMContentLoaded', function () {
    const deleteButtons = document.querySelectorAll('.delete-button');
    const modal = new bootstrap.Modal(document.getElementById('deleteConfirmModal'));
    const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');
    let patientProblemId;

    deleteButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault(); // Prevent the default form submission
            patientProblemId = this.getAttribute('data-patientProblemId');
            message = this.getAttribute('data-message'); // Get the name
            document.getElementById('messagePlaceholder').textContent = message; // Update the name placeholder
            modal.show();
        });
    });

    confirmDeleteBtn.addEventListener('click', function () {
        fetch(`/Problems/DeletePatientProblem?problemId=${patientProblemId}`, {
            method: 'POST',
        })
            .then(response => {
                if (response.ok) {
                    // Row to remove
                    const row = document.getElementById(`row-${patientProblemId}`);
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