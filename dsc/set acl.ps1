
$Acl = Get-Acl "C:\apps\rx"
$Ar = New-Object  System.Security.AccessControl.FileSystemAccessRule("ORG\web-snb","FullControl","Allow")
$Acl.SetAccessRule($Ar)
Set-Acl "C:\apps\rx" $Acl

try {
$r = Invoke-WebRequest -Uri "http://s001.org.1mdb:8080/ " -TimeoutSec 180 -ErrorAction Continue
} catch {

}

Get-EventLog -LogName Application -EntryType Warning