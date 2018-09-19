using System;

namespace Sol.Expressions
{
	public abstract class Executable : Expression, IExecutable
	{
		public abstract object Execute(params object[] args);
	}
	public interface IExecutable
	{
		object Execute(params object[] args);
	}
}
