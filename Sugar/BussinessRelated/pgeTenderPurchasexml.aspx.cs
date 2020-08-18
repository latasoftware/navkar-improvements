using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Threading;
using System.Text;

public partial class Sugar_pgeTenderPurchasexml : System.Web.UI.Page
{

    public DataSet ds = null;
    public DataTable dt = null;
    Hashtable hash = null;
    string tblHeadVoucher = string.Empty;
    string GLedgerTable = string.Empty;
    public string tableName { get; set; }
    public string code { get; set; }

    string cs = string.Empty;
    string Action = string.Empty;
    //int Tender_No = 0;
    int id = 0;
    int flag = 0;
    int count = 0;
    MySqlConnection con = null;
    MySqlCommand cmd = null;
    MySqlTransaction myTran = null;
    LocalVoucher LV = null;
    int voucher_no = 0;
    string GLEDGER_Insert = string.Empty;
    string GLEDGER_Delete = string.Empty;
    string msg = string.Empty;


    DataTable dt1 = null;
    DataTable dt2 = null;
    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;
    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string AccountMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string qryHead = string.Empty;
    string qryDetail = string.Empty;
    string strTextbox = string.Empty;
    int defaultAccountCode = 0;
    string searchString = string.Empty;
    static WebControl objAsp = null;
    string Tran_Type = "LV";
    string millShortName = string.Empty;
    string DOShortname = string.Empty;
    string voucherbyshortname = string.Empty;
    string AUTO_VOUCHER = string.Empty;
    string qry = string.Empty;
    string user = string.Empty;
    string isAuthenticate = string.Empty;
    string retValue;
    string fornotsaverecord;
    #endregion
    #region Head Declaration Field
    string Lifting_Date = string.Empty;
    string Lifting_Date_Head = String.Empty;
    string Tender_Date = String.Empty;
    Int32 Mill_Code = 0;
    string Sell_Note_No = String.Empty;
    double Mill_Rate = 0.00;
    string Grade = String.Empty;
    double Quantal = 0;
    int Packing = 0;
    int Bags = 0;
    double VOUCHERAMOUNT = 0;
    double DIFF = 0.00;
    bool isNumeric;
    int n;
    double PURCHASE_RATE = 0;
    int Payment_To, Tender_From, Tender_DO, Voucher_By, Broker = 0;
    Int32 VoucherNo = 0;
    float m = 0;
    float Excise_Rate = 0;
    Int32 GstRate_Code = 0;
    string Narration = string.Empty;
    string userName = string.Empty;
    float Purc_Rate = 0;
    string type = string.Empty;
    int Branch_Id = 0;
    string Created_By = clsGV.user;
    string Modified_By = clsGV.user;
    string myNarration = string.Empty;
    double Brokrage = 0.00;
    string str = string.Empty;
    double Diff_Amount = 0.00;
    int docno = 0;
    Int32 Tender_No = 0;
    string Year_Code = string.Empty;
    Int32 Company_Code = 0;
    Int32 mc = 0;
    Int32 itemcode = 0;
    string season = string.Empty;
    Int32 pt = 0;
    Int32 tf = 0;
    Int32 vb = 0;
    Int32 bk = 0;
    Int32 ic = 0;
    Int32 td = 0;
    Int32 Rc = 0;
    double CashDiff = 0.00;

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

    Int32 Buyer = 0;
    double Buyer_Quantal = 0.00;
    double Sale_Rate = 0.00;
    double Commission_Rate = 0.00;
    string Narration1 = "";
    // DateTime.Now.ToString("dd/MM/yyyy");
    string dtype = string.Empty;
    string Delivery_Type = "";
    string Sauda_Date = string.Empty;
    int subBroker = 0;
    string Lifting_Date1 = string.Empty;
    int ID = 0;
    double Buyer_Party = 0;

    #endregion
    #region Grid Fields
    int Detail_Id = 2;
    int Party = 3;
    int Name_of_Party = 4;
    int Broker_No = 5;
    int Name_Of_Broker = 6;
    int quantal = 7;
    int saleRate = 8;
    int commissions = 9;
    int saudaDate = 10;
    int liftingDate = 11;
    int Sauda_Narration = 12;
    int delivaryType = 13;

    int sub_broker = 14;
    int subBrokername = 15;
    int tenderdetailid = 16;

    int rowAction = 17;
    int SrNo = 18;
    #endregion


    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            #region [tn]
            ViewState["tn"] = Request.QueryString["tn"];
            ViewState["source"] = Request.QueryString["source"];
            #endregion

