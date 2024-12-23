#!/bin/bash

myApiName=az204-apim-$RANDOM
myLocation=eastus
myEmail=petevoulgaris@gmail.com
myGroup=az204-apim-rg

az group create --name $myGroup --location $myLocation

az apim create -n $myApiName \
	--location $myLocation \
	--publisher-email $myEmail \
	--resource-group $myGroup \
	--publisher-name AZ204-APIM-Exercise \
	--sku-name Consumption
