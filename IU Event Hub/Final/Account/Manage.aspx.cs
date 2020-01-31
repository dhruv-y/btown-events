using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Final;
//important data include
using System.Data.OleDb;
using System.Data;
using Microsoft.AspNet.Identity.Owin;
//email includes
using MailKit.Net.Smtp;
using MimeKit;

public partial class Account_Manage : System.Web.UI.Page
{
    protected string SuccessMessage
    {
        get;
        private set;
    }

    protected bool CanRemoveExternalLogins
    {
        get;
        private set;
    }

    private bool HasPassword(UserManager manager)
    {
        var user = manager.FindById(User.Identity.GetUserId());
        return (user != null && user.PasswordHash != null);
    }

    protected void loadVenue()
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        OleDbConnection c = new OleDbConnection(connectionString);
        c.Open();
        OleDbCommand cmd = c.CreateCommand();
        cmd.CommandType = CommandType.Text;
        // Create query
        cmd.CommandText = "SELECT V.venueName, V.location, V.facilities, V.daysAvailable, V.timeAvailable, V.capacity, V.isClosed FROM venue V WHERE owner= '"+User.Identity.GetUserName()+"'";
        //execute query
        OleDbDataReader reader = cmd.ExecuteReader();
        //check if owner has a venue
        while (reader.Read())
        {
            if (reader.GetBoolean(6) == false)
            {
                venuetxt.Text = reader.GetString(0);
                locationtxt.Text = reader.GetString(1);
                facilitiestxt.Text = reader.GetString(2);
                if (reader.GetString(3).Equals("MTWHFSS"))
                {
                    days.SelectedIndex = 0;
                }
                else if (reader.GetString(3).Equals("MWTHF"))
                {
                    days.SelectedIndex = 1;
                }
                else
                {
                    days.SelectedIndex = 2;
                }
                timetxt.Text = reader.GetString(4);
                capacitytxt.Text = Convert.ToString(reader.GetInt32(5));
            }
        }
        //close db connection for now
        c.Close();
    }

    protected void Page_Load()
    {
        UserManager manager = new UserManager();
        //check if the user has enabled 2FA or not, and render 2FA button accordingly
        if (manager.GetTwoFactorEnabled(User.Identity.GetUserId()))
        {
            twoFactorbtn.Text = "Disable Two-Factor Authentication";
        }
        //check if the user is a venue owner or a user, and make visible the correct panel
        if (manager.GetPhoneNumber(User.Identity.GetUserId()) != null)
        {
            ownerView.Visible = true;
            if (venuetxt.Text.Equals("")) {
                loadVenue();
            }
        }
        if (!IsPostBack)
        {
            // Determine the sections to render
            if (HasPassword(manager))
            {
                changePasswordHolder.Visible = true;
                if (questiontxt.Text.Equals(""))
                {
                    loadQuestion();
                }
            }
            else
            {
                changePasswordHolder.Visible = false;
            }
            CanRemoveExternalLogins = manager.GetLogins(User.Identity.GetUserId()).Count() > 1;

            // Render success message
            var message = Request.QueryString["m"];
            if (message != null)
            {
                // Strip the query string from action
                Form.Action = ResolveUrl("~/Account/Manage");

                SuccessMessage =
                    message == "ChangePwdSuccess" ? "Your password has been changed."
                    : message == "SetPwdSuccess" ? "Your password has been set."
                    : message == "RemoveLoginSuccess" ? "The account was removed."
                    : String.Empty;
                successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
            }
        }
    }

    protected void loadQuestion()
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        OleDbConnection c = new OleDbConnection(connectionString);
        c.Open();
        OleDbCommand cmd = c.CreateCommand();
        cmd.CommandType = CommandType.Text;
        // Create query
        cmd.CommandText = "SELECT U.securityQuestion, U.securityAnswer FROM users U WHERE U.userName = '"+User.Identity.GetUserName()+"'";
        //execute query
        OleDbDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            questiontxt.Text = reader.GetString(0);
            answertxt.Text = reader.GetString(1);
        }
        //close db connection for now
        c.Close();
    }

    protected void changeQuestion(object sender, EventArgs e)
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        OleDbConnection c = new OleDbConnection(connectionString);
        c.Open();
        OleDbCommand cmd = c.CreateCommand();
        cmd.CommandType = CommandType.Text;
        // Create query
        cmd.CommandText = "UPDATE users SET securityQuestion = '"+questiontxt.Text+"', securityAnswer = '"+answertxt.Text+"' WHERE userName = '"+User.Identity.GetUserName()+"'";
        //execute query
        OleDbDataReader reader = cmd.ExecuteReader();
        //close db connection for now
        c.Close();
    }

    protected void change2FA(object sender, EventArgs e)
    {
        UserManager manager = new UserManager();
        //check if the user has enabled 2FA or not, and change accordingly
        if (manager.GetTwoFactorEnabled(User.Identity.GetUserId()))
        {
            //disable
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), false);
            twoFactorbtn.Text = "Enable Two-Factor Authentication";
        }
        else
        {
            //enable
            manager.SetTwoFactorEnabled(User.Identity.GetUserId(), true);
            twoFactorbtn.Text = "Disable Two-Factor Authentication";
        }
    }

    protected void ChangePassword_Click(object sender, EventArgs e)
    {
        if (IsValid)
        {
            UserManager manager = new UserManager();
            IdentityResult result = manager.ChangePassword(User.Identity.GetUserId(), CurrentPassword.Text, NewPassword.Text);
            if (result.Succeeded)
            {
                var user = manager.FindById(User.Identity.GetUserId());
                IdentityHelper.SignIn(manager, user, isPersistent: false);
                Response.Redirect("~/Account/Manage?m=ChangePwdSuccess");
            }
            else
            {
                AddErrors(result);
            }
        }
    }

    public IEnumerable<UserLoginInfo> GetLogins()
    {
        UserManager manager = new UserManager();
        var accounts = manager.GetLogins(User.Identity.GetUserId());
        CanRemoveExternalLogins = accounts.Count() > 1 || HasPassword(manager);
        return accounts;
    }

    public void RemoveLogin(string loginProvider, string providerKey)
    {
        UserManager manager = new UserManager();
        var result = manager.RemoveLogin(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
        string msg = String.Empty;
        if (result.Succeeded)
        {
            var user = manager.FindById(User.Identity.GetUserId());
            IdentityHelper.SignIn(manager, user, isPersistent: false);
            msg = "?m=RemoveLoginSuccess";
        }
        Response.Redirect("~/Account/Manage" + msg);
    }

    private void AddErrors(IdentityResult result)
    {
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error);
        }
    }

    protected void submitbtn_Click(object sender, EventArgs e)
    {
        string venueTime = timetxt.Text;
        int venueTimeHour = Convert.ToInt32(venueTime.Substring(0, 2));
        int venueTimeHour2 = Convert.ToInt32(venueTime.Substring(6, 2));
        //check if event time range is valid
        if (venueTimeHour2 < venueTimeHour)
        {
            venuestatuslbl.Text = "Invalid Venue Time Available Range";
            return;
        }
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        OleDbConnection c = new OleDbConnection(connectionString);
        c.Open();
        OleDbCommand cmd = c.CreateCommand();
        cmd.CommandType = CommandType.Text;
        // Create query
        cmd.CommandText = "SELECT V.venueName FROM venue V WHERE V.owner = '" + User.Identity.GetUserName() + "'";
        //execute query
        OleDbDataReader reader = cmd.ExecuteReader();
        //check if owner has a venue already
        int i = 0;
        while (reader.Read())
        {
            i++;
        }
        reader.Close();
        if (i > 0)
        {
            //owner has a venue, modify details
            if (days.SelectedValue.Equals("All"))
            {

                cmd.CommandText = "UPDATE venue SET venueName = '"+venuetxt.Text+"', location = '"+locationtxt.Text+"', facilities = '"+facilitiestxt.Text+"', daysAvailable = 'MTWHFSS', timeAvailable = '" + timetxt.Text + "', capacity = " + Convert.ToInt32(capacitytxt.Text) + ", isClosed = 0 WHERE owner = '"+User.Identity.GetUserName()+ "'";
            }
            else if (days.SelectedValue.Equals("Weekdays"))
            {
                cmd.CommandText = "UPDATE venue SET venueName = '"+venuetxt.Text+"', location = '"+locationtxt.Text+"', facilities = '"+facilitiestxt.Text+"', daysAvailable = 'MTWHF', timeAvailable = '" + timetxt.Text + "', capacity = " + Convert.ToInt32(capacitytxt.Text) + ", isClosed = 0 WHERE owner = '" + User.Identity.GetUserName()+ "'";
            }
            else
            {
                cmd.CommandText = "UPDATE venue SET venueName = '"+venuetxt.Text+"', location = '"+locationtxt.Text+"', facilities = '"+facilitiestxt.Text+"', daysAvailable = 'SS', timeAvailable = '" + timetxt.Text + "', capacity = " + Convert.ToInt32(capacitytxt.Text) + ", isClosed = 0 WHERE owner = '" + User.Identity.GetUserName()+ "'";
            }
            reader = cmd.ExecuteReader();
            venuestatuslbl.Text = "Venue updated successfully!";
        }
        else
        {
            //owner doesn't have a venue, create new entry
            if (days.SelectedValue.Equals("All"))
            {
                cmd.CommandText = "INSERT INTO venue VALUES('" + venuetxt.Text + "', '" + User.Identity.GetUserName() + "', '" + locationtxt.Text + "', '" + facilitiestxt.Text + "', 'MTWHFSS', '" + timetxt.Text + "', " + Convert.ToInt32(capacitytxt.Text) + ", 0)";
            }
            else if (days.SelectedValue.Equals("Weekdays"))
            {
                cmd.CommandText = "INSERT INTO venue VALUES('" + venuetxt.Text + "', '" + User.Identity.GetUserName() + "', '" + locationtxt.Text + "', '" + facilitiestxt.Text + "', 'MWTHF', '" + timetxt.Text + "', " + Convert.ToInt32(capacitytxt.Text) + ", 0)";
            }
            else
            {
                cmd.CommandText = "INSERT INTO venue VALUES('" + venuetxt.Text + "', '" + User.Identity.GetUserName() + "', '" + locationtxt.Text + "', '" + facilitiestxt.Text + "', 'SS', '" + timetxt.Text + "', " + Convert.ToInt32(capacitytxt.Text) + ", 0)";
            }
            reader = cmd.ExecuteReader();
            venuestatuslbl.Text = "Venue added successfully!";
        }
        //close db connection for now
        c.Close();
    }

    protected void deletebtn_Click(object sender, EventArgs e)
    {
        loadVenue();
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        OleDbConnection c = new OleDbConnection(connectionString);
        c.Open();
        OleDbCommand cmd = c.CreateCommand();
        cmd.CommandType = CommandType.Text;
        //First alert all users who have scheduled an event at this venue and all people who have signed up for those events
        // Create query
        cmd.CommandText = "SELECT E.owner, E.eventName, E.eventDate FROM event E WHERE E.location = '" + venuetxt.Text + "'";
        OleDbDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            //check if date is in the past, if so leave it be
            if (!inPast(reader.GetString(2))) {
                notifyEvent(reader.GetString(0), reader.GetString(1), venuetxt.Text);
            }
        }
        reader.Close();
        var manager = new UserManager();
        cmd.CommandText = "UPDATE venue SET isClosed = 1 WHERE owner = '" + manager.GetEmail(User.Identity.GetUserId()) + "'";
        //execute query
        reader = cmd.ExecuteReader();
        //close db connection for now
        c.Close();
        venuetxt.Text = "";
        locationtxt.Text = "";
        facilitiestxt.Text = "";
        capacitytxt.Text = "";
        days.SelectedIndex = 0;
        timetxt.Text = "";
        venuestatuslbl.Text = "Venue removed successfully!";
    }

    protected void notifyEvent(string eventOwner, string eventName, string venueName)
    {
        var manager = new UserManager();
        //send an email to the event creator
        sendEmail(eventOwner, "We are emailing you to inform you that the venue, " + venueName + ", you booked for the event, " + eventName + ", has been closed, and therefore the event has been cancelled.", "IU Event Hub - Event Cancellation");
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        OleDbConnection c = new OleDbConnection(connectionString);
        c.Open();
        OleDbCommand cmd = c.CreateCommand();
        cmd.CommandType = CommandType.Text;
        //query the users who signed up for the event
        cmd.CommandText = "SELECT AE.attendee FROM attendedEvents AE WHERE AE.event = '" + eventName + "'";
        OleDbDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            sendEmail(reader.GetString(0), "We are emailing you to inform you that the event, " + eventName + ", you signed up for has been cancelled.", "IU Event Hub - Event Cancellation");
            OleDbCommand cmd2 = c.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "DELETE FROM attendedEvents WHERE event = '" + eventName + "' AND attendee = '" + reader.GetString(0) + "'";
            OleDbDataReader reader2 = cmd2.ExecuteReader();
        }
        //remove event from database
        reader.Close();
        cmd.CommandText = "DELETE FROM venueEvents WHERE event = '" + eventName + "'";
        reader = cmd.ExecuteReader();
        reader.Close();
        cmd.CommandText = "DELETE FROM event WHERE eventName = '" + eventName + "'";
        reader = cmd.ExecuteReader();
        //lastly send an email to the venue owner how they closed the venue
        sendEmail(manager.GetEmail(User.Identity.GetUserId()), "You have successfully closed the " + venueName + " venue you own, we have also alerted the event creator and those who registered for the event.", "IU Event Hub - Venue Closure");
        //close db connection for now
        c.Close();
    }

    protected Boolean inPast(String date)
    {
        //String today = Convert.ToString(DateTime.Now);
        //11/8/2019 1:44:48 PM
        String[] eventDate = date.Split('/');
        String[] today = Convert.ToString(DateTime.Now).Split(' ');
        today = today[0].Split('/');
        if (Convert.ToInt32(eventDate[2]) < Convert.ToInt32(today[2]))
        {
            return true;
        }
        else if (Convert.ToInt32(eventDate[1]) < Convert.ToInt32(today[1]))
        {
            return true;
        }
        else if (Convert.ToInt32(eventDate[0]) < Convert.ToInt32(today[0]))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void sendEmail(string userEmail, string content, string subject)
    {
        //From Address
        string FromAddress = "email";
        string FromAdressTitle = "Btown Events";
        //To Address
        string ToAddress = userEmail;
        string ToAdressTitle = "Microsoft ASP.NET Core";
        //string Subject = "IUEventHub - Event Cancellation";
        string Subject = subject;
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