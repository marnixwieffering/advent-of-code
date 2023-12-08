#nullable enable

using System.IO;

var input = "./input";
var data = File.ReadAllLines(input);

var names = data.Skip(2).Select(x => new string(x.Take(3).ToArray())).ToList();
var instructions = data[0].ToCharArray().Select(x => x == 'L' ? 0 : 1).ToArray();
var nodes = new List<Node>();

for (int i = 2; i < data.Length; i++)
{
	var node = new Node();
	var pair = data[i].Split('=');
	var targets = pair[1].Replace("(", "").Replace(")", "").Split(',');
	var name = pair[0].Trim();

	node.End = name == "ZZZ";
	node.Start = name == "AAA";
	node.GhostEnd = name.Last() == 'Z';
	node.GhostStart = name.Last() == 'A';
	node.Nodes = new int[] { names.IndexOf(targets[0].Trim()), names.IndexOf(targets[1].Trim()) };

	nodes.Add(node);
}

var current = nodes.FirstOrDefault(x => x.Start);
var steps = 0;

while (current != null && !current.End)
{
	var ins = instructions[steps++ % instructions.Count()];
	current = nodes[current.Nodes[ins]];
}

long ghostSteps = 0;
var starts = nodes.Where(x => x.GhostStart).ToArray();

while (starts.Any(x => !x.GhostEnd))
{
	var ins = instructions[ghostSteps++ % instructions.Count()];
	for (int i = 0; i < starts.Length; i++)
		starts[i] = nodes[starts[i].Nodes[ins]];
}

Internal.Console.WriteLine(steps.ToString());
Internal.Console.WriteLine(ghostSteps.ToString());

public class Node
{
	public bool Start = false;
	public bool End = false;
	public bool GhostEnd = false;
	public bool GhostStart = false;
	public int[] Nodes = new int[2];
}
