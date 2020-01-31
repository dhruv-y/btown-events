using Final;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//important email includes
using MailKit.Net.Smtp;
using MimeKit;

public static class globals
{
    public static string temp;
}

public partial class Account_TwoFactorAuthorization : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (globals.temp == null)
        {
            var manager = new UserManager();
            globals.temp = RandomString(6);
            sendEmail(manager.GetEmail(User.Identity.GetUserId()), "We have received a login request for your account, your 6 digit 2FA number is: " + globals.temp);
        }
    }

    protected void loginbtn_Click(object sender, EventArgs e)
    {
        if (codetxt.Text.Equals(globals.temp))
        {
            Response.Redirect("~/Pages/Dashboard");
        }
        else
        {
            errorlbl.Text = "2FA number incorrect";
        }
    }

    public static string RandomString(int length)
    {
        const string chars = "0123456789";
        Random random = new Random();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static void sendEmail(string userEmail, string content)
    {
        //From Address
        string FromAddress = "email";
        string FromAdressTitle = "Btown Events";
        //To Address
        string ToAddress = userEmail;
        string ToAdressTitle = "Microsoft ASP.NET Core";
        string Subject = "IUEventHub - 2FA Code";
        string BodyContent = content;

        //Smtp Server
        string SmtpServer = "smtp.gmail.com";
        //Smtp Port Number
        int SmtpPortNumber = 587;

        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(FromAdressTitle, FromAddress));
        mimeMessage.To.Add(new MailboxAddress(ToAdressTitle, ToAddress));
        mimeMessage.Subject = Subject;
        mimeMessage.Body = new TextPart("plain")
        {
            Text = BodyContent

        };

        using (var client = new SmtpClient())
        {
            client.Connect(SmtpServer, SmtpPortNumber, false);
            // Note: only needed if the SMTP server requires authentication
            // Error 5.5.1 Authentication 
            client.Authenticate("email", "pass");
            client.Send(mimeMessage);
            client.Disconnect(true);
        }
    }
}