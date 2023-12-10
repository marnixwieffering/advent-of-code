#nullable enable

using System.IO;

var input = "./input";
var data = File.ReadAllLines(input).Select(x => x.ToCharArray()).ToArray();

var pipes = new char[] { '-', '|', 'L', 'F', '7', 'J' };
var maps = new Dictionary<char, Func<Pos, Pos, Pos>>();

maps.Add('-', (i, j) => i.X < j.X ? new Pos(i.X + 2, i.Y) : new Pos(i.X - 2, i.Y));
maps.Add('|', (i, j) => i.Y < j.Y ? new Pos(i.X, i.Y + 2) : new Pos(i.X, i.Y - 2));
maps.Add('L', (i, j) => i.X > j.X ? new Pos(i.X - 1, i.Y - 1) : new Pos(i.X + 1, i.Y + 1));
maps.Add('F', (i, j) => i.X > j.X ? new Pos(i.X - 1, i.Y + 1) : new Pos(i.X + 1, i.Y - 1));
maps.Add('7', (i, j) => i.X < j.X ? new Pos(i.X + 1, i.Y + 1) : new Pos(i.X - 1, i.Y - 1));
maps.Add('J', (i, j) => i.X < j.X ? new Pos(i.X + 1, i.Y - 1) : new Pos(i.X - 1, i.Y + 1));

var actions = new Dictionary<Pos, List<Pos>>();

for (int y = 0; y < data.Length; y++)
	for (int x = 0; x < data[y].Length; x++)
		if (data[y][x] == 'S')
			actions.Add(new Pos(x, y), new List<Pos>() {
				new Pos(x - 1, y),
				new Pos(x, y + 1),
			});

var i = 0;

while (actions.Count() > 0)
{
	var next = new Dictionary<Pos, List<Pos>>();
	var any = false;
	foreach (var (start, targets) in actions)
	{
		foreach (var target in targets)
			if (bound(target.X, target.Y) && pipes.Contains(data[target.Y][target.X]))
			{
				any = true;
				next.Add(target, new List<Pos>() { maps[data[target.Y][target.X]](start, target) });
			}
		data[start.Y][start.X] = 'X';
	}
	actions = next;
	if (any) i++;
}

data[0][0] = 'O';

var markers = new Dictionary<Pos, List<Pos>>() { { new Pos(0, 0), adjacent(new Pos(0, 0)) } };
while (markers.Count() > 0)
{
	var newMarkers = new Dictionary<Pos, List<Pos>>();
	foreach (var (start, targets) in markers)
	{
		foreach (var target in targets)
			if (bound(target.X, target.Y) && data[target.Y][target.X] != 'X' && data[target.Y][target.X] != 'O')
			{
				newMarkers.Add(target, adjacent(target));
				data[target.Y][target.X] = 'O';
			}
	}
	markers = newMarkers;
}

var inner = data.Sum(x => x.Count(x => x == '.'));

Internal.Console.WriteLine(i.ToString());
Internal.Console.WriteLine(inner.ToString());

public List<Pos> adjacent(Pos pos)
{
	return new List<Pos>(){
		new Pos(pos.X+1, pos.Y),
		new Pos(pos.X-1, pos.Y),
		new Pos(pos.X, pos.Y+1),
		new Pos(pos.X, pos.Y-1),
	};
}

public bool bound(int x, int y)
{
	return y < data.Length && y >= 0 && x < data[0].Length && x >= 0;
}

public class Pos
{
	public Pos(int x, int y)
	{
		this.X = x;
		this.Y = y;
	}
	public int X;
	public int Y;
}