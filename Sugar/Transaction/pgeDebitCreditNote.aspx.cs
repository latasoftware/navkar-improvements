using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Threading;
using System.Text;
public partial class pgeDebitCreditNote : System.Web.UI.Page
{
    #region data section
    string qry = string.Empty;
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string qryCommon = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string sub_type = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    static WebControl objAsp = null;
    int Detail_Id = 2;
    int Ac_Code = 3;
    int AC_codeName = 4;
    //  int value = 5;
    int Rowaction = 7;
    //   int Srno = 7;
    int dcDetail_id = 8;

    string fornotsaverecord;
    #endregion

    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;

    string cs = string.Empty;
    string Action = string.Empty;
    //int Tender_No = 0;
    int id = 0;
    int flag = 0;
    int count = 0;
    MySqlConnection con = null;
    MySqlCommand cmd = null;
    MySqlTransaction myTran = null;
    string GLEDGER_Insert = string.Empty;
    string GLEDGER_Delete = string.Empty;
    string msg = string.Empty;


    #region Head Declaration Field
    Int32 Doc_No = 0;
    Int32 Entry_no = 0;
    Int32 EntryId = 0;
    string type = string.Empty;
    string Entry_date = string.Empty;
    Int32 Ac_code = 0;
    string Ac_name = string.Empty;
    Int32 Bill_No = 0;
    string Bill_date = string.Empty;
    Int32 Bill_Id = 0;
    Int32 Gst_Code = 0;
    string GstName = string.Empty;
    string Ac_Id = string.Empty;
    float Gst_Rate = 0;
    double TaxableAmount = 0.00;
    double CgstRate = 0.00;
    double CgstAmount = 0.00;
    double SgstRate = 0.00;
    double SgstAmount = 0.00;
    double IgstRate = 0.00;
    double IgstAmount = 0.00;
    double MiscAmount = 0.00;
    double FinalAmount = 0.00;

    string Head_Insert = string.Empty;
    StringBuilder Head_Update = null;
    string Head_Delete = string.Empty;
    StringBuilder Head_Fields = null;
    StringBuilder Head_Values = null;
    #endregion
    #region Detail Declare Field
    string Detail_Insert = string.Empty;
    StringBuilder Detail_Update = null;
    StringBuilder Detail_Delete = null;
    StringBuilder Detail_Fields = null;
    StringBuilder Detail_Values = null;
    Int32 ExpAccode = 0;
    Int32 DetailId = 0;
    string Expname = string.Empty;
    double value = 0;


