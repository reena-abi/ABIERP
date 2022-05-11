using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdvertisingERP.Models
{
    public class Vendor:Common
    {
        public string VendorName { get; set; }
        public string PanImage { get; set; }
        public string NatureOfBusiness { get; set; }
        public string ServiceTypeID { get; set; }
        public string ServiceTypeName { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string GSTNO { get; set; }
        public string BankID { get; set; }
        public string BankName { get; set; }
        public string BankNameSplit { get; set; }
        public string AccountNo { get; set; }
        public string IFSCCode { get; set; }
        public string PANNO { get; set; }
        public string ConcernPerson { get; set; }
        public string ConcerPersonEmail { get; set; }
        public string ConcerPersonContact { get; set; }
        public string ConcernPerson2 { get; set; }
        public string ConcerPersonEmail2 { get; set; }
        public string ConcerPersonContact2 { get; set; }
        public string ConcerPerson1Designation { get; set; }
        public string ConcerPerson2Designation { get; set; }
        public string ServiceTypeNameDisplay { get; set; }
        public string VendorId { get; set; }

        public List<Vendor> lstVendor { get; set; }

        public DataSet GetBankList()
        {
            SqlParameter[] para = { new SqlParameter("@PK_BankID", BankID), };
            DataSet ds = DBHelper.ExecuteQuery("GetBankList", para);
            return ds;
        }

        public DataSet GetAllVendors()
        {
            SqlParameter[] para ={ new SqlParameter ("@VendorCode", VendorId),
                                    new SqlParameter("@Name",VendorName ), };
            DataSet ds = DBHelper.ExecuteQuery("GetVendorDetails", para);
            return ds;
        }

        public DataSet VendorRegistration()
        {
            SqlParameter[] para ={    
                                      new SqlParameter ("@Name",VendorName),
                                      new SqlParameter("@Contact",MobileNo ),
                                      new SqlParameter("@Email",Email ),
                                      new SqlParameter("@Address",Address ),
                                      new SqlParameter("@AddedBy",AddedBy ),
                                      new SqlParameter("@PinCode",Pincode ),
                                      new SqlParameter("@State",StateName ),
                                      new SqlParameter("@City",City ),
                                      new SqlParameter("@BankName",BankNameSplit ),
                                      new SqlParameter("@AccountNumber",AccountNo ),
                                      new SqlParameter("@IFSCCode",IFSCCode ),
                                      new SqlParameter("@ConcernPersonName",ConcernPerson ),
                                      new SqlParameter("@ConcernPersonContact",ConcerPersonContact ),
                                      new SqlParameter("@ConcernPersonEmail",ConcerPersonEmail ),
                                      new SqlParameter("@ConcernPersonName2",ConcernPerson2 ),
                                      new SqlParameter("@ConcernPersonContact2",ConcerPersonContact2 ),
                                      new SqlParameter("@ConcernPersonEmail2",ConcerPersonEmail2 ),
                                      new SqlParameter("@GSTNo",GSTNO ),
                                      new SqlParameter("@PanNo",PANNO ),
                                      new SqlParameter("@NatureOfBusiness",NatureOfBusiness ),
                                      new SqlParameter("@ServiceTypeID", ServiceTypeID),
                                      new SqlParameter("@PanImage", PanImage),
                                      new SqlParameter("@ConcernPerson1Designation", ConcerPerson1Designation),
                                      new SqlParameter("@ConcernPerson2Designation", ConcerPerson2Designation),
                                 };
            DataSet ds = DBHelper.ExecuteQuery("VendorRegistration", para);
            return ds;
        }

        public DataSet UpdateVendor()
        {
            SqlParameter[] para ={ new SqlParameter ("@Name",VendorName),
                                     new SqlParameter("@Contact",MobileNo ),
                                       new SqlParameter("@Email",Email ),
                                        new SqlParameter("@Address",Address ),
                                         new SqlParameter("@UpdatedBy",UpdatedBy ),
                                          new SqlParameter("@PinCode",Pincode ),
                                           new SqlParameter("@State",StateName ),
                                            new SqlParameter("@City",City ),
                                           new SqlParameter("@BankName",BankNameSplit ),
                                           new SqlParameter("@AccountNumber",AccountNo ),
                                           new SqlParameter("@IFSCCode",IFSCCode ),
                                           new SqlParameter("@ConcernPersonName",ConcernPerson ),
                                           new SqlParameter("@ConcernPersonContact",ConcerPersonContact ),
                                           new SqlParameter("@ConcernPersonEmail",ConcerPersonEmail ),
                                           new SqlParameter("@ConcernPersonName2",ConcernPerson2 ),
                                            new SqlParameter("@ConcernPersonContact2",ConcerPersonContact2 ),
                                            new SqlParameter("@ConcernPersonEmail2",ConcerPersonEmail2 ),
                                           new SqlParameter("@GSTNo",GSTNO ),
                                           new SqlParameter("@PanNo",PANNO ),
                                            new SqlParameter("@VendorCode",VendorCode ),
                                            new SqlParameter("@NatureOfBusiness",NatureOfBusiness ),
                                            new SqlParameter("@ServiceTypeID", ServiceTypeID),
                                            new SqlParameter("@ConcernPerson1Designation", ConcerPerson1Designation),
                                      new SqlParameter("@ConcernPerson2Designation", ConcerPerson2Designation),
                                       new SqlParameter("@PanImage", PanImage),
                                 };
            DataSet ds = DBHelper.ExecuteQuery("UpdateVendorRegistration", para);
            return ds;
        }

        public DataSet DeleteVendor()
        {
            SqlParameter[] para ={    
                                      new SqlParameter ("@VendorCode",VendorId),
                                      new SqlParameter("@Deletedby",DeletedBy ),
                                     
                                 };
            DataSet ds = DBHelper.ExecuteQuery("DeleteVendor", para);
            return ds;
        }
    }
}