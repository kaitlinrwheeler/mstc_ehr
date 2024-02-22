// Validation functions

//variable for the 'X' symbol for error messages.
const xIcon = '<i class="fa-solid fa-circle-xmark"></i> ';

// Function to check if the input value is empty
function isEmpty(input, errorSpanID, errorMessage) {
    const errorSpan = document.getElementById(errorSpanID);
    let valid = true;

    // Check if the input value is empty
    if (input === null || input === undefined || input.value.trim() === '') {
        valid = false;
    }

    // Display error message if input is empty
    displayError(valid, errorSpan, errorMessage);

    return valid;
}

// Function to check if the input value contains only alphabetic characters
function isAlphabetic(input, errorSpanID, errorMessage) {
    const errorSpan = document.getElementById(errorSpanID);
    const alphabetRegex = /^[a-zA-Z]+$/;
    let valid = alphabetRegex.test(input.value.trim());

    // Display error message if input contains non-alphabetic characters
    displayError(valid, errorSpan, errorMessage);

    return valid;
}

// Function to check if the input value exceeds a character limit
function hasCharacterLimit(input, errorSpanID, characterLimit, errorMessage) {
    const errorSpan = document.getElementById(errorSpanID);
    let valid = input.value.trim().length <= characterLimit;

    // Display error message if input exceeds character limit
    displayError(valid, errorSpan, errorMessage);

    return valid;
}

// Function to display or hide the error message based on validation result
function displayError(valid, errorSpan, errorMessage) {
    if (!valid) {
        // Display error message with an icon
        errorSpan.innerHTML = xIcon + errorMessage;
        errorSpan.style.display = 'inline';
    } else {
        // Hide error message
        errorSpan.style.display = 'none';
    }
}

// call to validate a date picker input. Pass in InputID and error messages words to be displayed in the error message.
function validateDatePicker(inputID, errorMessage) {
    const input = document.getElementById(inputID);
    const errorSpanID = inputID + 'Error';

    // Validate empty input
    const isEmptyValid = isEmpty(input, errorSpanID, `Please enter a valid ${errorMessage}.`);

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

//Use when validating optional text inputs. Pass in the inputID, character limit for the input, and the words you would like to be displayed in the error message in.
function validateOptionalTextInput(inputID, characterLimit, errorMessage) {
    const input = document.getElementById(inputID);
    const inputErrorSpanID = inputID + 'Error';
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
                hasCharacterLimit(input, inputErrorSpanID, characterLimit, `Please enter a ${errorMessage} under ${characterLimit} characters.`);
            }
        }
    }

    // Attach input change event listener
    input.addEventListener('input', handleInputChange);

    // Call handleInputChange initially to perform initial validation
    handleInputChange();
}

//Use when validating a required text input. Pass in the inputID, character limit for the input, and the words you would like to be displayed in the error message in.
function validateRequiredTextInput(inputID, characterLimit, errorMessage) {
    const input = document.getElementById(inputID);
    const inputErrorSpanID = inputID + 'Error';

    function handleInputChange() {
        // Check if input value is empty
        if (input.value.trim() === '') {
            // If input is empty call isempty function
            const isEmptyValid = isEmpty(input, inputErrorSpanID, `Please enter a ${errorMessage}.`);
        } else {
            // If input is not empty, perform validation
            const isAlphabeticValid = isAlphabetic(input, inputErrorSpanID, 'Please enter alphabetic characters only.');
            if (isAlphabeticValid) {
                // Validate character limit
                hasCharacterLimit(input, inputErrorSpanID, characterLimit, `Please enter a ${errorMessage} under ${characterLimit} characters.`);
            }
        }
    }

    // Attach input change event listener
    input.addEventListener('input', handleInputChange);

    // Call handleInputChange initially to perform initial validation
    handleInputChange();
}

//Remove icon
$(document).ready(function () {
    var firstNameError = $('#FirstNameError');
    if ($.trim(firstNameError.text()) === "") {
        firstNameError.hide();
    }
});