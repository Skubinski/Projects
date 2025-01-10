// Global variable to track if the price has been calculated
let priceCalculated = false;

// On page load, initialize the form and setup event listeners
function initializeForm() {
    const arriving = localStorage.getItem("arriving");
    const leaving = localStorage.getItem("leaving");

    if (arriving) document.getElementById("arriving").value = arriving;
    if (leaving) document.getElementById("leaving").value = leaving;

    setupDateFields(); // Set min/max values for date fields
    updateOptions(true); // Initialize dropdown options
}

// Update dropdown options based on the selected room
function updateOptions(fromLoad = false) {
    const room = document.getElementById("room").value;
    const adultsSelect = document.getElementById("adults");
    const childrenSelect = document.getElementById("children");

    adultsSelect.innerHTML = "";
    childrenSelect.innerHTML = "";

    let maxPeople = 0;

    // Determine max capacity based on room type
    switch (room) {
        case "2": case "3":
            maxPeople = 2;
            break;
        case "4": case "5":
            maxPeople = 3;
            break;
        case "6": case "7":
            maxPeople = 5;
            break;
    }

    // Populate adults dropdown
    for (let i = 1; i <= maxPeople; i++) {
        const option = document.createElement("option");
        option.value = i;
        option.textContent = i;
        adultsSelect.appendChild(option);
    }

    // Update children dropdown based on selected adults
    adultsSelect.addEventListener("change", () => updateChildrenOptions(maxPeople));
    updateChildrenOptions(maxPeople);

    if (fromLoad) validateAdultsAndChildren(maxPeople);
}

// Update children dropdown based on selected adults
function updateChildrenOptions(maxPeople) {
    const adultsSelect = document.getElementById("adults");
    const childrenSelect = document.getElementById("children");

    const selectedAdults = parseInt(adultsSelect.value) || 1;
    childrenSelect.innerHTML = "";

    const maxChildren = Math.max(0, maxPeople - selectedAdults);
    for (let i = 0; i <= maxChildren; i++) {
        const option = document.createElement("option");
        option.value = i;
        option.textContent = i;
        childrenSelect.appendChild(option);
    }
}

// Validate adults and children count
function validateAdultsAndChildren(maxPeople) {
    const adultsSelect = document.getElementById("adults");
    const childrenSelect = document.getElementById("children");

    let selectedAdults = parseInt(adultsSelect.value) || 1;
    let selectedChildren = parseInt(childrenSelect.value) || 0;

    if (selectedAdults + selectedChildren > maxPeople) {
        selectedAdults = maxPeople;
        selectedChildren = 0;
        adultsSelect.value = selectedAdults;
        updateChildrenOptions(maxPeople);
        childrenSelect.value = selectedChildren;
    }
}

// Setup date fields with min and default values
function setupDateFields() {
    const arrivingInput = document.getElementById("arriving");
    const leavingInput = document.getElementById("leaving");
    const today = new Date();
    const tomorrow = new Date(today);
    tomorrow.setDate(today.getDate() + 1);

    arrivingInput.min = today.toISOString().split("T")[0];
    arrivingInput.value = today.toISOString().split("T")[0];
    leavingInput.min = tomorrow.toISOString().split("T")[0];
    leavingInput.value = tomorrow.toISOString().split("T")[0];

    arrivingInput.addEventListener("change", () => {
        const arrivingDate = new Date(arrivingInput.value);
        const minLeavingDate = new Date(arrivingDate);
        minLeavingDate.setDate(arrivingDate.getDate() + 1);
        leavingInput.min = minLeavingDate.toISOString().split("T")[0];
        leavingInput.value = minLeavingDate.toISOString().split("T")[0];
    });
}

// Validate and submit the form
function validateForm(event) {
    event.preventDefault();

    const name = document.getElementById("name").value.trim();
    const secondName = document.getElementById("second-name").value.trim();
    const surname = document.getElementById("surname").value.trim();
    const phone = document.getElementById("phone").value.trim();
    const mail = document.getElementById("mail").value.trim();
    const arriving = document.getElementById("arriving").value.trim();
    const leaving = document.getElementById("leaving").value.trim();
    const room = document.getElementById("room").value;

    const nameRegex = /^[^0-9]*$/;
    const phoneRegex = /^[0-9]+$/;
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!nameRegex.test(name)) {
        alert("Името не трябва да съдържа цифри.");
        return false;
    }
    if (!nameRegex.test(secondName)) {
        alert("Презимето не трябва да съдържа цифри.");
        return false;
    }
    if (!nameRegex.test(surname)) {
        alert("Фамилията не трябва да съдържа цифри.");
        return false;
    }
    if (!phoneRegex.test(phone)) {
        alert("Телефонният номер трябва да бъде само с цифри.");
        return false;
    }
    if (!emailRegex.test(mail)) {
        alert("Невалиден и-мейл адрес.");
        return false;
    }
    if (!arriving || !leaving) {
        alert("Изберете дата на пристигане и напускане.");
        return false;
    }

    const arrivingDate = new Date(arriving);
    const leavingDate = new Date(leaving);
    const differenceInDays = (leavingDate - arrivingDate) / (24 * 60 * 60 * 1000);

    if (differenceInDays <= 0) {
        alert("Датата на напускане трябва да бъде след датата на пристигане.");
        return false;
    }

    const roomPriceMap = { 2: 140, 3: 190, 4: 180, 5: 230, 6: 400, 7: 600 };
    const roomPrice = roomPriceMap[room];
    if (!roomPrice) {
        alert("Изберете стая.");
        return false;
    }

    const totalPrice = roomPrice * differenceInDays;

    // Display the calculated price
    const submitButton = document.getElementById("submit-button");
    if (!priceCalculated) {
        submitButton.textContent = `Резервирайте (${totalPrice} лв.)`;
        priceCalculated = true;
        return false; // Prevent submission for user confirmation
    }

    alert(`Успешно резервирахте вашият престой! Общо: ${totalPrice} лв.`);
    
    // Submit form via Fetch API
    submitReservation(totalPrice);
}

// Submit form data to the server and reset the form
function submitReservation(totalPrice) {
    const formData = new FormData(document.querySelector('form'));
    formData.append("totalPrice", totalPrice);

    fetch('http://localhost:5225/Reservation/Create', {
        method: 'POST',
        body: formData
    })
    .then(response => {
        if (!response.ok) {
            return response.text().then(text => {
                throw new Error(`HTTP ${response.status}: ${text}`);
            });
        }
        return response.text();
    })
    .then(data => {
        console.log('Reservation created successfully:', data);
        document.querySelector("form").reset(); // Reset the form
        document.getElementById("submit-button").textContent = "Резервирай"; // Reset button text
        priceCalculated = false; // Reset priceCalculated flag
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Failed to submit reservation.');
    });
}

// Initialize the form on page load
document.addEventListener("DOMContentLoaded", initializeForm);

document.addEventListener('DOMContentLoaded', () => {
    const arriving = localStorage.getItem('arriving');
    const leaving = localStorage.getItem('leaving');

    if (arriving) document.getElementById('arriving').value = arriving;
    if (leaving) document.getElementById('leaving').value = leaving;
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