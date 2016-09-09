@echo off
@setlocal enableDelayedExpansion
@set _repository_=%~1
@echo.............................................................
@echo.%time% Sync from server ...
@for /f "tokens=1,2,*" %%p in ('hg pull -u') do @(
	@if /i "%%p.%%q"=="pulling.from" (set _repository_=%%~r)
)
@echo.

@ if exist "!_repository_!" (
	@echo.............................................................
	@echo.!time! Push to server !_repository_! ...
	hg push -f "!_repository_!"
	@echo.
	@echo.............................................................
	@echo.!time! Sync to server !_repository_! ...
	pushd "!_repository_!" && hg update && hg pull -u && popd
	@echo.
) else (
	@for /f "tokens=1,2,*" %%n in (.hg\hgrc) do @(
	@if /i "%%~o"=="=" if exist "%%~p" (
		@echo.............................................................
		@echo.!time! Push to configured path %%~p ...
		hg push -f "%%~p"
		@echo.
		@echo.............................................................
		@echo.!time! Sync to configured path %%~p ...
		pushd "%%~p" && hg update && hg pull -u && popd
		@echo.
	))
)

@if ErrorLevel 1 pause
@endlocal