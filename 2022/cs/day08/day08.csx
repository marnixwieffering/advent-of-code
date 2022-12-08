using System.IO;

var input = "./input";
var data = File.ReadAllLines(input).Select(x => x.Select(y => int.Parse(y.ToString())).ToArray()).ToArray();

var visible = VisibleTrees(data);
var score = BestTreeScore(data);

Internal.Console.WriteLine(visible.ToString());
Internal.Console.WriteLine(score.ToString());

public int BestTreeScore(int[][] data)
{
	var score = 0;
	for (int i = 0; i < data.Length; i++)
	{
		for (int j = 0; j < data[i].Length; j++)
		{
			var up = TraceScore(i, -1, 0, true, -1, true, i, j);
			var down = TraceScore(i, 1, data.Length, false, 1, true, i, j);
			var left = TraceScore(j, -1, 0, true, -1, false, i, j);
			var right = TraceScore(j, 1, data[i].Length, false, 1, false, i, j);
			var local = up * down * left * right;
			if (local > score) score = local;
		}
	}
	return score;
}

public int VisibleTrees(int[][] data)
{
	var visible = (data.Length * 2) + ((data[0].Length - 2) * 2);
	for (int i = 1; i < data.Length - 1; i++)
	{
		for (int j = 1; j < data[i].Length - 1; j++)
		{
			var canSee = false;
			canSee |= TraceVisibility(i, -1, 0, true, -1, true, i, j); // up
			canSee |= TraceVisibility(i, 1, data.Length, false, 1, true, i, j); // down
			canSee |= TraceVisibility(j, -1, 0, true, -1, false, i, j); // left
			canSee |= TraceVisibility(j, 1, data[i].Length, false, 1, false, i, j); // right
			if (canSee) visible++;
		}
	}
	return visible;
}

public bool TraceVisibility(int start, int offset, int end, bool direction, int increment, bool ortho, int i, int j)
{
	var local = true;
	for (int k = start + offset; direction ? k >= end : k < end; k += increment)
		local &= ortho ? data[i][j] > data[k][j] : data[i][j] > data[i][k];
	return local;
}

public int TraceScore(int start, int offset, int end, bool direction, int increment, bool ortho, int i, int j)
{
	var score = 0;
	for (int k = start + offset; direction ? k >= end : k < end; k += increment)
		if (ortho ? data[i][j] > data[k][j] : data[i][j] > data[i][k]) score++;
		else
		{
			score++;
			break;
		}
	return score;
}
