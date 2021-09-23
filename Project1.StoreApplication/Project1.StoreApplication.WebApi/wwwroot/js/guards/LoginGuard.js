
function RedirectToLogin() {
    alert("Please log in");
    window.location.href = "/index.html";
}

function ValidateLogin() {
    let userJSON = sessionStorage.user;
    console.log("validate login", userJSON);
    if (!userJSON) {
        console.log("1");
        return RedirectToLogin();
    }
    let user;
    try {
        user = JSON.parse(userJSON);
    } catch (e) {
        console.log("2", e);
        return RedirectToLogin();
    }
    if (!user) {
        console.log("3");
        return RedirectToLogin();
    }
    if (!user.customerId) {
        console.log("4");
        return RedirectToLogin();
    }
}

window.addEventListener('load', function () {
    ValidateLogin();
})
