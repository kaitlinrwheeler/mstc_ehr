// Validation functions

const xIcon = '<i class="fa-solid fa-circle-xmark"></i> ';

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
        errorSpan.innerHTML = xIcon + errorMessage;
        errorSpan.style.display = 'inline';
    } else {
        errorSpan.style.display = 'none';
    }
}


// call to validate a date picker input
function validateDatePicker(inputID, errorWords) {
    const input = document.getElementById(inputID);
    const errorSpanID = inputID + 'Error';

    // Validate empty input
    const isEmptyValid = isEmpty(input, errorSpanID, `Please enter a valid ${errorWords}.`);

    if (isEmptyValid) {
        const inputValue = new Date(input.value);

        // Validate date range
        return validateDateRange(inputValue, errorSpanID);
    }

    return false; // Return false if input is empty
}

//get called by the validateDatePicker() function to check for past/future dates.
function validateDateRange(inputValue, errorSpanID) {
    const currentDate = new Date();
    const minDate = new Date(1900, 0, 1); // January 1, 1900

    // Check if input date is after 1900
    const isAfter1900 = inputValue > minDate;

    // Check if input date is before current date
    const isBeforeToday = inputValue <= currentDate;

    if (!isAfter1900) {
        document.getElementById(errorSpanID).textContent = 'Date must be after 1900.';
        document.getElementById(errorSpanID).style.display = 'inline'; // show error in span
        return false; 
    } else if (!isBeforeToday) {
        document.getElementById(errorSpanID).textContent = 'Date cannot be in the future.';
        document.getElementById(errorSpanID).style.display = 'inline'; //show error in span
        return false; 
    } else {
        // Hide error message if validation passes
        document.getElementById(errorSpanID).textContent = '';
        return true; 
    }
}

//Use when validating optional text inputs. Pass in the inputID and the words you would like to be displayed in the error message in.
function validateOptionalTextInput(inputID, errorMessage) {
    const input = document.getElementById(inputID);
    const inputErrorSpanID = inputID + 'Error';
    const errorWords = errorMessage;
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
                hasCharacterLimit(input, inputErrorSpanID, 25, `Please enter a ${errorWords} under 25 characters.`);
            }
        }
    }

    // Attach input change event listener
    input.addEventListener('input', handleInputChange);

    // Call handleInputChange initially to perform initial validation
    handleInputChange();
}

//Use when validating a required text input. Pass in the inputID and the words you would like to be displayed in the error message in.
function validateRequiredTextInput(inputID, errorMessage) {
    const input = document.getElementById(inputID);
    const inputErrorSpanID = inputID + 'Error';
    const errorWords = errorMessage;

    function handleInputChange() {
        // Check if input value is empty
        if (input.value.trim() === '') {
            // If input is empty call isempty function
            const isEmptyValid = isEmpty(input, inputErrorSpanID, `Please enter a ${errorWords}.`);
        } else {
            // If input is not empty, perform validation
            const isAlphabeticValid = isAlphabetic(input, inputErrorSpanID, 'Please enter alphabetic characters only.');
            if (isAlphabeticValid) {
                // Validate character limit
                hasCharacterLimit(input, inputErrorSpanID, 25, `Please enter a ${errorWords} under 25 characters.`);
            }
        }
    }

    // Attach input change event listener
    input.addEventListener('input', handleInputChange);

    // Call handleInputChange initially to perform initial validation
    handleInputChange();
}
