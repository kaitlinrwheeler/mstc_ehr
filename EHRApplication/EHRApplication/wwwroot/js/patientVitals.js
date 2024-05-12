// Adds event listener to delete button to delete ins;urance record without having to refresh the page.
document.addEventListener('DOMContentLoaded', function () {
    const deleteButtons = document.querySelectorAll('.delete-button');
    const modal = new bootstrap.Modal(document.getElementById('deleteConfirmModal'));
    const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');
    let patientVitalId;

    deleteButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault(); // Prevent the default form submission
            patientVitalId = this.getAttribute('data-patientVitalId');
            const vitalDate = this.getAttribute('data-vital-date');
            const vitalTime = this.getAttribute('data-vital-time');
            message = vitalDate + ' at ' + vitalTime;
            document.getElementById('messagePlaceholder').textContent = message; // Update the name placeholder
            modal.show();
        });
    });

    confirmDeleteBtn.addEventListener('click', function () {
        fetch(`/Patient/DeleteVitals?vitalsId=${patientVitalId}`, {
            method: 'POST',
        })
            .then(response => {
                if (response.ok) {
                    // Row to remove
                    const row = document.getElementById(`row-${patientVitalId}`);
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