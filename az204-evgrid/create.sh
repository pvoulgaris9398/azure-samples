#!/bin/bash

source .env

let rNum=$RANDOM*$RANDOM

# Check variables
[[ -z "${RESOURCE_GROUP_NAME}" ]] && echo "'RESOURCE_GROUP_NAME' not provided!" && exit 1

[[ -z "${LOCATION_NAME}" ]] && echo "'LOCATION_NAME' not provided!" && exit 1

[[ -z "${TOPIC_NAME}" ]] && echo "'TOPIC_NAME' not provided!" && exit 1

[[ -z "${SITE_NAME}" ]] && echo "'SITE_NAME' not provided!" && exit 1

[[ -z "${SITE_URL}" ]] && echo "'SITE_URL' not provided!" && exit 1

echo $RESOURCE_GROUP_NAME
echo $LOCATION_NAME
echo $TOPIC_NAME
echo $SITE_NAME
echo $SITE_URL

 # Create the resource group
if [ $(az group exists -n $RESOURCE_GROUP_NAME) = false ]; then
    echo "Creating resource group: '$RESOURCE_GROUP_NAME'..."
    az group create -l $REGION -n $RESOURCE_GROUP_NAME
else
    echo "Resource Group: '$RESOURCE_GROUP_NAME' already exists..."
fi

#az group create --name az204-evgrid-rg --location $myLocation

az eventgrid topic create --name $myTopicName \
	--location $myLocation \
	--resource-group az204-evgrid-rg

az deployment group create \
	--resource-group az204-evgrid-rg \
	--template-uri "https://raw.githubusercontent.com/Azure-Samples/azure-event-grid-viewer/main/azuredeploy.json" \
	--parameters siteName=$mySiteName hostingPlanName=viewerhost

echo "Your web app URL: ${mySiteURL}"
	

