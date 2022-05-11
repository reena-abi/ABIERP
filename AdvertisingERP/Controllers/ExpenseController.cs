using AdvertisingERP.Filter;
using AdvertisingERP.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace AdvertisingERP.Controllers
{
    public class ExpenseController : AdminBaseController
    {
        // GET: Expense
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ExpenseTypeMaster(string Id)
        {
            Expense model = new Expense();
            if(Id!=null)
            {
                model.ExpenseTypeId = Id;
                DataSet ds = model.GetExpenseTypeList();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    model.ExpenseType = ds.Tables[0].Rows[0]["ExpenseTypeName"].ToString();
                }
            }
           
            return View(model);
        }
        [HttpPost]
        [ActionName("ExpenseTypeMaster")]
        [OnAction(ButtonName = "btnsave")]
        public ActionResult ExpenseTypeMaster(Expense model)
        {
            try
            {
                model.AddedBy = Session["UserID"].ToString();
                DataSet ds = model.SaveExpenseType();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["msg"] = "Expense type save successfully";
                    }
                    else
                    {
                        TempData["msgerror"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msgerror"] = ex.Message;
            }
            return RedirectToAction("ExpenseTypeMaster", "Expense");
        }
        [HttpPost]
        [OnAction(ButtonName="btnupdate")]
        [ActionName("ExpenseTypeMaster")]
        public ActionResult UpdateExpenseTypeMaster(Expense model)
        {
            try
            {
                model.AddedBy= Session["UserID"].ToString();
                DataSet ds = model.UpdateExpenseType();
                if(ds!=null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0)
                {
                    if(ds.Tables[0].Rows[0][0].ToString()=="1")
                    {
                        TempData["msg"] = "Expense type updated successfully";
                    }
                    else if(ds.Tables[0].Rows[0][0].ToString()=="0")
                    {
                        TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch(Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("ExpenseTypeMaster", "Expense");
        }
        public ActionResult ExpenseTypeList()
        {
            Expense model = new Expense();
            try
            {
                List<Expense> lst = new List<Expense>();
                DataSet ds = model.GetExpenseTypeList();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Expense obj = new Expense();
                        obj.ExpenseTypeId = dr["PK_ExpenseTypeId"].ToString();
                        obj.ExpenseType = dr["ExpenseTypeName"].ToString();
                        lst.Add(obj);
                    }
                    model.lstExpenseType = lst;
                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }
        public ActionResult DeleteExpenseType(Expense model, string Id)
        {
            try
            {
                model.ExpenseTypeId = Id;
                model.AddedBy = Session["UserID"].ToString();
                DataSet ds = model.DeleteExpenseType();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["msg"] = "Expense type deleted successfully";
                    }
                    else
                    {
                        TempData["msgerror"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msgerror"] = ex.Message;
            }
            return RedirectToAction("ExpenseTypeList", "Expense");
        }
        public ActionResult ExpenseType(string Id)
        {
            Expense model = new Expense();
            List<SelectListItem> ddlExpenseType = new List<SelectListItem>();
            if (Id != null)
            {
                model.ExpenseId = Id;
                DataSet ds = model.GetExpenseList();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    model.ExpenseId = ds.Tables[0].Rows[0]["PK_ExpenseId"].ToString();
                    model.FK_ExpenseTypeId = ds.Tables[0].Rows[0]["FK_ExpenseTypeId"].ToString();
                    model.ExpenseName = ds.Tables[0].Rows[0]["ExpenseName"].ToString();
                }
                int count41 = 0;
                //List<SelectListItem> ddlExpenseType = new List<SelectListItem>();
                DataSet dsTemplate1 = model.GetExpenseTypeList();
                if (dsTemplate1 != null && dsTemplate1.Tables.Count > 0 && dsTemplate1.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dsTemplate1.Tables[0].Rows)
                    {
                        if (count41 == 0)
                        {
                            ddlExpenseType.Add(new SelectListItem { Text = "Select", Value = "0" });
                        }
                        ddlExpenseType.Add(new SelectListItem { Text = r["ExpenseTypeName"].ToString(), Value = r["PK_ExpenseTypeId"].ToString() });
                        count41 = count41 + 1;
                    }
                }
                model.ddlExpenseType = ddlExpenseType;

            }

            int count4 = 0;
            //List<SelectListItem> ddlExpenseType = new List<SelectListItem>();
            DataSet dsTemplate = model.GetExpenseTypeList();
            if (dsTemplate != null && dsTemplate.Tables.Count > 0 && dsTemplate.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsTemplate.Tables[0].Rows)
                {
                    if (count4 == 0)
                    {
                        ddlExpenseType.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlExpenseType.Add(new SelectListItem { Text = r["ExpenseTypeName"].ToString(), Value = r["PK_ExpenseTypeId"].ToString() });
                    count4 = count4 + 1;
                }
            }
            ViewBag.ddlExpenseType = ddlExpenseType;

            return View(model);
        }
        public ActionResult ExpenseList()
        {
            Expense model = new Expense();
            try
            {
                List<Expense> lst = new List<Expense>();
                DataSet ds = model.GetExpenseList();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Expense obj = new Expense();
                        obj.ExpenseId = dr["PK_ExpenseId"].ToString();
                        obj.ExpenseType = dr["ExpenseTypeName"].ToString();
                        obj.ExpenseName = dr["ExpenseName"].ToString();
                        lst.Add(obj);
                    }
                    model.lstExpense = lst;
                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }
        [HttpPost]
        [ActionName("ExpenseType")]
        [OnAction(ButtonName = "btnsave")]
        public ActionResult ExpenseType(Expense model)
        {
            try
            {
                model.AddedBy = Session["UserID"].ToString();
                DataSet ds = model.SaveExpense();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["msg"] = "Expense save successfully";
                    }
                    else
                    {
                        TempData["msgerror"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msgerror"] = ex.Message;
            }
            return RedirectToAction("ExpenseType", "Expense");
        }
        [HttpPost]
        [ActionName("ExpenseType")]
        [OnAction(ButtonName = "btnupdate")]
        public ActionResult UpdateExpenseType(Expense model)
        {
            try
            {
                model.AddedBy = Session["UserID"].ToString();
                DataSet ds = model.UpdateExpense();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["msg"] = "Expense updated successfully";
                    }
                    else
                    {
                        TempData["msgerror"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["msgerror"] = ex.Message;
            }
            return RedirectToAction("ExpenseList", "Expense");
        }
        public ActionResult DeleteExpense(Expense model, string Id)
        {
            try
            {
                model.ExpenseId = Id;
                model.AddedBy = Session["UserID"].ToString();
                DataSet ds = model.DeleteExpense();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        TempData["msg"] = "Expense deleted successfully";
                    }
                    else
                    {
                        TempData["msgerror"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
            }

            catch (Exception ex)
            {
                TempData["msgerror"] = ex.Message;
            }
            return RedirectToAction("ExpenseList", "Expense");
        }
        public ActionResult DrExpense()
        {
            Expense model = new Expense();
            #region ddlpaymentmode
            List<SelectListItem> ddlpaymentmode = new List<SelectListItem>();
            DataSet dsTemplate = model.GetPaymentMode();
            int count1 = 0;
            if (dsTemplate != null && dsTemplate.Tables.Count > 0 && dsTemplate.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsTemplate.Tables[0].Rows)
                {
                    if (count1 == 0)
                    {
                        ddlpaymentmode.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlpaymentmode.Add(new SelectListItem { Text = r["PaymentMode"].ToString(), Value = r["PK_paymentID"].ToString() });
                    count1 = count1 + 1;
                }
            }
            ViewBag.ddlpaymentmode = ddlpaymentmode;
            #endregion
            #region  ddlExpenseType
            int count4 = 0;
            List<SelectListItem> ddlExpenseType = new List<SelectListItem>();
            DataSet dsTemp = model.GetExpenseTypeList();
            if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsTemp.Tables[0].Rows)
                {
                    if (count4 == 0)
                    {
                        ddlExpenseType.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlExpenseType.Add(new SelectListItem { Text = r["ExpenseTypeName"].ToString(), Value = r["PK_ExpenseTypeId"].ToString() });
                    count4 = count4 + 1;
                }
            }
            ViewBag.ddlExpenseType = ddlExpenseType;
            #endregion
            #region company name
            int count2 = 0;
            List<SelectListItem> ddlcompany = new List<SelectListItem>();
            DataSet dscom = model.getCompany();
            if (dscom != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dscom.Tables[0].Rows)
                {
                    if (count2 == 0)
                    {
                        ddlcompany.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlcompany.Add(new SelectListItem { Text = r["CompanyName"].ToString(), Value = r["PK_CompanyID"].ToString() });
                    count2 = count2 + 1;
                }
            }
            ViewBag.ddlcompany = ddlcompany;
            #endregion
            List<SelectListItem> ddlExpenseName = new List<SelectListItem>();
            ViewBag.ddlExpenseName = ddlExpenseName;
            return View(model);

        }
        public ActionResult GetExpenseTypeName(string FK_ExpenseTypeId)
        {
            Expense model = new Expense();
            List<SelectListItem> ddlExpenseName = new List<SelectListItem>();

            model.ExpenseId = FK_ExpenseTypeId;
            DataSet dsTemp = model.GetExpenseTypeName();
            if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsTemp.Tables[0].Rows)
                {

                    ddlExpenseName.Add(new SelectListItem { Text = r["ExpenseName"].ToString(), Value = r["PK_ExpenseId"].ToString() });

                }
                model.ddlExpenseName = ddlExpenseName;


            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveDrExpense(Expense order, string dataValue)
        {
            
            try
            {
                string Fk_CompanyId = "";
                string FK_ExpenseTypeId = "";
                string PF_ExpenseID = "";
                string PK_PaymentId = "";
                string TransactionNo = "";
                string Amount = "";
                string PaymentDate = "";
                string Remark = "";
                var isValidModel = TryUpdateModel(order);
                var jss = new JavaScriptSerializer();
                var jdv = jss.Deserialize<dynamic>(dataValue);
                DataTable DrExpenseDetails = new DataTable();
                DrExpenseDetails.Columns.Add("Fk_CompanyId");
                DrExpenseDetails.Columns.Add("FK_ExpenseTypeId");
                DrExpenseDetails.Columns.Add("PF_ExpenseID");
                DrExpenseDetails.Columns.Add("PK_PaymentId");
                DrExpenseDetails.Columns.Add("TransactionNo");
                DrExpenseDetails.Columns.Add("Amount");
                DrExpenseDetails.Columns.Add("PaymentDate");
                DrExpenseDetails.Columns.Add("Remark");
                DataTable dt = new DataTable();
                dt = JsonConvert.DeserializeObject<DataTable>(jdv["AddData"]);
                int numberOfRecords = dt.Rows.Count;
                foreach (DataRow row in dt.Rows)
                {
                    Fk_CompanyId = row["Fk_CompanyId"].ToString();
                    FK_ExpenseTypeId = row["FK_ExpenseTypeId"].ToString();
                    PF_ExpenseID = row["PF_ExpenseID"].ToString();
                    PK_PaymentId = row["PK_PaymentId"].ToString();
                    TransactionNo = row["TransactionNo"].ToString();
                    Amount = row["Amount"].ToString();
                    PaymentDate = string.IsNullOrEmpty(row["PaymentDate"].ToString()) ? null : Common.ConvertToSystemDate(row["PaymentDate"].ToString(), "dd/MM/yyyy");
                    Remark = row["Remark"].ToString();
                    DrExpenseDetails.Rows.Add(Fk_CompanyId, FK_ExpenseTypeId, PF_ExpenseID, PK_PaymentId, TransactionNo, Amount, PaymentDate,  Remark);
                }
                order.dtExpenseDetails = DrExpenseDetails;
                order.AddedBy = Session["UserID"].ToString();
                order.EntryType = "Dr";
                DataSet ds = new DataSet();
                ds = order.SaveDataDr();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "Dr Expense  Save successfully";
                       
                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        order.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {

                TempData["msg"] = ex.Message;
            }

            return Json(order, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CrExpense()
        {
            Expense model = new Expense();
            #region ddlpaymentmode
            List<SelectListItem> ddlpaymentmode = new List<SelectListItem>();
            DataSet dsTemplate = model.GetPaymentMode();
            int count1 = 0;
            if (dsTemplate != null && dsTemplate.Tables.Count > 0 && dsTemplate.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsTemplate.Tables[0].Rows)
                {
                    if (count1 == 0)
                    {
                        ddlpaymentmode.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlpaymentmode.Add(new SelectListItem { Text = r["PaymentMode"].ToString(), Value = r["PK_paymentID"].ToString() });
                    count1 = count1 + 1;
                }
            }
            ViewBag.ddlpaymentmode = ddlpaymentmode;
            #endregion
            #region  ddlExpenseType
            int count4 = 0;
            List<SelectListItem> ddlExpenseType = new List<SelectListItem>();
            DataSet dsTemp = model.GetExpenseTypeList();
            if (dsTemp != null && dsTemp.Tables.Count > 0 && dsTemp.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dsTemp.Tables[0].Rows)
                {
                    if (count4 == 0)
                    {
                        ddlExpenseType.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlExpenseType.Add(new SelectListItem { Text = r["ExpenseTypeName"].ToString(), Value = r["PK_ExpenseTypeId"].ToString() });
                    count4 = count4 + 1;
                }
            }
            ViewBag.ddlExpenseType = ddlExpenseType;
            #endregion
            #region company name
            int count2 = 0;
            List<SelectListItem> ddlcompany = new List<SelectListItem>();
            DataSet dscom = model.getCompany();
            if (dscom != null && dscom.Tables.Count > 0 && dscom.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in dscom.Tables[0].Rows)
                {
                    if (count2 == 0)
                    {
                        ddlcompany.Add(new SelectListItem { Text = "Select", Value = "0" });
                    }
                    ddlcompany.Add(new SelectListItem { Text = r["CompanyName"].ToString(), Value = r["PK_CompanyID"].ToString() });
                    count2 = count2 + 1;
                }
            }
            ViewBag.ddlcompany = ddlcompany;
            #endregion
            List<SelectListItem> ddlExpenseName = new List<SelectListItem>();
            ViewBag.ddlExpenseName = ddlExpenseName;
            return View(model);

        }
        public ActionResult CrDrExpenseList()
        {
            Expense model = new Expense();
            List<Expense> crdrlst = new List<Expense>();

            List<SelectListItem> ddlCrDr = Common.ddlCrDr();
            ViewBag.ddlCrDr = ddlCrDr;
            DataSet ds = model.CrDrExpenseList();
            if(ds!=null && ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0)
            {

                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Expense obj = new Expense();
                    obj.CompanyName = r["CompanyName"].ToString();
                    obj.PK_CompanyID = r["FK_CompanyID"].ToString();
                    obj.ExpenseId = r["Fk_ExpenseId"].ToString();
                    obj.ExpenseName = r["ExpenseName"].ToString();
                    obj.ExpenseTypeId = r["FK_ExpensetypeId"].ToString();
                    obj.ExpenseType = r["ExpenseTypeName"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.totalBalance = r["totalBalance"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.TransactionNo = r["TransactionNo"].ToString();
                    obj.PaymentDate = r["PaymentDate"].ToString();
                    obj.UserName = r["Name"].ToString();
                    obj.EntryType = r["EntryType"].ToString();
                    obj.Remark = r["Remark"].ToString();
                    obj.PaymentModeName = r["PaymentMode"].ToString();
                    obj.PK_ExpenseDetailsID = r["PK_ExpenseDetailsID"].ToString();
                    crdrlst.Add(obj);
                    ViewBag.TotalDrAmount = Convert.ToDecimal(ViewBag.TotalDrAmount) + Convert.ToDecimal(r["DrAmount"].ToString());
                    ViewBag.TotalCrAmount = Convert.ToDecimal(ViewBag.TotalCrAmount) + Convert.ToDecimal(r["CrAmount"].ToString());
                    ViewBag.TotalBalances = Convert.ToDecimal(ViewBag.TotalBalances) + Convert.ToDecimal(r["totalBalance"].ToString());
                    ViewBag.TotalBalance = ViewBag.TotalCrAmount - ViewBag.TotalDrAmount;
                }
                model.lstCrDrExpense = crdrlst;
            }
            return View(model);
        }
        [HttpPost]
        [ActionName("CrDrExpenseList")]
        [OnAction(ButtonName = "GetDetails")]
        public ActionResult CrExpenseListDetails(Expense model)
        {
            List<SelectListItem> ddlCrDr = Common.ddlCrDr();
          
            ViewBag.ddlCrDr = ddlCrDr;

         
            model.EntryType = string.IsNullOrEmpty(model.EntryType) ? null : model.EntryType;
            model.LoginId = string.IsNullOrEmpty(model.LoginId) ? null : model.LoginId;
            model.FromDate = string.IsNullOrEmpty(model.FromDate) ? null : Common.ConvertToSystemDate(model.FromDate, "dd/MM/yyyy");
            model.ToDate = string.IsNullOrEmpty(model.ToDate) ? null : Common.ConvertToSystemDate(model.ToDate, "dd/MM/yyyy");
            List<Expense> crdrlst = new List<Expense>();
            DataSet ds = model.CrDrExpenseList();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    Expense obj = new Expense();
                    obj.CompanyName = r["CompanyName"].ToString();
                    obj.PK_CompanyID = r["FK_CompanyID"].ToString();
                    obj.ExpenseId = r["Fk_ExpenseId"].ToString();
                    obj.ExpenseName = r["ExpenseName"].ToString();
                    obj.ExpenseTypeId = r["FK_ExpensetypeId"].ToString();
                    obj.ExpenseType = r["ExpenseTypeName"].ToString();
                    obj.CrAmount = r["CrAmount"].ToString();
                    obj.DrAmount = r["DrAmount"].ToString();
                    obj.totalBalance = r["totalBalance"].ToString();
                    obj.LoginId = r["LoginId"].ToString();
                    obj.TransactionNo = r["TransactionNo"].ToString();
                    obj.PaymentDate = r["PaymentDate"].ToString();
                    obj.UserName = r["Name"].ToString();
                    obj.EntryType = r["EntryType"].ToString();
                    obj.Remark = r["Remark"].ToString();
                    obj.PaymentModeName = r["PaymentMode"].ToString();
                    obj.PK_ExpenseDetailsID = r["PK_ExpenseDetailsID"].ToString();
                    crdrlst.Add(obj);
                    ViewBag.TotalDrAmount = Convert.ToDecimal(ViewBag.TotalDrAmount) + Convert.ToDecimal(r["DrAmount"].ToString());
                    ViewBag.TotalCrAmount = Convert.ToDecimal(ViewBag.TotalCrAmount) + Convert.ToDecimal(r["CrAmount"].ToString());
                    ViewBag.TotalBalances = Convert.ToDecimal(ViewBag.TotalBalances) + Convert.ToDecimal(r["totalBalance"].ToString());
                    ViewBag.TotalBalance = ViewBag.TotalCrAmount - ViewBag.TotalDrAmount;
                }
                model.lstCrDrExpense = crdrlst;
            }
            return View(model);
        }

        public ActionResult DeleteCrDrExpense(string Id)
        {
            Expense model = new Expense();
            model.PK_ExpenseDetailsID = Id;
            model.AddedBy = Session["UserID"].ToString();
            DataSet ds = model.DeleteCrDrExpense();
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    TempData["msg"] = "Delete expense successfull";
                }
                else
                {
                    TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
            }
            return RedirectToAction("CrDrExpenseList", "Expense");
        }

        public ActionResult SaveCrExpense(Expense order, string dataValue)
        {

            try
            {
                string Fk_CompanyId = "";
                string FK_ExpenseTypeId = "";
                string PF_ExpenseID = "";
                string PK_PaymentId = "";
                string TransactionNo = "";
                string Amount = "";
                string PaymentDate = "";
                string Remark = "";
                var isValidModel = TryUpdateModel(order);
                var jss = new JavaScriptSerializer();
                var jdv = jss.Deserialize<dynamic>(dataValue);
                DataTable DrExpenseDetails = new DataTable();
                DrExpenseDetails.Columns.Add("Fk_CompanyId");
                DrExpenseDetails.Columns.Add("FK_ExpenseTypeId");
                DrExpenseDetails.Columns.Add("PF_ExpenseID");
                DrExpenseDetails.Columns.Add("PK_PaymentId");
                DrExpenseDetails.Columns.Add("TransactionNo");
                DrExpenseDetails.Columns.Add("Amount");
                DrExpenseDetails.Columns.Add("PaymentDate");
                DrExpenseDetails.Columns.Add("Remark");
                DataTable dt = new DataTable();
                dt = JsonConvert.DeserializeObject<DataTable>(jdv["AddData"]);
                int numberOfRecords = dt.Rows.Count;
                foreach (DataRow row in dt.Rows)
                {
                    Fk_CompanyId = row["Fk_CompanyId"].ToString();
                    FK_ExpenseTypeId = row["FK_ExpenseTypeId"].ToString();
                    PF_ExpenseID = row["PF_ExpenseID"].ToString();
                    PK_PaymentId = row["PK_PaymentId"].ToString();
                    TransactionNo = row["TransactionNo"].ToString();
                    Amount = row["Amount"].ToString();
                    PaymentDate = string.IsNullOrEmpty(row["PaymentDate"].ToString()) ? null : Common.ConvertToSystemDate(row["PaymentDate"].ToString(), "dd/MM/yyyy");
                    Remark = row["Remark"].ToString();
                    DrExpenseDetails.Rows.Add(Fk_CompanyId, FK_ExpenseTypeId, PF_ExpenseID, PK_PaymentId, TransactionNo, Amount, PaymentDate, Remark);
                }
                order.dtExpenseDetails = DrExpenseDetails;
                order.AddedBy = Session["UserID"].ToString();
                order.EntryType = "Cr";
                DataSet ds = new DataSet();
                ds = order.SaveDataCr();
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0].ToString() == "1")
                    {
                        TempData["msg"] = "Cr Expense  Save successfully";

                    }
                    else if (ds.Tables[0].Rows[0][0].ToString() == "0")
                    {
                        order.Result = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }
                }
                else
                {
                    TempData["msg"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }
            catch (Exception ex)
            {

                TempData["msg"] = ex.Message;
            }

            return Json(order, JsonRequestBehavior.AllowGet);
        }
    }
}