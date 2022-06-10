
function switchImages() {

    var number = Math.floor(Math.random() * 11)
    number = number == 0 ? 1 : number;
    document.getElementById("slidePhoto fade").src = "/images/photos/test" + number + ".jpg";

};

setInterval(switchImages, 3000);