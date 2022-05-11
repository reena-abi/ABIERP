using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdvertisingERP.Models
{
    public class SaleOrder : Common
    {
        #region Properties
        public string HSNCode { get; set; }
        public string PostStatus { get; set; }
        public string SOStatus { get; set; }
        public string DescriptionDisplay { get; set; }
        public string PK_InvoiceNoID { get; set; }
        public string MediaTypeID { get; set; }
        public string FK_SaleOrderId { get; set; }
        public string CityID { get; set; }
        public string LineSNo { get; set; }
        public string LineStatus { get; set; }
        public string IsDummySite { get; set; }
        public string CssClass { get; set; }
        public string Amt { get; set; }
        public string PK_SalesOrderID { get; set; }
        public string PK_SalesOrderDetailsID { get; set; }
        public string TotalAmountDisplay { get; set; }
        public string SaleOrderDetailsID { get; set; }
        public string SaleOrderNoEncrypt { get; set; }
        public string SaleOrderIDEncrypt { get; set; }
        public string CampaignNo { get; set; }
        public string SalesOrderNo { get; set; }
        public string SalesOrderNoID { get; set; }
        public string CustomerID { get; set; }
        public string GSTIN { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string CampaignID { get; set; }
        public string CampaignNumber { get; set; }
       public string POReceived { get; set; }
        public string Quantity { get; set; }
        public string Rate { get; set; }
        public string TotalAmount { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string IGST { get; set; }
        public string TDS { get; set; }
        public string Discount { get; set; }
        public string FinalAmount { get; set; }
        public string PONumber { get; set; }
        public string POImagePath { get; set; }
        public string PaymentTermsID { get; set; }
        public string SalesPersonID { get; set; }
        public string OperationExecutiveID { get; set; }
        public string BillingSnapsID { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }

        public string Side { get; set; }
        public string Unit { get; set; }
        public string Area { get; set; }
        public string SiteID { get; set; }
        public string SiteInfo { get; set; }
        public string SiteName { get; set; }
        public string SiteImage { get; set; }
        public string Facing { get; set; }
        public string Rational { get; set; }
        public string MediaVehicle { get; set; }
        public string MediaType { get; set; }
        public string ServiceID { get; set; }
        public string ServiceName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string Description { get; set; }
        public string SaleOrderNo { get; set; }
        public string OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerInfo { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerMobile { get; set; }
        public string PK_SalesOrderNoID { get; set; }
        public string CustomerAddress { get; set; }
        public string ServiceType { get; set; }
        public string isEditable { get; set; }
        public string isPOGenerated { get; set; }
        public DataTable dtSaleOrderDetails { get; set; }
        public List<SaleOrder> lstInvoiceNo { get; set; }
        public List<SaleOrder> lstsaleorder { get; set; }
        public List<SaleOrder> lstSites { get; set; }
        public List<SaleOrder> lstSalesPerson { get; set; }
        public List<SaleOrder> lstOperationExecutive { get; set; }
        public List<SelectListItem> lstMediaType { get; set; }
        public List<SelectListItem> lstVendors { get; set; }
        public List<SelectListItem> ddlSites { get; set; }
        public List<SelectListItem> lstsaleoredernolist { get; set; }
        public string StoreName { get;  set; }
        public string StoreCode { get; set; }
        public string StoreType { get; set; }
        public string ConcernPerson1 { get;  set; }
        public string CompanyMobile { get;  set; }
        public string Email { get;  set; }
        public string OtherCharges { get; set; }
        #endregion

        #region InvoiceNo
        public DataSet GenerateInvoiceNo()
        {
            SqlParameter[] para = { new SqlParameter("@AddedBy", AddedBy), };
            DataSet ds = DBHelper.ExecuteQuery("GenerateInvoiceNo", para);
            return ds;
        }
        public DataSet GetInvoiceNoList()
        {
            SqlParameter[] para = { new SqlParameter("@InvoiceNo", InvoiceNo), };
            DataSet ds = DBHelper.ExecuteQuery("GetInvoiceNoList", para);
            return ds;
        }
        public DataSet SalesOrderNoListForCampaign()
        {
            SqlParameter[] para ={ new SqlParameter ("@CampaignNo", CampaignID) };
            DataSet ds = DBHelper.ExecuteQuery("GetSaleOrderNoListAgainstCampaigns", para);
            return ds;
        }
        public DataSet InvoiceSOMapping()
        {
            SqlParameter[] para = { new SqlParameter("@FK_InvoiceID", PK_InvoiceNoID),
                                    new SqlParameter("@FK_SaleOrderID", FK_SaleOrderId),
                                    new SqlParameter("@AddedBy", AddedBy)};
            DataSet ds = DBHelper.ExecuteQuery("InvoiceSOMapping", para);
            return ds;
        }
        #endregion

        public DataSet GetStateList()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetStateList");
            return ds;
        }
        public DataSet GetCityByState()
        {
            SqlParameter[] para = { new SqlParameter("@StateName", StateName), };
            DataSet ds = DBHelper.ExecuteQuery("GetCityByState", para);
            return ds;
        }
        public DataSet GetCityList()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetCityList");
            return ds;
        }
        public DataSet GetSaleOrderLines()
        {
            SqlParameter[] para = { new SqlParameter("@FK_SalesOrderID", VendorCode), };
            DataSet ds = DBHelper.ExecuteQuery("GetSaleOrderLinesTmp", para);
            return ds;
        }
        public DataSet GetAllVendors()
        {
            SqlParameter[] para ={ new SqlParameter ("@City",City),
                                   new SqlParameter ("@ServiceID",ServiceID) };
            DataSet ds = DBHelper.ExecuteQuery("GetVendorForSOLines", para);
            return ds;
        }
        public DataSet SalesOrderNoList()
        {
            SqlParameter[] para ={ new SqlParameter ("@SaleOrderNo", SalesOrderNo), };
            DataSet ds = DBHelper.ExecuteQuery("GetSaleOrderNoList", para);
            return ds;
        }
        public DataSet SalesOrderNoForPO()
        {
            SqlParameter[] para ={ new SqlParameter ("@SaleOrderNo",SalesOrderNo),
                                    new SqlParameter ("@CampaignNo",CampaignNo)};
            DataSet ds = DBHelper.ExecuteQuery("GetSaleOrderNoForPurchase", para);
            return ds;
        }
        public DataSet SalesOrderNoForPOUpdate()
        {
            SqlParameter[] para ={ new SqlParameter ("@SaleOrderNo",SalesOrderNo),
                                    new SqlParameter ("@CampaignNo",CampaignNo)};
            DataSet ds = DBHelper.ExecuteQuery("GetSaleOrderNoForPurchaseUpdate", para);
            return ds;
        }
        public DataSet GenerateSalesOrderNo()
        {
            SqlParameter[] para = { new SqlParameter("@AddedBy", AddedBy), };

            DataSet ds = DBHelper.ExecuteQuery("GenerateSalesOrderNo", para);
            return ds;
        }
        
        public DataSet GetSiteList()
        {
            SqlParameter[] para ={new SqlParameter ("@VendorID",VendorID),
                                   new SqlParameter("@City",City) };
            DataSet ds = DBHelper.ExecuteQuery("GetSiteForSOLines", para);
            return ds;
        }

        public DataSet GetSiteDetails()
        {
            SqlParameter[] para ={new SqlParameter ("@SiteID", SiteID), };
            DataSet ds = DBHelper.ExecuteQuery("GetAllSiteList", para);
            return ds;
        }
        
        public DataSet GetSalesPersonList()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetSalesPersonList");
            return ds;
        }

        public DataSet GetOperationExecutiveList()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetOperationExecutiveList");
            return ds;
        }

        public DataSet GetServiceList()
        {
            SqlParameter[] para = { new SqlParameter("@Fk_ServiceId", ServiceID) };
            DataSet ds = DBHelper.ExecuteQuery("GetAllServices", para);
            return ds;
        }

        public DataSet SaleOrderInsert()
        {
            SqlParameter[] para = { new SqlParameter("@SalesOrderDetails", dtSaleOrderDetails),
                                  new SqlParameter("@SalesOrderNoID", SalesOrderNoID),
                                  new SqlParameter("@OrderDate", OrderDate),
                                  new SqlParameter("@CustomerCode", CustomerCode),
                                  new SqlParameter("@CampaignNo", CampaignID),
                                  new SqlParameter("@POReceived", POReceived),
                                  new SqlParameter("@PONumber", PONumber),
                                  new SqlParameter("@POImagePath", POImagePath),
                                  new SqlParameter("@PaymentTermsID", PaymentTermsID),
                                  new SqlParameter("@SalesPersonID", SalesPersonID),
                                  new SqlParameter("@OperationExecutiveID", OperationExecutiveID),
                                  new SqlParameter("@BillingSnapsID", BillingSnapsID),
                                  new SqlParameter("@AddedBy", AddedBy)  };
            DataSet ds = DBHelper.ExecuteQuery("SalesOrderInsert", para);
            return ds;
        }

        public DataSet SaleOrderUpdate()
        {
            SqlParameter[] para = { new SqlParameter("@SalesOrderDetails", dtSaleOrderDetails),
                                  new SqlParameter("@PK_SalesOrderID", PK_SalesOrderID),
                                  new SqlParameter("@SalesOrderNoID", SalesOrderNoID),
                                  new SqlParameter("@OrderDate", OrderDate),
                                  new SqlParameter("@CustomerCode", CustomerCode),
                                  new SqlParameter("@CampaignID", CampaignID),
                                  new SqlParameter("@POReceived", POReceived),
                                  new SqlParameter("@PONumber", PONumber),
                                  new SqlParameter("@POImagePath", POImagePath),
                                  new SqlParameter("@PaymentTermsID", PaymentTermsID),
                                  new SqlParameter("@SalesPersonID", SalesPersonID),
                                  new SqlParameter("@OperationExecutiveID", OperationExecutiveID),
                                  new SqlParameter("@BillingSnapsID", BillingSnapsID),
                                  new SqlParameter("@AddedBy", AddedBy)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("UpdateSaleOrder", para);
            return ds;
        }

        public DataSet SaveLineTemporary()
        {
            SqlParameter[] para = { new SqlParameter("@FK_SaleOrderID", SalesOrderNoID),
                                  new SqlParameter("@City",CityID),
                                  new SqlParameter("@FK_SiteID",SiteID),
                                  new SqlParameter("@FK_ServiceID", ServiceID),
                                  new SqlParameter("@FK_VendorID", VendorID),
                                  new SqlParameter("@FromDate", FromDate),
                                  new SqlParameter("@ToDate", ToDate),
                                  new SqlParameter("@Side", Side),
                                  new SqlParameter("@Height", Height),
                                  new SqlParameter("@Width", Width),
                                  new SqlParameter("@Area", Area),
                                  new SqlParameter("@Quantity", Unit),
                                  new SqlParameter("@Rate", Rate),
                                  new SqlParameter("@TotalAmount", TotalAmount),
                                  new SqlParameter("@CGST", CGST),
                                  new SqlParameter("@SGST", SGST),
                                  new SqlParameter("@IGST", IGST),
                                  new SqlParameter("@TDS", TDS),
                                  new SqlParameter("@Discount", Discount),
                                  new SqlParameter("@FK_MediaTypeID", MediaTypeID),
                                  new SqlParameter("@FinalAmount", FinalAmount) ,
                                  new SqlParameter("@Description", Description),
                                  new SqlParameter("@AddedBy", AddedBy),
                                  new SqlParameter("@OrderDate", OrderDate),
                                  new SqlParameter("@CustomerCode", CustomerCode),
                                  new SqlParameter("@CampaignNo", CampaignID),
                                  new SqlParameter("@POReceived", POReceived),
                                  new SqlParameter("@PONumber", PONumber),
                                  new SqlParameter("@POImagePath", POImagePath),
                                  new SqlParameter("@PaymentTermsID", PaymentTermsID),
                                  new SqlParameter("@SalesPersonID", SalesPersonID),
                                  new SqlParameter("@OperationExecutiveID", OperationExecutiveID),
                                  new SqlParameter("@BillingSnapsID", BillingSnapsID),
                                  new SqlParameter("@HSNCode", HSNCode),
                                  new SqlParameter("@StateName", StateName)
                                  };
            DataSet ds = DBHelper.ExecuteQuery("InsertSaleOrderLine", para);
            return ds;
        }

        public DataSet GetSaleOrderDetails()
        {
            SqlParameter[] para = { new SqlParameter("@SalesOrderNo", SalesOrderNo),
                                    new SqlParameter("@CustomerID", CustomerID) };
            DataSet ds = DBHelper.ExecuteQuery("GetSaleOrderDetailsNew", para);
            return ds;
        }
        public DataSet PrintSO()
        {
            SqlParameter[] para = { new SqlParameter("@SalesOrderNo", SalesOrderNo),
                                    new SqlParameter("@CustomerID", CustomerID) };
            DataSet ds = DBHelper.ExecuteQuery("PrintSO", para);
            return ds;
        }
        public DataSet GetSaleOrderDetailsForPO()
        {
            SqlParameter[] para = { new SqlParameter("@FK_SaleOrderId", FK_SaleOrderId), };
            DataSet ds = DBHelper.ExecuteQuery("GetSaleDetailsForPurchase", para);
            return ds;
        }
        public DataSet DeleteSaleOrderLine()
        {
            SqlParameter[] para = { new SqlParameter("@PK_SaleOrderDetailsID", PK_SalesOrderDetailsID),
                                      new SqlParameter("@LineStatus", LineStatus),
                                      new SqlParameter("@DeletedBy", AddedBy) };
            DataSet ds = DBHelper.ExecuteQuery("DeleteSaleOrderLine", para);
            return ds;
        }

        public DataSet GetDataForLineEdit()
        {
            SqlParameter[] para = { new SqlParameter("@PK_SalesOrderDetailsID", PK_SalesOrderDetailsID),
                                    new SqlParameter("@LineStatus", LineStatus),};
            DataSet ds = DBHelper.ExecuteQuery("GetDataForLineEdit", para);
            return ds;
        }

        public DataSet UpdateSaleOrderLine()
        {
            SqlParameter[] para = { new SqlParameter("@PK_SalesOrderDetailsID", SaleOrderDetailsID),
                                  new SqlParameter("@FK_SaleOrderID", SalesOrderNoID),
                                  new SqlParameter("@FK_SiteID", SiteID),
                                  new SqlParameter("@FK_ServiceID", ServiceID),
                                  new SqlParameter("@FK_VendorID", VendorID),
                                  new SqlParameter("@FromDate", FromDate),
                                  new SqlParameter("@ToDate", ToDate),
                                  new SqlParameter("@Side", Side),
                                  new SqlParameter("@Height", Height),
                                  new SqlParameter("@Width", Width),
                                  new SqlParameter("@Area", Area),
                                  new SqlParameter("@Quantity", Unit),
                                  new SqlParameter("@Rate", Rate),
                                  new SqlParameter("@TotalAmount", TotalAmount),
                                  new SqlParameter("@CGST", CGST),
                                  new SqlParameter("@SGST", SGST),
                                  new SqlParameter("@IGST", IGST),
                                  new SqlParameter("@TDS", TDS),
                                  new SqlParameter("@Discount", Discount),
                                  new SqlParameter("@FinalAmount", FinalAmount),
                                  new SqlParameter("@Description", Description),
                                  new SqlParameter("@FK_MediaTypeID", MediaTypeID),
                                  new SqlParameter("@AddedBy", AddedBy),
                                  new SqlParameter("@LineStatus", LineStatus),
                                  new SqlParameter("@HSNCode", HSNCode) };
            DataSet ds = DBHelper.ExecuteQuery("UpdateSaleOrderLine", para);
            return ds;
        }

        public DataSet SaveSaleOrder()
        {
            SqlParameter[] para = { new SqlParameter("@SalesOrderNoID", SalesOrderNoID),
                                  new SqlParameter("@OrderDate", OrderDate),
                                  new SqlParameter("@CustomerCode", CustomerCode),
                                  new SqlParameter("@CampaignNo", CampaignID),
                                  new SqlParameter("@POReceived", POReceived),
                                  new SqlParameter("@PONumber", PONumber),
                                  new SqlParameter("@POImagePath", POImagePath),
                                  new SqlParameter("@PaymentTermsID", PaymentTermsID),
                                  new SqlParameter("@SalesPersonID", SalesPersonID),
                                  new SqlParameter("@OperationExecutiveID", OperationExecutiveID),
                                  new SqlParameter("@BillingSnapsID", BillingSnapsID),
                                  new SqlParameter("@AddedBy", AddedBy),
                                  new SqlParameter("@OtherCharges",0)
            };
            DataSet ds = DBHelper.ExecuteQuery("_SalesOrderInsert", para);
            return ds;
        }

        public DataSet UpdateSaleOrder()
        {
            SqlParameter[] para = { new SqlParameter("@SalesOrderNoID", SalesOrderNoID),
                                  new SqlParameter("@OrderDate", OrderDate),
                                  new SqlParameter("@CustomerCode", CustomerCode),
                                  new SqlParameter("@CampaignNo", CampaignID),
                                  new SqlParameter("@POReceived", POReceived),
                                  new SqlParameter("@PONumber", PONumber),
                                  new SqlParameter("@POImagePath", POImagePath),
                                  new SqlParameter("@PaymentTermsID", PaymentTermsID),
                                  new SqlParameter("@SalesPersonID", SalesPersonID),
                                  new SqlParameter("@OperationExecutiveID", OperationExecutiveID),
                                  new SqlParameter("@BillingSnapsID", BillingSnapsID),
                                  new SqlParameter("@UpdatedBy", AddedBy) };
            DataSet ds = DBHelper.ExecuteQuery("_SalesOrderUpdate", para);
            return ds;
        }

        public DataSet GetPrintInvoice()
        {
            SqlParameter[] para = { new SqlParameter("@InvoiceNo",InvoiceNo) };
            DataSet ds = DBHelper.ExecuteQuery("GetSOMappedInvoice", para);
            return ds;
        }
        
    }
}
