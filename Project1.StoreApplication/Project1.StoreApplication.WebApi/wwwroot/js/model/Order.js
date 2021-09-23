
if (Order === undefined) {
    if (ModelObject === undefined) {
        throw new Error("Must include ModelObject");
    }
    var Order = (function () {
        class Order extends ModelObject.ModelObject {
            constructor(data) {
            }
        }

        return {
            Order
        }
    })();
}

