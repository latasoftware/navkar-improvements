<%@ Page Title="Tender Purchase Xml" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="pgeTenderPurchasexml.aspx.cs" Inherits="Sugar_pgeTenderPurchasexml" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../CSS/NewModalPopup.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Script/jquery-1.4.2.js"></script>

    <link href="../CSS/cssCommon.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../JQuery/jquery-1.4.1.js" type="text/javascript"></script>
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <script src="../JQuery/jquery.keynavigation.js" type="text/javascript"></script>
    <link type="text/css" href="../menu/menu.css" rel="stylesheet" />
    <script type="text/javascript" src="../menu/jquery.js"></script>
    <script type="text/javascript" src="../menu/menu.js"></script>
    <script type="text/javascript" src="../JS/DateValidation.js"> </script>

    <script src="../JS/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="../Scripts/selectfirstrow.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        debugger;
        document.addEventListener('keyup', function (event) {
            if (event.defaultPrevented) {
                return;
            }

            var key = event.key || event.keyCode;

            if (key === 'Escape' || key === 'Esc' || key === 27) {
                //                doWhateverYouWantNowThatYourKeyWasHit();
                debugger;
                document.getElementById("<%=pnlPopup.ClientID %>").style.display = "none";
                var hdnfClosePopupValue = document.getElementById("<%= hdnfClosePopup.ClientID %>").value;

                if (hdnfClosePopupValue == "GR") {
                    document.getElementById("<%=txtGrade.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "PT") {
                    document.getElementById("<%=txtPaymentTo.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "TF") {
                    document.getElementById("<%=txtTenderFrom.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "DO") {
                    document.getElementById("<%=txtDO.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "VB") {
                    document.getElementById("<%=txtVoucherBy.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "BR") {
                    document.getElementById("<%=txtBroker.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "BU") {
                    document.getElementById("<%=txtBuyer.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "BP") {
                    document.getElementById("<%=txtBuyerParty.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "TN") {
                    document.getElementById("<%=txtTenderNo.ClientID %>").disabled = false;
                    document.getElementById("<%=txtTenderNo.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtEditDoc_No") {
                    document.getElementById("<%=btnSearch.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "MM") {
                    document.getElementById("<%=txtMillCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "GC") {
                    document.getElementById("<%=txtGstrateCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtitem_code") {
                    document.getElementById("<%=txtitem_code.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtsubBroker") {
                    document.getElementById("<%=txtsubBroker.ClientID %>").focus();
                  }
                  document.getElementById("<%=txtSearchText.ClientID %>").value = "";
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";
            }

        });
    </script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to delete data?")) {
                confirm_value.value = "Yes";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "Yes";
            }
            else {
                confirm_value.value = "No";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <script type="text/javascript" language="javascript">

        var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;
        function SelectSibling(e) {
            var e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;
            if (KeyCode == 40) {
                SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1);
            }
            else if (KeyCode == 38) {
                SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1);
            }
            else if (KeyCode == 13) {
                document.getElementById("<%=pnlPopup.ClientID %>").style.display = "none";
                document.getElementById("<%=txtSearchText.ClientID %>").value = "";
                var hdnfClosePopupValue = document.getElementById("<%= hdnfClosePopup.ClientID %>").value;
                var grid = document.getElementById("<%= grdPopup.ClientID %>");
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";

                var pageCount = document.getElementById("<%= hdHelpPageCount.ClientID %>").value;
                pageCount = parseInt(pageCount);
                if (pageCount > 1) {
                    SelectedRowIndex = SelectedRowIndex + 1;
                }




                if (hdnfClosePopupValue == "GR") {
                    document.getElementById("<%=txtGrade.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtGrade.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "PT") {
                    document.getElementById("<%=txtPaymentTo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblPaymentTo.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtPaymentTo.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "TF") {
                    document.getElementById("<%=txtTenderFrom.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblTenderFrom.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtTenderFrom.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "DO") {
                    document.getElementById("<%=txtDO.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblDO.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtDO.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "VB") {
                    document.getElementById("<%=txtVoucherBy.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblVoucherBy.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtVoucherBy.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "BR") {
                    document.getElementById("<%=txtBroker.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblBroker.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtBroker.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "BU") {

                    document.getElementById("<%=txtBuyer.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblBuyerName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtBuyer.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "BP") {

                    document.getElementById("<%=txtBuyerParty.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblBuyerPartyName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtBuyerParty.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "TN") {
                    document.getElementById("<%=txtTenderNo.ClientID %>").value = "";
                    document.getElementById("<%=txtTenderNo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtTenderNo.ClientID %>").disabled = false;
                    document.getElementById("<%=txtTenderNo.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtEditDoc_No") {
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").value = "";
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").focus();
                    document.getElementById("<%=btnSearch.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "MM") {
                    document.getElementById("<%=txtMillCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblMillName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtMillCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "GC") {
                    document.getElementById("<%=txtGstrateCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblgstrateCode.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[2].innerText;
                    document.getElementById("<%=txtGstrateCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtitem_code") {
                    document.getElementById("<%=txtitem_code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblitemname.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;

                    document.getElementById("<%=txtitem_code.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "SubBrker") {
                    document.getElementById("<%=txtsubBroker.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblsubBroker.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtsubBroker.ClientID %>").focus();
                }
            }
}


function SelectRow(CurrentRow, RowIndex) {
    UpperBound = parseInt('<%= this.grdPopup.Rows.Count %>') - 1;
    LowerBound = 0;
    if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound)

        if (SelectedRow != null) {
            SelectedRow.style.backgroundColor = SelectedRow.originalBackgroundColor;
            SelectedRow.style.color = SelectedRow.originalForeColor;
        }
    if (CurrentRow != null) {
        CurrentRow.originalBackgroundColor = CurrentRow.style.backgroundColor;
        CurrentRow.originalForeColor = CurrentRow.style.color;
        CurrentRow.style.backgroundColor = '#DCFC5C';
        CurrentRow.style.color = 'Black';
    }
    SelectedRow = CurrentRow;
    SelectedRowIndex = RowIndex;
    setTimeout("SelectedRow.focus();", 0);
}

    </script>
    <script type="text/javascript">
        function refreshparent(source) {
            if (source == 'R') {
                window.close();
                window.opener.location = "";
                window.opener.location.reload();
            }
        }

    </script>
    <script type="text/javascript">
        function MillCode(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnMillCode.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtMillCode.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtMillCode.ClientID %>").val(unit);
                __doPostBack("txtMillCode", "TextChanged");

            }

        }

        function Grade(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnGrade.ClientID %>").click();

            }


        }

        function PaymentTo(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnPaymentTo.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtPaymentTo.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtPaymentTo.ClientID %>").val(unit);
                __doPostBack("txtPaymentTo", "TextChanged");

            }

        }
        function subBroker(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnsubBrker.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtsubBroker.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtsubBroker.ClientID %>").val(unit);
                __doPostBack("txtsubBroker", "TextChanged");

            }

        }
        function tenderDo(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnTenderDO.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtDO.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtDO.ClientID %>").val(unit);
                __doPostBack("txtDO", "TextChanged");

            }

        }
        function tenderfrom(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnTenderFrom.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtTenderFrom.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtTenderFrom.ClientID %>").val(unit);
                __doPostBack("txtTenderFrom", "TextChanged");

            }

        }
        function broker(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnBroker.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtBroker.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtBroker.ClientID %>").val(unit);
                __doPostBack("txtBroker", "TextChanged");

            }

        }
        function VoucherBy(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnVoucherBy.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtVoucherBy.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtVoucherBy.ClientID %>").val(unit);
                __doPostBack("txtVoucherBy", "TextChanged");

            }

        }
        function Party(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnBuyer.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtBuyer.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtBuyer.ClientID %>").val(unit);
                __doPostBack("txtBuyer", "TextChanged");

            }
            if (e.keyCode == 13) {
                e.preventDefault();
                $("#<%=btnSave.ClientID %>").focus();
            }
        }
        function detailBroker(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnBuyerParty.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtBuyerParty.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtBuyerParty.ClientID %>").val(unit);
                __doPostBack("txtBuyerParty", "TextChanged");

            }

        }
        function gstRateCode(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnGstrateCode.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtGstrateCode.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtGstrateCode.ClientID %>").val(unit);
                __doPostBack("txtGstrateCode", "TextChanged");

            }

        }
        function item(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtitem_code.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtitem_code.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtitem_code.ClientID %>").val(unit);
                __doPostBack("txtitem_code", "TextChanged");

            }

        }
    </script>
    <script type="text/javascript">
        function Back() {

            //alert(td);
            window.open('../BussinessRelated/PgeTenderHeadUtility.aspx', '_self');
        }

        function TenderOPen(TenderID) {
            var Action = 1;
            window.open('../BussinessRelated/pgeTenderPurchasexml.aspx?tenderid=' + TenderID + '&Action=' + Action, "_self");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Tender Purchase   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:UpdatePanel ID="upPnlPopup" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdnfIsClick" runat="server" Value="0" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfNextFocus" runat="server" />
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:HiddenField ID="vouchernumber" runat="server" />
            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="True" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table width="100%" align="left" cellspacing="4px" cellpadding="1px">
                    <tr>
                        <td colspan="4">
                            <table width="80%" align="left">
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="80px"
                                            Height="25px" OnClick="btnAdd_Click" />
                                        &nbsp;
                                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnHelp" Width="80px"
                                            Height="25px" ValidationGroup="add" OnClick="btnSave_Click" TabIndex="34" />
                                        &nbsp;
                                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="80px"
                                            Height="25px" ValidationGroup="save" OnClick="btnEdit_Click" />
                                        &nbsp;
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="80px"
                                            Height="25px" ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()" />
                                        &nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="80px"
                                            Height="25px" OnClick="btnCancel_Click" TabIndex="34" />
                                        &nbsp;
                                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btnHelp" Width="80px" OnClientClick="Back()"
                                            Height="25px" TabIndex="35" />
                                    </td>

                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">Change No:</td>

                        <td colspan="4" align="left">
                            <asp:TextBox runat="server" ID="txtEditDoc_No" CssClass="txt" Width="100px" Height="24px"
                                AutoPostBack="true" OnTextChanged="txtEditDoc_No_TextChanged" TabIndex="1"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="txtxf" FilterType="Numbers" InvalidChars="."
                                TargetControlID="txtEditDoc_No">
                            </ajax1:FilteredTextBoxExtender>
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Names="verdana" Font-Italic="true"
                                Font-Size="Small" ForeColor="Yellow" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">Tender No:
                        </td>
                        <td align="left" colspan="4">
                            <asp:TextBox ID="txtTenderNo" runat="server" CssClass="txt" Width="100px" TabIndex="1"
                                AutoPostBack="false" OnTextChanged="txtTenderNo_TextChanged" Height="24px"></asp:TextBox>
                            <asp:Button ID="btnChangeNo" runat="server" Text="Change No" CssClass="btnHelp" Width="69px"
                                TabIndex="1" OnClick="changeNo_click" Height="24px" />
                            <asp:Label ID="lblTender_Id" runat="server" Text="" Font-Names="verdana"
                                ForeColor="Blue" Font-Bold="true" Font-Size="12px" Visible="true"></asp:Label></legend>
                            &nbsp;Resale/Mill:&nbsp;&nbsp;&nbsp;
                            <asp:DropDownList ID="drpResale" runat="server" AutoPostBack="true" Width="120px"
                                CssClass="ddl" OnSelectedIndexChanged="drpResale_SelectedIndexChanged" TabIndex="2">
                                <asp:ListItem Text="Resale" Value="R"></asp:ListItem>
                                <asp:ListItem Text="Mill" Value="M" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="With Payment" Value="W"></asp:ListItem>
                            </asp:DropDownList>
                            &nbsp; Voucher No:<asp:Label runat="server" ID="lblVoucherNo" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Date:
                        </td>
                        <td align="left" colspan="3">
                            <asp:TextBox ID="txtDate" runat="server" CssClass="txt" Width="100px" AutoPostBack="True"
                                MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                OnTextChanged="txtDate_TextChanged" TabIndex="3" Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtDate"
                                PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Payment Date:
                            &nbsp;&nbsp;<asp:TextBox ID="txtLiftingDate" runat="server" CssClass="txt" Width="100px"
                                AutoPostBack="true" OnTextChanged="txtLiftingDate_TextChanged" TabIndex="4" onkeyup="ValidateDate(this,event.keyCode)"
                                onkeydown="return DateFormat(this,event.keyCode)" MaxLength="10" Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender1" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderLiftingdate" runat="server" TargetControlID="txtLiftingDate"
                                PopupButtonID="imgcalender1" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                            <asp:Label runat="server" ID="lblMesg"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Mill Code:
                        </td>
                        <td align="left" colspan="4">
                            <asp:TextBox ID="txtMillCode" runat="server" CssClass="txt" Width="100px" Style="text-align: right;" onkeydown="MillCode(event);"
                                AutoPostBack="false" OnTextChanged="txtMillCode_TextChanged"
                                TabIndex="5" Height="24px"></asp:TextBox>
                            <asp:Button ID="btnMillCode" runat="server" Text="..." CssClass="btnHelp" OnClick="btnMillCode_Click"
                                Height="24px" Width="20px" />
                            <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtMillCode" FilterType="Numbers" TargetControlID="txtMillCode"></ajax1:FilteredTextBoxExtender>--%>
                            <asp:Label ID="lblMillName" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Label ID="lblMill_Id" runat="server" CssClass="lblName" Visible="false"></asp:Label>
                            Season: 
                            <asp:TextBox ID="txtSeason" runat="server" CssClass="txt" Width="100px" Style="text-align: right;"
                                AutoPostBack="false"
                                TabIndex="6" Height="24px"></asp:TextBox>
                            Item Code: 
                            <asp:TextBox ID="txtitem_code" runat="server" CssClass="txt" Width="100px" Style="text-align: right;"
                                onkeydown="item(event);" AutoPostBack="false" OnTextChanged="txtitem_code_TextChanged"
                                TabIndex="6" Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtitem_code" runat="server" Text="..." CssClass="btnHelp" OnClick="btntxtitem_code_Click"
                                Height="24px" Width="20px" />
                            <asp:Label ID="lblitemname" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Grade:
                        </td>
                        <td align="left" colspan="6">
                            <asp:TextBox ID="txtGrade" runat="server" CssClass="txt" Width="100px" TabIndex="7"
                                AutoPostBack="false" OnTextChanged="txtGrade_TextChanged" Height="24px" onkeydown="Grade(event);"></asp:TextBox>
                            <asp:Button ID="btnGrade" runat="server" Text="..." CssClass="btnHelp" OnClick="btnGrade_Click"
                                Height="24px" Width="20px" />
                            <asp:Label ID="lblGrade_Id" runat="server" CssClass="lblName" Visible="false"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Quintal:
                            <asp:TextBox ID="txtQuantal" runat="server" CssClass="txt" Width="100px" Style="text-align: left;"
                                AutoPostBack="True" OnTextChanged="txtQuantal_TextChanged" TabIndex="8" Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtQuantal" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtQuantal">
                            </ajax1:FilteredTextBoxExtender>
                            Packing:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtPacking" runat="server" CssClass="txt" AutoPostBack="True" Width="100px"
                                Style="text-align: left;" OnTextChanged="txtPacking_TextChanged" TabIndex="9"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtPacking" FilterType="Numbers"
                                TargetControlID="txtPacking">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;&nbsp;&nbsp;Bags:
                            <asp:TextBox ID="txtBags" runat="server" CssClass="txt" ReadOnly="true" Width="100px"
                                TabIndex="10" Style="text-align: left;" Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBags" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtBags">
                            </ajax1:FilteredTextBoxExtender>
                            Balance Self:
                            <asp:Label ID="lblBalanceSelf" runat="server" Text="0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Mill Rate:
                        </td>
                        <td align="left" colspan="4">
                            <asp:TextBox ID="txtMillRate" runat="server" CssClass="txt" AutoPostBack="true" Width="100px"
                                Style="text-align: right;" OnTextChanged="txtMillRate_TextChanged" TabIndex="11"
                                Height="24px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtMillRate" SetFocusOnError="true" runat="server"
                                ControlToValidate="txtMillRate" CssClass="validator" Display="Dynamic" Text="Required"
                                ErrorMessage="Required" ValidationGroup="add">
                            </asp:RequiredFieldValidator>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtMillRate" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtMillRate">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Purch Rate:
                            <asp:TextBox ID="txtPurcRate" runat="server" AutoPostBack="True" CssClass="txt" Width="100px"
                                OnTextChanged="txtPurcRate_TextChanged" TabIndex="12" Height="24px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtPurcRate" Enabled="false" SetFocusOnError="true"
                                runat="server" ControlToValidate="txtPurcRate" CssClass="validator" Display="Dynamic"
                                Text="Required" ErrorMessage="Required" ValidationGroup="add">
                            </asp:RequiredFieldValidator>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtPurcRate" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtPurcRate">
                            </ajax1:FilteredTextBoxExtender>
                            &emsp;
                            C.A:
                             <asp:TextBox ID="txtCashDiff" runat="server" AutoPostBack="false" CssClass="txt" Width="100px"
                                 OnTextChanged="txtCashDiff_TextChanged" TabIndex="13" Height="24px"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;Diff:
                            <asp:Label ID="lbldiff" runat="server" Text="Diff"></asp:Label>
                            Amount:
                            <asp:Label ID="lblAmount" runat="server" Text="Amount"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Payment To:
                        </td>
                        <td colspan="6" align="left">
                            <asp:TextBox ID="txtPaymentTo" runat="server" CssClass="txt" Width="100px" Style="text-align: right;"
                                AutoPostBack="false" OnTextChanged="txtPaymentTo_TextChanged" TabIndex="14" Height="24px" onkeydown="PaymentTo(event);"></asp:TextBox>
                            <asp:Button ID="btnPaymentTo" runat="server" Text="..." CssClass="btnHelp" OnClick="btnPaymentTo_Click"
                                Height="24px" Width="20px" />
                            <asp:Label ID="lblPaymentTo" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Label ID="lblPayment_Id" runat="server" CssClass="lblName" Visible="false"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvtxtPaymentTo" runat="server" ControlToValidate="txtPaymentTo"
                                CssClass="validator" Display="Dynamic" Enabled="false" ErrorMessage="Required"
                                SetFocusOnError="true" Text="Required" ValidationGroup="add">
                            </asp:RequiredFieldValidator>
                            Tender From:
                            <asp:TextBox ID="txtTenderFrom" runat="server" CssClass="txt" Width="100px" TabIndex="14"
                                Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtTenderFrom_TextChanged"
                                Height="24px" onkeydown="tenderfrom(event);"></asp:TextBox>
                            <asp:Button ID="btnTenderFrom" runat="server" Text="..." CssClass="btnHelp" OnClick="btnTenderFrom_Click"
                                Height="24px" Width="20px" />
                            <asp:Label ID="lblTenderFrom" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Label ID="lblTenderForm_Id" runat="server" CssClass="lblName" Visible="false"></asp:Label>
                            <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtTenderFrom" FilterType="Numbers"  TargetControlID="txtTenderFrom"></ajax1:FilteredTextBoxExtender>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Tender D.O.:
                        </td>
                        <td colspan="6" align="left">
                            <asp:TextBox ID="txtDO" runat="server" CssClass="txt" Width="100px" TabIndex="15"
                                Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtDO_TextChanged"
                                Height="24px" onkeydown="tenderDo(event);"></asp:TextBox>
                            <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtDO" FilterType="Numbers"  TargetControlID="txtDO"></ajax1:FilteredTextBoxExtender>--%>
                            <asp:Button ID="btnTenderDO" runat="server" Text="..." OnClick="btnTenderDO_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblDO" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Label ID="lblTenderDo_Id" runat="server" CssClass="lblName" Visible="false"></asp:Label>
                            <asp:RequiredFieldValidator ID="rfvtxtDO" Enabled="false" SetFocusOnError="true"
                                runat="server" ControlToValidate="txtDO" CssClass="validator" Display="Dynamic"
                                Text="Required" ErrorMessage="Required" ValidationGroup="add">
                            </asp:RequiredFieldValidator>
                            Voucher By: &nbsp;
                            <asp:TextBox ID="txtVoucherBy" runat="server" CssClass="txt" Width="100px" Style="text-align: right;"
                                AutoPostBack="false" OnTextChanged="txtVoucherBy_TextChanged" TabIndex="16" Height="24px" onkeydown="VoucherBy(event);"></asp:TextBox>
                            <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtVoucherBy" FilterType="Numbers"  TargetControlID="txtVoucherBy"></ajax1:FilteredTextBoxExtender>--%>
                            <asp:Button ID="btnVoucherBy" runat="server" Text="..." CssClass="btnHelp" OnClick="btnVoucherBy_Click"
                                Height="24px" Width="20px" />
                            <asp:Label ID="lblVoucherBy" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Label ID="lblVoucherBy_Id" runat="server" CssClass="lblName" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Broker:
                        </td>
                        <td colspan="4" align="left">
                            <asp:TextBox ID="txtBroker" runat="server" CssClass="txt" Width="100px" Style="text-align: right;"
                                AutoPostBack="false" OnTextChanged="txtBroker_TextChanged" TabIndex="17" Height="24px" onkeydown="broker(event);"></asp:TextBox>
                            <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBroker" FilterType="Numbers"  TargetControlID="txtBroker"></ajax1:FilteredTextBoxExtender>--%>
                            <asp:Button ID="btnBroker" runat="server" Text="..." OnClick="btnBroker_Click" CssClass="btnHelp"
                                Height="24px" Width="20px" />
                            <asp:Label ID="lblBroker" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Label ID="lblBroker_Id" runat="server" CssClass="lblName"></asp:Label>&nbsp;&nbsp;Brokrage:<asp:TextBox
                                runat="server" ID="txtBrokrage" CssClass="txt" Width="80px" Height="24px" TabIndex="18" />
                            &nbsp;&nbsp; GST Rate Code
                       <asp:TextBox ID="txtGstrateCode" runat="server" CssClass="txt" Width="100px" Style="text-align: right;"
                           AutoPostBack="false" OnTextChanged="txtGstrateCode_TextChanged" TabIndex="19" Height="24px" onkeydown="gstRateCode(event);"></asp:TextBox>
                            <asp:Button ID="btnGstrateCode" runat="server" Text="..." CssClass="btnHelp" OnClick="btnGstrateCode_Click"
                                Height="24px" Width="20px" />
                            <asp:Label ID="lblgstrateCode" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Label ID="lblgstID" runat="server" CssClass="lblName" Visible="false"></asp:Label>

                            Excise / GST Rate:
                       
                            <asp:TextBox ID="txtExciseRate" runat="server" CssClass="txt" TabIndex="20" Width="100px"
                                Style="text-align: right;" AutoPostBack="true" OnTextChanged="txtExciseRate_TextChanged"
                                Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtExciseRate" FilterType="Custom,Numbers"
                                ValidChars="." TargetControlID="txtExciseRate">
                            </ajax1:FilteredTextBoxExtender>
                            &nbsp;GST Rate:<asp:Label Text="" runat="server" ID="lblMillRateGst" ForeColor="Yellow" />&nbsp;
                            Sell Note No:<asp:TextBox runat="server" ID="txtSellNoteNo" Width="150px" Height="24px"
                                TabIndex="21" Style="text-align: right;" CssClass="txt" OnTextChanged="txtSellNoteNo_TextChanged"></asp:TextBox>&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Narration:
                        </td>
                        <td colspan="4" align="left">

                            <asp:TextBox ID="txtNarration" runat="server" CssClass="txt" TabIndex="22" AutoPostBack="True"
                                Width="250px" OnTextChanged="txtNarration_TextChanged" Height="24px"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <fieldset style="border-top: 3px dotted rgb(131, 127, 130); border-radius: 3px; width: 172%; margin-left: -1160px; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 5px;">
                    <legend style="text-align: left;">
                        <h2 style="color: Black; text-align: left;" font-names="verdana" font-size="large">Tender Detail Section</h2>
                    </legend>
                </fieldset>
                <asp:Panel ID="Panel2" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                    Font-Size="Small" Style="margin-top: 0px; z-index: 100;">

                    <table width="100%" cellpadding="4px" cellspacing="4px">
                        <tr>
                            <td align="left">ID:
                           
                                <asp:Label ID="lblID" runat="server"></asp:Label>
                                <asp:Label ID="lblno" runat="server" ForeColor="Azure"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">Party:
                           
                                <asp:TextBox ID="txtBuyer" runat="server" Width="80px" Height="24px" AutoPostBack="false"
                                    CssClass="txt" OnTextChanged="txtBuyer_TextChanged" onkeydown="Party(event);" TabIndex="23"></asp:TextBox>
                                <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBuyer" FilterType="Numbers" TargetControlID="txtBuyer"></ajax1:FilteredTextBoxExtender>--%>
                                <asp:Button ID="btnBuyer" runat="server" Text="..." Height="24px" Width="20px" CssClass="btnHelp"
                                    OnClick="btnBuyer_Click" />
                                <asp:Label ID="lblBuyerName" runat="server" CssClass="lblName"></asp:Label>
                                <asp:Label ID="lblbuyer_id" runat="server" Visible="false"></asp:Label>
                                <asp:RequiredFieldValidator ID="rfvtxtBuyer" runat="server" ControlToValidate="txtBuyer"
                                    CssClass="validator" Display="Dynamic" ErrorMessage="Required" SetFocusOnError="true"
                                    Text="Required" ValidationGroup="addBuyerDetails">
                                </asp:RequiredFieldValidator>
                                Delivery Type:
                                   <asp:DropDownList ID="drpDeliveryType" runat="server" CssClass="ddl" Width="140px"
                                       TabIndex="24" AutoPostBack="true" Height="26px" OnSelectedIndexChanged="drpDeliveryType_SelectedIndexChanged">
                                       <asp:ListItem Text="Naka Delivery" Value="N" Selected="True"></asp:ListItem>
                                       <asp:ListItem Text="Commission" Value="C"></asp:ListItem>
                                       <asp:ListItem Text="DO" Value="D"></asp:ListItem>
                                   </asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <td align="left">Broker:
                           
                                <asp:TextBox ID="txtBuyerParty" runat="server" Width="80px" CssClass="txt" Height="24px"
                                    OnTextChanged="txtBuyerParty_TextChanged" AutoPostBack="false" onkeydown="detailBroker(event);" TabIndex="25"></asp:TextBox>
                                <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBuyerParty" FilterType="Numbers" TargetControlID="txtBuyerParty"></ajax1:FilteredTextBoxExtender>--%>
                                <asp:Button ID="btnBuyerParty" Height="24px" Width="20px" runat="server" Text="..."
                                    CssClass="btnHelp" OnClick="btnBuyerParty_Click" />
                                <asp:Label ID="lblBuyerPartyName" runat="server" CssClass="lblName"></asp:Label>
                                <asp:Label ID="lblbuyerparty_id" runat="server" Visible="false"></asp:Label>


                                Sub Broker:
                                  <asp:TextBox ID="txtsubBroker" runat="server" Width="80px" CssClass="txt" Height="24px"
                                      OnTextChanged="txtsubBroker_TextChanged" AutoPostBack="false" onkeydown="subBroker(event);" TabIndex="26"></asp:TextBox>
                                <%--<ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBuyerParty" FilterType="Numbers" TargetControlID="txtBuyerParty"></ajax1:FilteredTextBoxExtender>--%>
                                <asp:Button ID="btnsubBrker" Height="24px" Width="20px" runat="server" Text="..."
                                    CssClass="btnHelp" OnClick="btnsubBrker_Click" />
                                <asp:Label ID="lblsubBroker" runat="server" CssClass="lblName"></asp:Label>
                                <asp:Label ID="lblsubId" runat="server" Visible="false"></asp:Label>


                                Buyer Quantal:
                                

                                <asp:TextBox ID="txtBuyerQuantal" runat="server" Width="80px" Height="24px" CssClass="txt"
                                    AutoPostBack="true" OnTextChanged="txtBuyerQuantal_TextChanged" TabIndex="27"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBuyerQuantal" FilterType="Custom,Numbers"
                                    ValidChars="." TargetControlID="txtBuyerQuantal">
                                </ajax1:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfvtxtBuyerQuantal" runat="server" ControlToValidate="txtBuyerQuantal"
                                    CssClass="validator" Display="Dynamic" ErrorMessage="Required" SetFocusOnError="true"
                                    Text="Required" ValidationGroup="addBuyerDetails">
                                </asp:RequiredFieldValidator>

                                Sale Rate:
                           
                                <asp:TextBox ID="txtBuyerSaleRate" runat="server" Width="80px" Height="24px" CssClass="txt"
                                    AutoPostBack="true" OnTextChanged="txtBuyerSaleRate_TextChanged" TabIndex="28"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBuyerSaleRate" FilterType="Custom,Numbers"
                                    ValidChars="." TargetControlID="txtBuyerSaleRate">
                                </ajax1:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="rfvtxtBuyerSaleRate" runat="server" ControlToValidate="txtBuyerSaleRate"
                                    CssClass="validator" Display="Dynamic" ErrorMessage="Required" SetFocusOnError="true"
                                    Text="Required" ValidationGroup="addBuyerDetails">
                                </asp:RequiredFieldValidator>

                                Commission:
                          
                                <asp:TextBox ID="txtBuyerCommission" runat="server" CssClass="txt" Height="24px"
                                    TabIndex="29" Width="80px" AutoPostBack="true" OnTextChanged="txtBuyerCommission_TextChanged"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender runat="server" ID="filtertxtBuyerCommission" FilterType="Custom,Numbers"
                                    ValidChars="." TargetControlID="txtBuyerCommission">
                                </ajax1:FilteredTextBoxExtender>

                            </td>
                        </tr>

                        <tr>
                            <td align="left">Sauda Date:
                          
                                <asp:TextBox ID="txtDetailSaudaDate" runat="server" CssClass="txt" Width="100px"
                                    AutoPostBack="True" MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)"
                                    onkeydown="return DateFormat(this,event.keyCode)" OnTextChanged="txtDetailSaudaDate_TextChanged"
                                    TabIndex="30" Height="24px"></asp:TextBox>
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                                    Height="15px" />
                                <ajax1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDetailSaudaDate"
                                    PopupButtonID="Image1" Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>

                                Payment Date:
                           
                                <asp:TextBox ID="txtDetailLiftingDate" runat="server" CssClass="txt" Width="100px"
                                    AutoPostBack="True" MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)"
                                    onkeydown="return DateFormat(this,event.keyCode)" OnTextChanged="txtDetailLiftingDate_TextChanged"
                                    TabIndex="31" Height="24px"></asp:TextBox>
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                                    Height="15px" />
                                <ajax1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDetailLiftingDate"
                                    PopupButtonID="Image2" Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>

                                Narration:
                           
                                <asp:TextBox ID="txtBuyerNarration" runat="server" Width="360px" CssClass="txt" Height="50px"
                                    TextMode="MultiLine" TabIndex="32" AutoPostBack="true" OnTextChanged="txtBuyerNarration_TextChanged"></asp:TextBox>

                                <%--<td colspan="2" align="center">--%>
                                <asp:Button ID="btnADDBuyerDetails" runat="server" Text="ADD" CssClass="btnHelp"
                                    Font-Bold="false" OnClick="btnADDBuyerDetails_Click" Width="90px" Height="24px" ValidationGroup="addBuyerDetails"
                                    TabIndex="33" />
                                <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" CssClass="btnHelp"
                                    TabIndex="34" Font-Bold="false" CausesValidation="false" Width="90px" Height="24px" />

                                <asp:Label ID="lbltenderdetailid" runat="server" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <div>
                <asp:UpdatePanel ID="upGrid" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnlgrdDetail" runat="server" ScrollBars="Both" Height="220px" Width="1200px"
                            BorderColor="Maroon" BorderWidth="1px" BorderStyle="Solid" Font-Bold="true" Font-Names="Verdana"
                            Font-Size="11px" BackColor="SeaShell" Style="margin-left: 30px; float: left;">
                            <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="true" HeaderStyle-BackColor="#397CBB"
                                HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" GridLines="Both" Width="112%"
                                Height="65%" OnRowCommand="grdDetail_RowCommand" CellPadding="5" CellSpacing="5"
                                OnRowDataBound="grdDetail_RowDataBound" Style="table-layout: fixed;">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditRecord" Text="Edit"
                                                CommandArgument="lnk"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteRecord" Text="Delete"
                                                CommandArgument="lnk"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle Height="25px" Wrap="false" ForeColor="Black" />
                            </asp:GridView>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:Panel ID="pnlPopup" onkeydown="closepopup(event);" runat="server" Width="90%"
                ScrollBars="Both" align="center" BackColor="#FFFFE4" Direction="LeftToRight"
                Style="z-index: 5000; position: absolute; display: none; float: left; max-height: 500px; min-height: 500px; box-shadow: 1px 1px 8px 2px; background-position: center; left: 5%; top: 10%;">
                <asp:ImageButton ID="imgBtnClose" runat="server" ImageUrl="~/Images/closebtn.jpg"
                    Width="20px" Height="20px" Style="float: right; vertical-align: top;" OnClick="imgBtnClose_Click"
                    ToolTip="Close" />
                <table width="95%">
                    <tr>
                        <td align="center" styles="background-color: #F5B540; width: 100%;">
                            <asp:Label ID="lblPopupHead" runat="server" Font-Size="Medium" Font-Names="verdana"
                                Font-Bold="true" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%;" align="left" colspan="2">
                            <table id="Table1" runat="server" width="100%">
                                <tr>
                                    <td style="width: 40%;">Search Text:
                                        <asp:TextBox ID="txtSearchText" onkeydown="SelectFirstRow(event);" runat="server"
                                            Width="250px" Height="20px" AutoPostBack="false" OnTextChanged="txtSearchText_TextChanged"></asp:TextBox>
                                        &nbsp;<asp:Button ID="btnSearch" onkeydown="SelectFirstRow(event);" runat="server"
                                            Text="Search" CssClass="btnSubmit" OnClick="btnSearch_Click" />
                                    </td>
                                    <td align="left" runat="server" id="tdDate" visible="false">From:
                                        <asp:TextBox ID="txtFromDate" runat="server" CssClass="txt" Width="80px" Height="24px"
                                            MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                                            Height="15px" />
                                        <ajax1:CalendarExtender ID="calendertxtFromDate" runat="server" TargetControlID="txtFromDate"
                                            PopupButtonID="Image1" Format="dd/MM/yyyy">
                                        </ajax1:CalendarExtender>
                                        To:
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="txt" Width="80px" Height="24px"
                                            MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                                        <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                                            Height="15px" />
                                        <ajax1:CalendarExtender ID="CalendarExtendertxtToDate" runat="server" TargetControlID="txtToDate"
                                            PopupButtonID="Image2" Format="dd/MM/yyyy">
                                        </ajax1:CalendarExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlInner" runat="server" Width="100%" Direction="LeftToRight" BackColor="#FFFFE4"
                                Style="z-index: 5000; float: right; overflow: auto; height: 500px">
                                <asp:GridView ID="grdPopup" Font-Bold="true" CssClass="select" runat="server" AutoGenerateColumns="true"
                                    AllowPaging="true" PageSize="15" EmptyDataText="No Records Found" HeaderStyle-BackColor="#6D8980"
                                    HeaderStyle-ForeColor="White" OnRowCreated="grdPopup_RowCreated" OnPageIndexChanging="grdPopup_PageIndexChanging"
                                    OnRowDataBound="grdPopup_RowDataBound" Style="table-layout: fixed;">
                                    <HeaderStyle Height="30px" ForeColor="White" BackColor="#6D8980" />
                                    <RowStyle Height="25px" ForeColor="Black" Wrap="false" />
                                    <PagerStyle BackColor="Tomato" ForeColor="White" Width="100%" Font-Bold="true" />
                                    <PagerSettings Position="TopAndBottom" />
                                </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <%-- <asp:Panel ID="pnlPopupTenderDetails" runat="server" BackColor="GhostWhite" Width="800px"
                BorderColor="Teal" BorderWidth="1px" Height="430px" BorderStyle="Solid" Style="z-index: 4999; left: 20%; top: 10%; position: absolute; display: none; font-weight: bold; box-shadow: 1px 2px 10px 2px;">
            </asp:Panel>--%>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
