# Run in PowerShell AFTER: heroku login
# From repo root:  powershell -ExecutionPolicy Bypass -File .\scripts\heroku-deploy.ps1

$ErrorActionPreference = 'Stop'
$App = 'programming-assignemnt-2-mis32'

if (-not (Get-Command heroku -ErrorAction SilentlyContinue)) {
    Write-Error 'Heroku CLI not found. Install from https://devcenter.heroku.com/articles/heroku-cli and reopen PowerShell.'
}

Set-Location (Split-Path $PSScriptRoot)

Write-Host 'Heroku: set container stack...' -ForegroundColor Cyan
heroku stack:set container -a $App

$jwt = [Convert]::ToBase64String((1..48 | ForEach-Object { Get-Random -Maximum 256 } | ForEach-Object { [byte]$_ }))
Write-Host 'Heroku: set config (new Jwt__Secret generated)...' -ForegroundColor Cyan
heroku config:set ASPNETCORE_ENVIRONMENT=Production -a $App
heroku config:set "Jwt__Secret=$jwt" -a $App

Write-Host 'Git: ensure heroku remote...' -ForegroundColor Cyan
$has = git remote | Where-Object { $_ -eq 'heroku' }
if (-not $has) {
    git remote add heroku "https://git.heroku.com/$App.git"
}

Write-Host 'Git: push to Heroku (builds Docker image)...' -ForegroundColor Cyan
git push heroku main

Write-Host 'Done. Open app:' -ForegroundColor Green
heroku open -a $App
Write-Host "Saved Jwt__Secret to Heroku config (not printed). To view: heroku config -a $App" -ForegroundColor DarkGray
