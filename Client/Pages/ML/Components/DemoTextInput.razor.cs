using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vorlesung.Client.Pages.ML.Components
{
    public partial class DemoTextInput : ComponentBase
    {
        [Parameter]
        public Action<string> OnTextChange { get; set; }

        public DemoTextModel model = new DemoTextModel();

        public List<string> DemoText { get; set; } = new List<string>
        {
            "Hallo, mein Name ist Alexander.",
            "Hello, my name is Alexander.",
            "Bonjour, je m'appelle Alexander.",
            "Hola mi nombre es Alexander."
        };

        public void Analyze()
        {
            if (model != null && !string.IsNullOrEmpty(model.SelectedText))
                OnTextChange?.Invoke(model.SelectedText);
        }
    }

    public class DemoTextModel
    {
        [Required]
        public string SelectedText { get; set; }
    }
}
