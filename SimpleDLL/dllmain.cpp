// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include <iostream>

#define EXTERN_DLL_EXPORT extern "C" __declspec(dllexport)

struct HelloWorldData
{
	wchar_t Name[64];
	wchar_t Place[64];
};

///Shows a messagebox that displays the passed HelloWorld-struct
EXTERN_DLL_EXPORT int HelloWorld(_In_ LPVOID par)
{
	char msg[1024];
	HelloWorldData* param = (HelloWorldData*)par;
	sprintf_s(msg, "Hello %ls and welcome to %ls!", param->Name, param->Place);
	return MessageBoxA(NULL, msg, "Demo", MB_HELP);
}

///Returns the result of the passed ints
EXTERN_DLL_EXPORT int Add(_In_ LPVOID par)
{
	return *((int*)par) + *((int*)par + 1);
}

///Changes this application's windowtitle to the one passed to this function
EXTERN_DLL_EXPORT int ChangeTitle(_In_ LPVOID par)
{
	HWND wnd = GetForegroundWindow();
	DWORD pid;
	GetWindowThreadProcessId(wnd, &pid);
	if (pid == GetCurrentProcessId())
		return SetWindowText(wnd, (LPCWSTR)par);
	return 0;
}

///Changes this application's windowtitle to the one passed to this function
EXTERN_DLL_EXPORT int RemoveTitle(_In_ LPVOID par)
{
	HWND wnd = GetForegroundWindow();
	DWORD pid;
	GetWindowThreadProcessId(wnd, &pid);
	if (pid == GetCurrentProcessId())
		return SetWindowText(wnd, L"");
	return 0;
}


BOOL APIENTRY DllMain(HMODULE hModule,
	DWORD  ul_reason_for_call,
	LPVOID lpReserved
	)
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}

