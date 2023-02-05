using UnityEngine;

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "ScriptableObjects/SoundLibrary", order = 1)]
public class SoundLibrary : ScriptableObject
{
    private SoundFile[] _soundFiles;
}