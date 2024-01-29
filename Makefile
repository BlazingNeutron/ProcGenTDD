build: tests-only tests-coverage
	dotnet build

tests: tests-only tests-coverage tests-report

tests-only:
	dotnet test

tests-coverage: tests-only
	dotnet dotnet-coverage collect -s coverage-settings.xml -f cobertura dotnet test
	dotnet reportgenerator -reports:"output.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

tests-report:
	python3 -m http.server -d coveragereport/

clean:
	rm -rf coveragereport/
	rm output.cobertura.xml
	pkill -f "python3 -m http.server"