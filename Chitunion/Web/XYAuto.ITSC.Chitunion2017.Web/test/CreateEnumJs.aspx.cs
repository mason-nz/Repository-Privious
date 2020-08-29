using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

namespace XYAuto.ITSC.Chitunion2017.Web.test
{
    public partial class CreateEnumJs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            createAreaJs2();
        }

        /// <summary>
        /// 省、市、区县
        /// </summary>
        private void createAreaJs2()
        {
            string path = Server.MapPath("js/Enum/Area2.js");

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sr = new StreamWriter(fs, System.Text.Encoding.UTF8);//写文件流
            //StreamWriter sr = File.CreateText(path);

            //QueryMainBrand queryMainBrand = new QueryMainBrand();
            //queryMainBrand.Pid = "0";
            int o = 0;
            DataTable dt = BLL.AreaInfo.Instance.GetAreaByPid(0);
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.DefaultView.RowFilter = "areaname<>'全国'";
                dt = dt.DefaultView.ToTable();
                sr.Write("var JSonData={masterArea:[");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string prefix_Prov = dt.Rows[i]["AbbrName"].ToString().Substring(0, 1).ToUpper();
                    sr.Write("{\"pid\":\"-1\",\"id\":\"" + dt.Rows[i]["areaid"] + "\",\"name\":\"" + dt.Rows[i]["areaname"] + "\",\"szm\":\"" + prefix_Prov + "\",subArea:[");

                    //queryMainBrand.Pid = dt.Rows[i]["areaid"].ToString();
                    DataTable dtSub = BLL.AreaInfo.Instance.GetAreaByPid(int.Parse(dt.Rows[i]["areaid"].ToString()));
                    if (dtSub != null && dtSub.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtSub.Rows.Count; j++)
                        {
                            string prefix_City = dtSub.Rows[j]["AbbrName"].ToString().Substring(0, 1).ToUpper();
                            sr.Write("{\"pid\":\"" + dtSub.Rows[j]["pid"] + "\",\"id\":\"" + dtSub.Rows[j]["areaid"] + "\",\"name\":\"" + dtSub.Rows[j]["areaname"] + "\",\"szm\":\"" + prefix_City + "\",subArea2:[");

                            //区县
                            //queryMainBrand.Pid = dtSub.Rows[j]["areaid"].ToString();
                            DataTable dtSub2 = BLL.AreaInfo.Instance.GetAreaByPid(int.Parse(dtSub.Rows[j]["areaid"].ToString()));
                            if (dtSub2 != null && dtSub2.Rows.Count > 0)
                            {
                                for (int k = 0; k < dtSub2.Rows.Count; k++)
                                {
                                    string prefix_County = dtSub2.Rows[k]["AbbrName"].ToString().Substring(0, 1).ToUpper();
                                    sr.Write("{\"pid\":\"" + dtSub2.Rows[k]["pid"] + "\",\"id\":\"" + dtSub2.Rows[k]["areaid"] + "\",\"name\":\"" + dtSub2.Rows[k]["areaname"] + "\",\"szm\":\"" + prefix_County + "\"");
                                    if (k == (dtSub2.Rows.Count - 1))
                                    {
                                        sr.Write("}");
                                    }
                                    else
                                    {
                                        sr.Write("},");
                                    }
                                }
                            }

                            if (j == (dtSub.Rows.Count - 1))
                            {
                                sr.Write("]}");
                            }
                            else
                            {
                                sr.Write("]},");
                            }
                        }
                    }
                    if (i == (dt.Rows.Count - 1))
                    {
                        sr.Write("]}");
                    }
                    else
                    {
                        sr.Write("]},");
                    }
                }

                sr.Write("]};");
            }
            sr.Close();
        }
    }
}