<%@ Page Title="Search Events" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="SearchEvents.aspx.cs" Inherits="Pages_Search" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<link rel="stylesheet" href="../Style/search.css" type="text/css" media="screen" />
<link href="https://fonts.googleapis.com/css?family=Montserrat" rel="stylesheet">

<div class="container_main">
    <asp:Panel ID="eventSearchpnl" runat="server" Visible="true">
        <h2>Search Events</h2>
        <div class="search-event">
            <asp:Label ID="costlbl" runat="server" Text="Cost:" CssClass="filter-text"></asp:Label>
            <asp:DropDownList ID="cost" runat="server">
                <asp:ListItem Selected="True">Both</asp:ListItem>
                <asp:ListItem>Free</asp:ListItem>
                <asp:ListItem>Paid</asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="categorylbl" runat="server" Text="Category:" CssClass="filter-text"></asp:Label>
            <asp:DropDownList ID="category" runat="server">
                <asp:ListItem Selected="True">All</asp:ListItem>
                <asp:ListItem>Music</asp:ListItem>
                <asp:ListItem>Party</asp:ListItem>
                <asp:ListItem>Film</asp:ListItem>
                <asp:ListItem>Fine Arts</asp:ListItem>
                <asp:ListItem>Sports</asp:ListItem>
                <asp:ListItem>Academic</asp:ListItem>
                <asp:ListItem>Other</asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="venuelbl" runat="server" Text="Location:" CssClass="filter-text"></asp:Label>
            <asp:TextBox ID="venue" runat="server"></asp:TextBox>
            <asp:Label ID="searchlbl" runat="server" Text="Name:" CssClass="filter-text"></asp:Label>
            <asp:TextBox ID="search" runat="server"></asp:TextBox>
            <asp:Button ID="searchEventbtn" runat="server" Text="Search" OnClick="searchEventbtn_Click" />
        </div>
    </asp:Panel>
    <div class="form-group">
        <!--Results from search-->
        <h3>Search Results</h3>
        <asp:Panel ID="resultsFiller" runat="server">

        </asp:Panel>
    </div>
</div>
</asp:Content>