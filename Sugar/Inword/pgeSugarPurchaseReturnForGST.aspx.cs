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
public partial class Sugar_pgeSugarPurchaseReturnForGST : System.Web.UI.Page
{

    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string SystemMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string qryDetail = string.Empty;
    string cityMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string trntype = "PR";
    int defaultAccountCode = 0;
    string qryAccountList = string.Empty;
    string GLedgerTable = string.Empty;
    static WebControl objAsp = null;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    #endregion

    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;
    string cs = string.Empty;
    string Action = string.Empty;
    int id = 0;                          //Autoincrement id
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
    int debit_ac = 3;
    int debitAcName = 4;
    int DRCR = 5;
    int amount = 6;
    int GRDnarration = 7;
    int tenderdetailid = 8;
    int Rowaction = 9;
    int Srno = 10;

    #region Detail part Declaration
    int detail_id = 0;
    int item_code = 0;
    string narration = string.Empty;
    double Quantal = 0.00;
    int packing = 0;
    double rate = 0.00;
    double item_Amount = 0.00;
    int item_name = 0;
    int bags = 0;


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

    int PURCNO = 0;
    string PurcTranType = string.Empty;
    string tran_type = string.Empty;
    string doc_date = string.Empty;
    int Ac_Code = 0;
    int Unit_Code = 0;
    int mill_code = 0;
    string FROM_STATION = string.Empty;
    string TO_STATION = string.Empty;
    string LORRYNO = string.Empty;
    int BROKER = 0;
    string wearhouse = string.Empty;
    double subTotal = 0.00;
    double LESS_FRT_RATE = 0.00;
    double freight = 0.00;
    double cash_advance = 0.00;
    double bank_commission = 0.00;
    double OTHER_AMT = 0.00;
    double Bill_Amount = 0.00;
    int Due_Days = 0;
    double NETQNTL = 0.00;
    string Bill_No = string.Empty;
    double CGSTRate = 0.00;
    double CGSTAmount = 0.00;
    double SGSTRate = 0.00;
    double SGSTAmount = 0.00;
    double IGSTRate = 0.00;
    double IGSTAmount = 0.00;
    int GstRateCode = 0;
    int purcyearcode = 0;
    int Bill_To = 0;
    int srid = 0;
    int Unit_Id = 0;
    int mill_id = 0;
    int Ac_id = 0;
    int bill_id = 0;
    int Broker_id = 0;
    int gst_id = 0;
    int bt = 0;
    int sbid = 0;

    StringBuilder Head_Update = null;
    string Head_Insert = string.Empty;
    string Head_Delete = string.Empty;
    StringBuilder Head_Values = null;
    StringBuilder Head_Fields = null;
    #endregion-End of Head part declearation

    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            user = Session["user"].ToString();
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = "nt_1_sugarpurchasereturn";
            tblDetails = "nt_1_sugarpurchasedetailsreturn";
            AccountMasterTable = "qrymstaccountmaster";
            qryAccountList = "qrymstaccountmaster";
            SystemMasterTable = tblPrefix + "SystemMaster";
            qryCommon = "qrysugarpurchasereturnhead";
            qryDetail = "qrysugarpurchasereturndetail";


            cityMasterTable = tblPrefix + "CityMaster";

            GLedgerTable = tblPrefix + "GLEDGER";
            pnlPopup.Style["display"] = "none";
            cs = ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
            con = new MySqlConnection(cs);

            Head_Update = new StringBuilder();
            Head_Fields = new StringBuilder();
            Head_Values = new StringBuilder();

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
                        hdnf.Value = Request.QueryString["prid"];
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
                        txtPACKING.Text = "50";
                        setFocusControl(txtPURCNO);
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


