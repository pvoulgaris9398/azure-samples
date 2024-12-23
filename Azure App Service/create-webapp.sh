#! /bin/bash

az webapp create \
    -n "globally-unique-name" \
    -g "resource-group" \
    --plan "name of plan"