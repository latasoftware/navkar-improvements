using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Configuration;
using System.Text;

public partial class Sugar_pgeReceiptPaymentxml : System.Web.UI.Page
{

    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string qryHead = string.Empty;
    string qryDetail = string.Empty;
    string cityMasterTable = string.Empty;
    string systemMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string voucherTable = string.Empty;
    string qryVoucherList = string.Empty;
    int defaultAccountCode = 0;
    string trntype = string.Empty;
    string qryAccountList = string.Empty;
    string GLedgerTable = string.Empty;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    string user = string.Empty;
    static WebControl objAsp = null;

    string cs = string.Empty;
    string Action = string.Empty;
    int Receiptid = 0;
    int flag = 0;
    int count = 0;
    int Doc_No = 0;
    MySqlConnection con = null;
    MySqlCommand cmd = null;
    MySqlTransaction myTran = null;
    #endregion

    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;

    string GLEDGER_Insert = string.Empty;
    string GLEDGER_Delete = string.Empty;
    string msg = string.Empty;
    #region Head Declaration
    string transaction_type = string.Empty;
    string doc_date = string.Empty;
    int cashbank = 0;
    double total = 0.00;
    int Company_Code = 0;
    int Year_Code = 0;
    int cb = 0;
    int Branch_Code = 0;
    string Created_By = string.Empty;
    string Modified_By = string.Empty;
    string Created_Date = string.Empty;
    string Modified_Date = string.Empty;


    StringBuilder Head_Update = null;
    string Head_Insert = string.Empty;
    string Head_Delete = string.Empty;
    StringBuilder Head_Values = null;
    StringBuilder Head_Fields = null;
    #endregion

    #region Detail Declaration
    int AcCode = 0;
    string Name = string.Empty;
    int Unit_Code = 0;
    string Unit_Name = string.Empty;
    int Voucher_No = 0;
    string Voucher_Type = string.Empty;
    int TenderNo = 0;
    int TenderDetailId = 0;
    int DetailID = 0;
    double amount = 0.00;
    double Adjusted_Amount = 0.00;
    int trandetailid = 0;
    string narration = string.Empty;
    string narration2 = string.Empty;
    string drpFilterValue = string.Empty;
    string Branch_name = string.Empty;
    int YearCodeDetail = 0;

    string Detail_Insert = string.Empty;
    StringBuilder Detail_Update = null;
    StringBuilder Detail_Delete = null;
    StringBuilder Detail_Fields = null;
    StringBuilder Detail_Values = null;
    #endregion

    #region Grid Fields
    int Grid_ID = 2;
    int Grid_AcCode = 3;
    int Grid_Name = 4;
    int Grid_Unit_Code = 5;
    int Grid_Unit_Name = 6;
    int Grid_Voucher_No = 7;
    int Grid_Voucher_Type = 8;
    int Grid_TenderNo = 9;
    int Grid_DetailID = 10;
    int Grid_amount = 11;
    int Grid_Adjusted_Amount = 12;
    int Grid_narration = 13;
    int Grid_narration2 = 14;
    int Grid_drpFilterValue = 15;
    // int Grid_Branch_name = 16;
    int Grid_YearCodeDetail = 16;
    int _Grid_trandetailid = 17;
    int Grid_rowAction = 18;
    int Grid_SrNo = 19;
    #endregion
    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "transacthead";
            tblDetails = tblPrefix + "transactdetail";
            AccountMasterTable = tblPrefix + "AccountMaster";
            qryCommon = "qrytransheaddetail";
            qryHead = "qrytransacthead";
            qryDetail = "qrytransactdetail";
            qryAccountList = "qrymstaccountmaster";
            cityMasterTable = tblPrefix + "CityMaster";
            systemMasterTable = tblPrefix + "SystemMaster";
            voucherTable = tblPrefix + "Voucher";
            qryVoucherList = tblPrefix + "qryVoucherList";
            // trntype = drpTrnType.SelectedValue;
            user = Session["user"].ToString();
            GLedgerTable = tblPrefix + "GLEDGER";
            pnlPopup.Style["display"] = "none";
            cs = ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
            con = new MySqlConnection(cs);

            Head_Fields = new StringBuilder();
            Head_Values = new StringBuilder();
            Head_Update = new StringBuilder();
            Detail_Update = new StringBuilder();
            Detail_Delete = new StringBuilder();
            Detail_Fields = new StringBuilder();
            Detail_Values = new StringBuilder();

