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

public partial class Sugar_pgeSystemMaster : System.Web.UI.Page
{
    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string cityMasterTable = string.Empty;
    string systemMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string voucherTable = string.Empty;
    int defaultAccountCode = 0;
    string trntype = string.Empty;
    string qryAccountList = string.Empty;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    string user = string.Empty;
    static WebControl objAsp = null;
    string cs = string.Empty;
    string Action = string.Empty;
    Int32 Doc_No = 0;
    int counts = 0;
    int count = 0;
    Int32 SystemId = 0;
    #endregion

    StringBuilder Head_Fields = null;
    StringBuilder Head_Values = null;
    StringBuilder Head_Update = null;

    MySqlConnection con = null;
    MySqlCommand cmd = null;
    MySqlTransaction myTran = null;

    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;


    #region text and label data Declaration
    string System_Type = string.Empty;
    Int32 System_Code = 0;
    string System_Name_E = string.Empty;
    double System_Rate = 0.00;
    Int32 Purchase_AC = 0;
    Int32 Sale_AC = 0;
    Int32 Vat_AC = 0;
    double Opening_Bal = 0.00;
    double KgPerKatta = 0.00;
    double Opening_Value = 0.00;
    string HSNNumber = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        tblPrefix = Session["tblPrefix"].ToString();
        tblHead = tblPrefix + "SystemMaster";
        tblDetails = tblPrefix + "";
        AccountMasterTable = tblPrefix + "AccountMaster";
        qryCommon = tblPrefix + "";
        qryAccountList = tblPrefix + "qryAccountsList";
        cityMasterTable = tblPrefix + "CityMaster";
        systemMasterTable = tblPrefix + "SystemMaster";
        user = Session["user"].ToString();
        //trntype = drpSystype.SelectedValue;
        //drpSystype.SelectedValue = trntype;
        pnlPopup.Style["display"] = "none";
        cs = ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
        con = new MySqlConnection(cs);

