#nullable enable

using System.IO;

var data = File.ReadAllLines("./input").Select(x => x.Split(":")).Select(x => new { target = long.Parse(x[0]), source = x[1].TrimStart().Split(" ").Select(long.Parse).ToArray() });

var check = (long target, long[] source, Func<long, long, long>[] operations) =>
{
	return source.Aggregate(
		new List<long> { 0 },
		(acc, b) => acc.SelectMany(a => operations.Select(x => x(a, b))).ToList())
	.Contains(target);
};

var add = (long a, long b) => a + b;
var mul = (long a, long b) => a * b;
var con = (long a, long b) => long.Parse($"{a}{b}");

Internal.Console.WriteLine(data.Sum(x => check(x.target, x.source, new[] { add, mul }) ? x.target : 0).ToString());
Internal.Console.WriteLine(data.Sum(x => check(x.target, x.source, new[] { add, mul, con }) ? x.target : 0).ToString());
