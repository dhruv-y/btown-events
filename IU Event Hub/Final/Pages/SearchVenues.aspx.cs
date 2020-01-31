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
        searchVenuebtn_Click(sender, e);
    }

    protected void searchVenuebtn_Click(object sender, EventArgs e)
    {
        //the source may change based on where it is locally on your PC, or where it is when it is deployed
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        OleDbConnection c = new OleDbConnection(connectionString);
        c.Open();
        OleDbCommand cmd = c.CreateCommand();
        cmd.CommandType = CommandType.Text;
        // Create query
        string query = "SELECT V.venueName FROM venue V WHERE V.isClosed = 0 ";
        if (days.SelectedValue.Equals("Weekdays"))
        {
            query += "AND V.daysAvailable = 'MTWHF'";
        }
        else if (days.SelectedValue.Equals("Weekends"))
        {
            query += "AND V.daysAvailable = 'SS'";
        }
        if (!ownertxt.Text.Equals(""))
        {
            if (days.SelectedValue.Equals("All"))
            {
                query += "AND V.owner = '"+ownertxt.Text+"'";
            }
            else
            {
                query += " AND V.owner = '"+ownertxt.Text+"'";
            }
        }
        if (!locationtxt.Text.Equals(""))
        {
            if (days.SelectedValue.Equals("All") && ownertxt.Text.Equals(""))
            {
                query += "AND V.location = '"+locationtxt.Text+"'";
            }
            else
            {
                query += " AND V.location = '"+locationtxt.Text+"'";
            }
        }
        if (!capacitytxt.Text.Equals(""))
        {
            if (days.SelectedValue.Equals("All") && ownertxt.Text.Equals("") && locationtxt.Text.Equals(""))
            {
                query += "AND V.capacity > "+(Convert.ToInt32(capacitytxt.Text)-1)+"";
            }
            else
            {
                query += " AND V.capacity > "+(Convert.ToInt32(capacitytxt.Text)-1)+"";
            }
        }
        if (days.SelectedValue.Equals("All") && ownertxt.Text.Equals("") && locationtxt.Text.Equals("") && capacitytxt.Text.Equals(""))
        {
            query = "SELECT V.venueName FROM venue V WHERE V.isClosed = 0";
        }
        cmd.CommandText = query;
        //execute query
        OleDbDataReader reader = cmd.ExecuteReader();
        //show results
        Resultslbl.Text = "";
        resultsFiller.Controls.Clear();
        int i = 0;
        while (reader.Read())
        {
            //Resultslbl.Text += "<a href='Venue.aspx?VENUE="+reader.GetString(0)+"'>"+reader.GetString(0)+"</a>";
            //Resultslbl.Text += "<br />";
            System.Web.UI.HtmlControls.HtmlGenericControl dynDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
            dynDiv.ID = "dynDivCode" + i.ToString();
            dynDiv.Attributes["class"] = "col-lg-4";
            dynDiv.InnerHtml = "<div class = thumbnail><img src = \"../Style/eventhub3.jpg\" /><a href='Venue.aspx?VENUE=" + reader.GetString(0) + "'>" + reader.GetString(0) + "</a></div>";
            resultsFiller.Controls.Add(dynDiv);
            i++;
        }
        //close db connection for now
        c.Close();
    }
}
