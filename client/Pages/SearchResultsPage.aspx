<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchResultsPage.aspx.cs" Inherits="ClientSite.Pages.SearchResultsPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Search</title>
    <link rel="stylesheet" type="text/css" href="../Content/Site.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Search Results<br />
        <br />
        <asp:Label ID="Information" runat="server"></asp:Label>
        <br />
        <br />
        <asp:Table ID="ResultsTable" runat="server">
        </asp:Table>
        <br />
        <br />
        <br />
        <br />
        <asp:Button ID="GoBack" runat="server" OnClick="GoBack_Click" Text="Go Back" />
&nbsp;
        <asp:Button ID="Print" runat="server" OnClick="Print_Click" Text="Print" />
&nbsp;
        <asp:Button ID="Exit" runat="server" OnClick="Exit_Click" Text="Get me outta here!" />
        <br />
    
    </div>
    </form>
</body>
</html>
