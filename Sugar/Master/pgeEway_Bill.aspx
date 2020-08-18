<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="pgeEway_Bill.aspx.cs" Inherits="Sugar_Master_pgeEway_Bill" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%;
        margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px;
        border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="E-Way Bill" Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <br />
    <br />
    <asp:Panel runat="server" ID="pnlMain">
        <table width="60%" align="center" cellspacing="5">
            <tr>
                <td align="right" style="width: 30%;">
                    <b>Client Id:</b>
                </td>
                <td align="left" style="width: 30%;">
                    <asp:TextBox runat="server" ID="txtclientid" Width="150px" Height="24px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 30%;">
                    <b>Client Secret Key:</b>
                </td>
                <td align="left" style="width: 30%;">
                    <asp:TextBox runat="server" ID="txtclientsecretkey" Width="150px" Height="24px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 30%;">
                    <b>Token URL:</b>
                </td>
                <td align="left" style="width: 30%;">
                    <asp:TextBox runat="server" ID="txttokenurl" Width="150px" Height="24px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 30%;">
                    <b>GENEwayBill URL:</b>
                </td>
                <td align="left" style="width: 30%;">
                    <asp:TextBox runat="server" ID="txtgenewaybillurl" Width="150px" Height="24px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 30%;">
                    <b>User Name:</b>
                </td>
                <td align="left" style="width: 30%;">
                    <asp:TextBox runat="server" ID="txtusername" Width="150px" Height="24px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 30%;">
                    <b>Password:</b>
                </td>
                <td align="left" style="width: 30%;">
                    <asp:TextBox runat="server" ID="txtpassword" Width="150px" Height="24px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 30%;">
                    <b>Company GSTIN:</b>
                </td>
                <td align="left" style="width: 40%;">
                    <asp:TextBox runat="server" ID="txtgstin" Width="150px" Height="24px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button runat="server" ID="btnUpdate" Text="Update" CssClass="btnHelp" Width="100px" OnClick="btnUpdate_Click"/>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>

