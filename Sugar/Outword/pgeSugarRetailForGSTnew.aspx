<%@ Page Title="Retail Sell For GST" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="pgeSugarRetailForGSTnew.aspx.cs" Inherits="Sugar_pgeSugarRetailForGSTnew" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../CSS/NewModalPopup.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Script/jquery-1.4.2.js"></script>

    <link href="../../CSS/cssCommon.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../JQuery/jquery-1.4.1.js" type="text/javascript"></script>
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <script src="../../JQuery/jquery.keynavigation.js" type="text/javascript"></script>
    <link type="text/css" href="../../menu/menu.css" rel="stylesheet" />
    <script type="text/javascript" src="../../menu/jquery.js"></script>
    <script type="text/javascript" src="../../menu/menu.js"></script>
    <script type="text/javascript" src="../JS/DateValidation.js"> </script>

    <script src="../../JS/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="../../Scripts/selectfirstrow.js" type="text/javascript"></script>
    <script type="text/javascript" src="../JS/DateValidation.js">
    </script>

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
                if (hdnfClosePopupValue == "txtAC_CODE") {
                    document.getElementById("<%=txtAC_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtMillCode") {
                    document.getElementById("<%=txtMillCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtBroker_Code") {
                    document.getElementById("<%=txtBroker_Code.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtGSTRateCode") {
                    document.getElementById("<%=txtGSTRateCode.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtITEM_CODE") {
                    document.getElementById("<%=txtITEM_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtparty_Name_New") {
                    document.getElementById("<%=txtparty_Name_New.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtPurchaseno") {
                    document.getElementById("<%=btntxtPurchaseno.ClientID %>").focus();
                }
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";
            }

        });
    </script>
    <script type="text/javascript">
        function SB() {
            var billno = document.getElementById('<%=txtDOC_NO.ClientID %>').value;
            var type = document.getElementById('<%=drpCashCredit.ClientID %>').value;
            window.open('../Report/rptRetailSellPrintForGST.aspx?billno=' + billno + '&type=' + type);
        }
        function CRP() {
            var billno = document.getElementById('<%=txtDOC_NO.ClientID %>').value;
            var type = document.getElementById('<%=drpCashCredit.ClientID %>').value;
            window.open('../Report/rptCashRecivePrint.aspx?billno=' + billno + '&type=' + type);
        }
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

        function Confirm_print() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to print data?")) {
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
        debugger;
        function SelectSibling(e) {
            debugger;
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
                if (hdnfClosePopupValue == "txtAC_CODE") {
                    document.getElementById("<%=txtAC_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=LblPartyname.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtAC_CODE.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtMillCode") {

                    document.getElementById("<%=txtMillCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblMillName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    // document.getElementById("<%= hdnfDoNo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[3].innerText;
                    document.getElementById("<%=txtMillCode.ClientID %>").focus();


                }
                if (hdnfClosePopupValue == "txtPurchaseno") {


                    document.getElementById("<%=txtPurchaseno.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblpurchaseno.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[7].innerText;
                    document.getElementById("<%= hdnfDoNo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%= hdconfirm.ClientID %>").value = "Close";
                    document.getElementById("<%=btntxtPurchaseno.ClientID %>").focus();

                }
                if (hdnfClosePopupValue == "txtBroker_Code") {

                    document.getElementById("<%=txtBroker_Code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblbrokerName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;

                    document.getElementById("<%=txtBroker_Code.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtDOC_NO") {

                    document.getElementById("<%=txtDOC_NO.ClientID %>").disabled = true;
                    document.getElementById("<%=txtDOC_NO.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                }
                if (hdnfClosePopupValue == "txtITEM_CODE") {
                    document.getElementById("<%=txtITEM_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=LBLITEMNAME.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtITEM_CODE.ClientID %>").focus();

                }
                if (hdnfClosePopupValue == "txtGSTRateCode") {
                    document.getElementById("<%=txtGSTRateCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblGSTRateName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtGSTRateCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtparty_Name") {

                    document.getElementById("<%=txtparty_Name.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblCashParty_Name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;

                    document.getElementById("<%=txtparty_Name.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtparty_Name_New") {

                    document.getElementById("<%=txtparty_Name_New.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblParty_Name_New.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtparty_Name_New.ClientID %>").focus();
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
    <script type="text/javascript" language="javascript">

        function Focusbtn(e) {
            debugger;
            if (e.keyCode == 13) {
                debugger;
                e.preventDefault();
                $("#<%=btnSave.ClientID %>").focus();

            }
            else if (e.keyCode == 27) {
                e.preventDefault();
                $("#<%=drpDelivered.ClientID %>").focus();
            }

    }

    function ac_name(e) {
        if (e.keyCode == 112) {
            debugger;
            e.preventDefault();
            $("#<%=pnlPopup.ClientID %>").show();
            $("#<%=btntxtAC_CODE.ClientID %>").click();

        }
        if (e.keyCode == 9) {
            e.preventDefault();
            var unit = $("#<%=txtAC_CODE.ClientID %>").val();

            unit = "0" + unit;
            $("#<%=txtAC_CODE.ClientID %>").val(unit);
            __doPostBack("txtAC_CODE", "TextChanged");

        }
    }



    function ac_name_Part_Code(e) {
        if (e.keyCode == 112) {
            debugger;
            e.preventDefault();
            $("#<%=pnlPopup.ClientID %>").show();
            $("#<%=btntxtParty_Name.ClientID %>").click();

        }
        if (e.keyCode == 9) {
            e.preventDefault();
            var unit = $("#<%=txtparty_Name_New.ClientID %>").val();

            unit = "0" + unit;
            $("#<%=txtparty_Name_New.ClientID %>").val(unit);
            __doPostBack("txtparty_Name_New", "TextChanged");

        }
    }

    function GstRateCode(e) {
        if (e.keyCode == 112) {
            debugger;
            e.preventDefault();
            $("#<%=pnlPopup.ClientID %>").show();
            $("#<%=btntxtGSTRateCode.ClientID %>").click();

        }
        if (e.keyCode == 9) {
            e.preventDefault();
            var unit = $("#<%=txtGSTRateCode.ClientID %>").val();

            unit = "0" + unit;
            $("#<%=txtGSTRateCode.ClientID %>").val(unit);
            __doPostBack("txtGSTRateCode", "TextChanged");

        }
    }
    function Broker_code(e) {
        if (e.keyCode == 112) {
            debugger;
            e.preventDefault();
            $("#<%=pnlPopup.ClientID %>").show();
            $("#<%=btntxtBroker_Code.ClientID %>").click();

        }
        if (e.keyCode == 9) {
            e.preventDefault();
            var unit = $("#<%=txtBroker_Code.ClientID %>").val();

            unit = "0" + unit;
            $("#<%=txtBroker_Code.ClientID %>").val(unit);
            __doPostBack("txtBroker_Code", "TextChanged");

        }
    }
    function mill_code(e) {
        if (e.keyCode == 112) {
            debugger;
            e.preventDefault();
            $("#<%=pnlPopup.ClientID %>").show();
            $("#<%=btntxtMillCode.ClientID %>").click();

        }
        if (e.keyCode == 9) {
            e.preventDefault();
            var unit = $("#<%=txtMillCode.ClientID %>").val();

            unit = "0" + unit;
            $("#<%=txtMillCode.ClientID %>").val(unit);
            __doPostBack("txtMillCode", "TextChanged");

        }

        if (e.keyCode == 13) {
            e.preventDefault();
            $("#<%=txtDUE_DAYS.ClientID %>").focus();

        }
    }
    function purchesId(e) {
        if (e.keyCode == 112) {
            debugger;
            e.preventDefault();
            $("#<%=pnlPopup.ClientID %>").show();
            $("#<%=btntxtPurchaseno.ClientID %>").click();

        }
        if (e.keyCode == 9) {
            e.preventDefault();
            var unit = $("#<%=txtPurchaseno.ClientID %>").val();


            __doPostBack("txtPurchaseno", "TextChanged");

        }
    }
    function item_code(e) {
        if (e.keyCode == 112) {
            debugger;
            e.preventDefault();
            $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtITEM_CODE.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtITEM_CODE.ClientID %>").val();

                  unit = "0" + unit;
                  $("#<%=txtITEM_CODE.ClientID %>").val(unit);
                __doPostBack("txtITEM_CODE", "TextChanged");

            }

        }
        function save(e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                $("#<%=btnSave.ClientID %>").focus();
            }
        }
        function Back() {

            //alert(td);
            window.open('../Outword/PgeRetailSellUtility.aspx', '_self');
        }
    </script>
    <%--  <script type="text/javascript" language="javascript">
        $(document).keyup(function (e) {
            if (e.keyCode === 13) $('.save').click();     // enter
            if (e.keyCode === 27) $('.cancel').click();   // esc
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Retail Sell   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfSuffix" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:HiddenField ID="hdnfDoNo" runat="server" />
            <asp:HiddenField ID="hdnfdrpVal" runat="server" />
            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 0px; margin-top: 0px; z-index: 100;">
                <table width="80%" align="center">
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                                Height="24px" ValidationGroup="save" OnClick="btnAdd_Click" />
                            &nbsp;
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnHelp" Width="90px"
                            Height="24px" ValidationGroup="add" OnClick="btnSave_Click" TabIndex="24" OnClientClick="Confirm_print()" />
                            &nbsp;
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                            Height="24px" ValidationGroup="save" OnClick="btnEdit_Click" TabIndex="25" />
                            &nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                            Height="24px" ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()"
                            TabIndex="26" />
                            &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                            Height="24px" ValidationGroup="save" OnClick="btnCancel_Click" />&nbsp;

                             <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btnHelp" Width="90px"
                                 TabIndex="38" Height="24px" ValidationGroup="save" OnClientClick="Back()" />&nbsp;
                        <asp:Button runat="server" ID="btnPrintSaleBill" Text="Print" Visible="true" CssClass="btnHelp"
                            Width="80px" Height="24px" OnClientClick="SB();" TabIndex="20" />
                            <asp:Button runat="server" ID="btncashRecivePrint" Text=" Cash Recive Print" Visible="true"
                                CssClass="btnHelp" Width="120px" Height="24px" OnClientClick="CRP();" TabIndex="21" />
                        </td>
                        <td align="center">&nbsp;<asp:Button ID="btnFirst" runat="server" Text="<<" ToolTip="First" CssClass="btnHelp"
                            OnClick="btnFirst_Click" Width="90px" Height="24px" Visible="false" />
                            &nbsp;<asp:Button ID="btnPrevious" runat="server" Text="<" ToolTip="First" CssClass="btnHelp"
                                OnClick="btnPrevious_Click" Width="90px" Height="24px" Visible="false" />
                            &nbsp;<asp:Button ID="btnNext" runat="server" Text=">" ToolTip="First" CssClass="btnHelp"
                                OnClick="btnNext_Click" Width="90px" Height="24px" Visible="false" />
                            &nbsp;<asp:Button ID="btnLast" runat="server" Text=">>" ToolTip="First" CssClass="btnHelp"
                                OnClick="btnLast_Click" Width="90px" Height="24px" Visible="false" />
                        </td>
                    </tr>
                </table>


                <table width="90%" align="left" cellspacing="5">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>

                        <td>
                            <asp:TextBox runat="server" ID="txtEditDoc_No" CssClass="txt" Width="100px" Height="24px"
                                TabIndex="0" AutoPostBack="false" OnTextChanged="txtEditDoc_No_TextChanged" Visible="false"></asp:TextBox>
                            <%--<ajax1:FilteredTextBoxExtender runat="server" ID="txtxf" FilterType="Numbers" InvalidChars="."
                                TargetControlID="txtEditDoc_No">
                            </ajax1:FilteredTextBoxExtender>--%>
                            &nbsp;<asp:Label ID="Label2" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                        <td align="center" style="width: 10%;">Cash / Credit:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="drpCashCredit" Width="100px" CssClass="ddl"
                                AutoPostBack="true" TabIndex="1" OnSelectedIndexChanged="drpCashCredit_SelectedIndexChanged">

                                <asp:ListItem Text="Credit" Value="CR"></asp:ListItem>
                                <asp:ListItem Text="Cash" Value="CS"></asp:ListItem>

                                <%--   <asp:ListItem Text="Cash" Value="CH" />--%>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">To:
                        </td>
                        <td align="left" style="width: 480px;">
                            <asp:TextBox ID="txtAC_CODE" runat="Server" CssClass="txt" TabIndex="2" Width="80px"
                                Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtAC_CODE_TextChanged"
                                Height="24px" onkeydown="ac_name(event);"></asp:TextBox>
                            <asp:Button ID="btntxtAC_CODE" runat="server" Text="..." OnClick="btntxtAC_CODE_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="LblPartyname" runat="server" CssClass="lblName"></asp:Label>
                            <%--  Purc No:<asp:TextBox ID="txtPURCNO" runat="Server" CssClass="txt" TabIndex="1" Width="80px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtPURCNO_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtPURCNO" runat="server" Text="..." OnClick="btntxtPURCNO_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                &nbsp;DO:<asp:Label runat="server" ID="lblDONo" ForeColor="Yellow"></asp:Label>--%>
                        </td>
                        <td align="right" style="width: 10%;">Bill No.:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtDOC_NO" runat="Server" CssClass="txt" TabIndex="3" Width="80px"
                                Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtDOC_NO_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Label ID="lblDoc_Id" runat="server" Text="" Font-Names="verdana"
                                ForeColor="Blue" Font-Bold="true" Font-Size="12px" Visible="false"></asp:Label>
                            <asp:Button ID="btntxtdoc_no" runat="server" Text="..." Width="80px" OnClick="btntxtDOC_NO_Click"
                                CssClass="btnHelp" Height="24px" Visible="false" />
                        </td>
                        <td align="right" style="width: 10%;">Bill Date:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtDOC_DATE" runat="Server" CssClass="txt" TabIndex="4" Width="100px"
                                MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtDOC_DATE_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtDOC_DATE"
                                PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">Party Name:
                        </td>
                        <td align="left" style="width: 10px;">
                            <asp:TextBox ID="txtparty_Name" runat="Server" CssClass="txt" TabIndex="5" Width="200px"
                                Style="text-align: left;" Height="24px" OnTextChanged="txtparty_Name_TextChanged"
                                AutoPostBack="false"></asp:TextBox>
                            <%--<asp:Button ID="btntxtParty_Name" runat="server" Text="..." OnClick="btntxtParty_Name_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />--%>
                            <asp:Label ID="lblCashParty_Name" runat="server" CssClass="lblName" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">Party Name New:
                        </td>
                        <td align="left" style="width: 10px;">
                            <asp:TextBox ID="txtparty_Name_New" runat="Server" CssClass="txt" TabIndex="6" Width="80px"
                                Style="text-align: left;" Height="24px" OnTextChanged="txtparty_Name_New_TextChanged"
                                AutoPostBack="false" onkeydown="ac_name_Part_Code(event);"></asp:TextBox>
                            <asp:Button ID="btntxtParty_Name" runat="server" Text="..." OnClick="btntxtParty_Name_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblParty_Name_New" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">Address:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:Label Text="" runat="server" ID="lblPartyAddress" CssClass="lblName" />
                        </td>
                        <td align="right" style="width: 10%;">Challan No.:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtChallanNo" runat="Server" CssClass="txt" Width="120px" Style="text-align: right;"
                                TabIndex="7" AutoPostBack="false" Height="24px" OnTextChanged="txtChallanNo_TextChanged"></asp:TextBox>
                        </td>
                        <td align="right" style="width: 10%;">Challan Date:
                            <asp:TextBox ID="txtChallanDate" runat="Server" CssClass="txt" Width="100px" MaxLength="10"
                                TabIndex="8" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtChallanDate_TextChanged"
                                Height="24px"></asp:TextBox>
                            <asp:Image ID="imgcalender2" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" />
                            <ajax1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtChallanDate"
                                PopupButtonID="imgcalender2" Format="dd/MM/yyyy">
                            </ajax1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Lorry No:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtLORRYNO" runat="Server" CssClass="txt" Width="200px" Style="text-align: left; text-transform: uppercase;"
                                AutoPostBack="false" OnTextChanged="txtLORRYNO_TextChanged"
                                Height="24px" TabIndex="9"></asp:TextBox>
                        </td>
                        <td align="right">GST Rate Code
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtGSTRateCode" runat="Server" CssClass="txt" Width="80px" Style="text-align: left;"
                                TabIndex="10" AutoPostBack="false" onkeydown="GstRateCode(event);" OnTextChanged="txtGSTRateCode_TextChanged" Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtGSTRateCode" runat="server" Text="..." OnClick="btntxtGSTRateCode_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblGSTRateName" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 10%;">Broker Code:
                        </td>
                        <td align="left" style="width: 480px;">
                            <asp:TextBox ID="txtBroker_Code" runat="Server" CssClass="txt" Width="80px" Style="text-align: right;"
                                AutoPostBack="false" OnTextChanged="txtBroker_Code_TextChanged" Height="24px" onkeyDown="Broker_code(event);" TabIndex="11"></asp:TextBox>
                            <asp:Button ID="btntxtBroker_Code" runat="server" Text="..." OnClick="btntxtBroker_Code_Click"
                                CssClass="btnHelp" Height="24px" Width="20px" />
                            <asp:Label ID="lblbrokerName" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Label ID="lblbroker_id" runat="server" CssClass="lblName"></asp:Label>

                            <%--  Purc No:<asp:TextBox ID="txtPURCNO" runat="Server" CssClass="txt" TabIndex="1" Width="80px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtPURCNO_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtPURCNO" runat="server" Text="..." OnClick="btntxtPURCNO_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                        ++++++++++++++++++++++++++++++++++++++++++        &nbsp;DO:<asp:Label runat="server" ID="lblDONo" ForeColor="Yellow"></asp:Label>--%>
                        </td>
                        <td align="right" style="width: 10%; vertical-align: top;">Cash Recive:
                        </td>
                        <td align="left" style="width: 10%; vertical-align: top;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkcashrecive" runat="server"
                            Text="" />
                        </td>
                        <td>Narration  
                            <asp:TextBox ID="txtnarration" runat="Server" CssClass="txt" Width="250px" Style="text-align: left;"
                                AutoPostBack="True" OnTextChanged="txtnarration_TextChanged"
                                Height="34px" TabIndex="12"></asp:TextBox>
                            <%-- Narration:<asp:TextBox ID="txtnarration" runat="Server" AutoPostBack="True" CssClass="txt" Height="35px" OnTextChanged="txtnarration_TextChanged" Style="text-align: right;" TabIndex="12" Width="300px"></asp:TextBox>--%>
                        </td>
                    </tr>
                    </tr>
                    <tr style="visibility: hidden;">

                        <td align="center" style="width: 100px; vertical-align: top;">
                            <asp:Button ID="btnOpenDetailsPopup" runat="server" CssClass="btnHelp" Height="24px" OnClick="btnOpenDetailsPopup_Click" Text="ADD" Width="80px" />
                        </td>
                    </tr>
                    <tr align="center">
                        <td align="center" style="text-decoration: underline; margin-left: 50px; color: White;">Item Details </td>
                        <td>
                            <asp:Label ID="lblID" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblNo" runat="server" ForeColor="Azure" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <table align="center" cellspacing="5" style="margin-left: 100px;" width="70%">
                                <tr>
                                    <td align="center" style="width: 5%;">Mill: </td>
                                    <td align="left" colspan="8" style="width: 10%;">
                                        <asp:TextBox ID="txtMillCode" runat="Server" AutoPostBack="false" CssClass="txt" Height="24px" onKeyDown="mill_code(event);" OnTextChanged="txtMillCode_TextChanged" Style="text-align: right;" TabIndex="13" Width="80px"></asp:TextBox>
                                        <asp:Button ID="btntxtMillCode" runat="server" CssClass="btnHelp" Height="24px" OnClick="btntxtMillCode_Click" Text="..." Width="20px" />
                                        <asp:Label ID="lblMillName" runat="server" CssClass="lblName"></asp:Label>
                                        <asp:Label ID="lblgrade" runat="server" CssClass="lblName"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="width: 5%;">Purchase </td>
                                    <td align="left" colspan="8" style="width: 10%;">
                                        <asp:TextBox ID="txtPurchaseno" runat="Server" AutoPostBack="false" CssClass="txt" Height="24px" onKeyDown="purchesId(event);" OnTextChanged="txtPurchaseno_TextChanged" Style="text-align: right;" Width="80px"></asp:TextBox>
                                        <asp:Button ID="btntxtPurchaseno" runat="server" CssClass="btnHelp" Height="24px" OnClick="btntxtPurchaseno_Click" Text="..." Width="20px" TabIndex="14" />
                                        <asp:Label ID="lblpurchaseno" runat="server" CssClass="lblName"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="width: 10%;">Item: </td>
                                    <td align="left" colspan="3" style="width: 10%;">
                                        <asp:TextBox ID="txtITEM_CODE" runat="Server" AutoPostBack="false" CssClass="txt" Height="24px" onkeyDown="item_code(event);" OnTextChanged="txtITEM_CODE_TextChanged" Style="text-align: right;" TabIndex="15" Width="80px"></asp:TextBox>
                                        <asp:Button ID="btntxtITEM_CODE" runat="server" CssClass="btnHelp" Height="24px" OnClick="btntxtITEM_CODE_Click" Text="..." Width="20px" />
                                        <asp:Label ID="LBLITEMNAME" runat="server" CssClass="lblName"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="width: 10%;">Qty: </td>
                                    <td align="left" style="width: 10%;">
                                        <asp:TextBox ID="txtQUANTAL" runat="Server" AutoPostBack="True" CssClass="txt" Height="24px" OnTextChanged="txtQUANTAL_TextChanged" Style="text-align: right;" TabIndex="16" Width="80px"></asp:TextBox>
                                        <ajax1:FilteredTextBoxExtender ID="FilteredtxtQUANTAL" runat="server" FilterType="Numbers,Custom" TargetControlID="txtQUANTAL" ValidChars=".">
                                        </ajax1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="center" style="width: 10%;">Rate: </td>
                                    <td align="left" style="width: 5%;">
                                        <asp:TextBox ID="txtRATE" runat="Server" AutoPostBack="True" CssClass="txt" Height="24px" OnTextChanged="txtRATE_TextChanged" Style="text-align: right;" TabIndex="17" Width="80px"></asp:TextBox>
                                        <ajax1:FilteredTextBoxExtender ID="FilteredtxtRATE" runat="server" FilterType="Numbers,Custom" TargetControlID="txtRATE" ValidChars=".">
                                        </ajax1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="center" style="width: 1%;">Value: </td>
                                    <td align="left" style="width: 5%;">
                                        <asp:TextBox ID="txtITEMAMOUNT" runat="Server" AutoPostBack="True" CssClass="txt" Height="24px" OnTextChanged="txtITEMAMOUNT_TextChanged" ReadOnly="true" Style="text-align: right;" TabIndex="18" Width="80px"></asp:TextBox>
                                    </td>
                                    <td align="center" style="width: 10%;">Gross: </td>
                                    <td align="left" style="width: 10%;">
                                        <asp:TextBox ID="txtGross" runat="Server" AutoPostBack="True" CssClass="txt" Height="24px" ReadOnly="true" Style="text-align: left;" TabIndex="19" Width="80px"></asp:TextBox>
                                    </td>
                                    <td align="center" style="">
                                        <asp:Button ID="btnAdddetails" runat="server" CssClass="btnHelp" Height="25px" OnClick="btnAdddetails_Click" TabIndex="20" Text="ADD" Width="80px" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClosedetails" runat="server" CssClass="btnHelp" Height="25px" OnClick="btnClosedetails_Click" TabIndex="21" Text="Reset" Width="80px" />
                                    </td>
                                    <td align="left" style="width: 10%; visibility: hidden;">Billing No: </td>
                                    <td align="left" style="width: 10%; visibility: hidden;">
                                        <asp:TextBox ID="txtBillingNo" runat="Server" AutoPostBack="false" CssClass="txt" Height="24px" OnTextChanged="txtBillingNo_TextChanged" Style="text-align: right;" Width="80px"></asp:TextBox>
                                        <ajax1:FilteredTextBoxExtender ID="FilteredtxtPACKING" runat="server" FilterType="Numbers" TargetControlID="txtBillingNo">
                                        </ajax1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4" style="width: 100%; margin-left: 50px;">
                            <div style="width: 100%; position: relative; vertical-align: top; margin-top: -20px;">
                                <asp:UpdatePanel ID="upGrid" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlgrdDetail" runat="server" align="left" BackColor="SeaShell" BorderColor="Maroon" BorderStyle="Solid" BorderWidth="1px" Font-Bold="true" Font-Names="Verdana" Font-Size="11px" Height="200px" ScrollBars="Both" Style="margin-left: 30px; float: left;" Width="1100px">
                                            <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="true" CellPadding="5" CellSpacing="5" GridLines="Both" HeaderStyle-BackColor="#397CBB" HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" OnRowCommand="grdDetail_RowCommand" OnRowDataBound="grdDetail_RowDataBound" Style="table-layout: fixed; float: left" Width="100%">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument="lnk" CommandName="EditRecord" Text="Edit"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument="lnk" CommandName="DeleteRecord" Text="Delete"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle ForeColor="Black" Height="25px" Wrap="false" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                        <td align="left" style="width: 100%;">
                            <table width="100%">
                                <tr>
                                    <td align="left" style="width: 40%;">Total: </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtTotal" runat="Server" AutoPostBack="True" CssClass="txt" Height="24px" OnTextChanged="txtTotal_TextChanged" ReadOnly="true" Style="text-align: right;" Width="120px"></asp:TextBox>
                                        <ajax1:FilteredTextBoxExtender ID="FilteredtxtSUBTOTAL" runat="server" FilterType="Numbers,Custom" TargetControlID="txtTotal" ValidChars=".">
                                        </ajax1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>CGST% </td>
                                    <td style="">
                                        <asp:TextBox ID="txtCGSTRate" runat="Server" AutoPostBack="true" CssClass="txt" Height="24px" OnTextChanged="txtCGSTRate_TextChanged" Style="text-align: right;" Width="40px"></asp:TextBox>
                                        <ajax1:FilteredTextBoxExtender ID="FilteredtxtCGSTRate" runat="server" FilterType="Numbers,Custom" TargetControlID="txtCGSTRate" ValidChars=".">
                                        </ajax1:FilteredTextBoxExtender>
                                        <asp:TextBox ID="txtCGSTAmount" runat="Server" CssClass="txt" Height="24px" ReadOnly="true" Style="text-align: right;" Width="75px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="">SGST% </td>
                                    <td style="">
                                        <asp:TextBox ID="txtSGSTRate" runat="Server" AutoPostBack="true" CssClass="txt" Height="24px" OnTextChanged="txtSGSTRate_TextChanged" Style="text-align: right;" Width="40px"></asp:TextBox>
                                        <ajax1:FilteredTextBoxExtender ID="FilteredtxtSGSTRate" runat="server" FilterType="Numbers,Custom" TargetControlID="txtSGSTRate" ValidChars=".">
                                        </ajax1:FilteredTextBoxExtender>
                                        <asp:TextBox ID="txtSGSTAmount" runat="Server" CssClass="txt" Height="24px" ReadOnly="true" Style="text-align: right;" Width="75px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="">IGST% </td>
                                    <td style="">
                                        <asp:TextBox ID="txtIGSTRate" runat="Server" AutoPostBack="true" CssClass="txt" Height="24px" OnTextChanged="txtIGSTRate_TextChanged" Style="text-align: right;" Width="40px"></asp:TextBox>
                                        <ajax1:FilteredTextBoxExtender ID="FilteredtxtIGSTRate" runat="server" FilterType="Numbers,Custom" TargetControlID="txtIGSTRate" ValidChars=".">
                                        </ajax1:FilteredTextBoxExtender>
                                        <asp:TextBox ID="txtIGSTAmount" runat="Server" CssClass="txt" Height="24px" ReadOnly="true" Style="text-align: right;" Width="75px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">Vat@: </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtVatAmount" runat="Server" AutoPostBack="True" CssClass="txt" Height="24px" OnTextChanged="txtVatAmount_TextChanged" ReadOnly="true" Style="text-align: right;" Width="120px"></asp:TextBox>
                                        <ajax1:FilteredTextBoxExtender ID="FilteredtxtFREIGHT" runat="server" FilterType="Numbers,Custom" TargetControlID="txtVatAmount" ValidChars=".">
                                        </ajax1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">Sub-Total: </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtSubtotal" runat="Server" AutoPostBack="True" CssClass="txt" Height="24px" OnTextChanged="txtSubtotal_TextChanged" Style="text-align: right;" Width="120px"></asp:TextBox>
                                        <ajax1:FilteredTextBoxExtender ID="FilteredtxtOTHER_AMT" runat="server" FilterType="Numbers,Custom" TargetControlID="txtSubtotal" ValidChars=".">
                                        </ajax1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">Round Off: </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtRoundOff" runat="Server" AutoPostBack="True" CssClass="txt" Height="24px" OnTextChanged="txtRoundOff_TextChanged" Style="text-align: right;" Width="120px"></asp:TextBox>
                                        <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers,Custom" TargetControlID="txtRoundOff" ValidChars=".,-">
                                        </ajax1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">Hamali Amount: </td>
                                    <td align="left">
                                        <asp:TextBox ID="txthamliAmount" runat="Server" AutoPostBack="True" CssClass="txt" Height="24px" OnTextChanged="txthamliAmount_TextChanged" Style="text-align: right;" Width="120px"></asp:TextBox>
                                        <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers,Custom" TargetControlID="txthamliAmount" ValidChars=".,-">
                                        </ajax1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">Grand Total: </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtBILL_AMOUNT" runat="Server" AutoPostBack="True" CssClass="txt" Height="24px" OnTextChanged="txtBILL_AMOUNT_TextChanged" ReadOnly="true" Style="text-align: right;" Width="120px"></asp:TextBox>
                                        <ajax1:FilteredTextBoxExtender ID="FilteredtxtBILL_AMOUNT" runat="server" FilterType="Numbers,Custom" TargetControlID="txtBILL_AMOUNT" ValidChars=".">
                                        </ajax1:FilteredTextBoxExtender>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2" style="width: 100%;">
                            <table cellpadding="3" cellspacing="4" style="margin-top: 8px;" width="100%">
                                <tr>
                                    <td align="right">Due Days:
                                        <asp:TextBox ID="txtDUE_DAYS" runat="Server" AutoPostBack="True" CssClass="txt" Height="24px" onKeyDown="save(event);" OnTextChanged="txtDUE_DAYS_TextChanged" Style="text-align: right;" TabIndex="21 " Width="120px"></asp:TextBox>
                                        <ajax1:FilteredTextBoxExtender ID="filtertxtDUE_DAYS" runat="server" FilterType="Numbers" TargetControlID="txtDUE_DAYS">
                                        </ajax1:FilteredTextBoxExtender>
                                    </td>
                                    <td align="left" style="width: 10%;">Due Date:&nbsp;
                                        <asp:TextBox ID="txtDueDate" runat="Server" AutoPostBack="True" CssClass="txt" Height="24px" MaxLength="10" onkeydown="return DateFormat(this,event.keyCode)" onkeyup="ValidateDate(this,event.keyCode)" OnTextChanged="txtDueDate_TextChanged" Style="text-align: left;" TabIndex="22" Width="100px"></asp:TextBox>
                                        <asp:Image ID="imgDueCalendar" runat="server" Height="15px" ImageUrl="~/Images/calendar_icon1.png" Width="25px" />
                                        <ajax1:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgDueCalendar" TargetControlID="txtDueDate">
                                        </ajax1:CalendarExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="padding-left: 97px;">Delivered:
                                        <asp:DropDownList ID="drpDelivered" runat="server" AutoPostBack="true" CssClass="ddl" OnSelectedIndexChanged="drpDelivered_SelectedIndexChanged" TabIndex="23" Width="100px">
                                            <asp:ListItem Selected="True" Text="Select" />
                                            <asp:ListItem Text="YES" Value="1" />
                                            <asp:ListItem Text="NO" Value="0" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%; visibility: hidden;">Vat_Ac:
                                        <asp:TextBox ID="txtVatAc" runat="Server" AutoPostBack="True" CssClass="txt" Height="24px" OnTextChanged="txtVatAc_TextChanged" Style="text-align: right;" TabIndex="24" Width="80px"></asp:TextBox>
                                        <asp:Button ID="btntxtVatAc" runat="server" CssClass="btnHelp" Height="24px" OnClick="btntxtVatAc_Click" Text="..." Width="20px" />
                                        <asp:Label ID="lblVatAcName" runat="server" CssClass="lblName"></asp:Label>
                                        Vat %:
                                        <asp:TextBox ID="txtVatPercent" runat="Server" AutoPostBack="True" CssClass="txt" Height="24px" ReadOnly="true" Style="text-align: left;" TabIndex="25" Width="80px"></asp:TextBox>
                                        Vat Amount:
                                        <asp:TextBox ID="txtVatTotal" runat="Server" AutoPostBack="True" CssClass="txt" Height="24px" OnTextChanged="txtVatTotal_TextChanged" Style="text-align: left;" TabIndex="26" Width="80px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Panel ID="pnlPopup" onkeydown="closepopup(event);" runat="server" Width="70%"
                align="center" ScrollBars="None" BackColor="#FFFFE4" Direction="LeftToRight"
                Style="z-index: 5000; position: absolute; display: none; float: right; max-height: 500px; min-height: 500px; box-shadow: 1px 1px 8px 2px; background-position: center; left: 10%; top: 10%;">
                <asp:ImageButton ID="imgBtnClose" runat="server" ImageUrl="~/Images/closebtn.jpg"
                    Width="20px" Height="20px" Style="float: right; vertical-align: top;" OnClick="imgBtnClose_Click"
                    ToolTip="Close" />
                <table width="95%">
                    <tr>
                        <td align="center" style="background-color: #F5B540; width: 100%;">
                            <asp:Label ID="lblPopupHead" runat="server" Font-Size="Medium" Font-Names="verdana"
                                Font-Bold="true" ForeColor="White"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Search Text:
                            <asp:TextBox ID="txtSearchText" onkeydown="SelectFirstRow(event);" runat="server"
                                Width="250px" Height="20px" AutoPostBack="true" OnTextChanged="txtSearchText_TextChanged"></asp:TextBox>
                            <asp:Button ID="btnSearch" onkeydown="SelectFirstRow(event);" runat="server" Text="Search"
                                CssClass="btnSubmit" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlInner" runat="server" Width="100%" Direction="LeftToRight" BackColor="#FFFFE4"
                                Style="z-index: 5000; float: right; overflow: auto; height: 400px">
                                <asp:GridView ID="grdPopup" Font-Bold="true" CssClass="select" runat="server" AutoGenerateColumns="true"
                                    AllowPaging="true" PageSize="20" EmptyDataText="No Records Found" HeaderStyle-BackColor="#6D8980"
                                    HeaderStyle-ForeColor="White" OnPageIndexChanging="grdPopup_PageIndexChanging"
                                    OnRowCreated="grdPopup_RowCreated" OnRowDataBound="grdPopup_RowDataBound" Style="table-layout: fixed;">
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
            <asp:Panel ID="pnlPopupDetails" runat="server" BackColor="GhostWhite" Width="955px"
                BorderColor="Teal" BorderWidth="1px" Height="420px" BorderStyle="Solid" Style="z-index: 4999; left: 20%; top: 10%; position: absolute; display: none; font-weight: bold; box-shadow: 1px 2px 10px 2px;">
                <table width="80%" align="center" cellspacing="5">
                    <tr>
                        <td colspan="2" align="center" style="background-color: lightslategrey; color: White;">
                            <asp:Label ID="lblTenderDetails" runat="server" Font-Size="Medium" Font-Names="verdana"
                                Text="Item Details"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
