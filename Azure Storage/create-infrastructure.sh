#!/bin/bash

source .env

rg=az204rg
location=eastus

az group create --name $rg --location $location

az storage account create -g $rg --name $BLOB_ACCOUNT_NAME --location $location --sku Standard_LRS