# Azure Container Apps

## Azure Container Apps

- TODO: Add notes.

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

- Configure default `ACR`

```bash
regname=acr4588212
az configure --defaults acr=$regname
```

- Cancel a running task

```bash
# Note, acr previously set as default
az acr task cancel-run --run-id ca8 --verbose
```

- Query `ACR` tasks:

```bash
# Note, acr previously set as default
az acr task list-runs
```

- Enable `admin` user:

```bash
regname=acr4588212
az acr update -n $regname --admin-enabled true
```

- Get password programmatically

```bash
regname=acr4588212
az acr credential show --query "passwords[0].value"
az acr credential show -n $regname --query "passwords[0].value" --verbose
pwd=$(az acr credential show -n $regname --query "passwords[0].value")
```

- Create `Azure Container Instance`

```bash
rg=az204rg
regname=acr4588212
containername=cont4588212
dns=dns4588212
az container create \
-g $rg \
-n $containername \
--image "$regname.azurecr.io/webapp:v2" \
--cpu 1 \
--memory 1 \
--registry-login-server "$regname.azurecr.io" \
--registry-username $regname \
--registry-password $pwd \
--ports 80 \
--dns-name-label $dns
```

- Getting error:

```text
(InvalidOsType) The 'osType' for container group '<null>' is invalid. The value must be one of 'Windows,Linux'.
Code: InvalidOsType
Message: The 'osType' for container group '<null>' is invalid. The value must be one of 'Windows,Linux'.
```
