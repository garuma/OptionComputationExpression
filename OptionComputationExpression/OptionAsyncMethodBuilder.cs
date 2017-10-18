using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace OptionComputationExpression
{
	class OptionAsyncMethodBuilder<T>
	{
		Option<T> result = None<T>.Value;

		public static OptionAsyncMethodBuilder<T> Create() => new OptionAsyncMethodBuilder<T>();

		public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
		{
			// Simply start the state machine which will execute our code
			stateMachine.MoveNext();
		}

		public Option<T> Task => result;

		public void SetStateMachine(IAsyncStateMachine stateMachine) { }
		public void SetResult(T result) => this.result = Some.Of(result);
		public void SetException(Exception ex) { /* We leave the result to None */ }

		public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : INotifyCompletion
			where TStateMachine : IAsyncStateMachine
		{
			throw new NotSupportedException();
		}

		public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
			where TAwaiter : ICriticalNotifyCompletion
			where TStateMachine : IAsyncStateMachine
		{
			throw new NotSupportedException();
		}
	}

	class OptionAwaiter<T> : INotifyCompletion
	{
		Option<T> previousOption;

		public OptionAwaiter(Option<T> previousOption)
		{
			this.previousOption = previousOption;
		}

		public bool IsCompleted => previousOption is Some<T>;

		public void OnCompleted(Action continuation)
		{
			/* We never need to execute the continuation cause
			 * we only reach here when the result is None which
			 * means we are trying to short-circuit everything
			 * else
			 */
		}

		public T GetResult() => ((Some<T>)previousOption).Item;
	}

	static class OptionExtensions
	{
		public static OptionAwaiter<T> GetAwaiter<T>(this Option<T> option)
		{
			return new OptionAwaiter<T>(option);
		}
	}
}
