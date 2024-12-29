#!/bin/bash

# Load environment
source .env

# Check variables
[[ -z "${RESOURCE_GROUP_NAME}" ]] && echo "'RESOURCE_GROUP_NAME' not provided!" && exit 1

# Create the resource group
if [ $(az group exists -n $RESOURCE_GROUP_NAME) = false ]; then
    echo "Creating resource group: '$RESOURCE_GROUP_NAME'..."
    az group create -n $RESOURCE_GROUP_NAME -l $REGION 
else
    echo "Resource Group: '$RESOURCE_GROUP_NAME' already exists..."
fi

exists=$(az acr list --query "[?name=='$ACR_NAME']")
if [[ -z $exists=="[]" ]];
then
	echo "Azure Container Registry: '$ACR_NAME' DOES NOT exist..."

	az acr create -g $RESOURCE_GROUP_NAME -n $ACR_NAME --sku Basic

else
	echo "Azure Container Registry: '$ACR_NAME' DOES exist..."
fi

az acr update -n $ACR_NAME --admin-enabled true

PWD=$(az acr credential show -n $ACR_NAME --query "passwords[0].value")

echo '******'

echo $PWD
echo $RESOURCE_GROUP_NAME
echo $CONTAINER_NAME
echo $ACR_NAME

az container create \
	-g $RESOURCE_GROUP_NAME \
	--name $CONTAINER_NAME \
	--image $ACR_NAME.azurecr.io/webapp:v2 \
	--cpu 1 \
	--memory 1 \
	--registry-login-server $ACR_NAME.azurecr.io \
	--registry-username $ACR_NAME \
	--registry-password $PWD \
	--ports 80 \
	--os-type windows

#RED='\033[0;31m'
#NC='\033[0m' # No Color
#echo -e "I ${RED}love${NC} Stack Overflow"

#az acr create --resource-group $RESOURCE_GROUP --name $ACR_NAME --sku Premium


