using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OptionComputationExpression
{
	[AsyncMethodBuilder (typeof (OptionAsyncMethodBuilder<>))]
	interface Option<T> { }

	// Could use the closed type hierarchy Roslyn feature to be an approximation of a discriminated union
	// https://github.com/dotnet/csharplang/issues/485
	sealed class None<T> : Option<T> { public static readonly None<T> Value = new None<T> (); }
	sealed class Some<T> : Option<T>
	{
		public readonly T Item;
		public Some (T item) => Item = item;
		public static explicit operator T (Some<T> option) => option.Item;
	}

	static class Some
	{
		public static Some<T> Of<T> (T value) => new Some<T> (value);
	}
}
