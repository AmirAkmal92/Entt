@{
    # Node specific data
    AllNodes = @(
      
        @{
            NodeName          = "PMBIPTTN10"
            Roles             = @("Elasticsearch")
            Environment       = $false
            ElasticsearchConfigs = @{
                IpAddress           = "192.168.206"
                HttpPort            = "9200"
                JdkVersion          = "1.8.0_40"
                Version             = "1.7.5"
            }
        },  @{
            NodeName          = "PMBIPTTN11"
            Roles             = @("Elasticsearch")
            Environment       = $false
            ElasticsearchConfigs = @{
                IpAddress           = "192.168.207"
                HttpPort            = "9200"
                JdkVersion          = "1.8.0_40"
                Version             = "1.7.5"
            }
        }, @{
            NodeName          = "PMBIPTTN12"
            Roles             = @("Elasticsearch")
            Environment       = $false
            ElasticsearchConfigs = @{
                IpAddress           = "192.168.208"
                HttpPort            = "9200"
                JdkVersion          = "1.8.0_40"
                Version             = "1.7.5"
            }
        }
    );

    

    NonNodeData =
    @{
        BuildServer ="\\WS27\Shared"
     };


     RxConfigs = @{
            Home = "c:\apps\rx"
            ApplicationName = "PosEntt"
            WebPath = "c:\apps\rx\web"
            SqlConnectionString = "Data Source=S301\DEV2016;Initial Catalog=DevV1;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False"
            RabbitMqUserName = "PosEnttApp"
            RabbitMqPassword = "megadeth"
            RabbitMqHost = "S305"
            ElasticSearchHost = "http://S305:9200"
            BromConnectionString = "Data Source=S301\DEV2016;Initial Catalog=PittisNonCore;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
            SnbWebNewAccount_BaseAddress = "http://eryken2.asuscomm.com:8086"


     }

    
}