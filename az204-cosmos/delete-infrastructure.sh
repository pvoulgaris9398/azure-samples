#!/bin/bash

source .env

echo "Deleting resource group..."

az group delete -g $resource
