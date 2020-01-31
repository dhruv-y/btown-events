<%@ Page Title="Search Users" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="SearchUsers.aspx.cs" Inherits="Pages_Search" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<link rel="stylesheet" href="../Style/search.css" type="text/css" media="screen" />
<link href="https://fonts.googleapis.com/css?family=Montserrat" rel="stylesheet">

<div class="container_main">
    <h2>Search Users</h2>
    <div class="form-group">
        <asp:Label ID="userTypelbl" runat="server" Text="User type:" CssClass="filter-text"></asp:Label>
        <asp:DropDownList ID="userType" runat="server">
            <asp:ListItem Selected="True">Both</asp:ListItem>
            <asp:ListItem>User</asp:ListItem>
            <asp:ListItem>Venue Owner</asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="namelbl" runat="server" Text="Name:" CssClass="filter-text"></asp:Label>
        <asp:TextBox ID="nametxt" runat="server"></asp:TextBox>
        <asp:Button ID="searchUserbtn" runat="server" Text="Search" OnClick="searchUserbtn_Click" />
    </div>
    <div class="form-group">
        <!--Results from search-->
        <h3>Search Results</h3>
        <asp:Label ID="Resultslbl" runat="server" Font-Bold="True" style="color: white;"></asp:Label>
    </div>
</div>
</asp:Content>