# Instructions

## TODO

- Get this all working

## Run `az login`

## Create `.env` file with the following values:

- `RESOURCE_GROUP_NAME` &rarr; The resource group name
- `REGION` &rarr; The region to use
- `STORAGE_ACCOUNT_NAME` &rarr; The name of the storage account
- `STORAGE_ACCOUNT_TABLE_NAME` &rarr; The name of the table to create
- `AZURE_TABLE_CONNECTION_STRING` &rarr; The connection string to use

## Run `az group create -l $REGION -n $RESOURCE_GROUP_NAME`

## Run `az storage account create --name $STORAGE_ACCOUNT_NAME --resource-group $RESOURCE_GROUP_NAME`

## Run `az storage account show-connection-string -g $RESOURCE_GROUP_NAME -n $STORAGE_ACCOUNT_NAME`

- Note this works, but the order of the arguments is important
- For some reason, using the `-n $STORAGE_ACCOUNT_NAME` alone does not work

![](Screenshot%202024-12-10%20131933.png)

# Settings

`C:\Users\{username}\AppData\Roaming\Code\User`
