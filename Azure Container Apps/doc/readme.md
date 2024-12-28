# Azure Container Apps

## Azure Container Registry

- Create a `Resource Group`:

```bash
rg=az204rg
region=eastus
az group create -n $rg -l $region
```

- Create the `Azure Container Registry`:

```bash
rg=az204rg
region=eastus
regname=acr4588212
az acr create [--resource-group|-g] $rg -n $regname --sku Basic
```

- Query repositories:

```bash
regname=acr4588212
az acr repository list --name $regname -o tsv
```

- Pull and run an image:

```bash
regname=acr4588212
docker run -it --rm -p 81:80 $regname=.azurecr.io/webapp:v1
```

## Azure ACR Tasks

- Run an `ACR` build task

```bash
regname=acr4588212
az acr build --image webapp:v2 --registry $regname .
```

- Confirm success:

```bash
regname=acr4588212
az acr repository show-tags --name $regname --repository webapp -o tsv
```

- Run from the registry:

```bash
regname=acr4588212
az acr run --registry $regname --cmd '$Registry/webapp:v2' /dev/null
```
