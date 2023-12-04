#nullable enable

using System.IO;

var input = "./input";
var data = File.ReadAllLines(input).Select(x => x.Split(':')[1]).ToArray();

var flatScore = 0;
var cards = new Dictionary<int, int>();


for (int i = 0; i < data.Length; i++)
{
	cards.Add(i + 1, 0);
	flatScore += (int)Math.Pow(2, winningNumbs(data[i]) - 1);
}

scoreCards(data, cards, 0, data.Count());

Internal.Console.WriteLine(flatScore.ToString());
Internal.Console.WriteLine(cards.Values.Sum().ToString());

public void scoreCards(string[] data, Dictionary<int, int> cards, int start, int end)
{
	for (int i = start; i < end; i++)
	{
		cards[i + 1] += 1;
		scoreCards(data, cards, i + 1, Math.Clamp(i + 1 + winningNumbs(data[i]), 0, data.Length));
	}
}

public int winningNumbs(string input)
{
	var pair = input.Split("|");
	var numbers = pair[0].Trim().Replace("  ", " ").Split(" ").Select(x => int.Parse(x.Trim()));
	var winning = pair[1].Trim().Replace("  ", " ").Split(" ").Select(x => int.Parse(x.Trim()));
	return numbers.Intersect(winning).Count();
}
