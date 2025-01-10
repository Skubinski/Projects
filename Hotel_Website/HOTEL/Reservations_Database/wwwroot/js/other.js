document.addEventListener("DOMContentLoaded", () => {
    const burgerMenu = document.getElementById("burgerMenu");
    const navLinks = document.getElementById("links");

    // Toggle the burger menu and links
    burgerMenu.addEventListener("click", () => {
        burgerMenu.classList.toggle("active");
        navLinks.classList.toggle("active");
    });
});