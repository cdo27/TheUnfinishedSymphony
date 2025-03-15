using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeSong : Song
{
    public EscapeSong()
    {
        songID = 007;
        BPM = 156;
        offset = 0.5f;
        enemyName = "Aldric & Benedict";
        songName = "Mario & Luigi: Bowser's Inside Story: The Grand Finale Arrangement";
        songLength = 93;


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
                new BeatData(12f, 0), new BeatData(12.33f, 0), new BeatData(12.66f, 0), new BeatData(13.5f, 1), new BeatData(13.83f, 1) // Defend Phase 1
            },
            new List<BeatData>
            {
                new BeatData(20f, 1), new BeatData(20.33f, 2), new BeatData(20.66f, 1),  new BeatData(21.5f, 2), new BeatData(21.83f, 1),  // Defend Phase 1
            },

           //second set
           new List<BeatData> {
                new BeatData(60f, 0), new BeatData(60.33f, 1),
                new BeatData(61f, 0), new BeatData(61.33f, 1),
                new BeatData(62f, 2), new BeatData(62.33f, 2), new BeatData(62.66f, 2),  // Defend Phase 2 
            },

            new List<BeatData> {
                new BeatData(68f, 1), new BeatData(68.33f, 1),  new BeatData(68.66f, 2),
                new BeatData(69f, 2),
                new BeatData(70f, 1)  // Defend Phase 3 
            },

            new List<BeatData> {
                new BeatData(76f, 0), new BeatData(76.33f, 0),
                new BeatData(77f, 2), new BeatData(77.33f, 2),
                new BeatData(78f, 1),  new BeatData(78.33f, 1)  // Defend Phase 4 
            },

            new List<BeatData> {
                new BeatData(84f, 0), new BeatData(84.5f, 1),
                new BeatData(85f, 1), new BeatData(85.5f, 0),
                new BeatData(86f, 2), new BeatData(86.5f, 2)  // Defend Phase 5 
            },

            new List<BeatData> {
                new BeatData(92f, 0), new BeatData(92.25f, 0), new BeatData(92.5f, 0), new BeatData(92.75f, 0),
                new BeatData(94f, 1)  // Defend Phase 6 
            },

            new List<BeatData> {
                new BeatData(100f, 0),
                new BeatData(101f, 1), new BeatData(101.25f, 1), new BeatData(101.5f, 1), new BeatData(101.75f, 1),
                new BeatData(102f, 1)  // Defend Phase 7 
            },

            new List<BeatData> {
                new BeatData(108f, 0),
                new BeatData(109f, 2),
                new BeatData(110f, 1), new BeatData(110.33f, 1), new BeatData(110.66f, 1)  // Defend Phase 8 
            },

            new List<BeatData> {
                new BeatData(116f, 2),  new BeatData(116.33f, 2),
                new BeatData(117f, 1),  new BeatData(117.33f, 1),
                new BeatData(118f, 0),  new BeatData(118.33f, 0),  new BeatData(118.66f, 1),  // Defend Phase 9 
            },

            new List<BeatData> {
                new BeatData(124f, 0), new BeatData(124.33f, 0), new BeatData(124.66f, 0),
                new BeatData(126f, 1), new BeatData(126.33f, 1), new BeatData(126.66f, 1),  // Defend Phase 10 
            },

            new List<BeatData> {
                new BeatData(132f, 0),
                new BeatData(133f, 1), new BeatData(133.25f, 1), new BeatData(133.5f, 1),  new BeatData(133.75f, 1),
                new BeatData(134f, 1) // Defend Phase 11
            },

            //third set
            new List<BeatData> {
                new BeatData(204f, 0),
                new BeatData(204.5f, 0),
                new BeatData(205f, 1),
                new BeatData(205.5f, 0),
                new BeatData(206f, 1),
                new BeatData(206.5f, 0)  // Defend Phase 12
            },

            new List<BeatData> {
                new BeatData(212f, 0),
                new BeatData(212.5f, 1),
                new BeatData(213f, 0),
                new BeatData(213.5f, 1),
                new BeatData(214f, 2),
                new BeatData(214.5f, 2)  // Defend Phase 13
            },

            new List<BeatData> {
                new BeatData(220f, 0),
                new BeatData(220.5f, 0),
                new BeatData(221f, 0),
                new BeatData(221.5f, 0),
                new BeatData(222f, 0),
                new BeatData(222.5f, 0)  // Defend Phase 14
            },

            new List<BeatData> {
                new BeatData(228f, 0),
                new BeatData(228.5f, 2),
                new BeatData(229f, 2),
               
                new BeatData(230f, 0),
                new BeatData(230.5f, 1)  // Defend Phase 15
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
