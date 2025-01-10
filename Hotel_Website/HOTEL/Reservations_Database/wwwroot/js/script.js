document.addEventListener("DOMContentLoaded", function () {
    const slides = document.querySelectorAll('.slideshow img');
    let currentSlide = 0;

    function showSlide() {
        // Remove 'active' class from all slides
        slides.forEach((slide) => slide.classList.remove('active'));

        // Increment slide index and loop back to 0 if at the last slide
        currentSlide = (currentSlide + 1) % slides.length;

        // Add 'active' class to the current slide
        slides[currentSlide].classList.add('active');
    }

    // Initialize slideshow
    slides[currentSlide].classList.add('active'); // Ensure the first slide is visible

    // Set interval for automatic slide change
    setInterval(showSlide, 4000); // Change slide every 3 seconds
});


document.addEventListener("DOMContentLoaded", function () {
    const adultsSelect = document.getElementById("adults");
    const childrenSelect = document.getElementById("children");

    // Function to adjust children options based on adults selection
    function adjustChildrenOptions() {
        const adults = parseInt(adultsSelect.value);
        const maxPeople = 5;
        const maxChildren = maxPeople - adults;

        // Remove existing options
        childrenSelect.innerHTML = "";

        // Populate children options dynamically
        for (let i = 0; i <= maxChildren; i++) {
            const option = document.createElement("option");
            option.value = i;
            option.textContent = i;
            childrenSelect.appendChild(option);
        }
    }

    // Event listener on adults selection
    adultsSelect.addEventListener("change", adjustChildrenOptions);

    // Initialize children options on page load
    adjustChildrenOptions();
});

document.addEventListener("DOMContentLoaded", function () {
    const arrivingInput = document.getElementById("arriving");
    const leavingInput = document.getElementById("leaving");

    // Set default value to today's date
    const today = new Date();
    const formattedToday = today.toISOString().split('T')[0];
    arrivingInput.value = formattedToday;
    arrivingInput.min = formattedToday;

    // Set leaving date to default + 1 day
    const tomorrow = new Date(today);
    tomorrow.setDate(today.getDate() + 1);
    leavingInput.value = tomorrow.toISOString().split('T')[0];
    leavingInput.min = tomorrow.toISOString().split('T')[0];

    // Update leaving date 'min' based on arriving date
    arrivingInput.addEventListener("change", function () {
        const arrivingDate = new Date(arrivingInput.value);
        const minLeavingDate = new Date(arrivingDate);
        minLeavingDate.setDate(arrivingDate.getDate() + 1);

        leavingInput.min = minLeavingDate.toISOString().split('T')[0];
        leavingInput.value = minLeavingDate.toISOString().split('T')[0];
    });
});

// Validate the form to ensure leaving date > arriving date
function validateDates() {
    const arrivingInput = document.getElementById("arriving");
    const leavingInput = document.getElementById("leaving");

    if (new Date(leavingInput.value) <= new Date(arrivingInput.value)) {
        alert("Напускането трябва да бъде след датата на пристигане!");
        return false; // Prevent form submission
    }
    return true; // Allow form submission
}



const carousel = document.querySelector('.carousel');
const dots = document.querySelectorAll('.dot');
let isDragging = false;
let startPosition = 0;
let currentTranslate = 0;
let prevTranslate = 0;
let animationID = 0;
let currentIndex = 0;

// Disable default drag behavior
carousel.addEventListener('dragstart', (e) => e.preventDefault());

// Touch or Mouse Events
carousel.addEventListener('mousedown', dragStart);
carousel.addEventListener('mouseup', dragEnd);
carousel.addEventListener('mouseleave', dragEnd);
carousel.addEventListener('mousemove', drag);

carousel.addEventListener('touchstart', dragStart);
carousel.addEventListener('touchend', dragEnd);
carousel.addEventListener('touchmove', drag);

// Add click event for dots
dots.forEach((dot, index) => {
    dot.addEventListener('click', () => {
        currentIndex = index; // Set current index
        setPositionByIndex();
        updateActiveDot();
    });
});

