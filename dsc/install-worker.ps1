<#

 for web we also need to have output folder
#>

Configuration InstallRxWorkers{
 param
        (
            [PSCredential] $AppPoolCredential
        )
        

        Import-DscResource -Module xWebAdministration -ModuleVersion 1.14.0.0
        Import-DscResource -ModuleName xNetworking -ModuleVersion 2.12.0.0
        Import-DscResource -ModuleName PSDesiredStateConfiguration
        Import-DscResource -ModuleName Carbon -ModuleVersion 2.3.0
        Import-DscResource -ModuleName rxReactiveDeveloper

        $ApplicationName = $ConfigurationData.RxConfigs["ApplicationName"];
        $RxHome = $ConfigurationData.RxConfigs["Home"];


        #web 
        Node $AllNodes.Where({$_.Roles.IndexOf("Worker") -gt -1}).NodeName {
            Write-Host -ForegroundColor Yellow ("Setting workers " + $Node.NodeName)
		   
                    
            File RxAppOutput{
                Ensure = "Present" 
                Type = "Directory“
                Force = $True
                Recurse = $True
                SourcePath =  $ConfigurationData.NonNodeData["BuildServer"] + "\output"
                DestinationPath = $ConfigurationData.RxConfigs["Home"]  + "\output"
            }
            
                    
            File RxAppSource {
                Ensure = "Present" 
                Type = "Directory“
                Force = $True
                Recurse = $True
                SourcePath =  $ConfigurationData.NonNodeData["BuildServer"] + "\sources"
                DestinationPath = $ConfigurationData.RxConfigs["Home"]  + "\sources"
            }
            
            File RxAppSubscribers {
                Ensure = "Present" 
                Type = "Directory“
                Force = $True
                Recurse = $True
                SourcePath =  $ConfigurationData.NonNodeData["BuildServer"] + "\subscribers"
                DestinationPath = $ConfigurationData.RxConfigs["Home"]  + "\subscribers"
            }
            
            
            
            
            File RxAppTools{
                Ensure = "Present" 
                Type = "Directory“
                Force = $True
                Recurse = $True
                SourcePath =  $ConfigurationData.NonNodeData["BuildServer"] + "\tools"
                DestinationPath = $ConfigurationData.RxConfigs["Home"]  + "\tools"
            }

            
            
            File RxAppSchedules{
                Ensure = "Present" 
                Type = "Directory“
                Force = $True
                Recurse = $True
                SourcePath =  $ConfigurationData.NonNodeData["BuildServer"] + "\schedulers"
                DestinationPath = $ConfigurationData.RxConfigs["Home"]  + "\schedulers"
            }
            Write-Host $Node
            foreach($instance in $Node.Instances){
            
                $worker = $instance.Name
                Write-Host "Configuring workers : $worker" -ForegroundColor Yellow
                
                File "RxAppSubscribersHost$worker" {
                    Ensure = "Present" 
                    Type = "Directory“
                    Force = $True
                    Recurse = $True
                    SourcePath =  $ConfigurationData.NonNodeData["BuildServer"] + "\subscribers.host"
                    DestinationPath = $ConfigurationData.RxConfigs["Home"]  + "\subscribers.host.$worker"
                }
                
                xAppConfig "RxWorkerServiceConfig$worker"
                {
                      Ensure = "Present"
                      ConfigPath = $ConfigurationData.RxConfigs["Home"]  + "\subscribers.host.$worker\workers.windowsservice.runner.exe.config"
                      Key = 'sph:WorkersConfig'
                      Value = $instance.Config
                      DependsOn = "[File]RxAppSubscribersHost$worker"
                }

            
                Carbon_Service "RxWorkerService$worker"{
                    Ensure = "Present"
                    Name = "RxWorkerService$worker"
                    StartupType = "Automatic"
                    Credential = $AppPoolCredential
                    Path = $ConfigurationData.RxConfigs["Home"]  + "\subscribers.host.$worker\workers.windowsservice.runner.exe"
                    OnFirstFailure = "Restart"
                    OnSecondFailure = "Restart"
                    OnThirdFailure = "Reboot" 
                    DependsOn = "[xAppConfig]RxWorkerServiceConfig$worker"
                }
                
            }



            
	    }

        #environments settings for  ConfigurationManager
        Node $AllNodes.Where({$_.Environment -eq $true}).NodeName {
            Write-Host "Setting env on " + $Node.NodeName -ForegroundColor Yellow

            foreach($rxe in $ConfigurationData.RxConfigs.Keys.Where({$_.Contains(".") -eq $false})){
                 Write-Host "RX_$rxe" -ForegroundColor Gray

                Environment "RX_$rxe" {
                    Ensure = "Present"
                    Name = "RX_$ApplicationName" + "_$rxe"
                    Value = $ConfigurationData.RxConfigs[$rxe]
 
                }
            }

        }
}


<#




if($webPoolCred -eq $null){
    #TODO ; this account need to have the service priviledge and have permission is sql server
    $webPoolCred = Get-Credential -UserName ORG\web-snb -Message "Enter the credential for SnbWebpool application pool"
}

InstallRxWorkers  -AppPoolCredential $webPoolCred -OutputPath ".\output\install-workers" -ConfigurationData .\config.psd1



$configs =Import-PowerShellDataFile .\config.psd1

Copy-Item .\rxReactiveDeveloper\DSCResources\xAppConfig -Recurse -Force 'C:\Program Files\WindowsPowerShell\Modules\rxReactiveDeveloper\DSCResources'

$s308 = New-PSSession -ComputerName S308
Copy-Item .\rxReactiveDeveloper -Recurse  -Force -ToSession $s308 'C:\Program Files\WindowsPowerShell\Modules'
Copy-Item 'C:\Program Files\WindowsPowerShell\Modules\Carbon' -Recurse  -Force -ToSession $s308 'C:\Program Files\WindowsPowerShell\Modules'
Copy-Item 'C:\Program Files\WindowsPowerShell\Modules\xNetworking' -Recurse  -Force -ToSession $s308 'C:\Program Files\WindowsPowerShell\Modules'

Get-Service -ComputerName S308 -Name RX*
#>

