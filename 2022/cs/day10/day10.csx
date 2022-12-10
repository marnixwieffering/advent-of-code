using System.IO;

var input = "./input";
var data = File.ReadAllLines(input).Select(x => (x.Split(" ").First(), x.Split(" ").Count() == 1 ? 0 : int.Parse(x.Split(" ").Last())));

var register = 1;
var signalStrength = 0;
var cycle = 0;

var cycleMap = new Dictionary<string, int>() { { "addx", 2 }, { "noop", 1 } };
var operationMap = new Dictionary<string, Func<int, int>>() { { "addx", x => x }, { "noop", x => 0 } };

foreach (var operation in data)
{
	for (int i = 0; i < cycleMap[operation.Item1]; i++)
		if (++cycle == 20 || cycle % 40 == 20) signalStrength += cycle * register;
	register += operationMap[operation.Item1](operation.Item2);
}

var CRT = new char[6 * 40];
register = 1;
cycle = 0;

foreach (var operation in data)
{
	for (int i = 0; i < cycleMap[operation.Item1]; i++)
		CRT[cycle] = new[] { register - 1, register, register + 1 }.Contains(cycle++ % 40) ? '#' : '.';
	register += operationMap[operation.Item1](operation.Item2);
}

Internal.Console.WriteLine(signalStrength.ToString());
printCRT(CRT);

public void printCRT(char[] CRT)
{
	foreach (var row in CRT.Chunk(40))
	{
		foreach (var pixel in row)
			Internal.Console.Write(pixel.ToString());
		Internal.Console.WriteLine();
	}
}
