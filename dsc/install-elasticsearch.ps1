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
                SourcePath =  $ConfigurationData.NonNodeData["BuildServer"] + "\installers\jdk" + $Node.ElasticsearchConfigs.JdkVersion
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
                Path = $ConfigurationData.NonNodeData["BuildServer"] + "\installers\elasticsearch-" +  $Node.ElasticsearchConfigs.Version +".zip"
                 
            }



            Write-Host "Firewall rule for elasticsearh"
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
            xFirewall ElasticsearchClusterPorts{  
                    Name                  = "ElasticsearchClusterRule"
                    DisplayName           = "Firewall Rule for Elasticsearch clusterservice"
                    Ensure                = "Present"
                    Enabled               = "True"
                    Direction             = "InBound"
                    LocalPort             = ("9300-9400")
                    Protocol              = "TCP"
                    Description           = "Firewall Rule for Elasticsearch"            
                
            }

     


	    }

     
}

InstallElasticsearch -OutputPath .\output\elasticsearch -ConfigurationData .\config.psd1


$configs = Import-PowerShellDataFile .\config.psd1
$AllNodes = $configs.AllNodes



## copy the modules
foreach($Node in $AllNodes)
{
    if(($Node.Roles.IndexOf("Elasticsearch") -gt -1))
    {
        $NodeSession = New-PSSession -ComputerName $Node.NodeName
        Copy 'C:\Program Files\WindowsPowerShell\Modules\rxReactiveDeveloper' -Force -Recurse -ToSession $NodeSession 'C:\Program Files\WindowsPowerShell\Modules'
        Copy 'C:\Program Files\WindowsPowerShell\Modules\xNetworking' -Force -Recurse -ToSession $NodeSession 'C:\Program Files\WindowsPowerShell\Modules'        
        Copy 'C:\Program Files\WindowsPowerShell\Modules\xWebAdministration' -Force -Recurse -ToSession $NodeSession 'C:\Program Files\WindowsPowerShell\Modules'
    }
}


foreach($Node in $AllNodes)
{
    if(($Node.Roles.IndexOf("Elasticsearch") -gt -1))
    {
        $Server = $Node.NodeName
        Write-Host "Starting DSC configuration on $Server" -ForegroundColor Yellow
        Start-DscConfiguration -ComputerName $Server -Path .\output\elasticsearch –Verbose -Wait -Force
    }
}