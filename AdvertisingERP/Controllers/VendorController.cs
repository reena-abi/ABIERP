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
    public class VendorController : AdminBaseController
    {
        public ActionResult GetAllBanks()
        {
            Vendor obj = new Vendor();
            List<Vendor> lst = new List<Vendor>();
            
            DataSet ds = obj.GetBankList();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Vendor objVendor = new Vendor();
                    objVendor.BankName = dr["BankName"].ToString();
                    objVendor.IFSCCode = dr["PrefixIFSCCode"].ToString();
                    lst.Add(objVendor);
                }
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddVendor(string VendorId)
        {
            Master model = new Master();
            int count3 = 0;
            List<SelectListItem> ddlServiceType = new List<SelectListItem>();
            DataSet dsServiceList = model.GetServiceList();
            if (dsServiceList != null && dsServiceList.Tables.Count > 0 && dsServiceList.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsServiceList.Tables[0].Rows)
                {
                    if (count3 == 0)
                    {
                        ddlServiceType.Add(new SelectListItem { Text = "Select Service Type", Value = "0" });
                    }
                    ddlServiceType.Add(new SelectListItem { Text = r["ServiceName"].ToString(), Value = r["PK_ServiceId"].ToString() });
                    count3 = count3 + 1;
                }
            }
            ViewBag.ddlServiceType = ddlServiceType;

            Vendor objVendor = new Vendor();

            if (TempData["VendorError"] == null)
            {
                ViewBag.errormsg = "none";
            }
            if (VendorId != null && VendorId != "")
            {

                objVendor.VendorId =(VendorId);
                DataSet ds = objVendor.GetAllVendors();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                    objVendor.VendorId = ds.Tables[0].Rows[0]["VendorCode"].ToString();
                    objVendor.VendorCode = ds.Tables[0].Rows[0]["VendorCode"].ToString();
                    objVendor.VendorName = ds.Tables[0].Rows[0]["NameForEdit"].ToString();
                    objVendor.NatureOfBusiness = ds.Tables[0].Rows[0]["NatureOfBusiness"].ToString();
                    objVendor.MobileNo = ds.Tables[0].Rows[0]["Contact"].ToString();

                    objVendor.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                    objVendor.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    objVendor.Pincode = ds.Tables[0].Rows[0]["Pincode"].ToString();
                    objVendor.StateName = ds.Tables[0].Rows[0]["State"].ToString();
                    objVendor.City = ds.Tables[0].Rows[0]["City"].ToString();
                    objVendor.GSTNO = ds.Tables[0].Rows[0]["GSTNo"].ToString();

                    objVendor.BankName = ds.Tables[0].Rows[0]["BankName"].ToString();
                    objVendor.AccountNo = ds.Tables[0].Rows[0]["AccountNumber"].ToString();
                    objVendor.IFSCCode = ds.Tables[0].Rows[0]["IFSCCode"].ToString();
                    objVendor.ConcernPerson = ds.Tables[0].Rows[0]["ConcernPersonName"].ToString();
                    objVendor.ConcerPersonContact = ds.Tables[0].Rows[0]["ConcernPersonContact"].ToString();
                    objVendor.ConcerPersonEmail = ds.Tables[0].Rows[0]["ConcernPersonEmail"].ToString();
                    objVendor.ConcernPerson2 = ds.Tables[0].Rows[0]["ConcernPersonName2"].ToString();
                    objVendor.ConcerPersonContact2 = ds.Tables[0].Rows[0]["ConcernPersonContact2"].ToString();
                    objVendor.ConcerPersonEmail2 = ds.Tables[0].Rows[0]["ConcernPersonEmail2"].ToString();
                    objVendor.ConcerPerson1Designation = ds.Tables[0].Rows[0]["ConcernPerson1Designation"].ToString();
                    objVendor.ConcerPerson2Designation = ds.Tables[0].Rows[0]["ConcernPerson2Designation"].ToString();
                    objVendor.PANNO = ds.Tables[0].Rows[0]["PANNO"].ToString();
                    objVendor.PanImage =  ds.Tables[0].Rows[0]["PanImage"].ToString();
                    objVendor.ServiceTypeName= ds.Tables[0].Rows[0]["ServiceTypeID"].ToString();

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow r in ds.Tables[1].Rows)
                        {
                            objVendor.ServiceTypeID = r["Pk_ServiceId"].ToString() + "," + objVendor.ServiceTypeID;
                            //objVendor.ServiceTypeName = objVendor.ServiceTypeName + ", " + r["ServiceName"].ToString();
                            objVendor.Result = r["ServiceName"].ToString() + ", " + objVendor.Result;
                        }
                        objVendor.ServiceTypeID = objVendor.ServiceTypeID.Substring(0, objVendor.ServiceTypeID.Length - 1);
                        objVendor.Result = objVendor.Result.Substring(0, objVendor.Result.Length - 2);
                    }
                }
            }
            return View(objVendor);
        }

        [HttpPost]
        [ActionName("AddVendor")]
        [OnAction(ButtonName = "SaveVendor")]
        public ActionResult SaveVendor(Vendor obj, HttpPostedFileBase postedFile)
        {
            try
            {
                if (postedFile != null)
                {
                    obj.PanImage = "../assets/SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(postedFile.FileName);
                    postedFile.SaveAs(Path.Combine(Server.MapPath(obj.PanImage)));
                }
                obj.AddedBy = Session["UserID"].ToString();

                DataSet ds = obj.VendorRegistration();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        TempData["VendorError"] = "Vendor Registered Successfully.";
                    }
                    else
                    {
                        Master model = new Master();
                        int count3 = 0;
                        List<SelectListItem> ddlServiceType = new List<SelectListItem>();
                        DataSet dsServiceList = model.GetServiceList();
                        if (dsServiceList != null && dsServiceList.Tables.Count > 0 && dsServiceList.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow r in dsServiceList.Tables[0].Rows)
                            {
                                if (count3 == 0)
                                {
                                    ddlServiceType.Add(new SelectListItem { Text = "Select Service Type", Value = "0" });
                                }
                                ddlServiceType.Add(new SelectListItem { Text = r["ServiceName"].ToString(), Value = r["PK_ServiceId"].ToString() });
                                count3 = count3 + 1;
                            }
                        }
                        ViewBag.ddlServiceType = ddlServiceType;
                        TempData["VendorError"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        return View(obj);
                    }
                }
                else
                {
                    TempData["VendorError"] = "Something went wrong.Please contact to technical support.";
                }
            }
            catch (Exception ex)
            {
                TempData["VendorError"] = ex.Message;
            }
            return RedirectToAction("AddVendor");
        }

        public ActionResult VendorList()
        {
            Vendor objVendor = new Vendor();
            if (TempData["VendorDelete"] == null)
            {
                ViewBag.saverrormsg = "none";
            }
           


            List<Vendor> lst = new List<Vendor>();
            objVendor.VendorName = string.IsNullOrEmpty(objVendor.VendorName) ? null : objVendor.VendorName;
            objVendor.VendorId = string.IsNullOrEmpty(objVendor.VendorCode) ? null : objVendor.VendorCode;
           
            DataSet ds = objVendor.GetAllVendors();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Vendor obj = new Vendor();
                    obj.VendorId = Crypto.Encrypt(dr["VendorCode"].ToString());
                    obj.VendorCode = dr["VendorCode"].ToString();
                    obj.VendorName = dr["Name"].ToString();
                    obj.NatureOfBusiness = dr["NatureOfBusiness"].ToString();
                    obj.MobileNo = dr["Contact"].ToString();
                    obj.Email = dr["Email"].ToString();
                    obj.Address = dr["Address"].ToString();
                    obj.Pincode = dr["Pincode"].ToString();
                    obj.StateName = dr["State"].ToString();
                    obj.City = dr["City"].ToString();
                    obj.GSTNO = dr["GSTNo"].ToString();
                    obj.BankName = dr["BankName"].ToString();
                    obj.AccountNo = dr["AccountNumber"].ToString();
                    obj.IFSCCode = dr["IFSCCode"].ToString();
                    obj.ConcernPerson = dr["ConcernPersonName"].ToString();
                    obj.ConcerPersonContact = dr["ConcernPersonContact"].ToString();
                    obj.ConcerPersonEmail = dr["ConcernPersonEmail"].ToString();
                    obj.PANNO = dr["PANNO"].ToString();
                    obj.PanImage = dr["PanImage"].ToString();

                    lst.Add(obj);
                }
                objVendor.lstVendor = lst;
            }
            return View(objVendor);
        }
        [HttpPost]
        [ActionName("VendorList")]
        [OnAction(ButtonName = "GetDetails")]
        public ActionResult GetVendorList(Vendor objVendor)
         {
            if (TempData["VendorDelete"] == null)
            {
                ViewBag.saverrormsg = "none";
            }


            List<Vendor> lst = new List<Vendor>();
            objVendor.VendorName = string.IsNullOrEmpty(objVendor.VendorName) ? null : objVendor.VendorName;
            objVendor.VendorId = string.IsNullOrEmpty(objVendor.VendorCode) ? null : objVendor.VendorCode;
            DataSet ds = objVendor.GetAllVendors();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Vendor obj = new Vendor();
                    obj.VendorId = Crypto.Encrypt(dr["VendorCode"].ToString());
                    obj.VendorCode = dr["VendorCode"].ToString();
                    obj.VendorName = dr["Name"].ToString();
                    obj.NatureOfBusiness = dr["NatureOfBusiness"].ToString();
                    obj.MobileNo = dr["Contact"].ToString();
                    obj.Email = dr["Email"].ToString();
                    obj.Address = dr["Address"].ToString();
                    obj.Pincode = dr["Pincode"].ToString();
                    obj.StateName = dr["State"].ToString();
                    obj.City = dr["City"].ToString();
                    obj.GSTNO = dr["GSTNo"].ToString();
                    obj.BankName = dr["BankName"].ToString();
                    obj.AccountNo = dr["AccountNumber"].ToString();
                    obj.IFSCCode = dr["IFSCCode"].ToString();
                    obj.ConcernPerson = dr["ConcernPersonName"].ToString();
                    obj.ConcerPersonContact = dr["ConcernPersonContact"].ToString();
                    obj.ConcerPersonEmail = dr["ConcernPersonEmail"].ToString();
                    obj.PANNO = dr["PANNO"].ToString();
                    obj.PanImage = dr["PanImage"].ToString();

                    lst.Add(obj);
                }
                objVendor.lstVendor = lst;
            }
            return View(objVendor);
        }

        [HttpPost]
        [ActionName("AddVendor")]
        [OnAction(ButtonName = "UpdateVendor")]
        public ActionResult UpdateVendor(Vendor obj, HttpPostedFileBase postedFile)

        {
            if (postedFile != null)
            {
                obj.PanImage = "../assets/SoftwareImages/" + Guid.NewGuid() + Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(Path.Combine(Server.MapPath(obj.PanImage)));
            }
            try
            {
                obj.UpdatedBy = Session["UserID"].ToString();

                DataSet ds = obj.UpdateVendor();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["MSG"].ToString() == "1")
                    {
                        TempData["VendorError"] = "Vendor Updated Successfully.";
                    }
                    else
                    {
                        Master model = new Master();
                        int count3 = 0;
                        List<SelectListItem> ddlServiceType = new List<SelectListItem>();
                        DataSet dsServiceList = model.GetServiceList();
                        if (dsServiceList != null && dsServiceList.Tables.Count > 0 && dsServiceList.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow r in dsServiceList.Tables[0].Rows)
                            {
                                if (count3 == 0)
                                {
                                    ddlServiceType.Add(new SelectListItem { Text = "Select Service Type", Value = "0" });
                                }
                                ddlServiceType.Add(new SelectListItem { Text = r["ServiceName"].ToString(), Value = r["PK_ServiceId"].ToString() });
                                count3 = count3 + 1;
                            }
                        }
                        ViewBag.ddlServiceType = ddlServiceType;

                        TempData["VendorError"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                        return View(obj);
                    }
                }
                else
                {
                    TempData["VendorError"] = "Something went wrong.Please contact to technical support.";
                }
            }
            catch (Exception ex)
            {
                TempData["VendorError"] = ex.Message;
                
            }
            return RedirectToAction("AddVendor");
        }

        public ActionResult DeleteVendor(string VendorId)
        {
            Vendor obj = new Vendor();
            try
            {

                obj.DeletedBy = Session["UserID"].ToString();
                obj.VendorId = Crypto.Decrypt(VendorId);
                DataSet ds = new DataSet();


                ds = obj.DeleteVendor();
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {

                        TempData["VendorDelete"] = "Vendor Deleted Successfully";


                    }
                    else
                    {
                        TempData["VendorDelete"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();


                    }
                }
            }
            catch (Exception ex)
            {
                TempData["VendorDelete"] = ex.Message;

            }
            ViewBag.saverrormsg = "";
            return RedirectToAction("VendorList");
        }

    }
}