            Maindt = new DataTable();
            dr = null;
            Maindt.Columns.Add("Querys", typeof(string));
            dr = Maindt.NewRow();
            if (!Page.IsPostBack)
            {
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    Action = Request.QueryString["Action"];
                    hdnfTran_type.Value = Request.QueryString["tran_type"];
                    if (Action == "1")
                    {
                        hdnf.Value = Request.QueryString["tranid"];
                        pnlPopup.Style["display"] = "none";
                        ViewState["currentTable"] = null;
                        clsButtonNavigation.enableDisable("N");
                        this.makeEmptyForm("N");
                        //drpTrnType.Text = hdnfTran_type.Value;
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
                        this.TranTypeFilter();
                        drpTrnType.Text = hdnfTran_type.Value;
                        setFocusControl(txtdoc_date);
                    }

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
                obj.tableName = tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "and Tran_Type='" + trntype + "'";
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
                                    txtdoc_no.Enabled = false;
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

                pnlPopup.Style["display"] = "none";
                // btnOpenDetailsPopup.Enabled = false;
                btnSave.Text = "Save";
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                lblMsg.Text = string.Empty;
                calenderExtenderDate.Enabled = false;
                //btnDelete.Enabled = true;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                pnlgrdDetail.Enabled = false;
                // btnFirst.Enabled = true;
                // btnPrevious.Enabled = true;
                drpTrnType.Enabled = true;
                txtEditDoc_No.Enabled = true;
                lblCashBank.Text = string.Empty;
                txtTotal.Text = string.Empty;
                //btnDelete.Enabled = true;
                btntxtCashBank.Enabled = false;
                btntxtACCode.Enabled = false;
                btntxtUnitcode.Enabled = false;
                btnsoudaall.Enabled = false;

                trntype = hdnfTran_type.Value;
                if (trntype != "CR")
                {
                    drpFilter.Visible = true;
                    txtVoucherNo.Enabled = false;
                    txtvoucherType.Enabled = false;
                    btntxtVoucherNo.Enabled = false;
                }
                else
                {
                    txtVoucherNo.Enabled = false;
                    txtvoucherType.Enabled = false;
                    btntxtVoucherNo.Enabled = false;
                }
                btnAdddetails.Enabled = false;
                btnClosedetails.Enabled = false;

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
                btnSave.Text = "Save";
                btntxtdoc_no.Text = "Change No";

                btntxtdoc_no.Enabled = true;
                lblMsg.Text = string.Empty;
                #region set Business logic for save
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                setFocusControl(txtdoc_date);
                calenderExtenderDate.Enabled = true;
                // btnOpenDetailsPopup.Enabled = true;
                pnlgrdDetail.Enabled = true;
                txtEditDoc_No.Enabled = false;

                drpTrnType.Enabled = false;
                lblCashBank.Text = string.Empty;
                txtTotal.Text = string.Empty;
                trntype = hdnfTran_type.Value;
                if (trntype == "BP" || trntype == "BR")
                {
                    txtCashBank.Enabled = true;
                    btntxtCashBank.Enabled = true;
                }
                else
                {
                    txtCashBank.Enabled = false;
                    btntxtCashBank.Enabled = false;
                }
                txtdoc_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                btntxtdoc_no.Enabled = false;
                drpTrnType.Enabled = true;

                btntxtACCode.Enabled = true;
                btntxtUnitcode.Enabled = true;
                btntxtnarration.Enabled = true;
                #endregion



                drpTrnType.Enabled = false;
                btnAdddetails.Enabled = true;
                btnClosedetails.Enabled = true;

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
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                calenderExtenderDate.Enabled = false;
                //  btnOpenDetailsPopup.Enabled = false;
                pnlgrdDetail.Enabled = false;
                txtEditDoc_No.Enabled = true;
                drpTrnType.Enabled = true;
                btntxtCashBank.Enabled = false;
                btntxtACCode.Enabled = false;
                btntxtUnitcode.Enabled = false;
                btntxtVoucherNo.Enabled = false;
                btnsoudaall.Enabled = false;
                btntxtnarration.Enabled = false;

                btnAdddetails.Enabled = false;
                btnClosedetails.Enabled = false;
                drpTrnType.Enabled = true;
            }
            if (dAction == "E")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                    }
                }
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = true;
                lblMsg.Text = string.Empty;
                setFocusControl(txtdoc_date);
                calenderExtenderDate.Enabled = true;
                //btnOpenDetailsPopup.Enabled = true;
                pnlgrdDetail.Enabled = true;
                txtCashBank.Enabled = true;
                btntxtCashBank.Enabled = true;
                drpTrnType.Enabled = false;
                txtEditDoc_No.Enabled = false;
                txtvoucherType.Enabled = false;
                btntxtACCode.Enabled = true;
                btntxtUnitcode.Enabled = true;

                btnAdddetails.Enabled = true;
                btnClosedetails.Enabled = true;
                drpTrnType.Enabled = false;
            }
            #region Always check this
            //if (dAction == "A" || dAction == "E")
            //{
            //    if (txtCashBank.Text != String.Empty)
            //    {
            //        txtCashBank.Text = "";
            //        lblCashBank.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=1 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

            //    }
            //    //if (drpTrnType.SelectedValue == "CP" || drpTrnType.SelectedValue == "CR")
            //    //{
            //    //    txtCashBank.Text = "1";
            //    //    lblCashBank.Text = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=1 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            //    //    txtCashBank.Enabled = true;
            //    //    btntxtCashBank.Enabled = true;
            //    //}
            //    //else
            //    //{
            //    //    txtCashBank.Enabled = true;
            //    //    btntxtCashBank.Enabled = true;
            //    //}
            //}


            #endregion
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

            qry = getDisplayQuery();
            bool recordExist = this.fetchRecord(qry);
            if (recordExist == true)
            {
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnAdd.Focus();
            }
            else                     //new code
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
            }

            // this.enableDisableNavigateButtons();

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
    //    query = "select count(*) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";
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
    //    if (RecordCount > 0)
    //    {
    //        if (txtdoc_no.Text != string.Empty)
    //        {
    //            if (hdnf.Value != string.Empty)
    //            {
    //                #region check for next or previous record exist or not
    //                ds = new DataSet();
    //                dt = new DataTable();
    //                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "' ORDER BY doc_no asc  ";
    //                ds = clsDAL.SimpleQuery(query);
    //                if (ds != null)
    //                {
    //                    if (ds.Tables.Count > 0)
    //                    {
    //                        dt = ds.Tables[0];
    //                        if (dt.Rows.Count > 0)
    //                        {
    //                            //next record exist
    //                            btnNext.Enabled = true;
    //                            btnLast.Enabled = true;
    //                        }
    //                        else
    //                        {
    //                            //next record does not exist
    //                            btnNext.Enabled = false;
    //                            btnLast.Enabled = false;
    //                        }
    //                    }
    //                }
    //                ds = new DataSet();
    //                dt = new DataTable();
    //                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "' ORDER BY doc_no asc  ";
    //                ds = clsDAL.SimpleQuery(query);
    //                if (ds != null)
    //                {
    //                    if (ds.Tables.Count > 0)
    //                    {
    //                        dt = ds.Tables[0];
    //                        if (dt.Rows.Count > 0)
    //                        {
    //                            //previous record exist
    //                            btnPrevious.Enabled = true;
    //                            btnFirst.Enabled = true;
    //                        }
    //                        else
    //                        {
    //                            btnPrevious.Enabled = false;
    //                            btnFirst.Enabled = false;
    //                        }
    //                    }
    //                }

    //                #endregion
    //            }
    //        }
    //        this.makeEmptyForm("S");
    //    }
    //    else
    //    {
    //        this.makeEmptyForm("N");
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
            query = "select doc_no from " + tblHead + " where doc_no=(select MIN(doc_no) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "')  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";
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
                string query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) +
                            "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'" +
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
                string query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) +
                            "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'" +
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
            query = "select doc_no from " + tblHead + " where doc_no=(select MAX(doc_no) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "')  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";
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
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        hdnfTran_type.Value = drpTrnType.SelectedValue;
        this.makeEmptyForm("A");
        this.TranTypeFilter();
        this.NextNumber();
        // this.getMaxCode();
        // pnlPopupDetails.Style["display"] = "none";
    }

    private void TranTypeFilter()
    {
        try
        {
            trntype = hdnfTran_type.Value;
            if (trntype == "BP" || trntype == "CP")
            {
                drpFilter.Visible = true;
                drpFilter.Items.Clear();
                drpFilter.Items.Add(new ListItem("--Select--", "A"));
                drpFilter.Items.Add(new ListItem("Against Transport Advance", "T"));
                txtVoucherNo.Enabled = false;
                txtvoucherType.Enabled = false;
                btntxtVoucherNo.Enabled = false;
                btnsoudaall.Enabled = false;
            }
            else
            {
                if (trntype == "BR")
                {
                    drpFilter.Visible = true;
                    drpFilter.Items.Clear();
                    drpFilter.Items.Add(new ListItem("Against Sauda", "S"));
                    drpFilter.Items.Add(new ListItem("Against Frieght", "F"));
                    //drpFilter.Items.Add(new ListItem("Against Loading Voucher", "V"));
                    drpFilter.Items.Add(new ListItem("Against Debit Note", "D"));
                    drpFilter.Items.Add(new ListItem("Against Credit Bill", "P"));
                    txtVoucherNo.Enabled = true;
                    txtvoucherType.Enabled = true;
                    btntxtVoucherNo.Enabled = true;
                    btnsoudaall.Enabled = true;
                }
                else
                {
                    drpFilter.Visible = false;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        clsButtonNavigation.enableDisable("E");
        pnlgrdDetail.Enabled = true;
        this.makeEmptyForm("E");
        txtdoc_no.Enabled = false;
        trntype = drpTrnType.SelectedValue;
        if (trntype == "BP" || trntype == "CP")
        {
            drpFilter.Visible = true;
            drpFilter.Items.Clear();
            drpFilter.Items.Add(new ListItem("--Select--", "A"));
            drpFilter.Items.Add(new ListItem("Against Transport Advance", "T"));
            txtVoucherNo.Enabled = false;
            txtvoucherType.Enabled = false;
            btntxtVoucherNo.Enabled = false;
        }
        else
        {
            if (trntype == "BR")
            {
                drpFilter.Visible = true;
                drpFilter.Items.Clear();
                // drpFilter.Items.Add(new ListItem("Against Loading Voucher", "V"));
                drpFilter.Items.Add(new ListItem("Against Sauda", "S"));
                drpFilter.Items.Add(new ListItem("Against Frieght", "F"));
                drpFilter.Items.Add(new ListItem("Against Debit Note", "D"));
                drpFilter.Items.Add(new ListItem("Against Credit Bill", "P"));
                txtVoucherNo.Enabled = true;
                txtvoucherType.Enabled = true;
                btntxtVoucherNo.Enabled = true;
                btnsoudaall.Enabled = true;
            }
            else
            {
                drpFilter.Visible = false;
            }
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
                string currentDoc_No = lblReceiptPayment_Id.Text;
                string tran_type = drpTrnType.SelectedValue;
                Head_Delete = "delete from " + tblHead + " where tranid='" + currentDoc_No + "' and tran_type='" + tran_type + "'";

                string Detail_Deleteqry = "delete from " + tblDetails + " where tranid='" + currentDoc_No + "' and tran_type='" + tran_type + "'";

                GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='" + tran_type + "' and Doc_No=" + txtdoc_no.Text + " " +
                    " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " +
                    " Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = GLEDGER_Delete;
                Maindt.Rows.Add(dr);

                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Detail_Deleteqry;
                Maindt.Rows.Add(dr);

                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Head_Delete;
                Maindt.Rows.Add(dr);
                flag = 3;

                msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);

                if (msg == "Delete")
                {
                    Response.Redirect("../Transaction/PgeReceiptPaymentUtility.aspx");
                }
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
        string max = clsCommon.getString("select ifnull(max(tranid),0) as id from " + tblHead + " where tran_type='" + drpTrnType.SelectedValue + "' ");
        hdnfTran_type.Value = drpTrnType.SelectedValue;
        hdnf.Value = max;
        clsButtonNavigation.enableDisable("S");
        this.makeEmptyForm("S");
        this.showLastRecord();
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
                        lblReceiptPayment_Id.Text = hdnf.Value;
                        drpTrnType.Text = dt.Rows[0]["tran_type"].ToString();
                        txtdoc_date.Text = dt.Rows[0]["doc_dateConverted"].ToString();
                        txtCashBank.Text = dt.Rows[0]["cashbank"].ToString();
                        lblCashBank.Text = dt.Rows[0]["Ac_Name_E"].ToString();
                        txtTotal.Text = dt.Rows[0]["total"].ToString();
                        //Label lblCreated = (Label)Master.FindControl("MasterlblCreatedBy");
                        //Label lblModified = (Label)Master.FindControl("MasterlblModifiedBy");
                        //if (lblCreated != null)
                        //{
                        //    lblCreated.Text = "Created By: " + dt.Rows[0]["Created_By"].ToString();
                        //}
                        //if (lblModified != null)
                        //{
                        //    lblModified.Text = "Modified By: " + dt.Rows[0]["Modified_By"].ToString();
                        //}
                        recordExist = true;
                        lblMsg.Text = "";

                        trntype = drpTrnType.SelectedValue;

                        if (trntype == "BR")
                        {
                            drpFilter.Visible = true;
                        }
                        else
                        {
                            drpFilter.Visible = false;
                        }


                        #region ---------- Details -------------
                        //                       qry = "select detail_id as ID,credit_ac as AcCode,creditAcName as Name,Unit_Code,Unit_Name,Voucher_No,Voucher_Type,[Tender_No] as TenderNo,[TenderDetail_ID] as DetailID, amount,Adjusted_Amount,narration,narration2,drpFilterValue,Branch_name,isnull(YearCodeDetail,0) as YearCodeDetail,trandetailid" +
                        //" from " + qryDetail + " where doc_no=" + txtdoc_no.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";

                        qry = "select detail_id as ID,credit_ac as AcCode,creditname as Name,Unit_Code,unitname as Unit_Name,Voucher_No,Voucher_Type,Tender_No as TenderNo," +
                            " tenderdetailid as DetailID, amount,Adjusted_Amount,narration,narration2,drpFilterValue,ifnull(YearCodeDetail,0) as YearCodeDetail,trandetailid" +
" from " + qryDetail + " where tranid=" + hdnf.Value + " and Tran_Type='" + hdnfTran_type.Value + "'";
                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
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
            this.columnTotal();
            //csCalculations();
            //    this.enableDisableNavigateButtons();
            hdnf.Value = txtdoc_no.Text;
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
            string qryDisplay = "select * from " + qryHead + " where tranid=" + hdnf.Value + " and Tran_Type='" + hdnfTran_type.Value + "'";
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
                // this.enableDisableNavigateButtons();
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
        // pnlPopupDetails.Style["display"] = "block";
        lblNo.Text = string.Empty;
        lblID.Text = string.Empty;
        txtACCode.Text = string.Empty;
        txtUnit_Code.Text = string.Empty;
        txtVoucherNo.Text = string.Empty;
        txtvoucherType.Text = string.Empty;
        txtamount.Text = string.Empty;
        txtadAmount.Text = string.Empty;
        txtnarration.Text = string.Empty;
        lblACName.Text = "";
        lblUnitName.Text = "";
        trntype = drpTrnType.SelectedValue;
        if (trntype == "BP" || trntype == "CP")
        {
            drpFilter.Visible = true;
            drpFilter.Items.Clear();
            drpFilter.Items.Add(new ListItem("--Select--", "A"));
            drpFilter.Items.Add(new ListItem("Against Transport Advance", "T"));
            if (trntype == "BP")
            {
                drpFilter.Items.Add(new ListItem("Against Sauda", "M"));
            }
            txtVoucherNo.Enabled = false;
            txtvoucherType.Enabled = false;
            btntxtVoucherNo.Enabled = false;
        }
        else
        {
            if (trntype == "BR")
            {
                drpFilter.Visible = true;
                drpFilter.Items.Clear();
                drpFilter.Items.Add(new ListItem("Against Sauda", "S"));
                drpFilter.Items.Add(new ListItem("Against Frieght", "F"));
                // drpFilter.Items.Add(new ListItem("Against Loading Voucher", "V"));
                drpFilter.Items.Add(new ListItem("Against Debit Note", "D"));
                drpFilter.Items.Add(new ListItem("Against Credit Bill", "P"));
                txtVoucherNo.Enabled = true;
                txtvoucherType.Enabled = true;
                btntxtVoucherNo.Enabled = true;
            }
            else
            {
                drpFilter.Visible = false;
            }
        }
        setFocusControl(txtACCode);
    }
    #endregion

    #region [btnAdddetails_Click]
    protected void btnAdddetails_Click(object sender, EventArgs e)
    {
        try
        {
            bool isValidated = true;
            if (txtACCode.Text != string.Empty)
            {
                string iscarporate = clsCommon.getString("select carporate_party from " + qryAccountList + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (iscarporate == "Y")
                {
                    txtUnit_Code.Text = txtUnit_Code.Text != string.Empty ? txtUnit_Code.Text : "0";
                    if (txtUnit_Code.Text != string.Empty)
                    {
                        isValidated = true;
                    }
                    else
                    {
                        isValidated = false;
                        setFocusControl(txtUnit_Code);
                        return;
                    }
                }
            }
            if (txtamount.Text != string.Empty)
            {
                isValidated = true;

            }
            else
            {
                isValidated = false;
                setFocusControl(txtamount);
                return;
            }
            txtVoucherNo.Text = txtVoucherNo.Text.Trim();
            // Int32 voucherno = txtVoucherNo.Text != string.Empty ? Convert.ToInt32(txtVoucherNo.Text) : 0;
            if (txtVoucherNo.Text != string.Empty && txtVoucherNo.Text != "0")
            // if (voucherno != 0)
            {
                string hdnfVal = hdnfTransportBalance.Value.ToString().TrimStart();
                if (!string.IsNullOrEmpty(hdnfVal))
                {
                    double TransportAdvanceBalance = Convert.ToDouble(hdnfTransportBalance.Value.TrimStart());
                    double amount1 = Convert.ToDouble(txtamount.Text);
                    if (amount1 > TransportAdvanceBalance)
                    {
                        lblErrorAdvance.Text = "Amount Is Greater Than Transport Advance Balance!";
                        isValidated = true;
                        //setFocusControl(txtamount);
                        //return;
                    }
                    else
                    {
                        lblErrorAdvance.Text = "";

                        isValidated = true;
                    }
                }
                else
                {
                    lblErrorAdvance.Text = "";
                }
            }
            else
            {
                txtvoucherType.Text = "";
            }

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

                        // rowIndex = dt.Rows.Count + 1;
                        dr["ID"] = rowIndex;     //auto
                        dr["rowAction"] = "A";
                        dr["SrNo"] = 0;
                    }
                    else
                    {
                        //update row
                        int n = int.Parse(lblNo.Text);
                        rowIndex = Convert.ToInt32(lblID.Text);   //auto no
                        dr = (DataRow)dt.Rows[n - 1];
                        dr["ID"] = rowIndex;
                        dr["SrNo"] = 0;

                        #region decide whether actual row is updating or virtual [rowAction]
                        string id = clsCommon.getString("select detail_id from nt_1_transactdetail where detail_id=" + rowIndex + " " +
                        " and Tran_Type='" + drpTrnType.SelectedValue + "' and doc_no='"+txtdoc_no.Text+"'");
                        if (id != "0")
                        {
                            dr["rowAction"] = "U";   //actual row
                        }
                        else
                        {
                            dr["rowAction"] = "A";    //virtual row
                        }
                        #endregion
                    }
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add((new DataColumn("ID", typeof(int))));

                    #region [Write here columns]
                    dt.Columns.Add((new DataColumn("AcCode", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("Name", typeof(string))));
                    dt.Columns.Add((new DataColumn("Unit_Code", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("Unit_Name", typeof(string))));
                    dt.Columns.Add((new DataColumn("Voucher_No", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("Voucher_Type", typeof(string))));
                    dt.Columns.Add((new DataColumn("TenderNo", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("DetailID", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("amount", typeof(double))));
                    dt.Columns.Add((new DataColumn("Adjusted_Amount", typeof(double))));
                    dt.Columns.Add((new DataColumn("narration", typeof(string))));
                    dt.Columns.Add((new DataColumn("narration2", typeof(string))));
                    dt.Columns.Add((new DataColumn("drpFilterValue", typeof(string))));
                    //dt.Columns.Add((new DataColumn("Branch_name", typeof(string))));
                    dt.Columns.Add((new DataColumn("YearCodeDetail", typeof(int))));
                    dt.Columns.Add((new DataColumn("trandetailid", typeof(int))));
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
                dt.Columns.Add((new DataColumn("AcCode", typeof(Int32))));
                dt.Columns.Add((new DataColumn("Name", typeof(string))));
                dt.Columns.Add((new DataColumn("Unit_Code", typeof(Int32))));
                dt.Columns.Add((new DataColumn("Unit_Name", typeof(string))));
                dt.Columns.Add((new DataColumn("Voucher_No", typeof(Int32))));
                dt.Columns.Add((new DataColumn("Voucher_Type", typeof(string))));
                dt.Columns.Add((new DataColumn("TenderNo", typeof(Int32))));
                dt.Columns.Add((new DataColumn("DetailID", typeof(Int32))));
                dt.Columns.Add((new DataColumn("amount", typeof(double))));
                dt.Columns.Add((new DataColumn("Adjusted_Amount", typeof(double))));
                dt.Columns.Add((new DataColumn("narration", typeof(string))));
                dt.Columns.Add((new DataColumn("narration2", typeof(string))));
                dt.Columns.Add((new DataColumn("drpFilterValue", typeof(string))));
                //dt.Columns.Add((new DataColumn("Branch_name", typeof(string))));
                dt.Columns.Add((new DataColumn("YearCodeDetail", typeof(int))));
                dt.Columns.Add((new DataColumn("trandetailid", typeof(int))));
                #endregion
                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                dt.Columns.Add((new DataColumn("SrNo", typeof(int))));

                dr = dt.NewRow();
                dr["ID"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;
            }

            #region [ Set values to dr]
            if (txtACCode.Text != string.Empty)
            {
                //string str = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                //if (str != string.Empty)
                //{
                //    dr["AcCode"] = txtACCode.Text;
                //    dr["Name"] = str;
                //}
                //else
                //{
                //    lblACName.Text = string.Empty;
                //    txtACCode.Text = string.Empty;
                //    setFocusControl(txtACCode);
                //    return;
                //}
                dr["AcCode"] = txtACCode.Text;
                dr["Name"] = lblACName.Text;
            }
            else
            {
                lblACName.Text = string.Empty;
                txtACCode.Text = string.Empty;
                setFocusControl(txtACCode);
                return;
            }
            if (txtACCode.Text != string.Empty)
            {
                string iscarporate = clsCommon.getString("select carporate_party from " + qryAccountList + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (iscarporate == "Y")
                {
                    txtUnit_Code.Text = txtUnit_Code.Text != string.Empty ? txtUnit_Code.Text : "0";
                    if (txtUnit_Code.Text != string.Empty)
                    {
                        //string str = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtUnit_Code.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //if (str != string.Empty)
                        //{
                        //    dr["Unit_Code"] = txtUnit_Code.Text;
                        //    dr["Unit_Name"] = str;
                        //}
                        //else
                        //{
                        //    lblUnitName.Text = string.Empty;
                        //    txtUnit_Code.Text = string.Empty;
                        //    setFocusControl(txtUnit_Code);
                        //    return;
                        //}
                        dr["Unit_Code"] = txtUnit_Code.Text;
                        dr["Unit_Name"] = lblUnitName.Text;
                        isValidated = true;
                    }
                    else
                    {
                        isValidated = false;
                        setFocusControl(txtUnit_Code);
                        return;
                    }
                }
                else
                {
                    if (txtUnit_Code.Text != string.Empty)
                    {
                        dr["Unit_Code"] = txtUnit_Code.Text;
                        dr["Unit_Name"] = lblUnitName.Text;
                    }
                    else
                    {
                        dr["Unit_Code"] = "0";
                        dr["Unit_Name"] = "";
                    }
                }
            }


            if (drpFilter.SelectedValue == "V" || drpFilter.SelectedValue == "T" || drpFilter.SelectedValue == "D" || drpFilter.SelectedValue == "P")
            {
                //if (txtVoucherNo.Text != string.Empty)
                //{
                //    string str = clsCommon.getString("select Tran_Type from " + tblPrefix + "qryVoucherSaleUnion where Doc_No=" + txtVoucherNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                //    //string voucherby=clsCommon.getString("
                //    if (str != string.Empty)
                //    {
                //        dr["Voucher_No"] = txtVoucherNo.Text;
                //        dr["Voucher_Type"] = txtvoucherType.Text;

                //        dr["TenderNo"] = 0;
                //        dr["DetailID"] = 0;
                //    }
                //    else
                //    {
                //        txtvoucherType.Text = string.Empty;
                //        txtVoucherNo.Text = string.Empty;
                //        setFocusControl(txtVoucherNo);
                //        return;
                //    }

                //}
                dr["Voucher_No"] = txtVoucherNo.Text != string.Empty ? Convert.ToInt32(txtVoucherNo.Text) : 0;
                dr["Voucher_Type"] = txtvoucherType.Text;

                dr["TenderNo"] = 0;
                dr["DetailID"] = 0;

                dr["drpFilterValue"] = drpFilter.SelectedValue;
            }
            if (drpFilter.SelectedValue == "S" || drpFilter.SelectedValue == "M")
            {
                dr["Voucher_No"] = 0;
                dr["Voucher_Type"] = "";

                dr["TenderNo"] = txtVoucherNo.Text != string.Empty ? Convert.ToInt32(txtVoucherNo.Text) : 0;
                dr["DetailID"] = txtvoucherType.Text != string.Empty ? Convert.ToInt32(txtvoucherType.Text) : 0;
                dr["drpFilterValue"] = drpFilter.SelectedValue;
            }
            else
            {
                dr["drpFilterValue"] = drpFilter.SelectedValue;
            }
            if (txtamount.Text != string.Empty)
            {
                dr["amount"] = txtamount.Text;
            }
            else
            {
                setFocusControl(txtamount);
                return;
            }
            if (txtadAmount.Text != string.Empty)
            {
                dr["Adjusted_Amount"] = txtadAmount.Text;
            }
            else
            {
                dr["Adjusted_Amount"] = 0;
            }

            if (txtnarration.Text != string.Empty)
            {
                dr["narration"] = Server.HtmlDecode(txtnarration.Text);
            }
            else
            {
                dr["narration"] = Server.HtmlDecode(txtnarration.Text);
            }

            if (txtNarration2.Text != string.Empty)
            {
                dr["narration2"] = Server.HtmlDecode(txtNarration2.Text);
            }
            else
            {
                dr["narration2"] = Server.HtmlDecode(txtNarration2.Text);
            }


            //if (txtBranch_name.Text != string.Empty)
            //{
            //    dr["Branch_name"] = Server.HtmlDecode(txtBranch_name.Text);
            //}
            //else
            //{
            //    dr["Branch_name"] = Server.HtmlDecode(txtBranch_name.Text);
            //}
            lblVoucherBy.Text = lblVoucherBy.Text.Trim();
            if (lblVoucherBy.Text != string.Empty)
            {
                dr["YearCodeDetail"] = lblVoucherBy.Text;
            }
            else
            {
                dr["YearCodeDetail"] = 0;
            }

            #endregion

            if (btnAdddetails.Text == "ADD")
            {
                dr["trandetailid"] = 0;
                dt.Rows.Add(dr);
            }

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
                // pnlPopupDetails.Style["display"] = "block";
                setFocusControl(txtACCode);
            }
            else
            {
                // pnlPopupDetails.Style["display"] = "none";
                setFocusControl(btnAdddetails);
                // btnOpenDetailsPopup.Focus();
            }
            // Empty Code->
            csCalculations();
            txtACCode.Text = string.Empty;
            txtVoucherNo.Text = string.Empty;
            txtvoucherType.Text = string.Empty;
            lblACName.Text = string.Empty;
            txtamount.Text = string.Empty;
            txtadAmount.Text = string.Empty;
            txtnarration.Text = string.Empty;
            txtNarration2.Text = string.Empty;
            txtUnit_Code.Text = string.Empty;
            lblUnitName.Text = string.Empty;
            // txtBranch_name.Text = string.Empty;
            lblVoucherBy.Text = string.Empty;
            btnAdddetails.Text = "ADD";
            setFocusControl(txtACCode);
        }
        catch
        {
        }
    }
    #endregion

    #region [btnClosedetails_Click]
    protected void btnClosedetails_Click(object sender, EventArgs e)
    {
        // pnlPopupDetails.Style["display"] = "none";
        setFocusControl(btnSave);
    }
    #endregion

    #region [showDetailsRow]
    private void showDetailsRow(GridViewRow gr)
    {
        try
        {
            this.TranTypeFilter();
            lblNo.Text = Server.HtmlDecode(gr.Cells[Grid_SrNo].Text);
            lblID.Text = Server.HtmlDecode(gr.Cells[2].Text);
            txtACCode.Text = Server.HtmlDecode(gr.Cells[Grid_AcCode].Text);
            lblACName.Text = Server.HtmlDecode(gr.Cells[Grid_Name].Text);
            txtUnit_Code.Text = Server.HtmlDecode(gr.Cells[Grid_Unit_Code].Text);
            lblUnitName.Text = Server.HtmlDecode(gr.Cells[Grid_Unit_Name].Text);
            string selectedValue = Server.HtmlDecode(gr.Cells[Grid_drpFilterValue].Text);
            drpFilter.SelectedValue = selectedValue;
            if (Server.HtmlDecode(gr.Cells[Grid_Voucher_No].Text) != "0")
            {
                lblHead.Text = "Voucher No";
                txtVoucherNo.Text = Server.HtmlDecode(gr.Cells[Grid_Voucher_No].Text);
                txtvoucherType.Text = Server.HtmlDecode(gr.Cells[Grid_DetailID].Text);
            }
            else
            {
                lblHead.Text = "Sauda No";
                txtVoucherNo.Text = Server.HtmlDecode(gr.Cells[Grid_TenderNo].Text);
                txtvoucherType.Text = Server.HtmlDecode(gr.Cells[Grid_DetailID].Text);
            }
            txtVoucherNo.Enabled = false;
            txtvoucherType.Enabled = false;
            txtamount.Text = Server.HtmlDecode(gr.Cells[Grid_amount].Text);
            txtadAmount.Text = Server.HtmlDecode(gr.Cells[Grid_Adjusted_Amount].Text);
            txtnarration.Text = Server.HtmlDecode(gr.Cells[Grid_narration].Text);
            txtNarration2.Text = Server.HtmlDecode(gr.Cells[Grid_narration2].Text);
            // txtBranch_name.Text = Server.HtmlDecode(gr.Cells[16].Text);
            lblVoucherBy.Text = Server.HtmlDecode(gr.Cells[Grid_YearCodeDetail].Text);
        }
        catch (Exception)
        {
            throw;
        }
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
                int ID = Convert.ToInt32(dt.Rows[rowIndex]["trandetailid"].ToString());
                string IDExisting = clsCommon.getString("select detail_id from nt_1_transactdetail where trandetailid=" + ID + " and Tran_Type='" + hdnfTran_type.Value + "' ");
                if (IDExisting != string.Empty)
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[Grid_rowAction].Text = "D";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";            //D=Delete from table
                        csCalculations();
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[Grid_rowAction].Text = "N";
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
                        grdDetail.Rows[rowIndex].Cells[Grid_rowAction].Text = "R";       //R=Only remove fro grid
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "R";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[Grid_rowAction].Text = "A";
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

    #region [grdDetail_RowDataBound]
    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
        try
        {
            e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[Grid_ID].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[Grid_AcCode].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[Grid_Name].ControlStyle.Width = Unit.Percentage(30);
            e.Row.Cells[Grid_Unit_Code].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[Grid_Unit_Name].ControlStyle.Width = Unit.Percentage(30);
            e.Row.Cells[Grid_Voucher_No].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[Grid_Voucher_Type].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[Grid_TenderNo].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[Grid_DetailID].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[Grid_amount].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[Grid_Adjusted_Amount].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[Grid_narration].ControlStyle.Width = Unit.Percentage(25);
            e.Row.Cells[Grid_narration2].ControlStyle.Width = Unit.Percentage(25);
            e.Row.Cells[Grid_YearCodeDetail].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[_Grid_trandetailid].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[Grid_rowAction].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[Grid_SrNo].ControlStyle.Width = Unit.Percentage(20);

            e.Row.Cells[0].Style["overflow"] = "hidden";
            e.Row.Cells[1].Style["overflow"] = "hidden";
            e.Row.Cells[2].Style["overflow"] = "hidden";
            e.Row.Cells[3].Style["overflow"] = "hidden";
            e.Row.Cells[4].Style["overflow"] = "hidden";
            e.Row.Cells[11].Style["overflow"] = "hidden";
            e.Row.Cells[6].Style["overflow"] = "hidden";
            e.Row.Cells[7].Style["overflow"] = "hidden";
            e.Row.Cells[8].Style["overflow"] = "hidden";
            e.Row.Cells[9].Style["overflow"] = "hidden";
            e.Row.Cells[10].Style["overflow"] = "hidden";
            e.Row.Cells[12].Style["overflow"] = "hidden";
            e.Row.Cells[16].Style["overflow"] = "hidden";
            e.Row.Cells[15].Style["overflow"] = "hidden";
            e.Row.Cells[17].Style["overflow"] = "hidden";
            e.Row.Cells[18].Style["overflow"] = "hidden";
            e.Row.Cells[19].Style["overflow"] = "hidden";

            int i = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Left;

                if (e.Row.Cells[13].Text.Length > 27)
                {
                    e.Row.Cells[13].Style["overflow"] = "hidden";
                    string s = e.Row.Cells[13].Text.ToString();
                    //e.Row.Cells[11].Text = e.Row.Cells[11].Text.Substring(0, 27) + "..";
                    e.Row.Cells[13].ToolTip = s;
                }
                if (e.Row.Cells[14].Text.Length > 27)
                {
                    e.Row.Cells[14].Style["overflow"] = "hidden";
                    string s = e.Row.Cells[14].Text.ToString();
                    //e.Row.Cells[11].Text = e.Row.Cells[11].Text.Substring(0, 27) + "..";
                    e.Row.Cells[14].ToolTip = s;
                }

                if (e.Row.Cells[16].Text.Length > 27)
                {
                    e.Row.Cells[16].Style["overflow"] = "hidden";
                    string s = e.Row.Cells[16].Text.ToString();
                    //e.Row.Cells[11].Text = e.Row.Cells[11].Text.Substring(0, 27) + "..";
                    e.Row.Cells[16].ToolTip = s;
                }
            }
            // e.Row.Cells[_Grid_trandetailid].Visible = false;
            // e.Row.Cells[18].Visible = false;
            //e.Row.Cells[19].Visible = false;

        }
        catch
        {
        }
    }
    #endregion

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string v = hdnfClosePopup.Value;
        int i = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (v == "txtACCode" || v == "txtUnit_Code")
            {
                e.Row.Cells[0].Width = new Unit("40px");
                e.Row.Cells[1].Width = new Unit("600px");
                e.Row.Cells[2].Width = new Unit("100px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
            if (v == "txtnarration")
            {
                e.Row.Cells[0].Width = new Unit("60px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }


            i++;
            foreach (TableCell cell in e.Row.Cells)
            {
                string s = cell.Text.ToString();
                if (cell.Text.Length > 35)
                {
                    cell.Text = cell.Text.Substring(0, 35) + "..";
                    cell.ToolTip = s;
                }
            }
        }
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            if (v == "txtVoucherNo" || v == "txtVoucherNo1")
            {
                if (drpFilter.SelectedValue == "V")
                {
                    e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(5);
                    e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(20);
                    e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[9].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[10].ControlStyle.Width = Unit.Percentage(10);

                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Left;
                    i++;
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        string s = cell.Text.ToString();
                        if (cell.Text.Length > 30)
                        {
                            cell.Text = cell.Text.Substring(0, 30) + "..";
                            cell.ToolTip = s;
                        }
                    }
                }
                if (drpFilter.SelectedValue == "T")
                {
                    e.Row.Cells[0].ControlStyle.Width = new Unit("15px");
                    e.Row.Cells[1].ControlStyle.Width = new Unit("10px");
                    e.Row.Cells[2].ControlStyle.Width = new Unit("10px");
                    e.Row.Cells[3].ControlStyle.Width = new Unit("300px");
                    e.Row.Cells[4].ControlStyle.Width = new Unit("100px");
                    e.Row.Cells[5].ControlStyle.Width = new Unit("300px");
                    e.Row.Cells[6].ControlStyle.Width = new Unit("80px");
                    e.Row.Cells[7].ControlStyle.Width = new Unit("80px");
                    e.Row.Cells[8].ControlStyle.Width = new Unit("80px");
                    e.Row.Cells[9].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[10].ControlStyle.Width = Unit.Percentage(10);
                }
                if (drpFilter.SelectedValue == "S")
                {
                    e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(5);
                    e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(20);
                    e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[9].ControlStyle.Width = Unit.Percentage(10);

                    e.Row.Cells[10].ControlStyle.Width = Unit.Percentage(10);

                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Left;
                    e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Left;
                    i++;
                    foreach (TableCell cell in e.Row.Cells)
                    {
                        string s = cell.Text.ToString();
                        if (cell.Text.Length > 30)
                        {
                            cell.Text = cell.Text.Substring(0, 30) + "..";
                            cell.ToolTip = s;
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region [grdPopup_RowCreated]
    protected void grdPopup_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow &&
            (e.Row.RowState == DataControlRowState.Normal ||
           e.Row.RowState == DataControlRowState.Alternate))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
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
                        if (grdDetail.Rows[rowindex].Cells[Grid_rowAction].Text != "D" && grdDetail.Rows[rowindex].Cells[Grid_rowAction].Text != "R")
                        {
                            // pnlPopupDetails.Style["display"] = "block";
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
            hdnfClosePopup.Value = "txtdoc_no";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtdoc_date_TextChanged]
    protected void txtdoc_date_TextChanged(object sender, EventArgs e)
    {
        searchString = txtdoc_date.Text;
        strTextBox = "txtdoc_date";
        csCalculations();
    }
    #endregion

    #region [txtACCode_TextChanged]
    protected void txtACCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtACCode.Text;
        strTextBox = "txtACCode";
        csCalculations();
    }
    #endregion

    #region [btntxtACCode_Click]
    protected void btntxtACCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtACCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtVoucherNo_TextChanged]
    protected void txtVoucherNo_TextChanged(object sender, EventArgs e)
    {
        searchString = txtVoucherNo.Text;
        strTextBox = "txtVoucherNo";
        lblVoucherBy.Text = hdnflblvoucher.Value;
        csCalculations();
        setFocusControl(txtamount);
    }
    #endregion

    #region [btntxtVoucherNo_Click]
    protected void btntxtVoucherNo_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtVoucherNo";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtamount_TextChanged]
    protected void txtamount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtamount.Text;
        strTextBox = "txtamount";
        csCalculations();
    }
    #endregion

    #region [txtadAmount_TextChanged]
    protected void txtadAmount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtadAmount.Text;
        strTextBox = "txtadAmount";
        csCalculations();
    }
    #endregion

    #region [txtnarration_TextChanged]
    protected void txtnarration_TextChanged(object sender, EventArgs e)
    {
        searchString = txtnarration.Text;
        strTextBox = "txtnarration";
        csCalculations();
    }
    #endregion

    #region [txtNarration2_TextChanged]
    protected void txtNarration2_TextChanged(object sender, EventArgs e)
    {
        searchString = txtNarration2.Text;
        strTextBox = "txtNarration2";
        csCalculations();
    }
    #endregion

    protected void txtEditDoc_No_TextChanged(object sender, EventArgs e)
    {
        {
            try
            {
                bool a = clsCommon.isStringIsNumeric(txtEditDoc_No.Text);
                if (a == false)
                {
                    searchString = txtEditDoc_No.Text;
                    pnlPopup.Style["display"] = "block";
                    hdnfClosePopup.Value = "txtEditDoc_No";
                    btnSearch_Click(this, new EventArgs());
                }
                else
                {
                    hdnf.Value = txtEditDoc_No.Text;
                    string qry1 = getDisplayQuery();
                    fetchRecord(qry1);
                    setFocusControl(txtEditDoc_No);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    #region [btntxtCashBank_Click]
    protected void btntxtCashBank_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtCashBank";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtCashBank_TextChanged]
    protected void txtCashBank_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCashBank.Text;
        strTextBox = "txtCashBank";
        csCalculations();
    }
    #endregion

    #region [btntxtnarration_Click]
    protected void btntxtnarration_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtnarration";
            btnSearch_Click(sender, e);
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
            if (hdnfClosePopup.Value == "txtACCode")
            {
                setFocusControl(txtACCode);
            }
            if (hdnfClosePopup.Value == "txtVoucherNo")
            {
                setFocusControl(txtVoucherNo);
            }
            if (hdnfClosePopup.Value == "txtnarration")
            {
                setFocusControl(txtnarration);
            }

            hdnfClosePopup.Value = "Close";
            pnlPopup.Style["display"] = "none";
            txtSearchText.Text = string.Empty;
            grdPopup.DataSource = null;
            grdPopup.DataBind();
        }
        catch
        {
        }
    }
    #endregion

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion                trntype = drpTrnType.SelectedValue;

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

        #region [Validation Part]
        bool isValidated = true;
        if (txtdoc_no.Text != string.Empty)
        {
            if (ViewState["mode"] != null)
            {
                if (ViewState["mode"].ToString() == "I")
                {
                    string str = clsCommon.getString("select doc_no from " + tblHead + " where doc_no='" + txtdoc_no.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trntype + "'");
                    if (str != string.Empty)
                    {
                        lblMsg.Text = "Code " + txtdoc_no.Text + " already exist";
                        this.getMaxCode();
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
        if (txtdoc_date.Text != string.Empty)
        {
            string dt = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            if (clsCommon.isValidDate(dt) == true)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtdoc_date);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtdoc_date);
            return;
        }
        trntype = drpTrnType.SelectedValue;
        if (trntype == "BP" || trntype == "BR")
        {
            if (txtCashBank.Text != string.Empty)
            {
                string str = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtCashBank.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtCashBank);
                    return;
                }
            }
            else
            {
                isValidated = false;
                setFocusControl(txtCashBank);
                return;
            }
        }
        #endregion


        #region -Head part declearation
        // Int32 DOC_NO = txtdoc_no.Text != string.Empty ? Convert.ToInt32(txtdoc_no.Text) : 0;
        doc_date = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        int cmpcashac = Convert.ToInt32(clsCommon.getString("Select Ac_Code from " + qryAccountList + " where Ac_Code=1 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())));
        cashbank = txtCashBank.Text != string.Empty ? Convert.ToInt32(txtCashBank.Text) : cmpcashac;
        if (cashbank != 0)
        {
            cb = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as ccid from " + qryAccountList + " where Ac_Code=" + cashbank + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " "));
        }
        else
        {
            cb = 0;
        }
        Created_By = Session["user"].ToString();
        Modified_By = Session["user"].ToString();
        Created_Date = DateTime.Now.ToString("yyyy/MM/dd");
        Modified_Date = DateTime.Now.ToString("yyyy/MM/dd");

        transaction_type = drpTrnType.SelectedValue;
        string retValue = string.Empty;
        string strRev = string.Empty;
        Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
        Year_Code = Convert.ToInt32(Session["year"].ToString());
        Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
        total = Convert.ToDouble(txtTotal.Text != string.Empty ? txtTotal.Text : "0");
        string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");

        //string drcr = string.Empty;
        //string drcr0 = string.Empty;
        //if (trntype == "CP" || trntype == "BP")
        //{
        //    drcr = "D";
        //    drcr0 = "C";
        //}
        //else
        //{
        //    drcr = "C";
        //    drcr0 = "D";
        //}

        #endregion-End of Head part declearation

        #region Detail Fields
        Detail_Fields.Append("Tran_Type,");
        Detail_Fields.Append("doc_no,");
        Detail_Fields.Append("doc_date,");
        Detail_Fields.Append("detail_id,");
        Detail_Fields.Append("debit_ac,");
        Detail_Fields.Append("credit_ac,");
        Detail_Fields.Append("Unit_Code,");
        Detail_Fields.Append("amount,");
        Detail_Fields.Append("narration,");
        Detail_Fields.Append("narration2,");
        Detail_Fields.Append("Company_Code,");
        Detail_Fields.Append("Year_Code,");
        Detail_Fields.Append("Branch_Code,");
        Detail_Fields.Append("Created_By,");
        Detail_Fields.Append("Voucher_No,");
        //Detail_Fields.Append( "Voucher_Type,");
        Detail_Fields.Append("Adjusted_Amount,");
        Detail_Fields.Append("Tender_No,");
        Detail_Fields.Append("tenderdetailid,");
        Detail_Fields.Append("drpFilterValue,");
        Detail_Fields.Append("YearCodeDetail,");
        Detail_Fields.Append("tranid,");
        Detail_Fields.Append("ca,");
        Detail_Fields.Append("uc,");
        //Detail_Fields.Append( "tenderdetailid,");
        // Detail_Fields.Append( "sbid,");
        // Detail_Fields.Append( "da,");
        Detail_Fields.Append("trandetailid");

        #endregion

        btnSave.Enabled = false;



        if (btnSave.Text == "Save")
        {
            this.NextNumber();
            #region Head Fields Assign Values
            Head_Fields.Append("tran_type,");
            Head_Values.Append("'" + transaction_type + "',");
            Head_Fields.Append("doc_no,");
            Head_Values.Append("'" + Doc_No + "',");
            Head_Fields.Append("doc_date,");
            Head_Values.Append("'" + doc_date + "',");
            Head_Fields.Append("cashbank,");
            Head_Values.Append("'" + cashbank + "',");
            Head_Fields.Append("total,");
            Head_Values.Append("'" + total + "',");
            Head_Fields.Append("company_code,");
            Head_Values.Append("'" + Company_Code + "',");
            Head_Fields.Append("year_code,");
            Head_Values.Append("'" + Year_Code + "',");
            Head_Fields.Append("cb,");
            Head_Values.Append("case when 0='" + cb + "' then null else '" + cb + "' end,");
            Head_Fields.Append("tranid");
            Head_Values.Append("'" + Receiptid + "'");
            #endregion

            Head_Insert = "insert into  " + tblHead + " (" + Head_Fields + ") values (" + Head_Values + ")";

            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = Head_Insert;
            Maindt.Rows.Add(dr);

            #region Detail Part
            int ReceiptPayment_ID = Convert.ToInt32(clsCommon.getString("select ifnull(count(trandetailid),0) as trandetailid from " + tblDetails + " "));
            if (ReceiptPayment_ID == 0)
            {
                ReceiptPayment_ID = 1;
            }
            else
            {
                ReceiptPayment_ID = Convert.ToInt32(clsCommon.getString("select max(trandetailid) as trandetailid from " + tblDetails + " "));
            }

            #region Main Logic
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                ReceiptPayment_ID = ReceiptPayment_ID + 1;
                drpFilterValue = Server.HtmlDecode(grdDetail.Rows[i].Cells[Grid_drpFilterValue].Text);
                AcCode = grdDetail.Rows[i].Cells[3].Text != string.Empty ? Convert.ToInt32(grdDetail.Rows[i].Cells[Grid_AcCode].Text) : 0;
                Unit_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[Grid_Unit_Code].Text);
                //Int32 acCode = Convert.ToInt32(grdDetail.Rows[i].Cells[3].Text);
                DetailID = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);

                //if (drpFilterValue == "V" || drpFilterValue == "T" || drpFilterValue == "D" || drpFilterValue == "P")
                //{
                try
                {
                    Voucher_No = Convert.ToInt32(Server.HtmlDecode(grdDetail.Rows[i].Cells[Grid_Voucher_No].Text));
                }
                catch
                {
                    Voucher_No = 0;
                }
                Voucher_Type = Server.HtmlDecode(grdDetail.Rows[i].Cells[Grid_Voucher_Type].Text);
                //}
                //if (drpFilterValue == "S" || drpFilterValue == "M")
                //{
                try
                {
                    TenderNo = Convert.ToInt32(Server.HtmlEncode(grdDetail.Rows[i].Cells[Grid_TenderNo].Text));
                }
                catch
                {
                    TenderNo = 0;
                }
                try
                {
                    TenderDetailId = Convert.ToInt32(Server.HtmlEncode(grdDetail.Rows[i].Cells[Grid_DetailID].Text));
                }
                catch
                {
                    TenderDetailId = 0;
                }
                //}

                try
                {
                    amount = Convert.ToDouble(grdDetail.Rows[i].Cells[Grid_amount].Text);
                }
                catch
                {
                    amount = 0.00;
                }

                try
                {
                    Adjusted_Amount = Convert.ToDouble(grdDetail.Rows[i].Cells[Grid_Adjusted_Amount].Text);
                }
                catch
                {
                    Adjusted_Amount = 0.00;
                }
                narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[Grid_narration].Text);
                narration2 = Server.HtmlDecode(grdDetail.Rows[i].Cells[Grid_narration2].Text);
                // Branch_name = Server.HtmlDecode(grdDetail.Rows[i].Cells[Grid_Branch_name].Text);
                YearCodeDetail = Convert.ToInt32(Server.HtmlDecode(grdDetail.Rows[i].Cells[Grid_YearCodeDetail].Text));
                int ca = 0;
                try
                {
                    ca = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code=" + AcCode + " and Company_Code=" + Company_Code + ""));

                }
                catch
                {
                }
                int uc = 0;
                try
                {
                    uc = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code=" + Unit_Code + " and Company_Code=" + Company_Code + ""));
                }
                catch
                {
                }
                // int sbid = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + Voucher_No + " and Company_Code=" + Company_Code + ""));
                //int ca = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + AcCode + " and Company_Code=" + Company_Code + ""));

                Detail_Values.Append("('" + transaction_type + "','" + Doc_No + "','" + doc_date + "','" + DetailID + "','" + cashbank + "','" + AcCode + "'," +
                " '" + Unit_Code + "','" + amount + "','" + narration + "','" + narration2 + "','" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "'," +
                " '" + Created_By + "','" + Voucher_No + "','" + Adjusted_Amount + "','" + TenderNo + "','" + TenderDetailId + "','" + drpFilterValue + "'," +
                " '" + YearCodeDetail + "','" + Receiptid + "',case when 0='" + ca + "' then null else '" + ca + "' end, " +
                " case when 0='" + uc + "' then null else '" + uc + "' end,'" + ReceiptPayment_ID + "'),");

            }
            #endregion
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
            //Thread thred = new Thread(() => { count = DataStore(flag); }); //Calling DataStore Method Using Thread
            //thred.Start(); //Thread Operation Start
            //thred.Join();

        }
        else
        {
            #region Head Update
            //Head_Update = "tran_type=";
            //Head_Update = "'" + transaction_type + "',";
            //Head_Update.Append( "doc_no=";
            //Head_Update.Append( "'" + Doc_No + "',";
            Head_Update.Append("doc_date=");
            Head_Update.Append("'" + doc_date + "',");
            Head_Update.Append("cashbank=");
            Head_Update.Append("'" + cashbank + "',");
            Head_Update.Append("total=");
            Head_Update.Append("'" + total + "',");
            //Head_Update.Append( "company_code=");
            //Head_Update.Append( "'" + Company_Code + "',");
            //Head_Update.Append( "year_code=");
            //Head_Update.Append( "'" + Year_Code + "',");
            Head_Update.Append("cb=");
            Head_Update.Append("case when 0='" + cb + "' then null else '" + cb + "' end");
            //Head_Update.Append( "Modified_By=");
            //Head_Update.Append( "'" + Modified_By + "'");
            //Head_Update.Append( "tranid=");
            //Head_Update.Append( "'" + Receiptid + "'");
            #endregion
            string Head_Updateqry = "update " + tblHead + " set " + Head_Update + " where tranid=" + lblReceiptPayment_Id.Text + "";

            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = Head_Updateqry;
            Maindt.Rows.Add(dr);
            string concatid = string.Empty;

            int ReceiptPayment_ID = Convert.ToInt32(clsCommon.getString("select ifnull(count(trandetailid),0) as trandetailid from " + tblDetails + " "));
            if (ReceiptPayment_ID == 0)
            {
                ReceiptPayment_ID = 1;
            }
            else
            {
                ReceiptPayment_ID = Convert.ToInt32(clsCommon.getString("select max(trandetailid) as trandetailid from " + tblDetails + " "));
            }

            #region Grid Update
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                #region
                drpFilterValue = Server.HtmlDecode(grdDetail.Rows[i].Cells[Grid_drpFilterValue].Text);
                AcCode = grdDetail.Rows[i].Cells[3].Text != string.Empty ? Convert.ToInt32(grdDetail.Rows[i].Cells[Grid_AcCode].Text) : 0;
                Unit_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[Grid_Unit_Code].Text);
                //Int32 acCode = Convert.ToInt32(grdDetail.Rows[i].Cells[3].Text);
                DetailID = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);

                //if (drpFilterValue == "V" || drpFilterValue == "T" || drpFilterValue == "D" || drpFilterValue == "P")
                //{
                try
                {
                    Voucher_No = Convert.ToInt32("0" + Server.HtmlDecode(grdDetail.Rows[i].Cells[Grid_Voucher_No].Text));
                }
                catch
                {
                    Voucher_No = 0;
                }
                Voucher_Type = Server.HtmlDecode(grdDetail.Rows[i].Cells[Grid_Voucher_Type].Text);
                //}
                //if (drpFilterValue == "S" || drpFilterValue == "M")
                //{
                try
                {
                    TenderNo = Convert.ToInt32("0" + Server.HtmlEncode(grdDetail.Rows[i].Cells[Grid_TenderNo].Text));
                }
                catch
                {
                    TenderNo = 0;
                }
                try
                {
                    TenderDetailId = Convert.ToInt32("0" + Server.HtmlEncode(grdDetail.Rows[i].Cells[Grid_DetailID].Text));
                }
                catch
                {
                    TenderDetailId = 0;
                }

                trandetailid = Convert.ToInt32(Server.HtmlEncode(grdDetail.Rows[i].Cells[_Grid_trandetailid].Text));

                //}

                try
                {
                    amount = Convert.ToDouble(grdDetail.Rows[i].Cells[Grid_amount].Text);
                }
                catch
                {
                    amount = 0.00;
                }

                try
                {
                    Adjusted_Amount = Convert.ToDouble(grdDetail.Rows[i].Cells[Grid_Adjusted_Amount].Text);
                }
                catch
                {
                    Adjusted_Amount = 0.00;
                }
                narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[Grid_narration].Text);
                narration2 = Server.HtmlDecode(grdDetail.Rows[i].Cells[Grid_narration2].Text);
                // Branch_name = Server.HtmlDecode(grdDetail.Rows[i].Cells[Grid_Branch_name].Text);
                YearCodeDetail = Convert.ToInt32(Server.HtmlDecode(grdDetail.Rows[i].Cells[Grid_YearCodeDetail].Text));
                int ca = 0;
                int uc = 0;
                try
                {
                    ca = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + AcCode + " and Company_Code=" + Company_Code + ""));
                }
                catch
                {
                }
                try
                {
                    uc = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + Unit_Code + " and Company_Code=" + Company_Code + ""));
                }
                catch
                {
                }
                // int sbid = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + Voucher_No + " and Company_Code=" + Company_Code + ""));
                //int ca = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + AcCode + " and Company_Code=" + Company_Code + ""));
                #endregion
                #region Insert Record
                if (grdDetail.Rows[i].Cells[Grid_rowAction].Text == "A")
                {
                    ReceiptPayment_ID = ReceiptPayment_ID + 1;
                    Detail_Values.Append("('" + transaction_type + "','" + txtdoc_no.Text + "','" + doc_date + "','" + DetailID + "','" + cashbank + "','" + AcCode + "'," +
                    " '" + Unit_Code + "','" + amount + "','" + narration + "','" + narration2 + "','" + Company_Code + "','" + Year_Code + "','" + Branch_Code + "'," +
                    " '" + Created_By + "','" + Voucher_No + "','" + Adjusted_Amount + "','" + TenderNo + "','" + TenderDetailId + "','" + drpFilterValue + "'," +
                    " '" + YearCodeDetail + "','" + lblReceiptPayment_Id.Text + "',case when 0='" + ca + "' then null else '" + ca + "' end, " +
                " case when 0='" + uc + "' then null else '" + uc + "' end,'" + ReceiptPayment_ID + "'),");
                }
                #endregion
                #region Update
                else if (grdDetail.Rows[i].Cells[Grid_rowAction].Text == "U")
                {
                    Detail_Update.Append("doc_date=case trandetailid when '" + trandetailid + "' then '" + doc_date + "'  ELSE doc_date END,");
                    Detail_Update.Append("debit_ac=case trandetailid when '" + trandetailid + "' then '" + cashbank + "'  ELSE debit_ac END,");
                    Detail_Update.Append("credit_ac=case trandetailid when '" + trandetailid + "' then '" + AcCode + "'  ELSE credit_ac END,");
                    Detail_Update.Append("Unit_Code=case trandetailid when '" + trandetailid + "' then '" + Unit_Code + "'  ELSE Unit_Code END,");
                    Detail_Update.Append("amount=case trandetailid when '" + trandetailid + "' then '" + amount + "'  ELSE amount END,");
                    Detail_Update.Append("narration=case trandetailid when '" + trandetailid + "' then '" + narration + "'  ELSE narration END,");
                    Detail_Update.Append("narration2=case trandetailid when '" + trandetailid + "' then '" + narration2 + "'  ELSE narration2 END,");
                    Detail_Update.Append("Modified_By=case trandetailid when '" + trandetailid + "' then '" + Modified_By + "'  ELSE Modified_By END,");

                    Detail_Update.Append("Voucher_No=case trandetailid when '" + trandetailid + "' then '" + Voucher_No + "'  ELSE Voucher_No END,");
                    Detail_Update.Append("Adjusted_Amount=case trandetailid when '" + trandetailid + "' then '" + Adjusted_Amount + "'  ELSE Adjusted_Amount END,");
                    Detail_Update.Append("Tender_No=case trandetailid when '" + trandetailid + "' then '" + TenderNo + "'  ELSE Tender_No END,");
                    Detail_Update.Append("drpFilterValue=case trandetailid when '" + trandetailid + "' then '" + drpFilterValue + "'  ELSE drpFilterValue END,");
                    Detail_Update.Append("YearCodeDetail=case trandetailid when '" + trandetailid + "' then '" + YearCodeDetail + "'  ELSE YearCodeDetail END,");
                    Detail_Update.Append("ca=case trandetailid when '" + trandetailid + "' then case when 0='" + ca + "' then null else '" + ca + "' end  ELSE ca END,");
                    Detail_Update.Append("uc=case trandetailid when '" + trandetailid + "' then  case when 0='" + uc + "' then null else '" + uc + "' end  ELSE uc END,");

                    concatid = concatid + "'" + trandetailid + "',";

                }
                #endregion
                #region Delete
                else
                {
                    if (grdDetail.Rows[i].Cells[Grid_rowAction].Text == "D")
                    {
                        Detail_Delete.Append("'" + grdDetail.Rows[i].Cells[_Grid_trandetailid].Text + "',");
                    }
                }
                #endregion
            }
            #endregion
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
                string Detail_Deleteqry = "delete from " + tblDetails + " where trandetailid in(" + Detail_Delete + ")";

                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Detail_Deleteqry;
                Maindt.Rows.Add(dr);
            }
            if (Detail_Update.Length > 0)
            {
                concatid = concatid.Remove(concatid.Length - 1);
                Detail_Update.Remove(Detail_Update.Length - 1, 1);
                string Detail_Updateqry = "update " + tblDetails + " set " + Detail_Update + " where trandetailid in(" + concatid + ")";

                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Detail_Updateqry;
                Maindt.Rows.Add(dr);
            }
            flag = 2;
            Doc_No = Convert.ToInt32(txtdoc_no.Text);

        }
        #region Gledger Effect
        FormTypes types = new FormTypes();

        string drcr = string.Empty;
        string drcrGrid = string.Empty;
        if (trntype == "CP" || trntype == "BP")
        {
            drcr = "C";
            drcrGrid = "D";
        }
        else
        {
            drcr = "D";
            drcrGrid = "C";
        }

        StringBuilder Gledger_values = new StringBuilder();
        GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='" + trntype + "' and Doc_No=" + Doc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
        dr = null;
        dr = Maindt.NewRow();
        dr["Querys"] = GLEDGER_Delete;
        Maindt.Rows.Add(dr);

        StringBuilder Gledger_Column = new StringBuilder();
        Gledger_Column.Append("TRAN_TYPE,CASHCREDIT,DOC_NO,DOC_DATE,AC_CODE,UNIT_code,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE," +
                    " YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO,ac,vc,progid,tranid");
        int accoid = 0;
        int Order_Code = 0;
        for (int i = 0; i < grdDetail.Rows.Count; i++)
        {
            AcCode = grdDetail.Rows[i].Cells[3].Text != string.Empty ? Convert.ToInt32(grdDetail.Rows[i].Cells[Grid_AcCode].Text) : 0;
            if (AcCode != 0)
            {
                accoid = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code=" + AcCode + " and Company_Code=" + Company_Code + ""));
            }
            try
            {
                amount = Convert.ToDouble(grdDetail.Rows[i].Cells[Grid_amount].Text);
            }
            catch
            {
                amount = 0.00;
            }
            Order_Code = Order_Code + 1;
            Gledger_values.Append("('" + trntype + "','','" + Doc_No + "','" + doc_date + "','" + cashbank + "','0','" + narration + "','" + amount + "', " +
                                                   " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','" + drcr + "',0,0,'" + Branch_Code + "','" + trntype + "','" + Doc_No + "'," +
                                                   " case when 0='" + cb + "' then null else '" + cb + "' end ,'0','" + types.TT_ReceiptPayment + "','0'),");
            Order_Code = Order_Code + 1;
            Gledger_values.Append("('" + trntype + "','','" + Doc_No + "','" + doc_date + "','" + AcCode + "','0','" + narration + "','" + amount + "', " +
                                                  " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','" + drcrGrid + "',0,0,'" + Branch_Code + "','" + trntype + "','" + Doc_No + "'," +
                                                  " case when 0='" + accoid + "' then null else '" + accoid + "' end,'0','" + types.TT_ReceiptPayment + "','0'),");
        }

        if (Gledger_values.Length > 0)
        {
            Gledger_values.Remove(Gledger_values.Length - 1, 1);
            GLEDGER_Insert = "insert into nt_1_gledger (" + Gledger_Column + ") values " + Gledger_values + " ";

            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = GLEDGER_Insert;
            Maindt.Rows.Add(dr);
        }
        #endregion

        //  msg = clsDAL.DataStore(Head_Insert, Head_Update, Head_Delete, Detail_Insert, Detail_Update, Detail_Delete, GLEDGER_Insert, GLEDGER_Delete, flag);
        msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);
        if (msg == "Insert")
        {
            hdnf.Value = Receiptid.ToString();
            clsButtonNavigation.enableDisable("S");
            this.makeEmptyForm("S");
            qry = getDisplayQuery();
            this.fetchRecord(qry);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Added !')", true);
        }
        else if (msg == "Update")
        {
            hdnf.Value = lblReceiptPayment_Id.Text;
            clsButtonNavigation.enableDisable("S");
            this.makeEmptyForm("S");
            qry = getDisplayQuery();
            this.fetchRecord(qry);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update !')", true);
        }

    }

    #endregion


    #region csCalculations
    private void csCalculations()
    {
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

                            string qry = "select * from " + tblHead + " where   Doc_No='" + txtValue + "' " +
                                "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trntype + "'";

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
                                                txtdoc_no.Enabled = false;

                                                btnSave.Enabled = true;   //IMP
                                                setFocusControl(txtdoc_date);
                                            }

                                            if (ViewState["mode"].ToString() == "U")
                                            {
                                                //fetch record
                                                qry = getDisplayQuery();
                                                bool recordExist = this.fetchRecord(qry);
                                                if (recordExist == true)
                                                {
                                                    txtdoc_no.Enabled = false;
                                                    pnlgrdDetail.Enabled = true;
                                                }
                                            }
                                        }
                                    }
                                    else   //Record Not Found
                                    {
                                        if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                        {
                                            lblMsg.Text = "";
                                            setFocusControl(txtdoc_date);
                                            txtdoc_no.Enabled = false;
                                            btnSave.Enabled = true;   //IMP
                                        }
                                        if (ViewState["mode"].ToString() == "U")
                                        {
                                            this.makeEmptyForm("E");
                                            lblMsg.Text = "** Record Not Found";
                                            lblMsg.ForeColor = System.Drawing.Color.Red;
                                            txtdoc_no.Text = string.Empty;
                                            setFocusControl(txtdoc_no);
                                            pnlgrdDetail.Enabled = true;
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
            }
            if (strTextBox == "txtdoc_date")
            {
                if (txtdoc_date.Text != string.Empty)
                {
                    //string dt = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    //if (clsCommon.isValidDate(dt) == true)
                    //{
                    //    //setFocusControl(btnOpenDetailsPopup);
                    //}
                    //else
                    //{
                    //    txtdoc_date.Text = string.Empty;
                    //    setFocusControl(txtdoc_date);
                    //}
                }
                else
                {
                    setFocusControl(txtdoc_date);
                }
            }
            if (strTextBox == "txtACCode")
            {
                if (txtACCode.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtACCode.Text);
                    if (a == false)
                    {
                        btntxtACCode_Click(this, new EventArgs());
                    }
                    else
                    {
                        string str = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (str != string.Empty && str != "0")
                        {
                            lblACName.Text = str;
                            string ac_city = clsCommon.getString("select CityName from " + qryAccountList + "  where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            txtnarration.Text = ac_city;
                            if (drpFilter.SelectedValue == "S")
                            {
                                setFocusControl(txtUnit_Code);
                            }
                            else
                            {
                                setFocusControl(txtUnit_Code);
                            }

                        }
                        else
                        {
                            lblACName.Text = string.Empty;
                            txtACCode.Text = string.Empty;
                            setFocusControl(txtACCode);
                        }
                    }
                }
                else
                {
                    lblACName.Text = string.Empty;
                    txtACCode.Text = string.Empty;
                    setFocusControl(txtACCode);

                }
            }
            if (strTextBox == "txtUnit_Code")
            {
                string acname = "";
                if (txtUnit_Code.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtUnit_Code.Text);
                    if (a == false)
                    {
                        btntxtUnitcode_Click(this, new EventArgs());
                    }
                    else
                    {
                        string iscarporate = clsCommon.getString("select carporate_party from " + qryAccountList + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (iscarporate == "Y")
                        {
                            lblMsg.Text = "";
                            lblPopupHead.Text = "--Select Unit--";
                            //string qry = "select UnitName from " + qryAccountList + "  where Ac_Code=" + txtACCode.Text +
                            //    " and Unit_name=" + txtUnit_Code.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
                            string qry = "select Ac_Name_E from " + qryAccountList + "  where Ac_Code=" + txtUnit_Code.Text +
                               "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
                            acname = clsCommon.getString(qry);
                            if (acname != string.Empty && acname != "0")
                            {
                                lblUnitName.Text = acname;
                                if (txtVoucherNo.Enabled == true)
                                {
                                    setFocusControl(drpFilter);
                                }
                                else
                                {
                                    if (txtVoucherNo.Enabled == false)
                                    {
                                        setFocusControl(txtamount);
                                    }
                                    else
                                    {
                                        setFocusControl(drpFilter);
                                    }
                                }
                            }
                            else
                            {
                                txtUnit_Code.Text = string.Empty;
                                lblUnitName.Text = acname;
                                if (txtVoucherNo.Enabled == false)
                                {
                                    setFocusControl(txtamount);
                                }
                                else
                                {
                                    setFocusControl(drpFilter);
                                }
                            }
                        }
                        else
                        {
                            string str = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtUnit_Code.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            if (str != string.Empty && str != "0")
                            {
                                lblUnitName.Text = str;
                                if (txtVoucherNo.Enabled == true)
                                {
                                    setFocusControl(drpFilter);
                                }
                                else
                                {
                                    setFocusControl(txtamount);
                                }
                            }
                            else
                            {
                                lblUnitName.Text = string.Empty;
                                txtUnit_Code.Text = string.Empty;
                                if (txtVoucherNo.Enabled == true)
                                {
                                    setFocusControl(txtVoucherNo);
                                }
                                else
                                {
                                    setFocusControl(txtamount);
                                }

                            }
                        }
                    }
                }
                else
                {
                    setFocusControl(drpFilter);
                }
            }
            if (strTextBox == "txtCashBank")
            {
                if (txtCashBank.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtCashBank.Text);
                    if (a == false)
                    {
                        btntxtCashBank_Click(this, new EventArgs());
                    }
                    else
                    {
                        string str = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtCashBank.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (str != string.Empty && str != "0")
                        {
                            lblCashBank.Text = str;
                            setFocusControl(txtACCode);
                        }
                        else
                        {
                            lblCashBank.Text = string.Empty;
                            txtCashBank.Text = string.Empty;
                            setFocusControl(txtCashBank);
                        }
                    }
                }
                else
                {
                    lblCashBank.Text = string.Empty;
                    txtCashBank.Text = string.Empty;
                    setFocusControl(txtCashBank);

                }
            }
            //string ac_city = clsCommon.getString("select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (strTextBox == "txtVoucherNo")
            {
                setFocusControl(txtamount);
                DataSet ds;
                if (drpFilter.SelectedValue == "V")
                {
                    if (txtVoucherNo.Text != string.Empty)
                    {
                        qry = "";// "select * from " + tblPrefix + "Voucher where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Doc_No=" + txtVoucherNo.Text + " and Tran_Type='" + txtvoucherType.Text + "'";
                        qry = "select Doc_No as doc_no,Tran_Type,Suffix,Convert(varchar(10),Doc_Date,103) as Doc_Date,PartyName,Unit_Name,NETQNTL,BrokerName,Sale_Rate,Bill_Amount,mill_code, " +
                              " (Select ISNULL(SUM(amount),0) as UA from " + tblPrefix + "Transact where Voucher_No=" + tblPrefix + "qryVoucherSaleUnion.Doc_No and Voucher_Type=" + tblPrefix + "qryVoucherSaleUnion.Tran_Type and Company_Code=" + tblPrefix + "qryVoucherSaleUnion.Company_Code and Year_Code=" + tblPrefix + "qryVoucherSaleUnion.Year_Code" +
                              " ) as Paid_Amount,(Bill_Amount - (Select ISNULL(SUM(amount),0) as UA from " + tblPrefix + "Transact where " +
                              " Voucher_No=" + tblPrefix + "qryVoucherSaleUnion.Doc_No and Voucher_Type=" + tblPrefix + "qryVoucherSaleUnion.Tran_Type and Year_Code=" + tblPrefix + "qryVoucherSaleUnion.Year_Code and Company_Code=" + tblPrefix + "qryVoucherSaleUnion.Company_Code )) as Balance " +
                              " from " + tblPrefix + "qryVoucherSaleUnion where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "" +
                              "  and doc_no=" + txtVoucherNo.Text + " and Tran_Type='" + txtvoucherType.Text + "'";

                        ds = new DataSet();
                        ds = clsDAL.SimpleQuery(qry);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string VocNo = "Vouch No:" + txtVoucherNo.Text + "(" + txtvoucherType.Text + ")";
                            string millcode = ds.Tables[0].Rows[0]["mill_code"].ToString();
                            string millshortname = clsCommon.getString("Select Short_Name from " + tblPrefix + "AccountMaster where Ac_Code=" + millcode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            double Qntl = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["NETQNTL"].ToString())), 2);
                            double SR = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["Sale_Rate"].ToString())), 2);
                            //double frt = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["FreightPerQtl"].ToString())), 2);
                            //string naration = VocNo + " " + millshortname + "-" + Qntl + "-(SR:" + SR + "-frt:" + frt + ")";
                            string naration = VocNo + " " + millshortname + "-" + Qntl + "-(SR:" + SR + ")";
                            txtnarration.Text = naration;

                            hdnfTransportBalance.Value = ds.Tables[0].Rows[0]["Balance"].ToString();
                        }
                        else
                        {
                            txtnarration.Text = "";
                        }
                    }

                }
                if (drpFilter.SelectedValue == "D")
                {
                    if (txtVoucherNo.Text != string.Empty)
                    {
                        qry = "select Doc_No,MillName,Date,PartyName,Sale_Rate,Purchase_Rate,Quantal,Balance from NT_1_qryDebitNotesForBankReciept where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "" +
                              "  and Doc_No=" + txtVoucherNo.Text + " and Tran_Type='LV'";

                        ds = new DataSet();
                        ds = clsDAL.SimpleQuery(qry);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string VocNo = "Vouch No:" + txtVoucherNo.Text + "(LV)";
                            string millshortname = ds.Tables[0].Rows[0]["MillName"].ToString();
                            double Qntl = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["Quantal"].ToString())), 2);
                            double SR = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["Sale_Rate"].ToString())), 2);
                            //double frt = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["FreightPerQtl"].ToString())), 2);
                            //string naration = VocNo + " " + millshortname + "-" + Qntl + "-(SR:" + SR + "-frt:" + frt + ")";
                            string naration = VocNo + " " + millshortname + "-" + Qntl + "-(SR:" + SR + ")";
                            txtnarration.Text = naration;
                            hdnfTransportBalance.Value = ds.Tables[0].Rows[0]["Balance"].ToString();
                        }
                        else
                        {
                            txtnarration.Text = "";
                        }
                    }
                }
                if (drpFilter.SelectedValue == "S")
                {
                    string qry = "";  // "Select * from " + tblPrefix + "qryTenderList where Tender_No=" + txtVoucherNo.Text + " and ID=" + txtvoucherType.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

                    qry = "select [Tender_No],[ID],Convert(VarChar(10),[Tender_Date],103) as Tender_Date,[millname],[salerate],[salepartyfullname],[Buyer_Quantal],[salevalue],[received],[balance],[Commission_Rate] from " + tblPrefix + "qrySaudaBalance" +
                       " where [Company_Code]=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and [Year_Code]=" + Convert.ToInt32(Session["year"].ToString()) + " and Tender_No=" + txtVoucherNo.Text + " and ID=" + txtvoucherType.Text + "";
                    ds = new DataSet();
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string millshortname = ds.Tables[0].Rows[0]["millname"].ToString();
                        //string millshortname = clsCommon.getString("Select Short_Name from " + tblPrefix + "AccountMaster where Ac_Code=" + millcode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        double Qntl = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["Buyer_Quantal"].ToString())), 2);
                        double SR = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["salerate"].ToString())), 2);
                        double Commission = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["Commission_Rate"].ToString())), 2);
                        double Sale_Rate = SR - Commission;
                        string SaudaDate = ds.Tables[0].Rows[0]["Tender_Date"].ToString();
                        txtnarration.Text = millshortname + " " + Qntl + "(SR:" + Sale_Rate + "+Comm:" + Commission + ")dt-" + SaudaDate;
                        hdnfTransportBalance.Value = ds.Tables[0].Rows[0]["balance"].ToString();
                    }
                }

                if (drpFilter.SelectedValue == "P")
                {
                    string qry = "";  // "Select * from " + tblPrefix + "qryTenderList where Tender_No=" + txtVoucherNo.Text + " and ID=" + txtvoucherType.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

                    //qry = "select [Tender_No],[ID],Convert(VarChar(10),[Tender_Date],103) as Tender_Date,[millname],[salerate],[salepartyfullname],[Buyer_Quantal],[salevalue],[received],[balance],[Commission_Rate] from " + tblPrefix + "qrySaudaBalance" +
                    //   " where [Company_Code]=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and [Year_Code]=" + Convert.ToInt32(Session["year"].ToString()) + " and Tender_No=" + txtVoucherNo.Text + " and ID=" + txtvoucherType.Text + "";

                    qry = "select doc_no,Grand_Total,Party_Code,balance,amount from " + tblPrefix + "qryAgainstCreditBill" +
                      " where balance<>0 and[Company_Code]=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and [Year_Code]=" + Convert.ToInt32(Session["year"].ToString()) +
                      " and doc_no=" + txtVoucherNo.Text + " and Voucher_Type='" + txtvoucherType.Text + "'";
                    ds = new DataSet();
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtamount.Text = ds.Tables[0].Rows[0]["balance"].ToString();
                        //string millshortname = ds.Tables[0].Rows[0]["millname"].ToString();
                        //string millshortname = clsCommon.getString("Select Short_Name from " + tblPrefix + "AccountMaster where Ac_Code=" + millcode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //double billamount = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[0]["Buyer_Quantal"].ToString())), 2);
                        hdnfTransportBalance.Value = ds.Tables[0].Rows[0]["balance"].ToString();
                    }
                }

                if (drpFilter.SelectedValue == "T")
                {
                    string qry = ""; // "Select * from " + tblPrefix + "qryDeliveryOrderListReport where doc_no=" + txtVoucherNo.Text.Trim() + " and tran_type='" + txtvoucherType.Text.Trim() + "' and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

                    qry = "select do.doc_no,do.voucher_no,do.quantal,do.tran_type,Convert(varchar(10),do.doc_date,103) as date,do.millName as Mill,do.truck_no,do.TransportName,do.Memo_Advance," +
                         " (Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code ) as Paid,((do.Memo_Advance)-" +
                         "(Select (do.vasuli_amount+ISNULL(SUM(amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code)) as Balance" +
                         " from " + tblPrefix + "qryDeliveryOrderListReport do where tran_type='DO' and (do.Memo_Advance-do.vasuli_amount)!=0 and do.doc_no=" + txtVoucherNo.Text.Trim() + " and do.tran_type='" + txtvoucherType.Text.Trim() + "' and do.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and do.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                    ds = new DataSet();
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string vn = "";
                        string VoucherNo = ds.Tables[0].Rows[0]["voucher_no"].ToString();
                        string lorry = ds.Tables[0].Rows[0]["truck_no"].ToString();
                        string qntl = ds.Tables[0].Rows[0]["quantal"].ToString();
                        string Memo_Advance = ds.Tables[0].Rows[0]["Memo_Advance"].ToString();
                        if (VoucherNo != "0")
                        {
                            vn = "Vouc.No.:" + VoucherNo;
                        }
                        string ac_city = clsCommon.getString("select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string narration = vn + " Lorry:" + lorry + " Qntl:" + qntl + "-" + Memo_Advance + "(" + ac_city + ")";
                        txtnarration.Text = narration;
                        hdnfTransportBalance.Value = ds.Tables[0].Rows[0]["Balance"].ToString();
                        setFocusControl(txtamount);
                    }
                }

                setFocusControl(txtamount);
            }
            if (strTextBox == "txtamount")
            {
                if (txtVoucherNo.Text != string.Empty)
                {
                    string hdnfVal = hdnfTransportBalance.Value.ToString().TrimStart();
                    if (!string.IsNullOrEmpty(hdnfVal))
                    {
                        double TransportAdvanceBalance = hdnfVal != string.Empty ? Convert.ToDouble(hdnfVal) : 0.00;
                        double amount1 = Convert.ToDouble(txtamount.Text);
                        if (amount1 > TransportAdvanceBalance)
                        {
                            lblErrorAdvance.Text = "Amount Is Greater Than Transport Advance Balance!";
                            setFocusControl(txtamount);
                        }
                        else
                        {
                            lblErrorAdvance.Text = "";
                        }
                    }
                }
                setFocusControl(txtadAmount);
            }
            if (strTextBox == "txtadAmount")
            {
                setFocusControl(txtnarration);
            }
            if (strTextBox == "txtnarration")
            {
                setFocusControl(txtNarration2);
            }
            if (strTextBox == "txtNarration2")
            {
                setFocusControl(btnAdddetails);
            }

            this.columnTotal();
        }
        catch
        {
        }
    }

    private void columnTotal()
    {
        #region Calculation Part
        double total = 0.00;
        if (grdDetail.Rows.Count > 0)
        {

            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                if (grdDetail.Rows[i].Cells[Grid_rowAction].Text != "D")
                {
                    double Amt = Convert.ToDouble(grdDetail.Rows[i].Cells[11].Text);
                    total = total + Amt;
                }
            }
        }
        txtTotal.Text = Math.Round(total, 2).ToString();
        #endregion
    }
    #endregion

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (searchString != string.Empty)// && strTextBox == hdnfClosePopup.Value)
            {
                txtSearchText.Text = searchString;
            }
            else
            {
                txtSearchText.Text = txtSearchText.Text;
            }
            if (hdnfClosePopup.Value == "txtdoc_no" || hdnfClosePopup.Value == "txtEditDoc_No")
            {
                if (btntxtdoc_no.Text == "Change No")
                {
                    pnlPopup.Style["display"] = "none";
                    txtdoc_no.Text = string.Empty;
                    txtdoc_no.Enabled = true;
                    btnSave.Enabled = false;
                    setFocusControl(txtdoc_no);
                    hdnfClosePopup.Value = "Close";
                }
                else
                {
                    lblPopupHead.Text = "--Select DOC No--";
                    string qry = "select doc_no,doc_date,debitAcName from " + qryCommon + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'" +
                    " and  (doc_no like '%" + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%') group by doc_no,doc_date,debitAcName order by doc_no";
                    this.showPopup(qry);
                }
            }
            if (hdnfClosePopup.Value == "txtACCode")
            {
                lblPopupHead.Text = "--Select AC Code--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                    " (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName  like '%" + txtSearchText.Text + "%' ) ORDER BY Ac_Name_E asc";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtUnit_Code")
            {
                if (txtACCode.Text != string.Empty)
                {
                    string iscarporate = clsCommon.getString("select carporate_party from " + AccountMasterTable + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (iscarporate == "Y")
                    {
                        lblMsg.Text = "";
                        lblPopupHead.Text = "--Select Unit--";
                        string qry = "select Unit_name,UnitName,unitCity from " + tblPrefix + "qryPartyUnitlist where Ac_Code=" + txtACCode.Text +
                            " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (Unit_name like '%" + txtSearchText.Text + "%' or UnitName like '%" + txtSearchText.Text + "%' or unitCity like '%" + txtSearchText.Text + "%') order by UnitName";
                        this.showPopup(qry);
                    }
                    else
                    {
                        lblPopupHead.Text = "--Select Unit Code--";
                        string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E asc";
                        this.showPopup(qry);
                    }
                }
                else
                {
                    setFocusControl(txtACCode);
                }
            }
            if (hdnfClosePopup.Value == "txtCashBank")
            {
                string qry = "";
                lblPopupHead.Text = "--Select Cash/Bank--";
                if (drpTrnType.SelectedValue == "CP")
                {
                    qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                    " Ac_type='C' and  (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName  like '%" + txtSearchText.Text + "%' ) ORDER BY Ac_Name_E asc";
                }
                else
                {
                    qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                       " Ac_type='B' and  (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName  like '%" + txtSearchText.Text + "%' ) ORDER BY Ac_Name_E asc";
                }
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtVoucherNo")
            {
                string qry = "";
                if (drpTrnType.SelectedValue == "BR" && drpFilter.SelectedValue == "S")
                {

                    lblPopupHead.Text = "--Select Cash/Bank--";

                    qry = "select Tender_No,payment_dateConverted as date,buyername,Buyer_Quantal as qty,Sale_Rate,round(AMT) as AMT,received,adjusted as Adj_Amt,round(BALANCE) as BALANCE,tenderdetailid,Year_Code " +
                        " from qrysaudabalance where Buyer='" + txtACCode.Text + "' and Company_Code='" + Session["Company_Code"].ToString() + "' and Year_Code='" + Session["year"].ToString() + "' and " +
                        " (Buyer like '%" + txtSearchText.Text + "%' or buyername like '%" + txtSearchText.Text + "%' ) order by buyername";


                }
                this.showPopup(qry);
                #region
                //if (drpFilter.SelectedValue == "V")
                //{
                //    lblPopupHead.Text = "--Select Voucher No--";
                //    string qry = "select Doc_No as doc_no,Tran_Type,Suffix,Convert(varchar(10),Doc_Date,103) as Doc_Date,PartyName,Unit_Name,NETQNTL,BrokerName,Sale_Rate,Bill_Amount, " +
                //    " (Select ISNULL(SUM(amount) + SUM(Adjusted_Amount),0) as UA from " + tblPrefix + "Transact where Voucher_No=" + tblPrefix + "qryVoucherSaleUnion.Doc_No and Voucher_Type=" + tblPrefix + "qryVoucherSaleUnion.Tran_Type and Company_Code=" + tblPrefix + "qryVoucherSaleUnion.Company_Code and Year_Code=" + tblPrefix + "qryVoucherSaleUnion.Year_Code" +
                //    " ) as Paid_Amount,(Bill_Amount - (select ISNULL(SUM(Bill_Amount),0) as b from " + tblPrefix + "qrySugarPurcListReturn where PURCNO=" + tblPrefix + "qryVoucherSaleUnion.Doc_No and PurcTranType=" + tblPrefix + "qryVoucherSaleUnion.Tran_type and Company_Code=" + tblPrefix + "qryVoucherSaleUnion.Company_Code and Year_Code=" + tblPrefix + "qryVoucherSaleUnion.Year_Code) - (Select ISNULL(SUM(amount)+SUM(Adjusted_Amount),0) as UA from " + tblPrefix + "Transact where " +
                //    " Voucher_No=" + tblPrefix + "qryVoucherSaleUnion.Doc_No and Voucher_Type=" + tblPrefix + "qryVoucherSaleUnion.Tran_Type and Year_Code=" + tblPrefix + "qryVoucherSaleUnion.Year_Code and Company_Code=" + tblPrefix + "qryVoucherSaleUnion.Company_Code )) as Balance " +
                //    " from " + tblPrefix + "qryVoucherSaleUnion where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "" +
                //    "  and Ac_Code=" + txtACCode.Text + "";
                //    this.showPopup(qry);
                //}
                //if (drpFilter.SelectedValue == "M")
                //{
                //    lblPopupHead.Text = "--Select Sauda No--";
                //    string qry = "select [Tender_No],[ID],Convert(VarChar(10),[Tender_Date],103) as Tender_Date,[millname],[salerate],[salepartyfullname],[Buyer_Quantal],[salevalue],[received],[balance] from " + tblPrefix + "qrySaudaBalance" +
                //        " where [Company_Code]=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and [Year_Code]=" + Convert.ToInt32(Session["year"].ToString()) + " and [Buyer]=" + txtACCode.Text + "";
                //    this.showPopup(qry);
                //}


                //if (drpFilter.SelectedValue == "S")
                //{
                //    if (btnSave.Text == "Save")
                //    {
                //        lblPopupHead.Text = "--Select Sauda No--";
                //        string qry = "select [Tender_No],[ID],Convert(VarChar(10),[Lifting_Date],103) as Lifting_Date,[millname],[salerate],[buyerbrokerfullname],[Buyer_Quantal],[salevalue],[received],[balance], Year_Code from " + tblPrefix + "qrySaudaBalance" +
                //            " where balance!=0 and [Company_Code]=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and [Buyer]=" + txtACCode.Text + "";
                //        this.showPopup(qry);
                //    }
                //    else
                //    {
                //        lblPopupHead.Text = "--Select Sauda No--";
                //        string qry = "select [Tender_No],[ID],Convert(VarChar(10),[Lifting_Date],103) as Lifting_Date,[millname],[salerate],[buyerbrokerfullname],[Buyer_Quantal],[salevalue],[received],[balance], Year_Code from " + tblPrefix + "qrySaudaBalance" +
                //            " where [Company_Code]=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and [Buyer]=" + txtACCode.Text + "";
                //        this.showPopup(qry);
                //    }
                //}



                //if (drpFilter.SelectedValue == "D")
                //{
                //    lblPopupHead.Text = "--Select Debit Note No--";
                //    string qry = "select Doc_No,Tran_Type,Date,PartyName,Quantal,Amount,Received,Balance from NT_1_qryDebitNotesForBankReciept where [Company_Code]=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and [Year_Code]=" + Convert.ToInt32(Session["year"].ToString()) + " and Ac_Code=" + txtACCode.Text + "";
                //    this.showPopup(qry);
                //}

                //if (drpFilter.SelectedValue == "T")
                //{
                //    lblPopupHead.Text = "--Select Transport Account--";
                //    qry = "select do.doc_no,do.tran_type,Convert(varchar(10),do.doc_date,103) as date,do.millName as Mill,do.truck_no as Lorry,do.TransportName,do.Memo_Advance as Transport_Advance," +
                //          " (Select (do.vasuli_amount+ISNULL(SUM(amount)+SUM(Adjusted_Amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code ) as Paid,((do.Memo_Advance)-" +
                //          "(Select (do.vasuli_amount+ISNULL(SUM(amount)+SUM(Adjusted_Amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code)) as Balance" +
                //          " from " + tblPrefix + "qryDeliveryOrderListReport do where tran_type='DO' and (do.Memo_Advance-do.vasuli_amount)!=0 and "+
                //    " do.transport=" + txtACCode.Text + " and (do.doc_no like '%" + txtSearchText.Text + "%' or do.truck_no like '%" + txtSearchText.Text + "%' or "+
                //    " do.doc_date like '%" + txtSearchText.Text + "%') and do.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and "+
                //    " do.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                //    this.showPopup(qry);
                //}



                //if (drpFilter.SelectedValue == "P")
                //{
                //    lblPopupHead.Text = "--Select Credit Bill--";
                //    qry = "select distinct(doc_no),Tran_Type,Party_Code,partyname,doc_date,Grand_Total as Bill_amount from NT_1_qrySugarRetailSellList where Party_Code<>1 and Party_Code=" + txtACCode.Text + " and Company_Code="
                //        + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + " and (doc_no like '%" + txtSearchText.Text +
                //        "%' or partyname like '%" + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%')";
                //    //qry = "select do.doc_no,do.tran_type,Convert(varchar(10),do.doc_date,103) as date,do.millName as Mill,do.truck_no as Lorry,do.TransportName,do.Memo_Advance as Transport_Advance," +
                //    //      " (Select (do.vasuli_amount+ISNULL(SUM(amount)+SUM(Adjusted_Amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code ) as Paid,((do.Memo_Advance)-" +
                //    //      "(Select (do.vasuli_amount+ISNULL(SUM(amount)+SUM(Adjusted_Amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and Year_Code=do.Year_Code)) as Balance" +
                //    //      " from " + tblPrefix + "qryDeliveryOrderListReport do where tran_type='DO' and (do.Memo_Advance-do.vasuli_amount)!=0 and do.transport=" + txtACCode.Text + " and (do.doc_no like '%" + txtSearchText.Text + "%' or do.truck_no like '%" + txtSearchText.Text + "%' or do.doc_date like '%" + txtSearchText.Text + "%') and do.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and do.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                //    this.showPopup(qry);
                //}
                #endregion

            }
            if (hdnfClosePopup.Value == "txtVoucherNo1")
            {
                if (drpTrnType.SelectedValue == "BR" && drpFilter.SelectedValue == "S")
                {
                    qry = "select Tender_No,payment_dateConverted as date,buyername,Buyer_Quantal as qty,Sale_Rate,round(AMT) as AMT,received,adjusted as Adj_Amt,round(BALANCE) as BALANCE,tenderdetailid,Year_Code " +
                       " from qrysaudabalance where Company_Code='" + Session["Company_Code"].ToString() + "' and Year_Code='" + Session["year"].ToString() + "' and buyer!=2 order by buyername";


                }
                this.showPopup(qry);
                #region
                //if (drpFilter.SelectedValue == "S")
                //{
                //    if (btnSave.Text == "Save")
                //    {
                //        lblPopupHead.Text = "--Select Sauda No--";
                //        string qry = "select [Tender_No],[ID],Convert(VarChar(10),[Lifting_Date],103) as Lifting_Date,[millname]," +
                //        "[salerate],[buyerbrokerfullname],[Buyer_Quantal],[salevalue],[received],[balance], Year_Code from "
                //        + tblPrefix + "qrySaudaBalance" +
                //            " where balance!=0 and [Company_Code]="
                //            + Convert.ToInt32(Session["Company_Code"].ToString())
                //            + " and (buyerbrokerfullname like '%" + txtSearchText.Text + "%')";
                //        this.showPopup(qry);
                //    }
                //    else
                //    {
                //        lblPopupHead.Text = "--Select Sauda No--";
                //        string qry = "select [Tender_No],[ID],Convert(VarChar(10),[Lifting_Date],103) as Lifting_Date,[millname]," +
                //        "[salerate],[buyerbrokerfullname],[Buyer_Quantal],[salevalue],[received],[balance], Year_Code from " + tblPrefix
                //        + "qrySaudaBalance" +
                //            " where [Company_Code]="
                //            + Convert.ToInt32(Session["Company_Code"].ToString())
                //            + " and (buyerbrokerfullname like '%" + txtSearchText.Text + "%')";
                //        this.showPopup(qry);
                //    }
                //}
                #endregion
            }
            if (hdnfClosePopup.Value == "txtnarration")
            {
                lblPopupHead.Text = "--Select Narration--";
                string qry = "select System_Name_E as Narration from " + systemMasterTable + " where System_Type='N' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
        }
        catch
        {
        }
    }
    #endregion

    protected void drpTrnType_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnAdd.Focus();
        hdnfTran_type.Value = drpTrnType.SelectedValue;
        string max = clsCommon.getString("select ifnull(max(tranid),0) as id from " + tblHead + " where tran_type='" + hdnfTran_type.Value + "'");
        hdnf.Value = max;

        clsButtonNavigation.enableDisable("S");
        this.makeEmptyForm("S");
        this.showLastRecord();

        // this.NextNumber();
        //this.showLastRecord();
    }
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    }

    protected void drpFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (drpFilter.SelectedValue == "V" || drpFilter.SelectedValue == "T")
            {
                lblHead.Text = "Voucher Number";
                txtVoucherNo.Enabled = true;
                txtvoucherType.Enabled = true;
                btntxtVoucherNo.Enabled = true;
            }
            if (drpFilter.SelectedValue == "S" || drpFilter.SelectedValue == "M")
            {
                lblHead.Text = "Sauda Number";
                txtVoucherNo.Enabled = true;
                txtvoucherType.Enabled = true;
                btntxtVoucherNo.Enabled = true;
            }

            if (drpFilter.SelectedValue == "D")
            {
                lblHead.Text = "Debit Note Number";
                txtVoucherNo.Enabled = true;
                txtvoucherType.Enabled = true;
                btntxtVoucherNo.Enabled = true;
            }


            if (drpFilter.SelectedValue == "0")
            {
                txtVoucherNo.Enabled = false;
                txtvoucherType.Enabled = false;
                btntxtVoucherNo.Enabled = false;
            }

            if (drpFilter.SelectedValue == "P")
            {
                lblHead.Text = "Credit Number";
                txtVoucherNo.Enabled = true;
                txtvoucherType.Enabled = true;
                btntxtVoucherNo.Enabled = true;
            }
            setFocusControl(drpFilter);
        }
        catch
        {

        }
    }
    protected void grdPopup_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdPopup.SelectedIndex = -1;
            grdPopup.DataBind();
        }
        catch { }
    }
    protected void txtVoucherNo_TextChanged1(object sender, EventArgs e)
    {
        searchString = txtVoucherNo.Text;
        strTextBox = "txtVoucherNo";
        csCalculations();
    }
    protected void txtUnit_Code_TextChanged(object sender, EventArgs e)
    {
        searchString = txtUnit_Code.Text;
        strTextBox = "txtUnit_Code";
        csCalculations();
    }
    protected void btntxtUnitcode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtUnit_Code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }


    #region [btnsoudaall_Click]
    protected void btnsoudaall_Click(object sender, EventArgs e)
    {
        pnlPopup.Style["display"] = "block";
        //hdnfClosePopup.Value = "txtVoucherNo";
        hdnfClosePopup.Value = "txtVoucherNo1";


        btnSearch_Click(sender, e);


        //if (drpFilter.SelectedValue == "S")
        //{
        //    if (btnSave.Text == "Save")
        //    {
        //        lblPopupHead.Text = "--Select Sauda No--";
        //        string qry = "select [Tender_No],[ID],Convert(VarChar(10),[Lifting_Date],103) as Lifting_Date,[millname]," +
        //        "[salerate],[buyerbrokerfullname],[Buyer_Quantal],[salevalue],[received],[balance], Year_Code from "
        //        + tblPrefix + "qrySaudaBalance" +
        //            " where balance!=0 and [Company_Code]="
        //            + Convert.ToInt32(Session["Company_Code"].ToString())
        //            + " and (buyerbrokerfullname like '%" + txtSearchText.Text + "%')";
        //        this.showPopup(qry);
        //    }
        //    else
        //    {
        //        lblPopupHead.Text = "--Select Sauda No--";
        //        string qry = "select [Tender_No],[ID],Convert(VarChar(10),[Lifting_Date],103) as Lifting_Date,[millname]," +
        //        "[salerate],[buyerbrokerfullname],[Buyer_Quantal],[salevalue],[received],[balance], Year_Code from " + tblPrefix
        //        + "qrySaudaBalance" +
        //            " where [Company_Code]="
        //            + Convert.ToInt32(Session["Company_Code"].ToString())
        //            + " and (buyerbrokerfullname like '%" + txtSearchText.Text + "%')";
        //        this.showPopup(qry);
        //    }
        //}





    }

    #endregion

    #region Generate Next Number
    private void NextNumber()
    {
        try
        {
            int counts = 0;
            counts = Convert.ToInt32(clsCommon.getString("select count(doc_no) as doc_no from " + tblHead + " where tran_type='" + hdnfTran_type.Value + "' and Company_Code='" + Session["Company_Code"].ToString() + "' " +
                " and Year_Code='" + Session["year"].ToString() + "'"));
            if (counts == 0)
            {
                txtdoc_no.Text = "1";
                Doc_No = 1;
            }
            else
            {
                Doc_No = Convert.ToInt32(clsCommon.getString("SELECT max(doc_no) as doc_no from " + tblHead + " where tran_type='" + hdnfTran_type.Value + "' and Company_Code='" + Session["Company_Code"].ToString() + "' " +
                    "  and Year_Code='" + Session["year"].ToString() + "'")) + 1;
                txtdoc_no.Text = Doc_No.ToString();
            }

            counts = Convert.ToInt32(clsCommon.getString("SELECT count(tranid) as tranid from " + tblHead + " "));
            if (counts == 0)
            {
                lblReceiptPayment_Id.Text = "1";
                Receiptid = 1;

            }
            else
            {
                Receiptid = Convert.ToInt32(clsCommon.getString("SELECT max(tranid) as tranid from " + tblHead)) + 1;
                lblReceiptPayment_Id.Text = Receiptid.ToString();
            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Next Record Error!')", true);
        }
    }
    #endregion


}