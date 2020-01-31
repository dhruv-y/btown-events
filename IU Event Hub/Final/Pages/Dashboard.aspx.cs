using Final;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//important data include
using System.Data.OleDb;
using System.Data;
using Microsoft.AspNet.Identity.Owin;

public partial class Pages_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UserManager manager = new UserManager();
        //check if the user is a venue owner or a user, and make visible the correct panel
        if (manager.GetPhoneNumber(User.Identity.GetUserId()) != null)
        {
            ownerView.Visible = true;
            //query database for events booked at owner's venue
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
            OleDbConnection c = new OleDbConnection(connectionString);
            c.Open();
            OleDbCommand cmd = c.CreateCommand();
            cmd.CommandType = CommandType.Text;
            //create query
            cmd.CommandText = "SELECT V.venueName FROM venue V WHERE V.owner = '"+User.Identity.GetUserName()+"' AND V.isClosed = 0";
            //execute query
            OleDbDataReader reader = cmd.ExecuteReader();
            //check if user owns a venue
            String venueName = "";
            while (reader.Read())
            {
                venueName = reader.GetString(0);
            }
            //c.Close();
            reader.Close();
            if (!venueName.Equals(""))
            {
                //check if there are events booked at the venue
                cmd.CommandText = "SELECT E.eventName, E.eventDate, E.category FROM event E WHERE E.location = '"+venueName+"'";
                reader = cmd.ExecuteReader();
                int i = 0;
                while (reader.Read())
                {
                    //only show events that are upcoming
                    if (inFuture(reader.GetString(1))) {
                        System.Web.UI.HtmlControls.HtmlGenericControl dynDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
                        dynDiv.ID = "dynDivCode" + i.ToString();
                        dynDiv.Attributes["class"] = "col-lg-4";
                        switch (reader.GetString(2))
                        {
                            case "Music":
                                dynDiv.InnerHtml = "<div class = thumbnail><img src = \"../Style/test3.jpg\" /><h5>" + reader.GetString(1) + "</h5><h4>" + reader.GetString(0) + "</h4></div>";
                                break;
                            case "Academic":
                                dynDiv.InnerHtml = "<div class = thumbnail><img src = \"../Style/test1.jpg\" /><h5>" + reader.GetString(1) + "</h5><h4>" + reader.GetString(0) + "</h4></div>";
                                break;
                            case "Party":
                                dynDiv.InnerHtml = "<div class = thumbnail><img src = \"../Style/eventhub3.jpg\" /><h5>" + reader.GetString(1) + "</h5><h4>" + reader.GetString(0) + "</h4></div>";
                                break;
                            case "Film":
                                dynDiv.InnerHtml = "<div class = thumbnail><img src = \"../Style/test2.jpg\" /><h5>" + reader.GetString(1) + "</h5><h4>" + reader.GetString(0) + "</h4></div>";
                                break;
                            case "Fine Arts":
                                dynDiv.InnerHtml = "<div class = thumbnail><img src = \"../Style/test2.jpg\" /><h5>" + reader.GetString(1) + "</h5><h4>" + reader.GetString(0) + "</h4></div>";
                                break;
                            case "Sports":
                                dynDiv.InnerHtml = "<div class = thumbnail><img src = \"../Style/eventhub5.jpg\" /><h5>" + reader.GetString(1) + "</h5><h4>" + reader.GetString(0) + "</h4></div>";
                                break;
                            case "Other":
                                dynDiv.InnerHtml = "<div class = thumbnail><img src = \"../Style/eventhub4.jpg\" /><h5>" + reader.GetString(1) + "</h5><h4>" + reader.GetString(0) + "</h4></div>";
                                break;
                        }
                        ownerFiller.Controls.Add(dynDiv);
                        i++;
                    }
                }
            }
            reader.Close();
        } else
        {
            userView.Visible = true;
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
            OleDbConnection c = new OleDbConnection(connectionString);
            c.Open();
            OleDbCommand cmd = c.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select TOP 3 E.eventName, E.eventDate from event E INNER JOIN attendedEvents AE on E.eventName = AE.event where E.eventDate < '" + DateTime.Now.ToString("MM/dd/yyyy") + "' and AE.attendee = '" + User.Identity.GetUserName() + "'";
            OleDbDataReader reader = cmd.ExecuteReader();
            int i = 0;
            // reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (i == 0)
                { 
                    attendedEvent1.Text = reader.GetString(0);
                    attendedEventDate1.Text = reader.GetString(1);

                }
                if (i == 1)
                {
                    attendedEvent2.Text = reader.GetString(0);
                    attendedEventDate2.Text = reader.GetString(1);
                }
                if (i == 2)
                {
                    attendedEvent3.Text = reader.GetString(0);
                    attendedEventDate3.Text = reader.GetString(1);
                }
                i++;
            }

            reader.Close();
            c.Close();

            cmd.CommandText = "select TOP 3 E.eventName, E.eventDate from event E INNER JOIN attendedEvents AE on E.eventName = AE.event where E.eventDate >= '" + DateTime.Now.ToString("MM/dd/yyyy") + "' and AE.attendee = '" + User.Identity.GetUserName() + "'";
            c.Open();
           
            reader = cmd.ExecuteReader();
            i = 0;
            // reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (i == 0)
                {
                    bookedEvent1.Text = reader.GetString(0);
                    bookedEventDate1.Text = reader.GetString(1);

                }
                if (i == 1)
                {
                    bookedEvent2.Text = reader.GetString(0);
                    bookedEventDate2.Text = reader.GetString(1);
                }
                if (i == 2)
                {
                    bookedEvent3.Text = reader.GetString(0);
                    bookedEventDate3.Text = reader.GetString(1);
                }
                i++;
            }
            



            reader.Close();
            c.Close();
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
        } else if (Convert.ToInt32(eventDate[1]) > Convert.ToInt32(today[1]))
        {
            return true;
        } else if (Convert.ToInt32(eventDate[0]) > Convert.ToInt32(today[0]))
        {
            return true;
        } else
        {
            return false;
        }
    }

    protected void seeMore1_Click(object sender, EventArgs e)
    {
        
    }
}