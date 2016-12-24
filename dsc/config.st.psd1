@{
    # Node specific data
    AllNodes = @(
        @{
            NodeName                    = "PMBIPTTN15"
            Roles                       = @("Web")
            Environment                 = $true
            PSDscAllowPlainTextPassword = $true
            PSDscAllowDomainUser        = $true
        }, 
        @{
            NodeName                    = "PMBIPTTN16"
            Roles                       = @("Web")
            Environment                 = $true
            PSDscAllowPlainTextPassword = $true
            PSDscAllowDomainUser        = $true
        },
        @{
            NodeName                    = "PMBIPTTN06"
            Roles                       = @("Web")
            Environment                 = $true
            PSDscAllowPlainTextPassword = $true
            PSDscAllowDomainUser        = $true
        }, 
        @{
            NodeName                    = "PMBIPTTN07"
            Roles                       = @("Web")
            Environment                 = $true
            PSDscAllowPlainTextPassword = $true
            PSDscAllowDomainUser        = $true
        }
    );

    

    NonNodeData =
    @{
        BuildServer ="\\PMBIPTTN08\Shared\posentt"
     };


     RxConfigs = @{
            Home = "c:\apps\rx"
            BaseUrl = "http://rx.pos.com.my"
            TokenSecret = "PosEnttProd2016"
            Environment = "prod"
            ApplicationName = "PosEntt"
            WebPath = "c:\apps\rx\web"
            SqlConnectionString = "Data Source=PMBIPTTSV;Initial Catalog=PosEntt;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False"
            RabbitMqUserName = "PosEnttApp"
            RabbitMqPassword = "slayer"
            RabbitMqHost = "PMBIPTTN08"
            ElasticSearchHost = "http://PMBIPTTN10:9200"
            BromConnectionString = "Data Source=PMBIPPTQS01;Initial Catalog=PittisNonCore;User Id=sa;password=p@ssword12"
            OalConnectionString = "Data Source=10.1.1.120;Initial Catalog=oal_dbo;User Id=enttadm;password=P@ssw0rd"
            SnbWebNewAccount_BaseAddress = "10.1.1.119:9002"

     }

    
}