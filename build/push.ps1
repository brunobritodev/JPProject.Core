$files = Get-ChildItem -recurse -filter *.nupkg
foreach ($file in $files) {
	nuget add $file.Name -source D:\workspace\local-nuget
}