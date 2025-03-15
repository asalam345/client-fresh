using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;

namespace EntityModel.Models
{
    public class CustomerInfo
    {
        public string MobileNumber { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public bool IsRemember { get; set; }
        public bool IsLogedIn { get; set; }
        public string Result { get; set; }
        //public virtual TABLE_ORDER Order { get; set; }
        //public virtual List<TABLE_ORDER_DETAILS> OrderDetails { get; set; }
        public virtual List<ITEM_SELECT> SelectedItem { get; set; }

    }

   
}
