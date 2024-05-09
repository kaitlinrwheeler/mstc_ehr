// Adds event listener to delete button to delete patient allergy record
document.addEventListener('DOMContentLoaded', function () {
    const deleteButtons = document.querySelectorAll('.delete-button');
    const modal = new bootstrap.Modal(document.getElementById('deleteConfirmModal'));
    const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');
    let patientAllergyId;

    deleteButtons.forEach(button => {
        button.addEventListener('click', function (event) {
            event.preventDefault(); // Prevent the default form submission
            patientAllergyId = this.getAttribute('data-patientAllergyId');
            currentAllergyName = this.getAttribute('data-name'); // Get the name
            document.getElementById('allergyNamePlaceholder').textContent = currentAllergyName; // Update the name placeholder
            modal.show();
        });
    });

    confirmDeleteBtn.addEventListener('click', function () {
        const formId = `deleteForm-${patientAllergyId}`;

        fetch(`/Patient/DeletePatientAllergy?patientAllergyId=${patientAllergyId}`, {
            method: 'POST',
        })
            .then(response => {
                if (response.ok) {
                    // Row to remove
                    const row = document.getElementById(`row-${patientAllergyId}`);
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