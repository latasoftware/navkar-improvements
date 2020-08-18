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

public partial class Sugar_BussinessRelated_pgeSaudaBookUtility : System.Web.UI.Page
{
    #region data section


    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    string qryCommon = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    static WebControl objAsp = null;
    string user = string.Empty;
    string cs = string.Empty;
    string Action = string.Empty;
    string retValue;
    string fornotsaverecord;
    string str = string.Empty;
    #endregion
    #region updatepart

    MySqlConnection con = null;
    MySqlCommand cmd = null;
    MySqlTransaction myTran = null;

    DataTable Maindt = null;
    DataRow dr = null;
    string msg = string.Empty;

    Int32 mill_Code;
    Int32 Tender_No = 0;
    Int32 party = 0;
    string DeliveryType = string.Empty;
    Int32 broker = 0;
    int count = 0;
    Int32 subbroker = 0;
    Int32 Doc_No = 0;
    double quntal = 0.00;
    string saudadate = string.Empty;
    double sale_rate = 0.00;
    double commision_rate = 0.00;
    string payment_Date = string.Empty;
    int tenderID = 0;
    int tenderdetailid = 0;
    double TenderQuntl = 0.00;
    int partyid = 0;
    int brokerid = 0;
    int subbrokerid = 0;
    string dtype = string.Empty;
    string Delivery_Type = "";
    string Narration = "";
    #endregion


    string Head_Insert = string.Empty;
    StringBuilder Head_Update = null;
    string Head_Delete = string.Empty;
    StringBuilder Head_Fields = null;
    StringBuilder Head_Values = null;
    string Update_Detail = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        cs = ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
        con = new MySqlConnection(cs);
        tblHead = "nt_1_tenderdetails";

        Head_Fields = new StringBuilder();
        Head_Values = new StringBuilder();
        Head_Update = new StringBuilder();

