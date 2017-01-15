mkdir C:\data
mkdir C:\temp
mkdir C:\logs

$env:JAVA_HOME=[System.Environment]::GetEnvironmentVariable("JAVA_HOME", "Machine")

$NetIpAddress = Get-NetIPAddress -InterfaceAlias "Ethernet 2" -AddressFamily IPv4
$IpAddress = $NetIpAddress.IPAddress


$Master = "true"
$Data = "false"
if($env:COMPUTERNAME.ToLowerInvariant().Contains("data")){
    $Master = "false";
    $Data = "true";
}
Write-Host "IP : $IpAddress and data = $Data , master = $Master" -ForegroundColor Yellow


$Yaml = Get-Content 'C:\Program Files\Elasticsearch\elasticsearch-1.7.5\config\elasticsearch.yml'
$Yaml = $Yaml.Replace("#cluster.name: elasticsearch", "cluster.name: posentt2")
$Yaml = $Yaml.Replace('#node.name: "Franz Kafka"', "node.name: `"$env:COMPUTERNAME`"")
#$Yaml = $Yaml.Replace("#node.master: true", "node.master: $Master")
#$Yaml = $Yaml.Replace("#node.data: true", "node.data: $Data")
$Yaml = $Yaml.Replace("#index.number_of_shards: 5", "index.number_of_shards: 5")
$Yaml = $Yaml.Replace("#index.number_of_replicas: 1", "index.number_of_replicas: 1")
$Yaml = $Yaml.Replace("#path.data: /path/to/data1,/path/to/data2", "path.data: c:\data")
$Yaml = $Yaml.Replace("#path.work: /path/to/work", "path.work: c:\temp")
$Yaml = $Yaml.Replace("#path.logs: /path/to/logs", "path.logs: c:\logs")
$Yaml = $Yaml.Replace("#network.bind_host: 192.168.0.1", "network.bind_host: $IpAddress")
$Yaml = $Yaml.Replace("#gateway.recover_after_time: 5m", "gateway.recover_after_time: 5m")
$Yaml = $Yaml.Replace("#discovery.zen.ping.multicast.enabled: false", "discovery.zen.ping.multicast.enabled: false")
$Yaml = $Yaml.Replace('#discovery.zen.ping.unicast.hosts: ["host1", "host2:port"]', 'discovery.zen.ping.unicast.hosts: ["192.168.1.198", "192.168.1.199", "192.168.1.200", "192.168.1.201", "192.168.1.202", "192.168.1.203", "192.168.1.204", "192.168.1.205"]')
#$Yaml = $Yaml.Replace("", "")
#$Yaml[57] = ""
#$Yaml[58] = ""

#$Yaml[63]=""
#$Yaml[64]=""

$Yaml | Out-File -Encoding ascii -FilePath 'C:\Program Files\Elasticsearch\elasticsearch-1.7.5\config\elasticsearch.yml'
# PsEdit 'C:\Program Files\Elasticsearch\elasticsearch-1.7.5\config\elasticsearch.yml'


Write-Host "Installing elasticsearch service" -ForegroundColor Yellow

& 'C:\Program Files\Elasticsearch\elasticsearch-1.7.5\bin\service.bat' install

Write-Host "Waiting for service to start after install"
Sleep -Seconds 5

# & 'C:\Program Files\Elasticsearch\elasticsearch-1.7.5\bin\service.bat' start


<#

$ElasticsearchServers = @('PMBIPTTN10','PMBIPTTN11','PMBIPTTN12')

$ElasticsearchServers | % {

    Invoke-Command -ComputerName $_ -ScriptBlock { 
        $env:JAVA_HOME=[System.Environment]::GetEnvironmentVariable("JAVA_HOME", "Machine")
        & 'C:\Program Files\Elasticsearch\elasticsearch-1.7.5\bin\service.bat' start 
    };
}


  Invoke-Command -ComputerName 'es-data-03' -ScriptBlock { 
        $env:JAVA_HOME=[System.Environment]::GetEnvironmentVariable("JAVA_HOME", "Machine")
        & 'C:\Program Files\Elasticsearch\elasticsearch-1.7.5\bin\service.bat' stop
        Sleep -Seconds 10 
        & 'C:\Program Files\Elasticsearch\elasticsearch-1.7.5\bin\service.bat' start
    };

#>