using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSong : Song
{
    public TestSong()
    {
        songID = 000;
        BPM = 90;
        offset = 0.1f;
        songLength = 110;

        cutsceneModeBeats = new List<float> { 0f, 12f };
        attackModeBeats = new List<float> {36f };
        defendModeBeats = new List<float> { 84f };

        attackBeatsToHit = new List<BeatData>
        {
            new BeatData(48f, 0), new BeatData(56f, 1), new BeatData(64f, 2),

            new BeatData(72f, 0), new BeatData(74f, 0), new BeatData(76f, 0), new BeatData(78f, 0),
            new BeatData(80f, 1), new BeatData(81f, 1), new BeatData(82f, 2), new BeatData(83f, 2)  // First attack mode phase
        };

        // Defend mode beats (each phase has a list of BeatData objects)
        defendBeatsToHit = new List<List<BeatData>>
        {
            new List<BeatData>
            {
                new BeatData(108f, 0), new BeatData(109f, 0), new BeatData(110f, 0) // Defend Phase 1
            },
             new List<BeatData>
            {
                new BeatData(116f, 0), new BeatData(117f, 0), new BeatData(118f, 0) // Defend Phase 1
            },
             new List<BeatData>
            {
                new BeatData(124f, 1), new BeatData(125f, 1), new BeatData(126f, 1) // Defend Phase 1
            },
             new List<BeatData>
            {
                new BeatData(132f, 1), new BeatData(133f, 1),  new BeatData(134f, 1) // Defend Phase 1
            },
              new List<BeatData>
            {
                new BeatData(140f, 1), new BeatData(141f, 2), new BeatData(141.5f, 2), new BeatData(142f, 2) // Defend Phase 1
            },
               new List<BeatData>
            {
                new BeatData(148f, 1), new BeatData(149f, 2), new BeatData(149.5f, 2), new BeatData(150f, 2) // Defend Phase 1
            },
                new List<BeatData>
            {
                new BeatData(156f, 0), new BeatData(157f, 2), new BeatData(157.5f, 2), new BeatData(158f, 2) // Defend Phase 1
            },
        };

        songcompleteBeat = 164f;
    }

    public override void PlaySong(SongManager songManager)
    {
        songManager.playTestSong();
    }
}