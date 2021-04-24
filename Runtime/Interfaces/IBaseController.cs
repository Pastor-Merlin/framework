
namespace lus.framework
{
    public interface IBaseController
    {
        //初始化消息放入
        void Init();

        //设置本系统的Proxy
        void InitProxy();
    }
}