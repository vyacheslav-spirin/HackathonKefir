using UnityEngine;

namespace Speech
{
    public interface ISpeaker
    {
        int CharId { get; }
        Transform SpeechTransform { get; }
    }
}