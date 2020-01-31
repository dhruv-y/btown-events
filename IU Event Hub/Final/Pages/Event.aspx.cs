using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Final;
//important data include
using System.Data.OleDb;
using System.Data;
//email includes
using MailKit.Net.Smtp;
using MimeKit;

public partial class Pages_Event : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        eventNamelbl.Text = Request.QueryString["EVENT"];
        var manager = new UserManager();
        //load venue details
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        OleDbConnection c = new OleDbConnection(connectionString);
        c.Open();
        OleDbCommand cmd = c.CreateCommand();
        cmd.CommandType = CommandType.Text;
        // Create query
        cmd.CommandText = "SELECT E.owner, E.eventDate, E.location, E.category, E.capacity, E.cost, E.schedule, E.description FROM event E WHERE E.eventName = '" + Request.QueryString["EVENT"] + "'";
        //execute query

        OleDbDataReader reader = cmd.ExecuteReader();
        //show results

        while (reader.Read())
        {
            ownerlbl.Text = reader.GetString(0);
            eventDatelbl.Text = reader.GetString(1);
            locationlbl.Text = reader.GetString(2);
            categorylbl.Text = reader.GetString(3);
            int cap = 0;
            cap = reader.GetInt32(4);
            if (cap <= 0)
            {
                cap = cap - 1;
                capacitylbl2.Text = "Waitlist";
                capacitylbl.Text = Convert.ToString(Math.Abs(cap));
                resultslbl.Text = "Unfortunately there are no slots left available for this event. You are number" + (Math.Abs(cap)) + "in the waitlist";

            }
            else
            {
                capacitylbl.Text = Convert.ToString(reader.GetInt32(4));
            }
            costlbl.Text = Convert.ToString(reader.GetInt32(5));
            timelbl.Text = reader.GetString(6);
            descriptionlbl.Text = reader.GetString(7);


        }
        reader.Close();
        cmd.CommandText = "SELECT V.location FROM venue V WHERE V.venueName = '" + locationlbl.Text + "'";
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            addresslbl.Text = reader.GetString(0);

        }
        reader.Close();
        cmd.CommandText = "SELECT AE.attendee FROM attendedEvents AE WHERE AE.event = '" + eventNamelbl.Text + "' AND AE.attendee = '" + manager.GetEmail(User.Identity.GetUserId()) + "'";
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            eventbtn.OnClientClick = "";
            eventbtn.Text = "Cancel RSVP";
        }
        //close db connection for now
        c.Close();

        if (manager.GetEmail(User.Identity.GetUserId()).Equals(ownerlbl.Text))
        {
            Cancelpnl.Visible = true;
        }
        else
        {
            RSVPpnl.Visible = true;
        }
        if (manager.GetPhoneNumber(User.Identity.GetUserId()) != null)
        {
            Cancelpnl.Visible = false;
            RSVPpnl.Visible = false;
        }
    }

    protected void JoinEvent_Click(object sender, EventArgs e)
    {
        if (eventbtn.Text.Equals("RSVP"))
        {
            var manager = new UserManager();
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
            OleDbConnection c = new OleDbConnection(connectionString);
            c.Open();
            OleDbCommand cmd = c.CreateCommand();
            cmd.CommandType = CommandType.Text;
            // Create query
            //first check if there are slots left
            cmd.CommandText = "SELECT E.capacity FROM event E WHERE E.eventName = '" + eventNamelbl.Text + "'";
            OleDbDataReader reader = cmd.ExecuteReader();
            int cap = 0;
            while (reader.Read())
            {
                cap = reader.GetInt32(0);
            }
            reader.Close();
            if (cap <= 0)
            {
                capacitylbl.Text = Convert.ToString(Math.Abs(cap));
                cap = cap - 1;
                resultslbl.Text = "Unfortunately there are no slots left available for this event. You are number" + Math.Abs(cap) + "in the waitlist";
                //close db connection for now

                cmd.CommandText = "INSERT INTO waitlist VALUES( '" + eventNamelbl.Text + "','" + manager.GetEmail(User.Identity.GetUserId()) + "')";
                reader = cmd.ExecuteReader();
                reader.Close();
                cmd.CommandText = "UPDATE event SET capacity = " + cap + " WHERE eventName = '" + eventNamelbl.Text + "'";
                reader = cmd.ExecuteReader();
                reader.Close();

                c.Close();

                return;
            }
            else
            {
                cap--;
                reader.Close();
                cmd.CommandText = "UPDATE event SET capacity = " + cap + " WHERE eventName = '" + eventNamelbl.Text + "'";
                reader = cmd.ExecuteReader();
                reader.Close();
                cmd.CommandText = "INSERT INTO attendedEvents VALUES ('" + eventNamelbl.Text + "', '" + manager.GetEmail(User.Identity.GetUserId()) + "')";
                reader = cmd.ExecuteReader();
                //send confirmation email to user
                sendEmail(manager.GetEmail(User.Identity.GetUserId()), "You have successfully signed up for the " + eventNamelbl.Text + " event, have fun!", "Btown Events - Event Notification");
                Response.Redirect("~/Pages/Dashboard");
                //close db connection for now
                c.Close();
            }
        }
        else
        {
            LeaveEvent_Click(sender, e);
        }
    }

    protected void LeaveEvent_Click(object sender, EventArgs e)
    {
        var manager = new UserManager();
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        OleDbConnection c = new OleDbConnection(connectionString);
        c.Open();
        OleDbCommand cmd = c.CreateCommand();
        cmd.CommandType = CommandType.Text;
        // Create query
        cmd.CommandText = "SELECT E.capacity FROM event E WHERE E.eventName = '" + eventNamelbl.Text + "'";
        OleDbDataReader reader = cmd.ExecuteReader();
        int cap = 0;
        while (reader.Read())
        {
            cap = reader.GetInt32(0);
        }
        cap++;
        reader.Close();
        cmd.CommandText = "UPDATE event SET capacity = " + cap + " WHERE eventName = '" + eventNamelbl.Text + "'";
        reader = cmd.ExecuteReader();
        reader.Close();
        cmd.CommandText = "DELETE FROM attendedEvents WHERE event = '" + eventNamelbl.Text + "' AND attendee = '" + manager.GetEmail(User.Identity.GetUserId()) + "'";
        reader = cmd.ExecuteReader();
        reader.Close();
        if (cap <= 0)
        {
            cmd.CommandText = "SELECT TOP 1  username FROM waitlist WHERE eventName = '" + eventNamelbl.Text + "'";
            reader = cmd.ExecuteReader();

            String username = "";
            while (reader.Read())
            {
                username = reader.GetString(0);
            }
            reader.Close();
            cmd.CommandText = "INSERT INTO attendedEvents VALUES('" + eventNamelbl.Text + "','" + username + "')";
            reader = cmd.ExecuteReader();
            reader.Close();
            cmd.CommandText = "DELETE FROM waitlist WHERE eventName = '" + eventNamelbl.Text + "' AND userName = '" + username + "'";
            reader = cmd.ExecuteReader();
            reader.Close();
            sendEmail(username, "Congratulations! You are no longer in the waitlist and have been registered for " + eventNamelbl.Text + ".", "Btown Events - Event Notification");



        }
        c.Close();
        //send confirmation email to user
        sendEmail(manager.GetEmail(User.Identity.GetUserId()), "You have successfully notified the event, " + eventNamelbl.Text + " that you will no longer be attending.", "Btown Events - Event Notification");
        Response.Redirect("~/Pages/Dashboard");
    }

    protected void CancelEvent_Click(object sender, EventArgs e)
    {
        var manager = new UserManager();
        //send an email to the venue owner and event creator
        sendEmail(manager.GetEmail(User.Identity.GetUserId()), "You have successfully cancelled the " + eventNamelbl.Text + " event you booked, we have also alerted the venue owner and those who registered for the event.", "Btown Events - Event Cancellation");
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        OleDbConnection c = new OleDbConnection(connectionString);
        c.Open();
        OleDbCommand cmd = c.CreateCommand();
        cmd.CommandType = CommandType.Text;
        // Create query
        cmd.CommandText = "SELECT V.owner FROM venue V WHERE V.venueName = '" + locationlbl.Text + "'";
        OleDbDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            sendEmail(reader.GetString(0), "A user, " + manager.GetEmail(User.Identity.GetUserId()) + ", has cancelled the event, " + eventNamelbl.Text + ", at your venue.", "Btown Events - Event Cancellation");
        }
        reader.Close();
        //query the users who signed up for the event
        cmd.CommandText = "SELECT AE.attendee FROM attendedEvents AE WHERE AE.event = '" + eventNamelbl.Text + "'";
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            sendEmail(reader.GetString(0), "We are emailing you to inform you that the event, " + eventNamelbl.Text + ", you signed up for has been cancelled.", "Btown Events - Event Cancellation");
            OleDbCommand cmd2 = c.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "DELETE FROM attendedEvents WHERE event = '" + eventNamelbl.Text + "' AND attendee = '" + reader.GetString(0) + "'";
            OleDbDataReader reader2 = cmd2.ExecuteReader();
        }
        //remove event from database
        reader.Close();
        cmd.CommandText = "DELETE FROM venueEvents WHERE event = '" + eventNamelbl.Text + "'";
        reader = cmd.ExecuteReader();
        reader.Close();
        cmd.CommandText = "DELETE FROM event WHERE eventName = '" + eventNamelbl.Text + "'";
        reader = cmd.ExecuteReader();
        //close db connection for now
        c.Close();
        Response.Redirect("~/Pages/Dashboard");
    }

    public static void sendEmail(string userEmail, string content, string subject)
    {
        //From Address
        string FromAddress = "email";
        string FromAdressTitle = "Btown Events";
        //To Address
        string ToAddress = userEmail;
        string ToAdressTitle = "Microsoft ASP.NET Core";
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