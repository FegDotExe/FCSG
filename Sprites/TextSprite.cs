using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace FCSG{
    /// <summary>
    /// An elaborate sprite which contains text.
    /// </summary>
    /// <remarks>
    /// The text is first drawn on a texture, the size of which is determined by originalWidth and originalHeight. The texture is then drawn on the screen. The texture is updated when ElaborateTexture() is called.
    /// </remarks>
    public class TextSprite : SpriteBase{
        SpriteFont font;
        public string text{
            get{
                return _text;
            }
            set{
                _text=value;
                ElaborateTexture(reloadDimension:false);
            }
        }
        private string _text;
        private LinkedVariable<int> originalWidthVariable; //The size of the 2DRenderTarget used to first draw the text on; then the target is used to draw the text on the screen. This is done to save on performance.
        public int originalWidth{
            get{return originalWidthVariable.Get();}
            set{originalWidthVariable.Set(value);}
        }
        private LinkedVariable<int> originalHeightVariable; //The size of the 2DRenderTarget used to first draw the text on; then the target is used to draw the text on the screen. This is done to save on performance.
        public int originalHeight{
            get{return originalHeightVariable.Get();}
            set{originalHeightVariable.Set(value);}
        }
        public enum WrapMode {
            Word,
            Character
        }
        public enum LayoutMode {
            Left,
            Center,
            Right
        }
        public WrapMode wrapMode;
        public LayoutMode layoutMode;
        private int _offsetX; //Offset of the text when Elaborated
        public int offsetX{
            get{
                return _offsetX;
            }
            set{
                _offsetX=value;
                ElaborateTexture(reloadDimension:false,reloadLines:false);
            }
        }
        private int _offsetY; //Offset of the text when Elaborated
        public int offsetY{
            get{
                return _offsetY;
            }
            set{
                _offsetY=value;
                ElaborateTexture(reloadDimension:false,reloadLines:false);
            }
        }
        public SpriteBatchParameters textBatchParameters;
        private RenderTarget2D renderTarget;
        
        /// <summary>
        /// Constructs a new TextSprite.
        /// </summary>
        public TextSprite(
            SpriteParameters spriteParameters
        ):base(
            spriteParameters: spriteParameters
        ){
            this.font = spriteParameters.font;
            
            this._text = spriteParameters.text;// Private reference is used so that the texture isn't elaborated before it can be
            
            if(this.font==null || this._text==null){
                throw new ArgumentException("The font or text of the sprite is null");
            }
            
            if(spriteParameters.originalHeightVariable!=null)
                this.originalHeightVariable = new LinkedVariable<int>(this,spriteParameters.originalHeightVariable);
            else
                this.originalHeightVariable = new LinkedVariable<int>(this, (SpriteBase sprite) => 1000);
            if(spriteParameters.originalWidthVariable!=null)
                this.originalWidthVariable = new LinkedVariable<int>(this,spriteParameters.originalWidthVariable);
            else
                this.originalWidthVariable = new LinkedVariable<int>(this, (SpriteBase sprite) => 1000);

            if(spriteParameters.textBatchParameters!=null)
                this.textBatchParameters = spriteParameters.textBatchParameters;
            else
                this.textBatchParameters = new SpriteBatchParameters();

            this.wrapMode = spriteParameters.wrapMode;
            this.layoutMode = spriteParameters.layoutMode;
            this._offsetX = spriteParameters.offsetX;
            this._offsetY = spriteParameters.offsetY;
            
            ElaborateTexture();
        }

        /// <summary>
        /// Updates the texture of the TextSprite.
        /// </summary>
        /// <param name="reloadDimension">Whether the original dimension should be reloaded or not. Should only be called once the original dimensions are modified.</param>
        /// <param name="reloadLines">Whether the line division should be reloaded or not. Should only be called once the text or the original dimensions are modified.</param>
        private void ElaborateTexture(bool reloadDimension=true,bool reloadLines=true){
            if(reloadDimension){
                renderTarget = new RenderTarget2D(spriteBatch.GraphicsDevice, originalWidthVariable.Get(), originalHeightVariable.Get());
            }

            List<string> lines = new List<string>();

            if(reloadLines){
                lines=toLines(text,wrapMode:this.wrapMode);
            }

            int height=font.LineSpacing;
            int line=0;

            spriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin(this.textBatchParameters);

            foreach(string lineText in lines){
                int x=0;
                switch(layoutMode){
                    case LayoutMode.Left:
                        x=0;
                        break;
                    case LayoutMode.Center:
                        x=(int)((originalWidthVariable.Get()-font.MeasureString(lineText).X)/2);
                        break;
                    case LayoutMode.Right:
                        x=(int)(originalWidthVariable.Get()-font.MeasureString(lineText).X);
                        break;
                }
                spriteBatch.DrawString(font,lineText,new Vector2(x+offsetX,(line*height)+offsetY),Color.White);
                line++;
            }

            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);

            texture=renderTarget;
        }

        /// <summary>
        /// Splits a string into lines, coherently with the given <c>WrapMode</c>.
        /// </summary>
        private List<string> toLines(string text,WrapMode wrapMode=WrapMode.Character){
            List<string> lines = new List<string>();
            if(wrapMode==WrapMode.Character){
                string tempLine = "";
                for(int i=0;i<text.Length;i++){
                    if(font.MeasureString(tempLine+text[i]).X<originalWidth && i!=text.Length-1){
                        tempLine+=text[i];
                    }else{
                        if(i==text.Length-1){
                            tempLine+=text[i];
                        }

                        lines.Add(tempLine);

                        tempLine="";
                        if((char)text[i]!=' '){
                            tempLine+=text[i];
                        }
                    }
                }
            }
            else
            { //If wrapmode is by word
                string[] words=text.Split(' ');

                string tempLine = ""; //Create a temporary line

                foreach (string word in words) {
                    if (font.MeasureString(word).X < originalWidth) //If the word fits in a line by itself, it will either be added to the current line or to the next one
                    {
                        if (tempLine.Length > 0) //Tests if the line already has characters in it, so that it can add a space before
                        {
                            if(font.MeasureString(tempLine+" " + word).X < originalWidth) //If the word fits in the line
                            {
                                tempLine = tempLine + " " + word;
                            }
                            else
                            {
                                lines.Add(tempLine); //Add this line to the completed lines
                                tempLine = word; //Create a new line which only contains this word
                            }
                        }
                        else
                        {
                            tempLine += word;
                        }
                    }
                    else //If the word is larger than a line
                    {
                        if(tempLine.Length > 0) //If the line already contains some characters, a space should be added
                        {
                            tempLine += " ";
                        }

                        foreach(char character in word)
                        {
                            if(font.MeasureString(tempLine+character).X < originalWidth)
                            {
                                tempLine+=character;
                            }
                            else
                            {
                                lines.Add(tempLine);
                                tempLine = character+"";
                            }
                        }
                    }
                }

                lines.Add(tempLine); //Add eventual leftovers
            }

            return lines;
        }
        
        public override void Unlink()
        {
            base.Unlink();
            originalHeightVariable.Unlink();
            originalWidthVariable.Unlink();
        }

        public override void Draw(bool drawMiddle=true){
            BasicDraw(this.spriteBatch,drawMiddle);
        }
        public override void BasicDraw(SpriteBatch spriteBatch, bool drawMiddle = true)
        {
            if(draw){
                if(drawMiddle==true){
                    DrawMiddleTexture();
                }
                spriteBatch.Draw(texture, new Rectangle(this.x,this.y,this.width,this.height),null,color,rotation,origin,effects,(float)depth);
            }
        }
    }
}