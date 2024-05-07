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
                $("#passHeader").show();
                $("#verifySubmit").hide();
                $("#verifyInput").hide();
                $("#codeHeader").hide();
                $("#invalidCodeMsg").hide();
            } else {
                $("#passwordFields").hide();
                $("#passHeader").hide();
                $("#verifySubmit").show();
                $("#verifyInput").show();
                $("#codeHeader").show();
                $("#invalidCodeMsg").show();
            }
        })
        .catch(error => {
            console.error('Error:', error);
            // Handle error
        });
}
