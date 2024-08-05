# Configure the Azure provider
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.0.2"
    }
  }

  required_version = ">= 1.1.0"
#   cloud {
#     organization = "pete-tutorials-org"
#     workspaces { name = "terraform" }
#   }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location

  tags = {
    Description = "Terraform getting started"
    Environment = "Development"
    Team        = "DevOps"
  }
}

resource "azurerm_app_configuration" "appconf" {
  name                = "app-config-dev01"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
}

# DO I NEED THIS???
# Yes.
data "azurerm_client_config" "current" {}

# DO I NEED THIS???
# Yes
resource "azurerm_role_assignment" "appconf_dataowner" {
  scope                = azurerm_app_configuration.appconf.id
  role_definition_name = "App Configuration Data Owner"
  principal_id         = data.azurerm_client_config.current.client_id
}

resource "azurerm_app_configuration_key" "example" {
  configuration_store_id = azurerm_app_configuration.appconf.id
  key = "AppConfigExample:Settings:PlanetName"
  value = "Uranus"
  # This is KEY: https://registry.terraform.io/providers/hashicorp/azurerm/latest/docs/resources/app_configuration_key
  depends_on = [
    azurerm_role_assignment.appconf_dataowner
  ]
}

