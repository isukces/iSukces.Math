@echo off




if "%DEVENVEXE%" == "" goto a

echo DEVENVEXE= "%DEVENVEXE%
 
"%DEVENVEXE%" iSukces.Mathematics.csproj /clean RELEASE
"%DEVENVEXE%" iSukces.Mathematics.csproj /build RELEASE
 
goto end



:a
echo ERROR brak zmiennej DEVENVEXE
goto end

 


:end
pause
 