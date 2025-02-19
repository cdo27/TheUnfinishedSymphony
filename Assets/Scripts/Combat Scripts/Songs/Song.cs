using System.Collections.Generic;
using UnityEngine;

//the template class for all the songs

public class Song : MonoBehaviour
{
    public int songID;
  
    public List<float> attackModeBeats; //list of beats where game switch to attack mode
    public List<float> defendModeBeats; //list of beats where game switch to defend mode
    public List<float> cutsceneModeBeats; //list of beats where game switch to cutscene mode
    public float songcompleteBeat; //the beat where the song is completed

    public List<BeatData> attackBeatsToHit; // List of beats where the notes should be hit, set this manually
    public List<List<BeatData>> defendBeatsToHit; // List of beats where the notes should be hit, set this manually

    //the BPM of the song
    public float BPM;

    //Just in case the music file has a slight delay at the beginning before the music starts, use this to delay the beat counting. Also set manually
    public float offset;

    //function that plays the song, use song manager
    public virtual void PlaySong(SongManager songManager)
    {
    }
}