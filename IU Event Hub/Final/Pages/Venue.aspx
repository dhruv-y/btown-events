<%@ Page Title = "Book Venue" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Venue.aspx.cs" Inherits="Pages_Venue" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="../Style/venue_details.css" type="text/css" media="screen" />
    <link href="https://fonts.googleapis.com/css?family=Montserrat" rel="stylesheet">
    <script type ="text/javascript" src ="../Scripts/WebForms/Venue.js"></script>
    <div class = "row">
        <div class="col-md-6" style="color: white !important;">
            <h2>Venue Details</h2>
            <asp:Label ID="venueNamelbl2" runat="server" Text="Venue Name: "></asp:Label><asp:Label ID="venueNamelbl" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="ownerlbl2" runat="server" Text="Owner: "></asp:Label><asp:Label ID="ownerlbl" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="locationlbl2" runat="server" Text="Address: "></asp:Label><asp:Label ID="locationlbl" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="facilitieslbl2" runat="server" Text="Facilities: "></asp:Label><asp:Label ID="facilitieslbl" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="daysAvailablelbl2" runat="server" Text="Days Available: "></asp:Label><asp:Label ID="daysAvailablelbl" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="timeAvailablelbl2" runat="server" Text="Time Available: "></asp:Label><asp:Label ID="timeAvailablelbl" runat="server" Text=""></asp:Label>
            <br />
            <asp:Label ID="capacitylbl2" runat="server" Text="Capacity: "></asp:Label><asp:Label ID="capacitylbl" runat="server" Text=""></asp:Label>

            <h2>Events Booked</h2>
            <asp:Label ID="statuslbl" runat="server"></asp:Label>
    
        <asp:Panel ID="bookpnl" runat="server" Visible="false">
        
                <h2>Book Venue</h2>
                <asp:Label ID="resultslbl" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="eventNametxt" runat="server" placeholder = "Event Name" style="margin-bottom: 2%;"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="eventNametxt" ErrorMessage="Event Name field is required"></asp:RequiredFieldValidator>
                <br />
                <asp:TextBox ID="eventDatetxt" runat="server" placeholder = "Event Date" style="margin-bottom: 2%;"></asp:TextBox>
                <br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="eventDatetxt" ErrorMessage="Event Date field is required "></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="eventDatetxt" ErrorMessage="Event Date field must be in the form xx/xx/xxxx" ValidationExpression="^\d{1,2}\/\d{1,2}\/\d{4}$"></asp:RegularExpressionValidator>
                <asp:TextBox ID="scheduletxt" runat="server" placeholder = "Event Time" style="margin-bottom: 2%;"></asp:TextBox>
                <br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="scheduletxt" ErrorMessage="Event Time field is required "></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="scheduletxt" ErrorMessage="Event Time field must be in the form xx:xx-xx:xx" ValidationExpression="^([0-1]?[0-9]|[2][0-3]):([0-5][0-9])-([0-1]?[0-9]|[2][0-3]):([0-5][0-9])$"></asp:RegularExpressionValidator>
                <asp:Label ID="categorylbl" runat="server" Text="Category:"></asp:Label>
                <asp:DropDownList ID="category" runat="server">
                    <asp:ListItem>Music</asp:ListItem>
                    <asp:ListItem>Party</asp:ListItem>
                    <asp:ListItem>Film</asp:ListItem>
                    <asp:ListItem>Fine Arts</asp:ListItem>
                    <asp:ListItem>Sports</asp:ListItem>
                    <asp:ListItem>Academic</asp:ListItem>
                    <asp:ListItem Selected ="True">Other</asp:ListItem>
                </asp:DropDownList>
                <br />
                <asp:TextBox ID="costtxt" runat="server" placeholder = "Cost Per Person" style="margin-bottom: 2%; margin-top: 2%;"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="costtxt" ErrorMessage="Cost field is required"></asp:RequiredFieldValidator>
                <br />
                <asp:TextBox ID="descriptiontxt" runat="server" placeholder = "Event Description" style="margin-bottom: 2%;"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="descriptiontxt" ErrorMessage="Event Description field is required"></asp:RequiredFieldValidator>
                <br />
                <asp:Button runat="server" OnClick="CreateEvent_Click" Text="Book Venue" CssClass="btn btn-primary" style ="left: 0" />
               </asp:Panel>
           </div>
           <div class="col-md-6">
                <script src='https://api.mapbox.com/mapbox-gl-js/v1.4.1/mapbox-gl.js'></script>
                <link href='https://api.mapbox.com/mapbox-gl-js/v1.4.1/mapbox-gl.css' rel='stylesheet' />
                <div id='map' style='width: 100%; height: 400px; margin-top: 20px;'></div>
           </div>
    </div>

</asp:Content>