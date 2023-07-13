# Demo EF with Cosmos
Azure Cosmos DB is a globally distributed, multi-model database service 
offered by Microsoft as part of the Azure cloud platform. It is designed 
to provide fast and scalable access to data using a variety of APIs, 
including SQL, MongoDB, Cassandra, Azure Table Storage, and Gremlin.

## Install Azure Cosmos DB Emulator
https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator

---
## How to setup Azure Cosmos DB Emulator using docker compose

### Run Emulator

`docker-compose up -d`

### Install Certificate
After simulator is running you need to download simulator self-signed certificate and install it to you machine trusted store.

```
curl -k https://localhost:8081/_explorer/emulator.pem > azure.cosmosdb.crt
```
Because this is a self-signed certificate we need to add it to our trusted certificates.
On windows machine, we have two options:
- Options #1
  - Double click on certificate
  - Click **Install certificate**
  - Select **Current User** and click **Next**
  - IMPORTANT: Select **Place all certificates in the following store** and 
	click **Browse**. Because by default it installs into **Personal** stores. 
	And most likely certificate validation in you application will not work out 
	of the box.
  - Select **Trusted Root Certification Authority** and click **Ok**, then **Nest**
	and **Finish**
  - When Security Warning window appears click **Yes** button.
- Option #2
  - Run the following `certutil` command from the same folder where the cert is located: 
	- `certutil -addstore -f -v -enterprise -user root azure.cosmosdb.crt`

If the certificate is installed, we should be to navigate to the emulator portal 
using following link https://localhost:8081/_explorer/index.html without warning.

---
## Links:
- [Azure-Samples/cosmos-db-nosql-dotnet-samples](https://github.com/azure-samples/cosmos-db-nosql-dotnet-samples)
- [Install and use the Azure Cosmos DB Emulator for local development and testing](https://learn.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=ssl-netstd21)
- [Pricing Calculator](https://azure.microsoft.com/en-us/pricing/details/cosmos-db/autoscale-provisioned/)