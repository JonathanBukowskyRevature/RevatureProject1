

function login(e) {
    console.log("Logging in...");
    e.preventDefault();

    let form = document.querySelector(".login-form");
    let username = form.username.value;
    let password = form.password.value;
    let reqObj =
    {
        username,
        password
    }
    fetch("/api/login", {
        method: "POST",
        body: JSON.stringify(reqObj),
        headers: {
            'content-type': 'application/json'
        }
    }).then(res => {
        //console.log(res);
        if (!res.ok) {
            throw new Exception("Error logging in");
        }
        return res.json()
    }).then(data => {
        console.log(data);
        if (data.success) {
            sessionStorage.setItem("user", JSON.stringify(data.user));
            window.location.href = "/html/stores.html";
        } else {
            let statusDiv = document.querySelector(".status");
            statusDiv.innerHTML = '<p class="err-msg-login" style="background-color:red;"> Error logging in </p>';
            setTimeout(() => document.querySelector(".err-msg-login").remove(), 4000);
        }
        //console.log("custid: ", sessionStorage.getItem("customerId").customerId);
    }).catch(err => {
        // TODO: add error handling and display on webpage
    });
}

document.addEventListener("DOMContentLoaded", function (event) {
    document.getElementById("login_button")
        .addEventListener("click", login);
});
