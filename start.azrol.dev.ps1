$env:RX_POSENTT_OalConnectionString="Data Source=(localdb)\Projectsv13;Initial Catalog=oal;Integrated Security=True;Connect Timeout=30"
$env:RX_POSENTT_SowRtsJwtToken="eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImNhbl9lZGl0X2VudGl0eSIsImNhbl9lZGl0X3dvcmtmbG93IiwiZGV2ZWxvcGVycyJdLCJlbWFpbCI6ImFkbWluQHBvc2VudHQuY29tIiwic3ViIjoiNjM2MjQxNTI1MjkxNzc4Mzc5MWU4MWY2YWQiLCJuYmYiOjE1MDQ0MjQ1MjksImlhdCI6MTQ4ODUyNjkyOSwiZXhwIjoxNDkzNTEwNDAwLCJhdWQiOiJQb3NFbnR0In0.OfA4jTbBEz5LSZTCse4gb-km4_l-V4D_g9dptpxr7aA"
#$env:RX_POSENTT_SowRtsDueTime="10"
#$env:RX_POSENTT_SowRtsPeriod="1"
$env:RX_POSENTT_VasnRetry="3"
$env:RX_POSENTT_VasnInternal="100"
$env:RX_POSENTT_BaseUrl="http://localhost:8080/"

Start-Process "devenv" -ArgumentList .\posentt.sln