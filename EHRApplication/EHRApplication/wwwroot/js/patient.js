// checks for empty input values
// param = input value

function isEmpty(input, errorSpanID) {
    // set initial validity status
    let valid = true;
    const errorSpan = document.getElementById(errorSpanID);
  
    // invalidate input value if empty
    if (input === null || input === undefined || input.value.trim() === '') {
        valid = false;
    }

    // Display error message if input is invalid
    if (!valid) {
        errorSpan.style.display = 'inline';
    } else {
        errorSpan.style.display = 'none';
    }

    return valid;
}

// checks that number input is appropriate type
// param = number value
function isNumber(input) {
    // check input type
    const valid = typeof input !== 'number' && isNaN(input);

    return valid;
}

// checks for valid email address
// param = email input
function isEmail(input) {
    // defines the expected pattern for email address
    const pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    // check email input against previously defined criteria (pattern)
    const valid = pattern.test(input);

    return valid;
}

// checks file name input to ensure appropriate formatting to fit defined standards
// param = file name input
function validateFilename(input) {
    // retrieve file name input
    const filename = input.value.split('\\').pop();

    // retrieve maximum input length from html attribute
    const max = parseInt(input.getAttribute('maxlength'));

    // defines the expected pattern for the input file name
    // this is specifically looking for file types, and will only allow the defined image types (png, jpg, gif)
    const pattern = /^[a-zA-Z0-9]+\.(png|jpg|gif)$/;

    // check input against previously defined criteria (length and pattern)
    const valid = filename.length <= max && pattern.test(filename);

    // change input field border color/thickness to visually indicate to the user their input validity status
    input.style.borderColor = valid ? "green" : "red";
    input.style.borderWidth = "thick";

    return valid;
}

// checks text input to ensure appropriate formatting to fit defined standards
// param = text input
function validateText(input) {
    // retrieve text input
    const text = input.value;

    // defines the expected pattern/length for text input (letters and spaces, 3-50 characters)
    const pattern = /^[a-zA-Z ]{3,50}$/;

    // check input against predefined criteria (length and pattern)
    const valid = pattern.test(text);

    // change input field border color/thickness to visually indicate to the user their input validity status
    input.style.borderColor = valid ? "green" : "red";
    input.style.borderWidth = "thick";

    // display alert message if input doesn't meet criteria
    if (!valid) { alert("Your entry must be 3-50 characters, using only letters and spaces.") }

    return valid;
}

// checks year input to ensure appropriate formatting to fit defined standards
// param = year input
function validateEntryYear(input) {
    // retrieve year input
    const year = input.value;

    // retrieve minimum and maximum input lengths from html attributes
    const min = input.getAttribute('min');
    const max = input.getAttribute('max');

    // defines the expected pattern for year input (4 digits, numbers only)
    const pattern = /^[0-9]{4}$/;

    // check input against previously defined criteria (min/max length and pattern)
    const valid = pattern.test(year) && year >= min && year <= max;

    // change input field border color/thickness to visually indicate to the user their input validity status
    input.style.borderColor = valid ? "green" : "red";
    input.style.borderWidth = "thick";

    // display alert message if input doesn't meet criteria
    if (!valid) { alert("Year must be 4 digits, using numbers only (YYYY). 1891-present only.") }

    return valid;
}

// checks population input to ensure appropriate formatting to fit defined standards
// param = population input
function validatePop(input) {
    // retrieve input and parse value as an integer
    const value = parseInt(input.value);

    // retrieve minimum and maximum input lengths from html attributes
    const min = parseInt(input.getAttribute('min'));
    const max = parseInt(input.getAttribute('max'));

    // check input against previously defined criteria (type integer, min/max length)
    const valid = Number.isInteger(value) && value >= min && value <= max;

    // change input field border color/thickness to visually indicate to the user their input validity status
    input.style.borderColor = valid ? "green" : "red";
    input.style.borderWidth = "thick";

    // display alert message if input doesn't meet criteria
    if (!valid) { alert("Your entry must be between 1 and 99,999 using only numbers.") }

    return valid;
}

