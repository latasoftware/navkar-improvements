using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.IO;
using System.Globalization;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Text;
using System.Diagnostics;


public partial class Sugar_pgeDeliveryOrderForGSTxmlNew : System.Web.UI.Page
{
    #region data section
    string temp = "0";

    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string SystemMastertable = string.Empty;
    string qryCommon = string.Empty;
    string qryHead = string.Empty;
    string qryDetail = string.Empty;
    string cityMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string qrycarporateSalebalance = string.Empty;
    string qryUTRBalance = string.Empty;
    string qrypurc_No = string.Empty;
    string qryAccountList = string.Empty;
    string millShortName = string.Empty;
    int defaultAccountCode = 0;
    string trnType = "DO";
    string AUTO_VOUCHER = string.Empty;
    string GLedgerTable = string.Empty;
    static WebControl objAsp = null;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    public static int an = 0;
    double LvAmnt = 0.00;
    string Action = string.Empty;
    string cs = string.Empty;
    int DOC_NO = 0;
    int doid = 0;
    double OldGSTAmt = 0.00;
    double OldSaleAmt = 0.00;
    double OldCommission = 0.00;
    double OldBillAmt;
    double OldQty = 0.00;

    #endregion
    MySqlConnection con = null;
    MySqlCommand cmd = null;
    MySqlTransaction myTran = null;
    DataTable dt1 = null;
    DataTable dt2 = null;
    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;
    DataSet DS = null;

    StringBuilder Head_Update = null;
    string Head_Insert = string.Empty;
    string Head_Delete = string.Empty;
    StringBuilder Head_Values = null;
    StringBuilder Head_Fields = null;
    string Purchase_Delete = string.Empty;
    string Sale_Delete = string.Empty;
    string Detail_Insert = string.Empty;
    StringBuilder Detail_Update = null;
    StringBuilder Detail_Delete = null;
    StringBuilder Detail_Fields = null;
    StringBuilder Detail_Values = null;
    string GLEDGER_Insert = string.Empty;
    string GLEDGER_Delete = string.Empty;
    int flag = 0;
    string msg = string.Empty;
    string SelfBal = string.Empty;

    string timeset = "";
    public static string storeproceduertimmer = "";

    #region -Head part declearation
    string Limit = string.Empty;
    //Int32 DOC_NO = 0;
    string DOC_DATE = string.Empty;
    string PUR_DATE = string.Empty;
    string DESP_TYPE = string.Empty;
    string Delivery_Type = string.Empty;
    string MillInv_Date = string.Empty;
    string Inv_Chk = string.Empty;
    string MILL_CODE = string.Empty;
    string gst_code = string.Empty;
    string GETPASS_CODE = string.Empty;
    string VOUCHER_BY = string.Empty;
    double FRIEGHT_RATE = 0;
    double FRIEGHT_AMOUNT = 0.00;
    double VASULI_AMOUNT = 0.00;
    double VASULI_RATE = 0.00;
    double MEMO_ADVANCE = 0.00;
    string Ac_Code = string.Empty;
    string GRADE = string.Empty;
    double QUANTAL = 0.00;
    Int32 PACKING = 0;
    Int32 BAGS = 0;
    double mill_rate = 0.00;
    double EXCISE_RATE = 0.00;
    double Tender_Commission = 0.00;
    double SALE_RATE = 0.00;
    double MILL_AMOUNT = 0.00;

    double DIFF_RATE = 0.00;
    double DIFF_AMOUNT = 0.00;
    double VASULI_RATE_1 = 0.00;
    double VASULI_AMOUNT_1 = 0.00;
    string EWayBill_No = string.Empty;
    double Distance = 0.00;
    string SaleBillTo = string.Empty;

    string MM_CC = string.Empty;
    //double Party_Commission_Rate =0.00;
    double MM_Rate = 0.00;
    string PAN_NO = string.Empty;
    Int32 DO_CODE = 0;
    Int32 BROKER_CODE = 0;
    string TRUCK_NO = string.Empty;
    Int32 TRANSPORT_CODE = 0;
    Int32 VASULI_AC = 0;


    double Tender_Commission_Amount = 0.00;
    Int32 OVTransportCode = 0;

    string NARRATION1 = string.Empty;
    string NARRATION2 = string.Empty;
    string NARRATION3 = string.Empty;
    string NARRATION4 = string.Empty;
    string NARRATION5 = string.Empty;
    string INVOICE_NO = string.Empty;
    string CheckPost = string.Empty;
    int purc_no = 0;
    int purc_order = 0;

    #region other voucher amount
    double VoucherBrokrage = 0.00;
    double VoucherServiceCharge = 0.00;
    double VoucherRateDiffRate = 0.00;
    double VoucherRateDiffAmt = 0.00;
    double VoucherBankCommRate = 0.00;
    double VoucherBankCommAmt = 0.00;
    double VoucherInterest = 0.00;
    double VoucherTransport = 0.00;
    double VoucherOtherExpenses = 0.00;

    string EWay_BillChk = string.Empty;
    string MillInvoiceno = string.Empty;

    #endregion

    double FINAL_AMOUNT = 0.00;
    string userinfo = string.Empty;
    string retValue = string.Empty;
    string strRev = string.Empty;
    int Company_Code = 0;
    int Year_Code = 0;
    //int year_Code = 0;
    int Branch_Code = 0;
    float DIFF = 0;
    double LESS_DIFF = 0.00;
    double LESSDIFFOV = 0.00;
    string Driver_Mobile = string.Empty;

    Int32 Carporate_Sale_No = 0;
    string WhoseFrieght = string.Empty;
    int UTR_Year_Code = 0;
    int Carporate_Sale_Year_Code = 0;
    Int32 voucher_no = 0;
    string PDS = string.Empty;

    int memo_no = 0;

    string voucher_type = string.Empty;
    Int32 voucherlbl = 0;

    string myNarration = string.Empty;
    string myNarration2 = string.Empty;
    string myNarration3 = string.Empty;
    string myNarration4 = string.Empty;
    string vouchnarration = string.Empty;


    string utrno = string.Empty;
    string Utr_No = string.Empty;
    string nar = string.Empty;

    int VOUCHERAMOUNT = 0;
    //double MILL_AMOUNT =string.Empty;
    string city_code = string.Empty;
    string From_Place = string.Empty;
    string city_code2 = string.Empty;
    string To_Place = string.Empty;
    int SELFAC = 0;
    double BILL_AMOUNT = 0.00;
    double SUBTOTAL = 0.00;
    double TotalGstPurchaseAmount = 0.00;
    double TotalGstSaleAmount = 0.00;

    int GSTRateCode = 0;
    int GetpassGstStateCode = 0;
    int VoucherbyGstStateCode = 0;
    int SalebilltoGstStateCode = 0;
    int MillGstStateCode = 0;
    int TransportGstStateCode = 0;
    double GSTMillRateAmount = 0.00;
    double GSTSaleRateAmount = 0.00;
    double GSTExclSaleRateAmount = 0.00;
    double GSTExclMillRateAmount = 0.00;
    double GSTRate = 0.00;
    double cgstrate = 0.00;
    double sgstrate = 0.00;
    double igstrate = 0.00;
    double LessFriegthRateForSB = 0.00;
    double LessFriegthAmountForSB = 0.00;
    Int32 pdsparty = 0;
    Int32 pdsunit = 0;
    int paymentto = 0;
    int gstSateCodeForPurchaseBill = 0;
    int paymenttogststatecode = 0;
    // int CompanyGSTStateCode = 0;
    double millamount = 0.00;
    double cgsttaxAmountOnMR = 0.00;
    double sgsttaxAmountOnMR = 0.00;
    int pdspartystatecode = 0;
    double saleamount = 0.00;
    double sgsttaxAmountOnSR = 0.00;
    double igsttaxAmountOnSR = 0.00;
    double cgsttaxAmountOnSR = 0.00;
    int GSTRateCodeForRateDiff = 0;
    double GSTRateForLV = 0.00;
    double cgstrateForLV1 = 0.00;
    double sgstrateForLV1 = 0.00;
    double igstrateForLV1 = 0.00;

    double CGSTAmountForLV = 0.00;
    double SGSTAmountForLV = 0.00;
    double IGSTAmountForLV = 0.00;

    double CGSTRateForLV = 0.00;
    double SGSTRateForLV = 0.00;
    double IGSTRateForLV = 0.00;
    int CompanyGSTStateCode = 0;
    double voucherAmountForLV = 0.00;
    int SaleCGSTAc = 0;
    int SaleSGSTAc = 0;
    int SaleIGSTAc = 0;
    int PayableCGSTAc = 0;
    int PayableSGSTAc = 0;
    int PayableIGSTAc = 0;
    double CGSTAmountForPS = 0.0;
    double SGSTAmountForPS = 0.0;
    double IGSTAmountForPS = 0.0;
    double CGSTRateForPS = 0.00;
    double SGSTRateForPS = 0.00;
    double IGSTRateForPS = 0.00;

    double CGSTAmountForSB = 0.0;
    double SGSTAmountForSB = 0.0;
    double IGSTAmountForSB = 0.0;

    double CGSTRateForSB = 0.00;
    double SGSTRateForSB = 0.00;
    double IGSTRateForSB = 0.00;

    double SaleRateForSB = 0.00;
    double TaxableAmountForSB = 0.00;
    double SaleRateForNaka = 0.00;
    double gstOnSalerateAndAdvance = 0.00;

    double Diff_Rate = 0.00;
    double VOUCHER_AMOUNT = 0.00;
    string Rate_Type = string.Empty;
    Int32 SaleBillTransport = 0;
    Int32 DONumber = 0;
    string cureentbalance = string.Empty;
    string limitbalnce = string.Empty;
    double CurrBal = 0.00;
    double limitBal = 0.00;
    double GSTAmt = 0.00;
    double SaleAmt = 0.00;
    double Commission = 0.00;
    double BillAmt = 0.00;
    double Qty = 0.00;
    double NetLimit = 0.00;
    double OldBillAmtNew = 0.00;
    int Bill_To = 0;
    int bt = 0;
    int? mc = 0;
    int? gp = 0;
    int? st = 0;
    int? sb = 0;
    int? tc = 0;
    int? itemcode = 0;
    int? cscode = 0;
    int? ic = 0;
    int? tenderdetailid = 0;
    int? bk = 0;
    int? docd = 0;
    int? va = 0;

    string MillEwayBill = string.Empty;
    #endregion
    #region Detail Declare
    Int32 detail_Id = 0;
    string ddType = "";
    Int32 Bank_Code = 0;
    string Narration = "";
    double Amount = 0.0;
    int Utr_no = 0;
    int UTRDetail_ID = 0;
    int LT_no = 0;
    Int32 GID = 0;
    int bc = 0;
    int utrdetailid = 0;
    #endregion

    #region PURCHASE And SALE Posting VAriables
    PurchaseFields purchase = null;
    SaleFields salePosting = null;
    LocalVoucher LV = null;

    int millCityCode = 0;
    string fromPlace = string.Empty;
    int getPassCityCode = 0;
    string toPlace = string.Empty;
    int CompanyStateCode = 0;
    double MILLAMOUNT = 0.00;
    double CGSTtaxAmountOnMR = 0.00;
    double SGSTtaxAmountOnMR = 0.00;
    double IGSTtaxAmountOnMR = 0.00;

    double SGSTtaxAmountOnSR = 0.00;
    double CGSTtaxAmountOnSR = 0.00;
    double IGSTtaxAmountOnSR = 0.00;
    double CGST_AMOUNT = 0.00;
    double SGST_AMOUNT = 0.00;
    double IGST_AMOUNT = 0.00;
    double TOTALPurchase_Amount = 0.00;
    double ITEM_AMOUNT = 0.00;
    int VOUCHER_NO = 0;
    int maxcountpsno = 0;
    double CGSTRATE = 0.00;
    double SGSTRATE = 0.00;
    double IGSTRATE = 0.00;
    double GSTRATE = 0.00;
    int PaymentTo = 0;

    double TaxableAmountForSaleBill = 0.00;
    double LessFriegthRateForSaleBill = 0.00;
    double LessFriegthAmountForSaleBill = 0.00;
    int saleparty = 0;
    int pdsunitSaleBill = 0;
    double SaleBillSaleRate = 0.00;

    #endregion
    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["user"].ToString();
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "deliveryorder";
            tblDetails = tblPrefix + "DODetails";
            AccountMasterTable = tblPrefix + "AccountMaster";
            cityMasterTable = tblPrefix + "CityMaster";
            SystemMastertable = tblPrefix + "SystemMaster";
            qryCommon = tblPrefix + "qryDeliveryOrderList";
            qryHead = "qrydohead";
            qryDetail = "qrydodetail";
            pnlPopup.Style["display"] = "none";
            GLedgerTable = tblPrefix + "GLEDGER";
            qrycarporateSalebalance = tblPrefix + "qryCarporatesellbalance";
            qryUTRBalance = tblPrefix + "qryUTRBalance";
            qryAccountList = "qrymstaccountmaster";
            qrypurc_No = "qrysugarBalancestock";
            cs = ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
            con = new MySqlConnection(cs);
            Maindt = new DataTable();
            dr = null;
            Maindt.Columns.Add("Querys", typeof(string));
            dr = Maindt.NewRow();
            DS = new DataSet();
            if (!Page.IsPostBack)
            {
                //txtSearchText.Attributes.Add("onkeypress", "abc(event);");
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    //hdnvouchernumber.Value = "0";
                    Action = Request.QueryString["Action"];
                    if (Action == "1")
                    {
                        hdnf.Value = Request.QueryString["DO"];
                        pnlPopup.Style["display"] = "none";
                        ViewState["currentTable"] = null;
                        clsButtonNavigation.enableDisable("N");
                        this.makeEmptyForm("N");
                        ViewState["mode"] = "I";
                        this.showLastRecord();
                        setFocusControl(btnEdit);
                    }
                    else
                    {
                        string docno = string.Empty;
                        clsButtonNavigation.enableDisable("A");
                        ViewState["mode"] = null;
                        ViewState["mode"] = "I";
                        this.makeEmptyForm("A");
                        this.NextNumber();
                        btntxtDOC_NO.Enabled = false;
                        setFocusControl(txtDOC_DATE);
                    }
                    txtFromDate.Text = DateTime.Parse(clsGV.Start_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Parse(clsGV.End_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                }
                else
                {
                    Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
                }
            }
            if (objAsp != null)
                System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(objAsp);
            if (hdnfClosePopup.Value == "Close" || hdnfClosePopup.Value == "")
            {
                pnlPopup.Style["display"] = "none";
            }
            else
            {
                pnlPopup.Style["display"] = "block";
                objAsp = btnSearch;
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [getMaxCode]
    private void getMaxCode()
    {
        try
        {
            DataSet ds = null;
            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                obj.tableName = tblHead + " where  tran_type='" + trnType + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                obj.code = "doc_no";

                ds = new DataSet();
                ds = obj.getMaxCode();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ViewState["mode"] != null)
                            {
                                if (ViewState["mode"].ToString() == "I")
                                {
                                    txtdoc_no.Text = ds.Tables[0].Rows[0][0].ToString();
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [makeEmptyForm]
    private void makeEmptyForm(string dAction)
    {
        try
        {
            if (dAction == "N")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = false;
                    }
                    if (c is System.Web.UI.WebControls.Label)
                    {
                        ((System.Web.UI.WebControls.Label)c).Text = "";
                    }
                }
                foreach (System.Web.UI.Control c in pnlVoucherEntries.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = false;
                    }
                }
                btntxtGetpassGstStateCode.Enabled = false;
                btntxtVoucherbyGstStateCode.Enabled = false;
                btntxtSalebilltoGstStateCode.Enabled = false;
                btntxtMillGstStateCode.Enabled = false;
                btntxtTransportGstStateCode.Enabled = false;
                btntxtGstRate.Enabled = false;
                txtEditDoc_No.Enabled = true;
                txtcarporateSale.Enabled = false;
                txtSaleBillTo.Enabled = false;
                txtNARRATION4.Enabled = false;
                txtMillMobile.Enabled = true;
                txtMillMobile.Text.Trim();
                pnlVoucherEntries.Style["display"] = "none";
                pnlPopup.Style["display"] = "none";
                btnOpenDetailsPopup.Enabled = false;
                btnSave.Text = "Save";
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = false;
                ddlFrieghtType.Enabled = false;
                lblMsg.Text = string.Empty;
                //txtPartyCommission.Enabled = false;
                drpCC.Enabled = false;
                btnVoucherOtherAmounts.Enabled = false;
                txtitem_Code.Enabled = false;
                btntxtitem_Code.Enabled = false;

                txtBill_To.Enabled = false;
                btntxtbill_To.Enabled = false;
                txtMillInv_Date.Enabled = false;
                chkInv_Chk.Enabled = false;

                #region Logic
                CalendarExtender2.Enabled = false;
                calenderExtenderDate.Enabled = false;
                CalendarExtender1.Enabled = false;
                drpDOType.Enabled = false;
                drpDeliveryType.Enabled = false;
                btntxtMILL_CODE.Enabled = false;
                btntxtGETPASS_CODE.Enabled = false;
                btntxtvoucher_by.Enabled = false;
                btntxtVasuliAc.Enabled = false;
                btntxtGRADE.Enabled = false;
                btntxtDO_CODE.Enabled = false;
                btntxtBroker_CODE.Enabled = false;
                btntxtTRANSPORT_CODE.Enabled = false;
                btntxtNARRATION1.Enabled = false;
                btntxtNARRATION2.Enabled = false;
                btntxtNARRATION3.Enabled = false;
                btntxtNARRATION4.Enabled = false;
                btntxtPurcNo.Enabled = false;
                drpDeliveryType.Enabled = false;
                lblUTRYearCode.Text = string.Empty;
                lblCSYearCode.Text = string.Empty;
                btntxtUTRNo.Enabled = false;
                btntxtcarporateSale.Enabled = false;
                //btnTransLetter.Enabled = true;
                btnWayBill.Enabled = true;
                btnMail.Enabled = true;
                btnOurDO.Enabled = true;
                btnPrintSaleBill.Enabled = true;
                //btnPrintCarpVoucher.Enabled = true;
                btnPrintMotorMemo.Enabled = true;
                //btnPrintITCVoc.Enabled = true;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                btnSendSms.Enabled = true;
                lblVoucherLedgerByBalance.Text = string.Empty;
                lblSaleBillToLedgerByBalance.Text = string.Empty;
                #endregion
            }
            if (dAction == "A")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                    }
                }
                foreach (System.Web.UI.Control c in pnlVoucherEntries.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                    }
                }

                btntxtGetpassGstStateCode.Enabled = false;
                txtGetpassGstStateCode.Enabled = false;
                txtVoucherbyGstStateCode.Enabled = false;
                txtSalebilltoGstStateCode.Enabled = false;
                txtTransportGstStateCode.Enabled = false;
                txtMillGstStateCode.Enabled = false;
                btntxtVoucherbyGstStateCode.Enabled = false;
                btntxtSalebilltoGstStateCode.Enabled = false;
                btntxtMillGstStateCode.Enabled = false;
                btntxtTransportGstStateCode.Enabled = false;
                lbltxtGetpassGstStateName.Text = "";
                lbltxtVoucherbyGstStateName.Text = "";
                lbltxtSalebilltoGstStateName.Text = "";
                btntxtGstRate.Enabled = true;
                lblVoucherLedgerByBalance.Text = string.Empty;
                lblSaleBillToLedgerByBalance.Text = string.Empty;
                txtEditDoc_No.Enabled = false;
                txtcarporateSale.Enabled = true;
                chkEWayBill.Checked = false;
                lblchkEWayBill.Text = string.Empty;
                //txtSaleBillTo.Enabled = false;

                txtNARRATION4.Enabled = false;
                txtMillMobile.Enabled = true;
                txtMillMobile.Text.Trim();
                drpCC.Enabled = true;
                btnSendSms.Enabled = false;
                btnSave.Text = "Save";
                btntxtDOC_NO.Text = "Change No";
                btntxtDOC_NO.Enabled = true;
                txtitem_Code.Enabled = true;
                btntxtitem_Code.Enabled = true;

                txtMillInv_Date.Enabled = true;
                chkInv_Chk.Enabled = true;
                //drpDeliveryType.Enabled = true;
                #region set Business logic for save
                CalendarExtender2.Enabled = true;
                calenderExtenderDate.Enabled = true;
                CalendarExtender1.Enabled = true;
                btnVoucherOtherAmounts.Enabled = true;
                drpDOType.Enabled = true;
                // btnTransLetter.Enabled = false;
                btnWayBill.Enabled = false;
                drpDeliveryType.Enabled = true;
                btntxtMILL_CODE.Enabled = true;
                btntxtGETPASS_CODE.Enabled = true;
                btntxtvoucher_by.Enabled = true;
                btntxtVasuliAc.Enabled = true;
                btntxtGRADE.Enabled = true;
                btntxtDO_CODE.Enabled = true;
                btntxtBroker_CODE.Enabled = true;
                btntxtTRANSPORT_CODE.Enabled = true;
                btntxtNARRATION1.Enabled = true;
                btntxtNARRATION2.Enabled = true;
                btntxtNARRATION3.Enabled = true;
                btntxtNARRATION4.Enabled = true;
                btnOpenDetailsPopup.Enabled = true;
                txtdoc_no.Enabled = false;
                btntxtPurcNo.Enabled = true;
                lblMillAmount.Text = string.Empty;
                lbltxtVasuliAc.Text = string.Empty;
                LBLMILL_NAME.Text = string.Empty;
                LBLGETPASS_NAME.Text = string.Empty;
                lblvoucherbyname.Text = string.Empty;
                LBLBROKER_NAME.Text = string.Empty;
                LBLDO_NAME.Text = string.Empty;
                LBLTRANSPORT_NAME.Text = string.Empty;
                lblDiffrate.Text = string.Empty;
                lblMemoNo.Text = "";
                lblVoucherNo.Text = "";
                lblVoucherType.Text = "";
                //lblFreight.Text = "";
                ddlFrieghtType.Enabled = true;
                ddlFrieghtType.SelectedIndex = 0;
                lblMsg.Text = "";
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                ViewState["currentTable"] = null;
                txtUTRNo.Text = string.Empty;
                txtUTRNo.Enabled = false;
                lblUTRYearCode.Text = string.Empty;
                lblCSYearCode.Text = string.Empty;
                btntxtUTRNo.Enabled = true;
                btntxtcarporateSale.Enabled = true;
                btnMail.Enabled = false;
                btnPrintSaleBill.Enabled = false;
                btnOurDO.Enabled = false;
                // btnPrintCarpVoucher.Enabled = false;
                btnPrintMotorMemo.Enabled = false;
                // btnPrintITCVoc.Enabled = false;
                txtDOC_DATE.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtPurchase_Date.Text = txtDOC_DATE.Text;

                txtBill_To.Enabled = false;
                btntxtbill_To.Enabled = false;
                txtMillInv_Date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                setFocusControl(txtDOC_DATE);
                #endregion
            }
            if (dAction == "S")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = false;
                    }
                }
                foreach (System.Web.UI.Control c in pnlVoucherEntries.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = false;
                    }
                }
                btntxtGetpassGstStateCode.Enabled = false;
                btntxtVoucherbyGstStateCode.Enabled = false;
                btntxtSalebilltoGstStateCode.Enabled = false;
                btntxtMillGstStateCode.Enabled = false;
                btntxtTransportGstStateCode.Enabled = false;
                btntxtGstRate.Enabled = false;
                btntxtVasuliAc.Enabled = false;
                txtEditDoc_No.Enabled = true;
                txtcarporateSale.Enabled = false;
                txtSaleBillTo.Enabled = false;
                txtNARRATION4.Enabled = false;
                txtMillMobile.Enabled = true;
                txtMillMobile.Text.Trim();
                drpCC.Enabled = false;
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = false;
                txtitem_Code.Enabled = false;
                btntxtitem_Code.Enabled = false;

                txtBill_To.Enabled = false;
                btntxtbill_To.Enabled = false;

                txtMillInv_Date.Enabled = false;
                chkInv_Chk.Enabled = false;
                #region Logic
                CalendarExtender2.Enabled = false;
                btnVoucherOtherAmounts.Enabled = false;
                calenderExtenderDate.Enabled = false;
                CalendarExtender1.Enabled = false;
                drpDOType.Enabled = false;
                drpDeliveryType.Enabled = false;
                btntxtMILL_CODE.Enabled = false;
                btntxtGETPASS_CODE.Enabled = false;
                btntxtvoucher_by.Enabled = false;
                btntxtGRADE.Enabled = false;
                btntxtDO_CODE.Enabled = false;
                btntxtBroker_CODE.Enabled = false;
                btntxtTRANSPORT_CODE.Enabled = false;
                btntxtNARRATION1.Enabled = false;
                btntxtNARRATION2.Enabled = false;
                btntxtNARRATION3.Enabled = false;
                btntxtNARRATION4.Enabled = false;
                btnOpenDetailsPopup.Enabled = false;
                btntxtPurcNo.Enabled = false;
                btnMail.Enabled = true;
                drpDeliveryType.Enabled = false;
                btntxtUTRNo.Enabled = false;
                btntxtcarporateSale.Enabled = false;
                // btnTransLetter.Enabled = true;
                btnWayBill.Enabled = true;
                ddlFrieghtType.Enabled = false;
                btnOurDO.Enabled = true;
                btnPrintSaleBill.Enabled = true;
                // btnPrintCarpVoucher.Enabled = true;
                btnPrintMotorMemo.Enabled = true;
                // btnPrintITCVoc.Enabled = true;
                btnSendSms.Enabled = true;
                #endregion
            }
            if (dAction == "E")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;

                        if (((System.Web.UI.WebControls.TextBox)c).Text == "0.00")
                        {
                            ((System.Web.UI.WebControls.TextBox)c).Text = string.Empty;
                        }
                    }
                }
                foreach (System.Web.UI.Control c in pnlVoucherEntries.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;

                        if (((System.Web.UI.WebControls.TextBox)c).Text == "0.00")
                        {
                            ((System.Web.UI.WebControls.TextBox)c).Text = string.Empty;
                        }
                    }
                }
                btntxtGetpassGstStateCode.Enabled = true;
                btntxtVoucherbyGstStateCode.Enabled = true;
                btntxtSalebilltoGstStateCode.Enabled = true;
                btntxtMillGstStateCode.Enabled = true;
                btntxtTransportGstStateCode.Enabled = true;
                btntxtGstRate.Enabled = true;
                btntxtVasuliAc.Enabled = true;
                txtEditDoc_No.Enabled = false;
                txtitem_Code.Enabled = true;
                btntxtitem_Code.Enabled = true;
                //txtSaleBillTo.Enabled = false;

                txtNARRATION4.Enabled = false;
                txtcarporateSale.Enabled = true;
                txtMillMobile.Enabled = true;
                txtMillMobile.Text.Trim();
                btnVoucherOtherAmounts.Enabled = true;
                drpCC.Enabled = true;
                btnSendSms.Enabled = true;
                hdnfpacking.Value = "1";
                btntxtDOC_NO.Text = "Choose No";
                btntxtDOC_NO.Enabled = true;
                lblMsg.Text = string.Empty;
                drpDeliveryType.Enabled = true;


                txtBill_To.Enabled = false;
                btntxtbill_To.Enabled = false;
                #region set Business logic for edit
                CalendarExtender2.Enabled = true;
                calenderExtenderDate.Enabled = true;
                CalendarExtender1.Enabled = true;
                drpDOType.Enabled = false;
                btntxtMILL_CODE.Enabled = true;
                btntxtGETPASS_CODE.Enabled = true;
                btntxtvoucher_by.Enabled = true;
                btntxtGRADE.Enabled = true;
                btntxtDO_CODE.Enabled = true;
                btntxtBroker_CODE.Enabled = true;
                btntxtTRANSPORT_CODE.Enabled = true;
                btntxtNARRATION1.Enabled = true;
                btntxtNARRATION2.Enabled = true;
                btntxtNARRATION3.Enabled = true;
                btntxtNARRATION4.Enabled = true;
                btnOpenDetailsPopup.Enabled = true;
                btntxtPurcNo.Enabled = true;
                txtUTRNo.Text = string.Empty;
                txtUTRNo.Enabled = false;
                lblUTRYearCode.Text = string.Empty;
                btntxtUTRNo.Enabled = true;
                btnPrintSaleBill.Enabled = false;
                btntxtcarporateSale.Enabled = false;
                btnMail.Enabled = false;
                // btnTransLetter.Enabled = false;
                btnWayBill.Enabled = false;
                btnOurDO.Enabled = false;
                ddlFrieghtType.Enabled = true;
                // btnPrintCarpVoucher.Enabled = false;
                btnPrintMotorMemo.Enabled = false;
                // btnPrintITCVoc.Enabled = false;
                string ov = lblVoucherNo.Text.ToString();
                string sb = lblSB_No.Text != string.Empty ? lblSB_No.Text : "0";
                if (ov == "0" || sb == "0")
                {
                    txtmillRate.ReadOnly = false;
                }
                else
                {
                    txtmillRate.ReadOnly = true;
                }
                #endregion

                txtMillGstStateCode.Enabled = false;
                txtGetpassGstStateCode.Enabled = false;
                txtVoucherbyGstStateCode.Enabled = false;
                txtSalebilltoGstStateCode.Enabled = false;
                txtTransportGstStateCode.Enabled = false;

                txtMillInv_Date.Enabled = true;
                chkInv_Chk.Enabled = true;
            }
            #region Always check this
            string s_item = "";
            s_item = drpDOType.SelectedValue;
            if (dAction == "E" || dAction == "A")
            {
                if (s_item == "DI")
                {
                    pnlgrdDetail.Enabled = true;
                    btnOpenDetailsPopup.Enabled = true;
                    btntxtUTRNo.Enabled = true;
                    //txtUTRNo.Enabled = true;
                    //grdDetail.DataSource = null;
                    //grdDetail.DataBind();
                }
                else
                {
                    pnlgrdDetail.Enabled = false;
                    btnOpenDetailsPopup.Enabled = false;
                    grdDetail.DataSource = null;
                    grdDetail.DataBind();

                    txtUTRNo.Text = "";
                    lblUTRYearCode.Text = "";
                    //btntxtUTRNo.Enabled = false;
                    //txtUTRNo.Enabled = false;
                }
            }
            #endregion
            txtPurcNo.Enabled = false;
            txtPurcOrder.Enabled = false;
        }
        catch
        {
        }
    }
    #endregion

    #region [showLastRecord]
    private void showLastRecord()
    {
        try
        {
            string qry = string.Empty;

            qry = getDisplayQuery();
            bool recordExist = this.fetchRecord(qry);
            if (recordExist == true)
            {
                btnAdd.Focus();
            }
            else                     //new code
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }

        }
        catch
        {
        }
    }
    #endregion

    #region [grdDetail_RowDataBound]
    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int i = 0;
            // if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(6);
                e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(7);
                e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(6);
                e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(6);
                e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(12);
                e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(35);
                e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(30);
                e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(14);
                e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[9].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[10].ControlStyle.Width = Unit.Percentage(15);
                e.Row.Cells[11].ControlStyle.Width = Unit.Percentage(20);
                e.Row.Cells[12].ControlStyle.Width = Unit.Percentage(20);
                e.Row.Cells[13].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[8].Visible = true;
                e.Row.Cells[9].Visible = true;
                e.Row.Cells[10].Visible = true;
                e.Row.Cells[11].Visible = true;

                e.Row.Cells[5].Style["overflow"] = "hidden";
                e.Row.Cells[6].Style["overflow"] = "hidden";


                i++;
                foreach (TableCell cell in e.Row.Cells)
                {
                    string s = cell.Text.ToString();
                    if (cell.Text.Length > 33)
                    {
                        cell.Text = cell.Text.Substring(0, 33) + "...";
                        cell.ToolTip = s;
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int i = 0;
            string v = hdnfClosePopup.Value;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                if (v == "txtitem_Code")
                {
                    e.Row.Cells[0].Width = new Unit("50px");
                    e.Row.Cells[1].Width = new Unit("150px");
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                }
                if (v == "txtGRADE")
                {
                    e.Row.Cells[0].Width = new Unit("400px");
                }
                if (v == "txtMILL_CODE" || v == "txtGETPASS_CODE" || v == "txtvoucher_by" || v == "txtBroker_CODE" || v == "txtDO_CODE" || v == "txtTRANSPORT_CODE" || v == "txtVasuliAc")
                {

                    e.Row.Cells[0].Width = new Unit("90px");
                    e.Row.Cells[1].Width = new Unit("400px");
                    e.Row.Cells[2].Width = new Unit("150px");
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                }
                if (v == "txtdoc_no" || v == "txtEditDoc_No")
                {

                    e.Row.Cells[0].Width = new Unit("50px");
                    e.Row.Cells[1].Width = new Unit("150px");
                    e.Row.Cells[2].Width = new Unit("100px");
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    //e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(60);
                    //e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                }
                if (v == "txtdoc_no" || v == "txtEditDoc_No")
                {

                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(20);
                    e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(20);
                    e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(25);
                    e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(5);

                    i++;
                    //foreach (TableCell cell in e.Row.Cells)
                    //{
                    //    string s = cell.Text;
                    //    if (cell.Text.Length > 20)
                    //    {
                    //        cell.Text = cell.Text.Substring(0, 20) + "(..)";
                    //        cell.ToolTip = s;
                    //    }

                    //}
                }
                if (v == "txtMillGstStateCode" || v == "txtGetpassGstStateCode" || v == "txtVoucherbyGstStateCode" || v == "txtSalebilltoGstStateCode" || v == "txtTransportGstStateCode")
                {
                    e.Row.Cells[0].Width = new Unit("100px");
                    e.Row.Cells[1].Width = new Unit("400px");
                }
                if (v == "txtcarporateSale")
                {

                    e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(8);
                    e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(20);
                    e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(15);
                    e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
                    i++;
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        string s = cell.Text;
                        if (cell.Text.Length > 30)
                        {
                            cell.Text = cell.Text.Substring(0, 30) + "(..)";
                            cell.ToolTip = s;
                        }

                    }
                }

                if (v == "txtPurcNo")
                {


                    e.Row.Cells[0].ControlStyle.Width = new Unit("50px");
                    e.Row.Cells[1].ControlStyle.Width = new Unit("60px");
                    e.Row.Cells[2].ControlStyle.Width = new Unit("140px");
                    e.Row.Cells[3].ControlStyle.Width = new Unit("140px");
                    e.Row.Cells[4].ControlStyle.Width = new Unit("80px");
                    e.Row.Cells[5].ControlStyle.Width = new Unit("120px");

                    e.Row.Cells[6].ControlStyle.Width = new Unit("90px");
                    e.Row.Cells[7].ControlStyle.Width = new Unit("100px");
                    e.Row.Cells[8].ControlStyle.Width = new Unit("90px");
                    e.Row.Cells[9].ControlStyle.Width = new Unit("90px");
                    e.Row.Cells[10].ControlStyle.Width = new Unit("140px");
                    e.Row.Cells[11].ControlStyle.Width = new Unit("90px");

                    e.Row.Cells[12].ControlStyle.Width = new Unit("80px");
                    e.Row.Cells[13].ControlStyle.Width = new Unit("90px");


                    //grdPopup.Style["table-layout"] = "auto";
                    //grdPopup.CellSpacing = 10;

                    //i++;
                    //foreach (TableCell cell in e.Row.Cells)
                    //{
                    //    string s = cell.Text;
                    //    if (cell.Text.Length > 25)
                    //    {
                    //        cell.Text = cell.Text.Substring(0, 25) + "(..)";
                    //        cell.ToolTip = s;
                    //    }

                    //}
                }
                if (v == "txtGstRate")
                {
                    e.Row.Cells[0].ControlStyle.Width = new Unit("50px");
                    e.Row.Cells[1].ControlStyle.Width = new Unit("200px");
                    e.Row.Cells[2].ControlStyle.Width = new Unit("90px");
                    e.Row.Cells[3].ControlStyle.Width = new Unit("90px");
                    e.Row.Cells[4].ControlStyle.Width = new Unit("90px");
                    e.Row.Cells[5].ControlStyle.Width = new Unit("90px");
                }
                if (v == "txtUTRNo")
                {

                    e.Row.Cells[0].ControlStyle.Width = new Unit("40px");
                    e.Row.Cells[1].ControlStyle.Width = new Unit("240px");
                    e.Row.Cells[2].ControlStyle.Width = new Unit("440px");
                    e.Row.Cells[3].ControlStyle.Width = new Unit("150px");
                    e.Row.Cells[4].ControlStyle.Width = new Unit("150px");
                    e.Row.Cells[5].ControlStyle.Width = new Unit("150px");
                    e.Row.Cells[6].ControlStyle.Width = new Unit("40px");
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;

                    i++;
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        string s = cell.Text;
                        if (cell.Text.Length > 50)
                        {
                            cell.Text = cell.Text.Substring(0, 50) + "(..)";
                            cell.ToolTip = s;
                        }

                    }
                }

            }
            //e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
        }
        catch
        {

        }
    }
    #endregion

    #region [grdPopup_RowCreated]
    protected void grdPopup_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            int pgCount = 0;
            pgCount = grdPopup.PageCount;
            if (e.Row.RowType == DataControlRowType.DataRow &&
               (e.Row.RowState == DataControlRowState.Normal ||
                e.Row.RowState == DataControlRowState.Alternate))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex, pgCount);
                e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event);";
                e.Row.Attributes["onselectstart"] = "javascript:return false;";
                // e.Row.Attributes["onkeyup"] = "javascript:return selectRow(event);";
            }
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region [RowCommand]
    protected void grdDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int rowindex = row.RowIndex;
            if (e.CommandArgument == "lnk")
            {
                switch (e.CommandName)
                {
                    case "EditRecord":
                        if (grdDetail.Rows[rowindex].Cells[12].Text != "D" && grdDetail.Rows[rowindex].Cells[12].Text != "R")
                        {
                            pnlPopupDetails.Style["display"] = "block";
                            this.showDetailsRow(grdDetail.Rows[rowindex]);
                            btnAdddetails.Text = "Update";
                        }
                        break;
                    case "DeleteRecord":
                        string action = "";
                        LinkButton lnkDelete = (LinkButton)e.CommandSource;
                        if (lnkDelete.Text == "Delete")
                        {
                            action = "Delete";
                            lnkDelete.Text = "Open";
                        }
                        else
                        {
                            action = "Open";
                            lnkDelete.Text = "Delete";
                        }
                        this.DeleteDetailsRow(grdDetail.Rows[rowindex], action);
                        break;
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [enableDisableNavigateButtons]
    //private void enableDisableNavigateButtons()
    //{
    //    #region enable disable previous next buttons
    //    int RecordCount = 0;
    //    string query = "";
    //    query = "select count(*) from " + tblHead + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  and tran_type='" + trnType + "'";
    //    DataSet ds = new DataSet();
    //    DataTable dt = new DataTable();
    //    ds = clsDAL.SimpleQuery(query);
    //    if (ds != null)
    //    {
    //        if (ds.Tables.Count > 0)
    //        {
    //            dt = ds.Tables[0];
    //            if (dt.Rows.Count > 0)
    //            {
    //                RecordCount = Convert.ToInt32(dt.Rows[0][0].ToString());
    //            }
    //        }
    //    }
    //    if (RecordCount != 0 && RecordCount == 1)
    //    {
    //        btnFirst.Enabled = true;
    //        btnPrevious.Enabled = false;
    //        btnNext.Enabled = false;
    //        btnLast.Enabled = false;
    //    }
    //    else if (RecordCount != 0 && RecordCount > 1)
    //    {
    //        btnFirst.Enabled = true;
    //        btnPrevious.Enabled = false;
    //        btnNext.Enabled = false;
    //        btnLast.Enabled = true;
    //    }
    //    if (txtdoc_no.Text != string.Empty)
    //    {
    //        if (hdnf.Value != string.Empty)
    //        {
    //            #region check for next or previous record exist or not
    //            ds = new DataSet();
    //            dt = new DataTable();
    //            query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "' ORDER BY doc_no asc  ";
    //            ds = clsDAL.SimpleQuery(query);
    //            if (ds != null)
    //            {
    //                if (ds.Tables.Count > 0)
    //                {
    //                    dt = ds.Tables[0];
    //                    if (dt.Rows.Count > 0)
    //                    {
    //                        //next record exist
    //                        btnNext.Enabled = true;
    //                        btnLast.Enabled = true;
    //                    }
    //                    else
    //                    {
    //                        //next record does not exist
    //                        btnNext.Enabled = false;
    //                        btnLast.Enabled = false;
    //                    }
    //                }
    //            }
    //            ds = new DataSet();
    //            dt = new DataTable();
    //            query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  and tran_type='" + trnType + "' ORDER BY doc_no asc  ";
    //            ds = clsDAL.SimpleQuery(query);
    //            if (ds != null)
    //            {
    //                if (ds.Tables.Count > 0)
    //                {
    //                    dt = ds.Tables[0];
    //                    if (dt.Rows.Count > 0)
    //                    {
    //                        //previous record exist
    //                        btnPrevious.Enabled = true;
    //                        btnFirst.Enabled = true;
    //                    }
    //                    else
    //                    {
    //                        btnPrevious.Enabled = false;
    //                        btnFirst.Enabled = false;
    //                    }
    //                }
    //            }
    //            #endregion
    //        }
    //    }

    //    #endregion
    //}
    #endregion

    #region [First]
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select doc_no from " + tblHead + "  where doc_no=(select MIN(doc_no) from " + tblHead + "  where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "') and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                  "  and tran_type='" + trnType + "'";
            hdnf.Value = clsCommon.getString(query);
            navigateRecord();
        }
        catch
        {
        }
    }
    #endregion

    #region [Previous]
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtdoc_no.Text != string.Empty)
            {
                string query = "SELECT top 1 [doc_no] from " + tblHead + "  where doc_no<" + Convert.ToInt32(hdnf.Value) +
                    "  and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and  Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  and tran_type='" + trnType + "'" +
                    " ORDER BY doc_no DESC  ";
                hdnf.Value = clsCommon.getString(query);
                navigateRecord();
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [Next]
    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtdoc_no.Text != string.Empty)
            {
                string query = "SELECT top 1 [doc_no] from " + tblHead + "  where doc_no>" + Convert.ToInt32(hdnf.Value) +
                    "  and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  and tran_type='" + trnType + "'" +
                    " ORDER BY doc_no asc  ";
                hdnf.Value = clsCommon.getString(query);
                navigateRecord();
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [Last]
    protected void btnLast_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select doc_no from " + tblHead + "  where doc_no=(select MAX(doc_no) from " + tblHead + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "') and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  and tran_type='" + trnType + "'";

            hdnf.Value = clsCommon.getString(query);
            navigateRecord();
        }
        catch
        {
        }
    }
    #endregion

    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        drpDOType.SelectedValue = "DI";
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("A");
        this.getMaxCode();
        pnlPopupDetails.Style["display"] = "none";
        lblPDSParty.Text = "";
        lblSB_No.Text = "0";
        txtGstRate.Text = "1";
        this.NextNumber();
        lblGstRateName.Text = clsCommon.getString("Select GST_Name from " + tblPrefix + "GSTRateMaster where Doc_no=1");
        hdnfOldBillAmt.Value = "0";

    }
    #endregion

    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        if (lblMsg.Text != "Delete")
        {
            ViewState["mode"] = null;
            ViewState["mode"] = "U";
            clsButtonNavigation.enableDisable("E");
            pnlgrdDetail.Enabled = true;
            this.makeEmptyForm("E");
            txtdoc_no.Enabled = false;
            hdnfpacking.Value = "2";
            hdnfQty.Value = txtquantal.Text;


            PreQntl();
            //carporatesale();

            int vn = lblVoucherNo.Text.Trim() != string.Empty ? Convert.ToInt32(lblVoucherNo.Text) : 0;
            int sbn = lblSB_No.Text.Trim() != string.Empty ? Convert.ToInt32(lblSB_No.Text) : 0;
            if (vn != 0)
            {
                txtmillRate.ReadOnly = true;
            }
            else if (sbn != 0)
            {
                txtmillRate.ReadOnly = true;
            }
            else
            {
                txtmillRate.ReadOnly = false;
            }
            txtEditDoc_No.Text = string.Empty;



            OldQty = txtquantal.Text != string.Empty ? Convert.ToDouble(txtquantal.Text) : 0;

            OldSaleAmt = txtSALE_RATE.Text != string.Empty ? Convert.ToDouble(txtSALE_RATE.Text) : 0;
            OldCommission = txtCommission.Text != string.Empty ? Convert.ToDouble(txtCommission.Text) : 0;
            OldGSTAmt = (OldSaleAmt * 5) / 100;
            OldBillAmt = (OldGSTAmt + OldSaleAmt + OldCommission) * OldQty;
            hdnfOldBillAmt.Value = OldBillAmt.ToString();
        }
    }

    private void PreQntl()
    {
        if (txtcarporateSale.Text != "0" || !string.IsNullOrWhiteSpace(txtcarporateSale.Text))
        {
            ViewState["PreQntl"] = txtquantal.Text.ToString();
        }
    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                int flag = 3;
                purchase = new PurchaseFields();
                AUTO_VOUCHER = clsCommon.getString("select AutoVoucher from " + tblPrefix + "CompanyParameters where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");

                GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='" + drpDOType.SelectedValue + "' and Doc_No=" + txtdoc_no.Text + " and " +
                    " COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = GLEDGER_Delete;
                Maindt.Rows.Add(dr);
                if (AUTO_VOUCHER == "YES")
                {
                    if (lblVoucherType.Text == "PS")
                    {
                        purchase.PS_doc_no = Convert.ToInt32(lblVoucherNo.Text);
                        purchase.PS_PURCNO = Convert.ToInt32(txtPurcNo.Text);
                        purchase.PS_Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
                        purchase.PS_Year_Code = Convert.ToInt32(Session["year"].ToString());
                        purchase.PS_purchase_Id = Convert.ToInt32(clsCommon.getString("select purchaseid from nt_1_sugarpurchase where doc_no=" + lblVoucherNo.Text + " " +
                            " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + ""));
                        DataTable dt = (DataTable)ViewState["currentTable"];

                        SalePurcdt = new DataTable();
                        SalePurcdt = clsPurchase_Posting.Purchase_Posting(flag, purchase, "PS", dt);
                        Maindt.Merge(SalePurcdt);
                    }
                    else
                    {
                        LV = new LocalVoucher();
                        LV.LV_Doc_No = Convert.ToInt32(lblVoucherNo.Text);
                        LV.LV_Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
                        LV.LV_Year_Code = Convert.ToInt32(Session["year"].ToString());
                        LV.LV_commissionid = Convert.ToInt32(clsCommon.getString("select commissionid from commission_bill where doc_no=" + lblVoucherNo.Text + " " +
                            "and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + ""));

                        SalePurcdt = new DataTable();
                        SalePurcdt = clsLocalVoucher.LV_Posting(flag, LV, "LV");
                        Maindt.Merge(SalePurcdt);
                    }

                }
                qry = "select doc_no from nt_1_sugarsale where DO_No=" + txtdoc_no.Text + " and " +
                    " Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + "";
                int no = Convert.ToInt32(lblSB_No.Text != string.Empty ? lblSB_No.Text : "0");
                if (no == 0)
                {
                    no = Convert.ToInt32(clsCommon.getString(qry));
                    lblSB_No.Text = no.ToString();
                }
                if (no != 0)
                {
                    SaleFields sale = new SaleFields();
                    sale.SB_doc_no = Convert.ToInt32(lblSB_No.Text);
                    sale.SB_Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
                    sale.SB_Year_Code = Convert.ToInt32(Session["year"].ToString());

                    sale.SB_Sale_Id = Convert.ToInt32(clsCommon.getString("select saleid from nt_1_sugarsale where doc_no=" + lblSB_No.Text + " " +
                        " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + ""));
                    DataTable dt1 = (DataTable)ViewState["currentTable"];

                    SalePurcdt = new DataTable();
                    SalePurcdt = clsSale_Posting.Sale_Posting(flag, sale, "SB", dt1);
                    Maindt.Merge(SalePurcdt);
                }

                // Head_Delete = "delete from " + tblHead + " where  doid='" + lbldoid.Text + "'  and Company_code='" + Session["Company_Code"].ToString() + "' and Year_Code='" + Session["year"].ToString() + "'";
                string Detail_Deleteqry = "delete from " + tblDetails + " where  doid='" + lbldoid.Text + "' and Company_code='" + Session["Company_Code"].ToString() + "' and Year_Code='" + Session["year"].ToString() + "'";
                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Detail_Deleteqry;
                Maindt.Rows.Add(dr);

                qry = "update " + tblHead + " set purc_no='0',purc_order='0',voucher_no='0',voucher_type='',SB_No='0',tenderdetailid=null,UTR_Year_Code=null,Carporate_Sale_No=null,cs=null,memo_no=0 " +
                    " where  doid='" + lbldoid.Text + "'  and Company_code='" + Session["Company_Code"].ToString() + "' and Year_Code='" + Session["year"].ToString() + "'";
                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = qry;
                Maindt.Rows.Add(dr);
                int count = 0;

                msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);

                if (msg == "Delete")
                {
                    Response.Redirect("../BussinessRelated/PgeDoHeadUtility.aspx");

                }

            }
            else
            {
                //lblMsg.Text = "Cannot delete this entry !";
                //lblMsg.ForeColor = System.Drawing.Color.Red;
            }
        }

        catch
        {
        }
    }
    #endregion

    #region [btnCancel_Click]
    protected void btnCancel_Click(object sender, EventArgs e)
    {

        hdnf.Value = Request.QueryString["DO"];
        if (hdnf.Value == "0")
        {
            hdnf.Value = clsCommon.getString("select max(doid) from nt_1_deliveryorder where Company_Code=" + Session["Company_Code"].ToString() + " and " +
                " Year_Code=" + Session["year"].ToString() + "");
        }
        clsButtonNavigation.enableDisable("S");
        this.makeEmptyForm("S");
        showLastRecord();
    }
    #endregion

    #region [fetchrecord]
    private bool fetchRecord(string qry)
    {
        try
        {
            bool recordExist = false;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtdoc_no.Text = dt.Rows[0]["DOC_NO"].ToString();
                        lbldoid.Text = dt.Rows[0]["doid"].ToString();
                        txtDOC_DATE.Text = dt.Rows[0]["doc_dateConverted"].ToString();
                        txtMillInv_Date.Text = dt.Rows[0]["mill_inv_dateConverted"].ToString();
                        Inv_Chk = dt.Rows[0]["mill_rcv"].ToString();
                        if (Inv_Chk == "Y")
                        {
                            chkInv_Chk.Checked = true;
                        }
                        else
                        {
                            chkInv_Chk.Checked = false;
                        }
                        txtMillEwayBill_No.Text = dt.Rows[0]["MillEwayBill"].ToString();
                        drpDOType.SelectedValue = dt.Rows[0]["DESP_TYPE"].ToString();
                        if (drpDOType.SelectedValue == "DI")
                        {
                            drpDeliveryType.Visible = true;
                            btngenratesalebill.Enabled = true;
                        }
                        else
                        {
                            drpDeliveryType.Visible = false;
                            btngenratesalebill.Enabled = false;
                        }
                        txtMILL_CODE.Text = dt.Rows[0]["MILL_CODE"].ToString();
                        LBLMILL_NAME.Text = dt.Rows[0]["millName"].ToString();
                        txtGstRate.Text = dt.Rows[0]["GstRateCode"].ToString();
                        lblGstRateName.Text = dt.Rows[0]["GST_Name"].ToString();
                        txtMillEmailID.Text = dt.Rows[0]["millemailid"].ToString();
                        txtMillMobile.Text = dt.Rows[0]["MobileNo"].ToString();
                        txtBill_To.Text = dt.Rows[0]["carporate_ac"].ToString();
                        lblBill_To.Text = dt.Rows[0]["carporateacname"].ToString();
                        string getpasscode = dt.Rows[0]["GETPASSCODE"].ToString();
                        // hdnf.Value = dt.Rows[0]["tenderdetailid"].ToString();
                        txtGETPASS_CODE.Text = getpasscode;
                        string getpasscodecitycode = clsCommon.getString("Select City_Code from " + qryAccountList + "  where Ac_Code=" + getpasscode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string getpasscity = clsCommon.getString("Select city_name_e from " + tblPrefix + "CityMaster  where city_code=" + getpasscodecitycode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        LBLGETPASS_NAME.Text = dt.Rows[0]["GetPassName"].ToString() + ", " + getpasscity;
                        txtGetpassGstStateCode.Text = dt.Rows[0]["GetpassGstStateCode"].ToString();
                        lbltxtGetpassGstStateName.Text = dt.Rows[0]["getpassstatename"].ToString();
                        txtVoucherbyGstStateCode.Text = dt.Rows[0]["VoucherbyGstStateCode"].ToString();
                        lbltxtVoucherbyGstStateName.Text = clsCommon.getString("select State_Name from gststatemaster where State_Code=" + txtVoucherbyGstStateCode.Text + "");
                        //lbltxtVoucherbyGstStateName.Text = dt.Rows[0]["gstmillstatename"].ToString();
                        txtSalebilltoGstStateCode.Text = dt.Rows[0]["SalebilltoGstStateCode"].ToString();
                        lbltxtSalebilltoGstStateName.Text = dt.Rows[0]["gststatesellbillname"].ToString();

                        txtMillGstStateCode.Text = dt.Rows[0]["MillGSTStateCode"].ToString();
                        lbltxtMillGstStateCode.Text = dt.Rows[0]["gstmillstatename"].ToString();
                        txtTransportGstStateCode.Text = dt.Rows[0]["TransportGSTStateCode"].ToString();
                        lbltxtTransportGstStateCode.Text = dt.Rows[0]["gststatetransportname"].ToString();
                        string VoucherByCode = dt.Rows[0]["VOUCHER_BY"].ToString();
                        txtvoucher_by.Text = VoucherByCode;
                        lblVoucherLedgerByBalance.Text = AcBalance(VoucherByCode);
                        txtitem_Code.Text = dt.Rows[0]["itemcode"].ToString();
                        lblitem_Name.Text = dt.Rows[0]["itemname"].ToString();

                        lblvoucherbyname.Text = dt.Rows[0]["VoucherByname"].ToString();
                        string voucherbycitycode = clsCommon.getString("Select City_Code from " + tblPrefix + "AccountMaster  where Ac_Code=" + VoucherByCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string voucherbycity = clsCommon.getString("Select city_name_e from " + tblPrefix + "CityMaster  where city_code=" + voucherbycitycode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //lblvoucherbyname.Text = dt.Rows[0]["VoucherByname"].ToString() + ", " + voucherbycity;
                        txtGRADE.Text = dt.Rows[0]["GRADE"].ToString();
                        txtquantal.Text = dt.Rows[0]["QUANTAL"].ToString();
                        txtPACKING.Text = dt.Rows[0]["PACKING"].ToString();
                        txtBAGS.Text = dt.Rows[0]["BAGS"].ToString();
                        txtexcise_rate.Text = dt.Rows[0]["EXCISE_RATE"].ToString();
                        txtmillRate.Text = dt.Rows[0]["mill_rate"].ToString();
                        txtSALE_RATE.Text = dt.Rows[0]["SALE_RATE"].ToString();
                        txtCommission.Text = dt.Rows[0]["Tender_Commission"].ToString();
                        lblDiffrate.Text = dt.Rows[0]["DIFF_RATE"].ToString();
                        txtDIFF_AMOUNT.Text = dt.Rows[0]["DIFF_AMOUNT"].ToString();
                        txtDO_CODE.Text = dt.Rows[0]["DO"].ToString();
                        txtEWayBill_No.Text = dt.Rows[0]["EWay_Bill_No"].ToString();

                        LBLDO_NAME.Text = dt.Rows[0]["DOName"].ToString();
                        txtBroker_CODE.Text = dt.Rows[0]["BROKER"].ToString();
                        txtDistance.Text = dt.Rows[0]["Distance"].ToString();
                        LBLBROKER_NAME.Text = dt.Rows[0]["BrokerName"].ToString();
                        txtTruck_NO.Text = dt.Rows[0]["TRUCK_NO"].ToString();
                        txtTRANSPORT_CODE.Text = dt.Rows[0]["TRANSPORT"].ToString();
                        LBLTRANSPORT_NAME.Text = dt.Rows[0]["TransportName"].ToString();
                        txtNARRATION1.Text = dt.Rows[0]["NARRATION1"].ToString();
                        txtNARRATION2.Text = dt.Rows[0]["NARRATION2"].ToString();
                        txtNARRATION3.Text = dt.Rows[0]["NARRATION3"].ToString();
                        txtNARRATION4.Text = dt.Rows[0]["NARRATION4"].ToString();
                        txtNarration5.Text = dt.Rows[0]["NARRATION5"].ToString();
                        lblLoadingSms.Text = dt.Rows[0]["LoadingSms"].ToString();
                        txtPurcNo.Text = dt.Rows[0]["purc_no"].ToString();
                        txtPurchase_Date.Text = dt.Rows[0]["Purchase_DateConverted"].ToString();
                        txtPurcOrder.Text = dt.Rows[0]["purc_order"].ToString();
                        txtDriverMobile.Text = dt.Rows[0]["driver_no"].ToString();
                        // txtINVOICE_NO.Text = dt.Rows[0]["Invoice_No"].ToString();
                        txtVasuliRate1.Text = dt.Rows[0]["vasuli_rate1"].ToString();
                        txtVasuliAmount1.Text = dt.Rows[0]["vasuli_amount1"].ToString();
                        txtVasuliAc.Text = dt.Rows[0]["Vasuli_Ac"].ToString();
                        // string vByname = dt.Rows[0]["VasuliAcName"].ToString();
                        //string tooltip = vByname;
                        //if (vByname.Length > 25)
                        //{
                        //    vByname = vByname.Substring(0, vByname.Length - 25);
                        //}
                        //lbltxtVasuliAc.Text = vByname;
                        // lbltxtVasuliAc.ToolTip = tooltip;
                        //txtPartyCommission.Text = dt.Rows[0]["Party_Commission_Rate"].ToString();
                        txtMemoAdvanceRate.Text = dt.Rows[0]["MM_Rate"].ToString();
                        drpCC.SelectedValue = dt.Rows[0]["MM_CC"].ToString();
                        txtVoucherBrokrage.Text = dt.Rows[0]["Voucher_Brokrage"].ToString();
                        txtVoucherServiceCharge.Text = dt.Rows[0]["Voucher_Service_Charge"].ToString();
                        txtVoucherL_Rate_Diff.Text = dt.Rows[0]["Voucher_RateDiffRate"].ToString();
                        txtVoucherRATEDIFFAmt.Text = dt.Rows[0]["Voucher_RateDiffAmt"].ToString();
                        txtVoucherCommission_Rate.Text = dt.Rows[0]["Voucher_BankCommRate"].ToString();
                        txtVoucherBANK_COMMISSIONAmt.Text = dt.Rows[0]["Voucher_BankCommAmt"].ToString();
                        txtVoucherInterest.Text = dt.Rows[0]["Voucher_Interest"].ToString();
                        txtVoucherTransport_Amount.Text = dt.Rows[0]["Voucher_TransportAmt"].ToString();
                        txtVoucherOTHER_Expenses.Text = dt.Rows[0]["Voucher_OtherExpenses"].ToString();
                        //txtCheckPostName.Text = dt.Rows[0]["CheckPost"].ToString();
                        txtPanNo.Text = dt.Rows[0]["Pan_No"].ToString();
                        ddlFrieghtType.SelectedValue = dt.Rows[0]["WhoseFrieght"].ToString();
                        txtMillInvoiceno.Text = dt.Rows[0]["MillInvoiceNo"].ToString();

                        string ischecked = dt.Rows[0]["EWayBillChk"].ToString();
                        // chkEWayBill.Checked = dt.Rows[0]["EWayBill_Chk"].ToString();

                        if (ischecked == "Y")
                        {
                            chkEWayBill.Checked = true;
                            lblchkEWayBill.Text = LBLMILL_NAME.Text;
                            //string gstno = clsCommon.getString("select Gst_No from " + AccountMasterTable + " where Ac_Code=" + txtMILL_CODE.Text + "and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            //txtGStno.Text = gstno;
                        }
                        else
                        {
                            chkEWayBill.Checked = false;
                            //  txtGStno.Text = "27AABHJ9303C1ZM";
                            lblchkEWayBill.Text = "";
                        }

                        Label lblCreated = (Label)Master.FindControl("MasterlblCreatedBy");
                        Label lblModified = (Label)Master.FindControl("MasterlblModifiedBy");
                        if (lblCreated != null)
                        {
                            lblCreated.Text = "Created By: " + dt.Rows[0]["Created_By"].ToString();
                        }
                        if (lblModified != null)
                        {
                            lblModified.Text = "Modified By: " + dt.Rows[0]["Modified_By"].ToString();
                        }
                        txtFrieght.Text = dt.Rows[0]["FreightPerQtl"].ToString();
                        double frtAmount = Convert.ToDouble(dt.Rows[0]["Freight_Amount"].ToString());
                        txtFrieghtAmount.Text = frtAmount.ToString();

                        txtVasuliRate.Text = dt.Rows[0]["vasuli_rate"].ToString();
                        txtVasuliAmount.Text = dt.Rows[0]["vasuli_amount"].ToString();

                        double memoadvance = Convert.ToDouble(dt.Rows[0]["memo_advance"].ToString());
                        txtMemoAdvance.Text = memoadvance.ToString();

                        lblFrieghtToPay.Text = "Frieght To Pay: " + (frtAmount - memoadvance).ToString();

                        string CS_No = dt.Rows[0]["Carporate_Sale_No"].ToString();
                        txtcarporateSale.Text = CS_No;

                        PDS = clsCommon.getString("Select selling_type from carporatehead  where doc_no=" + CS_No + " and Company_Code=" +
                            Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        if (PDS == "P")
                        {
                            string prtyCode = clsCommon.getString("Select ac_code from carporatehead  where doc_no=" + CS_No + " and Company_Code="
                                + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            hdnfPDSPartyCode.Value = prtyCode;
                            string unitCode = clsCommon.getString("Select unit_code from carporatehead  where doc_no=" + CS_No +
                                " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            hdnfPDSUnitCode.Value = unitCode;
                            string nm = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster  where Ac_Code=" + prtyCode +
                                " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            lblPDSParty.Text = "<b Style=" + "color:black;" + ">Party:</b> " + nm;
                            btnPrintSaleBill.Visible = true;
                        }
                        else
                        {
                            hdnfPDSPartyCode.Value = "0";
                            lblPDSParty.Text = "";
                            btnPrintSaleBill.Enabled = true;
                        }
                        lblCSYearCode.Text = dt.Rows[0]["Carporate_Sale_Year_Code"].ToString();
                        lblMillAmount.Text = dt.Rows[0]["final_amout"].ToString();
                        if (txtcarporateSale.Text == "0")
                        {
                            txtcarporateSale.Text = "";
                        }
                        txtUTRNo.Text = dt.Rows[0]["Utr_No"].ToString();
                        lblUTRYearCode.Text = dt.Rows[0]["UTR_Year_Code"].ToString();
                        if (txtUTRNo.Text == "0")
                        {
                            txtUTRNo.Text = "";
                        }
                        hdnvouchernumber.Value = dt.Rows[0]["voucher_no"].ToString();
                        lblVoucherNo.Text = hdnvouchernumber.Value.TrimStart();
                        lblVoucherType.Text = dt.Rows[0]["voucher_type"].ToString();
                        hdnmemonumber.Value = dt.Rows[0]["memo_no"].ToString();
                        lblMemoNo.Text = hdnmemonumber.Value.TrimStart();
                        txtSaleBillTo.Text = dt.Rows[0]["SaleBillTo"].ToString();
                        lblSaleBillToLedgerByBalance.Text = AcBalance(txtSaleBillTo.Text);
                        string SB_No = dt.Rows[0]["SB_No"].ToString();

                        hdnfmc.Value = dt.Rows[0]["mc"].ToString();
                        hdnfgp.Value = dt.Rows[0]["gp"].ToString();
                        hdnfsb.Value = dt.Rows[0]["sb"].ToString();
                        hdnfst.Value = dt.Rows[0]["st"].ToString();
                        hdnftc.Value = dt.Rows[0]["tc"].ToString();
                        hdnfva.Value = dt.Rows[0]["va"].ToString();
                        hdnfbk.Value = dt.Rows[0]["bk"].ToString();
                        hdnfdocd.Value = dt.Rows[0]["docd"].ToString();
                        hdnfbt.Value = dt.Rows[0]["ca"].ToString();
                        hdnfic.Value = dt.Rows[0]["itemcode"].ToString();
                        hdnfcscode.Value = dt.Rows[0]["cs"].ToString();

                        hdnfmillshortname.Value = dt.Rows[0]["millshortname"].ToString();
                        hdnfsalebilltoshortname.Value = dt.Rows[0]["billtoshortname"].ToString();
                        hdnfshiptoshortname.Value = dt.Rows[0]["shiptoshortname"].ToString();
                        hdnftransportshortname.Value = dt.Rows[0]["transportshortname"].ToString();
                        hdnfgetpassshortname.Value = dt.Rows[0]["getpassshortname"].ToString();

                        hdnfbilltoshortname.Value = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster  where Ac_Code=" + txtBill_To.Text +
                              " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

                        if (!string.IsNullOrEmpty(SB_No))
                        {
                            if (SB_No != "0")
                            {
                                lblsbnol.Text = "Sale Bill No:";
                                lblSB_No.Text = SB_No;
                                hdnfSB_No.Value = SB_No;//"<b Style=" + "color:Black;" + ">Sale Bill No:</b> " +
                                btngenratesalebill.Enabled = false;
                            }
                            else
                            {
                                lblsbnol.Text = "";
                                lblSB_No.Text = "";
                                btnPrintSaleBill.Enabled = false;
                                btngenratesalebill.Enabled = true;
                            }

                        }
                        else
                        {
                            lblsbnol.Text = "";
                            lblSB_No.Text = "";
                            btnPrintSaleBill.Enabled = false;
                            btngenratesalebill.Enabled = true;
                        }
                        string DT = dt.Rows[0]["Delivery_Type"].ToString();
                        if (DT == "C")
                        {
                            drpDeliveryType.SelectedValue = "C";
                        }
                        else
                        {
                            drpDeliveryType.SelectedValue = "N";
                        }


                        if (txtEWayBill_No.Text != string.Empty)
                        {
                            if (txtEWayBill_No.Text != "0")
                            {
                                btnGentare_EWayBill.Enabled = false;
                            }
                            else
                            {
                                btnGentare_EWayBill.Enabled = true;
                            }
                        }
                        else
                        {
                            btnGentare_EWayBill.Enabled = true;
                        }
                        //lblFreight.Text = dt.Rows[0]["Freight_Amount"].ToString();
                        recordExist = true;
                        lblMsg.Text = "";
                        #region Deliverty order Details
                        qry = "select detail_Id as ID ,ddType as Type,Bank_Code,BankName, Narration,Amount,UTR_NO,LTNo,dodetailid,utrdetailid as UtrDetailId from " + qryDetail + "  where doid=" + hdnf.Value + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ";
                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    string idValue = dt.Rows[0]["ID"].ToString();
                                    if (idValue == "")  //blank Row
                                    {
                                        grdDetail.DataSource = null;
                                        grdDetail.DataBind();
                                        ViewState["currentTable"] = null;
                                    }
                                    else
                                    {
                                        dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                                        dt.Columns.Add(new DataColumn("SrNo", typeof(int)));
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            dt.Rows[i]["rowAction"] = "N";
                                            dt.Rows[i]["SrNo"] = i + 1;
                                        }
                                        grdDetail.DataSource = dt;
                                        grdDetail.DataBind();
                                        ViewState["currentTable"] = dt;
                                    }
                                }
                                else
                                {
                                    grdDetail.DataSource = null;
                                    grdDetail.DataBind();
                                    ViewState["currentTable"] = null;
                                }
                            }
                            else
                            {
                                grdDetail.DataSource = null;
                                grdDetail.DataBind();
                                ViewState["currentTable"] = null;
                            }
                        }
                        else
                        {
                            grdDetail.DataSource = null;
                            grdDetail.DataBind();
                            ViewState["currentTable"] = null;
                        }
                        #endregion
                        pnlgrdDetail.Enabled = false;
                    }
                }
            }
            PreQntl();
            if (txtPurcNo.Text == "0")
            {
                lblMsg.Text = "Delete";
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                btnCancel.Enabled = false;
            }
            if (drpDOType.SelectedValue == "DI")
            {
                btngenratesalebill.Enabled = true;
            }
            else
            {
                btngenratesalebill.Enabled = false;
            }
            return recordExist;
        }
        catch
        {
            return false;
        }
    }
    #endregion


    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            //string qryDisplay = "select *,CONVERT(varchar(10),doc_date,103) as doc_date1 from " + qryCommon + "  where doc_no='" + hdnf.Value + "' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and tran_type='" + trnType + "'";
            string qryDisplay = "select * from " + qryHead + "  where doid='" + hdnf.Value + "' ";
            return qryDisplay;
        }
        catch
        {
            return "";
        }
    }
    #endregion

    #region navigateRecord
    private void navigateRecord()
    {
        try
        {
            if (hdnf.Value != string.Empty)
            {
                ViewState["mode"] = "U";
                txtdoc_no.Text = hdnf.Value;
                hdnfSuffix.Value = "";
                string query = getDisplayQuery();
                clsButtonNavigation.enableDisable("N");
                bool recordExist = this.fetchRecord(query);
                if (recordExist == true)
                {
                    btnEdit.Enabled = true;
                    btnEdit.Focus();
                }
                //this.enableDisableNavigateButtons();
                this.makeEmptyForm("S");
            }
            else
            {
                showLastRecord();
            }
        }
        catch
        {

        }
    }
    #endregion

    #region [btnOpenDetailsPopup_Click]
    protected void btnOpenDetailsPopup_Click(object sender, EventArgs e)
    {
        btnAdddetails.Text = "ADD";
        pnlPopupDetails.Style["display"] = "block";
        txtBANK_CODE.Text = txtMILL_CODE.Text;
        //txtBANK_CODE.Text = txtSaleBillTo.Text;

        // lblBank_name.Text = LBLMILL_NAME.Text;
        lblBank_name.Text = LBLMILL_NAME.Text;
        //  double qntl = double.Parse(txtquantal.Text);

        // double millrate = double.Parse(txtmillRate.Text);

        //  double Mill_Amount = qntl * millrate;

        double Mill_Amount = double.Parse(lblMillAmount.Text != string.Empty ? lblMillAmount.Text : "0.00");

        if (grdDetail.Rows.Count > 0)
        {
            double total = 0.00;
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                if (grdDetail.Rows[i].Cells[10].Text.ToString() != "D")
                {
                    double amount = Convert.ToDouble(grdDetail.Rows[i].Cells[7].Text);
                    total += amount;
                }
            }
            if (Mill_Amount != total)
            {
                string BankAmt = Convert.ToString(Mill_Amount - total);
                hdnfMainBankAmount.Value = BankAmt;
                txtBANK_AMOUNT.Text = BankAmt;
            }

            else
            {
                txtBANK_AMOUNT.Text = Convert.ToString(Mill_Amount);
            }
        }
        setFocusControl(drpddType);
        lblUtrBalnceError.Text = "";
        double millamount = txtmillRate.Text != string.Empty ? Convert.ToDouble(txtmillRate.Text) : 0.00;
        double tax = 5.00;
        double result = 0.00;
        tax = tax / 100;
        result = millamount * tax;
        //txtwithGst_Amount.Text = result.ToString();
        txtwithGst_Amount.Text = (millamount + result).ToString();
    }
    #endregion

    #region [btnAdddetails_Click]
    protected void btnAdddetails_Click(object sender, EventArgs e)
    {
        Int32 UTR_No = txtUTRNo.Text != string.Empty ? Convert.ToInt32(txtUTRNo.Text) : 0;
        try
        {
            int rowIndex = 1;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt = new DataTable();
            if (ViewState["currentTable"] != null)
            {
                dt = (DataTable)ViewState["currentTable"];
                if (dt.Rows[0]["ID"].ToString().Trim() != "")
                {
                    if (btnAdddetails.Text == "ADD")
                    {
                        dr = dt.NewRow();
                        #region calculate rowindex
                        int maxIndex = 0;
                        int[] index = new int[dt.Rows.Count];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            index[i] = Convert.ToInt32(dt.Rows[i]["ID"].ToString());
                        }
                        if (index.Length > 0)
                        {
                            for (int i = 0; i < index.Length; i++)
                            {
                                if (index[i] > maxIndex)
                                {
                                    maxIndex = index[i];
                                }
                            }
                            rowIndex = maxIndex + 1;
                        }
                        else
                        {
                            rowIndex = maxIndex;          //1
                        }
                        #endregion
                        //     rowIndex = dt.Rows.Count + 1;
                        dr["ID"] = rowIndex;     //auto
                        dr["rowAction"] = "A";
                        dr["SrNo"] = 0;
                    }
                    else
                    {
                        //update row
                        int n = Convert.ToInt32(lblNo.Text);
                        rowIndex = Convert.ToInt32(lblID.Text);   //auto no
                        dr = (DataRow)dt.Rows[n - 1];
                        dr["ID"] = rowIndex;
                        dr["SrNo"] = 0;

                        #region decide whether actual row is updating or virtual [rowAction]
                        string id = clsCommon.getString("select detail_Id from " + tblDetails + " where detail_Id=" + rowIndex + " and " +
                            " doc_no='" + txtdoc_no.Text + "' and Company_Code = " + Convert.ToInt32(Session["Company_Code"].ToString()) + " " +
                            " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                        if (id != "0")
                        {
                            dr["rowAction"] = "U";   //actual row
                        }
                        else
                        {
                            dr["rowAction"] = "A";    //virtual row
                        }

                        if (id == "1" && ViewState["mode"].ToString() == "I")
                        {
                            temp = "1";
                        }
                        #endregion
                    }
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add((new DataColumn("ID", typeof(Int32))));
                    #region [Write here columns]
                    dt.Columns.Add((new DataColumn("Type", typeof(string))));   //ddType
                    dt.Columns.Add((new DataColumn("Bank_Code", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("BankName", typeof(string))));
                    dt.Columns.Add((new DataColumn("Narration", typeof(string))));
                    dt.Columns.Add((new DataColumn("Amount", typeof(double))));
                    dt.Columns.Add((new DataColumn("UTR_NO", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("LTNo", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("dodetailid", typeof(int))));

                    dt1.Columns.Add((new DataColumn("UtrDetailId", typeof(int))));
                    #endregion
                    dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                    dt.Columns.Add((new DataColumn("SrNo", typeof(int))));

                    dr = dt.NewRow();
                    dr["ID"] = rowIndex;
                    dr["rowAction"] = "A";
                    dr["SrNo"] = 0;
                }
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add((new DataColumn("ID", typeof(int))));
                #region [Write here columns]
                dt.Columns.Add((new DataColumn("Type", typeof(string))));   //ddType
                dt.Columns.Add((new DataColumn("Bank_Code", typeof(Int32))));
                dt.Columns.Add((new DataColumn("BankName", typeof(string))));
                dt.Columns.Add((new DataColumn("Narration", typeof(string))));
                dt.Columns.Add((new DataColumn("Amount", typeof(double))));
                dt.Columns.Add((new DataColumn("UTR_NO", typeof(Int32))));
                dt.Columns.Add((new DataColumn("LTNo", typeof(Int32))));
                dt.Columns.Add((new DataColumn("dodetailid", typeof(int))));

                dt1.Columns.Add((new DataColumn("UtrDetailId", typeof(int))));
                #endregion
                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                dt.Columns.Add((new DataColumn("SrNo", typeof(int))));

                dr = dt.NewRow();
                dr["ID"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;
            }
            #region [ Set values to dr]
            dr["Type"] = drpddType.SelectedValue;
            dr["Bank_Code"] = txtBANK_CODE.Text;
            dr["Narration"] = txtNARRATION.Text;
            dr["BankName"] = lblBank_name.Text;
            dr["Amount"] = txtBANK_AMOUNT.Text;
            dr["UTR_NO"] = UTR_No;
            dr["LTNo"] = txtLT_No.Text != string.Empty ? txtLT_No.Text : "0";



            #endregion
            if (btnAdddetails.Text == "ADD")
            {
                dr["UtrDetailId"] = Convert.ToInt32(hdnfUtrdetail.Value);
                dr["dodetailid"] = "1";
                dt.Rows.Add(dr);
            }
            else
            {
                dr["UtrDetailId"] = Convert.ToInt32(hdnfUtrdetail.Value.Trim());
            }
            //else
            //{
            //    hdnfUtrdetail.Value = hdnfUtrdetail.Value.Trim();
            //    if (hdnfUtrdetail.Value == string.Empty)
            //    {
            //        dr["UtrDetailId"] = 0;
            //    }
            //    else
            //    {
            //        dr["UtrDetailId"] = Convert.ToInt32(hdnfUtrdetail.Value);
            //    }


            //}

            #region set sr no
            DataRow drr = null;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    drr = (DataRow)dt.Rows[i];
                    drr["SrNo"] = i + 1;
                }
            }
            #endregion
            grdDetail.DataSource = dt;
            grdDetail.DataBind();
            ViewState["currentTable"] = dt;
            if (btnAdddetails.Text == "ADD")
            {
                pnlPopupDetails.Style["display"] = "block";
                setFocusControl(txtBANK_CODE);
            }
            else
            {
                pnlPopupDetails.Style["display"] = "none";
                setFocusControl(btnAdddetails);
                btnOpenDetailsPopup.Focus();
            }
            // Empty Code->
            //txtBANK_CODE.Text = string.Empty;
            txtNARRATION.Text = string.Empty;
            //lblBank_name.Text = string.Empty;
            txtBANK_AMOUNT.Text = string.Empty;
            txtUTRNo.Text = string.Empty;
            txtLT_No.Text = string.Empty;
            //double qntl = double.Parse(txtquantal.Text);
            //double millrate = double.Parse(txtmillRate.Text);
            //double Mill_Amount = qntl * millrate;
            double Mill_Amount = double.Parse(lblMillAmount.Text);

            if (grdDetail.Rows.Count > 0)
            {
                double total = 0.00;
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    if (grdDetail.Rows[i].Cells[9].Text.ToString() != "D")
                    {
                        double amount = Convert.ToDouble(grdDetail.Rows[i].Cells[7].Text);
                        total += amount;
                    }
                }
                if (Mill_Amount != total)
                {
                    string BankAmt = Convert.ToString(Mill_Amount - total);
                    hdnfMainBankAmount.Value = BankAmt;
                    txtBANK_AMOUNT.Text = BankAmt;
                }
                else
                {
                    txtBANK_AMOUNT.Text = Convert.ToString(Mill_Amount);
                    pnlPopupDetails.Style["display"] = "none";

                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [btnClosedetails_Click]
    protected void btnClosedetails_Click(object sender, EventArgs e)
    {
        pnlPopupDetails.Style["display"] = "none";
        setFocusControl(btnSave);
    }
    #endregion

    #region [showDetailsRow]
    private void showDetailsRow(GridViewRow gridViewRow)
    {

        drpddType.SelectedValue = Server.HtmlDecode(gridViewRow.Cells[3].Text);
        txtBANK_CODE.Text = Server.HtmlDecode(gridViewRow.Cells[4].Text);
        lblBank_name.Text = Server.HtmlDecode(gridViewRow.Cells[5].Text);
        txtNARRATION.Text = Server.HtmlDecode(gridViewRow.Cells[6].Text);
        string MainBankAmount = Server.HtmlDecode(gridViewRow.Cells[7].Text);
        txtBANK_AMOUNT.Text = MainBankAmount;
        hdnfMainBankAmount.Value = MainBankAmount;
        hdnfUtrdetail.Value = Server.HtmlDecode(gridViewRow.Cells[11].Text.Trim());
        hdnfUtrdetail.Value = hdnfUtrdetail.Value.Trim();
        hdnfUtrdetail.Value = hdnfUtrdetail.Value != string.Empty ? hdnfUtrdetail.Value : "0";
        txtUTRNo.Text = Server.HtmlDecode(gridViewRow.Cells[8].Text);
        txtLT_No.Text = Server.HtmlDecode(gridViewRow.Cells[9].Text);
        lblNo.Text = Server.HtmlDecode(gridViewRow.Cells[13].Text);
        lblID.Text = Server.HtmlDecode(gridViewRow.Cells[2].Text);
        setFocusControl(drpddType);
        lblUtrBalnceError.Text = "";

        double millamount = txtmillRate.Text != string.Empty ? Convert.ToDouble(txtmillRate.Text) : 0.00;
        double tax = 5.00;
        double result = 0.00;
        tax = tax / 100;
        result = millamount * tax;
        //txtwithGst_Amount.Text = result.ToString();
        txtwithGst_Amount.Text = (millamount + result).ToString();
    }
    #endregion

    #region [DeleteDetailsRow]
    private void DeleteDetailsRow(GridViewRow gridViewRow, string action)
    {
        try
        {
            int rowIndex = gridViewRow.RowIndex;
            if (ViewState["currentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["currentTable"];
                int ID = Convert.ToInt32(dt.Rows[rowIndex]["dodetailid"].ToString());
                string IDExisting = clsCommon.getString("select detail_Id from " + tblDetails + " where dodetailid=" + ID + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (IDExisting != string.Empty)
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[12].Text = "D";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";            //D=Delete from table
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[12].Text = "N";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "N";
                    }
                }
                else
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[12].Text = "R";       //R=Only remove fro grid
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "R";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[12].Text = "A";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "A";
                    }
                }
                ViewState["currentTable"] = dt;
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [txtdoc_no_TextChanged]
    protected void txtdoc_no_TextChanged(object sender, EventArgs e)
    {
        searchString = txtdoc_no.Text;
        strTextBox = "txtdoc_no";
        csCalculations();
    }
    #endregion

    #region [btntxtdoc_no_Click]
    protected void btntxtdoc_no_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtEditDoc_No";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtDOC_DATE_TextChanged]
    protected void txtDOC_DATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDOC_DATE.Text;
        strTextBox = "txtDOC_DATE";
        csCalculations();
    }
    #endregion

    #region [drpDOType_SelectedIndexChanged]
    protected void drpDOType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //searchString = drpDOType.SelectedValue;
        strTextBox = "drpDOType";
        if (drpDOType.SelectedValue == "DO")
        {
            txtGETPASS_CODE.Text = "";
            LBLGETPASS_NAME.Text = "";
        }
        csCalculations();
    }
    #endregion

    #region [txtMILL_CODE_TextChanged]
    protected void txtMILL_CODE_TextChanged(object sender, EventArgs e)
    {

        searchString = txtMILL_CODE.Text;
        strTextBox = "txtMILL_CODE";
        csCalculations();
        if (txtPurcNo.Text != string.Empty && txtPurcOrder.Text != string.Empty)
        {
            if (ViewState["mode"].ToString() == "I")
            {
                calculation();
            }
        }
    }
    #endregion

    #region [btntxtMILL_CODE_Click]
    protected void btntxtMILL_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtMILL_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtGstRate_TextChanged]
    protected void txtGstRate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGstRate.Text;
        strTextBox = "txtGstRate";
        csCalculations();
    }
    #endregion

    #region [btntxtGstRate_Click]
    protected void btntxtGstRate_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGstRate";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtGETPASS_CODE_TextChanged]
    protected void txtGETPASS_CODE_TextChanged(object sender, EventArgs e)
    {

        searchString = txtGETPASS_CODE.Text;
        strTextBox = "txtGETPASS_CODE";
        string selfac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
        if (txtGETPASS_CODE.Text == selfac)
        {
            txtNARRATION4.Enabled = false;
            txtSaleBillTo.Enabled = true;
        }
        else
        {
            //txtNARRATION4.Enabled = true;
            //txtSaleBillTo.Enabled = true;
            //txtSaleBillTo.Text = "";
            //txtNARRATION4.Text = "";
            //txtSalebilltoGstStateCode.Text = "";
            // lbltxtSalebilltoGstStateName.Text = "";
        }
        csCalculations();
        setFocusControl(txtitem_Code);
    }
    #endregion

    #region [btntxtGETPASS_CODE_Click]
    protected void btntxtGETPASS_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGETPASS_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtvoucher_by_TextChanged]
    protected void txtvoucher_by_TextChanged(object sender, EventArgs e)
    {

        searchString = txtvoucher_by.Text;
        strTextBox = "txtvoucher_by";
        csCalculations();
        calculation();
        GSTCalculations();
    }
    #endregion

    #region [btntxtvoucher_by_Click]
    protected void btntxtvoucher_by_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtvoucher_by";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtGRADE_TextChanged]
    protected void txtGRADE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGRADE.Text;
        if (txtGRADE.Text != string.Empty)
        {
            bool a = true;
            if (txtGRADE.Text.Length < 8)
            {
                a = clsCommon.isStringIsNumeric(txtGRADE.Text);
            }
            if (a == false)
            {
                btntxtGRADE_Click(this, new EventArgs());
            }
            else
            {
                pnlPopup.Style["display"] = "none";
                setFocusControl(txtquantal);
            }
        }
    }
    #endregion

    #region [btntxtGRADE_Click]
    protected void btntxtGRADE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGRADE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtquantal_TextChanged]
    protected void txtquantal_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtquantal.Text != string.Empty && txtPACKING.Text != string.Empty && txtquantal.Text != "0" && txtPACKING.Text != "0")
            {
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtquantal.Text) * (100 / float.Parse(txtPACKING.Text))));
                txtBAGS.Text = bags.ToString();
                if (hdnfpacking.Value != "1")
                {
                    //  setFocusControl(txtPACKING);
                    setFocusControl(txtmillRate);
                }
                else
                {
                    setFocusControl(txtGETPASS_CODE);
                    hdnfpacking.Value = "2";
                }
            }
            else if ((txtPACKING.Text == string.Empty || txtPACKING.Text == "0") && txtquantal.Text != string.Empty && txtquantal.Text != "0")
            {
                txtPACKING.Text = "50";
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtquantal.Text) * (100 / float.Parse(txtPACKING.Text))));
                txtBAGS.Text = bags.ToString();
                setFocusControl(txtPACKING);
            }
            else
            {
                txtquantal.Text = string.Empty;
                setFocusControl(txtquantal);
                txtBAGS.Text = "0";
            }
            // searchString = txtquantal.Text;
            strTextBox = "txtquantal";
            calculation();
            MemoadvanceCalculation();
            //setFocusControl(txtmillRate);
        }
        catch { }
    }
    #endregion

    #region [txtPACKING_TextChanged]
    protected void txtPACKING_TextChanged(object sender, EventArgs e)
    {
        // searchString = txtPACKING.Text;
        strTextBox = "txtPACKING";
        csCalculations();
    }
    #endregion

    #region [txtBAGS_TextChanged]
    protected void txtBAGS_TextChanged(object sender, EventArgs e)
    {
        //  searchString = txtBAGS.Text;
        strTextBox = "txtBAGS";
        csCalculations();
    }
    #endregion

    #region [txtexcise_rate_TextChanged]
    protected void txtexcise_rate_TextChanged(object sender, EventArgs e)
    {
        // searchString = txtexcise_rate.Text;
        strTextBox = "txtexcise_rate";
        csCalculations();
    }
    #endregion

    #region [txtSALE_RATE_TextChanged]
    protected void txtSALE_RATE_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtSALE_RATE.Text;
        strTextBox = "txtSALE_RATE";
        csCalculations();
        calculation();
        GSTCalculations();
    }
    #endregion

    #region [txtDIFF_AMOUNT_TextChanged]
    protected void txtDIFF_AMOUNT_TextChanged(object sender, EventArgs e)
    {
        // searchString = txtDIFF_AMOUNT.Text;
        strTextBox = "txtDIFF_AMOUNT";
        csCalculations();
    }
    #endregion

    #region [txtDO_CODE_TextChanged]
    protected void txtDO_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDO_CODE.Text;
        strTextBox = "txtDO_CODE";
        csCalculations();
    }
    #endregion

    #region [txtPurchase_Date_TextChanged]
    protected void txtPurchase_Date_TextChanged(object sender, EventArgs e)
    {
        // searchString = txtPurchase_Date.Text;
        strTextBox = "txtPurchase_Date";
        csCalculations();
    }
    #endregion

    #region [btntxtDO_CODE_Click]
    protected void btntxtDO_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtDO_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtBroker_CODE_TextChanged]
    protected void txtBroker_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBroker_CODE.Text;
        strTextBox = "txtBroker_CODE";
        csCalculations();
    }
    #endregion

    #region [btntxtBroker_CODE_Click]
    protected void btntxtBroker_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtBroker_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtTruck_NO_TextChanged]
    protected void txtTruck_NO_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtTruck_NO.Text;
        strTextBox = "txtTruck_NO";
        csCalculations();
    }
    #endregion

    #region [txtTRANSPORT_CODE_TextChanged]
    protected void txtTRANSPORT_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTRANSPORT_CODE.Text;
        strTextBox = "txtTRANSPORT_CODE";
        csCalculations();
    }
    #endregion

    #region [btntxtTRANSPORT_CODE_Click]
    protected void btntxtTRANSPORT_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtTRANSPORT_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtNARRATION1_TextChanged]
    protected void txtNARRATION1_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNARRATION1.Text;
        strTextBox = "txtNARRATION1";
        csCalculations();
    }
    #endregion

    #region [btntxtNARRATION1_Click]
    protected void btntxtNARRATION1_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtNARRATION1";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtNARRATION2_TextChanged]
    protected void txtNARRATION2_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNARRATION2.Text;
        strTextBox = "txtNARRATION2";
        csCalculations();
    }
    #endregion

    #region [btntxtNARRATION2_Click]
    protected void btntxtNARRATION2_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtNARRATION2";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtNARRATION3_TextChanged]
    protected void txtNARRATION3_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNARRATION3.Text;
        strTextBox = "txtNARRATION3";
        csCalculations();
    }
    #endregion

    #region [btntxtNARRATION3_Click]
    protected void btntxtNARRATION3_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtNARRATION3";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtNARRATION4_TextChanged]
    protected void txtNARRATION4_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNARRATION4.Text;
        strTextBox = "txtNARRATION4";
        csCalculations();
    }
    #endregion

    #region [btntxtNARRATION4_Click]
    protected void btntxtNARRATION4_Click(object sender, EventArgs e)
    {
        try
        {

            searchString = txtSaleBillTo.Text;
            strTextBox = "txtSaleBillTo";
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtNARRATION4";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion


    #region [txtBANK_CODE_TextChanged]
    protected void txtBANK_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBANK_CODE.Text;
        strTextBox = "txtBANK_CODE";
        csCalculations();
    }
    #endregion

    #region [btntxtBANK_CODE_Click]
    protected void btntxtBANK_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtBANK_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtNARRATION_TextChanged]
    protected void txtNARRATION_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNARRATION.Text;
        strTextBox = "txtNARRATION";
        setFocusControl(txtBANK_AMOUNT);
        //csCalculations();
    }
    #endregion

    #region [btntxtNARRATION_Click]
    protected void btntxtNARRATION_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtNARRATION";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtBANK_AMOUNT_TextChanged]
    protected void txtBANK_AMOUNT_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtBANK_AMOUNT.Text;
        //strTextBox = "txtBANK_AMOUNT";
        setFocusControl(btnAdddetails);
        //csCalculations();
    }
    #endregion
    #region [txtLT_No_TextChanged]
    protected void txtLT_No_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtBANK_AMOUNT.Text;
        //strTextBox = "txtBANK_AMOUNT";
        setFocusControl(btnAdddetails);
        //csCalculations();
    }
    #endregion

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion

    #region [txtSearchText_TextChanged]
    protected void txtSearchText_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == "Close")
            {
                txtSearchText.Text = string.Empty;
                pnlPopup.Style["display"] = "none";
                grdPopup.DataSource = null;
                grdPopup.DataBind();
                if (objAsp != null)
                    System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(objAsp);
            }
            else
            {
                pnlPopup.Style["display"] = "block";
                searchString = txtSearchText.Text;
                strTextBox = hdnfClosePopup.Value;
                setFocusControl(btnSearch);
            }
        }
        catch
        {
        }
    }
    #endregion




    #region [btnSave_Click]
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Stopwatch alltime = new Stopwatch();
            alltime.Start();

            if (Session["SELF_AC"].ToString() == null && Session["SELF_AC"].ToString() == "")
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Update selfAc in Company Parameter!!!');", true);
                return;
            }

            if (Session["AUTO_VOUCHER"].ToString() == null && Session["AUTO_VOUCHER"].ToString() == "")
            {
                Session["AUTO_VOUCHER"] = "NO";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Update Autovoucher Company Parameter!!!');", true);

            }


            #region Check PostDate
            string message = clsCheckingdate.checkdate(txtDOC_DATE.Text, Session["Post_Date"].ToString(), "Outword", "", "", Convert.ToInt32(Session["Company_Code"]), Convert.ToInt32(Session["year"]));
            if (message != string.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('" + message + "!!!');", true);
                return;
            }
            //string Post_date = string.Empty;
            //if (Session["Post_Date"] != null && Session["Post_Date"].ToString() != "")
            //{
            //    Post_date = Session["Post_Date"].ToString();
            //}
            //else
            //{
            //    setFocusControl(txtDOC_DATE);
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Update Post Date !!!');", true);
            //    return;
            //}
            //string dd = "";
            //string format = "MM/dd/yyyy";
            //dd = DateTime.Parse(Post_date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

            //DateTime dt = Convert.ToDateTime(dd);

            //// string dd = "";
            //dd = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

            //DateTime dtdate = Convert.ToDateTime(dd);
            //string Docdate = dtdate.ToString("yyyy-MM-dd HH:mm:ss.fff");

            //if (dtdate > dt)
            //{
            //    // isValidated = true;
            //}
            //else
            //{
            //    setFocusControl(txtDOC_DATE);
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Post Date Error!!!!!');", true);
            //    return;
            //}

            //string Outword_Date = clsCommon.getString("select date_format(Outword_Date,'%d/%m/%Y') as Outword from Post_Date where Company_Code=" + Convert.ToInt32(Session["Company_Code"]).ToString());

            //// value = Outword_Date;
            //Outword_Date = DateTime.Parse(Outword_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

            //dt = Convert.ToDateTime(Outword_Date);
            //Outword_Date = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");

            //if (dtdate > dt)
            //{

            //}
            //else
            //{
            //    setFocusControl(txtDOC_DATE);
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('GST Return Fined please Do not Delete Record!!!!!');", true);
            //    return;
            //}
            #endregion
            string qry = "";

            #region [Validation Part]
            bool isValidated = true;
            if (txtdoc_no.Text != string.Empty)
            {
                if (ViewState["mode"] != null)
                {
                    if (ViewState["mode"].ToString() == "I")
                    {
                        string str = clsCommon.getString("select doc_no from " + tblHead + " where doc_no='" + txtdoc_no.Text + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'");
                        if (str != "0")
                        {
                            lblMsg.Text = "Code " + txtdoc_no.Text + " already exist";
                            this.NextNumber();
                            isValidated = true;
                        }
                        else
                        {
                            isValidated = true;
                        }
                    }
                }
            }
            else
            {
                isValidated = false;
                setFocusControl(txtdoc_no);
                return;
            }
            if (txtPurcNo.Text != string.Empty)
            {
                string tenderNo = clsCommon.getString("Select Tender_No from qrytenderheaddetail where Mill_Code=" + txtMILL_CODE.Text + " and Tender_No=" + txtPurcNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (txtPurcNo.Text != tenderNo)
                {
                    isValidated = false;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Purchase Number Does Not Belongs to This Mill!');", true);
                    setFocusControl(txtMILL_CODE);
                    return;
                }
                else
                {
                    isValidated = true;
                }
            }

            //  int count = 0;
            //if (grdDetail.Rows.Count > 1)
            //{
            //    for (int i = 0; i < grdDetail.Rows.Count; i++)
            //    {
            //        if (grdDetail.Rows[i].Cells[12].Text.ToString() == "D")
            //        {
            //            count++;
            //        }
            //    }
            //    if (grdDetail.Rows.Count == count)
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Add Dispatch Details!');", true);
            //        isValidated = false;
            //        setFocusControl(btnOpenDetailsPopup);
            //        return;
            //    }
            //}


            //if (grdDetail.Rows.Count > 0)
            //{
            //    double total = 0.00;
            //    for (int i = 0; i < grdDetail.Rows.Count; i++)
            //    {
            //        if (grdDetail.Rows[i].Cells[12].Text.ToString() != "D" && grdDetail.Rows[i].Cells[12].Text.ToString() != "R")
            //        {
            //            double amount = Convert.ToDouble(grdDetail.Rows[i].Cells[7].Text);
            //            total += amount;
            //        }
            //    }

            //    if (total == Convert.ToDouble(lblMillAmount.Text))
            //    {
            //        isValidated = true;
            //    }
            //    else
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Mill Amount Does Not match with detail amount!');", true);
            //        isValidated = false;
            //        setFocusControl(btnOpenDetailsPopup);
            //        return;
            //    }
            //}
            if (drpDOType.SelectedValue == "DI")
            {
                if (grdDetail.Rows.Count == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Enter Dispatch Details!');", true);
                    isValidated = false;
                    setFocusControl(btnOpenDetailsPopup);
                    return;
                }
            }


            if (drpDOType.SelectedValue == "DI")
            {
                if (drpDeliveryType.SelectedValue == "C" || drpDeliveryType.SelectedValue == "N")
                {
                    int transportcode = txtTRANSPORT_CODE.Text != string.Empty ? Convert.ToInt32(txtTRANSPORT_CODE.Text) : 0;
                    if (transportcode == 0)
                    {
                        isValidated = false;
                        setFocusControl(txtTRANSPORT_CODE);
                        return;
                    }
                    else
                    {
                        isValidated = true;
                    }
                }
            }

            #endregion


            DS = returndata(txtGstRate.Text, "gstreturn", txtMILL_CODE.Text, txtGETPASS_CODE.Text, txtvoucher_by.Text, txtSaleBillTo.Text, txtBill_To.Text, txtTRANSPORT_CODE.Text, txtDO_CODE.Text, txtBroker_CODE.Text, txtVasuliAc.Text, txtitem_Code.Text, Session["Company_Code"].ToString());

            SELFAC = Convert.ToInt32(Session["SELF_AC"].ToString());
            Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            Year_Code = Convert.ToInt32(Session["year"].ToString());

            if (btnSave.Text == "Update")
            {
                if (txtSaleBillTo.Text != string.Empty)
                {
                    if (txtSaleBillTo.Text == SELFAC.ToString() || txtSaleBillTo.Text == "2")
                    {
                        DataSet saleB = clsDAL.SimpleQuery("select saleid,doc_no from qrysalehead where DO_No='" + txtdoc_no.Text + "' and Company_Code='" + Company_Code + "' " +
                            " and Year_Code='" + Session["year"].ToString() + "'");
                        if (saleB != null)
                        {
                            DataTable saledt = saleB.Tables[0];
                            if (saledt.Rows.Count > 0)
                            {
                                for (int i = 0; i < saledt.Rows.Count; i++)
                                {
                                    salePosting = new SaleFields();
                                    salePosting.SB_Sale_Id = Convert.ToInt32(saledt.Rows[i]["saleid"].ToString());
                                    salePosting.SB_doc_no = Convert.ToInt32(saledt.Rows[i]["doc_no"].ToString());
                                    salePosting.SB_Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
                                    salePosting.SB_Year_Code = Convert.ToInt32(Session["year"].ToString());

                                    SalePurcdt = new DataTable();
                                    SalePurcdt = clsSale_Posting.Sale_Posting(3, salePosting, "SB", saledt);
                                    msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, SalePurcdt);
                                }

                            }
                        }
                    }
                }

            }
            // string selfaccNew = Session["SELF_AC"].ToString();
            if (drpDOType.SelectedValue == "DI")
            {
                int accode = Convert.ToInt32(txtvoucher_by.Text);

                if (accode != SELFAC)
                {
                    #region

                    // cureentbalance = clsCommon.getString("select sum(case drcr when 'D' then AMOUNT when 'C' then -amount end) as Balance from NT_1_GLEDGER where ac_code='" + txtSaleBillTo.Text + "' and company_code=" + Company_Code);
                    //limitbalnce = clsCommon.getString("select Bal_Limit from NT_1_AccountMaster where Ac_Code='" + txtSaleBillTo.Text + "' and company_code=" + Company_Code);
                    //string Limit_By = clsCommon.getString("select Limit_By from NT_1_AccountMaster where Ac_Code='" + txtSaleBillTo.Text + "' and company_code=" + Company_Code);
                    //if (cureentbalance == string.Empty)
                    //{
                    //    cureentbalance = "0.00";
                    //}
                    //CurrBal = Convert.ToDouble(cureentbalance);
                    //if (limitbalnce == string.Empty)
                    //{
                    //    limitbalnce = "0.00";
                    //}
                    //limitBal = Convert.ToDouble(limitbalnce);

                    CurrBal = Convert.ToDouble(DS.Tables[0].Rows[1]["Balance"]);
                    limitBal = Convert.ToDouble(DS.Tables[0].Rows[1]["Bal_Limit"]);
                    string Limit_By = DS.Tables[0].Rows[1]["Limit_By"].ToString();

                    if (CurrBal < 0)
                    {
                        CurrBal = Math.Abs(CurrBal);
                    }
                    else
                    {
                        CurrBal = -CurrBal;
                    }
                    OldBillAmtNew = Convert.ToDouble(0 + hdnfOldBillAmt.Value);
                    NetLimit = limitBal + CurrBal + OldBillAmtNew;

                    Qty = txtquantal.Text != string.Empty ? Convert.ToDouble(txtquantal.Text) : 0;
                    SaleAmt = txtSALE_RATE.Text != string.Empty ? Convert.ToDouble(txtSALE_RATE.Text) : 0;
                    Commission = txtCommission.Text != string.Empty ? Convert.ToDouble(txtCommission.Text) : 0;
                    GSTAmt = (SaleAmt * 5) / 100;
                    BillAmt = (GSTAmt + SaleAmt + Commission) * Qty;
                    double dif = NetLimit - BillAmt;
                    if (Limit_By == "Y")
                    {
                        if (NetLimit < BillAmt)
                        {
                            string Limit = Convert.ToString(NetLimit);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Limit Problem! current limit " + Limit + "',Diff :" + dif + ");", true);
                            setFocusControl(txtquantal);
                            return;
                        }
                    }

                    #endregion
                }
            }
            btnSave.Enabled = false;


            #region -Head part declearation
            Int32 DONumber = 0;
            DOC_NO = txtdoc_no.Text != string.Empty ? Convert.ToInt32(txtdoc_no.Text) : 0;
            DOC_DATE = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            PUR_DATE = DateTime.Parse(txtPurchase_Date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            DESP_TYPE = drpDOType.SelectedValue;
            Delivery_Type = string.Empty;
            if (DESP_TYPE == "DI")
            {
                if (drpDOType.SelectedValue == "DI")
                {
                    Delivery_Type = drpDeliveryType.SelectedValue;
                }
            }
            MillInv_Date = DateTime.Parse(txtMillInv_Date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            if (chkInv_Chk.Checked == true)
            {
                Inv_Chk = "Y";
            }
            else
            {
                Inv_Chk = "N";
            }
            MillEwayBill = txtMillEwayBill_No.Text;
            MILL_CODE = txtMILL_CODE.Text;
            gst_code = txtGstRate.Text;
            GETPASS_CODE = txtGETPASS_CODE.Text;
            VOUCHER_BY = txtvoucher_by.Text;
            FRIEGHT_RATE = txtFrieght.Text != string.Empty ? Convert.ToDouble(txtFrieght.Text) : 0;
            FRIEGHT_AMOUNT = txtFrieghtAmount.Text != string.Empty ? Convert.ToDouble(txtFrieghtAmount.Text) : 0;
            VASULI_AMOUNT = txtVasuliAmount.Text != string.Empty ? Convert.ToDouble(txtVasuliAmount.Text) : 0;
            VASULI_RATE = txtVasuliRate.Text != string.Empty ? Convert.ToDouble(txtVasuliRate.Text) : 0;
            MEMO_ADVANCE = txtMemoAdvance.Text != string.Empty ? Convert.ToDouble(txtMemoAdvance.Text) : 0;
            Ac_Code = txtvoucher_by.Text;
            GRADE = txtGRADE.Text;
            itemcode = Convert.ToInt32(txtitem_Code.Text != string.Empty ? txtitem_Code.Text : "0");
            QUANTAL = txtquantal.Text != string.Empty ? Convert.ToDouble(txtquantal.Text) : 0.00;
            PACKING = txtPACKING.Text != string.Empty ? Convert.ToInt32(txtPACKING.Text) : 0;
            BAGS = txtBAGS.Text != string.Empty ? Convert.ToInt32(txtBAGS.Text) : 0;
            mill_rate = txtmillRate.Text != string.Empty ? Convert.ToDouble(txtmillRate.Text) : 0.00;
            EXCISE_RATE = txtexcise_rate.Text != string.Empty ? Convert.ToDouble(txtexcise_rate.Text) : 0.00;
            Tender_Commission = txtCommission.Text != string.Empty ? Convert.ToDouble(txtCommission.Text) : 0.00;
            SALE_RATE = txtSALE_RATE.Text != string.Empty ? Convert.ToDouble(txtSALE_RATE.Text) : 0.00;
            // MILL_AMOUNT = 0.00;// double.Parse(lblMillAmount.Text.ToString());
            MILL_AMOUNT = QUANTAL * (mill_rate + EXCISE_RATE);
            lblMillAmount.Text = MILL_AMOUNT.ToString();
            DIFF_RATE = lblDiffrate.Text != string.Empty ? Convert.ToDouble(lblDiffrate.Text) : 0.00;
            DIFF_AMOUNT = txtDIFF_AMOUNT.Text != string.Empty ? Convert.ToDouble(txtDIFF_AMOUNT.Text) : 0.00;
            VASULI_RATE_1 = txtVasuliRate1.Text != string.Empty ? Convert.ToDouble(txtVasuliRate1.Text) : 0.00;
            VASULI_AMOUNT_1 = txtVasuliAmount1.Text != string.Empty ? Convert.ToDouble(txtVasuliAmount1.Text) : 0.00;
            EWayBill_No = txtEWayBill_No.Text != string.Empty ? txtEWayBill_No.Text : "0";
            Distance = txtDistance.Text != string.Empty ? Convert.ToDouble(txtDistance.Text) : 0.00;
            txtSaleBillTo.Text = txtSaleBillTo.Text != string.Empty ? txtSaleBillTo.Text : "0";
            SaleBillTo = txtSaleBillTo.Text;
            if (SaleBillTo.Trim() == string.Empty)
            {
                SaleBillTo = "0";
            }
            MM_CC = drpCC.SelectedValue.ToString();
            //double Party_Commission_Rate = txtPartyCommission.Text != string.Empty ? Convert.ToDouble(txtPartyCommission.Text) : 0.00;
            MM_Rate = txtMemoAdvanceRate.Text != string.Empty ? Convert.ToDouble(txtMemoAdvanceRate.Text) : 0.00;
            PAN_NO = txtPanNo.Text.Trim();
            DO_CODE = txtDO_CODE.Text != string.Empty ? Convert.ToInt32(txtDO_CODE.Text) : 2;
            BROKER_CODE = txtBroker_CODE.Text != string.Empty ? Convert.ToInt32(txtBroker_CODE.Text) : 2;
            TRUCK_NO = txtTruck_NO.Text.ToUpper();
            TRANSPORT_CODE = txtTRANSPORT_CODE.Text != string.Empty ? Convert.ToInt32(txtTRANSPORT_CODE.Text) : 0;
            VASULI_AC = txtVasuliAc.Text != string.Empty ? Convert.ToInt32(txtVasuliAc.Text) : 0;
            SaleBillTransport = 0;
            if (drpCC.SelectedValue == "Credit")
            {
                SaleBillTransport = txtTRANSPORT_CODE.Text != string.Empty ? Convert.ToInt32(txtTRANSPORT_CODE.Text) : 0;
            }
            else
            {
                SaleBillTransport = 1;
            }
            Tender_Commission_Amount = Tender_Commission * QUANTAL;
            OVTransportCode = txtTRANSPORT_CODE.Text != string.Empty ? Convert.ToInt32(txtTRANSPORT_CODE.Text) : 0; ;
            if (drpCC.SelectedValue == "Cash")
            {
                OVTransportCode = 1;
            }

            NARRATION1 = txtNARRATION1.Text;
            NARRATION2 = txtNARRATION2.Text;
            NARRATION3 = txtNARRATION3.Text;
            NARRATION4 = txtNARRATION4.Text;
            NARRATION5 = txtNarration5.Text.Trim();
            // INVOICE_NO = txtINVOICE_NO.Text;
            // CheckPost = txtCheckPostName.Text;
            purc_no = txtPurcNo.Text != string.Empty ? Convert.ToInt32(txtPurcNo.Text) : 0;
            purc_order = txtPurcOrder.Text != string.Empty ? Convert.ToInt32(txtPurcOrder.Text) : 0;
            //double final_amout = mill_rate * QUANTAL;
            #region other voucher amount
            VoucherBrokrage = txtVoucherBrokrage.Text != string.Empty ? Convert.ToDouble(txtVoucherBrokrage.Text) : 0.00;
            VoucherServiceCharge = txtVoucherServiceCharge.Text != string.Empty ? Convert.ToDouble(txtVoucherServiceCharge.Text) : 0.00;
            VoucherRateDiffRate = txtVoucherL_Rate_Diff.Text != string.Empty ? Convert.ToDouble(txtVoucherL_Rate_Diff.Text) : 0.00;
            VoucherRateDiffAmt = txtVoucherRATEDIFFAmt.Text != string.Empty ? Convert.ToDouble(txtVoucherRATEDIFFAmt.Text) : 0.00;
            VoucherBankCommRate = txtVoucherCommission_Rate.Text != string.Empty ? Convert.ToDouble(txtVoucherCommission_Rate.Text) : 0.00;
            VoucherBankCommAmt = txtVoucherBANK_COMMISSIONAmt.Text != string.Empty ? Convert.ToDouble(txtVoucherBANK_COMMISSIONAmt.Text) : 0.00;
            VoucherInterest = txtVoucherInterest.Text != string.Empty ? Convert.ToDouble(txtVoucherInterest.Text) : 0.00;
            VoucherTransport = txtVoucherTransport_Amount.Text != string.Empty ? Convert.ToDouble(txtVoucherTransport_Amount.Text) : 0.00;
            VoucherOtherExpenses = txtVoucherOTHER_Expenses.Text != string.Empty ? Convert.ToDouble(txtVoucherOTHER_Expenses.Text) : 0.00;

            EWay_BillChk = string.Empty;

            if (chkEWayBill.Checked == true)
            {
                EWay_BillChk = "Y";
            }
            else
            {
                EWay_BillChk = "N";
            }
            MillInvoiceno = txtMillInvoiceno.Text;

            #endregion

            FINAL_AMOUNT = FRIEGHT_AMOUNT - MEMO_ADVANCE;
            userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");
            retValue = string.Empty;
            strRev = string.Empty;
            Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            Year_Code = Convert.ToInt32(Session["year"].ToString());

            Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
            DIFF = float.Parse(lblDiffrate.Text);
            LESS_DIFF = Math.Round(((SALE_RATE - FRIEGHT_RATE) - (mill_rate)) * QUANTAL);
            LESSDIFFOV = DIFF_RATE * QUANTAL;
            Driver_Mobile = txtDriverMobile.Text;
            Diff_Rate = 0.00;
            VOUCHER_AMOUNT = 0.00;
            Rate_Type = string.Empty;
            if (drpDeliveryType.SelectedValue == "N")
            {
                Diff_Rate = ((SALE_RATE - FRIEGHT_RATE) - mill_rate) * QUANTAL;
                VOUCHER_AMOUNT = MILL_AMOUNT + Diff_Rate + Tender_Commission_Amount + LESSDIFFOV + MEMO_ADVANCE + VoucherBrokrage +
                    VoucherServiceCharge + VoucherRateDiffAmt + VoucherBankCommAmt + VoucherInterest + VoucherTransport + VoucherOtherExpenses;
                Rate_Type = "A";
            }
            else
            {
                LESSDIFFOV = 0;
                Diff_Rate = ((SALE_RATE) - mill_rate) * QUANTAL;
                //VOUCHER_AMOUNT = MILL_AMOUNT + Tender_Commission_Amount + Diff_Rate + LESSDIFFOV + MEMO_ADVANCE + VoucherBrokrage + VoucherServiceCharge + VoucherRateDiffAmt + VoucherBankCommAmt + VoucherInterest + VoucherTransport + VoucherOtherExpenses;
                VOUCHER_AMOUNT = MILL_AMOUNT + Tender_Commission_Amount + Diff_Rate + LESSDIFFOV + VoucherBrokrage + VoucherServiceCharge +
                    VoucherRateDiffAmt + VoucherBankCommAmt + VoucherInterest + VoucherTransport + VoucherOtherExpenses;
                Rate_Type = "L";
            }
            //Int32 UTR_No = txtUTRNo.Text != string.Empty ? Convert.ToInt32(txtUTRNo.Text) : 0;
            Carporate_Sale_No = txtcarporateSale.Text != string.Empty ? Convert.ToInt32(txtcarporateSale.Text) : 0;
            WhoseFrieght = ddlFrieghtType.SelectedValue.ToString();
            UTR_Year_Code = lblUTRYearCode.Text != string.Empty ? Convert.ToInt32(lblUTRYearCode.Text) : 0;
            Carporate_Sale_Year_Code = lblCSYearCode.Text != string.Empty ? Convert.ToInt32(lblCSYearCode.Text) : 0;
            voucher_no = 0;
            if (Carporate_Sale_No != null && Carporate_Sale_No != 0)
            {
                PDS = clsCommon.getString("Select selling_type from carporatehead where doc_no="
                   + Carporate_Sale_No + " and Company_Code=" + Company_Code + "");
            }
            //  AUTO_VOUCHER = clsCommon.getString("select AutoVoucher from " + tblPrefix + "CompanyParameters where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
            AUTO_VOUCHER = Session["AUTO_VOUCHER"].ToString();
            memo_no = lblMemoNo.Text != string.Empty ? Convert.ToInt32(lblMemoNo.Text) : 0;
            if (ViewState["mode"].ToString() == "I")
            {
                if (AUTO_VOUCHER == "YES")
                {
                    if (DESP_TYPE == "DI")
                    {
                        if (MEMO_ADVANCE + VASULI_AMOUNT_1 != 0)
                        {
                            // memo_no = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(Doc_No),0)+1 from " + tblHead + " Where Tran_Type='MM' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                        }
                    }
                    if (DESP_TYPE == "DO")
                    {
                        if (DIFF_AMOUNT != 0)
                        {
                            //MaxVoucher();
                            if (hdnvouchernumber.Value != string.Empty)
                            {
                                voucher_no = Convert.ToInt32(int.Parse(hdnvouchernumber.Value.TrimStart()));
                            }
                        }
                        else
                        {
                            hdnvouchernumber.Value = "0";
                            voucher_no = 0;
                        }
                    }
                    else
                    {
                        // MaxVoucher();
                        if (hdnvouchernumber.Value != string.Empty)
                        {
                            // voucher_no = Convert.ToInt32(int.Parse(hdnvouchernumber.Value.TrimStart()));
                        }
                    }
                }
            }
            voucher_type = lblVoucherType.Text;
            //Int32 memo_no = lblMemoNo.Text != string.Empty ? Convert.ToInt32(lblMemoNo.Text) : 0;
            voucherlbl = lblVoucherNo.Text != string.Empty ? Convert.ToInt32(lblVoucherNo.Text) : 0;
            //double Freight_Amount = lblFreight.Text != string.Empty ? Convert.ToDouble(lblFreight.Text) : 0.00;
            myNarration = string.Empty;
            myNarration2 = string.Empty;
            myNarration3 = string.Empty;
            myNarration4 = string.Empty;
            vouchnarration = string.Empty;
            //  millShortName = clsCommon.getString("select short_name from " + qryAccountList + " where ac_code=" + MILL_CODE + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            millShortName = hdnfmillshortname.Value;
            if (ddlFrieghtType.SelectedValue == "O")
            {
                vouchnarration = millShortName + " (" + "S.R." + SALE_RATE + "-" + FRIEGHT_RATE + "- M.R." + mill_rate + ")*" + QUANTAL;
            }
            else
            {
                vouchnarration = "Qntl " + QUANTAL + "  " + millShortName + "(M.R." + mill_rate + " P.R." + SALE_RATE + ")";
            }
            if (grdDetail.Rows.Count > 0)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    utrno = Server.HtmlDecode(grdDetail.Rows[i].Cells[6].Text.ToString());
                    Utr_No = Server.HtmlDecode(grdDetail.Rows[i].Cells[8].Text.ToString());
                    nar = clsCommon.getString("select 'dt:'+Convert(varchar(10),doc_date,103)+'  amt:'+CONVERT(nvarchar(255),amount) from " + tblPrefix + "UTR where doc_no=" + Utr_No + " and Company_Code=" + Company_Code + " and Year_Code=" + Year_Code + "");
                    if (i == 0)
                    {
                        if (utrno != "Transfer Letter")
                        {
                            myNarration = utrno + " " + nar;
                        }
                        else
                        {
                            myNarration = "Please Debit The Same Amount in our A/c";
                        }

                    }
                    if (i == 1)
                    {
                        myNarration2 = utrno + " " + nar;
                    }
                    if (i == 2)
                    {
                        myNarration2 += " " + utrno + " " + nar;
                    }
                    if (i >= 3)
                    {
                        myNarration3 += " " + utrno + " " + nar;
                    }
                }
            }

            VOUCHERAMOUNT = Convert.ToInt32(Math.Ceiling(DIFF_AMOUNT));
            //double MILL_AMOUNT = mill_rate * QUANTAL;
            //city_code = clsCommon.getString("select City_Code from " + qryAccountList + " where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            // From_Place = clsCommon.getString("select cityname from " + qryAccountList + " where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Company_Code);
            //city_code2 = clsCommon.getString("select City_Code from " + qryAccountList + "  where Ac_Code=" + txtGETPASS_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            // To_Place = clsCommon.getString("select cityname from " + qryAccountList + " where Ac_Code=" + txtGETPASS_CODE.Text + " and Company_Code=" + Company_Code);
            // selfac = selfacc;
            //    selfac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

            From_Place = DS.Tables[0].Rows[1]["From_Place"].ToString();
            To_Place = DS.Tables[0].Rows[1]["To_Place"].ToString();


            BILL_AMOUNT = 0.00;
            SUBTOTAL = 0.00;
            SUBTOTAL = QUANTAL * mill_rate;
            BILL_AMOUNT = SUBTOTAL;
            TotalGstPurchaseAmount = 0.0;
            TotalGstSaleAmount = 0.0;
            #region  gstcalculation
            GSTRateCode = txtGstRate.Text != string.Empty ? Convert.ToInt32(txtGstRate.Text) : 1;
            GetpassGstStateCode = txtGetpassGstStateCode.Text != string.Empty ? Convert.ToInt32(txtGetpassGstStateCode.Text) : 0;
            VoucherbyGstStateCode = txtVoucherbyGstStateCode.Text != string.Empty ? Convert.ToInt32(txtVoucherbyGstStateCode.Text) : 0;
            SalebilltoGstStateCode = txtSalebilltoGstStateCode.Text != string.Empty ? Convert.ToInt32(txtSalebilltoGstStateCode.Text) : 0;
            MillGstStateCode = txtMillGstStateCode.Text != string.Empty ? Convert.ToInt32(txtMillGstStateCode.Text) : 0;
            TransportGstStateCode = txtTransportGstStateCode.Text != string.Empty ? Convert.ToInt32(txtTransportGstStateCode.Text) : 0;
            GSTMillRateAmount = 0.00; //txtGstMRAmount.Text != string.Empty ? Convert.ToDouble(txtGstMRAmount.Text) : 0.00;
            GSTSaleRateAmount = 0.00;// txtGstSRAmount.Text != string.Empty ? Convert.ToDouble(txtGstSRAmount.Text) : 0.00;
            GSTExclSaleRateAmount = 0.00;// txtGstExSaleRate.Text != string.Empty ? Convert.ToDouble(txtGstExSaleRate.Text) : 0.00;
            GSTExclMillRateAmount = 0.00;// txtGstExMillRate.Text != string.Empty ? Convert.ToDouble(txtGstExMillRate.Text) : 0.00;

            GSTRate = Convert.ToDouble(DS.Tables[0].Rows[0]["Rate"].ToString());
            cgstrate = Convert.ToDouble(DS.Tables[0].Rows[0]["CGST"].ToString());
            sgstrate = Convert.ToDouble(DS.Tables[0].Rows[0]["SGST"].ToString());
            igstrate = Convert.ToDouble(DS.Tables[0].Rows[0]["IGST"].ToString());

            //hdnf.Value = clsCommon.getString("select tenderdetailid from  nt_1_tenderdetails where Tender_No=" + txtPurcNo.Text + " and ID=" + txtPurcOrder.Text + " ");
            pdsparty = hdnfPDSPartyCode.Value != string.Empty ? Convert.ToInt32(hdnfPDSPartyCode.Value) : 0;
            pdsunit = hdnfPDSUnitCode.Value != string.Empty ? Convert.ToInt32(hdnfPDSUnitCode.Value) : 0;

            mc = Convert.ToInt32(DS.Tables[0].Rows[1]["millcode"].ToString() != string.Empty ? DS.Tables[0].Rows[1]["millcode"].ToString() : "0");
            gp = Convert.ToInt32(DS.Tables[0].Rows[1]["getpasscode"].ToString() != string.Empty ? DS.Tables[0].Rows[1]["getpasscode"].ToString() : "0");
            st = Convert.ToInt32(DS.Tables[0].Rows[1]["shipto"].ToString() != string.Empty ? DS.Tables[0].Rows[1]["shipto"].ToString() : "0");
            sb = Convert.ToInt32(DS.Tables[0].Rows[1]["salebillto"].ToString() != string.Empty ? DS.Tables[0].Rows[1]["salebillto"].ToString() : "0");
            tc = Convert.ToInt32(DS.Tables[0].Rows[1]["transport"].ToString() != string.Empty ? DS.Tables[0].Rows[1]["transport"].ToString() : "0");
            bk = Convert.ToInt32(DS.Tables[0].Rows[1]["broker"].ToString() != string.Empty ? DS.Tables[0].Rows[1]["broker"].ToString() : "0");
            ic = Convert.ToInt32(DS.Tables[0].Rows[1]["itemcode"].ToString() != string.Empty ? DS.Tables[0].Rows[1]["itemcode"].ToString() : "0");
            va = Convert.ToInt32(DS.Tables[0].Rows[1]["vasualiac"].ToString() != string.Empty ? DS.Tables[0].Rows[1]["vasualiac"].ToString() : "0");
            Bill_To = Convert.ToInt32(txtBill_To.Text != string.Empty ? txtBill_To.Text : "0");
            bt = Convert.ToInt32(DS.Tables[0].Rows[1]["billto"].ToString() != string.Empty ? DS.Tables[0].Rows[1]["billto"].ToString() : "0");
            docd = Convert.ToInt32(DS.Tables[0].Rows[1]["docode"].ToString() != string.Empty ? DS.Tables[0].Rows[1]["docode"].ToString() : "0");


            if (Carporate_Sale_No != null && Carporate_Sale_No != 0)
            {
                try
                {
                    cscode = Convert.ToInt32(hdnfcscode.Value);
                }
                catch
                {

                }
            }
            #endregion


            #endregion-End of Head part declearation

            #region Sale Purchase LV
            if (Carporate_Sale_No != null && Carporate_Sale_No != 0)
            {
                try
                {
                    saleparty = Convert.ToInt32(clsCommon.getString("select ifnull(ac_code,0) as ac_code from qrycarporateheaddetail where Doc_No='" + Carporate_Sale_No + "' and Company_Code=" + Company_Code + " " +
                        " Year_Code='" + Session["year"].ToString() + "'"));
                }
                catch
                {
                }
            }
            if (AUTO_VOUCHER == "YES")
            {
                if (drpDOType.SelectedValue == "DI")
                {
                    if (GETPASS_CODE == SELFAC.ToString() || PDS == "P")
                    {
                        this.PurchasePosting();
                    }

                }
                else
                {
                    this.LV_Posting();

                }
            }
            if (drpDOType.SelectedValue == "DI")
            {
                if (SaleBillTo != Session["SELF_AC"].ToString() && SaleBillTo != "2")
                {
                    this.SalePosting();
                }
            }

            #endregion

            #region Detail Fields
            Detail_Fields = new StringBuilder();
            Detail_Fields.Append("doc_no,");
            Detail_Fields.Append("detail_Id,");
            Detail_Fields.Append("ddType,");
            Detail_Fields.Append("Bank_Code,");
            Detail_Fields.Append("Narration,");
            Detail_Fields.Append("Amount,");
            Detail_Fields.Append("Company_Code,");
            Detail_Fields.Append("Year_Code,");
            Detail_Fields.Append("Branch_Code,");
            Detail_Fields.Append("UTR_NO,");
            Detail_Fields.Append("DO_No,");
            Detail_Fields.Append("UtrYearCode,");
            Detail_Fields.Append("LTNo,");

            Detail_Fields.Append("doid,");
            Detail_Fields.Append("dodetailid,");
            Detail_Fields.Append("bc,");
            Detail_Fields.Append("utrdetailid");
            #endregion


            GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='DO' and Doc_No=" + DOC_NO + " and COMPANY_CODE=" + Company_Code +
                " and Year_Code=" + Year_Code + "";
            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = GLEDGER_Delete;
            Maindt.Rows.Add(dr);


            if (btnSave.Text == "Save")
            {
                this.NextNumber();


                #region Head Create Qry


                Head_Fields = new StringBuilder();
                Head_Values = new StringBuilder();

                Head_Fields.Append("doc_no,");
                Head_Values.Append("'" + DOC_NO + "',");
                Head_Fields.Append("tran_type,");
                Head_Values.Append("'" + trnType + "',");
                Head_Fields.Append("doc_date,");
                Head_Values.Append("'" + DOC_DATE + "',");
                Head_Fields.Append("desp_type,");
                Head_Values.Append("'" + DESP_TYPE + "',");
                Head_Fields.Append("mill_code,");
                Head_Values.Append("'" + MILL_CODE + "',");
                Head_Fields.Append("GETPASSCODE,");
                Head_Values.Append("'" + GETPASS_CODE + "',");
                Head_Fields.Append("voucher_by,");
                Head_Values.Append("'" + VOUCHER_BY + "',");
                Head_Fields.Append("grade,");
                Head_Values.Append("'" + GRADE + "',");
                Head_Fields.Append("quantal,");
                Head_Values.Append("'" + QUANTAL + "',");
                Head_Fields.Append("packing,");
                Head_Values.Append("'" + PACKING + "',");
                Head_Fields.Append("bags,");
                Head_Values.Append("'" + BAGS + "',");
                Head_Fields.Append("mill_rate,");
                Head_Values.Append("'" + mill_rate + "',");
                Head_Fields.Append("amount,");
                Head_Values.Append("'" + MILL_AMOUNT + "',");
                Head_Fields.Append("excise_rate,");
                Head_Values.Append("'" + EXCISE_RATE + "',");
                Head_Fields.Append("sale_rate,");
                Head_Values.Append("'" + SALE_RATE + "',");
                Head_Fields.Append("diff_rate,");
                Head_Values.Append("'" + Diff_Rate + "',");
                Head_Fields.Append("diff_amount,");
                Head_Values.Append("'" + DIFF_AMOUNT + "',");
                Head_Fields.Append("DO,");
                Head_Values.Append("'" + DO_CODE + "',");
                Head_Fields.Append("broker,");
                Head_Values.Append("'" + BROKER_CODE + "',");
                Head_Fields.Append("truck_no,");
                Head_Values.Append("'" + TRUCK_NO + "',");
                Head_Fields.Append("transport,");
                Head_Values.Append("'" + TRANSPORT_CODE + "',");
                Head_Fields.Append("narration1,");
                Head_Values.Append("'" + myNarration + "',");
                Head_Fields.Append("narration2,");
                Head_Values.Append("'" + NARRATION2 + "',");
                Head_Fields.Append("narration3,");
                Head_Values.Append("'" + NARRATION3 + myNarration3 + "',");
                Head_Fields.Append("narration4,");
                Head_Values.Append("'" + NARRATION4 + myNarration4 + "',");
                Head_Fields.Append("company_code,");
                Head_Values.Append("'" + Company_Code + "',");
                Head_Fields.Append("Year_Code,");
                Head_Values.Append("'" + Year_Code + "',");
                Head_Fields.Append("Branch_Code,");
                Head_Values.Append("'" + Branch_Code + "',");
                Head_Fields.Append("purc_no,");
                Head_Values.Append("'" + purc_no + "',");
                Head_Fields.Append("Purchase_Date,");
                Head_Values.Append("'" + PUR_DATE + "',");
                Head_Fields.Append("purc_order,");
                Head_Values.Append("'" + purc_order + "',");
                Head_Fields.Append("Created_By,");
                Head_Values.Append("'" + user + "',");
                Head_Fields.Append("UTR_Year_Code,");
                Head_Values.Append("'" + UTR_Year_Code + "',");
                Head_Fields.Append("Carporate_Sale_No,");
                Head_Values.Append("'" + Carporate_Sale_No + "',");
                Head_Fields.Append("Carporate_Sale_Year_Code,");
                Head_Values.Append("'" + Carporate_Sale_Year_Code + "',");
                Head_Fields.Append("final_amout,");
                Head_Values.Append("'" + MILL_AMOUNT + "',");
                Head_Fields.Append("memo_no,");
                Head_Values.Append("'" + memo_no + "',");
                Head_Fields.Append("Ac_Code,");
                Head_Values.Append("'" + Ac_Code + "',");
                Head_Fields.Append("FreightPerQtl,");
                Head_Values.Append("'" + FRIEGHT_RATE + "',");
                Head_Fields.Append("Freight_Amount,");
                Head_Values.Append("'" + FRIEGHT_AMOUNT + "',");
                Head_Fields.Append("vasuli_rate,");
                Head_Values.Append("'" + VASULI_RATE + "',");
                Head_Fields.Append("vasuli_amount,");
                Head_Values.Append("'" + VASULI_AMOUNT + "',");
                Head_Fields.Append("Memo_Advance,");
                Head_Values.Append("'" + MEMO_ADVANCE + "',");
                Head_Fields.Append("Delivery_Type,");
                Head_Values.Append("'" + Delivery_Type + "',");
                Head_Fields.Append("driver_no,");
                Head_Values.Append("'" + Driver_Mobile + "',");
                Head_Fields.Append("WhoseFrieght,");
                Head_Values.Append("'" + WhoseFrieght + "',");
                Head_Fields.Append("Invoice_No,");
                Head_Values.Append("'" + INVOICE_NO + "',");
                Head_Fields.Append("vasuli_rate1,");
                Head_Values.Append("'" + VASULI_RATE_1 + "',");
                Head_Fields.Append("vasuli_amount1,");
                Head_Values.Append("'" + VASULI_AMOUNT_1 + "',");
                Head_Fields.Append("MM_CC,");
                Head_Values.Append("'" + MM_CC + "',");
                Head_Fields.Append("MM_Rate,");
                Head_Values.Append("'" + MM_Rate + "',");
                Head_Fields.Append("Voucher_Brokrage,");
                Head_Values.Append("'" + VoucherBrokrage + "',");
                Head_Fields.Append("Voucher_Service_Charge,");
                Head_Values.Append("'" + VoucherServiceCharge + "',");
                Head_Fields.Append("Voucher_RateDiffRate,");
                Head_Values.Append("'" + VoucherRateDiffRate + "',");
                Head_Fields.Append("Voucher_RateDiffAmt,");
                Head_Values.Append("'" + VoucherRateDiffAmt + "',");
                Head_Fields.Append("Voucher_BankCommRate,");
                Head_Values.Append("'" + VoucherBankCommRate + "',");
                Head_Fields.Append("Voucher_BankCommAmt,");
                Head_Values.Append("'" + VoucherBankCommAmt + "',");
                Head_Fields.Append("Voucher_Interest,");
                Head_Values.Append("'" + VoucherInterest + "',");
                Head_Fields.Append("Voucher_TransportAmt,");
                Head_Values.Append("'" + VoucherTransport + "',");
                Head_Fields.Append("Voucher_OtherExpenses,");
                Head_Values.Append("'" + VoucherOtherExpenses + "',");
                Head_Fields.Append("CheckPost,");
                Head_Values.Append("'" + CheckPost + "',");
                Head_Fields.Append("SaleBillTo,");
                Head_Values.Append("'" + SaleBillTo + "',");
                Head_Fields.Append("Tender_Commission,");
                Head_Values.Append("'" + Tender_Commission + "',");
                Head_Fields.Append("Pan_No,");
                Head_Values.Append("'" + PAN_NO + "',");
                Head_Fields.Append("Vasuli_Ac,");
                Head_Values.Append("'" + VASULI_AC + "',");
                Head_Fields.Append("narration5,");
                Head_Values.Append("'" + NARRATION5 + "',");
                Head_Fields.Append("GstRateCode,");
                Head_Values.Append("'" + GSTRateCode + "',");
                Head_Fields.Append("GetpassGstStateCode,");
                Head_Values.Append("'" + GetpassGstStateCode + "',");
                Head_Fields.Append("VoucherbyGstStateCode,");
                Head_Values.Append("'" + VoucherbyGstStateCode + "',");
                Head_Fields.Append("SalebilltoGstStateCode,");
                Head_Values.Append("'" + SalebilltoGstStateCode + "',");
                Head_Fields.Append("GstAmtOnMR,");
                Head_Values.Append("'" + GSTMillRateAmount + "',");

                Head_Fields.Append("GstAmtOnSR,");
                Head_Values.Append("'" + GSTSaleRateAmount + "',");
                Head_Fields.Append("GstExlSR,");
                Head_Values.Append("'" + GSTExclSaleRateAmount + "',");
                Head_Fields.Append("GstExlMR,");
                Head_Values.Append("'" + GSTExclMillRateAmount + "',");
                Head_Fields.Append("MillGSTStateCode,");
                Head_Values.Append("'" + MillGstStateCode + "',");
                Head_Fields.Append("TransportGSTStateCode,");
                Head_Values.Append("'" + TransportGstStateCode + "',");
                Head_Fields.Append("EWay_Bill_No,");
                Head_Values.Append("'" + EWayBill_No + "',");
                Head_Fields.Append("Distance,");
                Head_Values.Append("'" + Distance + "',");
                Head_Fields.Append("EWayBillChk,");
                Head_Values.Append("'" + EWay_BillChk + "',");
                Head_Fields.Append("MillInvoiceNo,");
                Head_Values.Append("'" + MillInvoiceno + "',");
                Head_Fields.Append("carporate_ac,");
                Head_Values.Append("'" + Bill_To + "',");
                Head_Fields.Append("mill_inv_date,");
                Head_Values.Append("'" + MillInv_Date + "',");
                Head_Fields.Append("mill_rcv,");
                Head_Values.Append("'" + Inv_Chk + "',");

                Head_Fields.Append("ca,");
                Head_Values.Append("case when 0=" + bt + " then null else " + bt + " end,");
                Head_Fields.Append("doid,");
                Head_Values.Append("case when 0=" + doid + " then null else " + doid + " end,");
                Head_Fields.Append("mc,");
                Head_Values.Append("case when 0=" + mc + " then null else " + mc + " end,");
                Head_Fields.Append("gp,");
                Head_Values.Append("case when 0=" + gp + " then null else " + gp + " end,");
                Head_Fields.Append("st,");
                Head_Values.Append("case when 0=" + st + " then null else " + st + " end,");
                Head_Fields.Append("sb,");
                Head_Values.Append("case when 0=" + sb + " then null else " + sb + " end,");
                Head_Fields.Append("tc ,");
                Head_Values.Append("case when 0=" + tc + " then null else " + tc + " end,");
                Head_Fields.Append("itemcode,");
                Head_Values.Append("case when 0=" + itemcode + " then null else " + itemcode + " end,");
                Head_Fields.Append("cs,");
                Head_Values.Append("case when 0=" + cscode + " then null else " + cscode + " end,");
                Head_Fields.Append("ic,");
                Head_Values.Append("case when 0=" + ic + " then null else " + ic + " end,");
                Head_Fields.Append("vb,");
                Head_Values.Append("case when 0=" + st + " then null else " + st + " end,");
                Head_Fields.Append("va,");
                Head_Values.Append("case when 0=" + va + " then null else " + va + " end,");
                //Head_Fields.Append("tenderdetailid,");
                //Head_Values.Append("case when 0=" + hdnf.Value + " then null else " + hdnf.Value + " end,");
                Head_Fields.Append("docd,");
                Head_Values.Append("case when 0=" + docd + " then null else " + docd + " end,");
                Head_Fields.Append("bk,");
                Head_Values.Append("case when 0=" + bk + " then null else " + bk + " end,");
                Head_Fields.Append("MillEwayBill");
                Head_Values.Append("'" + MillEwayBill + "'");
                //Head_Fields = Head_Fields + "docd";
                //Head_Values = Head_Values + "'" + docd + "'";
                //if (btnSave.Text != "Save")
                //{
                //    if (drpDOType.SelectedValue == "DO")
                //    {
                //        Head_Fields=Head_Fields+"VoucherNo,";
                //    }
                //    else
                //    {
                //        Head_Fields=Head_Fields+"voucher_no,";
                //        if (txtSaleBillTo.Text != "0")
                //        {
                //            Head_Fields=Head_Fields+"SB_No,";
                //        }

                //    }
                //}


                #endregion

                Head_Insert = "insert into " + tblHead + " (" + Head_Fields + ") values (" + Head_Values + ")";

                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Head_Insert;
                Maindt.Rows.Add(dr);
                #region Detail Crate Qry

                Detail_Values = new StringBuilder();
                int dodetailid = Convert.ToInt32(clsCommon.getString("select ifnull(count(dodetailid),0) as dodetailid from " + tblDetails + " "));
                if (dodetailid == 0)
                {
                    dodetailid = 1;
                }
                else
                {
                    dodetailid = Convert.ToInt32(clsCommon.getString("select max(dodetailid) as dodetailid from " + tblDetails + " "));
                }
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    if (grdDetail.Rows[i].Cells[7].Text == "0.00")
                    {
                        continue;
                    }
                    dodetailid = dodetailid + 1;
                    detail_Id = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);
                    ddType = grdDetail.Rows[i].Cells[3].Text;
                    Bank_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[4].Text);
                    bc = Convert.ToInt32(clsCommon.getString("select accoid from " + qryAccountList + " where Ac_Code=" + Bank_Code + " "));

                    Narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[6].Text);
                    Amount = Convert.ToDouble(grdDetail.Rows[i].Cells[7].Text);
                    Utr_no = Convert.ToInt32(grdDetail.Rows[i].Cells[8].Text);
                    LT_no = Convert.ToInt32(grdDetail.Rows[i].Cells[9].Text);
                    UTRDetail_ID = Convert.ToInt32(grdDetail.Rows[i].Cells[11].Text);
                    GID = GID + 1;
                    try
                    {
                        utrdetailid = Convert.ToInt32(clsCommon.getString("select iffnull(utrid,0) as id from nt_1_utr where doc_no=" + Utr_no + " and company_code=" + Company_Code + " and Year_Code=" + Year_Code + ""));
                    }
                    catch { }
                    if (grdDetail.Rows[i].Cells[12].Text != "D")
                    {
                        Detail_Values.Append("('" + DOC_NO + "','" + detail_Id + "','" + ddType + "','" + Bank_Code + "','" + Narration + "','" + Amount + "'," +
                        " '" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "','" + Utr_no + "','" + DOC_NO + "','" + UTR_Year_Code + "','" + LT_no + "'," +
                        " '" + doid + "','" + dodetailid + "','" + bc + "',case when 0=" + UTRDetail_ID + " then null else " + UTRDetail_ID + " end),");
                    }
                }

                if (Detail_Values.Length > 0)
                {
                    Detail_Values.Remove(Detail_Values.Length - 1, 1);
                    Detail_Insert = "insert into " + tblDetails + "(" + Detail_Fields + ") values " + Detail_Values + "";

                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = Detail_Insert;
                    Maindt.Rows.Add(dr);
                }
                #endregion

                flag = 1;

            }
            else
            {
                doid = Convert.ToInt32(lbldoid.Text);
                #region Update Head

                Head_Update = new StringBuilder();
                //Head_Update = Head_Update + "doc_no,";
                //Head_Update = Head_Update + "'" + DOC_NO + "',";
                //Head_Update = Head_Update + "tran_type,";
                //Head_Update = Head_Update + "'" + trnType + "',";
                Head_Update.Append("doc_date=");
                Head_Update.Append("'" + DOC_DATE + "',");
                Head_Update.Append("desp_type=");
                Head_Update.Append("'" + DESP_TYPE + "',");
                Head_Update.Append("mill_code=");
                Head_Update.Append("'" + MILL_CODE + "',");
                Head_Update.Append("GETPASSCODE=");
                Head_Update.Append("'" + GETPASS_CODE + "',");
                Head_Update.Append("voucher_by=");
                Head_Update.Append("'" + VOUCHER_BY + "',");
                Head_Update.Append("grade=");
                Head_Update.Append("'" + GRADE + "',");
                Head_Update.Append("quantal=");
                Head_Update.Append("'" + QUANTAL + "',");
                Head_Update.Append("packing=");
                Head_Update.Append("'" + PACKING + "',");
                Head_Update.Append("bags=");
                Head_Update.Append("'" + BAGS + "',");
                Head_Update.Append("mill_rate=");
                Head_Update.Append("'" + mill_rate + "',");
                Head_Update.Append("amount=");
                Head_Update.Append("'" + MILL_AMOUNT + "',");
                Head_Update.Append("excise_rate=");
                Head_Update.Append("'" + EXCISE_RATE + "',");
                Head_Update.Append("sale_rate=");
                Head_Update.Append("'" + SALE_RATE + "',");
                Head_Update.Append("diff_rate=");
                Head_Update.Append("'" + Diff_Rate + "',");
                Head_Update.Append("diff_amount=");
                Head_Update.Append("'" + DIFF_AMOUNT + "',");
                Head_Update.Append("DO=");
                Head_Update.Append("'" + DO_CODE + "',");
                Head_Update.Append("broker=");
                Head_Update.Append("'" + BROKER_CODE + "',");
                Head_Update.Append("truck_no=");
                Head_Update.Append("'" + TRUCK_NO + "',");
                Head_Update.Append("transport=");
                Head_Update.Append("'" + TRANSPORT_CODE + "',");
                Head_Update.Append("narration1=");
                Head_Update.Append("'" + myNarration + "',");
                Head_Update.Append("narration2=");
                Head_Update.Append("'" + NARRATION2 + "',");
                Head_Update.Append("narration3=");
                Head_Update.Append("'" + NARRATION3 + myNarration3 + "',");
                Head_Update.Append("narration4=");
                Head_Update.Append("'" + NARRATION4 + myNarration4 + "',");
                Head_Update.Append("company_code=");
                Head_Update.Append("'" + Company_Code + "',");
                Head_Update.Append("Year_Code=");
                Head_Update.Append("'" + Year_Code + "',");
                Head_Update.Append("Branch_Code=");
                Head_Update.Append("'" + Branch_Code + "',");
                Head_Update.Append("purc_no=");
                Head_Update.Append("'" + purc_no + "',");
                Head_Update.Append("Purchase_Date=");
                Head_Update.Append("'" + PUR_DATE + "',");
                Head_Update.Append("purc_order=");
                Head_Update.Append("'" + purc_order + "',");
                Head_Update.Append("Modified_By=");
                Head_Update.Append("'" + user + "',");
                Head_Update.Append("UTR_Year_Code=");
                Head_Update.Append("'" + UTR_Year_Code + "',");
                Head_Update.Append("Carporate_Sale_No=");
                Head_Update.Append("'" + Carporate_Sale_No + "',");
                Head_Update.Append("Carporate_Sale_Year_Code=");
                Head_Update.Append("'" + Carporate_Sale_Year_Code + "',");
                Head_Update.Append("final_amout=");
                Head_Update.Append("'" + MILL_AMOUNT + "',");
                Head_Update.Append("memo_no=");
                Head_Update.Append("'" + memo_no + "',");
                Head_Update.Append("Ac_Code=");
                Head_Update.Append("'" + Ac_Code + "',");
                Head_Update.Append("FreightPerQtl=");
                Head_Update.Append("'" + FRIEGHT_RATE + "',");
                Head_Update.Append("Freight_Amount=");
                Head_Update.Append("'" + FRIEGHT_AMOUNT + "',");
                Head_Update.Append("vasuli_rate=");
                Head_Update.Append("'" + VASULI_RATE + "',");
                Head_Update.Append("vasuli_amount=");
                Head_Update.Append("'" + VASULI_AMOUNT + "',");
                Head_Update.Append("Memo_Advance=");
                Head_Update.Append("'" + MEMO_ADVANCE + "',");
                Head_Update.Append("Delivery_Type=");
                Head_Update.Append("'" + Delivery_Type + "',");
                Head_Update.Append("driver_no=");
                Head_Update.Append("'" + Driver_Mobile + "',");
                Head_Update.Append("WhoseFrieght=");
                Head_Update.Append("'" + WhoseFrieght + "',");
                Head_Update.Append("Invoice_No=");
                Head_Update.Append("'" + INVOICE_NO + "',");
                Head_Update.Append("vasuli_rate1=");
                Head_Update.Append("'" + VASULI_RATE_1 + "',");
                Head_Update.Append("vasuli_amount1=");
                Head_Update.Append("'" + VASULI_AMOUNT_1 + "',");
                Head_Update.Append("MM_CC=");
                Head_Update.Append("'" + MM_CC + "',");
                Head_Update.Append("MM_Rate=");
                Head_Update.Append("'" + MM_Rate + "',");
                Head_Update.Append("Voucher_Brokrage=");
                Head_Update.Append("'" + VoucherBrokrage + "',");
                Head_Update.Append("Voucher_Service_Charge=");
                Head_Update.Append("'" + VoucherServiceCharge + "',");
                Head_Update.Append("Voucher_RateDiffRate=");
                Head_Update.Append("'" + VoucherRateDiffRate + "',");
                Head_Update.Append("Voucher_RateDiffAmt=");
                Head_Update.Append("'" + VoucherRateDiffAmt + "',");
                Head_Update.Append("Voucher_BankCommRate=");
                Head_Update.Append("'" + VoucherBankCommRate + "',");
                Head_Update.Append("Voucher_BankCommAmt=");
                Head_Update.Append("'" + VoucherBankCommAmt + "',");
                Head_Update.Append("Voucher_Interest=");
                Head_Update.Append("'" + VoucherInterest + "',");
                Head_Update.Append("Voucher_TransportAmt=");
                Head_Update.Append("'" + VoucherTransport + "',");
                Head_Update.Append("Voucher_OtherExpenses=");
                Head_Update.Append("'" + VoucherOtherExpenses + "',");
                Head_Update.Append("CheckPost=");
                Head_Update.Append("'" + CheckPost + "',");
                Head_Update.Append("SaleBillTo=");
                Head_Update.Append("'" + SaleBillTo + "',");
                Head_Update.Append("Tender_Commission=");
                Head_Update.Append("'" + Tender_Commission + "',");
                Head_Update.Append("Pan_No=");
                Head_Update.Append("'" + PAN_NO + "',");
                Head_Update.Append("Vasuli_Ac=");
                Head_Update.Append("'" + VASULI_AC + "',");
                Head_Update.Append("narration5=");
                Head_Update.Append("'" + NARRATION5 + "',");
                Head_Update.Append("GstRateCode=");
                Head_Update.Append("'" + GSTRateCode + "',");
                Head_Update.Append("GetpassGstStateCode=");
                Head_Update.Append("'" + GetpassGstStateCode + "',");
                Head_Update.Append("VoucherbyGstStateCode=");
                Head_Update.Append("'" + VoucherbyGstStateCode + "',");
                Head_Update.Append("SalebilltoGstStateCode=");
                Head_Update.Append("'" + SalebilltoGstStateCode + "',");
                Head_Update.Append("GstAmtOnMR=");
                Head_Update.Append("'" + GSTMillRateAmount + "',");

                Head_Update.Append("GstAmtOnSR=");
                Head_Update.Append("'" + GSTSaleRateAmount + "',");
                Head_Update.Append("GstExlSR=");
                Head_Update.Append("'" + GSTExclSaleRateAmount + "',");
                Head_Update.Append("GstExlMR=");
                Head_Update.Append("'" + GSTExclMillRateAmount + "',");
                Head_Update.Append("MillGSTStateCode=");
                Head_Update.Append("'" + MillGstStateCode + "',");
                Head_Update.Append("TransportGSTStateCode=");
                Head_Update.Append("'" + TransportGstStateCode + "',");
                Head_Update.Append("EWay_Bill_No=");
                Head_Update.Append("'" + EWayBill_No + "',");
                Head_Update.Append("Distance=");
                Head_Update.Append("'" + Distance + "',");
                Head_Update.Append("EWayBillChk=");
                Head_Update.Append("'" + EWay_BillChk + "',");
                Head_Update.Append("MillInvoiceNo=");
                Head_Update.Append("'" + MillInvoiceno + "',");
                Head_Update.Append("carporate_ac=");
                Head_Update.Append("'" + Bill_To + "',");
                Head_Update.Append("mill_inv_date=");
                Head_Update.Append("'" + MillInv_Date + "',");
                Head_Update.Append("mill_rcv=");
                Head_Update.Append("'" + Inv_Chk + "',");

                Head_Update.Append("ca=");
                Head_Update.Append("case when 0=" + bt + " then null else " + bt + " end,");
                Head_Update.Append("mc=");
                Head_Update.Append("case when 0=" + mc + " then null else " + mc + " end,");
                Head_Update.Append("gp=");
                Head_Update.Append("case when 0=" + gp + " then null else " + gp + " end,");
                Head_Update.Append("st=");
                Head_Update.Append("case when 0=" + st + " then null else " + st + " end,");
                Head_Update.Append("sb=");
                Head_Update.Append("case when 0=" + sb + " then null else " + sb + " end,");
                Head_Update.Append("tc=");
                Head_Update.Append("case when 0=" + tc + " then null else " + tc + " end,");
                Head_Update.Append("itemcode=");
                Head_Update.Append("case when 0=" + itemcode + " then null else " + itemcode + " end,");
                Head_Update.Append("cs=");
                Head_Update.Append("case when 0=" + cscode + " then null else " + cscode + " end,");
                Head_Update.Append("ic=");
                Head_Update.Append("case when 0=" + ic + " then null else " + ic + " end,");
                Head_Update.Append("vb=");
                Head_Update.Append("case when 0=" + st + " then null else " + st + " end,");
                Head_Update.Append("va=");
                Head_Update.Append("case when 0=" + va + " then null else " + va + " end,");
                Head_Update.Append("docd=");
                Head_Update.Append("case when 0=" + docd + " then null else " + docd + " end,");
                Head_Update.Append("MillEwayBill=");
                Head_Update.Append("'" + MillEwayBill + "',");
                Head_Update.Append("bk=");
                Head_Update.Append("'" + bk + "'");
                #endregion
                string Head_Updateqry = "update " + tblHead + " set " + Head_Update + " where doid='" + lbldoid.Text + "'";
                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Head_Updateqry;
                Maindt.Rows.Add(dr);
                #region Detail Crate Qry

                Detail_Values = new StringBuilder();
                Detail_Update = new StringBuilder();
                Detail_Delete = new StringBuilder();

                string concatid = string.Empty;
                int dodetailid = Convert.ToInt32(clsCommon.getString("select ifnull(count(dodetailid),0) as dodetailid from " + tblDetails + " "));
                if (dodetailid == 0)
                {
                    dodetailid = 1;
                }
                else
                {
                    dodetailid = Convert.ToInt32(clsCommon.getString("select max(dodetailid) as dodetailid from " + tblDetails + " "));
                }
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {

                    detail_Id = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);
                    ddType = grdDetail.Rows[i].Cells[3].Text;
                    Bank_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[4].Text);
                    Narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[6].Text);
                    bc = Convert.ToInt32(clsCommon.getString("select accoid from " + qryAccountList + " where Ac_Code=" + Bank_Code + " "));
                    Amount = Convert.ToDouble(grdDetail.Rows[i].Cells[7].Text);
                    Utr_no = Convert.ToInt32(grdDetail.Rows[i].Cells[8].Text);
                    LT_no = Convert.ToInt32(grdDetail.Rows[i].Cells[9].Text);
                    if (grdDetail.Rows[i].Cells[11].Text == "&nbsp;" || grdDetail.Rows[i].Cells[11].Text == string.Empty)
                    {
                        UTRDetail_ID = 0;
                    }
                    else
                    {
                        UTRDetail_ID = Convert.ToInt32(grdDetail.Rows[i].Cells[11].Text);
                    }
                    GID = GID + 1;
                    int doGridid = Convert.ToInt32(grdDetail.Rows[i].Cells[10].Text);
                    try
                    {
                        utrdetailid = Convert.ToInt32(clsCommon.getString("select iffnull(utrid,0) as id from nt_1_utr where doc_no=" + Utr_no + " and company_code=" + Company_Code + " and Year_Code=" + Year_Code + ""));
                    }
                    catch { }

                    if (grdDetail.Rows[i].Cells[12].Text == "A")
                    {
                        dodetailid = dodetailid + 1;
                        Detail_Values.Append("('" + DOC_NO + "','" + detail_Id + "','" + ddType + "','" + Bank_Code + "','" + Narration + "','" + Amount + "'," +
                        " '" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "','" + Utr_no + "','" + DOC_NO + "','" + UTR_Year_Code + "','" + LT_no + "'," +
                        " '" + lbldoid.Text + "','" + dodetailid + "','" + bc + "',case when 0=" + UTRDetail_ID + " then null else " + UTRDetail_ID + " end),");
                    }
                    else if (grdDetail.Rows[i].Cells[12].Text == "U")
                    {
                        Detail_Update.Append("ddType=case dodetailid when '" + doGridid + "' then '" + ddType + "'  ELSE ddType END,");
                        Detail_Update.Append("Bank_Code=case dodetailid when '" + doGridid + "' then '" + Bank_Code + "'  ELSE Bank_Code END,");
                        Detail_Update.Append("Narration=case dodetailid when '" + doGridid + "' then '" + Narration + "'  ELSE Narration END,");
                        Detail_Update.Append("Amount=case dodetailid when '" + doGridid + "' then '" + Amount + "'  ELSE Amount END,");
                        Detail_Update.Append("UTR_NO=case dodetailid when '" + doGridid + "' then '" + Utr_no + "'  ELSE UTR_NO END,");
                        Detail_Update.Append("LTNo=case dodetailid when '" + doGridid + "' then '" + LT_no + "'  ELSE LTNo END,");
                        Detail_Update.Append("bc=case dodetailid when '" + doGridid + "' then '" + bc + "'  ELSE bc END,");
                        Detail_Update.Append("utrdetailid=case dodetailid when '" + doGridid + "' then case when 0=" + UTRDetail_ID + " then null else " + UTRDetail_ID + " end  ELSE utrdetailid END,");
                        concatid = concatid + "'" + doGridid + "',";
                    }
                    if (grdDetail.Rows[i].Cells[12].Text == "D")
                    {
                        Detail_Delete.Append("'" + grdDetail.Rows[i].Cells[10].Text + "',");
                    }
                }

                if (Detail_Values.Length > 0)
                {
                    Detail_Values.Remove(Detail_Values.Length - 1, 1);
                    Detail_Insert = "insert into " + tblDetails + "(" + Detail_Fields + ") values " + Detail_Values + "";

                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = Detail_Insert;
                    Maindt.Rows.Add(dr);
                }
                if (Detail_Delete.Length > 0)
                {
                    Detail_Delete.Remove(Detail_Delete.Length - 1, 1);
                    string Detail_Deleteqry = "delete from " + tblDetails + " where dodetailid in(" + Detail_Delete + ")";

                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = Detail_Deleteqry;
                    Maindt.Rows.Add(dr);
                }
                if (Detail_Update.Length > 0)
                {
                    concatid = concatid.Remove(concatid.Length - 1);
                    Detail_Update.Remove(Detail_Update.Length - 1, 1);
                    string Detail_Updateqry = "update " + tblDetails + " set " + Detail_Update + " where dodetailid in(" + concatid + ")";

                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = Detail_Updateqry;
                    Maindt.Rows.Add(dr);
                }
                #endregion

                flag = 2;
                doid = Convert.ToInt32(lbldoid.Text);
            }


            #region Memo
            if (AUTO_VOUCHER == "YES")
            {

                if (MEMO_ADVANCE + VASULI_AMOUNT_1 != 0)
                {
                    if (lblMemoNo.Text != string.Empty && lblMemoNo.Text == "0")
                    {
                        memo_no = DOC_NO;
                    }
                    else
                    {
                        memo_no = Convert.ToInt32(lblMemoNo.Text != string.Empty ? lblMemoNo.Text : "0");
                    }
                }
            }
            #endregion
            #region Gledger effect
            FormTypes types = new FormTypes();
            string Gledger_values = string.Empty;
            string Gledger_Column = "TRAN_TYPE,CASHCREDIT,DOC_NO,DOC_DATE,AC_CODE,UNIT_code,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE," +
                        " YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO,ac,vc,progid,tranid";
            int Order_Code = 1;
            string CASHID = Session["CASHID"].ToString();

            string freightac = Session["Freight_Ac"].ToString();
            string freightac_id = string.Empty;
            if (freightac != string.Empty)
            {
                freightac_id = Session["freightac_id"].ToString();
            }
            else
            {
                freightac = "0";
                freightac_id = "0";
            }

            string millShortname = hdnfmillshortname.Value;
            string TransportShort = hdnftransportshortname.Value;

            string Partyshortname = hdnfsalebilltoshortname.Value;
            string CreditNarration = "" + millShortname + "  " + txtquantal.Text + "  L:" + txtTruck_NO.Text +
                "  DO:" + txtdoc_no.Text + "  F:" + MEMO_ADVANCE + "";
            string TransportNarration = "" + txtquantal.Text + "  " + millShortname + "  " + TransportShort + "  Lorry:" + txtTruck_NO.Text + "  Party:" + Partyshortname + "";

            if (GETPASS_CODE != Session["SELF_AC"].ToString())
            {
                if (DESP_TYPE == "DI")
                {
                    for (int i = 0; i < grdDetail.Rows.Count; i++)
                    {
                        Bank_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[4].Text);
                        Amount = Convert.ToDouble(grdDetail.Rows[i].Cells[7].Text);

                        bc = Convert.ToInt32(clsCommon.getString("select accoid from " + qryAccountList + " where Ac_Code=" + Bank_Code + " and Company_Code=" + Company_Code + " "));

                        Gledger_values = Gledger_values + "('DO','','" + DOC_NO + "','" + DOC_DATE + "','" + Bank_Code + "','0','','" + Amount + "', " +
                                                      " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','C','" + VOUCHER_BY + "',0,'" + Branch_Code + "','DO','" + DOC_NO + "'," +
                                                      " '" + bc + "','0','0','0'),";

                        Order_Code = Order_Code + 1;
                        Gledger_values = Gledger_values + "('DO','','" + DOC_NO + "','" + DOC_DATE + "','" + VOUCHER_BY + "','0','','" + Amount + "', " +
                                                     " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','C','" + Bank_Code + "',0,'" + Branch_Code + "','DO','" + DOC_NO + "'," +
                                                     " case when 0=" + st + " then null else " + st + " end,'0','0','0'),";

                    }
                }
                if (VASULI_AMOUNT_1 > 0)
                {
                    Order_Code = Order_Code + 1;
                    Gledger_values = Gledger_values + "('DO','','" + DOC_NO + "','" + DOC_DATE + "','" + GETPASS_CODE + "','0','','" + VASULI_AMOUNT_1 + "', " +
                                                   " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','D','9999971',0,'" + Branch_Code + "','DO','" + DOC_NO + "'," +
                                                   " case when 0=" + gp + " then null else " + gp + " end,'0','" + types.TT_DO + "','0'),";

                    Order_Code = Order_Code + 1;
                    Gledger_values = Gledger_values + "('DO','','" + DOC_NO + "','" + DOC_DATE + "','" + txtVasuliAc.Text + "','0','','" + VASULI_AMOUNT + "', " +
                                                   " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','C','" + GETPASS_CODE + "',0,'" + Branch_Code + "','DO','" + DOC_NO + "'," +
                                                   " case when 0=" + va + " then null else " + va + " end,'0','" + types.TT_DO + "','0'),";
                }

                if (MEMO_ADVANCE > 0)
                {

                    Order_Code = Order_Code + 1;
                    Gledger_values = Gledger_values + "('DO','','" + DOC_NO + "','" + DOC_DATE + "','" + freightac + "','0','" + TransportNarration + "','" + MEMO_ADVANCE + "', " +
                                                   " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','D','" + freightac + "',0,'" + Branch_Code + "','DO','" + DOC_NO + "'," +
                                                   " case when 0=" + freightac_id + " then null else " + freightac_id + " end,'0','" + types.TT_DO + "','0'),";
                    Order_Code = Order_Code + 1;
                    Gledger_values = Gledger_values + "('DO','','" + DOC_NO + "','" + DOC_DATE + "','" + TRANSPORT_CODE + "','0','" + TransportNarration + "','" + MEMO_ADVANCE + "', " +
                                                   " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','C','" + TRANSPORT_CODE + "',0,'" + Branch_Code + "','DO','" + DOC_NO + "'," +
                                                   " case when 0=" + tc + " then null else " + tc + " end,'0','" + types.TT_DO + "','0'),";
                }
                if (VASULI_AMOUNT != 0)
                {
                    Order_Code = Order_Code + 1;
                    Gledger_values = Gledger_values + "('DO','','" + DOC_NO + "','" + DOC_DATE + "','1','0','" + TransportNarration + "','" + VASULI_AMOUNT + "', " +
                                                   " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','C','1',0,'" + Branch_Code + "','DO','" + DOC_NO + "'," +
                                                   " case when 0=" + CASHID + " then null else " + CASHID + " end,'0','" + types.TT_DO + "','0'),";
                    Order_Code = Order_Code + 1;
                    Gledger_values = Gledger_values + "('DO','','" + DOC_NO + "','" + DOC_DATE + "','" + TRANSPORT_CODE + "','0','" + TransportNarration + "','" + VASULI_AMOUNT + "', " +
                                                   " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','D','" + TRANSPORT_CODE + "',0,'" + Branch_Code + "','DO','" + DOC_NO + "'," +
                                                   " case when 0=" + tc + " then null else " + tc + " end,'0','" + types.TT_DO + "','0'),";
                }
                if (Gledger_values.Length > 0)
                {
                    Gledger_values = Gledger_values.Remove(Gledger_values.Length - 1);
                    GLEDGER_Insert = "insert into nt_1_gledger (" + Gledger_Column + ") values " + Gledger_values + " ";

                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = GLEDGER_Insert;
                    Maindt.Rows.Add(dr);
                }

            }
            else
            {
                if (MEMO_ADVANCE > 0)
                {

                    Order_Code = Order_Code + 1;
                    Gledger_values = Gledger_values + "('DO','','" + DOC_NO + "','" + DOC_DATE + "','" + freightac + "','0','" + TransportNarration + "','" + MEMO_ADVANCE + "', " +
                                                   " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','D','" + freightac + "',0,'" + Branch_Code + "','DO','" + DOC_NO + "'," +
                                                   " case when 0=" + freightac_id + " then null else " + freightac_id + " end,'0','" + types.TT_DO + "','0'),";
                    Order_Code = Order_Code + 1;
                    Gledger_values = Gledger_values + "('DO','','" + DOC_NO + "','" + DOC_DATE + "','" + TRANSPORT_CODE + "','0','" + TransportNarration + "','" + MEMO_ADVANCE + "', " +
                                                   " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','C','" + TRANSPORT_CODE + "',0,'" + Branch_Code + "','DO','" + DOC_NO + "'," +
                                                   " case when 0=" + tc + " then null else " + tc + " end,'0','" + types.TT_DO + "','0'),";
                }
                if (VASULI_AMOUNT != 0)
                {
                    Order_Code = Order_Code + 1;
                    Gledger_values = Gledger_values + "('DO','','" + DOC_NO + "','" + DOC_DATE + "','1','0','" + TransportNarration + "','" + VASULI_AMOUNT + "', " +
                                                   " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','C','1',0,'" + Branch_Code + "','DO','" + DOC_NO + "'," +
                                                   " case when 0=" + CASHID + " then null else " + CASHID + " end,'0','" + types.TT_DO + "','0'),";
                    Order_Code = Order_Code + 1;
                    Gledger_values = Gledger_values + "('DO','','" + DOC_NO + "','" + DOC_DATE + "','" + TRANSPORT_CODE + "','0','" + TransportNarration + "','" + VASULI_AMOUNT + "', " +
                                                   " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','D','" + TRANSPORT_CODE + "',0,'" + Branch_Code + "','DO','" + DOC_NO + "'," +
                                                   " case when 0=" + tc + " then null else " + tc + " end,'0','" + types.TT_DO + "','0'),";
                }
                if (Gledger_values.Length > 0)
                {
                    Gledger_values = Gledger_values.Remove(Gledger_values.Length - 1);
                    GLEDGER_Insert = "insert into nt_1_gledger (" + Gledger_Column + ") values " + Gledger_values + " ";

                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = GLEDGER_Insert;
                    Maindt.Rows.Add(dr);
                }
            }
            #endregion

            #region Tender Effect
            int TenderDetailID = 0;
            int TenderID = 0;
            int TenderAutoID = 0;
            double TenderQty = 0.00;
            double Tender_TotalQty = 0.00;
            string TenderValues = string.Empty;
            int buyerid = 0;

            #region Column
            string TenderDetail_Fields = "Tender_No,";
            TenderDetail_Fields = TenderDetail_Fields + "Company_Code,";
            TenderDetail_Fields = TenderDetail_Fields + "Buyer,";
            TenderDetail_Fields = TenderDetail_Fields + "Buyer_Quantal,";
            TenderDetail_Fields = TenderDetail_Fields + "Sale_Rate,";
            TenderDetail_Fields = TenderDetail_Fields + "Commission_Rate,";
            TenderDetail_Fields = TenderDetail_Fields + "Sauda_Date,";
            TenderDetail_Fields = TenderDetail_Fields + "Lifting_Date,";
            TenderDetail_Fields = TenderDetail_Fields + "ID,";

            TenderDetail_Fields = TenderDetail_Fields + "Narration,";
            TenderDetail_Fields = TenderDetail_Fields + "Buyer_Party,";
            TenderDetail_Fields = TenderDetail_Fields + "year_code,";
            TenderDetail_Fields = TenderDetail_Fields + "Branch_Id,";
            TenderDetail_Fields = TenderDetail_Fields + "Delivery_Type,";
            TenderDetail_Fields = TenderDetail_Fields + "tenderid,";
            TenderDetail_Fields = TenderDetail_Fields + "tenderdetailid,";
            TenderDetail_Fields = TenderDetail_Fields + "buyerid,";
            TenderDetail_Fields = TenderDetail_Fields + "buyerpartyid,";
            TenderDetail_Fields = TenderDetail_Fields + "sub_broker,";
            TenderDetail_Fields = TenderDetail_Fields + "sbr";
            #endregion
            if (purc_order == 1)
            {
                #region

                if (PDS == "P")
                {

                    buyerid = Convert.ToInt32(clsCommon.getString("select accoid from  qrymstaccountmaster where Ac_Code=" + pdsparty + " and Company_Code="
                                                       + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
                }
                else if (PDS == "C")
                {

                    if (VOUCHER_BY == Session["SELF_AC"].ToString() && (SaleBillTo == "0" || SaleBillTo == ""))
                    {
                        pdsparty = Convert.ToInt32(VOUCHER_BY);
                        buyerid = Convert.ToInt32(hdnfst.Value);
                    }
                    else
                    {

                        pdsparty = Convert.ToInt32(SaleBillTo);
                        buyerid = Convert.ToInt32(hdnfsb.Value);
                    }
                }
                else
                {

                    if (VOUCHER_BY == Session["SELF_AC"].ToString() && (SaleBillTo == "0" || SaleBillTo == ""))
                    {
                        pdsparty = Convert.ToInt32(VOUCHER_BY);

                        buyerid = Convert.ToInt32(hdnfst.Value);
                    }
                    else
                    {

                        pdsparty = Convert.ToInt32(SaleBillTo);

                        buyerid = Convert.ToInt32(hdnfsb.Value);

                    }
                }
                #endregion

                TenderDetailID = Convert.ToInt32(clsCommon.getString("select max(ID) from " + tblPrefix + "Tenderdetails where Tender_No='" + txtPurcNo.Text + "' and " +
                  " Company_Code='" + Company_Code + "'  and year_code='" + Year_Code + "'")) + 1;

                TenderAutoID = Convert.ToInt32(clsCommon.getString("select max(tenderdetailid) from " + tblPrefix + "Tenderdetails ")) + 1;

                TenderID = Convert.ToInt32(clsCommon.getString("select ifnull(tenderid,0) from nt_1_tender where Tender_No=" + txtPurcNo.Text + " " +
                    " and Company_Code='" + Company_Code + "'  and year_code='" + Year_Code + "'"));

                // string buyerPartyid = clsCommon.getString("select ifnull(accoid,0) as accoid from qrymstaccountmaster where Ac_code=" + txtBroker_CODE.Text + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
                string buyerPartyid = hdnfbk.Value;
                if (buyerPartyid == string.Empty)
                {
                    buyerPartyid = "0";
                }
                string sub_brokerid = clsCommon.getString("select ifnull(accoid,0) as accoid from qrymstaccountmaster where Ac_code=2 and Company_Code='" + Company_Code + "'");
                if (sub_brokerid == string.Empty)
                {
                    sub_brokerid = "0";
                }

                if (btnSave.Text == "Save")
                {
                    TenderValues = TenderValues + "" + txtPurcNo.Text + "," + Company_Code + ", " +
                        " " + pdsparty + "," + QUANTAL + "," + SALE_RATE + "," + Tender_Commission + ",'" + DOC_DATE + "','" + DOC_DATE + "'," + TenderDetailID + ",''," + txtBroker_CODE.Text + "," +
                        " " + Convert.ToInt32(Session["year"].ToString()) + "," + Convert.ToInt32(Session["Branch_Code"].ToString()) + ",'" + Delivery_Type + "', " +
                        " '" + TenderID + "'," + TenderAutoID + ",case when 0=" + buyerid + " then null else '" + buyerid + "' end , case when 0=" + buyerPartyid + " then null else '" + buyerPartyid + "' end,2, " +
                        " case when 0=" + sub_brokerid + " then null else '" + sub_brokerid + "' end";

                    #region
                    qry = "insert into " + tblPrefix + "Tenderdetails (" + TenderDetail_Fields + ") values (" + TenderValues + ")";

                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = qry;
                    Maindt.Rows.Add(dr);


                    TenderQty = Convert.ToDouble(clsCommon.getString("Select Buyer_Quantal from " + tblPrefix + "Tenderdetails where Tender_No='" + txtPurcNo.Text
                                    + "' and Buyer=2 and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='"
                                    + Convert.ToInt32(Session["year"].ToString()) + "'"));

                    Tender_TotalQty = TenderQty - QUANTAL;

                    qry = string.Empty;
                    qry = "Update " + tblPrefix + "Tenderdetails SET Buyer_Quantal='" + Tender_TotalQty + "' where Tender_No='" + txtPurcNo.Text
                        + "' and Buyer=2 and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code='"
                        + Convert.ToInt32(Session["year"].ToString()) + "'";

                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = qry;
                    Maindt.Rows.Add(dr);
                    #endregion
                }
                else
                {
                    if (purc_order == 1)
                    {
                        TenderValues = TenderValues + "" + txtPurcNo.Text + "," + Company_Code + ", " +
                                                 " " + pdsparty + "," + QUANTAL + "," + SALE_RATE + "," + Tender_Commission + ",'" + DOC_DATE + "','" + DOC_DATE + "'," + TenderDetailID + ",''," + txtBroker_CODE.Text + "," +
                                                 " " + Convert.ToInt32(Session["year"].ToString()) + "," + Convert.ToInt32(Session["Branch_Code"].ToString()) + ",'" + Delivery_Type + "', " +
                                                 " '" + TenderID + "'," + TenderAutoID + ",case when 0=" + buyerid + " then null else '" + buyerid + "' end , case when 0=" + buyerPartyid + " then null else '" + buyerPartyid + "' end,2, " +
                                                 " case when 0=" + buyerPartyid + " then null else '" + buyerPartyid + "' end";

                        #region
                        qry = "insert into " + tblPrefix + "Tenderdetails (" + TenderDetail_Fields + ") values (" + TenderValues + ")";

                        dr = null;
                        dr = Maindt.NewRow();
                        dr["Querys"] = qry;
                        Maindt.Rows.Add(dr);


                        TenderQty = Convert.ToDouble(clsCommon.getString("Select Buyer_Quantal from " + tblPrefix + "Tenderdetails where Tender_No='" + txtPurcNo.Text
                                        + "' and Buyer=2 and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='"
                                        + Convert.ToInt32(Session["year"].ToString()) + "'"));

                        Tender_TotalQty = TenderQty - QUANTAL;

                        qry = string.Empty;
                        qry = "Update " + tblPrefix + "Tenderdetails SET Buyer_Quantal='" + Tender_TotalQty + "' where Tender_No='" + txtPurcNo.Text
                            + "' and Buyer=2 and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code='"
                            + Convert.ToInt32(Session["year"].ToString()) + "'";

                        dr = null;
                        dr = Maindt.NewRow();
                        dr["Querys"] = qry;
                        Maindt.Rows.Add(dr);

                        //qry = "update nt_1_deliveryorder set purc_order=" + TenderDetailID + ",tenderdetailid=" + TenderAutoID + " where doid=" + doid + "";
                        //dr = null;
                        //dr = Maindt.NewRow();
                        //dr["Querys"] = qry;
                        //Maindt.Rows.Add(dr);
                        #endregion
                    }

                }


            }
            else
            {
                TenderAutoID = Convert.ToInt32(clsCommon.getString("SELECT tenderdetailid FROM nt_1_tenderdetails where Tender_No=" + txtPurcNo.Text + " and " +
                     " ID='" + txtPurcOrder.Text + "' and Company_Code='" + Company_Code + "' and year_code='" + Year_Code + "'"));

                TenderDetailID = Convert.ToInt32(txtPurcOrder.Text);

            }
            #endregion
            #region old Tender
            //int ID1 = 0;
            //int tenderDetail = 0;
            //int tenderID = 0;
            //string TenderValues = string.Empty;
            //string buyerid = string.Empty;
            //string buyerPartyid = string.Empty;
            //string buyerQntl = string.Empty;
            //double buyerTotalQntl = 0.00;

            //string TenderDetail_Fields = "Tender_No,";
            //TenderDetail_Fields = TenderDetail_Fields + "Company_Code,";
            //TenderDetail_Fields = TenderDetail_Fields + "Buyer,";
            //TenderDetail_Fields = TenderDetail_Fields + "Buyer_Quantal,";
            //TenderDetail_Fields = TenderDetail_Fields + "Sale_Rate,";
            //TenderDetail_Fields = TenderDetail_Fields + "Commission_Rate,";
            //TenderDetail_Fields = TenderDetail_Fields + "Sauda_Date,";
            //TenderDetail_Fields = TenderDetail_Fields + "Lifting_Date,";
            //TenderDetail_Fields = TenderDetail_Fields + "ID,";

            //TenderDetail_Fields = TenderDetail_Fields + "Narration,";
            //TenderDetail_Fields = TenderDetail_Fields + "Buyer_Party,";
            //TenderDetail_Fields = TenderDetail_Fields + "year_code,";
            //TenderDetail_Fields = TenderDetail_Fields + "Branch_Id,";
            //TenderDetail_Fields = TenderDetail_Fields + "Delivery_Type,";
            //TenderDetail_Fields = TenderDetail_Fields + "tenderid,";
            //TenderDetail_Fields = TenderDetail_Fields + "tenderdetailid,";
            //TenderDetail_Fields = TenderDetail_Fields + "buyerid,";
            //TenderDetail_Fields = TenderDetail_Fields + "buyerpartyid,";
            //TenderDetail_Fields = TenderDetail_Fields + "sub_broker,";
            //TenderDetail_Fields = TenderDetail_Fields + "sbr";
            //if (purc_order == 1)
            //{
            //    string maxtenderid = string.Empty;
            //    if (btnSave.Text == "Update")
            //    {
            //        maxtenderid = clsCommon.getString("SELECT tenderdetailid FROM nt_1_tenderdetails where Tender_No=" + txtPurcNo.Text + " and ID='" + txtPurcOrder.Text + "' and Company_Code='" + Session["Company_Code"].ToString() + "' and year_code='" + Session["year"].ToString() + "'");

            //        buyerQntl = clsCommon.getString("Select Buyer_Quantal from " + tblPrefix + "Tenderdetails where Tender_No='" + txtPurcNo.Text
            //                       + "' and Buyer=2 and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='"
            //                       + Convert.ToInt32(Session["year"].ToString()) + "'");
            //        buyerTotalQntl = Convert.ToDouble(buyerQntl) + Convert.ToDouble(hdnfQty.Value);
            //        buyerTotalQntl = buyerTotalQntl - QUANTAL;

            //    }

            //    string voucheracno = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code="
            //                                                       + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

            //    //string id = clsCommon.getString("select AutoID from " + tblPrefix + "Tenderdetails where Tender_No='" + txtPurcNo.Text + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code=" + Convert.ToInt32(Session["year"].ToString()) + " and ID='" + purc_order + "'");
            //    //if (id != string.Empty)
            //    //{
            //    string Id = clsCommon.getString("select max(ID) from " + tblPrefix + "Tenderdetails where Tender_No='" + txtPurcNo.Text + "' and " +
            //        " Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'");

            //    string tender = clsCommon.getString("select max(tenderdetailid) from " + tblPrefix + "Tenderdetails ");
            //    tenderID = Convert.ToInt32(clsCommon.getString("select ifnull(tenderid,0) from nt_1_tender where Tender_No=" + txtPurcNo.Text + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'"));


            //    ID1 = Convert.ToInt32(Id) + 1;
            //    tenderDetail = Convert.ToInt32(tender) + 1; ;
            //    //}

            //    buyerid = clsCommon.getString("select ifnull(accoid,0) as accoid from qrymstaccountmaster where Ac_code=" + txtBroker_CODE.Text + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
            //    if (buyerid == string.Empty)
            //    {
            //        buyerid = "0";
            //    }
            //    buyerPartyid = clsCommon.getString("select ifnull(accoid,0) as accoid from qrymstaccountmaster where Ac_code=2 and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
            //    if (buyerPartyid == string.Empty)
            //    {
            //        buyerPartyid = "0";
            //    }

            //    if (drpDOType.SelectedValue != "DI")
            //    {
            //        Delivery_Type = "C";
            //    }

            //    if (PDS == "P")
            //    {

            //        if (btnSave.Text == "Save")
            //        {
            //            TenderValues = TenderValues + "" + txtPurcNo.Text + "," + Convert.ToInt32(Session["Company_Code"].ToString()) + ", " +
            //                " " + pdsparty + "," + QUANTAL + "," + SALE_RATE + "," + Tender_Commission + ",'" + DOC_DATE + "','" + DOC_DATE + "'," + ID1 + ",''," + txtBroker_CODE.Text + "," +
            //                " " + Convert.ToInt32(Session["year"].ToString()) + "," + Convert.ToInt32(Session["Branch_Code"].ToString()) + ",'" + Delivery_Type + "', " +
            //                " '" + tenderID + "'," + tenderDetail + ",case when 0=" + buyerid + " then null else '" + buyerid + "' end , case when 0=" + buyerPartyid + " then null else '" + buyerPartyid + "' end,2, " +
            //                " case when 0=" + buyerPartyid + " then null else '" + buyerPartyid + "' end";
            //        }
            //        else
            //        {
            //            qry = "Update " + tblPrefix + "Tenderdetails SET Buyer=" + pdsparty + ", Buyer_Party=" + txtBroker_CODE.Text + ",  Buyer_Quantal='" + QUANTAL + "', Sale_Rate='" + txtSALE_RATE.Text + "', " +
            //    " Commission_Rate='" + Tender_Commission + "',buyerid=" + buyerid + ", buyerpartyid=" + buyerPartyid + ",sub_broker=2,sbr=" + buyerPartyid + " where Tender_No='" + txtPurcNo.Text
            //   + "' and ID='" + txtPurcOrder.Text + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code='"
            //   + Convert.ToInt32(Session["year"].ToString()) + "'";

            //            dr = null;
            //            dr = Maindt.NewRow();
            //            dr["Querys"] = qry;
            //            Maindt.Rows.Add(dr);
            //        }
            //    }
            //    else if (PDS == "C")
            //    {

            //        if (VOUCHER_BY == voucheracno && (SaleBillTo == "0" || SaleBillTo == ""))
            //        {
            //            if (btnSave.Text == "Save")
            //            {
            //                TenderValues = TenderValues + "" + txtPurcNo.Text + "," + Convert.ToInt32(Session["Company_Code"].ToString()) + ", " +
            //               " " + VOUCHER_BY + "," + QUANTAL + "," + SALE_RATE + "," + Tender_Commission + ",'" + DOC_DATE + "','" + DOC_DATE + "'," + ID1 + ",''," + txtBroker_CODE.Text + "," +
            //               " " + Convert.ToInt32(Session["year"].ToString()) + "," + Convert.ToInt32(Session["Branch_Code"].ToString()) + ",'" + Delivery_Type + "', " +
            //               " '" + tenderID + "'," + tenderDetail + ",case when 0=" + buyerid + " then null else '" + buyerid + "' end , case when 0=" + buyerPartyid + " then null else '" + buyerPartyid + "' end,2, " +
            //               " case when 0=" + buyerPartyid + " then null else '" + buyerPartyid + "' end";
            //            }
            //            else
            //            {
            //                qry = "Update " + tblPrefix + "Tenderdetails SET Buyer=" + VOUCHER_BY + ", Buyer_Party=" + txtBroker_CODE.Text + ",  Buyer_Quantal='" + QUANTAL + "', Sale_Rate='" + txtSALE_RATE.Text + "', " +
            //                       " Commission_Rate='" + Tender_Commission + "',buyerid=" + buyerid + ", buyerpartyid=" + buyerPartyid + ",sub_broker=2,sbr=" + buyerPartyid + " where Tender_No='" + txtPurcNo.Text
            //                       + "' and ID='" + txtPurcOrder.Text + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code='"
            //                        + Convert.ToInt32(Session["year"].ToString()) + "'";

            //                dr = null;
            //                dr = Maindt.NewRow();
            //                dr["Querys"] = qry;
            //                Maindt.Rows.Add(dr);
            //            }
            //        }
            //        else
            //        {

            //            if (btnSave.Text == "Save")
            //            {
            //                TenderValues = TenderValues + "" + txtPurcNo.Text + "," + Convert.ToInt32(Session["Company_Code"].ToString()) + ", " +
            //               " " + SaleBillTo + "," + QUANTAL + "," + SALE_RATE + "," + Tender_Commission + ",'" + DOC_DATE + "','" + DOC_DATE + "'," + ID1 + ",''," + txtBroker_CODE.Text + "," +
            //               " " + Convert.ToInt32(Session["year"].ToString()) + "," + Convert.ToInt32(Session["Branch_Code"].ToString()) + ",'" + Delivery_Type + "', " +
            //               " '" + tenderID + "'," + tenderDetail + ",case when 0=" + buyerid + " then null else '" + buyerid + "' end , case when 0=" + buyerPartyid + " then null else '" + buyerPartyid + "' end,2," +
            //               " case when 0=" + buyerPartyid + " then null else '" + buyerPartyid + "' end";
            //            }
            //            else
            //            {
            //                qry = "Update " + tblPrefix + "Tenderdetails SET Buyer=" + SaleBillTo + ", Buyer_Party=" + txtBroker_CODE.Text + ",  Buyer_Quantal='" + QUANTAL + "', Sale_Rate='" + txtSALE_RATE.Text + "', " +
            //                     " Commission_Rate='" + Tender_Commission + "',buyerid=" + buyerid + ", buyerpartyid=" + buyerPartyid + ",sub_broker=2,sbr=" + buyerPartyid + " where Tender_No='" + txtPurcNo.Text
            //                    + "' and ID='" + txtPurcOrder.Text + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code='"
            //                    + Convert.ToInt32(Session["year"].ToString()) + "'";

            //                dr = null;
            //                dr = Maindt.NewRow();
            //                dr["Querys"] = qry;
            //                Maindt.Rows.Add(dr);
            //            }
            //        }
            //    }
            //    else
            //    {

            //        if (VOUCHER_BY == voucheracno && (SaleBillTo == "0" || SaleBillTo == ""))
            //        {
            //            if (btnSave.Text == "Save")
            //            {
            //                TenderValues = TenderValues + "" + txtPurcNo.Text + "," + Convert.ToInt32(Session["Company_Code"].ToString()) + ", " +
            //               " " + VOUCHER_BY + "," + QUANTAL + "," + SALE_RATE + "," + Tender_Commission + ",'" + DOC_DATE + "','" + DOC_DATE + "'," + ID1 + ",''," + txtBroker_CODE.Text + "," +
            //               " " + Convert.ToInt32(Session["year"].ToString()) + "," + Convert.ToInt32(Session["Branch_Code"].ToString()) + ",'" + Delivery_Type + "', " +
            //               " '" + tenderID + "'," + tenderDetail + ",case when 0=" + buyerid + " then null else " + buyerid + " end , case when 0=" + buyerPartyid + " then null else '" + buyerPartyid + "' end,2," +
            //               " case when 0=" + buyerPartyid + " then null else '" + buyerPartyid + "' end";
            //            }
            //            else
            //            {
            //                qry = "Update " + tblPrefix + "Tenderdetails SET Buyer=" + VOUCHER_BY + ", Buyer_Party=" + txtBroker_CODE.Text + ",  Buyer_Quantal='" + QUANTAL + "', Sale_Rate='" + txtSALE_RATE.Text + "', " +
            //                       " Commission_Rate='" + Tender_Commission + "',buyerid=" + buyerid + ", buyerpartyid=" + buyerPartyid + ",sub_broker=2,sbr=" + buyerPartyid + " where Tender_No='" + txtPurcNo.Text
            //                       + "' and ID='" + txtPurcOrder.Text + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code='"
            //                        + Convert.ToInt32(Session["year"].ToString()) + "'";

            //                dr = null;
            //                dr = Maindt.NewRow();
            //                dr["Querys"] = qry;
            //                Maindt.Rows.Add(dr);
            //            }
            //        }
            //        else
            //        {


            //            if (btnSave.Text == "Save")
            //            {
            //                TenderValues = TenderValues + "" + txtPurcNo.Text + "," + Convert.ToInt32(Session["Company_Code"].ToString()) + ", " +
            //               " " + SaleBillTo + "," + QUANTAL + "," + SALE_RATE + "," + Tender_Commission + ",'" + DOC_DATE + "','" + DOC_DATE + "'," + ID1 + ",''," + txtBroker_CODE.Text + "," +
            //               " " + Convert.ToInt32(Session["year"].ToString()) + "," + Convert.ToInt32(Session["Branch_Code"].ToString()) + ",'" + Delivery_Type + "', " +
            //               " '" + tenderID + "'," + tenderDetail + ",case when 0=" + buyerid + " then null else " + buyerid + " end , case when 0=" + buyerPartyid + " then null else '" + buyerPartyid + "' end,2," +
            //               " case when 0=" + buyerPartyid + " then null else '" + buyerPartyid + "' end ";
            //            }
            //            else
            //            {
            //                qry = "Update " + tblPrefix + "Tenderdetails SET Buyer=" + SaleBillTo + ", Buyer_Party=" + txtBroker_CODE.Text + ",  Buyer_Quantal='" + QUANTAL + "', Sale_Rate='" + txtSALE_RATE.Text + "', " +
            //                       " Commission_Rate='" + Tender_Commission + "',buyerid=" + buyerid + ", buyerpartyid=" + buyerPartyid + ",sub_broker=2,sbr=" + buyerPartyid + " " +
            //                       " where Tender_No='" + txtPurcNo.Text
            //                       + "' and ID='" + txtPurcOrder.Text + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code='"
            //                        + Convert.ToInt32(Session["year"].ToString()) + "'";

            //                dr = null;
            //                dr = Maindt.NewRow();
            //                dr["Querys"] = qry;
            //                Maindt.Rows.Add(dr);
            //            }
            //        }
            //    }

            //    // Tender Insert Qry
            //    if (btnSave.Text == "Save")
            //    {
            //        qry = "insert into " + tblPrefix + "Tenderdetails (" + TenderDetail_Fields + ") values (" + TenderValues + ")";

            //        dr = null;
            //        dr = Maindt.NewRow();
            //        dr["Querys"] = qry;
            //        Maindt.Rows.Add(dr);

            //        // end

            //        buyerQntl = clsCommon.getString("Select Buyer_Quantal from " + tblPrefix + "Tenderdetails where Tender_No='" + txtPurcNo.Text
            //                        + "' and Buyer=2 and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='"
            //                        + Convert.ToInt32(Session["year"].ToString()) + "'");

            //        buyerTotalQntl = Convert.ToDouble(buyerQntl) - QUANTAL;

            //        qry = string.Empty;
            //        qry = "Update " + tblPrefix + "Tenderdetails SET Buyer_Quantal='" + buyerTotalQntl + "' where Tender_No='" + txtPurcNo.Text
            //            + "' and Buyer=2 and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code='"
            //            + Convert.ToInt32(Session["year"].ToString()) + "'";

            //        dr = null;
            //        dr = Maindt.NewRow();
            //        dr["Querys"] = qry;
            //        Maindt.Rows.Add(dr);

            //        qry = "update nt_1_deliveryorder set purc_order=" + ID1 + ",tenderdetailid=" + tenderDetail + " where doid=" + doid + "";
            //        dr = null;
            //        dr = Maindt.NewRow();
            //        dr["Querys"] = qry;
            //        Maindt.Rows.Add(dr);
            //    }
            //    else
            //    {
            //        qry = string.Empty;
            //        qry = "update nt_1_deliveryorder set purc_order=" + txtPurcOrder.Text + ",tenderdetailid=" + maxtenderid + " where doid=" + doid + "";
            //        dr = null;
            //        dr = Maindt.NewRow();
            //        dr["Querys"] = qry;
            //        Maindt.Rows.Add(dr);

            //        qry = string.Empty;
            //        qry = "Update " + tblPrefix + "Tenderdetails SET Buyer_Quantal='" + buyerTotalQntl + "' where Tender_No='" + txtPurcNo.Text
            //            + "' and Buyer=2 and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code='"
            //            + Convert.ToInt32(Session["year"].ToString()) + "'";

            //        dr = null;
            //        dr = Maindt.NewRow();
            //        dr["Querys"] = qry;
            //        Maindt.Rows.Add(dr);
            //    }
            //    //}
            //    //else
            //    //{

            //    //}

            //}
            //else
            //{
            //    string maxtenderid = clsCommon.getString("SELECT tenderdetailid FROM nt_1_tenderdetails where Tender_No=" + txtPurcNo.Text + " and " +
            //         " ID='" + txtPurcOrder.Text + "' and Company_Code='" + Session["Company_Code"].ToString() + "' and year_code='" + Session["year"].ToString() + "'");

            //    qry = "Update " + tblPrefix + "deliveryorder SET tenderdetailid='" + maxtenderid + "' where doid='" + doid + "'";

            //    dr = null;
            //    dr = Maindt.NewRow();
            //    dr["Querys"] = qry;
            //    Maindt.Rows.Add(dr);
            //}
            #endregion


            //if (hdnfgeneratesalebill.Value == "Yes")
            //{
            #region [SaleBill genearate]
            if (PDS == "P")
            {
                if (drpDOType.SelectedValue != "DO")
                {
                    if (Convert.ToInt32(txtSaleBillTo.Text) != 0 && txtSaleBillTo.Text != SELFAC.ToString() && txtSaleBillTo.Text != "2")
                    {
                        if (lblSB_No.Text != string.Empty && Convert.ToInt32(lblSB_No.Text) != 0)
                        {
                            string Type = "SB";
                            SalePurcdt = new DataTable();
                            SalePurcdt = clsSale_Posting.Sale_Posting(flag, salePosting, Type, dt2);
                            Maindt.Merge(SalePurcdt);
                        }
                        else
                        {
                            string Type = "SB";
                            SalePurcdt = new DataTable();
                            SalePurcdt = clsSale_Posting.Sale_Posting(1, salePosting, Type, dt2);
                            Maindt.Merge(SalePurcdt);
                        }
                    }

                }
            }
            else
            {
                if (Convert.ToInt32(txtSaleBillTo.Text) != 0 && txtSaleBillTo.Text != SELFAC.ToString() && txtSaleBillTo.Text != "2")
                {
                    if (AUTO_VOUCHER == "YES")
                    {
                        if (drpDOType.SelectedValue != "DO")
                        {
                            int sbno = Convert.ToInt32(lblSB_No.Text != string.Empty ? lblSB_No.Text : "0");
                            if (lblSB_No.Text == string.Empty && sbno == 0)
                            {
                                string Type = "SB";
                                SalePurcdt = new DataTable();
                                SalePurcdt = clsSale_Posting.Sale_Posting(1, salePosting, Type, dt2);
                                Maindt.Merge(SalePurcdt);

                            }
                            else
                            {
                                string Type = "SB";
                                SalePurcdt = new DataTable();
                                SalePurcdt = clsSale_Posting.Sale_Posting(flag, salePosting, Type, dt2);
                                Maindt.Merge(SalePurcdt);
                            }
                        }
                        else
                        {
                            salePosting = new SaleFields();
                            salePosting.SB_doc_no = 0;
                        }
                    }
                    else
                    {
                        salePosting.SB_doc_no = 0;
                        salePosting.SB_Sale_Id = 0;
                    }

                }
                else
                {
                    salePosting = new SaleFields();
                    salePosting.SB_doc_no = 0;
                }
            }
            #endregion


            string NO = string.Empty;
            string Types = string.Empty;
            #region Purchase
            //if (lblVoucherNo.Text == string.Empty || lblVoucherNo.Text == "0")
            //{
            if (AUTO_VOUCHER == "YES")
            {
                int newFlag = 0;
                if (lblVoucherNo.Text == string.Empty || lblVoucherNo.Text == "0")
                {
                    newFlag = 1;
                }
                else
                {
                    newFlag = flag;
                }
                if (drpDOType.SelectedValue != "DO")
                {
                    SalePurcdt = new DataTable();
                    SalePurcdt = clsPurchase_Posting.Purchase_Posting(newFlag, purchase, "PS", dt1);
                    Maindt.Merge(SalePurcdt);
                    NO = Convert.ToString(purchase.PS_doc_no);
                    Types = "PS";
                }
                else
                {
                    if (DIFF_AMOUNT != 0)
                    {
                        SalePurcdt = new DataTable();
                        SalePurcdt = clsLocalVoucher.LV_Posting(newFlag, LV, "LV");
                        Maindt.Merge(SalePurcdt);
                        NO = Convert.ToString(LV.LV_Doc_No);
                        Types = "LV";
                    }
                    else
                    {
                        NO = "0";
                    }
                }
            }
            else
            {
                NO = "0";
            }

            #endregion


            if (hdnfgeneratesalebill.Value == "No")
            {

                salePosting.SB_doc_no = 0;
            }
            qry = "update nt_1_deliveryorder set SB_No=case when 0='" + salePosting.SB_doc_no + "' then null else '" + salePosting.SB_doc_no + "' end, " +
                " saleid=case when 0='" + salePosting.SB_Sale_Id + "' then null else '" + salePosting.SB_Sale_Id + "' end, voucher_no='" + NO + "',voucher_type='" + Types + "',memo_no='" + memo_no + "',purc_order=" + TenderDetailID + ",tenderdetailid=" + TenderAutoID + " where doid=" + doid + "";
            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = qry;
            Maindt.Rows.Add(dr);

            msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);
            timeset = DOPurcSaleCRUD.timervalue;
            timeset += storeproceduertimmer;

            string elapsedTime = "";

            if (msg == "Insert")
            {
                hdnf.Value = doid.ToString();
                clsButtonNavigation.enableDisable("S");
                this.makeEmptyForm("S");
                qry = getDisplayQuery();
                this.fetchRecord(qry);
                alltime.Stop();
                TimeSpan ts = alltime.Elapsed;

                // Format and display the TimeSpan value.
                elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                   ts.Hours, ts.Minutes, ts.Seconds,
                   ts.Milliseconds / 10);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Added !')", true);
            }
            else if (msg == "Update")
            {
                hdnf.Value = lbldoid.Text;
                clsButtonNavigation.enableDisable("S");
                this.makeEmptyForm("S");
                qry = getDisplayQuery();
                this.fetchRecord(qry);
                alltime.Stop();
                TimeSpan ts = alltime.Elapsed;

                // Format and display the TimeSpan value.
                elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                   ts.Hours, ts.Minutes, ts.Seconds,
                   ts.Milliseconds / 10);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update !')", true);
            }
            else
            {
                btnSave.Enabled = true;
                setFocusControl(btnSave);
            }
            // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showprint", "javascript:SB();", true);
            txtEditDoc_No.Text = string.Empty;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "showsms", "javascript:showsmspopup();", true);

        }
        catch (Exception exxx)
        {
            DOPurcSaleCRUD.LogError(exxx);
            btnSave.Enabled = true;
            setFocusControl(btnSave);
        }


    }

    #endregion

    public string AcBalance(string Ac_Code)
    {
        try
        {
            string todayDate = DateTime.Now.ToString("dd/MM/yyyy");
            string DOC_DTAE = DateTime.Parse(todayDate, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            qry = qry = "select AC_CODE,SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) as Balance " +
                            " from qryGledgernew where AC_CODE='" + Ac_Code + "' and DOC_DATE<='" + DOC_DTAE + "' and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  group by AC_CODE having SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) !=0";
            string Balance = "0";
            DataTable dtT = new DataTable();
            dtT.Columns.Add("accode", typeof(Int32));
            dtT.Columns.Add("acname", typeof(string));
            dtT.Columns.Add("city", typeof(string));
            dtT.Columns.Add("debitAmt", typeof(double));
            dtT.Columns.Add("creditAmt", typeof(double));
            dtT.Columns.Add("mobile", typeof(string));
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataView dv;
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        dv = new DataView();

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            DataRow dr = dtT.NewRow();
                            //dr["accode"] = dt.Rows[i]["AC_CODE"].ToString();
                            //dr["acname"] = dt.Rows[i]["Ac_Name_E"].ToString();
                            //dr["city"] = dt.Rows[i]["CityName"].ToString();
                            //dr["mobile"] = dt.Rows[i]["Mobile_No"].ToString();
                            double bal = Convert.ToDouble(ds.Tables[0].Rows[i]["Balance"].ToString());
                            //if (DrCr == "Dr")
                            //{
                            //    if (bal > 0)
                            //    {
                            //        dr["debitAmt"] = bal.ToString();
                            //        dr["creditAmt"] = 0.00;
                            //        dtT.Rows.Add(dr);
                            //    }
                            //}
                            //else if (DrCr == "Cr")
                            //{
                            //    if (bal < 0)
                            //    {
                            //        dr["debitAmt"] = 0.00;
                            //        dr["creditAmt"] = Math.Abs(bal);
                            //        dtT.Rows.Add(dr);
                            //    }
                            //}
                            //else
                            //{
                            if (bal > 0)
                            {
                                // groupdebitamt += bal;
                                dr["debitAmt"] = bal.ToString();
                                dr["creditAmt"] = 0.00;
                                Balance = Math.Abs(bal).ToString() + " Debit";
                            }
                            else
                            {
                                //  groupcreditamt += -bal;
                                dr["debitAmt"] = 0.00;
                                dr["creditAmt"] = Math.Abs(bal);
                                Balance = Math.Abs(bal).ToString() + " Credit";
                            }
                            dtT.Rows.Add(dr);
                            //}
                        }
                    }
                }
            }
            return Balance;
        }
        catch (Exception)
        {
            return "0";
        }
    }

    private void MaxVoucher()
    {
        //string selfac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
        if (AUTO_VOUCHER == "YES")
        {
            if (drpDOType.SelectedValue == "DO")
            {
                int voucherno = Convert.ToInt32(clsCommon.getString("Select COALESCE(ifnull(MAX(Doc_No),0))+1 from commission_bill Where Tran_Type='LV' and Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                hdnvouchernumber.Value = voucherno.ToString();
                lblVoucherType.Text = "LV";
            }
            if (drpDOType.SelectedValue == "DI")
            {
                if (txtGETPASS_CODE.Text == Session["SELF_AC"].ToString())
                {
                    int voucherno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(doc_no),0)+1 from " + tblPrefix + "SugarPurchase where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                    hdnvouchernumber.Value = voucherno.ToString();
                    lblVoucherType.Text = "PS";
                }
                //else
                //{
                //    int voucherno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(Doc_No),0)+1 from " + tblPrefix + "Voucher Where Tran_Type='LV' and Company_Code="
                //    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                //    hdnvouchernumber.Value = voucherno.ToString();
                //    lblVoucherType.Text = "LV";

                //    //int voucherno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(Doc_No),0)+1 from " + tblPrefix + "Voucher Where Tran_Type='OV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                //    //hdnvouchernumber.Value = voucherno.ToString();
                //    //lblVoucherType.Text = "OV";
                //}
            }
        }
        else
        {
            hdnvouchernumber.Value = string.Empty;
        }
    }

    protected void txtmillRate_TextChanged(object sender, EventArgs e)
    {
        // searchString = txtmillRate.Text;
        strTextBox = "txtmillRate";
        csCalculations();
        calculation();
        GSTCalculations();
        setFocusControl(txtSALE_RATE);
    }

    protected void txtPurcNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtPurcNo.Text;
            strTextBox = "txtPurcNo";
            txtPurcOrder.Enabled = false;
            txtPurcNo.Enabled = false;
            if (strTextBox == "txtPurcNo")
            {
                //txtPurcOrder.Text = "1";
                setFocusControl(txtGstRate);
                int i = 0;
                i++;
                if (txtcarporateSale.Text == string.Empty || txtcarporateSale.Text == "0")
                {
                    hdnfpacking.Value = Convert.ToString(i);
                }
                string a = txtPurcOrder.Text;
                if (txtPurcNo.Text != string.Empty && txtPurcOrder.Text != string.Empty)
                {
                    string qry = "select Buyer,buyername,Buyer_Party,buyerpartyname,Voucher_By,voucherbyname,Grade,Quantal,Packing,Bags," +
                        " Excise_Rate,Mill_Rate,Sale_Rate,Tender_DO,tenderdoname,Broker,brokername,Commission_Rate as CR,Delivery_Type as DT,Payment_To,paymenttoname, " +
                       " gstratecode,gstratename,itemcode,itemname from  qrytenderheaddetail" +
                        "  where Tender_No=" + txtPurcNo.Text + " and ID=" + txtPurcOrder.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                        + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                if (txtcarporateSale.Text == string.Empty || txtcarporateSale.Text == "0")
                                {
                                    string Buyer = dt.Rows[0]["Buyer"].ToString();
                                    string broker = dt.Rows[0]["Buyer_Party"].ToString();

                                    if (drpDOType.SelectedValue == "DO")
                                    {
                                        txtGETPASS_CODE.Text = dt.Rows[0]["Buyer"].ToString();
                                        LBLGETPASS_NAME.Text = dt.Rows[0]["buyername"].ToString();
                                    }

                                    txtGstRate.Text = dt.Rows[0]["gstratecode"].ToString();
                                    lblGstRateName.Text = dt.Rows[0]["gstratename"].ToString();
                                    txtitem_Code.Text = dt.Rows[0]["itemcode"].ToString();
                                    lblitem_Name.Text = dt.Rows[0]["itemname"].ToString();
                                    txtvoucher_by.Text = dt.Rows[0]["Buyer"].ToString();
                                    lblvoucherbyname.Text = dt.Rows[0]["buyername"].ToString();
                                    txtBroker_CODE.Text = broker;
                                    LBLBROKER_NAME.Text = dt.Rows[0]["buyerpartyname"].ToString();
                                    hdnfic.Value = clsCommon.getString("select systemid from qrymstitem where System_Code=" + txtitem_Code.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " ");
                                    //if (Buyer != broker)
                                    //{
                                    //    if (broker != "0")
                                    //    {
                                    //        txtBroker_CODE.Text = dt.Rows[0]["Buyer_Party"].ToString();
                                    //        LBLBROKER_NAME.Text = dt.Rows[0]["salepartyfullname"].ToString();
                                    //    }
                                    //    else
                                    //    {
                                    //        txtBroker_CODE.Text = "2";
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    txtBroker_CODE.Text = "0";
                                    //}

                                    if (drpDOType.SelectedValue == "DI")
                                    {
                                        txtGETPASS_CODE.Text = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code='" + Session["year"].ToString() + "'");

                                        string selfac_name = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtGETPASS_CODE.Text
                                            + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                                        string selftacc_city = clsCommon.getString("select CityName from " + qryAccountList + "  where Ac_Code=" + txtGETPASS_CODE.Text
                                            + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                                        LBLGETPASS_NAME.Text = selfac_name + "," + selftacc_city;

                                        if (txtvoucher_by.Text == txtGETPASS_CODE.Text)
                                        {
                                            txtNARRATION4.Text = "";
                                            txtSaleBillTo.Text = "";
                                            txtSalebilltoGstStateCode.Text = "";
                                            lbltxtSalebilltoGstStateName.Text = "";
                                        }
                                        if (txtvoucher_by.Text != txtGETPASS_CODE.Text)
                                        {
                                            txtNARRATION4.Text = lblvoucherbyname.Text;
                                            txtSaleBillTo.Text = txtvoucher_by.Text;
                                            txtSalebilltoGstStateCode.Text = txtVoucherbyGstStateCode.Text;
                                            lbltxtSalebilltoGstStateName.Text = lbltxtVoucherbyGstStateName.Text;
                                        }

                                    }

                                }
                                //txtSaleBillTo.Text = dt.Rows[0]["Buyer"].ToString();

                                // txtNARRATION4.Text = dt.Rows[0]["buyerbrokerfullname"].ToString();

                                // txtSalebilltoGstStateCode.Text = txtVoucherbyGstStateCode.Text;
                                // lbltxtSalebilltoGstStateName.Text = lbltxtVoucherbyGstStateName.Text;

                                txtGRADE.Text = dt.Rows[0]["Grade"].ToString();
                                txtPACKING.Text = dt.Rows[0]["Packing"].ToString();
                                txtBAGS.Text = dt.Rows[0]["Bags"].ToString();
                                txtexcise_rate.Text = dt.Rows[0]["Excise_Rate"].ToString();
                                txtmillRate.Text = dt.Rows[0]["Mill_Rate"].ToString();
                                double Comm_rate = Convert.ToDouble(dt.Rows[0]["CR"].ToString());
                                //txtPartyCommission.Text = Convert.ToString(Comm_rate);
                                double SR = Convert.ToDouble(dt.Rows[0]["Sale_Rate"].ToString());
                                hdnfSaleRate.Value = Convert.ToString(SR);
                                string DT = dt.Rows[0]["DT"].ToString();
                                if (txtcarporateSale.Text == string.Empty || txtcarporateSale.Text == "0")
                                {
                                    drpDeliveryType.SelectedValue = "N";
                                }
                                if (txtcarporateSale.Text == string.Empty || txtcarporateSale.Text == "0")
                                {
                                    txtSALE_RATE.Text = (SR).ToString();
                                    txtCommission.Text = Comm_rate.ToString();
                                }
                                txtDO_CODE.Text = dt.Rows[0]["Tender_DO"].ToString();
                                LBLDO_NAME.Text = dt.Rows[0]["tenderdoname"].ToString();
                                txtPurcNo.Enabled = false;
                                string distance = clsCommon.getString("Select Distance from " + qryAccountList + " where Ac_Code=" + txtvoucher_by.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                                txtDistance.Text = distance;

                                trnType = clsCommon.getString("select Delivery_Type from nt_1_tenderdetails where Tender_No=" + txtPurcNo.Text + " " +
                                    " and ID=" + txtPurcOrder.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + "");
                                drpDeliveryType.SelectedValue = trnType;
                                if (trnType == "D")
                                {
                                    drpDOType.SelectedValue = "DO";

                                }
                                else
                                {
                                    drpDOType.SelectedValue = "DI";
                                }
                            }

                            #region Assign
                            if (txtMILL_CODE.Text != string.Empty)
                            {
                                hdnfmc.Value = clsCommon.getString("select ifnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + MILL_CODE + " and Company_code='" + Company_Code + "'");
                                // tenderdetailid = Convert.ToInt32(clsCommon.getString("select tenderdetailid from qrytenderdobalanceview where Mill_Code=" + MILL_CODE + " and Company_code='" + Company_Code + "' and Year_Code='" + Year_Code + "'"));
                            }


                            if (txtGETPASS_CODE.Text != string.Empty)
                            {
                                hdnfgp.Value = clsCommon.getString("select  ifnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + GETPASS_CODE + " and Company_code='" + Company_Code + "'");
                            }

                            if (txtvoucher_by.Text != string.Empty)
                            {
                                hdnfst.Value = clsCommon.getString("select  ifnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + VOUCHER_BY + " and Company_code='" + Company_Code + "'");
                            }

                            if (txtSaleBillTo.Text != string.Empty)
                            {
                                try
                                {
                                    hdnfsb.Value = clsCommon.getString("select  ifnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + SaleBillTo + " and Company_code='" + Company_Code + "'");
                                }
                                catch { }
                            }

                            if (txtTRANSPORT_CODE.Text != string.Empty)
                            {
                                hdnftc.Value = clsCommon.getString("select  ifnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + TRANSPORT_CODE + " and Company_code='" + Company_Code + "'");
                            }


                            if (txtBroker_CODE.Text != string.Empty)
                            {
                                hdnfbk.Value = clsCommon.getString("select  ifnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + BROKER_CODE + " and Company_code='" + Company_Code + "'");
                            }


                            Bill_To = Convert.ToInt32(txtBill_To.Text != string.Empty ? txtBill_To.Text : "0");
                            if (txtBill_To.Text != string.Empty && txtBill_To.Text != "0")
                            {
                                hdnfbt.Value = clsCommon.getString("select  ifnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + Bill_To + " and Company_code='" + Company_Code + "'");
                            }
                            else
                            {
                                bt = 0;
                            }
                            if (txtDO_CODE.Text != string.Empty && txtDO_CODE.Text != "0")
                            {
                                hdnfdocd.Value = clsCommon.getString("select  ifnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + txtDO_CODE.Text + " and Company_code='" + Company_Code + "'");
                            }
                            #endregion
                            //if (txtPurcOrder.Text.Trim() == "1")
                            //{
                            //    drpDeliveryType.SelectedValue = "N";
                            //}
                        }
                    }
                }
            }
            calculation();
            if (txtGETPASS_CODE.Text.Trim() != string.Empty)
            {
                string gststatecode = clsCommon.getString("select GSTStateCode from " + qryAccountList + " where Ac_Code=" + txtGETPASS_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string stateName = "";
                if (gststatecode.Trim() != string.Empty)
                {
                    stateName = clsCommon.getString("select State_Name from GSTStateMaster where State_Code=" + gststatecode + "");
                }
                txtGetpassGstStateCode.Text = gststatecode;
                lbltxtGetpassGstStateName.Text = stateName;
            }
            if (txtvoucher_by.Text.Trim() != string.Empty)
            {
                string gststatecode = clsCommon.getString("select GSTStateCode from " + qryAccountList + " where Ac_Code=" + txtvoucher_by.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                string stateName = "";
                if (gststatecode.Trim() != string.Empty)
                {
                    stateName = clsCommon.getString("select State_Name from GSTStateMaster where State_Code=" + gststatecode + "");
                }
                txtVoucherbyGstStateCode.Text = gststatecode;
                lbltxtVoucherbyGstStateName.Text = stateName;
            }
            setFocusControl(txtGstRate);
        }
        catch
        {
        }
        //setFocusControl(txtvoucher_by);

    }

    protected void btntxtPurcNo_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPurcNo";
            pnlPopup.ScrollBars = ScrollBars.Both;
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    }

    protected void btntxtcarporateSale_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtcarporateSale";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void btntxtUTRNo_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtUTRNo";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void txtcarporateSale_TextChanged(object sender, EventArgs e)
    {
        searchString = txtcarporateSale.Text;
        strTextBox = "txtcarporateSale";
        hdnfpacking.Value = "2";
        csCalculations();
    }

    protected void txtUTRNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtUTRNo.Text;
            strTextBox = "txtUTRNo";
            double Utr_Balance = hdnfUtrBalance.Value.TrimStart().ToString() != string.Empty ? Convert.ToDouble(hdnfUtrBalance.Value.TrimStart()) : 0.0;
            double Bank_Amount = hdnfMainBankAmount.Value.TrimStart().ToString() != string.Empty ? Convert.ToDouble(hdnfMainBankAmount.Value.TrimStart()) : 0.0;
            bool isValidated = true;
            if (txtUTRNo.Text != string.Empty && txtUTRNo.Text.Trim() != "0")
            {
                string qry = "";
                qry = "select Year_Code from qryutrdobalanceforfinalview where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and doc_no=" + txtUTRNo.Text;
                string s = clsCommon.getString(qry);

                if (s != string.Empty)
                {
                    isValidated = true;
                    lblUTRYearCode.Text = s.ToString();
                }
                else
                {
                    isValidated = false;
                    txtUTRNo.Text = "";
                    lblUTRYearCode.Text = "";
                    setFocusControl(txtUTRNo);
                    return;
                }

                if (Bank_Amount > Utr_Balance)
                {
                    txtBANK_AMOUNT.Text = Utr_Balance.ToString();
                    lblUtrBalnceError.Text = "Mill Amount Is Greater Than Utr Balance.Remaining UTR Balance Reflect to Amount.Please Select Another UTR";
                    ViewState["ankush"] = "A";
                }
                else
                {
                    double millamount = Convert.ToDouble(txtBANK_AMOUNT.Text != string.Empty ? txtBANK_AMOUNT.Text : "0.00");
                    if (millamount < Bank_Amount)
                    {
                        txtBANK_AMOUNT.Text = millamount.ToString();
                    }
                    else
                    {
                        txtBANK_AMOUNT.Text = Bank_Amount.ToString();
                    }
                    lblUtrBalnceError.Text = "";
                }

            }
            if (strTextBox == "txtUTRNo")
            {
                setFocusControl(txtNARRATION);
            }
        }
        catch (Exception)
        {
            throw;
        }
        //csCalculations();
    }

    protected void txtMillEmailID_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMillEmailID.Text;
        strTextBox = "txtMillEmailID";
        csCalculations();
    }

    protected void txtSaleBillTo_TextChanged(object sender, EventArgs e)
    {

        searchString = txtSaleBillTo.Text;
        strTextBox = "txtSaleBillTo";
        csCalculations();
    }

    protected void txtwithGst_Amount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtwithGst_Amount.Text;
        strTextBox = "txtwithGst_Amount";
        csCalculations();
    }

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string searchtxt = searchString;
            string delimStr = "";
            char[] delimiter = delimStr.ToCharArray();
            string words = searchString;
            string[] split = null;
            string name = string.Empty;
            if (searchString != string.Empty)
            {
                txtSearchText.Text = searchString;
            }
            else
            {
                txtSearchText.Text = txtSearchText.Text;
            }
            if (hdnfClosePopup.Value == "txtdoc_no" || hdnfClosePopup.Value == "txtEditDoc_No")
            {
                if (btntxtDOC_NO.Text == "Change No")
                {
                    pnlPopup.Style["display"] = "none";
                    txtdoc_no.Text = string.Empty;
                    txtdoc_no.Enabled = true;
                    btnSave.Enabled = false;
                    setFocusControl(txtdoc_no);
                    hdnfClosePopup.Value = "Close";
                }
                if (btntxtDOC_NO.Text == "Choose No")
                {
                    lblPopupHead.Text = "--Select DO--";
                    tdDate.Visible = true;
                    string fromdt = DateTime.Parse(txtFromDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
                    string todt = DateTime.Parse(txtToDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

                    //string qry = "select distinct(doc_no) as No,ISNULL(LEFT(millName,15),millShortName) as Mill,VoucherByname As Voucher_By," +
                    //    "GetPassName as Getpass,getpasscity,quantal as Qntl,mill_rate as [M.R],sale_rate as [S.R],convert(varchar(10),doc_date,103) as Date" +
                    //    " ,voucher_no,truck_no,FreightPerQtl as Frieght,vasuli_rate1 as Vasuli,TransportName,memo_no from "
                    //    + qryCommon + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    //    " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'" +
                    //    " and doc_date between '" + fromdt + "' and '" + todt + "' and (doc_no like '%" + txtSearchText.Text + "%' or truck_no like '%"
                    //    + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%' or millName like '%" + txtSearchText.Text
                    //    + "%' or truck_no like '%" + txtSearchText.Text + "%' or VoucherByname like '%" + txtSearchText.Text + "%') order by doc_no desc";

                    string qry = "select  doc_no as No,convert(varchar(10),doc_date,103) as Date,ISNULL(LEFT(millName,15),millShortName) as Mill,GetPassName as Getpass," +
                        "VoucherByname As Voucher_By,voucherby_cityname,ISNULL(LEFT(narration4,15),millShortName) as SalebillTo,getpasscity,quantal as Qntl,mill_rate as [M.R]," +
                        "sale_rate as [S.R] ,SB_No as SaleBill_no,voucher_no as Purchase_No,truck_no, " +
                        "FreightPerQtl as Frieght,vasuli_rate1 as Vasuli,ISNULL(LEFT(TransportName,15),millShortName)AS TransportShortName,memo_no from NT_1_qryDeliveryOrderList" +
                        " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code="
                        + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='DO' and doc_date between '" + fromdt + "' and '" + todt + "'" +
                        "and(doc_no like '%" + txtSearchText.Text + "%' or truck_no like '%"
                        + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%' or millName like '%" + txtSearchText.Text
                        + "%' or truck_no like '%" + txtSearchText.Text + "%' or VoucherByname like '%" + txtSearchText.Text + "%' or voucherby_cityname like '%" + txtSearchText.Text + "%') order by doc_no desc";

                    this.showPopup(qry);
                }
            }
            if (hdnfClosePopup.Value == "txtMILL_CODE")
            {
                //txtSearchText.Text = searchString;
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Mill--";
                string qry = "select Ac_Code,Ac_Name_E as Name,cityname as City from qrymstaccountmaster where Company_Code=" + Session["Company_Code"].ToString() + " " +
                   "  and Ac_type='M' and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or cityname like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                this.showPopup(qry);

                //string qry = "select " + qryAccountList + ".Ac_Code," + qryAccountList + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + qryAccountList +
                //    " left outer join " + cityMasterTable + " on " + qryAccountList + ".City_Code=" + cityMasterTable + ".city_code and " + qryAccountList + ".Company_Code=" + cityMasterTable + ".company_code where " + qryAccountList + ".Company_Code="
                //    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + qryAccountList + ".Ac_type='M' " +
                //    " and (" + qryAccountList + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + qryAccountList + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + qryAccountList + ".Ac_Name_E";
                //this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtBill_To")
            {
                lblPopupHead.Text = "--Select Party--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from qrymstaccountmaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%' )";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtGstRate")
            {
                //txtSearchText.Text = searchString;
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Mill--";
                string qry = "select Doc_no,GST_Name,Rate,IGST,SGST,CGST from nt_1_gstratemaster where " +
                    "  (Doc_No like '%" + txtSearchText.Text + "%' or GST_Name like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtGETPASS_CODE")
            {
                //txtSearchText.Text = txtGETPASS_CODE.Text;
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select GetpassCode--";
                string qry = "select Ac_Code,Ac_Name_E as Name,cityname as City from qrymstaccountmaster where Company_Code=" + Session["Company_Code"].ToString() + " " +
                   "  and Ac_type!='C'   and Ac_type!='B' and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or cityname like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                this.showPopup(qry);

                //string qry = "select " + qryAccountList + ".Ac_Code," + qryAccountList + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + qryAccountList +
                //    " left outer join " + cityMasterTable + " on " + qryAccountList + ".City_Code=" + cityMasterTable + ".city_code and " + qryAccountList + ".Company_Code=" + cityMasterTable + ".company_code where " +
                //    qryAccountList + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + qryAccountList + ".Ac_type!='C' and " +
                //    qryAccountList + ".Ac_type!='B'" +
                //     " and (" + qryAccountList + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + qryAccountList + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + qryAccountList + ".Ac_Name_E";
                //this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtGetpassGstStateCode" || hdnfClosePopup.Value == "txtVoucherbyGstStateCode" || hdnfClosePopup.Value == "txtSalebilltoGstStateCode" || hdnfClosePopup.Value == "txtMillGstStateCode" || hdnfClosePopup.Value == "txtTransportGstStateCode")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Getpass State Code--";
                string qry = "select State_Code,State_Name from GSTStateMaster where (State_Code like '%" + txtSearchText.Text + "%' or State_Name LIKE '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtvoucher_by")
            {

                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Voucher--";
                string qry = "select Ac_Code,Ac_Name_E as Name,cityname as City from qrymstaccountmaster where Company_Code=" + Session["Company_Code"].ToString() + " " +
                    "  and Ac_type!='C'   and Ac_type!='B' and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or cityname like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtVasuliAc")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select VasuliAc--";
                string qry = "select Ac_Code,Ac_Name_E as Name,cityname as City from qrymstaccountmaster where Company_Code=" + Session["Company_Code"].ToString() + " " +
                   "  and Ac_type!='C'   and Ac_type!='B' and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or cityname like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                this.showPopup(qry);
                //string qry = "select " + qryAccountList + ".Ac_Code," + qryAccountList + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + qryAccountList +
                //    " left outer join " + cityMasterTable + " on " + qryAccountList + ".City_Code=" + cityMasterTable + ".city_code and " + qryAccountList + ".Company_Code=" + cityMasterTable + ".company_code where " +
                //    qryAccountList + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + qryAccountList + ".Ac_type!='C' and " + qryAccountList + ".Ac_type!='B' " +
                //     " and (" + qryAccountList + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + qryAccountList + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + qryAccountList + ".Ac_Name_E";
                //this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtGRADE")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Grade--";
                string qry = "select  System_Name_E from " + tblPrefix + "SystemMaster where System_Type='S' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and System_Name_E like '%" + txtSearchText.Text + "%' ";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtDO_CODE")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Do--";
                string qry = "select Ac_Code,Ac_Name_E as Name,cityname as City from qrymstaccountmaster where Company_Code=" + Session["Company_Code"].ToString() + " " +
                   "  and Ac_type!='C'   and Ac_type!='B' and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or cityname like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                this.showPopup(qry);

                //string qry = "select " + qryAccountList + ".Ac_Code," + qryAccountList + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + qryAccountList +
                //    " left outer join " + cityMasterTable + " on " + qryAccountList + ".City_Code=" + cityMasterTable + ".city_code and " + qryAccountList + ".Company_Code=" + cityMasterTable + ".company_code where " +
                //    qryAccountList + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + qryAccountList + ".Ac_type!='C' and " + qryAccountList + ".Ac_type!='B' " +
                //     " and (" + qryAccountList + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + qryAccountList + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + qryAccountList + ".Ac_Name_E";
                //this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtBroker_CODE")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Broker--";

                string qry = "select Ac_Code,Ac_Name_E as Name,cityname as City from qrymstaccountmaster where Company_Code=" + Session["Company_Code"].ToString() + " " +
                   "  and Ac_type!='C'   and Ac_type!='B' and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or cityname like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                this.showPopup(qry);
                //string qry = "select " + qryAccountList + ".Ac_Code," + qryAccountList + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + qryAccountList +
                //    " left outer join " + cityMasterTable + " on " + qryAccountList + ".City_Code=" + cityMasterTable + ".city_code and " + qryAccountList + ".Company_Code=" + cityMasterTable + ".company_code where " +
                //    qryAccountList + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + qryAccountList + ".Ac_type!='C' and " + qryAccountList + ".Ac_type!='B' " +
                //     " and (" + qryAccountList + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + qryAccountList + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + qryAccountList + ".Ac_Name_E";
                //this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtTRANSPORT_CODE")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select transport Code--";
                string qry = "select Ac_Code,Ac_Name_E as Name,cityname as City from qrymstaccountmaster where Company_Code=" + Session["Company_Code"].ToString() + " " +
                   "  and Ac_type='T'    and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or cityname like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                this.showPopup(qry);
                //string qry = "select " + qryAccountList + ".Ac_Code," + qryAccountList + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + qryAccountList +
                //    " left outer join " + cityMasterTable + " on " + qryAccountList + ".City_Code=" + cityMasterTable + ".city_code and " + qryAccountList + ".Company_Code=" + cityMasterTable + ".company_code where " +
                //    qryAccountList + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + qryAccountList + ".Ac_type='T' " +
                //     " and (" + qryAccountList + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + qryAccountList + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + qryAccountList + ".Ac_Name_E";
                //this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNARRATION1")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Narration --";
                string qry = "select System_Name_E as Narration from " + SystemMastertable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNARRATION2")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E as Narration from " + SystemMastertable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNARRATION3")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E as Narration from " + SystemMastertable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNARRATION4" || hdnfClosePopup.Value == "txtparty" || hdnfClosePopup.Value == "txtSaleBillTo")
            {
                tdDate.Visible = false;
                string self_ac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (drpDeliveryType.SelectedValue == "DI")
                {

                    string qry = "select Ac_Code,Ac_Name_E as Name,cityname as City from qrymstaccountmaster where Company_Code=" + Session["Company_Code"].ToString() + " " +
                  "   and Ac_Code!=2 and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or cityname like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                    this.showPopup(qry);
                    //if (self_ac == txtGETPASS_CODE.Text)
                    //{
                    //    hdnfClosePopup.Value = "txtparty";
                    //    lblPopupHead.Text = "--Select Party--";
                    //    string qry = "select " + qryAccountList + ".Ac_Code," + qryAccountList + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + qryAccountList +
                    //  " left outer join " + cityMasterTable + " on " + qryAccountList + ".City_Code=" + cityMasterTable + ".city_code and " + qryAccountList + ".Company_Code=" + cityMasterTable + ".company_code where " +
                    //  qryAccountList + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + qryAccountList + ".Ac_type!='C' and " + qryAccountList + ".Ac_type!='B' " +
                    //   " and (" + qryAccountList + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + qryAccountList + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + qryAccountList + ".Ac_Name_E";
                    //    this.showPopup(qry);
                    //}
                    //else
                    //{
                    //    hdnfClosePopup.Value = "txtNARRATION4";
                    //    lblPopupHead.Text = "--Select Narration--";
                    //    string qry = "select System_Name_E as Narration from " + SystemMastertable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                    //    this.showPopup(qry);
                    //}
                }
                else
                {
                    txtSearchText.Text = searchString;
                    hdnfClosePopup.Value = "txtparty";
                    lblPopupHead.Text = "--Select Party--";

                    string qry = "select Ac_Code,Ac_Name_E as Name,cityname as City from qrymstaccountmaster where Company_Code=" + Session["Company_Code"].ToString() + " " +
                  "  and Ac_Code!=2   and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or cityname like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                    this.showPopup(qry);

                    //  string qry = "select " + qryAccountList + ".Ac_Code," + qryAccountList + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + qryAccountList +
                    //" left outer join " + cityMasterTable + " on " + qryAccountList + ".City_Code=" + cityMasterTable + ".city_code and " + qryAccountList + ".Company_Code=" + cityMasterTable + ".company_code where " +
                    //qryAccountList + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + qryAccountList + ".Ac_type!='C' and " + qryAccountList + ".Ac_type!='B' " +
                    // " and (" + qryAccountList + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + qryAccountList + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + qryAccountList + ".Ac_Name_E";
                    //  this.showPopup(qry);
                }
            }
            if (hdnfClosePopup.Value == "txtBANK_CODE")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Bank--";
                string qry = "select Ac_Code,Ac_Name_E as Name,cityname as City from qrymstaccountmaster where Company_Code=" + Session["Company_Code"].ToString() + " " +
                   "  and Ac_type='B'   and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or cityname like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                this.showPopup(qry);
                //string qry = "select " + qryAccountList + ".Ac_Code," + qryAccountList + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + qryAccountList +
                //    " left outer join " + cityMasterTable + " on " + qryAccountList + ".City_Code=" + cityMasterTable + ".city_code and " + qryAccountList + ".Company_Code=" + cityMasterTable + ".company_code where " +
                //    qryAccountList + ".Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                //     " and (" + qryAccountList + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + qryAccountList + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + qryAccountList + ".Ac_Name_E";
                //this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtNARRATION")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E as Narration from " + SystemMastertable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtitem_Code")
            {
                tdDate.Visible = false;
                lblPopupHead.Text = "--Select item--";
                //string qry = "select itemcode ,itemname as Name from qrytenderhead where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString() + " " +
                //    " and Year_Code=" + Session["year"].ToString() + " and (itemcode like '%" + txtSearchText.Text + "%' or  itemname like  '%" + txtSearchText.Text + "%'");

                qry = "select distinct System_Code,System_Name_E from nt_1_systemmaster where System_Type='I' and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " +
                    " and (System_Code like '%" + txtSearchText.Text + "%' or System_Name_E like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtPurcNo")
            {
                tdDate.Visible = false;
                if (txtMILL_CODE.Text != string.Empty)
                {
                    lblPopupHead.Text = "--Select No--";
                    //string qry = "select Tender_No,Convert(varchar(10),Tender_Date,103) as Tender_Date,salepartyfullname as Party,buyerbrokerfullname as Party2,Mill_Rate,Grade,Sale_Rate,Buyer_Quantal,despatchqty,balance,doname,Convert(varchar(10),Lifting_Date,103) as Lifting_Date,ID,Delivery_Type as DT from "
                    // + qrypurc_No + " where  Mill_Code=" + txtMILL_CODE.Text + " and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                    // " and (Tender_No like '%" + txtSearchText.Text + "%' or  Tender_Date like '%" + txtSearchText.Text + "%' or salepartyfullname like  '%" + txtSearchText.Text + "%' or buyerbrokerfullname like '%" + txtSearchText.Text + "%' or doname like '%" + txtSearchText.Text + "%') and balance!=0 and Mill_Code=" + txtMILL_CODE.Text + "  order by Tender_No desc";

                    string qry = "select Tender_No,Tender_DateConverted as Tender_Date,buyername as Party2,buyerpartyname as Party,Mill_Rate,Grade,Sale_Rate,Buyer_Quantal,DESPATCH,BALANCE," +
                        "tenderdoname as doname,Lifting_DateConverted as Lifting_Date,ID,tenderdetailid from " +
                     " qrytenderdobalanceview where  Mill_Code=" + txtMILL_CODE.Text + " and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " +
                     " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                     " and (Tender_No like '%" + txtSearchText.Text + "%' or  buyerpartyname like  '%" + txtSearchText.Text + "%' or buyername like '%" + txtSearchText.Text + "%' or tenderdoname like '%" + txtSearchText.Text + "%') " +
                     "  and balance!=0   order by Tender_No desc";

                    this.showPopup(qry);
                }
                else
                {
                    setFocusControl(txtMILL_CODE);
                    pnlPopup.Style["display"] = "none";
                }
            }

            if (hdnfClosePopup.Value == "txtcarporateSale")//
            {
                tdDate.Visible = false;
                string qry = "";
                lblPopupHead.Text = "--Select Carporate Sale No--";
                if (ViewState["mode"].ToString() == "I")
                {
                    qry = "select distinct(Doc_No),doc_dateConverted as Doc_Date,carporatepartyaccountname as partyName,carporatepartyunitname as UnitName,sell_rate,pono as Po_Details,quantal,dispatched,balance,selling_type  from qrycarporatedobalance where balance!=0 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and (carporatepartyaccountname like '%" + txtSearchText.Text + "%' or carporatepartyunitname like '%" + txtSearchText.Text + "%' or doc_dateConverted like '%" + txtSearchText.Text + "%')";
                }
                else
                {
                    qry = "select distinct(Doc_No),doc_dateConverted as Doc_Date,carporatepartyaccountname as partyName,carporatepartyunitname as UnitName,sell_rate,pono as Po_Details,quantal,dispatched,balance,selling_type  from qrycarporatedobalance where balance!=0 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and (carporatepartyaccountname like '%" + txtSearchText.Text + "%' or carporatepartyunitname like '%" + txtSearchText.Text + "%' or doc_dateConverted like '%" + txtSearchText.Text + "%')";
                }

                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtUTRNo")
            {
                using (clsDataProvider obj = new clsDataProvider())
                {
                    tdDate.Visible = false;
                    if (txtBANK_CODE.Text != string.Empty)
                    {
                        lblPopupHead.Text = "--Select UTR No--";
                        //string qry = "select doc_no,utr_no,bankname,UTRAmount,UsedAmt,balance,narration_header,Year_Code,doc_date  from " + tblPrefix 
                        //    + "qryUTRBalanceForDO where balance!=0 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) 
                        //    + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and mill_code=" + txtBANK_CODE.Text;

                        //string qry = "select doc_no,utr_no,bankname,UTRAmount,UsedAmt,balance,narration_header,Year_Code,Convert(varchar(10),doc_date,103) as doc_date  from " + tblPrefix
                        //+ "qryUTRBalanceForDO where balance!=0 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                        //+ " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and mill_code=" + txtBANK_CODE.Text;

                        string qry = "select doc_no,utr_no,bankname,'' as UTRAmount,lot_no,amount as amountDetail,paidamount as UsedAmt,balanceamount as balance,narration_header,Year_Code,utrdateConverted as doc_date,utrdetailid  from " +
                        " qryutrdobalanceforfinalview  where balanceamount!=0 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                       + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and mill_code=" + txtBANK_CODE.Text;

                        DataSet ds = new DataSet();
                        ds = obj.GetDataSet(qry);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            dt = ds.Tables[0];
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string utrno = "";
                                string dtUtrNo = dt.Rows[i]["doc_no"].ToString();
                                double AmtTotal = 0.00;
                                for (int j = 0; j < grdDetail.Rows.Count; j++)
                                {
                                    string grdUtrNo = grdDetail.Rows[j].Cells[8].Text.ToString();
                                    string rowAction = grdDetail.Rows[j].Cells[12].Text.ToString();
                                    if (dtUtrNo == grdUtrNo && rowAction == "A")
                                    {
                                        double Amt = Convert.ToDouble(grdDetail.Rows[j].Cells[7].Text.ToString());
                                        AmtTotal += Amt;
                                        utrno = dtUtrNo;
                                    }
                                }
                                if (dtUtrNo == utrno)
                                {
                                    double balance = Convert.ToDouble(dt.Rows[i]["balance"].ToString());
                                    double totalBal = balance - AmtTotal;
                                    dt.Rows[i]["balance"] = totalBal;
                                }
                            }
                            if (dt.Rows.Count > 0)
                            {
                                for (int k = 0; k < dt.Rows.Count; k++)
                                {
                                    if (dt.Rows[k]["balance"].ToString() == "0")
                                    {
                                        dt.Rows[k].Delete();
                                    }
                                }
                                grdPopup.DataSource = dt;
                                grdPopup.DataBind();
                                hdHelpPageCount.Value = grdPopup.PageCount.ToString();
                                setFocusControl(txtSearchText);
                            }
                            else
                            {
                                grdPopup.DataSource = null;
                                grdPopup.DataBind();
                                hdHelpPageCount.Value = "0";
                            }
                        }
                        else
                        {
                            grdPopup.DataSource = null;
                            grdPopup.DataBind();
                            hdHelpPageCount.Value = "0";
                        }
                    }
                    else
                    {
                        setFocusControl(txtBANK_CODE);
                        pnlPopup.Style["display"] = "none";
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [Popup Button Code]
    protected void showPopup(string qry)
    {
        try
        {
            setFocusControl(txtSearchText);
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        grdPopup.DataSource = dt;
                        grdPopup.DataBind();
                        hdHelpPageCount.Value = grdPopup.PageCount.ToString();
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "kj", "javascript:SelectRow(0, {0});", true);
                        //grdPopup.Rows[0].Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});",grdPopup.Rows[0].RowIndex);
                    }
                    else
                    {
                        grdPopup.DataSource = null;
                        grdPopup.DataBind();
                        hdHelpPageCount.Value = "0";
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [imgBtnClose_Click]
    protected void imgBtnClose_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == "txtMILL_CODE")
            {
                setFocusControl(txtMILL_CODE);
            }
            if (hdnfClosePopup.Value == "txtGETPASS_CODE")
            {
                setFocusControl(txtGETPASS_CODE);
            }
            if (hdnfClosePopup.Value == "txtvoucher_by")
            {
                setFocusControl(txtvoucher_by);
            }
            if (hdnfClosePopup.Value == "txtBroker_CODE")
            {
                setFocusControl(txtBroker_CODE);
            }
            if (hdnfClosePopup.Value == "txtTRANSPORT_CODE")
            {
                setFocusControl(txtTRANSPORT_CODE);
            }
            if (hdnfClosePopup.Value == "txtDO_CODE")
            {
                setFocusControl(txtDO_CODE);
            }
            if (hdnfClosePopup.Value == "txtBANK_CODE")
            {
                setFocusControl(txtBANK_CODE);
            }

            hdnfClosePopup.Value = "Close";
            pnlPopup.Style["display"] = "none";
            txtSearchText.Text = string.Empty;
            grdPopup.DataSource = null;
            grdPopup.DataBind();
            setFocusControl(btnSave);
        }
        catch
        {
        }
    }
    #endregion

    #region csCalculations
    private void csCalculations()
    {

        bool isValidated = true;
        try
        {
            if (strTextBox == "txtdoc_no")
            {
                #region code
                try
                {
                    int n;
                    bool isNumeric = int.TryParse(txtdoc_no.Text, out n);
                    if (isNumeric == true)
                    {
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        string txtValue = "";
                        if (txtdoc_no.Text != string.Empty)
                        {
                            txtValue = txtdoc_no.Text;
                            string qry = "select * from " + tblHead + "  where   Doc_No='" + txtValue + "' " +
                                "  and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trnType + "'";

                            ds = clsDAL.SimpleQuery(qry);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    dt = ds.Tables[0];
                                    if (dt.Rows.Count > 0)
                                    {
                                        //Record Found
                                        hdnf.Value = dt.Rows[0]["doc_no"].ToString();

                                        if (ViewState["mode"] != null)
                                        {
                                            if (ViewState["mode"].ToString() == "I")
                                            {
                                                lblMsg.Text = "** Doc No (" + txtValue + ") Already Exist";
                                                lblMsg.ForeColor = System.Drawing.Color.Red;
                                                this.getMaxCode();
                                                txtdoc_no.Enabled = true;
                                                //hdnf.Value = txtdoc_no.Text;
                                                btnSave.Enabled = true;   //IMP
                                                setFocusControl(txtDOC_DATE);
                                            }

                                            if (ViewState["mode"].ToString() == "U")
                                            {
                                                //fetch record
                                                qry = getDisplayQuery();
                                                bool recordExist = this.fetchRecord(qry);
                                                pnlgrdDetail.Enabled = true;
                                                if (recordExist == true)
                                                {
                                                    txtdoc_no.Enabled = true;
                                                }
                                            }
                                        }
                                    }
                                    else   //Record Not Found
                                    {
                                        if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                        {
                                            lblMsg.Text = "";
                                            setFocusControl(txtDOC_DATE);
                                            txtdoc_no.Enabled = true;
                                            btnSave.Enabled = true;   //IMP
                                        }
                                        if (ViewState["mode"].ToString() == "U")
                                        {
                                            this.makeEmptyForm("E");
                                            lblMsg.Text = "** Record Not Found";
                                            lblMsg.ForeColor = System.Drawing.Color.Red;
                                            txtdoc_no.Text = string.Empty;
                                            setFocusControl(txtdoc_no);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblMsg.Text = string.Empty;
                            setFocusControl(txtdoc_no);
                        }
                    }
                    else
                    {
                        this.makeEmptyForm("A");
                        lblMsg.Text = "Doc No is numeric";
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        clsButtonNavigation.enableDisable("E");
                        txtdoc_no.Text = string.Empty;
                        setFocusControl(txtdoc_no);
                    }
                }
                catch
                {

                }
                #endregion

                return;
            }
            if (strTextBox == "txtDOC_DATE")
            {
                if (txtDOC_DATE.Text != string.Empty)
                {
                    try
                    {
                        //string dt = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                        //DateTime Start_Date = DateTime.Parse(HttpContext.Current.Session["Start_Date"].ToString());
                        //DateTime End_Date = DateTime.Parse(HttpContext.Current.Session["End_Date"].ToString());
                        //txtNARRATION1.Text = Start_Date.ToString();
                        //txtNARRATION2.Text = DateTime.Parse(HttpContext.Current.Session["Start_Date"].ToString()).ToString();
                        //txtNARRATION3.Text = End_Date.ToString();
                        //txtNARRATION4.Text = DateTime.Parse(HttpContext.Current.Session["End_Date"].ToString()).ToString();
                        if (clsCommon.isValidDate(txtDOC_DATE.Text) == true)
                        {
                            txtPurchase_Date.Text = txtDOC_DATE.Text;
                            setFocusControl(btntxtcarporateSale);
                        }
                        else
                        {
                            txtDOC_DATE.Text = string.Empty;
                            setFocusControl(txtDOC_DATE);
                        }
                    }
                    catch (Exception exx)
                    {
                        txtNARRATION1.Text = exx.Message;
                        txtDOC_DATE.Text = string.Empty;
                        setFocusControl(txtDOC_DATE);
                    }
                }
                else
                {
                    setFocusControl(txtDOC_DATE);
                }

                return;
            }
            if (strTextBox == "txtBill_To")
            {
                if (txtBill_To.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtBill_To.Text);
                    if (a == false)
                    {

                        // btntxtBill_To_Click(this, new EventArgs());
                        btntxtbill_To_Click(this, new EventArgs());
                    }
                    else
                    {
                        string billto = "";
                        string billshort = "";
                        if (txtBill_To.Text != string.Empty)
                        {
                            // billto = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtBill_To.Text + "");
                            //billshort = clsCommon.getString("select Short_Name from qrymstaccountmaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtBill_To.Text + "");

                            DataSet ds = clsDAL.SimpleQuery("select * from qrymstaccountmaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtBill_To.Text + "");
                            if (ds != null)
                            {
                                DataTable dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    billto = dt.Rows[0]["Ac_Name_E"].ToString();
                                    billshort = dt.Rows[0]["Short_Name"].ToString();
                                    hdnfbt.Value = dt.Rows[0]["accoid"].ToString();
                                }
                            }

                            if (billto != string.Empty && billto != "0")
                            {
                                lblBill_To.Text = billto;
                                hdnfbilltoshortname.Value = billshort;
                                // setFocusControl(txtpodetail);
                            }
                        }
                        else
                        {
                            //setFocusControl(txtpodetail);
                        }
                    }
                }
                else
                {
                    // setFocusControl(txtpodetail);
                }
            }
            if (strTextBox == "txtPurchase_Date")
            {
                if (txtPurchase_Date.Text != string.Empty)
                {
                    try
                    {
                        if (clsCommon.isValidDate(txtPurchase_Date.Text) == true)
                        {
                            setFocusControl(txtGETPASS_CODE);
                        }
                        else
                        {
                            txtPurchase_Date.Text = string.Empty;
                            setFocusControl(txtPurchase_Date);
                        }
                    }
                    catch (Exception exx)
                    {
                        txtNARRATION1.Text = exx.Message;
                        txtPurchase_Date.Text = string.Empty;
                        setFocusControl(txtPurchase_Date);
                    }
                }
                else
                {
                    setFocusControl(txtPurchase_Date);
                }

                return;
            }
            if (strTextBox == "drpDOType")
            {
                string s_item = "";
                s_item = drpDOType.SelectedValue;
                if (s_item == "DI")
                {
                    pnlgrdDetail.Enabled = true;
                    btnOpenDetailsPopup.Enabled = true;
                    grdDetail.DataSource = null;
                    grdDetail.DataBind();
                    drpDeliveryType.Visible = true;

                    // txtUTRNo.Enabled = true;
                    //btntxtUTRNo.Enabled = true;

                }
                else
                {
                    drpDeliveryType.Visible = false;
                    pnlgrdDetail.Enabled = false;
                    btnOpenDetailsPopup.Enabled = false;
                    grdDetail.DataSource = null;
                    grdDetail.DataBind();

                    //txtUTRNo.Text = "";
                    //txtUTRNo.Enabled = false;
                    //btntxtUTRNo.Enabled = false;
                }
                setFocusControl(txtMILL_CODE);
            }
            if (strTextBox == "txtSaleBillTo")
            {

                if (txtSaleBillTo.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtSaleBillTo.Text);
                    if (a == false)
                    {
                        btntxtNARRATION4_Click(this, new EventArgs());
                    }
                    else
                    {
                        if (txtSaleBillTo.Text != string.Empty)
                        {
                            txtSaleBillTo.Text = txtSaleBillTo.Text.Substring(1);
                        }

                        if (txtSaleBillTo.Text == "2")
                        {
                            txtSaleBillTo.Text = string.Empty;
                            setFocusControl(txtSaleBillTo);
                            return;
                        }
                        string salebillname = string.Empty;
                        string salebilltoshortname = "";
                        string gststatecode1 = string.Empty;
                        string stateName1 = "";

                        DataSet ds = clsDAL.SimpleQuery("select * from " + qryAccountList + "   where Ac_Code="
                            + txtSaleBillTo.Text + " and Ac_type!='C' and Ac_type!='B' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (ds != null)
                        {
                            DataTable dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                salebillname = dt.Rows[0]["Ac_Name_E"].ToString();
                                salebilltoshortname = dt.Rows[0]["Short_Name"].ToString();
                                gststatecode1 = dt.Rows[0]["GSTStateCode"].ToString();
                                stateName1 = dt.Rows[0]["State_Name"].ToString();
                                hdnfsb.Value = dt.Rows[0]["accoid"].ToString();
                            }
                        }

                        if (salebillname != string.Empty && salebillname != "0")
                        {

                            // salebilltoshortname = clsCommon.getString("select Short_Name from qrymstaccountmaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtSaleBillTo.Text + "");
                            hdnfbilltoshortname.Value = salebilltoshortname;
                            hdnfsalebilltoshortname.Value = salebilltoshortname;
                            txtNARRATION4.Text = salebillname;
                            lblSaleBillToLedgerByBalance.Text = AcBalance(txtSaleBillTo.Text);
                            // gststatecode1 = clsCommon.getString("select GSTStateCode from " + qryAccountList + "   where Ac_Code="
                            //    + txtSaleBillTo.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                            //if (gststatecode1.Trim() != string.Empty)
                            //{
                            //    stateName1 = clsCommon.getString("select State_Name from GSTStateMaster   where State_Code=" + gststatecode1 + "");
                            //}
                            txtSalebilltoGstStateCode.Text = gststatecode1;
                            lbltxtSalebilltoGstStateName.Text = stateName1;
                            setFocusControl(txtGRADE);
                        }
                        else
                        {
                            txtSaleBillTo.Text = string.Empty;
                            txtNARRATION4.Text = salebillname;
                            setFocusControl(txtSaleBillTo);
                        }


                    }
                }
                else
                {
                    txtNARRATION4.Text = "";
                    setFocusControl(txtSaleBillTo);
                }
                return;
            }
            if (strTextBox == "txtitem_Code")
            {
                if (txtitem_Code.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtitem_Code.Text);
                    if (a == false)
                    {
                        btntxtitem_Code_Click(this, new EventArgs());
                    }
                    else
                    {
                        lblitem_Name.Text = clsCommon.getString("select System_Name_E from qrymstitem where System_Code=" + txtitem_Code.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " ");
                        hdnfic.Value = clsCommon.getString("select systemid from qrymstitem where System_Code=" + txtitem_Code.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " ");

                        if (lblitem_Name.Text != string.Empty && lblitem_Name.Text != "0")
                        {
                            setFocusControl(txtvoucher_by);
                        }
                        else
                        {
                            txtitem_Code.Text = string.Empty;
                            setFocusControl(txtitem_Code);
                        }

                    }
                }
                else
                {
                    setFocusControl(txtitem_Code);
                    return;
                }
            }
            if (strTextBox == "txtMILL_CODE")
            {
                string millName = "";
                if (txtMILL_CODE.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtMILL_CODE.Text);
                    if (a == false)
                    {
                        btntxtMILL_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        string millshortname1 = "";
                        string gststatecode1 = "";
                        string stateName1 = "";
                        //millName = clsCommon.getString("select Ac_Name_E from " + qryAccountList + "  where Ac_Code=" + txtMILL_CODE.Text + " and Ac_type='M' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        DataSet ds = clsDAL.SimpleQuery("select * from " + qryAccountList + "  where Ac_Code=" + txtMILL_CODE.Text + " and Ac_type='M' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (ds != null)
                        {
                            DataTable dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                millName = dt.Rows[0]["Ac_Name_E"].ToString();
                                millshortname1 = dt.Rows[0]["Short_Name"].ToString();
                                txtMillEmailID.Text = dt.Rows[0]["Email_Id"].ToString();
                                txtMillMobile.Text = dt.Rows[0]["Mobile_No"].ToString();
                                gststatecode1 = dt.Rows[0]["GSTStateCode"].ToString();
                                stateName1 = dt.Rows[0]["State_Name"].ToString();
                                hdnfmc.Value = dt.Rows[0]["accoid"].ToString();
                            }
                        }

                        if (millName != string.Empty && millName != "0")
                        {

                            // millshortname1 = clsCommon.getString("select Short_Name from qrymstaccountmaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtMILL_CODE.Text + "");

                            hdnfmillshortname.Value = millshortname1;
                            // txtMillEmailID.Text = clsCommon.getString("select Email_Id from " + qryAccountList + "  where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            // txtMillMobile.Text = clsCommon.getString("select Mobile_No from " + qryAccountList + "  where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                            LBLMILL_NAME.Text = millName;

                            // gststatecode1 = clsCommon.getString("select GSTStateCode from " + qryAccountList + "   where Ac_Code=" + txtMILL_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                            //if (gststatecode1.Trim() != string.Empty)
                            //{
                            //    stateName1 = clsCommon.getString("select State_Name from GSTStateMaster  where State_Code=" + gststatecode1 + "");
                            //}
                            txtMillGstStateCode.Text = gststatecode1;
                            lbltxtMillGstStateCode.Text = stateName1;
                            setFocusControl(btntxtPurcNo);
                        }
                        else
                        {
                            txtMILL_CODE.Text = string.Empty;
                            LBLMILL_NAME.Text = millName;
                            setFocusControl(txtMILL_CODE);
                        }
                    }
                }
                else
                {
                    LBLMILL_NAME.Text = "";
                    setFocusControl(txtMILL_CODE);
                }
                return;
            }

            if (strTextBox == "txtGstRate")
            {
                string gstratename = "";
                if (txtGstRate.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtGstRate.Text);
                    if (a == false)
                    {
                        btntxtGstRate_Click(this, new EventArgs());
                    }
                    else
                    {
                        gstratename = clsCommon.getString("select GST_Name from " + tblPrefix + "GSTRateMaster  where Doc_No=" + txtGstRate.Text + "");
                        if (gstratename != string.Empty && gstratename != "0")
                        {
                            lblGstRateName.Text = gstratename;

                            GSTCalculations();
                            setFocusControl(txtPurchase_Date);
                        }
                        else
                        {
                            txtGstRate.Text = string.Empty;
                            lblGstRateName.Text = gstratename;
                            setFocusControl(txtGstRate);
                        }
                    }
                }
                else
                {
                    lblGstRateName.Text = "";
                    setFocusControl(txtGstRate);
                }
                return;
            }

            if (strTextBox == "txtMillEmailID")
            {
                setFocusControl(txtPurcNo);
            }
            if (strTextBox == "txtMillGstStateCode")
            {
                string stateName = "";
                if (txtMillGstStateCode.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtMillGstStateCode.Text);
                    if (a == false)
                    {
                        btntxtMillGstStateCode_Click(this, new EventArgs());
                    }
                    else
                    {
                        stateName = clsCommon.getString("select State_Name from GSTStateMaster where State_Code=" + txtMillGstStateCode.Text + "");
                        if (stateName != string.Empty && stateName != "0")
                        {
                            lbltxtMillGstStateCode.Text = stateName;
                            setFocusControl(btntxtPurcNo);
                        }
                        else
                        {
                            txtMillGstStateCode.Text = string.Empty;
                            lbltxtMillGstStateCode.Text = stateName;
                            setFocusControl(txtMillGstStateCode);
                        }
                    }
                }
                else
                {
                    lbltxtMillGstStateCode.Text = stateName;
                    setFocusControl(txtMillGstStateCode);
                }
                return;
            }

            if (strTextBox == "txtTransportGstStateCode")
            {
                string stateName = "";
                if (txtTransportGstStateCode.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtTransportGstStateCode.Text);
                    if (a == false)
                    {
                        btntxtTransportGstStateCode_Click(this, new EventArgs());
                    }
                    else
                    {
                        stateName = clsCommon.getString("select State_Name from GSTStateMaster  where State_Code=" + txtTransportGstStateCode.Text + "");
                        if (stateName != string.Empty && stateName != "0")
                        {
                            lbltxtTransportGstStateCode.Text = stateName;
                            setFocusControl(txtFrieght);
                        }
                        else
                        {
                            txtTransportGstStateCode.Text = string.Empty;
                            lbltxtTransportGstStateCode.Text = stateName;
                            setFocusControl(txtTransportGstStateCode);
                        }
                    }
                }
                else
                {
                    lbltxtTransportGstStateCode.Text = stateName;
                    setFocusControl(txtTransportGstStateCode);
                }
                return;
            }



            if (strTextBox == "txtGetpassGstStateCode")
            {
                string stateName = "";
                if (txtGetpassGstStateCode.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtGetpassGstStateCode.Text);
                    if (a == false)
                    {
                        btntxtGetpassGstStateCode_Click(this, new EventArgs());
                    }
                    else
                    {
                        stateName = clsCommon.getString("select State_Name from GSTStateMaster   where State_Code=" + txtGetpassGstStateCode.Text + "");
                        if (stateName != string.Empty && stateName != "0")
                        {
                            lbltxtGetpassGstStateName.Text = stateName;
                            setFocusControl(txtitem_Code);
                        }
                        else
                        {
                            txtGetpassGstStateCode.Text = string.Empty;
                            lbltxtGetpassGstStateName.Text = stateName;
                            setFocusControl(txtGetpassGstStateCode);
                        }
                    }
                }
                else
                {
                    lbltxtGetpassGstStateName.Text = stateName;
                    setFocusControl(txtGetpassGstStateCode);
                }
                return;
            }

            if (strTextBox == "txtVoucherbyGstStateCode")
            {
                string stateName = "";
                if (txtVoucherbyGstStateCode.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtVoucherbyGstStateCode.Text);
                    if (a == false)
                    {
                        btntxtVoucherbyGstStateCode_Click(this, new EventArgs());
                    }
                    else
                    {
                        stateName = clsCommon.getString("select State_Name from GSTStateMaster   where State_Code=" + txtVoucherbyGstStateCode.Text + "");
                        if (stateName != string.Empty && stateName != "0")
                        {
                            lbltxtVoucherbyGstStateName.Text = stateName;
                            if (txtNARRATION4.Enabled == true)
                            {
                                setFocusControl(txtNARRATION4);
                            }
                            else
                            {
                                setFocusControl(txtSaleBillTo);
                            }
                        }
                        else
                        {
                            txtVoucherbyGstStateCode.Text = string.Empty;
                            lbltxtVoucherbyGstStateName.Text = stateName;
                            setFocusControl(txtVoucherbyGstStateCode);
                        }
                    }
                }
                else
                {
                    lbltxtVoucherbyGstStateName.Text = stateName;
                    setFocusControl(txtVoucherbyGstStateCode);
                }
                return;
            }

            if (strTextBox == "txtSalebilltoGstStateCode")
            {
                string stateName = "";
                if (txtSalebilltoGstStateCode.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtSalebilltoGstStateCode.Text);
                    if (a == false)
                    {
                        btntxtSalebilltoGstStateCode_Click(this, new EventArgs());
                    }
                    else
                    {
                        stateName = clsCommon.getString("select State_Name from GSTStateMaster   where State_Code=" + txtSalebilltoGstStateCode.Text + "");
                        if (stateName != string.Empty && stateName != "0")
                        {
                            lbltxtSalebilltoGstStateName.Text = stateName;
                            setFocusControl(txtGRADE);
                        }
                        else
                        {
                            lbltxtSalebilltoGstStateName.Text = string.Empty;
                            lbltxtSalebilltoGstStateName.Text = stateName;
                            setFocusControl(txtSalebilltoGstStateCode);
                        }
                    }
                }
                else
                {
                    lbltxtSalebilltoGstStateName.Text = stateName;
                    setFocusControl(txtSalebilltoGstStateCode);
                }
                return;
            }
            if (strTextBox == "txtGETPASS_CODE")
            {
                string getPassName = "";
                if (txtGETPASS_CODE.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtGETPASS_CODE.Text);
                    if (a == false)
                    {
                        btntxtGETPASS_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        if (txtGETPASS_CODE.Text != string.Empty)
                        {
                            txtGETPASS_CODE.Text = txtGETPASS_CODE.Text.Substring(1);
                        }
                        getPassName = clsCommon.getString("select Ac_Name_E from " + qryAccountList + "   where Ac_Code=" + txtGETPASS_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (getPassName != string.Empty && getPassName != "0")
                        {
                            string getpasscodecitycode = clsCommon.getString("Select City_Code from " + qryAccountList + "   where Ac_Code=" + txtGETPASS_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            string getpasscity = clsCommon.getString("Select city_name_e from " + tblPrefix + "CityMaster   where city_code=" + getpasscodecitycode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            LBLGETPASS_NAME.Text = getPassName + ", " + getpasscity;
                            string getpassshortname = "";
                            getpassshortname = clsCommon.getString("select Short_Name from " + qryAccountList + "   where Ac_Code=" + txtGETPASS_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            hdnfgetpassshortname.Value = getpassshortname;

                            hdnfgp.Value = clsCommon.getString("select ifnull(accoid,0) as id from " + qryAccountList + "   where Ac_Code=" + txtGETPASS_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                            if (txtvoucher_by.Text == "2")
                            {
                                //txtGETPASS_CODE.Text = txtGETPASS_CODE.Text.Substring(1);
                                txtvoucher_by.Text = txtGETPASS_CODE.Text;
                                lblvoucherbyname.Text = LBLGETPASS_NAME.Text + ", " + getpasscity; ;
                                txtCommission.Text = clsCommon.getString("select ISNULL(Commission,0) from " + qryAccountList + "  where Ac_Code=" + txtvoucher_by.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                                //string selfac = Session["SELF_AC"].ToString();
                                if (txtGETPASS_CODE.Text == Session["SELF_AC"].ToString())
                                {
                                    txtSALE_RATE.Text = txtmillRate.Text;
                                    calculation();
                                }

                                string gststatecode1 = clsCommon.getString("select GSTStateCode from " + qryAccountList + "   where Ac_Code=" + txtvoucher_by.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                                string stateName1 = "";
                                if (gststatecode1.Trim() != string.Empty)
                                {
                                    stateName1 = clsCommon.getString("select State_Name from GSTStateMaster where State_Code=" + gststatecode1 + "");
                                }
                                txtVoucherbyGstStateCode.Text = gststatecode1;
                                lbltxtVoucherbyGstStateName.Text = stateName1;
                            }

                            string gststatecode = clsCommon.getString("select GSTStateCode from " + qryAccountList + "   where Ac_Code=" + txtGETPASS_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            string stateName = "";
                            if (gststatecode.Trim() != string.Empty && gststatecode != "0")
                            {
                                stateName = clsCommon.getString("select State_Name from GSTStateMaster   where State_Code=" + gststatecode + "");
                            }
                            txtGetpassGstStateCode.Text = gststatecode;
                            lbltxtGetpassGstStateName.Text = stateName;
                            setFocusControl(txtGetpassGstStateCode);
                        }
                        else
                        {
                            txtGETPASS_CODE.Text = string.Empty;
                            LBLGETPASS_NAME.Text = getPassName;
                            setFocusControl(txtGETPASS_CODE);
                        }
                    }
                }
                else
                {
                    LBLGETPASS_NAME.Text = "";
                    setFocusControl(txtGETPASS_CODE);
                }
            }
            if (strTextBox == "txtvoucher_by")
            {
                string vByName = "";
                string voucherbycitycode = "";
                string voucherbycity = "";
                string distance = "";
                if (txtvoucher_by.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtvoucher_by.Text);
                    if (a == false)
                    {
                        btntxtvoucher_by_Click(this, new EventArgs());
                    }
                    else
                    {
                        if (txtvoucher_by.Text != string.Empty)
                        {
                            txtvoucher_by.Text = txtvoucher_by.Text.Substring(1);
                        }
                        //vByName = clsCommon.getString("select Ac_Name_E from " + qryAccountList + "   where Ac_Code=" + txtvoucher_by.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        DataSet ds = clsDAL.SimpleQuery("select * from " + qryAccountList + "   where Ac_Code=" + txtvoucher_by.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (ds != null)
                        {
                            DataTable dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                vByName = dt.Rows[0]["Ac_Name_E"].ToString();
                                voucherbycitycode = dt.Rows[0]["City_Code"].ToString();
                                voucherbycity = dt.Rows[0]["cityname"].ToString();
                                distance = dt.Rows[0]["Distance"].ToString();
                                hdnfst.Value = dt.Rows[0]["accoid"].ToString();
                            }
                        }

                        if (vByName != string.Empty && vByName != "0")
                        {
                            lblVoucherLedgerByBalance.Text = AcBalance(txtvoucher_by.Text);
                            //string voucherbycitycode = clsCommon.getString("Select City_Code from " + qryAccountList + "   where Ac_Code=" + txtvoucher_by.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            // string voucherbycity = clsCommon.getString("Select city_name_e from " + tblPrefix + "CityMaster   where city_code=" + voucherbycitycode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            //string distance = clsCommon.getString("Select Distance from " + qryAccountList + "   where Ac_Code=" + txtvoucher_by.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                            lblvoucherbyname.Text = vByName + ", " + voucherbycity;
                            txtDistance.Text = distance;
                            //txtDistance.Enabled = false;


                            if (txtPurcOrder.Text != "1" && txtPurcOrder.Text.Trim() != string.Empty)
                            {
                                calculation();
                            }
                            else
                            {
                                int csale = txtcarporateSale.Text != string.Empty ? Convert.ToInt32(txtcarporateSale.Text) : 0;
                                if (csale == 0)
                                {
                                    txtCommission.Text = clsCommon.getString("select ISNULL(Commission,0) from " + qryAccountList + "   where Ac_Code=" + txtvoucher_by.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                                }

                                calculation();
                            }

                            string gststatecode = clsCommon.getString("select GSTStateCode from " + qryAccountList + "   where Ac_Code=" + txtvoucher_by.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            string stateName = "";
                            if (gststatecode.Trim() != string.Empty && gststatecode != "0")
                            {
                                stateName = clsCommon.getString("select State_Name from GSTStateMaster where State_Code=" + gststatecode + "");
                            }
                            txtVoucherbyGstStateCode.Text = gststatecode;
                            lbltxtVoucherbyGstStateName.Text = stateName;

                            setFocusControl(txtSaleBillTo);

                            // string selfac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters  where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            if (txtGETPASS_CODE.Text == Session["SELF_AC"].ToString() && Session["SELF_AC"].ToString() != "0")
                            {
                                //txtSALE_RATE.Text = txtmillRate.Text;
                                calculation();
                            }

                            if (txtvoucher_by.Text == Session["SELF_AC"].ToString())
                            {
                                if (txtSaleBillTo.Text == string.Empty)
                                {
                                    txtNARRATION4.Text = lblvoucherbyname.Text;
                                    txtSaleBillTo.Text = txtvoucher_by.Text;
                                    txtSalebilltoGstStateCode.Text = txtVoucherbyGstStateCode.Text;
                                    lbltxtSalebilltoGstStateName.Text = lbltxtVoucherbyGstStateName.Text;
                                }
                                //txtNARRATION4.Text = "";
                                // txtSaleBillTo.Text = "";
                                // txtSalebilltoGstStateCode.Text = "";
                                // lbltxtSalebilltoGstStateName.Text = "";
                            }
                            if (txtvoucher_by.Text != Session["SELF_AC"].ToString() && txtcarporateSale.Text == string.Empty)
                            {
                                if (txtSaleBillTo.Text == string.Empty)
                                {
                                    txtNARRATION4.Text = lblvoucherbyname.Text;
                                    txtSaleBillTo.Text = txtvoucher_by.Text;
                                    txtSalebilltoGstStateCode.Text = txtVoucherbyGstStateCode.Text;
                                    lbltxtSalebilltoGstStateName.Text = lbltxtVoucherbyGstStateName.Text;
                                }

                            }


                            if (txtGETPASS_CODE.Text == Session["SELF_AC"].ToString() && txtvoucher_by.Text == Session["SELF_AC"].ToString())
                            {
                                txtSALE_RATE.Text = txtmillRate.Text;
                            }
                            setFocusControl(txtSaleBillTo);



                        }
                        else
                        {
                            txtvoucher_by.Text = string.Empty;
                            lblvoucherbyname.Text = vByName;
                            setFocusControl(txtvoucher_by);
                        }
                    }
                }
                else
                {
                    lblvoucherbyname.Text = "";
                    setFocusControl(txtGETPASS_CODE);
                }
            }

            if (strTextBox == "txtVasuliAc")
            {
                string vByName = "";
                if (txtVasuliAc.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtVasuliAc.Text);
                    if (a == false)
                    {
                        btntxtVasuliAc_Click(this, new EventArgs());
                    }
                    else
                    {
                        vByName = clsCommon.getString("select Ac_Name_E from " + qryAccountList + "  where Ac_Code=" + txtVasuliAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        hdnfva.Value = clsCommon.getString("select ifnull(accoid,0) as id from " + qryAccountList + "  where Ac_Code=" + txtVasuliAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        if (vByName != string.Empty && vByName != "0")
                        {
                            string tooltip = vByName;
                            if (vByName.Length > 25)
                            {
                                vByName = vByName.Substring(0, vByName.Length - 25);
                            }

                            lbltxtVasuliAc.Text = vByName;
                            lbltxtVasuliAc.ToolTip = tooltip;

                            setFocusControl(txtDO_CODE);
                        }
                        else
                        {
                            txtVasuliAc.Text = string.Empty;
                            lbltxtVasuliAc.Text = vByName;
                            setFocusControl(txtDO_CODE);
                        }
                    }
                }
                else
                {
                    lbltxtVasuliAc.Text = "";
                    //setFocusControl(txtVasuliAc);
                }
            }
            if (strTextBox == "txtGRADE")
            {
                setFocusControl(txtquantal);
            }

            if (strTextBox == "txtmillRate")
            {
                //txtPACKING.Text = "50";
                setFocusControl(txtSALE_RATE);
            }
            if (strTextBox == "txtSALE_RATE")
            {
                setFocusControl(txtTruck_NO);
            }
            if (strTextBox == "txtDIFF_AMOUNT")
            {
                setFocusControl(txtFrieght);
            }
            if (strTextBox == "txtquantal")
            {
                setFocusControl(txtmillRate);
            }
            if (strTextBox == "txtwithGst_Amount")
            {

                double millamount = txtmillRate.Text != string.Empty ? Convert.ToDouble(txtmillRate.Text) : 0.00;
                double tax = 5.00;
                double result = 0.00;
                tax = tax / 100;
                result = millamount * tax;
                //txtwithGst_Amount.Text = result.ToString();
                txtwithGst_Amount.Text = (millamount + result).ToString();
            }
            if (strTextBox == "txtPACKING")
            {
                setFocusControl(txtBAGS);
            }
            if (strTextBox == "txtexcise_rate")
            {
                setFocusControl(txtFrieght);
            }
            if (strTextBox == "txtFrieght")
            {
                setFocusControl(txtMemoAdvanceRate);
            }
            if (strTextBox == "txtVasuliRate")
            {
                setFocusControl(txtVasuliRate1);
            }
            if (strTextBox == "txtVasuliRate1")
            {
                setFocusControl(txtVasuliAc);
            }
            if (strTextBox == "txtMemoAdvance")
            {
                setFocusControl(txtVasuliRate);
            }
            if (strTextBox == "txtexcise_rate")
            {
                setFocusControl(txtmillRate);
            }
            if (strTextBox == "txtDO_CODE")
            {
                string doname = "";
                if (txtDO_CODE.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtDO_CODE.Text);
                    if (a == false)
                    {
                        btntxtDO_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        doname = clsCommon.getString("select Ac_Name_E from " + qryAccountList + "   where Ac_Code=" + txtDO_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        hdnfdocd.Value = clsCommon.getString("select ifnull(accoid,0) as id from " + qryAccountList + "  where Ac_Code=" + txtDO_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        if (doname != string.Empty && doname != "0")
                        {
                            LBLDO_NAME.Text = doname;
                            setFocusControl(txtBroker_CODE);
                        }
                        else
                        {
                            txtDO_CODE.Text = string.Empty;
                            LBLDO_NAME.Text = doname;
                            setFocusControl(txtDO_CODE);
                        }
                    }
                }
                else
                {
                    LBLDO_NAME.Text = "";
                    setFocusControl(txtDO_CODE);
                }
            }
            if (strTextBox == "txtBroker_CODE")
            {
                string brokerName = "";
                if (txtBroker_CODE.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtBroker_CODE.Text);
                    if (a == false)
                    {
                        btntxtBroker_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        brokerName = clsCommon.getString("select Ac_Name_E from " + qryAccountList + "   where Ac_Code=" + txtBroker_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        hdnfbk.Value = clsCommon.getString("select ifnull(accoid,0) as id from " + qryAccountList + "  where Ac_Code=" + txtBroker_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        if (brokerName != string.Empty && brokerName != "0")
                        {

                            LBLBROKER_NAME.Text = brokerName;
                            setFocusControl(txtMillEwayBill_No);
                        }
                        else
                        {
                            txtBroker_CODE.Text = string.Empty;
                            LBLBROKER_NAME.Text = brokerName;
                            setFocusControl(txtBroker_CODE);
                        }
                    }
                }
                else
                {
                    LBLBROKER_NAME.Text = "";
                    setFocusControl(txtBroker_CODE);
                }
            }
            if (strTextBox == "txtTruck_NO")
            {
                setFocusControl(txtDriverMobile);
            }
            if (strTextBox == "txtDriverMobile")
            {
                setFocusControl(txtTRANSPORT_CODE);
            }
            if (strTextBox == "txtTRANSPORT_CODE")
            {
                string transportName = "";
                if (txtTRANSPORT_CODE.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtTRANSPORT_CODE.Text);
                    if (a == false)
                    {
                        btntxtTRANSPORT_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        transportName = clsCommon.getString("select Ac_Name_E from " + qryAccountList + "  where Ac_Code=" + txtTRANSPORT_CODE.Text + " and Ac_type='T' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string transportshortname = "";
                        transportshortname = clsCommon.getString("select Short_Name from " + qryAccountList + "  where Ac_Code=" + txtTRANSPORT_CODE.Text + " and Ac_type='T' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        hdnftransportshortname.Value = transportshortname;

                        hdnftc.Value = clsCommon.getString("select ifnull(accoid,0) as id from " + qryAccountList + "  where Ac_Code=" + txtTRANSPORT_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));


                        if (transportName != string.Empty && transportName != "0")
                        {
                            LBLTRANSPORT_NAME.Text = transportName;
                            string gststatecode1 = clsCommon.getString("select GSTStateCode from " + qryAccountList + "  where Ac_Code=" + txtTRANSPORT_CODE.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            string stateName1 = "";
                            if (gststatecode1.Trim() != string.Empty)
                            {
                                stateName1 = clsCommon.getString("select State_Name from GSTStateMaster   where State_Code=" + gststatecode1 + "");
                            }
                            txtTransportGstStateCode.Text = gststatecode1;
                            lbltxtTransportGstStateCode.Text = stateName1;
                            setFocusControl(txtFrieght);
                        }
                        else
                        {
                            txtTRANSPORT_CODE.Text = string.Empty;
                            LBLTRANSPORT_NAME.Text = transportName;
                            setFocusControl(txtTRANSPORT_CODE);
                        }
                    }
                }
                else
                {
                    LBLTRANSPORT_NAME.Text = "";
                    setFocusControl(txtTRANSPORT_CODE);
                }
            }
            if (strTextBox == "txtNARRATION1")
            {
                setFocusControl(txtNARRATION2);
            }
            if (strTextBox == "txtNARRATION2")
            {
                setFocusControl(txtNARRATION3);
            }
            if (strTextBox == "txtNARRATION3")
            {
                setFocusControl(txtNARRATION4);
            }
            if (strTextBox == "txtNARRATION4")
            {
                if (hdnfClosePopup.Value.Trim() == "Close")
                {
                    if (txtSaleBillTo.Text.Trim() != string.Empty)
                    {
                        string gststatecode = clsCommon.getString("select GSTStateCode from " + qryAccountList + "  where Ac_Code=" + txtSaleBillTo.Text + " and Ac_type!='B' and Ac_type!='C' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string stateName = "";
                        if (gststatecode.Trim() != string.Empty)
                        {
                            stateName = clsCommon.getString("select State_Name from GSTStateMaster  where State_Code=" + gststatecode + "");
                        }
                        txtSalebilltoGstStateCode.Text = gststatecode;
                        lbltxtSalebilltoGstStateName.Text = stateName;
                    }
                }

                setFocusControl(btnOpenDetailsPopup);
            }
            if (strTextBox == "txtBANK_CODE")
            {
                string bankName = "";
                if (txtBANK_CODE.Text != string.Empty)
                {
                    bankName = clsCommon.getString("select Ac_Name_E from " + qryAccountList + "  where Ac_Code=" + txtBANK_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (bankName != string.Empty && bankName != "0")
                    {

                        lblBank_name.Text = bankName;
                        setFocusControl(btntxtUTRNo);
                    }
                    else
                    {
                        txtBANK_CODE.Text = string.Empty;
                        lblBank_name.Text = bankName;
                        setFocusControl(txtBANK_CODE);
                    }
                }
                else
                {
                    lblBank_name.Text = "";
                    setFocusControl(txtBANK_CODE);
                }


            }
            if (strTextBox == "txtNARRATION")
            {
                setFocusControl(txtBANK_AMOUNT);
            }
            if (strTextBox == "txtBANK_AMOUNT")
            {
                setFocusControl(btnAdddetails);
            }

            if (strTextBox == "txtcarporateSale")
            {
                carporatesale();
            }
            //calculation();
        }
        catch
        {
        }
    }

    private void GSTCalculations()
    {
        try
        {
            if (txtGstRate.Text.Trim() != string.Empty)
            {
                string gstrate = clsCommon.getString("select Rate from " + tblPrefix + "GSTRateMaster   where Doc_No=" + txtGstRate.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                double rate = Convert.ToDouble(gstrate);

                double salerate = txtSALE_RATE.Text != string.Empty ? Convert.ToDouble(txtSALE_RATE.Text) : 0.0;
                double millrate = txtmillRate.Text != string.Empty ? Convert.ToDouble(txtmillRate.Text) : 0.0;

                double taxAmountOnSR = (salerate * (rate / 100));
                double gstExSaleRate = Math.Round(Math.Abs((salerate / (salerate + taxAmountOnSR) * salerate)), 2);
                double gstRateAmountOnSR = Math.Round(Math.Abs(salerate - gstExSaleRate), 2);

                //txtGstSRAmount.Text = gstRateAmountOnSR.ToString();
                //txtGstExSaleRate.Text = gstExSaleRate.ToString();

                double taxAmountOnMR = (millrate * (rate / 100));
                double gstExMillRate = Math.Round(Math.Abs((millrate / (millrate + taxAmountOnMR) * millrate)), 2);
                double gstRateAmountOnMR = Math.Round(Math.Abs(millrate - gstExMillRate), 2);

                //txtGstMRAmount.Text = gstRateAmountOnMR.ToString();
                //txtGstExMillRate.Text = gstExMillRate.ToString();
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void carporatesale()
    {
        if (txtcarporateSale.Text != string.Empty)
        {
            bool a = clsCommon.isStringIsNumeric(txtcarporateSale.Text);
            if (a == false)
            {
                btntxtcarporateSale_Click(this, new EventArgs());
            }
            else
            {

                if (txtcarporateSale.Text == "0")
                {
                    txtcarporateSale.Text = string.Empty;
                    setFocusControl(drpDOType);
                    return;
                }
                qry = "select ac_code as Ac_Code,carporatepartyaccountname as partyName,carporatepartyunitname as Unit_name,Unit_Code,carporatepartyunitname as UnitName, " +
                    " broker as BrokerCode,carporatepartybrokername as BrokerName,sell_rate as Sale_Rate,pono as Po_Details,balance,selling_type as SellingType, " +
                    " bill_to,carporatebilltoname from qrycarporatedobalance  where Doc_No=" + txtcarporateSale.Text + " and  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                DataSet ds = new DataSet();
                ds = clsDAL.SimpleQuery(qry);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];

                string sellingType = dt.Rows[0]["SellingType"].ToString();
                //string selfac = clsCommon.getString("Select SELF_AC from " + tblPrefix + "CompanyParameters   where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Session["year"].ToString() + "");
                if (sellingType == "C")
                {
                    string getvocname = clsCommon.getString("Select Ac_Name_E from qrymstaccountmaster   where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Session["SELF_AC"].ToString() + "");

                    txtGETPASS_CODE.Text = Session["SELF_AC"].ToString(); //dt.Rows[0]["Unit_name"].ToString();
                    LBLGETPASS_NAME.Text = getvocname; //dt.Rows[0]["UnitName"].ToString();
                    string partycode = dt.Rows[0]["Unit_Code"].ToString();
                    string partyname = dt.Rows[0]["Unit_name"].ToString();
                    string getpassstatecode = clsCommon.getString("select GSTStateCode from qrymstaccountmaster   where Ac_Code=" + Session["SELF_AC"].ToString() + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    string getpasstatename = "";
                    if (getpassstatecode.Trim() != string.Empty && getpassstatecode != "0")
                    {
                        getpasstatename = clsCommon.getString("select State_Name from GSTStateMaster   where State_Code=" + getpassstatecode + "");
                        txtGetpassGstStateCode.Text = getpassstatecode;
                        lbltxtGetpassGstStateName.Text = getpasstatename;
                    }

                    txtvoucher_by.Text = partycode;
                    lblvoucherbyname.Text = partyname;
                    txtSaleBillTo.Text = dt.Rows[0]["Ac_Code"].ToString();
                    txtNARRATION4.Text = dt.Rows[0]["partyName"].ToString();
                    string partystatecode = clsCommon.getString("select GSTStateCode from qrymstaccountmaster  where Ac_Code=" + partycode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    string stateName1 = "";
                    if (partystatecode.Trim() != string.Empty && partystatecode != "0")
                    {
                        stateName1 = clsCommon.getString("select State_Name from GSTStateMaster   where State_Code=" + partystatecode + "");
                        txtVoucherbyGstStateCode.Text = partystatecode;
                        lbltxtVoucherbyGstStateName.Text = stateName1;

                        txtSalebilltoGstStateCode.Text = partystatecode;
                        lbltxtSalebilltoGstStateName.Text = stateName1;
                    }
                }
                else
                {
                    string getvocname = clsCommon.getString("Select Ac_Name_E from qrymstaccountmaster  where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + Session["SELF_AC"].ToString() + "");
                    txtGETPASS_CODE.Text = Session["SELF_AC"].ToString();
                    LBLGETPASS_NAME.Text = getvocname;
                    string getpassstatecode = clsCommon.getString("select GSTStateCode from qrymstaccountmaster where   Ac_Code=" + Session["SELF_AC"].ToString() + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    string getpasstatename = "";
                    if (getpassstatecode.Trim() != string.Empty && getpassstatecode != "0")
                    {
                        getpasstatename = clsCommon.getString("select State_Name from GSTStateMaster   where State_Code=" + getpassstatecode + "");
                        txtGetpassGstStateCode.Text = getpassstatecode;
                        lbltxtGetpassGstStateName.Text = getpasstatename;
                    }
                    string partycode = dt.Rows[0]["Ac_Code"].ToString();
                    string partyname = dt.Rows[0]["partyName"].ToString();
                    hdnfPDSPartyCode.Value = partycode;
                    hdnfPDSUnitCode.Value = dt.Rows[0]["Unit_Code"].ToString();
                    lblPDSParty.Text = "Party: " + dt.Rows[0]["partyName"].ToString();
                    txtvoucher_by.Text = partycode;
                    lblvoucherbyname.Text = partyname;

                    txtSaleBillTo.Text = partycode;
                    txtNARRATION4.Text = partyname;

                    txtVoucherbyGstStateCode.Text = clsCommon.getString("select GSTStateCode from qrymstaccountmaster where   Ac_Code=" + partycode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString() + ""));
                    lbltxtVoucherbyGstStateName.Text = clsCommon.getString("select State_Name from GSTStateMaster   where State_Code=" + txtSalebilltoGstStateCode.Text + "");

                    txtSalebilltoGstStateCode.Text = txtVoucherbyGstStateCode.Text;
                    lbltxtSalebilltoGstStateName.Text = lbltxtVoucherbyGstStateName.Text;
                }


                lblPoDetails.Text = "PO Details:- " + dt.Rows[0]["Po_Details"].ToString();
                txtBroker_CODE.Text = dt.Rows[0]["BrokerCode"].ToString();
                LBLBROKER_NAME.Text = dt.Rows[0]["BrokerName"].ToString();
                if (ViewState["mode"].ToString() == "I")
                {
                    txtquantal.Text = dt.Rows[0]["balance"].ToString();
                    txtSALE_RATE.Text = dt.Rows[0]["Sale_Rate"].ToString();
                }
                txtBill_To.Text = dt.Rows[0]["bill_to"].ToString();
                lblBill_To.Text = dt.Rows[0]["carporatebilltoname"].ToString();

                drpDeliveryType.SelectedValue = "N";
                drpDeliveryType.Enabled = false;
                ddlFrieghtType.SelectedValue = "O";
                ddlFrieghtType.Enabled = false;
                drpDOType.SelectedValue = "DI";
                drpDOType.Enabled = false;
                setFocusControl(txtMILL_CODE);


                if (txtGETPASS_CODE.Text != string.Empty)
                {
                    hdnfgp.Value = clsCommon.getString("select  ifnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + txtGETPASS_CODE.Text + " and Company_code='" + Session["Company_Code"].ToString() + "'");
                }

                if (txtvoucher_by.Text != string.Empty)
                {
                    hdnfst.Value = clsCommon.getString("select  ifnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + txtvoucher_by.Text + " and Company_code='" + Session["Company_Code"].ToString() + "'");
                }

                if (txtSaleBillTo.Text != string.Empty)
                {
                    try
                    {
                        hdnfsb.Value = clsCommon.getString("select  ifnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + txtSaleBillTo.Text + " and Company_code='" + Session["Company_Code"].ToString() + "'");
                    }
                    catch { }
                }


                try
                {

                    hdnfcscode.Value = clsCommon.getString("select  ifnull(carpid,0) as carpid from carporatehead where doc_no=" + txtcarporateSale.Text + " and Company_code='" + Session["Company_Code"].ToString() + "'");
                }
                catch
                {

                }

                if (txtBroker_CODE.Text != string.Empty)
                {
                    hdnfbk.Value = clsCommon.getString("select  ifnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + txtBroker_CODE.Text + " and Company_code='" + Session["Company_Code"].ToString() + "'");
                }


                Bill_To = Convert.ToInt32(txtBill_To.Text != string.Empty ? txtBill_To.Text : "0");
                if (txtBill_To.Text != string.Empty && txtBill_To.Text != "0")
                {
                    hdnfbt.Value = clsCommon.getString("select  ifnull(accoid,0) as accoid from " + qryAccountList + " where Ac_code=" + Bill_To + " and Company_code='" + Session["Company_Code"].ToString() + "'");
                }
                else
                {
                    hdnfbt.Value = "0";
                }

                //}
                //else
                //{
                //    txtcarporateSale.Text = "";
                //    lblCSYearCode.Text = "";
                //    setFocusControl(txtcarporateSale);
                //}
            }


        }
        else
        {
            txtcarporateSale.Text = "";
            lblCSYearCode.Text = "";
            setFocusControl(txtcarporateSale);
        }
    }

    private void calculation()
    {
        try
        {
            double qt = Convert.ToDouble(Math.Abs(Convert.ToDouble(txtquantal.Text != string.Empty ? txtquantal.Text : "0.00")));
            //double qt = Convert.ToString(Math.Abs(Convert.ToDouble(txtquantal.Text))) != string.Empty ? Convert.ToDouble(txtquantal.Text) : 0.00;
            double tenderCommission = txtCommission.Text != string.Empty ? Convert.ToDouble(txtCommission.Text) : 0.00;
            double qtl = Math.Abs(qt);
            Int32 packing = Convert.ToInt32("0" + txtPACKING.Text != string.Empty ? txtPACKING.Text : "0");
            Int32 bags = 0;
            double saleRate = 0.00;
            double actualSaleRate = Convert.ToDouble("0" + txtSALE_RATE.Text != string.Empty ? txtSALE_RATE.Text : "0");
            double commision = Convert.ToDouble("0" + txtCommission.Text);
            saleRate = actualSaleRate + commision;
            double millRate = Convert.ToDouble("0" + txtmillRate.Text);
            double gstRate = Convert.ToDouble("0" + txtexcise_rate.Text);

            double mill_amount = double.Parse("0" + lblMillAmount.Text);
            double diffAmt = 0.00;
            double diff = 0.00;
            double frieght = Convert.ToDouble("0" + txtFrieght.Text);

            double vasuli_rate = Convert.ToDouble("0" + txtVasuliRate.Text);
            double frieght_amount = Convert.ToDouble("0" + txtFrieghtAmount.Text);
            double vasuli_amount = Convert.ToDouble("0" + txtVasuliAmount.Text);
            var vr1 = txtVasuliRate1.Text != string.Empty ? Convert.ToString(txtVasuliRate1.Text) : "0";
            var fmt = new NumberFormatInfo();
            fmt.NegativeSign = "-";
            var number = double.Parse(vr1, fmt);
            double vasuli_rate1 = number;// Convert.ToDouble("0" + txtVasuliRate1.Text);
            var va1 = txtVasuliAmount1.Text != string.Empty ? Convert.ToString(txtVasuliAmount1.Text) : "0";

            var va1amount = double.Parse(va1, fmt);
            double vasuli_amount1 = va1amount;//Convert.ToDouble("0" + txtVasuliAmount1.Text);
            if (qtl != 0 && packing != 0)
            {
                bags = Convert.ToInt32((qtl / packing) * 100);
                txtBAGS.Text = bags.ToString();
            }
            else
            {
                txtBAGS.Text = bags.ToString();
            }

            if (saleRate != 0 && millRate != 0)
            {
                hdnfSaleRate.Value = Convert.ToString(saleRate);
                diff = saleRate - millRate;
                diffAmt = Math.Round(diff * qtl, 2);
                mill_amount = qtl * Math.Round((millRate + gstRate), 2);

            }

            lblDiffrate.Text = diff.ToString();
            txtDIFF_AMOUNT.Text = diffAmt.ToString();
            lblMillAmount.Text = mill_amount.ToString();
            if (qtl != 0 && frieght != 0)
            {
                frieght_amount = qtl * frieght;
                txtFrieghtAmount.Text = frieght_amount.ToString();
                //txtMemoAdvance.Text = frieght_amount.ToString();
            }
            else
            {
                frieght_amount = 0.00;
                txtFrieghtAmount.Text = frieght_amount.ToString();
                //txtMemoAdvance.Text = frieght_amount.ToString();
            }

            if (qtl != 0 && vasuli_rate != 0)
            {
                vasuli_amount = qtl * vasuli_rate;
                txtVasuliAmount.Text = vasuli_amount.ToString();
            }
            else
            {
                vasuli_amount = 0.00;
                txtVasuliAmount.Text = vasuli_amount.ToString();
            }

            //if (qtl != 0 && vasuli_rate1 != 0)
            //{
            //    vasuli_amount1 = qtl * vasuli_rate1;
            //    txtVasuliAmount1.Text = vasuli_amount1.ToString();
            //}
            //else
            //{
            //    vasuli_amount1 = 0.00;
            //    txtVasuliAmount1.Text = vasuli_amount1.ToString();
            //}


            //if (qtl != 0 && vasuli_amount1 != 0)
            //{
            //    vasuli_rate1 = vasuli_amount1 / qtl;
            //    txtVasuliRate1.Text = vasuli_rate1.ToString();
            //}
            //else
            //{
            //    vasuli_rate1 = 0.00;
            //    txtVasuliRate1.Text = vasuli_rate1.ToString();
            //}


            #region ---Add default row in grid---
            if (ViewState["currentTable"] == null)
            {
                if (drpDOType.SelectedValue == "DI")
                {
                    DataTable dt1 = new DataTable();
                    dt1.Columns.Add((new DataColumn("ID", typeof(Int32))));
                    #region [Write here columns]
                    dt1.Columns.Add((new DataColumn("Type", typeof(string))));
                    dt1.Columns.Add((new DataColumn("Bank_Code", typeof(Int32))));
                    dt1.Columns.Add((new DataColumn("BankName", typeof(string))));
                    dt1.Columns.Add((new DataColumn("Narration", typeof(string))));
                    dt1.Columns.Add((new DataColumn("Amount", typeof(double))));
                    dt1.Columns.Add((new DataColumn("UTR_NO", typeof(string))));
                    dt1.Columns.Add((new DataColumn("LTNo", typeof(Int32))));
                    dt1.Columns.Add((new DataColumn("dodetailid", typeof(int))));

                    dt1.Columns.Add((new DataColumn("UtrDetailId", typeof(int))));
                    #endregion
                    dt1.Columns.Add(new DataColumn("rowAction", typeof(string)));
                    dt1.Columns.Add((new DataColumn("SrNo", typeof(int))));
                    DataRow dr = dt1.NewRow();
                    dr["ID"] = 1;
                    dr["rowAction"] = "A";
                    dr["SrNo"] = 1;
                    dr["Type"] = "T";
                    dr["dodetailid"] = 1;
                    if (txtMILL_CODE.Text != string.Empty)
                    {
                        Int32 Payment_To = Convert.ToInt32(clsCommon.getString("Select Payment_To from qrytenderheaddetail where Mill_Code=" + txtMILL_CODE.Text + " and Tender_No=" + txtPurcNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())));
                        dr["Bank_Code"] = Payment_To;
                    }
                    else
                    {
                        dr["Bank_Code"] = "0";
                    }
                    if (LBLMILL_NAME.Text != string.Empty)
                    {
                        string Payment_To_Name = clsCommon.getString("Select paymenttoname from qrytenderheaddetail where Mill_Code=" + txtMILL_CODE.Text + " and Tender_No=" + txtPurcNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                        dr["BankName"] = Payment_To_Name;
                    }
                    else
                    {
                        dr["BankName"] = "";
                    }
                    dr["Narration"] = "Transfer Letter";
                    dr["Amount"] = Math.Round(qtl * Math.Round((millRate + gstRate), 2), 2);

                    dr["UTR_NO"] = txtUTRNo.Text != string.Empty ? int.Parse(txtUTRNo.Text) : 0;


                    dr["UtrDetailId"] = 0;
                    dr["LTNo"] = txtLT_No.Text != string.Empty ? int.Parse(txtLT_No.Text) : 0;
                    dt1.Rows.Add(dr);
                    ViewState["currentTable"] = dt1;
                    grdDetail.DataSource = dt1;
                    grdDetail.DataBind();
                }
            }
            else
            {
                DataTable dt1 = (DataTable)ViewState["currentTable"];
                DataRow dr = dt1.Rows[0];

                if (temp == "0" && drpDOType.SelectedValue == "DI")
                {
                    dr["ID"] = dt1.Rows[0]["ID"].ToString();
                    //dr["rowAction"] = "U";
                    dr["SrNo"] = 1;

                    dr["Type"] = "T";
                    if (txtMILL_CODE.Text != string.Empty)
                    {
                        Int32 Payment_To = Convert.ToInt32(clsCommon.getString("Select Payment_To from qrytenderheaddetail where Mill_Code=" + txtMILL_CODE.Text + " and Tender_No=" + txtPurcNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())));
                        dr["Bank_Code"] = Payment_To;
                    }
                    else
                    {
                        dr["Bank_Code"] = "0";
                    }
                    if (LBLMILL_NAME.Text != string.Empty)
                    {
                        string Payment_To_Name = clsCommon.getString("Select paymenttoname from qrytenderheaddetail where Mill_Code=" + txtMILL_CODE.Text + " and Tender_No=" + txtPurcNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                        dr["BankName"] = Payment_To_Name;
                    }
                    else
                    {
                        dr["BankName"] = "";
                    }
                    dr["Narration"] = dt1.Rows[0]["Narration"].ToString();
                    dr["LTNo"] = dt1.Rows[0]["LTNo"].ToString();
                    double millAmount = Math.Round(qtl * Math.Round((millRate + gstRate), 2), 2);

                    if (dt1.Rows.Count == 1)
                    {
                        if (ViewState["mode"].ToString() == "I" || dt1.Rows[0]["UTR_NO"].ToString() == "0")
                        {
                            dt1.Rows[0]["Amount"] = millAmount;
                            if (ViewState["mode"].ToString() == "I")
                            {
                                dt1.Rows[0]["rowAction"] = "A";
                            }
                            else
                            {
                                dt1.Rows[0]["rowAction"] = "U";
                            }

                        }
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        if (dt1.Rows[0]["UTR_NO"].ToString() != "0")
                        {
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                if (dt1.Rows[i]["rowAction"].ToString() != "D")
                                {
                                    if (millAmount > Convert.ToDouble(dt1.Rows[i]["Amount"].ToString()))
                                    {
                                        millAmount = millAmount - Convert.ToDouble(dt1.Rows[i]["Amount"].ToString());
                                    }
                                    else
                                    {
                                        if (i < dt1.Rows.Count)
                                        {
                                            dt1.Rows[i]["Amount"] = millAmount;
                                            dt1.Rows[i]["rowAction"] = "U";

                                            for (int k = i; k < dt1.Rows.Count - 1; k++)
                                            {
                                                dt1.Rows[k + 1]["rowAction"] = "D";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    #region comment
                    //dr["Amount"] = millAmount;
                    //string UTR_NO = dt1.Rows[0]["UTR_NO"].ToString();
                    //hdnfMainBankAmount.Value = millAmount.ToString();
                    //dr["UTR_NO"] = UTR_NO;
                    //if (dt1.Rows.Count > 1)
                    //{
                    //    dt1.Rows[0]["rowAction"] = "U";

                    //    for (int i = 0; i < dt1.Rows.Count; i++)
                    //    {
                    //        //if (i == 0)
                    //        //{
                    //        //    dt1.Rows[i + 1]["rowAction"] = "D";
                    //        //    //dt1.Rows[i + 1].Delete();
                    //        //    //dt1.AcceptChanges();
                    //        //}
                    //    }
                    //}
                    //if (ViewState["mode"].ToString() == "U")
                    //{
                    //    double thisSum = Convert.ToDouble(clsCommon.getString("select ISNULL(SUM(UsedAmt),0) from " + tblPrefix + "qryUTRBalance where doc_no=" + UTR_NO + " and mill_code=" + txtMILL_CODE.Text + " and DO_No=" + txtdoc_no.Text.Trim() + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                    //    double utrSum = Convert.ToDouble(clsCommon.getString("select ISNULL(SUM(balance),0) from " + tblPrefix + "qryUTRBalance where doc_no=" + UTR_NO + " and mill_code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ""));
                    //    double utrBal = thisSum + utrSum;
                    //    if (millAmount > utrBal)
                    //    {
                    //        dt1.Rows[0]["Amount"] = utrBal;
                    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Mill Amount Is Greater You Need To Add Another Utr!');", true);
                    //    }
                    //}
                    #endregion

                    ViewState["currentTable"] = dt1;
                    grdDetail.DataSource = dt1;
                    grdDetail.DataBind();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
            #endregion
    }
    #endregion

    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        {
            string tenderno = txtPurcNo.Text;
            string ccMail = clsCommon.getString("Select Email_Id from qrymstaccountmaster where  Ac_Code='" + txtMILL_CODE.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            //if (!string.IsNullOrWhiteSpace(ccMail))
            //{
            //    ccMail = "," + ccMail;
            //}
            //string millEmail = txtMillEmailID.Text + ccMail;
            string do_no = lbldoid.Text;
            if (do_no != string.Empty)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:DOParty('" + do_no + "','" + txtMILL_CODE.Text + "','" + tenderno + "')", true);
                //Response.Redirect("../Report/rptDO.aspx?do_no=" + do_no + "&email=" + millEmail, true);
            }
        }
        catch
        {

        }
    }

    protected void txtFrieght_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtFrieght.Text;
        strTextBox = "txtFrieght";
        csCalculations();
        calculation();
        if (ViewState["mode"].ToString() == "I")
        {
            txtMemoAdvance.Text = "0.00";
            txtMemoAdvanceRate.Text = "0.00";
        }
        //double frieght_amount = txtFrieghtAmount.Text != string.Empty ? Convert.ToDouble(txtFrieghtAmount.Text) : 0.00;
    }

    protected void txtVasuliRate_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtVasuliRate.Text;
        strTextBox = "txtVasuliRate";
        csCalculations();
        calculation();
    }

    protected void txtVasuliAmount_TextChanged(object sender, EventArgs e)
    {
        if (txtquantal.Text != "")
        {
            calculation();
            setFocusControl(txtVasuliRate1);
        }
        else
        {
            setFocusControl(txtquantal);
        }
    }

    protected void txtVasuliAmount1_TextChanged(object sender, EventArgs e)
    {
        if (txtquantal.Text != "")
        {
            double qntl = Convert.ToDouble(txtquantal.Text);
            var va1 = txtVasuliAmount1.Text != string.Empty ? Convert.ToString(txtVasuliAmount1.Text) : "0";
            var fmt = new NumberFormatInfo();
            fmt.NegativeSign = "-";
            var va1amount = double.Parse(va1, fmt);
            double vasuli_amount1 = va1amount;
            txtVasuliRate1.Text = Convert.ToDouble(vasuli_amount1 / qntl).ToString();

            calculation();
            setFocusControl(txtVasuliAc);
        }
        else
        {
            setFocusControl(txtquantal);
        }
    }

    protected void txtMemoAdvance_TextChanged(object sender, EventArgs e)
    {
        // searchString = txtMemoAdvance.Text;
        strTextBox = "txtMemoAdvance";
        double qntl = Convert.ToDouble(txtquantal.Text);
        double memoadvane = Convert.ToDouble(txtMemoAdvance.Text);
        double rate = Math.Round((memoadvane / qntl), 2);
        txtMemoAdvanceRate.Text = rate.ToString();
        setFocusControl(txtVasuliRate);
    }

    protected void drpDeliveryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        setFocusControl(txtGETPASS_CODE);
        if (drpDeliveryType.SelectedValue == "C")
        {
            ddlFrieghtType.SelectedValue = "P";
        }
        else
        {
            ddlFrieghtType.SelectedValue = "O";
        }
    }

    //protected void btnAddNewAccount_Click(object sender, EventArgs e)
    //{

    //}

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void txtDriverMobile_TextChanged(object sender, EventArgs e)
    {
        //  searchString = txtDriverMobile.Text;
        strTextBox = "txtDriverMobile";
        csCalculations();
    }

    protected void btnOurDO_Click(object sender, EventArgs e)
    {
        try
        {
            string tenderno = txtPurcNo.Text;
            string ccMail = clsCommon.getString("Select Email_Id from qrymstaccountmaster where Ac_Code='" + txtMILL_CODE.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            //if (!string.IsNullOrWhiteSpace(ccMail))
            //{
            //    ccMail = "," + ccMail;
            //}
            //string millEmail = txtMillEmailID.Text + ccMail;
            string do_no = lbldoid.Text;
            string a = "0";
            if (chkNoprintondo.Checked == true)
            {
                a = "1";
            }
            if (hdconfirm.Value == "Yes")
            {
                if (do_no != string.Empty)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:od('" + do_no + "','" + txtMILL_CODE.Text + "','O','" + a + "','" + tenderno + "')", true);
                    //Response.Redirect("../Report/rptDO.aspx?do_no=" + do_no + "&email=" + millEmail, true);
                }
            }
            else
            {
                if (do_no != string.Empty)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:od1('" + do_no + "','" + txtMILL_CODE.Text + "','O','" + a + "','" + tenderno + "')", true);
                    //Response.Redirect("../Report/rptDO.aspx?do_no=" + do_no + "&email=" + millEmail, true);
                }
            }
        }
        catch
        {

        }
    }

    protected void btnDeliveryChallan_Click(object sender, EventArgs e)
    {
        try
        {
            string ccMail = clsCommon.getString("Select Email_Id_cc from qrymstAccountmaster where Ac_Code='" + txtMILL_CODE.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (!string.IsNullOrWhiteSpace(ccMail))
            {
                ccMail = "," + ccMail;
            }
            string millEmail = txtMillEmailID.Text + ccMail;
            string do_no = lbldoid.Text;
            string a = "0";
            if (chkNoprintondo.Checked == true)
            {
                a = "1";
            }

            //if (txtcarporateSale.Text != string.Empty || txtcarporateSale.Text!="0")
            //{
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kys", "javascript:DC('" + do_no + "','" + millEmail + "','O','" + a + "')", true);
            //Response.Redirect("../Report/rptDO.aspx?do_no=" + do_no + "&email=" + millEmail, true);
            //}
        }
        catch
        {

        }
    }

    protected void txtFrieghtAmount_TextChanged(object sender, EventArgs e)
    {

    }

    protected void txtINVOICE_NO_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(btnOpenDetailsPopup);
    }

    protected void txtVasuliRate1_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtVasuliRate1.Text;
        strTextBox = "txtVasuliRate1";

        VasuliAmountCalculation();

        csCalculations();
        calculation();
    }

    private void VasuliAmountCalculation()
    {
        double qntl = Convert.ToDouble(txtquantal.Text);
        var vr1 = txtVasuliRate1.Text != string.Empty ? Convert.ToString(txtVasuliRate1.Text) : "0";
        var fmt = new NumberFormatInfo();
        fmt.NegativeSign = "-";
        var number = double.Parse(vr1, fmt);
        double vasuli_rate1 = number;
        txtVasuliAmount1.Text = Convert.ToString(vasuli_rate1 * qntl);
    }

    protected void btnSendSms_Click(object sender, EventArgs e)
    {
        try
        {
            string driverMobile = txtDriverMobile.Text;
            string mobile = txtMillMobile.Text;
            string Cst_noC = clsCommon.getString("Select Cst_no from " + tblPrefix + "AccountMaster where Ac_Code=" + txtGETPASS_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string Tin_NoC = clsCommon.getString("Select Tin_No from " + tblPrefix + "AccountMaster where Ac_Code=" + txtGETPASS_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string ECC_NoC = clsCommon.getString("Select ECC_No from " + tblPrefix + "AccountMaster where Ac_Code=" + txtGETPASS_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string millshort = clsCommon.getString("Select Short_Name from " + tblPrefix + "AccountMaster where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string msg = millshort + " DO.No:" + txtdoc_no.Text + " " + LBLGETPASS_NAME.Text + " TIN:" + Tin_NoC + " ECC No:" + ECC_NoC + " CST:" + Cst_noC + " Qntl:" + txtquantal.Text + " Mill Rate:" + txtmillRate.Text + " Lorry:" + txtTruck_NO.Text + " " + txtNARRATION1.Text;
            string API = clsGV.msgAPI;
            string Url = API + "mobile=" + mobile + "&message=" + msg + "&senderid=NAVKAR&accusage=1";
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(Url);
            HttpWebResponse resp = (HttpWebResponse)myReq.GetResponse();
            StreamReader reder = new StreamReader(resp.GetResponseStream());
            string respString = reder.ReadToEnd();
            reder.Close();
            resp.Close();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Message Successfully Sent!');", true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void txtPartyCommission_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //double SR = Convert.ToDouble(hdnfSaleRate.Value.TrimStart());
            //double MR = Convert.ToDouble(txtmillRate.Text);
            //double CR = Convert.ToDouble(txtPartyCommission.Text);
            //txtSALE_RATE.Text = Convert.ToString(MR + CR);
            //double qtl = Convert.ToDouble(txtquantal.Text);
            //double diff = 0.00;
            //double diffAmt = 0.00;
            //diff = SR - MR;
            //diffAmt = Math.Round(diff * qtl, 2);
            //calculation();
            setFocusControl(txtSALE_RATE);
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void txtMemoAdvanceRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            MemoadvanceCalculation();
            setFocusControl(txtVasuliRate);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void MemoadvanceCalculation()
    {
        double rate = txtMemoAdvanceRate.Text != string.Empty ? Convert.ToDouble(txtMemoAdvanceRate.Text) : 0;
        double qntl = txtquantal.Text != string.Empty ? Convert.ToDouble(txtquantal.Text) : 0.0;
        txtMemoAdvance.Text = Convert.ToString(Math.Round((rate * qntl), 2));

        double frtRate = txtFrieght.Text != string.Empty ? Convert.ToDouble(txtFrieght.Text.ToString()) : 0.0;
        double remaingRate = frtRate - rate;
        lblFrieghtToPay.Text = "Frieght To Pay : " + (Math.Round((remaingRate * qntl), 2)).ToString();
    }

    protected void btnVoucherOtherAmounts_Click(object sender, EventArgs e)
    {
        pnlVoucherEntries.Style["display"] = "block";
        setFocusControl(txtVoucherBrokrage);
    }

    protected void txtVoucherBrokrage_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtVoucherServiceCharge);
    }

    protected void txtVoucherServiceCharge_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtVoucherL_Rate_Diff);
    }

    protected void txtVoucherL_Rate_Diff_TextChanged(object sender, EventArgs e)
    {
        try
        {
            double voucratediff = Convert.ToDouble(txtVoucherL_Rate_Diff.Text);
            double quintal = Convert.ToDouble(txtquantal.Text);
            double ratediffamt = voucratediff * quintal;
            txtVoucherRATEDIFFAmt.Text = ratediffamt.ToString();
            setFocusControl(txtVoucherCommission_Rate);
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void txtVoucherCommission_Rate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            double bankcommrate = Convert.ToDouble(txtVoucherCommission_Rate.Text);
            double quintal = Convert.ToDouble(txtquantal.Text);
            double commamt = bankcommrate * quintal;
            txtVoucherBANK_COMMISSIONAmt.Text = commamt.ToString();
            setFocusControl(txtVoucherInterest);
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void txtVoucherInterest_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtVoucherTransport_Amount);
    }

    protected void txtVoucherTransport_Amount_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtVoucherOTHER_Expenses);
    }

    protected void txtVoucherOTHER_Expenses_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(btnOk);
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        pnlVoucherEntries.Style["display"] = "none";
        setFocusControl(txtDO_CODE);
    }

    #region [getMaxCodeofvouchers]
    private void getvoucherscode(string tblName, string objCode, string trantype, string tblColumnType)
    {
        try
        {
            DataSet ds = null;
            string docno = "0";
            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                if (trantype == "NULL")
                {
                    obj.tableName = tblName + " where  company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                }
                else
                {
                    obj.tableName = tblName + " where  " + tblColumnType + "='" + trantype + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                }
                obj.code = objCode;

                ds = new DataSet();
                ds = obj.getMaxCode();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ViewState["mode"] != null)
                            {
                                if (ViewState["mode"].ToString() == "I")
                                {
                                    docno = ds.Tables[0].Rows[0][0].ToString();
                                    ViewState["maxval"] = docno;
                                }
                            }
                        }
                    }
                }
            }
            //return docno;
        }
        catch
        {
        }
    }
    #endregion

    #region [getMaxCodeofvouchers]
    private void getvoucherscode1(string tblName, string objCode, string trantype, string tblColumnType)
    {
        try
        {
            DataSet ds = null;
            string docno = "0";
            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                if (trantype == "NULL")
                {
                    obj.tableName = tblName + " where  company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                }
                else
                {
                    obj.tableName = tblName + " where  " + tblColumnType + "='" + trantype + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                }
                obj.code = objCode;

                ds = new DataSet();
                ds = obj.getMaxCode();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //if (ViewState["mode"] != null)
                            //{
                            if (ViewState["mode"].ToString() == "U")
                            {
                                docno = ds.Tables[0].Rows[0][0].ToString();
                                ViewState["maxval"] = docno;
                            }
                            // }
                        }
                    }
                }
            }
            //return docno;
        }
        catch
        {
        }
    }
    #endregion

    static string returnNumber(string docno)
    {
        return docno;
    }

    //protected void btnPrintITCVoc_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        string vtype = clsCommon.getString("select voucher_type from " + qryCommon + " where doc_no=" + txtdoc_no.Text + " and tran_type='" + trnType + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
    //        if (vtype != "PS")
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kysdsd", "javascript:ITCV('" + lblVoucherNo.Text + "');", true);
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    protected void lnkMemo_Click(object sender, EventArgs e)
    {
        Session["MEMO_NO"] = lblMemoNo.Text;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjs", "javascript:memo();", true);
    }

    protected void lnkVoucOrPurchase_Click(object sender, EventArgs e)
    {
        string vocno = lblVoucherNo.Text;
        string vocType = lblVoucherType.Text;
        if (vocType == "PS")
        {
            vocno = clsCommon.getString("select purchaseid from qrypurchasehead where doc_no=" + vocno + " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + "");
            Session["PURC_NO"] = vocno;
            int Action = 1;
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjsd", "javascript:sugarpurchase();", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "asdas1", "javascript:sugarpurchase('" + Action + "','" + vocno + "')", true);
        }
        if (vocType == "LV")
        {
            vocno = clsCommon.getString("select commissionid from qrycommissionbill where doc_no=" + vocno + " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + "");
            int Action = 1;
            //Session["LV_NO"] = vocno;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjsd", "javascript:LocalVoucher('" + Action + "','" + vocno + "');", true);
        }
        if (vocType == "OV")
        {
            Session["VOUC_NO"] = vocno;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjsd", "javascript:loadingvoucher();", true);
        }
    }

    protected void lblsbnol_Click(object sender, EventArgs e)
    {
        Int32 sbno = lblSB_No.Text != string.Empty ? Convert.ToInt32(lblSB_No.Text) : 0;
        if (sbno != 0)
        {
            sbno = Convert.ToInt32(clsCommon.getString("select saleid from qrysalehead where doc_no=" + sbno + " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + ""));
            Session["SB_NO"] = sbno;
            int Action = 1;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kjsdsd", "javascript:salebill('" + Action + "','" + sbno + "');", true);
        }
    }

    protected void txtEditDoc_No_TextChanged(object sender, EventArgs e)
    {
        try
        {
            bool a = clsCommon.isStringIsNumeric(txtEditDoc_No.Text);

            if (a == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Enter Only Numbers!')", true);
            }
            else
            {
                int do_id = Convert.ToInt32(clsCommon.getString("select doid from nt_1_deliveryorder where doc_no=" + txtEditDoc_No.Text +
                    " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" +
                    " " + Session["year"].ToString() + ""));

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:DoOPen('" + do_id + "')", true);

            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnClosePopup_Click(object sender, EventArgs e)
    {
        pnlSendSMS.Style["display"] = "none";
        setFocusControl(btnAdd);
    }

    protected void txtCommission_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtTruck_NO);
        calculation();
    }

    #region [txtVasuliAc_TextChanged]
    protected void txtVasuliAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtVasuliAc.Text;
        strTextBox = "txtVasuliAc";
        csCalculations();
    }
    #endregion

    #region [btntxtVasuliAc_Click]
    protected void btntxtVasuliAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtVasuliAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    protected void txtVoucherbyGstStateCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtVoucherbyGstStateCode.Text;
        strTextBox = "txtVoucherbyGstStateCode";
        csCalculations();
    }

    protected void btntxtVoucherbyGstStateCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtVoucherbyGstStateCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void txtGetpassGstStateCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGetpassGstStateCode.Text;
        strTextBox = "txtGetpassGstStateCode";
        csCalculations();
    }

    protected void btntxtGetpassGstStateCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGetpassGstStateCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void txtSalebilltoGstStateCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSalebilltoGstStateCode.Text;
        strTextBox = "txtSalebilltoGstStateCode";
        csCalculations();
    }

    protected void btntxtSalebilltoGstStateCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtSalebilltoGstStateCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void txtTransportGstStateCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTransportGstStateCode.Text;
        strTextBox = "txtTransportGstStateCode";
        csCalculations();
    }

    protected void btntxtTransportGstStateCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtTransportGstStateCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void txtMillGstStateCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMillGstStateCode.Text;
        strTextBox = "txtMillGstStateCode";
        csCalculations();
    }

    protected void btntxtMillGstStateCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtMillGstStateCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void txtDistance_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(btnSave);
    }

    protected void chkEWayBill_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEWayBill.Checked == true)
        {
            txtEWayBill_No.Text = txtMillEwayBill_No.Text;
            lblchkEWayBill.Text = LBLMILL_NAME.Text;

        }
        else
        {
            lblchkEWayBill.Text = "";
            txtEWayBill_No.Text = string.Empty;

        }

    }
    #region Generate Next Number
    private void NextNumber()
    {
        try
        {
            int counts = 0;
            //counts = Convert.ToInt32(clsCommon.getString("select ifnull(count(doc_no),0) as doc_no from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "'"));
            //if (counts == 0)
            //{
            //    txtdoc_no.Text = "1";
            //    DOC_NO = 1;

            //}
            //else
            //{
            //DOC_NO = Convert.ToInt32(clsCommon.getString("SELECT max(carpid) as carpid from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "'")) + 1;
            DOC_NO = Convert.ToInt32(clsCommon.getString("SELECT ifnull(max(doc_no),0) as doc_no from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' and Year_Code='" + Session["year"].ToString() + "'")) + 1;
            if (DOC_NO == 0)
            {
                txtdoc_no.Text = "1";
                DOC_NO = 1;
                txtdoc_no.Text = DOC_NO.ToString();
            }
            else
            {
                txtdoc_no.Text = DOC_NO.ToString();
            }
            // }

            doid = Convert.ToInt32(clsCommon.getString("SELECT ifnull(max(doid),0) as doid from " + tblHead)) + 1;
            if (doid == 0)
            {
                lbldoid.Text = "1";
            }
            else
            {
                lbldoid.Text = doid.ToString();
            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Next Record Error!')", true);
        }
    }
    #endregion
    #region DataStore
    //private int DataStore(int flag)
    //{
    //    int count = 0;
    //    try
    //    {
    //        //Connection open
    //        if (con.State == ConnectionState.Open)
    //        {
    //            con.Close();
    //        }
    //        con.Open();
    //        ///Execution
    //        myTran = con.BeginTransaction();
    //        //cmd.CommandText = qry;
    //        //cmd.Connection = con;
    //        //cmd.Transaction = myTran;
    //        if (flag == 1)
    //        {

    //            cmd = new MySqlCommand(Head_Insert, con, myTran);
    //            cmd.ExecuteNonQuery();
    //            if (Detail_Insert != "")
    //            {
    //                cmd = new MySqlCommand(Detail_Insert, con, myTran);
    //                cmd.ExecuteNonQuery();
    //            }
    //            if (Head_Update != "")
    //            {
    //                cmd = new MySqlCommand(Head_Update, con, myTran);
    //                cmd.ExecuteNonQuery();
    //            }

    //            myTran.Commit();
    //            Thread.Sleep(100);

    //            count = 1;
    //        }
    //        else if (flag == 2)
    //        {
    //            if (Head_Update != "")
    //            {
    //                cmd = new MySqlCommand(Head_Update, con, myTran);
    //                cmd.ExecuteNonQuery();
    //            }
    //            if (Detail_Update != "")
    //            {
    //                cmd = new MySqlCommand(Detail_Update, con, myTran);
    //                cmd.ExecuteNonQuery();
    //            }
    //            if (Detail_Insert != "")
    //            {
    //                cmd = new MySqlCommand(Detail_Insert, con, myTran);
    //                cmd.ExecuteNonQuery();
    //            }
    //            if (Detail_Delete != "")
    //            {
    //                cmd = new MySqlCommand(Detail_Delete, con, myTran);
    //                cmd.ExecuteNonQuery();
    //            }
    //            myTran.Commit();
    //            Thread.Sleep(100);
    //            count = 2;
    //        }
    //        else
    //        {
    //            cmd = new MySqlCommand(Detail_Delete, con, myTran);
    //            cmd.ExecuteNonQuery();
    //            cmd = new MySqlCommand(Head_Delete, con, myTran);
    //            cmd.ExecuteNonQuery();
    //            myTran.Commit();
    //            Thread.Sleep(100);
    //            count = 3;
    //        }

    //        return count;
    //    }
    //    catch
    //    {
    //        if (myTran != null)
    //        {
    //            myTran.Rollback();
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('check Entry AND Try Again !')", true);

    //        }
    //        return count;

    //    }
    //    finally
    //    {
    //        con.Close();
    //    }

    //}
    #endregion

    #region PurchasePosting
    private void PurchasePosting()
    {
        try
        {
            #region Purchase Posting
            purchase = new PurchaseFields();
            #region Find Max
            int counts = 0;
            if (btnSave.Text == "Save")
            {
                counts = Convert.ToInt32(clsCommon.getString("select ifnull(count(doc_no),0) as doc_no from nt_1_sugarpurchase where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                    " and Year_Code='" + Session["year"].ToString() + "'"));
                if (counts == 0)
                {
                    purchase.PS_doc_no = 1;
                }
                else
                {
                    purchase.PS_doc_no = Convert.ToInt32(clsCommon.getString("SELECT max(doc_no) as doc_no from nt_1_sugarpurchase where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                        " and Year_Code='" + Session["year"].ToString() + "'")) + 1;
                }
                counts = Convert.ToInt32(clsCommon.getString("SELECT ifnull(count(purchaseid),0) as purchaseid from nt_1_sugarpurchase "));
                if (counts == 0)
                {
                    purchase.PS_purchase_Id = 1;

                }
                else
                {
                    purchase.PS_purchase_Id = Convert.ToInt32(clsCommon.getString("SELECT max(purchaseid) as purchaseid from nt_1_sugarpurchase")) + 1;

                }
            }
            else
            {


                if (lblVoucherNo.Text == "0" || lblVoucherNo.Text == string.Empty)
                {
                    counts = Convert.ToInt32(clsCommon.getString("select ifnull(count(doc_no),0) as doc_no from nt_1_sugarpurchase where Company_Code='" + Session["Company_Code"].ToString() + "' " +
               " and Year_Code='" + Session["year"].ToString() + "'"));
                    if (counts == 0)
                    {
                        purchase.PS_doc_no = 1;
                    }
                    else
                    {
                        purchase.PS_doc_no = Convert.ToInt32(clsCommon.getString("SELECT max(doc_no) as doc_no from nt_1_sugarpurchase where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                            " and Year_Code='" + Session["year"].ToString() + "'")) + 1;
                    }
                    counts = Convert.ToInt32(clsCommon.getString("SELECT ifnull(count(purchaseid),0) as purchaseid from nt_1_sugarpurchasedetails "));
                    if (counts == 0)
                    {
                        purchase.PS_purchase_Id = 1;

                    }
                    else
                    {
                        purchase.PS_purchase_Id = Convert.ToInt32(clsCommon.getString("SELECT max(purchaseid) as purchaseid from nt_1_sugarpurchasedetails")) + 1;

                    }
                }
                else
                {
                    purchase.PS_doc_no = Convert.ToInt32(lblVoucherNo.Text);
                    purchase.PS_purchase_Id = Convert.ToInt32(clsCommon.getString("SELECT purchaseid from nt_1_sugarpurchase where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                             " and Year_Code='" + Session["year"].ToString() + "' and doc_no='" + lblVoucherNo.Text + "'"));
                }

            }
            #endregion

            gstSateCodeForPurchaseBill = 0;

            PaymentTo = Convert.ToInt32(clsCommon.getString("Select ifnull(Payment_To,0) as paymentto from NT_1_Tender where Tender_No=" + purc_no + " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + ""));
            gstSateCodeForPurchaseBill = MillGstStateCode;
            //int selfact = Convert.ToInt32(Session["SELF_AC"].ToString());
            //CGSTRATE = Convert.ToDouble(clsCommon.getString("select CGST from NT_1_GSTRateMaster where Doc_no=" + GSTRateCode + " "));
            //SGSTRATE = Convert.ToDouble(clsCommon.getString("select SGST from NT_1_GSTRateMaster where Doc_no=" + GSTRateCode + " "));
            //IGSTRATE = Convert.ToDouble(clsCommon.getString("select IGST from NT_1_GSTRateMaster where Doc_no=" + GSTRateCode + " "));
            //GSTRATE = Convert.ToDouble(clsCommon.getString("select Rate from NT_1_GSTRateMaster where Doc_no=" + GSTRateCode + " "));

            if (Convert.ToInt32(MILL_CODE) != PaymentTo)
            {
                gstSateCodeForPurchaseBill = Convert.ToInt32(clsCommon.getString("select ifnull(GSTStateCode,0) as code from qrymstaccountmaster where Ac_Code=" + PaymentTo + " " +
                    " and Company_Code=" + Session["Company_Code"].ToString() + " "));
            }


            if (AUTO_VOUCHER == "YES")
            {
                if (drpDOType.SelectedValue == "DI")
                {
                    if (GETPASS_CODE == SELFAC.ToString() || PDS == "P")
                    {
                        #region GST Calculation
                        CompanyStateCode = Convert.ToInt32(Session["CompanyGSTStateCode"].ToString());
                        if (CompanyStateCode == gstSateCodeForPurchaseBill)
                        {
                            MILLAMOUNT = (mill_rate * QUANTAL) + 0;
                            CGSTtaxAmountOnMR = Math.Round(MILLAMOUNT * cgstrate / 100, 2);
                            CGST_AMOUNT = CGSTtaxAmountOnMR;
                            SGSTtaxAmountOnMR = Math.Round(MILLAMOUNT * sgstrate / 100, 2);
                            SGST_AMOUNT = SGSTtaxAmountOnMR;
                            // IGSTRATE = 0;
                            IGST_AMOUNT = 0.00;
                        }
                        else
                        {
                            MILLAMOUNT = (mill_rate * QUANTAL) + 0;
                            IGSTtaxAmountOnMR = Math.Round(MILLAMOUNT * igstrate / 100, 2);
                            IGST_AMOUNT = IGSTtaxAmountOnMR;
                            CGST_AMOUNT = 0.00;
                            // CGSTRATE = 0;
                            SGST_AMOUNT = 0.00;
                            // SGSTRATE = 0;
                        }
                        #endregion
                        TOTALPurchase_Amount = Math.Round(QUANTAL * mill_rate + CGST_AMOUNT + SGST_AMOUNT + IGST_AMOUNT, 2);
                        ITEM_AMOUNT = QUANTAL * mill_rate;

                        VOUCHER_NO = Convert.ToInt32(lblVoucherNo.Text != string.Empty ? lblVoucherNo.Text : "0");
                        if (VOUCHER_NO > 0)
                        {
                            maxcountpsno = VOUCHER_NO;
                        }
                        else
                        {
                            maxcountpsno = 0;
                        }


                        // millCityCode = Convert.ToInt32(clsCommon.getString("select ifnull(city_code,0) as citycode from NT_1_AccountMaster where Ac_Code=" + MILL_CODE + " "));
                        fromPlace = clsCommon.getString("select cityname from qrymstaccountmaster where  Ac_Code=" + MILL_CODE + " and Company_Code='" + Session["Company_Code"].ToString() + "'");
                        // getPassCityCode = Convert.ToInt32(clsCommon.getString("select ifnull(city_code,0) as citycode from NT_1_AccountMaster where Ac_Code=" + GETPASS_CODE + " "));
                        toPlace = clsCommon.getString("select cityname from qrymstaccountmaster where  Ac_Code=" + GETPASS_CODE + " and Company_Code='" + Session["Company_Code"].ToString() + "'");

                        #region Assign Values To Purchase Common Class Fields
                        purchase.PS_Tran_Type = "PS";
                        purchase.PS_Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
                        purchase.PS_Year_Code = Convert.ToInt32(Session["year"].ToString());
                        purchase.PS_Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
                        purchase.PS_PURCNO = Convert.ToInt32(txtdoc_no.Text);
                        purchase.PS_Purcid = Convert.ToInt32(lbldoid.Text);
                        purchase.PS_doc_date = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                        purchase.PS_Ac_Code = Convert.ToInt32(PaymentTo);
                        purchase.PS_mill_code = Convert.ToInt32(MILL_CODE);
                        purchase.PS_GstRateCode = GSTRateCode;
                        purchase.PS_FROM_STATION = fromPlace;
                        purchase.PS_TO_STATION = toPlace;
                        purchase.PS_mill_inv_date = DateTime.Parse(txtMillInv_Date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                        purchase.PS_LORRYNO = TRUCK_NO;
                        purchase.PS_BROKER = BROKER_CODE;
                        purchase.PS_subTotal = (QUANTAL * mill_rate) + 0;
                        purchase.PS_LESS_FRT_RATE = 0.00;
                        purchase.PS_freight = 0.00;
                        purchase.PS_cash_advance = 0.00;
                        purchase.PS_bank_commission = 0.00;
                        purchase.PS_OTHER_AMT = 0.00;
                        purchase.PS_Grade = txtGRADE.Text;
                        purchase.PS_Bill_Amount = TOTALPurchase_Amount;
                        purchase.PS_Due_Days = 1;
                        purchase.PS_NETQNTL = QUANTAL + 0;
                        purchase.PS_Bill_No = txtMillInvoiceno.Text;
                        purchase.PS_CGSTRate = cgstrate + 0;
                        purchase.PS_CGSTAmount = CGST_AMOUNT + 0;
                        purchase.PS_IGSTRate = igstrate + 0;
                        purchase.PS_IGSTAmount = IGST_AMOUNT + 0;
                        purchase.PS_SGSTRate = sgstrate + 0;
                        purchase.PS_SGSTAmount = SGST_AMOUNT + 0;
                        purchase.PS_EWay_Bill_No = MillEwayBill;
                        if (GETPASS_CODE == SELFAC.ToString() && SaleBillTo == SELFAC.ToString())
                        {
                            purchase.PS_SelfBal = "Y";
                        }
                        else
                        {
                            purchase.PS_SelfBal = "N";
                        }
                        purchase.PS_Created_By = Session["user"].ToString();
                        purchase.PS_Modified_By = Session["user"].ToString();
                        purchase.PS_ac = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0)as ccid from qrymstaccountmaster where Ac_Code=" + PaymentTo + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
                        try
                        {
                            purchase.PS_bk = Convert.ToInt32(bk);
                        }
                        catch
                        {
                        }
                        try
                        {
                            purchase.PS_mc = Convert.ToInt32(mc);
                        }
                        catch
                        {
                        }
                        #endregion
                        dt1 = new DataTable();
                        DataRow dr = null;
                        dt1.Columns.Add((new DataColumn("ID", typeof(Int32))));

                        #region [Write here columns]
                        dt1.Columns.Add((new DataColumn("item_code", typeof(Int32))));
                        dt1.Columns.Add((new DataColumn("narration", typeof(string))));
                        dt1.Columns.Add((new DataColumn("Quantal", typeof(double))));
                        dt1.Columns.Add((new DataColumn("packing", typeof(Int32))));
                        dt1.Columns.Add((new DataColumn("bags", typeof(Int32))));
                        dt1.Columns.Add((new DataColumn("rate", typeof(double))));
                        dt1.Columns.Add((new DataColumn("item_Amount", typeof(double))));
                        dt1.Columns.Add((new DataColumn("purchasedetailid", typeof(int))));
                        #endregion
                        dt1.Columns.Add(new DataColumn("rowAction", typeof(string)));
                        dt1.Columns.Add((new DataColumn("SrNo", typeof(int))));
                        dr = dt1.NewRow();
                        dr["ID"] = 1;

                        dr["SrNo"] = 0;

                        #region [ Set values to dr]
                        dr["item_code"] = itemcode;
                        dr["narration"] = string.Empty;
                        dr["Quantal"] = QUANTAL;
                        dr["packing"] = PACKING;
                        dr["bags"] = BAGS;
                        dr["rate"] = mill_rate;
                        dr["item_Amount"] = ITEM_AMOUNT;
                        if (btnSave.Text == "Save")
                        {
                            dr["rowAction"] = "A";
                            dr["purchasedetailid"] = 0;
                        }
                        else
                        {
                            if (lblVoucherNo.Text == string.Empty || lblVoucherNo.Text == "0")
                            {
                                dr["rowAction"] = "A";
                                dr["purchasedetailid"] = 0;
                            }
                            else
                            {
                                dr["rowAction"] = "U";
                                //string purcid = clsCommon.getString("select purchaseid from nt_1_sugarpurchase where  doc_no='" + lblVoucherNo.Text + "' and Company_Code=" + Session["Company_Code"].ToString() + " " +
                                //    " and Year_Code=" + Session["year"].ToString() + "");

                                dr["purchasedetailid"] = Convert.ToInt32(clsCommon.getString("select purchasedetailid from nt_1_sugarpurchasedetails where  doc_no='" + lblVoucherNo.Text + "' " +
                                    " and  Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + ""));
                            }
                        }
                        dt1.Rows.Add(dr);

                        #endregion

                    }
                    else
                    {
                        #region LV
                        #endregion
                    }
                }

            }
            #endregion
        }
        catch
        {
        }
    }
    #endregion

    #region SalePosting
    private void SalePosting()
    {
        try
        {
            #region Sale Posting

            salePosting = new SaleFields();
            #region find max
            int counts = 0;
            salePosting.SB_FreightPaid_Amount = Convert.ToDouble(txtVasuliAmount.Text != string.Empty ? txtVasuliAmount.Text : "0");

            if (btnSave.Text == "Save")
            {
                counts = Convert.ToInt32(clsCommon.getString("select count(doc_no) as doc_no from nt_1_sugarsale where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                    " and Year_Code='" + Session["year"].ToString() + "'"));
                if (counts == 0)
                {
                    salePosting.SB_doc_no = 1;

                }
                else
                {
                    salePosting.SB_doc_no = Convert.ToInt32(clsCommon.getString("SELECT max(doc_no) as doc_no from nt_1_sugarsale where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                        " and Year_Code='" + Session["year"].ToString() + "'")) + 1;

                }
                counts = Convert.ToInt32(clsCommon.getString("SELECT count(saleid) as saleid from nt_1_sugarsale "));
                if (counts == 0)
                {
                    salePosting.SB_Sale_Id = 1;

                }
                else
                {
                    salePosting.SB_Sale_Id = Convert.ToInt32(clsCommon.getString("SELECT max(saleid) as saleid from nt_1_sugarsale")) + 1;

                }
            }
            else
            {

                int sb = Convert.ToInt32(lblSB_No.Text != string.Empty ? lblSB_No.Text : "0");
                if (sb == 0)
                {
                    counts = Convert.ToInt32(clsCommon.getString("select count(doc_no) as doc_no from nt_1_sugarsale where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                " and Year_Code='" + Session["year"].ToString() + "'"));
                    if (counts == 0)
                    {
                        salePosting.SB_doc_no = 1;

                    }
                    else
                    {
                        salePosting.SB_doc_no = Convert.ToInt32(clsCommon.getString("SELECT max(doc_no) as doc_no from nt_1_sugarsale where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                            " and Year_Code='" + Session["year"].ToString() + "'")) + 1;

                    }

                    counts = Convert.ToInt32(clsCommon.getString("SELECT count(saleid) as saleid from nt_1_sugarsale "));
                    if (counts == 0)
                    {
                        salePosting.SB_Sale_Id = 1;

                    }
                    else
                    {
                        salePosting.SB_Sale_Id = Convert.ToInt32(clsCommon.getString("SELECT max(saleid) as saleid from nt_1_sugarsale")) + 1;

                    }

                }
                else
                {

                    salePosting.SB_doc_no = Convert.ToInt32(lblSB_No.Text);
                    salePosting.SB_Sale_Id = Convert.ToInt32(clsCommon.getString("SELECT saleid  from nt_1_sugarsale where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                             " and Year_Code='" + Session["year"].ToString() + "' and doc_no='" + lblSB_No.Text + "'"));
                }


            }
            #endregion

            #region
            Int32 pdsparty = hdnfPDSPartyCode.Value != string.Empty ? Convert.ToInt32(hdnfPDSPartyCode.Value) : 0;
            Int32 pdsunit = hdnfPDSUnitCode.Value != string.Empty ? Convert.ToInt32(hdnfPDSUnitCode.Value) : 0;

            string PDS1 = string.Empty;
            int saleBillcityCode = 0;
            string saleBilltoCity = string.Empty;

            if (Carporate_Sale_No != null && Carporate_Sale_No != 0)
            {
                PDS1 = clsCommon.getString("select selling_type from carporatehead where Doc_No='" + Carporate_Sale_No + "' and Company_Code='"
                    + Session["Company_Code"].ToString() + "' ");

            }

            try
            {
                saleBilltoCity = clsCommon.getString("select cityname  from qrymstaccountmaster where Ac_Code='" + SaleBillTo + "' and Company_Code='" + Session["Company_Code"].ToString() + "'");
            }
            catch { }

            try
            {
                fromPlace = clsCommon.getString("select cityname  from qrymstaccountmaster where Ac_Code='" + MILL_CODE + "' and Company_Code='" + Session["Company_Code"].ToString() + "'");
            }
            catch { }


            CompanyStateCode = Convert.ToInt32(Session["CompanyGSTStateCode"].ToString());

            LessFriegthRateForSaleBill = MM_Rate + VASULI_RATE_1;
            LessFriegthAmountForSaleBill = MEMO_ADVANCE + VASULI_AMOUNT_1;

            string UnitCity = "";
            if (Carporate_Sale_No != null && Carporate_Sale_No != 0)
            {
                saleparty = Convert.ToInt32(clsCommon.getString("select ifnull(ac_code,0) as ac_code from qrycarporateheaddetail where Doc_No='" + Carporate_Sale_No
                    + "' and Company_Code=" + Session["Company_Code"].ToString() + " "));


                pdsunitSaleBill = Convert.ToInt32(clsCommon.getString("select ifnull(unit_code,0) as Unit from qrycarporateheaddetail where Doc_No='" + Carporate_Sale_No +
                    "' and Company_Code=" + Session["Company_Code"].ToString() + " "));

                UnitCity = clsCommon.getString("select CityName from qrymstaccountmaster where Ac_Code='" + pdsunitSaleBill + "' and Company_Code=" + Session["Company_Code"].ToString() + " ");
            }

            double saleRate = SALE_RATE + Tender_Commission + 0;
            double subTOTAL = saleRate + QUANTAL + 0;

            double salerateFor_naka = 0.00;
            double SalerateFor_Salebill = 0.00;
            double gstSalerateAndAdvance = 0.00;

            double taxableAmount = (saleRate * QUANTAL) + FRIEGHT_AMOUNT;
            int cSale = txtcarporateSale.Text.Trim() != string.Empty ? Convert.ToInt32(txtcarporateSale.Text) : 0;
            if (Delivery_Type == "C")
            {
                // TaxableAmountForSB = Math.Round((saleRate * QUANTAL) + MEMO_ADVANCE );
                TaxableAmountForSB = Math.Round((saleRate * QUANTAL) + MEMO_ADVANCE + VASULI_AMOUNT_1);
            }
            else
            {

                if (cSale == 0)
                {
                    salerateFor_naka = (saleRate - FRIEGHT_RATE + MM_Rate);
                    //double SaleRateForNaka = (SALE_RATE - FRIEGHT_RATE);
                    SaleRateForSB = Math.Round((((salerateFor_naka / (salerateFor_naka + (salerateFor_naka * GSTRate / 100))) * salerateFor_naka)), 2);
                    TaxableAmountForSB = Math.Round((SaleRateForSB * QUANTAL), 2);

                    //TaxableAmountForSB = Math.Round((SaleRateForSB * QUANTAL) + MEMO_ADVANCE);
                }
                else
                {
                    SaleRateForNaka = (saleRate - FRIEGHT_RATE + MM_Rate);
                    //double SaleRateForNaka = (SALE_RATE - FRIEGHT_RATE);
                    SaleRateForSB = (saleRate - FRIEGHT_RATE + MM_Rate); ;// Math.Round((((SaleRateForNaka / (SaleRateForNaka + (SaleRateForNaka * GSTRate / 100))) * SaleRateForNaka)), 2);
                    TaxableAmountForSB = Math.Round((SaleRateForSB * QUANTAL), 2);

                    //TaxableAmountForSB = Math.Round((SaleRateForSB * QUANTAL) + MEMO_ADVANCE);
                }



            }

            double SaleBillAmt = 0.00;
            if (SaleBillTo != "0" || saleparty != 0)
            {

                if (pdsparty != 0 && txtcarporateSale.Text != string.Empty)
                {
                    int pdspartystatecode = Convert.ToInt32(clsCommon.getString("select IfNULL(GSTStateCode,0) from qrymstaccountmaster  where Ac_Code=" + pdsparty + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
                    //SaleBillTo = pdsunit.ToString();
                    if (pdspartystatecode != 0)
                    {
                        if (CompanyStateCode == pdspartystatecode)
                        {
                            CGSTRateForSB = cgstrate;
                            double saleamount = saleRate * QUANTAL;
                            double cgsttaxAmountOnSR = Math.Round((TaxableAmountForSB * cgstrate / 100), 2);
                            //double cgstExSaleRate = Math.Round(Math.Abs((SALE_RATE / (SALE_RATE + cgsttaxAmountOnSR) * SALE_RATE)), 2);
                            //double cgstRateAmountOnSR = Math.Round(Math.Abs(SALE_RATE - cgstExSaleRate), 2);
                            CGSTAmountForSB = Math.Round(cgsttaxAmountOnSR, 2);

                            SGSTRateForSB = sgstrate;
                            double sgsttaxAmountOnSR = Math.Round((TaxableAmountForSB * sgstrate / 100), 2);
                            //double sgstExSaleRate = Math.Round(Math.Abs((SALE_RATE / (SALE_RATE + sgsttaxAmountOnSR) * SALE_RATE)), 2);
                            //double sgstRateAmountOnSR = Math.Round(Math.Abs(SALE_RATE - sgstExSaleRate), 2);
                            SGSTAmountForSB = Math.Round(sgsttaxAmountOnSR, 2);
                        }
                        else
                        {
                            IGSTRateForSB = igstrate;
                            double igsttaxAmountOnSR = (TaxableAmountForSB * igstrate / 100);
                            //double igstExSaleRate = Math.Round(Math.Abs((SaleRateForSB / (SaleRateForSB + igsttaxAmountOnSR) * SALE_RATE)), 2);
                            //double igstRateAmountOnSR = Math.Round(Math.Abs(SaleRateForSB - igstExSaleRate), 2);
                            IGSTAmountForSB = Math.Round(igsttaxAmountOnSR, 2);
                        }
                    }
                    else
                    {
                        //isValidated = false;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Add State Code to Pds Party!');", true);
                        setFocusControl(txtMILL_CODE);
                        return;
                    }
                }
                else
                {
                    if (Carporate_Sale_No != 0)
                    {
                        if (PDS == "C")
                        {
                            string csbilltoname = clsCommon.getString("Select bill_to from carporatehead where Doc_No=" + Carporate_Sale_No
                                + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            int csbilltoStateCode = Convert.ToInt32(clsCommon.getString("select IfNULL(GSTStateCode,0) from qrymstaccountmaster  where Ac_Code=" + csbilltoname +
                                " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));


                            if (CompanyStateCode == csbilltoStateCode)
                            {
                                CGSTRateForSB = cgstrate;
                                double saleamount = saleRate * QUANTAL;
                                double cgsttaxAmountOnSR = Math.Round((TaxableAmountForSB * cgstrate / 100), 2);
                                //double cgstExSaleRate = Math.Round(Math.Abs((SALE_RATE / (SALE_RATE + cgsttaxAmountOnSR) * SALE_RATE)), 2);
                                //double cgstRateAmountOnSR = Math.Round(Math.Abs(SALE_RATE - cgstExSaleRate), 2);
                                CGSTAmountForSB = Math.Round(cgsttaxAmountOnSR, 2);

                                SGSTRateForSB = sgstrate;
                                double sgsttaxAmountOnSR = Math.Round((TaxableAmountForSB * sgstrate / 100), 2);
                                //double sgstExSaleRate = Math.Round(Math.Abs((SALE_RATE / (SALE_RATE + sgsttaxAmountOnSR) * SALE_RATE)), 2);
                                //double sgstRateAmountOnSR = Math.Round(Math.Abs(SALE_RATE - sgstExSaleRate), 2);
                                SGSTAmountForSB = Math.Round(sgsttaxAmountOnSR, 2);
                            }
                            else
                            {
                                IGSTRateForSB = igstrate;
                                double igsttaxAmountOnSR = (TaxableAmountForSB * igstrate / 100);
                                //double igstExSaleRate = Math.Round(Math.Abs((SaleRateForSB / (SaleRateForSB + igsttaxAmountOnSR) * SALE_RATE)), 2);
                                //double igstRateAmountOnSR = Math.Round(Math.Abs(SaleRateForSB - igstExSaleRate), 2);
                                IGSTAmountForSB = Math.Round(igsttaxAmountOnSR, 2);
                            }
                        }
                        else
                        {
                            if (CompanyStateCode == SalebilltoGstStateCode)
                            {
                                CGSTRateForSB = cgstrate;
                                double saleamount = saleRate * QUANTAL;
                                double cgsttaxAmountOnSR = Math.Round((TaxableAmountForSB * cgstrate / 100), 2);
                                //double cgstExSaleRate = Math.Round(Math.Abs((SALE_RATE / (SALE_RATE + cgsttaxAmountOnSR) * SALE_RATE)), 2);
                                //double cgstRateAmountOnSR = Math.Round(Math.Abs(SALE_RATE - cgstExSaleRate), 2);
                                CGSTAmountForSB = Math.Round(cgsttaxAmountOnSR, 2);

                                SGSTRateForSB = sgstrate;
                                double sgsttaxAmountOnSR = Math.Round((TaxableAmountForSB * sgstrate / 100), 2);
                                //double sgstExSaleRate = Math.Round(Math.Abs((SALE_RATE / (SALE_RATE + sgsttaxAmountOnSR) * SALE_RATE)), 2);
                                //double sgstRateAmountOnSR = Math.Round(Math.Abs(SALE_RATE - sgstExSaleRate), 2);
                                SGSTAmountForSB = Math.Round(sgsttaxAmountOnSR, 2);
                            }
                            else
                            {
                                IGSTRateForSB = igstrate;
                                double igsttaxAmountOnSR = (TaxableAmountForSB * igstrate / 100);
                                //double igstExSaleRate = Math.Round(Math.Abs((SaleRateForSB / (SaleRateForSB + igsttaxAmountOnSR) * SALE_RATE)), 2);
                                //double igstRateAmountOnSR = Math.Round(Math.Abs(SaleRateForSB - igstExSaleRate), 2);
                                IGSTAmountForSB = Math.Round(igsttaxAmountOnSR, 2);
                            }
                        }
                    }
                    else
                    {
                        if (CompanyStateCode == SalebilltoGstStateCode)
                        {
                            CGSTRateForSB = cgstrate;
                            double saleamount = saleRate * QUANTAL;
                            double cgsttaxAmountOnSR = Math.Round((TaxableAmountForSB * cgstrate / 100), 2);
                            //double cgstExSaleRate = Math.Round(Math.Abs((SALE_RATE / (SALE_RATE + cgsttaxAmountOnSR) * SALE_RATE)), 2);
                            //double cgstRateAmountOnSR = Math.Round(Math.Abs(SALE_RATE - cgstExSaleRate), 2);
                            CGSTAmountForSB = Math.Round(cgsttaxAmountOnSR, 2);

                            SGSTRateForSB = sgstrate;
                            double sgsttaxAmountOnSR = Math.Round((TaxableAmountForSB * sgstrate / 100), 2);
                            //double sgstExSaleRate = Math.Round(Math.Abs((SALE_RATE / (SALE_RATE + sgsttaxAmountOnSR) * SALE_RATE)), 2);
                            //double sgstRateAmountOnSR = Math.Round(Math.Abs(SALE_RATE - sgstExSaleRate), 2);
                            SGSTAmountForSB = Math.Round(sgsttaxAmountOnSR, 2);
                        }
                        else
                        {
                            IGSTRateForSB = igstrate;
                            double igsttaxAmountOnSR = (TaxableAmountForSB * igstrate / 100);
                            //double igstExSaleRate = Math.Round(Math.Abs((SaleRateForSB / (SaleRateForSB + igsttaxAmountOnSR) * SALE_RATE)), 2);
                            //double igstRateAmountOnSR = Math.Round(Math.Abs(SaleRateForSB - igstExSaleRate), 2);
                            IGSTAmountForSB = Math.Round(igsttaxAmountOnSR, 2);
                        }
                    }
                }
                #region
                double TotalGstSaleBillAmount = Math.Round(TaxableAmountForSB + CGSTAmountForSB + SGSTAmountForSB + IGSTAmountForSB);
                double Roundoff = 0.00;
                int maxcountSBno = 0;
                int SB_NO = Convert.ToInt32(lblSB_No.Text != string.Empty ? lblSB_No.Text : "0");
                if (SB_NO > 0)
                {
                    maxcountSBno = SB_NO;
                }
                else
                {
                    maxcountSBno = 0;
                }

                if (PDS1 == "P")
                {
                    if (drpDeliveryType.SelectedValue == "C")
                    {
                        Roundoff = Math.Round(TotalGstSaleBillAmount - (TaxableAmountForSB + CGSTAmountForSB + SGSTAmountForSB + IGSTAmountForSB), 2);
                    }
                    else
                    {
                        Roundoff = Math.Round(TotalGstSaleBillAmount - (TaxableAmountForSB + CGSTAmountForSB + SGSTAmountForSB + IGSTAmountForSB), 2);
                        //Math.Round(TotalGstSaleAmount - (TaxableAmountForSB + CGSTAmountForSB + SGSTAmountForSB
                        //                              + IGSTAmountForSB), 2)
                    }
                }
                else
                {
                    if (drpDeliveryType.SelectedValue == "C")
                    {
                        Roundoff = Math.Round(TotalGstSaleBillAmount - (TaxableAmountForSB + CGSTAmountForSB + SGSTAmountForSB + IGSTAmountForSB), 2);
                    }
                    else
                    {
                        Roundoff = Math.Round(TotalGstSaleBillAmount - (TaxableAmountForSB + CGSTAmountForSB + SGSTAmountForSB + IGSTAmountForSB), 2);
                    }
                }
                #endregion


                #region Assign Values To Sale Common Class
                salePosting.SB_PURCNO = Convert.ToInt32(purchase.PS_doc_no != null ? purchase.PS_doc_no : 0);
                salePosting.SB_doc_date = DOC_DATE;
                salePosting.SB_DoNarrtion = txtNARRATION3.Text;
                if (PDS1 == "P")
                {
                    salePosting.SB_Ac_Code = Convert.ToInt32(hdnfPDSPartyCode.Value);
                    salePosting.SB_Unit_Code = Convert.ToInt32(hdnfPDSUnitCode.Value);
                    salePosting.SB_FROM_STATION = fromPlace;
                    salePosting.SB_TO_STATION = UnitCity;
                    if (drpDeliveryType.SelectedValue == "C")
                    {
                        salePosting.SB_subTotal = QUANTAL * saleRate + 0;
                    }
                    else
                    {
                        salePosting.SB_subTotal = QUANTAL * SaleRateForSB + 0;

                    }
                }
                else
                {
                    salePosting.SB_Ac_Code = Convert.ToInt32(txtSaleBillTo.Text);
                    salePosting.SB_Unit_Code = Convert.ToInt32(VOUCHER_BY);

                    salePosting.SB_FROM_STATION = fromPlace;
                    salePosting.SB_TO_STATION = saleBilltoCity;
                    if (drpDeliveryType.SelectedValue == "C")
                    {
                        salePosting.SB_subTotal = QUANTAL * saleRate + 0;
                    }
                    else
                    {
                        salePosting.SB_subTotal = QUANTAL * SaleRateForSB + 0;

                    }
                }

                salePosting.SB_LORRYNO = TRUCK_NO;
                salePosting.SB_mill_code = Convert.ToInt32(MILL_CODE);
                salePosting.SB_BROKER = BROKER_CODE;

                if (drpDeliveryType.SelectedValue == "C")
                {
                    //salePosting.SB_LESS_FRT_RATE = Convert.ToDouble(txtMemoAdvanceRate.Text != string.Empty ? txtMemoAdvanceRate.Text : "0.00");
                    //salePosting.SB_freight = MEMO_ADVANCE;

                    salePosting.SB_LESS_FRT_RATE = Convert.ToDouble(txtMemoAdvanceRate.Text != string.Empty ? txtMemoAdvanceRate.Text : "0.00") + VASULI_RATE_1;

                    salePosting.SB_freight = MEMO_ADVANCE + VASULI_AMOUNT_1;
                }
                else
                {
                    salePosting.SB_LESS_FRT_RATE = Convert.ToDouble(txtMemoAdvanceRate.Text != string.Empty ? txtMemoAdvanceRate.Text : "0.00") + VASULI_RATE_1;

                    salePosting.SB_freight = MEMO_ADVANCE + VASULI_AMOUNT_1;
                }

                salePosting.SB_cash_advance = 0.00;
                salePosting.SB_bank_commission = 0.00;
                salePosting.SB_OTHER_AMT = 0.00;
                salePosting.SB_Bill_Amount = TotalGstSaleBillAmount;
                salePosting.SB_Due_Days = 0;
                salePosting.SB_NETQNTL = QUANTAL;
                salePosting.SB_Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
                salePosting.SB_Year_Code = Convert.ToInt32(Session["year"].ToString());
                salePosting.SB_Branch_Code = Branch_Code;
                salePosting.SB_Created_By = Session["user"].ToString();
                salePosting.SB_Modified_By = Session["user"].ToString();
                salePosting.SB_Tran_Type = "SB";
                salePosting.SB_DONO = DOC_NO;

                salePosting.SB_TRANSPORT_CODE = TRANSPORT_CODE;
                salePosting.SB_RateDiff = 0.00;
                salePosting.SB_GstRateCode = GSTRateCode;
                salePosting.SB_CGSTRate = cgstrate;
                salePosting.SB_CGSTAmount = CGSTAmountForSB;
                salePosting.SB_SGSTRate = sgstrate;
                salePosting.SB_SGSTAmount = SGSTAmountForSB;
                salePosting.SB_IGSTRate = igstrate;
                salePosting.SB_IGSTAmount = IGSTAmountForSB;
                salePosting.SB_TAXABLEAMOUNT = TaxableAmountForSB;
                salePosting.SB_EWay_BillChk = EWay_BillChk;
                salePosting.SB_EwayBill_No = EWayBill_No;
                salePosting.SB_MillInvoiceno = MillInvoiceno;
                salePosting.SB_Roundoff = Roundoff;
                salePosting.SB_Purcid = Convert.ToInt32(purchase.PS_purchase_Id != null ? purchase.PS_purchase_Id : 0);
                try
                {
                    salePosting.SB_ac = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + salePosting.SB_Ac_Code + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
                }
                catch
                {

                }
                try
                {
                    salePosting.SB_uc = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + salePosting.SB_Unit_Code + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
                }
                catch
                {

                }
                try
                {
                    salePosting.SB_bk = Convert.ToInt32(bk);
                }
                catch
                {
                }
                try
                {
                    salePosting.SB_tc = Convert.ToInt32(tc);
                }
                catch
                {
                }
                try
                {
                    salePosting.SB_mc = Convert.ToInt32(mc);
                }
                catch
                {
                }
                #endregion


                dt2 = new DataTable();
                DataRow dr = null;
                dt2.Columns.Add((new DataColumn("ID", typeof(Int32))));
                #region [Write here columns]
                dt2.Columns.Add((new DataColumn("item_code", typeof(Int32))));
                //dt.Columns.Add((new DataColumn("item_name", typeof(string))));
                dt2.Columns.Add((new DataColumn("narration", typeof(string))));
                dt2.Columns.Add((new DataColumn("Quantal", typeof(double))));
                dt2.Columns.Add((new DataColumn("packing", typeof(Int32))));
                dt2.Columns.Add((new DataColumn("bags", typeof(Int32))));
                dt2.Columns.Add((new DataColumn("rate", typeof(double))));
                dt2.Columns.Add((new DataColumn("item_Amount", typeof(double))));
                dt2.Columns.Add((new DataColumn("saledetailid", typeof(int))));
                #endregion
                dt2.Columns.Add(new DataColumn("rowAction", typeof(string)));
                dt2.Columns.Add((new DataColumn("SrNo", typeof(int))));
                dr = dt2.NewRow();
                dr["ID"] = 1;

                dr["SrNo"] = 0;

                #region [ Set values to dr]
                dr["item_code"] = itemcode;
                //dr["item_name"] = LBLITEMNAME.Text;
                dr["narration"] = string.Empty;
                dr["Quantal"] = QUANTAL;
                dr["packing"] = PACKING;
                dr["bags"] = BAGS;




                if (drpDeliveryType.SelectedValue == "C")
                {
                    dr["rate"] = saleRate;
                    dr["item_Amount"] = QUANTAL * saleRate + 0;
                }
                else
                {
                    dr["rate"] = SaleRateForSB;
                    dr["item_Amount"] = QUANTAL * SaleRateForSB + 0;


                }



                if (btnSave.Text == "Save")
                {
                    dr["rowAction"] = "A";
                    dr["saledetailid"] = 1;
                }
                else
                {
                    if (lblSB_No.Text == string.Empty || lblSB_No.Text == "" || lblSB_No.Text == "0")
                    {
                        dr["rowAction"] = "A";
                        dr["saledetailid"] = 1;
                    }
                    else
                    {

                        dr["rowAction"] = "U";
                        //string saleid = clsCommon.getString("select saleid from nt_1_sugarsale where  doc_no='" + lblSB_No.Text + "' " +
                        //     " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + "");

                        dr["saledetailid"] = Convert.ToInt32(clsCommon.getString("select saledetailid from nt_1_sugarsaledetails where  doc_no='" + lblSB_No.Text + "' " +
                            " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + ""));
                    }
                }
                dt2.Rows.Add(dr);
                #endregion
            }

            #endregion

            #endregion
        }
        catch
        {
        }
    }
    #endregion

    #region LV_Posting
    private void LV_Posting()
    {
        LV = new LocalVoucher();

        #region max
        int counts = 0;
        if (btnSave.Text == "Save")
        {
            counts = Convert.ToInt32(clsCommon.getString("select ifnull(count(doc_no),0) as doc_no from commission_bill where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                " and Year_Code='" + Session["year"].ToString() + "'"));
            if (counts == 0)
            {
                LV.LV_Doc_No = 1;
                voucher_no = 1;

            }
            else
            {
                LV.LV_Doc_No = Convert.ToInt32(clsCommon.getString("SELECT max(doc_no) as doc_no from commission_bill where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                       " and Year_Code='" + Session["year"].ToString() + "'")) + 1;
                voucher_no = LV.LV_Doc_No;
            }
            counts = Convert.ToInt32(clsCommon.getString("SELECT count(commissionid) as commissionid from commission_bill "));
            if (counts == 0)
            {
                LV.LV_commissionid = 1;

            }
            else
            {
                LV.LV_commissionid = Convert.ToInt32(clsCommon.getString("SELECT max(commissionid) as commissionid from commission_bill")) + 1;

            }
        }
        else
        {
            LV.LV_Doc_No = Convert.ToInt32(lblVoucherNo.Text);
            voucher_no = LV.LV_Doc_No;
            LV.LV_commissionid = Convert.ToInt32(clsCommon.getString("SELECT commissionid  from qrycommissionbill where doc_no=" + lblVoucherNo.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " " +
                " and Year_Code=" + Session["year"].ToString() + ""));
        }
        #endregion



        if (DIFF_AMOUNT != 0)
        {

            GSTRateForLV = Convert.ToDouble(GSTRate);
            cgstrateForLV1 = Convert.ToDouble(cgstrate);
            sgstrateForLV1 = Convert.ToDouble(sgstrate);
            igstrateForLV1 = Convert.ToDouble(igstrate);

            //double CGSTAmountForLV = 0.0;
            //double SGSTAmountForLV = 0.0;
            //double IGSTAmountForLV = 0.0;

            //double CGSTRateForLV = 0.00;
            //double SGSTRateForLV = 0.00;
            //double IGSTRateForLV = 0.00;

            if (DESP_TYPE == "DO")
            {
                LvAmnt = DIFF_AMOUNT;
                //if (Tender_Commission != 0)
                //{
                //    //LvAmnt += QUANTAL * Tender_Commission;

                //}

                if (LvAmnt != 0)
                {
                    int CompanyGSTStateCode = 0;
                    if (Session["CompanyGSTStateCode"].ToString() == null || Session["CompanyGSTStateCode"].ToString() == string.Empty)
                    {
                        CompanyGSTStateCode = Convert.ToInt32(clsCommon.getString("select GSTStateCode from NT_1_CompanyParameters where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                    " and Year_Code='" + Session["year"].ToString() + "'"));

                        Session["CompanyGSTStateCode"] = CompanyGSTStateCode.ToString();
                        // int CompanyGSTStateCode = Convert.ToInt32(Session["CompanyGSTStateCode"].ToString());
                    }
                    else
                    {
                        CompanyGSTStateCode = Convert.ToInt32(Session["CompanyGSTStateCode"].ToString());
                    }
                    if (VoucherbyGstStateCode == CompanyGSTStateCode)
                    {
                        CGSTRateForLV = cgstrateForLV1;
                        CGSTAmountForLV = Math.Round(LvAmnt * CGSTRateForLV / 100, 2);

                        SGSTRateForLV = sgstrateForLV1;
                        SGSTAmountForLV = Math.Round(LvAmnt * SGSTRateForLV / 100, 2);
                    }
                    else
                    {
                        IGSTRateForLV = igstrateForLV1;
                        IGSTAmountForLV = Math.Round(LvAmnt * IGSTRateForLV / 100, 2);
                    }
                }
            }


            voucherAmountForLV = Math.Round((LvAmnt + CGSTAmountForLV + SGSTAmountForLV + IGSTAmountForLV), 2);

            SaleCGSTAc = Convert.ToInt32(Session["SaleCGSTAc"].ToString());
            SaleSGSTAc = Convert.ToInt32(Session["SaleSGSTAc"].ToString());
            SaleIGSTAc = Convert.ToInt32(Session["SaleIGSTAc"].ToString());


            PayableCGSTAc = Convert.ToInt32(Session["PurchaseCGSTAc"].ToString());
            PayableSGSTAc = Convert.ToInt32(Session["PurchaseSGSTAc"].ToString());
            PayableIGSTAc = Convert.ToInt32(Session["PurchaseIGSTAc"].ToString());

            string LVNumber = Convert.ToString(voucher_no);
            string voucherByShortName = clsCommon.getString("Select Short_Name from qrymstaccountmaster Where Company_Code="
                                          + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + VOUCHER_BY + "");
            string brokerShortName = string.Empty;
            if (BROKER_CODE != 2)
            {
                brokerShortName = clsCommon.getString("Select Short_Name from  qrymstaccountmaster Where Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + BROKER_CODE + "");
            }

            LV.LV_ac = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtvoucher_by.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
            LV.LV_uc = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtvoucher_by.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
            LV.LV_tc = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtTRANSPORT_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
            LV.LV_bc = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtBroker_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
            LV.LV_mc = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where ac_code=" + txtMILL_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));

            LV.LV_Doc_No = Convert.ToInt32(LVNumber);
            LV.LV_Doc_Date = DOC_DATE;
            LV.LV_Link_No = 0;
            LV.LV_link_Type = "";
            LV.LV_Link_id = 0;
            LV.LV_Ac_Code = Convert.ToInt32(Ac_Code);
            LV.LV_Unit_Code = Convert.ToInt32(GETPASS_CODE);
            LV.LV_Broker_CODE = BROKER_CODE;
            LV.LV_Quantal = QUANTAL;
            LV.LV_PACKING = PACKING;
            LV.LV_BAGS = BAGS;
            LV.LV_Grade = GRADE;
            LV.LV_Transport_Code = TRANSPORT_CODE;
            LV.LV_Mill_Rate = Convert.ToDouble(txtmillRate.Text);
            LV.LV_Sale_Rate = Convert.ToDouble(txtSALE_RATE.Text);
            LV.LV_Purchase_Rate = 0;
            LV.LV_FREIGHT = 0.00;
            LV.LV_Narration1 = "V.No " + vouchnarration + " " + voucherByShortName + " " + brokerShortName + "";
            LV.LV_Narration2 = "" + myNarration2 + " Lorry No:" + TRUCK_NO + "";
            LV.LV_Narration3 = myNarration3;
            LV.LV_Narration4 = "" + myNarration4 + " " + TRUCK_NO + "";
            LV.LV_Voucher_Amount = DIFF_AMOUNT;
            LV.LV_Diff_Amount = DIFF_RATE;
            LV.LV_Company_Code = Company_Code;
            LV.LV_Year_Code = Year_Code;
            LV.LV_Branch_Code = Branch_Code;
            LV.LV_Created_By = "" + Session["user"].ToString() + "";
            LV.LV_Commission_Rate = Tender_Commission;
            LV.LV_Resale_Commisson = Tender_Commission_Amount;
            LV.LV_GstRateCode = Convert.ToInt32(txtGstRate.Text);
            LV.LV_CGSTRate = CGSTRateForLV;
            LV.LV_CGSTAmount = CGSTAmountForLV;
            LV.LV_SGSTRate = SGSTRateForLV;
            LV.LV_SGSTAmount = SGSTAmountForLV;
            LV.LV_IGSTRate = IGSTRateForLV;
            LV.LV_IGSTAmount = IGSTAmountForLV;
            LV.LV_TaxableAmount = DIFF_AMOUNT;
        }
    }
    #endregion
    protected void txtitem_Code_TextChanged(object sender, EventArgs e)
    {
        searchString = txtitem_Code.Text;
        strTextBox = "txtitem_Code";
        csCalculations();
    }
    protected void btntxtitem_Code_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtitem_Code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void txtBill_To_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBill_To.Text;
        strTextBox = "txtBill_To";
        csCalculations();
    }
    protected void btntxtbill_To_Click(object sender, EventArgs e)
    {
        try
        {
            //if (txtBroker.Text != string.Empty)
            //{
            lblMsg.Text = "";
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtBill_To";
            btnSearch_Click(sender, e);
            //}
        }
        catch
        {
        }
    }
    protected void btnPrintMotorMemo_Click(object sender, EventArgs e)
    {
        string memono = lbldoid.Text;
        if (lblMemoNo.Text != string.Empty && lblMemoNo.Text != "0")
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:MM('" + memono + "')", true);
        }
    }
    protected void btnPrintSaleBill_Click(object sender, EventArgs e)
    {

        string billto = txtBill_To.Text != string.Empty ? txtBill_To.Text : "0";
        string saleid = clsCommon.getString("select saleid from qrysalehead where doc_no='" + lblSB_No.Text + "' and Company_Code='" + Session["Company_code"].ToString() + "' " +
            " and Year_Code='" + Session["year"].ToString() + "'");

        if (hdconfirm.Value == "Yes")
        {

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:SB('" + saleid + "','" + billto + "')", true);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:SB1('" + saleid + "','" + billto + "')", true);
        }
        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:SB('" + saleid + "','" + billto + "')", true);

    }
    protected void txtMillInv_Date_TextChanged(object sender, EventArgs e)
    {

    }


    protected void btngenratesalebill_Click(object sender, EventArgs e)
    {
        btnSave.Text = "Update";
        hdnfgeneratesalebill.Value = "Yes";
        qry = "select doc_no from nt_1_sugarsale where DO_No=" + txtdoc_no.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + "";
        string sbno = clsCommon.getString(qry);
        qry = "update nt_1_deliveryorder set SB_No=" + sbno + " where doid=" + hdnf.Value + "";

        dr = null;
        dr = Maindt.NewRow();
        dr["Querys"] = qry;
        Maindt.Rows.Add(dr);
        flag = 2;
        msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);

        if (msg == "Update")
        {
            hdnf.Value = lbldoid.Text;
            clsButtonNavigation.enableDisable("S");
            this.makeEmptyForm("S");
            qry = getDisplayQuery();
            this.fetchRecord(qry);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update !')", true);
        }

        //}
    }
    protected void btnpendingsale_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:pendingSB()", true);
    }

    #region[Database's simple Query for store procedure]
    public static DataSet returndata(string GSTCode, string storeprocedurename, string millcode, string getpasscode, string shipto, string salebillto, string billto, string transportcode, string DO, string broker, string vasualiac, string itemcode, string companycode)
    {
        try
        {
            Stopwatch timer = new Stopwatch();
            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
            MySqlConnection sql_conn = new MySqlConnection(conn);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = sql_conn;
            cmd.CommandText = storeprocedurename;
            cmd.CommandType = CommandType.StoredProcedure;
            string para_name = "GSTcode";


            //an out parameter


            //an in parameter

            cmd.Parameters.AddWithValue("GSTcode", GSTCode);
            cmd.Parameters[para_name].Direction = ParameterDirection.Input;


            cmd.Parameters.AddWithValue("millcode", millcode);
            cmd.Parameters[para_name].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("getpasscode", getpasscode);
            cmd.Parameters[para_name].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("shipto", shipto);
            cmd.Parameters[para_name].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("salebillto", salebillto);
            cmd.Parameters[para_name].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("billto", billto);
            cmd.Parameters[para_name].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("transport", transportcode);
            cmd.Parameters[para_name].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("Docode", DO);
            cmd.Parameters[para_name].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("broker", broker);
            cmd.Parameters[para_name].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("vasuliac", vasualiac);
            cmd.Parameters[para_name].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("itemcode", itemcode);
            cmd.Parameters[para_name].Direction = ParameterDirection.Input;
            cmd.Parameters.AddWithValue("companycode", companycode);
            cmd.Parameters[para_name].Direction = ParameterDirection.Input;
            sql_conn.Open();

            //  MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            timer.Start();
            MySqlDataAdapter _adapter = new MySqlDataAdapter(cmd);
            ds = new DataSet();
            _adapter.Fill(ds);
            timer.Stop();
            TimeSpan ts = timer.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                           ts.Hours, ts.Minutes, ts.Seconds,
                           ts.Milliseconds / 10);
            // timerset += elapsedTime;
            //string timerset = "";
            // timerset += "Time For " + i + " Time=" + elapsedTime;

            storeproceduertimmer = elapsedTime;
            return ds;
            // ds.Load(rdr);
            //   string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
            //   MySqlConnection conn = new MySqlConnection(connStr);
            //   MySqlDataAdapter da = new MySqlDataAdapter("gstreturn", conn); // Using a Store Procedure.
            //   da.SelectCommand.CommandType = CommandType.StoredProcedure; // Comment if using hard coded query.
            //   DataSet ds = new DataSet(); // Definition: Memory representation of the database.
            //   da.SelectCommand.Parameters.AddWithValue("@GSTcode", 1);
            //   //da.SelectCommand.Parameters("").Direction = ParameterDirection.Input// Repeat for each parameter present in the Store Procedure.
            //  // da.SelectCommand.Parameters.AddWithValue("@GSTcode",1).Direction = ParameterDirection.Input;

            ////   da.SelectCommand.Parameters.AddWithValue("@CGSTRate", "");
            //  // da.SelectCommand.Parameters.AddWithValue("@CGSTRate", 1).Direction = ParameterDirection.Output;

            //   da.Fill(ds);
            //   DataTable dt = ds.Tables[1];
            //if (dt.rows.count > 1)
            //{

            //}
            // string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
            // MySqlConnection conn = new MySqlConnection(connStr);
            // conn.Close();
            // MySqlCommand cmd = new MySqlCommand(storeprocedurename, conn);
            // conn.Open();
            // cmd.Connection = conn;
            // // cmd.CommandText = "add_emp";
            // cmd.CommandType = CommandType.StoredProcedure;
            //// cmd.Parameters.Add(new MySqlParameter("GSTcode", str));
            //// cmd.Connection.Open();
            // var result = cmd.ExecuteReader();
            // cmd.Connection.Close();
            //using (MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr))
            //{
            //    MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();
            //    cmd.Connection = conn;
            //    cmd.CommandText = "CALL " + storeprocedurename + "("+str +" , @gstrate, @CGSTrate, @SGSTrate, @IGSTrate)";

            //    // cmd.CommandText = "CALL MyProcedure(@MyOutputNum1, @MyOutputString2, ?MyParam1, ?MyParam2); SELECT CAST(@MyOutputNum1 AS SIGNED), @MyOutputString2;";
            //    // I am using the ?param style to make it easy to differentiate between user variables and parameters. @MyParam1 would also work.
            //  //  cmd.Parameters.AddWithValue("@GSTcode", str);
            //    //cmd.Parameters.AddWithValue("?MyParam2", "blah");
            //    conn.Open();
            //    MySql.Data.MySqlClient.MySqlDataReader rdr = cmd.ExecuteReader();
            //    // If MyProcedure returns a result set that will come first so you will need: 
            //    //   while (rdr.Read()) {...} 
            //    //   and rdr.NextResult();
            //    // Now get the output parameters.
            //    long myOutputNum1 = -1;
            //    string myOutputString2 = null;
            //    if (rdr.Read())
            //    {
            //        int i = 0;
            //        if (rdr.FieldCount > i && !rdr.IsDBNull(i)) myOutputNum1 = rdr.GetInt64(i);
            //        i++;
            //        if (rdr.FieldCount > i && !rdr.IsDBNull(i)) myOutputString2 = rdr.GetString(i);
            //    }
            //}
            //if (OpenConnection())
            //{
            //    //_sqlCmd = new MySqlCommand(str, _connection);
            //    //_sqlCmd.CommandTimeout = 100;
            //    //_adapter = new MySqlDataAdapter(_sqlCmd);
            //    //_ds = new DataSet();
            //    //_adapter.Fill(_ds);
            //    //return _ds;


            //    // string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

            //    _sqlCmd = new MySqlCommand(storeprocedurename, _connection);
            //    _sqlCmd.CommandType = CommandType.StoredProcedure;
            //    _sqlCmd.Parameters.AddWithValue("GSTcode", str);
            //    using (MySqlDataAdapter sda = new MySqlDataAdapter(_sqlCmd))
            //    {
            //        DataTable dt = new DataTable();
            //        sda.Fill(dt);
            //        //GridView1.DataSource = dt;
            //        //GridView1.DataBind();
            //        return _ds;
            //    }

            //}
            //else
            //{
            //    return null;
            //}
            //  return ds;
        }
        catch (Exception ex)
        {
            clsLog.Publish(ex);
            return null;
        }
        finally
        {
            //_ds.Dispose();
            //_sqlCmd.Dispose();
            //_adapter.Dispose();
            //_connection.Close();
            //_connection.Dispose();
        }
    }
    #endregion

}