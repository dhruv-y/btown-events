using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Web;
using Final;
//important data include
using System.Data.OleDb;
using System.Data;
using Microsoft.AspNet.Identity.Owin;

public partial class Account_RegisterExternalLogin : System.Web.UI.Page
{
    protected string ProviderName
    {
        get { return (string)ViewState["ProviderName"] ?? String.Empty; }
        private set { ViewState["ProviderName"] = value; }
    }

    protected string ProviderAccountKey
    {
        get { return (string)ViewState["ProviderAccountKey"] ?? String.Empty; }
        private set { ViewState["ProviderAccountKey"] = value; }
    }

    protected void Page_Load()
    {
        // Process the result from an auth provider in the request
        ProviderName = IdentityHelper.GetProviderNameFromRequest(Request);
        if (String.IsNullOrEmpty(ProviderName))
        {
            Response.Redirect("~/Account/Login");
        }
        if (!IsPostBack)
        {
            var manager = new UserManager();
            var loginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo();
            if (loginInfo == null)
            {
                Response.Redirect("~/Account/Login");
            }
            var user = manager.Find(loginInfo.Login);
            if (user != null)
            {
                IdentityHelper.SignIn(manager, user, isPersistent: false);
                Response.Redirect("~/Pages/Dashboard");
                //IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else if (User.Identity.IsAuthenticated)
            {
                // Apply Xsrf check when linking
                var verifiedloginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo(IdentityHelper.XsrfKey, User.Identity.GetUserId());
                if (verifiedloginInfo == null)
                {
                    Response.Redirect("~/Account/Login");
                }

                var result = manager.AddLogin(User.Identity.GetUserId(), verifiedloginInfo.Login);
                if (result.Succeeded)
                {
                    Response.Redirect("~/Pages/Dashboard");
                    //IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
                else
                {
                    AddErrors(result);
                    return;
                }
            }
            else
            {
                userName.Text = loginInfo.DefaultUserName;
                CreateAndLoginUser(loginInfo.Email);
            }
        }
    }

    private void CreateAndLoginUser(string email)
    {
        var manager = new UserManager();
        var user = new ApplicationUser() { UserName = email };
        IdentityResult result = manager.Create(user);
        if (result.Succeeded)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
            OleDbConnection c = new OleDbConnection(connectionString);
            c.Open();
            OleDbCommand cmd = c.CreateCommand();
            cmd.CommandType = CommandType.Text;
            // Create query
            cmd.CommandText = "INSERT INTO users VALUES ('"+email+"', 0, null, null)";
            //execute query
            OleDbDataReader reader = cmd.ExecuteReader();
            c.Close();
            manager.SetEmail(user.Id, email);
            var loginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo();
            if (loginInfo == null)
            {
                Response.Redirect("~/Account/Login");
                return;
            }
            result = manager.AddLogin(user.Id, loginInfo.Login);
            if (result.Succeeded)
            {
                IdentityHelper.SignIn(manager, user, isPersistent: false);
                Response.Redirect("~/Pages/Dashboard");
                return;
            }
        }
        AddErrors(result);
    }

    protected void LogIn_Click(object sender, EventArgs e)
    {
        CreateAndLoginUser();
    }

    private void CreateAndLoginUser()
    {
        if (!IsValid)
        {
            return;
        }
        var manager = new UserManager();
        var user = new ApplicationUser() { UserName = userName.Text };
        IdentityResult result = manager.Create(user);
        if (result.Succeeded)
        {
            manager.SetEmail(user.Id, userName.Text);
            var loginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo();
            if (loginInfo == null)
            {
                Response.Redirect("~/Account/Login");
                return;
            }
            result = manager.AddLogin(user.Id, loginInfo.Login);
            if (result.Succeeded)
            {
                IdentityHelper.SignIn(manager, user, isPersistent: false);
                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                return;
            }
        }
        AddErrors(result);
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error);
        }
    }
}