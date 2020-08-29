using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL
{
    public class OperateWord
    {
        public static void InsertTable(Document document, DataTable dt)
        {
            Section section = document.Sections[0];
            Paragraph paragraph = section.AddParagraph();
            paragraph.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
            TextRange Rang = paragraph.AppendText("自媒体监测数");
            Rang.CharacterFormat.FontSize = 18;
            Rang.CharacterFormat.FontName = "等线 (中文正文)";

            Table table = section.AddTable(true);
            String[] header = { "合作媒体", "执行日期", "24H阅读量", "24H点赞量" };
            table.ResetCells(dt.Rows.Count + 1, header.Length);
            TableRow Frow = table.Rows[0];
            Frow.IsHeader = true;
            Frow.Height = 30;
            for (int i = 0; i < header.Length; i++)
            {
                Frow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                Paragraph p = Frow.Cells[i].AddParagraph();
                p.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                TextRange txtRange = p.AppendText(header[i]);
                txtRange.CharacterFormat.FontSize = 18;
                txtRange.CharacterFormat.FontName = "等线 (中文正文)";
            }
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                TableRow dataRow = table.Rows[r + 1];
                dataRow.Height = 30;
                dataRow.RowFormat.BackColor = Color.Empty;
                for (int c = 0; c < dt.Columns.Count; c++)
                {

                    dataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    Paragraph p = dataRow.Cells[c].AddParagraph();
                    p.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Center;
                    TextRange txtRange = p.AppendText(dt.Rows[r][c].ToString());
                    txtRange.CharacterFormat.FontSize = 18;
                    txtRange.CharacterFormat.FontName = "等线 (中文正文)";

                }
            }
        }
        public static void InsertImage(Document document, List<string> ImgUrlList)
        {
            if (ImgUrlList.Count > 0)
            {
                Section section = document.AddSection();
                Paragraph paragraph = section.Paragraphs.Count > 0 ? section.Paragraphs[0] : section.AddParagraph();
                paragraph.ApplyStyle(BuiltinStyle.Heading2);
                paragraph = section.AddParagraph();
                paragraph.Format.HorizontalAlignment = Spire.Doc.Documents.HorizontalAlignment.Left;
                for (int i = 0; i < ImgUrlList.Count(); i++)
                {
                    Image srcImage = Image.FromFile(ImgUrlList[i]);
                    int imgWith = 0;
                    int imgHeight = 0;
                    if (srcImage.Width >= srcImage.Height)
                    {
                        imgWith = 500;
                    }
                    else
                    {
                        imgWith = 200;
                    }
                    imgHeight = Convert.ToInt32((Convert.ToDouble(imgWith) / Convert.ToDouble(srcImage.Width) * srcImage.Height));
                    Bitmap b = new Bitmap(imgWith, imgHeight);
                    Graphics g = Graphics.FromImage(b);
                    // 插值算法的质量
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(srcImage, new System.Drawing.Rectangle(0, 0, imgWith, imgHeight), new System.Drawing.Rectangle(0, 0, srcImage.Width, srcImage.Height), GraphicsUnit.Pixel);
                    g.Dispose();
                    paragraph.AppendPicture(b);
                }

            }
        }
    }
}
