using System;
using System.Collections.Generic;
using Sol.Expressions;

namespace Sol
{
	public class Data
	{
		public static Data global = new Data();
		public Data parent;
		public List<DataItem> items = new List<DataItem>(1024);

		public dynamic get(string name)
		{
			dynamic a = find(name)?.value;
			if(a == null)
			{
				if(parent != null)
					return parent.get(name);
				if(global != this)
					return global.get(name);
			}
			return a;
		}
		public void set(string name, dynamic value)
		{
			var f = find(name);
			if(f == null)
			{
				f = new DataItem(name, value);
				items.Add(f);
				return;
			}
			var ptr = f.value as InternalPtr;
			if(value is InternalPtr)
			{
				f.value = value;
				return;
			}
			if(ptr != null)
				ptr.adress.setValue = value;
			else
				f.value = value;
		}
		public bool contains(string name)
		{
			return items.Exists(item => item.name == name);
		}
		public DataItem find(string name)
		{
			return items.Find(item => item.name == name);
		}
	}
}
