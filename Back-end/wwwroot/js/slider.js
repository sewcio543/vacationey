
function switchImages() {

    const numbers = [1, 20, 17];
    var index = Math.floor(Math.random() * 2)

    try {
        document.getElementById("slidePhoto fade").src = "/images/photos/test" + numbers[index] + ".jpg";
    }
    catch { }

};

setInterval(switchImages, 3000);

