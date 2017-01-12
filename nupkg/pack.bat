REM "..\tools\gitlink\GitLink.exe" ..\ -u https://github.com/huoshan12345/WebQQWeChat -c release

@ECHO OFF
SET /P VERSION_SUFFIX=Please enter version-suffix (can be left empty): 
dotnet "pack" "..\src\HttpAction" -c "Release" -o "." --version-suffix "%VERSION_SUFFIX%"
pause
