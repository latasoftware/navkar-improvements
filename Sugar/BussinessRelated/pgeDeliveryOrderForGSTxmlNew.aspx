<%@ Page Title="Delivery Order" Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="pgeDeliveryOrderForGSTxmlNew.aspx.cs" Inherits="Sugar_pgeDeliveryOrderForGSTxmlNew" %>

<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../../CSS/NewModalPopup.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Scripts/jquery-1.4.2.js"></script>
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
    <style type="text/css">
        .sms {
            font-size: small;
            font-weight: bold;
            color: Black;
        }
    </style>
    <script src="../../JS/jquery-2.1.3.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function memo() {
            //            window.open('../Sugar/pgeMotorMemo.aspx');    //R=Redirected  O=Original
            window.open('../Sugar/pgeMotorMemoxml.aspx');    //R=Redirected  O=Original

        }
        function sugarpurchase(Action, vocno) {
            //            window.open('../Sugar/pgeSugarPurchaseForGST.aspx');    //R=Redirected  O=Original
            //R=Redirected  O=Original
            window.open('../Inword/pgeSugarPurchaseForGSTxml.aspx?Action=' + Action + '&purchaseid=' + vocno);

        }

        function LocalVoucher(Action, vocno) {
            window.open('../Outword/pgeLocalVoucherForGSTxmlNew.aspx?Action=' + Action + '&commissionid=' + vocno);    //R=Redirected  O=Original
        }
        function loadingvoucher() {
            window.open('../Sugar/pgeLoadingvoucher.aspx');    //R=Redirected  O=Original
        }
        function salebill(Action, sbno) {
            //            window.open('../Sugar/pgrSugarsaleForGST.aspx');  
            window.open('../Outword/pgrSugarsaleForGSTxml.aspx?Action=' + Action + '&saleid=' + sbno);   //R=Redirected  O=Original
        }

        function DOParty(do_no, mill, tenderno) {
            var tn;
            //window.open('../Report/rptDeliveryOrderParty.aspx?do_no=' + do_no + '&email=' + email, '_blank');    //R=Redirected  O=Original
            window.open('../Report/rptDeliveryOrderParty.aspx?do_no=' + do_no + '&mill=' + mill + '&tenderno=' + tenderno, '_blank');
        }
        function od(do_no, mill, PO, a, tenderno) {
            var tn;
            window.open('../Report/rptDeliveryOrderForGST.aspx?do_no=' + do_no + '&mill=' + mill + '&PO=' + PO + '&a=' + a + '&tenderno=' + tenderno);    //R=Redirected  O=Original
        }
        function DC(do_no, email, PO, a) {
            var tn;
            window.open('../Report/rptDelivery_Challan.aspx?do_no=' + do_no + '&email=' + email + '&PO=' + PO + '&a=' + a);    //R=Redirected  O=Original
        }
        function TL(DONO, DOCODE) {
            var Donumber = document.getElementById('<%=txtdoc_no.ClientID %>').value;
            var DOCode = document.getElementById('<%=txtDO_CODE.ClientID %>').value;
            //if (DOCode != '') {
            window.open('../Report/rptNewTransferLetter.aspx?DONO=' + Donumber + '&DOCODE=' + DOCode);
            //}
        }
        function WB(Doc_Code) {
            var Donumber = document.getElementById('<%=txtdoc_no.ClientID %>').value;
            window.open('../Report/rptWayBill.aspx?Doc_No=' + Donumber);
        }

        function SB(saleid, billto) {

            window.open('../Report/pgeSaleBill_Print.aspx?doc_no=' + saleid + '&billto=' + billto);
            // window.open('../Report/rptSellBillForGST.aspx?billno=' + billno);
        }
        function pendingSB() {

            window.open('../Report/rptcheckpendingsalebill.aspx');
            // window.open('../Report/rptSellBillForGST.aspx?billno=' + billno);
        }
        function SB1(saleid, billto) {

            window.open('../Report/pgeSaleBill_Print1.aspx?doc_no=' + saleid + '&billto=' + billto);
            // window.open('../Report/rptSellBillForGST.aspx?billno=' + billno);
        }

        function od1(do_no, mill, PO, a, tenderno) {
            var tn;
            window.open('../Report/rptDeliveryOrderForGST1.aspx?do_no=' + do_no + '&mill=' + mill + '&PO=' + PO + '&a=' + a + '&tenderno=' + tenderno);    //R=Redirected  O=Original
        }
        function CV() {
            var VNO = document.getElementById('<%=lblVoucherNo.ClientID %>').innerText;
            var type = document.getElementById('<%=lblVoucherType.ClientID %>').innerText;
            window.open('../Report/rptVouchersNew.aspx?VNO=' + VNO + '&type=' + type);
        }

        function ITCV() {
            var VNO = document.getElementById('<%=lblVoucherNo.ClientID %>').innerText;
            window.open('../Report/rptITCVouc.aspx?Doc_No=' + VNO);
        }

        function MM(memono) {

            window.open('../Report/rptMotor_Memo.aspx?do_no=' + memono);
        }
        function DebitNote() {
            //window.open('../Sugar/pgeLocalvoucher.aspx');    //R=Redirected  O=Original
            //            window.open('../Sugar/pgeLocalVoucherForGST.aspx');    //R=Redirected  O=Original
            window.open('../Sugar/pgeLocalVoucherForGSTxml.aspx');    //R=Redirected  O=Original


        }

        function close() {

            document.getElementById('<%=pnlSendSMS.ClientID %>').style.display = "none";
        }
        function GEway() {
            var dono = document.getElementById('<%=txtdoc_no.ClientID %>').value;
            window.open('../Utility/pgeEwayBill.aspx?dono=' + dono);
        }
    </script>
    <script type="text/javascript">
        function Vasuli() {

            var drp = document.getElementById('<%=drpDOType.ClientID %>');
            var val = drp.options[drp.selectedIndex].value;

            var drp1 = document.getElementById('<%=drpDeliveryType.ClientID %>');
            var val1 = drp1.options[drp1.selectedIndex].value;

            if (val == "DI") {
                if (val1 == "C" || val1 == "N") {
                    var transport = document.getElementById('<%=txtTRANSPORT_CODE.ClientID %>').value;
                    if (transport == "" || transport == "0") {
                        alert('Transport Code Is Compulsory');
                        document.getElementById('<%=txtTRANSPORT_CODE.ClientID %>').focus();
                        document.getElementById("<%=btnSave.ClientID %>").disabled = false;
                        //document.getElementById("<%=btnSave.ClientID %>").value = "";
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }
        }</script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Do you want to delete data and all Vouchers?")) {
                confirm_value.value = "Yes";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "Yes";
            }
            else {
                confirm_value.value = "No";
                document.getElementById("<%= hdconfirm.ClientID %>").value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
        function Back() {
            window.open('../BussinessRelated/PgeDoHeadUtility.aspx', '_self');
        }
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

                if (hdnfClosePopupValue == "txtMILL_CODE") {

                    document.getElementById("<%=txtMILL_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtGstRate") {

                    document.getElementById("<%=txtGstRate.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtGETPASS_CODE") {

                    document.getElementById("<%=txtGETPASS_CODE.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtGetpassGstStateCode") {

                    document.getElementById("<%=txtGetpassGstStateCode.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtvoucher_by") {

                    document.getElementById("<%=txtvoucher_by.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtVoucherbyGstStateCode") {

                    document.getElementById("<%=txtVoucherbyGstStateCode.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtSalebilltoGstStateCode") {

                    document.getElementById("<%=txtSalebilltoGstStateCode.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtMillGstStateCode") {

                    document.getElementById("<%=txtMillGstStateCode.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtTransportGstStateCode") {

                    document.getElementById("<%=txtTransportGstStateCode.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtVasuliAc") {

                    document.getElementById("<%=txtVasuliAc.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtGRADE") {

                    document.getElementById("<%=txtGRADE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtDO_CODE") {

                    document.getElementById("<%=txtDO_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtBroker_CODE") {

                    document.getElementById("<%=txtBroker_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtTRANSPORT_CODE") {

                    document.getElementById("<%=txtTRANSPORT_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtBANK_CODE") {

                    document.getElementById("<%=txtBANK_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtdoc_no") {

                    document.getElementById("<%=txtdoc_no.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtNARRATION1") {

                    document.getElementById("<%=txtNARRATION1.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNARRATION2") {

                    document.getElementById("<%=txtNARRATION2.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNARRATION3") {

                    document.getElementById("<%=txtNARRATION3.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNARRATION4") {

                    document.getElementById("<%=txtNARRATION4.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNARRATION") {
                    document.getElementById("<%=txtNARRATION.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtBANK_AMOUNT.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtparty") {

                    document.getElementById("<%=txtNARRATION4.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtitem_Code") {

                    document.getElementById("<%=txtitem_Code.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtcarporateSale") {

                    document.getElementById("<%=txtcarporateSale.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtUTRNo") {

                    document.getElementById("<%=txtUTRNo.ClientID%>").focus();
                }
                if (hdnfClosePopupValue == "txtUTRNoU") {

                    document.getElementById("<%=txtNARRATION.ClientID%>").focus();
                }

                if (hdnfClosePopupValue == "txtPurcNo") {

                    document.getElementById("<%=btntxtPurcNo.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtSaleBillTo") {
                    document.getElementById("<%=txtSaleBillTo.ClientID %>").focus();
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
                debugger;
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


                if (hdnfClosePopupValue == "txtMILL_CODE") {
                    document.getElementById("<%=txtMILL_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=LBLMILL_NAME.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtMILL_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtGstRate") {
                    document.getElementById("<%=txtGstRate.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblGstRateName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtGstRate.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtGETPASS_CODE") {
                    document.getElementById("<%=txtGETPASS_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=LBLGETPASS_NAME.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtGETPASS_CODE.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtGetpassGstStateCode") {
                    document.getElementById("<%=txtGetpassGstStateCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lbltxtGetpassGstStateName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtGetpassGstStateCode.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtvoucher_by") {
                    document.getElementById("<%=txtvoucher_by.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblvoucherbyname.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtvoucher_by.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtVoucherbyGstStateCode") {
                    document.getElementById("<%=txtVoucherbyGstStateCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lbltxtVoucherbyGstStateName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtVoucherbyGstStateCode.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtSalebilltoGstStateCode") {
                    document.getElementById("<%=txtSalebilltoGstStateCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lbltxtSalebilltoGstStateName.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtSalebilltoGstStateCode.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtMillGstStateCode") {
                    document.getElementById("<%=txtMillGstStateCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lbltxtMillGstStateCode.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtMillGstStateCode.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtTransportGstStateCode") {
                    document.getElementById("<%=txtTransportGstStateCode.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lbltxtTransportGstStateCode.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtTransportGstStateCode.ClientID %>").focus();
                }

                if (hdnfClosePopupValue == "txtVasuliAc") {
                    document.getElementById("<%=txtVasuliAc.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lbltxtVasuliAc.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtVasuliAc.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtGRADE") {
                    document.getElementById("<%=txtGRADE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;

                    document.getElementById("<%=txtquantal.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtDO_CODE") {
                    document.getElementById("<%=txtDO_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=LBLDO_NAME.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtDO_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtBroker_CODE") {
                    document.getElementById("<%=txtBroker_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=LBLBROKER_NAME.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtBroker_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtTRANSPORT_CODE") {
                    document.getElementById("<%=txtTRANSPORT_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=LBLTRANSPORT_NAME.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtTRANSPORT_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtBANK_CODE") {
                    document.getElementById("<%=txtBANK_CODE.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblBank_name.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtBANK_CODE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtdoc_no") {
                    document.getElementById("<%=txtdoc_no.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtdoc_no.ClientID %>").disabled = false;
                    document.getElementById("<%=txtDOC_DATE.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtEditDoc_No") {
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").disabled = false;
                    document.getElementById("<%=txtEditDoc_No.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNARRATION1") {
                    document.getElementById("<%=txtNARRATION1.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtNARRATION2.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNARRATION2") {
                    document.getElementById("<%=txtNARRATION2.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtNARRATION3.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNARRATION3") {
                    document.getElementById("<%=txtNARRATION3.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtNarration5.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNARRATION4") {
                    document.getElementById("<%=txtNARRATION4.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtSalebilltoGstStateCode.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtNARRATION") {
                    document.getElementById("<%=txtNARRATION.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtBANK_AMOUNT.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtparty") {
                    document.getElementById("<%=txtNARRATION4.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtSaleBillTo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtSaleBillTo.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtitem_Code") {
                    document.getElementById("<%=txtitem_Code.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblitem_Name.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtitem_Code.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtcarporateSale") {
                    document.getElementById("<%=txtcarporateSale.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblCSYearCode.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[6].innerText;
                    document.getElementById("<%=txtcarporateSale.ClientID %>").disabled = false;
                    document.getElementById("<%=txtcarporateSale.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtUTRNo") {

                    document.getElementById("<%=txtUTRNo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblUTRYearCode.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[6].innerText;
                    document.getElementById("<%= hdnfUtrBalance.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[5].innerText;
                    document.getElementById("<%=txtNARRATION.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtLT_No.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[4].innerText;
                    document.getElementById("<%= hdnfUtrdetail.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[11].innerText;
                    document.getElementById("<%=txtNARRATION.ClientID%>").focus();
                }
                if (hdnfClosePopupValue == "txtUTRNoU") {
                    document.getElementById("<%=txtUTRNo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblUTRYearCode.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[6].innerText;
                    document.getElementById("<%= hdnfUtrBalance.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[5].innerText;
                    document.getElementById("<%=txtNARRATION.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtNARRATION.ClientID%>").focus();
                }
                if (hdnfClosePopupValue == "txtBill_To") {
                    document.getElementById("<%=txtBill_To.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=lblBill_To.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cells[1].innerText;
                    document.getElementById("<%=txtBill_To.ClientID %>").focus();
                }
                if (hdnfClosePopupValue == "txtSaleBillTo") {
                    document.getElementById("<%=txtSaleBillTo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtNARRATION4.ClientID %>").innerText = grid.rows[SelectedRowIndex + 1].cell1[2].innerText;
                    document.getElementById("<%=txtSaleBillTo.ClientID%>").focus();
                }
                if (hdnfClosePopupValue == "txtPurcNo") {
                    document.getElementById("<%=txtPurcNo.ClientID %>").disabled = false;
                    document.getElementById("<%=txtPurcOrder.ClientID %>").disabled = false;
                    document.getElementById("<%=txtPurcNo.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[0].innerText;
                    document.getElementById("<%=txtPurcOrder.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[12].innerText;
                    document.getElementById("<%= hdnf.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[13].innerText;
                    var cs = document.getElementById("<%=txtcarporateSale.ClientID %>").value;
                    if (cs == '') {
                        if (cs == 0) {
                            document.getElementById("<%=txtquantal.ClientID %>").value = grid.rows[SelectedRowIndex + 1].cells[9].innerText;
                        }
                    }
                    document.getElementById("<%=txtPurcNo.ClientID %>").focus();
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
    <script type="text/javascript" src="../../JS/DateValidation.js">
    </script>
    <script type="text/javascript">
        function DisableButton() {
            document.getElementById("<%=btnSave.ClientID %>").disabled = true;
        }

        function EnableButton() {
            document.getElementById("<%=btnSave.ClientID %>").disabled = false;
        }


    </script>
    <script type="text/javascript" language="javascript">
        function carporatesale(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtcarporateSale.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtcarporateSale.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtcarporateSale.ClientID %>").val(unit);
                __doPostBack("txtcarporateSale", "TextChanged");

            }
        }
        function millcode(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtMILL_CODE.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtMILL_CODE.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtMILL_CODE.ClientID %>").val(unit);
                __doPostBack("txtMILL_CODE", "TextChanged");

            }
        }
        function distance(e) {
            if (e.keyCode == 13) {
                debugger;

                e.preventDefault();
                $("#<%=btnSave.ClientID %>").focus();
            }

        }

        function millstatecode(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtMillGstStateCode.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtMillGstStateCode.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtMillGstStateCode.ClientID %>").val(unit);
                __doPostBack("txtMillGstStateCode", "TextChanged");

            }
        }
        function purcno(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtPurcNo.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtPurcNo.ClientID %>").val();

                __doPostBack("txtPurcNo", "TextChanged");

            }
        }
        function gstcode(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtGstRate.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtGstRate.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtGstRate.ClientID %>").val(unit);
                __doPostBack("txtGstRate", "TextChanged");

            }
        }
        function getpass(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtGETPASS_CODE.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtGETPASS_CODE.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtGETPASS_CODE.ClientID %>").val(unit);
                __doPostBack("txtGETPASS_CODE", "TextChanged");

            }

        }
        function statecode(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                function narration1(e) {
                    if (e.keyCode == 112) {
                        debugger;
                        e.preventDefault();
                        $("#<%=pnlPopup.ClientID %>").show();
                        $("#<%=btntxtNARRATION1.ClientID %>").click();

                    }

                }

                function narration3(e) {
                    if (e.keyCode == 112) {
                        debugger;
                        e.preventDefault();
                        $("#<%=pnlPopup.ClientID %>").show();
                        $("#<%=btntxtNARRATION3.ClientID %>").click();

                    }

                } $("#<%=btntxtGetpassGstStateCode.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtGetpassGstStateCode.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtGetpassGstStateCode.ClientID %>").val(unit);
                __doPostBack("txtGetpassGstStateCode", "TextChanged");

            }
        }
        function shipto(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtvoucher_by.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtvoucher_by.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtvoucher_by.ClientID %>").val(unit);
                __doPostBack("txtvoucher_by", "TextChanged");

            }
        }
        function shiptostatecode(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtVoucherbyGstStateCode.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtVoucherbyGstStateCode.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtVoucherbyGstStateCode.ClientID %>").val(unit);
                __doPostBack("txtVoucherbyGstStateCode", "TextChanged");

            }
        }
        function narration4(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtNARRATION4.ClientID %>").click();

            }

        }
        function narration1(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtNARRATION1.ClientID %>").click();

            }

        }
        function narration2(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtNARRATION2.ClientID %>").click();

            }

        }
        function narration3(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtNARRATION3.ClientID %>").click();

            }

        }
        function salebillstatecode(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtSalebilltoGstStateCode.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtSalebilltoGstStateCode.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtSalebilltoGstStateCode.ClientID %>").val(unit);
                __doPostBack("txtSalebilltoGstStateCode", "TextChanged");

            }
        }
        function grade(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtGRADE.ClientID %>").click();

            }

        }
        function transport(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtTRANSPORT_CODE.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtTRANSPORT_CODE.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtTRANSPORT_CODE.ClientID %>").val(unit);
                __doPostBack("txtTRANSPORT_CODE", "TextChanged");

            }
        }
        function transportstatecode(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtTransportGstStateCode.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtTransportGstStateCode.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtTransportGstStateCode.ClientID %>").val(unit);
                __doPostBack("txtTransportGstStateCode", "TextChanged");

            }
        }
        function vasuli(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtVasuliAc.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtVasuliAc.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtVasuliAc.ClientID %>").val(unit);
                __doPostBack("txtVasuliAc", "TextChanged");

            }
        }
        function docode(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtDO_CODE.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtDO_CODE.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtDO_CODE.ClientID %>").val(unit);
                __doPostBack("txtDO_CODE", "TextChanged");

            }
        }
        function broker(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtBroker_CODE.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtBroker_CODE.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtBroker_CODE.ClientID %>").val(unit);
                __doPostBack("txtBroker_CODE", "TextChanged");

            }
        }
        function item(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtitem_Code.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtitem_Code.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtitem_Code.ClientID %>").val(unit);
                __doPostBack("txtitem_Code", "TextChanged");

            }
        }
        function salebillto(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                document.getElementById("<%= hdnfClosePopup.ClientID %>").value = "txtSaleBillTo";
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btnSearch.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtSaleBillTo.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtSaleBillTo.ClientID %>").val(unit);
                __doPostBack("txtSaleBillTo", "TextChanged");

            }
        }
        function bankcode(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtBANK_CODE.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtBANK_CODE.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtBANK_CODE.ClientID %>").val(unit);
                __doPostBack("txtBANK_CODE", "TextChanged");

            }
        }
        function billto(e) {
            debugger;
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtbill_To.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtBill_To.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtBill_To.ClientID %>").val(unit);
                __doPostBack("txtBill_To", "TextChanged");

            }
        }
        function UTR(e) {
            if (e.keyCode == 112) {
                debugger;
                e.preventDefault();
                $("#<%=pnlPopup.ClientID %>").show();
                $("#<%=btntxtUTRNo.ClientID %>").click();

            }
            if (e.keyCode == 9) {
                e.preventDefault();
                var unit = $("#<%=txtUTRNo.ClientID %>").val();

                unit = "0" + unit;
                $("#<%=txtUTRNo.ClientID %>").val(unit);
                __doPostBack("txtUTRNo", "TextChanged");

            }
        }
        function UTRPrint() {
            window.open('../Transaction/pgeUtrentryxml.aspx?utrid=0&Action=2');
        }
        function DoOPen(DO) {
            var Action = 1;
            window.open('../BussinessRelated/pgeDeliveryOrderForGSTxmlNew.aspx?DO=' + DO + '&Action=' + Action, "_self");
        }
    </script>

    <script type="text/javascript">
        function Confirm1() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm(" Print=>Ok / preprint=>Cancel ")) {
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
        function pagevalidation() {
            debugger;

            var txtitem_Code = $("#<%=txtitem_Code.ClientID %>").val();
            if ((txtitem_Code == "") || txtitem_Code == "0") {
                $("#<%=txtitem_Code.ClientID %>").focus();
                return false;
            }

            var txtGstRate = $("#<%=txtGstRate.ClientID %>").val();
            if ((txtGstRate == "") || txtGstRate == "0") {
                $("#<%=txtGstRate.ClientID %>").focus();
                return false;
            }

            var txtMillGstStateCode = $("#<%=txtMillGstStateCode.ClientID %>").val();
            if ((txtMillGstStateCode == "") || txtMillGstStateCode == "0") {
                $("#<%=txtMillGstStateCode.ClientID %>").focus();
                return false;
            }

            var txtTRANSPORT_CODE = $("#<%=txtTRANSPORT_CODE.ClientID %>").val();
            if ((txtTRANSPORT_CODE == "") || txtTRANSPORT_CODE == "0") {
                $("#<%=txtTRANSPORT_CODE.ClientID %>").focus();
                return false;
            }

            var txtSaleBillTo = $("#<%=txtSaleBillTo.ClientID %>").val();
            if ((txtSaleBillTo == "") || txtSaleBillTo == "0") {
                $("#<%=txtSaleBillTo.ClientID %>").focus();
                return false;
            }

            var txtTransportGstStateCode = $("#<%=txtTransportGstStateCode.ClientID %>").val();

            if ((txtTransportGstStateCode == "") || txtTransportGstStateCode == "0") {
                $("#<%=txtTransportGstStateCode.ClientID %>").focus();
                return false;
            }

            var txtGetpassGstStateCode = $("#<%=txtGetpassGstStateCode.ClientID %>").val();
            if ((txtGetpassGstStateCode == "") || txtGetpassGstStateCode == "0") {
                $("#<%=txtGetpassGstStateCode.ClientID %>").focus();
                return false;
            }

            var txtVoucherbyGstStateCode = $("#<%=txtVoucherbyGstStateCode.ClientID %>").val();
            if ((txtVoucherbyGstStateCode == "") || txtVoucherbyGstStateCode == "0") {
                $("#<%=txtVoucherbyGstStateCode.ClientID %>").focus();
                return false;
            }

            //   var txtVoucherbyGstStateCode = $("#<%=txtVoucherbyGstStateCode.ClientID %>").val();
            //  if ((txtVoucherbyGstStateCode == "") || txtVoucherbyGstStateCode == "0") {
            //     $("#<%=txtVoucherbyGstStateCode.ClientID %>").focus();
            //     return false;
            // }

            var txtSalebilltoGstStateCode = $("#<%=txtSalebilltoGstStateCode.ClientID %>").val();

            if ((txtSalebilltoGstStateCode == "") || txtSalebilltoGstStateCode == "0") {
                $("#<%=txtSalebilltoGstStateCode.ClientID %>").focus();
                return false;
            }

            //var drpDOType = $("#<%=drpDOType.ClientID %>").val();

            //  if (drpDOType != "0") {
            //  $("#<%=drpDOType.ClientID %>").focus();
            //  //   return false;
            //  }
            //   if (txtSalebilltoGstStateCode.Text != string.Empty && txtSalebilltoGstStateCode.Text != "0" && txtSaleBillTo.Text != "0" && txtSaleBillTo.Text != string.Empty)

            var txtMILL_CODE = $("#<%=txtMILL_CODE.ClientID %>").val();

            if ((txtMILL_CODE == "") || txtMILL_CODE == "0") {
                $("#<%=txtMILL_CODE.ClientID %>").focus();
                return false;
            }

            var txtGstRate = $("#<%=txtGstRate.ClientID %>").val();

            if ((txtGstRate == "") || txtGstRate == "0") {
                $("#<%=txtGstRate.ClientID %>").focus();
                return false;
            }
            var txtGETPASS_CODE = $("#<%=txtGETPASS_CODE.ClientID %>").val();

            if ((txtGETPASS_CODE == "") || txtGETPASS_CODE == "0" && txtGETPASS_CODE == "2") {
                $("#<%=txtGETPASS_CODE.ClientID %>").focus();
                return false;
            }

            var txtvoucher_by = $("#<%=txtvoucher_by.ClientID %>").val();
            if ((txtvoucher_by == "") || txtvoucher_by == "0") {
                $("#<%=txtvoucher_by.ClientID %>").focus();
                return false;
            }

            var txtGRADE = $("#<%=txtGRADE.ClientID %>").val();
            if ((txtGRADE == "") || txtGRADE == "0") {
                $("#<%=txtGRADE.ClientID %>").focus();
                return false;
            }

            var txtquantal = $("#<%=txtquantal.ClientID %>").val();
            if ((txtquantal == "") || txtquantal == "0") {
                $("#<%=txtquantal.ClientID %>").focus();
                return false;
            }

            var txtPACKING = $("#<%=txtPACKING.ClientID %>").val();
            if ((txtPACKING == "") || txtPACKING == "0") {
                $("#<%=txtPACKING.ClientID %>").focus();
                return false;
            }

            var txtexcise_rate = $("#<%=txtexcise_rate.ClientID %>").val();
            if ((txtexcise_rate == "") || txtexcise_rate == "0") {
                $("#<%=txtexcise_rate.ClientID %>").focus();
                return false;
            }
            var txtPurcNo = $("#<%=txtPurcNo.ClientID %>").val();
            if ((txtPurcNo == "") || txtPurcNo == "0") {
                $("#<%=txtPurcNo.ClientID %>").focus();
                return false;
            }
            var txtDistance = $("#<%=txtDistance.ClientID %>").val();
            if ((txtDistance == "") || txtDistance == "0" || txtDistance == "0.00") {
                $("#<%=txtDistance.ClientID %>").focus();
                return false;
            }

            //var drpDOType = $("#<%=drpDOType.ClientID %>").val();
            //if (drpDOType == "DI") {
            //  var count = $("#<%=drpDOType.ClientID %>")
            //$("#<%=drpDOType.ClientID %>").focus();
            //return false;
            // }

            var txtSALE_RATE = $("#<%=txtSALE_RATE.ClientID %>").val();
            if ((txtSALE_RATE == "") || txtSALE_RATE == "0" || txtSALE_RATE == "0.00") {
                $("#<%=txtSALE_RATE.ClientID %>").focus();
                return false;
            }


            var txtVasuliAmount1 = $("#<%=txtVasuliAmount1.ClientID %>").val();
            var txtVasuliAc = $("#<%=txtVasuliAc.ClientID %>").val();
            if (txtVasuliAmount1 != "0" && txtVasuliAmount1 != "") {
                if (txtVasuliAc == "" || txtVasuliAc == "0") {
                    $("#<%=txtVasuliAc.ClientID %>").focus();
                    return false;
                }

            }
            // if ((txtVasuliAmount1 == "") || txtVasuliAmount1 == "0" || txtVasuliAmount1 == "0.00") {
            //
            //   $("#<%=txtVasuliAmount1.ClientID %>").focus();
            //    return false;
            // }
            // else {
            //
            // }
            var gridView = document.getElementById("<%=grdDetail.ClientID %>");
            var grdrow = gridView.getElementsByTagName("tr");
            var count = 0;
            var amount = 0;
            var millamt = $("#<%=lblMillAmount.ClientID %>").text();
            if (grdrow.length > 2) {
                for (var i = 0; i < grdrow.length; i++) {

                    if (gridView.rows[i].cells[12].innerHTML == "D") {
                        count++;
                    }

                }
                if ((grdrow - 1) == count) {
                    alert('Please Add Dispatch Details!')
                }
            }

            if (grdrow.length - 1 > 0) {
                for (var i = 1; i < grdrow.length; i++) {

                    if (gridView.rows[i].cells[12].innerHTML != "D" && gridView.rows[i].cells[12].innerHTML != "R") {
                        amount = parseFloat(amount + gridView.rows[i].cells[7].innerHTML);
                    }

                }
                if (amount != millamt) {
                    alert('Mill Amount Does Not match with detail amount!')
                }
            }

            return true;
            var unit = $("#<%=lblSB_No.ClientID %>").text();

            if ($("#<%=lblSB_No.ClientID %>") == null || unit == "0" || unit == "") {

                var confirm_value = document.createElement("INPUT");
                confirm_value.type = "hidden";
                confirm_value.name = "confirm_value";
                if (confirm(" Sale Bill Genearate=>Ok / Not Generate=>Cancel ")) {
                    confirm_value.value = "Yes";
                    document.getElementById("<%= hdnfgeneratesalebill.ClientID %>").value = "Yes";
                }
                else {
                    confirm_value.value = "No";
                    document.getElementById("<%= hdnfgeneratesalebill.ClientID %>").value = "No";
                }
                document.forms[0].appendChild(confirm_value);
            }
            else {
                document.getElementById("<%= hdnfgeneratesalebill.ClientID %>").value = "Yes";
            }


        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset style="border-top: 1px dotted rgb(131, 127, 130); border-radius: 3px; width: 90%; margin-left: 30px; float: left; border-bottom: 0px; padding-top: 0px; padding-bottom: 10px; border-left: 0px; border-right: 0px; height: 7px;">
        <legend style="text-align: center;">
            <asp:Label ID="label1" runat="server" Text="   Delivery Order For GST   " Font-Names="verdana"
                ForeColor="White" Font-Bold="true" Font-Size="12px"></asp:Label></legend>
    </fieldset>
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdconfirm" runat="server" />
            <asp:HiddenField ID="hdnfClosePopup" runat="server" />
            <asp:HiddenField ID="hdnf" runat="server" />
            <asp:HiddenField ID="hdnfSuffix" runat="server" />
            <asp:HiddenField ID="hdHelpPageCount" runat="server" />
            <asp:HiddenField ID="hdnvouchernumber" runat="server" />
            <asp:HiddenField ID="hdnmemonumber" runat="server" />
            <asp:HiddenField ID="hdnfpacking" runat="server" />
            <asp:HiddenField ID="hdnfPDSPartyCode" runat="server" />
            <asp:HiddenField ID="hdnfPDSUnitCode" runat="server" />
            <asp:HiddenField ID="hdnfSB_No" runat="server" />
            <asp:HiddenField ID="hdnfSaleRate" runat="server" />
            <asp:HiddenField ID="hdnfUtrBalance" runat="server" />
            <asp:HiddenField ID="hdnfMainBankAmount" runat="server" />
            <asp:HiddenField ID="hdnfOldBillAmt" runat="server" />
            <asp:HiddenField ID="hdnfQty" runat="server" />
            <asp:HiddenField ID="hdnfUtrdetail" runat="server" />
            <asp:HiddenField ID="hdnfgeneratesalebill" runat="server" />
            <asp:HiddenField ID="hdnfmillshortname" runat="server" />
            <asp:HiddenField ID="hdnfshiptoshortname" runat="server" />
            <asp:HiddenField ID="hdnfgetpassshortname" runat="server" />
            <asp:HiddenField ID="hdnftransportshortname" runat="server" />
            <asp:HiddenField ID="hdnfbilltoshortname" runat="server" />
            <asp:HiddenField ID="hdnfsalebilltoshortname" runat="server" />
            <asp:HiddenField ID="hdnfmc" runat="server" />
            <asp:HiddenField ID="hdnfgp" runat="server" />
            <asp:HiddenField ID="hdnfst" runat="server" />
            <asp:HiddenField ID="hdnfsb" runat="server" />
            <asp:HiddenField ID="hdnftc" runat="server" />
            <asp:HiddenField ID="hdnfbk" runat="server" />
            <asp:HiddenField ID="hdnfva" runat="server" />
            <asp:HiddenField ID="hdnfbt" runat="server" />
            <asp:HiddenField ID="hdnfdocd" runat="server" />
            <asp:HiddenField ID="hdnfcscode" runat="server" />
            <asp:HiddenField ID="hdnfic" runat="server" />
            <div>
                <asp:Panel ID="pnlMain" runat="server" Font-Names="verdana" Font-Bold="true" ForeColor="Black"
                    Font-Size="Small" Style="margin-left: 30px; margin-top: 0px; z-index: 90; float: left">
                    <table cellspacing="5" align="left">
                        <tr>
                            <td style="width: 100%;" colspan="6">
                                <table width="100%" align="left" style="border: 1px solid white;">
                                    <tr>
                                        <td align="left" colspan="8">
                                            <asp:Button ID="btnAdd" runat="server" Text="Add New" CssClass="btnHelp" Width="90px"
                                                TabIndex="63" ValidationGroup="save" OnClick="btnAdd_Click" Height="24px" />&nbsp;
                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnHelp" Width="90px"
                                                OnClientClick="if(!pagevalidation()){return false;};" TabIndex="64" ValidationGroup="add" OnClick="btnSave_Click" UseSubmitBehavior="false"
                                                Height="24px" />
                                            &nbsp;
                                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btnHelp" Width="90px"
                                                TabIndex="65" ValidationGroup="save" OnClick="btnEdit_Click" Height="24px" />
                                            &nbsp;
                                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btnHelp" Width="90px"
                                                TabIndex="66" ValidationGroup="add" OnClick="btnDelete_Click" OnClientClick="Confirm()" Height="24px" />
                                            &nbsp;
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btnHelp" Width="90px"
                                                TabIndex="67" ValidationGroup="save" OnClick="btnCancel_Click" Height="24px" />
                                            &nbsp;
                                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btnHelp" Width="90px"
                                                TabIndex="68" ValidationGroup="save" Height="24px" OnClientClick="Back()" />


                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">Change No:</td>
                            <td align="left" colspan="4">
                                <asp:TextBox runat="server" ID="txtEditDoc_No" CssClass="txt" Width="100px" Height="24px"
                                    TabIndex="0" AutoPostBack="true" OnTextChanged="txtEditDoc_No_TextChanged"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender runat="server" ID="txtxf" FilterType="Numbers" InvalidChars="."
                                    TargetControlID="txtEditDoc_No">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp;<asp:Label ID="lblMsg" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                    Font-Size="Small" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">Entry No: &nbsp;
                            </td>
                            <td align="left" colspan="7" style="width: 100%;">
                                <asp:TextBox ID="txtdoc_no" runat="Server" CssClass="txt" Width="80px" Style="text-align: right;"
                                    AutoPostBack="True" OnTextChanged="txtdoc_no_TextChanged" Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtDOC_NO" runat="server" Text="..." Width="80px" OnClick="btntxtdoc_no_Click"
                                    CssClass="btnHelp" Height="24px" />
                                <asp:Label ID="lbldoid" runat="server" Font-Bold="true" Font-Italic="true" Font-Names="verdana"
                                    Font-Size="Small" ForeColor="Red" Visible="false"></asp:Label>
                                Date:
                                <asp:TextBox ID="txtDOC_DATE" runat="Server" CssClass="txt" TabIndex="1" Width="80px"
                                    MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                    Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtDOC_DATE_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Image ID="imgcalender" runat="server" ImageUrl="~/Images/calendar_icon1.png"
                                    Width="25px" Height="15px" />
                                <ajax1:CalendarExtender ID="calenderExtenderDate" runat="server" TargetControlID="txtDOC_DATE"
                                    PopupButtonID="imgcalender" Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>
                                &nbsp;Carporate Sale:<asp:TextBox ID="txtcarporateSale" CssClass="txt" Width="80px"
                                    TabIndex="2" onkeydown="carporatesale(event);" runat="server" AutoPostBack="false" OnTextChanged="txtcarporateSale_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtcarporateSale" runat="server" CssClass="btnHelp" Text="C. Sale"
                                    TabIndex="2" Width="80px" OnClick="btntxtcarporateSale_Click" Height="24px" />
                                <asp:Label ID="lblCSYearCode" runat="server" CssClass="lblName"></asp:Label>
                                <asp:Label runat="server" ID="lblPDSParty" CssClass="lblName" ForeColor="Yellow"></asp:Label>
                                &nbsp; Do Type:
                                <asp:DropDownList ID="drpDOType" runat="server" CssClass="ddl" Width="100px" AutoPostBack="false"
                                    OnSelectedIndexChanged="drpDOType_SelectedIndexChanged" Height="26px" TabIndex="3">
                                    <asp:ListItem Text="Dispatch" Value="DI"></asp:ListItem>
                                    <asp:ListItem Text="D.O." Value="DO"></asp:ListItem>
                                </asp:DropDownList>
                                <%--<asp:TextBox ID="txtDESP_TYPE" runat="Server" CssClass="txt" TabIndex="2" Width="80px" style="text-align:left;"
                                      AutoPostBack="True" ontextchanged="txtDESP_TYPE_TextChanged"></asp:TextBox>--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">Mill Code:
                            </td>
                            <td align="left" colspan="2" style="width: 10%;">
                                <asp:TextBox ID="txtMILL_CODE" runat="Server" CssClass="txt" TabIndex="4" Width="80px"
                                    onkeydown="millcode(event);" Style="text-align: right;" AutoPostBack="false"
                                    OnTextChanged="txtMILL_CODE_TextChanged" Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtMILL_CODE" runat="server" Text="..." OnClick="btntxtMILL_CODE_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="LBLMILL_NAME" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                            <td>Mill State Code:
                                <asp:TextBox ID="txtMillGstStateCode" runat="Server" CssClass="txt" TabIndex="5"
                                    onkeydown="millstatecode(event);" Width="80px" Style="text-align: right;" AutoPostBack="false"
                                    OnTextChanged="txtMillGstStateCode_TextChanged" Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtMillGstStateCode" runat="server" Text="..." OnClick="btntxtMillGstStateCode_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="lbltxtMillGstStateCode" runat="server" CssClass="lblName"></asp:Label>&nbsp;&nbsp;
                                <asp:TextBox ID="txtMillEmailID" runat="server" Visible="false" CssClass="txt" Width="200px"
                                    AutoPostBack="True" OnTextChanged="txtMillEmailID_TextChanged" TabIndex="5" Height="24px"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;Mill Mobile:&nbsp;<asp:TextBox runat="server" ID="txtMillMobile"
                                    Width="100px" Height="24px" MaxLength="11" CssClass="txt"></asp:TextBox>&nbsp;<asp:Button
                                        runat="server" ID="btnSendSms" Text="Send Sms" CssClass="btnHelp" Height="24px"
                                        Width="80px" OnClick="btnSendSms_Click" />
                            </td>
                        </tr>
                        <tr>
                            <%-- <td align="left">
                            UTR No:
                        </td>
                        <td colspan="2" align="left">
                            <asp:TextBox ID="txtUTRNo" CssClass="txt" Width="80px" runat="server" AutoPostBack="true"
                                OnTextChanged="txtUTRNo_TextChanged" TabIndex="6" Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtUTRNo" runat="server" CssClass="btnHelp" Text="UTR No" Width="80px"
                                OnClick="btntxtUTRNo_Click" Height="24px" />
                            <asp:Label ID="lblUTRYearCode" runat="server" CssClass="lblName"></asp:Label>
                            &nbsp;
                        </td>--%>
                            <td align="left">Purc. No:
                            </td>
                            <td colspan="3" align="left">
                                <asp:TextBox ID="txtPurcNo" runat="Server" Enabled="false" CssClass="txt" Width="80px"
                                    onkeydown="purcno(event);" Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtPurcNo_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtPurcNo" runat="server" Text="..." OnClick="btntxtPurcNo_Click"
                                    TabIndex="6" CssClass="btnHelp" Height="24px" Width="20px" />
                                &nbsp;
                                <asp:TextBox ID="txtPurcOrder" runat="Server" Enabled="false" AutoPostBack="false"
                                    OnTextChanged="txtPurcNo_TextChanged" CssClass="txt" Width="90px" Style="text-align: right;"
                                    Height="24px"></asp:TextBox>
                                <asp:Label ID="lbltenderDetailID" runat="server" CssClass="lblName"></asp:Label>
                                &nbsp; Delivery Type:
                                <asp:DropDownList ID="drpDeliveryType" runat="server" CssClass="ddl" Width="140px"
                                    TabIndex="7" Enabled="true" AutoPostBack="true" Height="26px" OnSelectedIndexChanged="drpDeliveryType_SelectedIndexChanged">
                                    <asp:ListItem Text="Commission" Value="C"></asp:ListItem>
                                    <asp:ListItem Text="Naka Delivery" Value="N"></asp:ListItem>
                                    <asp:ListItem Text="DO" Value="D"></asp:ListItem>
                                </asp:DropDownList>
                                GST Code:
                                <asp:TextBox ID="txtGstRate" runat="Server" CssClass="txt" TabIndex="8" Width="80px"
                                    onkeydown="gstcode(event);" Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtGstRate_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtGstRate" runat="server" Text="..." OnClick="btntxtGstRate_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="lblGstRateName" runat="server" CssClass="lblName"></asp:Label>
                                <asp:Label runat="server" ID="lblPoDetails" CssClass="lblName" ForeColor="Yellow"></asp:Label>
                                &nbsp;Purchase Date:
                                <asp:TextBox ID="txtPurchase_Date" runat="Server" CssClass="txt" TabIndex="9" Width="80px"
                                    MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                    Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtPurchase_Date_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Image ID="imgcal" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                                    Height="15px" />
                                <ajax1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtPurchase_Date"
                                    PopupButtonID="imgcal" Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">Get Pass:
                            </td>
                            <td align="left" colspan="4" style="width: 10%;">
                                <asp:TextBox ID="txtGETPASS_CODE" runat="Server" CssClass="txt" TabIndex="10" Width="80px"
                                    onkeydown="getpass(event);" Style="text-align: right;" AutoPostBack="true" OnTextChanged="txtGETPASS_CODE_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtGETPASS_CODE" runat="server" Text="..." OnClick="btntxtGETPASS_CODE_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="LBLGETPASS_NAME" runat="server" CssClass="lblName"></asp:Label>
                                &nbsp;State Code:
                                <asp:TextBox runat="server" ID="txtGetpassGstStateCode" OnTextChanged="txtGetpassGstStateCode_TextChanged"
                                    TabIndex="11" onkeydown="statecode(event);" AutoPostBack="false" CssClass="txt"
                                    Height="24px" Width="30px" />
                                <asp:Button Text="..." runat="server" ID="btntxtGetpassGstStateCode" OnClick="btntxtGetpassGstStateCode_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label Text="" runat="server" ID="lbltxtGetpassGstStateName" CssClass="lblName" />
                                &nbsp; Item Code
                                <asp:TextBox ID="txtitem_Code" runat="Server" CssClass="txt" TabIndex="12" Width="80px"
                                    onkeydown="item(event);" Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtitem_Code_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtitem_Code" runat="server" Text="..." OnClick="btntxtitem_Code_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="lblitem_Name" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">Shipped To:
                            </td>
                            <td align="left" colspan="4" style="width: 10%;">
                                <asp:TextBox ID="txtvoucher_by" runat="Server" CssClass="txt" TabIndex="13" Width="80px"
                                    onkeydown="shipto(event);" Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtvoucher_by_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtvoucher_by" runat="server" Text="..." OnClick="btntxtvoucher_by_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="lblvoucherbyname" runat="server" CssClass="lblName"></asp:Label>
                                &nbsp;&nbsp;<asp:Label runat="server" ID="lblVoucherLedgerByBalance" ForeColor="Yellow"
                                    Text=""></asp:Label>
                                &nbsp;State Code:
                                <asp:TextBox runat="server" ID="txtVoucherbyGstStateCode" OnTextChanged="txtVoucherbyGstStateCode_TextChanged"
                                    TabIndex="14" onkeydown="shiptostatecode(event);" AutoPostBack="true" CssClass="txt"
                                    Height="24px" Width="30px" />
                                <asp:Button Text="..." runat="server" ID="btntxtVoucherbyGstStateCode" OnClick="btntxtVoucherbyGstStateCode_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label Text="" runat="server" ID="lbltxtVoucherbyGstStateName" CssClass="lblName" />
                                &nbsp;Bill To:
                                <asp:TextBox ID="txtBill_To" runat="Server" CssClass="txt" TabIndex="3" Width="90px"
                                    onkeydown="billto(event);" Style="text-align: right;" AutoPostBack="false" OnTextChanged="txtBill_To_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtbill_To" runat="server" Text="..." OnClick="btntxtbill_To_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="lblBill_To" runat="server" CssClass="lblName"></asp:Label>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">Sale Bill To:
                            </td>
                            <td align="left" colspan="4" style="width: 10%;">
                                <asp:TextBox runat="server" ID="txtSaleBillTo" Height="24px" Width="80px" Enabled="false"
                                    onkeydown="salebillto(event);" CssClass="txt" OnTextChanged="txtSaleBillTo_TextChanged" AutoPostBack="false"
                                    TabIndex="15"></asp:TextBox>&nbsp;
                                <asp:TextBox ID="txtNARRATION4" runat="Server" CssClass="txt" TabIndex="16" Width="200px"
                                    onkeydown="narration4(event);" Style="text-align: left;" AutoPostBack="false"
                                    OnTextChanged="txtNARRATION4_TextChanged" Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtNARRATION4" runat="server" Text="..." OnClick="btntxtNARRATION4_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                &nbsp; &nbsp;<asp:Label runat="server" ID="lblSaleBillToLedgerByBalance" ForeColor="Yellow"
                                    Text=""></asp:Label>
                                &nbsp; State Code:
                                <asp:TextBox runat="server" ID="txtSalebilltoGstStateCode" OnTextChanged="txtSalebilltoGstStateCode_TextChanged"
                                    TabIndex="17" onkeydown="salebillstatecode(event);" AutoPostBack="false" CssClass="txt"
                                    Height="24px" Width="30px" />
                                <asp:Button Text="..." runat="server" ID="btntxtSalebilltoGstStateCode" OnClick="btntxtSalebilltoGstStateCode_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label Text="" runat="server" ID="lbltxtSalebilltoGstStateName" CssClass="lblName" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;">Grade:
                            </td>
                            <td align="left" colspan="6">
                                <asp:TextBox ID="txtGRADE" runat="Server" CssClass="txt" TabIndex="18" Width="150px"
                                    onkeydown="grade(event);" Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtGRADE_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtGRADE" runat="server" Text="..." OnClick="btntxtGRADE_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                Quantal:
                                <asp:TextBox ID="txtquantal" runat="Server" CssClass="txt" TabIndex="19" Width="100px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtquantal_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="filtertxtquantal" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtquantal">
                                </ajax1:FilteredTextBoxExtender>
                                <asp:Label runat="server" ID="count" ForeColor="White" Visible="false"></asp:Label>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Packing:&nbsp;&nbsp;
                                <asp:TextBox ID="txtPACKING" runat="Server" CssClass="txt" Width="100px" Style="text-align: right;"
                                    TabIndex="20" AutoPostBack="True" OnTextChanged="txtPACKING_TextChanged" Height="24px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="FilteretxtPACKING" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtPACKING">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp;&nbsp;&nbsp;Bags:&nbsp;&nbsp;<asp:TextBox ID="txtBAGS" runat="Server" CssClass="txt"
                                    TabIndex="21" Width="100px" ReadOnly="true" Style="text-align: right;" AutoPostBack="True"
                                    OnTextChanged="txtBAGS_TextChanged" Height="24px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="FilteretxtBAGS" runat="server" FilterType="Numbers"
                                    TargetControlID="txtBAGS">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp;Excise:&nbsp;
                                <asp:TextBox ID="txtexcise_rate" runat="Server" CssClass="txt" Width="100px" Style="text-align: right;"
                                    TabIndex="22" AutoPostBack="True" OnTextChanged="txtexcise_rate_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="Filteretxtexcise_rate" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtexcise_rate">
                                </ajax1:FilteredTextBoxExtender>
                                Mill Amount:
                                <asp:Label ID="lblMillAmount" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">Mill Rate:
                            </td>
                            <td align="left" colspan="6" style="vertical-align: top;">
                                <asp:TextBox ID="txtmillRate" runat="Server" CssClass="txt" Width="100px" ReadOnly="true"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtmillRate_TextChanged"
                                    Height="24px" TabIndex="23"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="FilteretxtmillRate" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtmillRate">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Sale
                                Rate:
                                <asp:TextBox ID="txtSALE_RATE" runat="Server" CssClass="txt" Width="100px" Style="text-align: right;"
                                    TabIndex="24" AutoPostBack="True" OnTextChanged="txtSALE_RATE_TextChanged" Height="24px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="FilteretxtSALE_RATE" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtSALE_RATE">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp;&nbsp; Commission:<asp:TextBox runat="server" ID="txtCommission" Width="50px"
                                    TabIndex="25" AutoPostBack="true" Height="24px" CssClass="txt" OnTextChanged="txtCommission_TextChanged" />
                                &nbsp;&nbsp;&nbsp;Diff:
                                <asp:Label ID="lblDiffrate" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <%-- <tr>
                            <td align="left">
                                Sale Rate:
                            </td>
                            <td align="left" colspan="6" style="vertical-align: top;">
                                <asp:TextBox ID="txtSALE_RATE" runat="Server" CssClass="txt" TabIndex="16" Width="100px"
                                    Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtSALE_RATE_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="FilteretxtSALE_RATE" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtSALE_RATE">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp;&nbsp;GST Amount On S.R:&nbsp;
                                <asp:TextBox runat="server" ID="txtGstSRAmount" ReadOnly="true" CssClass="txt" Style="text-align: right;"
                                    Width="100px" Height="24px" />
                                <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtGstSRAmount">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp;&nbsp;S.R Excl GST:
                                <asp:TextBox runat="server" ID="txtGstExSaleRate" ReadOnly="true" CssClass="txt"
                                    Width="100px" Height="24px" />
                                <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2s" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="." TargetControlID="txtGstExSaleRate">
                                </ajax1:FilteredTextBoxExtender>
                            </td>
                        </tr>--%>
                        <tr>
                            <td align="left" style="border-bottom: 3px solid white; width: 10%;">Truck No:
                            </td>
                            <td align="left" style="width: 20%; border-bottom: 3px solid white;" colspan="6">
                                <asp:TextBox ID="txtTruck_NO" runat="Server" CssClass="txt" TabIndex="26" Width="140px"
                                    Style="text-align: left; text-transform: uppercase;" AutoPostBack="false" OnTextChanged="txtTruck_NO_TextChanged"
                                    Height="24px"></asp:TextBox>&nbsp; Driver Mobile:
                                <asp:TextBox runat="server" ID="txtDriverMobile" CssClass="txt" TabIndex="27" Width="140px"
                                    ToolTip="seperate numbers by comma" Style="text-align: left;" Height="24px" OnTextChanged="txtDriverMobile_TextChanged"></asp:TextBox>
                                <ajax1:FilteredTextBoxExtender ID="ajxmob" runat="server" FilterType="Numbers,Custom"
                                    ValidChars="," TargetControlID="txtDriverMobile">
                                </ajax1:FilteredTextBoxExtender>
                                &nbsp; Transport:
                                <asp:TextBox ID="txtTRANSPORT_CODE" runat="Server" CssClass="txt" TabIndex="28" Width="80px"
                                    onkeydown="transport(event);" Style="text-align: right;" AutoPostBack="True"
                                    OnTextChanged="txtTRANSPORT_CODE_TextChanged" Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtTRANSPORT_CODE" runat="server" Text="..." OnClick="btntxtTRANSPORT_CODE_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="LBLTRANSPORT_NAME" runat="server" CssClass="lblName"></asp:Label>&nbsp;PAN:<asp:TextBox
                                    ID="txtPanNo" Width="150px" Height="24px" CssClass="txt" runat="server" />&nbsp;
                                State Code:
                                <asp:TextBox ID="txtTransportGstStateCode" runat="Server" CssClass="txt" Width="80px"
                                    TabIndex="29" onkeydown="transportstatecode(event);" Style="text-align: right;"
                                    AutoPostBack="True" OnTextChanged="txtTransportGstStateCode_TextChanged" Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtTransportGstStateCode" runat="server" Text="..." OnClick="btntxtTransportGstStateCode_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="lbltxtTransportGstStateCode" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">Diff Amount:
                            </td>
                            <td align="left" style="width: 10%;" colspan="3">
                                <asp:TextBox ID="txtDIFF_AMOUNT" runat="Server" CssClass="txt" Width="100px" ReadOnly="true"
                                    TabIndex="30" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtDIFF_AMOUNT_TextChanged"
                                    Height="24px"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Frieght:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox
                                    TabIndex="31" runat="server" ID="txtFrieght" CssClass="txt" Width="100px" Style="text-align: right;"
                                    AutoPostBack="true" Height="24px" OnTextChanged="txtFrieght_TextChanged"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtFrieghtAmount" CssClass="txt" AutoPostBack="false"
                                    TabIndex="32" ReadOnly="true" Height="24px" Width="100px" Style="text-align: right;"
                                    OnTextChanged="txtFrieghtAmount_TextChanged"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp; Memo Advance:
                                <asp:DropDownList runat="server" ID="drpCC" CssClass="ddl" Width="70px" Height="24px"
                                    TabIndex="33">
                                    <asp:ListItem Text="Credit" Value="Credit"></asp:ListItem>
                                    <asp:ListItem Text="Cash" Value="Cash"></asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;rate:<asp:TextBox runat="server" ID="txtMemoAdvanceRate" Width="40px" Height="24px"
                                    TabIndex="34" AutoPostBack="true" CssClass="txt" OnTextChanged="txtMemoAdvanceRate_TextChanged"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtMemoAdvance" CssClass="txt" Width="100px" Style="text-align: right;"
                                    TabIndex="35" AutoPostBack="true" Height="24px" OnTextChanged="txtMemoAdvance_TextChanged"></asp:TextBox>
                                &nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblFrieghtToPay" ForeColor="Yellow"></asp:Label><asp:DropDownList
                                    runat="server" Visible="false" Width="100px" ID="ddlFrieghtType" CssClass="ddl">
                                    <asp:ListItem Text="Own" Value="O"></asp:ListItem>
                                    <asp:ListItem Text="Party" Value="P"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">Freight Paid:
                            </td>
                            <td align="left" colspan="5">
                                <asp:TextBox runat="server" ID="txtVasuliRate" CssClass="txt" Width="100px" Style="text-align: right;"
                                    TabIndex="36" AutoPostBack="true" Height="24px" OnTextChanged="txtVasuliRate_TextChanged"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtVasuliAmount" CssClass="txt" AutoPostBack="true"
                                    TabIndex="37" OnTextChanged="txtVasuliAmount_TextChanged" Height="24px" Width="100px"
                                    Style="text-align: right;"></asp:TextBox>&nbsp;&nbsp;&nbsp; Vasuli Rate:<asp:TextBox
                                        runat="server" ID="txtVasuliRate1" CssClass="txt" Width="80px" TabIndex="38"
                                        Style="text-align: right;" AutoPostBack="true" Height="24px" OnTextChanged="txtVasuliRate1_TextChanged"></asp:TextBox>&nbsp;
                                <asp:TextBox runat="server" ID="txtVasuliAmount1" CssClass="txt" Width="100px" Style="text-align: right;"
                                    TabIndex="39" AutoPostBack="true" OnTextChanged="txtVasuliAmount1_TextChanged"
                                    Height="24px"></asp:TextBox>&nbsp;&nbsp;<asp:Button runat="server" ID="btnVoucherOtherAmounts"
                                        Visible="false" Text="Other" CssClass="btnHelp" Width="70px" Height="24px" OnClick="btnVoucherOtherAmounts_Click" />&nbsp;&nbsp;
                                Vasuli A/c:
                                <asp:TextBox ID="txtVasuliAc" runat="Server" CssClass="txt" Width="80px" Style="text-align: right;"
                                    TabIndex="40" onkeydown="vasuli(event);" AutoPostBack="false" OnTextChanged="txtVasuliAc_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtVasuliAc" runat="server" Text="..." OnClick="btntxtVasuliAc_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="lbltxtVasuliAc" runat="server" CssClass="lblName"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">D.O.
                            </td>
                            <td align="left" colspan="5">
                                <asp:TextBox ID="txtDO_CODE" runat="Server" CssClass="txt" Width="120px" Style="text-align: right;"
                                    TabIndex="41" onkeydown="docode(event);" AutoPostBack="false" OnTextChanged="txtDO_CODE_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtDO_CODE" runat="server" Text="..." OnClick="btntxtDO_CODE_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="LBLDO_NAME" runat="server" CssClass="lblName">
                                </asp:Label>
                                Mill EWay Bill No:
                           
                                <asp:TextBox ID="txtMillEwayBill_No" runat="Server" CssClass="txt" Width="150px"
                                    TabIndex="45" Style="text-align: left;" Height="24px"></asp:TextBox>
                                <asp:TextBox ID="txtMillInvoiceno" runat="Server" CssClass="txt" Width="150px" Style="text-align: left;"
                                    TabIndex="46" Height="24px"></asp:TextBox>

                                mill Invoice Date:
                                  <asp:TextBox ID="txtMillInv_Date" runat="Server" CssClass="txt" Width="80px"
                                      TabIndex="47" MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"
                                      Style="text-align: left;" AutoPostBack="false" OnTextChanged="txtMillInv_Date_TextChanged"
                                      Height="24px"></asp:TextBox>
                                <asp:Image ID="imgtxtMillInv_Date" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                                    Height="15px" />
                                <ajax1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtMillInv_Date"
                                    PopupButtonID="imgtxtMillInv_Date" Format="dd/MM/yyyy">
                                </ajax1:CalendarExtender>

                                <asp:CheckBox runat="server" ID="chkEWayBill" TabIndex="48" AutoPostBack="true" OnCheckedChanged="chkEWayBill_CheckedChanged" />
                                <asp:Label runat="server" ID="lblchkEWayBill" CssClass="lblName"></asp:Label>
                                My EWay Bill No: 
                                  <asp:TextBox ID="txtEWayBill_No" runat="Server" CssClass="txt" Width="150px" Style="text-align: left;"
                                      TabIndex="49" Height="24px"></asp:TextBox>


                            </td>

                        </tr>
                        <tr>
                            <td>Broker:
                            </td>
                            <td align="left" colspan="5">
                                <asp:TextBox ID="txtBroker_CODE" runat="Server" CssClass="txt" Width="120px" Style="text-align: right;"
                                    TabIndex="44" onkeydown="broker(event);" AutoPostBack="false" OnTextChanged="txtBroker_CODE_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtBroker_CODE" runat="server" Text="..." OnClick="btntxtBroker_CODE_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />
                                <asp:Label ID="LBLBROKER_NAME" runat="server" CssClass="lblName"></asp:Label>
                                Distance:
                                <asp:TextBox ID="txtDistance" runat="Server" CssClass="txt" TabIndex="50" Width="80px"
                                    onkeydown="distance(event);" Style="text-align: right;" AutoPostBack="false" Height="24px" OnTextChanged="txtDistance_TextChanged"></asp:TextBox>
                                Invoice Checked:<asp:CheckBox runat="server"
                                    TabIndex="51" ID="chkInv_Chk" Width="10px" Height="10px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 10%;"></td>
                            <td align="left" colspan="3" style="width: 10%;"></td>
                        </tr>
                        <tr>
                            <td align="left">Narration1:
                            </td>
                            <td align="left" colspan="5">
                                <asp:TextBox ID="txtNARRATION1" runat="Server" CssClass="txt" Width="400px" Style="text-align: left;"
                                    TabIndex="52" onkeydown="narration1(event);" AutoPostBack="True" OnTextChanged="txtNARRATION1_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtNARRATION1" runat="server" Text="..." OnClick="btntxtNARRATION1_Click"
                                    CssClass="btnHelp" Heignht="24px" Width="20px" />&nbsp;<asp:CheckBox runat="server"
                                        ID="chkNoprintondo" Width="10px" Height="10px" />
                                Narration2:
                           
                                <asp:TextBox ID="txtNARRATION2" runat="Server" CssClass="txt" Width="200px" Style="text-align: left;"
                                    TabIndex="53" onkeydown="narration2(event);" AutoPostBack="True" OnTextChanged="txtNARRATION2_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtNARRATION2" runat="server" Text="..." OnClick="btntxtNARRATION2_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />

                                Narration3:
                           
                                <asp:TextBox ID="txtNARRATION3" runat="Server" CssClass="txt" Width="200px" Style="text-align: left;"
                                    TabIndex="54" onkeydown="narration3(event);" AutoPostBack="True" OnTextChanged="txtNARRATION3_TextChanged"
                                    Height="24px"></asp:TextBox>
                                <asp:Button ID="btntxtNARRATION3" runat="server" Text="..." OnClick="btntxtNARRATION3_Click"
                                    CssClass="btnHelp" Height="24px" Width="20px" />

                            </td>
                            </td>
                        </tr>
                        <tr>

                            <td align="left" style="width: 10%;">Narration 5:
                            </td>
                            <td align="left" style="width: 50%;">
                                <asp:TextBox TextMode="MultiLine" runat="server" ID="txtNarration5" CssClass="txt"
                                    TabIndex="55" Width="280px" Height="50px" />
                                <asp:LinkButton runat="server" ID="lnkMemo" Text="Memo No:" Style="color: Black; text-decoration: none;"
                                    ToolTip="Click to Go On Motor Memo" OnClick="lnkMemo_Click"></asp:LinkButton>
                                &nbsp;<asp:Label ID="lblMemoNo" runat="server" CssClass="lblName"></asp:Label>&nbsp;
                                <asp:LinkButton runat="server" ID="lnkVoucOrPurchase" Text="Number:" Style="color: Black; text-decoration: none;"
                                    ToolTip="Click to Go On Respective Page" OnClick="lnkVoucOrPurchase_Click"></asp:LinkButton>&nbsp;<asp:Label
                                        ID="lblVoucherNo" runat="server" CssClass="lblName"></asp:Label>&nbsp;&nbsp;<asp:Label
                                            ID="lblVoucherType" runat="server" CssClass="lblName"></asp:Label>
                                &nbsp;&nbsp;<asp:LinkButton runat="server" ID="lblsbnol" Text="" Style="color: Black; text-decoration: none;"
                                    ToolTip="Click to Go On Sale Bill" OnClick="lblsbnol_Click"></asp:LinkButton>
                                &nbsp;<asp:Label ID="lblSB_No" runat="server" CssClass="lblName"></asp:Label>&nbsp;&nbsp;&nbsp;
                                Loading Sms Sent:
                                <asp:Label Text="" ID="lblLoadingSms" Font-Bold="true" ForeColor="Yellow" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Button ID="btnOpenDetailsPopup" runat="server" Text="ADD" CssClass="btnHelp"
                                    TabIndex="56" Width="80px" Height="24px" OnClick="btnOpenDetailsPopup_Click" />&nbsp;&nbsp;&nbsp;
                            </td>
                            <td colspan="4" align="left">
                                <asp:Button ID="btnOurDO" runat="server" Text="Our DO" CssClass="btnHelp" Width="70px"
                                    Height="24px" OnClick="btnOurDO_Click" OnClientClick="Confirm1()" />
                                <asp:Button ID="btnMail" runat="server" Text="Party DO" CssClass="btnHelp" Width="70px"
                                    ValidationGroup="save" OnClick="btnMail_Click" Height="24px" />
                                <asp:Button ID="btnpendingsale" runat="server" Text="Pending SB" CssClass="btnHelp" Width="90px"
                                    Height="24px" OnClick="btnpendingsale_Click" />
                                <asp:Button ID="btngenratesalebill" runat="server" Text="Genearate SB" CssClass="btnHelp" Width="90px"
                                    Height="24px" OnClick="btngenratesalebill_Click" />
                                <asp:Button runat="server" ID="btnGentare_EWayBill" Text="Gentare EWayBill" CssClass="btnHelp"
                                    Width="90px" Height="24px" OnClientClick="GEway();" />
                                <asp:Button runat="server" ID="btnPrintSaleBill" Text="SB Print" CssClass="btnHelp"
                                    OnClick="btnPrintSaleBill_Click" Width="70px" Height="24px" OnClientClick="Confirm1()" />
                                <input type="button" id="btnOpenSendsmspoup" onclick="showsmspopup();" runat="server"
                                    value="SMS Screen" class="btnHelp" style="width: 80px; height: 24px;" />
                                <asp:Button ID="btnUtrShortCut" runat="server" Text="Make UTR" CssClass="btnHelp"
                                    Width="70px" Height="24px" OnClientClick="UTRPrint()" />
                                <asp:Button runat="server" ID="btnPrintMotorMemo" Text="Motor Memo" CssClass="btnHelp"
                                    OnClick="btnPrintMotorMemo_Click" Width="70px" Height="24px" />

                                <asp:Button ID="btnDeliveryChallan" runat="server" Text="Delivery Challan" CssClass="btnHelp"
                                    Height="24px" OnClick="btnDeliveryChallan_Click" />


                                <asp:Button ID="btnWayBill" runat="server" Text="Way Bill" CssClass="btnHelp" Width="70px"
                                    ValidationGroup="save" OnClientClick="return WB();" Height="24px" />


                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div style="width: 100%; position: relative; top: 0px; left: 0px;">
                <table width="100%" style="vertical-align: top;" align="left" cellspacing="2">
                    <tr>
                        <td rowspan="6" style="vertical-align: top;" align="left">
                            <asp:UpdatePanel ID="upGrid" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlgrdDetail" runat="server" ScrollBars="Both" Height="150px" Width="1000px"
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
                        </td>
                    </tr>


                </table>
            </div>
            <asp:Panel ID="pnlPopup" onkeydown="closepopup(event);" runat="server" Width="90%"
                ScrollBars="Both" align="center" BackColor="#FFFFE4" Direction="LeftToRight"
                Style="z-index: 5000; position: absolute; display: none; float: left; max-height: 500px; min-height: 500px; box-shadow: 1px 1px 8px 2px; background-position: center; left: 5%; top: 10%;">
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
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
                                            Height="15px" />
                                        <ajax1:CalendarExtender ID="calendertxtFromDate" runat="server" TargetControlID="txtFromDate"
                                            PopupButtonID="Image1" Format="dd/MM/yyyy">
                                        </ajax1:CalendarExtender>
                                        To:
                                        <asp:TextBox ID="txtToDate" runat="server" CssClass="txt" Width="80px" Height="24px"
                                            MaxLength="10" onkeyup="ValidateDate(this,event.keyCode)" onkeydown="return DateFormat(this,event.keyCode)"></asp:TextBox>
                                        <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/calendar_icon1.png" Width="25px"
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
            <asp:Panel ID="pnlPopupDetails" runat="server" BackColor="GhostWhite" Width="800px"
                BorderColor="Teal" BorderWidth="1px" Height="320px" BorderStyle="Solid" Style="z-index: 4999; left: 15%; top: 20%; position: absolute; display: none; font-weight: bold; box-shadow: 1px 2px 10px 2px;">
                <table width="80%" align="center" cellspacing="5px">
                    <tr>
                        <td colspan="2" align="center" style="background-color: lightslategrey; color: White;">
                            <asp:Label ID="lblTenderDetails" runat="server" Font-Size="Medium" Font-Names="verdana"
                                Text="D.O. Details"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>ID&nbsp;<asp:Label ID="lblID" runat="server"></asp:Label>
                        </td>
                        <td>NO&nbsp;<asp:Label ID="lblNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">Type:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:DropDownList ID="drpddType" runat="server" CssClass="ddl" TabIndex="54" Width="200px"
                                Height="30px">
                                <asp:ListItem Text="transfer Letter" Value="T"></asp:ListItem>
                                <asp:ListItem Text="Demand Draft" Value="D"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">BANK CODE:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtBANK_CODE" runat="Server" CssClass="txt" TabIndex="55" Width="60px"
                                onkeydown="bankcode(event);" Height="24px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtBANK_CODE_TextChanged"></asp:TextBox>
                            <asp:Button ID="btntxtBANK_CODE" runat="server" Height="24px" Width="20px" Text="..."
                                OnClick="btntxtBANK_CODE_Click" CssClass="btnHelp" />
                            <asp:Label ID="lblBank_name" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">UTR No:
                        </td>
                        <td colspan="2" align="left">
                            <asp:TextBox ID="txtUTRNo" CssClass="txt" Width="80px" runat="server" AutoPostBack="true"
                                onkeydown="UTR(event);" OnTextChanged="txtUTRNo_TextChanged" TabIndex="56" Height="24px"></asp:TextBox>
                            <asp:Button ID="btntxtUTRNo" runat="server" CssClass="btnHelp" Text="UTR No" Width="80px"
                                TabIndex="56" OnClick="btntxtUTRNo_Click" Height="24px" />
                            <asp:Label ID="lblUTRYearCode" runat="server" CssClass="lblName"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">DD/CHQ/RTGS No:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtNARRATION" runat="Server" CssClass="txt" TabIndex="57" Height="24px"
                                Width="200px" Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtNARRATION_TextChanged"></asp:TextBox>
                            <asp:Button ID="btntxtNARRATION" runat="server" Text="..." Height="24px" Width="20px"
                                OnClick="btntxtNARRATION_Click" CssClass="btnHelp" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">Amount:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtBANK_AMOUNT" runat="Server" CssClass="txt" TabIndex="58" Height="24px"
                                Width="200px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtBANK_AMOUNT_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteretxtBANK_AMOUNT" runat="server" FilterType="Numbers,Custom"
                                ValidChars="." TargetControlID="txtBANK_AMOUNT">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">With GST Amount per Quantal:
                        </td>
                        <td align="left" style="width: 10%;">
                            <asp:TextBox ID="txtwithGst_Amount" runat="Server" CssClass="txt" TabIndex="59" Height="24px"
                                Width="200px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtwithGst_Amount_TextChanged"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 10%;">LT No:
                        </td>
                        <td>
                            <asp:TextBox ID="txtLT_No" runat="Server" CssClass="txt" TabIndex="60" Height="24px"
                                Width="200px" Style="text-align: left;" AutoPostBack="True" OnTextChanged="txtLT_No_TextChanged"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <asp:Label runat="server" ID="lblUtrBalnceError" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <asp:Button ID="btnAdddetails" runat="server" Text="ADD" CssClass="btnSubmit" Width="80px"
                                Height="25px" OnClick="btnAdddetails_Click" TabIndex="61" />
                            <asp:Button ID="btnClosedetails" runat="server" Text="Close" CssClass="btnSubmit"
                                Width="80px" Height="25px" OnClick="btnClosedetails_Click" TabIndex="62" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlVoucherEntries" runat="server" Width="500px" align="center" ScrollBars="None"
                Direction="LeftToRight" Style="z-index: 5000; position: absolute; display: none; background-color: White; float: right; max-height: 300px; min-height: 300px; background-position: center; left: 50%; margin-left: -400px; top: 30%;"
                Height="400px"
                BorderStyle="Groove" BorderColor="Blue" BorderWidth="2px">
                <table width="80%" align="center" cellspacing="5">
                    <tr>
                        <td align="center" colspan="2" style="border: 1px solid blue;">
                            <asp:Label runat="server" ID="lblKa" Text="Amounts For Voucher" ForeColor="Blue"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Brokrage:</b>
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtVoucherBrokrage" CssClass="txt" Width="120px"
                                Height="24px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtVoucherBrokrage_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtBrokrage" TargetControlID="txtVoucherBrokrage"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Service Charge:</b>
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txtVoucherServiceCharge" CssClass="txt" Width="120px"
                                Height="24px" Style="text-align: right;" AutoPostBack="True" OnTextChanged="txtVoucherServiceCharge_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txtVoucherServiceCharge"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Rate Diff:</b>
                            <asp:TextBox ID="txtVoucherL_Rate_Diff" runat="Server" CssClass="txt" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" Height="24px" OnTextChanged="txtVoucherL_Rate_Diff_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtL_Rate_Diff" TargetControlID="txtVoucherL_Rate_Diff"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                            <b>Amount:</b>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtVoucherRATEDIFFAmt" runat="Server" CssClass="txt" Width="120px"
                                Style="text-align: right;" AutoPostBack="True" ReadOnly="true" Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtRATEDIFF" TargetControlID="txtVoucherRATEDIFFAmt"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Bank Comm:</b>
                            <asp:TextBox ID="txtVoucherCommission_Rate" runat="Server" CssClass="txt" Width="80px"
                                Style="text-align: right;" AutoPostBack="True" Height="24px" OnTextChanged="txtVoucherCommission_Rate_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtCommission_Rate" TargetControlID="txtVoucherCommission_Rate"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                            <b>Amount:</b>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtVoucherBANK_COMMISSIONAmt" runat="Server" CssClass="txt" Width="120px"
                                ReadOnly="true" Style="text-align: right;" AutoPostBack="True" Height="24px"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtBANK_COMMISSION" TargetControlID="txtVoucherBANK_COMMISSIONAmt"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Interest:</b>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtVoucherInterest" runat="Server" CssClass="txt" Width="120px"
                                Style="text-align: right;" AutoPostBack="True" Height="24px" OnTextChanged="txtVoucherInterest_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtInterest" TargetControlID="txtVoucherInterest"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Transport:</b>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtVoucherTransport_Amount" runat="Server" CssClass="txt" Width="120px"
                                Style="text-align: right;" AutoPostBack="True" Height="24px" OnTextChanged="txtVoucherTransport_Amount_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtTransport_Amount" TargetControlID="txtVoucherTransport_Amount"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <b>Other Expenses:</b>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtVoucherOTHER_Expenses" runat="Server" CssClass="txt" Width="120px"
                                Style="text-align: right;" AutoPostBack="True" Height="24px" OnTextChanged="txtVoucherOTHER_Expenses_TextChanged"></asp:TextBox>
                            <ajax1:FilteredTextBoxExtender ID="filtertxtOTHER_Expenses" TargetControlID="txtVoucherOTHER_Expenses"
                                FilterType="Numbers,Custom" runat="server" ValidChars=".">
                            </ajax1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button runat="server" ID="btnOk" Text="OK" CssClass="btnHelp" Width="60px" Height="24px"
                                OnClick="btnOk_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlSendSMS" runat="server" Width="60%" align="center" ScrollBars="Vertical"
                BackColor="White" Direction="LeftToRight" Style="z-index: 5000; position: absolute; display: none; float: right; max-height: 500px; min-height: 500px; box-shadow: 1px 1px 8px 2px; background-position: center; left: 10%; top: 10%;">
                <table cellpadding="5" cellspacing="5" style="width: 100%; border: 1px solid black;"
                    class="smstable">
                    <thead style="background-color: Blue; color: White; font-weight: bold; height: 25px;">
                        <tr>
                            <th align="left">Message
                            </th>
                            <th align="left">To
                            </th>
                            <th align="left">Mobile
                            </th>
                            <th align="center">Send
                            </th>
                        </tr>
                    </thead>
                    <tbody id="table-body-id">
                        <tr>
                            <td style="width: 300px;">
                                <asp:Label ID="partysms" runat="server" CssClass="sms"></asp:Label>
                            </td>
                            <td style="width: 300px;">
                                <asp:Label ID="partyname" runat="server"></asp:Label>
                            </td>
                            <td style="width: 120px;">
                                <input type="text" id="partymobile" class="textbox" />
                            </td>
                            <td style="width: 30px;" id="partyyesno" class="sendyesno">Y
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 300px;">
                                <asp:Label ID="millsms" runat="server" CssClass="sms" />
                            </td>
                            <td style="width: 300px;">
                                <asp:Label ID="millname" runat="server" />
                            </td>
                            <td style="width: 120px;">
                                <input type="text" id="millmobile" class="textbox" />
                            </td>
                            <td style="width: 30px;" class="sendyesno">N
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 300px;">
                                <asp:Label ID="driversms" runat="server" CssClass="sms" />
                            </td>
                            <td style="width: 300px;">
                                <asp:Label ID="transportname" runat="server" />
                            </td>
                            <td style="width: 120px;">
                                <input type="text" id="txtdriverno" class="textbox" />
                            </td>
                            <td style="width: 30px;" class="sendyesno">Y
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 300px;">
                                <asp:Label ID="transportsms" runat="server" CssClass="sms" />
                            </td>
                            <td style="width: 300px;">
                                <asp:Label ID="party" runat="server" />
                            </td>
                            <td style="width: 120px;">
                                <input type="text" id="txtpartymobile" class="textbox" />
                            </td>
                            <td style="width: 30px;" class="sendyesno">N
                            </td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="2" align="center">
                                <input type="button" id="btnInvoke" value="SEND" class="btn btnHelp btn-primary"
                                    onclick="return send();" />
                                <asp:Button Text="CLOSE" CssClass="btn btnHelp btn-primary" runat="server" ID="btnClosePopup"
                                    OnClick="btnClosePopup_Click" />
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        $(document).ready(function () {

            //            $('#<%=btnOpenDetailsPopup.ClientID %>').focusout(function () {
            //                debugger;
            //                $('#ContentPlaceHolder1_grdDetail_lnkEdit_0').focus();
            //            });

            $('body').on("dblclick", "#table-body-id tr", function () {

                var rowindex = $(this).index();
                var cell = $(this).closest('tr').children('td.sendyesno').text().trim();
                if (cell == "Y") {
                    $(this).closest('tr').children('td.sendyesno').text("N");
                }
                else {
                    $(this).closest('tr').children('td.sendyesno').text("Y");
                }
            });
        });

        function showsmspopup() {
            debugger;
            var dono = $('#<%=txtdoc_no.ClientID %>').val();
            var company_code = '<%= Session["Company_Code"] %>';
            var year_code = '<%= Session["year"] %>';
            var param = 'Doc_No=' + dono + '&Company_Code=' + company_code + '&Year_Code=' + year_code;
            $.ajax({
                type: 'GET',
                url: '../Handlers/dodetails.ashx',
                data: param,
                success: function (data) {
                    debugger;
                    if (

                        data != null) {
                        var obj = JSON.parse(data);
                        var dono = obj[0].doc_no;
                        var qntl = obj[0].quantal;
                        var millshort = obj[0].millshortname;
                        var date = obj[0].doc_dateConverted;
                        var grade = obj[0].grade;
                        var lorry = obj[0].truck_no;
                        var voucherbyname = obj[0].voucherbyname;
                        var voucherbyshort = obj[0].voucherbyname;
                        var voucherbymobile = obj[0].voucherbymobno;
                        var voucherbyaddress = obj[0].vouvherbyaddress;
                        var voucherbycity = obj[0].voucherbycityname;
                        var voucherbystate = obj[0].voucherbycitystate;


                        var ShippetdToLICNO = obj[0].ShippetdToLICNO;
                        var shippedTo_Address = obj[0].shiptoaddress;
                        var ShippetdToTin_no = obj[0].ShippetdToTin_no;
                        var ShippetdToFSSAI = obj[0].shiptofssai;
                        var ShippetdToCompanyPan = obj[0].shiptopanno;
                        var ShippetdToECC = obj[0].shiptoeccno;
                        var ShippetdToCST_No = obj[0].ShippetdToCST_No;
                        var shippedTo_City_Name = obj[0].shiptocityname;
                        var ShippedtTo_City_State = obj[0].shiptocitystate;
                        var shippedTo_Mobileno = obj[0].shiptomobno;
                        var Shipped_ToName = obj[0].shiptoname;
                        var ShippedTo_GSt = obj[0].shiptogstno;



                        var getpassshort = obj[0].getpassname;
                        var getpasscity = obj[0].getpasscityname;
                        var getpassstate = obj[0].getpasscitystate;
                        var getpassmobile = obj[0].getpassmobno;
                        var millrate = obj[0].mill_rate;
                        var millmobile = obj[0].millmobno;
                        var drivermobile = obj[0].driver_no;
                        var frtperqtl = obj[0].freight;
                        var vasulirate = obj[0].vasuli_rate;
                        var frtplusvasuli = frtperqtl + vasulirate;


                        var getpassname = obj[0].getpassname;
                        var millname = obj[0].millname;
                        var transportname = obj[0].transportname;
                        var transportmobile = obj[0].transportmobno;
                        var getpasssln = obj[0].getpasssln;
                        var getpasstin = obj[0].getpasstin;
                        var getpasscst = obj[0].getpasscstno;
                        var getpassgst = obj[0].getpassgstno;
                        var getpassecc = obj[0].getpassecc;
                        var getpasspan = obj[0].getpasspanno;
                        var getpassfssai = obj[0].getpassfssai;


                        var voucherbysln = obj[0].voucherbysln;
                        var voucherbytin = obj[0].voucherbytin;
                        var voucherbycst = obj[0].voucherbycstno;
                        var voucherbygst = obj[0].voucherbygstno;
                        var voucherbyecc = obj[0].voucherbyecc;
                        var voucherbypan = obj[0].voucherbypan;
                        var voucherbyfssai = obj[0].voucherbyfssai;

                        var voucherbynumber = obj[0].voucherbymobno;
                        var voucher_no = obj[0].voucher_no;
                        var voucher_type = obj[0].voucher_type;
                        var memo_no = obj[0].memo_no;
                        var SB_No = obj[0].SB_No;

                        var brokermobile = obj[0].brokermobno;
                        var salebilltoMobile = obj[0].salebillmobno;
                        var sms1mobile = brokermobile;
                        if (brokermobile == '') {
                            sms1mobile = salebilltoMobile;
                            if (salebilltoMobile == '') {
                                sms1mobile = getpassmobile;
                            }
                        }

                        var vtype = "";
                        if (voucher_no != "0" && voucher_no != "") {
                            if (voucher_type == "PS") {
                                //vtype = "Sale Bill No: " + SB_No;
                            }
                            else if (voucher_type == "OV") {
                                vtype = "Voucher No: " + voucher_no;
                            }
                            else if (voucher_type == "LV") {
                                vtype = "Debit Note No: " + voucher_no;
                            }
                        }

                        var memonumber = "";
                        if (memo_no != "0") {
                            memonumber = "Motor Memo: " + memo_no;
                        }

                        var transordrivermobile = "";
                        if (drivermobile != "") {
                            transordrivermobile = " Driver Mob:" + drivermobile;
                        }
                        else if (transportmobile != "") {
                            transordrivermobile = " Transport Mob:" + transportmobile;
                        }

                        var getpassnumbers = getpasssln + ' ' + getpasstin + ' ' + getpasscst + ' ' + getpassgst + ' ' + getpassecc
                         + ' ' + getpasspan + ' ' + getpassfssai;
                        var getpassonlyonenumber = '';
                        var vocuherbyonemobile = '';


                        //var getpassonlyonenumber = getpassmobile.split(',')[0];
                        //var vocuherbyonemobile = voucherbymobile.split(',')[0];

                        var voucherbynumbers = voucherbysln + ' ' + voucherbytin + ' ' + voucherbycst + ' ' + voucherbygst + ' ' + voucherbyecc
                        + ' ' + voucherbypan + ' ' + voucherbyfssai;


                        var vocuherbydetails = " Shipped To: " + voucherbyname + " Address:" + voucherbyaddress + " City:" + voucherbycity + " State:"
                        + voucherbystate + " " + voucherbynumbers;


                        var Shippedbynumbers = ShippetdToLICNO + ' ' + ShippetdToTin_no + ' ' + ShippetdToCST_No + ' ' + ShippedTo_GSt + ' ' + ShippetdToECC
                        + ' ' + ShippetdToCompanyPan + ' ' + ShippetdToFSSAI;


                        var Shippeddbydetails = " Shipped To: " + Shipped_ToName + " Address:" + shippedTo_Address + " City:" + shippedTo_City_Name + " State:"
                        + ShippedtTo_City_State + " " + Shippedbynumbers;



                        var vocuherbydetailsfortansport = " Address:" + voucherbyaddress;

                        var Shippedbydetailsfortansport = " Address:" + shippedTo_Address;

                        //.......................................................

                        //                        var partysms = "Truck is send for loading.DO." + dono + " " + millshort + " Qntl:" + qntl + " Date:" + date + " " + grade + " "
                        //                         + lorry + " " + voucherbyshort + " Sale Bill No:" + SB_No;

                        var partysms = "Truck is send for loading.DO." + dono + " " + millshort + " Qntl:" + qntl + " Date:" + date + " " + grade + " "
                         + lorry + " " + Shipped_ToName + " Sale Bill No:" + SB_No;
                        //.......................................................
                        //                        var millsms = "DO." + dono + " Getpass:" + getpassname + " " + getpasscity + " " + getpassstate + " " + getpassnumbers
                        //                        + vocuherbydetails + ' ' + " Qntl:" + qntl + " Date:" + date + " M.R." + millrate + " " + grade + " " + lorry;


                        //                        var millsms = "DO." + dono + " Getpass:" + getpassname + " " + getpasscity + " " + getpassstate + " " + getpassnumbers
                        //                        + vocuherbydetails + " Qntl:" + qntl + " Date:" + date + " M.R." + millrate + " " + grade + " " + lorry;


                        var millsms = "DO." + dono + " Getpass:" + getpassname + " " + getpasscity + " " + getpassstate + " " + getpassnumbers
                        + Shippeddbydetails + " Qntl:" + qntl + " Date:" + date + " M.R." + millrate + " " + grade + " " + lorry;

                        //...........................................................

                        //                        var driversms = "DO." + dono + " " + millshort + " " + voucherbyname + " " + voucherbycity + " " + voucherbystate + " " + vocuherbydetailsfortansport
                        //                         + " " + voucherbytin + " " + voucherbygst + " " + salebilltoMobile + "  Qntl:" + qntl + " date:" + date + ' Freight:' + frtperqtl + " Lorry:" + lorry;




                        var driversms = "DO." + dono + " " + millshort + " " + Shipped_ToName + " " + shippedTo_City_Name + " " + ShippedtTo_City_State
                        + " " + Shippedbydetailsfortansport
                         + " " + ShippetdToTin_no + " " + ShippedTo_GSt + " " + salebilltoMobile + "  Qntl:" + qntl
                         + " date:" + date + ' Freight:' + frtperqtl + " Lorry:" + lorry;




                        //--------------------------------------------------------------------------------


                        //                        var transportsms = "DO." + dono + " The truck is confirm load dt." + date + " " + millshort + " Getpass:" + getpassname + " Shipped To:"
                        //                         + voucherbyshort + " qntl:" + qntl + " " + grade + " " + lorry + ' Freight:' + frtplusvasuli + transordrivermobile + " Sale Bill No:"
                        //                         + SB_No + " " + vtype;


                        var transportsms = "DO." + dono + " The truck is confirm load dt." + date + " " + millshort + " Getpass:" + getpassname + " Shipped To:"
                         + Shipped_ToName + " qntl:" + qntl + " " + grade + " " + lorry + ' Freight:' + frtplusvasuli + transordrivermobile + " Sale Bill No:"
                         + SB_No + " " + vtype;


                        //party
                        $('#<%=partysms.ClientID %>').html(partysms);
                        $('#<%=partyname.ClientID %>').html(voucherbyname);
                        $('#partymobile').val(sms1mobile);

                        //mill
                        $('#<%=millsms.ClientID %>').html(millsms);
                        $('#<%=millname.ClientID %>').html(millname);
                        $('#millmobile').val(millmobile);

                        //driver
                        $('#<%=driversms.ClientID %>').html(driversms);
                        $('#<%=transportname.ClientID %>').html(transportname);
                        $('#txtdriverno').val(transportmobile);

                        //transportsms 
                        $('#<%=transportsms.ClientID %>').html(transportsms);
                        $('#<%=party.ClientID %>').html(voucherbyname);
                        $('#txtpartymobile').val(sms1mobile);
                    }


                    document.getElementById('<%=pnlSendSMS.ClientID %>').style.display = "block";
                    document.getElementById('btnInvoke').focus();
                }
            });
        }

        function send() {
            debugger;
            var partymsg = $('#<%=partysms.ClientID %>').html();
            var partymobile = $('#partymobile').val();

            $('#table-body-id tr').each(function () {
                var a = $(this).closest('tr').children('td.sendyesno').text().trim();
                if (a == "Y") {
                    debugger;
                    var mobile = $(this).closest('tr').find('.textbox').val();
                    var msg = $(this).closest('tr').find('.sms').html();
                    if (mobile != "") {
                        $.ajax({
                            type: 'POST',
                            url: '../sendsms.asmx/SendSMS',
                            data: "{'msg':'" + msg + "','mobile':'" + mobile + "'}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (data) {
                                if (data == 1) {

                                }
                            }
                        });
                    }
                }
            });

            if (confirm("back to DO->OK, close->cancel")) {

                document.getElementById('<%=pnlSendSMS.ClientID %>').style.display = "none";
                document.getElementById('<%=btnAdd.ClientID %>').focus();
            }
        }
    </script>
</asp:Content>
