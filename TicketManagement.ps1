Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
$defaultSrcFolderPath = 'C:\Prog\EPAM\Lab'
$srcFolderPath = Read-Host "Press enter to accept the default [$($defaultSrcFolderPath)]"
$srcFolderPath = ($defaultSrcFolderPath,$prompt)[[bool]$prompt]

$eventDirectory = Join-Path $srcFolderPath 'src\TicketManagement.EventManagement.API'
$userDirectory = Join-Path $srcFolderPath 'src\TicketManagement.UserManagement.API'
$orderDirectory = Join-Path $srcFolderPath 'src\TicketManagement.OrderManagement.API'
$mvcDirectory = Join-Path $srcFolderPath 'src\TicketManagement.MVC'


$flag = $true

While($flag)
{
    Write-Host "1-Event"
    Write-Host "2-User"
    Write-Host "3-Order"
    Write-Host "10-MVC"
    Write-Host "A-All"

    $input = Read-Host "Choose project to start"

    if ( $input -eq 1)
    {
        Start-Process -FilePath 'dotnet' -WorkingDirectory $eventDirectory -ArgumentList "run --debug"
    }
    elseif($input -eq 2)
    {
        Start-Process -FilePath 'dotnet' -WorkingDirectory $userDirectory -ArgumentList "run --debug"
    }
    elseif($input -eq 3)
    {
        Start-Process -FilePath 'dotnet' -WorkingDirectory $orderDirectory -ArgumentList "run --debug"
    }
    elseif($input -eq 10)
    {
        Start-Process -FilePath 'dotnet' -WorkingDirectory $mvcDirectory -ArgumentList "run --debug"

        start microsoft-edge:https://localhost:7019
    }
    elseif($input -eq "A")
    {
        Start-Process -FilePath 'dotnet' -WorkingDirectory $eventDirectory -ArgumentList "run --debug"
        Start-Sleep -seconds 2

        Start-Process -FilePath 'dotnet' -WorkingDirectory $userDirectory -ArgumentList "run --debug"
        Start-Sleep -seconds 2

        Start-Process -FilePath 'dotnet' -WorkingDirectory $orderDirectory -ArgumentList "run --debug"
        Start-Sleep -seconds 2

        Start-Process -FilePath 'dotnet' -WorkingDirectory $mvcDirectory -ArgumentList "run --debug"
        Start-Sleep -seconds 5

        start microsoft-edge:http://localhost:3000
    }
    elseif($input -eq "Exit")
    {
        $flag = $false
    }
    else
    {
        Write-Host "Incorrect"
    }
}