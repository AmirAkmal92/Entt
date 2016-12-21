# Realtime scanner README

Clone the repository to your desired location, normally I would go and do
```
git clone https://bespoke.visualstudio.com/DefaultCollection/pos.entt/_git/entt.rts c:\project\work\entt.rts
```

Extract the package from my OneDrive [Download 10345](https://1drv.ms/u/s!AnfOLTS4EYc4g6g3KEz8_ldNjJAYsg) 

, since web already been extracted you may want to leave it out , except you still need to copy web/bin


## Edit Elasticsearch config manually

Now edit `elasticsearch\config\elasticsearch.yml`

 ```yaml
 
cluster.name={yourmachinename}_posentt
node.name={your-machinename}_posentt_001

ndex.number_of_shards: 1
index.number_of_replicas: 0


http.port: 9800

```

make sure you have setup your `ERLANG_HOME` and your `JAVA_HOME` correctly

Now run `Setup.ps1` , use the "-" to override any setup parameters


## verify your installation

## RabbitmMQ
Go to http://localhost:15672, see if the broker is running

## SQL Server 
Use LINQPad in utils to connect to your localdb\ProjectsV13  and see if there's `PosEntt` database created, check there are tables with Sph schemas and dbo for aspnet* objects



# Elasticsearch
Go to http://localhost:9800/_cat/indices, you see at least `posentt_sys`


# IIS
got to your `config/applicationHost.config` , now find the line when it says 
``` xml
    <sites>
      <site name="web.PosEntt" id="1">
        <application path="/" applicationPool="Clr4IntegratedAppPool">
          <virtualDirectory path="/" physicalPath="F:\project\work\entt.rts\web" />
        </application>
        <bindings>
          <binding protocol="http" bindingInformation="*:8080:localhost" />
        </bindings>
      </site>
      <siteDefaults>
        <logFile logFormat="W3C" directory="%IIS_USER_HOME%\Logs" />
        <traceFailedRequestsLogging directory="%IIS_USER_HOME%\TraceLogFiles" enabled="true" maxLogFileSizeKB="1024" />
      </siteDefaults>
      <applicationDefaults applicationPool="Clr4IntegratedAppPool" />
      <virtualDirectoryDefaults allowSubDirConfig="true" />
    </sites>
```
*physicalPath should point to your web folder absolute path*

there should be an entry fo `web.posentt` that point to your `PWD\web` with binding to port 50230


## Setting up oal_dbo database

use the script in snippet folder

## Building the solution
Before you do, create the Tables for each EntityDefinition (I'll update the sph.builder.exe to do this automatically)

```
sqlcmd -E -S "(localdb)\ProjectsV13" -d PosEntt -i sources\EntityDefinition\SalesOrder.sql

```

Do this for all of sql file
* Delivery.sql
* ItemCategory.sql
* Product.sql
* SalesOrder.sql
* SapFiAccount.sql
* SurchargeAddOn.sql

Then finally you can run `tools\sph.builder.exe` to build the solution
for trouble shooting you could compile each component individually with
```bat
.\tool\sph.builder.exe sources\EntityDefinition\sales-order.json
```

## Known bugs

* ~~SQL Adapter wrongly generating `id` column name as `Id`, this will cause compile error~~ Fixed in 10325
* sph.builder.exe did not re-create the SQL Table on compiling


## Fixed in 10326
* FormDialog and PartialView - doesn't effect entt.rts but the ost

## Fixed in 10325
* Table primary key is now ToPascal casing


## Things fixed improved since 10324

* Using Mono.Cecil for querying CLR metadata instead System.Reflection, this will allow us to compile things 
that has not been deployed without locking up the dll
* sph.builder.exe will now compile all EntityDefinition dependencies, like the 
    * ServiceContact,
    * QueryEndpoint
    * OperationEndpoint
    * ReceivePort
    * ReceiveLocation
