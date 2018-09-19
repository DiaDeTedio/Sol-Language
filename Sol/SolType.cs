using System;
using System.Collections.Generic;
using Sol.Expressions;
using Dlr = System.Linq.Expressions.Expression;
using System.Reflection;

namespace Sol
{
	public class SolType
	{
		public const int ExpressionMode = 0;
		public const int ReflectionMode = 1;
		public static int mapType = ExpressionMode;
		public string name;
		public Data data = new Data();
		public bool isCreatable;

		public static SolType MapType(Type type, bool canCreate = false, bool passGlobal = true)
		{
			var sol = new SolType();
			sol.name = type.Name;
			sol.isCreatable = canCreate;
			var fields = type.GetFields();
			string typeName = type.Name;
			foreach(var field in fields)
				mapField(typeName, field, passGlobal?Data.global:sol.data);
			var methods = type.GetMethods();
			foreach(var method in methods)
				mapMethod(typeName, method, passGlobal?Data.global:sol.data);
			var properties = type.GetProperties();
			foreach(var prop in properties)
				mapProperty(typeName, prop, passGlobal?Data.global:sol.data);
			return sol;
		}
		static void mapProperty(string typeName, PropertyInfo property, Data data)
		{
			//if(!property.GetMethod?.IsStatic??true)
			//	return;
			//if(!property.SetMethod?.IsStatic??true)
			//	return;
			if(property.CanRead)
			{
				mapMethod(typeName, property.GetMethod, data, property.Name);
			}
			if(property.CanWrite)
			{
				mapMethod(typeName, property.GetMethod, data, property.Name+".set");
			}
		}
		static void mapMethod(string typeName, MethodInfo method, Data data, string customName = null)
		{
			if(!method.IsStatic)
				return;
			if(mapType == ExpressionMode)
			{
				var args = method.GetParameters();
				var pars = new List<System.Linq.Expressions.ParameterExpression>();
				foreach(var arg in args)
					pars.Add(Dlr.Parameter(arg.ParameterType));
				var prs = pars.ToArray();
				var invoke = Dlr.Lambda(Dlr.Call(method, prs), prs).Compile();
				string finalName = customName??method.Name;
				data.set(typeName+'.'+finalName, invoke);
			}else if(mapType == ReflectionMode)
			{
				var invoke = (Action<string>)((args) => method.Invoke(null, new object[]{args}));
				data.set(typeName+'.'+method.Name, invoke);
			}
		}
		public delegate dynamic callMe(params dynamic[] args);
		static void mapField(string typeName, FieldInfo field, Data data)
		{
			if(!field.IsStatic)
				return;
			if(mapType == ExpressionMode)
			{
				var get = Dlr.Lambda(Dlr.Field(null, field)).Compile();
				data.set(typeName+'.'+field.Name, get);
				if(!field.IsInitOnly)
				{
					var par = Dlr.Parameter(field.FieldType);
					var set = Dlr.Lambda(Dlr.Assign(Dlr.Field(null, field), par),par).Compile(); 
					data.set(typeName+'.'+field.Name+".set", set);
				}
			}else if(mapType == ReflectionMode)
			{
				var get = (Func<dynamic>)(() => field.GetValue(null));
				var set = (Action<dynamic>)((n) => field.SetValue(null, n)); 
				data.set(typeName+'.'+field.Name, get);
				data.set(typeName+'.'+field.Name+".set", set);
			}
		}
	}
}
