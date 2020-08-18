using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Data;

public partial class Sugar_Report_pgeCreateReturnFilesNew : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnCreateb2b_Click(object sender, EventArgs e)
    {
        string fromdt = txtFromDt.Text;
        string todt = txtToDt.Text;
        fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
        todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
        try
        {
            string qry = "select * from (select Ac_Code,PartyName,LTRIM(RTRIM(PartyGST)) as [GSTIN/UIN of Recipient],ISNULL(NULLIF(PartyStateCode,''),0) as PartyStateCode,CONVERT(NVARCHAR,doc_no) as [Invoice Number],REPLACE(CONVERT(CHAR(11),doc_date, 106),' ','-') as [Invoice date]," +
                         "Bill_Amount as [Invoice Value],(RIGHT('0'+CONVERT(NVARCHAR,PartyStateCode),2) +'-'+ LTRIM(RTRIM(PartyState))) as [Place Of Supply],'N' as [Reverse Charge],'Regular' as [Invoice Type]," +
                         "'' as [E-Commerce GSTIN],5 as Rate,TaxableAmount as [Taxable Value],'' as [Cess Amount],CS_No from NT_1_qrySugarSaleForGSTReturn where doc_date>='2017-07-01' and doc_date between '" + fromdt + "' and '" + todt + "'  and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "" +
                         " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and UnregisterGST=0) as a";


            string qry1 = "select v.Ac_Code,a.Ac_Name_E as PartyName,LTRIM(RTRIM(a.Gst_No)) as [GSTIN/UIN of Recipient],ISNULL(NULLIF(a.GSTStateCode,''),0) as PartyStateCode,'D'+CONVERT(NVARCHAR,v.Doc_No) as [Invoice Number],REPLACE(CONVERT(CHAR(11),v.Doc_Date, 106),' ','-') as [Invoice date]," +
                        " v.Voucher_Amount as [Invoice Value],(RIGHT('0'+CONVERT(NVARCHAR,a.GSTStateCode),2) +'-'+ LTRIM(RTRIM(a.GSTStateName))) as [Place Of Supply],'N' as [Reverse Charge],'Regular' as [Invoice Type],'' as [E-Commerce GSTIN],18 as Rate,v.TaxableAmount as [Taxable Value],'' as [Cess Amount]  from NT_1_Voucher v " +
                        "left outer join NT_1_qryAccountsList a on v.Company_Code=a.Company_Code and v.Ac_Code=a.Ac_Code where a.UnregisterGST=0 and v.Tran_Type='LV' and v.Voucher_Amount>0 and v.Doc_Date between '" + fromdt + "' and '" + todt + "'  and v.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "" +
                         " and v.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + "";


            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);

            DataSet dsDNote = new DataSet();
            dsDNote = clsDAL.SimpleQuery(qry1);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {

                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    if (dsDNote.Tables[0].Rows.Count > 0)
                    {
                        dt.Merge(dsDNote.Tables[0]);
                    }

                    string fileName = "";
                    string strForCSV = "";

                    DataView dvCsNo = new DataView(dt);
                    dvCsNo.RowFilter = "CS_No<>0";

                    DataTable dtFiltered = new DataTable();
                    dtFiltered = dvCsNo.ToTable();
                    foreach (DataRow drRow in dtFiltered.Rows)
                    {
                        string csno = drRow["CS_No"].ToString();
                        string invno = drRow["Invoice Number"].ToString();
                        string BillToCode = clsCommon.getString("Select ISNULL(Bill_To,0) from NT_1_CarporateSale where Doc_No=" + csno + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        if (BillToCode != "0")
                        {
                            string billtoname = clsCommon.getString("Select Ac_Name_E from NT_1_qryAccountsList where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            string Gst_No = clsCommon.getString("Select Gst_No from NT_1_qryAccountsList where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            string placeofsupply = clsCommon.getString("Select (CONVERT(varchar,ISNULL([GSTStateCode],0))+'-'+LTRIM(RTRIM(GSTStateName))) from NT_1_qryAccountsList where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            string statecode = clsCommon.getString("Select GSTStateCode from NT_1_qryAccountsList where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

                            DataRow drForUpdate = dt.Select("CS_No=" + csno + " and [Invoice Number]='" + invno + "'").FirstOrDefault();
                            drForUpdate["Ac_Code"] = BillToCode;
                            drForUpdate["PartyName"] = billtoname;
                            drForUpdate["GSTIN/UIN of Recipient"] = Gst_No;
                            drForUpdate["PartyStateCode"] = statecode;
                            drForUpdate["Place Of Supply"] = placeofsupply;
                        }
                    }


                    DataView dvWrongGst = new DataView(dt);
                    dvWrongGst.RowFilter = "LEN([GSTIN/UIN of Recipient]) <> 15 ";

                    DataView dvWrongState = new DataView(dt);
                    dvWrongState.RowFilter = "PartyStateCode = 0";

                    if (dvWrongGst.ToTable().Rows.Count > 0)
                    {
                        fileName = "WrongGSTNumbers.csv";
                        DataTable dtnew = dvWrongGst.ToTable();

                        int colindex = 3;
                        for (int i = 0; i < dvWrongGst.ToTable().Columns.Count; i++)
                        {
                            dtnew.Columns.RemoveAt(colindex);
                            if (dtnew.Columns.Count < 4)
                            {
                                break;
                            }
                        }
                        dtnew = dtnew.DefaultView.ToTable(true, "Ac_Code", "PartyName");
                        strForCSV = clsCommon.DataTableToCSV(dtnew, ',');
                    }
                    else if (dvWrongState.ToTable().Rows.Count > 0)
                    {
                        fileName = "WrongState.csv";
                        DataTable dtnew = dvWrongState.ToTable();

                        int colindex = 3;
                        for (int i = 0; i < dvWrongState.ToTable().Columns.Count; i++)
                        {
                            dtnew.Columns.RemoveAt(colindex);
                            if (dtnew.Columns.Count < 4)
                            {
                                break;
                            }
                        }
                        dtnew = dtnew.DefaultView.ToTable(true, "Ac_Code", "PartyName");
                        strForCSV = clsCommon.DataTableToCSV(dtnew, ',');
                    }
                    else
                    {
                        fileName = "b2b.csv";
                        dt.Columns.Remove("Ac_Code");
                        dt.Columns.Remove("PartyName");
                        dt.Columns.Remove("PartyStateCode");
                        dt.Columns.Remove("CS_No");
                        strForCSV = clsCommon.DataTableToCSV(dt, ',');
                    }
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                    Response.Charset = "";
                    Response.ContentType = "application/text";
                    Response.Output.Write(strForCSV.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void grdAll_onRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (lblSummary.Text == "Purchase Summary")
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        var firstCell = e.Row.Cells[14];
        //        firstCell.Controls.Clear();
        //        LinkButton lnk = new LinkButton();
        //        lnk.ID = "lnkDO";
        //        lnk.Click += new EventHandler(lnkDO_Click);
        //        lnk.Text = firstCell.Text;
        //        //firstCell.Controls.Add(lnk);
        //        e.Row.Cells[14].Controls.Add(lnk);
        //    }
        //}
    }

    protected void lnkDO_Click(object sender, EventArgs e)
    {

    }

    protected void btnCreatePurchaseBillSummary_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            string qry = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SR_No,doc_no as OurNo,Bill_No as MillInvoiceNo,FromGSTNo,FromStateCode,Convert(varchar(10),doc_date,103) as Date, " +
                         " LORRYNO as Vehicle_No,Quantal as Quintal,rate as Rate,subTotal as TaxableAmount,CGSTAmount as CGST,SGSTAmount as SGST,IGSTAmount as IGST,Bill_Amount as Payable_Amount,PURCNO as DO from NT_1_qrySugarPurchListForReport " +
                         " where doc_date>='2017-07-01' and doc_date between '" + fromdt + "' and '" + todt + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " order by doc_date";

            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    lblSummary.Text = "Purchase Summary";
                    grdAll.DataSource = dt;
                    grdAll.DataBind();

                    double totalQuintal = Convert.ToDouble(dt.Compute("SUM(Quintal)", string.Empty));
                    double totalBagBillAmount = Convert.ToDouble(dt.Compute("SUM(TaxableAmount)", string.Empty));
                    double totalCGST = Convert.ToDouble(dt.Compute("SUM(CGST)", string.Empty));
                    double totalSGST = Convert.ToDouble(dt.Compute("SUM(SGST)", string.Empty));
                    double totalIGST = Convert.ToDouble(dt.Compute("SUM(IGST)", string.Empty));
                    double totalPayable_Amount = Convert.ToDouble(dt.Compute("SUM(Payable_Amount)", string.Empty));

                    grdAll.FooterRow.Cells[7].Text = totalQuintal.ToString();
                    grdAll.FooterRow.Cells[9].Text = totalBagBillAmount.ToString();
                    grdAll.FooterRow.Cells[10].Text = totalCGST.ToString();
                    grdAll.FooterRow.Cells[11].Text = totalSGST.ToString();
                    grdAll.FooterRow.Cells[12].Text = totalIGST.ToString();
                    grdAll.FooterRow.Cells[13].Text = totalPayable_Amount.ToString();

                    grdAll.FooterRow.Cells[7].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[9].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnCreateSaleBillSummary_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            //string qry = "select * from (select s.doc_no as Invoice_No,s.PartyGST as PartyGSTNo,s.Ac_Code as PartyCode,s.PartyName,s.PartyStateCode,CONVERT(varchar,s.doc_date,103) as Invoice_Date,s.LORRYNO as Vehicle_No,s.Quantal as Quintal,s.rate as Rate,s.TaxableAmount, " +
            //    " s.CGSTAmount as CGST,s.SGSTAmount as SGST,s.IGSTAmount as IGST,s.Bill_Amount as Payable_Amount,s.DO_No as DO,s.CS_No from NT_1_qrySugarSaleForGSTReturn s  " +
            //    " where s.doc_date>='2017-07-01' and s.doc_date between '" + fromdt + "' and '" + todt + "' and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ) as a order by a.Invoice_Date";

            //string qry = "select * from (select s.doc_no as Invoice_No,s.PartyGST as PartyGSTNo,s.Ac_Code as PartyCode,s.PartyName,s.PartyStateCode,CONVERT(varchar,s.doc_date,103) as Invoice_Date,s.LORRYNO as Vehicle_No,s.Quantal as Quintal,s.rate as Rate,s.TaxableAmount, " +
            //   " s.CGSTAmount as CGST,s.SGSTAmount as SGST,s.IGSTAmount as IGST,s.Bill_Amount as Payable_Amount,s.DO_No as DO,s.CS_No from NT_1_qrySugarSaleForGSTReturn s  " +
            //   " where s.doc_date>='2017-07-01' and s.doc_date between '" + fromdt + "' and '" + todt + "' and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ) as a order by a.Invoice_Date";

            string qry = " select doc_no as Invoice_No,billtogstno as PartyGSTNo,Ac_Code as PartyCode, billtoname as PartyName,partygststatecode as PartyStateCode,doc_dateConverted as Invoice_Date" +
                ",LORRYNO as Vehicle_No,Quantal as Quintal,salerate as Rate ,TaxableAmount as TaxableAmount,CGSTAmount as CGST,SGSTAmount as SGST,IGSTAmount as IGST,Bill_Amount as Payable_Amount,DONO as DO,Carporate_Sale_No as CS_No" +
                " from qrysalebillsummaryregister where doc_date between'" + fromdt + "' and '" + todt + "' and Company_Code="
                + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) +
                " order by doc_date";

            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    //dt.Columns.Remove("RowNumber");
                    lblSummary.Text = "Sale Summary";

                    DataView dvCsNo = new DataView(dt);
                    dvCsNo.RowFilter = "CS_No<>0";

                    DataTable dtFiltered = new DataTable();
                    dtFiltered = dvCsNo.ToTable();
                    foreach (DataRow drRow in dtFiltered.Rows)
                    {
                        string csno = drRow["CS_No"].ToString();
                        string invno = drRow["Invoice_No"].ToString();
                        string BillToCode = clsCommon.getString("Select IfNULL(bill_to,0) from carporatehead where Doc_No=" + csno + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                        if (BillToCode != "0")
                        {
                            string billtoname = clsCommon.getString("Select Ac_Name_E from qrymstaccountmaster where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            string Gst_No = clsCommon.getString("Select Gst_No from qrymstaccountmaster where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            string placeofsupply = clsCommon.getString("Select (CONVERT(varchar,IfNULL([GSTStateCode],0))+'-'+GSTStateName) from qrymstaccountmaster where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                            string statecode = clsCommon.getString("Select GSTStateCode from qrymstaccountmaster where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

                            DataRow drForUpdate = dt.Select("CS_No=" + csno + " and Invoice_No='" + invno + "'").FirstOrDefault();
                            drForUpdate["PartyCode"] = Convert.ToInt32(BillToCode);
                            drForUpdate["PartyName"] = billtoname;
                            drForUpdate["PartyGSTNo"] = Gst_No;
                            drForUpdate["PartyStateCode"] = statecode;
                        }
                    }

                    grdAll.DataSource = dt;
                    grdAll.DataBind();

                    double totalQuintal = Convert.ToDouble(dt.Compute("SUM(Quintal)", string.Empty));
                    double totalBagBillAmount = Convert.ToDouble(dt.Compute("SUM(TaxableAmount)", string.Empty));
                    double totalCGST = Convert.ToDouble(dt.Compute("SUM(CGST)", string.Empty));
                    double totalSGST = Convert.ToDouble(dt.Compute("SUM(SGST)", string.Empty));
                    double totalIGST = Convert.ToDouble(dt.Compute("SUM(IGST)", string.Empty));
                    double totalPayable_Amount = Convert.ToDouble(dt.Compute("SUM(Payable_Amount)", string.Empty));

                    grdAll.FooterRow.Cells[7].Text = totalQuintal.ToString();
                    grdAll.FooterRow.Cells[9].Text = totalBagBillAmount.ToString();
                    grdAll.FooterRow.Cells[10].Text = totalCGST.ToString();
                    grdAll.FooterRow.Cells[11].Text = totalSGST.ToString();
                    grdAll.FooterRow.Cells[12].Text = totalIGST.ToString();
                    grdAll.FooterRow.Cells[13].Text = totalPayable_Amount.ToString();

                    //grdAll.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.Columns.RemoveAt(15);
                }
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnFrieghtSummary_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            //string qry = "select '' as Challan_No,d.memo_no as Memo_No,CONVERT(varchar(10),d.doc_date,103) as Date,mill_code as Mill_Code,m.Ac_Name_E as MillName,d.MillGSTStateCode as MillStateCode," +
            //            " d.SaleBillTo as Billed_To,p.Ac_Name_E as BillToName,d.SalebilltoGstStateCode as BillToStateCode, d.truck_no as Vehicle_No,d.quantal as Quintal,d.MM_Rate as Rate,d.Memo_Advance as Amount " +
            //            " from NT_1_deliveryorder d left outer join NT_1_AccountMaster m on d.mill_code=m.Ac_Code and d.company_code=m.Company_Code " +
            //            " left outer join NT_1_AccountMaster p on d.SaleBillTo=p.Ac_Code and d.company_code=p.Company_Code where d.doc_date>='2017-07-01' and d.doc_date between '" + fromdt + "' and '" + todt + "' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and MM_Rate!=0 and memo_no!=0 order by d.doc_date";



            string qry = "select '' as Challan_No,d.memo_no as Memo_No,CONVERT(varchar(10),d.doc_date,103) as Date,mill_code as Mill_Code,m.Ac_Name_E as MillName,d.MillGSTStateCode as MillStateCode," +
                        " d.SaleBillTo as Billed_To,p.Ac_Name_E as BillToName,d.SalebilltoGstStateCode as BillToStateCode, d.truck_no as Vehicle_No,dbo.NT_1_AccountMaster.Ac_Name_E " +
                        "AS Transportname,d.TransportGSTStateCode as Transportstatecode,d.quantal as Quintal,d.MM_Rate as Rate,d.Memo_Advance as Amount " +
                        " from NT_1_deliveryorder d left outer join dbo.NT_1_AccountMaster ON d.transport = dbo.NT_1_AccountMaster.Ac_Code AND d.company_code = dbo.NT_1_AccountMaster.Company_Code LEFT OUTER JOIN NT_1_AccountMaster m on d.mill_code=m.Ac_Code and d.company_code=m.Company_Code " +
                        " left outer join NT_1_AccountMaster p on d.SaleBillTo=p.Ac_Code and d.company_code=p.Company_Code where d.doc_date>='2017-07-01' and d.doc_date between '" + fromdt + "' and '" + todt + "' and d.company_code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and d.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and MM_Rate!=0 and memo_no!=0 order by d.doc_date";

            DataSet ds1 = new DataSet();
            ds1 = clsDAL.SimpleQuery(qry);
            if (ds1 != null)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    DataTable dt1 = new DataTable();
                    dt1 = ds1.Tables[0];

                    dt1.Columns.Add(new DataColumn("CGST", typeof(double)));
                    dt1.Columns.Add(new DataColumn("SGST", typeof(double)));
                    dt1.Columns.Add(new DataColumn("IGST", typeof(double)));
                    dt1.Columns.Add(new DataColumn("FinalAmount", typeof(double)));

                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {

                        double cgstrate = 2.5;
                        double sgstrate = 2.5;
                        double igstrate = 5;

                        double CGSTAmount = 0.0;
                        double SGSTAmount = 0.0;
                        double IGSTAmount = 0.0;

                        int millStateCode = Convert.ToInt32(dt1.Rows[i]["MillStateCode"].ToString());
                        //int partyStateCode = Convert.ToInt32(dt1.Rows[i]["BillToStateCode"].ToString());
                        int transpotStateCode = Convert.ToInt32(dt1.Rows[i]["Transportstatecode"].ToString());
                        double Amount = Convert.ToDouble(dt1.Rows[i]["Amount"].ToString());
                        if (27 == transpotStateCode)
                        {
                            CGSTAmount = Math.Round((Amount * cgstrate / 100), 2);
                            SGSTAmount = Math.Round((Amount * sgstrate / 100), 2);
                        }
                        else
                        {
                            IGSTAmount = Math.Round((Amount * igstrate / 100), 2);
                        }

                        dt1.Rows[i]["CGST"] = CGSTAmount;
                        dt1.Rows[i]["SGST"] = SGSTAmount;
                        dt1.Rows[i]["IGST"] = IGSTAmount;
                        dt1.Rows[i]["FinalAmount"] = Math.Round((Amount + CGSTAmount + SGSTAmount + IGSTAmount), 2);
                    }
                    lblSummary.Text = "Frieght Summary";
                    grdAll.DataSource = dt1;
                    grdAll.DataBind();

                    double totalQuintal = Convert.ToDouble(dt1.Compute("SUM(Quintal)", string.Empty));
                    double totalBagBillAmount = Convert.ToDouble(dt1.Compute("SUM(Amount)", string.Empty));
                    double totalCGST = Convert.ToDouble(dt1.Compute("SUM(CGST)", string.Empty));
                    double totalSGST = Convert.ToDouble(dt1.Compute("SUM(SGST)", string.Empty));
                    double totalIGST = Convert.ToDouble(dt1.Compute("SUM(IGST)", string.Empty));
                    double totalPayable_Amount = Convert.ToDouble(dt1.Compute("SUM(FinalAmount)", string.Empty));

                    grdAll.FooterRow.Cells[10].Text = totalQuintal.ToString();
                    grdAll.FooterRow.Cells[12].Text = totalBagBillAmount.ToString();
                    grdAll.FooterRow.Cells[13].Text = totalCGST.ToString();
                    grdAll.FooterRow.Cells[14].Text = totalSGST.ToString();
                    grdAll.FooterRow.Cells[15].Text = totalIGST.ToString();
                    grdAll.FooterRow.Cells[16].Text = totalPayable_Amount.ToString();

                    grdAll.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[15].HorizontalAlign = HorizontalAlign.Right;
                    grdAll.FooterRow.Cells[16].HorizontalAlign = HorizontalAlign.Right;
                }
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnDebitNoteSummary_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            string qry = "select ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS SR_No,v.Doc_No as DebitNote_No,a.Gst_No as PartyGSTNo,v.Ac_Code as PartyCode,a.Ac_Name_E as PartyName,a.GSTStateCode as PartyStateCode," +
                        " CONVERT(varchar(10),v.Doc_Date,103) as Date,v.Quantal as Quintal,v.Diff_Amount as Rate,v.TaxableAmount,v.CGSTAmount as CGST,v.SGSTAmount as SGST," +
                        " v.IGSTAmount as IGST,v.Voucher_Amount as Final_Amount from NT_1_Voucher v " +
                        " left outer join NT_1_AccountMaster a on v.Ac_Code=a.Ac_Code and v.Company_Code=a.Company_Code" +
                        " where v.Tran_Type='LV' and v.TaxableAmount!=0 and v.doc_date>='2017-07-01' and v.doc_date between '" + fromdt + "' and '" + todt + "' and v.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and v.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and v.Voucher_Amount>0 order by v.Doc_Date";
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    lblSummary.Text = "Debit Note Summary";
                    grdAll.DataSource = dt;
                    grdAll.DataBind();

                    double totalQuintal = Convert.ToDouble(dt.Compute("SUM(Quintal)", string.Empty));
                    double totalBagBillAmount = Convert.ToDouble(dt.Compute("SUM(TaxableAmount)", string.Empty));
                    double totalCGST = Convert.ToDouble(dt.Compute("SUM(CGST)", string.Empty));
                    double totalSGST = Convert.ToDouble(dt.Compute("SUM(SGST)", string.Empty));
                    double totalIGST = Convert.ToDouble(dt.Compute("SUM(IGST)", string.Empty));
                    double totalPayable_Amount = Convert.ToDouble(dt.Compute("SUM(Final_Amount)", string.Empty));

                    grdAll.FooterRow.Cells[7].Text = totalQuintal.ToString();
                    grdAll.FooterRow.Cells[9].Text = totalBagBillAmount.ToString();
                    grdAll.FooterRow.Cells[10].Text = totalCGST.ToString();
                    grdAll.FooterRow.Cells[11].Text = totalSGST.ToString();
                    grdAll.FooterRow.Cells[12].Text = totalIGST.ToString();
                    grdAll.FooterRow.Cells[13].Text = totalPayable_Amount.ToString();

                    //grdAll.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
                }

            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnExportToexcel_Click(object sender, EventArgs e)
    {
        Export(grdAll, lblSummary.Text);
    }

    private void Export(GridView grd, string Name)
    {
        try
        {
            StringBuilder StrHtmlGenerate = new StringBuilder();
            StringBuilder StrExport = new StringBuilder();
            StrExport.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
            StrExport.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div class=Section1>");
            StrExport.Append("<DIV  style='font-size:12px;'>");
            StringWriter sw = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(sw);
            grd.RenderControl(tw);
            string sim = sw.ToString();
            StrExport.Append(sim);
            StrExport.Append("</div></body></html>");
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + Name + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(StrExport.ToString());
            Response.Flush();
            Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();

        }
        catch (Exception e)
        {
            throw;
        }
    }

    protected void btnEwayBill_Click(object sender, EventArgs e)
    {
        try
        {
            //  grdAll.DataBind() = null;
            //grdAll.DataSource = null;
            //grdAll.DataBind();
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            #region[comment]

            #endregion


            string qry = "select doc_no,CONVERT(varchar,doc_date,103) as doc_date,"
                      + " UPPER(BillToName) as BillToName,UPPER(BillToGst) as BillToGst ,UPPER(Ac_Name_E) as ShippTo,UPPER(Address_E) as Address_E," +
                      "UPPER(city_name_e) as city_name_e,(case Pincode when 0 then 999999  else pincode end) as pincode,upper(BillToStateCode) AS BillToStateCode," +
                      "UPPER(State_Name) as State_Name,NETQNTL,TaxableAmount,"
                      + "(convert(varchar,CGSTRate,0) + '+' + convert(varchar,SGSTRate,0)+ '+' + convert(varchar,IGSTRate,0)+'+'+'0'+'+'+'0') as Taxrate,CGSTAmount,"
                      + "SGSTAmount,IGSTAmount,Distance,LORRYNO,UPPER(millname) as millname,upper(milladdress) as milladdress,(case millpincode when 0 then 999999  else millpincode end) as millpincode,millcityname,DO_No,upper(millstatename) as millstatename,convert(varchar,TransDate,103)as TransDate"
                      + " from NT_1_qryNameEwayBill where (Carporate_Sale_No = 0 OR  Carporate_Sale_No IS NULL) and doc_date between '" + fromdt + "' and '" + todt
                      + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            #region[from query]
            string qrynm = clsCommon.getString("select UPPER(Company_Name_E) as Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string comnm = qrynm.ToUpper();
            string gstno = clsCommon.getString("select GST from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string address = clsCommon.getString("select UPPER(Address_E) as Address_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string city = clsCommon.getString("select UPPER(City_E) as City_E  from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string pin = clsCommon.getString("select PIN from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string state = clsCommon.getString("select UPPER(State_E) as State_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string DODate = clsCommon.getString("select doc_date from NT_1_deliveryorder ");
            #endregion

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    #region[colummn add]
                    dt.Columns.Add(new DataColumn("Supply Type", typeof(string)));
                    dt.Columns.Add(new DataColumn("Sub Type", typeof(string)));
                    dt.Columns.Add(new DataColumn("Doc Type", typeof(string)));
                    dt.Columns.Add(new DataColumn("Transaction Type", typeof(string)));

                    dt.Columns.Add(new DataColumn("Product", typeof(string)));
                    dt.Columns.Add(new DataColumn("Description", typeof(string)));
                    dt.Columns.Add(new DataColumn("HSN", typeof(string)));
                    dt.Columns.Add(new DataColumn("Unit", typeof(string)));
                    dt.Columns.Add(new DataColumn("CESS Amount", typeof(string)));
                    dt.Columns.Add(new DataColumn("CESS Non Advol Amount", typeof(string)));
                    dt.Columns.Add(new DataColumn("Other", typeof(string)));
                    dt.Columns.Add(new DataColumn("Total Invoice Value", typeof(string)));

                    dt.Columns.Add(new DataColumn("Trans Mode", typeof(string)));
                    //dt.Columns.Add(new DataColumn("Distance level (Km)", typeof(string)));
                    dt.Columns.Add(new DataColumn("Trans Name", typeof(string)));
                    dt.Columns.Add(new DataColumn("Trans ID", typeof(string)));
                    dt.Columns.Add(new DataColumn("Trans DocNo", typeof(string)));
                    dt.Columns.Add(new DataColumn("Trans Date", typeof(string)));
                    //dt.Columns.Add(new DataColumn("Doc No",typeof(Int32)));
                    dt.Columns.Add(new DataColumn("From_Address2", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_Address1", typeof(string)));
                    // dt.Columns.Add(new DataColumn("To_Address2", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_OtherPartyName", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_GSTIN", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_Place", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_Pin Code", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_State", typeof(string)));
                    dt.Columns.Add(new DataColumn("Dispatch_State", typeof(string)));
                    //dt.Columns.Add(new DataColumn("Ship_To_State", typeof(string)));
                    //dt.Columns.Add(new DataColumn("Ship_To_State", typeof(string)));

                    dt.Columns.Add(new DataColumn("Vehical_Type", typeof(string)));
                    #endregion
                    string vno;
                    string to_ac_name_e;
                    string to_address1;
                    string to_address2;
                    string to_place;
                    string from_partnm;
                    string from_address;
                    string from_city;
                    string do_no = "";

                    string Bill_To;
                    string Ship_To;
                    double taxamount;
                    string taxvalue;
                    double CGST;
                    double SGST;
                    double IGST;
                    double CessAmt = 0.00;
                    double CessNontAdvol = 0.00;
                    double Other = 0.00;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region[default value]
                        dt.Rows[i]["Supply Type"] = "Outward";
                        dt.Rows[i]["Sub Type"] = "Supply";
                        dt.Rows[i]["Doc Type"] = "Tax Invoice";

                        dt.Rows[i]["Product"] = "SUGAR";
                        dt.Rows[i]["Description"] = "SUGAR";
                        dt.Rows[i]["HSN"] = "1701";
                        dt.Rows[i]["Unit"] = "QUINTAL";
                        dt.Rows[i]["CESS Amount"] = "0";
                        dt.Rows[i]["CESS Non Advol Amount"] = "0.00";
                        dt.Rows[i]["Other"] = "0.00";

                        dt.Rows[i]["Trans Mode"] = "Road";
                        //dt.Rows[i]["Distance level (Km)"] = "0";
                        dt.Rows[i]["Trans Name"] = " ";
                        dt.Rows[i]["Trans ID"] = " ";
                        dt.Rows[i]["Trans DocNo"] = " ";
                        dt.Rows[i]["Trans Date"] = dt.Rows[i]["TransDate"].ToString();
                        // dt.Rows[i]["State_Name"] = dt.Rows[i]["BillToStateCode"];

                        #region[replace from adreess]
                        //from_address = address;

                        from_address = dt.Rows[i]["milladdress"].ToString();

                        //to_address1 = to_address1.Replace("-", "");
                        //to_address1 = to_address1.Replace("/", "");
                        //to_address1 = to_address1.Replace("&", "");
                        from_address = from_address.Replace(".", "");
                        //to_address1 = to_address1.Replace("#", "");
                        //to_address1 = to_address1.Replace(",", "");
                        from_address = from_address.Replace("(", "");
                        from_address = from_address.Replace(")", "");
                        from_address = from_address.Replace(":", "");
                        from_address = from_address.Replace("_", "");
                        from_address = from_address.Replace("@", "");
                        from_address = from_address.Replace(";", "");
                        from_address = from_address.Replace("=", "");
                        dt.Rows[i]["From_Address2"] = from_address;
                        dt.Rows[i]["From_Address1"] = dt.Rows[i]["millname"].ToString();
                        #endregion

                        #region[replace from other party name]
                        from_partnm = comnm;
                        from_partnm = from_partnm.Replace("-", "");
                        from_partnm = from_partnm.Replace("/", "");
                        from_partnm = from_partnm.Replace("&", "");
                        from_partnm = from_partnm.Replace(".", "");
                        from_partnm = from_partnm.Replace("#", "");
                        from_partnm = from_partnm.Replace("(", "");
                        from_partnm = from_partnm.Replace(")", "");
                        from_partnm = from_partnm.Replace(":", "");
                        from_partnm = from_partnm.Replace("_", "");
                        from_partnm = from_partnm.Replace("@", "");
                        from_partnm = from_partnm.Replace(";", "");
                        from_partnm = from_partnm.Replace("=", "");
                        dt.Rows[i]["From_OtherPartyName"] = from_partnm;
                        #endregion

                        dt.Rows[i]["From_GSTIN"] = gstno;

                        #region[replace from place]
                        //from_city = city;
                        from_city = dt.Rows[i]["millcityname"].ToString();
                        //to_address1 = to_address1.Replace("-", "");
                        //to_address1 = to_address1.Replace("/", "");
                        //to_address1 = to_address1.Replace("&", "");
                        from_city = from_city.Replace(".", "");
                        //to_address1 = to_address1.Replace("#", "");
                        //to_address1 = to_address1.Replace(",", "");
                        from_city = from_city.Replace("(", "");
                        from_city = from_city.Replace(")", "");
                        from_city = from_city.Replace(":", "");
                        from_city = from_city.Replace("_", "");
                        from_city = from_city.Replace("@", "");
                        from_city = from_city.Replace(";", "");
                        from_city = from_city.Replace("=", "");
                        dt.Rows[i]["From_Place"] = from_city;
                        #endregion

                        //dt.Rows[i]["From_Pin Code"] = pin;
                        dt.Rows[i]["From_Pin Code"] = dt.Rows[i]["millpincode"].ToString();
                        dt.Rows[i]["From_State"] = state;
                        dt.Rows[i]["Dispatch_State"] = dt.Rows[i]["millstatename"].ToString();


                        #endregion

                        Bill_To = dt.Rows[i]["BillToName"].ToString();
                        Ship_To = dt.Rows[i]["ShippTo"].ToString();
                        if (Bill_To != Ship_To)
                        {
                            dt.Rows[i]["Transaction Type"] = "Combination of 2 and 3";

                        }
                        else
                        {
                            dt.Rows[i]["Transaction Type"] = "Bill From-Dispatch From";

                        }

                        taxamount = Convert.ToDouble(dt.Rows[i]["TaxableAmount"].ToString());
                        CGST = Convert.ToDouble(dt.Rows[i]["CGSTAmount"].ToString());
                        SGST = Convert.ToDouble(dt.Rows[i]["SGSTAmount"].ToString());
                        IGST = Convert.ToDouble(dt.Rows[i]["IGSTAmount"].ToString());
                        // taxvalue = (dt.Rows[i]["Taxrate"].ToString());
                        //string TotaInvoice = (taxamount + '+' + taxvalue + '0').ToString();

                        //dt.Rows[i]["Total Invoice Value"] = TotaInvoice;
                        double TotalInvoice = taxamount + CGST + SGST + IGST + CessAmt + CessNontAdvol + Other;

                        dt.Rows[i]["Total Invoice Value"] = TotalInvoice;



                        #region[replace vehicle no]
                        vno = Convert.ToString(dt.Rows[i]["LORRYNO"]);
                        vno = vno.Replace("-", "");
                        vno = vno.Replace("/", "");
                        vno = vno.Replace(" ", "");
                        vno = vno.Replace("&", "");
                        vno = vno.Replace(".", "");
                        vno = vno.Replace("#", "");
                        vno = vno.Replace("(", "");
                        vno = vno.Replace(")", "");
                        vno = vno.Replace(":", "");
                        vno = vno.Replace("_", "");
                        vno = vno.Replace("@", "");
                        vno = vno.Replace(";", "");
                        vno = vno.Replace("=", "");
                        dt.Rows[i]["LORRYNO"] = vno;
                        #endregion

                        #region[replae to party name]
                        to_ac_name_e = Convert.ToString(dt.Rows[i]["BillToName"]);
                        to_ac_name_e = to_ac_name_e.Replace("-", "");
                        to_ac_name_e = to_ac_name_e.Replace("/", "");
                        to_ac_name_e = to_ac_name_e.Replace("&", "");
                        to_ac_name_e = to_ac_name_e.Replace(".", "");
                        to_ac_name_e = to_ac_name_e.Replace("#", "");
                        to_ac_name_e = to_ac_name_e.Replace("(", "");
                        to_ac_name_e = to_ac_name_e.Replace(")", "");
                        to_ac_name_e = to_ac_name_e.Replace(":", "");
                        to_ac_name_e = to_ac_name_e.Replace("_", "");
                        to_ac_name_e = to_ac_name_e.Replace("@", "");
                        to_ac_name_e = to_ac_name_e.Replace(";", "");
                        to_ac_name_e = to_ac_name_e.Replace("=", "");
                        dt.Rows[i]["BillToName"] = to_ac_name_e;
                        #endregion

                        #region[replace to address1]
                        to_address1 = Convert.ToString(dt.Rows[i]["ShippTo"]);
                        to_address1 = to_address1.Replace("-", "");
                        to_address1 = to_address1.Replace("/", "");
                        to_address1 = to_address1.Replace("&", "");
                        to_address1 = to_address1.Replace(".", "");
                        to_address1 = to_address1.Replace("#", "");
                        to_address1 = to_address1.Replace(",", "");
                        to_address1 = to_address1.Replace("(", "");
                        to_address1 = to_address1.Replace(")", "");
                        to_address1 = to_address1.Replace(":", "");
                        to_address1 = to_address1.Replace("_", "");
                        to_address1 = to_address1.Replace("@", "");
                        to_address1 = to_address1.Replace(";", "");
                        to_address1 = to_address1.Replace("=", "");
                        dt.Rows[i]["ShippTo"] = to_address1;
                        //dt.Rows[i]["To_Address2"] = to_address1;
                        #endregion

                        #region[replace to address2]
                        to_address2 = Convert.ToString(dt.Rows[i]["Address_E"]);
                        to_address2 = to_address2.Replace("-", "");
                        to_address1 = to_address1.Replace("/", "");
                        to_address1 = to_address1.Replace("&", "");
                        to_address2 = to_address2.Replace(".", "");
                        to_address1 = to_address1.Replace("#", "");
                        to_address2 = to_address2.Replace(",", "");
                        to_address2 = to_address2.Replace("(", "");
                        to_address2 = to_address2.Replace(")", "");
                        to_address2 = to_address2.Replace(":", "");
                        to_address2 = to_address2.Replace("_", "");
                        to_address2 = to_address2.Replace("@", "");
                        to_address2 = to_address2.Replace(";", "");
                        to_address2 = to_address2.Replace("=", "");
                        to_address2 = to_address2.Replace("*", "");
                        to_address2 = to_address2.Replace(":", "");
                        to_address2 = to_address2.Replace(";", "");
                        dt.Rows[i]["Address_E"] = to_address2;
                        //dt.Rows[i]["To_Address2"] = to_address1;
                        #endregion

                        #region[replace to place]
                        to_place = Convert.ToString(dt.Rows[i]["city_name_e"]);
                        //to_address1 = to_address1.Replace("-", "");
                        //to_address1 = to_address1.Replace("/", "");
                        //to_address1 = to_address1.Replace("&", "");
                        to_place = to_place.Replace(".", "");
                        //to_address1 = to_address1.Replace("#", "");
                        //to_address1 = to_address1.Replace(",", "");
                        to_place = to_place.Replace("(", "");
                        to_place = to_place.Replace(")", "");
                        to_place = to_place.Replace(":", "");
                        to_place = to_place.Replace("_", "");
                        to_place = to_place.Replace("@", "");
                        to_place = to_place.Replace(";", "");
                        to_place = to_place.Replace("=", "");
                        dt.Rows[i]["city_name_e"] = to_place;
                        //  dt.Rows[i]["Ship_To_State"] = dt.Rows[i]["State_Name"];
                        #endregion

                        double to_Distance;

                        #region[replace to Distance]
                        to_Distance = Convert.ToDouble(dt.Rows[i]["Distance"]);


                        dt.Rows[i]["Distance"] = to_Distance;

                        dt.Rows[i]["Vehical_Type"] = "REGULAR";
                        #endregion

                    }

                    lblSummary.Text = "E Way Bill";

                    grdAll.DataSource = dt;

                    #region[sequence]
                    dt.Columns["Supply Type"].SetOrdinal(0);
                    dt.Columns["Sub Type"].SetOrdinal(1);
                    dt.Columns["Doc Type"].SetOrdinal(2);
                    dt.Columns["Transaction Type"].SetOrdinal(5);

                    dt.Columns["From_OtherPartyName"].SetOrdinal(6);
                    dt.Columns["From_GSTIN"].SetOrdinal(7);
                    dt.Columns["From_Address1"].SetOrdinal(8);
                    dt.Columns["From_Address2"].SetOrdinal(9);
                    dt.Columns["From_Place"].SetOrdinal(10);
                    dt.Columns["From_Pin Code"].SetOrdinal(11);
                    dt.Columns["From_State"].SetOrdinal(12);
                    dt.Columns["Dispatch_State"].SetOrdinal(13);
                    dt.Columns["State_Name"].SetOrdinal(21);
                    dt.Columns["Product"].SetOrdinal(22);
                    dt.Columns["Description"].SetOrdinal(23);
                    dt.Columns["HSN"].SetOrdinal(24);
                    dt.Columns["Unit"].SetOrdinal(25);
                    dt.Columns["NETQNTL"].SetOrdinal(26);
                    dt.Columns["CESS Amount"].SetOrdinal(32);
                    dt.Columns["CESS Non Advol Amount"].SetOrdinal(33);
                    dt.Columns["Other"].SetOrdinal(34);
                    dt.Columns["Total Invoice Value"].SetOrdinal(35);
                    dt.Columns["Trans Mode"].SetOrdinal(36);
                    // dt.Columns["Distance level (Km)"].SetOrdinal(31);
                    dt.Columns["Trans Name"].SetOrdinal(38);
                    dt.Columns["Trans ID"].SetOrdinal(39);
                    dt.Columns["Trans DocNo"].SetOrdinal(40);
                    dt.Columns["Trans Date"].SetOrdinal(41);
                    dt.Columns["Vehical_Type"].SetOrdinal(43);
                    dt.Columns["DO_No"].SetOrdinal(44);
                    dt.Columns["millname"].SetOrdinal(45);

                    #endregion

                    grdAll.DataBind();

                    #region[header name]
                    grdAll.HeaderRow.Cells[3].Text = "Doc No";
                    grdAll.HeaderRow.Cells[4].Text = "Doc Date";
                    // grdAll.HeaderRow.Cells[5].Text = "From_OtherPartyName";
                    //  grdAll.HeaderRow.Cells[6].Text = "From_GSTIN";
                    //grdAll.HeaderRow.Cells[7].Text = "From_Address1";
                    //grdAll.HeaderRow.Cells[9].Text = "From_Place";
                    //grdAll.HeaderRow.Cells[10].Text = "From_Pin Code";
                    grdAll.HeaderRow.Cells[13].Text = "Dispatch State";
                    grdAll.HeaderRow.Cells[14].Text = "To_OtherPartyName";
                    grdAll.HeaderRow.Cells[15].Text = "To_GSTIN";
                    grdAll.HeaderRow.Cells[16].Text = "To_Address1";
                    grdAll.HeaderRow.Cells[17].Text = "To_Address2";
                    grdAll.HeaderRow.Cells[18].Text = "To_Place";
                    grdAll.HeaderRow.Cells[19].Text = "To_Pin Code";
                    grdAll.HeaderRow.Cells[20].Text = "To_State";
                    grdAll.HeaderRow.Cells[21].Text = "Ship To State";
                    grdAll.HeaderRow.Cells[26].Text = "Qty";
                    grdAll.HeaderRow.Cells[27].Text = "Assessable Value";
                    grdAll.HeaderRow.Cells[28].Text = "Tax Rate(C+S+I+Cess+CESS Non Advol Amount)";
                    grdAll.HeaderRow.Cells[29].Text = "CGST Amount";
                    grdAll.HeaderRow.Cells[30].Text = "SGST Amount";
                    grdAll.HeaderRow.Cells[31].Text = "IGST Amount";
                    grdAll.HeaderRow.Cells[37].Text = "Distance level (Km)";
                    grdAll.HeaderRow.Cells[42].Text = "Vehicel No";


                    #endregion


                    foreach (GridViewRow gvr in grdAll.Rows)
                    {
                        grdAll.HeaderRow.Cells[45].Visible = true;
                        grdAll.HeaderRow.Cells[46].Visible = false;
                        grdAll.HeaderRow.Cells[47].Visible = false;
                        grdAll.HeaderRow.Cells[48].Visible = false;
                        grdAll.HeaderRow.Cells[49].Visible = false;
                        grdAll.HeaderRow.Cells[50].Visible = false;
                        //grdAll.HeaderRow.Cells[45].Visible = false;
                        //grdAll.HeaderRow.Cells[46].Visible = false;
                        //grdAll.HeaderRow.Cells[47].Visible = false;

                        gvr.Cells[45].Visible = true;
                        gvr.Cells[46].Visible = false;
                        gvr.Cells[47].Visible = false;
                        gvr.Cells[48].Visible = false;
                        gvr.Cells[49].Visible = false;
                        gvr.Cells[50].Visible = false;

                    }

                    //  e.Row.Cells[7].ControlStyle.Width = new Unit("60px");
                    //  grdAll.HeaderRow.Cells[4].Width = new Unit("10px");
                    // grdAll.HeaderRow.Cells[4].ControlStyle.Width = new Unit("10px");

                }
            }

        }
        catch
        {
        }
    }

    protected void btnEmpty_E_way_Bill_Click(object sender, EventArgs e)
    {
        try
        {
            grdAll.DataSource = null;
            grdAll.DataBind();

            //grdAll.DataBind();
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            #region[comment]

            #endregion


            string qry = "select doc_no,CONVERT(varchar,doc_date,103) as doc_date,"
                      + " UPPER(BillToName) as BillToName,UPPER(BillToGst) as BillToGst ,UPPER(Ac_Name_E) as ShippTo,UPPER(Address_E) as Address_E,UPPER(city_name_e) as city_name_e,(case Pincode when 0 then 999999  else pincode end) as pincode,upper(BillToStateCode) AS BillToStateCode,UPPER(State_Name) as State_Name,NETQNTL,TaxableAmount,"
                      + "(convert(varchar,CGSTRate,0) + '+' + convert(varchar,SGSTRate,0)+ '+' + convert(varchar,IGSTRate,0)+'+'+'0'+'+'+'0') as Taxrate,CGSTAmount,"
                      + "SGSTAmount,IGSTAmount,Distance,LORRYNO,UPPER(millname) as millname,upper(milladdress) as milladdress,(case millpincode when 0 then 999999  else millpincode end) as millpincode,millcityname,DO_No,upper(millstatename) as millstatename,convert(varchar,TransDate,103)as TransDate"
                      + " from NT_1_qryNameEwayBill where doc_date between '" + fromdt + "' and '" + todt
                      + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and (Eway_Bill_No is null or Eway_Bill_No=0 )";
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            #region[from query]
            string qrynm = clsCommon.getString("select UPPER(Company_Name_E) as Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string comnm = qrynm.ToUpper();
            string gstno = clsCommon.getString("select GST from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string address = clsCommon.getString("select UPPER(Address_E) as Address_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string city = clsCommon.getString("select UPPER(City_E) as City_E  from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string pin = clsCommon.getString("select PIN from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string state = clsCommon.getString("select UPPER(State_E) as State_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            #endregion

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    #region[colummn add]
                    dt.Columns.Add(new DataColumn("Supply Type", typeof(string)));
                    dt.Columns.Add(new DataColumn("Sub Type", typeof(string)));
                    dt.Columns.Add(new DataColumn("Doc Type", typeof(string)));
                    dt.Columns.Add(new DataColumn("Transaction Type", typeof(string)));

                    dt.Columns.Add(new DataColumn("Product", typeof(string)));
                    dt.Columns.Add(new DataColumn("Description", typeof(string)));
                    dt.Columns.Add(new DataColumn("HSN", typeof(string)));
                    dt.Columns.Add(new DataColumn("Unit", typeof(string)));
                    dt.Columns.Add(new DataColumn("CESS Amount", typeof(string)));
                    dt.Columns.Add(new DataColumn("CESS Non Advol Amount", typeof(string)));
                    dt.Columns.Add(new DataColumn("Other", typeof(string)));
                    dt.Columns.Add(new DataColumn("Total Invoice Value", typeof(string)));

                    dt.Columns.Add(new DataColumn("Trans Mode", typeof(string)));
                    //dt.Columns.Add(new DataColumn("Distance level (Km)", typeof(string)));
                    dt.Columns.Add(new DataColumn("Trans Name", typeof(string)));
                    dt.Columns.Add(new DataColumn("Trans ID", typeof(string)));
                    dt.Columns.Add(new DataColumn("Trans DocNo", typeof(string)));
                    dt.Columns.Add(new DataColumn("Trans Date", typeof(string)));
                    //dt.Columns.Add(new DataColumn("Doc No",typeof(Int32)));
                    dt.Columns.Add(new DataColumn("From_Address2", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_Address1", typeof(string)));
                    // dt.Columns.Add(new DataColumn("To_Address2", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_OtherPartyName", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_GSTIN", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_Place", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_Pin Code", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_State", typeof(string)));
                    dt.Columns.Add(new DataColumn("Dispatch_State", typeof(string)));
                    //dt.Columns.Add(new DataColumn("Ship_To_State", typeof(string)));
                    //dt.Columns.Add(new DataColumn("Ship_To_State", typeof(string)));

                    dt.Columns.Add(new DataColumn("Vehical_Type", typeof(string)));
                    #endregion
                    string vno;
                    string to_ac_name_e;
                    string to_address1;
                    string to_address2;
                    string to_place;
                    string from_partnm;
                    string from_address;
                    string from_city;
                    string do_no = "";

                    string Bill_To;
                    string Ship_To;
                    double taxamount;
                    string taxvalue;
                    double CGST;
                    double SGST;
                    double IGST;
                    double CessAmt = 0.00;
                    double CessNontAdvol = 0.00;
                    double Other = 0.00;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region[default value]
                        dt.Rows[i]["Supply Type"] = "Outward";
                        dt.Rows[i]["Sub Type"] = "Supply";
                        dt.Rows[i]["Doc Type"] = "Tax Invoice";
                        dt.Rows[i]["Product"] = "SUGAR";
                        dt.Rows[i]["Description"] = "SUGAR";
                        dt.Rows[i]["HSN"] = "1701";
                        dt.Rows[i]["Unit"] = "QUINTAL";
                        dt.Rows[i]["CESS Amount"] = "0";

                        dt.Rows[i]["CESS Non Advol Amount"] = "0.00";
                        dt.Rows[i]["Other"] = "0.00";
                        dt.Rows[i]["Trans Mode"] = "ROAD";


                        //dt.Rows[i]["Distance level (Km)"] = "0";
                        dt.Rows[i]["Trans Name"] = " ";
                        dt.Rows[i]["Trans ID"] = " ";
                        dt.Rows[i]["Trans DocNo"] = " ";
                        dt.Rows[i]["Trans Date"] = dt.Rows[i]["TransDate"].ToString();
                        // dt.Rows[i]["State_Name"] = dt.Rows[i]["BillToStateCode"];

                        #region[replace from adreess]
                        //from_address = address;
                        from_address = dt.Rows[i]["milladdress"].ToString();

                        //to_address1 = to_address1.Replace("-", "");
                        //to_address1 = to_address1.Replace("/", "");
                        //to_address1 = to_address1.Replace("&", "");
                        from_address = from_address.Replace(".", "");
                        //to_address1 = to_address1.Replace("#", "");
                        //to_address1 = to_address1.Replace(",", "");
                        from_address = from_address.Replace("(", "");
                        from_address = from_address.Replace(")", "");
                        from_address = from_address.Replace(":", "");
                        from_address = from_address.Replace("_", "");
                        from_address = from_address.Replace("@", "");
                        from_address = from_address.Replace(";", "");
                        from_address = from_address.Replace("=", "");
                        dt.Rows[i]["From_Address2"] = from_address;
                        dt.Rows[i]["From_Address1"] = dt.Rows[i]["millname"].ToString();
                        #endregion

                        #region[replace from other party name]
                        from_partnm = comnm;
                        from_partnm = from_partnm.Replace("-", "");
                        from_partnm = from_partnm.Replace("/", "");
                        from_partnm = from_partnm.Replace("&", "");
                        from_partnm = from_partnm.Replace(".", "");
                        from_partnm = from_partnm.Replace("#", "");
                        from_partnm = from_partnm.Replace("(", "");
                        from_partnm = from_partnm.Replace(")", "");
                        from_partnm = from_partnm.Replace(":", "");
                        from_partnm = from_partnm.Replace("_", "");
                        from_partnm = from_partnm.Replace("@", "");
                        from_partnm = from_partnm.Replace(";", "");
                        from_partnm = from_partnm.Replace("=", "");
                        dt.Rows[i]["From_OtherPartyName"] = from_partnm;
                        #endregion

                        dt.Rows[i]["From_GSTIN"] = gstno;

                        #region[replace from place]
                        //from_city = city;
                        from_city = dt.Rows[i]["millcityname"].ToString();

                        //to_address1 = to_address1.Replace("-", "");
                        //to_address1 = to_address1.Replace("/", "");
                        //to_address1 = to_address1.Replace("&", "");
                        from_city = from_city.Replace(".", "");
                        //to_address1 = to_address1.Replace("#", "");
                        //to_address1 = to_address1.Replace(",", "");
                        from_city = from_city.Replace("(", "");
                        from_city = from_city.Replace(")", "");
                        from_city = from_city.Replace(":", "");
                        from_city = from_city.Replace("_", "");
                        from_city = from_city.Replace("@", "");
                        from_city = from_city.Replace(";", "");
                        from_city = from_city.Replace("=", "");
                        dt.Rows[i]["From_Place"] = from_city;
                        #endregion

                        //dt.Rows[i]["From_Pin Code"] = pin;
                        dt.Rows[i]["From_Pin Code"] = dt.Rows[i]["millpincode"].ToString();
                        dt.Rows[i]["From_State"] = state;
                        dt.Rows[i]["Dispatch_State"] = dt.Rows[i]["millstatename"].ToString();


                        #endregion


                        Bill_To = dt.Rows[i]["BillToName"].ToString();
                        Ship_To = dt.Rows[i]["ShippTo"].ToString();
                        if (Bill_To != Ship_To)
                        {
                            dt.Rows[i]["Transaction Type"] = "Combination of 2 and 3";

                        }
                        else
                        {
                            dt.Rows[i]["Transaction Type"] = "Bill From-Dispatch From";

                        }

                        taxamount = Convert.ToDouble(dt.Rows[i]["TaxableAmount"].ToString());
                        CGST = Convert.ToDouble(dt.Rows[i]["CGSTAmount"].ToString());
                        SGST = Convert.ToDouble(dt.Rows[i]["SGSTAmount"].ToString());
                        IGST = Convert.ToDouble(dt.Rows[i]["IGSTAmount"].ToString());
                        // taxvalue = (dt.Rows[i]["Taxrate"].ToString());
                        //string TotaInvoice = (taxamount + '+' + taxvalue + '0').ToString();

                        //dt.Rows[i]["Total Invoice Value"] = TotaInvoice;
                        double TotalInvoice = taxamount + CGST + SGST + IGST + CessAmt + CessNontAdvol + Other;

                        dt.Rows[i]["Total Invoice Value"] = TotalInvoice;

                        #region[replace vehicle no]
                        vno = Convert.ToString(dt.Rows[i]["LORRYNO"]);
                        vno = vno.Replace("-", "");
                        vno = vno.Replace("/", "");
                        vno = vno.Replace(" ", "");
                        vno = vno.Replace("&", "");
                        vno = vno.Replace(".", "");
                        vno = vno.Replace("#", "");
                        vno = vno.Replace("(", "");
                        vno = vno.Replace(")", "");
                        vno = vno.Replace(":", "");
                        vno = vno.Replace("_", "");
                        vno = vno.Replace("@", "");
                        vno = vno.Replace(";", "");
                        vno = vno.Replace("=", "");
                        dt.Rows[i]["LORRYNO"] = vno;
                        #endregion

                        #region[replae to party name]
                        to_ac_name_e = Convert.ToString(dt.Rows[i]["BillToName"]);
                        to_ac_name_e = to_ac_name_e.Replace("-", "");
                        to_ac_name_e = to_ac_name_e.Replace("/", "");
                        to_ac_name_e = to_ac_name_e.Replace("&", "");
                        to_ac_name_e = to_ac_name_e.Replace(".", "");
                        to_ac_name_e = to_ac_name_e.Replace("#", "");
                        to_ac_name_e = to_ac_name_e.Replace("(", "");
                        to_ac_name_e = to_ac_name_e.Replace(")", "");
                        to_ac_name_e = to_ac_name_e.Replace(":", "");
                        to_ac_name_e = to_ac_name_e.Replace("_", "");
                        to_ac_name_e = to_ac_name_e.Replace("@", "");
                        to_ac_name_e = to_ac_name_e.Replace(";", "");
                        to_ac_name_e = to_ac_name_e.Replace("=", "");
                        dt.Rows[i]["BillToName"] = to_ac_name_e;
                        #endregion

                        #region[replace to address1]
                        to_address1 = Convert.ToString(dt.Rows[i]["ShippTo"]);
                        to_address1 = to_address1.Replace("-", "");
                        to_address1 = to_address1.Replace("/", "");
                        to_address1 = to_address1.Replace("&", "");
                        to_address1 = to_address1.Replace(".", "");
                        to_address1 = to_address1.Replace("#", "");
                        to_address1 = to_address1.Replace(",", "");
                        to_address1 = to_address1.Replace("(", "");
                        to_address1 = to_address1.Replace(")", "");
                        to_address1 = to_address1.Replace(":", "");
                        to_address1 = to_address1.Replace("_", "");
                        to_address1 = to_address1.Replace("@", "");
                        to_address1 = to_address1.Replace(";", "");
                        to_address1 = to_address1.Replace("=", "");
                        dt.Rows[i]["ShippTo"] = to_address1;
                        //dt.Rows[i]["To_Address2"] = to_address1;
                        #endregion

                        #region[replace to address2]
                        to_address2 = Convert.ToString(dt.Rows[i]["Address_E"]);
                        to_address2 = to_address2.Replace("-", "");
                        to_address1 = to_address1.Replace("/", "");
                        to_address1 = to_address1.Replace("&", "");
                        to_address2 = to_address2.Replace(".", "");
                        to_address1 = to_address1.Replace("#", "");
                        to_address2 = to_address2.Replace(",", "");
                        to_address2 = to_address2.Replace("(", "");
                        to_address2 = to_address2.Replace(")", "");
                        to_address2 = to_address2.Replace(":", "");
                        to_address2 = to_address2.Replace("_", "");
                        to_address2 = to_address2.Replace("@", "");
                        to_address2 = to_address2.Replace(";", "");
                        to_address2 = to_address2.Replace("=", "");
                        to_address2 = to_address2.Replace("*", "");
                        to_address2 = to_address2.Replace(":", "");
                        to_address2 = to_address2.Replace(";", "");
                        dt.Rows[i]["Address_E"] = to_address2;
                        //dt.Rows[i]["To_Address2"] = to_address1;
                        #endregion

                        #region[replace to place]
                        to_place = Convert.ToString(dt.Rows[i]["city_name_e"]);
                        //to_address1 = to_address1.Replace("-", "");
                        //to_address1 = to_address1.Replace("/", "");
                        //to_address1 = to_address1.Replace("&", "");
                        to_place = to_place.Replace(".", "");
                        //to_address1 = to_address1.Replace("#", "");
                        //to_address1 = to_address1.Replace(",", "");
                        to_place = to_place.Replace("(", "");
                        to_place = to_place.Replace(")", "");
                        to_place = to_place.Replace(":", "");
                        to_place = to_place.Replace("_", "");
                        to_place = to_place.Replace("@", "");
                        to_place = to_place.Replace(";", "");
                        to_place = to_place.Replace("=", "");
                        dt.Rows[i]["city_name_e"] = to_place;
                        //  dt.Rows[i]["Ship_To_State"] = dt.Rows[i]["State_Name"];
                        #endregion

                        double to_Distance;
                        #region[replace to Distance]
                        to_Distance = Convert.ToDouble(dt.Rows[i]["Distance"]);


                        dt.Rows[i]["Distance"] = to_Distance;

                        dt.Rows[i]["Vehical_Type"] = "REGULAR";
                        #endregion



                    }

                    lblSummary.Text = "Empty E Way Bill";

                    grdAll.DataSource = dt;

                    #region[sequence]
                    dt.Columns["Supply Type"].SetOrdinal(0);
                    dt.Columns["Sub Type"].SetOrdinal(1);
                    dt.Columns["Doc Type"].SetOrdinal(2);
                    dt.Columns["Transaction Type"].SetOrdinal(5);

                    dt.Columns["From_OtherPartyName"].SetOrdinal(6);
                    dt.Columns["From_GSTIN"].SetOrdinal(7);
                    dt.Columns["From_Address1"].SetOrdinal(8);
                    dt.Columns["From_Address2"].SetOrdinal(9);
                    dt.Columns["From_Place"].SetOrdinal(10);
                    dt.Columns["From_Pin Code"].SetOrdinal(11);
                    dt.Columns["From_State"].SetOrdinal(12);
                    dt.Columns["Dispatch_State"].SetOrdinal(13);
                    dt.Columns["State_Name"].SetOrdinal(21);
                    dt.Columns["Product"].SetOrdinal(22);
                    dt.Columns["Description"].SetOrdinal(23);
                    dt.Columns["HSN"].SetOrdinal(24);
                    dt.Columns["Unit"].SetOrdinal(25);
                    dt.Columns["NETQNTL"].SetOrdinal(26);
                    dt.Columns["CESS Amount"].SetOrdinal(32);
                    dt.Columns["CESS Non Advol Amount"].SetOrdinal(33);
                    dt.Columns["Other"].SetOrdinal(34);
                    dt.Columns["Total Invoice Value"].SetOrdinal(35);
                    dt.Columns["Trans Mode"].SetOrdinal(36);
                    // dt.Columns["Distance level (Km)"].SetOrdinal(31);
                    dt.Columns["Trans Name"].SetOrdinal(38);
                    dt.Columns["Trans ID"].SetOrdinal(39);
                    dt.Columns["Trans DocNo"].SetOrdinal(40);
                    dt.Columns["Trans Date"].SetOrdinal(41);
                    dt.Columns["Vehical_Type"].SetOrdinal(43);
                    dt.Columns["DO_No"].SetOrdinal(44);
                    dt.Columns["millname"].SetOrdinal(45);

                    #endregion

                    grdAll.DataBind();

                    #region[header name]
                    grdAll.HeaderRow.Cells[3].Text = "Doc No";
                    grdAll.HeaderRow.Cells[4].Text = "Doc Date";
                    // grdAll.HeaderRow.Cells[5].Text = "From_OtherPartyName";
                    //  grdAll.HeaderRow.Cells[6].Text = "From_GSTIN";
                    //grdAll.HeaderRow.Cells[7].Text = "From_Address1";
                    //grdAll.HeaderRow.Cells[9].Text = "From_Place";
                    //grdAll.HeaderRow.Cells[10].Text = "From_Pin Code";
                    grdAll.HeaderRow.Cells[13].Text = "Dispatch State";
                    grdAll.HeaderRow.Cells[14].Text = "To_OtherPartyName";
                    grdAll.HeaderRow.Cells[15].Text = "To_GSTIN";
                    grdAll.HeaderRow.Cells[16].Text = "To_Address1";
                    grdAll.HeaderRow.Cells[17].Text = "To_Address2";
                    grdAll.HeaderRow.Cells[18].Text = "To_Place";
                    grdAll.HeaderRow.Cells[19].Text = "To_Pin Code";
                    grdAll.HeaderRow.Cells[20].Text = "To_State";
                    grdAll.HeaderRow.Cells[21].Text = "Ship To State";
                    grdAll.HeaderRow.Cells[26].Text = "Qty";
                    grdAll.HeaderRow.Cells[27].Text = "Assessable Value";
                    grdAll.HeaderRow.Cells[28].Text = "Tax Rate(C+S+I+Cess+CESS Non Advol Amount)";
                    grdAll.HeaderRow.Cells[29].Text = "CGST Amount";
                    grdAll.HeaderRow.Cells[30].Text = "SGST Amount";
                    grdAll.HeaderRow.Cells[31].Text = "IGST Amount";
                    grdAll.HeaderRow.Cells[37].Text = "Distance level (Km)";
                    grdAll.HeaderRow.Cells[42].Text = "Vehicel No";


                    #endregion

                    foreach (GridViewRow gvr in grdAll.Rows)
                    {
                        grdAll.HeaderRow.Cells[45].Visible = true;
                        grdAll.HeaderRow.Cells[46].Visible = false;
                        grdAll.HeaderRow.Cells[47].Visible = false;
                        grdAll.HeaderRow.Cells[48].Visible = false;
                        grdAll.HeaderRow.Cells[49].Visible = false;
                        grdAll.HeaderRow.Cells[50].Visible = false;



                        //grdAll.HeaderRow.Cells[44].Visible = false;
                        //grdAll.HeaderRow.Cells[45].Visible = false;
                        //grdAll.HeaderRow.Cells[46].Visible = false;
                        //grdAll.HeaderRow.Cells[47].Visible = false;

                        gvr.Cells[45].Visible = true;
                        gvr.Cells[46].Visible = false;
                        gvr.Cells[47].Visible = false;
                        gvr.Cells[48].Visible = false;
                        gvr.Cells[49].Visible = false;
                        gvr.Cells[50].Visible = false;

                        //gvr.Cells[44].Visible = false;
                        //gvr.Cells[45].Visible = false;
                        //gvr.Cells[46].Visible = false;
                        //gvr.Cells[47].Visible = false;

                    }

                    //  e.Row.Cells[7].ControlStyle.Width = new Unit("60px");
                    //  grdAll.HeaderRow.Cells[4].Width = new Unit("10px");
                    // grdAll.HeaderRow.Cells[4].ControlStyle.Width = new Unit("10px");

                }
            }

        }
        catch
        {
        }
    }

    protected void btnEwayBillcorpo_Click(object sender, EventArgs e)
    {
        #region[comment]
        //try
        //{
        //    //  grdAll.DataBind() = null;
        //    //grdAll.DataSource = null;
        //    //grdAll.DataBind();
        //    string fromdt = txtFromDt.Text;
        //    string todt = txtToDt.Text;
        //    fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
        //    todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
        //    #region[comment]

        //    #endregion


        //    string qry = "select doc_no,CONVERT(varchar,doc_date,103) as doc_date,"
        //              + " UPPER(BillToName) as BillToName,UPPER(BillToGst) as BillToGst ,UPPER(Ac_Name_E) as ShippTo,UPPER(Address_E) as Address_E,UPPER(city_name_e) as city_name_e,(case Pincode when 0 then 999999  else pincode end) as pincode,upper(BillToStateCode) AS BillToStateCode,UPPER(State_Name) as State_Name,NETQNTL,TaxableAmount,"
        //              + "(convert(varchar,CGSTRate,0) + '+' + convert(varchar,SGSTRate,0)+ '+' + convert(varchar,IGSTRate,0)+'+'+'0') as Taxrate,CGSTAmount,"
        //              + "SGSTAmount,IGSTAmount,Distance,LORRYNO,UPPER(millname) as millname,DO_No,UPPER(ToOtherPartyNameForCorporat) as ToOtherPartyNameForCorporat"
        //              + ",UPPER(AdrressForCorporate) as AdrressForCorporate,PincodeForCorporate,UPPER(StateNameForCorporate) as StateNameForCorporate,UPPER(CityNameForCorporate) as CityNameForCorporate,UPPER(ShipToForCorporate) as ShipToForCorporate"
        //              + " from NT_1_qryEWAYbillForCorporate where doc_date between '" + fromdt + "' and '" + todt
        //              + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
        //    DataSet ds = new DataSet();
        //    ds = clsDAL.SimpleQuery(qry);
        //    #region[from query]
        //    string qrynm = clsCommon.getString("select UPPER(Company_Name_E) as Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        //    string comnm = qrynm.ToUpper();
        //    string gstno = clsCommon.getString("select GST from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        //    string address = clsCommon.getString("select UPPER(Address_E) as Address_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        //    string city = clsCommon.getString("select UPPER(City_E) as City_E  from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        //    string pin = clsCommon.getString("select PIN from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
        //    string state = clsCommon.getString("select UPPER(State_E) as State_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));

        //    #endregion

        //    if (ds != null)
        //    {
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            DataTable dt = new DataTable();
        //            dt = ds.Tables[0];
        //            #region[colummn add]
        //            dt.Columns.Add(new DataColumn("Supply Type", typeof(string)));
        //            dt.Columns.Add(new DataColumn("Sub Type", typeof(string)));
        //            dt.Columns.Add(new DataColumn("Doc Type", typeof(string)));
        //            dt.Columns.Add(new DataColumn("Product", typeof(string)));
        //            dt.Columns.Add(new DataColumn("Description", typeof(string)));
        //            dt.Columns.Add(new DataColumn("HSN", typeof(string)));
        //            dt.Columns.Add(new DataColumn("Unit", typeof(string)));
        //            dt.Columns.Add(new DataColumn("CESS Amount", typeof(string)));
        //            dt.Columns.Add(new DataColumn("Trans Mode", typeof(string)));
        //            //dt.Columns.Add(new DataColumn("Distance level (Km)", typeof(string)));
        //            dt.Columns.Add(new DataColumn("Trans Name", typeof(string)));
        //            dt.Columns.Add(new DataColumn("Trans ID", typeof(string)));
        //            dt.Columns.Add(new DataColumn("Trans DocNo", typeof(string)));
        //            dt.Columns.Add(new DataColumn("Trans Date", typeof(string)));
        //            //dt.Columns.Add(new DataColumn("Doc No",typeof(Int32)));
        //            dt.Columns.Add(new DataColumn("From_Address2", typeof(string)));
        //            dt.Columns.Add(new DataColumn("From_Address1", typeof(string)));
        //            // dt.Columns.Add(new DataColumn("To_Address2", typeof(string)));
        //            dt.Columns.Add(new DataColumn("From_OtherPartyName", typeof(string)));
        //            dt.Columns.Add(new DataColumn("From_GSTIN", typeof(string)));
        //            dt.Columns.Add(new DataColumn("From_Place", typeof(string)));
        //            dt.Columns.Add(new DataColumn("From_Pin Code", typeof(string)));
        //            dt.Columns.Add(new DataColumn("From_State", typeof(string)));
        //            dt.Columns.Add(new DataColumn("Dispatch_State", typeof(string)));
        //            //dt.Columns.Add(new DataColumn("Ship_To_State", typeof(string)));
        //            //dt.Columns.Add(new DataColumn("Ship_To_State", typeof(string)));

        //            dt.Columns.Add(new DataColumn("Vehical_Type", typeof(string)));
        //            #endregion
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                string Do_No = dt.Rows[i]["DO_No"].ToString();
        //                string Carporate_No = clsCommon.getString("select isnull(Carporate_Sale_No,0) as Carporate_Sale_No from NT_1_qryEWAYbillForCorporate where DO_No='"
        //                    + Do_No + "'  and Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " and Company_Code=" 
        //                    + Convert.ToInt32(Session["Company_Code"].ToString()));



        //                if (Carporate_No != "0")
        //                {

        //                    string vno;
        //                    string to_ac_name_e;
        //                    string to_address1;
        //                    string to_address2;
        //                    string to_place;
        //                    string from_partnm;
        //                    string from_address;
        //                    string from_city;
        //                    string do_no = "";
        //                    //  for (int i = 0; i < dt.Rows.Count; i++)
        //                    //{
        //                    #region[default value]
        //                    dt.Rows[i]["Supply Type"] = "Outward";
        //                    dt.Rows[i]["Sub Type"] = "Supply";
        //                    dt.Rows[i]["Doc Type"] = "Tax Invoice";
        //                    dt.Rows[i]["Product"] = "SUGAR";
        //                    dt.Rows[i]["Description"] = "SUGAR";
        //                    dt.Rows[i]["HSN"] = "1701";
        //                    dt.Rows[i]["Unit"] = "QUINTAL";
        //                    dt.Rows[i]["CESS Amount"] = "0";
        //                    dt.Rows[i]["Trans Mode"] = "Road";
        //                    //dt.Rows[i]["Distance level (Km)"] = "0";
        //                    dt.Rows[i]["Trans Name"] = " ";
        //                    dt.Rows[i]["Trans ID"] = " ";
        //                    dt.Rows[i]["Trans DocNo"] = " ";
        //                    dt.Rows[i]["Trans Date"] = " ";
        //                    // dt.Rows[i]["State_Name"] = dt.Rows[i]["BillToStateCode"];

        //                    #region[replace from adreess]
        //                    from_address = address;

        //                    //to_address1 = to_address1.Replace("-", "");
        //                    //to_address1 = to_address1.Replace("/", "");
        //                    //to_address1 = to_address1.Replace("&", "");
        //                    from_address = from_address.Replace(".", "");
        //                    //to_address1 = to_address1.Replace("#", "");
        //                    //to_address1 = to_address1.Replace(",", "");
        //                    from_address = from_address.Replace("(", "");
        //                    from_address = from_address.Replace(")", "");
        //                    from_address = from_address.Replace(":", "");
        //                    from_address = from_address.Replace("_", "");
        //                    from_address = from_address.Replace("@", "");
        //                    from_address = from_address.Replace(";", "");
        //                    from_address = from_address.Replace("=", "");
        //                    dt.Rows[i]["From_Address2"] = from_address;
        //                    dt.Rows[i]["From_Address1"] = from_address;
        //                    #endregion

        //                    #region[replace from other party name]
        //                    from_partnm = comnm;
        //                    from_partnm = from_partnm.Replace("-", "");
        //                    from_partnm = from_partnm.Replace("/", "");
        //                    from_partnm = from_partnm.Replace("&", "");
        //                    from_partnm = from_partnm.Replace(".", "");
        //                    from_partnm = from_partnm.Replace("#", "");
        //                    from_partnm = from_partnm.Replace("(", "");
        //                    from_partnm = from_partnm.Replace(")", "");
        //                    from_partnm = from_partnm.Replace(":", "");
        //                    from_partnm = from_partnm.Replace("_", "");
        //                    from_partnm = from_partnm.Replace("@", "");
        //                    from_partnm = from_partnm.Replace(";", "");
        //                    from_partnm = from_partnm.Replace("=", "");
        //                    dt.Rows[i]["From_OtherPartyName"] = from_partnm;
        //                    #endregion

        //                    dt.Rows[i]["From_GSTIN"] = gstno;

        //                    #region[replace from place]
        //                    from_city = city;

        //                    //to_address1 = to_address1.Replace("-", "");
        //                    //to_address1 = to_address1.Replace("/", "");
        //                    //to_address1 = to_address1.Replace("&", "");
        //                    from_city = from_city.Replace(".", "");
        //                    //to_address1 = to_address1.Replace("#", "");
        //                    //to_address1 = to_address1.Replace(",", "");
        //                    from_city = from_city.Replace("(", "");
        //                    from_city = from_city.Replace(")", "");
        //                    from_city = from_city.Replace(":", "");
        //                    from_city = from_city.Replace("_", "");
        //                    from_city = from_city.Replace("@", "");
        //                    from_city = from_city.Replace(";", "");
        //                    from_city = from_city.Replace("=", "");
        //                    dt.Rows[i]["From_Place"] = from_city;
        //                    #endregion

        //                    dt.Rows[i]["From_Pin Code"] = pin;
        //                    dt.Rows[i]["From_State"] = state;
        //                    dt.Rows[i]["Dispatch_State"] = state;


        //                    #endregion

        //                    #region[replace vehicle no]
        //                    vno = Convert.ToString(dt.Rows[i]["LORRYNO"]);
        //                    vno = vno.Replace("-", "");
        //                    vno = vno.Replace("/", "");
        //                    vno = vno.Replace(" ", "");
        //                    vno = vno.Replace("&", "");
        //                    vno = vno.Replace(".", "");
        //                    vno = vno.Replace("#", "");
        //                    vno = vno.Replace("(", "");
        //                    vno = vno.Replace(")", "");
        //                    vno = vno.Replace(":", "");
        //                    vno = vno.Replace("_", "");
        //                    vno = vno.Replace("@", "");
        //                    vno = vno.Replace(";", "");
        //                    vno = vno.Replace("=", "");
        //                    dt.Rows[i]["LORRYNO"] = vno;
        //                    #endregion

        //                    #region[replae to party name]
        //                    to_ac_name_e = Convert.ToString(dt.Rows[i]["ToOtherPartyNameForCorporat"]);
        //                    to_ac_name_e = to_ac_name_e.Replace("-", "");
        //                    to_ac_name_e = to_ac_name_e.Replace("/", "");
        //                    to_ac_name_e = to_ac_name_e.Replace("&", "");
        //                    to_ac_name_e = to_ac_name_e.Replace(".", "");
        //                    to_ac_name_e = to_ac_name_e.Replace("#", "");
        //                    to_ac_name_e = to_ac_name_e.Replace("(", "");
        //                    to_ac_name_e = to_ac_name_e.Replace(")", "");
        //                    to_ac_name_e = to_ac_name_e.Replace(":", "");
        //                    to_ac_name_e = to_ac_name_e.Replace("_", "");
        //                    to_ac_name_e = to_ac_name_e.Replace("@", "");
        //                    to_ac_name_e = to_ac_name_e.Replace(";", "");
        //                    to_ac_name_e = to_ac_name_e.Replace("=", "");
        //                    dt.Rows[i]["ToOtherPartyNameForCorporat"] = to_ac_name_e;
        //                    dt.Rows[i]["BillToName"] = to_ac_name_e; 

        //                    #endregion

        //                    #region[replace to address1]
        //                    to_address1 = Convert.ToString(dt.Rows[i]["ShipToForCorporate"]);
        //                    to_address1 = to_address1.Replace("-", "");
        //                    to_address1 = to_address1.Replace("/", "");
        //                    to_address1 = to_address1.Replace("&", "");
        //                    to_address1 = to_address1.Replace(".", "");
        //                    to_address1 = to_address1.Replace("#", "");
        //                    to_address1 = to_address1.Replace(",", "");
        //                    to_address1 = to_address1.Replace("(", "");
        //                    to_address1 = to_address1.Replace(")", "");
        //                    to_address1 = to_address1.Replace(":", "");
        //                    to_address1 = to_address1.Replace("_", "");
        //                    to_address1 = to_address1.Replace("@", "");
        //                    to_address1 = to_address1.Replace(";", "");
        //                    to_address1 = to_address1.Replace("=", "");
        //                    dt.Rows[i]["ShipToForCorporate"] = to_address1;
        //                    //dt.Rows[i]["To_Address2"] = to_address1;
        //                    #endregion

        //                    #region[replace to address2]
        //                    to_address2 = Convert.ToString(dt.Rows[i]["AdrressForCorporate"]);
        //                    to_address2 = to_address2.Replace("-", "");
        //                    to_address1 = to_address1.Replace("/", "");
        //                    to_address1 = to_address1.Replace("&", "");
        //                    to_address2 = to_address2.Replace(".", "");
        //                    to_address1 = to_address1.Replace("#", "");
        //                    to_address2 = to_address2.Replace(",", "");
        //                    to_address2 = to_address2.Replace("(", "");
        //                    to_address2 = to_address2.Replace(")", "");
        //                    to_address2 = to_address2.Replace(":", "");
        //                    to_address2 = to_address2.Replace("_", "");
        //                    to_address2 = to_address2.Replace("@", "");
        //                    to_address2 = to_address2.Replace(";", "");
        //                    to_address2 = to_address2.Replace("=", "");
        //                    to_address2 = to_address2.Replace("*", "");
        //                    to_address2 = to_address2.Replace(":", "");
        //                    to_address2 = to_address2.Replace(";", "");
        //                    dt.Rows[i]["AdrressForCorporate"] = to_address2;
        //                    //dt.Rows[i]["To_Address2"] = to_address1;
        //                    #endregion

        //                    #region[replace to place]
        //                    to_place = Convert.ToString(dt.Rows[i]["CityNameForCorporate"]);
        //                    //to_address1 = to_address1.Replace("-", "");
        //                    //to_address1 = to_address1.Replace("/", "");
        //                    //to_address1 = to_address1.Replace("&", "");
        //                    to_place = to_place.Replace(".", "");
        //                    //to_address1 = to_address1.Replace("#", "");
        //                    //to_address1 = to_address1.Replace(",", "");
        //                    to_place = to_place.Replace("(", "");
        //                    to_place = to_place.Replace(")", "");
        //                    to_place = to_place.Replace(":", "");
        //                    to_place = to_place.Replace("_", "");
        //                    to_place = to_place.Replace("@", "");
        //                    to_place = to_place.Replace(";", "");
        //                    to_place = to_place.Replace("=", "");
        //                    dt.Rows[i]["CityNameForCorporate"] = to_place;
        //                    //  dt.Rows[i]["Ship_To_State"] = dt.Rows[i]["State_Name"];
        //                    #endregion

        //                    double to_Distance;

        //                    #region[replace to Distance]
        //                    to_Distance = Convert.ToDouble(dt.Rows[i]["Distance"]);


        //                    dt.Rows[i]["Distance"] = to_Distance;

        //                    dt.Rows[i]["Vehical_Type"] = "REGULAR";
        //                    #endregion

        //                    // }

        //                    lblSummary.Text = "E Way Bill";

        //                    grdAll.DataSource = dt;






        //                }
        //                //else
        //                //{

        //                //    string vno;
        //                //    string to_ac_name_e;
        //                //    string to_address1;
        //                //    string to_address2;
        //                //    string to_place;
        //                //    string from_partnm;
        //                //    string from_address;
        //                //    string from_city;
        //                //    string do_no = "";
        //                //    // for (int i = 0; i < dt.Rows.Count; i++)
        //                //    //{
        //                //    #region[default value]
        //                //    dt.Rows[i]["Supply Type"] = "Outward";
        //                //    dt.Rows[i]["Sub Type"] = "Supply";
        //                //    dt.Rows[i]["Doc Type"] = "Tax Invoice";
        //                //    dt.Rows[i]["Product"] = "SUGAR";
        //                //    dt.Rows[i]["Description"] = "SUGAR";
        //                //    dt.Rows[i]["HSN"] = "1701";
        //                //    dt.Rows[i]["Unit"] = "QUINTAL";
        //                //    dt.Rows[i]["CESS Amount"] = "0";
        //                //    dt.Rows[i]["Trans Mode"] = "Road";
        //                //    //dt.Rows[i]["Distance level (Km)"] = "0";
        //                //    dt.Rows[i]["Trans Name"] = " ";
        //                //    dt.Rows[i]["Trans ID"] = " ";
        //                //    dt.Rows[i]["Trans DocNo"] = " ";
        //                //    dt.Rows[i]["Trans Date"] = " ";
        //                //    // dt.Rows[i]["State_Name"] = dt.Rows[i]["BillToStateCode"];

        //                //    #region[replace from adreess]
        //                //    from_address = address;

        //                //    //to_address1 = to_address1.Replace("-", "");
        //                //    //to_address1 = to_address1.Replace("/", "");
        //                //    //to_address1 = to_address1.Replace("&", "");
        //                //    from_address = from_address.Replace(".", "");
        //                //    //to_address1 = to_address1.Replace("#", "");
        //                //    //to_address1 = to_address1.Replace(",", "");
        //                //    from_address = from_address.Replace("(", "");
        //                //    from_address = from_address.Replace(")", "");
        //                //    from_address = from_address.Replace(":", "");
        //                //    from_address = from_address.Replace("_", "");
        //                //    from_address = from_address.Replace("@", "");
        //                //    from_address = from_address.Replace(";", "");
        //                //    from_address = from_address.Replace("=", "");
        //                //    dt.Rows[i]["From_Address2"] = from_address;
        //                //    dt.Rows[i]["From_Address1"] = from_address;
        //                //    #endregion

        //                //    #region[replace from other party name]
        //                //    from_partnm = comnm;
        //                //    from_partnm = from_partnm.Replace("-", "");
        //                //    from_partnm = from_partnm.Replace("/", "");
        //                //    from_partnm = from_partnm.Replace("&", "");
        //                //    from_partnm = from_partnm.Replace(".", "");
        //                //    from_partnm = from_partnm.Replace("#", "");
        //                //    from_partnm = from_partnm.Replace("(", "");
        //                //    from_partnm = from_partnm.Replace(")", "");
        //                //    from_partnm = from_partnm.Replace(":", "");
        //                //    from_partnm = from_partnm.Replace("_", "");
        //                //    from_partnm = from_partnm.Replace("@", "");
        //                //    from_partnm = from_partnm.Replace(";", "");
        //                //    from_partnm = from_partnm.Replace("=", "");
        //                //    dt.Rows[i]["From_OtherPartyName"] = from_partnm;
        //                //    #endregion

        //                //    dt.Rows[i]["From_GSTIN"] = gstno;

        //                //    #region[replace from place]
        //                //    from_city = city;

        //                //    //to_address1 = to_address1.Replace("-", "");
        //                //    //to_address1 = to_address1.Replace("/", "");
        //                //    //to_address1 = to_address1.Replace("&", "");
        //                //    from_city = from_city.Replace(".", "");
        //                //    //to_address1 = to_address1.Replace("#", "");
        //                //    //to_address1 = to_address1.Replace(",", "");
        //                //    from_city = from_city.Replace("(", "");
        //                //    from_city = from_city.Replace(")", "");
        //                //    from_city = from_city.Replace(":", "");
        //                //    from_city = from_city.Replace("_", "");
        //                //    from_city = from_city.Replace("@", "");
        //                //    from_city = from_city.Replace(";", "");
        //                //    from_city = from_city.Replace("=", "");
        //                //    dt.Rows[i]["From_Place"] = from_city;
        //                //    #endregion

        //                //    dt.Rows[i]["From_Pin Code"] = pin;
        //                //    dt.Rows[i]["From_State"] = state;
        //                //    dt.Rows[i]["Dispatch_State"] = state;


        //                //    #endregion

        //                //    #region[replace vehicle no]
        //                //    vno = Convert.ToString(dt.Rows[i]["LORRYNO"]);
        //                //    vno = vno.Replace("-", "");
        //                //    vno = vno.Replace("/", "");
        //                //    vno = vno.Replace(" ", "");
        //                //    vno = vno.Replace("&", "");
        //                //    vno = vno.Replace(".", "");
        //                //    vno = vno.Replace("#", "");
        //                //    vno = vno.Replace("(", "");
        //                //    vno = vno.Replace(")", "");
        //                //    vno = vno.Replace(":", "");
        //                //    vno = vno.Replace("_", "");
        //                //    vno = vno.Replace("@", "");
        //                //    vno = vno.Replace(";", "");
        //                //    vno = vno.Replace("=", "");
        //                //    dt.Rows[i]["LORRYNO"] = vno;
        //                //    #endregion

        //                //    #region[replae to party name]
        //                //    to_ac_name_e = Convert.ToString(dt.Rows[i]["BillToName"]);
        //                //    to_ac_name_e = to_ac_name_e.Replace("-", "");
        //                //    to_ac_name_e = to_ac_name_e.Replace("/", "");
        //                //    to_ac_name_e = to_ac_name_e.Replace("&", "");
        //                //    to_ac_name_e = to_ac_name_e.Replace(".", "");
        //                //    to_ac_name_e = to_ac_name_e.Replace("#", "");
        //                //    to_ac_name_e = to_ac_name_e.Replace("(", "");
        //                //    to_ac_name_e = to_ac_name_e.Replace(")", "");
        //                //    to_ac_name_e = to_ac_name_e.Replace(":", "");
        //                //    to_ac_name_e = to_ac_name_e.Replace("_", "");
        //                //    to_ac_name_e = to_ac_name_e.Replace("@", "");
        //                //    to_ac_name_e = to_ac_name_e.Replace(";", "");
        //                //    to_ac_name_e = to_ac_name_e.Replace("=", "");
        //                //    dt.Rows[i]["BillToName"] = to_ac_name_e;
        //                //    #endregion

        //                //    #region[replace to address1]
        //                //    to_address1 = Convert.ToString(dt.Rows[i]["ShippTo"]);
        //                //    to_address1 = to_address1.Replace("-", "");
        //                //    to_address1 = to_address1.Replace("/", "");
        //                //    to_address1 = to_address1.Replace("&", "");
        //                //    to_address1 = to_address1.Replace(".", "");
        //                //    to_address1 = to_address1.Replace("#", "");
        //                //    to_address1 = to_address1.Replace(",", "");
        //                //    to_address1 = to_address1.Replace("(", "");
        //                //    to_address1 = to_address1.Replace(")", "");
        //                //    to_address1 = to_address1.Replace(":", "");
        //                //    to_address1 = to_address1.Replace("_", "");
        //                //    to_address1 = to_address1.Replace("@", "");
        //                //    to_address1 = to_address1.Replace(";", "");
        //                //    to_address1 = to_address1.Replace("=", "");
        //                //    dt.Rows[i]["ShippTo"] = to_address1;
        //                //    //dt.Rows[i]["To_Address2"] = to_address1;
        //                //    #endregion

        //                //    #region[replace to address2]
        //                //    to_address2 = Convert.ToString(dt.Rows[i]["Address_E"]);
        //                //    to_address2 = to_address2.Replace("-", "");
        //                //    to_address1 = to_address1.Replace("/", "");
        //                //    to_address1 = to_address1.Replace("&", "");
        //                //    to_address2 = to_address2.Replace(".", "");
        //                //    to_address1 = to_address1.Replace("#", "");
        //                //    to_address2 = to_address2.Replace(",", "");
        //                //    to_address2 = to_address2.Replace("(", "");
        //                //    to_address2 = to_address2.Replace(")", "");
        //                //    to_address2 = to_address2.Replace(":", "");
        //                //    to_address2 = to_address2.Replace("_", "");
        //                //    to_address2 = to_address2.Replace("@", "");
        //                //    to_address2 = to_address2.Replace(";", "");
        //                //    to_address2 = to_address2.Replace("=", "");
        //                //    to_address2 = to_address2.Replace("*", "");
        //                //    to_address2 = to_address2.Replace(":", "");
        //                //    to_address2 = to_address2.Replace(";", "");
        //                //    dt.Rows[i]["Address_E"] = to_address2;
        //                //    //dt.Rows[i]["To_Address2"] = to_address1;
        //                //    #endregion

        //                //    #region[replace to place]
        //                //    to_place = Convert.ToString(dt.Rows[i]["city_name_e"]);
        //                //    //to_address1 = to_address1.Replace("-", "");
        //                //    //to_address1 = to_address1.Replace("/", "");
        //                //    //to_address1 = to_address1.Replace("&", "");
        //                //    to_place = to_place.Replace(".", "");
        //                //    //to_address1 = to_address1.Replace("#", "");
        //                //    //to_address1 = to_address1.Replace(",", "");
        //                //    to_place = to_place.Replace("(", "");
        //                //    to_place = to_place.Replace(")", "");
        //                //    to_place = to_place.Replace(":", "");
        //                //    to_place = to_place.Replace("_", "");
        //                //    to_place = to_place.Replace("@", "");
        //                //    to_place = to_place.Replace(";", "");
        //                //    to_place = to_place.Replace("=", "");
        //                //    dt.Rows[i]["city_name_e"] = to_place;
        //                //    //  dt.Rows[i]["Ship_To_State"] = dt.Rows[i]["State_Name"];
        //                //    #endregion

        //                //    double to_Distance;

        //                //    #region[replace to Distance]
        //                //    to_Distance = Convert.ToDouble(dt.Rows[i]["Distance"]);


        //                //    dt.Rows[i]["Distance"] = to_Distance;

        //                //    dt.Rows[i]["Vehical_Type"] = "REGULAR";
        //                //    #endregion

        //                //    // }

        //                //    lblSummary.Text = "E Way Bill";

        //                //    grdAll.DataSource = dt;







        //                //    //  e.Row.Cells[7].ControlStyle.Width = new Unit("60px");
        //                //    //  grdAll.HeaderRow.Cells[4].Width = new Unit("10px");
        //                //    // grdAll.HeaderRow.Cells[4].ControlStyle.Width = new Unit("10px");

        //                //}
        //            }
        //            #region[sequence]
        //            dt.Columns["Supply Type"].SetOrdinal(0);
        //            dt.Columns["Sub Type"].SetOrdinal(1);
        //            dt.Columns["Doc Type"].SetOrdinal(2);
        //            dt.Columns["From_OtherPartyName"].SetOrdinal(5);
        //            dt.Columns["From_GSTIN"].SetOrdinal(6);
        //            dt.Columns["From_Address1"].SetOrdinal(7);
        //            dt.Columns["From_Address2"].SetOrdinal(8);
        //            dt.Columns["From_Place"].SetOrdinal(9);
        //            dt.Columns["From_Pin Code"].SetOrdinal(10);
        //            dt.Columns["From_State"].SetOrdinal(11);
        //            dt.Columns["Dispatch_State"].SetOrdinal(12);
        //            dt.Columns["State_Name"].SetOrdinal(20);
        //            dt.Columns["Product"].SetOrdinal(21);
        //            dt.Columns["Description"].SetOrdinal(22);
        //            dt.Columns["HSN"].SetOrdinal(23);
        //            dt.Columns["Unit"].SetOrdinal(24);
        //            dt.Columns["NETQNTL"].SetOrdinal(25);
        //            dt.Columns["CESS Amount"].SetOrdinal(31);
        //            dt.Columns["Trans Mode"].SetOrdinal(32);
        //            // dt.Columns["Distance level (Km)"].SetOrdinal(31);
        //            dt.Columns["Trans Name"].SetOrdinal(34);
        //            dt.Columns["Trans ID"].SetOrdinal(35);
        //            dt.Columns["Trans DocNo"].SetOrdinal(36);
        //            dt.Columns["Trans Date"].SetOrdinal(37);
        //            dt.Columns["Vehical_Type"].SetOrdinal(39);
        //            dt.Columns["DO_No"].SetOrdinal(40);
        //            dt.Columns["millname"].SetOrdinal(41);

        //            #endregion
        //            grdAll.DataBind();
        //            #region[header name]
        //            grdAll.HeaderRow.Cells[3].Text = "Doc No";
        //            grdAll.HeaderRow.Cells[4].Text = "Doc Date";
        //            // grdAll.HeaderRow.Cells[5].Text = "From_OtherPartyName";
        //            //  grdAll.HeaderRow.Cells[6].Text = "From_GSTIN";
        //            //grdAll.HeaderRow.Cells[7].Text = "From_Address1";
        //            //grdAll.HeaderRow.Cells[9].Text = "From_Place";
        //            //grdAll.HeaderRow.Cells[10].Text = "From_Pin Code";
        //            grdAll.HeaderRow.Cells[12].Text = "Dispatch State";
        //            grdAll.HeaderRow.Cells[13].Text = "To_OtherPartyName";
        //            grdAll.HeaderRow.Cells[14].Text = "To_GSTIN";
        //            grdAll.HeaderRow.Cells[15].Text = "To_Address1";
        //            grdAll.HeaderRow.Cells[16].Text = "To_Address2";
        //            grdAll.HeaderRow.Cells[17].Text = "To_Place";
        //            grdAll.HeaderRow.Cells[18].Text = "To_Pin Code";
        //            grdAll.HeaderRow.Cells[19].Text = "To_State";
        //            grdAll.HeaderRow.Cells[20].Text = "Ship To State";
        //            grdAll.HeaderRow.Cells[25].Text = "Qty";
        //            grdAll.HeaderRow.Cells[26].Text = "Assessable Value";
        //            grdAll.HeaderRow.Cells[27].Text = "Tax Rate(C+S+I+Cess)";
        //            grdAll.HeaderRow.Cells[28].Text = "CGST Amount";
        //            grdAll.HeaderRow.Cells[29].Text = "SGST Amount";
        //            grdAll.HeaderRow.Cells[30].Text = "IGST Amount";
        //            grdAll.HeaderRow.Cells[33].Text = "Distance level (Km)";
        //            grdAll.HeaderRow.Cells[38].Text = "Vehicel No";


        //            #endregion


        //            //foreach (GridViewRow gvr in grdAll.Rows)
        //            //{
        //            //    grdAll.HeaderRow.Cells[42].Visible = false;
        //            //    grdAll.HeaderRow.Cells[43].Visible = false;
        //            //    grdAll.HeaderRow.Cells[44].Visible = false;
        //            //    grdAll.HeaderRow.Cells[45].Visible = false;
        //            //    grdAll.HeaderRow.Cells[46].Visible = false;
        //            //    grdAll.HeaderRow.Cells[47].Visible = false;

        //            //    gvr.Cells[42].Visible = false;
        //            //    gvr.Cells[43].Visible = false;
        //            //    gvr.Cells[44].Visible = false;
        //            //    gvr.Cells[45].Visible = false;
        //            //    gvr.Cells[46].Visible = false;
        //            //    gvr.Cells[47].Visible = false;

        //            //}
        //        }
        //    }

        //}
        //catch
        //{
        //}
        #endregion


        try
        {
            //  grdAll.DataBind() = null;
            //grdAll.DataSource = null;
            //grdAll.DataBind();
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            #region[comment]

            #endregion


            string qry = "select doc_no,CONVERT(varchar,doc_date,103) as doc_date,"
                      + " UPPER(ToOtherPartyNameForCorporat) as ToOtherPartyNameForCorporat,UPPER(GSTNumForCaoporate) as GSTNumForCaoporate ," +
                      "UPPER(ShipToForCorporate) as ShipToForCorporate,UPPER(AdrressForCorporate) as AdrressForCorporate,UPPER(CityNameForCorporate) as CityNameForCorporate," +
                      "(case PincodeForCorporate when 0 then 999999  else PincodeForCorporate end) as PincodeForCorporate," +
                      "upper(ToOthrStateForCorporate) AS ToOthrStateForCorporate,UPPER(StateNameForCorporate) as StateNameForCorporate,NETQNTL,TaxableAmount,"
                      + "(convert(varchar,CGSTRate,0) + '+' + convert(varchar,SGSTRate,0)+ '+' + convert(varchar,IGSTRate,0)+'+'+'0'+'+'+'0') as Taxrate,CGSTAmount,"
                      + "SGSTAmount,IGSTAmount,Distance,LORRYNO,UPPER(millname) as millname,upper(milladdress) as milladdress,(case millpincode when 0 then 999999  else millpincode end) as millpincode,millcityname,DO_No,upper(millstatename) as millstatename,convert(varchar,TransDate,103)as TransDate"
                      + " from NT_1_qryEWAYbillForCorporate where (Carporate_Sale_No!=0 or Carporate_Sale_No is not null)  and doc_date between '" + fromdt + "' and '" + todt
                      + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());



            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);
            #region[from query]
            string qrynm = clsCommon.getString("select UPPER(Company_Name_E) as Company_Name_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string comnm = qrynm.ToUpper();
            string gstno = clsCommon.getString("select GST from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string address = clsCommon.getString("select UPPER(Address_E) as Address_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string city = clsCommon.getString("select UPPER(City_E) as City_E  from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string pin = clsCommon.getString("select PIN from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            string state = clsCommon.getString("select UPPER(State_E) as State_E from Company where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            #endregion

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    #region[colummn add]
                    dt.Columns.Add(new DataColumn("Supply Type", typeof(string)));
                    dt.Columns.Add(new DataColumn("Sub Type", typeof(string)));
                    dt.Columns.Add(new DataColumn("Doc Type", typeof(string)));

                    dt.Columns.Add(new DataColumn("Transaction Type", typeof(string)));
                    dt.Columns.Add(new DataColumn("Product", typeof(string)));
                    dt.Columns.Add(new DataColumn("Description", typeof(string)));
                    dt.Columns.Add(new DataColumn("HSN", typeof(string)));
                    dt.Columns.Add(new DataColumn("Unit", typeof(string)));
                    dt.Columns.Add(new DataColumn("CESS Amount", typeof(string)));
                    dt.Columns.Add(new DataColumn("CESS Non Advol Amount", typeof(string)));
                    dt.Columns.Add(new DataColumn("Other", typeof(string)));
                    dt.Columns.Add(new DataColumn("Total Invoice Value", typeof(string)));

                    dt.Columns.Add(new DataColumn("Trans Mode", typeof(string)));
                    //dt.Columns.Add(new DataColumn("Distance level (Km)", typeof(string)));
                    dt.Columns.Add(new DataColumn("Trans Name", typeof(string)));
                    dt.Columns.Add(new DataColumn("Trans ID", typeof(string)));
                    dt.Columns.Add(new DataColumn("Trans DocNo", typeof(string)));
                    dt.Columns.Add(new DataColumn("Trans Date", typeof(string)));
                    //dt.Columns.Add(new DataColumn("Doc No",typeof(Int32)));
                    dt.Columns.Add(new DataColumn("From_Address2", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_Address1", typeof(string)));
                    // dt.Columns.Add(new DataColumn("To_Address2", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_OtherPartyName", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_GSTIN", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_Place", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_Pin Code", typeof(string)));
                    dt.Columns.Add(new DataColumn("From_State", typeof(string)));
                    dt.Columns.Add(new DataColumn("Dispatch_State", typeof(string)));
                    //dt.Columns.Add(new DataColumn("Ship_To_State", typeof(string)));
                    //dt.Columns.Add(new DataColumn("Ship_To_State", typeof(string)));

                    dt.Columns.Add(new DataColumn("Vehical_Type", typeof(string)));
                    #endregion

                    string vno;
                    string to_ac_name_e;
                    string to_address1;
                    string to_address2;
                    string to_place;
                    string from_partnm;
                    string from_address;
                    string from_city;
                    string do_no = "";

                    string Bill_To;
                    string Ship_To;
                    double taxamount;
                    string taxvalue;
                    double CGST;
                    double SGST;
                    double IGST;
                    double CessAmt = 0.00;
                    double CessNontAdvol = 0.00;
                    double Other = 0.00;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        #region[default value]
                        dt.Rows[i]["Supply Type"] = "Outward";
                        dt.Rows[i]["Sub Type"] = "Supply";
                        dt.Rows[i]["Doc Type"] = "Tax Invoice";
                        dt.Rows[i]["Product"] = "SUGAR";
                        dt.Rows[i]["Description"] = "SUGAR";
                        dt.Rows[i]["HSN"] = "1701";
                        dt.Rows[i]["Unit"] = "QUINTAL";
                        dt.Rows[i]["CESS Amount"] = "0";

                        dt.Rows[i]["CESS Non Advol Amount"] = "0.00";
                        dt.Rows[i]["Other"] = "0.00";

                        dt.Rows[i]["Trans Mode"] = "Road";
                        //dt.Rows[i]["Distance level (Km)"] = "0";
                        dt.Rows[i]["Trans Name"] = " ";
                        dt.Rows[i]["Trans ID"] = " ";
                        dt.Rows[i]["Trans DocNo"] = " ";
                        dt.Rows[i]["Trans Date"] = dt.Rows[i]["TransDate"].ToString();
                        // dt.Rows[i]["State_Name"] = dt.Rows[i]["BillToStateCode"];

                        #region[replace from adreess]
                        //from_address = address;

                        from_address = dt.Rows[i]["milladdress"].ToString();

                        //to_address1 = to_address1.Replace("-", "");
                        //to_address1 = to_address1.Replace("/", "");
                        //to_address1 = to_address1.Replace("&", "");
                        from_address = from_address.Replace(".", "");
                        //to_address1 = to_address1.Replace("#", "");
                        //to_address1 = to_address1.Replace(",", "");
                        from_address = from_address.Replace("(", "");
                        from_address = from_address.Replace(")", "");
                        from_address = from_address.Replace(":", "");
                        from_address = from_address.Replace("_", "");
                        from_address = from_address.Replace("@", "");
                        from_address = from_address.Replace(";", "");
                        from_address = from_address.Replace("=", "");
                        dt.Rows[i]["From_Address2"] = from_address;
                        dt.Rows[i]["From_Address1"] = dt.Rows[i]["millname"].ToString();
                        #endregion

                        #region[replace from other party name]
                        from_partnm = comnm;
                        from_partnm = from_partnm.Replace("-", "");
                        from_partnm = from_partnm.Replace("/", "");
                        from_partnm = from_partnm.Replace("&", "");
                        from_partnm = from_partnm.Replace(".", "");
                        from_partnm = from_partnm.Replace("#", "");
                        from_partnm = from_partnm.Replace("(", "");
                        from_partnm = from_partnm.Replace(")", "");
                        from_partnm = from_partnm.Replace(":", "");
                        from_partnm = from_partnm.Replace("_", "");
                        from_partnm = from_partnm.Replace("@", "");
                        from_partnm = from_partnm.Replace(";", "");
                        from_partnm = from_partnm.Replace("=", "");
                        dt.Rows[i]["From_OtherPartyName"] = from_partnm;
                        #endregion

                        dt.Rows[i]["From_GSTIN"] = gstno;

                        #region[replace from place]
                        //from_city = city;
                        from_city = dt.Rows[i]["millcityname"].ToString();
                        //to_address1 = to_address1.Replace("-", "");
                        //to_address1 = to_address1.Replace("/", "");
                        //to_address1 = to_address1.Replace("&", "");
                        from_city = from_city.Replace(".", "");
                        //to_address1 = to_address1.Replace("#", "");
                        //to_address1 = to_address1.Replace(",", "");
                        from_city = from_city.Replace("(", "");
                        from_city = from_city.Replace(")", "");
                        from_city = from_city.Replace(":", "");
                        from_city = from_city.Replace("_", "");
                        from_city = from_city.Replace("@", "");
                        from_city = from_city.Replace(";", "");
                        from_city = from_city.Replace("=", "");
                        dt.Rows[i]["From_Place"] = from_city;
                        #endregion

                        //dt.Rows[i]["From_Pin Code"] = pin;

                        dt.Rows[i]["From_Pin Code"] = dt.Rows[i]["millpincode"].ToString();
                        dt.Rows[i]["From_State"] = state;
                        dt.Rows[i]["Dispatch_State"] = dt.Rows[i]["millstatename"].ToString();


                        #endregion

                        Bill_To = dt.Rows[i]["ToOtherPartyNameForCorporat"].ToString();
                        Ship_To = dt.Rows[i]["ShipToForCorporate"].ToString();
                        if (Bill_To != Ship_To)
                        {
                            dt.Rows[i]["Transaction Type"] = "Combination of 2 and 3";

                        }
                        else
                        {
                            dt.Rows[i]["Transaction Type"] = "Bill From-Dispatch From";

                        }

                        taxamount = Convert.ToDouble(dt.Rows[i]["TaxableAmount"].ToString());
                        CGST = Convert.ToDouble(dt.Rows[i]["CGSTAmount"].ToString());
                        SGST = Convert.ToDouble(dt.Rows[i]["SGSTAmount"].ToString());
                        IGST = Convert.ToDouble(dt.Rows[i]["IGSTAmount"].ToString());
                        // taxvalue = (dt.Rows[i]["Taxrate"].ToString());
                        //string TotaInvoice = (taxamount + '+' + taxvalue + '0').ToString();

                        //dt.Rows[i]["Total Invoice Value"] = TotaInvoice;
                        double TotalInvoice = taxamount + CGST + SGST + IGST + CessAmt + CessNontAdvol + Other;

                        dt.Rows[i]["Total Invoice Value"] = TotalInvoice;

                        #region[replace vehicle no]
                        vno = Convert.ToString(dt.Rows[i]["LORRYNO"]);
                        vno = vno.Replace("-", "");
                        vno = vno.Replace("/", "");
                        vno = vno.Replace(" ", "");
                        vno = vno.Replace("&", "");
                        vno = vno.Replace(".", "");
                        vno = vno.Replace("#", "");
                        vno = vno.Replace("(", "");
                        vno = vno.Replace(")", "");
                        vno = vno.Replace(":", "");
                        vno = vno.Replace("_", "");
                        vno = vno.Replace("@", "");
                        vno = vno.Replace(";", "");
                        vno = vno.Replace("=", "");
                        dt.Rows[i]["LORRYNO"] = vno;
                        #endregion

                        #region[replae to party name]
                        to_ac_name_e = Convert.ToString(dt.Rows[i]["ToOtherPartyNameForCorporat"]);
                        to_ac_name_e = to_ac_name_e.Replace("-", "");
                        to_ac_name_e = to_ac_name_e.Replace("/", "");
                        to_ac_name_e = to_ac_name_e.Replace("&", "");
                        to_ac_name_e = to_ac_name_e.Replace(".", "");
                        to_ac_name_e = to_ac_name_e.Replace("#", "");
                        to_ac_name_e = to_ac_name_e.Replace("(", "");
                        to_ac_name_e = to_ac_name_e.Replace(")", "");
                        to_ac_name_e = to_ac_name_e.Replace(":", "");
                        to_ac_name_e = to_ac_name_e.Replace("_", "");
                        to_ac_name_e = to_ac_name_e.Replace("@", "");
                        to_ac_name_e = to_ac_name_e.Replace(";", "");
                        to_ac_name_e = to_ac_name_e.Replace("=", "");
                        dt.Rows[i]["ToOtherPartyNameForCorporat"] = to_ac_name_e;
                        #endregion

                        #region[replace to address1]
                        to_address1 = Convert.ToString(dt.Rows[i]["ShipToForCorporate"]);
                        to_address1 = to_address1.Replace("-", "");
                        to_address1 = to_address1.Replace("/", "");
                        to_address1 = to_address1.Replace("&", "");
                        to_address1 = to_address1.Replace(".", "");
                        to_address1 = to_address1.Replace("#", "");
                        to_address1 = to_address1.Replace(",", "");
                        to_address1 = to_address1.Replace("(", "");
                        to_address1 = to_address1.Replace(")", "");
                        to_address1 = to_address1.Replace(":", "");
                        to_address1 = to_address1.Replace("_", "");
                        to_address1 = to_address1.Replace("@", "");
                        to_address1 = to_address1.Replace(";", "");
                        to_address1 = to_address1.Replace("=", "");
                        dt.Rows[i]["ShipToForCorporate"] = to_address1;
                        //dt.Rows[i]["To_Address2"] = to_address1;
                        #endregion

                        #region[replace to address2]
                        to_address2 = Convert.ToString(dt.Rows[i]["AdrressForCorporate"]);
                        to_address2 = to_address2.Replace("-", "");
                        to_address1 = to_address1.Replace("/", "");
                        to_address1 = to_address1.Replace("&", "");
                        to_address2 = to_address2.Replace(".", "");
                        to_address1 = to_address1.Replace("#", "");
                        to_address2 = to_address2.Replace(",", "");
                        to_address2 = to_address2.Replace("(", "");
                        to_address2 = to_address2.Replace(")", "");
                        to_address2 = to_address2.Replace(":", "");
                        to_address2 = to_address2.Replace("_", "");
                        to_address2 = to_address2.Replace("@", "");
                        to_address2 = to_address2.Replace(";", "");
                        to_address2 = to_address2.Replace("=", "");
                        to_address2 = to_address2.Replace("*", "");
                        to_address2 = to_address2.Replace(":", "");
                        to_address2 = to_address2.Replace(";", "");
                        dt.Rows[i]["AdrressForCorporate"] = to_address2;
                        //dt.Rows[i]["To_Address2"] = to_address1;
                        #endregion

                        #region[replace to place]
                        to_place = Convert.ToString(dt.Rows[i]["CityNameForCorporate"]);
                        //to_address1 = to_address1.Replace("-", "");
                        //to_address1 = to_address1.Replace("/", "");
                        //to_address1 = to_address1.Replace("&", "");
                        to_place = to_place.Replace(".", "");
                        //to_address1 = to_address1.Replace("#", "");
                        //to_address1 = to_address1.Replace(",", "");
                        to_place = to_place.Replace("(", "");
                        to_place = to_place.Replace(")", "");
                        to_place = to_place.Replace(":", "");
                        to_place = to_place.Replace("_", "");
                        to_place = to_place.Replace("@", "");
                        to_place = to_place.Replace(";", "");
                        to_place = to_place.Replace("=", "");
                        dt.Rows[i]["CityNameForCorporate"] = to_place;
                        //  dt.Rows[i]["Ship_To_State"] = dt.Rows[i]["State_Name"];
                        #endregion

                        double to_Distance;

                        #region[replace to Distance]
                        to_Distance = Convert.ToDouble(dt.Rows[i]["Distance"]);


                        dt.Rows[i]["Distance"] = to_Distance;

                        dt.Rows[i]["Vehical_Type"] = "REGULAR";
                        #endregion

                    }

                    lblSummary.Text = "E Way Bill For Corporate Sale";

                    grdAll.DataSource = dt;

                    #region[sequence comment]
                    dt.Columns["Supply Type"].SetOrdinal(0);
                    dt.Columns["Sub Type"].SetOrdinal(1);
                    dt.Columns["Doc Type"].SetOrdinal(2);
                    dt.Columns["Transaction Type"].SetOrdinal(5);

                    dt.Columns["From_OtherPartyName"].SetOrdinal(6);
                    dt.Columns["From_GSTIN"].SetOrdinal(7);
                    dt.Columns["From_Address1"].SetOrdinal(8);
                    dt.Columns["From_Address2"].SetOrdinal(9);
                    dt.Columns["From_Place"].SetOrdinal(10);
                    dt.Columns["From_Pin Code"].SetOrdinal(11);
                    dt.Columns["From_State"].SetOrdinal(12);
                    dt.Columns["Dispatch_State"].SetOrdinal(13);
                    dt.Columns["StateNameForCorporate"].SetOrdinal(21);
                    dt.Columns["Product"].SetOrdinal(22);
                    dt.Columns["Description"].SetOrdinal(23);
                    dt.Columns["HSN"].SetOrdinal(24);
                    dt.Columns["Unit"].SetOrdinal(25);
                    dt.Columns["NETQNTL"].SetOrdinal(26);
                    dt.Columns["CESS Amount"].SetOrdinal(32);
                    dt.Columns["CESS Non Advol Amount"].SetOrdinal(33);
                    dt.Columns["Other"].SetOrdinal(34);
                    dt.Columns["Total Invoice Value"].SetOrdinal(35);

                    dt.Columns["Trans Mode"].SetOrdinal(36);
                    // dt.Columns["Distance level (Km)"].SetOrdinal(31);
                    dt.Columns["Trans Name"].SetOrdinal(38);
                    dt.Columns["Trans ID"].SetOrdinal(39);
                    dt.Columns["Trans DocNo"].SetOrdinal(40);
                    dt.Columns["Trans Date"].SetOrdinal(41);
                    dt.Columns["Vehical_Type"].SetOrdinal(43);
                    dt.Columns["DO_No"].SetOrdinal(44);
                    dt.Columns["millname"].SetOrdinal(45);

                    #endregion



                    grdAll.DataBind();

                    #region[header name ]
                    grdAll.HeaderRow.Cells[3].Text = "Doc No";
                    grdAll.HeaderRow.Cells[4].Text = "Doc Date";


                    // grdAll.HeaderRow.Cells[5].Text = "From_OtherPartyName";
                    //  grdAll.HeaderRow.Cells[6].Text = "From_GSTIN";
                    //grdAll.HeaderRow.Cells[7].Text = "From_Address1";
                    //grdAll.HeaderRow.Cells[9].Text = "From_Place";
                    //grdAll.HeaderRow.Cells[10].Text = "From_Pin Code";
                    grdAll.HeaderRow.Cells[13].Text = "Dispatch State";
                    grdAll.HeaderRow.Cells[14].Text = "To_OtherPartyName";
                    grdAll.HeaderRow.Cells[15].Text = "To_GSTIN";
                    grdAll.HeaderRow.Cells[16].Text = "To_Address1";
                    grdAll.HeaderRow.Cells[17].Text = "To_Address2";
                    grdAll.HeaderRow.Cells[18].Text = "To_Place";
                    grdAll.HeaderRow.Cells[19].Text = "To_Pin Code";
                    grdAll.HeaderRow.Cells[20].Text = "To_State";
                    grdAll.HeaderRow.Cells[21].Text = "Ship To State";
                    grdAll.HeaderRow.Cells[26].Text = "Qty";
                    grdAll.HeaderRow.Cells[27].Text = "Assessable Value";
                    grdAll.HeaderRow.Cells[28].Text = "Tax Rate(C+S+I+Cess+CESS Non Advol Amount)";
                    grdAll.HeaderRow.Cells[29].Text = "CGST Amount";
                    grdAll.HeaderRow.Cells[30].Text = "SGST Amount";
                    grdAll.HeaderRow.Cells[31].Text = "IGST Amount";
                    grdAll.HeaderRow.Cells[37].Text = "Distance level (Km)";
                    grdAll.HeaderRow.Cells[42].Text = "Vehicel No";


                    #endregion





                    foreach (GridViewRow gvr in grdAll.Rows)
                    {
                        grdAll.HeaderRow.Cells[45].Visible = true;
                        grdAll.HeaderRow.Cells[46].Visible = false;
                        grdAll.HeaderRow.Cells[47].Visible = false;
                        grdAll.HeaderRow.Cells[48].Visible = false;
                        grdAll.HeaderRow.Cells[49].Visible = false;
                        grdAll.HeaderRow.Cells[50].Visible = false;
                        //grdAll.HeaderRow.Cells[47].Visible = false;

                        gvr.Cells[45].Visible = true;
                        gvr.Cells[46].Visible = false;
                        gvr.Cells[47].Visible = false;
                        gvr.Cells[48].Visible = false;
                        gvr.Cells[49].Visible = false;
                        gvr.Cells[50].Visible = false;
                        //gvr.Cells[47].Visible = false;

                    }

                    //  e.Row.Cells[7].ControlStyle.Width = new Unit("60px");
                    //  grdAll.HeaderRow.Cells[4].Width = new Unit("10px");
                    // grdAll.HeaderRow.Cells[4].ControlStyle.Width = new Unit("10px");

                }
            }

        }
        catch
        {
        }

    }

    protected void btnSale_Bill_Checking_Click(object sender, EventArgs e)
    {

        try
        {
            //  grdAll.DataBind() = null;
            //grdAll.DataSource = null;
            //grdAll.DataBind();
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            string qry = "select [doc_no],CONVERT(varchar,doc_date,103) as doc_date,"
                      + " TaxableAmount,[CGSTAmount] ,[SGSTAmount],[IGSTAmount]," +
                      "TotalGST,TOTAL"
                      + " from NT_1_qrySaleBillChecking where doc_date between '" + fromdt + "' and '" + todt
                      + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and Year_Code=" + Convert.ToInt32(Session["year"].ToString());
            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];

                    lblSummary.Text = "Sale Bill Checking";

                    grdAll.DataSource = dt;



                    grdAll.DataBind();


                }
            }

        }
        catch
        {
        }

        //btnExportToexcel_Click(this, new EventArgs());
    }

    protected void btnReturnSale_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            //string qry = "select * from (select s.doc_no as Invoice_No,s.PartyGST as PartyGSTNo,s.Ac_Code as PartyCode,s.PartyName,s.PartyStateCode,CONVERT(varchar,s.doc_date,103) as Invoice_Date,s.LORRYNO as Vehicle_No,s.Quantal as Quintal,s.rate as Rate,s.TaxableAmount, " +
            //    " s.CGSTAmount as CGST,s.SGSTAmount as SGST,s.IGSTAmount as IGST,s.Bill_Amount as Payable_Amount,s.DO_No as DO,s.CS_No from NT_1_qrySugarSaleForGSTReturn s  " +
            //    " where s.doc_date>='2017-07-01' and s.doc_date between '" + fromdt + "' and '" + todt + "' and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ) as a order by a.Invoice_Date";



            ////string qry = "select * from (select s.doc_no as Invoice_No,s.PartyGST as PartyGSTNo,s.Ac_Code as PartyCode,s.PartyName,s.PartyStateCode,CONVERT(varchar,s.doc_date,103) as Invoice_Date,s.LORRYNO as Vehicle_No,s.Quantal as Quintal,s.rate as Rate, " +
            ////   " s.CGSTAmount as CGST,s.SGSTAmount as SGST,s.IGSTAmount as IGST,s.Bill_Amount as Payable_Amount,s.DO_No as DO,s.PURCNO as OldInvNo from NT_1_qrySugarRetaileSaleSummary s  " +
            ////   " where s.doc_date>='2017-07-01' and s.doc_date between '" + fromdt + "' and '" + todt + "' and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ) as a order by a.Invoice_Date";


            string qry = "select * from (select s.doc_no as Invoice_No,s.PartyGST as PartyGSTNo,s.Ac_Code as PartyCode,s.PartyName,s.PartyStateCode,CONVERT(varchar,s.doc_date,103) as Invoice_Date,s.LORRYNO as Vehicle_No,s.Quantal as Quintal,s.rate as Rate,s.subTotal as TaxableAmount , " +
              " s.CGSTAmount as CGST,s.SGSTAmount as SGST,s.IGSTAmount as IGST,s.Bill_Amount as Payable_Amount,s.DO_No as DO,s.PURCNO as OldInvNo from NT_1_qrySugarRetaileSaleSummary s  " +
              " where s.doc_date>='2017-07-01' and s.doc_date between '" + fromdt + "' and '" + todt + "' and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ) as a order by a.Invoice_Date";


            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    //dt.Columns.Remove("RowNumber");
                    lblSummary.Text = "Return Sale Summary";

                    //DataView dvCsNo = new DataView(dt);
                    //dvCsNo.RowFilter = "CS_No<>0";

                    //DataTable dtFiltered = new DataTable();
                    //dtFiltered = dvCsNo.ToTable();
                    //foreach (DataRow drRow in dtFiltered.Rows)
                    //{
                    //    string csno = drRow["CS_No"].ToString();
                    //    string invno = drRow["Invoice_No"].ToString();
                    //    string BillToCode = clsCommon.getString("Select ISNULL(Bill_To,0) from NT_1_CarporateSale where Doc_No=" + csno + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    //    if (BillToCode != "0")
                    //    {
                    //        string billtoname = clsCommon.getString("Select Ac_Name_E from NT_1_qryAccountsList where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    //        string Gst_No = clsCommon.getString("Select Gst_No from NT_1_qryAccountsList where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    //        string placeofsupply = clsCommon.getString("Select (CONVERT(varchar,ISNULL([GSTStateCode],0))+'-'+GSTStateName) from NT_1_qryAccountsList where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    //        string statecode = clsCommon.getString("Select GSTStateCode from NT_1_qryAccountsList where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

                    //        DataRow drForUpdate = dt.Select("CS_No=" + csno + " and Invoice_No='" + invno + "'").FirstOrDefault();
                    //        drForUpdate["PartyCode"] = BillToCode;
                    //        drForUpdate["PartyName"] = billtoname;
                    //        drForUpdate["PartyGSTNo"] = Gst_No;
                    //        drForUpdate["PartyStateCode"] = statecode;
                    //    }
                    //}

                    grdAll.DataSource = dt;
                    grdAll.DataBind();

                    double totalQuintal = Convert.ToDouble(dt.Compute("SUM(Quintal)", string.Empty));
                    double totalBagBillAmount = Convert.ToDouble(dt.Compute("SUM(TaxableAmount)", string.Empty));
                    double totalCGST = Convert.ToDouble(dt.Compute("SUM(CGST)", string.Empty));
                    double totalSGST = Convert.ToDouble(dt.Compute("SUM(SGST)", string.Empty));
                    double totalIGST = Convert.ToDouble(dt.Compute("SUM(IGST)", string.Empty));
                    double totalPayable_Amount = Convert.ToDouble(dt.Compute("SUM(Payable_Amount)", string.Empty));

                    grdAll.FooterRow.Cells[7].Text = totalQuintal.ToString();
                    grdAll.FooterRow.Cells[9].Text = totalBagBillAmount.ToString();
                    grdAll.FooterRow.Cells[10].Text = totalCGST.ToString();
                    grdAll.FooterRow.Cells[11].Text = totalSGST.ToString();
                    grdAll.FooterRow.Cells[12].Text = totalIGST.ToString();
                    grdAll.FooterRow.Cells[13].Text = totalPayable_Amount.ToString();

                    //grdAll.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.Columns.RemoveAt(15);
                }
            }

        }
        catch (Exception)
        {
            throw;
        }

    }

    protected void btnpurchaseReturn_Click(object sender, EventArgs e)
    {
        try
        {
            string fromdt = txtFromDt.Text;
            string todt = txtToDt.Text;
            fromdt = DateTime.Parse(fromdt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");
            todt = DateTime.Parse(todt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("yyyy-MM-dd");

            //string qry = "select * from (select s.doc_no as Invoice_No,s.PartyGST as PartyGSTNo,s.Ac_Code as PartyCode,s.PartyName,s.PartyStateCode,CONVERT(varchar,s.doc_date,103) as Invoice_Date,s.LORRYNO as Vehicle_No,s.Quantal as Quintal,s.rate as Rate,s.TaxableAmount, " +
            //    " s.CGSTAmount as CGST,s.SGSTAmount as SGST,s.IGSTAmount as IGST,s.Bill_Amount as Payable_Amount,s.DO_No as DO,s.CS_No from NT_1_qrySugarSaleForGSTReturn s  " +
            //    " where s.doc_date>='2017-07-01' and s.doc_date between '" + fromdt + "' and '" + todt + "' and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ) as a order by a.Invoice_Date";



            //string qry = "select * from (select s.doc_no as Invoice_No,s.PartyGST as PartyGSTNo,s.Ac_Code as BillTo,s.PartyName,s.PartyStateCode,CONVERT(varchar,s.doc_date,103) as Invoice_Date,s.LORRYNO as Vehicle_No,s.Quantal as Quintal,s.rate as Rate, " +
            //   " s.CGSTAmount as CGST,s.SGSTAmount as SGST,s.IGSTAmount as IGST,s.Bill_Amount as Payable_Amount,s.PURCNO as OldInvNo from NT_1_SugarpurchaseReturnSummary s  " +
            //   " where s.doc_date>='2017-07-01' and s.doc_date between '" + fromdt + "' and '" + todt + "' and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ) as a order by a.Invoice_Date";

            ////string qry = "select * from (select s.doc_no as Invoice_No,s.PartyGST as PartyGSTNo,s.Bill_To as BillTo,s. BilltoName,s.BillToStateCode,CONVERT(varchar,s.doc_date,103) as Invoice_Date,s.LORRYNO as Vehicle_No,s.Quantal as Quintal,s.rate as Rate, " +
            ////   "s.subTotal as TaxableAmt, s.CGSTAmount as CGST,s.SGSTAmount as SGST,s.IGSTAmount as IGST,s.Bill_Amount as Payable_Amount,s.PURCNO as OldInvNo from NT_1_SugarpurchaseReturnSummary s  " +
            ////   " where s.doc_date>='2017-07-01' and s.doc_date between '" + fromdt + "' and '" + todt + "' and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ) as a order by a.Invoice_Date";


            string qry = "select * from (select s.doc_no as Invoice_No,s.PartyGSTNo as PartyGSTNo,s.Bill_To as BillTo,s. BilltoName,s.BillToStateCode,CONVERT(varchar,s.doc_date,103) as Invoice_Date,s.LORRYNO as Vehicle_No,s.Quantal as Quintal,s.rate as Rate, " +
               "s.subTotal as TaxableAmt, s.CGSTAmount as CGST,s.SGSTAmount as SGST,s.IGSTAmount as IGST,s.Bill_Amount as Payable_Amount,s.PURCNO as OldInvNo,'' as OldInvDate   from NT_1_SugarpurchaseReturnSummary s  " +
               " where s.doc_date>='2017-07-01' and s.doc_date between '" + fromdt + "' and '" + todt + "' and s.Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and s.Year_Code=" + Convert.ToInt32(Session["year"].ToString()) + " ) as a order by a.Invoice_Date";



            DataSet ds = new DataSet();
            ds = clsDAL.SimpleQuery(qry);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    //dt.Columns.Remove("RowNumber");
                    lblSummary.Text = "Return Purchase Summary";

                    //DataView dvCsNo = new DataView(dt);
                    //dvCsNo.RowFilter = "CS_No<>0";

                    //DataTable dtFiltered = new DataTable();
                    //dtFiltered = dvCsNo.ToTable();
                    //foreach (DataRow drRow in dtFiltered.Rows)
                    //{
                    //    string csno = drRow["CS_No"].ToString();
                    //    string invno = drRow["Invoice_No"].ToString();
                    //    string BillToCode = clsCommon.getString("Select ISNULL(Bill_To,0) from NT_1_CarporateSale where Doc_No=" + csno + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    //    if (BillToCode != "0")
                    //    {
                    //        string billtoname = clsCommon.getString("Select Ac_Name_E from NT_1_qryAccountsList where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    //        string Gst_No = clsCommon.getString("Select Gst_No from NT_1_qryAccountsList where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    //        string placeofsupply = clsCommon.getString("Select (CONVERT(varchar,ISNULL([GSTStateCode],0))+'-'+GSTStateName) from NT_1_qryAccountsList where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");
                    //        string statecode = clsCommon.getString("Select GSTStateCode from NT_1_qryAccountsList where Ac_Code=" + BillToCode + " and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "");

                    //        DataRow drForUpdate = dt.Select("CS_No=" + csno + " and Invoice_No='" + invno + "'").FirstOrDefault();
                    //        drForUpdate["PartyCode"] = BillToCode;
                    //        drForUpdate["PartyName"] = billtoname;
                    //        drForUpdate["PartyGSTNo"] = Gst_No;
                    //        drForUpdate["PartyStateCode"] = statecode;
                    //    }
                    //}
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            int invno = Convert.ToInt32(dt.Rows[i]["OldInvNo"].ToString());
                            string invdate;
                            if (invno != 0)
                            {
                                invdate = clsCommon.getString("select convert(varchar(10),doc_date,103) as doc_date from NT_1_SugarSale where doc_no='" + invno + "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + "  and Year_Code=" + Convert.ToInt32(Session["year"].ToString()));
                                dt.Rows[i]["OldInvDate"] = invdate;
                            }
                            grdAll.DataSource = dt;
                            grdAll.DataBind();
                        }


                    }
                    //grdAll.DataSource = dt;
                    //grdAll.DataBind();

                    double totalQuintal = Convert.ToDouble(dt.Compute("SUM(Quintal)", string.Empty));
                    //double totalBagBillAmount = Convert.ToDouble(dt.Compute("SUM(TaxableAmount)", string.Empty));

                    double totalTaxAmt = Convert.ToDouble(dt.Compute("SUM(TaxableAmt)", string.Empty));
                    double totalCGST = Convert.ToDouble(dt.Compute("SUM(CGST)", string.Empty));
                    double totalSGST = Convert.ToDouble(dt.Compute("SUM(SGST)", string.Empty));
                    double totalIGST = Convert.ToDouble(dt.Compute("SUM(IGST)", string.Empty));
                    double totalPayable_Amount = Convert.ToDouble(dt.Compute("SUM(Payable_Amount)", string.Empty));

                    grdAll.FooterRow.Cells[7].Text = totalQuintal.ToString();
                    // grdAll.FooterRow.Cells[9].Text = totalBagBillAmount.ToString();


                    //grdAll.FooterRow.Cells[9].Text = totalCGST.ToString();
                    //grdAll.FooterRow.Cells[10].Text = totalSGST.ToString();
                    //grdAll.FooterRow.Cells[11].Text = totalIGST.ToString();
                    //grdAll.FooterRow.Cells[12].Text = totalPayable_Amount.ToString();

                    grdAll.FooterRow.Cells[9].Text = totalTaxAmt.ToString();
                    grdAll.FooterRow.Cells[10].Text = totalCGST.ToString();
                    grdAll.FooterRow.Cells[11].Text = totalSGST.ToString();
                    grdAll.FooterRow.Cells[12].Text = totalIGST.ToString();
                    grdAll.FooterRow.Cells[13].Text = totalPayable_Amount.ToString();


                    //grdAll.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[10].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[11].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[12].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[13].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.FooterRow.Cells[14].HorizontalAlign = HorizontalAlign.Right;
                    //grdAll.Columns.RemoveAt(15);
                }
            }

        }
        catch (Exception)
        {
            throw;
        }

    }
}
