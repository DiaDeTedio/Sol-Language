using System;
using System.Collections.Generic;
using Sol.Expressions;
using Sol.SyntaX;

namespace Sol
{
	public static class TokenProcessor
	{
		public static void Process(List<Token> tokens, string fileName, CompilerOptions options)
		{
			if(options.debugMode)
				implement_debug(tokens, fileName);
		}
		static void implement_debug(List<Token> tokenList, string fileName)
		{
			Queue<Token> tokens = new Queue<Token>();
			tokens.Enqueue(new Token("debug", TokenType.Debug, null));
			tokens.Enqueue(new Token($"\"{fileName}\"", TokenType.StringLiteral, fileName));
			int line = 0;
			bool inLine = false;
			foreach(var token in tokenList)
			{
				if(token.type == TokenType.EOF)
				{
					line++;
					inLine = false;
				}else
				{
					if(!inLine)
					{
						tokens.Enqueue(new Token("debug", TokenType.Debug, null));
						tokens.Enqueue(new Token("line", TokenType.Line, null));
						tokens.Enqueue(new Token(line.ToString(), TokenType.IntegerLiteral, line));
					}
					inLine = true;
				}
				tokens.Enqueue(token);
			}
			tokenList.Clear();
			tokenList.AddRange(tokens);
		}
	}
}
