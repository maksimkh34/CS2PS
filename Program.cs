using System.Diagnostics;

namespace CS2PluginStarter
{
    internal class Program
    {
        public static void BuildPlugin()
        {
            var p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WorkingDirectory = Path.GetDirectoryName(PathToPluginProject);
            p.StartInfo.Arguments = "dotnet build";
            p.StartInfo.FileName = PathToDotnetExe;
            p.Start();
            p.WaitForExit();
        }

        private const string PathToCompiledDebug = @"D:\Work\C#\Plugins\ExamplePlugin\bin\Debug\net8.0\";
        private const string PathToDotnetExe = @"C:\Program Files\dotnet\dotnet.exe";
        private const string PluginFilename = "ExamplePlugin.dll";
        private const string PathToPluginProject = @"D:\Work\C#\Plugins\ExamplePlugin\ExamplePlugin.csproj";
        private const string PathToCompiledRelease = @"D:\Work\C#\Plugins\ExamplePlugin\bin\Release\net8.0\";
        private const string PathToPluginsFolder = @"C:\CS2Server\server\game\csgo\addons\counterstrikesharp\plugins\ExamplePlugin\";

        private static void Main()
        {
            Console.Write("Building... ");
            BuildPlugin();
            if (File.Exists(PathToCompiledDebug + PluginFilename))
            {
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine($"Error.\n\t{PathToCompiledDebug + PluginFilename} not found.");
                return;
            }

            Console.Write("Checking build... ");
            if (File.Exists(PathToCompiledDebug + PluginFilename))
            {
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine($"Error.\n\t{PathToCompiledDebug + PluginFilename} not found.");
                return;
            }

            Console.Write("Removing old plugin... ");
            Directory.Delete(PathToPluginsFolder, true);
            if (!Directory.Exists(PathToPluginsFolder))
            {
                Directory.CreateDirectory(PathToPluginsFolder);
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine($"Error removing " + PathToPluginsFolder);
                return;
            }

            Console.Write("Copying plugin... ");
            Microsoft.VisualBasic.FileIO.FileSystem.MoveDirectory(
                PathToCompiledDebug, PathToPluginsFolder);
            if (File.Exists(PathToPluginsFolder + PluginFilename))
            {
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine($"Error.\n\t{PathToPluginsFolder + PluginFilename} not found.");
                return;
            }

            Console.WriteLine("Starting server...");

            var process = new Process();
            var processStartInfo = new ProcessStartInfo("cmd.exe", @"/C C:\CS2Server\server_debug.bat")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            process.StartInfo = processStartInfo;
            process.OutputDataReceived += (_, dataReceivedEventArgs) => Console.WriteLine(dataReceivedEventArgs.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
        }
    }
}