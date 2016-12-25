
Param(
       [switch]$Install = $false,
       [switch]$Uninstall = $false,
       [string]$DropFolder = 'd:\temp\ipos-deposit-drop',
       [string]$BaseUrl="http://localhost:8080"
     )
     
$env:RX_PosEntt_BaseUrl="$BaseUrl"
$env:RX_PosEntt_IposDepositFileDrop_ArchiveLocation=""
$env:RX_PosEntt_IposDepositFileDrop_JwtToken="eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VyIjoiYWRtaW4iLCJyb2xlcyI6WyJhZG1pbmlzdHJhdG9ycyIsImNhbl9lZGl0X2VudGl0eSIsImNhbl9lZGl0X3dvcmtmbG93IiwiZGV2ZWxvcGVycyJdLCJlbWFpbCI6ImFkbWluQHlvdXJjb21wYW55LmNvbSIsInN1YiI6IjYzNjE3OTAxNzkzMDQ3ODk1MDQxOTRhMjExIiwibmJmIjoxNDk4MDAwOTkzLCJpYXQiOjE0ODIyNzYxOTMsImV4cCI6MTQ4MzE0MjQwMCwiYXVkIjoiUG9zRW50dCJ9.yheGBPR0T-tMuJdarF62lZxb4mHxlWS3Z9b4-IsYfgY"
$env:RX_PosEntt_IposDepositFileDrop_Path=$DropFolder
if($Install.IsPresent){

	& .\PosEntt.ReceiveLocation.IposDepositFileDrop.exe install
	return;

}
if($Uninstall.IsPresent){

	& .\PosEntt.ReceiveLocation.IposDepositFileDrop.exe uninstall
	return;

}

& .\PosEntt.ReceiveLocation.IposDepositFileDrop.exe run
