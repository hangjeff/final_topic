// vertical.js

$(document).ready(function () {
    // Toggle sidebar menu
    $('.navbar-toggle').click(function () {
        $('.side-menu').toggleClass('active');
    });

    // Close sidebar menu when clicking outside the menu
    $(document).click(function (e) {
        if (!$(e.target).closest('.side-menu').length &&
            !$(e.target).closest('.navbar-toggle').length) {
            $('.side-menu').removeClass('active');
        }
    });

    // Toggle dropdown menus
    $('.panel-heading').click(function () {
        $(this).toggleClass('active').next('.panel-collapse').slideToggle();
    });

    // Prevent default behavior of dropdown toggle
    $('.panel-heading a').click(function (e) {
        e.stopPropagation();
    });

    // Close dropdowns when clicking outside them
    $(document).click(function (e) {
        if (!$(e.target).closest('.panel-collapse').length &&
            !$(e.target).closest('.panel-heading').length) {
            $('.panel-collapse').slideUp();
            $('.panel-heading').removeClass('active');
        }
    });

    // Close search form when clicking outside it
    $(document).click(function (e) {
        if (!$(e.target).closest('#search').length &&
            !$(e.target).closest('#search-trigger').length) {
            $('#search').collapse('hide');
        }
    });
});


