namespace Project1.StoreApplication.Models
{
    /// <summary>
    /// A product sold at a Store that can be ordered by a Customer
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Unique identifier for the product.
        /// </summary>
        /// <value></value>
        public int ProductId { get; set; }

        /// <summary>
        /// The display name of the product
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// The sales price of the product
        /// </summary>
        /// <value></value>
        public decimal Price { get; set; }

        public string Description { get; set; }
        public int? CategoryID { get; set; }

        // TODO: this will let me attach quantities to products, but this might not be a good place to put this.
        public int Quantity { get; set; }

        public override int GetHashCode()
        {
            // Implementing GetHashCode to avoid compiler warnings.
            // I'm not sure I'm ever going to use this,
            // and I'm not sure this is appropriate functionality
            return ProductId;
        }
        public override bool Equals(object o)
        {
            if (o is Product)
            {
                return (this == (o as Product));
            }
            return false;
        }
        public static bool operator ==(Product a, Product b)
        {
            return a?.ProductId == b?.ProductId;
        }
        public static bool operator !=(Product a, Product b)
        {
            return a?.ProductId != b?.ProductId;
        }
        public override string ToString()
        {
            return $"{Name}: ${Price:f2}";
        }
    }
}