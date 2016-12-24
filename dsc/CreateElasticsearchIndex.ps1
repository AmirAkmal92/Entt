##indices
#2.postentt_sys
$ElasticSearchHost = "http://PMBIPTTN10:9200"
$ApplicationName = "PosEntt"

$esindex = $ElasticSearchHost + "/" + $ApplicationName.ToLowerInvariant() + "_sys"
Invoke-WebRequest -Method Put -Body "" -Uri $esindex  -ContentType "application/javascript" -UseBasicParsing

Get-ChildItem -Filter *.json -Path .\database\mapping `
| %{
    $mappingUri = $esindex + "/" + $_.Name.ToLowerInvariant().Replace(".json", "") + "/_mapping"
    Write-Debug "Creating elastic search mapping for $mappingUri"
    Invoke-WebRequest -Method PUT -Uri $mappingUri -InFile $_.FullName -ContentType "application/javascript" -UseBasicParsing
}

# templates
Get-ChildItem -Filter *.template -Path .\database\mapping `
| %{
    $templateName = $_.Name.ToLowerInvariant().Replace(".template", "")
    $templateUri = "$ElasticSearchHost/_template/$templateName"
    $templateContent = Get-Content $_.FullName

    Write-Debug "Creating elasticsearch index template for $templateName"
    Invoke-WebRequest -Method PUT -Uri $templateUri -ContentType "application/javascript" -Body $templateContent -UseBasicParsing
}


#1.postentt
$customIndex = $ElasticSearchHost + "/" + $ApplicationName.ToLowerInvariant()
#Invoke-WebRequest -Method Put -Body "" -Uri $customIndex  -ContentType "application/javascript" -UseBasicParsing

Get-ChildItem -Filter *.mapping -Path .\sources\EntityDefinition `
| %{
    Write-Host $_ -ForegroundColor Yellow
    $mappingUri = $customIndex + "/" + $_.Name.ToLowerInvariant().Replace("-", "").Replace(".mapping", "") + "/_mapping"
    Write-Debug "Creating elastic search mapping for $mappingUri"
    Invoke-WebRequest -Method PUT -Uri $mappingUri -InFile $_.FullName -ContentType "application/javascript" -UseBasicParsing
}


Get-ChildItem -Filter *.mapping -Path .\sources\EntityDefinition | measure | % Count

