using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml.Linq;
using System.Net;
using System.IO;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Threading;
using System.Text;

public partial class Sugar_Transaction_pgeJournal_Voucher : System.Web.UI.Page
{
    #region data section
    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;

    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string qryDetail = string.Empty;
    string cityMasterTable = string.Empty;
    string systemMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    int defaultAccountCode = 0;
    string trntype = "JV";
    string qryAccountList = string.Empty;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    string GLedgerTable = string.Empty;
    string user = string.Empty;
    static WebControl objAsp = null;

    string cs = string.Empty;
    string Action = string.Empty;
    int Autoid = 0;                          //Autoincrement id
    int id = 0;
    int flag = 0;
    int count = 0;
    int Doc_No = 0;
    MySqlConnection con = null;
    MySqlCommand cmd = null;
    MySqlTransaction myTran = null;


    int Detail_Id = 2;
    int debit_ac = 3;
    int debitAcName = 4;
    int DRCR = 5;
    int amount = 6;
    int narration = 7;
    int tenderdetailid = 8;
    int Rowaction = 9;
    int Srno = 10;
    #endregion

    #region Detail part Declaration
    int Grid_Id = 0;
    int Dr_Ac_Code = 0;
    int Cr_Ac_Code = 0;
    string DRCRGrid = string.Empty;
    double Grid_amount = 0.00;
    string Narration = string.Empty;
    int CA_Id = 0;
    int DA_Id = 0;

    string Detail_Insert = string.Empty;
    StringBuilder Detail_Update = null;
    StringBuilder Detail_Delete = null;
    StringBuilder Detail_Fields = null;
    StringBuilder Detail_Values = null;
    #endregion

    #region -Head part declearation
    int Company_Code = 0;
    int Year_Code = 0;
    int Branch_Code = 0;
    string Created_By = string.Empty;
    string Modified_By = string.Empty;
    string Created_Date = string.Empty;
    string Modified_Date = string.Empty;
    string retValue = string.Empty;
    string strRev = string.Empty;
    int IsSave = 0;

    string tran_type = string.Empty;
    string doc_date = string.Empty;
    int cashbank = 0;
    double total = 0.00;
    int CB_Id = 0;


    StringBuilder Head_Update = null;
    string Head_Insert = string.Empty;
    string Head_Delete = string.Empty;
    StringBuilder Head_Values = null;
    StringBuilder Head_Fields = null;
    string GLEDGER_Insert = string.Empty;
    string GLEDGER_Delete = string.Empty;
    string msg = string.Empty;
    #endregion-End of Head part declearation

    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = "nt_1_transacthead";
            tblDetails = "nt_1_transactdetail";

