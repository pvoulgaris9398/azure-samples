#!/bin/bash

source .env

# Check variables
[[ -z "${STORAGE_ACCOUNT_NAME}" ]] && echo "'STORAGE_ACCOUNT_NAME' not provided!" && exit 1
[[ -z "${STORAGE_ACCOUNT_TABLE_NAME}" ]] && echo "'STORAGE_ACCOUNT_TABLE_NAME' not provided!" && exit 1
[[ -z "${AZURE_TABLE_CONNECTION_STRING}" ]] && echo "'AZURE_TABLE_CONNECTION_STRING' not provided!" && exit 1

# Fetch a storage account key
key=$(az storage account keys list -n $STORAGE_ACCOUNT_NAME --query [0].value -o tsv)

# Example using key to connect #1
az storage entity insert --account-name $STORAGE_ACCOUNT_NAME --account-key $key --entity \
PartitionKey=tester1 RowKey=Caesar IsActive=true IsActive@odata.type=Edm.Boolean \
--if-exists fail --table-name $STORAGE_ACCOUNT_TABLE_NAME

# Example using key to connect #2
az storage entity insert --account-name $STORAGE_ACCOUNT_NAME --account-key $key --entity \
PartitionKey=Rand RowKey=AlThor IsActive=true IsActive@odata.type=Edm.Boolean \
--if-exists fail --table-name $STORAGE_ACCOUNT_TABLE_NAME

# Example using connection string to connect #1
az storage entity insert --connection-string $AZURE_TABLE_CONNECTION_STRING --entity \
PartitionKey=$random RowKey=Foo TotalComp=750000 IsActive=true IsActive@odata.type=Edm.Boolean \
--if-exists fail --table-name $STORAGE_ACCOUNT_TABLE_NAME

# Example using connection string to connect #2
az storage entity insert --connection-string $AZURE_TABLE_CONNECTION_STRING --entity \
PartitionKey=$random RowKey=Bar IsActive=false IsActive@odata.type=Edm.Boolean \
--if-exists fail --table-name $STORAGE_ACCOUNT_TABLE_NAME

# Example using connection string to connect #3
az storage entity insert --connection-string $AZURE_TABLE_CONNECTION_STRING --entity \
PartitionKey=$random RowKey=Baz TotalComp=1500000 IsActive=true IsActive@odata.type=Edm.Boolean \
--if-exists fail --table-name $STORAGE_ACCOUNT_TABLE_NAME
