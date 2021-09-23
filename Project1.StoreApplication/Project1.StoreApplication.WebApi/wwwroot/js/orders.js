
if (!Models) {
    throw new Error("Must include ModelObjects");
}

var ErrTimeout;

function DisplayError(err) {
    if (ErrTimeout) {
        ClearError();
    }
    let container = document.querySelector('.status');

    container.innerHTML = err.message;

    ErrTimeout = setTimeout(ClearError, 3000);
}

function ClearError() {
    let container = document.querySelector('.status');
    container.innerHTML = "";

    if (ErrTimeout) {
        clearTimeout(ErrTimeout);
    }
    ErrTimeout = undefined;
}

function LoadOrders(type, container, template) {
    function populateContainer(elms) {
        Models.RemoveGeneratedElements(container, 'generated-order');
        if (!elms || !elms.length) {
            throw new Error("No order data");
        }
        elms.forEach(ord => {
            let order = new Models.Order(ord);
            let elm = order.CreateElementFromTemplate(template, 'template-order');
            container.appendChild(elm);
        });
    }
    ClearError();
    if (type == "Store") {
        let store = JSON.parse(sessionStorage.selectedStore);
        if (!store) {
            throw new Error("Must have store selected");
        }
        fetch(`/api/stores/${store.storeId}/orders`)
            .then(res => {
                return res.json();
            }).then(data => {
                populateContainer(data);
            }).catch(err => {
                DisplayError(err);
            });
    } else if (type == "Customer") {
        let cust = JSON.parse(sessionStorage.user);
        if (!cust) {
            throw new Error("Must be logged in");
        }
        fetch(`/api/customers/${cust.customerId}/orders`)
            .then(res => {
                return res.json();
            }).then(data => {
                populateContainer(data);
            }).catch(err => {
                DisplayError(err);
            });
    } else {
        throw new Error("Invalid type in LoadOrders");
    }
}

window.addEventListener('load', function () {
    let selector = document.querySelector('.history-selector');
    console.log("adding handler...");

    let orderContainer = document.querySelector(".order-list");
    let templateOrder = document.querySelector(".template-order");
    selector.addEventListener('change', function (e) {
        if (e.target.value) {
            LoadOrders(e.target.value, orderContainer, templateOrder);
        }
    });
});
