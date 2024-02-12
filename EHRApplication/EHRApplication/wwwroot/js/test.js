// Validation functions
function isEmpty(input, errorSpanID, errorMessage) {
    const errorSpan = document.getElementById(errorSpanID);
    let valid = true;
    if (input === null || input === undefined || input.value.trim() === '') {
        valid = false;
    }
    displayError(valid, errorSpan, errorMessage);
    return valid;
}

function isAlphabetic(input, errorSpanID, errorMessage) {
    const errorSpan = document.getElementById(errorSpanID);
    const alphabetRegex = /^[a-zA-Z]+$/;
    let valid = alphabetRegex.test(input.value.trim());
    displayError(valid, errorSpan, errorMessage);
    return valid;
}

function hasCharacterLimit(input, errorSpanID, characterLimit, errorMessage) {
    const errorSpan = document.getElementById(errorSpanID);
    let valid = input.value.trim().length <= characterLimit;
    displayError(valid, errorSpan, errorMessage);
    return valid;
}

function displayError(valid, errorSpan, errorMessage) {
    if (!valid) {
        errorSpan.textContent = errorMessage;
        errorSpan.style.display = 'inline';
    } else {
        errorSpan.style.display = 'none';
    }
}

function validateDatePicker() {
    const dobInput = document.getElementById('DOB');
    const dobErrorSpan = document.getElementById('DOBError');

    // Validate empty input
    const isEmptyValid = isEmpty(dobInput, 'DOBError', 'Please enter a valid DOB.');

    if (isEmptyValid) {
        const dobValue = new Date(dobInput.value);
        const currentDate = new Date();
        const minDate = new Date(1900, 0, 1); // January 1, 1900

        // Check if DOB is after 1900
        const isAfter1900 = dobValue > minDate;

        // Check if DOB is before today's date
        const isBeforeToday = dobValue <= currentDate;

        if (!isAfter1900) {
            dobErrorSpan.textContent = 'DOB must be after 1900.';
            dobErrorSpan.style.display = 'inline';
            return false; // Return false indicating validation failed
        } else if (!isBeforeToday) {
            dobErrorSpan.textContent = 'DOB cannot be after today\'s date.';
            dobErrorSpan.style.display = 'inline';
            return false; // Return false indicating validation failed
        } else {
            // Hide error message if validation passes
            dobErrorSpan.style.display = 'none';
            return true; // Return true indicating validation passed
        }
    }

    return false; // Return false if input is empty
}


function validateGenericDatePicker(inputID, errorID) {
    const dobInput = document.getElementById(inputID);
    const dobErrorSpan = inputID + 'Error';
    const errorWords = errorID;

    // Validate empty input
    const isEmptyValid = isEmpty(dobInput, dobErrorSpan, `Please enter a valid ${errorWords}.`);


    if (isEmptyValid) {
        const dobValue = new Date(dobInput.value);
        const currentDate = new Date();
        const minDate = new Date(1900, 0, 1); // January 1, 1900

        // Check if DOB is after 1900
        const isAfter1900 = dobValue > minDate;

        // Check if DOB is before today's date
        const isBeforeToday = dobValue <= currentDate;

        if (!isAfter1900) {
            dobErrorSpan.textContent = 'DOB must be after 1900.';
            dobErrorSpan.style.display = 'inline';
            return false; // Return false indicating validation failed
        } else if (!isBeforeToday) {
            dobErrorSpan.textContent = 'DOB cannot be after today\'s date.';
            dobErrorSpan.style.display = 'inline';
            return false; // Return false indicating validation failed
        } else {
            // Hide error message if validation passes
            dobErrorSpan.style.display = 'none';
            return true; // Return true indicating validation passed
        }
    }

    return false; // Return false if input is empty
}



function validateOptionalTextInput(inputID, errorID) {
    const input = document.getElementById(inputID);
    const inputErrorSpanID = inputID + 'Error';
    const errorWords = errorID;
    const inputErrorSpan = document.getElementById(inputErrorSpanID);

    // Function to handle input change event
    function handleInputChange() {
        // Check if input value is empty
        if (input.value.trim() === '') {
            // If input is empty, hide error message
            inputErrorSpan.style.display = 'none';
        } else {
            // If input is not empty, perform validation
            const isAlphabeticValid = isAlphabetic(input, inputErrorSpanID, 'Please enter alphabetic characters only.');
            if (isAlphabeticValid) {
                // Validate character limit
                const hasCharacterLimitValid = hasCharacterLimit(input, inputErrorSpanID, 25, `Please enter a ${errorWords} under 25 characters.`);
            }
        }
    }

    // Attach input change event listener
    input.addEventListener('input', handleInputChange);

    // Call handleInputChange initially to perform initial validation
    handleInputChange();
}
