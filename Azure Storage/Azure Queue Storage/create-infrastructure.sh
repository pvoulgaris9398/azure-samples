##########################################################################################
# Following commands should be executed in bash. 
# Use CloudShell or install Azure CLI  http://aka.ms/azcli locally
# Provisioned resources will be used for next step.
# Save connection string from output for next step.
##########################################################################################

rg=MessagingDemo-RG

# create resource group
az group create -l eastus -n $rg

# to avoid name collisions generate unique name for your account
account=msg$RANDOM

# create a storage account 
az storage account create --name $account --resource-group $rg

# retrieve key
key=$(az storage account keys list --account-name $account --query [0].value -o tsv)

# create storage container by using key
az storage queue create --name demo --account-name $account  --account-key $key

# retrieve storage connection string for next demo code
echo 'your storage account connection string:'
az storage account show-connection-string --name $account --resource-group $rg --query connectionString


# do not delete the provision resources, it will be required for next step


