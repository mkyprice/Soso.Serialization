using System;
using Soso.Serialization.Logging;

namespace Soso.Serialization.Binary.Buffers.Reading
{
	public class RoundReadBuffer<T> : IReadBuffer<T>
	{
		public static int DEFAULT_CAPACITY = 1 * 1024 * 1024; // 1MB
		
		private readonly T[] _buffer;
		public long Position
		{
			get => _position;
			set => _position = value;
		}
		public long Count
		{
			get => _bufferPosition;
		}

		private long _bufferPosition = 0;
		private long _position = 0;

		public RoundReadBuffer()
		{
			_buffer = new T[DEFAULT_CAPACITY];
		}

		public RoundReadBuffer(int capacity)
		{
			_buffer = new T[capacity];
		}

		private int GetLocalPosition(long index)
		{
			return (int)(index % _buffer.Length);
		}
		
		public void Append(Span<T> bytes)
		{
			int remaining = bytes.Length;
			int localPos = GetLocalPosition(_position);
			
			while (remaining > 0)
			{
				int localBufferPos = GetLocalPosition(_bufferPosition);
				
				long leftInBuffer = _buffer.Length - localBufferPos;
				int copyAmount = (int)Math.Min(remaining, leftInBuffer);
				if (localBufferPos < localPos && copyAmount + localBufferPos > localPos)
				{
					throw new Exception("Exceeded buffer size");
				}

				Span<T> appendSection = new Span<T>(_buffer, localBufferPos, copyAmount);

				int bytesPos = bytes.Length - remaining;
				bytes.Slice(bytesPos, copyAmount).CopyTo(appendSection);

				_bufferPosition += copyAmount;

				remaining -= copyAmount;

				if (copyAmount <= 0)
				{
					Log.Error($"Copy amount was: {copyAmount}");
					break;
				}
			}
		}
		public T Peek(int offset)
		{
			long pos = GetLocalPosition(_position + offset);
			if (pos > _buffer.Length)
			{
				pos -= _buffer.Length;
			}
			if (pos > _bufferPosition)
			{
				throw new Exception("Exceeded buffer");
			}
			return _buffer[pos];
		}
		public T[] ReadRange(int count)
		{
			T[] bytes = new T[count];
			int remaining = count;

			while (remaining > 0)
			{
				int localPosition = GetLocalPosition(_position);
				long leftInBuffer = _buffer.Length - localPosition;
				int copyAmount = (int)Math.Min(remaining, leftInBuffer);
				
				if (localPosition + copyAmount > _bufferPosition)
				{
					throw new Exception("Exceeded buffer");
				}
				
				Memory<T> appendSection = new Memory<T>(_buffer, localPosition, copyAmount);

				int bytesPos = bytes.Length - remaining;
				appendSection.CopyTo(new Memory<T>(bytes, bytesPos, copyAmount));

				remaining -= copyAmount;

				Position += copyAmount;
			}

			return bytes;
		}
		public void Reset()
		{
			Position = 0;
			_bufferPosition = 0;
		}
	}
}
