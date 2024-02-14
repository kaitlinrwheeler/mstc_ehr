var message;
$(document).ready(function () {

    $('form').on('submit', function (event) {

        let isValid = true;

        $('input, textarea').each(function () {
            if (!validateField($(this))) {
                isValid = false;
            }
        });
        if (!isValid) {
            event.preventDefault(); // Prevent form submission if there are errors
        }
    });

    function validateField(field) {
        let value = field.val().trim();
        let isValid = true;
        switch (field.attr('name')) {
            case 'email':
                if (TestNull(value) == false) {
                    isValid = false;
                    message = "Field must not be left blank";
                } else if (!validateEmailField(value)) {
                    isValid = false;
                    message = "Field must not contain numbers and must not be greater than 20 characters";
                }
                toggleErrorMessage(field, !isValid, message);
                break;
            case 'password':
                if (TestNull(value) == false) {
                    isValid = false;
                    message = "Field must not be left blank";
                } else if (!LettersOnly(value) || value.length > 20) {
                    isValid = false;
                    message = "Field must not contain numbers and must not be greater than 20 characters";
                }
                toggleErrorMessage(field, !isValid, message);
                break;
        }
        if (isValid) {
            field.removeClass('is-invalid');
        } else {
            field.addClass('is-invalid');
        }
        return isValid;
    }
    function toggleErrorMessage(field, show, message) {
        const errorDiv = field.next('.invalid-feedback');
        if (show) {
            errorDiv.text(message);
        } else {
            errorDiv.text('');
        }
    }
    function validateEmailField(emailValue) {
        if (emailValue.endsWith("@mstc.edu" = true){
            isvalid = true;
        } else {
            isvalid = false;
        }
    }

    function validatePasswordField(passwordValue) {
        var errorMessage = "";
        if (!passwordValue || passwordValue.trim() === "") {
            errorMessage = "Password is required.";
        } else if (/\s/.test(passwordValue)) {
            errorMessage = "Password must not contain spaces.";
        } else if (!/(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/.test(passwordValue)) {
            errorMessage = "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number, and one special character.";
        }
        document.getElementById('password-error').innerText = errorMessage;
    }

});