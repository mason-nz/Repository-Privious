using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.IP2017.BLL.BatchLabelHistory
{
    public class BatchLabelHistory
    {
        public static readonly BatchLabelHistory Instance = new BatchLabelHistory();
        public int Insert(Entities.BatchLabelHistory.BatchLabelHistory entity)
        {
            return Dal.BatchLabelHistory.BatchLabelHistory.Instance.Insert(entity);
        }

        public void InsertBatch(BatchMedia.DTO.RequestDto.V1_2_4.ReqBatchMediaSubmitDto req, int batchMediaID, int currentUserID)
        {
            var sbSql = new StringBuilder();

            //if (req.Category != null)
            //{
            //    foreach (var itemCategory in req.Category)
            //    {
            //        sbSql.Append(GetInertBatchSql(batchMediaID, itemCategory.DictId, (int)Entities.ENUM.ENUM.EnumLabelType.分类, itemCategory.DictName, currentUserID));
            //    }
            //}
            if (req.MarketScene != null)
            {
                foreach (var itemCategory in req.MarketScene)
                {
                    sbSql.Append(GetInertBatchSql(batchMediaID, itemCategory.DictId, (int)Entities.ENUM.ENUM.EnumLabelType.市场场景, itemCategory.DictName, currentUserID));
                }
            }
            if (req.DistributeScene != null)
            {
                foreach (var itemCategory in req.DistributeScene)
                {
                    sbSql.Append(GetInertBatchSql(batchMediaID, itemCategory.DictId, (int)Entities.ENUM.ENUM.EnumLabelType.分发场景, itemCategory.DictName, currentUserID));
                }
            }
            if (!string.IsNullOrEmpty(sbSql.ToString()))
                Dal.BatchLabelHistory.BatchLabelHistory.Instance.InsertBatch(sbSql.ToString());

            foreach (var itemIPLabel in req.IPLabel)
            {
                int ipLabelID = Insert(
                    new Entities.BatchLabelHistory.BatchLabelHistory()
                    {
                        BatchMediaID = batchMediaID,
                        TitleID = itemIPLabel.DictId,
                        Name = itemIPLabel.DictName,
                        Type = (int)Entities.ENUM.ENUM.EnumLabelType.IP,
                        CreateUserID = currentUserID
                    }
                    );

                foreach (var itemSubIP in itemIPLabel.SubIP)
                {
                    int subIPID = IPSubLabel.IPSubLabel.Instance.Insert(new Entities.IPSubLabel.IPSubLabel()
                    {
                        LabelID = ipLabelID,
                        TitleID = itemSubIP.DictId,
                        CreateUserID = currentUserID,
                        BatchMediaID = batchMediaID
                    });

                    foreach (var itemSonIPLabel in itemSubIP.Label)
                    {
                        SonIPLabel.SonIPLabel.Instance.Insert(new Entities.SonIPLabel.SonIPLabel()
                        {
                            SubIPID = subIPID,
                            Name = itemSonIPLabel.DictName,
                            CreateUserID = currentUserID,
                            BatchMediaID = batchMediaID
                        });
                    }
                }
            }
        }
        protected string GetInertBatchSql(int batchMediaID, int dictID, int type, string dictName, int currentUserID)
        {
            return $@"
                    INSERT  dbo.BatchLabelHistory
                            ( BatchMediaID ,
                                TitleID ,
                                Type ,
                                Name ,
                                CreateTime ,
                                CreateUserID
                            )
                    VALUES  ( {batchMediaID} , -- BatchMediaID - int
                                {dictID} , -- TitleID - int
                                {type} , -- Type - INT
                                '{dictName}' , -- Name - varchar(100)
                                GETDATE() , -- CreateTime - datetime
                                {currentUserID}  -- CreateUserID - int
                            )";
        }
        public void InsertBatch(CarSerialLabel.DTO.RequestDto.V1_2_4.ReqBatchCarSubmitDto req, int batchMediaID, int currentUserID)
        {
            var sbSql = new StringBuilder();

            foreach (var itemIPLabel in req.IPLabel)
            {
                int ipLabelID = Insert(
                    new Entities.BatchLabelHistory.BatchLabelHistory()
                    {
                        BatchMediaID = batchMediaID,
                        TitleID = itemIPLabel.DictId,
                        Name = itemIPLabel.DictName,
                        Type = (int)Entities.ENUM.ENUM.EnumLabelType.IP,
                        CreateUserID = currentUserID
                    }
                    );

                foreach (var itemSubIP in itemIPLabel.SubIP)
                {
                    int subIPID = IPSubLabel.IPSubLabel.Instance.Insert(new Entities.IPSubLabel.IPSubLabel()
                    {
                        LabelID = ipLabelID,
                        TitleID = itemSubIP.DictId,
                        CreateUserID = currentUserID,
                        BatchMediaID = batchMediaID
                    });

                    foreach (var itemSonIPLabel in itemSubIP.Label)
                    {
                        SonIPLabel.SonIPLabel.Instance.Insert(new Entities.SonIPLabel.SonIPLabel()
                        {
                            SubIPID = subIPID,
                            Name = itemSonIPLabel.DictName,
                            CreateUserID = currentUserID,
                            BatchMediaID = batchMediaID
                        });
                    }
                }
            }
        }
    }
}
