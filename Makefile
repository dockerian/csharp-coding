# NOTE:
#       - Recommend to use Makefile variables, e.g. $@ and $(SOLUTION)
#       - Recommand to use "clean", "release", "debug", and (optionally) "build" targets
#       - Assume top level directory is the source code checkout directory
#         and reserves "debug" and "release" as final target folders (which
#         should not be part of the source; use .hgingore, or svn:ignore in subversion)
#       - The build result will be copied to target folder per the build target
#
SOLUTION=Coding.sln
BUILDDIR=Coding.App\bin

all : debug release
	@echo.%time% :: Build $@ DONE.
	@echo.

release : clean_release
	devenv /useenv /build "$@" "$(SOLUTION)"
	dir /a:d $@ 1>nul 2>nul || mkdir $@
	@echo.
	@echo.%time% :: Copying $@ build target files ...
	xcopy /ifshecrydz $(BUILDDIR)\$@ $@
	@echo.
	@echo.%time% :: Build $@ DONE.
	@echo.

debug : clean_debug
	devenv /useenv /build "$@" "$(SOLUTION)"
	dir /a:d $@ 1>nul 2>nul || mkdir $@
	@echo.
	@echo.%time% :: Copying $@ build target files ...
	xcopy /ifshecrydz $(BUILDDIR)\$@ $@
	@echo.
	@echo.%time% :: Build $@ DONE.
	@echo.

clean : clean_debug clean_release
	@echo.%time% :: All cleaned.
	@echo.

clean_release :
#	@ if exist release (rd /s /q release 1>nul 2>nul)
	@ rd /s /q release 1>nul 2>nul || echo.%time% :: release folder does not exist.
	devenv /useenv /clean "Release" "$(SOLUTION)"
	@echo.%time% :: Release cleaned.
	@echo.

clean_debug :
#	@ if exist debug (rd /s /q debug 1>nul 2>nul)
	@ rd /s /q debug 1>nul 2>nul || echo.%time% :: debug folder does not exist.
	devenv /useenv /clean "Debug" "$(SOLUTION)"
	@echo.%time% :: Debug cleaned.
	@echo.
