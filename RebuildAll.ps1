﻿param(

    [Switch]$NoNewWindow
)

& .\env.posentt.ps1

$erlang = Get-Process -Name erl* | measure
if($erlang.Count -ne 1){
    Start-Process .\rabbitmq_server\sbin\rabbitmq-server.bat
}

$java = Get-Process -Name java* | measure
if($java.Count -ne 1){
    Start-Process .\elasticsearch\bin\elasticsearch.bat
}
ls -Path .\output -Filter "*.*" | Remove-Item


ls -Path .\schedulers -Filter "PosEntt.*.dll" | Remove-Item
ls -Path .\schedulers -Filter "PosEntt.*.pdb" | Remove-Item

ls -Path .\subscribers -Filter "PosEntt.*.dll" | Remove-Item
ls -Path .\subscribers -Filter "PosEntt.*.pdb" | Remove-Item


ls -Path .\subscribers -Filter "subscriber.trigger.*.dll" | Remove-Item
ls -Path .\subscribers -Filter "subscriber.trigger.*.pdb" | Remove-Item


ls -Path .\subscribers -Filter "workflows.*.dll" | Remove-Item
ls -Path .\subscribers -Filter "workflows.*.pdb" | Remove-Item


ls -Path .\web\bin -Filter "workflows.*.dll" | Remove-Item
ls -Path .\web\bin -Filter "workflows.*.pdb" | Remove-Item


ls -Path .\web\bin -Filter "PosEntt.*.dll" | Remove-Item
ls -Path .\web\bin -Filter "PosEntt.*.pdb" | Remove-Item


#Build functoid
$MsBuild = Get-Command msbuild* | measure
if($MsBuild.Count -lt 1){
    Write-Error "Use Visual Studio 2015 command prompt to compile the custom functoids"
    return;
}
msbuild .\posentt.sln /m


if($NoNewWindow.IsPresent){

    Start-Process .\tools\sph.builder.exe -Wait -NoNewWindow
}
else{

    Start-Process .\tools\sph.builder.exe -Wait
}

Write-Host "Done compiling, please check any errors ... " -ForegroundColor Cyan


Write-Progress -Activity "Deploy" -Status "Deploying output to web\bin ..." -PercentComplete 25 -Id 15

ls -Path .\output -Filter PosEntt.*.dll | Copy-Item -Destination .\web\bin
ls -Path .\output -Filter PosEntt.*.pdb | Copy-Item -Destination .\web\bin

Write-Progress -Activity "Deploy" -Status "Deploying output to subscribers ..." -PercentComplete 40 -Id 15

ls -Path .\output -Filter PosEntt.*.dll | Copy-Item -Destination .\subscribers
ls -Path .\output -Filter PosEntt.*.pdb | Copy-Item -Destination .\subscribers


Write-Progress -Activity "Deploy" -Status "Deploying workflow to subscribers ..." -PercentComplete 70 -Id 15
ls -Path .\output -Filter workflows.*.dll | Copy-Item -Destination .\subscribers
ls -Path .\output -Filter workflows.*.pdb | Copy-Item -Destination .\subscribers

ls -Path .\output -Filter workflows.*.dll | Copy-Item -Destination .\web\bin
ls -Path .\output -Filter workflows.*.pdb | Copy-Item -Destination .\web\bin


Write-Progress -Activity "Deploy" -Status "Deploying custom triggers to subscribers ..." -PercentComplete 90 -Id 15
ls -Path .\output -Filter "subscriber.trigger.*.dll" | Copy-Item -Destination .\subscribers
ls -Path .\output -Filter "subscriber.trigger.*.pdb" | Copy-Item -Destination .\subscribers


Write-Progress -Activity "Deploy" -Status "Deployment done ..." -PercentComplete 100 -Id 15

Write-Host "Done..."

<# from sph to test


$subscribers = "$PWD\subscribers"
$subscribersHost = "$PWD\subscribers"
$webBin = "$PWD\web\bin"
$tools = "$PWD\tools"
$schedulers = "$PWD\schedulers"

$domaindll = "c:\project\work\sph\source\domain\domain.sph\bin\Debug\domain.sph.dll"
$domainpdb = "c:\project\work\sph\source\domain\domain.sph\bin\Debug\domain.sph.pdb"

copy c:\project\work\sph\bin\tools\sph.builder.exe $tools
copy c:\project\work\sph\bin\tools\sph.builder.pdb $tools

copy $domaindll $tools
copy $domainpdb $tools
copy $domaindll $webBin
copy $domainpdb $webBin
copy $domaindll $subscribers
copy $domainpdb $subscribers
copy $domaindll $subscribersHost
copy $domainpdb $subscribersHost


copy C:\project\work\sph\bin\tools\*.adapter.* .\tools
copy C:\project\work\sph\bin\tools\*.adapter.* .\web\bin

copy C:\project\work\sph\source\functoids\database.lookup\bin\Debug\database.lookup.dll F:\project\work\entt.rts\tools
copy C:\project\work\sph\source\functoids\database.lookup\bin\Debug\database.lookup.pdb F:\project\work\entt.rts\tools
copy C:\project\work\sph\source\functoids\database.lookup\bin\Debug\database.lookup.dll F:\project\work\entt.rts\web\bin
copy C:\project\work\sph\source\functoids\database.lookup\bin\Debug\database.lookup.pdb F:\project\work\entt.rts\web\bin

Start-Process .\tools\sph.builder.exe -ArgumentList @(".\sources\TransformDefinition\soc-to-snb-sales-order.json")



copy C:\project\work\sph\source\web\core.sph\bin\core.sph.dll .\web\bin
copy C:\project\work\sph\source\web\core.sph\bin\core.sph.pdb .\web\bin


copy C:\project\work\sph\source\web\webapi.common\bin\Debug\webapi.common.dll .\web\bin
copy C:\project\work\sph\source\web\webapi.common\bin\Debug\webapi.common.pdb .\web\bin



copy C:\project\work\sph\source\adapters\restapi.adapter\bin\Debug\restapi.adapter.dll .\tools
copy C:\project\work\sph\source\adapters\restapi.adapter\bin\Debug\restapi.adapter.pdb .\tools
copy C:\project\work\sph\source\adapters\restapi.adapter\bin\Debug\restapi.adapter.dll .\web\bin
copy C:\project\work\sph\source\adapters\restapi.adapter\bin\Debug\restapi.adapter.pdb .\web\bin


$mappings = ls .\sources\TransformDefinition -Filter *.json
foreach($map in $mappings){
    Write-Host "Compiling $map ..." -ForegroundColor Cyan
    .\tools\sph.builder.exe $map
}

#>

