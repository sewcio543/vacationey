$(document).ready(function () {
    $(".dropbtn").click(function () {
        var target = $(this).attr('href');
        $('html, body').animate({
            scrollTop: $(target).offset().top - 60
        }, 1000);
    });
});

$(document).ready(function () {
    $(".dropdown-content a").click(function () {
        var target = $(this).attr('href');
        $('html, body').animate({
            scrollTop: $(target).offset().top - 60
        }, 1000);
    });
});


$(document).ready(function () {
    $("#sideMenu a").click(function () {
        var target = $(this).attr('href');
        $('html, body').animate({
            scrollTop: $(target).offset().top - 60
        }, 1000);
    });
});

$(document).ready(function () {
    if ($(window).width() < 650) {
        $("#sideMenu").show();
    } else {
        $("#sideMenu").hide();
    }
});


$(window).resize(function () {
    if ($(window).width() < 650) {
        $("#sideMenu").show();
        $("header").hide();
    } else if (($(window).width() > 650) && ($(window).scrollTop() == 0)) {
        $("#sideMenu").hide();
        $("header").show();
    }

});

$(document).scroll(function () {

    if ($(window).scrollTop() == 0) {
        $("#sideMenu").hide();
        $("header").show();

    } else {

        $("header").hide();
        $("#sideMenu").show();
    }
});