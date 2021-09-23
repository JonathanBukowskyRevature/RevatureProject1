using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project1.StoreApplication.Models
{
    public class Customer : ModelObject
    {
        public Customer() : base()
        {
            Orders = new();
        }

        public int CustomerId { get; set; }

        //public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        /// <summary>
        /// Orders made by this ModelCustomer
        /// </summary>
        /// <value></value>
        public List<Order> Orders { get; set; }

        public override int GetHashCode()
        {
            return CustomerId;
        }

        public override bool Equals(object o)
        {
            if (o is Customer)
            {
                return (this == (o as Customer));
            }
            return false;
        }

        public static bool operator !=(Customer a, Customer b)
        {
            return a?.CustomerId != b?.CustomerId;
        }

        public static bool operator ==(Customer a, Customer b)
        {
            // NOTE: if the code is written correctly, this *should* be sufficient comparison
            //      it would be a great idea to compare other fields and such, but I'm not sure
            //  currently whether that would be either A) desired or B) necessary
            return a?.CustomerId == b?.CustomerId;
        }
    }
}
