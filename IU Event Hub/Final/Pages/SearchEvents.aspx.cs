using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//important data include
using System.Data.OleDb;
using System.Data;

public partial class Pages_Search : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        searchEventbtn_Click(sender, e);
    }

    protected void searchEventbtn_Click(object sender, EventArgs e)
    {
        //Search and display events based on search criteria

        //the source may change based on where it is locally on your PC, or where it is when it is deployed
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        OleDbConnection c = new OleDbConnection(connectionString);
        c.Open();
        OleDbCommand cmd = c.CreateCommand();
        cmd.CommandType = CommandType.Text;
        // Create query
        string query = "SELECT E.eventName, E.eventDate, E.category FROM event E WHERE ";
        if (cost.SelectedValue.Equals("Free"))
        {
            query += "E.cost = 0";
        } else if (cost.SelectedValue.Equals("Paid"))
        {
            query += "E.cost <> 0";
        }
        if (!category.SelectedValue.Equals("All"))
        {
            if (cost.SelectedValue.Equals("Both"))
            {
                query += "E.category = '"+category.SelectedValue+"'";
            } else
            {
                query += " AND E.category = '"+category.SelectedValue+"'";
            }
        }
        if (!venue.Text.Equals(""))
        {
            if (cost.SelectedValue.Equals("Both") && category.SelectedValue.Equals("All"))
            {
                query += "E.location = '"+venue.Text+"'";
            } else
            {
                query += " AND E.location = '" + venue.Text + "'";
            }
        }
        if (!search.Text.Equals(""))
        {
            if (cost.SelectedValue.Equals("Both") && category.SelectedValue.Equals("All") && venue.Text.Equals(""))
            {
                query += "E.eventName = '"+search.Text+"'";
            }
            else
            {
                query += " AND E.eventName = '"+search.Text+"'";
            }
        }
        if (cost.SelectedValue.Equals("Both") && category.SelectedValue.Equals("All") && venue.Text.Equals("") && search.Text.Equals(""))
        {
            query = "SELECT E.eventName, E.eventDate, E.category FROM event E";
        }
        cmd.CommandText = query;
        //execute query
        OleDbDataReader reader = cmd.ExecuteReader();
        //show results
        resultsFiller.Controls.Clear();
        int i = 0;
        while (reader.Read())
        {
            if (inFuture(reader.GetString(1)))
            {
                //Resultslbl.Text += "<a href='Event.aspx?EVENT=" + reader.GetString(0) + "'>" + reader.GetString(0) + "</a> " + reader.GetString(1);
                //Resultslbl.Text += "<br />";
                System.Web.UI.HtmlControls.HtmlGenericControl dynDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
                dynDiv.ID = "dynDivCode" + i.ToString();
                dynDiv.Attributes["class"] = "col-lg-4";
                switch (reader.GetString(2))
                {
                    case "Music":
                        dynDiv.InnerHtml = "<div class = thumbnail><img src = \"../Style/test3.jpg\" /><h5>" + reader.GetString(1) + "</h5><a href='Event.aspx?EVENT=" + reader.GetString(0) + "'>" + reader.GetString(0) + "</a></div>";
                        break;
                    case "Academic":
                        dynDiv.InnerHtml = "<div class = thumbnail><img src = \"../Style/test1.jpg\" /><h5>" + reader.GetString(1) + "</h5><a href='Event.aspx?EVENT=" + reader.GetString(0) + "'>" + reader.GetString(0) + "</a></div>";
                        break;
                    case "Party":
                        dynDiv.InnerHtml = "<div class = thumbnail><img src = \"../Style/eventhub3.jpg\" /><h5>" + reader.GetString(1) + "</h5><a href='Event.aspx?EVENT=" + reader.GetString(0) + "'>" + reader.GetString(0) + "</a></div>";
                        break;
                    case "Film":
                        dynDiv.InnerHtml = "<div class = thumbnail><img src = \"../Style/test2.jpg\" /><h5>" + reader.GetString(1) + "</h5><a href='Event.aspx?EVENT=" + reader.GetString(0) + "'>" + reader.GetString(0) + "</a></div>";
                        break;
                    case "Fine Arts":
                        dynDiv.InnerHtml = "<div class = thumbnail><img src = \"../Style/test2.jpg\" /><h5>" + reader.GetString(1) + "</h5><a href='Event.aspx?EVENT=" + reader.GetString(0) + "'>" + reader.GetString(0) + "</a></div>";
                        break;
                    case "Sports":
                        dynDiv.InnerHtml = "<div class = thumbnail><img src = \"../Style/eventhub5.jpg\" /><h5>" + reader.GetString(1) + "</h5><a href='Event.aspx?EVENT=" + reader.GetString(0) + "'>" + reader.GetString(0) + "</a></div>";
                        break;
                    case "Other":
                        dynDiv.InnerHtml = "<div class = thumbnail><img src = \"../Style/eventhub4.jpg\" /><h5>" + reader.GetString(1) + "</h5><a href='Event.aspx?EVENT=" + reader.GetString(0) + "'>" + reader.GetString(0) + "</a></div>";
                        break;
                }
                resultsFiller.Controls.Add(dynDiv);
                i++;
            }
        }
        //close db connection for now
        c.Close();
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