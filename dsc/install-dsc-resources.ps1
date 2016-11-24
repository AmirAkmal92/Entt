Configuration PushDscCustomResources{
    
        #web 
        Node $AllNodes.Where({$_.Roles -ne $null}).NodeName {
            Write-Host -ForegroundColor Yellow ("Pushing DSC resources on " + $Node.NodeName)

            Write-Host -ForegroundColor Yellow ("Pushing DSC resources on " + $Node.NodeName)
		                      
            File RemoverxReactiveDeveloper {
                Ensure = "Absent" 
                Type = "Directory“
                Force = $True
                Recurse = $True
                DestinationPath = "C:\Program Files\WindowsPowerShell\Modules\rxReactiveDeveloper" 
            }

		                      
            File RxApp {
                Ensure = "Present" 
                Type = "Directory“
                Force = $True
                Recurse = $True
                SourcePath =  $ConfigurationData.NonNodeData["BuildServer"] + "\DscResources"
                DestinationPath = "C:\Program Files\WindowsPowerShell\Modules" 
            }
     


	    }
    
}

PushDscCustomResources -OutputPath C:\Scripts\PushDscCustomResources -ConfigurationData c:\scripts\src\config.psd1

<# 

    rmdir -Path 'C:\Program Files\WindowsPowerShell\Modules\rxReactiveDeveloper' -Recurse -Force  -ErrorAction SilentlyContinue
    rmdir -Path '\\WIN2012-RX10323\Shared\10323\DSCResources\rxReactiveDeveloper' -Recurse -Force  -ErrorAction SilentlyContinue
    
    Copy-item -Path C:\Scripts\src\rxReactiveDeveloper -Destination 'C:\Program Files\WindowsPowerShell\Modules' -Recurse -Force
    Copy-item -Path C:\Scripts\src\rxReactiveDeveloper -Destination '\\WIN2012-RX10323\Shared\10323\DscResources' -Recurse -Force
    Start-DscConfiguration -ComputerName S202 -Path C:\Scripts\PushDscCustomResources –Verbose -Wait -Force

#>