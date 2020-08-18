using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class Sugar_pgeCarporateReciept : System.Web.UI.Page
{
    DataTable Maindt = null;
    DataTable SalePurcdt = null;
    DataRow dr = null;
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
    string qryVoucherList = string.Empty;
    int defaultAccountCode = 0;
    string trntype = string.Empty;
    string qryAccountList = string.Empty;
    string GLedgerTable = string.Empty;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    string user = string.Empty;
    static WebControl objAsp = null;
    #endregion

    #region head Declare
    Int32 DOC_NO = 0;
    string DOC_DATE = string.Empty;
    int cmpcashac = 0;
    Int32 cashBank = 0;
    string retValue = string.Empty;
    string strRev = string.Empty;
    int Company_Code = 0;
    int Year_Code = 0;
    int year_Code = 0;
    int Branch_Code = 0;
    string userinfo = string.Empty;
    string drpFilterValue = string.Empty;
    string drcr = string.Empty;
    string drcr0 = string.Empty;
    int? mr_no = 0;
    int? bc = 0;
    int? ac = 0;
    #endregion

    #region detail
    int Detail_Id = 0;
    int Bill_No = 0;
    string BillTran_Type = string.Empty;
    string Bill_Date = string.Empty;
    int PartyCode = 0;
    int UnitCode = 0;
    int MillCode = 0;
    double Quintal = 0.00;
    double BillAmt = 0.00;
    double PaidAmt = 0.00;
    double Balance = 0.00;
    double Amount = 0.00;
    double Adj_Amount = 0.00;
    double grdUsedAmt = 0.00;
    double grdadjustamt = 0.00;
    string Narration = string.Empty;
    string EntryYear = string.Empty;
    string EntryCompany = string.Empty;
    int billAuto_id = 0;

    int mrd_no = 0;
    int pc = 0;
    int mc = 0;
    int uc = 0;
    Int32 vocherNo = 0;
    string voucherType = string.Empty;
    Int32 acCode = 0;
    string lorry = string.Empty;
    // Int32 Unit_Code = 0;

    double EnteredAmt = 0.00;
    double finalAmt = 0.00;
    #endregion
    int flag = 0;

    #region
    string Action = string.Empty;

    string concatid = string.Empty;
    string Head_Insert = string.Empty;
    StringBuilder Head_Update = null;
    string Head_Delete = string.Empty;
    StringBuilder Head_Fields = null;
    StringBuilder Head_Values = null;
    string Purchase_Delete = string.Empty;
    string Sale_Delete = string.Empty;
    string Detail_Insert = string.Empty;
    StringBuilder Detail_Update = null;
    StringBuilder Detail_Delete = null;
    StringBuilder Detail_Fields = null;
    StringBuilder Detail_Values = null;
    string GLEDGER_Insert = string.Empty;
    string GLEDGER_Delete = string.Empty;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = "multiple_receipt_head";
            tblDetails = "multiple_receipt_detail";
            AccountMasterTable = tblPrefix + "AccountMaster";
            qryCommon = "qrymultiplereceipthead";
            qryAccountList = "qrymstaccountmaster";
            cityMasterTable = tblPrefix + "CityMaster";
            systemMasterTable = tblPrefix + "SystemMaster";
            voucherTable = tblPrefix + "Voucher";
            qryVoucherList = tblPrefix + "qryVoucherList";
            user = Session["user"].ToString();
            GLedgerTable = tblPrefix + "GLEDGER";
            pnlPopup.Style["display"] = "none";

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
                    trntype = Request.QueryString["tran_type"];
                    if (Action == "1")
                    {

                        hdnf.Value = Request.QueryString["mr_no"];
                        pnlPopup.Style["display"] = "none";
                        ViewState["currentTable"] = null;
                        clsButtonNavigation.enableDisable("N");
                        this.makeEmptyForm("N");
                        ViewState["mode"] = "I";
                        this.showLastRecord();
                    }
                    else
                    {
                        drpPaymentFor.SelectedValue = trntype;
                        string docno = string.Empty;
                        clsButtonNavigation.enableDisable("A");
                        ViewState["mode"] = null;
                        ViewState["mode"] = "I";
                        this.makeEmptyForm("A");
                        this.nextNumber();
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
                grdDetail.DataSource = null; grdDetail.DataBind();
                pnlPopup.Style["display"] = "none";
                btnSave.Text = "Save";
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                btntxtCashBank.Enabled = false;
                btntxtACCode.Enabled = false;
                btnGetvouchers.Enabled = false;
                lblMsg.Text = string.Empty;
                txtBalance.Enabled = false;
                calenderExtenderDate.Enabled = false;
                //btnDelete.Enabled = true;
                ViewState["currentTable"] = null;
                grdDetail.DataSource = null;
                grdDetail.DataBind();
                pnlgrdDetail.Enabled = false;
                // drpTrnType.Enabled = true;
                drpPaymentFor.Enabled = true;
                lblCashBank.Text = string.Empty;
                txtTotal.Text = string.Empty;
                //btnDelete.Enabled = true;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnAdd.Enabled = true;
                txtBalance.Enabled = false;
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
                pnlgrdDetail.Enabled = true;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                btnAdd.Enabled = false;
                btntxtCashBank.Enabled = true;
                btntxtACCode.Enabled = true;
                btnGetvouchers.Enabled = true;
                // drpTrnType.Enabled = false;
                drpPaymentFor.Enabled = false;
                lblCashBank.Text = string.Empty;
                txtTotal.Text = string.Empty;
                // trntype = drpTrnType.SelectedValue;
                txtBalance.Enabled = false;
                txtCashBank.Enabled = true;
                btntxtCashBank.Enabled = true;
                lblACName.Text = string.Empty;
                //if (trntype == "BP" || trntype == "BR")
                //{
                //    txtCashBank.Enabled = true;
                //    btntxtCashBank.Enabled = true;
                //}
                //else
                //{
                //    txtCashBank.Enabled = false;
                //    btntxtCashBank.Enabled = false;
                //}
                txtdoc_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
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
                btntxtdoc_no.Text = "Choose No";
                btntxtdoc_no.Enabled = false;
                calenderExtenderDate.Enabled = false;
                pnlgrdDetail.Enabled = false;
                btnSave.Enabled = false;
                btnCancel.Enabled = false;
                btnAdd.Enabled = true;
                // drpTrnType.Enabled = true;
                drpPaymentFor.Enabled = true;
                btntxtCashBank.Enabled = false;
                btntxtACCode.Enabled = false;
                btnGetvouchers.Enabled = false;
                txtBalance.Enabled = false;
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
                pnlgrdDetail.Enabled = true;
                txtCashBank.Enabled = true;
                btntxtCashBank.Enabled = true;
                // drpTrnType.Enabled = false;
                drpPaymentFor.Enabled = false;
                txtBalance.Enabled = false;
                btntxtACCode.Enabled = false;
                txtACCode.Enabled = false;
                txtdoc_no.Enabled = false;
                btnGetvouchers.Enabled = true;
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
        string max = clsCommon.getString("select ifnull(max(mr_no),0) as id from multiple_receipt_head where Company_Code='" + Session["Company_Code"].ToString() + "' " +
            " and Year_Code='" + Session["year"].ToString() + "' and tran_type='AB'");
        hdnf.Value = max;
        trntype = "AB";
        drpPaymentFor.Text = trntype;
        clsButtonNavigation.enableDisable("S");
        this.makeEmptyForm("S");
        this.showLastRecord();
    }
    #endregion


    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select * from " + qryCommon + " where mr_no=" + hdnf.Value + " and tran_type='" + trntype + "' ";
            return qryDisplay;
        }
        catch
        {
            return "";
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
                int ID = Convert.ToInt32(dt.Rows[rowIndex]["ID"].ToString());
                string IDExisting = clsCommon.getString("select detail_id from " + tblHead + " where detail_id=" + ID + " and Tran_Type='" + trntype + "' and doc_no=" + txtdoc_no.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (IDExisting != string.Empty)
                {
                    if (action == "Delete")
                    {
                        gridViewRow.Style["background-color"] = "#64BB7F";
                        gridViewRow.ForeColor = System.Drawing.Color.White;
                        grdDetail.Rows[rowIndex].Cells[15].Text = "D";
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "D";            //D=Delete from table
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[15].Text = "N";
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
                        grdDetail.Rows[rowIndex].Cells[15].Text = "R";       //R=Only remove fro grid
                        DataRow dr = dt.Rows[rowIndex];
                        dr["rowAction"] = "R";
                    }
                    if (action == "Open")
                    {
                        gridViewRow.Style["background-color"] = "#fff5ee";
                        gridViewRow.ForeColor = System.Drawing.Color.Gray;
                        grdDetail.Rows[rowIndex].Cells[15].Text = "A";
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
        int i = 0;
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        e.Row.Cells[0].ControlStyle.Width = new Unit("44px");
        e.Row.Cells[1].ControlStyle.Width = new Unit("44px");
        e.Row.Cells[2].ControlStyle.Width = new Unit("80px");
        e.Row.Cells[3].ControlStyle.Width = new Unit("80px");
        e.Row.Cells[4].ControlStyle.Width = new Unit("250px");
        e.Row.Cells[5].ControlStyle.Width = new Unit("80px");
        e.Row.Cells[6].ControlStyle.Width = new Unit("250px");
        e.Row.Cells[7].ControlStyle.Width = new Unit("200px");
        e.Row.Cells[8].ControlStyle.Width = new Unit("150px");
        e.Row.Cells[9].ControlStyle.Width = new Unit("150px");
        e.Row.Cells[10].ControlStyle.Width = new Unit("150px");
        e.Row.Cells[11].ControlStyle.Width = new Unit("150px");
        e.Row.Cells[12].ControlStyle.Width = new Unit("300px");
        e.Row.Cells[13].ControlStyle.Width = new Unit("300px");
        e.Row.Cells[14].ControlStyle.Width = new Unit("300px");

        e.Row.Cells[15].ControlStyle.Width = new Unit("130px");
        e.Row.Cells[16].ControlStyle.Width = new Unit("300px");
        e.Row.Cells[17].ControlStyle.Width = new Unit("130px");
        e.Row.Cells[18].ControlStyle.Width = new Unit("130px");
        e.Row.Cells[19].ControlStyle.Width = new Unit("130px");
        e.Row.Cells[20].ControlStyle.Width = new Unit("130px");

        //e.Row.Cells[18].Visible = false;
        //e.Row.Cells[19].Visible = false;
        //e.Row.Cells[20].Visible = false;
        if (drpPaymentFor.SelectedValue == "TP")
        {
            //e.Row.Cells[6].Visible = false;
            //e.Row.Cells[7].Visible = false;
        }
        e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
        e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;

        foreach (TableCell cell in e.Row.Cells)
        {
            string s = cell.Text.ToString();
            if (cell.Text.Length > 22)
            {
                cell.Text = cell.Text.Substring(0, 22) + "..";
                cell.ToolTip = s;
            }
        }


        //}
        try
        {
            //e.Row.Cells[0].ControlStyle.Width = Unit.Percentage(4);
            //e.Row.Cells[1].ControlStyle.Width = Unit.Percentage(5);
            //e.Row.Cells[2].ControlStyle.Width = Unit.Percentage(7);
            //e.Row.Cells[3].ControlStyle.Width = Unit.Percentage(10);
            //e.Row.Cells[4].ControlStyle.Width = Unit.Percentage(22);
            //e.Row.Cells[5].ControlStyle.Width = Unit.Percentage(10);
            //e.Row.Cells[6].ControlStyle.Width = Unit.Percentage(22);
            //e.Row.Cells[7].ControlStyle.Width = Unit.Percentage(10);
            //e.Row.Cells[8].ControlStyle.Width = Unit.Percentage(7);
            //e.Row.Cells[9].ControlStyle.Width = Unit.Percentage(7);
            //e.Row.Cells[10].ControlStyle.Width = Unit.Percentage(10);
            //e.Row.Cells[11].ControlStyle.Width = Unit.Percentage(10);
            //e.Row.Cells[12].ControlStyle.Width = Unit.Percentage(10);
            //e.Row.Cells[13].ControlStyle.Width = Unit.Percentage(25);
            //e.Row.Cells[14].ControlStyle.Width = Unit.Percentage(25);

            //e.Row.Cells[0].Style["overflow"] = "hidden";
            //e.Row.Cells[1].Style["overflow"] = "hidden";
            //e.Row.Cells[2].Style["overflow"] = "hidden";
            //e.Row.Cells[3].Style["overflow"] = "hidden";
            //e.Row.Cells[4].Style["overflow"] = "hidden";
            //e.Row.Cells[11].Style["overflow"] = "hidden";
            //e.Row.Cells[6].Style["overflow"] = "hidden";
            //e.Row.Cells[7].Style["overflow"] = "hidden";
            //e.Row.Cells[8].Style["overflow"] = "hidden";
            //e.Row.Cells[9].Style["overflow"] = "hidden";
            //e.Row.Cells[10].Style["overflow"] = "hidden";
            //e.Row.Cells[12].Style["overflow"] = "hidden";

            //int i = 0;
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            //    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Left;
            //    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Left;
            //    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
            //    e.Row.Cells[11].HorizontalAlign = HorizontalAlign.Left;

            //    if (e.Row.Cells[13].Text.Length > 27)
            //    {
            //        e.Row.Cells[13].Style["overflow"] = "hidden";
            //        string s = e.Row.Cells[13].Text.ToString();
            //        //e.Row.Cells[11].Text = e.Row.Cells[11].Text.Substring(0, 27) + "..";
            //        e.Row.Cells[13].ToolTip = s;
            //    }
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
        try
        {
            int i = 0;
            string v = hdnfClosePopup.Value;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Width = new Unit("50px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            }
            if (e.Row.RowType != DataControlRowType.Pager)
            {
                if (v == "txtCashBank")
                {
                    e.Row.Cells[0].Width = new Unit("20px");
                    e.Row.Cells[1].Width = new Unit("500px");
                    e.Row.Cells[2].Width = new Unit("100px");
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                }
                if (v == "txtACCode")
                {
                    e.Row.Cells[0].Width = new Unit("20px");
                    e.Row.Cells[1].Width = new Unit("500px");
                    e.Row.Cells[2].Width = new Unit("100px");
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                }
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

    #region [RowCommand]
    protected void grdDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
            int rowindex = row.RowIndex;

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

    #region [btntxtCashBank_Click]
    protected void btntxtCashBank_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfClosePopup.Value == string.Empty)
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtCashBank";
                btnSearch_Click(sender, e);
            }
            else
            {
                hdnfClosePopup.Value = string.Empty;
                setFocusControl(txtCashBank);
            }
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
        #region [Validation Part]
        bool isValidated = true;
        if (txtdoc_no.Text != string.Empty)
        {

            if (ViewState["mode"] != null)
            {
                //if (ViewState["mode"].ToString() == "I")
                //{
                //    trntype = drpTrnType.SelectedValue;

                //    string str = clsCommon.getString("select doc_no from " + tblHead + " where doc_no='" + txtdoc_no.Text + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trntype + "'");
                //    if (str != string.Empty)
                //    {
                //        lblMsg.Text = "Code " + txtdoc_no.Text + " already exist";
                //        this.getMaxCode();
                //        isValidated = true;
                //    }
                //    else
                //    {
                //        isValidated = true;
                //    }
                //}
            }
        }
        else
        {
            isValidated = false;
            setFocusControl(txtdoc_no);
            return;
        }
        trntype = drpPaymentFor.SelectedValue;
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
        // trntype = drpTrnType.SelectedValue;

        if (trntype == "BP" || trntype == "BR")
        {
            if (txtCashBank.Text != string.Empty)
            {
                string str = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_Code=" + txtCashBank.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
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
        if (txtBalance.Text == "0")
        {
            isValidated = true;
        }
        else
        {
            isValidated = false;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Balance Must Be Zero !')", true);
            setFocusControl(btnSave);
            return;
        }
        #endregion


        #region -Head part declearation
        // DOC_NO = txtdoc_no.Text != string.Empty ? Convert.ToInt32(txtdoc_no.Text) : 0;
        DOC_DATE = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
        cmpcashac = Convert.ToInt32(clsCommon.getString("Select Ac_Code from " + qryAccountList + " where Ac_Code=1 and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString())));
        cashBank = txtCashBank.Text != string.Empty ? Convert.ToInt32(txtCashBank.Text) : cmpcashac;
        retValue = string.Empty;
        strRev = string.Empty;
        Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
        Year_Code = Convert.ToInt32(Session["year"].ToString());
        year_Code = Convert.ToInt32(Session["year"].ToString());
        Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
        userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");

        drcr = string.Empty;
        drcr0 = string.Empty;
        if (trntype == "CP" || trntype == "BP")
        {
            drcr = "D";
            drcr0 = "C";
        }
        else
        {
            drcr = "C";
            drcr0 = "D";
        }

        drpFilterValue = string.Empty;
        if (drpPaymentFor.SelectedValue == "AB")
        {
            drpFilterValue = "AB";
        }
        if (drpPaymentFor.SelectedValue == "AS")
        {
            drpFilterValue = "AS";
        }
        if (drpPaymentFor.SelectedValue == "AF")
        {
            drpFilterValue = "AF";
        }
        if (drpPaymentFor.SelectedValue == "TP")
        {
            drpFilterValue = "TP";
        }


        try
        {
            bc = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code=" + txtCashBank.Text + " and Company_code='" + Company_Code + "'"));
        }
        catch
        {
        }
        try
        {
            ac = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code=" + txtACCode.Text + " and Company_code='" + Company_Code + "'"));
        }
        catch
        {
        }
        #endregion-End of Head part declearation
        #region Detail Column
        Detail_Fields.Append("Tran_Type,");
        Detail_Fields.Append("DOC_NO,");
        Detail_Fields.Append("detail_Id,");
        Detail_Fields.Append("Bill_No,");
        Detail_Fields.Append("Bill_Tran_Type,");
        Detail_Fields.Append("Bill_Tran_Date,");
        Detail_Fields.Append("Party_Code,");
        Detail_Fields.Append("Unit_code,");
        Detail_Fields.Append("Mill_Code,");
        Detail_Fields.Append("Quntal,");
        Detail_Fields.Append("Bill_Amount,");
        Detail_Fields.Append("Bill_Receipt,");
        Detail_Fields.Append("Bill_Balance,");
        Detail_Fields.Append("Value,");
        Detail_Fields.Append("Adj_Value,");
        Detail_Fields.Append("Narration,");
        Detail_Fields.Append("Bill_Year_Code,");
        Detail_Fields.Append("Bill_Auto_Id,");
        Detail_Fields.Append("Year_Code,");
        Detail_Fields.Append("Doc_Date,");
        Detail_Fields.Append("mr_no,");
        Detail_Fields.Append("mrd_no,");
        Detail_Fields.Append("pc,");
        Detail_Fields.Append("uc,");
        Detail_Fields.Append("mc,");
        Detail_Fields.Append("bill_comp_code");
        #endregion

        if (btnSave.Text == "Save")
        {
            this.nextNumber();
            #region head Insert
            Head_Fields.Append("DOC_NO,");
            Head_Values.Append("'" + DOC_NO + "',");
            Head_Fields.Append("Tran_Type,");
            Head_Values.Append("'" + drpFilterValue + "',");
            Head_Fields.Append("mr_no,");
            Head_Values.Append("'" + mr_no + "',");

            Head_Fields.Append("Doc_Date,");
            Head_Values.Append("'" + DOC_DATE + "',");
            Head_Fields.Append("Bank_Code,");
            Head_Values.Append("'" + cashBank + "',");
            Head_Fields.Append("Ac_Code,");
            Head_Values.Append("'" + txtACCode.Text + "',");
            Head_Fields.Append("Amount,");
            Head_Values.Append("'" + txtAmount.Text + "',");
            Head_Fields.Append("Company_Code,");
            Head_Values.Append("'" + Company_Code + "',");
            Head_Fields.Append("Year_Code,");
            Head_Values.Append("'" + year_Code + "',");
            Head_Fields.Append("Created_By,");
            Head_Values.Append("'" + user + "',");
            Head_Fields.Append("bc,");
            Head_Values.Append(" case when 0='" + bc + "' then null else '" + bc + "' end,");
            Head_Fields.Append("ac");
            Head_Values.Append(" case when 0='" + ac + "' then null else '" + ac + "' end");


            Head_Insert = "insert into " + tblHead + " (" + Head_Fields + ") values (" + Head_Values + ")";

            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = Head_Insert;
            Maindt.Rows.Add(dr);
            #endregion
            #region Detail


            mrd_no = Convert.ToInt32(clsCommon.getString("select ifnull(max(mrd_no),0) as id from multiple_receipt_detail "));
            if (mrd_no == 0)
            {
                mrd_no = 0;
            }

            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {

                #region Assign Values
                TextBox txtgrdAmount = (TextBox)grdDetail.Rows[i].Cells[14].FindControl("txtgrdAmount");
                Amount = txtgrdAmount.Text != string.Empty ? Convert.ToDouble(txtgrdAmount.Text) : 0.00;
                if (Amount == 0)
                {
                    continue;
                }
                mrd_no = mrd_no + 1;
                Detail_Id = Convert.ToInt32(grdDetail.Rows[i].Cells[0].Text);
                Bill_No = Convert.ToInt32(grdDetail.Rows[i].Cells[1].Text);
                BillTran_Type = grdDetail.Rows[i].Cells[2].Text;

                //if (drpPaymentFor.SelectedValue == "T")
                //{
                //    lorry = Server.HtmlEncode(grdDetail.Rows[i].Cells[5].Text);
                //}
                //else
                //{
                //    Unit_Code = Convert.ToInt32(grdDetail.Rows[i].Cells[5].Text);
                //}
                Bill_Date = grdDetail.Rows[i].Cells[3].Text;
                Bill_Date = DateTime.Parse(Bill_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                PartyCode = Convert.ToInt32(grdDetail.Rows[i].Cells[4].Text);
                if (drpPaymentFor.SelectedValue == "TP")
                {
                    UnitCode = 0;
                }
                else
                {
                    UnitCode = Convert.ToInt32(grdDetail.Rows[i].Cells[6].Text);
                }
                //unit_name = Server.HtmlEncode(grdDetail.Rows[i].Cells[6].Text);
                MillCode = Convert.ToInt32(grdDetail.Rows[i].Cells[8].Text);
                Quintal = Convert.ToDouble(grdDetail.Rows[i].Cells[10].Text);
                BillAmt = Convert.ToDouble(grdDetail.Rows[i].Cells[11].Text);
                PaidAmt = Convert.ToDouble(grdDetail.Rows[i].Cells[12].Text);
                Balance = Convert.ToDouble(grdDetail.Rows[i].Cells[13].Text);
                billAuto_id = Convert.ToInt32(grdDetail.Rows[i].Cells[20].Text);


                TextBox txtgrdAdjustedAmount = (TextBox)grdDetail.Rows[i].Cells[15].FindControl("txtgrdAdjustedAmount");
                TextBox txtNarration = (TextBox)grdDetail.Rows[i].Cells[16].FindControl("txtNarration");
                EntryYear = grdDetail.Rows[i].Cells[17].Text;
                EntryYear = EntryYear.Trim();
                EntryCompany = grdDetail.Rows[i].Cells[19].Text;
                EntryCompany = EntryCompany.Trim();
                //if (EntryYear == "&nbsp;")
                //{
                //    EntryYear = "0";
                //}
                Narration = txtNarration.Text;

                Adj_Amount = txtgrdAdjustedAmount.Text != string.Empty ? Convert.ToDouble(txtgrdAdjustedAmount.Text) : 0.00;


                try
                {
                    pc = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='" + PartyCode + "' and Company_Code='" + Session["Company_Code"].ToString() + "'"));
                }
                catch { }
                try
                {
                    uc = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='" + UnitCode + "' and Company_Code='" + Session["Company_Code"].ToString() + "'"));
                }
                catch { }
                try
                {
                    mc = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='" + MillCode + "' and Company_Code='" + Session["Company_Code"].ToString() + "'"));
                }
                catch { }
                #endregion
                Detail_Values.Append("('" + drpFilterValue + "','" + DOC_NO + "','" + Detail_Id + "','" + Bill_No + "','" + BillTran_Type + "','" + Bill_Date + "','" + PartyCode + "', " +
                " '" + UnitCode + "','" + MillCode + "','" + Quintal + "','" + BillAmt + "','" + PaidAmt + "','" + Balance + "','" + Amount + "','" + Adj_Amount + "', " +
                " '" + Narration + "','" + EntryYear + "','" + billAuto_id + "','" + Year_Code + "','" + DOC_DATE + "','" + mr_no + "','" + mrd_no + "',case when 0='" + pc + "' then null else '" + pc + "' end, " +
                " case when 0='" + uc + "' then null else '" + uc + "' end,case when 0='" + mc + "' then null else '" + mc + "' end,'" + EntryCompany + "'),");
                //if (drpPaymentFor.SelectedValue == "T")
                //{
                //    narration = voucherType + ":" + vocherNo + " Lorry:" + lorry + " Qntl:" + qntl + "-" + rate + " " + txtNarration.Text;
                //}
                //else
                //{
                //    narration = voucherType + ":" + vocherNo + " " + unit_name + " Qntl:" + qntl + " " + txtNarration.Text;
                //}


                grdUsedAmt += Amount;
                grdadjustamt += Adj_Amount;

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
            #region Head Update
            mr_no = Convert.ToInt32(lblmr_no.Text);
            DOC_NO = Convert.ToInt32(txtdoc_no.Text);
            Head_Update.Append("Tran_Type=");
            Head_Update.Append("'" + drpFilterValue + "',");
            Head_Update.Append("Doc_Date=");
            Head_Update.Append("'" + DOC_DATE + "',");
            Head_Update.Append("Bank_Code=");
            Head_Update.Append("'" + cashBank + "',");
            Head_Update.Append("Ac_Code=");
            Head_Update.Append("'" + txtACCode.Text + "',");
            Head_Update.Append("Amount=");
            Head_Update.Append("'" + txtAmount.Text + "',");
            Head_Update.Append("Company_Code=");
            Head_Update.Append("'" + Company_Code + "',");
            Head_Update.Append("Year_Code=");
            Head_Update.Append("'" + year_Code + "',");
            Head_Update.Append("Modified_By=");
            Head_Update.Append("'" + user + "',");
            Head_Update.Append("bc=");
            Head_Update.Append(" case when 0='" + bc + "' then null else '" + bc + "' end,");
            Head_Update.Append("ac=");
            Head_Update.Append(" case when 0='" + ac + "' then null else '" + ac + "' end");

            string Head_Updateqry = "update " + tblHead + " set " + Head_Update + " where mr_no=" + mr_no + "";
            dr = null;
            dr = Maindt.NewRow();
            dr["Querys"] = Head_Updateqry;
            Maindt.Rows.Add(dr);
            #endregion

            #region Detail Update

            string deleter = clsCommon.getString("delete from multiple_receipt_detail where mr_no='" + mr_no + "'");

            mrd_no = Convert.ToInt32(clsCommon.getString("select ifnull(max(mrd_no),0) as id from multiple_receipt_detail "));
            if (mrd_no == 0)
            {
                mrd_no = 0;
            }

            for (int i = 0; i < grdDetail.Rows.Count; i++)
            {
                TextBox txtgrdAmount = (TextBox)grdDetail.Rows[i].Cells[14].FindControl("txtgrdAmount");
                Amount = txtgrdAmount.Text != string.Empty ? Convert.ToDouble(txtgrdAmount.Text) : 0.00;
                if (Amount == 0)
                {
                    continue;
                }
                mrd_no = mrd_no + 1;
                #region Assign Values
                string AutoID = grdDetail.Rows[i].Cells[18].Text;
                Detail_Id = Convert.ToInt32(grdDetail.Rows[i].Cells[0].Text);
                Bill_No = Convert.ToInt32(grdDetail.Rows[i].Cells[1].Text);
                BillTran_Type = grdDetail.Rows[i].Cells[2].Text;

                Bill_Date = grdDetail.Rows[i].Cells[3].Text;
                Bill_Date = DateTime.Parse(Bill_Date, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy/MM/dd");
                PartyCode = Convert.ToInt32(grdDetail.Rows[i].Cells[4].Text);
                if (drpPaymentFor.SelectedValue == "TP")
                {
                    UnitCode = 0;
                }
                else
                {
                    UnitCode = Convert.ToInt32(grdDetail.Rows[i].Cells[6].Text);
                }
                //unit_name = Server.HtmlEncode(grdDetail.Rows[i].Cells[6].Text);
                MillCode = Convert.ToInt32(grdDetail.Rows[i].Cells[8].Text);
                Quintal = Convert.ToDouble(grdDetail.Rows[i].Cells[10].Text);
                BillAmt = Convert.ToDouble(grdDetail.Rows[i].Cells[11].Text);
                PaidAmt = Convert.ToDouble(grdDetail.Rows[i].Cells[12].Text);
                Balance = Convert.ToDouble(grdDetail.Rows[i].Cells[13].Text);

                TextBox txtgrdAdjustedAmount = (TextBox)grdDetail.Rows[i].Cells[15].FindControl("txtgrdAdjustedAmount");
                TextBox txtNarration = (TextBox)grdDetail.Rows[i].Cells[16].FindControl("txtNarration");
                EntryYear = grdDetail.Rows[i].Cells[17].Text;
                EntryYear = EntryYear.Trim();
                EntryCompany = grdDetail.Rows[i].Cells[19].Text;
                EntryCompany = EntryCompany.Trim();
                //if (EntryYear == "&nbsp;")
                //{
                //    EntryYear = "0";
                //}
                Narration = txtNarration.Text;

                Adj_Amount = txtgrdAdjustedAmount.Text != string.Empty ? Convert.ToDouble(txtgrdAdjustedAmount.Text) : 0.00;
                billAuto_id = Convert.ToInt32(grdDetail.Rows[i].Cells[20].Text);

                try
                {
                    pc = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='" + PartyCode + "' and Company_Code='" + Session["Company_Code"].ToString() + "'"));
                }
                catch { }
                try
                {
                    uc = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='" + UnitCode + "' and Company_Code='" + Session["Company_Code"].ToString() + "'"));
                }
                catch { }
                try
                {
                    mc = Convert.ToInt32(clsCommon.getString("select ifnull(accoid,0) as id from qrymstaccountmaster where Ac_Code='" + MillCode + "' and Company_Code='" + Session["Company_Code"].ToString() + "'"));
                }
                catch { }
                #endregion

                Detail_Values.Append("('" + drpFilterValue + "','" + DOC_NO + "','" + Detail_Id + "','" + Bill_No + "','" + BillTran_Type + "','" + Bill_Date + "','" + PartyCode + "', " +
              " '" + UnitCode + "','" + MillCode + "','" + Quintal + "','" + BillAmt + "','" + PaidAmt + "','" + Balance + "','" + Amount + "','" + Adj_Amount + "', " +
              " '" + Narration + "','" + EntryYear + "','" + billAuto_id + "','" + Year_Code + "','" + DOC_DATE + "','" + mr_no + "','" + mrd_no + "',case when 0='" + pc + "' then null else '" + pc + "' end, " +
              " case when 0='" + uc + "' then null else '" + uc + "' end,case when 0='" + mc + "' then null else '" + mc + "' end,'" + EntryCompany + "'),");
                grdUsedAmt += Amount;
                grdadjustamt += Adj_Amount;

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
            flag = 2;
        }

        #region Gledger
        StringBuilder Gledger_values = new StringBuilder();
        GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='" + drpFilterValue + "' and Doc_No=" + DOC_NO + " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

        dr = null;
        dr = Maindt.NewRow();
        dr["Querys"] = GLEDGER_Delete;
        Maindt.Rows.Add(dr);

        StringBuilder Gledger_Column = new StringBuilder();
        Gledger_Column.Append("TRAN_TYPE,CASHCREDIT,DOC_NO,DOC_DATE,AC_CODE,UNIT_code,NARRATION,AMOUNT,TENDER_ID,TENDER_ID_DETAIL,VOUCHER_ID,COMPANY_CODE," +
                     " YEAR_CODE,ORDER_CODE,DRCR,DRCR_HEAD,ADJUSTED_AMOUNT,Branch_Code,SORT_TYPE,SORT_NO,ac,vc,progid,tranid");

        int ordercode = 0;

        if (drpFilterValue == "AB" || drpFilterValue == "AS" || drpFilterValue == "AF")
        {
            drcr = "C";
            drcr0 = "D";
        }
        else
        {
            drcr = "D";
            drcr0 = "C";
        }
        if (txtAmount.Text != "0")
        {
            ordercode = ordercode + 1;
            Gledger_values.Append("('" + drpFilterValue + "','','" + DOC_NO + "','" + DOC_DATE + "','" + cashBank + "','0','','" + txtAmount.Text + "', " +
                                                " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + ordercode + "','" + drcr0 + "','" + txtACCode.Text + "',0,'" + Branch_Code + "','" + drpFilterValue + "','" + DOC_NO + "'," +
                                                " case when 0='" + bc + "' then null else '" + bc + "' end,'0','0','0'),");

            ordercode = ordercode + 1;
            Gledger_values.Append("('" + drpFilterValue + "','','" + DOC_NO + "','" + DOC_DATE + "','" + txtACCode.Text + "','0','','" + txtAmount.Text + "', " +
                                               " null,null,null,'" + Company_Code + "','" + Year_Code + "','" + ordercode + "','" + drcr + "','" + cashBank + "',0,'" + Branch_Code + "','" + drpFilterValue + "','" + DOC_NO + "'," +
                                               " case when 0='" + ac + "' then null else '" + ac + "' end,'0','0','0')");
        }

        GLEDGER_Insert = "insert into nt_1_gledger (" + Gledger_Column + ") values " + Gledger_values + " ";

        dr = null;
        dr = Maindt.NewRow();
        dr["Querys"] = GLEDGER_Insert;
        Maindt.Rows.Add(dr);
        #endregion
        string msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);
        if (msg == "Insert")
        {
            hdnf.Value = mr_no.ToString();
            clsButtonNavigation.enableDisable("S");
            this.makeEmptyForm("S");
            qry = getDisplayQuery();
            this.fetchRecord(qry);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Added !')", true);
        }
        else if (msg == "Update")
        {
            hdnf.Value = mr_no.ToString();
            clsButtonNavigation.enableDisable("S");
            this.makeEmptyForm("S");
            qry = getDisplayQuery();
            this.fetchRecord(qry);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Added !')", true);
        }
    }




    #endregion

    protected void txtgrdAmount_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtgrdAmount";
        int index = RowIndex(sender);
        this.calculation(index);
    }
    protected void txtgrdAdjustedAmount_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtgrdAdjustedAmount";
        int index = RowIndex(sender);
        this.calculation(index);
    }
    protected void txtNarration_TextChanged(object sender, EventArgs e)
    {
        strTextBox = "txtNarration";
        int index = RowIndex(sender);
        // this.calculation(index);
    }

    protected void calculation(int index)
    {
        double Balance = Convert.ToDouble(grdDetail.Rows[index].Cells[13].Text);
        TextBox txtgrdAmount = (TextBox)grdDetail.Rows[index].Cells[14].FindControl("txtgrdAmount");
        TextBox txtgrdAdjustedAmount = (TextBox)grdDetail.Rows[index].Cells[15].FindControl("txtgrdAdjustedAmount");
        TextBox txtNarration = (TextBox)grdDetail.Rows[index].Cells[16].FindControl("txtNarration");
        double grdamount = txtgrdAmount.Text != string.Empty ? Convert.ToDouble(txtgrdAmount.Text) : 0.00;

        if (strTextBox == "txtgrdAmount")
        {
            double enterAmt = txtAmount.Text != string.Empty ? Convert.ToDouble(txtAmount.Text) : 0.00;
            if (enterAmt > 0)
            {
                if (grdamount > Balance)
                {
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Amount Is Greater Than Voucher Balance !');", true);
                    //setFocusControl(txtgrdAmount);
                    //return;
                }
                else
                {
                    double amtAnkush = 0.00;
                    for (int k = 0; k < grdDetail.Rows.Count; k++)
                    {
                        TextBox txtgrdAmount2 = (TextBox)grdDetail.Rows[k].Cells[14].FindControl("txtgrdAmount");
                        double amt2 = txtgrdAmount2.Text != string.Empty ? Convert.ToDouble(txtgrdAmount2.Text) : 0.00;
                        amtAnkush += amt2;
                    }
                    txtBalance.Text = Convert.ToString(Convert.ToDouble(txtAmount.Text) - amtAnkush);
                    double bal = Convert.ToDouble(txtBalance.Text);
                    txtBalance.Text = bal.ToString();
                    //double amtfcuk = ((Convert.ToDouble(txtAmount.Text) - amtAnkush) + grdamount);
                    //if (grdamount > amtfcuk)
                    //{
                    //    txtBalance.Text = Convert.ToString(((Convert.ToDouble(txtAmount.Text) - amtAnkush) + grdamount));
                    //    double bal = Convert.ToDouble(txtBalance.Text);
                    //    txtBalance.Text = bal.ToString();

                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Entered Amount Is Greater Than Total Balance!');", true);
                    //    txtgrdAmount.Text = "0";
                    //    setFocusControl(txtgrdAmount);
                    //    return;
                    //}
                    //else
                    //{
                    //    txtBalance.Text = Convert.ToString(Convert.ToDouble(txtAmount.Text) - amtAnkush);
                    //    double bal = Convert.ToDouble(txtBalance.Text);
                    //    txtBalance.Text = bal.ToString();
                    //}
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Please Enter Amount!');", true);
                setFocusControl(txtAmount);
                txtgrdAmount.Text = "";
                return;
            }
        }
        if (strTextBox == "txtgrdAmount")
        {
            setFocusControl(txtgrdAdjustedAmount);
        }

        if (strTextBox == "txtgrdAdjustedAmount")
        {
            setFocusControl(txtNarration);
        }

        if (strTextBox == "txtNarration")
        {
            if (index < grdDetail.Rows.Count - 1)
            {
                TextBox txtgrdAmount1 = (TextBox)grdDetail.Rows[index + 1].Cells[12].FindControl("txtgrdAmount");
                setFocusControl(txtgrdAmount1);
            }
            else
            {
                setFocusControl(btnSave);
            }
        }
    }


    private static int RowIndex(object sender)
    {
        GridViewRow grow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = grow.RowIndex;
        return index;
    }



    #region csCalculations
    private void csCalculations()
    {
        try
        {
            if (strTextBox == "txtdoc_no")
            {
                //#region code
                //try
                //{
                //    int n;
                //    bool isNumeric = int.TryParse(txtdoc_no.Text, out n);

                //    if (isNumeric == true)
                //    {
                //        DataSet ds = new DataSet();
                //        DataTable dt = new DataTable();
                //        string txtValue = "";
                //        if (txtdoc_no.Text != string.Empty)
                //        {
                //            txtValue = txtdoc_no.Text;

                //            string qry = "select * from " + tblHead + " where   Doc_No='" + txtValue + "' " +
                //                "  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and tran_type='" + trntype + "'";

                //            ds = clsDAL.SimpleQuery(qry);
                //            if (ds != null)
                //            {
                //                if (ds.Tables.Count > 0)
                //                {
                //                    dt = ds.Tables[0];
                //                    if (dt.Rows.Count > 0)
                //                    {
                //                        //Record Found
                //                        hdnf.Value = dt.Rows[0]["doc_no"].ToString();

                //                        if (ViewState["mode"] != null)
                //                        {
                //                            if (ViewState["mode"].ToString() == "I")
                //                            {
                //                                lblMsg.Text = "** Doc No (" + txtValue + ") Already Exist";
                //                                lblMsg.ForeColor = System.Drawing.Color.Red;
                //                                this.getMaxCode();
                //                                txtdoc_no.Enabled = false;

                //                                btnSave.Enabled = true;   //IMP
                //                                setFocusControl(txtdoc_date);
                //                            }

                //                            if (ViewState["mode"].ToString() == "U")
                //                            {
                //                                //fetch record
                //                                qry = getDisplayQuery();

                //                                bool recordExist = this.fetchRecord(qry);
                //                                if (recordExist == true)
                //                                {

                //                                    txtdoc_no.Enabled = false;

                //                                }
                //                            }
                //                        }
                //                    }
                //                    else   //Record Not Found
                //                    {
                //                        if (ViewState["mode"].ToString() == "I")  //Insert Mode
                //                        {
                //                            lblMsg.Text = "";
                //                            setFocusControl(txtdoc_date);
                //                            txtdoc_no.Enabled = false;
                //                            btnSave.Enabled = true;   //IMP
                //                        }
                //                        if (ViewState["mode"].ToString() == "U")
                //                        {
                //                            this.makeEmptyForm("E");
                //                            lblMsg.Text = "** Record Not Found";
                //                            lblMsg.ForeColor = System.Drawing.Color.Red;
                //                            txtdoc_no.Text = string.Empty;
                //                            setFocusControl(txtdoc_no);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        else
                //        {
                //            lblMsg.Text = string.Empty;
                //            setFocusControl(txtdoc_no);
                //        }
                //    }
                //    else
                //    {
                //        this.makeEmptyForm("A");
                //        lblMsg.Text = "Doc No is numeric";
                //        lblMsg.ForeColor = System.Drawing.Color.Red;
                //        clsButtonNavigation.enableDisable("E");
                //        txtdoc_no.Text = string.Empty;
                //        setFocusControl(txtdoc_no);
                //    }
                //}
                //catch
                //{

                //}
                //#endregion
            }
            if (strTextBox == "txtdoc_date")
            {
                if (txtdoc_date.Text != string.Empty)
                {
                    string dt = DateTime.Parse(txtdoc_date.Text, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                    if (clsCommon.isValidDate(dt) == true)
                    {
                        setFocusControl(txtACCode);
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
                        string str = "";
                        if (drpPaymentFor.SelectedValue == "T")
                        {
                            str = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_type='T' and Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        }
                        else
                        {
                            str = clsCommon.getString("select Ac_Name_E from " + qryAccountList + " where Ac_type!='B' and Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        }

                        if (str != string.Empty)
                        {
                            lblACName.Text = str;
                            setFocusControl(txtAmount);
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
                        if (str != string.Empty)
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
            #region Calculation Part
            double total = 0.00;
            if (grdDetail.Rows.Count > 0)
            {

                for (int i = 0; i < grdDetail.Rows.Count; i++)
                {
                    double Amt = Convert.ToDouble(grdDetail.Rows[i].Cells[11].Text);
                    total = total + Amt;
                }
            }
            txtTotal.Text = Math.Round(total, 2).ToString();
            #endregion

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
            //if (searchString != string.Empty)// && strTextBox == hdnfClosePopup.Value)
            //{
            //    txtSearchText.Text = searchString;
            //}
            //else
            //{
            //    txtSearchText.Text = txtSearchText.Text;
            //}

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
            if (hdnfClosePopup.Value == "txtdoc_no")
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
                foreach (var s in split)
                {
                    string aa = s.ToString();
                    // name += " Sub_Group_Code Like '%" + aa + "%'or Sub_Group_Name Like '%" + aa + "%'or HSN_Code Like '%" + aa + "%'or Main_Group_Code Like '%" + aa + "%'or Remark Like '%" + aa + "%'or";
                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or CityName like '%" + aa + "%' ) and";

                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select AC Code--";
                string qry = "";
                if (drpPaymentFor.SelectedValue == "TP")
                {
                    qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Ac_type='T' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                        " " + name + "";
                }
                else
                {
                    qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                        " " + name + "";
                }
                this.showPopup(qry);
            }
            if (hdnfClosePopup.Value == "txtUnit_Code")
            {
                if (txtACCode.Text != string.Empty)
                {
                    string iscarporate = clsCommon.getString("select carporate_party from " + qryAccountList + " where Ac_Code=" + txtACCode.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
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
                        string qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and ( Ac_Code like '%" + txtSearchText.Text + "%' or Ac_Name_E like '%" + txtSearchText.Text + "%' or CityName like '%" + txtSearchText.Text + "%') order by Ac_Name_E";
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

                foreach (var s in split)
                {
                    string aa = s.ToString();
                    // name += " Sub_Group_Code Like '%" + aa + "%'or Sub_Group_Name Like '%" + aa + "%'or HSN_Code Like '%" + aa + "%'or Main_Group_Code Like '%" + aa + "%'or Remark Like '%" + aa + "%'or";
                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or CityName like '%" + aa + "%' ) and";

                }
                name = name.Remove(name.Length - 3);
                //if (drpTrnType.SelectedValue == "CP")
                //{
                qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                " " + name + "";
                //}
                //else
                //{
                //    qry = "select Ac_Code,Ac_Name_E,CityName from " + qryAccountList + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and" +
                //       " " + name + "";
                //}
                this.showPopup(qry);
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
        //this.showLastRecord();
    }
    protected void grdPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPopup.PageIndex = e.NewPageIndex;
        this.btnSearch_Click(sender, e);
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
    #region [btnAddNew Click]
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("A");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("A");
        trntype = drpPaymentFor.SelectedValue;
        this.nextNumber();
    }
    #endregion

    protected void txtUnit_Code_TextChanged(object sender, EventArgs e)
    {
        //searchString = txtUnit_Code.Text;
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
    protected void btnGetvouchers_Click(object sender, EventArgs e)
    {
        try
        {
            bool isValidated = true;
            if (txtACCode.Text != string.Empty)
            {
                isValidated = true;
            }
            else
            {
                isValidated = false;
                setFocusControl(txtACCode);
                return;
            }
            if (drpPaymentFor.SelectedValue == "AB")
            {
                qry = "select row_number() over ( order by billno) Detail_Id,billno as doc_no,bill_tran_type as tran_type,doc_dateConverted as Doc_date,Ac_Code as Party_Code,billtoname as PartyName, " +
                    " Unit_Code,shiptoname as Unit_Name,mill_code as Mill_Code,millname as Mill_Short,NETQNTL,Bill_Amount,received as Paid_Amount, " +
                    " balance as Balance,'' as Amount,'' as Adjusted_Amount,'' as Narration,Year_Code as EntryYearCode,'' as AutoID,Company_Code as BillCompany " +
                    " ,saleid from qrysalebillbalance where Ac_Code='" + txtACCode.Text + "' and balance!=0";

                //qry = "select do.doc_no as doc_no,do.tran_type as Tran_Type,' ' as Suffix,Convert(varchar(10),do.doc_date,103) as Doc_Date,do.truck_no as Unit_Code,GetPassName as Unit_Name,do.millName as Mill_Short,do.transport as Party_Code,do.TransportName as PartyName,do.quantal as NETQNTL,do.Memo_Advance as Bill_Amount," +
                //         " (Select (do.vasuli_amount+ISNULL(SUM(amount)+SUM(Adjusted_Amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and EntryYearCode=do.Year_Code ) as Paid_Amount,((do.Memo_Advance)-" +
                //         "(Select (do.vasuli_amount+ISNULL(SUM(amount)+SUM(Adjusted_Amount),0)) as UA from " + tblPrefix + "Transact where Voucher_No=do.doc_no and Voucher_Type=do.tran_type and credit_ac=do.transport and Company_Code=do.company_code and EntryYearCode=do.Year_Code)) as Balance,do.Year_Code as EntryYearCode " +
                //         " from " + tblPrefix + "qryDeliveryOrderListReport do where tran_type='DO' and (do.Memo_Advance-do.vasuli_amount)!=0 and do.transport=" + txtACCode.Text + " and do.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "";
            }
            else if (drpPaymentFor.SelectedValue == "TP")
            {

                qry = "select row_number() over ( order by doc_no) Detail_Id,doc_no as doc_no,tran_type ,doc_dateConverted as Doc_date,transport as Party_Code,transportname as PartyName, " +
                   " '' as Unit_Code,'' as Unit_Name,mill_code as Mill_Code,'' as Mill_Short,quantal as NETQNTL,Memo_Advance as Bill_Amount,Paid as Paid_Amount, " +
                   " Balance as Balance,'' as Amount,'' as Adjusted_Amount,'' as Narration,Year_Code as EntryYearCode,'' as AutoID,company_code as  BillCompany " +
                   " ,doid as saleid from qrydofreightbalance where transport='" + txtACCode.Text + "' and balance!=0";


                //qry = "select v.Doc_No as doc_no,v.Tran_Type,Convert(varchar(10),v.doc_date,103) as Doc_Date,v.Ac_Code as Party_Code,v.PartyName,v.Unit_Code,v.Unit_Name,v.NETQNTL," +
                //                " a.Short_Name as Mill_Short,v.Sale_Rate,v.Bill_Amount,(Select ISNULL(SUM(amount)+SUM(Adjusted_Amount),0) as UA from " + tblPrefix + "Transact where Voucher_No=v.Doc_No and Voucher_Type=v.Tran_Type " +
                //                " and Company_Code=v.Company_Code and EntryYearCode=v.Year_Code ) as Paid_Amount,(v.Bill_Amount - (select ISNULL(SUM(Bill_Amount),0) as b from " + tblPrefix + "qrySugarPurcListReturn " +
                //                " where PURCNO=v.Doc_No and PurcTranType=v.Tran_type and Company_Code=v.Company_Code and Year_Code=v.Year_Code) - (Select ISNULL(SUM(amount)+SUM(Adjusted_Amount),0) as UA from " + tblPrefix + "Transact where  Voucher_No=v.Doc_No and " +
                //                " Voucher_Type=v.Tran_Type and EntryYearCode=v.Year_Code and Company_Code=v.Company_Code )) as Balance,v.Year_Code as EntryYearCode  from " + tblPrefix + "qryVoucherSaleUnion v left outer join " + tblPrefix + "AccountMaster a on v.mill_code=a.Ac_Code and " +
                //                " v.Company_Code=a.Company_Code where v.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and v.Ac_Code=" + txtACCode.Text + "";
            }
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    grdDetail.DataSource = dt;
                    grdDetail.DataBind();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('No Record Found This Ac Code " + txtACCode.Text + " ')", true);
                }
                //DataTable dt = new DataTable();
                //dt.Columns.Add(new DataColumn("doc_no", typeof(string)));
                //dt.Columns.Add(new DataColumn("Tran_Type", typeof(string)));
                //dt.Columns.Add(new DataColumn("Doc_Date", typeof(string)));
                //dt.Columns.Add(new DataColumn("Party_Code", typeof(string)));
                //dt.Columns.Add(new DataColumn("PartyName", typeof(string)));
                //dt.Columns.Add(new DataColumn("Unit_Code", typeof(string)));
                //dt.Columns.Add(new DataColumn("Unit_Name", typeof(string)));
                //dt.Columns.Add(new DataColumn("NETQNTL", typeof(string)));
                //dt.Columns.Add(new DataColumn("Mill_Short", typeof(string)));
                //dt.Columns.Add(new DataColumn("Bill_Amount", typeof(double)));
                //dt.Columns.Add(new DataColumn("Paid_Amount", typeof(double)));
                //dt.Columns.Add(new DataColumn("Balance", typeof(double)));
                //dt.Columns.Add(new DataColumn("EntryYearCode", typeof(int)));

                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    DataRow dr = dt.NewRow();
                //    dr["doc_no"] = ds.Tables[0].Rows[i]["doc_no"].ToString();
                //    dr["Tran_Type"] = ds.Tables[0].Rows[i]["Tran_Type"].ToString();
                //    dr["Doc_Date"] = ds.Tables[0].Rows[i]["Doc_Date"].ToString();
                //    dr["Party_Code"] = ds.Tables[0].Rows[i]["Party_Code"].ToString();
                //    dr["PartyName"] = ds.Tables[0].Rows[i]["PartyName"].ToString();
                //    dr["Unit_Code"] = ds.Tables[0].Rows[i]["Unit_Code"].ToString();
                //    dr["Unit_Name"] = ds.Tables[0].Rows[i]["Unit_Name"].ToString();
                //    dr["NETQNTL"] = ds.Tables[0].Rows[i]["NETQNTL"].ToString();
                //    dr["Mill_Short"] = ds.Tables[0].Rows[i]["Mill_Short"].ToString();
                //    dr["Bill_Amount"] = ds.Tables[0].Rows[i]["Bill_Amount"].ToString();
                //    dr["Paid_Amount"] = ds.Tables[0].Rows[i]["Paid_Amount"].ToString();
                //    double Balance = Convert.ToDouble(ds.Tables[0].Rows[i]["Balance"].ToString());
                //    dr["Balance"] = Balance;
                //    dr["EntryYearCode"] = ds.Tables[0].Rows[i]["EntryYearCode"].ToString();

                //    if (Balance != 0)
                //    {
                //        dt.Rows.Add(dr);
                //    }
                //}
                //if (dt.Rows.Count > 0)
                //{
                //    grdDetail.DataSource = dt;
                //    grdDetail.DataBind();
                //    if (drpPaymentFor.SelectedValue == "T")
                //    {
                //        grdDetail.HeaderRow.Cells[5].Text = "Lorry";
                //        grdDetail.HeaderRow.Cells[6].Text = "Dispatch_To";
                //    }
                //}
                //else
                //{
                //    grdDetail.DataSource = null;
                //    grdDetail.DataBind();
                //}
            }
            else
            {
                grdDetail.DataSource = null;
                grdDetail.DataBind();
            }
            setFocusControl(btnGetvouchers);
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void txtAmount_TextChanged(object sender, EventArgs e)
    {
        txtBalance.Text = txtAmount.Text;

        double amtAnkush = 0.00;
        for (int k = 0; k < grdDetail.Rows.Count; k++)
        {
            TextBox txtgrdAmount2 = (TextBox)grdDetail.Rows[k].Cells[14].FindControl("txtgrdAmount");
            double amt2 = txtgrdAmount2.Text != string.Empty ? Convert.ToDouble(txtgrdAmount2.Text) : 0.00;
            amtAnkush += amt2;
        }
        txtBalance.Text = Convert.ToString(Convert.ToDouble(txtAmount.Text) - amtAnkush);
        double bal = Convert.ToDouble(txtBalance.Text);
        txtBalance.Text = bal.ToString();
        setFocusControl(btnGetvouchers);
    }
    protected void drpPaymentFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        string paymentFor = drpPaymentFor.SelectedValue;
        string max = clsCommon.getString("select max(mr_no) as id from " + tblHead + " where Tran_Type='" + paymentFor + "' " +
            " and Company_Code=" + Session["Company_Code"].ToString() + " and Year_Code=" + Session["year"].ToString() + "");
        hdnf.Value = max;
        trntype = paymentFor;
        clsButtonNavigation.enableDisable("N");
        this.makeEmptyForm("N");
        ViewState["mode"] = "I";
        this.showLastRecord();

    }
    #region NextNumber
    private void nextNumber()
    {
        try
        {
            DOC_NO = Convert.ToInt32(clsCommon.getString("select ifnull(max(DOC_NO),0) as docno from multiple_receipt_head where Company_Code='" + Session["Company_Code"].ToString() + "' and " +
            " Year_Code=" + Session["year"].ToString() + " and tran_type='" + trntype + "'")) + 1;
            if (DOC_NO == 0)
            {
                DOC_NO = 1;
                txtdoc_no.Text = "1";
            }
            else
            {
                txtdoc_no.Text = DOC_NO.ToString();
            }
            mr_no = Convert.ToInt32(clsCommon.getString("select ifnull(max(mr_no),0) as mr_no from multiple_receipt_head ")) + 1;
            if (mr_no == 0)
            {
                mr_no = 1;
                lblmr_no.Text = "1";
            }
            else
            {
                lblmr_no.Text = mr_no.ToString();
            }
        }
        catch
        {

        }
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
                        lblmr_no.Text = hdnf.Value;
                        drpPaymentFor.Text = dt.Rows[0]["Tran_Type"].ToString();
                        txtdoc_date.Text = dt.Rows[0]["Doc_DateConverted"].ToString();
                        txtACCode.Text = dt.Rows[0]["Ac_Code"].ToString();
                        lblACName.Text = dt.Rows[0]["Accountname"].ToString();
                        txtCashBank.Text = dt.Rows[0]["Bank_Code"].ToString();
                        lblCashBank.Text = dt.Rows[0]["Bankname"].ToString();
                        txtAmount.Text = dt.Rows[0]["Amount"].ToString();

                        qry = "select detail_Id as  Detail_Id,Bill_No as doc_no,bill_tran_type as tran_type," +
" Bill_Tran_Date as Doc_date,Party_Code as Party_Code,partyname as PartyName,  Unit_Code,unitname as Unit_Name," +
" mill_code as Mill_Code,millname as Mill_Short,Quntal as NETQNTL,Bill_Amount,Bill_Receipt as Paid_Amount, " +
" Bill_Balance as Balance,Value as Amount,Adj_Value as Adjusted_Amount,Narration,Bill_Year_Code as EntryYearCode,mrd_no as AutoID, " +
" bill_comp_code as BillCompany,Bill_Auto_Id as saleid from qrymultiplereceiptdetail where mr_no='" + hdnf.Value + "'";

                        DataSet ds1 = clsDAL.SimpleQuery(qry);
                        if (ds1 != null)
                        {
                            DataTable dt1 = ds1.Tables[0];
                            if (dt1.Rows.Count > 0)
                            {
                                double balanceAmt = 0.00;
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    balanceAmt = balanceAmt + Convert.ToDouble(dt1.Rows[0]["Amount"].ToString());
                                }
                                balanceAmt = Convert.ToDouble(txtAmount.Text) - balanceAmt;
                                txtBalance.Text = balanceAmt.ToString();
                                grdDetail.DataSource = dt1;
                                grdDetail.DataBind();
                            }
                        }


                    }
                }
            }
            pnlgrdDetail.Enabled = false;
            return true;
        }
        catch
        {
            return false;
        }
    }
    #endregion
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        clsButtonNavigation.enableDisable("E");
        ViewState["mode"] = null;
        ViewState["mode"] = "I";
        this.makeEmptyForm("E");
        btnSave.Text = "Update";
        btnSave.Enabled = true;
        btnEdit.Enabled = false;
    }

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

            }

        }
        catch
        {
        }
    }
    #endregion
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdconfirm.Value == "Yes")
            {
                string str = string.Empty;
                Head_Delete = "delete from " + tblHead + " where mr_no='" + lblmr_no.Text + "'";

                string Detail_Deleteqry = "delete from " + tblDetails + " where mr_no='" + lblmr_no.Text + "'";

                GLEDGER_Delete = "delete from nt_1_gledger where TRAN_TYPE='" + drpPaymentFor.SelectedValue + "' and Doc_No=" + txtdoc_no.Text + " " +
                    " and COMPANY_CODE=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";

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
                string msg = DOPurcSaleCRUD.DOPurcSaleCRUD_OP(flag, Maindt);
                //Thread thred = new Thread(() => { count = DataStore(flag); }); //Calling DataStore Method Using Thread
                //thred.Start(); //Thread Operation Start
                //thred.Join();

                if (msg == "Delete")
                {
                    Response.Redirect("../Transaction/pgeMultipleReceipt_utility.aspx");
                }
            }
        }
        catch
        {
        }
    }
}