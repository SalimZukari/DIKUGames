using System;
using DIKUArcade.State;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Input;
using DIKUArcade.Timers;


namespace Breakout.BreakoutStates;

public class MainMenu : IGameState {
    private static MainMenu? instance = null;
    private Entity backGroundImage;
    private Entity overLayImage;
    private Text[] menuButtons;
    private int activeMenuButton;
    private int maxMenuButtons;
    private Text newGame;
    private Text quitGame;
    public static MainMenu GetInstance() {
        if (MainMenu.instance == null) {
            MainMenu.instance = new MainMenu();
            MainMenu.instance.ResetState();
        }
        return MainMenu.instance;
    }

    public MainMenu() {
        backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f), 
            new Image(Path.Combine("..","Assets", "Images", "BreakoutTitleScreen.png")));
        overLayImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f), 
                new Image(Path.Combine("..","Assets", "Images", "Overlay.png")));
        activeMenuButton = 0;
        maxMenuButtons = 2;
        menuButtons = new Text[maxMenuButtons];
        
        newGame = new Text("New Game", new Vec2F(0.33f, 0.17f), new Vec2F(0.45f, 0.45f));
        quitGame = new Text( "Quit Game", new Vec2F(0.33f, 0.07f), new Vec2F(0.45f, 0.45f));

        menuButtons[0] = newGame;
        menuButtons[1] = quitGame;
        newGame.SetColor(System.Drawing.Color.Red);
        quitGame.SetColor(System.Drawing.Color.White);

        BreakoutBus.GetBus().RegisterEvent(new GameEvent { 
            EventType = GameEventType.WindowEvent, 
            Message = "MAIN_MENU_SCREEN", 
        });
    }

    public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
        if (action == KeyboardAction.KeyPress && MainMenu.instance != null) {
            KeyPress(key);
        }
    }

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
                if (activeMenuButton == 0) {
                    BreakoutBus.GetBus().RegisterEvent(new GameEvent { 
                        EventType = GameEventType.GameStateEvent, 
                        Message = "CHANGE_STATE",
                        StringArg1 = "GAME_RUNNING"
                    });
                    StaticTimer.RestartTimer();

                } else {
                    BreakoutBus.GetBus().RegisterEvent(new GameEvent { 
                        EventType = GameEventType.WindowEvent, 
                        Message = "Quit_Game", 
                    });
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
    }

    public void ResetState() {
        activeMenuButton = 0;
        newGame.SetColor(System.Drawing.Color.Red);
        quitGame.SetColor(System.Drawing.Color.White);
    }

    public void UpdateState() {
        BreakoutBus.GetBus().ProcessEvents();
    }
}
