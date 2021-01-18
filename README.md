# Socks and Shoes

## How to deploy

1. Run `dotnet publish -c Release /p:GenerateRuntimeConfigurationFiles=true` in the solution.
2. Run `python3 scripts/zip_lambda.py ./Zapatos.Chaussures.Schuhe/Function/bin/Release/net5.0/publish function.zip`.
