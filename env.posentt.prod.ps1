$RxHome = "$PWD"
$machine = ($env:COMPUTERNAME).Replace("-","_")

[System.Environment]::SetEnvironmentVariable("RX_POSENTT_SqlConnectionString", "Data Source=PMBIPTTSV;Initial Catalog=PosEntt;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False", "Process")
[System.Environment]::SetEnvironmentVariable("RX_PosEntt_PssConnectionString", "Data Source=PMBIPCBDV6;Initial Catalog=pss;User Id=enttuser;password=P@ssw0rd", "Process")

$env:RX_POSENTT_RabbitMqHost="PMBIPTTN08"
$env:RX_POSENTT_RabbitMqUserName="PosEnttApp"
$env:RX_POSENTT_RabbitMqPassword="slayer"
$env:RX_POSENTT_ElasticsearchHost="http://PMBIPTTN10:9200"
$env:RX_POSENTT_ElasticsearchLogHost="http://PMBIPTTN10:9200"
$env:RX_POSENTT_OalConnectionString = "Data Source=PMBIPVSSQL01;Initial Catalog=oal_dbo;User Id=enttuser;password=P@ssw0rd"
$env:RX_PosEntt_PssConnectionString = "Data Source=PMBIPCBDV6;Initial Catalog=pss;User Id=enttuser;password=P@ssw0rd"

<#

#Start-Process -NoNewWindow -FilePath .\subscribers.host\workers.console.runner.exe -ArgumentList @( "/log:console", "/h:$env:RX_POSENTT_RabbitMqHost", "/u:$env:RX_POSENTT_RabbitMqUserName", "/p:$env:RX_POSENTT_RabbitMqPassword", "/config:oal2", "/v:PosEntt" )
Start-Process -FilePath .\subscribers.host\workers.console.runner.exe -ArgumentList @( "/log:console", "/h:$env:RX_POSENTT_RabbitMqHost", "/u:$env:RX_POSENTT_RabbitMqUserName", "/p:$env:RX_POSENTT_RabbitMqPassword", "/config:oal1", "/v:PosEntt" )
#Start-Process -FilePath .\subscribers.host\workers.console.runner.exe -ArgumentList @( "/log:console", "/h:$env:RX_POSENTT_RabbitMqHost", "/u:$env:RX_POSENTT_RabbitMqUserName", "/p:$env:RX_POSENTT_RabbitMqPassword", "/config:oal2", "/v:PosEntt" )
Start-Process -FilePath .\subscribers.host\workers.console.runner.exe -ArgumentList @( "/log:console", "/h:$env:RX_POSENTT_RabbitMqHost", "/u:$env:RX_POSENTT_RabbitMqUserName", "/p:$env:RX_POSENTT_RabbitMqPassword", "/config:persistence", "/v:PosEntt" )

Start-Process -FilePath .\subscribers.host\workers.console.runner.exe -ArgumentList @( "/log:console", "/h:$env:RX_POSENTT_RabbitMqHost", "/u:$env:RX_POSENTT_RabbitMqUserName", "/p:$env:RX_POSENTT_RabbitMqPassword", "/config:trigger_subs_pickup-rts-pickup-to-oal", "/v:PosEntt" )
Start-Process -FilePath .\subscribers.host\workers.console.runner.exe -ArgumentList @( "/log:console", "/h:$env:RX_POSENTT_RabbitMqHost", "/u:$env:RX_POSENTT_RabbitMqUserName", "/p:$env:RX_POSENTT_RabbitMqPassword", "/config:trigger_subs_norm-rts-norm-to-oal", "/v:PosEntt" )
Start-Process -FilePath .\subscribers.host\workers.console.runner.exe -ArgumentList @( "/log:console", "/h:$env:RX_POSENTT_RabbitMqHost", "/u:$env:RX_POSENTT_RabbitMqUserName", "/p:$env:RX_POSENTT_RabbitMqPassword", "/config:trigger_subs_deco-deco-to-oal", "/v:PosEntt" )

#trigger_subs_pickup-rts-pickup-to-oal

#>