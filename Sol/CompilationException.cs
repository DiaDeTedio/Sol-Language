using System;
using System.Collections.Generic;
using Sol.Expressions;
using Sol.SyntaX;

namespace Sol
{
	public class CompilationException : Exception
	{
		public const int Implicit = 0;
		public const int Explicit = 1;
		public string message;
		public Token token;
		public bool error;
		public int mode;

		public CompilationException(string message, Token token, bool error = true, int mode = Implicit)
		{
			this.message = message;
			this.token = token;
			this.error = error;
			this.mode = mode;
		}
		public override string ToString ()
		{
			if(mode == 0)
				return error?"Error":"Warning"+message+token;
			else
				return error?"Error":"Warning"+message;
		}
	}
}
