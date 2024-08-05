# App Configuration Example

- Working through [this](https://learn.microsoft.com/en-us/azure/azure-app-configuration/quickstart-azure-app-configuration-create?tabs=azure-portal) sample

- Run `terraform init`
- Run `terraform apply`
- Navigate to `Azure` portal, create a setting per the instructions
- Navigate to `App Configuration` &rarr; `Settings` &rarr; `Access Settings`
- Grab a `Connection String`, update the `.env` file
- Run `dotnet run` from root of this sample:

![](2024-08-05-01.png)

- Having trouble getting my `terraform` to automatically create settings, see:

![](2024-08-05-03.png)

- But I can do via `az` command line:

![](2024-08-05-02.png)

```bash
az appconfig kv set --name app-config-dev01 --key TestApp:Settings:Test --value foobarwinp --yes
```

- Needed to add [depends_on](https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/app_configuration_key)

![](2024-08-05-04.png)

- Progress:

![](2024-08-05-05.png)

```text
authorization.RoleAssignmentsClient#Create: Failure responding to request: StatusCode=400 -- Original Error: autorest/azure: Service returned an error. Status=400 Code="InvalidPrincipalId" Message="A valid principal ID must be provided for role assignment."

```