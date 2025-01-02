# Monitoring and Logging Solutions in Azure

## Permissions

- Require `Monitoring Reader` permission at the _Subscription_ level to visualize metrics across multiple resources
- Most metrics in `Azure` are stored for _93_ days
- You can query no more than _30_ days on any one chart
- This limitation does not apply to _log-based metrics_

## Notes

- Note that custom events are subject to sampling, while custom metrics are not
- Recommend use of `GetMetric` because of pre-aggregation performed by this implementation. This is .NET specific
- `GetMetric` does not support tracking the _last value_ (of course) or tracking _histograms_ or _distributions_

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

### Overview

- Is an _extension_ of `Azure Monitor` and provides _Application Performance Monitoring_ (APIM) features
- Can collection _metrics_ and _application telemetry data_
- Can also collect and store _application trace data_
- The _log trace_ is associated with other _telemetry_ data, configuration usually just consists of providing a _destination_ for the logs; the logging framework rarely needs to be changed
- Telemetry service commonly used for web apps for monitoring and troubleshooting
- `Application Insights` is a web API service running in Azure
- `Application Insights` can perform event taking and performance tracing in realtime
- The technology is based on a client-side application script, reporting the loading and rendering time, and the server-side SDK's reporting the performance of the server
- Provides developers with a 360-degree view of the application
- Can track performance of dependent services (such as storage account and database), produce custom events and metrics and collection dumps of application crashes

### Live Metrics

- Observe activity from a deployed application in real-time with no effect on the host environment

### Availability

- Also known as _Synthetic Transaction Monitoring_
- Can _probe_ applications external endpoints to test the overall availability and responsiveness over time

### GitHub or Azure DevOps Integration

- Can create GitHub or Azure DevOps work items in the context of _Application Insights_ data

### Usage

- Understand which features are important to users and how users interact with the application

### Smart Detection

- Automatic failure and anomaly detection through proactive telemetry analysis

### Application Map

- A high-level, top-down view of the application archictecture and at-a-glance visual references to component health and responsiveness

### Distributed Tracing

- Search and visualize and end-to-end view of a given execution or transaction

### Monitors

- _Request Rates_, _Response Times_ and _Failure Rates_
- Helps to find which pages are popular, at what times of day and where the users are
- See which pages perform best
- If failure rates and response times go high when there are more requests, then perhaps there is a resource problem
- _Dependency Rates_, _Response Times_ and _Failure Rates_
- Find out which _external_ services are slowing down
- _Exceptions_
- Analyze aggregated statistics of pick specific instances to drill down into
- Both browser and server based exceptions
- _Page Views_ and _load performance_
- As reported by _users'_ browsers
- _User_ and _Session_ counts
- _Performance Counters_
- From _Windows_ or _linux_ server machines
- Such as _CPU Usage_, _memory_ and _network_ usage
- _Host_ diagnotics, such as from _Azure_ or _Docker_
- _Diagnosic Trace Logs_ from your app, so you can correlate _trace events_ with _requests_
- _Custom Events and Metrics_
- Written in client or server code, to track _business_ events such as _items sold_ or _games won_

### Getting Started

- Free to sign-up
- `Basic` pricing plan of `Application Insights` has no _charges_ until the application usage has grown to a substantial amount
- Start _monitoring_ and _analyzing_ app performance
- Can get started at _run time_ by instrumenting the app on the server. Ideal for apps already deployed. Avoids any updates to the code
- At _development time_ by adding `Application Insights` into the code. Allows customization of the telemetry collection and sending even more telemetry
- _Instrument_ your web pages for _page view_, _AJAX_ and other _client-side telemetry_
- _Analyze_ mobile app usage by integrating with `Visual Studio App Center`
- _Availability_ tests by _pinging_ the website regularly from the _Azure_ servers

### Log-Based Metrics

- `Application Insights` _log-based metrics_ let us analyze the _health_ of our _monitored apps_, create _powerful dashboard_ and _configure alerts_
  _Log-based_ metrics that behind the scenes are translated into _kusto_ queries from _stored events_
