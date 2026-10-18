using System;
using System.Collections.Generic;
using System.Text;

namespace Narrare.Application.DTOs;

public class CreateContentReportDto
{
    public int? PostId { get; set; }

    public int? CommentId { get; set; }

    public string Reason { get; set; } = string.Empty;
}