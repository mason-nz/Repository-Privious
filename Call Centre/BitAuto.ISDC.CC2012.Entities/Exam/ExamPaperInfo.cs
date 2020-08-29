using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.ISDC.CC2012.Entities
{
    public class ExamPaperInfo
    {
        public ExamPaper ExamPaper;
        public List<ExamBigQuestioninfo> ExamBigQuestioninfoList;
    }

    public class ExamBigQuestioninfo
    {
        public ExamBigQuestion ExamBigQuestion;
        public List<ExamBSQuestionShip> ExamBSQuestionShipList;
    }


    
}
