//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class TABLE_ITEM
    {
        public decimal ITEM_ID { get; set; }
        public string ITEM_NAME { get; set; }
        public string ITEM_SHORT_CODE { get; set; }
        public string ITEM_BARCODE { get; set; }
        public Nullable<decimal> CATEGORY_ID { get; set; }
        public Nullable<decimal> UNIT_ID { get; set; }
    }
}
