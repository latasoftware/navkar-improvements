using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Threading;
public partial class pgeAccountsmaster : System.Web.UI.Page
{
    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string GroupMasterTable = string.Empty;
    string AcGroupsTable = string.Empty;
    string SystemMastertable = string.Empty;
    string qryCommon = string.Empty;
    string cityMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextbox = string.Empty;
    string qryDisplay = string.Empty;
    string docno = string.Empty;

    string strTextBox = string.Empty;
    int defaultAccountCode = 0;
    string GLedgerTable = string.Empty;
    string TranTyp = "OP";
    string qry = string.Empty;
    string Debit = string.Empty;
    string Credit = string.Empty;
    string DRCRDiff = string.Empty;
    static WebControl objAsp = null;
    string user = string.Empty;
    string f_pan = "";
    string f_Main = "~/PAN/" + clsGV.user;
    string isAuthenticate = string.Empty;
    string Action = string.Empty;
    string AccountNo = string.Empty;
    string cs = string.Empty;
    int count = 0;
    int Doc_No = 0;
    int Acid = 0;
    int bsGroup_Id = 0;
    int cityid = 0;
    string Head_Insert = string.Empty;
    string Head_Update = string.Empty;
    string Head_Delete = string.Empty;
    string GLEDGER_Insert = string.Empty;
    string GLEDGER_Delete = string.Empty;
    string Detail_Insert = string.Empty;
    string Detail_Update = string.Empty;
    string Detail_Delete = string.Empty;

    string Group_Insert = string.Empty;
    string Group_Update = string.Empty;
    string Group_Delete = string.Empty;

    MySqlConnection con = null;
    MySqlCommand cmd = null;
    MySqlTransaction myTran = null;

    #endregion

    #region -Head part declearation
    Int32 AC_CODE = 0;
    string Limit_By = string.Empty;
    string AC_TYPE = string.Empty;
    double AC_RATE = 0.00;
    string AC_NAME_E = string.Empty;
    string AC_NAME_R = string.Empty;
    double COMMISSION = 0.00;
    string SHORT_NAME = string.Empty;
    string ADDRESS_E = string.Empty;
    string ADDRESS_R = string.Empty;
    Int32 CITY_CODE = 0;
    Int32 PINCODE = 0;
    double OPENING_BALANCE = 0.00;
    double DISTANCE = 0.00;
    int GSTStateCode = 0;
    double Branch1OB = 0.00;
    string Branch1Drcr = string.Empty;
    double Branch2OB = 0.00;
    string Branch2Drcr = string.Empty;
    Int32 GROUP_CODE = 0;
    string LOCAL_LIC_NO = string.Empty;
    string BANK_NAME = string.Empty;
    string TIN_NO = string.Empty;
    string BANK_AC_NO = string.Empty;
    string CST_NO = string.Empty;
    string EMAIL_ID = string.Empty;
    string GST_NO = string.Empty;
    string EMAIL_ID_CC = string.Empty;
    string ADHARNO = string.Empty;
    string OTHER_NARRATIOM = string.Empty;
    string ECC_NO = string.Empty;
    string MOBILE = string.Empty;
    string IFSC = string.Empty;
    string FSSAI = string.Empty;
    double BANK_OPENING = 0.00;
    string BANK_OP_DRCR = string.Empty;
    string carporate_party = string.Empty;
    string WHATSUPNO = string.Empty;
    int UnregisterGST = 0;

    int locked = 0;

    string referBy = string.Empty;
    string OffPhone = string.Empty;
    string Fax = string.Empty;
    string CompanyPan = string.Empty;

    string AC_Pan = string.Empty;
    string retValue = string.Empty;
    string strRev = string.Empty;
    int Company_Code = 0;
    int Year_Code = 0;
    int year_Code = 0;
    int Branch_Code = 0;
    string Branch1DrCr = string.Empty;
    string Branch2DrCr = string.Empty;

    string GlegerNarration = string.Empty;
    string Created_By = string.Empty;
    string Modified_By = string.Empty;

    StringBuilder Head_Fields = null;
    StringBuilder Head_Values = null;
    string op = string.Empty;
    string returnmaxno = string.Empty;
    int flag = 0;
    string DRCR = string.Empty;
    string msg = string.Empty;
    #endregion-End of Head part declearation

    #region Detail Declaration
    Int32 personId = 0;
    string personName = "";
    string mobile = "";
    string email = "";
    string pan = "";
    string other = "";
    string i_d = "";
    string DOC_DATE = null;

    StringBuilder Detail_Fields = null;
    StringBuilder Detail_Values = null;

    int personid_No = 2;
    int personName_E = 3;
    int mobile_No = 4;
    int email_Id = 5;
    int pan_No = 6;
    int other_Info = 7;
    int id = 8;
    int rowAction = 9;
    int SrNo = 10;

    #endregion

    #region Group Detail_Values
    StringBuilder Group_Fields = null;
    StringBuilder Group_Values = null;
    string Group_Code = string.Empty;

    #endregion
    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;


    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "AccountMaster";
            tblDetails = tblPrefix + "AcContacts";
            GroupMasterTable = tblPrefix + "BSGroupMaster";
            AcGroupsTable = tblPrefix + "AcGroups";
            cityMasterTable = tblPrefix + "CityMaster";
            SystemMastertable = tblPrefix + "SystemMaster";
            qryCommon = "qrymstaccountmaster";
            GLedgerTable = tblPrefix + "GLEDGER";
            pnlPopup.Style["display"] = "none";
            user = Session["user"].ToString();

            cs = ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
            con = new MySqlConnection(cs);

