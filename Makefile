build: tests-only tests-coverage
	dotnet build

tests: tests-only tests-coverage tests-report

tests-only:
	dotnet test

tests-coverage: tests-only
	dotnet dotnet-coverage collect -s runsettings.xml -f cobertura dotnet test
	dotnet reportgenerator -reports:"output.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

tests-report:
	npx http-server ./coveragereport

clean:
	rm -rf coveragereport/
	rm output.cobertura.xml

trace-report:
	dotnet build
	dotnet dotnet-trace collect -o dotnet.nettrace -- dotnet exec ./procgentest1cli/bin/Debug/net8.0/procgentest1cli.dll
	dotnet dotnet-trace report dotnet.nettrace topN -n 20
	rm dotnet.nettrace dotnet.nettrace.etlx
