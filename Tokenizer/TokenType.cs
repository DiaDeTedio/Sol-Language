using System;

namespace Sol
{
	public enum TokenType : byte
	{
		None,
		Debug,
		Line,
		Comma,
		/// <summary>
		/// The referencer token E-Commercial.
		/// </summary>
		Referencer,
		/// <summary>
		/// The unreferencer token '$'.
		/// </summary>
		UnReferencer,
		Global,
		OpenParenthesis,
		CloseParenthesis,
		OpenBrackets,
		CloseBrackets,
		OpenSquares,
		CloseSquares,
		Function,
		Return,
		Field,
		Type,
		Then,
		If,
		Else,
		For,
		ForEach,
		While,
		VoidLiteral,
		TrueLiteral,
		FalseLiteral,
		StringLiteral,
		IntegerLiteral,
		FloatLiteral,
		CharLiteral,
		NullLiteral,
		Plus,
		Minus,
		Divide,
		Multiply,
		Public,
		Private,
		Static,
		Internal,
		Export,
		ID,
		Variable,
		/// <summary>
		/// Traditional a '=' b
		/// </summary>
		Bind,
		/// <summary>
		/// a == b
		/// </summary>
		Equal,
		/// <summary>
		/// a != b
		/// </summary>
		Different,
		/// <summary>
		/// a > b
		/// </summary>
		Larger,
		/// <summary>
		/// a , b
		/// </summary>
		Lower,
		/// <summary>
		/// a >= b
		/// </summary>
		LargerOrEqual,
		/// <summary>
		/// a ,= b
		/// </summary>
		LowerOrEqual,
		EOF,
		End
	}
}
