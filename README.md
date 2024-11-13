# XAML Behaviors

XAML Behaviors is an easy-to-use means of adding common and reusable interactivity to your Windows UWP applications with minimal code. It is available for managed applications only. Use of XAML Behaviors is governed by the MIT License

## Build Status

| Platform | Status |
| -------- | ------ |
| Managed | ![Build Managed](https://github.com/microsoft/XamlBehaviors/workflows/Build%20Managed/badge.svg) |

## Getting Started

### Where to get it

- NuGet package for [Managed](https://www.nuget.org/packages/Microsoft.Xaml.Behaviors.Uwp.Managed/)
- [Source Code](https://github.com/Microsoft/XamlBehaviors)

### Resources

- [Documentation](https://github.com/Microsoft/XamlBehaviors/wiki)
- [Samples](/samples)
- [Changelog](https://github.com/Microsoft/XamlBehaviors/wiki/Changelog)
- [![Join the chat at https://gitter.im/Microsoft/XamlBehaviors](https://badges.gitter.im/Microsoft/XamlBehaviors.svg)](https://gitter.im/Microsoft/XamlBehaviors?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

### More Info

- [Report a bug or ask a question](https://github.com/Microsoft/XamlBehaviors/issues)
- [Contribute](https://github.com/Microsoft/XamlBehaviors/wiki/Contribute-to-XAML-Behaviors)
- [License](http://opensource.org/licenses/MIT)

### Code Example

For an example of using Behaviors in an application, here is a snippet of XAML:

```xml
<Button xmlns:Core="using:Microsoft.Xaml.Interactions.Core" xmlns:Interactivity="using:Microsoft.Xaml.Interactivity">
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

### Using Behaviors SDK

The [documentation](https://github.com/Microsoft/XamlBehaviors/wiki) explains how to install Visual Studio, add the XAML Behaviors NuGet package to your project, and get started using the API.

### Building Behaviors from Source

#### What You Need

- [Visual Studio 2022 17.12+ w/ Universal Windows Tools](https://visualstudio.microsoft.com/vs/features/universal-windows-platform/)
- [Multilingual App Toolkit](https://developer.microsoft.com/en-us/windows/develop/multilingual-app-toolkit)

#### Clone the Repository

- Go to 'View' -> 'Team Explorer' -> 'Local Git Repositories' -> 'Clone'
- Add the XAML Behaviors repository URL (https://github.com/Microsoft/XamlBehaviors) and hit 'Clone'

#### Build and Create Managed XAML Behaviors NuGet

- Open the "BehaviorsSDKManaged.sln" solution in Visual Studio
- Change Build Configuration to Release
- Build [Ctrl + B]
- Ensure that [nuget.exe](https://learn.microsoft.com/en-us/nuget/install-nuget-client-tools?tabs=windows) is available in PATH
- Run `msbuild /t:pack src\BehaviorsSDKManaged\Microsoft.Xaml.Interactions.Design\Microsoft.Xaml.Interactions.Design.csproj`
  - *(Optional)* Add `/p:TimestampPackage=true` to include the timestamp in the NuGet package version

For WinUI:

- Run `msbuild /t:Pack src\BehaviorsSDKManaged\Microsoft.Xaml.Interactivity.WinUI\Microsoft.Xaml.Interactivity.WinUI.csproj`
  - *(Optional)* Add `/p:TimestampPackage=true` to include the timestamp in the NuGet package version
