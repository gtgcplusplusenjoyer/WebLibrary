using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Application.Dto
{
    public record CreateBookDto(string Title, string Publisher, string? Description, int TotalCopies =1, int AvailableCopies=1);
}
