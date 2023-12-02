#nullable enable

using System.IO;

var input = "./input";
var data = File.ReadAllLines(input);

var sum = 0;
var literalSum = 0;

var numbers = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
var literals = new List<string>() { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

foreach (var line in data)
{
	var firstNumber = numbers.OrderBy(x => line.IndexOf(x)).Where(x => line.IndexOf(x) != -1).First();
	var lastNumber = numbers.OrderBy(x => line.LastIndexOf(x)).Where(x => line.LastIndexOf(x) != -1).Last();

	var first = numbers.IndexOf(firstNumber) + 1;
	var last = numbers.IndexOf(lastNumber) + 1;

	sum += int.Parse($"{first}{last}");

	var firstLiteral = literals.OrderBy(x => line.IndexOf(x)).Where(x => line.IndexOf(x) != -1).FirstOrDefault();
	var lastLiteral = literals.OrderBy(x => line.LastIndexOf(x)).Where(x => line.LastIndexOf(x) != -1).LastOrDefault();

	if (firstLiteral != null && line.IndexOf(firstLiteral) < line.IndexOf(firstNumber)) first = literals.IndexOf(firstLiteral) + 1;
	if (lastLiteral != null && line.LastIndexOf(lastLiteral) > line.LastIndexOf(lastNumber)) last = literals.IndexOf(lastLiteral) + 1;

	literalSum += int.Parse($"{first}{last}");
}

Internal.Console.WriteLine(sum.ToString());
Internal.Console.WriteLine(literalSum.ToString());
