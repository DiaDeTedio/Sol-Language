using System;
namespace Sol
{
	public class DataItem
	{
		public string name;
		public dynamic value;

		public DataItem(string name, dynamic value)
		{
			this.name = name;
			this.value = value;
		}
	}
}
