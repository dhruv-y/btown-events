<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Account_Login" Async="true" %>

<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <link rel="stylesheet" href="../Style/login.css" type="text/css" media="screen" />
    <link href="https://fonts.googleapis.com/css?family=Montserrat" rel="stylesheet">

    <h1><%: Title %></h1>

    <div class="row">
        <div class="col-md-12">
            <section id="loginForm">
                <div class="form-horizontal">
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="form-group">
                        
                        <div class="col-md-12">
                            <asp:TextBox runat="server" ID="UserName" CssClass="form-control" placeholder="enter your email" style="color:black" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                CssClass="text-danger" ErrorMessage="Username Required." />
                            <asp:RegularExpressionValidator ID="vldUser2" runat="server" ControlToValidate="UserName" ErrorMessage="Please enter a valid email address" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*">*</asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="form-group">

                        <div class="col-md-12">
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" placeholder="password" style="color:black"/>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="Password Required" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12">
                            <div class="checkbox" >
                                <asp:CheckBox runat="server" ID="RememberMe" />
                                <asp:Label runat="server" AssociatedControlID="RememberMe" style="color: white; font-size: 12px;">Remember Me</asp:Label>
                                
                                <p style="display: inline; margin-left: 25%; position:relative">
                                    <asp:HyperLink runat="server" ID="ForgotPasswordHyperLink" ViewStateMode="Disabled" style="color: white; font-size: 12px; text-decoration: none;">Forgot Password?</asp:HyperLink>
                                </p>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-12" style="margin-left:15px;">
                            <asp:Button runat="server" OnClick="LogIn" Text="Login" CssClass="btn btn-primary" style="text-transform: uppercase; font-weight: 700; letter-spacing: 1px;"/>
                        </div>
                    </div>
                </div>
                <p style="color: white; font-size: 14px;">
                    <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled" style="color: white; font-size: 14px; text-decoration:underline;">Register</asp:HyperLink>
                    a new account.
                </p>
            </section>
        </div>

        <div class="col-md-12">
            <section id="socialLoginForm">
                <uc:openauthproviders runat="server" id="OpenAuthLogin" />
            </section>
        </div>
    </div>
</asp:Content>

