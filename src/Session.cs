/*
  This pseudo-singleton is the focal point for the active session's state.
  The Session should be the root of the scene in the game. If it's null, things simply
  won't work.

*/
using Godot;
using System;
using System.Collections.Generic;
using System.Text;


public class Session : Node {
  public static Session session;
  public Node activeMenu;
  public Random random;
  public AudioStreamPlayer jukeBox;
  public float masterVolume, sfxVolume, musicVolume;
  public string userName;
  public float mouseSensitivityX, mouseSensitivityY;
  public DeviceManager.Devices player1Device;
  public Vector2 mousePosition;
  public Vector2 mouseMovement;
  public Sound.Songs currentSong;

  public override void _Ready() {
    EnforceSingleton();
    ChangeMenu(Menu.Menus.Main);
    InitSettings();
  }

  public override void _Input(Godot.InputEvent evt){
    InputEventMouseMotion mot = evt as InputEventMouseMotion;
    if(mot != null){
      mousePosition = mot.GlobalPosition;
      mouseMovement = mot.Relative;
    }
  }

  public override void _Process(float delta){}

  public void InitSettings(){
    SettingsDb db = new SettingsDb();
    masterVolume = Util.ToFloat(db.SelectSetting("master_volume"));
    sfxVolume = Util.ToFloat(db.SelectSetting("sfx_volume"));
    musicVolume = Util.ToFloat(db.SelectSetting("music_volume"));
    userName = db.SelectSetting("username");
    mouseSensitivityX = Util.ToFloat(db.SelectSetting("mouse_sensitivity_x"));
    mouseSensitivityY = Util.ToFloat(db.SelectSetting("mouse_sensitivity_y"));
    player1Device = (DeviceManager.Devices)Util.ToInt(db.SelectSetting("player1_device"));

    Sound.RefreshVolume();
  }

  public static void SaveSettings(){
    SettingsDb db = new SettingsDb();
    
    db.StoreSetting("master_volume", "" + Session.session.masterVolume);
    db.StoreSetting("sfx_volume", "" + Session.session.sfxVolume);
    db.StoreSetting("music_volume", "" + Session.session.musicVolume);
    db.StoreSetting("mouse_sensitivity_x", "" + Session.session.mouseSensitivityX);
    db.StoreSetting("mouse_sensitivity_y", "" + Session.session.mouseSensitivityY);
    db.StoreSetting("username", Session.session.userName);
    db.StoreSetting("player1_device", "" + (int)Session.session.player1Device);
    GD.Print("Saving player device as "  + (int)Session.session.player1Device);
    Sound.RefreshVolume();
  }
  
  public void Quit(){
    GetTree().Quit(); 
  }
  
  public static void InitJukeBox(){
    if(Session.session.jukeBox != null){
      return;
    }

    Session.session.jukeBox = new AudioStreamPlayer();
    Session.session.AddChild(Session.session.jukeBox);
  }
  
  public static System.Random GetRandom(){
    if(Session.session.random != null){
      return Session.session.random;
    }

    Session.session.random = new System.Random();
    return Session.session.random;
  }
  
  /* Remove game nodes/variables in order to return it to a menu. */
  public static void ClearGame(bool keepNet = false){
    Session ses = Session.session;
    Input.SetMouseMode(Input.MouseMode.Visible);
  }
  
  public static void QuitToMainMenu(){
    Session.ChangeMenu(Menu.Menus.Main);
    Session.ClearGame();
  }

  public static void ChangeMenu(Menu.Menus menu){
    Session ses = Session.session;
    if(ses.activeMenu != null){
      IMenu menuInstance = ses.activeMenu as IMenu;
      
      if(menuInstance != null){
        GD.Print("menuInstance.Clear() " + menu);
        menuInstance.Clear();
      }
      else{
        GD.Print("ChangeMenu.QueueFree ses.activeMenu" + menu);
        ses.activeMenu.QueueFree();
      }
      
      ses.activeMenu = null;
    }
    else{
      GD.Print("ChangeMenu: ses.activeMenu already null when setting " + menu);
    }

    Node createdMenu = Menu.MenuFactory(menu);
    if(ses.activeMenu != null){
      GD.Print("Menu Changed menu in its Init().");
      return;
    }
    else{
      ses.activeMenu = createdMenu;
    }

    if(ses.activeMenu == null){
      GD.Print("Session.ChangeMenu: menu null for " + menu);
    }
  }
  
  private void EnforceSingleton(){
    if(Session.session == null){ Session.session = this; }
    else{ this.QueueFree(); }
  }
}