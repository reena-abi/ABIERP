using AdvertisingERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvertisingERP.Filter;

namespace AdvertisingERP.Controllers
{
    public class MasterController : AdminBaseController
    {

        #region SiteMasterStart
        public ActionResult SiteMaster(string SiteId)
        {
            Master obj = new Master();
            if (TempData["SiteError"] == null)
            {
                ViewBag.errormsg = "none";
                ViewBag.saverrormsg = "none";
            }
            Session["dt"] = null;
            #region BindVendor
            Common objcomm = new Common();
            List<SelectListItem> ddlVendors = new List<SelectListItem>();
            DataSet ds1 = objcomm.BindVendor();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlVendors.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlVendors.Add(new SelectListItem { Text = r["Name"].ToString(), Value = r["Pk_VendorId"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlVendors = ddlVendors;
            #endregion BindVendor

            #region BindMediaVechile

            List<SelectListItem> ddlMediaVehicle = new List<SelectListItem>();
            ds1 = objcomm.GetMediaVehicle();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlMediaVehicle.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlMediaVehicle.Add(new SelectListItem { Text = r["MediaVehicleName"].ToString(), Value = r["PK_MediaVehicleID"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlMediaVehicle = ddlMediaVehicle;
            #endregion BindMediaVechile

            #region BindMediaType

            List<SelectListItem> ddlMediaType = new List<SelectListItem>();
            ds1 = objcomm.GetMediaType();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlMediaType.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlMediaType.Add(new SelectListItem { Text = r["MediaTypeName"].ToString(), Value = r["PK_MediaTypeID"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlMediaType = ddlMediaType;
            #endregion BindMediaType
            if (SiteId != "" && SiteId != null)
            {
                ViewBag.Isvisible = "none";
                Master objmaster = new Master();
                objmaster.SiteID = Crypto.Decrypt(SiteId);
                DataSet ds = objmaster.GetSiteList();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        obj.SiteID = (dr["PK_SiteID"].ToString());
                        obj.VendorID = dr["FK_VendorID"].ToString();
                        obj.SiteName = dr["SiteName"].ToString();
                        obj.Pincode = dr["Pincode"].ToString();
                        obj.StateName = dr["State"].ToString();
                        obj.City = dr["City"].ToString();
                        obj.Location = dr["Location"].ToString();
                        obj.Facing = dr["Facing"].ToString();
                        obj.Rational = dr["Rational"].ToString();
                        obj.Height = dr["Height"].ToString();
                        obj.Width = dr["Width"].ToString();
                        obj.Quantity = dr["Quantity"].ToString();
                        obj.Side = dr["Side"].ToString();
                        obj.Area = dr["Area"].ToString();
                        obj.MediaVehicleID = dr["PK_MediaVehicleID"].ToString();
                        obj.MediaTypeID = dr["PK_MediaTypeID"].ToString();
                        obj.CartRate = dr["CartRate"].ToString();
                        obj.SiteOwner = dr["ActualSiteOwner"].ToString();
                        obj.SiteImage = dr["SiteImage"].ToString();
                        obj.Comments = dr["Comments"].ToString();
                    }
                }
            }
            else
            {
                ViewBag.Isvisible = "";
            }


            return View(obj);
        }

        [HttpPost]
        [ActionName("SiteMaster")]
        [OnAction(ButtonName = "addSite")]
        //string VendorID, string SiteName, string Pincode, string StateName, string City, string Facing, string Rational, string Height, string Width, string Quantity, string Side, string Area, string MediaVehicleID, string MediaTypeID, string CartRate, string SiteOwner,
        public ActionResult SaveSiteTemp(Master model, HttpPostedFileBase postedFile)
        {
            try
            {
                if (TempData["SiteError"] == null)
                {
                    ViewBag.errormsg = "none";
                    ViewBag.saverrormsg = "none";
                }
                if (postedFile != null)
                {
                    model.SiteImage = "../assets/SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(postedFile.FileName);

                    //model.SiteImage = "/SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(postedFile.FileName);

                    postedFile.SaveAs(Path.Combine(Server.MapPath(model.SiteImage)));
                }
                DataTable dt = new DataTable();

                if (Session["dt"] != null)
                {
                    dt = (DataTable)Session["dt"];

                    DataRow dr = null;
                    dr = dt.NewRow();
                    dr["FK_VendorID"] = model.VendorID;
                    dr["SiteName"] = model.SiteName;
                    dr["Pincode"] = model.Pincode;
                    dr["State"] = model.StateName;
                    dr["City"] = model.City;
                    dr["Location"] = model.Location;
                    dr["Facing"] = model.Facing;
                    dr["Rational"] = model.Rational;
                    dr["Height"] = model.Height;
                    dr["Width"] = model.Width;
                    dr["Quantity"] = model.Quantity;
                    dr["Side"] = model.Side;
                    dr["Area"] = model.Area;
                    dr["MediaVehicle"] = model.MediaVehicleID;
                    dr["MediaType"] = model.MediaTypeID;
                    dr["CartRate"] = model.CartRate;
                    dr["ActualSiteOwner"] = model.SiteOwner;
                    dr["SiteImage"] = model.SiteImage == null ? null : model.SiteImage;
                    dr["Comments"] = model.Comments;
                    dr["IsDummySite"] = Request["chkDummySite"] != null ? "1" : "0";
                    dr["IsBrandingSite"] = Request["chkBrandingSite"] != null ? "1" : "0";

                    dt.Rows.Add(dr);

                    Session["dt"] = dt;
                }
                else
                {
                    dt.Columns.Add("FK_VendorID", typeof(string));
                    dt.Columns.Add("SiteName", typeof(string));
                    dt.Columns.Add("Pincode", typeof(string));
                    dt.Columns.Add("State", typeof(string));
                    dt.Columns.Add("City", typeof(string));
                    dt.Columns.Add("Location", typeof(string));
                    dt.Columns.Add("Facing", typeof(string));
                    dt.Columns.Add("Rational", typeof(string));
                    dt.Columns.Add("Height", typeof(string));
                    dt.Columns.Add("Width", typeof(string));
                    dt.Columns.Add("Quantity", typeof(string));
                    dt.Columns.Add("Side", typeof(string));
                    dt.Columns.Add("Area", typeof(string));
                    dt.Columns.Add("MediaVehicle", typeof(string));
                    dt.Columns.Add("MediaType", typeof(string));
                    dt.Columns.Add("CartRate", typeof(string));
                    dt.Columns.Add("ActualSiteOwner", typeof(string));
                    dt.Columns.Add("SiteImage", typeof(string));
                    dt.Columns.Add("Comments", typeof(string));
                    dt.Columns.Add("IsDummySite", typeof(string));
                    dt.Columns.Add("IsBrandingSite", typeof(string));

                    DataRow dr = null;
                    dr = dt.NewRow();
                    dr["FK_VendorID"] = model.VendorID;
                    dr["SiteName"] = model.SiteName;
                    dr["Pincode"] = model.Pincode;
                    dr["State"] = model.StateName;
                    dr["City"] = model.City;
                    dr["Location"] = model.Location;
                    dr["Facing"] = model.Facing;
                    dr["Rational"] = model.Rational;
                    dr["Height"] = model.Height;
                    dr["Width"] = model.Width;
                    dr["Quantity"] = model.Quantity;
                    dr["Side"] = model.Side;
                    dr["Area"] = model.Area;
                    dr["MediaVehicle"] = model.MediaVehicleID;
                    dr["MediaType"] = model.MediaTypeID;
                    dr["CartRate"] = model.CartRate;
                    dr["ActualSiteOwner"] = model.SiteOwner;
                    dr["SiteImage"] = model.SiteImage == null ? null : model.SiteImage;
                    dr["Comments"] = model.Comments;
                    dr["IsDummySite"] = Request["chkDummySite"] != null ? "1" : "0";
                    dr["IsBrandingSite"] = Request["chkBrandingSite"] != null ? "1" : "0";

                    dt.Rows.Add(dr);
                    Session["dt"] = dt;
                }

                List<Master> lst = new List<Master>();
                dt = (DataTable)Session["dt"];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Master obj = new Master();
                        obj.VendorID = dr["FK_VendorID"].ToString();
                        obj.SiteName = dr["SiteName"].ToString();
                        obj.Pincode = dr["Pincode"].ToString();
                        obj.StateName = dr["State"].ToString();
                        obj.City = dr["City"].ToString();
                        obj.Location = dr["Location"].ToString();
                        obj.Facing = dr["Facing"].ToString();
                        obj.Rational = dr["Rational"].ToString();
                        obj.Height = dr["Height"].ToString();
                        obj.Width = dr["Width"].ToString();
                        obj.Quantity = dr["Quantity"].ToString();
                        obj.Side = dr["Side"].ToString();
                        obj.Area = dr["Area"].ToString();
                        obj.MediaVehicleID = dr["MediaVehicle"].ToString();
                        obj.MediaTypeID = dr["MediaType"].ToString();
                        obj.CartRate = dr["CartRate"].ToString();
                        obj.SiteOwner = dr["ActualSiteOwner"].ToString();
                        obj.SiteImage = dr["SiteImage"].ToString();
                        obj.IsDummySite = dr["IsDummySite"].ToString();
                        obj.IsBrandingSite = dr["IsBrandingSite"].ToString();

                        lst.Add(obj);
                    }
                    model.lstSites = lst;


                }
                #region BindVendor
                Common objcomm = new Common();
                List<SelectListItem> ddlVendors = new List<SelectListItem>();
                ddlVendors.Add(new SelectListItem { Text = "Select", Value = "0" });
                DataSet ds1 = objcomm.BindVendor();
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    int count = 0;
                    foreach (DataRow r in ds1.Tables[0].Rows)
                    {
                        ddlVendors.Add(new SelectListItem { Text = r["Name"].ToString(), Value = r["Pk_VendorId"].ToString() });
                        count++;
                    }
                }

                ViewBag.ddlVendors = ddlVendors;
                #endregion BindVendor

                #region BindMediaVechile

                List<SelectListItem> ddlMediaVehicle = new List<SelectListItem>();
                ds1 = objcomm.GetMediaVehicle();
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    int count = 0;
                    foreach (DataRow r in ds1.Tables[0].Rows)
                    {
                        if (count == 0)
                        {
                            ddlMediaVehicle.Add(new SelectListItem { Text = "Select", Value = "0" });
                        }
                        ddlMediaVehicle.Add(new SelectListItem { Text = r["MediaVehicleName"].ToString(), Value = r["PK_MediaVehicleID"].ToString() });
                        count++;
                    }
                }

                ViewBag.ddlMediaVehicle = ddlMediaVehicle;
                #endregion BindMediaVechile

                #region BindMediaType

                List<SelectListItem> ddlMediaType = new List<SelectListItem>();
                ds1 = objcomm.GetMediaType();
                if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    int count = 0;
                    foreach (DataRow r in ds1.Tables[0].Rows)
                    {
                        if (count == 0)
                        {
                            ddlMediaType.Add(new SelectListItem { Text = "Select", Value = "0" });
                        }
                        ddlMediaType.Add(new SelectListItem { Text = r["MediaTypeName"].ToString(), Value = r["PK_MediaTypeID"].ToString() });
                        count++;
                    }
                }

                ViewBag.ddlMediaType = ddlMediaType;
                #endregion BindMediaType
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        public ActionResult GetStateCity(string PinCode)
        {
            Common obj = new Common();
            obj.Pincode = PinCode;
            DataSet ds = obj.GetStateCity();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                obj.StateName = ds.Tables[0].Rows[0]["StateName"].ToString();
                obj.City = ds.Tables[0].Rows[0]["CityName"].ToString();
                obj.Result = "1";
            }
            else
            {
                obj.Result = "Invalid PinCode";
            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ActionName("SiteMaster")]
        [OnAction(ButtonName = "SaveSite")]
        public ActionResult SaveSite(Master obj)
        {
            #region BindVendor
            Common objcomm = new Common();
            List<SelectListItem> ddlVendors = new List<SelectListItem>();
            DataSet ds1 = objcomm.BindVendor();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlVendors.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlVendors.Add(new SelectListItem { Text = r["Name"].ToString(), Value = r["Pk_VendorId"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlVendors = ddlVendors;
            #endregion BindVendor

            #region BindMediaVechile

            List<SelectListItem> ddlMediaVehicle = new List<SelectListItem>();
            ds1 = objcomm.GetMediaVehicle();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlMediaVehicle.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlMediaVehicle.Add(new SelectListItem { Text = r["MediaVehicleName"].ToString(), Value = r["PK_MediaVehicleID"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlMediaVehicle = ddlMediaVehicle;
            #endregion BindMediaVechile

            #region BindMediaType

            List<SelectListItem> ddlMediaType = new List<SelectListItem>();
            ds1 = objcomm.GetMediaType();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlMediaType.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlMediaType.Add(new SelectListItem { Text = r["MediaTypeName"].ToString(), Value = r["PK_MediaTypeID"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlMediaType = ddlMediaType;
            #endregion BindMediaType
            if (TempData["SiteError"] == null)
            {
                ViewBag.errormsg = "none";
                ViewBag.saverrormsg = "none";
            }
            if (Session["dt"] == null)
            {
                TempData["SiteError"] = "Please Add Atleast One Site.";
                ViewBag.errormsg = "";
                ViewBag.saverrormsg = "none";
                return View();
            }
            try
            {
                obj.dtSiteMaster = (DataTable)Session["dt"];
                obj.AddedBy = Session["UserID"].ToString();

                DataSet ds = new DataSet();

                ds = obj.SaveSite();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        Session["dt"] = null;
                        TempData["Sitesuccess"] = "Site Saved Successfully";
                        ViewBag.saverrormsg = "";
                        ViewBag.errormsg = "none";
                    }
                    else
                    {
                        TempData["SiteError"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        ViewBag.errormsg = "";
                        ViewBag.saverrormsg = "none";

                    }
                }
            }
            catch (Exception ex)
            {
                TempData["SiteError"] = ex.Message;
                ViewBag.errormsg = "";
                ViewBag.saverrormsg = "none";
            }


            return RedirectToAction("SiteMaster");
        }

        public ActionResult SiteList()
        {
            if (TempData["SiteDelete"] == null)
            {
                ViewBag.saverrormsg = "none";
            }
            #region BindVendor
            Common objcomm = new Common();
            List<SelectListItem> ddlVendors = new List<SelectListItem>();
            DataSet ds1 = objcomm.BindVendor();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlVendors.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlVendors.Add(new SelectListItem { Text = r["Name"].ToString(), Value = r["Pk_VendorId"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlVendors = ddlVendors;
            #endregion BindVendor

            #region BindMediaVechile

            List<SelectListItem> ddlMediaVehicle = new List<SelectListItem>();
            ds1 = objcomm.GetMediaVehicle();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlMediaVehicle.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlMediaVehicle.Add(new SelectListItem { Text = r["MediaVehicleName"].ToString(), Value = r["PK_MediaVehicleID"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlMediaVehicle = ddlMediaVehicle;
            #endregion BindMediaVechile

            #region BindMediaType

            List<SelectListItem> ddlMediaType = new List<SelectListItem>();
            ds1 = objcomm.GetMediaType();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlMediaType.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlMediaType.Add(new SelectListItem { Text = r["MediaTypeName"].ToString(), Value = r["PK_MediaTypeID"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlMediaType = ddlMediaType;
            #endregion BindMediaType
            return View();
        }
        [HttpPost]
        [ActionName("SiteList")]
        [OnAction(ButtonName = "GetDetails")]
        public ActionResult GetSiteList(Master objmaster)
        {
            if (TempData["SiteDelete"] == null)
            {
                ViewBag.saverrormsg = "none";
            }
            #region BindVendor
            Common objcomm = new Common();
            List<SelectListItem> ddlVendors = new List<SelectListItem>();
            DataSet ds1 = objcomm.BindVendor();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlVendors.Add(new SelectListItem { Text = "All", Value = "0" });
                    }
                    ddlVendors.Add(new SelectListItem { Text = r["Name"].ToString(), Value = r["Pk_VendorId"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlVendors = ddlVendors;
            #endregion BindVendor

            #region BindMediaVechile

            List<SelectListItem> ddlMediaVehicle = new List<SelectListItem>();
            ds1 = objcomm.GetMediaVehicle();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlMediaVehicle.Add(new SelectListItem { Text = "All", Value = "0" });
                    }
                    ddlMediaVehicle.Add(new SelectListItem { Text = r["MediaVehicleName"].ToString(), Value = r["PK_MediaVehicleID"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlMediaVehicle = ddlMediaVehicle;
            #endregion BindMediaVechile

            #region BindMediaType

            List<SelectListItem> ddlMediaType = new List<SelectListItem>();
            ds1 = objcomm.GetMediaType();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlMediaType.Add(new SelectListItem { Text = "All", Value = "0" });
                    }
                    ddlMediaType.Add(new SelectListItem { Text = r["MediaTypeName"].ToString(), Value = r["PK_MediaTypeID"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlMediaType = ddlMediaType;
            #endregion BindMediaType

            List<Master> lst = new List<Master>();
            objmaster.MediaTypeID = objmaster.MediaTypeID == "0" ? null : objmaster.MediaTypeID;
            objmaster.MediaVehicleID = objmaster.MediaVehicleID == "0" ? null : objmaster.MediaVehicleID;
            objmaster.VendorID = objmaster.VendorID == "0" ? null : objmaster.VendorID;
            DataSet ds = objmaster.GetSiteList();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Master obj = new Master();
                    obj.SiteID = Crypto.Encrypt(dr["PK_SiteID"].ToString());
                    obj.VendorID = dr["FK_VendorID"].ToString();
                    obj.SiteName = dr["SiteName"].ToString();
                    obj.Pincode = dr["Pincode"].ToString();
                    obj.StateName = dr["State"].ToString();
                    obj.City = dr["City"].ToString();
                    obj.Location = dr["Location"].ToString();
                    obj.Facing = dr["Facing"].ToString();
                    obj.Rational = dr["Rational"].ToString();
                    obj.Height = dr["Height"].ToString();
                    obj.Width = dr["Width"].ToString();
                    obj.Quantity = dr["Quantity"].ToString();
                    obj.Side = dr["Side"].ToString();
                    obj.Area = dr["Area"].ToString();
                    obj.MediaVehicleName = dr["MediaVehicleName"].ToString();
                    obj.MediaTypeName = dr["MediaTypeName"].ToString();
                    obj.CartRate = dr["CartRate"].ToString();
                    obj.SiteOwner = dr["ActualSiteOwner"].ToString();
                    obj.SiteImage = dr["SiteImage"].ToString();
                    lst.Add(obj);
                }
                objmaster.lstSites = lst;
            }
            return View(objmaster);
        }

        [HttpPost]
        [ActionName("SiteMaster")]
        [OnAction(ButtonName = "UpdateSite")]
        public ActionResult UpdateSite(Master obj, HttpPostedFileBase postedFile)
        {
            ViewBag.saverrormsg = "none";
            ViewBag.errormsg = "none";
            if (postedFile != null)
            {
                obj.SiteImage = "../SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(Path.Combine(Server.MapPath(obj.SiteImage)));
            }
            #region BindVendor
            Common objcomm = new Common();
            List<SelectListItem> ddlVendors = new List<SelectListItem>();
            DataSet ds1 = objcomm.BindVendor();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlVendors.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlVendors.Add(new SelectListItem { Text = r["Name"].ToString(), Value = r["Pk_VendorId"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlVendors = ddlVendors;
            #endregion BindVendor

            #region BindMediaVechile

            List<SelectListItem> ddlMediaVehicle = new List<SelectListItem>();
            ds1 = objcomm.GetMediaVehicle();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlMediaVehicle.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlMediaVehicle.Add(new SelectListItem { Text = r["MediaVehicleName"].ToString(), Value = r["PK_MediaVehicleID"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlMediaVehicle = ddlMediaVehicle;
            #endregion BindMediaVechile

            #region BindMediaType

            List<SelectListItem> ddlMediaType = new List<SelectListItem>();
            ds1 = objcomm.GetMediaType();
            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                foreach (DataRow r in ds1.Tables[0].Rows)
                {
                    if (count == 0)
                    {
                        ddlMediaType.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlMediaType.Add(new SelectListItem { Text = r["MediaTypeName"].ToString(), Value = r["PK_MediaTypeID"].ToString() });
                    count++;
                }
            }

            ViewBag.ddlMediaType = ddlMediaType;
            #endregion BindMediaType

            if (TempData["SiteError"] == null)
            {
                ViewBag.errormsg = "none";
                ViewBag.saverrormsg = "none";
            }

            try
            {

                obj.UpdatedBy = Session["UserID"].ToString();
                obj.SiteID = Crypto.Decrypt(obj.SiteID);
                DataSet ds = new DataSet();


                ds = obj.UpdateSite();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        Session["dt"] = null;
                        TempData["Sitesuccess"] = "Site Updated Successfully";
                        ViewBag.saverrormsg = "";
                        ViewBag.errormsg = "none";
                    }
                    else
                    {
                        TempData["SiteError"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        ViewBag.errormsg = "";
                        ViewBag.saverrormsg = "none";

                    }
                }
            }
            catch (Exception ex)
            {
                TempData["SiteError"] = ex.Message;
                ViewBag.errormsg = "";
                ViewBag.saverrormsg = "none";
            }


            return RedirectToAction("SiteMaster");
        }

        public ActionResult DeleteSite(string SiteId)
        {
            Master obj = new Master();
            try
            {

                obj.DeletedBy = Session["UserID"].ToString();
                obj.SiteID = Crypto.Decrypt(SiteId);
                DataSet ds = new DataSet();


                ds = obj.DeleteSite();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {

                        TempData["SiteDelete"] = "Site Deleted Successfully";


                    }
                    else
                    {
                        TempData["SiteDelete"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();


                    }
                }
            }
            catch (Exception ex)
            {
                TempData["SiteDelete"] = ex.Message;

            }
            ViewBag.saverrormsg = "";
            return RedirectToAction("SiteList");
        }
        #endregion SiteMaster Start
        #region ServiceMasterStart
        public ActionResult ServiceMaster(string ServiceId)
        {
            #region ddldateformat
            List<SelectListItem> ddldateformat = Common.BindDateFormat();
            ViewBag.ddldateformat = ddldateformat;
            #endregion ddldateformat
            Master obj = new Master();
            if (TempData["ServiceError"] == null)
            {
                ViewBag.errormsg = "none";

            }


            if ((ServiceId) != "" && ServiceId != null)
            {
                ViewBag.Isvisible = "none";
                Master objmaster = new Master();
                objmaster.ServiceId = Crypto.Decrypt(ServiceId);
                DataSet ds = objmaster.GetServiceList();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        obj.ServiceId = (dr["PK_ServiceId"].ToString());
                        obj.ServiceName = dr["ServiceName"].ToString();
                        obj.HSNCode = dr["HSNCode"].ToString();
                        obj.CGST = dr["GST"].ToString();
                        obj.IGST = dr["IGST"].ToString();
                        obj.SGST = dr["SGST"].ToString();
                        obj.DateFormat = dr["DateFormat"].ToString();
                    }
                }
            }
            else
            {
                ViewBag.Isvisible = "";
            }


            return View(obj);
        }

        [HttpPost]
        [ActionName("ServiceMaster")]
        [OnAction(ButtonName = "SaveService")]
        public ActionResult SaveService(Master obj)
        {

            if (TempData["ServiceError"] == null)
            {
                ViewBag.errormsg = "none";

            }

            try
            {


                obj.AddedBy = Session["UserID"].ToString();

                DataSet ds = new DataSet();


                ds = obj.SaveService();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {

                        TempData["ServiceSuccess"] = "Service Save Successfully";

                    }
                    else
                    {
                        TempData["ServiceError"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();


                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ServiceError"] = ex.Message;

            }
            ViewBag.errormsg = "";

            return RedirectToAction("ServiceMaster");
        }

        public ActionResult ServiceList()
        {
            if (TempData["ServiceDelete"] == null)
            {
                ViewBag.saverrormsg = "none";
            }

            return View();
        }
        [HttpPost]
        [ActionName("ServiceList")]
        [OnAction(ButtonName = "GetDetails")]
        public ActionResult GetServiceList(Master objmaster)
        {
            if (TempData["SiteDelete"] == null)
            {
                ViewBag.saverrormsg = "none";
            }


            List<Master> lst = new List<Master>();

            DataSet ds = objmaster.GetServiceList();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Master obj = new Master();
                    obj.ServiceId = Crypto.Encrypt(dr["PK_ServiceId"].ToString());
                    obj.ServiceName = dr["ServiceName"].ToString();
                    obj.HSNCode = dr["HSNCode"].ToString();
                    obj.CGST = dr["GST"].ToString();
                    obj.IGST = dr["IGST"].ToString();
                    obj.SGST = dr["SGST"].ToString();
                    obj.DateFormat = dr["DateFormat"].ToString();
                    lst.Add(obj);
                }
                objmaster.lstSites = lst;
            }
            return View(objmaster);
        }

        [HttpPost]
        [ActionName("ServiceMaster")]
        [OnAction(ButtonName = "UpdateService")]
        public ActionResult UpdateService(Master obj)
        {
            ViewBag.errormsg = "none";

            if (TempData["ServiceError"] == null)
            {
                ViewBag.errormsg = "none";

            }

            try
            {

                obj.UpdatedBy = Session["UserID"].ToString();
                obj.ServiceId = Crypto.Decrypt(obj.ServiceId);
                DataSet ds = new DataSet();


                ds = obj.UpdateService();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        Session["dt"] = null;
                        TempData["ServiceSuccess"] = "Service Updated Successfully";


                    }
                    else
                    {
                        TempData["ServiceError"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();


                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ServiceError"] = ex.Message;

            }

            ViewBag.errormsg = "";
            return RedirectToAction("ServiceMaster");
        }

        public ActionResult DeleteService(string ServiceId)
        {
            Master obj = new Master();
            try
            {
                obj.DeletedBy = Session["UserID"].ToString();
                obj.ServiceId = Crypto.Decrypt(ServiceId);
                DataSet ds = new DataSet();

                ds = obj.DeleteService();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["ServiceDelete"] = "Service Deleted Successfully";
                    }
                    else
                    {
                        TempData["ServiceDelete"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ServiceDelete"] = ex.Message;
            }

            ViewBag.saverrormsg = "";
            return RedirectToAction("ServiceList");
        }

        #endregion ServiceMaster End
        #region FinancialYear

        public ActionResult FinancialYear(Master model)
        {
            return View(model);
        }

        [HttpPost]
        [ActionName("FinancialYear")]
        [OnAction(ButtonName = "SaveFinancialYear")]
        public ActionResult SaveFinancialYear(Master model)
        {
            try
            {
                model.AddedBy = Session["UserID"].ToString();
                DataSet ds = model.SaveFinancialYear();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        TempData["Financialsuccess"] = "Financial Year saved successfully";
                    }
                    else if (ds.Tables[0].Rows[0]["MSG"].ToString() == "0")
                    {
                        TempData["Financialerror"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Financialerror"] = ex.Message;
            }
            return RedirectToAction("FinancialYear");
        }

        public ActionResult FinancialYearList()
        {
            Master model = new Master();
            try
            {
                DataSet ds = model.GetFinancialYearList();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    List<Master> lstFinancialYear = new List<Master>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Master obj = new Master();
                        obj.FinancialYearID = r["PK_FinancialYearID"].ToString();
                        obj.FinancialYearName = r["FinancialYear"].ToString();
                        obj.FinancialYearStatus = r["Status"].ToString();
                        lstFinancialYear.Add(obj);

                    }
                    model.lstFinancialYear = lstFinancialYear;
                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        public ActionResult ActivateFinancialYear(string fid)
        {
            try
            {
                Master model = new Master();
                model.FinancialYearID = fid;
                model.AddedBy = Session["UserID"].ToString();

                DataSet ds = model.ActivateFinancialYear();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        TempData["Financialsuccess"] = "Financial year updated successfully";
                    }
                    else if (ds.Tables[0].Rows[0]["MSG"].ToString() == "0")
                    {
                        TempData["Financialerror"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Financialerror"] = ex.Message;
            }
            return RedirectToAction("FinancialYearList");
        }

        //public ActionResult DeleteFinancialYear(string FinancialId)
        //{
        //    Master obj = new Master();
        //    try
        //    {
        //        obj.DeletedBy = Session["UserID"].ToString();
        //        obj.FinancialYearID = Crypto.Decrypt(FinancialId);
        //        DataSet ds = new DataSet();

        //        ds = obj.DeleteFinancialYear();
        //        if (ds != null && ds.Tables.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows[0][0].ToString() == "1")
        //            {
        //                TempData["FinancialYearDelete"] = "Financial Year Deleted Successfully";
        //            }
        //            else
        //            {
        //                TempData["FinancialYearDelete"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["FinancialYearDelete"] = ex.Message;
        //    }

        //    ViewBag.saverrormsg = "";
        //    return RedirectToAction("FinancialYearList");
        //}

        #endregion
        #region BankMaster

        public ActionResult BankMaster(string bid)
        {
            Master model = new Master();
            if (bid != null)
            {
                model.BankID = Crypto.Decrypt(bid);
                DataSet ds = model.GetBankList();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    model.BankName = ds.Tables[0].Rows[0]["BankName"].ToString();
                    model.IFSCPrefix = ds.Tables[0].Rows[0]["PrefixIFSCCode"].ToString();
                }
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("BankMaster")]
        [OnAction(ButtonName = "SaveBank")]
        public ActionResult SaveBank(Master model)
        {
            try
            {
                model.AddedBy = Session["UserID"].ToString();
                DataSet ds = model.SaveBank();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        TempData["Banksuccess"] = "Bank saved successfully";
                    }
                    else if (ds.Tables[0].Rows[0]["MSG"].ToString() == "0")
                    {
                        TempData["Bankerror"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Bankerror"] = ex.Message;
            }
            return RedirectToAction("BankMaster");
        }

        public ActionResult BankList()
        {
            Master model = new Master();
            try
            {
                DataSet ds = model.GetBankList();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    List<Master> lstBanks = new List<Master>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Master obj = new Master();
                        obj.EncryptKey = Crypto.Encrypt(r["Pk_BankId"].ToString());
                        obj.BankID = r["Pk_BankId"].ToString();
                        obj.BankName = r["BankName"].ToString();
                        obj.IFSCPrefix = r["PrefixIFSCCode"].ToString();
                        lstBanks.Add(obj);

                    }
                    model.lstBanks = lstBanks;
                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        [HttpPost]
        [ActionName("BankMaster")]
        [OnAction(ButtonName = "UpdateBank")]
        public ActionResult UpdateBank(Master model)
        {
            try
            {
                model.AddedBy = Session["UserID"].ToString();
                DataSet ds = model.UpdateBank();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        TempData["Banksuccess"] = "Bank updated successfully";
                    }
                    else if (ds.Tables[0].Rows[0]["MSG"].ToString() == "0")
                    {
                        TempData["Bankerror"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Bankerror"] = ex.Message;
            }
            return RedirectToAction("BankMaster");
        }

        public ActionResult DeleteBank(string bid)
        {
            Master obj = new Master();
            try
            {
                obj.DeletedBy = Session["UserID"].ToString();
                obj.BankID = Crypto.Decrypt(bid);
                DataSet ds = new DataSet();

                ds = obj.DeleteBank();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["BankDelete"] = "Bank Deleted Successfully";
                    }
                    else
                    {
                        TempData["BankDelete"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["BankDelete"] = ex.Message;
            }

            ViewBag.saverrormsg = "";
            return RedirectToAction("BankList");
        }

        #endregion
        #region Company Master --Get
        public ActionResult CompanyMaster(string id)
        {
            Master model = new Master();
            if (id != null)
            {
                model.Pk_CompanyID = Crypto.Decrypt(id);
                DataSet ds = model.GetCompanyList();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    model.Pk_CompanyID = ds.Tables[0].Rows[0]["Pk_CompanyID"].ToString();
                    model.CompanyName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                }
            }
            return View(model);
            
        }
        [HttpPost]
        [ActionName("CompanyMaster")]
        [OnAction(ButtonName = "SaveCompany")]
        public ActionResult CompanyMaster(Master model)
        {
            try
            {
                model.AddedBy = Session["UserID"].ToString();
                DataSet ds = model.SaveCompany();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        TempData["success"] = "Company saved successfully";
                    }
                    else if (ds.Tables[0].Rows[0]["MSG"].ToString() == "0")
                    {
                        TempData["error"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction("CompanyMaster");
        }
        public ActionResult CompanyList()
        {
            Master model = new Master();
            try
            {
                DataSet ds = model.GetCompanyList();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    List<Master> lstCompany = new List<Master>();
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        Master obj = new Master();
                        obj.EncryptKey = Crypto.Encrypt(r["Pk_CompanyID"].ToString());
                        obj.Pk_CompanyID = Crypto.Encrypt(r["Pk_CompanyID"].ToString());
                        obj.CompanyName = r["CompanyName"].ToString();
                       
                        lstCompany.Add(obj);

                    }
                    model.lstcompany = lstCompany;
                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }
        public ActionResult DeleteCompany(string id)
        {
            Master obj = new Master();
            try
            {
                obj.DeletedBy = Session["UserID"].ToString();
                obj.Pk_CompanyID = Crypto.Decrypt(id);
                DataSet ds = new DataSet();

                ds = obj.DeleteCompany();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["Success"] = "Company  Deleted Successfully";
                    }
                    else
                    {
                        TempData["error"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }

            ViewBag.saverrormsg = "";
            return RedirectToAction("CompanyList");
        }
        [HttpPost]
        [ActionName("CompanyMaster")]
        [OnAction(ButtonName = "Updatecompany")]
        public ActionResult UpdateCompany(Master model)
        {
             
            try
            {
                model.UpdatedBy = Session["UserID"].ToString();
                DataSet ds = model.UpdateCompany();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        TempData["success"] = "Company updated successfully";
                    }
                    else if (ds.Tables[0].Rows[0]["MSG"].ToString() == "0")
                    {
                        TempData["error"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return RedirectToAction("CompanyMaster");
        }
        #endregion
        public ActionResult DataTable()
        {
            return View();
        }
       
    }
}
