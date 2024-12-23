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
