// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the DOTNETBOORSTRAPPER_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// DOTNETBOORSTRAPPER_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef DOTNETBOORSTRAPPER_EXPORTS
#define DOTNETBOORSTRAPPER_API __declspec(dllexport)
#else
#define DOTNETBOORSTRAPPER_API __declspec(dllimport)
#endif

// This class is exported from the DotNetBoorstrapper.dll
class DOTNETBOORSTRAPPER_API CDotNetBoorstrapper {
public:
	CDotNetBoorstrapper(void);
	// TODO: add your methods here.
};

extern DOTNETBOORSTRAPPER_API int nDotNetBoorstrapper;

DOTNETBOORSTRAPPER_API int fnDotNetBoorstrapper(void);
