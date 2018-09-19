using System;
using System.Collections.Generic;
using Sol.Expressions;
using Sol.SyntaX;
using System.IO;

namespace Sol
{
	public static class ByteCode
	{
		public static List<Token> FromBytes(byte[] byteCode)
		{
			MemoryStream mem = new MemoryStream(byteCode);
			BinaryReader bin = new BinaryReader(mem);
			int tokenCount = bin.ReadInt32();
			List<Token> tokens = new List<Token>(tokenCount);
			int pos = 0;
			while(pos < tokenCount)
			{
				var tokenType = (TokenType)bin.ReadByte();
				dynamic value = null;
				if(Tokenizer.isValue(tokenType))
				{
					switch(tokenType)
					{
						case TokenType.ID:
							value = bin.ReadString();break;
						case TokenType.TrueLiteral:
							value = true;break;
						case TokenType.FalseLiteral:
							value = false;break;
						case TokenType.CharLiteral:
							value = bin.ReadChar();break;
						case TokenType.FloatLiteral:
							value = bin.ReadSingle();break;
						case TokenType.IntegerLiteral:
							value = bin.ReadInt32();break;
						case TokenType.StringLiteral:
							value = bin.ReadString();break;
					}
				}
				var token = new Token("", tokenType, value);
				tokens.Add(token);
				pos++;
			}
			mem.Dispose();
			return tokens;
		}
		public static byte[] GetBytes(List<Token> tokenList)
		{
			MemoryStream mem = new MemoryStream();
			BinaryWriter bin = new BinaryWriter(mem);
			Queue<Token> tokens = new Queue<Token>(tokenList);
			bin.Write(tokens.Count);
			while(tokens.Count > 0)
			{
				var token = tokens.Dequeue();
				bin.Write((byte)token.type);
				if(Tokenizer.isValue(token.type))
				{
					switch(token.type)
					{
						case TokenType.ID:
							bin.Write(token.value);break;
						case TokenType.TrueLiteral:
							bin.Write(true);break;
						case TokenType.FalseLiteral:
							bin.Write(false);break;
						case TokenType.CharLiteral:
							bin.Write((char)token.value);break;
						case TokenType.FloatLiteral:
							bin.Write((float)token.value);break;
						case TokenType.IntegerLiteral:
							bin.Write((int)token.value);break;
						case TokenType.StringLiteral:
							bin.Write((string)token.value);break;
						//case TokenType.NullLiteral:
						//	bin.Write(null);break;
					}
				}
			}
			var bc = mem.ToArray();
			mem.Dispose();
			return bc;
		}
	}
}
