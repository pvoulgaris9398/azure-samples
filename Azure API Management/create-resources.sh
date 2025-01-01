#!/bin/bash

rg=az204rg
location=eastus
appName=webapp.dev.4588212
account=apim.4588212

# Web app backend short name from previous script execution like apim-backend-XXXXX
webapp='apim-backend-XXXXX'

# Swagger URL from previously provisioned weather service. URL must be ended with /swagger/v1/swagger.json
url='https://apim-backend-XXXX.azurewebsites.net/swagger/v1/swagger.json'

# Adding API #1, if adding process produces an error continue to the next adding API step
az apim api import --service-url https://$webapp.azurewebsites.net/ --display-name weather-api --api-id weather-api --path weather-api --specification-url $url --specification-format OpenApiJson -g $rg -n $account 


#--------------------------------------------------------
# Connecting well-known APIs
# If the script generates an error, proceed to the next step
#--------------------------------------------------------

# Adding API #2, if adding process produces an error continue to the next adding API step
az apim api import  --display-name color-api --api-id color-api --path color-api --specification-url https://markcolorapi.azurewebsites.net/swagger/v1/swagger.json --specification-format OpenApiJson -g $rg -n $account 

# Create Product
az apim product create -g $rg -n $account --product-name "Color Management (free)" --product-id colors --subscription-required true --state published --description "This product to manage colors"

# Adding APIs
az apim product api add -g $rg -n $account --api-id color-api --product-id colors 


# Adding API #3, if adding process produces an error continue to the next adding API step
az apim api import -g $rg -n $account --display-name calc-api --api-id calc-api --path calc-api --specification-url http://calcapi.cloudapp.net/calcapi.json --specification-format Swagger -g $rg -n $account 

# Create Product
az apim product create -g $rg -n $account --product-name "Calculator API" --product-id calculator --subscription-required true --state published --description "This product to test calculator"

# Adding APIs
az apim product api add -g $rg -n $account --api-id calc-api  --product-id calculator 


# Adding API #4, if adding process produce an error continue to the next adding API step
az apim api import -g $rg -n $account --display-name conference-api --api-id conference-api --path conference-api --specification-url https://conferenceapi.azurewebsites.net?format=json --specification-format OpenApiJson -g $rg -n $account 

# Create Product
az apim product create -g $rg -n $account --product-name "Conference API" --product-id conference-api  --subscription-required true --state published --description "This product to list conferences"

# Adding APIs
az apim product api add -g $rg -n $account --api-id conference-api --product-id conference-api  
