using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefSong : Song
{
    public ThiefSong()
    {
        songID = 001;
        BPM = 131;
        offset = 0.11f;

        attackBeatsToHit = new List<BeatData>
        {
            // Phase 1
            new BeatData(8f, 0), new BeatData(12f, 0), new BeatData(16f, 0), new BeatData(20f, 0),
            new BeatData(22f, 0), new BeatData(24f, 1), new BeatData(26f, 0), new BeatData(28f, 0),
            new BeatData(30f, 0), new BeatData(32f, 2), new BeatData(34f, 0), new BeatData(38f, 0),
            new BeatData(40f, 1), new BeatData(41f, 1), new BeatData(42f, 2), new BeatData(43f, 2),
            new BeatData(44f, 0), new BeatData(45f, 0), new BeatData(46f, 0), new BeatData(48f, 2),
            new BeatData(50f, 2), new BeatData(52f, 2), new BeatData(54f, 2), new BeatData(56f, 1),
            new BeatData(58f, 1), new BeatData(60f, 1), new BeatData(61f, 0), new BeatData(62f, 0),
            new BeatData(63f, 2),

            // Phase 2
            new BeatData(104.5f, 1), new BeatData(107.5f, 1), new BeatData(111.5f, 1),
            new BeatData(115.5f, 1), new BeatData(116.5f, 1), new BeatData(117f, 1), new BeatData(117.5f, 1),
            new BeatData(119.5f, 2), new BeatData(121.5f, 2), new BeatData(123.5f, 2), new BeatData(124.5f, 2),
            new BeatData(125f, 2), new BeatData(125.5f, 0), new BeatData(127.5f, 0),

            // Phase 3
            new BeatData(170f, 0), new BeatData(171f, 0),
            new BeatData(173f, 1), new BeatData(174f, 1), new BeatData(177f, 2), new BeatData(178f, 2),
            new BeatData(181f, 0), new BeatData(182f, 0), new BeatData(184f, 2), new BeatData(185f, 2),
            new BeatData(186f, 2), new BeatData(188f, 1), new BeatData(189f, 1), new BeatData(191f, 0),

            // Phase 4
            new BeatData(241f, 0), new BeatData(242f, 0), new BeatData(243f, 0), new BeatData(245f, 1),
            new BeatData(247f, 1), new BeatData(249f, 1), new BeatData(251f,2), new BeatData(252f, 2),
            new BeatData(253f, 2), new BeatData(254f, 2), new BeatData(255f, 1), new BeatData(256f, 1),
            new BeatData(257f, 0), new BeatData(258f, 0), new BeatData(259f, 2), new BeatData(259.5f, 2),
            new BeatData(260f, 2), new BeatData(260.5f, 2), new BeatData(261f, 2), new BeatData(261.5f, 2),
            new BeatData(262f, 2), new BeatData(262.5f, 2), new BeatData(263.5f, 2)
        };

        // Defend mode beats (each phase has a list of BeatData objects)
        defendBeatsToHit = new List<List<BeatData>>
        {
            // Phase 1
            new List<BeatData>
            {
                new BeatData(72.5f, 0),  new BeatData(74.5f, 0)
            },
            new List<BeatData>
            {
                new BeatData(80.5f, 1),  new BeatData(82.5f, 1)
            },
            new List<BeatData>
            {
                new BeatData(88.5f, 0), new BeatData(89.5f, 0), new BeatData(90.5f, 0)
            },
            new List<BeatData>
            {
                new BeatData(96.5f, 2), new BeatData(97.5f, 2), new BeatData(98.5f, 2)
            },

            // Phase 2
           new List<BeatData>
            {
                new BeatData(136.5f, 0), new BeatData(137.5f, 0), new BeatData(138.5f, 0)
            },
            new List<BeatData>
            {
                new BeatData(144.5f, 1), new BeatData(145.5f, 1),  new BeatData(146.5f, 1)
            },
            new List<BeatData>
            {
                new BeatData(152.5f, 2),  new BeatData(154.5f, 2)
            },
            new List<BeatData>
            {
                new BeatData(160.5f, 2),  new BeatData(161.5f, 2), new BeatData(162f, 2), new BeatData(162.5f, 2)
            },

            // Phase 3
            new List<BeatData>
            {
                new BeatData(201f, 2), new BeatData(203f, 2)
            },
            new List<BeatData>
            {
                new BeatData(209f, 1), new BeatData(211f, 1)
            },
            new List<BeatData>
            {
                new BeatData(217f, 0), new BeatData(218f, 0), new BeatData(218.5f, 0), new BeatData(219f, 0)
            },
            new List<BeatData>
            {
                new BeatData(225f, 1), new BeatData(226f, 1), new BeatData(226.5f, 1), new BeatData(227f, 1)
            },
            new List<BeatData>
            {
                new BeatData(233f, 0), new BeatData(234f, 0), new BeatData(235f, 0)
            }
        };
        songcompleteBeat = 268f;


        attackModeBeats = new List<float> {4f, 100f, 166f, 237f};
        defendModeBeats = new List<float> {64f, 129f, 193f};
    }

    public override void PlaySong(SongManager songManager)
    {
        songManager.playThiefSong();
    }
}
