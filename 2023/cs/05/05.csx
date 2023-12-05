#nullable enable

using System.IO;

var input = "./input";
var data = File.ReadAllLines(input);

var parsed = parseInput(data);
var maps = parsed.Item1;
var seeds = parsed.Item2;
var extendedSeeds = parsed.Item3;

var lowest = long.MaxValue;

foreach (var seed in seeds)
{
	var location = getLocation(maps, seed);
	if (location < lowest) lowest = location;
}

var extendedLowest = long.MaxValue;

foreach (var seed in extendedSeeds)
{
	var location = getLocation(maps, seed);
	if (location < extendedLowest) extendedLowest = location;
}

Internal.Console.WriteLine(lowest.ToString());
Internal.Console.WriteLine(extendedLowest.ToString());


public class Map
{
	public string source = null!;
	public string destination = null!;
	public MapEntry[] entries = null!;
}

public class MapEntry
{
	public long source;
	public long destination;
	public long range;
}

public long getLocation(Map[] maps, (string, long) source)
{
	while (source.Item1 != "location")
		source = ApplyMap(maps, source);
	return source.Item2;
}

public (string, long) ApplyMap(Map[] maps, (string, long) source)
{
	Map targetMap = maps.First(x => x.source == source.Item1);
	MapEntry? targetEntry = targetMap.entries.FirstOrDefault(x => source.Item2 < x.source + x.range && source.Item2 >= x.source);

	if (targetEntry == null) return (targetMap.destination, source.Item2);

	return (targetMap.destination, targetEntry.destination + source.Item2 - targetEntry.source);
}

public (Map[], (string, long)[], (string, long)[]) parseInput(string[] data)
{
	Map[] maps = new Map[] { };
	(string, long)[] seeds = new (string, long)[] { };
	(string, long)[] extendedSeeds = new (string, long)[] { };

	Map? currentMap = null;
	foreach (var line in data)
	{
		if (line.Contains("seeds"))
		{
			var numbers = line.Split(":")[1].TrimStart().Split(' ').Select(x => long.Parse(x)).ToArray();
			seeds = numbers.Select(x => ("seed", x)).ToArray();
			for (int i = 0; i < numbers.Length; i += 2)
				for (long j = numbers[i]; j < numbers[i] + numbers[i + 1]; j++)
					extendedSeeds = extendedSeeds.Append(("seed", j)).ToArray();
		}
		else if (line.Contains(":"))
		{
			if (currentMap != null)
			{
				maps = maps.Append(currentMap).ToArray();
				currentMap = null;
			}

			var translation = line.Replace(" map:", "").Replace("-to", "").Split('-');
			currentMap = new Map
			{
				destination = translation[1],
				source = translation[0],
				entries = new MapEntry[] { }
			};
		}
		else if (line != "" && currentMap != null && currentMap.entries != null)
		{
			var numbers = line.Split(" ").Select(x => long.Parse(x)).ToArray();
			currentMap.entries = currentMap.entries.Append(new MapEntry
			{
				destination = numbers[0],
				source = numbers[1],
				range = numbers[2]
			}).ToArray();
		}
	}

	maps = maps.Append(currentMap!).ToArray();

	return (maps, seeds, extendedSeeds);
}
