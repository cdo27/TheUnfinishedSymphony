using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefSong : Song
{
    public ThiefSong()
    {
        songID = 001;
        BPM = 162;
        offset = 1.1f;

        attackBeatsToHit = new List<BeatData>
        {
            // First 16 beats: One note every 4 beats (0)
            new BeatData(8f, 0), new BeatData(12f, 0), new BeatData(16f, 0),
            new BeatData(20f, 0), new BeatData(24f, 0), new BeatData(28f, 0), new BeatData(32f, 0),

            // Next 8 beats: One note every 2 beats (0)
            new BeatData(34f, 0), new BeatData(36f, 0), new BeatData(38f, 0), new BeatData(40f, 0),
            new BeatData(42f, 0), new BeatData(44f, 0), new BeatData(46f, 0), new BeatData(48f, 0),

            // Introduce variation: 00110101
            new BeatData(50f, 0), new BeatData(52f, 0), // 00
            new BeatData(54f, 1), new BeatData(56f, 1), // 11
            new BeatData(58f, 0), new BeatData(60f, 1), // 10
            new BeatData(62f, 0), new BeatData(64f, 1), // 01

            // Followed by 22120212
            new BeatData(66f, 2), new BeatData(68f, 2), // 22
            new BeatData(70f, 1), new BeatData(72f, 2), // 12
            new BeatData(74f, 0), new BeatData(76f, 2), // 20
            new BeatData(78f, 1), new BeatData(80f, 2), // 12

            new BeatData(82f, 0), new BeatData(83f, 0), new BeatData(84f, 0), new BeatData(85f, 0),
            new BeatData(86f, 0), new BeatData(87f, 0), new BeatData(88f, 0),


            new BeatData(91f, 1), new BeatData(92f, 0), new BeatData(93f, 1),
            new BeatData(94f, 0), new BeatData(95f, 2), new BeatData(96f, 1), new BeatData(101f, 0),
            new BeatData(102f, 0), new BeatData(103f, 0), new BeatData(103.5f, 0), new BeatData(104f, 1), new BeatData(105f, 1),
            new BeatData(106f, 1), new BeatData(107f, 1), new BeatData(107.5f, 1), new BeatData(108f, 2),
            
            //---------------------------------second phase--------------------------------------------
           new BeatData(200.5f, 1), new BeatData(202.5f, 2), new BeatData(204.5f, 1), new BeatData(206.5f, 1), new BeatData(208.5f, 1), new BeatData(210.5f, 1), new BeatData(212.5f, 1), new BeatData(214.5f, 1),

            new BeatData(215.5f, 1), new BeatData(216.5f, 2), new BeatData(217.5f, 0), new BeatData(218.5f, 0), new BeatData(219.5f, 1), new BeatData(220.5f, 1), new BeatData(223f, 1), new BeatData(225.5f, 1),
                    };

        // Defend mode beats (each phase has a list of BeatData objects)
        defendBeatsToHit = new List<List<BeatData>>
        {
           new List<BeatData>
        {
            new BeatData(120.5f, 0), new BeatData(122.5f, 0) // Defend Phase 1
        },
        new List<BeatData>
        {
            new BeatData(128.5f, 1), new BeatData(130.5f, 2) // Defend Phase 2
        },
        new List<BeatData>
        {
            new BeatData(136.5f, 0), new BeatData(138.5f, 1) // Defend Phase 3
        },
        new List<BeatData>
        {
            new BeatData(144.5f, 2), new BeatData(146.5f, 0) // Defend Phase 4
        },
        new List<BeatData>
        {
            new BeatData(152.5f, 0), new BeatData(153.5f, 0), new BeatData(154.5f, 0) // Defend Phase 5
        },
        new List<BeatData>
        {
            new BeatData(160.5f, 0), new BeatData(161.5f, 1), new BeatData(162.5f, 1) // Defend Phase 6
        },
        new List<BeatData>
        {
            new BeatData(168.5f, 1), new BeatData(169.5f, 2), new BeatData(170.5f, 0) // Defend Phase 7
        },
        new List<BeatData>
        {
            new BeatData(176.5f, 0), new BeatData(177.5f, 1), new BeatData(178f, 1), new BeatData(178.5f, 1) // Defend Phase 8
        },
         new List<BeatData>
        {
            new BeatData(184.5f, 2), new BeatData(185f, 2), new BeatData(185.5f, 1), new BeatData(186.5f, 0) // Defend Phase 8
        },
          new List<BeatData>
        {
            new BeatData(192.5f, 0), new BeatData(193f, 0), new BeatData(193.5f, 1), new BeatData(194f, 1), new BeatData(194.5f, 2) // Defend Phase 8
        }

        };
        songcompleteBeat = 230f;


        attackModeBeats = new List<float> { 4f, 196f};
        defendModeBeats = new List<float> { 109f };
    }

    public override void PlaySong(SongManager songManager)
    {
        songManager.playThiefSong();
    }
}
