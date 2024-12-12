# Instructions

## TODO

- Get this all working

## Run `az login`

## Create `.env` file with the following values:

- `resource` &rarr; The resource group name
- `region` &rarr; The region to use
- `account` &rarr; The name of the storage account
- `table` &rarr; The name of the table to create
- `AZURE_COSMOSDB_CONNECTION_STRING` &rarr; The connection string to use

## Run `az group create -l $region -n $resource`

## Run `az storage account create --name $account --resource-group $resource`

## Run `az storage account show-connection-string -group $resource -bn $account`

- Note this works, but the order of the arguments is important
- For some reason, using the `-n $account` alone does not work

![](Screenshot%202024-12-10%20131933.png)

# Settings

`C:\Users\{username}\AppData\Roaming\Code\User`
