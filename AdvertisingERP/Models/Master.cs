using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdvertisingERP.Models
{
    public class Master : Common
    {
        public string IsBrandingSite { get; set; }
        public string IsDummySite { get; set; }
        public string EncryptKey { get; set; }
        public string BankID { get; set; }
        public string BankName { get; set; }
        public string IFSCPrefix { get; set; }
        public string Side { get; set; }
        public string SiteImage { get; set; }
        public string SiteOwner { get; set; }
        public string Comments { get; set; }
        public string CartRate { get; set; }
        public string RateSqft { get; set; }
        public string Quantity { get; set; }
        public string Facing { get; set; }
        public string SiteID { get; set; }
        public string VendorID { get; set; }
        public string Rational { get; set; }
        public string Location { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Area { get; set; }
        public string SiteName { get; set; }
        public List<Master> lstSites { get; set; }
        public DataTable dtSiteMaster { get; set; }
        public string MediaTypeID { get; set; }
        public string MediaTypeName { get; set; }
        public string MediaVehicleName { get; set; }
        public string MediaVehicleID { get; set; }
        public string ServiceId { get; set; }
        public string HSNCode { get; set; }
        public string ServiceName { get; set; }
        public string CGST { get; set; }
        public string IGST { get; set; }
        public string SGST { get; set; }
        public string DateFormat { get; set; }
        public string FinancialYearStatus { get; set; }

        public List<Master> lstFinancialYear { get; set; }
        public List<Master> lstBanks { get; set; }
        public List<Master> lstcompany { get; set; }
        public string CompanyName { get; set; }
        public string Pk_CompanyID { get; set; }

        public DataSet SaveSite()
        {
            SqlParameter[] para ={new SqlParameter ("@SiteMaster",dtSiteMaster),
                                new SqlParameter("@AddedBy",AddedBy),
                                 };
            DataSet ds = DBHelper.ExecuteQuery("SiteMaster", para);
            return ds;
        }

        public DataSet UpdateSite()
        {
            SqlParameter[] para ={
                                       new SqlParameter ("@SiteID",SiteID),
                                     new SqlParameter ("@VendorID",VendorID),
                                     new SqlParameter ("@SiteName",SiteName),
                                     new SqlParameter ("@Pincode",Pincode),
                                     new SqlParameter ("@StateName",StateName),
                                     new SqlParameter ("@City",City),
                                     new SqlParameter ("@Facing",Facing),
                                     new SqlParameter ("@Rational",Rational),
                                     new SqlParameter ("@Side",Side),
                                     new SqlParameter ("@Quantity",Quantity),
                                     new SqlParameter ("@Width",Width),
                                     new SqlParameter ("@Height",Height),
                                     new SqlParameter("@Area",Area),
                                      new SqlParameter("@MediaVehicleID",MediaVehicleID),
                                       new SqlParameter("@MediaTypeID",MediaTypeID),
                                        new SqlParameter("@CartRate",CartRate),
                                         new SqlParameter("@Comments",Comments),
                                           new SqlParameter("@SiteOwner",SiteOwner),
                                             new SqlParameter("@SiteImage",SiteImage),
                                             new SqlParameter("@UpdatedBy",UpdatedBy),
                                 };
            DataSet ds = DBHelper.ExecuteQuery("UpdateSite", para);
            return ds;
        }

        public DataSet GetSiteList()
        {
            SqlParameter[] para ={new SqlParameter ("@SiteName",SiteName),
                                new SqlParameter("@MediaTypeID",MediaTypeID),
                                 new SqlParameter("@MediaVehicleId",MediaVehicleID),
                                  new SqlParameter("@VendorID",VendorID),
                                   new SqlParameter("@SiteID",SiteID),
                                 };
            DataSet ds = DBHelper.ExecuteQuery("GetAllSiteList", para);
            return ds;
        }

        public DataSet DeleteSite()
        {
            SqlParameter[] para ={new SqlParameter ("@Fk_SiteId",SiteID),
                                new SqlParameter("@DeletedBy",DeletedBy),
                                 };
            DataSet ds = DBHelper.ExecuteQuery("DeleteSite", para);
            return ds;
        }

        public DataSet GetServiceList()
        {
            SqlParameter[] para ={new SqlParameter ("@Fk_ServiceId",ServiceId),
                                new SqlParameter("@HSNCode",HSNCode),
                                 };
            DataSet ds = DBHelper.ExecuteQuery("GetAllServices", para);
            return ds;
        }

        public DataSet SaveService()
        {
            SqlParameter[] para ={new SqlParameter ("@ServiceName",ServiceName),
                                    new SqlParameter("@HSNCode",HSNCode),
                                     new SqlParameter("@GST",CGST),
                                      new SqlParameter("@IGST",IGST),
                                       new SqlParameter("@SGST",SGST),
                                       new SqlParameter("@AddedBy",AddedBy),
                                        new SqlParameter("@DateFormat",DateFormat),

                                 };
            DataSet ds = DBHelper.ExecuteQuery("InsertService", para);
            return ds;
        }

        public DataSet UpdateService()
        {
            SqlParameter[] para ={new SqlParameter ("@ServiceName",ServiceName),
                                    new SqlParameter("@HSNCode",HSNCode),
                                     new SqlParameter("@GST",CGST),
                                      new SqlParameter("@IGST",IGST),
                                       new SqlParameter("@SGST",SGST),
                                       new SqlParameter("@UpdatedBy",UpdatedBy),
                                        new SqlParameter("@DateFormat",DateFormat),
                                         new SqlParameter("@ServiceID",ServiceId),
                                 };
            DataSet ds = DBHelper.ExecuteQuery("UpdateService", para);
            return ds;
        }

        public DataSet DeleteService()
        {
            SqlParameter[] para ={new SqlParameter ("@ServiceID",ServiceId),
                                     new SqlParameter("@DeletedBy",DeletedBy),

                                 };
            DataSet ds = DBHelper.ExecuteQuery("DeleteService", para);
            return ds;
        }

        public DataSet SaveFinancialYear()
        {
            SqlParameter[] para ={new SqlParameter ("@FinancialYearName", FinancialYearName),
                                new SqlParameter("@AddedBy",AddedBy), };
            DataSet ds = DBHelper.ExecuteQuery("SaveFinancialYear", para);
            return ds;
        }

        public DataSet GetFinancialYearList()
        {
            SqlParameter[] para = { new SqlParameter("@PK_FinancialYearID", FinancialYearID), };
            DataSet ds = DBHelper.ExecuteQuery("GetFinancialYearList", para);
            return ds;
        }

        public DataSet ActivateFinancialYear()
        {
            SqlParameter[] para ={new SqlParameter ("@FinancialYearID", FinancialYearID),
                                new SqlParameter("@DeletedBy", AddedBy), };
            DataSet ds = DBHelper.ExecuteQuery("ActivateFinancialYear", para);
            return ds;
        }

        //public DataSet DeleteFinancialYear()
        //{
        //    SqlParameter[] para ={new SqlParameter ("@FinancialId",FinancialYearID),
        //                             new SqlParameter("@DeletedBy",DeletedBy),

        //                         };
        //    DataSet ds = DBHelper.ExecuteQuery("DeleteFinancialYearDetails", para);
        //    return ds;
        //}

        public DataSet SaveBank()
        {
            SqlParameter[] para ={new SqlParameter ("@BankName", BankName),
                                     new SqlParameter("@IFSCPrefix",IFSCPrefix),
                                     new SqlParameter("@AddedBy",AddedBy), };
            DataSet ds = DBHelper.ExecuteQuery("SaveBank", para);
            return ds;
        }

        public DataSet GetBankList()
        {
            SqlParameter[] para = { new SqlParameter("@PK_BankID", BankID), };
            DataSet ds = DBHelper.ExecuteQuery("GetBankList", para);
            return ds;
        }

        public DataSet UpdateBank()
        {
            SqlParameter[] para ={new SqlParameter ("@PK_BankID", BankID),
                                     new SqlParameter ("@BankName", BankName),
                                     new SqlParameter("@IFSCPrefix",IFSCPrefix),
                                     new SqlParameter("@UpdatedBy",AddedBy), };
            DataSet ds = DBHelper.ExecuteQuery("UpdateBank", para);
            return ds;
        }

        public DataSet DeleteBank()
        {
            SqlParameter[] para ={new SqlParameter ("@Fk_BankId",BankID),
                                     new SqlParameter("@DeletedBy",DeletedBy),

                                 };
            DataSet ds = DBHelper.ExecuteQuery("DeleteBankDetails", para);
            return ds;
        }
        public DataSet GetCompanyList()
        {
            SqlParameter[] para =
            {
                new SqlParameter("@Pk_CompanyID",Pk_CompanyID)
            };
            DataSet ds = DBHelper.ExecuteQuery("GetCompany", para);
            return ds;
        }
        public DataSet SaveCompany()
        {
            SqlParameter[] Para ={
                new SqlParameter("@CompanyName",CompanyName),
                new SqlParameter("@AddedBy",AddedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("SaveCompany", Para);
            return
                ds;
        }
        public DataSet UpdateCompany()
        {
            SqlParameter[] Para ={
                new SqlParameter("@CompanyName",CompanyName),
                new SqlParameter("@Pk_CompanyID",Pk_CompanyID),
                new SqlParameter("@AddedBy",UpdatedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("UpdateCompany", Para);
            return
                ds;
        }
        public DataSet DeleteCompany()
        {
            SqlParameter[] Para ={
               
                new SqlParameter("@Pk_CompanyID",Pk_CompanyID),
                new SqlParameter("@DeletedBy",DeletedBy)
            };
            DataSet ds = DBHelper.ExecuteQuery("DeleteCompany", Para);
            return
                ds;
        }
    }
}