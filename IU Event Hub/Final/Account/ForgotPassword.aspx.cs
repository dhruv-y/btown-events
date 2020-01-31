using Final;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//email includes
using MailKit.Net.Smtp;
using MimeKit;
//important data include
using System.Data.OleDb;
using System.Data;

public static class globals2
{
    public static string temp;
}

public partial class Account_ForgotPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void ResetPass(object sender, EventArgs e)
    {
        if (answer.Text.Equals(globals2.temp)) {
            var manager = new UserManager();
            ApplicationUser user = manager.FindByName(Reset.Text);
            if (user != null)
            {
                //generate new temporary password
                string temp = RandomString(10);
                //change password
                manager.RemovePassword(user.Id);
                manager.AddPassword(user.Id, temp);
                //send an email to the user saying theyre password has changed
                sendEmail(manager.GetEmail(user.Id), "We have received a password reset request for your account, your new temporary password is: " + temp);
                errorlbl.Text = "Email sent successfully!";
            }
            else
            {
                errorlbl.Text = "Could not find user.";
            }
        } else
        {
            errorlbl.Text = "Incorrect security question.";
        }
    }

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
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
        string Subject = "IUEventHub - Password Reset";
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

    protected void GetQuestion(object sender, EventArgs e)
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString; 
              OleDbConnection c = new OleDbConnection(connectionString);
        c.Open();
        OleDbCommand cmd = c.CreateCommand();
        cmd.CommandType = CommandType.Text;
        // Create query
        cmd.CommandText = "SELECT U.securityQuestion, U.securityAnswer FROM users U WHERE U.userName = '"+Reset.Text+"'";
        //execute query
        OleDbDataReader reader = cmd.ExecuteReader();
        int i = 0;
        while (reader.Read())
        {
            if (reader.GetString(0).Equals(""))
            {
                ResetPass(sender, e);
                return;
            }
            i++;
            questionlbl.Text += reader.GetString(0);
            questionlbl.Visible = true;
            answer.Visible = true;
            resetbtn.Visible = true;
            globals2.temp = reader.GetString(1);
        }
        if (i == 0)
        {
            errorlbl.Text = "Could not find user.";
        }
        c.Close();
    }
}