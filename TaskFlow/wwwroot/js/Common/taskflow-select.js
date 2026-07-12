document.addEventListener("DOMContentLoaded", function () {

    document.querySelectorAll(".tf-select").forEach(function (element) {

        if (element.tomselect) {
            return;
        }

        new TomSelect(element, {
            create: false,
            maxOptions: 500,
            closeAfterSelect: true,
            hidePlaceholder: true,
            placeholder: element.dataset.placeholder || "Search...",
            sortField: [
                {
                    field: "text",
                    direction: "asc"
                }
            ]
        });

    });

});