    protected void txtEditDoc_No_TextChanged(object sender, EventArgs e)
    {
        try
        {
            bool a = clsCommon.isStringIsNumeric(txtEditDoc_No.Text);
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
                string qry = "select *,CONVERT(varchar(10),doc_date,103) as doc_date1 from " + qryCommon + " where doc_no='" + txtEditDoc_No.Text + "' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='" + Convert.ToInt32(Session["year"].ToString()) + "' and tran_type='" + trntype + "'"; ;
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
                obj.tableName = tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
                pnlPopup.Style["display"] = "none";

                btnSave.Text = "Save";
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                lblMsg.Text = string.Empty;
                txtEditDoc_No.Enabled = true;
                btntxtBillTo.Enabled = false;
                #region logic
                LblPartyname.Text = "";
                LBLMILLNAME.Text = "";
                LBLBROKERNAME.Text = "";
                lblGSTRateName.Text = "";
                btntxtAC_CODE.Enabled = false;
                btntxtMILL_CODE.Enabled = false;
                btntxtBROKER.Enabled = false;
                btntxtdoc_no.Enabled = false;
                btntxtPURCNO.Enabled = false;
                btntxtGSTRateCode.Enabled = false;
                calenderExtenderDate.Enabled = false;
                txtITEM_CODE.Enabled = false;
                btntxtITEM_CODE.Enabled = false;
                txtQUANTAL.Enabled = false;
                txtPACKING.Enabled = false;
                txtBAGS.Enabled = false;
                txtRATE.Enabled = false;
                txtITEMAMOUNT.Enabled = false;
                txtITEM_NARRATION.Enabled = false;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                pnlgrdDetail.Enabled = false;
                btntxtUnitcode.Enabled = false;
                LBLITEMNAME.Enabled = false;
                lblUnitName.Text = "";
                #endregion

                txtITEM_CODE.Enabled = false;
                btntxtITEM_CODE.Enabled = false;
                txtQUANTAL.Enabled = false;
                txtPACKING.Enabled = false;
                txtBAGS.Enabled = false;
                txtRATE.Enabled = false;
                txtITEMAMOUNT.Enabled = false;
                txtITEM_NARRATION.Enabled = false;
                btnAdddetails.Enabled = false;
                btnClosedetails.Enabled = false;
                btntxtBillTo.Enabled = false;
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
                btntxtdoc_no.Enabled = false;
                txtDOC_NO.Enabled = false;
                txtEditDoc_No.Enabled = false;
                txtDOC_DATE.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
                lblBillTo.Text = "";
                #region set Business logic for save
                LblPartyname.Text = "";
                LBLMILLNAME.Text = "";
                LBLBROKERNAME.Text = "";
                btntxtAC_CODE.Enabled = true;
                btntxtMILL_CODE.Enabled = true;
                btntxtBROKER.Enabled = true;
                btntxtUnitcode.Enabled = true;
                btntxtPURCNO.Enabled = true;
                btntxtGSTRateCode.Enabled = true;
                calenderExtenderDate.Enabled = true;
                lblUnitName.Text = "";
                lblGSTRateName.Text = "";
                txtITEM_CODE.Enabled = true;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                pnlgrdDetail.Enabled = true;
                lblTranType.Text = "";
                #endregion
                txtITEM_CODE.Enabled = true;
                btntxtITEM_CODE.Enabled = true;
                txtQUANTAL.Enabled = true;
                txtPACKING.Enabled = true;
                txtBAGS.Enabled = true;
                txtRATE.Enabled = true;
                txtITEMAMOUNT.Enabled = true;
                txtITEM_NARRATION.Enabled = true;
                btnAdddetails.Enabled = true;
                btnClosedetails.Enabled = true;
                btntxtBillTo.Enabled = true;
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

                pnlgrdDetail.Enabled = false;
                btntxtUnitcode.Enabled = false;
                txtEditDoc_No.Enabled = true;
                #region logic
                btntxtAC_CODE.Enabled = false;
                btntxtMILL_CODE.Enabled = false;
                btntxtBROKER.Enabled = false;
                calenderExtenderDate.Enabled = false;
                btntxtPURCNO.Enabled = false;
                btntxtGSTRateCode.Enabled = false;
                #endregion
                txtITEM_CODE.Enabled = false;
                btntxtITEM_CODE.Enabled = false;
                txtQUANTAL.Enabled = false;
                txtPACKING.Enabled = false;
                txtBAGS.Enabled = false;
                txtRATE.Enabled = false;
                txtITEMAMOUNT.Enabled = false;
                txtITEM_NARRATION.Enabled = false;
                btnAdddetails.Enabled = false;
                btnClosedetails.Enabled = false;
                btntxtBillTo.Enabled = false;

                txtDOC_NO.Enabled = false;
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

                pnlgrdDetail.Enabled = true;
                btntxtUnitcode.Enabled = true;
                #region logic
                btntxtAC_CODE.Enabled = true;
                btntxtMILL_CODE.Enabled = true;
                btntxtBROKER.Enabled = true;
                calenderExtenderDate.Enabled = true;
                btntxtPURCNO.Enabled = true;
                btntxtGSTRateCode.Enabled = true;
                txtEditDoc_No.Enabled = false;
                txtITEM_CODE.Enabled = true;
                btntxtITEM_CODE.Enabled = true;
                txtQUANTAL.Enabled = true;
                txtPACKING.Enabled = true;
                txtBAGS.Enabled = true;
                txtRATE.Enabled = true;
                txtITEMAMOUNT.Enabled = true;
                txtITEM_NARRATION.Enabled = true;

                #endregion

                txtITEM_CODE.Enabled = true;
                btntxtITEM_CODE.Enabled = true;
                txtQUANTAL.Enabled = true;
                txtPACKING.Enabled = true;
                txtBAGS.Enabled = true;
                txtRATE.Enabled = true;
                txtITEMAMOUNT.Enabled = true;
                txtITEM_NARRATION.Enabled = true;
                btnAdddetails.Enabled = true;
                btnClosedetails.Enabled = true;
                btntxtBillTo.Enabled = true;

                txtDOC_NO.Enabled = false;
            }

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


    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("A");

        this.NextNumber();
        //pnlPopupDetails.Style["display"] = "none";
        txtGSTRateCode.Text = "1";
        string gstname = clsCommon.getString("select GST_Name from " + tblPrefix + "GSTRateMaster where Doc_no=1  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        lblGSTRateName.Text = gstname;
        setFocusControl(txtPURCNO);
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
        setFocusControl(txtDOC_DATE);
    }
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            FormTypes types = new FormTypes();
            if (hdconfirm.Value == "Yes")
            {
                string currentDoc_No = lblDoc_Id.Text;

                Head_Delete = "delete from " + tblHead + " where prid='" + currentDoc_No + "' ";
                string Detail_Deleteqry = "delete from " + tblDetails + " where prid='" + currentDoc_No + "'";
                GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='PR' and Doc_No=" + txtDOC_NO.Text + " and " +
                    " COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ";


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
                    Response.Redirect("../Inword/PgeShugarPurchHeadUtility.aspx");
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
        string max = clsCommon.getString("select ifnull(max(prid),0) as id from nt_1_sugarpurchasereturn");
        hdnf.Value = max;
        trntype = "PR";
        qry = getDisplayQuery();
        clsButtonNavigation.enableDisable("S");
        this.makeEmptyForm("S");
        this.fetchRecord(qry);
        txtEditDoc_No.Text = string.Empty;
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
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        txtDOC_NO.Text = dt.Rows[0]["DOC_NO"].ToString();
                        lblDoc_Id.Text = hdnf.Value;
                        txtPURCNO.Text = dt.Rows[0]["PURCNO"].ToString();
                        lblTranType.Text = dt.Rows[0]["PurcTranType"].ToString();
                        txtDOC_DATE.Text = dt.Rows[0]["doc_dateConverted"].ToString();
                        txtAC_CODE.Text = dt.Rows[0]["AC_CODE"].ToString();
                        LblPartyname.Text = dt.Rows[0]["Ac_Name_E"].ToString();
                        txtUnit_Code.Text = dt.Rows[0]["Unit_Code"].ToString();
                        lblUnitName.Text = dt.Rows[0]["unitname"].ToString();
                        txtMILL_CODE.Text = dt.Rows[0]["MILL_CODE"].ToString();
                        LBLMILLNAME.Text = dt.Rows[0]["millname"].ToString();
                        txtFROM_STATION.Text = dt.Rows[0]["FROM_STATION"].ToString();
                        txtTO_STATION.Text = dt.Rows[0]["TO_STATION"].ToString();
                        txtLORRYNO.Text = dt.Rows[0]["LORRYNO"].ToString();
                        txtBROKER.Text = dt.Rows[0]["BROKER"].ToString();
                        LBLBROKERNAME.Text = dt.Rows[0]["brokername"].ToString();
                        txtWEARHOUSE.Text = dt.Rows[0]["WEARHOUSE"].ToString();
                        txtSUBTOTAL.Text = dt.Rows[0]["SUBTOTAL"].ToString();
                        txtLESS_FRT_RATE.Text = dt.Rows[0]["LESS_FRT_RATE"].ToString();
                        txtFREIGHT.Text = dt.Rows[0]["FREIGHT"].ToString();
                        txtCASH_ADVANCE.Text = dt.Rows[0]["CASH_ADVANCE"].ToString();
                        txtBANK_COMMISSION.Text = dt.Rows[0]["BANK_COMMISSION"].ToString();
                        txtOTHER_AMT.Text = dt.Rows[0]["OTHER_AMT"].ToString();
                        txtBILL_AMOUNT.Text = dt.Rows[0]["BILL_AMOUNT"].ToString();
                        txtDUE_DAYS.Text = dt.Rows[0]["DUE_DAYS"].ToString();
                        txtNETQNTL.Text = dt.Rows[0]["NETQNTL"].ToString();
                        txtbillNo.Text = dt.Rows[0]["Bill_No"].ToString();

                        txtCGSTRate.Text = dt.Rows[0]["CGSTRate"].ToString();
                        txtCGSTAmount.Text = dt.Rows[0]["CGSTAmount"].ToString();
                        txtSGSTRate.Text = dt.Rows[0]["SGSTRate"].ToString();
                        txtSGSTAmount.Text = dt.Rows[0]["SGSTAmount"].ToString();
                        txtIGSTRate.Text = dt.Rows[0]["IGSTRate"].ToString();
                        txtIGSTAmount.Text = dt.Rows[0]["IGSTAmount"].ToString();

                        txtGSTRateCode.Text = dt.Rows[0]["GstRateCode"].ToString();
                        lblGSTRateName.Text = dt.Rows[0]["GST_Name"].ToString();
                        lblyearcode.Text = dt.Rows[0]["purcyearcode"].ToString();

                        txtBillTo.Text = dt.Rows[0]["Bill_To"].ToString();
                        lblBillTo.Text = dt.Rows[0]["billtoname"].ToString();

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

                        #region  Details
                        qry = "select detail_id as ID,item_code,itemname as item_Name,narration,Quantal,packing,bags,rate,item_Amount,prdid as prdid from qrysugarpurchasereturndetail where prid=" + hdnf.Value + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
            string qryDisplay = "select * from " + qryCommon + " where prid=" + hdnf.Value + " and Tran_Type='" + trntype + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
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
    //    pnlPopupDetails.Style["display"] = "block";
    //    txtPACKING.Text = "50";
    //    txtITEM_CODE.Text = "";
    //    txtQUANTAL.Text = "";
    //    txtBAGS.Text = "";
    //    txtRATE.Text = "";
    //    txtITEMAMOUNT.Text = "";
    //    txtITEM_NARRATION.Text = "";
    //    LBLITEMNAME.Text = "";
    //    setFocusControl(txtITEM_CODE);
    //}
    #endregion

    #region [btnAdddetails_Click]
    protected void btnAdddetails_Click(object sender, EventArgs e)
    {
        try
        {
            bool isValidated = true;
            if (txtRATE.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtRATE);
                return;
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
                        //     rowIndex = dt.Rows.Count + 1;
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
                        string id = clsCommon.getString("select detail_id from " + tblDetails + " where doc_no=" + txtDOC_NO.Text + " and Detail_ID=" + lblID.Text + " And Company_Code = " + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
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
                    dt.Columns.Add((new DataColumn("ID", typeof(Int32))));

                    #region [Write here columns]
                    dt.Columns.Add((new DataColumn("item_code", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("item_Name", typeof(string))));
                    dt.Columns.Add((new DataColumn("narration", typeof(string))));
                    dt.Columns.Add((new DataColumn("Quantal", typeof(double))));
                    dt.Columns.Add((new DataColumn("packing", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("bags", typeof(Int32))));
                    dt.Columns.Add((new DataColumn("rate", typeof(double))));
                    dt.Columns.Add((new DataColumn("item_Amount", typeof(double))));
                    dt.Columns.Add((new DataColumn("prdid", typeof(int))));
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
                dt.Columns.Add((new DataColumn("item_code", typeof(Int32))));
                dt.Columns.Add((new DataColumn("item_Name", typeof(string))));
                dt.Columns.Add((new DataColumn("narration", typeof(string))));
                dt.Columns.Add((new DataColumn("Quantal", typeof(double))));
                dt.Columns.Add((new DataColumn("packing", typeof(Int32))));
                dt.Columns.Add((new DataColumn("bags", typeof(Int32))));
                dt.Columns.Add((new DataColumn("rate", typeof(double))));
                dt.Columns.Add((new DataColumn("item_Amount", typeof(double))));
                dt.Columns.Add((new DataColumn("prdid", typeof(int))));
                #endregion
                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                dt.Columns.Add((new DataColumn("SrNo", typeof(int))));

                dr = dt.NewRow();
                dr["ID"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;
            }
            #region [ Set values to dr]
            dr["item_code"] = txtITEM_CODE.Text;
            dr["item_Name"] = LBLITEMNAME.Text;
            dr["narration"] = txtITEM_NARRATION.Text;
            if (txtQUANTAL.Text != string.Empty)
            {
                dr["Quantal"] = txtQUANTAL.Text;
            }
            else
            {
                setFocusControl(txtQUANTAL);
            }
            if (txtPACKING.Text != string.Empty)
            {
                dr["packing"] = txtPACKING.Text;
            }
            else
            {
                setFocusControl(txtPACKING);
            }

            dr["bags"] = txtBAGS.Text;
            if (txtRATE.Text != string.Empty)
            {
                dr["rate"] = txtRATE.Text;
            }
            else
            {
                setFocusControl(txtRATE);
            }
            if (txtITEMAMOUNT.Text != string.Empty)
            {
                dr["item_Amount"] = txtITEMAMOUNT.Text;
            }
            else
            {
                setFocusControl(txtITEMAMOUNT);
            }
            #endregion
            if (btnAdddetails.Text == "ADD")
            {
                dr["prdid"] = 0;
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
                //pnlPopupDetails.Style["display"] = "block";
                setFocusControl(txtITEM_CODE);
            }
            else
            {
                //pnlPopupDetails.Style["display"] = "none";
                setFocusControl(btnAdddetails);
                btnAdddetails.Text = "ADD";

            }
            txtITEM_CODE.Text = string.Empty;
            txtITEM_NARRATION.Text = string.Empty;
            txtQUANTAL.Text = string.Empty;
            txtRATE.Text = string.Empty;
            LBLITEMNAME.Text = string.Empty;
            txtITEMAMOUNT.Text = string.Empty;
            txtPACKING.Text = "50";
            btnAdddetails.Text = "ADD";
            txtBAGS.Text = string.Empty;
            lblID.Text = string.Empty;
            lblNo.Text = string.Empty;
            csCalculations();
            setFocusControl(txtITEM_CODE);
        }
        catch
        {
        }
    }
    #endregion

    #region [btnClosedetails_Click]
    protected void btnClosedetails_Click(object sender, EventArgs e)
    {
        txtITEM_CODE.Text = string.Empty;
        txtITEM_NARRATION.Text = string.Empty;
        txtQUANTAL.Text = string.Empty;
        txtRATE.Text = string.Empty;
        LBLITEMNAME.Text = string.Empty;
        txtITEMAMOUNT.Text = string.Empty;
        txtPACKING.Text = "50";
        btnAdddetails.Text = "ADD";
        txtBAGS.Text = string.Empty;
        setFocusControl(txtITEM_CODE);
        lblID.Text = string.Empty;
        lblNo.Text = string.Empty;
    }
    #endregion

    #region [showDetailsRow]
    private void showDetailsRow(GridViewRow gvrow)
    {
        lblNo.Text = Server.HtmlDecode(gvrow.Cells[13].Text);
        lblID.Text = Server.HtmlDecode(gvrow.Cells[2].Text);
        txtITEM_CODE.Text = Server.HtmlDecode(gvrow.Cells[3].Text);
        LBLITEMNAME.Text = clsCommon.getString("Select System_Name_E from " + tblPrefix + "SystemMaster where System_Code=" + Server.HtmlDecode(gvrow.Cells[3].Text.ToString()) + " and System_Type='I' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        txtITEM_NARRATION.Text = Server.HtmlDecode(gvrow.Cells[5].Text);
        txtQUANTAL.Text = Server.HtmlDecode(gvrow.Cells[6].Text);
        txtPACKING.Text = Server.HtmlDecode(gvrow.Cells[7].Text);
        txtBAGS.Text = Server.HtmlDecode(gvrow.Cells[8].Text);
        txtRATE.Text = Server.HtmlDecode(gvrow.Cells[9].Text);
        txtITEMAMOUNT.Text = Server.HtmlDecode(gvrow.Cells[10].Text);
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
                string IDExisting = clsCommon.getString("select detail_id from " + tblDetails + " where prdid=" + ID + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (IDExisting != string.Empty)
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[11].Text = "D";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";            //D=Delete from table

                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[11].Text = "N";
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
                        grdDetail.Rows[rowIndex].Cells[11].Text = "R";       //R=Only remove fro grid
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "R";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[11].Text = "A";
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
            e.Row.Cells[0].Width = new Unit("50px");
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        }
        if (e.Row.RowType != DataControlRowType.Pager)
        {
            if (v == "txtPURCNO")
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[1].Width = new Unit("50px");
                e.Row.Cells[2].Width = new Unit("80px");
                e.Row.Cells[3].Width = new Unit("400px");
                e.Row.Cells[4].Width = new Unit("400px");
                e.Row.Cells[5].Width = new Unit("80px");
                e.Row.Cells[6].Width = new Unit("80px");
                e.Row.Cells[7].Width = new Unit("50px");
                e.Row.Cells[8].Width = new Unit("50px");
                //e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
            if (v == "txtAC_CODE" || v == "txtBillTo" || v == "txtUnit_Code" || v == "txtMILL_CODE" || v == "txtBROKER" || v == "txtGSTRateCode")
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[1].Width = new Unit("300px");
                e.Row.Cells[2].Width = new Unit("200px");
            }
            if (v == "txtITEM_CODE")
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[1].Width = new Unit("300px");
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
                        if (grdDetail.Rows[rowindex].Cells[11].Text != "D" && grdDetail.Rows[rowindex].Cells[11].Text != "R")
                        {
                            //pnlPopupDetails.Style["display"] = "block";
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
            // e.Row.Cells[10].Visible = false;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[0].ControlStyle.Width = new Unit("40px");
            e.Row.Cells[1].ControlStyle.Width = new Unit("50px");
            e.Row.Cells[2].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[3].ControlStyle.Width = new Unit("70px");
            e.Row.Cells[4].ControlStyle.Width = new Unit("150px");
            e.Row.Cells[5].ControlStyle.Width = new Unit("250px");
            e.Row.Cells[6].ControlStyle.Width = new Unit("60px");
            e.Row.Cells[7].ControlStyle.Width = new Unit("60px");
            e.Row.Cells[8].ControlStyle.Width = new Unit("90px");
            e.Row.Cells[9].ControlStyle.Width = new Unit("90px");
            e.Row.Cells[10].ControlStyle.Width = new Unit("90px");
            e.Row.Cells[11].ControlStyle.Width = new Unit("90px");
            e.Row.Cells[12].ControlStyle.Width = new Unit("90px");
            int i = 1;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                i++;
                foreach (TableCell cell in e.Row.Cells)
                {
                    string s = cell.Text.ToString();
                    if (cell.Text.Length > 20)
                    {
                        cell.Text = cell.Text.Substring(0, 31) + "..";
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

    #region [txtPURCNO_TextChanged]
    protected void txtPURCNO_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPURCNO.Text;
        strTextBox = "txtPURCNO";
        csCalculations();
    }
    #endregion

    #region [btntxtPURCNO_Click]
    protected void btntxtPURCNO_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPURCNO";
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

    #region [txtAC_CODE_TextChanged]
    protected void txtAC_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtAC_CODE.Text;
        strTextBox = "txtAC_CODE";
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

    #region [txtMILL_CODE_TextChanged]
    protected void txtMILL_CODE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtMILL_CODE.Text;
        strTextBox = "txtMILL_CODE";
        csCalculations();
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

    #region [txtFROM_STATION_TextChanged]
    protected void txtFROM_STATION_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtFROM_STATION.Text;
        strTextBox = "txtFROM_STATION";
        csCalculations();
    }
    #endregion

    #region [txtTO_STATION_TextChanged]
    protected void txtTO_STATION_TextChanged(object sender, EventArgs e)
    {
        // searchString = txtTO_STATION.Text;
        strTextBox = "txtTO_STATION";
        csCalculations();
    }
    #endregion

    #region [txtLORRYNO_TextChanged]
    protected void txtLORRYNO_TextChanged(object sender, EventArgs e)
    {
        // searchString = txtLORRYNO.Text;
        strTextBox = "txtLORRYNO";
        csCalculations();
    }
    #endregion

    #region [txtBROKER_TextChanged]
    protected void txtBROKER_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBROKER.Text;
        strTextBox = "txtBROKER";
        csCalculations();
    }
    #endregion

    #region [btntxtBROKER_Click]
    protected void btntxtBROKER_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtBROKER";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtWEARHOUSE_TextChanged]
    protected void txtWEARHOUSE_TextChanged(object sender, EventArgs e)
    {
        // searchString = txtWEARHOUSE.Text;
        strTextBox = "txtWEARHOUSE";
        csCalculations();
    }
    #endregion

    #region [txtSUBTOTAL_TextChanged]
    protected void txtSUBTOTAL_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSUBTOTAL.Text;
        strTextBox = "txtSUBTOTAL";
        csCalculations();
    }
    #endregion

    #region [txtLESS_FRT_RATE_TextChanged]
    protected void txtLESS_FRT_RATE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtLESS_FRT_RATE.Text;
        strTextBox = "txtLESS_FRT_RATE";
        csCalculations();
    }
    #endregion

    #region [txtFREIGHT_TextChanged]
    protected void txtFREIGHT_TextChanged(object sender, EventArgs e)
    {
        searchString = txtFREIGHT.Text;
        strTextBox = "txtFREIGHT";
        csCalculations();
    }
    #endregion

    #region [txtCASH_ADVANCE_TextChanged]
    protected void txtCASH_ADVANCE_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCASH_ADVANCE.Text;
        strTextBox = "txtCASH_ADVANCE";
        csCalculations();
    }
    #endregion

    #region [txtBANK_COMMISSION_TextChanged]
    protected void txtBANK_COMMISSION_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBANK_COMMISSION.Text;
        strTextBox = "txtBANK_COMMISSION";
        csCalculations();
    }
    #endregion

    #region [txtOTHER_AMT_TextChanged]
    protected void txtOTHER_AMT_TextChanged(object sender, EventArgs e)
    {
        searchString = txtOTHER_AMT.Text;
        strTextBox = "txtOTHER_AMT";
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
        // searchString = txtDUE_DAYS.Text;
        strTextBox = "txtDUE_DAYS";
        csCalculations();
    }
    #endregion

    #region [txtNETQNTL_TextChanged]
    protected void txtNETQNTL_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtNETQNTL.Text;
        strTextBox = "txtNETQNTL";
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

    #region [txtITEM_NARRATION_TextChanged]
    protected void txtITEM_NARRATION_TextChanged(object sender, EventArgs e)
    {
        searchString = txtITEM_NARRATION.Text;
        strTextBox = "txtITEM_NARRATION";
        csCalculations();
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

    #region [txtPACKING_TextChanged]
    protected void txtPACKING_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPACKING.Text;
        strTextBox = "txtPACKING";
        csCalculations();
    }
    #endregion

    #region [txtBAGS_TextChanged]
    protected void txtBAGS_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBAGS.Text;
        strTextBox = "txtBAGS";
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
            if (hdnfClosePopup.Value == "txtMILL_CODE")
            {
                setFocusControl(txtMILL_CODE);
            }
            if (hdnfClosePopup.Value == "txtBROKER")
            {
                setFocusControl(txtBROKER);
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
            string message = clsCheckingdate.checkdate(txtDOC_DATE.Text, Session["Post_Date"].ToString(), "", "Inword", "", Convert.ToInt32(Session["Company_Code"]), Convert.ToInt32(Session["year"]));
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

            //string Inword_Date = clsCommon.getString("select date_format(Inword_Date,'%d/%m/%Y') as Inword from Post_Date where Company_Code=" + Convert.ToInt32(Session["Company_Code"]).ToString());

            //// value = Outword_Date;
            //Inword_Date = DateTime.Parse(Inword_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

            //dt = Convert.ToDateTime(Inword_Date);
            //Inword_Date = dt.ToString("yyyy-MM-dd HH:mm:ss.fff");

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
                        string str = clsCommon.getString("select doc_no from " + tblHead + " where doc_no=" + txtDOC_NO.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                        if (str != "0")
                        {
                            lblMsg.Text = "Code " + txtDOC_NO.Text + " already exist";
                            NextNumber();
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
            if (txtAC_CODE.Text != string.Empty)
            {
                string iscarporate = clsCommon.getString("select carporate_party from " + AccountMasterTable + " where Ac_Code=" + txtAC_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (iscarporate == "Y")
                {
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
            if (txtDOC_DATE.Text != string.Empty)
            {

                if (clsCommon.isValidDate(txtDOC_DATE.Text) == true)
                {
                    isValidated = true;
                }
                else
                {
                    txtDOC_DATE.Text = "";
                    isValidated = false;
                    return;
                }
            }
            else
            {
                isValidated = false;
                setFocusControl(txtDOC_DATE);
                return;
            }

            if (txtAC_CODE.Text != string.Empty)
            {
                string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtAC_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                if (str != string.Empty)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtAC_CODE);
                    return;
                }
            }
            else
            {
                isValidated = false;
                setFocusControl(txtAC_CODE);
                return;
            }

            if (txtMILL_CODE.Text != string.Empty)
            {
                string str = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_type='M'");
                if (str != string.Empty)
                {
                    isValidated = true;
                }
                else
                {
                    isValidated = false;
                    setFocusControl(txtMILL_CODE);
                    return;
                }
            }
            else
            {
                isValidated = false;
                setFocusControl(txtMILL_CODE);
                return;
            }
            int count = 0;
            if (grdDetail.Rows.Count == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Please Enter Purchase Details!');", true);
                isValidated = false;

                return;
            }
            if (grdDetail.Rows.Count >= 1)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    if (grdDetail.Rows[i].Cells[11].Text == "D")
                    {
                        count++;
                    }
                }
                if (grdDetail.Rows.Count == count)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Minimum One Purchase Details is compulsory!');", true);
                    isValidated = false;

                    return;
                }
            }
            #endregion

            #region -Head part declearation
            // Doc_No = txtDOC_NO.Text != string.Empty ? Convert.ToInt32(txtDOC_NO.Text) : 0;
            PURCNO = txtPURCNO.Text != string.Empty ? Convert.ToInt32(txtPURCNO.Text) : 0;
            PurcTranType = lblTranType.Text;
            doc_date = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            Ac_Code = txtAC_CODE.Text != string.Empty ? Convert.ToInt32(txtAC_CODE.Text) : 0;
            Unit_Code = txtUnit_Code.Text != string.Empty ? Convert.ToInt32(txtUnit_Code.Text) : 0;
            Bill_To = txtBillTo.Text != string.Empty ? Convert.ToInt32(txtBillTo.Text) : 0;

            mill_code = txtMILL_CODE.Text != string.Empty ? Convert.ToInt32(txtMILL_CODE.Text) : 0;
            FROM_STATION = txtFROM_STATION.Text;
            TO_STATION = txtTO_STATION.Text;
            LORRYNO = txtLORRYNO.Text;
            BROKER = txtBROKER.Text != string.Empty ? Convert.ToInt32(txtBROKER.Text) : 2;
            wearhouse = txtWEARHOUSE.Text;
            subTotal = txtSUBTOTAL.Text != string.Empty ? Convert.ToDouble(txtSUBTOTAL.Text) : 0.00;
            LESS_FRT_RATE = txtLESS_FRT_RATE.Text != string.Empty ? Convert.ToDouble(txtLESS_FRT_RATE.Text) : 0.00;
            freight = txtFREIGHT.Text != string.Empty ? Convert.ToDouble(txtFREIGHT.Text) : 0.00;
            cash_advance = txtCASH_ADVANCE.Text != string.Empty ? Convert.ToDouble(txtCASH_ADVANCE.Text) : 0.00;
            bank_commission = txtBANK_COMMISSION.Text != string.Empty ? Convert.ToDouble(txtBANK_COMMISSION.Text) : 0.00;
            OTHER_AMT = txtOTHER_AMT.Text != string.Empty ? Convert.ToDouble(txtOTHER_AMT.Text) : 0.00;
            Bill_Amount = txtBILL_AMOUNT.Text != string.Empty ? Convert.ToDouble(txtBILL_AMOUNT.Text) : 0.00;
            Due_Days = txtDUE_DAYS.Text != string.Empty ? Convert.ToInt32(txtDUE_DAYS.Text) : 0;
            NETQNTL = txtNETQNTL.Text != string.Empty ? Convert.ToDouble(txtNETQNTL.Text) : 0.00;
            Bill_No = txtbillNo.Text;

            string retValue = string.Empty;
            string strRev = string.Empty;
            int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            int Year_Code = Convert.ToInt32(Session["year"].ToString());
            int year_Code = Convert.ToInt32(Session["year"].ToString());
            int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
            CGSTRate = txtCGSTRate.Text != string.Empty ? Convert.ToDouble(txtCGSTRate.Text) : 0;
            CGSTAmount = txtCGSTAmount.Text != string.Empty ? Convert.ToDouble(txtCGSTAmount.Text) : 0;
            IGSTRate = txtIGSTRate.Text != string.Empty ? Convert.ToDouble(txtIGSTRate.Text) : 0;
            IGSTAmount = txtIGSTAmount.Text != string.Empty ? Convert.ToDouble(txtIGSTAmount.Text) : 0;
            SGSTRate = txtSGSTRate.Text != string.Empty ? Convert.ToDouble(txtSGSTRate.Text) : 0;
            SGSTAmount = txtSGSTAmount.Text != string.Empty ? Convert.ToDouble(txtSGSTAmount.Text) : 0;
            GstRateCode = txtGSTRateCode.Text != string.Empty ? Convert.ToInt32(txtGSTRateCode.Text) : 0;

            purcyearcode = lblyearcode.Text != string.Empty ? Convert.ToInt32(lblyearcode.Text) : 0;
            Ac_id = 0;
            try
            {
                Ac_id = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + Ac_Code + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
            }
            catch
            {
            }
            Unit_Id = 0;
            try
            {
                Unit_Id = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + Unit_Code + " and Company_Code=" + Company_Code + ""));
            }
            catch
            {
            }
            mill_id = 0;
            try
            {
                mill_id = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + mill_code + " and Company_Code=" + Company_Code + ""));
            }
            catch
            {
            }
            Broker_id = 0;

