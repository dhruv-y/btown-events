<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Account_Register" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <link rel="stylesheet" href="../Style/register.css" type="text/css" media="screen" />
    <link href="https://fonts.googleapis.com/css?family=Montserrat" rel="stylesheet">
    
    <div class="container">
        <div class="header">
            <h1><%: Title %></h1>
        </div>
            
            <p class="text-danger">
                <asp:Literal runat="server" ID="ErrorMessage" />
            </p>

            <script src="https://www.google.com/recaptcha/api.js" async defer></script>

            <div class="inner-wrap">
                <div class="form-horizontal form">
                
                    <div class="form-group">
                    
                        <div class="col-md-12">
                            <asp:TextBox runat="server" ID="UserName" CssClass="form-control" placeholder="enter your email" style="color:black"/>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                CssClass="text-danger" ErrorMessage="Username Required" />
                        </div>
                    </div>
                    <div class="form-group">
                    
                        <div class="col-md-12">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" placeholder="password" style="color:black" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                CssClass="text-danger" ErrorMessage="Password Required" />
                        </div>
                    </div>
                    <div class="form-group">
                    
                        <div class="col-md-12">
                            <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" placeholder="confirm password" style="color:black" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="The confirm password field is required." />
                            <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="The password and confirmation password do not match." />
                            <br />
                            <asp:CheckBox ID="isVenueOwner" runat="server" /> <asp:Label ID="venueOwnerlbl" runat="server" Text="Are You A Venue Owner?" style="color: white; font-size: 16px; text-decoration: none; font-family: 'Montserrat', sans-serif !important;"></asp:Label>
                        </div>
                    </div>
                    <!--
                    <div class ="form-group">
                    
                        <div class="col-md-12">
                        <asp:TextBox runat="server" ID="question" CssClass="form-control" placeholder="security question" style="color:black" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="question"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="Security Question Required" />
                        </div>
                    </div>
                    <div class ="form-group">
                    
                        <div class="col-md-12">
                        <asp:TextBox runat="server" ID="answer" CssClass="form-control" placeholder="security answer" style="color:black"/>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="answer"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="Security Answer Required" />
                        </div>
                    </div>
                    -->
                    <div class="form-group">
                        <div class="col-md-12">
                            <asp:Button runat="server" OnClick="CreateUser_Click" Text="Register" CssClass="btn btn-primary" style="text-transform: uppercase; font-weight: 700; letter-spacing: 1px;"/>
                            <div class="g-recaptcha" data-sitekey="6Le3Zb0UAAAAAJaPgS3w8OBp7rGl_hh4lFwbGkcE" style="transform: translateX(-5%)"></div>
                        </div>
                    </div>
                </div>
            </div>
    </div>
    
</asp:Content>

