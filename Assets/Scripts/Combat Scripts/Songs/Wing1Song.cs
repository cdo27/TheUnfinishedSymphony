using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wing1Song : Song
{
    public Wing1Song()
    {
        songID = 002;
        BPM = 162;
        offset = 1.1f;
        enemyName = "Serpentura";
        songName = "The Flight of the Bumble Bee";
        songLength = 85;

        attackBeatsToHit = new List<BeatData>
        {
            // First 16 beats: One note every 4 beats (0)
            new BeatData(8f, 0), new BeatData(12f, 1), new BeatData(16f, 2),
            new BeatData(20f, 0), new BeatData(24f, 2), new BeatData(28f, 1), new BeatData(32f, 0),

            // 
            new BeatData(34f, 0), new BeatData(36f, 1), new BeatData(38f, 1), new BeatData(40f, 2),
            new BeatData(42f, 2), new BeatData(44f, 2), new BeatData(45f, 2), new BeatData(46f, 1), new BeatData(47f, 1), new BeatData(48f, 0),

            // faster
             new BeatData(49f, 0), new BeatData(50f, 2), new BeatData(51f, 2), new BeatData(52f, 1), // 00
            new BeatData(54f, 0), new BeatData(55f, 0), new BeatData(56f, 0), // 11
            
            //introduce 3 hit combo
            new BeatData(60f, 0), new BeatData(61f, 0), new BeatData(61.55f, 0), new BeatData(62f, 0), 
            
            new BeatData(64f, 1), new BeatData(65f, 1), new BeatData(65.55f, 1), new BeatData(66f, 1), 

            // slow down
            new BeatData(68f, 2), new BeatData(70f, 1), new BeatData(72f, 2), new BeatData(74f, 2), 
            
            //3 hit
            new BeatData(76f, 1),  new BeatData(77f, 0), new BeatData(77.5f, 0), new BeatData(78f, 0),  new BeatData(79f, 1),

            new BeatData(82f, 0), new BeatData(83f, 0), new BeatData(84f, 1), new BeatData(85f, 2),
            new BeatData(86f, 2), new BeatData(87f, 1), new BeatData(88f, 0),

            //complex 1
            new BeatData(89.5f, 1), new BeatData(90f, 1), new BeatData(90.5f, 1), 
            new BeatData(93.5f, 2), new BeatData(94f, 2), new BeatData(94.5f, 2), 

            new BeatData(101f, 1), new BeatData(102f, 1), new BeatData(103f, 0), new BeatData(103.5f, 0), new BeatData(104f, 0), 
            new BeatData(105f, 1), new BeatData(106f, 1), new BeatData(107f, 2), new BeatData(107.5f, 2), new BeatData(108f, 2),
            
            //---------------------------------second phase--------------------------------------------
           new BeatData(200.5f, 1), new BeatData(202.5f, 2), new BeatData(204.5f, 1), new BeatData(206.5f, 1), new BeatData(208.5f, 1), new BeatData(210.5f, 1), new BeatData(212.5f, 1), new BeatData(214.5f, 1),

            new BeatData(215.5f, 1), new BeatData(216.5f, 2), new BeatData(217.5f, 0), new BeatData(218.5f, 0), new BeatData(219.5f, 1), new BeatData(220.5f, 1), new BeatData(223f, 1), new BeatData(225.5f, 1),
                    };

        // Defend mode beats (each phase has a list of BeatData objects)
        defendBeatsToHit = new List<List<BeatData>>
        {
           new List<BeatData>
        {
            new BeatData(120.5f, 0), new BeatData(122.5f, 1) // Defend Phase 1
        },
        new List<BeatData>
        {
            new BeatData(128.5f, 1), new BeatData(129.5f, 2),  new BeatData(130.5f, 0) // Defend Phase 2
        },
        new List<BeatData>
        {
            new BeatData(136.5f, 2), new BeatData(137.5f, 1), new BeatData(138f, 1), new BeatData(138.5f, 1) // Defend Phase 3
        },
        new List<BeatData>
        {
            new BeatData(144.5f, 2), new BeatData(145f, 2), new BeatData(145.5f, 2), new BeatData(146.5f, 0) // Defend Phase 4
        },
        new List<BeatData>
        {
            new BeatData(152.5f, 0), new BeatData(153f, 0), new BeatData(153.5f, 0), new BeatData(154f, 0), new BeatData(154.5f, 0) // Defend Phase 5
        },
        new List<BeatData>
        {
            new BeatData(160.5f, 1), new BeatData(161f, 1), new BeatData(161.5f, 1), new BeatData(162f, 1), new BeatData(162.5f, 1) // Defend Phase 6
        },
        new List<BeatData>
        {
            new BeatData(168.5f, 2), new BeatData(169f, 2), new BeatData(169.5f, 2), new BeatData(170f, 1), new BeatData(170.5f, 1) // Defend Phase 7
        },
        new List<BeatData>
        {
            new BeatData(176.5f, 2), new BeatData(176.75f, 2), new BeatData(177.5f, 1), new BeatData(178.5f, 1) // Defend Phase 8
        },
         new List<BeatData>
        {
            new BeatData(184.5f, 2), new BeatData(184.75f, 2), new BeatData(185.5f, 1), new BeatData(185.75f, 1), new BeatData(186.5f, 0) // Defend Phase 8
        },
          new List<BeatData>
        {
            new BeatData(192.5f, 2), new BeatData(192.75f, 2), new BeatData(193.5f, 1), new BeatData(193.75f, 1), new BeatData(194.5f, 0), new BeatData(194.75f, 0), // Defend Phase 8
        }

        };
        songcompleteBeat = 230f;


        attackModeBeats = new List<float> { 4f, 196f };
        defendModeBeats = new List<float> { 109f };
    }

    public override void PlaySong(SongManager songManager)
    {
        songManager.playWing1Song();
    }
}
