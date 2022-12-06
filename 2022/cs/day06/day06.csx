using System.IO;

var input = "./input";
var data = File.ReadAllLines(input).First();

var StartOfPacketMarker = FindUniqueSequence(4, data);
var StartOfMessageMarker = FindUniqueSequence(14, data);

Internal.Console.WriteLine(StartOfPacketMarker.ToString());
Internal.Console.WriteLine(StartOfMessageMarker.ToString());

public int FindUniqueSequence(int n, string input)
{
	var sequence = new List<char>();
	for (int i = 0; i < input.Length; i++)
		if (sequence.Count() == n) return i;
		else if (!sequence.Contains(input[i])) sequence.Add(input[i]);
		else
		{
			sequence.RemoveRange(0, sequence.IndexOf(input[i]) + 1);
			sequence.Add(input[i]);
		}
	return -1;
}
