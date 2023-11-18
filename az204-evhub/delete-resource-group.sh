# bash
rgName=az204-evhub-rg

if [ $(az group exists --name $rgName) = true ]; then
	az group delete --name $rgName
else
	echo "$rgName is already deleted..."
fi
