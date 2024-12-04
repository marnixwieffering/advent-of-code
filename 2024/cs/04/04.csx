#nullable enable
#r "System.Text.RegularExpressions"

using System.IO;
using System.Text.RegularExpressions;

var data = File.ReadAllLines("./input").Select(line => line.ToCharArray()).ToArray();

int count(int x, int y, int dx, int dy)
{
	var line = string.Empty;
	while (x >= 0 && x < data.Length && y >= 0 && y < data[x].Length)
	{
		line += data[x][y];
		x += dx;
		y += dy;
	}

	if (line.Length < 4) return 0;

	var xmas = Enumerable.Range(0, line.Length - "XMAS".Length + 1).Count(i => line.Substring(i, "XMAS".Length) == "XMAS");
	var smax = Enumerable.Range(0, line.Length - "SAMX".Length + 1).Count(i => line.Substring(i, "SAMX".Length) == "SAMX");

	return xmas + smax;
}

var total = 0;

for (int x = 0; x < data.Length; x++)
{
	total += count(x, 0, 0, 1);
	total += count(x, 0, 1, 1);
	total += count(x, 0, -1, 1);
}

for (int y = 0; y < data[0].Length; y++)
{
	total += count(0, y, 1, 0);
	if (y != 0) total += count(0, y, 1, 1);
	if (y != 0) total += count(data.Length - 1, y, -1, 1);
}

Internal.Console.WriteLine(total.ToString());

var total2 = 0;

for (int x = 1; x < data.Length - 1; x++)
{
	for (int y = 1; y < data[0].Length - 1; y++)
	{
		if (data[x][y] != 'A') continue;

		var tl = data[x - 1][y - 1];
		var tr = data[x - 1][y + 1];
		var bl = data[x + 1][y - 1];
		var br = data[x + 1][y + 1];

		if (tl == 'S' && tr == 'S' && bl == 'M' && br == 'M') total2++;
		else if (tl == 'M' && tr == 'M' && bl == 'S' && br == 'S') total2++;
		else if (tl == 'M' && tr == 'S' && bl == 'M' && br == 'S') total2++;
		else if (tl == 'S' && tr == 'M' && bl == 'S' && br == 'M') total2++;
	}
}

Internal.Console.WriteLine(total2.ToString());
