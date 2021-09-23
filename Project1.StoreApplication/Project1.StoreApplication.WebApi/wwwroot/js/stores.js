
let htmlBeginContent = '<table><thead><tr><th>Name</th><th></th></tr></thead><tbody>';
let htmlEndContent = '</tbody></table>';

function PopulateStores() {
    let storeDiv = document.getElementById("storeList");
    console.log(storeDiv);
    fetch("/api/stores")
        .then(res => res.json())
        .then(data => {
            console.log(data)
            //storeDiv.innerHTML = DisplayStores(data);
            DisplayStores(data, storeDiv);
        });

}

/*
function DisplayStores(stores) {
    let htmlContent = "";
    stores.forEach((store) => {
        htmlContent += DisplayStore(store);
    });
    return htmlBeginContent + htmlContent + htmlEndContent;
}
*/

function CreateStoreHtmlFromTemplate(template, store) {
    let newElem = template.cloneNode(true);
    let fakeDoc = new DocumentFragment();
    let selectedId;
    if (sessionStorage.selectedStore) {
        selectedId = JSON.parse(sessionStorage.selectedStore).storeId;
    }
    fakeDoc.appendChild(newElem);
    let name = fakeDoc.querySelector(".template-store-name");
    name.classList.remove("template-store-name");
    name.textContent = store.name;
    let button = fakeDoc.querySelector(".template-store-button");
    let buttonHTML = getButtonHTML(store, selectedId);
    button.classList.remove("template-store-button");
    button.innerHTML = buttonHTML;
    newElem.classList.remove("template-store");

    return newElem;
}

function DisplayStores(stores, container) {
    let templateStore = document.querySelector(".template-store");
    //let container = document.querySelector(".storeList")
    while (container.firstChild) {
        container.removeChild(container.firstChild);
    }
    stores.forEach(store => {
        let newStore = CreateStoreHtmlFromTemplate(templateStore, store);
        console.log(newStore);
        container.appendChild(newStore);
    });
}

function chooseStore(storeId) {
    console.log(`Selecting store ${storeId}`);
    fetch(`/api/stores/select/${storeId}`, { method: "POST" })
        .then(res => res.json())
        .then(data => {
            if (data.success) {
                sessionStorage.setItem("selectedStore", JSON.stringify(data.store));
                console.log(JSON.parse(sessionStorage.getItem("selectedStore")));
                window.location.href = "/html/home.html";
            } else {
                throw new Exception("Error selecting store");
            }
        }).catch(err => {
            console.log("chooseStoreError: ", err);
        });
}

function getButtonHTML(store, selectedId) {
    let disabledText = '';
    if (store.storeId == selectedId) {
        disabledText = 'disabled';
    }
    return `<button ${disabledText} onclick="chooseStore(${store.storeId})">Select</button>`;
}

function DisplayStore(store) {
    return `<tr><td>${store.name}</td><td>${getButtonHTML(store)}</td></tr>`;
}

document.addEventListener("DOMContentLoaded", function (event) {
    PopulateStores();

    let btn = document.querySelector('.add-store-btn');
    btn.addEventListener('click', function () {
        window.location.href = "/html/addStore.html";
    });
});