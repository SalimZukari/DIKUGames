using System;
using DIKUArcade.Math;
using DIKUArcade.State;
using DIKUArcade.Input;
using DIKUArcade.Timers;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace Breakout.BreakoutStates;
public class GamePaused : IGameState {
    private static GamePaused? instance = null;
        private Text[] menuButtons;
        private Entity backGroundImage;
        private Entity overLayImage;
        private int activeMenuButton;
        private int maxMenuButtons;
        private Text continueGameButton;
        private Text mainMenuButton;
        public static GamePaused GetInstance() {
            if (GamePaused.instance == null) {
                GamePaused.instance = new GamePaused();
                GamePaused.instance.ResetState();
            }
            return GamePaused.instance;
        }

        public GamePaused() {
            activeMenuButton = 0;
            maxMenuButtons = 2;
            menuButtons = new Text[maxMenuButtons];
            continueGameButton = new Text("Continue Game", new Vec2F(0.3f, 0.17f), new Vec2F(0.45f, 0.45f));
            mainMenuButton = new Text("Main Menu", new Vec2F(0.3f, 0.07f), new Vec2F(0.45f, 0.45f));
            backGroundImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f), 
                new Image(Path.Combine("..","Assets", "Images", "BreakoutTitleScreen.png")));
            overLayImage = new Entity(new StationaryShape(0.0f, 0.0f, 1.0f, 1.0f), 
                new Image(Path.Combine("..","Assets", "Images", "Overlay.png")));
            

            menuButtons[0] = continueGameButton;
            menuButtons[1] = mainMenuButton;
            continueGameButton.SetColor(System.Drawing.Color.Red);
            mainMenuButton.SetColor(System.Drawing.Color.White);

            BreakoutBus.GetBus().RegisterEvent(new GameEvent { 
                EventType = GameEventType.WindowEvent, 
                Message = "GAME_PAUSED_SCREEN", 
            });
        }

        public void HandleKeyEvent(KeyboardAction action, KeyboardKey key) {
            if (action == KeyboardAction.KeyPress && GamePaused.instance != null) {
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
                    if (activeMenuButton < maxMenuButtons - 1) {
                        menuButtons[activeMenuButton].SetColor(System.Drawing.Color.White);
                        activeMenuButton++;
                        menuButtons[activeMenuButton].SetColor(System.Drawing.Color.Red);
                    }
                    break;
                case KeyboardKey.Enter:
                    switch (activeMenuButton) {
                        case 0:
                            BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                                EventType = GameEventType.GameStateEvent,
                                Message = "CHANGE_STATE",
                                StringArg1 = "GAME_RUNNING",
                            });
                            StaticTimer.ResumeTimer();
                            break;
                        case 1:
                            BreakoutBus.GetBus().RegisterEvent(new GameEvent {
                                EventType = GameEventType.GameStateEvent,
                                Message = "CHANGE_STATE",
                                StringArg1 = "MAIN_MENU",
                            });
                            GameRunning.GetInstance().NullInstance();
                            break;
                    } 
                    break;
            }
        }

        public void RenderState() {
            backGroundImage.RenderEntity();
            overLayImage.RenderEntity();
            foreach (var button in menuButtons) {
                button.RenderText();
            }
        }

        public void ResetState() {
            activeMenuButton = 0;
            continueGameButton.SetColor(System.Drawing.Color.Red);
            mainMenuButton.SetColor(System.Drawing.Color.White);
        }

        public void UpdateState() {
            BreakoutBus.GetBus().ProcessEvents();
        }
}
