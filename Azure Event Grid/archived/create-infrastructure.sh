#!/bin/bash

source .env

# Check variables
[[ -z "${RESOURCE_GROUP_NAME}" ]] && echo "'RESOURCE_GROUP_NAME' not provided!" && exit 1

[[ -z "${REGION}" ]] && echo "'REGION' not provided!" && exit 1

[[ -z "${TOPIC_NAME}" ]] && echo "'TOPIC_NAME' not provided!" && exit 1

[[ -z "${SITE_NAME}" ]] && echo "'SITE_NAME' not provided!" && exit 1

[[ -z "${TEMPLATE_URI}" ]] && echo "'TEMPLATE_URI' not provided!" && exit 1

[[ -z "${SITE_URL}" ]] && echo "'SITE_URL' not provided!" && exit 1

echo $RESOURCE_GROUP_NAME
echo $REGION
echo $TOPIC_NAME
echo $SITE_NAME
echo $TEMPLATE_URI
echo $SITE_URL

 # Create the resource group
if [ $(az group exists -n $RESOURCE_GROUP_NAME) = false ]; then
    echo "Creating resource group: '$RESOURCE_GROUP_NAME'..."
    az group create -l $REGION -n $RESOURCE_GROUP_NAME
else
    echo "Resource Group: '$RESOURCE_GROUP_NAME' already exists..."
    az group list
fi

# Create the topic
topic=$(az eventgrid topic show -g $RESOURCE_GROUP_NAME -n $TOPIC_NAME 2>nul)
if [ -z "$topic" ]; then

    echo "Creating eventgrid topic: '$TOPIC_NAME' ..."

    az eventgrid topic create --name $TOPIC_NAME \
        --location $REGION \
        --resource-group $RESOURCE_GROUP_NAME
else
    echo "Topic: '$TOPIC_NAME' already exists..."
fi

deployment=$(az deployment group show -g $RESOURCE_GROUP_NAME -n $SITE_NAME 2>nul)
if [ -z "$deployment" ]; then
    
    echo "Creating deployment group '$SITE_NAME'..."

    az deployment group create \
        --resource-group $RESOURCE_GROUP_NAME \
        --template-uri $TEMPLATE_URI \
        --parameters siteName=$SITE_NAME hostingPlanName=viewerhost

else
    echo "Deployment group: '$SITE_NAME' already exists..."
fi;

echo "Your web app URL: ${SITE_URL}"