using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AOUIEditor
{
    public class Prototype
    {
        public static Dictionary<string, PrototypeItem> Dictionary { get; private set; }

        public string Name { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public string MainFile { get; set; }
        public List<string> Files { get; set; }
        public string Format { get; set; }

        public static void Init()
        {
            string s = File.ReadAllText(@"Widgets\Prototypes.json");
            Dictionary = JsonSerializer.Deserialize<Dictionary<string, PrototypeItem>>(s);
        }
    }

    public class PrototypeItem
    {
        public List<Prototype> Prototypes { get; set; }

        public PrototypeItem()
        {
            Prototypes = new List<Prototype>();
        }
    }

    // PowerShell команда для вывода всех файлов
    // Get-ChildItem -Recurse | Where {! $_.PSIsContainer } | Select FullName
}
