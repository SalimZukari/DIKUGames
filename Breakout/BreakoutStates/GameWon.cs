using System;
using DIKUArcade.Math;
using DIKUArcade.Input;
using DIKUArcade.State;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;


namespace Breakout.BreakoutStates;
public class GameWon : IGameState {
    /// <summary>
    /// Sets up the game won menu.
    /// </summary>
    private static GameWon? instance = null;
    private Entity backGroundImage;
    private Entity overLayImage;
    private Text[] menuButtons;
    private int activeMenuButton;
    private int maxMenuButtons;
    private Text PlayAgain;
    private Text quitGame;
    private Text YouLost;
    public static GameWon GetInstance() {
        if (GameWon.instance == null) {
            GameWon.instance = new GameWon();
            GameWon.instance.ResetState();
        }
        return GameWon.instance;
    }

    /// <summary>
    /// Menu buttons are defined in the constructor.
    /// </summary>
    public GameWon() {
        backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f), 
            new Image(Path.Combine("..","Assets", "Images", "BreakoutTitleScreen.png")));
        overLayImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f), 
                new Image(Path.Combine("..","Assets", "Images", "Overlay.png")));
        activeMenuButton = 0;
        maxMenuButtons = 2;
        menuButtons = new Text[maxMenuButtons];
        
        PlayAgain = new Text("Play Again", new Vec2F(0.33f, 0.17f), new Vec2F(0.45f, 0.45f));
        quitGame = new Text( "Quit Game", new Vec2F(0.33f, 0.07f), new Vec2F(0.45f, 0.45f));
        YouLost = new Text("You Won!!", new Vec2F(0.27f, 0.10f), new Vec2F(0.7f, 0.7f));

        menuButtons[0] = PlayAgain;
        menuButtons[1] = quitGame;
        
        YouLost.SetFontSize(1);
        PlayAgain.SetColor(System.Drawing.Color.Red);
        quitGame.SetColor(System.Drawing.Color.White);
        YouLost.SetColor(System.Drawing.Color.Purple);

        BreakoutBus.GetBus().RegisterEvent(new GameEvent { 
            EventType = GameEventType.WindowEvent, 
            Message = "GAME_WON_SCREEN", 
        });
    }

    /// <summary>
    /// Handles the appropriate key presses
    /// </summary>
    public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
        if (action == KeyboardAction.KeyPress && GameWon.instance != null) {
            KeyPress(key);
        }
    }

    /// <summary>
    /// Method that says what to do in the event of a given
    /// key press.
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
