using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class s_TimelineBinding : MonoBehaviour
{
    public List<GameObject> trackList = new List<GameObject>();
    public PlayableDirector timeline;
    public TimelineAsset timelineAsset;
    public bool autoBindTracks = true;

    // Use this for initialization
    private void Start()
    {
        if (autoBindTracks)
            BindTimelineTracks();
    }

    public void BindTimelineTracks()
    {
        Debug.Log("Binding Timeline Tracks!");
        timelineAsset = (TimelineAsset)timeline.playableAsset;
        // iterate through tracks and map the objects appropriately
        foreach (var playableAsset in timelineAsset.outputs) {
            foreach (var animTrackName in trackList) {

                if (playableAsset.streamName == animTrackName.name)
                {
                    timeline.SetGenericBinding(playableAsset.sourceObject, animTrackName);
                }
            }
        }
        
        /*
        for (var i = 0; i < trackList.Count; i++)
        {
            if (trackList[i] != null)
            {
                var track = (TrackAsset)timelineAsset.outputs[i].sourceObject;
                timeline.SetGenericBinding(track, trackList[i]);
            }
        }
        */
    }
}
