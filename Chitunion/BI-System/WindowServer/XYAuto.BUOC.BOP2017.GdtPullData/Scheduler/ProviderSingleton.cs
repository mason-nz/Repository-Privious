/********************************************************
*创建人：lixiong
*创建时间：2017/8/22 11:46:51
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using XYAuto.BUOC.BOP2017.BLL.GDT;

namespace XYAuto.BUOC.BOP2017.GdtPullData.Scheduler
{
    internal class ProviderSingleton
    {
        // 定义一个静态变量来保存类的实例
        private static GdtLogicProvider _gdtLogicProvider;

        // 定义一个标识确保线程同步
        private static readonly object Locker = new object();

        // 定义私有构造函数，使外界不能创建该类实例
        private ProviderSingleton()
        {
        }

        /// <summary>
        /// 定义公有方法提供一个全局访问点,同时你也可以定义公有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static GdtLogicProvider GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            // 双重锁定只需要一句判断就可以了
            if (_gdtLogicProvider == null)
            {
                lock (Locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (_gdtLogicProvider == null)
                    {
                        _gdtLogicProvider = new GdtLogicProvider();
                    }
                }
            }
            return _gdtLogicProvider;
        }
    }
}