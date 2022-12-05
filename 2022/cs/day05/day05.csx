using System.IO;

var input = "./input";
var data = File.ReadAllLines(input);

var stackCount = parseStackCount(data);
var commands = parseCommands(data);

var stacks9000 = parseStacks(data, stackCount);
var stacks9001 = parseStacks(data, stackCount);
var code9000 = "";
var code9001 = "";

foreach (var command in commands)
	for (int i = 0; i < command.n; i++)
		stacks9000[command.target].Push(stacks9000[command.source].Pop());

foreach (var command in commands)
{
	var temp = new Stack<char>();
	for (int i = 0; i < command.n; i++) temp.Push(stacks9001[command.source].Pop());
	for (int i = 0; i < command.n; i++) stacks9001[command.target].Push(temp.Pop());
}

for (int i = 0; i < stackCount; i++) code9000 += stacks9000[i].Pop();
for (int i = 0; i < stackCount; i++) code9001 += stacks9001[i].Pop();

Internal.Console.WriteLine(code9000.ToString());
Internal.Console.WriteLine(code9001.ToString());

#region parsing

public int parseStackCount(string[] input)
{
	var count = -1;
	foreach (var row in input)
		if (row.Contains("move") || row.Contains("[") || row == string.Empty) continue;
		else count = int.Parse(row.TrimEnd().Last().ToString());
	return count;
}

public List<Stack<char>> parseStacks(string[] input, int count)
{
	var stacks = new List<Stack<char>>();
	for (int i = 0; i < count; i++) stacks.Add(new Stack<char>());

	foreach (var row in input.Reverse())
	{
		if (!row.Contains("[")) continue;
		var chunk = row.Chunk(4).Select(x => x[1]).ToArray();
		for (int i = 0; i < count; i++)
		{
			if (chunk[i] == ' ') continue;
			else stacks[i].Push(chunk[i]);
		}
	}
	return stacks;
}

public List<Command> parseCommands(string[] input)
{
	var output = new List<Command>();
	foreach (var row in input)
	{
		if (!row.Contains("move")) continue;
		var numbers = row.Replace("move ", "").Replace("from ", "").Replace("to ", "").Split(" ").Select(x => int.Parse(x)).ToArray();
		output.Add(new Command() { n = numbers[0], source = numbers[1] - 1, target = numbers[2] - 1 });
	}
	return output;
}

public class Command
{
	public int n { get; set; }
	public int source { get; set; }
	public int target { get; set; }
}

#endregion
