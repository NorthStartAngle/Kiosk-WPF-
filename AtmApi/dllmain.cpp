// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

BOOL APIENTRY DllMain(HMODULE /*hModule*/, DWORD  ul_reason_for_call, LPVOID /*lpReserved*/) {
	switch (ul_reason_for_call) {
		/*
		DON'T do anything "interesting" in this function without first considering best
		practices for DllMain, as described in a paper found at the following link:

		https://learn.microsoft.com/en-us/windows/win32/dlls/dynamic-link-library-best-practices
		*/

		case DLL_PROCESS_ATTACH:
		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH:
		case DLL_PROCESS_DETACH:
			break;
	}

	return TRUE;
}

