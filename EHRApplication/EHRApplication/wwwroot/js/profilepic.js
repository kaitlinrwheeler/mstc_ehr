function uploadFile(files, mhn) {
    if (files.length === 0) {
        displayErrorMessage('Please select a file to upload.');
        return;
    }

    var formData = new FormData();
    formData.append("file", files[0]);
    formData.append("mhn", mhn);

    fetch('/Patient/EditProfilePicture', {
        method: 'POST',
        body: formData
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log('Success:', data);
            if (data.success) {
                // Update the image on the page with the new file path returned from the server
                document.querySelector('.card-img-top').src = data.filePath;
                //alert('Image uploaded successfully.');
                window.location.reload();
            } else {
                //alert('Upload failed: ' + data.message);
                displayErrorMessage(data.message);
            }
        })
        .catch((error) => {
            console.error('Error:', error);
            //alert('Failed to upload image: ' + error.message);
            displayErrorMessage('Failed to upload image: ' + error.message);
        });
}

function displayErrorMessage(message) {
    const errorContainer = document.getElementById('errorContainer');
    const errorMessage = document.getElementById('errorMessage');
    errorMessage.textContent = message;
    errorContainer.style.display = 'block'; // Make the error container visible
}

document.addEventListener('DOMContentLoaded', function () {
    const fileInput = document.getElementById('fileInput');
    const errorContainer = document.getElementById('errorContainer');

    fileInput.addEventListener('change', () => {
        errorContainer.style.display = 'none'; // Hide the error container
    });
});