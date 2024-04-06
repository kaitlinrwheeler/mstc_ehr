// Adds event listener to active status switch boxes to handle database changes.
document.addEventListener('DOMContentLoaded', function () {
    // Gets all active checkboxes/sliders
    const checkboxes = document.querySelectorAll('.active-status-checkbox');

    checkboxes.forEach(checkbox => {
        checkbox.addEventListener('change', function (event) {
            event.preventDefault(); // Prevent the default form submission
            const testId = this.getAttribute('testId'); // Only the one that was selected
            const isActive = this.checked; // New switch position
            // Selects the label associated with the checkbox that was changed
            const statusLabel = this.parentElement.querySelector('.form-check-label');

            fetch(`/Lab/UpdatePatientActiveStatus?id=${testId}&activeStatus=${isActive}`, {
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