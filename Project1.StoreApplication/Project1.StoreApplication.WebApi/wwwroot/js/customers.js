

function LoadCustomers() {
    let container = document.querySelector('.customer-list');
    let template = document.querySelector('.template-customer');
    fetch('/api/customers')
        .then(res => {
            return res.json();
        }).then(data => {
            Models.RemoveGeneratedElements(container, 'generated-customer');
            data.forEach(cust => {
                console.log(cust);
                let customer = new Models.Customer(cust);
                let custElm = customer.CreateElementFromTemplate(template);
                container.appendChild(custElm);
            });
        });
}

function FilterCustomers(filter) {
    filter = filter.toLowerCase();
    let customers = document.querySelectorAll('.generated-customer');
    for (let c of customers) {
        let fname = c.getElementsByClassName('generated-customer-firstName')[0].textContent.toLowerCase();
        let lname = c.getElementsByClassName('generated-customer-lastName')[0].textContent.toLowerCase();
        if (fname.includes(filter) || lname.includes(filter)) {
            c.classList.remove('hidden');
        } else {
            c.classList.add('hidden');
        }
    }
}

window.addEventListener('load', function () {
    LoadCustomers();

    let filterInput = document.querySelector("#input-filter-name");
    console.log("filter: ", filterInput);

    let addCustBtn = document.querySelector(".add-customer-button");
    addCustBtn.addEventListener('click', function () {
        window.location.href = "/html/addCustomer.html";
    });

    filterInput.addEventListener('input', function (e) {
        let filterText = e.target.value;
        FilterCustomers(filterText);
    });
});