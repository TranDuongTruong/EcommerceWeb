document.addEventListener('DOMContentLoaded', function () {
    var dropdownToggle = document.getElementById('managedropdownmenu');
    var dropdownMenu = document.querySelector('.dropdown-menu');

    dropdownToggle.addEventListener('click', function (event) {
        event.preventDefault(); // Ngăn chặn hành động mặc định của liên kết
        dropdownMenu.classList.toggle('show');
    });

    // Đóng dropdown khi nhấp ra ngoài
    window.addEventListener('click', function (event) {
        if (!dropdownToggle.contains(event.target) && !dropdownMenu.contains(event.target)) {
            dropdownMenu.classList.remove('show');
        }
    });
});
