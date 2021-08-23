//###########################################    EXTERNAL DEPENDENCIES    ###########################################
#region External Dependencies
#tool "nuget:?package=NUnit.ConsoleRunner&version=3.10.0"
#tool "nuget:?package=Doxygen&version=1.8.14"
#endregion

//###########################################    ARGUMENTS    #######################################################
#region Arguments
// pattern: Argument("argumentName", "defaultValue")
// The target task Cake will execute
string _target = Argument("Target", "BASIC");

// Prints the arguments cake was called with
void PrintArguments() {
    Information("");
    Information("# Cake configuration:");
    Information($"Target:                   {_target}");
    Information("");
}
#endregion

//###########################################    FOLDERS    #########################################################
#region Folders

DirectoryPath root = MakeAbsolute(new DirectoryPath("./../"));
DirectoryPath sourceProjects = root.Combine(new DirectoryPath("03_Implementierung"));
DirectoryPath testProjects = root.Combine(new DirectoryPath("04_Test"));
DirectoryPath devOps = root.Combine(new DirectoryPath("06_DevOps"));

DirectoryPath artifacts = devOps.Combine(new DirectoryPath("artifacts"));
DirectoryPath buildDir = artifacts.Combine(new DirectoryPath("build"));
DirectoryPath testResults = artifacts.Combine(new DirectoryPath("test-results"));
DirectoryPath codeDoc = artifacts.Combine(new DirectoryPath("code-documentation"));

// Anonymous class of 'DirectoryPath'-objects for easy intellisense
var _folders = new
{
    root,
    sourceProjects,
    testProjects,
    devOps,

    buildDir,
    artifacts,
    testResults,
    codeDoc,
};
#endregion

//###########################################    FILES    ###########################################################
#region Files
// Anonymous class of 'FilePath'-objects for easy intellisense
var _files = new
{
    nunit3_junit_xslt = _folders.devOps.CombineWithFilePath("nunit3-junit.xslt"),
    solutionPath = root.CombineWithFilePath(new FilePath("QuantumCryptoCram.sln"))
};
#endregion

//###########################################    GLOBAL VARS    #####################################################
#region Global Variables
// Flag if the current build is called in context of the ci pipeline.
bool _isCIBuild;
// Flag if the current build is called manually via the local command line.
bool _isLocalBuild;

bool _isFullModeActive = true ; // _target.ToLower() == _myTasks.FULL.ToLower();
#endregion

//###########################################    SETUP    ###########################################################
#region Setup tasks
// Setup function runs once before all tasks.
// In general the Setup-/Teardown and Tasks-functions can expose the cake "context" that holds cakes internal objects and extra metadata like the current environment variables.
Setup(context =>
{
    // If this build is a CI build get the environment variables
    if(GitLabCI.IsRunningOnGitLabCI){
        _isCIBuild = true;
        Information("# This is a CI build on the build server.");;
    } else {
        _isLocalBuild = true;
        Information("# This is a build from the local command line.");;

    }
    Information("");

    Information("# Environment:");
    Information($"Date/Time:                {DateTime.Now}");

    PrintArguments();
});
#endregion

//###########################################    TASKS NAMES    #####################################################
#region Task Names
var _myTasks = new
{
    // Child Tasks
    Clean = "Clean",
    BuildApp = "BuildApp",
    BuildTests = "BuildTests",
    RunTests = "RunTests",
    PackApp = "PackApp",
    GenerateDocumentation = "GenerateDocumentation",
    // Parent Tasks
    BASIC = "BASIC",
    FULL = "FULL",
};
#endregion

//###########################################    TASK DEPENDENCY DEFINTIONS    ######################################
#region Task Dependency Definitions
Task(_myTasks.BASIC)
.Description($"Executes: {_myTasks.Clean} --> {_myTasks.BuildApp} --> {_myTasks.RunTests}")
.IsDependentOn(_myTasks.Clean)
.IsDependentOn(_myTasks.BuildApp)
.IsDependentOn(_myTasks.BuildTests)
.IsDependentOn(_myTasks.RunTests)

.Does(() =>{
    Information($"Parent Task {_myTasks.BASIC} completed successfully!");
});

Task(_myTasks.FULL)
.Description($"Executes: {_myTasks.BASIC} --> {_myTasks.GenerateDocumentation}")
.IsDependentOn(_myTasks.BASIC)
.IsDependentOn(_myTasks.PackApp)
.IsDependentOn(_myTasks.GenerateDocumentation)
.Does(() =>{
    Information($"Parent Task {_myTasks.FULL} completed successfully!");
});
#endregion

