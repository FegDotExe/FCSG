# General map
- [Wrapper](#Wrapper)

# Wrapper
A big big class which wraps all the Sprite Objects, helping with general click/draw actions.
## Fields
`private List<SpriteObject> sprites`: A list containing all the sprites contained in this wrapper.  `public SpriteBatch spriteBatch`: The spritebatch which will be used for all the drawing methods.

`LayerGroup leftClick`: A LayerGroup containing Sprites which can be left-clicked.

`LayerGroup middleClick`

`LayerGroup rightClick`

`LayerGroup wheelHover`: A LayerGroup containing Sprites which can be scrolled.

`LayerGroup hover`: A LayerGroup containing Sprites which can be hovered.
## Methods
`NewSprite(...)`: Creates a new sprite and returns it. This automatically sets the wrapper class as the sprite's wrapper.

`Add(SpriteBase sprite)`: Adds a new sprite to the wrapper (also adding it to click lists etc).

`Remove(SpriteBase sprite)`: Removes a sprite from the wrapper (also removing it from click lists etc).

`Draw()`: Calls the `Draw` method for all the sprites contained in the wrapper.

`Click(Clicks click, int x, int y)`: Triggers a click of the specified type for all the Sprites in the wrapper.
# Utilities
# LayerGroup
# ObjectGroup