function dragStart(event) {
    isDragging = true;
    startPosition = getPositionX(event);
    animationID = requestAnimationFrame(animation);
}

function drag(event) {
    if (!isDragging) return;
    const currentPosition = getPositionX(event);
    currentTranslate = prevTranslate + currentPosition - startPosition;
}

function dragEnd() {
    cancelAnimationFrame(animationID);
    isDragging = false;

    const movedBy = currentTranslate - prevTranslate;

    // Change slide if moved enough
    if (movedBy < -100 && currentIndex < dots.length - 1) currentIndex += 1;
    if (movedBy > 100 && currentIndex > 0) currentIndex -= 1;

    setPositionByIndex();
    updateActiveDot();
}

function getPositionX(event) {
    return event.type.includes('mouse') ? event.pageX : event.touches[0].clientX;
}

function animation() {
    setCarouselPosition();
    if (isDragging) requestAnimationFrame(animation);
}

function setCarouselPosition() {
    carousel.style.transform = `translateX(${currentTranslate}px)`;
}

function setPositionByIndex() {
    const itemWidth = carousel.offsetWidth / 2; // Adjust for 3 items visible
    currentTranslate = currentIndex * -itemWidth;
    prevTranslate = currentTranslate;
    setCarouselPosition();
}

function updateActiveDot() {
    dots.forEach((dot, index) => {
        dot.classList.toggle('active', index === currentIndex);
    });
}

// Select all elements to animate on scroll
const venuesSection = document.querySelector('.venues');
const venueCards = document.querySelectorAll('.venue');
const venuesTitle = document.querySelector('.venues-title');

// Helper function to check if an element is in the viewport
function isInViewport(element) {
    const rect = element.getBoundingClientRect();
    return (
        rect.top >= 0 &&
        rect.left >= 0 &&
        rect.bottom <= (window.innerHeight || document.documentElement.clientHeight) &&
        rect.right <= (window.innerWidth || document.documentElement.clientWidth)
    );
}

// Function to add animation classes when elements are in the viewport
function animateOnScroll() {
    if (isInViewport(venuesTitle)) {
        venuesTitle.style.opacity = 1;
        venuesTitle.style.transform = 'translateY(0)';
    }

    venueCards.forEach((card, index) => {
        if (isInViewport(card)) {
            card.style.opacity = 1;
            card.style.transform = 'translateY(0)';
        }
    });
}

// Attach the scroll event listener
window.addEventListener('scroll', animateOnScroll);

// Trigger animation for elements already in view on page load
document.addEventListener('DOMContentLoaded', animateOnScroll);


let slideIndex = 0;
const slides = document.querySelectorAll('.slider-spa-image img');
const dots_spa = document.querySelectorAll('dots-spa.dot');

// Function to show the current slide
function showSlide(index) {
    // Loop slide index if out of bounds
    if (index >= slider - spa - image.length) slideIndex = 0;
    if (index < 0) slideIndex = slider - spa - image.length - 1;

    // Hide all slides and remove active dots
    slider - spa.forEach(slide => slide.classList.remove('active'));
    dots.forEach(dot => dot.classList.remove('active'));

    // Show the current slide and add active class to the dot
    slides[slideIndex].classList.add('active');
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

function saveReservation() {
    const arriving = document.getElementById('arriving').value;
    const leaving = document.getElementById('leaving').value;

    // Check if both arriving and leaving dates are selected
    if (arriving && leaving) {
        localStorage.setItem('arriving', arriving);
        localStorage.setItem('leaving', leaving);
        
    } else {
        alert('Please select both arrival and departure dates.');
    }
}

document.addEventListener("DOMContentLoaded", () => {
    const burgerMenu = document.getElementById("burgerMenu");
    const navLinks = document.getElementById("links");

    // Toggle the burger menu and links
    burgerMenu.addEventListener("click", () => {
        burgerMenu.classList.toggle("active");
        navLinks.classList.toggle("active");
    });
});
