#nullable enable

using System.IO;

var data = File.ReadAllLines("./input").Select(line => line.ToCharArray()).ToArray();

var start = data
	.SelectMany((row, x) => row
		.Select((ch, y) => new State { X = x, Y = y, Ch = ch, Dx = -1, Dy = 0 }))
	.Where(pos => pos.Ch == '^')
	.First();

var marks = new[] { '+', '-', '|' };
var current = start.Clone();
var inRange = (int x, int y) => x >= 0 && x < data.Length && y >= 0 && y < data[x].Length;
var turn = (int x, int y) =>
{
	var nDx = 0;
	if (Math.Abs(x) == 1) nDx = 0;
	else if (x == 0) nDx = y;

	var nDy = 0;
	if (Math.Abs(y) == 1) nDy = 0;
	else if (y == 0) nDy = -x;

	return new { x = nDx, y = nDy };
};

bool navigate(char[][] map)
{
	var i = 0;
	while (inRange(current.X, current.Y) && i < 10000)
	{
		i++;
		var mark = Math.Abs(current.Dx) == 1 ? '|' : '-';

		if (!inRange(current.X + current.Dx, current.Y + current.Dy))
		{
			map[current.X][current.Y] = mark;
			break;
		}

		var val = map[current.X + current.Dx][current.Y + current.Dy];
		if (val == '.' || marks.Any(v => v == val))
		{
			if (marks.Any(v => v == map[current.X][current.Y])) mark = '+';
			map[current.X][current.Y] = mark;
			current.X += current.Dx;
			current.Y += current.Dy;
		}
		else if (val == '#')
		{
			var n = turn(current.Dx, current.Dy);
			map[current.X][current.Y] = '+';
			current.Dx = n.x;
			current.Dy = n.y;
		}
	}
	if (i == 10000) return true;
	return false;
}

var loops = 0;

for (int x = 0; x < data.Length; x++)
{
	for (int y = 0; y < data[x].Length; y++)
	{
		current = start.Clone();
		var clone = data.Select(row => row.ToArray()).ToArray();
		clone[x][y] = '#';
		if (navigate(clone)) loops++;
	}
}

Internal.Console.WriteLine(loops.ToString());

public class State
{
	public int X { get; set; }
	public int Y { get; set; }
	public char Ch { get; set; }
	public int Dx { get; set; }
	public int Dy { get; set; }

	public State Clone() => new State { X = this.X, Y = this.Y, Ch = this.Ch, Dx = this.Dx, Dy = this.Dy };
}
