Param(
       [switch]$InMemory = $false,
       [switch]$Debug = $false,
       [string]$config = "all"
       
     )

  
if($Debug -ne $false){
   Start-Process -FilePath .\subscribers.host\workers.console.runner.exe -ArgumentList @( "/log:console", "/h:$env:RX_POSENTT_RabbitMqHost", "/u:$env:RX_POSENTT_RabbitMqUserName", "/p:$env:RX_POSENTT_RabbitMqPassword", "/config:$config", "/v:PosEntt",  "/debug" )
}else{
   Start-Process -FilePath .\subscribers.host\workers.console.runner.exe -ArgumentList @( "/log:console", "/h:$env:RX_POSENTT_RabbitMqHost", "/u:$env:RX_POSENTT_RabbitMqUserName", "/p:$env:RX_POSENTT_RabbitMqPassword", "/config:$config", "/v:PosEntt" )
}
    
