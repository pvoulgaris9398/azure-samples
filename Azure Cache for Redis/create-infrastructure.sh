 #!/bin/bash

rg=az204rg
location=eastus
account=azurecache9784588212

# Create the resource group
if [ $(az group exists -n $rg) = false ]; then
    echo "Creating resource group: '$rg'..."
    az group create -n $rg -l $location 
else
    echo "Resource Group: '$rg' already exists..."
fi

# Create a Basic SKU instance
az redis create -l $location eastus2 --name $account -g $rg --sku Basic --vm-size C0

# Retrieve key and address 
key=$(az redis list-keys --name $account -g $rg --query primaryKey -o tsv)

echo $key
echo $account.redis.cache.windows.net