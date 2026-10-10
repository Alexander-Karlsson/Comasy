document.addEventListener('DOMContentLoaded', function () {
    const select = document.getElementById('blockTypeSelect');
    if (!select) return;

    const groups = document.querySelectorAll('[data-types]');

    function updateVisibleFields() {
        const selected = select.options[select.selectedIndex].dataset.name;

        groups.forEach(function (group) {
            const types = group.dataset.types.split(' ');
            group.hidden = !types.includes(selected);
        });
    }

    select.addEventListener('change', updateVisibleFields);
    updateVisibleFields();
});