    #endregion
    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = "debitnotehead";
            tblDetails = "debitnotedetail";
            qryCommon = "qrydebitnoteheaddetail";
            user = Session["user"].ToString();
            pnlPopup.Style["display"] = "none";
            sub_type = drpSub_Type.SelectedValue;
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
                        hdnf.Value = Request.QueryString["dcid"];
                        pnlPopup.Style["display"] = "none";
                        ViewState["currentTable"] = null;
                        clsButtonNavigation.enableDisable("N");
                        pnlgrdDetail.Enabled = true;
                        this.makeEmptyForm("N");
                        ViewState["mode"] = "I";
                        //this.DebitCreditDiff();
                        this.showLastRecord();
                        setFocusControl(drpSub_Type);
                    }
                    else
                    {
                        string docno = string.Empty;
                        clsButtonNavigation.enableDisable("A");
                        ViewState["mode"] = null;
                        ViewState["mode"] = "I";
                        this.makeEmptyForm("A");
                        this.NextNumber();
                        setFocusControl(txtDoc_Date);
                    }

                }
                else
                {
                    Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
                }
            }
            //if (objAsp != null)
            //System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(objAsp);

            if (hdnfClosePopup.Value == "Close" || hdnfClosePopup.Value == "")
            {
                pnlPopup.Style["display"] = "none";
            }
            else
            {
                pnlPopup.Style["display"] = "block";
                objAsp = btnSearch;
                //if (objAsp != null)
            }
        }
        catch
        {
            //  Response.Redirect("http://localhost:3994/HomePage/pgeloginForm.aspx");
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
                obj.tableName = tblHead + " where ";
                obj.code = "Doc_no";
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
                                    txtDoc_No.Text = ds.Tables[0].Rows[0][0].ToString();
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
                ViewState["currentTable"] = null;
                btnAdddetails.Enabled = false;
                btnClosedetails.Enabled = false;
                btnOpenDetailsPopup.Enabled = false;
                pnlgrdDetail.Enabled = false;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                btnSave.Text = "Save";
                btntxtDoc_No.Text = "Choose No";
                btntxtDoc_No.Enabled = false;
                txtEditDoc_No.Enabled = true;
                lblMsg.Text = string.Empty;

                txtDoc_Date.Enabled = false;

                txtRef_Date.Enabled = false;

                txtAc_Code.Enabled = false;
                btntxtAc_Code.Enabled = false;

                txtTaxable_Amount.Enabled = false;
                txtGST_Rate_Code.Enabled = false;
                btntxtGST_Rate_Code.Enabled = false;
                txtGST.Enabled = false;
                txtGross_Value.Enabled = false;

                txtCGST_Rate.Enabled = false;
                txtCGST_Amount.Enabled = false;
                txtSGST_Rate.Enabled = false;
                txtSGST_Amount.Enabled = false;
                txtIGST_Rate.Enabled = false;
                txtIGST_Amount.Enabled = false;

                //txtTran_Type.Enabled = false;
                drpSub_Type.Enabled = true;
                // txtSub_Type.Enabled = false;
                txtAc_Code.Enabled = false;
                btntxtAc_CodeDetails.Enabled = false;
                txtvalue.Enabled = false;
                ViewState["currentTable"] = null;
                txtCGST_Rate.Text = string.Empty;
                txtCGST_Amount.Text = string.Empty;
                txtSGST_Rate.Text = string.Empty;
                txtSGST_Amount.Text = string.Empty;
                txtIGST_Rate.Text = string.Empty;
                txtIGST_Amount.Text = string.Empty;

                txtGross_Value.Text = string.Empty;
                txtGST_Rate_Code.Text = string.Empty;
                lblGST_Rate_Code.Text = string.Empty;
                txtGST.Text = string.Empty;

                txtTaxable_Amount.Text = string.Empty;


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
                ViewState["currentTable"] = null;
                pnlgrdDetail.Enabled = true;
                btntxtDoc_No.Text = "Change No";
                btntxtDoc_No.Enabled = true;
                txtEditDoc_No.Enabled = false;
                txtDoc_No.Enabled = false;
                btnAdddetails.Enabled = true;
                btnClosedetails.Enabled = true;
                grdDetail.DataSource = null; grdDetail.DataBind();
                txtDoc_Date.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                txtRef_Date.Text = System.DateTime.Now.ToString("dd/MM/yyyy"); ;
                //  txtSub_Type.Enabled = true;
                txtAc_Code.Enabled = true;
                lblAc_CodeDetails.Text = string.Empty;
                btntxtAc_CodeDetails.Enabled = true;
                txtvalue.Enabled = true;

                txtDoc_Date.Enabled = true;



                txtAc_Code.Enabled = true;
                lblAc_code.Text = string.Empty;
                btntxtAc_Code.Enabled = true;

                txtTaxable_Amount.Enabled = true;
                txtGST_Rate_Code.Enabled = true;
                lblGST_Rate_Code.Text = string.Empty;
                btntxtGST_Rate_Code.Enabled = true;
                txtGST.Enabled = true;
                txtGross_Value.Enabled = true;

                txtCGST_Rate.Enabled = true;
                txtCGST_Amount.Enabled = true;
                txtSGST_Rate.Enabled = true;
                txtSGST_Amount.Enabled = true;
                txtIGST_Rate.Enabled = true;
                txtIGST_Amount.Enabled = true;

                //txtTran_Type.Enabled = true;
                drpSub_Type.Enabled = false;
                ViewState["currentTable"] = null;

                txtTaxable_Amount.Text = string.Empty;
                txtCGST_Rate.Text = string.Empty;
                txtCGST_Amount.Text = string.Empty;
                txtSGST_Rate.Text = string.Empty;
                txtSGST_Amount.Text = string.Empty;
                txtIGST_Rate.Text = string.Empty;
                txtIGST_Amount.Text = string.Empty;

                txtGross_Value.Text = string.Empty;
                txtGST_Rate_Code.Text = string.Empty;
                lblGST_Rate_Code.Text = string.Empty;
                txtGST.Text = string.Empty;
                drpSub_Type.Text = hdnfTran_type.Value;
                // lblHSN_Code.Text = string.Empty;

                #region set Business logic for save
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
                btntxtDoc_No.Text = "Choose No";
                pnlgrdDetail.Enabled = false;
                btntxtDoc_No.Enabled = false;
                txtEditDoc_No.Enabled = true;
                lblMsg.Text = string.Empty;

                txtDoc_Date.Enabled = false;

                txtRef_Date.Enabled = false;

                txtAc_Code.Enabled = false;
                btntxtAc_Code.Enabled = false;

                txtTaxable_Amount.Enabled = false;
                txtGST_Rate_Code.Enabled = false;
                btntxtGST_Rate_Code.Enabled = false;
                txtGST.Enabled = false;
                txtGross_Value.Enabled = false;

                txtCGST_Rate.Enabled = false;
                txtCGST_Amount.Enabled = false;
                txtSGST_Rate.Enabled = false;
                txtSGST_Amount.Enabled = false;
                txtIGST_Rate.Enabled = false;
                txtIGST_Amount.Enabled = false;

                drpSub_Type.Enabled = true;
                // txtSub_Type.Enabled = false;
                txtAc_CodeDetails.Enabled = false;
                btntxtAc_CodeDetails.Enabled = false;
                txtvalue.Enabled = false;
                btnAdddetails.Enabled = false;
                btnClosedetails.Enabled = false;

                btnAdddetails.Text = "ADD";
                txtAc_CodeDetails.Text = string.Empty;
                lblAc_CodeDetails.Text = string.Empty;
                txtvalue.Text = string.Empty;
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
                btntxtDoc_No.Text = "Choose No";
                btntxtDoc_No.Enabled = true;
                //txtDoc_No.Enabled = true;
                txtDoc_No.Enabled = true;
                txtEditDoc_No.Enabled = false;
                pnlgrdDetail.Enabled = true;
                lblMsg.Text = string.Empty;

                txtDoc_Date.Enabled = true;

                txtRef_Date.Enabled = true;

                txtAc_Code.Enabled = true;
                btntxtAc_Code.Enabled = true;

                txtTaxable_Amount.Enabled = true;
                txtGST_Rate_Code.Enabled = true;
                btntxtGST_Rate_Code.Enabled = true;
                txtGST.Enabled = true;
                txtGross_Value.Enabled = true;

                txtCGST_Rate.Enabled = true;
                txtCGST_Amount.Enabled = true;
                txtSGST_Rate.Enabled = true;
                txtSGST_Amount.Enabled = true;
                txtIGST_Rate.Enabled = true;
                txtIGST_Amount.Enabled = true;

                // txtTran_Type.Enabled = true;
                drpSub_Type.Enabled = false;
                //  txtSub_Type.Enabled = true;
                txtAc_CodeDetails.Enabled = true;
                btntxtAc_CodeDetails.Enabled = true;
                txtvalue.Enabled = true;
                btnAdddetails.Enabled = true;
                btnClosedetails.Enabled = true;

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




    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("A");
        //this.getMaxCode();
        hdnfTran_type.Value = drpSub_Type.SelectedValue;
        this.NextNumber();
        setFocusControl(txtDoc_Date);

    }
    #endregion
    #region Generate Next Number
    private void NextNumber()
    {
        try
        {
            int counts = 0;
            counts = Convert.ToInt32(clsCommon.getString("select count(doc_no) as A from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' and Year_Code='" + Session["Year"].ToString() + "' and tran_type='" + hdnfTran_type.Value + "'"));
            if (counts == 0)
            {

                txtDoc_No.Text = "1";
                Doc_No = 1;
            }
            else
            {
                Doc_No = Convert.ToInt32(clsCommon.getString("SELECT max(doc_no) as Doc_No from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "'and Year_Code='" + Session["Year"].ToString() + "' and tran_type='" + hdnfTran_type.Value + "'")) + 1;
                txtDoc_No.Text = Doc_No.ToString();
            }

            counts = Convert.ToInt32(clsCommon.getString("SELECT count(dcid) as bsid from " + tblHead + ""));
            if (counts == 0)
            {
                lblDoc_No.Text = "1";
                Entry_no = 1;
            }
            else
            {
                Entry_no = Convert.ToInt32(clsCommon.getString("SELECT max(dcid) as bsid from " + tblHead)) + 1;
                lblDoc_No.Text = Entry_no.ToString();
            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Next Record Error!')", true);
        }
    }
    #endregion

    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        hdnf.Value = lblDoc_No.Text;
        clsButtonNavigation.enableDisable("E");

        //pnlgrdDetail.Enabled = true;

        //string qry = getDisplayQuery();
        //fetchRecord(qry);
        this.makeEmptyForm("E");
        txtDoc_No.Enabled = false;
        // setFocusControl(txtDoc_No);
    }
    #endregion
    // private string getDisplayQuery()
    //{
    //    try
    //    {
    //        string qryDisplay = " select * from " + qryCommon + " where Doc_No=" + hdnf.Value + " and Company_Code='"
    //            + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and Sub_Type='" + sub_type + "' and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
    //        return qryDisplay;
    //    }
    //    catch
    //    {
    //        return "";
    //    }
    //}
    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                string tran_type = drpSub_Type.SelectedValue;
                Head_Delete = "delete from " + tblHead + " where dcid='" + lblDoc_No.Text + "' and tran_type='" + tran_type + "' ";
                string Detail_Deleteqry = "delete from " + tblDetails + " where dcid='" + lblDoc_No.Text + "' and tran_type='" + tran_type + "'";
                GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='" + tran_type + "' and Doc_No=" + txtDoc_No.Text + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " +
                    " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

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
                    Response.Redirect("../Transaction/PgeDebitCreditNoteUtility.aspx");
                }
                #region

                #endregion
            }
            else
            {
                hdnf.Value = lblDoc_No.Text;
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
        string max = clsCommon.getString("select max(dcid) as id from debitnotehead where Company_Code=" + Session["Company_Code"].ToString() + " " +
           " and Year_Code=" + Session["year"].ToString() + " and tran_type='" + drpSub_Type.SelectedValue + "'");
        hdnf.Value = max;
        hdnfTran_type.Value = drpSub_Type.SelectedValue;
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
                        Label lblCreatedDate = (Label)Master.FindControl("MasterlblCreatedDate");
                        Label lblModifiedDate = (Label)Master.FindControl("MasterlblModifiedDate");
                        if (lblCreatedDate != null)
                        {
                            if (dt.Rows[0]["Created_Date"].ToString() == string.Empty)
                            {
                                lblModifiedDate.Text = "";
                            }
                            else
                            {
                                lblCreatedDate.Text = "Created Date" + dt.Rows[0]["Created_Date"].ToString();
                            }
                        }
                        if (lblModifiedDate != null)
                        {
                            if (dt.Rows[0]["Modified_Date"].ToString() == string.Empty)
                            {
                                lblModifiedDate.Text = "";
                            }
                            else
                            {
                                lblModifiedDate.Text = "Modified Date" + dt.Rows[0]["Modified_Date"].ToString();
                            }
                        }
                        //txtEditDoc_No.Text = dt.Rows[0]["EditChange_No"].ToString();
                        txtDoc_No.Text = dt.Rows[0]["doc_no"].ToString();
                        lblDoc_No.Text = hdnf.Value;
                        txtRef_Date.Text = dt.Rows[0]["bill_dateConverted"].ToString();
                        txtDoc_Date.Text = dt.Rows[0]["doc_dateConverted"].ToString();
                        txtBillNo.Text = dt.Rows[0]["bill_no"].ToString();

                        txtDoc_Date.Text = dt.Rows[0]["doc_dateConverted"].ToString();
                        // lblRef_Unique_Id.Text = dt.Rows[0][""].ToString();
                        txtAc_Code.Text = dt.Rows[0]["ac_code"].ToString();
                        lblAc_code.Text = dt.Rows[0]["Ac_Name_E"].ToString();

                        txtTaxable_Amount.Text = dt.Rows[0]["texable_amount"].ToString();
                        txtGST_Rate_Code.Text = dt.Rows[0]["gst_code"].ToString();
                        lblGST_Rate_Code.Text = dt.Rows[0]["GST_Name"].ToString();


                        //lblHSN_Code.Text = dt.Rows[0]["HSN_Code"].ToString();
                        txtCGST_Rate.Text = dt.Rows[0]["cgst_rate"].ToString();
                        txtCGST_Amount.Text = dt.Rows[0]["cgst_amount"].ToString();
                        txtSGST_Rate.Text = dt.Rows[0]["sgst_rate"].ToString();
                        txtSGST_Amount.Text = dt.Rows[0]["sgst_amount"].ToString();
                        txtIGST_Rate.Text = dt.Rows[0]["igst_rate"].ToString();
                        txtIGST_Amount.Text = dt.Rows[0]["igst_amount"].ToString();
                        txtfinalAmount.Text = dt.Rows[0]["bill_amount"].ToString();
                        // drpSub_Type.Text = dt.Rows[0]["tran_type"].ToString();
                        drpSub_Type.SelectedValue = dt.Rows[0]["tran_type"].ToString();

                        if (drpSub_Type.SelectedValue == "DN" || drpSub_Type.SelectedValue == "CN" || drpSub_Type.SelectedValue == "D1" || drpSub_Type.SelectedValue == "C2")
                        {
                            //  string cust = "Customer";
                            //  lnkAcCode.Text = "Customer";
                            // lblAcCodeCpation.Text = cust.ToString();

                        }
                        else
                        {
                            //  string cust1 = "Supplier";
                            //lnkAcCode.Text = "Supplier";
                            // lblAcCodeCpation.Text = cust1.ToString();
                        }

                        //         lblRefYear.Text = "Year: " + dt.Rows[0]["RefYear_Code"].ToString();
                        if (drpSub_Type.SelectedValue == "C1")
                        {
                            //lblbilltrandocno.Text = dt.Rows[0]["Biil_TranType_DocNO"].ToString();
                        }
                        recordExist = true;
                        lblMsg.Text = "";
                        #region Details
                        qry = "";
                        //ds = clsDAL.SimpleQuery("select Detail_Id,HeadSub_Type,HeadAc_Code,HeadAc_Name_E,Amount from qryDebitCreditNote where Company_Code='"
                        //    + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString())
                        //    + "' and Sub_Type='" + sub_type + "'and Doc_No =" + txtDoc_No.Text);
                        ds = clsDAL.SimpleQuery("select  detail_Id,expac_code as ExAc_Code,expacaccountname as HeadAc_Name_E,value as Amount,dcdetailid as AutoIDetailId from qrydebitnoteheaddetail where Detail_ID is not null and Company_Code='"
                           + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString())
                           + "' and tran_type='" + hdnfTran_type.Value + "'and dcid=" + hdnf.Value + "");
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
                        csCalculations();
                        pnlgrdDetail.Enabled = false;
                        // hdnf.Value = lblDoc_No.Text;
                    }
                    else
                    {
                        grdDetail.DataSource = null;
                        grdDetail.DataBind();
                        ViewState["currentTable"] = null;
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
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = " select * from " + qryCommon + " where dcid=" + hdnf.Value + "  and tran_type='" + hdnfTran_type.Value + "' ";
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
                txtDoc_No.Text = hdnf.Value;
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
    }
    #endregion

    #region [btnAdddetails_Click]
    protected void btnAdddetails_Click(object sender, EventArgs e)
    {
        double amount;
        double.TryParse(txtvalue.Text, out amount);

        if (amount == 0 && amount != 0.0)
        {

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please enter correct amount! ');", true);

            setFocusControl(txtvalue);
            return;
        }
        int AcCode;
        Int32.TryParse(txtAc_CodeDetails.Text, out AcCode);
        if (AcCode == 0)
        {
            // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please enter correct amount!);", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please enter correct Ac Code! ');", true);

            setFocusControl(txtAc_CodeDetails);
            return;
        }

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
                if (dt.Rows[0]["Detail_Id"].ToString().Trim() != "")
                {
                    if (btnAdddetails.Text == "ADD")
                    {
                        dr = dt.NewRow();
                        #region calculate rowindex
                        int maxIndex = 0;
                        int[] index = new int[dt.Rows.Count];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            index[i] = Convert.ToInt32(dt.Rows[i]["Detail_Id"].ToString());
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
                        dr["Detail_Id"] = rowIndex;     //auto
                        dr["rowAction"] = "A";
                        dr["SrNo"] = 0;
                    }
                    else
                    {
                        //update row 

                        int n = Convert.ToInt32(lblID.Text);
                        rowIndex = Convert.ToInt32(lblID.Text);   //auto no
                        dr = (DataRow)dt.Rows[n - 1];
                        dr["Detail_Id"] = rowIndex;
                        dr["SrNo"] = 0;
                        #region decide whether actual row is updating or virtual [rowAction]

                        string id = clsCommon.getString("select detail_Id from " + tblDetails + " where doc_no='" + txtDoc_No.Text + "' and detail_Id='" + rowIndex + "' " +
                            " and Company_Code=" + Session["Company_Code"].ToString() + " and  Year_Code=" + Session["year"].ToString() + " and tran_type='" + drpSub_Type.SelectedValue + "'");
                        //  string id = clsCommon.getString("select Detail_Id from " + tblDetails + " where Detail_Id='" + lblID.Text + "' and Detail_Id=" + lblID.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
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
                    dt.Columns.Add((new DataColumn("Detail_Id", typeof(int))));
                    #region [Write here columns]
                    // dt.Columns.Add((new DataColumn("HeadSub_Type", typeof(string))));
                    dt.Columns.Add((new DataColumn("ExAc_Code", typeof(int))));
                    dt.Columns.Add((new DataColumn("AutoIDetailId", typeof(int))));
                    dt.Columns.Add((new DataColumn("HeadAc_Name_E", typeof(string))));
                    dt.Columns.Add((new DataColumn("Amount", typeof(double))));
                    #endregion
                    dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                    dt.Columns.Add((new DataColumn("SrNo", typeof(int))));

                    dr = dt.NewRow();
                    dr["Detail_Id"] = rowIndex;
                    dr["rowAction"] = "A";
                    dr["SrNo"] = 0;
                }
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add((new DataColumn("Detail_Id", typeof(int))));
                #region [Write here columns]
                //  dt.Columns.Add((new DataColumn("HeadSub_Type", typeof(string))));
                dt.Columns.Add((new DataColumn("ExAc_Code", typeof(int))));
                dt.Columns.Add((new DataColumn("HeadAc_Name_E", typeof(string))));
                dt.Columns.Add((new DataColumn("Amount", typeof(double))));
                dt.Columns.Add((new DataColumn("AutoIDetailId", typeof(int))));
                #endregion
                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                dt.Columns.Add((new DataColumn("SrNo", typeof(int))));

                dr = dt.NewRow();
                dr["Detail_Id"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;
            }
            #region [ Set values to dr]

            if (txtAc_CodeDetails.Text != string.Empty)
            {
                dr["ExAc_Code"] = txtAc_CodeDetails.Text;
            }
            else
            {
                setFocusControl(txtAc_Code);
                return;
            }
            dr["HeadAc_Name_E"] = lblAc_CodeDetails.Text;
            if (txtvalue.Text != string.Empty)
            {
                dr["Amount"] = txtvalue.Text;
            }
            else
            {
                dr["Amount"] = 0.00;


            }
            // dr["AutoIDetailId"] = lblExAcId.Text;

            if (btnAdddetails.Text == "ADD")
            {
                dr["AutoIDetailId"] = 0;
                dt.Rows.Add(dr);
            }
            else
            {

            }
            #endregion
            //if (btnAdddetails.Text == "ADD")
            //{
            //    dt.Rows.Add(dr);
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
                pnlPopupDetails.Style["display"] = "none";
                setFocusControl(txtAc_CodeDetails);
            }
            else
            {
                pnlPopupDetails.Style["display"] = "none";
                setFocusControl(btnAdddetails);
                btnOpenDetailsPopup.Focus();
            }
            // Empty Code->
            txtAc_CodeDetails.Text = string.Empty;
            lblAc_CodeDetails.Text = string.Empty;
            //txtAc_Code.Text = string.Empty;
            txtvalue.Text = string.Empty;
            btnAdddetails.Text = "ADD";
            csCalculations();
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

        txtAc_CodeDetails.Text = string.Empty;
        txtvalue.Text = string.Empty;
        btnAdddetails.Text = "ADD";
        setFocusControl(txtAc_CodeDetails);
        pnlPopupDetails.Style["display"] = "none";
    }
    #endregion

    #region [showDetailsRow]
    private void showDetailsRow(GridViewRow gvrow)
    {
        lblNo.Text = Server.HtmlDecode(gvrow.Cells[7].Text);//srno row id;
        lblID.Text = Server.HtmlDecode(gvrow.Cells[2].Text);//Detail id;

        txtAc_CodeDetails.Text = Server.HtmlDecode(gvrow.Cells[Ac_Code].Text);
        lblAc_CodeDetails.Text = Server.HtmlDecode(gvrow.Cells[4].Text);
        txtvalue.Text = Server.HtmlDecode(gvrow.Cells[5].Text);
        lblExAcId.Text = Server.HtmlDecode(gvrow.Cells[6].Text);
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
                int ID = Convert.ToInt32(dt.Rows[rowIndex]["Detail_Id"].ToString());
                string IDExisting = clsCommon.getString("select dcdetailid from " + tblDetails + " where Doc_No='"
                  + txtDoc_No.Text + "' and tran_type='" + sub_type + "'");
                if (IDExisting != string.Empty)
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[Rowaction].Text = "D";// rowAction Index add 
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";            //D=Delete from table
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[Rowaction].Text = "N";// Add rowaction id
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
                        grdDetail.Rows[rowIndex].Cells[Rowaction].Text = "R";       // add row action R=Only remove fro grid
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "R";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[Rowaction].Text = "A";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "A";
                    }
                }
                ViewState["currentTable"] = dt;
            }
            // csCalculations();
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
            //  if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //e.Row.Cells[0].Width = new Unit("120px");
            //e.Row.Cells[1].Width = new Unit("120px");
            //e.Row.Cells[Detail_Id].Width = new Unit("120px");
            //e.Row.Cells[Ac_Code].Width = new Unit("120px");
            //e.Row.Cells[4].Width = new Unit("120px");
            //e.Row.Cells[Amount].Width = new Unit("120px");
            //e.Row.Cells[2].Width = new Unit("120px");
            //e.Row.Cells[2].Width = new Unit("120px");
            //e.Row.Cells[2].Width = new Unit("120px");
            e.Row.Cells[0].ControlStyle.Width = new Unit("50px");
            e.Row.Cells[1].ControlStyle.Width = new Unit("50px");
            e.Row.Cells[2].ControlStyle.Width = new Unit("50px");
            e.Row.Cells[Ac_Code].ControlStyle.Width = new Unit("100px");
            e.Row.Cells[4].ControlStyle.Width = new Unit("300px");
            e.Row.Cells[5].ControlStyle.Width = new Unit("150px");
            //e.Row.Cells[Rowaction].ControlStyle.Width = new Unit("20px");
            e.Row.Cells[6].ControlStyle.Width = new Unit("50px");
            //e.row.cells[6].visible = false;
            e.Row.Cells[7].Visible = true;
            // e.row.cells[8].visible = fal

            e.Row.Cells[8].Visible = true;

            e.Row.Cells[AC_codeName].HorizontalAlign = HorizontalAlign.Right;
            //    e.Row.Cells[0].Style["overflow" ] = "hiden";
            //    e.Row.Cells[0].Visible =true;
            // }
            e.Row.Cells[Ac_Code].Visible = true;
            //e.Row.Cells[Rowaction].Visible = false;
            //e.Row.Cells[Srno].Visible = false;

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
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

        }
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            if (v == "txtDoc_No")
            {
                e.Row.Cells[0].ControlStyle.Width = new Unit("30px");
                //e.Row.Cells[1].ControlStyle.Width = new Unit("100px");
                //e.Row.Cells[2].ControlStyle.Width = new Unit("100px");
                //e.Row.Cells[3].ControlStyle.Width = new Unit("100px");

            }
            if (v == "txtBranch_Code" || v == "txtAc_Code" || v == "txtAc_CodeDetails"
                || v == "txtGST_Rate_code")
            {
                e.Row.Cells[0].ControlStyle.Width = new Unit("30px");
                e.Row.Cells[1].ControlStyle.Width = new Unit("200px");
                e.Row.Cells[2].ControlStyle.Width = new Unit("100px");
            }
            if (v == "txtBusiness_Code" || v == "txtHSN")
            {
                e.Row.Cells[0].ControlStyle.Width = new Unit("100px");
                e.Row.Cells[1].ControlStyle.Width = new Unit("150px");


            }
        }
    }
    #endregion

    #region [grdPopup_PageIndexChanging]
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
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
                        if (grdDetail.Rows[rowindex].Cells[Rowaction].Text != "D" && grdDetail.Rows[rowindex].Cells[Rowaction].Text != "R")//add row action id
                        {
                            pnlPopupDetails.Style["display"] = "none";
                            this.showDetailsRow(grdDetail.Rows[rowindex]);
                            btnAdddetails.Text = "UPDATE";
                            setFocusControl(txtAc_CodeDetails);
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
                        csCalculations();
                        break;
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [txtEditDoc_No_TextChanged]
    protected void txtEditDoc_No_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtEditDoc_No.Text;
        //strTextBox = "txtEditDoc_No";
        //csCalculations();

        try
        {
            bool a = clsCommon.isStringIsNumeric(txtEditDoc_No.Text);
            if (a == false)
            {
                searchString = txtEditDoc_No.Text;
                strTextBox = "txtEditDoc_No";
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtEditDoc_No";
                btnSearch_Click(this, new EventArgs());
            }
            else
            {
                hdnf.Value = txtEditDoc_No.Text;
                // string qry1 = getDisplayQuery();

                setFocusControl(txtEditDoc_No);
            }
        }
        catch (Exception)
        {
            throw;
        }

    }
    #endregion

    #region [txtDoc_No_TextChanged]
    protected void txtDoc_No_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtDoc_No.Text;
        //strTextBox = "txtDoc_No";
        //csCalculations();
        #region code
        try
        {
            int n;
            bool isNumeric = int.TryParse(txtDoc_No.Text, out n);

            if (isNumeric == true)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string txtValue = "";
                if (txtDoc_No.Text != string.Empty)
                {
                    txtValue = txtDoc_No.Text;

                    string qry = "select * from " + tblHead + " where  Doc_No='" + txtValue + "' " +
                        "  and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'";

                    ds = clsDAL.SimpleQuery(qry);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                //Record Found
                                hdnf.Value = dt.Rows[0]["Doc_No"].ToString();

                                if (ViewState["mode"] != null)
                                {
                                    if (ViewState["mode"].ToString() == "I")
                                    {
                                        lblMsg.Text = "** Doc No (" + txtValue + ") Already Exist";
                                        lblMsg.ForeColor = System.Drawing.Color.Red;
                                        this.getMaxCode();
                                        //txtDoc_no.Enabled = false;

                                        btnSave.Enabled = true;   //IMP                                       

                                    }

                                    if (ViewState["mode"].ToString() == "U")
                                    {
                                        //fetch record
                                        qry = getDisplayQuery();
                                        bool recordExist = this.fetchRecord(qry);
                                        if (recordExist == true)
                                        {
                                            txtDoc_No.Enabled = false;


                                            hdnf.Value = txtDoc_No.Text;
                                            txtEditDoc_No.Text = string.Empty;
                                        }
                                    }
                                }
                            }
                            else   //Record Not Found
                            {
                                if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                {
                                    lblMsg.Text = "";

                                    txtDoc_No.Enabled = false;
                                    btnSave.Enabled = true;   //IMP
                                }
                                if (ViewState["mode"].ToString() == "U")
                                {
                                    this.makeEmptyForm("E");
                                    lblMsg.Text = "** Record Not Found";
                                    lblMsg.ForeColor = System.Drawing.Color.Red;
                                    txtDoc_No.Text = string.Empty;
                                    setFocusControl(txtDoc_No);
                                }
                            }
                        }
                    }
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    setFocusControl(txtDoc_No);
                }
            }
            else
            {
                this.makeEmptyForm("A");
                lblMsg.Text = "Doc No is numeric";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                clsButtonNavigation.enableDisable("E");
                txtDoc_No.Text = string.Empty;
                setFocusControl(txtDoc_No);
            }
        }
        catch
        {

        }
        #endregion
    }
    #endregion

    #region [btntxtDoc_No_Click]
    protected void btntxtDoc_No_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    txtEditDoc_No.Text = string.Empty;
        //    pnlPopup.Style["display"] = "block";
        //    hdnfClosePopup.Value = "txtDoc_No";
        //    btnSearch_Click(sender, e);
        //}
        //catch
        //{
        //}
        try
        {
            if (btntxtDoc_No.Text == "Change No")
            {
                pnlPopup.Style["display"] = "none";
                txtDoc_No.Text = string.Empty;
                txtDoc_No.Enabled = true;

                btnSave.Enabled = false;
                setFocusControl(txtDoc_No);
            }

            if (btntxtDoc_No.Text == "Choose No")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtDoc_No";
                btnSearch_Click(sender, e);
            }
        }
        catch
        {
        }
    }
    #endregion





    #region [txtDoc_Date_TextChanged]
    protected void txtDoc_Date_TextChanged(object sender, EventArgs e)
    {
        searchString = txtDoc_Date.Text;
        strTextBox = "txtDoc_Date";
        //csCalculations();
    }
    #endregion

    protected void txtBillNo_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtRef_Date);
    }

    protected void txtRef_Date_TextChanged(object sender, EventArgs e)
    {
    }

    protected void txtGST_TextChanged(object sender, EventArgs e)
    {
    }
    protected void txtMISC_TextChanged(object sender, EventArgs e)
    {

    }


    private void csCalculations()
    {
        try
        {

            if (strTextBox == "txtAc_Code")
            {
                string acname = "";
                if (txtAc_Code.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtAc_Code.Text);
                    if (a == false)
                    {
                        btntxtAc_Code_Click(this, new EventArgs());
                    }
                    else
                    {

                        acname = clsCommon.getString("select Ac_Name_E from  qrymstaccountmaster where Ac_Type= 'P' and Ac_Code=" + txtAc_Code.Text
                                + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));



                        if (acname != string.Empty && acname != "0")
                        {
                            lblAc_code.Text = acname;
                            setFocusControl(txtBillNo);

                        }
                        else
                        {
                            txtAc_Code.Text = string.Empty;
                            lblAc_code.Text = acname;
                            setFocusControl(txtAc_Code);
                        }

                    }
                }
                else
                {
                    setFocusControl(txtAc_Code);
                }


            }

            if (strTextBox == "txtGST_Rate_Code")
            {
                string GST_Rate = "";
                if (txtGST_Rate_Code.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtGST_Rate_Code.Text);
                    if (a == false)
                    {
                        btntxtGST_Rate_Code_Click(this, new EventArgs());
                    }
                    else
                    {

                        DataSet ds = new DataSet();
                        DataTable dt = new DataTable();
                        string qry1 = "select ifnull(Rate,0) as Rate,ifnull(CGST,0) as CGST, " +
                        " ifnull(SGST,0) as SGST,ifnull(IGST,0) as IGST from nt_1_gstratemaster  where Doc_no='" + txtGST_Rate_Code.Text + "' ";
                        ds = clsDAL.SimpleQuery(qry1);
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    txtCGST_Rate.Text = dt.Rows[0]["CGST"].ToString();
                                    txtSGST_Rate.Text = dt.Rows[0]["SGST"].ToString();
                                    txtIGST_Rate.Text = dt.Rows[0]["IGST"].ToString();
                                    lblGST_Rate_Code.Text = dt.Rows[0]["Rate"].ToString();
                                    //setFocusControl(txtGST);
                                    setFocusControl(txtTaxable_Amount);
                                }
                                else
                                {
                                    txtGST_Rate_Code.Text = string.Empty;
                                    lblGST_Rate_Code.Text = GST_Rate;
                                    setFocusControl(txtGST_Rate_Code);
                                }
                            }
                            else
                            {
                                txtGST_Rate_Code.Text = string.Empty;
                                lblGST_Rate_Code.Text = GST_Rate;
                                setFocusControl(txtGST_Rate_Code);
                            }

                        }

                        else
                        {
                            txtGST_Rate_Code.Text = string.Empty;
                            lblGST_Rate_Code.Text = GST_Rate;
                            setFocusControl(txtGST_Rate_Code);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtGST_Rate_Code);
                }


            }
            if (strTextBox == "txtCGST_Rate")
            {
                setFocusControl(txtCGST_Amount);
            }
            if (strTextBox == "txtSGST_Rate")
            {
                setFocusControl(txtSGST_Amount);
            }
            if (strTextBox == "txtTaxable_Amount")
            {
                setFocusControl(btnAdd);
            }
            if (strTextBox == "txtAc_CodeDetails")
            {
                string acname = "";
                if (txtAc_CodeDetails.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtAc_CodeDetails.Text);
                    if (a == false)
                    {
                        btntxtAc_CodeDetails_Click(this, new EventArgs());
                    }
                    else
                    {

                        acname = clsCommon.getString("select Ac_Name_E from  qrymstaccountmaster where Ac_Code=" + txtAc_CodeDetails.Text
                                + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        if (acname != string.Empty && acname != "0")
                        {
                            lblAc_CodeDetails.Text = acname;
                            setFocusControl(txtvalue);

                        }
                        else
                        {
                            txtAc_CodeDetails.Text = string.Empty;
                            lblAc_CodeDetails.Text = acname;
                            setFocusControl(txtAc_CodeDetails);
                        }

                    }
                }
                else
                {
                    setFocusControl(txtAc_CodeDetails);
                }
            }

            #region calculate subtotal



            double cgstrate = 0.00;
            double sgstrate = 0.00;
            double igstrate = 0.00;


            double CGSTRateForPS = 0.00;
            double SGSTRateForPS = 0.00;
            double IGSTRateForPS = 0.00;

            double CGSTAmountForPS = 0.0;
            double SGSTAmountForPS = 0.0;
            double IGSTAmountForPS = 0.0;

            double subtotal = 0.00;
            if (grdDetail.Rows.Count > 0)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    if (grdDetail.Rows[i].Cells[7].Text != "D")
                    {
                        double item_Amt = Convert.ToDouble(Server.HtmlDecode(grdDetail.Rows[i].Cells[5].Text.Trim()));
                        subtotal = subtotal + item_Amt;
                    }
                }
                txtTaxable_Amount.Text = subtotal.ToString();

            }
            #endregion


            double amount = Convert.ToDouble(txtTaxable_Amount.Text);

            if (Session["CompanyGSTStateCode"] == null || Session["CompanyGSTStateCode"] == string.Empty)
            {
                Session["CompanyGSTStateCode"] = clsCommon.getString("select GSTStateCode from nt_1_companyparameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " +
                    "  Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
            }
            int companyGstStateCode = Convert.ToInt32(Session["CompanyGSTStateCode"].ToString());

            string AcStateCode = "";
            if (txtAc_Code.Text.Trim() != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtAc_Code.Text);
                if (a == true)
                {
                    AcStateCode = clsCommon.getString("select IfNULL(GSTStateCode,0) from  qrymstaccountmaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtAc_Code.Text + "");
                }
            }
            int partygstStateCode = 0;
            if (AcStateCode.Trim().ToString() != "" && AcStateCode != "0")
            {
                partygstStateCode = Convert.ToInt32(AcStateCode);
            }
            if (companyGstStateCode == partygstStateCode)
            {
                cgstrate = Convert.ToDouble(clsDAL.GetString("select Rate from nt_1_gstratemaster where Doc_no='" + txtGST_Rate_Code.Text + "' "));
                CGSTRateForPS = cgstrate;


                double cgsttaxAmountOnMR = Math.Round((amount * cgstrate / 100), 2);
                CGSTAmountForPS = Math.Round(cgsttaxAmountOnMR, 2);

                sgstrate = Convert.ToDouble(clsDAL.GetString("select Rate from nt_1_gstratemaster where Doc_no='" + txtGST_Rate_Code.Text + "' "));
                SGSTRateForPS = sgstrate;

                double sgsttaxAmountOnMR = Math.Round((amount * sgstrate / 100), 2);
                SGSTAmountForPS = Math.Round(sgsttaxAmountOnMR, 2);

            }
            else
            {

                igstrate = Convert.ToDouble(clsDAL.GetString("select Rate from nt_1_gstratemaster where Doc_no='" + txtGST_Rate_Code.Text + "' "));

                IGSTRateForPS = igstrate;
                double igsttaxAmountOnMR = ((amount) * igstrate / 100);

                IGSTAmountForPS = Math.Round(igsttaxAmountOnMR, 2);
            }
            txtCGST_Amount.Text = CGSTAmountForPS.ToString();
            txtSGST_Amount.Text = SGSTAmountForPS.ToString();
            txtIGST_Amount.Text = IGSTAmountForPS.ToString();

            double finalA = amount + CGSTAmountForPS + SGSTAmountForPS + IGSTAmountForPS;
            txtfinalAmount.Text = finalA.ToString();
        }




        catch
        {

        }
    }

    #region [txtAc_Code_TextChanged]
    protected void txtAc_Code_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAc_Code.Text;
        strTextBox = "txtAc_Code";
        csCalculations();
    }
    #endregion
    protected void txtfinalAmount_textchanged(object sender, EventArgs e)
    {
    }

    #region [btntxtAc_Code_Click]
    protected void btntxtAc_Code_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtAc_Code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion



    #region [txtRemark_TextChanged]
    protected void txtRemark_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtRemark.Text;
        //strTextBox = "txtRemark";
        //csCalculations();
        setFocusControl(btnSave);
    }
    #endregion



    #region [btntxtBusiness_Vertical_Code_Click]
    protected void btntxtBusiness_Vertical_Code_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtBusiness_Vertical_Code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtTaxable_Amount_TextChanged]
    protected void txtTaxable_Amount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTaxable_Amount.Text;
        strTextBox = "txtTaxable_Amount";
        csCalculations();
    }
    #endregion

    #region [txtGST_Rate_Code_TextChanged]
    protected void txtGST_Rate_Code_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGST_Rate_Code.Text;
        strTextBox = "txtGST_Rate_Code";
        csCalculations();
    }
    #endregion

    #region [btntxtGST_Rate_Code_Click]
    protected void btntxtGST_Rate_Code_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtGST_Rate_Code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion



    #region [txtGross_Value_TextChanged]
    protected void txtGross_Value_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGross_Value.Text;
        strTextBox = "txtGross_Value";

    }
    #endregion



    #region [btntxtHSN_Click]
    protected void btntxtHSN_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtHSN";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtCGST_Rate_TextChanged]
    protected void txtCGST_Rate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCGST_Rate.Text;
        strTextBox = "txtCGST_Rate";
        csCalculations();
    }
    #endregion

    #region [txtCGST_Amount_TextChanged]
    protected void txtCGST_Amount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCGST_Amount.Text;
        strTextBox = "txtCGST_Amount";
        csCalculations();
    }
    #endregion

    #region [txtSGST_Rate_TextChanged]
    protected void txtSGST_Rate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSGST_Rate.Text;
        strTextBox = "txtSGST_Rate";
        csCalculations();
    }
    #endregion

    #region [txtSGST_Amount_TextChanged]
    protected void txtSGST_Amount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSGST_Amount.Text;
        strTextBox = "txtSGST_Amount";
        csCalculations();
    }
    #endregion

    #region [txtIGST_Rate_TextChanged]
    protected void txtIGST_Rate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtIGST_Rate.Text;
        strTextBox = "txtIGST_Rate";
        csCalculations();
    }
    #endregion

    #region [txtIGST_Amount _TextChanged]
    protected void txtIGST_Amount_TextChanged(object sender, EventArgs e)
    {
        searchString = txtIGST_Amount.Text;
        strTextBox = "txtIGST_Amount ";
        csCalculations();
    }
    #endregion





    #region [drpSub_Type_TextChanged]
    protected void drpSub_Type_SelectedIndexChanged(object sender, EventArgs e)
    {

        string max = clsCommon.getString("select max(dcid) as id from debitnotehead where Company_Code=" + Session["Company_Code"].ToString() + " " +
           " and Year_Code=" + Session["year"].ToString() + " and tran_type='" + drpSub_Type.SelectedValue + "'");
        hdnf.Value = max;
        hdnfTran_type.Value = drpSub_Type.SelectedValue;
        clsButtonNavigation.enableDisable("N");
        pnlgrdDetail.Enabled = true;
        this.makeEmptyForm("N");
        ViewState["mode"] = "I";
        showLastRecord();
        setFocusControl(drpSub_Type);

    }
    #endregion



    #region [txtAc_Code_TextChanged]
    protected void txtAc_CodeDetails_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAc_CodeDetails.Text;
        strTextBox = "txtAc_CodeDetails";
        csCalculations();
    }
    #endregion

    #region [btntxtAc_Code_Click]
    protected void btntxtAc_CodeDetails_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtAc_CodeDetails";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtAmount_TextChanged]
    protected void txtvalue_TextChanged(object sender, EventArgs e)
    {
        searchString = txtvalue.Text;
        strTextBox = "txtvalue";
        //  csCalculations();
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
            if (searchString != string.Empty && strTextBox == hdnfClosePopup.Value)
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
                if (btntxtDoc_No.Text == "Change No")
                {
                    pnlPopup.Style["display"] = "none";
                    txtDoc_No.Text = string.Empty;
                    txtDoc_No.Enabled = true;
                    btnSave.Enabled = false;
                    setFocusControl(txtDoc_No);
                    hdnfClosePopup.Value = "Close";
                }
                if (btntxtDoc_No.Text == "Choose No")
                {
                    searchString = txtEditDoc_No.Text;
                    strTextBox = "txtEditDoc_No";
                    pnlPopup.Style["display"] = "block";
                    if (searchString != string.Empty && strTextBox == hdnfClosePopup.Value)
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
                        // name += "Doc_No Like '%" + aa + "%'or";

                        name += "( Doc_No like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or Net_Amount like '%" + aa + "%'  or Remark_For_Print like '%" + aa + "%') and";

                    }
                    name = name.Remove(name.Length - 3);
                    lblPopupHead.Text = "--Select Group--";
                    // string qry = " select Doc_No,Sub_Type,Ref_No as Bill_No,Unique_Id,Ac_Code from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (" + name + ") order by Doc_No";
                    string qry = " select Doc_No,Doc_Date,Ac_Name_E as  Party_name,Net_Amount as Amount,Remark_For_Print as Narration from qryDebitCreditNote where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (" + name + ") order by Doc_No";
                    this.showPopup(qry);
                }
            }


            if (hdnfClosePopup.Value == "txtBranch_Code")
            {

                foreach (var s in split)
                {
                    string aa = s.ToString();

                    // name += " Branch_Name_E like '%" + aa + "%'  or Branch_Code like '%" + aa + "%'  or Branch_Name_R like '%" + aa + "%'  or";
                    name += "( Branch_Name_E like '%" + aa + "%' or Branch_Code like '%" + aa + "%' or Branch_Name_R like '%" + aa + "%' ) and";


                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Branch Code--";
                string qry = "select Branch_Code,Branch_Name_E,Branch_Name_R from  Branch_Master   where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                + "  and ( " + name + ") order by Branch_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtRef_No")
            {


                lblPopupHead.Text = "--Select Ref no--";
                string qry = "";
                if (drpSub_Type.SelectedValue == "C2" || drpSub_Type.SelectedValue == "D2")
                {
                    foreach (var s in split)
                    {
                        string aa = s.ToString();

                        // name += " Branch_Name_E like '%" + aa + "%'  or Branch_Code like '%" + aa + "%'  or Branch_Name_R like '%" + aa + "%'  or";
                        name += "( Doc_No like '%" + aa + "%' or Supplier_name like '%" + aa + "%' or Unique_Id like '%" + aa + "%' ) ";


                    }
                    qry = " select distinct Unique_Id,Doc_No,Bill_No,Doc_Date,Supplier_name,Bill_Amount from qryPurchase" +
                         " where Tran_type='PB' and Supplier_Code=" + txtAc_Code.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                        + " and " + name + " order by Doc_Date desc,Doc_No desc";
                }
                else
                {


                    foreach (var s in split)
                    {
                        string aa = s.ToString();

                        // name += " Branch_Name_E like '%" + aa + "%'  or Branch_Code like '%" + aa + "%'  or Branch_Name_R like '%" + aa + "%'  or";
                        name += "( Doc_No like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or Unique_Id like '%" + aa + "%' ) and";


                    }
                    name = name.Remove(name.Length - 3);
                    qry = "select Unique_Id,Doc_No,convert(varchar(10),Doc_Date,103) as Doc_Date,Year_Code,Ac_Name_E as Partyname ,Doc_Date as DT from  qryPendingSaleBill   where Company_Code="
                        + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtAc_Code.Text
                   + "  and ( " + name + ") order by DT desc";
                }

                this.showPopup(qry);

            }
            if (hdnfClosePopup.Value == "txtRef_Unique_Id")
            {
                lblPopupHead.Text = "--Select--";
                string qry = "";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtAc_Code")
            {
                split = txtAc_Code.Text.Split(delimiter);
                foreach (var s in split)
                {
                    string aa = s.ToString();

                    //name += " Ac_Name_E like '%" + aa + "%'  or Ac_Code like '%" + aa + "%'  or Ac_Name_R like '%" + aa + "%'  or";
                    name += "( Ac_Name_E like '%" + aa + "%' or Ac_Code like '%" + aa + "%' or cityname like '%" + aa + "%' ) and";


                }

                name = name.Remove(name.Length - 3);
                string qry = string.Empty;

                lblPopupHead.Text = "--Select Branch Code--";
                qry = "select Ac_Code,Ac_Name_E,cityname from  qrymstaccountmaster   where  Ac_Type='P' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
               + "  and ( " + name + ") order by Ac_Name_E";


                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtGST_Rate_Code")
            {
                split = txtGST_Rate_Code.Text.Split(delimiter);
                foreach (var s in split)
                {
                    string aa = s.ToString();

                    //name += " GST_Code like '%" + aa + "%'  or GST_Name_E like '%" + aa + "%'  or Rate like '%" + aa + "%'  or";
                    name += "( Doc_no like '%" + aa + "%' or GST_Name like '%" + aa + "%' or Rate like '%" + aa + "%' ) and";


                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select--";
                string qry = "select Doc_no,GST_Name,Rate from  nt_1_gstratemaster   where ( " + name + ") order by GST_Name";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtHSN")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();

                    //name += " HSN_Code like '%" + aa + "%'  or Doc_no like '%" + aa + "%'  or Reference_Name like '%" + aa + "%'  or";
                    name += "( HSN_Code like '%" + aa + "%' or Doc_no like '%" + aa + "%' or Reference_Name like '%" + aa + "%' ) and";


                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select HSN Code--";
                string qry = "select Doc_no,HSN_Code from  HSN_Master   where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                + "  and ( " + name + ") order by HSN_Code";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtAc_CodeDetails")
            {
                split = txtAc_CodeDetails.Text.Split(delimiter);
                foreach (var s in split)
                {
                    string aa = s.ToString();

                    //name += " Ac_Name_E like '%" + aa + "%'  or Ac_Code like '%" + aa + "%'  or Ac_Name_R like '%" + aa + "%'  or";
                    name += "( Ac_Name_E like '%" + aa + "%' or Ac_Code like '%" + aa + "%' or cityname like '%" + aa + "%' ) and";


                }

                name = name.Remove(name.Length - 3);

                lblPopupHead.Text = "--Select Ac Code--";
                string qry = "select Ac_Code,Ac_Name_E,cityname from  qrymstaccountmaster   where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
               + "  and ( " + name + ") order by Ac_Name_E";
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
                        pnlPopup.Style["display"] = "block";
                    }
                    else
                    {
                        grdPopup.DataSource = null;
                        grdPopup.DataBind();
                        hdHelpPageCount.Value = "0";
                        pnlPopup.Style["display"] = "block";
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
        btnSave.Enabled = true;
        #region [Validation Part]
        bool isValidated = true;
        string typeauth = "";
        if (txtDoc_Date.Text != string.Empty)
        {
            string dt = DateTime.Parse(txtDoc_Date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            if (clsCommon.isValidDate(dt) == true)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtDoc_Date);
                return;
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtDoc_Date);
            return;
        }






        if (txtDoc_No.Text != string.Empty)
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtDoc_No);
            return;
        }
        if (txtAc_Code.Text != string.Empty)
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            setFocusControl(txtAc_Code);
            return;
        }


        int AcCode;
        Int32.TryParse(txtAc_Code.Text, out AcCode);
        if (AcCode == 0)
        {

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please enter correct Ac Code! ');", true);

            setFocusControl(txtAc_Code);
            return;
        }



        int GSTRate;
        Int32.TryParse(txtGST_Rate_Code.Text, out GSTRate);
        if (GSTRate == 0)
        {

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please enter correct GST Rate Code! ');", true);

            setFocusControl(txtGST_Rate_Code);
            return;
        }


        #endregion

        #region -Head part declearation


        int Company_Code = Convert.ToInt32(Convert.ToInt32(Session["Company_Code"].ToString()));
        int Year_Code = Convert.ToInt32(Convert.ToInt32(Session["year"].ToString()));
        int Branch_Code = Convert.ToInt32(Convert.ToInt32(Session["Branch_Code"].ToString()));
        string Created_By = Session["user"].ToString();
        string Modified_By = Session["user"].ToString();
        string Created_Date = DateTime.Now.ToString("yyyy/MM/dd");
        string Modified_Date = DateTime.Now.ToString("yyyy/MM/dd");
        string retValue = string.Empty;
        string strRev = string.Empty;



        #endregion-End of Head part declearation

        #region Detail Field
        Detail_Fields.Append("doc_no,");
        Detail_Fields.Append("tran_type,");
        Detail_Fields.Append("expac_code,");
        Detail_Fields.Append("value,");
        Detail_Fields.Append("expac,");
        Detail_Fields.Append("dcid,");
        Detail_Fields.Append("dcdetailid,");
        Detail_Fields.Append("detail_Id,");
        Detail_Fields.Append("Company_Code,");
        Detail_Fields.Append("year_code");

        #endregion

        #region Save Head Part
        //Entry_no = txtDoc_No.Text != string.Empty ? Convert.ToInt32(txtDoc_No.Text) : 0;
        //EntryId = lblDoc_No.Text != string.Empty ? Convert.ToInt32(lblDoc_No.Text) : 0;

        type = drpSub_Type.Text;
        Entry_date = DateTime.Parse(txtDoc_Date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        AcCode = Convert.ToInt32(txtAc_Code.Text);
        Ac_name = Convert.ToString(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + txtAc_Code.Text + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
        Bill_No = txtBillNo.Text != string.Empty ? Convert.ToInt32(txtBillNo.Text) : 0;
        Bill_date = DateTime.Parse(txtRef_Date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

        Bill_Id = lblBillid.Text != string.Empty ? Convert.ToInt32(lblBillid.Text) : 0;
        Gst_Code = Convert.ToInt32(txtGST_Rate_Code.Text);
        GstName = Convert.ToString(clsCommon.getString("select Doc_no from nt_1_gstratemaster  where Doc_no=" + txtGST_Rate_Code.Text + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
        try
        {
            Ac_Id = Convert.ToString(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + txtAc_Code.Text + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
        }
        catch
        {
        }
        TaxableAmount = txtTaxable_Amount.Text != string.Empty ? Convert.ToDouble(txtTaxable_Amount.Text) : 0.00;
        CgstRate = txtCGST_Rate.Text != string.Empty ? Convert.ToDouble(txtCGST_Rate.Text) : 0.00;
        CgstAmount = txtCGST_Amount.Text != string.Empty ? Convert.ToDouble(txtCGST_Amount.Text) : 0.00;
        SgstRate = txtSGST_Rate.Text != string.Empty ? Convert.ToDouble(txtSGST_Rate.Text) : 0.00;
        SgstAmount = txtSGST_Amount.Text != string.Empty ? Convert.ToDouble(txtSGST_Amount.Text) : 0.00;
        IgstRate = txtIGST_Rate.Text != string.Empty ? Convert.ToDouble(txtIGST_Rate.Text) : 0.00;
        IgstAmount = txtIGST_Amount.Text != string.Empty ? Convert.ToDouble(txtIGST_Amount.Text) : 0.00;
        MiscAmount = txtMISC.Text != string.Empty ? Convert.ToDouble(txtMISC.Text) : 0.00;
        FinalAmount = txtfinalAmount.Text != string.Empty ? Convert.ToDouble(txtfinalAmount.Text) : 0.00;
        #endregion
        if (btnSave.Text == "Save")
        {

            #region Head Add Fields And Values
            this.NextNumber();
            Head_Fields.Append("doc_no,");
            Head_Values.Append("'" + Doc_No + "',");
            Head_Fields.Append("dcid,");
            Head_Values.Append("'" + Entry_no + "',");
            Head_Fields.Append("Company_Code,");
            Head_Values.Append("'" + Company_Code + "',");
            Head_Fields.Append("tran_type,");
            Head_Values.Append("'" + type + "',");
            Head_Fields.Append("doc_date,");
            Head_Values.Append("'" + Entry_date + "',");
            Head_Fields.Append("ac_code,");
            Head_Values.Append("'" + AcCode + "',");

            Head_Fields.Append("bill_no,");
            Head_Values.Append("'" + Bill_No + "',");
            Head_Fields.Append("bill_date,");
            Head_Values.Append("'" + Bill_date + "',");

            Head_Fields.Append("bill_id,");
            Head_Values.Append("'" + Bill_Id + "',");
            Head_Fields.Append("gst_code,");
            Head_Values.Append("'" + Gst_Code + "',");
            Head_Fields.Append("ac,");
            Head_Values.Append("case when 0='" + Ac_Id + "' then null else '" + Ac_Id + "' end,");
            Head_Fields.Append("texable_amount,");
            Head_Values.Append("'" + TaxableAmount + "',");
            Head_Fields.Append("cgst_rate,");
            Head_Values.Append("'" + CgstRate + "',");
            Head_Fields.Append("cgst_amount,");
            Head_Values.Append("'" + CgstAmount + "',");
            Head_Fields.Append("sgst_rate,");
            Head_Values.Append("'" + SgstRate + "',");
            Head_Fields.Append("sgst_amount,");
            Head_Values.Append("'" + SgstAmount + "',");
            Head_Fields.Append("Created_By,");
            Head_Values.Append("'" + Created_By + "',");
            Head_Fields.Append("igst_rate,");
            Head_Values.Append("'" + IgstRate + "',");
            Head_Fields.Append("Year_Code,");
            Head_Values.Append("'" + Year_Code + "',");
            Head_Fields.Append("igst_amount,");
            Head_Values.Append("'" + IgstAmount + "',");
            Head_Fields.Append("misc_amount,");
            Head_Values.Append("'" + MiscAmount + "',");
            Head_Fields.Append("bill_amount");
            Head_Values.Append("'" + FinalAmount + "'");

            #endregion

            Head_Insert = "insert into " + tblHead + "(" + Head_Fields + ") values(" + Head_Values + ")";
            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = Head_Insert;
            Maindt.Rows.Add(dr);
            #region[details filed]


            int DCDetailid = Convert.ToInt32(clsCommon.getString("SELECT ifnull(count(dcdetailid),0) as dcdetailid from " + tblDetails + " "));
            if (DCDetailid == 0)
            {
                dcDetail_id = 0;
            }
            else
            {
                DCDetailid = Convert.ToInt32(clsCommon.getString("select max(dcdetailid) as dcdetailid from " + tblDetails + " "));
            }
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {

                DCDetailid = DCDetailid + 1;
                ExpAccode = Convert.ToInt32(Server.HtmlDecode(grdDetail.Rows[i].Cells[3].Text));
                Expname = Convert.ToString(grdDetail.Rows[i].Cells[4].Text);
                value = Convert.ToInt32(grdDetail.Rows[i].Cells[5].Text);
                DetailId = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);
                string ExpAcDetail = Convert.ToString(clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code=" + ExpAccode + " and Company_Code=" + Session["Company_Code"].ToString() + ""));

                Detail_Values.Append("('" + Doc_No + "','" + type + "','" + ExpAccode + "','" + value + "'," +
                    " case when 0='" + ExpAcDetail + "' then null else '" + ExpAcDetail + "' end,'" + Entry_no + "','" + DCDetailid + "','" + DetailId + "'," +
                    " '" + Session["Company_Code"].ToString() + "','" + Session["year"].ToString() + "'),");
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
            #region Update Head Add Fields And Values
            Head_Update.Append("doc_date=");
            Head_Update.Append("'" + Entry_date + "',");
            Head_Update.Append("ac_code=");
            Head_Update.Append("'" + AcCode + "',");
            Head_Update.Append("bill_no=");
            Head_Update.Append("'" + Bill_No + "',");
            Head_Update.Append("bill_date=");
            Head_Update.Append("'" + Bill_date + "',");
            Head_Update.Append("bill_id=");
            Head_Update.Append("'" + Bill_Id + "',");
            Head_Update.Append("gst_code=");
            Head_Update.Append("'" + Gst_Code + "',");

            Head_Update.Append("ac=");
            Head_Update.Append("case when 0='" + Ac_Id + "' then null else '" + Ac_Id + "' end,");
            Head_Update.Append("texable_amount=");
            Head_Update.Append("'" + TaxableAmount + "',");
            Head_Update.Append("cgst_rate=");
            Head_Update.Append("'" + CgstRate + "',");
            Head_Update.Append("cgst_amount=");
            Head_Update.Append("'" + CgstAmount + "',");
            Head_Update.Append("sgst_rate=");
            Head_Update.Append("'" + SgstRate + "',");
            Head_Update.Append("sgst_amount=");
            Head_Update.Append("'" + SgstAmount + "',");
            Head_Update.Append("Modified_By=");
            Head_Update.Append("'" + Modified_By + "',");
            Head_Update.Append("igst_rate=");
            Head_Update.Append("'" + IgstRate + "',");
            Head_Update.Append("igst_amount=");
            Head_Update.Append("'" + IgstRate + "',");
            Head_Update.Append("misc_amount=");
            Head_Update.Append("'" + MiscAmount + "',");
            Head_Update.Append("bill_amount=");
            Head_Update.Append("'" + FinalAmount + "'");

            string Head_Updateqry = "update " + tblHead + " set " + Head_Update + " where dcid='" + lblDoc_No.Text + "'";

            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = Head_Updateqry;
            Maindt.Rows.Add(dr);

            #endregion




            string concatid = string.Empty;

            int DBNoteId = Convert.ToInt32(clsCommon.getString("select max(dcdetailid) as dcdetailid from " + tblDetails + " "));
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {

                DetailId = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);
                ExpAccode = Convert.ToInt32(Server.HtmlDecode(grdDetail.Rows[i].Cells[3].Text));
                int Expname1 = 0;
                try
                {
                    Expname1 = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='" + ExpAccode + "'"));
                }
                catch
                {
                }

                value = Convert.ToDouble(Server.HtmlDecode(grdDetail.Rows[i].Cells[5].Text));
                int id = Convert.ToInt32(grdDetail.Rows[i].Cells[6].Text);
                #region Insert Grid Record
                if (grdDetail.Rows[i].Cells[Rowaction].Text == "A")
                {
                    DBNoteId = DBNoteId + 1;
                    Detail_Values.Append("('" + txtDoc_No.Text + "','" + type + "','" + ExpAccode + "','" + value + "'," +
                   " case when 0='" + Expname1 + "' then null else '" + Expname1 + "' end,'" + lblDoc_No.Text + "','" + DBNoteId + "','" + DetailId + "'," +
                    " '" + Session["Company_Code"].ToString() + "','" + Session["year"].ToString() + "'),");
                }
                #endregion
                #region Update Grid Record
                if (grdDetail.Rows[i].Cells[Rowaction].Text == "U")
                {

                    Detail_Update.Append("expac_code=case dcdetailid when '" + id + "' then '" + ExpAccode + "'  ELSE expac_code END,");
                    Detail_Update.Append("value=case dcdetailid when '" + id + "' then '" + value + "'  ELSE value END,");
                    Detail_Update.Append("expac=case dcdetailid when '" + id + "' then '" + Expname1 + "'  ELSE expac END,");

                    concatid = concatid + "'" + id + "',";


                }
                #endregion
                #region Delete Grid Record
                if (grdDetail.Rows[i].Cells[7].Text == "D")
                {
                    Detail_Delete.Append("'" + grdDetail.Rows[i].Cells[6].Text + "',");
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
                string Detail_Deleteqry = "delete from " + tblDetails + " where dcdetailid in(" + Detail_Delete + ")";

                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Detail_Deleteqry;
                Maindt.Rows.Add(dr);

            }
            if (Detail_Update.Length > 0)
            {
                concatid = concatid.Remove(concatid.Length - 1);
                Detail_Update.Remove(Detail_Update.Length - 1, 1);
                string Detail_Updateqry = "update " + tblDetails + " set " + Detail_Update + " where dcdetailid in(" + concatid + ")";

                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Detail_Updateqry;
                Maindt.Rows.Add(dr);

            }
            Doc_No = Convert.ToInt32(txtDoc_No.Text);
            flag = 2;

        }
        #region Gledger effect
        int Order_Code = 0;
        FormTypes types = new FormTypes();
        StringBuilder Gledger_values = new StringBuilder();
        string drcr = string.Empty;
        string drcr0 = string.Empty;
        GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='" + type + "' and Doc_No=" + Doc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
        dr = null;
        dr = Maindt.NewRow();
        dr["Querys"] = GLEDGER_Delete;
        Maindt.Rows.Add(dr);

        StringBuilder Gledger_Column = new StringBuilder();
        Gledger_Column.Append("TRAN_TYPE,CASHCREDIT,DOC_NO,DOC_DATE,AC_CODE,UNIT_code,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE," +
                    " YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO,ac,vc,progid,tranid");

        string SaleCGST = clsCommon.getString("select CGSTAc from nt_1_companyparameters where Company_Code=" + Company_Code + " and Year_Code=" + Year_Code + "");
        string SaleSGST = clsCommon.getString("select SGSTAc from nt_1_companyparameters where Company_Code=" + Company_Code + " and Year_Code=" + Year_Code + "");
        string SaleIGST = clsCommon.getString("select IGSTAc from nt_1_companyparameters where Company_Code=" + Company_Code + " and Year_Code=" + Year_Code + "");

        string CGST_id = clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='" + SaleCGST + "' and Company_Code=" + Company_Code + "");
        string SGST_id = clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='" + SaleSGST + "' and Company_Code=" + Company_Code + "");
        string IGST_id = clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='" + SaleIGST + "' and Company_Code=" + Company_Code + "");
        if (drpSub_Type.SelectedValue == "DN" || drpSub_Type.SelectedValue == "D1" || drpSub_Type.SelectedValue == "D2")
        {
            drcr = "D";
            drcr0 = "C";
        }
        else if (drpSub_Type.SelectedValue == "CN" || drpSub_Type.SelectedValue == "C1" || drpSub_Type.SelectedValue == "C2")
        {
            drcr = "C";
            drcr0 = "D";
        }

        // Acc Code Effect
        Order_Code = Order_Code + 1;
        Gledger_values.Append("('" + type + "','DN','" + Doc_No + "','" + Entry_date + "','" + txtAc_Code.Text + "','0','','" + FinalAmount + "', " +
                                                  " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','" + drcr + "',0,0,'" + Branch_Code + "','DN','" + Doc_No + "'," +
                                                  " case when 0='" + Ac_Id + "' then null else '" + Ac_Id + "' end,'0',0,'0'),");
        //Grid Effect
        for (int i = 0; i < grdDetail.Rows.Count; i++)
        {
            ExpAccode = Convert.ToInt32(Server.HtmlDecode(grdDetail.Rows[i].Cells[3].Text));
            value = Convert.ToDouble(Server.HtmlDecode(grdDetail.Rows[i].Cells[5].Text));
            string expac_id = clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='" + ExpAccode + "' and Company_Code=" + Company_Code + "");
            Gledger_values.Append("('" + type + "','DN','" + Doc_No + "','" + Entry_date + "','" + ExpAccode + "','0','','" + value + "', " +
                                                   " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','" + drcr0 + "',0,0,'" + Branch_Code + "','DN','" + Doc_No + "'," +
                                                   " case when 0='" + expac_id + "' then null else '" + expac_id + "' end,'0',0,'0'),");
        }


        //CGST Ac Effect
        if (CgstAmount > 0)
        {
            Gledger_values.Append("('" + type + "','DN','" + Doc_No + "','" + Entry_date + "','" + SaleCGST + "','0','','" + CgstAmount + "', " +
                                                      " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','" + drcr0 + "',0,0,'" + Branch_Code + "','DN','" + Doc_No + "'," +
                                                      " case when 0='" + CGST_id + "' then null else '" + CGST_id + "' end,'0',0,'0'),");
        }

        //SGST Acc
        if (SgstAmount > 0)
        {
            Gledger_values.Append("('" + type + "','DN','" + Doc_No + "','" + Entry_date + "','" + SaleSGST + "','0','','" + SgstAmount + "', " +
                                                     " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','" + drcr0 + "',0,0,'" + Branch_Code + "','DN','" + Doc_No + "'," +
                                                     " case when 0='" + SGST_id + "' then null else '" + SGST_id + "' end,'0',0,'0'),");
        }

        //IGST Acc
        if (IgstAmount > 0)
        {
            Gledger_values.Append("('" + type + "','DN','" + Doc_No + "','" + Entry_date + "','" + SaleIGST + "','0','','" + IgstAmount + "', " +
                                                     " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + Order_Code + "','" + drcr0 + "',0,0,'" + Branch_Code + "','DN','" + Doc_No + "'," +
                                                     " case when 0='" + IGST_id + "' then null else '" + IGST_id + "' end,'0',0,'0'),");
        }

        if (Gledger_values.Length > 0)
        {
            Gledger_values.Remove(Gledger_values.Length - 1, 1);
            GLEDGER_Insert = "insert into nt_1_gledger (" + Gledger_Column + ") values " + Gledger_values + " ";
        }


        dr = null;
        dr = Maindt.NewRow();
        dr["Querys"] = GLEDGER_Insert;
        Maindt.Rows.Add(dr);
        #endregion

        msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);
        if (msg == "Insert")
        {
            hdnf.Value = Entry_no.ToString();
            clsButtonNavigation.enableDisable("S");
            this.makeEmptyForm("S");
            qry = getDisplayQuery();
            this.fetchRecord(qry);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Added !')", true);
        }
        else if (msg == "Update")
        {
            hdnf.Value = lblDoc_No.Text;
            clsButtonNavigation.enableDisable("S");
            this.makeEmptyForm("S");
            qry = getDisplayQuery();
            this.fetchRecord(qry);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update !')", true);
        }

    }
}
    #endregion



