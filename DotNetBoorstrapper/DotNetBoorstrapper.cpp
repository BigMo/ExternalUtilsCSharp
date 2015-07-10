// DotNetBoorstrapper.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "DotNetBoorstrapper.h"


// This is an example of an exported variable
DOTNETBOORSTRAPPER_API int nDotNetBoorstrapper=0;

// This is an example of an exported function.
DOTNETBOORSTRAPPER_API int fnDotNetBoorstrapper(void)
{
	return 42;
}

// This is the constructor of a class that has been exported.
// see DotNetBoorstrapper.h for the class definition
CDotNetBoorstrapper::CDotNetBoorstrapper()
{
	return;
}
