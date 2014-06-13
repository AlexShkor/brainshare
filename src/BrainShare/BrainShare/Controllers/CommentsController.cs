using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using BrainShare.Documents;
using BrainShare.Domain.Documents;
using Brainshare.Infrastructure.Services;
using BrainShare.Services;
using BrainShare.Utils.Extensions;
using BrainShare.ViewModels;
using MongoDB.Bson;

namespace BrainShare.Controllers
{
    [RoutePrefix("comments")]
    public class CommentsController : BaseController
    {
        private readonly CommentsService _comments;

        public CommentsController(CommentsService comments, UsersService users):base(users)
        {
            _comments = comments;
        }

        [POST("load")]
        public ActionResult Load(string id)
        {
            var data = _comments.GetById(id);
            var model = data.Comments.OrderByDescending(x=> x.Timespan).Select(x => new CommentViewModel(x));
            return Json(model);
        }

        public ActionResult AddComment(string id, string content)
        {
            var doc = _comments.GetById(id) ?? new CommentsDocument(){ Id = id};
            var comment = BuildComment(content);
            doc.Comments.Add(comment);
            _comments.Save(doc);
            return Json(new CommentViewModel(comment));
        }

        public ActionResult AddReply(string id, string commentId, string content)
        {
            var doc = _comments.GetById(id) ?? new CommentsDocument() { Id = id };
            var comment = BuildComment(content);
            doc.Comments.Find(x => x.Id == commentId).Replies.Add(comment);
            _comments.Save(doc);
            return Json(new ReplyViewModel(comment));
        }

        private Comment BuildComment(string content)
        {
            var user = _users.GetById(UserId);
            return new Comment
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Content = content,
                Timespan = DateTime.Now,
                User = new UserData(user)
            };
        }
    }

    public class CommentViewModel : ReplyViewModel
    {
        public List<ReplyViewModel> Replies { get; set; }

        public CommentViewModel(Comment comment):base(comment)
        {
            Replies = comment.Replies.OrderBy(x=> x.Timespan).Select(x => new ReplyViewModel(x)).ToList();
        }
    }

    public class ReplyViewModel
    {
        public string Id { get; set; }

        public string Timespan { get; set; }

        public string Content { get; set; }

        public UserDataViewModel User { get; set; }

        public ReplyViewModel(Comment comment)
        {
            Id = comment.Id;
            Timespan = comment.Timespan.ToRelativeDate();
            Content = comment.Content;
            User = new UserDataViewModel(comment.User);
        }
    }
}