using System;
using System.Collections.Generic;

namespace Sol.Expressions
{
	public class AccessReference : Value
	{
		public override dynamic setValue
		{
			set
			{
				throw new NotImplementedException ();
			}
		}

		public override dynamic value
		{
			get
			{
				throw new NotImplementedException ();
			}
		}
		public string access;
		public Data data;
		public dynamic get()
		{
			return data.get(access);
		}
		public void set(dynamic value)
		{
			data.set(access, value);
		}
	}

}
