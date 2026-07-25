using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace TheSorceressMod.TheSorceressModCode.helpers;

public partial class NSorceryBorder : Control
{
        private NCard? _card;
        private Control? _container;
        private TextureRect? _frame;
        
        public override void _Ready()
        {
            this.MouseFilter = MouseFilterEnum.Ignore;

            if (GetParent() is TextureRect frame)
            {
                _frame = frame;
            }
            
            if (_frame != null && _frame.GetParent() is Control container)
            {
                _container = container;
            }
    
            if (_container != null && _container.GetParent() is NCard card)
            {
                _card = card;
            }
        }
        
        public override void _Process(double delta)
        {
            if (_card == null || _card.Model == null || !_card.Model.Keywords.Contains(SorceressKeywords.Sorcery))
            {
                this.Visible = false;
                return;
            }
            this.Visible = _card.Visible;
        }
}