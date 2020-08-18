<%@ Page Title="Debit Credit Note" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="pgeDebitCreditNote.aspx.cs" Inherits="pgeDebitCreditNote" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../../CSS/NewModalPopup.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Script/jquery-1.4.2.js"></script>

    <link href="../../CSS/cssCommon.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../JQuery/jquery-1.4.1.js" type="text/javascript"></script>
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <script src="../../JQuery/jquery.keynavigation.js" type="text/javascript"></script>
    <link type="text/css" href="../../menu/menu.css" rel="stylesheet" />
    <script type="text/javascript" src="../../menu/jquery.js"></script>
    <script type="text/javascript" src="../../menu/menu.js"></script>
    <script type="text/javascript" src="../../JS/DateValidation.js"> </script>

    <script src="../../JS/jquery-2.1.3.js" type="text/javascript"></script>
    <script src="../../Scripts/selectfirstrow.js" type="text/javascript"></script>

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
                if (hdnfClosePopupValue == "txtAc_Code") {
                    document.getElementById("<%=txtAc_Code.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtAc_CodeDetails") {
                    document.getElementById("<%=txtAc_CodeDetails.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtGST_Rate_Code") {
                    document.getElementById("<%=txtGST_Rate_Code.ClientID %>").focus();
                }
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";
                document.getElementById("<%= txtSearchText.ClientID %>").value = "";
            }

        });
    </script>
    <script type="text/javascript" language="javascript">
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
        function SB() {

            var billno = document.getElementById('<%=txtDoc_No.ClientID %>').value;
            var type = document.getElementById('<%=drpSub_Type.ClientID %>').value;
            var partycode = document.getElementById('<%=txtAc_Code.ClientID %>').value;


            window.open('rptDebitCreditNote.aspx?billno=' + billno + '&type=' + type + '&partycode=' + partycode + '&branchcode=' + branchcode)
        }
        function Back() {
            var tran_type = $("#<%=drpSub_Type.ClientID %>").val();
            window.open('../Transaction/PgeDebitCreditNoteUtility.aspx?tran_type=' + tran_type, "_self")

        }
    </script>
    <script type="text/javascript" language="javascript">

        var SelectedRow = null;
        var SelectedRowIndex = null;
        var UpperBound = null;
        var LowerBound = null;
        function SelectSibling(e) {
            debugger;
            var e = e ? e : window.event;
            var KeyCode = e.which ? e.which : e.keyCode;
            if (KeyCode == 40) {
                debugger;
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
                if (hdnfClosePopupValue == "txtEditDoc_No") {
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").value = "";
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtDoc_No") {
                    document.getElementById("<%= txtDoc_No.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%= lblDoc_No.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtDoc_No.ClientID %>").focus();
                }



                if (hdnfClosePopupValue == "txtAc_Code") {
                    document.getElementById("<%= txtAc_Code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%= lblAc_code.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtAc_Code.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtGST_Rate_Code") {
                    document.getElementById("<%= txtGST_Rate_Code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%= lblGST_Rate_Code.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtGST_Rate_Code.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtAc_CodeDetails") {
                    document.getElementById("<%= txtAc_CodeDetails.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%= lblAc_CodeDetails.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtAc_CodeDetails.ClientID %>").focus();
                }
            }
}
function SelectRow(CurrentRow, RowIndex) {
    debugger;
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
                e.preventDefault();
                $("#<%=btnSave.ClientID %>").focus();
            }

        }

        function AcCode(e) {
            if (e.keyCode == 112) {

                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtAc_Code.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var Accode = $("#<%=txtAc_Code.ClientID %>").val();

                Accode = "0" + Accode;
                $("#<%=txtAc_Code.ClientID %>").val(Accode);

                __doPostBack("txtAc_Code", "TextChanged");

            }

        }




        function GSTRate(e) {
            if (e.keyCode == 112) {


                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtGST_Rate_Code.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var GSTRate = $("#<%=txtGST_Rate_Code.ClientID %>").val();

                GSTRate = "0" + GSTRate;
                $("#<%=txtGST_Rate_Code.ClientID %>").val(GSTRate);
                __doPostBack("txtGST_Rate_Code", "TextChanged");

            }

        }

        function AcCodeDet(e) {
            if (e.keyCode == 112) {


                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtAc_CodeDetails.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var AcCode = $("#<%=txtAc_CodeDetails.ClientID %>").val();

                AcCode = "0" + AcCode;
                $("#<%=txtAc_CodeDetails.ClientID %>").val(AcCode);
                __doPostBack("txtAc_CodeDetails", "TextChanged");

            }
            if (e.keyCode == 13) {
                e.preventDefault();
                $("#<%=txtGST_Rate_Code.ClientID %>").focus();
            }

        }
        function DocNo(e) {
            if (e.keyCode == 112) {


                e.preventDefault();

                $("#<%=btntxtDoc_No.ClientID %>").click();

            }
            if (e.keyCode == 9) {

                __doPostBack("txtDoc_No", "TextChanged");

            }

        }

        function changeno(e) {
            debugger;
            if (e.keyCode == 112) {

                e.preventDefault();

                var edi = "txtEditDoc_No"
                $("#<%=hdnfClosePopup.ClientID %>").val(edi);
                $("#<%= btnSearch.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                __doPostBack("txtEditDoc_No", "TextChanged");

            }
        }
    </script>
    <script type="text/javascript">

        function auth() {
            window.open('../Master/pgeAuthentication.aspx', '_self');
        }
        function authenticate() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to Authenticate data?")) {
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
    <script type="text/javascript">
        function disableClick(elem) {
            elem.disabled = true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="Debit Credit Note " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfSuffix" runat="server" />
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:HiddenField ID="hdnfTran_type" runat="server" />
            <table width="80%" align="left">
                <tr>
                    <td align="center">
                        <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnAdd_Click" Height="24px" TabIndex="1" />
                        &nbsp;
                        <%--<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnHelp" Width="90px"
                            ValidationGroup="add" OnClick="btnSave_Click" Height="24px" />
                        &nbsp;--%>
                        <asp:Button OnClientClick="disableClick(this)" OnClick="btnSave_Click" runat="server"
                            Text="Save" UseSubmitBehavior="false" ID="btnSave" CssClass="btnHelp" ValidationGroup="add"
                            Width="90px" Height="24px" TabIndex="23" />
                        &nbsp;
                        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnEdit_Click" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                            ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()" Height="24px" />
                        &nbsp;
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" OnClick="btnCancel_Click" Height="24px" TabIndex="24" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btnHelp" Width="90px"
                            ValidationGroup="save" Height="24px" TabIndex="24" OnClientClick="Back();" />
                        &nbsp;
                        <asp:Button runat="server" ID="btnPrintSaleBill" Text="Print" CssClass="btnHelp"
                            Width="80px" Height="24px" OnClientClick="SB();" />
                    </td>

                </tr>
            </table>
            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table style="width: 70%;" align="left" cellpadding="4" cellspacing="4">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                Font-Size="Small" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>

                        <td align="left">
                            <asp:TextBox Height="24px" ID="txtEditDoc_No" runat="Server" CssClass="txt" TabIndex="1"
                                Width="90px" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtEditDoc_No_TextChanged"
                                onKeyDown="changeno(event);" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Type
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="drpSub_Type" runat="Server" CssClass="txt" TabIndex="1" Width="230px"
                                AutoPostBack="true" OnSelectedIndexChanged="drpSub_Type_SelectedIndexChanged"
                                Height="30px">
                                <asp:ListItem Text="Debit Credit note to Customer" Value="D1"></asp:ListItem>
                                <asp:ListItem Text="Credit Note to Customer" Value="C1"></asp:ListItem>
                                <asp:ListItem Text="Debit Note note to Supplier" Value="D2"></asp:ListItem>
                                <asp:ListItem Text="Credit Note to Supplier" Value="C2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td align="left">Entry No
                        </td>
                        <td align="left">
                            <asp:TextBox Height="24px" ID="txtDoc_No" runat="Server" CssClass="txt" TabIndex="2"
                                Width="90px" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtDoc_No_TextChanged"
                                onKeyDown="DocNo(event);"></asp:TextBox>
                            <asp:Button Width="70px" Height="24px" ID="btntxtDoc_No" runat="server" Text="..."
                                OnClick="btntxtDoc_No_Click" CssClass="btnHelp" onKeyDown="DocNo(event);" Visible="false" />
                            <asp:Label ID="lblDoc_No" runat="server" CssClass="lblName" Visible="false"></asp:Label>
                        </td>

                    </tr>
                    <tr>

                        <td align="left">Entry Date
                        </td>
                        <td align="left">
                            <asp:TextBox Height="24px" ID="txtDoc_Date" runat="Server" CssClass="txt" TabIndex="3"
                                Width="90px" Style="text-align: left;" AutoPostBack="true" OnTextChanged="txtDoc_Date_TextChanged"
                                onkeyup="ValiddateDate(this,event.keyCode)" onkeydown="retun DateFormat(this,event.keyCode)"></asp:TextBox>
                            <asp:Image ID="imgcalendertxtDoc_Date" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" /><ajax1:CalendarExtender ID="CalendarExtenderDatetxtDoc_Date"
                                    runat="server" TargetControlID="txtDoc_Date" PopupButtonID="imgcalendertxtDoc_Date"
                                    Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>
                        </td>
                        <td align="left">Ac Code
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtAc_Code" runat="Server" CssClass="txt" TabIndex="4" Width="90px"
                                Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtAc_Code_TextChanged"
                                Height="24px" MaxLength="18" onKeyDown="AcCode(event);"></asp:TextBox>
                            <asp:Button ID="btntxtAc_Code" runat="server" Text="..." OnClick="btntxtAc_Code_Click"
                                CssClass="btnHelp" Width="20px" />
                            <asp:Label ID="lblAc_code" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Label ID="lblAcId" runat="server" CssClass="lblName"></asp:Label>
                        </td>


                    </tr>
                    <tr>
                        <td align="left">Bill No
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtBillNo" runat="Server" CssClass="txt" TabIndex="5" Width="90px"
                                Style="text-align: right;" AutoPostBack="true" OnTextChanged="txtBillNo_TextChanged"
                                Height="24px" MaxLength="18"></asp:TextBox>
                        </td>
                        <td align="left">Bill Date
                        </td>
                        <td align="left">
                            <asp:TextBox Height="24px" ID="txtRef_Date" runat="Server" CssClass="txt" TabIndex="6"
                                Width="90px" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtRef_Date_TextChanged"
                                onkeyup="ValiddateDate(this,event.keyCode)" onkeydown="retun DateFormat(this,event.keyCode)"></asp:TextBox>
                            <asp:Image ID="imgcalendertxtRef_Date" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" /><ajax1:CalendarExtender ID="CalendarExtenderDatetxtRef_Date"
                                    runat="server" TargetControlID="txtRef_Date" PopupButtonID="imgcalendertxtRef_Date"
                                    Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>
                        </td>
                        <td align="left">Bill Id
                        </td>
                        <td align="left">
                            <asp:Label ID="lblBillid" runat="server" CssClass="lblName" Font-Names="verdana" Text=""></asp:Label>
                        </td>
                    </tr>




                    <tr>
                        <td align="left">
                            <asp:Button ID="btnOpenDetailsPopup" runat="server" Text="ADD" CssClass="btnHelp"
                                Width="80px" Height="25px" OnClick="btnOpenDetailsPopup_Click" Visible="false" />
                        </td>
                    </tr>
                </table>
                <fieldset style="border-top: 2px dotted rgb(249, 6, 197); border-radius: 3px; width: 90%; margin-left: -131px; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 7px;">
                    <legend style="text-align: center;">
                        <h3 style="color: Purple; font-family: verdana; font-weight: bold;">Debit Credit Note Detail</h3>
                </fieldset>
                <table width="100%" align="left">
                    <tr>

                        <td align="left">
                            <asp:Label ID="lblID" runat="server" CssClass="lblName" Font-Names="verdana" Text=""></asp:Label>
                        </td>
                        <td align="left">
                            <asp:Label ID="lblNo" runat="server" CssClass="lblName" Font-Names="verdana" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">Exp Ac Code
                            <asp:TextBox ID="txtAc_CodeDetails" runat="Server" CssClass="txt" TabIndex="7" Width="90px"
                                Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtAc_CodeDetails_TextChanged"
                                Height="24px" MaxLength="18" onKeyDown="AcCodeDet(event);"></asp:TextBox>
                            <asp:Button ID="btntxtAc_CodeDetails" runat="server" Text="..." OnClick="btntxtAc_CodeDetails_Click"
                                CssClass="btnHelp" />
                            <asp:Label ID="lblAc_CodeDetails" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Label ID="lblExAcId" runat="server" CssClass="lblName"></asp:Label>
                            Value
                            <asp:TextBox ID="txtvalue" runat="Server" CssClass="txt" TabIndex="8" Width="90px"
                                Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtvalue_TextChanged"
                                Height="24px" MaxLength="18"></asp:TextBox>
                            <asp:Button ID="btnAdddetails" runat="server" Text="ADD" Width="80px" Height="25px"
                                OnClick="btnAdddetails_Click" TabIndex="9" CssClass="btnHelp" />
                            <asp:Button ID="btnClosedetails" runat="server" Text="Close" CssClass="btnHelp" Width="80px"
                                Height="25px" OnClick="btnClosedetails_Click" TabIndex="10" />
                        </td>
                    </tr>
                </table>
                <div style="width: 100%; position: relative;">
                    <asp:UpdatePanel ID="upGrid" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlgrdDetail" runat="server" ScrollBars="Both" Height="200px" Width="1000px"
                                BorderColor="Maroon" BorderWidth="1px" BorderStyle="Solid" Font-Bold="true" Font-Names="Verdana"
                                Font-Size="11px" BackColor="SeaShell" Style="margin-left: 30px; float: left;">
                                <asp:GridView ID="grdDetail" runat="server" AutoGenerateColumns="true" HeaderStyle-BackColor="#397CBB"
                                    HeaderStyle-ForeColor="White" HeaderStyle-Height="30px" GridLines="Both" Width="100%"
                                    OnRowCommand="grdDetail_RowCommand" CellPadding="5" CellSpacing="5" OnRowDataBound="grdDetail_RowDataBound"
                                    Style="table-layout: fixed;">
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
                <asp:Panel ID="Panel1" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                    Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                    <table style="width: 74%;" align="left" cellpadding="4" cellspacing="4">
                        <tr>
                            <td style="width: 100%">
                                <table style="width: 50%;" align="left">
                                    <tr>
                                        <td align="left">GST Rate Code
                                            <asp:TextBox Height="24px" ID="txtGST_Rate_Code" runat="Server" CssClass="txt" TabIndex="11"
                                                Width="90px" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtGST_Rate_Code_TextChanged"
                                                onKeyDown="GSTRate(event);"></asp:TextBox>
                                            <asp:Button Width="20px" Height="24px" ID="btntxtGST_Rate_Code" runat="server" Text="..."
                                                OnClick="btntxtGST_Rate_Code_Click" CssClass="btnHelp" />
                                            <asp:Label ID="lblGST_Rate_Code" runat="server" CssClass="lblName"></asp:Label>

                                            <asp:TextBox Height="24px" ID="txtGST" runat="Server" CssClass="txt" TabIndex="12"
                                                Width="90px" Style="text-align: right;" AutoPostBack="true" OnTextChanged="txtGST_TextChanged"
                                                MaxLength="18" Visible="false"></asp:TextBox>

                                        </td>
                                    </tr>



                                </table>
                                <table style="width: 50%;" align="right">
                                    <tr>
                                        <td style="width: 5%;" align="right">Taxable Amount:
                                        </td>
                                        <td style="width: 5%;" align="left">
                                            <asp:TextBox Height="24px" ID="txtTaxable_Amount" runat="Server" CssClass="txt" TabIndex="13"
                                                Width="100px" Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtTaxable_Amount_TextChanged"
                                                MaxLength="19"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 5%;" align="right">CGST%
                                        </td>
                                        <td style="width: 5%;" align="left">
                                            <asp:TextBox Height="24px" ID="txtCGST_Rate" runat="Server" CssClass="txt" TabIndex="14"
                                                Width="30px" Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtCGST_Rate_TextChanged"
                                                MaxLength="18"></asp:TextBox>
                                            <asp:TextBox Height="24px" ID="txtCGST_Amount" runat="Server" CssClass="txt" TabIndex="15"
                                                Width="65px" Style="text-align: right;" AutoPostBack="true" OnTextChanged="txtCGST_Amount_TextChanged"
                                                MaxLength="18"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 5%;" align="right">SGST%
                                        </td>
                                        <td style="width: 5%;" align="left">
                                            <asp:TextBox Height="24px" ID="txtSGST_Rate" runat="Server" CssClass="txt" TabIndex="16"
                                                Width="30px" Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtSGST_Rate_TextChanged"
                                                MaxLength="18"></asp:TextBox>
                                            <asp:TextBox Height="24px" ID="txtSGST_Amount" runat="Server" CssClass="txt" TabIndex="17"
                                                Width="65px" Style="text-align: right;" AutoPostBack="true" OnTextChanged="txtSGST_Amount_TextChanged"
                                                MaxLength="18"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 5%;" align="right">IGST%
                                        </td>
                                        <td style="width: 5%;" align="left">
                                            <asp:TextBox Height="24px" ID="txtIGST_Rate" runat="Server" CssClass="txt" TabIndex="18"
                                                Width="30px" Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtIGST_Rate_TextChanged"
                                                MaxLength="18"></asp:TextBox>
                                            <asp:TextBox Height="24px" ID="txtIGST_Amount" runat="Server" CssClass="txt" TabIndex="19"
                                                Width="65px" Style="text-align: right;" AutoPostBack="true" OnTextChanged="txtIGST_Amount_TextChanged"
                                                MaxLength="18"></asp:TextBox>

                                        </td>
                                        <tr>
                                            <td style="width: 5%;" align="right">MISC%
                                            </td>
                                            <td style="width: 5%;" align="left">
                                                <asp:TextBox Height="24px" ID="txtMISC" runat="Server" CssClass="txt" TabIndex="20"
                                                    Width="80px" Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtMISC_TextChanged"
                                                    MaxLength="18"></asp:TextBox>



                                            </td>
                                        </tr>
                                        <tr>
                                            <%-- <td style="width: 5%;" align="right">
                                        Gross:
                                    </td>--%>
                                            <td style="width: 5%;" align="left">
                                                <asp:TextBox Height="24px" ID="txtGross_Value" runat="Server" CssClass="txt" TabIndex="21"
                                                    Width="100px" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtGross_Value_TextChanged"
                                                    MaxLength="18" Visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 5%;" align="right">final Amount
                                            </td>
                                            <td style="width: 5%;" align="left">
                                                <asp:TextBox Height="24px" ID="txtfinalAmount" runat="server" CssClass="txt" TabIndex="22"
                                                    Width="90px" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtfinalAmount_textchanged"
                                                    MaxLength="18"></asp:TextBox>
                                                <%--<asp:Label ID="lblFinalAmount" runat="server" CssClass="lblName" Font-Names="verdana"
                                                    Text="" Font-Size="Medium" Style="text-align: right;" Width="105px"></asp:Label>--%>
                                            </td>
                                        </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left"></td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel onkeydown="closepopup(event);" ID="pnlPopup" runat="server" Width="70%"
                align="center" ScrollBars="None" BackColor="#FFFFE4" Direction="LeftToRight"
                Style="z-index: 5000; position: absolute; display: none; float: right; max-height: 1000px; min-height: 700px; box-shadow: 1px 1px 8px 2px; background-position: center; left: 10%; top: 10%;">
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
                            <asp:TextBox onkeydown="SelectFirstRow(event);" ID="txtSearchText" runat="server"
                                Width="250px" Height="20px" AutoPostBack="false" OnTextChanged="txtSearchText_TextChanged"></asp:TextBox>
                            <asp:Button onkeydown="closepopup(event);" ID="btnSearch" runat="server" Text="Search"
                                CssClass="btnSubmit" OnClick="btnSearch_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Panel ID="pnlInner" runat="server" Width="100%" Direction="LeftToRight" BackColor="#FFFFE4"
                                Style="z-index: 5000; float: right; overflow: auto; height: 680">
                                <asp:GridView Font-Bold="true" CssClass="select" AllowPaging="true" PageSize="25"
                                    OnPageIndexChanging="grdPopup_PageIndexChanging" ID="grdPopup" runat="server"
                                    AutoGenerateColumns="true" EmptyDataText="No Records Found" HeaderStyle-BackColor="#6D8980"
                                    HeaderStyle-ForeColor="White" OnRowCreated="grdPopup_RowCreated" OnRowDataBound="grdPopup_RowDataBound"
                                    Style="table-layout: fixed;">
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
                BorderColor="Teal" BorderWidth="1px" Height="300px" BorderStyle="Solid" Style="z-index: 4999; left: 20%; top: 20%; position: absolute; display: none; font-weight: bold; box-shadow: 1px 2px 10px 2px;">
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
