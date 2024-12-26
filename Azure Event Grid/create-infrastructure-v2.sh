##########################################################################################
# Following commands should be executed in bash. Azure Cloud shell is preferable. 
# To run locally install Azure CLI  http://aka.ms/azcli locally
##########################################################################################

topicName=custom-events-web2
rg=EventGridDemo-RG

# create a resource group
az group create -l eastus -n EventGridDemo-RG

# create a topic for custom events
az eventgrid topic create --name $topicName --resource-group $rg -l eastus

# Show endpoint for submitting custom events
echo 'your endpoint:'
az eventgrid topic show --name $topicName --resource-group $rg --query "endpoint" -o tsv

# retrieve access key for event publisher
echo 'your access key:'
az eventgrid topic key list --name $topicName --resource-group $rg --query "key1" -o tsv

# generate name for Event Grid Viewer
sitename=handler$RANDOM

# deploy Event Grid Viewer
az deployment group create --resource-group $rg --template-uri "https://raw.githubusercontent.com/Azure-Samples/azure-event-grid-viewer/master/azuredeploy.json" --parameters siteName=$sitename hostingPlanName=viewerhost

# pull the endpoint for Event Grid Viewer
endpoint=https://$sitename.azurewebsites.net/api/updates

# pull azure subscription id
subid=$(az account show --query id -o tsv)

# create a subscription for Event Grid Viewer
# this command works better in CloudShell, but variables need to be defined 
az eventgrid event-subscription create --source-resource-id "/subscriptions/$subid/resourceGroups/$rg/providers/Microsoft.EventGrid/topics/$topicName" --name custom-handler-web --endpoint $endpoint

###################################################################
##  START PUBLISHER publisher.exe FROM CURRENT FOLDER            ##
###################################################################

# observe the events on Event Grid Viewer
az webapp browse --name $sitename --resource-group $rg

# cleanup 
# Now all resource group EventGridDemo-RG, EventHubDemo-RG and EventGridMonitoring can be deleted