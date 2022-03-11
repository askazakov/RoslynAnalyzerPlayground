all: restore_sample
	dotnet restore
	dotnet build

restore_sample: pack
	dotnet restore SampleProject/SampleProject.csproj

build_release:
	dotnet build -p:Configuration=Release ./PlaygroundAnalyzers/PlaygroundAnalyzers.csproj

pack: build_release
	nuget pack PlaygroundAnalyzers/PlaygroundAnalyzers.nuspec -p Configuration=Release -outputDirectory packages

clean:
	dotnet clean
	export pkgs=`dotnet nuget locals global-packages --list | cut -d ':' -f 2 | xargs`; \
	rm -r $${pkgs}playgroundanalyzers
