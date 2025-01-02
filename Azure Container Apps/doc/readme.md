# Azure Container Apps

## `Thursday, 1/2/25`

- There is an excellent Microsof Learn resource [here](https://learn.microsoft.com/en-us/training/modules/run-docker-with-azure-container-instances/)
- It covers a lot of good stuff about setting-up and running containers in `Azure Container Instances`
- But some of the _sample_ images referenced no longer exist
- Nevertheless, although it might take a little more time, \
  I would like to create my own _hello world_ style images to use \
  along with this sample, it would make a great end-to-end tutorial covering many concepts

## Azure Container Apps

### Container Apps Environment

- The highest level resource in ACA is the `Container Apps` environment
- Secure boundary where _container apps_ can be deployed
- All apps in this environment share the same `VNet`
- Can provision your own `VNet` or use one managed by Microsoft
- All apps write to the same _Log Analytics_ workspace and share the same `DAPR` configuration

### Container Apps

- Each container can be configured with secrets and `DAPR` settings
- Can configure _ingress_ settings, whether _ingress_ will be allowed from _anywhere_ or just your `VNet` (not available if `VNet` managed by Microsoft)
- Authentication can be enabled
- Three (3) main components, _containers_, _revisions_, _replicas_.
- Configure _image_ to use, set environment variables, health probes and volume mounts

#### Revisions

- Apps use _revisions_ for _versioning_. Certain changes, such as container configuration and scaling rules are considered _revision-scope_ changes.
- These _revision-scope_ changes will trigger a new _revision_ to be created.
- More than one _revision_ could be running side-by-side with percentage of traffic allocated to each of them
- Similar to _App Service_ deployment slots
- _Revisions_ can run in a single _revision_ configuration. In this case `ACA` will wait until the new _revision_ is ready and then route all traffic to the new _revision_ with zero (0) downtime

#### Replicas

- _Replicas_ are **instances** of a _Revision_. Container apps scale down to zero (0) when idle, by default.
- No _Replicas_ means no paying for them
- If multiple _Revisions_ are active at once, the could be a number of _Replicas_ per _Revision_, depending on scaling rules

### Creating An ACA Environment

- So the `az extension add --name containerapp --upgrade` command worked on my linuxmint development machine.

```bash
az group create -n $RESOURCE_GROUP_NAME -l $REGION

az containerapp env create --name $CONTAINER_NAME --resource-group $RESOURCE_GROUP_NAME --location $REGION

az containerapp compose create --environment $CONTAINER_NAME -g $RESOURCE_GROUP_NAME --location $REGION

```

### Creating and Configuring Container Apps

- `12/29/24 PM` &rarr; Was able to work through this example on my linuxmint development machine, pretty cool

```bash
az containerapp compose create --environment $CONTAINER_NAME -g $RESOURCE_GROUP_NAME --location $REGION
```

### Configuring Health Probes

- Liveness - periodically reports on the health of a replica
- Readiness - signals when a new replica is ready for traffic
- Startup - can be used to delay reporting on a _liveness_ or _readiness_ probe, when the app is slow to start up

### Configurint App Secrets

- Worked through example adding a secret for the `demoapi` container

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
