using _2FAGoogleAuthenticator.ViewModel;
using Google.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace _2FAGoogleAuthenticator.Controllers
{
    public class HomeController : Controller
    {
        private const string KEY = "qaz123!@@)(*";
        private static string UserUniqueKey = string.Empty;
        public ActionResult Login()
        {
            return View();
        }

        public string GetUserSession()
        {
            return Session["UserUniqueKey"].ToString();
        }

        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            string message = "";
            bool status = false;
            //check username and password from database here
            if(login.UserName == "Admin" && login.Password == "Password1")
            {
                status = true;
                message = "2FA Verification";
                Session["Username"] = login.UserName;

                //2FA Setup
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                string UserUniqueKey = login.UserName + KEY;
                Session["UserUniqueKey"] = UserUniqueKey;
                TempData["UniqueKey"] = UserUniqueKey;
                var setUpInfo = tfa.GenerateSetupCode("Dotnet", login.UserName, UserUniqueKey, false, 300);
                ViewBag.BarcodeImageUrl = setUpInfo.QrCodeSetupImageUrl;
                ViewBag.SetupCode = setUpInfo.ManualEntryKey;
            }
            else
            {
                message = "Invalid Credential....!!!";
            }
            ViewBag.Message = message;
            ViewBag.Status = status;
            return View();
        }

        [HttpPost]
        public ActionResult LoginFromAngular(LoginModel login)
        {
           
            MFAModel mfaModel = new MFAModel();
          
            if (login.UserName == "Admin" && login.Password == "Password1")
            {

                mfaModel.status = true;
                mfaModel.Message = "2FA Verification";
                Session["Username"] = login.UserName;

                //2FA Setup
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                UserUniqueKey = login.UserName + KEY;
                System.Web.HttpContext.Current.Session["UserUniqueKey"] = UserUniqueKey;
                var setUpInfo = tfa.GenerateSetupCode("Dotnet", login.UserName, UserUniqueKey, false, 300);
                mfaModel.BarcodeImageUrl = setUpInfo.QrCodeSetupImageUrl;
                mfaModel.SetupCode = setUpInfo.ManualEntryKey;
            }
            else
            {
                mfaModel.status = false;
                mfaModel.Message = "Invalid Credential....!!!";
            }

            var jsonResult = Json(mfaModel, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = Int32.MaxValue;
            return jsonResult;
        }

        public ActionResult MyProfile()
        {
            if(Session["Username"] == null || Session["IsValid2FA"] == null || !(bool)Session["IsValid2FA"])
            {
                return RedirectToAction("Login");
            }
            ViewBag.Message = "Welcome " + Session["Username"];
            return View();
        }
    
        public ActionResult Verify2FA()
        {
            var token = Request["passcode"];
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            string UserUniqueKey = Session["UserUniqueKey"].ToString();
            bool isValid = tfa.ValidateTwoFactorPIN(UserUniqueKey, token);
            if (isValid)
            {
                Session["IsValid2FA"] = true;
                return RedirectToAction("MyProfile", "Home");
            }
            return RedirectToAction("Login", "Home");
        }
        public JsonResult Verify2FAAngular(string passCode)
        {
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

            bool isValid = tfa.ValidateTwoFactorPIN(UserUniqueKey, passCode);
            return Json((isValid) ? true : false, JsonRequestBehavior.AllowGet);
        }

    }
}