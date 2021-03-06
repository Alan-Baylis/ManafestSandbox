using Godot;
using System;

public class MainMenu : Container, IMenu {
    
    public Godot.Button newGameButton;
    public Godot.Button settingsButton;
    public Godot.Button quitButton;
    public TextEdit background;
    public TexturedButton logo;

    public void Init(float minX, float minY, float maxX, float maxY){
      InitControls();
      ScaleControls();
      GetTree().GetRoot().Connect("size_changed", this, "ScaleControls");
    }
    
    public void Resize(float minX, float minY, float maxX, float maxY){}

    public bool IsSubMenu(){ return false; }

    public void Clear(){
      this.QueueFree();
    }

    public void InitControls(){
      background = Menu.BackgroundBox();
      AddChild(background);

      logo = Menu.TexturedButton("res://Assets/Textures/logo.jpg");
      AddChild(logo);

      newGameButton = Menu.Button(text : "New", onClick : NewGame);
      AddChild(newGameButton);

      quitButton = Menu.Button(text : "Quit", onClick : Quit);
      AddChild(quitButton);
      
      settingsButton = (Godot.Button)Menu.Button(text : "Settings", onClick : Settings);
      AddChild(settingsButton);
      
      Sound.PlayRandomSong(Sound.GetPlaylist(Sound.Playlists.Menu));
    }

    void ScaleControls(){
      Rect2 screen = this.GetViewportRect();
      float width = screen.Size.x;
      float height = screen.Size.y;
      float wu = width/10; // relative height and width units
      float hu = height/10;
      
      Menu.ScaleControl(background, width, height, 0, 0);
      Menu.ScaleControl(logo, 8 * wu, 3 * hu, 2 * wu, 0);
      Menu.ScaleControl(newGameButton, 2 * wu, 2 * hu, 0, 2 * hu);
      Menu.ScaleControl(settingsButton, 2 * wu, 2 * hu, 0, 4 * hu);
      Menu.ScaleControl(quitButton, 2 * wu, 2 * hu, 0, 8 * hu);
    }
    
    public void NewGame(){
      GD.Print("New Game");
    }

    public void Settings(){
      GD.Print("Settings menu");
      Session.ChangeMenu(Menu.Menus.Settings);
    }
    
    public void Quit(){
        Session.session.Quit();
    }    
}
