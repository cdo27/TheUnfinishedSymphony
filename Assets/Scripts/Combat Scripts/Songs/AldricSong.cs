using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AldricSong : Song
{
    public AldricSong()
    {
        songID = 005;
        BPM = 156;
        offset = 0.6f;

        attackBeatsToHit = new List<BeatData>
        {
            // New song attack beats
            new BeatData(28f, 1), new BeatData(30f, 1), new BeatData(32f, 0),
            new BeatData(35f, 2), new BeatData(36f, 2), new BeatData(38f, 1), new BeatData(39f, 1),
            new BeatData(40f, 0), new BeatData(43f, 0), new BeatData(44f, 2), new BeatData(47f, 2),
            new BeatData(48f, 1),

            new BeatData(51f, 0), new BeatData(51.5f, 0), new BeatData(52f, 0), new BeatData(54f, 1),

            new BeatData(144f, 0), new BeatData(145f, 0), new BeatData(145.5f, 0), new BeatData(146f, 1),
            new BeatData(147f, 2), new BeatData(148f, 2), new BeatData(149f, 1), new BeatData(150f, 2),
            new BeatData(151f, 0), new BeatData(152f, 0), new BeatData(152.5f, 1), new BeatData(153f, 2),
            new BeatData(154f, 0), new BeatData(158f, 2), new BeatData(159f, 2),
            new BeatData(160f, 1), new BeatData(161f, 2), new BeatData(161.5f, 2), new BeatData(162f, 1),
            new BeatData(163f, 0), new BeatData(164f, 0), new BeatData(165f, 1), new BeatData(166f, 1),
            new BeatData(167f, 1),

            new BeatData(169f, 0), new BeatData(170f, 0), new BeatData(171f, 0), new BeatData(171.25f, 0),
            new BeatData(172f, 0), new BeatData(172.25f, 0), 

            // Repeat 0 and 0.25s from 172 to 181.25
            new BeatData(173f, 0), new BeatData(173.25f, 0), new BeatData(174f, 0), new BeatData(174.25f, 1),
            new BeatData(175f, 0), new BeatData(175.25f, 1), new BeatData(176f, 0), new BeatData(176.25f, 1),
            new BeatData(177f, 0), new BeatData(177.25f, 2), new BeatData(178f, 0), new BeatData(178.25f, 2),
            new BeatData(179f, 0), new BeatData(179.25f, 2), new BeatData(180f, 0), 
            new BeatData(181f, 2), 

            // 182 - 188 (every half beat)
            new BeatData(182f, 0), new BeatData(183f, 0), new BeatData(184f, 1),
            new BeatData(185f, 2), new BeatData(185.5f, 2), new BeatData(186f, 1), new BeatData(186.5f, 2),
            new BeatData(187f, 2), new BeatData(188f, 2),

            new BeatData(190f, 0), new BeatData(191f, 0),

             new BeatData(192f, 0), new BeatData(192.25f, 1), new BeatData(193f, 1), new BeatData(193.25f, 0),
              new BeatData(194f, 0), new BeatData(194.25f, 2), new BeatData(195f, 2), new BeatData(195.25f, 0),
              new BeatData(196f, 0), new BeatData(196.25f, 0),

            new BeatData(236f, 0)
        };


        // Defend mode beats (each phase has a list of BeatData objects)
        defendBeatsToHit = new List<List<BeatData>>
        {
            new List<BeatData>
            {
                new BeatData(16f, 0), new BeatData(17f, 0), new BeatData(18f, 1), new BeatData(19f, 1),
                new BeatData(20f, 2), new BeatData(21f, 1), new BeatData(22f, 0) // Defend Phase 1
            },

           //second set
           new List<BeatData>
            {
                new BeatData(63.5f, 0), new BeatData(64.5f, 2), new BeatData(65.5f, 1), new BeatData(66.5f, 2),
                new BeatData(67.5f, 0), new BeatData(68.5f, 1), new BeatData(69.5f, 2) // Defend Phase 2
            },
            new List<BeatData>
            {
                new BeatData(79.5f, 0), new BeatData(80.5f, 1), new BeatData(81.5f, 1), new BeatData(82.5f, 0),
                new BeatData(83.5f, 2), new BeatData(84.5f, 0), new BeatData(85.5f, 2) // Defend Phase 3
            },
            new List<BeatData>
            {
                new BeatData(95.5f, 0), new BeatData(95.83f, 0), new BeatData(96.16f, 0), new BeatData(96.5f, 0),
                new BeatData(98.5f, 1), new BeatData(99.5f, 2), new BeatData(100.5f, 1), new BeatData(101.5f, 2) // Defend Phase 4
            },
            new List<BeatData>
            {
                new BeatData(111.5f, 0), new BeatData(112.5f, 1), new BeatData(113f, 1), new BeatData(113.5f, 1),
                new BeatData(115.5f, 2), new BeatData(116.5f, 2), new BeatData(117.5f, 2) // Defend Phase 5
            },
            new List<BeatData>
            {
                  new BeatData(127.5f, 0), new BeatData(128.5f, 1), new BeatData(129.25f, 2), new BeatData(129.5f, 2), new BeatData(129.75f, 2),
            new BeatData(131.5f, 0), new BeatData(132.5f, 1), new BeatData(133.5f, 0) // Defend Phase 6
            },

            //third set
            new List<BeatData>
            {
                new BeatData(207.5f, 0), new BeatData(208f, 0), new BeatData(208.5f, 0), new BeatData(209f, 0),
                new BeatData(209.5f, 1), new BeatData(210f, 1), new BeatData(210.5f, 1), new BeatData(211f, 1),
                new BeatData(211.5f, 0), new BeatData(212f, 0), new BeatData(212.5f, 0), new BeatData(213f, 0),
                new BeatData(213.5f, 2), new BeatData(214f, 2), new BeatData(214.5f, 2), new BeatData(215f, 2)
            },
            new List<BeatData>
            {
                new BeatData(223.5f, 0),
                new BeatData(224f, 0), new BeatData(224.5f, 1),
                new BeatData(225f, 1), new BeatData(225.5f, 0),
                new BeatData(226f, 0), new BeatData(226.5f, 2),
                new BeatData(227f, 2), new BeatData(227.5f, 0),
                new BeatData(228f, 0), new BeatData(228.5f, 1),
                new BeatData(229f, 1), new BeatData(229.5f, 0),
                new BeatData(230f, 0), new BeatData(230.5f, 2), new BeatData (231f, 2),
            },
        };
        songcompleteBeat = 242f;


        attackModeBeats = new List<float> { 24f, 139f, 232f };
        defendModeBeats = new List<float> { 5f, 55f, 197f };
    }

    public override void PlaySong(SongManager songManager)
    {
        songManager.playAldricSong();
    }
}
