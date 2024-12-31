rg=az204rg
appName=aidemo97888212
region=eastus

# Create Resource Group
az group create -l $region -n $rg

#-------------------------------
# Create Web App 
#-------------------------------
az appservice plan create -n $appName-plan -g $rg  --sku B1
az webapp create  -p $appName-plan -n $appName -g $rg --runtime 'dotnet:7'

#-------------------------------
# Create Azure SQL
#-------------------------------
az sql server create -n $appName-sql -u myadminuser -p myadmin@Password -g $rg
az sql db create -s $appName-sql -n $appName-db --service-objective Basic -g $rg 

#open firewall to connect from services
az sql server firewall-rule create -g $rg --server $appName-sql -n 'allowed to connect by Azure resources' --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0

#retrieve the connection string
sqlstring=$(az sql db show-connection-string -s $appName-sql -n $appName-db  -c ado.net  -o tsv)
#update user and pwd
sqlstring=${sqlstring/<username>/myadminuser}
sqlstring=${sqlstring/<password>/myadmin@Password}
echo $sqlstring
#-------------------------------
# Create Storage Account
#-------------------------------
az storage account create --name $appName --resource-group $rg  
#connection string, please copy
blobstring=$(az storage account  show-connection-string --name $appName  -o tsv)
echo $blobstring
#---------------------
#create Variable
#--------------------
az webapp config appsettings set -n $appName --settings SqlConnection="$sqlstring" -g $rg
az webapp config appsettings set -n $appName --settings BlobConnection="$blobstring" -g $rg

# Generate URL for open web app.
url=$(az webapp config hostname list --webapp-name $appName --resource-group $rg  --query [0].name -o tsv)
echo "your web app name: $appName"
echo "visit web app page: https://${url}"

