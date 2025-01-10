let slideIndex = 0;
const slides = document.querySelector('.slides');
const slideImages = document.querySelectorAll('.slides img');
const dots = document.querySelectorAll('.dot');

// Function to update the slide position
function showSlide(index) {
    // Loop slide index if out of bounds
    if (index >= slideImages.length) slideIndex = 0;
    if (index < 0) slideIndex = slideImages.length - 1;

    // Move the slides container to the correct position
    slides.style.transform = `translateX(-${slideIndex * 100}%)`;

    // Update active dots
    dots.forEach(dot => dot.classList.remove('active'));
    dots[slideIndex].classList.add('active');
}

// Function to move to the next/previous slide
function moveSlide(n) {
    slideIndex += n;
    showSlide(slideIndex);
}

// Function to jump to a specific slide
function currentSlide(n) {
    slideIndex = n;
    showSlide(slideIndex);
}

// Auto-slide every 5 seconds
setInterval(() => moveSlide(1), 5000);

document.addEventListener("DOMContentLoaded", () => {
    const burgerMenu = document.getElementById("burgerMenu");
    const navLinks = document.getElementById("links");

    // Toggle the burger menu and links
    burgerMenu.addEventListener("click", () => {
        burgerMenu.classList.toggle("active");
        navLinks.classList.toggle("active");
    });
});