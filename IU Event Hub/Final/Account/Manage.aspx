<%@ Page Title="Manage Account" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Manage.aspx.cs" Inherits="Account_Manage" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<link rel="stylesheet" href="../Style/manage.css" type="text/css" media="screen" />
<link href="https://fonts.googleapis.com/css?family=Montserrat" rel="stylesheet">
    <h1><%: Title %></h1>

    <div>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success" ><%: SuccessMessage %></p>
        </asp:PlaceHolder>
    </div>
    <p>You're logged in as <%: User.Identity.GetUserName() %></p>
    <div class="container">
        <div class="row">
            <div class="col-md-4">
                <section id="passwordForm">
                    <asp:PlaceHolder runat="server" ID="changePasswordHolder" Visible="false">
                        <div class="form-horizontal">
                            <h4>Change Password</h4>
                            <asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />

                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:TextBox runat="server" ID="CurrentPassword" TextMode="Password" CssClass="form-control" placeholder="Current Password" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="CurrentPassword"
                                        CssClass="text-danger" ErrorMessage="The current password field is required."
                                        ValidationGroup="ChangePassword" />
                                </div>
                            </div>
                            
                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:TextBox runat="server" ID="NewPassword" TextMode="Password" CssClass="form-control" placeholder="New Password" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="NewPassword"
                                        CssClass="text-danger" ErrorMessage="The new password is required."
                                        ValidationGroup="ChangePassword" />
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:TextBox runat="server" ID="ConfirmNewPassword" TextMode="Password" CssClass="form-control" placeholder="Confirm New Password" />
                                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmNewPassword"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Confirm new password is required."
                                        ValidationGroup="ChangePassword" />
                                    <asp:CompareValidator runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                                        CssClass="text-danger" Display="Dynamic" ErrorMessage="The new password and confirmation password do not match."
                                        ValidationGroup="ChangePassword" />
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-md-12">
                                    <asp:Button runat="server" Text="Change password" OnClick="ChangePassword_Click" CssClass="btn btn-primary" ValidationGroup="ChangePassword" />
                                </div>
                                <div class="col-md-12">
                                    <asp:Button ID="twoFactorbtn" runat="server" Text="Enable Two-Factor Authentication" OnClick="change2FA" CssClass="btn btn-primary" />
                                </div>
                            </div>
                        </div>
                        
                    </asp:PlaceHolder>
                </section>    
            </div>

            <div class="col-md-4">
                <div>
                    <h4>Add Security Question</h4>
                        <asp:TextBox runat="server" ID="questiontxt" CssClass="form-control" placeholder="Security Question" style="margin-bottom: 10%;"/>
                        <asp:TextBox runat="server" ID="answertxt" CssClass="form-control" placeholder="Question Answer" style="margin-bottom: 5%;" />
                        <asp:Button ID="securitybtn" runat="server" Text="Submit" OnClick="changeQuestion" CssClass="btn btn-primary" />
                </div>
            </div>

                
            
                <!--VENUE OWNER VIEW ONLY-->
                <asp:Panel ID="ownerView" runat="server" Visible="false">
                    <div class="col-md-4">
                    <h4>Add/Modify Venue Details</h4>
                    <div class="form-group">
                        <!--
                            Add a new venue
                        -->
                        <asp:Label ID="venuestatuslbl" runat="server" Text=""></asp:Label>
                       
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AddVenue" />

                        <asp:TextBox ID="venuetxt" runat="server" placeholder="Venue Name"  CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Venue Name Field Required" ControlToValidate="venuetxt" ValidationGroup="AddVenue"></asp:RequiredFieldValidator>


                        <asp:TextBox ID="locationtxt" runat="server" placeholder="Venue Address" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Address Field Required" ControlToValidate="locationtxt" ValidationGroup="AddVenue"></asp:RequiredFieldValidator>


                        
                        <asp:TextBox ID="facilitiestxt" runat="server"  placeholder="Venue Facilities" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Facilities Field Required" ControlToValidate="facilitiestxt" ValidationGroup="AddVenue"></asp:RequiredFieldValidator>


                        
                        <asp:TextBox ID="capacitytxt" runat="server" onkeydown = "return (!(event.keyCode>=65) && event.keyCode!=32);"  placeholder="Venue Capacity" CssClass="form-control" style="margin-bottom: 2%;"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Capacity Field Required" ControlToValidate="capacitytxt" ValidationGroup="AddVenue"></asp:RequiredFieldValidator>


                        <asp:Label ID="dayslbl" runat="server" Text="Days Available" style="color: white;"></asp:Label>
                        <asp:DropDownList ID="days" runat="server" style="margin: 2% 0;" CssClass="form-control">
                            <asp:ListItem>All</asp:ListItem>
                            <asp:ListItem>Weekdays</asp:ListItem>
                            <asp:ListItem>Weekends</asp:ListItem>
                        </asp:DropDownList>
                        
                        <asp:TextBox ID="timetxt" runat="server"  placeholder="Time Available" CssClass="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Time Available Field Required" ControlToValidate="timetxt" ValidationGroup="AddVenue"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="timetxt" ValidationGroup="AddVenue" ErrorMessage="Time Available field must be in the form xx:xx-xx:xx" ValidationExpression="^([0-1]?[0-9]|[2][0-3]):([0-5][0-9])-([0-1]?[0-9]|[2][0-3]):([0-5][0-9])$"></asp:RegularExpressionValidator>
                        <div class="col-md-12">
                            <asp:Button ID="submitbtn" runat="server" Text="Submit" OnClick="submitbtn_Click" ValidationGroup="AddVenue" CssClass="btn btn-primary" />
                            <asp:Button ID="deletebtn" runat="server" Text="Close Venue" OnClick="deletebtn_Click" CssClass="btn btn-primary" style="transform:translateX(30%);" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>        
    </div>

</asp:Content>
