namespace threads;

public class FileManager
{
    public int _processedFileCount;

    public int ProcessedFileCount => _processedFileCount;

    public string[] GetSortedFiles(string folderPath)
    {
        try
        {
            var files = Directory.GetFiles(folderPath);
            var sortedFiles = files.OrderBy(f => Path.GetFileName(f)).ToArray();
            _processedFileCount = sortedFiles.Length;
            return sortedFiles;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving sorted files: {ex.Message}");
            return Array.Empty<string>();
        }
    }

    public int ReadLastLineValue(string filePath)
    {
        try
        {
            var lines = File.ReadAllLines(filePath);
            return lines.Length > 0 ? Convert.ToInt32(lines[^1]) : 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading the last line of the file {filePath}: {ex.Message}");
            return 0;
        }
    }

    public void DeleteAllFiles(string directoryPath)
    {
        try
        {
            var files = Directory.GetFiles(directoryPath);
            Array.ForEach(files, File.Delete);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting files: {ex.Message}");
        }
    }
}

