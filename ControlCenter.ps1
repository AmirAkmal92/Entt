$RxHome = "$PWD"
$machine = ($env:COMPUTERNAME).Replace("-","_")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_HOME","$RxHome", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_WebPath","$PWD\web", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_WebsitePort","8080", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_BaseUrl","http://localhost:8080", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_IisExpressExecutable","$RxHome\IIS Express\iisexpress.exe", "Process")


[System.Environment]::SetEnvironmentVariable("RX_POSENTT_RabbitMqPassword","guest", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_RabbitMqManagementPort","15672", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_RabbitMqUserName","guest", "Process")

[System.Environment]::SetEnvironmentVariable("RABBITMQ_BASE","$RxHome\rabbitmq_base", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_RabbitMqBase","$RxHome\rabbitmq_base", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_RabbitMqDirectory","$RxHome\rabbitmq_server", "Process")
[System.Environment]::SetEnvironmentVariable("PATH","$env:Path;$RxHome\rabbitmq_server\sbin", "Process")

[System.Environment]::SetEnvironmentVariable("RX_POSENTT_ElasticsearchIndexNumberOfShards","1", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_ElasticsearchHttpPort","9800", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_ElasticsearchHost","http://localhost:9800", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_ElasticSearchJar","$RxHome\elasticsearch\lib\elasticsearch-1.7.5.jar", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_ElasticsearchClusterName","cluster_$machine""_POSENTT", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_ElasticsearchIndexNumberOfReplicas","0", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_ElasticsearchNodeName","node_$machine" + "_POSENTT", "Process")


[System.Environment]::SetEnvironmentVariable("RX_POSENTT_LoggerWebSocketPort","50438", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_SqlLocalDbName","ProjectsV13", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_SqlConnectionString", "Data Source=(localdb)\ProjectsV13;Initial Catalog=POSENTT;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", "Process")


[System.Environment]::SetEnvironmentVariable("RX_POSENTT_BromConnectionString", "Data Source=S301\DEV2016;Initial Catalog=PittisNonCore;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_SnbWebNewAccount_BaseAddress", "http://eryken2.asuscomm.com:8086", "Process")



if((Test-Path("Update.bat")) -eq $true){
    & .\Update.bat
}

& .\control.center\controlcenter.exe