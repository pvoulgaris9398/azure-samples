# Azure App Service Web Apps

## Overview

- An `Http`-based `PaaS` service which can host _web applications_, _RESTful APIs_ and _mobile_ backends
- Can automate business processes with `WebJobs`
- Can use common programming languages
- `WebJobs` can run background processes such as `Powershell` scripts, `Bash` scripts, etc.
- Can host `Docker` containers, as well and `docker-compose`
- _App Service Environments_ which offer a fully-isolated environment
- Has continuous integration and deployment capabilities

## App Service Plan

- `App Services` run within an `App Service Plan`
- Resource type is `Microsof.Web/serverfarms` (note the plural)
- Defines compute resources available
- Can host multiple apps
- Define the operating system of the host VM's, the region, the number of VM instances and the pricing tier
- _Free_ and _Shared_ tiers run on the same VM's as other `App Service Apps`
- This includes other customers' Apps
- Intended for development and testing, can't be scaled
- _Isolated_ and _Isolated v2_ run on dedicated VM's
- Run on same dedicated Azure virtual networks (VNets)
- `Azure Function Apps` can also run within `App Service Plan`
- `App Service Plan` is unit-of-scale for `App Services`
