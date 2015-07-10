// SimpleDLL.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "SimpleDLL.h"


// This is an example of an exported variable
SIMPLEDLL_API int nSimpleDLL=0;

// This is an example of an exported function.
SIMPLEDLL_API int fnSimpleDLL(void)
{
	return 42;
}

// This is the constructor of a class that has been exported.
// see SimpleDLL.h for the class definition
CSimpleDLL::CSimpleDLL()
{
	return;
}
