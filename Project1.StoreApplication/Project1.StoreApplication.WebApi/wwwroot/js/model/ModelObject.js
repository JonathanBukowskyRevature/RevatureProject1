
if (Models === undefined) {
    var Models = (function () {

        var moneyFormatter = new Intl.NumberFormat("en-US", { style: 'currency', currency: "USD" });

        // idea taken from stack overflow
        // https://stackoverflow.com/questions/18884249/checking-whether-something-is-iterable
        function isIterable(obj) {
            if (obj == null) {
                return false;
            }
            return typeof obj[Symbol.iterator] === 'function';
        }

        class ModelObject {
            _identifiers = [];

            /*
            constructor(data) {
                console.log("Base1", data);
                this.PopulateFromData(data);
                console.log("Base2", data);
            }
            */

            PopulateFromData(obj) {
                this._identifiers.forEach(prop => {
                    this[prop] = obj[prop];
                });
            }

            // populate in-place
    //let elms = this.PopulateElement(elm, this[prop], prop, className);
            PopulateElement(elm, data, propName, className, debug=false) {
                if (debug) console.log("PopuElm", elm);
                if (debug) console.log("data", data);
                if (debug) console.log("propName", propName);
                if (debug) console.log("className", className);
                if (isIterable(data) && typeof data !== 'string') {
                    if (debug) console.log("found a list of stuff");
                    for (let obj of data) {
                        // first, copy elm and add to container,
                        // then replace it using PopulateElement
                        let newElm = elm.cloneNode(true);
                        elm.parentElement.insertBefore(newElm, elm);
                        this.PopulateElement(newElm, obj, propName, className, debug);
                    }
                    elm.parentElement.removeChild(elm);
                } else if (data instanceof ModelObject) {
                    if (debug) console.log("Data? ", data);
                    let populated = data.CreateElementFromTemplate(elm, className);
                    elm.parentElement.replaceChild(populated, elm);
                } else if (typeof (data) == 'object') {
                    if (debug) console.log('object prop found', propName, data);
                } else {
                    if (debug) console.log("setting template data", elm, data);
                    elm.textContent = data;
                    elm.classList.remove(className);
                    elm.classList.add(className.replace('template', 'generated'));
                }
            }

            CreateElementFromTemplate(template, templatePrefix='template', debug=false) {
                if (debug) console.log("CreateElTemplate", template);
                let newElm = template.cloneNode(true);
                let templateClass = '';
                newElm.classList.forEach(className => {
                    if (className.includes(templatePrefix)) {
                        templateClass = className;
                    }
                });
                if (!templateClass) {
                    templateClass = templatePrefix;
                }
                if (!templateClass) {
                    throw new Error("Couldn't find template class");
                }
                if (debug) console.log("templateClass", templateClass);
                this._identifiers.forEach(prop => {
                    let className = `${templateClass}-${prop}`;
                    let elms = Array.from(newElm.getElementsByClassName(className));
                    if (!elms.length) {
                        if (debug) console.log(`Elm for ${className} not found`);
                    }
                    elms.forEach(elm => {
                        if (debug) console.log(`Found Elm for ${className}`, elm);
                        this.PopulateElement(elm, this[prop], prop, className, debug);
                    });
                });
                newElm.classList.remove(templateClass);
                let generatedClass = templateClass.replace('template', "generated");
                if (debug) console.log("Adding class to generated element", generatedClass);
                newElm.classList.add('generated');
                newElm.classList.add(generatedClass);
                return newElm;
            }
        }

        // If className provided, remove all elements from container with
        // className class. Otherwise, remove all generated elements
        function RemoveGeneratedElements(container, className='') {
            if (!className) {
                className = 'generated';
            }
            let elmsToRemove = Array.from(container.getElementsByClassName(className));
            elmsToRemove.forEach(elm => {
                container.removeChild(elm);
            });
        }

        // ************************ PRODUCT ************************************
        class Product extends ModelObject {
            _identifiers = [
                'productId',
                'name',
                'price',
                'description',
                'categoryID',
                'quantity',
                'subtotal',
                'formattedSubtotal',
                'formattedPrice'
            ];

            constructor(product) {
                super();
                this.PopulateFromData(product);
            }

            get formattedPrice() {
                return moneyFormatter.format(this.price);
            }

            set formattedPrice(value) {
                return;
            }

            get subtotal() {
                let quantity = (this.quantity) ? this.quantity : 0;
                let subtotal = this.price * quantity;
                return subtotal;
            }

            set subtotal(value) {
                return;
            }

            get formattedSubtotal() {
                return moneyFormatter.format(subtotal);
            }

            set formattedSubtotal(value) {
                return;
            }
        }

        // ************************ ORDER ************************************
        class Order extends ModelObject {
            _identifiers = ['customer', 'orderID', 'products', 'store', 'total', 'formattedTotal'];
            constructor(order) {
                super();
                this.PopulateFromData(order);
                this.products = this.products.map(p => new Product(p));
                this.customer = new Customer(this.customer);
                this.store = new Store(this.store);
            }

            get total() {
                let total = 0;
                this.products.forEach(prod => {
                    total += prod.subtotal;
                });
                return total;
            }

            set total(value) {
                return;
            }

            get formattedTotal() {
                return moneyFormatter.format(total);
            }

            set formattedTotal(value) {
                return;
            }
        }

        // ************************ ORDER ************************************
        class Store extends ModelObject {
            _identifiers = ['name', 'storeId', 'orders'];

            constructor(store) {
                super();
                this.PopulateFromData(store);
            }
        }

        // ************************ ORDER ************************************
        class Customer extends ModelObject {
            _identifiers = ['customerId', 'firstName', 'lastName', 'orders'];

            constructor(customer) {
                super();
                this.PopulateFromData(customer);
            }
        }


        // "export" list
        return {
            Product,
            Order,
            Customer,
            Store,
            RemoveGeneratedElements
        }
    })();
}
