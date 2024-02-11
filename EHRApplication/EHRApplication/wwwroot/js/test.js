// Factory function to generate validation functions for first name input
function validateFirstName() {
    const firstNameInput = document.getElementById('FirstName');

    // Validate empty input
    const isEmptyValid = isEmpty(firstNameInput, 'FirstNameError', 'Please enter a first name.');

    // If input is not empty, proceed with other validations
    if (isEmptyValid) {
        // Validate alphabetic characters only
        const isAlphabeticValid = isAlphabetic(firstNameInput, 'FirstNameError', 'Please enter alphabetic characters only.');

        if (isAlphabeticValid) {
            const hasCharacterLimitValid = hasCharacterLimit(firstNameInput, 'FirstNameError', 25, 'Please enter a first name under 25 characters.');
        }
    }
}



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