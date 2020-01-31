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
        searchUserbtn_Click(sender, e);
    }

    protected void searchUserbtn_Click(object sender, EventArgs e)
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DBConnect"].ConnectionString;
        OleDbConnection c = new OleDbConnection(connectionString);
        c.Open();
        OleDbCommand cmd = c.CreateCommand();
        cmd.CommandType = CommandType.Text;
        // Create query
        string query = "SELECT U.userName FROM users U WHERE ";
        if (userType.SelectedValue.Equals("User"))
        {
            query += "U.isOwner = 0";
        }
        else if (userType.SelectedValue.Equals("Venue Owner"))
        {
            query += "U.isOwner = -1";
        }
        if (!nametxt.Text.Equals(""))
        {
            if (userType.SelectedValue.Equals("Both"))
            {
                query += "U.userName = '"+nametxt.Text+"'";
            }
            else
            {
                query += " AND U.userName = '"+nametxt.Text+"'";
            }
        }
        if (userType.SelectedValue.Equals("Both") && nametxt.Text.Equals(""))
        {
            query = "SELECT U.userName FROM users U";
        }
        cmd.CommandText = query;
        //Resultslbl.Text = query; //debugging
        //execute query
        OleDbDataReader reader = cmd.ExecuteReader();
        //show results
        Resultslbl.Text = "";
        while (reader.Read())
        {
            Resultslbl.Text += reader.GetString(0);
            Resultslbl.Text += "<br />";
        }
        //close db connection for now
        c.Close();
    }
}