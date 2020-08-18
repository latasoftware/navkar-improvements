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
public partial class pgeUtrentryxml : System.Web.UI.Page
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
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    string AccountMasterTable = string.Empty;
    string qryAccountList = string.Empty;
    static WebControl objAsp = null;
    string cs = string.Empty;
    string Action = string.Empty;
    int id = 0;
    int flag = 0;
    int count = 0;
    int Doc_No = 0;
    MySqlConnection con = null;
    MySqlCommand cmd = null;
    MySqlTransaction myTran = null;
    string GLEDGER_Insert = string.Empty;
    string GLEDGER_Delete = string.Empty;
    string msg = string.Empty;

    int Detail_Id = 2;
    int lot_no = 3;
    int lot_no_Name = 4;
    int lot_no_comp = 5;
    int grade_no = 6;
    int amount = 7;
    int Adj_Amt = 8;
    int ltno = 9;
    int UtrDetailID = 10;
    int Rowaction = 11;
    int Srno = 12;
    #endregion

    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;
    #region Detail part Declaration
    int Grid_Id = 0;
    int Grid_lot_no = 0;
    int lotCompany_Code = 0;
    int lotYear_Code = 0;
    string Grid_grade_no = string.Empty;
    double Grid_amount = 0.00;
    double Adjusted_Amt = 0.00;
    int LTNo = 0;

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
    string doc_date = string.Empty;
    string bank_ac = string.Empty;
    int mill_code = 0;
    string Lott_No = string.Empty;
    double Amount = 0.00;
    string utr_no = string.Empty;
    string narration_header = string.Empty;
    string narration_footer = string.Empty;
    int bank_id = 0;
    int mill_id = 0;
    string Head_Insert = string.Empty;
    StringBuilder Head_Update = null;
    string Head_Delete = string.Empty;
    StringBuilder Head_Fields = null;
    StringBuilder Head_Values = null;
    #endregion-End of Head part declearation
    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = "NT_1_UTR";
            tblDetails = "NT_1_UTRDetail";
            qryCommon = "qryutrheaddetail";
            AccountMasterTable = "qrymstaccountmaster";
            qryAccountList = "qrymstaccountmaster";
            user = Session["user"].ToString();
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
                    if (Action == "1")
                    {
                        hdnf.Value = Request.QueryString["utrid"];
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
                pnlPopup.Style["display"] = "none";
                btnAdddetails.Enabled = false;
                btnClosedetails.Enabled = false;
                btnOpenDetailsPopup.Enabled = false;
                grdDetail.DataSource = null; grdDetail.DataBind();
                ViewState["currentTable"] = null;
                ViewState["currentTable"] = null;
                btnSave.Text = "Save";
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                txtEditDoc_No.Enabled = true;
                lblMsg.Text = string.Empty;
                txtdoc_date.Enabled = false;
                CalendarExtenderDatetxtdoc_date.Enabled = false;
                txtbank_ac.Enabled = false;
                btntxtbank_ac.Enabled = false;
                txtmill_code.Enabled = false;
                btntxtmill_code.Enabled = false;
                //txtlot_no.Enabled = false;
                //btntxtlot_no.Enabled = false;
                txtamount.Enabled = false;
                txtunt_no.Enabled = false;
                txtnarration_header.Enabled = false;
                txtnarration_footer.Enabled = false;
                txtlotno_Detail.Enabled = false;
                btntxtlot_no_Detail.Enabled = false;
                txtgrade_no.Enabled = false;
                btntxtgrade_no.Enabled = false;
                txtamount_Detail.Enabled = false;
                chkIsSave.Enabled = false;
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
                txtEditDoc_No.Enabled = false;
                txtdoc_no.Enabled = false;
                btnAdddetails.Enabled = true;
                btnClosedetails.Enabled = true;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null; grdDetail.DataBind();
                ViewState["currentTable"] = null;
                //txtlot_no.Enabled = true;
                lblyear_Code_Detail.Text = string.Empty;
                lblCompnycode_Detail.Text = string.Empty;
                //btntxtlot_no.Enabled = true;
                txtgrade_no.Enabled = true;
                // lblgrade_name.Text = string.Empty;
                btntxtgrade_no.Enabled = true;
                txtamount_Detail.Enabled = true;
                txtdoc_date.Enabled = true;
                txtdoc_date.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                CalendarExtenderDatetxtdoc_date.Enabled = true;
                txtbank_ac.Enabled = true;
                lblbank_Name.Text = string.Empty;
                btntxtbank_ac.Enabled = true;
                txtmill_code.Enabled = true;
                lblmill_name.Text = string.Empty;
                btntxtmill_code.Enabled = true;
                txtlotno_Detail.Enabled = true;
                //lblyear_code.Text = string.Empty;
                //lblCompnycode.Text = string.Empty;
                btntxtlot_no_Detail.Enabled = true;
                txtamount.Enabled = true;
                txtunt_no.Enabled = true;
                txtnarration_header.Enabled = true;
                txtnarration_footer.Enabled = true;
                chkIsSave.Enabled = true;
                #region set Business logic for save
                #endregion

                btntxtdoc_no.Enabled = false;
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
                txtEditDoc_No.Enabled = true;
                lblMsg.Text = string.Empty;
                txtdoc_date.Enabled = false;
                CalendarExtenderDatetxtdoc_date.Enabled = false;
                txtbank_ac.Enabled = false;
                btntxtbank_ac.Enabled = false;
                txtmill_code.Enabled = false;
                btntxtmill_code.Enabled = false;
                //txtlot_no.Enabled = false;
                //btntxtlot_no.Enabled = false;
                txtamount.Enabled = false;
                txtunt_no.Enabled = false;
                txtnarration_header.Enabled = false;
                txtnarration_footer.Enabled = false;
                //txtlot_no.Enabled = false;
                //btntxtlot_no.Enabled = false;
                txtgrade_no.Enabled = false;
                btntxtgrade_no.Enabled = false;
                txtamount.Enabled = false;
                txtlotno_Detail.Text = string.Empty;
                btntxtlot_no_Detail.Enabled = false;
                txtgrade_no.Text = string.Empty;
                btntxtgrade_no.Enabled = false;
                // txtamount.Text = string.Empty;
                btnAdddetails.Text = "ADD";
                btnAdddetails.Enabled = false;
                btnClosedetails.Enabled = false;
                chkIsSave.Enabled = false;
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
                txtEditDoc_No.Enabled = false;
                lblMsg.Text = string.Empty;
                txtdoc_date.Enabled = true;
                CalendarExtenderDatetxtdoc_date.Enabled = true;
                txtbank_ac.Enabled = true;
                btntxtbank_ac.Enabled = true;
                txtmill_code.Enabled = true;
                btntxtmill_code.Enabled = true;
                //txtlot_no.Enabled = true;
                //btntxtlot_no.Enabled = true;
                txtamount.Enabled = true;
                txtunt_no.Enabled = true;
                txtnarration_header.Enabled = true;
                txtnarration_footer.Enabled = true;
                txtlotno_Detail.Enabled = true;
                btntxtlot_no_Detail.Enabled = true;
                txtgrade_no.Enabled = true;
                btntxtgrade_no.Enabled = true;
                txtamount.Enabled = true;
                btnAdddetails.Enabled = true;
                btnClosedetails.Enabled = true;
                chkIsSave.Enabled = true;
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
    #region [enableDisableNavigateButtons]
    private void enableDisableNavigateButtons()
    {
        #region enable disable previous next buttons
        //   int RecordCount = 0;
        //   string query = "";
        //   query = "select count(*) from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' and Year_Code="+Convert.ToInt32(Session["year"]).ToString());
        //string cnt = clsCommon.getString(query); 
        //  if (cnt != string.Empty) 
        //       {
        //RecordCount = Convert.ToInt32(cnt);
        //       }
        //   if (RecordCount != 0 && RecordCount == 1)
        //   {
        //       btnFirst.Enabled = true;
        //       btnPrevious.Enabled = false;
        //       btnNext.Enabled = false;
        //       btnLast.Enabled = false;
        //   }
        //   else if (RecordCount != 0 && RecordCount > 1)
        //   {
        //       btnFirst.Enabled = true;
        //       btnPrevious.Enabled = false;
        //       btnNext.Enabled = false;
        //       btnLast.Enabled = true;
        //   }
        //   if (txtdoc_no.Text != string.Empty)
        //   {
        //       #region check for next or previous record exist or not
        //       query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString())+ "' and Year_Code="+Convert.ToInt32(Session["year"]).ToString())+" ORDER BY doc_no asc  ";
        //       string strDoc_No = clsCommon.getString(query);
        //        if (strDoc_No != string.Empty)
        //        {
        //         btnNext.Enabled = true;
        //         btnLast.Enabled = true;
        //        }
        //       else
        //        {
        //         btnNext.Enabled = false;
        //         btnLast.Enabled = false;
        //        }
        //       query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString())+ "' and Year_Code="+Convert.ToInt32(Session["year"]).ToString())+" ORDER BY doc_no desc  ";
        //        if (strDoc_No != string.Empty)
        //        {
        //         btnPrevious.Enabled = true;
        //         btnFirst.Enabled = true;
        //        }
        //       else
        //        {
        //         btnPrevious.Enabled = false;
        //         btnFirst.Enabled = false;
        //        }
        //   }
        //       #endregion
        #endregion

        #region enable disable previous next buttons
        //int RecordCount = 0;
        //string query = "";
        //query = "select count(*) from " + tblHead + " where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
        //DataSet ds = new DataSet();
        //DataTable dt = new DataTable();
        //ds = clsDAL.SimpleQuery(query);
        //if (ds != null)
        //{
        //    if (ds.Tables.Count > 0)
        //    {
        //        dt = ds.Tables[0];
        //        if (dt.Rows.Count > 0)
        //        {
        //            RecordCount = Convert.ToInt32(dt.Rows[0][0].ToString());
        //        }
        //    }
        //}
        //if (RecordCount != 0 && RecordCount == 1)
        //{
        //    btnFirst.Enabled = true;
        //    btnPrevious.Enabled = false;
        //    btnNext.Enabled = false;
        //    btnLast.Enabled = false;
        //}
        //else if (RecordCount != 0 && RecordCount > 1)
        //{
        //    btnFirst.Enabled = true;
        //    btnPrevious.Enabled = false;
        //    btnNext.Enabled = false;
        //    btnLast.Enabled = true;
        //}
        //if (txtdoc_no.Text != string.Empty)
        //{
        //    if (hdnf.Value != string.Empty)
        //    {
        //        #region check for next or previous record exist or not
        //        ds = new DataSet();
        //        dt = new DataTable();
        //        query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no>" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY doc_no asc  ";
        //        ds = clsDAL.SimpleQuery(query);
        //        if (ds != null)
        //        {
        //            if (ds.Tables.Count > 0)
        //            {
        //                dt = ds.Tables[0];
        //                if (dt.Rows.Count > 0)
        //                {
        //                    //next record exist
        //                    btnNext.Enabled = true;
        //                    btnLast.Enabled = true;
        //                }
        //                else
        //                {
        //                    //next record does not exist
        //                    btnNext.Enabled = false;
        //                    btnLast.Enabled = false;
        //                }
        //            }
        //        }
        //        ds = new DataSet();
        //        dt = new DataTable();
        //        query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no<" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  ORDER BY doc_no asc  ";
        //        ds = clsDAL.SimpleQuery(query);
        //        if (ds != null)
        //        {
        //            if (ds.Tables.Count > 0)
        //            {
        //                dt = ds.Tables[0];
        //                if (dt.Rows.Count > 0)
        //                {
        //                    //previous record exist
        //                    btnPrevious.Enabled = true;
        //                    btnFirst.Enabled = true;
        //                }
        //                else
        //                {
        //                    btnPrevious.Enabled = false;
        //                    btnFirst.Enabled = false;
        //                }
        //            }
        //        }
        //        #endregion
        //    }
        //}

        #endregion
    }
    #endregion

    #region [First]
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        try
        {
            //string query = "";
            //query = "select top (1) doc_no from " + tblHead + " where doc_no=(select MIN(doc_no) from " + tblHead + " where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ")";
            //hdnf.Value = clsCommon.getString(query);
            //navigateRecord();
            string query = "";
            query = "select doc_no from " + tblHead + " where doc_no=(select MIN(doc_no) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
                string query = "";
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no< " + Convert.ToInt32(hdnf.Value) + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY doc_no desc  ";
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
                string query = "";
                query = "SELECT top 1 [doc_no] from " + tblHead + " where doc_no> " + Convert.ToInt32(hdnf.Value) + " and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY doc_no asc  ";
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
            query = "select doc_no from " + tblHead + " where doc_no=(select MAX(doc_no) from " + tblHead + " where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ")";
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
        setFocusControl(txtdoc_date);
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
                    string currentDoc_No = lblUtr_Id.Text;
                    DataSet ds = new DataSet();
                    string strrev = "";

                    string refutr = clsCommon.getString("select detailutrno from qrydoheaddetail where detailutrno=" + txtdoc_no.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " " +
                        " and Year_Code=" + Session["year"].ToString() + "");

                    string doid = clsCommon.getString("select doc_no from qrydoheaddetail where detailutrno=" + txtdoc_no.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " " +
                        " and Year_Code=" + Session["year"].ToString() + "");
                    if (refutr == "0")
                    {

                        Head_Delete = "delete from " + tblHead + " where utrid='" + lblUtr_Id.Text + "'";
                        string Detail_Deleteqry = "delete from " + tblDetails + " where utrid='" + lblUtr_Id.Text + "'";
                        GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='UT' and Doc_No=" + txtdoc_no.Text + " and " +
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

                        //Thread thred = new Thread(() => { count = DataStore(flag); }); //Calling DataStore Method Using Thread
                        //thred.Start(); //Thread Operation Start
                        //thred.Join();
                        msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);
                        if (msg == "Delete")
                        {
                            Response.Redirect("../Transaction/PgeUTRHeadUtility.aspx");
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Is In Use DO: " + doid + "')", true);
                    }
                }
                else
                {
                    lblMsg.Text = "Cannot delete this Group , it is in use";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                }
            }
            else
            {
                hdnf.Value = lblUtr_Id.Text;
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
        string max = clsCommon.getString("select ifnull(max(utrid),0) as id from " + tblHead + " where Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + " ");
        hdnf.Value = max;
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
                                lblCreatedDate.Text = "";
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

                        txtdoc_no.Text = dt.Rows[0]["doc_no"].ToString();
                        lblUtr_Id.Text = hdnf.Value;
                        txtdoc_date.Text = dt.Rows[0]["doc_dateConverted"].ToString();
                        txtbank_ac.Text = dt.Rows[0]["bank_ac"].ToString();
                        lblbank_Name.Text = dt.Rows[0]["bankname"].ToString();
                        txtmill_code.Text = dt.Rows[0]["mill_code"].ToString();
                        lblmill_name.Text = dt.Rows[0]["millname"].ToString();
                        // txtlot_no.Text = dt.Rows[0]["lot_no"].ToString();

                        //txtlot_no.Text = dt.Rows[0]["Lott_No"].ToString();

                        //if (txtlot_no.Text != "0")
                        //{
                        //    lblyear_code.Text = dt.Rows[0]["Year_Code"].ToString();
                        //    lblCompnycode.Text = dt.Rows[0]["Company_Code"].ToString();
                        //}
                        //else
                        //{
                        //    lblyear_code.Text = string.Empty;
                        //    lblCompnycode.Text = string.Empty;
                        //}
                        //lblyear_code.Text = dt.Rows[0][""].ToString();
                        txtamount.Text = dt.Rows[0]["amount"].ToString();
                        txtunt_no.Text = dt.Rows[0]["utr_no"].ToString();
                        txtnarration_header.Text = dt.Rows[0]["narration_header"].ToString();
                        txtnarration_footer.Text = dt.Rows[0]["narration_footer"].ToString();
                        string IsSave = dt.Rows[0]["IsSave"].ToString();


                        if (IsSave == "1")
                        {
                            chkIsSave.Checked = true;
                        }
                        else
                        {
                            chkIsSave.Checked = false;
                        }
                        recordExist = true;
                        lblMsg.Text = "";
                        #region Details
                        qry = "";
                        ds = clsDAL.SimpleQuery("select Detail_Id, lot_no,lotYear_Code as lot_no_Name,lotCompany_Code as lot_no_comp,utrgradename as grade_no," +
 " detailamount as amount,Adjusted_Amt,'0' as ltno,utrdetailid as UtrDetailID from qryutrheaddetail where Detail_ID is not null and Company_Code='"
                   + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString())
     + "' and utrid=" + hdnf.Value);
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
            hdnf.Value = txtdoc_no.Text;
            columnTotal();
            this.enableDisableNavigateButtons();
            return recordExist;
        }
        catch
        {
            return false;
        }
    }
    #endregion
    #region csCalculations
    private void csCalculations()
    {
        try
        {
            if (strTextBox == "txtEditDoc_No")
            {
                setFocusControl(txtEditDoc_No);
            }
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
                                                txtdoc_no.Enabled = false;
                                                hdnf.Value = txtdoc_no.Text;
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
                                                setFocusControl(txtdoc_date);
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
                #region
                //try
                //{
                //    string dt = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                //    //if (clsCommon.isValidDateforOp(dt) == true || dt == "")
                //    if (dt == "")
                //    {
                //        setFocusControl(txtdoc_date);
                //    }
                //    else
                //    {
                //        txtdoc_date.Text = "";
                //        setFocusControl(txtdoc_date);
                //    }
                //}
                //catch
                //{
                //    txtdoc_date.Text = "";
                //    setFocusControl(txtdoc_date);
                //}
                #endregion

                if (txtdoc_date.Text != string.Empty)
                {
                    string dt = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    if (clsCommon.isValidDate(dt) == true)
                    {
                        setFocusControl(txtbank_ac);
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
            if (strTextBox == "txtbank_ac")
            {
                if (txtbank_ac.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtbank_ac.Text);
                    if (a == false)
                    {
                        btntxtbank_ac_Click(this, new EventArgs());
                    }
                    else
                    {
                        string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtbank_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (str != string.Empty)
                        {
                            lblbank_Name.Text = str;
                            setFocusControl(txtmill_code);
                        }
                        else
                        {
                            lblbank_Name.Text = str;
                            txtbank_ac.Text = string.Empty;
                            setFocusControl(txtbank_ac);

                        }
                    }
                }
                else
                {
                    lblbank_Name.Text = string.Empty;
                    txtbank_ac.Text = string.Empty;
                    setFocusControl(txtbank_ac);
                }
            }
            if (strTextBox == "txtmill_code")
            {
                if (txtmill_code.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtmill_code.Text);
                    if (a == false)
                    {
                        btntxtmill_code_Click(this, new EventArgs());
                    }
                    else
                    {
                        string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtmill_code.Text
                            + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_type IN ('M','P','T')");
                        if (str != string.Empty)
                        {
                            lblmill_name.Text = str;
                            setFocusControl(txtamount);
                        }
                        else
                        {
                            lblmill_name.Text = str;
                            txtmill_code.Text = string.Empty;
                            setFocusControl(txtmill_code);

                        }
                    }
                }
                else
                {
                    lblmill_name.Text = string.Empty;
                    txtmill_code.Text = string.Empty;
                    setFocusControl(txtmill_code);
                }
            }
            //if (strTextBox == "txtlot_no")
            //{
            //    if (txtlot_no.Text != string.Empty)
            //    {
            //        bool a = clsCommon.isStringIsNumeric(txtlot_no.Text);
            //        if (a == false)
            //        {
            //            btntxtlot_no_Click(this, new EventArgs());
            //        }
            //        else
            //        {
            //            //string str = clsCommon.getString("select Year_Code from [NT_1_qryTenderbalancereport]  where [Tender_No]=" + txtlot_no.Text
            //            //    + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            //            //string str1 = clsCommon.getString("select [Company_Code] from [NT_1_qryTenderbalancereport]  where [Tender_No]=" + txtlot_no.Text
            //            //   + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            //            string str1 =Session["Company_Code"].ToString();
            //            if (str1 != string.Empty)
            //            {
            //                lblyear_code.Text = hdyearcode.Value;
            //                lblCompnycode.Text = str1.ToString(); ;
            //                setFocusControl(txtamount);
            //            }
            //            else
            //            {
            //              //  lblyear_code.Text = str;
            //                lblCompnycode.Text = str1.ToString();
            //                txtlot_no.Text = string.Empty;
            //                setFocusControl(txtlot_no);

            //            }
            //        }
            //    }
            //    else
            //    {
            //        lblyear_code.Text = string.Empty;
            //        lblCompnycode.Text = string.Empty;
            //        txtlot_no.Text = string.Empty;
            //        setFocusControl(txtlot_no);
            //    }
            //}
            if (strTextBox == "txtamount")
            {
                setFocusControl(txtunt_no);
            }
            if (strTextBox == "txtunt_no")
            {
                setFocusControl(txtnarration_header);
            }
            if (strTextBox == "txtnarration_header")
            {
                setFocusControl(txtnarration_footer);
            }
            if (strTextBox == "txtnarration_footer")
            {
                setFocusControl(txtlotno_Detail);
            }
            if (strTextBox == "txtlotno_Detail")
            {
                if (txtlotno_Detail.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtlotno_Detail.Text);
                    if (a == false)
                    {
                        btntxtlot_no_Detail_Click(this, new EventArgs());
                    }
                    else
                    {
                        //string str = clsCommon.getString("select Year_Code from [NT_1_qryTenderbalancereport]  where [Tender_No]=" + txtlotno_Detail.Text
                        //    + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        string grade = clsCommon.getString("select Grade from qrymillpaymentbalance  where Tender_No=" + txtlotno_Detail.Text
                           + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                        string str1 = Session["Company_Code"].ToString();
                        if (str1 != string.Empty)
                        {
                            lblyear_Code_Detail.Text = hdyearcodedetail.Value;
                            lblCompnycode_Detail.Text = str1;
                            txtgrade_no.Text = grade;
                            txtamount_Detail.Text = hdnfpodetailid.Value;
                            setFocusControl(txtgrade_no);
                        }
                        else
                        {
                            lblyear_Code_Detail.Text = hdyearcodedetail.Value;
                            lblCompnycode_Detail.Text = str1;
                            txtlotno_Detail.Text = string.Empty;
                            setFocusControl(txtlotno_Detail);

                        }
                    }
                }
                else
                {
                    lblyear_Code_Detail.Text = string.Empty;
                    lblCompnycode_Detail.Text = string.Empty;
                    txtlotno_Detail.Text = string.Empty;
                    setFocusControl(txtlotno_Detail);
                }
            }
            if (strTextBox == "txtgrade_no")
            {
                setFocusControl(txtamount_Detail);
            }
            if (strTextBox == "txtamount_Detail")
            {
                setFocusControl(txtAdjusted_Amt);
            }

        }
        catch
        {
        }
    }
    #endregion
    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = " select * from " + qryCommon + " where utrid='" + hdnf.Value + "' ";
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
                        int n = Convert.ToInt32(lblNo.Text);
                        rowIndex = Convert.ToInt32(lblID.Text);   //auto no
                        dr = (DataRow)dt.Rows[n - 1];
                        dr["Detail_Id"] = rowIndex;
                        dr["SrNo"] = 0;
                        #region decide whether actual row is updating or virtual [rowAction]
                        string id = clsCommon.getString("select Detail_Id from " + tblDetails + " where Detail_Id='" + lblID.Text + "' " +
                            " and doc_no=" + txtdoc_no.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " +
                            " and Year_Code=" + Session["year"].ToString() + "");
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
                    dt.Columns.Add((new DataColumn("lot_no", typeof(int))));
                    dt.Columns.Add((new DataColumn("lot_no_Name", typeof(string))));
                    dt.Columns.Add((new DataColumn("lot_no_comp", typeof(string))));

                    dt.Columns.Add((new DataColumn("grade_no", typeof(string))));
                    // dt.Columns.Add((new DataColumn("grade_no_Name", typeof(string))));
                    dt.Columns.Add((new DataColumn("amount", typeof(double))));
                    dt.Columns.Add((new DataColumn("Adjusted_Amt", typeof(double))));
                    dt.Columns.Add((new DataColumn("ltno", typeof(int))));
                    dt.Columns.Add((new DataColumn("UtrDetailId", typeof(int))));
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
                dt.Columns.Add((new DataColumn("lot_no", typeof(int))));
                dt.Columns.Add((new DataColumn("lot_no_Name", typeof(string))));
                dt.Columns.Add((new DataColumn("lot_no_comp", typeof(string))));

                dt.Columns.Add((new DataColumn("grade_no", typeof(string))));
                //dt.Columns.Add((new DataColumn("grade_no_Name", typeof(string))));
                dt.Columns.Add((new DataColumn("amount", typeof(double))));
                dt.Columns.Add((new DataColumn("Adjusted_Amt", typeof(double))));
                dt.Columns.Add((new DataColumn("ltno", typeof(int))));
                dt.Columns.Add((new DataColumn("UtrDetailId", typeof(int))));
                #endregion
                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                dt.Columns.Add((new DataColumn("SrNo", typeof(int))));
                dr = dt.NewRow();
                dr["Detail_Id"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;
            }
            #region [ Set values to dr]
            dr["lot_no"] = txtlotno_Detail.Text;
            dr["lot_no_Name"] = lblyear_Code_Detail.Text;
            dr["lot_no_comp"] = lblCompnycode_Detail.Text;

            dr["grade_no"] = txtgrade_no.Text;
            //dr["grade_no_Name"] = lblgrade_name.Text;
            dr["amount"] = Convert.ToDouble(txtamount_Detail.Text);

            if (txtAdjusted_Amt.Text != string.Empty)
            {
                dr["Adjusted_Amt"] = Convert.ToDouble(txtAdjusted_Amt.Text);
            }
            else
            {
                dr["Adjusted_Amt"] = 0;
            }
            dr["LTNo"] = 0;

            #endregion
            if (btnAdddetails.Text == "ADD")
            {
                dr["UtrDetailId"] = 0;
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
                pnlPopupDetails.Style["display"] = "none";
                setFocusControl(txtlotno_Detail);
            }
            else
            {
                pnlPopupDetails.Style["display"] = "none";
                setFocusControl(txtlotno_Detail);
                btnOpenDetailsPopup.Focus();
            }
            // Empty Code->
            txtlotno_Detail.Text = string.Empty;
            lblyear_Code_Detail.Text = string.Empty;
            lblCompnycode_Detail.Text = string.Empty;
            txtgrade_no.Text = string.Empty;
            txtamount_Detail.Text = string.Empty;
            txtAdjusted_Amt.Text = string.Empty;
            btnAdddetails.Text = "ADD";
            columnTotal();
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
        txtlotno_Detail.Text = string.Empty;
        lblyear_Code_Detail.Text = string.Empty;
        lblCompnycode_Detail.Text = string.Empty;
        txtgrade_no.Text = string.Empty;
        txtamount_Detail.Text = string.Empty;
        txtAdjusted_Amt.Text = string.Empty;
        btnAdddetails.Text = "ADD";
        setFocusControl(txtlotno_Detail);
        pnlPopupDetails.Style["display"] = "none";
    }
    #endregion

    #region [showDetailsRow]
    private void showDetailsRow(GridViewRow gvrow)
    {
        lblNo.Text = Server.HtmlDecode(gvrow.Cells[Srno].Text);//srno row id;
        lblID.Text = Server.HtmlDecode(gvrow.Cells[2].Text);//Detail id;
        txtlotno_Detail.Text = Server.HtmlDecode(gvrow.Cells[lot_no].Text);
        lblyear_Code_Detail.Text = Server.HtmlDecode(gvrow.Cells[4].Text);
        hdyearcodedetail.Value = Server.HtmlDecode(gvrow.Cells[4].Text);
        lblCompnycode_Detail.Text = Server.HtmlDecode(gvrow.Cells[5].Text);

        txtgrade_no.Text = Server.HtmlDecode(gvrow.Cells[grade_no].Text);
        txtamount_Detail.Text = Server.HtmlDecode(gvrow.Cells[amount].Text);
        txtAdjusted_Amt.Text = Server.HtmlDecode(gvrow.Cells[Adj_Amt].Text);

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

                string IDExisting = clsCommon.getString("select Detail_Id from " + tblDetails + " where doc_no='" + hdnf.Value + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
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
            // if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            // e.Row.Cells[2].Width = new Unit("120px");
            e.Row.Cells[0].ControlStyle.Width = new Unit("30px");
            e.Row.Cells[1].ControlStyle.Width = new Unit("40px");
            e.Row.Cells[2].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[Rowaction].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[Srno].ControlStyle.Width = new Unit("70px");
            //--------------------------------------------------
            e.Row.Cells[lot_no].ControlStyle.Width = new Unit("100px");
            e.Row.Cells[lot_no].Style["overflow"] = "hidden";
            e.Row.Cells[lot_no].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[4].ControlStyle.Width = new Unit("50px");
            e.Row.Cells[4].Style["overflow"] = "hidden";
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[5].ControlStyle.Width = new Unit("50px");
            e.Row.Cells[5].Style["overflow"] = "hidden";
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Left;
            //--------------------------------------------------
            e.Row.Cells[grade_no].ControlStyle.Width = new Unit("120px");
            e.Row.Cells[grade_no].Style["overflow"] = "hidden";
            e.Row.Cells[grade_no].HorizontalAlign = HorizontalAlign.Left;



            //--------------------------------------------------
            e.Row.Cells[amount].ControlStyle.Width = new Unit("120px");
            e.Row.Cells[amount].Style["overflow"] = "hidden";
            e.Row.Cells[amount].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[Adj_Amt].ControlStyle.Width = new Unit("120px");
            e.Row.Cells[Adj_Amt].Style["overflow"] = "hidden";
            e.Row.Cells[Adj_Amt].HorizontalAlign = HorizontalAlign.Left;

            e.Row.Cells[ltno].ControlStyle.Width = new Unit("120px");
            e.Row.Cells[ltno].Style["overflow"] = "hidden";
            e.Row.Cells[ltno].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[Rowaction].Visible = true;
            e.Row.Cells[Srno].Visible = true;

            e.Row.Cells[UtrDetailID].ControlStyle.Width = new Unit("120px");
            e.Row.Cells[UtrDetailID].Style["overflow"] = "hidden";
            e.Row.Cells[UtrDetailID].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[UtrDetailID].Visible = true;
            //--------------------------------------------------
            //     e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[0].Style["overflow" ] = "hiden";
            //    e.Row.Cells[0].Visible =true;
            //}
        }
        catch
        {
        }
    }
    #endregion
    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //string v = hdnfClosePopup.Value;
        //if (e.Row.RowType == DataControlRowType.Pager)
        //{
        //    e.Row.Cells[0].Width = new Unit("120px");
        //    e.Row.Cells[0].ControlStyle.Width = new Unit("120px");
        //    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        //    //    e.Row.Cells[0].Style["overflow" ] = "hidden";
        //    //    e.Row.Cells[0].Visible =true;
        //}string v = hdnfClosePopup.Value;

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    e.Row.Cells[0].Width = new Unit("80px");
        //    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        //}
        string v = hdnfClosePopup.Value;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

        }
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            if (v == "txtDoc_No" || v == "txtEditDoc_No")
            {
                e.Row.Cells[0].ControlStyle.Width = new Unit("30px");
                //e.Row.Cells[1].ControlStyle.Width = new Unit("100px");
                //e.Row.Cells[2].ControlStyle.Width = new Unit("100px");
                //e.Row.Cells[3].ControlStyle.Width = new Unit("100px");

            }
            if (v == "txtbank_ac" || v == "txtmill_code" || v == "txtlot_no")
            {
                e.Row.Cells[0].ControlStyle.Width = new Unit("30px");
                e.Row.Cells[1].ControlStyle.Width = new Unit("200px");
                e.Row.Cells[2].ControlStyle.Width = new Unit("100px");
            }
            if (v == "txtgrade_no")
            {
                e.Row.Cells[0].ControlStyle.Width = new Unit("100px");


            }
            if (v == "txtlotno_Detail")
            {
                e.Row.Cells[0].ControlStyle.Width = new Unit("50px");
                e.Row.Cells[1].ControlStyle.Width = new Unit("90px");
                e.Row.Cells[2].ControlStyle.Width = new Unit("90px");

                e.Row.Cells[3].ControlStyle.Width = new Unit("90px");
                e.Row.Cells[4].ControlStyle.Width = new Unit("200px");
                e.Row.Cells[5].ControlStyle.Width = new Unit("120px");

                e.Row.Cells[6].ControlStyle.Width = new Unit("120px");
                e.Row.Cells[7].ControlStyle.Width = new Unit("90px");
                e.Row.Cells[8].ControlStyle.Width = new Unit("150px");
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
                            setFocusControl(txtlotno_Detail);
                        }
                        break;
                    case "DeleteRecord":
                        string action = "";

                        //GridViewRow gridViewRow1 = grdDetail.Rows[rowindex];
                        //int rowIndex = gridViewRow1.RowIndex;
                        DataTable dt1 = (DataTable)ViewState["currentTable"];
                        int refid = Convert.ToInt32(dt1.Rows[rowindex]["UtrDetailID"].ToString());

                        string utrdoid = clsCommon.getString("select utrdetailid from qrydodetail where utrdetailid=" + refid + "");
                        string doid = clsCommon.getString("select doc_no from qrydodetail where utrdetailid=" + refid + "");
                        if (utrdoid == string.Empty)
                        {
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
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('this Record Use In DO: " + doid + "!')", true);
                        }
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
    #endregion
    #region [txtdoc_no_TextChanged]
    protected void txtdoc_no_TextChanged(object sender, EventArgs e)
    {
        searchString = txtdoc_no.Text;
        strTextBox = "txtdoc_no";
        csCalculations();
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
    #region [txtbank_ac_TextChanged]
    protected void txtbank_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtbank_ac.Text;
        strTextBox = "txtbank_ac";
        csCalculations();
    }
    #endregion
    #region [btntxtbank_ac_Click]
    protected void btntxtbank_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtbank_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion
    #region [txtmill_code_TextChanged]
    protected void txtmill_code_TextChanged(object sender, EventArgs e)
    {
        searchString = txtmill_code.Text;
        strTextBox = "txtmill_code";
        csCalculations();
    }
    #endregion
    #region [btntxtmill_code_Click]
    protected void btntxtmill_code_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtmill_code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion
    #region [txtlot_no_TextChanged]
    //protected void txtlot_no_TextChanged(object sender, EventArgs e)
    //{
    //    searchString = txtlot_no.Text;
    //    strTextBox = "txtlot_no";
    //    csCalculations();
    //}
    #endregion
    #region [btntxtlot_no_Click]
    protected void btntxtlot_no_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtlot_no";
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
        //searchString = txtamount.Text;
        strTextBox = "txtamount";
        csCalculations();
    }
    #endregion


    #region [txtlotno_Detail_TextChanged]
    protected void txtlotno_Detail_TextChanged(object sender, EventArgs e)
    {
        searchString = txtlotno_Detail.Text;
        strTextBox = "txtlotno_Detail";
        csCalculations();
    }
    #endregion

    #region [btntxtlot_no_Detail_Click]
    protected void btntxtlot_no_Detail_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtlotno_Detail";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtgrade_no_TextChanged]
    protected void txtgrade_no_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtgrade_no.Text;
        //strTextBox = "txtgrade_no";
        //csCalculations();
        // searchString = txtgrade_no.Text;
        if (txtgrade_no.Text != string.Empty)
        {
            bool a = true;
            if (txtgrade_no.Text.Length < 8)
            {
                a = clsCommon.isStringIsNumeric(txtgrade_no.Text);
                //  pnlPopup.Style["display"] = "none";
            }
            if (a == false)
            {
                btntxtgrade_no_Click(this, new EventArgs());
                pnlPopup.Style["display"] = "none";
            }
            else
            {
                pnlPopup.Style["display"] = "none";
                setFocusControl(txtamount_Detail);
            }
            setFocusControl(txtgrade_no);
        }
    }
    #endregion

    #region [btntxtgrade_no_Click]
    protected void btntxtgrade_no_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtgrade_no";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }

    }
    #endregion

    #region [txtamount_Detai]
    protected void txtamount_Detail_TextChanged(object sender, EventArgs e)
    {

        searchString = txtamount_Detail.Text;

        strTextBox = "txtamount_Detail";
        csCalculations();
    }
    #endregion
    #region [txtAdjusted_Amt]
    protected void txtAdjusted_Amt_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAdjusted_Amt.Text;
        strTextBox = "txtAdjusted_Amt";
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

    private void columnTotal()
    {
        double GridAmount = 0.00;
        double GridtotalA = 0.00;
        double DiffAmount = 0.00;

        double HeadAmount = Convert.ToDouble(txtamount.Text);
        for (int i = 0; i < grdDetail.Rows.Count; i++)
        {
            GridAmount = Convert.ToDouble(grdDetail.Rows[i].Cells[amount].Text);
            GridtotalA += GridAmount;


        }
        lblGridTotal.Text = Convert.ToString(GridtotalA);
        DiffAmount = HeadAmount - GridtotalA;
        lblDiff.Text = Convert.ToString(DiffAmount);
    }

    #region [btnSearch_Click]
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        #region
        //try
        //{
        //    string searchtxt = "";
        //    string delimStr = "";
        //    char[] delimiter = delimStr.ToCharArray();
        //    string words = "";
        //    string[] split = null;
        //    string name = string.Empty;
        //    if (searchString != string.Empty && strTextBox == hdnfClosePopup.Value)
        //    {
        //        txtSearchText.Text = searchString;
        //        searchtxt = txtSearchText.Text;
        //        words = txtSearchText.Text;
        //        split = words.Split(delimiter);
        //    }
        //    else
        //    {
        //        txtSearchText.Text = txtSearchText.Text;
        //        searchtxt = txtSearchText.Text;
        //        words = txtSearchText.Text;
        //        split = words.Split(delimiter);
        //    }
        //    if (hdnfClosePopup.Value == "txtdoc_no" || hdnfClosePopup.Value == "txtEditDoc_No")
        //    {
        //        if (btntxtdoc_no.Text == "Change No")
        //        {
        //            pnlPopup.Style["display"] = "none";
        //            txtdoc_no.Text = string.Empty;
        //            txtdoc_no.Enabled = true;
        //            btnSave.Enabled = false;
        //            setFocusControl(txtdoc_no);
        //            hdnfClosePopup.Value = "Close";
        //        }
        //        if (btntxtdoc_no.Text == "Choose No")
        //        {
        //            foreach (var s in split)
        //            {
        //                string aa = s.ToString();
        //                name += "doc_no Like '%" + aa + "%'or";
        //            }
        //            name = name.Remove(name.Length - 2);
        //            lblPopupHead.Text = "--Select Group--";
        //            string qry = " select doc_no from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and (" + name + ") order by doc_no";
        //            this.showPopup(qry);
        //        }
        //    }

        //    if (hdnfClosePopup.Value == "txtbank_ac")
        //    {
        //        lblPopupHead.Text = "--Select--";
        //        string qry = "";
        //        this.showPopup(qry);
        //    }
        //    if (hdnfClosePopup.Value == "txtmill_code")
        //    {
        //        lblPopupHead.Text = "--Select--";
        //        string qry = "";
        //        this.showPopup(qry);
        //    }
        //    if (hdnfClosePopup.Value == "txtlot_no")
        //    {
        //        lblPopupHead.Text = "--Select--";
        //        string qry = "";
        //        this.showPopup(qry);
        //    }
        //    if (hdnfClosePopup.Value == "txtlot_no")
        //    {
        //        lblPopupHead.Text = "--Select--";
        //        string qry = "";
        //        this.showPopup(qry);
        //    }
        //    if (hdnfClosePopup.Value == "txtgrade_no")
        //    {
        //        lblPopupHead.Text = "--Select--";
        //        string qry = "";
        //        this.showPopup(qry);
        //    }
        //}
        //catch
        //{
        //}
        #endregion
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
            if (hdnfClosePopup.Value == "txtdoc_no" || hdnfClosePopup.Value == "txtEditDoc_No")
            {

                lblPopupHead.Text = "--Select DOC NO--";
                string qry = "select doc_no,doc_date,utr_no,amount,millnameshort,banknameshort from " + qryCommon + " where Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code="
                    + Convert.ToInt32(Session["year"].ToString()) +
                    " and (doc_no like '%" + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%' or utr_no like '%" + txtSearchText.Text
                    + "%' or millnameshort like '%" + txtSearchText.Text + "%' or banknameshort like '%" + txtSearchText.Text + "%' or amount like  '%"
                    + txtSearchText.Text + "%' )";
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtbank_ac")
            {

                //txtSearchText.Text = txtbank_ac.Text;
                lblPopupHead.Text = "--Select Bank--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from qrymstaccountmaster  where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                    " and Ac_type='B' and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%')  order by Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtmill_code")
            {

                // txtSearchText.Text = txtmill_code.Text;
                lblPopupHead.Text = "--Select--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from qrymstaccountmaster where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                   " and Ac_type IN ('M','P') and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtlot_no")
            {
                lblPopupHead.Text = "--Select--";
                //string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                //   " and Ac_type IN ('M','P','T') and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%"
                //   + txtSearchText.Text + "%') order by Ac_Name_E";


                //string qry = "select [Tender_No],[Tender_Date],[Quantal],[mr],[millamount],[received],[balance] from [NT_1_qryTenderbalancereport]"
                //    + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +" and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                //    "and [Payment_To]=" + txtmill_code.Text + " and  [Mill_Code]=" + txtmill_code.Text;

                string qry = "select [Tender_No],[Tender_Date],[Quantal],[mr],[millamount],[received],[balance],[Year_Code] from [NT_1_qryTenderbalancereport]"
                + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                "and [Payment_To]=" + txtmill_code.Text;


                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtlotno_Detail")
            {

                lblPopupHead.Text = "--Select--";

                if (btnSave.Text == "Save")
                {
                    qry = "select Tender_No,Tender_DateConverted as Tender_Date,Quantal,Mill_Rate ,millamount,paidamount,payableamount,Year_Code,Grade from qrymillpaymentbalance"
                       + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                       "  and payableamount !=0 and Payment_To=" + txtmill_code.Text;
                }
                else
                {

                    qry = "select Tender_No,Tender_DateConverted as Tender_Date,Quantal,Mill_Rate ,millamount,paidamount,payableamount,Year_Code,Grade from qrymillpaymentbalance"
                   + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                   " and Payment_To=" + txtmill_code.Text;
                }
                this.showPopup(qry);
            }


            if (hdnfClosePopup.Value == "txtgrade_no")
            {
                //tdDate.Visible = false;
                //txtSearchText.Text = txtgrade_no.Text;
                lblPopupHead.Text = "--Select Grade--";
                string qry = "select  System_Name_E from " + tblPrefix + "SystemMaster where System_Type='S' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and System_Name_E like '%" + txtSearchText.Text + "%' ";
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
        try
        {
            #region [Validation Part]
            bool isValidated = true;
            //        if textbox is date then if condition will be like this if(clsCommon.isValidDate(txtDoc_Date.Text==true))
            /*  In Grid At Least One Record is required
    int count = 0;
      if (grdDetail.Rows.Count == 0)
    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),"","alert('Please Enter Details!!!!             isValidated = false;
    setFocusControl(btnOpenDetailsPopup); 
    return;  
    }
    if (grdDetail.Rows.Count >= 1) 
    for (int i = 0; i < grdDetail.Rows.Count; i++)
    {
    if (grdDetail.Rows[i].Cells[10].Text == "D")
    {
    count++; 
    }
    }
    if (grdDetail.Rows.Count == count)
    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),"","alert('Please Enter Minumun One Details!!!!          isValidated = false; 
    setFocusControl(btnOpenDetailsPopup); 
    return;
    } 
    }
    */
            #endregion
            if (Convert.ToDouble(lblDiff.Text) != 0.00)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('diif must bo zero!');", true);
                setFocusControl(txtamount_Detail);
                return;

            }
            else
            {
                isValidated = true;
            }


            #region Assign Values to Variables
            IsSave = 0;
            if (chkIsSave.Checked == true)
            {
                IsSave = 1;
            }
            Doc_No = Convert.ToInt32(txtdoc_no.Text != string.Empty ? txtdoc_no.Text : "0");
            doc_date = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            bank_ac = txtbank_ac.Text != string.Empty ? txtbank_ac.Text : "0";
            try
            {
                bank_id = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + bank_ac + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
            }
            catch
            {
            }
            mill_code = Convert.ToInt32(txtmill_code.Text != string.Empty ? txtmill_code.Text : "0");
            try
            {
                mill_id = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as acoid from qrymstaccountmaster where Ac_Code=" + mill_code + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
            }
            catch
            {
            }
            Amount = Convert.ToDouble(txtamount.Text != string.Empty ? txtamount.Text : "0.00");
            utr_no = txtunt_no.Text;
            narration_header = txtnarration_header.Text;
            narration_footer = txtnarration_footer.Text;
            Lott_No = "0";

            Company_Code = Convert.ToInt32(Convert.ToInt32(Session["Company_Code"].ToString()));
            Year_Code = Convert.ToInt32(Convert.ToInt32(Session["year"].ToString()));
            Branch_Code = Convert.ToInt32(Convert.ToInt32(Session["Branch_Code"].ToString()));
            Created_By = Session["user"].ToString();
            Modified_By = Session["user"].ToString();
            Created_Date = DateTime.Now.ToString("yyyy/MM/dd");
            Modified_Date = DateTime.Now.ToString("yyyy/MM/dd");
            retValue = string.Empty;

            #endregion
            #region Detail Fields
            Detail_Fields.Append("doc_no,");
            Detail_Fields.Append("Company_Code,");
            Detail_Fields.Append("Year_Code,");

            Detail_Fields.Append("Detail_Id,");
            Detail_Fields.Append("lot_no,");
            Detail_Fields.Append("lotCompany_Code,");
            Detail_Fields.Append("lotYear_Code,");
            Detail_Fields.Append("grade_no,");
            Detail_Fields.Append("amount,");
            Detail_Fields.Append("Adjusted_Amt,");
            Detail_Fields.Append("utrdetailid,");
            Detail_Fields.Append("utrid,");
            Detail_Fields.Append("LTNo,");
            Detail_Fields.Append("ln");
            #endregion
            FormTypes types = new FormTypes();

            if (btnSave.Text == "Save")
            {
                this.NextNumber();

                #region Head Part
                Head_Fields.Append("doc_no,");
                Head_Values.Append("'" + Doc_No + "',");
                Head_Fields.Append("doc_date,");
                Head_Values.Append("'" + doc_date + "',");
                Head_Fields.Append("bank_ac,");
                Head_Values.Append("'" + bank_ac + "',");
                Head_Fields.Append("mill_code,");
                Head_Values.Append("'" + mill_code + "',");
                Head_Fields.Append("amount,");
                Head_Values.Append("'" + Amount + "',");
                Head_Fields.Append("utr_no,");
                Head_Values.Append("'" + utr_no + "',");
                Head_Fields.Append("narration_header,");
                Head_Values.Append("'" + narration_header + "',");
                Head_Fields.Append("narration_footer,");
                Head_Values.Append("'" + narration_footer + "',");
                Head_Fields.Append("Company_Code,");
                Head_Values.Append("'" + Company_Code + "',");
                Head_Fields.Append("Year_Code,");
                Head_Values.Append("'" + Year_Code + "',");
                Head_Fields.Append("Branch_Code,");
                Head_Values.Append("'" + Branch_Code + "',");
                Head_Fields.Append("Created_By,");
                Head_Values.Append("'" + Created_By + "',");
                Head_Fields.Append("IsSave,");
                Head_Values.Append("'" + IsSave + "',");
                Head_Fields.Append("Lott_No,");
                Head_Values.Append("'" + Lott_No + "',");
                Head_Fields.Append("utrid,");
                Head_Values.Append("'" + id + "',");
                Head_Fields.Append("ba,");
                Head_Values.Append("case when 0='" + bank_id + "' then null else '" + bank_id + "' end,");
                Head_Fields.Append("mc");
                Head_Values.Append("case when 0='" + mill_id + "' then null else '" + mill_id + "' end");

                #endregion End Head assign Field And Values Part
                Head_Insert = "insert into " + tblHead + "(" + Head_Fields + ") values(" + Head_Values + ")";
                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Head_Insert;
                Maindt.Rows.Add(dr);

                #region Detail Part
                int utrDetail_Id = Convert.ToInt32(clsCommon.getString("select ifnull(count(utrdetailid),0) as utrdetailid from " + tblDetails + " "));
                if (utrDetail_Id == 0)
                {
                    utrDetail_Id = 0;
                }
                else
                {
                    utrDetail_Id = Convert.ToInt32(clsCommon.getString("select max(utrdetailid) as utrdetailid from " + tblDetails + " "));
                }
                #region Create Main Logic Detail
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    utrDetail_Id = utrDetail_Id + 1;
                    Grid_Id = Convert.ToInt32(grdDetail.Rows[i].Cells[Detail_Id].Text);
                    Grid_lot_no = Convert.ToInt32(grdDetail.Rows[i].Cells[lot_no].Text);
                    int tenderid = Convert.ToInt32(clsCommon.getString("select tenderid from nt_1_tender where Tender_No='" + Grid_lot_no + "'"));

                    lotCompany_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[lot_no_comp].Text);
                    lotYear_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[lot_no_Name].Text);
                    Grid_grade_no = grdDetail.Rows[i].Cells[grade_no].Text;
                    Grid_amount = Convert.ToDouble(grdDetail.Rows[i].Cells[amount].Text);
                    Adjusted_Amt = Convert.ToDouble(grdDetail.Rows[i].Cells[Adj_Amt].Text);
                    LTNo = Convert.ToInt32(grdDetail.Rows[i].Cells[ltno].Text);

                    Detail_Values.Append("('" + Doc_No + "','" + Company_Code + "','" + Year_Code + "','" + Grid_Id + "','" + Grid_lot_no + "'," +
                    " '" + lotCompany_Code + "','" + lotYear_Code + "','" + Grid_grade_no + "','" + Grid_amount + "','" + Adjusted_Amt + "','" + utrDetail_Id + "','" + id + "','" + LTNo + "','" + tenderid + "'),");
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

                //if (count == 1)
                //{
                //    hdnf.Value = id.ToString();
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
                Head_Update.Append("doc_no=");
                Head_Update.Append("'" + Doc_No + "',");
                Head_Update.Append("doc_date=");
                Head_Update.Append("'" + doc_date + "',");
                Head_Update.Append("bank_ac=");
                Head_Update.Append("'" + bank_ac + "',");
                Head_Update.Append("mill_code=");
                Head_Update.Append("'" + mill_code + "',");
                Head_Update.Append("amount=");
                Head_Update.Append("'" + Amount + "',");
                Head_Update.Append("utr_no=");
                Head_Update.Append("'" + utr_no + "',");
                Head_Update.Append("narration_header=");
                Head_Update.Append("'" + narration_header + "',");
                Head_Update.Append("narration_footer=");
                Head_Update.Append("'" + narration_footer + "',");
                Head_Update.Append("Modified_By=");
                Head_Update.Append("'" + Modified_By + "',");
                Head_Update.Append("IsSave=");
                Head_Update.Append("'" + IsSave + "',");
                Head_Update.Append("Lott_No=");
                Head_Update.Append("'" + Lott_No + "',");
                Head_Update.Append("ba=");
                Head_Update.Append("'" + bank_id + "',");
                Head_Update.Append("mc=");
                Head_Update.Append("'" + mill_id + "'");

                string Head_Updateqry = "update " + tblHead + " set " + Head_Update + " where utrid=" + lblUtr_Id.Text + "";

                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Head_Updateqry;
                Maindt.Rows.Add(dr);
                #endregion

                string concatid = string.Empty;

                #region Main Logic
                int utrDid = Convert.ToInt32(clsCommon.getString("select max(utrdetailid) as utrdetailid from " + tblDetails + " "));
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    Grid_Id = Convert.ToInt32(grdDetail.Rows[i].Cells[Detail_Id].Text);
                    Grid_lot_no = Convert.ToInt32(grdDetail.Rows[i].Cells[lot_no].Text);
                    int tenderid = Convert.ToInt32(clsCommon.getString("select tenderid from nt_1_tender where Tender_No='" + Grid_lot_no + "'"));

                    lotCompany_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[lot_no_comp].Text);
                    lotYear_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[lot_no_Name].Text);
                    Grid_grade_no = grdDetail.Rows[i].Cells[grade_no].Text;
                    Grid_amount = Convert.ToDouble(grdDetail.Rows[i].Cells[amount].Text);
                    Adjusted_Amt = Convert.ToDouble(grdDetail.Rows[i].Cells[Adj_Amt].Text);
                    LTNo = Convert.ToInt32(grdDetail.Rows[i].Cells[ltno].Text);
                    id = Convert.ToInt32(grdDetail.Rows[i].Cells[10].Text);

                    #region Insert Grid Record
                    if (grdDetail.Rows[i].Cells[Rowaction].Text == "A")
                    {
                        utrDid = utrDid + 1;
                        Detail_Values.Append("('" + Doc_No + "','" + Company_Code + "','" + Year_Code + "','" + Grid_Id + "','" + Grid_lot_no + "'," +
                   " '" + lotCompany_Code + "','" + lotYear_Code + "','" + Grid_grade_no + "','" + Grid_amount + "','" + Adjusted_Amt + "','" + utrDid + "','" + lblUtr_Id.Text + "','" + LTNo + "','" + tenderid + "'),");
                    }
                    #endregion

                    #region Update Grid Record
                    if (grdDetail.Rows[i].Cells[Rowaction].Text == "U")
                    {
                        Detail_Update.Append("lot_no=case utrdetailid when '" + id + "' then '" + Grid_lot_no + "'  ELSE lot_no END,");
                        Detail_Update.Append("grade_no=case utrdetailid when '" + id + "' then '" + Grid_grade_no + "'  ELSE grade_no END,");
                        Detail_Update.Append("amount=case utrdetailid when '" + id + "' then '" + Grid_amount + "'  ELSE amount END,");
                        Detail_Update.Append("lotCompany_Code=case utrdetailid when '" + id + "' then '" + lotCompany_Code + "'  ELSE lotCompany_Code END,");
                        Detail_Update.Append("lotYear_Code=case utrdetailid when '" + id + "' then '" + lotYear_Code + "'  ELSE lotYear_Code END,");
                        Detail_Update.Append("Adjusted_Amt=case utrdetailid when '" + id + "' then '" + Adjusted_Amt + "'  ELSE Adjusted_Amt END,");
                        Detail_Update.Append("ln=case utrdetailid when '" + id + "' then '" + tenderid + "'  ELSE ln END,");
                        Detail_Update.Append("LTNo=case utrdetailid when '" + id + "' then '" + LTNo + "'  ELSE LTNo END,");
                        concatid = concatid + "'" + id + "',";

                    }
                    #endregion

                    #region Delete Grid Record
                    if (grdDetail.Rows[i].Cells[Rowaction].Text == "D")
                    {
                        Detail_Delete.Append("'" + grdDetail.Rows[i].Cells[UtrDetailID].Text + "',");
                    }
                    #endregion
                }
                #endregion

                #region
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
                    string Detail_Deleteqry = "delete from " + tblDetails + " where utrdetailid in(" + Detail_Delete + ")";

                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = Detail_Deleteqry;
                    Maindt.Rows.Add(dr);
                }
                if (Detail_Update.Length > 0)
                {
                    concatid = concatid.Remove(concatid.Length - 1);
                    Detail_Update.Remove(Detail_Update.Length - 1, 1);
                    string Detail_Updateqry = "update " + tblDetails + " set " + Detail_Update + " where utrdetailid in(" + concatid + ")";

                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = Detail_Updateqry;
                    Maindt.Rows.Add(dr);
                }
                #endregion
                flag = 2;


            }

            #region Gledger Effect
            StringBuilder Gledger_values = new StringBuilder();
            GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='UT' and Doc_No=" + Doc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = GLEDGER_Delete;
            Maindt.Rows.Add(dr);
            StringBuilder Gledger_Column = new StringBuilder();

            Gledger_Column.Append("TRAN_TYPE,CASHCREDIT,DOC_NO,DOC_DATE,AC_CODE,UNIT_code,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE," +
                        " YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO,ac,vc,progid,tranid");

            string millShortname = clsCommon.getString("select Short_Name from qrymstaccountmaster where Ac_Code=" + mill_code + " and Company_Code='" + Session["Company_Code"].ToString() + "'");
            string DebitNarration = "UTR NO:" + Doc_No + "' '" + narration_header + "' '" + millShortname + "";
            int ordercode = 0;
            // Mill Code Effect
            if (Amount > 0)
            {
                ordercode = ordercode + 1;
                Gledger_values.Append("('UT','','" + Doc_No + "','" + doc_date + "','" + mill_code + "','0','" + DebitNarration + "','" + Amount + "', " +
                                                    " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + ordercode + "','D','" + bank_ac + "',0,'" + Branch_Code + "','UT','" + Doc_No + "'," +
                                                    " '" + mill_id + "','0','" + types.TT_UTR + "','0')");

            }
            // Bank effect
            if (Amount > 0)
            {
                ordercode = ordercode + 1;
                Gledger_values.Append(",('UT','','" + Doc_No + "','" + doc_date + "','" + bank_ac + "','0','" + DebitNarration + "','" + Amount + "', " +
                                                   " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + ordercode + "','C','" + mill_code + "',0,'" + Branch_Code + "','UT','" + Doc_No + "'," +
                                                   " " + bank_id + ",'0','" + types.TT_UTR + "','0')");

            }
            GLEDGER_Insert = "insert into nt_1_gledger (" + Gledger_Column + ") values " + Gledger_values + " ";
            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = GLEDGER_Insert;
            Maindt.Rows.Add(dr);
            #endregion

            // msg = clsDAL.DataStore(Head_Insert, Head_Update, Head_Delete, Detail_Insert, Detail_Update, Detail_Delete, GLEDGER_Insert, GLEDGER_Delete, flag);
            msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);
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
                hdnf.Value = lblUtr_Id.Text;
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
    #endregion

    protected void btnSendSMS_Click(object sender, EventArgs e)
    {
        try
        {
            string API = clsGV.msgAPI;
            string mobile = txtMillMobile.Text.Trim();
            string narration = txtnarration_header.Text;
            string utrdate = txtdoc_date.Text;
            string companyCity = clsCommon.getString("Select City_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
            string msg = "From," + Session["Company_Name"].ToString() + ",Date:" + utrdate + "," + companyCity + " Rs." + txtamount.Text + " ,Ref.No/UTR No.:" + txtunt_no.Text + " and Narration: " + narration;
            string URL = API + "mobile=" + mobile + "&message=" + msg + "&senderid=NAVKAR&accusage=1";
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(URL);
            HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
            StreamReader reader = new StreamReader(myResp.GetResponseStream());
            string str = reader.ReadToEnd();
            reader.Close();
            myResp.Close();
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
            counts = Convert.ToInt32(clsCommon.getString("select count(doc_no) as doc_no from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "'"));
            if (counts == 0)
            {
                txtdoc_no.Text = "1";
                Doc_No = 1;
            }
            else
            {
                Doc_No = Convert.ToInt32(clsCommon.getString("SELECT max(doc_no) as doc_no from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "'")) + 1;
                txtdoc_no.Text = Doc_No.ToString();
            }

            counts = Convert.ToInt32(clsCommon.getString("SELECT count(utrid) as utrid from " + tblHead + " "));
            if (counts == 0)
            {
                lblUtr_Id.Text = "1";

            }
            else
            {
                id = Convert.ToInt32(clsCommon.getString("SELECT max(utrid) as utrid from " + tblHead)) + 1;
                lblUtr_Id.Text = id.ToString();
            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Next Record Error!')", true);
        }
    }
    #endregion


    protected void btnUtrReport_Click(object sender, EventArgs e)
    {
        string docno = lblUtr_Id.Text;
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "kys", "javascript:UTRReport('" + docno + "')", true);
    }
}