//###########################################    TASKS DEFINITIONS   ################################################
#region Task Definitions
#region Clean
Task(_myTasks.Clean)
.Description("Deletes the repositories local and intermediate output files and the artifacts directory.")
.Does((context) =>
    {
        // Delete local and intermediate output and visual studio directories.
        var directoryNamesToDelete = new string[] {"lib", "bin", "obj", ".vs"};
        // Weired globbing syntax is combination of:
        // - Cakes globbing syntax of multiple directory names, i.e.: "./**/^{bin,lib} matches "./a/b/c/bin" and ./a/b/c/lib"
        // - C#´s escaping of '{' and '}' braces in an interpolated string as '{{' and '}}'
        var globbingPattern = $"{_folders.sourceProjects}/**/^{{{string.Join(separator: ",", directoryNamesToDelete)}}}";
        Information($"Scan repository \"{_folders.sourceProjects}\" for directories (\"/{string.Join(separator: "\", \"/", directoryNamesToDelete)}\") to delete...");
        DirectoryPathCollection directoriesToDelete = GetDirectories(globbingPattern);
        var deletedDirs = 0;
        Information($"Delete {directoriesToDelete.Count} directories...");
        foreach (DirectoryPath dir in directoriesToDelete)
        {
            try {
                DeleteDirectory(dir, new DeleteDirectorySettings{
                    Recursive = true,
                    Force = false,
                });
                deletedDirs ++;
            } catch (Exception ex) {

                Warning($"Could not delete directory \"{dir}\"! {ex.Message}");
            }
        }
        Information($"Deleted {deletedDirs} of {directoriesToDelete.Count} directories.");
        Information("");

        // Delete unnecessary files
        var filePatternToDelete = "*.(suo|csproj.user|ncb|TLB|VBR|exp|lib)";
        Information($"Scan repository for files \"{filePatternToDelete}\" to delete...");
        FilePathCollection filesToDelete = GetFiles($"{_folders.sourceProjects}/**/{filePatternToDelete}");
        var deletedFiles = 0;
        Information($"Delete {filesToDelete.Count} files...");
        foreach (FilePath file in filesToDelete)
        {
            try {
                DeleteFile(file);
                deletedFiles++;
            } catch (Exception ex) {
                Information("");
                Warning($"Could not delete file \"{file}\"! {ex.Message}");
            }
        }
        Information($"Deleted {deletedFiles} of {filesToDelete.Count} repository files.");
        Information("");


        // Make sure artifacts dir exists and all artifacts from previous builds are deleted
        EnsureDirectoryExists(_folders.artifacts);
        Information($"Clean artifacts directory: \"{_folders.artifacts}\"");
        CleanDirectory(_folders.artifacts);

        if (_isFullModeActive)
        {
            CleanDirectory(_folders.buildDir);
        }

    }
)
// "OnError" works as global exception handler ("catch-block") for a task
.OnError(exception =>
    {
        // "Error"-Alias displays a non terminating error.
        // An exception in this task should just be printed and not terminate the script
        Error(exception.ToString());
    }
);

#endregion


#region Versioning Template
/*
Task("Versioning")
.Does(() =>
{
    var projects = GetFiles(_folders.src + "/.../*.csproj");
    var version = "0.0.1";
    var buildNo = "123";
    var semVersion = string.Concat(version + "-" + buildNo);

    var assemblyInfo = new AssemblyInfoSettings {
            Version = version,
            FileVersion = version,
            InformationalVersion = semVersion,

            Title =
            Description =
            Configuration = configuration,
            Company =
            Product =
            Copyright =
            Trademark =
        };

    foreach (var project in projects)
    {
        var assemblyInfoPath = project.GetDirectory() + "/Properties/AssemblyInfo.cs";
        Information($"Generating AssemblyInfo.cs for project {project}");

        CreateAssemblyInfo(assemblyInfoPath, assemblyInfo);
    }
});
*/
#endregion

#region Build
Task(_myTasks.BuildApp)
.Description("Builds the complete application.")
.Does(() =>
    {
        var failedSolutions = new List<string>();
        var msBuildSettings = new MSBuildSettings()
            .UseToolVersion(MSBuildToolVersion.VS2019)
            // Compile configuration ("Debug" or "Release")
            .SetConfiguration(_isFullModeActive ? "Publish" : "Release")
            // MSBuild Target ("Build" or "Rebuild")
            .WithTarget("Rebuild")
            // All necessary Nuget-packages get downloaded before compiling
            .WithRestore()
            // Number of cores allocated for a parallel build.
            // A value of "0" means as many cpu-cores as possible are used.
            .SetMaxCpuCount(0)
            // Defines if sub-processes of MSBuild (one per core) should stay alive
            // after parallel building, for a faster next build.
            .SetNodeReuse(false)
            // Compact log for each solution with only warnings, errors, and a build summary
            .WithConsoleLoggerParameter("WarningsOnly;ErrorsOnly;Summary")
            .SetVerbosity(Verbosity.Normal)
            .SetNoLogo(true);

            // Publish app to output dir if in FULL mode
            if (_isFullModeActive)
            {
                msBuildSettings.WithProperty("OutDir", $"{_folders.buildDir}");
            }

            // Build solution with provided msbuild settings
            Information("");
            Information($"BUILDING SOLUTION: \"{_files.solutionPath}\"");

            MSBuild(_files.solutionPath, msBuildSettings);

            Information("");
    }
);
#endregion

