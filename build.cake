#load "nuget:?package=Cake.Mix&version=0.13.0"

var target = Argument("Target", "Default");
var configuration = Argument("Configuration", "Release");

Information("Running target " + target + " in configuration " + configuration);

var packageJson = new PackageJson(Context, "./package.json");
var buildNumber = packageJson.GetVersion();

Information($"Building version {buildNumber}");

var artifactsDirectory = Directory("./artifacts");

var projects = new List<string>
{
  "./Source/IdentityServer4TestServer/IdentityServer4TestServer.csproj"
};

Task("Clean")
    .Does(() =>
    {
        CleanDirectory(artifactsDirectory);
    });

Task("SetNuSpecVersion")
    .IsDependentOn("Clean")
    .Does(() => 
    {
        var nuSpecFile = "./Source/IdentityServer4TestServer/IdentityServer4TestServer.nuspec";
        TransformTextFile(nuSpecFile)
            .WithToken("version", buildNumber.ToString())
            .Save(nuSpecFile);
    });

Task("Test")
    .IsDependentOn("SetNuSpecVersion")
    .Does(() =>
    {
        var projects = GetFiles("./**/*Tests.csproj");
        foreach(var project in projects)
        {
            Information("Testing project " + project);
            DotNetCoreTest(
                project.ToString(),
                new DotNetCoreTestSettings()
                {
                    Configuration = configuration,                    
                });
        }
    });

Task("Pack")
    .IsDependentOn("Test")
    .Does(() =>
    {
        var version = buildNumber.ToString();
        foreach (var project in projects)
        {
            Information("Packing dotnetcore project " + project);
            DotNetCorePack(
                project.ToString(),
                new DotNetCorePackSettings()
                {
                    Configuration = configuration,
                    OutputDirectory = artifactsDirectory,
                    VersionSuffix = version,
                    ArgumentCustomization  = builder => builder.Append("/p:PackageVersion=" + version)
                });
        }
    });

Task("Default")
    .IsDependentOn("Pack");

RunTarget(target);
