
if (Product === undefined) {
    if (ModelObject === undefined) {
        throw new Error("Error: Must include model object script");
    }
    var Product = (function () {
        class Product extends ModelObject.ModelObject {
            _identifiers = ['productId', 'name', 'price', 'description', 'categoryID', 'quantity'];

            constructor(product) {
                super();
                this.PopulateFromData(product);
            }
        }

        // "export" list
        return {
            Product
        }
    })();
}
