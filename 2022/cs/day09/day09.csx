using System.IO;

var input = "./input";
var data = File.ReadAllLines(input).Select(x => (x.Split(" ").First(), int.Parse(x.Split(" ").Last())));

var moveTable = new Dictionary<string, (int, int)>{
	{ "R", (1,0) },
	{ "U", (0,1) },
	{ "L", (-1,0) },
	{ "D", (0,-1) },
};

var translationTable = new Dictionary<(int, int), (int, int)>{
	{ (2,0), (1,0) },
	{ (0,2), (0,1) },
	{ (-2,0), (-1,0) },
	{ (0,-2), (0,-1) },

	{ (2,1), (1,1) },
	{ (1,2), (1,1) },
	{ (-1,2), (-1,1) },
	{ (2,-1), (1,-1) },

	{ (-2,1), (-1,1) },
	{ (1,-2), (1,-1) },
	{ (-1,-2), (-1,-1) },
	{ (-2,-1), (-1,-1) },

	{ (2,2), (1,1) },
	{ (-2,-2), (-1,-1) },
	{ (2,-2), (1,-1) },
	{ (-2,2), (-1,1) },
};

var coords = new List<(int, int)>();
var tailPos = (0, 0);
var headPos = (0, 0);

foreach (var move in data)
{
	for (int i = 0; i < move.Item2; i++)
	{
		headPos = add(headPos, moveTable[move.Item1]);
		var distance = sub(headPos, tailPos);
		if (translationTable.Keys.Contains(distance)) tailPos = add(tailPos, translationTable[distance]);
		if (!coords.Contains(tailPos)) coords.Add(tailPos);
	}
}

var knots = new List<(int, int)>();
var coords2 = new List<(int, int)>();

for (int i = 0; i < 10; i++) knots.Add((0, 0));

foreach (var move in data)
{
	for (int i = 0; i < move.Item2; i++)
	{
		knots[0] = add(knots[0], moveTable[move.Item1]);
		for (int j = 1; j < knots.Count(); j++)
		{
			var distance = sub(knots[j - 1], knots[j]);
			if (translationTable.Keys.Contains(distance)) knots[j] = add(knots[j], translationTable[distance]);
		}
		if (!coords2.Contains(knots[knots.Count() - 1])) coords2.Add(knots[knots.Count() - 1]);
	}
}

Internal.Console.WriteLine(coords.Count().ToString());
Internal.Console.WriteLine(coords2.Count().ToString());

public (int, int) add((int, int) a, (int, int) b) => (a.Item1 + b.Item1, a.Item2 + b.Item2);
public (int, int) sub((int, int) a, (int, int) b) => (a.Item1 - b.Item1, a.Item2 - b.Item2);
