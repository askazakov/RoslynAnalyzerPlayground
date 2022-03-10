build_release:
	dotnet build -p:Configuration=Release ./PlaygroundAnalyzers/PlaygroundAnalyzers.csproj

pack: build_release
	nuget pack PlaygroundAnalyzers/PlaygroundAnalyzers.nuspec -p Configuration=Release -outputDirectory packages

restore_sample:
	dotnet restore SampleProject/SampleProject.csproj

# dotnet nuget locals global-packages --list
# rm -r /Users/akazakov/.nuget/packages/playgroundanalyzers
