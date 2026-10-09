using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Domain.Entities;

public class SiteStyle
{
    public int Id { get; set; }

    // Rubrik
    public int HeadingFontSize { get; set; }
    public bool HeadingBold { get; set; }
    public string HeadingColor { get; set; } = "#000000";
    public int HeadingMarginBottom { get; set; }

    // Text
    public int TextFontSize { get; set; }
    public string TextColor { get; set; } = "#000000";
    public double TextLineHeight { get; set; }
    public int TextMarginBottom { get; set; }

    // Bild
    public int ImageMaxWidth { get; set; }
    public int ImageBorderRadius { get; set; }

    // Länk
    public string LinkColor { get; set; } = "#0000EE";
    public bool LinkBold { get; set; }
    public bool LinkUnderline { get; set; }
}