﻿// Validation functions

//variable for the 'X' symbol for error messages.
//Hopefully someone will get back to this and make the validation show on the javascript and the model too.
//const xIcon = '<i class="fa-solid fa-circle-xmark"></i> ';

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
    const alphabetRegex = /^[a-zA-Z\s'\/\-]+$/;
    let valid = alphabetRegex.test(input.value.trim());

    // Display error message if input contains non-alphabetic characters
    displayError(valid, errorSpan, errorMessage);

    return valid;
}

// Function to check if the input value conatins only numeric characters
function isANumber(input, errorSpanID, errorMessage) {
    const errorSpan = document.getElementById(errorSpanID);
    const numericRegex = /^[0-9]+$/;
    let valid = numericRegex.test(input.value.trim());

    // Display error message if input contains non-numeric characters
    displayError(valid, errorSpan, errorMessage);

    return valid;
}

// Function to check if the input value is a valid email address
function isValidEmail(input, errorSpanID, errorMessage) {
    const errorSpan = document.getElementById(errorSpanID);
    const emailRegex = /^[^@\s]+@([a-zA-Z0-9]+\.)+(com|net|edu|gov|org)$/;;
    let valid = emailRegex.test(input.value.trim());

    // Display error message if input is not a valid email address
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
        //Commenting this hopefully we come back and finish this icon
        //errorSpan.innerHTML = xIcon + errorMessage;
        errorSpan.innerHTML = errorMessage;
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

//Use when validating a required Number input. Pass in the inputID, character limit for the input, and the words you would like to be displayed in the error message in.
function validateRequiredNumberInput(inputID, characterLimit, errorMessage) {
    const input = document.getElementById(inputID);
    const inputErrorSpanID = inputID + 'Error';
    const trimedInput = input.value.trim();
    const errorSpan = document.getElementById(inputErrorSpanID);

    function handleInputChange() {
        // Check if input value is empty
        if (trimedInput === '') {
            // If input is empty call isempty function
            const isEmptyValid = isEmpty(input, inputErrorSpanID, `Please enter a ${errorMessage}.`);
        } else {
            // If input is not empty, perform validation
            const isNumericValid = isANumber(input, inputErrorSpanID, 'Please enter numeric characters only.');
            if (isNumericValid) {
                // Validate character limit
                hasCharacterLimit(input, inputErrorSpanID, characterLimit, `Please enter a ${errorMessage} under ${characterLimit} characters.`);
                switch (inputID) {
                    case "Phone":
                        if (trimedInput.length !== 10) {
                            displayError(false, errorSpan, `Please enter exactly 10 characters for ${errorMessage}.`);
                            return
                        } 
                        break;
                    case "Zipcode":
                        if (trimedInput.length !== 5) {
                            displayError(false, errorSpan, `Please enter exactly 5 characters for ${errorMessage}.`);
                        } 
                        break;
                }
            }
        }
    }

    // Attach input change event listener
    input.addEventListener('input', handleInputChange);

    // Call handleInputChange initially to perform initial validation
    handleInputChange();
} 

//Use when validating an Optional number input. Pass in the inputID, character limit for the input, and the words you would like to be displayed in the error message in.
function validateOptionalNumberInput(inputID, characterLimit, errorMessage) {
    const input = document.getElementById(inputID);
    const inputErrorSpanID = inputID + 'Error';
    const trimedInput = input.value.trim();
    const errorSpan = document.getElementById(inputErrorSpanID);

    function handleInputChange() {
        // Check if input value is empty
        if (trimedInput === '') {
            // If input is empty, hide error message
            errorSpan.style.display = 'none';
        } else {
            // If input is not empty, perform validation
            const isNumericValid = isANumber(input, inputErrorSpanID, 'Please enter numeric characters only.');
            if (isNumericValid) {
                // Validate character limit
                hasCharacterLimit(input, inputErrorSpanID, characterLimit, `Please enter a ${errorMessage} under ${characterLimit} characters.`);
                switch (inputID) {
                    case "Phone",
                        "ECPhone":
                        if (trimedInput.length !== 10) {
                            displayError(false, errorSpan, `Please enter exactly 10 characters for ${errorMessage}.`);
                            return
                        }
                        break;
                    case "Zipcode":
                        if (trimedInput.length !== 5) {
                            displayError(false, errorSpan, `Please enter exactly 5 characters for ${errorMessage}.`);
                        }
                        break;
                }
            }
        }
    }

    // Attach input change event listener
    input.addEventListener('input', handleInputChange);

    // Call handleInputChange initially to perform initial validation
    handleInputChange();
} 

// Call to validate an email input. Pass in InputID and error messages words to be displayed in the error message.
function validateOptionalEmailInput(inputID, errorMessage) {
    const input = document.getElementById(inputID);
    const inputErrorSpanID = inputID + 'Error';
    const trimedInput = input.value.trim();
    const errorSpan = document.getElementById(inputErrorSpanID);

    function handleInputChange() {
        // Check if input value is empty
        if (trimedInput === '') {
            // If input is empty, hide error message
            errorSpan.style.display = 'none';
        } else {
            // If input is not empty, perform validation                
            const isEmailValid = isValidEmail(input, inputErrorSpanID, 'Please enter a valid email address.');
        }
    }

    // Attach input change event listener
    input.addEventListener('input', handleInputChange);

    // Call handleInputChange initially to perform initial validation
    handleInputChange();
}


function validateRequiredInput(inputID, errorMessage) {
    const input = document.getElementById(inputID);
    const inputErrorSpanID = inputID + 'Error';

    function handleInputChange() {
        // Check if input value is empty
        if (input.value.trim() === '') {
            // If input is empty call isempty function
            const isEmptyValid = isEmpty(input, inputErrorSpanID, `Please enter a ${errorMessage}.`);
        }
    }

    // Attach input change event listener
    input.addEventListener('input', handleInputChange);

    // Call handleInputChange initially to perform initial validation
    handleInputChange();
}

//handles when OTHER option is clicked and adds a text box that the user can type in.
function handleDropdownWithTextField(dropdownId, textFieldId, errorSpanId, inputType) {
    var selectedValue = document.getElementById(dropdownId).value;
    var textField = document.getElementById(textFieldId);
    var errorSpan = document.getElementById(errorSpanId);

    if (selectedValue === "Other") {
        textField.style.display = "block";
        textField.required = true;
        errorSpan.style.display = "block";

        // Set placeholder based on input type
        if (inputType === 'gender') {
            textField.placeholder = "Please specify a gender.";
        } else if (inputType === 'pronoun') {
            textField.placeholder = "Please specify other pronouns.";
        }

        // Call validateRequiredTextInput for the optional text box (validation)
        validateRequiredTextInput(textFieldId, 25, inputType);
    } else {
        textField.style.display = "none";
        textField.required = false;
        errorSpan.style.display = "none";
    }
}

//handles when the OTHER is selected in the race list. Had to do this one seperately because the js couldn't grab it successfully using the previous function.
function handleOtherRaceSelection() {
    var otherRaceCheckbox = document.getElementById("OtherRace");
    var otherRaceInput = document.getElementById("OtherRaceInput");

    if (otherRaceCheckbox.checked) {
        otherRaceInput.style.display = "block";
        otherRaceInput.required = true;

    } else {
        otherRaceInput.style.display = "none";
        otherRaceInput.required = false;
    }
}

//validates image size
function validateImageSize(input) {
    const maxSize = 1000 * 1024; // 1000 KB in bytes
    const minSize = 500 * 1024;  // 500 KB in bytes

    if (input.files && input.files[0]) {
        const fileSize = input.files[0].size;

        if (fileSize < minSize || fileSize > maxSize) {
            document.getElementById("patientImage").innerText = "Image size should be between 500KB and 1000KB.";
            input.value = ''; // Clear the file input to prevent submission
        } else {
            document.getElementById("patientImage").innerText = ""; // Clear any previous error message
        }
    }
}
