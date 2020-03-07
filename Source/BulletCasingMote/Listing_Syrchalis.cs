using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace BulletCasingMote
{
    public class Listing_Syrchalis : Listing_Standard
    {
        public void LabelHighlight(string label, float maxHeight = -1f, string tooltip = null)
        {
            float num = Text.CalcHeight(label, base.ColumnWidth);
            bool flag = false;
            if (maxHeight >= 0f && num > maxHeight)
            {
                num = maxHeight;
                flag = true;
            }
            Rect rect = base.GetRect(num);
            if (flag)
            {
                Vector2 labelScrollbarPosition = this.GetLabelScrollbarPosition(this.curX, this.curY);
                Widgets.LabelScrollable(rect, label, ref labelScrollbarPosition, false, true);
                this.SetLabelScrollbarPosition(this.curX, this.curY, labelScrollbarPosition);
            }
            else
            {
                Widgets.Label(rect, label);
            }
            if (!tooltip.NullOrEmpty())
            {
                if (Mouse.IsOver(rect))
                {
                    Widgets.DrawHighlight(rect);
                }
                TooltipHandler.TipRegion(rect, tooltip);
            }
            base.Gap(this.verticalSpacing);
        }
        private Vector2 GetLabelScrollbarPosition(float x, float y)
        {
            if (this.labelScrollbarPositions == null)
            {
                return Vector2.zero;
            }
            for (int i = 0; i < this.labelScrollbarPositions.Count; i++)
            {
                Vector2 first = this.labelScrollbarPositions[i].First;
                if (first.x == x && first.y == y)
                {
                    return this.labelScrollbarPositions[i].Second;
                }
            }
            return Vector2.zero;
        }
        private void SetLabelScrollbarPosition(float x, float y, Vector2 scrollbarPosition)
        {
            if (this.labelScrollbarPositions == null)
            {
                this.labelScrollbarPositions = new List<Pair<Vector2, Vector2>>();
                this.labelScrollbarPositionsSetThisFrame = new List<Vector2>();
            }
            this.labelScrollbarPositionsSetThisFrame.Add(new Vector2(x, y));
            for (int i = 0; i < this.labelScrollbarPositions.Count; i++)
            {
                Vector2 first = this.labelScrollbarPositions[i].First;
                if (first.x == x && first.y == y)
                {
                    this.labelScrollbarPositions[i] = new Pair<Vector2, Vector2>(new Vector2(x, y), scrollbarPosition);
                    return;
                }
            }
            this.labelScrollbarPositions.Add(new Pair<Vector2, Vector2>(new Vector2(x, y), scrollbarPosition));
        }
        private List<Pair<Vector2, Vector2>> labelScrollbarPositions;
        private List<Vector2> labelScrollbarPositionsSetThisFrame;
        private const float DefSelectionLineHeight = 21f;
    }
}
