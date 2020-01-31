using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.UI;
using Final;
using Newtonsoft.Json;
using System.Collections.Generic;
//important data include
using System.Data.OleDb;
using System.Data;
using Microsoft.AspNet.Identity.Owin;

public partial class Account_Register : Page
{
    protected void CreateUser_Click(object sender, EventArgs e)
    {
        string response = Request.Form["g-Recaptcha-Response"];
        bool isValid;
        if (ReCaptchaClass.Validate(response) == "true")
        {
            isValid = true;
        } else
        {
            isValid = false;
        }
        if (isValid)
        {
            var manager = new UserManager();
            var user = new ApplicationUser() { UserName = UserName.Text };
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                if (isVenueOwner.Checked)
                {
                    manager.SetPhoneNumber(user.Id,"");
                }
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
                OleDbConnection c = new OleDbConnection(connectionString);
                c.Open();
                OleDbCommand cmd = c.CreateCommand();
                cmd.CommandType = CommandType.Text;
                // Create query
                if (isVenueOwner.Checked)
                {
                    cmd.CommandText = "INSERT INTO users VALUES ('"+UserName.Text+"', 1, '"+""+"', '"+""+"')";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO users VALUES ('"+UserName.Text+"', 0, '"+""+"', '"+""+"')";
                }
                //execute query
                OleDbDataReader reader = cmd.ExecuteReader();
                c.Close();
                manager.SetEmail(user.Id, UserName.Text);
                Response.Redirect("~/Account/Login");
            }
            else
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }
        else
        {
            ErrorMessage.Text = "ReCaptcha Failed";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}

public class ReCaptchaClass
{
    public static string Validate(string EncodedResponse)
    {
        var client = new System.Net.WebClient();

        string PrivateKey = "6Le3Zb0UAAAAAHF5wNlKj_sqX2LXGOhEiDJdA-0V";

        var GoogleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", PrivateKey, EncodedResponse));

        var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ReCaptchaClass>(GoogleReply);

        return captchaResponse.Success.ToLower();
    }

    [JsonProperty("success")]
    public string Success
    {
        get { return m_Success; }
        set { m_Success = value; }
    }

    private string m_Success;
    [JsonProperty("error-codes")]
    public List<string> ErrorCodes
    {
        get { return m_ErrorCodes; }
        set { m_ErrorCodes = value; }
    }


    private List<string> m_ErrorCodes;
}