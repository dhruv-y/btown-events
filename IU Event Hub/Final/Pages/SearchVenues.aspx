<%@ Page Title="Search Venues" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="SearchVenues.aspx.cs" Inherits="Pages_Search" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<link rel="stylesheet" href="../Style/search.css" type="text/css" media="screen" />
<link href="https://fonts.googleapis.com/css?family=Montserrat" rel="stylesheet">

<div class="container_main">
    <h2>Search Venues</h2>
    <div class="form-group">
        <asp:Label ID="ownerlbl" runat="server" Text="Owner:" CssClass="filter-text"></asp:Label>
        <asp:TextBox ID="ownertxt" runat="server"></asp:TextBox>
        <asp:Label ID="locationlbl" runat="server" Text="Location:" CssClass="filter-text"></asp:Label>
        <asp:TextBox ID="locationtxt" runat="server"></asp:TextBox>
        <asp:Label ID="capacitylbl" runat="server" Text="Capacity:" CssClass="filter-text"></asp:Label>
        <asp:TextBox ID="capacitytxt" runat="server" onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);"></asp:TextBox>
        <asp:Label ID="timelbl" runat="server" Text="Days Available:" CssClass="filter-text"></asp:Label>
        <asp:DropDownList ID="days" runat="server">
            <asp:ListItem>All</asp:ListItem>
            <asp:ListItem>Weekdays</asp:ListItem>
            <asp:ListItem>Weekends</asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="searchVenuebtn" runat="server" Text="Search" OnClick="searchVenuebtn_Click" />
    </div>
    <div class="form-group">
        <!--Results from search-->
        <h3>Search Results</h3>
        <asp:Label ID="Resultslbl" runat="server" Font-Bold="True"></asp:Label>
        <asp:Panel ID="resultsFiller" runat="server">

        </asp:Panel>
    </div>
</div>
</asp:Content>