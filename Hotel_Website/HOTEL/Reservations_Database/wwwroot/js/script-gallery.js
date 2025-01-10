const images = document.querySelectorAll(".gallery-item img");
const modal = document.getElementById("imageModal");
const modalImage = document.getElementById("modalImage");
let currentIndex = 0;

function openModal(index) {
    currentIndex = index;
    modalImage.src = images[currentIndex].src;
    modal.style.display = "flex";
}

function closeModal() {
    modal.style.display = "none";
}

function changeImage(direction) {
    currentIndex = (currentIndex + direction + images.length) % images.length;
    modalImage.src = images[currentIndex].src;
}

// Close modal on clicking outside or pressing escape
window.addEventListener("click", (e) => {
    if (e.target === modal) {
        closeModal();
    }
});

window.addEventListener("keydown", (e) => {
    if (e.key === "Escape") {
        closeModal();
    } else if (e.key === "ArrowLeft") {
        changeImage(-1);
    } else if (e.key === "ArrowRight") {
        changeImage(1);
    }
});

document.addEventListener("DOMContentLoaded", () => {
    const burgerMenu = document.getElementById("burgerMenu");
    const navLinks = document.getElementById("links");

    // Toggle the burger menu and links
    burgerMenu.addEventListener("click", () => {
        burgerMenu.classList.toggle("active");
        navLinks.classList.toggle("active");
    });
});