// Click event for the question mark icon
$('.icon').click(function (e) {
    $('.icon.active').removeClass('active');
    $(this).addClass('active');
    e.preventDefault();
    e.stopPropagation();
});

// Click event for the document to remove active class
$(document).click(() => {
    $('.icon.active').removeClass('active');
});

// Joyride functionality
let joyridePos = 0;
const joyride = ((config) => {
    const tester = () => {
        $('body').attr('joyride-step', joyridePos);
        const step = config[joyridePos];
        const el = $(step[0]);
        const check = () => {
            if (step[2].bind(el[0])(el[0])) {
                el[0][`on${step[1]}`] = undefined;
                joyridePos++;
                tester();
            }
        };
        el[0][`on${step[1]}`] = check;
    };
    tester();
})([
    ['.text textarea', 'click', el => true],
    ['.text textarea', 'keydown', el => el.value.length > 10],
    ['.icon .fa-facebook', 'click', el => true],
    ['.yes-btns', 'click', el => true],
    ['.promote-btns', 'click', el => true],
    // Add more steps as needed
]);