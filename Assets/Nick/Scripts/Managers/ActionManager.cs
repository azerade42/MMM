using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : Singleton<ActionManager>
{
    public Action MoveNotesHalf;
    public Action<SafeNote> DeleteSafeNote;
    public Action<MalwareNote> DeleteMalwareNote;
}
