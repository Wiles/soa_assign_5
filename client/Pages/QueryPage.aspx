<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryPage.aspx.cs" Inherits="ClientSite.Pages.QueryPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Query Mr. Freeman</title>

    <link rel="stylesheet" type="text/css" href="../Content/Site.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="headerLabel" runat="server" Text="Insert some order"></asp:Label>
        <br />
        <br />
        <asp:Label ID="ClientErrors" runat="server" Text="Client Errors&lt;br&gt;..."></asp:Label>
        <br />
        <br />
        <asp:Label ID="ServerErrors" runat="server" Text="Service Errors&lt;br&gt;..."></asp:Label>
        <br />
        <br />
        <br />
    
    </div>
        <asp:CheckBox ID="GeneratePurchaseOrder" runat="server" Text="Generate Purchase Order (P.O.)" OnCheckedChanged="generatePurchaseOrder_CheckedChanged" Visible="False" AutoPostBack="True" />
        <br />
        <br />
        <br />
        <asp:RadioButton ID="customerRadio" runat="server" GroupName="section" AutoPostBack="True" OnCheckedChanged="customerRadio_CheckedChanged" />
        Customer<br />
        <asp:Label ID="Label2" runat="server" Text="CustID"></asp:Label>
        <asp:TextBox ID="CustId" runat="server" Width="173px" EnableTheming="True"></asp:TextBox>
        <asp:Label ID="Label3" runat="server" Text="Firstname"></asp:Label>
        <asp:TextBox ID="FirstName" runat="server"></asp:TextBox>
        <asp:Label ID="Label4" runat="server" Text="Lastname"></asp:Label>
        <asp:TextBox ID="LastName" runat="server"></asp:TextBox>
        <asp:Label ID="Label5" runat="server" Text="Phone Number"></asp:Label>
        <asp:TextBox ID="Phonenumber" runat="server"></asp:TextBox>
        xxx-xxx-xxxx<br />
        <br />
        <br />
        <asp:RadioButton ID="productRadio" runat="server" GroupName="section" AutoPostBack="True" OnCheckedChanged="productRadio_CheckedChanged" />
        Product<br />
        ProdID<asp:TextBox ID="ProdId" runat="server"></asp:TextBox>
        ProdName<asp:TextBox ID="ProdName" runat="server"></asp:TextBox>
        Price<asp:TextBox ID="Price" runat="server"></asp:TextBox>
        ProdWeight<asp:TextBox ID="ProdWeight" runat="server"></asp:TextBox>
&nbsp;<asp:CheckBox ID="SoldOut" runat="server" Text="Sold Out" />
        <br />
        <br />
        <br />
        <asp:RadioButton ID="orderRadio" runat="server" GroupName="section" AutoPostBack="True" OnCheckedChanged="orderRadio_CheckedChanged" />
        Order<br />
        OrderID<asp:TextBox ID="OrderId" runat="server"></asp:TextBox>
        CustId<asp:TextBox ID="OrderCustId" runat="server"></asp:TextBox>
        PoNumber<asp:TextBox ID="PoNumber" runat="server" Height="22px" Width="128px"></asp:TextBox>
        OrderDate<asp:TextBox ID="OrderDate" runat="server" Height="22px" Width="128px"></asp:TextBox>
        MM-DD-YY<br />
        <br />
        <br />
        <asp:RadioButton ID="cartRadio" runat="server" GroupName="section" AutoPostBack="True" OnCheckedChanged="cartRadio_CheckedChanged" />
        Cart<br />
        OrderID<asp:TextBox ID="CartOrderId" runat="server"></asp:TextBox>
        ProdId<asp:TextBox ID="CartProdId" runat="server"></asp:TextBox>
        Quantity<asp:TextBox ID="Quantity" runat="server"></asp:TextBox>
        <br />
        <br />
        <br />
        <asp:Button ID="GoBack" runat="server" OnClick="goBack_Click" Text="Go Back" />
&nbsp;
        <asp:Button ID="Execute" runat="server" OnClick="execute_Click" Text="Execute" />
&nbsp;
        <asp:Button ID="Exit" runat="server" OnClick="exit_Click" Text="Get me Outta Here!" />
        <br />
        <br />
    </form>
    <p>
        &nbsp;</p>
</body>
</html>
