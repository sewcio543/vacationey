
function switchImages() {

    var number = Math.floor(Math.random() * 11)
    number = number == 0 ? 1 : number;
    try {
        document.getElementById("slidePhoto fade").src = "/images/photos/test" + number + ".jpg";
    }
    catch { }

};

setInterval(switchImages, 3000);

