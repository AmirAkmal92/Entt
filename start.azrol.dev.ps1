$env:RX_POSENTT_OalConnectionString="Data Source=(localdb)\Projectsv13;Initial Catalog=oal;Integrated Security=True;Connect Timeout=30"
$env:RX_POSENTT_SowRtsJwtToken="eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImNhbl9lZGl0X2VudGl0eSIsImNhbl9lZGl0X3dvcmtmbG93IiwiZGV2ZWxvcGVycyJdLCJlbWFpbCI6ImFkbWluQGJlc3Bva2UuY29tLm15Iiwic3ViIjoiNjM2MjMzNzcwODI2MzI3NjA3ZjU3MWE2ZGUiLCJuYmYiOjE1MDMzODk4ODMsImlhdCI6MTQ4Nzc1MTQ4MywiZXhwIjoxNDkzNTEwNDAwLCJhdWQiOiJQb3NFbnR0In0.723kvDMgsfO6MtpNuRCk4CSXF9qEEfMvZgt5csY1a-c"
#$env:RX_POSENTT_SowRtsDueTime="10"
#$env:RX_POSENTT_SowRtsPeriod="1"
$env:RX_POSENTT_VasnRetry="3"
$env:RX_POSENTT_VasnInternal="100"

Start-Process "devenv" -ArgumentList .\posentt.sln