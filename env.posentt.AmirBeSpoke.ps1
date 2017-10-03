[System.Environment]::SetEnvironmentVariable("RX_POSENTT_RabbitMqPassword","slayer", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_RabbitMqManagementPort","15672", "Process")
[System.Environment]::SetEnvironmentVariable("RX_POSENTT_RabbitMqUserName","PosEnttApp", "Process")

[System.Environment]::SetEnvironmentVariable("RX_POSENTT_OalConnectionString","Data Source=(localdb)\ProjectsV13;Initial Catalog=POSENTT;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", "Process")
[System.Environment]::SetEnvironmentVariable("RX_PosEntt_BromConnectionString","Data Source=10.1.1.118;Initial Catalog=PittisNonCoreStaging;User Id=sa;password=P@ssw0rd", "Process")

[System.Environment]::SetEnvironmentVariable("RX_PosEntt_SnbWebApi_BaseAddress","http://10.1.1.119:9002", "Process")
[System.Environment]::SetEnvironmentVariable("RX_PosEntt_SnbWebNewAccount_BaseAddress","http://10.1.1.119:9002", "Process")
[System.Environment]::SetEnvironmentVariable("RX_PosEntt_IposPemFileDrop_ArchiveLocation","R:\trial\archive", "Process")

#[System.Environment]::SetEnvironmentVariable("RX_POSENTT_SowRtsJwtToken","eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImNhbl9lZGl0X2VudGl0eSIsImNhbl9lZGl0X3dvcmtmbG93IiwiZGV2ZWxvcGVycyJdLCJlbWFpbCI6ImFkbWluQHlvdXJjb21wYW55LmNvbSIsInN1YiI6IjYzNjI3Njg0NjMwNDQ5MTA5NTc2NjU5ZDcxIiwibmJmIjoxNTA3ODcwMjMwLCJpYXQiOjE0OTIwNTkwMzAsImV4cCI6MTU3NDg5OTIwMCwiYXVkIjoiUG9zRW50dCJ9.UKMnBbUxENpK_c94nBk1PuTD9xR31Q4zCgCw1ZYVecM", "Process")
#[System.Environment]::SetEnvironmentVariable("RX_POSENTT_OalConnectionString","Data Source=10.1.1.120;Initial Catalog=oal.staging;User Id=sa;password=P@ssw0rd", "Process")

#Start-Process "devenv" -ArgumentList .\posentt.sln