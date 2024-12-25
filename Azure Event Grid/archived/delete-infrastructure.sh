#!/bin/bash

source .env

# Check variables
[[ -z "${RESOURCE_GROUP_NAME}" ]] && echo "'RESOURCE_GROUP_NAME' not provided!" && exit 1

 # Delete the resource group
if [ $(az group exists -n $RESOURCE_GROUP_NAME) = true ]; then
    echo "deleting resource group: '$RESOURCE_GROUP_NAME'..."
    az group delete -n $RESOURCE_GROUP_NAME --no-wait -y
else
    echo "Resource Group: '$RESOURCE_GROUP_NAME' does not exist..."
    az group list
fi