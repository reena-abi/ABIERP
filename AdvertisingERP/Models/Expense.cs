using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvertisingERP.Models
{
    public class Expense : Common
    {
        public string ExpenseTypeId { get; set; }
        public string Encrypt { get; set; }
        public string FK_ExpenseTypeId { get; set; }
        public string ExpenseId { get; set; }
        public string ExpenseType { get; set; }
        public string ExpenseName { get; set; }
        public string AddedBy { get; set; }
        public string PK_CompanyID { get; set; }
        public string Remark { get; set; }
        public string PaymentDate { get; set; }
        public string PK_DrExpenseID { get; set; }
        public string PF_ExpenseNameID { get; set; }
        public string PK_PaymentmodeId { get; set; }
        public string TransactionNo { get; set; }
        public string ChaqueDate { get; set; }
        public string Amount { get; set; }
        public string LoginId { get; set; }
        public string EntryType { get; set; }
        public string DrAmount { get; set; }
        public string CrAmount { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string totalBalance { get; set; }
        public string PaymentModeName { get; set; }
        public string PK_ExpenseDetailsID { get; set; }
        public DataTable dt { get; set; }
        public DataTable dtExpenseDetails { get; set; }
        public List<Expense> lstExpenseType { get; set; }
        public List<Expense> lstExpense { get; set; }
        public List<Expense> lstCrDrExpense { get; set; }
        public List<SelectListItem> ddlExpenseType { get; set; }
        public List<SelectListItem> ddlExpenseName { get; set; }


        


        public DataSet SaveExpenseType()
        {
            SqlParameter[] para = { new SqlParameter("@ExpenseTypeName",ExpenseType ),
                                       new SqlParameter("@AddedBy", AddedBy)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("SaveExpenseType", para);
            return ds;
        }


        public DataSet UpdateExpenseType()
        {
            SqlParameter[] para = {
                                   new SqlParameter("@PK_ExpenseTypeId",ExpenseTypeId),
                                   new SqlParameter("@ExpenseTypeName",ExpenseType ),
                                       new SqlParameter("@AddedBy", AddedBy)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("UpdateExpenseType", para);
            return ds;
        }


        public DataSet GetExpenseTypeList()
        {
            SqlParameter[] para = { new SqlParameter("@PK_ExpenseTypeId",ExpenseTypeId)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetExpenseTypeList",para);
            return ds;
        }

        public DataSet DeleteExpenseType()
        {
            SqlParameter[] para = { new SqlParameter("@ExpenseTypeId",ExpenseTypeId ),
                                       new SqlParameter("@AddedBy", AddedBy)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("DeleteExpenseType", para);
            return ds;
        }
        public DataSet SaveExpense()
        {
            SqlParameter[] para = {
                new SqlParameter("@FK_ExpenseTypeId",FK_ExpenseTypeId ),
                   new SqlParameter("@ExpenseName",ExpenseName ),
                                       new SqlParameter("@AddedBy", AddedBy)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("SaveExpense", para);
            return ds;
        }


        public DataSet GetExpenseList()
        {

            SqlParameter[] para = {
                new SqlParameter("@PK_ExpenseId",ExpenseId)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetExpenseList", para);
            return ds;
        }

        public DataSet UpdateExpense()
        {
            SqlParameter[] para = {
                  new SqlParameter("@ExpenseId",ExpenseId),
              new SqlParameter("@FK_ExpenseTypeId",FK_ExpenseTypeId ),
                   new SqlParameter("@ExpenseName",ExpenseName ),
                                       new SqlParameter("@AddedBy", AddedBy)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("UpdateExpense", para);
            return ds;
        }


        public DataSet DeleteExpense()
        {
            SqlParameter[] para = { new SqlParameter("@PK_ExpenseId",ExpenseId ),
                                       new SqlParameter("@AddedBy", AddedBy)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("DeleteExpense", para);
            return ds;
        }

        public DataSet GetPaymentMode()
        {
            DataSet ds = DBHelper.ExecuteQuery("getpaymentmode");
            return ds;
        }
         public DataSet getCompany()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetCompany");
            return ds;
        }
        public DataSet GetExpenseTypeName()
        {
            SqlParameter[] para = { new SqlParameter("@FK_ExpenseTypeId",ExpenseId ),
                                      
                                  };
            DataSet ds = DBHelper.ExecuteQuery("GetExpenseTypeName", para);
            return ds;
        }
        public DataSet SaveDataDr()
        {
            SqlParameter[] para = { new SqlParameter("@dt",dtExpenseDetails),
                new SqlParameter("@EntryType",EntryType),
                  new SqlParameter("@AddedBy",AddedBy)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("SaveExpenseDetailsDr", para);
            return ds;
        }
       
        public DataSet CrDrExpenseList()
        {
            SqlParameter[] para = { new SqlParameter("@LoginID",LoginId),
                                new SqlParameter("@EntryType",EntryType),
                                 new SqlParameter("@FromDate",FromDate),
                                  new SqlParameter("@ToDate",ToDate),
                                  };
            DataSet ds = DBHelper.ExecuteQuery("CrDrExpenseList", para);
            return ds;
        }
        public DataSet DeleteCrDrExpense()
        {
            SqlParameter[] para = { new SqlParameter("@PK_ExpenseDetailsID",PK_ExpenseDetailsID ),
                                       new SqlParameter("@DeletedBy", AddedBy)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("DeleteCrDrEpense", para);
            return ds;
        }













        public DataSet SaveDataCr()
        {
            SqlParameter[] para = { new SqlParameter("@dt",dtExpenseDetails),
                new SqlParameter("@EntryType",EntryType),
                  new SqlParameter("@AddedBy",AddedBy)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("SaveExpenseDetailsCr", para);
            return ds;
        }
    }
}