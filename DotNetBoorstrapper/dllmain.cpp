// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include <metahost.h>

#pragma comment(lib, "mscoree.lib")

#import "mscorlib.tlb" raw_interfaces_only \
	high_property_prefixes("_get", "_put", "_putref") \
	rename("ReportEvent", "InteropServices_ReportEvent")

#define EXTERN_DLL_EXPORT extern "C" __declspec(dllexport)

struct BootStrapData
{
	wchar_t pwzVersion[64];
	wchar_t pwzAssemblyPath[512];
	wchar_t pwzTypeName[128];
	wchar_t pwzMethodName[128];
	wchar_t pwzArgument[256];
};

///Attempts to start the .NET runtime and execute the given managed assembly
///Source: http://www.codeproject.com/Articles/607352/Injecting-Net-Assemblies-Into-Unmanaged-Processes
///Returns the return-value of the executed assembly on success, returns HRESULT of failed operation (low-word) and indentifier (high-word) if failed
EXTERN_DLL_EXPORT int RunManagedAssembly(_In_ LPVOID pArgs)
{
	HRESULT hr;
	BootStrapData *pBootStrapData = (BootStrapData*)pArgs;
	ICLRMetaHost *pMetaHost = NULL;
	ICLRRuntimeInfo *pRuntimeInfo = NULL;
	ICLRRuntimeHost *pClrRuntimeHost = NULL;

	// build runtime
	hr = CLRCreateInstance(CLSID_CLRMetaHost, IID_PPV_ARGS(&pMetaHost));
	if (hr != S_OK)
	{
		return hr + (1 << 16);
	}
	hr = pMetaHost->GetRuntime(pBootStrapData->pwzVersion, IID_PPV_ARGS(&pRuntimeInfo));
	if (hr != S_OK)
	{
		pMetaHost->Release();
		return hr + (2 << 16);
	}
	hr = pRuntimeInfo->GetInterface(CLSID_CLRRuntimeHost,
		IID_PPV_ARGS(&pClrRuntimeHost));
	if (hr != S_OK)
	{
		pMetaHost->Release();
		pRuntimeInfo->Release();
		return hr + (3 << 16);
	}

	// start runtime
	hr = pClrRuntimeHost->Start();
	if (hr != S_OK)
	{
		pMetaHost->Release();
		pRuntimeInfo->Release();
		pClrRuntimeHost->Release();
		return hr + (4 << 16);
	}

	// execute managed assembly
	DWORD pReturnValue;
	hr = pClrRuntimeHost->ExecuteInDefaultAppDomain(
		pBootStrapData->pwzAssemblyPath,
		pBootStrapData->pwzTypeName,
		pBootStrapData->pwzMethodName,
		pBootStrapData->pwzArgument,
		&pReturnValue);

	// free resources
	pMetaHost->Release();
	pRuntimeInfo->Release();
	pClrRuntimeHost->Release();
	
	return pReturnValue;
}

BOOL APIENTRY DllMain( HMODULE hModule,
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

