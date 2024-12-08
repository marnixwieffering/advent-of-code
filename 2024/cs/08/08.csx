#nullable enable

using System.IO;

var data = File.ReadAllLines("./input").Select(x => x.ToCharArray()).ToArray();
var inRange = (int x, int y) => x >= 0 && x < data.Length && y >= 0 && y < data[x].Length;

var locations = new Dictionary<char, List<Point>>();

for (int x = 0; x < data.Length; x++)
{
	for (int y = 0; y < data[x].Length; y++)
	{
		if (data[x][y] == '.') continue;
		List<Point> points;
		if (locations.ContainsKey(data[x][y])) points = locations[data[x][y]];
		else
		{
			locations.Add(data[x][y], new List<Point>());
			points = locations[data[x][y]];
		}
		points.Add(new Point() { X = x, Y = y });
	}
}

var nodes = 0;

foreach (var pair in locations)
{
	var subsets = pair.Value.SelectMany((item1, index1) =>
						pair.Value.Skip(index1 + 1).Select(item2 => (item1, item2)))
						  .ToList();

	foreach (var set in subsets)
	{
		var delta = (set.item2 - set.item1);
		var place = (Point p) =>
				{
					if (!inRange(p.X, p.Y) || data[p.X][p.Y] == '#') return;
					data[p.X][p.Y] = '#';
					nodes++;
				};

		var t2 = set.item2;
		while (inRange(t2.X, t2.Y))
		{
			place(t2);
			t2 += delta;
		}

		var t1 = set.item1;
		while (inRange(t1.X, t1.Y))
		{
			place(t1);
			t1 -= delta;
		}
	}

}

Internal.Console.WriteLine(nodes.ToString());

public class Point
{
	public int X;
	public int Y;

	public static Point operator +(Point a, Point b)
	{
		return new Point { X = a.X + b.X, Y = a.Y + b.Y };
	}

	public static Point operator -(Point a, Point b)
	{
		return new Point { X = a.X - b.X, Y = a.Y - b.Y };
	}
}
