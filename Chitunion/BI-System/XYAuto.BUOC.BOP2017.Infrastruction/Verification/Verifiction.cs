using System;
using System.Collections.Generic;

namespace XYAuto.BUOC.BOP2017.Infrastruction.Verification
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