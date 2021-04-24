/* 
 Framework Code By Jarjin lee 
*/
using System;

namespace lus.framework
{
    public interface IMessage
    {
        string Name { get; }

        object Body { get; set; }

        string Type { get; set; }

        string ToString();
    }
}
