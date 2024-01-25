run-tests: test test-coverage test-coverage-report

test:
	dotnet test

test-coverage:
	dotnet dotnet-coverage collect -s coverage-settings.xml -f cobertura dotnet test
	dotnet reportgenerator -reports:"output.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

test-coverage-report:
	python3 -m http.server -d coveragereport/

test-coverage-report-stop:
	pkill -f "python3 -m http.server"