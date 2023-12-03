#nullable enable

using System.IO;

var input = "./input";
var data = File.ReadAllLines(input).Select(line => line.ToArray()).ToArray();

var sum = 0;

for (int i = 0; i < data.Count(); i++)
{
	var current = "";
	var parse = false;
	for (int j = 0; j < data[i].Length; j++)
	{
		if (char.IsDigit(data[i][j])) current += data[i][j];
		else if (current == "" && !char.IsDigit(data[i][j])) continue;
		else parse = true;

		if (j == data[i].Length - 1) parse = true;

		if (parse)
		{
			var cancel = false;
			for (int ik = Math.Clamp(i - 1, 0, data.Count() - 1); ik <= Math.Clamp(i + 1, 0, data.Count() - 1) && !cancel; ik++)
				for (int jk = Math.Clamp(j - current.Length - 1, 0, data[i].Length - 1); jk <= j && !cancel; jk++)
					if (!char.IsDigit(data[ik][jk]) && data[ik][jk] != '.')
					{
						sum += int.Parse(current);
						cancel = true;
					}
			current = "";
			parse = false;
		}
	}
}

var ratioSum = 0;

for (int i = 0; i < data.Count(); i++)
	for (int j = 0; j < data[i].Length; j++)
		if (data[i][j] == '*')
		{
			var r1 = "";
			var r2 = "";
			var current = "";
			var cancel = false;

			for (int ik = Math.Clamp(i - 1, 0, data.Count()); ik <= Math.Clamp(i + 1, 0, data.Count()) && !cancel; ik++)
				for (int jk = Math.Clamp(j - 1, 0, data[i].Count()); jk <= Math.Clamp(j + 1, 0, data[i].Count()) && !cancel; jk++)
				{
					if (char.IsDigit(data[ik][jk]))
					{
						current += data[ik][jk];

						for (int jkp = Math.Clamp(jk + 1, 0, data[i].Count() - 1); jkp < data[i].Count(); jkp++)
							if (char.IsDigit(data[ik][jkp])) current += data[ik][jkp];
							else break;
						for (int jkn = Math.Clamp(jk - 1, 0, data[i].Count() - 1); jkn >= 0; jkn--)
							if (char.IsDigit(data[ik][jkn])) current = data[ik][jkn] + current;
							else break;

						if (current == r1)
						{
							current = "";
							continue;
						}

						if (r1 == "") r1 = current;
						else if (r2 == "") r2 = current;
						else cancel = true;

						current = "";
					}
				}

			if (r1 != "" && r2 != "") ratioSum += int.Parse(r1) * int.Parse(r2);
		}

Internal.Console.WriteLine(sum.ToString());
Internal.Console.WriteLine(ratioSum.ToString());
