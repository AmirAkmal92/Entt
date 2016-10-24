  param(
  [pscredential]$Admin = (Get-Credential "ORG\Administrator"),
  [PsCredential]$AppPoolCredential,
  [string]$PosEnttEnvironment = "ken2",
  [string]$RxDscPath = "F:\project\work\rx-dsc\sources",
  [switch]$GenerateDscMof  
)
$configs = Import-PowerShellDataFile ".\config.$PosEnttEnvironment.psd1"
$AllNodes = $configs.AllNodes
$BuildServer = $configs.NonNodeData.BuildServer
$PosEnttHome = $configs.RxConfigs.Home

# TODO : get all assets to $BuildServer
$DevHome = $env:RX_PosEntt_HOME
Write-Host "Creating build server at $BuildServer"
if((Test-Path($BuildServer)) -eq $true){
   ls $BuildServer -Recurse | Remove-Item -Recurse -Force
}else{

    Mkdir $BuildServer
}

Robocopy $DevHome\sources $BuildServer\sources /mir
Robocopy $DevHome\output $BuildServer\output /mir
Robocopy $DevHome\web $BuildServer\web  /mir
Robocopy $DevHome\subscribers $BuildServer\subscribers /mir
Robocopy $DevHome\tools $BuildServer\tools /mir
Robocopy $DevHome\subscribers.host $BuildServer\subscribers.host /mir


$WebServers = New-Object System.Collections.ArrayList
$WorkerServers = New-Object System.Collections.ArrayList

foreach($Node in $AllNodes)
{
    if(($Node.Roles.IndexOf("Web") -gt -1))
    {
        $NodeName = $Node.NodeName
        Write-Host  "Node $NodeName is a Web" -ForegroundColor Yellow
        $row = $WebServers.Add($NodeName)
    }
}


foreach($Node in $AllNodes)
{
    if(($Node.Roles.IndexOf("Worker") -gt -1))
    {
        $NodeName = $Node.NodeName
        Write-Host  "Node $NodeName is a Worker" -ForegroundColor Yellow
        $row = $WorkerServers.Add($NodeName)
    }
}

<# TODO verify that we had DSC into this computer before, if not then #>
if($GenerateDscMof.IsPresent -eq $true)
{
    if($AppPoolCredential -eq $null)
    {
        Write-Host "To execute DSC MOF, please provide AppPoolCredental" -ForegroundColor Red
        return
    }
        
    $configFile = "$PWD\config." + $PosEnttEnvironment + ".psd1"
    if((test-path($configFile)) -eq $false){
        Write-Host -ForegroundColor Yellow "Cannot find any config file for $PosEnttEnvironment"
        return;
    }

    try{
        
        & "$RxDscPath\install-iis.ps1"
        InstallIIS -AppPoolCredential $AppPoolCredential  -OutputPath "$PWD\$PosEnttEnvironment\webs-configuration" -ConfigurationData $configFile

    }catch{    
        Write-Error -Exception $_.Exception
        Write-Host "You may have to execute  $RxDscPath\install-iis.ps1 before running this script"
        return
    }

    
    try{
        & "$RxDscPath\install-worker.ps1"
        InstallRxWorkers  -AppPoolCredential $AppPoolCredential  -OutputPath "$PWD\$PosEnttEnvironment\workers-configuration" -ConfigurationData $configFile

    }catch{
        Write-Error -Exception $_.Exception
        Write-Host "You may have to execute  $RxDscPath\install-worker before running this script"
        return
    }
}

foreach($server in $WebServers)
{
    Write-Host "Starting DSC configuration on $server ..... please wait.."
    Invoke-Command -Credential $Admin -ComputerName $server -ScriptBlock { if((Test-Path("$using:PosEnttHome")) -eq $true){ ls $using:PosEnttHome -Recurse | Remove-Item -Recurse -Force }}    
   
    Invoke-Command -Credential $Admin -ComputerName $server -ScriptBlock { Install-Module xWebAdministration -MinimumVersion  1.14.0.0 }
    Invoke-Command -Credential $Admin -ComputerName $server -ScriptBlock { Install-Module xNetworking -MinimumVersion 2.12.0.0 }

    Start-DscConfiguration -Credential $Admin -ComputerName $server -Path $PWD\$PosEnttEnvironment\webs-configuration -Verbose -Wait -Force
      
} 

foreach($server in $WorkerServers)
{
    Write-Host "Starting DSC configuration on $server ..... please wait.."
    # TODO : stop and remove any workers service
    Invoke-Command -Credential $Admin -ComputerName $server -ScriptBlock { if((Test-Path("$using:PosEnttHome")) -eq $true){ ls $using:PosEnttHome -Recurse | Remove-Item -Recurse -Force }}    
   
    $ps = New-PSSession -ComputerName $server -Credential $Admin

    Copy-Item 'C:\Program Files\WindowsPowerShell\Modules\rxReactiveDeveloper' -Recurse  -Force -ToSession $ps 'C:\Program Files\WindowsPowerShell\Modules'
    Copy-Item 'C:\Program Files\WindowsPowerShell\Modules\Carbon' -Recurse  -Force -ToSession $ps 'C:\Program Files\WindowsPowerShell\Modules'
    Copy-Item 'C:\Program Files\WindowsPowerShell\Modules\xNetworking' -Recurse  -Force -ToSession $ps 'C:\Program Files\WindowsPowerShell\Modules'


    Start-DscConfiguration -Credential $Admin -ComputerName $server -Path $PWD\$PosEnttEnvironment\workers-configuration -Verbose -Wait -Force
      
} 
  