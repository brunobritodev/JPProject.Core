get-childitem * -include *.nupkg | remove-item
dotnet build ..\src\JpProject.Core.sln
dotnet test ..\src\JpProject.Core.sln
dotnet pack ..\src\JpProject.Core.sln -o .\ -p:PackageVersion=1.0.1-$([System.DateTime]::Now.ToString('MM-dd-HHmmss'))