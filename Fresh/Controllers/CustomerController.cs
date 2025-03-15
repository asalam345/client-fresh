using BLL;
using EntityModel;
using EntityModel.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fresh.Controllers
{
    public class CustomerController : Controller
    {
        FreshDBEntities db = new FreshDBEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Signin(CustomerInfo info)
        {
            info.IsLogedIn = false;
            info.Result = "";
            decimal newId = -1;
            PrimaryId pk = new PrimaryId();

            try
            {
                DateTime dt = DateTime.Now;
                TABLE_CUSTOMER customer = new TABLE_CUSTOMER();
                TABLE_CONTACT contact = db.TABLE_CONTACT.Where(w => w.CONTACT_INFO == info.MobileNumber).FirstOrDefault();
                if (contact == null)
                {
                    newId = pk.GetNewId("TABLE_CUSTOMER", "CUSTOMER_ID");
                    if (newId > -1)
                    {
                        customer = new TABLE_CUSTOMER();
                        customer.CUSTOMER_ID = newId;
                        customer.Password = info.Password;
                        customer.CREATION_DATE = dt.Date;
                        newId = pk.GetNewId("TABLE_CONTACT", "CONTACT_ID");
                        if (newId > -1)
                        {
                            contact = new TABLE_CONTACT();
                            contact.CONTACT_ID = newId;
                            contact.CONTACT_INFO = info.MobileNumber;
                            contact.CONTACT_TYPE_ID = 1;
                            contact.CONTACT_PERSON_ID = customer.CUSTOMER_ID;
                            customer.ISLOGEDIN = true;
                           
                            db.TABLE_CONTACT.Add(contact);
                            db.TABLE_CUSTOMER.Add(customer);
                            db.SaveChanges();
                            info.IsLogedIn = true;
                        }
                    }
                }
                else
                {
                    customer = db.TABLE_CUSTOMER.Where(w => w.CUSTOMER_ID == contact.CONTACT_PERSON_ID).FirstOrDefault();
                    if (customer != null && info.Password == customer.Password)
                    {
                        info.IsLogedIn = true;
                    }
                }

                if (info.IsLogedIn)
                {
                    newId = pk.GetNewId("LOGIN_INFO", "LOGIN_INFO_ID");
                    if (newId > -1)
                    {
                        LOGIN_INFO logInfo = new LOGIN_INFO();
                        logInfo.LOGIN_INFO_ID = newId;
                        logInfo.DATE = dt.Date;
                        logInfo.TIME = dt.TimeOfDay;
                        db.LOGIN_INFO.Add(logInfo);
                        db.SaveChanges();
                    }
                    info.SelectedItem = db.ITEM_SELECT.Where(w => w.CustomerID == customer.CUSTOMER_ID).ToList();
                }
            }
            catch (Exception ex)
            {
                info.IsLogedIn = false;
            }
            
            return Json(info, JsonRequestBehavior.AllowGet);
        }
	}
}