/* 
 Framework Code By Jarjin leeibution 3.0 License 
*/
using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IController
    {
        void RegisterCommand(string messageName, Type commandType);
        void RegisterViewCommand(IBaseView view, string[] commandNames);

        void ExecuteCommand(IMessage message);

        void RemoveCommand(string messageName);
        void RemoveViewCommand(IBaseView view, string[] commandNames);

        bool HasCommand(string messageName);
    }
}
