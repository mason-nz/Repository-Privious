using System;

namespace XYAuto.ChiTu2018.DAO.Infrastructure
{
    /// <summary>
    /// 注释：垃圾回收非托管资源
    /// 作者：guansl
    /// 日期：2014/7/9 20:20:53
    /// </summary>
    public abstract class Disposable : IDisposable
    {
        private bool isDisposed;

        ~Disposable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (!isDisposed && disposing)
            {
                Console.WriteLine("this is Dispose");
                DisposeCore();
            }

            isDisposed = true;
        }

        protected virtual void DisposeCore()
        {
        }
    }
}
