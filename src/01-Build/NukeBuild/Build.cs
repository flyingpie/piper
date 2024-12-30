using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Serilog;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[GitHubActions(
	"ci",
	GitHubActionsImage.WindowsLatest,
	FetchDepth = 0,
	OnPushBranches = ["master"],
	OnWorkflowDispatchOptionalInputs = ["name"],
	EnableGitHubToken = true,
	InvokedTargets = [nameof(PublishDebug)]
)]
[SuppressMessage(
	"Major Bug",
	"S3903:Types should be defined in named namespaces",
	Justification = "MvdO: Build script."
)]
public sealed class Build : NukeBuild
{
	public static int Main() => Execute<Build>(x => x.PublishDebug);

	[Parameter(
		"Configuration to build - Default is 'Debug' (local) or 'Release' (server)"
	)]
	private readonly Configuration Configuration = IsLocalBuild
		? Configuration.Debug
		: Configuration.Release;

	[Required]
	[GitRepository]
	private readonly GitRepository GitRepository;

	private AbsolutePath VersionFile => RootDirectory / "VERSION";

	private AbsolutePath OutputDirectory => RootDirectory / "_output";

	private AbsolutePath ArtifactsDirectory => OutputDirectory / "artifacts";

	private AbsolutePath StagingDirectory => OutputDirectory / "staging";

	[Solution(GenerateProjects = true, SuppressBuildProjectCheck = true)]
	private readonly Solution Solution;

	private AbsolutePath PathToLinux64FrameworkDependentZip =>
		ArtifactsDirectory / $"linux-x64_framework-dependent.zip";

	/// <summary>
	/// Returns the version as defined by VERSION (eg. "2.3.4").
	/// </summary>
	private string SemVerVersion => File.ReadAllText(VersionFile).Trim();

	/// <summary>
	/// Returns the version for use in assembly versioning.
	/// </summary>
	private string AssemblyVersion => $"{SemVerVersion}";

	/// <summary>
	/// Returns the version for use in assembly versioning.
	/// </summary>
	private string InformationalVersion
		=> $"{SemVerVersion}.{DateTimeOffset.UtcNow:yyyyMMdd}+{GitRepository.Commit}";

	private Target ReportInfo =>
		_ =>
			_.Executes(() =>
			{
				Log.Information("SemVerVersion:{SemVerVersion}", SemVerVersion);
				Log.Information("AssemblyVersion:{AssemblyVersion}", AssemblyVersion);
				Log.Information("InformationalVersion:{InformationalVersion}", InformationalVersion);
			});

	/// <summary>
	/// Clean output directories.
	/// </summary>
	private Target Clean =>
		_ =>
			_.DependsOn(ReportInfo)
				.Executes(() =>
				{
					OutputDirectory.CreateOrCleanDirectory();
				});

	/// <summary>
	/// Linux x64 framework dependent.
	/// </summary>
	private Target PublishLinux64FrameworkDependent =>
		_ =>
			_.DependsOn(Clean)
				.Produces(PathToLinux64FrameworkDependentZip)
				.Executes(() =>
				{
					var st = StagingDirectory / "linux-x64_framework-dependent";

					DotNetPublish(_ =>
						_.SetAssemblyVersion(AssemblyVersion)
							.SetInformationalVersion(InformationalVersion)
							.SetConfiguration(Configuration)
							.SetProject(Solution._0_Host.Piper_Host_Photino)
							.SetOutput(st)
					);

					st.DeleteUnnecessaryFiles();

					st.MoveWtqUI();

					st.ZipTo(
						PathToLinux64FrameworkDependentZip,
						compressionLevel: CompressionLevel.SmallestSize,
						fileMode: FileMode.CreateNew
					);
				});

	private Target PublishDebug =>
		_ =>
			_.DependsOn(Clean)
				.DependsOn(PublishLinux64FrameworkDependent)
				.Executes();
}
