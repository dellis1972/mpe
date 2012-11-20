using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectMercury.TestBench
{
    /// <summary>
    /// A base class to handle input as a game component. A game usually inherits from this and uses the APIs
    /// to produce a more game specific class.
    /// </summary>
    public class InputComponent : GameComponent
    {

        public PlayerIndex CurrentGamePad = PlayerIndex.One;
        public KeyboardState KeyboardState;
        public readonly GamePadState[] GamePadState = new GamePadState[4];
        public MouseState MouseState;

        private KeyboardState _lastKey;
        private readonly GamePadState[] _lastPad = new GamePadState[4];


        public InputComponent(Game game)
            : base(game)
        {
        }


        /// <summary>
        /// Updates the state - handles storing of state from previous frames
        /// </summary>
        /// <param name="gameTime">current GameTime</param>
        public override void Update(GameTime gameTime)
        {
            //Move to next frame, remembering values from last frame
            _lastKey = KeyboardState;
            KeyboardState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            for (int i = 0; i < (int)PlayerIndex.Four; i++)
            {
                _lastPad[i] = GamePadState[i];
                GamePadState[i] = GamePad.GetState((PlayerIndex)i);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Was a key that was up been pressed since the last frame
        /// </summary>
        /// <param name="key">The Key</param>
        /// <returns>True if it was pressed, false otherwise</returns>
        public bool IsKeyPressed(Keys key)
        {
            return (_lastKey.IsKeyUp(key) && KeyboardState.IsKeyDown(key));
        }

        /// <summary>
        /// Has a gamepad button that was up been pressed since the last frame
        /// </summary>
        /// <param name="button">Which gamepad button</param>
        /// <returns>True if it was pressed, false otherwise</returns>
        public bool IsButtonPressed(Buttons button)
        {

                return IsButtonPressed(button, CurrentGamePad);
        }


        public bool IsButtonPressed(Buttons button, PlayerIndex gamePad)
        {
            return (_lastPad[(int)gamePad].IsButtonUp(button) && GamePadState[(int)gamePad].IsButtonDown(button));
        }
    }
}