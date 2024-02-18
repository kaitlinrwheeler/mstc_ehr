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


//Start of question mark popout

// Initialize variables to track the visibility and hover state of the requirements popup
var requirementsVisible = false; // Indicates whether the requirements popup is currently visible
var hoveringPopup = false; // Indicates whether the mouse is currently hovering over the requirements popup

// Function to display the requirements popup
function showRequirements() {
    // Set the display style of the requirements popup to block to make it visible
    document.getElementById("requirementsPopup").style.display = "block";
}

// Function to hide the requirements popup
function hideRequirements() {
    // Check if the requirements popup should be hidden
    if (!requirementsVisible && !hoveringPopup) {
        // If neither the requirements popup is visible nor the mouse is hovering over it,
        // set the display style of the requirements popup to none to hide it
        document.getElementById("requirementsPopup").style.display = "none";
    }
}

// Event listener to track when the mouse enters the requirements popup
document.getElementById("requirementsPopup").addEventListener("mouseover", function () {
    // When the mouse enters the requirements popup, set hoveringPopup to true
    hoveringPopup = true;
});

// Event listener to track when the mouse leaves the requirements popup
document.getElementById("requirementsPopup").addEventListener("mouseout", function () {
    // When the mouse leaves the requirements popup, set hoveringPopup to false
    hoveringPopup = false;
});

// Event listener to track clicks anywhere on the document
document.addEventListener("click", function (event) {
    // Retrieve the requirements popup and question icon elements
    var requirementsPopup = document.getElementById("requirementsPopup");
    var questionIcon = document.getElementById("questionIcon");
    var targetElement = event.target; // Retrieve the element that was clicked

    // Check if the requirements popup is visible and the clicked element is not the requirements popup or the question icon
    if (requirementsVisible && targetElement !== requirementsPopup && targetElement !== questionIcon) {
        // If so, hide the requirements popup
        hideRequirements();
        // Set requirementsVisible to false, indicating that the popup is no longer visible
        requirementsVisible = false;
    }
});

//End of quesiton mark popout