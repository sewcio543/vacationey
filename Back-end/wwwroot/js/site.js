
// redirecting with filters
function submitForm(page) {
    var select = document.getElementById("selectCountry");
    var value = select.options[select.selectedIndex].value;
    window.location.replace(page + value);
};