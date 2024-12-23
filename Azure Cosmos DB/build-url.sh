#!/bin/bash

# Load environment variables
source .env

# Check variables
[[ -z "${STORAGE_ACCOUNT_NAME}" ]] && echo "'STORAGE_ACCOUNT_NAME' not provided!" && exit 1
[[ -z "${STORAGE_ACCOUNT_TABLE_NAME}" ]] && echo "'STORAGE_ACCOUNT_TABLE_NAME' not provided!" && exit 1

# Fetch a storage account key
key=$(az storage account keys list -n $STORAGE_ACCOUNT_NAME --query [0].value -o tsv)

# Generate the SAS
sas=$(az storage table generate-sas --name $STORAGE_ACCOUNT_TABLE_NAME --account-name $STORAGE_ACCOUNT_NAME --account-key $key --permissions r --expiry 2200-01-01)

## Replace the quotes
sas=${sas//\"/}

# Echo the URL
echo https://${STORAGE_ACCOUNT_NAME}.table.core.windows.net/${STORAGE_ACCOUNT_TABLE_NAME}\(\)?$sas\&\$format=json
