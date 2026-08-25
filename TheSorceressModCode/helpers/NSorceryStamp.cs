using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace TheSorceressMod.TheSorceressModCode.helpers;

public partial class NSorceryStamp : Control
{
        private NCard? _card;
        private Control? _container;
        private Vector2 _position;
        
        public override void _Ready()
        {
            this.MouseFilter = MouseFilterEnum.Ignore;
    
            if (GetParent() is Control container)
            {
                _container = container;
            }
    
            if (_container != null && _container.GetParent() is NCard card)
            {
                _card = card;
            }

            _position = Position;
        }
        
        public override void _Process(double delta)
        {
            if (_card == null || _card.Model == null || !_card.Model.Keywords.Contains(SorceressKeywords.Sorcery))
            {
                this.Visible = false;
                return;
            }
            this.Visible = _card.Visible;
            if (Visible)
            {
                if (_card.Model.Enchantment != null && _card.EnchantmentTab.Visible)
                    Position = (_card._enchantmentIcon.Position + new Vector2(-12f, 48f));
                else if (_card.Model.Enchantment != null && _card.EnchantmentVfxOverride.Visible)
                    Position = (_card._enchantmentIcon.Position + new Vector2(-12f, 96f));
                else
                {
                    Position = _position;
                }
            }
        }
    
}