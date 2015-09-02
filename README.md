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
