using System.IO;

var input = "./input";
var data = File.ReadAllLines(input).Select(x => x.Select(y => charToInt(y)).ToArray()).ToArray();

var end = (0, 0);
var paths1 = new List<Path>();
var paths2 = new List<Path>();

for (int i = 0; i < data.Length; i++)
	for (int j = 0; j < data[i].Length; j++)
	{
		if (data[i][j] == -1) paths1.Add(new Path((i, j)));
		else if (data[i][j] == 26) end = (i, j);
		else if (data[i][j] == 0) paths2.Add(new Path((i, j)));
	}

paths1 = findPath(data, paths1, end);
paths2 = findPath(data, paths2, end);

var completedPathsFromStart = paths1.OrderByDescending(x => x.nMoves).First();
var completedPaths = paths2.OrderByDescending(x => x.nMoves).First();

Internal.Console.WriteLine(completedPathsFromStart.nMoves.ToString());
Internal.Console.WriteLine(completedPaths.nMoves.ToString());

public bool repeat(List<(int, int)> moves, (int, int) a) => moves.Contains(a);
public bool movable(int[][] data, (int, int) a, (int, int) b) => data[a.Item1][a.Item2] + 1 >= data[b.Item1][b.Item2];
public bool contains(int height, int width, (int, int) a) => a.Item1 >= 0 && a.Item1 < height && a.Item2 >= 0 && a.Item2 < width;
public (int, int) add((int, int) a, (int, int) b) => (a.Item1 + b.Item1, a.Item2 + b.Item2);

public List<Path> findPath(int[][] data, List<Path> paths, (int, int) end)
{
	var movements = new List<(int, int)>() { { (0, 1) }, { (1, 0) }, { (-1, 0) }, { (0, -1) }, };

	while (anyPath(paths, end))
	{
		var newPaths = new List<Path>();
		foreach (var path in paths)
			foreach (var option in movements.Select(x => add(path.current, x)))
				if (contains(data.Length, data[0].Length, option) && movable(data, path.current, option) && !repeat(path.moves, option))
				{
					var newPath = new Path(path.moves.First());
					foreach (var move in path.moves.Skip(1)) newPath.moves.Add(move);
					newPath.nMoves = path.nMoves;
					newPath.current = path.current;
					newPath.move(option);
					if (!containsPath(paths, newPath) && !containsPath(newPaths, newPath)) newPaths.Add(newPath);
				}
		paths = trimPaths(newPaths);
	}
	return paths;
}

public List<Path> trimPaths(List<Path> paths)
{
	var newPaths = new List<Path>();
	foreach (var path in paths)
		if (!containsPath(paths, path)) newPaths.Add(path);
	return paths;
}

public bool anyPath(List<Path> paths, (int, int) end)
{
	foreach (var path in paths) if (path.current == end) return false;
	return true;
}

public bool containsPath(List<Path> paths, Path target)
{
	foreach (var path in paths) if (containsMoves(path, target)) return true;
	return false;
}

public bool containsMoves(Path a, Path b)
{
	if (a.moves.Last() == b.moves.Last()) return true;
	if (a.moves.Count() != b.moves.Count()) return false;
	for (int i = 0; i < a.moves.Count(); i++)
		if (a.moves[i] != b.moves[i]) return false;
	return true;
}

public class Path
{
	public List<(int, int)> moves = new List<(int, int)>();
	public (int, int) current;
	public int nMoves = 0;

	public Path((int, int) start)
	{
		current = start;
		moves.Add(start);
	}

	public void move((int, int) target)
	{
		current = target;
		nMoves++;
		moves.Add(target);
	}
}

public int charToInt(char value)
{
	int converted = (int)value;
	if (converted >= 97 && converted <= 122) return converted - 97;
	else if (value == 'S') return -1;
	else if (value == 'E') return 26;
	return -2;
}
