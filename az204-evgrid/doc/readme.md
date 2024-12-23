# Event Hubs

# Getting Started with this sample

- To use this sample create a `.env` and populate the following environment variables, as desired:

```bash
UNIQUE_STRING=****
RESOURCE_GROUP_NAME=****
REGION=****
TOPIC_NAME="****-egtopic-$UNIQUE_STRING"
SITE_NAME="****-egsite-$UNIQUE_STRING"
TEMPLATE_URI="https://raw.githubusercontent.com/Azure-Samples/azure-event-grid-viewer/main/azuredeploy.json"
SITE_URL="https://$SITE_NAME.azurewebsites.net"
```

## Details

- Designed to implement a _publish/subscribe_ pattern
- Multiple publishers, multiple subscribers
- Temporarily store the events unless a _subscriber_ can process them
- _Ingress_ connections by services to produce events and streams (_publishers_)
- _Egress_ connections by services (_consumers_) to receive the events for further processing
- Event hubs can support Apache Kafka
- Target for Azure resources such as VM's, app services, databases and networking resources
- Logs can be forwarded to external _Security Information and Event Management_ systems (SIEMS)
- Event hub _namespace_ is a virtual server, a logical container for hubs and can provide isolation and management features for access control
- Provide _namespace_, receive _fully-qualified domain name_ (FQDN)
- Can be managed from firewall and supports MQ Telemetry Transport (MQTT) and Advanced Message Queue Protocol (AMQP) over WebSockers and HTTPS

## Event Types

- Discrete: report state changes and are actionable - EventGrid
- Series: report a condition, time-ordered and analyzable - EventHub
- User Notification: prompt user or their device for attention - Notification Hub

## Pricing Model

- Basic Tier - No dynamic partition scaling, capturing events or VNet integration. Retention is limited to one (1) day
- Standard Tier - Can capture events in a storage account, integrate with VNets and retention up to seven (7) days
- Premium Tier - Expensive tier with no throughput limits and a retention period of 90 days
- Dedicated Tier - Same as premium but provisioned in an exclusive single-tenant environment

## Scaling

- Highly scalable service by extending the number of throughout units and processing units
- Basic and Standard up to 40 throughput units, responsible for ingress and egress traffic
- Premium allows horizontal scaling by increasing the _processing_ units. No throughput limits
- Premium processing unit equivalent to approximately 10 throughput units of the Basic and Standard tiers. Premium can scale up to 16 _processing_ units

## Partitions

- Number of partitions configured in privisioning step, minimum of one (1) and can not be changed later
- Number of _partitions_ should correspond to number of _throughput_ units, roughly
- Recommended number of partitions can be calculated from the planned solution throughput
- A _partition_ is a logically separated queue, operating in first-in-first-out (FIFO) pattern
- Service will load-balance subscribers among the available partitions, if there are more partitions that subscribers
- If all partitions already have a single subscriber, any new subscriber won't be able to connect

## Capturing Events

- Event _capturing_ is available for Standard and Premium Price where the event can still be available in the blob storage, even after expiration of the retention period
- Extra charges may apply

## Consumer Groups

- Used to perform independent read of events by an application
- Number of subscribers should not exceed five (5) consumers per group
- This topic is a little hazy to me, come back to it

## Event Consumption Services

- Azure Event Grid is designed for communication between cloud and on-premises applications and services
- Azure services can consume, process and analyze events from Event Hub
- Azure Event Grid can be connected to Azure Event Hub as a bridge to Azure Queue Storage, Azure Service Bus, Azure Functions, Azure Cosmos DB, etc.
- Azure Stream Analytics to process events and ingest them into Azure Blob Storage, Azure Queue Storage, Azure Service Bus, Azure Cosmos DB, Azure Synapse Analalytics and Power BI datasets
  - Compared to Azure Event Grid, sophisticated filtering algorithms, including leveraging Azure Machine Learning Services
- Big data ingestion via Azure Synapse Data Explorer and Azure Databricks solution

## Authentication & Authorization

- Shared Access Signature (SAS) tokens used to authenticate and authorize publishers and consumers with listen, send and manage rights
- Managed Identifies - both user-assigned and system-assigned

```text
Left-off at `Developing Applications for Event Hubs` &rarr; `Sunday, 11/22/24`
```
