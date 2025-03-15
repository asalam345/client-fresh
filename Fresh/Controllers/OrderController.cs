using EntityModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
//using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fresh.Controllers
{
    public class OrderController : Controller
    {
        FreshDBEntities db = new FreshDBEntities();
        DateTime d = DateTime.Now;

        public void Check()
        {
            if (User.Identity == null && User.Identity.IsAuthenticated || Session["AdminId"] == null)
                Response.Redirect("~/Home");
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Orders()
        {
            List<TABLE_ORDER> orders = db.TABLE_ORDER.OrderByDescending(o => o.ORDER_ID).ToList();
            //List<TABLE_ORDER> todayOrders = db.TABLE_ORDER.Where(q => EntityFunctions.TruncateTime(q.OrderDate) == EntityFunctions.TruncateTime(d.Date)).OrderBy(o => o.Id).ToList();
            //List<TABLE_ORDER> notDeleverOrders = orders.Where(w => w.IsConfirm == false).OrderBy(o => o.Id).ToList();
           // List<TABLE_ORDER> MenualOrders = orders.Where(w => w.MenuallyOrder != null).OrderBy(o => o.Id).ToList();
            //List<TABLE_ORDER> nonMenualOrders = orders.Where(w => w.MenuallyOrder == null).OrderBy(o => o.Id).ToList();
            //ViewBag.TO = todayOrders;
            //ViewBag.NotDO = notDeleverOrders;
            //ViewBag.MO = MenualOrders;
            //ViewBag.NMO = nonMenualOrders;
            return View(orders);
        }

        [HttpPost]
        public JsonResult Delete(long id)
        {
            Check();
            TABLE_ORDER order = db.TABLE_ORDER.Find(id);
            if (order != null)
            {
                db.TABLE_ORDER.Remove(order);
                db.SaveChanges();
                return Json("Deleted", JsonRequestBehavior.AllowGet);
            }
            return Json("Delete Again!", JsonRequestBehavior.AllowGet);
        }

        //public string BookName(long id)
        //{
        //    tbBook book = db.tbBooks.Find(id);
        //    if (book != null)
        //        return book.Name;
        //    return "";
        //}

        [HttpPost]
        public JsonResult Confirm(long id, string date, string time, long deliveredBy)
        {
            Check();
            TABLE_ORDER order = db.TABLE_ORDER.Find(id);
            if (order != null)
            {
                //order.IsConfirm = true;
                //order.DeliverDate = Convert.ToDateTime(date).Date;
                //order.DeliverTime = time;
                //order.DeliverdBy = deliveredBy;
                //order.ConfirmBy = Convert.ToInt64(Session["AdminId"]);
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return Json("Deleted", JsonRequestBehavior.AllowGet);
            }
            return Json("Item not fouund!", JsonRequestBehavior.AllowGet);
        }

        public ActionResult t()
        {
            return View();
        }
    }
}
