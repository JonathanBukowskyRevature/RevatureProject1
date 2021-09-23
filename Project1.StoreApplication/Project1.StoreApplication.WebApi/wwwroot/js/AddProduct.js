
function AddProduct(form) {
    console.log("Adding product...");
    let data = {
        name: form.name.value,
        description: form.description.value,
        price: form.price.value,
        //categoryID: form.categoryID.value
    }

    fetch(`/api/products/add`, {
        method: "POST",
        headers: {
            'content-type': "application/json"
        },
        body: JSON.stringify(data)
    }).then(res => {
        if (res.ok) {
            window.location.href = "/html/products.html";
        }
    });
}

window.addEventListener('load', function () {
    let form = document.querySelector('.add-product-form');

    form.addEventListener('submit', function (e) {
        e.preventDefault();
        AddProduct(form);
    });
});
