﻿using System;
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

public partial class Sugar_Report_rptDeliveryOrderParty : System.Web.UI.Page
{
    string tenderno = string.Empty;
    int billno;
    int company_code;
    int year_code;
    string FromDt = string.Empty;
    string ToDt = string.Empty;
    string ac_code;
    string utr_no;
    string AcType = string.Empty;
    string mail = string.Empty;
    string doc_no = string.Empty;
    ReportDocument rprt1 = new ReportDocument();
    ReportDocument rprt2 = new ReportDocument();
    ReportDocument rpt = new ReportDocument();
    string company_name = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {
                string mill=Request.QueryString["mill"];
                txtEmail.Text = clsCommon.getString("Select Email_Id from qrymstaccountmaster where  Ac_Code='" + mill+ "' and Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()));
            }

            tenderno = Request.QueryString["tenderno"];
            company_name = Session["Company_Name"].ToString();
            doc_no = Request.QueryString["do_no"];
            DataTable dt = GetData();
            MySqlDataAdapter da = new MySqlDataAdapter();
            rpt.Load(Server.MapPath("cryDeliveryOrderParty.rpt"));
            rpt.SetDataSource(dt);

            string tenderDate = clsCommon.getString("select Tender_DateConverted from qrytenderhead where Tender_No='" + tenderno + "' and Company_Code='" + Session["Company_Code"] + "' " +
                " and Year_Code='" + Session["year"].ToString() + "'");
            cryDeliveryOrderParty.ReportSource = rpt;
            rpt.DataDefinition.FormulaFields["tenderDate"].Text = "\"" + tenderDate + "\"";

          string  imagepath = clsCommon.getString("select ImagePath from tblsign where ImageOrLogo='S' and Company_Code='"
           + Session["Company_Code"].ToString() + "'");
            String path = Server.MapPath("") + imagepath;

            string imagepath1 = path.Replace("Sugar", "Images");
            imagepath = imagepath1.Replace("Report", "");

            rpt.DataDefinition.FormulaFields["img"].Text = "\"" + imagepath + "\"";
            // rprt.DataDefinition.FormulaFields["todate"].Text = datefrom;
            //rprt.DataDefinition.FormulaFields["Date"].Text = "\"Reverse Charge Report From " + datefrom + " To " + dateto + "\"";
            cryDeliveryOrderParty.RefreshReport();

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
                MySqlCommand cmd = new MySqlCommand("select * from qrydohead where  doid=" + doc_no + "", con);
                cmd.CommandType = CommandType.Text;
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            return dt;
        }
        catch
        {
            return null;
        }
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
            string pdfname = filepath + "\\Do Party.pdf";

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
            string pdfname = filepath + "\\DOParty.pdf";

            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, pdfname);

            if (txtEmail.Text != string.Empty)
            {
                //string fileName = "Saudapending.pdf";
                //string filepath1 = "~/PAN/" + fileName;

                mail = txtEmail.Text;

                ContentType contentType = new ContentType();
                contentType.MediaType = MediaTypeNames.Application.Pdf;
                contentType.Name = "DOParty";
                Attachment attachment = new Attachment(pdfname, contentType);

                string mailFrom = Session["EmailId"].ToString();
                string smtpPort = "587";
                string emailPassword = Session["EmailPassword"].ToString();
                MailMessage msg = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com", 587);
                SmtpServer.Host = clsGV.Email_Address;
                msg.From = new MailAddress(mailFrom);
                msg.To.Add(mail);
                msg.Body = "DoParty";
                msg.Attachments.Add(attachment);
                msg.IsBodyHtml = true;
                msg.Subject = "DOC.No:"+doc_no+"";
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
        this.cryDeliveryOrderParty.ReportSource = null;

        cryDeliveryOrderParty.Dispose();

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