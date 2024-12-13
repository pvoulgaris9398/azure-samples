#!/bin/bash

source .env

# Check variables
[[ -z "${RESOURCE_GROUP_NAME}" ]] && echo "'RESOURCE_GROUP_NAME' not provided!" && exit 1
[[ -z "${STORAGE_ACCOUNT_NAME}" ]] && echo "'STORAGE_ACCOUNT_NAME' not provided!" && exit 1

# Create the resource group
if [ $(az group exists -n $RESOURCE_GROUP_NAME) = false ]; then
    echo "Creating resource group: '$RESOURCE_GROUP_NAME'..."
    az group create -l $REGION -n $RESOURCE_GROUP_NAME
else
    echo "Resource Group: '$RESOURCE_GROUP_NAME' already exists..."
fi

# Returns nothing if the storage account doesn't exist
storage_account=$(az storage account show -g $RESOURCE_GROUP_NAME -n $STORAGE_ACCOUNT_NAME 2>nul)
if [ -z "$storage_account" ]; then
    echo "Creating storage account..."

    # Create storage account
    az storage account create --name $STORAGE_ACCOUNT_NAME --resource-group $RESOURCE_GROUP_NAME

else
    echo "Storage Account: '$STORAGE_ACCOUNT_NAME' already exists..."
fi
