using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Web;
using System.Web.UI;
using Final;
//important data include
using System.Data.OleDb;
using System.Data;
using System.Linq;

public partial class Account_Login : Page
{
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register";
            ForgotPasswordHyperLink.NavigateUrl = "ForgotPassword";
            OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (IsValid)
            { 
                var manager = new UserManager();
                ApplicationUser user = manager.Find(UserName.Text, Password.Text);
                if (user != null)
                {
                    IdentityHelper.SignIn(manager, user, RememberMe.Checked);
                    if (manager.GetTwoFactorEnabled(user.Id))
                    {
                        Response.Redirect("~/Account/TwoFactorAuthorization");
                    }
                    else
                    {
                        Response.Redirect("~/Pages/Dashboard");
                    }
                }
                else
                {
                    FailureText.Text = "Invalid username or password.";
                    ErrorMessage.Visible = true;
                }
            }
        }
}