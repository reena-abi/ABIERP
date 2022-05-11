using AdvertisingERP.Filter;
using AdvertisingERP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace AdvertisingERP.Controllers
{
    public class HomeController : AdminBaseController
    {
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult Login()
        {
            // Session.Abandon();
            Home obj = new Home();
            if (TempData["Login"] == null)
            {
                ViewBag.errormsg = "none";
            }
            return View(obj);
        }
        [HttpPost]
        [ActionName("Login")]
        [OnAction(ButtonName= "btnlogin")]
        public ActionResult LoginAction(Home obj)
        {
            if (obj.LoginId == null)
            {
                ViewBag.errormsg = "";
                TempData["Login"] = "Please Enter LoginId";
                return RedirectToAction("Login");

            }
            if (obj.Password == null)
            {
                ViewBag.errormsg = "";
                TempData["Login"] = "Please Enter Password";
                return RedirectToAction("Login");
            }
            if (obj.LoginId.Trim() == "")
            {
                ViewBag.errormsg = "";
                TempData["Login"] = "Please Enter LoginId";
                return RedirectToAction("Login");
               
            }
            if (obj.Password.Trim() == "")
            {
                ViewBag.errormsg = "";
                TempData["Login"] = "Please Enter Password";
                return RedirectToAction("Login");

            }
          
            try
            {
                Home Modal = new Home();
                DataSet ds = obj.Login();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Msg"].ToString() == "1")
                    {
                        ViewBag.errormsg = "";
                        Session["UserID"] = ds.Tables[0].Rows[0]["Pk_Id"].ToString();
                        Session["LoginID"] = ds.Tables[0].Rows[0]["LoginId"].ToString();
                        Session["Username"] = ds.Tables[0].Rows[0]["Name"].ToString();
                        Session["EmailId"] = ds.Tables[0].Rows[0]["EmailId"].ToString();
                        Session["ProfilePic"] = ds.Tables[0].Rows[0]["ProfilePic"].ToString();
                        
                        return RedirectToAction("DashBoard","Admin");
                    }
                    else
                    {
                        ViewBag.errormsg = "";
                        TempData["Login"] = "Incorrect LoginId Or Password";
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    ViewBag.errormsg = "";
                    TempData["Login"] = "Incorrect LoginId Or Password";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                ViewBag.errormsg = "";
                TempData["Login"] = ex.Message;
                return RedirectToAction("Login");
               
            }
        }
        public ActionResult UserProfile()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        

        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ActionName("ForgetPassword")]
        [OnAction(ButtonName = "btnforgetpassword")]
        public ActionResult ChangePassword(Home model)
        {
            if (model.LoginId == null)
            {
                ViewBag.errormsg = "";
                TempData["Error"] = "Please Enter LoginId";
                return RedirectToAction("ForgetPassword");

            }
            if (model.Email == null)
            {
                ViewBag.errormsg = "";
                TempData["Error"] = "Please Enter EmailId";
                return RedirectToAction("ForgetPassword");
            }
            DataSet ds = model.PasswordForget();
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0].ToString() == "1")
                {
                    try
                    {
                        if (model.Email != null)
                        {
                            string mailbody = "";
                            mailbody = "Dear Member,<br> your Passoword is : " + ds.Tables[0].Rows[0]["Password"].ToString();
                            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient
                            {
                                Host = "smtp.gmail.com",
                                Port = 587,
                                EnableSsl = true,
                                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                                UseDefaultCredentials = true,
                                Credentials = new NetworkCredential("developer5.afluex@gmail.com", "Afluex@123")
                            };
                            using (var message = new MailMessage("developer5.afluex@gmail.com", model.Email)
                            {
                                IsBodyHtml = true,
                                Subject = "Recover Password",
                                Body = mailbody
                            })
                                smtp.Send(message);
                            TempData["Success"] = "Your Password Has Been Send On your EmailId";

                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["Error"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                    }

                }
                else
                {
                    TempData["Error"] = ds.Tables[0].Rows[0]["ErrorMessage"].ToString();
                }
            }

            return RedirectToAction("ForgetPassword", "Home");
        }
    }
}

