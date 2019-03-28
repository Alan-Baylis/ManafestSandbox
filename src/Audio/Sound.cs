/* 
  This a centralized storage place for sound effects and music filenames separate
  from business logic.
*/
using Godot;
using System.Collections.Generic;

public class Sound {
  public enum Effects{
    None,
    RifleShot,
    RifleReload,
    FistSwing,
    FistImpact,
    ActorDamage,
    ActorDeath
  };

  public enum Songs{
    None,
    Menu1,
  };

  public enum Playlists{
    None,
    Menu
  };

  public static string SongFile(Songs song){
    string ret = "";
    switch(song){
      case Songs.Menu1: 
        ret = "res://Assets/Audio/Songs/floating_horizons.ogg";
        break;
    }
    return ret;
  }
  
  // TODO: Update these
  public static string EffectFile(Effects effect){
    string ret = "";
    switch(effect){
      case Effects.RifleShot:
        ret = "res://Audio/Effects/pew.wav";
        break;
      case Effects.RifleReload:
        ret = "res://Audio/Effects/chtcht.wav";
        break;
      case Effects.FistSwing:
        ret = "res://Audio/Effects/swing.wav";
        break;
      case Effects.FistImpact:
        ret = "res://Audio/Effects/impact.wav";
        break;
      case Effects.ActorDamage:
        ret = "res://Audio/Effects/actor_damage.wav";
        break;
      case Effects.ActorDeath:
        ret = "res://Audio/Effects/actor_die.wav";
        break;
    }
    return ret;
  }

  public static void RefreshVolume(){
    float musicVolume = Sound.VolumeMath(Session.session.musicVolume);
    Session.session.jukeBox.VolumeDb = musicVolume;
  }

  public static float VolumeMath(float val){
    val *= Session.session.masterVolume;
    val *= 100f;
    
    float remainder = 100f - val; // The distance from 100%
    val = 0f - remainder; // volume should be -distance decibals
    return val;
  }

  public static List<Songs> GetPlaylist(Playlists playlist){
    switch(playlist){
      case Playlists.Menu:
        return new List<Songs>{
          Songs.Menu1
        };
        break;
    }

    return new List<Songs>();
  }

  public static void PlayRandomSong(List<Songs> playlist){
    if(playlist == null || playlist.Count == 0){
      GD.Print("Tried to play bad playlist");
      return;
    }
    int choice = Util.RandInt(0, playlist.Count);
    Sound.PlaySong(playlist[choice]);
  }
  
  public static void PlaySong(Songs song){
    if(song == Songs.None){
      return;
    }
    
    if(song == Session.session.currentSong){
      GD.Print("Don't restart the current song");
      return;
    }
    GD.Print("Changing " + Session.session.currentSong + " to " + song);

    string fileName = SongFile(song);
    AudioStreamOGGVorbis stream = (AudioStreamOGGVorbis)GD.Load(fileName);
    
    Session.InitJukeBox();
    Session.session.jukeBox.Stream = stream;
    Session.session.jukeBox.Playing = true;
    Session.session.currentSong = song;
  }
  
  public static void PauseSong(){
    if(Session.session.jukeBox == null){
      return;
    }
    Session.session.jukeBox.Playing = false;
  }
}