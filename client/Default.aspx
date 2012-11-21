<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ClientSite._Default" %>

<form id="form1" runat="server">
    <asp:Button ID="search" runat="server" OnClick="search_Click" Text="Search" />
    <p>
        <asp:Button ID="insert" runat="server" OnClick="insert_Click" Text="Insert Some Stuff" />
    </p>
    <p>
        <asp:Button ID="update" runat="server" OnClick="update_Click" Text="Update Some Stuff" />
    </p>
    <p>
        <asp:Button ID="delete" runat="server" OnClick="delete_Click" Text="Delete Some Stuff" />
    </p>
    <p>
        <asp:Button ID="exit" runat="server" OnClick="exit_Click" Text="Get me Outta Here!" />
    </p>
</form>