            try
            {
                Broker_id = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + BROKER + " and Company_Code=" + Company_Code + ""));
            }
            catch
            {
            }
            bill_id = 0;
            try
            {
                bill_id = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + Bill_To + " and Company_Code=" + Company_Code + ""));
            }
            catch
            {
            }

            try
            {
                sbid = Convert.ToInt32(clsCommon.getString("select saleid from nt_1_sugarsale where doc_no=" + PURCNO + " and Company_Code=" + Company_Code + " and Year_Code=" + Year_Code + ""));
            }
            catch
            {
            }



            string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");
            #endregion-End of Head part declearation
            #region Detail Fields
            Detail_Fields.Append("doc_no,");
            Detail_Fields.Append("detail_id,");
            Detail_Fields.Append("Tran_Type,");

            Detail_Fields.Append("item_code,");
            Detail_Fields.Append("narration,");
            Detail_Fields.Append("Quantal,");
            Detail_Fields.Append("packing,");
            Detail_Fields.Append("bags,");
            Detail_Fields.Append("rate,");
            Detail_Fields.Append("item_Amount,");
            Detail_Fields.Append("Company_Code,");
            Detail_Fields.Append("Year_Code,");
            Detail_Fields.Append("Branch_Code,");
            Detail_Fields.Append("Created_By,");

            Detail_Fields.Append("prid,");
            Detail_Fields.Append("ic,");
            Detail_Fields.Append("prdid");
            #endregion

            if (btnSave.Text == "Save")
            {
                this.NextNumber();

                #region Head Part
                Head_Fields.Append("doc_no,");
                Head_Values.Append("'" + Doc_No + "',");
                Head_Fields.Append("PURCNO,");
                Head_Values.Append("'" + PURCNO + "',");
                Head_Fields.Append("PurcTranType,");
                Head_Values.Append("'" + PURCNO + "',");
                Head_Fields.Append("Tran_Type,");
                Head_Values.Append("'" + trntype + "',");

                Head_Fields.Append("doc_date,");
                Head_Values.Append("'" + doc_date + "',");
                Head_Fields.Append("Ac_Code,");
                Head_Values.Append("'" + Ac_Code + "',");
                Head_Fields.Append("Bill_To,");
                Head_Values.Append("'" + Bill_To + "',");
                Head_Fields.Append("Unit_Code,");
                Head_Values.Append("'" + Unit_Code + "',");
                Head_Fields.Append("mill_code,");
                Head_Values.Append("'" + mill_code + "',");
                Head_Fields.Append("FROM_STATION,");
                Head_Values.Append("'" + FROM_STATION + "',");
                Head_Fields.Append("TO_STATION,");
                Head_Values.Append("'" + TO_STATION + "',");
                Head_Fields.Append("LORRYNO,");
                Head_Values.Append("'" + LORRYNO + "',");
                Head_Fields.Append("BROKER,");
                Head_Values.Append("'" + BROKER + "',");
                Head_Fields.Append("wearhouse,");
                Head_Values.Append("'" + wearhouse + "',");
                Head_Fields.Append("subTotal,");
                Head_Values.Append("'" + subTotal + "',");
                Head_Fields.Append("LESS_FRT_RATE,");
                Head_Values.Append("'" + LESS_FRT_RATE + "',");
                Head_Fields.Append("freight,");
                Head_Values.Append("'" + freight + "',");
                Head_Fields.Append("cash_advance,");
                Head_Values.Append("'" + cash_advance + "',");
                Head_Fields.Append("bank_commission,");
                Head_Values.Append("'" + bank_commission + "',");
                Head_Fields.Append("OTHER_AMT,");
                Head_Values.Append("'" + OTHER_AMT + "',");
                Head_Fields.Append("Bill_Amount,");
                Head_Values.Append("'" + Bill_Amount + "',");
                Head_Fields.Append("Due_Days,");
                Head_Values.Append("'" + Due_Days + "',");
                Head_Fields.Append("NETQNTL,");
                Head_Values.Append("'" + NETQNTL + "',");
                Head_Fields.Append("Company_Code,");
                Head_Values.Append("'" + Company_Code + "',");
                Head_Fields.Append("Year_Code,");
                Head_Values.Append("'" + year_Code + "',");
                Head_Fields.Append("Branch_Code,");
                Head_Values.Append("'" + Branch_Code + "',");
                Head_Fields.Append("Created_By,");
                Head_Values.Append("'" + Created_By + "',");
                Head_Fields.Append("Modified_By,");
                Head_Values.Append("'" + Modified_By + "',");
                Head_Fields.Append("Bill_No,");
                Head_Values.Append("'" + Bill_No + "',");

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
                Head_Fields.Append("GstRateCode,");
                Head_Values.Append("'" + GstRateCode + "',");
                Head_Fields.Append("purcyearcode,");
                Head_Values.Append("'" + purcyearcode + "',");
                Head_Fields.Append("ac,");
                Head_Values.Append("case when 0='" + Ac_id + "' then null else '" + Ac_id + "' end,");
                Head_Fields.Append("sbid,");
                Head_Values.Append("case when 0='" + sbid + "' then null else '" + sbid + "' end,");

                Head_Fields.Append("uc,");
                Head_Values.Append("case when 0='" + Unit_Id + "' then null else '" + Unit_Id + "' end,");
                Head_Fields.Append("mc,");
                Head_Values.Append("case when 0='" + mill_id + "' then null else '" + mill_id + "' end,");
                Head_Fields.Append("bc,");
                Head_Values.Append("case when 0='" + Broker_id + "' then null else '" + Broker_id + "' end,");
                Head_Fields.Append("bt,");
                Head_Values.Append("case when 0='" + bill_id + "' then null else '" + bill_id + "' end,");
                Head_Fields.Append("prid");
                Head_Values.Append("'" + lblDoc_Id.Text + "'");

                #endregion
                Head_Insert = "insert into " + tblHead + "(" + Head_Fields + ") values(" + Head_Values + ")";
                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Head_Insert;
                Maindt.Rows.Add(dr);


                #region Detail Part
                int purchesReturn_Id = Convert.ToInt32(clsCommon.getString("select ifnull(count(prdid),0) as prdid from " + tblDetails + " "));
                if (purchesReturn_Id == 0)
                {
                    purchesReturn_Id = 0;
                }
                else
                {
                    purchesReturn_Id = Convert.ToInt32(clsCommon.getString("select max(prdid) as prdid from " + tblDetails + " "));
                }

                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    purchesReturn_Id = purchesReturn_Id + 1;
                    detail_id = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);
                    item_code = Convert.ToInt32(grdDetail.Rows[i].Cells[3].Text);
                    narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[5].Text);
                    Quantal = Convert.ToDouble(grdDetail.Rows[i].Cells[6].Text);
                    packing = Convert.ToInt32(grdDetail.Rows[i].Cells[7].Text);
                    bags = Convert.ToInt32(grdDetail.Rows[i].Cells[8].Text);
                    rate = Convert.ToDouble(grdDetail.Rows[i].Cells[9].Text);
                    item_Amount = Convert.ToDouble(grdDetail.Rows[i].Cells[10].Text);
                    item_name = 0;
                    try
                    {

                        item_name = Convert.ToInt32(clsCommon.getString(" select ifnull(systemid,0) as id from qrymstitem where System_Code='" + item_code + "' and Company_Code=" + Session["Company_Code"].ToString() + " "));
                    }
                    catch
                    {
                    }
                    Detail_Values.Append("('" + Doc_No + "','" + detail_id + "','" + trntype + "','" + item_code + "','" + narration + "','" + Quantal + "'" +
                 " ,'" + packing + "','" + bags + "','" + rate + "','" + item_Amount + "','" + Company_Code + "'" +
                 " ,'" + year_Code + "','" + Branch_Code + "','" + Created_By + "','" + lblDoc_Id.Text + "',case when 0='" + item_name + "' then null else '" + item_name + "' end,'" + purchesReturn_Id + "'),");
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
                //Thread thred = new Thread(() => { count = DataStore(flag); }); //Calling DataStore Method Using Thread
                //thred.Start(); //Thread Operation Start
                //thred.Join();
                //hdnf.Value = txtTenderNo.Text;
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
                #region Update Head Add Fields And Values

                Head_Update.Append("doc_no=");
                Head_Update.Append("'" + txtDOC_NO.Text + "',");
                Head_Update.Append("PURCNO=");
                Head_Update.Append("'" + PURCNO + "',");
                Head_Update.Append("PurcTranType=");
                Head_Update.Append("'" + PURCNO + "',");
                Head_Update.Append("doc_date=");
                Head_Update.Append("'" + doc_date + "',");
                Head_Update.Append("Ac_Code=");
                Head_Update.Append("'" + Ac_Code + "',");
                Head_Update.Append("Unit_Code=");
                Head_Update.Append("'" + Unit_Code + "',");
                Head_Update.Append("mill_code=");
                Head_Update.Append("'" + mill_code + "',");
                Head_Update.Append("FROM_STATION=");
                Head_Update.Append("'" + FROM_STATION + "',");
                Head_Update.Append("TO_STATION=");
                Head_Update.Append("'" + TO_STATION + "',");
                Head_Update.Append("LORRYNO=");
                Head_Update.Append("'" + LORRYNO + "',");
                Head_Update.Append("BROKER=");
                Head_Update.Append("'" + BROKER + "',");
                Head_Update.Append("wearhouse=");
                Head_Update.Append("'" + wearhouse + "',");
                Head_Update.Append("subTotal=");
                Head_Update.Append("'" + subTotal + "',");
                Head_Update.Append("LESS_FRT_RATE=");
                Head_Update.Append("'" + LESS_FRT_RATE + "',");
                Head_Update.Append("freight=");
                Head_Update.Append("'" + freight + "',");
                Head_Update.Append("cash_advance=");
                Head_Update.Append("'" + cash_advance + "',");
                Head_Update.Append("bank_commission=");
                Head_Update.Append("'" + bank_commission + "',");
                Head_Update.Append("OTHER_AMT=");
                Head_Update.Append("'" + OTHER_AMT + "',");
                Head_Update.Append("Bill_Amount=");
                Head_Update.Append("'" + Bill_Amount + "',");
                Head_Update.Append("Due_Days=");
                Head_Update.Append("'" + Due_Days + "',");
                Head_Update.Append("NETQNTL=");
                Head_Update.Append("'" + NETQNTL + "',");
                Head_Update.Append("Company_Code=");
                Head_Update.Append("'" + Company_Code + "',");
                Head_Update.Append("Year_Code=");
                Head_Update.Append("'" + year_Code + "',");
                Head_Update.Append("Branch_Code=");
                Head_Update.Append("'" + Branch_Code + "',");
                Head_Update.Append("Modified_By=");
                Head_Update.Append("'" + Modified_By + "',");
                Head_Update.Append("Tran_Type=");
                Head_Update.Append("'" + trntype + "',");

                Head_Update.Append("CGSTRate=");
                Head_Update.Append("'" + CGSTRate + "',");
                Head_Update.Append("CGSTAmount=");
                Head_Update.Append("'" + CGSTAmount + "',");
                Head_Update.Append("SGSTRate=");
                Head_Update.Append("'" + SGSTAmount + "',");
                Head_Update.Append("SGSTAmount=");
                Head_Update.Append("'" + SGSTAmount + "',");
                Head_Update.Append("IGSTRate=");
                Head_Update.Append("'" + IGSTRate + "',");
                Head_Update.Append("IGSTAmount=");
                Head_Update.Append("'" + IGSTAmount + "',");
                Head_Update.Append("GstRateCode=");
                Head_Update.Append("'" + GstRateCode + "',");
                Head_Update.Append("purcyearcode=");
                Head_Update.Append("'" + purcyearcode + "',");
                Head_Update.Append("Bill_To=");
                Head_Update.Append("'" + Bill_To + "',");
                Head_Update.Append("Bill_No=");
                Head_Update.Append("'" + Bill_No + "',");
                Head_Update.Append("sbid=");
                Head_Update.Append("case when 0='" + sbid + "' then null else '" + sbid + "' end,");
                Head_Update.Append("ac=");
                Head_Update.Append("case when 0='" + Ac_id + "' then null else '" + Ac_id + "' end,");
                Head_Update.Append("uc=");
                Head_Update.Append("case when 0='" + Unit_Id + "' then null else '" + Unit_Id + "' end,");
                Head_Update.Append("mc=");
                Head_Update.Append("case when 0='" + mill_id + "' then null else '" + mill_id + "' end,");
                Head_Update.Append("bc=");
                Head_Update.Append("case when 0='" + Broker_id + "' then null else '" + Broker_id + "' end,");
                Head_Update.Append("bt=");
                Head_Update.Append("case when 0='" + bill_id + "' then null else '" + bill_id + "' end ");


                string Head_Updateqry = "update " + tblHead + " set " + Head_Update + " where prid='" + hdnf.Value + "'";

                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Head_Updateqry;
                Maindt.Rows.Add(dr);
                #endregion

                #region[details]



                string concatid = string.Empty;

                int sellautoid = Convert.ToInt32(clsCommon.getString("select max(prdid) as prdid from " + tblDetails + " "));
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    detail_id = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);
                    item_code = Convert.ToInt32(grdDetail.Rows[i].Cells[3].Text);
                    narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[5].Text);
                    Quantal = Convert.ToDouble(grdDetail.Rows[i].Cells[6].Text);
                    packing = Convert.ToInt32(grdDetail.Rows[i].Cells[7].Text);
                    bags = Convert.ToInt32(grdDetail.Rows[i].Cells[8].Text);
                    rate = Convert.ToDouble(grdDetail.Rows[i].Cells[9].Text);
                    item_Amount = Convert.ToDouble(grdDetail.Rows[i].Cells[10].Text);
                    int Id = Convert.ToInt32(grdDetail.Rows[i].Cells[11].Text);
                    try
                    {
                        item_name = Convert.ToInt32(clsCommon.getString(" select ifnull(systemid,0) as id from qrymstitem where System_Code='" + item_code + "' and Company_Code=" + Session["Company_Code"].ToString() + " "));
                    }
                    catch
                    {
                    }
                    #region Insert Grid Record
                    if (grdDetail.Rows[i].Cells[12].Text == "A")
                    {
                        sellautoid = sellautoid + 1;

                        Detail_Values.Append("('" + txtDOC_NO.Text + "','" + detail_id + "','" + trntype + "','" + item_code + "','" + narration + "','" + Quantal + "','" + packing + "','" + bags + "','" + rate + "','" + item_Amount + "','" + Company_Code + "','" + year_Code + "','" + Branch_Code + "','" + Created_By + "','" + lblDoc_Id.Text + "',case when 0='" + item_name + "' then null else '" + item_name + "' end,'" + sellautoid + "'),");
                    }
                    #endregion
                    #region Update Grid Record
                    if (grdDetail.Rows[i].Cells[12].Text == "U")
                    {
                        Detail_Update.Append("item_code=case prdid when '" + Id + "' then '" + item_code + "'  ELSE item_code END,");
                        Detail_Update.Append("narration=case prdid when '" + Id + "' then '" + narration + "'  ELSE narration END,");
                        Detail_Update.Append("Quantal=case prdid when '" + Id + "' then '" + Quantal + "'  ELSE Quantal END,");
                        Detail_Update.Append("packing=case prdid when '" + Id + "' then '" + packing + "'  ELSE packing END,");
                        Detail_Update.Append("bags=case prdid when '" + Id + "' then '" + bags + "'  ELSE bags END,");
                        Detail_Update.Append("rate=case prdid when '" + Id + "' then '" + rate + "'  ELSE rate END,");
                        Detail_Update.Append("item_Amount=case prdid when '" + Id + "' then '" + item_Amount + "'  ELSE item_Amount END,");
                        //Detail_Update .Append( "ic=case srdtid when '" + id + "' then '" + item__Name + "'  ELSE ic END,");
                        Detail_Update.Append("ic=case prdid when '" + Id + "' then '" + item_name + "'  ELSE ic END,");
                        concatid = concatid + "'" + Id + "',";

                    }
                    #endregion
                    #region Delete Grid Record
                    if (grdDetail.Rows[i].Cells[12].Text == "D")
                    {
                        Detail_Delete.Append("'" + grdDetail.Rows[i].Cells[10].Text + "',");
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
                    string Detail_Deleteqry = "delete from " + tblDetails + " where prdid in(" + Detail_Delete + ")";

                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = Detail_Deleteqry;
                    Maindt.Rows.Add(dr);
                }
                if (Detail_Update.Length > 0)
                {

                    concatid = concatid.Remove(concatid.Length - 1);
                    Detail_Update.Remove(Detail_Update.Length - 1, 1);
                    string Detail_Updateqry = "update " + tblDetails + " set " + Detail_Update + " where prdid in(" + concatid + ")";

                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = Detail_Updateqry;
                    Maindt.Rows.Add(dr);
                }
                #endregion

                Doc_No = Convert.ToInt32(txtDOC_NO.Text);
                flag = 2;



            }

            #region Gledger Effect
            int ORDER_CODE = 0;
            FormTypes types = new FormTypes();
            StringBuilder Gledger_values = new StringBuilder();
            GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='PR' and Doc_No=" + Doc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = GLEDGER_Delete;
            Maindt.Rows.Add(dr);

            StringBuilder Gledger_Column = new StringBuilder();
            Gledger_Column.Append("TRAN_TYPE,CASHCREDIT,DOC_NO,DOC_DATE,AC_CODE,UNIT_code,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE," +
                         " YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO,ac,vc,progid,tranid");

            string saleReturnAcc = clsCommon.getString("select ifnull(Purchase_AC,0) as id from qrymstitem where Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + " limit 1");
            string ReturnSaleAcc_id = clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code=" + saleReturnAcc + " and Company_Code=" + Company_Code + "");

            if (Ac_Code != 0)
            {
                ORDER_CODE = ORDER_CODE + 1;
                Gledger_values.Append("('PR','','" + Doc_No + "','" + doc_date + "','" + Ac_Code + "','0','','" + Bill_Amount + "', " +
                                                           " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + ORDER_CODE + "','C','" + saleReturnAcc + "',0,'" + Branch_Code + "','PR','" + Doc_No + "'," +
                                                           " case when 0='" + Ac_id + "' then null else '" + Ac_id + "' end,'0','" + types.TT_SR + "','0')");
                ORDER_CODE = ORDER_CODE + 1;
                Gledger_values.Append(",('PR','','" + Doc_No + "','" + doc_date + "','" + saleReturnAcc + "','0','','" + Bill_Amount + "', " +
                                                           " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + ORDER_CODE + "','D','" + Ac_Code + "',0,'" + Branch_Code + "','PR','" + Doc_No + "'," +
                                                           " case when 0='" + ReturnSaleAcc_id + "' then null else '" + ReturnSaleAcc_id + "' end,'0','" + types.TT_SR + "','0')");
            }

            GLEDGER_Insert = "insert into nt_1_gledger (" + Gledger_Column + ") values " + Gledger_values + " ";
            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = GLEDGER_Insert;
            Maindt.Rows.Add(dr);
            #endregion

            msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);
            // msg = clsDAL.DataStore(Head_Insert, Head_Update, Head_Delete, Detail_Insert, Detail_Update, Detail_Delete, GLEDGER_Insert, GLEDGER_Delete, flag);
            if (msg == "Insert")
            {
                hdnf.Value = id.ToString();
                clsButtonNavigation.enableDisable("S");
                this.makeEmptyForm("S");
                showLastRecord();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Added !')", true);
            }
            else if (msg == "Update")
            {
                hdnf.Value = lblDoc_Id.Text;
                clsButtonNavigation.enableDisable("S");
                this.makeEmptyForm("S");
                showLastRecord();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update !')", true);

            }
            txtEditDoc_No.Text = string.Empty;
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
                    string qry = "select doc_no,doc_date,PartyShortname,PartyCity from " + qryCommon + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) +
                        " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                        " and (doc_no like '%" + txtSearchText.Text + "%' or doc_date like '%" + txtSearchText.Text + "%' or PartyShortname like '%" + txtSearchText.Text + "%' or PartyCity like '%" + txtSearchText.Text + "%')";
                    this.showPopup(qry);
                }
            }
            if (hdnfClosePopup.Value == "txtPURCNO")
            {
                lblPopupHead.Text = "--Select Purchase No--";
                //string qry = "Select doc_no,Tran_Type,doc_date,PartyName,MillName,Bill_Amount,Quantal,Balance,Year_Code from " + tblPrefix
                //    + "qrySugarSaleAndVouchersForReturn where Balance!=0 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                //    + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and (PartyName like '%" + txtSearchText.Text + "%' or MillName like '%"
                //    + txtSearchText.Text + "%' or doc_no like '%" + txtSearchText.Text + "%' )";


                string qry = "Select doc_no,Tran_Type,doc_dateConverted as doc_date,billtoname as PartyName,millshortname as MillName,Bill_Amount,NETQNTL as Quantal,Year_Code,saleid from " +
                   " qrysalehead where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                   + " and Year_Code<=" + Convert.ToInt32(Session["year"].ToString()) + " and (billtoname like '%" + txtSearchText.Text + "%' or millname like '%"
                   + txtSearchText.Text + "%' or doc_no like '%" + txtSearchText.Text + "%' )";


                //string qry = "select doc_no as PurcNo,doc_date,MillName,PartyName,NETQNTL,Balance,Year_Code from "
                //    + tblPrefix + "qrySugarPurchaseReturnBalance where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                //    + " and Year_Code<=" + Convert.ToInt32(Session["year"].ToString())
                //    + " and Balance!=0 and  (doc_no like '%"
                //    + txtSearchText.Text + "%' or MillName like '%" + txtSearchText.Text + "%' or PartyName like '%" + txtSearchText.Text + "%')";

                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtAC_CODE")
            {
                lblPopupHead.Text = "--Select Party Code--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtBillTo")
            {
                lblPopupHead.Text = "--Select Party Code--";
                string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
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
            if (hdnfClosePopup.Value == "txtMILL_CODE")
            {
                lblPopupHead.Text = "--Select Mill--";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " + AccountMasterTable + ".Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + AccountMasterTable + ".Ac_type='M' " +
                    " and (" + AccountMasterTable + ".Ac_Code like '%" + txtSearchText.Text + "%' or " + AccountMasterTable + ".Ac_Name_E like '%" + txtSearchText.Text + "%' or " + cityMasterTable + ".city_name_e like '%" + txtSearchText.Text + "%') order by " + AccountMasterTable + ".Ac_Name_E";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtBROKER")
            {
                lblPopupHead.Text = "--Select Broker--";
                string qry = "select " + AccountMasterTable + ".Ac_Code," + AccountMasterTable + ".Ac_Name_E," + cityMasterTable + ".city_name_e as City from " + AccountMasterTable +
                    " left outer join " + cityMasterTable + " on " + AccountMasterTable + ".City_Code=" + cityMasterTable + ".city_code and " + AccountMasterTable + ".Company_Code=" + cityMasterTable + ".company_code where " + AccountMasterTable + ".Company_Code="
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
                                                txtDOC_NO.Enabled = false;
                                                hdnf.Value = txtDOC_NO.Text;
                                                btnSave.Enabled = true;   //IMP                                       
                                                setFocusControl(txtPURCNO);
                                            }

                                            if (ViewState["mode"].ToString() == "U")
                                            {
                                                //fetch record
                                                qry = getDisplayQuery();
                                                bool recordExist = this.fetchRecord(qry);
                                                if (recordExist == true)
                                                {
                                                    txtDOC_NO.Enabled = false;
                                                    setFocusControl(txtPURCNO);
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
                                            setFocusControl(txtPURCNO);
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
            if (strTextBox == "txtPURCNO")
            {

                //qry = "select doc_no,Tran_Type,Convert(varchar(10),doc_date,103) as doc_date,Party as Party_Code,PartyName as Party,Unit_Code,Unit_Name,mill_code," +
                //    "MillName,FROM_STATION,TO_STATION,Lorry,broker,BrokerName as broker_name,Wearhouse" +
                //            ",Bill_Amount,Balance,Year_Code from " + tblPrefix + "qrySugarSaleAndVouchersForReturn  where doc_no=" + txtPURCNO.Text
                //            + " and Tran_Type='" + hdnfTranType.Value.TrimStart() + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                //            + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                qry = "select doc_no,Tran_Type,doc_dateConverted as doc_date,Ac_Code as Party_Code,billtoname as Party,Unit_Code,shiptoname as Unit_Name,mill_code," +
                   "millname,FROM_STATION,TO_STATION,LORRYNO as Lorry,BROKER as broker,brokername as  broker_name,wearhouse" +
                           ",TaxableAmount as Bill_Amount,'' as Balance,Year_Code,GstRateCode,GST_Name from qrysalehead  where doc_no=" + txtPURCNO.Text
                          + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                           + " and Year_Code=" + Session["year"] + "";



                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds = clsDAL.SimpleQuery(qry);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string doc_no = ds.Tables[0].Rows[0]["doc_no"].ToString();
                        string Tran_Type = ds.Tables[0].Rows[0]["Tran_Type"].ToString();
                        lblTranType.Text = Tran_Type;

                        string SB_No = clsCommon.getString("Select PURCNO from " + tblPrefix + "SugarSale where doc_no=" + doc_no + " and Company_Code="
                       + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");

                        string DO_no = clsCommon.getString("Select DO_No from " + tblPrefix + "SugarSale where doc_no=" + doc_no + " and Company_Code="
                       + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");

                        string carporateNo = clsCommon.getString("Select Carporate_Sale_No from " + tblPrefix + "deliveryorder where doc_no=" + DO_no
                       + " and tran_type='DO' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code="
                       + Convert.ToInt32(Session["year"].ToString()) + " ");


                        ViewState["carporateNo"] = carporateNo;




                        txtAC_CODE.Text = ds.Tables[0].Rows[0]["Party_Code"].ToString();
                        LblPartyname.Text = ds.Tables[0].Rows[0]["Party"].ToString();
                        txtUnit_Code.Text = ds.Tables[0].Rows[0]["Unit_Code"].ToString();
                        lblUnitName.Text = ds.Tables[0].Rows[0]["Unit_Name"].ToString();
                        txtMILL_CODE.Text = ds.Tables[0].Rows[0]["mill_code"].ToString();
                        LBLMILLNAME.Text = ds.Tables[0].Rows[0]["MillName"].ToString();
                        txtFROM_STATION.Text = ds.Tables[0].Rows[0]["FROM_STATION"].ToString();
                        txtTO_STATION.Text = ds.Tables[0].Rows[0]["TO_STATION"].ToString();
                        txtDOC_DATE.Text = ds.Tables[0].Rows[0]["doc_date"].ToString();
                        txtLORRYNO.Text = ds.Tables[0].Rows[0]["Lorry"].ToString();
                        txtWEARHOUSE.Text = ds.Tables[0].Rows[0]["Wearhouse"].ToString();
                        txtBROKER.Text = ds.Tables[0].Rows[0]["broker"].ToString();
                        LBLBROKERNAME.Text = ds.Tables[0].Rows[0]["broker_name"].ToString();
                        txtNETQNTL.Text = ds.Tables[0].Rows[0]["Balance"].ToString();
                        txtSUBTOTAL.Text = ds.Tables[0].Rows[0]["Bill_Amount"].ToString();
                        txtBILL_AMOUNT.Text = ds.Tables[0].Rows[0]["Bill_Amount"].ToString();
                        lblyearcode.Text = ds.Tables[0].Rows[0]["Year_Code"].ToString();
                        txtGSTRateCode.Text = ds.Tables[0].Rows[0]["GstRateCode"].ToString();
                        lblGSTRateName.Text = ds.Tables[0].Rows[0]["GST_Name"].ToString();


                        if (!string.IsNullOrEmpty(carporateNo))
                        {
                            if (carporateNo != "0")
                            {
                                //ds.Tables[0].Rows[0]["Less_Frieght"] = 0.00;
                                //ds.Tables[0].Rows[0]["Cash_Advance"] = 0.00;
                                string billto = clsCommon.getString("select ISNULL(Bill_To,0) from NT_1_CarporateSale where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and DOC_NO=" + carporateNo + "");

                                //                                string billto = clsCommon.getString("select ISNULL(Bill_To,0) from NT_1_CarporateSale where Year_Code=" + lblyearcode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Doc_No=" + doc_no + "");
                                if (billto != "0")
                                {
                                    string BillToName = clsCommon.getString("select Ac_Name_E from NT_1_AccountMaster where Ac_Code=" + billto + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                                    string BillToAddress = clsCommon.getString("select Address_E from NT_1_AccountMaster where Ac_Code=" + billto + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");


                                    txtBillTo.Text = billto.ToString();
                                    lblBillTo.Text = BillToName;
                                }
                            }
                            else
                            {

                                txtBillTo.Text = ds.Tables[0].Rows[0]["Party_Code"].ToString();
                                lblBillTo.Text = ds.Tables[0].Rows[0]["Party"].ToString();
                            }

                        }
                        else
                        {
                        }

                        //qry = "select detail_id as ID,item_code,narration,Quantal,packing,bags,rate,item_Amount,Balance from " + tblPrefix
                        //    + "qrySugarSaleAndVouchersForReturn where doc_no=" + doc_no + " and Tran_Type='" + Tran_Type + "' and Company_Code="
                        //    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code="
                        //    + Convert.ToInt32(Session["year"].ToString()) + " order by detail_id";

                        qry = "select saledetailid as ID,item_code,itemname as item_Name,narration,Quantal,packing,bags,salerate as rate,TaxableAmount as item_Amount from qrysaleheaddetail " +
                            " where doc_no=" + doc_no + " and Company_Code="
                           + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code="
                           + Convert.ToInt32(Session["year"].ToString()) + " order by saledetailid";
                        ds = new DataSet();
                        ds = clsDAL.SimpleQuery(qry);
                        if (ds != null)
                        {
                            dt = new DataTable();
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                dt.Columns.Add(new DataColumn("ID", typeof(string)));
                                dt.Columns.Add(new DataColumn("item_code", typeof(string)));
                                dt.Columns.Add(new DataColumn("item_Name", typeof(string)));
                                dt.Columns.Add(new DataColumn("narration", typeof(string)));
                                dt.Columns.Add(new DataColumn("Quantal", typeof(string)));
                                dt.Columns.Add(new DataColumn("packing", typeof(string)));
                                dt.Columns.Add(new DataColumn("bags", typeof(string)));
                                dt.Columns.Add(new DataColumn("rate", typeof(string)));
                                dt.Columns.Add(new DataColumn("item_Amount", typeof(string)));
                                dt.Columns.Add(new DataColumn("prdid", typeof(int)));
                                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                                dt.Columns.Add(new DataColumn("SrNo", typeof(string)));
                                int srno = 1;
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    DataRow dr = dt.NewRow();
                                    dr["ID"] = ds.Tables[0].Rows[i]["ID"].ToString();
                                    dr["item_code"] = ds.Tables[0].Rows[i]["item_code"].ToString();
                                    dr["item_Name"] = ds.Tables[0].Rows[i]["item_Name"].ToString();
                                    dr["narration"] = ds.Tables[0].Rows[i]["narration"].ToString();
                                    dr["Quantal"] = ds.Tables[0].Rows[i]["Quantal"].ToString();
                                    dr["packing"] = ds.Tables[0].Rows[i]["packing"].ToString();
                                    dr["bags"] = ds.Tables[0].Rows[i]["bags"].ToString();
                                    dr["rate"] = ds.Tables[0].Rows[i]["rate"].ToString();
                                    dr["item_Amount"] = ds.Tables[0].Rows[i]["item_Amount"].ToString();
                                    dr["prdid"] = 0;
                                    dr["rowAction"] = "A";
                                    dr["SrNo"] = srno++;
                                    dt.Rows.Add(dr);
                                }
                                if (dt.Rows.Count > 0)
                                {
                                    ViewState["currentTable"] = dt;
                                    grdDetail.DataSource = dt;
                                    grdDetail.DataBind();
                                }
                                else
                                {
                                    grdDetail.DataSource = null;
                                    grdDetail.DataBind();
                                }
                            }
                        }
                    }
                }
                setFocusControl(txtDOC_DATE);
            }
            if (strTextBox == "txtDOC_DATE")
            {
                try
                {
                    string dt = DateTime.Parse(txtDOC_DATE.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    if (clsCommon.isValidDate(dt) == true)
                    {
                        setFocusControl(txtAC_CODE);
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
                        acname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtAC_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (acname != string.Empty && acname != "0")
                        {
                            LblPartyname.Text = acname;
                            setFocusControl(txtBillTo);
                            txtTO_STATION.Text = clsCommon.getString("select CityName from " + qryAccountList + " where Ac_Code=" + txtAC_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
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

            if (strTextBox == "txtBillTo")
            {
                string acname = "";
                if (txtBillTo.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtBillTo.Text);
                    if (a == false)
                    {
                        btntxtBillTo_Click(this, new EventArgs());
                    }
                    else
                    {
                        acname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtBillTo.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (acname != string.Empty && acname != "0")
                        {
                            lblBillTo.Text = acname;
                            setFocusControl(txtUnit_Code);
                            // txtTO_STATION.Text = clsCommon.getString("select CityName from " + qryAccountList + " where Ac_Code=" + txtBillTo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        }
                        else
                        {
                            txtBillTo.Text = string.Empty;
                            lblBillTo.Text = acname;
                            setFocusControl(txtBillTo);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtBillTo);
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
                        string iscarporate = clsCommon.getString("select carporate_party from " + AccountMasterTable + " where Ac_Code=" + txtAC_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (iscarporate == "Y")
                        {
                            lblMsg.Text = "";
                            lblPopupHead.Text = "--Select Unit--";
                            string qry = "select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtUnit_Code.Text +
                                "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
                            acname = clsCommon.getString(qry);
                            if (acname != string.Empty && acname != "0")
                            {
                                lblUnitName.Text = acname;
                                setFocusControl(txtMILL_CODE);
                                //txtTo_Place.Text = clsCommon.getString("select CityName from " + tblPrefix + "qryAccountsList where Ac_Code=" + txtAC_CODE.Text);
                            }
                            else
                            {
                                txtUnit_Code.Text = string.Empty;
                                lblUnitName.Text = acname;
                                setFocusControl(txtUnit_Code);
                            }
                        }
                        else
                        {
                            acname = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtUnit_Code.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            if (acname != string.Empty && acname != "0")
                            {

                                lblUnitName.Text = acname;
                                setFocusControl(txtMILL_CODE);
                                // txtTO_STATION.Text = clsCommon.getString("select CityName from " + qryAccountList + " where Ac_Code=" + txtUnit_Code.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                            }
                            else
                            {
                                txtUnit_Code.Text = string.Empty;
                                lblUnitName.Text = acname;
                                setFocusControl(txtUnit_Code);
                            }
                        }
                    }
                }
                else
                {
                    setFocusControl(txtUnit_Code);
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
                        millName = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtMILL_CODE.Text + " and Ac_type='M' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (millName != string.Empty && millName != "0")
                        {
                            LBLMILLNAME.Text = millName;
                            setFocusControl(txtFROM_STATION);
                            txtFROM_STATION.Text = clsCommon.getString("select CityName from " + qryAccountList + " where Ac_Code=" + txtMILL_CODE.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        }
                        else
                        {
                            txtMILL_CODE.Text = string.Empty;
                            LBLMILLNAME.Text = millName;
                            setFocusControl(txtMILL_CODE);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtMILL_CODE);
                }
            }
            if (strTextBox == "txtFROM_STATION")
            {
                setFocusControl(txtTO_STATION);
            }
            if (strTextBox == "txtTO_STATION")
            {
                setFocusControl(txtLORRYNO);
            }
            if (strTextBox == "txtLORRYNO")
            {
                setFocusControl(txtWEARHOUSE);
            }
            if (strTextBox == "txtWEARHOUSE")
            {
                setFocusControl(txtBROKER);
            }
            if (strTextBox == "txtBROKER")
            {
                string brokername = "";
                if (txtBROKER.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtBROKER.Text);
                    if (a == false)
                    {
                        btntxtBROKER_Click(this, new EventArgs());
                    }
                    else
                    {
                        brokername = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code=" + txtBROKER.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (brokername != string.Empty && brokername != "0")
                        {
                            LBLBROKERNAME.Text = brokername;
                            setFocusControl(txtGSTRateCode);
                        }
                        else
                        {
                            txtBROKER.Text = string.Empty;
                            LBLBROKERNAME.Text = brokername;
                            setFocusControl(txtBROKER);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtBROKER);
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
                        gstname = clsCommon.getString("select GST_Name from " + tblPrefix + "GSTRateMaster where Doc_no=" + txtGSTRateCode.Text + "  ");
                        if (gstname != string.Empty && gstname != "0")
                        {
                            lblGSTRateName.Text = gstname;
                            setFocusControl(txtbillNo);
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

            if (strTextBox == "txtbillNo")
            {

            }
            if (strTextBox == "txtCASH_ADVANCE")
            {
                setFocusControl(txtDUE_DAYS);
            }
            if (strTextBox == "txtBANK_COMMISSION")
            {
                setFocusControl(txtOTHER_AMT);
            }
            if (strTextBox == "txtLESS_FRT_RATE")
            {
                setFocusControl(txtBANK_COMMISSION);
            }
            //if (strTextBox == "txtFREIGHT")
            //{
            //    setFocusControl(txtDUE_DAYS);
            //}
            if (strTextBox == "txtOTHER_AMT")
            {
                double txtBILL_AMOUNT_Convert = txtBILL_AMOUNT.Text != string.Empty ? Convert.ToDouble(txtBILL_AMOUNT.Text) : 0.00;
                double txtOTHER_AMT_Convert = txtOTHER_AMT.Text != string.Empty ? Convert.ToDouble(txtOTHER_AMT.Text) : 0.00;

                txtBILL_AMOUNT_Convert = (txtBILL_AMOUNT_Convert) + (txtOTHER_AMT_Convert);
                txtBILL_AMOUNT.Text = Convert.ToString(txtBILL_AMOUNT_Convert);
                setFocusControl(txtCASH_ADVANCE);
            }
            if (strTextBox == "txtBILL_AMOUNT")
            {
                setFocusControl(btnSave);
            }
            if (strTextBox == "txtDUE_DAYS")
            {
                setFocusControl(btnSave);
            }
            //if (strTextBox == "txtOTHER_AMT")
            //{
            //    setFocusControl(txtBILL_AMOUNT);
            //}
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
                        itemname = clsCommon.getString("select System_Name_E from " + SystemMasterTable + " where System_Code=" + txtITEM_CODE.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and System_Type='I'");
                        if (itemname != string.Empty && itemname != "0")
                        {
                            LBLITEMNAME.Text = itemname;
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
            if (strTextBox == "txtITEM_NARRATION")
            {
                setFocusControl(btnAdddetails);
            }
            if (strTextBox == "txtQUANTAL")
            {
                setFocusControl(txtPACKING);
            }
            if (strTextBox == "txtPACKING")
            {
                setFocusControl(txtRATE);
            }

            if (strTextBox == "txtRATE")
            {
                setFocusControl(txtITEMAMOUNT);
            }
            if (strTextBox == "txtITEMAMOUNT")
            {
                setFocusControl(txtITEM_NARRATION);
            }

            if (strTextBox == "txtLCST")
            {
                this.CSTCalculation();
                setFocusControl(txtSGSTRate);
            }

            if (strTextBox == "txtLIGST")
            {
                this.LIGSTCalculation();
                setFocusControl(txtLESS_FRT_RATE);
            }

            if (strTextBox == "txtLSGST")
            {
                this.LSGSTCalculation();
                setFocusControl(txtIGSTRate);
            }

            #region calculation part
            double qtl = Convert.ToDouble("0" + txtQUANTAL.Text);
            Int32 packing = Convert.ToInt32("0" + txtPACKING.Text);
            Int32 bags = 0;

            double lessfreight = Convert.ToDouble("0" + txtLESS_FRT_RATE.Text);
            double freight = 0.00;

            double netQntl = 0.00;
            double subtotal = 0.00;
            double cashAdv = Convert.ToDouble("0" + txtCASH_ADVANCE.Text);
            double bankComm = Convert.ToDouble("0" + txtBANK_COMMISSION.Text);
            double other = Convert.ToDouble("0" + txtOTHER_AMT.Text);
            double billAmt = 0.00;

            double item_Amount = 0.00;
            double rate = Convert.ToDouble("0" + txtRATE.Text);
            double RCST = Convert.ToDouble("0" + txtCGSTAmount.Text);
            double RIGST = Convert.ToDouble("0" + txtIGSTAmount.Text);
            double RSGST = Convert.ToDouble("0" + txtSGSTAmount.Text);

            if (qtl != 0 && packing != 0)
            {
                bags = Convert.ToInt32((qtl / packing) * 100);
                txtBAGS.Text = bags.ToString();
            }
            else
            {
                txtBAGS.Text = bags.ToString();
            }
            item_Amount = Math.Round((qtl * rate), 2);
            txtITEMAMOUNT.Text = item_Amount.ToString();

            #region calculate subtotal
            if (grdDetail.Rows.Count > 0)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    if (grdDetail.Rows[i].Cells[12].Text != "D")
                    {

                        double item_Amt = Convert.ToDouble(grdDetail.Rows[i].Cells[10].Text.Trim());
                        subtotal = subtotal + item_Amt;
                    }
                }
                txtSUBTOTAL.Text = subtotal.ToString();
            }
            #endregion

            #region calculate net Quantal
            if (grdDetail.Rows.Count > 0)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    if (grdDetail.Rows[i].Cells[12].Text != "D")
                    {
                        double qntl = Convert.ToDouble(grdDetail.Rows[i].Cells[6].Text.Trim());
                        netQntl = netQntl + qntl;
                    }
                }
                txtNETQNTL.Text = netQntl.ToString();
            }
            #endregion

            string aaa = "";
            if (txtAC_CODE.Text.Trim() != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtAC_CODE.Text);
                if (a == true)
                {
                    aaa = clsCommon.getString("select IFNULL(GSTStateCode,0) from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtBillTo.Text + "");
                }
            }
            int partygstStateCode = 0;
            if (aaa.Trim().ToString() != "")
            {
                partygstStateCode = Convert.ToInt32(aaa);
            }

            if (Session["CompanyGSTStateCode"] == null || Session["CompanyGSTStateCode"] == string.Empty)
            {
                Session["CompanyGSTStateCode"] = clsCommon.getString("select GSTStateCode from nt_1_companyparameters where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " +
                    "  Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "");
            }

            int companyGstStateCode = Convert.ToInt32(Session["CompanyGSTStateCode"].ToString());
            string GSTRateCode = txtGSTRateCode.Text;
            double GSTRate = Convert.ToDouble(clsCommon.getString("select Rate from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " "));
            double cgstrate = Convert.ToDouble(clsCommon.getString("select CGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " "));
            double sgstrate = Convert.ToDouble(clsCommon.getString("select SGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " "));
            double igstrate = Convert.ToDouble(clsCommon.getString("select IGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " "));

            double CGSTAmountForPS = 0.0;
            double SGSTAmountForPS = 0.0;
            double IGSTAmountForPS = 0.0;

            double CGSTRateForPS = 0.00;
            double SGSTRateForPS = 0.00;
            double IGSTRateForPS = 0.00;

            if (companyGstStateCode == partygstStateCode)
            {
                CGSTRateForPS = cgstrate;
                double millamount = subtotal;
                double cgsttaxAmountOnMR = Math.Round((millamount * cgstrate / 100), 2);
                //double cgstExMillRate = Math.Round(Math.Abs((mill_rate / (mill_rate + cgsttaxAmountOnMR) * mill_rate)), 2);
                //double cgstRateAmountOnMR = Math.Round(Math.Abs(mill_rate - cgstExMillRate), 2);
                CGSTAmountForPS = Math.Round(cgsttaxAmountOnMR, 2);

                SGSTRateForPS = sgstrate;
                double sgsttaxAmountOnMR = Math.Round((millamount * sgstrate / 100), 2);
                //double sgstExMillRate = Math.Round(Math.Abs((mill_rate / (mill_rate + sgsttaxAmountOnMR) * mill_rate)), 2);
                //double sgstRateAmountOnMR = Math.Round(Math.Abs(mill_rate - sgstExMillRate), 2);
                SGSTAmountForPS = Math.Round(sgsttaxAmountOnMR, 2);
            }
            else
            {
                IGSTRateForPS = igstrate;
                double igsttaxAmountOnMR = ((subtotal) * igstrate / 100);
                //double igstExMillRate = Math.Round(Math.Abs((mill_rate / (mill_rate + igsttaxAmountOnMR) * mill_rate)), 2);
                //double igstRateAmountOnMR = Math.Round(Math.Abs(mill_rate - igstExMillRate), 2);
                IGSTAmountForPS = Math.Round(igsttaxAmountOnMR, 2);
            }

            txtCGSTRate.Text = CGSTRateForPS.ToString();
            txtCGSTAmount.Text = CGSTAmountForPS.ToString();
            txtSGSTRate.Text = SGSTRateForPS.ToString();
            txtSGSTAmount.Text = SGSTAmountForPS.ToString();
            txtIGSTRate.Text = IGSTRateForPS.ToString();
            txtIGSTAmount.Text = IGSTAmountForPS.ToString();
            freight = Math.Round((lessfreight * netQntl), 2);
            txtFREIGHT.Text = freight.ToString();

            billAmt = Math.Round((subtotal + bankComm + cashAdv + other + CGSTAmountForPS + SGSTAmountForPS + IGSTAmountForPS) - freight);
            txtBILL_AMOUNT.Text = billAmt.ToString();
            // txtTaxableAmount.Text = Math.Round((subtotal + freight), 2).ToString();




            //freight = Math.Round((lessfreight * netQntl), 2);
            //txtFREIGHT.Text = freight.ToString();
            //billAmt = (subtotal + bankComm + cashAdv + other + RCST + RIGST + RSGST) - freight;
            //txtBILL_AMOUNT.Text = billAmt.ToString();
            #endregion
        }
        catch
        {
        }
    }
    #endregion

    #region[CSTCalculation]
    private void CSTCalculation()
    {
        double Totalamt = txtSUBTOTAL.Text != string.Empty ? Convert.ToDouble(txtSUBTOTAL.Text) : 0.00;
        double CSTtax = txtCGSTRate.Text != string.Empty ? Convert.ToDouble(txtCGSTRate.Text) : 0.00;
        double result = 0.00;

        CSTtax = CSTtax / 100;
        result = Totalamt * CSTtax;
        txtCGSTAmount.Text = result.ToString();

    }

    #endregion

    #region[LSGSTCalculation]
    private void LSGSTCalculation()
    {
        double Totalamt = txtSUBTOTAL.Text != string.Empty ? Convert.ToDouble(txtSUBTOTAL.Text) : 0;
        double LSTtax = txtSGSTRate.Text != string.Empty ? Convert.ToDouble(txtSGSTRate.Text) : 0;
        double result = 0;

        LSTtax = LSTtax / 100;
        result = Totalamt * LSTtax;
        txtSGSTAmount.Text = result.ToString();

    }

    #endregion

    #region[LIGSTCalculation]
    private void LIGSTCalculation()
    {
        double Totalamt = txtSUBTOTAL.Text != string.Empty ? Convert.ToDouble(txtSUBTOTAL.Text) : 0;
        double LITtax = txtIGSTRate.Text != string.Empty ? Convert.ToDouble(txtIGSTRate.Text) : 0;
        double result = 0;

        LITtax = LITtax / 100;
        result = Totalamt * LITtax;
        txtIGSTAmount.Text = result.ToString();

    }

    #endregion


    protected void txtbillNo_TextChanged(object sender, EventArgs e)
    {
        // searchString = txtbillNo.Text;
        strTextBox = "txtbillNo";
        csCalculations();
    }
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
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
    protected void txtLCST_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCGSTRate.Text;
        strTextBox = "txtLCST";
        csCalculations();

    }
    protected void txtLSGST_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSGSTRate.Text;
        strTextBox = "txtLSGST";
        csCalculations();

    }
    protected void txtLIGST_TextChanged(object sender, EventArgs e)
    {
        searchString = txtIGSTRate.Text;
        strTextBox = "txtLIGST";
        csCalculations();

    }
    protected void txtGSTRateCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGSTRateCode.Text;
        strTextBox = "txtGSTRateCode";
        csCalculations();

    }

    protected void txtBillTo_TextChanged(object sender, EventArgs e)
    {
        searchString = txtBillTo.Text;
        strTextBox = "txtBillTo";
        csCalculations();
    }
    protected void btntxtBillTo_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtBillTo";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }

    }

    #region Generate Next Number
    private void NextNumber()
    {
        try
        {
            int counts = 0;
            counts = Convert.ToInt32(clsCommon.getString("select count(doc_no) as doc_no from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                " and Year_Code='" + Session["year"].ToString() + "' "));
            if (counts == 0)
            {
                txtDOC_NO.Text = "1";
                Doc_No = 1;
            }
            else
            {
                Doc_No = Convert.ToInt32(clsCommon.getString("SELECT max(doc_no) as doc_no from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                " and Year_Code='" + Session["year"].ToString() + "'  ")) + 1;
                txtDOC_NO.Text = Doc_No.ToString();
            }

            counts = Convert.ToInt32(clsCommon.getString("SELECT count(prid) as prid from " + tblHead + " "));
            if (counts == 0)
            {
                lblDoc_Id.Text = "1";

            }
            else
            {
                id = Convert.ToInt32(clsCommon.getString("SELECT max(prid) as prid from " + tblHead)) + 1;
                lblDoc_Id.Text = id.ToString();
            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Next Record Error!')", true);
        }
    }
    #endregion
}