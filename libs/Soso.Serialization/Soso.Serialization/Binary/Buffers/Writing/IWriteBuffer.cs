using System;

namespace Soso.Serialization.Binary.Buffers.Writing
{
	public interface IWriteBuffer<T>
	{
		public static int DEFAULT_CAPACITY = 1 * 1024 * 1024; // 1MB
		public int Position { get; set; }
		public int Count { get; set; }
		public void Write(T data);
		public void Write(Span<T> data);
		public Span<T> ToSpan();
	}
}
