using System;
using System.Collections.Generic;

namespace Sol.Expressions
{
	public class Container : Expression
	{
		public Data data = new Data();
		public List<Expression> lines = new List<Expression>();

		protected object ExecuteContainer()
		{
			foreach(var line in lines)
			{
				var ret = line as Return;
				if(ret != null)
					return ret.value.value;
				var ex = line as IExecutable;
				if(ex != null)
					ex.Execute();
			}
			return null;
		}
	}
}
