#!/bin/bash

redisName=az204redis$RANDOM
az redis create --location eastus \
    --resource-group az204-redis-rg \
    --name $redisName \
    --sku Basic --vm-size c0