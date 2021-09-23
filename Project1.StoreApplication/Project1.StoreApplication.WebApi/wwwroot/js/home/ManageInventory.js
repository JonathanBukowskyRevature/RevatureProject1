
function MakeRemoveFromInventoryButton(storeId, product) {
    return `<button onclick="RemoveFromInventory(${storeId},${product.productId})">Remove</button>`;
}

function MakeAddToInventoryButton(storeId, product) {
    return `<button onclick="AddToInventory(${storeId},${product.productId})">Add</button>`
}

function RemoveFromInventory(storeId, productId) {
    console.log(`Remove ${productId} from inventory (${storeId})`)
    fetch(`/api/stores/${storeId}/products/remove`, {
        method: "POST",
        body: JSON.stringify({ productId }),
        headers: {
            'content-type': "application/json"
        }
    }).then(res => {
        return res.json();
    }).then(data => {
        console.log(data);
        let container = document.querySelector(".store-products-content");
        container.innerHTML = DisplayProductTable(data, storeId, MakeRemoveFromInventoryButton);
    })
}

function AddToInventory(storeId, productId) {
    fetch(`/api/stores/${storeId}/products/add`, {
        method: "POST",
        body: JSON.stringify({ productId }),
        headers: {
            'content-type': "application/json"
        }
    }).then(res => {
        return res.json();
    }).then(data => {
        console.log(data);
        let container = document.querySelector(".store-products-content");
        container.innerHTML = DisplayProductTable(data, storeId, MakeRemoveFromInventoryButton);
    })
}

function ViewInventory() {
    let homeDiv = document.querySelector(".home-content");
    let store = JSON.parse(sessionStorage.getItem("selectedStore"));
    console.log(`Manage Inventory for ${store.name}`);
    homeDiv.innerHTML = '<h2>Manage Inventory:</h2>'
        + '<div><h3>Inventory:</h3><div class="store-products-content"></div></div>'
        + '<div><h3>Products:</h3><div class="all-products-content"></div></div>';
    fetch(`/api/stores/${store.storeId}/products`)
        .then(res => {
            return res.json();
        }).then(data => {
            console.log(data);
            let container = document.querySelector(".store-products-content");
            container.innerHTML = DisplayProductTable(data, store.storeId, MakeRemoveFromInventoryButton);
        });
    fetch('/api/products')
        .then(res => {
            //console.log(res);
            return res.json();
        })
        .then(data => {
            //console.log(s);
            //data = JSON.parse(s);
            console.log(data);
            let container = document.querySelector(".all-products-content");
            /*
            let prodDisplay = "<table><thead><tr><th>Name</th><th>Price</th><th>Description</th><th></th></tr></thead><tbody>";
            if (data !== undefined) {
                data.forEach(product => {
                    prodDisplay += "<tr>";
                    prodDisplay += `<td>${product.name}</td>`;
                    prodDisplay += `<td>${product.price}</td>`;
                    prodDisplay += `<td>${product.description}</td>`;
                    prodDisplay += `<td><button onclick="AddToInventory(${store.storeId}, ${product.productId})">Add to inventory</button></td>`;
                    prodDisplay += "</tr>";
                });
            }
            prodDisplay += "</tbody></table>";
            */
            container.innerHTML = DisplayProductTable(data, store.storeId, MakeAddToInventoryButton);
        });
}