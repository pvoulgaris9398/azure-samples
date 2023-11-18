# bash
let rNum=$RANDOM*$RANDOM
myLocation=eastus
myTopicName="az204-egtopic-$rNum"
mySiteName="az204-egsite-$rNum"
mySiteURL="https://$mySiteName.azurewebsites.net"

echo $rNum
echo $myLocation
echo $myTopicName
echo $mySiteName
echo $mySiteURL

#az group create --name az204-evgrid-rg --location $myLocation

az eventgrid topic create --name $myTopicName \
	--location $myLocation \
	--resource-group az204-evgrid-rg

az deployment group create \
	--resource-group az204-evgrid-rg \
	--template-uri "https://raw.githubusercontent.com/Azure-Samples/azure-event-grid-viewer/main/azuredeploy.json" \
	--parameters siteName=$mySiteName hostingPlanName=viewerhost

echo "Your web app URL: ${mySiteURL}"
	

