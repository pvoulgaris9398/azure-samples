# bash
rgName=az204-evhub-rg
location=eastus
namespace=demos776279
eventhubName=demoshub776279

az eventhubs eventhub create --name $eventhubName --resource-group $rgName --namespace-name $namespace
