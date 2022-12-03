using System.IO;

var input = "./input";
var data = File.ReadAllLines(input);

var score = 0;
var badgeScore = 0;

foreach (var rucksack in data.Select(x => x.Chunk(x.Length / 2)))
	score += findMatch(new string(rucksack.First()), new string(rucksack.Last()));

foreach (var rucksack in data.Chunk(3))
	badgeScore += findBadge(rucksack[0], rucksack[1], rucksack[2]);

Internal.Console.WriteLine(score.ToString());
Internal.Console.WriteLine(badgeScore.ToString());

public int charToScore(char value)
{
	int converted = (int)value;
	if (converted >= 65 && converted <= 90) return converted - 38;
	else return converted - 96;
}

public int findMatch(string first, string last)
{
	foreach (var target in first)
		foreach (var value in last)
			if (target == value) return charToScore(target);
	return 0;
}

public int findBadge(string first, string second, string third)
{
	foreach (var f in first)
		foreach (var s in second)
			foreach (var t in third)
				if (f == s && s == t) return charToScore(f);
	return 0;
}
