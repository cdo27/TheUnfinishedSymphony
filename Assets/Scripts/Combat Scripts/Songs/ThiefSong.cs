using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefSong : Song
{
    public ThiefSong()
    {
        songID = 001;
        BPM = 131;
        offset = 0.18f;
        enemyName = "Thief Spirit";
        songName = "The Marriage of Figaro, K. 492: Overture";
        songLength = 123;

        attackBeatsToHit = new List<BeatData>
        {
            // Phase 1
            new BeatData(8f, 0), new BeatData(12f, 0), new BeatData(16f, 0), new BeatData(20f, 0),
            new BeatData(22f, 0), new BeatData(24f, 1), new BeatData(26f, 0), new BeatData(28f, 0),
            new BeatData(30f, 0), new BeatData(32f, 2), new BeatData(34f, 0), 
            new BeatData(38f, 0), new BeatData(40.25f, 1), new BeatData(41.25f, 1), new BeatData(42.25f, 2), new BeatData(43.25f, 2),
            new BeatData(44.25f, 0), new BeatData(45.25f, 0), new BeatData(46.25f, 0), 
            new BeatData(48.25f, 2), new BeatData(50.25f, 2), new BeatData(52.25f, 2), new BeatData(54.25f, 2), new BeatData(56.25f, 1),
            new BeatData(58.25f, 1), new BeatData(60.25f, 1), new BeatData(61.25f, 0), new BeatData(62.25f, 0),
            new BeatData(63.25f, 2),

            // Phase 2
            new BeatData(104.5f, 1), new BeatData(108.5f, 1), new BeatData(112.5f, 1),
            
            new BeatData(116.5f, 1), new BeatData(117f, 1), new BeatData(117.5f, 1),
            new BeatData(119.5f, 2), new BeatData(121.5f, 2), new BeatData(123.5f, 2),
            new BeatData(124.5f, 0), new BeatData(125f, 0), new BeatData(125.5f, 0), new BeatData(127.5f, 2),

            // Phase 3
           new BeatData(170.75f, 0), new BeatData(171.75f, 0), new BeatData(172.75f, 0), new BeatData(174.75f, 1),
            new BeatData(175.75f, 2), new BeatData(176.75f, 2), new BeatData(178.5f, 2), new BeatData(180.75f, 0),
            new BeatData(182.55f, 0), new BeatData(183.75f, 2), new BeatData(184.75f, 2), new BeatData(186.5f, 2),
            new BeatData(187.75f, 1), new BeatData(188.75f, 1), new BeatData(190.5f, 0),

            // Phase 4
            new BeatData(241f, 0), new BeatData(242f, 0), new BeatData(243f, 0), new BeatData(245f, 1),
            new BeatData(247f, 1), new BeatData(249f, 1), new BeatData(251f,2),
            new BeatData(253f, 2), new BeatData(255f, 1), new BeatData(256f, 1), new BeatData(257f, 0), 
            new BeatData(259f, 2), new BeatData(260f, 2), new BeatData(261f, 2), new BeatData(262f, 2), new BeatData(263f, 2)
        };

        // Defend mode beats (each phase has a list of BeatData objects)
        defendBeatsToHit = new List<List<BeatData>>
        {
            // Phase 1
           new List<BeatData>
            {
                new BeatData(72.25f, 0), new BeatData(74.25f, 0)
            },
            new List<BeatData>
            {
                new BeatData(80.25f, 1), new BeatData(82.25f, 1)
            },
            new List<BeatData>
            {
                new BeatData(88.25f, 0), new BeatData(89.25f, 0), new BeatData(90.25f, 0)
            },
            new List<BeatData>
            {
                new BeatData(96.25f, 2), new BeatData(97.25f, 2), new BeatData(98.25f, 2)
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
                new BeatData(200.75f, 2), new BeatData(202.75f, 2)
            },
            new List<BeatData>
            {
                new BeatData(208.75f, 1), new BeatData(210.25f, 1), new BeatData(210.75f, 1)
            },
            new List<BeatData>
            {
                new BeatData(216.75f, 0), new BeatData(218.25f, 0), new BeatData(218.75f, 0)
            },
            new List<BeatData>
            {
                new BeatData(224.75f, 1), new BeatData(226.25f, 1), new BeatData(226.75f, 1)
            },
            new List<BeatData>
            {
                new BeatData(232.75f, 0), new BeatData(233.75f, 0), new BeatData(234.75f, 0)
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
