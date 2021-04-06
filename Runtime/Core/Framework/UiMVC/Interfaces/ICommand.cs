/* 
 Framework Code By Jarjin lee
*/
using System;

namespace Framework
{
	public interface ICommand
	{
		void Execute(IMessage message);
	}
}

