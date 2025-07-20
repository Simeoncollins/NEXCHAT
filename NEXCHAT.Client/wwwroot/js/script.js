//// Toggle user dropdown menu
//const userMenuButton = document.getElementById('userMenuButton');
//const userDropdown = document.getElementById('userDropdown');
//userMenuButton.addEventListener('click', (e) => {
//    // Prevent event propagation so that document click doesn't immediately hide the dropdown
//    e.stopPropagation();
//    userDropdown.classList.toggle('invisible');
//    userDropdown.classList.toggle('opacity-100');
//});

//// Close the user dropdown when clicking outside
//document.addEventListener('click', (e) => {
//    if (!userMenuButton.contains(e.target) && !userDropdown.contains(e.target)) {
//        userDropdown.classList.add('invisible');
//        userDropdown.classList.remove('opacity-100');
//    }
//});