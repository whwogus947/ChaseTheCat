using UnityEngine;
using UnityEngine.UIElements;

namespace Com2usGameDev
{
    public static class UIToolkitVectorExtensions
    {
        public static void DrawLine(this VisualElement element, Vector2 start, Vector2 end, Color? lineColor = null, float lineWidth = 2f)
        {
            element.generateVisualContent += (MeshGenerationContext context) =>
            {
                var painter = context.painter2D;

                painter.strokeColor = lineColor ?? new Color(0.2f, 0.6f, 0.8f, 1f);
                painter.lineWidth = lineWidth;

                painter.BeginPath();
                painter.MoveTo(start);
                painter.LineTo(end);
                painter.Stroke();
            };
        }

        public static void DrawLine(this VisualElement element, Vector2 start, Vector2 end, float lineWidth, Color lineColor)
        {
            element.DrawLine(start, end, lineColor, lineWidth);
        }

        public static void DrawLine(this VisualElement element, (float x, float y) from, (float x, float y) to)
        {
            var lineContainer = new VisualElement();
            lineContainer.style.width = new Length(1920, LengthUnit.Pixel);
            lineContainer.style.height = new Length(1080, LengthUnit.Pixel);
            lineContainer.DrawLine(
                new Vector2(from.x, from.y),
                new Vector2(to.x, to.y),
                3f,
                Color.red
            );
            lineContainer.style.left = 0;
            lineContainer.style.top = 0;
            lineContainer.style.position = Position.Absolute;

            element.Add(lineContainer);
        }
    }
}
