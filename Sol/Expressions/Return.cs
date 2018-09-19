using System;

namespace Sol.Expressions
{
	public class Return : Expression
	{
		public IValue value;

		public Return(IValue value)
		{
			this.value = value;
		}
	}
}
