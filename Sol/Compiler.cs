using System;
using System.Collections.Generic;
using Sol.Expressions;
using Sol.SyntaX;

namespace Sol
{
	public class Compiler
	{
		public Data data => container.data;
		public Container container => GetContainer();
		Stack<Container> containers = new Stack<Container>();
		public Container GetContainer()
		{
			return containers.Peek();
		}
		public Tokenizer tokenizer;
		Script script;
		public Script Compile(string code, Data custom = null, string fileName = "",
		                      CompilerOptions options = default(CompilerOptions))
		{
			code = code.Replace("\r", "");
			if(tokenizer == null)
				tokenizer = new Tokenizer();
			script = new Script();
			if(custom != null)
				script.data = custom;
			var tokenList = tokenizer.Tokenize(code);
			tokenList.RemoveAll(token => token.type == TokenType.EOF &&
			                    tokenList.IndexOf(token)+1 < tokenList.Count && 
			                    tokenList[tokenList.IndexOf(token)+1].type == TokenType.EOF);
			TokenProcessor.Process(tokenList, fileName, options);
			compile(new Queue<Token>(tokenList));

			return script;
		}
		public Script CompileDirect(List<Token> tokenList, Data custom = null, string fileName = "",
		                            CompilerOptions options = default(CompilerOptions))
		{
			script = new Script();
			if(custom != null)
				script.data = custom;
			tokenList.RemoveAll(token => token.type == TokenType.EOF &&
			                    tokenList.IndexOf(token)+1 < tokenList.Count && 
			                    tokenList[tokenList.IndexOf(token)+1].type == TokenType.EOF);
			TokenProcessor.Process(tokenList, fileName, options);
			compile(new Queue<Token>(tokenList));

			return script;
		}
		void compile(Queue<Token> tokens)
		{
			containers.Push(script);
			while(tokens.Count > 0)
			{
				var line = toEOF(tokens);
				if(line != null)
				container.lines.Add(line);
			}
		}
		Expression toEOF(Queue<Token> tokens)
		{
			var token = tokens.Dequeue();
			if(tokens.Count == 0)
				return null;
			var next = tokens.Peek();
			switch(token.type)
			{
				case TokenType.End:
				{
					if(containers.Count == 1)
						throw new CompilationException("Unexpected token ", token);
					containers.Pop();
					return null;
				}
				case TokenType.Debug:
					{
						if(next.type == TokenType.StringLiteral)
						{
							tokens.Dequeue();
							return new DebugExpression(DebugSymbol.Name, next.value, 0);
						}else if(next.type == TokenType.Line)
						{
							tokens.Dequeue();
							next = tokens.Dequeue();
							if(next.type != TokenType.IntegerLiteral)
								throw new CompilationException("Cannot find integer of line debug symbol at",
								                               next);
							return new DebugExpression(DebugSymbol.Line, null, next.value);
						}
						return null;
					}
				case TokenType.Variable:
				{
					string name = next.value;
					data.set(name, null);
					if(next.type != TokenType.EOF)
					{
						tokens.Dequeue();
						return getID(next, tokens.Peek(), tokens);
					}
					return null;
				}
				case TokenType.ID:
				{
					return getID(token, next, tokens);
				}
				case TokenType.Function:
				{
					return getFunction(token, next, tokens);
				}
				case TokenType.For:
				{
					return getFor(token, next, tokens);
				}
				case TokenType.While:
				{
					return getWhile(token, tokens);
				}
				case TokenType.If:
				{
					return getConditional(token, next, tokens);
				}
				case TokenType.Return:
				{
					var value = getValue(tokens);
					return new Return(value);
				}
				/*
				case TokenType.OpenBrackets:
				{
					token = tokens.Dequeue();
					return getImplArray(token, tokens);
				}
				*/
			}
			return null;
		}
		Expression getWhile(Token token, Queue<Token> tokens)
		{
			var conditional  = getCond(token, tokens.Peek(), tokens);
			var loop = new WhileExpression(conditional as ConditionalExpression);
			loop.data.parent = data;
			container.lines.Add(loop);
			containers.Push(loop);
			return null;
		}
		IValue getImplArray(Token token, Queue<Token> tokens)
		{
			//token = tokens.Dequeue();
			List<IValue> values = new List<IValue>();
			while(tokens.Count > 0)
			{
				IValue item = getValue(tokens);
				values.Add(item);
				token = tokens.Peek();
				if(token.type == TokenType.CloseBrackets)
				{
					tokens.Dequeue();
					break;
				}
			}
			return new ConstantArray(values.ToArray());
		}
		Expression getFor(Token token, Token next, Queue<Token> tokens)
		{
			outter:
			token = tokens.Dequeue();
			switch(token.type)
			{
				case TokenType.OpenParenthesis:
				{
					goto outter;
				}
				case TokenType.ID:
				{
					string variable = token.value;
					token = tokens.Dequeue();
					if(token.type != TokenType.Bind)
						throw new CompilationException("Unexpected token "+token+", expecting token '='",null,true,1);
					token = tokens.Dequeue();
					if(!Tokenizer.isValue(token.type))
						throw new CompilationException("Unexpected token "+token+", expecting a value.",null,true,1);
					dynamic begin = token.value;
					token = tokens.Dequeue();
					if(token.type != TokenType.EOF)
						throw new CompilationException("Unexpected token "+token+", expecting ';' token.",null,mode:1);
					token = tokens.Dequeue();
					if(!Tokenizer.isValue(token.type))
						throw new CompilationException("Unexpected token "+token+", expecting a value.",null,true,1);
					dynamic end = token.value;
					token = tokens.Dequeue();
					if(token.type != TokenType.EOF)
						throw new CompilationException("Unexpected token "+token+", expecting ';' token.",null,mode:1);
					token = tokens.Dequeue();
					if(!Tokenizer.isValue(token.type))
						throw new CompilationException("Unexpected token "+token+", expecting a value.",null,true,1);
					dynamic increment = token.value;
					var loop = new LoopExpression(variable, new Constant(begin), new Constant(end), 
					                          new Constant(increment));
					loop.data.parent = data;
					container.lines.Add(loop);
					containers.Push(loop);
					return null;
				}
			}
			return null;
		}
		Expression getConditional(Token token, Token next, Queue<Token> tokens)
		{
			var conditional = getCond(token, next, tokens);
			conditional.data.parent = data;
			container.lines.Add(conditional);
			containers.Push(conditional);
			return null;
		}
		ConditionalExpression getCond(Token token, Token next, Queue<Token> tokens)
		{
			var a = getValue(tokens);
			var comp = tokens.Dequeue();
			if(comp.type == TokenType.Then)
			{
				return new ConditionalExpression(a, new Constant(true), TokenType.Equal);
			}
			if(!Tokenizer.isComparisson(comp.type))
				throw new CompilationException("Cannot define comparisson signal of token -> ", comp);
			var b = getValue(tokens);
			token = tokens.Dequeue();
			if(token.type != TokenType.Then)
				throw new CompilationException("[Then] token spected, find ", token);
			return new ConditionalExpression(a, b, comp.type);
		}
		Expression getFunction(Token token, Token next, Queue<Token> tokens)
		{
			string name = next.value;
			tokens.Dequeue();
			token = tokens.Dequeue();
			if(token.type != TokenType.OpenParenthesis)
				throw new CompilationException("'(' expected, occurred ", token);
			List<string> args = new List<string>();
			while(true)
			{
				token = tokens.Dequeue();
				if(token.type == TokenType.CloseParenthesis)
					break;
				if(token.type == TokenType.Comma)
					continue;
				if(token.type != TokenType.ID)
					throw new CompilationException("ID expected, retrieved ", token);
				args.Add(token.value);
			}
			var function = new FunctionExpression(name, args.ToArray());
			function.data.parent = data;
			data.set(name, function);
			container.lines.Add(function);
			containers.Push(function);
			return null;
		}
		Expression getID(Token token, Token next, Queue<Token> tokens)
		{
			var reference = new Reference(token.value, data);
			if(next.type == TokenType.OpenParenthesis)
			{
				tokens.Dequeue();
				return getInvoke(reference, tokens);
			}
			else if(next.type == TokenType.Bind)
			{
				tokens.Dequeue();
				return getBind(reference, tokens);
			}
			return null;
		}
		BindExpression getBind(Reference reference, Queue<Token> tokens)
		{
			var value = getValue(tokens);
			return new BindExpression(reference, value);
		}
		InvokeExpression getInvoke(Reference reference, Queue<Token> tokens)
		{
			List<IValue> args = new List<IValue>();
			//tokens.Dequeue();
			while(tokens.Count > 0)
			{
				var f = tokens.Peek();
				if(f.type == TokenType.None)
					throw new CompilationException("Cannot recognize token -> ", f);
				if(f.type == TokenType.CloseParenthesis)
				{
					tokens.Dequeue();
					break;
				}
				IValue arg = getValue(tokens);
				args.Add(arg);
			}
			return new InvokeExpression(reference, args);
		}
		IValue simpleValue(Queue<Token> tokens)
		{
			var token = tokens.Dequeue();
			if(token.type == TokenType.ID)
				return new Reference(token.value, data);
			if(!Tokenizer.isValue(token.type))
				throw new CompilationException("Cannot obtain value from -> ", token);
			return new Constant(token.value);
		}
		IValue getValue(Queue<Token> tokens)
		{
			List<Expression> exprs = new List<Expression>();
			int opP = 0;
			bool asRef = false;
			bool asUnRef = false;
			while(tokens.Count > 0)
			{
				var token = tokens.Peek();
				if(token.type == TokenType.Then)
					break;
				if(token.type == TokenType.Referencer)
				{
					asRef = true;
					tokens.Dequeue();
					continue;
				}
				if(token.type == TokenType.UnReferencer)
				{
					asUnRef = true;
					tokens.Dequeue();
					continue;
				}
				if(Tokenizer.isValue(token.type))
				{
					tokens.Dequeue();
					if(token.type == TokenType.ID)
					{
						if(tokens.Count > 0)
						{
							var next = tokens.Peek();
							if(next.type == TokenType.OpenSquares)
							{
								tokens.Dequeue();
								IValue index = getValue(tokens);
								next = tokens.Dequeue();
								if(next.type != TokenType.CloseSquares)
									throw new CompilationException("Missing ']' token, instead, found -> ", next);
								var xp = new ArrayItem(new Reference(token.value, data), index);
								exprs.Add(xp);
								continue;
							}else if(next.type == TokenType.OpenParenthesis)
							{
								var reference = new Reference(token.value, data);
								tokens.Dequeue();
								var xp = getInvoke(reference, tokens);
								exprs.Add(xp);
								continue;
							}
						}
						if(asRef)
						{
							exprs.Add(new Pointer(token.value, data));
							asRef = false;
						}
						else if(asUnRef)
						{
							exprs.Add(new UnRef(token.value, data));
							asUnRef = false;
						}
						else
							exprs.Add(new Reference(token.value, data));
					}
					else
					{
						if(asRef)
							throw new CompilationException("Wrong '&' token, expecting 'ID', instead, found ->", token);
						exprs.Add(new Constant(token.value));
					}
				}else if(Tokenizer.isOperator(token.type))
				{
					if(token.type == TokenType.OpenParenthesis)
						opP++;
					else if(token.type == TokenType.CloseParenthesis)
					{
						if(opP == 0)
							break;
						opP--;
					}
					tokens.Dequeue();
					exprs.Add(new Operator(token.type));
				}else if(token.type == TokenType.EOF || token.type == TokenType.Comma)
				{
					tokens.Dequeue();
					break;
				}else if(token.type == TokenType.OpenBrackets)
				{
					token = tokens.Dequeue();
					var array = getImplArray(token, tokens);
					exprs.Add(array as Expression);
				}
				else if(token.type == TokenType.CloseBrackets || token.type == TokenType.CloseSquares)
					break;
				else
					break;
			}
			return new MathExpression(exprs);
		}




		public Compiler()
		{
		}
	}
	public struct CompilerOptions
	{
		public bool debugMode;
	}
}
