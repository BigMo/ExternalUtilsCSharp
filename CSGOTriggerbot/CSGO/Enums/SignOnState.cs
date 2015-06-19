﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot.CSGO.Enums
{
    public enum SignOnState
    {
        SIGNONSTATE_NONE = 0,
        SIGNONSTATE_CHALLENGE = 1,
        SIGNONSTATE_CONNECTED = 2,
        SIGNONSTATE_NEW = 3,
        SIGNONSTATE_PRESPAWN = 4,
        SIGNONSTATE_SPAWN = 5,
        SIGNONSTATE_FULL = 6,
        SIGNONSTATE_CHANGELEVEL = 7
    }
}
