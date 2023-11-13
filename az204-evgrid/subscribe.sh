#bash

endpoint=https://az204-egsite-381807886.azurewebsites.net/api/updates

subId=63e4942f-0a3a-412a-8faf-424cf389be74

echo $subId

myTopicName="az204-egtopic-381807886"

az eventgrid event-subscription create \
    --source-resource-id "/subscriptions/$subId/resourceGroups/az204-evgrid-rg/providers/Microsoft.EventGrid/topics/$myTopicName" \
    --name az204ViewerSub \
    --endpoint $endpoint
