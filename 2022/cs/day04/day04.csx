using System.IO;

var input = "./input";
var data = File.ReadAllLines(input).Select(x => x.Split(','));

var contains = 0;
var overlaps = 0;

foreach (var pair in data)
{
	var elfOne = new Bound(pair.First().Split("-").Select(x => int.Parse(x)));
	var elfTwo = new Bound(pair.Last().Split("-").Select(x => int.Parse(x)));

	if (elfOne.lower >= elfTwo.lower && elfOne.upper <= elfTwo.upper) contains++;
	else if ((elfTwo.lower >= elfOne.lower && elfTwo.upper <= elfOne.upper)) contains++;

	if (elfOne.lower >= elfTwo.lower && elfOne.lower <= elfTwo.upper) overlaps++;
	else if (elfTwo.upper <= elfOne.upper && elfTwo.upper >= elfOne.lower) overlaps++;
	else if (elfTwo.lower >= elfOne.lower && elfTwo.lower <= elfOne.upper) overlaps++;
	else if (elfOne.upper <= elfTwo.upper && elfOne.upper >= elfTwo.lower) overlaps++;
}

Internal.Console.WriteLine(contains.ToString());
Internal.Console.WriteLine(overlaps.ToString());

class Bound
{
	public int lower;
	public int upper;

	public Bound(IEnumerable<int> bounds)
	{
		lower = bounds.First();
		upper = bounds.Last();
	}
}
