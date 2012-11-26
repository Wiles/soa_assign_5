<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryPage.aspx.cs" Inherits="ClientSite.Pages.QueryPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
        <asp:CheckBox ID="generatePurchaseOrder" runat="server" Text="Generate Purchase Order (P.O.)" OnCheckedChanged="generatePurchaseOrder_CheckedChanged" Visible="False" AutoPostBack="True" />
        <br />
        <br />
        <br />
        <asp:RadioButton ID="customerRadio" runat="server" GroupName="section" AutoPostBack="True" OnCheckedChanged="customerRadio_CheckedChanged" />
        Customer<br />
        <asp:Label ID="Label2" runat="server" Text="CustID"></asp:Label>
        <asp:TextBox ID="custId" runat="server" Width="173px" EnableTheming="True"></asp:TextBox>
        <asp:Label ID="Label3" runat="server" Text="Firstname"></asp:Label>
        <asp:TextBox ID="firstname" runat="server"></asp:TextBox>
        <asp:Label ID="Label4" runat="server" Text="Lastname"></asp:Label>
        <asp:TextBox ID="lastname" runat="server"></asp:TextBox>
        <asp:Label ID="Label5" runat="server" Text="Phone Number"></asp:Label>
        <asp:TextBox ID="phonenumber" runat="server"></asp:TextBox>
        xxx-xxx-xxxx<br />
        <br />
        <br />
        <asp:RadioButton ID="productRadio" runat="server" GroupName="section" AutoPostBack="True" OnCheckedChanged="productRadio_CheckedChanged" />
        Product<br />
        ProdId<asp:TextBox ID="prodId" runat="server"></asp:TextBox>
        ProdName<asp:TextBox ID="prodName" runat="server"></asp:TextBox>
        Price<asp:TextBox ID="price" runat="server"></asp:TextBox>
        ProdWeight<asp:TextBox ID="prodWeight" runat="server"></asp:TextBox>
&nbsp;<asp:CheckBox ID="soldOut" runat="server" Text="Sold Out" />
        <br />
        <br />
        <br />
        <asp:RadioButton ID="orderRadio" runat="server" GroupName="section" AutoPostBack="True" OnCheckedChanged="orderRadio_CheckedChanged" />
        Order<br />
        OrderId<asp:TextBox ID="orderId" runat="server"></asp:TextBox>
        CustId<asp:TextBox ID="orderCustId" runat="server"></asp:TextBox>
        PoNumber<asp:TextBox ID="poNumber" runat="server" Height="22px" Width="128px"></asp:TextBox>
        OrderDate<asp:TextBox ID="orderDate" runat="server" Height="22px" Width="128px"></asp:TextBox>
        MM-DD-YY<br />
        <br />
        <br />
        <asp:RadioButton ID="cartRadio" runat="server" GroupName="section" AutoPostBack="True" OnCheckedChanged="cartRadio_CheckedChanged" />
        Cart<br />
        OrderId<asp:TextBox ID="cartOrderId" runat="server"></asp:TextBox>
        ProdId<asp:TextBox ID="cartProdId" runat="server"></asp:TextBox>
        Quantity<asp:TextBox ID="quantity" runat="server"></asp:TextBox>
        <br />
        <br />
        <br />
        <asp:Button ID="goBack" runat="server" OnClick="goBack_Click" Text="Go Back" />
&nbsp;
        <asp:Button ID="execute" runat="server" OnClick="execute_Click" Text="Execute" />
&nbsp;
        <asp:Button ID="exit" runat="server" OnClick="exit_Click" Text="Get me Outta Here!" />
        <br />
        <br />
    </form>
    <p>
        &nbsp;</p>
</body>
</html>
