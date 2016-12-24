
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