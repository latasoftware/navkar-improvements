
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Web.Services;
using MySql.Data.MySqlClient;
using System.Threading;


public partial class Sugar_Master_PgeTenderHeadUtility : System.Web.UI.Page
{
    MySqlConnection con = null;
    MySqlCommand cmd = null;
    MySqlTransaction myTran = null;
    string cs = string.Empty;
    string qryCommon = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        qryCommon = "qrytenderhead";
        cs = ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
        con = new MySqlConnection(cs);
        if (!IsPostBack)
        {
            BindDummyRow();
        }
        SetFocus(btnAdd);
    }
    private void BindDummyRow()
    {
        DataTable dummy = new DataTable();
        dummy.Columns.Add("Tender_No");
        dummy.Columns.Add("Tender_Date");
        dummy.Columns.Add("millshortname");
        dummy.Columns.Add("Quantal");
        dummy.Columns.Add("Grade");
        dummy.Columns.Add("Mill_Rate");
        dummy.Columns.Add("tenderdoname");
        dummy.Columns.Add("season");


        dummy.Columns.Add("tenderid");
        dummy.Rows.Add();
        gvCustomers.DataSource = dummy;
        gvCustomers.DataBind();
    }
    [WebMethod]
    public static string GetCustomers(string searchTerm, int pageIndex, int PageSize, int Company_Code, int year)
    {
        string searchtxt = "";
        string delimStr = "";
        char[] delimiter = delimStr.ToCharArray();
        string words = "";
        string[] split = null;
        string name = string.Empty;

        searchtxt = searchTerm;
        words = searchTerm;
        split = words.Split(delimiter);
        foreach (var s in split)
        {
            string aa = s.ToString();
            // name += "Doc_No Like '%" + aa + "%'or";
            name += "( millshortname like '%" + aa + "%' or tenderdoname like '%" + aa + "%' or Grade like '%" + aa +
                "%' or Tender_No like '%" + aa + "%' or Tender_DateConverted like '%" + aa + "%' or Quantal like '%" + aa +
                "%' or Mill_Rate like '%" + aa + "%'or tenderid like '%" + aa + "%' or season like '%" + aa + "%') and";


        }
        name = name.Remove(name.Length - 3);

        string query = "SELECT ROW_NUMBER() OVER ( order by Tender_No desc) AS RowNumber,Tender_No,Tender_DateConverted as Tender_Date,"
       + "millshortname ,Quantal,Grade,Mill_Rate,tenderdoname,season,tenderid FROM qrytenderhead   where " + name + " and Company_Code=" + Company_Code + " " +
       " and Year_Code=" + year + " order by Tender_no desc";

        MySqlCommand cmd = new MySqlCommand(query);
        cmd.CommandType = CommandType.Text;
        return GetData(cmd, pageIndex, PageSize).GetXml();

    }
    private static DataSet GetData(MySqlCommand cmd, int pageIndex, int PageSize)
    {

        string RecordCount = "";
        string cs1 = ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;

        using (MySqlConnection con = new MySqlConnection(cs1))
        {
            using (MySqlDataAdapter sda = new MySqlDataAdapter())
            {

                cmd.Connection = con;
                sda.SelectCommand = cmd;
                DataSet dsreturn = new DataSet();
                using (DataSet ds = new DataSet())
                {
                    sda.Fill(ds);
                    int number = 1;
                    DataTable dtnew = new DataTable();
                    dtnew = ds.Tables[0];
                    for (int i = 0; i < dtnew.Rows.Count; i++)
                    {
                        dtnew.Rows[i][0] = number;
                        number = number + 1;

                    }
                    string f1 = " RowNumber >=(" + pageIndex + " -1) * (" + PageSize + "+1) and RowNumber<=";
                    string f2 = "(((" + pageIndex + " -1) * " + PageSize + " +1) +" + PageSize + ")-1";

                    DataRow[] results = ds.Tables[0].Select(f1 + f2, "Tender_No desc");
                    if (results.Count() > 0)
                    {
                        DataTable dt1 = results.CopyToDataTable();
                        dt1.TableName = "Customers";
                        DataTable dt = new DataTable("Pager");
                        dt.Columns.Add("PageIndex");
                        dt.Columns.Add("PageSize");
                        dt.Columns.Add("RecordCount");
                        dt.Rows.Add();
                        RecordCount = ds.Tables[0].Rows.Count.ToString();

                        dt.Rows[0]["PageIndex"] = pageIndex;
                        dt.Rows[0]["PageSize"] = PageSize;
                        dt.Rows[0]["RecordCount"] = RecordCount;

                        dsreturn = new DataSet();
                        dsreturn.Tables.Add(dt1);
                        dsreturn.Tables.Add(dt);
                        return dsreturn;
                    }
                    else
                    {
                        return dsreturn;
                    }

                }
            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {

    }
}