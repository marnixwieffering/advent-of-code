#nullable enable

using System.IO;

var input = "./input";
var data = File.ReadAllLines(input).Select(x => x.Split(':')[1]).ToArray();

var stock = new Dictionary<string, int>() { { "red", 12 }, { "green", 13 }, { "blue", 14 } };
var sum = 0;

for (int i = 0; i < data.Length; i++)
	if (data[i].Split(';').All(
		round => round.Split(',').Select(x => x.TrimStart().Split(' ')).ToArray().All(
			entry => int.Parse(entry[0]) <= stock[entry[1]])
	)) sum += (i + 1);

var power = 0;

for (int i = 0; i < data.Length; i++)
{
	var neededStock = new Dictionary<string, int>() { { "red", 0 }, { "green", 0 }, { "blue", 0 } };
	foreach (var round in data[i].Split(';'))
		foreach (var entry in round.Split(',').Select(x => x.TrimStart().Split(' ')).ToArray())
			if (neededStock[entry[1]] < int.Parse(entry[0])) neededStock[entry[1]] = int.Parse(entry[0]);
	power += neededStock.Values.Aggregate(1, (a, b) => a * b);
}

Internal.Console.WriteLine(sum.ToString());
Internal.Console.WriteLine(power.ToString());
