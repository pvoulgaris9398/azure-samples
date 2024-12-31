rg=az204rg
appName=aidemo97888212
region=eastus

# Install CLI extension to provision App Insights
az extension add -n application-insights

# Create Log Analytics workspace
az monitor log-analytics workspace create -g $rg -n $appName-ai

# Create Application Insights, the resource required a minute to be provisioned
az monitor app-insights component create --app $appName-ai --location eastus --resource-group $rg --kind web --workspace $appName-ai


# Return the App Insides key for future use.
instrumentation=$(az monitor app-insights component show --app $appName-ai -g $rg --query  "connectionString" --output tsv)
echo $instrumentation

# Update App insights key for Web App
az webapp config appsettings set -n $appName --settings APPLICATIONINSIGHTS_CONNECTION_STRING=$instrumentation -g $rg
az webapp config appsettings set -n $appName --settings ApplicationInsightsAgent_EXTENSION_VERSION=~2 -g $rg
az webapp config appsettings set -n $appName --settings DiagnosticServices_EXTENSION_VERSION=~3 -g $rg
az webapp config appsettings set -n $appName --settings InstrumentationEngine_EXTENSION_VERSION=~1 -g $rg
az webapp config appsettings set -n $appName --settings SnapshotDebugger_EXTENSION_VERSION=~1 -g $rg
az webapp config appsettings set -n $appName --settings XDT_MicrosoftApplicationInsights_BaseExtensions=~1 -g $rg

# Connect Web App                                          
az monitor app-insights component connect-webapp -g $rg -a $appName-ai --web-app $appName --enable-profiler --enable-snapshot-debugger