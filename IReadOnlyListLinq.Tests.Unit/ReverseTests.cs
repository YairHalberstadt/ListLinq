﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace IReadOnlyListLinq.Tests.Unit
{
    public class ReverseTests : IReadOnlyListLinqTests
    {
		[Fact]
		public void InvalidArguments()
		{
			AssertExtensions.Throws<ArgumentNullException>("source", () => IReadOnlyListLinq.Reverse<string>(null));
		}

		[Theory]
		[MemberData(nameof(ReverseData))]
		public void Reverse<T>(IReadOnlyList<T> source)
		{
			T[] expected = source.ToArray();
			Array.Reverse(expected);

			var actual = source.Reverse();

			Assert.Equal(expected, actual);
			Assert.Equal(expected.Count(), actual.Count()); // Count may be optimized.
			Assert.Equal(expected, actual.ToArray());
			Assert.Equal(expected, actual.ToList());

			Assert.Equal(expected.FirstOrDefault(), actual.FirstOrDefault());
			Assert.Equal(expected.LastOrDefault(), actual.LastOrDefault());

			for (int i = 0; i < expected.Length; i++)
			{
				Assert.Equal(expected[i], actual.ElementAt(i));

				Assert.Equal(expected.Skip(i), actual.Skip(i));
				Assert.Equal(expected.Take(i), actual.Take(i));
			}

			Assert.Equal(default(T), actual.ElementAtOrDefault(-1));
			Assert.Equal(default(T), actual.ElementAtOrDefault(expected.Length));

			Assert.Equal(expected, actual.Select(_ => _));
			Assert.Equal(expected, actual.Where(_ => true));

			Assert.Equal(actual, actual); // Repeat the enumeration against itself.
		}

		[Theory, MemberData(nameof(ReverseData))]
		public void RunOnce<T>(IReadOnlyList<T> source)
		{
			T[] expected = source.ToArray();
			Array.Reverse(expected);

			IEnumerable<T> actual = source.RunOnce().Reverse();

			Assert.Equal(expected, actual);
		}

		public static IEnumerable<object[]> ReverseData()
		{
			var integers = new[]
			{
				Array.Empty<int>(), // No elements.
                new[] { 1 }, // One element.
                new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }, // Distinct elements.
                new[] { -10, 0, 5, 0, 9, 100, 9 }, // Some repeating elements.
            };

			return integers
				.Select(collection => new object[] { collection })
				.Concat(
					integers.Select(c => new object[] { c.Select(i => i.ToString()) })
				);
		}
	}
}