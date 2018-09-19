using System;
using System.Collections.Generic;

namespace Sol.Expressions
{
	public abstract class Value : Expression, IValue
	{
		public abstract dynamic setValue { set; }
		public abstract dynamic value{get;}

		public override string ToString ()
		{
			return $"From({GetType().Name}) - Value : {value}";
		}
	}
	public class Reference : Value
	{
		public override dynamic value
		{
			get
			{
				return data.get(name);
			}
		}
		public override dynamic setValue
		{
			set
			{
				data.set(name, value);
			}
		}
		public string name;
		public Data data;

		public Reference(string name, Data data)
		{
			this.name = name;
			this.data = data;
		}
	}
	public class Pointer : Value
	{
		public override dynamic value
		{
			get
			{
				return ptr;
			}
		}
		public override dynamic setValue
		{
			set
			{
				ptr = value;
			}
		}
		Reference _ptr;
		InternalPtr ptr;

		public Pointer(string name, Data data)
		{
			_ptr = new Reference(name, data);
			ptr = new InternalPtr(_ptr);
		}
	}
	public class UnRef : Value
	{
		public override dynamic setValue
		{
			set
			{
				dynamic val = data.get(name);
				if(val is InternalPtr)
					((InternalPtr)val).adress.setValue = value;
			}
		}

		public override dynamic value
		{
			get
			{
				dynamic val = data.get(name);
				if(val is InternalPtr)
					return ((InternalPtr)val).adress.value;
				return val;
			}
		}
		string name;
		Data data;
		public UnRef(string name, Data data)
		{
			this.name = name;
			this.data = data;
		}
	}
	public class InternalPtr
	{
		public Reference adress;
		public void setValue(dynamic value)
		{
			if(value is Reference)
				adress = value;
			else
				adress.setValue = value;
		}
		public InternalPtr(Reference adress)
		{
			this.adress = adress;
		}
	}
	public class Constant : Value
	{
		public override dynamic value
		{
			get
			{
				return _value;
			}
		}
		public override dynamic setValue
		{
			set
			{
				throw new NotImplementedException ();
			}
		}
		dynamic _value;

		public Constant(dynamic value)
		{
			_value = value;
		}
	}
	public class ConstantArray : Value
	{
		public override dynamic value
		{
			get
			{
				return _array;
			}
		}
		public override dynamic setValue
		{
			set
			{
				throw new NotImplementedException ();
			}
		}
		public dynamic this[IValue index]
		{
			get
			{
				return get(index);
			}
			set
			{
				set(index, value);
			}
		}
		public dynamic this[int index]
		{
			get
			{
				return get(index);
			}
			set
			{
				set(index, value);
			}
		}
		IValue[] _array;
		public dynamic get(IValue index)
		{
			return _array[index.value];
		}
		public dynamic get(int index)
		{
			return _array[index];
		}
		public void set(IValue index, IValue value)
		{
			_array[index.value] = value.value;
		}
		public void set(int index, IValue value)
		{
			_array[index] = value.value;
		}
		public ConstantArray(IValue[] array)
		{
			_array = array;
		}
		public ConstantArray(object[] arrayValues)
		{
			List<IValue> items = new List<IValue>();
			foreach(var item in arrayValues)
				items.Add(new Constant(item));
			_array = items.ToArray();
		}
	}
	public class ArrayItem : Value
	{
		public override dynamic value
		{
			get
			{
				dynamic item = array.value[getIndex()];
				if(item is IValue)
					return item.value;
				return item;
			}
		}
		public override dynamic setValue
		{
			set
			{
				array.value[index] = value;
			}
		}
		dynamic array;
		IValue index;
		dynamic getIndex()
		{
			dynamic ind = index.value;
			if(ind is IValue)
				return ind.value;
			return ind;
		}

		public ArrayItem(dynamic array, IValue index)
		{
			this.array = array;
			this.index = index;
		}
	}
	public interface IValue
	{
		dynamic value{get;}
		dynamic setValue{set;}
	}
	public interface INeedCompletion
	{
		void OnCompilationEnd(Compiler compiler, Script script);
	}
}
