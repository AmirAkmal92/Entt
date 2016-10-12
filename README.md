# Realtime scanner README

Clone the repository to your desired location, normally I would go and do
```
git clone https://bespoke.visualstudio.com/DefaultCollection/pos.entt/_git/entt.rts c:\project\work\entt.rts
```

Extract the package from my OneDrive [Download 10324-01](https://1drv.ms/u/s!AnfOLTS4EYc4g5lmE6o_C-BED4DZTQ) 

, since web already been extracted you may want to leave it out , except you still need to copy web/bin


Now run `Setup.ps1` , use the "-" to override any setup parameters

make sure you have setup your `ERLANG_HOME` and your `JAVA_HOME` correctly


## verify your installation

## RabbitmMQ
Go to http://localhost:15672, see if the broker is running

## SQL Server 
Use LINQPad in utils to connect to your localdb\ProjectsV13  and see if there's `PosEntt` database created, check there are tables with Sph schemas and dbo for aspnet* objects

## Edit Elasticsearch config manually

Now edit `elasticsearch\config\elasticsearch.yml`

 ```yaml
 
cluster.name={yourmachinename}_posentt
node.name={your-machinename}_posentt_001

ndex.number_of_shards: 1
index.number_of_replicas: 0


http.port: 9800

```

# Elasticsearch
Go to http://localhost:9800/_cat/indices, you see at least `posentt_sys`


# IIS
got to your `config/applicationHost.config` , now find the line when it says 
```
<sites...


</sites>
```

there should be an entry fo `web.posentt` that point to your `PWD\web` with binding to port 50230


## Setting up oal_dbo database

use the script in snippet folder