# bash

rgName=az204-evhub-rg
location=eastus
namespace=demos776279

az eventhubs namespace create \
	--name $namespace \
	--resource-group $rgName

