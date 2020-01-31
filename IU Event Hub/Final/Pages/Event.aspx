<%@ Page Title = "Event Signup" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Event.aspx.cs" Inherits="Pages_Event" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script type ="text/javascript" src ="../Scripts/WebForms/Event.js"></script>
    <link rel="stylesheet" href="../Style/dashboard.css" type="text/css" media="screen" />
    <link href="https://fonts.googleapis.com/css?family=Montserrat" rel="stylesheet">

<div class="row content" style="transform:translateY(20%);">
    <div class="details col-md-6" style="color: white !important;">
        <h2>Event Details</h2>
        <asp:Label ID="eventNamelbl2" runat="server" Text="Venue Name: " CssClass="heading"></asp:Label><asp:Label ID="eventNamelbl" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="ownerlbl2" runat="server" Text="Owner: " CssClass="heading"></asp:Label><asp:Label ID="ownerlbl" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="eventDatelbl2" runat="server" Text="Event Date: " CssClass="heading"></asp:Label><asp:Label ID="eventDatelbl" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="locationlbl2" runat="server" Text="Location: " CssClass="heading"></asp:Label><asp:Label ID="locationlbl" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="addresslbl2" runat="server" Text="Address: " CssClass="heading"></asp:Label><asp:Label ID="addresslbl" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="categorylbl2" runat="server" Text="Spots left: " CssClass="heading"></asp:Label><asp:Label ID="categorylbl" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="costlbl2" runat="server" Text="Cost: " CssClass="heading"></asp:Label><asp:Label ID="costlbl" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="timelbl2" runat="server" Text="Time: " CssClass="heading"></asp:Label><asp:Label ID="timelbl" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="capacitylbl2" runat="server" Text="Capacity: " CssClass="heading"></asp:Label><asp:Label ID="capacitylbl" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="descriptionlbl2" runat="server" Text="Description: " CssClass="heading"></asp:Label><asp:Label ID="descriptionlbl" runat="server" Text=""></asp:Label>
        <asp:Panel ID="RSVPpnl" runat="server" Visible ="false">
        <div class ="form-group">
            <asp:Label ID="resultslbl" runat="server" Text=""></asp:Label>
            <asp:Button runat="server" ID = "eventbtn" OnClick="JoinEvent_Click" OnClientClick="window.open('Payment.html')" Text="RSVP" CssClass="btn btn-primary" style="text-transform: uppercase; font-weight: 600; letter-spacing: 1px; margin-top: 10px;" />
        </div>
    </asp:Panel>
    <asp:Panel ID="Cancelpnl" runat="server" Visible ="false">
        <div class ="form-group">
            <asp:Button runat="server" OnClick="CancelEvent_Click" Text="Cancel Event" CssClass="btn btn-primary" style="text-transform: uppercase; font-weight: 600; letter-spacing: 1px; margin-top: 10px;" />
        </div>
    </asp:Panel>
    </div>
    <div class="col-md-6">
        <script src='https://api.mapbox.com/mapbox-gl-js/v1.4.1/mapbox-gl.js'></script>
        <link href='https://api.mapbox.com/mapbox-gl-js/v1.4.1/mapbox-gl.css' rel='stylesheet' />
        <div id='map' style='width: 100%; height: 400px; margin-top: 20px;'></div>
    </div>
    </div>
    
</asp:Content>