        Head_Fields = new StringBuilder();
        Head_Values = new StringBuilder();
        Head_Update = new StringBuilder();

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
                hdnfTranType.Value = Request.QueryString["System_Type"];
                if (Action == "1")
                {
                    hdnf.Value = Request.QueryString["systemid"];

                    clsButtonNavigation.enableDisable("N");
                    pnlPopup.Style["display"] = "none";
                    this.makeEmptyForm("N");
                    ViewState["mode"] = "I";
                    this.showLastRecord();
                    setFocusControl(txtsystemName);
                   // drpSystype.Enabled = false;
                }
                else
                {
                    drpSystype.Text = hdnfTranType.Value;
                    string docno = string.Empty;
                    clsButtonNavigation.enableDisable("A");
                    ViewState["mode"] = null;
                    ViewState["mode"] = "I";
                    this.makeEmptyForm("A");
                    this.NextNumber();
                    setFocusControl(txtsystemName);

                    if (drpSystype.SelectedValue == "I")
                    {
                        btntxtPurchaseAc.Enabled = true;
                        btntxtSaleAC.Enabled = true;
                        btntxtvatAC.Enabled = true;
                    }
                    else
                    {
                        btntxtPurchaseAc.Enabled = false;
                        btntxtSaleAC.Enabled = false;
                        btntxtvatAC.Enabled = false;
                    }

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

    private void NextNumber()
    {
        try
        {
            int counts = 0;
            counts = Convert.ToInt32(clsCommon.getString("select count(System_Code) as A from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' and System_Type='" + drpSystype.SelectedValue + "'"));
            if (counts == 0)
            {

                txtsystemcode.Text = "1";
                Doc_No = 1;
            }
            else
            {
                Doc_No = Convert.ToInt32(clsCommon.getString("SELECT max(System_Code) as Doc_No from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' and System_Type='" + drpSystype.SelectedValue + "' ")) + 1;
                txtsystemcode.Text = Doc_No.ToString();
            }

            counts = Convert.ToInt32(clsCommon.getString("SELECT count(systemid) as bsid from " + tblHead + " "));
            if (counts == 0)
            {
                lblbsid.Text = "1";
                SystemId = 1;
            }
            else
            {
                SystemId = Convert.ToInt32(clsCommon.getString("SELECT max(systemid) as bsid from " + tblHead)) + 1;
                lblbsid.Text = SystemId.ToString();
            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Next Record Error!')", true);
        }
    }
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
        //try
        //{
        //    string qry = string.Empty;
        //    qry = "select max(System_Code) as System_Code from " + tblHead +
        //         " where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and System_Type='" + trntype + "'";

        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable();
        //    ds = clsDAL.SimpleQuery(qry);
        //    if (ds != null)
        //    {
        //        if (ds.Tables.Count > 0)
        //        {
        //            dt = ds.Tables[0];
        //            if (dt.Rows.Count > 0)
        //            {
        //                hdnf.Value = dt.Rows[0][0].ToString();
        //                qry = getDisplayQuery();
        //                bool recordExist = this.fetchRecord(qry);
        //                if (recordExist == true)
        //                {
        //                    btnEdit.Enabled = true;
        //                    btnDelete.Enabled = true;
        //                    btnAdd.Focus();
        //                    //this.enableDisableNavigateButtons();
        //                }
        //                else                     //new code
        //                {
        //                    this.makeEmptyForm("N");
        //                    btnEdit.Enabled = false;
        //                    btnDelete.Enabled = false;
        //                }
        //            }
        //        }
        //    }
        //}
        //catch
        //{
        //}

    }

    private string getDisplayQuery()
    {


        try
        {

            string qryDisplay = "select * from " + tblHead +
                " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and systemid=" + hdnf.Value + " and System_Type='" + hdnfTranType.Value + "'";
            return qryDisplay;
        }
        catch
        {
            return "";
        }
    }

    private void makeEmptyForm(string dAction)
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
            btnSave.Text = "Save";
            btntxtsystemcode.Text = "Choose No";
            btntxtsystemcode.Enabled = false;
            lblMsg.Text = string.Empty;
            txtEditDoc_No.Enabled = true;

            #region logic
            txtPurchaseAc.Enabled = false;
            txtSaleAC.Enabled = false;
            txtvatAC.Enabled = false;
            txtOpeningBal.Enabled = false;
            btntxtPurchaseAc.Enabled = false;
            btntxtSaleAC.Enabled = false;
            btntxtvatAC.Enabled = false;
            txtOpening_Value.Enabled = false;
            lblPurchaseACName.Text = string.Empty;
            lblSaleACName.Text = string.Empty;
            lblVatACName.Text = string.Empty;
            drpSystype.Enabled = true;
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
            btnSave.Text = "Save";
            btntxtsystemcode.Text = "Change No";
            btntxtsystemcode.Enabled = true;
            txtsystemcode.Enabled = false;
            setFocusControl(txtsystemName);
            txtPurchaseAc.Enabled = true;
            txtSaleAC.Enabled = true;
            txtvatAC.Enabled = true;
            txtOpeningBal.Enabled = true;
            txtOpening_Value.Enabled = true;
            #region logic
            if (drpSystype.SelectedValue == "I")
            {
                txtPurchaseAc.Enabled = true;
                txtSaleAC.Enabled = true;
                txtvatAC.Enabled = true;
                txtOpeningBal.Enabled = true;
                btntxtPurchaseAc.Enabled = true;
                btntxtSaleAC.Enabled = true;
                btntxtvatAC.Enabled = true;

                lblPurchaseACName.Text = string.Empty;
                lblSaleACName.Text = string.Empty;
                lblVatACName.Text = string.Empty;

            }
            if (drpSystype.SelectedValue == "S")
            {
                txtPurchaseAc.Enabled = false;
                txtSaleAC.Enabled = false;
                txtvatAC.Enabled = false;
                txtOpeningBal.Enabled = false;
                btntxtPurchaseAc.Enabled = false;
                btntxtSaleAC.Enabled = false;
                btntxtvatAC.Enabled = false;
                txtSysRate.Enabled = false;
                lblPurchaseACName.Text = string.Empty;
                lblSaleACName.Text = string.Empty;
                lblVatACName.Text = string.Empty;
            }
            //else
            //{
            //    txtPurchaseAc.Enabled = false;
            //    txtSaleAC.Enabled = false;
            //    txtvatAC.Enabled = false;
            //    txtOpeningBal.Enabled = false;
            //    btntxtPurchaseAc.Enabled = false;
            //    btntxtSaleAC.Enabled = false;
            //    btntxtvatAC.Enabled = false;

            //    lblPurchaseACName.Text = string.Empty;
            //    lblSaleACName.Text = string.Empty;
            //    lblVatACName.Text = string.Empty;
            //}
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
            btntxtsystemcode.Text = "Choose No";
            btntxtsystemcode.Enabled = false;
            txtEditDoc_No.Enabled = true;
            txtOpening_Value.Enabled = false;
            #region logic
            drpSystype.Enabled = true;
            btntxtPurchaseAc.Enabled = false;
            btntxtSaleAC.Enabled = false;
            btntxtvatAC.Enabled = false;
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
            btntxtsystemcode.Text = "Choose No";
            btntxtsystemcode.Enabled = true;
            lblMsg.Text = string.Empty;
            setFocusControl(txtsystemName);
            txtEditDoc_No.Enabled = false;
            txtOpening_Value.Enabled = true;
            if (drpSystype.SelectedValue == "I")
            {
                txtKgPerKatta.Enabled = true;
            }
            else
            {
                txtKgPerKatta.Enabled = false;
            }
            #region logic
            if (drpSystype.SelectedValue == "I")
            {
                txtPurchaseAc.Enabled = true;
                txtSaleAC.Enabled = true;
                txtvatAC.Enabled = true;
                txtOpeningBal.Enabled = true;
                btntxtPurchaseAc.Enabled = true;
                btntxtSaleAC.Enabled = true;
                btntxtvatAC.Enabled = true;

            }
            else
            {
                txtPurchaseAc.Enabled = false;
                txtSaleAC.Enabled = false;
                txtvatAC.Enabled = false;
                txtOpeningBal.Enabled = false;
                btntxtPurchaseAc.Enabled = false;
                btntxtSaleAC.Enabled = false;
                btntxtvatAC.Enabled = false;

                lblPurchaseACName.Text = string.Empty;
                lblSaleACName.Text = string.Empty;
                lblVatACName.Text = string.Empty;
            }

            #endregion
        }
    }

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

    protected void txtsystemcode_TextChanged(object sender, EventArgs e)
    {
        #region code
        try
        {
            int n;
            bool isNumeric = int.TryParse(txtsystemcode.Text, out n);

            if (isNumeric == true)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string txtValue = "";
                if (txtsystemcode.Text != string.Empty)
                {
                    txtValue = txtsystemcode.Text;

                    string qry = "select * from " + tblHead + " where  System_Code='" + txtValue + "' " +
                        "  and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and System_Type='" + trntype + "'";

                    ds = clsDAL.SimpleQuery(qry);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                //Record Found
                                hdnf.Value = dt.Rows[0]["System_Code"].ToString();

                                if (ViewState["mode"] != null)
                                {
                                    if (ViewState["mode"].ToString() == "I")
                                    {
                                        lblMsg.Text = "** Doc No (" + txtValue + ") Already Exist";
                                        lblMsg.ForeColor = System.Drawing.Color.Red;
                                        // this.getMaxCode();
                                        //txtDoc_no.Enabled = false;

                                        btnSave.Enabled = true;   //IMP                                       
                                        setFocusControl(txtsystemName);
                                    }

                                    if (ViewState["mode"].ToString() == "U")
                                    {
                                        //fetch record
                                        qry = getDisplayQuery();
                                        bool recordExist = this.fetchRecord(qry);
                                        if (recordExist == true)
                                        {
                                            txtsystemcode.Enabled = false;
                                            setFocusControl(txtsystemcode);

                                            hdnf.Value = txtsystemcode.Text;

                                        }
                                    }
                                }
                            }
                            else   //Record Not Found
                            {
                                if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                {
                                    lblMsg.Text = "";
                                    setFocusControl(txtsystemName);
                                    txtsystemcode.Enabled = false;
                                    btnSave.Enabled = true;   //IMP
                                }
                                if (ViewState["mode"].ToString() == "U")
                                {
                                    this.makeEmptyForm("E");
                                    lblMsg.Text = "** Record Not Found";
                                    lblMsg.ForeColor = System.Drawing.Color.Red;
                                    txtsystemcode.Text = string.Empty;
                                    setFocusControl(txtsystemcode);
                                }
                            }
                        }
                    }
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    setFocusControl(txtsystemcode);
                }
            }
            else
            {
                this.makeEmptyForm("A");
                lblMsg.Text = "Doc No is numeric";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                clsButtonNavigation.enableDisable("E");
                txtsystemcode.Text = string.Empty;
                setFocusControl(txtsystemcode);
            }
        }
        catch
        {

        }
        #endregion
    }

    protected void btntxttxtsystemcode_click(object sender, EventArgs e)
    {
        try
        {
            if (btntxtsystemcode.Text == "Change No")
            {
                pnlPopup.Style["display"] = "none";
                txtsystemcode.Text = string.Empty;
                txtsystemcode.Enabled = true;

                btnSave.Enabled = false;
                setFocusControl(txtsystemcode);
            }

            if (btntxtsystemcode.Text == "Choose No")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtsystemcode";
                btnSearch_Click(sender, e);
            }
        }
        catch
        {
        }
    }

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
            if (searchString != string.Empty || strTextBox == hdnfClosePopup.Value)
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

            if (hdnfClosePopup.Value == "txtsystemcode" || hdnfClosePopup.Value == "txtEditDoc_No")
            {
                if (btntxtsystemcode.Text == "Change No")
                {
                    pnlPopup.Style["display"] = "none";
                    txtsystemcode.Text = string.Empty;
                    txtsystemcode.Enabled = true;

                    btnSave.Enabled = false;
                    setFocusControl(txtsystemcode);
                    hdnfClosePopup.Value = "Close";
                }

                if (btntxtsystemcode.Text == "Choose No")
                {
                    lblPopupHead.Text = "--Select Group--";
                    //string qry = "SELECT  System_Code,System_Name_E from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                    //    + " and System_Type='" + drpSystype.SelectedValue + "'";
                    //this.showPopup(qry);


                    //lblPopupHead.Text = "--Select DOC No--";
                    string qry = "select System_Code,System_Name_E from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +

                        " and (System_Code like '%" + txtSearchText.Text + "%' or System_Name_E like '%" + txtSearchText.Text + "%')";
                    this.showPopup(qry);
                }
            }
            if (hdnfClosePopup.Value == "txtPurchaseAc" || hdnfClosePopup.Value == "txtSaleAC")
            {
                lblPopupHead.Text = "--Select Purchase AC--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from qrymstaccountmaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                    " (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName  like '%" + txtSearchText.Text + "%' )";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtvatAC")
            {
                string qry = "select Doc_no,GST_Name,Rate from nt_1_gstratemaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                                   " (Doc_no like '%" + txtSearchText.Text + "%' or GST_Name like '%" + txtSearchText.Text + "%' or Rate  like '%" + txtSearchText.Text + "%' )";
                this.showPopup(qry);
            }
        }
        catch
        {
        }

    }


    #region [imgBtnClose_Click]
    protected void imgBtnClose_Click(object sender, EventArgs e)
    {
        try
        {
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


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        string docno = string.Empty;
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("A");
        drpSystype.Enabled = false;
        this.NextNumber();
        setFocusControl(txtsystemName);

        if (drpSystype.SelectedValue == "I")
        {
            btntxtPurchaseAc.Enabled = true;
            btntxtSaleAC.Enabled = true;
            btntxtvatAC.Enabled = true;
        }
        else
        {
            btntxtPurchaseAc.Enabled = false;
            btntxtSaleAC.Enabled = false;
            btntxtvatAC.Enabled = false;
        }

    }

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion

    private void getMaxCode()
    {
        try
        {
            DataSet ds = null;

            using (clsGetMaxCode obj = new clsGetMaxCode())
            {
                obj.tableName = tblHead + " where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and System_Type='" + drpSystype.SelectedValue + "'";
                obj.code = "System_Code";
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
                                    txtsystemcode.Text = ds.Tables[0].Rows[0][0].ToString();
                                    txtsystemcode.Enabled = false;
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            #region validation
            bool isValidated = true;
            if (txtsystemcode.Text != string.Empty)
            {
                if (ViewState["mode"] != null)
                {
                    if (ViewState["mode"].ToString() == "I")
                    {
                        string str = clsCommon.getString("select System_Name_E from " + tblHead + " where System_Code=" + txtsystemcode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and System_Type='" + drpSystype.SelectedValue + "'");
                        if (str != "0")
                        {
                            lblMsg.Text = "Code " + txtsystemcode.Text + " already exist";
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
                setFocusControl(txtsystemcode);
            }


            if (txtsystemName.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtsystemName);
            }
            if (drpSystype.SelectedValue == "V")
            {
                if (txtSysRate.Text != string.Empty)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtSysRate);
                }
            }

            if (drpSystype.SelectedValue == "I")          //if type I=Item
            {
                if (txtPurchaseAc.Text != string.Empty)
                {
                    //string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPurchaseAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtPurchaseAc.Text + "");
                    if (str != string.Empty)
                    {
                        isValidated = true;
                    }
                    else
                    {
                        isValidated = false;
                        setFocusControl(txtPurchaseAc);
                        return;
                    }
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtPurchaseAc);
                    return;
                }

                if (txtSaleAC.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtSaleAC.Text + "");
                    if (str != string.Empty)
                    {
                        isValidated = true;
                    }
                    else
                    {
                        isValidated = false;
                        setFocusControl(txtSaleAC);
                        return;
                    }
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtSaleAC);
                    return;
                }
                if (txtvatAC.Text != string.Empty)
                {

                    isValidated = true;

                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtvatAC);
                    return;
                }
            }


            #endregion

            #region declaration
            string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");
            System_Type = drpSystype.SelectedValue;
            // Int32 System_Code = txtsystemcode.Text != string.Empty ? Convert.ToInt32(txtsystemcode.Text) : 0;
            // SystemId = lblbsid.Text != string.Empty ? Convert.ToInt32(lblbsid.Text) : 0;
            System_Name_E = txtsystemName.Text.ToUpper();
            System_Rate = txtSysRate.Text != string.Empty ? Convert.ToDouble(txtSysRate.Text) : 0.00;
            Purchase_AC = txtPurchaseAc.Text != string.Empty ? Convert.ToInt32(txtPurchaseAc.Text) : 0;
            Sale_AC = txtSaleAC.Text != string.Empty ? Convert.ToInt32(txtSaleAC.Text) : 0;
            Vat_AC = txtvatAC.Text != string.Empty ? Convert.ToInt32(txtvatAC.Text) : 0;
            Opening_Bal = txtOpeningBal.Text != string.Empty ? Convert.ToDouble(txtOpeningBal.Text) : 0.00;
            KgPerKatta = txtKgPerKatta.Text.Trim() != string.Empty ? Convert.ToDouble(txtKgPerKatta.Text) : 100;
            HSNNumber = txtHsnNumber.Text;
            Opening_Value = Convert.ToDouble(txtOpening_Value.Text != string.Empty ? txtOpening_Value.Text : "0.00");
            string retValue = string.Empty;
            string strRev = string.Empty;


            int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            //int Year_Code = Convert.ToInt32(Session["year"].ToString());
            int year_Code = Convert.ToInt32(Session["year"].ToString());
            int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
            string Created_By = Session["user"].ToString();
            string Modified_By = Session["user"].ToString();
            //string Head_Fields = string.Empty;
            //string Head_Values = string.Empty;
            //string Head_Update = string.Empty;
            int flag = 0;
            int no = 0;
            string op = string.Empty;
            string returnmaxno = string.Empty;
            #endregion

            if (btnSave.Text == "Save")
            {
                this.NextNumber();
                #region Create Head Insert  Query
                Head_Fields.Append("System_Type,");
                Head_Values.Append("'" + System_Type + "',");
                Head_Fields.Append("System_Code,");
                Head_Values.Append("'" + Doc_No + "',");
                Head_Fields.Append("System_Name_E,");
                Head_Values.Append("'" + System_Name_E + "',");
                Head_Fields.Append("System_Rate,");
                Head_Values.Append("'" + System_Rate + "',");
                Head_Fields.Append("Purchase_AC,");
                Head_Values.Append("'" + Purchase_AC + "',");
                Head_Fields.Append("Sale_AC,");
                Head_Values.Append("'" + Sale_AC + "',");
                Head_Fields.Append("Vat_AC,");
                Head_Values.Append("'" + Vat_AC + "',");
                Head_Fields.Append("Opening_Bal,");
                Head_Values.Append("'" + Opening_Bal + "',");
                Head_Fields.Append("KgPerKatta,");
                Head_Values.Append("'" + KgPerKatta + "',");
                Head_Fields.Append("HSN,");
                Head_Values.Append("'" + HSNNumber + "',");
                Head_Fields.Append("Opening_Value,");
                Head_Values.Append("'" + Opening_Value + "',");
                Head_Fields.Append("systemid,");
                Head_Values.Append("'" + SystemId + "',");
                Head_Fields.Append("Year_Code,");
                Head_Values.Append("'" + year_Code + "',");
                Head_Fields.Append("Company_Code,");
                Head_Values.Append("'" + Company_Code + "' ,");
                Head_Fields.Append("Created_By");
                Head_Values.Append("'" + Created_By + "'");
                #endregion
                flag = 1;
                qry = "insert into " + tblHead + "(" + Head_Fields + ") values(" + Head_Values + ")";
                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = qry;
                Maindt.Rows.Add(dr);

            }
            else
            {
                #region Create Update Query
                Head_Update.Append("System_Name_E=");
                Head_Update.Append("'" + System_Name_E + "',");
                Head_Update.Append("System_Rate=");
                Head_Update.Append("'" + System_Rate + "',");
                Head_Update.Append("Purchase_AC=");
                Head_Update.Append("'" + Purchase_AC + "',");
                Head_Update.Append("Sale_AC=");
                Head_Update.Append("'" + Sale_AC + "',");
                Head_Update.Append("Vat_AC=");
                Head_Update.Append("'" + Vat_AC + "',");
                Head_Update.Append("Opening_Bal=");
                Head_Update.Append("'" + Opening_Bal + "',");

                Head_Update.Append("KgPerKatta=");
                Head_Update.Append("'" + KgPerKatta + "',");
                Head_Update.Append("HSN=");
                Head_Update.Append("'" + HSNNumber + "',");
                Head_Update.Append("Opening_Value=");
                Head_Update.Append("'" + Opening_Value + "',");
                Head_Update.Append("Modified_By=");
                Head_Update.Append("'" + Modified_By + "'");
                #endregion

                flag = 2;
                qry = "update " + tblHead + " set " + Head_Update + " where Company_Code='" + Company_Code + "' and System_Code='" + txtsystemcode.Text + "' and System_Type='" + drpSystype.Text + "'";
                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = qry;
                Maindt.Rows.Add(dr);




            }
            string msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);

            if (msg == "Insert")
            {

                hdnf.Value = SystemId.ToString();
                clsButtonNavigation.enableDisable("S");
                this.makeEmptyForm("S");
                qry = getDisplayQuery();
                this.fetchRecord(qry);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Added !')", true);
            }
            else if (msg == "Update")
            {
                hdnf.Value = lblbsid.Text;
                clsButtonNavigation.enableDisable("S");
                this.makeEmptyForm("S");
                qry = getDisplayQuery();
                this.fetchRecord(qry);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update !')", true);
            }

        }
        catch
        {

        }
    }


    private bool fetchRecord(string qry)
    {
        try
        {
            bool recordExist = false;
            if (hdnf.Value != string.Empty)
            {
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
                            #region[assign value]

                            txtsystemcode.Text = dt.Rows[0]["System_Code"].ToString();
                            lblbsid.Text = hdnf.Value;

                            //drpSystype.SelectedValue = dt.Rows[0]["System_Type"].ToString();
                            drpSystype.SelectedValue = hdnfTranType.Value;
                            txtsystemName.Text = dt.Rows[0]["System_Name_E"].ToString();
                            txtSysRate.Text = dt.Rows[0]["System_Rate"].ToString();
                            string purch_Ac = dt.Rows[0]["Purchase_AC"].ToString();
                            txtPurchaseAc.Text = purch_Ac;
                            lblPurchaseACName.Text = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + purch_Ac + "");
                            string sale_Ac = dt.Rows[0]["Sale_AC"].ToString();
                            txtSaleAC.Text = sale_Ac;
                            lblSaleACName.Text = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + sale_Ac + " ");
                            string vat_Ac = dt.Rows[0]["Vat_AC"].ToString();
                            txtvatAC.Text = vat_Ac;
                            lblVatACName.Text = clsCommon.getString("Select Ac_Name_E from " + tblPrefix + "AccountMaster where Ac_Code=" + vat_Ac + " ");
                            txtOpeningBal.Text = dt.Rows[0]["Opening_Bal"].ToString();
                            txtKgPerKatta.Text = dt.Rows[0]["KgPerKatta"].ToString();
                            txtHsnNumber.Text = dt.Rows[0]["HSN"].ToString();
                            txtOpening_Value.Text = dt.Rows[0]["Opening_Value"].ToString();

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
                            #endregion
                            recordExist = true;
                            lblMsg.Text = "";
                        }
                    }
                }
                // this.enableDisableNavigateButtons();
                //hdnf.Value = txtsystemcode.Text;
                return recordExist;
            }
            else
            {
                //this.enableDisableNavigateButtons();
                return recordExist;
            }
        }
        catch
        {
            return false;
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        clsButtonNavigation.enableDisable("E");

        this.makeEmptyForm("E");
        txtsystemcode.Enabled = false;
        drpSystype.Enabled = false;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {

                string currentDoc_No = txtsystemcode.Text;
                int flag = 3;
                string strrev = "";

                qry = "delete from " + tblHead + " where System_Code='" + currentDoc_No + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and systemid='" + hdnf.Value + "'";
                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = qry;
                Maindt.Rows.Add(dr);

                string msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);
                if (msg == "Delete")
                {
                    Response.Redirect("../Master/PgeSystemMasterUtility.aspx");
                }
                else
                {
                    lblMsg.Text = "Cannot delete this Group , it is in use";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                hdnf.Value = txtsystemcode.Text;
                showLastRecord();
            }
        }
        catch
        {
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        hdnf.Value = Request.QueryString["systemid"];
        hdnfTranType.Value = Request.QueryString["System_Type"];
        if (hdnf.Value == "0")
        {
            hdnf.Value = clsCommon.getString("select max(systemid) from " + tblHead + " where System_Type='" + hdnfTranType.Value + "' and Company_Code=" + Session["Company_Code"].ToString() + " ");
        }
        clsButtonNavigation.enableDisable("S");
        this.makeEmptyForm("S");
        showLastRecord();


    }



    #region navigateRecord
    private void navigateRecord()
    {
        try
        {
            if (hdnf.Value != string.Empty)
            {
                ViewState["mode"] = "U";
                txtsystemcode.Text = hdnf.Value;

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




    protected void txtsystemName_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtSysRate);
    }

    protected void txtSysRate_TextChanged(object sender, EventArgs e)
    {
        if (txtPurchaseAc.Enabled == true)
        {
            setFocusControl(txtPurchaseAc);
        }
        else
        {
            setFocusControl(btnSave);
        }
    }

    protected void txtPurchaseAc_TextChanged(object sender, EventArgs e)
    {

        if (txtPurchaseAc.Text != string.Empty)
        {
            bool a = clsCommon.isStringIsNumeric(txtPurchaseAc.Text);
            if (a == false)
            {
                searchString = txtPurchaseAc.Text;
                btntxtPurchaseAc_Click(this, new EventArgs());
            }
            else
            {
                string str = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_Code=" + txtPurchaseAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty && str != "0")
                {
                    lblPurchaseACName.Text = str;
                    setFocusControl(txtSaleAC);
                }
                else
                {
                    lblPurchaseACName.Text = string.Empty;
                    txtPurchaseAc.Text = string.Empty;
                    setFocusControl(txtSaleAC);
                }
            }
        }
        else
        {
            lblPurchaseACName.Text = string.Empty;
            txtPurchaseAc.Text = string.Empty;
            setFocusControl(txtPurchaseAc);
        }
    }

    protected void txtSaleAC_TextChanged(object sender, EventArgs e)
    {
        if (txtSaleAC.Text != string.Empty)
        {
            bool a = clsCommon.isStringIsNumeric(txtSaleAC.Text);
            if (a == false)
            {
                searchString = txtSaleAC.Text;
                btntxtSaleAC_Click(this, new EventArgs());
            }
            else
            {
                string str = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_Code=" + txtSaleAC.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty && str != "0")
                {
                    lblSaleACName.Text = str;
                    setFocusControl(txtvatAC);
                }
                else
                {
                    lblSaleACName.Text = string.Empty;
                    txtSaleAC.Text = string.Empty;
                    setFocusControl(txtvatAC);
                }
            }
        }
        else
        {
            lblSaleACName.Text = string.Empty;
            txtSaleAC.Text = string.Empty;
            setFocusControl(txtSaleAC);
        }
    }

    protected void txtvatAC_TextChanged(object sender, EventArgs e)
    {

        if (txtvatAC.Text != string.Empty)
        {
            bool a = clsCommon.isStringIsNumeric(txtvatAC.Text);
            if (a == false)
            {
                searchString = txtvatAC.Text;
                btntxtvatAC_Click(this, new EventArgs());
            }
            else
            {
                string str = clsCommon.getString("select GST_Name from nt_1_gstratemaster where Doc_no=" + txtvatAC.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty && str != "0")
                {
                    lblVatACName.Text = str;
                    setFocusControl(txtOpeningBal);
                }
                else
                {
                    lblVatACName.Text = string.Empty;
                    txtvatAC.Text = string.Empty;
                    setFocusControl(txtOpeningBal);
                }
            }
        }
        else
        {
            lblVatACName.Text = string.Empty;
            txtvatAC.Text = string.Empty;
            setFocusControl(txtvatAC);
        }
    }

    protected void txtOpeningBal_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtKgPerKatta);
    }

    protected void btntxtPurchaseAc_Click(object sender, EventArgs e)
    {
        pnlPopup.Style["display"] = "block";
        hdnfClosePopup.Value = "txtPurchaseAc";
        searchString = txtPurchaseAc.Text;
        btnSearch_Click(sender, e);
    }

    protected void btntxtSaleAC_Click(object sender, EventArgs e)
    {
        pnlPopup.Style["display"] = "block";
        hdnfClosePopup.Value = "txtSaleAC";
        btnSearch_Click(sender, e);

    }

    protected void btntxtvatAC_Click(object sender, EventArgs e)
    {
        pnlPopup.Style["display"] = "block";
        hdnfClosePopup.Value = "txtvatAC";
        btnSearch_Click(sender, e);
    }

    protected void drpSystype_TextChanged(object sender, EventArgs e)
    {
        string s_item = drpSystype.SelectedValue;
        hdnf.Value = clsCommon.getString("select max(systemid) from " + tblHead + " where System_Type='" + s_item + "' and Company_Code=" + Session["Company_Code"].ToString() + " ");
        Action = "1";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kys", "javascript:SY('" + hdnf.Value + "','" + Action + "','" + s_item + "')", true);
        //drpSystype.SelectedValue = s_item;
        //trntype = s_item;
        //this.NextNumber();
        //showLastRecord();
        //if (drpSystype.SelectedValue == "I")
        //{
        //    btntxtPurchaseAc.Enabled = true;
        //    btntxtSaleAC.Enabled = true;
        //    btntxtvatAC.Enabled = true;
        //}
        //else
        //{
        //    btntxtPurchaseAc.Enabled = false;
        //    btntxtSaleAC.Enabled = false;
        //    btntxtvatAC.Enabled = false;
        //}


    }

    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);

    }

    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Width = new Unit("60px");
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].Width = new Unit("80px");
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
        }
    }

}