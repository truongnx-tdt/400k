// Validation functions
function isValidEmail(email) {
    return email.indexOf("@") > 0 && email.indexOf(".") > email.indexOf("@");
}

function isValidPhone(phone) {
    return /^[0-9]{10,11}$/.test(phone) &&
        (phone.startsWith('84') || ['03', '05', '07', '08', '09'].includes(phone.substring(0, 2)));
}

function validateForm() {
    console.log("Validating form...");
    let isValid = true;
    const errors = [];

    // Required fields validation
    const requiredFields = ['FullName', 'Email', 'PhoneNumber', 'DateOfBirth', 'ClassId'];
    requiredFields.forEach(field => {
        const value = $(`#${field}`).val();
        const fieldElement = $(`#${field}`);
        const errorElement = $(`#${field}`).next('.text-danger');

        if (!value) {
            isValid = false;
            fieldElement.addClass('is-invalid');
            errors.push(`Vui lòng nhập ${fieldElement.prev('label').text().toLowerCase()}`);
        } else {
            fieldElement.removeClass('is-invalid');
        }
    });

    // Email validation
    const email = $('#Email').val();
    if (email && !isValidEmail(email)) {
        isValid = false;
        $('#Email').addClass('is-invalid');
        errors.push('Email không đúng định dạng');
    }

    // Phone validation
    const phone = $('#PhoneNumber').val();
    if (phone && !isValidPhone(phone)) {
        isValid = false;
        $('#PhoneNumber').addClass('is-invalid');
        errors.push('Số điện thoại không đúng định dạng');
    }

    // Date of birth validation
    const dob = $('#DateOfBirth').val();
    if (dob) {
        const birthDate = new Date(dob);
        const today = new Date();
        if (birthDate > today) {
            isValid = false;
            $('#DateOfBirth').addClass('is-invalid');
            errors.push('Ngày sinh không được lớn hơn ngày hiện tại');
        }
    }

    // Show validation summary if there are errors
    if (!isValid) {
        const errorHtml = `
            <div class="alert alert-danger mb-3">
                <h6 class="alert-heading"><i class="fas fa-exclamation-triangle me-2"></i>Vui lòng sửa các lỗi sau:</h6>
                <ul class="mb-0 mt-2">
                    ${errors.map(error => `<li>${error}</li>`).join('')}
                </ul>
            </div>
        `;
        $('.alert').remove();
        $('form').prepend(errorHtml);
        $('html, body').animate({
            scrollTop: $('.alert').offset().top - 100
        }, 500);
    }

    console.log("Form validation result:", isValid);
    return isValid;
}

// Wait for document ready
$(document).ready(function () {
    console.log("Validation script loaded");

    // Initialize datetimepicker
    if ($.fn.datetimepicker) {
        $('.datetimepicker').datetimepicker({
            format: 'L'
        });
    }

    // Add hover effect to form controls
    $('.form-control, .form-select').hover(
        function () { $(this).addClass('shadow'); },
        function () { $(this).removeClass('shadow'); }
    );

    // Handle form submission
    $('#createStudentForm').on('submit', function (e) {
        console.log('Form submission intercepted');
        if (!validateForm()) {
            e.preventDefault();
            console.log('Form validation failed');
        } else {
            console.log('Form validation passed');
        }
    });

    // Handle save button click
    $('#saveButton').on('click', function (e) {
        console.log('Save button clicked');
        if (!validateForm()) {
            e.preventDefault();
            console.log('Form validation failed');
        } else {
            console.log('Form validation passed');
        }
    });

    // Clear validation styling on input change
    $('.form-control, .form-select').on('input change', function () {
        $(this).removeClass('is-invalid');
        const errorElement = $(this).next('.text-danger');
        if (errorElement.length) {
            errorElement.remove();
        }
    });

    // Handle preview button click
    $('#previewModal').on('show.bs.modal', function () {
        updatePreview();
    });

    function updatePreview() {
        // Get form values
        const fullName = $('#FullName').val() || '-';
        const email = $('#Email').val() || '-';
        const phone = $('#PhoneNumber').val() || '-';
        const address = $('#Address').val() || '-';
        const dob = $('#DateOfBirth').val() || '-';
        const gender = $('input[name="Gender"]:checked').val() === 'True' ? 'Nam' : 'Nữ';
        const className = $('#ClassId option:selected').text() || '-';

        // Format date if exists
        let formattedDate = '-';
        if (dob !== '-') {
            const date = new Date(dob);
            formattedDate = date.toLocaleDateString('vi-VN', {
                year: 'numeric',
                month: 'long',
                day: 'numeric'
            });
        }

        // Update preview fields with formatted data
        $('#previewFullName').text(fullName);
        $('#previewEmail').text(email);
        $('#previewPhone').text(phone);
        $('#previewAddress').text(address);
        $('#previewDOB').text(formattedDate);
        $('#previewGender').text(gender);
        $('#previewClass').text(className);

        // Add validation styling
        const requiredFields = ['FullName', 'Email', 'PhoneNumber', 'DateOfBirth', 'ClassId'];
        requiredFields.forEach(field => {
            const value = $(`#${field}`).val();
            const previewElement = $(`#preview${field}`);
            if (!value) {
                previewElement.addClass('text-danger').append(' <i class="fas fa-exclamation-circle text-danger"></i>');
            } else {
                previewElement.removeClass('text-danger');
                previewElement.find('i').remove();
            }
        });

        // Validate email format
        if (email !== '-' && !isValidEmail(email)) {
            $('#previewEmail').addClass('text-danger').append(' <i class="fas fa-exclamation-circle text-danger"></i>');
        }

        // Validate phone format
        if (phone !== '-' && !isValidPhone(phone)) {
            $('#previewPhone').addClass('text-danger').append(' <i class="fas fa-exclamation-circle text-danger"></i>');
        }

        // Add validation summary
        const invalidFields = $('.text-danger').length;
        if (invalidFields > 0) {
            $('.modal-body').prepend(`
                <div class="alert alert-warning mb-3">
                    <i class="fas fa-exclamation-triangle me-2"></i>
                    Có trường thông tin chưa hợp lệ. Vui lòng kiểm tra lại.
                </div>
            `);
        } else {
            $('.alert').remove();
        }
    }

    // Clear validation styling when modal is hidden
    $('#previewModal').on('hidden.bs.modal', function () {
        $('.text-danger').removeClass('text-danger');
        $('.fas.fa-exclamation-circle').remove();
        $('.alert').remove();
    });
});
