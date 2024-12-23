#!/bin/bash
ACR_NAME=demos919398
RESOURCE_GROUP=az204-container-rg
LOCATION=eastus

if [ $(az group exists --name $RESOURCE_GROUP) = false ]; then
	az group create --name $RESOURCE_GROUP --location $LOCATION
else
	echo "$RESOURCE_GROUP already exists..."
fi

az acr create --resource-group $RESOURCE_GROUP --name $ACR_NAME --sku Premium
