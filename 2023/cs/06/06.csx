#nullable enable
#r "System.Text.RegularExpressions"

using System.IO;
using System.Text.RegularExpressions;

var input = "./input";
var data = File.ReadAllLines(input);

int[] times = Regex.Replace(data[0].Split(':')[1].Trim(), @"\s+", " ").Split(' ').Select(x => int.Parse(x.Trim())).ToArray();
int[] distances = Regex.Replace(data[1].Split(':')[1].Trim(), @"\s+", " ").Split(' ').Select(x => int.Parse(x.Trim())).ToArray();

var product = 1;

for (int i = 0; i < times.Count(); i++)
{
	var valid = 0;
	for (int j = 0; j < times[i]; j++) if ((times[i] - j) * j > distances[i]) valid++;
	product *= valid;
}

var time = long.Parse(data[0].Split(':')[1].Replace(" ", ""));
var distance = long.Parse(data[1].Split(':')[1].Replace(" ", ""));

var valid = 0;
for (int j = 0; j < time; j++) if ((time - j) * j > distance) valid++;

Internal.Console.WriteLine(product.ToString());
Internal.Console.WriteLine(valid.ToString());
