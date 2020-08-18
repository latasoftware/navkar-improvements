using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Configuration;

public partial class Sugar_Utility_pgeUploadSignature : System.Web.UI.Page
{
    string tblPrefix = string.Empty;
    string tblHead = string.Empty;
    string isAuthenticate = string.Empty;
    string user = string.Empty;
    string cs = string.Empty;
    static WebControl objAsp = null;
    MySqlConnection con = null;
    MySqlCommand cmd = null;
    MySqlTransaction myTran = null;


    protected void Page_Load(object sender, EventArgs e)
    {
        cs = ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
        con = new MySqlConnection(cs);
        tblPrefix = Session["tblPrefix"].ToString();
        tblHead = "tblsign";
        user = Session["user"].ToString();
        if (!Page.IsPostBack)
        {
            isAuthenticate = Security.Authenticate(tblPrefix, user);
            string User_Type = clsCommon.getString("Select User_Type from tblUser WHERE User_Name='" + user + "'");
            if (isAuthenticate == "1" || User_Type == "A")
            {

            }
            else
            {
                Response.Redirect("~/UnAuthorized/Unauthorized_User.aspx", false);
            }
        }
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            string fileName = string.Empty;
            string qry = "";
            #region - declearation
            string retValue = string.Empty;
            string strRev = string.Empty;
            int Company_Code = Convert.ToInt32(Session["Company_Code"].ToString());
            string op = string.Empty;
            string returnmaxno = string.Empty;
            #endregion

            #region-Save Part
            string path = string.Empty;
            path = Server.MapPath("");
            path = path.Replace("Utility", "Images");
            string imgname = clsCommon.getString("Select ImageName from tblsign where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and ImageOrLogo='S'");
            imgname = path + "\\" + imgname;
            if (File.Exists(imgname))
            {
                File.Delete(imgname);
            }
            if (fu1.HasFile)
            {
                fileName = Path.GetFileName(fu1.PostedFile.FileName);
                fu1.SaveAs(Server.MapPath("~/Sugar/Images/" + fileName));
               
            }
            string id = clsCommon.getString("Select ID from tblsign where Company_Code=" + Convert.ToInt32(Session["Company_Code"].ToString()) + " and ImageOrLogo='S'");
           
            // string fullPath = path + "\\" + fileName;
            if (id == "0")
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                qry = "insert into tblsign (ID,ImageName,ImagePath,Company_Code,ImageOrLogo) values (1,'" + fileName + "','" + fileName + "','" + Convert.ToInt32(Session["Company_Code"].ToString()) + "','S')";
                ds = clsDAL.SimpleQuery(qry);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Uploaded Image Successfully!')", true);
            }
            else
            {
                
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                qry = "update tblsign set ImageName='" + fileName + "',ImagePath='" + fileName + "', ImageOrLogo='S' where Company_Code='" + Convert.ToInt32(Session["Company_Code"].ToString()) + "' and ID=1 and ImageOrLogo= 'S'";
                ds = clsDAL.SimpleQuery(qry);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Uploaded Sign Successfully!')", true);
            }
            #endregion

        }
        catch (Exception)
        {

            throw;
        }
    }

}