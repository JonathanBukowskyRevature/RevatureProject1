
function AddStore(form) {
    let data = {
        name: form.name.value
    };

    fetch(`/api/stores/add`, {
        method: "POST",
        headers: {
            'content-type': "application/json"
        },
        body: JSON.stringify(data)
    }).then(res => {
        if (res.ok) {
            window.location.href = "/html/stores.html";
        }
    });
}

window.addEventListener('load', function () {
    let form = document.querySelector('.add-store-form');

    form.addEventListener('submit', function (e) {
        e.preventDefault();
        AddStore(form);
    });
});
