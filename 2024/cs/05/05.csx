#nullable enable

using System.IO;

var data = File.ReadAllLines("./input");

var rules = data.Where(line => line.Contains('|')).Select(line => line.Split('|').Select(int.Parse).ToArray()).ToArray();
var manual = data.Where(line => line.Contains(',')).Select(line => line.Split(',').Select(int.Parse).ToArray()).ToArray();

var total = 0;

var incorrect = new List<int[]>();

foreach (var pages in manual)
{
	var correct = true;

	for (int i = 0; i < pages.Count(); i++)
	{
		for (int j = 0; j < pages.Count(); j++)
		{
			if (i == j) continue;

			if (i < j && rules.Any(rule => rule[0] == pages[j] && rule[1] == pages[i])) correct = false;
			if (j < i && rules.Any(rule => rule[0] == pages[i] && rule[1] == pages[j])) correct = false;
		}
	}

	if (correct) total += pages.ElementAt(pages.Count() / 2);
	else incorrect.Add(pages);
}

Internal.Console.WriteLine(total.ToString());

var total2 = 0;

foreach (var pages in incorrect)
{
	foreach (var page in pages)
	{
		var before = rules.Count(x => x[0] == page && pages.Any(y => y == x[1]));
		var after = rules.Count(x => x[1] == page && pages.Any(y => y == x[0]));
		if (before == after)total2 += page;
	}
}

Internal.Console.WriteLine(total2.ToString());

