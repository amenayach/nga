using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nga
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = Config.Get();
            var appname = config.AppName;

            while (string.IsNullOrWhiteSpace(appname))
            {
                PrintWarning("Please enter the angular app name (ng-app):");
                appname = Console.ReadLine();
                config.AppName = appname;
                Config.Save(config);
            }

            var cmd = args.GetArgument(0);

            switch (cmd)
            {
                case "g":
                case "generate":
                    ExecGenerator(args);
                    break;
                default:
                    break;
            }
        }

        private static void ExecGenerator(string[] args)
        {
            if (args?.Any() ?? false && args.Length > 2)
            {
                var cmd = args.GetArgument(1);
                switch (cmd)
                {
                    case "c":
                        var componentName = args.GetArgument(2);
                        CreateComponent(componentName);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                PrintError("Invalid syntax");
            }
        }

        private static void CreateComponent(string componentName)
        {
            var dashedComponent = componentName.ToDash();
            var currentFolder = Directory.GetCurrentDirectory();
            var componentFolder = Path.Combine(Path.Combine(currentFolder, "components"), dashedComponent);
            var indexPath = Path.Combine(currentFolder, "index.html");
            var componentFilePrefix = Path.Combine(componentFolder, dashedComponent) + ".cmp";
            var scriptHtmlPath = $"./components/{dashedComponent}/{dashedComponent}.cmp";

            if (!Directory.Exists(componentFolder))
            {
                Directory.CreateDirectory(componentFolder);
            }

            if (!File.Exists(componentFilePrefix + ".html"))
            {
                File.WriteAllText(componentFilePrefix + ".html", $"<p>Hello it's {componentName}</p>");
                Print(componentFilePrefix + ".html");
            }

            if (!File.Exists(componentFilePrefix + ".css"))
            {
                File.WriteAllText(componentFilePrefix + ".css", string.Empty);
                Print(componentFilePrefix + ".css");
            }

            if (!File.Exists(componentFilePrefix + ".js"))
            {
                File.WriteAllText(componentFilePrefix + ".js",
                    $@"angular.module('FncApp').component('{componentName}', {{
    templateUrl: './components/{dashedComponent}/{dashedComponent}.cmp.html',
	controller: {componentName}Controller
}});

function {componentName}Controller($scope) {{

}}");
                Print(componentFilePrefix + ".js");
            }

            if (File.Exists(indexPath))
            {
                var indexScript = File.ReadAllText(indexPath);
                indexScript = indexScript.Insert(indexScript.IndexOf("</head>"), $@"  <link rel=""stylesheet"" href=""{scriptHtmlPath}.css"">{Environment.NewLine}");
                indexScript = indexScript.Insert(indexScript.IndexOf("</body>"), $@"  <script src=""{scriptHtmlPath}.js""></script>{Environment.NewLine}");
                File.WriteAllText(indexPath, indexScript);
                PrintWarning("Updating index.html");
            }
            else
            {
                PrintWarning("index.html not found!");
            }
        }

        private static void PrintError(string text)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }

        private static void Print(string text)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }

        private static void PrintWarning(string text)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }
    }
}
