$(function () {
    $("#slider4").responsiveSlides({
        auto: true,
        pager: false,
        nav: true,
        pause: true,
        pauseControls: true,
        speed: 500,
        namespace: "centered-btns",
        prevText: "",
        nextText: ""
        /*after: function () {
            loadEventsSlider();
        }*/
    });
    loadEventsSlider();
});

function loadEventsSlider() {
    $("#mobileEventsSlider").slick({
        dots: true,
        infinite: true,
        mobileFirst: true,
        speed: 300,
        slidesToShow: 1,
        centerMode: false,
        variableWidth: true,
        arrows: true
    });
};