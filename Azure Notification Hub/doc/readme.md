# Azure Notification Hub (ANH)

## Overview

- Send push notifications
- Send to multiple platforms - iOS, Android, Windows
- ANH provides abstraction over platform notificaiton services

## Features

- Cross-platform, front-and-back-end
- Multiple delivery formats, _push to user_, _push to device_, _localization_, _silent push_
- Telemetry
- Scalable

## Components

- Platform notification service (PNS) - vendor specific
- Notification Hub - communicates with PNS
- Namespace - regional collection of hubs

## Notification Hubs & Namespaces

- Namespace is a collection of Notification Hubs
- One namespace per application
- One hub per application environment
- Credentials at the namespace level
- Billing at the namespace level

## Workflow

- Setup PNS, implement vendor-specific details for each platform
- Setup ANH, create namespace and hub via portal
- Map PNS to ANH, apply keys
- Register devices via .NET SDK or Web API backend
- Send pushs

## C# Classes

- NotificationHubClient - Requires connection string and name
- Installation - Requires installation id and push channel and _Platform_
- CreateOrUpdateInstallationAsync
- Microsoft.Azure.NotificationHubs
- SendTemplateNotificationAsync
