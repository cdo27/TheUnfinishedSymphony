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
        enemyName = "Dueterno";
        songName = "Ga1ahad and Scientific Witchery";
        songLength = 126;


        attackBeatsToHit = new List<BeatData>
        {
            /////////////////////////////////////phase 1////////////////////////////////////////////////////
            //9 to 24 every beat
            new BeatData(9f, 2), new BeatData(10f, 2), new BeatData(11f, 2), new BeatData(12f, 2), new BeatData(13f, 0), new BeatData(14f, 0), new BeatData(15f, 2), new BeatData(16f, 2), new BeatData(17f, 1),
            new BeatData(18f, 1), new BeatData(19f, 2), new BeatData(20f, 2), new BeatData(21f, 0), new BeatData(22f, 2), new BeatData(23f, 1), new BeatData(24f, 2),

            //more complicated
             new BeatData(25f, 2), new BeatData(27f, 1), new BeatData(27.5f, 1), new BeatData(28f, 2),
             new BeatData(29f, 0), new BeatData(31f, 0), new BeatData(31.5f, 0), new BeatData(32f, 2),
             new BeatData(33f, 1), new BeatData(34f, 1), new BeatData(35f, 0), new BeatData(35.5f, 0), new BeatData(36f, 2),
             new BeatData(37f, 1), new BeatData(38f, 1), new BeatData(39f, 2), new BeatData(39.5f, 2), new BeatData(40f, 0),
             new BeatData(41f, 1), new BeatData(42f, 1),

             /////////////////////////////////////phase 2////////////////////////////////////////////////////
             //start simple
             new BeatData(101f, 2), new BeatData(102f, 2), new BeatData(103f, 1), new BeatData(104f, 1),

             //old patterns
             new BeatData(105f, 2), new BeatData(106f, 1), new BeatData(107f, 0), new BeatData(107.5f, 0), new BeatData(108f, 0),
             new BeatData(109f, 2), new BeatData(110f, 1), new BeatData(111f, 0), new BeatData(111.5f, 0), new BeatData(112f, 0),

             //double hits time (for certain beats
             new BeatData(117f, 1), new BeatData(117.33f, 1), new BeatData(118f, 2),
             new BeatData(119f, 2),  new BeatData(120f, 2),

             new BeatData(121f, 0),  new BeatData(122f, 0),
             new BeatData(123f, 0),  new BeatData(124f, 2),

             new BeatData(125f, 2), new BeatData(125.33f, 2), new BeatData(126f, 2),
             new BeatData(127f, 1), new BeatData(127.33f, 2), new BeatData(128f, 2), new BeatData(128.33f, 1),

             new BeatData(129f, 0), new BeatData(129.33f, 0), new BeatData(130f, 0),
             new BeatData(131f, 1), new BeatData(131.33f, 1), new BeatData(132f, 1),

             new BeatData(133f, 0), new BeatData(133.33f, 0), new BeatData(134f, 0),
             new BeatData(135f, 1), new BeatData(135.33f, 1), new BeatData(136f, 1), new BeatData(136.33f, 1), new BeatData(137f, 2), new BeatData(138f, 2),

            /////////////////////////////////////phase 3////////////////////////////////////////////////////
            new BeatData(181f, 2), new BeatData(182f, 2), new BeatData(183f, 2), new BeatData(184f, 2),

            new BeatData(185f, 2), new BeatData(186f, 2), new BeatData(187f, 0), new BeatData(187.5f, 0), new BeatData(188f, 0),

            new BeatData(189f, 0), new BeatData(189.5f, 0), new BeatData(190f, 0), new BeatData(190.5f, 0),
            new BeatData(191f, 1), new BeatData(191.5f, 1), new BeatData(192f, 1),

            new BeatData(193f, 2), new BeatData(194f, 2), new BeatData(195f, 0), new BeatData(195.5f, 0), new BeatData(196f, 0),

            new BeatData(197f, 2), new BeatData(198f, 2), new BeatData(199f, 1), new BeatData(199.5f, 1), new BeatData(200f, 1),

            new BeatData(201f, 2), new BeatData(202f, 2), new BeatData(203f, 0), new BeatData(203.5f, 0),
            new BeatData(204f, 0), new BeatData(204.5f, 0), new BeatData(205f, 0), new BeatData(206f, 1),
            new BeatData(207f, 1), new BeatData(208f, 1),

            new BeatData(209f, 2), new BeatData(209.33f, 2), new BeatData(210f, 2), new BeatData(210.33f, 2),
            new BeatData(211f, 2), new BeatData(211.33f, 2), new BeatData(212f, 2), new BeatData(212.33f, 2),
            new BeatData(213f, 2), new BeatData(213.33f, 1), new BeatData(214f, 2), new BeatData(214.33f, 1),
            new BeatData(215f, 2), new BeatData(215.33f, 1), new BeatData(216f, 2), new BeatData(216.33f, 1),

            new BeatData(217f, 0), new BeatData(218f, 0),

            /////////////////////////////////////phase 4////////////////////////////////////////////////////
            new BeatData(249f, 0), new BeatData(250f, 2), new BeatData(250.5f, 2), new BeatData(251f, 2), new BeatData(251.5f, 2),
            new BeatData(253f, 0), new BeatData(254f, 0), new BeatData(255.5f, 2), new BeatData(256f, 2), new BeatData(256.5f, 2), new BeatData(257f, 2),
            new BeatData(258f, 0), new BeatData(259f, 0), new BeatData(260f, 2), new BeatData(260.5f, 2), new BeatData(261f, 1), new BeatData(261.5f, 1),
             new BeatData(262f, 0), new BeatData(263f, 0),
            new BeatData(264.5f, 2), new BeatData(265f, 2), new BeatData(265.5f, 1), new BeatData(266f, 1),
            new BeatData(268f, 1), new BeatData(268.5f, 1), new BeatData(269f, 2), new BeatData(269.5f, 2),
            new BeatData(270f, 1), new BeatData(271.5f, 1), new BeatData(272f, 1), new BeatData(272.5f, 2), new BeatData(273f, 2),
            new BeatData(274f, 0), new BeatData(275f, 0),new BeatData(276f, 0),

            new BeatData(277f, 0), new BeatData(278f, 0), new BeatData(279f, 1), new BeatData(280f, 1),

            new BeatData(281f, 0), new BeatData(282f, 2), new BeatData(282.5f, 1), new BeatData(283f, 2),
            new BeatData(284f, 0), new BeatData(285f, 1), new BeatData(285.5f, 2), new BeatData(286f, 1),
            new BeatData(286.5f, 0), new BeatData(287f, 0),

            new BeatData(288f, 0), new BeatData(289f, 2), new BeatData(289.5f, 2), new BeatData(290f, 0),
            new BeatData(291f, 0), new BeatData(292f, 2), new BeatData(292.5f, 2), new BeatData(293f, 2),
            new BeatData(293.5f, 2), new BeatData(294f, 2),

            new BeatData(296f, 2), new BeatData(296.5f, 1), new BeatData(297f, 0), new BeatData(298f, 0),

            new BeatData(299.5f, 0), new BeatData(300f, 1), new BeatData(300.5f, 2), new BeatData(301f, 2),

            new BeatData(303f, 2), new BeatData(304.5f, 0), new BeatData(305f, 0)


        };


        // Defend mode beats (each phase has a list of BeatData objects)
        defendBeatsToHit = new List<List<BeatData>>
        {
            /////////////////////////////////////phase 1////////////////////////////////////////////////////
            new List<BeatData>
            {
                new BeatData(53f, 2), new BeatData(54f, 2), new BeatData(55f, 2)
            },
            new List<BeatData>
            {
                new BeatData(61f, 2), new BeatData(62f, 2), new BeatData(63f, 1)
            },
            new List<BeatData>
            {
                new BeatData(69f, 2), new BeatData(70f, 2), new BeatData(71f, 0), new BeatData(71.33f, 1)
            },
            new List<BeatData>
            {
                new BeatData(77f, 2), new BeatData(78f, 2), new BeatData(79f, 1), new BeatData(79.33f, 0)
            },
            new List<BeatData>
            {
                new BeatData(85f, 1), new BeatData(86f, 2), new BeatData(86.33f, 2), new BeatData(87f, 2),  new BeatData(87.33f, 2),
            },
            new List<BeatData>
            {
                new BeatData(93f, 0), new BeatData(94f, 1), new BeatData(94.33f, 2), new BeatData(95f, 1), new BeatData(95.33f, 2)
            },

            /////////////////////////////////////phase 2////////////////////////////////////////////////////
            new List<BeatData>
            {
                new BeatData(149f, 0), new BeatData(149.5f, 0), new BeatData(150f, 0), new BeatData(151f, 1)
            },
            new List<BeatData>
            {
                new BeatData(157f, 1), new BeatData(157.5f, 1), new BeatData(158f, 1), new BeatData(159f, 2)
            },
            new List<BeatData>
            {
                new BeatData(165f, 1), new BeatData(166f, 2), new BeatData(166.5f, 2), new BeatData(167f, 1)
            },
            new List<BeatData>
            {
                new BeatData(173f, 0), new BeatData(174f, 1), new BeatData(174.5f, 1), new BeatData(175f, 2)
            },

            /////////////////////////////////////phase 3////////////////////////////////////////////////////
           new List<BeatData>
            {
                new BeatData(229f, 0), new BeatData(230f, 0), new BeatData(231f, 2), new BeatData(231.5f, 2)
            },
            new List<BeatData>
            {
                new BeatData(237f, 1), new BeatData(237.5f, 1), new BeatData(238f, 1), new BeatData(238.5f, 0), new BeatData(239f, 0)
            },

            /////////////////////////////////////phase 4////////////////////////////////////////////////////
            new List<BeatData>
            {
                new BeatData(317f, 2), new BeatData(317.5f, 2), new BeatData(318f, 2)
            },
            new List<BeatData>
            {
                new BeatData(325f, 2), new BeatData(325.5f, 2), new BeatData(326f, 2), new BeatData(326.5f, 1),
            },
            new List<BeatData>
            {
                new BeatData(333f, 2),  new BeatData(333.5f, 1), new BeatData(334f, 2),  new BeatData(334.5f, 1), 
            },
            new List<BeatData>
            {
                new BeatData(341f, 0), new BeatData(341.5f, 0), new BeatData(342f, 2), new BeatData(342.5f, 1), 
            },
        };

        songcompleteBeat = 348f;


        attackModeBeats = new List<float> { 4f, 97f, 177f, 241f };
        defendModeBeats = new List<float> { 43f, 139f, 219f, 307f };
    }

    public override void PlaySong(SongManager songManager)
    {
        songManager.playWing3Song();
    }
}
