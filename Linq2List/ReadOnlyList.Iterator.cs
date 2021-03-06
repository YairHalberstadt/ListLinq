﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Linq2List
{
	public static partial class ReadOnlyList
	{
		private abstract class Iterator<T> : IReadOnlyList<T>, IEnumerator<T>, IList<T>
		{
			protected T current;
			private bool _enumeratorTaken;
			protected int index = -1;

			public T Current => current;
			object IEnumerator.Current => Current;

			public abstract bool MoveNext();

			public void Dispose()
			{
				current = default;
				index = -1;
			}

			public void Reset()
			{
				current = default;
				index = -1;
			}

			public abstract T this[int index] { get; }

			public abstract int Count { get; }

			public IEnumerator<T> GetEnumerator()
			{
				var enumerator = _enumeratorTaken ? Clone() : this;
				enumerator._enumeratorTaken = true;
				return enumerator;
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public abstract Iterator<T> Clone();

			#region IList Implementation

			int IList<T>.IndexOf(T item)
			{
				var comparer = EqualityComparer<T>.Default;
				for (int i = 0; i < Count; i++)
					if (comparer.Equals(this[i], item))
						return i;

				return -1;
			}

			void IList<T>.Insert(int index, T item)
			{
				throw new NotSupportedException();
			}

			void IList<T>.RemoveAt(int index)
			{
				throw new NotSupportedException();
			}

			void ICollection<T>.Add(T item)
			{
				throw new NotSupportedException();
			}

			void ICollection<T>.Clear()
			{
				throw new NotSupportedException();
			}

			bool ICollection<T>.Contains(T item)
			{
				var comparer = EqualityComparer<T>.Default;
				for (int i = 0; i < Count; i++)
					if (comparer.Equals(this[i], item))
						return true;

				return false;
			}

			void ICollection<T>.CopyTo(T[] array, int arrayIndex)
			{
				if (array is null)
					throw new ArgumentNullException(nameof(array));
				if (arrayIndex < 0)
					throw new ArgumentOutOfRangeException(nameof(arrayIndex));
				if (array.Length - arrayIndex < Count)
					throw new ArgumentException(
						$"The number of elements in the source collection is greater than the available space from {nameof(arrayIndex)} to the end of the destination array.");
				for (int i = 0; i < Count; i++, arrayIndex++)
					array[arrayIndex] = this[i];
			}

			bool ICollection<T>.Remove(T item)
			{
				throw new NotSupportedException();
			}

			bool ICollection<T>.IsReadOnly => true;

			T IList<T>.this[int index]
			{
				get => this[index];
				set => throw new NotSupportedException();
			}

			#endregion
		}
	}
}