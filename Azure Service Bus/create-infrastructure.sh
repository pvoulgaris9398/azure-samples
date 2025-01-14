##########################################################################################
# Following commands should be executed in bash. 
# Use CloudShell or install Azure CLI  http://aka.ms/azcli locally
# Provisioned resources will be used for next step. 
# Save connection string from output for next step
##########################################################################################

rg=az204rg
location=eastus
account=sb4588212

# create resource group
az group create -l $location -n $rg

# create a service bus namespace 
az servicebus namespace create --name $account --resource-group $rg

# create a service bus simple queue 
az servicebus queue create --name "simple-queue" --namespace-name $account --resource-group $rg 

# create a service bus session queue 
az servicebus queue create --name "advanced-queue" --namespace-name $account --resource-group $rg --enable-partitioning --enable-session --enable-dead-lettering-on-message-expiration

# create a service bus topic 
az servicebus topic create --name "booking" --namespace-name $account --resource-group $rg

# create subscription for flight-booking
az servicebus topic subscription create --name "flight-booking" --topic-name "booking" --namespace-name $account --resource-group $rg

# create subscription for hotel-booking
az servicebus topic subscription create --name "hotel-booking" --topic-name "booking" --namespace-name $account --resource-group $rg

# create authorization SAS
az servicebus namespace authorization-rule create --namespace-name $account --name manage --rights Manage Send Listen --resource-group $rg

# list connection string
echo 'your queue connection string:'
az servicebus namespace authorization-rule keys list --name manage --namespace-name $account --resource-group $rg --query primaryConnectionString -o tsv

# do not delete provisioned resources, it will be required for next step