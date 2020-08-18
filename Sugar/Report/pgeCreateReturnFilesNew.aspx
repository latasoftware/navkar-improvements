<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pgeCreateReturnFilesNew.aspx.cs" Inherits="Sugar_Report_pgeCreateReturnFilesNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    
    <form id="form1" runat="server">
    <div>
        <ajax1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="true">
        </ajax1:ToolkitScriptManager>
        <table border="0" cellpadding="5" cellspacing="5" style="margin: 0 auto;">
            <tr>
                <td align="left">
                    From Date:
                </td>
                <td align="left">
                    <asp:TextBox ID="txtFromDt" runat="server" Width="80px" CssClass="txt" Height="24px"
                        MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                    <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                        Width="25px" Height="15px" />
                    <ajax1:CalendarExtender ID="calenderExtendertxtFromDt" runat="server" TargetControlID="txtFromDt"
                        PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                    </ajax1:CalendarExtender>
                </td>
                <td align="left">
                    To Date:
                </td>
                <td align="left">
                    <asp:TextBox ID="txtToDt" runat="server" Width="80px" CssClass="txt" Height="24px"
                        MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                        Height="15px" />
                    <ajax1:CalendarExtender ID="CalendarExtendertxtToDt" runat="server" TargetControlID="txtToDt"
                        PopupButtonID="Image1" Format="dd/MM/yyyy">
                    </ajax1:CalendarExtender>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <asp:Button Text="CREATE B2B FILE" runat="server" CssClass="btnHelp" ID="btnCreateb2b"
                        OnClick="btnCreateb2b_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <asp:Button ID="btnCreatePurchaseBillSummary" Text="PURCHASE BILL SUMMARY" runat="server"
                        OnClick="btnCreatePurchaseBillSummary_Click" />
                </td>
                <td colspan="1">
                    <asp:Button ID="btnCreateSaleBillSummary" Text="SALE BILL SUMMARY" runat="server"
                        OnClick="btnCreateSaleBillSummary_Click" />
                </td>
                <td colspan="1">
                    <asp:Button ID="btnFrieghtSummary" Text="FRIEGHT SUMMARY" runat="server" OnClick="btnFrieghtSummary_Click" />
                </td>
                <td colspan="1">
                    <asp:Button ID="btnDebitNoteSummary" Text="DEBIT NOTE SUMMARY" runat="server" OnClick="btnDebitNoteSummary_Click" />
                </td>
                <td colspan="1">
                    <asp:Button Text="E Way Bill" runat="server" ID="btnEwayBill" OnClick="btnEwayBill_Click" />
                </td>
                <td colspan="1">
                    <asp:Button Text="E Way Bill frp corpo" runat="server" ID="btnEwayBillcorpo" OnClick="btnEwayBillcorpo_Click" />
                </td>
                <td colspan="1">
                    <asp:Button Text="Empty E Way Bill" runat="server" ID="btnEmpty_E_way_Bill" OnClick="btnEmpty_E_way_Bill_Click" />
                </td>
                <td colspan="1">
                    <asp:Button Text="Sale BIll Checking" runat="server" ID="btnSale_Bill_Checking" OnClick="btnSale_Bill_Checking_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <asp:Button Text="Return Sale" runat="server" ID="btnReturnSale" OnClick="btnReturnSale_Click" />
                </td>
                <td>
                    <asp:Button Text="Return Purchase" runat="server" ID="btnpurchaseReturn" OnClick="btnpurchaseReturn_Click" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel runat="server" ID="pnlSale" BorderColor="Blue" Style="margin: 0 auto;"
            Width="100%">
            <br />
            <h3>
                <asp:Label Text="" ID="lblSummary" runat="server" /></h3>
            <asp:Button Text="EXPORT TO EXCEL" ID="btnExportToexcel" OnClick="btnExportToexcel_Click"
                runat="server" />
            <asp:GridView runat="server" ID="grdAll" AutoGenerateColumns="true" GridLines="Both"
                OnRowDataBound="grdAll_onRowDataBound" HeaderStyle-Font-Bold="true" RowStyle-Height="30px"
                ShowFooter="true">
                <FooterStyle BackColor="Yellow" Font-Bold="true" />
            </asp:GridView>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
