using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using XYAuto.Utils.Data;

namespace XYAuto.BUOC.ChiTuData2017.Dal.Distribute
{
    //点击明细，转发明细
    public partial class MaterielOperateDetailed : DataBase
    {
        public static readonly MaterielOperateDetailed Instance = new MaterielOperateDetailed();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Insert(Entities.Distribute.MaterielOperateDetailed entity)
        {
            var strSql = new StringBuilder();
            strSql.Append("insert into Materiel_OperateDetailed(");
            strSql.Append("MaterielId,StatisticsId,DicId,ClickType,ClickValue,CreateTime,Status,ConentType");
            strSql.Append(") values (");
            strSql.Append("@MaterielId,@StatisticsId,@DicId,@ClickType,@ClickValue,@CreateTime,@Status,@ConentType");
            strSql.Append(") ");
            strSql.Append(";select SCOPE_IDENTITY()");
            var parameters = new SqlParameter[]{
                        new SqlParameter("@MaterielId",entity.MaterielId),
                        new SqlParameter("@StatisticsId",entity.StatisticsId),
                        new SqlParameter("@DicId",entity.DicId),
                        new SqlParameter("@ClickType",entity.ClickType),
                        new SqlParameter("@ClickValue",entity.ClickValue),
                        new SqlParameter("@CreateTime",entity.CreateTime),
                        new SqlParameter("@Status",entity.Status),
                        new SqlParameter("@ConentType",entity.ConentType),
                        };

            var obj = SqlHelper.ExecuteScalar(CONNECTIONSTRINGS, CommandType.Text, strSql.ToString(), parameters);
            return obj == null ? 0 :

                Convert.ToInt32(obj);
        }

        public int Insert(List<Entities.Distribute.MaterielOperateDetailed> list)
        {
            if (!list.Any())
            {
                return 1;
            }
            var sbCode = new StringBuilder();
            sbCode.AppendFormat(@"
                    INSERT INTO DBO.Materiel_OperateDetailed
                            ( MaterielId ,
                              StatisticsId ,
                              DicId ,
                              ClickType ,
                              ClickValue ,
                              CreateTime ,
                              Status ,
                              ConentType
                            ) VALUES
                    ");
            list.ForEach(item =>
            {
                sbCode.AppendFormat($@"( {item.MaterielId},{item.StatisticsId},{item.DicId},{item.ClickType},{item.ClickValue},getdate(),0,{item.ConentType}),");
            });
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sbCode.ToString().Trim(','));
        }
    }
}