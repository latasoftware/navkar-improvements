using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Threading;
using System.Text;

public partial class Sugar_pgeSugarRetailForGSTnew : System.Web.UI.Page
{

    #region data section

    DataTable dt1 = null;
    DataTable dt2 = null;
    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;

    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string SystemMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string cityMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    int defaultAccountCode = 0;
    string trntype = string.Empty;
    string user = string.Empty;
    string qryAccountList = string.Empty;
    string GLedgerTable = string.Empty;
    static WebControl objAsp = null;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    string cs = string.Empty;
    string Action = string.Empty;
    int id = 0;
    int flag = 0;
    int count = 0;
    MySqlConnection con = null;
    MySqlCommand cmd = null;
    MySqlTransaction myTran = null;
    #endregion
    string GLEDGER_Insert = string.Empty;
    string GLEDGER_Delete = string.Empty;
    string msg = string.Empty;
    #region Head Declaration Field
    Int32 Doc_No = 0;

    Int32 AC_CODE = 0;
    Int32 partyCode = 0;
    string DOC_DATE = string.Empty;
    string Party_Name = string.Empty;
    string TRAN_TYPE = string.Empty;
    Int32 BN = 0;
    Int32 AN = 0;
    int Unit_id = 0;
    Int32 Party_Name_New = 0;
    string Created_By = clsGV.user;
    string Modified_By = clsGV.user;
    string LORRYNO = string.Empty;
    string CHALLAN_NO = string.Empty;
    string CHALLAN_DATE = string.Empty;
    Int32 GSTRateCode = 0;
    Int32 Broker_CODE = 0;
    string DUE_DATE = string.Empty;
    Int32 DUE_DAYS = 0;
    int DELIVERED = 0;
    double TOTAL = 0.00;
    double CGSTRate = 0.00;
    double CGSTAmount = 0.00;
    double IGSTRate = 0.00;
    double IGSTAmount = 0.00;
    double SGSTRate = 0.00;
    double SGSTAmount = 0.00;
    double VAT = 0.00;
    double SUBTOTAL = 0.00;
    double ROUND_OFF = 0.00;
    double hamali_amount = 0.00;
    double GRAND_TOTAL = 0.00;
    string deliver_type = string.Empty;
    string Head_Insert = string.Empty;
    StringBuilder Head_Update = null;
    string Head_Delete = string.Empty;
    StringBuilder Head_Fields = null;
    StringBuilder Head_Values = null;
    string narrtion = string.Empty;

    #endregion

    #region Detail Declaration Field
    Int32 mill_Code = 0;
    Int32 item_code = 0;
    Int32 purches_No = 0;

    double Quantity = 0.00;
    string billing_no = "";
    double rate = 0.00;
    double Value1 = 0.00;
    // Int32 Value1 = 0;
    string Detail_Id = "";
    Int32 vat_ac = 0;
    double vat_percent = 0.0;
    double vat_amount = 0.0;
    double gross = 0.0;
    string grade = "";
    string Detail_Insert = string.Empty;
    StringBuilder Detail_Update = null;
    StringBuilder Detail_Delete = null;
    StringBuilder Detail_Fields = null;
    StringBuilder Detail_Values = null;
    #endregion
    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "retailsalehead";
            tblDetails = tblPrefix + "retailselldetails";
            AccountMasterTable = tblPrefix + "AccountMaster";
            SystemMasterTable = tblPrefix + "SystemMaster";
            cityMasterTable = tblPrefix + "CityMaster";
            qryCommon = "qryretailsellheaddetail";
            qryAccountList = tblPrefix + "qryAccountsList";
            GLedgerTable = tblPrefix + "GLEDGER";
            pnlPopup.Style["display"] = "none";
            user = Session["user"].ToString();
            //trntype = drpCashCredit.SelectedValue.ToString();
            cs = ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
            con = new MySqlConnection(cs);

            dr = null;
            Maindt = new DataTable();
            Maindt.Columns.Add("Querys", typeof(string));
            dr = Maindt.NewRow();

