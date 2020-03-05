using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2FAGoogleAuthenticator.ViewModel
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class MFAModel
    {
        public string BarcodeImageUrl { get; set; }

        public string SetupCode { get; set; }

        public bool UniqueKey { get; set; }

        public string Passcode { get; set; }

        public bool status { get; set; }

        public string Message { get; set; }
    }
}