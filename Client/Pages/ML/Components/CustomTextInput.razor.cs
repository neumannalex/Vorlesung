using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vorlesung.Client.Pages.ML.Components
{
    public partial class CustomTextInput : ComponentBase
    {
        [Parameter]
        public Action<string> OnTextChange { get; set; }

        public CustomTextModel model = new CustomTextModel();

        public void Analyze()
        {
            if(model != null && !string.IsNullOrEmpty(model.Text))
                OnTextChange?.Invoke(model.Text);
        }
    }

    public class CustomTextModel
    {
        [Required]
        public string Text { get; set; }
    }
}
