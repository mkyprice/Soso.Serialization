using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Soso.Serialization.Logging;

namespace Soso.Serialization.Binary.Buffers.Reading
{
	public class ReadBufferList<T> : IReadBuffer<T>
	{
		public long Count => _bufferPosition + (_buffers.Count - 1) * BUFFER_SIZE;
		public long Position
		{
			get
			{
				return (BUFFER_SIZE * _bufferIndex) + _position;
			}
			set
			{
				int index = (int)(value / BUFFER_SIZE);
				_position = value % BUFFER_SIZE;
				_bufferIndex = index;
			}
		}
		
		private readonly int BUFFER_SIZE = 128;//1024 * 1024;
		private readonly List<T[]> _buffers = new List<T[]>();
		private long _position = 0;
		private int _bufferIndex = 0;
		private long _bufferPosition = 0;

		public ReadBufferList()
		{
		}
		
		public ReadBufferList(int bufferSize)
		{
			BUFFER_SIZE = bufferSize;
		}

		public ReadBufferList(T[] bytes)
		{
			Append(bytes);
		}

		public ReadBufferList(T[] bytes, int bufferSize)
		{
			BUFFER_SIZE = bufferSize;
			Append(bytes);
		}

		public void Append(Span<T> bytes)
		{
			if (_buffers.Count <= 0)
			{
				_buildBuffer();
			}

			int remaining = bytes.Length;
			while (remaining > 0)
			{
				T[] buffer = _buffers[_buffers.Count - 1];
				int bytes_to_write = remaining;
				if (bytes_to_write + _bufferPosition > BUFFER_SIZE)
				{
					bytes_to_write = (int)(BUFFER_SIZE - _bufferPosition);
				}

				if (bytes_to_write > 0)
				{
					int bytesIndex = (int)(bytes.Length - remaining);
					Span<T> mem = new Span<T>(buffer, (int)_bufferPosition, bytes_to_write);
					bytes.Slice(bytesIndex, bytes_to_write).CopyTo(mem);
					remaining -= bytes_to_write;
				}

				if (remaining > 0)
				{
					_buildBuffer();
					_bufferPosition = 0;
				}
				else
				{
					_bufferPosition += bytes_to_write;
				}
			}
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T Peek(int offset)
		{
			int pos = (int)(_position + offset);
			int idx = _bufferIndex;
			if (pos >= BUFFER_SIZE)
			{
				pos -= BUFFER_SIZE;
				idx++;
			}
			return _buffers[idx][pos];
		}
		
		public T[] ReadRange(int count)
		{
			VerifyBufferLength(count);
			T[] bytes = new T[count];
			int bytes_idx = 0;

			while (bytes_idx < count)
			{
				int reader_length = (int)(_bufferIndex < _buffers.Count ? BUFFER_SIZE : _bufferPosition);
				int copy_amount = count - bytes_idx;
				if (_position + copy_amount > reader_length)
				{
					copy_amount = (int)(reader_length - _position);
				}

				Array.Copy(_buffers[_bufferIndex], _position, bytes, bytes_idx, copy_amount);
				bytes_idx += copy_amount;

				Position += copy_amount;
			}

			return bytes;
		}
		
		public void Reset()
		{
			Position = 0;
			_bufferPosition = 0;
			_buffers.Clear();
		}
		

		public void VerifyBufferLength(long len)
		{
			if (_buffers.Count <= 0) throw new Exception("No data");
			if (len + _position > BUFFER_SIZE && (_buffers.Count == 1 && (len + _position - BUFFER_SIZE) > _bufferPosition))
			{
				throw new Exception("Buffer out of range");
			}
		}

		private T[] _buildBuffer()
		{
			T[] buffer = new T[BUFFER_SIZE];
			_buffers.Add(buffer);
			Log.Debug($"{nameof(ReadBufferList<T>)} - building new buffer. Buffer count is {_buffers.Count}");
			return buffer;
		}
	}
}