#region BuildTests
Task(_myTasks.BuildTests)
.Description("Builds all unit test programs (*_UnitTest.csproj) of the unit test directory.")
// Using DoesForEach() in combination with DeferOnError() so all tests are getting executed first,
// errors are collected and at the end of the task all errors are thrown.
.DoesForEach(context => GetFiles($"{_folders.testProjects}/**/*.Tests.csproj"), (testProject) =>
    {
        var msBuildSettings = new MSBuildSettings()
            .UseToolVersion(MSBuildToolVersion.VS2019)
            // Compile configuration ("Debug" or "Release")
            .SetConfiguration("Release")
            // MSBuild Target ("Build" or "Rebuild")
            .WithTarget("Build")
            // All necessary Nuget-packages get downloaded before compiling
            .WithRestore()
            // Number of cores allocated for a parallel build.
            // A value of "0" means as many cpu-cores as possible are used.
            .SetMaxCpuCount(0)
            // Defines if sub-processes of MSBuild (one per core) should stay alive
            // after parallel building, for a faster next build.
            .SetNodeReuse(false)
            // Compact log for each solution with only warnings, errors, and a build summary
            .WithConsoleLoggerParameter("WarningsOnly;ErrorsOnly;Summary")
            .SetVerbosity(Verbosity.Normal)
            .SetNoLogo(true);

            // Build solution with provided msbuild settings
            Information("");
            Information($"BUILDING TEST PROJECT: \"{testProject}\"");

            MSBuild(testProject, msBuildSettings);

            Information("");
        Information("\n--------------------------------------\n");

    }
)
.DeferOnError();
#endregion

#region Test
Task(_myTasks.RunTests)
.Description("Runs all compiled unit test programs (*_UnitTest.dll) of the unit test directory and outputs result files to artifacts directory.")
.Does(() =>
    {
        // Clean output dir
        EnsureDirectoryExists(_folders.testResults);
        CleanDirectory(_folders.testResults);
    }
)
// Using DoesForEach() in combination with DeferOnError() so all tests are getting executed first,
// errors are collected and at the end of the task all errors are thrown.
.DoesForEach(context => GetFiles($"{_folders.testProjects}/**/bin/Release/net47/*.Tests.dll"), (testAssembly) =>
    {
        string resultsFileName = $"{testAssembly.GetFilenameWithoutExtension()}_Results.xml";

        Information("");
        Information($"TESTING PROJECT: \"{testAssembly}\"");
        NUnit3(
            testAssembly.FullPath,
            new NUnit3Settings {
                NoHeader = true,
                Results = new[] {
                    new NUnit3Result {
                        FileName = _folders.testResults.CombineWithFilePath(resultsFileName).FullPath,
                        Transform = _files.nunit3_junit_xslt
                    }
                },
            }
        );
        Information("\n--------------------------------------\n");

    }
)
.DeferOnError();
#endregion

#region PackApp
Task(_myTasks.PackApp)
.Description("")
.Does(() =>
    {

        Information($"Packing Application...");
        Zip(_folders.buildDir, _folders.artifacts.CombineWithFilePath(new FilePath("QuantumCryptoCram_Application.zip")));
    }
);
#endregion

#region GenerateDocumentation
Task(_myTasks.GenerateDocumentation)
.Description("Generates a html code documentation from C# code xml-annotations with Doxygen.")
.Does(() =>
    {
        EnsureDirectoryExists(_folders.codeDoc); // This output dir is defined in doxygen.config
        FilePath doxgyenPath = Context.Tools.Resolve("doxygen.exe");
        Information($"Generating Doxygen code documentation...");
        int exitCode = StartProcess(
            doxgyenPath,
            new ProcessSettings {
                Arguments = "doxygen.config",
                WorkingDirectory = _folders.devOps,
            }
        );
        if (exitCode != 0) {
            Warning($"Could not generate code documentation!");
        } else {
            Information($"SUCCESS: See {_folders.codeDoc}");
        }
    }
);
#endregion

#endregion

//###########################################    RUN TARGET    ######################################################
// After the definition of tasks run the target provided as script argument (or the default one, if not provided).
RunTarget(_target);

//###########################################    HELPER FUNCTIONS    ################################################