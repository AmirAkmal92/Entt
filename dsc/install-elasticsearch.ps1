Configuration InstallElasticsearch{


        Import-DscResource -Module xWebAdministration -ModuleVersion 1.14.0.0
        Import-DscResource -ModuleName xNetworking -ModuleVersion 2.12.0.0
        Import-DscResource -ModuleName rxReactiveDeveloper
        Import-DscResource -ModuleName PSDesiredStateConfiguration

        $ApplicationName = $ConfigurationData.RxConfigs["ApplicationName"];
        $RxHome = $ConfigurationData.RxConfigs["Home"];


        # elasticsearch
        Node $AllNodes.Where({$_.Roles.Contains("Elasticsearch")}).NodeName {
            Write-Host -ForegroundColor Yellow ("Setting Elasticsearch server on " + $Node.NodeName)
	             
            File Jdk{
                Ensure = "Present" 
                Type = "Directory“ # Default is “File”
                Force = $True
                Recurse = $True
                SourcePath =  $ConfigurationData.NonNodeData["BuildServer"] + "\jdk" + $Node.ElasticsearchConfigs.JdkVersion
                DestinationPath = "$env:ProgramFiles\jdk" + $Node.ElasticsearchConfigs.JdkVersion
            }

            Environment JAVA_HOME {
                Ensure = "Present"
                Name = "JAVA_HOME"
                Value = "$env:ProgramFiles\jdk" +$Node.ElasticsearchConfigs.JdkVersion
            }
            
            Archive ElasticsearchZip {
                Ensure = "Present"
                Destination = "$env:ProgramFiles\Elasticsearch"
                Path = $ConfigurationData.NonNodeData["BuildServer"] + "\elasticsearch-" +  $Node.ElasticsearchConfigs.Version +".zip"
                 
            }

            <#
            $esconfig = $ConfigurationData.NonNodeData["BuildServer"] + "\elasticsearch.yml"
            Write-Host "Reading $esconfig"
            $esconfig2 = ( (Get-Content $esconfig).Replace("network.host: 192.168.0.1", "network.host: " + $Node.ElasticsearchConfigs.IpAddress))
        

            $esconfig3 = "";
            foreach($line in $esconfig2){
                $esconfig3 = $esconfig3 + "`r`n" + $line

            }
            Write-Host $esconfig3
            File ElasticsearchConfig {
                DependsOn = "[Archive]ElasticsearchZip"
                DestinationPath = "$env:ProgramFiles\Elasticsearch\elasticsearch-" + $Node.ElasticsearchConfigs.Version +"\config\elasticsearch.yml"
                Contents =$esconfig3
                Ensure = "Present"
                Type = 'File'
                MatchSource = $True
                Force = $True

            }
            #>

            Write-Host "Installing elasticsearch service"

            Script InstallService {
                DependsOn = "[Archive]ElasticsearchZip"
                GetScript =                 {
                
                    return @{
                        ElasticsearchService = (Get-Service -Name "elasticsearch*" -ErrorAction SilentlyContinue)
                    };
                }
                SetScript = {
                    $service_bat = (& ("$env:ProgramFiles\Elasticsearch\elasticsearch-1.7.5\bin\service.bat") "install")
                    # Start-DscConfiguration -ComputerName S202 -Path C:\Scripts\InstallIIS –Verbose -Wait -Force

                }
                TestScript = {
                    $ElasticsearchService = Get-Service -Name "elasticsearch*" -ErrorAction SilentlyContinue
                    Write-Verbose ("Elasticsearch service " + $ElasticsearchService)
                    if($ElasticsearchService -eq  $null){
                        return $false
                    }

                    return $true
                }
            }

            Write-Host "Starting elasticsearch service"
            Service StartElasticsearchService {
                Ensure = "Present"
                Name = "elasticsearch-service-x64"
                State = "Running"
                StartupType = 'Automatic'
                DependsOn = '[Script]InstallService'
            }


            Write-Host "Firewall rule for elasticsearh"
            # TODO : only allows connection from workers and web server only
            xFirewall EnableHttpPort{  
                    Name                  = "ElasticsearchFirewallRule"
                    DisplayName           = "Firewall Rule for Elasticsearch service"
                    Ensure                = "Present"
                    Enabled               = "True"
                    Direction             = "InBound"
                    LocalPort             = ($Node.ElasticsearchConfigs.HttpPort)
                    Protocol              = "TCP"
                    Description           = "Firewall Rule for Elasticsearch"
            
                
            }

            # TODO : Shards and clustering config
            
            # TODO - Create the system index and populate the Sph object mapping
            # Time to write custom resource as the configuraion is only available durin the MEF compilation only   
            # http://stackoverflow.com/questions/23346901/powershell-dsc-how-to-pass-configuration-parameters-to-scriptresources         
            <# 
            curl http://S202:9200/myapp -Method Delete | % content
            curl http://S202:9200/myapp_system -Method Delete | % content

            Start-DscConfiguration -ComputerName S202 -Path C:\Scripts\InstallIIS –Verbose -Wait -Force

            
            (ConvertFrom-Json(curl http://S202:9200/myapp/_mapping | % content)).myapp.mappings
            
            (ConvertFrom-Json(curl http://S202:9200/myapp_system/_mapping | % content)).myapp.mappings

            #>
            $applicationNameLowered =  $ConfigurationData.RxConfigs["ApplicationName"].ToLowerInvariant();
            EsIndex CreateSystemIndex{
              Ensure = "Present"
              Name =  $applicationNameLowered + "_system"
              Url = "http://" + $Node.NodeName + ":" + $Node.ElasticsearchConfigs.HttpPort
              TypeMappingPath = $ConfigurationData.NonNodeData["BuildServer"] + "\database\mapping"

            }

            EsIndex CreateApplicationIndex{
              Ensure = "Present"
              Name =  $applicationNameLowered
              Url = "http://" + $Node.NodeName + ":" + $Node.ElasticsearchConfigs.HttpPort
              TypeMappingPath = $ConfigurationData.NonNodeData["BuildServer"] + "\sources\EntityDefinition"

            }

            # TODO - Create the application index and populate Custom entity mapping

            # TODO - where the audit trail and logging goes

	    }

     
}

InstallElasticsearch -OutputPath .\output\elasticsearch -ConfigurationData .\config.psd1


# Start-DscConfiguration -ComputerName S305 -Path .\output\elasticsearch –Verbose -Wait -Force