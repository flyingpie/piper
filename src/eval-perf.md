Id|ParentId|Pass|File|Line #|Expression|Inc (ms)|Inc (%)|Exc (ms)|Exc (%)|#|Kind|Bug
---|---|---|---|---:|---|---:|---:|---:|---:|---:|---:|---
1341765254024||Total evaluation||||3014|100%|13|0.4%|10|Element|
1361957104524||Total evaluation for globbing||||22|0.7%|22|0.7%|6|Glob|
1341766892172|1341765254024|Initial properties (pass 0)||||28|0.9%|28|0.9%|10|Element|
1341768530321|1341765254024|Properties (pass 1)||||1227|40.7%|1|0%|10|Element|
1341999519341|1341765254024|Item definition groups (pass 2)||||14|0.5%|1|0%|10|Element|
1342001157632|1341765254024|Items (pass 3)||||471|15.6%|58|1.9%|10|Element|
1342010987399|1341765254024|Lazy items (pass 3.1)||||754|25%|201|6.7%|10|Element|
1342015902296|1341765254024|Using tasks (pass 4)||||21|0.7%|21|0.7%|10|Element|
1342017540597|1341765254024|Targets (pass 5)||||486|16.1%|239|7.9%|10|Element|
1343121982946|1342871231399|Properties (pass 1)|Microsoft.NET.Sdk.ImportWorkloads.props|14|`<Import Project="AutoImport.props" Sdk="Microsoft.NET.SDK.WorkloadAutoImportPropsLocator"  />`|62|2.1%|62|2.1%|9|Element|
1343580935286|1343576017524|Properties (pass 1)|Microsoft.NET.Sdk.ImportWorkloads.targets|16|`<Import Project="WorkloadManifest.targets" Sdk="Microsoft.NET.SDK.WorkloadManifestTargetsLocator"  /...`|88|2.9%|52|1.7%|9|Element|
1349375380437|1342010987399|Lazy items (pass 3.1)|Microsoft.Managed.Core.targets|139|`<_AllDirectoriesAbove Include="@(Compile-&gt;GetPathsOfAllDirectoriesAbove())" Condition="'$(Discove...`|51|1.7%|51|1.7%|9|Element|
1344453160724|1344448241366|Properties (pass 1)|Microsoft.Common.targets|22|`<Import Project="$(CommonTargetsPath)" />`|166|5.5%|49|1.6%|9|Element|
1343385870962|1343377675292|Properties (pass 1)|Microsoft.NET.TargetFrameworkInference.targets|54|`<TargetFrameworkIdentifier >$([MSBuild]::GetTargetFrameworkIdentifier('$(TargetFramework)'))</Target...`|40|1.3%|40|1.3%|9|Element|
1347033783656|1346945151647|Properties (pass 1)|Microsoft.NET.Sdk.StaticWebAssets.SingleTargeting.targets|17|`<Import Project="Microsoft.NET.Sdk.StaticWebAssets.targets" Condition="'$(UseStaticWebAssetsV2)' == ...`|65|2.1%|36|1.2%|5|Element|
1349533092549|1342017540597|Targets (pass 5)|Microsoft.NET.TargetFrameworkInference.targets|109|`<NETSdkError Condition="!$(TargetFramework.Contains(';'))" ResourceName="CannotInferTargetFrameworkI...`|32|1.1%|32|1.1%|5|Element|
1349125688441|1342010987399|Lazy items (pass 3.1)|WorkloadManifest.targets|95|`<KnownWebAssemblySdkPack Update="@(KnownWebAssemblySdkPack)"><WebAssemblySdkPackVersion>$(_RuntimePa...`|31|1%|31|1%|9|Element|
1344272790149|1343266216662|Properties (pass 1)|Microsoft.CSharp.targets|34|`<Import Project="$(CSharpTargetsPath)" />`|299|9.9%|29|0.9%|9|Element|
1347414606174|1343266216662|Properties (pass 1)|Sdk.targets|66|`<Import Project="$(_ContainersTargetsDir)Microsoft.NET.Build.Containers.targets" Condition="$(_IsNot...`|30|1%|28|0.9%|9|Element|
1347972805424|1342001157632|Items (pass 3)|Microsoft.NET.WindowsSdkSupportedTargetPlatforms.props|17|`Condition="'@(WindowsSdkSupportedTargetPlatformVersion)' == ''")`|53|1.8%|27|0.9%|9|Condition|
1344454800512|1344453160724|Properties (pass 1)|Microsoft.Common.CurrentVersion.targets|||117|3.9%|26|0.9%|9|Element|
1346945151647|1341819313466|Properties (pass 1)|Sdk.targets|20|`<Import Project="$(StaticWebAssetsSdkCurrentVersionTargets)" />`|92|3%|23|0.8%|5|Element|
1347787273436|1347637872002|Items (pass 3)|Microsoft.NETCoreSdk.BundledVersions.props|812|`<WindowsSdkSupportedTargetPlatformVersion Include="10.0.26100.1" WindowsSdkPackageVersion="10.0.2610...`|23|0.7%|23|0.7%|19|Element|
1351176475079|1342010987399|Lazy items (pass 3.1)|Microsoft.AspNetCore.StaticWebAssetEndpoints.props|141|`<StaticWebAssetEndpoint Include="_content/Blazorise/floatingUi.js"><AssetFile>$([System.IO.Path]::Ge...`|21|0.7%|21|0.7%|5|Element|
1341827504385|1341768530321|Properties (pass 1)|Piper.Test.csproj|0|`<Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk" />`|47|1.6%|20|0.7%|0|Element|
1348900656899|1342010987399|Lazy items (pass 3.1)|MSTest.TestAdapter.props|14|`<TestingPlatformBuilderHook Include="031F8871-2660-4208-8F6B-FC142B40ABFF"><DisplayName>MSTest</Disp...`|20|0.6%|20|0.6%|1|Element|
1352227120127|1342017540597|Targets (pass 5)|Microsoft.NET.ConflictResolution.targets|101|`<Target Name="_HandleFileConflictsForPublish" AfterTargets="ComputeFilesToPublish" Condition="'$(Err...`|19|0.6%|19|0.6%|19|Element|
1342556595911|1342394376392|Properties (pass 1)|Microsoft.Common.props|74|`<Import Project="$(MSBuildProjectExtensionsPath)$(MSBuildProjectFile).*.props" Condition="'$(ImportP...`|80|2.7%|18|0.6%|9|Element|
1341825866199|1341824228015|Properties (pass 1)|Piper.csproj|0|`<Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk" />`|47|1.6%|18|0.6%|1|Element|
1344359694461|1344356415002|Properties (pass 1)|Microsoft.CSharp.Core.targets|4|`<Import Project="Microsoft.Managed.Core.targets"  />`|20|0.7%|18|0.6%|9|Element|
1346167284674|1343266216662|Properties (pass 1)|Sdk.targets|42|`<Import Project="$(MSBuildThisFileDirectory)..\targets\Microsoft.NET.Sdk.targets" Condition="'$(IsCr...`|156|5.2%|17|0.6%|9|Element|
1351337580366|1342010987399|Lazy items (pass 3.1)|Microsoft.AspNetCore.StaticWebAssetEndpoints.props|213|`<StaticWebAssetEndpoint Include="_content/Blazorise.Icons.FontAwesome/v6/css/all.min.6en5hypow4.css"...`|17|0.6%|17|0.6%|5|Element|
1345898201592|1345793200856|Properties (pass 1)|Microsoft.Testing.Platform.MSBuild.targets|2|`<Import Project="$(MSBuildThisFileDirectory)..\buildMultiTargeting\Microsoft.Testing.Platform.MSBuil...`|25|0.8%|17|0.6%|1|Element|
1348112373329|1342001157632|Items (pass 3)|Microsoft.AspNetCore.StaticWebAssetEndpoints.props|2|`<ItemGroup><StaticWebAssetEndpoint Include="_content/BlazorMonaco/jsInterop.3rwx0l4qnu.js"><AssetFil...`|57|1.9%|16|0.5%|5|Element|
1344356415002|1344274429827|Properties (pass 1)|Microsoft.CSharp.CurrentVersion.targets|340|`<Import Project="$(CSharpCoreTargetsPath)" />`|41|1.4%|15|0.5%|9|Element|
1349138829549|1349135544266|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.targets|104|`Condition="'%(Link)' == '' And '%(DefiningProjectExtension)' != '.projitems' And !$([MSBuild]::Value...`|15|0.5%|15|0.5%|215|Condition|
1351970586971|1342010987399|Lazy items (pass 3.1)|Sdk.StaticWebAssets.StaticAssets.ProjectSystem.props|48|`<Compile Remove="wwwroot\**" />`|14|0.5%|14|0.5%|5|Element|
1344576147632|1344454800512|Properties (pass 1)|Microsoft.Common.CurrentVersion.targets|1222|`<PropertyGroup><Framework40Dir>@(_TargetFramework40DirectoryItem)</Framework40Dir><Framework35Dir>@(...`|13|0.4%|13|0.4%|7|Element|
1345622583396|1344454800512|Properties (pass 1)|Microsoft.Common.CurrentVersion.targets|7045|`<Import Condition="'$(IsRestoreTargetsFileLoaded)' != 'true' and Exists('$(NuGetRestoreTargets)')" P...`|20|0.7%|12|0.4%|9|Element|
1346938586399|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|1382|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.Publish.targets"  />`|16|0.5%|12|0.4%|9|Element|
1346618551457|1341999519341|Item definition groups (pass 2)|Microsoft.Common.CurrentVersion.targets|2111|`<ProjectReference><!-- Target to build in the project reference; by default, this property is blank,...`|11|0.4%|11|0.4%|7|Element|
1343041674347|1343033479727|Properties (pass 1)|Microsoft.NET.Sdk.DefaultItems.props|18|`<Import Project="$(NETCoreSdkBundledVersionsProps)" Condition="Exists('$(NETCoreSdkBundledVersionsPr...`|13|0.4%|11|0.4%|9|Element|
1341825866196|1341824228012|Properties (pass 1)|Sdk.props|15|`Condition="'$([MSBuild]::GetTargetPlatformIdentifier($(TargetFramework)))' != 'browser'")`|11|0.4%|11|0.4%|1|Condition|
1341824228014|1341822589831|Properties (pass 1)|Piper.UI.csproj|0|`<Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk.Razor" />`|67|2.2%|10|0.3%|1|Element|
1341822589830|1341820951648|Properties (pass 1)|Piper.Host.Photino.csproj|0|`<Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk.Razor" />`|68|2.3%|10|0.3%|1|Element|
1341820951647|1341819313466|Properties (pass 1)|Piper.Host.BlazorServer.csproj|0|`<Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk.Web" />`|84|2.8%|10|0.3%|1|Element|
1342140416052|1342556595911|Properties (pass 1)|BlazorMonaco.props|3|`<Import Project="Microsoft.AspNetCore.StaticWebAssets.props" />`|10|0.3%|10|0.3%|5|Element|
1349101049036|1342010987399|Lazy items (pass 3.1)|Microsoft.NETCoreSdk.BundledVersions.props|423|`<KnownRuntimePack Include="Microsoft.NETCore.App" TargetFramework="net7.0" RuntimeFrameworkName="Mic...`|10|0.3%|10|0.3%|9|Element|
1347037066379|1347033783656|Properties (pass 1)|Microsoft.NET.Sdk.StaticWebAssets.targets|||29|1%|9|0.3%|5|Element|
1342137139301|1342556595911|Properties (pass 1)|BlazorMonaco.props|2|`<Import Project="Microsoft.AspNetCore.StaticWebAssetEndpoints.props" />`|9|0.3%|9|0.3%|5|Element|
1346419984737|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|47|`<_GenerateSingleFileBundlePropertyInputsCache Condition="'$(_GenerateSingleFileBundlePropertyInputsC...`|9|0.3%|9|0.3%|9|Element|
1347299697179|1347232395597|Properties (pass 1)|Sdk.Razor.CurrentVersion.targets|140|`<_Targeting30OrNewerRazorLangVersion Condition="&#xA;        '$(RazorLangVersion)' == 'Latest' OR&#x...`|9|0.3%|9|0.3%|5|Element|
1348485137066|1342001157632|Items (pass 3)|Microsoft.AspNetCore.StaticWebAssets.props|2|`<ItemGroup><StaticWebAsset Include="$([System.IO.Path]::GetFullPath('$(MSBuildThisFileDirectory)..\s...`|43|1.4%|9|0.3%|5|Element|
1348677286517|1348672359437|Items (pass 3)|Microsoft.AspNetCore.StaticWebAssetEndpoints.props|15|`<StaticWebAssetEndpoint Include="_content/Blazorise/blazorise.min.xzuw7s6v8g.css"><AssetFile>$([Syst...`|8|0.3%|8|0.3%|5|Element|
1343680931732|1343580935286|Properties (pass 1)|WorkloadManifest.targets|||15|0.5%|8|0.3%|9|Element|
1352016629597|1342010987399|Lazy items (pass 3.1)|Microsoft.NET.Sdk.Web.DefaultItems.props|29|`<None Include="@(_WebToolingArtifacts-&gt;Distinct())" CopyToOutputDirectory="Never" CopyToPublishDi...`|8|0.3%|8|0.3%|1|Element|
1351600630046|1342010987399|Lazy items (pass 3.1)|Microsoft.AspNetCore.StaticWebAssets.props|1963|`<StaticWebAsset Include="$([System.IO.Path]::GetFullPath('$(MSBuildThisFileDirectory)..\staticwebass...`|7|0.2%|7|0.2%|5|Element|
1346758049207|1342001157632|Items (pass 3)|Microsoft.NETCoreSdk.BundledVersions.props|33|`<ItemGroup><ImplicitPackageReferenceVersion Include="Microsoft.NETCore.App" TargetFrameworkVersion="...`|31|1%|7|0.2%|7|Element|
1348830030461|1348815248411|Items (pass 3)|Microsoft.AspNetCore.StaticWebAssets.props|163|`<StaticWebAsset Include="$([System.IO.Path]::GetFullPath('$(MSBuildThisFileDirectory)..\staticwebass...`|7|0.2%|7|0.2%|5|Element|
1347718317899|1347637872002|Items (pass 3)|Microsoft.NETCoreSdk.BundledVersions.props|401|`<KnownCrossgen2Pack Include="Microsoft.NETCore.App.Crossgen2" TargetFramework="net7.0" Crossgen2Pack...`|7|0.2%|7|0.2%|9|Element|
1348853024922|1348815248411|Items (pass 3)|Microsoft.AspNetCore.StaticWebAssets.props|443|`<StaticWebAsset Include="$([System.IO.Path]::GetFullPath('$(MSBuildThisFileDirectory)..\staticwebass...`|7|0.2%|7|0.2%|5|Element|
1349462449526|1342010987399|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.targets|107|`<AdditionalFiles Update="@(AdditionalFiles)" ><LinkBase Condition="'%(LinkBase)' != ''">$([MSBuild]:...`|8|0.3%|7|0.2%|9|Element|
1349692456889|1342017540597|Targets (pass 5)|Microsoft.Common.CurrentVersion.targets|5756|`<Target Name="_CleanGetCurrentAndPriorFileWrites" DependsOnTargets="_CheckForCompileOutputs;_SGenChe...`|7|0.2%|7|0.2%|15|Element|
1343277690131|1343266216662|Properties (pass 1)|Sdk.targets|22|`<Import Project="$(MSBuildThisFileDirectory)..\targets\Microsoft.NET.Sdk.BeforeCommon.targets" Condi...`|190|6.3%|7|0.2%|9|Element|
1342094541872|1342017540597|Targets (pass 5)|NuGet.targets|474|`<Target Name="_FilterRestoreGraphProjectInputItems" DependsOnTargets="_LoadRestoreGraphEntryPoints" ...`|6|0.2%|6|0.2%|17|Element|
1346362550822|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|25|`<Import Project="Microsoft.NET.Sdk.FrameworkReferenceResolution.targets"  />`|7|0.2%|6|0.2%|9|Element|
1347857872599|1342001157632|Items (pass 3)|Microsoft.NET.Sdk.DefaultItems.props|44|`<None Remove="**/*$(DefaultLanguageSourceExtension)"  />`|6|0.2%|6|0.2%|9|Element|
1342722102722|1341824228012|Properties (pass 1)|Sdk.props|19|`<Import Sdk="Microsoft.NET.Sdk" Project="Sdk.props" Condition="'$(_RazorSdkImportsMicrosoftNetSdk)' ...`|98|3.3%|6|0.2%|5|Element|
1351582544562|1349132258987|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.props|36|`Glob="root: '$/home/marco/workspace/flyingpie/piper_1/src/40-Host/Piper.Host.Photino', pattern: '$**...`|6|0.2%|6|0.2%|1|Glob|
1351365528182|1342017540597|Targets (pass 5)|NuGet.Build.Tasks.Pack.targets|313|`<Target Name="_GetProjectReferenceVersions" Condition="'$(NuspecFile)' == ''" DependsOnTargets="_Get...`|6|0.2%|6|0.2%|15|Element|
1342430424327|1342394376392|Properties (pass 1)|Microsoft.Common.props|34|`<Import Project="$(DirectoryBuildPropsPath)" Condition="'$(ImportDirectoryBuildProps)' == 'true' and...`|12|0.4%|6|0.2%|9|Element|
1349510092292|1342017540597|Targets (pass 5)|Verify.props|60|`<Warning Condition="'$(VerifyShouldDiscover)' == 'true' And '$(VerifySlnxCount)' &gt; '1'" Text="Mul...`|6|0.2%|6|0.2%|1|Element|
1348672359437|1342001157632|Items (pass 3)|Microsoft.AspNetCore.StaticWebAssetEndpoints.props|2|`<ItemGroup><StaticWebAssetEndpoint Include="_content/Blazorise/blazorise.css"><AssetFile>$([System.I...`|28|0.9%|6|0.2%|5|Element|
1348977855386|1342010987399|Lazy items (pass 3.1)|NuGet.targets|156|`<PackageVersion Include="@(GlobalPackageReference)" Version="%(Version)"  />`|6|0.2%|6|0.2%|9|Element|
1349132258987|1342010987399|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.props|36|`<Compile Include="**/*$(DefaultLanguageSourceExtension)" Exclude="$(DefaultItemExcludes);$(DefaultEx...`|28|0.9%|6|0.2%|9|Element|
1349393451149|1342010987399|Lazy items (pass 3.1)|Microsoft.Common.CurrentVersion.targets|398|`<FinalDocFile Include="@(DocFileItem-&gt;'$(OutDir)%(Filename)%(Extension)')" />`|6|0.2%|6|0.2%|9|Element|
1346216510099|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|19|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.PackageDependencyResolution.targets" Condition...`|10|0.3%|5|0.2%|9|Element|
1349398379546|1342010987399|Lazy items (pass 3.1)|Microsoft.Common.CurrentVersion.targets|404|`<CreateDirectory Include="@(IntermediateRefAssembly-&gt;'%(RootDir)%(Directory)')" />`|5|0.2%|5|0.2%|9|Element|
1347931757454|1342001157632|Items (pass 3)|Microsoft.AspNetCore.StaticWebAssetEndpoints.props|2|`<ItemGroup><StaticWebAssetEndpoint Include="_content/Radzen.Blazor/css/dark-base.6bstwmtvpd.css"><As...`|21|0.7%|5|0.2%|5|Element|
1347271791411|1343266216662|Properties (pass 1)|Sdk.targets|53|`<Import Project="$(NuGetBuildTasksPackTargets)" Condition="Exists('$(NuGetBuildTasksPackTargets)') A...`|10|0.3%|5|0.2%|9|Element|
1346170566341|1346167284674|Properties (pass 1)|Microsoft.NET.Sdk.targets|||139|4.6%|5|0.2%|9|Element|
1342033923662|1342017540597|Targets (pass 5)|Microsoft.NET.Sdk.Solution.targets|27|`<NETSdkError ResourceName="CannotHaveSolutionLevelRuntimeIdentifier"  />`|5|0.2%|5|0.2%|0|Element|
1346895912872|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|1380|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.CrossGen.targets"  />`|6|0.2%|5|0.2%|9|Element|
1342369798532|1342368160016|Properties (pass 1)|Sdk.props|14|`Condition="'$(_AfterSdkPublishDependsOn)' == ''")`|5|0.2%|5|0.2%|9|Condition|
1343123621922|1342871231399|Properties (pass 1)|Microsoft.NET.Sdk.props|158|`<Import Project="Microsoft.NET.SupportedTargetFrameworks.props"  />`|8|0.3%|5|0.2%|9|Element|
1343805522674|1343580935286|Properties (pass 1)|WorkloadManifest.targets|||5|0.2%|5|0.2%|9|Element|
1342771265117|1342725380187|Properties (pass 1)|Sdk.StaticWebAssets.CurrentVersion.props|34|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.Sdk.StaticWebAssets.ContentTypeMappings.pr...`|5|0.2%|5|0.2%|5|Element|
1351508559748|1349132258987|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.props|36|`Glob="root: '$/home/marco/workspace/flyingpie/piper_1/src/20-UI/Piper.UI', pattern: '$**/*.cs', excl...`|5|0.2%|5|0.2%|1|Glob|
1342717186532|1341824228012|Properties (pass 1)|Sdk.props|15|`Condition="'$(UsingMicrosoftNETSdk)' != 'true'")`|5|0.1%|5|0.1%|5|Condition|
1345945781399|1345898201592|Properties (pass 1)|Microsoft.Testing.Platform.MSBuild.targets|326|`<_TestArchitecture Condition="('$(_TestArchitecture)' == '' or '$(_TestArchitecture)' == 'AnyCpu') a...`|4|0.1%|4|0.1%|1|Element|
1346795796866|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|1376|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.GenerateAssemblyInfo.targets" Condition="'...`|8|0.3%|4|0.1%|9|Element|
1349137186907|1349135544266|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.targets|93|`Condition="'%(LinkBase)' != ''")`|4|0.1%|4|0.1%|215|Condition|
1351968942606|1349132258987|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.props|36|`Glob="root: '$/home/marco/workspace/flyingpie/piper_1/src/40-Host/Piper.Host.BlazorServer', pattern:...`|4|0.1%|4|0.1%|1|Glob|
1344592546274|1344454800512|Properties (pass 1)|Microsoft.Common.CurrentVersion.targets|93|`<TargetPlatformSdkPath Condition="'$(TargetPlatformSdkPath)' == ''">$([Microsoft.Build.Utilities.Too...`|4|0.1%|4|0.1%|9|Element|
1352136672987|1349464092366|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.targets|119|`Condition="'%(Link)' == '' And '%(DefiningProjectExtension)' != '.projitems' And !$([MSBuild]::Value...`|4|0.1%|4|0.1%|105|Condition|
1351352376231|1342010987399|Lazy items (pass 3.1)|Microsoft.AspNetCore.StaticWebAssetEndpoints.props|267|`<StaticWebAssetEndpoint Include="_content/Blazorise.Icons.FontAwesome/v6/webfonts/fa-regular-400.ttf...`|4|0.1%|4|0.1%|5|Element|
1343995698932|1343280968274|Properties (pass 1)|Microsoft.NET.Sdk.BeforeCommon.targets|82|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.RuntimeIdentifierInference.targets"  />`|12|0.4%|4|0.1%|9|Element|
1343376036161|1343280968274|Properties (pass 1)|Microsoft.NET.Sdk.BeforeCommon.targets|54|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.TargetFrameworkInference.targets"  />`|52|1.7%|4|0.1%|9|Element|
1342158438254|1342556595911|Properties (pass 1)|Blazorise.props|3|`<Import Project="Microsoft.AspNetCore.StaticWebAssets.props" />`|4|0.1%|4|0.1%|5|Element|
1348204327880|1349132258987|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.props|36|`Glob="root: '$/home/marco/workspace/flyingpie/piper_1/src/10-Core/Piper', pattern: '$**/*.cs', exclu...`|4|0.1%|4|0.1%|0|Glob|
1347230754116|1341819313466|Properties (pass 1)|Sdk.targets|21|`<Import Project="$(RazorSdkCurrentVersionTargets)" />`|28|0.9%|4|0.1%|5|Element|
1342420593024|1342394376392|Properties (pass 1)|Microsoft.Common.props|28|`<_DirectoryBuildPropsBasePath Condition="'$(_DirectoryBuildPropsBasePath)' == ''">$([MSBuild]::GetDi...`|4|0.1%|4|0.1%|9|Element|
1342109287079|1342556595911|Properties (pass 1)|Radzen.Blazor.props|3|`<Import Project="Microsoft.AspNetCore.StaticWebAssets.props" />`|4|0.1%|4|0.1%|5|Element|
1347467137678|1342001157632|Items (pass 3)|Piper.UI.csproj.nuget.g.props|13|`<SourceRoot Include="/home/marco/.nuget/packages/"  />`|4|0.1%|4|0.1%|1|Element|
1343044952235|1343680931732|Properties (pass 1)|WorkloadManifest.targets|3|`<PropertyGroup><_RuntimePackInWorkloadVersionCurrent>10.0.9</_RuntimePackInWorkloadVersionCurrent><_...`|4|0.1%|4|0.1%|7|Element|
1342155161481|1342556595911|Properties (pass 1)|Blazorise.props|2|`<Import Project="Microsoft.AspNetCore.StaticWebAssetEndpoints.props" />`|4|0.1%|3|0.1%|5|Element|
1346890989047|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|1379|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.ComposeStore.targets" Condition="'$(UsingN...`|4|0.1%|3|0.1%|9|Element|
1341773444774|1341771806622|Properties (pass 1)|Piper.slnx.metaproj|0|`Condition="'$(ImportByWildcardBeforeSolution)' != 'false' and exists('$(MSBuildExtensionsPath)\$(MSB...`|3|0.1%|3|0.1%|0|Condition|
1341838971674|1341768530321|Properties (pass 1)|Piper.slnx.metaproj|0|`<Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\SolutionFile\ImportAfter\*" Conditi...`|6|0.2%|3|0.1%|0|Element|
1344408886826|1344356415002|Properties (pass 1)|Microsoft.CSharp.Core.targets|23|`<_MaxSupportedLangVersion Condition="'$(TargetFrameworkIdentifier)' == '.NETCoreApp' AND&#xA;       ...`|3|0.1%|3|0.1%|9|Element|
1347759362649|1347637872002|Items (pass 3)|Microsoft.NETCoreSdk.BundledVersions.props|631|`<KnownFrameworkReference Include="Microsoft.WindowsDesktop.App.WindowsForms" TargetFramework="net5.0...`|3|0.1%|3|0.1%|9|Element|
1348887516981|1342001157632|Items (pass 3)|Microsoft.AspNetCore.StaticWebAssetEndpoints.props|2|`<ItemGroup><StaticWebAssetEndpoint Include="_content/Blazorise.Icons.FontAwesome/v5/css/all.css"><As...`|12|0.4%|3|0.1%|5|Element|
1342004434217|1342001157632|Items (pass 3)|Piper.slnx.metaproj|0|`<SolutionConfiguration Include="Debug\|Any CPU" ><Configuration>Debug</Configuration><Platform>Any C...`|3|0.1%|3|0.1%|0|Element|
1350234694541|1342017540597|Targets (pass 5)|Microsoft.Common.CurrentVersion.targets|3983|`<SGen BuildAssemblyName="$(TargetFileName)" BuildAssemblyPath="$(IntermediateOutputPath)" References...`|3|0.1%|3|0.1%|5|Element|
1342012625697|1342010987399|Lazy items (pass 3.1)|Piper.slnx.metaproj|0|`<SolutionConfiguration Include="Debug\|Any CPU" ><Configuration>Debug</Configuration><Platform>Any C...`|3|0.1%|3|0.1%|0|Element|
1342106010366|1342556595911|Properties (pass 1)|Radzen.Blazor.props|2|`<Import Project="Microsoft.AspNetCore.StaticWebAssetEndpoints.props" />`|3|0.1%|3|0.1%|5|Element|
1348815248411|1342001157632|Items (pass 3)|Microsoft.AspNetCore.StaticWebAssets.props|2|`<ItemGroup><StaticWebAsset Include="$([System.IO.Path]::GetFullPath('$(MSBuildThisFileDirectory)..\s...`|31|1%|3|0.1%|5|Element|
1348836600287|1348815248411|Items (pass 3)|Microsoft.AspNetCore.StaticWebAssets.props|243|`<StaticWebAsset Include="$([System.IO.Path]::GetFullPath('$(MSBuildThisFileDirectory)..\staticwebass...`|3|0.1%|3|0.1%|5|Element|
1348889159441|1342010987399|Lazy items (pass 3.1)|Microsoft.NET.Sdk.targets|1370|`<ProjectCapability Remove="ReferenceManagerAssemblies"  />`|3|0.1%|3|0.1%|9|Element|
1343230157507|1342871231399|Properties (pass 1)|Microsoft.NET.Sdk.props|175|`<Import Project="$(MSBuildThisFileDirectory)../../Microsoft.NET.Sdk.WindowsDesktop/targets/Microsoft...`|6|0.2%|3|0.1%|9|Element|
1348371824546|1342001157632|Items (pass 3)|NuGet.targets|156|`<PackageVersion Include="@(GlobalPackageReference)" Version="%(Version)"  />`|3|0.1%|3|0.1%|9|Element|
1348516339862|1342001157632|Items (pass 3)|Microsoft.NET.Sdk.targets|511|`<ItemGroup ><RuntimeHostConfigurationOption Include="Microsoft.Extensions.DependencyInjection.Verify...`|6|0.2%|3|0.1%|9|Element|
1347493403766|1347485195576|Items (pass 3)|Microsoft.AspNetCore.StaticWebAssetEndpoints.props|27|`<StaticWebAssetEndpoint Include="_content/Z.Blazor.Diagrams/script.gd8b3shkm2.js"><AssetFile>$([Syst...`|3|0.1%|3|0.1%|8|Element|
1345949062796|1345898201592|Properties (pass 1)|Microsoft.Testing.Platform.MSBuild.targets|330|`<_TestArchitecture Condition="('$(_TestArchitecture)' == '' or '$(_TestArchitecture)' == 'AnyCpu') a...`|3|0.1%|3|0.1%|1|Element|
1351972231337|1342010987399|Lazy items (pass 3.1)|Microsoft.NET.Sdk.Web.DefaultItems.props|35|`<Compile Remove="@(_WebToolingArtifacts)" />`|4|0.1%|3|0.1%|1|Element|
1349377023224|1342010987399|Lazy items (pass 3.1)|Microsoft.Managed.Core.targets|142|`<PotentialEditorConfigFiles Include="@(_AllDirectoriesAbove-&gt;'%(FullPath)'-&gt;Distinct()-&gt;Com...`|3|0.1%|3|0.1%|9|Element|
1343280968274|1343277690131|Properties (pass 1)|Microsoft.NET.Sdk.BeforeCommon.targets|||183|6.1%|3|0.1%|9|Element|
1342173183782|1342556595911|Properties (pass 1)|Blazorise.Icons.FontAwesome.props|2|`<Import Project="Microsoft.AspNetCore.StaticWebAssetEndpoints.props" />`|3|0.1%|3|0.1%|5|Element|
1348051619366|1342001157632|Items (pass 3)|Microsoft.AspNetCore.StaticWebAssets.props|2|`<ItemGroup><StaticWebAsset Include="$([System.IO.Path]::GetFullPath('$(MSBuildThisFileDirectory)..\s...`|15|0.5%|3|0.1%|5|Element|
1346336295726|1346290349924|Properties (pass 1)|Microsoft.NET.Sdk.DefaultItems.targets|71|`<Import Project="Microsoft.NET.Sdk.DefaultItems.Shared.targets"  />`|4|0.1%|3|0.1%|9|Element|
1345719374666|1344454800512|Properties (pass 1)|Microsoft.Common.CurrentVersion.targets|7049|`<Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.targets\ImportAfte...`|11|0.3%|3|0.1%|9|Element|
1343128538856|1343123621922|Properties (pass 1)|Microsoft.NET.SupportedTargetFrameworks.props|38|`<UnsupportedTargetFrameworkVersion>$([MSBuild]::Add($(NETCoreAppMaximumVersion), 1)).0</UnsupportedT...`|3|0.1%|3|0.1%|9|Element|
1343454715577|1343280968274|Properties (pass 1)|Microsoft.NET.Sdk.BeforeCommon.targets|57|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.DefaultOutputPaths.targets" Condition="'$(...`|9|0.3%|3|0.1%|9|Element|
1345762029551|1345719374666|Properties (pass 1)|Microsoft.TestPlatform.ImportAfter.targets|19|`<Import Condition="Exists('$(VSTestTargets)')" Project="$(VSTestTargets)"  />`|4|0.1%|3|0.1%|9|Element|
1352105428304|1342010987399|Lazy items (pass 3.1)|Sdk.StaticWebAssets.StaticAssets.ProjectSystem.props|31|`<Content Include="wwwroot\**" ExcludeFromSingleFile="true" CopyToPublishDirectory="PreserveNewest" E...`|5|0.2%|2|0.1%|11|Element|
1342871231399|1342869592577|Properties (pass 1)|Microsoft.NET.Sdk.props|||121|4%|2|0.1%|9|Element|
1346641527060|1342001157632|Items (pass 3)|Piper.csproj.nuget.g.props|13|`<SourceRoot Include="/home/marco/.nuget/packages/"  />`|2|0.1%|2|0.1%|1|Element|
1342391099331|1342366521501|Properties (pass 1)|Sdk.props|49|`<Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" Condition="...`|123|4.1%|2|0.1%|9|Element|
1351442797166|1342010987399|Lazy items (pass 3.1)|Microsoft.AspNetCore.StaticWebAssets.props|43|`<StaticWebAsset Include="$([System.IO.Path]::GetFullPath('$(MSBuildThisFileDirectory)..\staticwebass...`|2|0.1%|2|0.1%|5|Element|
1343997338441|1343995698932|Properties (pass 1)|Microsoft.NET.RuntimeIdentifierInference.targets|||7|0.2%|2|0.1%|9|Element|
1352115295007|1342010987399|Lazy items (pass 3.1)|Sdk.StaticWebAssets.StaticAssets.ProjectSystem.props|37|`<Content Include="**\*.json" ExcludeFromSingleFile="true" CopyToOutputDirectory="PreserveNewest" Cop...`|5|0.2%|2|0.1%|11|Element|
1347880858772|1342001157632|Items (pass 3)|Microsoft.NET.Sdk.DefaultItems.props|77|`<PackageReference Update="Microsoft.NETCore.App" Condition="('$(_TargetFrameworkVersionWithoutV)' !=...`|2|0.1%|2|0.1%|9|Element|
1348199401631|1342001157632|Items (pass 3)|Microsoft.Managed.Core.targets|139|`<_AllDirectoriesAbove Include="@(Compile-&gt;GetPathsOfAllDirectoriesAbove())" Condition="'$(Discove...`|2|0.1%|2|0.1%|9|Element|
1347097797476|1347037066379|Properties (pass 1)|Microsoft.NET.Sdk.StaticWebAssets.targets|779|`<Import Project="Microsoft.NET.Sdk.StaticWebAssets.ScopedCss.targets" Condition="'$(UsingMicrosoftNE...`|3|0.1%|2|0.1%|5|Element|
1347591904029|1342001157632|Items (pass 3)|Microsoft.NET.Sdk.DefaultItems.targets|87|`<Compile Update="@(Compile)" ><!-- First, add a trailing slash to the LinkBase metadata if necessary...`|2|0.1%|2|0.1%|7|Element|
1347795482546|1347637872002|Items (pass 3)|Microsoft.NETCoreSdk.BundledVersions.props|817|`<WindowsSdkSupportedTargetPlatformVersion Include="10.0.18362.1" WindowsSdkPackageVersion="10.0.1836...`|2|0.1%|2|0.1%|19|Element|
1352112006102|1342010987399|Lazy items (pass 3.1)|Sdk.StaticWebAssets.StaticAssets.ProjectSystem.props|36|`<Content Include="**\*.config" ExcludeFromSingleFile="true" CopyToOutputDirectory="PreserveNewest" C...`|5|0.2%|2|0.1%|11|Element|
1347468779271|1342001157632|Items (pass 3)|Directory.Build.props|84|`<PackageReference Include="coverlet.collector" />`|2|0.1%|2|0.1%|1|Element|
1347124059996|1347037066379|Properties (pass 1)|Microsoft.NET.Sdk.StaticWebAssets.targets|781|`<Import Project="Microsoft.NET.Sdk.StaticWebAssets.JSModules.targets" Condition="'$(JSModulesEnabled...`|3|0.1%|2|0.1%|5|Element|
1347537728455|1342001157632|Items (pass 3)|Piper.Host.Photino.csproj.nuget.g.props|13|`<SourceRoot Include="/home/marco/.nuget/packages/"  />`|2|0.1%|2|0.1%|1|Element|
1342176460577|1342556595911|Properties (pass 1)|Blazorise.Icons.FontAwesome.props|3|`<Import Project="Microsoft.AspNetCore.StaticWebAssets.props" />`|2|0.1%|2|0.1%|5|Element|
1346142672299|1343266216662|Properties (pass 1)|Microsoft.CSharp.targets|36|`<Import Project="$(MSBuildToolsPath)\Microsoft.Managed.After.targets" />`|3|0.1%|2|0.1%|9|Element|
1347688766066|1347637872002|Items (pass 3)|Microsoft.NETCoreSdk.BundledVersions.props|221|`<KnownFrameworkReference Include="Microsoft.WindowsDesktop.App" TargetFramework="net9.0" RuntimeFram...`|2|0.1%|2|0.1%|9|Element|
1347892351962|1342001157632|Items (pass 3)|Piper.Host.BlazorServer.csproj.nuget.g.props|13|`<SourceRoot Include="/home/marco/.nuget/packages/"  />`|2|0.1%|2|0.1%|1|Element|
1346290349924|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|23|`<Import Project="Microsoft.NET.Sdk.DefaultItems.targets"  />`|10|0.3%|2|0.1%|9|Element|
1347826677392|1347637872002|Items (pass 3)|Microsoft.NETCoreSdk.BundledVersions.props|840|`<WindowsSdkSupportedTargetPlatformVersion Include="10.0.18362.0" WindowsSdkPackageVersion="10.0.1836...`|2|0.1%|2|0.1%|19|Element|
1352108717201|1342010987399|Lazy items (pass 3.1)|Sdk.StaticWebAssets.StaticAssets.ProjectSystem.props|34|`<Content Include="wwwroot\.well-known\**" ExcludeFromSingleFile="true" CopyToPublishDirectory="Prese...`|3|0.1%|2|0.1%|11|Element|
1347066611036|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|1393|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.Sdk.Analyzers.targets" Condition="'$(Langu...`|10|0.3%|2|0.1%|9|Element|
1342869592577|1342366521501|Properties (pass 1)|Sdk.props|50|`<Import Project="$(MSBuildThisFileDirectory)..\targets\Microsoft.NET.Sdk.props"  />`|123|4.1%|2|0.1%|9|Element|
1349495306516|1342017540597|Targets (pass 5)|Sdk.props|19|`<Target Name="AfterSdkPublish" AfterTargets="$(_AfterSdkPublishDependsOn)" ></Target>`|2|0.1%|2|0.1%|19|Element|
1341781635549|1341768530321|Properties (pass 1)|Piper.slnx.metaproj|0|`<_DirectorySolutionPropsBasePath Condition="'$(_DirectorySolutionPropsBasePath)' == ''" >$([MSBuild]...`|2|0.1%|2|0.1%|0|Element|
1342394376392|1342391099331|Properties (pass 1)|Microsoft.Common.props|||120|4%|2|0.1%|9|Element|
1347839812172|1342001157632|Items (pass 3)|Microsoft.NET.Sdk.DefaultItems.props|36|`<Compile Include="**/*$(DefaultLanguageSourceExtension)" Exclude="$(DefaultItemExcludes);$(DefaultEx...`|2|0.1%|2|0.1%|9|Element|
1341888117899|1341838971674|Properties (pass 1)|Microsoft.NuGet.ImportAfter.targets|17|`<Import Condition="Exists('$(NuGetRestoreTargets)')" Project="$(NuGetRestoreTargets)"  />`|3|0.1%|2|0.1%|0|Element|
1349572522047|1342001157632|Items (pass 3)|Microsoft.NET.Sdk.StaticWebAssets.ContentTypeMappings.props|27|`<ItemGroup><StaticWebAssetContentTypeMapping Include="image/bmp" Cache="$(StaticWebAssetEndpointDefa...`|6|0.2%|2|0.1%|5|Element|
1342602477957|1342556595911|Properties (pass 1)|Z.Blazor.Diagrams.props|2|`<Import Project="Microsoft.AspNetCore.StaticWebAssetEndpoints.props" />`|2|0.1%|2|0.1%|8|Element|
1347232395597|1347230754116|Properties (pass 1)|Sdk.Razor.CurrentVersion.targets|||24|0.8%|2|0.1%|5|Element|
1347071535212|1347037066379|Properties (pass 1)|Microsoft.NET.Sdk.StaticWebAssets.targets|769|`<Import Project="Microsoft.NET.Sdk.StaticWebAssets.Publish.targets" />`|2|0.1%|2|0.1%|5|Element|
1348440796833|1349132258987|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.props|36|`Glob="root: '$/home/marco/workspace/flyingpie/piper_1/src/30-Test/Piper.Test', pattern: '$**/*.cs', ...`|2|0.1%|2|0.1%|0|Glob|
1342432062881|1342430424327|Properties (pass 1)|Microsoft.Common.props|34|`Condition="'$(ImportDirectoryBuildProps)' == 'true' and exists('$(DirectoryBuildPropsPath)')")`|2|0.1%|2|0.1%|9|Condition|
1348977855416|1342001157632|Items (pass 3)|Microsoft.AspNetCore.StaticWebAssets.props|2|`<ItemGroup><StaticWebAsset Include="$([System.IO.Path]::GetFullPath('$(MSBuildThisFileDirectory)..\s...`|9|0.3%|2|0.1%|5|Element|
1345793200856|1344448241366|Properties (pass 1)|Microsoft.Common.targets|42|`<Import Project="$(MSBuildProjectExtensionsPath)$(MSBuildProjectFile).*.targets" Condition="'$(Impor...`|51|1.7%|2|0.1%|9|Element|
1349319526274|1342010987399|Lazy items (pass 3.1)|Microsoft.NET.SupportedTargetFrameworks.props|75|`<SupportedTargetFramework Include="@(SupportedNETCoreAppTargetFramework);@(SupportedNETStandardTarge...`|2|0.1%|2|0.1%|9|Element|
1347081383531|1347037066379|Properties (pass 1)|Microsoft.NET.Sdk.StaticWebAssets.targets|775|`<Import Project="Microsoft.NET.Sdk.StaticWebAssets.EmbeddedAssets.targets" />`|2|0.1%|2|0.1%|5|Element|
1342368160016|1342366521501|Properties (pass 1)|Sdk.props|14|`<PropertyGroup Condition="'$(_AfterSdkPublishDependsOn)' == ''" ><_AfterSdkPublishDependsOn Conditio...`|8|0.3%|2|0.1%|9|Element|
1355929824782|1342017540597|Targets (pass 5)|Microsoft.NET.Sdk.Razor.GenerateAssemblyInfo.targets|121|`<Target Name="_CreateRazorTargetAssemblyInfoInputsCacheFile" Condition="'@(RazorTargetAssemblyAttrib...`|2|0.1%|2|0.1%|11|Element|
1342831899947|1342394376392|Properties (pass 1)|NuGet.props|35|`<Import Project="$(DirectoryPackagesPropsPath)" Condition="'$(ImportDirectoryPackagesProps)' == 'tru...`|2|0.1%|2|0.1%|9|Element|
1341842248091|1341824228012|Properties (pass 1)|Sdk.Server.props|28|`<Import Sdk="Microsoft.NET.Sdk" Project="Sdk.props"  />`|51|1.7%|2|0.1%|1|Element|
1346482342946|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|94|`<Import Project="Microsoft.NET.Sdk.Shared.targets"  />`|5|0.2%|2|0.1%|9|Element|
1341819313466|1341768530321|Properties (pass 1)|Piper.Host.BlazorServer.csproj|||282|9.4%|2|0.1%|1|Element|
1342725380187|1341824228012|Properties (pass 1)|Sdk.props|20|`<Import Sdk="Microsoft.NET.Sdk.StaticWebAssets" Project="Sdk.props" Condition="'$(_RazorSdkImportsMi...`|13|0.4%|2|0.1%|5|Element|
1347151964204|1347037066379|Properties (pass 1)|Microsoft.NET.Sdk.StaticWebAssets.targets|783|`<Import Project="Microsoft.NET.Sdk.StaticWebAssets.Compression.targets" Condition="'$(CompressionEna...`|4|0.1%|2|0.1%|5|Element|
1346779384761|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|1375|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.DesignerSupport.targets"  />`|3|0.1%|2|0.1%|9|Element|
1343033479727|1342871231399|Properties (pass 1)|Microsoft.NET.Sdk.props|144|`<Import Project="Microsoft.NET.Sdk.DefaultItems.props"  />`|19|0.6%|2|0.1%|9|Element|
1349173325262|1342010987399|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.props|37|`<EmbeddedResource Include="**/*.resx" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFold...`|3|0.1%|2|0.1%|9|Element|
1343457993936|1343454715577|Properties (pass 1)|Microsoft.NET.DefaultOutputPaths.targets|||6|0.2%|2|0.1%|9|Element|
1348205969954|1342010987399|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.targets|87|`<Compile Update="@(Compile)" ><!-- First, add a trailing slash to the LinkBase metadata if necessary...`|10|0.3%|2|0.1%|7|Element|
1345379800306|1345793200856|Properties (pass 1)|Piper.UI.csproj.nuget.g.targets|9|`<Import Project="$(NuGetPackageRoot)microsoft.codeanalysis.analyzers/5.3.0-2.25625.1/buildTransitive...`|2|0.1%|2|0.1%|1|Element|
1346874576362|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|1377|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.GenerateGlobalUsings.targets" Condition="'...`|2|0.1%|2|0.1%|9|Element|
1343576017524|1343280968274|Properties (pass 1)|Microsoft.NET.Sdk.BeforeCommon.targets|76|`<Import Project="Microsoft.NET.Sdk.ImportWorkloads.targets" Condition="'$(MSBuildEnableWorkloadResol...`|90|3%|2|0.1%|9|Element|
1344274429827|1344272790149|Properties (pass 1)|Microsoft.CSharp.CurrentVersion.targets|||270|9%|2|0.1%|9|Element|
1342605755276|1342556595911|Properties (pass 1)|Z.Blazor.Diagrams.props|3|`<Import Project="Microsoft.AspNetCore.StaticWebAssets.props" />`|2|0.1%|1|0%|8|Element|
1342786013981|1342394376392|Properties (pass 1)|Microsoft.Common.props|111|`<Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.VisualStudioVersion.v*.Co...`|1|0%|1|0%|9|Element|
1345489706166|1345793200856|Properties (pass 1)|Piper.Host.BlazorServer.csproj.nuget.g.targets|5|`<Import Project="$(NuGetPackageRoot)microsoft.codeanalysis.analyzers/5.3.0-2.25625.1/buildTransitive...`|2|0.1%|1|0%|1|Element|
1343377675292|1343376036161|Properties (pass 1)|Microsoft.NET.TargetFrameworkInference.targets|||48|1.6%|1|0%|9|Element|
1341976583372|1341891394346|Properties (pass 1)|NuGet.targets|1581|`<Import Project="NuGet.RestoreEx.targets" Condition="'$(RestoreUseStaticGraphEvaluation)' == 'true' ...`|2|0.1%|1|0%|10|Element|
1347486837212|1347485195576|Items (pass 3)|Microsoft.AspNetCore.StaticWebAssetEndpoints.props|3|`<StaticWebAssetEndpoint Include="_content/Z.Blazor.Diagrams/default.styles.9tb2uy8qwd.css"><AssetFil...`|1|0%|1|0%|8|Element|
1346674349657|1346648091521|Properties (pass 1)|Microsoft.NET.Sdk.SourceLink.targets|28|`<Import Project="$(MSBuildThisFileDirectory)..\..\Microsoft.SourceLink.Common\$(_SourceLinkSdkSubDir...`|3|0.1%|1|0%|9|Element|
1348076249156|1342001157632|Items (pass 3)|Verify.AfterMicrosoftNetSdk.props|4|`<None Include="**\*.received.*;**\*.verified.*" Condition="$(Language) == 'C#'" ><ParentFile>$([Syst...`|1|0%|1|0%|1|Element|
1347485195576|1342001157632|Items (pass 3)|Microsoft.AspNetCore.StaticWebAssetEndpoints.props|2|`<ItemGroup><StaticWebAssetEndpoint Include="_content/Z.Blazor.Diagrams/default.styles.9tb2uy8qwd.css...`|9|0.3%|1|0%|8|Element|
1349178253257|1342010987399|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.props|43|`<None Include="**/*" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)"  />`|3|0.1%|1|0%|9|Element|
1343387510099|1343377675292|Properties (pass 1)|Microsoft.NET.TargetFrameworkInference.targets|55|`<TargetFrameworkVersion >v$([MSBuild]::GetTargetFrameworkVersion('$(TargetFramework)', 2))</TargetFr...`|1|0%|1|0%|9|Element|
1345194446859|1345793200856|Properties (pass 1)|Piper.csproj.nuget.g.targets|9|`<Import Project="$(NuGetPackageRoot)microsoft.codeanalysis.analyzers/5.3.0-2.25625.1/buildTransitive...`|2|0.1%|1|0%|1|Element|
1347074817981|1347037066379|Properties (pass 1)|Microsoft.NET.Sdk.StaticWebAssets.targets|771|`<Import Project="Microsoft.NET.Sdk.StaticWebAssets.References.targets" />`|1|0%|1|0%|5|Element|
1343825195456|1343580935286|Properties (pass 1)|WorkloadManifest.targets|||3|0.1%|1|0%|9|Element|
1343133455799|1342871231399|Properties (pass 1)|Microsoft.NET.Sdk.props|164|`<Import Project="Microsoft.NET.WindowsSdkSupportedTargetPlatforms.props"  />`|1|0%|1|0%|9|Element|
1346690761122|1346648091521|Properties (pass 1)|Microsoft.NET.Sdk.SourceLink.targets|29|`<Import Project="$(MSBuildThisFileDirectory)..\..\Microsoft.SourceLink.GitHub\build\Microsoft.Source...`|2|0.1%|1|0%|9|Element|
1348678928882|1349462449526|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.targets|109|`Condition="'%(Link)' == '' And '%(DefiningProjectExtension)' != '.projitems' And !$([MSBuild]::Value...`|1|0%|1|0%|31|Condition|
1349128973712|1342010987399|Lazy items (pass 3.1)|Microsoft.NETCoreSdk.BundledVersions.props|846|`<_KnownRuntimeIdentiferPlatforms Include="any;aot;freebsd;illumos;solaris;unix;any;aot;freebsd;illum...`|1|0%|1|0%|9|Element|
1342366521501|1341768530321|Properties (pass 1)|Sdk.props|||257|8.5%|1|0%|9|Element|
1355644948277|1342017540597|Targets (pass 5)|Microsoft.NET.Sdk.StaticWebAssets.ScopedCss.targets|139|`<Target Name="ComputeCssScope" DependsOnTargets="ResolveScopedCssInputs"><ComputeCssScope ScopedCssI...`|1|0%|1|0%|11|Element|
1346707172687|1346648091521|Properties (pass 1)|Microsoft.NET.Sdk.SourceLink.targets|30|`<Import Project="$(MSBuildThisFileDirectory)..\..\Microsoft.SourceLink.GitLab\build\Microsoft.Source...`|2|0.1%|1|0%|9|Element|
1352126806206|1342010987399|Lazy items (pass 3.1)|Sdk.Razor.CurrentVersion.props|79|`<Content Include="**\*.razor" ExcludeFromSingleFile="true" Exclude="$(DefaultItemExcludes);$(Default...`|2|0.1%|1|0%|5|Element|
1347020652774|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|1387|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.ConflictResolution.targets"  />`|2|0.1%|1|0%|9|Element|
1341891394346|1341888117899|Properties (pass 1)|NuGet.targets|||8|0.3%|1|0%|10|Element|
1347962953847|1342001157632|Items (pass 3)|Microsoft.NET.SupportedTargetFrameworks.props|75|`<SupportedTargetFramework Include="@(SupportedNETCoreAppTargetFramework);@(SupportedNETStandardTarge...`|1|0%|1|0%|9|Element|
1343948153606|1343580935286|Properties (pass 1)|WorkloadManifest.targets|||2|0.1%|1|0%|9|Element|
1346666143962|1346648091521|Properties (pass 1)|Microsoft.NET.Sdk.SourceLink.targets|27|`<Import Project="$(MSBuildThisFileDirectory)..\..\Microsoft.Build.Tasks.Git\build\Microsoft.Build.Ta...`|2|0.1%|1|0%|9|Element|
1349544592751|1342017540597|Targets (pass 5)|Microsoft.NET.TargetFrameworkInference.targets|142|`<ItemGroup ><TFTelemetry Include="TargetFrameworkVersion" Value="$([MSBuild]::Escape('$(TargetFramew...`|1|0%|1|0%|5|Element|
1347637872002|1342001157632|Items (pass 3)|Microsoft.NETCoreSdk.BundledVersions.props|33|`<ItemGroup><ImplicitPackageReferenceVersion Include="Microsoft.NETCore.App" TargetFrameworkVersion="...`|2|0.1%|1|0%|1|Element|
1348202685774|1342001157632|Items (pass 3)|Microsoft.Managed.Core.targets|142|`<PotentialEditorConfigFiles Include="@(_AllDirectoriesAbove-&gt;'%(FullPath)'-&gt;Distinct()-&gt;Com...`|1|0%|1|0%|9|Element|
1346172207176|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|12|`<Import Project="Microsoft.NET.Sdk.Common.targets"  />`|3|0.1%|1|0%|9|Element|
1347257017902|1343266216662|Properties (pass 1)|Microsoft.NET.ApiCompat.targets|28|`<Import Project="Microsoft.NET.ApiCompat.ValidatePackage.targets" />`|2|0.1%|1|0%|9|Element|
1352123517287|1342010987399|Lazy items (pass 3.1)|Sdk.Razor.CurrentVersion.props|78|`<Content Include="**\*.cshtml" ExcludeFromSingleFile="true" CopyToPublishDirectory="PreserveNewest" ...`|2|0.1%|1|0%|5|Element|
1347788915256|1347637872002|Items (pass 3)|Microsoft.NETCoreSdk.BundledVersions.props|813|`<WindowsSdkSupportedTargetPlatformVersion Include="10.0.22621.1" WindowsSdkPackageVersion="10.0.2262...`|1|0%|1|0%|19|Element|
1346684196557|1342001157632|Items (pass 3)|Directory.Packages.props|5|`<ItemGroup><PackageVersion Include="Ardalis.GuardClauses" Version="5.0.0" /><PackageVersion Include=...`|3|0.1%|1|0%|7|Element|
1342382906696|1342366521501|Properties (pass 1)|Sdk.props|41|`<CustomAfterDirectoryBuildProps >$(CustomAfterDirectoryBuildProps);$(MSBuildThisFileDirectory)UseArt...`|1|0%|1|0%|9|Element|
1343136733766|1342871231399|Properties (pass 1)|Microsoft.NET.Sdk.props|166|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.Sdk.SourceLink.props"  />`|9|0.3%|1|0%|9|Element|
1341771806622|1341768530321|Properties (pass 1)|Piper.slnx.metaproj|0|`<Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\SolutionFile\ImportBefore\*" Condit...`|5|0.2%|1|0%|0|Element|
1347009163331|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|1386|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.PreserveCompilationContext.targets"  />`|2|0.1%|1|0%|9|Element|
1347166737107|1347069893799|Properties (pass 1)|Microsoft.NET.Sdk.Analyzers.targets|117|`<Import Project="$(MSBuildThisFileDirectory)..\codestyle\cs\build\Microsoft.CodeAnalysis.CSharp.Code...`|1|0%|1|0%|9|Element|
1351719010755|1342010987399|Lazy items (pass 3.1)|Piper.Host.Photino.csproj|20|`<Content Update="Graphs\*"><CopyToOutputDirectory>Always</CopyToOutputDirectory></Content>`|1|0%|1|0%|1|Element|
1344069477827|1343280968274|Properties (pass 1)|Microsoft.NET.Sdk.BeforeCommon.targets|85|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.EolTargetFrameworks.targets"  />`|2|0.1%|1|0%|9|Element|
1342756516304|1342725380187|Properties (pass 1)|Sdk.StaticWebAssets.CurrentVersion.props|33|`<Import Project="$(MSBuildThisFileDirectory)..\Sdk\Sdk.StaticWebAssets.StaticAssets.ProjectSystem.pr...`|2|0.1%|1|0%|5|Element|
1341820951648|1341768530321|Properties (pass 1)|Piper.Host.Photino.csproj|||211|7%|1|0%|1|Element|
1344448241366|1344274429827|Properties (pass 1)|Microsoft.Common.targets|||220|7.3%|1|0%|9|Element|
1341822589831|1341768530321|Properties (pass 1)|Piper.UI.csproj|||280|9.3%|1|0%|1|Element|
1348830030431|1342010987399|Lazy items (pass 3.1)|NuGet.targets|148|`<PackageReference Include="@(GlobalPackageReference)" Version="" IncludeAssets="Runtime;Build;Native...`|1|0%|1|0%|9|Element|
1347084666312|1347037066379|Properties (pass 1)|Microsoft.NET.Sdk.StaticWebAssets.targets|777|`<Import Project="Microsoft.NET.Sdk.StaticWebAssets.Pack.targets" />`|2|0.1%|1|0%|5|Element|
1343866180881|1343580935286|Properties (pass 1)|WorkloadManifest.targets|||2|0.1%|1|0%|9|Element|
1347286565001|1347271791411|Properties (pass 1)|NuGet.Build.Tasks.Pack.targets|28|`<PropertyGroup ><PackageId Condition=" '$(PackageId)' == '' ">$(AssemblyName)</PackageId><PackageVer...`|3|0.1%|1|0%|9|Element|
1347069893799|1347066611036|Properties (pass 1)|Microsoft.NET.Sdk.Analyzers.targets|||8|0.3%|1|0%|9|Element|
1349464092366|1342010987399|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.targets|117|`<Content Update="@(Content)" ><LinkBase Condition="'%(LinkBase)' != ''">$([MSBuild]::EnsureTrailingS...`|6|0.2%|1|0%|9|Element|
1346966488686|1346938586399|Properties (pass 1)|Microsoft.NET.Publish.targets|30|`<_FirstTargetFrameworkVersionToSupportTrimAnalyzer >$([MSBuild]::GetTargetFrameworkVersion('$(_First...`|1|0%|1|0%|9|Element|
1349133901626|1349132258987|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.props|36|`Glob="root: '$/home/marco/workspace/flyingpie/piper_1/src/10-Core/Piper.UnitTest', pattern: '$**/*.c...`|1|0%|1|0%|0|Glob|
1352118583916|1342010987399|Lazy items (pass 3.1)|Sdk.StaticWebAssets.StaticAssets.ProjectSystem.props|44|`<Content Update="$(AppDesignerFolder)\**" CopyToPublishDirectory="Never" Condition="'$(AppDesignerFo...`|1|0%|1|0%|11|Element|
1343223601349|1342871231399|Properties (pass 1)|Microsoft.NET.Sdk.props|173|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.PackProjectTool.props"  />`|2|0.1%|1|0%|9|Element|
1346723584352|1346648091521|Properties (pass 1)|Microsoft.NET.Sdk.SourceLink.targets|31|`<Import Project="$(MSBuildThisFileDirectory)..\..\Microsoft.SourceLink.AzureRepos.Git\build\Microsof...`|2|0.1%|1|0%|9|Element|
1341824228015|1341768530321|Properties (pass 1)|Piper.csproj|||159|5.3%|1|0%|1|Element|
1341824228012|1341820951647|Properties (pass 1)|Sdk.props|15|`<Import Project="..\Targets\Sdk.Server.props" Condition="'$([MSBuild]::GetTargetPlatformIdentifier($...`|73|2.4%|1|0%|1|Element|
1342477942799|1342394376392|Properties (pass 1)|Microsoft.Common.props|36|`<Import Project="$(CustomAfterDirectoryBuildProps)" Condition="'$(CustomAfterDirectoryBuildProps)' !...`|5|0.2%|1|0%|9|Element|
1349135544266|1342010987399|Lazy items (pass 3.1)|Microsoft.NET.Sdk.DefaultItems.targets|87|`<Compile Update="@(Compile)" ><!----><LinkBase Condition="'%(LinkBase)' != ''">$([MSBuild]::EnsureTr...`|12|0.4%|1|0%|1|Element|
1343149845674|1343136733766|Properties (pass 1)|Microsoft.NET.Sdk.SourceLink.props|23|`<Import Project="$(MSBuildThisFileDirectory)..\..\Microsoft.Build.Tasks.Git\build\Microsoft.Build.Ta...`|2|0.1%|1|0%|9|Element|
1343338336424|1343280968274|Properties (pass 1)|Microsoft.NET.Sdk.BeforeCommon.targets|47|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.Sdk.ImportPublishProfile.targets" Conditio...`|4|0.1%|1|0%|9|Element|
1346648091521|1346170566341|Properties (pass 1)|Microsoft.NET.Sdk.targets|1373|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.Sdk.SourceLink.targets" Condition="'$(Supp...`|15|0.5%|1|0%|9|Element|
1343130177836|1342871231399|Properties (pass 1)|Microsoft.NET.Sdk.props|161|`<Import Project="Microsoft.NET.SupportedPlatforms.props"  />`|1|0%|1|0%|9|Element|
1348856309831|1342010987399|Lazy items (pass 3.1)|Microsoft.AspNetCore.StaticWebAssets.props|3|`<StaticWebAsset Include="$([System.IO.Path]::GetFullPath('$(MSBuildThisFileDirectory)..\staticwebass...`|1|0%|1|0%|8|Element|
1346739996117|1346648091521|Properties (pass 1)|Microsoft.NET.Sdk.SourceLink.targets|32|`<Import Project="$(MSBuildThisFileDirectory)..\..\Microsoft.SourceLink.Bitbucket.Git\build\Microsoft...`|2|0.1%|1|0%|9|Element|
1352130095129|1342010987399|Lazy items (pass 3.1)|Microsoft.NET.Sdk.Web.DefaultItems.props|34|`<Content Remove="@(_WebToolingArtifacts)" />`|1|0%|1|0%|1|Element|
1343297359049|1343280968274|Properties (pass 1)|Microsoft.NET.Sdk.BeforeCommon.targets|32|`<Import Project="$(MSBuildThisFileDirectory)Microsoft.NET.DefaultAssemblyInfo.targets" Condition="'$...`|3|0.1%|1|0%|9|Element|
1343266216662|1341768530321|Properties (pass 1)|Sdk.targets|||699|23.2%|1|0%|9|Element|
