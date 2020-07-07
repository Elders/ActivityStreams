# **Elders.ActivityStreams**

## **Description**

Placeholder text.

## **Needed Configuration**

**`activitystreams:cassandra`** is the contextual setting key. This can be structured as a `json` object if appsettings.json are being used.

- `activitystreams:cassandra:ConnectionString`
  - `string`
  - required
- `activitystreams:cassandra:ReplicationStrategy`
  - `string` 
  - optional (default = "simple")
- `activitystreams:cassandra:ReplicationFactor`
  - `integer` 
  - optional (default = 1)
- `activitystreams:cassandra:Datacenters`
  - `array<string>`
  - required

small caveat, if you are using `Elders.Pandora` with consul, then the Array of datacenters should be as separate keys named as:
- `activitystreams:cassandra:Datacenters:0`
- `activitystreams:cassandra:Datacenters:1`
- `activitystreams:cassandra:Datacenters:2`
