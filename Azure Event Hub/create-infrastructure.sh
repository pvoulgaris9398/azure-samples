#!/bin/bash

# From Chapter12 of Developing-Solutions-for-Microsoft-Azure-AZ-204-Exam-Guide-2nd-Edition

rg=EventHubDemo-RG

# Create resource group
az group create -l eastus -n $rg

# To avoid name collisions generate a unique name for your account
account=eventhub$RANDOM

# create an Event Hubs namespace. 
az eventhubs namespace create --name $account --resource-group $rg -l eastus --sku Standard

# create an event hub. Specify a name for the event hub. 
az eventhubs eventhub create --name $account --resource-group $rg --namespace-name $account 

# create a storage account 
az storage account create --name $account --resource-group $rg 

# retrieve key
key=$(az storage account keys list --account-name $account --query [0].value -o tsv)

# create a storage container by using key
az storage container create --name checkpoint --account-name $account  --account-key $key

# create authorization string for the event hub
az eventhubs eventhub authorization-rule create --name apps --rights Listen Send --eventhub-name $account --namespace-name $account --resource-group $rg 

# Output the account
echo 'your eventhub account name: '$account

# retrieve eventhub connection string for the next demo code
echo 'your eventhub connection string:'
az eventhubs eventhub authorization-rule keys list --name apps --eventhub-name $account --namespace-name $account --resource-group $rg --query primaryConnectionString -o tsv

# retrieve storage connection string for next demo code
echo 'your storage account connection string:'
az storage account show-connection-string --name $account --resource-group $rg --query connectionString -o tsv

# do not delete the provision resources, it will be required for next step
