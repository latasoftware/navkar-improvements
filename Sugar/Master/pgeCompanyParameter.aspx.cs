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

public partial class Sugar_Master_pgeCompanyParameter : System.Web.UI.Page
{
    #region data section
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string tblDetails = string.Empty;
    //string AccountMasterTable = string.Empty;
    string qryCommon = string.Empty;
    string cityMasterTable = string.Empty;
    string searchString = string.Empty;
    string strTextBox = string.Empty;
    string qryDisplay = string.Empty;
    int defaultAccountCode = 0;
    string qryAccountList = string.Empty;
    static WebControl objAsp = null;
    string autovoucher = string.Empty;
    string user = string.Empty;
    string qry = string.Empty;
    string isAuthenticate = string.Empty;
    string cs = string.Empty;
    int flag = 0;
    int count = 0;
    int counts = 0;
    MySqlConnection con = null;
    MySqlCommand cmd = null;
    MySqlTransaction myTran = null;
    #endregion

    #region text and label data Declaration
    Int32 Commission_ac = 0;
    Int32 Interest_ac = 0;
    Int32 Transport_ac = 0;
    Int32 Postage_ac = 0;
    Int32 Self_ac = 0;
    Int32 GST_state_code = 0;
    Int32 Sale_cgst = 0;
    Int32 Sale_sgst = 0;
    Int32 Sale_igst = 0;
    Int32 Purchase_cgst = 0;
    Int32 Purchase_sgst = 0;
    Int32 Purchase_igst = 0;
    Int32 Transport_rcm = 0;
    Int32 cgst_rcm = 0;
    Int32 sgst_rcm = 0;
    Int32 igst_rcm = 0;
    string Auto_voucher = string.Empty;
    Int32 Round_off = 0;
    int Freight_Ac = 0;
    #endregion

    #region Head part Declaration
    string Created_By = string.Empty;
    string Modified_By = string.Empty;
    string Created_Date = string.Empty;
    string Modified_Date = string.Empty;
    string retValue = string.Empty;
    string strRev = string.Empty;

    int Company_Code = 0;
    int Year_Code = 0;
    string Head_Update = string.Empty;
    string Head_Insert = string.Empty;
    string Head_Delete = string.Empty;
    string Head_Values = string.Empty;
    string Head_Fields = string.Empty;
    #endregion

    #region [Page Load]
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cs = ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
            con = new MySqlConnection(cs);
            tblPrefix = Session["tblPrefix"].ToString();
            tblHead = tblPrefix + "companyparameters";
            tblDetails = tblPrefix + "";
            qryCommon = "qrymstaccountmaster";
            qryAccountList = tblPrefix + "qryAccountsList";
            pnlPopup.Style["display"] = "none";
            user = Session["user"].ToString();
            if (!Page.IsPostBack)
            {
                isAuthenticate = Security.Authenticate(tblPrefix, user);
                string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
                if (isAuthenticate == "1" || User_Type == "A")
                {
                    pnlPopup.Style["display"] = "none";
                    ViewState["currentTable"] = null;
                    //clsButtonNavigation.enableDisable("N");
                    // this.makeEmptyForm("N");
                    ViewState["mode"] = "I";
                    this.showLastRecord();
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
            setFocusControl(txtcommission_ac);
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
                btnUpdate.Text = "Save";
                lblMsg.Text = string.Empty;
                #region set Business logic for save

                btntxtcommission_ac.Enabled = false;
                btntxtinterest_ac.Enabled = false;
                btntxttransport_ac.Enabled = false;
                btntxtpostage_ac.Enabled = false;
                btntxtself_ac.Enabled = false;
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
                btnUpdate.Text = "Save";

                #region set Business logic for save

                btntxtcommission_ac.Enabled = true;
                btntxtinterest_ac.Enabled = true;
                btntxttransport_ac.Enabled = true;
                btntxtpostage_ac.Enabled = true;
                btntxtself_ac.Enabled = true;
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
                #region set Business logic for save

                btntxtcommission_ac.Enabled = false;
                btntxtinterest_ac.Enabled = false;
                btntxttransport_ac.Enabled = false;
                btntxtpostage_ac.Enabled = false;
                btntxtself_ac.Enabled = false;
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

                lblMsg.Text = string.Empty;
                #region set Business logic for save

                btntxtcommission_ac.Enabled = true;
                btntxtinterest_ac.Enabled = true;
                btntxttransport_ac.Enabled = true;
                btntxtpostage_ac.Enabled = true;
                btntxtself_ac.Enabled = true;
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
            string qry = this.getDisplayQuery();
            this.fetchRecord(qry);
            btnUpdate.Enabled = true;
        }

        catch
        {

        }
    }
    #endregion

