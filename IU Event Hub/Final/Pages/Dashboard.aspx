<%@ Page Title="Dashboard" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Dashboard.aspx.cs" Inherits="Pages_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<link rel="stylesheet" href="../Style/dashboard.css" type="text/css" media="screen" />
<link href="https://fonts.googleapis.com/css?family=Montserrat" rel="stylesheet">
    
    <div class="row">
       <script type ="text/javascript" src ="../Scripts/WebForms/Dashboard.js"></script> 
    </div>
    <asp:Panel ID="userView" runat="server" Visible="false">
       
    <div class="row">
        <div class="col-md-12">
            <h2>Upcoming Events</h2>
            <div class="col-lg-4" id="cardBookedEvent1">
                <div class="thumbnail">
                    <img src="../Style/test1.jpg" />
                    <h5><asp:Label ID="bookedEvent1" runat="server" Text="No events"></asp:Label></h5>
                    <h4><asp:Label ID="bookedEventDate1" runat="server" Text="No events"></asp:Label></h4>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="thumbnail" id="cardBookedEvent2">
                    <img src="../Style/test2.jpg" />
                    <h5><asp:Label ID="bookedEvent2" runat="server" Text="No events"></asp:Label></h5>
                    <h4><asp:Label ID="bookedEventDate2" runat="server" Text="No events"></asp:Label></h4>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="thumbnail" id="cardBookedEvent3">
                    <img src="../Style/test3.jpg" />
                    <h5><asp:Label ID="bookedEvent3" runat="server" Text="No events"></asp:Label></h5>
                    <h4><asp:Label ID="bookedEventDate3" runat="server" Text="No events"></asp:Label></h4>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <h2>Attended Events</h2>
            <div class="col-lg-4" id = "cardAttendedevent1">
                <div class="thumbnail">
                    <img src="../Style/test3.jpg" />
                    <h5>
                        <asp:Label ID="attendedEventDate1" runat="server" Text=""></asp:Label></h5>
                    <h4>
                        <asp:Label ID="attendedEvent1" runat="server" Text="No events"></asp:Label>

                    </h4>
                </div>
            </div>
            <div class="col-lg-4" id = "cardAttendedevent2">
                <div class="thumbnail">
                    <img src="../Style/test3.jpg" />
                    <h5>
                        <asp:Label ID="attendedEventDate2" runat="server" Text=""></asp:Label></h5>
                    <h4>
                        <asp:Label ID="attendedEvent2" runat="server" Text="No events"></asp:Label>

                    </h4>
                </div>
            </div>
            <div class="col-lg-4" id = "cardAttendedevent3">
                <div class="thumbnail">
                    <img src="../Style/test3.jpg" />
                    <h5>
                        <asp:Label ID="attendedEventDate3" runat="server" Text=""></asp:Label></h5>
                    <h4>
                        <asp:Label ID="attendedEvent3" runat="server" Text="No events"></asp:Label>

                    </h4>
                </div>
                <br />
                <asp:Button ID="seeMore1" runat="server" Text="See more" OnClick="seeMore1_Click" CssClass="btn btn-primary" style="margin-top:10%; margin-left:5%" />
            </div>
            
        </div>
        <br />
        <asp:Button ID="Button1" runat="server" Text="Chat" OnClientClick="window.open('Chat.html')" CssClass="btn btn-primary" style="margin-left:70%; margin-top:2%" />
    </div>
    </asp:Panel>
    <asp:Panel ID="ownerView" runat="server" Visible="false">
        <div class="row">
            <div class="col-md-12">
                <h2>Events Booked at your Venue</h2>
                <asp:Panel ID="ownerFiller" runat="server">

                </asp:Panel>
            </div>
        </div>
    </asp:Panel>

</asp:Content>