// checks score input to ensure appropriate formatting to fit defined standards
// param = basketball game input
function validateScore(input) {
    // retrieve input and parse value as integer
    const value = parseInt(input.value);

    // retrieve minimum and maximum input lengths from html attributes
    const min = parseInt(input.getAttribute('min'));
    const max = parseInt(input.getAttribute('max'));

    // defines the expected pattern for score input (1-3 digits, numbers only)
    const pattern = /^[0-9]{1,3}$/;

    // check input against previously defined criteria (type integer, min/max length, and pattern)
    const valid = Number.isInteger(value) && value >= min && value <= max && pattern.test(input.value);

    // change input field border color/thickness to visually indicate to the user their input validity status
    input.style.borderColor = valid ? "green" : "red";
    input.style.borderWidth = "thick";

    // display alert message if input doesn't meet criteria
    if (!valid) {
        alert("Please enter a score between " + min + " and " + max + ".");
    }

    return valid;
}

// checks time input to ensure appropriate formatting to fit defined standards
// param = time input
function validateTime(input) {
    // retrieve input and parse value as integer
    const value = parseInt(input.value);

    // retrieve minimum and maximum input lengths from html attributes
    const min = parseInt(input.getAttribute('min'));
    const max = parseInt(input.getAttribute('max'));

    // check input against previously defined criteria (type integer, min/max length)
    const valid = Number.isInteger(value) && value >= min && value <= max;

    // change input field border color/thickness to visually indicate to the user their input validity status
    input.style.borderColor = valid ? "green" : "red";
    input.style.borderWidth = "thick";

    // display alert message if input doesn't meet criteria
    if (!valid) {
        alert("Please enter a game time between 30 and 200 minutes, using numbers only.");
    }

    return valid;
}

// checks date input to ensure appropriate formatting to fit defined standards
// param = date input
function validateDate(input) {
    // retrieve input and parse value as a date
    const enteredDate = new Date(input.value);

    // set date entry limits
    const minDate = new Date("10/31/1891"); // earliest acceptable date
    const maxDate = new Date(); // current date may not be exceeded
    maxDate.setHours(0, 0, 0, 0);

    // defines the expected pattern for date input (MM/DD/YYYY)
    const pattern = /^(0[1-9]|1[012])\/(0[1-9]|[12][0-9]|3[01])\/\d{4}$/;

    // check input against previously defined criteria (date range)
    const valid = enteredDate >= minDate && enteredDate <= maxDate;

    // check input against previously defined criteria (pattern)
    // returns invalid result if the input doesn't match the pattern
    if (pattern.test(enteredDate)) {
        valid = false;
    }

    // change input field border color/thickness to visually indicate to the user their input validity status
    input.style.borderColor = valid ? "green" : "red";
    input.style.borderWidth = "thick";

    // display alert message if input doesn't meet criteria
    if (!valid) {
        alert("Please select a valid date that falls between 11/01/1891 and today's date.");
    }

    return valid;
}

// compare team names to ensure they're not the same
// param = team name input
function validateTeamName() {
    // retrieve home and away team names from dropdown menu
    var hName = document.getElementById("home-team-name");
    var aName = document.getElementById("away-team-name");

    // set initial validity status
    let valid = true;

    // display alert message if the same team is selected for both home and away values
    if (hName.value === aName.value) {
        alert("Home and Away team names must be different. Please check names again, and make any necessary changes before resubmitting.");
        valid = false;
    }

    return valid;
}

// Reset Forms

// resets form input field
// param = form id
function resetForm(formId) {
    // select all input fields from specified form
    let inputFields = document.querySelectorAll(formId + " input");

    // resets each listed field type (text, number, checkbox, file)
    inputFields.forEach(function (field) {
        if (field.type === "text") {
            field.value = "";
        } else if (field.type === "number") {
            field.value = 0;
        } else if (field.type === "checkbox") {
            field.checked = false;
        } else if (field.type === "file") {
            field.value = "";
        }
    })
}

// Other common functions

// retrieve element using id value
function getById(id) {
    return document.getElementById(id);
}

// attach event handler to an element
// params = element, event, handler
function addEvent(element, event, handler) {
    element.addEventListener(event, handler);
}

// remove event handler from an element
// params = element, event, handler
function removeEvent(element, event, handler) {
    element.removeEventListener(event, handler);
}