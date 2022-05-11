using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdvertisingERP.Models
{
    public class Customer:Common
    {
        public string CompanyName { get; set; }
        public string NatureOfBusiness { get; set; }

        public string PanImage { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string DirectorName { get; set; }
        public string DirectorContact { get; set; }
        public string  Address{ get; set; }
        public string ConcertPerson1 { get; set; }
        public string ConcertPersonContact1 { get; set; }
        public string ConcertPerson2 { get; set; }
        public string ConcertPersonContact2 { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string BankAddress { get; set; }
        public string TANNO { get; set; }
        public string TINNO { get; set; }
        public string CINNO { get; set; }
        public string STAXNO { get; set; }
        public string PANNO { get; set; }
        public string CompanyId { get; set; }
        public string GSTIN { get; set; }
        public string AdhaarNo { get; set; }
        public string IDType { get; set; }
        public string IDNumber { get; set; }
        public string CustomerCode { get; set; }
        public string PK_CustomerID { get; set; }
        public string IdImage { get; set; }
        public List<Customer> lstcustomer { get; set; }

        public DataSet CustomerRegistration()
        {
            SqlParameter[] para ={    
                                      new SqlParameter ("@CompanyName",CompanyName),
                                      new SqlParameter("@NatureOfBusiness",NatureOfBusiness),
                                      new SqlParameter("@CompanyMobile",MobileNo),
                                      new SqlParameter("@CompanyPhone",PhoneNo),
                                      new SqlParameter("@NameOfCompanyDirector",DirectorName),
                                      new SqlParameter("@DirectorContact",DirectorContact),
                                      new SqlParameter("@Email",Email),
                                      new SqlParameter("@Address",Address),
                                      new SqlParameter("@Pincode",Pincode),
                                      new SqlParameter("@State",StateName),
                                      new SqlParameter("@City",City),
                                      new SqlParameter("@ConcernPerson1",ConcertPerson1),
                                      new SqlParameter("@ConcernPerson1Contact",ConcertPersonContact1),
                                      new SqlParameter("@ConcernPerson2",ConcertPerson2),
                                      new SqlParameter("@ConcernPerson2Contact",ConcertPersonContact2),
                                      new SqlParameter("@BankName",BankName),
                                      new SqlParameter("@BankAccountNumber",AccountNo),
                                      new SqlParameter("@IFSCCode",IFSCCode),
                                      new SqlParameter("@BankAddress",BankAddress),
                                      new SqlParameter("@TANNO",TANNO),
                                      new SqlParameter("@TINNO",TINNO),
                                      new SqlParameter("@CINNO",CINNO),
                                      new SqlParameter("@STAXNO",STAXNO),
                                      new SqlParameter("@PANNO",PANNO),
                                      new SqlParameter("@GSTIN",GSTIN),
                                      new SqlParameter("@AdharNumber",AdhaarNo),
                                      new SqlParameter("@IDType",IDType),
                                      new SqlParameter("@IDNumber",IDNumber),
                                      new SqlParameter("@IDImagePath",IdImage),
                                      new SqlParameter("@PanImage",PanImage),
                                      new SqlParameter("@AddedBy",AddedBy),
                                 };
            DataSet ds = DBHelper.ExecuteQuery("CustomerRegistration", para);
            return ds;
        }

        public DataSet GetAllCustomers()
        {
            SqlParameter[] para ={    new SqlParameter ("@CompanyID",CompanyId),
                                      new SqlParameter ("@CompanyName",CompanyName),
                                      new SqlParameter("@CustomerCode",CustomerCode ),
                                     
                                 };
           DataSet ds = DBHelper.ExecuteQuery("GetAllCustomers", para);
           return ds;
        }

        public DataSet UpdateCustomer()
        {
            SqlParameter[] para ={    new SqlParameter ("@PK_CustomerID",CompanyId),
                                      new SqlParameter ("@CompanyName",CompanyName),
                                      new SqlParameter("@NatureOfBusiness",NatureOfBusiness),
                                      new SqlParameter("@CompanyMobile",MobileNo),
                                      new SqlParameter("@CompanyPhone",PhoneNo),
                                      new SqlParameter("@NameOfCompanyDirector",DirectorName),
                                      new SqlParameter("@DirectorContact",DirectorContact),
                                      new SqlParameter("@Email",Email),
                                      new SqlParameter("@Address",Address),
                                      new SqlParameter("@Pincode",Pincode),
                                      new SqlParameter("@State",StateName),
                                      new SqlParameter("@City",City),
                                      new SqlParameter("@ConcernPerson1",ConcertPerson1),
                                      new SqlParameter("@ConcernPerson1Contact",ConcertPersonContact1),
                                      new SqlParameter("@ConcernPerson2",ConcertPerson2),
                                      new SqlParameter("@ConcernPerson2Contact",ConcertPersonContact2),
                                      new SqlParameter("@BankName",BankName),
                                      new SqlParameter("@BankAccountNumber",AccountNo),
                                      new SqlParameter("@IFSCCode",IFSCCode),
                                      new SqlParameter("@BankAddress",BankAddress),
                                      new SqlParameter("@TANNO",TANNO),
                                      new SqlParameter("@TINNO",TINNO),
                                      new SqlParameter("@CINNO",CINNO),
                                      new SqlParameter("@STAXNO",STAXNO),
                                      new SqlParameter("@PANNO",PANNO),
                                      new SqlParameter("@GSTIN",GSTIN),
                                      new SqlParameter("@AdharNumber",AdhaarNo),
                                      new SqlParameter("@IDType",IDType),
                                      new SqlParameter("@IDNumber",IDNumber),
                                      new SqlParameter("@IDImagePath",IdImage),
                                      new SqlParameter("@PanImage",PanImage),
                                      new SqlParameter("@UpdatedBy",UpdatedBy),
                                 };
            DataSet ds = DBHelper.ExecuteQuery("UpdateCustomer", para);
            return ds;
        }

        public DataSet DeleteCustomer()
        {
            SqlParameter[] para ={    
                                      new SqlParameter ("@PK_CustomerID",CompanyId),
                                      new SqlParameter("@DeletedBy",DeletedBy ),
                                     
                                 };
            DataSet ds = DBHelper.ExecuteQuery("DeleteCustomer", para);
            return ds;
        }
    }
}