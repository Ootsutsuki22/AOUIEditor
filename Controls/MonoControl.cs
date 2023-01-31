using AOUIEditor.ResourceSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AOUIEditor
{
    public class MonoControl : GraphicsDeviceControl
    {
        public static MonoControl Instance { get; private set; }

        public int Resolution;
        public bool XRay;

        GameServiceContainer services;
        ContentManager Content;

        SpriteBatch spriteBatch;

        Texture2D blankTexture;
        Effect tiledEffect;
        Effect simpleEffect;

        float width = 0;
        float height = 0;
        float virtualWidth = 0;
        float virtualHeight = 0;

        Vector2 cameraPos;
        Matrix cameraMatrix;
        float cameraZoomSpeed = 0.1f;
        float cameraZoom = 1f;

        Vector2 mousePos;

        MouseValue mouseState;
        MouseValue prevMouseState;

        Item topMostHoveredItem = null;

        LayerPlacement layerPlacement = new LayerPlacement();

        long time = 0;

        protected override void Initialize()
        {
            services = new GameServiceContainer();
            services.AddService<IGraphicsDeviceService>(Service);
            Content = new ContentManager(services, "Content");

            spriteBatch = new SpriteBatch(GraphicsDevice);

            tiledEffect = Content.Load<Effect>("TiledEffect");
            simpleEffect = Content.Load<Effect>("SimpleEffect");
            blankTexture = new Texture2D(GraphicsDevice, 1, 1);
            blankTexture.SetData<Color>(new Color[] { Color.White });

            cameraPos = new Vector2(ClientSize.Width * 0.5f, ClientSize.Height * 0.5f);

            MouseMove += MonoControl_MouseMove;
            MouseWheel += MonoControl_MouseWheel;
            MouseDown += MonoControl_MouseDown;
            MouseUp += MonoControl_MouseUp;

            Instance = this;

            // Hook the idle event to constantly redraw our animation.
            Application.Idle += delegate { Invalidate(); };
        }

        protected override void Dispose(bool disposing)
        {
            Content?.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            ResetCamera();
        }

        public void ResetCamera()
        {
            cameraZoom = 1f;
            cameraPos = new Vector2(ClientSize.Width * 0.5f, ClientSize.Height * 0.5f);
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.DarkSlateBlue);

            if (Item.rootItem == null || Item.rootItem.children == null)
                return;

            UpdateResolution();
            UpdateCamera();

            DrawScreenRectangle();

            mousePos.X = mouseState.X;
            mousePos.Y = mouseState.Y;
            mousePos = Vector2.Transform(mousePos, Matrix.Invert(cameraMatrix));

            topMostHoveredItem = null;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, cameraMatrix);
            for (int i = 0; i < Item.rootItem.children.Count; i++)
            {
                DrawItem(Item.rootItem.children[i], Vector2.Zero, new Vector2(virtualWidth, virtualHeight));
            }
            spriteBatch.End();

            if (mouseState.LeftPressed == true && prevMouseState.LeftPressed == false)
            {
                TreeForm.SelectItem(topMostHoveredItem);
            }

            DrawBorders();

            prevMouseState = mouseState;

            time += 16;
        }

        private void DrawItem(Item item, Vector2 parentPosition, Vector2 parentSize)
        {
            float posX, posY, sizeX, sizeY;
            CalculatePosition(item.placement.X, parentPosition.X, parentPosition.X + parentSize.X, out posX, out sizeX);
            CalculatePosition(item.placement.Y, parentPosition.Y, parentPosition.Y + parentSize.Y, out posY, out sizeY);

            float widthRatio = item.placement.X.Align == WidgetAlign.WIDGET_ALIGN_LOW_ABS ? 1 : width / virtualWidth;
            float heightRatio = item.placement.Y.Align == WidgetAlign.WIDGET_ALIGN_LOW_ABS ? 1 : height / virtualHeight;

            float realPosX = posX * widthRatio;
            float realPosY = posY * heightRatio;
            float realSizeX = sizeX * widthRatio;
            float realSizeY = sizeY * heightRatio;

            item.posX = realPosX;
            item.posY = realPosY;
            item.sizeX = realSizeX;
            item.sizeY = realSizeY;

            if (mousePos.X > item.posX && mousePos.X < item.posX + item.sizeX
                && mousePos.Y > item.posY && mousePos.Y < item.posY + item.sizeY)
            {
                topMostHoveredItem = item;
            }

            layerPlacement.posX = posX;
            layerPlacement.posY = posY;
            layerPlacement.sizeX = sizeX;
            layerPlacement.sizeY = sizeY;
            layerPlacement.realPosX = realPosX;
            layerPlacement.realPosY = realPosY;
            layerPlacement.realSizeX = realSizeX;
            layerPlacement.realSizeY = realSizeY;
            layerPlacement.widthRatio = widthRatio;
            layerPlacement.heightRatio = heightRatio;

            bool hovered = false;
            bool pushed = false;
            if (mousePos.X > item.posX && mousePos.X < item.posX + item.sizeX
                && mousePos.Y > item.posY && mousePos.Y < item.posY + item.sizeY)
            {
                topMostHoveredItem = item;
                hovered = true;
                if (mouseState.LeftPressed == true)
                {
                    pushed = true;
                }
            }

            if (item.widget.GetType() == typeof(WidgetButton))
            {
                bool disabled = item.widget.Enabled == false;
                if (disabled)
                {
                    DrawLayer(item.stateDisabled, layerPlacement);
                }
                else
                {
                    if (pushed)
                    {
                        if (hovered)
                        {
                            if (item.statePushedHighlighted != null)
                            {
                                DrawLayer(item.statePushedHighlighted, layerPlacement);
                            }
                            else if (item.statePushed != null)
                            {
                                DrawLayer(item.statePushed, layerPlacement);
                            }
                            else
                            {
                                DrawLayer(item.stateNormal, layerPlacement);
                            }
                            DrawLayer(item.layerHighlight, layerPlacement);
                        }
                        else
                        {
                            if (item.statePushed != null)
                            {
                                DrawLayer(item.statePushed, layerPlacement);
                            }
                            else
                            {
                                DrawLayer(item.stateNormal, layerPlacement);
                            }
                        }
                    }
                    else
                    {
                        if (hovered)
                        {
                            if (item.stateHighlighted != null)
                            {
                                DrawLayer(item.stateHighlighted, layerPlacement);
                            }
                            else
                            {
                                DrawLayer(item.stateNormal, layerPlacement);
                            }
                            DrawLayer(item.layerHighlight, layerPlacement);
                        }
                        else
                        {
                            DrawLayer(item.stateNormal, layerPlacement);
                        }
                    }
                }
            }
            else
            {
                DrawLayer(item.backLayer, layerPlacement);
                DrawLayer(item.frontLayer, layerPlacement);
            }

            if (item.children != null)
            {
                for (int i = 0; i < item.children.Count; i++)
                {
                    float mulX = item.placement.X.Align == WidgetAlign.WIDGET_ALIGN_LOW_ABS ? virtualWidth / width : 1;
                    float mulY = item.placement.Y.Align == WidgetAlign.WIDGET_ALIGN_LOW_ABS ? virtualHeight / height : 1;
                    DrawItem(item.children[i], new Vector2(posX * mulX, posY * mulY), new Vector2(sizeX * mulX, sizeY * mulY));
                }
            }
        }

        private void DrawLayer(WidgetLayer widgetLayer, LayerPlacement placement)
        {
            if (widgetLayer == null)
                return;
            if (widgetLayer != null)
            {
                Color color = widgetLayer.XnaColor;
                if (XRay)
                {
                    color = Color.FromNonPremultiplied(color.R, color.G, color.B, color.A / 2);
                }
                if (widgetLayer.GetType() == typeof(WidgetLayerSimpleTexture))
                {
                    WidgetLayerSimpleTexture layer = widgetLayer as WidgetLayerSimpleTexture;
                    BlendState blendState = layer.BlendEffect == BlendEffectType.BLEND_EFFECT_ADD || layer.BlendEffect == BlendEffectType.BLEND_EFFECT_HIGHLIGHT ? BlendState.Additive : BlendState.AlphaBlend;
                    GraphicsDevice.BlendState = blendState;
                    Texture2D layerTexture = null;
                    if (layer.textureItem != null && layer.textureItem.singleTexture != null && layer.textureItem.singleTexture.texture != null && !layer.textureItem.singleTexture.texture.IsDisposed)
                    {
                        layerTexture = layer.textureItem.singleTexture.texture;
                    }
                    if (layerTexture != null)
                    {
                        simpleEffect.CurrentTechnique.Passes[0].Apply();
                        if (layer.Scaling == true)
                        {
                            if (layer.flatPlacement == true)
                            {
                                spriteBatch.Draw(layerTexture, new Rectangle((int)placement.realPosX, (int)placement.realPosY, (int)placement.realSizeX, (int)placement.realSizeY), color);
                            }
                            else
                            {
                                spriteBatch.Draw(layerTexture, new Vector2(placement.realPosX, placement.realPosY), null, color, 0, Vector2.Zero, new Vector2(placement.realSizeX / layerTexture.Width, placement.realSizeY / layerTexture.Height), SpriteEffects.None, 0);
                            }
                        }
                        else
                        {
                            int textureSizeX = Math.Min(layerTexture.Width, (int)placement.sizeX);
                            int textureSizeY = Math.Min(layerTexture.Height, (int)placement.sizeY);
                            if (layer.flatPlacement == true)
                            {
                                spriteBatch.Draw(layerTexture, new Rectangle((int)placement.realPosX, (int)placement.realPosY, (int)(textureSizeX * placement.widthRatio), (int)(textureSizeY * placement.heightRatio)), new Rectangle(0, 0, textureSizeX, textureSizeY), color);
                            }
                            else
                            {
                                spriteBatch.Draw(layerTexture, new Vector2(placement.realPosX, placement.realPosY), new Rectangle(0, 0, textureSizeX, textureSizeY), color, 0, Vector2.Zero, new Vector2(placement.widthRatio, placement.heightRatio), SpriteEffects.None, 0);
                            }
                        }
                    }
                    // Если слой есть, а текстуры в нём нет, то игра рисует просто прямоугольник цветом WidgetLayer.Color
                    else
                    {
                        simpleEffect.CurrentTechnique.Passes[0].Apply();
                        if (layer.flatPlacement == true)
                        {
                            spriteBatch.Draw(blankTexture, new Vector2((int)placement.realPosX, (int)placement.realPosY), null, color, 0, Vector2.Zero, new Vector2((int)placement.realSizeX, (int)placement.realSizeY), SpriteEffects.None, 0);
                        }
                        else
                        {
                            spriteBatch.Draw(blankTexture, new Vector2(placement.realPosX, placement.realPosY), null, color, 0, Vector2.Zero, new Vector2(placement.realSizeX, placement.realSizeY), SpriteEffects.None, 0);
                        }
                    }
                }
                else if (widgetLayer.GetType() == typeof(WidgetLayerTiledTexture))
                {
                    WidgetLayerTiledTexture layer = widgetLayer as WidgetLayerTiledTexture;
                    BlendState blendState = layer.BlendEffect == BlendEffectType.BLEND_EFFECT_ADD || layer.BlendEffect == BlendEffectType.BLEND_EFFECT_HIGHLIGHT ? BlendState.Additive : BlendState.AlphaBlend;
                    GraphicsDevice.BlendState = blendState;
                    Texture2D layerTexture = null;
                    if (layer.textureItem != null && layer.textureItem.singleTexture != null && layer.textureItem.singleTexture.texture != null && !layer.textureItem.singleTexture.texture.IsDisposed)
                    {
                        layerTexture = layer.textureItem.singleTexture.texture;
                    }
                    if (layerTexture != null)
                    {
                        tiledEffect.Parameters["targetSize"].SetValue(new Vector2(placement.sizeX, placement.sizeY));
                        tiledEffect.Parameters["textureSize"].SetValue(new Vector2(layerTexture.Width, layerTexture.Height));
                        tiledEffect.Parameters["padding"].SetValue(new Vector4(layer.Layout.LeftX, layer.Layout.TopY, layer.Layout.RightX, layer.Layout.BottomY));
                        tiledEffect.Parameters["mode"].SetValue(new Vector2((int)(layer.layoutTypeX ?? 0), (int)(layer.layoutTypeY ?? 0)));
                        tiledEffect.CurrentTechnique.Passes[0].Apply();
                        if (layer.flatPlacement == true)
                        {
                            spriteBatch.Draw(layerTexture, new Rectangle((int)placement.realPosX, (int)placement.realPosY, (int)placement.realSizeX, (int)placement.realSizeY), color);
                        }
                        else
                        {
                            spriteBatch.Draw(layerTexture, new Vector2(placement.realPosX, placement.realPosY), null, color, 0, Vector2.Zero, new Vector2(placement.realSizeX / layerTexture.Width, placement.realSizeY / layerTexture.Height), SpriteEffects.None, 0);
                        }
                    }
                    // Если слой есть, а текстуры в нём нет, то игра рисует просто прямоугольник цветом WidgetLayer.Color
                    else
                    {
                        simpleEffect.CurrentTechnique.Passes[0].Apply();
                        if (layer.flatPlacement == true)
                        {
                            spriteBatch.Draw(blankTexture, new Vector2((int)placement.realPosX, (int)placement.realPosY), null, color, 0, Vector2.Zero, new Vector2((int)placement.realSizeX, (int)placement.realSizeY), SpriteEffects.None, 0);
                        }
                        else
                        {
                            spriteBatch.Draw(blankTexture, new Vector2(placement.realPosX, placement.realPosY), null, color, 0, Vector2.Zero, new Vector2(placement.realSizeX, placement.realSizeY), SpriteEffects.None, 0);
                        }
                    }
                }
                else if (widgetLayer.GetType() == typeof(WidgetLayerAnimatedTexture))
                {
                    WidgetLayerAnimatedTexture layer = widgetLayer as WidgetLayerAnimatedTexture;
                    BlendState blendState = layer.BlendEffect == BlendEffectType.BLEND_EFFECT_ADD || layer.BlendEffect == BlendEffectType.BLEND_EFFECT_HIGHLIGHT ? BlendState.Additive : BlendState.AlphaBlend;
                    GraphicsDevice.BlendState = blendState;
                    Texture2D layerTexture = null;
                    if (layer.frames != null && layer.frames.Length > 0
                        && layer.frames[0].rects != null && layer.frames[0].rects.Length > 0
                        && layer.frames[0].textureItem != null &&layer.frames[0].textureItem.singleTexture != null
                        && layer.frames[0].textureItem.singleTexture.texture != null && !layer.frames[0].textureItem.singleTexture.texture.IsDisposed)
                    {
                        layerTexture = layer.frames[0].textureItem.singleTexture.texture;
                    }
                    if (layerTexture != null)
                    {
                        simpleEffect.CurrentTechnique.Passes[0].Apply();

                        int frame = (int)(time / layer.delayMs);
                        int frame2 = frame % layer.frames[0].rects.Length;
                        FrameRect source = layer.frames[0].rects[frame2];
                        Rectangle sourceRecr = new Rectangle(source.offsetX, source.offsetY, source.sizeX, source.sizeY);

                        Rectangle destRect = new Rectangle(
                            (int)(placement.realPosX - source.centerOffsetX * placement.widthRatio),
                            (int)(placement.realPosY - source.centerOffsetY * placement.heightRatio),
                            (int)(source.sizeX * placement.widthRatio),
                            (int)(source.sizeY * placement.heightRatio));

                        spriteBatch.Draw(layerTexture, destRect, sourceRecr, color);
                    }
                    // Если слой есть, а текстуры в нём нет, то игра рисует просто прямоугольник цветом WidgetLayer.Color
                    else
                    {
                        simpleEffect.CurrentTechnique.Passes[0].Apply();
                        if (layer.flatPlacement == true)
                        {
                            spriteBatch.Draw(blankTexture, new Vector2((int)placement.realPosX, (int)placement.realPosY), null, color, 0, Vector2.Zero, new Vector2((int)placement.realSizeX, (int)placement.realSizeY), SpriteEffects.None, 0);
                        }
                        else
                        {
                            spriteBatch.Draw(blankTexture, new Vector2(placement.realPosX, placement.realPosY), null, color, 0, Vector2.Zero, new Vector2(placement.realSizeX, placement.realSizeY), SpriteEffects.None, 0);
                        }
                    }
                }
                else
                {
                    // Неопознанный слой, ситуация с ингейм ссылкой
                }
            }
        }

        private void CalculatePosition(WidgetPlacement placement, float start, float end, out float pos, out float size)
        {
            pos = 0;
            size = 0;

            WidgetAlign pAlign = placement.Align ?? WidgetAlign.WIDGET_ALIGN_LOW;
            float pPos = placement.Pos ?? 0;
            float pHighPos = placement.HighPos ?? 0;
            float pSize = placement.Size ?? 0;

            switch (pAlign)
            {
                case WidgetAlign.WIDGET_ALIGN_LOW:
                    pos = start + pPos;
                    size = pSize;
                    break;
                case WidgetAlign.WIDGET_ALIGN_HIGH:
                    pos = end - pHighPos - pSize;
                    size = pSize;
                    break;
                case WidgetAlign.WIDGET_ALIGN_BOTH:
                    pos = start + pPos;
                    size = end - pHighPos - pos;
                    break;
                case WidgetAlign.WIDGET_ALIGN_CENTER:
                    pos = (start + end) * 0.5f - pSize * 0.5f + pPos;
                    size = pSize;
                    break;
                case WidgetAlign.WIDGET_ALIGN_LOW_ABS:
                    pos = pPos;
                    size = pSize;
                    break;
            }
            // Предотвращаем отрицательные размеры
            if (size < 0)
            {
                pos = pos + size;
                size = -size;
            }
        }

        private void UpdateResolution()
        {
            switch (Resolution)
            {
                case 0:
                    width = GraphicsDevice.PresentationParameters.BackBufferWidth;
                    height = GraphicsDevice.PresentationParameters.BackBufferHeight;
                    break;
                case 1:
                    width = 1280;
                    height = 1024;
                    break;
                case 2:
                    width = 1920;
                    height = 1080;
                    break;
                case 3:
                    width = 2560;
                    height = 1440;
                    break;
                case 4:
                    width = 3840;
                    height = 2160;
                    break;
            }

            if (width <= 0 || height <= 0)
                return;

            float ratio = width / height;
            if (ratio > 1.25f)
            {
                virtualHeight = 1024f;
                virtualWidth = 1024f * ratio;
            }
            else
            {
                virtualWidth = 1280f;
                virtualHeight = 1280f / ratio;
            }
        }

        private void UpdateCamera()
        {
            if (mouseState.Wheel > 0)
                cameraZoom += cameraZoomSpeed;
            if (mouseState.Wheel < 0)
                cameraZoom -= cameraZoomSpeed;
            cameraZoom = Math.Clamp(cameraZoom, 0.2f, 2.5f);
            mouseState.Wheel = 0;

            if (prevMouseState.RightPressed == true)
            {
                cameraPos.X -= (mouseState.X - prevMouseState.X) / cameraZoom;
                cameraPos.Y -= (mouseState.Y - prevMouseState.Y) / cameraZoom;
            }
            if (mouseState.MiddlePressed == true && prevMouseState.MiddlePressed == false)
            {
                ResetCamera();
            }

            cameraMatrix = Matrix.Identity * Matrix.CreateTranslation(new Vector3(-cameraPos.X, -cameraPos.Y, 0)) * Matrix.CreateScale(cameraZoom) * Matrix.CreateTranslation(new Vector3(GraphicsDevice.Viewport.Width * 0.5f, GraphicsDevice.Viewport.Height * 0.5f, 0));
        }

        private void DrawScreenRectangle()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, cameraMatrix);
            Primitives2D.FillRectangle(spriteBatch, Vector2.Zero, new Vector2(width, height), Color.CornflowerBlue);
            spriteBatch.End();
        }

        private void DrawBorders()
        {
            if (Item.selectedItem != null)
            {
                spriteBatch.Begin();

                Vector2 upperLeft;
                Vector2 bottomRight;
                Rectangle rect;

                if (Item.selectedItem.parent != null && Item.selectedItem.parent != Item.rootItem)
                {
                    upperLeft = new Vector2(Item.selectedItem.parent.posX, Item.selectedItem.parent.posY);
                    upperLeft = Vector2.Transform(upperLeft, cameraMatrix);
                    bottomRight = new Vector2(Item.selectedItem.parent.posX + Item.selectedItem.parent.sizeX, Item.selectedItem.parent.posY + Item.selectedItem.parent.sizeY);
                    bottomRight = Vector2.Transform(bottomRight, cameraMatrix);
                    rect = new Rectangle(
                        (int)Math.Floor(upperLeft.X) - 2,
                        (int)Math.Floor(upperLeft.Y) - 3,
                        (int)Math.Ceiling(bottomRight.X - upperLeft.X) + 4,
                        (int)Math.Ceiling(bottomRight.Y - upperLeft.Y) + 4
                        );
                    Primitives2D.DrawRectangle(spriteBatch, rect, Color.Orange, 2f);
                }

                upperLeft = new Vector2(Item.selectedItem.posX, Item.selectedItem.posY);
                upperLeft = Vector2.Transform(upperLeft, cameraMatrix);
                bottomRight = new Vector2(Item.selectedItem.posX + Item.selectedItem.sizeX, Item.selectedItem.posY + Item.selectedItem.sizeY);
                bottomRight = Vector2.Transform(bottomRight, cameraMatrix);
                rect = new Rectangle(
                    (int)Math.Floor(upperLeft.X) - 2,
                    (int)Math.Floor(upperLeft.Y) - 3,
                    (int)Math.Ceiling(bottomRight.X - upperLeft.X) + 4,
                    (int)Math.Ceiling(bottomRight.Y - upperLeft.Y) + 4
                    );
                Primitives2D.DrawRectangle(spriteBatch, rect, Color.Lime, 2f);

                spriteBatch.End();
            }
        }

        private void MonoControl_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    mouseState.LeftPressed = false;
                    break;
                case MouseButtons.Right:
                    mouseState.RightPressed = false;
                    break;
                case MouseButtons.Middle:
                    mouseState.MiddlePressed = false;
                    break;
            }
        }

        private void MonoControl_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    mouseState.LeftPressed = true;
                    break;
                case MouseButtons.Right:
                    mouseState.RightPressed = true;
                    break;
                case MouseButtons.Middle:
                    mouseState.MiddlePressed = true;
                    break;
            }
        }

        private void MonoControl_MouseWheel(object sender, MouseEventArgs e)
        {
            mouseState.Wheel = e.Delta;
        }

        private void MonoControl_MouseMove(object sender, MouseEventArgs e)
        {
            mouseState.X = e.X;
            mouseState.Y = e.Y;
        }
    }
}
