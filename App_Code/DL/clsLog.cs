using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Configuration;
using MySql.Data.MySqlClient;
public static class clsLog
{
    public static void Publish(Exception ex)
    {
        try
        {
            String strMessage = ex.Message;
            String strInnerException = Convert.ToString(ex.InnerException);
            String strExceptionId = "0";
            String PageName = clsAdvanceUtility.GetCurrentPageName();
            string cs = string.Empty;
            MySqlConnection con = null;
            cs = ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString;
            con = new MySqlConnection(cs);
            using (clsDataProvider objDataProvider = new clsDataProvider())
            {
                con.Open();

                MySqlCommand sqlcmd = new MySqlCommand("SP_Exception", con);
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("ExceptionId", strExceptionId);
                sqlcmd.Parameters.AddWithValue("ExceptionName", strInnerException);
                sqlcmd.Parameters.AddWithValue("ExceptionDetails", strMessage);
                sqlcmd.Parameters.AddWithValue("PageName", PageName);
                sqlcmd.ExecuteNonQuery();
                con.Close();
            }
        }
        catch
        {
        }
    }
}