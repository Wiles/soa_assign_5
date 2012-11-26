<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderPage.aspx.cs" Inherits="ClientSite.Pages.Screen3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <strong>Purchase Order<br />
        </strong>
        <br />
        <strong>Customer Information</strong><br />
        Customer ID:&nbsp;
        <asp:Label ID="CustomerID" runat="server"></asp:Label>
        <br />
        Name:
        <asp:Label ID="LastName" runat="server"></asp:Label>
        ,
        <asp:Label ID="FirstName" runat="server"></asp:Label>
        <br />
        Purchase Date:
        <asp:Label ID="PurchaseDate" runat="server"></asp:Label>
        <br />
        P.O. Number:
        <asp:Label ID="PoNumber" runat="server"></asp:Label>
        <br />
        <br />
        <strong>Orders</strong><br />
        <asp:GridView ID="OrdersGrid" runat="server">
        </asp:GridView>
        <br />
        <br />
        SubTotal:
        <asp:Label ID="SubTotal" runat="server"></asp:Label>
        <br />
        Tax (13%):
        <asp:Label ID="Tax" runat="server"></asp:Label>
        <br />
        Total:
        <asp:Label ID="Total" runat="server"></asp:Label>
        <br />
        <br />
        <br />
        Total Number of Pieces in Order:        Total Weight of Order:
        <br />
    
    </div>
        <asp:Button ID="GoBack" runat="server" Text="Go Back" />
&nbsp;
        <asp:Button ID="Print" runat="server" Text="Print" />
&nbsp;
        <asp:Button ID="Exit" runat="server" Text="Get me outta here!" />
    </form>
</body>
</html>
