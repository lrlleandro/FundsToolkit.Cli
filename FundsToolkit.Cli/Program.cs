using Cocona;
using System.Diagnostics;
using System.Xml;

CoconaApp.Run((string? protoPath, string? projectName, string? outputFolder) =>
{
    if (string.IsNullOrWhiteSpace(protoPath))
    {
        protoPath = """C:\Users\Windows\Desktop\Protos\Teste.proto""";

    }
    if (string.IsNullOrWhiteSpace(projectName))
    {
        projectName = "Azul";
    }

    if (string.IsNullOrWhiteSpace(outputFolder))
    {
        outputFolder = """C:\Users\Windows\Desktop\Client""";
    }

    var preProcessStartInfo = new ProcessStartInfo()
    {
        FileName = "powershell.exe",
        Arguments = $"""-NoProfile -ExecutionPolicy ByPass -File ".\preBuild.ps1" {projectName} {protoPath}""",
        UseShellExecute = false
    };

    var preProcess = new Process();
    preProcess.StartInfo = preProcessStartInfo;
    preProcess.Start();
    preProcess.WaitForExit();

    var fileInfo = new FileInfo($"{Directory.GetCurrentDirectory()}\\{projectName}\\{projectName}.csproj");
    var projectFile = new XmlDocument();
    projectFile.Load(fileInfo.FullName);
    var node = projectFile.SelectSingleNode("Project/ItemGroup");
    node.InnerXml += $"""
        <Protobuf Include=".\Protos\Teste.proto" GrpcServices="Client">
            <Link>Protos\Teste.proto</Link>
        </Protobuf>
        """;
    projectFile.Save(fileInfo.FullName);

    var processStartInfo = new ProcessStartInfo()
    {
        FileName = "powershell.exe",
        Arguments = $"""-NoProfile -ExecutionPolicy ByPass -File ".\build.ps1" {projectName} {outputFolder} """,
        UseShellExecute = false
    };

    var process = new Process();
    process.StartInfo = processStartInfo;
    process.Start();
    process.WaitForExit();

    Console.ReadLine();
});