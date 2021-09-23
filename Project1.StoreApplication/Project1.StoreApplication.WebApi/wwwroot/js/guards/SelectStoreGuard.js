

function RedirectToStoreSelect() {
    alert("No store selected");
    window.location.href = "/html/stores.html";
}

function ValidateSelectedStore() {
    let storeJSON = sessionStorage.selectedStore;
    console.log("validate store");
    if (!storeJSON) {
        console.log("1");
        return RedirectToStoreSelect();
    }
    let store;
    try {
        store = JSON.parse(storeJSON);
    } catch {
        console.log("2");
        return RedirectToStoreSelect();
    }
    if (!store) {
        console.log("3");
        return RedirectToStoreSelect();
    }
    if (!store.storeId) {
        console.log("4");
        return RedirectToStoreSelect();
    }
}

window.addEventListener('load', function () {
    ValidateSelectedStore();
})

