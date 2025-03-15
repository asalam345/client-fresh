using EntityModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fresh.Controllers
{
	public class AddToCartController : Controller
	{
        FreshDBEntities db = new FreshDBEntities();
		CartList objCartList = new CartList();
		DateTime d = DateTime.Now;
		private NameValueCollection GenerateNameValueCollection(HttpCookie cookie, long productId, double quantity)
		{
			var collection = new NameValueCollection();
            //CartRow cRow = new CartRow();
            //cRow.Id = productId;
			//cRow.Name = name;
			//cRow.Pic = pic;
			//cRow.Quantity = quantity;
			//cRow.Selling = price;
			foreach (var value in cookie.Values)
			{
				//If the current element isn't the first empty element.
				if (value != null)
				{
					collection.Add(value.ToString(), cookie.Values[value.ToString()]);
				}
			}

			//Does this product exist in the cookie?
			if (cookie.Values[productId.ToString()] != null)
			{
				collection.Remove(productId.ToString());
				//Get current count of item in cart.
				int tmpAmount = Convert.ToInt32(cookie.Values[productId.ToString()]);
				double total = tmpAmount + quantity;
				collection.Add(productId.ToString(), total.ToString());
			}
			else //It doesn't exist, so add it.
			{
				collection.Add(productId.ToString(), quantity.ToString());
			}

			if (!collection.HasKeys())
				collection.Add(productId.ToString(), quantity.ToString());

			return collection;
		}

		public ActionResult Index()
		{
			if ((Session["Cart"] == null))
			{
				Session["Cart"] = objCartList;
			}
			else
			{
				objCartList = (CartList)(Session["cart"]);
				return View(objCartList);
			}
			return View(objCartList);
		}

		[HttpPost]
		public JsonResult CartUpdateOnRefresh()
		{
			if ((Session["Cart"] == null))
			{
				Session["Cart"] = objCartList;
			}
			else
			{
				objCartList = (CartList)(Session["cart"]);
				
			}
			return Json(new
			{
				Count = objCartList.list.Count,
				GrandTotla = objCartList.list.Select(s => s.PerSubTotla).Sum(),
				Model = objCartList.list,
				Data = objCartList
				//unReaded = notifUnread.Count,
				//read = notifRead,
				//unRead = notifUnread

			}, JsonRequestBehavior.AllowGet);
		}
		
		[HttpPost]
		public JsonResult GetCartProducts(List<string> bookIds, int quantity, bool refreshTime)
		{
			IEnumerable<BookCookie> ids = bookIds.GroupBy(a => a).Select(g => new BookCookie { Id = Convert.ToInt64(g.Key), Count = g.Count() });
			List<decimal> bookIdsOnly = ids.Select(s => s.Id).ToList();
			List<TABLE_ITEM> booksCookie = db.TABLE_ITEM.Where(w => bookIdsOnly.Contains(w.ITEM_ID)).ToList();
			string message = "";
			foreach (TABLE_ITEM b in booksCookie) {
				CartRow cartRow = new CartRow();
				cartRow.Id = b.ITEM_ID;
				//cartRow.Pic = b.Pic;
				cartRow.Name = b.ITEM_NAME;
				//cartRow.Selling = b.Selling;
                cartRow.Count = ids.Where(w => w.Id == b.ITEM_ID).Select(s => s.Count).FirstOrDefault();
				if(!refreshTime)
				cartRow.Count = cartRow.Count + quantity - 1;
                //if (b.RemainQuantity >= cartRow.Count)
                //{
                //    cartRow.Quantity = Convert.ToDouble(cartRow.Count);
                //}
                //else {
                //    cartRow.Quantity = Convert.ToDouble(b.RemainQuantity);
                //    message += cartRow.Count + " Quantity Is Not Available!" + Environment.NewLine;
                //}
                //cartRow.PerSubTotla = cartRow.Count * b.Selling;
				objCartList.list.Add(cartRow);
			}
			return Json(new
			{
				Count = objCartList.list.Count,
				GrandTotla = objCartList.list.Select(s => s.PerSubTotla).Sum(),
				Model = objCartList.list,
				Data = objCartList
				//unReaded = notifUnread.Count,
				//read = notifRead,
				//unRead = notifUnread

			}, JsonRequestBehavior.AllowGet);
		}
		[HttpPost]
		public JsonResult CartUpdate(long id, string name, double quantity, string pic, decimal price)
		{
			//If the cart cookie doesn't exist, create it.
			if (Request.Cookies["cart"] == null)
			{
				Response.Cookies.Add(new HttpCookie("cart"));
			}

			//Helper method here.
			var values = GenerateNameValueCollection(Request.Cookies["cart"], id, quantity);
			Response.Cookies["cart"].Values.Add(values);



			CartRow cart = new CartRow();
			cart.Id = id;
			cart.Name = name;
			cart.Quantity = quantity;
			cart.Pic = pic;
			cart.Selling = price;
			if ((Session["Cart"] == null))
			{
				Session["Cart"] = objCartList;
			}
			else
			{
				objCartList = (CartList)(Session["cart"]);
			}

			CartRow objCartrow = new CartRow();
			CartRow objTestCartRow = new CartRow();

			objCartrow.Id = cart.Id;

			TABLE_ITEM product = db.TABLE_ITEM.Find(cart.Id);

			if (product != null)
			{
				if (true)//(cart.Quantity > product.RemainQuantity)
				{
					ViewBag.Message = "WW";
					ViewBag.Result = cart.Quantity + " Quantity Is Not Available!";
					Session["Message"] = "WW";
					Session["Result"] = cart.Quantity + " Quantity Is Not Available!";
					Session["Object"] = objCartList.list;
					ViewBag.Model = Session["Object"];
					return Json(new
					{
						Count = objCartList.list.Count,
						GrandTotla = objCartrow.GrandTotla,
						Model = objCartList.list,
						Data = objCartList,
						Message = "W",
						Result = "Sorry " +  quantity + " Quantities Is Not Available!"
						//unReaded = notifUnread.Count,
						//read = notifRead,
						//unRead = notifUnread

					}, JsonRequestBehavior.AllowGet);
				}
				else
				{
					int listCount = objCartList.list.Count;
					for (int i = 0; i < listCount; i++)
					{
						objTestCartRow = (CartRow)objCartList.list[i];
						if (objTestCartRow.Id == cart.Id)
						{
							//int nItems = Int32.Parse(objTestCartRow.Quantity.ToString());
							//nItems += 1;
							//objTestCartRow.Count = nItems;
							objCartList.list.RemoveAt(i);
							//objCartrow = objTestCartRow;
							break;
						}
					}
					objCartrow.Name = product.ITEM_NAME;
					objCartrow.Quantity = cart.Quantity;

					string[] pics = null;
					objCartrow.Pic = "nusrahbd.png";
                    //if (product.Pic != null) pics = product.Pic.Split(',');
                    //if (pics != null && pics.Length > 0)
                    //{
                    //    objCartrow.Pic = pics[0];
                    //}
                    //objCartrow.Selling = product.SpecialPrice == null ? product.Selling : (decimal)product.SpecialPrice;
					objCartrow.PerSubTotla = Convert.ToDecimal(Convert.ToDouble(objCartrow.Selling) * cart.Quantity);
					objCartrow.SubTotla = objCartList.list.Select(s => s.PerSubTotla).Sum();
					objCartrow.GrandTotla = objCartrow.SubTotla + objCartrow.Tax;



					objCartList.list.Add(objCartrow);

					objCartrow.SubTotla = objCartList.list.Select(s => s.PerSubTotla).Sum();
					objCartrow.GrandTotla = objCartrow.SubTotla + objCartrow.Tax;

					listCount = objCartList.list.Count;

					Session["Cast"] = objCartrow.GrandTotla;


					Session["Items"] = listCount;

					ViewBag.Message = "O";
					ViewBag.Result = objCartrow.Name + " Add To List Successfully.";
					Session["Message"] = "O";
					Session["Result"] = objCartrow.Name + " Add To List Successfully.";
					Session["Object"] = objCartList.list;
					//return View(objCartList);
					ViewBag.Model = Session["Object"];
					return Json(new {
						Count = listCount,
						GrandTotla = objCartrow.GrandTotla,
						Model = objCartList.list,
						Data = objCartList,
						Message = "O",
						Result = objCartrow.Name + " Add To List Successfully."
						//unReaded = notifUnread.Count,
						//read = notifRead,
						//unRead = notifUnread
					
					}, JsonRequestBehavior.AllowGet);
				}
			}
			else
			{
				ViewBag.Message = "WW";
				ViewBag.Result = "Sorry This Product Is Not Available!";
				Session["Message"] = "WW";
				Session["Result"] = "Sorry This Product Is Not Available!";
				Session["Object"] = objCartList.list;
				ViewBag.Model = Session["Object"];
				return Json(new
				{
					Count = objCartList.list.Count,
					GrandTotla = objCartrow.GrandTotla,
					Model = objCartList.list,
					Data = objCartList,
					Message = "W",
					Result = "Sorry This Product Is Not Available!"
					//unReaded = notifUnread.Count,
					//read = notifRead,
					//unRead = notifUnread

				}, JsonRequestBehavior.AllowGet);
				//return View(objCartList);
			}


		}

		public JsonResult Delete(long id)
		{
			decimal grandTotal = 0;
			if ((Session["Cart"] != null))
			{
				objCartList = (CartList)(Session["cart"]);


				CartRow objTestCartRow = new CartRow();
				int listCount = objCartList.list.Count;
				for (int i = 0; i < listCount; i++)
				{
					objTestCartRow = (CartRow)objCartList.list[i];
					if (objTestCartRow.Id == id)
					{

						objCartList.list.RemoveAt(i);
						break;
					}
				}
				if (objCartList.list.Count > 0)
				{
					objCartList.list[objCartList.list.Count - 1].SubTotla = objCartList.list.Select(s => s.PerSubTotla).Sum();
					objCartList.list[objCartList.list.Count - 1].GrandTotla = objCartList.list.Select(s => s.PerSubTotla).Sum();
					Session["Cast"] = objCartList.list[objCartList.list.Count - 1].GrandTotla;
					grandTotal = objCartList.list[objCartList.list.Count - 1].GrandTotla;
				}
				else
				{
					Session["Cast"] = 0;
					grandTotal = 0;
				}

				Session["Items"] = listCount - 1;

				Session["Object"] = objCartList.list;
			}
			ViewBag.Model = Session["Object"];
			return Json(new
			{
				Count = objCartList.list.Count,
				GrandTotla = grandTotal,
				Model = objCartList.list,
				Data = objCartList
				//unReaded = notifUnread.Count,
				//read = notifRead,
				//unRead = notifUnread

			}, JsonRequestBehavior.AllowGet);

		}

		public JsonResult ClearAll()
		{
			if ((Session["Cart"] != null))
			{
				objCartList = (CartList)(Session["cart"]);

				objCartList.list.Clear();
			}
			Session["Cast"] = 0;
			Session["Items"] = 0;

			Session["Object"] = objCartList.list;
			ViewBag.Model = Session["Object"];
			return Json(new
			{
				Count = 0,
				GrandTotla = 0,
				Model = objCartList.list,
				Data = objCartList
			}, JsonRequestBehavior.AllowGet);
		}

		public JsonResult Order(string phone, bool isHomeDelivery)
		{
            //if (Session["cart"] != null && !String.IsNullOrEmpty(phone))
            //{
            //    objCartList = (CartList)(Session["cart"]);
            //    TABLE_ORDER order = new TABLE_ORDER();
            //    int listCount = objCartList.list.Count;
            //    order.BookIds = "";
            //    order.MenuallyOrder = null;
            //    order.OrderDate = d.Date;
            //    order.OrderTime = d.ToShortTimeString();
            //    order.Phone = phone;
            //    order.IsConfirm = false;
            //    order.IsHomeDelivery = isHomeDelivery;
            //    for (int i = 0; i < listCount; i++)
            //    {
            //        CartRow objTestCartRow = new CartRow();
            //        objTestCartRow = (CartRow)objCartList.list[i];
            //        if (i != listCount - 1)
            //            order.BookIds += objTestCartRow.Id + "=" + objTestCartRow.Quantity + ", ";
            //        else
            //            order.BookIds += objTestCartRow.Id.ToString() + "=" + objTestCartRow.Quantity;
            //    }
				
            //    db.TABLE_ORDER.Add(order);
            //    db.SaveChanges();
            //    ClearAll();

            //    return Json(new
            //{
            //    Count = 0,
            //    GrandTotla = 0,
            //    Model = objCartList.list,
            //    Data = objCartList,
            //    Result = "Al-Hamdulillah Successfully Order Compleated."
            //}, JsonRequestBehavior.AllowGet);
            //}

			return Json(
				new { Result = "Something goes wrong!" }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult MenualOrder(string phone, string name, string author, string publisher, int quantity)
		{
            //if (!String.IsNullOrEmpty(phone) && !String.IsNullOrEmpty(name))
            //{
            //    TABLE_ORDER order = new TABLE_ORDER();
            //    order.OrderDate = d.Date;
            //    order.OrderTime = d.ToShortTimeString();
            //    order.Phone = phone;
            //    order.IsConfirm = false;
            //    order.MenuallyOrder = name + "," + author + "," + publisher + "," + quantity;
            //    db.TABLE_ORDERs.Add(order);
            //    db.SaveChanges();
            //    ViewBag.Message = "O";
            //    ViewBag.Result = "Al-Hamdulillah order submited successfully.";
            //    return PartialView("~/Views/Shared/AjaxPartialView.cshtml", null);
            //}

			ViewBag.Message = "WW";
			ViewBag.Result = "There are some error please check your phone nuber and book name.";
			return PartialView("~/Views/Shared/AjaxPartialView.cshtml", null);
		}

	}
	public class CartRow
	{
		public decimal Id { set; get; }
		public string Pic { set; get; }
		public string Name { set; get; }
		public double Quantity { set; get; }
		public decimal Selling { set; get; }
		public decimal PerSubTotla { set; get; }
		public decimal SubTotla { set; get; }
		public decimal Tax { set; get; }
		public decimal GrandTotla { set; get; }
		public int Count { set; get; }
	}
	public class CartList
	{
		public List<CartRow> list = new List<CartRow>();
	}
	public class BookCookie
	{
		public decimal Id { set; get; }
		public int Count { set; get; }
	}
}
