rd .\GUI\BuildResults /S /Q
md .\GUI\BuildResults
rd .\GUI\WindowsFormsApp1\Bin\Release  /S /Q

set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v4.0.30319
call "%msBuildDir%\msbuild.exe"  .\GUI\WindowsFormsApp1.sln /p:Configuration=Release /l:FileLogger,Microsoft.Build.Engine;logfile=Manual_MSBuild_ReleaseVersion_LOG.log
set msBuildDir=

XCOPY .\GUI\WindowsFormsApp1\Bin\Release\*.exe .\GUI\BuildResults\
XCOPY .\GUI\WindowsFormsApp1\Bin\Release\*.dll .\GUI\BuildResults\