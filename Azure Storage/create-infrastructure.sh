#!/bin/bash

rg=az204rg
location=eastus
account=storage.dev.4588212

az group create --name $rg --location $location

az storage account create -g $rg --name $account --location $location --sku Standard_LRS