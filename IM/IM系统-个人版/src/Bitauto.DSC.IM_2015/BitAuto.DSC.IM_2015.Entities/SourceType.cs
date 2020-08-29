using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Entities
{
    public class SourceType
    {
        public SourceType()
        {
            _sourceTypeName = Constant.STRING_INVALID_VALUE;
            _sourceTypeValue = Constant.STRING_INVALID_VALUE;
        }
        private string _sourceTypeName;
        private string _sourceTypeValue;
        public string SourceTypeName
        { get { return _sourceTypeName; } set { _sourceTypeName = value; } }
        public string SourceTypeValue
        { get { return _sourceTypeValue; } set { _sourceTypeValue = value; } }
    }
}
