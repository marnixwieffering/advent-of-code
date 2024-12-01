#nullable enable

using System.IO;

var data = File.ReadAllLines("./input")
    .Select(line => line.Split("   ").Select(int.Parse).ToArray())
    .ToArray();

var left = data.Select(x => x[0]).OrderBy(x => x).ToArray();
var right = data.Select(x => x[1]).OrderBy(x => x).ToArray();

var sum = left.Zip(right, (l, r) => Math.Abs(l - r)).Sum();
Internal.Console.WriteLine(sum.ToString());

var score = right.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
var sim = left.Sum(l => l * score.GetValueOrDefault(l, 0));
Internal.Console.WriteLine(sim.ToString());

