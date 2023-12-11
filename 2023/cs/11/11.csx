#nullable enable

using System.IO;

var input = "./input";
var data = File.ReadAllLines(input).Select(x => x.ToCharArray().ToList()).ToList();
var galaxies = GetSubset<Pos>(FindGalaxies(), 2);

Internal.Console.WriteLine(Sum(2 - 1).ToString());
Internal.Console.WriteLine(Sum(1000000 - 1).ToString());

public long Sum(long expansion)
{
	long sum = 0;

	foreach (var pair in galaxies)
	{
		var source = pair.First();
		var destination = pair.Last();
		long length = Math.Abs(destination.Y - source.Y) + Math.Abs(destination.X - source.X);

		var yle = source.Y < destination.Y;
		for (int y = yle ? source.Y : destination.Y; y < (yle ? destination.Y : source.Y); y++)
			if (data[y].All(x => x == '.'))
				length += expansion;

		var xle = source.X < destination.X;
		for (int x = xle ? source.X : destination.X; x < (xle ? destination.X : source.X); x++)
			if (data.Select(y => y[x]).All(x => x == '.'))
				length += expansion;

		sum += length;
	}
	return sum;
}

public List<Pos> FindGalaxies()
{
	var galaxies = new List<Pos>();
	for (int y = 0; y < data.Count(); y++)
		for (int x = 0; x < data[0].Count(); x++)
			if (data[y][x] == '#') galaxies.Add(new Pos(x, y));
	return galaxies;
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

public List<List<T>> GetSubset<T>(List<T> set, int size)
{
	var result = new List<List<T>>();
	GetSubsets<T>(new List<T>(set), size, 0, new List<T>(), result);
	return result;
}

private void GetSubsets<T>(List<T> set, int size, int idx, List<T> current, List<List<T>> result)
{
	if (current.Count + (set.Count - idx + 1) <= size) return;

	GetSubsets(set, size, idx + 1, current, result);
	current.Add(set[idx]);

	if (current.Count == size) result.Add(new List<T>(current));
	else if (current.Count + (set.Count - idx + 2) >= size) GetSubsets(set, size, idx + 1, current, result);

	current.RemoveAt(current.Count - 1);
}