// Toggle function with title change
function setupToggle(passwordFieldId, toggleIconId) {
    const passwordField = document.getElementById(passwordFieldId);
    const toggleIcon = document.getElementById(toggleIconId);

    toggleIcon.addEventListener('click', function () {
        const isPassword = passwordField.getAttribute('type') === 'password';
        passwordField.setAttribute('type', isPassword ? 'text' : 'password');

        // Toggle icon and title
        this.classList.toggle('fa-eye');
        this.classList.toggle('fa-eye-slash');
        this.setAttribute('title', isPassword ? 'Hide Password' : 'Show Password');
    });
}

setupToggle('password', 'togglePassword');
setupToggle('confirmPassword', 'toggleConfirm');

// Password match validation
const password = document.getElementById('password');
const confirmPassword = document.getElementById('confirmPassword');
const errorMessage = document.getElementById('errorMessage');

confirmPassword.addEventListener('input', function () {
    if (confirmPassword.value !== password.value) {
        confirmPassword.classList.add('error');
        errorMessage.style.display = 'block';
    } else {
        confirmPassword.classList.remove('error');
        errorMessage.style.display = 'none';
    }
});
