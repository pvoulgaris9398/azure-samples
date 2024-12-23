# bash
rgName=az204-evhub-rg
region=eastus

if [ $(az group exists --name $rgName) = false ]; then
	az group create --name $rgName --location $region
else
	echo "$rgName already exists..."
fi
