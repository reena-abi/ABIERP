using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace AdvertisingERP.Models
{
    public class PO : Common
    {
        #region Properties
        public string PK_PurchaseOrderID { get; set; }
        public string PK_PurchaseOrderDetailsID { get; set; }
        public string FK_SaleOrderDetailsID { get; set; }
        public string SiteID { get; set; }
        public string ServiceID { get; set; }
        public string VendorID { get; set; }
        public string Description { get; set; }
        public string TDS { get; set; }
        public string Amt { get; set; }
        public string TotalAmount { get; set; }
        public string Discount { get; set; }
        public string FinalAmount { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string IGST { get; set; }
        public string Amount { get; set; }
        public string Rate { get; set; }
        public string Side { get; set; }
        public string Quantity { get; set; }
        public string CreativeName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ServiceName { get; set; }
        public string VendorName { get; set; }
        public string PostStatus { get; set; }
        public string ErrorMessage { get; set; }
        public string EncryptKey { get; set; }
        public string PONumberID { get; set; }
        public string PONumber { get; set; }
        public DataTable dtPODetails { get; set; }
        public string PODate { get; set; }
        public string PaymentTerms { get; set; }
        public string SaleOrderNumber { get; set; }
        public string SaleOrderNumberID { get; set; }
        public string CampaignNoID { get; set; }
        public string CampaignNumber { get; set; }
        public string OperationExecutiveID { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceImage { get; set; }
        public string SiteName { get; set; }   public string HSNCode { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Area { get; set; }
        public string MediaTypeName { get; set; }
        public string MediaVehicleName { get; set; }
        public string SaleOrderNoEncrypt { get; set; }
        public List<PO> lstPO { get; set; }
        #endregion

        public DataSet GetOperationExecutiveList()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetOperationExecutiveList");
            return ds;
        }

        public DataSet InsertPurchaseOrder()
        {
            SqlParameter[] para = { new SqlParameter("@dtPODetails", dtPODetails),
                                  new SqlParameter("@FK_SaleOrderNoID", SaleOrderNumberID),
                                  new SqlParameter("@PurchaseOrderNo", PONumber), 
                                  new SqlParameter("@PurchaseOrderDate", PODate),
                                  new SqlParameter("@PaymentTermsID", PaymentTerms),
                                  new SqlParameter("@CampaignNo", CampaignNoID),
                                  new SqlParameter("@AddedBy", AddedBy)};

            DataSet ds = DBHelper.ExecuteQuery("PurchaseOrderInsert", para);
            return ds;
        }

        public DataSet POList()
        {
            SqlParameter[] para = { new SqlParameter("@PONumber", PONumber) };
            DataSet ds = DBHelper.ExecuteQuery("POList", para);
            return ds;
        }

        public DataSet GeneratePurchaseOrderNo()
        {
            SqlParameter[] para = { new SqlParameter("@AddedBy", AddedBy), };
            DataSet ds = DBHelper.ExecuteQuery("GeneratePurchaseOrderNo", para);
            return ds;
        }

        public DataSet PurchaseOrderNoList()
        {
            SqlParameter[] para ={ new SqlParameter ("@PurchaseOrderNo",PONumber) };
            DataSet ds = DBHelper.ExecuteQuery("GetPONoList", para);
            return ds;
        }

        #region EditPO
        public DataSet GetPODetails()
        {
            SqlParameter[] para = { new SqlParameter("@PONumber", PONumber) };
            DataSet ds = DBHelper.ExecuteQuery("GetPODetails", para);
            return ds;
        }

        public DataSet UpdatePurchaseOrder()
        {
            SqlParameter[] para = { new SqlParameter("@dtPODetails", dtPODetails),
                                  new SqlParameter("@PK_PurchaseOrderID", PK_PurchaseOrderID),
                                  new SqlParameter("@FK_SaleOrderNoID", SaleOrderNumberID),
                                  new SqlParameter("@PurchaseOrderNo", PONumber),
                                  new SqlParameter("@PurchaseOrderDate", PODate),
                                  new SqlParameter("@PaymentTermsID", PaymentTerms),
                                  new SqlParameter("@CampaignNo", CampaignNoID),
                                  new SqlParameter("@AddedBy", AddedBy),
                                  new SqlParameter("@InvoiceNo", InvoiceNo),
                                  new SqlParameter("@InvoiceFile", InvoiceImage)};

            DataSet ds = DBHelper.ExecuteQuery("PurchaseOrderUpdate", para);
            return ds;
        }

        public DataSet DeletePOLine()
        {
            SqlParameter[] para = { new SqlParameter("@PK_PurchaseOrderDetailsID", PK_PurchaseOrderDetailsID),
                                    new SqlParameter("@DeletedBy", DeletedBy) };
            DataSet ds = DBHelper.ExecuteQuery("DeletePOLine", para);
            return ds;
        }
        #endregion

    }
}