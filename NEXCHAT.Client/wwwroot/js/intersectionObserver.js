window.intersectionObserver = {
    observe: function (element, dotNetHelper) {
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    dotNetHelper.invokeMethodAsync('LoadMore');
                }
            });
        }, {
            root: null, // Use the viewport
            threshold: 0.1 // Trigger when at least 10% is visible
        });
        observer.observe(element);
    }
};

// Scroll to bottom function
function scrollToBottom(element) {
    element.scrollTop = element.scrollHeight;
}

// Initialize as global function
window.scrollToBottom = scrollToBottom;
