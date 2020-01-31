<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TwoFactorAuthorization.aspx.cs" Inherits="Account_TwoFactorAuthorization" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Authorize Login - IU Event Hub</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h4>Enter 6 digit code</h4>
            <p>You have enabled Two-Factor Authentication on your account. A 6 digit code has been sent to your email address, please enter it to login.</p>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="codetxt" runat="server" ErrorMessage="Field is required"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="errorlbl" runat="server" Text=""></asp:Label>
            <br />
            <asp:TextBox ID="codetxt" runat="server"></asp:TextBox>
            <asp:Button ID="loginbtn" runat="server" Text="Login" OnClick="loginbtn_Click" />
        </div>
    </form>
</body>
</html>
