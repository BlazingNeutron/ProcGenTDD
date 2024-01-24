run-tests: test test-coverage test-coverage-report

test:
	dotnet test

test-coverage:
	dotnet-coverage collect -s coverage-settings.xml -f cobertura dotnet test
	reportgenerator -reports:"output.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

test-coverage-report:
	python3 -m http.server &
	xdg-open http://localhost:8000/coveragereport/

test-coverage-report-stop:
	pkill -f "python3 -m http.server"