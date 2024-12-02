#nullable enable

using System.IO;

var data = File.ReadAllLines("./input").Select(x => x.Split(' ').Select(int.Parse)).ToArray();

Internal.Console.WriteLine(data.Count(isValid).ToString());

var safe = data.Where(report =>
{
	if (isValid(report)) return true;

	for (int i = 0; i < report.Count(); i++)
		if (isValid(report.Where((_, j) => j != i).ToArray())) return true;

	return false;
});

Internal.Console.WriteLine(safe.Count().ToString());

bool isValid(IEnumerable<int> sequence)
{
	var asc = sequence.SequenceEqual(sequence.Order());
	var dsc = sequence.SequenceEqual(sequence.OrderDescending());
	var difference = sequence.Zip(sequence.Skip(1), (a, b) => Math.Abs(b - a))
		.All(diff => diff >= 1 && diff <= 3);

	return difference && (asc || dsc);
}
