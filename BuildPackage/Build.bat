
Call nuget.exe restore ..\Diplo.DictionaryEditor.sln
Call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\bin\MSBuild.exe" Package.build.xml /p:Configuration=Release
rem Call "C:\Program Files (x86)\MSBuild\12.0\Bin\MsBuild.exe" Package.build.xml /p:Configuration=Release