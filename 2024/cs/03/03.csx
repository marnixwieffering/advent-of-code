#nullable enable
#r "System.Text.RegularExpressions"

using System.IO;
using System.Text.RegularExpressions;

var data = string.Join("", File.ReadAllLines("./input"));

var sum1 = Regex.Matches(data, @"mul\((\d+),(\d+)\)")
	.Cast<Match>()
	.Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));


Internal.Console.WriteLine(sum1.ToString());

var sum2 = Regex.Matches(data, @"mul\((\d+),(\d+)\)|(do(?:n't)?)\(\)")
	.Cast<Match>()
	.Aggregate(new { enabled = true, sum = 0 }, (acc, match) =>
	{
		var state = match.Groups[3].Value;
		if (state == "do") return new { enabled = true, acc.sum };
		if (state == "don't") return new { enabled = false, acc.sum };
		return new { enabled = acc.enabled, sum = acc.enabled ? acc.sum + int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value) : acc.sum };
	}).sum;

Internal.Console.WriteLine(sum2.ToString());
