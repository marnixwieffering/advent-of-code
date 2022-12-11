using System.IO;

var input = "./input";
var data = File.ReadAllLines(input);

var operationMap = new Dictionary<string, Func<int, int, int>>() {
	{"+", (x, y) => x + y},
	{"*", (x, y) => x * y},
};

var monkeys = parse(data);
var monkeys2 = parse(data);

for (int i = 0; i < 20; i++)
	foreach (var monkey in monkeys)
		while (monkey.items.Count() > 0)
		{
			monkey.inspect++;
			var item = monkey.items.Dequeue();
			var operand = monkey.operationParam == 0 ? item : monkey.operationParam;
			item = monkey.operation(item, operand) / 3;
			monkeys[monkey.test(item)].items.Enqueue(item);
		}

int modules = 1;
foreach (var monkey in monkeys2)
	modules *= monkey.divider;

for (int i = 0; i < 10000; i++)
	foreach (var monkey in monkeys2)
		while (monkey.items.Count() > 0)
		{
			monkey.inspect++;
			var item = monkey.items.Dequeue();
			var operand = monkey.operationParam == 0 ? item : monkey.operationParam;
			item = monkey.operation(item, operand) % modules;
			monkeys2[monkey.test(item)].items.Enqueue(item);
		}

var active = monkeys.OrderByDescending(x => x.inspect).Take(2).ToArray();
var business = active[0].inspect * active[1].inspect;

var active2 = monkeys2.OrderByDescending(x => x.inspect).Take(2).ToArray();
var business2 = active2[0].inspect * active2[1].inspect;

Internal.Console.WriteLine(business.ToString());
Internal.Console.WriteLine(business2.ToString());

public List<Monkey> parse(string[] data)
{
	var monkeys = new List<Monkey>();
	var templates = data.Chunk(7).Select(x => x.Take(6));

	foreach (var template in templates)
	{
		var monkey = new Monkey();
		monkey.id = int.Parse(template.ElementAt(0)[7].ToString());
		var items = template.ElementAt(1).Replace("  Starting items: ", string.Empty).Split(",").Select(x => int.Parse(x));
		foreach (var item in items) monkey.items.Enqueue(item);
		monkey.operation = operationMap[template.ElementAt(2).Replace("  Operation: new = old ", string.Empty)[0].ToString()];
		var op = new string(template.ElementAt(2).Replace("  Operation: new = old ", string.Empty).Skip(2).ToArray());
		monkey.operationParam = op.Contains("old") ? 0 : int.Parse(op);
		monkey.divider = int.Parse(template.ElementAt(3).Replace("  Test: divisible by ", string.Empty));
		var divider = int.Parse(template.ElementAt(3).Replace("  Test: divisible by ", string.Empty));
		var ifTrue = int.Parse(template.ElementAt(4).Replace("    If true: throw to monkey ", string.Empty));
		var ifFalse = int.Parse(template.ElementAt(5).Replace("    If false: throw to monkey ", string.Empty));
		monkey.test = x => x % divider == 0 ? ifTrue : ifFalse;
		monkeys.Add(monkey);
	}
	return monkeys;
}

public class Monkey
{
	public int id { get; set; }
	public int inspect = 0;
	public int operationParam { get; set; }
	public int divider { get; set; }
	public Queue<int> items = new Queue<int>();
	public Func<int, int, int> operation { get; set; }
	public Func<int, int> test { get; set; }

	public void log()
	{
		Internal.Console.WriteLine(this.id.ToString());
		Internal.Console.WriteLine(this.inspect.ToString());
		foreach (var item in this.items) Internal.Console.Write(item.ToString() + ",");
		Internal.Console.WriteLine();
	}
}
