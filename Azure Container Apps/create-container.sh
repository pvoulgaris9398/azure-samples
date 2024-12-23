#!bash

ACR_NAME=demos919398

az container create \
	--resource-group az204-container-rg \
	--name acr-tasks \
	--image $ACR_NAME.azurecr.io/helloacrtasks:v1 \
	--registry-login-server $ACR_NAME.azurecr.io \
	--registry-username demos919398 \
 	--registry-password /6xG5YYJE+L0q3h3RdOTHHhlwAhCaJuc2B+MeEzaeZ+ACRCMTZVK \
	--ports 80
	--os-type windows

