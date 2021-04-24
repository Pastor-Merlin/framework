using System;

namespace lus.framework
{
    public interface IBaseView
    {
        void OnMessage(IMessage message);
    }
}