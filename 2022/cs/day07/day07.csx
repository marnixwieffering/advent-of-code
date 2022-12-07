using System.IO;

var input = "./input";
var data = File.ReadAllLines(input);

var root = parseInput(data);
var sizes = root.sizes();
var requiredDiskSpace = 30000000 - (70000000 - root.size());

var sum = sizes.Where(x => x <= 100000).Sum();
var size = sizes.Where(x => x >= requiredDiskSpace).OrderBy(x => x).First();

Internal.Console.WriteLine(sum.ToString());
Internal.Console.WriteLine(size.ToString());

#region parsing

public DeviceDirectory parseInput(string[] input)
{
	var root = new DeviceDirectory("/", null);
	var currentDirectory = root;

	foreach (var line in input)
	{
		if (line.StartsWith("$"))
		{
			if (line.Contains("$ cd "))
			{
				var name = line.Replace("$ cd ", string.Empty);
				if (name.Equals("..")) currentDirectory = currentDirectory.parent;
				else if (name.Equals("/")) currentDirectory = root;
				else currentDirectory = currentDirectory.find(name);
			}
		}
		else
		{
			if (line.StartsWith("dir")) currentDirectory.directories.Add(new DeviceDirectory(line.Split(" ").Last(), currentDirectory));
			else currentDirectory.files.Add(new DeviceFile(line.Split(" ").Last(), int.Parse(line.Split(" ").First())));
		}
	}
	return root;
}

public class DeviceDirectory
{
	public string name { get; set; }
	public DeviceDirectory parent;
	public List<DeviceDirectory> directories = new List<DeviceDirectory>();
	public List<DeviceFile> files = new List<DeviceFile>();

	public DeviceDirectory(string name, DeviceDirectory parent)
	{
		this.name = name;
		this.parent = parent;
	}

	public DeviceDirectory find(string target)
	{
		foreach (var directory in directories)
			if (directory.name.Equals(target)) return directory;
		return null;
	}

	public int size()
	{
		int size = 0;
		foreach (var file in this.files)
			size += file.size;
		foreach (var directory in this.directories)
			size += directory.size();
		return size;
	}

	public List<int> sizes()
	{
		var sizes = new List<int>(this.size());
		foreach (var directory in this.directories)
		{
			sizes.Add(directory.size());
			sizes.AddRange(directory.sizes());
		}
		return sizes;
	}
}

public class DeviceFile
{
	public string name { get; set; }
	public int size { get; set; }

	public DeviceFile(string name, int size)
	{
		this.name = name;
		this.size = size;
	}
}

#endregion parsing
