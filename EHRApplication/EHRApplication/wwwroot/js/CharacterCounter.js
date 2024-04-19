// Counts and updates the text area's character counter.
function updateCharacterCount(textAreaId, counterId) {
    const textArea = document.getElementById(textAreaId);
    const counter = document.getElementById(counterId);
    // Must set the max length of the text area for this line to grab.
    const maxLength = textArea.getAttribute('maxlength');
    const currentLength = textArea.value.length;
    counter.textContent = `${currentLength}/${maxLength}`;
}

// Call updateCharacterCount on page load to initialize the character count when the text area 
window.onload = function () {
    updateCharacterCount('description', 'descriptionCounter');
};