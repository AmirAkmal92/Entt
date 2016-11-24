[Diagnostics.CodeAnalysis.SuppressMessageAttribute("PSAvoidUsingConvertToSecureStringWithPlainText", "")]
param ()

Configuration SQLNetwork
{
    Import-DscResource -Module xSQLServer

    # A Configuration expects at least one Node
    Node $AllNodes.NodeName
    {
        # Set DCM Settings for each Node 
        LocalConfigurationManager 
        { 
            RebootNodeIfNeeded = $true 
            ConfigurationMode = "ApplyOnly" 
        } 

        <#
        WindowsFeature "NET-Framework-Core"
        {
            Ensure = "Present"
            Name = "NET-Framework-Core"
        }
        #>
        xSqlServerSetup "RDBMS"
        {
            #DependsOn = @("[WindowsFeature]NET-Framework-Core")
            SourcePath = $Node.SourcePath
            SourceFolder = $Node.SQL2012FolderPath
            InstanceName = $Node.Instance
            Features = $Node.Features
            SetupCredential = $Node.InstallerServiceAccount
            SQLCollation = "Latin1_General_CI_AS"
            SQLSysAdminAccounts = $Node.AdminAccount
            #SQLSvcAccount = $Node.LocalSystemAccount
            #AgtSvcAccount = $Node.LocalSystemAccount
        }

        xSqlServerFirewall "RDBMS"
        {
            DependsOn = @("[xSqlServerSetup]RDBMS")
            SourcePath = $Node.SourcePath
            SourceFolder = $Node.SQL2012FolderPath
            InstanceName = $Node.Instance
            Features = $Node.Features
        }

        # This will enable TCP/IP protocol and set custom static port, this will also restart sql service
        xSQLServerNetwork "RDBMS"
        {
            DependsOn = @("[xSqlServerSetup]RDBMS")
            InstanceName = $Node.Instance
            ProtocolName = "tcp"
            IsEnabled = $true
            TCPPort = 4509
            RestartService = $true 
        }        
    } 
}



if($InstallerServiceAccount -eq $null){
    $InstallerServiceAccount = Get-Credential "ORG\Administrator"
}
if($LocalSystemAccount -eq $null){
    $LocalSystemAccount = Get-Credential "ORG\sqldev2016"
}

$ConfigurationData = @{
    AllNodes = @(
        @{
            NodeName = "*"
            PSDscAllowPlainTextPassword = $true
            SourcePath = "D:\"
            SQL2012FolderPath = ""
            InstallerServiceAccount = $InstallerServiceAccount
            LocalSystemAccount = $LocalSystemAccount
            AdminAccount = "ORG\Administrator"

        }
        @{
            NodeName = "S048"
            Instance = "DEV2016"
            Features = "SQLENGINE"
        }
    )
}


SQLNetwork -ConfigurationData $ConfigurationData -OutputPath C:\Scripts\src\SQLSA
# Start-DscConfiguration -ComputerName S048 -Path C:\Scripts\src\SQLSA -Verbose -Wait -Force

