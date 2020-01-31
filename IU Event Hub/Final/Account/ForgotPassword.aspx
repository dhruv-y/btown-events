<%@ Page Title = "Forgot Password" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ForgotPassword.aspx.cs" Inherits="Account_ForgotPassword" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="../Style/forgot_password.css" type="text/css" media="screen" />
    <link href="https://fonts.googleapis.com/css?family=Montserrat" rel="stylesheet">

    <div class="container">
        <h1>Forgot Password</h1>
        <div class="inner">
        <asp:Label ID="errorlbl" runat="server" Text=""></asp:Label>
        <br />
        <asp:Label ID="Usernamelbl" runat="server" Text="Enter username below"></asp:Label>
        <asp:TextBox runat="server" ID="Reset" CssClass="form-control" style="position:relative; margin: 20px auto; width: 50%; text-transform: uppercase; font-weight: 700;"/>
        <asp:Button runat="server" OnClick="GetQuestion" Text="Answer Security Question" CssClass="btn btn-primary" style="text-transform: uppercase; font-weight: 700; letter-spacing: 1px;" />
        <br />
        <asp:Label ID="questionlbl" runat="server" Text=""></asp:Label>
        <br />
        <asp:TextBox runat="server" ID="answer" CssClass="form-control" Visible ="false" />
        <br />
        <asp:Button ID="resetbtn" runat="server" OnClick="ResetPass" Text="Get Password" CssClass="btn btn-default" visible ="false"/>
        </div>
        
    </div>

</asp:Content>