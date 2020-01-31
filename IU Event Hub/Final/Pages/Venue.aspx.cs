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

public partial class Pages_Venue : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UserManager manager = new UserManager();
        if (manager.GetPhoneNumber(User.Identity.GetUserId()) == null)
        {
            bookpnl.Visible = true;
        }
        venueNamelbl.Text = Request.QueryString["VENUE"];
        //load venue details
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        OleDbConnection c = new OleDbConnection(connectionString);
        c.Open();
        OleDbCommand cmd = c.CreateCommand();
        cmd.CommandType = CommandType.Text;
        // Create query
        cmd.CommandText = "SELECT V.owner, V.location, V.facilities, V.daysAvailable, V.timeAvailable, V.capacity FROM venue V WHERE V.venueName = '"+Request.QueryString["VENUE"]+"'";
        //execute query
        OleDbDataReader reader = cmd.ExecuteReader();
        //show results
        while (reader.Read())
        {
            ownerlbl.Text = reader.GetString(0);
            locationlbl.Text = reader.GetString(1);
            facilitieslbl.Text = reader.GetString(2);
            daysAvailablelbl.Text = reader.GetString(3);
            timeAvailablelbl.Text = reader.GetString(4);
            capacitylbl.Text = Convert.ToString(reader.GetInt32(5));
        }
        reader.Close();
        //find events already booked to show unavailable times
        cmd.CommandText = "SELECT E.eventName, E.eventDate, E.schedule FROM event E WHERE E.location = '"+Request.QueryString["VENUE"]+"'";
        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            if (inFuture(reader.GetString(1)))
            {
                statuslbl.Text += "<a href='Event.aspx?EVENT=" + reader.GetString(0) + "'>" + reader.GetString(0) + "</a> Date: " + reader.GetString(1) + " Time: " + reader.GetString(2);
                statuslbl.Text += "<br />";
            }
        }
        //close db connection for now
        c.Close();
    }

    protected void CreateEvent_Click(object sender, EventArgs e)
    {
        string eventTime = scheduletxt.Text;
        string venueTime = timeAvailablelbl.Text;
        int eventTimeHour = Convert.ToInt32(eventTime.Substring(0,2));
        int eventTimeHour2 = Convert.ToInt32(eventTime.Substring(6,2));
        //check if event time range is valid
        if (eventTimeHour2 < eventTimeHour)
        {
            resultslbl.Text = "Invalid Event Time Range";
            return;
        }
        eventTimeHour2 = eventTimeHour2 * 100;
        eventTimeHour2 += Convert.ToInt32(eventTime.Substring(9, 2));
        //check if given time is within the venue's time available range
        int venueTimeHour = Convert.ToInt32(venueTime.Substring(0,2));
        int venueTimeHour2 = Convert.ToInt32(venueTime.Substring(6,2));
        venueTimeHour2 = venueTimeHour2 * 100;
        venueTimeHour2 += Convert.ToInt32(venueTime.Substring(9, 2));
        if (eventTimeHour < venueTimeHour || eventTimeHour2 > venueTimeHour2)
        {
            resultslbl.Text = "Event time is not within the Venue's available time range";
            return;
        }
        //check if there are any other events on the given day that conflict with this event
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        OleDbConnection c = new OleDbConnection(connectionString);
        c.Open();
        OleDbCommand cmd = c.CreateCommand();
        cmd.CommandType = CommandType.Text;
        // Create query
        cmd.CommandText = "SELECT E.eventDate, E.schedule FROM event E WHERE E.location = '"+locationlbl.Text+"'";
        //execute query
        OleDbDataReader reader = cmd.ExecuteReader();
        //show results
        while (reader.Read())
        {
            if (reader.GetString(0).Equals(eventDatetxt.Text))
            {
                //this event is on the same day, check for conflicts
                if (checkConflicts(scheduletxt.Text, reader.GetString(1)))
                {
                    //there is a conflict
                    resultslbl.Text = "Time conflict with another existing event at this venue";
                    return;
                }
            }
        }
        //this event is completely valid, add it to the database
        reader.Close();
        cmd.CommandText = "INSERT INTO event VALUES ('"+eventNametxt.Text+"', '"+ User.Identity.GetUserName() + "', '"+eventDatetxt.Text+"', '"+venueNamelbl.Text+"', '"+category.SelectedValue+"', "+Convert.ToInt32(capacitylbl.Text)+", "+Convert.ToInt32(costtxt.Text)+", '"+scheduletxt.Text+"', '"+descriptiontxt.Text+"')";
        reader = cmd.ExecuteReader();
        reader.Close();
        cmd.CommandText = "INSERT INTO venueEvents VALUES ('"+venueNamelbl.Text+"', '"+eventNametxt.Text+"')";
        reader = cmd.ExecuteReader();
        reader.Close();
        //close db connection for now
        c.Close();
        var manager = new UserManager();
        //email both the event owner and venue owner
        sendEmail(ownerlbl.Text, "A user, " + manager.GetEmail(User.Identity.GetUserId()) + ", has booked an event, " + eventNametxt.Text +", at your venue!");
        sendEmail(manager.GetEmail(User.Identity.GetUserId()), "You have successfully booked an event at "+venueNamelbl.Text+"! An email has also been sent to the venue owner.");
        resultslbl.Text = "Event booked successfully!";
        Response.Redirect("~/Pages/Dashboard");
    }

    protected Boolean checkConflicts(string event1, string event2)
    {
        int event1hour = Convert.ToInt32(event1.Substring(0, 2));
        event1hour = event1hour * 100;
        event1hour += Convert.ToInt32(event1.Substring(3, 2));
        int event1hour2 = Convert.ToInt32(event1.Substring(6, 2));
        event1hour2 = event1hour2 * 100;
        event1hour2 += Convert.ToInt32(event1.Substring(9, 2));

        int event2hour = Convert.ToInt32(event2.Substring(0, 2));
        event2hour = event2hour * 100;
        event2hour += Convert.ToInt32(event2.Substring(3, 2));
        int event2hour2 = Convert.ToInt32(event2.Substring(6, 2));
        event2hour2 = event2hour2 * 100;
        event2hour2 += Convert.ToInt32(event2.Substring(9, 2));
        if (event2hour <= event1hour && event2hour2 > event1hour)
        {
            return true;
        }
        if (event2hour <= event1hour2 && event2hour2 > event1hour2)
        {
            return false;
        }
        return false;
    }

    public static void sendEmail(string userEmail, string content)
    {
        //From Address
        string FromAddress = "email";
        string FromAdressTitle = "Btown Events";
        //To Address
        string ToAddress = userEmail;
        string ToAdressTitle = "Microsoft ASP.NET Core";
        string Subject = "IUEventHub - Event Booking";
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

    protected Boolean inFuture(String date)
    {
        //String today = Convert.ToString(DateTime.Now);
        //11/8/2019 1:44:48 PM
        String[] eventDate = date.Split('/');
        String[] today = Convert.ToString(DateTime.Now).Split(' ');
        today = today[0].Split('/');
        if (Convert.ToInt32(eventDate[2]) > Convert.ToInt32(today[2]))
        {
            return true;
        }
        else if (Convert.ToInt32(eventDate[1]) > Convert.ToInt32(today[1]))
        {
            return true;
        }
        else if (Convert.ToInt32(eventDate[0]) > Convert.ToInt32(today[0]))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
