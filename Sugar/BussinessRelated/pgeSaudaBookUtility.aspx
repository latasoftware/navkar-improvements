<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="pgeSaudaBookUtility.aspx.cs" Inherits="Sugar_BussinessRelated_pgeSaudaBookUtility"
    ValidateRequest="false" %>

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
                if (hdnfClosePopupValue == "txtmillcode") {
                    document.getElementById("<%=txtmillcode.ClientID %>").focus();
                }
                document.getElementById("<%=txtSearchText.ClientID %>").value = "";
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "Close";
            }

        });
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
                if (hdnfClosePopupValue == "txtmillcode") {
                    document.getElementById("<%=txtmillcode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[8].innerText;
                    document.getElementById("<%=lblmillname.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txttender.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblselfqty.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[6].innerText;
                    document.getElementById("<%=lblgrade.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[5].innerText;
                    document.getElementById("<%=lbllifting.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[2].innerText;
                    document.getElementById("<%=txtqntl.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[6].innerText;
                    document.getElementById("<%=hdnfpopup.ClientID %>").value = "0";
                    document.getElementById("<%=txtmillcode.ClientID %>").focus();

                }

                if (hdnfClosePopupValue == "txtparty") {
                    document.getElementById("<%=txtparty.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblpartyname.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=hdnfpopup.ClientID %>").value = "0";
                    document.getElementById("<%=txtparty.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtbroker") {
                    document.getElementById("<%=txtbroker.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblbrokername.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=hdnfpopup.ClientID %>").value = "0";
                    document.getElementById("<%=txtbroker.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtsubbroker") {
                    document.getElementById("<%=txtsubbroker.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblsubbroker.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=hdnfpopup.ClientID %>").value = "0";
                    document.getElementById("<%=txtsubbroker.ClientID %>").focus();
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


function sendingaccode(e) {
    debugger;
    if (e.keyCode == 112) {
        debugger;
        e.preventDefault();
        $("#<%=pnlPopup.ClientID %>").show();

        $("#<%=btntxtmillcode.ClientID %>").click();

    }
    if (e.keyCode == 9) {
        e.preventDefault();
        var unit = $("#<%=txtmillcode.ClientID %>").val();

        unit = "0" + unit;
        $("#<%=txtmillcode.ClientID %>").val(unit);
        __doPostBack("txtmillcode", "TextChanged");

    }

}

function Party(e) {
    debugger;
    if (e.keyCode == 112) {
        debugger;
        e.preventDefault();
        //$("#<%=pnlPopup.ClientID %>").show();
        $("#<%=hdnfpopup.ClientID%>").val("0");
        $("#<%=btntxtparty.ClientID %>").click();

    }
    if (e.keyCode == 9) {
        e.preventDefault();
        var unit = $("#<%=txtparty.ClientID %>").val();

        unit = "0" + unit;
        $("#<%=txtparty.ClientID %>").val(unit);
        __doPostBack("txtparty", "TextChanged");

    }

}

function broker(e) {
    debugger;
    if (e.keyCode == 112) {
        debugger;
        e.preventDefault();
        //$("#<%=pnlPopup.ClientID %>").show();
                $("#<%=hdnfpopup.ClientID%>").val("0");
                $("#<%=btntxtbroker.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtbroker.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtbroker.ClientID %>").val(unit);
                __doPostBack("txtbroker", "TextChanged");

            }

        }

        function subbroker(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                //$("#<%=pnlPopup.ClientID %>").show();
                $("#<%=hdnfpopup.ClientID%>").val("0");
                $("#<%=btntxtsubbroker.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtsubbroker.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtsubbroker.ClientID %>").val(unit);
                __doPostBack("txtsubbroker", "TextChanged");

            }

        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 7px;">
                <legend style="text-align: center;">
                    <asp:Label ID="label1" runat="server" Text="Sauda Book Utility" Font-Names="verdana"
                        ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
            </fieldset>
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfSuffix" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:HiddenField ID="hdnfpopup" runat="server" />

            <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 100;">
                <table style="width: 60%;" align="left" cellpadding="4" cellspacing="4">
                    <tr>
                        <td align="right" style="width: 30%;">Mill Code:
                        </td>
                        <td align="left" style="width: 20%;">
                            <asp:TextBox Height="24px" ID="txtmillcode" runat="Server" CssClass="txt" TabIndex="1"
                                Width="90px" Style="text-align: left;" OnkeyDown="sendingaccode(event);" AutoPostBack="false"
                                OnTextChanged="txtmillcode_TextChanged"></asp:TextBox>
                            <asp:Button Width="20px" Height="24px" ID="btntxtmillcode" runat="server" Text="..."
                                OnClick="btntxtmillcode_Click" CssClass="btnHelp" />
                            <asp:Label ID="lblmillname" runat="server" CssClass="lblName"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%;">Tender No:
                        </td>
                        <td align="left" style="width: 20%;">
                            <asp:TextBox Height="24px" ID="txttender" runat="Server" CssClass="txt" TabIndex="2"
                                Width="90px" Style="text-align: left;" AutoPostBack="false"></asp:TextBox>
                            <asp:Label ID="lblselfqty" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Label ID="lblgrade" runat="server" CssClass="lblName"></asp:Label>
                            <asp:Label ID="lbllifting" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%;">Party:
                        </td>
                        <td align="left" style="width: 20%;">
                            <asp:TextBox Height="24px" ID="txtparty" runat="Server" CssClass="txt" TabIndex="3"
                                Width="90px" Style="text-align: left;" OnkeyDown="Party(event);" AutoPostBack="false"
                                OnTextChanged="txtparty_TextChanged"></asp:TextBox>
                            <asp:Button Width="20px" Height="24px" ID="btntxtparty" runat="server" Text="..."
                                OnClick="btntxtparty_Click" CssClass="btnHelp" />
                            <asp:Label ID="lblpartyname" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%;">Delivery Type:
                        </td>
                        <td>

                            <asp:DropDownList ID="drpDeliveryType" runat="server" CssClass="ddl" TabIndex="4" Width="140px"
                                AutoPostBack="true" Height="26px" OnSelectedIndexChanged="drpDeliveryType_SelectedIndexChanged">
                                <asp:ListItem Text="Naka Delivery" Value="N" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Commission" Value="C"></asp:ListItem>
                            </asp:DropDownList>

                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%;">Broker:
                        </td>
                        <td align="left" style="width: 20%;">
                            <asp:TextBox Height="24px" ID="txtbroker" runat="Server" CssClass="txt" TabIndex="5"
                                Width="90px" Style="text-align: left;" OnkeyDown="broker(event);" AutoPostBack="false"
                                OnTextChanged="txtbroker_TextChanged"></asp:TextBox>
                            <asp:Button Width="20px" Height="24px" ID="btntxtbroker" runat="server" Text="..."
                                OnClick="btntxtbroker_Click" CssClass="btnHelp" />
                            <asp:Label ID="lblbrokername" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%;">sub Broker:
                        </td>
                        <td align="left" style="width: 20%;">
                            <asp:TextBox Height="24px" ID="txtsubbroker" runat="Server" CssClass="txt" TabIndex="6"
                                Width="90px" Style="text-align: left;" OnkeyDown="subbroker(event);" AutoPostBack="false"
                                OnTextChanged="txtsubbroker_TextChanged"></asp:TextBox>
                            <asp:Button Width="20px" Height="24px" ID="btntxtsubbroker" runat="server" Text="..."
                                OnClick="btntxtsubbroker_Click" CssClass="btnHelp" />
                            <asp:Label ID="lblsubbroker" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%;">Qntl:
                        </td>
                        <td align="left" style="width: 20%;">
                            <asp:TextBox Height="24px" ID="txtqntl" runat="Server" CssClass="txt" TabIndex="7"
                                Width="90px" Style="text-align: left;" AutoPostBack="false"></asp:TextBox>
                            Sauda date:
                
                    <asp:TextBox Height="24px" ID="txtsaudadate" runat="Server" CssClass="txt" TabIndex="8"
                        Width="90px" Style="text-align: left;" AutoPostBack="false"></asp:TextBox>

                            <asp:Image ID="imgcalendertxtsaudadate" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" /><ajax1:CalendarExtender ID="CalendarExtenderDatetxtsaudadate"
                                    runat="server" TargetControlID="txtsaudadate" PopupButtonID="imgcalendertxtsaudadate"
                                    Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>

                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%;">Sale Rate:
                        </td>
                        <td align="left" style="width: 20%;">
                            <asp:TextBox Height="24px" ID="txtsalerate" runat="Server" CssClass="txt" TabIndex="9"
                                Width="90px" Style="text-align: left;" AutoPostBack="false"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td align="right" style="width: 30%;">Commision Rate:
                        </td>
                        <td align="left" style="width: 20%;">
                            <asp:TextBox Height="24px" ID="txtcommrate" runat="Server" CssClass="txt" TabIndex="10"
                                Width="90px" Style="text-align: left;" AutoPostBack="false"></asp:TextBox>

                        </td>

                    </tr>
                    <tr>
                        <td align="right" style="width: 30%;">Payment Date:
                        </td>
                        <td align="left" style="width: 20%;">
                            <asp:TextBox Height="24px" ID="txtpayment" runat="Server" CssClass="txt" TabIndex="11"
                                Width="90px" Style="text-align: left;" AutoPostBack="false"></asp:TextBox>

                            <asp:Image ID="imgcalendertxtpaymentdate" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                Width="25px" Height="15px" /><ajax1:CalendarExtender ID="CalendarExtenderDatetxtpaymentdate"
                                    runat="server" TargetControlID="txtpayment" PopupButtonID="imgcalendertxtpaymentdate"
                                    Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>

                        </td>
                    </tr>
                    <br />
                    <br />
                    <tr align="right">
                        <td>
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btnHelp" TabIndex="12"
                                Width="90px" Height="24px" ValidationGroup="save" OnClick="btnUpdate_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>

            <br />
            <br />

            <asp:Panel ID="pnlPopup" onkeydown="closepopup(event);" runat="server" Width="70%"
                align="center" ScrollBars="None" BackColor="#FFFFE4" Direction="LeftToRight"
                Style="z-index: 5000; position: absolute; display: none; float: right; max-height: 500px; min-height: 500px; box-shadow: 1px 1px 8px 2px; background-position: center; left: 10%; top: 10%; margin: 0 auto;">
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
                            <asp:Panel ID="pnlInner" runat="server" ScrollBars="Both" Width="100%" Direction="LeftToRight"
                                BackColor="#FFFFE4" Style="z-index: 5000; float: right; overflow: auto; height: 400px">
                                <asp:GridView ID="grdPopup" Font-Bold="true" CssClass="select" runat="server" AutoGenerateColumns="true"
                                    AllowPaging="true" PageSize="13" EmptyDataText="No Records Found" HeaderStyle-BackColor="#6D8980"
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

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

