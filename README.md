# **XAML Behaviors**
XAML Behaviors is an easy-to-use means of adding common and reusable interactivity to your Windows UWP applications with minimal code. It is available for both native and managed applications. Use of XAML Behaviors is governed by the MIT License

Getting Started
-------------------
 **Where to get it**

 - NuGet package
 - [Source Code](https://github.com/Microsoft/XamlBehaviors)

**Resources**

 - [Documentation](https://github.com/Microsoft/XamlBehaviors/wiki)
 - [Samples](/samples)

**More Info**

 - [Report a bug or ask a question](https://github.com/Microsoft/XamlBehaviors/issues)
 - [Contribute](https://github.com/Microsoft/XamlBehaviors/blob/master/CONTRIBUTING.md)
 - [License](http://opensource.org/licenses/MIT)
 - [FAQ](https://github.com/Microsoft/XamlBehaviors/blob/master/FAQ.md)

Code Example
------------
For an example of using Behaviors in an application, here is a snippet of XAML:
```xml
<Button>
	<Interactivity:Interaction.Behaviors>
		<Core:EventTriggerBehavior EventName="Click">
			<Core:ChangePropertyAction PropertyName="Background">
				<Core:ChangePropertyAction.Value>
					<SolidColorBrush Color="Red"/>
				</Core:ChangePropertyAction.Value>
			</Core:ChangePropertyAction>
		</Core:EventTriggerBehavior>
	</Interactivity:Interaction.Behaviors>
</Button>
```
Using Behaviors SDK
-------------------
The [documentation](https://github.com/Microsoft/XamlBehaviors/wiki) explains how to install Visual Studio, add the Behaviors SDK NuGet package to your project, and get started using the API.

Building Behaviors from Source
------------------------------
**What You Need**

 - [Visual Studio 2015 w/ Universal Windows Tools](https://www.visualstudio.com/features/windows-apps-games-vs)
 
**Build and Create Managed XAML Behaviors NuGet**
 
 - [Clone the Repository](https://github.com/Microsoft/XamlBehaviors)
 - Open the solution in Visual Studio
 - Change Build Configuration to Release
 - Build [Shift + B]
 - Run scripts/CreateManagedNuGet.cmd 
 
**Build and Create Native XAML Behaviors NuGet**
 
 - [Clone the Repository](https://github.com/Microsoft/XamlBehaviors)
 - Open the solution in Visual Studio
 - [Batch Build](https://msdn.microsoft.com/en-us/library/169az28z(v=vs.90).aspx) for x86, x64, and ARM
 - Run scripts/CreateNativeNuGet.cmd 

Symbols
-------

Note that due to the presence of the -Symbols switch in the nuget pack call embedded in the Create...NuGet.cmd scripts, a second package will be generated called <package name>.symbols.nuget. When uploading your NuGet package to nuget.org, nuget.exe will automatically detect this symbols package file (if it's in the same directory as the dll package) and upload it, not to nuget.org but to symbolsource.org (you can change which symbolsource server endpoint is used using the -Source switch, but this is the default). Note that this does not appear to happen when uploading your nuget package via the nuget.org website. Also note that while you can only upload a given version of a nuget.org package once, you can upload a symbol package for a version as many times as you like. To upload symbols for a nuget package already in nuget.org, you can specify the symbol package explicitly in a nuget push <package> command.

Examples:
nuget push Microsoft.Xaml.Behaviors.Uwp.Managed.1.0.0.nupkg              // upload the package, and Microsoft.Xaml.Behaviors.Uwp.Managed.1.0.0.symbols.nupkg if present
nuget push Microsoft.Xaml.Behaviors.Uwp.Managed.1.0.0.symbols.nupkg      // upload just the symbols for Microsoft.Xaml.Behaviors.Uwp.Managed.1.0.0.nupkg

In VS, setting http://srv.symbolsource.org/pdb/Public as a symbol file location will enable lookups to this package. You will have symbols and source in your behavior debugging sessions. Full details at http://docs.nuget.org/Create/Creating-and-Publishing-a-Symbol-Package and http://www.symbolsource.org/Public/Home/VisualStudio.

Versioning
----------

Version files are all collected together into the source trees for the two SDKs, in src\<SDK>\Version. When revising a version for either SDK, be sure to change it in all files represented here before rebuilding and re-packaging. Keeping the NuGet package version in sync with the assemblies in that package will help avoid confusion.