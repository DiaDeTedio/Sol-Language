using System;
using System.Collections.Generic;
using Sol.Expressions;
using System.Linq;

namespace Sol
{
	public class FastExpressions
	{
		public Expression[] expressions;
		public int current;
		public int count;
		public int size => expressions.Length;
		public void Reset()
		{
			current = 0;
			if(fromEnumer)
				count = size;
		}
		public void Enqueue(Expression token)
		{
			if(current < 0)
				current = 0;
			expressions[current] = token;
			//current++;
			count++;
			if(count > size)
				count = size;
		}
		public Expression Dequeue()
		{
			if(current < 0)
				return null;
			var token = expressions[current];
			current++;
			count--;
			if(count < 0)
				count = 0;
			return token;
		}
		public IEnumerator<Expression> GetEnumerator()
		{
			foreach(var item in expressions)
				yield return item;
		}
		public FastExpressions(int size)
		{
			expressions = new Expression[size];
		}
		bool fromEnumer;
		public FastExpressions(IEnumerable<Expression> expressionEnumerable)
		{
			fromEnumer = true;
			int l = expressionEnumerable.Count();
			var expressionEnumerator = expressionEnumerable.GetEnumerator();
			expressions = new Expression[l];
			int i = 0;
			while(expressionEnumerator.MoveNext())
			{
				expressions[i] = expressionEnumerator.Current;
				i++;
			}
			count = size;
			current = 0;
		}
	}
}
