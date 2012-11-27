<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeletePage.aspx.cs" Inherits="ClientSite.Pages.Delete" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Delete Stuff...</title>
    <link rel="stylesheet" type="text/css" href="../Content/Site.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Delete some items!<br />
        <br />
        <asp:Label ID="ServerResponseText" runat="server"></asp:Label>
        <br />
        <br />
        Customer<br />
        <asp:DropDownList ID="CustomerDropDown" runat="server" AutoPostBack="True" DataSourceID="CustomerLinqDataSource" DataTextField="custID" DataValueField="custID" OnDataBound="CustomerDropDown_DataBound">
        </asp:DropDownList>
        <asp:Button ID="CustomerDelete" runat="server" OnClick="CustomerDelete_Click" Text="Delete" />
        <asp:LinqDataSource ID="CustomerLinqDataSource" runat="server" ContextTypeName="ClientSite.Sql.SoaDataContext" EntityTypeName="" Select="new (key as custID, it as Customers)" TableName="Customers" GroupBy="custID" OrderGroupsBy="key" Where="deleted == byte.MinValue">
        </asp:LinqDataSource>
        <br />
        <br />
        Order<br />
        <asp:DropDownList ID="OrderDropDown" runat="server" AutoPostBack="True" DataSourceID="OrderLinqDataSource" DataTextField="orderID" DataValueField="orderID" OnDataBound="OrderDropDown_DataBound">
        </asp:DropDownList>
        <asp:Button ID="OrderDelete" runat="server" OnClick="OrderDelete_Click" Text="Delete" />
        <asp:LinqDataSource ID="OrderLinqDataSource" runat="server" ContextTypeName="ClientSite.Sql.SoaDataContext" EntityTypeName="" Select="new (key as orderID, it as Carts)" TableName="Carts" GroupBy="orderID" OrderGroupsBy="key" Where="deleted == byte.MinValue">
        </asp:LinqDataSource>
        <br />
        <br />
        Product<br />
        <asp:DropDownList ID="ProductDropDown" runat="server" AutoPostBack="True" DataSourceID="ProductLinqDataSource" DataTextField="prodID" DataValueField="prodID" OnDataBound="ProductDropDown_DataBound">
        </asp:DropDownList>
        <asp:Button ID="ProductDelete" runat="server" OnClick="ProductDelete_Click" Text="Delete" />
        <asp:LinqDataSource ID="ProductLinqDataSource" runat="server" ContextTypeName="ClientSite.Sql.SoaDataContext" EntityTypeName="" Select="new (key as prodID, it as Carts)" TableName="Carts" GroupBy="prodID" OrderGroupsBy="key" Where="deleted == byte.MinValue">
        </asp:LinqDataSource>
        <br />
        <br />
        Cart (orderID,prodID)<br />
        <asp:DropDownList ID="CartDropDown" runat="server" AutoPostBack="True">
        </asp:DropDownList>
        <asp:Button ID="CartDelete" runat="server" OnClick="CartDelete_Click" Text="Delete" />
        <br />
        <br />
        <br />
        <asp:Button ID="GoBack" runat="server" OnClick="GoBack_Click" Text="Go Back" />
&nbsp;
        <asp:Button ID="Exit" runat="server" OnClick="Exit_Click" Text="Get me outta here!" />
    
    </div>
    </form>
</body>
</html>
