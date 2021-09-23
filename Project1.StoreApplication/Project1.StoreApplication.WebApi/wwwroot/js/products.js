
//var moneyFormatter = new Intl.NumberFormat("en-US", { style: 'currency', currency: "USD" });
var moneyFormatter = { format: () => "fakemoney" }

function CreateProductFromTemplate(template, product) {
    let newElm = template.cloneNode(true);
    newElm.classList.remove("template-product");
    newElm.classList.add("generated-product");
    let fakeDoc = new DocumentFragment();
    fakeDoc.appendChild(newElm);
    //let nameElm = fakeDoc.querySelector("template-product-name");
    let nameElm = newElm.getElementsByClassName("template-product-name").item(0);
    nameElm.classList.remove("template-product-name");
    nameElm.textContent = product.name;
    //let descElm = fakeDoc.querySelector("template-product-description");
    let descElm = newElm.getElementsByClassName("template-product-description").item(0);
    descElm.textContent = product.description;
    descElm.classList.remove("template-product-description");
    //let priceElm = fakeDoc.querySelector("template-product-price");
    let priceElm = newElm.getElementsByClassName("template-product-price").item(0);
    priceElm.textContent = moneyFormatter.format(product.price);
    priceElm.classList.remove("template-product-price");
    //let quantElm = fakeDoc.querySelector("template-product-quantity");
    let quantElm = newElm.getElementsByClassName("template-product-quantity").item(0);
    quantElm.textContent = product.quantity;
    quantElm.classList.remove("template-product-quantity");

    return newElm;
}

function RemoveProducts(container) {
    /*
    let products = Array.from(container.getElementsByClassName("generated-product"));
    console.log("products to remove: ", products);
    if (!products) return;
    products.forEach(child => {
        container.removeChild(child);
    });
    */
    console.log("container", container);
    Models.RemoveGeneratedElements(container, 'generated-product');
}

/*
function DisplayProducts(products, container) {
    let template = document.querySelector('.template-product');
    console.log("template", template);
    console.log("products", products);
    products.forEach(product => {
        let productElm = CreateProductFromTemplate(template, product);
        container.appendChild(productElm);
    });
}
*/

function DisplayProducts(products, container) {
    let template = document.querySelector('.template-product');
    console.log("template", template);
    console.log("products", products);
    products.forEach(product => {
        let productElm = (new Models.Product(product)).CreateElementFromTemplate(template);
        container.appendChild(productElm);
    });
}

function LoadProducts(container) {
    fetch('/api/products')
        .then(res => {
            return res.json();
        }).then(data => {
            RemoveProducts(container);
            DisplayProducts(data, container);
        })
}


window.addEventListener('load', () => {
    let container = document.querySelector('.product-list');
    LoadProducts(container);

    let btn = document.querySelector('.add-product-button');
    btn.addEventListener('click', function () {
        window.location.href = "/html/addProduct.html";
    });
});


