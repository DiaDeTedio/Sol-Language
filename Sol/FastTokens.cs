using System;
using System.Collections.Generic;
using Sol.Expressions;
using System.Linq;

namespace Sol
{
	public class FastTokens
	{
		public Token[] tokens;
		public int current;
		public int count;
		public int size => tokens.Length;

		public void Enqueue(Token token)
		{
			if(current < 0)
				current = 0;
			tokens[current] = token;
			current++;
			count++;
			if(count > size)
				count = size;
		}
		public Token Dequeue()
		{
			if(current < 0)
				return null;
			var token = tokens[current];
			current--;
			count--;
			if(count < 0)
				count = 0;
			return token;
		}
		public IEnumerator<Token> GetEnumerator()
		{
			foreach(var item in tokens)
				yield return item;
		}
		public FastTokens(IEnumerable<Token> tokenEnumerable)
		{
			int l = tokenEnumerable.Count();
			var tokenEnumerator = tokenEnumerable.GetEnumerator();
			tokens = new Token[l];
			int i = 0;
			while(tokenEnumerator.MoveNext())
			{
				tokens[i] = tokenEnumerator.Current;
				i++;
			}
		}
	}
}
