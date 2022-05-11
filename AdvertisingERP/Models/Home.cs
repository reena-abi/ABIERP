using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AdvertisingERP.Models
{
    public class Home:Common
    {
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public string PK_VendorID { get; set; }
        public string Address { get; set; }
        public string ProfilePic { get; set; }
        

        public DataSet Login()
        {
            //LoginProc
            SqlParameter[] para ={new SqlParameter ("@UserName",LoginId),
                                new SqlParameter("@Password",Password)};
            DataSet ds = DBHelper.ExecuteQuery("LoginProc", para);
            return ds;
        }

        public DataSet PasswordForget()
        {
            SqlParameter[] para =
                {
                new SqlParameter("",LoginId),
                new SqlParameter("",Email)

            };
            DataSet ds = DBHelper.ExecuteQuery("", para);
            return ds;
        }
        
    }
}