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
    public class SaleOrderController : AdminBaseController
    {
        DataTable dt = new DataTable();

        public ActionResult GetAllCampaigns(string customerCode)
        {
            Compaign obj = new Compaign();
            List<Compaign> lst = new List<Compaign>();
            obj.CustomerId = string.IsNullOrEmpty(customerCode) ? null : customerCode;
            DataSet ds = obj.GetCampaigns();
            if (ds != null && ds.Tables[2].Rows.Count > 0)
            {
                obj.Address = ds.Tables[2].Rows[0]["Address"].ToString();
            }
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Compaign objCampaign = new Compaign();
                    objCampaign.CompaignId = dr["PK_CamapignID"].ToString();
                    objCampaign.CampaignNo = dr["CampaignNo"].ToString();
                    objCampaign.CreativeName = dr["DisplayCreativeName"].ToString();

                    lst.Add(objCampaign);
                }
                obj.lstCompaign = lst;
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        #region GenerateSaleOrderNo
        public ActionResult GenerateSaleOrder()
        {
            if (TempData["GenerateOrder"] == null)
            {
                ViewBag.saverrormsg = "none";
            }
            SaleOrder obj = new SaleOrder();
            List<SaleOrder> lst = new List<SaleOrder>();

            DataSet ds = obj.SalesOrderNoList();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SaleOrder objsaleorder = new SaleOrder();

                    objsaleorder.SalesOrderNo = (dr["SalesOrderNo"].ToString());
                    objsaleorder.OrderDate = dr["OrderDate"].ToString();
                    objsaleorder.CustomerName = dr["CustomerName"].ToString();
                    objsaleorder.CustomerMobile = dr["CustomerMobile"].ToString();
                    objsaleorder.SaleOrderNoEncrypt = Crypto.Encrypt(dr["SalesOrderNo"].ToString());
                    objsaleorder.SaleOrderIDEncrypt = Crypto.Encrypt(dr["PK_SalesOrderNoID"].ToString());
                    objsaleorder.PostStatus = dr["PostStatus"].ToString();
                    objsaleorder.CustomerID= Crypto.Encrypt(dr["Pk_CustomerId"].ToString());
                    lst.Add(objsaleorder);
                }
                obj.lstsaleorder = lst;
            }
            return View(obj);
        }

        [HttpPost]
        [ActionName("GenerateSaleOrder")]
        [OnAction(ButtonName = "Generate")]
        public ActionResult CreateSaleOrder(SaleOrder obj)
        {
            obj.AddedBy = Session["UserID"].ToString();

            DataSet ds = obj.GenerateSalesOrderNo();
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    TempData["Class"] = "alert alert-success";
                    TempData["GenerateOrder"] = "Sale Order Generated Successfully.";
                }
                else
                {
                    TempData["Class"] = "alert alert-danger";
                    TempData["GenerateOrder"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            else
            {
                TempData["Class"] = "alert alert-danger";
                TempData["GenerateOrder"] = "Something went wrong.Please contact to technical support.";

            }
            return RedirectToAction("GenerateSaleOrder");
        }
        #endregion

        #region ddlChangeEvents
        public ActionResult BindAllCityList()
        {
            SaleOrder model = new SaleOrder();
            try
            {
                List<SelectListItem> ddlCity = new List<SelectListItem>();
                DataSet ds = model.GetCityList();
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ddlCity.Add(new SelectListItem { Text = r["City"].ToString(), Value = r["City"].ToString() });
                    }
                }
                model.Result = "1";
                model.lstVendors = ddlCity;
            }
            catch (Exception ex)
            {
                model.Result = "0";
                model.Result = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ServiceType(string serviceID)
        {
            SaleOrder model = new SaleOrder();
            try
            {
                model.ServiceID = serviceID;
                DataSet ds = model.GetServiceList();
                if (ds != null && ds.Tables.Count > 0)
                {
                    model.Result = "1";
                    model.ServiceType = ds.Tables[0].Rows[0]["DateFormat"].ToString();

                    //Bind Media Type according to the selected Service
                    List<SelectListItem> ddlmediaType = new List<SelectListItem>();
                    //DataSet dsMediaType = model.GetMediaType();

                    if (ds != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow r in ds.Tables[1].Rows)
                        {
                            ddlmediaType.Add(new SelectListItem { Text = r["MediaTypeName"].ToString(), Value = r["PK_MediaTypeID"].ToString() });
                        }
                    }
                    model.lstMediaType = ddlmediaType;
                    //Bind Media Type according to the Selected Service

                    //Bind Vendors according to the Selected Service
                    if (serviceID != "2" && serviceID != "6")
                    {
                        List<SelectListItem> ddlVendors = new List<SelectListItem>();
                        //DataSet dsVendors = model.GetAllVendors();
                        if (ds != null && ds.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow r in ds.Tables[2].Rows)
                            {
                                ddlVendors.Add(new SelectListItem { Text = r["Name"].ToString(), Value = r["PK_VendorID"].ToString() });
                            }
                        }
                        model.lstVendors = ddlVendors;
                    }
                    //Bind Vendors according to the Selected Service
                }
            }
            catch (Exception ex)
            {
                model.Result = "0";
                model.Result = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SiteChange(string siteID)
        {
            SaleOrder model = new SaleOrder();
            try
            {
                model.SiteID = siteID;
                DataSet ds = model.GetSiteDetails();
                if (ds != null && ds.Tables.Count > 0)
                {
                    model.Result = "1";
                    model.Side = ds.Tables[0].Rows[0]["Side"].ToString();
                    model.Height = ds.Tables[0].Rows[0]["Height"].ToString();
                    model.Width = ds.Tables[0].Rows[0]["Width"].ToString();
                    model.Area = ds.Tables[0].Rows[0]["Area"].ToString();
                    model.Unit = ds.Tables[0].Rows[0]["Quantity"].ToString();
                    model.Rate = ds.Tables[0].Rows[0]["CartRate"].ToString();
                    model.IsDummySite = ds.Tables[0].Rows[0]["IsDummySite"].ToString();
                    model.SiteName = ds.Tables[0].Rows[0]["SiteName"].ToString();
                    model.Facing = ds.Tables[0].Rows[0]["Facing"].ToString();
                    model.Rational = ds.Tables[0].Rows[0]["Rational"].ToString();
                    model.MediaType = ds.Tables[0].Rows[0]["MediaTypeName"].ToString();
                    model.MediaVehicle = ds.Tables[0].Rows[0]["MediaVehicleName"].ToString();
                    model.SiteImage = ds.Tables[0].Rows[0]["SiteImage"].ToString();
                    model.SiteInfo = ds.Tables[0].Rows[0]["SiteInfo"].ToString();
                    model.MediaTypeID = ds.Tables[0].Rows[0]["PK_MediaTypeID"].ToString();

                    //if (ds.Tables[1].Rows.Count > 0)
                    //{
                    //    model.VendorID = ds.Tables[1].Rows[0]["PK_VendorID"].ToString();
                    //    model.VendorName = ds.Tables[1].Rows[0]["Name"].ToString();
                    //}
                }
            }
            catch (Exception ex)
            {
                model.Result = "0";
                model.Result = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StateChange(string statename)
        {
            SaleOrder model = new SaleOrder();
            try
            {
                model.StateName = statename;
                List<SelectListItem> ddlCity = new List<SelectListItem>();
                DataSet ds = model.GetCityByState();
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ddlCity.Add(new SelectListItem { Text = r["Districtname"].ToString(), Value = r["Districtname"].ToString() });
                    }
                }
                model.Result = "1";
                model.lstVendors = ddlCity;
            }
            catch (Exception ex)
            {
                model.Result = "0";
                model.Result = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CityChange(string cityID, string serviceID)
        {
            SaleOrder model = new SaleOrder();
            try
            {
                model.City = cityID;
                model.ServiceID = serviceID;

                List<SelectListItem> ddlVendors = new List<SelectListItem>();
                DataSet ds = model.GetAllVendors();
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ddlVendors.Add(new SelectListItem { Text = r["Name"].ToString(), Value = r["PK_VendorID"].ToString() });
                    }
                }
                model.Result = "1";
                model.lstVendors = ddlVendors;
            }
            catch (Exception ex)
            {
                model.Result = "0";
                model.Result = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VendorChange(string vendorID, string city, string serviceID)
        {
            SaleOrder model = new SaleOrder();
            try
            {
                if (serviceID == "2" || serviceID == "6")
                {
                    model.VendorID = vendorID;
                    model.City = city;
                }
                else
                {
                    model.VendorID = null;
                    model.City = city;
                }
                List<SelectListItem> ddlSites = new List<SelectListItem>();
                DataSet ds = model.GetSiteList();
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ddlSites.Add(new SelectListItem { Text = r["SiteInfo"].ToString(), Value = r["PK_SiteID"].ToString() });
                    }
                }
                model.Result = "1";
                model.ddlSites = ddlSites;
            }
            catch (Exception ex)
            {
                model.Result = "0";
                model.Result = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ServiceChangeForMediaType(string serviceType)
        {
            SaleOrder model = new SaleOrder();
            try
            {
                model.ServiceTypeNameSO = serviceType;
                List<SelectListItem> ddlmediaType = new List<SelectListItem>();
                DataSet ds = model.GetMediaType();

                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        ddlmediaType.Add(new SelectListItem { Text = r["MediaTypeName"].ToString(), Value = r["PK_MediaTypeID"].ToString() });
                    }
                }
                model.Result = "1";
                model.lstMediaType = ddlmediaType;
            }
            catch (Exception ex)
            {
                model.Result = "0";
                model.Result = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion

        //Old Sale Order Details View
        public ActionResult GetSaleOrderDetails(string SaleOrderId, string no)
        {
            Session["tmpLines"] = null;
            SaleOrder model = new SaleOrder();
            model.SalesOrderNo = Crypto.Decrypt(no);
            model.SalesOrderNoID = Crypto.Decrypt(SaleOrderId);
            model.OrderDate = DateTime.Now.ToString("dd/MM/yyyy");

            #region Sites
            int count1 = 0;
            List<SelectListItem> ddlSites = new List<SelectListItem>();
            DataSet ds2 = model.GetSiteList();
            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds2.Tables[0].Rows)
                {
                    if (count1 == 0)
                    {
                        ddlSites.Add(new SelectListItem { Text = "Select Site", Value = "0" });
                    }
                    ddlSites.Add(new SelectListItem { Text = r["SiteName"].ToString(), Value = r["PK_SiteID"].ToString() });
                    count1 = count1 + 1;
                }
            }

            ViewBag.ddlSites = ddlSites;

            #endregion
            #region Services
            int count2 = 0;
            List<SelectListItem> ddlServices = new List<SelectListItem>();
            DataSet ds3 = model.GetServiceList();
            if (ds3 != null && ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds3.Tables[0].Rows)
                {
                    if (count1 == 0)
                    {
                        ddlServices.Add(new SelectListItem { Text = "Select Service", Value = "0" });
                    }
                    ddlServices.Add(new SelectListItem { Text = r["ServiceName"].ToString(), Value = r["PK_ServiceId"].ToString() });
                    count2 = count2 + 1;
                }
            }

            ViewBag.ddlServices = ddlServices;

            #endregion
            #region defaults

            List<SelectListItem> ddlPaymentTerms = Common.BindPaymentTerms();
            ViewBag.ddlPaymentTerms = ddlPaymentTerms;

            List<SelectListItem> ddlBillingSnaps = Common.BillingSnaps();
            ViewBag.ddlBillingSnaps = ddlBillingSnaps;

            List<SelectListItem> ddlPOReceived = Common.POReceived();
            ViewBag.ddlPOReceived = ddlPOReceived;

            int count5 = 0;
            List<SelectListItem> ddlVendors = new List<SelectListItem>();
            DataSet dsVendors = model.GetAllVendors();
            if (dsVendors != null && dsVendors.Tables.Count > 0 && dsVendors.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsVendors.Tables[0].Rows)
                {
                    if (count5 == 0)
                    {
                        ddlVendors.Add(new SelectListItem { Text = "Select Vendor", Value = "0" });
                    }
                    ddlVendors.Add(new SelectListItem { Text = r["Name"].ToString(), Value = r["PK_VendorID"].ToString() });
                    count5 = count5 + 1;
                }
            }
            ViewBag.ddlVendors = ddlVendors;

            int count3 = 0;
            List<SelectListItem> ddlSalesPerson = new List<SelectListItem>();
            DataSet dsSalesPerson = model.GetSalesPersonList();
            if (dsSalesPerson != null && dsSalesPerson.Tables.Count > 0 && dsSalesPerson.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsSalesPerson.Tables[0].Rows)
                {
                    if (count3 == 0)
                    {
                        ddlSalesPerson.Add(new SelectListItem { Text = "Select Sales Person", Value = "0" });
                    }
                    ddlSalesPerson.Add(new SelectListItem { Text = r["SalesPersonName"].ToString(), Value = r["PK_SalesPersonID"].ToString() });
                    count3 = count3 + 1;
                }
            }
            ViewBag.ddlSalesPerson = ddlSalesPerson;

            int count4 = 0;
            List<SelectListItem> ddlOperationExecutive = new List<SelectListItem>();
            DataSet dsOperationExecutive = model.GetOperationExecutiveList();
            if (dsOperationExecutive != null && dsOperationExecutive.Tables.Count > 0 && dsOperationExecutive.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsOperationExecutive.Tables[0].Rows)
                {
                    if (count4 == 0)
                    {
                        ddlOperationExecutive.Add(new SelectListItem { Text = "Select Operation Executive", Value = "0" });
                    }
                    ddlOperationExecutive.Add(new SelectListItem { Text = r["OperationExecutivePersonName"].ToString(), Value = r["PK_OperationExecutiveID"].ToString() });
                    count4 = count4 + 1;
                }
            }
            ViewBag.ddlOperationExecutive = ddlOperationExecutive;

            #endregion

            DataSet ds = model.GetSaleOrderDetails();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {

                model.PK_SalesOrderID = ds.Tables[0].Rows[0]["PK_SalesOrderID"].ToString();
                model.SalesOrderNoID = ds.Tables[0].Rows[0]["PK_SalesOrderID"].ToString();
                model.SalesOrderNo = ds.Tables[0].Rows[0]["SalesOrderNo"].ToString();
                model.OrderDate = ds.Tables[0].Rows[0]["SalesOrderDate"].ToString();
                model.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                model.CustomerCode = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                model.CampaignID = ds.Tables[0].Rows[0]["PK_CamapignID"].ToString();
                model.CampaignNumber = ds.Tables[0].Rows[0]["CampaignNo"].ToString();
                model.POReceived = ds.Tables[0].Rows[0]["IsPOReceived"].ToString();
                model.PONumber = ds.Tables[0].Rows[0]["PONumber"].ToString();
                model.PaymentTermsID = ds.Tables[0].Rows[0]["FK_PaymentTermsID"].ToString();
                model.SalesPersonID = ds.Tables[0].Rows[0]["FK_SalesPersonID"].ToString();
                model.OperationExecutiveID = ds.Tables[0].Rows[0]["FK_OperationExecutiveID"].ToString();
                model.BillingSnapsID = ds.Tables[0].Rows[0]["FK_BillingSnapsID"].ToString();
                model.POImagePath = ds.Tables[0].Rows[0]["POImagePath"].ToString();

                //Sale Order Details 
                Session["tmpLinesOld"] = ds.Tables[1];
                model.isEditable = "1";
            }
            return View(model);
        }
        //Old Sale Order Details View

        #region newSaleOrder

        public ActionResult SaleOrderDetails(string SaleOrderId, string no)
        {
            try
            {
                SaleOrder model = new SaleOrder();
                model.SalesOrderNo = Crypto.Decrypt(no);
                model.SalesOrderNoID = Crypto.Decrypt(SaleOrderId);
                model.OrderDate = DateTime.Now.ToString("dd/MM/yyyy");

                model.SaleOrderIDEncrypt = SaleOrderId;
                model.SaleOrderNoEncrypt = no;

                #region BindCityList
                int ctrCity = 0;
                List<SelectListItem> ddlCity = new List<SelectListItem>();
                DataSet dsCity = model.GetCityList();
                if (dsCity != null && dsCity.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dsCity.Tables[0].Rows)
                    {
                        if (ctrCity == 0)
                        {
                            ddlCity.Add(new SelectListItem { Text = "Select City", Value = "0" });
                        }
                        ddlCity.Add(new SelectListItem { Text = r["City"].ToString(), Value = r["City"].ToString() });
                        ctrCity = ctrCity + 1;
                    }
                }
                ViewBag.ddlCity = ddlCity;
                #endregion
                #region BindStateList
                int ctrState = 0;
                List<SelectListItem> ddlState = new List<SelectListItem>();
                DataSet dsState = model.GetStateList();
                if (dsState != null && dsState.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dsState.Tables[0].Rows)
                    {
                        if (ctrState == 0)
                        {
                            ddlState.Add(new SelectListItem { Text = "Select State", Value = "0" });
                        }
                        ddlState.Add(new SelectListItem { Text = r["statename"].ToString(), Value = r["statename"].ToString() });
                        ctrState = ctrState + 1;
                    }
                }
                ViewBag.ddlState = ddlState;
                #endregion
                #region Sites
                List<SelectListItem> ddlSites = new List<SelectListItem>();
                ddlSites.Add(new SelectListItem { Text = "Select Site", Value = "0" });
                ViewBag.ddlSites = ddlSites;
                #endregion
                #region Services
                int count2 = 0;
                List<SelectListItem> ddlServices = new List<SelectListItem>();
                DataSet ds3 = model.GetServiceList();
                if (ds3 != null && ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in ds3.Tables[0].Rows)
                    {
                        if (count2 == 0)
                        {
                            ddlServices.Add(new SelectListItem { Text = "Select Service", Value = "0" });
                        }
                        ddlServices.Add(new SelectListItem { Text = r["ServiceName"].ToString(), Value = r["PK_ServiceId"].ToString() });
                        count2 = count2 + 1;
                    }
                }

                ViewBag.ddlServices = ddlServices;

                #endregion
                #region defaults

                List<SelectListItem> ddlPaymentTerms = Common.BindPaymentTerms();
                ViewBag.ddlPaymentTerms = ddlPaymentTerms;

                List<SelectListItem> ddlBillingSnaps = Common.BillingSnaps();
                ViewBag.ddlBillingSnaps = ddlBillingSnaps;

                List<SelectListItem> ddlPOReceived = Common.POReceived();
                ViewBag.ddlPOReceived = ddlPOReceived;

                List<SelectListItem> ddlMediaType = new List<SelectListItem>();
                ddlMediaType.Add(new SelectListItem { Text = "Select Media Type", Value = "0" });
                ViewBag.ddlMediaType = ddlMediaType;

                List<SelectListItem> ddlVendors = new List<SelectListItem>();
                ddlVendors.Add(new SelectListItem { Text = "Select Vendor", Value = "0" });
                ViewBag.ddlVendors = ddlVendors;

                int count3 = 0;
                List<SelectListItem> ddlSalesPerson = new List<SelectListItem>();
                DataSet dsSalesPerson = model.GetSalesPersonList();
                if (dsSalesPerson != null && dsSalesPerson.Tables.Count > 0 && dsSalesPerson.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dsSalesPerson.Tables[0].Rows)
                    {
                        if (count3 == 0)
                        {
                            ddlSalesPerson.Add(new SelectListItem { Text = "Select Sales Person", Value = "0" });
                        }
                        ddlSalesPerson.Add(new SelectListItem { Text = r["SalesPersonName"].ToString(), Value = r["PK_SalesPersonID"].ToString() });
                        count3 = count3 + 1;
                    }
                }
                ViewBag.ddlSalesPerson = ddlSalesPerson;

                int count4 = 0;
                List<SelectListItem> ddlOperationExecutive = new List<SelectListItem>();
                DataSet dsOperationExecutive = model.GetOperationExecutiveList();
                if (dsOperationExecutive != null && dsOperationExecutive.Tables.Count > 0 && dsOperationExecutive.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dsOperationExecutive.Tables[0].Rows)
                    {
                        if (count4 == 0)
                        {
                            ddlOperationExecutive.Add(new SelectListItem { Text = "Select Operation Executive", Value = "0" });
                        }
                        ddlOperationExecutive.Add(new SelectListItem { Text = r["OperationExecutivePersonName"].ToString(), Value = r["PK_OperationExecutiveID"].ToString() });
                        count4 = count4 + 1;
                    }
                }
                ViewBag.ddlOperationExecutive = ddlOperationExecutive;

                #endregion

                DataSet ds = model.GetSaleOrderDetails();
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        model.PK_SalesOrderID = ds.Tables[0].Rows[0]["PK_SalesOrderID"].ToString();
                        model.SalesOrderNoID = ds.Tables[0].Rows[0]["PK_SalesOrderNoID"].ToString();
                        model.SalesOrderNo = ds.Tables[0].Rows[0]["SalesOrderNo"].ToString();
                        model.OrderDate = ds.Tables[0].Rows[0]["SalesOrderDate"].ToString();
                        model.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                        model.CustomerCode = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                        model.CampaignID = ds.Tables[0].Rows[0]["CampaignNo"].ToString();
                        model.CampaignNumber = ds.Tables[0].Rows[0]["CampaignName"].ToString() + " (" + ds.Tables[0].Rows[0]["CampaignNo"].ToString() + ")";
                        model.POReceived = ds.Tables[0].Rows[0]["IsPOReceived"].ToString();
                        model.PONumber = ds.Tables[0].Rows[0]["PONumber"].ToString();
                        model.PaymentTermsID = ds.Tables[0].Rows[0]["FK_PaymentTermsID"].ToString();
                        model.SalesPersonID = ds.Tables[0].Rows[0]["FK_SalesPersonID"].ToString();
                        model.OperationExecutiveID = ds.Tables[0].Rows[0]["FK_OperationExecutiveID"].ToString();
                        model.BillingSnapsID = ds.Tables[0].Rows[0]["FK_BillingSnapsID"].ToString();
                        model.POImagePath = ds.Tables[0].Rows[0]["POImagePath"].ToString();
                        model.isPOGenerated = ds.Tables[0].Rows[0]["IsPOGenerated"].ToString();
                        model.SOStatus = ds.Tables[0].Rows[0]["Status"].ToString();
                        model.PostStatus = ds.Tables[0].Rows[0]["PostStatus"].ToString();
                        model.StateName = ds.Tables[0].Rows[0]["StateName"].ToString();
                        model.CustomerAddress = ds.Tables[0].Rows[0]["CustomerAddress"].ToString();
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        List<SaleOrder> lstLines = new List<SaleOrder>();
                        foreach (DataRow r in ds.Tables[1].Rows)
                        {
                            SaleOrder obj = new SaleOrder();
                            //ServiceID ki value set kr rahe hain..jab second time extra Lines add hongi to Service na change ho sake
                            model.ServiceID = r["FK_ServiceID"].ToString();
                            //ServiceID ki value set kr rahe hain..jab second time extra Lines add hongi to Service na change ho sake
                            obj.SaleOrderDetailsID = r["PK_SalesOrderDetailsID"].ToString();
                            obj.City = r["City"].ToString();
                            obj.SiteID = r["FK_SiteID"].ToString();
                            obj.SiteName = r["SiteName"].ToString();
                            obj.VendorID = r["FK_VendorID"].ToString();
                            obj.VendorName = r["VendorName"].ToString();
                            obj.MediaTypeID = r["FK_MediaTypeID"].ToString();
                            obj.MediaType = r["MediaTypeName"].ToString();
                            obj.Side = r["Side"].ToString();
                            obj.Height = r["Height"].ToString();
                            obj.Width = r["Width"].ToString();
                            obj.Area = r["Area"].ToString();
                            obj.Quantity = r["Quantity"].ToString();
                            obj.Unit = r["Quantity"].ToString();
                            obj.Rate = r["Rate"].ToString();
                            obj.ServiceID = r["FK_ServiceID"].ToString();
                            obj.ServiceName = r["ServiceName"].ToString();
                            obj.FromDate = r["FromDate"].ToString();
                            obj.ToDate = r["ToDate"].ToString();
                            obj.TotalAmount = r["TotalAmount"].ToString();
                            obj.CGST = r["CGST"].ToString();
                            obj.SGST = r["SGST"].ToString();
                            obj.IGST = r["IGST"].ToString();
                            obj.TDS = r["TDS"].ToString();
                            obj.Discount = r["Discount"].ToString();
                            obj.FinalAmount = r["FinalAmount"].ToString();
                            obj.DescriptionDisplay = r["Description"].ToString();
                            obj.LineStatus = r["Status"].ToString();
                            obj.CssClass = r["Class"].ToString();
                            obj.isPOGenerated = r["IsPOGenerated"].ToString();
                            obj.PONumber = r["PONumber"].ToString();
                            obj.HSNCode = r["HSNCode"].ToString();

                            ViewBag.SUM = Convert.ToDecimal(ViewBag.SUM) + Convert.ToDecimal(r["FinalAmount"].ToString());

                            lstLines.Add(obj);
                        }
                        model.lstsaleorder = lstLines;
                        //Bind Media Type according to Selected Service
                        DataSet dsMediaType = model.GetServiceList();
                        if (dsMediaType != null && dsMediaType.Tables.Count > 0)
                        {
                            model.Result = "1";
                            model.ServiceType = dsMediaType.Tables[0].Rows[0]["DateFormat"].ToString();

                            //Bind Media Type according to the selected Service
                            List<SelectListItem> ddlmediaType = new List<SelectListItem>();
                            //DataSet dsMediaType = model.GetMediaType();

                            int ctrMediaType = 0;
                            if (dsMediaType != null && dsMediaType.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow r in dsMediaType.Tables[1].Rows)
                                {
                                    if (ctrMediaType == 0)
                                    {
                                        ddlmediaType.Add(new SelectListItem { Text = "Select Media Type", Value = "0" });
                                    }
                                    ddlmediaType.Add(new SelectListItem { Text = r["MediaTypeName"].ToString(), Value = r["PK_MediaTypeID"].ToString() });
                                    ctrMediaType = ctrMediaType + 1;
                                }
                            }
                            ViewBag.ddlMediaType = ddlmediaType;
                            //Bind Media Type according to selected Service

                            //Bind Vendors according to the Selected Service
                            if (model.ServiceID != "2" && model.ServiceID != "6")
                            {
                                if (dsMediaType != null && dsMediaType.Tables[2].Rows.Count > 0)
                                {
                                    //List<SelectListItem> ddlVendors = new List<SelectListItem>();
                                    //DataSet dsVendors = model.GetAllVendors();
                                    if (dsMediaType != null && dsMediaType.Tables[2].Rows.Count > 0)
                                    {
                                        foreach (DataRow r in dsMediaType.Tables[2].Rows)
                                        {
                                            ddlVendors.Add(new SelectListItem { Text = r["Name"].ToString(), Value = r["PK_VendorID"].ToString() });
                                        }
                                    }
                                    ViewBag.ddlVendors = ddlVendors;
                                }
                            }
                            //Bind Vendors according to the Selected Service
                        }
                    }
                    return View(model);
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpPost]
        [ActionName("SaleOrderDetails")]
        [OnAction(ButtonName = "btnAddLine")]
        public ActionResult SaveDataTemporary(HttpPostedFileBase postedFile, SaleOrder model)
        {
            try
            {
                if (Request["hdServiceID"] != null)
                {
                    model.ServiceID = Request["hdServiceID"];
                }
                model.OrderDate = string.IsNullOrEmpty(model.OrderDate) ? null : Common.ConvertToSystemDate(model.OrderDate, "dd/MM/yyyy");
                model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
                model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
                model.AddedBy = Session["UserID"].ToString();

                DataSet ds = model.SaveLineTemporary();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                       
                        ds = model.GetSaleOrderDetails();
                        if (ds != null)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                model.PK_SalesOrderID = ds.Tables[0].Rows[0]["PK_SalesOrderID"].ToString();
                                model.SalesOrderNoID = ds.Tables[0].Rows[0]["PK_SalesOrderID"].ToString();
                                model.SalesOrderNo = ds.Tables[0].Rows[0]["SalesOrderNo"].ToString();
                                model.OrderDate = ds.Tables[0].Rows[0]["SalesOrderDate"].ToString();
                                model.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                                model.CustomerCode = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                                model.CampaignID = ds.Tables[0].Rows[0]["PK_CamapignID"].ToString();
                                model.CampaignNumber = ds.Tables[0].Rows[0]["CampaignNo"].ToString();
                                model.POReceived = ds.Tables[0].Rows[0]["IsPOReceived"].ToString();
                                model.PONumber = ds.Tables[0].Rows[0]["PONumber"].ToString();
                                model.PaymentTermsID = ds.Tables[0].Rows[0]["FK_PaymentTermsID"].ToString();
                                model.SalesPersonID = ds.Tables[0].Rows[0]["FK_SalesPersonID"].ToString();
                                model.OperationExecutiveID = ds.Tables[0].Rows[0]["FK_OperationExecutiveID"].ToString();
                                model.BillingSnapsID = ds.Tables[0].Rows[0]["FK_BillingSnapsID"].ToString();
                                model.POImagePath = ds.Tables[0].Rows[0]["POImagePath"].ToString();
                                model.isPOGenerated = ds.Tables[0].Rows[0]["IsPOGenerated"].ToString();
                                model.SOStatus = ds.Tables[0].Rows[0]["Status"].ToString();
                                model.PostStatus = ds.Tables[0].Rows[0]["PostStatus"].ToString();
                                model.StateName = ds.Tables[0].Rows[0]["StateName"].ToString();
                            }
                            //Save Uploaded file for the First Time & Store its Path
                            if (ds.Tables[1].Rows.Count == 1)
                            {
                                if (postedFile != null)
                                {
                                    model.POImagePath = "../SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(postedFile.FileName);
                                    postedFile.SaveAs(Path.Combine(Server.MapPath(model.POImagePath)));
                                }
                            }
                            //Save Uploaded file for the First Time & Store its Path
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                List<SaleOrder> lstLines = new List<SaleOrder>();
                                foreach (DataRow r in ds.Tables[1].Rows)
                                {
                                    SaleOrder obj = new SaleOrder();
                                    obj.SaleOrderDetailsID = r["PK_SalesOrderDetailsID"].ToString();
                                    obj.City = r["City"].ToString();
                                    obj.SiteID = r["FK_SiteID"].ToString();
                                    obj.SiteName = r["SiteName"].ToString();
                                    obj.VendorID = r["FK_VendorID"].ToString();
                                    obj.VendorName = r["VendorName"].ToString();
                                    obj.MediaTypeID = r["FK_MediaTypeID"].ToString();
                                    obj.MediaType = r["MediaTypeName"].ToString();
                                    obj.Side = r["Side"].ToString();
                                    obj.Height = r["Height"].ToString();
                                    obj.Width = r["Width"].ToString();
                                    obj.Area = r["Area"].ToString();
                                    obj.Quantity = r["Quantity"].ToString();
                                    obj.Unit = r["Quantity"].ToString();
                                    obj.Rate = r["Rate"].ToString();
                                    obj.ServiceID = r["FK_ServiceID"].ToString();
                                    obj.ServiceName = r["ServiceName"].ToString();
                                    obj.FromDate = r["FromDate"].ToString();
                                    obj.ToDate = r["ToDate"].ToString();
                                    obj.TotalAmount = r["TotalAmount"].ToString();
                                    obj.CGST = r["CGST"].ToString();
                                    obj.SGST = r["SGST"].ToString();
                                    obj.IGST = r["IGST"].ToString();
                                    obj.TDS = r["TDS"].ToString();
                                    obj.Discount = r["Discount"].ToString();
                                    obj.FinalAmount = r["FinalAmount"].ToString();
                                    obj.DescriptionDisplay = r["Description"].ToString();
                                    obj.LineStatus = r["Status"].ToString();
                                    obj.CssClass = r["Class"].ToString();
                                    obj.isPOGenerated = r["IsPOGenerated"].ToString();
                                    obj.PONumber = r["PONumber"].ToString();
                                    obj.HSNCode = r["HSNCode"].ToString();

                                    ViewBag.SUM = Convert.ToDecimal(ViewBag.SUM) + Convert.ToDecimal(r["FinalAmount"].ToString());

                                    lstLines.Add(obj);
                                }
                                model.lstsaleorder = lstLines;
                            }
                        }

                        TempData["Class"] = "alert alert-success";
                        TempData["SaleOrderDetails"] = "Sale Order line added successfully";
                    }
                    else if (ds.Tables[0].Rows[0]["MSG"].ToString() == "0")
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["SaleOrderDetails"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["SaleOrderDetails"] = ex.Message;
            }

            int ctrMediaType = 0;
            int ctrVendors = 0;
            DataSet dsVendors = model.GetServiceList();
            if (dsVendors != null && dsVendors.Tables.Count > 0 && dsVendors.Tables[0].Rows.Count > 0)
            {
                //Bind Media Type according to the selected Service
                List<SelectListItem> ddlmediaType = new List<SelectListItem>();
                //DataSet dsMediaType = model.GetMediaType();

                if (dsVendors != null && dsVendors.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow r in dsVendors.Tables[1].Rows)
                    {
                        if (ctrMediaType == 0)
                        {
                            ddlmediaType.Add(new SelectListItem { Text = "Select Media Type", Value = "0" });
                        }
                        ddlmediaType.Add(new SelectListItem { Text = r["MediaTypeName"].ToString(), Value = r["PK_MediaTypeID"].ToString() });
                        ctrMediaType = ctrMediaType + 1;
                    }
                }
                ViewBag.ddlMediaType = ddlmediaType;
                //Bind Media Type according to the Selected Service

                //Bind Vendors according to the Selected Service
                List<SelectListItem> ddlVendors = new List<SelectListItem>();
                //DataSet dsVendors = model.GetAllVendors();
                if (dsVendors != null && dsVendors.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow r in dsVendors.Tables[2].Rows)
                    {
                        if (ctrVendors == 0)
                        {
                            ddlVendors.Add(new SelectListItem { Text = "Select Vendor", Value = "0" });
                        }
                        ddlVendors.Add(new SelectListItem { Text = r["Name"].ToString(), Value = r["PK_VendorID"].ToString() });
                        ctrVendors = ctrVendors + 1;
                    }
                }
                ViewBag.ddlVendors = ddlVendors;
                //Bind Vendors according to the Selected Service
            }

            #region BindStateList
            int ctrState = 0;
            List<SelectListItem> ddlState = new List<SelectListItem>();
            DataSet dsState = model.GetStateList();
            if (dsState != null && dsState.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsState.Tables[0].Rows)
                {
                    if (ctrState == 0)
                    {
                        ddlState.Add(new SelectListItem { Text = "Select State", Value = "0" });
                    }
                    ddlState.Add(new SelectListItem { Text = r["statename"].ToString(), Value = r["statename"].ToString() });
                    ctrState = ctrState + 1;
                }
            }
            ViewBag.ddlState = ddlState;
            #endregion
            #region BindCityList
            int ctrCity = 0;
            List<SelectListItem> ddlCity = new List<SelectListItem>();
            DataSet dsCity = model.GetCityList();
            if (dsCity != null && dsCity.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsCity.Tables[0].Rows)
                {
                    if (ctrCity == 0)
                    {
                        ddlCity.Add(new SelectListItem { Text = "Select City", Value = "0" });
                    }
                    ddlCity.Add(new SelectListItem { Text = r["City"].ToString(), Value = r["City"].ToString() });
                    ctrCity = ctrCity + 1;
                }
            }
            ViewBag.ddlCity = ddlCity;
            #endregion
            #region Sites
            int count1 = 0;
            List<SelectListItem> ddlSites = new List<SelectListItem>();
            if (model.ServiceID == "2" || model.ServiceID == "6")
            {
                model.VendorID = model.VendorID;
                model.City = model.CityID;
            }
            else
            {
                model.VendorID = null;
                model.City = model.CityID;
            }
            DataSet ds2 = model.GetSiteList();
            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds2.Tables[0].Rows)
                {
                    if (count1 == 0)
                    {
                        ddlSites.Add(new SelectListItem { Text = "Select Site", Value = "0" });
                    }
                    ddlSites.Add(new SelectListItem { Text = r["SiteName"].ToString(), Value = r["PK_SiteID"].ToString() });
                    count1 = count1 + 1;
                }
            }

            ViewBag.ddlSites = ddlSites;

            #endregion
            #region Services
            int count2 = 0;
            List<SelectListItem> ddlServices = new List<SelectListItem>();
            DataSet ds3 = model.GetServiceList();
            if (ds3 != null && ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds3.Tables[0].Rows)
                {
                    if (count1 == 0)
                    {
                        ddlServices.Add(new SelectListItem { Text = "Select Service", Value = "0" });
                    }
                    ddlServices.Add(new SelectListItem { Text = r["ServiceName"].ToString(), Value = r["PK_ServiceId"].ToString() });
                    count2 = count2 + 1;
                }
            }

            ViewBag.ddlServices = ddlServices;

            #endregion
            #region defaults

            List<SelectListItem> ddlPaymentTerms = Common.BindPaymentTerms();
            ViewBag.ddlPaymentTerms = ddlPaymentTerms;

            List<SelectListItem> ddlBillingSnaps = Common.BillingSnaps();
            ViewBag.ddlBillingSnaps = ddlBillingSnaps;

            List<SelectListItem> ddlPOReceived = Common.POReceived();
            ViewBag.ddlPOReceived = ddlPOReceived;

            int count3 = 0;
            List<SelectListItem> ddlSalesPerson = new List<SelectListItem>();
            DataSet dsSalesPerson = model.GetSalesPersonList();
            if (dsSalesPerson != null && dsSalesPerson.Tables.Count > 0 && dsSalesPerson.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsSalesPerson.Tables[0].Rows)
                {
                    if (count3 == 0)
                    {
                        ddlSalesPerson.Add(new SelectListItem { Text = "Select Sales Person", Value = "0" });
                    }
                    ddlSalesPerson.Add(new SelectListItem { Text = r["SalesPersonName"].ToString(), Value = r["PK_SalesPersonID"].ToString() });
                    count3 = count3 + 1;
                }
            }
            ViewBag.ddlSalesPerson = ddlSalesPerson;

            int count4 = 0;
            List<SelectListItem> ddlOperationExecutive = new List<SelectListItem>();
            DataSet dsOperationExecutive = model.GetOperationExecutiveList();
            if (dsOperationExecutive != null && dsOperationExecutive.Tables.Count > 0 && dsOperationExecutive.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsOperationExecutive.Tables[0].Rows)
                {
                    if (count4 == 0)
                    {
                        ddlOperationExecutive.Add(new SelectListItem { Text = "Select Operation Executive", Value = "0" });
                    }
                    ddlOperationExecutive.Add(new SelectListItem { Text = r["OperationExecutivePersonName"].ToString(), Value = r["PK_OperationExecutiveID"].ToString() });
                    count4 = count4 + 1;
                }
            }
            ViewBag.ddlOperationExecutive = ddlOperationExecutive;

            #endregion
            model.Description = "";
            return View(model);
        }

        public ActionResult LoadDataForEdit(string saleorderdetailsid, string linestatus)
        {
            SaleOrder model = new SaleOrder();
            try
            {
                model.PK_SalesOrderDetailsID = saleorderdetailsid;
                model.LineStatus = linestatus;
                DataSet ds = model.GetDataForLineEdit();

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    model.HSNCode = ds.Tables[0].Rows[0]["HSNCode"].ToString();
                    model.City = ds.Tables[0].Rows[0]["City"].ToString();
                    model.SiteID = ds.Tables[0].Rows[0]["FK_SiteID"].ToString();
                    model.MediaTypeID = ds.Tables[0].Rows[0]["FK_MediaTypeID"].ToString();
                    model.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    model.ServiceID = ds.Tables[0].Rows[0]["FK_ServiceID"].ToString();
                    model.VendorID = ds.Tables[0].Rows[0]["FK_VendorID"].ToString();
                    model.FromDate = ds.Tables[0].Rows[0]["FromDate"].ToString();
                    model.ToDate = ds.Tables[0].Rows[0]["ToDate"].ToString();
                    model.Width = ds.Tables[0].Rows[0]["Width"].ToString();
                    model.Height = ds.Tables[0].Rows[0]["Height"].ToString();
                    model.Side = ds.Tables[0].Rows[0]["Side"].ToString();
                    model.Unit = ds.Tables[0].Rows[0]["Quantity"].ToString();
                    model.Area = ds.Tables[0].Rows[0]["Area"].ToString();
                    model.Rate = ds.Tables[0].Rows[0]["Rate"].ToString();
                    model.TotalAmount = ds.Tables[0].Rows[0]["TotalAmount"].ToString();
                    model.TDS = ds.Tables[0].Rows[0]["TDS"].ToString();
                    model.Discount = ds.Tables[0].Rows[0]["Discount"].ToString();
                    model.CGST = ds.Tables[0].Rows[0]["CGST"].ToString();
                    model.SGST = ds.Tables[0].Rows[0]["SGST"].ToString();
                    model.IGST = ds.Tables[0].Rows[0]["IGST"].ToString();
                    model.FinalAmount = ds.Tables[0].Rows[0]["FinalAmount"].ToString();
                    model.ServiceName = ds.Tables[0].Rows[0]["ServiceName"].ToString();
                    model.ServiceType = ds.Tables[0].Rows[0]["ServiceDateType"].ToString();

                    model.Amt = (Convert.ToDecimal(ds.Tables[0].Rows[0]["Area"].ToString()) * Convert.ToDecimal(ds.Tables[0].Rows[0]["Rate"].ToString())).ToString();

                    DataSet dsEditLineData = model.GetServiceList();
                    if (dsEditLineData != null && dsEditLineData.Tables.Count > 0)
                    {
                        model.Result = "1";
                        model.ServiceType = dsEditLineData.Tables[0].Rows[0]["DateFormat"].ToString();

                        //Bind Media Type according to the selected Service
                        List<SelectListItem> ddlmediaType = new List<SelectListItem>();
                        //DataSet dsMediaType = model.GetMediaType();

                        if (dsEditLineData != null && dsEditLineData.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow r in dsEditLineData.Tables[1].Rows)
                            {
                                ddlmediaType.Add(new SelectListItem { Text = r["MediaTypeName"].ToString(), Value = r["PK_MediaTypeID"].ToString() });
                            }
                        }
                        model.lstMediaType = ddlmediaType;
                        //Bind Media Type according to the Selected Service

                        //Bind Vendors according to the Selected Service
                        List<SelectListItem> ddlVendors = new List<SelectListItem>();
                        //DataSet dsVendors = model.GetAllVendors();
                        if (dsEditLineData != null && dsEditLineData.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow r in dsEditLineData.Tables[2].Rows)
                            {
                                ddlVendors.Add(new SelectListItem { Text = r["Name"].ToString(), Value = r["PK_VendorID"].ToString() });
                            }
                        }
                        model.lstVendors = ddlVendors;
                        //Bind Vendors according to the Selected Service
                    }

                    //Load Site DDL at the time of SO Line Edit
                    if (model.ServiceID == "2" || model.ServiceID == "6")
                    {
                        model.VendorID = model.VendorID;
                        model.City = model.City;
                    }
                    else
                    {
                        model.VendorID = null;
                        model.City = model.City;
                    }
                    List<SelectListItem> ddlSites = new List<SelectListItem>();
                    DataSet dsSites = model.GetSiteList();
                    if (dsSites != null && dsSites.Tables.Count > 0)
                    {
                        foreach (DataRow r in dsSites.Tables[0].Rows)
                        {
                            ddlSites.Add(new SelectListItem { Text = r["SiteInfo"].ToString(), Value = r["PK_SiteID"].ToString() });
                        }
                        model.ddlSites = ddlSites;
                    }
                    //Load Site DDL at the time of SO Line Edit
                    //This line is important
                    model.VendorID = ds.Tables[0].Rows[0]["FK_VendorID"].ToString();
                    //This line is important
                }
            }
            catch (Exception ex)
            {

            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("SaleOrderDetails")]
        [OnAction(ButtonName = "btnUpdateLine")]
        public ActionResult UpdateSaleOrderLine(SaleOrder model)
        {
            try
            {
                model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
                model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
                model.AddedBy = Session["UserID"].ToString();

                DataSet ds = model.UpdateSaleOrderLine();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        ds = model.GetSaleOrderDetails();
                        if (ds != null)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                model.PK_SalesOrderID = ds.Tables[0].Rows[0]["PK_SalesOrderID"].ToString();
                                model.SalesOrderNoID = ds.Tables[0].Rows[0]["PK_SalesOrderID"].ToString();
                                model.SalesOrderNo = ds.Tables[0].Rows[0]["SalesOrderNo"].ToString();
                                model.OrderDate = ds.Tables[0].Rows[0]["SalesOrderDate"].ToString();
                                model.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                                model.CustomerCode = ds.Tables[0].Rows[0]["CustomerCode"].ToString();
                                model.CampaignID = ds.Tables[0].Rows[0]["PK_CamapignID"].ToString();
                                model.CampaignNumber = ds.Tables[0].Rows[0]["CampaignNo"].ToString();
                                model.POReceived = ds.Tables[0].Rows[0]["IsPOReceived"].ToString();
                                model.PONumber = ds.Tables[0].Rows[0]["PONumber"].ToString();
                                model.PaymentTermsID = ds.Tables[0].Rows[0]["FK_PaymentTermsID"].ToString();
                                model.SalesPersonID = ds.Tables[0].Rows[0]["FK_SalesPersonID"].ToString();
                                model.OperationExecutiveID = ds.Tables[0].Rows[0]["FK_OperationExecutiveID"].ToString();
                                model.BillingSnapsID = ds.Tables[0].Rows[0]["FK_BillingSnapsID"].ToString();
                                model.POImagePath = ds.Tables[0].Rows[0]["POImagePath"].ToString();
                                model.isPOGenerated = ds.Tables[0].Rows[0]["IsPOGenerated"].ToString();
                                model.SOStatus = ds.Tables[0].Rows[0]["Status"].ToString();
                                model.PostStatus = ds.Tables[0].Rows[0]["PostStatus"].ToString();
                                model.StateName = ds.Tables[0].Rows[0]["StateName"].ToString();
                            }

                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                List<SaleOrder> lstLines = new List<SaleOrder>();
                                foreach (DataRow r in ds.Tables[1].Rows)
                                {
                                    SaleOrder obj = new SaleOrder();
                                    obj.SaleOrderDetailsID = r["PK_SalesOrderDetailsID"].ToString();
                                    obj.City = r["City"].ToString();
                                    obj.SiteID = r["FK_SiteID"].ToString();
                                    obj.SiteName = r["SiteName"].ToString();
                                    obj.VendorID = r["FK_VendorID"].ToString();
                                    obj.VendorName = r["VendorName"].ToString();
                                    obj.MediaTypeID = r["FK_MediaTypeID"].ToString();
                                    obj.MediaType = r["MediaTypeName"].ToString();
                                    obj.Side = r["Side"].ToString();
                                    obj.Height = r["Height"].ToString();
                                    obj.Width = r["Width"].ToString();
                                    obj.Area = r["Area"].ToString();
                                    obj.Quantity = r["Quantity"].ToString();
                                    obj.Unit = r["Quantity"].ToString();
                                    obj.Rate = r["Rate"].ToString();
                                    obj.ServiceID = r["FK_ServiceID"].ToString();
                                    obj.ServiceName = r["ServiceName"].ToString();
                                    obj.FromDate = r["FromDate"].ToString();
                                    obj.ToDate = r["ToDate"].ToString();
                                    obj.TotalAmount = r["TotalAmount"].ToString();
                                    obj.CGST = r["CGST"].ToString();
                                    obj.SGST = r["SGST"].ToString();
                                    obj.IGST = r["IGST"].ToString();
                                    obj.TDS = r["TDS"].ToString();
                                    obj.Discount = r["Discount"].ToString();
                                    obj.FinalAmount = r["FinalAmount"].ToString();
                                    obj.Description = r["Description"].ToString();
                                    obj.DescriptionDisplay = r["Description"].ToString();
                                    obj.LineStatus = r["Status"].ToString();
                                    obj.CssClass = r["Class"].ToString();
                                    obj.isPOGenerated = r["IsPOGenerated"].ToString();
                                    obj.PONumber = r["PONumber"].ToString();
                                    obj.HSNCode = r["HSNCode"].ToString();

                                    ViewBag.SUM = Convert.ToDecimal(ViewBag.SUM) + Convert.ToDecimal(r["FinalAmount"].ToString());

                                    lstLines.Add(obj);
                                }
                                model.lstsaleorder = lstLines;
                            }
                        }

                        TempData["Class"] = "alert alert-success";
                        TempData["SaleOrderDetails"] = "Sale Order line updated successfully";
                    }
                    else if (ds.Tables[0].Rows[0]["MSG"].ToString() == "0")
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["SaleOrderDetails"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["SaleOrderDetails"] = ex.Message;
            }
            int ctrMediaType = 0;
            int ctrVendors = 0;
            DataSet dsVendors = model.GetServiceList();
            if (dsVendors != null && dsVendors.Tables.Count > 0 && dsVendors.Tables[0].Rows.Count > 0)
            {
                //Bind Media Type according to the selected Service
                List<SelectListItem> ddlmediaType = new List<SelectListItem>();
                //DataSet dsMediaType = model.GetMediaType();

                if (dsVendors != null && dsVendors.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow r in dsVendors.Tables[1].Rows)
                    {
                        if (ctrMediaType == 0)
                        {
                            ddlmediaType.Add(new SelectListItem { Text = "Select Media Type", Value = "0" });
                        }
                        ddlmediaType.Add(new SelectListItem { Text = r["MediaTypeName"].ToString(), Value = r["PK_MediaTypeID"].ToString() });
                        ctrMediaType = ctrMediaType + 1;
                    }
                }
                ViewBag.ddlMediaType = ddlmediaType;
                //Bind Media Type according to the Selected Service

                //Bind Vendors according to the Selected Service
                List<SelectListItem> ddlVendors = new List<SelectListItem>();
                //DataSet dsVendors = model.GetAllVendors();
                if (dsVendors != null && dsVendors.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow r in dsVendors.Tables[2].Rows)
                    {
                        if (ctrVendors == 0)
                        {
                            ddlVendors.Add(new SelectListItem { Text = "Select Vendor", Value = "0" });
                        }
                        ddlVendors.Add(new SelectListItem { Text = r["Name"].ToString(), Value = r["PK_VendorID"].ToString() });
                        ctrVendors = ctrVendors + 1;
                    }
                }
                ViewBag.ddlVendors = ddlVendors;
                //Bind Vendors according to the Selected Service
            }

            #region BindStateList
            int ctrState = 0;
            List<SelectListItem> ddlState = new List<SelectListItem>();
            DataSet dsState = model.GetStateList();
            if (dsState != null && dsState.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsState.Tables[0].Rows)
                {
                    if (ctrState == 0)
                    {
                        ddlState.Add(new SelectListItem { Text = "Select State", Value = "0" });
                    }
                    ddlState.Add(new SelectListItem { Text = r["statename"].ToString(), Value = r["statename"].ToString() });
                    ctrState = ctrState + 1;
                }
            }
            ViewBag.ddlState = ddlState;
            #endregion
            #region BindCityList
            int ctrCity = 0;
            List<SelectListItem> ddlCity = new List<SelectListItem>();
            DataSet dsCity = model.GetCityList();
            if (dsCity != null && dsCity.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsCity.Tables[0].Rows)
                {
                    if (ctrCity == 0)
                    {
                        ddlCity.Add(new SelectListItem { Text = "Select City", Value = "0" });
                    }
                    ddlCity.Add(new SelectListItem { Text = r["City"].ToString(), Value = r["City"].ToString() });
                    ctrCity = ctrCity + 1;
                }
            }
            ViewBag.ddlCity = ddlCity;
            #endregion
            #region Sites
            int count1 = 0;
            List<SelectListItem> ddlSites = new List<SelectListItem>();
            if (model.ServiceID == "2" || model.ServiceID == "6")
            {
                model.VendorID = model.VendorID;
                model.City = model.CityID;
            }
            else
            {
                model.VendorID = null;
                model.City = model.CityID;
            }
            DataSet ds2 = model.GetSiteList();
            if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds2.Tables[0].Rows)
                {
                    if (count1 == 0)
                    {
                        ddlSites.Add(new SelectListItem { Text = "Select Site", Value = "0" });
                    }
                    ddlSites.Add(new SelectListItem { Text = r["SiteName"].ToString(), Value = r["PK_SiteID"].ToString() });
                    count1 = count1 + 1;
                }
            }

            ViewBag.ddlSites = ddlSites;

            #endregion
            #region Services
            int count2 = 0;
            List<SelectListItem> ddlServices = new List<SelectListItem>();
            DataSet ds3 = model.GetServiceList();
            if (ds3 != null && ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds3.Tables[0].Rows)
                {
                    if (count1 == 0)
                    {
                        ddlServices.Add(new SelectListItem { Text = "Select Service", Value = "0" });
                    }
                    ddlServices.Add(new SelectListItem { Text = r["ServiceName"].ToString(), Value = r["PK_ServiceId"].ToString() });
                    count2 = count2 + 1;
                }
            }

            ViewBag.ddlServices = ddlServices;

            #endregion
            #region defaults

            List<SelectListItem> ddlPaymentTerms = Common.BindPaymentTerms();
            ViewBag.ddlPaymentTerms = ddlPaymentTerms;

            List<SelectListItem> ddlBillingSnaps = Common.BillingSnaps();
            ViewBag.ddlBillingSnaps = ddlBillingSnaps;

            List<SelectListItem> ddlPOReceived = Common.POReceived();
            ViewBag.ddlPOReceived = ddlPOReceived;

            int count3 = 0;
            List<SelectListItem> ddlSalesPerson = new List<SelectListItem>();
            DataSet dsSalesPerson = model.GetSalesPersonList();
            if (dsSalesPerson != null && dsSalesPerson.Tables.Count > 0 && dsSalesPerson.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsSalesPerson.Tables[0].Rows)
                {
                    if (count3 == 0)
                    {
                        ddlSalesPerson.Add(new SelectListItem { Text = "Select Sales Person", Value = "0" });
                    }
                    ddlSalesPerson.Add(new SelectListItem { Text = r["SalesPersonName"].ToString(), Value = r["PK_SalesPersonID"].ToString() });
                    count3 = count3 + 1;
                }
            }
            ViewBag.ddlSalesPerson = ddlSalesPerson;

            int count4 = 0;
            List<SelectListItem> ddlOperationExecutive = new List<SelectListItem>();
            DataSet dsOperationExecutive = model.GetOperationExecutiveList();
            if (dsOperationExecutive != null && dsOperationExecutive.Tables.Count > 0 && dsOperationExecutive.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsOperationExecutive.Tables[0].Rows)
                {
                    if (count4 == 0)
                    {
                        ddlOperationExecutive.Add(new SelectListItem { Text = "Select Operation Executive", Value = "0" });
                    }
                    ddlOperationExecutive.Add(new SelectListItem { Text = r["OperationExecutivePersonName"].ToString(), Value = r["PK_OperationExecutiveID"].ToString() });
                    count4 = count4 + 1;
                }
            }
            ViewBag.ddlOperationExecutive = ddlOperationExecutive;

            #endregion
            model.Description = "";
            return View(model);
        }

        [HttpPost]
        [ActionName("SaleOrderDetails")]
        [OnAction(ButtonName = "SaveSaleOrder")]
        public ActionResult SaveSaleOrder(HttpPostedFileBase postedFile, SaleOrder model)
        {
            //if (postedFile != null)
            //{
            //    model.POImagePath = "../SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(postedFile.FileName);
            //    postedFile.SaveAs(Path.Combine(Server.MapPath(model.POImagePath)));
            //}

            if (Request["hdPOImagePath"] != null)
            {
                model.POImagePath = Request["hdPOImagePath"];
            }
            model.OrderDate = string.IsNullOrEmpty(model.OrderDate) ? null : Common.ConvertToSystemDate(model.OrderDate, "dd/MM/yyyy");
            model.AddedBy = Session["UserID"].ToString();

            DataSet ds = model.SaveSaleOrder();
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    TempData["Class"] = "alert alert-success";
                    TempData["GenerateOrder"] = "Sale Order details saved successfully";
                }
                else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    TempData["Class"] = "alert alert-danger";
                    TempData["GenerateOrder"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }

            return RedirectToAction("GenerateSaleOrder");
        }

        [HttpPost]
        [ActionName("SaleOrderDetails")]
        [OnAction(ButtonName = "UpdateSaleOrder")]
        public ActionResult UpdateSaleOrderNew(HttpPostedFileBase postedFile, SaleOrder model)
        {
            if (postedFile != null)
            {
                model.POImagePath = "../SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(Path.Combine(Server.MapPath(model.POImagePath)));
            }

            model.OrderDate = string.IsNullOrEmpty(model.OrderDate) ? null : Common.ConvertToSystemDate(model.OrderDate, "dd/MM/yyyy");
            model.AddedBy = Session["UserID"].ToString();

            DataSet ds = model.UpdateSaleOrder();
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    TempData["Class"] = "alert alert-success";
                    TempData["GenerateOrder"] = "Sale Order details updated successfully";
                }
                else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    TempData["Class"] = "alert alert-danger";
                    TempData["GenerateOrder"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            return RedirectToAction("GenerateSaleOrder");
        }

        public ActionResult DeleteSaleOrderLine(string id, string st)
        {
            SaleOrder model = new SaleOrder();
            try
            {
                model.PK_SalesOrderDetailsID = id;
                model.LineStatus = st;
                model.AddedBy = Session["UserID"].ToString();

                DataSet ds = model.DeleteSaleOrderLine();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        model.Result = "1";
                        TempData["Class"] = "alert alert-success";
                        TempData["SaleOrderDetails"] = "Sale Order line deleted";
                    }
                    else if (ds.Tables[0].Rows[0]["MSG"].ToString() == "0")
                    {
                        model.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        TempData["Class"] = "alert alert-success";
                        TempData["SaleOrderDetails"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region printSaleOrder

        public ActionResult PrintSO(string SaleOrderId, string no)
        {
            SaleOrder model = new SaleOrder();
            List<SaleOrder> lstSaleOrderDetails = new List<SaleOrder>();

            ViewBag.CompanyName = CompanyProfile.CompanyName;
            ViewBag.CompanyMobile = CompanyProfile.CompanyMobile;
            ViewBag.CompanyEmail = CompanyProfile.CompanyEmail;
            ViewBag.CompanyAddress = CompanyProfile.CompanyAddress;
            ViewBag.BankName = CompanyProfile.BankName;
            ViewBag.AccountNo = CompanyProfile.AccountNo;
            ViewBag.IFSC = CompanyProfile.IFSC;

            model.SalesOrderNo = Crypto.Decrypt(no);
            model.CustomerID = Crypto.Decrypt(SaleOrderId);
            DataSet ds = model.PrintSO();
            model.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
            model.CustomerAddress = ds.Tables[0].Rows[0]["CustomerAddress"].ToString();
            ViewBag.SaleOrderDate = ds.Tables[0].Rows[0]["SalesOrderDate"].ToString();
       

            if (ds != null && ds.Tables[1].Rows.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables.Count>0)
            {
                ViewBag.FinalAmount = 0;
                ViewBag.CGST = ViewBag.SGST = ViewBag.IGST = 0;
                ViewBag.CustomerAddress = ds.Tables[0].Rows[0]["CustomerAddress"].ToString();
                ViewBag.InvoiceNumber = ds.Tables[0].Rows[0]["SalesOrderNo"].ToString();
                foreach (DataRow r in ds.Tables[1].Rows)
                {
                    SaleOrder obj = new SaleOrder();
                    obj.Description = r["Description"].ToString();
                    obj.HSNCode = r["HSNCode"].ToString();
                    obj.Width = r["Width"].ToString();
                    obj.Height = r["Height"].ToString();
                    obj.Area = r["Area"].ToString();
                    obj.Quantity = r["Quantity"].ToString();
                    obj.Rate = r["Rate"].ToString();
                    obj.TotalAmount = r["TotalAmount"].ToString();
                   ViewBag.HSNCode= r["HSNCode"].ToString();
                    ViewBag.AmountInWords = r["FinalAmountWords"].ToString();
                    ViewBag.Rate = r["Rate"].ToString();
                    ViewBag.CGST = Math.Round(Convert.ToDecimal(ViewBag.CGST) + Convert.ToDecimal(r["CGSTAmt"].ToString()), 2);
                    ViewBag.SGST = Math.Round(Convert.ToDecimal(ViewBag.SGST) + Convert.ToDecimal(r["SGSTAmt"].ToString()), 2);
                    ViewBag.IGST = Math.Round(Convert.ToDecimal(ViewBag.IGST) + Convert.ToDecimal(r["IGSTAmt"].ToString()), 2);

                    ViewBag.FinalAmount = Convert.ToDecimal(ViewBag.FinalAmount) + Convert.ToDecimal(r["FinalAmount"].ToString());
                    //ViewBag.FinalAmountGST = Convert.ToDecimal(ViewBag.FinalAmount) + Convert.ToDecimal(ViewBag.SGST) + Convert.ToDecimal(ViewBag.CGST) + Convert.ToDecimal(ViewBag.IGST);

                    ViewBag.CentralTax = (Convert.ToDecimal(ViewBag.FinalAmount) * 9 / 100);
                    ViewBag.StateTax = (Convert.ToDecimal(ViewBag.FinalAmount) * 9 / 100);
                    ViewBag.TotalTax = Convert.ToDecimal(ViewBag.CentralTax) + Convert.ToDecimal(ViewBag.StateTax);

                    lstSaleOrderDetails.Add(obj);
                }
                model.lstsaleorder = lstSaleOrderDetails;
            }
            return View(model);
        }

        #endregion

        #region oldMethods
        public ActionResult AddLines(string siteID, string siteName, string side, string area, string qty, string rate, string serviceID, string serviceName, string FromDate, string ToDate, string totalAmount, string cgst, string sgst, string igst, string tds, string discount, string finalAmount, string vendorID, string vendorName, string height, string width, string description)
        {
            SaleOrder model = new SaleOrder();
            try
            {
                if (Session["tmpLines"] != null)
                {
                    dt = (DataTable)Session["tmpLines"];
                    DataRow dr = null;

                    if (dt.Rows.Count >= 0)
                    {
                        dr = dt.NewRow();
                        dr["PK_SalesOrderDetailsID"] = "0";
                        dr["FK_SiteID"] = siteID;
                        dr["SiteName"] = siteName;
                        dr["FK_VendorID"] = vendorID;
                        dr["VendorName"] = vendorName;
                        dr["Side"] = side;
                        dr["Height"] = height;
                        dr["Width"] = width;
                        dr["Area"] = area;
                        dr["Quantity"] = qty.ToString();
                        dr["Rate"] = rate;
                        dr["FK_ServiceID"] = serviceID;
                        dr["ServiceName"] = serviceName;
                        dr["FromDate"] = FromDate;
                        dr["ToDate"] = ToDate;
                        dr["TotalAmount"] = totalAmount;
                        dr["CGST"] = cgst;
                        dr["SGST"] = sgst;
                        dr["IGST"] = igst;
                        dr["TDS"] = tds;
                        dr["Discount"] = discount;
                        dr["FinalAmount"] = finalAmount;
                        dr["Description"] = description;

                        dt.Rows.Add(dr);
                        Session["tmpLines"] = dt;
                    }
                }
                else
                {
                    dt.Columns.Add("PK_SalesOrderDetailsID", typeof(string));
                    dt.Columns.Add("FK_SiteID", typeof(string));
                    dt.Columns.Add("SiteName", typeof(string));
                    dt.Columns.Add("FK_VendorID", typeof(string));
                    dt.Columns.Add("VendorName", typeof(string));
                    dt.Columns.Add("Side", typeof(string));
                    dt.Columns.Add("Height", typeof(string));
                    dt.Columns.Add("Width", typeof(string));
                    dt.Columns.Add("Area", typeof(string));
                    dt.Columns.Add("Quantity", typeof(string));
                    dt.Columns.Add("Rate", typeof(string));
                    dt.Columns.Add("FK_ServiceID", typeof(string));
                    dt.Columns.Add("ServiceName", typeof(string));
                    dt.Columns.Add("FromDate", typeof(string));
                    dt.Columns.Add("ToDate", typeof(string));
                    dt.Columns.Add("TotalAmount", typeof(string));
                    dt.Columns.Add("CGST", typeof(string));
                    dt.Columns.Add("SGST", typeof(string));
                    dt.Columns.Add("IGST", typeof(string));
                    dt.Columns.Add("TDS", typeof(string));
                    dt.Columns.Add("Discount", typeof(string));
                    dt.Columns.Add("FinalAmount", typeof(string));
                    dt.Columns.Add("Description", typeof(string));

                    DataRow dr = dt.NewRow();

                    dr["PK_SalesOrderDetailsID"] = "0";
                    dr["FK_SiteID"] = siteID;
                    dr["SiteName"] = siteName;
                    dr["FK_VendorID"] = vendorID;
                    dr["VendorName"] = vendorName;
                    dr["Side"] = side;
                    dr["Height"] = height;
                    dr["Width"] = width;
                    dr["Area"] = area;
                    dr["FK_ServiceID"] = serviceID;
                    dr["ServiceName"] = serviceName;
                    dr["FromDate"] = FromDate;
                    dr["ToDate"] = ToDate;
                    dr["Quantity"] = qty;
                    dr["Rate"] = rate;
                    dr["TotalAmount"] = totalAmount;
                    dr["CGST"] = cgst;
                    dr["SGST"] = sgst;
                    dr["IGST"] = igst;
                    dr["TDS"] = tds;
                    dr["Discount"] = discount;
                    dr["FinalAmount"] = finalAmount;
                    dr["Description"] = description;

                    dt.Rows.Add(dr);
                    Session["tmpLines"] = dt;
                }

                dt = (DataTable)Session["tmpLines"];

                //Merge Old Sale Order Details data in new DataTable Start
                if (Session["tmpLinesOld"] != null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("PK_SalesOrderDetailsID", typeof(string));
                    dt.Columns.Add("FK_SiteID", typeof(string));
                    dt.Columns.Add("SiteName", typeof(string));
                    dt.Columns.Add("FK_VendorID", typeof(string));
                    dt.Columns.Add("VendorName", typeof(string));
                    dt.Columns.Add("Side", typeof(string));
                    dt.Columns.Add("Height", typeof(string));
                    dt.Columns.Add("Width", typeof(string));
                    dt.Columns.Add("Area", typeof(string));
                    dt.Columns.Add("Quantity", typeof(string));
                    dt.Columns.Add("Rate", typeof(string));
                    dt.Columns.Add("FK_ServiceID", typeof(string));
                    dt.Columns.Add("ServiceName", typeof(string));
                    dt.Columns.Add("FromDate", typeof(string));
                    dt.Columns.Add("ToDate", typeof(string));
                    dt.Columns.Add("TotalAmount", typeof(string));
                    dt.Columns.Add("CGST", typeof(string));
                    dt.Columns.Add("SGST", typeof(string));
                    dt.Columns.Add("IGST", typeof(string));
                    dt.Columns.Add("TDS", typeof(string));
                    dt.Columns.Add("Discount", typeof(string));
                    dt.Columns.Add("FinalAmount", typeof(string));
                    dt.Columns.Add("Description", typeof(string));

                    DataTable dtOldData = new DataTable();
                    dtOldData = (DataTable)Session["tmpLinesOld"];
                    dt.Merge(dtOldData, true, MissingSchemaAction.Ignore);
                    //dtOldData.Merge(dt);
                    //dt = dtOldData;
                    Session["tmpLines"] = dt;
                    Session["tmpLinesOld"] = null;
                }
                //Merge Old Sale Order Details data in new DataTable End

                int sno = 1;
                List<SaleOrder> lstTmpData = new List<SaleOrder>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        SaleOrder obj = new SaleOrder();
                        obj.LineSNo = sno++.ToString();
                        obj.SaleOrderDetailsID = r["PK_SalesOrderDetailsID"].ToString();
                        obj.SiteID = r["FK_SiteID"].ToString();
                        obj.SiteName = r["SiteName"].ToString();
                        obj.VendorID = r["FK_VendorID"].ToString();
                        obj.VendorName = r["VendorName"].ToString();
                        obj.Side = r["Side"].ToString();
                        obj.Height = r["Height"].ToString();
                        obj.Width = r["Width"].ToString();
                        obj.Area = r["Area"].ToString();
                        obj.ServiceID = r["FK_ServiceID"].ToString();
                        obj.ServiceName = r["ServiceName"].ToString();
                        obj.FromDate = r["FromDate"].ToString();
                        obj.ToDate = r["ToDate"].ToString();
                        obj.Quantity = r["Quantity"].ToString();
                        obj.Rate = r["Rate"].ToString();
                        obj.TotalAmount = r["TotalAmount"].ToString();
                        obj.CGST = r["CGST"].ToString();
                        obj.SGST = r["SGST"].ToString();
                        obj.IGST = r["IGST"].ToString();
                        obj.TDS = r["TDS"].ToString();
                        obj.Discount = r["Discount"].ToString();
                        obj.FinalAmount = r["FinalAmount"].ToString();
                        obj.Description = r["Description"].ToString();

                        model.TotalAmountDisplay = (Convert.ToDecimal(model.TotalAmountDisplay) + Convert.ToDecimal(r["FinalAmount"].ToString())).ToString();
                        lstTmpData.Add(obj);
                    }
                    model.lstsaleorder = lstTmpData;
                }
            }
            catch (Exception ex)
            {
                model.Result = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult selectDataForLineEdit(string linesno)
        {
            SaleOrder model = new SaleOrder();
            try
            {
                //Store the line number from Lines to update Data later
                Session["LineNo"] = linesno;
                //Store the line number from Lines to update Data later

                dt = (DataTable)Session["tmpLines"];
                DataRow[] rows = dt.Select();
                DataRow rowtoedit = rows[int.Parse(linesno) - 1];

                model.SiteName = rowtoedit["SiteName"].ToString();
                model.ServiceName = rowtoedit["ServiceName"].ToString();
                model.VendorName = rowtoedit["VendorName"].ToString();
                model.FromDate = rowtoedit["FromDate"].ToString();
                model.ToDate = rowtoedit["ToDate"].ToString();
                model.Width = rowtoedit["Width"].ToString();
                model.Height = rowtoedit["Height"].ToString();
                model.Side = rowtoedit["Side"].ToString();
                model.Unit = rowtoedit["Quantity"].ToString();
                model.Area = rowtoedit["Area"].ToString();
                model.Rate = rowtoedit["Rate"].ToString();
                model.TotalAmount = rowtoedit["TotalAmount"].ToString();
                model.TDS = rowtoedit["TDS"].ToString();
                model.Discount = rowtoedit["Discount"].ToString();
                model.CGST = rowtoedit["CGST"].ToString();
                model.SGST = rowtoedit["SGST"].ToString();
                model.IGST = rowtoedit["IGST"].ToString();
                model.FinalAmount = rowtoedit["FinalAmount"].ToString();
                model.Description = rowtoedit["Description"].ToString();

                //rowtoedit.Delete();
                //dt.AcceptChanges();
            }
            catch (Exception ex)
            {

            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateLineData(string siteid, string sitename, string serviceid, string servicename, string vendorid, string vendorname, string fromdate, string todate, string width, string height, string side,
            string quantity, string area, string rate, string totalamount, string tds, string discount, string cgst, string sgst, string igst, string finalamount, string description)
        {
            SaleOrder model = new SaleOrder();
            try
            {
                dt = (DataTable)Session["tmpLines"];
                DataRow[] rows = dt.Select();
                DataRow rowtoedit = rows[int.Parse(Session["LineNo"].ToString()) - 1];

                //rowtoedit.Delete();
                //dt.AcceptChanges();

                rowtoedit["FK_SiteID"] = siteid;
                rowtoedit["SiteName"] = sitename;
                rowtoedit["FK_ServiceID"] = serviceid;
                rowtoedit["ServiceName"] = servicename;
                rowtoedit["FK_VendorID"] = vendorid;
                rowtoedit["VendorName"] = vendorname;
                rowtoedit["FromDate"] = fromdate;
                rowtoedit["ToDate"] = todate;
                rowtoedit["Width"] = width;
                rowtoedit["Height"] = height;
                rowtoedit["Side"] = side;
                rowtoedit["Quantity"] = quantity;
                rowtoedit["Area"] = area;
                rowtoedit["Rate"] = rate;
                rowtoedit["TotalAmount"] = totalamount;
                rowtoedit["TDS"] = tds;
                rowtoedit["Discount"] = discount;
                rowtoedit["CGST"] = cgst;
                rowtoedit["SGST"] = sgst;
                rowtoedit["IGST"] = igst;
                rowtoedit["FinalAmount"] = finalamount;
                rowtoedit["Description"] = description;

                //dt.Rows.Add(rowtoedit);

                //Bind data in Lines again after Updated value
                int sno = 1;
                List<SaleOrder> lstTmpData = new List<SaleOrder>();
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        SaleOrder obj = new SaleOrder();
                        obj.LineSNo = sno++.ToString();
                        obj.SaleOrderDetailsID = r["PK_SalesOrderDetailsID"].ToString();
                        obj.SiteID = r["FK_SiteID"].ToString();
                        obj.SiteName = r["SiteName"].ToString();
                        obj.VendorID = r["FK_VendorID"].ToString();
                        obj.VendorName = r["VendorName"].ToString();
                        obj.Side = r["Side"].ToString();
                        obj.Height = r["Height"].ToString();
                        obj.Width = r["Width"].ToString();
                        obj.Area = r["Area"].ToString();
                        obj.ServiceID = r["FK_ServiceID"].ToString();
                        obj.ServiceName = r["ServiceName"].ToString();
                        obj.FromDate = r["FromDate"].ToString();
                        obj.ToDate = r["ToDate"].ToString();
                        obj.Quantity = r["Quantity"].ToString();
                        obj.Rate = r["Rate"].ToString();
                        obj.TotalAmount = r["TotalAmount"].ToString();
                        obj.CGST = r["CGST"].ToString();
                        obj.SGST = r["SGST"].ToString();
                        obj.IGST = r["IGST"].ToString();
                        obj.TDS = r["TDS"].ToString();
                        obj.Discount = r["Discount"].ToString();
                        obj.FinalAmount = r["FinalAmount"].ToString();
                        obj.Description = r["Description"].ToString();

                        model.TotalAmountDisplay = (Convert.ToDecimal(model.TotalAmountDisplay) + Convert.ToDecimal(r["FinalAmount"].ToString())).ToString();
                        lstTmpData.Add(obj);
                    }
                    model.lstsaleorder = lstTmpData;
                }
                //Bind data in Lines again after Updated value
            }
            catch (Exception ex)
            {

            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteLine(string saleOrderDetailsID)
        {
            SaleOrder model = new SaleOrder();
            model.SaleOrderDetailsID = saleOrderDetailsID;
            model.AddedBy = Session["UserID"].ToString();

            DataSet ds = model.DeleteSaleOrderLine();
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    model.Result = "1";
                }
                else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    model.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ActionName("GetSaleOrderDetails")]
        [OnAction(ButtonName = "SaveSaleOrder")]
        public ActionResult InsertSaleOrder(HttpPostedFileBase postedFile, SaleOrder model)
        {
            if (postedFile != null)
            {
                model.POImagePath = "../SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(Path.Combine(Server.MapPath(model.POImagePath)));
            }

            string noofrows = Request["hdrows"].ToString();
            DataTable dtst = new DataTable();

            dtst.Columns.Add("PK_SalesOrderDetailsID", typeof(string));
            dtst.Columns.Add("FK_SiteID", typeof(string));
            dtst.Columns.Add("SiteName", typeof(string));
            dtst.Columns.Add("FK_ServiceID", typeof(string));
            dtst.Columns.Add("ServiceName", typeof(string));
            dtst.Columns.Add("FK_VendorID", typeof(string));
            dtst.Columns.Add("VendorName", typeof(string));
            dtst.Columns.Add("FromDate", typeof(string));
            dtst.Columns.Add("ToDate", typeof(string));
            dtst.Columns.Add("Side", typeof(string));
            dtst.Columns.Add("Height", typeof(string));
            dtst.Columns.Add("Width", typeof(string));
            dtst.Columns.Add("Area", typeof(string));
            dtst.Columns.Add("Quantity", typeof(string));
            dtst.Columns.Add("Rate", typeof(string));
            dtst.Columns.Add("TotalAmount", typeof(string));
            dtst.Columns.Add("CGST", typeof(string));
            dtst.Columns.Add("SGST", typeof(string));
            dtst.Columns.Add("IGST", typeof(string));
            dtst.Columns.Add("TDS", typeof(string));
            dtst.Columns.Add("Discount", typeof(string));
            dtst.Columns.Add("FinalAmount", typeof(string));
            dtst.Columns.Add("Description", typeof(string));

            string siteID, pkSaleOrderDetailsID = "";
            string vendorID, vendorName, height, width, description = "";
            string siteName = ""; string serviceID = ""; string serviceName = ""; string fromDate = ""; string toDate = ""; string side = ""; string area = ""; string qty = "";
            string rate = ""; string totalAmount = ""; string cgst = ""; string sgst = ""; string igst = ""; string tds = ""; string discount = ""; string finalAmount = "";
            for (int i = 0; i < int.Parse(noofrows) - 1; i++)
            {
                pkSaleOrderDetailsID = null;
                siteID = Request["SiteID_ " + i].ToString();
                siteName = Request["SiteName_ " + i].ToString();
                serviceID = Request["ServiceID_ " + i].ToString();
                serviceName = Request["ServiceName_ " + i].ToString();
                vendorID = Request["VendorID_ " + i].ToString();
                vendorName = Request["VendorName_ " + i].ToString();
                height = Request["Height_ " + i].ToString();
                width = Request["Width_ " + i].ToString();
                description = Request["Description_ " + i].ToString();
                fromDate = Common.ConvertToSystemDate(Request["FromDate_ " + i].ToString(), "dd/MM/yyyy");
                toDate = string.IsNullOrEmpty(Request["ToDate_ " + i].ToString()) ? null : Common.ConvertToSystemDate(Request["ToDate_ " + i].ToString(), "dd/MM/yyyy");
                side = Request["Side_ " + i].ToString();
                area = Request["Area_ " + i].ToString();
                qty = Request["Quantity_ " + i].ToString();
                rate = Request["Rate_ " + i].ToString();
                totalAmount = Request["TotalAmount_ " + i].ToString();
                cgst = Request["CGST_ " + i].ToString();
                sgst = Request["SGST_ " + i].ToString();
                igst = Request["IGST_ " + i].ToString();
                discount = Request["Discount_ " + i].ToString();
                tds = Request["TDS_ " + i].ToString();
                finalAmount = Request["FinalAmount_ " + i].ToString();

                dtst.Rows.Add(pkSaleOrderDetailsID, siteID, siteName, serviceID, serviceName, vendorID, vendorName, fromDate, toDate, side, height, width, area, qty, rate, totalAmount, cgst, sgst, igst, tds, discount, finalAmount, description);
            }

            model.dtSaleOrderDetails = dtst;
            model.OrderDate = string.IsNullOrEmpty(model.OrderDate) ? null : Common.ConvertToSystemDate(model.OrderDate, "dd/MM/yyyy");
            model.AddedBy = Session["UserID"].ToString();

            DataSet ds = model.SaleOrderInsert();
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    //Success
                }
                else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    //Error
                }
            }
            TempData["GenerateOrder"] = "Sale Order details saved successfully";
            return RedirectToAction("GenerateSaleOrder");
        }

        [HttpPost]
        [ActionName("GetSaleOrderDetails")]
        [OnAction(ButtonName = "UpdateSaleOrder")]
        public ActionResult UpdateSaleOrder(HttpPostedFileBase postedFile, SaleOrder model)
        {
            if (postedFile != null)
            {
                model.POImagePath = "../SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(Path.Combine(Server.MapPath(model.POImagePath)));
            }

            string noofrows = Request["hdrows"].ToString();
            DataTable dtst = new DataTable();

            dtst.Columns.Add("PK_SalesOrderDetailsID", typeof(string));
            dtst.Columns.Add("FK_SiteID", typeof(string));
            dtst.Columns.Add("SiteName", typeof(string));
            dtst.Columns.Add("FK_ServiceID", typeof(string));
            dtst.Columns.Add("ServiceName", typeof(string));
            dtst.Columns.Add("FK_VendorID", typeof(string));
            dtst.Columns.Add("VendorName", typeof(string));
            dtst.Columns.Add("FromDate", typeof(string));
            dtst.Columns.Add("ToDate", typeof(string));
            dtst.Columns.Add("Side", typeof(string));
            dtst.Columns.Add("Height", typeof(string));
            dtst.Columns.Add("Width", typeof(string));
            dtst.Columns.Add("Area", typeof(string));
            dtst.Columns.Add("Quantity", typeof(string));
            dtst.Columns.Add("Rate", typeof(string));
            dtst.Columns.Add("TotalAmount", typeof(string));
            dtst.Columns.Add("CGST", typeof(string));
            dtst.Columns.Add("SGST", typeof(string));
            dtst.Columns.Add("IGST", typeof(string));
            dtst.Columns.Add("TDS", typeof(string));
            dtst.Columns.Add("Discount", typeof(string));
            dtst.Columns.Add("FinalAmount", typeof(string));
            dtst.Columns.Add("Description", typeof(string));

            string siteID, pkSaleOrderDetailsID = "";
            string vendorID, vendorName, height, width, description = "";
            string siteName = ""; string serviceID = ""; string serviceName = ""; string fromDate = ""; string toDate = ""; string side = ""; string area = ""; string qty = "";
            string rate = ""; string totalAmount = ""; string cgst = ""; string sgst = ""; string igst = ""; string tds = ""; string discount = ""; string finalAmount = "";
            for (int i = 0; i < int.Parse(noofrows) - 1; i++)
            {
                if (Request["SaleOrderDetailsID_ " + i] != null)
                {
                    pkSaleOrderDetailsID = Request["SaleOrderDetailsID_ " + i].ToString();
                    if (pkSaleOrderDetailsID == "0")
                    {
                        siteID = Request["SiteID_ " + i].ToString();
                        siteName = Request["SiteName_ " + i].ToString();
                        serviceID = Request["ServiceID_ " + i].ToString();
                        serviceName = Request["ServiceName_ " + i].ToString();
                        vendorID = Request["VendorID_ " + i].ToString();
                        vendorName = Request["VendorName_ " + i].ToString();
                        height = Request["Height_ " + i].ToString();
                        width = Request["Width_ " + i].ToString();
                        description = Request["Description_ " + i].ToString();
                        fromDate = Common.ConvertToSystemDate(Request["FromDate_ " + i].ToString(), "dd/MM/yyyy");
                        toDate = string.IsNullOrEmpty(Request["ToDate_ " + i].ToString()) ? null : Common.ConvertToSystemDate(Request["ToDate_ " + i].ToString(), "dd/MM/yyyy");
                        side = Request["Side_ " + i].ToString();
                        area = Request["Area_ " + i].ToString();
                        qty = Request["Quantity_ " + i].ToString();
                        rate = Request["Rate_ " + i].ToString();
                        totalAmount = Request["TotalAmount_ " + i].ToString();
                        cgst = Request["CGST_ " + i].ToString();
                        sgst = Request["SGST_ " + i].ToString();
                        igst = Request["IGST_ " + i].ToString();
                        discount = Request["Discount_ " + i].ToString();
                        tds = Request["TDS_ " + i].ToString();
                        finalAmount = Request["FinalAmount_ " + i].ToString();

                        dtst.Rows.Add(pkSaleOrderDetailsID, siteID, siteName, serviceID, serviceName, vendorID, vendorName, fromDate, toDate, side, height, width, area, qty, rate, totalAmount, cgst, sgst, igst, tds, discount, finalAmount, description);
                    }
                }
            }

            model.dtSaleOrderDetails = dtst;
            model.OrderDate = string.IsNullOrEmpty(model.OrderDate) ? null : Common.ConvertToSystemDate(model.OrderDate, "dd/MM/yyyy");
            model.AddedBy = Session["UserID"].ToString();

            DataSet ds = model.SaleOrderUpdate();
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    TempData["GenerateOrder"] = "Sale Order details updated successfully";
                }
                else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                {
                    TempData["GenerateOrder"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }

            return RedirectToAction("GenerateSaleOrder");
        }

        #endregion

        #region Invoice

        public ActionResult InvoiceNo()
        {
            SaleOrder obj = new SaleOrder();
            List<SaleOrder> lst = new List<SaleOrder>();

            DataSet ds = obj.GetInvoiceNoList();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SaleOrder objInvoice = new SaleOrder();

                    objInvoice.PK_InvoiceNoID = Crypto.Encrypt(dr["PK_InvoiceNoID"].ToString());
                    objInvoice.SaleOrderNoEncrypt = Crypto.Encrypt(dr["InvoiceNo"].ToString());
                    objInvoice.InvoiceNo = dr["InvoiceNo"].ToString();
                    objInvoice.InvoiceDate = dr["InvoiceDate"].ToString();
                    objInvoice.LineStatus = dr["Status"].ToString();

                    lst.Add(objInvoice);
                }
                obj.lstInvoiceNo = lst;
            }
            return View(obj);
        }

        [HttpPost]
        [ActionName("InvoiceNo")]
        [OnAction(ButtonName = "btnGenerateInvoiceNo")]
        public ActionResult GenerateInvoiceNo(SaleOrder model)
        {
            try
            {
                model.AddedBy = Session["UserID"].ToString();
                DataSet ds = model.GenerateInvoiceNo();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Class"] = "alert alert-success";
                        TempData["InvoiceNo"] = "Invoice Number generated successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["InvoiceNo"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["InvoiceNo"] = "ERROR : " + ex.Message;
            }
            return RedirectToAction("InvoiceNo");
        }

        public ActionResult GenerateBill(string invid, string no)
        {
            SaleOrder model = new Models.SaleOrder();
            model.PK_InvoiceNoID = Crypto.Decrypt(invid);
            model.InvoiceNo = Crypto.Decrypt(no);

            return View(model);
        }

        [HttpPost]
        [ActionName("GenerateBill")]
        [OnAction(ButtonName = "btnGetCampaignDetails")]
        public ActionResult GetCampaignDetails(SaleOrder model)
        {
            try
            {
                DataSet ds = model.SalesOrderNoListForCampaign();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    List<SaleOrder> lst = new List<SaleOrder>();
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SaleOrder objsaleorder = new SaleOrder();

                        objsaleorder.ServiceName = dr["ServiceName"].ToString();
                        objsaleorder.FK_SaleOrderId = dr["PK_SalesOrderNoID"].ToString();
                        objsaleorder.SalesOrderNo = dr["SalesOrderNo"].ToString();
                        objsaleorder.OrderDate = dr["OrderDate"].ToString();
                        objsaleorder.CustomerName = dr["CustomerName"].ToString();
                        objsaleorder.CustomerMobile = dr["CustomerMobile"].ToString();
                        objsaleorder.SaleOrderNoEncrypt = Crypto.Encrypt(dr["SalesOrderNo"].ToString());
                        objsaleorder.SaleOrderIDEncrypt = Crypto.Encrypt(dr["PK_SalesOrderNoID"].ToString());
                        objsaleorder.LineStatus = dr["Status"].ToString();
                        objsaleorder.PONumber = dr["PONumber"].ToString();
                        lst.Add(objsaleorder);
                    }
                    model.lstsaleorder = lst;
                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }
        [HttpPost]
        [ActionName("GenerateBill")]
        [OnAction(ButtonName = "btnSaveDetails")]
        public ActionResult InvoiceSOMapping(SaleOrder model)
        {
            try
            {
                string noofrows = Request["hdrows"];
                string pontomatch = "";
                bool validSOSelected = false;
                #region checkifAllSOSelectedHaveSamePONumbers
                for (int i = 1; i <= int.Parse(noofrows); i++)
                {                    
                    string chk = Request["chkSO_" + i];
                    if (chk == "on")
                    {
                        if (pontomatch == "")
                        {
                            pontomatch = Request["hdPON_" + i].ToString();
                        }
                        if (pontomatch != Request["hdPON_" + i].ToString())
                        {
                            validSOSelected = false;
                            break;
                        }
                        else
                        {
                            validSOSelected = true;
                        }
                    }
                }
                #endregion
                if (validSOSelected == true)
                {
                    for (int i = 1; i <= int.Parse(noofrows); i++)
                    {
                        model.AddedBy = Session["UserID"].ToString();
                        string chk = Request["chkSO_" + i];
                        if (chk == "on")
                        {
                            model.FK_SaleOrderId = Request["hdSaleOrderID_" + i].ToString();
                            DataSet ds = model.InvoiceSOMapping();
                        }
                        TempData["Class"] = "alert alert-success";
                        TempData["InvoiceNo"] = "Mapping successfull";
                    }
                }
                else
                {
                    TempData["Class"] = "alert alert-danger";
                    TempData["InvoiceNo"] = "All SO selected must have the save PO Number";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["Class"] = "alert alert-danger";
                TempData["InvoiceNo"] = "ERROR : " + ex.Message;
            }
            return RedirectToAction("InvoiceNo");
        }

        #endregion

        #region printInvoice
        public ActionResult PrintInvoice(string InvoiceNo)
        {
            SaleOrder model = new SaleOrder();
            List<SaleOrder> lstSaleOrderDetails = new List<SaleOrder>();

            model.InvoiceNo = InvoiceNo;
            DataSet ds = model.GetPrintInvoice();


            //ViewBag.SaleOrderDate = ds.Tables[0].Rows[0]["SalesOrderNo"].ToString();
            ViewBag.InvoiceNumber = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();


            if (ds != null && ds.Tables[1].Rows.Count > 0)
            {
                ViewBag.FinalAmount = 0;
                ViewBag.CGST = ViewBag.SGST = ViewBag.IGST = 0;
                foreach (DataRow r in ds.Tables[1].Rows)
                {
                    SaleOrder obj = new SaleOrder();
                    obj.Description = r["Description"].ToString();
                    obj.Width = r["Width"].ToString();
                    obj.Height = r["Height"].ToString();
                    obj.SiteName = r["SiteName"].ToString();
                    obj.Area = r["Area"].ToString();
                    obj.Quantity = r["Quantity"].ToString();
                    obj.Rate = r["Rate"].ToString();
                    obj.TotalAmount = r["TotalAmount"].ToString();

                    ViewBag.CGST = Math.Round(Convert.ToDecimal(ViewBag.CGST) + Convert.ToDecimal(r["CGSTAmt"].ToString()), 2);
                    ViewBag.SGST = Math.Round(Convert.ToDecimal(ViewBag.SGST) + Convert.ToDecimal(r["SGSTAmt"].ToString()), 2);
                    ViewBag.IGST = Math.Round(Convert.ToDecimal(ViewBag.IGST) + Convert.ToDecimal(r["IGSTAmt"].ToString()), 2);

                    ViewBag.FinalAmount = Convert.ToDecimal(ViewBag.FinalAmount) + Convert.ToDecimal(r["TotalAmount"].ToString());
                    ViewBag.FinalAmountGST = Convert.ToDecimal(ViewBag.FinalAmount) + Convert.ToDecimal(ViewBag.SGST) + Convert.ToDecimal(ViewBag.CGST) + Convert.ToDecimal(ViewBag.IGST);
                    lstSaleOrderDetails.Add(obj);
                }
                model.lstsaleorder = lstSaleOrderDetails;
            }



            return View(model);
        }
        public ActionResult Invoice(string InvoiceNo)
        {
            SaleOrder model = new SaleOrder();
            List<SaleOrder> lstSaleOrderDetails = new List<SaleOrder>();

            ViewBag.CompanyName = CompanyProfile.CompanyName;
            ViewBag.CompanyMobile = CompanyProfile.CompanyMobile;
            ViewBag.CompanyEmail = CompanyProfile.CompanyEmail;
            ViewBag.CompanyAddress = CompanyProfile.CompanyAddress;
            ViewBag.BankName = CompanyProfile.BankName;
            ViewBag.AccountNo = CompanyProfile.AccountNo;
            ViewBag.IFSC = CompanyProfile.IFSC;
            ViewBag.GSTIN = CompanyProfile.GSTIN;
            ViewBag.PAN = CompanyProfile.PAN;
            model.InvoiceNo = InvoiceNo;
            DataSet ds = model.GetPrintInvoice();

            //ViewBag.SaleOrderDate = ds.Tables[0].Rows[0]["SalesOrderNo"].ToString();
            ViewBag.InvoiceNumber = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
           
            ViewBag.Description = ds.Tables[0].Rows[0]["Description"].ToString();
            if (ds != null && ds.Tables[1].Rows.Count > 0)
            {
                model.CustomerName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                model.CustomerAddress = ds.Tables[0].Rows[0]["Address"].ToString();
                model.InvoiceNo = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
                model.GSTIN = ds.Tables[0].Rows[0]["GSTIN"].ToString();
                model.StoreName = ds.Tables[0].Rows[0]["StoreName"].ToString();
                model.StoreCode = ds.Tables[0].Rows[0]["StoreCode"].ToString();
                model.StoreType = ds.Tables[0].Rows[0]["StoreType"].ToString();
                model.ConcernPerson1 = ds.Tables[0].Rows[0]["ConcernPerson1"].ToString();
                model.CompanyMobile = ds.Tables[0].Rows[0]["CompanyContactNo"].ToString();
                model.Email = ds.Tables[0].Rows[0]["Email"].ToString();

                ViewBag.FinalAmount = 0;
                ViewBag.TotalWithoutGST = 0;
                ViewBag.CGST = ViewBag.SGST = ViewBag.IGST = 0;
                foreach (DataRow r in ds.Tables[1].Rows)
                {
                    SaleOrder obj = new SaleOrder();
                    obj.Description = r["Description"].ToString();
                    obj.ServiceName = r["ServiceName"].ToString();
                    obj.Width = r["Width"].ToString();
                    obj.Height = r["Height"].ToString();
                    obj.SiteName = r["SiteName"].ToString();
                    obj.Area = r["Area"].ToString();
                    obj.Quantity = r["Quantity"].ToString();
                    obj.Rate = r["Rate"].ToString();  obj.HSNCode = r["HSNCode"].ToString();
                    obj.TotalAmount = r["TotalAmount"].ToString();
                    ViewBag.PONumber= r["PONumber"].ToString();
                    ViewBag.PODate = r["PODate"].ToString();
                   

                    ViewBag.TotalWithoutGST = Math.Round(Convert.ToDecimal(ViewBag.TotalWithoutGST) + Convert.ToDecimal(r["TotalAmount"].ToString()), 2);
                    ViewBag.CGST = Math.Round(Convert.ToDecimal(ViewBag.CGST) + Convert.ToDecimal(r["CGSTAmt"].ToString()), 2);
                    ViewBag.SGST = Math.Round(Convert.ToDecimal(ViewBag.SGST) + Convert.ToDecimal(r["SGSTAmt"].ToString()), 2);
                    ViewBag.IGST = Math.Round(Convert.ToDecimal(ViewBag.IGST) + Convert.ToDecimal(r["IGSTAmt"].ToString()), 2);

                  
                    //ViewBag.FinalAmountGST = Convert.ToDecimal(ViewBag.FinalAmount) + Convert.ToDecimal(ViewBag.SGST) + Convert.ToDecimal(ViewBag.CGST) + Convert.ToDecimal(ViewBag.IGST);
                    lstSaleOrderDetails.Add(obj);
                }
                ViewBag.AmountInWords = ds.Tables[2].Rows[0]["FinalAmountWords"].ToString();
                ViewBag.FinalAmount = ds.Tables[2].Rows[0]["TotalAMt"].ToString();
                model.lstsaleorder = lstSaleOrderDetails;
            }



            return View(model);
        }
        #endregion
        #region
        public ActionResult InvoicePrint(string InvoiceNo)
        {
            SaleOrder model = new SaleOrder();
            List<SaleOrder> lstSaleOrderDetails = new List<SaleOrder>();

            ViewBag.CompanyName = CompanyProfile.CompanyName;
            ViewBag.CompanyMobile = CompanyProfile.CompanyMobile;
            ViewBag.CompanyEmail = CompanyProfile.CompanyEmail;
            ViewBag.CompanyAddress = CompanyProfile.CompanyAddress;
            ViewBag.BankName = CompanyProfile.BankName;
            ViewBag.AccountNo = CompanyProfile.AccountNo;
            ViewBag.IFSC = CompanyProfile.IFSC;
            ViewBag.GSTIN = CompanyProfile.GSTIN;
            ViewBag.PAN = CompanyProfile.PAN;
            model.InvoiceNo = InvoiceNo;
            DataSet ds = model.GetPrintInvoice();

            //ViewBag.SaleOrderDate = ds.Tables[0].Rows[0]["SalesOrderNo"].ToString();
            ViewBag.InvoiceNumber = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();

            ViewBag.Description = ds.Tables[0].Rows[0]["Description"].ToString();
            if (ds != null && ds.Tables[1].Rows.Count > 0)
            {
                model.CustomerName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                model.CustomerAddress = ds.Tables[0].Rows[0]["Address"].ToString();
                model.InvoiceNo = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
                model.GSTIN = ds.Tables[0].Rows[0]["GSTIN"].ToString();
                model.StoreName = ds.Tables[0].Rows[0]["StoreName"].ToString();
                model.StoreCode = ds.Tables[0].Rows[0]["StoreCode"].ToString();
                model.StoreType = ds.Tables[0].Rows[0]["StoreType"].ToString();
                model.ConcernPerson1 = ds.Tables[0].Rows[0]["ConcernPerson1"].ToString();
                model.CompanyMobile = ds.Tables[0].Rows[0]["CompanyContactNo"].ToString();
                model.Email = ds.Tables[0].Rows[0]["Email"].ToString();

                ViewBag.FinalAmount = 0;
                ViewBag.TotalWithoutGST = 0;
                ViewBag.CGST = ViewBag.SGST = ViewBag.IGST = 0;
                foreach (DataRow r in ds.Tables[1].Rows)
                {
                    SaleOrder obj = new SaleOrder();
                    obj.Description = r["Description"].ToString();
                    obj.ServiceName = r["ServiceName"].ToString();
                    obj.Width = r["Width"].ToString();
                    obj.Height = r["Height"].ToString();
                    obj.SiteName = r["SiteName"].ToString();
                    obj.Area = r["Area"].ToString();
                    obj.Quantity = r["Quantity"].ToString();
                    obj.Rate = r["Rate"].ToString(); obj.HSNCode = r["HSNCode"].ToString();
                    obj.TotalAmount = r["TotalAmount"].ToString();
                    ViewBag.PONumber = r["PONumber"].ToString();
                    ViewBag.PODate = r["PODate"].ToString();


                    ViewBag.TotalWithoutGST = Math.Round(Convert.ToDecimal(ViewBag.TotalWithoutGST) + Convert.ToDecimal(r["TotalAmount"].ToString()), 2);
                    ViewBag.CGST = Math.Round(Convert.ToDecimal(ViewBag.CGST) + Convert.ToDecimal(r["CGSTAmt"].ToString()), 2);
                    ViewBag.SGST = Math.Round(Convert.ToDecimal(ViewBag.SGST) + Convert.ToDecimal(r["SGSTAmt"].ToString()), 2);
                    ViewBag.IGST = Math.Round(Convert.ToDecimal(ViewBag.IGST) + Convert.ToDecimal(r["IGSTAmt"].ToString()), 2);


                    //ViewBag.FinalAmountGST = Convert.ToDecimal(ViewBag.FinalAmount) + Convert.ToDecimal(ViewBag.SGST) + Convert.ToDecimal(ViewBag.CGST) + Convert.ToDecimal(ViewBag.IGST);
                    lstSaleOrderDetails.Add(obj);
                }
                ViewBag.AmountInWords = ds.Tables[2].Rows[0]["FinalAmountWords"].ToString();
                ViewBag.FinalAmount = ds.Tables[2].Rows[0]["TotalAMt"].ToString();
                model.lstsaleorder = lstSaleOrderDetails;
            }



            return View(model);
        }

        #endregion
    }
}
