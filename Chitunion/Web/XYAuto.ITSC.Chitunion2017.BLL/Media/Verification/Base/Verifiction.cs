using System;
using System.Collections.Generic;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Verification.Base
{
    public class Verifiction<T> where T : new()
    {
        protected Dictionary<int, Func<T, ReturnValue>> FuncDic;

        public Verifiction(Dictionary<int, Func<T, ReturnValue>> funcDic)
        {
            FuncDic = funcDic;
        }

        public ReturnValue Verify(T entity, bool isOutParam)
        {
            var retValue = new ReturnValue();
            foreach (var item in FuncDic)
            {
                var result = item.Value(entity);
                retValue.Message = result.Message;
                retValue.HasError = result.HasError;
                retValue.ErrorCode = result.ErrorCode;
                retValue.ReturnObject = isOutParam ? result.ReturnObject : null;
                if (result.HasError) break;
            }

            return retValue;
        }
    }
}