    #region [btnUpdate_Click]
    protected void btnUpdate_Click(object sender, EventArgs e)
    {

        try
        {
            bool isvalidate = true;
            string qry = "";
            #region validation
            if (txtGstStateCode.Text != string.Empty && txtGstStateCode.Text != "0")
            {
                isvalidate = true;
            }
            else
            {
                txtGstStateCode.Text = string.Empty;
                setFocusControl(txtGstStateCode);
                return;
            }
            if (txtSGSTAc.Text != string.Empty && txtSGSTAc.Text != "0")
            {
                isvalidate = true;
            }
            else
            {
                txtSGSTAc.Text = string.Empty;
                setFocusControl(txtSGSTAc);
                return;
            }

            if (txtCGSTAc.Text != string.Empty && txtCGSTAc.Text != "0")
            {
                isvalidate = true;
            }
            else
            {
                txtCGSTAc.Text = string.Empty;
                setFocusControl(txtCGSTAc);
                return;
            }
            if (txtIGSTAc.Text != string.Empty && txtIGSTAc.Text != "0")
            {
                isvalidate = true;
            }
            else
            {
                txtIGSTAc.Text = string.Empty;
                setFocusControl(txtIGSTAc);
                return;
            }

            if (txtPurchaseCGSTAc.Text != string.Empty && txtPurchaseCGSTAc.Text != "0")
            {
                isvalidate = true;
            }
            else
            {
                txtPurchaseCGSTAc.Text = string.Empty;
                setFocusControl(txtPurchaseCGSTAc);
                return;
            }

            if (txtPurchaseSGSTAc.Text != string.Empty && txtPurchaseSGSTAc.Text != "0")
            {
                isvalidate = true;
            }
            else
            {
                txtPurchaseSGSTAc.Text = string.Empty;
                setFocusControl(txtPurchaseSGSTAc);
                return;
            }

            if (txtPurchaseIGSTAc.Text != string.Empty && txtPurchaseIGSTAc.Text != "0")
            {
                isvalidate = true;
            }
            else
            {
                txtPurchaseIGSTAc.Text = string.Empty;
                setFocusControl(txtPurchaseIGSTAc);
                return;
            }
            #endregion

            #region -Head part declearation

            Commission_ac = txtcommission_ac.Text != string.Empty ? Convert.ToInt32(txtcommission_ac.Text) : 0;
            Interest_ac = txtinterest_ac.Text != string.Empty ? Convert.ToInt32(txtinterest_ac.Text) : 0;
            Transport_ac = txttransport_ac.Text != string.Empty ? Convert.ToInt32(txttransport_ac.Text) : 0;
            Postage_ac = txtpostage_ac.Text != string.Empty ? Convert.ToInt32(txtpostage_ac.Text) : 0;
            Self_ac = txtself_ac.Text != string.Empty ? Convert.ToInt32(txtself_ac.Text) : 0;
            GST_state_code = txtGstStateCode.Text != string.Empty ? Convert.ToInt32(txtGstStateCode.Text) : 0;

            Sale_cgst = txtCGSTAc.Text != string.Empty ? Convert.ToInt32(txtCGSTAc.Text) : 0;
            Sale_sgst = txtSGSTAc.Text != string.Empty ? Convert.ToInt32(txtSGSTAc.Text) : 0;
            Sale_igst = txtIGSTAc.Text != string.Empty ? Convert.ToInt32(txtIGSTAc.Text) : 0;
            Purchase_cgst = txtPurchaseCGSTAc.Text != string.Empty ? Convert.ToInt32(txtPurchaseCGSTAc.Text) : 0;
            Purchase_sgst = txtPurchaseSGSTAc.Text != string.Empty ? Convert.ToInt32(txtPurchaseSGSTAc.Text) : 0;
            Purchase_igst = txtPurchaseIGSTAc.Text != string.Empty ? Convert.ToInt32(txtPurchaseIGSTAc.Text) : 0;
            Transport_rcm = txtTransport_RCM_GSTRate.Text != string.Empty ? Convert.ToInt32(txtTransport_RCM_GSTRate.Text) : 0;
            cgst_rcm = txtCGST_RCM_Ac.Text != string.Empty ? Convert.ToInt32(txtCGST_RCM_Ac.Text) : 0;
            sgst_rcm = txtSGST_RCM_Ac.Text != string.Empty ? Convert.ToInt32(txtSGST_RCM_Ac.Text) : 0;
            igst_rcm = txtIGST_RCM_Ac.Text != string.Empty ? Convert.ToInt32(txtIGST_RCM_Ac.Text) : 0;
            Round_off = txtRoundOff.Text != string.Empty ? Convert.ToInt32(txtRoundOff.Text) : 0;
            Freight_Ac = txtFreight_Ac.Text != string.Empty ? Convert.ToInt32(txtFreight_Ac.Text) : 0;
            if (chkAutoVoucher.Checked == true)
            {
                Auto_voucher = "YES";
            }
            else
            {
                Auto_voucher = "NO";
            }

            string retValue = string.Empty;
            string strRev = string.Empty;
            int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            int Year_Code = Convert.ToInt32(Session["year"].ToString());
            //int year_Code = Convert.ToInt32(Session["year"].ToString());
            //int Branch_Code = Convert.ToInt32(Session["Branch_Code"].ToString());
            string userinfo = clsGV.userInfo + DateTime.Now.ToString("dd/MM/yyyy:HHmmss");
            string Created_By = Session["user"].ToString();
            string Modified_By = Session["user"].ToString();
            string Head_Fields = string.Empty;
            string Head_Values = string.Empty;
            string Head_Update = string.Empty;
            int flag = 0;
            string op = string.Empty;
            string returnmaxno = string.Empty;
            #endregion-End of Head part declearation
            string checkyear = clsCommon.getString("select Year_Code from nt_1_companyparameters where Year_Code=" + Convert.ToInt32(Session["year"]).ToString());
            if (checkyear == null || checkyear == string.Empty || checkyear == "0")
            {
                Head_Fields = Head_Fields + "COMMISSION_AC,";
                Head_Values = Head_Values + "'" + Commission_ac + "',";
                Head_Fields = Head_Fields + "INTEREST_AC,";
                Head_Values = Head_Values + "'" + Interest_ac + "',";
                Head_Fields = Head_Fields + "TRANSPORT_AC,";
                Head_Values = Head_Values + "'" + Transport_ac + "',";
                Head_Fields = Head_Fields + "POSTAGE_AC,";
                Head_Values = Head_Values + "'" + Postage_ac + "',";
                Head_Fields = Head_Fields + "SELF_AC,";
                Head_Values = Head_Values + "'" + Self_ac + "',";
                Head_Fields = Head_Fields + "GSTStateCode,";
                Head_Values = Head_Values + "'" + GST_state_code + "',";
                Head_Fields = Head_Fields + "CGSTAc,";
                Head_Values = Head_Values + "'" + Sale_cgst + "',";
                Head_Fields = Head_Fields + "SGSTAc,";
                Head_Values = Head_Values + "'" + Sale_sgst + "',";
                Head_Fields = Head_Fields + "IGSTAc,";
                Head_Values = Head_Values + "'" + Sale_igst + "',";
                Head_Fields = Head_Fields + "PurchaseCGSTAc,";
                Head_Values = Head_Values + "'" + Purchase_cgst + "',";
                Head_Fields = Head_Fields + "PurchaseSGSTAc,";
                Head_Values = Head_Values + "'" + Purchase_sgst + "',";
                Head_Fields = Head_Fields + "PurchaseIGSTAc,";
                Head_Values = Head_Values + "'" + Purchase_igst + "',";
                Head_Fields = Head_Fields + "Transport_RCM_GSTRate,";
                Head_Values = Head_Values + "'" + Transport_rcm + "',";
                Head_Fields = Head_Fields + "CGST_RCM_Ac,";
                Head_Values = Head_Values + "'" + cgst_rcm + "',";
                Head_Fields = Head_Fields + "SGST_RCM_Ac,";
                Head_Values = Head_Values + "'" + sgst_rcm + "',";
                Head_Fields = Head_Fields + "IGST_RCM_Ac,";
                Head_Values = Head_Values + "'" + igst_rcm + "',";
                Head_Fields = Head_Fields + "AutoVoucher,";
                Head_Values = Head_Values + "'" + Auto_voucher + "',";
                Head_Fields = Head_Fields + "RoundOff,";
                Head_Values = Head_Values + "'" + Round_off + "',";
                Head_Fields = Head_Fields + "Freight_Ac,";
                Head_Values = Head_Values + "'" + Freight_Ac + "',";
                Head_Fields = Head_Fields + "Company_Code,";
                Head_Values = Head_Values + "'" + Company_Code + "',";
                Head_Fields = Head_Fields + "Year_Code,";
                Head_Values = Head_Values + "'" + Year_Code + "',";
                Head_Fields = Head_Fields + "Created_By";
                Head_Values = Head_Values + "'" + Created_By + "'";

                flag = 1;
                qry = "insert into " + tblHead + "(" + Head_Fields + ") values(" + Head_Values + ")";


                Thread thred = new Thread(() => { count = DataStore(qry, flag); }); //Calling DataStore Method Using Thread
                thred.Start(); //Thread Operation Start
                thred.Join();

                if (count == 1)
                {
                    hdnf.Value = counts.ToString();
                    //clsButtonNavigation.enableDisable("S");
                    //this.makeEmptyForm("S");
                    qry = getDisplayQuery();
                    this.fetchRecord(qry);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Added !')", true);
                }
            }
            else
            {
                string s = clsCommon.getString("select Year_Code from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                if (s != string.Empty)
                {
                    #region Create Update Query
                    Head_Update = Head_Update + "COMMISSION_AC=";
                    Head_Update = Head_Update + "'" + Commission_ac + "',";
                    Head_Update = Head_Update + "INTEREST_AC=";
                    Head_Update = Head_Update + "'" + Interest_ac + "',";
                    Head_Update = Head_Update + "TRANSPORT_AC=";
                    Head_Update = Head_Update + "'" + Transport_ac + "',";
                    Head_Update = Head_Update + "POSTAGE_AC=";
                    Head_Update = Head_Update + "'" + Postage_ac + "',";
                    Head_Update = Head_Update + "SELF_AC=";
                    Head_Update = Head_Update + "'" + Self_ac + "',";
                    Head_Update = Head_Update + "GSTStateCode=";
                    Head_Update = Head_Update + "'" + GST_state_code + "',";
                    Head_Update = Head_Update + "CGSTAc=";
                    Head_Update = Head_Update + "'" + Sale_cgst + "',";
                    Head_Update = Head_Update + "SGSTAc=";
                    Head_Update = Head_Update + "'" + Sale_sgst + "',";
                    Head_Update = Head_Update + "IGSTAc=";
                    Head_Update = Head_Update + "'" + Sale_igst + "',";
                    Head_Update = Head_Update + "PurchaseCGSTAc=";
                    Head_Update = Head_Update + "'" + Purchase_cgst + "',";
                    Head_Update = Head_Update + "PurchaseSGSTAc=";
                    Head_Update = Head_Update + "'" + Purchase_sgst + "',";
                    Head_Update = Head_Update + "PurchaseIGSTAc=";
                    Head_Update = Head_Update + "'" + Purchase_igst + "',";
                    Head_Update = Head_Update + "Transport_RCM_GSTRate=";
                    Head_Update = Head_Update + "'" + Transport_rcm + "',";
                    Head_Update = Head_Update + "CGST_RCM_Ac=";
                    Head_Update = Head_Update + "'" + cgst_rcm + "',";
                    Head_Update = Head_Update + "SGST_RCM_Ac=";
                    Head_Update = Head_Update + "'" + sgst_rcm + "',";
                    Head_Update = Head_Update + "IGST_RCM_Ac=";
                    Head_Update = Head_Update + "'" + igst_rcm + "',";
                    Head_Update = Head_Update + "AutoVoucher=";
                    Head_Update = Head_Update + "'" + Auto_voucher + "',";
                    Head_Update = Head_Update + "RoundOff=";
                    Head_Update = Head_Update + "'" + Round_off + "',";
                    Head_Update = Head_Update + "Freight_Ac=";
                    Head_Update = Head_Update + "'" + Freight_Ac + "',";
                    Head_Update = Head_Update + "Modified_By=";
                    Head_Update = Head_Update + "'" + Modified_By + "'";
                    #endregion
                }
                flag = 2;
                qry = "update " + tblHead + " set " + Head_Update + " where Company_Code='" + Company_Code + "' and Year_Code='" + Year_Code + "'";
                Thread thred = new Thread(() => { count = DataStore(qry, flag); });
                thred.Start();
                thred.Join();
                if (count == 2)
                {
                    //hdnf.Value = lblbsid.Text;
                    //clsButtonNavigation.enableDisable("S");
                    //this.makeEmptyForm("S");
                    qry = getDisplayQuery();
                    this.fetchRecord(qry);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Record Successfully Update !')", true);
                }


            }

        }
        catch
        {

        }
    }
    #endregion

    #region DataStore
    private int DataStore(string Query, int flag)
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
                cmd = new MySqlCommand(Query, con, myTran);
                cmd.ExecuteNonQuery();
                //cmd = new MySqlCommand(Detail_Insert, con, myTran);
                //cmd.ExecuteNonQuery();
                myTran.Commit();
                Thread.Sleep(100);

                count = 1;
            }
            else if (flag == 2)
            {
                if (Query != "")
                {
                    cmd = new MySqlCommand(Query, con, myTran);
                    cmd.ExecuteNonQuery();
                }
                myTran.Commit();
                Thread.Sleep(100);
                count = 2;
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

                        txtcommission_ac.Text = dt.Rows[0]["COMMISSION_AC"].ToString();
                        lblCommission_name.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtcommission_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtinterest_ac.Text = dt.Rows[0]["INTEREST_AC"].ToString();
                        lblInterest_name.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtinterest_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                        txttransport_ac.Text = dt.Rows[0]["TRANSPORT_AC"].ToString();
                        lblTransport_name.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txttransport_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtpostage_ac.Text = dt.Rows[0]["POSTAGE_AC"].ToString();
                        lblPostage_name.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtpostage_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtself_ac.Text = dt.Rows[0]["SELF_AC"].ToString();
                        lblSelf_ac.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtself_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtGstStateCode.Text = dt.Rows[0]["GSTStateCode"].ToString();
                        lbltxtGstStateName.Text = clsCommon.getString("select State_Name from GSTStateMaster where State_Code=" + txtGstStateCode.Text + "");

                        txtCGSTAc.Text = dt.Rows[0]["CGSTAc"].ToString();
                        lbltxtCGSTAcName.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtCGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtSGSTAc.Text = dt.Rows[0]["SGSTAc"].ToString();
                        lbltxtSGSTAcName.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtSGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtIGSTAc.Text = dt.Rows[0]["IGSTAc"].ToString();
                        lbltxtIGSTAcName.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtIGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtPurchaseCGSTAc.Text = dt.Rows[0]["PurchaseCGSTAc"].ToString();
                        lbltxtPurchaseCGSTAc.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtPurchaseCGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtPurchaseSGSTAc.Text = dt.Rows[0]["PurchaseSGSTAc"].ToString();
                        lbltxtPurchaseSGSTAc.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtPurchaseSGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtPurchaseIGSTAc.Text = dt.Rows[0]["PurchaseIGSTAc"].ToString();
                        lbltxtPurchaseIGSTAc.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtPurchaseIGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtCGST_RCM_Ac.Text = dt.Rows[0]["CGST_RCM_Ac"].ToString();
                        lblCGST_RCM_Ac.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtCGST_RCM_Ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtSGST_RCM_Ac.Text = dt.Rows[0]["SGST_RCM_Ac"].ToString();
                        lblSGST_RCM_Ac.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtSGST_RCM_Ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtIGST_RCM_Ac.Text = dt.Rows[0]["IGST_RCM_Ac"].ToString();
                        lblIGST_RCM_Ac.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtIGST_RCM_Ac.Text
                            + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtRoundOff.Text = dt.Rows[0]["RoundOff"].ToString();
                        lblRoundOff.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtRoundOff.Text
                            + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtFreight_Ac.Text = dt.Rows[0]["Freight_Ac"].ToString();
                        lblFreight_Ac.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtFreight_Ac.Text
                            + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

                        txtTransport_RCM_GSTRate.Text = dt.Rows[0]["Transport_RCM_GSTRate"].ToString();
                        lblTransport_RCM_GSTRate.Text = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtTransport_RCM_GSTRate.Text
                     + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

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
                        string chk = dt.Rows[0]["AutoVoucher"].ToString();
                        if (chk == "YES")
                        {
                            chkAutoVoucher.Checked = true;
                        }
                        else
                        {
                            chkAutoVoucher.Checked = false;
                        }
                        foreach (System.Web.UI.Control c in pnlMain.Controls)
                        {
                            if (c is System.Web.UI.WebControls.TextBox)
                            {
                                string name = c.ClientID;
                                string newStr = name.Remove(0, 20);
                                newStr = newStr + ".Text";
                                string textname = ((System.Web.UI.WebControls.TextBox)c).Text;
                                string acname = clsCommon.getString("select Ac_Name_E from nt_1_accountmaster where Ac_Code=" + textname + "");
                                if (newStr == "txtcommission_ac")
                                {
                                    lblCommission_name.Text = acname;
                                }
                                if (newStr == "txtinterest_ac")
                                {
                                    lblInterest_name.Text = acname;
                                }
                                if (newStr == "txttransport_ac")
                                {
                                    lblTransport_name.Text = acname;
                                }
                                if (newStr == "txtpostage_ac")
                                {
                                    lblPostage_name.Text = acname;
                                }
                                if (newStr == "txtself_ac")
                                {
                                    lblSelf_ac.Text = acname;
                                }
                                if (newStr == "txtGstStateCode")
                                {
                                    lbltxtGstStateName.Text = acname;
                                }
                                if (newStr == "txtCGSTAc")
                                {
                                    lbltxtCGSTAcName.Text = acname;
                                }
                                if (newStr == "txtSGSTAc")
                                {
                                    lbltxtSGSTAcName.Text = acname;
                                }
                                if (newStr == "txtIGSTAc")
                                {
                                    lbltxtIGSTAcName.Text = acname;
                                }
                                if (newStr == "txtPurchaseCGSTAc")
                                {
                                    lbltxtPurchaseCGSTAc.Text = acname;
                                }
                                if (newStr == "txtPurchaseSGSTAc")
                                {
                                    lbltxtPurchaseSGSTAc.Text = acname;
                                }
                                if (newStr == "txtPurchaseIGSTAc")
                                {
                                    lbltxtPurchaseIGSTAc.Text = acname;
                                }
                                if (newStr == "txtTransport_RCM_GSTRate")
                                {
                                    lblTransport_RCM_GSTRate.Text = acname;
                                }
                                if (newStr == "txtCGST_RCM_Ac")
                                {
                                    lblCGST_RCM_Ac.Text = acname;
                                }
                                if (newStr == "txtSGST_RCM_Ac")
                                {
                                    lblSGST_RCM_Ac.Text = acname;
                                }
                                if (newStr == "txtIGST_RCM_Ac")
                                {
                                    lblIGST_RCM_Ac.Text = acname;
                                }
                                if (newStr == "txtRoundOff")
                                {
                                    lblRoundOff.Text = acname;
                                }

                            }

                        }
                        recordExist = true;
                        lblMsg.Text = "";
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

    protected void txtCGST_RCM_Ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCGST_RCM_Ac.Text;
        strTextBox = "txtCGST_RCM_Ac";
        csCalculations();

    }
    protected void btntxtCGST_RCM_Ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtCGST_RCM_Ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }

    }
    protected void txtSGST_RCM_Ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSGST_RCM_Ac.Text;
        strTextBox = "txtSGST_RCM_Ac";
        csCalculations();

    }
    protected void btntxtSGST_RCM_Ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtSGST_RCM_Ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }

    }
    protected void txtIGST_RCM_Ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtIGST_RCM_Ac.Text;
        strTextBox = "txtIGST_RCM_Ac";
        csCalculations();

    }
    protected void btntxtIGST_RCM_Ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtIGST_RCM_Ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }

    }

    #region getDisplayQuery
    private string getDisplayQuery()
    {
        try
        {
            string qryDisplay = "select * from " + tblHead + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            return qryDisplay;
        }
        catch
        {
            return "";
        }
    }
    #endregion

    #region [grdPopup_RowDataBound]
    protected void grdPopup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string v = hdnfClosePopup.Value;


        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (v != "txtGstStateCode")
            {
                e.Row.Cells[0].Width = new Unit("60px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].Width = new Unit("80px");
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].Width = new Unit("80px");
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            }
            else
            {

                e.Row.Cells[0].Width = new Unit("60px");
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].Width = new Unit("80px");
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
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

    protected void txtRoundOff_TextChanged(object sender, EventArgs e)
    {
        searchString = txtRoundOff.Text;
        strTextBox = "txtRoundOff";
        csCalculations();
    }
    protected void txtbtnRoundOff_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtRoundOff";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }

    #region [txtcommission_ac_TextChanged]
    protected void txtcommission_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtcommission_ac.Text;
        strTextBox = "txtcommission_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtcommission_ac_Click]
    protected void btntxtcommission_ac_Click(object sender, EventArgs e)
    {
        try
        {
            if (hdnfpopup.Value == "0")
            {
                pnlPopup.Style["display"] = "block";
                hdnfClosePopup.Value = "txtcommission_ac";
                btnSearch_Click(sender, e);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region [txtinterest_ac_TextChanged]
    protected void txtinterest_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtinterest_ac.Text;
        strTextBox = "txtinterest_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtinterest_ac_Click]
    protected void btntxtinterest_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtinterest_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txttransport_ac_TextChanged]
    protected void txttransport_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txttransport_ac.Text;
        strTextBox = "txttransport_ac";
        csCalculations();
    }
    #endregion

    #region [btntxttransport_ac_Click]
    protected void btntxttransport_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txttransport_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtpostage_ac_TextChanged]
    protected void txtpostage_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtpostage_ac.Text;
        strTextBox = "txtpostage_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtpostage_ac_Click]
    protected void btntxtpostage_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtpostage_ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtself_ac_TextChanged]
    protected void txtself_ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtself_ac.Text;
        strTextBox = "txtself_ac";
        csCalculations();
    }
    #endregion

    #region [btntxtself_ac_Click]
    protected void btntxtself_ac_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtself_ac";
            btnSearch_Click(sender, e);
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
            if (searchString != string.Empty)
            {
                txtSearchText.Text = searchString;
            }
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

            if (hdnfClosePopup.Value == "txtcommission_ac")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();

                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or cityname like '%" + aa + "%') and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Commission AC--";
                string qry = "select Ac_Code,Ac_Name_E,cityname from " + qryCommon
                    + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + name;
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtFreight_Ac")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();

                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or cityname like '%" + aa + "%') and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Interest AC--";
                string qry = "select Ac_Code,Ac_Name_E,cityname from " + qryCommon
                    + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + name;
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtinterest_ac")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();

                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or cityname like '%" + aa + "%') and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Interest AC--";
                string qry = "select Ac_Code,Ac_Name_E,cityname from " + qryCommon
                    + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + name;
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txttransport_ac")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();

                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or cityname like '%" + aa + "%') and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Transport AC--";
                string qry = "select Ac_Code,Ac_Name_E,cityname from " + qryCommon
                    + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + name;
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtpostage_ac")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();

                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or cityname like '%" + aa + "%') and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Postage AC--";
                string qry = "select Ac_Code,Ac_Name_E,cityname from " + qryCommon
                    + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + name;
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtself_ac")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();

                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or cityname like '%" + aa + "%') and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select Self AC--";
                string qry = "select Ac_Code,Ac_Name_E,cityname from " + qryCommon
                    + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + name;
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtGstStateCode")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();

                    name += "( State_Code like '%" + aa + "%' or State_Name like '%" + aa + "%') or";
                }
                name = name.Remove(name.Length - 2);
                string qry = "Select State_Code,State_Name from GSTStateMaster where " + name;
                this.showPopup(qry);
            }

            if (hdnfClosePopup.Value == "txtCGSTAc" || hdnfClosePopup.Value == "txtSGSTAc" || hdnfClosePopup.Value == "txtIGSTAc"
                || hdnfClosePopup.Value == "txtPurchaseCGSTAc" || hdnfClosePopup.Value == "txtPurchaseSGSTAc" || hdnfClosePopup.Value == "txtPurchaseIGSTAc" || hdnfClosePopup.Value == "txtTransport_RCM_GSTRate"
                || hdnfClosePopup.Value == "txtCGST_RCM_Ac" || hdnfClosePopup.Value == "txtSGST_RCM_Ac" || hdnfClosePopup.Value == "txtIGST_RCM_Ac" || hdnfClosePopup.Value == "txtRoundOff")
            {
                foreach (var s in split)
                {
                    string aa = s.ToString();

                    name += "( Ac_Code like '%" + aa + "%' or Ac_Name_E like '%" + aa + "%' or cityname like '%" + aa + "%') and";
                }
                name = name.Remove(name.Length - 3);
                lblPopupHead.Text = "--Select GST Account--";
                string qry = "select Ac_Code,Ac_Name_E,cityname from " + qryCommon
                    + " where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and " + name;
                this.showPopup(qry);
            }

        }
        catch
        {
        }
    }

    private void branchgrid(string qry)
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
                    grdPopup.DataSource = dt;
                    grdPopup.DataBind();
                }
                else
                {
                    grdPopup.DataSource = null;
                    grdPopup.DataBind();
                }
            }
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

            if (hdnfClosePopup.Value == "txtcommission_ac")
            {
                setFocusControl(txtcommission_ac);
            }

            if (hdnfClosePopup.Value == "txtinterest_ac")
            {
                setFocusControl(txtinterest_ac);
            }
            if (hdnfClosePopup.Value == "txttransport_ac")
            {
                setFocusControl(txttransport_ac);
            }
            if (hdnfClosePopup.Value == "txtpostage_ac")
            {
                setFocusControl(txtpostage_ac);
            }
            if (hdnfClosePopup.Value == "txtself_ac")
            {
                setFocusControl(txtself_ac);
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


    #region csCalculations
    private void csCalculations()
    {
        try
        {
            hdnfpopup.Value = null;


            if (strTextBox == "txtcommission_ac")
            {
                if (txtcommission_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtcommission_ac.Text + "");
                    if (str != string.Empty && str != "0")
                    {
                        lblCommission_name.Text = str;

                        setFocusControl(txtinterest_ac);
                    }
                    else
                    {
                        lblCommission_name.Text = string.Empty;
                        txtcommission_ac.Text = string.Empty;
                        setFocusControl(txtinterest_ac);
                    }
                }
                else
                {
                    lblCommission_name.Text = string.Empty;
                    txtcommission_ac.Text = string.Empty;
                    setFocusControl(txtinterest_ac);
                }
            }

            if (strTextBox == "txtinterest_ac")
            {
                if (txtinterest_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtinterest_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lblInterest_name.Text = str;
                        setFocusControl(txttransport_ac);
                    }
                    else
                    {
                        lblInterest_name.Text = string.Empty;
                        txtinterest_ac.Text = string.Empty;
                        setFocusControl(txttransport_ac);
                    }
                }
                else
                {
                    lblInterest_name.Text = string.Empty;
                    txtinterest_ac.Text = string.Empty;
                    setFocusControl(txttransport_ac);
                }
            }
            if (strTextBox == "txttransport_ac")
            {
                if (txttransport_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txttransport_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lblTransport_name.Text = str;
                        setFocusControl(txtpostage_ac);
                    }
                    else
                    {
                        lblTransport_name.Text = string.Empty;
                        txttransport_ac.Text = string.Empty;
                        setFocusControl(txtpostage_ac);
                    }
                }
                else
                {
                    lblTransport_name.Text = string.Empty;
                    txttransport_ac.Text = string.Empty;
                    setFocusControl(txtpostage_ac);
                }
            }

            if (strTextBox == "txtpostage_ac")
            {
                if (txtpostage_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtpostage_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lblPostage_name.Text = str;
                        setFocusControl(txtself_ac);
                    }
                    else
                    {
                        lblPostage_name.Text = string.Empty;
                        txtpostage_ac.Text = string.Empty;
                        setFocusControl(txtself_ac);
                    }
                }
                else
                {
                    lblPostage_name.Text = string.Empty;
                    txtpostage_ac.Text = string.Empty;
                    setFocusControl(txtself_ac);
                }
            }

            if (strTextBox == "txtself_ac")
            {
                if (txtself_ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtself_ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lblSelf_ac.Text = str;
                        setFocusControl(txtGstStateCode);
                    }
                    else
                    {
                        lblSelf_ac.Text = string.Empty;
                        txtself_ac.Text = string.Empty;
                        setFocusControl(txtGstStateCode);
                    }
                }
                else
                {
                    lblSelf_ac.Text = string.Empty;
                    txtself_ac.Text = string.Empty;
                    setFocusControl(txtGstStateCode);
                }
            }

            if (strTextBox == "txtGstStateCode")
            {
                if (txtGstStateCode.Text != string.Empty)
                {
                    string str = clsCommon.getString("Select State_Name from GSTStateMaster where State_Code=" + txtGstStateCode.Text + "");
                    if (str != string.Empty && str != "0")
                    {
                        lbltxtGstStateName.Text = str;
                        setFocusControl(txtCGSTAc);
                    }
                    else
                    {
                        lbltxtGstStateName.Text = string.Empty;
                        txtGstStateCode.Text = string.Empty;
                        setFocusControl(txtGstStateCode);
                    }
                }
                else
                {
                    lbltxtGstStateName.Text = string.Empty;
                    txtGstStateCode.Text = string.Empty;
                    setFocusControl(txtGstStateCode);
                }
            }
            if (strTextBox == "txtCGSTAc")
            {
                if (txtCGSTAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtCGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lbltxtCGSTAcName.Text = str;
                        setFocusControl(txtSGSTAc);
                    }
                    else
                    {
                        lbltxtCGSTAcName.Text = string.Empty;
                        txtCGSTAc.Text = string.Empty;
                        setFocusControl(txtCGSTAc);
                    }
                }
                else
                {
                    lbltxtCGSTAcName.Text = string.Empty;
                    txtCGSTAc.Text = string.Empty;
                    setFocusControl(txtCGSTAc);
                }
            }
            if (strTextBox == "txtSGSTAc")
            {
                if (txtSGSTAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtSGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lbltxtSGSTAcName.Text = str;
                        setFocusControl(txtIGSTAc);
                    }
                    else
                    {
                        lbltxtSGSTAcName.Text = string.Empty;
                        txtSGSTAc.Text = string.Empty;
                        setFocusControl(txtSGSTAc);
                    }
                }
                else
                {
                    lbltxtSGSTAcName.Text = string.Empty;
                    txtSGSTAc.Text = string.Empty;
                    setFocusControl(txtSGSTAc);
                }
            }
            if (strTextBox == "txtIGSTAc")
            {
                if (txtIGSTAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtIGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lbltxtIGSTAcName.Text = str;
                        setFocusControl(txtPurchaseCGSTAc);
                    }
                    else
                    {
                        lbltxtIGSTAcName.Text = string.Empty;
                        txtIGSTAc.Text = string.Empty;
                        setFocusControl(txtIGSTAc);
                    }
                }
                else
                {
                    lbltxtIGSTAcName.Text = string.Empty;
                    txtIGSTAc.Text = string.Empty;
                    setFocusControl(txtIGSTAc);
                }
            }

            if (strTextBox == "txtPurchaseCGSTAc")
            {
                if (txtPurchaseCGSTAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtPurchaseCGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lbltxtPurchaseCGSTAc.Text = str;
                        setFocusControl(txtPurchaseSGSTAc);
                    }
                    else
                    {
                        lbltxtPurchaseCGSTAc.Text = string.Empty;
                        txtPurchaseCGSTAc.Text = string.Empty;
                        setFocusControl(txtPurchaseCGSTAc);
                    }
                }
                else
                {
                    lbltxtPurchaseCGSTAc.Text = string.Empty;
                    txtPurchaseCGSTAc.Text = string.Empty;
                    setFocusControl(txtPurchaseCGSTAc);
                }
            }

            if (strTextBox == "txtPurchaseSGSTAc")
            {
                if (txtPurchaseSGSTAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtPurchaseSGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lbltxtPurchaseSGSTAc.Text = str;
                        setFocusControl(txtPurchaseIGSTAc);
                    }
                    else
                    {
                        lbltxtPurchaseSGSTAc.Text = string.Empty;
                        txtPurchaseSGSTAc.Text = string.Empty;
                        setFocusControl(txtPurchaseSGSTAc);
                    }
                }
                else
                {
                    lbltxtPurchaseSGSTAc.Text = string.Empty;
                    txtPurchaseSGSTAc.Text = string.Empty;
                    setFocusControl(txtPurchaseSGSTAc);
                }
            }

            if (strTextBox == "txtPurchaseIGSTAc")
            {
                if (txtPurchaseIGSTAc.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtPurchaseIGSTAc.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lbltxtPurchaseIGSTAc.Text = str;
                        setFocusControl(txtTransport_RCM_GSTRate);
                    }
                    else
                    {
                        lbltxtPurchaseIGSTAc.Text = string.Empty;
                        txtPurchaseIGSTAc.Text = string.Empty;
                        setFocusControl(txtPurchaseIGSTAc);
                    }
                }
                else
                {
                    lbltxtPurchaseIGSTAc.Text = string.Empty;
                    txtPurchaseIGSTAc.Text = string.Empty;
                    setFocusControl(txtPurchaseIGSTAc);
                }
            }

            if (strTextBox == "txtTransport_RCM_GSTRate")
            {
                if (txtTransport_RCM_GSTRate.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtTransport_RCM_GSTRate.Text + " and Company_Code="
                        + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lblTransport_RCM_GSTRate.Text = str;
                        setFocusControl(txtCGST_RCM_Ac);
                    }
                    else
                    {
                        lblTransport_RCM_GSTRate.Text = string.Empty;
                        txtTransport_RCM_GSTRate.Text = string.Empty;
                        setFocusControl(txtCGST_RCM_Ac);
                    }
                }
                else
                {
                    lblTransport_RCM_GSTRate.Text = string.Empty;
                    txtTransport_RCM_GSTRate.Text = string.Empty;
                    setFocusControl(txtCGST_RCM_Ac);
                }
            }

            if (strTextBox == "txtCGST_RCM_Ac")
            {
                if (txtCGST_RCM_Ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtCGST_RCM_Ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lblCGST_RCM_Ac.Text = str;
                        setFocusControl(txtSGST_RCM_Ac);
                    }
                    else
                    {
                        lblCGST_RCM_Ac.Text = string.Empty;
                        txtCGST_RCM_Ac.Text = string.Empty;
                        setFocusControl(txtSGST_RCM_Ac);
                    }
                }
                else
                {
                    lblCGST_RCM_Ac.Text = string.Empty;
                    txtCGST_RCM_Ac.Text = string.Empty;
                    setFocusControl(txtSGST_RCM_Ac);
                }
            }

            if (strTextBox == "txtSGST_RCM_Ac")
            {
                if (txtSGST_RCM_Ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtSGST_RCM_Ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lblSGST_RCM_Ac.Text = str;
                        setFocusControl(txtIGST_RCM_Ac);
                    }
                    else
                    {
                        lblSGST_RCM_Ac.Text = string.Empty;
                        txtSGST_RCM_Ac.Text = string.Empty;
                        setFocusControl(txtIGST_RCM_Ac);
                    }
                }
                else
                {
                    lblSGST_RCM_Ac.Text = string.Empty;
                    txtSGST_RCM_Ac.Text = string.Empty;
                    setFocusControl(txtIGST_RCM_Ac);
                }
            }

            if (strTextBox == "txtIGST_RCM_Ac")
            {
                if (txtIGST_RCM_Ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon + " where Ac_Code=" + txtIGST_RCM_Ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lblIGST_RCM_Ac.Text = str;
                        setFocusControl(txtRoundOff);
                    }
                    else
                    {
                        lblIGST_RCM_Ac.Text = string.Empty;
                        txtIGST_RCM_Ac.Text = string.Empty;
                        setFocusControl(txtRoundOff);
                    }
                }
                else
                {
                    lblIGST_RCM_Ac.Text = string.Empty;
                    txtIGST_RCM_Ac.Text = string.Empty;
                    setFocusControl(txtRoundOff);
                }
            }


            if (strTextBox == "txtRoundOff")
            {
                if (txtRoundOff.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon
                        + " where Ac_Code=" + txtRoundOff.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lblRoundOff.Text = str;
                        setFocusControl(txtFreight_Ac);
                    }
                    else
                    {
                        lblRoundOff.Text = string.Empty;
                        txtRoundOff.Text = string.Empty;
                        setFocusControl(txtFreight_Ac);
                    }
                }
                else
                {
                    lblRoundOff.Text = string.Empty;
                    txtRoundOff.Text = string.Empty;
                    setFocusControl(txtFreight_Ac);
                }
            }

            if (strTextBox == "txtFreight_Ac")
            {
                if (txtFreight_Ac.Text != string.Empty)
                {
                    string str = clsCommon.getString("select Ac_Name_E from " + qryCommon
                        + " where Ac_Code=" + txtFreight_Ac.Text + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
                    if (str != string.Empty && str != "0")
                    {
                        lblFreight_Ac.Text = str;
                        setFocusControl(btnUpdate);
                    }
                    else
                    {
                        lblFreight_Ac.Text = string.Empty;
                        txtFreight_Ac.Text = string.Empty;
                        setFocusControl(btnUpdate);
                    }
                }
                else
                {
                    lblFreight_Ac.Text = string.Empty;
                    txtFreight_Ac.Text = string.Empty;
                    setFocusControl(btnUpdate);
                }
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

    #region [txtGstStateCode_TextChanged]
    protected void txtGstStateCode_TextChanged(object sender, EventArgs e)
    {
        searchString = txtGstStateCode.Text;
        strTextBox = "txtGstStateCode";
        csCalculations();
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

    #region [txtCGSTAc_TextChanged]
    protected void txtCGSTAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtCGSTAc.Text;
        strTextBox = "txtCGSTAc";
        csCalculations();
    }
    #endregion

    #region [btntxtCGSTAc_Click]
    protected void btntxtCGSTAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtCGSTAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtSGSTAc_TextChanged]
    protected void txtSGSTAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtSGSTAc.Text;
        strTextBox = "txtSGSTAc";
        csCalculations();
    }
    #endregion

    #region [btntxtSGSTAc_Click]
    protected void btntxtSGSTAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtSGSTAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtIGSTAc_TextChanged]
    protected void txtIGSTAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtIGSTAc.Text;
        strTextBox = "txtIGSTAc";
        csCalculations();
    }
    #endregion

    #region [btntxtIGSTAc_Click]
    protected void btntxtIGSTAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtIGSTAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtPurchaseCGSTAc_TextChanged]
    protected void txtPurchaseCGSTAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPurchaseCGSTAc.Text;
        strTextBox = "txtPurchaseCGSTAc";
        csCalculations();
    }
    #endregion

    #region [btntxtPurchaseCGSTAc_Click]
    protected void btntxtPurchaseCGSTAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPurchaseCGSTAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtPurchaseSGSTAc_TextChanged]
    protected void txtPurchaseSGSTAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPurchaseSGSTAc.Text;
        strTextBox = "txtPurchaseSGSTAc";
        csCalculations();
    }
    #endregion

    #region [btntxtPurchaseSGSTAc_Click]
    protected void btntxtPurchaseSGSTAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPurchaseSGSTAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtPurchaseIGSTAc_TextChanged]
    protected void txtPurchaseIGSTAc_TextChanged(object sender, EventArgs e)
    {
        searchString = txtPurchaseIGSTAc.Text;
        strTextBox = "txtPurchaseIGSTAc";
        csCalculations();
    }
    #endregion

    #region [btntxtPurchaseIGSTAc_Click]
    protected void btntxtPurchaseIGSTAc_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtPurchaseIGSTAc";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion

    #region [txtPurchaseIGSTAc_TextChanged]
    protected void txtTransport_RCM_GSTRate_TextChanged(object sender, EventArgs e)
    {
        searchString = txtTransport_RCM_GSTRate.Text;
        strTextBox = "txtTransport_RCM_GSTRate";
        csCalculations();
    }
    #endregion

    #region [btntxtPurchaseIGSTAc_Click]
    protected void btntxtTransport_RCM_GSTRate_Click(object sender, EventArgs e)
    {
        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtTransport_RCM_GSTRate";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }
    }
    #endregion
    protected void txtFreight_Ac_TextChanged(object sender, EventArgs e)
    {
        searchString = txtFreight_Ac.Text;
        strTextBox = "txtFreight_Ac";
        csCalculations();
    }
    protected void btntxtFreight_Ac_Click(object sender, EventArgs e)
    {

        try
        {
            pnlPopup.Style["display"] = "block";
            hdnfClosePopup.Value = "txtFreight_Ac";
            btnSearch_Click(sender, e);
        }
        catch
        {
        }

    }
}



