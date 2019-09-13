#tool nuget:?package=ReportGenerator&version=4.2.15
#tool nuget:?package=coverlet.console&version=1.5.3
// https://github.com/cake-build/cake/issues/2077
#tool nuget:?package=Microsoft.TestPlatform&version=16.2.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("Configuration", "Release");
var outputDirectory = Argument<DirectoryPath>("OutputDirectory", "output");
var codeCoverageDirectory = Argument<DirectoryPath>("CodeCoverageDirectory", "output/coverage");
var solutionFile = Argument("SolutionFile", "assemblyrunner.sln");
var versionSuffix = Argument("VersionSuffix", "");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

// Target : Clean
// 
// Description
// - Cleans binary directories.
// - Cleans output directory.
// - Cleans the test coverage directory.
Task("Clean")
    .Does(() =>
{
    CleanDirectory(codeCoverageDirectory);
    CleanDirectory(outputDirectory);

    // remove all binaries in source files
    var srcBinDirectories = GetDirectories("./src/**/bin");
    foreach(var directory in srcBinDirectories)
    {
        CleanDirectory(directory);
    }

    // remove all binaries in test files
    var testsBinDirectories = GetDirectories("./tests/**/bin");
    foreach(var directory in testsBinDirectories)
    {
        CleanDirectory(directory);
    }
});

// Target : Restore-NuGet-Packages
// 
// Description
// - Restores all needed NuGet packages for the projects.
Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    // https://docs.microsoft.com/en-us/nuget/consume-packages/package-restore
    //
    // Reload all nuget packages used by the solution
    NuGetRestore(solutionFile);
});

// Target : Build
// 
// Description
// - Builds the artifacts.
Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
    
      // Use MSBuild
      MSBuild(solutionFile, settings => {
        settings.ArgumentCustomization = 
            args => args
                .Append("/p:IncludeSymbols=true")
                .Append("/p:IncludeSource=true")
                .Append($"/p:VersionSuffix={versionSuffix}");
        settings.SetConfiguration(configuration);
      });
    
    } else {
    
      // Use XBuild
      XBuild(solutionFile, settings => {
        settings.ArgumentCustomization = 
            args => args
                .Append("/p:IncludeSymbols=true")
                .Append("/p:IncludeSource=true")
                .Append($"/p:VersionSuffix={versionSuffix}");
        settings.SetConfiguration(configuration);
      });

    }
});

// Target : Test-With-Coverage
// 
// Description
// - Executes the test and generates with code coverage files.
Task("Test-With-CodeCoverage")
    .IsDependentOn("Build")
    .Does(() =>
{
    var includeFilter = "[Compori.*]*"; 
    var excludeFilter = "[xunit.*]*"; 

    FilePath coverletPath = Context.Tools.Resolve("coverlet.console.dll");
    Information("coverlet.console.dll: " + (coverletPath ?? "N/A"));
    FilePath vstestPath = Context.Tools.Resolve("vstest.console.exe");    
    Information("vstest.console.exe: " + (vstestPath ?? "N/A"));

    var testAssemblies = GetFiles($"./tests/**/bin/{configuration}/net35/*Tests.dll");
    foreach(var testAssembly in testAssemblies)
    {
        var assemblyDirectory = testAssembly.GetDirectory();
        var testAssemblyPath = testAssembly.FullPath;
        var targetFramework = assemblyDirectory.Segments[assemblyDirectory.Segments.Length - 1];
        
        var logFileName = testAssembly.GetFilenameWithoutExtension() + "." + targetFramework + ".trx";
        var logFilePath = MakeAbsolute(codeCoverageDirectory).CombineWithFilePath(logFileName);

        var coverageFile = testAssembly.GetFilenameWithoutExtension() + "." + targetFramework + ".cobertura.xml";
        var coveragePath = MakeAbsolute(codeCoverageDirectory).CombineWithFilePath(coverageFile);

        // VSTest test
        DotNetCoreExecute(
            coverletPath,
            new ProcessArgumentBuilder()
                    .Append(testAssemblyPath)
                    .Append($"--target \"{vstestPath}\"")
                    .Append($"--targetargs \"{testAssemblyPath} /Framework:Framework35 /logger:trx;LogFileName=\\\"{logFilePath}\\\"\"")
                    .Append("--format cobertura")
                    .Append("--output \"" + coveragePath.FullPath + "\"")
                    .Append("--include \"" + includeFilter + "\"")
                    .Append("--exclude \"" + excludeFilter + "\""),
            new DotNetCoreExecuteSettings {               
            }
        );       
    }

    var targetFrameworks = new string[] {"net461", "netcoreapp2.1"};
    var projectFiles = GetFiles("./tests/**/*Tests.csproj");
    foreach(var projectFile in projectFiles)
    {
        foreach(var targetFramework in targetFrameworks)
        {
            var coverageFile = projectFile.GetFilenameWithoutExtension() + "." + targetFramework + ".cobertura.xml";
            var coveragePath = MakeAbsolute(codeCoverageDirectory).CombineWithFilePath(coverageFile);
            var logFileName = projectFile.GetFilenameWithoutExtension() + "." + targetFramework + ".trx";
            var logFilePath = MakeAbsolute(codeCoverageDirectory).CombineWithFilePath(logFileName);

            // coverlet test via dotnet test
            var settings = new DotNetCoreTestSettings
            {
                Configuration = configuration,
                Framework = targetFramework,
                ArgumentCustomization = args => args
                    .Append("/p:CollectCoverage=true")
                    .Append("/p:CoverletOutputFormat=cobertura")
                    .Append($"/p:Include={includeFilter}")                    
                    .Append($"/p:Exclude={excludeFilter}")                    
                    .Append($"/p:CoverletOutput=\"{coveragePath.FullPath}\"")
                    .Append($"--logger trx;LogFileName=\"{logFilePath}\"")
            };
            DotNetCoreTest(projectFile.FullPath, settings);
        }
    }           
});

// Target : Test-With-CoverageReport
// 
// Description
// - Executes the test and generates with code coverage files.
// - Generates a code coverage html report with badges.
Task("Test-With-CodeCoverageReport")
    .IsDependentOn("Test-With-CodeCoverage")
    .Does(() =>
{
    ReportGenerator( 
        MakeAbsolute(codeCoverageDirectory).FullPath + "/*.cobertura.xml", 
        MakeAbsolute(codeCoverageDirectory).FullPath + "/report",
        new ReportGeneratorSettings(){
            ReportTypes = new[] { 
                ReportGeneratorReportType.HtmlInline,
                ReportGeneratorReportType.Badges 
            }
        }
    );
});

// Target : Build
// 
// Description
// - Setup the default task.
Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
