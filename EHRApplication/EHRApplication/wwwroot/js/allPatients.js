
// Adds event listener to active status switch boxes to handle database changes.
document.addEventListener('DOMContentLoaded', function () {
    // Gets all active checkboxes/sliders
    const checkboxes = document.querySelectorAll('.active-status-checkbox');

    checkboxes.forEach(checkbox => {
        checkbox.addEventListener('change', function (event) {
            event.preventDefault(); // Prevent the default form submission
            const mhn = this.getAttribute('data-mhn'); // Only the one that was selected
            const isActive = this.checked; // New switch position
            // Selects the label associated with the checkbox that was changed
            const statusLabel = this.parentElement.querySelector('.form-check-label');

            fetch(`/Patient/UpdateActiveStatus?mhn=${mhn}&activeStatus=${isActive}`, {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                }
            })
                .then(response => {
                    if (response.ok) {
                        // If the server returns a success status, update the <span> text based on the new active status
                        statusLabel.textContent = isActive ? "Yes" : "No";
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

// Adds event listener to delete button to delete patients without having to refresh the page.
document.addEventListener('DOMContentLoaded', function () {
    const deleteButtons = document.querySelectorAll('.delete-button');
    const modal = new bootstrap.Modal(document.getElementById('deleteConfirmModal'));
    const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');
    let currentMhn;

    deleteButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault(); // Prevent the default form submission
            currentMhn = this.getAttribute('data-mhn');
            modal.show();
        });
    });

    confirmDeleteBtn.addEventListener('click', function () {
        const formId = `deleteForm-${currentMhn}`;
        const form = document.getElementById(formId);

        fetch(form.action, {
            method: 'POST',
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value,
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            body: `mhn=${currentMhn}`
        })
            .then(response => {
                if (response.ok) {
                    // If the server returns a success status, remove the row from the table
                    document.getElementById(`patient-${currentMhn}`).remove();
                } else {
                    // Not the cleanest way to throw an error to the user, but maybe this will get changed later across the board.
                    alert('Failed to delete the patient. Please try again.');
                }
            })
            .catch(error => {
                console.error('There was a problem with your fetch operation:', error);
                alert('Failed to delete the patient. Please try again.');
            })
            .finally(() => {
                modal.hide();
            });
    });
});