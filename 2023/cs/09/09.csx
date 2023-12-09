#nullable enable

using System.IO;

var input = "./sample";
var data = File.ReadAllLines(input).Select(x => x.Split(' ').Select(x => int.Parse(x))).ToArray();
var future = 0;
var past = 0;

foreach (var start in data)
{
	var sequences = new List<List<int>>() { start.ToList() };
	while (sequences.Last().Any(x => x != 0)) sequences.Add(Diff(sequences.Last()));
	sequences.Reverse();

	var current = 0;
	foreach (var sequence in sequences)
		current += sequence.Last();
	future += current;

	current = 0;
	foreach (var sequence in sequences)
		current = sequence.First() - current;
	past += current;
}

Internal.Console.WriteLine(future.ToString());
Internal.Console.WriteLine(past.ToString());

List<int> Diff(List<int> list)
{
	int previous = list.First();
	var result = new List<int>();
	foreach (var current in list.Skip(1))
	{
		result.Add(current - (int)previous);
		previous = current;
	}
	return result;
}
