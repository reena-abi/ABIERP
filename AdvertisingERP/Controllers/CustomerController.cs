using AdvertisingERP.Filter;
using AdvertisingERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvertisingERP.Controllers
{
    public class CustomerController : AdminBaseController
    {

        public ActionResult AddCustomer(string CustomerId)
        {
            Customer objcustomer = new Customer();
            if (TempData["CustomerError"] == null)
            {
                ViewBag.errormsg = "none";
            }
            if (CustomerId != null && CustomerId != "")
            {
               
                objcustomer.CompanyId =Crypto.Decrypt( CustomerId);
                 DataSet ds = objcustomer.GetAllCustomers();
                 if (ds != null && ds.Tables[0].Rows.Count > 0)
                 {
                     objcustomer.CompanyId =ds.Tables[0].Rows[0]["PK_CustomerID"].ToString();
                     objcustomer.CustomerCode = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                     objcustomer.CompanyName = ds.Tables[0].Rows[0]["NameForEdit"].ToString();
                     objcustomer.NatureOfBusiness = ds.Tables[0].Rows[0]["NatureOfBusiness"].ToString();
                     objcustomer.MobileNo = ds.Tables[0].Rows[0]["CompanyMobile"].ToString();
                     objcustomer.PhoneNo = ds.Tables[0].Rows[0]["CompanyPhone"].ToString();
                     objcustomer.DirectorName = ds.Tables[0].Rows[0]["NameOfCompanyDirector"].ToString();
                     objcustomer.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                     objcustomer.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                     objcustomer.Pincode = ds.Tables[0].Rows[0]["Pincode"].ToString();
                     objcustomer.StateName = ds.Tables[0].Rows[0]["State"].ToString();
                     objcustomer.City = ds.Tables[0].Rows[0]["City"].ToString();
                     objcustomer.ConcertPerson1 = ds.Tables[0].Rows[0]["ConcernPerson1"].ToString();
                     objcustomer.ConcertPersonContact1 = ds.Tables[0].Rows[0]["ConcernPerson1Contact"].ToString();
                     objcustomer.ConcertPerson2 = ds.Tables[0].Rows[0]["ConcernPerson2"].ToString();
                     objcustomer.ConcertPersonContact2 = ds.Tables[0].Rows[0]["ConcernPerson2Contact"].ToString();                     
                     objcustomer.DirectorContact = ds.Tables[0].Rows[0]["DirectorContact"].ToString();
                     objcustomer.BankName = ds.Tables[0].Rows[0]["BankName"].ToString();
                     objcustomer.AccountNo = ds.Tables[0].Rows[0]["BankAccountNumber"].ToString();
                     objcustomer.IFSCCode = ds.Tables[0].Rows[0]["IFSCCode"].ToString();
                     objcustomer.BankAddress = ds.Tables[0].Rows[0]["BankAddress"].ToString();
                     objcustomer.TANNO = ds.Tables[0].Rows[0]["TANNO"].ToString();
                     objcustomer.TINNO = ds.Tables[0].Rows[0]["TINNO"].ToString();
                     objcustomer.CINNO = ds.Tables[0].Rows[0]["CINNO"].ToString();
                     objcustomer.STAXNO = ds.Tables[0].Rows[0]["STAXNO"].ToString();
                     objcustomer.PANNO = ds.Tables[0].Rows[0]["PANNO"].ToString();
                     objcustomer.GSTIN = ds.Tables[0].Rows[0]["GSTIN"].ToString();
                     objcustomer.AdhaarNo = ds.Tables[0].Rows[0]["AdharNumber"].ToString();
                     objcustomer.IDType = ds.Tables[0].Rows[0]["IDType"].ToString();
                     objcustomer.IDNumber = ds.Tables[0].Rows[0]["IDNumber"].ToString();
                     objcustomer.IdImage = ds.Tables[0].Rows[0]["IDImagePath"].ToString();
                     objcustomer.PanImage = ds.Tables[0].Rows[0]["PanImage"].ToString();
                 }
            }
            return View(objcustomer);
        }

        [HttpPost]
        [ActionName("AddCustomer")]
        [OnAction(ButtonName = "SaveCustomer")]
        public ActionResult SaveCustomer(Customer obj, IEnumerable<HttpPostedFileBase> postedFile)
        {

            try
            {
                int count = 0;
                foreach (var file in postedFile)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        if (count == 0)
                        {
                            obj.IdImage = "../assets/SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                            file.SaveAs(Path.Combine(Server.MapPath(obj.IdImage)));
                        }
                        if (count == 1)
                        {
                            obj.PanImage = "../assets/SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                            file.SaveAs(Path.Combine(Server.MapPath(obj.PanImage)));
                        }

                    }
                    count++;
                }
                obj.AddedBy = Session["UserID"].ToString();

                DataSet ds = obj.CustomerRegistration();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        TempData["MsgClass"] = "text-success";
                        TempData["CustomerError"] = "Customer Registered Successfully.";
                    }
                    else
                    {
                        TempData["MsgClass"] = "text-danger";
                        TempData["CustomerError"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    TempData["MsgClass"] = "text-danger";
                    TempData["CustomerError"] = "Something went wrong.Please contact to technical support.";
                }
            }
            catch (Exception ex)
            {
                TempData["MsgClass"] = "text-danger";
                TempData["CustomerError"] = ex.Message;
            }
            return RedirectToAction("AddCustomer");
        }

        public ActionResult CustomerList()
        {
            Customer objcustomer = new Customer();
            if (TempData["CustomerDelete"] == null)
            {
                ViewBag.saverrormsg = "none";
            }

            List<Customer> lst = new List<Customer>();
            objcustomer.CompanyName = string.IsNullOrEmpty(objcustomer.CompanyName) ? null : objcustomer.CompanyName;
            objcustomer.CustomerCode = string.IsNullOrEmpty(objcustomer.CustomerCode) ? null : objcustomer.CustomerCode;
            DataSet ds = objcustomer.GetAllCustomers();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Customer obj = new Customer();
                    obj.CompanyId = Crypto.Encrypt(dr["PK_CustomerID"].ToString());
                    obj.CustomerCode = dr["CustomerCode"].ToString();
                    obj.CompanyName = dr["CompanyName"].ToString();
                    obj.NatureOfBusiness = dr["NatureOfBusiness"].ToString();
                    obj.MobileNo = dr["CompanyMobile"].ToString();
                    obj.PhoneNo = dr["CompanyPhone"].ToString();
                    obj.BankName = dr["BankName"].ToString();
                    obj.AccountNo = dr["BankAccountNumber"].ToString();
                    obj.IFSCCode = dr["IFSCCode"].ToString();
                    obj.BankAddress = dr["BankAddress"].ToString();
                    obj.Address = dr["Address"].ToString();
                    obj.Pincode = dr["Pincode"].ToString();
                    obj.StateName = dr["State"].ToString();
                    obj.City = dr["City"].ToString();
                    obj.Email = dr["Email"].ToString();
                    lst.Add(obj);
                }
                objcustomer.lstcustomer = lst;
            }
            return View(objcustomer);
        }
        [HttpPost]
        [ActionName("CustomerList")]
        [OnAction(ButtonName = "GetDetails")]
        public ActionResult GetCustomerList(Customer objcustomer)
        {
            if (TempData["CustomerDelete"] == null)
            {
                ViewBag.saverrormsg = "none";
            }


            List<Customer> lst = new List<Customer>();
            objcustomer.CompanyName = string.IsNullOrEmpty(objcustomer.CompanyName) ? null : objcustomer.CompanyName;
            objcustomer.CustomerCode = string.IsNullOrEmpty(objcustomer.CustomerCode)  ? null : objcustomer.CustomerCode;
            DataSet ds = objcustomer.GetAllCustomers();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Customer obj = new Customer();
                    obj.CompanyId = Crypto.Encrypt(dr["PK_CustomerID"].ToString());
                    obj.CustomerCode = dr["CustomerCode"].ToString();
                    obj.CompanyName = dr["CompanyName"].ToString();
                    obj.NatureOfBusiness = dr["NatureOfBusiness"].ToString();
                    obj.MobileNo = dr["CompanyMobile"].ToString();
                    obj.PhoneNo = dr["CompanyPhone"].ToString();
                    obj.BankName = dr["BankName"].ToString();
                    obj.AccountNo = dr["BankAccountNumber"].ToString();
                    obj.IFSCCode = dr["IFSCCode"].ToString();
                    obj.BankAddress = dr["BankAddress"].ToString();
                    obj.Address = dr["Address"].ToString();
                    obj.Pincode = dr["Pincode"].ToString();
                    obj.StateName = dr["State"].ToString();
                    obj.City = dr["City"].ToString();
                    obj.Email = dr["Email"].ToString();
                    lst.Add(obj);
                }
                objcustomer.lstcustomer = lst;
            }
            return View(objcustomer);
        }

        [HttpPost]
        [ActionName("AddCustomer")]
        [OnAction(ButtonName = "UpdateCustomer")]
        public ActionResult UpdateCustomer(Customer obj, IEnumerable<HttpPostedFileBase> postedFile)
        {
            try
            {
                int count = 0;
                foreach (var file in postedFile)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        if (count == 0)
                        {
                            obj.IdImage = "../assets/SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                            file.SaveAs(Path.Combine(Server.MapPath(obj.IdImage)));
                        }
                        if (count == 1)
                        {
                            obj.PanImage = "../assets/SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(file.FileName);
                            file.SaveAs(Path.Combine(Server.MapPath(obj.PanImage)));
                        }

                    }
                    count++;
                }

                obj.UpdatedBy = Session["UserID"].ToString();


                DataSet ds = obj.UpdateCustomer();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        TempData["MsgClass"] = "text-success";
                        TempData["CustomerError"] = "Customer Updated Successfully.";
                    }
                    else
                    {
                        TempData["MsgClass"] = "text-danger";
                        TempData["CustomerError"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                       
                    }
                }
                else
                {
                    TempData["MsgClass"] = "text-danger";
                    TempData["CustomerError"] = "Something went wrong.Please contact to technical support.";
                    
                }
            }
            catch (Exception ex)
            {
                TempData["MsgClass"] = "text-danger";
                TempData["CustomerError"] = ex.Message;
               
            }
            return View("CustomerList");
        }

        public ActionResult DeleteCustomer(string CustomerId)
        {
            Customer obj = new Customer();
            try
            {

                obj.DeletedBy = Session["UserID"].ToString();
                obj.CompanyId = Crypto.Decrypt(CustomerId);
                DataSet ds = new DataSet();


                ds = obj.DeleteCustomer();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {

                        TempData["CustomerError"] = "Customer Deleted Successfully";


                    }
                    else
                    {
                        TempData["CustomerError"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();


                    }
                }
            }
            catch (Exception ex)
            {
                TempData["CustomerError"] = ex.Message;

            }
            ViewBag.saverrormsg = "";
            return RedirectToAction("CustomerList");
        }
    }
}
