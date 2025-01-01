#!/bin/bash

rg=az204rg
location=eastus
appName=webapp.dev.4588212
account=apim.4588212

# Create resource group
az group create --name $rg --location $location

# Create App Service Plan
az appservice plan create -n $appName-plan -g $rg -sku B1

# Create Web App
az webapp create -p $appName-plan -n $appName -g $rg --runtime 'dotnet:8.0'

# Enable swagger support
az webapp config appsetings set -n $appName -g $rg --settings ASPNETCORE_ENVIRONMENT=Development

# Provision APIM with consumption tier, might take a little time
az apim create -n $apiName \
--location $location \
--resource-group $rg \
--publisher-name $account \
--publisher-email $account'@demo.com' \
--sku-name Consumption
