//// Adds event listener to delete button to delete patients without having to refresh the page.
//document.addEventListener('DOMContentLoaded', function () {
//    const deleteButtons = document.querySelectorAll('.delete-button');
//    const modal = new bootstrap.Modal(document.getElementById('deleteConfirmModal'));
//    const confirmDeleteBtn = document.getElementById('confirmDeleteBtn');
//    let currentMhn;
//    let currentPatientName;

//    deleteButtons.forEach(button => {
//        button.addEventListener('click', function (event) {
//            event.preventDefault(); // Prevent the default form submission
//            currentMhn = this.getAttribute('data-mhn');
//            currentPatientName = this.closest('tr').querySelector('td:nth-child(2)').textContent + " " + this.closest('tr').querySelector('td:nth-child(3)').textContent; // Get the first and last name of the patient
//            document.getElementById('patientNamePlaceholder').textContent = currentPatientName; // Update the patient's name placeholder
//            modal.show();
//        });
//    });

//    confirmDeleteBtn.addEventListener('click', function () {
//        const formId = `deleteForm-${currentMhn}`;
//        const form = document.getElementById(formId);

//        fetch(form.action, {
//            method: 'POST',
//            headers: {
//                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value,
//                'Content-Type': 'application/x-www-form-urlencoded'
//            },
//            body: `mhn=${currentMhn}`
//        })
//            .then(response => {
//                if (response.ok) {
//                    // If the server returns a success status, remove the row from the table
//                    document.getElementById(`patient-${currentMhn}`).remove();
//                } else {
//                    // Not the cleanest way to throw an error to the user, but maybe this will get changed later across the board.
//                    alert('Failed to delete the patient. Please try again.');
//                }
//            })
//            .catch(error => {
//                console.error('There was a problem with your fetch operation:', error);
//                alert('Failed to delete the patient. Please try again.');
//            })
//            .finally(() => {
//                modal.hide();
//            });
//    });
//});
