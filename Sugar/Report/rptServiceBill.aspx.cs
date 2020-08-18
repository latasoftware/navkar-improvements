using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Reporting;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using System.IO;
using System.Configuration;
using System.Drawing.Printing;
//using System.Printing;
using System.Net;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mime;
using iTextSharp.tool.xml;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using MySql.Data.MySqlClient;


public partial class Sugar_Report_rptServiceBill : System.Web.UI.Page
{
    int billno;
    int company_code;
    int year_code;
    string FromDt = string.Empty;
    string ToDt = string.Empty;
    string ac_code;
    string pur_no;
    string AcType = string.Empty;
    string mail = string.Empty;
    string AL1 = string.Empty;
    string AL2 = string.Empty;
    string AL3 = string.Empty;
    string AL4 = string.Empty;
    string other = string.Empty;
    ReportDocument rprt1 = new ReportDocument();
    ReportDocument rprt2 = new ReportDocument();
    ReportDocument rpt = new ReportDocument();
    string company_name = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            billno = Convert.ToInt32(Request.QueryString["billno"]);
            FromDt = Request.QueryString["FromDt"];
            ToDt = Request.QueryString["ToDt"];

            ac_code = Request.QueryString["accode"];
            AcType = Request.QueryString["AcType"];
            pur_no = Request.QueryString["utr_no"];


            //  string dateto = DateTime.Parse(ToDt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            //     string dateto = DateTime.Parse(ToDt, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
            int company_code = Convert.ToInt32(Session["Company_Code"].ToString());
            int year_code = Convert.ToInt32(Session["year"].ToString());
            string Address = Convert.ToString(Session["address"].ToString());
            //DataTable dt = new DataTable();
            company_name = Session["Company_Name"].ToString();
           // int counts = Convert.ToInt16(clsCommon.getString("SELECT Amount as Amount from  qryrentbillheaddetail "));

         
            DataTable dt = GetData();
            string TotalAmount = dt.Rows[0]["Final_Amount"].ToString();
            string docno = dt.Rows[0]["DOC_NO"].ToString();
            docno = docno.PadLeft(6, '0');

            string inWords = clsNoToWord.ctgword(TotalAmount);
            MySqlDataAdapter da = new MySqlDataAdapter();

            rpt.Load(Server.MapPath("CryServiceBill.rpt"));
            rpt.SetDataSource(dt);

            CryServiceBill.ReportSource = rpt;
            //  rpt.DataDefinition.FormulaFields["fromto"].Text = "\"UTR report From " + FromDt + " To " + ToDt + "\"";
            //   rpt.DataDefinition.FormulaFields["address"].Text = "\"" + address + "\"";
            rpt.DataDefinition.FormulaFields["Address"].Text = "\"" + Address + "\"";
            rpt.DataDefinition.FormulaFields["companyname"].Text = "\"" + company_name + "\"";
            rpt.DataDefinition.FormulaFields["word"].Text = "\"" + inWords + "\"";


            string imagepath = clsCommon.getString("select ImagePath from tblsign where ImageOrLogo='S' and Company_Code='"
                 + Session["Company_Code"].ToString() + "'");
            String path = Server.MapPath("") + imagepath;

            string imagepath1 = path.Replace("Sugar", "Images");
            imagepath = imagepath1.Replace("Report", "");

            rpt.DataDefinition.FormulaFields["img"].Text = "\"" + imagepath + "\"";
            rpt.DataDefinition.FormulaFields["AL1"].Text = "\"" + AL1 + "\"";
            rpt.DataDefinition.FormulaFields["AL2"].Text = "\"" + AL2 + "\"";
            rpt.DataDefinition.FormulaFields["AL3"].Text = "\"" + AL3 + "\"";
            rpt.DataDefinition.FormulaFields["Al4"].Text = "\"" + AL4 + "\"";
            rpt.DataDefinition.FormulaFields["other"].Text = "\"" + other + "\"";
            rpt.DataDefinition.FormulaFields["docno"].Text = "\"" + "SB" + docno + "\"";

