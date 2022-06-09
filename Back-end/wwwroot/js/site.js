
// redirecting with filters
function submitForm() {
    var selectCountry = document.getElementById("selectCountry");
    var country = selectCountry.options[selectCountry.selectedIndex].value;

    var selectPrice = document.getElementById("priceOrder");
    var order = selectPrice.options[selectPrice.selectedIndex].value;

    var selectCityFrom = document.getElementById("selectFrom");
    var cityFrom = selectCityFrom.options[selectCityFrom.selectedIndex].value;

    var selectCityTo = document.getElementById("selectTo");
    var cityTo = selectCityTo.options[selectCityTo.selectedIndex].value;

    var url = "?"

    if (country != "All") { url = url + 'countrySearch=' + country + '&' };
    if (order != "") { url = url + 'sortOrder=' + order + '&' };
    if (cityFrom != "All") { url = url + 'cityFrom=' + cityFrom + '&' };
    if (cityTo != "All") { url = url + 'cityTo=' + cityTo };


    window.location.replace(url);
    //delay(5000).then(() => console.log('ran after 1 second1 passed'));
    //document.getElementById("query-filters").scrollIntoView();
    
};

