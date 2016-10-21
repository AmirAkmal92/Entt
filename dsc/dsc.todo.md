# Things to do when using DSC deployment


## Web server and IIS

* Remove web.console logger from web.config
* Change the url to server logger, or remove it completely


## Existing RabbitMQ

* Create a new vhost

```bat
rabbitmqctl add_vhost PosEntt
```
* Set the user permission

```bat
REM if the user doesnot exist add_user
rabbitmqctl set_permissions -p PosEntt rx-myapp ".*" ".*" ".*"
```


# Elasticsearch

I use [elasticdump](https://github.com/taskrabbit/elasticsearch-dump) to export the elasticsearch index from dev to the server

``` bat
node .\node_modules\elasticdump\bin\elasticdump --input http://localhost:9800/posentt --output http://S305:9200/posentt
node .\node_modules\elasticdump\bin\elasticdump --input http://localhost:9800/posentt_sys --output http://S305:9200/posentt_sys

```

Bear in mind that the access token is not transferable unless we set the same token secret in out environment variable


## Database
Just backup and restore, that's the easiest way to do it, but for the real one, we need to configure diferrent file group for tables