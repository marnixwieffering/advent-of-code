using System.IO;
using System.Collections.Generic;

var input = "./input";
var data = File.ReadAllLines(input);

var scoreTable = new Dictionary<string, int>
{
	{ "BX", 1 },
	{ "CY", 2 },
	{ "AZ", 3 },
	{ "AX", 4 },
	{ "BY", 5 },
	{ "CZ", 6 },
	{ "CX", 7 },
	{ "AY", 8 },
	{ "BZ", 9 },
};

var actualScoreTable = new Dictionary<string, int>
{
	{ "BX", 1 },
	{ "CX", 2 },
	{ "AX", 3 },
	{ "AY", 4 },
	{ "BY", 5 },
	{ "CY", 6 },
	{ "CZ", 7 },
	{ "AZ", 8 },
	{ "BZ", 9 },
};

var points = data.Sum(x => scoreTable[x.Replace(" ", string.Empty)]);
var actualPoints = data.Sum(x => actualScoreTable[x.Replace(" ", string.Empty)]);

Internal.Console.WriteLine(points.ToString());
Internal.Console.WriteLine(actualPoints.ToString());
