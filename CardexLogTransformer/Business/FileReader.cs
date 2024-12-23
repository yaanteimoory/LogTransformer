using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardexLogTransformer.Business;

public class FileReader(string path)
{

    public readonly string Path = path;

    public IEnumerable<string> Read()
    {
        using StreamReader reader = new(Path);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }

    }

}

