using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace AdvertisingERP.Models
{
    public class Common
    {
        public string DeletedBy { get; set; }
        public string StateName { get; set; }
        public string VendorCode { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string AddedBy { get; set; }
        public string City { get; set; }
        public string Pincode { get; set; }
        public string UpdatedBy { get; set; }
        public string ServiceTypeNameSO { get; set; }
        public string FinancialYearID { get; set; }
        public string FinancialYearName { get; set; }

        public DataSet BindVendor()
        {
            SqlParameter[] para ={new SqlParameter ("@VendorCode",VendorCode),
                                new SqlParameter("@Name",Name),
                                  new SqlParameter("@MobileNo",MobileNo)};
            DataSet ds = DBHelper.ExecuteQuery("GetVendorDetails", para);
            return ds;
        }

        public DataSet GetMediaVehicle()
        {
            DataSet ds = DBHelper.ExecuteQuery("GetMediaVehicle");
            return ds;
        }

        public DataSet GetMediaType()
        {
            SqlParameter[] para ={new SqlParameter ("@ServiceType", ServiceTypeNameSO), };
            DataSet ds = DBHelper.ExecuteQuery("GetMediaType", para);
            return ds;
        }

        public DataSet GetStateCity()
        {
            SqlParameter[] para ={new SqlParameter ("@PinCode",Pincode),
                               };
            DataSet ds = DBHelper.ExecuteQuery("GetStateCity", para);
            return ds;
        }

        public string Result { get; set; }

        public static List<SelectListItem> BindDateFormat()
        {
            List<SelectListItem> ddldateformat = new List<SelectListItem>();
            ddldateformat.Add(new SelectListItem { Text = "Single", Value = "Single" });
            ddldateformat.Add(new SelectListItem { Text = "Double", Value = "Double" });
         
            return ddldateformat;
        }
        public static List<SelectListItem> ddlCrDr()
        {
            List<SelectListItem> ddlCrDr = new List<SelectListItem>();
            ddlCrDr.Add(new SelectListItem { Text = "-Select-", Value = "" });
            ddlCrDr.Add(new SelectListItem { Text = "Credit", Value = "Cr" });
            ddlCrDr.Add(new SelectListItem { Text = "Debit", Value = "Dr" });

            return ddlCrDr;
        }
        public static List<SelectListItem> ddlCrDr1()
        {
            List<SelectListItem> ddlcrdr = new List<SelectListItem>();
            ddlcrdr.Add(new SelectListItem { Text = "Credit", Value = "Cr" });
            ddlcrdr.Add(new SelectListItem { Text = "Debit", Value = "Dr" });
            return ddlcrdr;
        }
        public static List<SelectListItem> BindPaymentTerms()
        {
            List<SelectListItem> PaymentMode = new List<SelectListItem>();
            PaymentMode.Add(new SelectListItem { Text="Please Select ",Value="0" });
            PaymentMode.Add(new SelectListItem { Text="Monthly",Value="1" });
            PaymentMode.Add(new SelectListItem { Text="Quarterly",Value="2" });
            PaymentMode.Add(new SelectListItem { Text="Yearly",Value="3" });
            PaymentMode.Add(new SelectListItem { Text="Immediate",Value="4" });
            return PaymentMode;
        }
        public static List<SelectListItem> BillingSnaps()
        {
            List<SelectListItem> PaymentMode = new List<SelectListItem>();
            PaymentMode.Add(new SelectListItem { Text = "Please Select ", Value = "0" });
            PaymentMode.Add(new SelectListItem { Text="Start Date-End Date",Value="1" });
            PaymentMode.Add(new SelectListItem { Text="Start Date-Mid Date-End Date",Value="2" });
            return PaymentMode;
        }
        public static List<SelectListItem> POReceived()
        {
            List<SelectListItem> PaymentMode = new List<SelectListItem>();
            PaymentMode.Add(new SelectListItem { Text = "Please Select ", Value = "0" });
            PaymentMode.Add(new SelectListItem { Text="Yes",Value="1" });
            PaymentMode.Add(new SelectListItem { Text="Awaited",Value="2" });
            PaymentMode.Add(new SelectListItem { Text = "Billing on Mail Confirmation", Value = "3" });
            return PaymentMode;
        }
        public static string ConvertToSystemDate(string InputDate, string InputFormat)
        {
            string DateString = "";
            DateTime Dt;

            string[] DatePart = (InputDate).Split(new string[] { "-", @"/" }, StringSplitOptions.None);

            if (InputFormat == "dd-MMM-yyyy" || InputFormat == "dd/MMM/yyyy" || InputFormat == "dd/MM/yyyy" || InputFormat == "dd-MM-yyyy")
            {
                string Day = DatePart[0];
                string Month = DatePart[1];
                string Year = DatePart[2];

                if (Month.Length > 2)
                    DateString = InputDate;
                else
                    DateString = Month + "/" + Day + "/" + Year;
            }
            else if (InputFormat == "MM/dd/yyyy" || InputFormat == "MM-dd-yyyy")
            {
                DateString = InputDate;
            }
            else
            {
                throw new Exception("Invalid Date");
            }

            try
            {
                //Dt = DateTime.Parse(DateString);
                //return Dt.ToString("MM/dd/yyyy");
                return DateString;
            }
            catch
            {
                throw new Exception("Invalid Date");
            }

        }
    }

    public class CompanyProfile
    {
        static public string CompanyName = "Afluex Multiservices LLP";
        static public string CompanyMobile = "7310000412";
        static public string CompanyEmail = "supportnow@afluex.com";
        static public string CompanyAddress = "D-54,Second Floor, Arjun Tower,Near OLA Office,Vibhuti Khand,Gomti Nagar, Lucknow";
        static public string BankName = "IndusInd Bank Limited";
        static public string AccountNo = "201001661706";
        static public string IFSC = "INDB0000542";
        static public string PAN = "ABJFA3997N";
        static public string GSTIN = "09ABJFA3997N1ZK";
    }

}
