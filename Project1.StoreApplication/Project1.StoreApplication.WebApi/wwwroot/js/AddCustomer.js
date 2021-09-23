
function SubmitCustomerData(form) {
    let data = {
        username: form.username.value,
        password: form.password.value,
        firstName: form.firstName.value,
        lastName: form.lastName.value
    }

    fetch(`/api/customers/add`, {
        method: "POST",
        headers: {
            'content-type': "application/json"
        },
        body: JSON.stringify(data)
    }).then(res => {
        if (res.ok) {
            window.location.href = "/html/customers.html";
        }
    })
}

window.addEventListener('load', function () {
    let form = document.querySelector('.add-customer-form');

    form.addEventListener('submit', function (e) {
        e.preventDefault();
        SubmitCustomerData(form);
    })
})
