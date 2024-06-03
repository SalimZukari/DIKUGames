using System;
using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Input;


namespace Breakout.BreakoutStates;
public class GameLost : IGameState {
    /// <summary>
    /// Sets up the Game Lost menu
    /// </summary>
    private static GameLost? instance = null;
    private Entity backGroundImage;
    private Entity overLayImage;
    private Text[] menuButtons;
    private int activeMenuButton;
    private int maxMenuButtons;
    private Text PlayAgain;
    private Text quitGame;
    private Text YouLost;
    public static GameLost GetInstance() {
        if (GameLost.instance == null) {
            GameLost.instance = new GameLost();
            GameLost.instance.ResetState();
        }
        return GameLost.instance;
    }

    /// <summary>
    /// The constructor is where the text objects are defined.
    /// </summary>
    public GameLost() {
        backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f), 
            new Image(Path.Combine("..","Assets", "Images", "BreakoutTitleScreen.png")));
        overLayImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f), 
            new Image(Path.Combine("..","Assets", "Images", "Overlay.png")));
        activeMenuButton = 0;
        maxMenuButtons = 2;
        menuButtons = new Text[maxMenuButtons];
        
        PlayAgain = new Text("Play Again", new Vec2F(0.33f, 0.17f), new Vec2F(0.45f, 0.45f));
        quitGame = new Text( "Quit Game", new Vec2F(0.33f, 0.07f), new Vec2F(0.45f, 0.45f));
        YouLost = new Text("You Lost :(", new Vec2F(0.27f, 0.10f), new Vec2F(0.7f, 0.7f));

        menuButtons[0] = PlayAgain;
        menuButtons[1] = quitGame;
    
        
        PlayAgain.SetColor(System.Drawing.Color.Red);
        quitGame.SetColor(System.Drawing.Color.White);
        YouLost.SetColor(System.Drawing.Color.Purple);

        BreakoutBus.GetBus().RegisterEvent(new GameEvent { 
            EventType = GameEventType.WindowEvent, 
            Message = "GAME_LOST_SCREEN", 
        });
    }

    /// <summary>
    /// Made so the event bus registers and manages the event.
    /// </summary>
    public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
        if (action == KeyboardAction.KeyPress && GameLost.instance != null) {
            KeyPress(key);
        }
    }

    /// <summary>
    /// Pressing the up or down keys will change the selected menu button.
    /// </summary>
    public void KeyPress(KeyboardKey key) {
        switch (key) {
            case KeyboardKey.Up:
                if (activeMenuButton > 0) {
                    menuButtons[activeMenuButton].SetColor(System.Drawing.Color.White);
                    activeMenuButton--;
                    menuButtons[activeMenuButton].SetColor(System.Drawing.Color.Red);
                }
                break;
            case KeyboardKey.Down:
                if (activeMenuButton < menuButtons.Length - 1) {
                    menuButtons[activeMenuButton].SetColor(System.Drawing.Color.White);
                    activeMenuButton++;
                    menuButtons[activeMenuButton].SetColor(System.Drawing.Color.Red);
                }
                break;
            case KeyboardKey.Enter:
                switch(activeMenuButton) {
                    case 0: 
                        BreakoutBus.GetBus().RegisterEvent(new GameEvent { 
                            EventType = GameEventType.GameStateEvent, 
                            Message = "CHANGE_STATE",
                            StringArg1 = "GAME_RUNNING"
                        });
                        break;
                    case 1:
                        BreakoutBus.GetBus().RegisterEvent(new GameEvent { 
                            EventType = GameEventType.WindowEvent, 
                            Message = "Quit_Game", 
                        });
                        break;
                }
                break;
            }
    }

    public void RenderState() {
        backGroundImage.RenderEntity();
        overLayImage.RenderEntity();
        foreach (Text menuButton in menuButtons) {
            menuButton.RenderText();
        }
        YouLost.RenderText();
    }

    public void ResetState() {
        activeMenuButton = 0;
        PlayAgain.SetColor(System.Drawing.Color.Red);
        quitGame.SetColor(System.Drawing.Color.White);
    }

    public void UpdateState() {
        BreakoutBus.GetBus().ProcessEvents();
    }
}