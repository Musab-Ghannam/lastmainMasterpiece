
const hamburger = document.querySelector('.hamburger');
const navLink = document.querySelector('.nav__link');

hamburger.addEventListener('click', () => {
    navLink.classList.toggle('hide');
});






const right = document.getElementById('rightt');
const left = document.getElementById('leftt');
const logo = document.getElementById('logo')

const KNOWLEDGE = document.getElementById('KNOWLEDGE');

const slide = document.getElementById('slide')
let x = 1;
let interval = 2000
setInterval(myTimer, interval);

let y = 0;



let array = ["class1", "class2", "class3", "class4"]
function myTimer() {


    slide.classList.remove(array[x - 1])
    if (x > 3) {
        x = 0;
    }
    slide.classList.add(array[x])







    x++;

}



const pic = function () {


    slide.classList.remove(array[x - 1])
    if (x > 3) {
        x = 0;
    }
    slide.classList.add(array[x])






    x++;
    console.log(y)



}


const picbefore = function () {
    if (x < 0) {
        x = 3
        slide.classList.remove(array[0])
    } else {
        slide.classList.remove(array[x + 1])
    }
    slide.classList.add(array[x])

    x--;




}

right.addEventListener('click', pic);
left.addEventListener('click', picbefore)



setInterval(tim, 2000);





function tim() {









    y++;
    if (y > 2) {
        y = 0;
    }
    if (y == 0) {
        KNOWLEDGE.innerHTML =



            "We connect you with licensed therapists, we provide online consultation for  those of you who want  to consult but want to   stay at home    "










    }

    if (y == 1) {
        KNOWLEDGE.innerHTML = "Introducing our new online psychology service,Our platform is easy to use and secure, ensuring that all  personal information are kept confidential. ";

    }

    if (y == 2) {
        KNOWLEDGE.innerHTML = "Our team of psychologists is dedicated to helping you, Whether you're struggling with anxiety, depression, or dealing with a difficult life transition.";

    }



}


console.log("123");

//tru

//try
console.log("ok");

$(document).ready(function () {
    var silder = $(".owl-carousel");
    silder.owlCarousel({
        autoplay: true,
        autoplayTimeout: 3000,
        autoplayHoverPause: false,
        items: 1,
        stagePadding: 20,
        center: true,
        nav: false,
        margin: 50,
        dots: true,
        loop: true,
        responsive: {
            0: { items: 1 },
            480: { items: 2 },
            575: { items: 2 },
            768: { items: 2 },
            991: { items: 3 },
            1200: { items: 4 }
        }
    });
});

