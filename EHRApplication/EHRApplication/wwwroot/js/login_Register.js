var requirementsVisible = false;
var hoveringPopup = false;

function showRequirements() {
    document.getElementById("requirementsPopup").style.display = "block";
}

function hideRequirements() {
    if (!requirementsVisible && !hoveringPopup) {
        document.getElementById("requirementsPopup").style.display = "none";
    }
}

document.getElementById("requirementsPopup").addEventListener("mouseover", function () {
    hoveringPopup = true;
});

document.getElementById("requirementsPopup").addEventListener("mouseout", function () {
    hoveringPopup = false;
});

document.addEventListener("click", function (event) {
    var requirementsPopup = document.getElementById("requirementsPopup");
    var questionIcon = document.getElementById("questionIcon");
    var targetElement = event.target;

    if (requirementsVisible && targetElement !== requirementsPopup && targetElement !== questionIcon) {
        hideRequirements();
        requirementsVisible = false;
    }
});
