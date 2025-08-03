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

