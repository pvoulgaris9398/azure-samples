#!/bin/bash

source .env

# Check variables
[[ -z "${RESOURCE_GROUP_NAME}" ]] && echo "'RESOURCE_GROUP_NAME' not provided!" && exit 1
[[ -z "${STORAGE_ACCOUNT_NAME}" ]] && echo "'STORAGE_ACCOUNT_NAME' not provided!" && exit 1
[[ -z "${COSMOS_ACCOUNT_NAME}" ]] && echo "'COSMOS_ACCOUNT_NAME' not provided!" && exit 1
[[ -z "${COSMOS_DATABASE_NAME}" ]] && echo "'COSMOS_DATABASE_NAME' not provided!" && exit 1
[[ -z "${COSMOS_CONTAINER_NAME}" ]] && echo "'COSMOS_CONTAINER_NAME' not provided!" && exit 1
[[ -z "${COSMOS_PARTITION_KEY_PATH}" ]] && echo "'COSMOS_PARTITION_KEY_PATH' not provided!" && exit 1

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

# Returns nothing if the cosmosdb account doesn't exist
cosmosdb_account=$(az cosmosdb show --resource-group $RESOURCE_GROUP_NAME --name $COSMOS_ACCOUNT_NAME 2>nul)
if [ -z "$cosmosdb_account" ]; then
    echo "Creating cosmosdb account..."

    # Create cosmosdb account
    az cosmosdb create --name $COSMOS_ACCOUNT_NAME --resource-group $RESOURCE_GROUP_NAME

else
    echo "Cosmosdb Account: '$COSMOS_ACCOUNT_NAME' already exists..."
fi

# Returns nothing if the cosmosdb database doesn't exist
cosmosdb_database=$(az cosmosdb database show --db-name $COSMOS_DATABASE_NAME --resource-group $RESOURCE_GROUP_NAME --name $COSMOS_ACCOUNT_NAME 2>nul)
if [ -z "$cosmosdb_database" ]; then
    echo "Creating cosmosdb database..."

    # Create cosmosdb database
    az cosmosdb sql database create --account-name $COSMOS_ACCOUNT_NAME --resource-group $RESOURCE_GROUP_NAME --name $COSMOS_DATABASE_NAME

    # Create cosmosdb container (analagous to table)
    az cosmosdb sql container create -g $RESOURCE_GROUP_NAME -a $COSMOS_ACCOUNT_NAME -d $COSMOS_DATABASE_NAME -n $COSMOS_CONTAINER_NAME \
    --partition-key-path $COSMOS_PARTITION_KEY_PATH --throughput "400"

else
    echo "Cosmosdb Database: '$COSMOS_DATABASE_NAME' already exists..."
fi
