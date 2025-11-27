window.katexInterop = {
    // Renders ONE single LaTeX string into ONE specific element ID
    render: function (elementId, latexString, displayMode) {
        const element = document.getElementById(elementId);
        if (element) {
            try {
                katex.render(latexString, element, {
                    displayMode: displayMode,
                    throwOnError: false
                });
            } catch (e) {
                console.error("KaTeX rendering failed:", e);
                element.innerHTML = `<span style="color: red;">[Math Error]</span>`;
            }
        }
    }
};