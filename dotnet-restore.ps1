$dirs = ".\src"# , ".\test"
foreach ($dir in $dirs)
{
	$subDirs = Get-ChildItem $dir -recurse -Depth 0 -Directory | ForEach-Object {$_.FullName}
	# $subDirs
	foreach ($subDir in $subDirs) {
		# $subDir
		$path = $subDir + "\project.json"
		# $path
		if (test-path $path) {	
			dotnet restore $path
		}
	}
}
pause