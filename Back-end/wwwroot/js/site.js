
// redirecting with filters
function submitFormOffer(page) {
    var selectCountry = document.getElementById("selectCountry");
    var country = selectCountry.options[selectCountry.selectedIndex].value;

    var selectPrice = document.getElementById("priceOrder");
    var order = selectPrice.options[selectPrice.selectedIndex].value;

    var selectCityFrom = document.getElementById("selectFrom");
    var cityFrom = selectCityFrom.options[selectCityFrom.selectedIndex].value;

    var selectCityTo = document.getElementById("selectTo");
    var cityTo = selectCityTo.options[selectCityTo.selectedIndex].value;

    var url = "?"

    if (country != "All") { url += 'countrySearch=' + country + '&' };
    if (order != "") { url += 'sortOrder=' + order + '&' };
    if (cityFrom != "All") { url += 'cityFrom=' + cityFrom + '&' };
    if (cityTo != "All") { url += 'cityTo=' + cityTo + '&' };

    var actual_page = parseInt(window.location.href.slice(-1));

    if (page == 0) { url += 'page=1' }
    else if (page == 1) { url += 'page=' + (actual_page + 1) }
    else if (page == -1) { url += 'page=' + (actual_page - 1) }

    window.location.replace(url);
    //delay(5000).then(() => console.log('ran after 1 second1 passed'));
    //document.getElementById("query-filters").scrollIntoView();

};

// redirecting with filters
function submitFormHotel(page) {

    var selectRate = document.getElementById("rateOrder");
    var order = selectRate.options[selectRate.selectedIndex].value;

    var selectCity = document.getElementById("selectCity");
    var city = selectCity.options[selectCity.selectedIndex].value;

    var url = "?"

    if (order != "") { url += 'sortOrder=' + order + '&' };
    if (city != "All") { url += 'citySearch=' + city + '&' };

    var actual_page = parseInt(window.location.href.slice(-1));

    if (page == 0) { url += 'page=1' }
    else if (page == 1) { url += 'page=' + (actual_page + 1) }
    else if (page == -1) { url += 'page=' + (actual_page - 1) }

    window.location.replace(url);
    //delay(5000).then(() => console.log('ran after 1 second1 passed'));
    //document.getElementById("query-filters").scrollIntoView();

};

