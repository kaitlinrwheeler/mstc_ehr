﻿
// Adds event listener to active status switch boxes to handle database changes.
document.addEventListener('DOMContentLoaded', function () {
    // Gets all active checkboxes/sliders
    const checkboxes = document.querySelectorAll('.active-status-checkbox');

    checkboxes.forEach(checkbox => {
        checkbox.addEventListener('change', function (event) {
            event.preventDefault(); // Prevent the default form submission
            const medId = this.getAttribute('data-medId'); // Only the one that was selected
            const isActive = this.checked; // New switch position
            // Selects the label associated with the checkbox that was changed
            const statusLabel = this.parentElement.querySelector('.form-check-label');

            //This will go to the function in the controller and update the database but not the page as it would require a reload and looks bad on the page.
            fetch(`/Medications/UpdateActiveStatusForMedProfile?medId=${medId}&activeStatus=${isActive}`, {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
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