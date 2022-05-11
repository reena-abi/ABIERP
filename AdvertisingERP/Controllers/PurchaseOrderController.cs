using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvertisingERP.Models;
using AdvertisingERP.Filter;
using System.Data;
using System.IO;

namespace AdvertisingERP.Controllers
{
    public class PurchaseOrderController : AdminBaseController
    {
        DataTable dt = new DataTable();

        #region SavePO
        public ActionResult PO(string purchaseno)
        {

            PO model = new PO();
            #region defaults
            List<SelectListItem> ddlPaymentTerms = Common.BindPaymentTerms();
            ViewBag.ddlPaymentTerms = ddlPaymentTerms;

            List<SelectListItem> ddlBillingSnaps = Common.BillingSnaps();
            ViewBag.ddlBillingSnaps = ddlBillingSnaps;

            List<SelectListItem> ddlPOReceived = Common.POReceived();
            ViewBag.ddlPOReceived = ddlPOReceived;

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


            List<SelectListItem> ddlsaleorderno = new List<SelectListItem>();
            ddlsaleorderno.Add(new SelectListItem { Text = "Select Sale Order", Value = "0" });
            ViewBag.ddlsaleorderno = ddlsaleorderno;
            #endregion
            model.PONumber = Crypto.Decrypt(purchaseno);

            return View(model);
        }
        public ActionResult POList()
        {
            if (TempData["GenerateOrder"] == null)
            {
                ViewBag.saverrormsg = "none";
            }
            PO obj = new PO();
            List<PO> lst = new List<PO>();

            DataSet ds = obj.PurchaseOrderNoList();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    PO objsaleorder = new PO();
                    objsaleorder.PONumber = dr["PurchasOrderNo"].ToString();
                    objsaleorder.PODate = dr["PurchaseOrderDate"].ToString();
                    objsaleorder.CampaignNumber = dr["CampaignNo"].ToString();
                    objsaleorder.SaleOrderNumber = dr["SalesOrderNo"].ToString();
                    objsaleorder.PostStatus = dr["PostStatus"].ToString();
                    objsaleorder.EncryptKey = Crypto.Encrypt(dr["PurchasOrderNo"].ToString());

                    lst.Add(objsaleorder);
                }
                obj.lstPO = lst;
            }
            return View(obj);
        }
        [HttpPost]
        [ActionName("POList")]
        [OnAction(ButtonName = "Generate")]
        public ActionResult GeneratePONO()
        {
            PO model = new PO();
            model.AddedBy = Session["UserID"].ToString();

            DataSet ds = model.GeneratePurchaseOrderNo();
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                {
                    TempData["GenerateOrder"] = ds.Tables[0].Rows[0]["PurchaseOrderNo"].ToString();

                }
                else
                {
                    TempData["GenerateOrder"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }

            return RedirectToAction("POList");
        }
        public ActionResult GetSaleOrderList()
        {
            SaleOrder obj = new SaleOrder();
            List<SaleOrder> lst = new List<SaleOrder>();
            DataSet ds = obj.SalesOrderNoList();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SaleOrder objSaleOrder = new SaleOrder();
                    objSaleOrder.SalesOrderNo = dr["SalesOrderNo"].ToString();
                    objSaleOrder.PK_SalesOrderNoID = dr["PK_SalesOrderNoID"].ToString();
                    lst.Add(objSaleOrder);
                }
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSaleOrderDetails(string saleOrderNo)
        {
            SaleOrder model = new SaleOrder();
            model.FK_SaleOrderId = saleOrderNo;

            DataSet ds = model.GetSaleOrderDetailsForPO();
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
                if (ds.Tables[1].Rows.Count > 0)
                {
                    List<SaleOrder> lstTmpData = new List<SaleOrder>();
                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow r in ds.Tables[1].Rows)
                        {
                            SaleOrder obj = new SaleOrder();
                            obj.SaleOrderDetailsID = r["PK_SalesOrderDetailsID"].ToString();
                            obj.PONumber = r["PONumber"].ToString();
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

                            lstTmpData.Add(obj);
                        }
                        model.lstsaleorder = lstTmpData;
                    }
                }
            }
            else
            {
                model.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("PO")]
        [OnAction(ButtonName = "SavePurchaseOrder")]
        public ActionResult InsertPurchaseOrder(PO model, HttpPostedFileBase postedFile)
        {
            try
            {
                string noofrows = Request["hdrows"];
                dt.Columns.Add("FK_SaleOrderDetailsID", typeof(string));
                dt.Columns.Add("FK_SiteID", typeof(string));
                dt.Columns.Add("FK_ServiceID", typeof(string));
                dt.Columns.Add("FK_VendorID", typeof(string));
                dt.Columns.Add("FromDate", typeof(string));
                dt.Columns.Add("ToDate", typeof(string));
                dt.Columns.Add("Side", typeof(string));
                dt.Columns.Add("Height", typeof(string));
                dt.Columns.Add("Width", typeof(string));
                dt.Columns.Add("Area", typeof(string));
                dt.Columns.Add("Quantity", typeof(string));
                dt.Columns.Add("Rate", typeof(string));
                dt.Columns.Add("TotalAmount", typeof(string));
                dt.Columns.Add("CGST", typeof(string));
                dt.Columns.Add("SGST", typeof(string));
                dt.Columns.Add("IGST", typeof(string));
                dt.Columns.Add("TDS", typeof(string));
                dt.Columns.Add("Discount", typeof(string));
                dt.Columns.Add("FinalAmount", typeof(string));
                dt.Columns.Add("Description", typeof(string));

                for (int i = 1; i <= int.Parse(noofrows); i++)
                {
                    string chk = Request["chkPOLine_" + i];
                    if (chk == "on")
                    {
                        DataRow dr = null;
                        dr = dt.NewRow();
                        dr["FK_SaleOrderDetailsID"] = Request["txtSaleOrderDetailsID_" + i].ToString();
                        dr["FK_SiteID"] = Request["txtSiteID_" + i].ToString();
                        dr["FK_ServiceID"] = Request["txtServiceID_" + i].ToString();
                        dr["FK_VendorID"] = Request["txtVendorID_" + i].ToString();
                        dr["FromDate"] = string.IsNullOrEmpty(Request["txtFromDate_" + i].ToString()) ? null : Common.ConvertToSystemDate(Request["txtFromDate_" + i].ToString(), "dd/MM/yyyy");
                        dr["ToDate"] = string.IsNullOrEmpty(Request["txtToDate_" + i].ToString()) ? null : Common.ConvertToSystemDate(Request["txtToDate_" + i].ToString(), "dd/MM/yyyy");
                        dr["Side"] = Request["txtSide_" + i].ToString();
                        dr["Height"] = Request["txtHeight_" + i].ToString();
                        dr["Width"] = Request["txtWidth_" + i].ToString();
                        dr["Area"] = Request["txtArea_" + i].ToString();
                        dr["Quantity"] = Request["txtQuantity_" + i].ToString();
                        dr["Rate"] = Request["txtRate_" + i].ToString();
                        dr["TotalAmount"] = Request["txtTotalAmount_" + i].ToString();
                        dr["CGST"] = Request["txtCGST_" + i].ToString();
                        dr["SGST"] = Request["txtSGST_" + i].ToString();
                        dr["IGST"] = Request["txtIGST_" + i].ToString();
                        dr["TDS"] = Request["txtTDS_" + i].ToString();
                        dr["Discount"] = Request["txtDiscount_" + i].ToString();
                        dr["FinalAmount"] = Request["txtFinalAmount_" + i].ToString();
                        dr["Description"] = Request["txtDescription_" + i].ToString();
                        dt.Rows.Add(dr);
                    }
                }

                if (postedFile != null)
                {
                    model.InvoiceImage = "../SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(Path.Combine(Server.MapPath(model.InvoiceImage)));
                }

                model.dtPODetails = dt;
                model.PODate = Common.ConvertToSystemDate(model.PODate, "dd/MM/yyyy");
                model.AddedBy = Session["UserID"].ToString();

                DataSet ds = model.InsertPurchaseOrder();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["GenerateOrder"] = "PO saved successfully";
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        TempData["PO"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        return RedirectToAction("PO");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["GenerateOrder"] = ex.Message;
            }

            return RedirectToAction("POList");
        }
        #endregion

        #region PrintPO

        public ActionResult PrintPO(string poNumber)
        {
            PO model = new PO();
            model.PONumber = Crypto.Decrypt(poNumber);
            ViewBag.CompanyName = CompanyProfile.CompanyName;
            ViewBag.CompanyMobile = CompanyProfile.CompanyMobile;
            ViewBag.CompanyEmail = CompanyProfile.CompanyEmail;
            ViewBag.CompanyAddress = CompanyProfile.CompanyAddress;
            ViewBag.BankName = CompanyProfile.BankName;
            ViewBag.AccountNo = CompanyProfile.AccountNo;
            ViewBag.IFSC = CompanyProfile.IFSC;
            ViewBag.GSTIN = CompanyProfile.GSTIN;
            ViewBag.PAN = CompanyProfile.PAN;
            List<PO> lstPO = new List<PO>();
            DataSet ds = model.POList();
            if (ds != null && ds.Tables[0].Rows.Count>0)
            {
                ViewBag.PurchasOrderNo= ds.Tables[0].Rows[0]["PurchasOrderNo"].ToString();
                ViewBag.PODate = ds.Tables[0].Rows[0]["PODate"].ToString();
                ViewBag.Description = ds.Tables[0].Rows[0]["Description"].ToString();
            }
            if (ds != null && ds.Tables[1].Rows.Count > 0)
            {
                ViewBag.VendorName = ds.Tables[1].Rows[0]["Name"].ToString();
                ViewBag.Address = ds.Tables[1].Rows[0]["Address"].ToString();
                ViewBag.GSTNo = ds.Tables[1].Rows[0]["GSTNo"].ToString();
                ViewBag.ServiceName = ds.Tables[1].Rows[0]["ServiceName"].ToString();
                foreach (DataRow r in ds.Tables[1].Rows)
                {
                    PO obj = new PO();
                    obj.Description = r["Description"].ToString();
                    obj.Width = r["Width"].ToString();
                    obj.Height = r["Height"].ToString();
                    obj.SiteName = r["SiteName"].ToString();
                    obj.Area = r["Area"].ToString();
                    obj.Quantity = r["Quantity"].ToString();
                    obj.Rate = r["Rate"].ToString(); obj.HSNCode = r["HSNCode"].ToString();
                    obj.TotalAmount = r["TotalAmount"].ToString();
                    
                    ViewBag.AmountInWords = r["FinalAmountWords"].ToString();

                    ViewBag.CGST = Math.Round(Convert.ToDecimal(ViewBag.CGST) + Convert.ToDecimal(r["CGSTAmt"].ToString()), 2);
                    ViewBag.SGST = Math.Round(Convert.ToDecimal(ViewBag.SGST) + Convert.ToDecimal(r["SGSTAmt"].ToString()), 2);
                    ViewBag.IGST = Math.Round(Convert.ToDecimal(ViewBag.IGST) + Convert.ToDecimal(r["IGSTAmt"].ToString()), 2);

                    ViewBag.FinalAmount = Convert.ToDecimal(ViewBag.FinalAmount) + Convert.ToDecimal(r["FinalAmount"].ToString());


                    lstPO.Add(obj);
                }
                model.lstPO = lstPO;
            }

            return View(model);
        }

        #endregion

        public ActionResult GetAllCampians()
        {
            Compaign obj = new Compaign();
            List<Compaign> lst = new List<Compaign>();
            DataSet ds = obj.GetCampaigns();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Compaign objSaleOrder = new Compaign();
                    objSaleOrder.CampaignNo = dr["CampaignNo"].ToString();
                    objSaleOrder.CreativeName = dr["CreativeName"].ToString();
                    lst.Add(objSaleOrder);
                }
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSaleOrder(string CampionNo)
        {
            SaleOrder obj = new SaleOrder();

            List<SelectListItem> lst = new List<SelectListItem>();
            obj.CampaignNo = CampionNo;
            DataSet ds = obj.SalesOrderNoForPO();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lst.Add(new SelectListItem { Text = dr["SalesOrderNo"].ToString() + '(' + dr["ServiceName"].ToString() + ')', Value = dr["PK_SalesOrderNoID"].ToString() });
                }
                //Value = dr["PK_SalesOrderNoID"].ToString() });
            obj.Result = "1";
            }
            obj.lstsaleoredernolist = lst;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        #region EditPO
        public ActionResult GetSaleOrderForPOUpdate(string CampionNo)
        {
            SaleOrder obj = new SaleOrder();

            List<SelectListItem> lst = new List<SelectListItem>();
            obj.CampaignNo = CampionNo;
            DataSet ds = obj.SalesOrderNoForPOUpdate();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    lst.Add(new SelectListItem { Text = dr["SalesOrderNo"].ToString() + '(' + dr["ServiceName"].ToString() + ')', Value = dr["PK_SalesOrderNoID"].ToString() });
                }
                obj.Result = "1";
            }
            obj.lstsaleoredernolist = lst;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditPO(string pon)
        {
            PO model = new PO();
            #region defaults
            List<SelectListItem> ddlPaymentTerms = Common.BindPaymentTerms();
            ViewBag.ddlPaymentTerms = ddlPaymentTerms;

            List<SelectListItem> ddlBillingSnaps = Common.BillingSnaps();
            ViewBag.ddlBillingSnaps = ddlBillingSnaps;

            List<SelectListItem> ddlPOReceived = Common.POReceived();
            ViewBag.ddlPOReceived = ddlPOReceived;

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


            List<SelectListItem> ddlsaleorderno = new List<SelectListItem>();
            ddlsaleorderno.Add(new SelectListItem { Text = "Select Sale Order", Value = "0" });
            ViewBag.ddlsaleorderno = ddlsaleorderno;
            #endregion
            try
            {
                model.PONumber = Crypto.Decrypt(pon);
                DataSet ds = model.GetPODetails();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    List<PO> lstPODetails = new List<PO>();

                    model.PK_PurchaseOrderID = ds.Tables[0].Rows[0]["PK_PurchaseOrderID"].ToString();
                    model.PODate = ds.Tables[0].Rows[0]["PODate"].ToString();
                    model.PaymentTerms = ds.Tables[0].Rows[0]["PaymentTerms"].ToString();
                    model.SaleOrderNumber = ds.Tables[0].Rows[0]["SalesOrderNo"].ToString();
                    model.SaleOrderNumberID = ds.Tables[0].Rows[0]["PK_SalesOrderNoID"].ToString();
                    model.CampaignNumber = ds.Tables[0].Rows[0]["CampaignNo"].ToString();
                    model.CampaignNoID = ds.Tables[0].Rows[0]["CampaignNo"].ToString();
                    model.CreativeName = ds.Tables[0].Rows[0]["CreativeName"].ToString();
                    model.InvoiceNo = ds.Tables[0].Rows[0]["InvoiceNo"].ToString();
                    model.InvoiceImage = ds.Tables[0].Rows[0]["InvoiceFile"].ToString();

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow r in ds.Tables[1].Rows)
                        {
                            PO obj = new PO();
                            obj.PK_PurchaseOrderDetailsID = r["PK_PurchaseOrderDetailsID"].ToString();
                            obj.FK_SaleOrderDetailsID = r["Fk_SaleOrderDetailsId"].ToString();
                            obj.SiteID = r["FK_SiteID"].ToString();
                            obj.ServiceID = r["FK_ServiceID"].ToString();
                            obj.VendorID = r["FK_VendorID"].ToString();
                            obj.SiteName = r["SiteInfo"].ToString();
                            obj.ServiceName = r["ServiceName"].ToString();
                            obj.StartDate = r["FromDate"].ToString();
                            obj.EndDate = r["ToDate"].ToString();
                            obj.Side = r["Side"].ToString();
                            obj.Height = r["Height"].ToString();
                            obj.Width = r["Width"].ToString();
                            obj.Area = r["Area"].ToString();
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
                            obj.Amt = (decimal.Parse(r["Area"].ToString()) * decimal.Parse(r["Rate"].ToString())).ToString();

                            lstPODetails.Add(obj);
                        }
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        model.VendorName = ds.Tables[2].Rows[0]["Name"].ToString();
                        model.MobileNo = ds.Tables[2].Rows[0]["Contact"].ToString();
                    }
                    model.lstPO = lstPODetails;
                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }
        [HttpPost]
        [ActionName("EditPO")]
        [OnAction(ButtonName = "PostPurchaseOrder")]
        public ActionResult EditPOAction(PO model)
        {
            try
            {
                DataTable dtPODetails = new DataTable();
                dtPODetails.Columns.Add("PK_PurchaseOrderDetailsID", typeof(string));
                dtPODetails.Columns.Add("FK_SaleOrderDetailsID", typeof(string));
                dtPODetails.Columns.Add("FK_SiteID", typeof(string));
                dtPODetails.Columns.Add("FK_ServiceID", typeof(string));
                dtPODetails.Columns.Add("FK_VendorID", typeof(string));
                dtPODetails.Columns.Add("FromDate", typeof(string));
                dtPODetails.Columns.Add("ToDate", typeof(string));
                dtPODetails.Columns.Add("Side", typeof(string));
                dtPODetails.Columns.Add("Height", typeof(string));
                dtPODetails.Columns.Add("Width", typeof(string));
                dtPODetails.Columns.Add("Area", typeof(string));
                dtPODetails.Columns.Add("Quantity", typeof(string));
                dtPODetails.Columns.Add("Rate", typeof(string));
                dtPODetails.Columns.Add("TotalAmount", typeof(string));
                dtPODetails.Columns.Add("CGST", typeof(string));
                dtPODetails.Columns.Add("SGST", typeof(string));
                dtPODetails.Columns.Add("IGST", typeof(string));
                dtPODetails.Columns.Add("TDS", typeof(string));
                dtPODetails.Columns.Add("Discount", typeof(string));
                dtPODetails.Columns.Add("FinalAmount", typeof(string));
                dtPODetails.Columns.Add("Description", typeof(string));

                string rowCount = Request["hdRowCount"].ToString();
                for (int i = 0; i < int.Parse(rowCount); i++)
                {
                    string startdate = string.IsNullOrEmpty(Request["txtStartDate_" + i].ToString()) ? null : Common.ConvertToSystemDate(Request["txtStartDate_" + i].ToString(), "dd/MM/yyyy");
                    string enddate = string.IsNullOrEmpty(Request["txtEndDate_" + i].ToString()) ? null : Common.ConvertToSystemDate(Request["txtEndDate_" + i].ToString(), "dd/MM/yyyy");

                    dtPODetails.Rows.Add(Request["hdPurchaseOrderDetailsID_" + i], Request["hdSaleOrderDetailsID_" + i], Request["hdSiteID_" + i], Request["hdServiceID_" + i], Request["hdVednorID_" + i]
                        , startdate, enddate, Request["txtSide_" + i], Request["txtQuantity_" + i], Request["txtHeight_" + i], Request["txtWidth_" + i], Request["txtArea_" + i]
                        , Request["txtRate_" + i], Request["txtAmount_" + i], Request["txtCGST_" + i], Request["txtSGST_" + i], Request["txtIGST_" + i], Request["txtTDS_" + i], Request["txtDiscount_" + i], Request["txtFinalAmount_" + i], Request["txtDescription_" + i]);

                }

                model.dtPODetails = dtPODetails;
                model.AddedBy = Session["UserID"].ToString();

                model.PODate = string.IsNullOrEmpty(model.PODate) ? null : Common.ConvertToSystemDate(model.PODate, "dd/MM/yyyy");

                DataSet ds = model.UpdatePurchaseOrder();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["UpdatePO"] = "Purchase Order details updated";
                        TempData["GenerateOrder"] = "Purchase Order details updated";
                    }
                    else if (ds.Tables[0].Rows[0]["MSG"].ToString() == "0")
                    {
                        TempData["Class"] = "alert alert-danger";
                        TempData["UpdatePO"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        TempData["GenerateOrder"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["GenerateOrder"] = ex.Message;
            }
            return RedirectToAction("POList");
        }

        public ActionResult DeletePOLine(string pid, string pon)
        {
            PO model = new PO();
            try
            {
                model.DeletedBy = Session["UserID"].ToString();
                model.PK_PurchaseOrderDetailsID = pid;

                DataSet ds = model.DeletePOLine();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        model.Result = "1";
                        model.PostStatus = "PO Line Deleted successfully";
                    }
                    else if (ds.Tables[0].Rows[0]["MSG"].ToString() == "0")
                    {
                        model.Result = "0";
                        model.PostStatus = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                model.PONumber = Crypto.Encrypt(pon);
            }
            catch (Exception ex)
            {
                model.Result = "0";
                model.PostStatus = ex.Message;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
