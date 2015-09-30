@echo off


:: ---- HOVERBOARD --------------------------------------------

call :setVr 0 "Hoverboard Demo (Non-VR)"
call :build BuildBoardKeysNonVr

call :setVr 1 "Hoverboard Demo (VR)"
call :build BuildBoardKeysVr


:: ---- HOVERCAST ---------------------------------------------

call :setVr 0 "Hovercast Demo (Non-VR)"
call :build BuildCastCubesNonVr

call :setVr 1 "Hovercast Demo (VR)"
call :build BuildCastCubesVr


:: ---- CLEANUP -----------------------------------------------

del Unity\ProjectSettings\*.bak

echo %DATE% %TIME% > Builds/Auto/timestamp.txt
timeout /t -1
exit /b


:: ---- FUNCTIONS ---------------------------------------------


:setVr
	if %1 == 1 (
		SET res=2
		SET aspect=0
	) else (
		SET res=1
		SET aspect=1
	)

	SET file="Unity\ProjectSettings\ProjectSettings.asset"

	@echo on
	perl -i.bak -pe 's/productName: .+/productName: %2/' %file%
	perl -i.bak -pe 's/virtualRealitySupported: \d/virtualRealitySupported: %1/' %file%
	@echo off
	perl -i.bak -pe 's/displayResolutionDialog: \d/displayResolutionDialog: %res%/' %file%
	perl -i.bak -pe 's/4:3: \d/4:3: %aspect%/' %file%
	perl -i.bak -pe 's/5:4: \d/5:4: %aspect%/' %file%
	perl -i.bak -pe 's/Others: \d/Others: %aspect%/' %file%
goto :eof



:build
	@echo on
	C:\Zach\Programs\Dev\Unity5\Editor\Unity.exe -quit -batchmode -executeMethod Hover.Unity.Editor.AutomatedBuilds.%~1
	@echo off
goto :eof

