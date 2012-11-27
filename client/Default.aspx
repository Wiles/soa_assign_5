<%@ Page Title="Home Page" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ClientSite._Default" %>

<form id="form1" runat="server">
    <style type="text/css">
        html {
            background-color: #e2e2e2;
            margin: 0;
            padding: 0;
        }

        body {
            background-color: #fff;
            color: #333;
            font-size: .85em;
            font-family: "Segoe UI", Verdana, Helvetica, Sans-Serif;
            margin: 0;
            padding: 50px;
        }

        input[type="submit"],
        input[type="button"],
        button {
            background-color: #d3dce0;
            border: 1px solid #787878;
            cursor: pointer;
            font-size: 1.2em;
            font-weight: 600;
            padding: 7px;
            margin-right: 8px;
            width: auto;
        }
    </style>

    <h1>CRAZY MELVIN'S SHOPPING EMPORIUM</h1>

    <p>
        Here at Crazy Melvin's we believe in selling things for cheap!!  That's why our User Interface is cheap!
    </p>

    <p>
        Use the buttons below to tell me what you'd like to do here at Crazy Melvin's!!
    </p>

    <br />
    <br />
    <asp:button id="search" runat="server" onclick="search_Click" text="Search" />
    <asp:button id="insert" runat="server" onclick="insert_Click" text="Insert Some Stuff" />
    <asp:button id="update" runat="server" onclick="update_Click" text="Update Some Stuff" />
    <asp:button id="delete" runat="server" onclick="delete_Click" text="Delete Some Stuff" />
    <asp:button id="exit" runat="server" onclick="exit_Click" text="Get me Outta Here!" />

    <br />
    <p style="margin-top: 5000px; color: white">
        (why you no muffins?) - Mr. Freeman
    </p>
</form>
