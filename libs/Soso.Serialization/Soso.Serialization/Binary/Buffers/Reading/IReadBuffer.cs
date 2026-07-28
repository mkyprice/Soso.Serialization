using System;

namespace Soso.Serialization.Binary.Buffers.Reading
{
	public interface IReadBuffer<T>
	{
		public long Position { get; set; }
		public long Count { get; }
		void Append(Span<T> bytes);
		T Peek(int offset);
		T[] ReadRange(int count);
		void Reset();
	}
}
