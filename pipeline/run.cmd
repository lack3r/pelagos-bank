@echo off
rem Runs the security pipeline without changing your PowerShell policy.
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0run.ps1"
