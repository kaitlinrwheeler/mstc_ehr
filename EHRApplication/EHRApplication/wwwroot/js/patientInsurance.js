// Adds event listener to active status switch boxes to handle database changes.
document.addEventListener('DOMContentLoaded', function () {
    // Gets all active checkboxes/sliders
    const checkboxes = document.querySelectorAll('.active-status-checkbox');

    checkboxes.forEach(checkbox => {
        checkbox.addEventListener('change', function (event) {
            event.preventDefault(); // Prevent the default form submission
            const insuranceId = this.dataset.insuranceid; // Only the one that was selected
            const isActive = this.checked; // New switch position
            // Selects the label associated with the checkbox that was changed
            const statusLabel = this.parentElement.querySelector('.form-check-label');

            fetch(`/Patient/UpdateInsuranceActiveStatus?insuranceId=${insuranceId}&activeStatus=${isActive}`, {
                method: 'POST',
                headers: {
                }
            })
                .then(response => {
                    if (response.ok) {
                        // If the server returns a success status, update the <span> text based on the new active status
                        statusLabel.textContent = isActive ? "Active" : "Inactive";
                        // Do not revert the switch's state here; let it reflect the user's action
                    }
                    else {
                        this.checked = !isActive;
                        // Not the cleanest way to throw an error to the user, but maybe this will get changed later across the board.
                        alert('Failed to update the active status. Please try again.');
                    }
                })
        });
    });
})


// Adds event listener to delete button to delete ins;urance record without having to refresh the page.
document.addEventListener('DOMContentLoaded', function () {
    const deleteButtons = document.querySelectorAll('.delete-button');
    const modal = new bootstrap.Modal(document.getElementById('deleteConfirmModal'));
    const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');
    let insuranceId;

    deleteButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault(); // Prevent the default form submission
            insuranceId = this.getAttribute('data-insuranceId');
            currentInsuranceName = this.getAttribute('data-name'); // Get the name
            document.getElementById('insuranceNamePlaceholder').textContent = currentInsuranceName; // Update the name placeholder
            modal.show();
        });
    });

    confirmDeleteBtn.addEventListener('click', function () {
        const formId = `deleteForm-${insuranceId}`;

        fetch(`/Patient/DeletePatientInsurance?insuranceId=${insuranceId}`, {
            method: 'POST',
        })
        .then(response => {
            if (response.ok) {
                // Row to remove
                const row = document.getElementById(`row-${insuranceId}`);
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