            Head_Fields = new StringBuilder();
            Head_Values = new StringBuilder();
            Detail_Fields = new StringBuilder();
            Detail_Values = new StringBuilder();
            Detail_Update = new StringBuilder();
            Detail_Delete = new StringBuilder();
            Head_Update = new StringBuilder();
            if (!Page.IsPostBack)
            {

                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    Action = Request.QueryString["Action"];
                    hdnfdrpVal.Value = Request.QueryString["tran_type"];
                    trntype = hdnfdrpVal.Value;
                    drpCashCredit.Text = hdnfdrpVal.Value;
                    if (Action == "1")
                    {
                        hdnf.Value = Request.QueryString["retailid"];
                        pnlPopup.Style["display"] = "none";
                        ViewState["currentTable"] = null;
                        clsButtonNavigation.enableDisable("N");
                        pnlgrdDetail.Enabled = true;
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
                        txtDOC_NO.Enabled = false;
                        this.NextNumber();
                        setFocusControl(drpCashCredit);



                        if (drpCashCredit.SelectedValue == "CS")
                        {
                            txtAC_CODE.Text = "1";
                            LblPartyname.Text = "Cash";
                            lblPartyAddress.Text = "";
                            txtGSTRateCode.Text = "1";
                            txtITEM_CODE.Text = "1";
                            if (txtGSTRateCode.Text == "1")
                            {
                                searchString = txtGSTRateCode.Text;
                                strTextBox = "txtGSTRateCode";
                                csCalculations();
                            }

                            if (txtITEM_CODE.Text == "1")
                            {
                                searchString = txtITEM_CODE.Text;
                                strTextBox = "txtITEM_CODE";
                                csCalculations();
                            }

                            chkcashrecive.Checked = true;
                            drpDelivered.SelectedIndex = 0;
                            setFocusControl(txtparty_Name);
                        }
                        else if (drpCashCredit.SelectedValue == "CR")
                        {
                            txtAC_CODE.Text = "1";
                            LblPartyname.Text = "Cash";
                            lblPartyAddress.Text = "";
                            txtGSTRateCode.Text = "1";
                            txtITEM_CODE.Text = "1";
                            //txtparty_Name.Enabled = false;
                            if (txtGSTRateCode.Text == "1")
                            {
                                searchString = txtGSTRateCode.Text;
                                strTextBox = "txtGSTRateCode";
                                setFocusControl(txtDOC_DATE);
                                csCalculations();

                            }

                            if (txtITEM_CODE.Text == "1")
                            {
                                searchString = txtITEM_CODE.Text;
                                strTextBox = "txtITEM_CODE";
                                csCalculations();
                            }
                            // chkcashrecive.Checked = false;
                            chkcashrecive.Enabled = true;
                            chkcashrecive.Checked = true;

                            drpDelivered.SelectedIndex = 0;
                            setFocusControl(txtparty_Name);
                        }


                        else
                        {
                            setFocusControl(txtparty_Name);
                        }

                        setFocusControl(txtDOC_DATE);
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

    #region Generate Next Number
    private void NextNumber()
    {
        try
        {
            int counts = 0;
            counts = Convert.ToInt32(clsCommon.getString("select ifnull(count(doc_no),0) as doc_no from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' and Year_Code='" + Session["Year"].ToString() + "' and Tran_Type='" + drpCashCredit.Text + "'"));
            if (counts == 0)
            {
                txtDOC_NO.Text = "1";
                Doc_No = 1;
            }
            else
            {
                Doc_No = Convert.ToInt32(clsCommon.getString("SELECT max(doc_no) as doc_no from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' and  Year_Code='" + Session["Year"].ToString() + "'  and Tran_Type='" + drpCashCredit.Text + "'")) + 1;
                txtDOC_NO.Text = Doc_No.ToString();
            }

            counts = Convert.ToInt32(clsCommon.getString("SELECT ifnull(count(retailid),0) as retailid from " + tblHead + " "));
            if (counts == 0)
            {
                lblDoc_Id.Text = "1";
                id = 1;
            }
            else
            {
                id = Convert.ToInt32(clsCommon.getString("SELECT max(retailid) as retailid from " + tblHead)) + 1;
                lblDoc_Id.Text = id.ToString();
            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Next Record Error!')", true);
        }
    }
    #endregion
    protected void txtEditDoc_No_TextChanged(object sender, EventArgs e)
    {

        bool a = clsCommon.isStringIsNumeric(txtEditDoc_No.Text);
        try
        {
            if (a == false)
            {
                hdnf.Value = txtEditDoc_No.Text;
                pnlPopup.Style["display"] = "block";
                txtSearchText.Text = txtEditDoc_No.Text;
                hdnfClosePopup.Value = "txtEditDoc_No";
                btnSearch_Click(this, new EventArgs());
            }
            else
            {
                hdnf.Value = txtEditDoc_No.Text;
                string qry = "select *,CONVERT(varchar(10),doc_date,103) as doc_date1 from " + qryCommon + " where doc_no='" + txtEditDoc_No.Text + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and Tran_Type='" + trntype + "'";
                this.fetchRecord(qry);
                setFocusControl(txtEditDoc_No);
                //pnlgrdDetail.Enabled = true;
            }
        }
        catch (Exception)
        {
            throw;
        }

    }

    #region [getMaxCode]
    private void getMaxCode()
    {
        try
        {
            DataSet ds = null;
            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                obj.tableName = tblHead + " where Tran_Type='" + trntype + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
                                    txtDOC_NO.Text = ds.Tables[0].Rows[0][0].ToString();
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
                drpDelivered.Enabled = false;
                drpCashCredit.Enabled = true;
                pnlPopup.Style["display"] = "none";
                btnOpenDetailsPopup.Enabled = false;
                btnSave.Text = "Save";
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                lblMsg.Text = string.Empty;
                btnBack.Enabled = true;
                #region logic
                LblPartyname.Text = "";
                lblCashParty_Name.Visible = true;
                //LBLMILLNAME.Text = "";
                //LBLBROKERNAME.Text = "";
                btntxtAC_CODE.Enabled = false;
                //btntxtMILL_CODE.Enabled = false;
                btntxtITEM_CODE.Enabled = false;
                btntxtBroker_Code.Enabled = false;
                btntxtParty_Name.Enabled = false;
                btntxtdoc_no.Enabled = false;
                btnAdddetails.Enabled = false;
                btnClosedetails.Enabled = false;
                calenderExtenderDate.Enabled = false;
                CalendarExtender1.Enabled = false;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                pnlgrdDetail.Enabled = false;
                btntxtMillCode.Enabled = false;
                btntxtPurchaseno.Enabled = false;
                btnPrintSaleBill.Enabled = true;
                //lblUnitName.Text = "";
                txtEditDoc_No.Enabled = true;
                btntxtGSTRateCode.Enabled = false;
                chkcashrecive.Enabled = false;

                // string nm=txtAC_CODE.Text.ToString();

                if (chkcashrecive.Checked)
                {
                    btncashRecivePrint.Enabled = true;
                }
                else
                {
                    btncashRecivePrint.Enabled = false;
                }

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
                drpDelivered.Enabled = true;
                drpCashCredit.Enabled = true;
                CalendarExtender1.Enabled = true;
                txtEditDoc_No.Enabled = false;
                btnSave.Text = "Save";
                txtDOC_NO.Enabled = false;
                btntxtdoc_no.Text = "Change No";
                btntxtdoc_no.Enabled = true;
                btnBack.Enabled = false;
                #region set Business logic for save
                LblPartyname.Text = "";
                lblParty_Name_New.Text = "";
                //LBLMILLNAME.Text = "";
                //LBLBROKERNAME.Text = "";
                btntxtAC_CODE.Enabled = true;
                btntxtGSTRateCode.Enabled = true;
                btntxtMillCode.Enabled = true;
                btntxtPurchaseno.Enabled = true;
                //lblTransportName.Text = "";
                btntxtITEM_CODE.Enabled = true;
                btnAdddetails.Enabled = true;
                btnClosedetails.Enabled = true;
                calenderExtenderDate.Enabled = true;
                txtDOC_DATE.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                txtChallanDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                txtDueDate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                btnOpenDetailsPopup.Enabled = true;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                pnlgrdDetail.Enabled = true;
                btnPrintSaleBill.Enabled = false;
                drpCashCredit.Enabled = true;
                txtPurchaseno.Enabled = false;
                btntxtBroker_Code.Enabled = true;
                btntxtParty_Name.Enabled = true;
                lblbrokerName.Text = "";
                btncashRecivePrint.Enabled = false;
                if (drpCashCredit.Text == "CS")
                {
                    chkcashrecive.Enabled = true;
                }
                else
                {
                    chkcashrecive.Enabled = false;
                    chkcashrecive.Checked = false;
                }
                //btntxtUnitcode.Enabled = true;
                //lblUnitName.Text = "";
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
                drpDelivered.Enabled = false;
                drpCashCredit.Enabled = true;
                CalendarExtender1.Enabled = false;
                txtEditDoc_No.Enabled = true;
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                btnOpenDetailsPopup.Enabled = false;
                pnlgrdDetail.Enabled = false;
                btnPrintSaleBill.Enabled = true;
                btncashRecivePrint.Enabled = true;
                //btntxtUnitcode.Enabled = false;
                btnBack.Enabled = true;
                #region logic
                btntxtAC_CODE.Enabled = false;
                btntxtGSTRateCode.Enabled = false;
                btntxtMillCode.Enabled = false;
                btntxtPurchaseno.Enabled = false;
                btntxtITEM_CODE.Enabled = false;
                calenderExtenderDate.Enabled = false;
                btnAdddetails.Enabled = false;
                btnClosedetails.Enabled = false;
                btntxtBroker_Code.Enabled = false;
                btntxtParty_Name.Enabled = false;
                txtITEM_CODE.Text = "";
                LBLITEMNAME.Text = "";
                txtMillCode.Text = "";
                lblMillName.Text = "";
                txtRATE.Text = "";
                txtITEMAMOUNT.Text = "";
                txtGross.Text = "";
                chkcashrecive.Enabled = false;
                if (chkcashrecive.Checked)
                {
                    btncashRecivePrint.Enabled = true;
                }
                else
                {
                    btncashRecivePrint.Enabled = false;
                }
                #endregion
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
                drpDelivered.Enabled = true;
                drpCashCredit.Enabled = false;
                CalendarExtender1.Enabled = true;
                txtEditDoc_No.Enabled = false;
                txtDOC_NO.Enabled = true;
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = true;
                btntxtGSTRateCode.Enabled = true;
                lblMsg.Text = string.Empty;
                btnOpenDetailsPopup.Enabled = true;
                pnlgrdDetail.Enabled = true;
                btnPrintSaleBill.Enabled = false;
                btntxtBroker_Code.Enabled = true;
                btntxtParty_Name.Enabled = true;

                //btntxtUnitcode.Enabled = true;
                // btncashRecivePrint.Enabled = false;
                //if (drpCashCredit.Text == "CS")
                //{
                //    chkcashrecive.Enabled = true;
                //}
                //else
                //{
                //    chkcashrecive.Enabled = falsse;
                //    chkcashrecive.Checked = false;
                //}
                chkcashrecive.Enabled = true;

                btntxtPurchaseno.Enabled = true;
                btnBack.Enabled = false;
                #region logic
                btntxtAC_CODE.Enabled = true;
                btntxtMillCode.Enabled = true;
                btntxtITEM_CODE.Enabled = true;
                calenderExtenderDate.Enabled = true;
                btnAdddetails.Enabled = true;
                btnClosedetails.Enabled = true;
                #endregion
            }
            #region Always check this
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

    #region [enableDisableNavigateButtons]
    private void enableDisableNavigateButtons()
    {
        #region enable disable previous next buttons
        int RecordCount = 0;
        string query = "";
        query = "select count(*) from " + tblHead + " where Tran_Type='" + trntype + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        ds = clsDAL.SimpleQuery(query);
        if (ds != null)
        {
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    RecordCount = Convert.ToInt32(dt.Rows[0][0].ToString());
                }
            }
        }
        if (RecordCount != 0 && RecordCount == 1)
        {
            btnFirst.Enabled = true;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
            btnLast.Enabled = false;
        }
        else if (RecordCount != 0 && RecordCount > 1)
        {
            btnFirst.Enabled = true;
            btnPrevious.Enabled = false;
            btnNext.Enabled = false;
            btnLast.Enabled = true;
        }
        if (txtDOC_NO.Text != string.Empty)
        {
            if (hdnf.Value != string.Empty)
            {
                #region check for next or previous record exist or not
                ds = new DataSet();
                dt = new DataTable();
                query = "SELECT top 1 [doc_no] from " + tblHead + " where Tran_Type='" + trntype + "' and doc_no>" + Convert.ToInt32(hdnf.Value) +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                    " ORDER BY doc_no asc  ";
                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            //next record exist
                            btnNext.Enabled = true;
                            btnLast.Enabled = true;
                        }
                        else
                        {
                            //next record does not exist
                            btnNext.Enabled = false;
                            btnLast.Enabled = false;
                        }
                    }
                }
                ds = new DataSet();
                dt = new DataTable();
                query = "SELECT top 1 [doc_no] from " + tblHead + " where Tran_Type='" + trntype + "' and doc_no<" + Convert.ToInt32(hdnf.Value) +
                    " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                    " ORDER BY doc_no asc  ";
                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            //previous record exist
                            btnPrevious.Enabled = true;
                            btnFirst.Enabled = true;
                        }
                        else
                        {
                            btnPrevious.Enabled = false;
                            btnFirst.Enabled = false;
                        }
                    }
                }

                #endregion
            }
        }

        #endregion
    }
    #endregion

    #region [First]
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select doc_no from " + tblHead + " where doc_no=(select MIN(doc_no) from " + tblHead + " where   Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "')  " +
                " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Tran_Type='" + trntype + "'  and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
            if (txtDOC_NO.Text != string.Empty)
            {
                string query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) +
                            "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Tran_Type='" + trntype + "' and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
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
            if (txtDOC_NO.Text != string.Empty)
            {
                string query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) +
                            "  and Tran_Type='" + trntype + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
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
            query = "select DOC_NO from " + tblHead + " where DOC_NO=(select MAX(DOC_NO) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ") " +
                " and Tran_Type='" + trntype + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
        this.makeEmptyForm("A");
        this.NextNumber();
        this.getMaxCode();
        txtDOC_NO.Enabled = false;
        pnlPopupDetails.Style["display"] = "none";
        if (drpCashCredit.SelectedValue == "CS")
        {
            txtAC_CODE.Text = "1";
            LblPartyname.Text = "Cash";
            lblPartyAddress.Text = "";
            txtGSTRateCode.Text = "1";
            txtITEM_CODE.Text = "1";
            if (txtGSTRateCode.Text == "1")
            {
                searchString = txtGSTRateCode.Text;
                strTextBox = "txtGSTRateCode";
                csCalculations();
            }

            if (txtITEM_CODE.Text == "1")
            {
                searchString = txtITEM_CODE.Text;
                strTextBox = "txtITEM_CODE";
                csCalculations();
            }

            chkcashrecive.Checked = true;
            drpDelivered.SelectedIndex = 0;
            setFocusControl(txtparty_Name);
        }
        else if (drpCashCredit.SelectedValue == "CR")
        {
            txtAC_CODE.Text = "1";
            LblPartyname.Text = "Cash";
            lblPartyAddress.Text = "";
            txtGSTRateCode.Text = "1";
            txtITEM_CODE.Text = "1";
            //txtparty_Name.Enabled = false;
            if (txtGSTRateCode.Text == "1")
            {
                searchString = txtGSTRateCode.Text;
                strTextBox = "txtGSTRateCode";
                csCalculations();
            }

            if (txtITEM_CODE.Text == "1")
            {
                searchString = txtITEM_CODE.Text;
                strTextBox = "txtITEM_CODE";
                csCalculations();
            }
            // chkcashrecive.Checked = false;
            chkcashrecive.Enabled = true;
            chkcashrecive.Checked = true;

            drpDelivered.SelectedIndex = 0;
            setFocusControl(txtparty_Name);
        }


        else
        {
            setFocusControl(txtparty_Name);
        }


        //lblDONo.Text = "0";
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
        txtDOC_NO.Enabled = false;
        txtITEM_CODE.Text = "1";
        if (txtITEM_CODE.Text == "1")
        {
            searchString = txtITEM_CODE.Text;
            strTextBox = "txtITEM_CODE";
            csCalculations();
        }
        setFocusControl(txtMillCode);
    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                string Detail_Deleteqry = "delete from " + tblDetails + " where retailid='" + lblDoc_Id.Text + "'";
                GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='RR' and Doc_No=" + txtDOC_NO.Text + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                Head_Delete = "delete from " + tblHead + " where retailid='" + lblDoc_Id.Text + "' ";

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
                //Thread thred = new Thread(() => { count = DataStore(flag); }); //Calling DataStore Method Using Thread
                //thred.Start(); //Thread Operation Start
                //thred.Join();
                if (msg == "Delete")
                {
                    Response.Redirect("../Outword/PgeRetailSellUtility.aspx");
                }
            }

            else
            {
                hdnf.Value = lblDoc_Id.Text;
                showLastRecord();
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

        string max = clsCommon.getString("select max(retailid) as id from nt_1_retailsalehead where Company_Code=" + Session["Company_Code"].ToString() + " " +
            " and Year_Code=" + Session["year"].ToString() + " and Tran_Type='" + drpCashCredit.SelectedValue + "'");
        hdnf.Value = max;
        trntype = drpCashCredit.SelectedValue;
        this.makeEmptyForm("N");
        clsButtonNavigation.enableDisable("N");
        ViewState["mode"] = "I";
        this.showLastRecord();
        setFocusControl(btnEdit);


    }
    #endregion

    protected void drpCashCredit_SelectedIndexChanged(object sender, EventArgs e)
    {
        setFocusControl(drpCashCredit);
        if (drpCashCredit.Text == "CS")
        {
            btncashRecivePrint.Enabled = true;
        }
        else
        {
            btncashRecivePrint.Enabled = false;
        }
        string max = clsCommon.getString("select max(retailid) as id from nt_1_retailsalehead where Company_Code=" + Session["Company_Code"].ToString() + " " +
           " and Year_Code=" + Session["year"].ToString() + " and Tran_Type='" + drpCashCredit.SelectedValue + "'");
        hdnf.Value = max;
        trntype = drpCashCredit.SelectedValue;
        clsButtonNavigation.enableDisable("N");
        pnlgrdDetail.Enabled = true;
        this.makeEmptyForm("N");
        ViewState["mode"] = "I";
        showLastRecord();
    }

    protected void txtparty_Name_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtparty_Name.Text;
        //strTextBox = "txtparty_Name";
        //csCalculations();
        //setFocusControl(txtMillCode);



        // searchString = txtparty_Name.Text;
        strTextBox = "txtparty_Name";
        csCalculations();
    }


    protected void txtparty_Name_New_TextChanged(object sender, EventArgs e)
    {
        searchString = txtparty_Name_New.Text;
        strTextBox = "txtparty_Name_New";
        csCalculations();
    }


    protected void drpDelivered_SelectedIndexChanged(object sender, EventArgs e)
    {
        setFocusControl(drpDelivered);

    }

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
                        txtDOC_NO.Text = dt.Rows[0]["doc_no"].ToString();
                        lblDoc_Id.Text = hdnf.Value;
                        drpCashCredit.SelectedValue = dt.Rows[0]["Tran_Type"].ToString().Trim();
                        txtDOC_DATE.Text = dt.Rows[0]["doc_dateConverted"].ToString();// DateTime.Parse(dt.Rows[0]["doc_date"].ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-BG")).ToString("MM/dd/yyyy");
                        txtChallanNo.Text = dt.Rows[0]["Challan_No"].ToString();
                        txtChallanDate.Text = dt.Rows[0]["Challan_DateConverted"].ToString();// DateTime.Parse(dt.Rows[0]["Challan_Date"].ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-BG")).ToString("MM/dd/yyyy");
                        txtLORRYNO.Text = dt.Rows[0]["Vehical_No"].ToString();
                        //string partyCode = dt.Rows[0]["Party_Code"].ToString();
                        txtAC_CODE.Text = dt.Rows[0]["Party_Code"].ToString();
                        string acname = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_Code=" + txtAC_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //LblPartyname.Text = dt.Rows[0]["partyname"].ToString();
                        LblPartyname.Text = acname;

                        // LblPartyname.Text = dt.Rows[0]["Ac_Name_E"].ToString();
                        lblPartyAddress.Text = dt.Rows[0]["Address_E"].ToString();
                        txtGSTRateCode.Text = dt.Rows[0]["GstRateCode"].ToString();
                        lblGSTRateName.Text = dt.Rows[0]["GST_Name"].ToString();
                        //string address = clsCommon.getString("select Address_E from " + qryAccountList + " where Ac_Code=" + partyCode + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //string city = clsCommon.getString("select CityName from " + qryAccountList + " where Ac_Code=" + partyCode + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //lblPartyAddress.Text = dt.Rows[0]["partyaddress"].ToString() + " <br/>" + dt.Rows[0]["partycity"].ToString();
                        txtDUE_DAYS.Text = dt.Rows[0]["Due_Days"].ToString();
                        txtDueDate.Text = dt.Rows[0]["Due_DateConverted"].ToString();// DateTime.Parse(dt.Rows[0]["Due_Date"].ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-BG")).ToString("MM/dd/yyyy");
                        txtTotal.Text = dt.Rows[0]["Total"].ToString();
                        txtVatAmount.Text = dt.Rows[0]["Vat"].ToString();
                        txtCGSTRate.Text = dt.Rows[0]["CGSTRate"].ToString();
                        txtCGSTAmount.Text = dt.Rows[0]["CGSTAmount"].ToString();
                        txtSGSTRate.Text = dt.Rows[0]["SGSTRate"].ToString();
                        txtSGSTAmount.Text = dt.Rows[0]["SGSTAmount"].ToString();
                        txtIGSTRate.Text = dt.Rows[0]["IGSTRate"].ToString();
                        txtIGSTAmount.Text = dt.Rows[0]["IGSTAmount"].ToString();
                        txtSubtotal.Text = dt.Rows[0]["Subtotal"].ToString();
                        txtRoundOff.Text = dt.Rows[0]["Round_Off"].ToString();
                        txthamliAmount.Text = dt.Rows[0]["HamaliAmount"].ToString();
                        txtBILL_AMOUNT.Text = dt.Rows[0]["Grand_Total"].ToString();
                        txtparty_Name.Text = dt.Rows[0]["Party_Name"].ToString();
                        txtBroker_Code.Text = dt.Rows[0]["Broker_Code"].ToString();
                        lblbrokerName.Text = dt.Rows[0]["BrokerName"].ToString();

                        txtnarration.Text = dt.Rows[0]["Narration"].ToString();
                        string s = clsCommon.getString("select Ac_Name_E from NT_1_AccountMaster where Ac_Code='" + txtparty_Name.Text + "' and Ac_type='CR'");

                        // lblCashParty_Name.Text = s;
                        string cashrecive = dt.Rows[0]["CashRecive"].ToString();
                        // chkcashrecive.Checked = Convert.ToBoolean(cashrecive);

                        bool delivered = Convert.ToBoolean(dt.Rows[0]["Delivered"].ToString());
                        if (delivered)
                        {
                            drpDelivered.SelectedValue = "1";
                        }
                        else
                        {
                            drpDelivered.SelectedValue = "0";
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
                        recordExist = true;
                        lblMsg.Text = "";
                        txtparty_Name_New.Text = dt.Rows[0]["Party_Name_New"].ToString();
                        string s1 = clsCommon.getString("select Ac_Name_E from NT_1_AccountMaster where Ac_Code='" + txtparty_Name_New.Text + "' and Ac_type='CR'");
                        lblParty_Name_New.Text = s1;

                        #region  Details

                        qry = "select detail_id as ID,Mill_Code as Mill_Code,millname as MillShort,Ifnull(purchaseid,0)  as Purchase_Id,grade,Item_Code as item_code, itemname as item_name,Quantity,Billing_No,Rate," +
                            "Value,Vat_Ac,vat_percent as Vat_Percent,vat_amount as Vat_Amount,Gross , retaildetailid as AutoDetail_Id from qryretailselldetail where retailid="
                            + hdnf.Value + " and TRan_Type='" + hdnfdrpVal.Value + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())


                            + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
            //string qryDisplay = "select * from " + qryCommon +
            //   " where doc_no =" + hdnf.Value + " and Tran_Type='" + trntype + "' ";

            string qryDisplay = " select * from " + qryCommon + " where retailid=" + hdnf.Value + "  and Tran_Type='" + hdnfdrpVal.Value + "'";
            return qryDisplay;

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
                txtDOC_NO.Text = hdnf.Value;
                string query = getDisplayQuery();
                clsButtonNavigation.enableDisable("N");
                bool recordExist = this.fetchRecord(query);
                if (recordExist == true)
                {
                    btnEdit.Enabled = true;
                    btnEdit.Focus();
                }
                this.enableDisableNavigateButtons();
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
        //txtPACKING.Text = "50";
        txtITEM_CODE.Text = "";
        txtQUANTAL.Text = "";
        //txtBAGS.Text = "";
        txtRATE.Text = "";
        txtITEMAMOUNT.Text = "";
        //txtITEM_NARRATION.Text = "";
        LBLITEMNAME.Text = "";
        setFocusControl(txtMillCode);
        txtITEM_CODE.Text = "1";
        string itemname = clsCommon.getString("select System_Name_E from " + SystemMasterTable + " where System_Code=1  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and System_Type='I'");
        ItemSelection(itemname, "1");
    }
    #endregion

    #region [btnAdddetails_Click]
    protected void btnAdddetails_Click(object sender, EventArgs e)
    {
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
                        //rowIndex = dt.Rows.Count + 1;
                        dr["ID"] = rowIndex;     //auto
                        dr["rowAction"] = "A";
                        dr["SrNo"] = 0;
                    }
                    else
                    {
                        // update row
                        int n = Convert.ToInt32(lblNo.Text);
                        rowIndex = Convert.ToInt32(lblID.Text);   //auto no
                        dr = (DataRow)dt.Rows[n - 1];
                        dr["ID"] = rowIndex;
                        dr["SrNo"] = 0;
                        #region decide whether actual row is updating or virtual [rowAction]
                        string id = clsCommon.getString("select detail_id from " + tblDetails + " where doc_no=" + txtDOC_NO.Text + " and Detail_ID=" + lblID.Text + "  And Company_Code = " + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                        if (id != string.Empty && id != "0")
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
                    dt.Columns.Add((new DataColumn("ID", typeof(Int32))));
                    #region [Write here columns]
                    dt.Columns.Add((new DataColumn("Mill_Code", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("MillShort", typeof(string))));
                    dt.Columns.Add((new DataColumn("Purchase_Id", typeof(Int32))));

                    dt.Columns.Add((new DataColumn("grade", typeof(string))));
                    dt.Columns.Add((new DataColumn("item_code", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("item_name", typeof(string))));
                    dt.Columns.Add((new DataColumn("Quantity", typeof(string))));
                    dt.Columns.Add((new DataColumn("Billing_No", typeof(double))));
                    dt.Columns.Add((new DataColumn("Rate", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("Value", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("Vat_Ac", typeof(double))));
                    dt.Columns.Add((new DataColumn("Vat_Percent", typeof(double))));
                    dt.Columns.Add((new DataColumn("Vat_Amount", typeof(double))));
                    dt.Columns.Add((new DataColumn("Gross", typeof(double))));
                    dt.Columns.Add((new DataColumn("AutoDetail_Id", typeof(double))));
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
                dt.Columns.Add((new DataColumn("Mill_Code", typeof(Int32))));
                dt.Columns.Add((new DataColumn("MillShort", typeof(string))));
                dt.Columns.Add((new DataColumn("Purchase_Id", typeof(Int32))));
                dt.Columns.Add((new DataColumn("grade", typeof(string))));
                dt.Columns.Add((new DataColumn("item_code", typeof(Int32))));
                dt.Columns.Add((new DataColumn("item_name", typeof(string))));
                dt.Columns.Add((new DataColumn("Quantity", typeof(string))));
                dt.Columns.Add((new DataColumn("Billing_No", typeof(double))));
                dt.Columns.Add((new DataColumn("Rate", typeof(double))));
                dt.Columns.Add((new DataColumn("Value", typeof(double))));
                dt.Columns.Add((new DataColumn("Vat_Ac", typeof(double))));
                dt.Columns.Add((new DataColumn("Vat_Percent", typeof(double))));
                dt.Columns.Add((new DataColumn("Vat_Amount", typeof(double))));
                dt.Columns.Add((new DataColumn("Gross", typeof(double))));
                dt.Columns.Add((new DataColumn("AutoDetail_Id", typeof(double))));
                #endregion
                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                dt.Columns.Add(new DataColumn("SrNo", typeof(int)));

                dr = dt.NewRow();
                dr["ID"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;
            }
            #region [ Set values to dr]
            dr["Mill_Code"] = txtMillCode.Text;
            string MillShortName = clsCommon.getString("select Short_Name from qrymstaccountmaster where Ac_Code=" + txtMillCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            // dr["MillShort"] = MillShortName;
            if (btnAdddetails.Text == "ADD")
            {
                if (txtPurchaseno.Text != string.Empty)
                {
                    dr["Purchase_Id"] = txtPurchaseno.Text;
                }
                else
                {
                    dr["Purchase_Id"] = 0;
                }
            }
            else
            {
                dr["Purchase_Id"] = txtPurchaseno.Text;
            }
            dr["MillShort"] = lblMillName.Text;

            //lblgrade.Text = Server.HtmlDecode(gvrow.Cells[5].Text);
            dr["grade"] = Server.HtmlDecode(lblgrade.Text);
            dr["item_code"] = txtITEM_CODE.Text;
            dr["item_name"] = LBLITEMNAME.Text;

            //dr["narration"] = Server.HtmlDecode(txtITEM_NARRATION.Text);
            if (txtQUANTAL.Text != string.Empty)
            {
                dr["Quantity"] = txtQUANTAL.Text;
            }
            else
            {
                setFocusControl(txtQUANTAL);
            }
            if (txtBillingNo.Text != string.Empty)
            {
                dr["Billing_No"] = txtBillingNo.Text;
            }
            else
            {
                dr["Billing_No"] = "0";
            }

            //dr["bags"] = txtBAGS.Text;
            if (txtRATE.Text != string.Empty)
            {
                dr["Rate"] = txtRATE.Text;
            }
            else
            {
                setFocusControl(txtRATE);
            }
            if (txtITEMAMOUNT.Text != string.Empty)
            {
                dr["Value"] = txtITEMAMOUNT.Text;
            }
            else
            {
                setFocusControl(txtITEMAMOUNT);
            }

            if (txtVatAc.Text != string.Empty)
            {
                dr["Vat_Ac"] = txtVatAc.Text;
            }
            else
            {
                dr["Vat_Ac"] = 0;
            }

            if (txtVatPercent.Text != string.Empty)
            {
                dr["Vat_Percent"] = txtVatPercent.Text;
            }
            else
            {
                dr["Vat_Percent"] = "0";
            }

            if (txtVatTotal.Text != string.Empty)
            {
                dr["Vat_Amount"] = txtVatTotal.Text;
            }
            else
            {
                dr["Vat_Amount"] = "0";
            }

            if (txtGross.Text != string.Empty)
            {
                dr["Gross"] = txtGross.Text;
            }
            else
            {
                setFocusControl(txtGross);
            }

            #endregion
            if (btnAdddetails.Text == "ADD")
            {
                dr["AutoDetail_Id"] = 1;
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
                pnlPopupDetails.Style["display"] = "block";
                setFocusControl(txtITEM_CODE);
            }
            else
            {
                pnlPopupDetails.Style["display"] = "none";
                setFocusControl(btnAdddetails);
                btnOpenDetailsPopup.Focus();
            }
            btnAdddetails.Text = "ADD";
            // Empty Code->
            txtMillCode.Text = string.Empty;
            lblMillName.Text = string.Empty;
            txtPurchaseno.Text = string.Empty;
            lblpurchaseno.Text = string.Empty;
            txtITEM_CODE.Text = string.Empty;
            LBLITEMNAME.Text = string.Empty;
            txtQUANTAL.Text = string.Empty;
            txtRATE.Text = string.Empty;
            txtITEMAMOUNT.Text = string.Empty;
            txtBillingNo.Text = string.Empty;
            txtITEMAMOUNT.Text = string.Empty;
            txtVatAc.Text = string.Empty;
            lblVatAcName.Text = string.Empty;
            txtVatPercent.Text = string.Empty;
            txtVatTotal.Text = string.Empty;
            txtGross.Text = string.Empty;
            lblgrade.Text = string.Empty;
            csCalculations();
            setFocusControl(txtMillCode);

            txtITEM_CODE.Text = "1";
            LBLITEMNAME.Text = "SUGAR 50 KG";
            pnlPopupDetails.Style["display"] = "none";
            Calculation();
        }
        catch
        {
        }
    }
    #endregion

    #region [btnClosedetails_Click]
    protected void btnClosedetails_Click(object sender, EventArgs e)
    {
        lblNo.Text = string.Empty;
        lblID.Text = string.Empty;
        txtMillCode.Text = "";
        lblMillName.Text = string.Empty;
        txtITEM_CODE.Text = "1";
        LBLITEMNAME.Text = clsCommon.getString("Select System_Name_E from " + tblPrefix + "SystemMaster where System_Code=" + LBLITEMNAME.Text + " and System_Type='I' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        //txtITEM_NARRATION.Text = Server.HtmlDecode(gvrow.Cells[4].Text);
        txtQUANTAL.Text = "";
        txtBillingNo.Text = "";
        txtRATE.Text = "";
        txtITEMAMOUNT.Text = "";
        txtVatAc.Text = "";
        //vat ac name
        txtVatPercent.Text = "";
        txtVatTotal.Text = "";
        txtGross.Text = "";
        // pnlPopupDetails.Style["display"] = "none";
        if (txtITEM_CODE.Text == "1")
        {
            searchString = txtITEM_CODE.Text;
            strTextBox = "txtITEM_CODE";
            csCalculations();
        }
        btnAdddetails.Text = "ADD";
        setFocusControl(txtMillCode);
    }
    #endregion

    #region [showDetailsRow]
    private void showDetailsRow(GridViewRow gvrow)
    {
        lblNo.Text = Server.HtmlDecode(gvrow.Cells[19].Text);
        lblID.Text = Server.HtmlDecode(gvrow.Cells[2].Text);
        txtMillCode.Text = Server.HtmlDecode(gvrow.Cells[3].Text);

        //lblMillName.Text = Server.HtmlDecode(gvrow.Cells[4].Text);
        lblMillName.Text = clsCommon.getString("select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + txtMillCode.Text + " ");

        //txtMillCode.Text = Server.HtmlDecode(gvrow.Cells[5].Text);
        txtPurchaseno.Text = Server.HtmlDecode(gvrow.Cells[5].Text);
        lblpurchaseno.Text = clsCommon.getString("select Doc_No from qrypurchasehead where purchaseid=" + txtPurchaseno.Text + "");
        lblgrade.Text = Server.HtmlDecode(gvrow.Cells[6].Text);
        txtITEM_CODE.Text = Server.HtmlDecode(gvrow.Cells[7].Text);
        LBLITEMNAME.Text = clsCommon.getString("Select System_Name_E from " + tblPrefix + "SystemMaster where System_Code=" + Server.HtmlDecode(gvrow.Cells[7].Text.ToString()) + " and System_Type='I' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        //txtITEM_NARRATION.Text = Server.HtmlDecode(gvrow.Cells[4].Text);

        txtQUANTAL.Text = Server.HtmlDecode(gvrow.Cells[9].Text);
        txtBillingNo.Text = Server.HtmlDecode(gvrow.Cells[10].Text);
        txtRATE.Text = Server.HtmlDecode(gvrow.Cells[11].Text);
        txtITEMAMOUNT.Text = Server.HtmlDecode(gvrow.Cells[12].Text);
        txtVatAc.Text = Server.HtmlDecode(gvrow.Cells[13].Text);
        //vat ac name
        txtVatPercent.Text = Server.HtmlDecode(gvrow.Cells[14].Text);
        txtVatTotal.Text = Server.HtmlDecode(gvrow.Cells[15].Text);
        txtGross.Text = Server.HtmlDecode(gvrow.Cells[16].Text);
        pnlPopupDetails.Style["display"] = "none";
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
                int ID = Convert.ToInt32(dt.Rows[rowIndex]["ID"].ToString());
                string IDExisting = clsCommon.getString("select detail_id from " + tblDetails + " where retailid=" + hdnf.Value + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (IDExisting != string.Empty && IDExisting != "0")
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[18].Text = "D";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";            //D=Delete from table
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[18].Text = "N";
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
                        grdDetail.Rows[rowIndex].Cells[18].Text = "R";       //R=Only remove fro grid
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "R";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[18].Text = "A";
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

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string v = hdnfClosePopup.Value;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (v == "txtPURCNO")
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[1].Width = new Unit("100px");
                e.Row.Cells[2].Width = new Unit("300px");
                e.Row.Cells[3].Width = new Unit("300px");
                e.Row.Cells[4].Width = new Unit("120px");
                e.Row.Cells[5].Width = new Unit("120px");

                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;


            }
            if (v == "txtGSTRateCode")
            {
                e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(10);
                e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(10);
            }
            if (v == "txtMillCode")
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[1].Width = new Unit("300px");
                e.Row.Cells[2].Width = new Unit("300px");
                e.Row.Cells[3].Width = new Unit("300px");
            }
            if (v == "txtparty_Name_New")
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[1].Width = new Unit("300px");
                e.Row.Cells[2].Width = new Unit("300px");
            }
            if (v == "txtBroker_Code")
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[1].Width = new Unit("300px");
                e.Row.Cells[2].Width = new Unit("300px");
            }
            if (v == "txtPurchaseno")
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[1].Width = new Unit("100px");
                e.Row.Cells[2].Width = new Unit("400px");
                e.Row.Cells[3].Width = new Unit("100px");
                e.Row.Cells[4].Width = new Unit("100px");
                e.Row.Cells[5].Width = new Unit("100px");
                e.Row.Cells[5].Width = new Unit("150px");
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
                        if (grdDetail.Rows[rowindex].Cells[18].Text != "D" && grdDetail.Rows[rowindex].Cells[18].Text != "R")
                        {
                            pnlPopupDetails.Style["display"] = "block";
                            this.showDetailsRow(grdDetail.Rows[rowindex]);
                            btnAdddetails.Text = "Update";
                            setFocusControl(txtITEM_CODE);
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
        csCalculations();
    }
    #endregion

    #region [grdDetail_RowDataBound]
    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            // {
            //lblgrade.Text = Server.HtmlDecode(gvrow.Cells[5].Text);
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;

            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            //e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[15].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[16].Visible = true;
            e.Row.Cells[17].Visible = true;
            e.Row.Cells[18].Visible = true;
            e.Row.Cells[12].HorizontalAlign = HorizontalAlign.Center;

            e.Row.Cells[0].ControlStyle.Width = new Unit("40px");
            e.Row.Cells[1].ControlStyle.Width = new Unit("50px");
            e.Row.Cells[2].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[3].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[4].ControlStyle.Width = new Unit("200px");
            e.Row.Cells[5].ControlStyle.Width = new Unit("200px");
            e.Row.Cells[6].ControlStyle.Width = new Unit("200px");
            e.Row.Cells[7].ControlStyle.Width = new Unit("180px");


            e.Row.Cells[8].ControlStyle.Width = new Unit("100px");

            //0
            e.Row.Cells[9].ControlStyle.Width = new Unit("100px");
            //

            e.Row.Cells[10].ControlStyle.Width = new Unit("80px");


            e.Row.Cells[11].ControlStyle.Width = new Unit("100px");

            //0
            e.Row.Cells[12].ControlStyle.Width = new Unit("100px");
            //

            //0
            e.Row.Cells[13].ControlStyle.Width = new Unit("100px");
            //

            //0
            e.Row.Cells[14].ControlStyle.Width = new Unit("100px");
            //

            e.Row.Cells[15].ControlStyle.Width = new Unit("80px");


            e.Row.Cells[16].ControlStyle.Width = new Unit("80px");
            e.Row.Cells[17].ControlStyle.Width = new Unit("200px");

            e.Row.Cells[18].ControlStyle.Width = new Unit("80px");
            e.Row.Cells[9].Style["overflow"] = "hidden";
            e.Row.Cells[12].Style["overflow"] = "hidden";
            e.Row.Cells[13].Style["overflow"] = "hidden";
            e.Row.Cells[14].Style["overflow"] = "hidden";

            int i = 1;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                i++;
                foreach (TableCell cell in e.Row.Cells)
                {
                    string s = cell.Text.ToString();
                    if (cell.Text.Length > 20)
                    {
                        cell.Text = cell.Text.Substring(0, 26) + "..";
                        cell.ToolTip = s;
                    }
                }

            }
            //}
        }
        catch
        {
        }
    }
    #endregion

    #region [txtDOC_NO_TextChanged]
    protected void txtDOC_NO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDOC_NO.Text;
        strTextBox = "txtDOC_NO";
        csCalculations();
    }
    #endregion

    #region [btntxtDOC_NO_Click]
    protected void btntxtDOC_NO_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtDOC_NO";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    //#region [txtPURCNO_TextChanged]
    //protected void txtPURCNO_TextChanged(object sender, EventArgs e)
    //{
    //    searchString = txtPURCNO.Text;
    //    strTextBox = "txtPURCNO";
    //    csCalculations();
    //}
    //#endregion

    //#region [btntxtPURCNO_Click]
    //protected void btntxtPURCNO_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        pnlPopup.Style["display"] = "block";
    //        hdnfClosePopup.Value = "txtPURCNO";
    //        btnSearch_Click(sender, e);
    //    }
    //    catch
    //    {
    //    }
    //}
    //#endregion

    #region [txtDOC_DATE_TextChanged]
    protected void txtDOC_DATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDOC_DATE.Text;
        strTextBox = "txtDOC_DATE";
        csCalculations();
    }
    #endregion

    #region [txtChallanDate_TextChanged]
    protected void txtChallanDate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtChallanDate.Text;
        strTextBox = "txtChallanDate";
        csCalculations();

    }
    #endregion

    protected void txtChallanNo_TextChanged(object sender, EventArgs e)
    {
        searchString = txtChallanNo.Text;
        strTextBox = "txtChallanNo";
        csCalculations();

    }

    #region [txtAC_CODE_TextChanged]
    protected void txtAC_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAC_CODE.Text;
        strTextBox = "txtAC_CODE";
        csCalculations();
    }
    #endregion
    protected void txtnarration_TextChanged(object sender, EventArgs e)
    {
        searchString = txtnarration.Text;
        strTextBox = "txtnarration";
        csCalculations();
    }

    #region [txtAC_CODE_TextChanged]
    protected void txtBroker_Code_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBroker_Code.Text;
        strTextBox = "txtBroker_Code";
        csCalculations();
    }
    #endregion

    #region [btntxtAC_CODE_Click]
    protected void btntxtAC_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtAC_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    protected void btntxtBroker_Code_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtBroker_Code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    #region [txtLORRYNO_TextChanged]
    protected void txtLORRYNO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtLORRYNO.Text;
        strTextBox = "txtLORRYNO";
        csCalculations();
    }
    #endregion

    #region [txtTotal_TextChanged]
    protected void txtTotal_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTotal.Text;
        strTextBox = "txtTotal";
        csCalculations();
    }
    #endregion

    #region [txtVatAmount_TextChanged]
    protected void txtVatAmount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtVatAmount.Text;
        strTextBox = "txtVatAmount";
        csCalculations();
    }
    #endregion

    #region [txtRoundOff_TextChanged]
    protected void txtRoundOff_TextChanged(object sender, EventArgs e)
    {
        searchString = txtRoundOff.Text;
        strTextBox = "txtRoundOff";
        csCalculations();
    }
    #endregion

    protected void txthamliAmount_TextChanged(object sender, EventArgs e)
    {
        searchString = txthamliAmount.Text;
        strTextBox = "txthamliAmount";
        csCalculations();
    }

    #region [txtSubtotal_TextChanged]
    protected void txtSubtotal_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSubtotal.Text;
        strTextBox = "txtSubtotal";
        csCalculations();
    }
    #endregion

    #region [txtBILL_AMOUNT_TextChanged]
    protected void txtBILL_AMOUNT_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBILL_AMOUNT.Text;
        strTextBox = "txtBILL_AMOUNT";
        csCalculations();
    }
    #endregion

    #region [txtDUE_DAYS_TextChanged]
    protected void txtDUE_DAYS_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDUE_DAYS.Text;
        strTextBox = "txtDUE_DAYS";
        csCalculations();
    }
    #endregion

    #region [txtITEM_CODE_TextChanged]
    protected void txtITEM_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtITEM_CODE.Text;
        strTextBox = "txtITEM_CODE";
        csCalculations();
    }
    #endregion

    #region [btntxtITEM_CODE_Click]
    protected void btntxtITEM_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtITEM_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtMillCode_TextChanged]
    protected void txtMillCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMillCode.Text;
        strTextBox = "txtMillCode";
        csCalculations();
        //  string dono = hdnfDoNo.Value.Trim();
    }
    #endregion

    #region [txtPurchaseno]
    protected void txtPurchaseno_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPurchaseno.Text;
        strTextBox = "txtPurchaseno";
        csCalculations();

    }
    #endregion

    #region [btntxtMillCode_Click]
    protected void btntxtMillCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtMillCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [btntxtPurchaseno_Click]
    protected void btntxtPurchaseno_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value != "Close")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtPurchaseno";
                btnSearch_Click(sender, e);
                //txtPurchaseno.Enabled = false;
            }
            else
            {
                hdconfirm.Value = string.Empty;
                lblpurchaseno.Text = clsCommon.getString("select Doc_No from qrypurchasehead where purchaseid=" + txtPurchaseno.Text + "");
                setFocusControl(btntxtPurchaseno);
            }


        }
        catch
        {
        }
    }
    #endregion

    #region [txtQUANTAL_TextChanged]
    protected void txtQUANTAL_TextChanged(object sender, EventArgs e)
    {
        searchString = txtQUANTAL.Text;
        strTextBox = "txtQUANTAL";
        csCalculations();
    }
    #endregion

    #region [txtBillingNo_TextChanged]
    protected void txtBillingNo_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBillingNo.Text;
        strTextBox = "txtBillingNo";
        csCalculations();
    }
    #endregion

    #region [txtRATE_TextChanged]
    protected void txtRATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtRATE.Text;
        strTextBox = "txtRATE";
        csCalculations();
    }
    #endregion

    #region [txtITEMAMOUNT_TextChanged]
    protected void txtITEMAMOUNT_TextChanged(object sender, EventArgs e)
    {
        searchString = txtITEMAMOUNT.Text;
        strTextBox = "txtITEMAMOUNT";
        csCalculations();
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
            if (hdnfClosePopup.Value == "txtAC_CODE")
            {
                setFocusControl(txtAC_CODE);
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
            //string format = "MM/dd/yyyy";
            //DateTime dt = Convert.ToDateTime(Post_date);
            //Post_date = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");

            //string dd = "";
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
            #region [Validation Part]
            bool isValidated = true;
            string qry = "";
            if (txtDOC_NO.Text != string.Empty)
            {
                if (ViewState["mode"] != null)
                {
                    if (ViewState["mode"].ToString() == "I")
                    {
                        string str = clsCommon.getString("select doc_no from " + tblHead + " where doc_no=" + txtDOC_NO.Text + " and Tran_Type='" + drpCashCredit.SelectedValue
                            + "' and Company_Code="
                            + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                        if (str != string.Empty)
                        {
                            lblMsg.Text = "Code " + txtDOC_NO.Text + " already exist";
                            this.getMaxCode();
                            hdnf.Value = txtDOC_NO.Text;
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
                setFocusControl(txtDOC_NO);
                return;
            }




            double CashAdvacne = 0;// txtCASH_ADVANCE.Text != string.Empty ? Convert.ToDouble(txtCASH_ADVANCE.Text) : 0.00;
            string Transport = "";// txtTransportCode.Text != string.Empty ? txtTransportCode.Text : "0";
            //if (CashAdvacne != 0)
            //{
            //    if (Transport == "0" || string.IsNullOrEmpty(Transport))
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Transport Is Compulsory!');", true);
            //        isValidated = false;
            //        setFocusControl(txtTransportCode);
            //        return;
            //    }
            //}
            #endregion


            #region -Head part declearation
            //  Int32 DOC_NO = txtDOC_NO.Text != string.Empty ? Convert.ToInt32(txtDOC_NO.Text) : 0;
            // TRAN_TYPE = drpCashCredit.SelectedValue.ToString();
            Created_By = Session["user"].ToString();
            Modified_By = Session["user"].ToString();
            deliver_type = drpDelivered.SelectedValue.ToString();
            DOC_DATE = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            CHALLAN_NO = txtChallanNo.Text;
            CHALLAN_DATE = DateTime.Parse(txtChallanDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            LORRYNO = txtLORRYNO.Text.ToUpper();
            AC_CODE = txtAC_CODE.Text != string.Empty ? Convert.ToInt32(txtAC_CODE.Text) : 0;
            DUE_DAYS = txtDUE_DAYS.Text != string.Empty ? Convert.ToInt32(txtDUE_DAYS.Text) : 0;
            DUE_DATE = DateTime.Parse(txtDueDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            TOTAL = txtTotal.Text != string.Empty ? Convert.ToDouble(txtTotal.Text) : 0;
            SUBTOTAL = txtSubtotal.Text != string.Empty ? Convert.ToDouble(txtSubtotal.Text) : 0;
            VAT = txtVatAmount.Text != string.Empty ? Convert.ToDouble(txtVatAmount.Text) : 0;
            ROUND_OFF = txtRoundOff.Text != string.Empty ? Convert.ToDouble(txtRoundOff.Text) : 0;
            hamali_amount = txthamliAmount.Text != string.Empty ? Convert.ToDouble(txthamliAmount.Text) : 0;
            GRAND_TOTAL = txtBILL_AMOUNT.Text != string.Empty ? Convert.ToDouble(txtBILL_AMOUNT.Text) : 0;
            CGSTRate = txtCGSTRate.Text != string.Empty ? Convert.ToDouble(txtCGSTRate.Text) : 0;
            CGSTAmount = txtCGSTAmount.Text != string.Empty ? Convert.ToDouble(txtCGSTAmount.Text) : 0;
            TRAN_TYPE = drpCashCredit.SelectedValue.ToString();
            IGSTRate = txtIGSTRate.Text != string.Empty ? Convert.ToDouble(txtIGSTRate.Text) : 0;
            IGSTAmount = txtIGSTAmount.Text != string.Empty ? Convert.ToDouble(txtIGSTAmount.Text) : 0;
            SGSTRate = txtSGSTRate.Text != string.Empty ? Convert.ToDouble(txtSGSTRate.Text) : 0;
            SGSTAmount = txtSGSTAmount.Text != string.Empty ? Convert.ToDouble(txtSGSTAmount.Text) : 0;
            GSTRateCode = txtGSTRateCode.Text != string.Empty ? Convert.ToInt32(txtGSTRateCode.Text) : 0;

            // Broker_CODE = txtBroker_Code.Text != string.Empty ? Convert.ToInt32(txtBroker_Code.Text) : 0;
            Broker_CODE = string.IsNullOrEmpty(txtBroker_Code.Text) ? 2 : int.Parse(txtBroker_Code.Text);
            DELIVERED = 0;
            narrtion = txtnarration.Text;
            Party_Name = txtparty_Name.Text;
            Party_Name_New = txtparty_Name_New.Text != string.Empty ? Convert.ToInt32(txtparty_Name_New.Text) : 0;
            // Bc = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + txtMillCode.Text + " and Company_Code=" + Session["Company_Code"].ToString() + "")
            BN = 0;
            try
            {
                BN = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as ccid from qrymstaccountmaster where Ac_Code=" + Broker_CODE + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
            }
            catch
            {
            }
            AN = 0;
            try
            {
                AN = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as ccid from qrymstaccountmaster where Ac_Code=" + txtAC_CODE.Text + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
            }
            catch
            {
            }
            purches_No = 0;
            try
            {
                purches_No = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as ccid from qrypurchaseselfbalance where purchaseid=" + txtPurchaseno.Text + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
            }
            catch
            {
            }

            if (deliver_type == "1")
            {
                DELIVERED = 1;
            }
            else
            {
                DELIVERED = 0;
            }
            string cashcredit = string.Empty;

            if (TRAN_TYPE == "CS" || TRAN_TYPE == "CS1")
            {
                cashcredit = "CS";
            }
            else
            {
                cashcredit = "CR";
            }

            int cashrecive = 1;
            if (!chkcashrecive.Checked)
            {
                cashrecive = 0;
            }
            //double SUBTOTAL = 0;// txtSUBTOTAL.Text != string.Empty ? Convert.ToDouble(txtSUBTOTAL.Text) : 0.00;
            double BILL_AMOUNT = txtBILL_AMOUNT.Text != string.Empty ? Convert.ToDouble(txtBILL_AMOUNT.Text) : 0.00;
            //Int32 DUE_DAYS = txtDUE_DAYS.Text != string.Empty ? Convert.ToInt32(txtDUE_DAYS.Text) : 0;
            string retValue = string.Empty;
            string strRev = string.Empty;
            int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            int Year_Code = Convert.ToInt32(Session["year"].ToString());
            int year_Code = Convert.ToInt32(Session["year"].ToString());
            int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
            string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");
            #endregion
            //double GSTRate = Convert.ToDouble(clsCommon.getString("select Rate from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
            //double cgstrate = Convert.ToDouble(clsCommon.getString("select CGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
            //double sgstrate = Convert.ToDouble(clsCommon.getString("select SGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
            //double igstrate = Convert.ToDouble(clsCommon.getString("select IGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));


            //double CGSTAmountForRR = 0.0;
            //double SGSTAmountForRR = 0.0;
            //double IGSTAmountForRR = 0.0;

            int SaleCGSTAc = Convert.ToInt32(System.Web.HttpContext.Current.Session["SaleCGSTAc"].ToString());
            int SaleSGSTAc = Convert.ToInt32(System.Web.HttpContext.Current.Session["SaleSGSTAc"].ToString());
            int SaleIGSTAc = Convert.ToInt32(System.Web.HttpContext.Current.Session["SaleIGSTAc"].ToString());

            #region Detail Fields
            Detail_Fields.Append("doc_no,");
            Detail_Fields.Append("detail_id,");
            Detail_Fields.Append("Tran_Type,");
            Detail_Fields.Append("Mill_Code,");
            Detail_Fields.Append("Item_Code,");
            Detail_Fields.Append("Quantity,");
            Detail_Fields.Append("Rate,");
            Detail_Fields.Append("Value,");
            Detail_Fields.Append("Vat_Ac,");
            Detail_Fields.Append("Billing_No,");
            Detail_Fields.Append("vat_percent,");
            Detail_Fields.Append("vat_amount,");
            Detail_Fields.Append("Gross,");
            Detail_Fields.Append("Company_Code,");
            Detail_Fields.Append("Year_Code,");
            Detail_Fields.Append("Branch_Code,");
            Detail_Fields.Append("grade,");
            Detail_Fields.Append("purchaseid,");

            Detail_Fields.Append("retailid,");
            Detail_Fields.Append("retaildetailid,");
            Detail_Fields.Append("ic,");
            Detail_Fields.Append("mc");
            #endregion

            if (btnSave.Text == "Save")
            {
                #region Head Add Fields And Values
                this.NextNumber();


                //Head_Fields = "doc_no,";
                //Head_Values = "'" + Doc_No + "',";


                Head_Fields.Append("doc_no,");
                Head_Values.Append("'" + Doc_No + "',");
                Head_Fields.Append("Tran_Type,");
                Head_Values.Append("'" + TRAN_TYPE + "',");
                Head_Fields.Append("Company_Code,");
                Head_Values.Append("'" + Company_Code + "',");
                Head_Fields.Append("doc_date,");
                Head_Values.Append("'" + DOC_DATE + "',");
                Head_Fields.Append("Challan_No,");
                Head_Values.Append("'" + CHALLAN_NO + "',");
                Head_Fields.Append("Vehical_No,");
                Head_Values.Append("'" + LORRYNO + "',");
                Head_Fields.Append("Challan_Date,");
                Head_Values.Append("'" + CHALLAN_DATE + "',");
                Head_Fields.Append("Party_Code,");
                Head_Values.Append("'" + AC_CODE + "',");

                Head_Fields.Append("Party_Name_New,");
                Head_Values.Append("'" + Party_Name_New + "',");
                Head_Fields.Append("Party_Name,");
                Head_Values.Append("'" + Party_Name + "',");
                Head_Fields.Append("Due_Days,");
                Head_Values.Append("'" + DUE_DAYS + "',");
                Head_Fields.Append("Due_Date,");
                Head_Values.Append("'" + DUE_DATE + "',");
                Head_Fields.Append("CashRecive,");
                Head_Values.Append("'" + cashrecive + "',");
                Head_Fields.Append("Total,");
                Head_Values.Append("'" + TOTAL + "',");
                Head_Fields.Append("Subtotal,");
                Head_Values.Append("'" + SUBTOTAL + "',");
                Head_Fields.Append("Vat,");
                Head_Values.Append("'" + VAT + "',");
                Head_Fields.Append("Round_Off,");
                Head_Values.Append("'" + ROUND_OFF + "',");
                Head_Fields.Append("Grand_Total,");
                Head_Values.Append("'" + GRAND_TOTAL + "',");
                Head_Fields.Append("Year_Code,");
                Head_Values.Append("'" + Year_Code + "',");

                Head_Fields.Append("Branch_Code,");
                Head_Values.Append("'" + Branch_Code + "',");
                Head_Fields.Append("Delivered,");
                Head_Values.Append("'" + DELIVERED + "',");
                Head_Fields.Append("Created_By,");
                Head_Values.Append("'" + Created_By + "',");
                Head_Fields.Append("GstRateCode,");
                Head_Values.Append("'" + GSTRateCode + "',");
                Head_Fields.Append("CGSTRate,");
                Head_Values.Append("'" + CGSTRate + "',");
                Head_Fields.Append("CGSTAmount,");
                Head_Values.Append("'" + CGSTAmount + "',");
                Head_Fields.Append("SGSTRate,");
                Head_Values.Append("'" + SGSTRate + "',");
                Head_Fields.Append("SGSTAmount,");
                Head_Values.Append("'" + SGSTAmount + "',");
                Head_Fields.Append("IGSTRate,");
                Head_Values.Append("'" + IGSTRate + "',");
                Head_Fields.Append("IGSTAmount,");
                Head_Values.Append("'" + IGSTAmount + "',");
                Head_Fields.Append("Broker_Code,");
                Head_Values.Append("'" + Broker_CODE + "',");
                Head_Fields.Append("HamaliAmount,");
                Head_Values.Append("'" + hamali_amount + "',");
                Head_Fields.Append("Narration,");
                Head_Values.Append("'" + narrtion + "',");
                Head_Fields.Append("retailid,");
                Head_Values.Append("'" + id + "',");
                Head_Fields.Append("bc");
                Head_Values.Append("'" + BN + "'");
                #endregion
                Head_Insert = "insert into " + tblHead + "(" + Head_Fields + ") values(" + Head_Values + ")";

                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Head_Insert;
                Maindt.Rows.Add(dr);
                #region[details]


                int retailGrid_id = Convert.ToInt32(clsCommon.getString("SELECT ifnull(count(retaildetailid),0) as retaildetailid from " + tblDetails + " "));
                if (retailGrid_id == 0)
                {
                    retailGrid_id = 0;
                }
                else
                {
                    retailGrid_id = Convert.ToInt32(clsCommon.getString("select max(retaildetailid) as retaildetailid from " + tblDetails + " "));
                }
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {

                    retailGrid_id = retailGrid_id + 1;
                    mill_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[3].Text);
                    // purches_No  =  Convert.ToInt32(Server.HtmlDecode(grdDetail.Rows[i].Cells[5].Text));
                    purches_No = Convert.ToInt32(grdDetail.Rows[i].Cells[5].Text);
                    grade = Server.HtmlDecode(grdDetail.Rows[i].Cells[6].Text);
                    item_code = Convert.ToInt32(grdDetail.Rows[i].Cells[7].Text);
                    //narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[4].Text);
                    Quantity = Convert.ToDouble(grdDetail.Rows[i].Cells[9].Text);
                    billing_no = Server.HtmlDecode(grdDetail.Rows[i].Cells[10].Text);
                    rate = Convert.ToDouble(grdDetail.Rows[i].Cells[11].Text);
                    Value1 = Convert.ToDouble(grdDetail.Rows[i].Cells[12].Text);
                    vat_ac = Convert.ToInt32(grdDetail.Rows[i].Cells[13].Text);
                    vat_percent = Convert.ToDouble(grdDetail.Rows[i].Cells[14].Text);
                    vat_amount = Convert.ToDouble(grdDetail.Rows[i].Cells[15].Text);
                    gross = Convert.ToDouble(grdDetail.Rows[i].Cells[16].Text);

                    Detail_Id = grdDetail.Rows[i].Cells[2].Text;
                    int mill_Id = 0;
                    try
                    {

                        mill_Id = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code='" + mill_Code + "' and Company_Code=" + Session["Company_Code"].ToString() + ""));
                    }
                    catch
                    {

                    }
                    int item_id = 0;
                    try
                    {
                        item_id = Convert.ToInt32(clsCommon.getString("select systemid from nt_1_systemmaster where System_Code='" + item_code + "' and Company_Code=" + Session["Company_Code"].ToString() + " and System_Type='I' "));
                    }
                    catch
                    {
                    }


                    Detail_Values.Append("('" + Doc_No + "','" + Detail_Id + "','" + TRAN_TYPE + "','" + mill_Code + "','" + item_code + "'" +
                 " ,'" + Quantity + "','" + rate + "','" + Value1 + "','" + vat_ac + "','" + billing_no + "','" + vat_percent + "'" +
                 " ,'" + vat_amount + "','" + gross + "','" + Company_Code + "','" + year_Code + "','" + Branch_Code + "','" + grade + "',Case when  0='" + purches_No + "' then null else '" + purches_No + "' end ,'" + id + "','" + retailGrid_id + "','" + item_id + "','" + mill_Id + "'),");

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
                #region[details]


                string concatid = string.Empty;
                int RetailDetailAuto_Id = Convert.ToInt32(clsCommon.getString("select max(retaildetailid) as retaildetailid from " + tblDetails + " "));
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {

                    mill_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[3].Text);
                    purches_No = Convert.ToInt32(Server.HtmlDecode(grdDetail.Rows[i].Cells[5].Text));
                    //  purches_No = Convert.ToInt32(grdDetail.Rows[i].Cells[5].Text);
                    grade = Server.HtmlDecode(grdDetail.Rows[i].Cells[6].Text);
                    item_code = Convert.ToInt32(grdDetail.Rows[i].Cells[7].Text);
                    //narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[4].Text);
                    Quantity = Convert.ToDouble(grdDetail.Rows[i].Cells[9].Text);
                    billing_no = Server.HtmlDecode(grdDetail.Rows[i].Cells[10].Text);
                    rate = Convert.ToDouble(grdDetail.Rows[i].Cells[11].Text);
                    Value1 = Convert.ToDouble(grdDetail.Rows[i].Cells[12].Text);
                    vat_ac = Convert.ToInt32(grdDetail.Rows[i].Cells[13].Text);
                    vat_percent = Convert.ToDouble(grdDetail.Rows[i].Cells[14].Text);
                    vat_amount = Convert.ToDouble(grdDetail.Rows[i].Cells[15].Text);
                    gross = Convert.ToDouble(grdDetail.Rows[i].Cells[16].Text);
                    int id = Convert.ToInt32(grdDetail.Rows[i].Cells[17].Text);
                    Detail_Id = grdDetail.Rows[i].Cells[2].Text;
                    int mill_Id = 0;
                    try
                    {
                        mill_Id = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code='" + mill_Code + "' and Company_Code=" + Session["Company_Code"].ToString() + ""));
                    }
                    catch
                    {
                    }
                    int item_id = 0;
                    try
                    {
                        item_id = Convert.ToInt32(clsCommon.getString("select systemid from nt_1_systemmaster where System_Code='" + item_code + "' and Company_Code=" + Session["Company_Code"].ToString() + " and System_Type='I' "));

                    }
                    catch
                    {
                    }
                    #region Insert Grid Record
                    if (grdDetail.Rows[i].Cells[18].Text == "A")
                    {
                        RetailDetailAuto_Id = RetailDetailAuto_Id + 1;

                        Detail_Values.Append("('" + txtDOC_NO.Text + "','" + Detail_Id + "','" + TRAN_TYPE + "','" + mill_Code + "','" + item_code + "', '" + Quantity + "','" + rate + "','" + Value1 + "'," +
                        "'" + vat_ac + "','" + billing_no + "','" + vat_percent + "','" + vat_amount + "','" + gross + "','" + Company_Code + "','" + year_Code + "','" + Branch_Code + "','" + grade + "',Case when  0='" + purches_No + "' then null else '" + purches_No + "' end,'" + lblDoc_Id.Text + "','" + RetailDetailAuto_Id + "','" + item_id + "','" + mill_Id + "'),");

                        //Detail_Values = Detail_Values + "('" + txtDOC_NO.Text + "','" + Detail_Id + "','" + TRAN_TYPE + "','" + mill_Code + "','" + grade + "','" + item_code + "','" + RetailDetailAuto_Id + "', '" + Quantity + "','" + billing_no + "','" + rate + "','" + Value1 + "'," +
                        //"'" + vat_ac + "','" + vat_percent + "','" + vat_amount + "','" + gross + "','" + mill_Id + "','" + item_id + "','" + id + "'),";
                    }
                    #endregion
                    #region Update Grid Record
                    if (grdDetail.Rows[i].Cells[18].Text == "U")
                    {


                        Detail_Update.Append("Mill_Code=case retaildetailid when '" + id + "' then '" + mill_Code + "'  ELSE Mill_Code END,");
                        Detail_Update.Append("Item_Code=case retaildetailid when '" + id + "' then '" + item_code + "'  ELSE Item_Code END,");
                        Detail_Update.Append("Quantity=case retaildetailid when '" + id + "' then '" + Quantity + "'  ELSE Quantity END,");
                        Detail_Update.Append("Billing_No=case retaildetailid when '" + id + "' then '" + billing_no + "'  ELSE Billing_No END,");
                        Detail_Update.Append("Rate=case retaildetailid when '" + id + "' then '" + rate + "'  ELSE Rate END,");
                        Detail_Update.Append("Value=case retaildetailid when '" + id + "' then '" + Value1 + "'  ELSE Value END,");
                        Detail_Update.Append("Vat_Ac=case retaildetailid when '" + id + "' then '" + vat_ac + "'  ELSE Vat_Ac END,");
                        Detail_Update.Append("vat_percent=case retaildetailid when '" + id + "' then '" + vat_percent + "'  ELSE vat_percent END,");
                        Detail_Update.Append("vat_amount=case retaildetailid when '" + id + "' then '" + vat_amount + "'  ELSE vat_amount END,");
                        Detail_Update.Append("Gross=case retaildetailid when '" + id + "' then '" + gross + "'  ELSE Gross END,");
                        Detail_Update.Append("grade=case retaildetailid when '" + id + "' then '" + grade + "'  ELSE grade END,");
                        Detail_Update.Append("purchaseid=case retaildetailid when '" + id + "' then Case when  0='" + purches_No + "' then null else '" + purches_No + "' end  ELSE purchaseid END,");
                        Detail_Update.Append("mc=case retaildetailid when '" + id + "' then '" + mill_Id + "'  ELSE mc END,");
                        Detail_Update.Append("ic=case retaildetailid when '" + id + "' then '" + item_id + "'  ELSE ic END,");


                        concatid = concatid + "'" + id + "',";


                    }
                    #endregion
                    #region Delete Grid Record
                    if (grdDetail.Rows[i].Cells[18].Text == "D")
                    {
                        Detail_Delete.Append("'" + grdDetail.Rows[i].Cells[17].Text + "',");
                    }
                    #endregion

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

                    string Detail_Deleteqry = "delete from " + tblDetails + " where retaildetailid in(" + Detail_Delete + ")";
                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = Detail_Deleteqry;
                    Maindt.Rows.Add(dr);

                }
                if (Detail_Update.Length > 0)
                {
                    concatid = concatid.Remove(concatid.Length - 1);
                    Detail_Update.Remove(Detail_Update.Length - 1, 1);

                    string Detail_Updateqry = "update " + tblDetails + " set " + Detail_Update + " where retaildetailid in(" + concatid + ")";
                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = Detail_Updateqry;
                    Maindt.Rows.Add(dr);

                }
                #endregion

                #region Update Head Add Fields And Values

                Head_Update.Append("doc_date=");
                Head_Update.Append("'" + DOC_DATE + "',");
                Head_Update.Append("Challan_No=");
                Head_Update.Append("'" + CHALLAN_NO + "',");
                Head_Update.Append("Vehical_No=");
                Head_Update.Append("'" + LORRYNO + "',");
                Head_Update.Append("Challan_Date=");
                Head_Update.Append("'" + CHALLAN_DATE + "',");
                Head_Update.Append("Party_Code=");
                Head_Update.Append("'" + AC_CODE + "',");

                Head_Update.Append("Party_Code=");
                Head_Update.Append("'" + Party_Name_New + "',");
                Head_Update.Append("Party_Name_New=");
                Head_Update.Append("'" + Party_Name_New + "',");

                Head_Update.Append("Due_Days=");
                Head_Update.Append("'" + DUE_DAYS + "',");
                Head_Update.Append("Due_Date=");
                Head_Update.Append("'" + DUE_DATE + "',");
                Head_Update.Append("Total=");
                Head_Update.Append("'" + TOTAL + "',");
                Head_Update.Append("Subtotal=");
                Head_Update.Append("'" + SUBTOTAL + "',");
                Head_Update.Append("Vat=");
                Head_Update.Append("'" + VAT + "',");
                Head_Update.Append("Round_Off=");
                Head_Update.Append("'" + ROUND_OFF + "',");
                Head_Update.Append("Year_Code=");
                Head_Update.Append("'" + Year_Code + "',");
                Head_Update.Append("Grand_Total=");
                Head_Update.Append("'" + GRAND_TOTAL + "',");
                Head_Update.Append("Branch_Code=");
                Head_Update.Append("'" + Branch_Code + "',");


                Head_Update.Append("Created_By=");
                Head_Update.Append("'" + Created_By + "',");
                Head_Update.Append("GstRateCode=");
                Head_Update.Append("'" + GSTRateCode + "',");
                Head_Update.Append("CGSTRate=");
                Head_Update.Append("'" + CGSTRate + "',");
                Head_Update.Append("CGSTAmount=");
                Head_Update.Append("'" + CGSTAmount + "',");
                Head_Update.Append("SGSTRate=");
                Head_Update.Append("'" + SGSTRate + "',");
                Head_Update.Append("SGSTAmount=");
                Head_Update.Append("'" + SGSTAmount + "',");


                Head_Update.Append("IGSTRate=");
                Head_Update.Append("'" + IGSTRate + "',");
                Head_Update.Append("IGSTAmount=");
                Head_Update.Append("'" + IGSTAmount + "',");
                Head_Update.Append("Broker_Code=");
                Head_Update.Append("'" + Broker_CODE + "',");
                Head_Update.Append("HamaliAmount=");
                Head_Update.Append("'" + hamali_amount + "',");
                //Head_Update.Append( "ac=");
                //Head_Update.Append( "'" + AN + "',");
                Head_Update.Append("bc=");
                Head_Update.Append("'" + BN + "',");
                Head_Update.Append("CashRecive=");
                Head_Update.Append("'" + cashrecive + "',");
                Head_Update.Append("Narration=");
                Head_Update.Append("'" + narrtion + "',");
                Head_Update.Append("Delivered=");
                Head_Update.Append("'" + DELIVERED + "'");

                string Head_Updateqry = "update " + tblHead + " set " + Head_Update + " where retailid='" + hdnf.Value + "'";
                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Head_Updateqry;
                Maindt.Rows.Add(dr);
                #endregion

                flag = 2;

            }
            #region Gledger Effect
            int ORDER_CODE = 0;
            FormTypes types = new FormTypes();
            StringBuilder Gledger_values = new StringBuilder();
            StringBuilder Gledger_Column = new StringBuilder();
            GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='RR' and Doc_No=" + Doc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = GLEDGER_Delete;
            Maindt.Rows.Add(dr);

            Gledger_Column.Append("TRAN_TYPE,CASHCREDIT,DOC_NO,DOC_DATE,AC_CODE,UNIT_code,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE," +
                        " YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO,ac,vc,progid,tranid");

            string SaleAcc = clsCommon.getString("select Sale_AC from nt_1_systemmaster where system_type='I' and system_code='1' and company_code=" + Session["Company_Code"].ToString() + "");
            string SaleCGST = clsCommon.getString("select CGSTAc from nt_1_companyparameters where Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + "");
            string SaleSGST = clsCommon.getString("select SGSTAc from nt_1_companyparameters where Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + "");
            string SaleIGST = clsCommon.getString("select IGSTAc from nt_1_companyparameters where Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + "");

            string ac1 = clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='1' and Company_Code=" + Session["Company_Code"].ToString() + "");
            string saleAcc_id = clsCommon.getString("select  ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='" + SaleAcc + "' and Company_Code=" + Session["Company_Code"].ToString() + "");
            string SaleCGST_id = clsCommon.getString("select  ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='" + SaleCGST + "' and Company_Code=" + Session["Company_Code"].ToString() + "");
            string saleSGST_id = clsCommon.getString("select  ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='" + SaleSGST + "' and Company_Code=" + Session["Company_Code"].ToString() + "");
            string saleIGST_id = clsCommon.getString("select  ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='" + SaleIGST + "' and Company_Code=" + Session["Company_Code"].ToString() + "");
            // Ac code Effect
            if (drpCashCredit.SelectedValue == "CS" || drpCashCredit.SelectedValue == "CS1")
            {
                ORDER_CODE = ORDER_CODE + 1;
                Gledger_values.Append("('RR','CS','" + Doc_No + "','" + DOC_DATE + "','1','0','','" + GRAND_TOTAL + "', " +
                                                          " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + ORDER_CODE + "','D','1',0,'" + Branch_Code + "','RR','" + Doc_No + "'," +
                                                          "case when 0='" + ac1 + "' then null else '" + ac1 + "' end,'0','" + types.TT_RS + "','0'),");
            }
            else
            {
                ORDER_CODE = ORDER_CODE + 1;
                Gledger_values.Append("('RR','CR','" + Doc_No + "','" + DOC_DATE + "','" + AC_CODE + "','0','','" + GRAND_TOTAL + "', " +
                                                         " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + ORDER_CODE + "','D','" + AC_CODE + "',0,'" + Branch_Code + "','RR','" + Doc_No + "'," +
                                                         " case when 0='" + AN + "' then null else '" + AN + "' end,'0','" + types.TT_RS + "','0'),");
            }
            // end Ac Code Effect

            // SaleCGST Acc
            if (CGSTAmount > 0)
            {
                ORDER_CODE = ORDER_CODE + 1;
                Gledger_values.Append("('RR','CR','" + Doc_No + "','" + DOC_DATE + "','" + SaleCGST + "','0','','" + CGSTAmount + "', " +
                                                            " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + ORDER_CODE + "','C',0,0,'" + Branch_Code + "','RR','" + Doc_No + "'," +
                                                            " case when 0='" + SaleCGST_id + "' then null else '" + SaleCGST_id + "' end,'0','" + types.TT_RS + "','0'),");
            }
            // End SaleCGST

            // SaleSGST Acc
            if (SGSTAmount > 0)
            {
                ORDER_CODE = ORDER_CODE + 1;
                Gledger_values.Append("('RR','CR','" + Doc_No + "','" + DOC_DATE + "','" + SaleSGST + "','0','','" + SGSTAmount + "', " +
                                                           " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + ORDER_CODE + "','C',0,0,'" + Branch_Code + "','RR','" + Doc_No + "'," +
                                                           " case when 0='" + saleSGST_id + "' then null else '" + saleSGST_id + "' end,'0','" + types.TT_RS + "','0'),");
            }
            // End SaleSGST

            // SaleIGST Acc
            if (IGSTAmount > 0)
            {
                ORDER_CODE = ORDER_CODE + 1;
                Gledger_values.Append("('RR','CR','" + Doc_No + "','" + DOC_DATE + "','" + SaleIGST + "','0','','" + IGSTAmount + "', " +
                                                             " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + ORDER_CODE + "','C',0,0,'" + Branch_Code + "','RR','" + Doc_No + "'," +
                                                             " case when 0='" + SaleCGST_id + "' then null else '" + saleIGST_id + "' end,'0','" + types.TT_RS + "','0'),");
            }
            // End SaleIGST

            //SaleAc
            ORDER_CODE = ORDER_CODE + 1;
            Gledger_values.Append("('RR','CR','" + Doc_No + "','" + DOC_DATE + "','" + SaleAcc + "','0','','" + (TOTAL + hamali_amount) + "', " +
                                                      " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + ORDER_CODE + "','C',0,0,'" + Branch_Code + "','RR','" + Doc_No + "'," +
                                                      " case when 0='" + saleAcc_id + "' then null else '" + saleAcc_id + "' end,'0','" + types.TT_RS + "','0'),");
            //End Ac
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
            msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);
            // msg = clsDAL.DataStore(Head_Insert, Head_Update, Head_Delete, Detail_Insert, Detail_Update, Detail_Delete, GLEDGER_Insert, GLEDGER_Delete, flag);
            if (msg == "Insert")
            {
                hdnf.Value = id.ToString();
                clsButtonNavigation.enableDisable("S");
                this.makeEmptyForm("S");
                qry = getDisplayQuery();
                this.fetchRecord(qry);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Added !')", true);
            }
            else if (msg == "Update")
            {
                hdnf.Value = lblDoc_Id.Text;
                clsButtonNavigation.enableDisable("S");
                this.makeEmptyForm("S");
                qry = getDisplayQuery();
                this.fetchRecord(qry);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update !')", true);
            }
            if (hdconfirm.Value == "Yes")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>SB();</script>", false);
                //csCalculations();
                //Calculation();
            }
        }
        catch
        {
        }
    }
    #endregion



    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string searchtxt = "";
            string delimStr = "";
            char[] delimiter = delimStr.ToCharArray();
            string words = "";
            string[] split = null;
            string name = string.Empty;
            if (searchString != string.Empty)
            {
                txtSearchText.Text = searchString;
                searchtxt = txtSearchText.Text;
                words = txtSearchText.Text;
                split = words.Split(delimiter);
            }
            else
            {

                txtSearchText.Text = txtSearchText.Text;
                searchtxt = txtSearchText.Text;
                words = txtSearchText.Text;
                split = words.Split(delimiter);
            }
            if (hdnfClosePopup.Value == "txtDOC_NO" || hdnfClosePopup.Value == "txtEditDoc_No")
            {
                if (btntxtdoc_no.Text == "Change No")
                {
                    pnlPopup.Style["display"] = "none";
                    txtDOC_NO.Text = string.Empty;
                    txtDOC_NO.Enabled = true;

                    btnSave.Enabled = false;
                    setFocusControl(txtDOC_NO);
                    hdnfClosePopup.Value = "Close";
                }
                if (btntxtdoc_no.Text == "Choose No")
                {
                    lblPopupHead.Text = "--Select DOC No--";
                    string qry = "select doc_no,doc_date,PartyName,PartyCity from " + qryCommon + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                        " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                        " and (doc_no like '%" + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%' or PartyName like '%" + txtSearchText.Text + "%' or PartyCity like '%" + txtSearchText.Text + "%') order by PartyName asc";
                    this.showPopup(qry);
                }
            }
            if (hdnfClosePopup.Value == "txtPURCNO")
            {
                lblPopupHead.Text = "--Select Purchase No--";
                //string qry = "Select s.doc_no as PurcNo,a.Ac_Name_E as mill,b.Ac_Name_E as broker,Convert(varchar(10),s.doc_date,103) as doc_date,s.NETQNTL from " + tblPrefix + "SugarPurchase s" +
                //            " left outer join " + tblPrefix + "AccountMaster a on a.Ac_Code=s.mill_code and a.Company_Code=s.Company_Code left outer join " + tblPrefix + "AccountMaster b on b.Ac_Code=s.BROKER and b.Company_Code=s.Company_Code" +
                //            " where s.doc_no NOT IN (Select k.PURCNO from " + tblPrefix + "SugarSale k where k.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and k.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ") and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and ( PURCNO like '%" + txtSearchText.Text + "%')";

                string qry = "select doc_no as PurcNo,PURCNO as DO_No,doc_date,MillName,PartyName,NETQNTL,Balance from "
                    + tblPrefix + "qrySugarPurchaseBalance  where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                    + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())
                    + " and Balance!=0 and (doc_no like '%" + txtSearchText.Text + "%' or MillName like '%"
                    + txtSearchText.Text + "%' or PartyName like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtAC_CODE")
            {
                lblPopupHead.Text = "--Select Party Code--";
                string qry = "select Ac_Code,Ac_Name_E,cityname from qrymstaccountmaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or cityname like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtBroker_Code")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    name += " Ac_Code Like '%" + aa + "%'or Ac_Name_E like '%" + aa + "%'  or cityname like '%" + aa + "%' or";

                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Party Code--";

                string qry = "select Ac_Code,Ac_Name_E,cityname from qrymstaccountmaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and Ac_type='BR' and  (" + name + ") order by Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtMillCode")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    name += " Ac_Code Like '%" + aa + "%'or Ac_Name_E like '%" + aa + "%'  or cityname like '%" + aa + "%' or";

                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Party Code--";
                string qry = "select Ac_Code,Ac_Name_E,cityname,group_Name_E from qrymstaccountmaster where Ac_type='M' and Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and (" + name + ") order by Ac_Name_E";

                this.showPopup(qry);

            }

            if (hdnfClosePopup.Value == "txtPurchaseno")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    name += " mill_code Like '%" + aa + "%'or millname like '%" + aa + "%'  or NETQNTL like '%" + aa + "%' or balance like '%" + aa + "%' or doc_no like '%" + aa + "%'  or purchaseid like '%" + aa + "%'  or ";

                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Party Code--";
                string qry = "select purchaseid ,mill_code,millname,NETQNTL,desp,balance,doc_dateConverted as Pur_Date,doc_no As Pur_No from qrypurchaseselfstockbalance " +
                    " where  balance!=0 and mill_code=" + txtMillCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " +
                    " and Year_Code=" + Session["year"].ToString() + "  and (" + name + ")  order by doc_dateConverted";

                this.showPopup(qry);

            }

            if (hdnfClosePopup.Value == "txtparty_Name_New")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    name += " Ac_Code Like '%" + aa + "%'or Ac_Name_E like '%" + aa + "%'  or cityname like '%" + aa + "%' or";

                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Party Code--";
                string qry = "select Ac_Code,Ac_Name_E,cityname from qrymstaccountmaster where Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_type='CR'   and (" + name + ") order by Ac_Name_E";
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtUnit_Code")
            {
                if (txtAC_CODE.Text != string.Empty)
                {
                    string iscarporate = clsCommon.getString("select carporate_party from " + AccountMasterTable + " where Ac_Code=" + txtAC_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (iscarporate == "Y")
                    {
                        lblMsg.Text = "";
                        lblPopupHead.Text = "--Select Unit--";
                        string qry = "select Unit_name,UnitName,unitCity from " + tblPrefix + "qryPartyUnitlist where Ac_Code=" + txtAC_CODE.Text +
                            " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (Unit_name like '%" + txtSearchText.Text + "%' or UnitName like '%" + txtSearchText.Text + "%' or unitCity like '%" + txtSearchText.Text + "%') order by UnitName";
                        this.showPopup(qry);
                    }
                    else
                    {
                        lblPopupHead.Text = "--Select Unit Code--";
                        string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                        this.showPopup(qry);
                    }
                }
                else
                {
                    lblMsg.Text = "Please Enter Ac_Code First!";
                    setFocusControl(txtAC_CODE);
                }
            }
            if (hdnfClosePopup.Value == "txtTransportCode")
            {
                lblPopupHead.Text = "--Select Transport Code--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtMILL_CODE")
            {
                lblPopupHead.Text = "--Select Mill--";

                string qry = "select Ac_Code as [Account Code], Ac_Name_E as [Account Name],Short_Name as [Short Name] from " + AccountMasterTable + " where (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%') and Ac_type='M' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtBROKER")
            {
                lblPopupHead.Text = "--Select Broker--";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " Left Outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".Company_Code where " + AccountMasterTable + ".Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtITEM_CODE")
            {
                lblPopupHead.Text = "--Select Item--";
                string qry = "select System_Code,System_Name_E as Item_Name from " + SystemMasterTable + " where System_Type='I' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtGSTRateCode")
            {
                lblPopupHead.Text = "--Select Item--";
                string qry = "select Doc_no,GST_Name,Rate from " + tblPrefix + "GSTRateMaster where ( Doc_no like '%" + txtSearchText.Text + "%' or GST_Name like '%" + txtSearchText.Text + "%' or Rate like '%" + txtSearchText.Text + "%') order by GST_Name"; ;
                this.showPopup(qry);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region csCalculations
    private void csCalculations()
    {
        try
        {
            if (strTextBox == "txtDOC_NO")
            {
                #region code
                try
                {
                    int n;
                    bool isNumeric = int.TryParse(txtDOC_NO.Text, out n);

                    if (isNumeric == true)
                    {
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        string txtValue = "";
                        if (txtDOC_NO.Text != string.Empty)
                        {
                            txtValue = txtDOC_NO.Text;
                            string qry = "select * from " + tblHead + " where  doc_no='" + txtValue + "' " +
                                "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

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
                                                //txtDoc_no.Enabled = false;
                                                hdnf.Value = txtDOC_NO.Text;
                                                btnSave.Enabled = true;   //IMP                                       
                                                //setFocusControl(txtPURCNO);
                                            }

                                            if (ViewState["mode"].ToString() == "U")
                                            {
                                                //fetch record
                                                qry = getDisplayQuery();
                                                bool recordExist = this.fetchRecord(qry);
                                                if (recordExist == true)
                                                {
                                                    txtDOC_NO.Enabled = false;
                                                    //setFocusControl(txtPURCNO);
                                                    pnlgrdDetail.Enabled = true;
                                                    hdnf.Value = txtDOC_NO.Text;

                                                }
                                            }
                                        }
                                    }
                                    else   //Record Not Found
                                    {
                                        if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                        {
                                            lblMsg.Text = "";
                                            //setFocusControl(txtPURCNO);
                                            txtDOC_NO.Enabled = false;
                                            btnSave.Enabled = true;   //IMP
                                        }
                                        if (ViewState["mode"].ToString() == "U")
                                        {
                                            this.makeEmptyForm("E");
                                            lblMsg.Text = "** Record Not Found";
                                            lblMsg.ForeColor = System.Drawing.Color.Red;
                                            txtDOC_NO.Text = string.Empty;
                                            setFocusControl(txtDOC_NO);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblMsg.Text = string.Empty;
                            setFocusControl(txtDOC_NO);
                        }
                    }
                    else
                    {
                        this.makeEmptyForm("A");
                        lblMsg.Text = "Doc No is numeric";
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        clsButtonNavigation.enableDisable("E");
                        txtDOC_NO.Text = string.Empty;
                        setFocusControl(txtDOC_NO);
                    }
                }
                catch
                {

                }
                #endregion
            }
            //if (strtextbox == "txtpurcno")
            //{
            //    setfocuscontrol(txtdoc_date);
            //}
            if (strTextBox == "txtDOC_DATE")
            {
                try
                {
                    string dt = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    if (clsCommon.isValidDate(dt) == true)
                    {
                        setFocusControl(txtChallanNo);
                    }
                    else
                    {
                        txtDOC_DATE.Text = "";
                        setFocusControl(txtDOC_DATE);
                    }
                }
                catch
                {
                    txtDOC_DATE.Text = "";
                    setFocusControl(txtDOC_DATE);
                }
            }
            if (strTextBox == "txtChallanDate")
            {
                try
                {
                    string dt = DateTime.Parse(txtChallanDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    if (clsCommon.isValidDate(dt) == true)
                    {
                        setFocusControl(txtLORRYNO);
                    }
                    else
                    {
                        txtChallanDate.Text = "";
                        setFocusControl(txtChallanDate);
                    }
                }
                catch
                {
                    txtChallanDate.Text = "";
                    setFocusControl(txtChallanDate);
                }
            }

            if (strTextBox == "txtAC_CODE")
            {
                string acname = "";
                if (txtAC_CODE.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtAC_CODE.Text);
                    if (a == false)
                    {
                        btntxtAC_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        acname = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_Code=" + txtAC_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (acname != string.Empty && acname != "0")
                        {
                            LblPartyname.Text = acname;
                            string address = clsCommon.getString("select Address_E from qrymstaccountmaster where Ac_Code=" + txtAC_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            string city = clsCommon.getString("select cityname from qrymstaccountmaster where Ac_Code=" + txtAC_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            lblPartyAddress.Text = address + " <br/>" + city;

                            setFocusControl(txtDOC_DATE);

                        }
                        else
                        {
                            txtAC_CODE.Text = string.Empty;
                            LblPartyname.Text = acname;
                            setFocusControl(txtAC_CODE);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtAC_CODE);
                }
            }

            if (strTextBox == "txtBroker_Code")
            {
                string brname = "";
                if (txtBroker_Code.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtBroker_Code.Text);
                    if (a == false)
                    {
                        btntxtBroker_Code_Click(this, new EventArgs());
                    }
                    else
                    {
                        brname = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_Code=" + txtBroker_Code.Text + " and Ac_type='BR'  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (brname != string.Empty && brname != "0")
                        {
                            lblbrokerName.Text = brname;
                            lblbroker_id.Text = brname;
                            setFocusControl(txtnarration);
                        }
                        else
                        {
                            txtBroker_Code.Text = string.Empty;
                            lblbrokerName.Text = brname;
                            setFocusControl(txtnarration);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtBroker_Code);
                }
            }

            if (strTextBox == "txtparty_Name")
            {
                setFocusControl(txtparty_Name_New);
            }
            if (strTextBox == "txtnarration")
            {
                setFocusControl(txtnarration);
            }


            if (strTextBox == "txtparty_Name_New")
            {
                string PartyName = "";
                if (txtparty_Name_New.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtparty_Name_New.Text);
                    if (a == false)
                    {
                        btntxtParty_Name_Click(this, new EventArgs());

                    }
                    else
                    {
                        PartyName = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_Code=" + txtparty_Name_New.Text
                            + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (PartyName != string.Empty && PartyName != "0")
                        {
                            lblParty_Name_New.Text = PartyName;
                            setFocusControl(txtLORRYNO);

                        }
                        else
                        {
                            txtparty_Name_New.Text = string.Empty;
                            lblParty_Name_New.Text = PartyName;
                            setFocusControl(txtLORRYNO);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtparty_Name_New);
                }
            }



            if (strTextBox == "txtDUE_DAYS")
            {
                setFocusControl(txtDueDate);
            }
            if (strTextBox == "txtCASH_ADVANCE")
            {
                setFocusControl(txtBILL_AMOUNT);
            }

            if (strTextBox == "txtLESS_FRT_RATE")
            {
                //setFocusControl(txtBankCommRate);
            }
            if (strTextBox == "txtChallanNo")
            {
                if (drpCashCredit.SelectedValue == "CR")
                {
                    setFocusControl(txtLORRYNO);
                }
            }
            if (strTextBox == "txtOTHER_AMT")
            {
                //setFocusControl(txtTransportCode);
            }
            if (strTextBox == "txtMillCode")
            {
                string brname = "";
                if (txtMillCode.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtMillCode.Text);
                    if (a == false)
                    {

                        btntxtMillCode_Click(this, new EventArgs());
                    }
                    else
                    {
                        brname = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_Code=" + txtMillCode.Text + " and Ac_type='M'  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (brname != string.Empty && brname != "0")
                        {

                            lblMillName.Text = brname;
                            lblgrade.Text = brname;
                            setFocusControl(btntxtPurchaseno);
                        }
                        else
                        {
                            txtMillCode.Text = string.Empty;
                            lblMillName.Text = brname;
                            setFocusControl(txtMillCode);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtBroker_Code);
                }

            }

            if (strTextBox == "txtPurchaseno")
            {
                string PartyName = "";
                if (txtPurchaseno.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtPurchaseno.Text);

                    if (a == false)
                    {
                        btntxtPurchaseno_Click(this, new EventArgs());

                    }
                    else
                    {
                        PartyName = clsCommon.getString("select millname from qrypurchaseselfstockbalance where mill_code=" + txtMillCode.Text
                            + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (PartyName != string.Empty && PartyName != "0")
                        {
                            // lblpurchaseno.Text = hdnfDoNo.Value;

                            setFocusControl(txtITEM_CODE);
                            // txtPurchaseno.Enabled = false;

                        }
                        else
                        {
                            txtPurchaseno.Text = string.Empty;
                            // lblpurchaseno.Text = PartyName;
                            setFocusControl(txtPurchaseno);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtPurchaseno);
                }
            }



            if (strTextBox == "txtTransportCode")
            {
            }

            if (strTextBox == "txtITEM_CODE")
            {
                string itemname = "";
                if (txtITEM_CODE.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtITEM_CODE.Text);
                    if (a == false)
                    {
                        btntxtITEM_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        itemname = clsCommon.getString("select System_Name_E from " + SystemMasterTable + " where System_Code=" + txtITEM_CODE.Text + "  and Company_Code="
                            + Convert.ToInt32(Session["Company_Code"].ToString()) + " and System_Type='I'");
                        if (itemname != string.Empty && itemname != "0")
                        {
                            ItemSelection(itemname, txtITEM_CODE.Text);

                            setFocusControl(txtQUANTAL);
                        }
                        else
                        {
                            txtITEM_CODE.Text = string.Empty;
                            LBLITEMNAME.Text = itemname;
                            setFocusControl(txtITEM_CODE);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtITEM_CODE);
                }
            }
            if (strTextBox == "txtGSTRateCode")
            {
                string gstname = "";
                if (txtGSTRateCode.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtGSTRateCode.Text);
                    if (a == false)
                    {
                        btntxtGSTRateCode_Click(this, new EventArgs());
                    }
                    else
                    {
                        gstname = clsCommon.getString("select GST_Name from nt_1_gstratemaster where Doc_no=" + txtGSTRateCode.Text + " ");
                        if (gstname != string.Empty && gstname != "0")
                        {
                            lblGSTRateName.Text = gstname;
                            // string trntype = "C";
                            // GSTCalc();
                            setFocusControl(txtBroker_Code);
                        }
                        else
                        {
                            txtGSTRateCode.Text = string.Empty;
                            lblGSTRateName.Text = gstname;
                            setFocusControl(txtGSTRateCode);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtGSTRateCode);
                }
            }

            if (strTextBox == "txtQUANTAL")
            {
                // double total = Convert.ToDouble(txtQUANTAL.Text != string.Empty ? txtQUANTAL.Text : "0.00") * Convert.ToDouble(txtRATE.Text != string.Empty ? txtRATE.Text : "0.00");
                //    txtITEMAMOUNT.Text = total.ToString();
                setFocusControl(txtRATE);
            }
            if (strTextBox == "txtBillingNo")
            {
                setFocusControl(txtGross);
            }
            if (strTextBox == "txtRoundOff")
            {
                Calculation();
            }
            if (strTextBox == "txthamliAmount")
            {
                Calculation();
            }
            if (strTextBox == "txtRATE")
            {
                //double total = Convert.ToDouble(txtQUANTAL.Text != string.Empty ? txtQUANTAL.Text : "0.00") * Convert.ToDouble(txtRATE.Text != string.Empty ? txtRATE.Text : "0.00");
                //txtITEMAMOUNT.Text = total.ToString();
                if (drpCashCredit.SelectedValue == "CR")
                {

                    setFocusControl(btnAdddetails);
                }
                else if (drpCashCredit.SelectedValue == "CS")
                {
                    setFocusControl(btnAdddetails);
                }
            }
            if (strTextBox == "txtDUE_DAYS")
            {
                int dueDays = txtDUE_DAYS.Text != string.Empty ? Convert.ToInt32(txtDUE_DAYS.Text) : 0;
                if (dueDays != 0)
                {
                    DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).AddDays(dueDays).ToString("dd/MM/yyyy");
                }
            }
            if (strTextBox == "txtLCST")
            {
                //  this.CSTCalculation();
                setFocusControl(txtSGSTRate);
            }

            if (strTextBox == "txtIGSTRate")
            {
                //this.IGSTRateCalculation();
                setFocusControl(txtIGSTRate);
            }

            if (strTextBox == "txtSGSTRate")
            {
                //this.SGSTRateCalculation();
                setFocusControl(txtIGSTRate);
            }
            if (strTextBox == "txtLORRYNO")
            {
                if (drpCashCredit.Text == "CS")
                {
                    setFocusControl(txtMillCode);
                }
                else if (drpCashCredit.SelectedValue == "CR")
                {
                    setFocusControl(txtBroker_Code);
                }
            }


            Calculation();
            GSTCalc();
            //CSTCalculation();

        }
        catch
        {
        }
    }

    private void Calculation()
    {
        #region calculation part
        double qtl = Convert.ToDouble("0" + txtQUANTAL.Text);
        Int32 packing = 0;// Convert.ToInt32("0" + txtPACKING.Text);
        Int32 bags = 0;

        double lessfreight = 0;// Convert.ToDouble("0" + txtLESS_FRT_RATE.Text);
        double freight = 0.00;

        double netQntl = 0.00;
        double subtotal = 0.00;
        double vatAmount = 0.00;
        double grossAmount = 0.00;
        double cashAdv = 0;// Convert.ToDouble("0" + txtCASH_ADVANCE.Text);
        double bankComm = 0;// Convert.ToDouble("0" + txtBANK_COMMISSION.Text);
        double other = 0;// Convert.ToDouble("0" + txtOTHER_AMT.Text);
        double billAmt = 0.00;
        double CGSTAmount = Convert.ToDouble("0" + txtCGSTAmount.Text);
        double SGSTAmount = Convert.ToDouble("0" + txtSGSTAmount.Text);
        double IGSTAmount = Convert.ToDouble("0" + txtIGSTAmount.Text);
        double alltotal = 0.00;
        double item_Amount = 0.00;
        double rate = Convert.ToDouble("0" + txtRATE.Text);
        double vatPercent = Convert.ToDouble("0" + txtVatPercent.Text);
        double vatTotal = 0;


        double hamliamount = txthamliAmount.Text != string.Empty ? Convert.ToDouble(txthamliAmount.Text) : 0.00;
        double roundoff = txtRoundOff.Text != string.Empty ? Convert.ToDouble(txtRoundOff.Text) : 0.00;

        if (Session["CompanyGSTStateCode"] == null || Session["CompanyGSTStateCode"] == string.Empty)
        {
            Session["CompanyGSTStateCode"] = clsCommon.getString("select GSTStateCode from nt_1_companyparameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " +
                "  Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
        }
        int companyGstStateCode = Convert.ToInt32(Session["CompanyGSTStateCode"].ToString());
        // int companyGstStateCode = 27;
        string aaa = "";
        if (txtAC_CODE.Text.Trim() != string.Empty)
        {
            bool a = clsCommon.isStringIsNumeric(txtAC_CODE.Text);
            if (a == true)
            {
                aaa = clsCommon.getString("select IFNULL(GSTStateCode,0) from " + tblPrefix + "AccountMaster where  Ac_Code=" + txtAC_CODE.Text + "");
            }
        }
        double gstRate = Convert.ToDouble(clsCommon.getString("select IFNULL(Rate,0) from  nt_1_gstratemaster where Doc_no=" + txtGSTRateCode.Text + " and Company_Code="
                            + Convert.ToInt32(Session["Company_Code"].ToString())));

        int partygstStateCode = 0;
        if (aaa.Trim().ToString() != "")
        {
            partygstStateCode = Convert.ToInt32(aaa);
        }


        if (qtl != 0 && packing != 0)
        {
            bags = Convert.ToInt32((qtl / packing) * 100);
            //txtBAGS.Text = bags.ToString();
        }
        else
        {
            //txtBAGS.Text = bags.ToString();
        }

        double kgperk = 100;
        string itemName = clsCommon.getString("select System_Name_E from " + SystemMasterTable + " where System_Code=" + txtITEM_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and System_Type='I'");
        if (itemName != string.Empty)
        {
            string kgPerKatta = clsCommon.getString("select IfNULL(KgPerKatta,100) as KgPerKatta from " + SystemMasterTable + " where System_Code=" + txtITEM_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and System_Type='I'");
            kgperk = Convert.ToDouble(kgPerKatta);
        }

        double kg = qtl * kgperk;
        double quintal = kg / 100;

        item_Amount = Math.Round((quintal * (rate)), 2);

        if (vatPercent != 0)
        {
            vatTotal = item_Amount * (vatPercent / 100);
        }

        txtVatTotal.Text = vatTotal.ToString();

        txtITEMAMOUNT.Text = item_Amount.ToString();

        txtGross.Text = (item_Amount + vatTotal).ToString();
        #region calculate subtotal
        if (grdDetail.Rows.Count > 0)
        {
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                if (grdDetail.Rows[i].Cells[18].Text != "D")
                {
                    double item_Amt = Convert.ToDouble(grdDetail.Rows[i].Cells[12].Text.Trim());
                    subtotal = subtotal + item_Amt;
                    double vat_Amt = Convert.ToDouble(grdDetail.Rows[i].Cells[15].Text.Trim());
                    vatAmount = vatAmount + vat_Amt;
                    double gross = Convert.ToDouble(grdDetail.Rows[i].Cells[16].Text.Trim());
                    grossAmount = grossAmount + gross;
                }
            }
            txtTotal.Text = subtotal.ToString();
            txtVatAmount.Text = vatAmount.ToString();
            txtBILL_AMOUNT.Text = grossAmount.ToString();
            GSTCalc();
        }
        #endregion


        if (partygstStateCode == companyGstStateCode)
        {
            double cgstRate = txtCGSTRate.Text != string.Empty ? Convert.ToDouble(txtCGSTRate.Text) : 0.0;
            double sgstRate = txtSGSTRate.Text != string.Empty ? Convert.ToDouble(txtSGSTRate.Text) : 0.0;
            double cgstAmountOnBillAmount = Math.Round(grossAmount / (100 + gstRate) * cgstRate, 2);
            double sgstAmountOnBillAmount = Math.Round(grossAmount / (100 + gstRate) * sgstRate, 2);

            double totalWithoutGST = Math.Round(grossAmount - (cgstAmountOnBillAmount + sgstAmountOnBillAmount), 2);

            txtTotal.Text = totalWithoutGST.ToString();


            txtCGSTAmount.Text = cgstAmountOnBillAmount.ToString();
            txtSGSTAmount.Text = sgstAmountOnBillAmount.ToString();

            //alltotal = subtotal + cgstAmountOnBillAmount + sgstAmountOnBillAmount + IGSTAmount;
            alltotal = subtotal;
            txtSubtotal.Text = alltotal.ToString();

            freight = Math.Round((lessfreight * netQntl), 2);
            //txtFREIGHT.Text = freight.ToString();
            //billAmt = subtotal;

            billAmt = (subtotal) + (roundoff) + (hamliamount);
            //billAmt = (subtotal + bankComm + cashAdv + other + CGSTAmount + SGSTAmount + IGSTAmount);
            txtBILL_AMOUNT.Text = billAmt.ToString();
        }
        else
        {
            double igstRate = txtIGSTRate.Text != string.Empty ? Convert.ToDouble(txtIGSTRate.Text) : 0.0;
            // double sgstRate = txtSGSTRate.Text != string.Empty ? Convert.ToDouble(txtSGSTRate.Text) : 0.0;
            //double cgstAmountOnBillAmount = Math.Round(grossAmount / 105 * cgstRate, 2);
            double igstAmountOnBillAmount = Math.Round(grossAmount / (100 + gstRate) * igstRate, 2);
            double totalWithoutGST = Math.Round(grossAmount - igstAmountOnBillAmount, 2);

            txtTotal.Text = totalWithoutGST.ToString();


            txtIGSTAmount.Text = igstAmountOnBillAmount.ToString();
            // txtSGSTAmount.Text = sgstAmountOnBillAmount.ToString();

            //alltotal = subtotal + cgstAmountOnBillAmount + sgstAmountOnBillAmount + IGSTAmount;
            alltotal = subtotal;
            txtSubtotal.Text = alltotal.ToString();

            freight = Math.Round((lessfreight * netQntl), 2);
            //txtFREIGHT.Text = freight.ToString();
            //billAmt = subtotal;
            billAmt = (subtotal) + (roundoff) + (hamliamount);
            //billAmt = (subtotal + bankComm + cashAdv + other + CGSTAmount + SGSTAmount + IGSTAmount);
            txtBILL_AMOUNT.Text = billAmt.ToString();
        }
        #endregion
    }

    private void GSTCalc()
    {
        int companyGstStateCode = Convert.ToInt32(Session["CompanyGSTStateCode"].ToString());
        string aaa = "";
        if (txtAC_CODE.Text.Trim() != string.Empty)
        {
            bool a = clsCommon.isStringIsNumeric(txtAC_CODE.Text);
            if (a == true)
            {
                aaa = clsCommon.getString("select IFNULL(GSTStateCode,0) from " + tblPrefix + "AccountMaster where  Ac_Code=" + txtAC_CODE.Text + "");
            }
        }
        int partygstStateCode = 0;
        if (aaa.Trim().ToString() != "")
        {
            partygstStateCode = Convert.ToInt32(aaa);
        }

        //txtCGSTRate.Text = clsCommon.getString("select CGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + txtGSTRateCode.Text + "and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        //CSTCalculation();
        //txtSGSTRate.Text = clsCommon.getString("select SGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + txtGSTRateCode.Text + "and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        //SGSTRateCalculation();

        //txtIGSTRate.Text = clsCommon.getString("select IGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + txtGSTRateCode.Text + "and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        //IGSTRateCalculation();
        if (companyGstStateCode == partygstStateCode)
        {
            txtCGSTRate.Text = clsCommon.getString("select CGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + txtGSTRateCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            CSTCalculation();
            txtSGSTRate.Text = clsCommon.getString("select SGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + txtGSTRateCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            SGSTRateCalculation();
            txtIGSTRate.Text = "";
            txtIGSTAmount.Text = "";

        }
        else
        {
            txtIGSTRate.Text = clsCommon.getString("select IGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + txtGSTRateCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            IGSTRateCalculation();

            txtCGSTRate.Text = "";
            txtCGSTAmount.Text = "";
            txtSGSTRate.Text = "";
            txtSGSTAmount.Text = "";
        }

    }

    #region[CSTCalculation]
    private void CSTCalculation()
    {
        double Totalamt = txtTotal.Text != string.Empty ? Convert.ToDouble(txtTotal.Text) : 0.00;
        double CSTtax = txtCGSTRate.Text != string.Empty ? Convert.ToDouble(txtCGSTRate.Text) : 0.00;
        double result = 0.00;

        CSTtax = CSTtax / 100;
        result = Totalamt * CSTtax;
        txtCGSTAmount.Text = result.ToString();

    }

    #endregion

    #region[SGSTRateCalculation]
    private void SGSTRateCalculation()
    {
        double Totalamt = txtTotal.Text != string.Empty ? Convert.ToDouble(txtTotal.Text) : 0;
        double LSTtax = txtSGSTRate.Text != string.Empty ? Convert.ToDouble(txtSGSTRate.Text) : 0;
        double result = 0;

        LSTtax = LSTtax / 100;
        result = Totalamt * LSTtax;
        txtSGSTAmount.Text = result.ToString();

    }

    #endregion

    #region[IGSTRateCalculation]
    private void IGSTRateCalculation()
    {
        double Totalamt = txtTotal.Text != string.Empty ? Convert.ToDouble(txtTotal.Text) : 0;
        double LITtax = txtIGSTRate.Text != string.Empty ? Convert.ToDouble(txtIGSTRate.Text) : 0;
        double result = 0;

        LITtax = LITtax / 100;
        result = Totalamt * LITtax;
        //txtIGSTRate.Text = result.ToString();
        txtIGSTAmount.Text = result.ToString();

    }
    #endregion

    private void ItemSelection(string itemname, string itemCode)
    {
        LBLITEMNAME.Text = itemname;
        string qryGetVatAc = "select Vat_AC from " + tblPrefix + "SystemMaster where System_Code=" + itemCode + " and System_Type='I' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
        string vatAc_Code = clsCommon.getString(qryGetVatAc);
        if (vatAc_Code != "0")
        {
            txtVatAc.Text = vatAc_Code;
            string qryVatAcDetail = "select Ac_Name_E from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + vatAc_Code + "";
            string qryVatRate = "select Ac_rate from " + tblPrefix + "AccountMaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + vatAc_Code + "";
            txtVatPercent.Text = clsCommon.getString(qryVatRate);
            lblVatAcName.Text = clsCommon.getString(qryVatAcDetail);
        }
    }
    #endregion


    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    }

    protected void txtDueDate_TextChanged(object sender, EventArgs e)
    {

    }


    protected void btntxtVatAc_Click(object sender, EventArgs e)
    {
    }


    protected void txtVatAc_TextChanged(object sender, EventArgs e)
    {
    }


    protected void txtVatTotal_TextChanged(object sender, EventArgs e)
    {
        searchString = txtVatTotal.Text;
        strTextBox = "txtVatTotal";
        csCalculations();
    }


    protected void txtCGSTRate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCGSTRate.Text;
        strTextBox = "txtLCST";
        csCalculations();

    }


    protected void txtSGSTRate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSGSTRate.Text;
        strTextBox = "txtSGSTRate";
        csCalculations();

    }


    protected void txtIGSTRate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtIGSTRate.Text;
        strTextBox = "txtIGSTRate";
        csCalculations();

    }


    protected void txtGSTRateCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGSTRateCode.Text;
        strTextBox = "txtGSTRateCode";
        csCalculations();

    }


    protected void btntxtGSTRateCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGSTRateCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    protected void btntxtParty_Name_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtparty_Name_New";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
}