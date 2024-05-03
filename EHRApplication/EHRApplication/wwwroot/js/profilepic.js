// change default action of close button to hide error container
const closeButton = errorContainer.querySelector('.btn-close');
closeButton.addEventListener('click', function (event) {
    event.preventDefault();
    errorContainer.style.display = 'none';
});

// get error container and message span on page load
document.addEventListener('DOMContentLoaded', function () {
    const errorContainer = document.getElementById('errorContainer');
    const errorMessage = document.getElementById('errorMessage');

    // get file and mhn on file input change
    document.getElementById('fileInput').addEventListener('input', function (e) {
        if (e.target.id === 'fileInput') {
            let fileInput = e.target;
            let files = fileInput.files;
            let file = files[0];

            const mhn = this.getAttribute('data-mhn');
           
            var valid = validateFile(file, errorContainer, errorMessage);

            if (valid) {
                // set file and mhn for controller action
                var formData = new FormData();
                formData.append("file", file);
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
                            // update patient image file path
                            document.querySelector('.card-img-top').src = data.filePath;
                            window.location.reload();
                        } else {
                            displayErrorMessage(errorContainer, errorMessage, data.message);
                        }
                    })
                    .catch((error) => {
                        console.error('Error:', error);
                        displayErrorMessage(errorContainer, errorMessage, 'Failed to upload image: ' + error.message);
                    })
                    .finally(() => {
                        // clear input value for next submission
                        resetInput(fileInput);
                    });
            }
            
        };

    });
});

// validate file type and size
function validateFile(file, errorContainer, errorMessage) {
    var valid = true;
    // check for file existence
    if (!file) {
        displayErrorMessage(errorContainer, errorMessage, 'Please select a file to upload.');
        resetInput(fileInput);
        valid = false;
    }

    // check file size
    if (file.size > 4 * 1024 * 1024) {
        displayErrorMessage(errorContainer, errorMessage, 'File size must be less than 4MB.');
        resetInput(fileInput);
        valid = false;
    }

    // check file extension for type
    const allowedFileTypes = [".jpg", ".png"];
    const fileName = file.name;
    const extension = fileName.slice(fileName.lastIndexOf('.')).toLowerCase();
    if (!allowedFileTypes.includes(extension)) {
        displayErrorMessage(errorContainer, errorMessage, 'Image file must be of type .jpg or .png.');
        resetInput(fileInput);
        valid = false;
    }

    return valid;
};

// populate and display error message
function displayErrorMessage(errorContainer, errorMessage, message) {
    if (errorContainer) {
        errorContainer.style.display = 'none';
        errorMessage.textContent = '';
        errorMessage.textContent = message;
        errorContainer.style.display = 'block';
    }
};

// clear input value for next submission
function resetInput(fileInput) {
    fileInput.value = null;
    fileInput.type = '';
    fileInput.type = 'file';
}