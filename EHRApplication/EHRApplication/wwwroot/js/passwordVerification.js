function CheckNumber() {
    var userNumber = document.getElementById("verifyInput").value;

    fetch(`/Login_Register/CheckNumber?userNumber=${userNumber}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json' // Add Content-Type header if sending JSON data
        }
    })
        .then(response => {
            if (response.ok) {
                // Show or hide fields
                $("#passwordFields").show();
                $("#verifySubmit").hide();
                $("#verifyInput").hide();
            } else {
                $("#passwordFields").hide();
                $("#verifySubmit").show();
                $("#verifyInput").show();
            }
        })
        .catch(error => {
            console.error('Error:', error);
            // Handle error
        });
}
