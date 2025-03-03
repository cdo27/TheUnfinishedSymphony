using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing3Song : Song
{
    public Wing3Song()
    {
        songID = 004;
        BPM = 166;
        offset = 0f;


        attackBeatsToHit = new List<BeatData>();

        for (float i = 8f; i <= 30f; i++)
        {
            attackBeatsToHit.Add(new BeatData(i, 0));
        }


        // Defend mode beats (each phase has a list of BeatData objects)
        defendBeatsToHit = new List<List<BeatData>>
        {
        };
        songcompleteBeat = 34f;


        attackModeBeats = new List<float> { 0f };
        defendModeBeats = new List<float> { };
    }

    public override void PlaySong(SongManager songManager)
    {
        songManager.playWing2Song();
    }
}
