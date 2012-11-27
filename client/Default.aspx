<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ClientSite._Default" %>

<style type="text/css">
    h1
    {
        text-align:center;
    }
    p
    {
        margin: 150px 100px;
        text-align:center;
    }
    input
    {
        margin: 0px 0px 100px 0px;
        width:150px;
        height: 35px;
    }
</style>

<form id="form1" runat="server">
    <h1>CRAZY MELVIN'S SHOPPING EMPORIUM</h1>

    <p>
        Here at Crazy Melvin's we believe in selling things for cheap!!  That's why our User Interface is cheap!
    </p>

    <p>
        Use the buttons below to tell me what you'd like to do here at Crazy Melvin's!!
    </p>

    <table style="width:99%;text-align:center;table-layout:fixed;">
        <tr>
            <td>
                <asp:Button ID="search" runat="server" OnClick="search_Click" Text="Search" />
            </td>
            <td>
                <asp:Button ID="insert" runat="server" OnClick="insert_Click" Text="Insert Some Stuff" />
            </td>
            <td>
                <asp:Button ID="update" runat="server" OnClick="update_Click" Text="Update Some Stuff" />
            </td>
            <td>
                <asp:Button ID="delete" runat="server" OnClick="delete_Click" Text="Delete Some Stuff" />
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:Button ID="exit" runat="server" OnClick="exit_Click" Text="Get me Outta Here!" />
            </td>
        </tr>
    </table>
</form>


