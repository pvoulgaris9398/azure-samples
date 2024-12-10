#!/bin/bash

source .env

echo "Creating resource group..."

# Create the resource group
az group create -l $region -n $resource

echo "Creating storage account..."

# Create storage account
az storage account create --name $account --resource-group $resource



