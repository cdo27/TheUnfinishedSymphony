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

        cutsceneModeBeats = new List<float> { 0f, 12f };
        attackModeBeats = new List<float> {36f };
        defendModeBeats = new List<float> { 100f };

        attackBeatsToHit = new List<BeatData>
        {
            new BeatData(48f, 0), new BeatData(56f, 1), new BeatData(64f, 2),

            new BeatData(72f, 0), new BeatData(76f, 0), new BeatData(80f, 0), new BeatData(84f, 0),
            new BeatData(88f, 1), new BeatData(90f, 1), new BeatData(92f, 1), new BeatData(94f, 1),
            new BeatData(96f, 2), new BeatData(98f, 2)// First attack mode phase
        };

        // Defend mode beats (each phase has a list of BeatData objects)
        defendBeatsToHit = new List<List<BeatData>>
        {
            new List<BeatData>
            {
                new BeatData(128f, 0), new BeatData(130f, 0), new BeatData(132f, 0), new BeatData(134f, 0) // Defend Phase 1
            },
             new List<BeatData>
            {
                new BeatData(144f, 1), new BeatData(146f, 1), new BeatData(148f, 2), new BeatData(150f, 2) // Defend Phase 1
            },
             new List<BeatData>
            {
                new BeatData(160f, 0), new BeatData(162f, 0), new BeatData(164f, 1), new BeatData(166f, 2) // Defend Phase 1
            },
        };

        /*
        attackBeatsToHit = new List<BeatData>
{
            new BeatData(5f, 0), new BeatData(6f, 0), new BeatData(7f, 0), new BeatData(8f, 0),
            new BeatData(9f, 0), new BeatData(9.25f, 0), new BeatData(9.5f, 0), new BeatData(9.75f, 0),
            new BeatData(10f, 0), new BeatData(11f, 0), new BeatData(12f, 1), new BeatData(13f, 1), new BeatData(14f, 1), // First attack mode phase

            new BeatData(52f, 0), new BeatData(52.5f, 0), new BeatData(53f, 1), new BeatData(53.5f, 1),
            new BeatData(54f, 0), new BeatData(54.5f, 0), new BeatData(57f, 2), new BeatData(58f, 2),
            new BeatData(59f, 2), new BeatData(60f, 2), new BeatData(60.25f, 0), new BeatData(60.5f, 2) // Second attack mode phase
        };

        // Defend mode beats (each phase has a list of BeatData objects)
        defendBeatsToHit = new List<List<BeatData>>
{
            new List<BeatData>
            {
                new BeatData(24f, 0), new BeatData(26f, 1), new BeatData(28f, 1), new BeatData(30f, 2) // Defend Phase 1
            },
            new List<BeatData>
            {
                new BeatData(40f, 0), new BeatData(41f, 0), new BeatData(42f, 1), new BeatData(43f, 1),
                new BeatData(44f, 0), new BeatData(45f, 0), new BeatData(46f, 2) // Defend Phase 2
            }
        };

        songcompleteBeat = 62f;


        attackModeBeats = new List<float> {0f, 48f};
        defendModeBeats = new List<float> {15f};
        */

        songcompleteBeat = 170f;
    }

    public override void PlaySong(SongManager songManager)
    {
        songManager.playTestSong();
    }
}