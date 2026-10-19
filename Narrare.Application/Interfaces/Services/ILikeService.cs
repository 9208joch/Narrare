using System;
using System.Collections.Generic;
using System.Text;

using Narrare.Domain.Entities;

namespace Narrare.Application.Interfaces.Services;

public interface ILikeService
{
    Task<List<Like>> GetByPostAsync(int postId);

    Task<List<Like>> GetByCommentAsync(int commentId);

    Task<bool> TogglePostLikeAsync(int userId, int postId);

    Task<bool> ToggleCommentLikeAsync(int userId, int commentId);
}
