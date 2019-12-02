using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp110_worksheet_7
{
	public static class DirectoryUtils
	{
		// Return the size, in bytes, of the given file
		public static long GetFileSize(string filePath)
		{
			return new FileInfo(filePath).Length;
		}

		// Return true if the given path points to a directory, false if it points to a file
		public static bool IsDirectory(string path)
		{
			return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
		}

		// Return the total size, in bytes, of all the files below the given directory
		public static long GetTotalSize(string directory)
		{
            long directorySize = 0;
            Stack<string> directoriesLeft = new Stack<string>();

            if (IsDirectory(directory))
            {
                directoriesLeft.Push(directory);

                while (directoriesLeft.Count > 0)
                {
                    string currentDirectory = directoriesLeft.Pop();
                    string[] newDirectories = Directory.GetDirectories(currentDirectory);
                    string[] newfiles = Directory.GetFiles(currentDirectory);

                    for (int i = 0; i < newDirectories.Length; i++)
                    {
                        directoriesLeft.Push(newDirectories[i]);
                    }

                    for (int i = 0; i < newfiles.Length; i++)
                    {
                        directorySize += GetFileSize(newfiles[i]);
                    }
                }
            }
            return directorySize;
		}

		// Return the number of files (not counting directories) below the given directory
		public static int CountFiles(string directory)
		{
            int directoryCount = 0;
            Stack<string> directoriesLeft = new Stack<string>();

            if (IsDirectory(directory))
            {
                directoriesLeft.Push(directory);

                while (directoriesLeft.Count > 0)
                {
                    string currentDirectory = directoriesLeft.Pop();
                    string[] newDirectories = Directory.GetDirectories(currentDirectory);
                    string[] newfiles = Directory.GetFiles(currentDirectory);

                    for (int i = 0; i < newDirectories.Length; i++)
                    {
                        directoriesLeft.Push(newDirectories[i]);
                    }

                    for (int i = 0; i < newfiles.Length; i++)
                    {
                        directoryCount++;
                    }
                }
            }
            return directoryCount;
        }

		// Return the nesting depth of the given directory. A directory containing only files (no currentDirectories) has a depth of 0.
		public static int GetDepth(string directory)
		{
            int depth = 0;
            Stack<string> directoriesLeft = new Stack<string>();
            Stack<string> currentDirectories = new Stack<string>();

            if (IsDirectory(directory))
            {
                while (currentDirectories.Count > 0)
                {
                    foreach (string files in currentDirectories)
                    {
                        string[] newDirectories = Directory.GetDirectories(currentDirectories.Pop());

                        foreach (string directoryPath in newDirectories)
                        {
                            directoriesLeft.Push(directoryPath);
                        }
                    }

                    if (currentDirectories.Count > 0)
                    {
                        currentDirectories.Clear();

                        for (int i = 0; i < currentDirectories.Count; i++)
                        {
                            currentDirectories.Push(currentDirectories.Pop());
                        }

                        currentDirectories.Clear();
                        depth++;
                    }
                }
            }
            return depth;
 		}

		// Get the path and size (in bytes) of the smallest file below the given directory
		public static Tuple<string, long> GetSmallestFile(string directory)
		{
            Tuple<String, long> smallestFile = new Tuple<string, long>("Directory not found", 1000000000000);
            Stack<string> subDirectories = new Stack<string>();

            if (IsDirectory(directory))
            {
                subDirectories.Push(directory);

                while (subDirectories.Count > 0)
                {
                    string currentDirectory = subDirectories.Pop();
                    string[] newDirectories = Directory.GetDirectories(currentDirectory);
                    string[] newFiles = Directory.GetFiles(currentDirectory);

                    foreach (string directories in newDirectories)
                    {
                        subDirectories.Push(directories);
                    }

                    foreach (string files in newFiles)
                    {
                        long fileSize = GetFileSize(files);

                        if (fileSize < smallestFile.Item2)
                        {
                            smallestFile = new Tuple<string, long>(files, fileSize);
                        }
                    }
                }
            }
            return smallestFile;
        }

		// Get the path and size (in bytes) of the largest file below the given directory
		public static Tuple<string, long> GetLargestFile(string directory)
		{
            Tuple<String, long> largestFile = new Tuple<string, long>("Directory not found", 0);
            Stack<string> subDirectories = new Stack<string>();

            if (IsDirectory(directory))
            {
                subDirectories.Push(directory);

                while (subDirectories.Count > 0)
                {
                    string currentDirectory = subDirectories.Pop();
                    string[] newDirectories = Directory.GetDirectories(currentDirectory);
                    string[] newFiles = Directory.GetFiles(currentDirectory);

                    foreach (string directories in newDirectories)
                    {
                        subDirectories.Push(directories);
                    }

                    foreach (string files in newFiles)
                    {
                        long fileSize = GetFileSize(files);

                        if (fileSize > largestFile.Item2)
                        {
                            largestFile = new Tuple<string, long>(files, fileSize);
                        }
                    }
                }
            }
            return largestFile;
        }

		// Get all files whose size is equal to the given value (in bytes) below the given directory
		public static IEnumerable<string> GetFilesOfSize(string directory, long size)
		{
            List<string> filesOfSize = new List<string>();
            Stack<string> subDirectories = new Stack<string>();

            if (IsDirectory(directory))
            {
                subDirectories.Push(directory);

                while (subDirectories.Count > 0)
                {
                    string currentDirectory = subDirectories.Pop();
                    string[] newDirectories = Directory.GetDirectories(currentDirectory);
                    string[] newFiles = Directory.GetFiles(currentDirectory);

                    foreach (string directories in newDirectories)
                    {
                        subDirectories.Push(directories);
                    }

                    foreach (string files in newFiles)
                    {
                        long fileSize = GetFileSize(files);

                        if (fileSize == size)
                        {
                            filesOfSize.Add(files);
                        }
                    }
                }
            }
            return filesOfSize;
        }
	}
}
