const form = document.getElementById('registrationForm');
const nameInput = document.getElementById('name');
const emailInput = document.getElementById('email');
const courseSelect = document.getElementById('course');
const termsCheckbox = document.getElementById('terms');
const output = document.getElementById('output');

const nameError = document.getElementById('nameError');
const emailError = document.getElementById('emailError');
const genderError = document.getElementById('genderError');
const courseError = document.getElementById('courseError');
const termsError = document.getElementById('termsError');

form.addEventListener('submit', function(e) {
    e.preventDefault();
    
    let isValid = true;

    nameError.style.display = 'none';
    emailError.style.display = 'none';
    genderError.style.display = 'none';
    courseError.style.display = 'none';
    termsError.style.display = 'none';

    if (nameInput.value.trim() === '') {
        nameError.style.display = 'block';
        isValid = false;
    }

    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailPattern.test(emailInput.value)) {
        emailError.style.display = 'block';
        isValid = false;
    }

    const genderInputs = document.querySelectorAll('input[name="gender"]');
    let genderSelected = false;
    let selectedGender = '';
    
    genderInputs.forEach(input => {
        if (input.checked) {
            genderSelected = true;
            selectedGender = input.value;
        }
    });

    if (!genderSelected) {
        genderError.style.display = 'block';
        isValid = false;
    }

    if (courseSelect.value === '') {
        courseError.style.display = 'block';
        isValid = false;
    }

    if (!termsCheckbox.checked) {
        termsError.style.display = 'block';
        isValid = false;
    }

    if (isValid) {
        document.getElementById('outputName').textContent = nameInput.value;
        document.getElementById('outputEmail').textContent = emailInput.value;
        document.getElementById('outputGender').textContent = selectedGender;
        document.getElementById('outputCourse').textContent = courseSelect.value;
        
        output.style.display = 'block';
        
        form.reset();
    }
});