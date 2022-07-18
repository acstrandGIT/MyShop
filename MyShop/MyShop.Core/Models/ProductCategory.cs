using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class ProductCategory
    {
        public string ProductCategoryId { get; set; }
        public string ProductCategoryName { get; set; }

        //constructor for this model. So that every time this model is used a new ProductCategory is created. 
        public ProductCategory()
        {
            this.ProductCategoryId = Guid.NewGuid().ToString();
        }

    }
}
