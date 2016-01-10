@echo off
:csv
ucsgflzma.exe ../gamefiles/csv ../patch/gamefiles/csv *.* >nul
IF %ERRORLEVEL% EQU 0 echo CSV Folder Generated
echo.

:sc
if exist ../gamefiles/sc goto scs
echo SC Folder Not Found, No Problem
echo.

:logic
ucsgflzma.exe ../gamefiles/logic ../patch/gamefiles/logic *.* >nul
IF %ERRORLEVEL% EQU 0 echo Logic Folder Generated
echo.

:checkmusic
if exist ../gamefiles/music goto music
echo Music Folder Not Found, No Problem
echo.

:checkfont
if exist ../gamefiles/font goto font
echo Font Folder Not Found, No Problem
echo.

:checkimage
if exist ../gamefiles/image goto image
echo Image Folder Not Found, No Problem
echo.

:checksfx
if exist ../gamefiles/sfx goto sfx
echo SFX Folder Not Found, No Problem
echo.

:checktitan
if exist ../gamefiles/titan goto titan
echo Titan Folder Not Found, No Problem
echo.

:genjson
echo generate fingerprint.json
echo.
ucsbuildsha.exe ../patch/gamefiles ../patch ../gamefiles *.*
IF %ERRORLEVEL% EQU 0 echo fingerprint.json Generated
echo.
echo Finished !!!
pause >nul
echo.
echo By By :)
exit

:music
mkdir "..\patch\gamefiles\music" >nul
copy "..\gamefiles\music" "..\patch\gamefiles\music" >nul
IF %ERRORLEVEL% EQU 0 echo Music Folder Generated
echo.
goto checkfont

:font
mkdir "..\patch\gamefiles\font" >nul
copy "..\gamefiles\font" "..\patch\gamefiles\font" >nul
IF %ERRORLEVEL% EQU 0 echo Font Folder Generated
echo.
goto checkimage

:image
mkdir "..\patch\gamefiles\image" >nul
copy "..\gamefiles\image" "..\patch\gamefiles\image" >nul
IF %ERRORLEVEL% EQU 0 echo Image Folder Generated
echo.
goto checksfx

:sfx
mkdir "..\patch\gamefiles\sfx" >nul
mkdir "..\patch\gamefiles\sfx\ogg" >nul
copy "..\gamefiles\sfx\ogg" "..\patch\gamefiles\sfx\ogg" >nul
IF %ERRORLEVEL% EQU 0 echo SFX Folder Generated
echo.
goto checktitan

:titan
mkdir "..\patch\gamefiles\titan" >nul
mkdir "..\patch\gamefiles\titan\fonts" >nul
copy "..\gamefiles\titan\fonts" "..\patch\gamefiles\titan\fonts" >nul
IF %ERRORLEVEL% EQU 0 echo Titan Folder Generated
echo.
goto genjson

:scs
ucsgflzma.exe ../gamefiles/sc ../patch/gamefiles/sc *.* >nul
IF %ERRORLEVEL% EQU 0 echo SC Folder Generated
echo.
goto logic
