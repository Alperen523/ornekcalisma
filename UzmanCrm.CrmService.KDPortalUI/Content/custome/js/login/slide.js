"use strict";


$(document).ready(function () {
    let slideIndex = 0;
    const backgroundImages = [
        '/Content/metronic_v8.0.37/assets/media/misc/coffe_2.jpg',
        '/Content/metronic_v8.0.37/assets/media/misc/coffe-bg.jpg', // İkinci resim yolunu buraya ekleyin
    ];

    //function changeBackground() {
    //    const container = document.getElementById('slideshow-container');
    //    slideIndex = (slideIndex + 1) % backgroundImages.length;
    //    container.style.backgroundImage = `url(${backgroundImages[slideIndex]})`;
    //}

    const container = document.getElementById('slideshow-container');

    // Slaytları oluştur
    backgroundImages.forEach((image, index) => {
        const slide = document.createElement('div');
        slide.classList.add('mySlides');
        if (index === 0) slide.classList.add('active');
        slide.style.backgroundImage = `url(${image})`;
        container.appendChild(slide);
    });

    function changeBackground() {
        const slides = document.getElementsByClassName('mySlides');
        slides[slideIndex].classList.remove('active');
        slideIndex = (slideIndex + 1) % slides.length;
        slides[slideIndex].classList.add('active');
    }



    // Her 3 saniyede bir arka plan resmini değiştir
    setInterval(changeBackground, 4000);

});