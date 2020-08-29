using Lucene.Net.Analysis.PanGu;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.ITSC.Chitunion2017.BLL.ChituMedia;
using XYAuto.ITSC.Chitunion2017.Entities.ChituMedia;
using XYAuto.ITSC.Chitunion2017.LuceneMediaConsole.Common;

namespace XYAuto.ITSC.Chitunion2017.LuceneMediaConsole.LuceneManage
{
    public class TagVehicleLucene
    {
        #region 单例
        private TagVehicleLucene() { }

        public static TagVehicleLucene instance = null;
        public static readonly object padlock = new object();
        public static readonly string path = ConfigurationManager.AppSettings["ConfigArgsPath"];


        public static TagVehicleLucene Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new TagVehicleLucene();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        public void AddTagVehicleIndex()
        {
            try
            {
                List<TagVehicleModel> TagList = TagVehicleBll.Instance.GetTagVehicleList();

                //微信存储目录
                string strPath = path + "\\Index\\TagVehicle";
                if (!System.IO.Directory.Exists(strPath))
                    System.IO.Directory.CreateDirectory(strPath);

                FSDirectory directory = FSDirectory.Open(new DirectoryInfo(strPath), new NativeFSLockFactory());
                bool isUpdate = IndexReader.IndexExists(directory); //判断索引库是否存在
                if (isUpdate)
                {
                    if (IndexWriter.IsLocked(directory))
                    {
                        IndexWriter.Unlock(directory);
                    }
                }
                //  创建向索引库写操作对象  IndexWriter(索引目录,指定使用盘古分词进行切词,最大写入长度限制)

                IndexWriter writer = new IndexWriter(directory, new PanGuAnalyzer(), !isUpdate,
                    IndexWriter.MaxFieldLength.UNLIMITED);

                foreach (TagVehicleModel entity in TagList)
                {

                    //  一条Document相当于一条记录
                    Document document = new Document();
                    //  每个Document可以有自己的属性（字段），所有字段名都是自定义的，值都是string类型
                    //  Field.Store.YES不仅要对文章进行分词记录，也要保存原文，就不用去数据库里查一次了               
                    Field MasterNameFile = new Field("MasterName", entity.MasterName + string.Empty, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                    MasterNameFile.SetBoost(1);
                    document.Add(MasterNameFile);

                    Field BrandNameNameFile = new Field("BrandName", entity.BrandName + string.Empty, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                    BrandNameNameFile.SetBoost(3);
                    document.Add(BrandNameNameFile);

                    Field SerialNameFile = new Field("SerialName", entity.SerialName + string.Empty, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
                    SerialNameFile.SetBoost(5);
                    document.Add(SerialNameFile);

                    document.Add(new Field("MediaTagName", entity.MediaTagName + string.Empty, Field.Store.YES, Field.Index.NO));
                    document.Add(new Field("SerialID", entity.SerialID + string.Empty, Field.Store.YES, Field.Index.NO));
                    
                    //  把文档写入索引库
                    writer.AddDocument(document);
                }
                writer.Close(); // Close后自动对索引库文件解锁
                directory.Close();  //  不要忘了Close，否则索引结果搜不到

            }
            catch (Exception ex)
            {
                Log4NetHelper.Error("标签车型对应关系Lucene索引发生异常", ex);
            }
        }
    }
}
