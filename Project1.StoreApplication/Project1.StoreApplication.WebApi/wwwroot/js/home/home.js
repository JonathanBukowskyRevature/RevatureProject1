

function DisplayProduct(product) {
    // TODO:
    return `<p>Product: ${product.name} Price: ${product.price} Description: ${product.description}</p>`;
}


function DisplayProductTable(products, storeId, getButton) {
    let prodDisplay = "<table><thead><tr><th>Name</th><th>Price</th><th>Description</th><th>Quantity</th><th></th></tr></thead><tbody>";
    if (products !== undefined) {
        products.forEach(product => {
            prodDisplay += "<tr>";
            prodDisplay += `<td>${product.name}</td>`;
            prodDisplay += `<td>${product.price}</td>`;
            prodDisplay += `<td>${product.description}</td>`;
            prodDisplay += `<td>${product.quantity > 0 ? product.quantity : ""}</td>`;
            prodDisplay += `<td>${getButton(storeId, product)}</td>`;
            prodDisplay += "</tr>";
        });
    }
    prodDisplay += "</tbody></table>";
    return prodDisplay;
}

/*
function DisplayOrder(order) {
    var html = '<h4>Order:</h4>'
        + `<div><p>Created By ${order.customer.firstName} ${order.customer.lastName}</p>`
        + `<p>Store Location: ${order.store.name}</p>`
        + `<ul>`;
    let cost = 0.0;
    order.products.forEach(product => {
        let quantity = (product.quantity) ? product.quantity : 1;
        let subtotal = product.price * quantity;
        cost += subtotal;
        html += '<li>'
            + `<p>${product.name} - ${product.description}</p>`
            + `<p>Price: ${product.price}</p>`
            + `<p>Quantity: ${quantity}</p>`
            + `<p>Subtotal: ${subtotal}</p>`
            + `</li>`
    })
    html += `</ul><p>Total: ${cost}</p></div>`;
    return html;
}
*/

var OrderProductsHeader =
    '<div class="row larger-text">' +
    '<div class="col-md-3">' +
        '<p>Product</p>' +
    '</div>' +
    '<div class="col-md-3">' +
        '<p>Price</p>' +
    '</div>' +
    '<div class="col-md-3">' +
        '<p>Quantity</p>' +
    '</div>' +
    '<div class="col-md-3">' +
        '<p>Subtotal</p>' +
    '</div>' +
    '</div>';

function DisplayOrderProducts(order) {
    let rowsHtml = "";
    let total = 0;
    order.products.forEach(product => {
        let subtotal = product.quantity * product.price;
        total += subtotal;
        rowsHtml +=
            '<div class="row">' +
                '<div class="col-md-3">' +
                    `<p>${product.name}</p>` +
                '</div>' +
                '<div class="col-md-3">' +
                    `<p>${product.price}</p>` +
                '</div>' +
                '<div class="col-md-3">' +
                    `<p>${product.quantity}</p>` +
                '</div>' +
                '<div class="col-md-3">' +
                    `<p>${subtotal}</p>` +
                '</div>' +
            '</div>';
    });
    let html = 
        '<div class="container">' +
            OrderProductsHeader +
            rowsHtml +
            '<div class="row">' +
                '<div class="col-md-10">' +
                '</div>' +
                '<div class="col-md-2">' +
                    `<p>Total: ${total}</p>` +
                '</div>'
            '</div>' +
        '</div>'
    return html;
}

function DisplayOrder(order) {
    let html =
        '<div class="row order">' +
        '<div class="container">' +
        '<div class="row">' +
        '<div class="col order-header"><h4 class="center-text larger-text">Order</h4></div>' +
        '</div>' +
        '<div class="row center-text">' +
        `<div class="col-md-6"><p>Customer: ${order.customer.firstName} ${order.customer.lastName}</p></div>` +
        `<div class="col-md-6"><p>Store: ${order.store.name}</p></div>` +
        '</div>' +
        '<div class="row">' +
        '<div class="col-md-2"></div>' +
        '<div class="col-md-10">' +
        DisplayOrderProducts(order) +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>';
    return html;
}

function DisplayOrderHistory(orders) {
    let container = document.querySelector(".order-history-content");
    let display = "";
    console.log("order history:", orders);
    orders.forEach(order => {
        display += DisplayOrder(order);
    });
    container.innerHTML = display;
}

function GetOrderHistory(type) {
    let btn = document.querySelector(".change-history");
    if (type == "Store") {
        let store = JSON.parse(sessionStorage.getItem("selectedStore"));
        fetch(`/api/stores/${store.storeId}/orders`)
            .then(res => {
                return res.json();
            }).then(data => {
                console.log(type + " order history:", data);
                DisplayOrderHistory(data);
                btn.outerHTML = GetChangeHistoryButton("Customer");
            })
    } else if (type == "Customer") {
        let cust = JSON.parse(sessionStorage.getItem("user"));
        fetch(`/api/customers/${cust.customerId}/orders`)
           .then(res => {
              return res.json();
           }).then(data => {
              console.log(type + " order history:", data);
              DisplayOrderHistory(data);
              btn.outerHTML = GetChangeHistoryButton("Store");
           })
    } else {
        let container = document.querySelector(".status");
        container.innerHTML = "<p>Error -- invalid type in GetOrderHistory</p>";
        setTimeout(() => container.innerHTML = "", 4000);
    }
}

function GetChangeHistoryButton(type) {
    let target = `GetOrderHistory('${type}')`;
    return `<button class="change-history" onclick="${target}">View ${type} History</button>`;
}

function ViewOrderHistory() {
    let homeDiv = document.querySelector(".home-content");
    console.log("View orders");
    let changeButton = GetChangeHistoryButton("Store");
    homeDiv.innerHTML = `<h2>Order History:</h2><div>${changeButton}<div class="order-history-content"></div></div>`;
    GetOrderHistory("Customer");
}


function ChangeStore() {
    sessionStorage.removeItem("selectedStore");
    window.location.href = "/html/stores.html";
}

function Logout() {
    sessionStorage.removeItem("selectedStore");
    sessionStorage.removeItem("user");
    window.location.href = "/index.html";
}
