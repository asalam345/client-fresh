using EntityModel;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Data.Linq;
using System.Linq;

namespace Fresh.Controllers
{
    public class ProductsController : Controller
    {
        FreshDBEntities db = new FreshDBEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetProducts(long id = 0, string name = null, string category = null, string subCategory = null, string favorite = null)
        {
            var items = from item in db.TABLE_ITEM
                        join pic in db.ITEM_PICTURE on item.ITEM_ID equals pic.ITEM_ID
                        join pur in db.TABLE_PURCHASE_DETAILS on item.ITEM_ID equals pur.ITEM_ID
                        join dis in db.ITEM_DISCOUNT.DefaultIfEmpty() on item.ITEM_ID equals dis.ITEM_ID into disItem
                        from diss in disItem.DefaultIfEmpty()
                        //where post.ID == id
                        select new { Name = item.ITEM_NAME, Quantity = pur.REMAIN_QTY, Price = pur.SALES_PRICE, Image = pic.PICTURE_NAME, Id = pur.PURCHASE_DETAILS_ID,
                                     Discount = item.ITEM_ID == diss.ITEM_ID ? diss.AMOUNT : 0
                        };
            //List<TABLE_ITEM> I = db.TABLE_ITEM.Where(w => w.ITEM_ID == 1).ToList();
            //var i = I.Count();
            return Json(items, JsonRequestBehavior.AllowGet);
        }
	}
}