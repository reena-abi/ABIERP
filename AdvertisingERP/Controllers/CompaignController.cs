using AdvertisingERP.Filter;
using AdvertisingERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvertisingERP.Controllers
{
    public class CompaignController : AdminBaseController
    {

        public ActionResult GetAllCustomer()
        {
            Customer obj = new Customer();
            List<Customer> lst = new List<Customer>();
            DataSet ds = obj.GetAllCustomers();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Customer objcustomer = new Customer();
                    objcustomer.PK_CustomerID= (dr["PK_CustomerID"].ToString());
                    objcustomer.CustomerCode = (dr["CustomerCode"].ToString());
                    objcustomer.CompanyName = dr["CompanyName"].ToString();
                    lst.Add(objcustomer);
                }
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CampaignMaster(string CompaignId)
        {
            
            Compaign objCompaign = new Compaign();
            if (TempData["CompaignError"] == null)
            {
                ViewBag.errormsg = "none";
            }
            if (CompaignId != null && CompaignId != "")
            {
                objCompaign.CompaignId = Crypto.Decrypt(CompaignId);
                DataSet ds = objCompaign.GetCampaigns();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    objCompaign.CompaignId = ds.Tables[0].Rows[0]["PK_CamapignID"].ToString();
                    objCompaign.CampaignNo = ds.Tables[0].Rows[0]["CampaignNo"].ToString();
                    objCompaign.CustomerId = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                    objCompaign.CustomerName = ds.Tables[0].Rows[0]["CompanyName"].ToString() + '(' + ds.Tables[0].Rows[0]["CustomerCode"].ToString() + ')';
                    objCompaign.CreativeName = ds.Tables[0].Rows[0]["CreativeName"].ToString();
                    objCompaign.StartDate = ds.Tables[0].Rows[0]["StartDate"].ToString();
                    objCompaign.EndDate = ds.Tables[0].Rows[0]["EndDate"].ToString();
                    objCompaign.Decription = ds.Tables[0].Rows[0]["Description"].ToString();
                    objCompaign.SOMappedStatus = ds.Tables[1].Rows[0]["SOMappedStatus"].ToString();

                }
            }
            return View(objCompaign);
        }

        [HttpPost]
        [ActionName("CampaignMaster")]
        [OnAction(ButtonName = "SaveCompaign")]
        public ActionResult SaveCompaign(Compaign obj, HttpPostedFileBase postedFile)
        {

            try
            {
                obj.AddedBy = Session["UserID"].ToString();
                obj.StartDate = Common.ConvertToSystemDate(obj.StartDate, "dd/MM/yyyy");
                obj.EndDate = Common.ConvertToSystemDate(obj.EndDate, "dd/MM/yyyy");
                obj.CompaignId = Crypto.Decrypt(obj.CompaignId);
                //obj.CustomerId = Crypto.Decrypt(obj.CustomerId);

                DataSet ds = obj.CampaignEntry();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        TempData["CompaignError"] = "Compaign Added Successfully.";
                    }
                    else
                    {
                        TempData["CompaignError"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();

                    }
                }
                else
                {
                    TempData["CompaignError"] = "Something went wrong.Please contact to technical support.";
                }
            }
            catch (Exception ex)
            {
                TempData["CompaignError"] = ex.Message;
            }
            return RedirectToAction("CampaignMaster");
        }

        public ActionResult CompaignList()
        {
            Compaign objCompaign = new Compaign();
            if (TempData["CompaignDelete"] == null)
            {
                ViewBag.saverrormsg = "none";
            }
            List<Compaign> lst = new List<Compaign>();

            objCompaign.CampaignNo = string.IsNullOrEmpty(objCompaign.CampaignNo) ? null : objCompaign.CampaignNo;
            objCompaign.CustomerId = string.IsNullOrEmpty(objCompaign.CustomerId) ? null : objCompaign.CustomerId;
            DataSet ds = objCompaign.GetCampaigns();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Compaign obj = new Compaign();

                    obj.CompaignId = Crypto.Encrypt(dr["PK_CamapignID"].ToString());
                    obj.CampaignNo = dr["CampaignNo"].ToString();
                    obj.CustomerName = dr["CompanyName"].ToString();
                    obj.CreativeName = dr["CreativeName"].ToString();
                    obj.StartDate = dr["StartDate"].ToString();

                    obj.EndDate = dr["EndDate"].ToString();
                    obj.Decription = dr["Description"].ToString();



                    lst.Add(obj);
                }
                objCompaign.lstCompaign = lst;
            }
            return View(objCompaign);
        }
        [HttpPost]
        [ActionName("CompaignList")]
        [OnAction(ButtonName = "GetDetails")]
        public ActionResult GetCompaignList(Compaign objCompaign)
        {
            if (TempData["CompaignDelete"] == null)
            {
                ViewBag.saverrormsg = "none";
            }


            List<Compaign> lst = new List<Compaign>();
            
            objCompaign.CampaignNo = string.IsNullOrEmpty(objCompaign.CampaignNo) ? null : objCompaign.CampaignNo;
            objCompaign.CustomerId = string.IsNullOrEmpty(objCompaign.CustomerId) ? null : objCompaign.CustomerId;
            DataSet ds = objCompaign.GetCampaigns();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Compaign obj = new Compaign();

                    obj.CompaignId = Crypto.Encrypt(dr["PK_CamapignID"].ToString());
                    obj.CampaignNo =  dr["CampaignNo"].ToString();
                    obj.CustomerName = dr["CompanyName"].ToString();
                    obj.CreativeName =  dr["CreativeName"].ToString();
                    obj.StartDate =  dr["StartDate"].ToString();

                    obj.EndDate =  dr["EndDate"].ToString();
                    obj.Decription =  dr["Description"].ToString();


                  
                    lst.Add(obj);
                }
                objCompaign.lstCompaign = lst;
            }
            return View(objCompaign);
        }

        [HttpPost]
        [ActionName("CampaignMaster")]
        [OnAction(ButtonName = "UpdateCompaign")]
        public ActionResult UpdateCompaign(Compaign obj, HttpPostedFileBase postedFile)
        {
            try
            {
                obj.UpdatedBy = Session["UserID"].ToString();
                obj.CompaignId = Crypto.Decrypt(obj.CompaignId);
                obj.StartDate = Common.ConvertToSystemDate(obj.StartDate, "dd/MM/yyyy");
                obj.EndDate = Common.ConvertToSystemDate(obj.EndDate, "dd/MM/yyyy");

                DataSet ds = obj.UpdateCompaign();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        TempData["CompaignError"] = "Compaign Updated Successfully.";
                    }
                    else
                    {
                        TempData["CompaignError"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();


                    }
                }
                else
                {
                    
                    TempData["CompaignError"] = "Something went wrong.Please contact to technical support.";

                }
            }
            catch (Exception ex)
            {
                TempData["CompaignError"] = ex.Message;

            }
            return RedirectToAction("CampaignMaster");
        }
        //public ActionResult DeleteCompaign(string CompaignId)
        //{
        //    Compaign obj = new Compaign();
        //    try
        //    {

        //        obj.DeletedBy = Session["UserID"].ToString();
        //        obj.CompaignCode = Crypto.Decrypt(CompaignId);
        //        DataSet ds = new DataSet();


        //        ds = obj.DeleteCompaign();
        //        if (ds != null && ds.Tables.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows[0][0].ToString() == "1")
        //            {

        //                TempData["CompaignDelete"] = "Compaign Deleted Successfully";


        //            }
        //            else
        //            {
        //                TempData["CompaignDelete"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();


        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["CompaignDelete"] = ex.Message;

        //    }
        //    ViewBag.saverrormsg = "";
        //    return RedirectToAction("CompaignList");
        //}

    }
}
