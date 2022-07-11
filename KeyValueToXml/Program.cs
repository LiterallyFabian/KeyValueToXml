using System.Xml.Linq;

namespace KeyValueToXml;

internal class Program
{
    private static void Main(string[] args)
    {
        while (true)
        {
            Console.Write("Path to translation file: ");
            string path = Console.ReadLine();
                
            if(File.Exists(path))
                Convert(path);
            else 
                Console.WriteLine("File not found");
        }
    }

    private static void Convert(string path)
    {
        XDocument xml = new XDocument();
        XElement root = new XElement("resources");
        xml.Add(root);
            

        string[] lines = File.ReadAllLines(path);
            
        // Disclaimer: The following foreach loop is from Galtaker.
        // It might have flaws not encountered in the original project.
        foreach (string line in lines)
        {
            // Skip empty or commented lines
            if (!line.Contains('=') || line.StartsWith('#')) continue;

            string key = line.Substring(0, line.IndexOf("=", StringComparison.Ordinal));
            string value = line.Substring(line.IndexOf("=", StringComparison.Ordinal) + 1);

            value = value.Replace("\\n", Environment.NewLine);
                
            XElement element = new XElement("string", new XAttribute("name", key), value);
            root.Add(element);
        }
            
        Console.Write("Path to output file: ");
            
        string? outputPath;
        while (true)
        {
            outputPath = Console.ReadLine();
            if(string.IsNullOrEmpty(outputPath))
                Console.WriteLine("Path cannot be empty");
            else if (File.Exists(outputPath))
            {
                Console.WriteLine("File already exists, overwrite? (y/n)");
                if (Console.ReadLine() == "y")
                    break;
            }            
            else
                break;
        }
                
        xml.Save(outputPath);
        Console.WriteLine("Ok");
    }
}