using System;

namespace Framework
{
    public interface IBaseView
    {
        void OnMessage(IMessage message);
    }
}