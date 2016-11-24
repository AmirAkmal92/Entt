Configuration InstallErlang{


        Import-DscResource -Module xWebAdministration -ModuleVersion 1.14.0.0
        Import-DscResource -ModuleName xNetworking -ModuleVersion 2.12.0.0
        Import-DscResource -ModuleName rxReactiveDeveloper

        $ApplicationName = $ConfigurationData.RxConfigs["ApplicationName"];
        $RxHome = $ConfigurationData.RxConfigs["Home"];


        # erlang
        Node $AllNodes.Where({$_.Roles.Contains("RabbitMq")}).NodeName {
            Write-Host -ForegroundColor Yellow ("Setting Erlang server on " + $Node.NodeName)
	             
            rxErlang InstallErlang {
                Ensure = "Present"
                Name = "Erlang"
                Version = $Node.RabbitMqConfigs.ErlangVersion
                InstallerPath =  $Node.RabbitMqConfigs.ErlangInstallerPath
                ErlangHome = "$env:ProgramFiles\erl" +$Node.RabbitMqConfigs.ErlangVersion
               
            }

            Environment ERLANG_HOME {
                Ensure = "Present"
                Name = "ERLANG_HOME"
                Value = "$env:ProgramFiles\erl" +$Node.RabbitMqConfigs.ErlangVersion
                DependsOn = '[rxErlang]InstallErlang'
            }
            
                     

	    }

     
}

InstallErlang -OutputPath .\output\erlang -ConfigurationData .\config.psd1

# Start-DscConfiguration -ComputerName S202 -Path C:\Scripts\PushDscCustomResources –Verbose -Wait -Force
# Start-DscConfiguration -ComputerName S305 -Path .\output\erlang  –Verbose -Wait -Force

$configs = Import-PowerShellDataFile .\config.psd1
$AllNodes = $configs.AllNodes

$computers = New-Object System.Collections.ArrayList

## copy the modules
foreach($Node in $AllNodes)
{
    if(($Node.Roles.IndexOf("RabbitMq") -gt -1))
    {
        $NodeSession = New-PSSession -ComputerName $Node.NodeName
        Copy 'C:\Program Files\WindowsPowerShell\Modules\rxReactiveDeveloper' -Force -Recurse -ToSession $NodeSession 'C:\Program Files\WindowsPowerShell\Modules'
        Copy 'C:\Program Files\WindowsPowerShell\Modules\xNetworking' -Force -Recurse -ToSession $NodeSession 'C:\Program Files\WindowsPowerShell\Modules'        
        Copy 'C:\Program Files\WindowsPowerShell\Modules\xWebAdministration' -Force -Recurse -ToSession $NodeSession 'C:\Program Files\WindowsPowerShell\Modules'
    }
}



foreach($Node in $AllNodes)
{
    if(($Node.Roles.IndexOf("RabbitMq") -gt -1))
    {
        Start-DscConfiguration -ComputerName $Node.NodeName -Path .\output\erlang  –Verbose -Wait -Force
    }
}