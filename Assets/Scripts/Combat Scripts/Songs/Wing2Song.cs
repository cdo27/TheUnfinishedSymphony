using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wing2Song : Song
{
    public Wing2Song()
    {
        songID = 003;
        BPM = 162;
        offset = 0.46f;
        enemyName = "Sonatine";
        songName = "Beethoven Virus";
        songLength = 105;


        attackBeatsToHit = new List<BeatData>
        {
            new BeatData(9f, 0), new BeatData(13f, 0), new BeatData(17f, 0),
            new BeatData(21f, 0), new BeatData(25f, 0), new BeatData(29.5f, 2), new BeatData(30.5f, 2), new BeatData(31.5f, 2),

            new BeatData(32f, 2), new BeatData(32.5f, 2), new BeatData(33f, 0), new BeatData(33.5f, 0), new BeatData(35.5f, 1), new BeatData(36f, 1), new BeatData(37.5f, 0), new BeatData(38f, 0),

            new BeatData(42f, 0),  new BeatData(43f, 1),  new BeatData(44f, 2),  new BeatData(45f, 1),  new BeatData(46f, 0),

            new BeatData(49.5f, 0),  new BeatData(50f, 0),  new BeatData(50.5f, 0),

            new BeatData(52.5f, 0), new BeatData(53.5f, 1),  new BeatData(54f, 1),  new BeatData(54.5f, 1), new BeatData(56.5f, 0),

            new BeatData(57.5f, 2),  new BeatData(58f, 2),  new BeatData(58.5f, 2), new BeatData(61f, 0), new BeatData(63f, 0),

             new BeatData(65.5f, 0),  new BeatData(66f, 0),  new BeatData(66.5f, 0),  new BeatData(67f, 0),  new BeatData(68f, 0),  new BeatData(69f, 0),  new BeatData(70f, 0), new BeatData(71f, 0),

             new BeatData(74f, 0),  new BeatData(75f, 2),  new BeatData(76f, 1),  new BeatData(77f, 0),  new BeatData(78f, 1), new BeatData(79f, 2),

             new BeatData(82.5f, 0),  new BeatData(83f, 0),  new BeatData(83.5f, 0),

            new BeatData(85.5f, 0),  new BeatData(87f, 1),  new BeatData(87.5f, 1),  new BeatData(88f, 1), new BeatData(90f, 0),  new BeatData(92f, 0),  new BeatData(94f, 0),
            
            
            
            // phase 2
             new BeatData(187f, 0), new BeatData(190f, 0), new BeatData(191f, 2), new BeatData(192f, 1), new BeatData(193f, 0), new BeatData(194f, 1), new BeatData(195f, 2), new BeatData(196f, 0),
            new BeatData(199f, 1), new BeatData(199.5f, 1), new BeatData(200f, 1), new BeatData(202f, 0), new BeatData(203f, 2), new BeatData(203.5f, 2),
            new BeatData(204f, 2), new BeatData(205f, 0), new BeatData(206f, 0), new BeatData(207f, 1),  new BeatData(208f, 1), new BeatData(209f, 2), new BeatData(210f, 2),
            new BeatData(211f, 0), new BeatData(212f, 0),

            //phase 3
            new BeatData(256f, 1), new BeatData(257f, 1), new BeatData(258.5f, 1), new BeatData(259f, 1), new BeatData(260f, 1), new BeatData(261f, 1),

            new BeatData(265.5f, 2), new BeatData(266f, 2), new BeatData(266.5f, 2), new BeatData(268f, 0), new BeatData(269f, 2), new BeatData(269.5f, 2), new BeatData(270f, 2), new BeatData(271f, 1),

            new BeatData(274f, 2), new BeatData(274.5f, 2), new BeatData(275f, 2), new BeatData(276.5f, 1), new BeatData(278.5f, 1), 
            new BeatData(280f, 2), new BeatData(280.25f, 2), new BeatData(280.5f, 2), new BeatData(280.75f, 2), new BeatData(281f, 2),
        };


        // Defend mode beats (each phase has a list of BeatData objects)
        defendBeatsToHit = new List<List<BeatData>>
        {
            //phase 1
           new List<BeatData>
            {
                new BeatData(104.5f, 1), new BeatData(105.5f, 0),  new BeatData(106f, 0), new BeatData(106.5f, 0) // Defend Phase 1
            },
            new List<BeatData>
            {
                new BeatData(112.5f, 0), new BeatData(113f, 0),  new BeatData(113.5f, 2), new BeatData(114f, 2), new BeatData(114.5f, 2) // Defend Phase 2
            },
            new List<BeatData>
            {
                new BeatData(121f, 1), new BeatData(121.33f, 1), new BeatData(123f, 2), new BeatData(123.33f, 2), // Defend Phase 3
            },
            new List<BeatData>
            {
                new BeatData(129f, 1), new BeatData(130f, 0), new BeatData(130.5f, 0), new BeatData(131f, 0) // Defend Phase 4
            },
            new List<BeatData>
            {
                new BeatData(137.5f, 1),     // Beat 1
                new BeatData(139f, 2),     // Combo Start
                new BeatData(139.5f, 2),    // Combo Hit 2 (sub-beat)
                new BeatData(140f, 2)     // Combo Hit 3 (sub-beat)
                // Note: Beat 140.5 is reserved for the turn switch
            },
            new List<BeatData>
            {
                new BeatData(146f, 0), new BeatData(147.5f, 1), new BeatData(148f, 2), new BeatData(148.5f, 1) // Defend Phase 6
            },
            new List<BeatData>
            {
                new BeatData(154f, 0), new BeatData(154.33f, 0), new BeatData(155f, 1), new BeatData(156f, 2) // Defend Phase 7
            },
            new List<BeatData>
            {
                new BeatData(162.5f, 0), new BeatData(163.5f, 1), new BeatData(164f, 1), new BeatData(164.5f, 0) // Defend Phase 8
            },
            new List<BeatData>
            {
                new BeatData(171f, 0), new BeatData(171.5f, 0),  new BeatData(173f, 2), new BeatData(173.5f, 2) // Defend Phase 9
            },
            new List<BeatData>
            {
                new BeatData(179f, 0),  new BeatData(179.5f, 0), new BeatData(180f, 1),  new BeatData(180.5f, 1), new BeatData(181f, 2), new BeatData(181.5f, 2)  // Defend Phase 10
            },

            //phase 2
            new List<BeatData>
            {
                new BeatData(221f, 0), new BeatData(221.33f, 0),  new BeatData(222.5f, 0), new BeatData(223.5f, 1)
            },
              new List<BeatData>
            {
                new BeatData(229f, 0),  new BeatData(230f, 0), new BeatData(230.25f, 0), new BeatData(231.5f, 1)
            },
              new List<BeatData>
            {
                new BeatData(237f, 0),  new BeatData(237.33f, 0), new BeatData(238f, 1), new BeatData(238.33f, 1)
            },
              new List<BeatData>
            {
                new BeatData(245f, 0),  new BeatData(245.33f, 0),  new BeatData(246.33f, 0), new BeatData(246.66f, 1)
            },
        };
        songcompleteBeat = 282f;


        attackModeBeats = new List<float> { 5f, 182f, 248f};
        defendModeBeats = new List<float> { 96f, 213f };
    }

    public override void PlaySong(SongManager songManager)
    {
        songManager.playWing2Song();
    }
}
