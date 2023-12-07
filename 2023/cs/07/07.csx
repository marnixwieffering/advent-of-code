#nullable enable

using System.IO;

var input = "./input";
var data = File.ReadAllLines(input).Select(x => x.Split(' ')).ToArray();
var cards1 = new List<char>() { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
var cards2 = new List<char>() { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };

Internal.Console.WriteLine(product(orderHands(data, cards1)).ToString());
Internal.Console.WriteLine(product(orderHands(data, cards2, true)).ToString());

public long product(string[][] order)
{
	long product = 0;
	for (int i = 1; i <= order.Count(); i++)
		product += i * int.Parse(order[i - 1][1]);
	return product;
}

public string[][] orderHands(string[][] data, List<char> cards, bool useJoker = false)
{
	return data.OrderBy(x => solveHand(cards, x[0], useJoker))
		.ThenBy(x => cards.IndexOf(x[0][0]))
		.ThenBy(x => cards.IndexOf(x[0][1]))
		.ThenBy(x => cards.IndexOf(x[0][2]))
		.ThenBy(x => cards.IndexOf(x[0][3]))
		.ThenBy(x => cards.IndexOf(x[0][4]))
		.ToArray();
}

public int solveHand(List<char> cards, string input, bool useJoker)
{
	if (solveNHand(cards, input, 5, useJoker)) return 6;
	if (solveNHand(cards, input, 4, useJoker)) return 5;
	if (solveNMHand(cards, input, 3, 2, useJoker)) return 4;
	if (solveNHand(cards, input, 3, useJoker)) return 3;
	if (solveNMHand(cards, input, 2, 2, useJoker)) return 2;
	if (solveNHand(cards, input, 2, useJoker)) return 1;
	return 0;
}

public bool solveNHand(List<char> cards, string input, int n, bool useJoker)
{
	foreach (var card in cards)
		if (input.Count(x => x == card || (useJoker && x == 'J')) == n) return true;
	return false;
}

public bool solveNMHand(List<char> cards, string input, int n, int m, bool useJoker)
{
	foreach (var first in cards)
		if (input.Count(x => x == first || (useJoker && x == 'J')) == n)
			foreach (var second in cards.Where(x => x != first))
				if (input.Count(x => x == second || (useJoker && x == 'J')) == m)
					if (m + n - input.Count(x => x == second) + input.Count(x => x == first) > input.Count(x => x == 'J')) continue;
					else return true;
	return false;
}
