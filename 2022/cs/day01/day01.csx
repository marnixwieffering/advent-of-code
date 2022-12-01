using System.IO;

var input = "./input";

var data = File.ReadAllLines(input)
	.Split(string.Empty)
	.Select(array => Array.ConvertAll(array.ToArray(), x => int.Parse(x)).Sum());

var mostCalories = data.Max();
var topThreeMostCalories = data.OrderByDescending(x => x).Take(3).Sum();

Internal.Console.WriteLine(mostCalories.ToString());
Internal.Console.WriteLine(topThreeMostCalories.ToString());

public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this TSource[] array, TSource delimiter)
{
	var output = new List<List<TSource>>();
	var temp = new List<TSource>();
	foreach (var item in array)
		if (!item.Equals(delimiter)) temp.Add(item);
		else output.Add(temp = new List<TSource>());
	output.Add(temp);
	return output;
}
