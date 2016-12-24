$S13 = New-PSSession -ComputerName PMBIPTTN13
copy .\utils -Recurse -ToSession $S13 c:\apps\rx
copy .\sources\WorkersConfig\prod.rtslogs.json -ToSession $S13 c:\apps\rx\sources\WorkersConfig


$S14 = New-PSSession -ComputerName PMBIPTTN14
copy .\utils -Recurse -ToSession $S14 c:\apps\rx
copy .\sources\WorkersConfig\prod.rtslogs.json -ToSession $S14 c:\apps\rx\sources\WorkersConfig



$S15 = New-PSSession -ComputerName PMBIPTTN15
copy .\utils -Recurse -ToSession $S15 c:\apps\rx


$S16 = New-PSSession -ComputerName PMBIPTTN16
copy .\utils -Recurse -ToSession $S16 c:\apps\rx


$ApplicationName = "PosEntt"


& .\utils\mru.exe -u admin2 -p 1q@W#E4r1q -e admin2@$ApplicationName.com -r administrators -r developers -r can_edit_entity -r can_edit_workflow -c ".\web\Web.config" -env -app $ApplicationName