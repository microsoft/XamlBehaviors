$scriptPath = (split-path -parent $MyInvocation.MyCommand.Definition) + "\"

$pdbcopyPath = ${Env:ProgramFiles(x86)} + "\Windows Kits\10\Debuggers\x64\pdbcopy.exe"

& $pdbcopyPath "$scriptPath..\out\BehaviorsSDKNative\bin\Win32\Release\Microsoft.Xaml.Interactions\Microsoft.Xaml.Interactions.pdb" "$scriptPath..\out\BehaviorsSDKNative\bin\Win32\Release\Microsoft.Xaml.Interactions\Microsoft.Xaml.Interactions_stripped.pdb" -p | Out-String
& $pdbcopyPath "$scriptPath..\out\BehaviorsSDKNative\bin\x64\Release\Microsoft.Xaml.Interactions\Microsoft.Xaml.Interactions.pdb" "$scriptPath..\out\BehaviorsSDKNative\bin\x64\Release\Microsoft.Xaml.Interactions\Microsoft.Xaml.Interactions_stripped.pdb" -p | Out-String
& $pdbcopyPath "$scriptPath..\out\BehaviorsSDKNative\bin\ARM\Release\Microsoft.Xaml.Interactions\Microsoft.Xaml.Interactions.pdb" "$scriptPath..\out\BehaviorsSDKNative\bin\ARM\Release\Microsoft.Xaml.Interactions\Microsoft.Xaml.Interactions_stripped.pdb" -p | Out-String
& $pdbcopyPath "$scriptPath..\out\BehaviorsSDKNative\bin\ARM64\Release\Microsoft.Xaml.Interactions\Microsoft.Xaml.Interactions.pdb" "$scriptPath..\out\BehaviorsSDKNative\bin\ARM64\Release\Microsoft.Xaml.Interactions\Microsoft.Xaml.Interactions_stripped.pdb" -p | Out-String

& $pdbcopyPath "$scriptPath..\out\BehaviorsSDKNative\bin\Win32\Release\Microsoft.Xaml.Interactivity\Microsoft.Xaml.Interactivity.pdb" "$scriptPath..\out\BehaviorsSDKNative\bin\Win32\Release\Microsoft.Xaml.Interactivity\Microsoft.Xaml.Interactivity_stripped.pdb" -p | Out-String
& $pdbcopyPath "$scriptPath..\out\BehaviorsSDKNative\bin\x64\Release\Microsoft.Xaml.Interactivity\Microsoft.Xaml.Interactivity.pdb" "$scriptPath..\out\BehaviorsSDKNative\bin\x64\Release\Microsoft.Xaml.Interactivity\Microsoft.Xaml.Interactivity_stripped.pdb" -p | Out-String
& $pdbcopyPath "$scriptPath..\out\BehaviorsSDKNative\bin\ARM\Release\Microsoft.Xaml.Interactivity\Microsoft.Xaml.Interactivity.pdb" "$scriptPath..\out\BehaviorsSDKNative\bin\ARM\Release\Microsoft.Xaml.Interactivity\Microsoft.Xaml.Interactivity_stripped.pdb" -p | Out-String
& $pdbcopyPath "$scriptPath..\out\BehaviorsSDKNative\bin\ARM64\Release\Microsoft.Xaml.Interactivity\Microsoft.Xaml.Interactivity.pdb" "$scriptPath..\out\BehaviorsSDKNative\bin\ARM64\Release\Microsoft.Xaml.Interactivity\Microsoft.Xaml.Interactivity_stripped.pdb" -p | Out-String