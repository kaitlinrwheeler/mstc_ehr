//Everything in this function loads when the page is loaded. Loads when page is loaded.
$(function () {

    initMenu();
    getPreviouslySetTheme();
});

// Handle click event on the first menu toggle button
$("#menu-toggle").on('click', function (e) {
    // Prevent the default behavior of the link
    e.preventDefault();

    // Toggle the 'toggled' class on the #wrapper element
    $("#wrapper").toggleClass("toggled");
});


// Function to initialize the menu behavior
function initMenu() {
    // Hide all submenus initially
    $('#menu ul').hide();

    // Show the submenu of the currently selected item
    $('#menu ul').children('.current').parent().show();

    // Uncomment the line below if you want the first submenu to show initially.
    // $('#menu ul:first').show();

    // Handle click events on menu items
    $('#menu li a').on('click',
        function () {
            // Get the next element (potential submenu) of the clicked item
            var checkElement = $(this).next();

            // If the next element is a visible submenu, do nothing
            if ((checkElement.is('ul')) && (checkElement.is(':visible'))) {
                return false;
            }

            // If the next element is a hidden submenu, close all visible submenus
            // and open the submenu of the clicked item
            if ((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
                // Slide up all visible submenus
                $('#menu ul:visible').slideUp('normal');
                // Slide down the submenu of the clicked item
                checkElement.slideDown('normal');
                return false;
            }
        }
    );
}


// Gets the theme from local storage and set it to the data-bs-theme. Needed for when the user refreshes the page.
//function getPreviouslySetTheme() {

//    // Gets the theme previously seleted theme from local storage.
//    const savedTheme = localStorage.getItem('theme') || 'light';

//    // Gets the body element. This might need to become a list of elements once we add pages, not entirely sure how dark mode will be working with all the content planned.
//    var element = document.body;

//    // Sets the element's theme to the previously saved theme.
//    element.dataset.bsTheme = savedTheme;

//    // Set the dark mode switch to the proper direction based on the selected theme.
//    if (savedTheme === 'dark') {
//        $('#flexSwitchCheckChecked').prop('checked', true);
//    }
//}



// Gets the theme from local storage and set it to the data-bs-theme. Needed for when the user refreshes the page.
function getPreviouslySetTheme() {

    // Gets the theme previously selected theme from local storage.
    const savedTheme = localStorage.getItem('theme') || 'light';

    // Get all elements with the data-bs-theme attribute
    var elements = document.querySelectorAll('[data-bs-theme]');

    // Iterate over each element
    elements.forEach(function (element) {
        // Set the element's theme to the previously saved theme. 
        element.dataset.bsTheme = savedTheme;
    });

    // Set the dark mode switch to the proper direction based on the selected theme.
    if (savedTheme === 'dark') {
        $('#flexSwitchCheckChecked').prop('checked', true);
    }
}



// Switches the theme. Tied the the dark mode switch.
//function darkSwitch() {
//    // Gets the body element so we can check the the bs-theme is and work with it.
//    var element = document.body;

//    // Determines what the new theme is. It flip flops.
//    var newTheme = element.dataset.bsTheme == "light" ? "dark" : "light";

//    // Sets the body's bs-theme to the new theme, i.e. light or dark.
//    element.dataset.bsTheme = newTheme;

//    // Sets the local storage theme to the new theme. Needed for if a user refreshes the page, so the theme doesn't revert back to it's default.
//    localStorage.setItem('theme', newTheme);
//}


// Switches the theme. Tied to the dark mode switch.
function darkSwitch() {
    // Get all elements with the data-bs-theme attribute
    var elements = document.querySelectorAll('[data-bs-theme]');

    // Iterate over each element
    elements.forEach(function (element) {
        // Determine what the new theme is. It flip flops.
        var newTheme = element.dataset.bsTheme === "light" ? "dark" : "light";

        // Set the element's data-bs-theme to the new theme, i.e. light or dark.
        element.dataset.bsTheme = newTheme;
    });

    // Get the current theme from the last processed element
    var newTheme = elements[elements.length - 1].dataset.bsTheme;

    // Set the local storage theme to the new theme. Needed for if a user refreshes the page, so the theme doesn't revert back to its default.
    localStorage.setItem('theme', newTheme);
}
