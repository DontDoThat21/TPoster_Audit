using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System;
using ADODB;
using System.Reflection;

namespace TPoster_Auditor
{
    /// <summary>
    /// Summary description for TPoster_Audit
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TPoster_Audit : System.Web.Services.WebService
    {
        ADODB.Connection conn = new ADODB.Connection
        {
            //ConnectionString = "User ID=annotator;Password=GPJ123;Data Source=E810DB"            
            ConnectionString = "DSN=JDE910;UID=ANNOTATOR;PWD=GPJ123"
        };

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }


        void Main(string[] args)
        {
            SetAuditLogin(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        [WebMethod]
        public void SetAuditLogin(string appVer)
        {
            string pcName = Environment.MachineName.Substring(0, 8);
            string pcOSVer = Environment.OSVersion.ToString();
            string pcPlatForm = Environment.OSVersion.Platform.ToString();
            int osMajor = Environment.OSVersion.Version.Major;
            int osMinor = Environment.OSVersion.Version.Minor;
            bool bitBool = Environment.Is64BitOperatingSystem; int bit = -1;
            string appVersion = appVer;

            if (bitBool) {
                bit = 64;
            }
            else {
                bit = 32;
            }

            long seqNum = GetSeq("annotator.GPJ_AUDIT_SEQ");
            int month = DateTime.Now.Month; int day = DateTime.Now.Day; int year = DateTime.Now.Year;

            conn.Open();
            string sqlCommand = $"INSERT INTO GPJ_APP_AUDIT VALUES ({seqNum}, '{pcName}', SYSDATE, 'GPJ', '{pcName}', '{pcOSVer}', '{pcPlatForm}', {osMajor}, {osMinor}, " +
                $"{bit.ToString()}, 'TWITTER_POSTER', '{appVersion}', 'TWITPSTR', SYSDATE, '{TimeZone.CurrentTimeZone.StandardName}');";

            ADODB.Recordset rst = new ADODB.Recordset();
            object rowsAffected;
            rst = conn.Execute(sqlCommand, out rowsAffected);
            conn.Close();
        }

        public void GetAuditLogin()
        {

        }

        public void GetLastLogin(string userName)
        {
            // sql get max audit id for given user name? Or machine name? Same val.. eniron.machone.name...
        }

        public long GetSeq(string seq)
        {
            ADODB.Recordset rst = new ADODB.Recordset();
            object rowsAffected;
            conn.Open();
            rst = conn.Execute("SELECT " + seq + ".NEXTVAL FROM DUAL", out rowsAffected);
            long sequence = long.Parse(rst.Fields[0].Value.ToString());
            conn.Close();
            return sequence;
        }
    }
}

namespace testDebugApp
{
    class Program
    {
    }
}
