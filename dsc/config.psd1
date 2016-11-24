@{
    # Node specific data
    AllNodes = @(
      
       @{
            NodeName                    = "S001"
            Roles                       = @("Web", "Console", "Worker")
            Environment                 = $true
            PSDscAllowPlainTextPassword = $true
            PSDscAllowDomainUser        = $true
       },
       @{
            NodeName           = "S301"
            Roles              = @("SqlServer")
            Environment        = $false
        },
        @{
            NodeName          = "S305"
            Roles             = @("Elasticsearch", "RabbitMq")
            Environment       = $false
            ElasticsearchConfigs = @{
                IpAddress           = "192.168.182"
                HttpPort            = "9200"
                JdkVersion          = "1.8.0_102"
                Version             = "1.7.5"
            }
            RabbitMqConfigs   = @{
                ErlangVersion       = "8.0"
                ErlangInstallerPath = "\\WS27\Shared\installers\otp_win64_19.1.exe"
                Version             = "3.6.5"
                ArchivePath         = "\\WS27\Shared\installers\rabbitmq-server-windows-3.6.5.zip"
                BrokerPort          = 5672
                ManagementPort      = 15672
                BaseDirectory       = "c:\rabbitmq_base"
                
            }
        }, 
        @{
            NodeName          = "S322"
            Roles             = @("RabbitMq")
            Environment       = $false
            RabbitMqConfigs   = @{
                ErlangVersion       = "8.0"
                ErlangInstallerPath = "\\WS27\Shared\installers\otp_win64_19.1.exe"
                Version             = "3.6.5"
                ArchivePath         = "\\WS27\Shared\installers\rabbitmq-server-windows-3.6.5.zip"
                BrokerPort          = 5672
                ManagementPort      = 15672
                BaseDirectory       = "c:\rabbitmq_base"
                RABBITMQ_NODENAME   = "S322"
                RABBITMQ_SERVICENAME= "RabbitMQ"
                RABBITMQ_CONSOLE_LOG= "new"
                RABBITMQ_NODE_PORT  = 5672
                RABBITMQ_CONF_ENV_FILE="c:\apps\rabbitmq\rabbitmq-env-conf.bat"
                
            }
        },
        @{
            NodeName          = "S323"
            Roles             = @("RabbitMq")
            Environment       = $false
            RabbitMqConfigs   = @{
                ErlangVersion       = "8.0"
                ErlangInstallerPath = "\\WS27\Shared\installers\otp_win32_19.1.exe"
                Version             = "3.6.5"
                ArchivePath         = "\\WS27\Shared\installers\rabbitmq-server-windows-3.6.5.zip"
                BrokerPort          = 5672
                ManagementPort      = 15672
                BaseDirectory       = "c:\rabbitmq_base"
                RABBITMQ_NODENAME   = "S323"
                RABBITMQ_SERVICENAME= "RabbitMQ"
                RABBITMQ_CONSOLE_LOG= "new"
                RABBITMQ_NODE_PORT  = 5672
                RABBITMQ_CONF_ENV_FILE="c:\apps\rabbitmq\rabbitmq-env-conf.bat"
                
            }
        },
        @{
            NodeName          = "S308"
            Roles             = @("Worker")
            Environment       = $true
            Instances = @(
                                @{
                                    Name = "core"
                                    Config = "core"
                                },
                                @{
                                    Name = "persistence1"
                                    Config = "persistence"
                                },
                                @{
                                    Name = "persistence2"
                                    Config = "persistence"
                                },
                                @{
                                    Name = "snb"
                                    Config = "snb"
                                }
            )
            PSDscAllowPlainTextPassword = $true
            PSDscAllowDomainUser        = $true
        },
        @{
            NodeName          = "S309"
            Roles             = @("Worker")
            Environment       = $true
            PSDscAllowPlainTextPassword = $true
            PSDscAllowDomainUser        = $true
            Instances = @()
        }
    );

    

    NonNodeData =
    @{
        BuildServer ="\\WIN2012-RX10323\Shared\10323"
     };


     RxConfigs = @{
            Home = "c:\apps\rx"
            ApplicationName = "DevV1"
            WebPath = "c:\apps\rx\web"
            SqlConnectionString = "Data Source=S301\DEV2016;Initial Catalog=DevV1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False"
            RabbitMqUserName = "rx-myapp"
            RabbitMqPassword = "mypassword"
            RabbitMqHost = "S305"
            ElasticSearchHost = "http://S305:9200"
            BromConnectionString = "Data Source=S301\DEV2016;Initial Catalog=PittisNonCore;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
            SnbWebNewAccount_BaseAddress = "http://eryken2.asuscomm.com:8086"


     }

    
}