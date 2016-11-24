# deploy to win2012-rx10323/shared/10323
 param
        (
            [string] $binFolder,
            [string] $deployFolder
        )
# clear all $deployFolder
ls $deployFolder\output -Recurse -Force