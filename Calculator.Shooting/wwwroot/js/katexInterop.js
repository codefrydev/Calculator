// Function to be called from Blazor
window.katexInterop = {
    render: function (elementId, latexString, displayMode) {
        // Find the DOM element where the formula should be rendered
        const element = document.getElementById(elementId);
        if (element) {
            try {
                // Call the KaTeX render function
                katex.render(latexString, element, {
                    displayMode: displayMode, // true for block, false for inline
                    throwOnError: false // Prevents application crash on bad syntax
                });
            } catch (e) {
                console.error("KaTeX rendering failed:", e);
                element.innerHTML = `<span style="color: red;">[Math Error: ${e.message}]</span>`;
            }
        }
    }
};