            #region set company name
            string Company_Name_E = clsCommon.getString("select Company_Name_E from Company where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
            Label lbl = (Label)Master.FindControl("lblCompanyName");
            lbl.Text = Company_Name_E;
            #endregion

            tblHeadVoucher = tblPrefix + "voucher";
            user = Session["user"].ToString();
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "Tender";
            tblDetails = tblPrefix + "TenderDetails";
            AccountMasterTable = "qrymstaccountmaster";
            qryCommon = "qrytenderheaddetail";
            GLedgerTable = tblPrefix + "GLEDGER";
            qryDetail = "qrytenderdetail";
            qryHead = "qrytenderhead";
            defaultAccountCode = Convert.ToInt32(clsCommon.getString("select Ac_Code from " + AccountMasterTable + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Name_E='Self'"));
            pnlPopup.Style["display"] = "none";
            cs = ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
            con = new MySqlConnection(cs);
            #region purc rate enable/disables
            if (drpResale.SelectedValue == "M")
            {
                txtPurcRate.Enabled = false;
                // txtDate.Focus();
                rfvtxtPurcRate.Enabled = false;
                rfvtxtDO.Enabled = false;
                rfvtxtPaymentTo.Enabled = false;
                rfvtxtPaymentTo.Enabled = false;
            }
            else
            {
                txtPurcRate.Enabled = true;
                txtDate.Focus();
                rfvtxtPurcRate.Enabled = true;
                rfvtxtDO.Enabled = true;
                rfvtxtPaymentTo.Enabled = true;
                rfvtxtPaymentTo.Enabled = true;
            }
            #endregion

            Maindt = new DataTable();
            dr = null;
            Maindt.Columns.Add("Querys", typeof(string));
            dr = Maindt.NewRow();

            Detail_Fields = new StringBuilder();
            Head_Fields = new StringBuilder();
            Head_Values = new StringBuilder();
            Detail_Values = new StringBuilder();
            Detail_Update = new StringBuilder();
            Detail_Delete = new StringBuilder();
            Head_Update = new StringBuilder();
            if (!Page.IsPostBack)
            {
                isAuthenticate = "1";
                //isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    Action = Request.QueryString["Action"];
                    if (Action == "1")
                    {

                        hdnf.Value = Request.QueryString["tenderid"];
                        pnlPopup.Style["display"] = "none";
                        ViewState["currentTable"] = null;
                        clsButtonNavigation.enableDisable("N");
                        pnlgrdDetail.Enabled = true;
                        this.makeEmptyForm("N");
                        ViewState["mode"] = "I";
                        //this.DebitCreditDiff();
                        this.showLastRecord();
                        setFocusControl(drpResale);


                    }
                    else
                    {
                        string docno = string.Empty;
                        clsButtonNavigation.enableDisable("A");
                        ViewState["mode"] = null;
                        ViewState["mode"] = "I";
                        this.makeEmptyForm("A");
                        this.NextNumber();
                        #region add self row into grid
                        if (grdDetail.Rows.Count == 0)
                        {
                            this.btnADDBuyerDetails_Click(sender, e);
                        }
                        #endregion

                        txtitem_code.Text = "1";
                        lblitemname.Text = clsCommon.getString("select System_Name_E from nt_1_systemmaster where System_Code=1 and " +
                            "Company_Code=" + Session["Company_Code"].ToString() + " and System_Type='I'");
                        txtTenderFrom.Text = "2";
                        lblTenderFrom.Text = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_Code='2' and " +
                            "Company_Code=" + Session["Company_Code"].ToString() + "");
                        txtDO.Text = "2";
                        lblDO.Text = lblTenderFrom.Text;
                        txtVoucherBy.Text = "2";
                        lblVoucherBy.Text = lblTenderFrom.Text;
                        txtBroker.Text = "2";
                        lblBroker.Text = lblDO.Text;
                        txtGstrateCode.Text = "1";
                        setFocusControl(drpResale);
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
                //Button1.Enabled = false;
                btnSave.Text = "Save";
                btnSave.Enabled = false;
                btnChangeNo.Text = "Choose No";
                btnChangeNo.Enabled = false;
                txtBuyer.Enabled = false;
                btnBuyer.Enabled = false;
                txtBuyerParty.Enabled = false;
                btnBuyerParty.Enabled = false;
                lblMsg.Text = string.Empty;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                btntxtitem_code.Enabled = false;
                btnGstrateCode.Enabled = false;
                ViewState["currentTable"] = null;

                drpDeliveryType.Enabled = false;
                txtsubBroker.Enabled = false;
                txtBuyerQuantal.Enabled = false;
                txtBuyerSaleRate.Enabled = false;
                txtBuyerCommission.Enabled = false;
                txtDetailSaudaDate.Enabled = false;
                txtDetailLiftingDate.Enabled = false;
                txtBuyerNarration.Enabled = false;
                btnADDBuyerDetails.Enabled = false;
                btnClose.Enabled = false;
                btnsubBrker.Enabled = false;
                //  Button1.Enabled = false;

                txtBuyer.Enabled = false;
                txtBuyerParty.Enabled = false;
                txtCashDiff.Enabled = false;

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
                btnChangeNo.Text = "Change No";
                btnChangeNo.Enabled = false;

                #region set Business logic for save
                string start = DateTime.Now.ToShortDateString(); //clsCommon.getString("select Convert(varchar(10),GETDATE(),103) as d");
                DateTime startdate = DateTime.Parse(start);
                txtDate.Text = clsCommon.getString("SELECT date_format(CURDATE(),'%d/%m/%Y') as d"); //startdate.ToShortDateString();
                //  clsCommon.getString("select Convert(varchar(10),DATEADD(day,15,getdate()),103) as d");
                txtLiftingDate.Text = clsCommon.getString("SELECT date_format(DATE_ADD(CURDATE(), INTERVAL 10 DAY),'%d/%m/%Y') as d"); //liftingdate.ToString("dd/MM/yyyy");

                // DateTime Headliftingdate = DateTime.Parse(txtLiftingDate.Text);
                txtTenderNo.Enabled = false;
                pnlgrdDetail.Enabled = true;
                lblBroker_Id.Text = string.Empty;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                lblVoucherNo.Text = "";
                txtDetailSaudaDate.Text = clsCommon.getString("SELECT date_format(CURDATE(),'%d/%m/%Y') as d");

                //string Dlift = clsCommon.getString("SELECT date_format(CURDATE(),'%d/%m/%Y') as d");
                //DateTime DetailLift = DateTime.Parse(Dlift);


                string dd = "";

                dd = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

                DateTime Headliftingdate = Convert.ToDateTime(dd);
                string Docdate = Headliftingdate.ToString("yyyy-MM-dd HH:mm:ss.fff");

                string Dlift = DateTime.Parse(clsCommon.getString("SELECT date_format(CURDATE(),'%d/%m/%Y') as d"), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                DateTime DetailLift = Convert.ToDateTime(Dlift);


                if (Headliftingdate > DetailLift)
                {
                    txtDetailLiftingDate.Text = txtLiftingDate.Text;
                }
                else
                {
                    txtDetailLiftingDate.Text = Dlift;
                }
                ViewState["currentTable"] = null;
                btntxtitem_code.Enabled = true;
                btnGstrateCode.Enabled = true;
                #endregion

                drpDeliveryType.Enabled = true;
                txtsubBroker.Enabled = true;
                txtBuyerQuantal.Enabled = true;
                txtBuyerSaleRate.Enabled = true;
                txtBuyerCommission.Enabled = true;
                txtDetailSaudaDate.Enabled = true;
                txtDetailLiftingDate.Enabled = true;
                txtBuyerNarration.Enabled = true;
                btnADDBuyerDetails.Enabled = true;
                btnClose.Enabled = true;
                btnsubBrker.Enabled = true;

                txtBuyer.Enabled = true;
                txtBuyerParty.Enabled = true;
                btnBuyer.Enabled = true;
                btnBuyerParty.Enabled = true;
                txtCashDiff.Enabled = true;
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
                btnChangeNo.Text = "Choose No";
                btnChangeNo.Enabled = false;
                drpDeliveryType.Enabled = false;
                txtsubBroker.Enabled = false;
                txtBuyerQuantal.Enabled = false;
                txtBuyerSaleRate.Enabled = false;
                txtBuyerCommission.Enabled = false;
                txtDetailSaudaDate.Enabled = false;
                txtDetailLiftingDate.Enabled = false;
                txtBuyerNarration.Enabled = false;
                btnADDBuyerDetails.Enabled = false;
                btnClose.Enabled = false;
                btnsubBrker.Enabled = false;

                txtBuyer.Enabled = false;
                txtBuyerParty.Enabled = false;
                btnBuyer.Enabled = false;
                btnBuyerParty.Enabled = false;
                btntxtitem_code.Enabled = false;
                btnGstrateCode.Enabled = false;
                txtCashDiff.Enabled = false;


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


                GridViewRow gr = (GridViewRow)grdDetail.Rows[0];

                gr.Enabled = false;
                //txtTenderNo.Enabled = true;
                btnChangeNo.Text = "Choose No";
                btnChangeNo.Enabled = false;
                lblMsg.Text = string.Empty;
                setFocusControl(drpResale);
                txtEditDoc_No.Enabled = false;
                pnlgrdDetail.Enabled = true;
                txtBuyer.Enabled = true;
                txtBuyerParty.Enabled = true;
                btnBuyer.Enabled = true;
                btnBuyerParty.Enabled = true;
                btntxtitem_code.Enabled = true;
                btnGstrateCode.Enabled = true;

                drpDeliveryType.Enabled = true;
                txtsubBroker.Enabled = true;
                txtBuyerQuantal.Enabled = true;
                txtBuyerSaleRate.Enabled = true;
                txtBuyerCommission.Enabled = true;
                txtDetailSaudaDate.Enabled = true;
                txtDetailLiftingDate.Enabled = true;
                txtBuyerNarration.Enabled = true;
                btnADDBuyerDetails.Enabled = true;
                btnClose.Enabled = true;
                btnsubBrker.Enabled = true;

                txtBuyer.Enabled = true;
                txtBuyerParty.Enabled = true;
                btnBuyer.Enabled = true;
                btnBuyerParty.Enabled = true;
                txtCashDiff.Enabled = true;


            }

            #region Always check this
            if (drpResale.SelectedValue == "M")
            {
                txtPurcRate.Text = string.Empty;
                txtPurcRate.Enabled = false;

                rfvtxtPurcRate.Enabled = false;
                rfvtxtPaymentTo.Enabled = false;
                rfvtxtDO.Enabled = false;
            }
            else
            {
                //  txtPurcRate.Text = string.Empty;
                //   txtPurcRate.Enabled = true;

                rfvtxtPurcRate.Enabled = true;
                rfvtxtPaymentTo.Enabled = true;
                rfvtxtDO.Enabled = true;
            }
            #endregion

            #region common
            if (dAction == "S" || dAction == "N")
            {
                pnlgrdDetail.Enabled = false;

                btnMillCode.Enabled = false;
                btnGrade.Enabled = false;
                btnPaymentTo.Enabled = false;
                btnTenderFrom.Enabled = false;
                btnTenderDO.Enabled = false;
                btnVoucherBy.Enabled = false;
                btnBroker.Enabled = false;
                // Button1.Enabled = false;
                calenderExtenderDate.Enabled = false;
                calenderExtenderLiftingdate.Enabled = false;
                drpResale.Enabled = false;

            }
            if (dAction == "A" || dAction == "N")
            {
                lblMillName.Text = string.Empty;
                lblPaymentTo.Text = string.Empty;
                lblTenderFrom.Text = string.Empty;
                lblDO.Text = string.Empty;
                lblVoucherBy.Text = string.Empty;
                lblBroker.Text = string.Empty;
                lblMsg.Text = string.Empty;

                lbldiff.Text = "0";
                lblAmount.Text = "0";
                drpResale.SelectedValue = "M";

                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
            }

            if (dAction == "A" || dAction == "E")
            {
                btnMillCode.Enabled = true;
                btnGrade.Enabled = true;
                btnPaymentTo.Enabled = true;
                btnTenderFrom.Enabled = true;
                btnTenderDO.Enabled = true;
                btnVoucherBy.Enabled = true;
                btnBroker.Enabled = true;

                // Button1.Enabled = true;
                calenderExtenderDate.Enabled = true;
                calenderExtenderLiftingdate.Enabled = true;
                drpResale.Enabled = true;
            }
            #endregion
            // AutoPostBackControl.Dispose();

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

    private string getDisplayQuery()
    {
        try
        {
            //string qryDisplay = "select * from " + tblHead +
            //    " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and group_Code=" + hdnf.Value;

            string qryDisplay = "select * from " + qryHead +
                " where tenderid =" + hdnf.Value;
            return qryDisplay;
        }
        catch
        {
            return "";
        }
    }


    #region [btnClose_Click]
    protected void btnClose_Click(object sender, EventArgs e)
    {
        txtBuyer.Text = string.Empty;
        lblBuyerName.Text = string.Empty;
        txtBuyerParty.Text = string.Empty;
        lblBuyerPartyName.Text = string.Empty;
        txtBuyerQuantal.Text = string.Empty;
        txtBuyerSaleRate.Text = string.Empty;
        txtBuyerCommission.Text = string.Empty;
        txtBuyerNarration.Text = string.Empty;

        lblno.Text = string.Empty;
        txtsubBroker.Text = string.Empty;
        // pnlPopupTenderDetails.Style["display"] = "none";
        hdnfNextFocus.Value = "";
        btnSave.Focus();
    }
    #endregion

    #region [btnADDBuyerDetails_Click]
    protected void btnADDBuyerDetails_Click(object sender, EventArgs e)
    {
        try
        {
            hdnfNextFocus.Value = "";
            int rowIndex = 0;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt = new DataTable();
            if (ViewState["currentTable"] != null)
            {
                dt = (DataTable)ViewState["currentTable"];

                if (btnADDBuyerDetails.Text == "ADD")
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
                        rowIndex = maxIndex;          //0
                    }
                    #endregion

                    //rowIndex = dt.Rows.Count + 1;
                    dr["ID"] = rowIndex;
                    dr["rowAction"] = "A";
                    dr["SrNo"] = 0;
                }
                else
                {
                    //update row
                    int n = Convert.ToInt32(lblno.Text);
                    rowIndex = Convert.ToInt32(lblID.Text);
                    dr = (DataRow)dt.Rows[n - 1];
                    dr["ID"] = rowIndex;
                    dr["SrNo"] = 0;

                    #region decide whether actual row is updating or virtual [rowAction]
                    string id = clsCommon.getString("select ID from " + tblDetails + " where  tenderDetailid='" + lbltenderdetailid.Text + "'");
                    if (id != string.Empty && id != "0")
                    {
                        dr["rowAction"] = "U";   //actual row
                    }
                    else
                    {
                        id = clsCommon.getString("select ID from " + tblDetails + " where Tender_No='" + txtTenderNo.Text + "' and ID='" + lblID.Text
                            + "' and IsActive='False' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code="
                            + Convert.ToInt32(Session["year"].ToString()) + "");
                        if (id != string.Empty && id != "0")  //added but deleted by another user
                        {
                            dr["rowAction"] = "N";
                        }
                        else
                        {
                            dr["rowAction"] = "A";    //virtual row
                        }
                    }

                    #endregion
                }

            }
            else
            {
                rowIndex = 1;
                dr = null;

                dt.Columns.Add(new DataColumn("ID", typeof(string)));
                dt.Columns.Add(new DataColumn("Party", typeof(int)));
                dt.Columns.Add(new DataColumn("Name_of_Party", typeof(string)));
                dt.Columns.Add(new DataColumn("Broker", typeof(int)));
                dt.Columns.Add(new DataColumn("Name_Of_Broker", typeof(string)));
                dt.Columns.Add(new DataColumn("Quantal", typeof(float)));
                dt.Columns.Add(new DataColumn("Sale_Rate", typeof(float)));
                dt.Columns.Add(new DataColumn("Commission", typeof(float)));
                dt.Columns.Add(new DataColumn("Sauda_Date", typeof(string)));
                dt.Columns.Add(new DataColumn("Lifting_Date", typeof(string)));
                dt.Columns.Add(new DataColumn("Sauda_Narration", typeof(string)));
                dt.Columns.Add(new DataColumn("Delivery_Type", typeof(string)));
                dt.Columns.Add(new DataColumn("sub_broker", typeof(int)));
                dt.Columns.Add(new DataColumn("subBrokername", typeof(string)));
                dt.Columns.Add(new DataColumn("tenderdetailid", typeof(int)));
                dt.Columns.Add(new DataColumn("rowAction", typeof(string)));

                dt.Columns.Add(new DataColumn("SrNo", typeof(int)));

                dr = dt.NewRow();
                dr["ID"] = rowIndex;
                dr["rowAction"] = "A";
                dr["SrNo"] = 0;
            }

            if (rowIndex != 1)
            {
                dr["Party"] = Convert.ToInt32(txtBuyer.Text);
                dr["Name_of_Party"] = lblBuyerName.Text;
                if (txtBuyerParty.Text != string.Empty)
                {
                    dr["Broker"] = Convert.ToInt32(txtBuyerParty.Text);
                }
                else
                {
                    dr["Broker"] = 2;
                }
                if (lblBuyerPartyName.Text == string.Empty)
                {
                    dr["Name_Of_Broker"] = "Self";
                }
                else
                {
                    dr["Name_Of_Broker"] = Server.HtmlDecode(lblBuyerPartyName.Text);
                }

                int Quntal = Convert.ToInt32(txtBuyerQuantal.Text);
                double grdqntal = float.Parse(grdDetail.Rows[0].Cells[7].Text);
                if (grdqntal < Quntal)
                {
                    dr["Quantal"] = txtBuyerQuantal.Text;
                    //setFocusControl(txtBuyerQuantal);
                    //return;
                }
                else
                {
                    dr["Quantal"] = txtBuyerQuantal.Text;
                }

                if (txtBuyerSaleRate.Text != string.Empty)
                {
                    dr["Sale_Rate"] = float.Parse(txtBuyerSaleRate.Text);
                }
                else
                {
                    dr["Sale_Rate"] = 0.00;
                }
                if (txtBuyerCommission.Text != string.Empty)
                {
                    dr["Commission"] = float.Parse(txtBuyerCommission.Text);
                }
                else
                {
                    dr["Commission"] = 0.00;
                }
                if (txtDetailSaudaDate.Text != string.Empty)
                {
                    dr["Sauda_Date"] = txtDetailSaudaDate.Text;
                }
                else
                {
                    dr["Sauda_Date"] = txtDate.Text;
                }

                if (txtDetailLiftingDate.Text != string.Empty)
                {
                    //string a = System.DateTime.Now.ToString("dd-MM-yyyy");
                    dr["Lifting_Date"] = txtDetailLiftingDate.Text;
                }
                else
                {
                    dr["Lifting_Date"] = txtLiftingDate.Text;
                }

                dr["Sauda_Narration"] = txtBuyerNarration.Text;
                if (drpDeliveryType.SelectedValue == "C")
                {
                    dr["Delivery_Type"] = "Commission";
                }
                if (drpDeliveryType.SelectedValue == "N")
                {
                    dr["Delivery_Type"] = "Naka Delivery";
                }
                if (drpDeliveryType.SelectedValue == "D")
                {
                    dr["Delivery_Type"] = "DO";
                }
                if (txtsubBroker.Text != string.Empty && txtsubBroker.Text != "0")
                {
                    dr["sub_broker"] = Convert.ToInt32(txtsubBroker.Text);
                }
                else
                {
                    dr["sub_broker"] = 2;
                }
                if (lblsubBroker.Text == string.Empty)
                {
                    dr["subBrokername"] = "Self";
                }
                else
                {
                    dr["subBrokername"] = Server.HtmlDecode(lblsubBroker.Text);
                }

                //dr["subBrokername"] = lblsubBroker.Text;


                if (btnADDBuyerDetails.Text == "ADD")
                {
                    dr["tenderdetailid"] = 0;
                    dt.Rows.Add(dr);


                }
                //else
                //{
                //    dr["tenderdetailid"] = lbltenderdetailid.Text;

                //}

                string id = clsCommon.getString("select ID from " + tblDetails + " where Tender_No='" + txtTenderNo.Text + "' and ID='1' and Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code=" + Convert.ToInt32(Session["year"].ToString()) + "");
                if (id != string.Empty && id != "0")
                {
                    dt.Rows[0]["rowAction"] = "U";
                }
                else
                {
                    dt.Rows[0]["rowAction"] = "A";
                }
            }
            else
            {
                dr["rowAction"] = "A";
                dr["SrNo"] = 1;
                dr["Party"] = "2";
                dr["Name_of_Party"] = "Self";
                dr["Broker"] = "2";
                dr["Name_Of_Broker"] = "Self";
                lblbuyer_id.Text = clsCommon.getString("select accoid from nt_1_accountmaster where Ac_Code=2");
                lblbuyerparty_id.Text = clsCommon.getString("select accoid from nt_1_accountmaster where Ac_Code=2");
                if (txtQuantal.Text != string.Empty)
                {
                    dr["Quantal"] = float.Parse(txtQuantal.Text);
                }
                else
                {
                    dr["Quantal"] = 0;
                }
                if (txtMillRate.Text != string.Empty)
                {
                    dr["Sale_Rate"] = float.Parse(txtMillRate.Text);
                }
                else
                {
                    dr["Sale_Rate"] = 0.00;
                }
                dr["Commission"] = 0.00;
                dr["Sauda_Date"] = txtDate.Text;
                dr["Lifting_Date"] = txtLiftingDate.Text;
                dr["sub_broker"] = "2";
                dr["subBrokername"] = "self";
                dr["Sauda_Narration"] = string.Empty;
                dr["Delivery_Type"] = "";
                //dr["tenderdetailid"] = 1;
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

            GridViewRow gr = (GridViewRow)grdDetail.Rows[0];
            gr.Enabled = false;
            #region set grid view ro colors
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                if (grdDetail.Rows[i].Cells[14].Text == "D" || grdDetail.Rows[i].Cells[14].Text == "R")
                {
                    grdDetail.Rows[i].Style["background-color"] = "#64BB7F";
                    grdDetail.Rows[i].ForeColor = System.Drawing.Color.White;
                    //  grdDetail.Rows[i].BackColor = System.Drawing.Color.Red;
                }
            }

            #endregion

            ViewState["currentTable"] = dt;
            //PopupTenderDetails.Show();

            txtBuyer.Text = string.Empty;
            drpDeliveryType.SelectedIndex = 1;
            lblBuyerName.Text = string.Empty;
            txtBuyerParty.Text = string.Empty;
            lblBuyerPartyName.Text = string.Empty;
            txtBuyerQuantal.Text = string.Empty;
            lblno.Text = string.Empty;
            txtsubBroker.Text = string.Empty;
            //txtDetailLiftingDate.Text = string.Empty;
            //txtDetailSaudaDate.Text = string.Empty;

            if (drpResale.SelectedValue == "R" || drpResale.SelectedValue == "W")
            {
                txtBuyerSaleRate.Text = txtPurcRate.Text != string.Empty ? Convert.ToString(txtPurcRate.Text) : "0";
            }
            else
            {
                txtBuyerSaleRate.Text = txtMillRate.Text;
            }
            txtBuyerCommission.Text = string.Empty;
            txtBuyerNarration.Text = string.Empty;
            lblID.Text = string.Empty;

            if (btnADDBuyerDetails.Text == "ADD")
            {
                // pnlPopupTenderDetails.Style["display"] = "block";
                txtBuyer.Focus();
            }
            else
            {
                // pnlPopupTenderDetails.Style["display"] = "none";
                // Button1.Focus();
            }

            btnADDBuyerDetails.Text = "ADD";

            //calculate balance self
            this.calculateBalanceSelf();
            setFocusControl(txtBuyer);
            string dd = "";
            //DateTime Headliftingdate = DateTime.Parse(txtLiftingDate.Text);
            //string Dlift = clsCommon.getString("SELECT date_format(CURDATE(),'%d/%m/%Y') as d");
            //DateTime DetailLift = DateTime.Parse(Dlift);
            if (txtLiftingDate.Text != string.Empty)
            {
                // string d = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

                //DateTime oDate = DateTime.ParseExact(txtLiftingDate.Text, "yyyy/MM/dd", null);
                string liftdate = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                DateTime oDate = DateTime.ParseExact(liftdate, "yyyy/MM/dd", null);
                string d = DateTime.Now.ToString("yyyy/MM/dd");
                DateTime currentDate = DateTime.ParseExact(d, "yyyy/MM/dd", null);

                if (currentDate >= oDate)
                {
                    txtDetailLiftingDate.Text = txtLiftingDate.Text;
                }
                else
                {
                    txtDetailLiftingDate.Text = DateTime.Now.ToString("dd/MM/yyyy"); ;
                }

            }

        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            clsButtonNavigation.enableDisable("A");

            ViewState["mode"] = null;
            ViewState["mode"] = "I";
            this.makeEmptyForm("A");
            this.NextNumber();
            #region Set Excise Rate
            txtExciseRate.Text = clsCommon.getString("select EXCISE_RATE from " + tblPrefix + "CompanyParameters where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");

            #endregion

            #region add self row into grid
            if (grdDetail.Rows.Count == 0)
            {
                this.btnADDBuyerDetails_Click(sender, e);
            }
            #endregion

            txtitem_code.Text = "1";
            lblitemname.Text = clsCommon.getString("select System_Name_E from nt_1_systemmaster where System_Code=1 and " +
                "Company_Code=" + Session["Company_Code"].ToString() + " and System_Type='I'");
            txtTenderFrom.Text = "2";
            lblTenderFrom.Text = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_Code='2' and " +
                "Company_Code=" + Session["Company_Code"].ToString() + "");
            txtDO.Text = "2";
            lblDO.Text = lblTenderFrom.Text;
            txtVoucherBy.Text = "2";
            lblVoucherBy.Text = lblTenderFrom.Text;
            txtGstrateCode.Text = "1";
            setFocusControl(drpResale);
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
                obj.tableName = tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                obj.code = "Tender_No";
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
                                    txtTenderNo.Text = ds.Tables[0].Rows[0][0].ToString();
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

    #region [btnSave_Click]
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //btnSave.Enabled = false;
        #region Assign Values
        fornotsaverecord = retValue;
        retValue = string.Empty;


        string qry = "";

        str = string.Empty;
        bool isValidated = true;
        Diff_Amount = lbldiff.Text != string.Empty ? Convert.ToDouble(lbldiff.Text) : 0.00;
        docno = 0;
        //Int32 Voucher_No = 0;

        string Year_Code = Convert.ToInt32(Session["year"].ToString()).ToString();
        Int32 Company_Code = Convert.ToInt32(Convert.ToInt32(Session["Company_Code"].ToString()));

        Lifting_Date = string.Empty;
        Lifting_Date_Head = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

        Tender_Date = DateTime.Parse(txtDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        if (txtDetailLiftingDate.Text != string.Empty)
        {
            Lifting_Date = DateTime.Parse(txtDetailLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        else
        {
            Lifting_Date = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        }
        Mill_Code = Convert.ToInt32(txtMillCode.Text);
        Sell_Note_No = txtSellNoteNo.Text;
        Mill_Rate = double.Parse(txtMillRate.Text);
        Grade = txtGrade.Text;
        Quantal = float.Parse(txtQuantal.Text);
        Packing = Convert.ToInt32(txtPacking.Text);
        Bags = Convert.ToInt32(Math.Round(float.Parse(txtBags.Text)));

        VOUCHERAMOUNT = Convert.ToDouble(lblAmount.Text);
        DIFF = Convert.ToDouble(lbldiff.Text);
        bool isNumeric;
        int n;
        PURCHASE_RATE = txtPurcRate.Text != string.Empty ? Convert.ToDouble(txtPurcRate.Text) : 0.00;
        season = txtSeason.Text;
        Payment_To = string.IsNullOrEmpty(txtPaymentTo.Text) ? Mill_Code : int.Parse(txtPaymentTo.Text);
        Tender_From = string.IsNullOrEmpty(txtTenderFrom.Text) ? 0 : int.Parse(txtTenderFrom.Text);
        Tender_DO = string.IsNullOrEmpty(txtDO.Text) ? 2 : int.Parse(txtDO.Text);
        Voucher_By = string.IsNullOrEmpty(txtVoucherBy.Text) ? 0 : int.Parse(txtVoucherBy.Text);
        Broker = string.IsNullOrEmpty(txtBroker.Text) ? 2 : int.Parse(txtBroker.Text);
        VoucherNo = lblVoucherNo.Text != string.Empty ? Convert.ToInt32(lblVoucherNo.Text) : 0;
        GstRate_Code = string.IsNullOrEmpty(txtGstrateCode.Text) ? 0 : int.Parse(txtGstrateCode.Text);
        itemcode = Convert.ToInt32(txtitem_code.Text != string.Empty ? txtitem_code.Text : "0");
        CashDiff = txtCashDiff.Text != string.Empty ? Convert.ToDouble(txtCashDiff.Text) : 0.00;
        //try
        //{
        //    mc = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + txtMillCode.Text + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
        //}
        //catch
        //{
        //}
        mc = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + txtMillCode.Text + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
        if (mc == 0)
        {
            mc = 0;
        }
        //try
        //{
        pt = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + txtPaymentTo.Text + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
        //}
        //catch
        //{
        //}
        if (pt == 0)
        {
            pt = 0;
        }
        //try
        //{
        tf = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code=" + txtTenderFrom.Text + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
        //}
        //catch
        //{
        //}
        if (tf == 0)
        {
            tf = 0;
        }
        //try
        //{
        td = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + txtDO.Text + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
        //}
        //catch
        //{
        //}
        if (td == 0)
        {
            td = 0;
        }
        //try
        //{
        vb = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + txtVoucherBy.Text + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
        //}
        //catch
        //{
        //}
        if (vb == 0)
        {
            vb = 0;
        }

        //try
        //{
        bk = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + txtBroker.Text + " and Company_Code=" + Session["Company_Code"].ToString() + ""));
        //}
        //catch { }
        if (bk == 0)
        {
            bk = 0;
        }
        //try
        //{
        ic = Convert.ToInt32(clsCommon.getString("select systemid from qrymstitem where System_Code='" + itemcode + "' and Company_Code='" + Company_Code + "'"));
        //}
        //catch { }
        if (ic == 0)
        {
            ic = 0;
        }
        if (int.TryParse(txtPaymentTo.Text, out n))
        {
            Payment_To = n;
        }
        if (int.TryParse(txtTenderFrom.Text, out n))
        {
            Tender_From = n;
        }
        if (int.TryParse(txtDO.Text, out n))
        {
            Tender_DO = n;
        }
        if (int.TryParse(txtVoucherBy.Text, out n))
        {
            Voucher_By = n;
        }
        if (int.TryParse(txtBroker.Text, out n))
        {
            Broker = n;
        }
        m = 0;
        Excise_Rate = 0;
        isNumeric = float.TryParse(txtExciseRate.Text, out m);
        if (isNumeric == true)
            Excise_Rate = m;
        Narration = txtNarration.Text;
        userName = Session["user"].ToString();

        Purc_Rate = 0;
        isNumeric = float.TryParse(txtPurcRate.Text, out m);
        if (isNumeric == true)
            Purc_Rate = m;
        type = drpResale.SelectedValue;
        Branch_Id = Convert.ToInt32(Convert.ToInt32(Session["Branch_Code"].ToString()));
        Created_By = clsGV.user;
        Created_By = Session["user"].ToString();
        //lblCreatedBy.Text = Created_By.ToString();
        Modified_By = clsGV.user;
        Modified_By = Session["user"].ToString();
        //lblModifiedBy.Text = Modified_By.ToString();
        AUTO_VOUCHER = clsCommon.getString("select AutoVoucher from " + tblPrefix + "CompanyParameters where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
        myNarration = string.Empty;
        millShortName = clsCommon.getString("select short_name from qrymstaccountmaster where ac_code=" + Mill_Code + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        DOShortname = clsCommon.getString("select short_name from qrymstaccountmaster where ac_code=" + Tender_DO + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        voucherbyshortname = clsCommon.getString("select short_name from qrymstaccountmaster where ac_code=" + Voucher_By + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        if (drpResale.SelectedValue != "M")
        {
            if (PURCHASE_RATE > 0)
            {
                myNarration = "Qntl " + Quantal + "  " + millShortName + " (M.R." + Mill_Rate + " P.R." + PURCHASE_RATE + ")";
            }
        }


        Brokrage = txtBrokrage.Text != string.Empty ? Convert.ToDouble(txtBrokrage.Text) : 0.0;
        #endregion


        #region ------------- Valiation ---------------
        if (txtTenderNo.Text != string.Empty)
        {
            if (ViewState["mode"].ToString() == "I")
            {

                string str1 = clsCommon.getString("select nt_1_tender from " + tblHead + " where  Tender_No='" + txtTenderNo.Text + "'" +
                         "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                if (str1 != "0")
                {
                    this.NextNumber();

                }
                else
                {
                    isValidated = true;
                }
            }
            else
            {
                isValidated = true;
            }
        }
        if (txtMillCode.Text == string.Empty)
        {
            isValidated = false;
            txtMillCode.Focus();
        }
        else
        {
            string s = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtMillCode.Text
                + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Ac_type='M'");
            if (s == string.Empty || s == "0")
            {
                isValidated = false;
                txtMillCode.Text = string.Empty;
                lblMillName.Text = string.Empty;
                txtMillCode.Focus();
            }
        }
        if (txtMillRate.Text == string.Empty)
        {
            isValidated = false;
            txtMillRate.Focus();
        }
        if (txtGrade.Text == string.Empty)
        {
            isValidated = false;
            txtGrade.Focus();
        }
        if (txtQuantal.Text == string.Empty)
        {
            isValidated = false;
            txtQuantal.Focus();
        }
        if (txtPacking.Text == string.Empty)
        {
            isValidated = false;
            txtPacking.Focus();
        }
        if (Convert.ToInt32(txtBags.Text) > 0)
        {
            isValidated = true;

        }
        else
        {
            setFocusControl(txtBags);
            return;
        }

        if (drpResale.SelectedValue == "R" || drpResale.SelectedValue == "W")
        {
            if (txtPaymentTo.Text == string.Empty)
            {
                isValidated = false;
                setFocusControl(txtPaymentTo);
            }
            else
            {
                string v = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtPaymentTo.Text + "' and Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                if (v != "0")
                {
                    isValidated = true;
                }
                else
                {
                    txtPaymentTo.Text = string.Empty;
                    isValidated = false;
                    setFocusControl(txtPaymentTo);
                }
            }
            if (txtPurcRate.Text == string.Empty)
            {
                isValidated = false;
                txtPurcRate.Focus();
            }
            if (txtDO.Text == string.Empty)
            {
                isValidated = false;
                setFocusControl(txtDO);
            }
            if (txtVoucherBy.Text == string.Empty)
            {
                if (drpResale.SelectedValue == "R" || drpResale.SelectedValue == "W")
                {
                    isValidated = false;
                    setFocusControl(txtVoucherBy);
                }
                else
                {
                    Voucher_By = int.Parse(txtVoucherBy.Text);
                }
            }
            else
            {
                string v = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtDO.Text
                    + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                if (v != string.Empty && v != "0")
                {
                    isValidated = true;
                    if (txtTenderFrom.Text != string.Empty)
                    {
                        Tender_From = int.Parse(txtTenderFrom.Text);
                        isValidated = true;
                    }
                    else
                    {
                        v = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtTenderFrom.Text
                            + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        if (v != string.Empty && v != "0")
                        {
                            Tender_From = Convert.ToInt32(txtTenderFrom.Text);
                            isValidated = true;
                        }
                        else
                        {
                            txtTenderFrom.Text = "0";
                            Tender_From = Convert.ToInt32(txtTenderFrom.Text);
                            txtTenderFrom.Text = Tender_From.ToString();
                            isValidated = true;
                        }
                    }
                    if (txtVoucherBy.Text != string.Empty)
                    {

                        Voucher_By = int.Parse(txtVoucherBy.Text);
                    }
                    else
                    {
                        v = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtVoucherBy.Text
                            + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        if (v != string.Empty)
                        {
                            Voucher_By = Convert.ToInt32(txtVoucherBy.Text);
                            isValidated = true;
                        }
                        else
                        {
                            txtVoucherBy.Text = "0";
                            Voucher_By = Convert.ToInt32(txtVoucherBy.Text);
                            txtVoucherBy.Text = Voucher_By.ToString();
                            isValidated = true;
                        }
                    }
                    if (txtBroker.Text != string.Empty)
                    {
                        Broker = int.Parse(txtBroker.Text);
                    }
                    else
                    {
                        v = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtBroker.Text + "' and Company_Code="
                            + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        if (v != string.Empty && v != "0")
                        {
                            Broker = Convert.ToInt32(txtBroker.Text);
                            isValidated = true;
                        }
                        else
                        {
                            txtBroker.Text = "2";
                            Broker = Convert.ToInt32(txtBroker.Text);
                            txtBroker.Text = Broker.ToString();
                            isValidated = true;
                        }
                    }
                }
                else
                {
                    isValidated = false;
                    txtDO.Text = string.Empty;
                    setFocusControl(txtDO);
                }
            }
        }

        #endregion

        #region[Gst Cal]
        string aaa = "";
        if (txtVoucherBy.Text.Trim() != string.Empty)
        {
            bool a = clsCommon.isStringIsNumeric(txtVoucherBy.Text);
            if (a == true)
            {
                aaa = clsCommon.getString("select ifNULL(GSTStateCode,0) from " + AccountMasterTable + " where Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtVoucherBy.Text + "");
            }
        }
        int partygstStateCode = 0;
        if (aaa.Trim().ToString() != "")
        {
            partygstStateCode = Convert.ToInt32(aaa);
        }

        int companyGstStateCode = 27;
        // = Convert.ToInt32(Session["CompanyGSTStateCode"].ToString());
        companyGstStateCode = Convert.ToInt32(clsCommon.getString("select ifNULL(GSTStateCode,0) from nt_1_companyparameters where Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString())));

        double cgstrate = 0.00;
        double sgstrate = 0.00;
        double igstrate = 0.00;
        double CGSTAmountForPS = 0.0;
        double SGSTAmountForPS = 0.0;
        double IGSTAmountForPS = 0.0;

        double CGSTRateForPS = 0.00;
        double SGSTRateForPS = 0.00;
        double IGSTRateForPS = 0.00;
        double taxmillamt = Convert.ToDouble(lblAmount.Text);
        double vouchergrandamnt = 0.00;
        if (companyGstStateCode == partygstStateCode)
        {
            cgstrate = Convert.ToDouble(clsDAL.GetString("select Rate from nt_1_gstratemaster where Doc_no='" + txtGstrateCode.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
            CGSTRateForPS = cgstrate;
            // double millamount = subtotal;


            double cgsttaxAmountOnMR = Math.Round((taxmillamt * cgstrate / 100), 2);
            CGSTAmountForPS = Math.Round(cgsttaxAmountOnMR, 2);

            sgstrate = Convert.ToDouble(clsDAL.GetString("select Rate from nt_1_gstratemaster where Doc_no='" + txtGstrateCode.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));

            SGSTRateForPS = sgstrate;
            double sgsttaxAmountOnMR = Math.Round((taxmillamt * sgstrate / 100), 2);

            SGSTAmountForPS = Math.Round(sgsttaxAmountOnMR, 2);
        }
        else
        {
            igstrate = Convert.ToDouble(clsDAL.GetString("select Rate from nt_1_gstratemaster where Doc_no='" + txtGstrateCode.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));

            IGSTRateForPS = igstrate;
            double igsttaxAmountOnMR = ((taxmillamt) * igstrate / 100);

            IGSTAmountForPS = Math.Round(igsttaxAmountOnMR, 2);
        }
        vouchergrandamnt = taxmillamt + CGSTAmountForPS + SGSTAmountForPS + IGSTAmountForPS;
        #endregion
        //if tender details contains at least one record then only save the tender

        btnSave.Enabled = false;
        #region[head master]

        #region Detail Column

        Detail_Fields.Append("Tender_No,");
        Detail_Fields.Append("Company_Code,");
        Detail_Fields.Append("Buyer,");
        Detail_Fields.Append("Buyer_Quantal,");
        Detail_Fields.Append("Sale_Rate,");
        Detail_Fields.Append("Commission_Rate,");
        Detail_Fields.Append("Sauda_Date,");
        Detail_Fields.Append("Lifting_Date,");
        Detail_Fields.Append("ID,");

        Detail_Fields.Append("Narration,");
        Detail_Fields.Append("Buyer_Party,");
        Detail_Fields.Append("year_code,");
        Detail_Fields.Append("Branch_Id,");
        Detail_Fields.Append("Delivery_Type,");
        Detail_Fields.Append("sub_broker,");
        Detail_Fields.Append("sbr,");
        Detail_Fields.Append("tenderid,");
        Detail_Fields.Append("tenderdetailid,");
        Detail_Fields.Append("buyerid,");
        Detail_Fields.Append("buyerpartyid");
        #endregion
        string isactive = "true";
        if (btnSave.Text == "Save")
        {

            this.NextNumber();
            #region Head Add Fields And Values
            Head_Fields.Append("Tender_No,");
            Head_Values.Append("'" + Tender_No + "',");
            Head_Fields.Append("Company_Code,");
            Head_Values.Append("'" + Company_Code + "',");
            Head_Fields.Append("Tender_Date,");
            Head_Values.Append("'" + Tender_Date + "',");
            Head_Fields.Append("Lifting_Date,");
            Head_Values.Append("'" + Lifting_Date + "',");
            Head_Fields.Append("Mill_Code,");
            Head_Values.Append("'" + Mill_Code + "',");
            Head_Fields.Append("Grade,");
            Head_Values.Append("'" + Grade + "',");
            Head_Fields.Append("Quantal,");
            Head_Values.Append("'" + Quantal + "',");
            Head_Fields.Append("Packing,");
            Head_Values.Append("'" + Packing + "',");
            Head_Fields.Append("season,");
            Head_Values.Append("'" + season + "',");
            Head_Fields.Append("Bags,");
            Head_Values.Append("'" + Bags + "',");
            Head_Fields.Append("Payment_To,");
            Head_Values.Append("'" + Payment_To + "',");
            Head_Fields.Append("Tender_From,");
            Head_Values.Append("'" + Tender_From + "',");
            Head_Fields.Append("Tender_DO,");
            Head_Values.Append("'" + Tender_DO + "',");
            Head_Fields.Append("Voucher_By,");
            Head_Values.Append("'" + Voucher_By + "',");
            Head_Fields.Append("Broker,");
            Head_Values.Append("'" + Broker + "',");
            Head_Fields.Append("Excise_Rate,");
            Head_Values.Append("'" + Excise_Rate + "',");
            Head_Fields.Append("Narration,");
            Head_Values.Append("'" + Narration + "',");
            Head_Fields.Append("Mill_Rate,");
            Head_Values.Append("'" + Mill_Rate + "',");
            Head_Fields.Append("Created_By,");
            Head_Values.Append("'" + Created_By + "',");
            Head_Fields.Append("gstratecode,");
            Head_Values.Append("'" + GstRate_Code + "',");
            Head_Fields.Append("Year_Code,");
            Head_Values.Append("'" + Year_Code + "',");
            Head_Fields.Append("Purc_Rate,");
            Head_Values.Append("'" + Purc_Rate + "',");
            Head_Fields.Append("type,");
            Head_Values.Append("'" + type + "',");
            Head_Fields.Append("Branch_Id,");
            Head_Values.Append("'" + Branch_Id + "',");
            Head_Fields.Append("Sell_Note_No,");
            Head_Values.Append("'" + Sell_Note_No + "',");
            Head_Fields.Append("Brokrage,");
            Head_Values.Append("'" + Brokrage + "',");
            Head_Fields.Append("Voucher_No,");
            Head_Values.Append("'" + VoucherNo + "',");
            Head_Fields.Append("tenderid,");
            Head_Values.Append("'" + lblTender_Id.Text + "',");
            Head_Fields.Append("mc,");
            Head_Values.Append("'" + mc + "',");
            Head_Fields.Append("itemcode,");
            Head_Values.Append("'" + itemcode + "',");
            Head_Fields.Append("CashDiff,");
            Head_Values.Append("'" + CashDiff + "',");
            Head_Fields.Append("pt,");
            Head_Values.Append("'" + pt + "',");
            Head_Fields.Append("tf,");
            Head_Values.Append("'" + tf + "',");
            Head_Fields.Append("td,");
            Head_Values.Append("'" + td + "',");
            Head_Fields.Append("vb,");
            Head_Values.Append("'" + vb + "',");
            Head_Fields.Append("bk,");
            Head_Values.Append("'" + bk + "',");
            Head_Fields.Append("ic");
            Head_Values.Append("'" + ic + "'");
            #endregion

            Head_Insert = "insert into " + tblHead + "(" + Head_Fields + ") values(" + Head_Values + ")";
            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = Head_Insert;
            Maindt.Rows.Add(dr);
            #region[details]




            // int tenderDetailid = Convert.ToInt32(clsCommon.getString("select max(tenderdetailid) as tenderdetailid from " + tblDetails + " "));
            int tenderDetailid = Convert.ToInt32(clsCommon.getString("SELECT ifnull(count(tenderdetailid),0) as tenderdetailid from " + tblDetails + " "));
            if (tenderDetailid == 0)
            {
                tenderdetailid = 0;
            }
            else
            {
                tenderDetailid = Convert.ToInt32(clsCommon.getString("select max(tenderdetailid) as tenderdetailid from " + tblDetails + " "));
            }
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {

                tenderDetailid = tenderDetailid + 1;
                Buyer_Party = Convert.ToInt32(Server.HtmlDecode(grdDetail.Rows[i].Cells[Broker_No].Text));
                Buyer = Convert.ToInt32(grdDetail.Rows[i].Cells[Party].Text);
                Buyer_Quantal = Convert.ToDouble(grdDetail.Rows[i].Cells[quantal].Text);
                Sale_Rate = Convert.ToDouble(grdDetail.Rows[i].Cells[saleRate].Text);
                Commission_Rate = Convert.ToDouble(grdDetail.Rows[i].Cells[commissions].Text);
                //string Narration1 = "";
                // DateTime.Now.ToString("dd/MM/yyyy");
                dtype = grdDetail.Rows[i].Cells[delivaryType].Text.ToString();

                if (dtype == "Commission")
                {
                    Delivery_Type = "C";
                }
                if (dtype == "Naka Delivery")
                {
                    Delivery_Type = "N";
                }
                if (dtype == "DO")
                {
                    Delivery_Type = "D";
                }

                // subBroker = grdDetail.Rows[i].Cells[sub_broker].Text.ToString();
                subBroker = Convert.ToInt32(Server.HtmlDecode(grdDetail.Rows[i].Cells[sub_broker].Text));
                Sauda_Date = grdDetail.Rows[i].Cells[saudaDate].Text.ToString();
                Lifting_Date1 = grdDetail.Rows[i].Cells[liftingDate].Text.ToString();

                Sauda_Date = DateTime.Parse(Sauda_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                Lifting_Date = DateTime.Parse(Lifting_Date1, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                Narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[Sauda_Narration].Text);
                ID = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);
                int buyerpartyid = 0;
                int buyerid = 0;
                int subbroker_id = 0;
                //try
                //{
                buyerpartyid = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) from qrymstaccountmaster where Ac_Code='" + Buyer_Party + "' and Company_Code=" + Session["Company_Code"].ToString() + ""));
                //}
                //catch
                //{
                //}
                if (buyerpartyid == 0)
                {
                    buyerpartyid = 0;
                }
                //try
                //{
                buyerid = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) from qrymstaccountmaster where Ac_Code='" + Buyer + "' and Company_Code=" + Session["Company_Code"].ToString() + ""));
                //}
                //catch
                //{
                //}
                if (buyerid == 0)
                {
                    buyerid = 0;
                }
                //try
                //{
                subbroker_id = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) from qrymstaccountmaster where Ac_Code='" + subBroker + "' and Company_Code=" + Session["Company_Code"].ToString() + ""));
                //}
                //catch
                //{
                //}
                if (subbroker_id == 0)
                {
                    subbroker_id = 0;
                }
                Detail_Values.Append("('" + Tender_No + "','" + Company_Code + "','" + Buyer + "','" + Buyer_Quantal + "','" + Sale_Rate + "','" + Commission_Rate + "'" +
                " ,'" + Sauda_Date + "','" + Lifting_Date + "','" + ID + "','" + Narration + "','" + Buyer_Party + "','" + Year_Code + "'" +
                " ,'" + Branch_Id + "','" + Delivery_Type + "','" + subBroker + "','" + subbroker_id + "','" + lblTender_Id.Text + "','" + tenderDetailid + "','" + buyerid + "','" + buyerpartyid + "'),");
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
            Tender_No = Convert.ToInt32(txtTenderNo.Text);

            #region[details]


            string concatid = string.Empty;

            int tenderid = Convert.ToInt32(clsCommon.getString("select max(tenderdetailid) as tenderdetailid from " + tblDetails + " "));
            Tender_No = Convert.ToInt32(txtTenderNo.Text);
            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                subBroker = Convert.ToInt32(Server.HtmlDecode(grdDetail.Rows[i].Cells[sub_broker].Text));
                Buyer = Convert.ToInt32(grdDetail.Rows[i].Cells[Party].Text);
                int buyerid = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code='" + Buyer + "' and Company_Code=" + Session["Company_Code"].ToString() + ""));
                Buyer_Quantal = Convert.ToDouble(grdDetail.Rows[i].Cells[quantal].Text);
                Sale_Rate = Convert.ToDouble(grdDetail.Rows[i].Cells[saleRate].Text);
                Commission_Rate = Convert.ToDouble(grdDetail.Rows[i].Cells[commissions].Text);
                int id = Convert.ToInt32(grdDetail.Rows[i].Cells[16].Text);
                dtype = grdDetail.Rows[i].Cells[delivaryType].Text.ToString();

                if (dtype == "Commission")
                {
                    Delivery_Type = "C";
                }
                if (dtype == "Naka Delivery")
                {
                    Delivery_Type = "N";
                }
                if (dtype == "DO")
                {
                    Delivery_Type = "D";
                }
                Sauda_Date = grdDetail.Rows[i].Cells[saudaDate].Text.ToString();
                Lifting_Date1 = grdDetail.Rows[i].Cells[liftingDate].Text.ToString();

                Sauda_Date = DateTime.Parse(Sauda_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                Lifting_Date = DateTime.Parse(Lifting_Date1, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                Narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[Sauda_Narration].Text);
                ID = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);
                Buyer_Party = 0;
                int subbroker_id = 0;
                //try
                //{
                subbroker_id = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) from qrymstaccountmaster where Ac_Code='" + subBroker + "' and Company_Code=" + Session["Company_Code"].ToString() + ""));
                //}
                //catch
                //{
                //}
                if (subbroker_id == 0)
                {
                    subbroker_id = 0;
                }

                Buyer_Party = Convert.ToInt32(Server.HtmlDecode(grdDetail.Rows[i].Cells[Broker_No].Text));
                int buyerpartyid = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code='" + Buyer_Party + "'  and Company_Code=" + Session["Company_Code"].ToString() + ""));

                if (grdDetail.Rows[i].Cells[rowAction].Text == "A")
                {
                    tenderid = tenderid + 1;

                    Detail_Values.Append("('" + Tender_No + "','" + Company_Code + "','" + Buyer + "','" + Buyer_Quantal + "','" + Sale_Rate + "','" + Commission_Rate + "'" +
                                " ,'" + Sauda_Date + "','" + Lifting_Date + "','" + ID + "','" + Narration + "','" + Buyer_Party + "','" + Year_Code + "'" +
                                " ,'" + Branch_Id + "','" + Delivery_Type + "','" + subBroker + "','" + subbroker_id + "','" + lblTender_Id.Text + "','" + tenderid + "','" + buyerid + "','" + buyerpartyid + "'),");
                }
                if (grdDetail.Rows[i].Cells[rowAction].Text == "U")
                {
                    #region Update

                    #endregion

                    Detail_Update.Append("Buyer=case tenderdetailid when '" + id + "' then '" + Buyer + "'  ELSE Buyer END,");
                    Detail_Update.Append("Buyer_Quantal=case tenderdetailid when '" + id + "' then '" + Buyer_Quantal + "'  ELSE Buyer_Quantal END,");
                    Detail_Update.Append("Sale_Rate=case tenderdetailid when '" + id + "' then '" + Sale_Rate + "'  ELSE Sale_Rate END,");
                    Detail_Update.Append("Commission_Rate=case tenderdetailid when '" + id + "' then '" + Commission_Rate + "'  ELSE Commission_Rate END,");
                    Detail_Update.Append("Sauda_Date=case tenderdetailid when '" + id + "' then '" + Sauda_Date + "'  ELSE Sauda_Date END,");
                    Detail_Update.Append("Lifting_Date=case tenderdetailid when '" + id + "' then '" + Lifting_Date + "'  ELSE Lifting_Date END,");
                    Detail_Update.Append("ID=case tenderdetailid when '" + id + "' then '" + ID + "'  ELSE ID END,");

                    Detail_Update.Append("Narration=case tenderdetailid when '" + id + "' then '" + Narration + "'  ELSE Narration END,");
                    Detail_Update.Append("Buyer_Party=case tenderdetailid when '" + id + "' then '" + Buyer_Party + "'  ELSE Buyer_Party END,");
                    Detail_Update.Append("Branch_Id=case tenderdetailid when '" + id + "' then '" + Branch_Id + "'  ELSE Branch_Id END,");

                    Detail_Update.Append("Delivery_Type=case tenderdetailid when '" + id + "' then '" + Delivery_Type + "'  ELSE Delivery_Type END,");
                    Detail_Update.Append("sub_broker=case tenderdetailid when '" + id + "' then '" + sub_broker + "'  ELSE sub_broker END,");
                    Detail_Update.Append("sbr=case tenderdetailid when '" + id + "' then '" + subbroker_id + "'  ELSE sbr END,");
                    Detail_Update.Append("buyerid=case tenderdetailid when '" + id + "' then '" + buyerid + "'  ELSE buyerid END,");
                    Detail_Update.Append("buyerpartyid=case tenderdetailid when '" + id + "' then '" + buyerpartyid + "'  ELSE buyerpartyid END,");

                    // Detail_Update = "update " + tblDetails + " set " + Detail_Update + " where tenderdetailid='" + id + "'");

                    concatid = concatid + "'" + id + "',";

                }
                if (grdDetail.Rows[i].Cells[rowAction].Text == "D")
                {
                    Detail_Delete.Append("'" + grdDetail.Rows[i].Cells[tenderdetailid].Text + "',");
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
                string Detail_Deleteqry = "delete from " + tblDetails + " where tenderdetailid in(" + Detail_Delete + ")";
                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Detail_Deleteqry;
                Maindt.Rows.Add(dr);
            }
            if (Detail_Update.Length > 0)
            {
                concatid = concatid.Remove(concatid.Length - 1);
                Detail_Update.Remove(Detail_Update.Length - 1, 1);
                string Detail_Updateqry = "update " + tblDetails + " set " + Detail_Update + " where tenderdetailid in(" + concatid + ")";
                dr = null;
                dr = Maindt.NewRow();
                dr["Querys"] = Detail_Updateqry;
                Maindt.Rows.Add(dr);
            }
            #endregion

            #region Update Head Add Fields And Values

            Head_Update.Append("Tender_Date=");
            Head_Update.Append("'" + Tender_Date + "',");
            Head_Update.Append("Lifting_Date=");
            Head_Update.Append("'" + Lifting_Date_Head + "',");
            Head_Update.Append("Mill_Code=");
            Head_Update.Append("'" + Mill_Code + "',");
            Head_Update.Append("Grade=");
            Head_Update.Append("'" + Grade + "',");
            Head_Update.Append("Quantal=");
            Head_Update.Append("'" + Quantal + "',");
            Head_Update.Append("Packing=");
            Head_Update.Append("'" + Packing + "',");
            Head_Update.Append("season=");
            Head_Update.Append("'" + season + "',");
            Head_Update.Append("Bags=");
            Head_Update.Append("'" + Bags + "',");
            Head_Update.Append("Payment_To=");
            Head_Update.Append("'" + Payment_To + "',");
            Head_Update.Append("Tender_From=");
            Head_Update.Append("'" + Tender_From + "',");
            Head_Update.Append("Tender_DO=");
            Head_Update.Append("'" + Tender_DO + "',");
            Head_Update.Append("Voucher_By=");
            Head_Update.Append("'" + Voucher_By + "',");
            Head_Update.Append("Broker=");
            Head_Update.Append("'" + Broker + "',");
            Head_Update.Append("Excise_Rate=");
            Head_Update.Append("'" + Excise_Rate + "',");
            Head_Update.Append("Narration=");
            Head_Update.Append("'" + Narration + "',");
            Head_Update.Append("Mill_Rate=");
            Head_Update.Append("'" + Mill_Rate + "',");
            Head_Update.Append("Modified_By=");
            Head_Update.Append("'" + Modified_By + "',");
            Head_Update.Append("gstratecode=");
            Head_Update.Append("'" + GstRate_Code + "',");
            Head_Update.Append("Purc_Rate=");
            Head_Update.Append("'" + Purc_Rate + "',");
            Head_Update.Append("type=");
            Head_Update.Append("'" + type + "',");
            Head_Update.Append("Branch_Id=");
            Head_Update.Append("'" + Branch_Id + "',");
            Head_Update.Append("Sell_Note_No=");
            Head_Update.Append("'" + Sell_Note_No + "',");
            Head_Update.Append("Brokrage=");
            Head_Update.Append("'" + Brokrage + "',");
            Head_Update.Append("Voucher_No=");
            Head_Update.Append("'" + VoucherNo + "',");
            Head_Update.Append("CashDiff=");
            Head_Update.Append("'" + CashDiff + "',");

            Head_Update.Append("mc=");
            Head_Update.Append("'" + mc + "',");
            Head_Update.Append("itemcode=");
            Head_Update.Append("'" + itemcode + "',");
            Head_Update.Append("pt=");
            Head_Update.Append("'" + pt + "',");
            Head_Update.Append("tf=");
            Head_Update.Append("'" + tf + "',");
            Head_Update.Append("td=");
            Head_Update.Append("'" + td + "',");
            Head_Update.Append("vb=");
            Head_Update.Append("'" + vb + "',");
            Head_Update.Append("bk=");
            Head_Update.Append("'" + bk + "',");
            Head_Update.Append("ic=");
            Head_Update.Append("case when 0='" + ic + "' then null else '" + ic + "' end");

            string Head_Updateqry = "update " + tblHead + " set " + Head_Update + " where tenderid='" + lblTender_Id.Text + "'";

            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = Head_Updateqry;
            Maindt.Rows.Add(dr);
            #endregion

            flag = 2;


        }

        #region LV Effect
        string NO = string.Empty;
        if (AUTO_VOUCHER == "YES" && drpResale.SelectedValue == "R" || drpResale.SelectedValue == "W")
        {
            this.LV_Posting();
            if (lblVoucherNo.Text == string.Empty || lblVoucherNo.Text == "0")
            {
                SalePurcdt = new DataTable();
                SalePurcdt = clsLocalVoucher.LV_Posting(1, LV, "LV");
                Maindt.Merge(SalePurcdt);
                NO = Convert.ToString(LV.LV_Doc_No);
            }
            else
            {
                SalePurcdt = new DataTable();
                SalePurcdt = clsLocalVoucher.LV_Posting(flag, LV, "LV");
                Maindt.Merge(SalePurcdt);
                NO = Convert.ToString(LV.LV_Doc_No);
            }

            qry = "update nt_1_tender set Voucher_No=case when 0='" + LV.LV_Doc_No + "' then null else '" + LV.LV_Doc_No + "' end where " +
               " tenderid=" + lblTender_Id.Text + "";
            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = qry;
            Maindt.Rows.Add(dr);
        }
        #endregion
        #endregion

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
            hdnf.Value = lblTender_Id.Text;
            clsButtonNavigation.enableDisable("S");
            this.makeEmptyForm("S");
            qry = getDisplayQuery();
            this.fetchRecord(qry);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update!')", true);
        }
        else
        {
            btnSave.Enabled = true;
            setFocusControl(btnSave);
        }
    }


    #endregion

    #region [saveTenderDetails]
    private void saveTenderDetails(string tenderNo, string companyCode)
    {
        try
        {
            string str = "";
            if (grdDetail.Rows.Count > 0)
            {
                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    Int32 Buyer = Convert.ToInt32(grdDetail.Rows[i].Cells[3].Text);
                    double Buyer_Quantal = Convert.ToDouble(grdDetail.Rows[i].Cells[7].Text);
                    double Sale_Rate = Convert.ToDouble(grdDetail.Rows[i].Cells[8].Text);
                    double Commission_Rate = Convert.ToDouble(grdDetail.Rows[i].Cells[9].Text);
                    string Narration = "";
                    // DateTime.Now.ToString("dd/MM/yyyy");
                    string dtype = grdDetail.Rows[i].Cells[13].Text.ToString();
                    string Delivery_Type = "";
                    if (dtype == "Commission")
                    {
                        Delivery_Type = "C";
                    }
                    else
                    {
                        Delivery_Type = "N";
                    }
                    string Sauda_Date = grdDetail.Rows[i].Cells[10].Text.ToString();
                    string Lifting_Date = grdDetail.Rows[i].Cells[11].Text.ToString();

                    Sauda_Date = DateTime.Parse(Sauda_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                    Lifting_Date = DateTime.Parse(Lifting_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                    Narration = Server.HtmlDecode(grdDetail.Rows[i].Cells[12].Text);
                    int ID = Convert.ToInt32(grdDetail.Rows[i].Cells[2].Text);
                    double Buyer_Party = 0;
                    //if (grdDetail.Rows.Count == 1)
                    //{
                    //    Buyer_Party = Convert.ToInt32(txtBroker.Text);
                    //}
                    //else
                    //{

                    Buyer_Party = Convert.ToInt32(Server.HtmlDecode(grdDetail.Rows[i].Cells[5].Text));

                    //}
                    using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                    {
                        if (grdDetail.Rows[i].Cells[14].Text != "N")   //For N do nothing for that row
                        {
                            if (grdDetail.Rows[i].Cells[14].Text == "A")
                            {
                                #region check whether same id is inserted in table already or not (if then insert next no)
                                string id = clsCommon.getString("select AutoID from " + tblDetails + " where Tender_No='" + tenderNo + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and year_code=" + Convert.ToInt32(Session["year"].ToString()) + " and ID='" + ID + "'");
                                if (id != string.Empty && id != "0")
                                {
                                    //this id is already inserted Get max id
                                    string newId = clsCommon.getString("select max(ID) from " + tblDetails + " where Tender_No='" + tenderNo + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'");
                                    ID = Convert.ToInt32(newId) + 1;
                                }
                                #endregion

                                obj.flag = 1;
                                obj.columnNm = "Tender_No,Company_Code,Buyer,Buyer_Quantal,Sale_Rate,Commission_Rate,Sauda_Date,Lifting_Date,Narration,ID,Buyer_Party,IsActive,year_code,Branch_Id,Delivery_Type";
                                obj.values = "'" + tenderNo + "','" + companyCode + "','" + Buyer + "','" + Buyer_Quantal + "','" + Sale_Rate + "','" + Commission_Rate
                                    + "','" + Sauda_Date + "','" + Lifting_Date + "','" + Narration + "','" + ID + "','" + Buyer_Party + "','True','"
                                    + Convert.ToInt32(Session["year"].ToString()) + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Delivery_Type + "'";
                            }
                            if (grdDetail.Rows[i].Cells[14].Text == "U")
                            {
                                obj.flag = 2;
                                obj.columnNm = " Buyer='" + Buyer + "',Buyer_Quantal='" + Buyer_Quantal + "',Sale_Rate='" + Sale_Rate + "',Commission_Rate='"
                                    + Commission_Rate + "',Sauda_Date='" + Sauda_Date + "',Lifting_Date='" + Lifting_Date + "',Narration='" + Narration + "',Buyer_Party='"
                                    + Buyer_Party + "',Delivery_Type='" + Delivery_Type + "' where Tender_No='" + tenderNo + "' and Company_Code='"
                                    + Convert.ToInt32(Session["Company_Code"].ToString()) + "'  and year_code='" + Convert.ToInt32(Session["year"].ToString())
                                    + "'  and ID='" + ID + "'";
                                obj.values = "none";
                            }
                            if (grdDetail.Rows[i].Cells[14].Text == "D")
                            {
                                obj.flag = 3;
                                obj.columnNm = "Tender_No='" + tenderNo + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and ID='" + ID
                                    + "' and year_code='" + Convert.ToInt32(Session["year"].ToString()) + "'";
                                obj.values = "none";
                            }
                            obj.tableName = tblDetails;

                            DataSet ds = new DataSet();
                            ds = obj.insertAccountMaster(ref str);
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

    #region [saveData]
    private string saveData()
    {
        string qry = "";
        try
        {
            string str = "";
            bool isValidated = true;
            double Diff_Amount = lbldiff.Text != string.Empty ? Convert.ToDouble(lbldiff.Text) : 0.00;
            int docno = 0;
            //Int32 Voucher_No = 0;
            Int32 Tender_No = Convert.ToInt32(txtTenderNo.Text);
            Int32 Company_Code = Convert.ToInt32(Convert.ToInt32(Session["Company_Code"].ToString()));
            string Tender_Date = DateTime.Parse(txtDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            string Lifting_Date = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            Int32 Mill_Code = Convert.ToInt32(txtMillCode.Text);
            string Sell_Note_No = txtSellNoteNo.Text;
            double Mill_Rate = double.Parse(txtMillRate.Text);
            string Grade = txtGrade.Text;
            float Quantal = float.Parse(txtQuantal.Text);
            int Packing = Convert.ToInt32(txtPacking.Text);
            int Bags = Convert.ToInt32(Math.Round(float.Parse(txtBags.Text)));
            int VOUCHERAMOUNT = Convert.ToInt32(lblAmount.Text);
            double DIFF = Convert.ToDouble(lbldiff.Text);
            bool isNumeric;
            int n;
            double PURCHASE_RATE = txtPurcRate.Text != string.Empty ? Convert.ToDouble(txtPurcRate.Text) : 0.00;
            int Payment_To, Tender_From, Tender_DO, Voucher_By, Broker;
            Payment_To = string.IsNullOrEmpty(txtPaymentTo.Text) ? Mill_Code : int.Parse(txtPaymentTo.Text);
            Tender_From = string.IsNullOrEmpty(txtTenderFrom.Text) ? 0 : int.Parse(txtTenderFrom.Text);
            Tender_DO = string.IsNullOrEmpty(txtDO.Text) ? 2 : int.Parse(txtDO.Text);
            Voucher_By = string.IsNullOrEmpty(txtVoucherBy.Text) ? 0 : int.Parse(txtVoucherBy.Text);
            Broker = string.IsNullOrEmpty(txtBroker.Text) ? 2 : int.Parse(txtBroker.Text);
            if (int.TryParse(txtPaymentTo.Text, out n))
            {
                Payment_To = n;
            }
            if (int.TryParse(txtTenderFrom.Text, out n))
            {
                Tender_From = n;
            }
            if (int.TryParse(txtDO.Text, out n))
            {
                Tender_DO = n;
            }
            if (int.TryParse(txtVoucherBy.Text, out n))
            {
                Voucher_By = n;
            }
            if (int.TryParse(txtBroker.Text, out n))
            {
                Broker = n;
            }

            #region previous code
            //int Payment_To = defaultAccountCode;
            //int n;
            //bool isNumeric = int.TryParse(txtPaymentTo.Text, out n);
            //if (isNumeric == true)
            //    Payment_To = n;
            //int Tender_From = defaultAccountCode;

            //isNumeric = int.TryParse(txtTenderFrom.Text, out n);
            //if (isNumeric == true)
            //Tender_From = n;

            //int Tender_DO = defaultAccountCode;
            //isNumeric = int.TryParse(txtDO.Text, out n);
            //if (isNumeric == true)
            //    Tender_DO = n;
            //int Voucher_By = defaultAccountCode;
            //isNumeric = int.TryParse(txtVoucherBy.Text, out n);
            //if (isNumeric == true)
            //    Voucher_By = n;
            //int Broker = defaultAccountCode;
            //isNumeric = int.TryParse(txtBroker.Text, out n);
            //if (isNumeric == true)
            //    Broker = n;
            #endregion

            float m = 0;
            float Excise_Rate = 0;
            isNumeric = float.TryParse(txtExciseRate.Text, out m);
            if (isNumeric == true)
                Excise_Rate = m;
            string Narration = txtNarration.Text;
            string userName = Session["user"].ToString();
            string Year_Code = Convert.ToInt32(Session["year"].ToString()).ToString();
            float Purc_Rate = 0;
            isNumeric = float.TryParse(txtPurcRate.Text, out m);
            if (isNumeric == true)
                Purc_Rate = m;
            string type = drpResale.SelectedValue;
            int Branch_Id = Convert.ToInt32(Convert.ToInt32(Session["Branch_Code"].ToString()));
            string Created_By = clsGV.user;
            Created_By = Session["user"].ToString();
            //lblCreatedBy.Text = Created_By.ToString();
            string Modified_By = clsGV.user;
            Modified_By = Session["user"].ToString();
            //lblModifiedBy.Text = Modified_By.ToString();
            AUTO_VOUCHER = clsCommon.getString("select AutoVoucher from " + tblPrefix + "CompanyParameters where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "'");
            string myNarration = string.Empty;
            millShortName = clsCommon.getString("select short_name from " + AccountMasterTable + " where ac_code=" + Mill_Code + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            DOShortname = clsCommon.getString("select short_name from " + AccountMasterTable + " where ac_code=" + Tender_DO + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            voucherbyshortname = clsCommon.getString("select short_name from " + AccountMasterTable + " where ac_code=" + Voucher_By + " and company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            if (drpResale.SelectedValue != "M")
            {
                if (PURCHASE_RATE > 0)
                {
                    myNarration = "Qntl " + Quantal + "  " + millShortName + " (M.R." + Mill_Rate + " P.R." + PURCHASE_RATE + ")";
                }
            }

            double Brokrage = txtBrokrage.Text != string.Empty ? Convert.ToDouble(txtBrokrage.Text) : 0.0;

            //else
            //{
            //    myNarration = "Qntl " + Quantal + "  " + millShortName + " (M.R." + MILL_RATE + " P.R." + SALE_RATE + ")";
            //}

            #region ------------- Valiation ---------------
            if (txtTenderNo.Text == string.Empty)
            {
                isValidated = false;
                txtTenderNo.Focus();
            }
            if (txtMillCode.Text == string.Empty)
            {
                isValidated = false;
                txtMillCode.Focus();
            }
            else
            {
                string s = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtMillCode.Text
                    + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Ac_type='M'");
                if (s == string.Empty)
                {
                    isValidated = false;
                    txtMillCode.Text = string.Empty;
                    lblMillName.Text = string.Empty;
                    txtMillCode.Focus();
                }
            }
            if (txtMillRate.Text == string.Empty)
            {
                isValidated = false;
                txtMillRate.Focus();
            }
            if (txtGrade.Text == string.Empty)
            {
                isValidated = false;
                txtGrade.Focus();
            }
            if (txtQuantal.Text == string.Empty)
            {
                isValidated = false;
                txtQuantal.Focus();
            }
            if (txtPacking.Text == string.Empty)
            {
                isValidated = false;
                txtPacking.Focus();
            }

            if (drpResale.SelectedValue == "R" || drpResale.SelectedValue == "W")
            {
                if (txtPaymentTo.Text == string.Empty)
                {
                    isValidated = false;
                    setFocusControl(txtPaymentTo);
                }
                else
                {
                    string v = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtPaymentTo.Text + "' and Company_Code="
                        + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (v != string.Empty)
                    {
                        isValidated = true;
                    }
                    else
                    {
                        txtPaymentTo.Text = string.Empty;
                        isValidated = false;
                        setFocusControl(txtPaymentTo);
                    }
                }
                if (txtPurcRate.Text == string.Empty)
                {
                    isValidated = false;
                    txtPurcRate.Focus();
                }
                if (txtDO.Text == string.Empty)
                {
                    isValidated = false;
                    setFocusControl(txtDO);
                }
                if (txtVoucherBy.Text == string.Empty)
                {
                    if (drpResale.SelectedValue == "R" || drpResale.SelectedValue == "W")
                    {
                        isValidated = false;
                        setFocusControl(txtVoucherBy);
                    }
                    else
                    {
                        Voucher_By = int.Parse(txtVoucherBy.Text);
                    }
                }
                else
                {
                    string v = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtDO.Text
                        + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (v != string.Empty && v != "0")
                    {
                        isValidated = true;
                        if (txtTenderFrom.Text != string.Empty)
                        {
                            Tender_From = int.Parse(txtTenderFrom.Text);
                            isValidated = true;
                        }
                        else
                        {
                            v = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtTenderFrom.Text
                                + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            if (v != string.Empty && v != "0")
                            {
                                Tender_From = Convert.ToInt32(txtTenderFrom.Text);
                                isValidated = true;
                            }
                            else
                            {
                                txtTenderFrom.Text = "0";
                                Tender_From = Convert.ToInt32(txtTenderFrom.Text);
                                txtTenderFrom.Text = Tender_From.ToString();
                                isValidated = true;
                            }
                        }
                        if (txtVoucherBy.Text != string.Empty)
                        {

                            Voucher_By = int.Parse(txtVoucherBy.Text);
                        }
                        else
                        {
                            v = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtVoucherBy.Text
                                + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            if (v != string.Empty && v != "0")
                            {
                                Voucher_By = Convert.ToInt32(txtVoucherBy.Text);
                                isValidated = true;
                            }
                            else
                            {
                                txtVoucherBy.Text = "0";
                                Voucher_By = Convert.ToInt32(txtVoucherBy.Text);
                                txtVoucherBy.Text = Voucher_By.ToString();
                                isValidated = true;
                            }
                        }
                        if (txtBroker.Text != string.Empty)
                        {
                            Broker = int.Parse(txtBroker.Text);
                        }
                        else
                        {
                            v = clsCommon.getString("select Ac_Name_E from " + AccountMasterTable + " where Ac_Code='" + txtBroker.Text + "' and Company_Code="
                                + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            if (v != string.Empty && v != "0")
                            {
                                Broker = Convert.ToInt32(txtBroker.Text);
                                isValidated = true;
                            }
                            else
                            {
                                txtBroker.Text = "2";
                                Broker = Convert.ToInt32(txtBroker.Text);
                                txtBroker.Text = Broker.ToString();
                                isValidated = true;
                            }
                        }
                    }
                    else
                    {
                        isValidated = false;
                        txtDO.Text = string.Empty;
                        setFocusControl(txtDO);
                    }
                }
            }

            #endregion

            #region[Gst Cal]
            string aaa = "";
            if (txtVoucherBy.Text.Trim() != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtVoucherBy.Text);
                if (a == true)
                {
                    aaa = clsCommon.getString("select ifNULL(GSTStateCode,0) from " + AccountMasterTable + " where Company_Code="
                        + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Ac_Code=" + txtVoucherBy.Text + "");
                }
            }
            int partygstStateCode = 0;
            if (aaa.Trim().ToString() != "")
            {
                partygstStateCode = Convert.ToInt32(aaa);
            }

            int companyGstStateCode = Convert.ToInt32(Session["CompanyGSTStateCode"].ToString());
            //  string GSTRateCode = "18";
            //double GSTRate = Convert.ToDouble(clsCommon.getString("select Rate from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
            //double cgstrate = Convert.ToDouble(clsCommon.getString("select CGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
            //double sgstrate = Convert.ToDouble(clsCommon.getString("select SGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
            //double igstrate = Convert.ToDouble(clsCommon.getString("select IGST from " + tblPrefix + "GSTRateMaster where Doc_no=" + GSTRateCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + ""));
            //double GSTRate = 18.00;
            double cgstrate = 9.00;
            double sgstrate = 9.00;
            double igstrate = 18.00;
            double CGSTAmountForPS = 0.0;
            double SGSTAmountForPS = 0.0;
            double IGSTAmountForPS = 0.0;

            double CGSTRateForPS = 0.00;
            double SGSTRateForPS = 0.00;
            double IGSTRateForPS = 0.00;
            double taxmillamt = Convert.ToDouble(lblAmount.Text);
            double vouchergrandamnt = 0.00;
            if (companyGstStateCode == partygstStateCode)
            {
                CGSTRateForPS = cgstrate;
                // double millamount = subtotal;


                double cgsttaxAmountOnMR = Math.Round((taxmillamt * cgstrate / 100), 2);
                //double cgstExMillRate = Math.Round(Math.Abs((mill_rate / (mill_rate + cgsttaxAmountOnMR) * mill_rate)), 2);
                //double cgstRateAmountOnMR = Math.Round(Math.Abs(mill_rate - cgstExMillRate), 2);
                CGSTAmountForPS = Math.Round(cgsttaxAmountOnMR, 2);

                SGSTRateForPS = sgstrate;
                double sgsttaxAmountOnMR = Math.Round((taxmillamt * sgstrate / 100), 2);
                //double sgstExMillRate = Math.Round(Math.Abs((mill_rate / (mill_rate + sgsttaxAmountOnMR) * mill_rate)), 2);
                //double sgstRateAmountOnMR = Math.Round(Math.Abs(mill_rate - sgstExMillRate), 2);
                SGSTAmountForPS = Math.Round(sgsttaxAmountOnMR, 2);
            }
            else
            {
                IGSTRateForPS = igstrate;
                double igsttaxAmountOnMR = ((taxmillamt) * igstrate / 100);
                //double igstExMillRate = Math.Round(Math.Abs((mill_rate / (mill_rate + igsttaxAmountOnMR) * mill_rate)), 2);
                //double igstRateAmountOnMR = Math.Round(Math.Abs(mill_rate - igstExMillRate), 2);
                IGSTAmountForPS = Math.Round(igsttaxAmountOnMR, 2);
            }
            vouchergrandamnt = taxmillamt + CGSTAmountForPS + SGSTAmountForPS + IGSTAmountForPS;
            #endregion

            if (isValidated == true)
            {
                using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                {
                    if (ViewState["mode"] != null)
                    {
                        //------- Check whether tender no already exist or not ---------
                        string no = clsCommon.getString("select Tender_No from " + tblHead + " where Tender_No=" + txtTenderNo.Text + " and Company_Code="
                            + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                        if (ViewState["mode"].ToString() == "I")
                        {
                            if (no == string.Empty)
                            {
                                //same tender no does not exist
                                Tender_No = Convert.ToInt32(txtTenderNo.Text);
                            }
                            else
                            {
                                //get next no and save
                                this.getMaxCode();
                                Tender_No = Convert.ToInt32(txtTenderNo.Text);
                            }
                            //if (AUTO_VOUCHER == "YES")
                            //{
                            //    if (drpResale.SelectedValue == "R" || drpResale.SelectedValue == "W")
                            //    {
                            //        if (Mill_Rate > PURCHASE_RATE)
                            //        {
                            //            docno = MaxVoucher();
                            //        }
                            //    }
                            //}
                            obj.tableName = tblHead;
                            obj.flag = 1;
                            obj.columnNm = "Tender_No,Company_Code,Tender_Date,Lifting_Date,Mill_Code,Grade,Quantal,Packing,Bags,Payment_To,Tender_From,Tender_DO,Voucher_By,Broker,Excise_Rate,Narration,Mill_Rate,Created_By,Year_Code,Purc_Rate,type,Branch_Id,Sell_Note_No,Brokrage";
                            obj.values = "'" + Tender_No + "','" + Company_Code + "','" + Tender_Date + "','" + Lifting_Date + "','" + Mill_Code + "','" + Grade + "','" + Quantal + "','" + Packing + "','" + Bags + "','" + Payment_To + "','" + Tender_From + "','" + Tender_DO + "','" + Voucher_By + "','" + Broker + "','" + Excise_Rate + "','" + Narration + "','" + Mill_Rate + "','" + user + "','" + Year_Code + "','" + Purc_Rate + "','" + type + "','" + Branch_Id + "','" + Sell_Note_No + "','" + Brokrage + "'";
                            obj.insertAccountMaster(ref str);

                            #region Saving To Voucher Table
                            if (AUTO_VOUCHER == "YES")
                            {
                                if (drpResale.SelectedValue == "R" || drpResale.SelectedValue == "W")
                                {
                                    if (Mill_Rate > PURCHASE_RATE)
                                    {
                                        docno = MaxVoucher();
                                        vouchernumber.Value = Convert.ToString(docno);
                                        ///int docno =Convert.ToInt32(clsCommon.getString("Select MAX(Doc_No)+1 from NT_1_Voucher"));
                                        vouchernumber.Value = docno.ToString();
                                        lblVoucherNo.Text = vouchernumber.Value;
                                        obj.flag = 1;
                                        obj.tableName = "" + tblPrefix + "Voucher";
                                        obj.columnNm = "Tran_Type,Doc_No,Doc_Date,Ac_Code,Suffix,Company_Code,Year_Code,Branch_Code,Tender_No,Mill_Code," +
                                            "Grade,Quantal,PACKING,BAGS,Payment_To,Tender_From,Tender_DO,Broker_CODE,Mill_Rate,Purchase_Rate,Diff_Amount," +
                                            "Voucher_Amount,Narration1,Diff_Type,Created_By,CGSTRate,CGSTAmount,SGSTRate,SGSTAmount,IGSTRate,IGSTAmount,GstRateCode,TaxableAmount";
                                        obj.values = "'" + "LV" + "','" + docno + "','" + Tender_Date + "','" + Voucher_By + "','" + string.Empty.Trim() + "','"
                                            + Company_Code + "','" + Year_Code + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','"
                                            + Tender_No + "','" + Mill_Code + "','" + Grade + "','" + Quantal + "','" + Packing + "','" + Bags + "','"
                                            + Payment_To + "','" + Tender_From + "','" + Tender_DO + "','" + Broker + "','" + Mill_Rate + "','" + Purc_Rate
                                            + "','" + Diff_Amount + "','" + vouchergrandamnt + "','"
                                            + Narration + myNarration + "','TD','" + user + "','" + CGSTRateForPS
                                            + "','" + CGSTAmountForPS + "','" + SGSTRateForPS + "','" + SGSTAmountForPS + "','" + IGSTRateForPS + "','"
                                            + IGSTAmountForPS + "','2','" + taxmillamt + "'";
                                        obj.insertAccountMaster(ref str);

                                        string updateVNo = "Update " + tblPrefix + "Tender SET Voucher_No=" + docno + " where Company_Code="
                                            + Company_Code + " and Year_Code=" + Year_Code + " and Tender_No=" + Tender_No + "";
                                        clsDAL.SimpleQuery(updateVNo);
                                    }
                                    else
                                    {
                                        if (drpResale.SelectedValue == "W" || drpResale.SelectedValue == "R")
                                        {
                                            if (Mill_Rate != PURCHASE_RATE)
                                            {
                                                string todaysDate = DateTime.Now.ToString("yyyy-MM-dd");
                                                DateTime dateOfToday = DateTime.ParseExact(todaysDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                                                DateTime dateToCompare = DateTime.ParseExact("2017-01-20", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                                                int result = DateTime.Compare(dateOfToday, dateToCompare);
                                                if (result > 0)
                                                {
                                                    docno = MaxVoucher();
                                                    vouchernumber.Value = Convert.ToString(docno);
                                                    ///int docno =Convert.ToInt32(clsCommon.getString("Select MAX(Doc_No)+1 from NT_1_Voucher"));
                                                    vouchernumber.Value = docno.ToString();
                                                    lblVoucherNo.Text = vouchernumber.Value;
                                                    obj.flag = 1;
                                                    obj.tableName = "" + tblPrefix + "Voucher";
                                                    obj.columnNm = "Tran_Type,Doc_No,Doc_Date,Ac_Code,Suffix,Company_Code,Year_Code,Branch_Code,Tender_No," +
                                                        "Mill_Code,Grade,Quantal,PACKING,BAGS,Payment_To,Tender_From,Tender_DO,Broker_CODE,Mill_Rate," +
                                                        "Purchase_Rate,Diff_Amount,Voucher_Amount,Narration1,Diff_Type,Created_By,CGSTRate,CGSTAmount," +
                                                        "SGSTRate,SGSTAmount,IGSTRate,IGSTAmount,GstRateCode,TaxableAmount";
                                                    obj.values = "'" + "LV" + "','" + docno + "','" + Tender_Date + "','" + Voucher_By + "','"
                                                        + string.Empty.Trim() + "','" + Company_Code + "','" + Year_Code + "','"
                                                        + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tender_No + "','" + Mill_Code
                                                        + "','" + Grade + "','" + Quantal + "','" + Packing + "','" + Bags + "','" + Payment_To
                                                        + "','" + Tender_From + "','" + Tender_DO + "','" + Broker + "','" + Mill_Rate + "','"
                                                        + Purc_Rate + "','" + Diff_Amount + "','" + VOUCHERAMOUNT + "','" + Narration
                                                        + myNarration + "','TD','" + user + "','" + CGSTRateForPS
                                                        + "','" + CGSTAmountForPS + "','" + SGSTRateForPS + "','" + SGSTAmountForPS + "','" + IGSTRateForPS + "','"
                                                        + IGSTAmountForPS + "','2','" + taxmillamt + "'";
                                                    obj.insertAccountMaster(ref str);

                                                    string updateVNo = "Update " + tblPrefix + "Tender SET Voucher_No=" + docno + " where Company_Code="
                                                        + Company_Code + " and Year_Code=" + Year_Code + " and Tender_No=" + Tender_No + "";
                                                    clsDAL.SimpleQuery(updateVNo);

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                        }

                        else
                        {
                            if (no != string.Empty)
                            {
                                //tender no does exist then update  
                                obj.flag = 2;
                                obj.columnNm = "Tender_Date='" + Tender_Date + "',Lifting_Date='" + Lifting_Date + "',Mill_Code='"
                                    + Mill_Code + "',Grade='" + Grade + "',Quantal='" + Quantal + "',Packing='" + Packing + "',Bags='"
                                    + Bags + "',Payment_To='" + Payment_To + "',Tender_From='" + Tender_From + "',Tender_DO='" + Tender_DO
                                    + "',Voucher_By='" + Voucher_By + "',Broker='" + Broker + "',Excise_Rate='" + Excise_Rate + "',Narration='"
                                    + Narration + "',Mill_Rate='" + Mill_Rate + "',Modified_By='" + user + "',Purc_Rate='" + Purc_Rate + "',type='"
                                    + type + "',Sell_Note_No='" + Sell_Note_No + "',Brokrage='" + Brokrage + "' where Tender_No='" + Tender_No
                                    + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Year_Code='"
                                    + Convert.ToInt32(Session["year"].ToString()) + "'";
                                obj.values = "none";
                                obj.tableName = tblHead;
                                obj.insertAccountMaster(ref str);

                                //Updating voucher table
                                if (drpResale.SelectedValue == "R" || drpResale.SelectedValue == "W")
                                {
                                    int Voucher_No = int.Parse(vouchernumber.Value.TrimStart());
                                    if (Voucher_No != 0)
                                    {
                                        obj.flag = 2;
                                        obj.tableName = "" + tblPrefix + "Voucher";
                                        //obj.columnNm = "Tran_Type='" + "LV" + "',Doc_Date='" + Tender_Date + "',Ac_Code='" + Voucher_By
                                        //    + "',Suffix='" + string.Empty.Trim() + "',Company_Code='" + Company_Code + "',Year_Code='"
                                        //    + Year_Code + "',Tender_No='" + Tender_No + "',Mill_Code='" + Mill_Code + "',Grade='" + Grade
                                        //    + "',Quantal='" + Quantal + "',PACKING='" + Packing + "',BAGS='" + Bags + "',Payment_To='" + Payment_To
                                        //    + "',Tender_From='" + Tender_From + "',Tender_DO='" + Tender_DO + "',Broker_CODE='" + Broker + "',Mill_Rate='"
                                        //    + Mill_Rate + "',Purchase_Rate='" + Purc_Rate + "',Diff_Amount='" + Diff_Amount + "',Voucher_Amount='"
                                        //    + VOUCHERAMOUNT + "',Narration1='" + Narration + "',Modified_By='" + user + "' where " +
                                        //                " Company_Code='" + Company_Code + "' and  Year_Code='" + Year_Code + "'  and Tran_Type='" + "LV"
                                        //                + "' and Doc_No='" + Voucher_No + "'";

                                        obj.columnNm = "Tran_Type='" + "LV" + "',Doc_Date='" + Tender_Date + "',Ac_Code='" + Voucher_By
                                           + "',Suffix='" + string.Empty.Trim() + "',Company_Code='" + Company_Code + "',Year_Code='"
                                           + Year_Code + "',Tender_No='" + Tender_No + "',Mill_Code='" + Mill_Code + "',Grade='" + Grade
                                           + "',Quantal='" + Quantal + "',PACKING='" + Packing + "',BAGS='" + Bags + "',Payment_To='" + Payment_To
                                           + "',Tender_From='" + Tender_From + "',Tender_DO='" + Tender_DO + "',Broker_CODE='" + Broker + "',Mill_Rate='"
                                           + Mill_Rate + "',Purchase_Rate='" + Purc_Rate + "',Diff_Amount='" + Diff_Amount + "',Voucher_Amount='"
                                           + vouchergrandamnt + "',Narration1='" + Narration + "',Modified_By='" + user + "',CGSTRate='" + CGSTRateForPS
                                           + "',CGSTAmount='" + CGSTAmountForPS + "',SGSTRate='" + SGSTRateForPS + "',SGSTAmount='" + SGSTAmountForPS
                                           + "',IGSTRate='" + IGSTRateForPS + "',IGSTAmount='" + IGSTAmountForPS + "',TaxableAmount='" + taxmillamt + "' where " +
                                                       " Company_Code='" + Company_Code + "' and  Year_Code='" + Year_Code + "'  and Tran_Type='" + "LV"
                                                       + "' and Doc_No='" + Voucher_No + "'";
                                        obj.values = "none";
                                        ds = obj.insertAccountMaster(ref str);
                                    }
                                    else
                                    {
                                        if (AUTO_VOUCHER == "YES")
                                        {
                                            if (drpResale.SelectedValue == "R" || drpResale.SelectedValue == "W")
                                            {
                                                if (Mill_Rate > PURCHASE_RATE)
                                                {
                                                    docno = MaxVoucher();
                                                    vouchernumber.Value = Convert.ToString(docno);
                                                    ///int docno =Convert.ToInt32(clsCommon.getString("Select MAX(Doc_No)+1 from NT_1_Voucher"));
                                                    vouchernumber.Value = docno.ToString();
                                                    lblVoucherNo.Text = vouchernumber.Value;
                                                    obj.flag = 1;
                                                    obj.tableName = "" + tblPrefix + "Voucher";
                                                    obj.columnNm = "Tran_Type,Doc_No,Doc_Date,Ac_Code,Suffix,Company_Code,Year_Code,Branch_Code,Tender_No,Mill_Code," +
                                                    "Grade,Quantal,PACKING,BAGS,Payment_To,Tender_From,Tender_DO,Broker_CODE,Mill_Rate,Purchase_Rate,Diff_Amount," +
                                                    "Voucher_Amount,Narration1,Diff_Type,Created_By,CGSTRate,CGSTAmount," +
                                                        "SGSTRate,SGSTAmount,IGSTRate,IGSTAmount,GstRateCode,TaxableAmount";
                                                    obj.values = "'" + "LV" + "','" + docno + "','" + Tender_Date + "','" + Voucher_By
                                                        + "','" + string.Empty.Trim() + "','" + Company_Code + "','" + Year_Code + "','"
                                                        + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tender_No + "','"
                                                        + Mill_Code + "','" + Grade + "','" + Quantal + "','" + Packing + "','" + Bags + "','" +
                                                        Payment_To + "','" + Tender_From + "','" + Tender_DO + "','" + Broker + "','" + Mill_Rate
                                                        + "','" + Purc_Rate + "','" + Diff_Amount + "','" + VOUCHERAMOUNT + "','" + Narration + myNarration
                                                        + "','TD','" + user + "','" + CGSTRateForPS
                                                        + "','" + CGSTAmountForPS + "','" + SGSTRateForPS + "','" + SGSTAmountForPS + "','" + IGSTRateForPS + "','"
                                                        + IGSTAmountForPS + "','2','" + taxmillamt + "'";
                                                    obj.insertAccountMaster(ref str);
                                                    string updateVNo = "Update " + tblPrefix + "Tender SET Voucher_No=" + docno + " where Company_Code="
                                                        + Company_Code + " and Year_Code=" + Year_Code + " and Tender_No=" + Tender_No + "";
                                                    clsDAL.SimpleQuery(updateVNo);
                                                }
                                                else
                                                {
                                                    if (drpResale.SelectedValue == "W" || drpResale.SelectedValue == "R")
                                                    {
                                                        if (Mill_Rate != PURCHASE_RATE)
                                                        {

                                                            string todaysDate = DateTime.Now.ToString("yyyy-MM-dd");
                                                            DateTime dateOfToday = DateTime.ParseExact(todaysDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                                                            DateTime dateToCompare = DateTime.ParseExact("2017-01-20", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                                                            int result = DateTime.Compare(dateOfToday, dateToCompare);
                                                            if (result > 0)
                                                            {
                                                                docno = MaxVoucher();
                                                                vouchernumber.Value = Convert.ToString(docno);
                                                                ///int docno =Convert.ToInt32(clsCommon.getString("Select MAX(Doc_No)+1 from NT_1_Voucher"));
                                                                vouchernumber.Value = docno.ToString();
                                                                lblVoucherNo.Text = vouchernumber.Value;
                                                                obj.flag = 1;
                                                                obj.tableName = "" + tblPrefix + "Voucher";
                                                                obj.columnNm = "Tran_Type,Doc_No,Doc_Date,Ac_Code,Suffix,Company_Code,Year_Code,Branch_Code," +
                                                                    "Tender_No,Mill_Code,Grade,Quantal,PACKING,BAGS,Payment_To,Tender_From,Tender_DO,Broker_CODE," +
                                                                "Mill_Rate,Purchase_Rate,Diff_Amount,Voucher_Amount,Narration1,Diff_Type,Created_By,CGSTRate,CGSTAmount," +
                                                        "SGSTRate,SGSTAmount,IGSTRate,IGSTAmount,GstRateCode,TaxableAmount";
                                                                obj.values = "'" + "LV" + "','" + docno + "','" + Tender_Date + "','" + Voucher_By + "','" +
                                                                    string.Empty.Trim() + "','" + Company_Code + "','" + Year_Code + "','" +
                                                                    Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tender_No + "','"
                                                                    + Mill_Code + "','" + Grade + "','" + Quantal + "','" + Packing + "','" + Bags + "','" +
                                                                    Payment_To + "','" + Tender_From + "','" + Tender_DO + "','" + Broker + "','" + Mill_Rate + "','"
                                                                    + Purc_Rate + "','" + Diff_Amount + "','" + VOUCHERAMOUNT + "','" + Narration + myNarration + "','TD','"
                                                                    + user + "','" + CGSTRateForPS
                                                        + "','" + CGSTAmountForPS + "','" + SGSTRateForPS + "','" + SGSTAmountForPS + "','" + IGSTRateForPS + "','"
                                                        + IGSTAmountForPS + "','2','" + taxmillamt + "'";
                                                                obj.insertAccountMaster(ref str);

                                                                string updateVNo = "Update " + tblPrefix + "Tender SET Voucher_No=" + docno + " where Company_Code="
                                                                    + Company_Code + " and Year_Code=" + Year_Code + " and Tender_No=" + Tender_No + "";
                                                                clsDAL.SimpleQuery(updateVNo);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                            else
                            {
                                //show msg
                                pnlPopup.Style["display"] = "none";
                                ViewState["currentTable"] = null;
                                clsButtonNavigation.enableDisable("N");
                                this.makeEmptyForm("N");
                                ViewState["mode"] = "I";
                                this.showLastRecord();
                                lblMsg.Text = "This Tender No (" + txtTenderNo.Text + ") is deleted";
                            }
                        }

                        //Gledger effect for local voucher 
                        if (AUTO_VOUCHER == "YES")
                        {
                            if (drpResale.SelectedValue == "R" || drpResale.SelectedValue == "W")
                            {
                                docno = int.Parse(vouchernumber.Value.TrimStart());

                                qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + Tran_Type + "' and DOC_NO=" + docno + " and COMPANY_CODE="
                                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                                ds = clsDAL.SimpleQuery(qry);
                                if (Mill_Rate > PURCHASE_RATE)
                                {
                                    if (docno != 0)
                                    {
                                        //Gledger effect
                                        qry = "";
                                        //qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + Tran_Type + "' and DOC_NO=" + docno + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                                        //ds = clsDAL.SimpleQuery(qry);
                                        Int32 GID = 0;
                                        if (VOUCHERAMOUNT > 0)
                                        {
                                            GID = GID + 1;
                                            obj.flag = 1;
                                            obj.tableName = GLedgerTable;
                                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                            obj.values = "'" + Tran_Type + "','" + docno + "','" + Tender_Date + "','" + Voucher_By + "','" + myNarration + "DO-" + DOShortname + "','" + Math.Abs(VOUCHERAMOUNT) + "','" + Tender_No + "','" + Company_Code + "','" + Year_Code + "','" + GID + "','" + "D" + "','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tran_Type + "','" + docno + "'";
                                            obj.insertAccountMaster(ref str);
                                        }
                                        else
                                        {
                                            GID = GID + 1;
                                            obj.flag = 1;
                                            obj.tableName = GLedgerTable;
                                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                            obj.values = "'" + Tran_Type + "','" + docno + "','" + Tender_Date + "','" + Voucher_By + "','" + myNarration + "DO-" + DOShortname + "','" + Math.Abs(VOUCHERAMOUNT) + "','" + Company_Code + "','" + Year_Code + "','" + GID + "','" + "C" + "','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tran_Type + "','" + docno + "'";
                                            ds = obj.insertAccountMaster(ref str);
                                        }
                                        // difference amount effect
                                        if (DIFF > 0)
                                        {
                                            //------------Credit effect
                                            GID = GID + 1;
                                            obj.flag = 1;
                                            obj.tableName = GLedgerTable;
                                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                            obj.values = "'" + Tran_Type + "','" + int.Parse(lblVoucherNo.Text) + "','" + Tender_Date + "','" + int.Parse(Session["QUALITY_DIFF_AC"].ToString()) + "','" + myNarration + "DO-" + DOShortname + " vouc.by-" + voucherbyshortname + "','" + Math.Abs(VOUCHERAMOUNT) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "C" + "','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tran_Type + "','" + int.Parse(lblVoucherNo.Text) + "'";

                                            ds = obj.insertAccountMaster(ref str);
                                        }
                                        else
                                        {
                                            //------------Credit effect
                                            GID = GID + 1;
                                            obj.flag = 1;
                                            obj.tableName = GLedgerTable;
                                            obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                            obj.values = "'" + Tran_Type + "','" + int.Parse(lblVoucherNo.Text) + "','" + Tender_Date + "','" + int.Parse(Session["QUALITY_DIFF_AC"].ToString()) + "','" + myNarration + "DO-" + DOShortname + " vouc.by-" + voucherbyshortname + "','" + Math.Abs(VOUCHERAMOUNT) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "D" + "','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tran_Type + "','" + int.Parse(lblVoucherNo.Text) + "'";
                                            ds = obj.insertAccountMaster(ref str);
                                        }
                                    }
                                }
                                //else if (Mill_Rate == PURCHASE_RATE)
                                //{
                                //}
                                else
                                {
                                    if (drpResale.SelectedValue == "W" || drpResale.SelectedValue == "R")
                                    {
                                        if (Mill_Rate != PURCHASE_RATE)
                                        {

                                            string todaysDate = DateTime.Now.ToString("yyyy-MM-dd");
                                            DateTime dateOfToday = DateTime.ParseExact(todaysDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                                            DateTime dateToCompare = DateTime.ParseExact("2017-01-20", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                                            int result = DateTime.Compare(dateOfToday, dateToCompare);
                                            if (result > 0)
                                            {
                                                docno = int.Parse(vouchernumber.Value.TrimStart());
                                                if (docno != 0)
                                                {
                                                    //Gledger effect
                                                    qry = "";
                                                    qry = "delete from " + GLedgerTable + " where TRAN_TYPE='" + Tran_Type + "' and DOC_NO=" + docno + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                                                    ds = clsDAL.SimpleQuery(qry);
                                                    Int32 GID = 0;
                                                    if (VOUCHERAMOUNT > 0)
                                                    {
                                                        GID = GID + 1;
                                                        obj.flag = 1;
                                                        obj.tableName = GLedgerTable;
                                                        obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,TENDER_ID,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                                        obj.values = "'" + Tran_Type + "','" + docno + "','" + Tender_Date + "','" + Voucher_By + "','" + myNarration + "DO-" + DOShortname + "','" + Math.Abs(VOUCHERAMOUNT) + "','" + Tender_No + "','" + Company_Code + "','" + Year_Code + "','" + GID + "','" + "D" + "','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tran_Type + "','" + docno + "'";
                                                        obj.insertAccountMaster(ref str);
                                                    }
                                                    else
                                                    {
                                                        GID = GID + 1;
                                                        obj.flag = 1;
                                                        obj.tableName = GLedgerTable;
                                                        obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                                        obj.values = "'" + Tran_Type + "','" + docno + "','" + Tender_Date + "','" + Voucher_By + "','" + myNarration + "DO-" + DOShortname + "','" + Math.Abs(VOUCHERAMOUNT) + "','" + Company_Code + "','" + Year_Code + "','" + GID + "','" + "C" + "','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tran_Type + "','" + docno + "'";
                                                        ds = obj.insertAccountMaster(ref str);
                                                    }
                                                    // difference amount effect
                                                    if (DIFF > 0)
                                                    {
                                                        //------------Credit effect
                                                        GID = GID + 1;
                                                        obj.flag = 1;
                                                        obj.tableName = GLedgerTable;
                                                        obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                                        obj.values = "'" + Tran_Type + "','" + int.Parse(lblVoucherNo.Text) + "','" + Tender_Date + "','" + int.Parse(Session["QUALITY_DIFF_AC"].ToString()) + "','" + myNarration + "DO-" + DOShortname + " vouc.by-" + voucherbyshortname + "','" + Math.Abs(VOUCHERAMOUNT) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "C" + "','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tran_Type + "','" + int.Parse(lblVoucherNo.Text) + "'";

                                                        ds = obj.insertAccountMaster(ref str);
                                                    }
                                                    else
                                                    {
                                                        //------------Credit effect
                                                        GID = GID + 1;
                                                        obj.flag = 1;
                                                        obj.tableName = GLedgerTable;
                                                        obj.columnNm = "TRAN_TYPE,DOC_NO,DOC_DATE,AC_CODE,NARRATION,AMOUNT,COMPANY_CODE,YEAR_CODE,ORDER_CODE,DRCR,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO";
                                                        obj.values = "'" + Tran_Type + "','" + int.Parse(lblVoucherNo.Text) + "','" + Tender_Date + "','" + int.Parse(Session["QUALITY_DIFF_AC"].ToString()) + "','" + myNarration + "DO-" + DOShortname + " vouc.by-" + voucherbyshortname + "','" + Math.Abs(VOUCHERAMOUNT) + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','" + Convert.ToInt32(Session["year"].ToString()) + "','" + GID + "','" + "D" + "','" + 0 + "','" + Convert.ToInt32(Session["Branch_Code"].ToString()) + "','" + Tran_Type + "','" + int.Parse(lblVoucherNo.Text) + "'";
                                                        ds = obj.insertAccountMaster(ref str);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }//end if isvalidated
            return str;
        }
        catch
        {
            return "";
        }
    }

    private int MaxVoucher()
    {
        int docno = Convert.ToInt32(clsCommon.getString("Select COALESCE(MAX(Doc_No),0)+1 from " + tblPrefix + "Voucher where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Tran_Type='LV'"));
        return docno;
    }
    #endregion

    #region [btnEdit_Click]
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        int k = clsIsEdit.Tender_No;
        //if (!clsIsEdit.Tender.Any(a => a == Convert.ToInt32(txtTenderNo.Text)))
        //{
        ViewState["mode"] = null;
        ViewState["mode"] = "U";
        //Session["enableSave"] = 0;
        hdnf.Value = lblTender_Id.Text;
        // this.showLastRecord();
        //this.fetchRecord(txtTenderNo.Text);
        clsButtonNavigation.enableDisable("E");
        pnlgrdDetail.Enabled = true;
        this.makeEmptyForm("E");
        txtTenderNo.Enabled = false;
        txtEditDoc_No.Text = "";
        //}
        //DateTime Headliftingdate = DateTime.Parse(txtLiftingDate.Text);
        //string Dlift = clsCommon.getString("SELECT date_format(CURDATE(),'%d/%m/%Y') as d");
        //DateTime DetailLift = DateTime.Parse(Dlift);
        string dd = "";
        //DateTime Headliftingdate = DateTime.Parse(txtLiftingDate.Text);
        //string Dlift = clsCommon.getString("SELECT date_format(CURDATE(),'%d/%m/%Y') as d");
        //DateTime DetailLift = DateTime.Parse(Dlift);
        dd = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

        DateTime Headliftingdate = Convert.ToDateTime(dd);
        string Docdate = Headliftingdate.ToString("yyyy-MM-dd HH:mm:ss.fff");

        string Dlift = DateTime.Parse(clsCommon.getString("SELECT date_format(CURDATE(),'%d/%m/%Y') as d"), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        DateTime DetailLift = Convert.ToDateTime(Dlift);


        if (Headliftingdate > DetailLift)
        {
            txtDetailLiftingDate.Text = txtLiftingDate.Text;
        }
        else
        {
            txtDetailLiftingDate.Text = Dlift;
        }
    }

    //private void SaveChanges()
    //{
    //    Session["enableSave"] = 1;
    //}
    #endregion

    #region [btnDelete_Click]
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                string reftenderid = clsCommon.getString(" select purc_no from qrydohead where purc_no=" + txtTenderNo.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + "");
                string doid = clsCommon.getString(" select doc_no from qrydohead where purc_no=" + txtTenderNo.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + "");
                string refutr = clsCommon.getString(" select tenderid from qryutrheaddetail where tenderid=" + lblTender_Id.Text + "");
                string utrno = clsCommon.getString(" select doc_no from qryutrheaddetail where tenderid=" + lblTender_Id.Text + "");
                string concat = string.Empty;

                if (reftenderid != string.Empty && reftenderid != "0")
                {
                    concat = "DO: " + doid + "";
                }
                if (refutr != string.Empty && refutr != "0")
                {
                    concat = concat + "UTR: " + utrno + "";
                }
                if (reftenderid == "0" && refutr == "0")
                {
                    if (lblVoucherNo.Text != string.Empty && lblVoucherNo.Text != "0")
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
                    string refdo = clsCommon.getString("select tenderid from qryutrheaddetail where tenderid=" + lblTender_Id.Text + "");

                    Head_Delete = "delete from " + tblHead + " where tenderid='" + lblTender_Id.Text + "'";
                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = Head_Delete;
                    Maindt.Rows.Add(dr);

                    string Detail_Deleteqry = "delete from " + tblDetails + " where tenderid='" + lblTender_Id.Text + "'";
                    dr = null;
                    dr = Maindt.NewRow();
                    dr["Querys"] = Detail_Deleteqry;
                    Maindt.Rows.Add(dr);
                    flag = 3;

                    msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);
                    if (msg == "Delete")
                    {
                        Response.Redirect("../BussinessRelated/PgeTenderHeadUtility.aspx");
                    }

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('this Record Use In " + concat + "')", true);
                }


                #region
                //string query = string.Empty;
                //query = "select doc_no from " + tblPrefix + "deliveryorder where purc_no=" + txtTenderNo.Text + " and tran_type='DO' and company_code="
                //    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
                //string dono = clsCommon.getString(query);
                //string currentDoc_No = vouchernumber.Value;
                //string currentSuffix = string.Empty.Trim();
                //string delvoucher = "";
                //delvoucher = "";
                //if (dono != string.Empty)
                //{
                //    lblMsg.Text = "Cannot delete this entry It is in used!!";
                //    lblMsg.ForeColor = System.Drawing.Color.Red;
                //    setFocusControl(txtDate);
                //}
                //else
                //{
                //    lblMsg.Text = "";
                //    query = "delete from " + tblHead + " where Tender_No =" + txtTenderNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ";
                //    DataSet ds = clsDAL.SimpleQuery(query);
                //    query = "delete from " + tblDetails + " where Tender_No =" + txtTenderNo.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                //    ds = clsDAL.SimpleQuery(query);
                //    ds = new DataSet();
                //    DataTable dt = new DataTable();
                //    //Deleting Voucher Entry
                //    string deleteDebitNote = "delete from " + tblPrefix + "Voucher where Doc_No=" + currentDoc_No + " and Tran_Type='LV' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
                //    clsDAL.SimpleQuery(deleteDebitNote);
                //    delvoucher = "delete from " + GLedgerTable + " where TRAN_TYPE='" + Tran_Type + "' and DOC_NO=" + currentDoc_No + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and YEAR_CODE=" + Convert.ToInt32(Session["year"].ToString());
                //    ds = clsDAL.SimpleQuery(delvoucher);
                //    //string strrev = "";
                //    //using (clsUniversalInsertUpdateDelete obj = new clsUniversalInsertUpdateDelete())
                //    //{
                //    //    obj.flag = 3;
                //    //    obj.tableName = "" + tblPrefix + "Voucher";
                //    //    obj.columnNm = "  Tran_Type='" + Tran_Type + "' and Doc_No=" + currentDoc_No + " and Suffix='" + currentSuffix.Trim() + "'" +
                //    //        " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());

                //    //    obj.values = "none";
                //    //    ds = obj.insertAccountMaster(ref strrev);

                //    //}
                //    lblMsg.Text = "Successfully Deleted";
                //    query = "SELECT top 1 [Tender_No] from " + tblHead + "  where Tender_No>" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "  ORDER BY Tender_No asc  ";
                //    string Tender_No = clsCommon.getString(query);
                //    if (Tender_No != string.Empty)
                //    {
                //        fetchRecord(Tender_No);
                //        this.makeEmptyForm("S");
                //        clsButtonNavigation.enableDisable("S");
                //    }
                //    else
                //    {
                //        query = "SELECT top 1 [Tender_No] from " + tblHead + "  where Tender_No<" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY Tender_No DESC  ";
                //        Tender_No = clsCommon.getString(query);
                //        if (Tender_No != string.Empty)
                //        {
                //            fetchRecord(Tender_No);
                //            this.makeEmptyForm("S");
                //            clsButtonNavigation.enableDisable("S");
                //        }
                //        else
                //        {
                //            this.makeEmptyForm("N");
                //            //new code
                //            clsButtonNavigation.enableDisable("N");         //No record exist  Last record deleted.
                //            btnEdit.Enabled = false;
                //            btnDelete.Enabled = false;
                //        }
                //    }
                //    //clsButtonNavigation.enableDisable("S");
                //   // this.enableDisableNavigateButtons();
                //}
                #endregion
            }
            //else
            //{
            //    hdnf.Value = lblTender_Id.Text;
            //    showLastRecord();
            //}
        }
        catch
        {

        }
    }
    #endregion

    #region [btnCancel_Click]
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        hdnf.Value = Request.QueryString["tenderid"];
        if (hdnf.Value == "0" || hdnf.Value == "")
        {
            hdnf.Value = clsCommon.getString("select max(tenderid) from nt_1_tender where Company_Code=" + Session["Company_Code"].ToString() + " and " +
                " Year_Code=" + Session["year"].ToString() + "");
        }
        clsButtonNavigation.enableDisable("S");
        this.makeEmptyForm("S");
        showLastRecord();

    }
    #endregion

    #region [fetchRecord]
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
                        //hdnf.Value = txtValue;
                        //txtTenderNo.Text = txtValue;
                        txtDate.Text = dt.Rows[0]["Tender_DateConverted"].ToString();
                        txtLiftingDate.Text = dt.Rows[0]["Lifting_DateConverted"].ToString();
                        lblTender_Id.Text = hdnf.Value;
                        //txtDate.Text = dt.Rows[0]["Tender_Date"].ToString();
                        //txtLiftingDate.Text = dt.Rows[0]["Lifting_Date"].ToString();
                        //Response.Write("MillCode" + dt.Rows[0]["Mill_Code"].ToString());
                        //hdnf.Value = dt.Rows[0]["Tender_No"].ToString();
                        txtCashDiff.Text = dt.Rows[0]["CashDiff"].ToString();
                        txtTenderNo.Text = dt.Rows[0]["Tender_No"].ToString();
                        txtMillCode.Text = dt.Rows[0]["Mill_Code"].ToString();
                        txtSeason.Text = dt.Rows[0]["season"].ToString();
                        txtitem_code.Text = dt.Rows[0]["itemcode"].ToString();
                        lblitemname.Text = dt.Rows[0]["itemname"].ToString();
                        txtGrade.Text = dt.Rows[0]["Grade"].ToString();
                        txtQuantal.Text = dt.Rows[0]["Quantal"].ToString();//Convert.ToString(Math.Abs(Convert.ToDouble(dt.Rows[0]["Quantal"].ToString())));
                        txtPacking.Text = dt.Rows[0]["Packing"].ToString();
                        txtBags.Text = dt.Rows[0]["Bags"].ToString();
                        txtMillRate.Text = dt.Rows[0]["Mill_Rate"].ToString();
                        // txtPurcRate.Text = dt.Rows[0][""].ToString();
                        txtTenderFrom.Text = dt.Rows[0]["Tender_From"].ToString();
                        txtPaymentTo.Text = dt.Rows[0]["Payment_To"].ToString();
                        txtDO.Text = dt.Rows[0]["Tender_DO"].ToString();
                        txtVoucherBy.Text = dt.Rows[0]["Voucher_By"].ToString();
                        txtBroker.Text = dt.Rows[0]["Broker"].ToString();
                        txtExciseRate.Text = dt.Rows[0]["Excise_Rate"].ToString();
                        txtSellNoteNo.Text = dt.Rows[0]["Sell_Note_No"].ToString();
                        txtNarration.Text = dt.Rows[0]["Narration"].ToString();
                        txtGstrateCode.Text = dt.Rows[0]["gstratecode"].ToString();
                        lblgstrateCode.Text = dt.Rows[0]["gstratename"].ToString();
                        lblMill_Id.Text = dt.Rows[0]["mc"].ToString();
                        lblPayment_Id.Text = dt.Rows[0]["pt"].ToString();
                        lblTenderForm_Id.Text = dt.Rows[0]["tf"].ToString();
                        lblTenderDo_Id.Text = dt.Rows[0]["td"].ToString();
                        lblVoucherBy_Id.Text = dt.Rows[0]["vb"].ToString();
                        lblBroker_Id.Text = dt.Rows[0]["bk"].ToString();
                        lblTender_Id.Text = dt.Rows[0]["tenderid"].ToString();

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
                        //lblCreatedBy.Text = dt.Rows[0]["CreatedBy"].ToString();
                        // lblModifiedBy.Text = dt.Rows[0]["ModifiedBy"].ToString();
                        //set Label Texts
                        //lblVoucherNo.Text = dt.Rows[0]["Doc_No"].ToString();
                        Int32 voucherno = dt.Rows[0]["Voucher_No"].ToString().Trim() != string.Empty ? Convert.ToInt32(dt.Rows[0]["Voucher_No"].ToString()) : 0;
                        vouchernumber.Value = voucherno.ToString();
                        lblMillName.Text = dt.Rows[0]["millname"].ToString();
                        lblPaymentTo.Text = dt.Rows[0]["paymenttoname"].ToString();
                        lblTenderFrom.Text = dt.Rows[0]["tenderfromname"].ToString();
                        lblVoucherBy.Text = dt.Rows[0]["voucherbyname"].ToString();
                        lblDO.Text = dt.Rows[0]["tenderdoname"].ToString();
                        lblBroker.Text = dt.Rows[0]["brokername"].ToString();

                        txtPurcRate.Text = dt.Rows[0]["Purc_Rate"].ToString();
                        drpResale.SelectedValue = dt.Rows[0]["type"].ToString();
                        lblVoucherNo.Text = vouchernumber.Value;
                        txtBrokrage.Text = dt.Rows[0]["Brokrage"].ToString();
                        recordExist = true;
                        lblMsg.Text = "";

                        #region Tender Details

                        // qry = "select  ID,Buyer as Party,buyerbrokerfullname  as [Name of Party],Buyer_Party as Broker,salepartyfullname as [Name of Broker],Buyer_Quantal as [Quantal],Sale_Rate as [Sale Rate],Commission_Rate as [Commission],Convert(varchar(10),DetailSaudaDate,103) as Sauda_Date,Convert(varchar(10),DetailLiftingDate,103) as Lifting_Date,saudanarration as [Sauda Narration],Delivery_Type from " + qryDetail + " where tenderid='" + hdnf.Value + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and IsActive='True' and TDYearCode='" + Convert.ToInt32(Session["year"].ToString()) + "' order by ID";
                        qry = "select  ID,Buyer as Party,buyername as Name_of_Party,Buyer_Party as Broker," +
" buyerpartyname as Name_of_Broker,Buyer_Quantal as Quantal,Sale_Rate as Sale_Rate," +
" Commission_Rate as Commission,Sauda_DateConverted as Sauda_Date,payment_dateConverted as Lifting_Date,detail_narration as Sauda_Narration," +
" Delivery_Type,sub_broker as sub_broker,subbrokername as subBrokername ,tenderdetailid from qrytenderheaddetail where tenderid='" + hdnf.Value + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' order by ID";
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
                                        if (dt.Rows[i]["Delivery_Type"].ToString() == "C")
                                        {
                                            dt.Rows[i]["Delivery_Type"] = "Commission";
                                        }
                                        if (dt.Rows[i]["Delivery_Type"].ToString() == "N")
                                        {
                                            dt.Rows[i]["Delivery_Type"] = "Naka Delivery";
                                        }
                                        if (dt.Rows[i]["Delivery_Type"].ToString() == "D")
                                        {
                                            dt.Rows[i]["Delivery_Type"] = "DO";
                                        }
                                    }
                                    dt.Columns.Add(new DataColumn("rowAction", typeof(string)));
                                    dt.Columns.Add(new DataColumn("SrNo", typeof(int)));

                                    dt.Rows[0]["SrNo"] = 1;                           //self row
                                    dt.Rows[0]["rowAction"] = "U";                     //self row

                                    for (int i = 1; i < dt.Rows.Count; i++)
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

                        this.calculateDiff();
                        this.calculateAmount();
                        GridViewRow gr = (GridViewRow)grdDetail.Rows[0];
                        gr.Enabled = false;
                        lblBalanceSelf.Text = Server.HtmlDecode(gr.Cells[7].Text);
                        pnlgrdDetail.Enabled = false;
                    }
                }
            }
            //hdnf.Value = txtTenderNo.Text;
            return recordExist;
        }
        catch
        {
            // throw;
            return false;
        }
    }
    #endregion

    #region [First]
    protected void btnFirst_Click(object sender, EventArgs e)
    {
        #region [code]
        try
        {
            string query = "";
            query = "select Tender_No from " + tblHead + " where Tender_No=(select MIN(Tender_No) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ") and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
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
                        txtTenderNo.Text = dt.Rows[0][0].ToString();
                        ViewState["mode"] = "U";
                        clsButtonNavigation.enableDisable("N");
                        bool recordExist = this.fetchRecord(dt.Rows[0][0].ToString());
                        if (recordExist == true)
                        {
                            btnEdit.Enabled = true;
                            btnEdit.Focus();
                        }

                        this.enableDisableNavigateButtons();
                        this.makeEmptyForm("S");

                    }
                }
            }


        }
        catch
        {

        }
        #endregion
    }
    #endregion

    #region [Previous]
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtTenderNo.Text != string.Empty)
            {
                string query = "";
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                query = "SELECT top 1 [Tender_No] from " + tblHead + " where Tender_No<" + Convert.ToInt32(hdnf.Value) + " and Company_Code="
                    + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString())
                    + " ORDER BY Tender_No DESC  ";
                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            ViewState["mode"] = "U";
                            clsButtonNavigation.enableDisable("N");
                            bool recordExist = this.fetchRecord(dt.Rows[0]["Tender_No"].ToString());
                            if (recordExist == true)
                            {
                                btnEdit.Enabled = true;
                                btnEdit.Focus();
                            }
                            this.enableDisableNavigateButtons();
                            this.makeEmptyForm("S");
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

    #region [Next]
    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtTenderNo.Text != string.Empty)
            {
                string query = "";
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                query = "SELECT top 1 [Tender_No] from " + tblHead + " where Tender_No>" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY Tender_No asc  ";

                ds = clsDAL.SimpleQuery(query);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            ViewState["mode"] = "U";
                            clsButtonNavigation.enableDisable("N");
                            bool recordExist = this.fetchRecord(dt.Rows[0]["Tender_No"].ToString());
                            if (recordExist == true)
                            {
                                btnEdit.Enabled = true;
                                btnEdit.Focus();
                            }
                            this.enableDisableNavigateButtons();
                            this.makeEmptyForm("S");
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

    #region [Last]
    protected void btnLast_Click(object sender, EventArgs e)
    {
        try
        {
            string query = "";
            query = "select Tender_No from " + tblHead + " where Tender_No=(select MAX(Tender_No) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + ")  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ";
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
                        txtTenderNo.Text = dt.Rows[0][0].ToString();
                        ViewState["mode"] = "U";
                        clsButtonNavigation.enableDisable("N");
                        bool recordExist = this.fetchRecord(dt.Rows[0][0].ToString());
                        if (recordExist == true)
                        {
                            btnEdit.Enabled = true;
                            btnEdit.Focus();
                        }

                        this.enableDisableNavigateButtons();
                        this.makeEmptyForm("S");

                    }
                }
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

        ////if (ViewState["mode"].ToString() == "U")
        ////{

        //int RecordCount = 0;

        //string query = "";
        //query = "select count(*) from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";
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
        //    //  btnLast.Focus();
        //}

        //if (txtTenderNo.Text != string.Empty)
        //{
        //    if (hdnf.Value != string.Empty)
        //    {
        //        #region check for next or previous record exist or not
        //        ds = new DataSet();
        //        dt = new DataTable();

        //        query = "SELECT top 1 [Tender_No] from " + tblHead + " where Tender_No>" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY Tender_No asc  ";
        //        ds = clsDAL.SimpleQuery(query);
        //        if (ds != null)
        //        {
        //            if (ds.Tables.Count > 0)
        //            {
        //                dt = ds.Tables[0];
        //                if (dt.Rows.Count > 0)
        //                {
        //                    //next record exist
        //                    btnLast.Enabled = true;
        //                    btnNext.Enabled = true;
        //                }
        //                else
        //                {
        //                    //next record does not exist
        //                    btnLast.Enabled = false;
        //                    btnNext.Enabled = false;
        //                }
        //            }
        //        }

        //        ds = new DataSet();
        //        dt = new DataTable();

        //        query = "SELECT top 1 [Tender_No] from " + tblHead + " where Tender_No<" + Convert.ToInt32(hdnf.Value) + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ORDER BY Tender_No asc  ";
        //        ds = clsDAL.SimpleQuery(query);
        //        if (ds != null)
        //        {
        //            if (ds.Tables.Count > 0)
        //            {
        //                dt = ds.Tables[0];
        //                if (dt.Rows.Count > 0)
        //                {
        //                    //previous record exist
        //                    btnFirst.Enabled = true;
        //                    btnPrevious.Enabled = true;
        //                }
        //                else
        //                {
        //                    btnFirst.Enabled = false;
        //                    btnPrevious.Enabled = false;
        //                }
        //            }
        //        }
        //        #endregion
        //    }
        //}

        //// }

        #endregion
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
                        if (grdDetail.Rows[rowindex].Cells[rowAction].Text != "D" && grdDetail.Rows[rowindex].Cells[rowAction].Text != "R")
                        {
                            // pnlPopupTenderDetails.Style["display"] = "block";
                            this.showRecord(grdDetail.Rows[rowindex]);
                            btnADDBuyerDetails.Text = "Update";
                        }
                        break;

                    case "DeleteRecord":
                        string action = "";

                        DataTable dt1 = (DataTable)ViewState["currentTable"];
                        int refid = Convert.ToInt32(dt1.Rows[rowindex]["tenderdetailid"].ToString());

                        string reftenderid = clsCommon.getString(" select tenderdetailid from qrydohead where tenderdetailid=" + refid + "");
                        string doid = clsCommon.getString(" select doc_no from qrydohead where tenderdetailid=" + refid + "");

                        //string refutr = clsCommon.getString(" select tenderid from qryutrheaddetail where lot_no=" + txtTenderNo.Text + " and " +
                        //    " Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + "");

                        //string utrno = clsCommon.getString(" select doc_no from qryutrheaddetail where lot_no=" + txtTenderNo.Text + " and " +
                        //    " Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + "");
                        string concat = string.Empty;

                        if (reftenderid != string.Empty && reftenderid != "0")
                        {
                            concat = "DO: " + doid + "";
                        }
                        //if (refutr != string.Empty && refutr != "0")
                        //{
                        //    concat = concat + "UTR: " + utrno + "";
                        //}
                        if (reftenderid == "0")
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
                            this.deleteTenderDetailsRecord(grdDetail.Rows[rowindex], action);

                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('This Record Use In " + concat + "')", true);
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

    #region [deleteTenderDetailsRecord]
    private void deleteTenderDetailsRecord(GridViewRow gridViewRow, string action)
    {
        try
        {
            int rowIndex = gridViewRow.RowIndex;

            if (ViewState["currentTable"] != null)
            {
                DataTable dt = (DataTable)ViewState["currentTable"];
                int ID = Convert.ToInt32(dt.Rows[rowIndex]["tenderdetailid"].ToString());

                string IDExisting = clsCommon.getString("select ID from " + tblDetails + " where tenderdetailid=" + ID + " ");
                if (IDExisting != string.Empty && IDExisting != "0")
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[rowAction].Text = "D";

                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[rowAction].Text = "N";

                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "N";
                    }
                    //  dt.Rows[rowIndex]["rowAction"] = "D";  //isactive false
                }
                else
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;

                        grdDetail.Rows[rowIndex].Cells[rowAction].Text = "R";

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

                    // dt.Rows[rowIndex]["rowAction"] = "N";   //Do nothing
                }
                ViewState["currentTable"] = dt;
                this.calculateBalanceSelf();
                //ViewState["currentTable"] = dt;
            }
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i]["RowNumber"] = i + 1;
            //}
        }
        catch
        {

        }
    }
    #endregion

    #region [showRecord]
    private void showRecord(GridViewRow gridViewRow)
    {
        lblno.Text = Server.HtmlDecode(gridViewRow.Cells[SrNo].Text);
        lblID.Text = Server.HtmlDecode(gridViewRow.Cells[2].Text);
        lbltenderdetailid.Text = Server.HtmlDecode(gridViewRow.Cells[tenderdetailid].Text);
        txtBuyer.Text = Server.HtmlDecode(gridViewRow.Cells[Party].Text);
        lblBuyerName.Text = Server.HtmlDecode(gridViewRow.Cells[Name_of_Party].Text);
        string deliveryType = Server.HtmlDecode(gridViewRow.Cells[delivaryType].Text);
        string type = "";
        if (deliveryType == "Commission")
        {
            type = "C";
        }
        else if (deliveryType == "DO")
        {
            type = "D";
        }
        else
        {
            type = "N";
        }
        txtDetailSaudaDate.Text = Server.HtmlDecode(gridViewRow.Cells[saudaDate].Text);
        txtDetailLiftingDate.Text = Server.HtmlDecode(gridViewRow.Cells[liftingDate].Text);
        drpDeliveryType.SelectedValue = type;
        txtBuyerParty.Text = Server.HtmlDecode(gridViewRow.Cells[Broker_No].Text);
        lblBuyerPartyName.Text = Server.HtmlDecode(gridViewRow.Cells[Name_Of_Broker].Text);
        double buyerqntl = Convert.ToDouble(Server.HtmlDecode(gridViewRow.Cells[quantal].Text));
        txtBuyerQuantal.Text = Convert.ToString(Math.Abs(buyerqntl));
        double salerate = Convert.ToDouble(Server.HtmlDecode(gridViewRow.Cells[saleRate].Text));
        txtBuyerSaleRate.Text = Convert.ToString(Math.Abs(salerate));
        double buyercmmrate = Convert.ToDouble(Server.HtmlDecode(gridViewRow.Cells[commissions].Text));
        txtBuyerCommission.Text = Convert.ToString(Math.Abs(buyercmmrate));
        txtBuyerNarration.Text = Server.HtmlDecode(gridViewRow.Cells[Sauda_Narration].Text);
        txtsubBroker.Text = Server.HtmlDecode(gridViewRow.Cells[sub_broker].Text);
        lblsubBroker.Text = Server.HtmlDecode(gridViewRow.Cells[subBrokername].Text);
        if (ViewState["currentTable"] != null)
        {

        }
        setFocusControl(txtBuyer);
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

    #region[btnMillCode_Click]
    protected void btnMillCode_Click(object sender, EventArgs e)
    {
        try
        {
            txtSearchText.Text = string.Empty;
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "MM";
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
        try
        {
            hdnfClosePopup.Value = "Close";
            string millName = string.Empty;
            searchString = txtMillCode.Text;
            if (txtMillCode.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtMillCode.Text);
                if (a == false)
                {
                    btnMillCode_Click(this, new EventArgs());
                }
                else
                {
                    millName = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_Code='" + txtMillCode.Text
                        + "' and Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and Ac_type='M'");

                    if (millName != string.Empty && millName != "0")
                    {
                        lblMillName.Text = millName;
                        setFocusControl(txtSeason);
                        txtPaymentTo.Text = txtMillCode.Text;
                        lblPaymentTo.Text = millName;
                    }
                    else
                    {
                        txtMillCode.Text = string.Empty;
                        lblMillName.Text = string.Empty;
                        setFocusControl(txtMillCode);
                    }
                }
            }
            else
            {
                txtMillCode.Text = string.Empty;
                lblMillName.Text = millName;
                setFocusControl(txtMillCode);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [btnGrade_Click]
    protected void btnGrade_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "GR";
            btnSearch_Click(sender, e);
            setFocusControl(txtSearchText);

        }
        catch
        {

        }
    }
    #endregion

    #region [txtGrade_TextChanged]
    protected void txtGrade_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGrade.Text;
        if (txtGrade.Text != string.Empty)
        {
            bool a = true;
            if (txtGrade.Text.Length < 8)
            {
                a = clsCommon.isStringIsNumeric(txtGrade.Text);
            }
            if (a == false)
            {
                // btnGrade_Click(this, new EventArgs());
            }
            else
            {
                pnlPopup.Style["display"] = "none";
                setFocusControl(txtQuantal);
            }
        }
    }
    #endregion

    #region[btnPaymentTo_Click]
    protected void btnPaymentTo_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "PT";
            btnSearch_Click(sender, e);
        }
        catch
        {

        }
    }
    #endregion

    #region [txtPaymentTo_TextChanged]
    protected void txtPaymentTo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtPaymentTo.Text;
            string paymentToname = string.Empty;
            if (txtPaymentTo.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtPaymentTo.Text);
                if (a == false)
                {
                    btnPaymentTo_Click(this, new EventArgs());
                }
                else
                {
                    paymentToname = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_Code='" + txtPaymentTo.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (paymentToname != string.Empty && paymentToname != "0")
                    {
                        if (paymentToname.Length > 15)
                        {
                            paymentToname.Substring(0, 15);
                        }
                        else if (paymentToname.Length > 10)
                        {
                            paymentToname.Substring(0, 10);
                        }
                        lblPaymentTo.Text = paymentToname;
                        if (drpResale.SelectedValue == "M")
                        {
                            setFocusControl(txtBroker);
                        }
                        else
                        {
                            setFocusControl(txtTenderFrom);
                        }
                    }
                    else
                    {
                        txtPaymentTo.Text = string.Empty;
                        lblPaymentTo.Text = string.Empty;
                        setFocusControl(txtPaymentTo);
                    }
                }
            }
            else
            {
                txtPaymentTo.Text = string.Empty;
                lblPaymentTo.Text = paymentToname;
                setFocusControl(txtPaymentTo);
            }

        }
        catch
        {
        }
    }
    #endregion

    #region[btnTenderFrom_Click]
    protected void btnTenderFrom_Click(object sender, EventArgs e)
    {
        try
        {
            hdnfClosePopup.Value = "TF";
            pnlPopup.Style["display"] = "block";
            btnSearch_Click(sender, e);
        }
        catch
        {

        }
    }
    #endregion

    #region[txtTenderFrom_TextChanged]
    protected void txtTenderFrom_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtTenderFrom.Text;
            string tenderFromName = string.Empty;
            if (txtTenderFrom.Text != string.Empty)
            {

                bool a = clsCommon.isStringIsNumeric(txtTenderFrom.Text);
                if (a == false)
                {
                    btnTenderFrom_Click(this, new EventArgs());
                }
                else
                {
                    tenderFromName = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_Code='" + txtTenderFrom.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (tenderFromName != string.Empty && tenderFromName != "0")
                    {
                        if (tenderFromName.Length > 15)
                        {
                            tenderFromName.Substring(0, 15);
                        }
                        else if (tenderFromName.Length > 10)
                        {
                            tenderFromName.Substring(0, 10);
                        }
                        lblTenderFrom.Text = tenderFromName;
                        setFocusControl(txtDO);
                    }
                    else
                    {
                        txtTenderFrom.Text = string.Empty;
                        lblTenderFrom.Text = string.Empty;
                        setFocusControl(txtDO);
                    }
                }
            }
            else
            {
                txtTenderFrom.Text = string.Empty;
                lblTenderFrom.Text = tenderFromName;
                setFocusControl(txtDO);
            }

        }
        catch
        {
        }
    }
    #endregion

    #region[btnTenderDO_Click]
    protected void btnTenderDO_Click(object sender, EventArgs e)
    {
        try
        {
            hdnfClosePopup.Value = "DO";
            pnlPopup.Style["display"] = "block";
            btnSearch_Click(sender, e);
        }
        catch
        {

        }
    }
    #endregion

    #region[txtDO_TextChanged]
    protected void txtDO_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtDO.Text;
            string doName = string.Empty;
            if (txtDO.Text != string.Empty)
            {

                bool a = clsCommon.isStringIsNumeric(txtDO.Text);
                if (a == false)
                {
                    btnTenderDO_Click(this, new EventArgs());
                }
                else
                {
                    doName = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_Code='" + txtDO.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (doName != string.Empty && doName != "0")
                    {
                        if (doName.Length > 15)
                        {
                            doName.Substring(0, 15);
                        }
                        else if (doName.Length > 10)
                        {
                            doName.Substring(0, 10);
                        }
                        lblDO.Text = doName;

                        txtTenderFrom.Text = txtDO.Text;
                        lblTenderFrom.Text = doName;
                        txtVoucherBy.Text = txtDO.Text;
                        lblVoucherBy.Text = doName;

                        setFocusControl(txtVoucherBy);
                    }
                    else
                    {
                        txtDO.Text = string.Empty;
                        lblDO.Text = string.Empty;
                        setFocusControl(txtDO);
                    }
                }
            }
            else
            {
                txtDO.Text = string.Empty;
                lblDO.Text = doName;
                setFocusControl(txtDO);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region[btnVoucherBy_Click]
    protected void btnVoucherBy_Click(object sender, EventArgs e)
    {
        try
        {

            hdnfClosePopup.Value = "VB";
            pnlPopup.Style["display"] = "block";
            btnSearch_Click(sender, e);

        }
        catch
        {

        }
    }
    #endregion

    #region[txtVoucherBy_TextChanged]
    protected void txtVoucherBy_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtVoucherBy.Text;
            string voucherByName = string.Empty;
            if (txtVoucherBy.Text != string.Empty)
            {

                bool a = clsCommon.isStringIsNumeric(txtVoucherBy.Text);
                if (a == false)
                {
                    btnVoucherBy_Click(this, new EventArgs());
                }
                else
                {
                    voucherByName = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_Code='" + txtVoucherBy.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    if (voucherByName != string.Empty && voucherByName != "0")
                    {
                        if (voucherByName.Length > 15)
                        {
                            voucherByName.Substring(0, 15);
                        }
                        else if (voucherByName.Length > 10)
                        {
                            voucherByName.Substring(0, 10);
                        }
                        lblVoucherBy.Text = voucherByName;
                        setFocusControl(txtBroker);
                    }
                    else
                    {
                        txtVoucherBy.Text = string.Empty;
                        lblVoucherBy.Text = string.Empty;
                        setFocusControl(txtVoucherBy);
                    }
                }
            }
            else
            {
                txtVoucherBy.Text = string.Empty;
                lblVoucherBy.Text = voucherByName;
                setFocusControl(txtVoucherBy);
            }

        }
        catch
        {
        }
    }
    #endregion

    #region[btnBroker_Click]
    protected void btnBroker_Click(object sender, EventArgs e)
    {
        try
        {

            hdnfClosePopup.Value = "BR";
            pnlPopup.Style["display"] = "block";
            btnSearch_Click(sender, e);
        }
        catch
        {

        }
    }
    #endregion
    protected void btnsubBrker_Click(object sender, EventArgs e)
    {
        try
        {

            hdnfClosePopup.Value = "SubBrker";
            pnlPopup.Style["display"] = "block";
            btnSearch_Click(sender, e);
        }
        catch
        {

        }
    }
    protected void txtBrokrage_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtExciseRate);

    }
    #region[txtBroker_TextChanged]
    protected void txtBroker_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtBroker.Text;
            string brokerName = string.Empty;
            if (txtBroker.Text != string.Empty)
            {

                bool a = clsCommon.isStringIsNumeric(txtBroker.Text);
                if (a == false)
                {
                    btnBroker_Click(this, new EventArgs());
                }
                else
                {
                    qry = "select Ac_Name_E,accoid from qrymstaccountmaster where Ac_Code='" + txtBroker.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";

                    brokerName = clsCommon.getString(qry);
                    if (brokerName != string.Empty && brokerName != "0")
                    {
                        if (brokerName.Length > 15)
                        {
                            brokerName.Substring(0, 15);
                        }
                        else if (brokerName.Length > 10)
                        {
                            brokerName.Substring(0, 10);
                        }
                        lblBroker.Text = brokerName;
                        setFocusControl(txtBrokrage);
                    }
                    else
                    {
                        txtBroker.Text = "2";
                        setFocusControl(txtBrokrage);
                    }

                }
            }
            else
            {
                txtBroker.Text = string.Empty;
                lblBroker.Text = brokerName;
                setFocusControl(txtBroker);
            }
        }
        catch
        {
        }
    }
    #endregion

    protected void txtsubBroker_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtsubBroker.Text;
            string brokerName = string.Empty;
            if (txtsubBroker.Text == "0")
            {
                txtsubBroker.Text = "2";
            }
            if (txtsubBroker.Text != string.Empty)
            {

                bool a = clsCommon.isStringIsNumeric(txtsubBroker.Text);
                if (a == false)
                {
                    btnsubBrker_Click(this, new EventArgs());
                }
                else
                {
                    qry = "select Ac_Name_E,accoid from qrymstaccountmaster where Ac_Code='" + txtsubBroker.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                brokerName = dt.Rows[0]["Ac_Name_E"].ToString();
                                if (brokerName.Length > 15)
                                {
                                    brokerName.Substring(0, 15);
                                }
                                else if (brokerName.Length > 10)
                                {
                                    brokerName.Substring(0, 10);
                                }
                                lblsubBroker.Text = brokerName;
                                lblsubId.Text = dt.Rows[0]["accoid"].ToString();
                                //if (grdDetail.Rows.Count > 0)
                                //{
                                //    grdDetail.Rows[0].Cells[5].Text = Convert.ToString(txtBroker.Text);
                                //    grdDetail.Rows[0].Cells[6].Text = Convert.ToString(lblBroker.Text);
                                //}
                                setFocusControl(txtBuyerQuantal);
                            }
                            else
                            {
                                txtBroker.Text = "2";

                                setFocusControl(txtBuyerQuantal);
                            }
                        }
                    }

                    else
                    {
                        txtBroker.Text = string.Empty;
                        lblBroker.Text = string.Empty;
                        setFocusControl(txtBroker);
                    }
                }
            }
            else
            {
                txtBroker.Text = string.Empty;
                lblBroker.Text = brokerName;
                setFocusControl(txtBroker);
            }
        }
        catch
        {
        }
    }
    protected void txtGstrateCode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtGstrateCode.Text;
            string GstrateCode = string.Empty;
            if (txtGstrateCode.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtGstrateCode.Text);
                if (a == false)
                {
                    btnGstrateCode_Click(this, new EventArgs());

                }
                else
                {
                    GstrateCode = clsCommon.getString("select Rate from nt_1_gstratemaster where Doc_no='" + txtGstrateCode.Text + "' ");
                    if (GstrateCode != string.Empty && GstrateCode != "0")
                    {
                        if (GstrateCode.Length > 15)
                        {
                            GstrateCode.Substring(0, 15);
                        }
                        else if (GstrateCode.Length > 10)
                        {
                            GstrateCode.Substring(0, 10);
                        }
                        lblgstrateCode.Text = GstrateCode;
                        calculateAmount();
                        setFocusControl(txtExciseRate);

                    }
                    else
                    {
                        txtGstrateCode.Text = string.Empty;
                        lblgstrateCode.Text = string.Empty;
                        setFocusControl(txtGstrateCode);
                    }
                }
            }
            else
            {
                txtGstrateCode.Text = string.Empty;
                lblPaymentTo.Text = GstrateCode;
                setFocusControl(txtGstrateCode);
            }

        }
        catch
        {
        }
    }
    protected void btnGstrateCode_Click(object sender, EventArgs e)
    {
        //  pnlPopupTenderDetails.Style["display"] = "block";
        pnlPopup.Style["display"] = "block";
        hdnfClosePopup.Value = "GC";
        btnSearch_Click(sender, e);
    }

    #region[btnBuyer_Click]
    protected void btnBuyer_Click(object sender, EventArgs e)
    {
        //  pnlPopupTenderDetails.Style["display"] = "block";
        pnlPopup.Style["display"] = "block";
        hdnfClosePopup.Value = "BU";
        btnSearch_Click(sender, e);
    }
    #endregion

    #region[txtBuyer_TextChanged]
    protected void txtBuyer_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtBuyer.Text;
            string buyerName = string.Empty;
            if (txtBuyer.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtBuyer.Text);
                if (a == false)
                {
                    btnBuyer_Click(this, new EventArgs());
                }
                else
                {
                    qry = "select Ac_Name_E,accoid from qrymstaccountmaster where Ac_Code=" + txtBuyer.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                buyerName = dt.Rows[0]["Ac_Name_E"].ToString();
                                lblBuyerName.Text = buyerName;
                                lblbuyer_id.Text = dt.Rows[0]["accoid"].ToString();
                                setFocusControl(drpDeliveryType);

                                AddCommission();
                            }
                        }
                    }

                    else
                    {
                        txtBuyer.Text = string.Empty;
                        lblBuyerName.Text = string.Empty;
                        txtBuyer.Focus();
                        setFocusControl(txtBuyer);
                    }
                }
            }
            else
            {
                txtBuyer.Text = string.Empty;
                lblBuyerName.Text = buyerName;
                setFocusControl(txtBuyer);
            }

        }
        catch
        {
        }

    }

    private void AddCommission()
    {
        txtBuyerCommission.Text = clsCommon.getString("select ifNULL(Commission,0) from " + AccountMasterTable
            + " where Ac_Code=" + txtBuyer.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
    }
    #endregion

    #region[txtBuyerParty_TextChanged]
    protected void txtBuyerParty_TextChanged(object sender, EventArgs e)
    {
        try
        {
            searchString = txtBuyerParty.Text;
            string buyerPartyName = string.Empty;
            if (txtBuyerParty.Text != string.Empty)
            {

                bool a = clsCommon.isStringIsNumeric(txtBuyerParty.Text);
                if (a == false)
                {
                    btnBuyerParty_Click(this, new EventArgs());
                }
                else
                {
                    qry = "select Ac_Name_E from qrymstaccountmaster where Ac_Code=" + txtBuyerParty.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());

                    buyerPartyName = clsCommon.getString(qry);
                    if (buyerPartyName != string.Empty && buyerPartyName != "0")
                    {
                        lblBuyerPartyName.Text = buyerPartyName;
                        setFocusControl(txtsubBroker);
                    }
                    else
                    {
                        txtBuyerParty.Text = "2";
                        lblBuyerPartyName.Text = "self";
                        setFocusControl(txtsubBroker);
                    }
                }
            }
        }
        catch
        {
        }
    }
    #endregion

    #region[btnBuyerParty_Click]
    protected void btnBuyerParty_Click(object sender, EventArgs e)
    {
        // pnlPopupTenderDetails.Style["display"] = "None";
        pnlPopup.Style["display"] = "block";
        hdnfClosePopup.Value = "BP";
        btnSearch_Click(sender, e);
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

    #region [txtQuantal_TextChanged]
    protected void txtQuantal_TextChanged(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "none";
            if (txtQuantal.Text != string.Empty && txtPacking.Text != string.Empty && txtQuantal.Text != "0" && txtPacking.Text != "0")
            {
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtQuantal.Text) * (100 / float.Parse(txtPacking.Text))));
                txtBags.Text = bags.ToString();
            }
            else if ((txtPacking.Text == string.Empty || txtPacking.Text == "0") && txtQuantal.Text != string.Empty && txtQuantal.Text != "0")
            {
                txtPacking.Text = "50";
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtQuantal.Text) * (100 / float.Parse(txtPacking.Text))));
                txtBags.Text = bags.ToString();
                setFocusControl(txtPacking);
            }
            else
            {
                txtQuantal.Text = string.Empty;
                setFocusControl(txtQuantal);
                txtBags.Text = "0";
            }



            if (grdDetail.Rows.Count == 0)
            {
                this.btnADDBuyerDetails_Click(sender, e);
            }
            else
            {
                #region decide whether actual row is updating or virtual [rowAction]
                string id = clsCommon.getString("select ID from " + tblDetails + " where Tender_No='" + txtTenderNo.Text + "' and ID='1' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and year_code=" + Convert.ToInt32(Session["year"].ToString()) + " ");
                DataRow dr = null;
                DataTable dt = (DataTable)ViewState["currentTable"];

                dr = dt.Rows[0];
                if (id != string.Empty && id != "0")
                {
                    dr["rowAction"] = "U";   //actual row
                }
                else
                {
                    dr["rowAction"] = "A";    //virtual row
                }


                #endregion
                ViewState["currentTable"] = dt;
                grdDetail.DataSource = dt;
                grdDetail.DataBind();
                GridViewRow gr = (GridViewRow)grdDetail.Rows[0];

                gr.Enabled = false;
            }

            this.calculateAmount();
            this.calculateBalanceSelf();
        }
        catch
        {

        }
        SetFocus(txtPacking);
    }
    #endregion

    #region[txtPacking_TextChanged]
    protected void txtPacking_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtQuantal.Text != string.Empty && txtPacking.Text != string.Empty && txtQuantal.Text != "0" && txtPacking.Text != "0")
            {
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtQuantal.Text) * (100 / float.Parse(txtPacking.Text))));
                txtBags.Text = bags.ToString();
                setFocusControl(txtMillRate);
            }
            else if ((txtPacking.Text == string.Empty || txtPacking.Text == "0") && txtQuantal.Text != string.Empty && txtQuantal.Text != "0")
            {
                txtPacking.Text = "50";
                int bags = Convert.ToInt32(Math.Round(float.Parse(txtQuantal.Text) * (100 / float.Parse(txtPacking.Text))));
                txtBags.Text = bags.ToString();
                setFocusControl(txtMillRate);
            }
            else
            {
                txtQuantal.Text = string.Empty;
                setFocusControl(txtQuantal);
                txtBags.Text = "0";
            }
        }
        catch
        {

        }
    }
    #endregion

    #region [txtDate_TextChanged]
    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDate.Text != string.Empty)
            {
                DateTime d = new DateTime();
                // d = DateTime.Now;
                string date = DateTime.Parse(txtDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                //  d = DateTime.Parse(date);
                // d = d.AddDays(15);

                txtLiftingDate.Text = clsCommon.getString("select adddate('" + date + "', interval 10 DAY) as d");
                txtLiftingDate.Text = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                setFocusControl(txtLiftingDate);
            }
            else
            {
                setFocusControl(txtDate);
            }
        }
        catch
        {
            txtDate.Text = string.Empty;
            setFocusControl(txtDate);
            calenderExtenderDate.Animated = true;
        }
    }
    #endregion

    #region [txtLiftingDate_TextChanged]
    protected void txtLiftingDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtLiftingDate.Text != string.Empty)
            {
                // string d = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");

                //DateTime oDate = DateTime.ParseExact(txtLiftingDate.Text, "yyyy/MM/dd", null);
                string liftdate = DateTime.Parse(txtLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                DateTime oDate = DateTime.ParseExact(liftdate, "yyyy/MM/dd", null);
                string d = DateTime.Now.ToString("yyyy/MM/dd");
                DateTime currentDate = DateTime.ParseExact(d, "yyyy/MM/dd", null);

                if (currentDate >= oDate)
                {
                    txtDetailLiftingDate.Text = txtLiftingDate.Text;
                }
                else
                {
                    txtDetailLiftingDate.Text = DateTime.Now.ToString("dd/MM/yyyy"); ;
                }
                setFocusControl(txtMillCode);
            }
        }
        catch
        {
            txtLiftingDate.Text = string.Empty;
            setFocusControl(txtLiftingDate);
        }
    }
    #endregion

    #region [txtDetailSaudaDate_TextChanged]
    protected void txtDetailSaudaDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDetailSaudaDate.Text != string.Empty)
            {
                //string d = DateTime.Parse(txtDetailSaudaDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                //txtDetailLiftingDate.Text = clsCommon.getString("SELECT date_format(DATE_ADD('" + d + "', INTERVAL 10 DAY),'%d/%m/%Y') as d");
                setFocusControl(txtDetailLiftingDate);
            }
        }
        catch
        {
            txtDetailSaudaDate.Text = string.Empty;
            setFocusControl(txtDetailSaudaDate);
        }
    }
    #endregion

    #region [txtDetailLiftingDate_TextChanged]
    protected void txtDetailLiftingDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtDetailLiftingDate.Text != string.Empty)
            {
                // string d = DateTime.Parse(txtDetailLiftingDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                setFocusControl(txtBuyerNarration);
            }
        }
        catch
        {
            txtDetailLiftingDate.Text = string.Empty;
            setFocusControl(txtDetailLiftingDate);
        }
    }
    #endregion

    #region[txtMillRate_TextChanged]
    protected void txtMillRate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txtMillRate.Text != string.Empty)
            {
                setFocusControl(txtCashDiff);
                this.calculateAmount();
                if (drpResale.SelectedValue == "R" || drpResale.SelectedValue == "W")
                {
                    this.calculateDiff();
                }
                if (drpResale.SelectedValue == "M")
                {

                    this.setFocusControl(txtCashDiff);
                }
                else
                {
                    this.setFocusControl(txtPurcRate);
                }
            }
            txtBuyerSaleRate.Text = txtMillRate.Text;

            pnlPopup.Style["display"] = "none";
        }
        catch
        {

        }
    }
    #endregion

    #region [calculateDiff]
    protected void calculateDiff()
    {
        try
        {
            float millrate = 0;
            float purcRate = 0;
            float diff = 0;
            if (txtMillRate.Text != string.Empty)
            {
                millrate = float.Parse(txtMillRate.Text);
            }
            if (txtPurcRate.Text != string.Empty)
            {
                purcRate = float.Parse(txtPurcRate.Text);
            }

            diff = millrate - purcRate;
            lbldiff.Text = diff.ToString();
        }
        catch
        {

        }
    }
    #endregion

    #region [calculateAmount]
    protected void calculateAmount()
    {
        try
        {
            float quantal = 0;
            float millrate = 0;
            double amount = 0;
            float purcrate = 0;
            float diff = 0;

            if (txtQuantal.Text != string.Empty)
            {
                quantal = float.Parse(txtQuantal.Text);
            }
            if (txtMillRate.Text != string.Empty)
            {
                millrate = float.Parse(txtMillRate.Text);
            }
            if (txtPurcRate.Text != string.Empty)
            {
                purcrate = float.Parse(txtPurcRate.Text);
            }
            if (lbldiff.Text != string.Empty)
            {
                diff = float.Parse(lbldiff.Text);
            }

            if (drpResale.SelectedValue == "M")
            {
                amount = quantal * millrate;

            }
            else
            {
                amount = quantal * diff;
            }
            lblAmount.Text = Math.Round(amount, 2).ToString();

            double gstrate = Convert.ToDouble(lblgstrateCode.Text);
            gstrate = Math.Round((millrate * gstrate / 100), 2);
            txtExciseRate.Text = gstrate.ToString();
            lblMillRateGst.Text = (millrate + gstrate).ToString();
        }
        catch
        {

        }
    }
    #endregion

    #region[calculateBalanceSelf]
    /// <summary>
    /// /////////////
    /// </summary>
    protected void calculateBalanceSelf()
    {
        try
        {
            float quantal = 0;
            float balanceSelf = 0;
            float buyerQuantal = 0;
            float quantalTotal = 0;

            //calculate total of quantals in grid

            for (int i = 1; i < grdDetail.Rows.Count; i++)
            {
                if (grdDetail.Rows[i].Cells[rowAction].Text != "D" && grdDetail.Rows[i].Cells[rowAction].Text != "R")
                {
                    //                   if (grdDetail.Rows[i].RowIndex != 0)
                    //                 {
                    quantalTotal = quantalTotal + float.Parse(grdDetail.Rows[i].Cells[7].Text);
                    //               }
                }
            }
            //  quantalTotal = quantalTotal + buyerQuantal;

            if (txtQuantal.Text != string.Empty)
            {
                quantal = float.Parse(txtQuantal.Text);
            }

            if (lblBalanceSelf.Text != string.Empty)
            {
                balanceSelf = float.Parse(lblBalanceSelf.Text);
            }
            if (txtBuyerQuantal.Text != string.Empty)
            {
                buyerQuantal = float.Parse(txtBuyerQuantal.Text);
            }
            balanceSelf = quantal - quantalTotal;
            lblBalanceSelf.Text = balanceSelf.ToString();

            //set to first row balance self
            grdDetail.Rows[0].Cells[7].Text = balanceSelf.ToString();
            //  grdDetail.Rows[0].Cells[12].Text = "U";
            GridViewRow gr = (GridViewRow)grdDetail.Rows[0];
            gr.Enabled = false;
        }
        catch
        {

        }
    }
    #endregion

    #region [txtPurcRate_TextChanged]
    protected void txtPurcRate_TextChanged(object sender, EventArgs e)
    {
        try
        {

            this.calculateDiff();
            this.calculateAmount();
            setFocusControl(txtPaymentTo);
        }
        catch
        {

        }
    }
    #endregion

    #region [drpResale_SelectedIndexChanged]
    protected void drpResale_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (drpResale.SelectedValue == "M")
            {
                txtPurcRate.Enabled = false;
                rfvtxtPurcRate.Enabled = false;

                txtTenderFrom.Text = "2";
                lblTenderFrom.Text = clsCommon.getString("select Ac_Name_E from qrymstaccountmaster where Ac_Code='2' and " +
                    "Company_Code=" + Session["Company_Code"].ToString() + "");
                txtDO.Text = "2";
                lblDO.Text = lblTenderFrom.Text;
                txtVoucherBy.Text = "2";
                lblVoucherBy.Text = lblTenderFrom.Text;

            }
            else
            {
                txtPurcRate.Enabled = true;
                //setFocusControl(txtDate);
                rfvtxtPurcRate.Enabled = true;

                txtTenderFrom.Text = string.Empty;
                lblTenderFrom.Text = string.Empty;
                txtDO.Text = string.Empty;
                lblDO.Text = string.Empty;
                txtVoucherBy.Text = string.Empty;
                lblVoucherBy.Text = string.Empty;
            }

            setFocusControl(drpResale);

        }
        catch
        {

        }
    }
    #endregion

    #region [txtTenderNo_TextChanged]
    protected void txtTenderNo_TextChanged(object sender, EventArgs e)
    {
        #region code
        try
        {
            int n;
            bool isNumeric = int.TryParse(txtTenderNo.Text, out n);

            if (isNumeric == true)
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                string txtValue = "";
                if (txtTenderNo.Text != string.Empty)
                {
                    txtValue = txtTenderNo.Text;

                    string qry = "select * from " + tblHead + " where Tender_No='" + txtValue + "'  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ";
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                //Record Found
                                if (ViewState["mode"] != null)
                                {
                                    if (ViewState["mode"].ToString() == "I")
                                    {
                                        lblMsg.Text = "** Tender No (" + txtValue + ") Already Exist";
                                        lblMsg.ForeColor = System.Drawing.Color.Red;
                                        this.getMaxCode();
                                        txtTenderNo.Enabled = false;
                                        //Session["enableSave"] = 1;
                                        btnSave.Enabled = true;   //IMP
                                        setFocusControl(txtVoucherBy);
                                    }

                                    if (ViewState["mode"].ToString() == "U")
                                    {
                                        //fetch record
                                        bool recordExist = this.fetchRecord(txtValue);
                                        if (recordExist == true)
                                        {
                                            //txtTenderNo.Enabled = true;
                                            pnlgrdDetail.Enabled = true;
                                            setFocusControl(drpResale);
                                        }
                                    }
                                }
                            }
                            else   //Record Not Found
                            {
                                if (ViewState["mode"].ToString() == "I")  //Insert Mode
                                {
                                    lblMsg.Text = "";
                                    setFocusControl(drpResale);
                                    txtTenderNo.Enabled = false;
                                    btnSave.Enabled = true;   //IMP
                                    calculateBalanceSelf();
                                }
                                if (ViewState["mode"].ToString() == "U")
                                {
                                    this.makeEmptyForm("A");
                                    lblMsg.Text = "** Record Not Found";
                                    lblMsg.ForeColor = System.Drawing.Color.Red;
                                    txtTenderNo.Text = string.Empty;
                                    setFocusControl(txtTenderNo);
                                    calculateBalanceSelf();
                                    //txtTenderNo.Enabled = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    lblMsg.Text = string.Empty;
                    txtTenderNo.Focus();
                }
            }
            else
            {
                this.makeEmptyForm("A");
                lblMsg.Text = "Tender No is numeric";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                clsButtonNavigation.enableDisable("E");
                txtTenderNo.Text = string.Empty;
                txtTenderNo.Focus();
            }
        }
        catch
        {

        }
        #endregion
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

            if (hdnfClosePopup.Value == "MM")
            {

                foreach (var s in split)
                {
                    string aa = s.ToString();
                    //name += " Branch_Code Like '%" + aa + "%'or Branch_Name_E Like '%" + aa + "%'or Branch_Address Like '%" + aa + "%'or";
                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or Address_E like '%" + aa + "%' or cityname like '%" + aa + "%' ) and";
                }
                name = name.Remove(name.Length - 3);
                // lblPopupHead.Text = "--Select Group--";

                string qry = " select distinct Ac_Code,Ac_Name_E,cityname as City,Address_E,Ac_type from " + AccountMasterTable
                       + " where Ac_type='M' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())
                       + " and (" + name + ") order by Ac_Name_E desc";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "GR")
            {
                if (txtGrade.Text != string.Empty)
                {
                    split = txtGrade.Text.Split(delimiter);
                }
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    //name += " Branch_Code Like '%" + aa + "%'or Branch_Name_E Like '%" + aa + "%'or Branch_Address Like '%" + aa + "%'or";
                    name += "( System_Name_E like '%" + aa + "%' or System_Type like '%" + aa + "%' ) and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Group--";
                string qry = "select System_Name_E from " + tblPrefix + "SystemMaster where System_Type='S' and company_code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and System_Name_E like '%" + txtSearchText.Text + "%' ";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "PT")
            {
                split = txtPaymentTo.Text.Split(delimiter);
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or CityName like '%" + aa + "%' ) and";

                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Group--";
                string qry = "select Ac_Code , Ac_Name_E,cityname from " + AccountMasterTable + " where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and " + name + " order by Ac_Name_E asc";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "TF")
            {
                txtSearchText.Text = txtTenderFrom.Text;
                lblPopupHead.Text = "--Select Tender From--";
                string qry = "select Ac_Code , Ac_Name_E ,cityname from " + AccountMasterTable +
                    " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " + "and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E asc";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "DO")
            {
                txtSearchText.Text = txtDO.Text;
                lblPopupHead.Text = "--Select DO--";
                string qry = "select Ac_Code , Ac_Name_E ,cityname from " + AccountMasterTable +
                    " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " " + "and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E asc";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "VB")
            {
                txtSearchText.Text = txtVoucherBy.Text;
                lblPopupHead.Text = "--Select Voucher By--";
                string qry = "select Ac_Code ,Ac_Name_E,cityname  from " + AccountMasterTable + " where  Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' " + "and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E asc";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "BR")
            {
                txtSearchText.Text = txtBroker.Text;
                lblPopupHead.Text = "--Select Broker--";
                string qry = "select Ac_Code , Ac_Name_E ,cityname  from " + AccountMasterTable + " where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' " + "and (Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or Short_Name like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E asc";
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "SubBrker")
            {

                txtSearchText.Text = txtsubBroker.Text;
                lblPopupHead.Text = "--Select Buyer--";
                string qry = "SELECT     Ac_Code , Ac_Name_E , Short_Name, cityname  "
                            + " FROM  AccountMaster "
                            + " where AccountMaster.Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' " + "and AccountMaster.Ac_Code like'%" + txtSearchText.Text + "%' or AccountMaster.Ac_Name_E like'%" + txtSearchText.Text + "%' or AccountMaster.Short_Name like'%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%' order by Ac_Name_E asc";
                qry = qry.Replace("AccountMaster", AccountMasterTable);
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "BU")
            {
                txtSearchText.Text = txtBuyer.Text;
                lblPopupHead.Text = "--Select Buyer--";
                string qry = "SELECT     Ac_Code , Ac_Name_E , Short_Name, cityname  "
                            + " FROM  AccountMaster "
                            + " where AccountMaster.Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' " + "and AccountMaster.Ac_Code like'%" + txtSearchText.Text + "%' or AccountMaster.Ac_Name_E like'%" + txtSearchText.Text + "%' or AccountMaster.Short_Name like'%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%' order by Ac_Name_E asc";
                qry = qry.Replace("AccountMaster", AccountMasterTable);
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "BP")
            {
                txtSearchText.Text = txtBuyerParty.Text;
                lblPopupHead.Text = "--Select Buyer Party--";
                string qry = "SELECT     Ac_Code , Ac_Name_E , Short_Name, cityname  "
                            + " FROM  AccountMaster "
                            + " where AccountMaster.Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' " + "and AccountMaster.Ac_Code like'%" + txtSearchText.Text + "%' or AccountMaster.Ac_Name_E like'%" + txtSearchText.Text + "%' or AccountMaster.Short_Name like'%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%' order by Ac_Name_E asc";
                qry = qry.Replace("AccountMaster", AccountMasterTable);
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "GC")
            {
                txtSearchText.Text = txtGstrateCode.Text;
                lblPopupHead.Text = "--Select Buyer Party--";
                string qry = "select Doc_no,GST_Name,Rate from " + tblPrefix + "GSTRateMaster where  ( Doc_no like '%" + txtSearchText.Text + "%' or GST_Name like '%" + txtSearchText.Text + "%' or Rate like '%" + txtSearchText.Text + "%') order by GST_Name"; ;

                this.showPopup(qry);
            }


            if (hdnfClosePopup.Value == "TN" || hdnfClosePopup.Value == "txtEditDoc_No")
            {
                txtSearchText.Text = txtTenderNo.Text;
                pnlPopup.Style["display"] = "block";
                lblPopupHead.Text = "--Select Tender --";
                // string qry = "select distinct Tender_No,CONVERT(Date, Tender_Date,103) as Tender_Date,millname,Quantal,Packing,Mill_Rate,doname from " + qryCommon + " where ([millname] like '%" + txtSearchText.Text + "%' or Tender_No like '%" + txtSearchText.Text + "%' or doname like '%" + txtSearchText.Text + "%') and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by Tender_No desc";

                qry = "SELECT  Tender_No, Tender_Date,  millname, Quantal,Grade,buyerbrokershortname, Buyer_Quantal,Mill_Rate, Sale_Rate,doname" +
                " FROM  " + qryCommon + " where Buyer=2 and (Tender_No like '%" + txtSearchText.Text + "%' or Tender_Date like '%" + txtSearchText.Text + "%' or millname like '%" + txtSearchText.Text + "%' or millfullname like '%" + txtSearchText.Text + "%') and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by Tender_No desc";
                this.showPopup(qry);

            }
            if (hdnfClosePopup.Value == "txtitem_code")
            {
                txtSearchText.Text = txtitem_code.Text;
                lblPopupHead.Text = "--Select item--";
                //string qry = "select itemcode ,itemname as Name from qrytenderhead where  Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString() + " " +
                //    " and Year_Code=" + Session["year"].ToString() + " and (itemcode like '%" + txtSearchText.Text + "%' or  itemname like  '%" + txtSearchText.Text + "%'");

                qry = "select  System_Code,System_Name_E,Vat_AC as Gst,gstratre from qrymstitem where   (System_Code like '%" + txtSearchText.Text + "%' or System_Name_E like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);
            }
            //  hdnfClosePopup.Value = "";
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
            if (hdnfClosePopup.Value == "MM")
            {
                setFocusControl(txtMillCode);
            }
            if (hdnfClosePopup.Value == "GR")
            {
                setFocusControl(txtGrade);
            }
            if (hdnfClosePopup.Value == "PT")
            {
                setFocusControl(txtPaymentTo);
            }
            if (hdnfClosePopup.Value == "TF")
            {
                setFocusControl(txtTenderFrom);
            }
            if (hdnfClosePopup.Value == "DO")
            {
                setFocusControl(txtDO);
            }
            if (hdnfClosePopup.Value == "VB")
            {
                setFocusControl(txtVoucherBy);
            }
            if (hdnfClosePopup.Value == "BR")
            {
                setFocusControl(txtBroker);
            }
            if (hdnfClosePopup.Value == "BU")
            {
                setFocusControl(txtBuyer);
            }
            if (hdnfClosePopup.Value == "BP")
            {
                setFocusControl(txtBuyerParty);
            }
            if (hdnfClosePopup.Value == "BU")
            {
                setFocusControl(txtBuyer);
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

    #region [btnChangeNo_Click]
    protected void changeNo_click(object sender, EventArgs e)
    {
        try
        {

            //if (hdnfClosePopup.Value =="TN")
            //{

            if (btnChangeNo.Text == "Change No")
            {
                pnlPopup.Style["display"] = "none";
                txtTenderNo.Text = string.Empty;
                txtTenderNo.Enabled = true;

                btnSave.Enabled = false;
                setFocusControl(txtTenderNo);

            }
            if (btnChangeNo.Text == "Choose No")
            {
                try
                {
                    pnlgrdDetail.Enabled = true;
                    //setFocusControl(txtSearchText);
                    hdnfClosePopup.Value = "TN";
                    pnlPopup.Style["display"] = "block";
                    btnSearch_Click(sender, e);

                }
                catch
                {

                }
                //}

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

            e.Row.Cells[0].ControlStyle.Width = new Unit("40px");
            e.Row.Cells[1].ControlStyle.Width = new Unit("40px");
            e.Row.Cells[Detail_Id].ControlStyle.Width = new Unit("30px");
            e.Row.Cells[Party].ControlStyle.Width = new Unit("60px");
            e.Row.Cells[Name_of_Party].ControlStyle.Width = new Unit("120px");
            e.Row.Cells[Broker_No].ControlStyle.Width = new Unit("50px");
            e.Row.Cells[Name_Of_Broker].ControlStyle.Width = new Unit("120px");
            e.Row.Cells[quantal].ControlStyle.Width = new Unit("60px");
            e.Row.Cells[saleRate].ControlStyle.Width = new Unit("60px");
            e.Row.Cells[commissions].ControlStyle.Width = new Unit("60px");
            e.Row.Cells[saudaDate].ControlStyle.Width = new Unit("90px");
            e.Row.Cells[liftingDate].ControlStyle.Width = new Unit("90px");
            e.Row.Cells[Sauda_Narration].ControlStyle.Width = new Unit("120px");
            e.Row.Cells[delivaryType].Style["overflow"] = "hidden";
            e.Row.Cells[delivaryType].ToolTip = e.Row.Cells[10].Text;
            e.Row.Cells[delivaryType].ControlStyle.Width = new Unit("120px");
            e.Row.Cells[tenderdetailid].ControlStyle.Width = new Unit("90px");
            e.Row.Cells[sub_broker].ControlStyle.Width = new Unit("60px");
            e.Row.Cells[subBrokername].ControlStyle.Width = new Unit("180px");

            e.Row.Cells[delivaryType].Visible = true;
            e.Row.Cells[tenderdetailid].Visible = true;
            e.Row.Cells[rowAction].Visible = true;
            e.Row.Cells[SrNo].Visible = false;
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
            e.Row.Cells[11].Style["overflow"] = "hidden";
            e.Row.Cells[subBrokername].Style["overflow"] = "hidden";
            int i = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;

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
            string v = hdnfClosePopup.Value;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                if (v == "TN")
                {

                    e.Row.Cells[0].Width = new Unit("30px");
                    e.Row.Cells[2].Width = new Unit("100px");
                    e.Row.Cells[3].Width = new Unit("80px");
                    e.Row.Cells[4].Width = new Unit("100px");

                }
                if (v == "txtitem_code")
                {
                    e.Row.Cells[0].Width = new Unit("50px");
                    e.Row.Cells[1].Width = new Unit("150px");
                    e.Row.Cells[2].Width = new Unit("60px");
                    e.Row.Cells[3].Width = new Unit("100px");
                }
                if (v == "VB" || v == "GC")
                {
                    e.Row.Cells[0].Width = new Unit("100px");
                    e.Row.Cells[1].Width = new Unit("400px");
                    e.Row.Cells[2].Width = new Unit("100px");

                }
                if (v == "MM")
                {

                    e.Row.Cells[0].Width = new Unit("30px");
                    e.Row.Cells[1].Width = new Unit("30px");
                    e.Row.Cells[2].Width = new Unit("50px");
                    e.Row.Cells[3].Width = new Unit("30px");
                }


                if (v == "PT" || v == "TF" || v == "DO" || v == "VB" || v == "BR")
                {
                    //if (e.Row.RowType == DataControlRowType.DataRow)
                    //{
                    e.Row.Cells[0].Width = new Unit("150px");
                    e.Row.Cells[1].Width = new Unit("500px");
                    e.Row.Cells[2].Width = new Unit("300px");
                    //}
                    //if (e.Row.RowType != DataControlRowType.Pager)
                    //{
                    //e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    ////e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                    ////e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(60);
                    ////e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(30);
                    //e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    //}
                }
                if (v == "GR")
                {
                    e.Row.Cells[0].Width = new Unit("400px");

                }

                if (v == "BU" || v == "BP")
                {
                    //if (e.Row.RowType != DataControlRowType.Pager)
                    //{
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(10);
                    e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(50);
                    e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(20);
                    e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(20);
                    // }
                }

            }
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
                strTextbox = hdnfClosePopup.Value;

                setFocusControl(btnSearch);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [txtBuyerQuantal_TextChanged]
    protected void txtBuyerQuantal_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtBuyerSaleRate);
    }
    #endregion

    #region [txtBuyerSaleRate_TextChanged]
    protected void txtBuyerSaleRate_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtBuyerCommission);
    }
    #endregion

    #region [txtBuyerCommission_TextChanged]
    protected void txtBuyerCommission_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtDetailSaudaDate);
    }
    #endregion

    #region [txtBuyerNarration_TextChanged]
    protected void txtBuyerNarration_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(btnADDBuyerDetails);
    }
    #endregion

    #region [txtExciseRate_TextChanged]
    protected void txtExciseRate_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtSellNoteNo);
    }
    #endregion

    #region [txtNarration_TextChanged]
    protected void txtNarration_TextChanged(object sender, EventArgs e)
    {
        setFocusControl(txtBuyer);
    }
    #endregion

    #region [setFocusControl]
    private void setFocusControl(WebControl wc)
    {
        objAsp = wc;
        System.Web.UI.ScriptManager.GetCurrent(this).SetFocus(wc);
    }
    #endregion

    protected void btn_Click(object sender, EventArgs e)
    {
        //string url = "http://www.google.com";
        //StringBuilder sb = new StringBuilder();
        //sb.Append("<script type = 'text/javascript'>");
        //sb.Append("window.open('");
        //sb.Append(url);
        //sb.Append(",'_blank'");
        //sb.Append("');");
        //sb.Append("</script>");
        //ClientScript.RegisterStartupScript(this.GetType(), "script", sb.ToString());
    }

    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);

    }
    protected void drpDeliveryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        setFocusControl(drpDeliveryType);
        //if (drpDeliveryType.SelectedValue == "N")
        //{
        //    //txtBuyerCommission.Enabled = false;
        //    txtBuyerCommission.Text = "0";
        //}
        //else
        //{
        //txtBuyerCommission.Enabled = true;
        AddCommission();
        //}
    }
    protected void txtSellNoteNo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            setFocusControl(txtNarration);
        }
        catch (Exception)
        {
            throw;
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
                int Tender_id = Convert.ToInt32(clsCommon.getString("select tenderid from nt_1_tender where Tender_No=" + txtEditDoc_No.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " and " +
                    " Year_Code=" + Session["year"].ToString() + ""));

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ky", "javascript:TenderOPen('" + Tender_id + "')", true);

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
            counts = Convert.ToInt32(clsCommon.getString("select ifnull(count(Tender_No),0) as Tender_No from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' and Year_Code='" + Session["Year"].ToString() + "'"));
            if (counts == 0)
            {
                txtTenderNo.Text = "1";
                Tender_No = 1;
            }
            else
            {
                Tender_No = Convert.ToInt32(clsCommon.getString("SELECT max(Tender_No) as Tender_No from " + tblHead + " where Company_Code='" + Session["Company_Code"].ToString() + "' and  Year_Code='" + Session["Year"].ToString() + "'")) + 1;
                txtTenderNo.Text = Tender_No.ToString();
            }

            counts = Convert.ToInt32(clsCommon.getString("SELECT ifnull(count(tenderid),0) as tenderid from " + tblHead + " "));
            if (counts == 0)
            {
                lblTender_Id.Text = "1";
                id = 1;
            }
            else
            {
                id = Convert.ToInt32(clsCommon.getString("SELECT max(tenderid) as tenderid from " + tblHead)) + 1;
                lblTender_Id.Text = id.ToString();
            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Next Record Error!')", true);
        }
    }
    #endregion





    protected void txtitem_code_TextChanged(object sender, EventArgs e)
    {
        try
        {
            hdnfClosePopup.Value = "Close";
            string itemname = string.Empty;
            searchString = txtMillCode.Text;
            if (txtitem_code.Text != string.Empty)
            {
                bool a = clsCommon.isStringIsNumeric(txtitem_code.Text);
                if (a == false)
                {
                    btntxtitem_code_Click(this, new EventArgs());
                }
                else
                {
                    itemname = clsCommon.getString("select System_Name_E from qrymstitem where System_Code=" + txtitem_code.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " ");

                    string itemgst = clsCommon.getString("select Vat_AC from qrymstitem where System_Code=" + txtitem_code.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " ");
                    string rr = clsCommon.getString("select gstratre from qrymstitem where System_Code=" + txtitem_code.Text + " and Company_Code=" + Session["Company_Code"].ToString() + " ");
                    if (itemname != string.Empty && itemname != "0")
                    {
                        lblitemname.Text = itemname;
                        txtGstrateCode.Text = itemgst;
                        lblgstrateCode.Text = rr;
                        this.calculateAmount();
                        setFocusControl(txtGrade);
                    }
                    else
                    {
                        txtitem_code.Text = string.Empty;
                        lblitemname.Text = string.Empty;
                        setFocusControl(txtitem_code);
                    }
                }
            }
            else
            {
                txtitem_code.Text = string.Empty;
                lblitemname.Text = string.Empty;
                setFocusControl(txtitem_code);
            }
        }
        catch
        {
        }
    }
    protected void btntxtitem_code_Click(object sender, EventArgs e)
    {
        try
        {
            txtSearchText.Text = string.Empty;
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtitem_code";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    #region LV POsting
    private void LV_Posting()
    {
        try
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

            string PartyStateCode = clsCommon.getString("select GSTStateCode from qrymstaccountmaster where Ac_Code=" + Voucher_By + " and Company_Code='" + Session["Company_Code"].ToString() + "'");
            string CompanyStateCode = clsCommon.getString("select GSTStateCode from NT_1_CompanyParameters where Company_Code='" + Session["Company_Code"].ToString() + "' and Year_Code='" + Session["year"].ToString() + "'");

            double CGSTRATE = Convert.ToDouble(clsCommon.getString("select CGST from NT_1_GSTRateMaster where Doc_no=" + GstRate_Code + " and Company_Code=" + Session["Company_Code"].ToString() + " "));
            double SGSTRATE = Convert.ToDouble(clsCommon.getString("select SGST from NT_1_GSTRateMaster where Doc_no=" + GstRate_Code + " and Company_Code=" + Session["Company_Code"].ToString() + " "));
            double IGSTRATE = Convert.ToDouble(clsCommon.getString("select IGST from NT_1_GSTRateMaster where Doc_no=" + GstRate_Code + " and Company_Code=" + Session["Company_Code"].ToString() + " "));
            double GSTRATE = Convert.ToDouble(clsCommon.getString("select Rate from NT_1_GSTRateMaster where Doc_no=" + GstRate_Code + " and Company_Code=" + Session["Company_Code"].ToString() + " "));


            double taxmillamt = Quantal * Diff_Amount;

            double CGSTAmount = 0.00;
            double SGSTAmount = 0.00;
            double IGSTAmount = 0.00;

            if (CompanyStateCode == PartyStateCode)
            {
                CGSTAmount = Math.Round((taxmillamt * CGSTRATE / 100), 2);
                SGSTAmount = Math.Round((taxmillamt * SGSTRATE / 100), 2);
                IGSTAmount = 0.00;
                IGSTRATE = 0.00;
            }
            else
            {
                CGSTAmount = 0.000;
                SGSTAmount = 0.00;
                CGSTRATE = 0.00;
                SGSTRATE = 0.00;
                IGSTAmount = Math.Round((taxmillamt * IGSTRATE / 100), 2);

            }

            double Voucher_Amt = taxmillamt + CGSTAmount + SGSTAmount + IGSTAmount;

            LV.LV_ac = vb;
            LV.LV_bc = bk;
            LV.LV_mc = mc;
            LV.LV_Doc_No = voucher_no;
            LV.LV_Doc_Date = DateTime.Parse(txtDate.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
            LV.LV_Link_No = 0;
            LV.LV_link_Type = "";
            LV.LV_Link_id = 0;
            LV.LV_Ac_Code = Convert.ToInt32(Voucher_By);
            LV.LV_Unit_Code = 0;
            LV.LV_Broker_CODE = Convert.ToInt32(txtBroker.Text != string.Empty ? txtBroker.Text : "0");
            LV.LV_Quantal = Quantal;
            LV.LV_PACKING = Packing;
            LV.LV_BAGS = Bags;
            LV.LV_Grade = Grade;
            LV.LV_Transport_Code = 0;
            LV.LV_Mill_Rate = Convert.ToDouble(Mill_Rate);
            LV.LV_Sale_Rate = 0.00;
            LV.LV_Purchase_Rate = Convert.ToDouble(txtPurcRate.Text != string.Empty ? txtPurcRate.Text : "0.00"); ;
            LV.LV_FREIGHT = 0.00;
            LV.LV_Narration1 = myNarration;
            LV.LV_Narration2 = string.Empty;
            LV.LV_Narration3 = string.Empty;
            LV.LV_Narration4 = string.Empty;
            LV.LV_Voucher_Amount = Diff_Amount;
            LV.LV_Diff_Amount = Diff_Amount;
            LV.LV_Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            LV.LV_Year_Code = Convert.ToInt32(Session["year"].ToString());
            LV.LV_Branch_Code = 0;
            LV.LV_Created_By = "" + Session["user"].ToString() + "";
            LV.LV_Commission_Rate = 0.00;
            LV.LV_Resale_Commisson = 0.00;
            LV.LV_GstRateCode = GstRate_Code;
            LV.LV_CGSTRate = CGSTRATE;
            LV.LV_CGSTAmount = CGSTAmount;
            LV.LV_SGSTRate = SGSTRATE;
            LV.LV_SGSTAmount = SGSTAmount;
            LV.LV_IGSTRate = IGSTRATE;
            LV.LV_IGSTAmount = IGSTAmount;
            LV.LV_TaxableAmount = Diff_Amount;

        }
        catch
        {
        }
    }
    #endregion
    protected void txtCashDiff_TextChanged(object sender, EventArgs e)
    {

    }
}