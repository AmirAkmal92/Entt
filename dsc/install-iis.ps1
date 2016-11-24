<#

 for web we also need to have output folder
#>

Configuration InstallIIS {
 param
        (
            [PSCredential] $AppPoolCredential
        )
        

        Import-DscResource -Module xWebAdministration -ModuleVersion 1.14.0.0
        Import-DscResource -ModuleName xNetworking -ModuleVersion 2.12.0.0
        Import-DscResource -ModuleName PSDesiredStateConfiguration

        $ApplicationName = $ConfigurationData.RxConfigs["ApplicationName"];
        $RxHome = $ConfigurationData.RxConfigs["Home"];


        #web 
        Node $AllNodes.Where({$_.Roles.IndexOf("Web") -gt -1}).NodeName {
            Write-Host -ForegroundColor Yellow ("Setting Web server on " + $Node.NodeName)
		    WindowsFeature IIS {
			    Ensure = 'Present'
			    Name   = 'web-server'
		    }

            WindowsFeature IISMgmt {
			    Ensure = 'Present'
			    Name   = 'web-Mgmt-Service'
                DependsOn = "[WindowsFeature]IIS"
		    }

            WindowsFeature AspNet45 {
                Ensure          = "Present"
                Name            = "Web-Asp-Net45"
            }
            

			Package UrlRewrite
			{
				#Install URL Rewrite module for IIS
				DependsOn = "[WindowsFeature]IIS"
				Ensure = "Present"
				Name = "IIS URL Rewrite Module 2"
				Path = "https://download.microsoft.com/download/C/9/E/C9E8180D-4E51-40A6-A9BF-776990D8BCA9/rewrite_amd64.msi"
				Arguments = "/quiet"
				ProductId = "08F0318A-D113-4CF0-993E-50F191D397AD"
			}

			Script ReWriteRules
			{
				#Adds rewrite allowedServerVariables to applicationHost.config
				SetScript = {
					$current = Get-WebConfiguration /system.webServer/rewrite/allowedServerVariables | select -ExpandProperty collection | ?{$_.ElementTagName -eq "add"} | select -ExpandProperty name
					$expected = @("HTTPS", "HTTP_X_FORWARDED_FOR", "HTTP_X_FORWARDED_PROTO", "REMOTE_ADDR")
					$missing = $expected | where {$current -notcontains $_}
					try
					{
						Start-WebCommitDelay 
						$missing | %{ Add-WebConfiguration /system.webServer/rewrite/allowedServerVariables -atIndex 0 -value @{name="$_"} -Verbose }
						Stop-WebCommitDelay -Commit $true 
					} 
					catch [System.Exception]
					{ 
						$_ | Out-String
					}
				}
				TestScript = {
					$current = Get-WebConfiguration /system.webServer/rewrite/allowedServerVariables | select -ExpandProperty collection | select -ExpandProperty name
					$expected = @("HTTPS", "HTTP_X_FORWARDED_FOR", "HTTP_X_FORWARDED_PROTO", "REMOTE_ADDR")
					$result = -not @($expected| where {$current -notcontains $_}| select -first 1).Count
					return $result
				}
				GetScript = {
					$allowedServerVariables = Get-WebConfiguration /system.webServer/rewrite/allowedServerVariables | select -ExpandProperty collection
					return $allowedServerVariables
				}
				DependsOn = "[Package]UrlRewrite"
			}
                    
            File RxAppSource {
                Ensure = "Present" 
                Type = "Directory“
                Force = $True
                Recurse = $True
                SourcePath =  $ConfigurationData.NonNodeData["BuildServer"] + "\sources"
                DestinationPath = $ConfigurationData.RxConfigs["Home"]  + "\sources"
                DependsOn = "[WindowsFeature]IIS"  
            }
            
            File RxAppWeb {
                Ensure = "Present" 
                Type = "Directory“
                Force = $True
                Recurse = $True
                SourcePath =  $ConfigurationData.NonNodeData["BuildServer"] + "\web"
                DestinationPath = $ConfigurationData.RxConfigs["Home"]  + "\web"
                DependsOn = "[WindowsFeature]IIS"  
            }
            
            xWebsite DefaultSite {
                Ensure          = "Absent"
                Name            = "Default Web Site"
                State           = "Stopped"
                PhysicalPath    = "C:\inetpub\wwwroot"
                DependsOn       = "[File]RxAppWeb"
            }

              xWebAppPool RxWebPool {
                Ensure       = "Present"
                Name         = "RxWebPool"
                State        = "Started"
                managedRuntimeVersion = "v4.0"
                identityType = "SpecificUser"
                Credential = $AppPoolCredential
                DependsOn = "[xWebsite]DefaultSite"
            }
     
            xWebSite RxWeb{
                Ensure = "Present"
                Name =  $ConfigurationData.RxConfigs["ApplicationName"]
                PhysicalPath = $ConfigurationData.RxConfigs["WebPath"]
                ApplicationPool = "RxWebPool"
                State = "Started"            
                BindingInfo     = @(
                    MSFT_xWebBindingInformation
                    {
                        Protocol              = "HTTP"
                        Port                  = 8080
                    }
                )
                DependsOn = "[xWebAppPool]RxWebPool"
            }


            xFirewall EnableWebAppHttpPort{  
                    Name                  = "RxWebApp"
                    DisplayName           = "Firewall Rule for RxWebApp service"
                    Ensure                = "Present"
                    Enabled               = "True"
                    Direction             = "InBound"
                    LocalPort             = (8080)
                    Protocol              = "TCP"
                    Description           = "Firewall Rule for RxWebApp"            
                
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
    $webPoolCred = Get-Credential -UserName ORG\web-snb -Message "Enter the credential for SnbWebpool application pool"
}

InstallIIS  -AppPoolCredential $webPoolCred -OutputPath ".\output\install-iis" -ConfigurationData .\config.psd1



Set-ItemProperty -Path $rws -Name "EnableRemoteManagement" -Value "1"
Get-Service WMSVC | Set-Service -StartupType Automatic | Start-Service
#>

# Start-DscConfiguration -ComputerName S001 -Path ".\output\install-iis" –Verbose -Wait -Force