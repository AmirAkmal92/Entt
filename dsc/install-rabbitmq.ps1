Configuration InstallRabbitMq{


        Import-DscResource -Module xWebAdministration -ModuleVersion 1.14.0.0
        Import-DscResource -ModuleName xNetworking -ModuleVersion 2.12.0.0
        Import-DscResource -ModuleName PSDesiredStateConfiguration

        $ApplicationName = $ConfigurationData.RxConfigs["ApplicationName"];
        $RxHome = $ConfigurationData.RxConfigs["Home"];


        # elasticsearch
        Node $AllNodes.Where({$_.Roles.Contains("RabbitMq")}).NodeName {
            Write-Host -ForegroundColor Yellow ("Setting RabbitMq server on " + $Node.NodeName)

            File "RABBITMQ_BASE_DIR"
            {
                Ensure = "Present"
                DestinationPath = $Node.RabbitMqConfigs.BaseDirectory
                Type = "Directory"
            }

            Environment "RABBITMQ_BASE" 
            {
                Ensure = "Present"
                Name = "RABBITMQ_BASE"
                Value = $Node.RabbitMqConfigs.BaseDirectory
                DependsOn = "[File]RABBITMQ_BASE_DIR"
 
            }

            
            Environment "RABBITMQ_SERVICENAME" 
            {
                Ensure = "Present"
                Name = "RABBITMQ_SERVICENAME"
                Value = $Node.RabbitMqConfigs.RABBITMQ_SERVICENAME 
            }
            
            
            Environment "RABBITMQ_CONSOLE_LOG" 
            {
                Ensure = "Present"
                Name = "RABBITMQ_CONSOLE_LOG"
                Value = $Node.RabbitMqConfigs.RABBITMQ_CONSOLE_LOG 
            }
                        
            Environment "RABBITMQ_NODE_PORT" 
            {
                Ensure = "Present"
                Name = "RABBITMQ_NODE_PORT"
                Value = $Node.RabbitMqConfigs.RABBITMQ_NODE_PORT
 
            }
            
            
            Environment "RABBITMQ_CONF_ENV_FILE" 
            {
                Ensure = "Present"
                Name = "RABBITMQ_CONF_ENV_FILE"
                Value = $Node.RabbitMqConfigs.RABBITMQ_CONF_ENV_FILE
 
            }
	             
            Archive InstallRabbitMq {
                Ensure = "Present"
                Path = $Node.RabbitMqConfigs.ArchivePath
                Destination = "$env:ProgramFiles"
               
            }
            

            
            Write-Host "Installing RabbitMq Service"

            Script InstallService {
                DependsOn = "[Archive]InstallRabbitMq"
                GetScript =                 {
                
                    return @{
                        RabbitMqService = (Get-Service -Name "rabbitmq*" -ErrorAction SilentlyContinue)
                    };
                }
                SetScript = {
                    # install-erlang should have set this value
                    $env:ERLANG_HOME = [System.Environment]::GetEnvironmentVariable('ERLANG_HOME','Machine')

                    if((Test-Path("c:\temprx")) -eq $false){                        
                         mkdir "c:\temprx";
                    }

                    "`"$env:ProgramFiles\rabbitmq_server-3.6.5\sbin\rabbitmq-service.bat`" install" `
                    | Out-File -Encoding ascii -Force -FilePath "c:\temprx\install.rabbitmq.service.bat"
                
                    & c:\temprx\install.rabbitmq.service.bat
                    Write-Verbose -Message "Wait for 5 seconds in order for the service to be installed" 
                    Start-Sleep -Seconds 2
                    #Write-Verbose $install

                    rmdir "c:\temprx" -Recurse -Force

                }
                TestScript = {
                    $RabbitMqService = Get-Service -Name "rabbitmq*" -ErrorAction SilentlyContinue
                    Write-Verbose ("RabbitMq `service " + $RabbitMqService)
                    if($RabbitMqService -eq  $null){
                        return $false
                    }

                    return $true
                }
            }
     

          

            Write-Host "Starting RabbitMq service"
            Service StartRabbitMqService {
                Ensure      = "Present"
                Name        = "RabbitMq"
                State       = "Running"
                StartupType = 'Automatic'
                DependsOn   = '[Script]InstallService'
            }


            Write-Host "Firewall rule for RabbitMq"
            # TODO : only allows connection from workers and web server only
            xFirewall EnableBrokerPort{  
                    Name                  = "RabbitMqBrokerFirewallRule"
                    DisplayName           = "Firewall Rule for RabbitMq service"
                    Ensure                = "Present"
                    Enabled               = "True"
                    Direction             = "InBound"
                    LocalPort             = (5671, $Node.RabbitMqConfigs.BrokerPort)
                    Protocol              = "TCP"
                    Description           = "Firewall Rule for RabbitMq broker"            
                
            }

            #4369
            xFirewall EnableEpmdPort{  
                    Name                  = "EpmdFirewallRule"
                    DisplayName           = "Firewall Rule for RabbitMq clustering"
                    Ensure                = "Present"
                    Enabled               = "True"
                    Direction             = "InBound"
                    LocalPort             = (4369)
                    Protocol              = "TCP"
                    Description           = "Firewall Rule for erlang"            
                
            }

            
            xFirewall ErlangCLI{  
                    Name                  = "ErlangCLI"
                    DisplayName           = "Firewall Rule for RabbitMq clustering"
                    Ensure                = "Present"
                    Enabled               = "True"
                    Direction             = "InBound"
                    LocalPort             = (25672)
                    Protocol              = "TCP"
                    Description           = "Firewall Rule for erlang"            
                
            }

            
            xFirewall RabbitMqManagement{  
                    Name                  = "RabbitMqManagement"
                    DisplayName           = "Firewall Rule for RabbitMq clustering"
                    Ensure                = "Present"
                    Enabled               = "True"
                    Direction             = "InBound"
                    LocalPort             = ($Node.RabbitMqConfigs.ManagementPort)
                    Protocol              = "TCP"
                    Description           = "Firewall Rule for erlang"            
                
            }

        

            #TODO : username and password


            #TODO : clustering and shit

            #  rabbitmqctl rotate_logs
            # https://www.rabbitmq.com/install-windows.html


	    }

     
}

InstallRabbitMq -OutputPath .\output\rabbitmq -ConfigurationData .\config.psd1

# Start-DscConfiguration -ComputerName S202 -Path C:\Scripts\PushDscCustomResources –Verbose -Wait -Force
# Start-DscConfiguration -ComputerName S305 -Path .\output\rabbitmq –Verbose -Wait -Force

$configs = Import-PowerShellDataFile .\config.psd1
$AllNodes = $configs.AllNodes

foreach($Node in $AllNodes)
{
    if(($Node.Roles.IndexOf("RabbitMq") -gt -1))
    {
        Start-DscConfiguration -ComputerName $Node.NodeName -Path .\output\rabbitmq –Verbose -Wait -Force
    }
}