            Group_Fields = new StringBuilder();
            Group_Values = new StringBuilder();
            Head_Fields = new StringBuilder();
            Head_Values = new StringBuilder();
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
                    if (Action == "1")
                    {
                        hdnf.Value = Request.QueryString["Ac_Code"];
                        pnlPopup.Style["display"] = "none";
                        ViewState["currentTable"] = null;
                        clsButtonNavigation.enableDisable("N");

                        this.makeEmptyForm("N");
                        ViewState["mode"] = "I";
                        //this.DebitCreditDiff();
                        this.showLastRecord();
                        this.fillGroupsGrid();
                        setFocusControl(btnEdit);
                    }
                    else
                    {
                        clsButtonNavigation.enableDisable("A");
                        ViewState["mode"] = null;
                        ViewState["mode"] = "I";
                        this.makeEmptyForm("A");
                        this.NextNumber();
                        this.fillGroupsGrid();
                        btntxtAC_CODE.Enabled = false;
                        txtGROUP_CODE.Text = "24";
                        lblGROUPNAME.Text = clsCommon.getString("Select group_Name_E from " + tblPrefix + "BSGroupMaster where group_Code=24 and Company_Code="
                            + Convert.ToInt32(Session["Company_Code"].ToString()));
                        pnlPopupDetails.Style["display"] = "none";
                        drpDrCr.Enabled = true;
                        // disableOpening();
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
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void DebitCreditDiff()
    {
        try
        {
            Debit = Convert.ToString(clsCommon.getString("select SUM(AMOUNT) from " + tblPrefix + "GLEDGER where COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and Drcr='D' and Tran_Type='OP'"));
            if (Debit == string.Empty)
            {
                Debit = "0";
            }
            Credit = Convert.ToString(clsCommon.getString("select SUM(AMOUNT) from " + tblPrefix + "GLEDGER where COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and Drcr='C' and Tran_Type='OP'"));
            if (Credit == string.Empty)
            {
                Credit = "0";
            }
            DRCRDiff = Convert.ToString(Convert.ToDouble(Debit) - Convert.ToDouble(Credit));
            double value = 0;
            double diffn = double.Parse(DRCRDiff);
            if (diffn < 0)
            {
                value = Math.Abs(Math.Round(diffn, 2));
                lblDRCRDiff.Text = Convert.ToString(value);
                lblDRCRDiff.ForeColor = Color.Red;
            }
            else
            {
                value = Math.Abs(Math.Round(diffn, 2));
                lblDRCRDiff.Text = Convert.ToString(value);
                lblDRCRDiff.ForeColor = Color.Yellow;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void fillGroupsGrid()
    {
        try
        {
            string qry = "select System_Code,System_Name_E from " + SystemMastertable + " where System_Type='G' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            grdGroup.DataSource = ds;
            grdGroup.DataBind();
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
                obj.tableName = tblHead + " where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code>100";
                obj.code = "Ac_Code";
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
                                    if (ds.Tables[0].Rows[0][0].ToString() != "1")
                                    {
                                        txtAC_CODE.Text = ds.Tables[0].Rows[0][0].ToString();
                                        txtAC_CODE.Enabled = false;
                                    }
                                    else
                                    {
                                        txtAC_CODE.Text = "101";
                                    }
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
                btntxtGstStateCode.Enabled = false;
                txtEditDoc_No.Enabled = true;
                pnlPopup.Style["display"] = "none";
                btnOpenDetailsPopup.Enabled = false;
                btnSave.Text = "Save";
                btntxtAC_CODE.Text = "Choose No";
                btntxtAC_CODE.Enabled = true;
                btnAddCity.Enabled = false;
                btnAddGroup.Enabled = false;
                txtAC_CODE.Enabled = false;
                lblMsg.Text = string.Empty;

                txtSendingAcCode.Enabled = true;
                #region logic
                btntxtCITY_CODE.Enabled = false;
                btntxtGROUP_CODE.Enabled = false;
                drpType.Enabled = false;
                drpDrCr.Enabled = false;
                lblCITYNAME.Text = string.Empty;
                lblGROUPNAME.Text = string.Empty;
                pnlGroup.Enabled = false;
                btntxtAC_CODE.Enabled = false;
                chkCarporate.Enabled = false;
                btnOpenDetailsPopup.Enabled = false;
                #endregion
                txtADDRESS_R.Enabled = false;
                txtBANK_OPENING.Enabled = false;
                drpBankDrCr.Enabled = false;
                drpLimit.Enabled = false;
            }
            if (dAction == "A")
            {
                drpType.SelectedIndex = 0;
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        ((System.Web.UI.WebControls.TextBox)c).Text = "";
                        ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                    }
                }
                btntxtGstStateCode.Enabled = true;
                txtEditDoc_No.Enabled = false;
                lbltxtGstStateName.Text = "";
                btnSave.Text = "Save";
                btntxtAC_CODE.Text = "Change No";
                txtAC_CODE.Enabled = false;
                btnOpenDetailsPopup.Enabled = true;
                txtSendingAcCode.Enabled = true;
                #region set Business logic for add
                setFocusControl(drpType);
                btntxtCITY_CODE.Enabled = true;
                btntxtGROUP_CODE.Enabled = true;
                lblCITYNAME.Text = string.Empty;
                lblGROUPNAME.Text = string.Empty;
                drpType.Enabled = true;
                drpDrCr.Enabled = true;

                pnlGroup.Enabled = true;
                chkCarporate.Enabled = true;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                btnOpenDetailsPopup.Enabled = true;
                chkCarporate.Checked = false;
                btnAddCity.Enabled = true;
                btnAddGroup.Enabled = true;
                for (int i = 0; i < grdGroup.Rows.Count; i++)
                {
                    CheckBox chk = (CheckBox)grdGroup.Rows[i].Cells[2].FindControl("chk");
                    chk.Checked = false;
                }
                #endregion
                txtADDRESS_R.Enabled = true;

                txtOPENING_BALANCE.Enabled = true;
                drpDrCr.Enabled = true;
                btntxtAC_CODE.Enabled = false;

                txtBANK_OPENING.Enabled = true;
                drpBankDrCr.Enabled = true;
                drpLimit.Enabled = true;
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
                btntxtGstStateCode.Enabled = false;
                txtEditDoc_No.Enabled = true;
                btntxtAC_CODE.Text = "Choose No";
                txtAC_CODE.Enabled = false;
                btntxtAC_CODE.Enabled = false;
                btnAddCity.Enabled = false;
                btnAddGroup.Enabled = false;
                txtADDRESS_R.Enabled = false;
                txtSendingAcCode.Enabled = true;
                #region logic
                btntxtCITY_CODE.Enabled = false;
                btntxtGROUP_CODE.Enabled = false;
                drpType.Enabled = false;
                drpDrCr.Enabled = false;
                //lblCITYNAME.Text = string.Empty;
                //lblGROUPNAME.Text = string.Empty;
                pnlGroup.Enabled = false;
                btnOpenDetailsPopup.Enabled = false;
                chkCarporate.Enabled = false;
                #endregion

                txtBANK_OPENING.Enabled = false;
                drpBankDrCr.Enabled = false;
                drpLimit.Enabled = false;
            }
            if (dAction == "E")
            {
                foreach (System.Web.UI.Control c in pnlMain.Controls)
                {
                    if (c is System.Web.UI.WebControls.TextBox)
                    {
                        if (drpType.SelectedValue == "F")
                        {
                            FixedAssetsControls();
                        }
                        else
                        {
                            ((System.Web.UI.WebControls.TextBox)c).Enabled = true;
                        }
                    }
                }
                btntxtGstStateCode.Enabled = true;
                txtEditDoc_No.Enabled = false;
                btntxtAC_CODE.Text = "Choose No";
                txtAC_CODE.Enabled = true;
                lblMsg.Text = string.Empty;
                txtAC_NAME_R.Enabled = true;

                txtSHORT_NAME.Enabled = true;
                btnAddCity.Enabled = true;
                btnAddGroup.Enabled = true;
                txtSendingAcCode.Enabled = true;
                #region logic
                drpDrCr.Enabled = true;
                btntxtGROUP_CODE.Enabled = true;
                drpType.Enabled = true;

                //Ckecking if the type is fixed assets
                if (drpType.SelectedValue != "F")
                {
                    btntxtAC_CODE.Enabled = true;
                    pnlGroup.Enabled = true;
                    btntxtCITY_CODE.Enabled = true;
                    btnOpenDetailsPopup.Enabled = true;
                    chkCarporate.Enabled = true;
                }
                #endregion

                txtBANK_OPENING.Enabled = true;
                drpBankDrCr.Enabled = true;
                drpLimit.Enabled = true;
            }
            #region always check
            if (dAction == "A" || dAction == "E")
            {
                string s_item = drpType.SelectedValue;
                if (s_item == "I")
                {
                    setFocusControl(txtAC_RATE);
                    //    txtCOMMISSION.Text = "";
                    txtCOMMISSION.Enabled = false;
                    //    txtADDRESS_E.Text = "";
                    txtADDRESS_E.Enabled = false;
                    //     txtADDRESS_R.Text = "";
                    // txtADDRESS_R.Enabled = false;
                    //     txtCITY_CODE.Text = "";
                    //      lblCITYNAME.Text = "";
                    txtCITY_CODE.Enabled = false;
                    //     txtPINCODE.Text = "";
                    txtPINCODE.Enabled = false;
                    //     txtOPENING_BALANCE.Text = "";
                    txtOPENING_BALANCE.Enabled = false;
                    drpDrCr.Enabled = false;
                    //    txtLOCAL_LIC_NO.Text = "";
                    txtLOCAL_LIC_NO.Enabled = false;
                    //   txtTIN_NO.Text = "";

                    //    txtGST_NO.Text = "";
                    txtGST_NO.Enabled = false;
                    //    txtBANK_OPENING.Text = "";
                    // txtBANK_OPENING.Enabled = false;
                    //drpBankDrCr.Enabled = false;
                }
                else if (drpType.SelectedValue != "F")
                {
                    //  txtCOMMISSION.Text = "";
                    txtCOMMISSION.Enabled = true;
                    //    txtADDRESS_E.Text = "";
                    txtADDRESS_E.Enabled = true;
                    //    txtADDRESS_R.Text = "";
                    // txtADDRESS_R.Enabled = true;
                    //    txtCITY_CODE.Text = "";
                    //    lblCITYNAME.Text = "";
                    txtCITY_CODE.Enabled = true;
                    //     txtPINCODE.Text = "";
                    txtPINCODE.Enabled = true;
                    //     txtOPENING_BALANCE.Text = "";
                    txtOPENING_BALANCE.Enabled = true;
                    drpDrCr.Enabled = true;
                    //   txtLOCAL_LIC_NO.Text = "";
                    txtLOCAL_LIC_NO.Enabled = true;
                    //   txtTIN_NO.Text = "";

                    //    txtGST_NO.Text = "";
                    txtGST_NO.Enabled = true;
                    //    //     txtBANK_OPENING.Text = "";
                    //  txtBANK_OPENING.Enabled = true;
                    //  drpBankDrCr.Enabled = true;
                    grdGroup.Enabled = true;
                    btntxtAC_CODE.Enabled = true;
                }
                if (s_item == "B")
                {
                    //   txtBANK_OPENING.Text = "";
                    //  txtBANK_OPENING.Enabled = true;
                    // drpBankDrCr.Enabled = true;
                }
                else
                {
                    //   txtBANK_OPENING.Text = "";
                    //txtBANK_OPENING.Enabled = false;
                    // drpBankDrCr.Enabled = false;
                }

                if (s_item == "F" || s_item == "I")
                {
                    btntxtAC_CODE.Enabled = true;

                    setFocusControl(txtAC_RATE);
                    //  txtAC_RATE.Text = "";
                    txtAC_RATE.Enabled = true;
                }
                else
                {
                    // txtAC_RATE.Text = "";
                    txtAC_RATE.Enabled = false;
                }
                if (s_item == "O" || s_item == "E")
                {
                    btntxtAC_CODE.Enabled = true;

                    TradingAndExpensesControls();
                }
            }

            #endregion
        }
        catch
        {
        }
    }
    private void TradingAndExpensesControls()
    {
        txtCOMMISSION.Enabled = false;
        txtADDRESS_E.Enabled = true;
        // txtADDRESS_R.Enabled = false;
        txtCITY_CODE.Enabled = false;
        txtPINCODE.Enabled = false;
        txtOPENING_BALANCE.Enabled = false;
        drpDrCr.Enabled = false;
        txtLOCAL_LIC_NO.Enabled = false;

        txtGST_NO.Enabled = false;
        txtBANK_OPENING.Enabled = false;
        drpBankDrCr.Enabled = false;
        chkCarporate.Enabled = false;
        txtBANK_AC_NO.Enabled = false;
        txtBANK_NAME.Enabled = false;
        txtEMAIL_ID.Enabled = false;
        txtEMAIL_ID_CC.Enabled = false;

        txtRefBy.Enabled = false;
        txtOffPhone.Enabled = false;
        txtcompanyPan.Enabled = false;
        txtfax.Enabled = false;
        btnOpenDetailsPopup.Enabled = false;
        grdGroup.Enabled = false;
        txtAC_RATE.Enabled = false;
        txtOTHER_NARRATION.Enabled = false;
        txtAdhar_No.Enabled = false;
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
                btnAdd.Focus();
            }
            else                     //new code
            {
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }




        }
        catch
        {
        }
    }
    #endregion



    #region [First]
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select AC_CODE from " + tblHead + " where AC_CODE=(select MIN(AC_CODE) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
            if (txtAC_CODE.Text != string.Empty)
            {
                string query = "SELECT top 1 [AC_CODE] from " + tblHead + " where AC_CODE<" + Convert.ToInt32(hdnf.Value) +
                                " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " order by Ac_Code desc";
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
            if (txtAC_CODE.Text != string.Empty)
            {
                string query = "SELECT top 1 [AC_CODE] from " + tblHead + " where AC_CODE>" + Convert.ToInt32(hdnf.Value) +
                                " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " order by Ac_Code asc";
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
            query = "select AC_CODE from " + tblHead + " where AC_CODE=(select MAX(AC_CODE) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
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
        this.fillGroupsGrid();
        txtGROUP_CODE.Text = "24";
        lblGROUPNAME.Text = clsCommon.getString("Select group_Name_E from " + tblPrefix + "BSGroupMaster where group_Code=24 and Company_Code="
            + Convert.ToInt32(Session["Company_Code"].ToString()));
        //disableOpening();

    }

    private void disableOpening()
    {
        int yearCode = Convert.ToInt32(Session["year"].ToString());
        if (yearCode > 1)
        {
            txtOPENING_BALANCE.Enabled = false;
            drpDrCr.Enabled = false;

        }
    }
    #endregion

    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        clsButtonNavigation.enableDisable("E");
        txtOPENING_BALANCE.Enabled = true;
        drpDrCr.Enabled = true;
        pnlgrdDetail.Enabled = true;
        this.makeEmptyForm("E");

        setFocusControl(txtAC_NAME_E);
    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string currentDoc_No = txtAC_CODE.Text;
            if (hdconfirm.Value == "Yes")
            {
                // string str = clsCommon.getString("select DOC_NO from " + GLedgerTable + " where DOC_NO=" + txtAC_CODE.Text
                //     + " and TRAN_TYPE!='" + TranTyp + "' and COMPANY_CODE='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
                //if (str == string.Empty)   //Gledger does not contain this account then delete
                //{
                flag = 3;
                Head_Delete = "delete from " + tblHead + " where accoid='" + lblAc_Code.Text + "'";
                Detail_Delete = "delete from " + tblDetails + " where accoid='" + lblAc_Code.Text + "'";
                GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='OP' and Doc_No=" + currentDoc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " ";
                Group_Delete = "delete from " + AcGroupsTable + " where accoid='" + lblAc_Code.Text + "'";

                msg = clsDAL.DataStore(Head_Insert, Head_Update, Head_Delete, Detail_Insert, Detail_Update, Detail_Delete, GLEDGER_Insert, GLEDGER_Delete, flag);
                if (msg == "Delete")
                {
                    DataSet ds = clsDAL.SimpleQuery(Group_Delete);
                    Response.Redirect("../Master/pgeAccountUtility.aspx");
                }
                //Thread thred = new Thread(() => { count = DataStore(flag); }); //Calling DataStore Method Using Thread
                //thred.Start(); //Thread Operation Start
                //thred.Join();

                //if (count == 3)
                //{
                //    Response.Redirect("../Master/pgeAccMaster_Data.aspx");
                //}

                //}
                //else
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Cannot delete this Account , it is in use!')", true);
                //    // lblMsg.Text = "Cannot delete this Account , it is in use";
                //    //lblMsg.ForeColor = System.Drawing.Color.Red;
                //}

            }
            else
            {
                hdnf.Value = lblAc_Code.Text;
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
        ClearSendingSmsTextboxes();

        int maxno = Convert.ToInt32(clsCommon.getString("select ifnull(max(accoid),0) as Accode from qrymstaccountmaster where Company_Code=" + Session["Company_Code"].ToString() + " "));

        hdnf.Value = Convert.ToString(maxno);
        clsButtonNavigation.enableDisable("S");
        this.makeEmptyForm("S");
        qry = getDisplayQuery();
        this.fetchRecord(qry);

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
                        txtAC_CODE.Text = dt.Rows[0]["AC_CODE"].ToString();

                        txtAC_NAME_E.Text = dt.Rows[0]["AC_NAME_E"].ToString();
                        txtAC_RATE.Text = dt.Rows[0]["AC_RATE"].ToString();
                        txtAC_NAME_R.Text = dt.Rows[0]["AC_NAME_R"].ToString();
                        txtCOMMISSION.Text = dt.Rows[0]["COMMISSION"].ToString();
                        txtSHORT_NAME.Text = dt.Rows[0]["SHORT_NAME"].ToString();
                        txtADDRESS_E.Text = dt.Rows[0]["ADDRESS_E"].ToString();
                        txtADDRESS_R.Text = dt.Rows[0]["ADDRESS_R"].ToString();
                        txtCITY_CODE.Text = dt.Rows[0]["CITY_CODE"].ToString();
                        lblCITYNAME.Text = dt.Rows[0]["CityName"].ToString();
                        txtGstStateCode.Text = dt.Rows[0]["GSTStateCode"].ToString();
                        lbltxtGstStateName.Text = dt.Rows[0]["citystate"].ToString();
                        txtPINCODE.Text = dt.Rows[0]["PINCODE"].ToString();
                        txtOPENING_BALANCE.Text = dt.Rows[0]["OPENING_BALANCE"].ToString();
                        txtGROUP_CODE.Text = dt.Rows[0]["GROUP_CODE"].ToString();
                        drpDrCr.SelectedValue = dt.Rows[0]["DRCR"].ToString();
                        txtwhatsup_No.Text = dt.Rows[0]["whatsup_no"].ToString();
                        drpLimit.SelectedValue = dt.Rows[0]["Limit_By"].ToString();
                        string Branch1DrCr = dt.Rows[0]["Branch1Drcr"].ToString();

                        lblGROUPNAME.Text = dt.Rows[0]["Group_Name_E"].ToString();

                        txtLOCAL_LIC_NO.Text = dt.Rows[0]["LOCAL_LIC_NO"].ToString();
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
                        //lblCreated_Date.Text = dt.Rows[0]["Created_Date"].ToString();
                        //lblModified.Text = dt.Rows[0]["Modified_By"].ToString();
                        //lblModified_Date.Text = dt.Rows[0]["Modified_Date"].ToString();
                        txtBANK_NAME.Text = dt.Rows[0]["BANK_NAME"].ToString();
                        txtIfsc.Text = dt.Rows[0]["IFSC"].ToString();

                        txtBANK_AC_NO.Text = dt.Rows[0]["BANK_AC_NO"].ToString();

                        txtEMAIL_ID.Text = dt.Rows[0]["EMAIL_ID"].ToString();
                        txtGST_NO.Text = dt.Rows[0]["GST_NO"].ToString();
                        txtEMAIL_ID_CC.Text = dt.Rows[0]["EMAIL_ID_CC"].ToString();

                        txtAdhar_No.Text = dt.Rows[0]["adhar_no"].ToString();
                        txtOTHER_NARRATION.Text = dt.Rows[0]["OTHER_NARRATION"].ToString();

                        txtFssaiNo.Text = dt.Rows[0]["FSSAI"].ToString();
                        txtcompanyPan.Text = dt.Rows[0]["CompanyPan"].ToString();
                        txtBANK_OPENING.Text = dt.Rows[0]["BANK_OPENING"].ToString();
                        drpBankDrCr.SelectedValue = dt.Rows[0]["BANK_OP_DRCR"].ToString();
                        txtMOBILE.Text = dt.Rows[0]["Mobile_No"].ToString();
                        txtOffPhone.Text = dt.Rows[0]["OffPhone"].ToString();
                        txtRefBy.Text = dt.Rows[0]["referBy"].ToString();
                        txtfax.Text = dt.Rows[0]["Fax"].ToString();
                        drpType.SelectedValue = dt.Rows[0]["AC_TYPE"].ToString();
                        txtDistance.Text = dt.Rows[0]["Distance"].ToString();
                        Acid = Convert.ToInt32(dt.Rows[0]["accoid"].ToString());
                        lblAc_Code.Text = dt.Rows[0]["accoid"].ToString();
                        lblgroupid.Text = dt.Rows[0]["bsid"].ToString();
                        lblcityid.Text = dt.Rows[0]["cityid"].ToString();
                        lblAc_Code.Text = dt.Rows[0]["accoid"].ToString();
                        if (dt.Rows[0]["Carporate_Party"].ToString() == "Y")
                        {
                            chkCarporate.Checked = true;
                        }
                        else
                        {
                            chkCarporate.Checked = false;
                        }

                        string abcd = dt.Rows[0]["UnregisterGST"].ToString();
                        chkUnregisterGST.Checked = Convert.ToBoolean(abcd);

                        string locked_check = dt.Rows[0]["Locked"].ToString();
                        chkLocked.Checked = Convert.ToBoolean(locked_check);


                        recordExist = true;
                        hdnf.Value = Acid.ToString();
                        lblMsg.Text = "";

                        string branch1code = clsCommon.getString("select Branch1 from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        // lblBranch1.Text = clsCommon.getString("select Branch from BranchMaster where Branch_Id=" + branch1code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string branch2code = clsCommon.getString("select Branch2 from " + tblPrefix + "CompanyParameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        //  lblBranch2.Text = clsCommon.getString("select Branch from BranchMaster where Branch_Id=" + branch2code + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));


                        #region Account Details
                        qry = "SELECT  PersonId as personid_No, Person_Name, Person_Mobile as Mobile, Person_Email as Email, Person_Pan as Pan,Other,accoid as id " +
                            " FROM   " + tblPrefix + "AcContacts where accoid=" + hdnf.Value;
                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    string v = dt.Rows[0]["personid_No"].ToString();
                                    if (v != "")
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
                        }
                        else
                        {
                            grdDetail.DataSource = null;
                            grdDetail.DataBind();
                            ViewState["currentTable"] = null;
                        }
                        #endregion

                        pnlgrdDetail.Enabled = false;

                        #region Show Groups
                        DataTable dtTemp = new DataTable();
                        string strChecked = "";
                        qry = "select Group_Code from " + AcGroupsTable + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and accoid=" + hdnf.Value;
                        ds = new DataSet();
                        dt = new DataTable();
                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        strChecked = strChecked + dt.Rows[i]["Group_Code"].ToString() + ",";
                                    }
                                    strChecked = strChecked.Substring(0, strChecked.Length - 1);
                                }
                                //grdGroup.DataSource = dt;
                                //grdGroup.DataBind();
                            }
                        }

                        if (strChecked != string.Empty)
                        {
                            qry = "select System_Code,System_Name_E from " + SystemMastertable + " where System_Type='G' and Company_Code=" +
                                Convert.ToInt32(Session["Company_Code"].ToString()) + " ";

                            ds = ds = clsDAL.SimpleQuery(qry);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    dt = ds.Tables[0];
                                    if (dt.Rows.Count > 0)
                                    {
                                        dtTemp = dt;
                                        //if (ds.Tables[1].Rows.Count > 0)
                                        //{
                                        //    dtTemp.Merge(ds.Tables[1]);
                                        //}
                                    }
                                    //else if (ds.Tables[1].Rows.Count > 0)
                                    //{
                                    //    dtTemp = ds.Tables[1];
                                    //}

                                }
                            }
                            grdGroup.DataSource = dtTemp;
                            grdGroup.DataBind();
                            for (int i = 0; i < grdGroup.Rows.Count; i++)
                            {
                                CheckBox chk = (CheckBox)grdGroup.Rows[i].Cells[2].FindControl("chk");
                                if (strChecked.Contains(grdGroup.Rows[i].Cells[0].Text) == true)
                                {
                                    chk.Checked = true;
                                }
                                else
                                {
                                    chk.Checked = false;
                                }
                            }
                        }
                        else
                        {
                            qry = "select System_Code,System_Name_E from " + SystemMastertable + " where System_Type='G' and Company_Code=" +
                                Convert.ToInt32(Session["Company_Code"].ToString());
                            ds = clsDAL.SimpleQuery(qry);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    dt = ds.Tables[0];
                                    if (dt.Rows.Count > 0)
                                    {
                                        grdGroup.DataSource = dt;
                                        grdGroup.DataBind();
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            //this.enableDisableNavigateButtons();
            return recordExist;
        }
        catch
        {
            return false;
        }
    }
    #endregion


    #region getDisplayQuery
    protected string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select * from " + qryCommon + " where accoid=" + hdnf.Value + " ";
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
                txtAC_CODE.Text = hdnf.Value;
                hdnfSuffix.Value = "";
                string query = getDisplayQuery();
                clsButtonNavigation.enableDisable("N");
                bool recordExist = this.fetchRecord(query);
                if (recordExist == true)
                {
                    btnEdit.Enabled = true;
                    btnEdit.Focus();
                }

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
        //pnlMain.Enabled = false;
        setFocusControl(txtPERSON_NAME);
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

                if (dt.Rows[0]["personid_No"].ToString().Trim() != "")
                {

                    if (btnAdddetails.Text == "ADD")
                    {
                        dr = dt.NewRow();

                        #region calculate rowindex

                        int maxIndex = 0;

                        int[] index = new int[dt.Rows.Count];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            index[i] = Convert.ToInt32(dt.Rows[i]["personid_No"].ToString());
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
                        dr["personid_No"] = rowIndex;     //auto
                        dr["rowAction"] = "A";
                        dr["SrNo"] = 0;
                    }
                    else
                    {
                        //update row
                        int n = Convert.ToInt32(lblID.Text);
                        rowIndex = Convert.ToInt32(lblID.Text);   //auto no
                        dr = (DataRow)dt.Rows[n - 1];
                        dr["personid_No"] = rowIndex;
                        dr["SrNo"] = 0;

                        #region decide whether actual row is updating or virtual [rowAction]

                        string id = clsCommon.getString("select PersonId from " + tblDetails +
                        " where Ac_Code='" + txtAC_CODE.Text + "' and PersonId=" + lblID.Text +
                        " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        if (id != string.Empty)
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
                    dt.Columns.Add((new DataColumn("personid_No", typeof(int))));
                    dt.Columns.Add((new DataColumn("Person_Name", typeof(string))));
                    dt.Columns.Add((new DataColumn("Mobile", typeof(string))));
                    dt.Columns.Add((new DataColumn("Email", typeof(string))));
                    dt.Columns.Add((new DataColumn("Pan", typeof(double))));
                    dt.Columns.Add((new DataColumn("Other", typeof(double))));
                    dt.Columns.Add((new DataColumn("id", typeof(int))));
                    dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                    dt.Columns.Add((new DataColumn("SrNo", typeof(int))));

                    dr = dt.NewRow();
                    dr["personid_No"] = rowIndex;
                    dr["rowAction"] = "A";
                    dr["SrNo"] = 0;
                }
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add((new DataColumn("personid_No", typeof(int))));
                dt.Columns.Add((new DataColumn("Person_Name", typeof(string))));
                dt.Columns.Add((new DataColumn("Mobile", typeof(string))));
                dt.Columns.Add((new DataColumn("Email", typeof(string))));
                dt.Columns.Add((new DataColumn("Pan", typeof(string))));
                dt.Columns.Add((new DataColumn("Other", typeof(string))));
                dt.Columns.Add((new DataColumn("id", typeof(int))));
                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                dt.Columns.Add((new DataColumn("SrNo", typeof(int))));

                dr = dt.NewRow();
                dr["personid_No"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;

            }
            dr["Person_Name"] = txtPERSON_NAME.Text.ToUpper();
            if (txtPERSON_MOBILE.Text != string.Empty)
            {
                dr["Mobile"] = txtPERSON_MOBILE.Text;
            }
            else
            {
                setFocusControl(txtPERSON_MOBILE);
                return;
            }
            dr["Email"] = txtPERSON_EMAIL.Text;
            dr["Pan"] = txtPerson_PAN.Text;
            dr["Other"] = txtPERSON_OTHER.Text;
            dr["id"] = lblAc_Code.Text;
            if (btnAdddetails.Text == "ADD")
            {
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
                setFocusControl(txtPERSON_NAME);
            }
            else
            {
                pnlPopupDetails.Style["display"] = "none";
                setFocusControl(btnAdddetails);
                btnOpenDetailsPopup.Focus();
            }
            txtPERSON_NAME.Text = string.Empty;
            txtPERSON_MOBILE.Text = string.Empty;
            txtPERSON_EMAIL.Text = string.Empty;
            txtPerson_PAN.Text = string.Empty;
            txtPERSON_OTHER.Text = string.Empty;
            btnAdddetails.Text = "ADD";
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

    #region [grdDetail_RowDataBound]
    protected void grdDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        try
        {
            e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(4);
            e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(5);
            e.Row.Cells[personid_No].ControlStyle.Width = Unit.Percentage(7);
            e.Row.Cells[personName_E].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[mobile_No].ControlStyle.Width = Unit.Percentage(8);
            e.Row.Cells[email_Id].ControlStyle.Width = Unit.Percentage(15);
            e.Row.Cells[pan_No].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[other_Info].ControlStyle.Width = Unit.Percentage(15);
            e.Row.Cells[id].ControlStyle.Width = Unit.Percentage(15);
            e.Row.Cells[rowAction].ControlStyle.Width = Unit.Percentage(15);
            e.Row.Cells[SrNo].ControlStyle.Width = Unit.Percentage(15);

            e.Row.Cells[0].Style["overflow"] = "hidden";
            e.Row.Cells[1].Style["overflow"] = "hidden";
            e.Row.Cells[personid_No].Style["overflow"] = "hidden";
            e.Row.Cells[personName_E].Style["overflow"] = "hidden";
            e.Row.Cells[mobile_No].Style["overflow"] = "hidden";
            e.Row.Cells[email_Id].Style["overflow"] = "hidden";
            e.Row.Cells[pan_No].Style["overflow"] = "hidden";
            e.Row.Cells[other_Info].Style["overflow"] = "hidden";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[personid_No].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[personName_E].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[mobile_No].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[email_Id].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[pan_No].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[other_Info].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[id].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[rowAction].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[SrNo].HorizontalAlign = HorizontalAlign.Left;


            }
            //e.Row.Cells[id].Visible = false;
            //e.Row.Cells[rowAction].Visible = false;
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
            if (v == "txtGstStateCode" || v == "txtstatecode")
            {
                e.Row.Cells[0].Width = new Unit("100px");
                e.Row.Cells[1].Width = new Unit("400px");
            }
            else if (v == "txtSendingAcCode")
            {
                e.Row.Cells[0].Width = new Unit("100px");
                e.Row.Cells[1].Width = new Unit("400px");
            }
            else
            {
                e.Row.Cells[0].Width = new Unit("100px");
                e.Row.Cells[1].Width = new Unit("400px");
                e.Row.Cells[2].Width = new Unit("200px");
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


                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
                e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event);";
                e.Row.Attributes["onselectstart"] = "javascript:return false;";
                e.Row.Attributes["onkeyup"] = "javascript:return selectRow(event);";

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
                        if (grdDetail.Rows[rowindex].Cells[8].Text != "D" && grdDetail.Rows[rowindex].Cells[8].Text != "R")
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
                            //Making Changes by ankush
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

    #region [showDetailsRow]
    private void showDetailsRow(GridViewRow gridViewRow)
    {


        lblID.Text = Server.HtmlDecode(gridViewRow.Cells[personid_No].Text.Trim());
        lblNo.Text = Server.HtmlDecode(gridViewRow.Cells[rowAction].Text.Trim());
        txtPERSON_NAME.Text = Server.HtmlDecode(gridViewRow.Cells[personName_E].Text.Trim());
        txtPERSON_MOBILE.Text = Server.HtmlDecode(gridViewRow.Cells[mobile_No].Text.Trim());
        txtPERSON_EMAIL.Text = Server.HtmlDecode(gridViewRow.Cells[email_Id].Text.Trim());
        txtPerson_PAN.Text = Server.HtmlDecode(gridViewRow.Cells[pan_No].Text.Trim());
        txtPERSON_OTHER.Text = Server.HtmlDecode(gridViewRow.Cells[other_Info].Text.Trim());
        Acid = Convert.ToInt32(gridViewRow.Cells[id].Text);
        btnAdddetails.Text = "Update";
        setFocusControl(txtPERSON_NAME);
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
                int ID = Convert.ToInt32(dt.Rows[rowIndex]["personid_No"].ToString());

                string IDExisting = clsCommon.getString("select PersonId from " + tblDetails +
                    " where  Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and " +
                    " Ac_Code=" + hdnf.Value + " and PersonId=" + ID);

                if (IDExisting != string.Empty)
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[rowAction].Text = "D";

                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";            //D=Delete from table

                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[rowAction].Text = "N";

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
                        grdDetail.Rows[rowIndex].Cells[rowAction].Text = "R";       //R=Only remove fro grid        
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "R";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[rowAction].Text = "A";

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



    #region [txtAC_CODE_TextChanged]
    protected void txtAC_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAC_CODE.Text;
        strTextbox = "txtAC_CODE";
        csCalculations();
        setFocusControl(txtAC_NAME_E);
    }
    #endregion

    protected void txtDistance_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDistance.Text;
        strTextbox = "txtDistance";
        csCalculations();
        setFocusControl(txtDistance);
    }


    #region [btntxtAC_CODE_Click]
    protected void btntxtAC_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtEditDoc_No";
            btnSearch_Click(sender, e);
            setFocusControl(txtSearchText);
        }
        catch
        {
        }
    }
    #endregion


    #region [txtAC_RATE_TextChanged]
    protected void txtAC_RATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAC_RATE.Text;
        strTextbox = "txtAC_RATE";
        csCalculations();
    }
    #endregion

    #region [txtAC_NAME_E_TextChanged]
    protected void txtAC_NAME_E_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAC_NAME_E.Text;
        strTextbox = "txtAC_NAME_E";
        csCalculations();
    }
    #endregion

    #region [txtwhatsup_No_TextChanged]
    protected void txtwhatsup_No_TextChanged(object sender, EventArgs e)
    {
        searchString = txtwhatsup_No.Text;
        strTextbox = "txtwhatsup_No";
        csCalculations();
    }
    #endregion

    #region [txtAC_NAME_R_TextChanged]
    protected void txtAC_NAME_R_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAC_NAME_R.Text;
        strTextbox = "txtAC_NAME_R";
        csCalculations();
    }
    #endregion

    #region [txtCOMMISSION_TextChanged]
    protected void txtCOMMISSION_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCOMMISSION.Text;
        strTextbox = "txtCOMMISSION";
        csCalculations();
    }
    #endregion

    #region [txtSHORT_NAME_TextChanged]
    protected void txtSHORT_NAME_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSHORT_NAME.Text;
        strTextbox = "txtSHORT_NAME";
        csCalculations();
    }
    #endregion

    #region [txtADDRESS_E_TextChanged]
    protected void txtADDRESS_E_TextChanged(object sender, EventArgs e)
    {
        searchString = txtADDRESS_E.Text;
        strTextbox = "txtADDRESS_E";
        csCalculations();
    }
    #endregion

    #region [txtADDRESS_R_TextChanged]
    protected void txtADDRESS_R_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtADDRESS_R.Text;
        //strTextbox = "txtADDRESS_R";
        //csCalculations();
    }
    #endregion

    #region [txtCITY_CODE_TextChanged]
    protected void txtCITY_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCITY_CODE.Text;
        strTextbox = "txtCITY_CODE";
        csCalculations();
    }
    #endregion

    #region [btntxtCITY_CODE_Click]
    protected void btntxtCITY_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtCITY_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtPINCODE_TextChanged]
    protected void txtPINCODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPINCODE.Text;
        strTextbox = "txtPINCODE";
        csCalculations();
    }
    #endregion

    #region [txtOPENING_BALANCE_TextChanged]
    protected void txtOPENING_BALANCE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtOPENING_BALANCE.Text;
        strTextbox = "txtOPENING_BALANCE";
        csCalculations();
    }
    #endregion

    #region [drpDrCr_SelectedIndexChanged]
    protected void drpDrCr_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = drpDrCr.SelectedValue;
            strTextbox = "drpDrCr";
            csCalculations();
        }
        catch
        {

        }
    }
    #endregion

    #region [txtGROUP_CODE_TextChanged]
    protected void txtGROUP_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGROUP_CODE.Text;
        strTextbox = "txtGROUP_CODE";
        csCalculations();
    }
    #endregion

    #region [btntxtGROUP_CODE_Click]
    protected void btntxtGROUP_CODE_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGROUP_CODE";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtLOCAL_LIC_NO_TextChanged]
    protected void txtLOCAL_LIC_NO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtLOCAL_LIC_NO.Text;
        strTextbox = "txtLOCAL_LIC_NO";
        csCalculations();
    }
    #endregion

    #region [txtBANK_NAME_TextChanged]
    protected void txtBANK_NAME_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBANK_NAME.Text;
        strTextbox = "txtBANK_NAME";
        csCalculations();
    }
    #endregion

    #region [txtBANK_AC_NO_TextChanged]
    protected void txtBANK_AC_NO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBANK_AC_NO.Text;
        strTextbox = "txtBANK_AC_NO";
        csCalculations();
    }
    #endregion

    #region [txtCST_NO_TextChanged]

    #endregion

    #region [txtEMAIL_ID_TextChanged]
    protected void txtEMAIL_ID_TextChanged(object sender, EventArgs e)
    {
        searchString = txtEMAIL_ID.Text;
        strTextbox = "txtEMAIL_ID";
        csCalculations();
    }
    #endregion

    #region [txtGST_NO_TextChanged]
    protected void txtGST_NO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGST_NO.Text;
        strTextbox = "txtGST_NO";
        csCalculations();
    }
    #endregion

    #region [txtEMAIL_ID_CC_TextChanged]
    protected void txtEMAIL_ID_CC_TextChanged(object sender, EventArgs e)
    {
        searchString = txtEMAIL_ID_CC.Text;
        strTextbox = "txtEMAIL_ID_CC";
        csCalculations();
    }
    #endregion

    #region [txtAdhar_No_TextChanged]
    protected void txtAdhar_No_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAdhar_No.Text;
        strTextbox = "txtAdhar_No";
        csCalculations();
    }
    #endregion

    #region [txtOTHER_NARRATION_TextChanged]
    protected void txtOTHER_NARRATION_TextChanged(object sender, EventArgs e)
    {
        searchString = txtOTHER_NARRATION.Text;
        strTextbox = "txtOTHER_NARRATION";
        csCalculations();
    }
    #endregion

    #region [txtBANK_OPENING_TextChanged]
    protected void txtBANK_OPENING_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBANK_OPENING.Text;
        strTextbox = "txtBANK_OPENING";
        csCalculations();
    }
    #endregion

    #region [drpBankDrCr_SelectedIndexChanged]
    protected void drpBankDrCr_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = drpBankDrCr.SelectedValue;
            strTextbox = "drpBankDrCr";
            csCalculations();
        }
        catch
        {

        }
    }
    #endregion

    #region [txtPERSON_NAME_TextChanged]
    protected void txtPERSON_NAME_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPERSON_NAME.Text;
        strTextbox = "txtPERSON_NAME";
        csCalculations();
    }
    #endregion

    #region [txtPERSON_MOBILE_TextChanged]
    protected void txtPERSON_MOBILE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPERSON_MOBILE.Text;
        strTextbox = "txtPERSON_MOBILE";
        csCalculations();
    }
    #endregion

    #region [txtPERSON_EMAIL_TextChanged]
    protected void txtPERSON_EMAIL_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPERSON_EMAIL.Text;
        strTextbox = "txtPERSON_EMAIL";
        csCalculations();
    }
    #endregion

    #region [txtPerson_PAN_TextChanged]
    protected void txtPerson_PAN_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPerson_PAN.Text;
        strTextbox = "txtPerson_PAN";
        csCalculations();
    }
    #endregion

    #region [txtPERSON_OTHER_TextChanged]
    protected void txtPERSON_OTHER_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPERSON_OTHER.Text;
        strTextbox = "txtPERSON_OTHER";
        csCalculations();
    }
    #endregion

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            string searchtxt = searchString;
            string delimStr = "";
            char[] delimiter = delimStr.ToCharArray();
            string words = searchString;
            string[] split = null;
            string name = string.Empty;
            if (searchString != string.Empty && strTextbox == hdnfClosePopup.Value)
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
            if (hdnfClosePopup.Value == "txtDoc_No" || hdnfClosePopup.Value == "txtEditDoc_No")
            {

                if (btntxtAC_CODE.Text == "Change No")
                {
                    pnlPopup.Style["display"] = "none";
                    txtAC_CODE.Text = string.Empty;
                    txtAC_CODE.Enabled = true;
                    btnSave.Enabled = false;
                    setFocusControl(txtAC_CODE);
                    hdnfClosePopup.Value = "Close";
                }
                if (btntxtAC_CODE.Text == "Choose No")
                {
                    searchString = txtEditDoc_No.Text;
                    strTextbox = "txtEditDoc_No";
                    pnlPopup.Style["display"] = "block";
                    if (searchString != string.Empty && strTextbox == hdnfClosePopup.Value)
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

                    foreach (var s in split)
                    {
                        string aa = s.ToString();
                        //name += " Doc_No Like '%" + aa + "%'or Supplier_name Like '%" + aa
                        //    + "%'or Part_No_Name Like '%" + aa + "%'or TDS_name Like '%" + aa + "%'or";
                        name += "( Doc_No like '%" + aa + "%' or Supplier_name like '%" + aa + "%' or Part_No_Name like '%" + aa + "%' or TDS_name like '%" + aa + "%' ) and";

                    }
                    name = name.Remove(name.Length - 3);
                    lblPopupHead.Text = "--Select Group--";
                    string qry = " select distinct Doc_No,Doc_Date,Supplier_name,Bill_Amount,Bill_No,Branch_Code from " + qryCommon
                        + " where Tran_type='PB' and  Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                        + " and (" + name + ") order by Doc_No desc, Doc_Date desc";
                    this.showPopup(qry);
                }
            }

            if (hdnfClosePopup.Value == "txtSendingAcCode")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    //name += " Branch_Code Like '%" + aa + "%'or Branch_Name_E Like '%" + aa + "%'or Branch_Address Like '%" + aa + "%'or";
                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or Address_E like '%" + aa + "%' ) and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Account--";
                string qry = "select Ac_Code,Ac_Name_E from " + tblHead + " where (" + name + ") order by Ac_Name_E asc";
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtCITY_CODE")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    //name += " Branch_Code Like '%" + aa + "%'or Branch_Name_E Like '%" + aa + "%'or Branch_Address Like '%" + aa + "%'or";
                    name += "( city_code like '%" + aa + "%' or city_name_e like '%" + aa + "%' or state like '%" + aa + "%' ) and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select City--";
                string qry = "select city_code,city_name_e,city_name_r,state,cityid from " + cityMasterTable + " where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and (" + name + ") order by city_name_e";
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtGstStateCode")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    //name += " Branch_Code Like '%" + aa + "%'or Branch_Name_E Like '%" + aa + "%'or Branch_Address Like '%" + aa + "%'or";
                    name += "( State_Code like '%" + aa + "%' or State_Name like '%" + aa + "%'  ) and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select GST State Code--";
                string qry = "Select State_Code,State_Name from GSTStateMaster where (" + name + ")";
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtstatecode")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    //name += " Branch_Code Like '%" + aa + "%'or Branch_Name_E Like '%" + aa + "%'or Branch_Address Like '%" + aa + "%'or";
                    name += "( State_Code like '%" + aa + "%' or State_Name like '%" + aa + "%'  ) and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select GST State Code--";
                string qry = "Select State_Code,State_Name from GSTStateMaster where (" + name + ")";
                // string qry = "Select State_Code,State_Name from GSTStateMaster where State_Code like'%" + txtSearchText.Text + "%' or State_Name like'%" + txtSearchText.Text + "%'";
                this.showPopup(qry);
            }


            if (hdnfClosePopup.Value == "txtGROUP_CODE")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    //name += " Branch_Code Like '%" + aa + "%'or Branch_Name_E Like '%" + aa + "%'or Branch_Address Like '%" + aa + "%'or";
                    name += "( group_Code like '%" + aa + "%' or group_Name_E like '%" + aa + "%' or group_Name_R like '%" + aa + "%' ) and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select BS group--";
                string qry = "select group_Code,group_Name_E,group_Name_R,bsid from " + GroupMasterTable + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                " and (" + name + ") order by group_Name_E";
                this.showPopup(qry);
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
            using (clsDataProvider objDataProvider = new clsDataProvider())
            {
                setFocusControl(txtSearchText);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = objDataProvider.GetDataSet(qry);
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
            if (hdnfClosePopup.Value == "txtCITY_CODE")
            {
                setFocusControl(txtCITY_CODE);
            }
            if (hdnfClosePopup.Value == "txtGROUP_CODE")
            {
                setFocusControl(txtGROUP_CODE);
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

                searchString = txtSearchText.Text;
                strTextbox = hdnfClosePopup.Value;

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
        #region Validation
        bool isValidated = true;
        if (txtAC_CODE.Text != string.Empty)
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtAC_CODE);
            return;
        }
        if (txtAC_NAME_E.Text != string.Empty)
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtAC_NAME_E);
            return;
        }
        if (txtGROUP_CODE.Text != string.Empty)
        {
            string str = clsCommon.getString("select group_Code from " + GroupMasterTable + " where group_Code=" + txtGROUP_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (str != "0")
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtGROUP_CODE);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtGROUP_CODE);
            return;
        }
        //if (drpType.SelectedValue == "E")
        //{
        //    txtGstStateCode.Text = "27";
        //}
        if (drpType.SelectedValue != "F")
        {
            if (txtGstStateCode.Text != string.Empty)
            {
                string str = clsCommon.getString("select State_Code from  GSTStateMaster where State_Code=" + txtGstStateCode.Text);
                if (str != "0")
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtGstStateCode);
                    return;
                }
            }
            else
            {
                isValidated = false;
                setFocusControl(txtGstStateCode);
                return;
            }
        }
        else
        {
            txtGstStateCode.Text = "27";
        }

        if (drpType.SelectedValue != "E")
        {
            if (txtCITY_CODE.Text != string.Empty)
            {
                string str = clsCommon.getString("select city_code from " + cityMasterTable + " where city_code="
                    + txtCITY_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != "0")
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtCITY_CODE);
                    return;
                }
            }
            else
            {
                isValidated = false;
                setFocusControl(txtCITY_CODE);
                return;
            }

        }



        if (drpType.SelectedValue != "E")
        {

            if (txtDistance.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtDistance);
                return;
            }
        }

        if (drpType.SelectedValue != "E")
        {
            if (txtPINCODE.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtPINCODE);
                return;
            }
        }

        if (txtSHORT_NAME.Text != string.Empty)
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtSHORT_NAME);
            return;
        }
        if (drpType.SelectedValue == "P" || drpType.SelectedValue == "M")
        {
            if (txtGST_NO.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                if (chkUnregisterGST.Checked)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtGST_NO);
                    return;

                }
            }

            if (txtGST_NO.Text.Length == 15)
            {
                isValidated = true;
            }
            else
            {
                if (chkUnregisterGST.Checked)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtGST_NO);
                    return;

                }
                //isValidated = false;
                //setFocusControl(txtGST_NO);
                //return;

            }
        }
        #endregion

        #region -Head part Assign Value
        AC_CODE = txtAC_CODE.Text != string.Empty ? Convert.ToInt32(txtAC_CODE.Text) : 0;
        AC_TYPE = drpType.SelectedValue;
        Limit_By = drpLimit.SelectedValue;
        AC_RATE = txtAC_RATE.Text != string.Empty ? Convert.ToDouble(txtAC_RATE.Text) : 0.00;
        AC_NAME_E = txtAC_NAME_E.Text.ToUpper();
        AC_NAME_R = txtAC_NAME_R.Text;
        COMMISSION = txtCOMMISSION.Text != string.Empty ? Convert.ToDouble(txtCOMMISSION.Text) : 0.00;
        SHORT_NAME = txtSHORT_NAME.Text.ToUpper();
        ADDRESS_E = txtADDRESS_E.Text.ToUpper();
        ADDRESS_R = txtADDRESS_R.Text.ToUpper();
        CITY_CODE = txtCITY_CODE.Text != string.Empty ? Convert.ToInt32(txtCITY_CODE.Text) : 0;
        if (CITY_CODE != 0)
        {
            try
            {
                cityid = Convert.ToInt32(clsCommon.getString("select ifnull(cityid,0) as id from nt_1_citymaster where city_code=" + CITY_CODE + " and company_code='" + Session["Company_Code"].ToString() + "'"));
            }
            catch
            {

            }
        }
        PINCODE = txtPINCODE.Text != string.Empty ? Convert.ToInt32(txtPINCODE.Text) : 0;
        OPENING_BALANCE = txtOPENING_BALANCE.Text != string.Empty ? Convert.ToDouble(txtOPENING_BALANCE.Text) : 0.00;
        DRCR = drpDrCr.SelectedValue;
        DISTANCE = txtDistance.Text != string.Empty ? Convert.ToDouble(txtDistance.Text) : 0.00;

        GSTStateCode = txtGstStateCode.Text.Trim() != string.Empty ? Convert.ToInt32(txtGstStateCode.Text) : 0;



        GROUP_CODE = txtGROUP_CODE.Text != string.Empty ? Convert.ToInt32(txtGROUP_CODE.Text) : 0;
        if (GROUP_CODE != 0)
        {
            try
            {
                bsGroup_Id = Convert.ToInt32(clsCommon.getString("select ifnull(bsid,0) as id from nt_1_bsgroupmaster where group_Code='" + GROUP_CODE + "' and company_code='" + Session["Company_Code"].ToString() + "'"));
            }
            catch
            {

            }
        }
        LOCAL_LIC_NO = txtLOCAL_LIC_NO.Text;
        BANK_NAME = txtBANK_NAME.Text.ToUpper();

        BANK_AC_NO = txtBANK_AC_NO.Text;

        EMAIL_ID = txtEMAIL_ID.Text;
        GST_NO = txtGST_NO.Text.ToUpper();
        EMAIL_ID_CC = txtEMAIL_ID_CC.Text;
        ADHARNO = txtAdhar_No.Text;
        OTHER_NARRATIOM = txtOTHER_NARRATION.Text.ToUpper();

        MOBILE = txtMOBILE.Text;
        IFSC = txtIfsc.Text.ToString().ToUpper();
        FSSAI = txtFssaiNo.Text.ToString().ToUpper();
        BANK_OPENING = txtBANK_OPENING.Text != string.Empty ? Convert.ToDouble(txtBANK_OPENING.Text) : 0.00;
        BANK_OP_DRCR = drpBankDrCr.SelectedValue;
        WHATSUPNO = txtwhatsup_No.Text;
        carporate_party = string.Empty;

        if (chkCarporate.Checked == true)
        {
            carporate_party = "Y";
        }
        else
        {
            carporate_party = "N";
        }

        if (chkUnregisterGST.Checked)
        {
            UnregisterGST = 1;
        }

        if (chkLocked.Checked)
        {
            locked = 1;
        }

        referBy = txtRefBy.Text;
        OffPhone = txtOffPhone.Text;
        Fax = txtfax.Text;
        CompanyPan = txtcompanyPan.Text.ToUpper();

        AC_Pan = f_pan;
        retValue = string.Empty;
        strRev = string.Empty;
        Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
        Year_Code = Convert.ToInt32(Session["year"].ToString());
        year_Code = Convert.ToInt32(Session["year"].ToString());
        Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
        Branch1DrCr = "";
        Branch2DrCr = "";



        Created_By = clsGV.user;
        Created_By = Session["user"].ToString();
        Modified_By = clsGV.user;
        Modified_By = Session["user"].ToString();

        #endregion-End of Head part Assign Value

        #region save Account Master


        DataSet ds = new DataSet();
        try
        {
            FormTypes types = new FormTypes();
            #region Detail Fields
            Detail_Fields.Append("PersonId,");
            Detail_Fields.Append("Company_Code,");
            Detail_Fields.Append("Ac_Code,");
            Detail_Fields.Append("Person_Name,");
            Detail_Fields.Append("Person_Mobile,");
            Detail_Fields.Append("Person_Email,");
            Detail_Fields.Append("Person_Pan,");
            Detail_Fields.Append("Other,");
            Detail_Fields.Append("accoid");
            #endregion
            #region
            Group_Fields.Append("GroupID,");
            Group_Fields.Append("Company_Code,");
            Group_Fields.Append("Ac_Code,");
            Group_Fields.Append("Group_Code,");
            Group_Fields.Append("accoid");

            #endregion
            if (btnSave.Text == "Save")
            {
                this.NextNumber();

                #region ---- Accountmaster ----
                #region Assign Tables Field And Values
                Head_Fields.Append("Ac_Code,");
                Head_Values.Append("'" + Doc_No + "',");
                Head_Fields.Append("Ac_Name_E,");
                Head_Values.Append("'" + AC_NAME_E + "',");
                Head_Fields.Append("Ac_Name_R,");
                Head_Values.Append("'" + AC_NAME_R + "',");
                Head_Fields.Append("Ac_type,");
                Head_Values.Append("'" + AC_TYPE + "',");
                Head_Fields.Append("Ac_rate,");
                Head_Values.Append("'" + AC_RATE + "',");
                Head_Fields.Append("Address_E,");
                Head_Values.Append("'" + ADDRESS_E + "',");
                Head_Fields.Append("Address_R,");
                Head_Values.Append("'" + ADDRESS_R + "',");
                Head_Fields.Append("City_Code,");
                Head_Values.Append("'" + CITY_CODE + "',");
                Head_Fields.Append("Pincode,");
                Head_Values.Append("'" + PINCODE + "',");
                Head_Fields.Append("Local_Lic_No,");
                Head_Values.Append("'" + LOCAL_LIC_NO + "',");
                //Head_Fields .Append( "Tin_No,");
                //Head_Values.Append( "'" + TIN_NO + "',");
                //Head_Fields .Append( "Cst_no,");
                //Head_Values.Append( "'" + CST_NO + "',");
                Head_Fields.Append("Gst_No,");
                Head_Values.Append("'" + GST_NO + "',");
                Head_Fields.Append("Email_Id,");
                Head_Values.Append("'" + EMAIL_ID + "',");
                Head_Fields.Append("EMAIL_ID_CC,");
                Head_Values.Append("'" + EMAIL_ID_CC + "',");
                Head_Fields.Append("adhar_no,");
                Head_Values.Append("'" + ADHARNO + "',");
                Head_Fields.Append("Other_Narration,");
                Head_Values.Append("'" + OTHER_NARRATIOM + "',");

                //Head_Fields .Append( "ECC_No,");
                //Head_Values.Append( "'" + ECC_NO + "',");
                Head_Fields.Append("Bank_Name,");
                Head_Values.Append("'" + BANK_NAME + "',");
                Head_Fields.Append("Bank_Ac_No,");
                Head_Values.Append("'" + BANK_AC_NO + "',");
                Head_Fields.Append("Bank_Opening,");
                Head_Values.Append("'" + BANK_OPENING + "',");
                Head_Fields.Append("bank_Op_Drcr,");
                Head_Values.Append("'" + BANK_OP_DRCR + "',");
                Head_Fields.Append("Opening_Balance,");
                Head_Values.Append("'" + OPENING_BALANCE + "',");
                Head_Fields.Append("Drcr,");
                Head_Values.Append("'" + DRCR + "',");
                Head_Fields.Append("Group_Code,");
                Head_Values.Append("'" + GROUP_CODE + "',");
                //Head_Fields .Append( "Company_Code,");
                //Head_Values.Append( "'" + Company_Code + "',");
                Head_Fields.Append("Created_By,");
                Head_Values.Append("'" + Created_By + "',");
                Head_Fields.Append("Short_Name,");
                Head_Values.Append("'" + SHORT_NAME + "',");



                Head_Fields.Append("Commission,");
                Head_Values.Append("'" + COMMISSION + "',");
                Head_Fields.Append("carporate_party,");
                Head_Values.Append("'" + carporate_party + "',");
                Head_Fields.Append("referBy,");
                Head_Values.Append("'" + referBy + "',");
                Head_Fields.Append("OffPhone,");
                Head_Values.Append("'" + OffPhone + "',");
                Head_Fields.Append("Fax,");
                Head_Values.Append("'" + Fax + "',");
                Head_Fields.Append("CompanyPan,");
                Head_Values.Append("'" + CompanyPan + "',");
                Head_Fields.Append("AC_Pan,");
                Head_Values.Append("'" + AC_Pan + "',");
                Head_Fields.Append("Mobile_No,");
                Head_Values.Append("'" + MOBILE + "',");
                Head_Fields.Append("IFSC,");
                Head_Values.Append("'" + IFSC + "',");
                Head_Fields.Append("FSSAI,");
                Head_Values.Append("'" + FSSAI + "',");
                Head_Fields.Append("Branch1OB,");
                Head_Values.Append("'" + Branch1OB + "',");
                Head_Fields.Append("company_code,");
                Head_Values.Append("'" + Company_Code + "',");

                Head_Fields.Append("GSTStateCode,");
                Head_Values.Append("'" + GSTStateCode + "',");
                Head_Fields.Append("UnregisterGST,");
                Head_Values.Append("'" + UnregisterGST + "',");
                Head_Fields.Append("Distance,");
                Head_Values.Append("'" + DISTANCE + "',");
                Head_Fields.Append("Locked,");
                Head_Values.Append("'" + locked + "',");
                Head_Fields.Append("accoid,");
                Head_Values.Append("'" + Acid + "',");
                Head_Fields.Append("bsid,");
                Head_Values.Append("case when 0='" + bsGroup_Id + "' then null else '" + bsGroup_Id + "' end,");
                Head_Fields.Append("whatsup_no,");
                Head_Values.Append("'" + WHATSUPNO + "',");
                Head_Fields.Append("Limit_By,");
                Head_Values.Append("'" + Limit_By + "',");
                Head_Fields.Append("cityid");
                Head_Values.Append("case when 0='" + cityid + "' then null else '" + cityid + "' end");


                Head_Insert = "insert into " + tblHead + "(" + Head_Fields + ") values(" + Head_Values + ")";

                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Head_Insert;
                Maindt.Rows.Add(dr);
                #endregion

                #region -------------------- Account Details --------------------



                if (grdDetail.Rows.Count > 0)
                {
                    foreach (GridViewRow row in grdDetail.Rows)
                    {
                        #region
                        personId = Convert.ToInt32(row.Cells[2].Text);
                        if (row.Cells[personName_E].Text.Trim() != "&nbsp;" || row.Cells[personName_E].Text.Trim() != "")
                        {
                            personName = row.Cells[personName_E].Text.Trim();
                        }
                        else
                        {
                            personName = row.Cells[personName_E].Text = "";
                        }
                        if (row.Cells[mobile_No].Text.Trim() != "&nbsp;" || row.Cells[mobile_No].Text.Trim() != "")
                        {
                            mobile = row.Cells[mobile_No].Text.Trim();
                        }
                        else
                        {
                            mobile = row.Cells[mobile_No].Text = "";
                        }
                        if (row.Cells[email_Id].Text.Trim() != "&nbsp;" || row.Cells[email_Id].Text.Trim() != "")
                        {
                            email = row.Cells[email_Id].Text.Trim();
                        }
                        else
                        {
                            email = row.Cells[email_Id].Text = "";
                        }
                        //mobile = row.Cells[4].Text;
                        //email = row.Cells[5].Text;
                        if (row.Cells[pan_No].Text.Trim() != "&nbsp;" || row.Cells[pan_No].Text.Trim() != "")
                        {
                            pan = row.Cells[pan_No].Text.Trim();
                        }
                        else
                        {
                            pan = row.Cells[pan_No].Text = "";
                        }
                        //pan = row.Cells[6].Text;
                        if (row.Cells[other_Info].Text.Trim() != " &nbsp;" || row.Cells[other_Info].Text.Trim() != "")
                        {
                            other = row.Cells[other_Info].Text.Trim();
                        }
                        else
                        {
                            other = row.Cells[other_Info].Text = "";
                        }
                        //other = row.Cells[7].Text;
                        i_d = row.Cells[2].Text;
                        #endregion
                        if (row.Cells[rowAction].Text != "N" && row.Cells[rowAction].Text != "R")
                        {

                            // INSERT GRID RECORDS IN TABLE
                            Detail_Values.Append("('" + personId + "','" + Company_Code + "','" + Doc_No + "','" + Server.HtmlDecode(personName) + "'," +
                                " '" + Server.HtmlDecode(mobile) + "','" + Server.HtmlDecode(email) + "','" + Server.HtmlDecode(pan) + "'," +
                                " '" + Server.HtmlDecode(other) + "','" + Acid + "'," + Company_Code + "),");

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


                }
                else
                {
                    grdDetail.DataSource = null;
                    grdDetail.DataBind();
                }

                #endregion

                #region ----------------  Account Groups ----------


                if (grdGroup.Rows.Count > 0)
                {
                    ds = new DataSet();
                    qry = "delete from " + AcGroupsTable + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + AC_CODE;
                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = qry;
                    Maindt.Rows.Add(dr);

                    string max = clsCommon.getString("select ifnull(max(GroupID),0) as GroupID from " + AcGroupsTable + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    int no = Convert.ToInt32(max);
                    for (int i = 0; i < grdGroup.Rows.Count; i++)
                    {
                        no = no + 1;
                        CheckBox chk = (CheckBox)grdGroup.Rows[i].Cells[2].FindControl("chk");
                        if (chk.Checked)
                        {
                            Group_Code = grdGroup.Rows[i].Cells[0].Text;
                            Group_Values.Append("('" + no + "','" + Company_Code + "','" + AC_CODE + "','" + Group_Code + "','" + Acid + "'),");
                        }
                    }
                    if (Group_Values.Length > 0)
                    {
                        Group_Values.Remove(Group_Values.Length - 1, 1);

                        Group_Insert = "insert into " + AcGroupsTable + "(" + Group_Fields + ") values " + Group_Values + " ";
                        dr = null;
                        dr = Maindt.NewRow();
                        dr["Querys"] = Group_Insert;
                        Maindt.Rows.Add(dr);

                    }
                }
                #endregion
                flag = 1;

                //Thread thred = new Thread(() => { count = DataStore(flag); }); //Calling DataStore Method Using Thread
                //thred.Start(); //Thread Operation Start

                //if (count == 1)
                //{
                //    hdnf.Value = Acid.ToString();
                //    clsButtonNavigation.enableDisable("S");
                //    this.makeEmptyForm("S");
                //    qry = getDisplayQuery();
                //    this.fetchRecord(qry);
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Added !')", true);

                //}

            }
            else
            {
                #region Update Head
                Acid = Convert.ToInt32(lblAc_Code.Text);
                //Update Mode

                Head_Update = "Ac_Name_E='" + AC_NAME_E + "',Ac_Name_R='" + AC_NAME_R + "',Ac_type='" + AC_TYPE + "',Ac_rate='" + AC_RATE + "',Address_E='"
                         + ADDRESS_E + "',Address_R='" + ADDRESS_R + "'," +
                         " City_Code='" + CITY_CODE + "',Pincode='" + PINCODE + "',Local_Lic_No='" + LOCAL_LIC_NO + "', GST_NO='"
                         + GST_NO + "',Email_Id='" + EMAIL_ID + "',EMAIL_ID_CC='" + EMAIL_ID_CC + "',adhar_no='" + ADHARNO + "',Other_Narration='" + OTHER_NARRATIOM + "', " +
                          " Bank_Name='" + BANK_NAME + "',Bank_Ac_No='" + BANK_AC_NO + "',Bank_Opening='" + BANK_OPENING + "',bank_Op_Drcr='"
                         + BANK_OP_DRCR + "',Opening_Balance='" + OPENING_BALANCE + "',Drcr='" + DRCR + "',Group_Code='" + GROUP_CODE + "',Modified_By='"
                         + Session["user"].ToString() + "',Short_Name='" + SHORT_NAME + "',Commission='" + COMMISSION +
                         "',carporate_party='" + carporate_party + "',referBy='" + referBy + "',OffPhone='" + OffPhone + "',Fax='" + Fax + "',IFSC='"
                         + IFSC + "',FSSAI='" + FSSAI + "',CompanyPan='" + CompanyPan + "',AC_Pan='" + AC_Pan + "',Mobile_No='" + MOBILE + "',GSTStateCode='"
                         + GSTStateCode + "',UnregisterGST='" + UnregisterGST + "',Distance='" + DISTANCE + "',Locked='" + locked + "' ,whatsup_no='" + WHATSUPNO + "'," +
                         " cityid=case when 0='" + cityid + "' then null else '" + cityid + "' end,bsid=case when 0='" + bsGroup_Id + "' then null else '" + bsGroup_Id + "' end " +
                         " ,Company_Code=" + Company_Code + ",Limit_By='" + Limit_By + "' where accoid=" + lblAc_Code.Text + " ";

                Head_Update = "update " + tblHead + " set " + Head_Update + " ";

                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Head_Update;
                Maindt.Rows.Add(dr);


                #region -------------------- Account Details --------------------


                if (grdDetail.Rows.Count > 0)
                {
                    Detail_Delete = "delete from " + tblDetails + " where accoid='" + lblAc_Code.Text + "'";
                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = Detail_Delete;
                    Maindt.Rows.Add(dr);
                    foreach (GridViewRow row in grdDetail.Rows)
                    {
                        #region
                        personId = Convert.ToInt32(row.Cells[2].Text);
                        if (row.Cells[personName_E].Text.Trim() != "&nbsp;" || row.Cells[personName_E].Text.Trim() != "")
                        {
                            personName = row.Cells[personName_E].Text.Trim();
                        }
                        else
                        {
                            personName = row.Cells[personName_E].Text = "";
                        }
                        if (row.Cells[mobile_No].Text.Trim() != "&nbsp;" || row.Cells[mobile_No].Text.Trim() != "")
                        {
                            mobile = row.Cells[mobile_No].Text.Trim();
                        }
                        else
                        {
                            mobile = row.Cells[mobile_No].Text = "";
                        }
                        if (row.Cells[email_Id].Text.Trim() != "&nbsp;" || row.Cells[email_Id].Text.Trim() != "")
                        {
                            email = row.Cells[email_Id].Text.Trim();
                        }
                        else
                        {
                            email = row.Cells[email_Id].Text = "";
                        }
                        //mobile = row.Cells[4].Text;
                        //email = row.Cells[5].Text;
                        if (row.Cells[pan_No].Text.Trim() != "&nbsp;" || row.Cells[pan_No].Text.Trim() != "")
                        {
                            pan = row.Cells[pan_No].Text.Trim();
                        }
                        else
                        {
                            pan = row.Cells[pan_No].Text = "";
                        }
                        //pan = row.Cells[6].Text;
                        if (row.Cells[other_Info].Text.Trim() != " &nbsp;" || row.Cells[other_Info].Text.Trim() != "")
                        {
                            other = row.Cells[other_Info].Text.Trim();
                        }
                        else
                        {
                            other = row.Cells[other_Info].Text = "";
                        }
                        Acid = Convert.ToInt32(row.Cells[id].Text);
                        i_d = row.Cells[2].Text;
                        #endregion
                        if (row.Cells[rowAction].Text != "R" && row.Cells[rowAction].Text != "D")
                        {


                            // INSERT GRID RECORDS IN TABLE
                            Detail_Values.Append("('" + personId + "','" + Company_Code + "','" + txtAC_CODE.Text + "','" + Server.HtmlDecode(personName) + "'," +
                                " '" + Server.HtmlDecode(mobile) + "','" + Server.HtmlDecode(email) + "','" + Server.HtmlDecode(pan) + "'," +
                                " '" + Server.HtmlDecode(other) + "','" + Acid + "'),");

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


                }
                else
                {
                    grdDetail.DataSource = null;
                    grdDetail.DataBind();
                }

                #endregion

                #region ----------------  Account Groups ----------


                if (grdGroup.Rows.Count > 0)
                {
                    ds = new DataSet();
                    qry = "delete from " + AcGroupsTable + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + AC_CODE;
                    string dele = clsCommon.getString(qry);

                    string max = clsCommon.getString("select ifnull(max(GroupID),0) as GroupID from " + AcGroupsTable + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    int no = Convert.ToInt32(max);
                    for (int i = 0; i < grdGroup.Rows.Count; i++)
                    {
                        no = no + 1;
                        CheckBox chk = (CheckBox)grdGroup.Rows[i].Cells[2].FindControl("chk");
                        if (chk.Checked)
                        {
                            Group_Code = grdGroup.Rows[i].Cells[0].Text;
                            Group_Values.Append("('" + no + "','" + Company_Code + "','" + AC_CODE + "','" + Group_Code + "','" + Acid + "'),");
                        }
                    }
                    if (Group_Values.Length > 0)
                    {
                        Group_Values.Remove(Group_Values.Length - 1, 1);

                        Group_Insert = "insert into " + AcGroupsTable + "(" + Group_Fields + ") values " + Group_Values + " ";

                        dr = null;
                        dr = Maindt.NewRow();
                        dr["Querys"] = Group_Insert;
                        Maindt.Rows.Add(dr);
                    }
                }
                #endregion


                //if (count == 2)
                //{

                //    hdnf.Value = lblAc_Code.Text;
                //    clsButtonNavigation.enableDisable("S");
                //    this.makeEmptyForm("S");
                //    qry = getDisplayQuery();
                //    this.fetchRecord(qry);
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update!')", true);

                //}

                #endregion
                flag = 2;
                // msg = clsDAL.DataStore(Head_Insert, Head_Update, Head_Delete, Detail_Insert, Detail_Update, Detail_Delete, GLEDGER_Insert, GLEDGER_Delete, flag);
            }
            #region GLedger Effect
            GLEDGER_Delete = "delete from " + GLedgerTable + " where TRAN_TYPE='OP' and Doc_No=" + AC_CODE + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";

            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = GLEDGER_Delete;
            Maindt.Rows.Add(dr);

            if (OPENING_BALANCE != 0)
            {
                StringBuilder Gledger_Column = new StringBuilder();
                StringBuilder Gledger_values = new StringBuilder();
                Gledger_Column.Append("TRAN_TYPE,");
                Gledger_values.Append("'OP',");
                Gledger_Column.Append("CASHCREDIT,");
                Gledger_values.Append(" '',");
                Gledger_Column.Append("DOC_NO,");
                Gledger_values.Append(" '" + AC_CODE + "',");
                Gledger_Column.Append("DOC_DATE,");
                Gledger_values.Append(" '2020/03/31',");
                Gledger_Column.Append("AC_CODE,");
                Gledger_values.Append(" '" + AC_CODE + "',");
                Gledger_Column.Append("UNIT_code,");
                Gledger_values.Append(" '0',");
                Gledger_Column.Append("NARRATION,");
                Gledger_values.Append("'Opening Balance ',");
                Gledger_Column.Append("AMOUNT,");
                Gledger_values.Append(" " + OPENING_BALANCE + ",");
                Gledger_Column.Append("TENDER_ID,");
                Gledger_values.Append(" null,");
                Gledger_Column.Append("TENDER_ID_DETAIL,");
                Gledger_values.Append(" null,");
                Gledger_Column.Append("VOUCHER_ID,");
                Gledger_values.Append("null,");
                Gledger_Column.Append("COMPANY_CODE,");
                Gledger_values.Append(" '" + Convert.ToInt32(Session["Company_Code"].ToString()) + "',");
                Gledger_Column.Append("YEAR_CODE,");
                Gledger_values.Append(" '" + Convert.ToInt32(Session["Company_Code"].ToString()) + "',");
                Gledger_Column.Append("ORDER_CODE,");
                Gledger_values.Append("0,");
                Gledger_Column.Append("DRCR,");
                Gledger_values.Append(" '" + DRCR + "',");
                Gledger_Column.Append("DRCR_HEAD,");
                Gledger_values.Append(" '" + AC_CODE + "',");
                Gledger_Column.Append("ADJUSTED_AMOUNT,");
                Gledger_values.Append(" 0,");
                Gledger_Column.Append("Branch_Code,");
                Gledger_values.Append(" 1,");
                Gledger_Column.Append("SORT_TYPE,");
                Gledger_values.Append("'',");
                Gledger_Column.Append("SORT_NO,");
                Gledger_values.Append(" 0,");
                Gledger_Column.Append("ac,");
                Gledger_values.Append(" '" + Acid + "',");
                Gledger_Column.Append("vc,");
                Gledger_values.Append(" 0,");
                Gledger_Column.Append("progid,");
                Gledger_values.Append(" '" + types.TT_AC + "',");
                Gledger_Column.Append("tranid");
                Gledger_values.Append(" '" + Acid + "'");

                //string Gledger_values = " 'OP','','" + AC_CODE + "','2015/03/31','" + AC_CODE + "','0','" + "Opening Balance " + GlegerNarration + "' ," +
                //    " " + OPENING_BALANCE + ",null,null,null,'" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','1',0,'" + DRCR + "',0,0,'1','','0','" + Acid + "','0', " +
                //    " '" + types.TT_AC + "','" + Acid + "'");
                GLEDGER_Insert = "insert into nt_1_gledger (" + Gledger_Column + ") values (" + Gledger_values + ")";

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
                //ds = clsDAL.SimpleQuery(Group_Insert);
                hdnf.Value = Acid.ToString();
                clsButtonNavigation.enableDisable("S");
                this.makeEmptyForm("S");
                qry = getDisplayQuery();
                this.fetchRecord(qry);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Added !')", true);
            }
            else if (msg == "Update")
            {
                hdnf.Value = lblAc_Code.Text;
                clsButtonNavigation.enableDisable("S");
                this.makeEmptyForm("S");
                qry = getDisplayQuery();
                this.fetchRecord(qry);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update!')", true);
            }
        }

                #endregion


        catch { }




        #endregion

        ClearSendingSmsTextboxes();
    }
    #endregion

    #region [saveDocuments]
    private void saveDocuments()
    {
        //try
        //{
        //    if (FileUpload_PAN.HasFile)
        //    {
        //        try
        //        {
        //            string filename = Path.GetFileName(FileUpload_PAN.FileName);
        //            FileUpload_PAN.SaveAs(Server.MapPath("~/PAN/PAN_" + clsGV.user + "") + filename);
        //            //
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //            // StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
        //        }
        //    }
        //    //using (FileStream fs = new FileStream(Server.MapPath(f), FileMode.Create))
        //    //{
        //    //    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
        //    //    {
        //    //        w.WriteLine(s);
        //    //    }
        //    //}
        //}
        //catch
        //{
        //}
    }
    #endregion

    #region DataInsert
    private int DataStore(int flag)
    {
        int count = 0;
        try
        {
            //Connection open
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            ///Execution
            myTran = con.BeginTransaction();
            //cmd.CommandText = qry;
            //cmd.Connection = con;
            //cmd.Transaction = myTran;
            if (flag == 1)
            {
                cmd = new MySqlCommand(Head_Insert, con, myTran);
                cmd.ExecuteNonQuery();

                if (Detail_Insert != "")
                {
                    cmd = new MySqlCommand(Detail_Insert, con, myTran);
                    cmd.ExecuteNonQuery();
                }
                if (Group_Insert != "")
                {
                    cmd = new MySqlCommand(Group_Insert, con, myTran);
                    cmd.ExecuteNonQuery();
                }

                myTran.Commit();
                Thread.Sleep(100);

                count = 1;
            }

            if (flag == 2)
            {

                cmd = new MySqlCommand(Head_Update, con, myTran);
                cmd.ExecuteNonQuery();

                if (Detail_Update != "")
                {
                    cmd = new MySqlCommand(Detail_Update, con, myTran);
                    cmd.ExecuteNonQuery();
                }
                if (Group_Update != "")
                {
                    cmd = new MySqlCommand(Group_Update, con, myTran);
                    cmd.ExecuteNonQuery();
                }

                myTran.Commit();
                Thread.Sleep(100);

                count = 2;
            }
            if (flag == 3)
            {
                cmd = new MySqlCommand(Detail_Delete, con, myTran);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(Group_Delete, con, myTran);
                cmd.ExecuteNonQuery();
                cmd = new MySqlCommand(Head_Delete, con, myTran);
                cmd.ExecuteNonQuery();


                myTran.Commit();
                Thread.Sleep(100);

                count = 3;
            }

            return count;
        }
        catch
        {
            if (myTran != null)
            {
                myTran.Rollback();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('check Entry AND Try Again !')", true);

            }
            return count;

        }
        finally
        {
            con.Close();
        }

    }
    #endregion

    #region [drpType_SelectedIndexChanged]
    protected void drpType_SelectedIndexChanged(object sender, EventArgs e)
    {
        #region always check
        setFocusControl(drpType);
        string val = drpType.SelectedValue.ToString();
        //drpType.Attributes.Add("onChange", "javascript:EnableDisable();");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "javascript:EnableDisable('" + val + "');", true);
        if (btnSave.Enabled == true)
        {

            string s_item = drpType.SelectedValue;
            if (s_item == "T")
            {
                txtCOMMISSION.Enabled = false;
                txtADDRESS_E.Enabled = false;
                // txtADDRESS_R.Enabled = false;
                txtCITY_CODE.Enabled = true;
                txtPINCODE.Enabled = false;
                txtOPENING_BALANCE.Enabled = false;
                drpDrCr.Enabled = false;
                txtLOCAL_LIC_NO.Enabled = false;
                //txtTIN_NO.Enabled = false;
                //txtCST_NO.Enabled = false;
                txtGST_NO.Enabled = false;
                txtBANK_OPENING.Enabled = true;
                drpBankDrCr.Enabled = false;
            }
            else if (drpType.SelectedValue == "E" || drpType.SelectedValue == "O")
            {
                txtAC_NAME_E.Enabled = true;
                txtOPENING_BALANCE.Enabled = false;
                drpDrCr.Enabled = false;
                txtGROUP_CODE.Enabled = true;
                txtCOMMISSION.Enabled = false;
                txtADDRESS_E.Enabled = false;
                // txtADDRESS_R.Enabled = false;
                txtCITY_CODE.Enabled = false;
                txtPINCODE.Enabled = false;
                txtLOCAL_LIC_NO.Enabled = false;
                //txtTIN_NO.Enabled = false;
                //txtCST_NO.Enabled = false;
                txtGST_NO.Enabled = false;
                txtBANK_AC_NO.Enabled = false;
                txtBANK_NAME.Enabled = false;
                txtEMAIL_ID.Enabled = false;
                txtEMAIL_ID_CC.Enabled = false;
                //txtECC_NO.Enabled = false;
                txtRefBy.Enabled = false;
                txtOffPhone.Enabled = false;
                txtcompanyPan.Enabled = false;
                txtfax.Enabled = false;
                btnOpenDetailsPopup.Enabled = false;
                grdGroup.Enabled = false;
                txtAC_RATE.Enabled = false;
                txtOTHER_NARRATION.Enabled = false;
                chkCarporate.Enabled = false;
            }
            else if (s_item != "B")
            {
                txtCOMMISSION.Enabled = true;
                txtADDRESS_E.Enabled = true;
                //txtADDRESS_R.Enabled = true;
                txtCITY_CODE.Enabled = true;
                txtPINCODE.Enabled = true;
                txtOPENING_BALANCE.Enabled = true;
                drpDrCr.Enabled = true;
                txtLOCAL_LIC_NO.Enabled = true;
                //txtTIN_NO.Enabled = true;
                //txtCST_NO.Enabled = true;
                txtGST_NO.Enabled = true;
                txtBANK_OPENING.Enabled = true;
                drpBankDrCr.Enabled = true;
                txtAC_NAME_R.Enabled = false;
                chkCarporate.Enabled = true;
                //txtECC_NO.Enabled = true;
            }

            if (s_item == "B")
            {
                txtAC_NAME_R.Enabled = false;
                txtCOMMISSION.Enabled = false;
                chkCarporate.Enabled = false;
                txtLOCAL_LIC_NO.Enabled = false;
                //txtTIN_NO.Enabled = false;
                //txtGST_NO.Enabled = false;
                //txtECC_NO.Enabled = false;
                //txtCST_NO.Enabled = false;
                txtBANK_OPENING.Enabled = true;
                drpBankDrCr.Enabled = true;
            }
            else
            {
                txtBANK_OPENING.Enabled = false;
                drpBankDrCr.Enabled = false;
            }

            //Enabling Required controls to true
            if (s_item == "F")
            {
                FixedAssetsControls();
            }
            if (s_item == "F" || s_item == "I")
            {

                setFocusControl(txtAC_RATE);
                txtAC_RATE.Enabled = true;
            }
            else
            {
                txtAC_RATE.Enabled = false;
            }
        }
        #endregion
    }

    private void FixedAssetsControls()
    {
        txtAC_NAME_E.Enabled = true;
        txtOPENING_BALANCE.Enabled = true;
        drpDrCr.SelectedValue = "D";
        txtGROUP_CODE.Enabled = true;
        drpDrCr.Enabled = false;
        txtCOMMISSION.Enabled = false;
        txtADDRESS_E.Enabled = false;
        // txtADDRESS_R.Enabled = false;
        txtCITY_CODE.Enabled = false;
        txtPINCODE.Enabled = false;
        txtLOCAL_LIC_NO.Enabled = false;

        txtGST_NO.Enabled = false;
        txtBANK_AC_NO.Enabled = false;
        txtBANK_NAME.Enabled = false;
        txtEMAIL_ID.Enabled = false;
        txtEMAIL_ID_CC.Enabled = false;
        txtAdhar_No.Enabled = false;

        txtRefBy.Enabled = false;
        txtOffPhone.Enabled = false;
        txtcompanyPan.Enabled = false;
        txtfax.Enabled = false;
        btnOpenDetailsPopup.Enabled = false;
        grdGroup.Enabled = false;
        txtAC_RATE.Enabled = false;
        txtOTHER_NARRATION.Enabled = false;
        chkCarporate.Enabled = false;
    }
    #endregion

    #region csCalculations
    private void csCalculations()
    {
        try
        {
            #region  txtAC_CODE
            if (strTextbox == "txtAC_CODE")
            {
                #region code
                try
                {
                    int n;
                    bool isNumeric = int.TryParse(txtAC_CODE.Text, out n);

                    if (isNumeric == true)
                    {
                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        string txtValue = "";
                        if (txtAC_CODE.Text != string.Empty)
                        {
                            txtValue = txtAC_CODE.Text;

                            string qry = "select * from " + tblHead + " where  Ac_Code='" + txtValue + "' " +
                                "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());

                            ds = clsDAL.SimpleQuery(qry);
                            if (ds != null)
                            {
                                if (ds.Tables.Count > 0)
                                {
                                    dt = ds.Tables[0];
                                    if (dt.Rows.Count > 0)
                                    {
                                        //Record Found
                                        hdnf.Value = dt.Rows[0]["Ac_Code"].ToString();
                                        if (ViewState["mode"] != null)
                                        {
                                            if (ViewState["mode"].ToString() == "I")
                                            {
                                                lblMsg.Text = "** Ac_Code (" + txtValue + ") Already Exist";
                                                lblMsg.ForeColor = System.Drawing.Color.Red;
                                                btnSave.Enabled = true;   //IMP
                                                this.getMaxCode();
                                                setFocusControl(drpType);
                                            }

                                            if (ViewState["mode"].ToString() == "U")
                                            {
                                                //fetch record
                                                qry = this.getDisplayQuery();
                                                bool recordExist = this.fetchRecord(qry);
                                                if (recordExist == true)
                                                {
                                                    txtAC_CODE.Enabled = false;
                                                    setFocusControl(drpType);
                                                    hdnf.Value = txtAC_CODE.Text;
                                                }
                                            }
                                        }
                                    }
                                    else   //Record Not Found
                                    {
                                        if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                        {
                                            lblMsg.Text = "";
                                            setFocusControl(txtAC_CODE);
                                            txtAC_CODE.Enabled = false;
                                            btnSave.Enabled = true;   //IMP
                                        }
                                        if (ViewState["mode"].ToString() == "U")
                                        {
                                            this.makeEmptyForm("E");
                                            lblMsg.Text = "** Record Not Found";
                                            lblMsg.ForeColor = System.Drawing.Color.Red;
                                            txtAC_CODE.Text = string.Empty;
                                            setFocusControl(txtAC_CODE);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblMsg.Text = string.Empty;
                            setFocusControl(txtAC_CODE);
                        }
                    }
                    else
                    {
                        this.makeEmptyForm("A");
                        lblMsg.Text = "Ac code is numeric";
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        clsButtonNavigation.enableDisable("E");
                        txtAC_CODE.Text = string.Empty;
                        setFocusControl(txtAC_CODE);
                    }
                }
                catch
                {

                }
                #endregion
            }
            #endregion



            try
            {
                if (drpType.SelectedValue != "F" || drpType.SelectedValue != "E" || drpType.SelectedValue != "O")
                {
                    if (strTextbox == "txtAC_NAME_E")
                    {
                        setFocusControl(txtAC_NAME_R);
                    }
                    if (strTextbox == "txtAC_NAME_R")
                    {
                        if (drpType.SelectedValue == "F" || drpType.SelectedValue == "O" || drpType.SelectedValue == "E")
                        {
                            setFocusControl(txtSHORT_NAME);
                        }
                        else
                        {
                            setFocusControl(txtCOMMISSION);
                        }
                    }
                    if (strTextbox == "txtAC_RATE")
                    {
                        setFocusControl(txtAC_NAME_E);
                    }
                    if (strTextbox == "txtCOMMISSION")
                    {
                        setFocusControl(txtSHORT_NAME);
                    }
                    if (strTextbox == "txtSendingAcCode")
                    {
                        bool a = clsCommon.isStringIsNumeric(txtSendingAcCode.Text);
                        if (a == false)
                        {
                            btntxtSendingAcCode_Click(this, new EventArgs());
                        }
                        else
                        {
                            string SendingAcCode = clsCommon.getString("select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + txtSendingAcCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            if (SendingAcCode != string.Empty)
                            {
                                txtSendingEmail.Text = clsCommon.getString("select Email_Id from " + tblPrefix + "AccountMaster where Ac_Code=" + txtSendingAcCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                                txtSendingMobile.Text = clsCommon.getString("select Mobile_No from " + tblPrefix + "AccountMaster where Ac_Code=" + txtSendingAcCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                                if (SendingAcCode.Length >= 10)
                                {
                                    lblSendingAcCode.ToolTip = SendingAcCode;
                                    SendingAcCode = SendingAcCode.Substring(0, 10);
                                }
                                lblSendingAcCode.Text = SendingAcCode;
                                setFocusControl(btnSendSMS);
                            }
                            else
                            {
                                txtSendingAcCode.Text = string.Empty;
                                lblSendingAcCode.Text = SendingAcCode;
                                setFocusControl(txtSendingAcCode);
                            }
                        }
                    }
                    //else
                    //{
                    //    lblSendingAcCode.Text = "";
                    //    setFocusControl(txtSendingAcCode);
                    //}
                    if (strTextbox == "txtSHORT_NAME")
                    {
                        if (drpType.SelectedValue == "E" || drpType.SelectedValue == "O")
                        {
                            setFocusControl(txtGROUP_CODE);
                        }
                        else if (drpType.SelectedValue == "F")
                        {
                            setFocusControl(txtOPENING_BALANCE);
                        }
                        else
                        {
                            setFocusControl(txtADDRESS_E);
                        }
                    }
                    if (strTextbox == "txtADDRESS_E")
                    {
                        //setFocusControl(txtADDRESS_R);
                    }
                    if (strTextbox == "txtADDRESS_R")
                    {
                        setFocusControl(txtCITY_CODE);
                    }
                    if (strTextbox == "txtCITY_CODE")
                    {
                        if (txtCITY_CODE.Text == "0")
                        {
                            txtCITY_CODE.Text = string.Empty;
                        }
                        if (txtCITY_CODE.Text != string.Empty)
                        {
                            bool a = clsCommon.isStringIsNumeric(txtCITY_CODE.Text);
                            if (a == false)
                            {
                                btntxtCITY_CODE_Click(this, new EventArgs());
                            }
                            else
                            {
                                string str = clsCommon.getString("select city_name_e from " + cityMasterTable + " where city_code="
                                    + txtCITY_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                                string pincode = clsCommon.getString("select ifnull(pincode,'') from " + cityMasterTable + " where city_code="
                                    + txtCITY_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                                string distance = clsCommon.getString("select ifnull(Distance,'') from " + cityMasterTable + " where city_code="
                                  + txtCITY_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                                string subarea = clsCommon.getString("select ifnull(Sub_Area,'') from " + cityMasterTable + " where city_code="
                                 + txtCITY_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));


                                string gststatecode = clsCommon.getString("select ifnull(GstStateCode,0) from " + cityMasterTable + " where city_code="
                                + txtCITY_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                                cityid = Convert.ToInt32(clsCommon.getString("select ifnull(cityid,0) from " + cityMasterTable + " where city_code="
                                + txtCITY_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString())));
                                lblcityid.Text = cityid.ToString();

                                string statename = clsCommon.getString("select gststatename from qrycitymaster where city_code="
                               + txtCITY_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                                string SubAreaCity = clsCommon.getString("select Sub_Area from qrycitymaster where city_code="
                               + txtCITY_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                                txtADDRESS_R.Text = txtADDRESS_R.Text + " " + SubAreaCity;
                                if (str != string.Empty && str != "0")
                                {
                                    if (str.Length > 14)
                                    {
                                        str = str.Substring(1, 14);
                                    }
                                    else if (str.Length > 12)
                                    {
                                        str = str.Substring(1, 12);
                                    }
                                    else if (str.Length > 8)
                                    {
                                        str = str.Substring(1, 8);
                                    }

                                    lblCITYNAME.Text = str;
                                    txtPINCODE.Text = pincode;
                                    txtDistance.Text = distance;
                                    // txtADDRESS_R.Text = subarea;
                                    txtGstStateCode.Text = gststatecode;
                                    lbltxtGstStateName.Text = statename;

                                    setFocusControl(txtPINCODE);
                                }
                                else
                                {
                                    lblCITYNAME.Text = str;
                                    txtCITY_CODE.Text = string.Empty;
                                    setFocusControl(txtCITY_CODE);
                                }
                            }
                        }
                        else
                        {
                            setFocusControl(txtPINCODE);
                        }
                    }
                    if (strTextbox == "txtGstStateCode")
                    {
                        if (txtGstStateCode.Text != string.Empty)
                        {
                            bool a = clsCommon.isStringIsNumeric(txtGstStateCode.Text);
                            if (a == false)
                            {
                                btntxtGstStateCode_Click(this, new EventArgs());
                            }
                            else
                            {
                                string str = clsCommon.getString("select State_Name from GSTStateMaster where State_Code=" + txtGstStateCode.Text + "");
                                if (str != string.Empty && str != "0")
                                {
                                    lbltxtGstStateName.Text = str;
                                    setFocusControl(txtDistance);
                                }
                                else
                                {
                                    lbltxtGstStateName.Text = str;
                                    txtGstStateCode.Text = string.Empty;
                                    setFocusControl(txtGstStateCode);
                                }
                            }
                        }
                        else
                        {
                            setFocusControl(txtGstStateCode);
                        }
                    }
                    if (strTextbox == "txtstatecode")
                    {
                        if (txtstatecode.Text != string.Empty)
                        {

                            bool a = clsCommon.isStringIsNumeric(txtstatecode.Text);
                            if (a == false)
                            {
                                btntxtGstStateCode_Click(this, new EventArgs());
                            }
                            else
                            {
                                string str = clsCommon.getString("select State_Name from GSTStateMaster where State_Code=" + txtstatecode.Text + "");
                                if (str != string.Empty && str != "0")
                                {
                                    lblGstStateName.Text = str;
                                    setFocusControl(btnSaveCity);

                                }
                                else
                                {
                                    lblGstStateName.Text = str;
                                    txtstatecode.Text = string.Empty;
                                    setFocusControl(txtstatecode);
                                }
                            }
                        }
                        else
                        {
                            setFocusControl(txtstatecode);
                        }
                    }
                    if (strTextbox == "txtPINCODE")
                    {
                        setFocusControl(txtGstStateCode);
                    }
                    if (strTextbox == "txtopening_balance")
                    {
                        if (drpType.SelectedValue == "f")
                        {
                            setFocusControl(txtGROUP_CODE);

                        }
                        else
                        {

                            setFocusControl(drpDrCr);
                        }
                    }
                    if (strTextbox == "drpDrCr")
                    {
                        setFocusControl(txtGROUP_CODE);
                    }

                    if (strTextbox == "txtLOCAL_LIC_NO")
                    {

                    }
                    if (strTextbox == "txtTIN_NO")
                    {

                    }
                    if (strTextbox == "txtCST_NO")
                    {
                        setFocusControl(txtGST_NO);
                    }
                    if (strTextbox == "txtGST_NO")
                    {
                        setFocusControl(txtOTHER_NARRATION);
                    }
                    if (strTextbox == "txtOTHER_NARRATION")
                    {
                        setFocusControl(txtRefBy);
                    }
                    if (strTextbox == "txtBANK_NAME")
                    {
                        setFocusControl(txtIfsc);
                    }
                    if (strTextbox == "txtIfsc")
                    {
                        setFocusControl(txtBANK_AC_NO);
                    }

                    if (strTextbox == "txtBANK_AC_NO")
                    {
                        setFocusControl(txtEMAIL_ID);
                    }
                    if (strTextbox == "txtEMAIL_ID")
                    {
                        setFocusControl(txtEMAIL_ID_CC);
                    }
                    if (strTextbox == "txtEMAIL_ID_CC")
                    {

                    }
                    if (strTextbox == "txtECC_NO")
                    {
                        setFocusControl(txtFssaiNo);
                    }
                    if (strTextbox == "txtFssaiNo")
                    {
                        setFocusControl(txtBANK_NAME);
                    }
                    if (strTextbox == "txtRefBy")
                    {
                        setFocusControl(txtOffPhone);
                    }
                    if (strTextbox == "txtOffPhone")
                    {
                        setFocusControl(txtcompanyPan);
                    }
                    if (strTextbox == "txtcompanyPan")
                    {
                        setFocusControl(txtfax);
                    }
                    if (strTextbox == "txtPERSON_NAME")
                    {
                        setFocusControl(txtPERSON_MOBILE);
                    }
                    if (strTextbox == "txtPERSON_MOBILE")
                    {
                        setFocusControl(txtPERSON_EMAIL);
                    }
                    if (strTextbox == "txtPERSON_EMAIL")
                    {
                        setFocusControl(txtPerson_PAN);
                    }
                    if (strTextbox == "txtPerson_PAN")
                    {
                        setFocusControl(txtPERSON_OTHER);
                    }
                    if (strTextbox == "txtPERSON_OTHER")
                    {
                        setFocusControl(btnAdddetails);
                    }
                    if (strTextbox == "txtRefBy")
                    {
                        setFocusControl(txtOffPhone);
                    }
                    if (strTextbox == "txtOffPhone")
                    {
                        setFocusControl(txtcompanyPan);
                    }
                    if (strTextbox == "txtcompanyPan")
                    {
                        setFocusControl(txtfax);
                    }
                    if (strTextbox == "txtfax")
                    {
                        setFocusControl(txtMOBILE);
                    }
                    if (strTextbox == "txtMOBILE")
                    {
                        setFocusControl(btnOpenDetailsPopup);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (strTextbox == "txtGROUP_CODE")
            {
                if (txtGROUP_CODE.Text != string.Empty)
                {
                    string s = string.Empty;
                    bool a = clsCommon.isStringIsNumeric(txtGROUP_CODE.Text);
                    if (a == false)
                    {
                        btntxtGROUP_CODE_Click(this, new EventArgs());
                    }
                    else
                    {
                        qry = "select group_Name_E,bsid from " + GroupMasterTable + " where group_Code=" + txtGROUP_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                        DataSet ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                DataTable dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    s = dt.Rows[0]["group_Name_E"].ToString();
                                    bsGroup_Id = Convert.ToInt32(dt.Rows[0]["bsid"].ToString());
                                    lblgroupid.Text = bsGroup_Id.ToString();
                                }
                            }
                        }

                        if (s != string.Empty)
                        {
                            if (s.Length > 14)
                            {
                                s = s.Substring(0, 14);
                            }
                            else if (s.Length > 13)
                            {
                                s = s.Substring(0, 13);
                            }
                            else if (s.Length > 12)
                            {
                                s = s.Substring(0, 12);
                            }
                            else if (s.Length > 11)
                            {
                                s = s.Substring(0, 11);
                            }
                            else if (s.Length > 10)
                            {
                                s = s.Substring(0, 10);
                            }
                            else if (s.Length > 9)
                            {
                                s = s.Substring(0, 9);
                            }
                            else if (s.Length > 5)
                            {
                                s = s.Substring(0, 5);
                            }
                            lblGROUPNAME.Text = s;

                            setFocusControl(txtBANK_OPENING);
                        }
                        else
                        {
                            lblGROUPNAME.Text = string.Empty;
                            txtGROUP_CODE.Text = string.Empty;
                            setFocusControl(txtGROUP_CODE);
                        }
                    }
                }
                else
                {
                    lblGROUPNAME.Text = string.Empty;
                    txtGROUP_CODE.Text = string.Empty;
                    setFocusControl(txtGROUP_CODE);
                }
            }
            #region always check
            //if (btnSave.Enabled == true)
            //{
            //    string s_item = drpType.SelectedValue;
            //    if (s_item == "T")
            //    {
            //        //    txtCOMMISSION.Text = "";
            //        txtCOMMISSION.Enabled = true;
            //        //    txtADDRESS_E.Text = "";
            //        txtADDRESS_E.Enabled = true;
            //        //     txtADDRESS_R.Text = "";
            //        // txtADDRESS_R.Enabled = true;
            //        //     txtCITY_CODE.Text = "";
            //        //      lblCITYNAME.Text = "";
            //        txtCITY_CODE.Enabled = true;
            //        //     txtPINCODE.Text = "";
            //        txtPINCODE.Enabled = true;
            //        //     txtOPENING_BALANCE.Text = "";
            //        if (Session["year"].ToString() != "1")
            //        {
            //            txtOPENING_BALANCE.Enabled = false;
            //        }
            //        else
            //        {
            //            txtOPENING_BALANCE.Enabled = true;
            //        }
            //        drpDrCr.Enabled = true;
            //        //    txtLOCAL_LIC_NO.Text = "";
            //        txtLOCAL_LIC_NO.Enabled = true;
            //        //   txtTIN_NO.Text = "";

            //        //    txtGST_NO.Text = "";
            //        txtGST_NO.Enabled = true;
            //        //    txtBANK_OPENING.Text = "";
            //        txtBANK_OPENING.Enabled = true;
            //        drpBankDrCr.Enabled = true;
            //    }
            //    else if (drpType.SelectedValue == "F" || drpType.SelectedValue == "O" || drpType.SelectedValue == "E")
            //    {
            //        //  txtCOMMISSION.Text = "";
            //        txtCOMMISSION.Enabled = false;
            //        //    txtADDRESS_E.Text = "";
            //        txtADDRESS_E.Enabled = false;
            //        //    txtADDRESS_R.Text = "";
            //        // txtADDRESS_R.Enabled = false;
            //        //    txtCITY_CODE.Text = "";
            //        //    lblCITYNAME.Text = "";
            //        txtCITY_CODE.Enabled = false;
            //        //     txtPINCODE.Text = "";
            //        txtPINCODE.Enabled = false;
            //        //     txtOPENING_BALANCE.Text = "";
            //        //txtOPENING_BALANCE.Enabled = false;
            //        drpDrCr.Enabled = false;
            //        //   txtLOCAL_LIC_NO.Text = "";
            //        txtLOCAL_LIC_NO.Enabled = false;
            //        //   txtTIN_NO.Text = "";

            //        //    txtGST_NO.Text = "";
            //        txtGST_NO.Enabled = false;
            //        //     txtBANK_OPENING.Text = "";
            //        txtBANK_OPENING.Enabled = false;
            //        drpBankDrCr.Enabled = true;
            //    }
            //    else
            //    {
            //        //  txtCOMMISSION.Text = "";
            //        txtCOMMISSION.Enabled = true;
            //        //    txtADDRESS_E.Text = "";
            //        txtADDRESS_E.Enabled = true;
            //        //    txtADDRESS_R.Text = "";
            //        //txtADDRESS_R.Enabled = true;
            //        //    txtCITY_CODE.Text = "";
            //        //    lblCITYNAME.Text = "";
            //        txtCITY_CODE.Enabled = true;
            //        //     txtPINCODE.Text = "";
            //        txtPINCODE.Enabled = true;
            //        //     txtOPENING_BALANCE.Text = "";
            //        if (Session["year"].ToString() != "1")
            //        {
            //            txtOPENING_BALANCE.Enabled = false;
            //        }
            //        else
            //        {
            //            txtOPENING_BALANCE.Enabled = true;
            //        }
            //        drpDrCr.Enabled = true;
            //        //   txtLOCAL_LIC_NO.Text = "";
            //        txtLOCAL_LIC_NO.Enabled = true;
            //        //   txtTIN_NO.Text = "";

            //        //    txtGST_NO.Text = "";
            //        txtGST_NO.Enabled = true;
            //        //     txtBANK_OPENING.Text = "";
            //        txtBANK_OPENING.Enabled = true;
            //        drpBankDrCr.Enabled = true;
            //    }
            //    if (s_item == "B")
            //    {
            //        //   txtBANK_OPENING.Text = "";
            //        txtBANK_OPENING.Enabled = true;
            //        drpBankDrCr.Enabled = true;
            //    }
            //    else
            //    {
            //        //   txtBANK_OPENING.Text = "";
            //        txtBANK_OPENING.Enabled = false;
            //        drpBankDrCr.Enabled = false;
            //    }

            //    if (s_item == "F" || s_item == "I")
            //    {
            //        //  txtAC_RATE.Text = "";
            //        txtAC_RATE.Enabled = true;
            //    }
            //    else
            //    {
            //        // txtAC_RATE.Text = "";
            //        txtAC_RATE.Enabled = false;
            //    }
            //}


            #endregion
        }
        catch
        {
        }
    }

    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
        pnlPopup.Style["display"] = "block";
    }
    #endregion

    protected void FileUpload_PAN_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
    {
        //string filePath = "~/PAN/" + clsGV.user + "_" + e.FileName;
        //FileUpload_PAN.SaveAs(Server.MapPath(filePath));
        //f_pan = filePath;
    }
    protected void grdDetail_RowCreated(object sender, GridViewRowEventArgs e)
    {
    }
    protected void txtcompanyPan_TextChanged(object sender, EventArgs e)
    {
        searchString = txtcompanyPan.Text;
        strTextbox = "txtcompanyPan";
        csCalculations();
    }
    protected void txtMOBILE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMOBILE.Text;
        strTextbox = "txtMOBILE";
        csCalculations();

    }
    protected void txtfax_TextChanged(object sender, EventArgs e)
    {
        searchString = txtfax.Text;
        strTextbox = "txtfax";
        csCalculations();
    }
    protected void txtOffPhone_TextChanged(object sender, EventArgs e)
    {
        searchString = txtOffPhone.Text;
        strTextbox = "txtOffPhone";
        csCalculations();

    }
    protected void txtRefBy_TextChanged(object sender, EventArgs e)
    {
        searchString = txtRefBy.Text;
        strTextbox = "txtRefBy";
        csCalculations();
    }
    protected void txtFssaiNo_TextChanged(object sender, EventArgs e)
    {
        searchString = txtFssaiNo.Text;
        strTextbox = "txtFssaiNo";
        csCalculations();
    }
    protected void txtIfsc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtIfsc.Text;
        strTextbox = "txtIfsc";
        csCalculations();
    }
    protected void btnAddCity_Click(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception)
        {

            throw;
        }

    }
    protected void imgClose_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnSaveCity_Click(object sender, EventArgs e)
    {
        try
        {
            string citycode = txtCityCode.Text;
            string cityname = txtCityName.Text;
            string citynamer = txtRegionalName.Text;
            string state = txtState.Text;
            string pincode1 = txtpincodecity.Text;
            string subarea = txtSubArea.Text;
            string statecode = txtstatecode.Text;
            string distance = txtdist.Text;
            bool isValidated = true;
            if (!string.IsNullOrEmpty(txtCityName.Text))
            {
                string str = clsCommon.getString("select city_code from " + tblHead + " where  city_code='" + txtCityCode.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty)
                {
                    lblErr.Text = "Doc No " + txtCityCode.Text + " already exist";
                    string maxcity = clsCommon.getString("Select ISNULL(MAX(city_code+1),1) from  " + tblPrefix + "CityMaster where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    txtCityCode.Text = maxcity;
                    modalCity.Show();
                    isValidated = true;
                    return;
                }
                else
                {
                    isValidated = true;
                }
                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    string strRev = "";
                    obj.flag = 1;
                    obj.tableName = tblPrefix + "CityMaster";
                    obj.columnNm = "city_code,city_name_e,city_name_r,company_code,state,Created_By,pincode,Sub_Area,GstStateCode,Distance";
                    obj.values = "'" + citycode + "','" + cityname + "','" + citynamer + "','" + Convert.ToInt32(Session["Company_Code"].ToString())
                        + "','" + state + "','" + user + "','" + pincode1 + "','" + subarea + "','" + statecode + "','" + distance + "'";
                    DataSet ds = new DataSet();
                    ds = obj.insertAccountMaster(ref strRev);
                    txtCITY_CODE.Text = citycode;
                    lblCITYNAME.Text = cityname;
                    //txtpincodecity.Text = pincode1;
                    // txtGstStateCode.Text = statecode;
                }
            }
            else
            {
                lblErr.Text = "City Name Is Reuired!";
                setFocusControl(txtCityName);
                modalCity.Show();
            }
            txtpincodecity.Text = "";
            txtSubArea.Text = "";
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void drpgroupSection_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void drpGroupSummary_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnAddGroup_Click1(object sender, EventArgs e)
    {
        try
        {
            string GroupCode = clsCommon.getString("Select ISNULL(MAX(group_Code+1),1) from  " + tblPrefix + "BSGroupMaster where company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            txtGroupCode.Text = GroupCode;
            ViewState["group"] = "S";
            txtGroupName.Text = string.Empty;
            setFocusControl(txtGroupName);
            ModalGroupMaster.Show();
        }
        catch (Exception)
        {

            throw;
        }
    }
    protected void btnSaveGroup_Click(object sender, EventArgs e)
    {
        try
        {
            bool isValidated = true;
            if (txtGroupCode.Text != string.Empty)
            {
                if (ViewState["group"].ToString() == "S")
                {
                    string str = clsCommon.getString("select group_Name_E from " + tblPrefix + "BSGroupMaster where group_Code=" + txtGroupCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty)
                    {
                        lblGropCodeexist.Text = "Code " + txtGroupCode.Text + " already exist";
                        isValidated = false;
                        return;
                    }
                    else
                    {
                        isValidated = true;
                    }
                }


            }
            string GroupCod = txtGroupCode.Text;
            string GroupName = txtGroupName.Text;
            string group_type = drpgroupSection.SelectedValue;
            string group_Summary = drpGroupSummary.SelectedValue;
            Int32 group_order = txtGroupOrder.Text != string.Empty ? Convert.ToInt32(txtGroupOrder.Text) : 0;
            string user = Session["user"].ToString();

            if (isValidated == true)
            {
                if (!string.IsNullOrEmpty(txtGroupName.Text))
                {
                    using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                    {
                        string strRev = "";
                        DataSet ds = new DataSet();
                        obj.flag = 1;
                        obj.tableName = tblPrefix + "BSGroupMaster";
                        obj.columnNm = "group_Code,group_Name_E,group_Type,group_Summary,group_Order,Company_Code,Created_By";
                        obj.values = "'" + GroupCod + "','" + GroupName + "','" + group_type + "','" + group_Summary + "','" + group_order + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + user + "'";
                        ds = obj.insertAccountMaster(ref strRev);
                        txtGROUP_CODE.Text = GroupCod;
                        lblGROUPNAME.Text = GroupName;
                    }
                }
                else
                {
                    lblGrr.Text = "Required!";
                    ModalGroupMaster.Show();
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    protected void btnSendSMS_Click(object sender, CommandEventArgs e)
    {
        try
        {
            string PartyName = txtAC_NAME_E.Text;
            string PartyAddress = txtADDRESS_E.Text;
            string CityName = clsCommon.getString("Select city_name_e from " + tblPrefix + "CityMaster where city_code=" + txtCITY_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
            CityName = CityName == string.Empty ? Convert.ToString(CityName) : " City: " + CityName;
            string State = clsCommon.getString("Select state from " + tblPrefix + "CityMaster where city_code=" + txtCITY_CODE.Text + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
            State = State == string.Empty ? Convert.ToString(State) : " State: " + State;
            string PINCODE = txtPINCODE.Text == string.Empty ? Convert.ToString(txtPINCODE.Text) : " PINCODE: " + txtPINCODE.Text.Trim();
            string LOCAL_LIC_NO = txtLOCAL_LIC_NO.Text == string.Empty ? Convert.ToString(txtLOCAL_LIC_NO.Text) : "LIC: " + txtLOCAL_LIC_NO.Text + Environment.NewLine + ",";

            string GST_NO = txtGST_NO.Text.Trim() == string.Empty ? Convert.ToString(txtGST_NO.Text) : "GST: " + txtGST_NO.Text + Environment.NewLine + ",";

            string FSSAI = txtFssaiNo.Text.Trim() == string.Empty ? Convert.ToString(txtFssaiNo.Text) : "FSSAI: " + txtFssaiNo.Text + Environment.NewLine + ",";
            string PAN = txtcompanyPan.Text.Trim() == string.Empty ? Convert.ToString(txtcompanyPan.Text) : "PAN: " + txtcompanyPan.Text + Environment.NewLine + ",";
            string MOBILE = txtSendingMobile.Text;
            string EMAIL_ID = txtSendingEmail.Text;
            string ADHARNO = txtAdhar_No.Text;

            string BankName = txtBANK_NAME.Text == string.Empty ? Convert.ToString(txtBANK_NAME.Text) : "Bank Name: " + txtBANK_NAME.Text + ",";
            string BankAc_number = txtBANK_AC_NO.Text == string.Empty ? Convert.ToString(txtBANK_AC_NO.Text) : "Bank A/c Number: " + txtBANK_AC_NO.Text + ",";
            string BankIFSCode = txtIfsc.Text == string.Empty ? Convert.ToString(txtIfsc.Text) : "IFSC: " + txtIfsc.Text + ",";
            string msg = string.Empty;
            string MsgforMail = string.Empty;
            if (chkBankDetails.Checked == true)
            {
                msg = "Bank Details Of Party <br/>" + PartyName + " " + CityName + " " + PINCODE + " " + State + BankName + " " + BankAc_number + " " + BankIFSCode;
                MsgforMail = "Bank Details Of Party <br/>" + PartyName + " <br/>" + BankName + " <br/>" + BankAc_number + " <br/>" + BankIFSCode;
            }
            if (chkAddressDetails.Checked == true)
            {
                msg = PartyName + " Address:" + PartyAddress + " " + CityName + " " + State + Environment.NewLine + LOCAL_LIC_NO + TIN_NO +
                    CST_NO + GST_NO + ECC_NO + FSSAI + PAN;
                MsgforMail = PartyName + "<br/> Address:" + PartyAddress + " <br/>" + CityName + " <br/>" + State + " <br/>" + PINCODE + " <br/>" + LOCAL_LIC_NO + " <br/>" + TIN_NO + " <br/>" + CST_NO + " <br/>" + GST_NO + " <br/>" + ECC_NO + " <br/>" + FSSAI + " <br/>" + PAN + " <br/>";
            }
            if (e.CommandName == "sms")
            {
                if (!string.IsNullOrWhiteSpace(MOBILE))
                {
                    string msgAPI = clsGV.msgAPI;
                    string URL = msgAPI + "mobile=" + MOBILE + "&message=" + msg + "&senderid=NAVKAR&accusage=1";
                    clsCommon.apicall(URL);
                    //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(URL);
                    //HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                    //StreamReader reader = new StreamReader(response.GetResponseStream());
                    //string read = reader.ReadToEnd();
                    //reader.Close();
                    //response.Close();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "sms", "javascript:alert('SMS  Sent Successfully!')", true);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(EMAIL_ID))
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            string mailFrom = Session["EmailId"].ToString();
                            string smtpPort = "587";
                            string emailPassword = Session["EmailPassword"].ToString();
                            MailMessage msgs = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                            SmtpServer.Host = clsGV.Email_Address;
                            msgs.From = new MailAddress(mailFrom);
                            msgs.To.Add(EMAIL_ID);
                            msgs.Body = MsgforMail;
                            //msgs.Attachments.Add(attachment);
                            msgs.IsBodyHtml = true;
                            msgs.Subject = "Account details...";
                            msgs.IsBodyHtml = true;
                            if (smtpPort != string.Empty)
                            {
                                SmtpServer.Port = Convert.ToInt32(smtpPort);
                            }
                            SmtpServer.EnableSsl = true;
                            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                            SmtpServer.UseDefaultCredentials = false;
                            SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object k,
                                System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                                System.Security.Cryptography.X509Certificates.X509Chain chain,
                                System.Net.Security.SslPolicyErrors sslPolicyErrors)
                            {
                                return true;
                            };
                            SmtpServer.Send(msgs);
                        }
                    }
                    catch (Exception e1)
                    {
                        Response.Write("mail err:" + e1);
                        return;
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "sms", "javascript:alert('Email Sent Successfully!')", true);
                }
            }

            //ClearSendingSmsTextboxes();

        }
        catch (Exception)
        {
            throw;
        }
    }

    private void ClearSendingSmsTextboxes()
    {
        txtSendingAcCode.Text = string.Empty;
        txtSendingMobile.Text = string.Empty;
        txtSendingEmail.Text = string.Empty;
        lblSendingAcCode.Text = string.Empty;
        chkAddressDetails.Checked = false;
        chkBankDetails.Checked = false;
    }

    protected void txtSendingAcCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSendingAcCode.Text;
        strTextbox = "txtSendingAcCode";
        csCalculations();

    }
    protected void btntxtSendingAcCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtSendingAcCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    protected void chkAddressDetails_CheckedChanged(object sender, EventArgs e)
    {
        chkBankDetails.Checked = false;
    }
    protected void chkBankDetails_CheckedChanged(object sender, EventArgs e)
    {
        chkAddressDetails.Checked = false;
    }

    protected void txtEditDoc_No_TextChanged(object sender, EventArgs e)
    {
        try
        {
            bool a = clsCommon.isStringIsNumeric(txtEditDoc_No.Text);
            if (a == false)
            {
                txtSearchText.Text = txtEditDoc_No.Text.ToString();
                strTextbox = "txtAC_CODE";
                btntxtAC_CODE_Click(this, new EventArgs());
            }
            else
            {
                hdnf.Value = txtEditDoc_No.Text;
                string qry = "select * from " + qryCommon + " where Ac_Code='" + txtEditDoc_No.Text + "' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";
                this.fetchRecord(qry);
                setFocusControl(txtEditDoc_No);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    #region [txtGstStateCode_TextChanged]
    protected void txtGstStateCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGstStateCode.Text;
        strTextbox = "txtGstStateCode";
        csCalculations();
    }
    #endregion

    #region [txtstatecode_TextChanged]
    protected void txtstatecode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtstatecode.Text;
        strTextbox = "txtstatecode";
        csCalculations();
        modalCity.Show();
    }
    #endregion


    #region [btntxtGstStateCode_Click]
    protected void btntxtGstStateCode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGstStateCode";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion
    #region [btngststatecode_Click]
    protected void btngststatecode_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtstatecode";
            strTextbox = "txtstatecode";
            btnSearch_Click(sender, e);
            modalCity.Show();

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
            counts = Convert.ToInt32(clsCommon.getString("select count(Ac_Code) as Ac_Code from " + tblHead + " "));
            if (counts == 0)
            {
                txtAC_CODE.Text = "1";
                Doc_No = 1;
            }
            else
            {
                Doc_No = Convert.ToInt32(clsCommon.getString("SELECT ifnull(max(Ac_Code),0) as Doc_No from " + qryCommon + " where Company_Code=" + Session["Company_Code"].ToString() + " ")) + 1;
                txtAC_CODE.Text = Doc_No.ToString();
            }

            counts = Convert.ToInt32(clsCommon.getString("SELECT count(accoid) as accoid from " + tblHead + " "));
            if (counts == 0)
            {
                lblAc_Code.Text = "1";
                Acid = 1;
            }
            else
            {
                Acid = Convert.ToInt32(clsCommon.getString("SELECT max(accoid) as accoid from " + tblHead)) + 1;
                lblAc_Code.Text = Acid.ToString();
            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Next Record Error!')", true);
        }
    }
    #endregion


    protected void TextBankOpning_Bal_TextChanged(object sender, EventArgs e)
    {

    }
    protected void drpBankDrCr_SelectedIndexChanged1(object sender, EventArgs e)
    {

    }
}