            qryCommon = "qrytransacthead";
            qryDetail = "qrytransactdetail";
            AccountMasterTable = "qrymstaccountmaster";
            qryAccountList = "qrymstaccountmaster";
            systemMasterTable = tblPrefix + "SystemMaster";
            GLedgerTable = tblPrefix + "GLEDGER";
            pnlPopup.Style["display"] = "none";
            user = Session["user"].ToString();
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
                    if (Action == "1")
                    {
                        hdnf.Value = Request.QueryString["tranid"];
                        pnlPopup.Style["display"] = "none";
                        ViewState["currentTable"] = null;
                        clsButtonNavigation.enableDisable("N");
                        this.makeEmptyForm("N");
                        ViewState["mode"] = "I";
                        this.showLastRecord();
                    }
                    else
                    {
                        string docno = string.Empty;
                        clsButtonNavigation.enableDisable("A");
                        ViewState["mode"] = null;
                        ViewState["mode"] = "I";
                        this.makeEmptyForm("A");
                        this.NextNumber();
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
                obj.tableName = tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                    + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "and Tran_Type='" + trntype + "'";
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
                txtEditDoc_No.Enabled = true;
                pnlPopup.Style["display"] = "none";
                //btnOpenDetailsPopup.Enabled = false;
                btnSave.Text = "Save";
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                lblMsg.Text = string.Empty;
                calenderExtenderDate.Enabled = false;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                pnlgrdDetail.Enabled = false;
                btntxtdebit_ac.Enabled = false;
                btntxtnarration.Enabled = false;
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
                txtEditDoc_No.Enabled = false;
                btnSave.Text = "Save";
                btntxtdoc_no.Text = "Change No";
                btntxtdoc_no.Enabled = false;
                txtdoc_no.Enabled = false;
                txtdoc_date.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                lbldiff.Text = string.Empty;
                lbldebittotal.Text = string.Empty;
                lblcredittotal.Text = string.Empty;
                #region set Business logic for save
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                setFocusControl(txtdoc_date);
                calenderExtenderDate.Enabled = true;
                //btnOpenDetailsPopup.Enabled = true;
                pnlgrdDetail.Enabled = true;
                btntxtdebit_ac.Enabled = true;
                lblDebit_name.Text = string.Empty;
                btntxtnarration.Enabled = true;
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
                txtEditDoc_No.Enabled = true;
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                calenderExtenderDate.Enabled = false;
                // btnOpenDetailsPopup.Enabled = false;
                pnlgrdDetail.Enabled = false;
                btntxtdebit_ac.Enabled = false;
                btntxtnarration.Enabled = false;
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
                txtEditDoc_No.Enabled = true;
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = true;
                lblMsg.Text = string.Empty;
                setFocusControl(txtdoc_date);
                calenderExtenderDate.Enabled = true;
                //btnOpenDetailsPopup.Enabled = true;
                pnlgrdDetail.Enabled = true;
                btntxtdebit_ac.Enabled = true;
                btntxtnarration.Enabled = true;
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
                btnEdit.Focus();
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
                            "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
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
                            "   and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
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

        this.makeEmptyForm("A");
        this.NextNumber();
        setFocusControl(txtdoc_date);

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
    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                string str = string.Empty;
                DataSet xml_ds = new DataSet();
                if (str == string.Empty)
                {
                    string currentDoc_No = lblJV_Id.Text;
                    DataSet ds = new DataSet();
                    string strrev = "";

                    Head_Delete = "delete from " + tblHead + " where tranid='" + lblJV_Id.Text + "'";
                    string Detail_Deleteqry = "delete from " + tblDetails + " where tranid='" + lblJV_Id.Text + "'";
                    GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='JV' and Doc_No=" + txtdoc_no.Text + " and " +
                        " COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

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
                        Response.Redirect("../Transaction/PgeJVUtility.aspx");
                    }

                }
                else
                {
                    lblMsg.Text = "Cannot delete this Group , it is in use";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
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
        string max = clsCommon.getString("select ifnull(max(tranid),0) as id from " + tblHead + " where tran_type='JV' and Company_Code=" + Session["Company_Code"].ToString() +
        " and Year_Code=" + Session["year"].ToString() + "");
        hdnf.Value = max;
        trntype = "JV";
        qry = getDisplayQuery();
        clsButtonNavigation.enableDisable("S");
        this.makeEmptyForm("S");
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
                        lblJV_Id.Text = hdnf.Value;
                        txtdoc_no.Text = dt.Rows[0]["DOC_NO"].ToString();
                        txtdoc_date.Text = dt.Rows[0]["DOC_DATE"].ToString();
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
                        #region ---------- Details -------------


                        qry = "select detail_id as ID,debit_ac as debit_ac,debitname as debitAcName,drcr as DRCR, amount as amount,narration as narration,trandetailid from "
                          + qryDetail + " where tranid=" + hdnf.Value + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "' ";
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
                }
            }

            columnTotal();
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
            string qryDisplay = "select * from " + qryCommon + " where tranid=" + hdnf.Value + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";
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
    //protected void btnOpenDetailsPopup_Click(object sender, EventArgs e)
    //{
    //    btnAdddetails.Text = "ADD";

    //    lblNo.Text = string.Empty;
    //    lblID.Text = string.Empty;
    //    setFocusControl(txtdebit_ac);
    //}
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

                        // rowIndex = dt.Rows.Count + 1;
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
                        string id = clsCommon.getString("select detail_id from " + tblDetails + " where doc_no=" + txtdoc_no.Text + " " +
                        " And Company_Code = " + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " +
                        "Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='JV' and detail_id=" + rowIndex + "");
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
                    dt.Columns.Add((new DataColumn("debit_ac", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("debitAcName", typeof(string))));
                    dt.Columns.Add((new DataColumn("DRCR", typeof(string))));

                    dt.Columns.Add((new DataColumn("amount", typeof(double))));
                    dt.Columns.Add((new DataColumn("narration", typeof(string))));
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
                dt.Columns.Add((new DataColumn("debit_ac", typeof(Int32))));
                dt.Columns.Add((new DataColumn("debitAcName", typeof(string))));
                dt.Columns.Add((new DataColumn("DRCR", typeof(string))));

                dt.Columns.Add((new DataColumn("amount", typeof(double))));
                dt.Columns.Add((new DataColumn("narration", typeof(string))));
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
            if (txtdebit_ac.Text != string.Empty)
            {
                string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtdebit_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty)
                {
                    dr["debit_ac"] = txtdebit_ac.Text;
                    dr["debitAcName"] = str;
                }
                else
                {
                    lblDebit_name.Text = string.Empty;
                    txtdebit_ac.Text = string.Empty;
                    setFocusControl(txtdebit_ac);
                    return;
                }
            }
            else
            {
                lblDebit_name.Text = string.Empty;
                txtdebit_ac.Text = string.Empty;
                setFocusControl(txtdebit_ac);
                return;
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

            if (drpDrCr.Text != string.Empty)
            {
                dr["DRCR"] = drpDrCr.SelectedValue;
            }
            else
            {
                setFocusControl(drpDrCr);
                return;
            }

            if (txtnarration.Text != string.Empty)
            {
                dr["narration"] = txtnarration.Text;

            }
            else
            {
                dr["narration"] = txtnarration.Text;
                //setFocusControl(txtnarration);
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

                setFocusControl(txtdebit_ac);
            }
            else
            {

                setFocusControl(txtdebit_ac);
                //btnOpenDetailsPopup.Focus();
            }
            // Empty Code->
            txtdebit_ac.Text = string.Empty;
            lblDebit_name.Text = string.Empty;
            lblclosingbal.Text = string.Empty;
            txtamount.Text = string.Empty;
            txtnarration.Text = string.Empty;
            columnTotal();
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

        btnSave.Focus();

    }
    #endregion

    #region [showDetailsRow]
    private void showDetailsRow(GridViewRow gr)
    {
        lblNo.Text = Server.HtmlDecode(gr.Cells[10].Text);
        lblID.Text = Server.HtmlDecode(gr.Cells[2].Text);

        txtdebit_ac.Text = Server.HtmlDecode(gr.Cells[3].Text);
        lblDebit_name.Text = Server.HtmlDecode(gr.Cells[4].Text);
        drpDrCr.Text = Server.HtmlDecode(gr.Cells[5].Text);
        txtamount.Text = Server.HtmlDecode(gr.Cells[6].Text);
        txtnarration.Text = Server.HtmlDecode(gr.Cells[7].Text);
        setFocusControl(txtdebit_ac);



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
                string IDExisting = clsCommon.getString("select detail_id from " + tblDetails + " where trandetailid=" + ID + " and Tran_Type='" + trntype + "' and doc_no=" + txtdoc_no.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (IDExisting != string.Empty)
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[9].Text = "D";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";            //D=Delete from table
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[9].Text = "N";
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
                        grdDetail.Rows[rowIndex].Cells[9].Text = "R";       //R=Only remove fro grid
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "R";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[9].Text = "A";
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
        try
        {
            e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(4);
            e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(5);
            e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(7);
            e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(22);
            e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(10);
            e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(22);
            e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(15);
            e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[9].ControlStyle.Width = Unit.Percentage(20);
            e.Row.Cells[10].ControlStyle.Width = Unit.Percentage(20);

            e.Row.Cells[0].Style["overflow"] = "hidden";
            e.Row.Cells[1].Style["overflow"] = "hidden";
            e.Row.Cells[2].Style["overflow"] = "hidden";
            e.Row.Cells[3].Style["overflow"] = "hidden";
            e.Row.Cells[4].Style["overflow"] = "hidden";
            e.Row.Cells[5].Style["overflow"] = "hidden";
            e.Row.Cells[6].Style["overflow"] = "hidden";
            e.Row.Cells[7].Style["overflow"] = "hidden";
            e.Row.Cells[8].Style["overflow"] = "hidden";
            e.Row.Cells[9].Style["overflow"] = "hidden";
            e.Row.Cells[10].Style["overflow"] = "hidden";

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Left;
            }

            e.Row.Cells[9].Visible = true;
            e.Row.Cells[10].Visible = true;

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
            if (v == "txtnarration")
            {
                e.Row.Cells[0].Width = new Unit("400px");
            }


            if (v != "txtnarration")
            {
                e.Row.Cells[0].Width = new Unit("60px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].Width = new Unit("80px");
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
                        if (grdDetail.Rows[rowindex].Cells[9].Text != "D" && grdDetail.Rows[rowindex].Cells[9].Text != "R")
                        {

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
                        columnTotal();
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
            hdnfClosePopup.Value = "txtEditDoc_No";
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

    #region [txtdebit_ac_TextChanged]
    protected void txtdebit_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtdebit_ac.Text;
        strTextBox = "txtdebit_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtdebit_ac_Click]
    protected void btntxtdebit_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtdebit_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    private void columnTotal()
    {
        #region[total cal]
        double debitamnt_total = 0.00;
        double creditamnt_toatl = 0.00;
        double diff = 0.00;
        double debitamount = 0.00;
        double creditamont = 0.00;
        if (grdDetail.Rows.Count > 0)
        {

            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                //if (grdDetail.Rows[i].Cells[9].Text == "U" || grdDetail.Rows[i].Cells[9].Text == "A")
                //{
                if (grdDetail.Rows[i].Cells[9].Text != "D")
                {

                    if (grdDetail.Rows[i].Cells[5].Text == "D")
                    {
                        debitamount = Convert.ToDouble(grdDetail.Rows[i].Cells[6].Text);
                        debitamnt_total += debitamount;
                    }

                    if (grdDetail.Rows[i].Cells[5].Text == "C")
                    {
                        creditamont = Convert.ToDouble(grdDetail.Rows[i].Cells[6].Text);
                        creditamnt_toatl += creditamont;
                    }

                }

                //Sale_Amnt_Total += Sale_Amnt_Total1;
                diff = debitamnt_total - creditamnt_toatl;
            }
        }

        lbldebittotal.Text = Convert.ToString(Math.Round(debitamnt_total, 2)).ToString();
        lblcredittotal.Text = Convert.ToString(Math.Round(creditamnt_toatl, 2)).ToString();
        lbldiff.Text = Convert.ToString(Math.Round(diff, 2)).ToString();
        #endregion
    }


    #region [txtamount_TextChanged]
    protected void txtamount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtamount.Text;
        strTextBox = "txtamount";
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
            if (hdnfClosePopup.Value == "txtdebit_ac")
            {
                setFocusControl(txtdebit_ac);
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
        columnTotal();
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
                        //lblMsg.Text = "Code " + txtdoc_no.Text + " already exist";
                        //this.getMaxCode();
                        this.NextNumber();
                        // hdnf.Value = txtdoc_no.Text;
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
            string dt = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
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

        if (Convert.ToDouble(lbldiff.Text) != 0.00)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Difference must be zero!');", true);
            setFocusControl(btnAdddetails);
            return;

        }
        else
        {
            isValidated = true;
        }

        int count = 0;
        if (grdDetail.Rows.Count == 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Enter Purchase Details!');", true);
            isValidated = false;
            //setFocusControl(btnOpenDetailsPopup);

            return;
        }
        if (grdDetail.Rows.Count >= 1)
        {
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                if (grdDetail.Rows[i].Cells[9].Text == "D" || grdDetail.Rows[i].Cells[9].Text == "R")
                {
                    count++;
                }
            }
            if (grdDetail.Rows.Count == count)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Enter Purchase Details!');", true);
                isValidated = false;
                // setFocusControl(btnOpenDetailsPopup);
                return;
            }
        }

        #endregion
        #region Assign Values to Variables

        tran_type = trntype;
        doc_date = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        cashbank = 0;
        double totalamount = 0.00;
        double final_total = 0.00;
        if (grdDetail.Rows.Count > 0)
        {

            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                if (grdDetail.Rows[i].Cells[9].Text != "D")
                {

                    totalamount = Convert.ToDouble(grdDetail.Rows[i].Cells[6].Text);
                    final_total += totalamount;

                }
            }
        }
        total = final_total;
        Company_Code = Convert.ToInt32(Convert.ToInt32(Session["Company_Code"].ToString()));
        Year_Code = Convert.ToInt32(Convert.ToInt32(Session["year"].ToString()));
        Branch_Code = Convert.ToInt32(Convert.ToInt32(Session["Branch_Code"].ToString()));
        Created_By = Session["user"].ToString();
        Modified_By = Session["user"].ToString();
        Created_Date = DateTime.Now.ToString("yyyy/MM/dd");
        Modified_Date = DateTime.Now.ToString("yyyy/MM/dd");
        CB_Id = 0;
        retValue = string.Empty;

        #endregion
        #region Detail Fields
        Detail_Fields.Append("Tran_Type,");
        Detail_Fields.Append("doc_no,");
        Detail_Fields.Append("doc_date,");
        Detail_Fields.Append("detail_id,");
        Detail_Fields.Append("debit_ac,");
        Detail_Fields.Append("credit_ac,");
        Detail_Fields.Append("amount,");
        Detail_Fields.Append("drcr,");
        Detail_Fields.Append("narration,");
        Detail_Fields.Append("Company_Code,");
        Detail_Fields.Append("Year_Code,");
        Detail_Fields.Append("Branch_Code,");
        Detail_Fields.Append("Created_By,");
        Detail_Fields.Append("trandetailid,");
        Detail_Fields.Append("tranid,");
        Detail_Fields.Append("da");
        //Detail_Fields.Append( "ca");
        #endregion
        if (btnSave.Text == "Save")
        {
            this.NextNumber();

            #region Head Part
            Head_Fields.Append("tran_type,");
            Head_Values.Append("'" + tran_type + "',");
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
            //Head_Fields.Append( "cb,");
            //Head_Values.Append( "'" + CB_Id + "',");
            Head_Fields.Append("tranid");
            Head_Values.Append("'" + Autoid + "'");


            #endregion End Head assign Field And Values Part
            Head_Insert = "insert into " + tblHead + "(" + Head_Fields + ") values(" + Head_Values + ")";
            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = Head_Insert;
            Maindt.Rows.Add(dr);

            #region Detail Part
            int utrDetail_Id = Convert.ToInt32(clsCommon.getString("select ifnull(count(trandetailid),0) as trandetailid from " + tblDetails + " "));
            if (utrDetail_Id == 0)
            {
                utrDetail_Id = 1;
            }
            else
            {
                utrDetail_Id = Convert.ToInt32(clsCommon.getString("select max(trandetailid) as trandetailid from " + tblDetails + " "));
            }
            #region Create Main Logic Detail
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                utrDetail_Id = utrDetail_Id + 1;
                Grid_Id = Convert.ToInt32(grdDetail.Rows[i].Cells[Detail_Id].Text);
                Dr_Ac_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[debit_ac].Text);
                try
                {
                    DA_Id = Convert.ToInt32(clsCommon.getString("select accoid from " + qryAccountList + " where Ac_Code=" + Dr_Ac_Code + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
                }
                catch
                {
                }
                Cr_Ac_Code = 0;
                CA_Id = 0;
                Grid_amount = Convert.ToDouble(grdDetail.Rows[i].Cells[amount].Text);
                DRCRGrid = grdDetail.Rows[i].Cells[DRCR].Text;
                Narration = grdDetail.Rows[i].Cells[narration].Text;


                Detail_Values.Append("('" + tran_type + "','" + Doc_No + "','" + doc_date + "','" + Grid_Id + "','" + Dr_Ac_Code
                                + "','" + Cr_Ac_Code + "','" + Grid_amount + "','" + DRCRGrid + "','" + Narration + "','" + Company_Code + "','" + Year_Code + "', '" + Branch_Code
                                + "','" + Created_By + "','" + utrDetail_Id + "','" + Autoid + "',case when 0='" + DA_Id + "' then null else '" + DA_Id + "' end),");
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

        }
        else
        {
            Doc_No = Convert.ToInt32(txtdoc_no.Text);
            #region Update Head
            Head_Update.Append("tran_type=");
            Head_Update.Append("'" + tran_type + "',");
            Head_Update.Append("doc_no=");
            Head_Update.Append("'" + Doc_No + "',");
            Head_Update.Append("doc_date=");
            Head_Update.Append("'" + doc_date + "',");
            Head_Update.Append("cashbank=");
            Head_Update.Append("'" + cashbank + "',");
            Head_Update.Append("total=");
            Head_Update.Append("'" + total + "'");
            //Head_Update.Append( "cb=");
            //Head_Update.Append( "'" + CB_Id + "'");

            string Head_Updateqry = "update " + tblHead + " set " + Head_Update + " where tranid =" + lblJV_Id.Text + "";
            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = Head_Updateqry;
            Maindt.Rows.Add(dr);
            #endregion

            string concatid = string.Empty;

            int utrDid = Convert.ToInt32(clsCommon.getString("select max(trandetailid) as trandetailid from " + tblDetails + " "));
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                Grid_Id = Convert.ToInt32(grdDetail.Rows[i].Cells[Detail_Id].Text);
                Dr_Ac_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[debit_ac].Text);
                try
                {
                    DA_Id = Convert.ToInt32(clsCommon.getString("select accoid from " + qryAccountList + " where Ac_Code=" + Dr_Ac_Code + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
                }
                catch
                {
                }
                Cr_Ac_Code = 0;
                CA_Id = 0;
                Grid_amount = Convert.ToDouble(grdDetail.Rows[i].Cells[amount].Text);
                DRCRGrid = grdDetail.Rows[i].Cells[DRCR].Text;
                Narration = grdDetail.Rows[i].Cells[narration].Text;
                id = Convert.ToInt32(grdDetail.Rows[i].Cells[8].Text);

                #region Insert Grid Record
                if (grdDetail.Rows[i].Cells[Rowaction].Text == "A")
                {
                    utrDid = utrDid + 1;
                    Detail_Values.Append("('" + tran_type + "','" + Doc_No + "','" + doc_date + "','" + Grid_Id + "','" + Dr_Ac_Code
                        + "','" + Cr_Ac_Code + "','" + Grid_amount + "','" + DRCRGrid + "','" + Narration + "','" + Company_Code + "','" + Year_Code
                        + "','" + Branch_Code + "','" + Created_By + "'," + " '" + utrDid + "','" + lblJV_Id.Text + "',case when 0='" + DA_Id + "' then null else '" + DA_Id + "' end),");
                }
                #endregion

                #region Update Grid Record
                if (grdDetail.Rows[i].Cells[Rowaction].Text == "U")
                {
                    Detail_Update.Append("doc_date=case trandetailid when '" + id + "' then '" + doc_date + "'  ELSE doc_date END,");
                    Detail_Update.Append("debit_ac=case trandetailid when '" + id + "' then '" + Dr_Ac_Code + "'  ELSE debit_ac END,");
                    Detail_Update.Append("credit_ac=case trandetailid when '" + id + "' then '" + Cr_Ac_Code + "'  ELSE credit_ac END,");
                    Detail_Update.Append("amount=case trandetailid when '" + id + "' then '" + Grid_amount + "'  ELSE amount END,");
                    Detail_Update.Append("drcr=case trandetailid when '" + id + "' then '" + DRCRGrid + "'  ELSE drcr END,");
                    Detail_Update.Append("narration=case trandetailid when '" + id + "' then '" + Narration + "'  ELSE narration END,");
                    Detail_Update.Append("da=case trandetailid when '" + id + "' then case when 0='" + DA_Id + "' then null else '" + DA_Id + "' end  ELSE da END,");
                    //Detail_Update.Append( "ca=case trandetailid when '" + id + "' then '" + CA_Id + "'  ELSE ca END,");


                    concatid = concatid + "'" + id + "',";

                }
                #endregion

                #region Delete Grid Record
                if (grdDetail.Rows[i].Cells[Rowaction].Text == "D")
                {
                    Detail_Delete.Append("'" + grdDetail.Rows[i].Cells[tenderdetailid].Text + "',");
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


        }
        #region Gledger Effect
        FormTypes types = new FormTypes();
        StringBuilder Gledger_values = new StringBuilder();
        GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='JV' and Doc_No=" + Doc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

        dr = null;
        dr = Maindt.NewRow();
        dr["Querys"] = GLEDGER_Delete;
        Maindt.Rows.Add(dr);

        StringBuilder Gledger_Column = new StringBuilder();
        Gledger_Column.Append("TRAN_TYPE,CASHCREDIT,DOC_NO,DOC_DATE,AC_CODE,UNIT_code,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE," +
                    " YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO,ac,vc,progid,tranid");
        int ORDER_CODE = 0;
        for (int i = 0; i < grdDetail.Rows.Count; i++)
        {
            Grid_Id = Convert.ToInt32(grdDetail.Rows[i].Cells[Detail_Id].Text);
            Dr_Ac_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[debit_ac].Text);
            try
            {
                DA_Id = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as id from " + qryAccountList + " where Ac_Code=" + Dr_Ac_Code + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
            }
            catch
            {
            }
            Grid_amount = Convert.ToDouble(grdDetail.Rows[i].Cells[amount].Text);
            DRCRGrid = grdDetail.Rows[i].Cells[DRCR].Text;
            Narration = grdDetail.Rows[i].Cells[narration].Text;
            id = Convert.ToInt32(grdDetail.Rows[i].Cells[8].Text);

            ORDER_CODE = ORDER_CODE + 1;
            Gledger_values.Append("('JV','','" + Doc_No + "','" + doc_date + "','" + Dr_Ac_Code + "','0','" + Narration + "','" + Grid_amount + "', " +
                                                    " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + ORDER_CODE + "','" + DRCRGrid + "',0,0,'" + Branch_Code + "','JV','" + Doc_No + "'," +
                                                    " case when 0='" + DA_Id + "' then null else '" + DA_Id + "' end,'0','" + types.TT_JV + "','0'),");
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
        msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);
        // msg = clsDAL.DataStore(Head_Insert, Head_Update, Head_Delete, Detail_Insert, Detail_Update, Detail_Delete, GLEDGER_Insert, GLEDGER_Delete, flag);
        if (msg == "Insert")
        {
            hdnf.Value = Autoid.ToString();
            clsButtonNavigation.enableDisable("S");
            this.makeEmptyForm("S");
            qry = getDisplayQuery();
            this.fetchRecord(qry);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Added !')", true);
        }
        else if (msg == "Update")
        {
            hdnf.Value = lblJV_Id.Text;
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
                                                // hdnf.Value = txtdoc_no.Text;
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
                    string dt = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    if (clsCommon.isValidDate(dt) == true)
                    {
                        setFocusControl(txtdebit_ac);
                    }
                    else
                    {
                        txtdoc_date.Text = string.Empty;
                        setFocusControl(txtdoc_date);
                    }
                }
                else
                {
                    setFocusControl(txtdoc_date);
                }
            }


            if (strTextBox == "txtamount")
            {
                setFocusControl(txtnarration);
            }
            if (strTextBox == "txtnarration")
            {
                setFocusControl(btnAdddetails);
            }
            if (strTextBox == "txtdebit_ac")
            {
                if (txtdebit_ac.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtdebit_ac.Text);
                    if (a == false)
                    {
                        btntxtdebit_ac_Click(this, new EventArgs());
                    }
                    else
                    {
                        string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtdebit_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (str != string.Empty && str != "0")
                        {
                            lblDebit_name.Text = str;
                            string cloasebal = clsCommon.getString("select SUM(case drcr when 'D' then AMOUNT when 'C' then -amount end) as Balance from " + tblPrefix
                                + "GLEDGER where Ac_Code=" + txtdebit_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            if (cloasebal != string.Empty)
                            {
                                lblclosingbal.Text = cloasebal;
                            }
                            else
                            {
                                lblclosingbal.Text = "";
                            }
                            setFocusControl(drpDrCr);
                        }
                        else
                        {
                            lblDebit_name.Text = string.Empty;
                            txtdebit_ac.Text = string.Empty;
                            setFocusControl(txtdebit_ac);

                        }
                    }
                }
                else
                {
                    lblDebit_name.Text = string.Empty;
                    txtdebit_ac.Text = string.Empty;
                    setFocusControl(txtdebit_ac);

                }
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
            if (searchString != string.Empty && strTextBox == hdnfClosePopup.Value)
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
                    string qry = "select distinct(doc_no),doc_date,debitAcName,creditAcName from " + qryCommon
                        + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())
                        + " and Tran_Type='" + trntype + "'" +
                    " and  (doc_no like '%" + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%' or debitAcName like '%" + txtSearchText.Text + "%' or creditAcName like '%" + txtSearchText.Text + "%')  group by doc_no,doc_date,debitAcName,creditAcName order by doc_no";
                    this.showPopup(qry);
                }
            }
            if (hdnfClosePopup.Value == "txtdebit_ac")
            {
                lblPopupHead.Text = "--Select debit AC--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " +
                    " and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName  like '%" + txtSearchText.Text + "%' )";
                this.showPopup(qry);
            }


            if (hdnfClosePopup.Value == "txtnarration")
            {
                lblPopupHead.Text = "--Select Narration--";
                //string qry = "select System_Name_E as Narration from " + systemMasterTable + " where System_Type='G' and Company_Code="
                //    + Convert.ToInt32(Session["Company_Code"].ToString());
                string qry = "select System_Name_E from " + systemMasterTable + " where  System_Type='G' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " +
                   " and (System_Name_E like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }

        }
        catch
        {
        }
    }
    #endregion

    #region [drpDrCr_SelectedIndexChanged]
    protected void drpDrCr_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = drpDrCr.SelectedValue;
            strTextBox = "drpDrCr";
            csCalculations();
        }
        catch
        {

        }
    }
    #endregion
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
    }



    protected void txtEditDoc_No_TextChanged(object sender, EventArgs e)
    {
        try
        {
            bool a = clsCommon.isStringIsNumeric(txtEditDoc_No.Text);

            if (a == false)
            {
                txtSearchText.Text = txtEditDoc_No.Text;
                btntxtdoc_no_Click(this, new EventArgs());
            }
            else
            {
                hdnf.Value = txtEditDoc_No.Text;
                string qry = "select * from " + qryCommon + " where doc_no=" + hdnf.Value + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='" + trntype + "'";
                this.fetchRecord(qry);
                setFocusControl(txtEditDoc_No);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    #region Generate Next Number
    private void NextNumber()
    {
        try
        {
            int counts = 0;
            counts = Convert.ToInt32(clsCommon.getString("select count(doc_no) as doc_no from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                " and Year_Code='" + Session["year"].ToString() + "' and Tran_Type='JV'"));
            if (counts == 0)
            {
                txtdoc_no.Text = "1";
                Doc_No = 1;
            }
            else
            {
                Doc_No = Convert.ToInt32(clsCommon.getString("SELECT max(doc_no) as doc_no from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                " and Year_Code='" + Session["year"].ToString() + "' and Tran_Type='JV'")) + 1;
                txtdoc_no.Text = Doc_No.ToString();
            }

            counts = Convert.ToInt32(clsCommon.getString("SELECT count(tranid) as tranid from " + tblHead + " "));
            if (counts == 0)
            {
                Autoid = 1;
                lblJV_Id.Text = "1";

            }
            else
            {
                Autoid = Convert.ToInt32(clsCommon.getString("SELECT max(tranid) as tranid from " + tblHead)) + 1;
                lblJV_Id.Text = Autoid.ToString();
            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Next Record Error!')", true);
        }
    }
    #endregion

}