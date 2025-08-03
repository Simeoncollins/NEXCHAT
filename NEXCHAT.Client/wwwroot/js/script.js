
// Scroll to bottom function
function scrollToBottom(element) {
    if (!element) {
        console.warn("scrollToBottom: Element is null");
        return;
    }
    window.scrollTo(0, element.scrollHeight)
}

// Initialize as global function
window.scrollToBottom = scrollToBottom;