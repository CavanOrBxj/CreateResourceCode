using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CreateResourceCode
{
    public partial class Form1 : Form
    {

        private IniFiles serverini;
        private string FirstLocaltion;
        private string Benindregion;
        public static dbAccess dba;

        public List<OrganizationItem> dd;
        public Form1()
        {
            InitializeComponent();
            Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serverini = new IniFiles(@Application.StartupPath + "\\Config.ini");
            FirstLocaltion= serverini.ReadValue("ResourceCodeParam", "FirstLocaltion");
            Benindregion = serverini.ReadValue("ResourceCodeParam", "Benindregion");

            dba = new dbAccess();
            dba.conn.ConnectionString = GetConnectString();

            dd = new List<OrganizationItem>();
        }


        private String GetConnectString()
        {
            string sReturn;
            string sPass;
            string sServer = serverini.ReadValue("Database", "ServerName");
            string sUser = serverini.ReadValue("Database", "LogID");
            string sDataBase = serverini.ReadValue("Database", "DataBase");
            sPass = serverini.ReadValue("Database", "LogPass");
            if (sPass == "")
                sPass = "tuners2012";
            sReturn = "server = " + sServer +
                   ";UID = " + sUser +
                    ";Password =" + sPass +
                     ";DataBase = " + sDataBase.Trim() + ";"
                     + "MultipleActiveResultSets=True";

            return sReturn;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            string MediaSql = "select SRV_ID, SRV_ADDRESS FROM SRV";
            DataTable dtMedia = dba.getQueryInfoBySQL(MediaSql);
            if (dtMedia != null && dtMedia.Rows.Count > 0)
            {
                for (int i = 0; i < dtMedia.Rows.Count; i++)
                {
                  string[]  tmp= dtMedia.Rows[i]["SRV_ADDRESS"].ToString().Split('.');
                  string organizationname=  tmp[tmp.Length - 1];

                    if (dic.ContainsKey(organizationname))
                    {
                        dic[organizationname].Add(dtMedia.Rows[i]["SRV_ID"].ToString());
                    }
                    else
                    {
                        List<string> srv_id_list = new List<string>();
                        srv_id_list.Add(dtMedia.Rows[i]["SRV_ID"].ToString());
                        dic.Add(organizationname, srv_id_list);
                    }

                }
              
            }
            foreach (var item in dic)
            {
                string organizationname = item.Key;

                string strSql = string.Format("select GB_CODE from Organization where ORG_DETAIL='{0}'", organizationname);
                DataTable dtGBCODE = dba.getQueryInfoBySQL(strSql);
                string GB_CODE = "";
                if (dtGBCODE.Rows.Count>0)
                {
                    GB_CODE= dtGBCODE.Rows[0]["GB_CODE"].ToString();
                }


                for (int i = 0; i < item.Value.Count; i++)
                {
                    string SRV_ID = item.Value[i];
                    string SRV_LOGICAL_CODE_GB = FirstLocaltion + GB_CODE + Benindregion + (i + 1).ToString().PadLeft(2, '0');
                    string strSql11 = string.Format("update SRV set SRV.SRV_LOGICAL_CODE_GB = '{0}' where SRV_ID='{1}'", SRV_LOGICAL_CODE_GB, SRV_ID);
                    dba.UpdateDbBySQL(strSql11);
                }
            }
        }
    }
}
