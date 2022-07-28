# General map
- [Wrapper](#Wrapper)

# Wrapper
A big big class which wraps all the Sprite Objects, helping with general click/draw actions.

**File**: [Sprites/Wrapper.cs](Sprites/Wrapper.cs)
## Fields
### Private fields
`List<SpriteObject> sprites`: A list containing all the sprites contained in this wrapper.  
`SpriteBatch spriteBatch`: The spritebatch which will be used for all the drawing methods.
### Public fields
`LayerGroup leftClick`: A LayerGroup containing Sprites which can be left-clicked.  
`LayerGroup middleClick`: A LayerGroup containing Sprites which can be middle-clicked.  
`LayerGroup rightClick`: A LayerGroup containing Sprites which can be right-clicked.  
`LayerGroup wheelHover`: A LayerGroup containing Sprites which can be scrolled.  
`LayerGroup hover`: A LayerGroup containing Sprites which can be hovered.  
`SpriteBatchParameters spriteBatchParams`: The parameters used when calling [SpriteBatch.Begin](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html) in the [Draw](#Draw).
## Constructors
### Wrapper(SpriteBatch, SpriteBatchParameters)

| Type                    | Name                | Description                                                                                                                                          |
| ----------------------- | ------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------- |
| `SpriteBatch`           | `spriteBatch`       | The [SpriteBatch](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html) which will be used in draw methods                |
| `SpriteBatchParameters` | `spriteBatchParams` | The parameters which will be used in the [SpriteBatch.Begin](https://docs.monogame.net/api/Microsoft.Xna.Framework.Graphics.SpriteBatch.html) method | 

## Methods
### Add(SpriteBase)
Adds the given sprite to the wrapper (also adding it to click lists etc).
*Declaration*:
```cs
public void Add(SpriteBase sprite)
```

| Type         | Name     | Description                                                      |
| ------------ | -------- | ---------------------------------------------------------------- |
| `SpriteBase` | `sprite` | The [SpriteBase](#SpriteBase) which will be added to the Wrapper | 

### Remove(SpriteBase)
Removes the given sprite from the wrapper (also removing it from click lists etc).
*Declaration*:
```cs
public void Remove(SpriteBase sprite)
```

| Type         | Name     | Description                                                          |
| ------------ | -------- | -------------------------------------------------------------------- |
| `SpriteBase` | `sprite` | The [SpriteBase](#SpriteBase) which will be removed from the Wrapper | 


### Draw()
Calls the `Draw` method for all the sprites contained in the wrapper, using this wrapper's `spriteBatchParams`.
*Declaration*:
```cs
public void Draw()
```


### Click(Clicks, x, y)
`Click(Clicks click, int x, int y)`: Triggers a click of the specified type for all the Sprites in the wrapper.

# SpriteBase
The base class for all the sprite classes.
## Fields
## Constructors
## Methods