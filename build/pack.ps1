param([string] $v)

if (!$v)
{
    $version = '3.0.2-prerelease1.' + $([System.DateTime]::Now.ToString('MM-dd-HHmmss'))
}
else{
	$version = $v
}
Write-Host 'Version: ' $version 
get-childitem * -include *.nupkg | remove-item
dotnet build ..\src\JpProject.Core.sln
dotnet test ..\src\JpProject.Core.sln
dotnet pack ..\src\JpProject.Core.sln -o .\ -p:PackageVersion=$version