- Can use the SDK to send events manually or can rely on the automatic collection of events from auto-instrumentation
- `Application Insights` stores collected events as _logs_
- `Application Insights` blade in `Azure Portal` acts as the diagnostic and analytical tool for visualizing event-based data from logs
- Examples: get an exact count of requests to a URL or the number of distinct users who made these calls
- Examples: detailed diagnostic traces, including exceptions and dependency calls for any user session
- `Application Insights` implements several telemetry volume reduction techniques, such as _sampling_ and _filtering_ that reduces the number of collected and stored events
- The trade-off is that this reduces the accuracy of the metrics used to perform query-time aggregations of the events stored in logs

### Standard Metrics or Pre-Aggregated Metrics

- Stored as _pre-aggregated_ time series with only a few _key_ dimenstions
- Since _Standard Metrics_ are stored as _pre-aggregated_ time series during collection, they are more performant at query time
- Better choice for _dashboarding_ and _real-time_ analysis and alerting
- _Log-based_ metrics have _more dimensions_, which makes them superior for _data analysis_ and _ad-hoc diagnostics_
- Use the _namespace_ selector to switch between _log-based_ and _standard-metrics_ in the _metrics explorer_
- The newer SDK's such as `Application Insights` 2.7 or later pre-aggregate metrics during collection
- Applies to _standard metrics_ by default, so accuracy isn't affected by _sampling_ or _filtering_
- Also applies to custom metrics sent with `GetMetric`, resulting in less data ingestion and lower cost
- Note that _collection endpoint_ pre-aggregates events _before_ ingestion sampling, which means that _ingestion sampling_ will **never** impact the accuracy of _pre-aggregated_ metrics, regardless of which `SDK` version is used

### Instrument An App for Monitoring

- Simply **enabling** an application to capture _telemetry_
- Two (2) methods to do so: _Automatic_ and _Manual_
- `Autoinstrumentation` enables _telemetry_ collection through _configuration_ changes, without touching the application code
- Although its more convenient, it's less configurable
- Not available in _all_ languages
- When _available_ its the easiest way to enable instrumentation
- `Manual Instrumentation` is coding against the `Application Insights` or `OpenTelemetry API`
- Install and use a language-specific SDK
- Used to make custom _dependency_ calls or _API_ calls that are not captured by default
- `Application Insights` required if:
  - You require _custom events_ or _custom metrics_
  - You require control over the _flow_ of telemetry
  - Auto-instrumentation is not available
- `OpenTelemetry`
  - Combination of `Open Census` and `Open Tracing`
  - Contributions from all major cloud vendors
  - Contributions from `Application Performance Management` (APM) vendors
  - `Cloud Native Computing Framework` (CNCF)
  - Microsoft is a member of the `Cloud Native Computing Framework`

#### Terms

- `Application Insights` _autocollectors_ goes to `OpenTelemetry` _Instrumentation Libraries_
- `Application Insights` _Channel_ goes to `OpenTelemetry` _Exporter_
- `Application Insights` _Codeless/Agent-based_ goes to `OpenTelemetry` _Autoinstrumentation_
- `Application Insights` _Traces_ goes to `OpenTelemetry` _Logs_
- `Application Insights` _Requests_ goes to `OpenTelemetry` _Server Spans_
- `Application Insights` _Dependencies_ goes to `OpenTelemetry` _Other Span Types (Client, Internal, etc.)_
- `Application Insights` _Operation ID_ goes to `OpenTelemetry` _Trace ID_
- `Application Insights` _ID_ or _Operation Parent ID_ goes to `OpenTelemetry` _Span ID_

### Tests

- `Application Insights` can send _pings_ or _web requests_ at regular intervals
- It can create up to _100_ _availability_ tests per `Application Insights` resource

###

## Azure Workbooks & Azure Dashboards

- Combinations of _telemetry charts_, _graphs_, _widgets_ and Markdown ares with descriptions
- Fully-customizable, usage of templates, can combine data from multiple sources, at a glance
