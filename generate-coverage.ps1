# Script para gerar relatório de cobertura de testes
Write-Host "🧪 Executando testes com cobertura..." -ForegroundColor Green

# Executar testes unitários
dotnet test Softplan.API.UnitTests/Softplan.API.UnitTests.csproj --collect:"XPlat Code Coverage" --results-directory ./coverage --verbosity quiet

# Executar testes de integração
dotnet test Softplan.API.IntegrationTests/Softplan.API.IntegrationTests.csproj --collect:"XPlat Code Coverage" --results-directory ./coverage --verbosity quiet

# Gerar relatório HTML
Write-Host "📊 Gerando relatório de cobertura..." -ForegroundColor Blue
reportgenerator -reports:"coverage/**/*.xml" -targetdir:"coverage-report" -reporttypes:Html

Write-Host "✅ Relatório gerado em: coverage-report/index.html" -ForegroundColor Green
Write-Host "🌐 Abra o arquivo no navegador para ver os detalhes" -ForegroundColor Cyan