        Maindt = new DataTable();
        dr = null;
        Maindt.Columns.Add("Querys", typeof(string));
        dr = Maindt.NewRow();
        if (!Page.IsPostBack)
        {
            pnlPopup.Style["display"] = "none";
            txtsaudadate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            txtpayment.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            if (hdnfClosePopup.Value == "Close" || hdnfClosePopup.Value == "")
            {
                pnlPopup.Style["display"] = "none";
            }
            else
            {
                pnlPopup.Style["display"] = "block";
                objAsp = btnSearch;
            }
            setFocusControl(txtmillcode);
        }
    }
    #region[txtmillcode_TextChanged]
    protected void txtmillcode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtmillcode.Text;
        strTextBox = "txtmillcode";
        csCalculations();

    }
    #endregion

    #region[txtparty_TextChanged]
    protected void txtparty_TextChanged(object sender, EventArgs e)
    {
        searchString = txtparty.Text;
        strTextBox = "txtparty";
        csCalculations();

    }
    #endregion

    #region[txtbroker_TextChanged]
    protected void txtbroker_TextChanged(object sender, EventArgs e)
    {
        searchString = txtbroker.Text;
        strTextBox = "txtbroker";
        csCalculations();

    }
    #endregion

    #region[txtsubbroker_TextChanged]
    protected void txtsubbroker_TextChanged(object sender, EventArgs e)
    {
        searchString = txtsubbroker.Text;
        strTextBox = "txtsubbroker";
        csCalculations();

    }
    #endregion

    #region [btntxtmillcode_Click]
    protected void btntxtmillcode_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfpopup.Value == "")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtmillcode";
                btnSearch_Click(sender, e);
            }
            else
            {
                pnlPopup.Style["display"] = "none";
                hdnfpopup.Value = null;

            }

        }
        catch
        {
        }
    }
    #endregion

    #region [btntxtparty_Click]
    protected void btntxtparty_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfpopup.Value == "")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtparty";
                btnSearch_Click(sender, e);
            }
            else
            {
                pnlPopup.Style["display"] = "none";
                hdnfpopup.Value = null;

            }
        }
        catch
        {
        }
    }
    #endregion

    #region [btntxtbroker_Click]
    protected void btntxtbroker_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfpopup.Value == "")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtbroker";
                btnSearch_Click(sender, e);
            }
            else
            {
                pnlPopup.Style["display"] = "none";
                hdnfpopup.Value = null;

            }
        }
        catch
        {
        }
    }
    #endregion

    #region [btntxtsubbroker_Click]
    protected void btntxtsubbroker_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfpopup.Value == "")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtsubbroker";
                btnSearch_Click(sender, e);
            }
            else
            {
                pnlPopup.Style["display"] = "none";
                hdnfpopup.Value = null;

            }
        }
        catch
        {
        }
    }
    #endregion

    #region [btnUpdate_Click]
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        //btnUpdate.Enabled = false;
        #region Assign Values
        fornotsaverecord = retValue;
        retValue = string.Empty;

        //string qry = "";
        str = string.Empty;

        bool isvalidate = true;

        #region validation
        if (txtmillcode.Text != string.Empty && txtmillcode.Text != "0")
        {
            isvalidate = true;
        }
        else
        {
            txtmillcode.Text = string.Empty;
            setFocusControl(txtmillcode);
            return;
        }
        if (txttender.Text != string.Empty && txttender.Text != "0")
        {
            isvalidate = true;
        }
        else
        {
            txttender.Text = string.Empty;
            setFocusControl(txttender);
            return;
        }

        if (txtparty.Text != string.Empty && txtparty.Text != "0")
        {
            isvalidate = true;
        }
        else
        {
            txtparty.Text = string.Empty;
            setFocusControl(txtparty);
            return;
        }
        if (txtbroker.Text != string.Empty && txtbroker.Text != "0")
        {
            isvalidate = true;
        }
        else
        {
            txtbroker.Text = string.Empty;
            setFocusControl(txtbroker);
            return;
        }

        if (txtsubbroker.Text != string.Empty && txtsubbroker.Text != "0")
        {
            isvalidate = true;
        }
        else
        {
            txtsubbroker.Text = string.Empty;
            setFocusControl(txtsubbroker);
            return;
        }

        if (txtqntl.Text != string.Empty && txtqntl.Text != "0")
        {
            isvalidate = true;
        }
        else
        {
            txtqntl.Text = string.Empty;
            setFocusControl(txtqntl);
            return;
        }
        #endregion

        int Year_Code = Convert.ToInt32(Session["year"].ToString());
        int Company_Code = Convert.ToInt32(Convert.ToInt32(Session["Company_Code"].ToString()));

        mill_Code = txtmillcode.Text != string.Empty ? Convert.ToInt32(txtmillcode.Text) : 0;
        Tender_No = txttender.Text != string.Empty ? Convert.ToInt32(txttender.Text) : 0;
        party = txtparty.Text != string.Empty ? Convert.ToInt32(txtparty.Text) : 0;
        try
        {
            partyid = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + party + " and company_code='" + Session["Company_Code"].ToString() + "'"));
        }
        catch
        {

        }
        dtype = drpDeliveryType.SelectedValue;

        if (dtype == "Commission")
        {
            Delivery_Type = "C";
        }
        else
        {
            Delivery_Type = "N";
        }

        broker = txtbroker.Text != string.Empty ? Convert.ToInt32(txtbroker.Text) : 0;
        try
        {
            brokerid = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + broker + " and company_code='" + Session["Company_Code"].ToString() + "'"));
        }
        catch
        {

        }
        subbroker = txtsubbroker.Text != string.Empty ? Convert.ToInt32(txtsubbroker.Text) : 0;
        try
        {
            subbrokerid = Convert.ToInt32(clsCommon.getString("select accoid from qrymstaccountmaster where Ac_Code=" + subbroker + " and company_code='" + Session["Company_Code"].ToString() + "'"));
        }
        catch
        {

        }
        quntal = txtqntl.Text != string.Empty ? Convert.ToDouble(txtqntl.Text) : 0.00;
        DateTime saudadate1 = Convert.ToDateTime(txtsaudadate.Text);
        saudadate = saudadate1.ToString("yyyy/MM/dd");
        sale_rate = txtsalerate.Text != string.Empty ? Convert.ToDouble(txtsalerate.Text) : 0.00;
        commision_rate = txtcommrate.Text != string.Empty ? Convert.ToDouble(txtcommrate.Text) : 0.00;
        DateTime payment_Date1 = Convert.ToDateTime(txtpayment.Text);
        payment_Date = payment_Date1.ToString("yyyy/MM/dd");
        //int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
        Year_Code = Convert.ToInt32(Session["year"].ToString());
        string Created_By = Session["user"].ToString();


        int AutoID = 0;
        int flag = 0;
        string op = string.Empty;
        string returnmaxno = string.Empty;
        #endregion

        btnUpdate.Enabled = false;

        #region[head master]

        string isactive = "true";
        if (btnUpdate.Text == "Update")
        {
            this.NextNumber();

            TenderQuntl = Convert.ToDouble(clsCommon.getString("SELECT Buyer_Quantal from nt_1_tenderdetails where tenderdetailid=" + tenderdetailid + " and Company_Code='" + Session["Company_Code"].ToString() + "' and Year_Code=" + Convert.ToInt32(Session["year"]).ToString()));
            TenderQuntl = TenderQuntl - quntal;

            #region Head Add Fields And Values
            Head_Fields.Append("Tender_No,");
            Head_Values.Append("'" + Tender_No + "',");
            Head_Fields.Append("Company_Code,");
            Head_Values.Append("'" + Company_Code + "',");
            Head_Fields.Append("Buyer,");
            Head_Values.Append("'" + party + "',");
            Head_Fields.Append("Buyer_Quantal,");
            Head_Values.Append("'" + quntal + "',");
            Head_Fields.Append("Sale_Rate,");
            Head_Values.Append("'" + sale_rate + "',");
            Head_Fields.Append("Commission_Rate,");
            Head_Values.Append("'" + commision_rate + "',");
            Head_Fields.Append("Sauda_Date,");
            Head_Values.Append("'" + saudadate + "',");
            Head_Fields.Append("Lifting_Date,");
            Head_Values.Append("'" + payment_Date + "',");
            Head_Fields.Append("Narration,");
            Head_Values.Append("'" + Narration + "',");
            Head_Fields.Append("ID,");
            Head_Values.Append("'" + count + "',");
            Head_Fields.Append("Buyer_Party,");
            Head_Values.Append("'" + broker + "',");
            Head_Fields.Append("sub_broker,");
            Head_Values.Append("'" + subbroker + "',");
            Head_Fields.Append("year_code,");
            Head_Values.Append("'" + Year_Code + "',");
            Head_Fields.Append("Delivery_Type,");
            Head_Values.Append("'" + Delivery_Type + "',");
            Head_Fields.Append("tenderid,");
            Head_Values.Append("'" + tenderID + "',");
            Head_Fields.Append("tenderdetailid,");
            Head_Values.Append("'" + Doc_No + "',");
            Head_Fields.Append("buyerid,");
            Head_Values.Append("'" + partyid + "',");
            Head_Fields.Append("buyerpartyid,");
            Head_Values.Append("'" + brokerid + "',");
            Head_Fields.Append("sbr");
            Head_Values.Append("'" + subbrokerid + "'");
            #endregion

            Head_Insert = "insert into " + tblHead + "(" + Head_Fields + ") values(" + Head_Values + ")";
            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = Head_Insert;
            Maindt.Rows.Add(dr);

            Update_Detail = "Update " + tblHead + " set Buyer_Quantal='" + TenderQuntl + "' where tenderdetailid ='" + tenderdetailid + "'";
            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = Update_Detail;
            Maindt.Rows.Add(dr);
            flag = 1;

        }
        else
        {
            //Tender_No = Convert.ToInt32(txttender.Text);


            //#region Update Head Add Fields And Values          
            //Head_Update = Head_Update + "Buyer_Quantal=";
            //Head_Update = Head_Update + "'" + quntal + "'";

            //Head_Update = "update " + tblHead + " set " + Head_Update + " where tenderdetailid='" + txttender + "'";

            //#endregion

            //flag = 2;
            //Thread thred = new Thread(() => { count = DataStore(Head_Insert, Update_detail, flag); });
            //thred.Start();
            //thred.Join();
            //if (count == 2)
            //{
            //    hdnf.Value = count.ToString();   
            //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update !')", true);
            //}

        }

        #endregion
        msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);
        if (msg == "Insert")
        {
            txtmillcode.Text = string.Empty;
            txttender.Text = string.Empty;
            txtparty.Text = string.Empty;
            txtbroker.Text = string.Empty;
            txtsubbroker.Text = string.Empty;
            txtqntl.Text = string.Empty;
            txtsalerate.Text = string.Empty;
            txtcommrate.Text = string.Empty;
            btnUpdate.Enabled = true;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Added !')", true);
        }

    }


    #endregion


    #region Generate Next Number
    private void
        NextNumber()
    {
        try
        {
            int counts = 0;

            Doc_No = Convert.ToInt32(clsCommon.getString("SELECT max(tenderdetailid) as Tender_No from nt_1_tenderdetails")) + 1;
            counts = Convert.ToInt32(clsCommon.getString("SELECT max(ID) as ID from nt_1_tenderdetails where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                " and year_code='" + Session["year"].ToString() + "' and Tender_No=" + txttender.Text + " ")) + 1;
            count = counts;
            tenderID = Convert.ToInt32(clsCommon.getString("SELECT tenderid as tenderid from nt_1_tender where Company_Code='" + Session["Company_Code"].ToString() + "' " +
                " and year_code='" + Session["year"].ToString() + "' and Tender_No=" + txttender.Text + " "));

            tenderdetailid = Convert.ToInt32(clsCommon.getString("SELECT tenderdetailid from nt_1_tenderdetails where Tender_No=" + txttender.Text + " and Company_Code='" + Session["Company_Code"].ToString() + "' " +
                " and year_code='" + Session["year"].ToString() + "' and Buyer='2' "));

        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Next Record Error!')", true);
        }
    }
    #endregion

    #region [imgBtnClose_Click]
    protected void imgBtnClose_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == "txtmillcode")
            {
                setFocusControl(txtmillcode);
            }

            if (hdnfClosePopup.Value == "txtparty")
            {
                setFocusControl(txtparty);
            }
            if (hdnfClosePopup.Value == "txtbroker")
            {
                setFocusControl(txtbroker);
            }
            if (hdnfClosePopup.Value == "txtsubbroker")
            {
                setFocusControl(txtsubbroker);
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

    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);

    }

    protected void drpDeliveryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        setFocusControl(txtbroker);

    }

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            string v = hdnfClosePopup.Value;
            if (e.Row.RowType != DataControlRowType.Pager)
            {

                e.Row.Cells[0].ControlStyle.Width = new Unit("40px");
                e.Row.Cells[1].ControlStyle.Width = new Unit("100px");
                e.Row.Cells[2].ControlStyle.Width = new Unit("100px");
                e.Row.Cells[3].ControlStyle.Width = new Unit("200px");
                e.Row.Cells[4].ControlStyle.Width = new Unit("150px");
                e.Row.Cells[5].ControlStyle.Width = new Unit("50px");
                e.Row.Cells[6].ControlStyle.Width = new Unit("50px");
                e.Row.Cells[7].ControlStyle.Width = new Unit("40px");
                e.Row.Cells[8].ControlStyle.Width = new Unit("50px");
                e.Row.Cells[9].ControlStyle.Width = new Unit("40px");
            }
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
            if (hdnfClosePopup.Value == "txtmillcode")
            {

                foreach (var s in split)
                {
                    string aa = s.ToString();

                    name += "( Tender_No like '%" + aa + "%' or millshortname like '%" + aa + "%' or date_format(Tender_Date,'%Y/%m/%d') like '%" + aa + "%' or tenderdoname like '%" + aa + "%' or Grade like '%" + aa + "%'or Mill_Rate like '%"
                         + aa + "%' or balance like '%" + aa + "%' or tenderid like '%" + aa + "%' or Mill_Code like '%" + aa + "%' or mc like '%" + aa + "%' ) and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Mill code--";
                string qry = "select Tender_No,millshortname,date_format(Tender_Date,'%Y/%m/%d') as Lifting_Date,tenderdoname,Grade,Mill_Rate,balance,tenderid,Mill_Code,mc " +
                    " from qryselftenderbalancemain where Buyer=2 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_code=" + Session["year"].ToString() + " " +
                " and " + name + "";
                this.showPopup(qry);

            }

            if (hdnfClosePopup.Value == "txtparty")
            {

                foreach (var s in split)
                {
                    string aa = s.ToString();

                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or cityname like '%" + aa + "%') and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Party--";
                string qry = "select Ac_Code,Ac_Name_E,cityname from qrymstaccountmaster where (Ac_Code like '%" + txtSearchText.Text +
                             "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or cityname like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);

            }
            if (hdnfClosePopup.Value == "txtbroker")
            {

                foreach (var s in split)
                {
                    string aa = s.ToString();

                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or cityname like '%" + aa + "%') and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Broker--";
                string qry = "select Ac_Code,Ac_Name_E,cityname from qrymstaccountmaster where (Ac_Code like '%" + txtSearchText.Text +
                             "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or cityname like '%" + txtSearchText.Text + "%')";
                this.showPopup(qry);

            }

            if (hdnfClosePopup.Value == "txtsubbroker")
            {

                foreach (var s in split)
                {
                    string aa = s.ToString();

                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or cityname like '%" + aa + "%') and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Sub Broker--";
                string qry = "select Ac_Code,Ac_Name_E,cityname from qrymstaccountmaster where (Ac_Code like '%" + txtSearchText.Text +
                             "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or cityname like '%" + txtSearchText.Text + "%')";
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
                        //pnlPopup.Style["display"] = "block";

                    }
                    else
                    {
                        grdPopup.DataSource = null;
                        grdPopup.DataBind();
                        hdHelpPageCount.Value = "0";
                        // pnlPopup.Style["display"] = "block";
                    }
                }
            }
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

    #region csCalculations
    private void csCalculations()
    {
        try
        {
            //hdnfpopup.Value = null;

            if (strTextBox == "txtmillcode")
            {
                if (txtmillcode.Text != string.Empty)
                {
                    qry = "select Mill_Code,millshortname,Tender_No,BALANCE,Grade,Lifting_DateConverted from " + tblPrefix + "qrytenderdobalanceview  where Mill_Code=" + txtmillcode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString());

                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    ds = clsDAL.SimpleQuery(qry);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            txtmillcode.Text = ds.Tables[0].Rows[0]["Mill_Code"].ToString();
                            lblmillname.Text = ds.Tables[0].Rows[0]["millshortname"].ToString();
                            txttender.Text = ds.Tables[0].Rows[0]["Tender_No"].ToString();
                            lblselfqty.Text = ds.Tables[0].Rows[0]["BALANCE"].ToString();
                            lblgrade.Text = ds.Tables[0].Rows[0]["Grade"].ToString();
                            lbllifting.Text = ds.Tables[0].Rows[0]["Lifting_DateConverted"].ToString();
                            txtqntl.Text = ds.Tables[0].Rows[0]["BALANCE"].ToString();
                            setFocusControl(txtparty);


                        }
                    }
                }
            }

            if (strTextBox == "txtparty")
            {
                string brokername = "";
                if (txtparty.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtparty.Text);
                    if (a == false)
                    {
                        btntxtparty_Click(this, new EventArgs());
                    }
                    else
                    {
                        brokername = clsCommon.getString("select Ac_Name_E from  qrymstaccountmaster where Ac_Code=" + txtparty.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (brokername != string.Empty)
                        {
                            lblpartyname.Text = brokername;
                            setFocusControl(drpDeliveryType);
                        }
                        else
                        {
                            txtparty.Text = string.Empty;
                            lblpartyname.Text = brokername;
                            setFocusControl(txtparty);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtparty);
                }
            }

            if (strTextBox == "txtbroker")
            {
                string broker = "";
                if (txtbroker.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtbroker.Text);
                    if (a == false)
                    {
                        btntxtbroker_Click(this, new EventArgs());
                    }
                    else
                    {
                        broker = clsCommon.getString("select Ac_Name_E from  qrymstaccountmaster where Ac_Code=" + txtbroker.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (broker != string.Empty)
                        {
                            lblbrokername.Text = broker;
                            setFocusControl(txtsubbroker);
                        }
                        else
                        {
                            txtbroker.Text = string.Empty;
                            lblbrokername.Text = broker;
                            setFocusControl(txtbroker);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtbroker);
                }
            }

            if (strTextBox == "txtsubbroker")
            {
                string subbrokername = "";
                if (txtsubbroker.Text != string.Empty)
                {
                    bool a = clsCommon.isStringIsNumeric(txtsubbroker.Text);
                    if (a == false)
                    {
                        btntxtsubbroker_Click(this, new EventArgs());
                    }
                    else
                    {
                        subbrokername = clsCommon.getString("select Ac_Name_E from  qrymstaccountmaster where Ac_Code=" + txtparty.Text + "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        if (subbrokername != string.Empty)
                        {
                            lblsubbroker.Text = subbrokername;
                            setFocusControl(txtqntl);
                        }
                        else
                        {
                            txtsubbroker.Text = string.Empty;
                            lblsubbroker.Text = subbrokername;
                            setFocusControl(txtsubbroker);
                        }
                    }
                }
                else
                {
                    setFocusControl(txtsubbroker);
                }
            }
        }
        catch
        {

        }

    }
    #endregion

}