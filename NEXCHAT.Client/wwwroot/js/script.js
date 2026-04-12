
// Scroll to bottom function
function scrollToBottom(element) {
    if (!element) {
        console.warn("scrollToBottom: Element is null");
        return;
    }
    window.scrollTo(0, element.scrollHeight)
}

// Focus a given element (used when entering edit mode)
function focusElement(element) {
    if (element) element.focus();
}

// Copy text to clipboard (used by message bottom sheet)
async function copyToClipboard(text) {
    try {
        await navigator.clipboard.writeText(text);
    } catch {
        // fallback for older browsers
        const ta = document.createElement('textarea');
        ta.value = text;
        document.body.appendChild(ta);
        ta.select();
        document.execCommand('copy');
        document.body.removeChild(ta);
    }
}

// Initialize as global functions
window.scrollToBottom = scrollToBottom;
window.focusElement = focusElement;
window.copyToClipboard = copyToClipboard;
