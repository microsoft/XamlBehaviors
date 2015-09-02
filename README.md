# **XAML Behaviors**
The Behaviors SDK is an easy-to-use means of adding common and reusable interactivity to your Windows UWP applications with minimal code. It is available for both native and managed applications. Use of the Behaviors SDK is governed by the MIT License

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
    <Button x:Name="button" Content=Click Me">
	    <Interactivity:Interaction.Behaviors>
		    <Core:EventTriggerBehavior EventName="Click"
		    SourceObject="{Binding ElementName=button}"/>
	    </Interactivity:Interaction.Behaviors>
    </Button>
```
Using Behaviors SDK
-------------------
The [documentation](https://github.com/Microsoft/XamlBehaviors/wiki) explains how to install Visual Studio, add the Behaviors SDK NuGet package to your project, and get started using the API.

Building Behaviors from Source
------------------------------
**What You Need**

 - Visual Studio 2015 (Recommended)
 - 
