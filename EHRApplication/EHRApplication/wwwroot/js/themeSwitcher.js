$(function () {
    const savedTheme = localStorage.getItem('theme') || 'light';

    var element = document.body;

    element.dataset.bsTheme = savedTheme;
});


//Switches the theme. Tied the the dark mode switch.
function darkSwitch() {
    var element = document.body;
    element.dataset.bsTheme = element.dataset.bsTheme == "light" ? "dark" : "light";
}
