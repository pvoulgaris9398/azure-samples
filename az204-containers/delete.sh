#!bash

RESOURCE_GROUP=az204-container-rg

if [ $(az group exists --name $RESOURCE_GROUP) = true ]; then
	az group delete --name $RESOURCE_GROUP --yes
else
	echo "$RESOURCE_GROUP is already deleted..."
fi
