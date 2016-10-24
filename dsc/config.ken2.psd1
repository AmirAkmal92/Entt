@{
    # Node specific data
    AllNodes = @(
      
       @{
            NodeName                    = "S314"
            Roles                       = @("Web")
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
        BuildServer ="\\WS27\Shared\entt.rts"
     };


     RxConfigs = @{
            Home = "c:\apps\entt.rts"
            ApplicationName = "PosEntt"
            WebPath = "c:\apps\entt.rts\web"
            SqlConnectionString = "Data Source=S301\DEV2016;Initial Catalog=PosEntt;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False"
            RabbitMqUserName = "rx-myapp"
            RabbitMqPassword = "mypassword"
            RabbitMqHost = "S305"
            ElasticSearchHost = "http://S305:9200"
            BromConnectionString = "Data Source=S301\DEV2016;Initial Catalog=PittisNonCore;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
            OalConnectionString = "Data Source=S301\DEV2016;Initial Catalog=Oal;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
            SnbWebNewAccount_BaseAddress = "http://eryken2.asuscomm.com:8086"
     }    
}