            CryServiceBill.RefreshReport();






        }
        catch (Exception)
        {

            throw;
        }

    }
    private DataTable GetData()
    {
        try
        {

            DataTable dt = new DataTable();
            string strcon = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(strcon))
            {



                MySqlCommand cmd = new MySqlCommand("select * from qryrentbillheaddetail where  Doc_No ='" + billno + "'", con);
                cmd.CommandType = CommandType.Text;
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                sda.Fill(dt);

            }
            string qry = "select * from tblvoucherheadaddress where Company_Code='" + Session["Company_Code"].ToString() + "'";
            DataSet ds = clsDAL.SimpleQuery(qry);
            if (ds != null)
            {
                DataTable dt1 = ds.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    AL1 = dt1.Rows[0]["AL1"].ToString();
                    AL2 = dt1.Rows[0]["AL2"].ToString();
                    AL3 = dt1.Rows[0]["AL3"].ToString();
                    AL4 = dt1.Rows[0]["AL4"].ToString();
                    other = dt1.Rows[0]["Other"].ToString();
                }
            }
         
            return dt;
        }
        catch
        {
            return null;
        }
    }

    protected void Print(object sender, EventArgs e)
    {

        // Refresh Report.
        rpt.Refresh();
        //PrintDialog myPrintDialog = new PrintDialog();
        rpt.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;

        // Set Paper Size.
        rpt.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
        // CrystalDecisions.Shared.ExportFormatType to change the format i.e. Excel, Word, PDF
        //crystalReport.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, "CustomerDetails");
        //PrintDialog dialog1 = new PrintDialog();


        rpt.PrintOptions.PrinterName = GetDefaultPrinter();
        //CrystalDecisions.Shared.PageMargins objPageMargins;
        //objPageMargins = rprt1.PrintOptions.PageMargins;
        //objPageMargins.bottomMargin = 100;
        //objPageMargins.leftMargin = 100;
        //objPageMargins.rightMargin = 100;
        //objPageMargins.topMargin = 100;
        //rprt1.PrintOptions.ApplyPageMargins(objPageMargins);
        //rprt1.PrintOptions.PrinterName = GetDefaultPrinter(); ;

        rpt.PrintToPrinter(1, false, 0, 0);

        //rprt1.PrintToPrinter(1, true, 0, 0);

    }


    private string GetDefaultPrinter()
    {
        PrinterSettings settings = new PrinterSettings();

        foreach (string printer in PrinterSettings.InstalledPrinters)
        {
            settings.PrinterName = printer;
            if (settings.IsDefaultPrinter)
            {
                return printer;
            }
        }
        return string.Empty;
    }

    protected void btnPDF_Click(object sender, EventArgs e)
    {
        try
        {
            // string filepath=@"D:\pdffiles\cryChequePrinting.pdf";
            string filepath = @"D:\pdffiles";
            if (!System.IO.Directory.Exists(filepath))
            {
                System.IO.Directory.CreateDirectory("D:\\pdffiles");
            }
            string pdfname = filepath + "\\UTR.pdf";

            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfname);

            //open PDF File
            //System.Diagnostics.Process.Start(pdfname);
            // string FilePath = Server.MapPath("javascript1-sample.pdf");

            WebClient User = new WebClient();

            Byte[] FileBuffer = User.DownloadData(pdfname);

            if (FileBuffer != null)
            {

                Response.ContentType = "application/pdf";

                Response.AddHeader("content-length", FileBuffer.Length.ToString());

                Response.BinaryWrite(FileBuffer);

            }
        }
        catch (Exception e1)
        {
            Response.Write("PDF err:" + e1);
            return;
        }
        //   Response.Write("<script>alert('PDF successfully Generated');</script>");

    }
    protected void btnMail_Click(object sender, EventArgs e)
    {
        try
        {
            // string filepath = @"D:\ashwini\bhavani10012019\accowebBhavaniNew\PAN\cryChequePrinting.pdf";
            string filepath = @"D:\pdffiles";
            if (!System.IO.Directory.Exists(filepath))
            {
                System.IO.Directory.CreateDirectory("D:\\pdffiles");
            }
            string pdfname = filepath + "\\UTR.pdf";

            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfname);

            if (txtEmail.Text != string.Empty)
            {
                //string fileName = "Saudapending.pdf";
                //string filepath1 = "~/PAN/" + fileName;

                mail = txtEmail.Text;

                ContentType contentType = new ContentType();
                contentType.MediaType = MediaTypeNames.Application.Pdf;
                contentType.Name = "UTR";
                Attachment attachment = new Attachment(pdfname, contentType);

                string mailFrom = Session["EmailId"].ToString();
                string smtpPort = "587";
                string emailPassword = Session["EmailPassword"].ToString();
                MailMessage msg = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.Host = clsGV.Email_Address;
                msg.From = new MailAddress(mailFrom);
                msg.To.Add(mail);
                msg.Body = "UTR";
                msg.Attachments.Add(attachment);
                msg.IsBodyHtml = true;
                msg.Subject = "DOC.No:";
                //msg.IsBodyHtml = true;
                if (smtpPort != string.Empty)
                {
                    SmtpServer.Port = Convert.ToInt32(smtpPort);
                }
                SmtpServer.EnableSsl = true;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, emailPassword);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object k,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
                SmtpServer.Send(msg);
                attachment.Dispose();
                if (File.Exists(pdfname))
                {
                    File.Delete(pdfname);
                }
            }


        }
        catch (Exception e1)
        {
            Response.Write("Mail err:" + e1);
            return;
        }
        Response.Write("<script>alert('Mail Send successfully');</script>");

    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        //rprt1.Close();
        //rprt1.Clone();
        //rprt1.Dispose();
        //GC.Collect();
        this.CryServiceBill.ReportSource = null;

        CryServiceBill.Dispose();

        if (rpt != null)
        {

            rpt.Close();

            rpt.Dispose();

            rpt = null;

        }

        GC.Collect();

        GC.WaitForPendingFinalizers();
    }
}