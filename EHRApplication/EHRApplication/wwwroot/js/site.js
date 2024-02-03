//$("#menu-toggle").click(function (e) {
//    e.preventDefault();
//    $("#wrapper").toggleClass("toggled");
//});
//$("#menu-toggle-2").click(function (e) {
//    e.preventDefault();
//    $("#wrapper").toggleClass("toggled-2");
//    $('#menu ul').hide();
//});

//function initMenu() {
//    $('#menu ul').hide();
//    $('#menu ul').children('.current').parent().show();
//    //$('#menu ul:first').show();
//    $('#menu li a').click(
//        function () {
//            var checkElement = $(this).next();
//            if ((checkElement.is('ul')) && (checkElement.is(':visible'))) {
//                return false;
//            }
//            if ((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
//                $('#menu ul:visible').slideUp('normal');
//                checkElement.slideDown('normal');
//                return false;
//            }
//        }
//    );
//}
//$(document).ready(function () {
//    initMenu();
//});



//$("#menu-toggle").click(function (e) {
//    e.preventDefault();
//    $("#wrapper").toggleClass("toggled");
//});




$("#menu-toggle").on('click', function (e) {
    e.preventDefault();
    $("#wrapper").toggleClass("toggled");
});
$("#menu-toggle-2").on('click', function (e) {
    e.preventDefault();
    $("#wrapper").toggleClass("toggled-2");
    $('#menu ul').hide();
});

function initMenu() {
    $('#menu ul').hide();
    $('#menu ul').children('.current').parent().show();
    //$('#menu ul:first').show();
    $('#menu li a').on('click',
        function () {
            var checkElement = $(this).next();
            if ((checkElement.is('ul')) && (checkElement.is(':visible'))) {
                return false;
            }
            if ((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
                $('#menu ul:visible').slideUp('normal');
                checkElement.slideDown('normal');
                return false;
            }
        }
    );
}
$(function() {
    initMenu();
});





//const themeSwitch = document.getElementById('theme-switch');

//themeSwitch.addEventListener('change', function () {
//    let htmlClasses = document.documentElement.classList;
//    if (this.checked) {
//        htmlClasses.add('dark');
//        localStorage.setItem('theme', 'dark'); //we are using this to remember the user choice
//    } else {
//        htmlClasses.remove('dark');
//        localStorage.setItem('theme', 'light');
//    }
//});


//Loads when page is loaded.
//Gets the theme from local storage and set it to the data-bs-theme. Needed for when the user refreshes the page.
$(function () {
    const savedTheme = localStorage.getItem('theme') || 'light';

    var element = document.body;

    element.dataset.bsTheme = savedTheme;

    //Set the dark mode switch to the proper direction based on the selected theme.
    if (savedTheme === 'dark') {
        $('#flexSwitchCheckChecked').prop('checked', true);
    }
});


//Switches the theme. Tied the the dark mode switch.
function darkSwitch() {
    //Gets the body element so we can check the the bs-theme is and work with it.
    var element = document.body;

    //Determines what the new theme is. It flip flops.
    var newTheme = element.dataset.bsTheme == "light" ? "dark" : "light";

    //Sets the body's bs-theme to the new theme, i.e. light or dark.
    element.dataset.bsTheme = newTheme;

    //Sets the local storage theme to the new theme. Needed for if a user refreshes the page, so the theme doesn't revert back to it's default.
    localStorage.setItem('theme', newTheme);

     
}
