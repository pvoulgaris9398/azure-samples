# Monitoring and Logging Solutions in Azure

## TODO

- Review the metrics creation, etc. on `Application Insights`

## Azure Monitor

- Is a well-known, free monitoring tool for infrastructure services
- Highly extensible tool that can be used for analytics queries on the `Log Analytics` platform
- Multiple extensions for specific services, platforms and databases
- Can persist data for free for only a limited time
- Can be exceeded by leveraging a `Log Analytics Workspace`
- Functions as a _monitoring_ hub
- Collects performance data and logs
- Individual product groups can decide what best to monitor and report
- `Azure Virtual Machines` report their `CPU Usage`, `Available Memory`, `Networking` and `Disc Activities`
- `Azure Web Apps` report thier `request rate`, `response time`, `memory workset` and `exception rate`
- All services in `Azure` repor their metrics to a single location in `Azure`
- Can run multiple queries acrodd different services to find out the real cause of a problem
- _Performance_ metrics are collected and persisted for up to _90_ days by default
- Available as a _chart_ that can be observed on the _Monitoring_ page
- Metrics can be queried by `Log Analytics` and exported to files
- Collected metrics can be added to a _dashboard_
- _Dashboards_ can be **shared** with users of the organization, in live/realtime
- Performance data and logs can also be persisted in the storage account at low cost for years
- _Azure Monitor_ collects logs for _90_ days. It starts overwriting after \_90_days
- Activity logs to find out who provisioned resources, restarted VM's or modified resource settings
- Health monitoring of the global Azure platform
- Logs of VMs for example, _application event logs_ on _Windows_ or _system logs_ on \_linux_can be forwarded to an Azure storage account
- Can be pulled by a `Log Analytics` workspace and queried via `KQL`
- Azure Monitor exposes a _restful_ interface to connect to third-party tools such as _Grafana_ and _Datadog_ or _Power BI_
- Can be exported via command-line and parsed with tools such as `Performance Analysis of Logs (PAL)`
- Also, `VM insights` monitors health and performance of your VM in Azure
- `VM insights` can collect data and provide output with prebuilt _templates_ of _workbooks_, _charts_ and _alerts_
- Can also monitor dependencies between services and analyze network traffic

## Azure Log Analytics

- Requires an `Azure Log Analytics` workspace to be provisioned to install monitoring tools to monitor: the state of VM updates, security baselines, the performance of the server, web requests and app crashes, database size and load
- You can prepare queries with `KQL` with the collected data and metrics
- The requests results can be provided as a chart to pin on the monitoring dashboard
- The query can be automatically executed and it's results compared against a provide threshold to generate Azure alerts if needed

## Application Insights

- Telemetry service commonly used for web apps for monitoring and troubleshooting
- `Application Insights` is a web API service running in Azure
- `Application Insights` can perform event taking and performance tracing in realtime
- The technology is based on a client-side application script, reporting the loading and rendering time, and the server-side SDK's reporting the performance of the server
- Provides developers with a 360-degree view of the application
- Can track performance of dependent services (such as storage account and database), produce custom events and metrics and collection dumps of application crashes

## Azure Workbooks & Azure Dashboards

- Combinations of _telemetry charts_, _graphs_, _widgets_ and Markdown ares with descriptions
- Fully-customizable, usage of templates, can combine data from multiple sources, at a glance
